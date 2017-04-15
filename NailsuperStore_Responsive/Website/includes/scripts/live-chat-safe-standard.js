var psPr0esid="orncV2WgtBea";var psPr0eiso;try{psPr0eiso=(opener!=null)&&(typeof(opener.name)!="unknown")&&(opener.psPr0ewid!=null);}catch(e){psPr0eiso=false;}
if(psPr0eiso){window.psPr0ewid=opener.psPr0ewid+1;psPr0esid=psPr0esid+"_"+window.psPr0ewid;}else{window.psPr0ewid=1;}
function psPr0en(){return(new Date()).getTime();}
var psPr0es=psPr0en();function psPr0est(f,t){if((psPr0en()-psPr0es)<7200000){return setTimeout(f,t*1000);}else{return null;}}
var psPr0eol=false;function psPr0eow(){if(psPr0eol||(1==1)){var pswo="menubar=0,location=0,scrollbars=auto,resizable=1,status=0,width=650,height=680";var pswn="pscw_"+psPr0en();var url="http://messenger.providesupport.com/messenger/nailsuperstore.html?ps_s="+psPr0esid;if(true&&!false){window.open(url,pswn,pswo);}else{var w=window.open("",pswn,pswo);try{w.document.body.innerHTML+='<form id="pscf" action="http://messenger.providesupport.com/messenger/nailsuperstore.html" method="post" target="'+pswn+'" style="display:none"><input type="hidden" name="ps_s" value="'+psPr0esid+'"></form>';w.document.getElementById("pscf").submit();}catch(e){w.location.href=url;}}}else if(1==2){document.location="http\u003a\u002f\u002f";}}
var psPr0eil;var psPr0eit;function psPr0epi(){var il;if(2==2){il=window.pageXOffset+50;}else if(2==3){il=(window.innerWidth*50/100)+window.pageXOffset;}else{il=50;}
il-=(266/2);var it;if(2==2){it=window.pageYOffset+50;}else if(2==3){it=(window.innerHeight*50/100)+window.pageYOffset;}else{it=50;}
it-=(173/2);if((il!=psPr0eil)||(it!=psPr0eit)){psPr0eil=il;psPr0eit=it;var d=document.getElementById('ciPr0e');if(d!=null){d.style.left=Math.round(psPr0eil)+"px";d.style.top=Math.round(psPr0eit)+"px";}}
setTimeout("psPr0epi()",100);}
var psPr0elc=0;function psPr0esi(t){window.onscroll=psPr0epi;window.onresize=psPr0epi;psPr0epi();psPr0elc=0;var url="https://messenger.providesupport.com/"+((t==2)?"auto":"chat")+"-invitation/nailsuperstore.html?ps_s="+psPr0esid+"&ps_t="+psPr0en()+"";var d=document.getElementById('ciPr0e');if(d!=null){d.innerHTML='<iframe allowtransparency="true" style="background:transparent;width:266;height:173" src="'+url+'" onload="psPr0eld()" frameborder="no" width="266" height="173" scrolling="no"></iframe>';}}
function psPr0eld(){if(psPr0elc==1){var d=document.getElementById('ciPr0e');if(d!=null){d.innerHTML="";}}
psPr0elc++;}
if (false) {
    psPr0esi(1);
}

var psPr0ed = document.getElementById('scPr0e');
if (psPr0ed != null) {
    if (psPr0eol || (1 == 1) || (1 == 2)) {
        var ctt = "";
        if (ctt != "") {
            tt = 'alt="' + ctt + '" title="' + ctt + '"';
        }
        else {
            tt = '';
        }

        if (false) {
            var p1 = '<table style="display:inline;border:0px;border-collapse:collapse;border-spacing:0;"><tr><td style="padding:0px;text-align:center;border:0px;vertical-align:middle"><a href="#" onclick="psPr0eow(); return false;"><img name="psPr0eimage" class="ic-chat" src="/includes/theme/images/chat_offline.jpg"  style="border:0;display:block;margin:auto"'; var p2 = '<td style="padding:0px;text-align:center;border:0px;vertical-align:middle"><a href="http://www.providesupport.com/pb/nailsuperstore" target="_blank"><img src="https://image.providesupport.com/'; var p3 = 'style="border:0;display:block;margin:auto"></a></td></tr></table>'; if ((0 >= 140) || (0 >= 0)) { psPr0ed.innerHTML = p1 + tt + '></a></td></tr><tr>' + p2 + 'lcbpsh.gif" width="140" height="17"' + p3; } else {
                psPr0ed.innerHTML = p1 + tt + '></a></td>' + p2 + 'lcbpsv.gif" width="17" height="140"' + p3; 
        }
    } 
    else {
        psPr0ed.innerHTML = '<a href="#" onclick="psPr0eow(); return false;"><img name="psPr0eimage" src="/includes/theme/images/chat_offline.jpg"  border="0"' + tt + '></a>'; 
    } 
}
else {
    psPr0ed.innerHTML = '';
}

}

var psPr0eop = false;
function psPr0eco() {
    var w1 = psPr0eci.width - 1;
    psPr0eol = (w1 & 1) != 0;

    psPr0esb(psPr0eol ? "/includes/theme/images/chat_online.jpg" : "/includes/theme/images/chat_offline.jpg");
    psPr0escf((w1 & 2) != 0);

    var h = psPr0eci.height;
    if (h == 1) { psPr0eop = false; }
    else if ((h == 2) && (!psPr0eop))
    { psPr0eop = true; psPr0esi(1); }
    else if ((h == 3) && (!psPr0eop))
    { psPr0eop = true; psPr0esi(2); } 
}

var psPr0eci = new Image();
psPr0eci.onload = psPr0eco;
var psPr0epm = false; 
var psPr0ecp = psPr0epm ? 30 : 60;
var psPr0ect = null;

function psPr0escf(p) {
    if (psPr0epm != p) {
        psPr0epm = p;
        psPr0ecp = psPr0epm ? 30 : 60;

        if (psPr0ect != null) {
            clearTimeout(psPr0ect);
            psPr0ect = null;
        }

        psPr0ect = psPr0est("psPr0erc()", psPr0ecp);
    }
}

function psPr0erc() {
    psPr0ect = psPr0est("psPr0erc()", psPr0ecp);
    try {
        psPr0eci.src = "https://image.providesupport.com/cmd/nailsuperstore?" + "ps_t=" + psPr0en() + "&ps_l=" + escape(document.location) + "&ps_r=" + escape(document.referrer) + "&ps_s=" + psPr0esid + "" + "";
    }
    catch (e)
    { }
}

psPr0erc();
var psPr0ecb = "/includes/theme/images/chat_offline.jpg";
function psPr0esb(b) {
    if (psPr0ecb != b) {
        var i = document.images['psPr0eimage'];
        if (i != null) {
            i.src = b;
        }
        psPr0ecb = b;
        

        var psPr0ed2 = document.getElementById('scPr0e2');
        if (psPr0ed2 != null) {
            psPr0ed2.innerHTML = psPr0ed.innerHTML.replace('chat_online.jpg', 'chat_online2.jpg');
        }
    }
}