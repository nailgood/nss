<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cart-send.ascx.vb" Inherits="controls_checkout_cart_send" %>
<table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
    <tr valign="top">
        <td width="780px">
   
            <table cellspacing="0" cellpadding="0" border="0" style="width: 780px;">
                <tbody>
                    <tr valign="top">
                        <td style="padding: 20px 0px;">
                            <table>
                                <tr>
                                    <td style="width: 780px; font-weight: bold; color: #428bca; font-size: 14px; vertical-align: top;
                                        font: bold 14px/18px Open Sans;">
                                        Dear #TONAME#!
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%; color: #333333; font-size: 14px; vertical-align: top; font: 14px/18px Open Sans;
                                        padding: 6px 0px 0px 0px;">
                                        Thank you for visiting NailSuperstore.com.<br />
                                        Items left in the shopping cart are waiting for you to check out.
                                    </td>
                                    <td align="right">
                                        <a href="<%=sUrl %>"><img src="/includes/theme/images/view-cart.jpg" /></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
          </td>
    </tr>
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" style="width: 780px; height: auto; border:solid 1px #dadada" summary="cart contents">
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
                                <%#	IIf(Container.DataItem.PriceDesc = Nothing, "&nbsp;", Container.DataItem.PriceDesc)%>
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
        <td style="padding: 10px 0 0px 0;">
            <div id="ship" class="form1">
                <table cellpadding="0" cellspacing="0" style="width: 780px">
                    <tr>
                        <td style="background: #59595b; color: White; font: bold 16px Open Sans; padding: 9px 0px 9px 20px;
                            text-align: left;">
                            Order Payment Summary
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" style="width: 780px; border:solid 1px #dadada" summary="order summary">
                    <tr>
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
                                <tr>
                                    <td style="padding: 10px 15px 10px 0px; color: #333333; font: bold 18px Open Sans;
                                        text-align: left;">
                                        Order SubTotal
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
                   
                </table>
                <table style="width:100%">
                <tr><td align="right" style="padding-top:23px"><a href="<%=sUrl %>"><img src="/includes/theme/images/view-cart.jpg" /></a></td></tr>
                </table>
            </div>
        </td>
    </tr>
</table>
