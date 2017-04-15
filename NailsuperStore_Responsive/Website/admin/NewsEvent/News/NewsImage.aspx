<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewsImage.aspx.vb" Inherits="admin_NewsEvent_News_NewsImage" MasterPageFile="~/includes/masterpage/admin.master"  %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

</script>

<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="List items"></asp:Literal></h4>



<input type="button" class="btn" id="btnSearch" value="Add Image" onclick="OpenPopUp();" />
<CC:OneClickButton id="btnBack" runat="server" Text="Back" cssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btnHidden"></CC:OneClickButton>

<asp:Label ID="ltrMsg" runat="server" CssClass="red"></asp:Label>

<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>

<asp:Repeater ID="rptItem" runat="server">
    <HeaderTemplate>
        <div style="width:608px;text-align:right"><asp:Button id="btnSaveArrange" runat="server" Text="Save Arrange" CssClass="btn" CommandName="SaveArrange" Visible="false"></asp:Button></div>
        
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:5px 0">
            <tr style="height:25px">
                <th style="width:30px">#</th>
                <th>Image Name</th>
                <th style="width:60px">Image</th>
                <th style="width:50px">Active</th>
                <th style="width:50px">Delete</th>
                <th style="width:70px">Arrange</th>                
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row" align="center"><%#Container.ItemIndex + 1%></td>
            <td class="row" style="padding:0 5px"><%#Container.DataItem.ImageName%></td>
            <td class="row" align="center"><a href="/admin/NewsEvent/Image/edit.aspx?Id=<%#Container.DataItem.ImageId%>"><asp:Literal ID="ltrImage" runat="server"></asp:Literal> </a></td>
            <td class="row" align="center"><asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.Id%>" /></td>
            <td class="row" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.Id%>" OnClientClick="return ConfirmDelete();" /></td>
           
             <td class="row" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.Id%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.Id%>" />
            </td>
        </tr>
    </ItemTemplate>
   <AlternatingItemTemplate>
     <tr style="height:25px">
            <td class="alternate" align="center"><%#Container.ItemIndex + 1%></td>
            <td class="alternate" style="padding:0 5px"><%#Container.DataItem.ImageName%></td>
            <td class="alternate" align="center"><a href="/admin/NewsEvent/Image/edit.aspx?Id=<%#Container.DataItem.ImageId%>"><asp:Literal ID="ltrImage" runat="server"></asp:Literal> </a></td>
            <td class="alternate" align="center"><asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.Id%>" /></td>
            <td class="alternate" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.Id%>" OnClientClick="return ConfirmDelete();" /></td>
           
             <td class="alternate" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.Id%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.Id%>" />
            </td>
        </tr>
   </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

 <input type="hidden" runat="server" value="" id="hidPopUpImageId" />
</ContentTemplate>
</asp:UpdatePanel>

<div id="divEmpty" runat="server" visible="false" style="padding:10px"><i>List image is empty</i></div>
<CC:OneClickButton id="btnAddNew" runat="server" Text="Add new" cssClass="btn" Visible="false"></CC:OneClickButton>

</div>

<script>
    function OpenPopUp() {       
        var item = document.getElementById('<%=hidPopUpImageId.ClientID %>').value;
        var url = 'PopupImage.aspx?item=' + item
        var p = window.showModalDialog(url, '', 'dialogHeight: 700px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
        document.getElementById('<%=hidPopUpImageId.ClientID %>').value = p;
       
        var button = document.getElementById('<%=btnSave.ClientID %>');     
        if (button)
            button.click();
    }
</script>
</asp:content>

