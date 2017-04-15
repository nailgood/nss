/* Modernizr 2.8.3 (Custom Build) | MIT & BSD
* use this script check smartphone
*/
; window.Modernizr = function (a, b, c) { function u(a) { j.cssText = a } function v(a, b) { return u(prefixes.join(a + ";") + (b || "")) } function w(a, b) { return typeof a === b } function x(a, b) { return !! ~("" + a).indexOf(b) } function y(a, b, d) { for (var e in a) { var f = b[a[e]]; if (f !== c) return d === !1 ? a[e] : w(f, "function") ? f.bind(d || b) : f } return !1 } var d = "2.8.3", e = {}, f = !0, g = b.documentElement, h = "modernizr", i = b.createElement(h), j = i.style, k, l = {}.toString, m = {}, n = {}, o = {}, p = [], q = p.slice, r, s = {}.hasOwnProperty, t; !w(s, "undefined") && !w(s.call, "undefined") ? t = function (a, b) { return s.call(a, b) } : t = function (a, b) { return b in a && w(a.constructor.prototype[b], "undefined") }, Function.prototype.bind || (Function.prototype.bind = function (b) { var c = this; if (typeof c != "function") throw new TypeError; var d = q.call(arguments, 1), e = function () { if (this instanceof e) { var a = function () { }; a.prototype = c.prototype; var f = new a, g = c.apply(f, d.concat(q.call(arguments))); return Object(g) === g ? g : f } return c.apply(b, d.concat(q.call(arguments))) }; return e }); for (var z in m) t(m, z) && (r = z.toLowerCase(), e[r] = m[z](), p.push((e[r] ? "" : "no-") + r)); return e.addTest = function (a, b) { if (typeof a == "object") for (var d in a) t(a, d) && e.addTest(d, a[d]); else { a = a.toLowerCase(); if (e[a] !== c) return e; b = typeof b == "function" ? b() : b, typeof f != "undefined" && f && (g.className += " " + (b ? "" : "no-") + a), e[a] = b } return e }, u(""), i = k = null, function (a, b) { function l(a, b) { var c = a.createElement("p"), d = a.getElementsByTagName("head")[0] || a.documentElement; return c.innerHTML = "x<style>" + b + "</style>", d.insertBefore(c.lastChild, d.firstChild) } function m() { var a = s.elements; return typeof a == "string" ? a.split(" ") : a } function n(a) { var b = j[a[h]]; return b || (b = {}, i++, a[h] = i, j[i] = b), b } function o(a, c, d) { c || (c = b); if (k) return c.createElement(a); d || (d = n(c)); var g; return d.cache[a] ? g = d.cache[a].cloneNode() : f.test(a) ? g = (d.cache[a] = d.createElem(a)).cloneNode() : g = d.createElem(a), g.canHaveChildren && !e.test(a) && !g.tagUrn ? d.frag.appendChild(g) : g } function p(a, c) { a || (a = b); if (k) return a.createDocumentFragment(); c = c || n(a); var d = c.frag.cloneNode(), e = 0, f = m(), g = f.length; for (; e < g; e++) d.createElement(f[e]); return d } function q(a, b) { b.cache || (b.cache = {}, b.createElem = a.createElement, b.createFrag = a.createDocumentFragment, b.frag = b.createFrag()), a.createElement = function (c) { return s.shivMethods ? o(c, a, b) : b.createElem(c) }, a.createDocumentFragment = Function("h,f", "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" + m().join().replace(/[\w\-]+/g, function (a) { return b.createElem(a), b.frag.createElement(a), 'c("' + a + '")' }) + ");return n}")(s, b.frag) } function r(a) { a || (a = b); var c = n(a); return s.shivCSS && !g && !c.hasCSS && (c.hasCSS = !!l(a, "article,aside,dialog,figcaption,figure,footer,header,hgroup,main,nav,section{display:block}mark{background:#FF0;color:#000}template{display:none}")), k || q(a, c), a } var c = "3.7.0", d = a.html5 || {}, e = /^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i, f = /^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i, g, h = "_html5shiv", i = 0, j = {}, k; (function () { try { var a = b.createElement("a"); a.innerHTML = "<xyz></xyz>", g = "hidden" in a, k = a.childNodes.length == 1 || function () { b.createElement("a"); var a = b.createDocumentFragment(); return typeof a.cloneNode == "undefined" || typeof a.createDocumentFragment == "undefined" || typeof a.createElement == "undefined" } () } catch (c) { g = !0, k = !0 } })(); var s = { elements: d.elements || "abbr article aside audio bdi canvas data datalist details dialog figcaption figure footer header hgroup main mark meter nav output progress section summary template time video", version: c, shivCSS: d.shivCSS !== !1, supportsUnknownElements: k, shivMethods: d.shivMethods !== !1, type: "default", shivDocument: r, createElement: o, createDocumentFragment: p }; a.html5 = s, r(b) } (this, b), e._version = d, g.className = g.className.replace(/(^|\s)no-js(\s|$)/, "$1$2") + (f ? " js " + p.join(" ") : ""), e } (this, this.document), function (a, b, c) { function d(a) { return "[object Function]" == o.call(a) } function e(a) { return "string" == typeof a } function f() { } function g(a) { return !a || "loaded" == a || "complete" == a || "uninitialized" == a } function h() { var a = p.shift(); q = 1, a ? a.t ? m(function () { ("c" == a.t ? B.injectCss : B.injectJs)(a.s, 0, a.a, a.x, a.e, 1) }, 0) : (a(), h()) : q = 0 } function i(a, c, d, e, f, i, j) { function k(b) { if (!o && g(l.readyState) && (u.r = o = 1, !q && h(), l.onload = l.onreadystatechange = null, b)) { "img" != a && m(function () { t.removeChild(l) }, 50); for (var d in y[c]) y[c].hasOwnProperty(d) && y[c][d].onload() } } var j = j || B.errorTimeout, l = b.createElement(a), o = 0, r = 0, u = { t: d, s: c, e: f, a: i, x: j }; 1 === y[c] && (r = 1, y[c] = []), "object" == a ? l.data = c : (l.src = c, l.type = a), l.width = l.height = "0", l.onerror = l.onload = l.onreadystatechange = function () { k.call(this, r) }, p.splice(e, 0, u), "img" != a && (r || 2 === y[c] ? (t.insertBefore(l, s ? null : n), m(k, j)) : y[c].push(l)) } function j(a, b, c, d, f) { return q = 0, b = b || "j", e(a) ? i("c" == b ? v : u, a, b, this.i++, c, d, f) : (p.splice(this.i++, 0, a), 1 == p.length && h()), this } function k() { var a = B; return a.loader = { load: j, i: 0 }, a } var l = b.documentElement, m = a.setTimeout, n = b.getElementsByTagName("script")[0], o = {}.toString, p = [], q = 0, r = "MozAppearance" in l.style, s = r && !!b.createRange().compareNode, t = s ? l : n.parentNode, l = a.opera && "[object Opera]" == o.call(a.opera), l = !!b.attachEvent && !l, u = r ? "object" : l ? "script" : "img", v = l ? "script" : u, w = Array.isArray || function (a) { return "[object Array]" == o.call(a) }, x = [], y = {}, z = { timeout: function (a, b) { return b.length && (a.timeout = b[0]), a } }, A, B; B = function (a) { function b(a) { var a = a.split("!"), b = x.length, c = a.pop(), d = a.length, c = { url: c, origUrl: c, prefixes: a }, e, f, g; for (f = 0; f < d; f++) g = a[f].split("="), (e = z[g.shift()]) && (c = e(c, g)); for (f = 0; f < b; f++) c = x[f](c); return c } function g(a, e, f, g, h) { var i = b(a), j = i.autoCallback; i.url.split(".").pop().split("?").shift(), i.bypass || (e && (e = d(e) ? e : e[a] || e[g] || e[a.split("/").pop().split("?")[0]]), i.instead ? i.instead(a, e, f, g, h) : (y[i.url] ? i.noexec = !0 : y[i.url] = 1, f.load(i.url, i.forceCSS || !i.forceJS && "css" == i.url.split(".").pop().split("?").shift() ? "c" : c, i.noexec, i.attrs, i.timeout), (d(e) || d(j)) && f.load(function () { k(), e && e(i.origUrl, h, g), j && j(i.origUrl, h, g), y[i.url] = 2 }))) } function h(a, b) { function c(a, c) { if (a) { if (e(a)) c || (j = function () { var a = [].slice.call(arguments); k.apply(this, a), l() }), g(a, j, b, 0, h); else if (Object(a) === a) for (n in m = function () { var b = 0, c; for (c in a) a.hasOwnProperty(c) && b++; return b } (), a) a.hasOwnProperty(n) && (!c && ! --m && (d(j) ? j = function () { var a = [].slice.call(arguments); k.apply(this, a), l() } : j[n] = function (a) { return function () { var b = [].slice.call(arguments); a && a.apply(this, b), l() } } (k[n])), g(a[n], j, b, n, h)) } else !c && l() } var h = !!a.test, i = a.load || a.both, j = a.callback || f, k = j, l = a.complete || f, m, n; c(h ? a.yep : a.nope, !!i), i && c(i) } var i, j, l = this.yepnope.loader; if (e(a)) g(a, 0, l, 0); else if (w(a)) for (i = 0; i < a.length; i++) j = a[i], e(j) ? g(j, 0, l, 0) : w(j) ? B(j) : Object(j) === j && h(j, l); else Object(a) === a && h(a, l) }, B.addPrefix = function (a, b) { z[a] = b }, B.addFilter = function (a) { x.push(a) }, B.errorTimeout = 1e4, null == b.readyState && b.addEventListener && (b.readyState = "loading", b.addEventListener("DOMContentLoaded", A = function () { b.removeEventListener("DOMContentLoaded", A, 0), b.readyState = "complete" }, 0)), a.yepnope = k(), a.yepnope.executeStack = h, a.yepnope.injectJs = function (a, c, d, e, i, j) { var k = b.createElement("script"), l, o, e = e || B.errorTimeout; k.src = a; for (o in d) k.setAttribute(o, d[o]); c = j ? h : c || f, k.onreadystatechange = k.onload = function () { !l && g(k.readyState) && (l = 1, c(), k.onload = k.onreadystatechange = null) }, m(function () { l || (l = 1, c(1)) }, e), i ? k.onload() : n.parentNode.insertBefore(k, n) }, a.yepnope.injectCss = function (a, c, d, e, g, i) { var e = b.createElement("link"), j, c = i ? h : c || f; e.href = a, e.rel = "stylesheet", e.type = "text/css"; for (j in d) e.setAttribute(j, d[j]); g || (n.parentNode.insertBefore(e, n), m(c, 0)) } } (this, document), Modernizr.load = function () { yepnope.apply(window, [].slice.call(arguments, 0)) };
/*End modernizr*/
/* File Created: May 29, 2014 */
var isMobile = false;
fnCheckSmartPhone = function () {
    /*neu deviceAgent khong check, con Modernizr check*/
    var deviceAgent = navigator.userAgent.toLowerCase();
    isMobile = Modernizr.touch ||
    (deviceAgent.match(/(iphone|ipod|ipad)/) ||
    deviceAgent.match(/(android)/) ||
    deviceAgent.match(/(iemobile)/) ||
    deviceAgent.match(/iphone/i) ||
    deviceAgent.match(/ipad/i) ||
    deviceAgent.match(/ipod/i) ||
    deviceAgent.match(/blackberry/i) ||
    deviceAgent.match(/bada/i));
    if (isMobile != null)
        isMobile = true;
    else isMobile = false;
}
function IsiPad() {
    var isiPad = navigator.userAgent.match(/iPad/i) != null;
    return isiPad;
}
var cdnurl = ""; //nhap url cnd
var sUrl = "";
(function () {
    if ("-ms-user-select" in document.documentElement.style && navigator.userAgent.match(/IEMobile\/10\.0/)) {
        var msViewportStyle = document.createElement("style");
        msViewportStyle.appendChild(
                    document.createTextNode("@-ms-viewport{width:auto!important}")
                );
        document.getElementsByTagName("head")[0].appendChild(msViewportStyle);
    }

})();

//hide cac popup dang show de mo popup khac khi click
fnResetAllPopup = (function (type) {
    if (type == 1) {  //click menu faq, off menu dept, acc, cart, n-cart
        $("#ListDepartment").removeClass("active");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#ListDepartment div.sub-menu-dept").removeClass("active");
        //$("#acc-down").removeClass("show");
        //$("#acc-down").addClass("hide");
        $("#cart-down").removeClass("show");
        $("#cart-down").addClass("hide");
        $("#n-cart-down").removeClass("show");
        $("#n-cart-down").addClass("hide");
    }
    else if (type == 2) {//click menu dept, off menu faq, resource, acc, cart, n-cart
        $("#menu div.submenu").removeClass("active");
        //$("#acc-down").removeClass("show");
        // $("#acc-down").addClass("hide");
        $("#cart-down").removeClass("show");
        $("#cart-down").addClass("hide");
        $("#n-cart-down").removeClass("show");
        $("#n-cart-down").addClass("hide");
    }
    else if (type == 3) {//click menu acc, off menu faq, resource, dept, cart
        $("#ListDepartment").removeClass("active");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#ListDepartment div.sub-menu-dept").removeClass("active");
        $("#menu div.submenu").removeClass("active");
        $("#cart-down").removeClass("show");
        $("#cart-down").addClass("hide");
    }
    else if (type == 4) {//click menu cart, off menu faq, resource, dept, acc
        $("#ListDepartment").removeClass("active");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#ListDepartment div.sub-menu-dept").removeClass("active");
        $("#menu div.submenu").removeClass("active");
        // $("#acc-down").removeClass("show");
        // $("#acc-down").addClass("hide");
    }
    else if (type == 5) {//click menu cart chua login, off menu faq, resource, dept, acc
        $("#ListDepartment").removeClass("active");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#ListDepartment div.sub-menu-dept").removeClass("active");
        $("#menu div.submenu").removeClass("active");
    }
});
//set link for main department
fnSetLinkMainDepartment = (function () {
    var vhref = "",
           vli = "",
           lurl = "",
           deptId = 0;
    if (window.ViewPortVidth() <= 992) {
        $('.popover-content > ul li.main a.main').attr("href", "javascript:void(0);");
    }
    else {
        $('.popover-content > ul li.main').each(function () {
            //ids.push($(this).attr('id'));
            vli = $(this).attr('id');
            lurl = vli.split("-");
            deptid = lurl[lurl.length - 1];
            vli = vli.replace(lurl[0] + "-" + lurl[1] + "-", "/" + lurl[0] + "-" + lurl[1] + "/");
            vli = vli.replace("-" + deptid, "/" + deptid);
            //hrefs.push($(this).find('a').attr('href'));
            $(this).find('a.main').attr('href', vli);
            vli = "";
        })
    }
});
//top menu
//lay vi tri arrow tu menu de hien thi arrow cho popup menu
fnGetPositionArrowForMenu = (function (element) {
    var e = $(element).find("span")
    if ($(element).find("span").css("display") == "none") {
        e = $(element).find("span").next();
    }
    var pos = $(e).find(".arrow-down").position();
    if (pos != null) {
        var left = (parseInt(pos.left));
        left += 13;
        
        if ($(element).find("div.submenu").css("left") != null && $(element).find("div.submenu").css("left") != "0px") {
            left = left - parseInt($(element).find("div.submenu").css("left").replace("px", ""));
        }
        if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
            $(element).find("div.arrow").css("left", parseInt(left + 3) + "px");
        }
        else {
            $(element).find("div.arrow").css("left", left + "px");
        }
    }
});

