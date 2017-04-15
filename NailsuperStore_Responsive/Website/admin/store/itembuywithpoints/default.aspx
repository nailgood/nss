<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_store_itembuywithpoints_default" %>
<%@ Register TagPrefix="uc1" TagName="pager" Src="~/controls/layout/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>Item Buy With Points</h4>    
     
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
         <%--       <asp:DropDownList ID="drpItemId" runat="server" Visible="false" />--%>
                <input type="button" class="btn" id="Button1" value="Select Item" onclick="OpenPopUp();" /><asp:TextBox
                    ID="txtSku" runat="server" MaxLength="8" Columns="8" Enabled="false" Style="width: 67px;"></asp:TextBox>
            </td>
            <td>
            <%--  <CC:IntegerValidator ID="IntegerValidator2" runat="server" ControlToValidate="txtSku"
                    CssClass="msgError" ErrorMessage="Please add Item Point" Display="Dynamic" />--%>
               <asp:Label ID="lbError" runat="server" CssClass="red"></asp:Label><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtSku"
                    CssClass="msgError" ErrorMessage="Please add Item Free Gift"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Reward Points:
            </td>
            <td class="field">
                <asp:TextBox ID="txtpoint" runat="server" MaxLength="8" Columns="8" Style="width: 67px;"></asp:TextBox>
            </td>
            <td>
            <%-- <CC:IntegerValidator ID="IntegerValidator1" runat="server" ControlToValidate="txtpoint"
                    CssClass="msgError" ErrorMessage="Field 'RewardPoints' is invalid" Display="Dynamic" />--%>
                <asp:RequiredFieldValidator ID="rfvMinimumAmount" runat="server" Display="Dynamic"
                    ControlToValidate="txtpoint" CssClass="msgError" ErrorMessage="Field 'RewardPoints' is blank" ></asp:RequiredFieldValidator>
                    <CC:FloatValidator
                        Display="Dynamic" runat="server" ID="fvMinimumAmount" ControlToValidate="txtpoint"
                        CssClass="msgError" ErrorMessage="Field 'RewardPoints' is invalid"  EnableClientScript="false" />
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
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>    
     <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
    <input type="hidden" runat="server" value="" id="hidItemId" />
    <table>
        <tr>
            <td>
                <uc1:pager ID="pagerTop" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                    OnPageIndexChanging="pagerTop_PageIndexChanging" />
            </td>
        </tr>
        <tr>
            <td>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>            
               <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links"
        EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
        BorderWidth="0" PagerSettings-Position="Bottom"  SortOrder="Asc" CausesValidation="false">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "default.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Image?"
                        runat="server" NavigateUrl='<%# "delete.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SKU" SortExpression="SKU" HeaderText="SKU" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Item Name" />
            <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="QtyOnHand" SortExpression="QtyOnHand" HeaderText="Qty" />
            <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="RewardPoints" SortExpression="RewardPoints" HeaderText="Reward Points" />
            <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                HeaderText="Is Active" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"  CausesValidation="false"/>
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CausesValidation="false"
                        CommandName="Down" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
                 <asp:HiddenField ID="hidCon" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <uc1:pager ID="pagerBottom" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                    OnPageIndexChanging="pagerTop_PageIndexChanging" />
            </td>
        </tr>
    </table>
    <CC:OneClickButton ID="btnSort" runat="server" Text="Save" CssClass="btnHidden">
    </CC:OneClickButton>
    <input type="hidden" id="hidSortField" runat="server" />

    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js"></script>
     <script>
        function OpenPopUp() {
            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var itemid = document.getElementById('<%=hidItemId.ClientID %>').value;
            var sku = document.getElementById('<%=txtSKU.ClientID %>').value;
            var url = '/admin/promotions/ShopSave/SearchItem.aspx?Type=0&item=' + item
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            var brow = GetBrowser();
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '') {
                        document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                        document.getElementById('<%=txtSKU.ClientID %>').value = p;
                        document.getElementById('<%=chkIsActive.ClientID %>').checked = p;
                    }
                }
            }
        }
        function SetValue(save, value, isactive) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                document.getElementById('<%=txtSKU.ClientID %>').value = value;
                if (isactive == "True") {
                    document.getElementById('<%=chkIsActive.ClientID %>').checked = true;
                }
                else {
                    document.getElementById('<%=chkIsActive.ClientID %>').checked = false;
                }
            }
        }
  </script>
</asp:Content>

