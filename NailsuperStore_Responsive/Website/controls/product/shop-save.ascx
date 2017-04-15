<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shop-save.ascx.vb" Inherits="controls_ShopSave" %>
<%@ Register Src="shop-save-item.ascx" TagName="shop" TagPrefix="uc1" %>
<%  If (Not lstCollection Is Nothing AndAlso lstCollection.Count > 0) Then%>
<div class="shopway <%=strclass %>">
    <div class="shopsave-title">
        <%= title%></div>
    <div class="data" id="shopsavedata_<%=Type %>">
        <asp:Literal ID="ltrData" runat="server"></asp:Literal>
    </div>
    <div class="slider" id="shopsaveslider_<%=Type %>">
        <div class="bxslider" id="ulSlideContent" runat="server">
        </div>
    </div>
</div>
<%  End If%>
