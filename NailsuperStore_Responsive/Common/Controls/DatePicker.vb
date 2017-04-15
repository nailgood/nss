Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.web
Imports System.Text

Namespace Controls

	Public Class DatePicker
		Inherits Label
		Implements INamingContainer
		Implements IPostBackDataHandler

#Region "Members"

		Dim m_closeGif As String = "close.gif"
		Dim m_dividerGif As String = "divider.gif"
		Dim m_drop1Gif As String = "drop1.gif"
		Dim m_drop2Gif As String = "drop2.gif"
		Dim m_left1Gif As String = "left1.gif"
		Dim m_left2Gif As String = "left2.gif"
		Dim m_right1Gif As String = "right1.gif"
		Dim m_right2Gif As String = "right2.gif"
        Dim m_imgDirectory As String = "/includes/theme-admin/images/calendar/"
		Dim m_ControlCssClass As String = ""
		Dim m_DateType As String = "mm/dd/yyyy"

#End Region
#Region "Properties"

		Public Property Value() As DateTime
			Get
				If Text = String.Empty Then Return Nothing
				Return Text
			End Get
			Set(ByVal value As DateTime)
				If value = Nothing Then
					Text = String.Empty
				Else
					Text = value
				End If
			End Set
		End Property

		Public Overrides Property Text() As String
			Get
				If (Me.Controls.Count = 0) Then
					If ViewState("Text") <> Nothing Then
						Return ViewState("Text")
					Else
						Return ""
					End If
				End If
				Return CType(FindControl("cal"), System.Web.UI.WebControls.TextBox).Text
			End Get
			Set(ByVal Value As String)
				If Value = "12:00:00 AM" Then Value = String.Empty

				If Not (Me.Controls.Count = 0) Then
					CType(FindControl("cal"), System.Web.UI.WebControls.TextBox).Text = Value
					ViewState("Text") = ""
				Else
					ViewState("Text") = Value
				End If
			End Set
		End Property

		Public Overrides Property CssClass() As String
			Get
				If (Me.Controls.Count = 0) Then
					Return ""
				End If
				Return CType(FindControl("cal"), System.Web.UI.WebControls.TextBox).CssClass
			End Get
			Set(ByVal Value As String)
				If Not (Me.Controls.Count = 0) Then
					CType(FindControl("cal"), System.Web.UI.WebControls.TextBox).CssClass = Value
					ViewState("CSS") = ""
				Else
					ViewState("CSS") = Value
				End If
			End Set
		End Property

		Public Property imgClose() As String
			Get
				Return m_closeGif
			End Get
			Set(ByVal Value As String)
				m_closeGif = Value
			End Set
		End Property

		Public Property imgDivider() As String
			Get
				Return m_dividerGif
			End Get
			Set(ByVal Value As String)
				m_dividerGif = Value
			End Set
		End Property

		Public Property imgDrop1() As String
			Get
				Return m_drop1Gif
			End Get
			Set(ByVal Value As String)
				m_drop1Gif = Value
			End Set
		End Property

		Public Property imgDrop2() As String
			Get
				Return m_drop2Gif
			End Get
			Set(ByVal Value As String)
				m_drop2Gif = Value
			End Set
		End Property

		Public Property imgLeft1() As String
			Get
				Return m_left1Gif
			End Get
			Set(ByVal Value As String)
				m_left1Gif = Value
			End Set
		End Property

		Public Property imgLeft2() As String
			Get
				Return m_left2Gif
			End Get
			Set(ByVal Value As String)
				m_left2Gif = Value
			End Set
		End Property

		Public Property imgRight1() As String
			Get
				Return m_right1Gif
			End Get
			Set(ByVal Value As String)
				m_right1Gif = Value
			End Set
		End Property

		Public Property imgRight2() As String
			Get
				Return m_right2Gif
			End Get
			Set(ByVal Value As String)
				m_right2Gif = Value
			End Set
		End Property

		Public Property imgDirectory() As String
			Get
				Return m_imgDirectory
			End Get
			Set(ByVal Value As String)
				m_imgDirectory = Value
			End Set
		End Property

		Public Property ControlCssClass() As String
			Get
				Return m_ControlCssClass
			End Get
			Set(ByVal Value As String)
				m_ControlCssClass = Value
			End Set
		End Property

		Public Property DateType() As String
			Get
				Return m_DateType
			End Get
			Set(ByVal Value As String)
				m_DateType = Value
			End Set
		End Property

