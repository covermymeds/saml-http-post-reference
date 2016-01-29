//-----------------------------------------------------------------------
// <copyright file="CertificateUtility.cs" company="CoverMyMeds">
//  Copyright (c) 2012 CoverMyMeds.  All rights reserved.
//  This code is presented as reference material only.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using CoverMyMeds.SAML.Library.Schema;

namespace CoverMyMeds.SAML.Library
{
    /// <summary>
    /// Methods specific to interacting with certificate on the machine
    /// </summary>
    public class CertificateUtility
    {

        /// <summary>
        /// Use an X509 certificate to append a computed signature to an XML serialized Response
        /// </summary>
        /// <param name="XMLSerializedSAMLResponse"></param>
        /// <param name="ReferenceURI">Assertion ID from SAML Response</param>
        /// <param name="SigningCert">X509 Certificate for signing</param>
        /// <remarks>Referenced this article:
        ///     http://www.west-wind.com/weblog/posts/2008/Feb/23/Digitally-Signing-an-XML-Document-and-Verifying-the-Signature
        /// </remarks>
        public static void AppendSignatureToXMLDocument(ref XmlDocument XMLSerializedSAMLResponse, String ReferenceURI, X509Certificate2 SigningCert)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(XMLSerializedSAMLResponse.NameTable);
            ns.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlElement xeAssertion = XMLSerializedSAMLResponse.DocumentElement.SelectSingleNode("saml:Assertion", ns) as XmlElement;

            SignedXml signedXML = new SignedXml(xeAssertion);

            signedXML.SigningKey = SigningCert.PrivateKey;
            signedXML.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            Reference reference = new Reference(ReferenceURI);
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform("http://www.w3.org/2001/10/xml-exc-c14n#"));
            
            signedXML.AddReference(reference);
            signedXML.ComputeSignature();

            XmlElement signature = signedXML.GetXml();

            xeAssertion.AppendChild(signature);
        }
    }
}