//hover de hien thi menu Deal, Resource, FAQ
fnHoverMenu = (function () {
    $('#menu ul li').unbind('click mouseenter mouseleave');
    $("div.submenu").removeClass("active");

    $("#menu ul li").bind("mouseenter", function () {
        $("#ListDepartment").removeClass("active");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#ListDepartment div.sub-menu-dept").removeClass("active");

        $(this).find("div.submenu").addClass("active");
        fnGetPositionArrowForMenu($(this));
    });
    $("#menu ul li").bind("mouseleave", function () {
        $(this).find("div.submenu").removeClass("active");
    });
});

//click de hien thi menu Deal, Resource, FAQ
fnClickMenu = (function () {
    $('#menu ul li').unbind('click mouseenter mouseleave');
    $("div.submenu").removeClass("active");
    $("#ListDepartment").css("position", "");
    $("#menu ul li").click(function () {
        fnResetAllPopup(1);
        if ($(this).find("div.submenu").hasClass("active")) {
            $(this).find("div.submenu").removeClass("active");
        }
        else {
            $("#menu div.submenu").removeClass("active");
            $(this).find("div.submenu").addClass("active");
            fnGetPositionArrowForMenu($(this));
        }
    });
});


///menu department
//hover de hien thi sub menu tu main menu department
fnHoverMainDept = (function () {
    //$('#ListDepartment ul.main-dept li').unbind('click mouseenter mouseleave');
    $("#ListDepartment ul.main-dept > li").bind("mouseenter mouseleave", function () {
        $("#ListDepartment ul.main-dept > li").removeClass("active");
        $("div[id^='sub-']").removeClass("active");
        $(this).addClass("active");
        $("#sub-" + this.id).addClass("active");
    });
})

//click de hien thi sub menu tu main menu department
fnClickMainDept = (function () {
    return;
    $('#ListDepartment ul.main-dept li').unbind('click mouseenter mouseleave');

    $("#ListDepartment ul.main-dept > li").click(function () {
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("div[id^='sub-']").removeClass("active");

        $(this).addClass("active");
        $("#sub-" + this.id).addClass("active");
    });
})

//lay vi tri top, left de hien thi menu deparment va mui ten
fnGetPostionForDepartment = (function () {
    if (!document.getElementById('h-wrapper')) {
        return;
    }

    if (!$("#ListDepartment").hasClass("menu-push")) {
        
        var pos = $("#menu-dept").position();
        var h = $("#menu-dept").height();
        if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
            $("#ListDepartment").css("top", (pos.top + h - 5) + "px");
        }
        else {
            $("#ListDepartment").css("top", (pos.top + h - 6) + "px");
        }
        
        $("#ListDepartment").css("left", pos.left + "px");
        
        if ($("nav").hasClass("fixed-top")) {
            var pathname = window.location.pathname,
                URL = document.getElementById("hidUrl").value;

            if (URL.indexOf("home.aspx") > 0) {
                var posleftaMenuDept = $("#menu-dept").position();
                $("#ListDepartment").css("left", (parseInt(posleftaMenuDept.left)) + "px");
            }
            else {
                var leftpx = 0;
                if (URL.indexOf("sub-category.aspx") > 0 || URL.indexOf("collection.aspx") > 0)
                    leftpx = 5;
                else
                    leftpx = 15;

                $("#ListDepartment").css("left", (parseInt($("#wrapper").position().left) + leftpx) + "px");
            }

            $("#ListDepartment").css("position", "fixed");
        }
        else {
            $("#ListDepartment").css("position", "");
           
        }

        $("#ListDepartment").css("display", "");
        $("#ListDepartment .sub-menu-dept").css("display", "");
        $("#menu-dept").removeClass("open");
        $("#ListDepartment div.arrow").css("left", parseInt($("#menu-dept").width() / 2) + "px");
    }
});
//function de tat menu department
fnSetMenuOffDepartment = (function () {
    if ($("#ListDepartment").hasClass("active")) {
        $("#menu-dept").removeClass("active");
        $("#ListDepartment").removeClass("active");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#ListDepartment div.sub-menu-dept").removeClass("active");

    }
});
//su kien de tat menu department khi hover
fnEventMenuOffDepartment = (function () {
    $("#menu, header, footer, #main").bind("mouseover", function () {
        fnSetMenuOffDepartment();
    });
    $("#ListDepartment").mouseleave(function () {
        fnSetMenuOffDepartment();
    });
});
fnEventMenuOff = (function () {
    $("header, footer, #main").bind("click", function () {
        $("#menu div.submenu").removeClass("active");
        //tat popup cart khi click cho khac
        if ($("#cart-down").hasClass("show") && ClickCart == 0 && window.ViewPortVidth() < 992) {
            $("#cart-down").removeClass("show");
        }
        if (ClickCart == 1) {
            ClickCart = 0;
        }
        //////
        fnSetMenuOffDepartment();
    });
});
function isAllowHoverMainDepartmentMenu() {

    if (document.getElementById('main-department-menu')) {//trang main category khong can show menu department

        return false;
    }
    return true;
}
//hover de hien thi menu department
fnHoverMenuDept = (function () {
    
    $('#menu-dept').unbind('click mouseenter mouseleave');
    
    $('#menu, header, footer, #main').unbind('click mouseover');
    $('#ListDepartment').unbind('mouseleave');
    $("#ListDepartment").removeClass("menu-push");
    $("#ListDepartment").css("overflow", ""); //mat mui ten khi tu small -> large
    
    $("div[id^='sub-']").removeClass("active");
   
    $("#ListDepartment div.sub-menu-dept").removeClass("active");
    
    $("#ListDepartment ul.main-dept li").removeClass("active");
    
    $("div.submenu").removeClass("active");
    
    fnGetPostionForDepartment();
    fnEventMenuOffDepartment();
    fnEventMenuOff();
    if (!isAllowHoverMainDepartmentMenu()) {
        return;
    }
    
    $("#menu-dept").bind("mouseenter mouseleave", function () {
        $("#menu div.submenu").removeClass("active");
        if (!$("#ListDepartment").hasClass("active")) {
            $("#ListDepartment").addClass("active");
            $("#menu-dept").addClass("active");
            fnHoverMainDept();
        }
    });
});

//click de hien thi menu department
fnClickMenuDept = (function () {
    $('#menu-dept').unbind('click mouseenter mouseleave');
    $('#menu, header, footer, #main').unbind('click mouseover');
    $('#ListDepartment').unbind('mouseleave');

    $("#ListDepartment").removeClass("menu-push");
    $("#ListDepartment").css("overflow", ""); //mat mui ten khi tu small -> medium
    $("div[id^='sub-']").removeClass("active");
    $("#ListDepartment div.sub-menu-dept").removeClass("active");
    $("#ListDepartment ul.main-dept li").removeClass("active");
    $("div.submenu").removeClass("active");
    fnGetPostionForDepartment();
    //fnEventMenuOff();
    //    if (!isAllowHoverMainDepartmentMenu()) {
    //        return;
    //    }

    $("#menu-dept").click(function () {
        fnResetAllPopup(2);

        if ($("#ListDepartment").hasClass("active")) {
            $("#ListDepartment").removeClass("active");
            $("#menu-dept").removeClass("active");
            $("#ListDepartment ul.main-dept li").removeClass("active");
            $("#ListDepartment div.sub-menu-dept").removeClass("active");
        }
        else {
            if (window.ViewPortVidth() < 768) {
                if ($("#ListDepartment").hasClass("menu-push")) {
                    $("#ListDepartment ul.main-dept li").removeClass("active");
                    $("#ListDepartment div.sub-menu-dept").removeClass("active");
                }
                else {
                    $("#ListDepartment").addClass("menu-push");
                    fnClickMainPushDept();
                }
            }
            else {
                $("#ListDepartment").addClass("active");
                $("#menu-dept").addClass("active");
                fnClickMainDept();
            }

        }
    });
});

//click day sub menu tu main departmnet
fnClickMainPushDept = (function () {
    $('#ListDepartment ul.main-dept li').unbind('click mouseenter mouseleave');
    $("div.submenu").removeClass("active");

    $("#ListDepartment ul.main-dept li").click(function () {

        return; //khong show sub cate, link thang vao page main category
        $("#ListDepartment ul li div.sub-menu-dept").css("display", "none");
        $("#ListDepartment ul.main-dept li").removeClass("active");
        $("#sub-" + this.id).slideToggle();

    });
});
$("#ListDepartment").on("tap", function () {
    $(this).hide();
});
fnClickListDept = (function () {//click select li (change css backgroup ipad) before redirect

    $("#ListDepartment ul.main-dept li a").removeAttr("href");
    // $('#ListDepartment ul.main-dept li a').unbind('mouseenter mouseleave');
    $("#ListDepartment ul.main-dept li").click(function (event) {


        // $("#ListDepartment ul.main-dept li a").unbind('mouseenter mouseleave')
        // $(this).unbind('mouseenter mouseleave')
        var lid = this.id;
        var a_href = $('#' + lid + ' a').attr('data-context');
        $('#' + lid + ' a').css('color', 'white');
        $('#' + lid).addClass('hover-menu-dept');

        //lid = lid.replace(/\-/g, '/');

        var waitdo = 0;
        clearTimeout(waitdo);

        waitdo = setTimeout(function () {
            window.location.href = a_href;
        }, 50);


    });
});
fnBindhrefMenudept = function () {
    $("#ListDepartment ul.main-dept li").find('a').each(function (event) {

        //    var lid = $("#ListDepartment ul.main-dept li").id;
        if (!$(this).attr('href')) {
            var a_href = $(this).attr('data-context');
            $(this).attr('href', a_href);
        }

    });

}
var btn = false;
//click day menu department
fnClickMenuPushDept = (function (scroll) {
    $('#menu-dept').unbind('click mouseenter mouseleave');
    $('#menu, header, footer, #main').unbind('click');
    $('#ListDepartment').unbind('mouseleave');

    $("div[id^='sub-']").removeClass("active");
    $("#ListDepartment div.sub-menu-dept").removeClass("active");
    $("#ListDepartment ul.main-dept li").removeClass("active");
    $("div.submenu").removeClass("active");
    $("#ListDepartment").css("top", "");
    $("#ListDepartment").css("left", "");
    $("#menu-dept").bind("click", function () {
        //"click menu-dept"
        var menumobile = $('#menu-mobile').css('display');
        if (menumobile == 'block') {
            $("#menu-mobile").css("display", "none");
        }
        $("#menu div.submenu").removeClass("active");
        if ($("#ListDepartment").hasClass("menu-push")) {
            $("#ListDepartment ul.main-dept li").removeClass("active");
            $("#ListDepartment div.sub-menu-dept").removeClass("active");
        }
        else {
            $("#ListDepartment").addClass("menu-push");
            $('html, body').animate({
                scrollTop: 0
            }, 500);
            fnClickMainPushDept();
        }

        if (scroll == 1 || $('#top-bar').position().top > 90) {
            if ($('#top-bar').position().top > 90) {
                $("#ListDepartment").css("display", "none");
                $("#menu-dept").removeClass("open");
            }
            $('html, body').animate({
                scrollTop: 0
            }, 500);
        }
        $("#ListDepartment").slideToggle("slow");
        $("#menu-dept").toggleClass("open");
        fnSetDefaultMenuCus();
    });
});
//////////////////////////////////////

//chuyen doi click va hover menu khi man hinh duoi 940px va push menu khi man hinh < 760
fnChangeEventMenu = (function (scroll) {
    sUrl = document.getElementById("hidUrl").value;
    //Menu popup Department chuyen thanh menu day khi width browser < 768px,

    if (ViewPortVidth() <= 991) { //Menu popup chuyen sang event click hien thi khi browser > 768 va <= 991
        if (ViewPortVidth() < 768) {
            fnClickMenu();
            fnClickMenuPushDept(scroll);

        } else {
            fnClickMenu();
            if (sUrl.indexOf('store/main-category.aspx') < 0)
                fnClickMenuDept();

            if ($(window).scrollTop() >= 100) {
                //$("#ListDepartment").css({'top}'top', '35px');
                $("#ListDepartment").css("left", "1px");
            }
            fnClickListDept();
        }
    }
    else {//Menu popup chuyen sang event hover hien thi khi browser > 991
        
        if (isMobile)//------(ViewPortVidth() == 1024 && $(window).height() < 762)-->khi ipad quay ngang, hover se khong click lai duoc, phai goi event click
        {

            if (sUrl.indexOf('store/main-category.aspx') < 0)
                fnClickMenuDept();

            fnClickMenu();
            // if (sUrl.indexOf('store/main-category.aspx') < 0) {
            /////////////////////////////fnClickMenuDept();
            //}
            if ($(window).scrollTop() >= 100) {
                //$("#ListDepartment").css({'top}'top', '35px');
                $("#ListDepartment").css({ "top": "35px", "position": "fixed" });
            }
        }
        else {
            fnHoverMenu();
            fnHoverMenuDept();
            if (sUrl.indexOf('store/collection.aspx') > 0 || sUrl.indexOf('store/sub-category.aspx') > 0 || sUrl.indexOf('store/shop-save.aspx') > 0) {
                if (window.ViewPortVidth() > 991) {
                    $("#ListDepartment").css('left', '15px');
                }
            }
        }
        fnClickListDept();

    }
});
//fnChangeEventMenu(0);

