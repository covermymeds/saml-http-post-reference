<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IdPLauncher.aspx.cs" Theme="Basic"
    Inherits="CoverMyMeds.SAML.IdentityProvider.IdPLauncher" %>

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
                        <asp:TextBox ID="txtIssuer" runat="server" Text="CMMTest.com"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Provider URL
                    </td>
                    <td>
                        <asp:TextBox ID="txtSPURL" runat="server" Text="http://localhost:40728/ServiceProviderDefault.aspx"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Target
                    </td>
                    <td>
                        <asp:TextBox ID="txtTarget" runat="server" Text="http://localhost:2456/PromisedLand.aspx"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        Name ID
                    </td>
                    <td>
                        <asp:TextBox ID="txtNameID" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Test SAML Attributes</legend>
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
                <asp:Button ID="btnLoadGroup1" runat="server" Text="Load Group 1" Width="120px"
                    OnClick="btnLoadGroup1_Click" />
                <asp:Button ID="btnLoadGroup2" runat="server" Text="Load Group 2" Width="120px"
                    OnClick="btnLoadGroup2_Click" />
                <asp:Button ID="btnLoadGroup3" runat="server" Text="Load Group 3" Width="120px"
                    OnClick="btnLoadGroup3_Click" />
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
                        <asp:TextBox ID="TextBox13" runat="server" Text="UserID"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox5" runat="server" Text="UserFirstName"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox6" runat="server" Text=""></asp:TextBox>
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
                        <asp:TextBox ID="TextBox10" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox6" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox1" runat="server" Text="UserCredentials"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox3" runat="server" Text="UserPrefix"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox15" runat="server" Text="UserSuffix"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox16" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        <asp:TextBox ID="TextBox17" runat="server" Text="UserDisplayName"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox18" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox10" runat="server" />
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
                        <asp:TextBox ID="TextBox19" runat="server" Text="GroupID"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox21" runat="server" Text="GroupName"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox23" runat="server" Text="GroupAddress1"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox25" runat="server" Text="GroupAddress2"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox27" runat="server" Text="GroupCity"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox29" runat="server" Text="GroupState"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox31" runat="server" Text="GroupZip"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox33" runat="server" Text="GroupFax"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox35" runat="server" Text="GroupPhone"></asp:TextBox>
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
                        <asp:TextBox ID="TextBox37" runat="server" Text="GroupContactPerson"></asp:TextBox>
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
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
