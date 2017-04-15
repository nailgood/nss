<%@ Control Language="VB" AutoEventWireup="false" CodeFile="reward-point-summary.ascx.vb"
    Inherits="controls_checkout_reward_point_summary" %>
<section class="summary-box summary-point" id="secPointSummary">
    <table class="tbl-summary">
        <tr class="header">
            <td class="left">
                &nbsp;
            </td>
            <td colspan="2" class="point">
                <asp:Literal ID="tdRewardsPoint" runat="server"></asp:Literal>
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="left">
                &nbsp;
            </td>
            <td class="label-text">
               Available Points
            </td>
            <td class="label-data">
                <asp:Literal ID="divAvailablePoints" runat="server"></asp:Literal>
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="left">
                &nbsp;
            </td>
            <td class="label-text">
               Pending Points <a onclick="ShowPendingPointTip();"><span class="question"></span></a>
            </td>
            <td class="label-data">
                 <asp:Literal ID="tdPendingPoints" runat="server"></asp:Literal>       
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
         <tr id="trBuyPoint" runat="server" visible="false">
            <td class="left">
                &nbsp;
            </td>
            <td class="label-text">
               Points Purchased 
               <div class="removePoint" id="divPointPurchase" runat="server">
                            (<a id="lnkRemoveBuyPoint" onclick="OnRemoveBuyPointOrder();"  >Remove</a>)
                        </div>
            </td>
            <td class="label-data">
              
                        <%= Utility.Common.FormatPointPrice(Cart.GetTotalBuyPointByOrder(Cart.Order.OrderId))%>
                       
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <%--<tr >
            <td class="left">
                &nbsp;
            </td>
            <td class="label-text">
              Points for Product
            </td>
             <td class="label-data save">
               -  <%=Utility.Common.FormatPointPrice(m_TotalRewardsPoint)%>
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>--%>
        <tr >
            <td class="left">
                &nbsp;
            </td>
            <td class="label-text">
              Points Used
            </td>
             <td class="label-data save">
               - <%=Utility.Common.FormatPointPrice(m_PurchasePoint)%>
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
         <tr >
            <td class="left">
                &nbsp;
            </td>
            <td class="label-text balance">
               New Balance
            </td>
             <td class="label-data balance ">
              <%=Utility.Common.FormatPointPrice(Cart.GetCurrentBalancePoint(TotalPointAvailable))%>
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</section>
<script type="text/javascript">
    function ShowCashPointTip(content) {
        showQtip('qtip-msg', content, 'Cash Reward Points Program');
    }

    function showPointBalancePopup(evt) {
        $.get('/members/pointbalance.aspx?isPopUp=true', {}, function (result) {
            var resultString = $(result).find('div#popup').html();
            showQtip('qtip-msg', resultString, 'Your Cash Reward Points Balance');
        }
                , 'text');
    }

    function ShowPendingPointTip() {
        showQtip('qtip-msg', 'Points available to use after 30 days from shipping date of this order.', 'Pending Points');
    }
</script>