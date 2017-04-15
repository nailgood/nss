<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="true" CodeFile="collection.aspx.vb" Inherits="Store_Collection" %>
<%@ Register src="~/controls/product/list.ascx" tagname="product" tagprefix="uc" %>
<%@ Register src="~/controls/product/item-description.ascx" TagName="item" TagPrefix="uc" %>
<%@ Register src="~/controls/product/review-list.ascx" TagName="listreview" TagPrefix="uc" %>
<%@ Register Src="~/controls/product/Filter.ascx" TagName="Filter" TagPrefix="uc" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <header id="h-product">
    <h1><asp:Literal id="litDepTitle" runat="server"></asp:Literal></h1>

    <nav>
         <ul class="pull-left">
             <li><asp:Literal ID="ltrHeaderReview" runat="server"></asp:Literal></li>
              <li class="countreview">
                  <a href="#customer-review">
                  <asp:Literal ID="ltrHeaderReviewCount" runat="server"></asp:Literal></a>
              </li>
              <li>
                  <a href="#description">Description</a>
              </li>
              <li>
                  <asp:Literal ID="ltrHeaderRelated" runat="server"></asp:Literal>
              </li>
              <li>
                  <asp:Literal ID="ltrInstructionHeader" runat="server"></asp:Literal>
              </li>
          </ul>                    
          <div class="collection-header-cart">
               <input type="button" value="Add to Cart" onclick="fnGetListQty();">
          </div>
    </nav>
</header>
<div id="content-left" class="hidden-md hidden-lg"></div>
<div id="fct-fltr"><uc:Filter id="ucFilter" runat="server"></uc:Filter></div>
<uc:product ID="ucListProduct" runat="server" />
<div class="collection-bottom-cart visible-xs"><input type="button" onclick="fnGetListQty();" value="Add to Cart"></div>

<div id="item-detail">
    <div class="data" style="width:100%">
        <a id="description">&nbsp;</a>
        <uc:item ID="ucDescription" runat="server" />
        
        <section class="review-section" id="review-section">
            <div class="label"><a id="customer-review">&nbsp;</a>Customer Reviews</div>
            <div class="content">
                <uc:listreview ID="ucProductReview"  runat="server" />
            </div>
        </section>
    </div>  
</div>

<script language="javascript">
    function isFixPageHeader() {
        return $("#top-bar").hasClass("fixed-top");
    }
</script>
</asp:Content>

