<%@ Control Language="VB" AutoEventWireup="false" CodeFile="top-banner.ascx.vb" Inherits="controls_banner_top_banner" %>
<div class="top-banner">
    <ul>
        <li class="top-banner-item">
            <a href="/services/free-shipping-policies.aspx">
                   <span class="ic-free-ship pull-left"></span>
                   <span class="pull-left text">
                       <span class="name">Free Shipping</span>
                       <span class="desc">with purchase of $<%=Utility.ConfigData.FreeShippingOrderAmount() %></span>
                    </span>
                   <span class="pull-right line-right"></span>
            </a>
        </li>
        <li class="top-banner-item">
        <a href="/free-samples">
            <span class="ic-free-sample pull-left"></span>
            <span class="pull-left text">
                       <span class="name">Free Samples</span>
                       <span class="desc">for orders over $99</span>
            </span>
            <span class="pull-right line-right"></span>
        </a>
        </li>
        <li class="top-banner-item">
           <a href="/free-gift">
                <span class="ic-free-gift pull-left"></span>
                <span class="pull-left text">
                           <span class="name">Free Gift</span>
                           <span class="desc">for orders over $150</span>
                </span>
                <span class="pull-right line-right"></span>
            </a>
        </li>
          <li class="top-banner-item">
              <a href="/services/reward-point-program.aspx">
                <span class="ic-rewards-points pull-left"></span>
                <span class="pull-left text">
                           <span class="name">Rewards Points</span>
                           <span class="desc">earn from orders & reviews</span>
                </span>
              </a>
          </li>
    </ul>
   <ul class="xsmall">
        <li class="top-banner-item free-ship"><a href="/services/free-shipping-policies.aspx">
            Free Shipping</a></li>
       
        <li class="top-banner-item free-sample"><a href="/free-samples">Free Samples </a>
        </li>
       
        <li class="top-banner-item free-gift"><a href="/free-gift">Free Gift</a> </li>
    </ul>
</div>