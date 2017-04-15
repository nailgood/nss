<%@ Page Language="VB" AutoEventWireup="false" Inherits="admin_store_items_policies_new" CodeFile="~/admin/store/items/policies-addnew.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="jscript">
        window.name = "modal";
    </script>

    <script type="text/javascript" src="/includes/theme-admin/scripts/Browser.js">
    </script>

    <link href="/includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
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
                                <asp:CheckBox ID="chk_policySel" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="Title" DataField="Title" HeaderText="Title"></asp:BoundField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:Image ID="imbActive" runat="server" ImageUrl='<%# IIf(Boolean.Parse(CType(Container.DataItem, System.Data.DataRowView)("IsActive").ToString()), "/includes/theme-admin/images/active.png", "/includes/theme-admin/images/inactive.png")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" value="" id="hidPoliciesSelect" />
    <script type="text/javascript">
        function CheckItem(id, status) {
            var idSelect = document.getElementById('<%=hidPoliciesSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ';';
            }
            else idSelect = idSelect.replace(id + ';', '');

            document.getElementById('<%=hidPoliciesSelect.ClientID%>').value = idSelect;
        }
        function CheckItemRadio(id,isactive, status) {

            document.getElementById('<%=hidPoliciesSelect.ClientID%>').value = id;
        }


        function Save() {

            var id = document.getElementById('<%=hidPoliciesSelect.ClientID%>').value;
            if (id != '') {
                SetParentData('1', id);
                //window.opener.SetValue('1',id)           
                window.close();
            }
        }
        function SetParentData(save, data) {
            var brow = GetBrowser();
            if (brow == 'ie') {
                window.returnValue = data;
            }
            else {
                window.opener.SetValue('1', data);
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
