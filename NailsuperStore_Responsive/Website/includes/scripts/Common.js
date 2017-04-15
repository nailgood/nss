// JScript File
var maskPhoneUSRegular = '?999-999-9999 9999';
var maskFaxUSRegular = '?999-999-9999 9999';
function ClientCheckPhoneUS(sender, args) {

    var isValid = CheckPhoneUSValid(args.Value);
    args.IsValid = isValid;

}
function ClientCheckPhoneInternational(sender, args) {
    // alert(sender.errormessage);
    //step 1 : check valid
    var result = false;
    var valueCheck = args.Value;  
    var intPhoneExt = /^([0-9\(\)\/\+ \-]*)$/;
    if (valueCheck.match(intPhoneExt)) {
        var number = valueCheck.replace(/[^0-9]/g, '');
        if (is_int(number)) {
            if (number.length >= 10)
                result = true;
            else {
                sender.errormessage = 'Phone number must be at least 10 digit.'
                result = false;
            }
        }
        else {
            sender.errormessage = 'Phone number is invalid'
            result = false;
        }
    }
    else {
        sender.errormessage = 'Phone number is invalid'
        result = false;
    }
    args.IsValid = result;
    if (result == false) {
        $('#' + sender.id).html(sender.errormessage);
    }


}
function MergeErrorMessage(totalMsg, errorMsg) {
    errorMsg = "<div class='error-row'>" + errorMsg + "</div>"
    if (totalMsg == '') {
        totalMsg = errorMsg ;
    }
    else {
        totalMsg = totalMsg + errorMsg ;
    }
    return totalMsg;
}
function ClientCheckFaxInternational(sender, args) {
   
    var result = false;
    var valueCheck = args.Value;
    var intPhoneExt = /^([0-9\(\)\/\+ \-]*)$/;
    if (valueCheck.match(intPhoneExt)) {
        args.IsValid = true;        
    }
    else {
        args.IsValid = false;
    }
}
function ResetHeaderCartItemcount(countitem) {
    if (document.getElementById('cart-count')) {
         $("#cart-count").html(countitem);
         $(".xs-cart-count").text(countitem);
    }   
}
function CheckMinLengthNumberValid(number, length) {

    if (number != '') {
        number = number.replace(/[^0-9]/g, '');
        if (is_int(number)) {
            if (number.length >= parseInt(length))
                return true;
        }

    }
    return false;
}
function CheckUSPhoneExt(value) {

    if (value == '') {
        return true;
    }
    var phone = value;
    if (phone.length > 4) {
        return false;
    }
    if (!is_int(phone)) {
        return false;
    }
    return true;

}
function is_int(value) {
    for (i = 0; i < value.length; i++) {
        if ((value.charAt(i) < '0') || (value.charAt(i) > '9')) return false
    }
    return true;
}
function CheckPhoneUSValid(phone) {
    if (phone == '') {
        return false;
    }
    var phone1;
    var phone2;
    var phone3;
    var arr;
    try {
        if (phone != '') {
            arr = phone.split(' ');
            if (arr[0] != '') {
                var lstPhone = arr[0].split('-');

                phone1 = lstPhone[0];
                phone2 = lstPhone[1];
                phone3 = lstPhone[2];
            }
        }

        if (phone1.length != 3 || phone2.length != 3 || phone3.length != 4) {
            return false;
        }
        if (!is_int(phone1) || !is_int(phone2) || !is_int(phone3)) {
            return false;
        }
        else {
            var ext = '';
            if (parseInt(arr.length) > 1) {
                ext = arr[1];
            }
            return CheckUSPhoneExt(ext);
        }
    }
    catch (e) {
        return false;
    }
}
function numbersonly(myfield, e, dec) {
    var key;
    var keychar;
    if (window.event)
        key = window.event.keyCode;
    else if (e)
        key = e.which;
    else
        return true;
    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
        return true;

    // numbers
    else if ((("0123456789").indexOf(keychar) > -1))
        return true;

    // decimal point jump
    else if (dec && (keychar == ".")) {
        myfield.form.elements[dec].focus();
        return false;
    } else
        return false;
}

function isEmail(emailStr) {
    var emailPat = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
    var matchArray = emailStr.match(emailPat);

    if (matchArray == null) {
        return (false);
    }

    return (true);
}

