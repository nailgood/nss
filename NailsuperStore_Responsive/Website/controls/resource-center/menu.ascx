<%@ Control Language="VB" AutoEventWireup="false" CodeFile="menu.ascx.vb" Inherits="controls_ResourceCenter" %>
<%@ Register Src="~/controls/banner/infor-banner.ascx" TagName="inforBanner" TagPrefix="uc" %>
<section class="resourcecenter">  
        <div id="rs-header"> 
                 <div class="title">
                    <h5>Resource Center</h5>
                </div>  
               
        </div>  
         <uc:inforBanner ID="ucInforBanner" runat="server" />
        <div class="content">
            <div class="content-item">
                <div class="content-item-box">
                <a  href="/tips" class="item">
                    <span class="text">Expert Tips &amp; Advice</span>
                    <span class="tip"></span>
                    <span class="link">more info</span>
                </a>
            </div>
            </div>
              <div class="content-item">
                <div class="content-item-box">
                <a href="/gallery/nail-art-trend" class="item">                
                    <span class="text">Photo Gallery</span>
                    <span class="gallery"></span>
                    <span class="link">more info</span>             
                </a>
            </div>
            </div>
              <div class="content-item">
                <div class="content-item-box">
                <a href="/video-topic" class="item">                 
                <span class="text">How-To Videos</span>
                <span class="video"></span>
                <span class="link">more info</span>              
            </a> 
            </div>
            </div>
              <div class="content-item">
                <div class="content-item-box">
                <a href="/news-topic" class="item">
                   <span class="text">News &amp; Events</span>
                   <span class="news"></span>
                   <span class="link">more info</span>               
                 </a>
            </div>
            </div>
              <div class="content-item">
                <div class="content-item-box">
             <a href="/product-reviews" class="item">
              <span class="text">Product Reviews</span>
              <span class="review"></span>
              <span class="link">more info</span>                
            </a>        
            </div>
            </div>
         </div>        
  

</section>
