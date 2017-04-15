<%@ Control Language="VB" AutoEventWireup="false" CodeFile="category.ascx.vb" Inherits="controls_NewsCategory" %>

<%@ Register Src="~/controls/layout/menu/resource-center.ascx" TagName="resource" TagPrefix="uchideResourceCenter" %>
<section id="news" runat="server">
    <div id="leftmenu">
        <div  id="divTitle" runat="server"></div>        
        <ul class="boxcontent">
            <asp:Repeater ID="rptCategoryNews" runat="server">
                <ItemTemplate>
                   <asp:Literal ID="ltrLink" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</section>
    <div id="categoryhide" style="display:none;">
        <uchideResourceCenter:resource ID="resource" runat="server" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            CheckShowBreadCrumbMenuPopup('categoryhide', 'Resouce Center');
        });
        $(window).resize(function () {
            CheckShowBreadCrumbMenuPopup('categoryhide', 'Resouce Center');
        });
</script>

