using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace CoverMyMeds.SAML.IdentityProvider
{
    public class CertificateUtility
    {

        /// <summary>
        /// Find and return a certificate on the server to allow signing of SAML Assertion
        /// </summary>
        /// <param name="CertSubjectName">Certificate name</param>
        /// <param name="CertStoreName">Which certificate store to look in</param>
        /// <param name="CertStoreLocation">Where in the store to look for the certificate</param>
        /// <returns></returns>
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

    }
}