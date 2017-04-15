<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="main.aspx.vb" Inherits="admin_main" %>
<%@ Register TagPrefix="Ctrl" TagName="AdminLastLoginActivity" Src="~/controls/AdminLastLoginActivity.ascx" %>

<asp:content ID="Content" runat="server" contentplaceholderid="ph">
<div style="margin-right:10px;float:left">
    <Ctrl:AdminLastLoginActivity id=ctrlAdminLastLoginActivity runat=server></Ctrl:AdminLastLoginActivity>
</div>

<div style="padding-top:50px;">
    <p>
        <strong>Server Time</strong>: 
        <asp:Label runat="server" ID="lbServerTime"></asp:Label>
    </p>
    <p>
        <strong>Your Time</strong>: 
        <span id="lbYourTime"></span>
    </p>
    <p>
        <strong>DB Name</strong>: 
        <asp:Label runat="server" ID="lbDBName"></asp:Label>
   </p>
   <p>
        <strong>Number Of Connections</strong>: 
        <asp:Label runat="server" ID="lblNoConnections"></asp:Label>
   </p>
    
</div>

<div style="margin-top:20px;">
    <CC:OneClickButton ID="ClearIndexSearch" runat="server" Text="Run Quick Search" CssClass="btn">
    </CC:OneClickButton>
</div>
<div style="margin-top:20px;">
    <CC:OneClickButton ID="updateCss" runat="server" Text="Refesh Css Script" CssClass="btn" OnClick="updateCss_Click">
    </CC:OneClickButton>
</div>
 <div style="margin-top:20px;">
    <CC:OneClickButton ID="btnClearCache" runat="server" Text="Clear Cache" CssClass="btn" OnClick="clearCache_Click" Visible="false">
    </CC:OneClickButton>
</div>
<script type="text/javascript">
    var now = new Date();
    var lb = document.getElementById('lbYourTime');
    lb.innerHTML = (now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear() + " " + now.getHours() + ":" + now.getMinutes() ;    
</script>
</asp:content>