#End Region

		Private Sub RegisterJavascript()
			Dim Build As New StringBuilder

			Build.Append("<script language=JavaScript>")
			Build.Append("// based on calendar by Tan Ling Wee" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var fixedX = -1			// x position (-1 if to appear below control)" & vbCrLf)
			Build.Append("var fixedY = -1			// y position (-1 if to appear below control)" & vbCrLf)
			Build.Append("var startAt = 0			// 0 - sunday ; 1 - monday" & vbCrLf)
			Build.Append("var showWeekNumber = 0	// 0 - don't show; 1 - show" & vbCrLf)
			Build.Append("var showToday = 1		// 0 - don't show; 1 - show" & vbCrLf)
			Build.Append("var imgDir = """ & m_imgDirectory & """" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var todayString = ""Today is """ & vbCrLf)
			Build.Append("var weekString = ""Wk""" & vbCrLf)
			Build.Append("var scrollLeftMessage = ""Click to scroll to previous month. Hold mouse button to scroll automatically.""" & vbCrLf)
			Build.Append("var scrollRightMessage = ""Click to scroll to next month. Hold mouse button to scroll automatically.""" & vbCrLf)
			Build.Append("var selectMonthMessage = ""Click to select a month.""" & vbCrLf)
			Build.Append("var selectYearMessage = ""Click to select a year.""" & vbCrLf)
			Build.Append("var selectDateMessage = ""Select [date] as date."" // do not replace [date], it will be replaced by date." & vbCrLf)
			Build.Append("var styleBottom=""color:#0000AA;""" & vbCrLf)
			Build.Append("var styleAnchor=""text-decoration:none;color:black;""" & vbCrLf)
			Build.Append("var styleLightBorder=""border-style:solid;border-width:1px;border-color:#a0a0a0;""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var	crossobj, crossMonthObj, crossYearObj, monthSelected, yearSelected, dateSelected, omonthSelected, oyearSelected, odateSelected, monthConstructed, yearConstructed, intervalID1, intervalID2, timeoutID1, timeoutID2, ctlToPlaceValue, ctlNow, dateFormat, nStartingYear" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var	bPageLoaded=false" & vbCrLf)
			Build.Append("var	ie=document.all" & vbCrLf)
			Build.Append("var	dom=document.getElementById" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var	ns4=document.layers" & vbCrLf)
			Build.Append("var	today =	new	Date()" & vbCrLf)
			Build.Append("var	dateNow	 = today.getDate()" & vbCrLf)
			Build.Append("var	monthNow = today.getMonth()" & vbCrLf)
			Build.Append("var	yearNow	 = today.getYear()" & vbCrLf)
			Build.Append("var	imgsrc = new Array(""" & m_drop1Gif & """,""" & m_drop2Gif & """,""" & m_left1Gif & """,""" & m_left2Gif & """,""" & m_right1Gif & """,""" & m_right2Gif & """)" & vbCrLf)
			Build.Append("var	img	= new Array()" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var bShow = false;" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("/* hides <select> and <applet> objects (for IE only) */" & vbCrLf)
			Build.Append("function hideElement( elmID, overDiv )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if( ie )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("for( i = 0; i < document.all.tags( elmID ).length; i++ )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("obj = document.all.tags( elmID )[i];" & vbCrLf)
			Build.Append("if( !obj || !obj.offsetParent )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("continue;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("// Find the element's offsetTop and offsetLeft relative to the BODY tag." & vbCrLf)
			Build.Append("objLeft   = obj.offsetLeft;" & vbCrLf)
			Build.Append("objTop    = obj.offsetTop;" & vbCrLf)
			Build.Append("objParent = obj.offsetParent;" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var bContinue = true;" & vbCrLf)
			Build.Append("while( objParent.tagName.toUpperCase() != ""BODY"" && bContinue)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("objLeft  += objParent.offsetLeft;" & vbCrLf)
			Build.Append("objTop   += objParent.offsetTop;" & vbCrLf)
			Build.Append("objParent.offsetParent ? objParent = objParent.offsetParent : bContinue = false;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("objHeight = obj.offsetHeight;" & vbCrLf)
			Build.Append("objWidth = obj.offsetWidth;" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if(( overDiv.offsetLeft + overDiv.offsetWidth ) <= objLeft );" & vbCrLf)
			Build.Append("else if(( overDiv.offsetTop + overDiv.offsetHeight ) <= objTop );" & vbCrLf)
			Build.Append("else if( overDiv.offsetTop >= ( objTop + objHeight ));" & vbCrLf)
			Build.Append("else if( overDiv.offsetLeft >= ( objLeft + objWidth ));" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("obj.style.visibility = ""hidden"";" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("/*" & vbCrLf)
			Build.Append("* unhides <select> and <applet> objects (for IE only)" & vbCrLf)
			Build.Append("*/" & vbCrLf)
			Build.Append("function showElement( elmID )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if( ie )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("for( i = 0; i < document.all.tags( elmID ).length; i++ )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("obj = document.all.tags( elmID )[i];" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if( !obj || !obj.offsetParent )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("continue;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("obj.style.visibility = """";" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function HolidayRec (d, m, y, desc)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("this.d = d" & vbCrLf)
			Build.Append("this.m = m" & vbCrLf)
			Build.Append("this.y = y" & vbCrLf)
			Build.Append("this.desc = desc" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var HolidaysCounter = 0" & vbCrLf)
			Build.Append("var Holidays = new Array()" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function addHoliday (d, m, y, desc)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("Holidays[HolidaysCounter++] = new HolidayRec ( d, m, y, desc )" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (dom)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("for	(i=0;i<imgsrc.length;i++)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("img[i] = new Image" & vbCrLf)
			Build.Append("img[i].src = imgDir + imgsrc[i]" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("document.write (""<div onclick='bShow=true' id='calendar'	style='z-index:+999;position:absolute;visibility:hidden;'><table	width=""+((showWeekNumber==1)?250:220)+"" style='font-family:arial;font-size:11px;border-width:1px;border-style:solid;border-color:#a0a0a0;font-family:arial; font-size:11px}' bgcolor='#ffffff'><tr bgcolor='#0000aa'><td><table width='""+((showWeekNumber==1)?248:218)+""'><tr><td style='padding:2px;font-family:arial; font-size:11px;'><font color='#ffffff'><B><span id='caption'></span></B></font></td><td align=right><a href='javascript:hideCalendar()'><IMG SRC='""+imgDir+""" & m_closeGif & "' WIDTH='15' HEIGHT='13' BORDER='0' ALT='Close the Calendar'></a></td></tr></table></td></tr><tr><td style='padding:5px' bgcolor=#ffffff><span id='content'></span></td></tr>"")" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (showToday==1)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("document.write (""<tr bgcolor=#f0f0f0><td style='padding:5px' align=center><span id='lblToday'></span> | <a href='javascript:dateSelected=0;closeCalendar();' style='""+styleBottom+""'>Clear</a></td></tr>"")" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("document.write (""</table></div><div id='selectMonth' style='z-index:+999;position:absolute;visibility:hidden;'></div><div id='selectYear' style='z-index:+999;position:absolute;visibility:hidden;'></div>"");" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var	monthName =	new	Array(""January"",""February"",""March"",""April"",""May"",""June"",""July"",""August"",""September"",""October"",""November"",""December"")" & vbCrLf)
			Build.Append("if (startAt==0)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("dayName = new Array	(""Sun"",""Mon"",""Tue"",""Wed"",""Thu"",""Fri"",""Sat"")" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("dayName = new Array	(""Mon"",""Tue"",""Wed"",""Thu"",""Fri"",""Sat"",""Sun"")" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function swapImage(srcImg, destImg){" & vbCrLf)
			Build.Append("if (ie)	{ document.getElementById(srcImg).setAttribute(""src"",imgDir + destImg) }" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function init()	{" & vbCrLf)
			Build.Append("if (!ns4)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if (!ie) { yearNow += 1900	}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("crossobj=(dom)?document.getElementById(""calendar"").style : ie? document.all.calendar : document.calendar" & vbCrLf)
			Build.Append("hideCalendar()" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("crossMonthObj=(dom)?document.getElementById(""selectMonth"").style : ie? document.all.selectMonth	: document.selectMonth" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("crossYearObj=(dom)?document.getElementById(""selectYear"").style : ie? document.all.selectYear : document.selectYear" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("monthConstructed=false;" & vbCrLf)
			Build.Append("yearConstructed=false;" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (showToday==1)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("document.getElementById(""lblToday"").innerHTML =	todayString + ""<a onmousemove='window.status=\""\""' onmouseout='window.status=\""\""' style='""+styleBottom+""' href='javascript:monthSelected=monthNow;yearSelected=yearNow;closeCalendar();'>""+dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]+"", "" + dateNow + "" "" + monthName[monthNow].substring(0,3)	+ ""	"" +	yearNow	+ ""</a>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHTML1=""<span id='spanLeft'	style='border-style:solid;border-width:1px;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\""changeLeft\"",\""left2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+scrollLeftMessage+""\""' onclick='javascript:decMonth()' onmouseout='clearInterval(intervalID1);swapImage(\""changeLeft\"",\""left1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\""StartDecMonth()\"",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeLeft' SRC='""+imgDir+""left1.gif' width=10 height=11 BORDER=0>&nbsp</span>&nbsp;""" & vbCrLf)
			Build.Append("sHTML1+=""<span id='spanRight' style='border-style:solid;border-width:1px;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\""changeRight\"",\""right2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+scrollRightMessage+""\""' onmouseout='clearInterval(intervalID1);swapImage(\""changeRight\"",\""right1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""' onclick='incMonth()' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\""StartIncMonth()\"",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeRight' SRC='""+imgDir+""right1.gif'	width=10 height=11 BORDER=0>&nbsp</span>&nbsp;""" & vbCrLf)
			Build.Append("sHTML1+=""<span id='spanMonth' style='border-style:solid;border-width:1px;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\""changeMonth\"",\""drop2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+selectMonthMessage+""\""' onmouseout='swapImage(\""changeMonth\"",\""drop1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""' onclick='popUpMonth()'></span>&nbsp;""" & vbCrLf)
			Build.Append("sHTML1+=""<span id='spanYear' style='border-style:solid;border-width:1px;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\""changeYear\"",\""drop2.gif\"");this.style.borderColor=\""#88AAFF\"";window.status=\""""+selectYearMessage+""\""'	onmouseout='swapImage(\""changeYear\"",\""drop1.gif\"");this.style.borderColor=\""#3366FF\"";window.status=\""\""'	onclick='popUpYear()'></span>&nbsp;""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("document.getElementById(""caption"").innerHTML  =	sHTML1" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("bPageLoaded=true" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function hideCalendar()	{" & vbCrLf)
			Build.Append("crossobj.visibility=""hidden""" & vbCrLf)
			Build.Append("if (crossMonthObj != null){crossMonthObj.visibility=""hidden""}" & vbCrLf)
			Build.Append("if (crossYearObj !=	null){crossYearObj.visibility=""hidden""}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("showElement( 'SELECT' );" & vbCrLf)
			Build.Append("showElement( 'APPLET' );" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function padZero(num) {" & vbCrLf)
			Build.Append("return (num	< 10)? '0' + num : num ;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function constructDate(d,m,y)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if (d==0) {return ''; }")
			Build.Append("sTmp = dateFormat" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""dd"",""<e>"")" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""d"",""<d>"")" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""<e>"",padZero(d))" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""<d>"",d)" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""mmm"",""<o>"")" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""mm"",""<n>"")" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""m"",""<m>"")" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""<m>"",m+1)" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""<n>"",padZero(m+1))" & vbCrLf)
			Build.Append("sTmp = sTmp.replace	(""<o>"",monthName[m])" & vbCrLf)
			Build.Append("return sTmp.replace (""yyyy"",y)" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function closeCalendar() {" & vbCrLf)
			Build.Append("var	sTmp" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("hideCalendar();" & vbCrLf)
			Build.Append("ctlToPlaceValue.value =	constructDate(dateSelected,monthSelected,yearSelected)" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("/*** Month Pulldown	***/" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function StartDecMonth()" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("intervalID1=setInterval(""decMonth()"",80)" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function StartIncMonth()" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("intervalID1=setInterval(""incMonth()"",80)" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function incMonth () {" & vbCrLf)
			Build.Append("monthSelected++" & vbCrLf)
			Build.Append("if (monthSelected>11) {" & vbCrLf)
			Build.Append("monthSelected=0" & vbCrLf)
			Build.Append("yearSelected++" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("constructCalendar()" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function decMonth () {" & vbCrLf)
			Build.Append("monthSelected--" & vbCrLf)
			Build.Append("if (monthSelected<0) {" & vbCrLf)
			Build.Append("monthSelected=11" & vbCrLf)
			Build.Append("yearSelected--" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("constructCalendar()" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function constructMonth() {" & vbCrLf)
			Build.Append("popDownYear()" & vbCrLf)
			Build.Append("if (!monthConstructed) {" & vbCrLf)
			Build.Append("sHTML =	""""" & vbCrLf)
			Build.Append("for	(i=0; i<12;	i++) {" & vbCrLf)
			Build.Append("sName =	monthName[i];" & vbCrLf)
			Build.Append("if (i==monthSelected){" & vbCrLf)
			Build.Append("sName =	""<B>"" +	sName +	""</B>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("sHTML += ""<tr><td id='m"" + i + ""' onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='this.style.backgroundColor=\""\""' style='cursor:pointer' onclick='monthConstructed=false;monthSelected="" + i + "";constructCalendar();popDownMonth();event.cancelBubble=true'>&nbsp;"" + sName + ""&nbsp;</td></tr>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("document.getElementById(""selectMonth"").innerHTML = ""<table width=70	style='font-family:arial; font-size:11px; border-width:1px; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\""popDownMonth()\"",100);event.cancelBubble=true'>"" +	sHTML +	""</table>""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("monthConstructed=true" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function popUpMonth() {" & vbCrLf)
			Build.Append("constructMonth()" & vbCrLf)
			Build.Append("crossMonthObj.visibility = (dom||ie)? ""visible""	: ""show""" & vbCrLf)
			Build.Append("crossMonthObj.left = (parseInt(crossobj.left) + 50) + 'px';" & vbCrLf)
			Build.Append("crossMonthObj.top = (parseInt(crossobj.top) + 26) + 'px';" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("hideElement( 'SELECT', document.getElementById(""selectMonth"") );" & vbCrLf)
			Build.Append("hideElement( 'APPLET', document.getElementById(""selectMonth"") );" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function popDownMonth()	{" & vbCrLf)
			Build.Append("crossMonthObj.visibility= ""hidden""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("/*** Year Pulldown ***/" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function incYear() {" & vbCrLf)
			Build.Append("for	(i=0; i<7; i++){" & vbCrLf)
			Build.Append("newYear	= (i+nStartingYear)+1" & vbCrLf)
			Build.Append("if (newYear==yearSelected)" & vbCrLf)
			Build.Append("{ txtYear =	""&nbsp;<B>""	+ newYear +	""</B>&nbsp;"" }" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{ txtYear =	""&nbsp;"" + newYear + ""&nbsp;"" }" & vbCrLf)
			Build.Append("document.getElementById(""y""+i).innerHTML = txtYear" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("nStartingYear ++;" & vbCrLf)
			Build.Append("bShow=true" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function decYear() {" & vbCrLf)
			Build.Append("for	(i=0; i<7; i++){" & vbCrLf)
			Build.Append("newYear	= (i+nStartingYear)-1" & vbCrLf)
			Build.Append("if (newYear==yearSelected)" & vbCrLf)
			Build.Append("{ txtYear =	""&nbsp;<B>""	+ newYear +	""</B>&nbsp;"" }" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{ txtYear =	""&nbsp;"" + newYear + ""&nbsp;"" }" & vbCrLf)
			Build.Append("document.getElementById(""y""+i).innerHTML = txtYear" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("nStartingYear --;" & vbCrLf)
			Build.Append("bShow=true" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function selectYear(nYear) {" & vbCrLf)
			Build.Append("yearSelected=parseInt(nYear+nStartingYear);" & vbCrLf)
			Build.Append("yearConstructed=false;" & vbCrLf)
			Build.Append("constructCalendar();" & vbCrLf)
			Build.Append("popDownYear();" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function constructYear() {" & vbCrLf)
			Build.Append("popDownMonth()" & vbCrLf)
			Build.Append("sHTML =	""""" & vbCrLf)
			Build.Append("if (!yearConstructed) {" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHTML =	""<tr><td align='center'	onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\""\""' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\""decYear()\"",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("j =	0" & vbCrLf)
			Build.Append("nStartingYear =	yearSelected-3" & vbCrLf)
			Build.Append("for	(i=(yearSelected-3); i<=(yearSelected+3); i++) {" & vbCrLf)
			Build.Append("sName =	i;" & vbCrLf)
			Build.Append("if (i==yearSelected){" & vbCrLf)
			Build.Append("sName =	""<B>"" +	sName +	""</B>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHTML += ""<tr><td id='y"" + j + ""' onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='this.style.backgroundColor=\""\""' style='cursor:pointer' onclick='selectYear(""+j+"");event.cancelBubble=true'>&nbsp;"" + sName + ""&nbsp;</td></tr>""" & vbCrLf)
			Build.Append("j ++;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHTML += ""<tr><td align='center' onmouseover='this.style.backgroundColor=\""#FFCC99\""' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\""\""' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\""incYear()\"",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("document.getElementById(""selectYear"").innerHTML	= ""<table width=44 style='font-family:arial; font-size:11px; border-width:1px; border-style:solid; border-color:#a0a0a0;'	bgcolor='#FFFFDD' onmouseover='clearTimeout(timeoutID2)' onmouseout='clearTimeout(timeoutID2);timeoutID2=setTimeout(\""popDownYear()\"",100)' cellspacing=0>""	+ sHTML	+ ""</table>""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("yearConstructed	= true" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function popDownYear() {" & vbCrLf)
			Build.Append("clearInterval(intervalID1)" & vbCrLf)
			Build.Append("clearTimeout(timeoutID1)" & vbCrLf)
			Build.Append("clearInterval(intervalID2)" & vbCrLf)
			Build.Append("clearTimeout(timeoutID2)" & vbCrLf)
			Build.Append("crossYearObj.visibility= ""hidden""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function popUpYear() {" & vbCrLf)
			Build.Append("var	leftOffset" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("constructYear()" & vbCrLf)
			Build.Append("crossYearObj.visibility	= (dom||ie)? ""visible"" : ""show""" & vbCrLf)
			Build.Append("leftOffset = parseInt(crossobj.left) + document.getElementById(""spanYear"").offsetLeft" & vbCrLf)
			Build.Append("leftOffset += 6" & vbCrLf)
			Build.Append("crossYearObj.left = leftOffset + 'px';" & vbCrLf)
			Build.Append("crossYearObj.top = (parseInt(crossobj.top) +	26) + 'px';" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("/*** calendar ***/" & vbCrLf)
			Build.Append("function WeekNbr(n) {" & vbCrLf)
			Build.Append("// Algorithm used:" & vbCrLf)
			Build.Append("// From Klaus Tondering's Calendar document (The Authority/Guru)" & vbCrLf)
			Build.Append("// hhtp://www.tondering.dk/claus/calendar.html" & vbCrLf)
			Build.Append("// a = (14-month) / 12" & vbCrLf)
			Build.Append("// y = year + 4800 - a" & vbCrLf)
			Build.Append("// m = month + 12a - 3" & vbCrLf)
			Build.Append("// J = day + (153m + 2) / 5 + 365y + y / 4 - y / 100 + y / 400 - 32045" & vbCrLf)
			Build.Append("// d4 = (J + 31741 - (J mod 7)) mod 146097 mod 36524 mod 1461" & vbCrLf)
			Build.Append("// L = d4 / 1460" & vbCrLf)
			Build.Append("// d1 = ((d4 - L) mod 365) + L" & vbCrLf)
			Build.Append("// WeekNumber = d1 / 7 + 1" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("year = n.getFullYear();" & vbCrLf)
			Build.Append("month = n.getMonth() + 1;" & vbCrLf)
			Build.Append("if (startAt == 0) {" & vbCrLf)
			Build.Append("day = n.getDate() + 1;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else {" & vbCrLf)
			Build.Append("day = n.getDate();" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("a = Math.floor((14-month) / 12);" & vbCrLf)
			Build.Append("y = year + 4800 - a;" & vbCrLf)
			Build.Append("m = month + 12 * a - 3;" & vbCrLf)
			Build.Append("b = Math.floor(y/4) - Math.floor(y/100) + Math.floor(y/400);" & vbCrLf)
			Build.Append("J = day + Math.floor((153 * m + 2) / 5) + 365 * y + b - 32045;" & vbCrLf)
			Build.Append("d4 = (((J + 31741 - (J % 7)) % 146097) % 36524) % 1461;" & vbCrLf)
			Build.Append("L = Math.floor(d4 / 1460);" & vbCrLf)
			Build.Append("d1 = ((d4 - L) % 365) + L;" & vbCrLf)
			Build.Append("week = Math.floor(d1/7) + 1;" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("return week;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function constructCalendar () {" & vbCrLf)
			Build.Append("var aNumDays = Array (31,0,31,30,31,30,31,31,30,31,30,31)" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var dateMessage" & vbCrLf)
			Build.Append("var	startDate =	new	Date (yearSelected,monthSelected,1)" & vbCrLf)
			Build.Append("var endDate" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (monthSelected==1)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("endDate	= new Date (yearSelected,monthSelected+1,1);" & vbCrLf)
			Build.Append("endDate	= new Date (endDate	- (24*60*60*1000));" & vbCrLf)
			Build.Append("numDaysInMonth = endDate.getDate()" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("numDaysInMonth = aNumDays[monthSelected];" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("datePointer	= 0" & vbCrLf)
			Build.Append("dayPointer = startDate.getDay() - startAt" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (dayPointer<0)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("dayPointer = 6" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHTML =	""<table	 border=0 style='font-family:verdana;font-size:10px;'><tr>""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (showWeekNumber==1)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("sHTML += ""<td width=27><b>"" + weekString + ""</b></td><td width=1 rowspan=7 bgcolor='#d0d0d0' style='padding:0px'><img src='""+imgDir+""" & m_dividerGif & "' width=1></td>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("for	(i=0; i<7; i++)	{" & vbCrLf)
			Build.Append("sHTML += ""<td width='27' align='right'><B>""+ dayName[i]+""</B></td>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("sHTML +=""</tr><tr>""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (showWeekNumber==1)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("sHTML += ""<td align=right>"" + WeekNbr(startDate) + ""&nbsp;</td>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("for	( var i=1; i<=dayPointer;i++ )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("sHTML += ""<td>&nbsp;</td>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("for	( datePointer=1; datePointer<=numDaysInMonth; datePointer++ )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("dayPointer++;" & vbCrLf)
			Build.Append("sHTML += ""<td align=right>""" & vbCrLf)
			Build.Append("sStyle=styleAnchor" & vbCrLf)
			Build.Append("if ((datePointer==odateSelected) &&	(monthSelected==omonthSelected)	&& (yearSelected==oyearSelected))" & vbCrLf)
			Build.Append("{ sStyle+=styleLightBorder }" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHint = """"" & vbCrLf)
			Build.Append("for (k=0;k<HolidaysCounter;k++)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if ((parseInt(Holidays[k].d)==datePointer)&&(parseInt(Holidays[k].m)==(monthSelected+1)))" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if ((parseInt(Holidays[k].y)==0)||((parseInt(Holidays[k].y)==yearSelected)&&(parseInt(Holidays[k].y)!=0)))" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("sStyle+=""background-color:#FFDDDD;""" & vbCrLf)
			Build.Append("sHint+=sHint==""""?Holidays[k].desc:""\n""+Holidays[k].desc" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("var regexp= /\""/g" & vbCrLf)
			Build.Append("sHint=sHint.replace(regexp,""&quot;"")" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("dateMessage = ""onmousemove='window.status=\""""+selectDateMessage.replace(""[date]"",constructDate(datePointer,monthSelected,yearSelected))+""\""' onmouseout='window.status=\""\""' """ & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if ((datePointer==dateNow)&&(monthSelected==monthNow)&&(yearSelected==yearNow))" & vbCrLf)
			Build.Append("{ sHTML += ""<b><a ""+dateMessage+"" title=\"""" + sHint + ""\"" style='""+sStyle+""' href='javascript:dateSelected=""+datePointer+"";closeCalendar();'><font color=#ff0000>&nbsp;"" + datePointer + ""</font>&nbsp;</a></b>""}" & vbCrLf)
			Build.Append("else if	(dayPointer % 7 == (startAt * -1)+1)" & vbCrLf)
			Build.Append("{ sHTML += ""<a ""+dateMessage+"" title=\"""" + sHint + ""\"" style='""+sStyle+""' href='javascript:dateSelected=""+datePointer + "";closeCalendar();'>&nbsp;<font color=#909090>"" + datePointer + ""</font>&nbsp;</a>"" }" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{ sHTML += ""<a ""+dateMessage+"" title=\"""" + sHint + ""\"" style='""+sStyle+""' href='javascript:dateSelected=""+datePointer + "";closeCalendar();'>&nbsp;"" + datePointer + ""&nbsp;</a>"" }" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("sHTML += """"" & vbCrLf)
			Build.Append("if ((dayPointer+startAt) % 7 == startAt) {" & vbCrLf)
			Build.Append("sHTML += ""</tr><tr>""" & vbCrLf)
			Build.Append("if ((showWeekNumber==1)&&(datePointer<numDaysInMonth))" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("sHTML += ""<td align=right>"" + (WeekNbr(new Date(yearSelected,monthSelected,datePointer+1))) + ""&nbsp;</td>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("document.getElementById(""content"").innerHTML   = sHTML" & vbCrLf)
			Build.Append("document.getElementById(""spanMonth"").innerHTML = ""&nbsp;"" +	monthName[monthSelected] + ""&nbsp;<IMG id='changeMonth' SRC='""+imgDir+""drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>""" & vbCrLf)
			Build.Append("document.getElementById(""spanYear"").innerHTML =	""&nbsp;"" + yearSelected	+ ""&nbsp;<IMG id='changeYear' SRC='""+imgDir+""drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("function popUpCalendar(ctl,	ctl2, format) {" & vbCrLf)
			Build.Append("var	leftpos=0" & vbCrLf)
			Build.Append("var	toppos=0" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if (bPageLoaded)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if ( crossobj.visibility ==	""hidden"" ) {" & vbCrLf)
			Build.Append("ctlToPlaceValue	= ctl2" & vbCrLf)
			Build.Append("dateFormat=format;" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("formatChar = "" """ & vbCrLf)
			Build.Append("aFormat	= dateFormat.split(formatChar)" & vbCrLf)
			Build.Append("if (aFormat.length<3)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("formatChar = ""/""" & vbCrLf)
			Build.Append("aFormat	= dateFormat.split(formatChar)" & vbCrLf)
			Build.Append("if (aFormat.length<3)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("formatChar = "".""" & vbCrLf)
			Build.Append("aFormat	= dateFormat.split(formatChar)" & vbCrLf)
			Build.Append("if (aFormat.length<3)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("formatChar = ""-""" & vbCrLf)
			Build.Append("aFormat	= dateFormat.split(formatChar)" & vbCrLf)
			Build.Append("if (aFormat.length<3)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("// invalid date	format" & vbCrLf)
			Build.Append("formatChar=""""" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("tokensChanged =	0" & vbCrLf)
			Build.Append("if ( formatChar	!= """" )" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("// use user's date" & vbCrLf)
			Build.Append("aData =	ctl2.value.split(formatChar)" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("for	(i=0;i<3;i++)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if ((aFormat[i]==""d"") || (aFormat[i]==""dd""))" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("dateSelected = parseInt(aData[i], 10)" & vbCrLf)
			Build.Append("tokensChanged ++" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else if	((aFormat[i]==""m"") || (aFormat[i]==""mm""))" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("monthSelected =	parseInt(aData[i], 10) - 1" & vbCrLf)
			Build.Append("tokensChanged ++" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else if	(aFormat[i]==""yyyy"")" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("yearSelected = parseInt(aData[i], 10)" & vbCrLf)
			Build.Append("tokensChanged ++" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else if	(aFormat[i]==""mmm"")" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("for	(j=0; j<12;	j++)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("if (aData[i]==monthName[j])" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("monthSelected=j" & vbCrLf)
			Build.Append("tokensChanged ++" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if ((tokensChanged!=3)||isNaN(dateSelected)||isNaN(monthSelected)||isNaN(yearSelected))" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("dateSelected = dateNow" & vbCrLf)
			Build.Append("monthSelected =	monthNow" & vbCrLf)
			Build.Append("yearSelected = yearNow" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("odateSelected=dateSelected" & vbCrLf)
			Build.Append("omonthSelected=monthSelected" & vbCrLf)
			Build.Append("oyearSelected=yearSelected" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("aTag = ctl" & vbCrLf)
			Build.Append("do {" & vbCrLf)
			Build.Append("aTag = aTag.offsetParent;" & vbCrLf)
			Build.Append("leftpos	+= aTag.offsetLeft;" & vbCrLf)
			Build.Append("toppos += aTag.offsetTop;" & vbCrLf)
			Build.Append("} while(aTag.tagName!=""BODY"" && aTag.offsetParent);" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("crossobj.left =	(fixedX==-1 ? ctl.offsetLeft	+ leftpos :	fixedX) + 'px';" & vbCrLf)
			Build.Append("crossobj.top = (fixedY==-1 ?	ctl.offsetTop +	toppos + ctl.offsetHeight +	2 :	fixedY) + 'px';" & vbCrLf)
			Build.Append("constructCalendar (1, monthSelected, yearSelected);" & vbCrLf)
			Build.Append("crossobj.visibility=(dom||ie)? ""visible"" : ""show""" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("hideElement( 'SELECT', document.getElementById(""calendar"") );" & vbCrLf)
			Build.Append("hideElement( 'APPLET', document.getElementById(""calendar"") );" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("bShow = true;" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("hideCalendar()" & vbCrLf)
			Build.Append("if (ctlNow!=ctl) {popUpCalendar(ctl, ctl2, format)}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("ctlNow = ctl" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("document.onkeypress = function hidecal1 (e) {" & vbCrLf)
			Build.Append("var keynum;")
			Build.Append("if (window.event) {keynum = event.keyCode;} else if(e.keyCode) {keynum = e.keyCode;}" & vbCrLf)
			Build.Append("if (keynum==27)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("hideCalendar()" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("document.onclick = function hidecal2 () {" & vbCrLf)
			Build.Append("if (!bShow)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("hideCalendar()" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("bShow = false" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("" & vbCrLf)
			Build.Append("if(ie)" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("init()" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("else" & vbCrLf)
			Build.Append("{" & vbCrLf)
			Build.Append("window.onload=init" & vbCrLf)
			Build.Append("}" & vbCrLf)
			Build.Append("<")
			Build.Append("/")
			Build.Append("script>")

			If Not Page.ClientScript.IsClientScriptBlockRegistered("DatePicker") Then
				Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DatePicker", Build.ToString)
			End If
		End Sub

		'Register Controls
		Protected Overrides Sub CreateChildControls()
			Dim txtTextBox As New System.Web.UI.WebControls.TextBox()
			txtTextBox.Style.Add("width", "80px")
			If Len(m_ControlCssClass) > 0 Then
				txtTextBox.CssClass = m_ControlCssClass
			End If
			If Not (ViewState("Text") = "") Then
				txtTextBox.Text = ViewState("Text")
			End If
			If Not (ViewState("CSS") = "") Then
				txtTextBox.CssClass = ViewState("CSS")
			End If
			txtTextBox.ID = "cal"
			txtTextBox.Attributes.Add("onclick", "popUpCalendar(document.getElementById('" & Me.ClientID & "_cal'),document.getElementById('" & Me.ClientID & "_cal'), '" & m_DateType & "');")

			Dim lnkCalendar As New Web.UI.WebControls.ImageButton
            lnkCalendar.ImageUrl = "/includes/theme-admin/images/calendar/picker.gif"
			lnkCalendar.OnClientClick = "popUpCalendar(document.getElementById('" & Me.ClientID & "_cal'),document.getElementById('" & Me.ClientID & "_cal'), '" & m_DateType & "'); return false;"
			lnkCalendar.EnableViewState = False

			Me.Controls.Add(New LiteralControl("<table border=0 cellpadding=0 cellspacing=0><tr><td>"))
			Me.Controls.Add(txtTextBox)
			Me.Controls.Add(New LiteralControl("</td><td>"))
			Me.Controls.Add(lnkCalendar)
			Me.Controls.Add(New LiteralControl("</td></tr></table>"))

		End Sub

		Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
			RegisterJavascript()

			Page.RegisterRequiresPostBack(Me)
			MyBase.OnInit(e)
		End Sub

		Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
			Text = CType(FindControl("cal"), System.Web.UI.WebControls.TextBox).Text
			Return False
		End Function

		Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
		End Sub
	End Class

End Namespace



