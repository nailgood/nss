<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master"
    Inherits="service_default" %>

<%@ Register Src="~/controls/store-department-list.ascx" TagName="department" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="dbContent" class="pageinfo">
        Welcome to the new The Nail Superstore! We want your experience with us to be easy
        and fun... so we've updated our Customer Service section to put all you need to
        know right at your fingertips. You can track orders, view your order history, check
        your credit memo and more! And, our updated e-mail inquiry system lets you contact
        us in a snap. Thanks for visiting The Nail Superstore shopping!
        <div>
            <div class="boxcol">
                <div class="lblAsk bold mag">
                    My Account</div>
                <p>
                    It's all about you. Get answers to your questions about your information on The
                    Nail Superstore.</p>
                <ul class="lblAsk">
                    <%	For Each row As DataRowView In dvMyAccount%>
                    <li><a href="<%=row("Url")%>">
                        <%=row("Text")%></a></li>
                    <%	Next%>
                </ul>
                <div class="lblAsk bold mag">
                    Legal Notice and Privacy</div>
                <p>
                    <a class="clickhere" href="/service/privacy.aspx">Click here</a> to view our legal
                    notice and privacy policy.</p>
                <div class="lblAsk bold mag">
                    Price Match Request</div>
                <p>
                    Learn more about the Nail Superstore's <a class="clickhere" href="../contact/pricematch.aspx">price
                        match guarantee.</a></p>
            </div>
            <div class="boxcol">
                <div class="lblAsk bold mag">
                    Ordering & Shipping</div>
                <p>
                    Can't wait for your order? get more information about:</p>
                <ul class="lblAsk">
                    <%		For Each row As DataRowView In dvOrders%>
                    <li class="lblAsk"><a href="<%=row("Url")%>">
                        <%=row("Text")%></a></li>
                    <%	Next%>
                </ul>
                <div class="lblAsk bold mag">
                    Our Catalog</div>
                <p>
                    New to The Nail Superstore? <a class="clickhere" href="/service/catalog.aspx">Here's
                        all the info you need</a> to get started!</p>
            </div>
            <div class="boxcol">
                <div class="lblAsk bold mag">
                    Frequently Asked Questions</div>
                <p>
                    Get answers to the most commonly asked questions about The Nail Superstore. <a class="clickhere"
                        href="faq.aspx">Click here!</a></p>
                <div class="lblAsk bold mag">
                    Contact Us</div>
                <p>
                    Have a question? We're here to help! <a class="clickhere" href="/contact/default.aspx">
                        Click here!</a></p>
                <p style="margin-left: 10px">
                    The Nail Superstore<br />
                    3804 Carnation St<br />
                    Franklin Park, IL 60131<br />
                    Phone: 800.669.9430<br />
                    Fax: 773.275.6948
                </p>
                <div class="lblAsk bold mag lnpad4">
                    Site Map</div>
                <ul class="lblAsk t5">
                    <li class="lblAsk"><a href="/sitemap/default.aspx">View Entire Map</a> </li>
                </ul>
                <ul class="lblAsk">
                    <span class="lblAsk mag">View by Section </span>
                    <div id="lstStoreDepartment">
                        <ul>
                            <uc1:department ID="lstDepartment" runat="server" />
                        </ul>
                    </div>
                    <li><a href="/service/catalog.aspx">Catalog</a> </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
