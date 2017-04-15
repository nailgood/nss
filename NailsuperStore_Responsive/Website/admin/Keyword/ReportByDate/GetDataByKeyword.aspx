<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GetDataByKeyword.aspx.vb"
    Inherits="admin_KeyWord_ReportByDate_GetDataByKeyword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script language="jscript">
        window.name = "modal";
    </script>
    <link href="/includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
    <table>
        <tr>
            <td align="left">               
                <input type="button" value="Close" class="btn" onclick="window.close();" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    AllowPaging="true" BorderWidth="0" CellPadding="2" CellSpacing="2" EmptyDataText="There are no records that match the search criteria"
                    HeaderText="" PagerSettings-Position="Bottom" PageSize="40">
                    <HeaderStyle VerticalAlign="Top"></HeaderStyle>
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
                    <RowStyle CssClass="row" VerticalAlign="Top" />
                    <Columns>
                        <asp:BoundField DataField="KeywordName"  SortExpression="KeywordName"
                            HeaderText="Keyword"></asp:BoundField>
                        <asp:BoundField DataField="TotalSearch" SortExpression="TotalSearch" HeaderText="Total Search"></asp:BoundField>
                      
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
