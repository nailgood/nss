<%@ Page Language="VB" AutoEventWireup="false" CodeFile="special-offer.aspx.vb" Inherits="Components.Popup_SpecialOffer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href='//fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' />
   <%-- <link href="/includes/theme/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/default.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/mixmatchpopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/includes/scripts/jquery-1.7.1.min.js"></script> Edit css.xml--%>
       <asp:Literal ID="litCSS" runat="server"></asp:Literal>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <meta name="robots" content="noindex, nofollow" />
</head>
<body>
  
    <div id="promotion-main">
        <section class="list" id="divPurchase" runat="server">
            <h3>These items are available for purchase in this promotion</h3>
            <div class="list">
                 <asp:Repeater runat="server" ID="rptPurchase">
                    <ItemTemplate>
                         <div class="item">
                         <asp:Literal ID="ltrItem" runat="server">
                         </asp:Literal>
                     </div>
                    </ItemTemplate>
                 </asp:Repeater>
                
            </div>
        </section>
          <section class="list discount" id="divDiscount" runat="server">
            <h3>These items are available for discount in this promotion</h3>
            <div class="list">
               <asp:Repeater runat="server" ID="prtDiscount">
                    <ItemTemplate>
                         <div class="item">
                         <asp:Literal ID="ltrItem" runat="server">
                         </asp:Literal>
                     </div>
                    </ItemTemplate>
                 </asp:Repeater>
            </div>
        </section>
    </div>
     <%--<script type="text/javascript" src="/includes/scripts/layout.js"></script> Edit css--%>
    <asp:Literal ID="litScriptCustom" runat="server"></asp:Literal>
  <script type="text/javascript">
      
    
      $(window).load(function () {
          ResetHeightList('#divPurchase .list .item', 'mixmatch');
          ResetHeightList('#divDiscount .list .item', 'mixmatch');
      });
      $(window).resize(function () {
          ResetHeightList('#divPurchase .list .item', 'mixmatch');
          ResetHeightList('#divDiscount .list .item', 'mixmatch');
      });
  </script>
  
</body>
</html>
