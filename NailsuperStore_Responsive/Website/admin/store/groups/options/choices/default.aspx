<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Group Option Choice" CodeFile="default.aspx.vb" Inherits="admin_store_groups_options_choices_Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Group Option Choice</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    <b>Option:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_OptionId" runat="server" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Choice Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_ChoiceName" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
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
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Group Option Choice"
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
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?ChoiceId=" & DataBinder.Eval(Container.DataItem, "ChoiceId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Group Option Choice?"
                        runat="server" NavigateUrl='<%# "delete.aspx?ChoiceId=" & DataBinder.Eval(Container.DataItem, "ChoiceId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="optionname" DataField="optionname" HeaderText="Option">
            </asp:BoundField>
            <asp:BoundField SortExpression="ChoiceName" DataField="ChoiceName" HeaderText="Choice Name">
            </asp:BoundField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Thumb Image</HeaderTemplate>
                <ItemTemplate>
                    <img src='/assets/groupchoice/<%#Container.DataItem("ThumbImage")%>' alt=""  /></ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
</asp:Content>
