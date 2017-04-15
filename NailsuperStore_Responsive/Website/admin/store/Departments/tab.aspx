<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tab.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_departments_tab" Title="Tab for Main Department" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div style="margin: 0 20px">
                <h4>
                    <asp:Literal ID="ltrHeader" runat="server" Text="List tabs"></asp:Literal></h4>
               
                <asp:Panel ID="pnList" runat="server">
                    Main Category
                    <asp:DropDownList ID="ddlDepartmentTab" runat="server" Width="150px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlDepartmentTab_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Repeater ID="rptDepartmentTab" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="1" cellspacing="1" border="0" style="border: solid 1px Black;
                                margin: 10px 0">
                                <tr style="height: 25px">
                                    <th style="width: 200px">
                                        Name
                                    </th>
                                    <th style="width: 60px">
                                        Products
                                    </th>
                                    <th style="width: 50px">
                                        Active
                                    </th>
                                    <th style="width: 50px">
                                        Edit
                                    </th>
                                    <th style="width: 50px">
                                        Delete
                                    </th>
                                    <th style="width: 60px">
                                        Arrange
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="height: 25px">
                                <td class="row">
                                    <%#Container.DataItem.Name%>
                                </td>
                                <td class="row" align="center">
                                    <a href="tabitem.aspx?DepartmentTabId=<%#Container.DataItem.DepartmentTabId%>&TabName=<%#Container.DataItem.Name%>&DepartmentName=<%#Container.DataItem.DepartmentName%>">
                                        <img src="/includes/theme-admin/images/Create.gif" style="border: 0px" /></a>
                                </td>
                                <td class="row" align="center">
                                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                                        CommandName="Active" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                </td>
                                <td class="row" align="center">
                                    <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif"
                                        CommandName="Edit" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                </td>
                                <td class="row" align="center">
                                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                        CommandName="Delete" CommandArgument="<%#Container.DataItem.DepartmentTabId%>"
                                        OnClientClick="return ConfirmDelete();" />
                                </td>
                                <td class="row" align="center">
                                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif"
                                        CommandName="Up" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                        CommandName="Down" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr style="height: 25px">
                                <td class="alternate">
                                    <%#Container.DataItem.Name%>
                                </td>
                                <td class="alternate" align="center">
                                    <a href="tabitem.aspx?DepartmentTabId=<%#Container.DataItem.DepartmentTabId%>&TabName=<%#Container.DataItem.Name%>&DepartmentName=<%#Container.DataItem.DepartmentName%>">
                                        <img src="/includes/theme-admin/images/Create.gif" style="border: 0px" /></a>
                                </td>
                                <td class="alternate" align="center">
                                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                                        CommandName="Active" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                </td>
                                <td class="alternate" align="center">
                                    <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif"
                                        CommandName="Edit" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                </td>
                                <td class="alternate" align="center">
                                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                        CommandName="Delete" CommandArgument="<%#Container.DataItem.DepartmentTabId%>"
                                        OnClientClick="return ConfirmDelete();" />
                                </td>
                                <td class="alternate" align="center">
                                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif"
                                        CommandName="Up" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                        CommandName="Down" CommandArgument="<%#Container.DataItem.DepartmentTabId%>" />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <div id="divEmpty" runat="server" visible="false" style="padding: 10px">
                        <i>List tab is empty</i></div>
                    <asp:HiddenField ID="hidId" runat="server" />
                    <CC:OneClickButton ID="btnAddNew" runat="server" Text="Add new" CssClass="btn" Visible="false">
                    </CC:OneClickButton>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
