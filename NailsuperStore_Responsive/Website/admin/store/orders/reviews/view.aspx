<%@ Page Language="VB" AutoEventWireup="false" CodeFile="view.aspx.vb" Inherits="admin_store_orders_reviews_view" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script language="jscript">
        window.name = "modal";
    </script>

    <script type="text/javascript" src="/includes/theme-admin/scripts/Browser.js">
    </script>
  <link href="/includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />
   

</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
    <div id="divReadReviews" style="padding: 20px" valign="top">
        <b>First Name:</b>
        <label id="lblname" runat="server">
        </label>
        <br />
        <br />
        <b>Rating:</b>
        <asp:Literal ID="ltImage" runat="server"></asp:Literal><br />
        <br />
        <b>Item arrived on time:</b>
        <label id="lblarrived" runat="server">
        </label>
        <br />
        <br />
        <b>Item as describled:</b>
        <label id="lbldescibled" runat="server">
        </label>
        <br />
        <br />
        <b>Prompt and counteous service:</b>
        <label id="lblservice" runat="server">
        </label>
        <br />
        <br />
        <b>Comment:</b>
        <label id="lblContent" runat="server">
        </label>
    </div>
    <div style="margin-left:20px;">
    <asp:Button ID="btnActive" runat="server" Text="Active" CssClass="btn" />
    <asp:Button ID="btnShare" runat="server" Text="Post" CssClass="btn" />
    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn" />
    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn" />
    </div>
    </form>
</body>
</html>
