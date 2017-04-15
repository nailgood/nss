<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="default.aspx.vb" Inherits="admin_AdminLog_Default" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <div style="margin: 0 20px">
       <br />
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    <b>Date:</b>
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="F_FromDate" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_ToDate" runat="server" />
                            </td>
                        </tr>
                    </table>
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
            <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
                AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                <Columns>
                    <asp:BoundField DataField="Username" HeaderStyle-Width="50" HeaderText="User name">
                    </asp:BoundField>
                    <asp:BoundField DataField="FullName" HeaderStyle-Width="150" HeaderText="Full name">
                    </asp:BoundField>
                    <asp:BoundField DataField="RemoteIP" HeaderStyle-Width="90" HeaderText="IP Address">
                    </asp:BoundField>
                    <asp:BoundField DataField="LoginDate" HeaderStyle-Width="120" HeaderText="Entry Date/Time">
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="400" HeaderText="Message">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "Message")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </CC:GridView>
    </div>
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
</asp:Content>
