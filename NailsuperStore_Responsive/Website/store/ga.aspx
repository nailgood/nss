<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ga.aspx.vb" Inherits="store_ga" EnableViewState="false" %>
<%@ Register TagPrefix="CC" TagName="ga" Src="~/controls/layout/google-analytics.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/includes/scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">
        function submitGA(orderid) {
            $.ajax({
                url: '/store/ga.aspx/SubmitGA',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{orderId: '" + orderid + "'}",
                success: function (response) {
                    console.log('submit GA success.')
                },
                failure: function (response) {
                    console.log('submit GA failure.')
                }
            });
        }
        function sendError(err) {
            $.ajax({
                url: '/store/ga.aspx/SendError',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{err: '" + err + "'}",
                success: function (response) {
                    
                },
                failure: function (response) {
                    
                }
            });
        }
        //$(document).ready(function () {
        //    submitGA('abc')
        //});
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <cc:ga ID="ga" runat="server"></cc:ga>

        </div>
    </form>
</body>
</html>
