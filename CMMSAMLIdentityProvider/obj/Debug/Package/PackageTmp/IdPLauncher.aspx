<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IdPLauncher.aspx.cs" Theme="Basic"
    Inherits="CMMSAMLIdPTestHarness.IdPLauncher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body runat="server" id="bodySSO">
    <form id="frmIdPLauncher" runat="server">
    <div style="display: none">
        <input id="SAMLResponse" type="text" runat="server" enableviewstate="False" />
        <input id="RelayState" type="text" runat="server" enableviewstate="False" />
    </div>
    <div style="width: 700px;">
        <div style="font-size: x-large;">
            CoverMyMeds SAML IdentityProvider SSO Launcher</div>
        <asp:Button ID="btnLaunchSSO" runat="server" Text="Launch SSO" OnClick="btnLaunchSSO_Click" />
        <fieldset>
            <legend>SSO Configs</legend>
            <table>
                <tr>
                    <td style="width: 150px;">
                        Issuer
                    </td>
                    <td>
                        <asp:TextBox Visible="False" ID="txtIssuer" runat="server" Text="CMMTest.com"></asp:TextBox>
                        <asp:DropDownList Width="100%" ID="ddlIssuer" runat="server">
                            <asp:ListItem Value="SSOTest" Text="CMMTest.com" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="navinet.test1.qa" Text="navinet.test1.qa" Enabled="false"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Provider URL
                    </td>
                    <td>
                        <asp:TextBox ID="txtSPURL" runat="server" Text="https://devsso.covermymeds.com/consume"></asp:TextBox>
                        <asp:DropDownList ID="ddlSPUrl" Visible="false" Width="100%" runat="server">
                            <asp:ListItem Text="Webstage 2 Port 50333" Value="http://webstage2:50333/consume"
                                Selected="True" />
                            <asp:ListItem Text="Dev CoverMyMeds SSO" Value="https://devsso.covermymeds.com/" />
                            <asp:ListItem Text="CoverMyMeds SSO" Value="https://sso.covermymeds.com/" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Target
                    </td>
                    <td>
                        <asp:TextBox ID="txtTarget" runat="server" Text="https://staging.covermymeds.com/pub/request/new"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Test Navinet Attributes</legend>
            <div>
                <asp:Button ID="btnClearAttr" runat="server" Text="Clear Values" Width="120px" OnClick="btnClearAttr_Click" /><asp:Button
                    ID="btnClearChecks" runat="server" Text="Clear Submit Nulls" Width="120px" 
                    onclick="btnClearChecks_Click" /></div>
            <div>
                <asp:Button ID="btnLoadSamp1" runat="server" Text="Load User 1" Width="120px" OnClick="btnLoadSamp1_Click" />
                <asp:Button ID="btnLoadSamp2" runat="server" Text="Load User 2" Width="120px" OnClick="btnLoadSamp2_Click" />
                <asp:Button ID="btnLoadSamp3" runat="server" Text="Load User 3" Width="120px" OnClick="btnLoadSamp3_Click" />
            </div>
            <div>
                <asp:Button ID="btnLoadOffice1" runat="server" Text="Load Office 1" Width="120px"
                    OnClick="btnLoadOffice1_Click" />
                <asp:Button ID="btnLoadOffice2" runat="server" Text="Load Office 2" Width="120px"
                    OnClick="btnLoadOffice2_Click" />
                <asp:Button ID="btnLoadOffice3" runat="server" Text="Load Office 3" Width="120px"
                    OnClick="btnLoadOffice3_Click" />
            </div>
            <table runat="server" id="tblAttrs">
                <tr>
                    <td style="width: 150px;">
                        Attribute Name
                    </td>
                    <td>
                        Value
                    </td>
                    <td>
                        Submit Null
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox41" runat="server" Text="MPPVersion"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox42" runat="server" Text="">1.00</asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox2" runat="server" Text="SessionKey"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" Text="{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxx}"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox3" runat="server" Text="SessionExpireDTM"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox5" runat="server" Text="UserFirstName"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox6" runat="server" Text="Heywood"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox4" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox7" runat="server" Text="UserMiddleInitial"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox8" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox5" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox9" runat="server" Text="UserLastName"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox10" runat="server" Text="Floyd"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox6" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox11" runat="server" Text="Username"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox12" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox7" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox49" runat="server" Text="UserEmail"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox50" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox25" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox51" runat="server" Text="UserPhone"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox52" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox26" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox53" runat="server" Text="UserFax"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox54" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox27" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox13" runat="server" Text="UserNID"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox14" runat="server" Text="">123456</asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox24" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox15" runat="server" Text="UserRole"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox16" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox23" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox17" runat="server" Text="UserIsSecurityOfficer"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox18" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox22" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox19" runat="server" Text="OfficeNID"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox20" runat="server" Text="23174296"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox21" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox21" runat="server" Text="OfficeName"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox22" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox20" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox23" runat="server" Text="OfficeAddress1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox24" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox19" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox25" runat="server" Text="OfficeAddress2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox26" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox18" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox27" runat="server" Text="OfficeCity"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox28" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox17" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox29" runat="server" Text="OfficeStateID"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox30" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox16" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox31" runat="server" Text="OfficeZip"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox32" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox15" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox33" runat="server" Text="OfficeFax"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox34" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox14" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox35" runat="server" Text="OfficePhone"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox36" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox13" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox37" runat="server" Text="OfficeContactPerson"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox38" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox12" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox39" runat="server" Text="OfficeIsFacility"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox40" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox11" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox43" runat="server" Text="OfficeStatus"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox44" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox10" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox45" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox46" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox9" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox47" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox48" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox8" runat="server" />
                    </td>
                </tr>
                <%--<tr>
                    <td style="width: 150px;">
                        Session Key
                    </td>
                    <td>
                        <asp:TextBox ID="SessionKey" runat="server" Text="{2FF9E76B-6424-47FE-90BC-A8197043F089}"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        User NID
                    </td>
                    <td>
                        <asp:TextBox ID="UserNID" runat="server" Text="12345"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        User NPI
                    </td>
                    <td>
                        <asp:TextBox ID="NPI" runat="server" Text="3312345678"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Navinet Username
                    </td>
                    <td>
                        <asp:TextBox ID="Username" runat="server" Text="NNUsername"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Last Name
                    </td>
                    <td>
                        <asp:TextBox ID="UserLastName" runat="server" Text="Jones"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        First Name
                    </td>
                    <td>
                        <asp:TextBox ID="UserFirstName" runat="server" Text="Fred"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Office NID
                    </td>
                    <td>
                        <asp:TextBox ID="OfficeNID" runat="server" Text="6947"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Office Name
                    </td>
                    <td>
                        <asp:TextBox ID="OfficeName" runat="server" Text="The Des Moines Institute"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Office Fax
                    </td>
                    <td>
                        <asp:TextBox ID="OfficeFax" runat="server" Text="614-867-5309"></asp:TextBox>
                    </td>
                </tr>--%>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
