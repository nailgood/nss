<%@ Control Language="VB" AutoEventWireup="false" CodeFile="resource-center.ascx.vb" Inherits="controls_layout_menu_resource_center" %>
<nav class="left-nav">
    <div id="divResourceCenter" class="titleroot">Resource Center</div>
    <ul id="resource-center">
    <% If String.IsNullOrEmpty(Args) Then%>
         <%= GenerateGroupMenu("", "Expert Tips & Advice", "/tips")%>
          <%= GenerateGroupMenu("", "Photo Gallery", "/gallery/nail-art-trend")%>
          <%= GenerateGroupMenu("video", "How-To Videos", "/video-topic")%>
          <%= GenerateGroupMenu("news", "News & Events", "/news-topic")%>
          <%= GenerateGroupMenu("", "Blog", "/blog")%>
          <%= GenerateGroupMenu("", "As Seen In Media", "/media-topic")%>
          <%= GenerateGroupMenu("", "Customer Reviews", "/order-reviews")%>
          <%= GenerateGroupMenu("", "Product Reviews", "/product-reviews")%>
          <%= GenerateGroupMenu("msds", "Material Safety Data Sheet", "/service/msds.aspx")%>
      <%End If%>      
    </ul>
</nav>
<script type="text/javascript">
    $(document).ready(function () {
        if (typeof CheckShowBreadCrumbMenuPopup !== 'undefined')
            CheckShowBreadCrumbMenuPopup('divResourceCenter', 'Resource Center');
    });
    $(window).resize(function () {
        if (typeof CheckShowBreadCrumbMenuPopup !== 'undefined')
            CheckShowBreadCrumbMenuPopup('divResourceCenter', 'Resource Center');
        var url = $('#hidUrl').val();
        if ((url.indexOf('product-list') != -1 || url.indexOf('order-list') != -1) && ViewPortVidth() < 992) {
            window.location.href = window.location.pathname;
        }
    });
</script>
