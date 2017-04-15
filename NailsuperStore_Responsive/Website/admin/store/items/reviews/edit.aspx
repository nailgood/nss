<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_items_reviews_Edit" Title="Item Reviews" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If ReviewId = 0 Then %>Add<% Else %>Edit<% End If %>
        Item Review</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Item Id:
            </td>
            <td class="field">
                <asp:DropDownList ID="drpItemId" runat="server" Width="500px" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvItemId" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="drpItemId"
                    ErrorMessage="Field 'Item Id' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                UserName:
            </td>
            <td class="field">
                <asp:HyperLink ID="hplEmail" runat="server"></asp:HyperLink>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="required">
                Date Added:
            </td>
            <td class="field">
                <asp:Label runat="server" ID="lblDateAdded"></asp:Label>
            </td>
            <td>
                <CC:RequiredDateValidator Display="Dynamic" CssClass="msgError" runat="server" ID="rdtvDateAdded" ControlToValidate="dtDateAdded"
                    ErrorMessage="Date Field 'Date Added' is blank" /><CC:DateValidator Display="Dynamic" CssClass="msgError"
                        runat="server" ID="dtvDateAdded" ControlToValidate="dtDateAdded" ErrorMessage="Date Field 'Date Added' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" />
            </td>
        </tr>
        
         <tr>
            <td class="field">
                <b>The Nail Superstore replied</b>
            </td>
            <td class="field">
               <asp:TextBox ID="txtAdminReply" TextMode="MultiLine" runat="server" Width="500px" Height="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field">
                <b>Edit Comment</b>
            </td>
            <td class="field">
                <a href="/store/review/product-write.aspx?ReviewId=<%=ReviewId %>&ItemId=<%=Itemid %>">
                    Click to edit comment</a>
            </td>
        </tr>
        <% if transExists then %>
        <tr>
            <td class="field">
                <b>Cash Point Transaction</b>
            </td>
            <td class="field">
                <asp:Literal ID="ltrCastPointLink" runat="server"></asp:Literal>
            </td>
        </tr>
        <%end if %>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:ConfirmButton ID="btnDelete" runat="server" Message="Are you sure want to delete this Item Review?"
        Text="Delete" CssClass="btn" CausesValidation="False"></CC:ConfirmButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>

    <script language="javascript">
        function ShowEidt() {
            document.getElementById("divEditcm").style.display = 'block';
            document.getElementById("dShow").style.display = 'none';
            document.getElementById("dhide").style.display = 'block';
        }
        function HideEidt() {
            document.getElementById("divEditcm").style.display = 'none';
            document.getElementById("dShow").style.display = 'block';
            document.getElementById("dhide").style.display = 'none';
        }
    </script>

</asp:Content>
