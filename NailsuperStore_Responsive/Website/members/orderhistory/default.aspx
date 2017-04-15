<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_orderhistory_default" MasterPageFile="~/includes/masterpage/interior.master" CodeFile="default.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="order-history">
        <h1 class="page-title">
            Order History</h1>
        <div class="hidden-xs" style="padding: 0 0 15px 0">
            Click on icon <span class="glyphicon glyphicon-search s-16"></span> to view your
            order history</div>
        <!-- cart table -->
        <asp:Repeater ID="rptOrderHistory" EnableViewState="False" runat="server">
            <HeaderTemplate>
                <table cellspacing="0" cellpadding="0" class="tbl-order" summary="shopping cart table">
                    <tr>
                        <td class="header hidden-xs" style="max-width: 45px">
                        </td>
                        <td class="header r-top" style="max-width: 80px">
                            Order #
                        </td>
                        <td class="header hidden-xs">
                            Billing Name
                        </td>
                        <td class="header hidden-xs" style="max-width: 80px">
                            Total
                        </td>
                        <td class="header">
                            Purchase Date
                        </td>
                        <td class="header hidden-xs" style="max-width: 80px">
                            Status
                        </td>
                        <td class="header">
                            Tracking No
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="text-center hidden-xs">
                        <asp:LinkButton ID="btnDetails" runat="server" CommandName="Details" CssClass="glyphicon glyphicon-search s-16" />
                    </td>
                    <%--<a href="../classifieds.aspx">../classifieds.aspx</a>--%>
                    <td class="text-center">
                        <asp:Literal ID="ltlOrderNo" runat="server" />
                    </td>
                    <td class="text-left hidden-xs">
                        <asp:Literal ID="ltlBillingName" runat="server" />
                    </td>
                    <td class="text-right hidden-xs">
                        <asp:Literal ID="ltlTotal" runat="server" />
                    </td>
                    <td class="text-right">
                        <asp:Literal ID="ltlPurchaseDate" runat="server" />
                    </td>
                    <td class="text-center hidden-xs">
                        <asp:Literal ID="ltlStatus" runat="server" />
                    </td>
                    <td style="padding-top: 5px; padding-bottom: 5px;">
                        <table>
                            <tr>
                                <td style="width: 40px; text-align: center;">
                                    <asp:Literal ID="ltrIconShipping" runat="server" />
                                </td>
                                <td align="left">
                                    <asp:Literal ID="ltlTrackingNo" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

</asp:Content>