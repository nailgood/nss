<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Item Image" CodeFile="default.aspx.vb" Inherits="admin_store_items_images_Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Additional Item Images</h4>
    <p>
        Images for <b>
            <% =dbStoreItem.ItemName%></b> | <a href="/admin/store/items/default.aspx?<%= GetPageParams(Components.FilterFieldType.All,"F_ItemId") %>">
                &laquo; Go Back To Store Item List</a>
    </p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Alternate Image" CssClass="btn">
    </CC:OneClickButton>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use arrows"
        EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
        BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?ImageId=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Item Image?"
                        runat="server" NavigateUrl='<%# "delete.aspx?ImageId=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <img alt="<%# DataBinder.Eval(Container.DataItem, "ImageAltTag")%>" src="/assets/items/cart/<%# DataBinder.Eval(Container.DataItem, "Image")%>"
                        border="0" />
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
</asp:Content>
