<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confirmation.aspx.vb" Inherits="confirmation" MasterPageFile="~/includes/masterpage/checkout.master" %>
<%@ Register Src="~/controls/product/order-detail.ascx" TagName="cart" TagPrefix="uc1" %>
<%@ Register Src="~/controls/checkout/payment-infor.ascx" TagName="payment" TagPrefix="uc2" %>
<%@ Register Src="~/controls/checkout/cart-summary.ascx" TagName="cart" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">

    <div id="orderconfirm">
        <div class="pending" id="divPending" runat="server" visible="false">
            Your order is currently pending. eCheck payment generally takes 3-5 business days
            to be cleared, sometimes a little bit longer. You will get an email order confirmation
            and we will process your order as soon as your check has been cleared. Thank you
            for your business.
        </div>
        <div class="msg">
            <div class="thanks">
                Thanks for your order,
                <asp:Literal ID="ltrCustomername" runat="server"></asp:Literal>!
            </div>
            <div class="des">
                <p>
                    If you need to check the status of your order or tracking number, please log on
                    to your account or click on one of the links below. If you do not receive your package 14 days after shipping date, please contact FedEx
                    or your local Post Office with the tracking #.</p>
            </div>
            <nav class="link">
                <ul>
                 <li>
                    <a href="/members/orderhistory/">Your Orders</a>
                 </li>
                 <li class="sep">|</li>
                 <li>   
                    <a href="/members/">Your Account</a>
                 </li>
                 <li class="sep">|</li>
                 <li>
                    <a href="/">Nailsuperstore.com</a>
                 </li>
                </ul>
                 
            </nav>
        </div>
       
        <uc1:cart ID="ucCart" runat="server" />
       
    </div>

</asp:Content>
