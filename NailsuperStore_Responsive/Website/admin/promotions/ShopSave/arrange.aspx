<%@ Page Language="VB" AutoEventWireup="false" CodeFile="arrange.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_shopsave_arange"  Title="Shop Save Now" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>

<div style="margin:0 20px">
<h4 style="margin-bottom:0px">Arrange Homepage Tab</h4>
<asp:Panel ID="pnList" runat="server">

<asp:Repeater ID="rptShopSave" runat="server">
    <HeaderTemplate>
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:10px 0">
            <tr style="height:25px">
                <th style="width:200px">Name</th>
                 <th style="width:50px">Active</th>
                  <th style="width:110px">Homepage Tab</th>
                <th style="width:50px">Arrange</th>

            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row"><%#Container.DataItem.Name%></td>
             <td class="row" align="center"><asp:ImageButton ID="imbActive"  runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active"   CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             <td class="row" align="center"><asp:ImageButton ID="imbActiveHomeTab"  runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="ActiveHomeTab"  CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             
            <td class="row" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="height:25px">
            <td class="alternate"><%#Container.DataItem.Name%></td>
             <td class="alternate" align="center"><asp:ImageButton ID="imbActive"   runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             <td class="alternate" align="center"><asp:ImageButton ID="imbActiveHomeTab"   runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="ActiveHomeTab" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             
            <td class="alternate" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

<h4 style="margin-bottom:0px">Arrange Shop Now Menu</h4>
<asp:Repeater ID="rptShopNow" runat="server">
    <HeaderTemplate>
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:10px 0">
            <tr style="height:25px">
                <th style="width:200px">Name</th>
                 <th style="width:50px">Active</th>
                  <th style="width:110px">Homepage Tab</th>
                <th style="width:50px">Arrange</th>

            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row"><%#Container.DataItem.Name%></td>
             <td class="row" align="center"><asp:ImageButton ID="imbActive" runat="server"  ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             <td class="row" align="center"><asp:ImageButton ID="imbActiveHomeTab" runat="server"  ImageUrl="/includes/theme-admin/images/active.png" CommandName="ActiveHomeTab" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
            
            <td class="row" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="height:25px">
            <td class="alternate"><%#Container.DataItem.Name%></td>
             <td class="alternate" align="center"><asp:ImageButton ID="imbActive"  runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             <td class="alternate" align="center"><asp:ImageButton ID="imbActiveHomeTab"  runat="server" ImageUrl="/includes/theme-admin/images/active.png" CommandName="ActiveHomeTab" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
            
            <td class="alternate" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>



<h4 style="margin-bottom:0px">Arrange Save Now Menu</h4>
<asp:Repeater ID="rptSaveNow" runat="server">
    <HeaderTemplate>
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:10px 0">
            <tr style="height:25px">
                <th style="width:200px">Name</th>
                 <th style="width:50px">Active</th>
                  <th style="width:110px">Homepage Tab</th>
                <th style="width:50px">Arrange</th>

            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row"><%#Container.DataItem.Name%></td>
             <td class="row" align="center"><asp:ImageButton ID="imbActive" runat="server"  ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             <td class="row" align="center"><asp:ImageButton ID="imbActiveHomeTab" runat="server"  ImageUrl="/includes/theme-admin/images/active.png" CommandName="ActiveHomeTab" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
            
            <td class="row" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="height:25px">
            <td class="alternate"><%#Container.DataItem.Name%></td>
             <td class="alternate" align="center"><asp:ImageButton ID="imbActive" runat="server"  ImageUrl="/includes/theme-admin/images/active.png" CommandName="Active" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
             <td class="alternate" align="center"><asp:ImageButton ID="imbActiveHomeTab" runat="server"  ImageUrl="/includes/theme-admin/images/active.png" CommandName="ActiveHomeTab" CommandArgument="<%#Container.DataItem.ShopSaveId%>" /></td>
            
            <td class="alternate" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ShopSaveId%>" />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:content>

