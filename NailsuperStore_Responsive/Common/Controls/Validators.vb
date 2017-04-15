Imports System.Text.RegularExpressions
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Components

Namespace Controls

#Region " Regular Validators "

	Public Class EmailValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator

        Public Sub New()
            MyBase.ValidationExpression = "^[A-Za-z0-9]+[A-Za-z0-9_\-\.]*\@([A-Za-z0-9\-]+\.)+[A-Za-z]{2,5}$"
            'MyBase.ValidationExpression = "^[A-Za-z0-9]+[A-Za-z0-9_\-\.]*\@([A-Za-z0-9\-]+\.)+[A-Za-z]{2,5}$"
        End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class
    Public Class RegularExpressionValidator
        Inherits System.Web.UI.WebControls.RegularExpressionValidator
        Private validator As FrontValidator
        Public Sub New()
          
        End Sub
        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
            validator.Initialize()
        End Sub
        Protected Overrides Function EvaluateIsValid() As Boolean
            If MyBase.EvaluateIsValid() Then
                Return True
            Else
                If Enabled Then validator.ChangeStyles()
                Return False
            End If
        End Function
    End Class
    Public Class InternationalPhoneValidator
        Inherits System.Web.UI.WebControls.RegularExpressionValidator
        Private validator As FrontValidator
        Public FrontValidator As Boolean = False
        Public Sub New()

        End Sub
        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)
            ValidationExpression = Utility.ConfigData.PhoneOutUSPattern
            If FrontValidator Then
                validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
                validator.Initialize()
            End If
        End Sub
        Protected Overrides Function EvaluateIsValid() As Boolean
            If MyBase.EvaluateIsValid() Then
                Dim Text As String = CType(FindControl(ControlToValidate), TextBox).Text
                If Not String.IsNullOrEmpty(Text) Then
                    Text = Replace(Text, "+", "")
                    Text = Replace(Text, "(", "")
                    Text = Replace(Text, ")", "")
                    Text = Replace(Text, "[", "")
                    Text = Replace(Text, "]", "")
                    Text = Replace(Text, "-", "")
                    If String.IsNullOrEmpty(Text) Then
                        If Enabled And FrontValidator Then validator.ChangeStyles()
                        Return False
                    End If
                End If
                Return True
            Else
                If Enabled And FrontValidator Then validator.ChangeStyles()
                Return False
            End If
        End Function
    End Class
	Public Class UserNameValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator

		Public Sub New()
			MyBase.ValidationExpression = "^([^$@\ ]+)$"
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	Public Class FloatValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator

        Public Sub New()
            MyBase.ValidationExpression = "^(\+|\-)?([0-9]+)(((\.|\,)?([0-9]+))?)$"
        End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	Public Class IntegerValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator

        Public Sub New()
            MyBase.ValidationExpression = "^([0-9]+)$"
        End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	Public Class PasswordValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator

		Public Sub New()
            MyBase.ValidationExpression = "^(?=.*[a-zA-Z]+|[0-9])(?!.*\s).{4,20}$"
            '"^([a-zA-Z]+|[0-9])(?!.*\s).{4,10}$"
            'MyBase.ValidationExpression = "^(?=.*\d)(?=.*[a-zA-Z][0-9])(?!.*\s).{4,10}$"

		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	Public Class URLValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator

		Public Sub New()
			MyBase.ValidationExpression = "^http(s?)://([^$@\ ]+)$"
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	Public Class RequiredTimeValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is TimePicker Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
			MyBase.AddAttributesToRender(writer)
			If (RenderUplevel And EnableClientScript) Then
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "isNotEmptyTime")
			End If
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim Text As String = CType(FindControl(ControlToValidate), TimePicker).Text
			Return Not String.Empty = Text
		End Function
	End Class

	Public Class TimeValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is TimePicker Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
			MyBase.AddAttributesToRender(writer)

			If (RenderUplevel And EnableClientScript) Then
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "isValidTime")
			End If
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim Text As String = CType(FindControl(ControlToValidate), TimePicker).Text
			If Text = String.Empty Then Return True

			Try
				Dim myTime As DateTime = Date.Parse(Text)
				Return True
			Catch ex As FormatException
				Return False
			End Try
		End Function
	End Class

	Public Class RequiredDateValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is DatePicker Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
			MyBase.AddAttributesToRender(writer)
			If (RenderUplevel And EnableClientScript) Then
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "isNotEmptyDate")
			End If
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim Text As String = CType(FindControl(ControlToValidate), DatePicker).Text
			Return Not String.Empty = Text
		End Function
	End Class

	Public Class DateValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Private m_ExpirationDate As Boolean = False

		Public Property ExpirationDate() As Boolean
			Get
				Return m_ExpirationDate
			End Get
			Set(ByVal value As Boolean)
				m_ExpirationDate = value
			End Set
		End Property

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is DatePicker Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
			MyBase.AddAttributesToRender(writer)

			If (RenderUplevel And EnableClientScript) Then
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "isValidDate")
			End If
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim Text As String = CType(FindControl(ControlToValidate), DatePicker).Text
			If Text = String.Empty Then Return True

			Try
				Dim myDate As Date = Date.Parse(Text)
				If m_ExpirationDate Then
					Dim nDate As Date = Date.Now()
					If Now.Month < nDate.Month And Now.Year <= nDate.Year Then Return False
				End If
				Return True
			Catch ex As FormatException
				Return False
			End Try
		End Function
	End Class

	Public Class RequiredFileUploadValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is FileUpload Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
			MyBase.AddAttributesToRender(writer)

			If (RenderUplevel And EnableClientScript) Then
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "isNotEmptyFile")
			End If
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim currentText As String = CType(FindControl(ControlToValidate), FileUpload).CurrentFileName
			Dim newText As String = CType(FindControl(ControlToValidate), FileUpload).NewFileName
			Dim bDelete As Boolean = CType(FindControl(ControlToValidate), FileUpload).MarkedToDelete

			If newText = String.Empty AndAlso currentText = String.Empty Then Return False
			If newText = String.Empty AndAlso bDelete Then Return False

			Return True
		End Function
	End Class

	Public Class FileUploadExtensionValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Private m_Extensions As String = "doc,txt,pdf,jpg,jpeg,gif,bmp"

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is FileUpload Then
				Return True
			Else
				Return False
			End If
		End Function

		Public Property Extensions() As String
			Get
				Return m_Extensions.ToLower
			End Get
			Set(ByVal value As String)
				If IsNothing(value) OrElse value = Nothing Then
					m_Extensions = String.Empty
				Else
					m_Extensions = value
				End If
			End Set
		End Property

		Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
			MyBase.AddAttributesToRender(writer)

			If (RenderUplevel And EnableClientScript) Then
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "isValidFile")
				Page.ClientScript.RegisterExpandoAttribute(ClientID, "extensions", Extensions)
			End If
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim FileName As String = CType(FindControl(ControlToValidate), FileUpload).NewFileName

			If FileName = String.Empty Then Return True

			Dim Extension As String = Replace(System.IO.Path.GetExtension(FileName).ToLower, ".", "")

			Dim aExtensions() As String = Extensions.Split(",")

			Return (Array.IndexOf(aExtensions, Extension, 0) <> -1)
		End Function
	End Class

	'Custom Classes
	Public Class CurrencyValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator
        Public Sub New()
            MyBase.ValidationExpression = "^(([0-9]{1,3}(\,[0-9]{3})*)|([0-9]{0,3}))(\.[0-9]{2})?$"
        End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	Public Class ZipCodeValidator
		Inherits System.Web.UI.WebControls.RegularExpressionValidator
		Public Sub New()
			MyBase.ValidationExpression = "^\d{5}(-?\d{4})?$"
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Return MyBase.EvaluateIsValid()
		End Function
	End Class

	'Custom Validators
	Public Class PhoneValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf Me.FindControl(Me.ControlToValidate) Is Phone Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim sPhone As String = (CType(Me.FindControl(Me.ControlToValidate), Phone)).Value

			' Return true if phone is empty
			If sPhone = String.Empty Then
				Return True
			End If

			Dim sPattern As String = "^\d{3}-\d{3}-\d{4}$"
			Return Regex.IsMatch(sPhone, sPattern, RegexOptions.IgnoreCase)
		End Function

		Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
			If (Me.EnableClientScript) Then
				Me.ClientScript()
			End If
			MyBase.OnPreRender(e)
		End Sub

		Protected Sub ClientScript()
			Me.Attributes("evaluationfunction") = "isValidPhone"

			Dim validatorScript As Text.StringBuilder = New Text.StringBuilder()

			validatorScript.Append("<script language=""javascript"">" & vbCrLf)
			validatorScript.Append("function isValidPhone(val) {" & vbCrLf)
			validatorScript.Append("    var phone1 = document.all[val.controltovalidate + '_PHONE1'].value;" & vbCrLf)
			validatorScript.Append("    var phone2 = document.all[val.controltovalidate + '_PHONE2'].value;" & vbCrLf)
			validatorScript.Append("    var phone3 = document.all[val.controltovalidate + '_PHONE3'].value;" & vbCrLf & vbCrLf)
			validatorScript.Append("    if ((phone1 == '' || phone1 == null) && (phone2 == '' || phone2 == null) && (phone3 == '' || phone3 == null))" & vbCrLf)
			validatorScript.Append("        return true;" & vbCrLf & vbCrLf)
			validatorScript.Append("    var phone = phone1 + '-' + phone2 + '-' + phone3;" & vbCrLf)
			validatorScript.Append("    regexp = /^\d{3}-\d{3}-\d{4}$/" & vbCrLf)
			validatorScript.Append("    return regexp.test(phone);" & vbCrLf)
			validatorScript.Append("}" & vbCrLf)
			validatorScript.Append("</script>")

			If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("isValidPhone") Then
				Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "isValidPhone", validatorScript.ToString())
			End If
		End Sub
	End Class

	Public Class CreditCardValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf Me.FindControl(Me.ControlToValidate) Is System.Web.UI.WebControls.TextBox Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim sText As String = CType(Me.FindControl(Me.ControlToValidate), System.Web.UI.WebControls.TextBox).Text
			Return isCreditCardNumber(sText)
		End Function

		Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
			If Me.EnableClientScript Then
				'Me.RegClientScript()
			End If
			MyBase.OnPreRender(e)
		End Sub

		Protected Sub RegClientScript()
			Me.Attributes("evaluationfunction") = "isCreditCardNumber"
			Dim validatorScript As New System.Text.StringBuilder

			validatorScript.Append("function isCreditCardNumber(val) {" & vbCrLf)
			validatorScript.Append("	var iChkSum=0;" & vbCrLf)
			validatorScript.Append("	var ctrl = document.all[val.controltovalidate];" & vbCrLf)
			validatorScript.Append("	var ccnum = ctrl.value;" & vbCrLf & vbCrLf)

			validatorScript.Append("	ccnum = ccnum.replace( /\D/g, """" );" & vbCrLf)
			validatorScript.Append("	if (ccnum.length<13) return false;" & vbCrLf)
			validatorScript.Append("    ccnumchk=new Array;" & vbCrLf)
			validatorScript.Append("	for (iLoop=0; iLoop < ccnum.length; iLoop++) {" & vbCrLf)
			validatorScript.Append("		ccnumchk[ccnum.length-1-iLoop] = ccnum.substring(iLoop, iLoop+1);" & vbCrLf)
			validatorScript.Append("	}" & vbCrLf & vbCrLf)

			validatorScript.Append("    var skemp=0;" & vbCrLf)
			validatorScript.Append("	for (iLoop=0; iLoop < ccnum.length; iLoop++) {" & vbCrLf)
			validatorScript.Append("        if (iLoop %2 != 0) {" & vbCrLf)
			validatorScript.Append("			ccnumchk[iLoop]=ccnumchk[iLoop]*2;" & vbCrLf)
			validatorScript.Append("			if (ccnumchk[iLoop] >= 10) ccnumchk[iLoop]=ccnumchk[iLoop]-9;" & vbCrLf)
			validatorScript.Append("		}" & vbCrLf)
			validatorScript.Append("		ccnumchk[iLoop]++;" & vbCrLf)
			validatorScript.Append("		ccnumchk[iLoop]--;" & vbCrLf)
			validatorScript.Append("		iChkSum = iChkSum + ccnumchk[iLoop].valueOf();" & vbCrLf)
			validatorScript.Append("	}" & vbCrLf)
			validatorScript.Append("	if (iChkSum%10 != 0) { return false; }" & vbCrLf)
			validatorScript.Append("	return true;" & vbCrLf)
			validatorScript.Append("}" & vbCrLf)
			validatorScript.Append("</script>")

			If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("isCreditCardNumber") Then
				Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "isCreditCardNumber", validatorScript.ToString())
			End If
		End Sub

		Private Function isCreditCardNumber(ByVal ccnum As String)
			Dim iChkSum = 0
			ccnum = Regex.Replace(ccnum, "\D", "", RegexOptions.IgnoreCase)

			'Check for correct card number length
			If ccnum.Length < 13 Then Return False

			If ccnum(0) = "4" Then
				'VISA
				If (ccnum.Length <> 13 And ccnum.Length <> 16) Then Return False
			ElseIf ccnum(0) = "3" Then
				'AmEx
				If ccnum.Length <> 15 Then Return False
			ElseIf ccnum(0) = "6" Then
				'Discover
				If ccnum.Length <> 16 Then Return False
			ElseIf ccnum(0) = "5" Then
				'MasterCard
				If ccnum.Length <> 16 Then Return False
			End If

			'Make an array and fill it with the individual digits of the cc number
			Dim ccnumchk() As Integer = New Integer(ccnum.Length) {}
			Dim iLoop As Integer
			For iLoop = 0 To ccnum.Length - 1
				ccnumchk(ccnum.Length - 1 - iLoop) = Int32.Parse(ccnum.Substring(iLoop, 1))
			Next

			'Perform the mathematical method (some base 10 stuff) to
			'convert the number to a two digit number
			For iLoop = 0 To ccnum.Length - 1
				'If splits an even number
				If iLoop Mod 2 <> 0 Then
					ccnumchk(iLoop) = ccnumchk(iLoop) * 2
					If ccnumchk(iLoop) >= 10 Then ccnumchk(iLoop) = ccnumchk(iLoop) - 9
				End If

				'Switch ccnumchk[splits] to a number
				ccnumchk(iLoop) = ccnumchk(iLoop) + 1
				ccnumchk(iLoop) = ccnumchk(iLoop) - 1

				iChkSum = iChkSum + ccnumchk(iLoop)
			Next

            'If iChkSum Mod 10 <> 0 Then Return False 'The result isn't base 10

			Return True
		End Function
	End Class

	Public Class RequiredZipValidator
		Inherits ZipValidator

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim sZip As String = (CType(Me.FindControl(Me.ControlToValidate), Zip)).Value
			Dim sPattern As String = "^\d{5}(-?\d{4})?$"
			If sZip = String.Empty Then Return False
			Return Regex.IsMatch(sZip, sPattern, RegexOptions.IgnoreCase)
		End Function
	End Class

	Public Class ZipValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf Me.FindControl(Me.ControlToValidate) Is Zip Then
				Return True
			Else
				Return False
			End If
		End Function

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim sZip As String = (CType(Me.FindControl(Me.ControlToValidate), Zip)).Value
			Dim sPattern As String = "^\d{5}(-?\d{4})?$"
			Return sZip = String.Empty Or Regex.IsMatch(sZip, sPattern, RegexOptions.IgnoreCase)
		End Function

		Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
			If (Me.EnableClientScript) Then
				Me.ClientScript()
			End If
			MyBase.OnPreRender(e)
		End Sub

		Protected Sub ClientScript()
			Me.Attributes("evaluationfunction") = "isValidZip"

			Dim validatorScript As Text.StringBuilder = New Text.StringBuilder()
			validatorScript.Append("<script language=""javascript"">")
			validatorScript.Append("function isValidZip(val) {" & vbCrLf)
			validatorScript.Append("    var z5 = document.all[val.controltovalidate + '_ZIP5'].value;" & vbCrLf)
			validatorScript.Append("    var z4 = document.all[val.controltovalidate + '_ZIP4'].value;" & vbCrLf)
			validatorScript.Append("    var z;" & vbCrLf & vbCrLf)
			validatorScript.Append("    if ((z5 == null || z5 == '') && (z4 == null || z4 == ''))" & vbCrLf)
			validatorScript.Append("        return true;" & vbCrLf & vbCrLf)
			validatorScript.Append("    if (z4 == null || z4 == '') {" & vbCrLf)
			validatorScript.Append("        z = z5;" & vbCrLf)
			validatorScript.Append("    } else {" & vbCrLf)
			validatorScript.Append("        z = z5 + '-' + z4;" & vbCrLf)
			validatorScript.Append("    }" & vbCrLf)
			validatorScript.Append("    regexp = /^\d{5}(-?\d{4})?$/" & vbCrLf)
			validatorScript.Append("    return regexp.test(z);" & vbCrLf)
			validatorScript.Append("}" & vbCrLf)
			validatorScript.Append("</script>")

			If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("isValidZip") Then
				Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "isValidZip", validatorScript.ToString())
			End If
		End Sub
	End Class

#End Region

#Region " Front End Validators "

	Public Class FrontValidator
		Private m_Page As Page
		Private m_Validator As BaseValidator
		Private m_ControlToValidate As String
		Private m_Bar As HtmlControl
		Private m_Label As HtmlControl
		Private m_Viewstate As StateBag

		Sub New()
		End Sub

		Sub New(ByVal p As BasePage, ByVal c As WebControl, ByVal s As StateBag, ByVal v As String)
			m_Page = p
			m_Validator = c
			m_ControlToValidate = v
			m_Viewstate = s
		End Sub

		Private Property Bar() As HtmlControl
			Get
				Return m_Bar
			End Get
			Set(ByVal value As HtmlControl)
				m_Bar = value
			End Set
		End Property

		Private Property Label() As HtmlControl
			Get
				Return m_Label
			End Get
			Set(ByVal value As HtmlControl)
				m_Label = value
			End Set
		End Property

		Private Property Viewstate() As StateBag
			Get
				Return m_Viewstate
			End Get
			Set(ByVal value As StateBag)
				m_Viewstate = value
			End Set
		End Property

		Public Sub Initialize()
			Bar = GetBar()
			Label = GetLabel()

			Validator.EnableClientScript = False
			Validator.Display = ValidatorDisplay.None

			If Not Bar Is Nothing Then
				If Not p.IsPostBack Then
					Viewstate("orig" + Bar.ID) = Bar.Attributes("class")
				End If
				Bar.Attributes("class") = Viewstate("orig" + Bar.ID)
			End If
			If Not Label Is Nothing Then
				If Not p.IsPostBack Then
					Viewstate("orig" + Label.ID) = Label.Attributes("class")
				End If
				Label.Attributes("class") = Viewstate("orig" + Label.ID)
			End If
		End Sub

		Public Property p() As Page
			Get
				Return m_Page
			End Get
			Set(ByVal value As Page)
				m_Page = value
			End Set
		End Property

		Public Property Validator() As BaseValidator
			Get
				Return m_Validator
			End Get
			Set(ByVal value As BaseValidator)
				m_Validator = value
			End Set
		End Property

		Public Property ControlToValidate() As String
			Get
				Return m_ControlToValidate
			End Get
			Set(ByVal value As String)
				m_ControlToValidate = value
			End Set
		End Property

		Private Function GetBar() As HtmlControl
			Dim m_Bar As HtmlControl = Nothing
			Dim Bar As String = "bar" + ControlToValidate
			If TypeOf Validator.FindControl(Bar) Is HtmlGenericControl Then
				m_Bar = (CType(Validator.FindControl(Bar), HtmlGenericControl))
			End If
			If TypeOf Validator.FindControl(Bar) Is HtmlTableCell Then
				m_Bar = (CType(Validator.FindControl(Bar), HtmlTableCell))
			End If
			Return m_Bar
		End Function

		Private Function GetLabel() As HtmlControl
			Dim m_Label As HtmlControl = Nothing
			Dim Label As String = "label" + ControlToValidate
			If TypeOf Validator.FindControl(Label) Is HtmlGenericControl Then
				m_Label = (CType(Validator.FindControl(Label), HtmlGenericControl))
			End If
			If TypeOf Validator.FindControl(Label) Is HtmlTableCell Then
				m_Label = (CType(Validator.FindControl(Label), HtmlTableCell))
			End If
			Return m_Label
		End Function

		Public Sub ChangeStyles()
			If Not Bar Is Nothing Then
				If Not Viewstate("orig" + Bar.ID) Is Nothing Then
					Bar.Attributes("class") = Viewstate("orig" + Bar.ID).ToString().Replace("req", "red")
				End If
			End If
			If Not Label Is Nothing Then
				Label.Attributes("class") = "fielderror"
			End If
		End Sub
	End Class

	Public Class RequiredFieldValidatorFront
		Inherits System.Web.UI.WebControls.RequiredFieldValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)

			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function

	End Class

	Public Class DateValidatorFront
		Inherits DateValidator
		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class RequiredExpDateValidatorFront
		Inherits RequiredExpDateValidator
		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class ExpDateValidatorFront
		Inherits ExpDateValidator
		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class RequiredDateValidatorFront
		Inherits RequiredDateValidator
		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class PhoneValidatorFront
		Inherits PhoneValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class RequiredPhoneValidatorFront
		Inherits PhoneValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If CType(Me.FindControl(Me.ControlToValidate), Phone).Value <> String.Empty AndAlso MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class RequiredExpDateValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
		End Sub

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is ExpDate Then Return True Else Return False
		End Function

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim Value As String = CType(Me.FindControl(Me.ControlToValidate), ExpDate).Value
			If Value = String.Empty Then Return False
			Return True
		End Function
	End Class

	Public Class ExpDateValidator
		Inherits System.Web.UI.WebControls.BaseValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
		End Sub

		Protected Overrides Function ControlPropertiesValid() As Boolean
			If TypeOf FindControl(ControlToValidate) Is ExpDate Then Return True Else Return False
		End Function

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim bInvalid As Boolean = False
			Dim sDate As String = CType(Me.FindControl(Me.ControlToValidate), ExpDate).Value
			If (sDate = String.Empty) Then
				Return True
			ElseIf DateDiff(DateInterval.Month, Convert.ToDateTime(sDate), Now) > 0 Then
				bInvalid = True
			End If

			If bInvalid = False Then
				Return True
			Else
				Return False
			End If
		End Function
	End Class

	Public Class RequiredZipValidatorFront
		Inherits ZipValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			Dim sZip As String = CType(Me.FindControl(Me.ControlToValidate), Zip).Value
			If MyBase.EvaluateIsValid() And sZip <> "" Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class ZipValidatorFront
		Inherits ZipValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class EmailValidatorFront
		Inherits EmailValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class PasswordValidatorFront
		Inherits PasswordValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class CompareValidatorFront
		Inherits CompareValidator

		Private validator As FrontValidator
		Private validator2 As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
			validator2 = New FrontValidator(Page, Me, ViewState, ControlToCompare)
			validator2.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				If Enabled Then validator2.ChangeStyles()
				Return False
			End If
		End Function
	End Class

	Public Class CustomValidatorFront
		Inherits CustomValidator

		Private validator As FrontValidator

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			MyBase.OnLoad(e)
			validator = New FrontValidator(Page, Me, ViewState, ControlToValidate)
			validator.Initialize()
		End Sub

		Protected Overrides Function EvaluateIsValid() As Boolean
			If MyBase.EvaluateIsValid() Then
				Return True
			Else
				If Enabled Then validator.ChangeStyles()
				Return False
			End If
		End Function
	End Class
#End Region

End Namespace
