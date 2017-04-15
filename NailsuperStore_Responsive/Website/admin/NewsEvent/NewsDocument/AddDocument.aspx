<%@ Page Language="VB" AutoEventWireup="false" Title="AddDocument" MasterPageFile="~/includes/masterpage/admin.master"
    CodeFile="AddDocument.aspx.vb" Inherits="admin_NewsEvent_NewsDocument_AddDocument" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content1" runat="server">
<script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js"></script>
    <script language="javascript">

//        function expandit(objid) {
//            var span = document.getElementById('SPAN' + objid).style;
//            var img = document.getElementById('IMG' + objid);
//            var imgtext = document.getElementById('imgtext');
//            if (span.display == "none") {
//                span.display = "block"
//                img.src = img.src.replace(/down/i, "up");
//                imgtext.innerText = 'Hide Image';
//            } else {
//                span.display = "none"
//                img.src = img.src.replace(/up/i, "down");
//                imgtext.innerText = 'View Image';
//            }
//        }
//
    </script>

    <span class="smaller">Please provide search criteria below</span>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Document Name:
            </th>
            <td valign="top" class="field">
              <asp:TextBox ID="F_DocumentName" runat="server" Columns="40" MaxLength="255"></asp:TextBox>
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
         </tr>
      
            <tr>
                <td colspan="4" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <CC:OneClickButton ID="btnClear" runat="server" Text="Clear" CssClass="btn" />
                </td>
            </tr>
    </table>
    <p>
    </p>
    <table>
        <tr>
            <td align="left" style="padding-bottom:5px">
                <input type="button" value="Save" class="btn" onclick="Save();" />
                <input type="button" value="Close" class="btn" onclick="ClosePopup();" />
            </td>
        </tr>
        <tr>
            <td>
                
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="10"
                    AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                               <asp:CheckBox ID="chk_DocId" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="Documentname" DataField="Documentname" HeaderText="Document name"></asp:BoundField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                            HeaderText="Is Active" />
                        </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" value="" id="hidDocSelect" />
    <input type="hidden" runat="server" value="" id="hidNewsId" />
    <script type="text/javascript">
        function CheckItem(id, status) {
            var idSelect = document.getElementById('<%=hidDocSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ';';
            }
            else idSelect = idSelect.replace(id + ';', '');
            document.getElementById('<%=hidDocSelect.ClientID %>').value = idSelect;
        }
        function CheckItemRadio(id, status) {         
            document.getElementById('<%=hidDocSelect.ClientID %>').value = id;
        }
        function ClosePopup() {            
            SetData('0', '');
            window.close();
        }
        function Save() {
            var id =  document.getElementById('<%=hidDocSelect.ClientID %>').value;
            if (id != '') {
                SetData('1', id);
                window.close();
            }
            else {
                alert('Please select at least a document!');
            }
        }
        function SetData(save, data) {
            var brow = GetBrowser();           
            if (brow == 'ie') {
                window.returnValue = id;
            }
            else {
                window.opener.SetValue(save, data)
            }
        }
    </script>

</asp:Content>
