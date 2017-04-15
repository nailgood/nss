
var queryField;
var lookupURL;
var divName;
var ifName;
var lastVal = "";
var val = "";
var xmlHttp;
var cache = new Object();
var searching = false;
var globalDiv;
var divFormatted = false;
var DIV_BG_COLOR = ""; //#ffffff
var DIV_HIGHLIGHT_COLOR = "#818181"; //#3366cc #eeeeee  #f2eef1 #477DB7 E1E1E1
var FONT_COLOR = "#707070"; //#454545  #707070
var FONT_HOVER_COLOR = "#FFFFFF"; //#ffffff
var divWidth = "310px";
var chkShow = "";
var vFName = "";

/**
The InitQueryCode function should be called by the <body onload> event, passing
at least the queryFieldName and lookupURLPrefix parameters, where:

queryFieldName = the name of the form field we're using for lookups
lookupURLPrefix = the URL we'll use to pass the query string back to the server,
which will be immediately proceeded by the query string

For example:
<body onload="InitQueryCode('lookupField', 'http://lookupserver/QueryHandler?q=')">

The above example will monitor the input box called "lookupField" on this page,
and when it changes the contents of the field will be passed to lookupserver like:
http://lookupserver/QueryHandler?q=fieldValue

The http://lookupserver/QueryHandler will be expected to return a text response
with a single line of text that calls the showQueryDiv function, in a format like:
showQueryDiv("smi", new Array("John Smith", "Mary Smith"), new Array("555-1212", "555-1234"));

*/
function InitQueryCode(queryFieldName, lookupURLPrefix, hiddenDivName) {
    vFName = queryFieldName;
    queryField = document.getElementsByName(queryFieldName).item(0);
    queryField.onblur = hideDiv;
    queryField.onkeydown = keypressHandler;

    // for some reason, Firefox 1.0 doesn't allow us to set autocomplete to off
    // this way, so you should manually set autocomplete="off" in the input tag
    // if you can -- we'll try to set it here in case you forget
    queryField.autocomplete = "off";

    lookupURL = lookupURLPrefix;
    if (hiddenDivName)
        divName = hiddenDivName;
    else
        divName = "querydiv";
    ifName = "queryiframe";

    // add a blank value to the cache (so we don't try to do a lookup when the
    // field is empty) and start checking for changes to the input field
    addToCache("", new Array(), new Array());
    setTimeout("mainLoop()", 100);
}
function InitQueryCode2(queryFieldName, lookupURLPrefix, hiddenDivName, show) {
    chkShow = show;
    InitQueryCode(queryFieldName, lookupURLPrefix, hiddenDivName);
}

/**
This is a helper function that just adds results to our cache, to avoid
repeat lookups.
*/
function addToCache(queryString, resultArray1, resultArray2) {
    cache[queryString] = new Array(resultArray1, resultArray2);
}


/**
This is the function that monitors the queryField, and calls the lookup
functions when the queryField value changes.
*/
mainLoop = function () {

    var hidReset = '';
    if (document.getElementById('hidReset')) {
        var hidReset = document.getElementById('hidReset').value;
        if (hidReset == '1') {
            //document.getElementById('hidReset').value = '0';
            setTimeout("mainLoop()", 100);
            return;
        }
    }
    val = queryField.value;

    // if the field value has changed and we're not currently waiting for
    // a lookup result to be returned, do a lookup (or use the cache, if
    // we can)
    //alert('lastVal:' + lastVal + '-val:' + val);
   
    if (lastVal != val && searching == false) {
       
        var cacheResult = cache[val];
        if (cacheResult) {
            showQueryDiv(val, cacheResult[0], cacheResult[1]);
        }
        else {
            doRemoteQuery(val);
        }
        lastVal = val;
    }

    setTimeout("mainLoop()", 100);
    return true;
}
;


