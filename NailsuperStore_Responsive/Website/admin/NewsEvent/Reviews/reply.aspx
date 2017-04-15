<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="reply.aspx.vb" Inherits="admin_NewsEvent_Reviews_reply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
    <h4> NewsEvents Reviews Reply</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
           
            <tr >
                <th valign="top">
                    News :
                </th>
                <td valign="top" class="field" style="width:500px;">
                   <asp:Label ID="lblNews" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                <th valign="top">
                    Comment:
                </th>
                <td valign="top" class="field">
                   <asp:Label ID="lblComment" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th >
                    Username:
                </th>
                <td  class="field">
                    <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th >
                    Customer no:
                </th>
                <td  class="field">
                    <asp:TextBox ID="txtCustomerNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Date:
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="dtpDateLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="dtpDateRbound" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th >
                    Status:
                </th>
                <td  class="field">
                    <asp:DropDownList ID="drlStatus" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="0">Un-Approved</asp:ListItem>
                        <asp:ListItem Value="2">Added Point</asp:ListItem>
                        <asp:ListItem Value="1">Actived</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <CC:OneClickButton ID="btnreset" runat="server" Text="Clear" CssClass="btn" />
                     <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
    </p>
    <CC:OneClickButton ID="AddNew" Visible="false" runat="server" Text="Add New Item Review"
        CssClass="btn"></CC:OneClickButton>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="User Reply" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Literal ID="ltrReviewName" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText="Comment Reply" ControlStyle-CssClass="" ItemStyle-Width="370">
                <ItemTemplate>
                    <asp:Literal ID="ltrComment" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Like" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:Literal ID="ltrLike" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:BoundField SortExpression="CreatedDate" ItemStyle-Width="130" DataField="CreatedDate"
                HeaderText="Date" HtmlEncode="False" ItemStyle-HorizontalAlign="Right">
            </asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" HeaderText="Points" ItemStyle-Width="60">
                <ItemTemplate>
                    <asp:ImageButton ID="imbPoint" runat="server" ImageUrl="/includes/theme-admin/images/plus.png" CommandName="AddPoint"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>' />
                    <asp:Literal ID="ltrPoint" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField ItemStyle-HorizontalAlign="right">
                <ItemTemplate>
                    <a href="edit.aspx?ReviewId=<%#DataBinder.Eval(Container.DataItem, "ReviewId") & "&ParentReviewId=" & DataBinder.Eval(Container.DataItem, "ParentReviewId") %>">
                        <img src="/includes/theme-admin/images/edit.gif" style="border: none;" /></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right">
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Item Review?"
                        runat="server" NavigateUrl='<%# "delete.aspx?ReviewId=" & DataBinder.Eval(Container.DataItem, "ReviewId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
</asp:Content>

