<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tabitem.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_departments_tabitem" Title="Item for Department Tab" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">

    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }

    </script>

    <div style="margin: 0 20px">
        <h4>
            <asp:Literal ID="ltrHeader" runat="server" Text="List items"></asp:Literal></h4>
        <input type="button" class="btn" id="btnSearch" value="Add SKU" onclick="OpenPopUp();" />
        <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btnHidden">
        </CC:OneClickButton>
        <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" ValidationGroup="val1"
            CausesValidation="False"></CC:OneClickButton>
        <asp:Label ID="ltrMsg" runat="server" CssClass="red"></asp:Label>
        <asp:Repeater ID="rptItem" runat="server">
            <HeaderTemplate>
                <table cellpadding="1" cellspacing="1" border="0" style="border: solid 1px Black;
                    margin: 10px 0">
                    <tr style="height: 25px">
                        <th style="width: 30px">
                            #
                        </th>
                        <th>
                            Item Name
                        </th>
                        <th style="width: 60px">
                            SKU
                        </th>
                        <th style="width: 50px">
                            Active
                        </th>
                        <th style="width: 50px">
                            Delete
                        </th>
                        <th style="width: 70px">
                            Arrange
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="height: 25px">
                    <td class="row" align="center">
                        <%#Container.ItemIndex + 1%>
                    </td>
                    <td class="row" style="padding: 0 5px">
                        <%#Container.DataItem.ItemName%>
                    </td>
                    <td class="row" align="center">
                        <a href="/admin/store/items/edit.aspx?ItemId=<%#Container.DataItem.ItemId%>&F_SortBy=ItemId&F_SortOrder=ASC">
                            <%#Container.DataItem.SKU%></a>
                    </td>
                    <td class="row" align="center">
                        <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                            CommandName="Active" CommandArgument="<%#Container.DataItem.ItemId%>" />
                    </td>
                    <td class="row" align="center">
                        <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                            CommandName="Delete" CommandArgument="<%#Container.DataItem.ItemId%>" />
                    </td>
                    <td class="row" align="center">
                        <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                            CommandArgument="<%#Container.DataItem.ItemId%>" />
                        <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                            CommandName="Down" CommandArgument="<%#Container.DataItem.ItemId%>" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr style="height: 25px">
                    <td class="alternate" align="center">
                        <%#Container.ItemIndex + 1%>
                    </td>
                    <td class="alternate" style="padding: 0 5px">
                        <%#Container.DataItem.ItemName%>
                    </td>
                    <td class="alternate" align="center">
                        <a href="/admin/store/items/edit.aspx?ItemId=<%#Container.DataItem.ItemId%>&F_SortBy=ItemId&F_SortOrder=ASC">
                            <%#Container.DataItem.SKU%></a>
                    </td>
                    <td class="alternate" align="center">
                        <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                            CommandName="Active" CommandArgument="<%#Container.DataItem.ItemId%>" />
                    </td>
                    <td class="alternate" align="center">
                        <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                            CommandName="Delete" CommandArgument="<%#Container.DataItem.ItemId%>" />
                    </td>
                    <td class="alternate" align="center">
                        <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                            CommandArgument="<%#Container.DataItem.ItemId%>" />
                        <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                            CommandName="Down" CommandArgument="<%#Container.DataItem.ItemId%>" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
        <div id="divEmpty" runat="server" visible="false" style="padding: 10px">
            <i>List product is empty</i></div>
        <CC:OneClickButton ID="btnAddNew" runat="server" Text="Add new" CssClass="btn" Visible="false">
        </CC:OneClickButton>
    </div>
    <input type="hidden" runat="server" value="" id="hidSaveValue" />

    <script type="text/javascript" src="/includes/theme-admin/scripts/Browser.js">
    </script>

    <script>
        function SetValue(save, value, isactive) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;
            $("#<%= btnSave.ClientID %>").click();
        }
        function OpenPopUp() {
            var brow = GetBrowser();
            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var url = '../../promotions/ShopSave/SearchItem.aspx?Type=1&F_HasSalesPrice=&item=' + item;
            ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
//            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');

//            if (brow == 'ie') {
//                if (typeof p != "undefined") {
//                    if (p != '') {
//                        document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
//                        var button = document.getElementById('<%=btnSave.ClientID %>');
//                        if (button)
//                            button.click();
//                    }
//                }
//            }
//            else {
//                var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;
//                if (saveValue == '1') {
//                    var button = document.getElementById('<%=btnSave.ClientID %>');
//                    if (button)
//                        button.click();
//                }
//            }
        }
    </script>

</asp:Content>