/**
Get the <DIV> we're using to display the lookup results, and create the
<DIV> if it doesn't already exist.
*/
function getDiv(divID) {
    var temp = document.getElementById(divID);
    if (globalDiv != temp) {
        globalDiv = "";
        divFormatted = false;
    }
    if (!globalDiv) {
        // if the div doesn't exist on the page already, create it
        if (!document.getElementById(divID)) {
            var newNode = document.createElement("div");
            newNode.setAttribute("class", "varHidden");
            newNode.setAttribute("id", divID);
            document.body.appendChild(newNode);
        }

        // set the globalDiv reference
        globalDiv = document.getElementById(divID);

        // figure out where the top corner of the div should be, based on the
        // bottom left corner of the input field
        var x = queryField.offsetLeft;
        var y = queryField.offsetTop + queryField.offsetHeight;
        var parent = queryField;
        while (parent.offsetParent) {
            parent = parent.offsetParent;
            x += parent.offsetLeft;
            y += parent.offsetTop;
        }

        // add some formatting to the div, if we haven't already
        if (!divFormatted) {
            globalDiv.style.backgroundColor = DIV_BG_COLOR;
            //      globalDiv.style.fontFamily = "Arial, Helvetica, sans-serif";
            //globalDiv.style.padding = "4px";
            //      globalDiv.style.border = "1px solid #666666";
            //      globalDiv.style.fontSize = "13px";
            //      globalDiv.style.width = divWidth;
            //      globalDiv.style.position = "absolute";
            globalDiv.style.left = x + "px";
            globalDiv.style.top = y + "px";
            //      globalDiv.style.visibility = "hidden";
            //      globalDiv.style.zIndex = 10000;
            //      globalDiv.style.height = "550px";
            //      globalDiv.style.overflow = "auto";
            divFormatted = true;
        }
    }

    return globalDiv;
}


/**
This is the function that should be returned by the XMLHTTP call. It will
format and display the lookup results.
*/
function showQueryDiv(queryString, resultArray1, resultArray2) {
    var div = getDiv(divName);

    // remove any results that are already there
    while (div.childNodes.length > 0)
        div.removeChild(div.childNodes[0]);

    // add an entry for each of the results in the resultArray
    for (var i = 0; i < resultArray1.length; i++) {
        // each result will be contained within its own div
        var result = document.createElement("div");
        result.id = "dSearch";
        result.style.cursor = "pointer";
        //result.style.borderBottom = "1px solid #000000";
        result.style.padding = "0px 3px 0px 3px";
        _unhighlightResult(result);
        result.onmousedown = selectResult;
        result.onmouseover = highlightResult;
        result.onmouseout = unhighlightResult;

        var result1 = document.createElement("span");
        result1.Id = resultArray2[i];
        result1.className = "result1";

        result1.style.textAlign = "left";
        result1.style.fontWeight = "normal";
        result1.innerHTML = resultArray1[i];

        //var result2 = document.createElement("span");
        //result2.className = "result2";
        //result2.style.textAlign = "right";
        //result2.style.paddingLeft = "20px";
        //result2.innerHTML = resultArray2[i];

        result.appendChild(result1);

        //result.appendChild(result2);
        div.appendChild(result);
    }

    // if this resultset isn't already in our cache, add it
    var isCached = cache[queryString];
    if (!isCached)
        addToCache(queryString, resultArray1, resultArray2);

    // display the div if we had at least one result

    showDiv(resultArray1.length > 0);
}

