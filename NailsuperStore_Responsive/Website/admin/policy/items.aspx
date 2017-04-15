<%@ Page Language="VB" AutoEventWireup="false" CodeFile="items.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_PolicyItem" Title="List Policy Item" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }
    </script>

<div style="margin: 0 20px">
    <h4><asp:Literal ID="ltrHeader" runat="server" Text="List items"></asp:Literal></h4>
    
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up">
            <ProgressTemplate>
                <div class="UpdateProgress">
                    <center>
                        Please wait...<br />
                        <img src="/includes/theme/images/loader.gif" alt="Waiting" />
                    </center>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    <input type="button" class="btn" id="btnSearch" value="Add SKU" onclick="OpenPopUp();" />
    <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
    <CC:OneClickButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn" Visible="false"></CC:OneClickButton>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btnHidden"></CC:OneClickButton>       
    <asp:Label ID="ltrMsg" runat="server" CssClass="red"></asp:Label>
    <br /><br />

            
        <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="99"
            AllowPaging="False" AllowSorting="False" EmptyDataText="There are no records that match the search criteria"
            AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
            <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
            <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
            <Columns>
                <asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="ItemName" >
                </asp:BoundField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SKU">
                    <ItemTemplate>
                        <a href="/admin/store/items/edit.aspx?ItemId= <%#DataBinder.Eval(Container.DataItem, "ItemId")%>&F_SortBy=ItemId&F_SortOrder=ASC"><%#DataBinder.Eval(Container.DataItem, "SKU")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                    <ItemTemplate>
                        <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" OnClientClick="return ConfirmDelete();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                    <ItemTemplate>
                        <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" />
                        <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </CC:GridView>

        <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
        <input type="hidden" runat="server" value="" id="hidSaveValue" />
        <input type="hidden" value="" runat="server" id="hidIDSelect" />
        <input type="hidden" value="" runat="server" id="hidID" />


    <div id="divEmpty" runat="server" visible="false" style="padding: 10px">List policy item is empty</div>
    <CC:OneClickButton ID="btnAddNew" runat="server" Text="Add new" CssClass="btn" Visible="false"></CC:OneClickButton>
</div>

<script type="text/javascript" src="/includes/theme-admin/scripts/Browser.js"></script>
<script type="text/javascript">
    function SetValue(save, value, isactive) {
        if (save == '1') {
            document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
        }
        document.getElementById('<%=hidSaveValue.ClientID %>').value = save;
        $("#<%= btnSave.ClientID %>").click();
    }
    function OpenPopUp() {

        var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
        var url = 'SearchItem.aspx?Type=1&F_HasSalesPrice=&item=' + item
        ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
    }
    function CheckAll(status) {
        var id = document.getElementById('<%=hidID.ClientID %>').value;
        var arr = new Array();
        arr = id.split(';');
        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                if (document.getElementById('chk_' + arr[i].toString())) {
                    document.getElementById('chk_' + arr[i].toString()).checked = status;

                }
            }

        }
        if (status) {
            document.getElementById('<%=hidIDSelect.ClientID %>').value = id;
        }
        else document.getElementById('<%=hidIDSelect.ClientID %>').value = '';

    }
    function CheckItem(id, status) {

        var idSelect = document.getElementById('<%=hidIDSelect.ClientID %>').value;
        if (status) {
            idSelect += id + ';';
        }
        else idSelect = idSelect.replace(id + ';', '');
        if (idSelect.length == document.getElementById('<%=hidID.ClientID %>').value.length) {
            document.getElementById('chkCheckAll').checked = true;
        }
        else
            document.getElementById('chkCheckAll').checked = false;
        document.getElementById('<%=hidIDSelect.ClientID %>').value = idSelect;

    }
</script>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
