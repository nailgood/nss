<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_navision_mixmatch_Edit" Title="Mix Match" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Mix Match Promotion</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Type:
            </td>
            <td class="field">
                <asp:DropDownList ID="drlType" runat="server">
                    <asp:ListItem Value="1">Public</asp:ListItem>
                    <asp:ListItem Value="2">Product coupon</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
              <asp:RequiredFieldValidator ID="refvType" runat="server" Display="Dynamic" ControlToValidate="drlType"
                    CssClass="msgError" ErrorMessage="Field 'Type' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Mix Match No:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMixMatchNo" runat="server" MaxLength="20" Columns="20" Style="width: 139px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvMixMatchNo" runat="server" Display="Dynamic" ControlToValidate="txtMixMatchNo"
                    CssClass="msgError" ErrorMessage="Field 'Mix Match No' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
                <tr>
            <td class="required">
                Product:
            </td>
            <td class="field">
                <asp:TextBox ID="txtProduct" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvProduct" runat="server" Display="Dynamic"
                    ControlToValidate="txtProduct" CssClass="msgError" ErrorMessage="Field 'Product' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Promotion:
            </td>
            <td class="field">
                <asp:TextBox ID="txtDescription" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="Dynamic"
                    ControlToValidate="txtDescription" CssClass="msgError" ErrorMessage="Field 'Promotion' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trCustomerPriceGroup" runat="server">
            <td class="optional">
                Customer Price Group:
            </td>
            <td class="field">
                <asp:DropDownList ID="drpCustomerPriceGroupId" runat="server" />
            </td>
            <td>
            </td>
        </tr>
        <tr id="trStartDate" runat="server">
            <td class="optional">
                Starting Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvStartingDate" ControlToValidate="dtStartingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Starting Date' is invalid" />
            </td>
        </tr>
        <tr id="trEndDate" runat="server">
            <td class="optional">
                Ending Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator Display="Dynamic" runat="server" ID="dtvEndingDate" ControlToValidate="dtEndingDate"
                    CssClass="msgError" ErrorMessage="Date Field 'Ending Date' is invalid" />
            </td>
        </tr>
        <tr id="trActive" runat="server">
            <td class="required">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Discount Type:
            </td>
            <td class="field">
                <asp:DropDownList runat="server" ID="drpDiscountType">
                    <asp:ListItem Text="Line Specific" Value="Line spec." />
                    <asp:ListItem Text="Least Expensive" Value="Least Expensive" />
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvDiscountType" runat="server" Display="Dynamic"
                    ControlToValidate="drpDiscountType" CssClass="msgError" ErrorMessage="Field 'Discount Type' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Lines To Trigger:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLinesToTrigger" runat="server" MaxLength="4" Columns="4" Style="width: 43px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvLinesToTrigger" runat="server" Display="Dynamic"
                    ControlToValidate="txtLinesToTrigger" CssClass="msgError" ErrorMessage="Field 'Lines To Trigger' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator
                        Display="Dynamic" runat="server" ID="fvLinesToTrigger" ControlToValidate="txtLinesToTrigger"
                        CssClass="msgError" ErrorMessage="Field 'Lines To Trigger' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Times Applicable:
            </td>
            <td class="field">
                <asp:TextBox ID="txtTimesApplicable" runat="server" MaxLength="4" Columns="4" Style="width: 43px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvTimesApplicable" runat="server" Display="Dynamic"
                    ControlToValidate="txtTimesApplicable" CssClass="msgError" ErrorMessage="Field 'Times Applicable' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator
                        Display="Dynamic" runat="server" ID="fvTimesApplicable" ControlToValidate="txtTimesApplicable"
                        CssClass="msgError" ErrorMessage="Field 'Times Applicable' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Mandatory:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMandatory" runat="server" MaxLength="4" Columns="4" Style="width: 43px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvMandatory" runat="server" Display="Dynamic" ControlToValidate="txtMandatory"
                    CssClass="msgError" ErrorMessage="Field 'Mandatory' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator
                        Display="Dynamic" runat="server" ID="fvMandatory" ControlToValidate="txtMandatory"
                        CssClass="msgError" ErrorMessage="Field 'Mandatory' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Optional:
            </td>
            <td class="field">
                <asp:TextBox ID="txtOptional" runat="server" MaxLength="4" Columns="4" Style="width: 43px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvOptional" runat="server" Display="Dynamic" ControlToValidate="txtOptional"
                    CssClass="msgError" ErrorMessage="Field 'Optional' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator
                        Display="Dynamic" runat="server" ID="fvOptional" ControlToValidate="txtOptional"
                        CssClass="msgError" ErrorMessage="Field 'Optional' is invalid" />
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td class="required">
                <b>Is Collection?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="cbIsCollection" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Default Type:
            </td>
            <td class="field">
                <asp:DropDownList runat="server" ID="ddlDefaultType" >
                    <asp:ListItem Text="Non Item Default" value="0" Selected="True" />
                    <asp:ListItem Text="One Item Default" value="1" Selected="False" />
                    <asp:ListItem Text="All Item Default" value="2" Selected="False" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Mix Match Promotion?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</asp:Content>
