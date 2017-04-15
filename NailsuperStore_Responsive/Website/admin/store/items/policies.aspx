<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_store_items_policies" Title="List Policies" CodeFile="policies.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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
        <input type="button" class="btn" id="btnSearch" value="Add Policies" onclick="OpenPopUp();" />
        <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" ValidationGroup="val1"
            CausesValidation="False"></CC:OneClickButton>
        <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btnHidden">
        </CC:OneClickButton>
        <CC:OneClickButton ID="btnDelete" OnClientClick="return CheckDelete();" runat="server" Text="Delete" CssClass="btn"></CC:OneClickButton>
        <asp:Label ID="ltrMsg" runat="server" CssClass="red"></asp:Label>
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
                <br />
                <br />
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="20"
                    AllowPaging="True" AllowSorting="False" 
                    EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" 
                    BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <input type="checkbox" id="chk_<%#DataBinder.Eval(Container.DataItem, "PolicyId")%>"
                                    onclick="CheckItem(<%#DataBinder.Eval(Container.DataItem, "PolicyId")%>,this.checked)" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this.checked)" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="Title" DataField="Title" HeaderText="Title" >
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbActive" runat="server" 
                                    ImageUrl="/includes/theme-admin/images/active.png"
                                    CommandName="Active" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbDelete" runat="server" 
                                    ImageUrl="/includes/theme-admin/images/delete.gif"
                                    CommandName="Delete" OnClientClick="return ConfirmDelete();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbUp" runat="server"
                                    ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" />
                                <asp:ImageButton ID="imbDown" runat="server"
                                    ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                    CommandName="Down" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
                <input type="hidden" runat="server" value="" id="hidPopUpItemId" />
                 <input type="hidden" runat="server" value="" id="hidSaveValue" />
                <input type="hidden" value="" runat="server" id="hidIDSelect" />
                <input type="hidden" value="" runat="server" id="hidID" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divEmpty" runat="server" visible="false" style="padding: 10px">
            <i>List policies is empty</i></div>
    </div>
<script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
</script>
    <script type="text/javascript">
        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpItemId.ClientID%>').value = value;
            }

            document.getElementById('<%= hidSaveValue.ClientID%>').value = save;
            $("#<%= btnSave.ClientID %>").click();
        }
        function OpenPopUp() {

            var item = document.getElementById('<%=hidPopUpItemId.ClientID %>').value;
            var url = 'policies-addnew.aspx?itemId=' + <%= itemId %>
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
        
        function CheckDelete() {

            var id = document.getElementById('<%=hidIDSelect.ClientID %>').value;
            if (id == '') {
                alert('You must select one item');
                return false;
            }
            else return ConfirmDelete();
        }
        function CheckActive() {

            var id = document.getElementById('<%=hidIDSelect.ClientID %>').value;
            if (id == '') {
                alert('You must select one item');
                return false;
            }
           
        }
    </script>

</asp:Content>
