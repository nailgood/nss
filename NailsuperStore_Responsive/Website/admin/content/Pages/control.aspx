<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="control.aspx.vb" Inherits="admin_content_Pages_control" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
   <h4>List Control <%=ltrtitle%></h4>

   <asp:Panel ID="pnAddNew" runat="server">
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <th>
                Control
            </th>
            <td class="field" style="width: 150px">
                <asp:DropDownList ID="dlControl" runat="server"></asp:DropDownList>
            </td>
             <td>
                <asp:Label ID="lblControl" runat="server" CssClass="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                Param
            </th>
            <td class="field" style="width: 150px">
                <asp:TextBox ID="txtParam" Text="" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table><br />
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton id="btnBack" runat="server" Text="Back" cssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
     <CC:OneClickButton id="btnDelete" OnClientClick="return CheckDelete();" runat="server" Text="Delete" cssClass="btn"></CC:OneClickButton>
     <CC:OneClickButton id="btnActive" OnClientClick="return CheckActive();" runat="server" Text="Active" cssClass="btn"></CC:OneClickButton>
      <CC:OneClickButton id="btnDeActive" OnClientClick="return CheckActive();" runat="server" Text="DeActive" cssClass="btn"></CC:OneClickButton>
 </asp:Panel><br />

 <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">               
                <ItemTemplate>
                    <input type="checkbox" id="chk_<%#DataBinder.Eval(Container.DataItem, "PageRegionControlId")%>"
                        onclick="CheckItem(<%#DataBinder.Eval(Container.DataItem, "PageRegionControlId")%>,this.checked)" />
                </ItemTemplate>  
                 <HeaderTemplate>
                    <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this.checked)" />
                </HeaderTemplate>              
            </asp:TemplateField>          
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Controls">
                <ItemTemplate>
                    <asp:Literal ID="ltControls" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageRegionControlId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageRegionControlId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageRegionControlId")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageRegionControlId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <input type="hidden" value="" runat ="server" id="hidControlId" />
     <input type="hidden" value="" runat="server" id="hidIDSelect" />
    <script type="text/javascript">
        function CheckAll(status) {
            var id = document.getElementById('<%=hidControlId.ClientID %>').value;
           
            var arr = new Array();
            arr = id.split(',');
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
                idSelect += id + ',';
            }
            else idSelect = idSelect.replace(id + ',', '');
            if (idSelect.length == document.getElementById('<%=hidControlId.ClientID %>').value.length) {
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
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
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

