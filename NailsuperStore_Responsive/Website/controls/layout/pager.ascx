<%@ Control Language="VB" AutoEventWireup="false" CodeFile="pager.ascx.vb" Inherits="controls_layout_pager" %>

<div id="pager">
    <%  If ViewMode = 1 Then
            If ShowTwoLine = True Then
    %>
    <table border="0" class="pagingStyle2" id="tblParentPostBack2Line" runat="server">
            <tr class="pagingRow">
                <td width="auto">
                </td>
                <td align="right" valign="middle" class="viewallss">
                    <asp:LinkButton ID="lbtnViewAllPostBack2Line" runat="server" Text="View All" CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnViewPagingPostBack2Line" runat="server" Text="View Paging"
                        CausesValidation="false"></asp:LinkButton>
                </td>
                <td align="center" valign="middle" class="pageline">
                </td>
                <td align="left" valign="middle" class="pagesizeText">
                    Items per Page:
                </td>
                <td class="pagesizeControlTd">
                    <asp:DropDownList ID="drlPageSizePostBack2Line" AutoPostBack="true" runat="server" CausesValidation="false">
                     <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                        <asp:ListItem Value="24">24</asp:ListItem>
                        <asp:ListItem Value="36">36</asp:ListItem>
                        <asp:ListItem Value="72">72</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="pagingRow">
                <td width="auto">
                </td>
                <td colspan="4" align="right">
                    <table class="tblSubPaging" id="tblSubPagingShow2Line" runat="server">
                        <tr>
                            <td width="auto">
                            </td>
                            <td align="center" class="totalpages" id="totalPageTextShow2Line" runat="server">
                            </td>
                            <td align="center" valign="middle" class="pageline1">
                            </td>
                            <td align="left" class="prev">
                                <asp:LinkButton ID="lbtnPrevPostBack2Line" runat="server" Text="« Previous" CausesValidation="false"></asp:LinkButton>
                            </td>
                            <td align="left" class="pageindexTextTd" id="tdPageIndexPostBack2Line" runat="server">
                                <asp:LinkButton ID="lbtnPageIndexPostBack2Line_1" runat="server" OnClick="PageIndex_Click"
                                    Text="1"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnPageIndexPostBack2Line_2" runat="server" OnClick="PageIndex_Click"
                                    Text="2"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnPageIndexPostBack2Line_3" runat="server" OnClick="PageIndex_Click"
                                    Text="3"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnPageIndexPostBack2Line_4" runat="server" OnClick="PageIndex_Click"
                                    Text="4"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnPageIndexPostBack2Line_5" runat="server" OnClick="PageIndex_Click"
                                    Text="5"></asp:LinkButton>
                            </td>
                            <td align="right" class="next">
                                <asp:LinkButton ID="lbtnNextPostBack2Line" runat="server" Text="Next »" CausesValidation="false"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
    </table>
    <%
    Else
    %>
    <table border="0" class="pagingStyle2" id="tblParentPostBack" runat="server">

            <tr class="pagingRow">
                <td width="auto">
                </td>
                <td align="right" valign="middle" class="viewallss">
                    <asp:LinkButton ID="lbtnViewAllPostBack" runat="server" Text="View All" CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnViewPagingPostBack" runat="server" Text="View Paging" CausesValidation="false"></asp:LinkButton>
                </td>
                <td align="center" valign="middle" class="pageline">
                </td>
                <td align="left" valign="middle" class="pagesizeText">
                    Items per Page:
                </td>
                <td class="pagesizeControlTd">
                    <asp:DropDownList ID="drlPageSizePostBack" AutoPostBack="true" runat="server" CausesValidation="false">
                        <asp:ListItem Value="4">4</asp:ListItem>                        
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                        <asp:ListItem Value="24">24</asp:ListItem>
                        <asp:ListItem Value="36">36</asp:ListItem>
                        <asp:ListItem Value="72">72</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="center" class="totalpages">
                </td>
                <td align="center" valign="middle" class="pageline1">
                    
                </td>
                <td align="left" class="prev">
                    <asp:LinkButton ID="lbtnPrevPostBack" runat="server" Text="«" CausesValidation="false"></asp:LinkButton>
                </td>
                <td align="left" class="pageindexTextTd" id="tdPageIndexPostBack" runat="server">
                    <asp:LinkButton ID="lbtnPageIndexPostBack_1" runat="server" Text="1" OnClick="PageIndex_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnPageIndexPostBack_2" OnClick="PageIndex_Click" runat="server"
                        Text="2"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnPageIndexPostBack_3" OnClick="PageIndex_Click" runat="server"
                        Text="3"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnPageIndexPostBack_4" OnClick="PageIndex_Click" runat="server"
                        Text="4"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnPageIndexPostBack_5" OnClick="PageIndex_Click" runat="server"
                        Text="5"></asp:LinkButton>
                </td>
                <td align="right" class="next">
                    <asp:LinkButton ID="lbtnNextPostBack" runat="server" Text="»" CausesValidation="false"></asp:LinkButton>
                </td>
            </tr>

    </table>
    <%  End If
        
    %>
    <%
    ElseIf Not ShowTwoLine Then
    %>
    <table border="0" class="pagingStyle2" id="tblParentQuery" runat="server">
            <tr class="pagingRow">
                <td width="auto">
                </td>
                <td align="right" valign="middle" class="viewallss" id="tdViewAllQuery">
                    <a href="#">View All</a>
                </td>
                <td align="center" valign="middle" class="pageline">
                </td>
                <td align="left" valign="middle" class="pagesizeText">
                    Items per Page:
                </td>
                <td class="pagesizeControlTd">
                    <asp:DropDownList ID="drlPageSizeQuery" AutoPostBack="true" runat="server" CausesValidation="false">
                      <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                        <asp:ListItem Value="24">24</asp:ListItem>
                        <asp:ListItem Value="36">36</asp:ListItem>
                        <asp:ListItem Value="72">72</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="center" class="totalpages">
                </td>
                <td align="center" valign="middle" class="pageline1">
                </td>
                <td align="left" class="prev" id="tdPrevQuery" runat="server">
                    Prev
                </td>
                <td align="left" class="pageindexTextTd" id="tdPageIndexQuery" runat="server">
                </td>
                <td align="right" class="next" id="tdNextQuery" runat="server">
                </td>
            </tr>

    </table>
    <%  Else
    %>
    <table border="0" class="pagingStyle2" id="tblParentQueryShow2Line1" runat="server" align="right">

            <tr class="pagingRow">
                <td width="auto">
                </td>
                <td align="right" valign="middle" class="viewallss" id="tdViewAllQueryShow2Line" runat="server">
                    <a href="#">View All</a>
                </td>
                <td align="center" valign="middle" class="pageline">
                </td>
                <td align="left" valign="middle" class="pagesizeText">
                    Items per Page:
                </td>
                <td class="pagesizeControlTd">
                    <asp:DropDownList ID="drlPageSizeQueryShow2Line" AutoPostBack="true" runat="server" CausesValidation="false">
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                        <asp:ListItem Value="24">24</asp:ListItem>
                        <asp:ListItem Value="36">36</asp:ListItem>
                        <asp:ListItem Value="72">72</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
           

    </table>
    <%  If Not show2LineViewPagingTextMode Then
            %>
             <br />
    <br />
            <%
        End If%>
   
    <table border="0" class="pagingShow2Line2" id="tblParentQueryShow2Line2" runat="server" align="right">
        <tr>
            <td width="auto">
            </td>
            <td align="center" class="totalpages" id="totalPageTextQueryShow2Line" runat="server">
            </td>
            <td align="center" valign="middle" class="pageline1">
            </td>
            <td align="left" class="prev" id="tdPrevQueryShow2Line" runat="server">
            Prev
            </td>
            <td align="left" class="pageindexTextTd" id="tdPageIndexQueryShow2Line" runat="server">
            </td>
            <td align="right" class="next" id="tdNextQueryShow2Line" runat="server">
            </td>
        </tr>
    </table>
    <%End If%>
</div>
