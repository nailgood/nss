<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    CodeFile="AddTrackingNumber.aspx.vb" Inherits="admin_store_orders_AddTrackingNumber" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Tracking Number</h4>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSubmit" runat="server">
        <div class="red">
            <asp:Label ID="lbMsg" runat="server"></asp:Label>
        </div>
        <table cellpadding="2" cellspacing="2">
            <tr>
                <td class="field">
                    Order No:
                </td>
                <td class="field">
                    <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Ship Via / Options
                </td>
                <td class="field">
                    <asp:Label ID="lblShippingType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trShippingDetail" runat="server">
                <td class="field">
                    Shipping Detail
                </td>
                <td class="field">
                    <asp:DropDownList ID="drlShippingDetail" onChange="ChangeShippingDetail(this.value);"
                        runat="server">
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdDefaultvalue" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td class="required">
                    <asp:Label ID="lblTrackingNumber" runat="server" Text="Tracking Number"></asp:Label>
                </td>
                <td class="field">
                    <asp:TextBox ID="F_TrackingNumber" runat="server" MaxLength="255" Columns="50" Style="width: 319px;"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="msgError" runat="server" ErrorMessage="Tracking Number is required"
                        ControlToValidate="F_TrackingNumber" ValidationGroup="grTracking" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="field">
                    <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                </td>
                <td class="field">
                    <asp:TextBox ID="txtNote" runat="server" TextMode="SingleLine" Width="319px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="field">
                     Email Tracking To Customer
                </td>
                <td class="field">
                    <asp:CheckBox ID="chkSendTracking" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSubmit" runat="server" Text="Save" OnClientClick="return ConfirmTrackingNumber();" CssClass="btn" ValidationGroup="grTracking" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script type="text/javascript">
    function ChangeShippingDetail(value) {
       
        if (value == 4) {
            document.getElementById('<%=lblNote.ClientID %>').innerHTML = 'Label'
            document.getElementById('<%=lblTrackingNumber.ClientID %>').innerHTML = 'Link'
        }
        else {
            document.getElementById('<%=lblNote.ClientID %>').innerHTML = 'Note'
            document.getElementById('<%=lblTrackingNumber.ClientID %>').innerHTML = 'Tracking Number'
        }
    }

    function ConfirmTrackingNumber() {
        var defaultValue = document.getElementById("<%=hdDefaultvalue.ClientID %>");
        var drpShipping = document.getElementById("<%=drlShippingDetail.ClientID %>");
        var TrackingNumber = document.getElementById("<%=F_TrackingNumber.ClientID %>");
        var txtNote =  document.getElementById("<%=txtNote.ClientID %>");
        var TrackingID = <%=TrackingId %>;               
        if (defaultValue.value != "0" && TrackingNumber.value != '')
        {
            var bCheckOK = true;
            if (drpShipping.options[drpShipping.selectedIndex].value == '4')
            { 
                if (txtNote.value == '')
                {
                    bCheckOK = false;
                }
            }            
            if (defaultValue.value != drpShipping.options[drpShipping.selectedIndex].value && bCheckOK == true) {
                var msg ="The shipping detail selected is different with order shipping option. Are you sure?";
                if (TrackingID != '' && TrackingID != '0')
                {
                    msg ="Are you sure you want to change shipping detail?";
                }
                if (!confirm(msg)) {
                    return false;
                }
            }
        }

        return true;
    }
    </script>

</asp:Content>
