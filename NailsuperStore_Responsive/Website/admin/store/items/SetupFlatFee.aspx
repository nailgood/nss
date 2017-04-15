<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SetupFlatFee.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_items_SetupFlatFee" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Setup Fee Shipping</h4>
    <input type="button" value="Close" class="btn" onclick="window.close();" />
    <input type="hidden" runat="server" id="hidID" value="" />
    <table border="0" cellspacing="1" cellpadding="2" width="793px">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required" width="110px">
                Fee Apply In US:
            </td>
            <td class="field">
                <asp:TextBox ID="txtItemFee" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"
                    Enabled="true"></asp:TextBox>
                <CC:OneClickButton ID="btnSaveItemFee" runat="server" Text="Save" ValidationGroup="ItemFee"
                    CssClass="btn"></CC:OneClickButton>
                <asp:RequiredFieldValidator ID="rvUnitPrice" CssClass="msgError" runat="server" ValidationGroup="ItemFee"
                    ErrorMessage="Fee Apply For US is blank" ControlToValidate="txtItemFee" Display="Dynamic"></asp:RequiredFieldValidator>
                <CC:FloatValidator Display="Dynamic" CssClass="msgError" runat="server" ID="fvItemFee" ValidationGroup="ItemFee"
                    ControlToValidate="txtItemFee" ErrorMessage="Field 'Fee Apply For US' is invalid" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <span class="smallest">Fee Shipping Apply for each state</span>
    <table border="0" cellspacing="1" cellpadding="2" width="800px">
        <tr>
            <td class="required" width="110px">
                State:
            </td>
            <td class="field">
                <asp:DropDownList ID="drlState" AutoPostBack="true" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="msgError" runat="server" ValidationGroup="ItemFeeByState"
                    ErrorMessage="State is blank" ControlToValidate="drlState" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="required">
                1 item:
            </td>
            <td class="field">
                <asp:TextBox ID="txtFirstItemFeeShipping" runat="server" MaxLength="8" Columns="8"
                    Style="width: 67px;" Enabled="true"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="msgError" runat="server" ValidationGroup="ItemFeeByState"
                    ErrorMessage=" 1 item is blank" ControlToValidate="txtFirstItemFeeShipping" Display="Dynamic"></asp:RequiredFieldValidator>
                <CC:FloatValidator Display="Dynamic" runat="server" CssClass="msgError" ID="FloatValidator2" ValidationGroup="ItemFeeByState"
                    ControlToValidate="txtFirstItemFeeShipping" ErrorMessage="Field '1 item ' is invalid" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="required">
                Per item thereafter:
            </td>
            <td class="field">
                <asp:TextBox ID="txtNextItemFeeShipping" runat="server" MaxLength="8" Columns="8"
                    Style="width: 67px;" Enabled="true"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="msgError" runat="server" ValidationGroup="ItemFeeByState"
                    ErrorMessage="Per item thereafter is blank" ControlToValidate="txtNextItemFeeShipping"
                    Display="Dynamic"></asp:RequiredFieldValidator>
                <CC:FloatValidator Display="Dynamic" runat="server" CssClass="msgError" ID="FloatValidator1" ValidationGroup="ItemFeeByState"
                    ControlToValidate="txtNextItemFeeShipping" ErrorMessage="Field 'Per item thereafter ' is invalid" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="field">
            </td>
            <td class="field">
                <CC:OneClickButton ID="btnSaveItemFeeByState" runat="server" Text="Save" ValidationGroup="ItemFeeByState"
                    CssClass="btn"></CC:OneClickButton>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <p>
    </p>
</asp:Content>
