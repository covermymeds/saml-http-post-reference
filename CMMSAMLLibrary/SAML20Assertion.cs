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
    public class SAML20Assertion
    {
        public string Target;
        public List<KeyValuePair<string, string>> Attributes;

        public static string CreateSAML20Response(string Issuer,
            int AssertionExpirationMinutes,
            string Audience,
            string Subject,
            string Recipient,
            Dictionary<string, string> Attributes,
            X509Certificate2 SigningCert)
        {
            // Create SAML Response
            ResponseType response = new ResponseType()
            {
                ID = "_" + System.Guid.NewGuid().ToString(),
                Version = "2.0",
                IssueInstant = System.DateTime.UtcNow,
                Destination = Recipient.Trim()
            };

            response.Issuer = new NameIDType() { Value = Issuer.Trim() };
            response.Status = new StatusType() { StatusCode = new StatusCodeType() { Value = "urn:oasis:names:tc:SAML:2.0:status:Success" } };

            // Put SAML 2.0 Assertion in Response
            response.Items = new AssertionType[] { CreateSAML20Assertion(Issuer, AssertionExpirationMinutes, Audience, Subject, Recipient, Attributes) };

            XmlDocument XMLResponse = SerializeAndSignSAMLResponse(response, SigningCert);

            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(XMLResponse.OuterXml));
        }

        private static XmlDocument SerializeAndSignSAMLResponse(ResponseType response,X509Certificate2 SigningCert)
        {
            // http://www.west-wind.com/weblog/posts/2008/Feb/23/Digitally-Signing-an-XML-Document-and-Verifying-the-Signature 
            XmlDocument xmlResponse = SerializeResponseToXML(response);
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlResponse.NameTable);
            ns.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            AssertionType assertion = (AssertionType)response.Items[0];
            SignedXml signedXML = new SignedXml(xmlResponse);

            signedXML.SigningKey = SigningCert.PrivateKey;
            //signedXML.SignedInfo.SignatureMethod = SignedXml.XmlDsigEnvelopedSignatureTransformUrl;

            // Key Info not needed at this point
            //KeyInfo keyInfo = new KeyInfo();
            //KeyInfoX509Data keyInfoData = new KeyInfoX509Data();
            //keyInfoData.AddIssuerSerial(cert.Issuer, cert.GetSerialNumberString());
            //keyInfo.AddClause(keyInfoData);

            //signedXML.KeyInfo = keyInfo;

            //signedXML.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NTransformUrl;
            signedXML.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            Reference reference = new Reference();
            reference.Uri = "#" + assertion.ID;
            //reference.Uri = assertion.ID;
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            //reference.AddTransform(new XmlDs
            signedXML.AddReference(reference);
            signedXML.ComputeSignature();

            XmlElement signature = signedXML.GetXml();

            XmlElement xeResponse = xmlResponse.DocumentElement;
            
            xeResponse.AppendChild(signature);

            return xmlResponse;
        }

        private static XmlDocument SerializeResponseToXML(ResponseType response)
        {
            XmlSerializer responseSerializer = new XmlSerializer(response.GetType());
            StringWriter stringWriter = new StringWriter();
            XmlDocument xmlRet = new XmlDocument();
            XmlWriter responseWriter = XmlTextWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, Encoding = Encoding.UTF8 });
            string SAMLString = string.Empty;
            responseSerializer.Serialize(responseWriter, response);
            responseWriter.Close();
            xmlRet.LoadXml(stringWriter.ToString());

            return xmlRet;
        }

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