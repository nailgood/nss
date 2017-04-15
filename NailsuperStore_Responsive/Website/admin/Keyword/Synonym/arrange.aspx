<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="arrange.aspx.vb" Inherits="admin_Keyword_Synonym_arrange" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script type="text/javascript">

        function Delete(keywordId, KeywordSynonymId) {

            var yes = confirm('Are you sure?');
            if (yes) {
                window.location.href = 'delete.aspx?KeywordId=' + keywordId + '&KeywordSynonymId=' + KeywordSynonymId;
            }
            else {
                return false
            }
        }


        //-->
    </script>
    <h4>
        Arrange Synonym for Keyword  '<asp:Literal ID="ltrKeywordName" runat="server"></asp:Literal>'
    </h4>
    <br>
    <a href="default.aspx?<%=params & IIf(String.IsNullOrEmpty(params), "SynonymType=" & SynonymType, "&SynonymType=" & SynonymType)%>">«« Go Back To Keyword List</a>
    <p>
        <p>
            <table cellpadding="0" cellspacing="0" border="0" id="tblList" runat="server">
                <tr>
                    <td width="435">
                        <CC:GridView ID="gvList" DataKeyNames="KeywordId" CellSpacing="2" CellPadding="2"
                            runat="server" PageSize="50" AllowPaging="false" AllowSorting="false" HeaderText=""
                            EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
                            PagerSettings-Position="Bottom" BorderWidth="1px" BorderColor="Black">
                            <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                            <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrDelete" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="KeywordSynonymName" HeaderStyle-Width="200px" HeaderText="Keyword">
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif"
                                            CommandName="Up" />
                                        <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                            CommandName="Down" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </CC:GridView>
                    </td>
                </tr>
                </table>
            <asp:PlaceHolder ID="plcNoRecords" runat="server" Visible="false">
                <p>
                There are currently no coordinating items for this item.</asp:PlaceHolder>
                 <asp:HiddenField ID="hidKeywordId" ClientIDMode="Static" runat="server" Value="" />
</asp:Content>
