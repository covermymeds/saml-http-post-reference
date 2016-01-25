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
            public Dictionary<string, string> SAMLAttributes;

            public AssertionData(AssertionType assertion)
            {
                // Find the attribute statement within the assertion
                AttributeStatementType ast = null;
                foreach (StatementAbstractType sat in assertion.Items)
                {
                    if (sat.GetType().Equals(typeof(AttributeStatementType)))
                    {
                        ast = (AttributeStatementType)sat;
                    }
                }

                if (ast == null)
                {
                    throw new ApplicationException("Invalid SAML Assertion: Missing Attribute Values");
                }

                SAMLAttributes = new Dictionary<string, string>();

                // Do what needs to be done to pull specific attributes out for sending on
                // For now assuming this is a simple list of string key and string values
                foreach (AttributeType at in ast.Items)
                {
                    SAMLAttributes.Add(at.Name, at.AttributeValue.ToString());
                    //switch (at.Name)
                    //{
                    //    case "UserID":
                    //        if (at.AttributeValue.Length > 0) UserID = at.AttributeValue[0].ToString();
                    //        break;
                    //    case "UserFirstName":
                    //        if (at.AttributeValue.Length > 0) UserFirstName = int.Parse(at.AttributeValue[0].ToString());
                    //        break;
                    //    case "UserLastName":
                    //        if (at.AttributeValue.Length > 0) UserLastName = at.AttributeValue[0].ToString();
                    //        break;
                    //    case "UserDisplayName":
                    //        if (at.AttributeValue.Length > 0) UserDisplayName = at.AttributeValue[0].ToString();
                    //        break;
                    //    case "UserEmail":
                    //        if (at.AttributeValue.Length > 0) UserEmail = at.AttributeValue[0].ToString();
                    //        break;
                    //    case "GroupID":
                    //        if (at.AttributeValue.Length > 0) GroupID = int.Parse(at.AttributeValue[0].ToString());
                    //        break;
                    //}
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
                    String SAMLResponseString = System.Text.Encoding.UTF8.GetString(
                        Convert.FromBase64String(context.Request.Params["SAMLResponse"].ToString()));
                    SAMLXML.LoadXml(SAMLResponseString);

                    // Validate X509 Certificate Signature
                    if (!ValidateX509CertificateSignature(SAMLXML))
                    {
                        context.Response.Redirect("ServiceProviderError.aspx");
                    }

                    // Finding 
                    AssertionType assertion = GetAssertionFromXMLDoc(SAMLXML);
                    if (assertion.Issuer.Value == ConfigurationManager.AppSettings["CertIssuer"])
                    {
                        AssertionData SSOData = new AssertionData(assertion);
                    }

                    // At this point any specific work that needs to be done to establish user context with
                    // the SSOData should be executed before redirecting the user browser to the target
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

            AssertionType assertion = (AssertionType)serializer.Deserialize(new XmlNodeReader(xeAssertion));

            return assertion;
        }

        private bool ValidateX509CertificateSignature(XmlDocument SAMLResponse)
        {
            XmlNodeList XMLSignatures = SAMLResponse.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");

            // Checking If the Response or the Assertion has been signed once and only once.
            if (XMLSignatures.Count != 1) return false;
            SignedXml SignedSAML = new SignedXml(SAMLResponse);
            SignedSAML.LoadXml((XmlElement)XMLSignatures[0]);

            String CertPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/CoverMyMeds.cer");
            X509Certificate2 SigningCert = new X509Certificate2(CertPath);

            return SignedSAML.CheckSignature(SigningCert, true);
        }

    }
}