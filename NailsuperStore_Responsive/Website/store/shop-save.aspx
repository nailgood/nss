<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="shop-save.aspx.vb" Inherits="Store_ShopSave" %>
<%@ Register src="~/controls/product/list.ascx" tagname="product" tagprefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    
    <h1 class="c-h1"><asp:Literal id="litDepTitle" runat="server"></asp:Literal></h1>
    <div class="line" style="clear:both"></div>
    <asp:Literal id="litBanner" runat="server"></asp:Literal>
    <div id="cat-desc" class="dept-desc"><asp:Literal id="litDescription" runat="server"></asp:Literal></div>
    <uc:product ID="ucListProduct" runat="server" />
    <script>
        $(window).load(function () {
            fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false); //1:container, 2:line show,  3:line height, 4: min space of word last line, 5: min left postion read more, 6: word add,7: end call function
        });
    </script>
</asp:Content>
