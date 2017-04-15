<%@ Page Language="VB" AutoEventWireup="false" CodeFile="view.aspx.vb" Inherits="tips_view" MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="page">
        <div id="tips-detail">        
            <div class="title">
                <asp:Literal id= "lblTitle" runat="server"></asp:Literal>
            </div>
            <div class="border">
                <div runat="server" id="divTip">
                </div>
                <script type="text/javascript" src="/includes/youtube-auto-iframes.min.js"></script>
            </div>
        </div>
    </div>
</asp:Content>