<%@ Page Language="VB" AutoEventWireup="false" CodeFile="view.aspx.vb" Inherits="admin_store_items_reviews_view" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
   <script type="text/javascript" src="/includes/theme-admin/scripts/Browser.js"></script>
    
    <link href="~/includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />

</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
    <div style="border: 1px solid #DDDDDD; height: 130px; width: 596px">
        <div runat="server" id="divImg1" class="floatleft" style="padding-right: 20px">
            <img src="/assets/nobg.gif" id="img" style="height: 115px; width: 115px" alt="" />
        </div>
        <div style="padding: 0px 10px 10px 10px">
            <div class="padbot5">
                <h1>
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </h1>
            </div>
            <div class="titdes">
                <asp:Label ID="lblDesc" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <br />
    <div id="divReadReviews" valign="top">
        <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
    </div>
    <div style="margin-left: 20px;">
        <asp:Button  ID="btnActive" runat="server" Text="Active" CssClass="btn" />
        <asp:Button  ID="btnAddPoint" runat="server" Text="Add point" CssClass="btn" />
        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn" />
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn" />
    </div>
     <input type="hidden" runat="server" value="" id="hidPopUpReviewId" />
    <input type="hidden" runat="server" value="" id="hidSaveValue" />
    <input type="hidden" runat="server" value="" id="hidPopupReturn" />
    </form>
</body>
</html>