function showQueryDivSearch(queryString, resultArray1, resultArray2, resultArray3, resultArray4, resultArray5, resultArray6, resultArray7, resultArray8) {
    var div = getDiv(divName);
    // remove any results that are already there
    while (div.childNodes.length > 0)
        div.removeChild(div.childNodes[0]);

    if (resultArray1.length > 0) {
        var link = document.createElement("div");
        link.className = "linkSearchAll";
        link.innerHTML = "See All Result Searches for \"" + queryField.value + "\"";
        //link.setAttribute("onmousedown", "MyCallbackKeyword();");
        link.onmousedown = selectResultAll;
        link.onmouseover = highlight;
        link.onmouseout = unhighlight;
        div.appendChild(link);
    }
    //add keyword to div search
    if (resultArray7.length > 0) {
        var title = document.createElement("div");
        title.className = "SearchTitle";
        title.innerHTML = "POPULAR KEYWORD";
        div.appendChild(title);
        for (var i = 0; i < resultArray6.length; i++) {
            var result = document.createElement("div");
            result.id = "dSearch";
            result.className = "dvSearch";
            _unhighlightResult(result);
            result.onmousedown = selectResultKeyword;
            result.onmouseover = highlightResult;
            result.onmouseout = unhighlightResult;
            var divContent = document.createElement("div");
            var divtitle = document.createElement("div");
            divtitle.className = "title";
            var result2 = document.createElement("span");
            result2.Id = resultArray6[i];
            result2.className = "result2";
            result2.innerHTML = resultArray7[i];
            result.appendChild(result2);

            //result.appendChild(result2);
            div.appendChild(result);

        }

    }

    if (resultArray1.length > 0) {


        var title = document.createElement("div");
        title.className = "SearchTitle";
        title.innerHTML = "PRODUCT MATCHES";
        div.appendChild(title);
    }

    // add an entry for each of the results in the resultArray
    for (var i = 0; i < resultArray1.length; i++) {
        // each result will be contained within its own div
        var result = document.createElement("div");
        result.id = "dSearch";

        if (i % 2 == 0) {
            result.className = "dvSearch alt";
        }
        else {
            result.className = "dvSearch";
        }
        ////        result.style.width = "355px";
        ////        result.style.cssFloat = "left";
        ////        result.style.clear = "both";
        ////        result.style.cursor = "pointer";
        //result.style.borderBottom = "1px solid #000000";
        ////        result.style.padding = "10px 5px 5px 5px";
        _unhighlightResult(result);
        result.onmousedown = selectResult;
        result.onmouseover = highlightResult;
        result.onmouseout = unhighlightResult;
        var divContent = document.createElement("div");
        var divtitle = document.createElement("div");
        divtitle.className = "title";
        var result1 = document.createElement("span");
        result1.Id = resultArray2[i];
        result1.className = "result1";
        ////        result1.style.textAlign = "left";
        ////        result1.style.font = "bold 12px arial,serif";
        result1.innerHTML = resultArray1[i];
        var img = document.createElement("div");
        img.style.cssFloat = "left";
        img.className = "img";
        ////        img.style.width = "65px";
        img.innerHTML = "<img src='/assets/items/small/" + resultArray3[i] + "'><br/><span>" + resultArray5[i] + "</span>";
        ////        img.style.position = "static";
        var divDesc = document.createElement("div");
        divDesc.className = "desc";
        divDesc.innerHTML = resultArray4[i];
        //var result2 = document.createElement("span");
        //result2.className = "result2";
        //result2.style.textAlign = "right";
        //result2.style.paddingLeft = "20px";
        //result2.innerHTML = resultArray2[i];
        //div1.appendChild(img);


        // alert(resultArray8[i]);
        divtitle.appendChild(result1);
        divContent.appendChild(divtitle);
        divContent.appendChild(divDesc);

        if (resultArray8[i].length > 0) {
            var price = document.createElement("div");
            //            price.style.cssFloat = "right";
            price.className = "price";
            price.innerHTML = resultArray8[i];
            divContent.appendChild(price);
        }


        result.appendChild(img);
        result.appendChild(divContent);

        //result.appendChild(result2);
        div.appendChild(result);

    }


    // if this resultset isn't already in our cache, add it
    var isCached = cache[queryString];
    if (!isCached)
        addToCache(queryString, resultArray1, resultArray2);

    // display the div if we had at least one result

    showDiv(resultArray1.length > 0 || resultArray7.length > 0);
}