function IsNumber(e) {

    if (!((e.keyCode >= 48 && e.keyCode <= 57))) {
        e.returnValue = false;
    }
}
function IsPhone(e) {

    if (!((e.keyCode >= 48 && e.keyCode <= 57))) {
        if (e.keyCode == 45) {
            e.returnValue = true;
        }
        else {
            e.returnValue = false;
        }

    }

}

function GoDrp(drp) {
    if (drp.value.length > 0) {
        window.location.href = drp.value;
    }
}

function GoUrl(url, drp) {
    if (url.length > 0) {
        window.location.href = url.replace('{0}', drp.value);
    }
}

function isNum(s) {
    var i, isnum = true, len = s.length
    for (i = 0; i < len; i++) {
        if (!isDigit(s.charAt(i)))
            isnum = false;
    }
    return isnum;
}

function isNumberLenZero(s) {
    var i, isnum = true, len = s.length
    for (i = 0; i < len; i++) {
        if (!isDigit(s.charAt(i)))
            isnum = false;
    }
    return isnum;
}
function isDigit(c) {
    if ((c == '0') || (c == '1') || (c == '2') || (c == '3') || (c == '4') || (c == '5') || (c == '6') || (c == '7') || (c == '8') || (c == '9'))
        return true;
    else
        return false;
}
function IsCookieEnable() {
    /* check for a cookie */

    var cookieEnabled = (navigator.cookieEnabled) ? true : false;

    if (typeof navigator.cookieEnabled == "undefined" && !cookieEnabled) {
        document.cookie = "testcookie";
        cookieEnabled = (document.cookie.indexOf("testcookie") != -1) ? true : false;
    }

    if (!cookieEnabled) {
        window.location.href = '/CookieError.aspx';
        return false;
    }
    return true;

}
function checkKeycode(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;

    alert("keycode: [" + keycode + "]");
}

function IncreaseMulti(strName) {
    var $textboxes = $('input[name="' + strName + '"]');
    if ($textboxes.length >= 1) {
        for (var i = 0; i < $textboxes.length; i++) {
            var value = parseInt($textboxes.eq(i).val(), 10);
            if (value < 9999) $textboxes.eq(i).val(value + 1);
        }
    }
}

function Increase(strName) {
    var ctrl = document.getElementById(strName);
    var value = parseInt(ctrl.value, 10);
    if (value < 9999) ctrl.value = value + 1;

    //  RefreshArrows(strName);
}

function DecreaseMulti(strName, val) {
    var $textboxes = $('input[name="' + strName + '"]');
    if ($textboxes.length >= 1) {
        for (var i = 0; i < $textboxes.length; i++) {
            var value = parseInt($textboxes.eq(i).val(), 10);
            if (value > val) $textboxes.eq(i).val(value - 1);
        }
    }
}

function Decrease(strName, val) {
    var ctrl = document.getElementById(strName);
    var value = parseInt(ctrl.value, 10);
    if (value > val) ctrl.value = value - 1;
}

function Decrease2(strName) {
    var ctrl = document.getElementById(strName);
    var value = parseInt(ctrl.value, 10);
    if (value > 1) ctrl.value = value - 1;

    RefreshArrows2(strName);
}

function Increase3(strName) {
    var ctrl = document.getElementById(strName);
    var value = parseInt(ctrl.value, 10);
    if (value < 9999) ctrl.value = value + 1;
    RefreshArrows3(strName);
}

function Decrease3(strName) {
    var ctrl = document.getElementById(strName);
    var value = parseInt(ctrl.value, 10);
    if (value > 0) ctrl.value = value - 1;
    RefreshArrows3(strName);
}

function RefreshArrows(strName) {
    var ctrl = document.getElementById(strName);

    strName = strName.replace('_txtQty', '');
    var imgIncrease = document.getElementById(strName + '_imgIntxtQty');
    var imgDecrease = document.getElementById(strName + '_imgDetxtQty');
    var value = parseInt(ctrl.value, 10);

    if (value > 0) {
        imgDecrease.src = '/includes/theme/images/btn-arrow-down.gif';
    } else {
        imgDecrease.src = '/includes/theme/images/btn-arrow-down-inact.gif';
    }

    if (value < 9999) {
        imgIncrease.src = '/includes/theme/images/btn-arrow-up.gif';
    } else {
        imgIncrease.src = '/includes/theme/images/btn-arrow-up-inact.gif';
    }
}

