﻿//-----------------------------------------------------------------------
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

                if (ast == null) throw new ApplicationException("Invalid SAML Assertion: Missing Attribute Values");
                SAMLAttributes = new Dictionary<string, string>();
                // Do what needs to be done to pull specific attributes out for sending on
                // For now assuming this is a simple list of string key and string values
                foreach (AttributeType at in ast.Items)
                {
                    SAMLAttributes.Add(at.Name, at.AttributeValue.ToString());
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
                    SAMLXML.LoadXml(System.Text.Encoding.UTF8.GetString(
                        Convert.FromBase64String(context.Request.Params["SAMLResponse"].ToString()))
                        );

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
            XmlSerializer serializer = new XmlSerializer(typeof(ResponseType));

            ResponseType response = (ResponseType)serializer.Deserialize(
                new MemoryStream(Encoding.UTF8.GetBytes(SAMLXML.OuterXml)));

            return (AssertionType)response.Items[0];
        }

        private bool ValidateX509CertificateSignature(XmlDocument SAMLResponse)
        {
            XmlNodeList XMLSignatures = SAMLResponse.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");

            // Checking If the Response or the Assertion has been signed once and only once.
            if (XMLSignatures.Count != 1) return false;

            SignedXml SignedSAML = new SignedXml(SAMLResponse);
            SignedSAML.LoadXml((XmlElement)XMLSignatures[0]);

            // Get X509 Certificate from Cert Store
            X509Certificate2 SigningCert = CertificateUtility.GetCertificateForSigning(
                ConfigurationManager.AppSettings["CertIssuer"], 
                StoreName.Root, 
                StoreLocation.LocalMachine);

            return SignedSAML.CheckSignature(SigningCert, true);
        }

    }
}