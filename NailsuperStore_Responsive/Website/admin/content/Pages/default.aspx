<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="default.aspx.vb" Inherits="admin_content_pages_index" Title="Content Tool Template" %>

<asp:Content ID="Content" ContentPlaceHolderID="ph" runat="Server">
    <h4>
        <font face="Arial">Website Page Administration</font></h4>
    <table>
        <tr>
            <th>
                URL
            </th>
            <td class="field">
                <asp:TextBox ID="F_PageURL" runat="server" Width="300px"></asp:TextBox>
        </tr>
        <tr>
            <th>
                Page Name
            </th>
            <td class="field">
                <asp:TextBox ID="F_Name" runat="server" Width="300px"></asp:TextBox>
                &nbsp;
                <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
            </td>
        </tr>
    </table>
    <p>
        <CC:OneClickButton ID="btnRegister" Text="Register Existing Page" runat="server"
            CssClass="btn"></CC:OneClickButton>
    </p>
    <table cellspacing="0" cellpadding="0" border="0" id="tblList" runat="server">
        <tr>
            <td>
                <asp:DataGrid ID="dgList" runat="server" AllowPaging="True" PageSize="20" AutoGenerateColumns="False"
                    CellPadding="2" CellSpacing="2" BorderWidth="0px">
                    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="row" VerticalAlign="top"></ItemStyle>
                    <HeaderStyle CssClass="header"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink ID="Hyperlink1" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif" NavigateUrl='<%# "/admin/content/Pages/register.aspx?PageId=" & DataBinder.Eval(Container.DataItem, "PageId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    EnableViewState="False">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this page?"
                                    runat="server" NavigateUrl='<%# "delete.aspx?PageId=" & DataBinder.Eval(Container.DataItem, "PageId") & "&" & params %>'
                                    ImageUrl="/includes/theme-admin/images/delete.gif" ID="Confirmlink1">Delete</CC:ConfirmLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="220" HeaderStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrName" runat="server"></asp:Literal>
                              <%--  <b><%# DataBinder.Eval(Container, "DataItem.Name") %></b><br />--%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                         
                        <asp:TemplateColumn  HeaderStyle-Width="320" HeaderStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Page URL
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrPageURL" runat="server"></asp:Literal>
                               <%-- <a target="_blank" href="<%# DataBinder.Eval(Container, "DataItem.PageURL") %>">
                                    <%# DataBinder.Eval(Container, "DataItem.PageURL") %></a>--%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                       <asp:TemplateColumn  HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Page Title
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrPageTitle" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn  HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Meta Keywords
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrMetaKeywords" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn  HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Meta Description
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrMetaDescription" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn  HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Left
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrLeft" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn  HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                Right
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrRight" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td>
                <CC:Navigator ID="myNavigator" runat="server"></CC:Navigator>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidCon" runat="server" />
    <p>
        <asp:PlaceHolder ID="plcNoRecords" runat="server" Visible="false">There are no records
            that mach the search criteria</asp:PlaceHolder>
    </p>
</asp:Content>
