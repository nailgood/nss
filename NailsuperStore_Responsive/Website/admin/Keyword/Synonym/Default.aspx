<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_Keyword_Synonym_Default" %>

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
            InitQueryCode2('<%=txtKeyword.ClientID.Replace("_", "$") %>', '/admin/ajax.aspx?f=SuggestAdminKeywordSynonym&q=', 'varKeyword', s);
        }
    </script>
    <h4><%=dblSynonymType.SelectedItem.Text  %></h4>
    <p>
        <table cellspacing="2" cellpadding="3" border="0">
            <tbody>
                <tr>
                    <td class="optional" valign="top" style="font-weight: bold" align="right">
                        Keyword
                    </td>
                    <td class="field" width="400" style="padding-left: 6px;">
                        <asp:TextBox ID="txtKeyword" runat="server" Columns="30" MaxLength="200"></asp:TextBox>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top">
                        <asp:RadioButtonList runat="server" AutoPostBack="true" ID="dblSynonymType" 
                            OnSelectedIndexChanged="dblSynonymType_CheckedChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Keyword Synonym" Selected="True" value=""/>
                            <asp:ListItem Text="Tone Synonym" Value="tone" />
                            <asp:ListItem Text="Buy In Bulk" Value="buyinbulk" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top">
                        <p>
                            <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClientClick="ClickSearch();" />
                            <CC:OneClickButton ID="btnAddnew" runat="server" Text="Add new" CssClass="btn" />
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
                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KeywordName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="One way keyword" HeaderStyle-Width="190px">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrOneWay" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Round trip keyword" HeaderStyle-Width="190px">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrRoundtrip" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrArrange" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                <ItemTemplate>
                                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this keyword?"
                                        runat="server" NavigateUrl='<%# "delete.aspx?KeywordId=" & DataBinder.Eval(Container.DataItem, "KeywordId") & "&SynonymType=" & dblSynonymType.SelectedValue %>'
                                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                                </ItemTemplate>
            </asp:TemplateField>
                            </Columns>
                            <PagerSettings Position="Bottom" />
                        </CC:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:pager ID="pagerBottom" PageSize="36" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
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
                function Sort(field) {

                    document.getElementById('<%=hidSortField.ClientID %>').value = field;
                    var btn = document.getElementById('<%=btnSort.ClientID %>');
                    if (btn) {

                        btn.click();
                    }
                }
                function Addnew() {
                    var keyword = document.getElementById('<%=txtKeyword.ClientID %>').value;
                    if (keyword == '') {
                        alert('Please input keyword');
                        return false;
                    }
                    return true;
                }
              
            </script>
</asp:Content>