function showQueryDivKeyword(queryString, resultArray1, resultArray2) {
    var div = getDiv(divName);

    // remove any results that are already there
    while (div.childNodes.length > 0)
        div.removeChild(div.childNodes[0]);

    // add an entry for each of the results in the resultArray
    for (var i = 0; i < resultArray1.length; i++) {
        // each result will be contained within its own div
        var result = document.createElement("div");
        result.id = "dSearch";
        result.style.cursor = "pointer";
        //result.style.borderBottom = "1px solid #000000";
        result.style.padding = "0px 3px 0px 3px";
        _unhighlightResult(result);
        result.onmousedown = selectResultOther;
        result.onmouseover = highlightResult;
        result.onmouseout = unhighlightResult;

        var result1 = document.createElement("span");
        result1.Id = resultArray2[i];
        result1.className = "result1";

        result1.style.textAlign = "left";
        result1.style.fontWeight = "normal";
        result1.innerHTML = resultArray1[i];

        //var result2 = document.createElement("span");
        //result2.className = "result2";
        //result2.style.textAlign = "right";
        //result2.style.paddingLeft = "20px";
        //result2.innerHTML = resultArray2[i];

        result.appendChild(result1);

        //result.appendChild(result2);
        div.appendChild(result);
    }

    // if this resultset isn't already in our cache, add it
    var isCached = cache[queryString];
    if (!isCached)
        addToCache(queryString, resultArray1);

    // display the div if we had at least one result

    showDiv(resultArray1.length > 0);
}

/**
This is called whenever the user clicks one of the lookup results.
It puts the value of the result in the queryField and hides the
lookup div.
*/
function selectResult() {
    _selectResult(this);
}
function selectResultKeyword() {
    _selectResultKeyword(this);
}
function selectResultAll() {
    _selectResultAll(this);
}
function selectResultOther() {
    _selectResultOther(this);
}

function _setHidden(item) {
    if (divName == "varHidden") {
        var spans = item.getElementsByTagName("span");
        var hidden = document.getElementById("LookupHidden");
        var hiddenkey = document.getElementById("LookupKeyword");
        if (spans) {
            for (var i = 0; i < spans.length; i++) {
                if (spans[i].className == "result1") {
                    hiddenkey.value = "";
                    hidden.value = spans[i].Id;
                }
                if (spans[i].className == "result2") {
                    hidden.value = "";
                    //hiddenkey.value = spans[i].Id
                }
            }
        }
        return;
    }
}
function _setHiddenKeyword(item) {
    if (divName == "varHidden") {
        var spans = item.getElementsByTagName("span");
        var hidden = document.getElementById("LookupKeyword");
        if (spans) {
            for (var i = 0; i < spans.length; i++) {
                if (spans[i].className == "result2") {
                    hidden.value = spans[i].Id;

                }
            }
        }
    }
}

/** This actually fills the field with the selected result and hides the div */
function _selectResult(item) {
    var spans = item.getElementsByTagName("span");
    if (spans) {
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].className == "result1") {
                var str = spans[i].innerHTML;
                var re = new RegExp('&amp;', 'gi');
                str = str.replace(re, '&');

                if (divName != "varHidden") {
                    showDiv(false);
                    queryField.value = str;
                }
                else {
                    document.getElementById("LookupHidden").value = spans[i].Id;
                }
                //     
                MyCallback(spans[i].Id);
                lastVal = val = escape(str);
                searching = false;
                mainLoop();
                queryField.focus();
                return;
            }
            else if (spans[i].className == "result2") {
                _selectResultKeyword(item);
            }
        }
    }
}
function _selectResultKeyword(item) {
    var spans = item.getElementsByTagName("span");
    if (spans) {
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].className == "result2") {

                var str = spans[i].Id;
                //              var re = new RegExp('&amp;', 'gi');
                //                 str = str.replace(re, '&');

                //set keyword search lucene
                //_setHiddenKeyword(item)
                document.getElementById("LookupKeyword").value = str;
                queryField.value = str;
                MyCallbackKeyword();

                lastVal = val = escape(str);
                searching = false;
                mainLoop();
                queryField.focus();
                if (divName != "varHidden") {
                    showDiv(false);
                }
                return;
            }
        }
    }
}
function _selectResultAll(item) {

    MyCallbackKeyword();
    searching = false;
    mainLoop();
    queryField.focus();

    return;

}

