<%@ Control Language="VB" AutoEventWireup="false" CodeFile="mail-order-detail.ascx.vb"
    Inherits="controls_checkout_mail_order_detail" %>
<table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
    <tr valign="top">
        <%If EbayOrder Then%>
        <td width="780px" style="padding-bottom: 10px;">
            <%Else%>
        <td width="780px">
            <%End If%>
            <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
                <tbody>
                    <tr valign="top">
                        <td style="padding: 20px 20px 20px 20px;">
                            <table>
                                <tr>
                                    <td style="width: 780px; font-weight: bold; color: #333333; font-size: 14px; vertical-align: top;
                                        font: bold 14px/18px Open Sans;">
                                        Thanks for your order,
                                        <%=m_LoggedInName%>!
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 780px; color: #333333; font-size: 14px; vertical-align: top; font: 14px/18px Open Sans;
                                        padding: 6px 0px 0px 0px;">
                                        If you need to check the status of your order or tracking number, please log on
                                        to your account or click on one of the links below.<br />
                                        If you do not receive your package 14 days after shipping date, please contact FedEx
                                        or your local Post Office with the tracking #.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 780px; padding-top: 6px; color: #3b76ba; vertical-align: top; font: bold 14px/18px Open Sans;">
                                        <a style="text-decoration: none; color: #3b76ba;" href="<%=weebRoot %>/members/orderhistory/">
                                            Your Orders</a> | <a href="<%=weebRoot %>/members/" style="text-decoration: none;
                                                color: #3b76ba;">Your Account</a> | <a href="<%=weebRoot %>" style="text-decoration: none;
                                                    color: #3b76ba;">Nailsuperstore.com</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellpadding="0" cellspacing="0" style="width: 780px">
                <tr>
                    <td style="background: #59595b; color: White; font: bold 16px Open Sans; padding: 9px 0px 9px 20px;
                        text-align: left;">
                        <% If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                        View Order
                        <% Else%>
                        Order Confirmation:
                        <% End If%>
                        #<asp:Literal ID="lit" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
                <tr valign="top">
                    <td style="width: 50%; vertical-align: top;">
                        <%If Not EbayOrder Then%>
                        <!-- ship to -->
                        <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                            <tr valign="top">
                                <td style="width: 95px; padding: 10px 15px 0px 0px; color: #b70072; font-size: 14px;
                                    vertical-align: top; font: bold 14px/18px Open Sans;" align="right">
                                    <asp:Literal ID="ltrBillingToTitle" runat="server"></asp:Literal>
                                </td>
                                <td style="width: 268px; line-height: 16px; padding: 10px 6px 14px 0; color: #333333;
                                    font: 14px/18px Open Sans;">
                                    <asp:Literal runat="server" ID="litBilling" />
                                </td>
                            </tr>
                        </table>
                        <%Else%>
                        <table cellspacing="0" cellpadding="0" border="0" style="width: 363px; vertical-align: top;">
                            <tr>
                                <td style="width: 250px; padding: 10px 15px 0px 0px; color: #b70072; font-size: 14px;
                                    vertical-align: top; font: bold 14px/18px Open Sans;" align="right">
                                    eBay Customer ID:
                                </td>
                                <td style="width: 268px; line-height: 16px; padding: 10px 6px 2px 0; color: #333333;
                                    font: 14px/18px Open Sans;">
                                    <asp:Literal runat="server" ID="ltrEbayCustomerMail" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td style="padding: 2px 15px 0px 0px; color: #b70072; font-size: 14px; vertical-align: top;
                                    font: bold 14px/18px Open Sans" align="right">
                                    Order Date:
                                </td>
                                <td style="width: 268px; line-height: 16px; padding: 2px 6px 14px 0; color: #333333;
                                    font: 14px/18px Open Sans;">
                                    <asp:Literal runat="server" ID="ltrEbayOrderData" />
                                </td>
                            </tr>
                        </table>
                        <%End If%>
                    </td>
                    <td style="width: 50%;">
                        <div id="dvOrderNo" runat="server">
                            <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                                <tr valign="top">
                                    <td style="width: 50px; padding-top: 10px; color: #b70072; font-size: 14px; vertical-align: top;
                                        font: bold 14px/18px Open Sans;">
                                        <asp:Literal ID="ltrShipToTitle" runat="server"></asp:Literal>
                                    </td>
                                    <td valign="top" align="left" style="width: 268px; line-height: 16px; padding: 10px 6px 10px 0;
                                        color: #333333; font: 14px/18px Open Sans;">
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
                                        <img src="/images/global/hdg-bill2-sm.gif" width="101" height="24" style="border-style: none"
                                            alt="Bill To:" /><br />
                                        <div style="line-height: 16px; padding: 0 15px 15px 75px; color: #333333; font: bold 14px Open Sans;
                                            font: 14px/18px Open Sans;">
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
            <table cellspacing="0" cellpadding="0" style="width: 780px; height: auto;" summary="cart contents">
                <tr>
                    <td style="width: 60px; text-align: center; background: #59595b; color: White; font: bold 14px Open Sans;
                        letter-spacing: inherit; padding: 10px 1px 10px 1px; font-weight: bold;">
                        Item
                    </td>
                    <td style="background: #59595b; color: White; font: bold 14px Open Sans; letter-spacing: inherit;
                        padding: 10px 1px 10px 1px; font-weight: bold;">
                        &nbsp;
                    </td>
                    <td style="width: 50px; background: #59595b; color: White; font: bold 14px Open Sans;
                        letter-spacing: inherit; padding: 10px 1px 10px 1px; font-weight: bold;">
                        Qty
                    </td>
                    <% If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                    <td style="width: 70px; background: #59595b; color: White; font: bold 14px Open Sans;
                        letter-spacing: inherit; padding: 10px 1px 10px 1px; font-weight: bold;">
                        Qty Shipped
                    </td>
                    <%End If%>
                    <td style="width: 50px; background: #59595b; color: White; font: bold 14px Open Sans;
                        letter-spacing: inherit; padding: 10px 1px 10px 1px; font-weight: bold;">
                        Unit
                    </td>
                    <td style="width: 245px; background: #59595b; color: White; font: bold 14px Open Sans;
                        letter-spacing: inherit; padding: 10px 1px 10px 1px; font-weight: bold;">
                        Ship Via / Options
                    </td>
                    <td style="width: 70px; background: #59595b; color: White; font: bold 14px Open Sans;
                        letter-spacing: inherit; padding: 10px 1px 10px 1px; font-weight: bold;">
                        Total
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rptCartItems">
                    <ItemTemplate>
                        <tr>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072;") %>">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 58px;" summary="image">
                                    <tr>
                                        <td style="width: 58px; height: auto; border: 1px solid #999999; text-align: center;
                                            color: #333333; font: 14px/18px Open Sans;">
                                            <asp:Literal runat="server" ID="lnkImg" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px;color:#333333;font:14px/18px Open Sans;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072; color:#333333; font:14px/18px Open Sans;") %>">
                                <asp:Literal runat="server" ID="litDetails" />
                                <asp:Literal runat="server" ID="litCoupon" />
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px;color:#333333;font:14px/18px Open Sans;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072;color:#333333; font:14px/18px Open Sans;") %>">
                                <%#Container.DataItem.Quantity%>
                            </td>
                            <% If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px;color:#333333;font:14px/18px Open Sans;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072; color:#333333;font:14px/18px Open Sans;") %>">
                                <asp:Literal runat="server" ID="ltlQtyShipped" />
                            </td>
                            <% End If%>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px;color:#333333;font:14px/18px Open Sans;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072;color:#333333;font:14px/18px Open Sans;") %>">
                                <asp:Literal runat="server" ID="litUnit" />
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px;color:#333333;font:14px/18px Open Sans;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072;color:#333333;font:14px/18px Open Sans;") %>">
                                <table cellspacing="0" cellpadding="0" border="0" summary="shipping method">
                                    <tr valign="top">
                                        <td style="width: 27px; color: #333333; font: 14px/18px Open Sans;">
                                            <asp:Image runat="server" ID="imgShipping" Width="26" Height="25" />
                                        </td>
                                        <td style="padding-top: 2px; text-align: left; color: #333333; font: 14px/18px Open Sans;"
                                            align="left">
                                            <asp:Label runat="server" ID="lblCartItemId" Visible="false" Text="<%#Container.DataItem.CartItemId%>" />
                                            <asp:Panel runat="server" ID="pnlSelected">
                                                <asp:Literal runat="server" ID="litSelected" />
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlRush">
                                                <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px; width: 240px;">
                                                    <tr valign="top">
                                                        <td>
                                                            <asp:Image runat="server" ID="Image1" Width="20" Height="20" ImageUrl="~/includes/theme/images/checkbox.jpg" />
                                                        </td>
                                                        <td style="color: #333333; font: 14px/18px Open Sans; padding-left: 5px;">
                                                            <asp:Literal runat="server" ID="ltlRush" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlOversize">
                                                <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px; width: 240px;">
                                                    <tr valign="top" id="trLiftGate" runat="server">
                                                        <td>
                                                            <img src="<%=weebRoot %>/includes/theme/images/checkbox.jpg" width="20" height="20" />
                                                        </td>
                                                        <td style="color: #333333; font: 14px/18px Open Sans; padding-left: 5px;">
                                                            <asp:Literal runat="server" ID="ltlLiftGate" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top" id="trScheduleDelivery" runat="server">
                                                        <td style="padding-top: 4px;">
                                                            <img src="<%=weebRoot %>/includes/theme/images/checkbox.jpg" width="20" height="20" />
                                                        </td>
                                                        <td style="padding-top: 4px; color: #333333; font: 14px/18px Open Sans; padding-left: 5px;">
                                                            <asp:Literal runat="server" ID="ltlScheduleDelivery" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top" id="trInsideDelivery" runat="server">
                                                        <td style="padding-top: 4px;">
                                                            <img src="<%=weebRoot %>/includes/theme/images/checkbox.jpg" width="20" height="20" />
                                                        </td>
                                                        <td style="padding-top: 4px; color: #333333; font: 14px/18px Open Sans; padding-left: 5px;">
                                                            <asp:Literal runat="server" ID="ltlInsideDelivery" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 5px;color:#333333;font:14px/18px Open Sans;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #b70072;color:#333333;font:14px/18px Open Sans;") %>">
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
            <div id="ship" class="form1" style="margin: 10px 0 0px 0;">
                <table cellpadding="0" cellspacing="0" style="width: 780px">
                    <tr>
                        <td style="background: #59595b; color: White; font: bold 16px Open Sans; padding: 9px 0px 9px 20px;
                            text-align: left;">
                            Order Payment Summary
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;" summary="order summary">
                    <tr>
                        <td style="width: 200px; padding-left: 15px; padding-top: 5px;" valign="top">
                            <table style="width: 220px" cellspacing="0" cellpadding="0" border="0" runat="server"
                                id="tblSalesTax">
                                <tr valign="top">
                                    <td>
                                        <span style="color: #b70072; font: 14px/16px Open Sans;">*</span>
                                    </td>
                                    <td>
                                        <span style="color: #b70072; font: 14px/16px Open Sans;">Amount Subject to<br />
                                            Sales Tax</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="padding-bottom: 10px">
                                        <asp:Label runat="server" ID="lblSubjectToTax" Style="color: #333333; font: bold 14px Open Sans;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: #b70072; font: 14px/16px Open Sans;">*</span>
                                    </td>
                                    <td>
                                        <span style="color: #b70072; font: 14px/16px Open Sans;">Amount Exempt from<br />
                                            Sales Tax</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="padding-bottom: 10px">
                                        <asp:Label runat="server" ID="lblExempt" Style="color: #333333; font: bold 14px Open Sans;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" style="width: 280px">
                            <div style="padding: 5px 15px 15px 15px; color: #333333; font: bold 14px/25px Open Sans;">
                                <asp:Literal runat="server" ID="litPayment" />
                            </div>
                        </td>
                        <td align="right" style="width: 300px; padding: 5px 10px 0px 0px;" valign="top">
                            <table cellspacing="0" cellpadding="0" border="0" summary="subtotals">
                                <tr>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        Merchandise Subtotal
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #333333;">
                                        <asp:Label runat="server" ID="lblMerchSubTotal" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trPromotionalDiscount">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        You Saved
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: Red; font-weight: bold;
                                        color: #333333;">
                                        <asp:Label runat="server" ID="lblPromotionalDiscount" Style="font-weight: bold; color: #bb0008;" />
                                    </td>
                                </tr>
                                <asp:Literal runat="server" ID="litShippingDetails" /><tr runat="server" id="trCODFee"
                                    visible="false">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        COD Fee
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #333333;">
                                        <asp:Label runat="server" ID="lblCODFee" Style="color: #333333; font: bold 14px/25px Open Sans;" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trResidential">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        Residential Delivery Surcharge
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #333333;">
                                        <asp:Label runat="server" ID="lblResidentil" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trSalesTax">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        **Estimated Sales Tax
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #333333;">
                                        <asp:Label runat="server" ID="lblSalesTax" Style="color: #333333; font: bold 14px/25px Open Sans;" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trCouponDiscount" visible="false">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        Coupon Discount<asp:Label runat="server" ID="Label1"></asp:Label>
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #bb0008; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblCouponDiscount" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trPoint">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        Cash Reward Points
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #bb0008; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblPurchasePoint" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trLevelPoint">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        <asp:Label ID="lblMLevelPoint" runat="server"></asp:Label>
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #bb0008; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblDiscPoint" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trHandlingFee">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        Special Handling Fee
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #454545; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblHandlingFee" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trHazardousMaterialFee">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #333333;">
                                        Hazardous Material Fee
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 14px/25px Open Sans; padding: 5px 0 5px 0;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #454545; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblHazardousMaterialFee" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 15px 10px 0px; color: #333333; font: bold 18px Open Sans;
                                        text-align: left;">
                                        Order Total
                                    </td>
                                    <td align="right" style="padding: 10px 0px 10px 15px; color: #333333; font: bold 16px Open Sans;
                                        text-align: left;">
                                        <asp:Label runat="server" ID="lblSubTotal" Style="color: #333333; font: bold 16px/25px Open Sans;" />
                                    </td>
                                </tr>
                                <asp:Literal ID="litInter" runat="server" Visible="false"></asp:Literal>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-left: 15px">
                            <div runat="server" style="color: #333333; font: 14px/18px Open Sans;" id="divOversize">
                                Due to size and/or weight of your order may require additional shipping and handling
                                fees.</div>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
<div runat="server" style="margin: 10px 0 0 10px; color: #333333; font: bold 14px Open Sans;"
    id="divNotesHead">
    Order Notes:</div>
<div runat="server" style="margin: 0 0 10px 10px; border: solid 1px #cccccc; padding: 8px;
    width: 700px;" id="divNotes">
</div>
<div runat="server" style="margin: 0 0 10px 10px; padding: 8px; width: 700px;" id="divComment">
    Order via mobile</div>
