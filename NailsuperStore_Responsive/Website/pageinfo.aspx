<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" EnableViewState="false" CodeFile="pageinfo.aspx.vb" Inherits="PageInfo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel="stylesheet" type="text/css" href="/includes/Theme/css/pageinfo.css">
    <asp:Literal ID="litMetaRobots" runat="server"></asp:Literal>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<script>
    $(window).load(function () {
        infoheight('.csdefault li');
    });

</script>
    <h1><asp:Literal ID="litTitle" runat="server"></asp:Literal></h1>
    <div class="pageinfo">
        <asp:Literal ID="litContent" runat="server"></asp:Literal>
    </div>

</asp:Content>