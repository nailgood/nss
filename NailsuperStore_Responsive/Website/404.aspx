<%@ Page Language="VB" AutoEventWireup="false" CodeFile="404.aspx.vb" Inherits="_404" MasterPageFile="~/includes/masterpage/main.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
    <meta name="robots" content="noindex, nofollow" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <h1>Page Not Found</h1>
    
    <div class="content">
        Sorry, an error has occurred on the page you are trying to access.
        It is possible that the page has been moved, or you typed the address incorrectly.
        <br /><br />
        Please click the link below to go to the Nailsuperstore.com home page.
        <br />
        <a href="/">www.nailsuperstore.com</a>
    </div>
    
    <div class="ps">
        If you have any problem, please don’t hesitate to <a href="/contact/default.aspx">contact us for further assistance.</a>
    </div>

</asp:Content>