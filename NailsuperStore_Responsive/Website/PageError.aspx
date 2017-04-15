<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PageError.aspx.vb" Inherits="PageError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <meta name="robots" content="noindex,follow">
    <link rel="/includes/style.css" type="text/css" />
    <style type="text/css">
        p
        {
            font-family:  'Open Sans' , sans-serif;
            font-size: 11px;
            color: #333333;
        }
        td
        {
            font-family:  'Open Sans' , sans-serif;
            font-size: 11px;
            color: #333333;
        }
        a
        {
            font-family:  'Open Sans' , sans-serif;
            font-size: 13px;
            font-weight: bold;
            color: #be048c;
            text-decoration: none;
        }
        a:link
        {
            color: #be048c;
        }
        a:active
        {
            color: #999999;
        }
        a:visited
        {
            color: #be048c;
        }
        a:hover
        {
            color: #be048c;
        }
    </style>

    <script type="text/javascript">
        function CountDown(backURL) {
            return;
            var countdownElement = document.getElementById('timeCount')
            var seconds = 10;
            var second = 0;
            var interval;
            interval = setInterval(function() {
                countdownElement.innerHTML = seconds - second;
                if (second >= seconds) {
                    countdownElement.innerHTML = 0;
                    window.location = backURL;
                }
                second++;
            }, 1000);
        }
        function ReturnPage() {
            var lnkBack = document.getElementById('lnkBack');
            if (lnkBack)
                lnkBack.click();
        }
    </script>

</head>
<body bgcolor="#ffffff" onload="CountDown('<%=backURL %>');">
    <div align="center" style="margin-top: 100px;">
        <table width="500" height="220" border="0" cellpadding="10" style="border: 1px solid #666666;">
            <tr>
                <td>
                    <p style="padding-left: 10px; font-size:14px;">Website is being updated. Please try to access it again in <span id="timeCount">10</span> seconds</p>
                    <p align="center" style="font-size:12px;">Please click  <a id="lnkBack" href="<%=backURL %>" class="maglnk">here</a> to return your current page.</p>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
