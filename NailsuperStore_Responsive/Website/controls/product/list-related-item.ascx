<%@ Control Language="VB" AutoEventWireup="false" CodeFile="list-related-item.ascx.vb" Inherits="controls_layout_list_related_item" %>
<%@ Register Src="~/controls/product/product-list.ascx" TagName="product" TagPrefix="uc1" %>

 <uc1:product ID="ucListProduct" runat="server" />
    <input type="hidden" runat="server" value="" id="hidTotal" />
    <input type="hidden" runat="server" value="" id="hidListItemId" />
