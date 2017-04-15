<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_orders_reviews_Edit" Title="Item Reviews" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4> Order Review</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
       <tr>
            <td class="required">
                Order No:
            </td>
            <td class="field">
                <asp:HyperLink ID="hplOrder" runat="server"></asp:HyperLink>
            </td>
            <td>
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
               Rating:
            </td>
            <td class="field">
                <asp:DropDownList ID="drpStar" runat="server" >
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:DropDownList>
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
                <CC:RequiredDateValidator Display="Dynamic" runat="server" ID="rdtvDateAdded" ControlToValidate="dtDateAdded"
                    ErrorMessage="Date Field 'Date Added' is blank" CssClass="msgError" /><CC:DateValidator Display="Dynamic" CssClass="msgError"
                        runat="server" ID="dtvDateAdded" ControlToValidate="dtDateAdded" ErrorMessage="Date Field 'Date Added' is invalid" />
            </td>
        </tr>
        <tr>
            <td class="required">
            Item arrived on time:
            </td>
             <td class="field">
               <asp:DropDownList ID="drArrived" runat="server">
                   <asp:ListItem Value="1">Yes</asp:ListItem>
                   <asp:ListItem Value="0">No</asp:ListItem>
                 </asp:DropDownList> 
            </td>
            <td>
               
            </td>
        </tr>
          <tr>
            <td class="required">
            Item as describled:
            </td>
             <td class="field">
               <asp:DropDownList ID="drDescribled" runat="server">
                   <asp:ListItem Value="1">Yes</asp:ListItem>
                   <asp:ListItem Value="0">No</asp:ListItem>
                 </asp:DropDownList> 
            </td>
            <td>
               
            </td>
        </tr>
          <tr>
            <td class="required">
            Prompt and counteous service:
            </td>
             <td class="field">
               <asp:DropDownList ID="drService" runat="server">
                   <asp:ListItem Value="1">Yes</asp:ListItem>
                   <asp:ListItem Value="0">No</asp:ListItem>
                   <asp:ListItem Value="2">Did not contact</asp:ListItem>
                 </asp:DropDownList> 
            </td>
            <td>
               
            </td>
        </tr>
        <tr>
            <td class="required">
            Comment:
            </td>
             <td class="field">
             <textarea ID="txtComment" runat="server" style="width:500px;height:200px"></textarea>
            </td>
            <td>
               
            </td>
        </tr>
       
          <tr>
            <td class="field">
                <b>Is Post?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkPost"  onclick="CheckPost(this.checked);" />
            </td>
        </tr>
         <tr>
            <td class="required">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" onclick="CheckActive(this.checked);" />
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
        function CheckPost(check) {
            if (check) {
                document.getElementById('<%=chkIsActive.ClientID %>').checked = true;
            }

        }
        function CheckActive(check) {
            if (!check) {
                document.getElementById('<%=chkPost.ClientID %>').checked = false;
            }

        }
    </script>

</asp:Content>
