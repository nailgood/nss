<%@ Page Title="" Language="VB"  AutoEventWireup="false" CodeFile="PopupItemRelatedVideo.aspx.vb" Inherits="admin_Video_Video_PopupItemRelatedVideo" %>
<%--MasterPageFile="~/includes/masterpage/admin.master" <asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server"></asp:Content>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script language="jscript">
        window.name = "modal";
    </script>

    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <link href="../../../includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
 <span class="smaller">Please provide search criteria below</span>
   <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Department:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_DepartmentId" runat="server" />
            </td>
            <th valign="top">
                Item Name:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_ItemName" runat="server" Columns="25" MaxLength="255"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th valign="top">
                Item Type:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_ItemType" runat="server">
                    <asp:ListItem Text="-- ALL --" Value="" />
                    <asp:ListItem Text="Single Items" Value="0" />
                    <asp:ListItem Text="Item Group Items" Value="1" />
                </asp:DropDownList>
            </td>
            <th valign="top">
                SKU:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_SKU" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
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
            <th valign="top">
                <b>Is Featured:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsFeatured" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th valign="top">
                <b>Brand:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_BrandId" runat="server" />
            </td>
            <th valign="top">
                <b>Is New:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsNew" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th valign="top">
                <b>Group Name:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_GroupName" runat="server" />
            </td>
            <th valign="top">
                <b>Has Sales Price?</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_HasSalesPrice" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th valign="top">
                <b>Is Free Sample:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsFreeSample" runat="server">
                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
            </td>
        </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" />
                </td>
            </tr>
    </table>
    <p></p>
    <table>
        <tr>
            <td align="left">
                <input type="button" value="Save" class="btn" onclick="Save();" />
                <input type="button" value="Close" class="btn" onclick="ClosePopup();" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="20"
                    AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_ItemId" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="SKU" DataField="SKU" HeaderText="SKU"></asp:BoundField>
                        <asp:TemplateField HeaderText="Group Name" SortExpression="GroupName">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litType" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="Item Name">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Price" SortExpression="Price">
                            <ItemTemplate>
                                <asp:Literal EnableViewState="False" runat="server" ID="ltlPrice" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department(s)">
                            <ItemTemplate>
                                <asp:Repeater ID="Departments" runat="server">
                                    <SeparatorTemplate>
                                        <br />
                                    </SeparatorTemplate>
                                    <ItemTemplate>
                                        <li style="list-style-image: url(/includes/theme-admin/images/minifolder.gif)">
                                            <%#Container.DataItem("NAME")%>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="BrandName" DataField="BrandName" HeaderText="Brand Name">
                        </asp:BoundField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                            HeaderText="Is Active" />
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsFeatured"
                            DataField="IsFeatured" HeaderText="Is Featured" />
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" value="" id="hidItemIdSelect" />
    
     <script type="text/javascript">
        function CheckItem(id, status) {
            var idSelect = document.getElementById('<%=hidItemIdSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ';';
            }
            else idSelect = idSelect.replace(id + ';', '');
            document.getElementById('<%=hidItemIdSelect.ClientID %>').value = idSelect;
        }
        function Save() {
            var id = document.getElementById('<%=hidItemIdSelect.ClientID %>').value;
            if (id != '') {
                SetParentData('1', id);         
                
            }
//            window.returnValue = id;
            window.close();
        }

        function SetParentData(save, data) {
            var brow = GetBrowser();
            if (brow == 'ie') {
                window.returnValue = data;
            }
            else {
                window.close();
                window.opener.SetValue('1', data)
            }
        }
        function ClosePopup() {
            window.close();
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


