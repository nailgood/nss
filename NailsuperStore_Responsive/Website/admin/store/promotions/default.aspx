<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Store Promotion" CodeFile="default.aspx.vb" Inherits="Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Order Coupon</h4>
    <span class="smaller">Please provide search criteria below</span>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Promotion Name:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_PromotionName" runat="server" Columns="50" MaxLength="255"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th valign="top">
                Promotion Code:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_PromotionCode" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th valign="top">
                Promotion Type:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_drpPromotionType" runat="server">
                    <asp:ListItem Value="">ALL</asp:ListItem>
                    <asp:ListItem Value="Percent">Percent Off</asp:ListItem>
                    <asp:ListItem Value="Monetary">Dollar Off</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th valign="top">
                Customer Price Group:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_drlCustomerPriceGroup" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th valign="top">
                <b>Start Date:</b>
            </th>
            <td valign="top" class="field">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="smaller">
                            From
                            <CC:DatePicker ID="F_StartDateLbound" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="smaller">
                            To
                            <CC:DatePicker ID="F_StartDateUbound" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th valign="top">
                <b>End Date:</b>
            </th>
            <td valign="top" class="field">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="smaller">
                            From
                            <CC:DatePicker ID="F_EndDateLbound" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="smaller">
                            To
                            <CC:DatePicker ID="F_EndDateUbound" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr runat="server" visible="true">
            <th valign="top">
                <b>Is One Use:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsOneUse" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <th valign="top">
                <b>Is Used:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsUsed" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                <span class="smaller">single use only</span>
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
                <b>Is Register Send:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsRegisterSend" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="F_IsProductCoupon" runat="server" Visible="false">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0" Selected="true">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Store Promotion" CssClass="btn">
    </CC:OneClickButton>
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
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?PromotionId=" & DataBinder.Eval(Container.DataItem, "PromotionId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Store Promotion?"
                        runat="server" NavigateUrl='<%# "delete.aspx?PromotionId=" & DataBinder.Eval(Container.DataItem, "PromotionId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Button ID="btnRelatedItems" runat="server" CssClass="btn" Text="Items/Departments" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="PromotionName" DataField="PromotionName" HeaderText="Promotion Name">
            </asp:BoundField>
            <asp:BoundField SortExpression="PromotionCode" DataField="PromotionCode" HeaderText="Promotion Code">
            </asp:BoundField>
            <asp:BoundField SortExpression="PromotionType" DataField="PromotionType" HeaderText="Promotion Type">
            </asp:BoundField>
            <asp:TemplateField SortExpression="Discount" HeaderText="Discount">
                <ItemTemplate>
                    <asp:Literal ID="ltlDiscount" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="StartDate" DataField="StartDate" HeaderText="Start Date"
                DataFormatString="{0:d}" HtmlEncode="False"></asp:BoundField>
            <asp:BoundField SortExpression="EndDate" DataField="EndDate" HeaderText="End Date"
                DataFormatString="{0:d}" HtmlEncode="False"></asp:BoundField>
            <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                HeaderText="Is Active" />
            <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsRegisterSend"
                DataField="IsRegisterSend" HeaderText="Is Register Send" />
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>
