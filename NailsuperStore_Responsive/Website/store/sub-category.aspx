<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="true" CodeFile="sub-category.aspx.vb" Inherits="Store_SubCategory" %>
<%@ Register TagName="Filter" TagPrefix="uc" Src="~/controls/product/Filter.ascx" %>
<%@ Register src="~/controls/product/list.ascx" tagname="product" tagprefix="uc" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="cphContent" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="nct-fltr" class="pull-right"></div>
<h1 class="c-h1"><%=DepTitle %></h1>
<div class="line" style="clear:both"></div>
<div id="content-left" class="hidden-md hidden-lg"></div>
<div id="cat-desc" class="dept-desc"><%= description%></div>
<div id="fct-fltr"><uc:Filter id="ucFilter" runat="server"></uc:Filter></div>
<uc:product ID="ucListProduct" runat="server" />
<script type="text/javascript">
    $(window).load(function () {
        $(".dept-desc").css("display", "block");
        fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false);
    });
    $(window).resize(function () {
        fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false);
    });
</script>
</asp:Content>