function _selectResultOther(item) {
    var spans = item.getElementsByTagName("span");
    if (spans) {
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].className == "result1") {

                var str = spans[i].Id;
                var re = new RegExp('&amp;', 'gi');
                str = str.replace(re, '&');

                queryField.value = str;
                MyCallback();


                return;
            }
        }
    }
}
/**
This is called when a user mouses over a lookup result
*/
function highlightResult() {
    _highlightResult(this);
}
function highlight() {
    _highlight(this);
}

/** This actually highlights the selected result */
function _highlightResult(item) {
    item.style.backgroundColor = DIV_HIGHLIGHT_COLOR;
    item.style.color = FONT_HOVER_COLOR;
    if (item.className.indexOf('active') < 0) {
        item.className = item.className + ' active';
    }

}
function _highlight(item) {
    item.style.backgroundColor = DIV_HIGHLIGHT_COLOR;
}


/**
This is called when a user mouses away from a lookup result
*/
function unhighlightResult() {
    _unhighlightResult(this);
}
function unhighlight() {
    _unhighlight(this);
}

/** This actually unhighlights the selected result */
function _unhighlightResult(item) {
    item.style.backgroundColor = DIV_BG_COLOR;
    item.style.color = FONT_COLOR;
    item.className = item.className.replace(' active', '');

}
function _unhighlight(item) {
    item.style.backgroundColor = "#f2f2f2";
}


/**
This either shows or hides the lookup div, depending on the value of
the "show" parameter.
*/
function showDiv(show) {
    var div = getDiv(divName);
    if (show && chkShow == '')
    { div.style.visibility = "visible"; }
    else {
        div.style.visibility = "hidden";
        //        if (divName == "varHidden") {
        //            document.getElementById("LookupHidden").value = "";
        //        }
        if (chkShow != '') {
            chkShow = '';
        }
    }

    //adjustiFrame();
}


/**
We originally used showDiv as the function that was called by the onBlur
event of the field, but it turns out that Firefox will pass an event as the first
parameter of the function, which would cause the div to always be visible.
So onBlur now calls hideDiv instead.
*/
function hideDiv() {
    showDiv(false);
}


/**
Use an "iFrame shim" to deal with problems where the lookup div shows up behind
selection list elements, if they're below the queryField. The problem and solution are
described at:

http://dotnetjunkies.com/WebLog/jking/archive/2003/07/21/488.aspx
http://dotnetjunkies.com/WebLog/jking/archive/2003/10/30/2975.aspx
*/
function adjustiFrame() {
    if (!document.getElementById(ifName)) {
        var newNode = document.createElement("iFrame");
        newNode.setAttribute("id", ifName);
        newNode.setAttribute("src", "javascript:false;");
        newNode.setAttribute("scrolling", "no");
        newNode.setAttribute("frameborder", "0");
        document.body.appendChild(newNode);
    }

    iFrameDiv = document.getElementById(ifName);
    var div = getDiv(divName);

    try {
        iFrameDiv.style.position = "absolute";
        iFrameDiv.style.width = div.offsetWidth;
        iFrameDiv.style.height = div.offsetHeight;
        iFrameDiv.style.top = div.style.top;
        iFrameDiv.style.left = div.style.left;
        iFrameDiv.style.zIndex = div.style.zIndex - 1;
        iFrameDiv.style.visibility = div.style.visibility;
    } catch (e) {
    }
}


/**
This sets up the XMLHTTP object we're using for the dynamic lookups.
*/
function getXMLHTTP() {
    var A = null;

    try {
        A = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            A = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (oc) {
            A = null;
        }
    }

    if (!A && typeof XMLHttpRequest != "undefined") {
        A = new XMLHttpRequest();
    }

    return A;
}


