<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-samples-cart.ascx.vb" Inherits="controls_checkout_free_samples_cart" %>
    <section id="secFreeSamples">
        <div style="width: 90%; float:left; overflow: hidden;">
            <span class="lblsub">FREE Samples</span><asp:Literal ID="litFreeSamplesMsg" runat="server"></asp:Literal>
            <%--<div id="icon" class="fclose" onclick="OpenFreeSamples();"></div>--%>
        </div>
        
         <i class="fa fa-angle-double-down" onclick="OpenFreeSamples();"></i>
        <div id="divListSamples"></div>
    </section>
    