<%@ Page Language="VB" AutoEventWireup="false" CodeFile="searchmixmatch.aspx.vb"
    Inherits="admin_store_productpromotions_searchmixmatch" %>

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
    <form id="form2" runat="server">
    <span class="smaller">Please provide search criteria below</span>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Mixmatch No:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="txtMixmatchno" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                </td>
            </tr>
    </table>
    <p>
    </p>
    <table>
        <tr>
            <td align="left">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" OnClientClick="return Save();" />
                <input type="button" value="Close" class="btn" onclick="ClosePopup();" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="20"
                    SortBy="MixMatchNo" AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <input type="radio" <%#getRadioSelect(Eval("MixMatchNo").toString()) %> onclick="CheckItemRadio('<%#Container.DataItem("MixMatchNo")%>',this.checked);"
                                    name="SelectSigle" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="MixMatchNo" DataField="MixMatchNo" HeaderText="Mix Match No">
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description"></asp:BoundField>
                        <asp:BoundField SortExpression="StartingDate" DataField="StartingDate" HeaderText="Starting Date" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField SortExpression="EndingDate" DataField="EndingDate" HeaderText="Ending Date" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
	
                        <asp:BoundField SortExpression="DiscountType" DataField="DiscountType" HeaderText="Discount Type">
                        </asp:BoundField>
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" value="" id="hidMMSelect" />

    <script type="text/javascript">
        function CheckItem(id, status) {
            var idSelect = document.getElementById('<%=hidMMSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ';';
            }
            else idSelect = idSelect.replace(id + ';', '');
            document.getElementById('<%=hidMMSelect.ClientID %>').value = idSelect;
        }
        function CheckItemRadio(id, status) {

            document.getElementById('<%=hidMMSelect.ClientID %>').value = id;
        }
        function ClosePopup() {
            SetParentData('0', '');
            window.close();
        }

        function Save() {

            var id = document.getElementById('<%=hidMMSelect.ClientID %>').value;
            if (id != '') {
                SetParentData('1', id);
                //window.opener.SetValue('1',id)           
                window.close();
            }
            return false;
        }
        function SetParentData(save, data) {
            var brow = GetBrowser();

            if (brow == 'ie') {
                window.returnValue = data;
            }
            else {
                window.opener.SetValueMM('1', data)
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
