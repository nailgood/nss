<%@ Page Language="VB" AutoEventWireup="false" CodeFile="category.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="store_category" %>
<%@ Register TagPrefix="CT" Namespace="MasterPages" Assembly="Common" %>
<%@ Register src="~/controls/product/product-list.ascx" tagname="product" tagprefix="uc1" %>
<%@ Import Namespace="Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<h1 class="c-h1"><asp:Literal id="litDepTitle" runat="server"></asp:Literal></h1>
<div class="line" style="clear:both"></div>
<div id="content-left" style="display:none"></div>    
<%--<uc1:product ID="ucListProduct" runat="server" />--%>
<asp:PlaceHolder ID="phListItem" runat="server"></asp:PlaceHolder>
 </asp:Content>   
