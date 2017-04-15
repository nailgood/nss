<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="list.aspx.vb" Inherits="shop_design_list" %>
<%@ Register Src="~/controls/shop-design.ascx" TagName="ShopDesign" TagPrefix="ucShopDesign" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <%--<link rel="stylesheet" type="text/css" href="/includes/Theme/css/shopdesign.css" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
<h1 id="hsDesign"><asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>  

<div id="shop-designlst" style="float:left;width:100%;">
    <asp:Repeater ID="rptlist" runat="server">
        <ItemTemplate>
           <asp:Literal ID="ltrCategory" runat="server"></asp:Literal>
            <ucShopDesign:ShopDesign ID="dvShopDesign" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
</div>
<input type="hidden" id="hidCategoryId" value="<%=CategoryId %>" />
<input type="hidden" id="hidVideoIndex" clientidmode="Static" value="<%=hidShopDesignIndex %>" />
 <input type="hidden" id="hiCountdataVideo" value="<%=TotalRecords %>" />
<input type="hidden" id="hidPageSizeVideo" value="<%=PageSize %>" />
<input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
<div style="display:block;text-align:center" id="imgloadingVideo">
        <img id="loader" alt="" src="<%=Utility.ConfigData.CDNMediaPath %>"/includes/theme/images/loader.gif" style="display: none" />
</div>
<div id="loadmoreVideo" onclick="GetRecordsResourceCenter(0,pgsize);" class="see-more-items"  style="display:none">
      <%=Resources.Msg.SeeMoreData%>
</div>
<script type="text/javascript">
    var pgsize = document.getElementById('hidPageSizeVideo').value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdataVideo').value,
        isLoading = 0;

    if (pageCount - pageIndex * pgsize > 0) {
        $("#loadmoreVideo").show();
        //  alert('1');
    }
    $(window).scroll(function () {
        var url = document.location.href;
        var i = url.indexOf("shop-by-design-collection");
        var arr = url.substring(i, url.length).split('/');
        //  alert(arr.length);
        if (arr.length == 1) {
            $("#loadmoreVideo").hide();
        }
        else {
           
            ScrollDataResourceCenter();
        }
    });
    $(window).load(function () {
        DelayResetHeightListVideo(false, 'shopdesign');
    });
    $(window).resize(function () {
        ResetHeightListVideo(true, 'shopdesign');
    });
</script>
</asp:Content>

