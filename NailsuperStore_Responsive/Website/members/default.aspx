<%@ Page Language="vb" AutoEventWireup="false" Inherits="members_default" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" %>
<%@Import Namespace="System.Configuration.ConfigurationManager"%>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <style type="text/css">
        @media (max-width: 991px){
             #content-page{padding-right: 0px;}
        }
    </style>
        <div class="panel-content">
           <%-- <div class="title">My Account Details</div>
            <div class="content">--%>
                <section class="sec-acc">
                    <div class="col-sm-12 sec-block" id="acc-detail">
                        <div class="sec-title">Account Details</div>
                        <div class="sec-content">
                            <asp:literal id="ltlAccountDetails" runat="server" />
                        </div>
                        <div class="sec-content-btn">
                            <div class="content-row">
                                <div>
                                    <asp:Literal ID="btnUnsubscribe" runat="server" Text="<a href='/members/unsubscribe.aspx'>Unsubscribe Email</a>"></asp:Literal>
                                    <a href="/members/account.aspx">Edit</a>
                               </div>
                            </div>
                       </div>
                    </div>
                   <%-- <div class="col-sm-6 sec-block ">
                         <div class="sec-title">Account Navigation</div>
                         <div class="sec-content">
                            <ul>
                                <li><a href="/members/orderhistory/">Order History</a></li>
                                <li><a href="/members/purchased-product.aspx">Purchased Product History</a></li>
                                <li><a href="/members/LeaveReview.aspx">Order / Product Review</a></li>
                                <li><a href="/members/creditmemo/">Credit Memo</a></li>
                                <li><a href="/members/referfriend/manager.aspx">Refer Friends</a></li>
                                <%If Not Components.ShoppingCart.MemberInGroupWHS(DB, memberID) Then%><li><a href="/members/pointbalance.aspx">Cash Reward Points Balance</a></li><% End If%>
                                <li><a href="/members/addressbook/">Address Book</a></li>
                                <li><a href="/members/reminders/">Reminders</a></li>
                                <li><a href="/members/logout.aspx">Sign Out</a></li>
                            </ul>
                        
                        </div>
                    </div>--%>
                </section>
                <section class="sec-acc">
                    <div class="col-sm-6 sec-block" id="billing-addr">
                         <div class="sec-title">Billing Address</div>
                         <div class="sec-content">
                            <asp:literal id="ltlBillingDetails" runat="server" />
                        </div>
                        <div class="sec-content-btn">
                            <a href="/members/address.aspx">Edit</a>
                        </div>
                    </div>
                    <div class="col-sm-6 sec-block">
                         <div class="sec-title">Shipping Address</div>
                         <div class="sec-content">
                            <asp:literal id="ltlShippingDetails" runat="server" />                        
                        </div>
                        <div class="sec-content-btn">
                            <a href="/members/address.aspx">Edit</a>
                        </div>
                    </div>
                </section>
           <%-- </div>--%>
        </div>
</asp:Content>