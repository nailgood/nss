<%@ Page Language="VB" AutoEventWireup="false" CodeFile="gallery-detail.aspx.vb" Inherits="includes_popup_gallery_detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link href="/includes/theme/css/resource-center.css" rel="stylesheet" type="text/css" /> Edit css.xml--%>
    <asp:Literal ID="litCSS" runat="server"></asp:Literal>
</head>
<body id="bodyGalery">
    <form id="form1" runat="server">
        <div id="pop-gallery">
            <div id="dvLargeImg">
                <div id="largedImage">
                    <img src="<%=imgDetail %>"  id="imgEnlargedImage" class="imglarge" alt="<%=artName %>" />
				</div>
                <div id="relatedImage"  runat="server"></div>
            </div >
            <div id="dvInfoImg">
                <div class="title"><%=title%></div>
				<div class="text-content"><%=salonName%></div>
				<div class="text-content"><%=instruction%></div>
            </div>
	    </div>
  <script language="javascript">
      function updateAlternateimg(sUrl, altText) {
          document.getElementById("imgEnlargedImage").src = sUrl.replace('list', 'admin');
          document.getElementById("imgEnlargedImage").alt = altText;

      }
      function ChangeCss(id) {

          var el = document.getElementById("lisImg").getElementsByTagName("li");
          var getId;
          for (var i = 0; i < el.length; i++) {
              getId = el[i].id;
              if (getId == id) {
                  document.getElementById(getId).className = "active";
              }
              else {
                  document.getElementById(getId).className = "normal";
              }

          }
      }
      
   </script>
    </form>
</body>
</html>