/*width*/
//function fnOpenLinkAccountCart() {
//    if (window.ViewPortVidth() > 991) {
//        $("#logged .cart a").click(function () {
//            window.location.href = "/store/cart.aspx";
//        });
//    }
//    else {
//        $("#logged .cart a").unbind("click");
//    }
//}

$(window).scroll(function () {
    SetPositionBreadCrumbNav();
    fnCallMenuscroll();
    fnChangeVisibleHeader();
});

$(window).resize(function () {
    SetupBreadCrumb();
    SetPositionBreadCrumbNav();
    fnChangeVisibleHeader();
});

var IsAllow = 0;

function fnChangeVisibleHeader() {
    if ($("#h-wrapper")) {
        try {
            
            $('#menu-dept').unbind('click mouseenter mouseleave');
            fnCheckSmartPhone();

            var hiddomain = document.getElementById("hiddomain").value,
            hidFixMenu = document.getElementById("hidFixMenu").value,
            sUrl = document.getElementById("hidUrl").value;
            fnAllowPage(hiddomain, hidFixMenu, sUrl, IsAllow);
            var scroll = 0;

            //popup cart
            if ($("#cart-down")) {
                var left = parseInt($("li.cart").outerWidth() - $("#cart-down").outerWidth() + 10);
                if (left > 0) left = (left * -1) + 10;
                //console.log($("li.cart").outerWidth() + "-" + $("#cart-down").outerWidth() + "=" + left);
                $('#cart-down').css('left', left);
            }

            //faqs
            if ($("div.faq")) {
                var left = parseInt($("div#menu ul li").outerWidth() - $("div.faq").outerWidth());
                if (window.ViewPortVidth() >= 768) left += 10;

                $('div.faq').css('left', left);
            }

            //deals center
            if (($("#wrapper").hasClass('container-fluid')) || (window.ViewPortVidth() >= 768)) {
                if ($("div.deals-center")) {
                    //console.log(window.ViewPortVidth() +"-"+ $("div#menu").position().left +"-"+ $("div.deals-center").outerWidth())
                    var left = parseInt(window.ViewPortVidth() - $("div#menu").position().left - $("div.deals-center").outerWidth());
                    if (left > 0) {
                        left = (left * -1);
                    }
                    else {
                        if (window.ViewPortVidth() > 768) { //$("#wrapper").hasClass('container-fluid'))
                            left -= 30;
                        }
                        else {
                            left -= 10;
                        }
                    }

                    $('div.deals-center').css('left', left);
                }
            }

            $("#aCart").attr("href", "/store/cart.aspx");
            if (window.ViewPortVidth() < 1170 && window.ViewPortVidth() >= 768) {
                //cart
                if (isMobile) {
                    $("li.cart a").removeAttr("href");
                }
            }

            if (window.ViewPortVidth() < 992) {
                var browser = navigator.userAgent.toLowerCase();

                if ($(window).scrollTop() >= 100) {
                    scroll = 1;
                    fnFixtopHeader();
                }
                else {
                    fnRemoveFixtopHeader();
                }
            }
            else {
                
                //fixed main menu at large screen home, product list
                if (IsAllow == 1) {
                    if ($(window).scrollTop() < 100) {
                        fnRemoveFixtopHeader();
                    }
                    else {
                        fnFixtopHeader();
                        fnSetWidthMenu();
                        $("#top-bar").css("display", "");
                        if (sUrl.indexOf('store/main-category.aspx') > 0) {
                            fnDepartmenttotop();
                        }
                    }
                    fnHoverMenuDept();

                }
                else {
                    fnRemoveFixtopHeader();
                }
            }

            if (window.ViewPortVidth() <= 767) //only show xs and sm
            {
                //$(".w-xs-li").css("width", "25%");
                //$(".container").css("margin:0px!important");
                fnBindhrefMenudept();
                if ($(window).scrollTop() >= 100) {
                    //$("#top-bar").css('width', 'auto');
                    //$("#top-bar").css('display', 'table');

                    //$("h-wrapper").addClass("hidden");
                    //$("h-wrapper").removeClass("show");
                    $("#menu-mobile").removeClass('default');
                    $("#menu-mobile").addClass("fix");

                    //$("#sm-search").removeClass("hidden");
                    //$("#sm-search").addClass("show");
                    //$("#menu").removeClass('d-block');

                    //$("#menu-dept").css("display", "table-cell");
                    scroll = 1;
                }
                else {
                    //$(".w-xs-li").css("width", "33.33333%");
                    $("#top-bar").removeClass('fixed-top');
                    //$("h-wrapper").addClass("show");
                    //$("h-wrapper").removeClass("hidden");
                    //$("#menu-mobile").removeClass('fix');
                    //$("#menu-mobile").addClass("default");
                    //$("#ic-menu").css("padding", "0");
                    //$("#sm-search").removeClass("show");
                    //$("#sm-search").addClass("hidden");
                }
            }
            else {
                //$("#sm-search").removeClass("show");
                //$("#sm-search").addClass("hidden");
            }

            
            if ((window.ViewPortVidth() > 767) && (window.ViewPortVidth() <= 991)) {
                fnGetPostionForDepartment();
            }

            if ($(window).scrollTop() < 200)
                $("#imgTop").css("display", "none");
            else
                $("#imgTop").css("display", "block");

            //return;
            fnChangeEventMenu(scroll);
        }
        catch (err) { }
    }
}
function SetArrowPosition() {

    $('#main-department-menu .arrow').css('display', 'block');
    var left = 0;
    var top = 0;
    var h = 0;
    var p1 = 0;
    try {
    left =  $("#aMenuDept .arrow-down-dept").offset().left;
    top = $("#main-department-menu").offset().top;
    h = $("#main-department-menu .arrow").outerHeight();
    p1 = $("#aMenuDept .arrow-down-dept").position().left;
    }
    catch (err) { }

   
    var windoww = window.ViewPortVidth();
    var webw = $("#h-wrapper").innerWidth();
    var left1 = p1 + ((windoww - webw) / 2);
    top = top - h;

    if ($(window).scrollTop() < 100) {
        top = 129;
    }

    $("#main-department-menu .arrow").offset({ top: top, left: left1 - 10 });
}

fnCallMenuscroll = function () {
    if (document.getElementById("hidUrl")) {
        sUrl = document.getElementById("hidUrl").value;
        if (isMobile) {
            fnClickMenu();
            if (sUrl.indexOf('store/main-category.aspx') > 0) {
                fnDepartmenttotop();
            } else {
                //////////////////////////////fnClickMenuDept();
            }
        } else {
            if (sUrl.indexOf('store/main-category.aspx') > 0) {
                fnDepartmenttotop();
            }
        }
    }

}
fnDepartmenttotop = function () {
    if ($(window).scrollTop() >= 100) {
        var waitdo = 0;
        clearTimeout(waitdo);

        waitdo = setTimeout(function () {
            $("#menu-dept").bind("click", function () {
                $('html, body').animate({ scrollTop: 0 }, 50, function () {
                    // Animation complete.
                    // $('#menu-dept').unbind('click');
                    fnRemoveFixtopHeader();
                    SetArrowPosition();
                });
            });
        }, 500);



    }
    else {
        if (window.ViewPortVidth() < 992) {
            $('#menu-dept').attr('onclick', '').unbind('click');
        }
    }
}

fnAllowPage = function (domain, listUrl, Url) {
    var partUrl = "";
    if (Url.indexOf(domain) > -1) {
        partUrl = Url.split(domain)[1];

        if (partUrl.indexOf("?") > 0) {
            partUrl = partUrl.split("?")[0];
        }
        if (listUrl.indexOf(partUrl) > 0) {
            IsAllow = 1;
        }
        else {
            IsAllow = 0;
        }

    }
}

fnFixtopHeader = function () {
    var browser = navigator.userAgent.toLowerCase();

    $("#top-bar").addClass('fixed-top');
    //$("#top-bar").addClass('scroll-top');
    $("#top-bar").removeClass('n-scroll');
    $("h-wrapper").addClass('hidden');
    $("h-wrapper").removeClass('show');

    $("#menu-mobile").removeClass('default');
    $("#menu-mobile").addClass("fix");
    if (browser.indexOf("iemobile") == -1) {
        $("#top-bar").addClass("shadow");
    }
    else {
        $("#top-bar").removeClass("shadow");
    }

    $("#menu").addClass("d-block");
}
fnRemoveFixtopHeader = function () {
    $("#cart-down").removeClass('cart-fixed');
    $("#top-bar").removeClass('fixed-top');
    $("#top-bar").addClass("n-scroll");
    //$("#top-bar").removeClass("scroll-top");
    $("#top-bar").removeClass("shadow");
    //$('#top-bar').css('width', '100%');

    $("h-wrapper").addClass("show");
    $("h-wrapper").removeClass("hidden");

    $("#menu-mobile").removeClass('fix');
    $("#menu-mobile").addClass("default");


    // $("#menu").css('width', '100%');
    $("#menu").removeClass('d-block');
    $('#endCol').addClass('endCol');
}

fnSetWidthMenu = function () {
    //    if (window.ViewPortVidth() > 991) {
    //        $("#top-bar").removeClass('scroll-top');
    //    }
    //    else {
    //        $("#top-bar").addClass('scroll-top');
    //    }
    //   
    //    $('#endCol').addClass('endCol');
    if (sUrl.indexOf('store/collection.aspx') > 0 || sUrl.indexOf('store/sub-category.aspx') > 0 || sUrl.indexOf('store/shop-save.aspx') > 0) {
        //if (window.ViewPortVidth() > 991) {
        //    $("#top-bar").addClass('scroll-top-full');
        //    $("#top-bar").removeClass('scroll-top');
        //} else {
        //    $("#top-bar").removeClass('scroll-top-full');
        //    $("#top-bar").addClass('scroll-top');
        //}

        $('#endCol').addClass('endCol');
    }
    else {
        //$("#top-bar").removeClass('scroll-top-full');
        $('#menu').addClass('d-block');
        //var wmenu = window.ViewPortVidth();
        //$('#top-bar').css('width', '100%');

        //if (wmenu > 1175)
        //    $('#top-bar').css('width', '100%');
        //else {
        //    if (isMobile)
        //        $('#top-bar').css('width', wmenu - 25 + 'px');
        //    else
        //        $('#top-bar').css('width', wmenu - 45 + 'px');
        //}

        $('#endCol').removeClass('endCol');
        //$("#top-bar").removeClass("scroll-top");
    }

}
////////function fnResizeVisibleHeader() {

////////    var scroll = 0;
////////    //    if ((window.ViewPortVidth() > 768) && (window.ViewPortVidth() <= 992)) {
////////    //        // $("#menu").css("width", "100%");
////////    //    }
////////    // alert(IsAllow);
////////    var hiddomain = document.getElementById("hiddomain").value,
////////        hidFixMenu = document.getElementById("hidFixMenu").value,
////////        sUrl = document.getElementById("hidUrl").value;
////////    fnAllowPage(hiddomain, hidFixMenu, sUrl, IsAllow);
////////    //alert(IsAllow + "---" + sUrl + "----" + hidFixMenu);
////////    if (window.ViewPortVidth() <= 991 || IsAllow == 1) {
////////        $('#top-bar').css('display', 'table');
////////        if ($(window).scrollTop() >= 100) {
////////            $("#top-bar").addClass('fixed-top');
////////            //$("#top-bar").css({ "width": "750px", "padding-right": "30px" });
////////            $("#top-bar").addClass("scroll-top");
////////            $("#top-bar").removeClass("n-scroll");
////////            $("h-wrapper").addClass("hidden");
////////            $("h-wrapper").removeClass("show");
////////            $('#menu').addClass('d-block');
////////            //$("#menu").css('display', 'table-cell');
////////            $("#menu").css("width", "auto");
////////            //$('#top-bar').css('margin-right', '10px');
////////            scroll = 1;

////////        }
////////        else {
////////            $("#top-bar").removeClass('fixed-top');
////////            //$("#top-bar").css({ "width": "100%", "padding": "0px" });
////////            $("#top-bar").addClass("n-scroll");
////////            $("#top-bar").removeClass("scroll-top");
////////            $("h-wrapper").addClass("show");
////////            $("h-wrapper").removeClass("hidden");
////////            //$('#top-bar').css('margin-right', '0px');
////////            // $("#menu").css("width", "100%");
////////        }
////////        $("#cart-down").removeClass("hide");
////////        $("#n-cart-down").removeClass("hide");
////////        $("#logged .cart a").attr("href", "#");
////////        $("#top-bar").css("display", "block");
////////    }
////////    if (window.ViewPortVidth() > 991) {
////////        $("#top-bar").removeClass("scroll-top");
////////        $("#top-bar").css("display", "");
////////        //$("#logged .login .arrow-down-cart").css("display", "");
////////        $("#logged .login").unbind("click");
////////        $("#logged .cart").unbind("click");
////////        $("#n-login .cart").unbind("click");
////////        $("#n-cart-down").removeClass("hide");
////////        $("#n-cart-down").removeClass("show");
////////       // $("#acc-down").removeClass("show");
////////       //fnOpenLinkAccountCart();
////////        //$("#logged .cart > a").attr("href", "/store/cart.aspx");
////////    }
////////    // else {
////////        //       // $("#logged .login .arrow-down-cart").css("display", "none");
////////        
////////    //}
////////    if (window.ViewPortVidth() < 768) {
////////        //$("#top-bar").css({ "width": "100%", "padding-right": "10px" });
////////        $("#sm-search").removeClass("show");
////////        //        $("#sm-search").removeClass("hidden");
////////        $('#menu').removeClass('d-block');
////////        if ($(window).scrollTop() >= 100) {
////////            $("#sm-search").removeClass("hidden");
////////            $("#sm-search").addClass("show");
////////            $("#top-bar").css("display", "table");
////////        } else {
////////            $("#sm-search").removeClass("show");
////////            $("#sm-search").addClass("hidden");
////////            $("#top-bar").css("display", "");
////////        }
////////               // $("#acc-down").removeClass("hide");
////////                //$("#acc-down").removeClass("show");
////////                $("#cart-down").removeClass("show");
////////                $("#cart-down").removeClass("hide");
////////                $("#n-cart-down").removeClass("show");
////////                $("#n-cart-down").removeClass("hide");
////////                //$("#logged .login .arrow-down-cart").css("display", "");
////////               // $("#logged .cart > a").attr("href", "/store/cart.aspx");
////////    }
////////    else {
////////        $("#sm-search").removeClass("show");
////////        $("#sm-search").addClass("hidden");
////////       // $("#top-bar").css("display", "block");
////////    }

