<%@ Page Language="VB" AutoEventWireup="false" CodeFile="msg.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="msg" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
    <meta name="robots" content="noindex, nofollow" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

    <div id="msg">
        <h1><asp:Literal ID="ltrTitle" runat="server" Text="Message"></asp:Literal></h1>
        <div class="pageinfo">
        <div class="msg-content">
            <asp:Literal ID="ltrContent" runat="server" Text="This is content text"></asp:Literal>
        </div>
        
        <div class="ps">
            <b>Didn't get the email?</b> Please check your Spam/Junk email folder. If you didn't receive an email please <a href="/contact/default.aspx">contact us for further assistance.</a>
        </div>
        </div>
    </div>
</asp:Content>