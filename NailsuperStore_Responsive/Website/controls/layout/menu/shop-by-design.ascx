<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shop-by-design.ascx.vb" Inherits="controls_layout_shop_by_design" %>
<nav class="left-nav">
    <div id="divShopByDesign" class="titleroot"> Shop by Design</div>
    <asp:Literal ID="ltrLink" runat="server"></asp:Literal>
</nav>
    <script type="text/javascript">
        $(document).ready(function () {
            CheckShowBreadCrumbMenuPopup('divShopByDesign', 'Shop by Design');
        });
        $(window).resize(function () {
            CheckShowBreadCrumbMenuPopup('divShopByDesign', 'Shop by Design');
        });
</script>