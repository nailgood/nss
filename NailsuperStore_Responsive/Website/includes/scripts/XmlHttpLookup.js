
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
var DIV_HIGHLIGHT_COLOR = "#fff"; //#3366cc #eeeeee  #f2eef1 #477DB7 E1E1E1
var FONT_COLOR = "#333333"; //#454545  #707070
var FONT_HOVER_COLOR = "#FFFFFF"; //#ffffff
var divWidth = "310px";
var chkShow = "";
var vFName = "";
var useSolr = false;

function InitQueryCode(queryFieldName, lookupURLPrefix, hiddenDivName) {
    if (lookupURLPrefix.toLowerCase().indexOf('solr') > -1)
        useSolr = true;

    vFName = queryFieldName;
    queryField = document.getElementsByName(queryFieldName).item(0);
    queryField.onblur = hideDiv;
    queryField.onkeydown = keypressHandler;
    queryField.autocomplete = "off";

    $(queryField).bind('input', mainLoop);

    lookupURL = lookupURLPrefix;
    if (hiddenDivName)
        divName = hiddenDivName;
    else
        divName = "querydiv";
    ifName = "queryiframe";
    addToCache("", new Array(), new Array());
    setTimeout("mainLoop()", 100);
}
function InitQueryCode2(queryFieldName, lookupURLPrefix, hiddenDivName, show) {
    chkShow = show;
    InitQueryCode(queryFieldName, lookupURLPrefix, hiddenDivName);
}
function addToCache(queryString, resultArray1, resultArray2, suggestion) {
    cache[queryString] = new Array(resultArray1, resultArray2, suggestion);
}
mainLoop = function () {
    val = queryField.value;
    if (val.length > 50) {
        alert("Your queries are too long. Please limit to 50 characters..");
        $("#LookupField").val('');
        return;
    }
    if (val.length < 2) {
        if (!useSolr)
            showQueryDivSearch(val, undefined, undefined);
        else
            showQueryDivSearchSolr(val, undefined, undefined, undefined);
        return;
    }

    if (lastVal != val && searching == false) {
        var cacheResult = cache[val];
        if (val == 'al') {
            cacheResult = undefined;
        }
        if (cacheResult) {
            showQueryDiv(val, cacheResult[0], cacheResult[1], cacheResult[2]);

            if (!useSolr)
                showQueryDivSearch(val, cacheResult[0], cacheResult[1]);
            else
                showQueryDivSearchSolr(val, cacheResult[0], cacheResult[1], cacheResult[2]);
        }
        else {
            if (!useSolr)
                doRemoteQuery(val);
            else
                doRemoteQuerySolr(val);
        }
        lastVal = val;
    }

    setTimeout("mainLoop()", 100);
    return true;
}
;
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
        //        var x = queryField.offsetLeft;
        //        var y = queryField.offsetTop + queryField.offsetHeight;
        //        var parent = queryField;
        //        while (parent.offsetParent) {
        //            parent = parent.offsetParent;
        //            x += parent.offsetLeft;
        //            y += parent.offsetTop;
        //        }

        // add some formatting to the div, if we haven't already
        //if (!divFormatted) {
        //globalDiv.style.backgroundColor = DIV_BG_COLOR;
        //      globalDiv.style.fontFamily = "Arial, Helvetica, sans-serif";
        //globalDiv.style.padding = "4px";
        //      globalDiv.style.border = "1px solid #666666";
        //      globalDiv.style.fontSize = "13px";
        //      globalDiv.style.width = divWidth;
        //      globalDiv.style.position = "absolute";
        // globalDiv.style.left = x + "px";
        // globalDiv.style.top = y + "px";
        //      globalDiv.style.visibility = "hidden";
        //      globalDiv.style.zIndex = 10000;
        //      globalDiv.style.height = "550px";
        //      globalDiv.style.overflow = "auto";
        // divFormatted = true;
        //}
    }

    return globalDiv;
}
function showQueryDiv3(queryString, resultArray1, resultArray2) {
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
        result.appendChild(result1);
        div.appendChild(result);
    }

    // if this resultset isn't already in our cache, add it
    var isCached = cache[queryString];
    if (!isCached)
        addToCache(queryString, resultArray1, resultArray2);

    // display the div if we had at least one result

    showDiv(resultArray1.length > 0);
}
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
        result.appendChild(result1);
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

    if (resultArray1 === undefined || resultArray2 === undefined) {
        showDiv(false); return;
    };
    if (resultArray1.length > 0) {
        var link = document.createElement("div");
        link.className = "linkSearchAll";
        link.innerHTML = "See All Result Searches for \"" + "<span class='highlightKeyword'>" + queryField.value + "</span>" + "\"";
        //link.setAttribute("onmousedown", "MyCallbackKeyword();");
        link.onmousedown = selectResultAll;
        link.onmouseover = highlight;
        link.onmouseout = unhighlight;
        div.appendChild(link);
    }
    //add keyword to div search
    if (resultArray7 != undefined && resultArray7.length > 0) {
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

        result1.innerHTML = resultArray1[i];
        var img = document.createElement("div");
        img.style.cssFloat = "left";
        img.className = "img";

        img.innerHTML = "<img src='/assets/items/small/" + resultArray3[i] + "'>";

        var sku = resultArray5[i];
        var resultSKU = document.createElement("span");
        resultSKU.className = "sku";
        resultSKU.innerHTML = ' #' + sku;
        divtitle.appendChild(result1);
        divtitle.appendChild(resultSKU);
        divContent.appendChild(divtitle);


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
    showDiv(resultArray1.length > 0 || (resultArray7 != undefined && resultArray7.length > 0));
}
function showQueryDivSearchSolr(queryString, resultArray, highlighting, suggestion) {

    var div = getDiv(divName);

    // remove any results that are already there
    while (div.childNodes.length > 0)
        div.removeChild(div.childNodes[0]);

    if (resultArray === undefined || highlighting === undefined) {
        if (suggestion != undefined)
            showDiv(suggestion.length > 0);
        else
            showDiv(false);
        return;
    };

    if (queryField.value == '') {
        showDiv(false);
        return;
    }

    var check = false;
    if ($(window).width() < 768)
        check = true;


    //Khong co ket qua.
    var hasItem = false; var RedirectIndex = -1;
    for (var i = 0; i < resultArray.length; i++) {
        if (resultArray[i].hasredirect == true && resultArray[i].itemname == queryField.value.toLowerCase().trim()) {
            RedirectIndex = i;
            break;
        }
    }
    if (RedirectIndex > -1) {
        var link = document.createElement("div");
        link.className = "linkSearchAll";
        var linkUrlCode = '';
        if (resultArray[i].urlcode != undefined && resultArray[i].urlcode.length > 0)
            linkUrlCode = resultArray[i].urlcode
        else
            linkUrlCode = queryString

        link.innerHTML = "Go to <span class='highlightKeyword'>" + linkUrlCode + "</span>";
        link.onmousedown = selectResultAll;
        link.onmouseover = highlight;
        link.onmouseout = unhighlight;
        div.appendChild(link);
        showDiv(true);
        return;
    }

    var isinbulk = true;
    for (var i = 0; i < resultArray.length; i++) {
        if (isinbulk && !resultArray[i].isinbulk) {
            isinbulk = false;
        }

        if (!(highlighting[resultArray[i].id].itemname === undefined) || !(highlighting[resultArray[i].id].autocompleteitemname === undefined) || highlighting[resultArray[i].id].itemnamepricedesc !== undefined) //sku
        {
            if (resultArray[i].hasredirect == true) continue;
            hasItem = true;
            break;
        }
    }

    var isSkuItem = false;
    if (hasItem == false && resultArray.length == 1 && resultArray[0].hasredirect == false) {
        hasItem = true;

        var isnum = /^\d+$/.test(queryString);
        if (isnum)
            isSkuItem = true;
    }

    
    if (resultArray.length > 0 && !check) {
        var link = document.createElement("div");
        link.className = "linkSearchAll";
        link.innerHTML = "See All Result Searches for \"" + "<span class='highlightKeyword'>" + queryField.value + "</span>" + "\"";
        //link.setAttribute("onmousedown", "MyCallbackKeyword();");
        link.onmousedown = selectResultAll;
        link.onmouseover = highlight;
        link.onmouseout = unhighlight;
        div.appendChild(link);
    }

    if (!isinbulk) {
        if (suggestion != undefined && suggestion.length > 0) {
            var title = document.createElement("div");
            title.className = "SearchTitle";
            title.innerHTML = "POPULAR KEYWORD";
            div.appendChild(title);

            var count = 5; //hien thi 5 kq
            for (var i = 0; i < suggestion.length; i++) {
                var kw = suggestion[i].term;
                if (kw == queryString) continue;
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
                result2.style.cssText = "text-transform:capitalize;";
                result2.Id = suggestion[i].term;
                result2.className = "result2";
                simpletext = new RegExp("(" + queryField.value.toLowerCase().trim() + ")", "gi");
                result2.innerHTML = suggestion[i].term.replace(simpletext, "<span class='highlightKeyword' style='text-decoration: underline;font-weight: bold;'>$1</span>");
                result.appendChild(result2);

                //result.appendChild(result2);
                div.appendChild(result);
                count--;
                if (count == 0) break;
            }
            if (count == 5)
                $(title).hide();
        }
    };

    var title = document.createElement("div");
    title.className = "SearchTitle";
    title.innerHTML = "PRODUCT MATCHES";
    div.appendChild(title);

    var isLogin = $('#isLogin').val() == "0" ? false : true;

    // add an entry for each of the results in the resultArray
    var count = 0;
    for (var i = 0; i < resultArray.length; i++) {
        if (resultArray[i].hasredirect == true) continue;

        if (check && count == 5) break;

        // each result will be contained within its own div
        var result = document.createElement("div");
        result.id = "dSearch";

        if (i % 2 == 0) {
            result.className = "dvSearch alt";
        }
        else {
            result.className = "dvSearch";
        }
        _unhighlightResult(result);
        result.onmousedown = selectResult;
        result.onmouseover = highlightResult;
        result.onmouseout = unhighlightResult;
        var divContent = document.createElement("div");
        var divtitle = document.createElement("div");
        divtitle.className = "title";
        var result1 = document.createElement("span");
        result1.Id = resultArray[i].urlcode + "/" + resultArray[i].id;
        result1.className = "result1";

        try {
            //result1.innerHTML = highlighting[resultArray[i].id].autocomplete[0];
            if (!(highlighting[resultArray[i].id].autocompleteitemname === undefined))
                result1.innerHTML = highlighting[resultArray[i].id].autocompleteitemname;
            else if (!(highlighting[resultArray[i].id].itemname === undefined))
                result1.innerHTML = highlighting[resultArray[i].id].itemname;
            else {
                simpletext = new RegExp("(" + queryField.value.toLowerCase() + ")", "gi");
                result1.innerHTML = resultArray[i].itemname.replace(simpletext, "<span class='highlightKeyword'>$1</span>");
            }
        }
        catch (err) {
            simpletext = new RegExp("(" + queryField.value.toLowerCase() + ")", "gi");
            result1.innerHTML = resultArray[i].itemname.replace(simpletext, "<span class='highlightKeyword'>$1</span>");
        }
        var img = document.createElement("div");
        img.style.cssFloat = "left";
        img.className = "img";

        img.innerHTML = "<img src='/assets/items/small/" + resultArray[i].image + "'>";

        var sku = '';
        if (!isSkuItem)
            sku = resultArray[i].sku;
        else
            sku = highlighting[resultArray[i].id].sku;

            var resultSKU = document.createElement("span");
            resultSKU.className = "sku";
            resultSKU.innerHTML = ' #' + sku;
            divtitle.appendChild(result1);
            
        if(!check)
            divtitle.appendChild(resultSKU);

        divContent.appendChild(divtitle);

            var price = document.createElement("div");
            //            price.style.cssFloat = "right";
            price.className = "price";
            if (isLogin || (!isLogin && resultArray[i].enableisloginviewprice == false))
                price.innerHTML = resultArray[i].pricedesc === 'undefined' ? "" : resultArray[i].pricedesc;
            else
                price.innerHTML = "&nbsp;";

            if(!check)
                divContent.appendChild(price);

        result.appendChild(img);
        result.appendChild(divContent);

        //result.appendChild(result2);
        div.appendChild(result);
        if (check)
            count++;
    }
   
    // if this resultset isn't already in our cache, add it
    var isCached = cache[queryString];
    if (!isCached)
        addToCache(queryString, resultArray, highlighting, suggestion);
    // display the div if we had at least one result

    showDiv(resultArray.length > 0);
    //if (check) {
    //    $('form').css({ "position": "fixed", "overflow-y": "hidden", "width": "100%" });
    //    var height = document.createElement("div");
    //    height.className = 'dvSearchTemp';
    //    div.appendChild(height);
    //    $("#top-bar").removeClass('fixed-top');
    //    $("#top-bar").removeClass('scroll-top');
    //}

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
        result.appendChild(result1);
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
    item.style.backgroundColor = "#fff";
}


