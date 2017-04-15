<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="pendingpoint.aspx.vb" Inherits="admin_members_PendingPoint" %>
<%@ Register src="~/controls/layout/pager.ascx" tagname="pager" tagprefix="uc1" %>
<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>
        Pending Points</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    <b>Member Type:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_MemberTypeId" runat="server" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Customer No:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_CustomerNo" runat="server" Width="131px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Username:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Username" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Email Address:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_EmailAddress" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Phone:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Phone" runat="server" Columns="50" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Address:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Address" runat="server" Columns="50" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Create Date:</b>
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="F_CreateDateLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_CreateDateUbound" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Customer Price Group Code:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList runat="server" ID="F_CustomerPriceGroupId" CssClass="smaller" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
    </p>
  
                <CC:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    BorderWidth="0" CellPadding="2" CellSpacing="2" EmptyDataText="There are no records that match the search criteria"
                    HeaderText="In order to change display order, please use header links" PagerSettings-Position="Bottom"
                    PageSize="50">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
                    <RowStyle CssClass="row" VerticalAlign="Top" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Member?"
                                    runat="server" NavigateUrl='<%# "delete.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="MemberType" DataField="MemberType" HeaderText="Member Type">
                        </asp:BoundField>
                        <asp:BoundField SortExpression="Username" DataField="Username" HeaderText="Username">
                        </asp:BoundField>
                        <asp:BoundField SortExpression="CreateDate" DataField="CreateDate" HeaderText="Create Date"
                            DataFormatString="{0:d}" HtmlEncode="False"></asp:BoundField>
                        <asp:BoundField DataField="ModifyDate" HeaderText="Modify Date" DataFormatString="{0:d}"
                            HtmlEncode="False"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                Password
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# string.Format("password.aspx?MemberId={0}&F_SortBy=Email&F_SortOrder=ASC", DataBinder.Eval(Container.DataItem, "MemberId")) %>'
                                    ImageUrl="/includes/theme-admin/images/preview.gif" ID="lnkPassword">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                Orders
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# string.Format("../store/orders/default.aspx?MemberId={0}", DataBinder.Eval(Container.DataItem, "MemberId")) %>'
                                    ImageUrl="/includes/theme-admin/images/preview.gif" ID="lnkOrder">Order</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="right">
                            <HeaderTemplate>
                                Pending Points
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrPendingPoint" runat="server"></asp:Literal>&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="right">
                            <HeaderTemplate>
                                Total Points
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrTotalPoint" runat="server"></asp:Literal>&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
       <asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>
