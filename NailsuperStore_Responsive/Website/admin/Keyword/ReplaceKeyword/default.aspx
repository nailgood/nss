<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_Keyword_ReplaceKeyword_default" %>

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
        Keyword Replace
    </h4>
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
                    <td class="optional" valign="top" style="font-weight: bold" align="right">
                        Has Replace Keyword
                    </td>
                    <td class="field">
                        <asp:CheckBox ID="chkHasReplaceKeyword" runat="server" Checked="true" />
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top">
                        <p>
                            <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClientClick="ClickSearch();" />
                            <input class="btn" type="button" value="Clear" onclick="window.location='default.aspx';return false;" />
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
                                <asp:TemplateField HeaderText="Replace Keyword" HeaderStyle-Width="190px">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrReplaceKeyword" runat="server"></asp:Literal>
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
                function Sort(field) {

                    document.getElementById('<%=hidSortField.ClientID %>').value = field;
                    var btn = document.getElementById('<%=btnSort.ClientID %>');
                    if (btn) {

                        btn.click();
                    }
                }
              
            </script>
</asp:Content>
