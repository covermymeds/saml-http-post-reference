//-----------------------------------------------------------------------
// <copyright file="AssertionConsumerHttpHandler.cs" company="CoverMyMeds">
//  Copyright (c) 2012 CoverMyMeds.  All rights reserved.
//  This code is presented as reference material only.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Configuration;
using CoverMyMeds.SAML.Library;
using CoverMyMeds.SAML.Library.Schema;

namespace CoverMyMeds.SAML.ServiceProvider
{
    public class AssertionConsumerHttpHandler : IHttpHandler, IRequiresSessionState
    {
        public AssertionConsumerHttpHandler() { }

        public bool IsReusable
        {
            get { return false; }
        }

        private class AssertionData
        {
            public Dictionary<string,string> SAMLAttributes
            public string SessionKey;
            public int UserNID;
            public string Username;
            public string LastName;
            public string FirstName;
            public int OfficeNID;

            public AssertionData(AssertionType assertion)
            {
                AttributeStatementType ast = null;
                foreach (StatementAbstractType sat in assertion.Items)
                {
                    if (sat.GetType().Equals(typeof(AttributeStatementType)))
                    {
                        ast = (AttributeStatementType)sat;
                    }
                }
                if (ast == null) throw new ApplicationException("Invalid SAML Assertion: Missing Attribute Values");
                foreach (AttributeType at in ast.Items)
                {
                    switch (at.Name)
                    {
                        case "SessionKey":
                            if (at.AttributeValue.Length > 0) SessionKey = at.AttributeValue[0].ToString();
                            break;
                        case "UserNID":
                            if (at.AttributeValue.Length > 0) UserNID = int.Parse(at.AttributeValue[0].ToString());
                            break;
                        case "Username":
                            if (at.AttributeValue.Length > 0) Username = at.AttributeValue[0].ToString();
                            break;
                        case "UserLastName":
                            if (at.AttributeValue.Length > 0) LastName = at.AttributeValue[0].ToString();
                            break;
                        case "UserFirstName":
                            if (at.AttributeValue.Length > 0) FirstName = at.AttributeValue[0].ToString();
                            break;
                        case "OfficeNID":
                            if (at.AttributeValue.Length > 0) OfficeNID = int.Parse( at.AttributeValue[0].ToString());
                            break;
                        //case "OfficeName":
                        //    if (at.AttributeValue.Length > 0) SessionKey = at.AttributeValue[0].ToString();
                        //    break;
                        //case "OfficeFax":
                        //    if (at.AttributeValue.Length > 0) SessionKey = at.AttributeValue[0].ToString();
                        //    break;
                    }
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "POST":
                    // Checking to verify that a SAML Assertion is included
                    if (context.Request.Params["SAMLResponse"] == null)
                    {
                        context.Response.Redirect("ServiceProviderError.aspx");
                    }

                    // Checking for the intended resource
                    if (context.Request.Params["RelayState"] == null)
                    {
                        context.Response.Redirect("ServiceProviderError.aspx");
                    }

                    // pull Base 64 encoded XML saml assertion from Request and decode it
                    XmlDocument SAMLXML = new XmlDocument();
                    SAMLXML.LoadXml(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(context.Request.Params["SAMLResponse"].ToString())));

                    // Validate X509 Certificate Signature
                    if (!ValidateX509CertificateSignature(SAMLXML)) context.Response.Redirect("ServiceProviderError.aspx");

                    // Finding 
                    AssertionType assertion = GetAssertionFromXMLDoc(SAMLXML);
                    if (assertion.Issuer.Value == ConfigurationManager.AppSettings["CertIssuer"])
                    {
                        AssertionData NNData = new AssertionData(assertion);
                    }

                    context.Response.Redirect(context.Request.Params["RelayState"].ToString());

                    break;
            }
        }

        private AssertionType GetAssertionFromXMLDoc(XmlDocument SAMLXML)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(SAMLXML.NameTable);
            ns.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlElement xeAssertion = SAMLXML.DocumentElement.SelectSingleNode("saml:Assertion", ns) as XmlElement;

            XmlSerializer serializer = new XmlSerializer(typeof(AssertionType));

            MemoryStream ms = new MemoryStream();

            AssertionType assertion = (AssertionType)serializer.Deserialize(ms);

            return assertion;
        }

        private bool ValidateX509CertificateSignature(XmlDocument SAMLResponse)
        {
            XmlNodeList XMLSignatures = SAMLResponse.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");

            // Checking If the Response or the Assertion has been signed once and only once.
            if (XMLSignatures.Count != 1) return false;
            SignedXml SignedSAML = new SignedXml(SAMLResponse);
            SignedSAML.LoadXml((XmlElement)XMLSignatures[0]);

            // Get X509 Certificate from Cert Store
            X509Certificate2 SigningCert = CertificateUtility.GetCertificateForSigning("DodgeDerek", StoreName.Root, StoreLocation.LocalMachine);

            return SignedSAML.CheckSignature(SigningCert, true);
        }

    }
}