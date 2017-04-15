<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PopupImage.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master"
    Inherits="admin_NewsEvent_News_PopupImage" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content1" runat="server">
    <span class="smaller">Please provide search criteria below</span>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Name:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_Name" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
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
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <CC:OneClickButton ID="btnClear" runat="server" Text="Clear" CssClass="btn" />
                </td>
            </tr>
    </table>
    <p>
    </p>
    <table>
        <tr>
            <td align="left">
                <input type="button" value="Save" class="btn" onclick="Save();" />
                <input type="button" value="Close" class="btn" onclick="ClosePopup();" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="5"
                    AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                    AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkImageId" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField SortExpression="ImageName" ItemStyle-Width="300" DataField="ImageName" HeaderText="Name">
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Image" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:Literal ID="litImage" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" DataField="IsActive"
                            HeaderText="Is Active" />
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" value="" id="hidImageSelect" />

    <script type="text/javascript">
        function CheckItem(id, status) {
            var idSelect = document.getElementById('<%=hidImageSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ';';
            }
            else idSelect = idSelect.replace(id + ';', '');
            document.getElementById('<%=hidImageSelect.ClientID %>').value = idSelect;
        }
        
        function ClosePopup() {
            window.close();
        }
        function Save() {
            var id = document.getElementById('<%=hidImageSelect.ClientID %>').value;
            window.returnValue = id;
            window.close();


        }
    </script>

</asp:Content>
