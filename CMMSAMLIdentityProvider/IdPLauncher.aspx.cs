//-----------------------------------------------------------------------
// <copyright file="IdPLauncher.aspx.cs" company="CoverMyMeds">
//  Copyright (c) 2012 CoverMyMeds.  All rights reserved.
//  This code is presented as reference material only.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CoverMyMeds.SAML.Library;
using System.Security.Cryptography.X509Certificates;

namespace CoverMyMeds.SAML.IdentityProvider
{
    public partial class IdPLauncher : System.Web.UI.Page
    {

        protected void btnLaunchSSO_Click(object sender, EventArgs e)
        {
            // Set RelayState - Target Resource
            RelayState.Value = txtTarget.Text;

            // Create SAML Response and set Form Value
            // Collect SAML Attributes for packing into assertion
            Dictionary<string, string> SAMLAttributes = new Dictionary<string, string>();
            foreach (System.Web.UI.HtmlControls.HtmlTableRow tr in tblAttrs.Rows)
            {
                if (tr.Cells[1].Controls.Count > 1)
                {
                    TextBox AttrKey = (TextBox)tr.Cells[0].Controls[1];
                    if (!string.IsNullOrEmpty(AttrKey.Text))
                    {
                        TextBox AttrValue = (TextBox)tr.Cells[1].Controls[1];
                        CheckBox SendNull = (CheckBox)tr.Cells[2].Controls[1];
                        if (SendNull.Checked)
                        {
                            if (string.IsNullOrEmpty(AttrValue.Text))
                            {
                                SAMLAttributes.Add(((TextBox)tr.Cells[0].Controls[1]).Text, null);
                            }
                            else
                            {
                                SAMLAttributes.Add(((TextBox)tr.Cells[0].Controls[1]).Text, AttrValue.Text);
                            }
                        }
                        else
                        {
                            SAMLAttributes.Add(((TextBox)tr.Cells[0].Controls[1]).Text, AttrValue.Text);
                        }
                    }
                }
            }

            // get the certificate
            String CertPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/CoverMyMeds.pfx");
            X509Certificate2 SigningCert = new X509Certificate2(CertPath, "4CoverMyMeds");
            
            // Add base 64 encoded SAML Response to form for POST
            String NameID = String.Empty;
            if (!string.IsNullOrEmpty(txtNameID.Text))
            {
                NameID = txtNameID.Text;
            }

            SAMLResponse.Value = SAML20Assertion.CreateSAML20Response(
               txtIssuer.Text, 5, "Audience", NameID, "Recipient", SAMLAttributes, SigningCert);

            // Set Body page load action
            frmIdPLauncher.Action = txtSPURL.Text;

            // add javascript to HTTP POST to the SSO configured
            // This implements the IdP-initiated HTTP POST use case
            HtmlGenericControl body = (HtmlGenericControl)this.Page.FindControl("bodySSO");
            if (body != null)
            {
                body.Attributes.Add("onload", "document.forms.frmIdPLauncher.submit();");
            }

        }

        #region Page Admin and Maintenance

        protected void btnClearAttr_Click(object sender, EventArgs e)
        {
            ClearAttributes();
        }

        private void ClearAttributes()
        {
            foreach (System.Web.UI.HtmlControls.HtmlTableRow tr in tblAttrs.Rows)
            {
                if (tr.Cells[1].Controls.Count > 1)
                {
                    TextBox AttrValue = (TextBox)tr.Cells[1].Controls[1];
                    AttrValue.Text = string.Empty;
                }
            }

        }

        private void LoadAttributes(List<KeyValuePair<string, string>> AttributeValues)
        {
            foreach (KeyValuePair<string, string> AttributeValue in AttributeValues)
            {
                foreach (System.Web.UI.HtmlControls.HtmlTableRow tr in tblAttrs.Rows)
                {
                    if (tr.Cells[1].Controls.Count > 1)
                    {
                        if (((TextBox)tr.Cells[0].Controls[1]).Text.Equals(AttributeValue.Key))
                        {
                            TextBox AttrValue = (TextBox)tr.Cells[1].Controls[1];
                            AttrValue.Text = AttributeValue.Value;
                        }
                    }
                }
            }
        }