////////    if (window.ViewPortVidth() <= 768) {
////////        // $("#top-bar").css({ "width": "auto", "padding-right": "15px" }); ;
////////        //$("#top-bar").css({ "width": "100%", "padding-right": "10px" });
////////        $("#menu").css("width", "100%");

////////        $(".w-xs-li").css("width", "25%");
////////        $("#wrapper").css("margin", "0px!important");
////////        if ($(window).scrollTop() >= 100) {
////////            $("#menu-dept").css("display", "table-cell");
////////            $("#top-bar").addClass('fixed-top');
////////            $("h-wrapper").addClass("hidden");
////////            $("h-wrapper").removeClass("show");
////////            $("#menu").css("width", "100%");
////////            // $("#ic-menu").css("padding-right", "30px");
////////            scroll = 1;
////////            $("#menu").css('display', 'table-cell');
////////        }
////////        else {
////////            $("#sm-search").removeClass("show");
////////            $(".w-xs-li").css("width", "33.33333%");
////////            $("#top-bar").removeClass('fixed-top');
////////            $("h-wrapper").addClass("show");
////////            $("h-wrapper").removeClass("hidden");
////////            //  $("#ic-menu").css("padding", "0");
////////        }
////////        $("#logo").attr("src", "/includes/theme/images/medium-logo.png");
////////    }
////////    else {
////////        $(".w-xs-li").css("width", "33.33333%");
////////        if (IsAllow != 1 || $(window).scrollTop() < 100) {
////////            $("#top-bar").removeClass('fixed-top');
////////        }

////////        $("h-wrapper").removeClass("hidden");
////////        $("#top-bar").removeClass('hidden');
////////        $("#logo").attr("src", "/includes/theme/images/logo.png");
////////        $("#menu").css("width", "100%");
////////        //  $("#ic-menu").css("padding", "0");
////////    }

////////    //fnhideArrowQty();

////////    fnChangeEventMenu(scroll);
////////}
fnReplateContentLeftMenu = function () {
    $(".pro-item").css("display", "none");   //link promotion header
    if ($.trim($("#main-cat").text()) == "") {

        $("#main-cat").addClass("hidden");
    }
    else {
        $("#main-cat").removeClass("hidden");
    }
    var pathname = window.location.pathname;
    if (window.ViewPortVidth() <= 991) {
        $(".ic-minus").removeClass("hidden");
        $(".ic-minus").show();

        if (pathname.indexOf('nail-collection') == -1) {
            $(".filter-by").css("display", "none");
        }
        if (($("#pnDepartmentBrand").text() != "") && ($.trim($("#content-left").text()) == "")) {
            var ctfilter = "<li class='cfilter'><div id='filter'>" + $("#filter").html() + "</div></li>";
            $("#nct-fltr").html('');
            $("#content-mn").append(ctfilter);
            var ctleft = $("#pnDepartmentBrand").html();
            $("#content-left").append(ctleft);
            $("#pnDepartmentBrand").html("");
            $("#leftmenu > ul").css("display", "none");
            $("#leftmenu > div.title-dept").css("border-bottom", "none");
            if ($.trim($("#narrowsearch").text()) == "") {
                $(".cfilter").css("padding-top", "10px");
            }
            fnCollapseDepartment();
            $("#content-left").css("border", "1px solid #c7c8c8");
        }
    }
    else {
        if (pathname.indexOf('nail-collection') == -1) {
            $(".filter-by").css("display", "block");
        }
        $(".filter-by").css("display", "block");
        $(".ic-minus").hide();
        //$("#nct-fltr .cate-title").html("");
        $("#leftmenu > div.title-dept").css("border", "none");
        if (($.trim($("#content-left").text()) != "") && ($.trim($("#pnDepartmentBrand").text()) == "")) {
            if ($("#filter div:last-child").hasClass("tb-cell") == false) {
                if ($(".cfilter").val() != 0) {
                    $("#nct-fltr").html($(".cfilter").html());
                }
            }

            $('#content-mn .cfilter').remove();
            $("#pnDepartmentBrand").append($("#content-left").html());
            $("#content-left").html("");
            $("#content-left").css("border", "none");
            $("#leftmenu > ul").css("display", "block");
        }
    }

    if (window.ViewPortVidth() <= 767) {
        fnSetContent("#order-detail .unit", "#order-detail .xs-unit");
    }
    else {
        $("#order-detail .xs-unit").html("");
    }
}

//fnhideArrowQty = function () {
//    if (window.ViewPortVidth() <= 768) {
//        //$("article .bao-arrow").removeClass("show");
//        //$("article .bao-arrow").addClass("hidden");

////        $('.qty').find('input[type="tel"]').each(function () {
////            $("<input type='number' class='txt-qty' onkeypress='return numbersonly();' maxlength='4' />").attr({ name: this.name, value: this.value, id: this.id}).insertBefore(this);
////        }).remove();

////        $('.qty').find('div[class="bao-arrow"]').each(function () {
////        }).remove();
//    }
//    else {
//        //$("article .bao-arrow").removeClass("hidden");
//        //$("article .bao-arrow").addClass("show");
//    }
//}

fnChangeImageProductList = function () {
    var collection = "#";
    if (hidPageNotScroll.indexOf(partUrl) != -1) {
        collection = "#c";
    }
    else {
        collection = "#";
    }
    var img_array = $(collection + "list-item article .image img").map(function () {
        return $(this).attr("src");
    });
    var imgsrc = "",
    countimg = 0;

    if (window.ViewPortVidth() <= 440) {
        for (i = 0; i < img_array.length; i++) {
            countimg = i + 1;
            imgsrc = String(img_array[i]).replace('/assets/items/medium/', '/assets/items/featured/');
            $(collection + "list-item article #img-" + countimg).attr("src", imgsrc);
        }
    }
    else {
        for (i = 0; i < img_array.length + 1; i++) {
            countimg = i + 1;
            imgsrc = String(img_array[i]).replace('/assets/items/featured/', '/assets/items/medium/');
            $(collection + "list-item article #img-" + countimg).attr("src", imgsrc);
        }
    }
}

fnChangeImageMobile = function (container, newpath, oldpath) {
    var imgsrc = "";
    if (window.ViewPortVidth() <= 640) {
        $(container).each(function (i) {
            imgsrc = $(this).attr("src").replace(oldpath, newpath);
            $(this).attr("src", imgsrc);
        });
    }
    else {
        $(container).each(function (i) {
            imgsrc = $(this).attr("src").replace(newpath, oldpath);
            $(this).attr("src", imgsrc);
        });
    }
}
//set height product list
fnReplacedecimal = function (val) {
    if (val.toString().indexOf(".") != -1) {
        val = val.toString().substring(0, val.toString().indexOf("."));
    }
    return val;
}
fnSetheightLineFix = function () {
    if ($("#uplist #list-item")) {
        $("#line-fix").height($("#uplist #list-item").outerHeight());
        //if ($("#uplist #list-item").outerHeight() < $("#uplist #line-fix").height()) {
        $("#line-fix").height($("#uplist #list-item").outerHeight());
        //}
    }
}
var divValue = 0,
    firstHTML = "",
    countspace = "",
    getChars = 0,
    vreadmore;
fnGetTopRowTitle = function (containers, lineselect, lineheight, minspace, minposReadmore, addMoreWord, endCall) {
    if (containers) {
        if (firstHTML == "") {
            firstHTML = containers.html();
            vreadmore = containers.innerHeight() / lineheight;
        }
        if (vreadmore <= lineselect) {
            if ($(".dept-desc").css("display") == "none") {
                $(".dept-desc").css("display", "block");

            }
            return false;
        }
        var ellipsestext = "&nbsp;......<img src='" + cdnurl + "/includes/theme/images/plus.png'><span></span>",
             lesstext = "&nbsp;<img src='" + cdnurl + "/includes/theme/images/minus.png'>",
             content = $(containers).html();
        if (countspace == "") {
            countspace = content + ellipsestext;
            countspace = countspace.split(" ");
        }

        if (divValue == 0) {
            divValue = (countspace.length / vreadmore).toString();
        }
        else {
            divValue = divValue.toString();
        }
        if (divValue.indexOf(".") != -1) {
            divValue = divValue.substring(0, divValue.indexOf("."));
        }
        if (addMoreWord > 0) {
            content = firstHTML;
            if (endCall) {
                //if (countspace[getChars].length < "read more".length) {
                //13 is length icon plus
                if (countspace[getChars].length <= 13) {
                    addMoreWord = 2;
                }
                getChars = getChars - parseInt(addMoreWord);
            } else {
                getChars = getChars + parseInt(addMoreWord);
            }
        } else {
            getChars = divValue * lineselect;
        }
        var slicetContent = countspace.slice(0, getChars);
        slicetContent = slicetContent.toString().split(",").join(" ");
        var showChar = slicetContent.length; // How many characters are shown by default
        if (content.length > showChar) {
            var c = content.substr(0, showChar);
            var h = content.substr(showChar, content.length - showChar);
            var html = c + '<span class="morecontent">' + h + '</span><span class="moreellipses">' + ellipsestext + '&nbsp;</span>';
            if ($(".dept-desc").css("display") == "none") {
                $(".dept-desc").css("display", "block");

            }
            $(containers).html(html);
        }
        var browser = navigator.userAgent.toLowerCase(),
            leftpos = $(".moreellipses > span").position().left;
        if (browser.indexOf("safari") != -1 && browser.indexOf("chrome") == -1) {
            leftpos = leftpos - 110;
        }
        var spacelastline = window.ViewPortVidth() - leftpos;
        if (spacelastline > minspace && endCall == false && (parseInt(containers.innerHeight() / lineheight) == lineselect)) {
            if (leftpos > minposReadmore) {
                fnGetTopRowTitle(containers, 5, 20, 65, 60, 1, false);
            }
            else {
                fnGetTopRowTitle(containers, 5, 20, 65, 60, 1, true);
            }
        }
        else {
            if (endCall == false) {
                fnGetTopRowTitle(containers, 5, 20, 65, 60, 1, true);
            }
        }
        addMoreWord = 0;

    }
}

fnClickReadmoreDept = function () {
    var ellipsestext = "...<img src='" + cdnurl + "/includes/theme/images/plus.png'><span></span>"
    lesstext = "&nbsp;<img src='" + cdnurl + "/includes/theme/images/minus.png'>",
    $("#cat-desc").click(function () {
        if ($(".moreellipses").hasClass("less")) {
            $(".moreellipses").removeClass("less");
            $(".morecontent").css("display", "none");
            $(".moreellipses").html(ellipsestext);
        } else {
            $(".moreellipses").addClass("less");
            $(".morecontent").css("display", "inline");
            $(".moreellipses").html(lesstext);
        }
    });
}
fnClickReadmoreAboutUs = function () {
    // return;
    var ellipsestext = "...<img src='" + cdnurl + "/includes/theme/images/plus.png'><span></span>"
    lesstext = "&nbsp;<img src='" + cdnurl + "/includes/theme/images/minus.png'>",
    $("#about-desc .moreellipses").click(function () {
        if ($(".moreellipses").hasClass("less")) {
            $(".moreellipses").removeClass("less");
            $(".morecontent").css("display", "none");
            $(".moreellipses").html(ellipsestext);
        } else {
            $(".moreellipses").addClass("less");
            $(".morecontent").css("display", "inline");
            $(".moreellipses").html(lesstext);
        }
    });
}
//set height product list
infoheight = function (container) {
    var $el,
     topPosition = 0,
     toptemp = 0,
     htemp = 0,
     countdiv = 0,
     hval = 0,
     hfix = 0,
     hbtnCart = 0,
     hbtnCarttemp = 0,
     topbtnCart = 0,
     topbtnCarttemp = 0;

    $(container).each(function () {
        countdiv++;
        topbtnCart = $("#info-" + countdiv).position().top;
        topbtnCart = String(topbtnCart).replace(".", "");
        $("#info-" + countdiv).removeAttr('class');
        $("#info-" + countdiv).css("height", "auto");
        hbtnCart = $("#info-" + countdiv).height();
        if (topbtnCart != topbtnCarttemp) {
            topbtnCarttemp = topbtnCart;
            hbtnCarttemp = 0;
            $("#info-" + countdiv).addClass("h-" + topbtnCart);
        }
        else {
            $("#info-" + countdiv).addClass("h-" + topbtnCarttemp);

        }
        if (hbtnCart > hbtnCarttemp) {
            hbtnCarttemp = hbtnCart;
            $(".h-" + topbtnCart).css("height", hbtnCart);

        }
        else {
            $(".h-" + topbtnCart).css("height", hbtnCarttemp);
        }
    });
}
var wait;
if (navigator.userAgent.match(/MSIE\s(?!9.0)/) == false) {
    // ie less than version 9
    window.addEventListener('resize', function (event) {
        clearTimeout(wait);
        wait = setTimeout(function () {
            fnSetheightLineFix();
        }, 500);

    });
}
else {
    $(window).resize(function () {
        clearTimeout(wait);
        wait = setTimeout(function () {
            fnSetheightLineFix();
        }, 500);

    });
}

