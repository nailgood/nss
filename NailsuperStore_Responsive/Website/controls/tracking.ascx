<%@ Control Language="VB" AutoEventWireup="false" CodeFile="tracking.ascx.vb" Inherits="controls_tracking" %>
<table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
    <tr valign="top">
        <td width="780px">
            <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
                <tbody>
                    <tr valign="top">
                        <td style="padding: 10px 20px 10px 20px;">
                            <table>
                                <tr>
                                    <td style="width: 780px; font-weight: bold; color: #454545; vertical-align: top;
                                        font: bold 12px/18px Arial;">
                                        <span style="font-size: 14px;">Hello
                                            <%=m_LoggedInName%>! </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 780px; color: #454545; font-size: 12px; vertical-align: top; font: 12px/18px Arial;
                                        padding: 6px 0px 0px 0px;">
                                        <asp:Literal ID="ltHello" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellpadding="0" cellspacing="0" style="width: 780px">
                <tr>
                    <td style="background: Gray; color: White; font: bold 16px Arial; padding: 5px 0px 5px 20px;
                        text-align: left;">
                        View Order #<asp:Literal ID="lit" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;" id="tbBeginTracking"
                runat="server" visible="false">
                <tr valign="top">
                    <td style="width: 50%; vertical-align: top; padding: 0px 0px 0px 20px">
                        <!-- ship to -->
                        <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                            <tr valign="top">
                                <td style="width: 268px; line-height: 16px; padding: 8px 6px 12px 0; color: #454545;
                                    font: 12px/18px Arial;">
                                    <b>
                                        <asp:Literal ID="ltEstimatedDelivery" runat="server"></asp:Literal></b>
                                    <p>
                                        <a href="<%=UrlTracking%>" style="border: none" target="_blank">
                                            <img src="<%=weebRoot%>/includes/theme/images/btn-tracking.gif" style="height: 35px;
                                                width: 150px;" alt="Track Your Package" /></a></p>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; padding: 10px 0px 10px 0px">
                        <div id="dvOrderNo" runat="server">
                            <table cellspacing="0" cellpadding="0" border="0" style="width: 363px;">
                                <tr valign="top">
                                    <td valign="top" align="left" style="width: 268px; line-height: 16px; padding: 10px 6px 0px 0;
                                        color: #454545; font: 12px/18px Arial;">
                                        Your order was send to:<br />
                                        <asp:Literal runat="server" ID="litShipping" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding: 0px 0px 15px 20px; font: 12px/18px Arial;">
                        Depending on the shipping option you chose, it may take 24 hours for tracking information
                        to be available for your order
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" border="0" style="width: 780px; padding: 10px 0px 10px 15px;
                color: #454545; font: 12px/18px Arial;" id="tbDelivery" runat="server" visible="false">
                <tr style="vertical-align: top">
                    <td style="width: 30%;">
                        <div>
                            Ship (P/U) date :</div>
                        <div class="line-track">
                            <asp:Label ID="lbShipTimestamp" runat="server"></asp:Label></div>
                        <div>
                            FRANKLIN PARK, IL US</div>
                    </td>
                    <td style="width: 38%; padding: 3px 60px 0px 0px; text-align: center;">
                        <asp:Literal ID="ltStatus" runat="server"></asp:Literal>
                    </td>
                    <td style="width: 25%; padding: 0px 10px 0px 10px">
                        <div>
                            <asp:Label ID="lbStatus" runat="server"></asp:Label></div>
                        <div class="line-track">
                            <asp:Label ID="lbActualDeliveryTimestamp" runat="server"></asp:Label></div>
                        <div>
                            <asp:Label ID="lbActualAddress" runat="server"></asp:Label></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding: 10px 10px 10px 20px">
                        <asp:Literal ID="ltSeeMore" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" style="width: 780px; height: auto;" summary="cart contents">
                <tr>
                    <td style="width: 60px; text-align: center; background: Gray; color: White; font: bold 13px arial;
                        letter-spacing: inherit; padding: 5px 1px 5px 1px; font-weight: bold;">
                        Item
                    </td>
                    <td style="background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 5px 1px 5px 1px; font-weight: bold;">
                        &nbsp;
                    </td>
                    <td style="width: 50px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 5px 1px 5px 1px; font-weight: bold;">
                        Qty
                    </td>
                    <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                    <td style="width: 70px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 5px 1px 5px 1px; font-weight: bold;">
                        Qty Shipped
                    </td>
                    <%End If%>
                    <td style="width: 245px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 5px 1px 5px 1px; font-weight: bold;">
                        Ship Via / Options
                    </td>
                    <td style="width: 70px; background: Gray; color: White; font: bold 13px arial; letter-spacing: inherit;
                        padding: 5px 1px 5px 1px; font-weight: bold;">
                        Total
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rptCartItems">
                    <ItemTemplate>
                        <tr>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 2px","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;") %>">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 58px;" summary="image">
                                    <tr>
                                        <td style="width: 58px; height: auto; border: 1px solid #999999; text-align: center;
                                            color: #454545; font: 12px/18px Arial;">
                                            <asp:Literal runat="server" ID="lnkImg" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 2px;color:#454545;font:12px/18px Arial;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C; color:#454545; font:12px/18px Arial;") %>">
                                <asp:Literal runat="server" ID="litDetails" />
                                
                                    <asp:Literal runat="server" ID="litCoupon" />
                                     
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 2px;color:#454545;font:12px/18px Arial;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;color:#454545; font:12px/18px Arial;") %>">
                                <%#Container.DataItem.Quantity%>
                            </td>
                            <%	If Not Request.Path.ToLower.IndexOf("/store/") = 0 Then%>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 2px;color:#454545;font:12px/18px Arial;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C; color:#454545;font:12px/18px Arial;") %>">
                                <asp:Literal runat="server" ID="ltlQtyShipped" />
                            </td>
                            <% End If%>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 2px;color:#454545;font:12px/18px Arial;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;color:#454545;font:12px/18px Arial;") %>">
                                <table cellspacing="0" cellpadding="0" border="0" summary="shipping method">
                                    <tr valign="top">
                                        <td style="width: 27px; color: #454545; font: 12px/18px Arial;" id="tdIconShipping" runat="server">
                              
                                        </td>
                                        <td style="padding-top: 2px; text-align: left; color: #454545; font: 12px/18px Arial;"
                                            align="left">
                                            <asp:Label runat="server" ID="lblCartItemId" Visible="false" Text="<%#Container.DataItem.CartItemId%>" />
                                            <asp:Panel runat="server" ID="pnlSelected">
                                                <asp:Literal runat="server" ID="litSelected" />
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlRush">
                                                <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px; width: 240px;">
                                                    <tr valign="top">
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkIsRushDelivery" AutoPostBack="true" />
                                                        </td>
                                                        <td style="color: #454545; font: 12px/18px Arial;">
                                                            <asp:Literal runat="server" ID="ltlRush" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlOversize">
                                                <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 5px; width: 240px;">
                                                    <tr valign="top" id="trLiftGate" runat="server">
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkIsLiftGate" AutoPostBack="true" />
                                                        </td>
                                                        <td style="color: #454545; font: 12px/18px Arial;">
                                                            <asp:Literal runat="server" ID="ltlLiftGate" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top" id="trScheduleDelivery" runat="server">
                                                        <td style="padding-top: 4px;">
                                                            <asp:CheckBox runat="server" ID="chkScheduleDelivery" AutoPostBack="true" />
                                                        </td>
                                                        <td style="padding-top: 4px; color: #454545; font: 12px/18px Arial;">
                                                            <asp:Literal runat="server" ID="ltlScheduleDelivery" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top" id="trInsideDelivery" runat="server">
                                                        <td style="padding-top: 4px;">
                                                            <asp:CheckBox runat="server" ID="chkInsideDelivery" AutoPostBack="true" />
                                                        </td>
                                                        <td style="padding-top: 4px; color: #454545; font: 12px/18px Arial;">
                                                            <asp:Literal runat="server" ID="ltlInsideDelivery" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="<%# IIf(Container.DataItem.Final = "1","padding: 5px 5px 5px 2px;color:#454545;font:12px/18px Arial;","padding: 5px 4px 8px 2px ;border-bottom: solid 1px #BE048C;color:#454545;font:12px/18px Arial;") %>">
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
                        <td style="background: Gray; color: White; font: bold 16px Arial; padding: 5px 0px 5px 20px;
                            text-align: left;">
                            Order Payment Summary
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;" summary="order summary">
                    <tr>
                        <td align="right" style="width: 300px; padding: 5px 10px 0px 0px;" valign="top">
                            <table cellspacing="0" cellpadding="0" border="0" summary="subtotals">
                                <tr>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        Merchandise Subtotal
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #454545;">
                                        <asp:Label runat="server" ID="lblMerchSubTotal" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trPromotionalDiscount">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        You Saved
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: Red; font-weight: bold;
                                        color: #454545;">
                                        <asp:Label runat="server" ID="lblPromotionalDiscount" Style="font-weight: bold; color: Red;" />
                                    </td>
                                </tr>
                                <asp:Literal runat="server" ID="litShippingDetails" /><tr runat="server" id="trCODFee"
                                    visible="false">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        COD Fee
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #454545;">
                                        <asp:Label runat="server" ID="lblCODFee" Style="color: #454545; font: bold 12px Arial;" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trResidential">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        Residential Delivery Surcharge
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #454545;">
                                        <asp:Label runat="server" ID="lblResidentil" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trSalesTax">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        **Estimated Sales Tax
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: #454545;">
                                        <asp:Label runat="server" ID="lblSalesTax" Style="color: #454545; font: bold 12px Arial;" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trCouponDiscount" visible="false">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        Coupon Discount<asp:Label runat="server" ID="Label1"></asp:Label>
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: Red; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblCouponDiscount" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trPoint">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        Cash Rewards Points
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: Red; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblPurchasePoint" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trLevelPoint">
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        height: 22px; text-align: left; vertical-align: bottom; width: 250px; color: #454545;">
                                        <asp:Label ID="lblMLevelPoint" runat="server"></asp:Label>
                                    </td>
                                    <td style="border-bottom: dotted 1px #CCCCCC; font: bold 12px Arial; padding-bottom: 5px;
                                        text-align: right; height: 22px; vertical-align: bottom; color: Red; font-weight: bold;">
                                        <asp:Label runat="server" ID="lblDiscPoint" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trHandlingFee">
                                    <td class="titprice">
                                        Special Handling Fee
                                    </td>
                                    <td class="price1 ">
                                        <asp:Label runat="server" ID="lblHandlingFee" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trHazardousMaterialFee">
                                    <td style="padding: 10px 15px 0px 0px; color: #454545; font: bold 12px Arial; text-align: left">
                                        Hazardous Material Fee
                                    </td>
                                    <td style="padding: 10px 0px 0px 15px; color: #454545; font: bold 12px Arial;
                                        text-align: left;">
                                        <asp:Label runat="server" ID="lblHazardousMaterialFee" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 15px 0px 0px; color: #454545; font: bold 16px Arial; text-align: left">
                                        Order Total
                                    </td>
                                    <td align="right" style="padding: 10px 0px 0px 15px; color: #454545; font: bold 16px Arial;
                                        text-align: left;">
                                        <asp:Label runat="server" ID="lblSubTotal" Style="color: #454545; font: bold 12px Arial;" />
                                    </td>
                                </tr>
                                <asp:Literal ID="litInter" runat="server" Visible="false"></asp:Literal>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-left: 15px">
                            <div runat="server" style="color: #454545; font: 12px/18px Arial;" id="divOversize">
                                Due to size and/or weight of your order may require additional shipping and handling
                                fees.</div>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td style="padding: 20px; border-top: solid 1px #dadada; line-height: 25px; font: 12px/18px Arial;">
            If you need further assistance with your order, please visit <a href="https://www.nailsuperstore.com/contact/GeneralQuestion.aspx">
                Customer Service</a>
            <br />
            We hope to see you again soon!
            <br />
            <span style="font-weight: bold; font-size: 14px"><a href="https://www.nailsuperstore.com"
                style="text-decoration: none; color: #454545" target="_blank">Nailsuperstore.com</a></span>
        </td>
    </tr>
</table>
<div runat="server" style="margin: 10px 0 0 10px; color: #454545; font: bold 12px Arial;"
    id="divNotesHead">
    Order Notes:</div>
<div runat="server" style="margin: 0 0 10px 10px; border: solid 1px #cccccc; padding: 8px;
    width: 700px;" id="divNotes">
</div>
<div runat="server" style="margin: 0 0 10px 10px; padding: 8px; width: 700px;" id="divComment">
    Order via mobile</div>
<asp:HiddenField ID="hTrackingId" runat="server" />
