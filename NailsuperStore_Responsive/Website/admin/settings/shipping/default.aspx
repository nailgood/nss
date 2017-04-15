<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_settings_shipping_Default"
    MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>
        Free Shipping Policy</h4>
    <table cellspacing="2" cellpadding="3" border="0">
        <tr>
            <td class="required" style="vertical-align: middle; width: 180px; padding-left: 12px;"
                align="left">
                Qualifying Purchase Amount
            </td>
            <td class="field" style="padding-left: 13px;" colspan="4">
                <asp:TextBox ID="txtAmountFee" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regvAmountFee" ValidationGroup="shipping" runat="server"
                    ErrorMessage="Data is not valid" EnableClientScript="true" ControlToValidate="txtAmountFee"
                    ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <asp:Repeater EnableViewState="True" ID="sysparamRepeater" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="required" style="vertical-align: middle; padding-left: 12px;" align="left">
                        <asp:Label ID="lblWeightRange" runat="server" Text="Weight Range">
                        </asp:Label>
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                    <td class="field">
                        <table>
                            <tr>
                                <td style="padding-left: 10px;">
                                    <asp:TextBox ID="txtLowWeightValue" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox>
                                </td>
                                <td>
                                    ~
                                </td>
                                <td style="padding-right: 1px;">
                                    <asp:TextBox ID="txtHighWeightValue" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox>
                                </td>
                                <td style="width: 150px;">
                                    <asp:CustomValidator ID="cusvHighWeightValue" ControlToValidate="txtHighWeightValue" 
                                        EnableClientScript="true" ValidationGroup="shipping" ErrorMessage=""
                                        ValidateEmptyText="true" ClientValidationFunction="CheckHightWeightRange" runat="server"> 
                                    </asp:CustomValidator>
                                    <asp:CustomValidator ID="cusvLowWeightValue" ControlToValidate="txtLowWeightValue"
                                        EnableClientScript="true" ValidationGroup="shipping" ErrorMessage=""
                                        ValidateEmptyText="true" ClientValidationFunction="CheckLowWeightRange" runat="server"> 
                                    </asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="required" style="vertical-align: middle; width: 100px; padding-left: 15px;"
                        align="left">
                        Charge:
                    </td>
                    <td class="field" style="vertical-align: middle; width: 220px;">
                        <asp:TextBox ID="txtHandlingFee" runat="server" Columns="50" MaxLength="50" Style="width: 80px;"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regvHandlingFee" ValidationGroup="shipping" runat="server"
                            ErrorMessage="Charge is not valid" EnableClientScript="true" ControlToValidate="txtHandlingFee"
                            ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td>
            </td>
            <td colspan="4">
                <br />
                <CC:OneClickButton ID="btnSave" ValidationGroup="shipping" CausesValidation="true"
                    runat="server" Text="Save" CssClass="btn" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        
        function CheckHightWeightRange(source, arguments) {
            var txtLowWeightValueId = source.id.replace('cusvHighWeightValue', 'txtLowWeightValue');
            var LowWeightValue = document.getElementById(txtLowWeightValueId).value;
            var HighWeightValue = arguments.Value;
            if (!isDoubleValid(LowWeightValue) || !isDoubleValid(HighWeightValue)) {
                arguments.IsValid = false;
                source.textContent = source.innerText = source.innerHTML = "Weight Range is not valid";
                return;
            }
            arguments.IsValid = true;
        }
        function CheckLowWeightRange(source, arguments) {
            var txtHighWeightValueId = source.id.replace('cusvLowWeightValue', 'txtHighWeightValue');
            var HighWeightValue = document.getElementById(txtHighWeightValueId).value;
            var LowWeightValue = arguments.Value;
            if (!isDoubleValid(LowWeightValue) || !isDoubleValid(HighWeightValue)) {
                arguments.IsValid = false;
                return;
            }
            arguments.IsValid = true;
        }
        function isDoubleValid(value) {
           
            var doubleFormat = /^[0-9.]*$/;
            if (!doubleFormat.test(value)) {
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