fnCheckboxNarrowSeach = function () {
    try {

        $("#main-cat input:checkbox,#archives input:checkbox").change(function () {
            $("#main-cat input:checkbox,#archives input:checkbox").attr("checked", false);
            $(this).attr("checked", true);
        });
        $("#dvBrand input:checkbox").change(function () {
            $("#dvBrand input:checkbox").attr("checked", false);
            $(this).attr("checked", true);
        });
        $("#dvRating input:checkbox").change(function () {
            $("#dvRating input:checkbox").attr("checked", false);
            $(this).attr("checked", true);
        });
        $("#dvPrice input:checkbox").change(function () {
            $("#dvPrice input:checkbox").attr("checked", false);
            $(this).attr("checked", true);
        });
    }
    catch (err) { }
}

fnCollapseDepartment = function () {
    $(".ic-minus").addClass("ic-plus");
    $(".ic-minus").click(function () {
        if ($(".ic-minus").hasClass("ic-plus")) {
            $(".ic-minus").removeClass("ic-plus");
            $("#leftmenu > div.title-dept").css("border-bottom", "1px solid #c7c8c8");
        }
        else {
            $(".ic-minus").addClass("ic-plus");
            $("#leftmenu > div.title-dept").css("border", "none");
        }
        $("#leftmenu > ul").slideToggle('fast');
    });
}


fnGetListQty = function () {
    var arrItem = [];
    var arrQty = [];
    $("#clist-item input[id^='txtQtyItem']").each(function () {
        var id = this.id,
        itemid = id.replace("txtQtyItem", ""),
        qty = $("#" + id).val();
        if (qty > 0) {
            arrItem.push(itemid);
            arrQty.push(qty);
        }
    });
    if (arrItem.toString() == "") {
        ShowError("Please select at least one item to add to your shopping cart.");
    }
    else {
        mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', [arrItem, arrQty, false]);
    }

}

function ShowPageLoadError(msg) {
    if (document.getElementById('loadMsg')) {
        var msg = $('#loadMsg').html();
        if (msg != '') {
            showQtip('qtip-error', msg, 'Ooops');
        }
    }

    if (document.getElementById('loadMsgFix')) {
        var msg = $('#loadMsgFix').html();
        if (msg != '') {
            showQtipFix('qtip-error', msg, 'Ooops');
        }
    }

}

function ShowError(msg) {
    showQtip('qtip-error', msg, 'Ooops');
}
$(window).load(function () {
    fnClickReadmoreAboutUs();
    fnClickReadmoreDept();
    fnChangeVisibleHeader();
});
fnSetContent = function (divcontent, divreplace) {
    var content = $(divcontent).html();
    $(divreplace).html(content);
}

//purchase,orderreview
function ScrollDataOrder() {
    var val1 = ",100,200,300,400,500";
    var hscroll = $(window).scrollTop(),
        hdoc = $("#order-detail").height() + $("header").height(); // -$("footer").height(); //$(document).height() - $(window).height() - $("footer").height();
    hscroll = hscroll + $("footer").height() + $("header").height() + 200,
        topmore = $("#loadmore").position().top,
        val = 00,
        lsroll2 = hscroll.toString().substr(hscroll.toString().length - 2, 2),
        ldoc2 = hdoc.toString().substr(hdoc.toString().length - 2, 2),
        eqhscroll = hscroll - lsroll2,
        eqhdoc = hdoc - ldoc2,
        topmore = $("#loadmore").position().top,
        ltopmore = topmore.toString().substr(2, 2),
        etopmore = topmore.toString().replace(ltopmore, "00");
    var browser = navigator.userAgent.toLowerCase();
    if (browser.indexOf("safari") != -1 && browser.indexOf("chrome") == -1) {
        //if ((topmore - hscroll <= val) && isExistScroll != eqhscroll) {
        if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll) {
            isExistScroll = eqhscroll;
            if (pageIndex * pgsize < pageCount || pageIndex * pgsize - pageCount < pgsize) {
                GetOrderRecords();
            }
        }
    }
    else {
        if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll) {
            isExistScroll = eqhscroll;
            if (pageIndex * pgsize < pageCount || pageIndex * pgsize - pageCount < pgsize) {
                GetOrderRecords();
            }
        }
    }

}
function GetOrderRecords() {

    var url = document.getElementById('hidUrl').value,
      arrurl = url.split("?");
    if (arrurl.length > 1)
        url = arrurl[0] + "/Getdata?" + arrurl[1];
    else
        url = arrurl[0] + "/Getdata";
    pageIndex++;
    if ((pgsize * pageIndex <= pageCount || pgsize * pageIndex - 12 < pageCount)) {
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: url,
            data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: LoadSuccess,
            failure: function (response) {
            },
            error: function (response) {
            }
        });

    }

    if (pageIndex * pgsize > pageCount) {
        $("#loadmore").hide();
    }
}
function LoadSuccess(response) {
    var xmlDoc = $.parseXML(response.d);
    var xml = $(xmlDoc);
    var Items = xml.find("Items");
    if (Items.text() == "") {
        document.getElementById("imgloading").style.display = 'none'; ;
    }
    Items.each(function () {
        var itemlist = $(this),
            divContent = itemlist.find("content").text(),
            infoId = itemlist.find("RowNum").text(),
            table = $("#order-detail .order").eq(0).clone(true),
            content = table.html();
        content = content.replace("content1", "content" + infoId);
        table.html(content);
        $(".content" + infoId, table).html(divContent);
        $("#order-detail").append(table);
    });
    $("#loader").hide();
    isLoading = 0;
}
/*********Scroll Paging In ResourceCenter*************/
var isExistScroll;
function ScrollDataResourceCenter() {
    var val1 = ",100,200,300,400,500,600";
    var hlstgallery = $("#lstGallery").height(),
    hlstTip = $("#lstTipCategory").height(),
      hlstVideo = $("#lstvideo").height(),
      hlstMedia = $("#lstMedia").height(),
      hlstNews = $(".news-lst").height();
    hlstShopDesign = $("#shop-designlst").height();
    var hdoc = 0;
    if (hlstgallery > 0) {
        hdoc = hlstgallery;
    }
    else if (hlstTip > 0) {
        hdoc = hlstTip;
    }
    else if (hlstVideo > 0) {
        hdoc = hlstVideo + $('.content-news').height();
    }
    else if (hlstNews > 0) {
        hdoc = hlstNews + $('#news-top').height();
        val1 = ",1300,2200,3100,4000,4900,5800";
    }
    else if (hlstMedia > 0) {
        hdoc = hlstMedia;
    }
    else if (hlstShopDesign > 0) {
        hdoc = hlstShopDesign;
    }
    var hscroll = $(window).scrollTop();
    hdoc = hdoc + $("header").height();
    hscroll = hscroll + $("footer").height() + $("header").height() + 200,
		 val = 00,
        lsroll2 = hscroll.toString().substr(hscroll.toString().length - 2, 2),
        ldoc2 = hdoc.toString().substr(hdoc.toString().length - 2, 2),
        eqhscroll = hscroll - lsroll2,
        eqhdoc = hdoc - ldoc2,
        topmore = $("#loadmoreVideo").position().top,
        ltopmore = topmore.toString().substr(2, 2),
        etopmore = topmore.toString().replace(ltopmore, "00");

    var browser = navigator.userAgent.toLowerCase();
    if (browser.indexOf("safari") != -1 && browser.indexOf("chrome") == -1) {
        //if ((topmore - hscroll <= val) && isExistScroll != eqhscroll) {
        if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll && isLoading == 0) {
            // if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll) {
            isExistScroll = eqhscroll;
            isLoading = 1;
            if (pageIndex * pgsize < pageCount || pageIndex * pgsize - pageCount < pgsize) {
                GetRecordsResourceCenter();
            }
        }
    }
    else {
        // $('#hidtest').val(val1.indexOf(eqhscroll - eqhdoc) + '..' + pageIndex + '..' + pgsize + '..' + eqhscroll + '..' + eqhdoc + '..' + ldoc2);
        //   if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll) {
        if (val1.indexOf(eqhscroll - eqhdoc) != -1 && isExistScroll != eqhscroll && isLoading == 0) {
            isExistScroll = eqhscroll;
            isLoading = 1;
            if (pageIndex * pgsize < pageCount || pageIndex * pgsize - pageCount < pgsize) {
                GetRecordsResourceCenter();
            }
        }
    }
}
function GetRecordsResourceCenter() {
    var url = document.getElementById('hidUrlVideo').value,
      arrurl = url.split("?"),
      categoryId = document.getElementById('hidCategoryId').value;
    if (arrurl.length > 1)
        url = arrurl[0] + "/GetdataVideo?" + arrurl[1];
    else
        url = arrurl[0] + "/GetdataVideo";
    pageIndex++;
    if (pgsize * pageIndex - pageCount < pgsize) {
        $("#loader").show();
        // $("#imgTop").html(pageIndex);
        if (url.match("gallery")) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadSuccessGallery,
                failure: function (response) {
                    // alert(response.d);
                },
                error: function (response) {
                    //                       alert("error");
                    //                           alert(response.d);
                }
            });
        }
        else if (url.match("tips")) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadSuccessTips,
                failure: function (response) {
                    // alert(response.d);
                },
                error: function (response) {
                    //                       alert("error");
                    //                           alert(response.d);
                }
            });
        }
        else if (url.match("video")) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + ', categoryId:' + categoryId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadSuccessVideo,
                failure: function (response) {
                    // alert(response.d);
                },
                error: function (response) {
                    //                       alert("error");
                    //                           alert(response.d);
                }
            });
        }
        else if (url.match("news")) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + ', categoryId:' + categoryId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadSuccessNews,
                failure: function (response) {
                    // alert(response.d);
                },
                error: function (response) {
                    //                       alert("error");
                    //                           alert(response.d);
                }
            });
        }
        else if (url.match("media")) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + ', categoryId:' + categoryId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadSuccessMedia(),
                failure: function (response) {
                    // alert(response.d);
                },
                error: function (response) {
                    //                       alert("error");
                    //                           alert(response.d);
                }
            });
        }
        else if (url.match("shop-design")) {
            $.ajax({
                type: "POST",
                url: url,
                data: '{pageIndex:' + pageIndex + ', pageSize:' + pgsize + ', categoryId:' + categoryId + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadSuccessShopDesign,
                failure: function (response) {
                    // alert(response.d);
                },
                error: function (response) {
                    //                       alert("error");
                    //                           alert(response.d);
                }
            });
        }
    }

    if (pageIndex * pgsize > pageCount) {
        $("#loadmoreVideo").hide();
    }
}
function LoadSuccessGallery(response) {
    var htmlGallery = response.d;
    var oldHTML = $('#lstGallery').html();
    $('#lstGallery').html(oldHTML + htmlGallery);
    $("#loader").hide();
    isLoading = 0;
    var pageindex = $('#nexthidVideoIndex').val();
    $('#hidVideoIndex').val(pageindex + ',' + $('.Gallery').length);
    $('#nexthidVideoIndex').remove();
    ResetHeightListVideo(false, 'gallery');
}
function LoadSuccessTips(response) {
    var htmlTips = response.d;
    var oldHTML = $('#lstTipCategory').html();
    $('#lstTipCategory').html(oldHTML + htmlTips);
    $("#loader").hide();
    isLoading = 0;
    var pageindex = $('#nexthidVideoIndex').val();
    $('#hidVideoIndex').val(pageindex + ',' + $('.dvTipCategory').length);
    $('#nexthidVideoIndex').remove();
    ResetHeightListVideo(false, 'tip');
}
function LoadSuccessVideo(response) {
    var htmlVideo = response.d;
    var oldHTML = $('#lstvideo').html();
    $('#lstvideo').html(oldHTML + htmlVideo);
    $("#loader").hide();
    isLoading = 0;
    ResetHeightListVideo(false, 'video');
}
function LoadSuccessMedia(response) {
    var htmlMedia = response.d;
    var oldHTML = $('#lstMedia').html();
    $('#lstMedia').html(oldHTML + htmlMedia);
    $("#loader").hide();
    isLoading = 0;
    ResetHeightListVideo(false, 'media');
}
function LoadSuccessNews(response) {
    var htmlNews = response.d;
    var oldHTML = $('.news-lst').html();
    $('.news-lst').html(oldHTML + htmlNews);
    $("#loader").hide();
    isLoading = 0;
}
function LoadSuccessShopDesign(response) {
    var htmlNews = response.d;
    var oldHTML = $('#shop-designlst').html();
    $('#shop-designlst').html(oldHTML + htmlNews);
    $("#loader").hide();
    isLoading = 0;
    var pageindex = $('#nexthidVideoIndex').val();
    $('#hidVideoIndex').val(pageindex + ',' + $('.dvShopDesign').length);
    $('#nexthidVideoIndex').remove();
    ResetHeightListVideo(false, 'shopdesign');
}
//End purchase, orderreview
fnClicktoTop = function () {
    $("#imgTop").click(function () {
        $('html, body').animate({ scrollTop: 0 }, 500, function () {
            // Animation complete.
        });
    });
}