/**
This actually sends the lookup request (as a URL with a query string) to a
server in the background. When a response comes back from the server, the
function attached to the onReadyStateChange event is fired off.
*/
function doRemoteQuery(queryString) {
    searching = true;
    if (xmlHttp && xmlHttp.readyState != 0) {
        xmlHttp.abort();
    }
    xmlHttp = getXMLHTTP();
    if (xmlHttp) {
        xmlHttp.open("GET", lookupURL + queryString, true);

        // What do we do when the response comes back?
        xmlHttp.onreadystatechange = function() {
            if (xmlHttp.readyState == 4 && xmlHttp.responseText && searching) {
                eval(xmlHttp.responseText);
                searching = false;
            }
        };
        xmlHttp.send(null);
    }

}
function GetSynonymKeywordData(keyword) {
    searching = true;
    
    if (xmlHttp && xmlHttp.readyState != 0) {
        xmlHttp.abort();
    }
    xmlHttp = getXMLHTTP();
    if (xmlHttp) {
        xmlHttp.open("GET", '/admin/ajax.aspx?f=GetSynonymKeywordData&keyword=' + keyword, true);

        // What do we do when the response comes back?
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState == 4 && xmlHttp.responseText && searching) {
                eval(xmlHttp.responseText);
                lastVal = '';
                val = '';
                searching = false;
            }
        };
        xmlHttp.send(null);
    }

}


/**
This is the key handler function, for when a user presses the up arrow,
down arrow, tab key, or enter key from the input field.
*/
function keypressHandler(evt) {
  
    if (document.getElementById('hidReset')) {
        document.getElementById('hidReset').value = '0';
    }

    // don't do anything if the div is hidden
    var div = getDiv(divName);
    if (div.style.visibility == "hidden" && divName != "varKeyword") {
        return;
    }

    // make sure we have a valid event variable
    if (!evt && window.event) {
        evt = window.event;
    }
    var key = evt.keyCode;

    // if this key isn't one of the ones we care about, just return
    var KEYUP = 38;
    var KEYDOWN = 40;
    var KEYENTER = 13;
    var KEYTAB = 9;

    if ((key != KEYUP) && (key != KEYDOWN) && (key != KEYENTER) && (key != KEYTAB))
        return true;

    // get the span that's currently selected, and perform an appropriate action
    var selNum = getSelectedSpanNum(div);
    var selSpan = setSelectedSpan(div, selNum);
    if ((key == KEYENTER) || (key == KEYTAB)) {
        if (selSpan) {
            if (divName == "varKeyword") {
                _selectResultOther(selSpan);
            }
            else {
                _selectResult(selSpan);
            }
        }
      
        hideDiv();
        evt.cancelBubble = true;
        return false;
    } else {
        if (key == KEYUP)
            selSpan = setSelectedSpan(div, selNum - 1);
        if (key == KEYDOWN)
            selSpan = setSelectedSpan(div, selNum + 1);
        if (selSpan) {
            _highlightResult(selSpan);
        }
    }

    showDiv(true);
    return true;
}


/**
Get the number of the result that's currently selected/highlighted
(the first result is 0, the second is 1, etc.)
*/
function getSelectedSpanNum(div) {
    var count = -1;
    var spans = div.getElementsByTagName("div");
    if (spans) {
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].id == "dSearch") {
                count++;
                if (spans[i].style.backgroundColor != div.style.backgroundColor) {
                    return count;
                }
            }
        }
    }

    return -1;
}


/**
Select/highlight the result at the given position
*/
function setSelectedSpan(div, spanNum) {
    var count = -1;
    var thisSpan;
    var spans = div.getElementsByTagName("div");
    if (spans) {
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].id == "dSearch") {
                if (++count == spanNum) {

                    _highlightResult(spans[i]);
                    thisSpan = spans[i];
                } else {
                    _unhighlightResult(spans[i]);
                }
            }
        }
    }
    return thisSpan;
}

