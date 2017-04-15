<%@ Control Language="VB" AutoEventWireup="false" CodeFile="buy-point.ascx.vb" Inherits="controls_checkout_buy_point" %>
<div class="buy-point" id="secBuyPoint">
    <div class="summary-box">
        <table class="tbl-summary">
            <tr class="header">
                <td class="left">
                    &nbsp;
                </td>
                <td colspan="2">
                    Need Points? Buy Points
                </td>
                <td class="right">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="left">
                    &nbsp;
                </td>
                <td colspan="2" class="text">
                    Choose Buy Points to get the reward you want!
                </td>
                <td class="right">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="left">
                    &nbsp;
                </td>
                <td class="pointselect">
                    <div class="nf-dropdown">
                        <asp:DropDownList ID="drpPoint" ClientIDMode="Static" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
                <td class="addpoint">
                    <input type="button" class="btnGreen" value="Buy Point" onclick="BuyPoint();" />
                </td>
                <td class="right">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="checkout">
        <div class="checkout-button" onclick="CheckOutNow();">
            <ul class="content-checkout">
                <li class="icon">
                    <img src="/includes/theme/images/secure.png">
                </li>
                <li class="text">Secure Checkout Now </li>
            </ul>
        </div>
    </div>
</div>