/**************Submit Validate css bootstrap error cho control asp.net********************/
//check valid HTML5 cho button submit co data-btn="submit" dua vao cac control valid cua ASP.NET
$(':input[type=submit][data-btn=submit]').each(function () {
    $(this).on('click', function () {
        var val = Page_ClientValidate();
        var arrControlValid = "|";
        //check xem control valid nao bao loi va lay id input bi loi
        if (!val) {
            for (var i = 0; i < Page_Validators.length; i++) {
                var controlval = Page_Validators[i].controltovalidate;
                if (!Page_Validators[i].isvalid) {
                    if (arrControlValid.indexOf(controlval) > 0) {
                        arrControlValid += arrControlValid.replace(controlval + ":0|", controlval + ":1|");
                    }
                    else {
                        arrControlValid += controlval + ":1|";
                    }
                }
                else {
                    if (arrControlValid.indexOf(controlval) < 1) {
                        arrControlValid += controlval + ":0|";
                    }
                }
            }
            //thuc hien tach cac textbox bi loi ra de bat dau gan class error HTML5 cho textbox va div chua textbox do 
            //neu textbox dc check lai het error se remove class error day
            var arr = arrControlValid.split("|");
            var scroll = 0;
            var top = 0;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] != null && arr[i].indexOf(":") >= 0) {
                    var value = arr[i].split(":");
                    if (value[1] == 1) {
                        var field = $("#" + value[0]);
                        var control = field.parent();
                        if (control.hasClass("form-group")) {
                            control.addClass("has-error has-feedback");
                        }
                        else if (control.parent().hasClass("form-group")) {
                            control = control.parent();
                            control.addClass("has-error has-feedback");
                        }

                        var tagName = '';
                        if (document.getElementById(value[0]) != null) {
                            tagName = document.getElementById(value[0]).tagName;
                        }
                        if (tagName != "TABLE" && tagName != "SELECT" && (arr[i].toString().indexOf("tbItem") == -1) && (arr[i].toString().indexOf("txtItem") == -1)) {
                            var iconError = "<span id=spError_" + value[0] + " class='glyphicon glyphicon-remove form-control-feedback'></span>";
                            if (!document.getElementById("spError_" + value[0])) {
                                $(iconError).insertAfter(field);
                            }
                        }
                        if (field.offset() != null) {
                            var tmp = field.offset().top;

                            if (top == 0 || parseInt(tmp) < parseInt(top)) {
                                top = tmp;
                            }
                            scroll = 1;
                        }
                    }
                    else {
                        var field = $("#" + value[0]);
                        var control = field.parent();
                        if (control.hasClass("has-error has-feedback")) {
                            control.removeClass("has-error has-feedback");
                        }
                        else if (control.parent().hasClass("has-error has-feedback")) {
                            control = control.parent();
                            control.removeClass("has-error has-feedback");
                        }
                        //var spErr = control.find("#spError_" + value[0]);
                        $("#spError_" + value[0]).remove();
                    }
                }
            }
            if (scroll == 1 && parseInt(top) > 0) {
                $('html, body').animate({
                    scrollTop: top - 40
                }, 500);
            }
        }
        else {
            return val;
        }
    });
});

//sau khi postback trong updatepanel, goi lai ham bind de check valid

if (typeof (Sys) != "undefined") {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        bindInputCheckValid();
    });
}
//load bind khi load trang bthg
bindInputCheckValid();

//check valid HTML5 cho cac input sau khi roi khoi o input de remove hay bao loi~ ngay tren o
function bindInputCheckValid() {
    $(":input.form-control").bind('blur', function () {
        try {
            var val = Page_Validators;

            if (val != 'undefined' && val != null) {
                var haserror = 0;
                for (var i = 0; i < val.length; i++) {
                    var isValidPhone = CheckPhoneUSValid($("#" + this.id).val(), this.id);
                    if (val[i].controltovalidate == this.id && !val[i].isvalid && !isValidPhone) {
                        haserror = 1;
                        break;
                    }
                }
                if (haserror == 1) {

                    var control = $(this).parent();
                    if (control.hasClass("form-group")) {
                        control.addClass("has-error has-feedback");
                    }
                    else if (control.parent().hasClass("form-group")) {
                        control = control.parent();
                        control.addClass("has-error has-feedback");
                    }
                    var tagName = this.tagName;
                    if (tagName != "SELECT") {
                        var iconError = "<span id=spError_" + this.id + " class='glyphicon glyphicon-remove form-control-feedback'></span>";
                        if (!document.getElementById("spError_" + this.id)) {
                            $(iconError).insertAfter(this);
                        }
                    }
                }
                else if (haserror == 0) {
                    var control = $(this).parent();
                    if (control.hasClass("has-error has-feedback")) {
                        control.removeClass("has-error has-feedback");
                    }
                    else if (control.parent().hasClass("has-error has-feedback")) {
                        control = control.parent();
                        control.removeClass("has-error has-feedback");
                    }
                    $("#spError_" + this.id).remove();
                }
            }
        }
        catch (err)
    { }
    });
}
/**************Shop Save**************************/
function ResetHeightShopSave() {

    var all = $(".shopway .data").map(function () {
        var lstchild = $(this).children('.shop-save-item');
        ResetHeightList(lstchild, 'shopsave');
    });
}
function ResetHeightShopSaveSlider() {
    var allslide = $(".shopway .bxslider").map(function () {
        var lstchild = $(this).children('.shop-save-item');
        ResetHeightListSlider(lstchild);
    });
}
function ResetHeightListSlider(lstchild) {

    ResetHeightSingleList(lstchild);
}
function ResetHeightSingleList(lstchild) {

    var lstReset = new Array();
    var maxRowHeight = 0;
    var $el;
    $(lstchild).each(function () {
        $el = $(this);
        $($el).height('auto');
        if ($el.height() > maxRowHeight)
            maxRowHeight = $el.height();
        lstReset.push($el);
    })

    if (lstReset.length > 0) {
        for (var j = 0; j < lstReset.length; j++) {
            lstReset[j].height(maxRowHeight);
        }
    }
}
function ResetHeightRow(lstReset, maxRowHeight, type) {

    for (var j = 0; j < lstReset.length; j++) {
        lstReset[j].height(maxRowHeight);
    }
    if (type == 'shopsave' || type == 'free-sample' || type == 'main-cate' || type == 'free-gift' || type == 'infor-banner' || type == 'search-article') {
        lstReset[lstReset.length - 1].addClass('noneborderright');
    }

}


function ResetHeightList(lstchild, type) {
    var lstReset = new Array();
    var defaultTop = -1;
    var maxRowHeight = 0;
    var $el;

    $(lstchild).each(function () {
        $el = $(this);
        $($el).height('auto');

        if (type == 'shopsave' || type == 'free-sample' || type == 'main-cate' || type == 'free-gift' || type == 'infor-banner' || type == 'search-article') {
            $($el).removeClass("noneborderright")
        }
        if (type == 'infor-banner') {
            $(this).children(".name").height('auto');
            $(this).children(".desc").height('auto');
        }
        var position = $el.position();
        if (position.top != defaultTop) {

            defaultTop = position.top;
            if (lstReset.length > 0) {
                ResetHeightRow(lstReset, maxRowHeight, type);
                lstReset.length = 0;
                defaultTop = $el.position().top;
            }
            //else AddShopSaveBorderLeft($el)
            maxRowHeight = 0;
            if ($el.height() > maxRowHeight)
                maxRowHeight = $el.height();
            lstReset.push($el);
        }
        else {
            if ($el.height() > maxRowHeight)
                maxRowHeight = $el.height();
            lstReset.push($el);
        }
    })

    if (lstReset.length > 0) {
        ResetHeightRow(lstReset, maxRowHeight, type);
    }
}
function DelayResetHeightListVideo(resizeAll, type) {
    setTimeout(function () {
        ResetHeightListVideo(resizeAll, type);
    }, 500);
}
function ResetHeightListVideo(resizeAll, type) {
    var isCheckLoadFullWidthVideo = false;
    var isCheckLoadFullWidthVideoResult = false;
    var currentPage = window.location.href;
    if (currentPage.indexOf('search-result.aspx') >= 0) {
        if (isLoadFullWidthVideo) {
            isCheckLoadFullWidthVideo = true;
        }
    }

    var selectorItemClass = '';
    var divItemId = '';
    var selectorImageClass = '';
    if (type == 'video') {
        selectorItemClass = '#lstvideo .dvVideoItem';
        divItemId = 'videoItem_';
        selectorImageClass = '#lstvideo .dvVideoItem #imgVideo_';
    }
    else if (type == 'tabvideo') {
        selectorItemClass = '#tab_video .dvVideoItem';
        divItemId = 'videoItem_';
        selectorImageClass = '#tab_video .dvVideoItem #imgVideo_';
    }
    else if (type == 'gallery') {
        selectorItemClass = '#lstGallery .Gallery';
        divItemId = 'galleryItem_';
        selectorImageClass = '#lstGallery .Gallery #imgGallery_';
    }
    else if (type == 'media') {
        selectorItemClass = '#lstMedia .dvMediaItem';
        divItemId = 'mediaItem_';
        selectorImageClass = '#lstMedia .dvMediaItem #imgGallery_';
    }
    else if (type == 'shopdesign') {
        selectorItemClass = '#shop-designlst .dvShopDesign';
        divItemId = 'shopDesignItem_';
        selectorImageClass = '#shop-designlst .dvShopDesign #imgShopDesign_';
    }
    else if (type == 'tip') {
        selectorItemClass = '#lstTipCategory .dvTipCategory';
        divItemId = 'tipItem_';
        selectorImageClass = '';
    }
    var lstReset = new Array();
    var defaultTop = -1;
    var maxRowHeight = 0;
    var $el;
    var beginIndex = 0;
    var endIndex = 0;
    var heightImage = 0;
    if (resizeAll) {
        beginIndex = 1;
        var lastItem = $(selectorItemClass).last();
        endIndex = lastItem[0].id.replace(divItemId, '');
        endIndex = parseInt(endIndex);
    }
    else {
        if (!document.getElementById('hidVideoIndex')) {
            return;
        }
        var lstId = $('#hidVideoIndex').val();
        var arr = new Array();
        arr = lstId.split(',');
        if (arr.length > 0) {
            beginIndex = parseInt(arr[0].toString());
            try { endIndex = parseInt(arr[1].toString()); }
            catch (e) { }
            if (beginIndex > 1) { //page thu 2, can lay chieu cao cua image dau tien default cho tat ca image page hien tai
                heightImage = $(selectorImageClass + "1").height();
                if (heightImage < 100) {
                    heightImage = $(selectorImageClass + "2").height();
                }

            }
        }
    }
    var countRowItem1 = 0;
    var countRowItem2 = 0;
    for (var i = beginIndex; i <= endIndex; i++) {
        $el = $('#' + divItemId + i.toString());
        if (heightImage > 0) {
            $(selectorImageClass + i.toString()).height(heightImage);
        }
        else {
            $(selectorImageClass + i.toString()).height('auto');
        }
        $($el).height('auto');
        var position = $el.position();
        if (position.top != defaultTop) {

            defaultTop = position.top;
            if (lstReset.length > 1) {
                ResetHeightRow(lstReset, maxRowHeight, type);
                countRowItem1 = lstReset.length;
                lstReset.length = 0;
                defaultTop = $el.position().top;
            }
            else if (lstReset.length == 1) {
                lstReset.length = 0;
                defaultTop = $el.position().top;
            }
            maxRowHeight = 0;
            if ($el.height() > maxRowHeight)
                maxRowHeight = $el.height();
            lstReset.push($el);
        }
        else {
            if ($el.height() > maxRowHeight)
                maxRowHeight = $el.height();
            lstReset.push($el);
        }
    }
    if (lstReset.length > 1) {
        ResetHeightRow(lstReset, maxRowHeight, type);
        countRowItem2 = lstReset.length;
    }
    if (isCheckLoadFullWidthVideo) {

        var addCountItem = 0;
        var maxItemInRow = 0;
        if (countRowItem1 > countRowItem2) {
            maxItemInRow = countRowItem1;
            addCountItem = countRowItem1 - countRowItem2;
        }
        else if (countRowItem2 > countRowItem1) {
            maxItemInRow = countRowItem2;
            addCountItem = countRowItem2 - countRowItem1;
        }
        else {
            maxItemInRow = countRowItem1;
        }
        LoadFullWidthVideo(addCountItem, maxItemInRow, endIndex)

    }
    $("#lstvideo #hidVideoIndex").remove();
}


function ResetHeightListFreeGift(lstchild) {

    type = 'free-gift';
    var heightCart = 0;
    var lstReset = new Array();
    var defaultTop = -1;
    var maxRowHeight = 0;
    var $el;
    var tmpHeightCart = 0;
    $(lstchild).each(function () {
        $el = $(this);


        $($el).height('auto');
        $($el).removeClass("noneborderright")
        var position = $el.position();
        if (position.top != defaultTop) {
            tmpHeightCart = $(this).children(".selectGift").height();
            if (tmpHeightCart == null)
                tmpHeightCart = 0;
            defaultTop = position.top;
            if (lstReset.length > 0) {
                if (heightCart > 0) {
                    maxRowHeight = maxRowHeight + 70;
                    if (ViewPortVidth() <= 767) {
                        maxRowHeight = maxRowHeight + 12;
                    }
                }
                else {
                    maxRowHeight = maxRowHeight + 30;
                }
                ResetHeightRow(lstReset, maxRowHeight, type);
                tmpHeightCart = 0;
                lstReset.length = 0;
                defaultTop = $el.position().top;
            }
            //else AddShopSaveBorderLeft($el)
            maxRowHeight = 0;
            if ($el.height() > maxRowHeight) {
                maxRowHeight = $el.height();
            }
            if (heightCart == 0) {
                heightCart = tmpHeightCart;
            }
            lstReset.push($el);
        }
        else {
            if ($el.height() > maxRowHeight) {
                maxRowHeight = $el.height();
            }
            tmpHeightCart = $(this).children(".selectGift").height();
            if (tmpHeightCart == null)
                tmpHeightCart = 0;
            if (heightCart == 0) {
                heightCart = tmpHeightCart;
            }
            lstReset.push($el);
        }


    })
    if (lstReset.length > 0) {
        if (heightCart > 0) {
            maxRowHeight = maxRowHeight + 70;
            if (ViewPortVidth() <= 767) {
                maxRowHeight = maxRowHeight + 12;
            }
        }
        else {
            maxRowHeight = maxRowHeight + 30;
        }
        ResetHeightRow(lstReset, maxRowHeight, type);
        tmpHeightCart = 0;
    }
}
$(document).ready(function () {
    setupRotator();
    SetupBreadCrumb();
});

