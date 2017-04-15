<%@ Page Language="VB" AutoEventWireup="false" CodeFile="review.aspx.vb" Inherits="includes_popup_review" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Write a review</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href='//fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' />
    <%--<link href="/includes/theme/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/default.css" rel="stylesheet" type="text/css" />
    <link href="/includes/theme/css/videopopup.css" rel="stylesheet" type="text/css" /> Edit css.xml--%>
    <asp:Literal ID="litCSS" runat="server"></asp:Literal>
</head>
<body id="bodyVideo">
    <form id="form1" runat="server">
        <h1><%=lbTitle%> a review</h1>
        <p>Now to post a comment</p>
        <asp:Panel ID="pnResult" runat="server">
            <asp:Literal ID="ltrResult" runat="server"></asp:Literal>
        </asp:Panel>

        <asp:Panel ID="pnField" runat="server">
            <div id="dvPostComment">
                <div class="form-group">
                    <asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" CssClass="placeholder form-control"></asp:TextBox>
                </div>
                <div class="btnSumit">            
                    <asp:Button runat="server" id="btnComment" CssClass="btn btn-submit" UseSubmitBehavior="false" Text="Submit" />
                </div>
            </div>
        </asp:Panel>
    
    </form>
</body>
</html>
