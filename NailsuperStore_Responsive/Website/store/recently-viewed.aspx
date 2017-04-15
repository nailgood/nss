<%@ Page Language="VB" AutoEventWireup="false" CodeFile="recently-viewed.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="Store_RecentlyViewed" %>
<%@ Register src="~/controls/product/list.ascx" tagname="product" tagprefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    
    <div class="line" style="clear:both"></div>
    <div id="content-left" style="display:none"></div>  
    <h1 class="c-h1">Recently View</h1>
    <uc:product ID="ucListProduct" runat="server" />
    <script>
        function deleteRecentlyItem(removeLink) {
            var itemId = $(removeLink).parent().parent().attr("data-itemid");
            var itemName = $(removeLink).parent().parent().attr("data-itemname");
            mainScreen.ExecuteCommand('deleteRecentlyViewItem', 'methodHandlers.deleteRecentlyViewItemCallback', [itemId, itemName]);
        }
        methodHandlers.deleteRecentlyViewItemCallback = function (temp, itemId, itemName) {
            location.reload();
            //var items = undefined;
            //if (itemId != '0')
            //    items = $('div[data-itemid="' + itemId + '"]');
            //if (items === undefined && itemName.length > 0)
            //    items = $('div[data-itemname="' + itemName + '"]');

            //if (items.length == 1) {
            //    $(items[0]).next().remove();
            //    $(items[0]).remove();

            //    if ($('#list-item').children().length == 1) {
            //        $('#line-fix').css('display', 'none');
            //        $('#list-item').replaceWith('<div id="list-item">We are sorry! No items were found that match your selection criteria.</div>');
            //    }
            //    fnSetheightverlineDealsCenter(1);
            //    fnOpenLinkAccountCart();
            //    hideAccountArrow();
            //    SendInfo();
            //}
        }
    </script>
</asp:Content>