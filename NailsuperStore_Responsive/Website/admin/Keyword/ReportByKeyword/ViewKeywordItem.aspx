<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewKeywordItem.aspx.vb" Inherits="admin_KeyWord_ReportByKeyword_ViewKeywordItem" title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Item For Keyword</title>

    <script language="jscript">
        window.name = "modal";
    </script>

    <script type="text/javascript" src="/includes/theme-admin/scripts/Browser.js">
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
            <td align="left">
                <b>List item by keyword: <asp:Label ID="lbKeyword" runat="server"></asp:Label></b>
            </td>
        </tr>
        <tr>
            <td>

  <CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" 
            PageSize="40" AllowPaging="True" AllowSorting="True" 
            HeaderText="In order to change display order, please use header links" 
            EmptyDataText="There are no records that match the search criteria" 
            AutoGenerateColumns="False" BorderWidth="0px" PagerSettings-Position="Bottom" 
            CausesValidation="True" PageSelectIndex="0" 
            SortImageAsc="/includes/theme-admin/images/asc3.gif" SortImageDesc="/includes/theme-admin/images/desc3.gif" 
            SortOrder="DESC" SortBy="TotalPoint">                   
                        
        <HeaderStyle VerticalAlign="Top"></HeaderStyle>
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" />

            <Columns>               
                <asp:TemplateField HeaderText="SKU" SortExpression="SKU" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SKU") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Name" SortExpression="ItemName" >
                    <ItemTemplate>
                        <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ItemName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Point" SortExpression="TotalPoint" HeaderStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalPoint") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Detail" SortExpression="TotalDetail" HeaderStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalDetail") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Add Cart" SortExpression="TotalAddCart" HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalAddCart") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

<PagerSettings Position="Bottom"></PagerSettings>
                    
                        
    </CC:GridView>
    </td>
    </tr>
  </table>
</form>
</body>



</html>