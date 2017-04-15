<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="Store_Default" %>
<%@ Register TagName="Filter" TagPrefix="CC" Src="~/controls/product/Filter.ascx" %>
<%@ Register src="~/controls/product/product-list.ascx" tagname="product" tagprefix="uc1" %>
<%@ Import Namespace="Utility" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="nct-fltr" class="pull-right"></div>
<h1 class="c-h1"><%--<asp:Literal id="litDepTitle" runat="server"></asp:Literal>--%><%=DepTitle %></h1>

<div class="line" style="clear:both"></div>
<div id="content-left" style="display:none"></div>  
<div id="cat-desc" class="dept-desc"><%= Department.Description%></div>
<div id="fct-fltr"><CC:Filter id="fltr" runat="server"></CC:Filter></div>
<%--<uc1:product ID="ucListProduct" runat="server" />--%>
<asp:PlaceHolder ID="phListItem" runat="server"></asp:PlaceHolder>
</asp:Content>