        protected void btnLoadSamp1_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("UserID", "123456"));
            lsUser.Add(new KeyValuePair<string, string>("Username", "user_1"));
            lsUser.Add(new KeyValuePair<string, string>("UserFirstName", "Heywood"));
            lsUser.Add(new KeyValuePair<string, string>("UserLastName", "Floyd"));
            lsUser.Add(new KeyValuePair<string, string>("UserEmail", "user_1@bar.net"));
            lsUser.Add(new KeyValuePair<string, string>("UserPhone", "(614) 555-5555"));
            lsUser.Add(new KeyValuePair<string, string>("UserFax", "(614) 555-6666"));
            LoadAttributes(lsUser);
        }

        protected void btnLoadSamp2_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("UserID", "456789"));
            lsUser.Add(new KeyValuePair<string, string>("Username", "user_2"));
            lsUser.Add(new KeyValuePair<string, string>("UserDisplayName", "Dr. David Chandra"));
            lsUser.Add(new KeyValuePair<string, string>("UserEmail", "user_2@bar.net"));
            lsUser.Add(new KeyValuePair<string, string>("UserPhone", "(614) 555-5555"));
            lsUser.Add(new KeyValuePair<string, string>("UserFax", "(614) 555-6666"));
            LoadAttributes(lsUser);
        }

        protected void btnLoadSamp3_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("UserID", "789234"));
            lsUser.Add(new KeyValuePair<string, string>("Username", "user_3"));
            lsUser.Add(new KeyValuePair<string, string>("UserFirstName", "Rakesh"));
            lsUser.Add(new KeyValuePair<string, string>("UserLastName", "Chandrasehkar"));
            lsUser.Add(new KeyValuePair<string, string>("UserEmail", "user_3@bar.net"));
            lsUser.Add(new KeyValuePair<string, string>("UserPhone", "(614) 555-5555"));
            lsUser.Add(new KeyValuePair<string, string>("UserFax", "(614) 555-6666"));
            LoadAttributes(lsUser);
        }

        protected void btnLoadGroup1_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("GroupID", "23174296"));
            lsUser.Add(new KeyValuePair<string, string>("GroupName", "CMM"));
            lsUser.Add(new KeyValuePair<string, string>("GroupAddress1", "E. Chestnut St."));
            lsUser.Add(new KeyValuePair<string, string>("GroupAddress2", "Ste 100"));
            lsUser.Add(new KeyValuePair<string, string>("GroupCity", "Columbus"));
            lsUser.Add(new KeyValuePair<string, string>("GroupState", "OH"));
            lsUser.Add(new KeyValuePair<string, string>("GroupZip", "43212"));
            lsUser.Add(new KeyValuePair<string, string>("GroupPhone", "(614) 555-6666"));
            lsUser.Add(new KeyValuePair<string, string>("GroupFax", "(614) 555-6666"));
            LoadAttributes(lsUser);
        }

        protected void btnLoadGroup2_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("GroupID", "34897612"));
            lsUser.Add(new KeyValuePair<string, string>("GroupName", "Gorb"));
            lsUser.Add(new KeyValuePair<string, string>("GroupAddress1", "W. Market St."));
            lsUser.Add(new KeyValuePair<string, string>("GroupAddress2", "0"));
            lsUser.Add(new KeyValuePair<string, string>("GroupCity", "Columbus"));
            lsUser.Add(new KeyValuePair<string, string>("GroupState", "OH"));
            lsUser.Add(new KeyValuePair<string, string>("GroupZip", "43212"));
            lsUser.Add(new KeyValuePair<string, string>("GroupPhone", "(614) 555-3333"));
            lsUser.Add(new KeyValuePair<string, string>("GroupFax", "(614) 555-4444"));
            LoadAttributes(lsUser);
        }

        protected void btnLoadGroup3_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("GroupID", "89652387"));
            lsUser.Add(new KeyValuePair<string, string>("GroupName", "Spraynard Group"));
            lsUser.Add(new KeyValuePair<string, string>("GroupAddress1", "Wonk Ave."));
            lsUser.Add(new KeyValuePair<string, string>("GroupAddress2", "Ste 23"));
            lsUser.Add(new KeyValuePair<string, string>("GroupCity", "Columbus"));
            lsUser.Add(new KeyValuePair<string, string>("GroupState", "OH"));
            lsUser.Add(new KeyValuePair<string, string>("GroupZip", "43212"));
            lsUser.Add(new KeyValuePair<string, string>("GroupPhone", "(614) 555-7777"));
            lsUser.Add(new KeyValuePair<string, string>("GroupFax", "(614) 555-6666"));
            LoadAttributes(lsUser);
        }

        protected void btnClearChecks_Click(object sender, EventArgs e)
        {
            foreach (System.Web.UI.HtmlControls.HtmlTableRow tr in tblAttrs.Rows)
            {
                if (tr.Cells[1].Controls.Count > 1)
                {
                    CheckBox SubmitNull = (CheckBox)tr.Cells[2].Controls[1];
                    SubmitNull.Checked = false;
                }
            }
        }

        #endregion

    }
}