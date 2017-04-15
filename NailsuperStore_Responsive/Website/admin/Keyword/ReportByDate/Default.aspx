<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_KeyWord_ReportByDate_Default"
    MasterPageFile="~/includes/masterpage/admin.master" %>

<%@ Register Src="~/controls/layout/pager.ascx" TagName="pager" TagPrefix="uc1" %>
<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <script type="text/javascript">
        function OpenPopUp(date) {
            var url = 'GetDataByKeyword.aspx?d=' + date;
            ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
        }
        function ExportByDate(date) {
            var hid = document.getElementById("hidExport");
            hid.value = date;
            document.getElementById('<%=btnExport.ClientID %>').click();
            hid.value = "";
        }
    </script>
    <h4>
        Daily Search Activity</h4>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th style="text-align: right;">
                    Starting Date:
                </th>
                <td class="field">
                    <CC:DatePicker ID="dtpFromDate" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="text-align: right;">
                    Ending Date:
                </th>
                <td class="field">
                    <CC:DatePicker ID="dtpToDate" runat="server" />
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
    <br />
    <div>
        <table>
            <tr>
                <td>
                    <uc1:pager ID="pagerTop" PageSize="36" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                        OnPageIndexChanging="pagerTop_PageIndexChanging" />
                </td>
            </tr>
            <tr>
                <td>
                    <CC:GridView ID="gvList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        SortOrder="DESC" AllowPaging="False" BorderWidth="0" CellPadding="2" CellSpacing="2"
                        EmptyDataText="There are no records that match the search criteria" HeaderText=""
                        PagerSettings-Position="Bottom" PageSize="30">
                        <HeaderStyle VerticalAlign="Top"></HeaderStyle>
                        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
                        <RowStyle CssClass="row" VerticalAlign="Top" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle Width="100px" />
                                <HeaderTemplate>
                                    <a href="#" onclick="Sort('SearchedDate')">Date</a>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle Width="100px" />
                                <HeaderTemplate>
                                    <a href="#" onclick="Sort('[dbo].[fc_Keyword_GetTotalSearchByDate](SearchedDate)')">
                                        Total Search</a>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TotalSearch") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Literal ID="ltrDetail" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Literal ID="ltrExport" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
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
    </div>
    <CC:OneClickButton ID="btnExport" runat="server" Text="Export All" CssClass="btn" />
    <input type="hidden" value="" id="hidExport" name="hidExport" />
    <asp:HiddenField ID="hidFromDate" runat="server" Value="" />
    <asp:HiddenField ID="hidToDate" runat="server" Value="" />
    <CC:OneClickButton ID="btnSort" runat="server" Text="Save" CssClass="btnHidden">
    </CC:OneClickButton>
    <input type="hidden" id="hidSortField" runat="server" />
    <script type="text/javascript">
        f
        function Sort(field) {

            document.getElementById('<%=hidSortField.ClientID %>').value = field;
            var btn = document.getElementById('<%=btnSort.ClientID %>');
            if (btn) {

                btn.click();
            }
        }
              
    </script>
</asp:Content>
