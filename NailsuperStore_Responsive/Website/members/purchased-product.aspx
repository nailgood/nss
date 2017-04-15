<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="purchased-product.aspx.vb" Inherits="members_purchased_product" %>

<%@ Register src="~/controls/product/purchased.ascx" tagname="order" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
<h1>Prices, promotions are the latest, not from last orders.</h1>
<div id="order-detail">

        <asp:Repeater id="rpListOrder" runat="server">
        <ItemTemplate>
            <uc1:order ID="ucOrder" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
</div>

<input type="hidden" id="hiCountdata" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSize" value="<%=PageSize %>" />
<input type="hidden" id="hidUrl" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="display:block;text-align:center" id="imgloading">
        <img id="loader" alt="" src="/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmore" onclick="GetOrderRecords(0,pgsize);" class="see-more-items" style="display: none">
      See More Item >>
</div>
<script type="text/javascript" language="javascript">
    var pgsize = 1,
        pageIndex = document.getElementById('hidPageSize').value,
        pageCount = document.getElementById('hiCountdata').value,
    isLoading = 0;
    $(window).scroll(function () {
          ScrollDataOrder();
    });
    if (pageIndex * pgsize < pageCount) {
        $("#loadmore").show();
    }
    methodHandlers.AddCartRewardPointCallBack = function (htmlReturn, maxqty, itemId, cartItemId, linkredirect, countitem) {
       if (linkredirect != '') {
           window.location.href = linkredirect;
           return;
       }

       var htmlPopupCart = '';
       var htmlError = '';
       if (htmlReturn != null) {
           if (htmlReturn.length > 0) {
               htmlError = htmlReturn[0];
               htmlPopupCart = htmlReturn[1];
           }
       }
       if (htmlPopupCart.length > 0) {
           document.getElementById("cart-down").innerHTML = "<div id='cart-modal'>" + htmlPopupCart + "</div>";
       }
       if (countitem.length > 0 && countitem.toString() != 'null') {
           $("#cart-count").html(countitem);
           $(".xs-cart-count").text(countitem);
       }

       if (htmlError.length > 0) {
           ShowError(htmlError);
       }

       var arrQty = maxqty.split("|");
       var arrItem = itemId.split("|");
       for (var i = 0; i < arrQty.length; i++) {
           // alert(arrQty[i]);
           if (arrQty[i] > 0) {
               /* show item with max qty */
               try {
                   var $txt = $('input[name="txtQtyItemPoint' + arrItem[i] + '"]');
                   if ($txt.length > 0) {
                       for (var y = 0; y < $txt.length; y++) {
                           $txt.eq(y).val(arrQty[i]);
                       }
                   }
               } catch (err) { alert(err); }
           }
           else if (arrQty[i] == 0) {
               /* show item add cart success */
               try {
                   var $lbl = $('div[name="lblInCartPoint' + arrItem[i] + '"]');
                   if ($lbl.length > 0) {
                       for (var y = 0; y < $lbl.length; y++) {
                           $lbl.eq(y).html('Added to your cart');
                           $lbl.eq(y).css("padding-top", "3px");
                       }
                   }

                   var $txt = $('input[name="txtQtyItemPoint' + arrItem[i] + '"]');
                   if ($txt.length > 0) {
                       for (var y = 0; y < $txt.length; y++) {
                           $txt.eq(y).val(0);
                       }
                   }
               } catch (err) { alert(err); }
           }
           else if (arrQty[i] == -2) {
               /* item out of stock */
               try {
                   var $txt = $('input[name="txtQtyItemPoint' + arrItem[i] + '"]');
                   if ($txt.length > 0) {
                       for (var y = 0; y < $txt.length; y++) {
                           $txt.eq(y).val(0);
                       }
                   }
               } catch (err) { alert(err); }
           }
       }

//       maxqty = parseInt(maxqty);
//       if (maxqty >= 0) {
//           if (document.getElementById('txtQtyItem' + itemId)) {
//               $('#txtQtyItem' + itemId).val(maxqty);
//           }
//       }
//       $("#lblInCartPoint" + itemId).html('Added to your cart');

   }

  
</script>

</asp:Content>
