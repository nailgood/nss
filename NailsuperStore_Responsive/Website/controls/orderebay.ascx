<%@ Control Language="VB" AutoEventWireup="false" CodeFile="orderebay.ascx.vb" Inherits="controls_orderebay" %>
<table cellspacing="0" cellpadding="0" border="0" >
    <tr valign="top">
        <%If EbayOrder Then%>
        <td width="<%=formWidth%>px" style="padding-bottom: 10px;">
            <%Else%>
            <td width="<%=formWidth%>px">
                <%End If%>
                <%If (Request.RawUrl.Contains("confirmation.aspx")) Then%>
                <table cellspacing="0" cellpadding="0" border="0" class="orderConfirm" style="width: <%=formWidth%>px;">
                    <tbody>
                        <tr valign="top">
                            <td>
                                <span class="title">Thanks for your order, <%=m_LoggedInName%>! </span><span class="text">
                                  If you need to check the status of your order or tracking number, please log on to your account or click on one of the links below.<br />
If you do not receive your package 14 days after shipping date, please contact UPS or your local Post Office with the tracking #. </span>
<span class="link">
                                        <a href="/members/orderhistory/">Your Orders</a> | <a href="/members/">Your Account</a> | <a href="/">Nailsuperstore.com</a>
                                    </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <%End If%>
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td class="PSTitle">
                            <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                            View Order
                            <% Else%>
                            Order Confirmation:
                            <% End If%>
                            #<asp:Literal ID="lit" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" style="width: <%=formWidth%>px;">
                    <tr valign="top">
                        <td style="width: 50%;">
                            <%If Not EbayOrder Then%>
                            <!-- ship to -->
                            <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                                <tr valign="top">
                                    <td style="width: 95px; padding: 10px 15px 0px 0px" class="ShipTo" align="right">
                                        <asp:Literal ID="ltrBillingToTitle" runat="server"></asp:Literal>
                                    </td>
                                    <td style="width: 268px; line-height: 16px; padding: 12px 6px 12px 0;">
                                        <asp:Literal runat="server" ID="litBilling" />
                                    </td>
                                </tr>
                                <%If linkTracking <> String.Empty Then%>
                                <tr valign="top">
                                    <td style="width: 150px; padding: 1px 15px 0px 0px" class="ShipTo" align="right">
                                        Tracking number:
                                    </td>
                                    <td style="width: 268px; line-height: 16px; padding: 1px 6px 12px 0;">
                                        <div>
                                            <%=linkTracking%></div>
                                        <div>
                                            <%=linkEdit%></div>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                            <%Else%>
                            <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                                <tr valign="top">
                                    <td style="width: 250px; padding: 10px 15px 0px 0px" class="ShipTo" align="right">
                                        eBay Customer ID:
                                    </td>
                                    <td style="width: 268px; line-height: 16px; padding: 12px 6px 2px 0;">
                                        <asp:Literal runat="server" ID="ltrEbayCustomerMail" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td style="padding: 2px 15px 0px 0px" class="ShipTo" align="right">
                                        Order Date:
                                    </td>
                                    <td style="width: 268px; line-height: 16px; padding: 2px 6px 0px 0;">
                                        <asp:Literal runat="server" ID="ltrEbayOrderData" />
                                    </td>
                                </tr>
                                <%If linkTracking <> String.Empty Then%>
                                <tr valign="top">
                                    <td style="padding: 4px 15px 0px 0px" class="ShipTo" align="right">
                                        Tracking number:
                                    </td>
                                    <td style="width: 268px; line-height: 16px; padding: 4px 6px 12px 0;">
                                        <%=linkTracking%>
                                        <div>
                                            <%=linkEdit%></div>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                            <%End If%>
                        </td>
                        <td style="width: 50%;">
                            <div id="dvOrderNo" runat="server">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                                    <tr valign="top">
                                        <td style="width: 50px; padding-top: 10px" class="ShipTo">
                                            <asp:Literal ID="ltrShipToTitle" runat="server"></asp:Literal>
                                        </td>
                                        <td valign="top" align="left" style="width: 268px; line-height: 16px; padding: 12px 6px 0px 0;">
                                            <asp:Literal runat="server" ID="litShipping" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="dvNotOrderNo" runat="server">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                                    <tr>
                                        <td style="" width="100%" colspan="3">
                                            <div class="large bold SectionBlockHeader carthdr">
                                                Billing Address</div>
                                            <img src="/includes/theme-admin/images/global/hdg-bill2-sm.gif" width="101" height="24" style="border-style: none"
                                                alt="Bill To:" /><br />
                                            <div class="bold" style="line-height: 16px; padding: 0 15px 15px 75px;">
                                                <asp:Literal runat="server" ID="litBillingNotOrder" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
    </tr>
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" style="width: 100%; height: auto;" summary="cart contents">
                <tr>
                    <td style="width: 60px; height: 25px; text-align: center;" class="CartItemhdr">
                        Item
                    </td>
                    <td class="CartItemhdr">
                        &nbsp;
                    </td>
                    <td style="width: 50px" class="CartItemhdr">
                        Qty
                    </td>
                    <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                    <td style="width: 70px" class="CartItemhdr">
                        Qty Shipped
                    </td>
                    <%End If%>
                    <td style="width: 50px" class="CartItemhdr">
                        Unit
                    </td>
                    <td style="width: 245px" class="CartItemhdr">
                        Ship Via / Options
                    </td>
                    <td style="width: 70px" class="CartItemhdr">
                        Total
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rptCartItems">
                    <ItemTemplate>
                        <tr>
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2" style="padding: 5px 5px 5px 5px;">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 58px;" summary="image">
                                    <tr>
                                        <td class="center" style="width: 58px; height: auto; border: 1px solid #999999;">
                                            <asp:Literal runat="server" ID="lnkImg" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2">
                                <asp:Literal runat="server" ID="litDetails" />
                                <div class="mag">
                                    <asp:Literal runat="server" ID="litCoupon" /></div>
                                <div class="bold mag">
                                    <asp:Literal runat="server" ID="litPromotion" /></div>
                            </td>
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2">
                                <%#Container.DataItem.Quantity%>
                            </td>
                            <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2">
                                <asp:Literal runat="server" ID="ltlQtyShipped" />
                            </td>
                            <% End If%>
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2">
                                <%#	IIf(Container.DataItem.PriceDesc = Nothing, "&nbsp;", Container.DataItem.PriceDesc)%>
                            </td>
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2">
                                <table cellspacing="0" cellpadding="0" border="0" summary="shipping method">
                                    <tr valign="top">
                                        <td style="width: 27px">
                                            <asp:Image runat="server" ID="imgShipping" Width="26" Height="25" />
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
                            <td class="<%# IIf(Container.DataItem.Final = "1","","carttd") %> cartpad2 left">
                                <asp:Literal ID="ltrTotal" runat="server">
                                </asp:Literal>
                                
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr valign="top">
        <td>
            <div id="ship" class="form1" cellpadding="0" cellspacing="0" style="margin: 10px 0 0px 0;
                padding-left: 0px; width: 754px;">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td class="PSTitle">
                            Order Payment Summary
                        </td>
                    </tr>
                </table>
                <% If formWidth = 754 Then%>
                <table cellspacing="0" cellpadding="0" border="0" class="OrderWidthDetail" summary="order summary">
                    <%Else%>
                    <table cellspacing="0" cellpadding="0" border="0" class="OrderWidth" summary="order summary">
                        <%End If%>
                        <tr>
                            <td style="width: 200px; padding-left: 15px; padding-top: 5px;" valign="top">
                                <table style="width: 220px" cellspacing="0" cellpadding="0" border="0" runat="server"
                                    id="tblSalesTax">
                                    <tr valign="top">
                                        <td>
                                            <span id="con-label">*</span>
                                        </td>
                                        <td>
                                            <span id="con-label">Amount Subject to<br />
                                                Sales Tax</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="padding-bottom: 10px">
                                            <asp:Label runat="server" ID="lblSubjectToTax" CssClass="bold" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="con-label">*</span>
                                        </td>
                                        <td>
                                            <span id="con-label">Amount Exempt from<br />
                                                Sales Tax</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="padding-bottom: 10px">
                                            <asp:Label runat="server" ID="lblExempt" CssClass="bold" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" style="width: 280px">
                                <div class="bold" style="line-height: 16px; padding: 5px 15px 15px 15px">
                                    <asp:Literal runat="server" ID="litPayment" />
                                </div>
                            </td>
                            <td align="right" style="width: 300px; padding: 5px 10px 0px 0px;">
                                <table cellspacing="0" cellpadding="0" border="0" summary="subtotals">
                                    <tr>
                                        <td class="titprice">
                                            Merchandise Subtotal
                                        </td>
                                        <td class="price1">
                                            <asp:Label runat="server" ID="lblMerchSubTotal" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trPromotionalDiscount">
                                        <td class="titprice">
                                            You Saved
                                        </td>
                                        <td class="price1 red">
                                            <asp:Label runat="server" ID="lblPromotionalDiscount" CssClass="savePrice" />
                                        </td>
                                    </tr>
                                    <asp:Literal runat="server" ID="litShippingDetails" /><tr runat="server" id="trCODFee"
                                        visible="false">
                                        <td class="titprice">
                                            COD Fee
                                        </td>
                                        <td class="price1">
                                            <asp:Label runat="server" ID="lblCODFee" CssClass="bold" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trResidential">
                                        <td class="titprice">
                                            Residential Delivery Surcharge
                                        </td>
                                        <td class="price1">
                                            <asp:Label runat="server" ID="lblResidentil" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trSalesTax">
                                        <td class="titprice">
                                            **Estimated Sales Tax
                                        </td>
                                        <td class="price1">
                                            <asp:Label runat="server" ID="lblSalesTax" CssClass="bold" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trCouponDiscount" visible="false">
                                        <td class="titprice">
                                            Coupon Discount
                                        </td>
                                        <td class="saveprice">
                                            <asp:Label runat="server" ID="lblCouponDiscount" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trPoint">
                                        <td class="titprice">
                                            Cash Reward Points 
                                        </td>
                                        <td class="price1 red">
                                            <asp:Label runat="server" ID="lblPurchasePoint" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trLevelPoint">
                                        <td class="titprice">
                                            <asp:Label ID="lblMLevelPoint" runat="server"></asp:Label>
                                        </td>
                                        <td class="price1 red">
                                            <asp:Label runat="server" ID="lblDiscPoint" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PMTotal" style="padding: 10px 15px 0px 0px">
                                            Order Total
                                        </td>
                                        <td class="PMTotal" align="right" style="padding: 10px 0px 0px 15px;">
                                            <asp:Label runat="server" ID="lblSubTotal" CssClass="bold" />
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litInter" runat="server" Visible="false"></asp:Literal>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left: 15px">
                                <div runat="server" id="divOversize">
                                    Due to size and/or weight of your order may require additional shipping and handling
                                    fees.</div>
                            </td>
                        </tr>
                    </table>
            </div>
        </td>
    </tr>
</table>
<div runat="server" style="margin: 10px 0 0 10px;" class="bold" id="divNotesHead">
    Order Notes:</div>
<div runat="server" style="margin: 0 0 10px 10px; border: solid 1px #cccccc; padding: 8px;
    width: 700px;" id="divNotes">
</div>
