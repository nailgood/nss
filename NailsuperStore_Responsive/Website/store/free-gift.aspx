<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/main.master" AutoEventWireup="false" CodeFile="free-gift.aspx.vb" Inherits="store_free_gift" %>
<%@ Register Src="../controls/product/free-item-page.ascx" TagName="free" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel="canonical" href="<%=Utility.ConfigData.GlobalRefererName  %>/free-gift" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <h1 id="hTitle" runat="server">Free Gift</h1>
    <div id="free-gift">
        <uc1:free ID="ucListItem" runat="server" />
    </div>
   
    <script type="text/javascript">
        function LoadFreeGift(level) {
            mainScreen.ExecuteCommand('GetFreeGiftByTotal', 'methodHandlers.GetFreeGiftByTotalCallBack', [level]);
        }
        methodHandlers.GetFreeGiftByTotalCallBack = function (htmlReturn, linkredirect) {

            if (linkredirect != '') {
                window.location.href = linkredirect; // '/store/free-sample.aspx';    
            }
            var htmlError = '';
            var htmlListFreeGift = '';
            var htmlFreeGiftLevel = '';
            if (htmlReturn != null) {
                if (htmlReturn.length > 0) {
                    htmlError = htmlReturn[0];
                    htmlListFreeGift = htmlReturn[1];
                    htmlFreeGiftLevel = htmlReturn[2];
                }
            }
            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            $('#freeitemList').replaceWith(htmlListFreeGift);
            $('#ulOrderLevel').replaceWith(htmlFreeGiftLevel);

            ResetHeightListFreeGift('.list .sample-item');
        }
      
        $(window).load(function () {
            fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false); //1:container, 2:line show,  3:line height, 4: min space of word last line, 5: min left postion read more, 6: word add,7: end call function

            ResetHeightListFreeGift('.list .sample-item');
           


        });
        $(window).resize(function () {
            ResetHeightListFreeGift('.list .sample-item');
        });
        function AddFreeGift(itemId) {
            var btn = $('#btnAddcart_' + itemId);
            var className = $('#btnAddcart_' + itemId).attr('class');
            if (className == 'in-cart') {
                return false;
            }
            mainScreen.ExecuteCommand('AddCartFreeGift', 'methodHandlers.AddCartFreeGiftCallBack', [itemId]);
            return false;
        }
        methodHandlers.AddCartFreeGiftCallBack = function (htmlReturn, linkredirect, cartItemCount, prevItem, currentItem) {

            if (linkredirect != '') {
                window.location.href = linkredirect; // '/store/free-sample.aspx';  
                return;
            }
            var htmlError = '';
            if (htmlReturn != null) {
                if (htmlReturn.length > 0) {
                    htmlError = htmlReturn[0];
                }
            }
            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            var currentPage = window.location.toString();
            if (currentPage.indexOf('?act=checkout') > 0) {
                ResetHeaderCartItemcount(cartItemCount);
                if (prevItem > 0) {
                    $('#btnAddcart_' + prevItem).removeClass("in-cart");
                    $('#btnAddcart_' + prevItem).addClass("add-cart");
                    $('#btnAddcart_' + prevItem).attr('value', 'Select');
                }
                if (currentItem > 0) {
                    $('#btnAddcart_' + currentItem).removeClass("add-cart");
                    $('#btnAddcart_' + currentItem).addClass("in-cart");
                    $('#btnAddcart_' + currentItem).attr('value', 'Selected');
                }
            }
            else {
                window.location.href = '/store/cart.aspx';
            }

        }
      
    </script>
</asp:Content>
