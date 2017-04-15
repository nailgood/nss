<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Filter.ascx.vb" Inherits="control_Filter" %>

<div id="filter" class="<%=css2 %>">
	<%--<div style="width:746px; height:30px; position:relative;">
		<div class="bold" style="top:10px; left:10px; position:absolute;">
			<%=Title%>
		</div>
    </div>--%>
<div class='cate-title hidden-md hidden-lg'><b class='arrow-down'></b><span id='lbFilter'>Filter By:</span></div>
<div class="pull-left filter-by <%=css %>">Filter By:</div>
<div class="<%=css1 %>">
    <asp:DropDownList runat="server" ID="ddlRange" Visible="false"/>
    <%--<span class="n-dropdown" id="sBrand" runat="server"><asp:DropDownList runat="server" ID="ddlBrands" AutoPostBack="true" CssClass="form-control"/></span>--%>
    <span class="n-dropdown" id="sCollections" runat="server"><asp:DropDownList runat="server" ID="ddlCollections" AutoPostBack="true" CssClass="form-control"/></span>
    <span class="n-dropdown" id="sTones" runat="server"><asp:DropDownList runat="server" ID="ddlTones" AutoPostBack="true" CssClass="form-control"/></span>
    <span class="n-dropdown" id="sShades" runat="server"><asp:DropDownList runat="server" ID="ddlShades" AutoPostBack="true" CssClass="form-control"/></span>
    <span class="n-dropdown" id="sFeatures" runat="server"><asp:DropDownList runat="server" ID="ddlFeatures" AutoPostBack="true" CssClass="form-control"/></span>

    <span class="n-dropdown" id="sSortBy" runat="server">
    <asp:DropDownList runat="server" ID="ddlSortBy" CssClass="form-control" onchange="ddlSortByChange(this)">
        <asp:ListItem Text="Sort By" Value="" />
        <asp:ListItem Text="Hot Items" Value="hot-items" />
        <asp:ListItem Text="On Sale" Value="on-sale" Enabled="false" />
        <asp:ListItem Text="Product Name" Value="product" Enabled="false" />
        <asp:ListItem Text="SKU" Value="sku" />
        <asp:ListItem Text="Newest" Value="new-items" />
        <asp:ListItem Text="Best Sellers" Value="best-sellers" />
        <asp:ListItem Text="Highest Rating" Value="top-rated" />
        <asp:ListItem Text="Most Rated" Value="most-popular-review" />
        <asp:ListItem Text="Prices Low to High" Value="price" />
        <asp:ListItem Text="Prices High to Low" Value="pricehigh" />
    </asp:DropDownList>
    <asp:HiddenField ID="hidSortBy" runat="server" />
    </span>
    <asp:Button runat="server" ID="btnGo" CssClass="g34" Text="Go" Visible="false" />

</div>

</div>
<input type="hidden" id="hidCountdr" value="<%=CountDr %>" />
<script language="javascript">
    $(window).load(function () {
        var CountDr = $("#hidCountdr").val();
        if (CountDr == 0) {
            var dinline = $("#fct-fltr").html();

            if ($('#box-search-replace').length = 0){ //replace keyword
                $("#fct-fltr").css("display", "none");
                $("#nct-fltr").html(dinline);
            }
            else {
                //$("#fct-fltr").css("float", "right");
            }
        }

        $("#filter").removeClass("hidden");

    });

    function ddlSortByChange(sel) {
        var ddl = sel.value;
        var url = $('#hidSortBy').val();
        
        if (ddl != '') {
            url += "sort=" + ddl;
        }
        else {
            if (url.endsWith('?'))
                url = url.substring(0, url.length - 1);
        }

        window.location.replace(url);
    }
</script>
