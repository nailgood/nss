<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="" CodeFile="edit.aspx.vb" Inherits="admin_store_orders_edit" %>

<%@ Register TagName="OrderDetail" TagPrefix="CC" Src="~/controls/product/order-detail.ascx" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script type="text/javascript">
        function ViewCustomer(id) {
            window.location.href = '/admin/members/edit.aspx?MemberId=' + id;
        }
        function EditTracking(OrderId, TrackingId) {
            
            window.location.href = '/admin/store/orders/AddTrackingNumber.aspx?OrderId=' + OrderId + '&TrackingId=' + TrackingId;
        }
        function AddTracking(OrderId) {
            window.location.href = '/admin/store/orders/AddTrackingNumber.aspx?OrderId=' + OrderId;
        }
    </script>
    <h4>
        Store Order Administration - View Order#
        <asp:Literal runat="server" ID="litOrderNo"></asp:Literal>
    </h4>
    <iframe width="100%" style="margin-left: 0px;" src="/members/orderhistory/view.aspx?OrderId=<%=OrderId %>&admin=1"
        scrolling="no" id="frOrderDetail" onload="AutoHeightFrame('frOrderDetail');"
        height="1px" frameborder="0"></iframe>
    <p>
        <asp:Button ID="btnCancel" runat="server" Text="Return to Order List" CssClass="btn"
            CausesValidation="False" />
        &nbsp;&nbsp;
        <asp:Button ID="btnReExport" runat="server" Text="Re-Export" CssClass="btn" OnClientClick="return ConfirmMsg();" />
    </p>
    <div id="dvMsg">
        <asp:Literal runat="server" ID="ltrMsgError"></asp:Literal></div>
    <table runat="server" id="tblExport" visible="false" width="480px;">
        <tr>
            <th style="width: 220px;">
                Header File
            </th>
            <th style="width: 220px;">
                Cart Item File
            </th>
        </tr>
        <tr style="min-height: 30px; vertical-align: top;">
            <td>
                <asp:LinkButton OnClientClick="clearMsgError();" runat="server" ID="lbHeaderFile"></asp:LinkButton>
                <span runat="server" id="spPendingHeader" visible="false" style="font-style: italic;">
                    (pending)</span>
            </td>
            <td>
                <asp:LinkButton OnClientClick="clearMsgError();" runat="server" ID="lbCartItemFile"></asp:LinkButton>
                <span runat="server" id="spPendingCartItem" visible="false" style="font-style: italic;">
                    (pending)</span>
            </td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rptNoteFile">
        <HeaderTemplate>
            <table width="480px;">
                <tr runat="server" id="trNote">
                    <th style="width: 220px;">
                        Old Header File
                    </th>
                    <th style="width: 220px;">
                        Old Cart Item File
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:LinkButton OnClientClick="clearMsgError();" CommandName="DownloadHeader" runat="server"
                        ID="lbNoteHeaderFile"></asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton OnClientClick="clearMsgError();" CommandName="DownloadCartItem" runat="server"
                        ID="lbNoteCartItemFile"></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <script type="text/javascript">
        function ConfirmMsg() {
            if (!confirm("Are you sure re-export this order?")) {
                return false;
            }
            return true;
        }

        function clearMsgError() {
            var dvMsg = document.getElementById("dvMsg");
            dvMsg.innerHTML = '';
            return false;
        }
    </script>
</asp:Content>
