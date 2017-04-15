<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Mix Match" CodeFile="default.aspx.vb" Inherits="admin_navision_mixmatch_Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Mix Match Administration</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    Type:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_drlType" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="1">Public</asp:ListItem>
                        <asp:ListItem Value="2">Product Coupon</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Mix Match No:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_MixMatchNo" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Product:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Product" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Promotion:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Description" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Customer Price Group:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_CustomerPriceGroupId" runat="server" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Starting Date:</b>
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="F_StartingDateLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_StartingDateUbound" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Ending Date:</b>
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="F_EndingDateLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_EndingDateUbound" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Is Active:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_IsActive" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Discount Type:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_DiscountType" runat="server" Columns="15" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    SKU:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_SKU" runat="server" Columns="15" MaxLength="15"></asp:TextBox>
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
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Mix Match Promotion"
        CssClass="btn"></CC:OneClickButton>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links"
        EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
        BorderWidth="0" PagerSettings-Position="Bottom">
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
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Mix Match Promotion?"
                        runat="server" NavigateUrl='<%# "delete.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") &  "&"  & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "line/default.aspx?F_MixMatchId=" & DataBinder.Eval(Container.DataItem, "Id")%>'
                        ImageUrl="/includes/theme-admin/images/collection.gif" ID="lnkLines">Items</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                   <asp:Literal ID="ltrType" runat="server">
                   </asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="MixMatchNo" DataField="MixMatchNo" HeaderText="Mix Match No">
            </asp:BoundField>
              <asp:BoundField DataField="Product" HeaderText="Product"></asp:BoundField>
            <asp:BoundField DataField="Description" HeaderText="Promotion"></asp:BoundField>
            <asp:BoundField SortExpression="CustomerPriceGroupCode" DataField="CustomerPriceGroupCode"
                HeaderText="Customer Price Group"></asp:BoundField>
            <asp:BoundField SortExpression="DiscountType" DataField="DiscountType" HeaderText="Discount Type">
            </asp:BoundField>
            <asp:BoundField SortExpression="StartingDate" DataField="StartingDate" HeaderText="Starting Date"
                DataFormatString="{0:d}" HtmlEncode="False"></asp:BoundField>
            <asp:BoundField SortExpression="EndingDate" DataField="EndingDate" HeaderText="Ending Date"
                DataFormatString="{0:d}" HtmlEncode="False"></asp:BoundField>
            <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                HeaderText="Is Active" />
            
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>
