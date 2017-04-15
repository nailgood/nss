<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_graphic_InforBanner_Default" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Info Banner</h4>
    <p>
        <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Banner" CssClass="btn">
        </CC:OneClickButton></p>
    <table>
        <tr>
            <td colspan="3">
                <CC:GridView ID="gvList" DataKeyNames="Id" CellSpacing="2" CellPadding="2" runat="server"
                    PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use arrows"
                    EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
                    PagerSettings-Position="Bottom" BorderWidth="1px" BorderColor="Black">
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
                                <CC:ConfirmLink EnableViewState="False" Message="Are you sure?" runat="server" NavigateUrl='<%# "delete.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Name"></asp:BoundField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Image</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrImage" runat="server"></asp:Literal></ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" DataField="IsActive" HeaderText="Is Active" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif"
                                    CommandName="Up" />
                                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                    CommandName="Down" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
