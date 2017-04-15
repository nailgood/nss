<%@ Control Language="VB" AutoEventWireup="false" CodeFile="checkout-header.ascx.vb"
    Inherits="controls_checkout_checkout_header" %>
    <a href="#" id="nyroModal" target="_blank"></a>
<section class="checkout-header">
     <div id="divLogo" class="logo" runat="server">
        <img src="/includes/theme/images/logo-checkout.png" />
    </div>
    <div class="address">
        3804 Carnation St<br />
        Franklin Park, IL 60131, USA<br />
        <a href="/contact/default.aspx">customerservice@nss.com</a>
    </div>
</section>
<div class="checkout-bar">
    <table class="text">
        <tr>
            <td class="cart" id="tdText_1">
                <%If (String.IsNullOrEmpty(step1Link)) Then%>
                    Shopping Cart
                <%Else%>
                <a href="<%=step1Link %>">Shopping Cart</a>
                <%End If%>                 
                    
            </td>
            <td class="shipping" id="tdText_2">
                    <%If (String.IsNullOrEmpty(step2Link)) Then%>
                        Shipping & Payment  
                    <%Else%>
                        <a href="<%=step2Link %>">Shipping & Payment</a>                  
                    <%End If%>
                
            </td>
            <td class="confirm" id="tdText_3">
                    Order Confirmation
            </td>
        </tr>
    </table>
    <table class="icon" cellpadding="0" cellspacing="0">
        <tr>
            <td class="<%=GetNodeNumberClass(1) %>" id="tdNumber_1">
                1
            </td>
            <td class="<%=GetNodeBarClass(2) %>" id="tdNumberBar_2">
                <hr />
            </td>
            <td class="<%=GetNodeNumberClass(2) %>" id="tdNumber_2">
                2
            </td>
            <td class="<%=GetNodeBarClass(3) %>" id="tdNumberBar_3">
                <hr />
            </td>
            <td class="<%=GetNodeNumberClass(3) %>" id="tdNumber_3">
                3
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    var methodHandlers = {};
</script>