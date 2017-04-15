<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_Keyword_RedirectKeyword_default" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>Keyword Redirect</h4>
    <p>
    </p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Redirect Keyword" CssClass="btn">
    </CC:OneClickButton>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links"
        EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
        BorderWidth="0" PagerSettings-Position="Bottom" SortOrder="Asc">
        <HeaderStyle VerticalAlign="Top"></HeaderStyle>
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this keyword?"
                        runat="server" NavigateUrl='<%# "delete.aspx?id=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="KeywordName" HeaderStyle-Width="120px" HeaderText="Keyword Name">
            </asp:BoundField>
            <asp:BoundField DataField="LinkRedirect" HeaderStyle-Width="120px" HeaderText="Link">
            </asp:BoundField>
             <asp:BoundField DataField="Description" HeaderStyle-Width="140px" HeaderText="Description">
            </asp:BoundField>
        </Columns>
    </CC:GridView>
</asp:Content>
