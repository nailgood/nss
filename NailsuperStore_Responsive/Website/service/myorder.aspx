<%@ Page Language="VB" AutoEventWireup="false" CodeFile="myorder.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master"
    Inherits="myorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <script type="text/javascript">
        function openCallback() {
            window.open('/RequestCallBack/Request.aspx', '_blank', 'menubar=0,location=0,scrollbars=auto,resizable=no,status=0,width=512,height=510');
        }
    $(window).load(function () {
        infoheight('.csdefault li');
    });

    </script>
    <div id="dContent" class="form-horizontal panel-content">
        <h1 class="formtitle">
            My Order</h1>
        <div class="content">
            <p>
                The answers to many of your questions are right at your fingertips. If you aren't
                able to the answer to your question on this page, please contact us.</p>
                <div class="pageinfo">
                    <ul class="csdefault">
                        <li id="info-1"><strong><span style="text-decoration: underline;">Order Status &amp; Tracking</span></strong><br />To view the status of an order you have placed at Nailsuperstore.com, click here to view <a class="clickhere" href="/members/orderhistory/">order history</a>.</li>
                        <li id="info-2"><strong><span style="text-decoration: underline;">Shipping Information</span></strong><br />Want to know how long it takes for orders to ship? How about estimated delivery time? Read our <a class="clickhere" href="/services/order-shipping-policies.aspx">Shipping Information page</a>.</li>
                        <li id="info-3"><strong><span style="text-decoration: underline;">Returns</span></strong><br />Find answers to your questions about returning or exchanging merchandise on our <a class="clickhere" href="/services/returns-policies.aspx">Returns Information page</a>.</li>
                        <li id="info-4"><strong><span style="text-decoration: underline;">Warranty Information</span></strong><br />Click here to request detailed warranty information for a specific product.</li>
                    </ul>
                </div>
            <%--<div class="border-return">
                <div class="bold">
                     Order Status &amp; Tracking</div>
                <div>
                    To view the status of an order you have placed at Nailsuperstore.com, click here to view <a class="clickhere" href="/members/orderhistory/">order history</a>.</div>
            </div>
            <div class="border-return">
                <div class="bold">
                    Shipping Information</div>
                <div>
                    Want to know how long it takes for orders to ship? How about estimated delivery
                    time? Read our <a class="clickhere" href="/services/order-shipping-policies.aspx">Shipping
                        Information page</a>.</div>
            </div>
            <div class="border-return">
                <div class="bold">
                    Returns</div>
                <div>
                    Find answers to your questions about returning or exchanging merchandise on our
                    <a class="clickhere" href="/services/returns-policies.aspx">Returns Information page</a>.</div>
            </div>
            <div class="border-return">
                <div class="bold">
                    Warranty Information</div>
                <div>
                    Click here to request detailed warranty information for a specific product</div>
            </div>--%>
            <div id="border-faq" style="border-top: 1px solid #c7c8c8;">
                <div class="bold">
                    FAQ's about Nailsuperstore.com orders</div>
                <p>
                    The answers you need are just a click away!</p>
                <ul>
                    <asp:Repeater runat="server" ID="rptFAQ">
                        <ItemTemplate>
                            <li>Q. <a href='/service/faq.aspx#FAQ<%#Container.DataItem("FaqId")%>'>
                                <%#Container.DataItem("Question")%></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <p>
                    Have more questions?<br />
                    Visit our complete list or <a class="bold clickhere" href="/service/faq.aspx">Frequently
                        Asked Questions</a> about Your thenailsuperstore.com Order</p>
                <hr />
                <p>
                    Not able to find the answer to your question? Contact us.</p>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function CheckValidEmail(sender, args) {
            var email = document.getElementById('txtEmail').value;
            if (email != '') {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (!re.test(email)) {
                    args.IsValid = false;
                    document.getElementById('cusEmail').innerHTML = 'Email Address is invalid';
                    return;
                }
            }
        }
    </script>
</asp:Content>