function setupRotator() {
    if ($('.pro-item').length > 1) {
        $('.pro-item:first').addClass('current').fadeIn(1000);
        setInterval('textRotate()', 3000);
    }
}
function textRotate() {
    var current = $('#quotes > .current');
    if (current.next().length == 0 && current.next().length < 2) {
        current.removeClass('current').fadeOut(500);
        $('.pro-item:first').addClass('current').fadeIn(500);
    }
    else {
        current.removeClass('current').fadeOut(500);
        current.next().addClass('current').fadeIn(500);
    }
}
function ToggleMobileMenu() {
    var width = $("#menu-mobile").width();
    if (ViewPortVidth() <= 767) {
        width -= 43;
    }
    else {
        width -= 46;
    }

    $("#nav-menu-mobile").css("width", width + 'px');
    $("#menu-mobile").css("height", jQuery(window).height() + 'px');
    $("#menu-mobile").toggle("slide");

    var ListDepartment = $('#ListDepartment').css('display');
    if (ListDepartment == 'block') {
        $("#ListDepartment").css("display", "none");
        $("#menu-dept").removeClass('open');
    }

    var breadcrumb = $('#bread-crumb-nav-content').css('display');
    if (breadcrumb == 'block') {
        $('#bread-crumb-nav-content').css('display', 'none');
        $('#bread-crumb-nav').next().css('display', 'block');
        $("#bread-crumb-nav .title > h1").css('display', 'block');
        $("#bread-crumb-nav > div.arrow").css('display', 'block');
    }

    //    if ($("#menu-mobile").hasClass('open')) {
    //        $("#menu-mobile").removeClass('open');
    //        $("#menu-mobile").css("display", "none");
    //    } else {
    //        
    //        //$("#menu-mobile").addClass('open');
    //        //$("#menu-mobile").css("display", "block");

    //    }
}


/**********Bread crumb**************/
function getInternetExplorerVersion() {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}


/**********Bread crumb**************/
function SetupBreadCrumb() {
    var ieVersion = getInternetExplorerVersion();
    if (ieVersion > -1 && ieVersion < 9)
        return;
    if (document.getElementById('customer-service')) {
        $("#bread-crumb-nav-content").html($("#customer-service").html());
    }
    else if (document.getElementById('shop-by-design')) {
        $("#bread-crumb-nav-content").html($("#shop-by-design").html());
    }
    else if (document.getElementById('resource-center')) {
        $("#bread-crumb-nav-content").html($("#resource-center").html());
    }

    SetPositionBreadCrumbNav();
    if (window.ViewPortVidth() > 991) {
        fnSetDefaultMenuCus();
    }
}
fnClicCusService = function () {
    $("#bread-crumb-nav").click(function () {
        if ($("#cus-service")) {
            if (window.ViewPortVidth() < 992) {

                $("#cus-service").html($("#left-page").html());
                //delete div ko can load ben cot trai, danh rieng cho cac trag video, news
                $("#categoryhide").hide();
                $("#cus-service #multimedia").html("");
                $("#cus-service #archives").html("");
                $("#cus-service #vpopular").html("");
                $("#cus-service #share_social").html("");
                $("#cus-service #likesocial").html("");
                // end

                $("#cus-service .left-nav #divCustomerServiceMenu").html("");
                $("#cus-service .left-nav #divResourceCenter").html("");
                $("#cus-service .left-nav #divShopByDesign").html("");
                $("#cus-service .left-nav #divMyAccountMenu").html("");
                if ($("#cus-service").css("display") == "none") {
                    $("#cus-service").css("display", "block");
                    ClickCusService = 1;
                } else {
                    fnSetDefaultMenuCus();
                }
            }
            fnSetContentCusService();
        }
    });
}

function SetPositionBreadCrumbNav() {
    if (document.getElementById('bread-crumb-nav-content')) {
        var pos = $("#bread-crumb-nav").position();
        var breadCrumbHeight = $('#bread-crumb-nav').height();
        if ($('#bread-crumb-nav-content').css('display') == 'none') {
            $("#bread-crumb-nav-content").css("top", (pos.top + breadCrumbHeight) + "px");
        }
        else {

            $("#bread-crumb-nav-content").css("top", (pos.top + breadCrumbHeight) + "px");
        }
    }
}
function ShowBreadCrumbMenu() {
    ToggleBreadCrumb();
    $("#bread-crumb-nav-content").slideToggle("fast", function () {
        //ToggleBreadCrumb();
    });
}
function ToggleBreadCrumb() {
    if ($('#bread-crumb-nav').next().css('display') == 'none') {
        $('#bread-crumb-nav').next().css('display', 'block');
        $("#bread-crumb-nav .title > h1").css('display', 'block');
        $("#bread-crumb-nav > div.arrow").css('display', 'block');
    }
    else {
        $('#bread-crumb-nav').next().css('display', 'none');
        $("#bread-crumb-nav .title > h1").css('display', 'none');
        $("#bread-crumb-nav > div.arrow").css('display', 'none');
    }
}
/**********Menu Archive News**************/
function ShowArchive() {
    if (ViewPortVidth() <= 991) {
        if (($("#archives").text() != "") && ($("#smcategory").text() == "")) {
            $('#smcategoryNews #smtitleNews').html($('#archives .title').html());
            $('#smcategory').html($('#archives').html());
            $('#smcategory,#smcategory .title').css('display', 'none');

            $('<span style="float: right;" class="glyphicon ic-minus ic-plus"></span>').insertAfter('#smcategoryNews #smtitleNews');
            $('#contentnews-left').show();
            $('#smcategory #vpopular').html('');
            $('#smcategory #categoryhide').html('');
            fnCollapseCategory();
        }
    }
}
fnCollapseCategory = function () {
    $(".ic-minus").addClass("ic-plus");
    $(".ic-minus").click(function () {
        if ($(".ic-minus").hasClass("ic-plus")) {
            $(".ic-minus").removeClass("ic-plus");
            $('#smcategory').css("border-top", "1px solid #ccc");
        }
        else {
            $(".ic-minus").addClass("ic-plus");
        }
        $("#smcategory").slideToggle('fast');
    });
}
function ShowBoxShareNews() {
    if (ViewPortVidth() <= 991) {
        $('#dvshare').html($('#left-page #share_social').html());
        $('#dvshare').show();
    }
    else {
        $('#dvshare').hide();
    }
}
function ShowTabOfNews() {
    if (ViewPortVidth() <= 767) {
        if (document.getElementById('newsdetail')) {
            $('#tabbox nav a').removeClass('selected');
            $('#tabnav_comment').addClass('vitem');
            $('#tabnav_comment').attr('onclick', '').unbind('click');
            $('#stab_comment').removeClass('hide');
            $('#sh_comment').attr('onclick', '').unbind('click');
            if ($('#tab_comment').html() != "") {
                $('#stab_comment').html($('#tab_comment').html());
            }
            $('#tabnav_comment').click(function () {
                $('#stab_comment').slideToggle(function () {
                    if ($('#stab_comment').css('display') != 'none') {
                        $('#tabnav_comment .arrow').css('transform', 'rotate(90deg)');
                    }
                    else {
                        $('#tabnav_comment .arrow').css('transform', 'none');
                    }
                });
            });
        }
    }
    else if (document.getElementById('newsdetail')) {
        $('#tabnav_comment').removeClass('vitem');
        $('#tabnav_comment').attr('onclick', 'changePackageTab(comment)').bind('click');
        $('#sh_comment').attr('onclick', 'changePackageTab(comment)').bind('click');
        $('#stab_comment').addClass('hide');
        $('#tabnav_comment').addClass('selected');
    }
}
/******Show Tab Video**********/
function ShowTabOfVideo() {
    if (ViewPortVidth() <= 767) {
        if (document.getElementById('video-detail')) {
            $('#tabbox nav a').removeClass('selected');

            var arrname = ["comment", "item", "embed", "video"];
            for (var i = 0; i < arrname.length; i++) {
                $('#tabnav_' + arrname[i]).addClass('vitem');
                $('#tabnav_' + arrname[i]).attr('onclick', '').unbind('click');
                $('#stab_' + arrname[i]).removeClass('hide');
                $('#sh_' + arrname[i]).attr('onclick', '').unbind('click');
            }
            if ($('#tab_comment').html() != "") {
                $('#stab_comment').html($('#tab_comment').html());
            }

            $('#tabnav_comment').click(function () {
                $('#stab_comment').slideToggle(function () {
                    if ($('#stab_comment').css('display') != 'none') {
                        $('#tabnav_comment .arrow').css('transform', 'rotate(90deg)');
                    }
                    else {
                        $('#tabnav_comment .arrow').css('transform', 'none');
                    }
                });
            });
            if ($('#tab_item').html() != "") {
                $('#stab_item').html($('#tab_item').html());
            }
            $('#stab_item').css('display', 'none');
            $('#tabnav_item').click(function () {
                $('#stab_item').slideToggle(function () {

                    equalheighta('#list-item .item');
                    if ($('#stab_item').css('display') != 'none') {
                        $('#tabnav_item .arrow').css('transform', 'rotate(90deg)');
                    }
                    else {
                        $('#tabnav_item .arrow').css('transform', 'none');
                    }
                });
            });

            $('#stab_video').html($('#tab_video').html());
            $('#tabnav_video').click(function () {
                $('#stab_video').slideToggle(function () {
                    if ($('#stab_video').css('display') != 'none') {
                        $('#tabnav_video .arrow').css('transform', 'rotate(90deg)');
                    }
                    else {
                        $('#tabnav_video .arrow').css('transform', 'none');
                    }
                });
            });

            $('#stab_embed').html($('#tab_embed').html());
            $('#tabnav_embed,#sh_embed').click(function () {
                $('#stab_embed').slideToggle(function () {
                    if ($('#stab_embed').css('display') != 'none') {
                        $('#tabnav_embed .arrow').css('transform', 'rotate(90deg)');
                    }
                    else {
                        $('#tabnav_embed .arrow').css('transform', 'none');
                    }
                });
            });

        }

    }
    else if (document.getElementById('video-detail')) {
        if ($('#stab_item').html() != "") {
            $('#tab_item').html($('#stab_item').html());
            $('#stab_item').html("");
        }
        var arrname = ["comment", "item", "embed", "video"];
        $('#tabbox nav a').removeClass('selected');
        for (var i = 0; i < arrname.length; i++) {
            $('#tabnav_' + arrname[i]).removeClass('vitem');
            $('#tabnav_' + arrname[i]).attr('onclick', 'changePackageTab("' + arrname[i] + '")').bind('click');
            $('#sh_' + arrname[i]).attr('onclick', 'changePackageTab("' + arrname[i] + '")').bind('click');
            $('#stab_' + arrname[i]).addClass('hide');
            if ($('#tab_' + arrname[i]).css('display') == 'block') {
                $('#tabnav_' + arrname[i]).addClass('selected');
            }

        }
    }
}
/*******End show tab video********/
function viewport() {
    var e = window, a = 'inner';
    if (!('innerWidth' in window)) {
        a = 'client';
        e = document.documentElement || document.body;
    }
    return { width: e[a + 'Width'], height: e[a + 'Height'] }
}
function ViewPortVidth() {
    return viewport().width;
}
//gan event cho cac checkbox khi browser la IE 6 7 8 ko support CSS3 de change checkbox
if (getInternetExplorerVersion() < 9 && getInternetExplorerVersion() != -1) {
    $('.checkbox input[type="checkbox"]').each(function () {
        var checkbox = $(this);
        var label = $(this).parent();
        $(label).click(function () {
            if ($(checkbox).is(":checked")) {
                $(label).find("span.checkbox-icon").removeClass("checked");
                $(checkbox).attr("checked", false);
            }
            else {
                $(label).find("span.checkbox-icon").addClass("checked");
                $(checkbox).attr("checked", true);
            }
        });

    });
}

fnSetContentCusService = function () {

    if ($("#customer-service")) {

        if (window.ViewPortVidth() < 992) {
            if ($("#customer-service")) {
                if ($("#cus-service").html() != "") {
                    $("#cus-service").css("display", "block");
                    $(".row-offcanvas-left").css("left", "245px");
                } else {
                    fnSetDefaultMenuCus();
                }
                //            $("#cus-service").html($("#left-page").html());
                //            $("#cus-service .left-nav #divCustomerServiceMenu").html("");
            }
        }
        else {
            if ($("#formmain").hasClass("active")) {
                $("#formmain").removeClass("active");
            }
            fnSetDefaultMenuCus();
        }
    }
}
fnClickOffCusService = function () {
    if (window.ViewPortVidth() < 992) {

        $("#content-page,#wrapper,footer").click(function () {
            if ($("#cus-service").css("display") == "block" && ClickCusService == 0) {
                fnSetDefaultMenuCus();
            }
            if (ClickCusService == 1) {
                ClickCusService = 0;
            }
        });

    }

}
var ClickCusService = 0,
    ClickCart = 0;
fnSetDefaultMenuCus = function () {
    if ($("#cus-service")) {
        $("#cus-service").html("");
        $("#cus-service").css("display", "none");
        $(".row-offcanvas-left").css("left", "0");
    }
}
function getRows(selector) {
    var height = $(selector).height();
    var line_height = $(selector).css('line-height');
    line_height = parseFloat(line_height)
    var rows = height / line_height;
    return Math.round(rows);
}
fnSetheightverlineDealsCenter = function (isResize) {
    //var browser = navigator.userAgent.toLowerCase();
    if ($("#menu .deals-center")) {
        $("#menu .deals-center > .ver-line").css("height", "0");
        // $("#menu .submenu.deals-center .popover-content ul.shopnow li, #main-shop-save ul li").css("border-left", "none");
        var hverline = $("#menu .deals-center").height(),
            minusheight = 15;
        if (window.ViewPortVidth() < 842) {
            minusheight = 20;
        }
        $("#menu .deals-center > .ver-line").css("height", hverline - minusheight);
        $("#menu .submenu.deals-center .popover-content ul.shopnow li, #main-shop-save ul li").css("border-left", "solid 1px #c7c8c8");
        //$("#menu .submenu.deals-center .popover-content ul.shopnow li").css("border-left", "solid 1px #c7c8c8");
        if ($("#menu .shopnow li:last-child div").hasClass("see-more")) {
            $("#menu .submenu.deals-center .popover-content ul.shopnow li:nth-last-child(2)").css("border-right", "solid 1px #c7c8c8");
            $("#menu .submenu.deals-center .popover-content ul.shopnow li:last-child").css("border-left", "none");
        }
    }
    if ($("#main-shop-save")) {
        var browser = navigator.userAgent.toLowerCase();
        $("#main-shop-save > .ver-line").css("height", 0);
        var hverline = $("#main").outerHeight();
        if (isResize == 1) {
            if (browser.indexOf(".net") == -1 && ViewPortVidth() > 991) {
                $("#main-shop-save > .ver-line").css("height", hverline + 55);
            }
            else
                $("#main-shop-save > .ver-line").css("height", hverline + 75);

        }
        else
            $("#main-shop-save > .ver-line").css("height", hverline);

        if (ViewPortVidth() <= 540) {
            var hdiv = $("#main-shop-save ul li > div").height();
            $("#main-shop-save ul li div .ver-line").css('top', hdiv - 14);
        }
        else
            $("#main-shop-save ul li div .ver-line").removeAttr("style");

    }

}

