<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_Keyword_ExcludeSearchIP_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }
    </script>
    <asp:Panel ID="pAddNew" runat="server">
        <h4>IP Exclude</h4>
         <table border="0" cellspacing="1" cellpadding="2">
            <tr>
                <td colspan="2">
                    <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
                </td>
            </tr>
            <tr>
                <td class="required">
                    IP:
                </td>
                <td class="field">
                    <asp:TextBox ID="txtIP" runat="server"  Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvIP" runat="server" Display="Dynamic" ControlToValidate="txtIP"  CssClass="msgError"
                        ErrorMessage="Field 'IP' is blank"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <p></p>
        <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
        <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False"></CC:OneClickButton>
    </asp:Panel>
    <p></p>
    <asp:Panel ID="plist" runat="server">
         <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server"
                EmptyDataText="There are no records that match the search criteria"
                AutoGenerateColumns="False" BorderWidth="0">
                <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="STT">
                        <ItemTemplate>
                            <asp:Literal ID="index" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IP" HeaderStyle-Width="250" HeaderText="IP"></asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                        <ItemTemplate>
                            <a href="default.aspx?Id=<%#DataBinder.Eval(Container.DataItem, "Id") %>">
                            <img src="/includes/theme-admin/images/edit.gif" style="border: none;" alt="" /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'
                                OnClientClick="return ConfirmDelete();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </CC:GridView>
    </asp:Panel>
</asp:Content>

