<%@ Control Language="VB" AutoEventWireup="false" CodeFile="product-list.ascx.vb" Inherits="controls_product_product_list" %>
<%@ Register src="~/controls/product/product.ascx" tagname="product" tagprefix="uc1" %>
<%--<div id="cat-desc" class="dept-desc"><%=Description %></div>--%>
<%--<div id="track"></div>--%>
<div id="uplist">
<div id="line-fix" class="ver-line">&nbsp;</div>
<div id="list-item">
    <div class="ver-line-group">&nbsp;</div>
    <asp:Repeater id="rpListItem" runat="server">
        <ItemTemplate>
           <uc1:product ID="ucItem" runat="server" />
            <div class="ver-line">&nbsp;</div>
        </ItemTemplate>
    </asp:Repeater>
<%--    <asp:PlaceHolder ID="phListItem" runat="server"></asp:PlaceHolder>--%>
</div>
    <asp:Label ID="emtyList" runat="server"></asp:Label>
</div>
<input type="hidden" id="hidPageSize" value="<%=PageSize%>" />
<input type="hidden" id="hiCountdata" value="<%=ItemsCollectionCount %>" />
<input type="hidden" id="hidIndex" value="<%=ItemIndex %>" />
<%If Not (Request.Path.Contains("/store/department-tab.aspx")) Then%>
<input type="hidden" id="hidUrl"  value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<%End If%>
<input type="hidden" id="hiddomain" value="<%=Utility.ConfigData.GlobalRefererName %>" />
<input type="hidden" id="hidProductPages" value="<%=Utility.ConfigData.ProductPages %>" />
<input type="hidden" id="hidPageNotScroll" value="<%=Utility.ConfigData.PageNotScroll %>" />
<input type="hidden" name="hlastCart" value="" />
<input type="hidden" name="toplasttemp" value="" />
<input type="hidden" name="hlastinfotemp" value="" />
<div id="imgloading">
        <img id="loader" alt="Waiting" src="/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmore" onclick="GetRecords(0,pgsize,1);" class="see-more-items" style="display: none">
      <%=Resources.Msg.SeeMoreData %>
</div>
<script type="text/javascript" language="javascript">
    var pgsize = document.getElementById("hidPageSize").value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdata').value,
        itemindex = document.getElementById('hidIndex').value,
        cpgsize = 0;
    var hurl = $("#hidUrl").val();
    $(window).scroll(function () {

        if (hurl.indexOf("product-collection-detail" == -1)) {
             if (pgsize * pageIndex < pageCount && isLoading == 0) {
                //if (pgsize * pageIndex < pageCount) {
               ScrollData();

            }
        }

    });
    $(window).load(function () {
        $("#imgloading").css("right", $("#uplist").width() / 2);
        //only for collection detail, set equal list related
        if (hurl.indexOf("product-collection-detail") != -1) {
            equalheightAll('#list-item .item',0);
        }
    });
    $(window).resize(function () {
        if (hurl.indexOf("product-collection-detail") != -1) {
             equalheightAll('#list-item .item',0);
            }
    });
   if (pageIndex * pgsize < pageCount) {
      $("#loadmore").show();
     }
</script>