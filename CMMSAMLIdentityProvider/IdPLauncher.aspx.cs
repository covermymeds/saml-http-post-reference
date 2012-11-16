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
            X509Certificate2 SigningCert = CertificateUtility.GetCertificateForSigning(ddlIssuer.SelectedValue, StoreName.Root, StoreLocation.LocalMachine);
            
            // Add base 64 encoded SAML Response to form for POST 
            SAMLResponse.Value = SAML20Assertion.CreateSAML20Response(ddlIssuer.SelectedItem.Text, 5, "Audience", "Subject", "Recipient", SAMLAttributes, SigningCert);

            // Set Body page load action
            if (string.IsNullOrEmpty(txtSPURL.Text))
            {
                frmIdPLauncher.Action = ddlSPUrl.SelectedValue;
            }
            else
            {
                frmIdPLauncher.Action = txtSPURL.Text;
            }
            HtmlGenericControl body = (HtmlGenericControl)this.Page.FindControl("bodySSO");
            if (body != null)
            {
                //body.Attributes.Add("onload", "alert('Testing!!');");
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
            lsUser.Add(new KeyValuePair<string, string>("UserNID", "123456"));
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
            lsUser.Add(new KeyValuePair<string, string>("UserNID", "456789"));
            lsUser.Add(new KeyValuePair<string, string>("Username", "user_2"));
            lsUser.Add(new KeyValuePair<string, string>("UserFirstName", "David"));
            lsUser.Add(new KeyValuePair<string, string>("UserLastName", "Chandra"));
            lsUser.Add(new KeyValuePair<string, string>("UserEmail", "user_2@bar.net"));
            lsUser.Add(new KeyValuePair<string, string>("UserPhone", "(614) 555-5555"));
            lsUser.Add(new KeyValuePair<string, string>("UserFax", "(614) 555-6666"));
            LoadAttributes(lsUser);
        }

        protected void btnLoadSamp3_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("UserNID", "789234"));
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
            lsUser.Add(new KeyValuePair<string, string>("OfficeNID", "23174296"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeName", "CMM"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeAddress1", "E. Chestnut St."));
            lsUser.Add(new KeyValuePair<string, string>("OfficeAddress2", "Ste 100"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeCity", "Columbus"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeState", "OH"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeZip", "43212"));
            lsUser.Add(new KeyValuePair<string, string>("OfficePhone", "(614) 555-6666"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeFax", "(614) 555-6666"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeContactPerson", ""));
            lsUser.Add(new KeyValuePair<string, string>("OfficeFacility", ""));
            lsUser.Add(new KeyValuePair<string, string>("OfficeStatus", ""));
            LoadAttributes(lsUser);
        }

        protected void btnLoadGroup2_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("OfficeNID", "34897612"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeName", "Gorb"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeAddress1", "W. Market St."));
            lsUser.Add(new KeyValuePair<string, string>("OfficeAddress2", "0"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeCity", "Columbus"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeState", "OH"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeZip", "43212"));
            lsUser.Add(new KeyValuePair<string, string>("OfficePhone", "(614) 555-3333"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeFax", "(614) 555-4444"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeContactPerson", ""));
            lsUser.Add(new KeyValuePair<string, string>("OfficeFacility", ""));
            lsUser.Add(new KeyValuePair<string, string>("OfficeStatus", ""));
            LoadAttributes(lsUser);
        }

        protected void btnLoadGroup3_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> lsUser = new List<KeyValuePair<string, string>>();
            lsUser.Add(new KeyValuePair<string, string>("OfficeNID", "89652387"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeName", "Spraynard Group"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeAddress1", "Wonk Ave."));
            lsUser.Add(new KeyValuePair<string, string>("OfficeAddress2", "Ste 23"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeCity", "Columbus"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeState", "OH"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeZip", "43212"));
            lsUser.Add(new KeyValuePair<string, string>("OfficePhone", "(614) 555-7777"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeFax", "(614) 555-6666"));
            lsUser.Add(new KeyValuePair<string, string>("OfficeContactPerson", ""));
            lsUser.Add(new KeyValuePair<string, string>("OfficeFacility", ""));
            lsUser.Add(new KeyValuePair<string, string>("OfficeStatus", ""));
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