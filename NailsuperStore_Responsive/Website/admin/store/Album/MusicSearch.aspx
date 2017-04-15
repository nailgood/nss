<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MusicSearch.aspx.vb" Inherits="admin_store_Album_MusicSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script language="jscript">
        window.name = "modal";
    </script>

    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <link href="../../../includes/admin.css" rel="stylesheet" type="text/css" />
</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
    <span class="smaller">Please provide search criteria below</span>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                <b>Name:</b>
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_Name" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <tr>
                <td colspan="4" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <CC:OneClickButton ID="btnClear" runat="server" Text="Clear" CssClass="btn" />
                </td>
            </tr>
    </table>
    <p>
    </p>
    <table>
        <tr>
            <td align="left">
                <input type="button" value="Save" class="btn" onclick="Save();" />
                <input type="button" value="Close" class="btn" onclick="ClosePopup();" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvListAlbum" CellSpacing="2" CellPadding="2" runat="server" PageSize="10"
                    AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%If Type = "1" Then%>
                                <asp:CheckBox ID="chk_Album" runat="server" />
                                <%End If %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                            HeaderText="Is Active" />
                    </Columns>
                </CC:GridView>
                <CC:GridView ID="gvListSong" CellSpacing="2" CellPadding="2" runat="server" PageSize="10"
                    AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%If Type = "2" Then%>
                                <asp:CheckBox ID="chk_Song" runat="server" />
                                <%End If %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                            HeaderText="Is Active" />
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" value="" id="hidMusicSelect" />

    <script type="text/javascript">
        function CheckItem(id, status) {

            var idSelect = document.getElementById('<%=hidMusicSelect.ClientID %>').value;
            if ((status) && (idSelect.indexOf(id) == -1)) {

                idSelect += id + ';';

            }
            else idSelect = idSelect.replace(id + ';', '');
            document.getElementById('<%=hidMusicSelect.ClientID %>').value = idSelect;
        }
        function CheckItemRadio(id, status) {

            document.getElementById('<%=hidMusicSelect.ClientID %>').value = id;
        }
        function ClosePopup() {
            var id = document.getElementById('<%=hidMusicSelect.ClientID %>').value;
            SetParentData('1',id);
            window.close();
        }
        function Save() {
            var id = document.getElementById('<%=hidMusicSelect.ClientID %>').value;
            SetParentData('1', id);
            window.close();
           

        }
        function SetParentData(save, data) {
            var brow = GetBrowser();
            if (brow == 'ie') {
                window.returnValue = data;
            }
            else {
                window.opener.SetValue('1', data)
            }
        }
        function CheckTarget() {
            var frm = document.getElementById('form1')

            if (frm) {
                var brow = GetBrowser();
                if (brow == 'ie') {
                    frm.target = "modal"
                }
                else frm.target = ""

            }
        }
    </script>

    </form>
</body>
</html>
