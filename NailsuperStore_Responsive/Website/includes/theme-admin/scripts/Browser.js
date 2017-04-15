function GetBrowser() {
    var browserName = "";
    var ua = navigator.userAgent.toLowerCase();
    if (ua.indexOf("opera") != -1) {
        browserName = "opera";
    }
    else if (ua.indexOf("msie") != -1) {
        browserName = "ie";
    }
    else if (ua.indexOf("safari") != -1) {
        browserName = "safari";
    }
    else if (ua.indexOf("mozilla") != -1) {
        if (ua.indexOf("firefox") != -1) {
            browserName = "ff";
        }
        else {
            browserName = "ie"; //not sure
        }
    }
    return browserName;
}
function ClosePopup() {
    // SetParentData('0');
    window.close();
}

function ReloadParent() {
    SetParentData(1, 0);
    window.close();

}
function SetParentData(type, value) {
    var brow = GetBrowser();
    if (brow == 'ie') {
        window.returnValue = value;
    }
    else {
        window.opener.SetValue(type, value)
    }


}
function CheckTarget() {
    var frm = document.getElementById('form1')

    if (frm) {
        var brow = GetBrowser();
        if (brow == 'ie') {
            frm.target = "modal"
        }
        else frm.target = ""

    }
}
function EditReview(url) {
    SetParentData(0, url);
    window.close();
}