/**
This either shows or hides the lookup div, depending on the value of
the "show" parameter.
*/
function showDiv(show) {
    var div = getDiv(divName);
    if (show && chkShow == '') {
        div.style.visibility = "visible";
        var w = ViewPortVidth();
        if (w >= 768) {
            // $('#varHidden').width('300');
            $("#varHidden").css({
                "min-width": "450px"
            });
        }
        if (w < 768) {
            $("#varHidden").css({
                "min-width": "auto"
            });
        }
        div.style.zIndex = 999999;
    }
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
function hideDiv() {
    showDiv(false);
}
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
function doRemoteQuerySolr(queryString) {
    queryString = queryString.replace("\\", "").replace('\"', '').toLowerCase();
    searching = true;
    $.when(
            $.ajax({
                url: '/store/search-result.aspx/getSearch',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{ queryString: "' + queryString + '" }'
            })
            , $.ajax({
                'url': '/solr/mail/suggesthandler',
                'data': { 'wt': 'json', 'q': queryString },
                'dataType': 'jsonp',
                'jsonp': 'json.wrf'
            })
        )
        .done(function (responseSearch, responseSuggest) {
            var resultSearch = $.parseJSON(responseSearch[0].d);
            if (resultSearch == null || resultSearch == undefined
                || resultSearch.response == undefined
                || resultSearch.response == null) {
                showQueryDivSearchSolr(queryString, undefined, undefined);
                searching = false;
                return;
            }
            var resultSuggest = responseSuggest[0];
            showQueryDivSearchSolr(queryString, resultSearch.response.docs, resultSearch.highlighting
                , resultSuggest.suggest.mySuggester[queryString].suggestions);
            searching = false;
        });

}
function doRemoteQuery(queryString) {
    searching = true;
    if (xmlHttp && xmlHttp.readyState != 0) {
        xmlHttp.abort();
    }
    xmlHttp = getXMLHTTP();
    if (xmlHttp) {
        xmlHttp.open("GET", lookupURL + queryString, true);

        // What do we do when the response comes back?
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState == 4 && xmlHttp.responseText && searching) {
                eval(xmlHttp.responseText);
                searching = false;
            }
        };
        xmlHttp.send(null);
    }

}
function keypressHandler(evt) {
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
        return;

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
