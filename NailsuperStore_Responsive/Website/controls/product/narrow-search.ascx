<%@ Control Language="VB" AutoEventWireup="false" CodeFile="narrow-search.ascx.vb" Inherits="controls_product_narrow_search" %>
<%--<%@ Register src="product/top-brands.ascx" tagname="top" tagprefix="uc1" %>--%>
<div id="narrowsearch" runat="server" clientidmode="Static">
    <div class="title-right">
    </div>
    <div class="title-left">
    </div>
    <% If Not isReviewRating Then%>
<div class="title">Refine Results</div>
    <%End If%>
<div class="cate-filter">
    <div runat="server" id="dvCategories" >
        <div class="cate-title"><b class="arrow-down"></b>Category</div>
        <div class="cate-filter-content">
            <asp:Literal ID="ltCategory" runat="server"></asp:Literal>
        </div>
    </div>
    <div runat="server" id="dvBrand" >
        <div class="cate-title"><b class="arrow-down"></b>Brands</div>
        <div class="cate-filter-content">
            <asp:Literal ID="ltBrand" runat="server"></asp:Literal>
        </div>
    </div>

    <div runat="server" id="dvPrice" >
        <div class="cate-title"><b class="arrow-down"></b>Price</div>
            <ul class="parent">
                <asp:Literal ID="ltPriceContent" runat="server"></asp:Literal>
            </ul>
        </div>
    
    <div runat="server" id="dvCategory" visible="false" >
        <div class="cate-filter-title">Category</div>
        <div class="cate-filter-content">
            <asp:DropDownList style="margin-left:22px; width:185px;" AutoPostBack="true" ID="ddlCategory" runat="server">
            </asp:DropDownList>
        </div>
    </div>
    <div runat="server" id="dvRating" >
        <div class="cate-title"><b class="arrow-down"></b><asp:Label runat="server" id="lbRateTitle"></asp:Label></div>
        <div class="cate-filter-content">
            <ul class="parent">
                <asp:Literal ID="ltRatingContent" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>
    
    <div runat="server" id="dvPros" visible="false" >
        <div class="cate-filter-title">Pros</div>
        <div class="cate-filter-content">
            <ul class="parent">
                  <asp:Repeater ID="rpPros" runat="server">
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="hlPros" runat="server"  >
                            </asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>      
            </ul>
        </div>
    </div>
    <div runat="server" id="dvCons" visible="false">
        <div class="cate-filter-title">Cons</div>
        <div class="cate-filter-content">
            <ul class="parent">
                  <asp:Repeater ID="rpCons" runat="server">
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="hlCons" runat="server"  >
                            </asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater> 
            </ul>
        </div>
    </div>
    
    <div runat="server" id="dvExpLevel" visible="false" >
        <div class="cate-filter-title">Experience Level</div>
        <div class="cate-filter-content">
            <ul class="parent">
                <asp:Repeater ID="rpExpLevel" runat="server">
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="hlExpLevel" runat="server"  >
                            </asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater> 
            </ul>
        </div>
    </div>
   <%-- <uc1:top ID="top1" runat="server" />--%>
</div>
<div class="brand-footer" runat="server" id="footer"></div>
</div>
