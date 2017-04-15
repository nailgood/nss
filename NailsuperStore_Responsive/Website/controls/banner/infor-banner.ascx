<%@ Control Language="VB" AutoEventWireup="false" CodeFile="infor-banner.ascx.vb"
    Inherits="controls_banner_infor_banner" %>
<%@ Register Src="infor-banner-item.ascx" TagName="infor" TagPrefix="uc1" %>
<div id="secInforBanner" class="infor-banner">
    <div class="hidden-line">&nbsp;</div>
    <asp:PlaceHolder ID="phdItem" runat="server"></asp:PlaceHolder>
</div>
