<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Sales Banner" CodeFile="default.aspx.vb" Inherits="admin_store_salesBanner_Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Flash Banner</h4>
    <p>
        <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Banner" CssClass="btn">
        </CC:OneClickButton></p>
    <table>
        <tr>
            <td style="width: 70px; text-align: right;">
                Department:
            </td>
            <td style="width: 210px; text-align: left;">
                <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="0">Home Page</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:CheckBox ID="chkActive" Checked=true runat="server" Text="Active" AutoPostBack="True" />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkRunning" Checked=true runat="server" Text="Running" AutoPostBack="True" />
            </td>
             
        </tr>
        <tr id="trEffect" runat="server" visible="false">
            <td style="text-align: right;">
                Slide Effect:
            </td>
            <td colspan="2">
                <asp:CheckBoxList ID="chkEffect" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Move" Value="move"></asp:ListItem>
                    <asp:ListItem Text="Slide" Value="move1"></asp:ListItem>
                    <asp:ListItem Text="pixel" Value="move2"></asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr id="trSaveEffect" runat="server" visible="false">
            <td>
            </td>
            <td colspan="2">
                <asp:Button ID="btnSaveEffect" runat="server" CssClass="btn" Text="Save Effect" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <CC:GridView ID="gvList"  DataKeyNames="Id" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
                    AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use arrows"
                    EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
                    PagerSettings-Position="Bottom" BorderWidth="1px" BorderColor="Black">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&departmentId="& ddlDepartment.SelectedValue & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <CC:ConfirmLink EnableViewState="False" Message="Are you sure?" runat="server" NavigateUrl='<%# "delete.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Category</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="litDepartment" runat="server"></asp:Literal></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Image</HeaderTemplate>
                            <ItemTemplate>
                                <img src='/assets/Banner/<%#Container.DataItem("BannerName")%>' alt="" height="100px" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" DataField="IsActive" HeaderText="Is Active" />
                        <asp:BoundField DataField="StartingDate" SortExpression="StartingDate" HeaderText="Start Date"
                            HtmlEncode="False"></asp:BoundField>
                        <asp:BoundField DataField="EndingDate" SortExpression="EndingDate" HeaderText="End Date"
                            HtmlEncode="False"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                            
                            <ItemTemplate >
                                 <asp:ImageButton ID="imbUp"   runat="server" ImageUrl="/images/admin/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItemIndex %>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/images/admin/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItemIndex %>" />
                
                                </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
