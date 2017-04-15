<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="~/service/catalog-link.aspx.vb" Inherits="service_catalog_link" %>
<%@ Register src="~/controls/product/list.ascx" tagname="product" tagprefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <script language="javascript">
        $(window).load(function () {
            fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false); //1:container, 2:line show,  3:line height, 4: min space of word last line, 5: min left postion read more, 6: word add,7: end call function

            var url = window.location.href.replace(/[^a-zA-Z0-9,/.:?=-]/g, '');
            //alert("url: " + url);
            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(url);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }
            var returnQueryString = getParameterByName('sku');          
            var newStr = returnQueryString.replace(/[^a-zA-Z0-9,]/g, '');
            //window.location = "http://192.168.41.56:2015/service/catalog-link.aspx?sku=" + newStr;
        });
    </script>
    <h1 class="c-h1">Your choice in Catalog</h1>
    <asp:Literal id="litTest" runat="server"></asp:Literal>
    <uc:product ID="ucListProduct" runat="server" />
    

</asp:Content>
