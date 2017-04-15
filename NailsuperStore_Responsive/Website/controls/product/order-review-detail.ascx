<%@ Control Language="VB" AutoEventWireup="false" CodeFile="order-review-detail.ascx.vb" Inherits="controls_product_order_review_detail" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<table cellspacing="0" cellpadding="0" style="width: 780px">
    <tr valign="top">
        <td style="" width="850">
            <table cellpadding="0" cellspacing="0" style="width: 850px">
                <tr>
                    <td style="background: Gray; color: White; font: bold 16px Arial; padding: 5px 0px 5px 20px;
                        text-align: left;">
                        Order #<asp:Literal ID="lit" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" border="0" style="width: 850px;">
                <tr valign="top">
                    <td style="padding-left: 15px; padding-right: 15px; padding-bottom: 15px; padding-top: 10px;
                        line-height: 30px;">
                         <span style="font-size: 14px; font-weight: bold">Dear
                            <%=memberName%>, </span>
                        <br />
                            Thank you for your recent purchase from The Nail Superstore website.
                            <br />
                            We invite you to submit a review for the product you purchased that would benefit
                            other customers. Your input will help customers choose the best products on The
                            Nail Superstore website.
                            <br />
                            Customers who submit a review will earn
                            <%=pointReview%>
                            Cash Reward Points per product. <a style="color: #0107fd;" href="https://www.nailsuperstore.com/services/reward-point-program.aspx">
                                See more details</a> .<br />
                            <%=Resources.Msg.review_msgFirst %><br />
                            It's easy to submit a review--just click the <span style="font-family: Arial; font-size: 12px;
                                font-weight: bold; color: #0605ff;">Review this product</span></strong> button
                            next to the product.
                            <br />
                            Thanks again for choosing The Nail Superstore.
                            <br />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellspacing="0" border="0" cellpadding="0" style="width: 850px; height: auto;"
                summary="cart contents">
                <tr>
                    <td style="width: 60px; height: 25px; text-align: center; background: Gray; color: White;
                        font: bold 13px arial; letter-spacing: inherit; padding: 1px; font-weight: bold;">
                        Item
                    </td>
                    <td style="background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                        &nbsp;
                    </td>
                    <td style="width: 50px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                        Qty
                    </td>
                    <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                    <td style="width: 70px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                        Qty Shipped
                    </td>
                    <%End If%>
                    <td style="width: 70px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                        Unit
                    </td>
                    <td style="width: 190px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                        Ship Via / Options
                    </td>
                    <td style="width: 50px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                        Total
                    </td>
                    <td style="width: 120px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 1px; font-weight: bold;">
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rptCartItems">
                    <ItemTemplate>
                        <tr>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 58px;" summary="image">
                                    <tr>
                                        <td class="center" style="width: 58px; height: 58px; border: 1px solid #999999;">
                                            <asp:Literal runat="server" ID="lnkImg" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <asp:Literal runat="server" ID="litDetails" />
                                <asp:Literal runat="server" ID="litCoupon" />
                               
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <%#Container.DataItem.Quantity%>
                            </td>
                            <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                            <td style="<%# IIf(Container.DataItem.Final = "1","","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <asp:Literal runat="server" ID="ltlQtyShipped" />
                            </td>
                            <% End If%>
                            <td style="<%# IIf(Container.DataItem.Final = "1","","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <%#	IIf(Container.DataItem.PriceDesc = Nothing, "&nbsp;", Container.DataItem.PriceDesc)%>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <table cellspacing="0" cellpadding="0" border="0" summary="shipping method">
                                    <tr valign="top">
                                        <td style="width: 27px" id="tdIconShipping" runat="server">
                                            <%--<asp:Image runat="server" ID="imgShipping" Width="26" Height="25" />--%>
                                        </td>
                                        <td style="padding-top: 2px; text-align: left;" align="left">
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
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <asp:Literal ID="ltrTotal" runat="server">
                                </asp:Literal>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","font-family: Arial;font-size: 12px;font-weight: bold;line-height: 1.3;list-style-type: none;text-align: right;padding: 5px 4px 8px 2px ;","font-family: Arial;font-size: 12px;font-weight: bold;line-height: 1.3;list-style-type: none;text-align: right;border-bottom: 1px solid #BE048C;padding: 5px 4px 8px 2px ;") %>;text-align:center">
                                <asp:Literal runat="server" ID="ltrLinkReview" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
</table>
<div runat="server" style="margin: 10px 0 0 10px;" class="bold" id="divNotesHead">
    Order Notes:</div>
<div runat="server" style="margin: 0 0 10px 10px; border: solid 1px #cccccc; padding: 8px;
    width: 700px;" id="divNotes">
</div>
