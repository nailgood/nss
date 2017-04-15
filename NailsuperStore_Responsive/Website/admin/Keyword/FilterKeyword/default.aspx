<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_Keyword_FilterKeyword_default" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>Keyword Redirect</h4>
    <p>
    </p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Filter Keyword" CssClass="btn">
    </CC:OneClickButton>
     <p>
    </p>
    <div>
        <asp:textbox runat="server" id="txtSearch"/>
        <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn">
    </CC:OneClickButton>
    <CC:OneClickButton ID="btnSynKwImport" runat="server" Text="Keyword import" CssClass="btn">
    </CC:OneClickButton>
    </div>
   
    
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links"
        EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
        BorderWidth="0" PagerSettings-Position="Bottom" SortOrder="Asc">
        <HeaderStyle VerticalAlign="Top"></HeaderStyle>
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & IIf(Not String.IsNullOrEmpty(txtSearch.Text), "&KwSearch=" + txtSearch.Text, "")%>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this keyword?"
                        runat="server" NavigateUrl='<%# "delete.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle Width="120px" />
                <HeaderTemplate>
                    <a href="#" onclick="Sort('KeywordName')">Keyword Name</a>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KeywordName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
                 <asp:TemplateField>
                <HeaderStyle Width="120px" />
                <HeaderTemplate>
                    <a href="#" onclick="Sort('OriginalKeyword')">Original Keyword</a>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OriginalKeyword") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:BoundField DataField="FilterType" HeaderStyle-Width="120px" HeaderText="Filter Type">
            </asp:BoundField>
            <asp:BoundField DataField="FilterProperty" HeaderStyle-Width="120px" HeaderText="Filter Property">
            </asp:BoundField>
            <asp:BoundField DataField="FilterValue" HeaderStyle-Width="120px" HeaderText="Filter Value">
            </asp:BoundField>
            
        </Columns>
    </CC:GridView>
    <input type="hidden" id="hidSearch" runat="server" name="hidSearch" value="" />
            <CC:OneClickButton ID="btnSort" runat="server" Text="Save" CssClass="btnHidden">
            </CC:OneClickButton>
            <input type="hidden" id="hidSortField" runat="server" />
    <script type="text/javascript">
                function MyCallback() {
                    if (document.getElementById('ctl00_ph_btnSearch')) {
                        document.getElementById('ctl00_ph_btnSearch').click();
                    }
                }
                function ClickSearch() {
                    document.getElementById('<%=hidSearch.ClientID %>').value = '1';
                }
                function Sort(field) {

                    document.getElementById('<%=hidSortField.ClientID %>').value = field;
                    var btn = document.getElementById('<%=btnSort.ClientID %>');
                    if (btn) {

                        btn.click();
                    }
                }
              
            </script>
</asp:Content>
