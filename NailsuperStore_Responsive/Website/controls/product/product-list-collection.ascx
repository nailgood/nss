<%@ Control Language="VB" AutoEventWireup="false" CodeFile="product-list-collection.ascx.vb" Inherits="controls_product_product_list_collection" %>
<%@ Register src="~/controls/product/product-collection.ascx" tagname="product" tagprefix="uc1" %>
<%@ Register TagName="Filter" TagPrefix="CC" Src="~/controls/product/Filter.ascx" %>
<CC:Filter id="fltr" runat="server"></CC:Filter>
<div id="uplist">
<section id="clist-item">
    <asp:Repeater id="rpListItem" runat="server">
        <ItemTemplate>
           <uc1:product ID="ucItem" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
     <%--asp:PlaceHolder ID="phListItem" runat="server"></asp:PlaceHolder>--%>
</section>
   <div valign="bottom" style="display:block;text-align:center" id="imgloading">
        <img id="loader" alt="Waiting" src="/includes/theme/images/loader.gif" style="display: none" />
    </div>
    <div id="loadmore" onclick="GetRecords(0,pgsize,1);" class="see-more-items" style="display: none">
      <%=Resources.Msg.SeeMoreData %>
    </div>
     <asp:Label ID="emtyList" runat="server"></asp:Label>
</div>
<div class="collection-bottom-cart visible-xs"><input type="button" onclick="fnGetListQty();" value="Add to Cart"></div>

<input type="hidden" id="hidPageSize" value="<%=PageSize%>" />
<input type="hidden" id="hidIndex" value="<%=ItemIndex %>" />
<input type="hidden" id="hiCountdata" value="<%=ItemsCollectionCount %>" />
<input type="hidden" id="hiddomain" value="<%=Utility.ConfigData.GlobalRefererName %>" />
<%--<input type="hidden" id="hidPageindex" value="" runat="server" />--%>
<input type="hidden" id="hidProductPages" value="<%=Utility.ConfigData.ProductPages %>" />
<input type="hidden" id="hidPageNotScroll" value="<%=Utility.ConfigData.PageNotScroll %>" />
<input type="hidden" name="hlastCart" value="" />
<input type="hidden" name="toplasttemp" value="" />
<input type="hidden" name="hlastinfotemp" value="" />
<script type="text/javascript" language="javascript">

    var pgsize = document.getElementById("hidPageSize").value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdata').value
        itemindex = document.getElementById('hidIndex').value,
        cpgsize = 0;
    $(window).scroll(function () {
        if (pgsize * pageIndex < pageCount) {
                ScrollData();
        }

    });
    if (pageIndex * pgsize < pageCount) {
        $("#loadmore").show();
    }
    function showBonosOffer(id) {
        var link = '/includes/popup/special-offer.aspx?mmid=' + id

        var heightPopup = 490;
        var popupW = 800;


        $('#nyroModal').attr('href', link);
        $('#nyroModal').nyroModalManual({
            showCloseButton: true,
            bgColor: 'gray',
            zIndexStart: '9999999',
            modal: false,
            width: popupW,
            height: heightPopup,
            windowResize: true,
            titleFromIframe: false
        });

    }
</script>
