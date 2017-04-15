<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="item.aspx.vb" Inherits="admin_shopdesign_item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }
</script>
    <h4>List Item</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Item:
            </td>
            <td class="field">
                <input type="button" class="btn" id="btnSelectItem" value="Select Item" onclick="OpenPopUp();" runat="server" />
                <asp:TextBox ID="txtSku" runat="server" MaxLength="8" Columns="8" Enabled="false" Style="width: 200px;"></asp:TextBox>
            </td>
            <td>
               <asp:Label ID="lbError" runat="server" CssClass="red"></asp:Label><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtSku"
                    CssClass="msgError" ErrorMessage="Please add Item Shop By Design"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
               Qty Default:
            </td>
            <td class="field">
                <asp:TextBox ID="txtQty" runat="server" Text="1" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvMinimumAmount" runat="server" Display="Dynamic"
                    ControlToValidate="txtQty" CssClass="msgError" ErrorMessage="Field 'QtyDefault' is blank" ></asp:RequiredFieldValidator>
                    <CC:FloatValidator
                        Display="Dynamic" runat="server" ID="fvMinimumAmount" ControlToValidate="txtQty"
                        CssClass="msgError" ErrorMessage="Field 'QtyDefault' is invalid"  EnableClientScript="false" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Is Active?
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" />
            </td>
        </tr>
    </table>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False"></CC:OneClickButton>
    <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
    <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
    <input type="hidden" runat="server" value="" id="hidSaveValue" />
    <p></p>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="20"
                AllowPaging="True" AllowSorting="False" EmptyDataText="There are no records that match the search criteria"
                AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom" CausesValidation="false">
                <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                <Columns>
                    <asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="ItemName" >
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SKU">
                        <ItemTemplate>
                            <a href="/admin/store/items/edit.aspx?ItemId= <%#DataBinder.Eval(Container.DataItem, "ItemId")%>&F_SortBy=ItemId&F_SortOrder=ASC">
                                <%#DataBinder.Eval(Container.DataItem, "SKU")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField  ItemStyle-HorizontalAlign="Center" DataField="QtyDefault" HeaderText="QtyDefault" />
                    <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                        <ItemTemplate>
                            <a href="item.aspx?ShopDesignId=<%#DataBinder.Eval(Container.DataItem, "ShopDesignId") %>&ItemId=<%#DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All)%>">
                                <img src="/includes/theme-admin/images/edit.gif" style="border: none;" alt="" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId")%>' OnClientClick="return ConfirmDelete();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbUp" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId")%>' ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CausesValidation="false" />
                            <asp:ImageButton ID="imbDown" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId")%>' ImageUrl="/includes/theme-admin/images/MoveDown.gif" CausesValidation="false"
                                CommandName="Down" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </CC:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js"></script>
    <script type="text/javascript">
        function SetValue(save, value,isactive) {
            if (save == '1') {
                document.getElementById('<%=txtSku.ClientID %>').value = value;
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                if (isactive=='True')
                {
                    document.getElementById('<%=chkIsActive.ClientID %>').checked = true;
                }
                else
                {
                    document.getElementById('<%=chkIsActive.ClientID %>').checked = false;
                }
            }    
        }
        function OpenPopUp() {
            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var url = '/admin/promotions/ShopSave/SearchItem.aspx?Type=0&item=' + item;
           
            var brow = GetBrowser();
            if (brow == 'ie') {
                var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                document.getElementById('<%=txtSku.ClientID %>').value = p;
            }
            else {
                ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            }
            
        }
    </script>
</asp:Content>

