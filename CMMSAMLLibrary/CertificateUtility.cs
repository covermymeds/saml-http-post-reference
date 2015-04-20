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
        /// Find and return a certificate on the server to allow signing of SAML Assertion
        /// </summary>
        /// <param name="CertSubjectName">Certificate name</param>
        /// <param name="CertStoreName">Which certificate store to look in</param>
        /// <param name="CertStoreLocation">Where in the store to look for the certificate</param>
        /// <returns>X509 certificate to use for signing XML within a SAML Response</returns>
        public static X509Certificate2 GetCertificateForSigning(string CertSubjectName, StoreName CertStoreName, StoreLocation CertStoreLocation)
        {
            X509Certificate2 XMLSigningCert = null;

            X509Store CertStore = new X509Store(CertStoreName, CertStoreLocation);
            CertStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection CertStoreCollection = CertStore.Certificates.Find(X509FindType.FindBySubjectName, CertSubjectName, true);
            if (CertStoreCollection.Count < 1)
            {
                throw new ArgumentException(String.Format("Unable to locate certificate from issuer {0}", CertSubjectName));
            }
            XMLSigningCert = CertStoreCollection[0];
            CertStore.Close();
            return XMLSigningCert;
        }

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
            SignedXml signedXML = new SignedXml(XMLSerializedSAMLResponse);

            signedXML.SigningKey = SigningCert.PrivateKey;
            signedXML.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            Reference reference = new Reference();
            reference.Uri = ReferenceURI;
            //reference.Uri = "#" + ReferenceURI;
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXML.AddReference(reference);
            signedXML.ComputeSignature();

            XmlElement signature = signedXML.GetXml();

            XmlElement xeResponse = XMLSerializedSAMLResponse.DocumentElement;

            xeResponse.AppendChild(signature);
        }
    }
}