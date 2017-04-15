var psHwEbsid="orncV2WgtBea";var psHwEbiso;try{psHwEbiso=(opener!=null)&&(typeof(opener.name)!="unknown")&&(opener.psHwEbwid!=null);}catch(e){psHwEbiso=false;}
if(psHwEbiso){window.psHwEbwid=opener.psHwEbwid+1;psHwEbsid=psHwEbsid+"_"+window.psHwEbwid;}else{window.psHwEbwid=1;}
function psHwEbn(){return(new Date()).getTime();}
var psHwEbs=psHwEbn();function psHwEbst(f,t){if((psHwEbn()-psHwEbs)<7200000){return setTimeout(f,t*1000);}else{return null;}}
var psHwEbol=false;function psHwEbow(){if(psHwEbol||(1==1)){var pswo="menubar=0,location=0,scrollbars=auto,resizable=1,status=0,width=650,height=680";var pswn="pscw_"+psHwEbn();var url="http://messenger.providesupport.com/messenger/nailsuperstore.html?ps_s="+psHwEbsid;if(true&&!false){window.open(url,pswn,pswo);}else{var w=window.open("",pswn,pswo);try{w.document.body.innerHTML+='<form id="pscf" action="http://messenger.providesupport.com/messenger/nailsuperstore.html" method="post" target="'+pswn+'" style="display:none"><input type="hidden" name="ps_s" value="'+psHwEbsid+'"></form>';w.document.getElementById("pscf").submit();}catch(e){w.location.href=url;}}}else if(1==2){document.location="http\u003a\u002f\u002f";}}
var psHwEbil;var psHwEbit;function psHwEbpi(){var il;if(2==2){il=window.pageXOffset+50;}else if(2==3){il=(window.innerWidth*50/100)+window.pageXOffset;}else{il=50;}
il-=(266/2);var it;if(2==2){it=window.pageYOffset+50;}else if(2==3){it=(window.innerHeight*50/100)+window.pageYOffset;}else{it=50;}
it-=(173/2);if((il!=psHwEbil)||(it!=psHwEbit)){psHwEbil=il;psHwEbit=it;var d=document.getElementById('ciHwEb');if(d!=null){d.style.left=Math.round(psHwEbil)+"px";d.style.top=Math.round(psHwEbit)+"px";}}
setTimeout("psHwEbpi()",100);}
var psHwEblc=0;function psHwEbsi(t){window.onscroll=psHwEbpi;window.onresize=psHwEbpi;psHwEbpi();psHwEblc=0;var url="https://messenger.providesupport.com/"+((t==2)?"auto":"chat")+"-invitation/nailsuperstore.html?ps_s="+psHwEbsid+"&ps_t="+psHwEbn()+"";var d=document.getElementById('ciHwEb');if(d!=null){d.innerHTML='<iframe allowtransparency="true" style="background:transparent;width:266;height:173" src="'+url+'" onload="psHwEbld()" frameborder="no" width="266" height="173" scrolling="no"></iframe>';}}
function psHwEbld(){if(psHwEblc==1){var d=document.getElementById('ciHwEb');if(d!=null){d.innerHTML="";}}
psHwEblc++;}
if(false){psHwEbsi(1);}
var psHwEbd = document.getElementById('scHwEb'); if (psHwEbd != null) { if (psHwEbol || (1 == 1) || (1 == 2)) { if (false) { psHwEbd.innerHTML = '<table style="display:inline;border:0px;border-collapse:collapse;border-spacing:0;"><tr><td style="padding:0px;text-align:center;border:0px"><a href="#" onclick="psHwEbow(); return false;"><span id="psHwEbl">' + 'Leave Chat Online' + '</span></a></td></tr><tr><td style="padding:0px;text-align:center;border:0px"><a href="http://www.providesupport.com/pb/nailsuperstore" target="_blank"><img src="https://image.providesupport.com/lcbpsh.gif" width="140" height="17" style="border:0;display:block;margin:auto"></a></td></tr></table>'; } else { psHwEbd.innerHTML = '<a href="#" onclick="psHwEbow(); return false;"><span id="psHwEbl">' + 'Leave Chat Online' + '</span></a>'; } } else { psHwEbd.innerHTML = ''; } }
var psHwEbop=false;function psHwEbco(){var w1=psHwEbci.width-1;psHwEbol=(w1&1)!=0;psHwEbsl(psHwEbol?"Live Chat Online":"Leave Chat Online");psHwEbscf((w1&2)!=0);var h=psHwEbci.height;if(h==1){psHwEbop=false;}else if((h==2)&&(!psHwEbop)){psHwEbop=true;psHwEbsi(1);}else if((h==3)&&(!psHwEbop)){psHwEbop=true;psHwEbsi(2);}}
var psHwEbci=new Image();psHwEbci.onload=psHwEbco;var psHwEbpm=false;var psHwEbcp=psHwEbpm?30:60;var psHwEbct=null;function psHwEbscf(p){if(psHwEbpm!=p){psHwEbpm=p;psHwEbcp=psHwEbpm?30:60;if(psHwEbct!=null){clearTimeout(psHwEbct);psHwEbct=null;}
psHwEbct=psHwEbst("psHwEbrc()",psHwEbcp);}}
function psHwEbrc(){psHwEbct=psHwEbst("psHwEbrc()",psHwEbcp);try{psHwEbci.src="https://image.providesupport.com/cmd/nailsuperstore?"+"ps_t="+psHwEbn()+"&ps_l="+escape(document.location)+"&ps_r="+escape(document.referrer)+"&ps_s="+psHwEbsid+""+"";}catch(e){}}
psHwEbrc(); var psHwEbcl = "Leave Chat Online";

function psHwEbsl(l) {
    if (psHwEbcl != l) {
        var d = document.getElementById('psHwEbl');
        if (d != null) {
            d.innerHTML = l;
        }
        psHwEbcl = l;
        var psHwEbd2 = document.getElementById('scHwEb2');
        if (psHwEbd2 != null) {
            psHwEbd2.innerHTML = psHwEbd.innerHTML;
        }
    }
}