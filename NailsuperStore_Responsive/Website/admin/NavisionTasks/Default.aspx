<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_NavisionTasks_Default" title="Navision Tasks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">

    <h4>Navision Tasks</h4>
    <br />
    <asp:Literal runat="server" ID="lit" />
    
    <CC:OneClickButton runat="server" ID="btnExport" Text="Export Data" CssClass="btn" />
    <CC:OneClickButton runat="server" ID="btnImages" Text="Images" CssClass="btn" />
    <CC:OneClickButton runat="server" ID="btnProductImport" Text="Product Import" CssClass="btn" />
    <CC:OneClickButton ID="btnReminders" runat="server" CssClass="btn" Text="Send Availability Reminders" />
    <CC:OneClickButton ID="btnWishListEmails" runat="server" CssClass="btn" Text="Send Wish List Emails" />
    <CC:OneClickButton ID="btnPurgeViewedItems" runat="server" CssClass="btn" Text="Purge Viewed Items" />
    <CC:OneClickButton ID="tbnPurgeCC" runat="server" CssClass="btn" Text="Purge CC" />
    <br /><br /><br /><br /><br />
    
    
</asp:Content>