/*******popup free item MixMatch**********************/
function ResetHeaderHeightPopupReviseFreeItem() {
    var h = $('#promotion-main .header').outerHeight();
    $('#promotion-main .header').css('height', h + 'px');
}

function FixPagePopupReviseFreeItem() {
    var itemposition = $("#promotion-main .header").position().top;
    if ($(window).scrollTop() >= itemposition) {
        FixHeaderPopupReviseFreeItem();
    }
    else {
        ClearFixHeaderPopupReviseFreeItem();
    }

}
function ClearFixHeaderPopupReviseFreeItem() {

    $('#promotion-main .header .pro-header-wrapper').removeAttr('style')
    $("#promotion-main .header .pro-header-wrapper").removeClass("header-fix");
    // $('#promotion-main .header').css('height', h + 'px');
    $('#promotion-main .header').height('auto');
}
function FixHeaderPopupReviseFreeItem() {
    ResetHeaderHeightPopupReviseFreeItem();
    var Width = $("#promotion-main .header .pro-header-wrapper").outerWidth();
    $("#promotion-main .header .pro-header-wrapper").css('width', Width + 'px');
    $("#promotion-main .header .pro-header-wrapper").addClass("header-fix");

}

function isFixHeaderPopupReviseFreeItem() {

    var display = $(".free-item-pop .header-fix").css('display');
    if (display == 'none')
        return false;
    return true;

}


function ShowErrorPopupReviseFreeItem(msg) {
    $('#spanError').remove();
    $('#divError').html(msg)
    $('#divError').css('display', 'block');

}

function gotoLinkPopupReviseFreeItem(url) {
    var parentURL = parent.window.location.href;
    if (parentURL.indexOf('/store/revise-cart.aspx') > 0) {
        return;
    }
    parent.window.location = url;
}

/*------------------- KHOA FUNCTIONS | PRODUCT LIST--------------------*/
setsortby = function (isHot, isNew, isBestSeller) {
    try {
        if (!isHot)
            $("#ddlSortBy option[value='hot-items']").remove();
    }
    catch (err) { }

    try {
        if (!isNew)
            $("#ddlSortBy option[value='new-items']").remove();
    }
    catch (err) { }

    try {
        if (!isBestSeller)
            $("#ddlSortBy option[value='best-sellers']").remove();
    }
    catch (err) { }
}

ChangeParam = function (key, value, action) {
    var arr = [];
    $.each(arrParam.split(","), function (idx, val) {
        var sar = val.split(":");
        var str = sar[0] + ":";

        if (sar[0].indexOf(key) >= 0) {
            if (action == 'next') {
                str += parseInt(sar[1]) + 1;
            }
            else if (action == 'previous') {
                str += parseInt(sar[1]) - 1;
            }
            else if (action == 'pageSize') {
                str += ((parseInt(sar[1] / value) * value) + value);
            }
            else {
                str += value;
            }
        }
        else {
            str += sar[1];
        }
        arr.push(str);
    });
    arrParam = arr.join(", ");
}

GetParam = function (key) {
    var str;
    $.each(arrParam.split(","), function (idx, val) {
        var sar = val.split(":");

        if (sar[0].indexOf(key) >= 0) {
            str = sar[1];
            return false;
        }
    });

    return str;
}
function showAllBrand() {
    if (document.getElementById('more-brand')) {
        var brand = document.getElementById('more-brand');
        var show = document.getElementById('show-brand');
        brand.style.display = 'block';
        show.style.display = 'none';
        $(".dvmore").css('display', 'none');
    }

}
function showAllCategory() {
    if (document.getElementById('more-category')) {
        var brand = document.getElementById('more-category');
        var show = document.getElementById('show-category');
        brand.style.display = 'block';
        show.style.display = 'none';
        $(".dvmoreCategory").css('display', 'none');
    }

}

function ShowMoreBrand(idList) {
    if (idList != '') {
        var arr = new Array();
        arr = idList.split(',');
        if (arr.length > 0) {
            var id = '';
            for (var i = 0; i < arr.length; i++) {
                id = arr[i].toString();
                if (id != '') {
                    if (document.getElementById('liBrand_' + id)) {
                        document.getElementById('liBrand_' + id).style.display = '';
                    }
                }
            }
            document.getElementById('liMoreBrand').style.display = 'none';
        }
    }
}
function ShowBrand(id) {
    if (document.getElementById('departmentBrandL_' + id)) {
        if (document.getElementById('departmentBrandL_' + id).style.display == 'none')
            document.getElementById('departmentBrandL_' + id).style.display = '';
        else document.getElementById('departmentBrandL_' + id).style.display = 'none';
    }
}
fnFormatPhoneUS = function (container, e, textId) {
    // var textId = container.toString().replace('#txt', '');
    var arrowkey = '';
    try {
        switch (e.which) {
            case 16: // shift
                arrowkey = 'shift';
                breakk;
            case 35: // left
                arrowkey = 'end';
                break;
            case 36: // left
                arrowkey = 'home';
                break;
            case 37: // left
                arrowkey = 'left';
                break;

            case 38: // up
                arrowkey = 'up';
                break;

            case 39: // right
                arrowkey = 'right';
                break;

            case 40: // down
                arrowkey = 'down';
                break;

            default: arrowkey = ''; // exit this handler for other keys
        }
        e.preventDefault(); // prevent the default action (scroll / move caret)
    }
    catch (err) { }
    if (arrowkey == '') {
       
        var val = $.trim($(container).val());
        val = fnPhoneUS(val, textId);
        $(container).val(val);
        //        if (val.length > 0)
        //        Page_ClientValidate("grPhone");
        //        var phoneus = '';
        //        var txt = $.trim($(container).val());
        //        txt = fnformatPhone(txt);
        //        lchars = txt.length;
        //        var phone1 = '',
        //             phone2 = '',
        //             phone3 = '',
        //             phoneEx = '';
        //        if (lchars > 12) {//phone length > 12, process ext phone
        //            txt = fnReplaceCharPhone(txt, '');

        //            if (txt.indexOf(' ') > 0) {

        //                phoneEx = txt.substring(12, 17);
        //                if (phoneus == '' && txt.indexOf('-') > 0) {
        //                    var arr = txt.split(' ');
        //                    phoneus = arr[0];
        //                }
        //                $(container).val(phoneus + phoneEx);
        //            }
        //            else {
        //                if (txt.indexOf(' ') > 0 && txt.indexOf('-') > 0) {
        //                    $(container).attr('maxlenght', 16);
        //                }
        //                if (txt.indexOf('-') != -1)
        //                    phoneEx = txt.substring(12, 16);
        //                else
        //                    phoneEx = txt.substring(10, 14);

        //                txt = txt.replace(/\-/g, '');
        //                phone1 = txt.substring(0, 3);
        //                phone2 = txt.substring(3, 6);
        //                phone3 = txt.substring(6, 10);
        //                phoneus = phone1 + '-' + phone2 + '-' + phone3;
        //                $(container).val(phoneus + ' ' + phoneEx);
        //            }

        //        }
        //        else {
        //            if (lchars >= 10 && txt.indexOf('-') == -1) {//phone length == 10 --> xxx-xxx-xxx
        //                txt = fnReplaceCharPhone(txt, '');
        //                phone1 = txt.substring(0, 3);
        //                phone2 = txt.substring(3, 6);
        //                phone3 = txt.substring(6, 10);
        //                phoneus = phone1 + '-' + phone2 + '-' + phone3;
        //                $(container).val(phoneus);
        //            }
        //            else {//replace special char
        //                txt = fnReplaceCharPhone(txt, '-');
        //                $(container).val(txt);
        //            }
        //        }

    }
}
fnPhoneUS = function (val, textId) {
    var phoneus = '';
    var txt = val;
    txt = fnformatPhone(txt);
    lchars = txt.length;
    var phone1 = '',
             phone2 = '',
             phone3 = '',
             phoneEx = '';
    if (lchars > 12) {//phone length > 12, process ext phone
        txt = fnReplaceCharPhone(txt, '');
        if (txt.indexOf(' ') > 0) {
            phoneEx = txt.substring(12, 17);
            if (phoneus == '' && txt.indexOf('-') > 0) {
                var arr = txt.split(' ');
                phoneus = arr[0];
            }
            //$(container).val(phoneus + phoneEx);
            return phoneus + phoneEx;
        }
        else {
            //            if (txt.indexOf(' ') > 0 && txt.indexOf('-') > 0) {
            //               $(container).attr('maxlenght', 16);
            //            }
            if (txt.indexOf('-') != -1)
                phoneEx = txt.substring(12, 16);
            else
                phoneEx = txt.substring(10, 14);

            txt = txt.replace(/\-/g, '');
            phone1 = txt.substring(0, 3);
            phone2 = txt.substring(3, 6);
            phone3 = txt.substring(6, 10);
            phoneus = phone1 + '-' + phone2 + '-' + phone3;
            //$(container).val(phoneus + ' ' + phoneEx);
            return phoneus + ' ' + phoneEx;
        }

    }
    else {
        if (lchars >= 10 && txt.indexOf('-') == -1) {//phone length == 10 --> xxx-xxx-xxx
            txt = fnReplaceCharPhone(txt, '');
            phone1 = txt.substring(0, 3);
            phone2 = txt.substring(3, 6);
            phone3 = txt.substring(6, 10);
            phoneus = phone1 + '-' + phone2 + '-' + phone3;
            //$(container).val(phoneus);
            textId = textId.replace('txt', '');
            if (textId == 'BillingPhone' || textId == 'ShippingPhone') {
                //$('#' + textId + " .text-danger").css("display", "none");
                //$('#spErrortxt' + textId).addClass('aaa');
                $('#' + textId + " span").css("display", "none");
                //$('#' + textId).removeClass("has-error has-feedback");
                //$('#' + textId).removeClass("has-feedback");
                //$('#' + textId + ' input').addClass("no-border-phone");
                //$('#' + textId + ' label span').css("display", "block");
            }

            return phoneus;
        }
        else {//replace special char
            txt = fnReplaceCharPhone(txt, '-');
            // $(container).val(txt);
            return txt;
        }
    }

}
fnReplaceCharPhone = function (str, schar) {
    str = str.replace(/\-/g, schar).replace(/\s/g, schar).replace(/\./g, schar).replace(/\_/g, schar).replace(/\,/g, schar).replace(/\;/g, schar).replace(/\//g, schar).replace(/\\/g, schar);
    return str;
}
//process when copy paste phon1
fnformatPhone = function (str) {
    var bool = false;
    if (str.indexOf('(') == 0 && str.indexOf(')') == 6)//(1-212) 555-1212 --> xxx-xxx-xxxx
        str = str.replace('(1-', '').replace(/\(/g, '').replace(/\)/g, '').replace(/\ /g, '-');

    if (str.indexOf('(') == 0 && str.indexOf(')') == 4)//(xxx) xxx-xxxx -> xxx-xxx-xxxx
        str = str.replace(/\(/g, '').replace(/\)/g, '').replace(/\ /g, '-');
    if (str.length > 10 && str.substring(3, 4) == '-' && str.substring(7, 8) == '-')// format true xxx-xxx-xxxx
        bool = true;

    if (str.indexOf('1.') == 0 || str.indexOf('+1') == 0)// replace country code
        str = str.replace('1.', '').replace('+1', '');

    if (bool == false && str.length > 10)//format false
        str = str.replace(/\-/g, '').replace(/\ /g, '').replace(/\//g, '').replace(/\\/g, '').replace(/\_/g, '');
    return str;
}

fnSelectCountryCode = function (container) {
    var id = getUrlParameter('id'),
             firstDropVal = $('#' + container).val();
    act = getUrlParameter('act'),
             URL = window.location.toString(),
             param = '';
    if (act == 'guest')
        param = '&act=' + act;
    else if ($.isNumeric(id)) {
        param = '&id=' + id;
    }



    if (URL.indexOf('store/billing.aspx') != -1 || URL.indexOf('store/billingint.aspx') != -1) {
        if (firstDropVal == "US" && URL.indexOf('store/billingint.aspx') != -1) {
            param = param + '&ctr=US';
            //window.location.href = '/store/billing.aspx?type=Billing&id=' + id + "&ctr=US" + prACT;
            window.location.href = '/store/billing.aspx?type=Billing' + param;
        }
        if (firstDropVal != "US" && URL.indexOf('store/billing.aspx') != -1) {
            param = param + "&ctr=" + firstDropVal;
            // window.location.href = '/store/billingint.aspx?type=Billing&id=' + id + "&ctr=" + firstDropVal + prACT;
            window.location.href = '/store/billingint.aspx?type=Billing' + param;
        }

    }
}
function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}
function SiteSeal(img, type) {
    if (window.location.protocol.toLowerCase() == "https:") { var mode = "https:"; } else { var mode = "http:"; }
    var host = location.host;
    var baseURL = mode + "//seals.networksolutions.com/siteseal_seek/siteseal?v_shortname=" + type + "&v_querytype=W&v_search=" + host + "&x=5&y=5";
    document.write('<a href="#" onClick=\'window.open("' + baseURL + '","' + type + '","width=450,height=500,toolbar=no,location=no,directories=no,\
status=no,menubar=no,scrollbars=no,copyhistory=no,resizable=no");return false;\'>\
<img src="' + img + '" style="border:none;" oncontextmenu="alert(\'This SiteSeal is protected\');return false;"></a>');
}
