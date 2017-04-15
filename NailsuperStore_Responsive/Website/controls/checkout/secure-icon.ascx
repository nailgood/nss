<%@ Control Language="VB" AutoEventWireup="false" CodeFile="secure-icon.ascx.vb" Inherits="controls_checkout_secure_icon" %>
 <div class="secure visible-md visible-lg">
     <% If IsCartPage %>
     <div class="secure-cart">
         <i class="fa fa-lock fa-lg" style="color:Green;padding-right:5px;"></i> 256-Bit evSSL Secure Encryption
         <div class="msg">
            <i>Your personal information is encrypted and secure.</i>
         </div>
         <div class="icon"><img style="cursor:pointer;cursor:pointer" src="/includes/scripts/siteSeal/siteseal_gd.png" onclick="verifySeal();" alt="SSL site seal - click to verify"></div>
     </div>
     <% Else %>
    <span id="siteseal">
        <img style="cursor:pointer;cursor:pointer" src="/includes/scripts/siteSeal/siteseal_gd.png" onclick="verifySeal();" alt="SSL site seal - click to verify">   
    </span>
     <% End If %>
</div>