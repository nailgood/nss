<%
Response.Cache.SetCacheability(HttpCacheability.NoCache)
Dim TimeoutMinutes As Integer = 30
Dim doc As New System.Xml.XmlDocument
doc.Load(Server.MapPath("/web.config"))
Dim ns As New System.Xml.XmlNamespaceManager(doc.NameTable)
ns.AddNamespace("x", "http://schemas.microsoft.com/.NetConfiguration/v2.0")
Dim n As System.Xml.XmlNode = doc.SelectSingleNode("/x:configuration/x:system.web/x:authentication/x:forms", ns)
If Not n Is Nothing AndAlso Not n.Attributes("timeout") Is Nothing Then
	TimeoutMinutes = n.Attributes("timeout").Value
End If
%>
var sessionTimer;
var loginTimer;
var totalTimeoutMinutes = <%=TimeoutMinutes%>;
var warningMinutes = 3;
var oneMinute = 60000;
var timeoutInterval = (totalTimeoutMinutes - warningMinutes) * oneMinute;

function extend(s) {
	if(s == '1') {
		clearTimeout(sessionTimer);
		clearTimeout(loginTimer);
		sessionTimer = setTimeout("extendSession()", timeoutInterval);
		document.getElementById('divAdminPopup').style.display = 'none';
	} else {
		gotoLogin();
	}
}

function gotoLogin() {window.location = '/admin/logout.aspx';}

function calculatePosition() {
	var clientBounds = getClientBounds();
	var width = clientBounds.split('|')[0];
	var height = clientBounds.split('|')[1];
	var inner = document.getElementById('divAdminPopupInner');
	var element = document.getElementById('divAdminPopupInfo');
	var popup = document.getElementById('divAdminPopup');
	if (!element || !inner || !popup || popup.style.display == 'none') return;
    
	var pageHeight = getPageHeight();
	var innerHeight = height;
	if(pageHeight > innerHeight) innerHeight = pageHeight; else innerHeight = innerHeight - 10;
	inner.style.height = innerHeight + 'px';

	var x = 0;
	var y = 0;

    if (document.documentElement && document.documentElement.scrollTop) {
        x = document.documentElement.scrollLeft;
        y = document.documentElement.scrollTop;
    } else {
        x = document.body.scrollLeft;
        y = document.body.scrollTop;
    }

	x = Math.max(0, Math.floor(x + width / 2.0 - element.offsetWidth / 2.0 - popup.offsetWidth / 2.0));
	y = Math.max(0, Math.floor(y + height / 2.0 - element.offsetHeight / 2.0));
	
	element.style.left = x + 'px';
	element.style.top = y + 'px';
}

function extendSession() {
	document.getElementById('warningMinutes').innerHTML = warningMinutes + ' ';
	document.getElementById('divAdminPopup').style.display = '';
	calculatePosition();
	loginTimer = setTimeout("gotoLogin()", warningMinutes * oneMinute);
}

function getPageHeight() {
	return document.getElementById('divAdminWrapper').offsetHeight;
}

function sessionBtnClick(btn) {
	if(btn.value=='Continue Logged-In') {
		enqueue("/admin/ajax.aspx?F=ExtendSession", extend);
	} else if(btn.value=='Logout Now') {
		gotoLogin();
	} else {
		loginTimer = setTimeout("gotoLogin()", warningMinutes * oneMinute);
	}
	document.getElementById('divAdminPopup').style.display = 'none';
}

sessionTimer = setTimeout("extendSession()", timeoutInterval);
