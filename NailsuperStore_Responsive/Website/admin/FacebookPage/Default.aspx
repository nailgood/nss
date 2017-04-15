<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="admin_FacebookPage_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" runat="Server">

    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }

    </script>

    <h4>
        Facebook/Twitter</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">Title:</th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Title" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
    </p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New" CssClass="btn">
    </CC:OneClickButton>
    <CC:OneClickButton ID="PostFaceBook"   OnClientClick="return CheckPostData();" runat="server" Text="Post to Facebook/Twitter" CssClass="btn">
    </CC:OneClickButton>
    <p>
   
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText=""  ItemStyle-Width="20px">
                <HeaderTemplate>
                 <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this.checked)" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input type="checkbox" id="chkSelect_<%# DataBinder.Eval(Container.DataItem, "PageId")%>"  name="chkSelect_<%# DataBinder.Eval(Container.DataItem, "PageId")%>" onclick="CheckItem('<%#Eval("PageId") %>',this.checked);" />
                </ItemTemplate>
                </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="" ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?id=" & DataBinder.Eval(Container.DataItem, "PageId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="" ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PageId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PageTitle" HeaderStyle-Width="250" HeaderText="Title">
            </asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Link copy to Facebook/Twitter" ItemStyle-Width="400px">
                <ItemTemplate>
                   <asp:TextBox ID="txtLink" runat="server" ReadOnly="true" Width="98%" ></asp:TextBox>
                </ItemTemplate>
                </asp:TemplateField>
            
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
    <input type="hidden" id="hidId"  runat="server" value="" />
    <input type="hidden" id="hidSelect"  runat="server" value="" />
    <script type="text/javascript">
        function CheckAll(status) {
            var id = document.getElementById('<%=hidId.ClientID %>').value;
            var arr = new Array();
            arr = id.split(',');
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    if (document.getElementById('chkSelect_' + arr[i].toString())) {
                        document.getElementById('chkSelect_' + arr[i].toString()).checked = status;

                    }
                }

            }
            if (status) {
                document.getElementById('<%=hidSelect.ClientID %>').value = id;
            }
            else document.getElementById('<%=hidSelect.ClientID %>').value = '';

        }
        function CheckItem(id, status) {

            var idSelect = document.getElementById('<%=hidSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ',';
            }
            else idSelect = idSelect.replace(id + ',', '');
            if (idSelect.length == document.getElementById('<%=hidId.ClientID %>').value.length) {
                document.getElementById('chkCheckAll').checked = true;
            }
            else
                document.getElementById('chkCheckAll').checked = false;
            document.getElementById('<%=hidSelect.ClientID %>').value = idSelect;

        }
        function CheckPostData() {
            var idSelect = document.getElementById('<%=hidSelect.ClientID %>').value;
            if (idSelect == '') {
                alert('Please select data post to Facebook/Twitter');
                return false;
            }
            else
            return true;
        }
    </script>
</asp:Content>
