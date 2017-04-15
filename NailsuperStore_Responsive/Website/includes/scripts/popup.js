function toggle(div_id) {
    var el = document.getElementById(div_id);
    if (el.style.display == 'none') { el.style.display = 'block'; }
    else { el.style.display = 'none'; }
}
function blanket_size(popUpDivVar) {
    if (typeof window.innerWidth != 'undefined') {
        viewportheight = window.innerHeight;
    } else {
        viewportheight = document.documentElement.clientHeight;
    }
    if ((viewportheight > document.body.parentNode.scrollHeight) && (viewportheight > document.body.parentNode.clientHeight)) {
        blanket_height = viewportheight;
    } else {
        if (document.body.parentNode.clientHeight > document.body.parentNode.scrollHeight) {
            blanket_height = document.body.parentNode.clientHeight;
        } else {
            blanket_height = document.body.parentNode.scrollHeight;
        }
    }
    var blanket = document.getElementById('blanket');
    blanket.style.height = blanket_height + 'px';
    var popUpDiv = document.getElementById(popUpDivVar);
    popUpDiv_height = blanket_height / 2 - 150; //150 is half popup's height
    popUpDiv.style.top = popUpDiv_height + 'px';
}
function window_pos(popUpDivVar) {
    if (typeof window.innerWidth != 'undefined') {
        viewportwidth = window.innerHeight;
    } else {
        viewportwidth = document.documentElement.clientHeight;
    }
    if ((viewportwidth > document.body.parentNode.scrollWidth) && (viewportwidth > document.body.parentNode.clientWidth)) {
        window_width = viewportwidth;
    } else {
        if (document.body.parentNode.clientWidth > document.body.parentNode.scrollWidth) {
            window_width = document.body.parentNode.clientWidth;
        } else {
            window_width = document.body.parentNode.scrollWidth;
        }
    }
    var popUpDiv = document.getElementById(popUpDivVar);
    window_width = window_width / 2 - 235; //150 is half popup's width
    popUpDiv.style.left = window_width + 'px';
}
function popup(windowname) {
    blanket_size(windowname);
    window_pos(windowname);
    toggle('blanket');
    toggle(windowname);
}


/***div popup*****////



function GetHeight() {
    var scrollX, scrollY, windowX, windowY, pageX, pageY;
    if (window.innerHeight && window.scrollMaxY) {
        scrollX = document.body.scrollWidth;
        scrollY = window.innerHeight + window.scrollMaxY;
    } else if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac
        scrollX = document.body.scrollWidth;
        scrollY = document.body.scrollHeight;
    } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
        scrollX = document.body.offsetWidth;
        scrollY = document.body.offsetHeight;
    }

    if (self.innerHeight) {    // all except Explorer
        windowX = self.innerWidth;
        windowY = self.innerHeight;
    } else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
        windowX = document.documentElement.clientWidth;
        windowY = document.documentElement.clientHeight;
    } else if (document.body) { // other Explorers
        windowX = document.body.clientWidth;
        windowY = document.body.clientHeight;
    }

    pageY = (scrollY < windowY) ? windowY : scrollY; // for small pages with total height less then height of the viewport
    pageX = (scrollX < windowX) ? windowX : scrollX; // for small pages with total width less then width of the viewport
    return { pageWidth: scrollX, pageHeight: scrollY, winWidth: windowX, winHeight: windowY };
}

// returns the size of the document
function f_documentSize() {

    var n_scrollX = 0,
    n_scrollY = 0;

    if (typeof (window.pageYOffset) == 'number') {
        n_scrollX = window.pageXOffset;
        n_scrollY = window.pageYOffset;
    }
    else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        n_scrollX = document.body.scrollLeft;
        n_scrollY = document.body.scrollTop;
    }
    else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        n_scrollX = document.documentElement.scrollLeft;
        n_scrollY = document.documentElement.scrollTop;
    }

    if (typeof (window.innerWidth) == 'number')
        return [window.innerWidth, window.innerHeight, n_scrollX, n_scrollY];
    if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight))
        return [document.documentElement.clientWidth, document.documentElement.clientHeight, n_scrollX, n_scrollY];
    if (document.body && (document.body.clientWidth || document.body.clientHeight))
        return [document.body.clientWidth, document.body.clientHeight, n_scrollX, n_scrollY];
    return [0, 0];
}

function f_putScreen(e_message, e_screen, b_show) {
    if (b_show) {
        e_message.className = 'dialogWindow';
        e_message.style.display = '';
        // set properties
        var a_docSize = f_documentSize();
        var pageSize = GetHeight();
        e_screen.style.top = 0 + 'px';
        e_screen.style.height = pageSize.pageHeight + 'px';
        e_screen.style.width = pageSize.pageWidth + 'px';
        e_screen.style.left = 0 + 'px';
        e_screen.style.display = 'block';
        e_screen.className = 'lightbox';
        var n_height = e_message.clientHeight;
        var n_width = e_message.clientWidth;
        //e_message.style.left = ((a_docSize[0] - n_width) / 2) + a_docSize[2] + 'px';
        //e_message.style.top = ((a_docSize[1] - n_height) / 2) + a_docSize[3] + 'px';
        var left = ((a_docSize[0] - n_width) / 2) + a_docSize[2];
        var val = 0;
        if (e_message.id == "dNotice") {
            val = 40;
        }
        if (e_message.id == "dSuccess") {
            val = 80;
        }
        var top = ((a_docSize[1] - n_height) / 2) + a_docSize[3] - val;
        if (left < 0)
            left = 0;
        if (top < 0)
            top = 0;
        e_message.style.left = left + 'px';
        e_message.style.top = top + 'px';
    }
    else {
        e_screen.style.display = 'none';
        e_message.className = '';
        e_message.style.display = 'none';
    }
}

