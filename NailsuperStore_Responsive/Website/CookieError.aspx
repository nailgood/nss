<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CookieError.aspx.vb" Inherits="CookieError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Nail Superstore </title>
    <meta name="robots" content="noindex,follow">
    <link href="App_Themes/Default/default.css" rel="stylesheet" type="text/css" />
  
</head>
<body bgcolor="#ffffff">
    <form id="Form1" runat="server">
    <div id="cookiemsg">
        <div class="title">
            Please Enable Cookies to Continue.
        </div>
        <div class="desc">
            To continue shopping at <a href="/">nailsuperstore.com</a>, please enable cookies
            in your Web browser follow the instructions below for the browser version you're
            using:
        </div>
        <div class="brow">
            <h3>
                Internet Explorer 7.0+</h3>
            <div class="step">
                1. Click the <span class="mask">Tools</span> icon in the browser toolbar<br>
                2. Choose <span class="mask">Internet Options</span><br>
                3. Click the&nbsp;<span class="mask">Privacy</span> tab<br>
                4. Click <span class="mask">Custom level</span> in the Security Level zone<br>
                5. Slide the settings button down to <span class="mask">Accepts All Cookies</span>
                or <span class="mask">Low</span><br>
                6. Click <span class="mask">Apply</span>, and then <span class="mask">OK</span><br>
            </div>
            <div class="more">
                For more information on Internet Explorer, please <a href="http://windows.microsoft.com/en-US/windows7/Block-enable-or-allow-cookies"
                    target="_new">see Microsoft's Help Center on enabling cookies</a>.
            </div>
        </div>
        <div class="brow">
            <h3>
                Safari 2 or 3</h3>
            <div class="step">
                1. Click the <span class="mask">Safari</span> menu from the top toolbar<br>
                2. Choose <span class="mask">Preferences</span><br>
                3. Click the&nbsp;<span class="mask">Privacy</span> tab<br>
                4. Click the <span class="mask">Never</span> checkbox for Block Cookies
            </div>
            <div class="more">
                For more information on Safari, please <a href="http://support.apple.com/kb/HT1677"
                    target="_new">see Apple's Help Center</a>.
            </div>
        </div>
        <div class="brow">
            <h3>
                Google Chrome</h3>
            <div class="step">
                1. Click the <span class="mask">Wrench</span> icon in the browser toolbar<br>
                2. Choose <span class="mask">Settings</span><br>
                3. Click the <span class="mask">Under the Hood</span> tab<br>
                4. Click <span class="mask">Content settings</span> in the Privacy section<br>
                5. Ensure that the bullet for <span class="mask">Allow local data to be set (recommended)</span>&nbsp;<br>
                &nbsp;&nbsp;&nbsp; is checked<br>
                6. Also ensure that "Block third-party cookies and site data" is <span class="mask">
                    unchecked</span><br>
            </div>
            <div class="more">
                For more information on Google Chrome, please <a href="http://support.google.com/chrome/bin/answer.py?hl=en&amp;answer=114662"
                    target="_new">see Google's Help Center</a>.
            </div>
        </div>
        <div class="brow">
            <h3>
                Firefox 3.6+</h3>
            <div class="step">
                1. Click the <span class="mask">Tools</span> menu from the top toolbar<br>
                2. Choose&nbsp;<span class="mask">Options</span><br>
                3. Click the&nbsp;<span class="mask">Privacy</span> tab<br>
                4. Under "History" click <span class="mask">Firefox will use custom settings for history</span>
                <span style="font-weight: normal;">from the<br>
                    &nbsp;&nbsp;&nbsp; drop-down menu</span><br>
                5. Ensure that the checkboxes for <span class="mask">Accept cookies from sites</span>
                 <span style="font-weight: normal;">is 
                         checked<br>
                        6. Click</span> <span class="mask">OK</span><span class="mask"><br>
                        </span>
            </div>
            <div class="more">
                For more information on Mozilla, please see Firefox Help for <a href="http://support.mozilla.org/en-US/kb/Enabling%20and%20disabling%20cookies"
                    target="_new">more instructions on enabling cookies</a>.
            </div>
        </div>
    </div>
    </form>
</body>
</html>
