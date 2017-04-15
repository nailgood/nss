<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_PolicyDefault" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

</script>

<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="List Policy"></asp:Literal></h4>

<asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>

<asp:Repeater ID="rptPolicy" runat="server">
    <HeaderTemplate>
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:10px 0">
            <tr style="height:25px">
                <th style="width:200px">Name</th>
                <th style="width:60px">Products</th>
                <th style="width:50px">Active</th>
                <th style="width:50px">Edit</th>
                <th style="width:50px">Delete</th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row"><asp:Literal ID="litName" runat="server"></asp:Literal></td>
            <td class="row" align="center"><a href="items.aspx?id=<%#Container.DataItem.PolicyId%>"><img src="/includes/theme-admin/images/Create.gif" style="border:0px" /></a></td>
            <td class="row" align="center"><asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.PolicyId%>" /></td>
            <td class="row" align="center"><a href="edit.aspx?id=<%#Container.DataItem.PolicyId%>"><img src="/includes/theme-admin/images/edit.gif" /></a></td>
            <td class="row" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.PolicyId%>" OnClientClick="return ConfirmDelete();" /></td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="height:25px">
            <td class="alternate"><asp:Literal ID="litName" runat="server"></asp:Literal></td>
            <td class="alternate" align="center"><a href="items.aspx?id=<%#Container.DataItem.PolicyId%>"><img src="/includes/theme-admin/images/Create.gif" style="border:0px" /></a></td>
            <td class="alternate" align="center"><asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.PolicyId%>" /></td>
            <td class="alternate" align="center"><a href="edit.aspx?id=<%#Container.DataItem.PolicyId%>"><img src="/includes/theme-admin/images/edit.gif" /></a></td>
            <td class="alternate" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.PolicyId%>" OnClientClick="return ConfirmDelete();" /></td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

    </ContentTemplate>
</asp:UpdatePanel>
<CC:OneClickButton id="btnAddNew" runat="server" Text="Add new" cssClass="btn"></CC:OneClickButton>

</div>
</asp:content>


