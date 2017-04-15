<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="admin_KeyWord_ReportByKeyword_Default" Title="Untitled Page" %>

<%@ Register Src="~/controls/layout/pager.ascx" TagName="pager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ph" runat="Server">
    <script type="text/javascript">
        if (window.addEventListener) {
            window.addEventListener('load', InitializeQuery, false);
        } else if (window.attachEvent) {
            window.attachEvent('onload', InitializeQuery);
        }
        function InitializeQuery() {
            var s = document.getElementById('<%=hidSearch.ClientID %>').value;
            InitQueryCode2('<%=txtKeyword.ClientID.Replace("_", "$") %>', '/admin/ajax.aspx?f=DisplayKeyword&q=', 'varKeyword', s);
        }
    </script>
    <h4>
        Keyword Search</h4>
    <p>
        <table cellspacing="2" cellpadding="3" border="0">
            <tbody>
                <tr>
                    <td class="optional" valign="top" style="font-weight: bold">
                        Keyword
                    </td>
                    <td class="field" width="400">
                        <asp:TextBox ID="txtKeyword" runat="server" Columns="30" MaxLength="200"></asp:TextBox>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top">
                        <p>
                            <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClientClick="ClickSearch();" />
                            <input class="btn" type="button" value="Clear" onclick="window.location='default.aspx';return false;" />
                            <CC:OneClickButton ID="btnExportExcel" runat="server" Text="Export Excel" CssClass="btn"/>
                            <CC:OneClickButton ID="btnExportExcelNoResult" runat="server" Text="Export Excel Keyword No Result" CssClass="btn"/>
                            <br /><br />
                            <asp:HyperLink id="lnkDownload" runat="server"></asp:HyperLink>
                             <br />
                            <asp:HyperLink id="lnkDownloadKwNoResult" runat="server"></asp:HyperLink>
                    </td>
                </tr>
            </tbody>
        </table>
        <p>
            <table>
                <tr>
                    <td>
                        <uc1:pager ID="pagerTop" PageSize="36" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                            OnPageIndexChanging="pagerTop_PageIndexChanging" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" AllowPaging="false"
                            AllowSorting="True" HeaderText="In order to change display order, please use header links"
                            EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
                            BorderWidth="0px" PagerSettings-Position="Bottom" CausesValidation="True" PageSelectIndex="0"
                            SortImageAsc="/includes/theme-admin/images/asc3.gif" SortImageDesc="/includes/theme-admin/images/desc3.gif"
                            SortOrder="ASC" SortBy="KeywordId" PageSize="40">
                            <HeaderStyle VerticalAlign="Top"></HeaderStyle>
                            <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
                            <RowStyle CssClass="row" VerticalAlign="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle Width="200px" />
                                    <HeaderTemplate>
                                        <a href="#" onclick="Sort('KeywordName')">Keyword Name</a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblKeywordName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KeywordName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle />
                                    <HeaderTemplate>
                                        <a href="#" onclick="Sort('TotalSearch')">Total Search</a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalSearch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalSearch") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle />
                                    <HeaderTemplate>
                                        <a href="#" onclick="Sort('TotalDetail')">Total Detail</a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalDetail" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalDetail") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle />
                                    <HeaderTemplate>
                                        <a href="#" onclick="Sort('TotalAddCart')">Total AddCart</a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAddCart" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalAddCart") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle />
                                    <HeaderTemplate>
                                        <a href="#" onclick="Sort('TotalPoint')">Total Point </a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalPoint") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPopUpView('<%# DataBinder.Eval(Container, "DataItem.KeywordId") %>')">
                                            View Items</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a style="cursor: pointer;" onclick="OpenPopUpDetail('<%# DataBinder.Eval(Container, "DataItem.KeywordId") %>')">
                                            Detail</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Position="Bottom" />
                        </CC:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:pager ID="pagerBottom" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                            OnPageIndexChanging="pagerTop_PageIndexChanging" />
                    </td>
                </tr>
            </table>
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
                function OpenPopUpView(id) {
                    var url = 'ViewKeywordItem.aspx?Id=' + id
                    ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');

                }
                function OpenPopUpDetail(id) {
                    var url = 'KeywordDetail.aspx?Id=' + id
                    ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
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
