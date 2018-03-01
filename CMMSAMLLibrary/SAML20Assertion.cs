//-----------------------------------------------------------------------
// <copyright file="SAML20Assertion.cs" company="CoverMyMeds">
//  Copyright (c) 2012 CoverMyMeds.  All rights reserved.
//  This code is presented as reference material only.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.IO;
using CoverMyMeds.SAML.Library.Schema;

namespace CoverMyMeds.SAML.Library
{
    /// <summary>
    /// Encapsulate functionality for building a SAML Response using the Schema object
    ///     created by xsd.exe from the OASIS spec
    /// </summary>
    /// <remarks>Lots of guidance from this CodeProject implementation
    ///     http://www.codeproject.com/Articles/56640/Performing-a-SAML-Post-with-C#xx0xx
    /// </remarks>
    public class SAML20Assertion
    {
        /// <summary>
        /// Build a signed XML SAML Response string to be inlcuded in an HTML Form
        /// for POSTing to a SAML Service Provider
        /// </summary>
        /// <param name="Issuer">Identity Provider - Used to match the certificate for verifying 
        ///     Response signing</param>
        /// <param name="AssertionExpirationMinutes">Assertion lifetime</param>
        /// <param name="Audience"></param>
        /// <param name="Subject"></param>
        /// <param name="Recipient"></param>
        /// <param name="Attributes">Dictionary of attributes to send through for user SSO</param>
        /// <param name="SigningCert">X509 Certificate used to sign Assertion</param>
        /// <returns></returns>
        public static string CreateSAML20Response(string Issuer,
            int AssertionExpirationMinutes,
            string Audience,
            string Subject,
            string Recipient,
            Dictionary<string, string> Attributes,
            X509Certificate2 SigningCert)
        {
            // Create SAML Response object with a unique ID and correct version
            ResponseType response = new ResponseType()
            {
                ID = "_" + System.Guid.NewGuid().ToString(),
                Version = "2.0",
                IssueInstant = System.DateTime.UtcNow,
                Destination = Recipient.Trim(),
                Issuer = new NameIDType() { Value = Issuer.Trim() },
                Status = new StatusType() { StatusCode = new StatusCodeType() { Value = "urn:oasis:names:tc:SAML:2.0:status:Success" } }
            };

            // Put SAML 2.0 Assertion in Response
            response.Items = new AssertionType[] { CreateSAML20Assertion(Issuer, AssertionExpirationMinutes, Audience, Subject, Recipient, Attributes) };

            XmlDocument XMLResponse = SerializeAndSignSAMLResponse(response, SigningCert);

            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(XMLResponse.OuterXml));
        }

        /// <summary>
        /// Accepts SAML Response, serializes it to XML and signs using the supplied certificate
        /// </summary>
        /// <param name="Response">SAML 2.0 Response</param>
        /// <param name="SigningCert">X509 certificate</param>
        /// <returns>XML Document with computed signature</returns>
        private static XmlDocument SerializeAndSignSAMLResponse(ResponseType Response, X509Certificate2 SigningCert)
        {
            // Set serializer and writers for action
            XmlSerializer responseSerializer = new XmlSerializer(Response.GetType());
            StringWriter stringWriter = new StringWriter();
            XmlWriter responseWriter = XmlTextWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, Encoding = Encoding.UTF8 });
            responseSerializer.Serialize(responseWriter, Response);
            responseWriter.Close();
            XmlDocument xmlResponse = new XmlDocument(); 
            xmlResponse.LoadXml(stringWriter.ToString());

            // Set the namespace for prettier and more consistent XML
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlResponse.NameTable);
            ns.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");

            CertificateUtility.AppendSignatureToXMLDocument(ref xmlResponse, "#" + ((AssertionType)Response.Items[0]).ID, SigningCert);

            return xmlResponse;
        }

        /// <summary>
        /// Creates a SAML 2.0 Assertion Segment for a Response
        /// Simple implmenetation assuming a list of string key and value pairs
        /// </summary>
        /// <param name="Issuer"></param>
        /// <param name="AssertionExpirationMinutes"></param>
        /// <param name="Audience"></param>
        /// <param name="Subject"></param>
        /// <param name="Recipient"></param>
        /// <param name="Attributes">Dictionary of string key, string value pairs</param>
        /// <returns>Assertion to sign and include in Response</returns>
        private static AssertionType CreateSAML20Assertion(string Issuer,
            int AssertionExpirationMinutes,
            string Audience,
            string Subject,
            string Recipient,
            Dictionary<string, string> Attributes)
        {
            AssertionType NewAssertion = new AssertionType()
            {
                Version = "2.0",
                IssueInstant = System.DateTime.UtcNow,
                ID = "_" + System.Guid.NewGuid().ToString()
            };

            // Create Issuer
            NewAssertion.Issuer = new NameIDType() { Value = Issuer.Trim() };

            // Create Assertion Subject
            SubjectType subject = new SubjectType();
            NameIDType subjectNameIdentifier = new NameIDType() { Value = Subject.Trim(), Format = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified" };
            SubjectConfirmationType subjectConfirmation = new SubjectConfirmationType() { Method = "urn:oasis:names:tc:SAML:2.0:cm:bearer", SubjectConfirmationData = new SubjectConfirmationDataType() { NotOnOrAfter = DateTime.UtcNow.AddMinutes(AssertionExpirationMinutes), Recipient = Recipient } };
            subject.Items = new object[] { subjectNameIdentifier, subjectConfirmation };
            NewAssertion.Subject = subject;

            // Create Assertion Conditions
            ConditionsType conditions = new ConditionsType();
            conditions.NotBefore = DateTime.UtcNow;
            conditions.NotBeforeSpecified = true;
            conditions.NotOnOrAfter = DateTime.UtcNow.AddMinutes(AssertionExpirationMinutes);
            conditions.NotOnOrAfterSpecified = true;
            conditions.Items = new ConditionAbstractType[] { new AudienceRestrictionType() { Audience = new string[] { Audience.Trim() } } };
            NewAssertion.Conditions = conditions;

            // Add AuthnStatement and Attributes as Items
            AuthnStatementType authStatement = new AuthnStatementType() { AuthnInstant = DateTime.UtcNow, SessionIndex = NewAssertion.ID };
            AuthnContextType context = new AuthnContextType();
            context.ItemsElementName = new ItemsChoiceType5[] { ItemsChoiceType5.AuthnContextClassRef };
            context.Items = new object[] { "urn:oasis:names:tc:SAML:2.0:ac:classes:unspecified" };
            authStatement.AuthnContext = context;

            AttributeStatementType attributeStatement = new AttributeStatementType();
            attributeStatement.Items = new AttributeType[Attributes.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> attribute in Attributes)
            {
                attributeStatement.Items[i] = new AttributeType()
                {
                    Name = attribute.Key,
                    AttributeValue = new object[] { attribute.Value },
                    NameFormat = "urn:oasis:names:tc:SAML:2.0:attrname-format:basic"
                };
                i++;
            }

            NewAssertion.Items = new StatementAbstractType[] { authStatement, attributeStatement };

            return NewAssertion;
        }

    }
}