function RefreshArrows2(strName) {
    var ctrl = document.getElementById(strName);

    strName = strName.replace('_txtQty', '');
    var imgIncrease = document.getElementById(strName + '_imgIntxtQty');
    alert(imgIncrease);
    var imgDecrease = document.getElementById(strName + '_imgDetxtQty');
    alert(imgDecrease);
    var value = parseInt(ctrl.value, 10);

    if (value > 1) {
        imgDecrease.src = '/includes/theme/images/btn-arrow-down.gif';
    } else {
        imgDecrease.src = '/includes/theme/images/btn-arrow-down-inact.gif';
    }

    if (value < 9999) {
        imgIncrease.src = '/includes/theme/images/btn-arrow-up.gif';
    } else {
        imgIncrease.src = '/includes/theme/images/btn-arrow-up-inact.gif';
    }
}


function RefreshArrows3(strName) {
    var ctrl = document.getElementById(strName);

    var imgIncrease = document.getElementById(strName + '_imgIntxtQty');
    var imgDecrease = document.getElementById(strName + '_imgDetxtQty');
    var value = parseInt(ctrl.value, 10);

    if (value > 0) {
        imgDecrease.src = '/includes/theme/images/btn-arrow-down.gif';
    } else {
        imgDecrease.src = '/includes/theme/images/btn-arrow-down-inact.gif';
    }

    if (value < 9999) {
        imgIncrease.src = '/includes/theme/images/btn-arrow-up.gif';
    } else {
        imgIncrease.src = '/includes/theme/images/btn-arrow-up-inact.gif';
    }
}
//if (window.addEventListener) {
//    window.addEventListener('load', RefreshImages, false);
//} else if (window.attachEvent) {
//    window.attachEvent('onload', RefreshImages);
//}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}
function countWords(text) {

    if (text == '')
        return 0;
    text = text.replace(/(\r\n|\n|\r)/gm, " ");
    //a = y.replace(/\s/g, ' ');
    var fullStr = text + " ";
    var splitString = fullStr.split(" ");

    var word_count = 0;
    for (var w = 0; w < splitString.length; w++) {
        if (splitString[w].toString() != '')
            word_count = word_count + 1;
    }
    return word_count;
}
function FormatString(str) {
    if (str == '')
        return str;
    var args = str.split(',');
    for (var i = 0; i < args.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "");
        args[0] = args[0].replace(reg, args[i + 1]);
    }
    return args[0];
}
function NewWindow(sURL, sWindowName, iWidth, iHeight, sScrollYesNo, sResizableYesNo) {
    var fLeftPosition, fTopPosition;
    var sSettings;

    fLeftPosition = (screen.width) ? (screen.width - iWidth) / 2 : 0;
    fTopPosition = (screen.height) ? (screen.height - iHeight) / 2 : 0;

    sSettings = 'height=' + iHeight + ',width=' + iWidth + ',top=' + fTopPosition + ',left=' + fLeftPosition + ',scrollbars=' + sScrollYesNo + ',resizable=' + sResizableYesNo;

    window.open(sURL, sWindowName, sSettings);
}
function isInteger(argument) { return argument == ~ ~argument; }

function GetQty(id) {
    try {
        var qty = 0;
        var $txt = $('input[name="txtQtyItem' + id + '"]');
        if ($txt.length > 0) {
            for (var y = 0; y < $txt.length; y++) {
                if (qty < $txt.eq(y).val()) qty = $txt.eq(y).val();
            }
        }
        return qty;
    }
    catch (err) {
        return 0;
    }
}

function GetQtyPoint(id) {
    try {
        var qty = 0;
        var $txt = $('input[name="txtQtyItemPoint' + id + '"]');
        if ($txt.length > 0) {
            for (var y = 0; y < $txt.length; y++) {
                if (qty < $txt.eq(y).val()) qty = $txt.eq(y).val();
            }
        }
        return qty;
    }
    catch (err) {
        return 0;
    }
}