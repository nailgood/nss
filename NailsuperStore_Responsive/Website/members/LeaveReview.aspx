<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leavereview.aspx.vb" Inherits="members_LeaveReview" MasterPageFile="~/includes/masterpage/interior.master" %>
<%@ Register src="~/controls/product/purchased.ascx" tagname="order" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<div id="order-detail">
<div id="filter" runat="server" clientidmode="Static" style="padding-top:15px">
  All orders placed in
  <span class="n-dropdown">
        <asp:DropDownList runat="server" ID="ddTopRecord" AutoPostBack="true" CssClass="form-control">
            <asp:ListItem Text="last 30 days" Value="30" />
            <asp:ListItem Text="last 90 days" Value="90" />
            <asp:ListItem Text="last 180 days" Value="180" />
            <asp:ListItem Text="---- All ----" Value="2147483647" />
        </asp:DropDownList>
    </span>
</div>
    <asp:Repeater runat="server" ID="rptOrder">
        <ItemTemplate>
        <uc1:order ID="ucOrder" runat="server" />
           <%-- <div class="order-review">
                <asp:Literal runat="server" ID="ltOrderNo" />
                <asp:PlaceHolder runat="server" ID="trCartItems">
                    <table cellspacing="0" border="0" cellpadding="0" class="tbl-review" summary="cart contents">
                        <tr>
                            <td style="max-width: 60px;" class="header">
                                Item
                            </td>
                            <td class="header">
                                &nbsp;
                            </td>
                            <td style="max-width: 50px" class="header">
                                Qty
                            </td>
                            <td style="max-width: 190px" class="header">
                                Ship Via
                            </td>
                            <td style="max-width: 70px" class="header">
                                Total
                            </td>
                            <td style="min-width:140px;max-width: 150px" class="header">
                                &nbsp;
                            </td>
                        </tr>
                        <asp:Repeater runat="server" ID="rptCartItems">
                            <ItemTemplate>
                                <tr>
                                    <td class="imgItem">
                                        <div class="dvImg">
                                            <asp:Literal runat="server" ID="lnkImg" /></div>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litDetails" />
                                        <div class="mag">
                                            <asp:Literal runat="server" ID="litCoupon" /></div>
                                        <div class="bold mag">
                                            <asp:Literal runat="server" ID="litPromotion" /></div>
                                    </td>
                                    <td class="text-center">
                                        <%#Container.DataItem.Quantity%>
                                    </td>
                                    <td class="text-center">
                                        <ul class="list-inline">
                                            <li style="width: 27px">
                                                <asp:Image runat="server" ID="imgShipping" Width="26" Height="25" /></li>
                                            <li>
                                                <asp:Label runat="server" ID="lblCartItemId" Visible="false" Text="<%#Container.DataItem.CartItemId%>" />
                                                <asp:Panel runat="server" ID="pnlSelected">
                                                    <asp:Literal runat="server" ID="litSelected" />
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlRush">
                                                    <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px;">
                                                        <tr valign="top">
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsRushDelivery" AutoPostBack="true" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal runat="server" ID="ltlRush" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlOversize">
                                                    <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px;">
                                                        <tr valign="top" id="trLiftGate" runat="server">
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsLiftGate" AutoPostBack="true" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal runat="server" ID="ltlLiftGate" />
                                                            </td>
                                                        </tr>
                                                        <tr valign="top" id="trScheduleDelivery" runat="server">
                                                            <td style="padding-top: 4px;">
                                                                <asp:CheckBox runat="server" ID="chkScheduleDelivery" AutoPostBack="true" />
                                                            </td>
                                                            <td style="padding-top: 4px;">
                                                                <asp:Literal runat="server" ID="ltlScheduleDelivery" />
                                                            </td>
                                                        </tr>
                                                        <tr valign="top" id="trInsideDelivery" runat="server">
                                                            <td style="padding-top: 4px;">
                                                                <asp:CheckBox runat="server" ID="chkInsideDelivery" AutoPostBack="true" />
                                                            </td>
                                                            <td style="padding-top: 4px;">
                                                                <asp:Literal runat="server" ID="ltlInsideDelivery" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </li>
                                        </ul>
                                    </td>
                                    <td class="text-center">
                                        <asp:Literal runat="server" ID="ltrTotal" />
                                    </td>
                                    <td class="text-right review-link">
                                        <asp:Literal ID="ltCartReview" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </asp:PlaceHolder>
            </div>--%>
        </ItemTemplate>
    </asp:Repeater>
</div>
</asp:Content>