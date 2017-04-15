<%@ Control Language="VB" AutoEventWireup="false" CodeFile="about-us.ascx.vb" Inherits="controls_layout_about_us" %>
 <div id="main-about">
    <div class="about">
        <div id="about-shortdesc" class="col-md-5">   
            <div id="thumb_about" runat="server"><asp:Literal ID="ltrImg" runat ="server"></asp:Literal></div>         
            <div id="title-about"><asp:Literal ID="ltrtitle" runat ="server"></asp:Literal></div>
        </div>
        <div id="about-desc" class="col-md-7 dept-desc">
            <asp:Literal ID="ltrDesc" runat ="server"></asp:Literal>
        </div>
    </div>
</div>