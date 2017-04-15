Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace MasterPages

	<ToolboxData("<{0}:ErrorMessage runat=server></{0}:ErrorMessage>")> _
	Public Class ErrorMessage
		Inherits System.Web.UI.WebControls.PlaceHolder

        Private m_Message As String = String.Empty
        Private m_Title As String = String.Empty
		Private m_Conn As String = ""

		Public Sub New()
			Visible = False
		End Sub

		Public Sub AddError(ByVal s As String)
            Message &= "<li>" & s & "</li>"
		End Sub
        Public Sub AddErrorTitle(ByVal s As String)
            m_Title = s
        End Sub

		Public Property Message() As String
			Get
				Return m_Message
			End Get
			Set(ByVal Value As String)
				m_Message = Value
			End Set
		End Property

		Public Property Width() As Integer
			Get
				Return ViewState("Width")
			End Get
			Set(ByVal Value As Integer)
				ViewState("Width") = Value
			End Set
		End Property

		Public Sub UpdateSummary()
			Dim valCol As ValidatorCollection = Page.Validators
			For Each val As IValidator In valCol
                If Not val.IsValid AndAlso Not Message.Contains(val.ErrorMessage) Then
                    AddError(val.ErrorMessage)
                End If
			Next
		End Sub

		Public Sub UpdateVisibility()
			Visible = (Not Message = String.Empty)
		End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If m_Title Is Nothing Or m_Title = String.Empty Then
                m_Title = "This form was not processed due to the following reasons:"
            End If
            'writer.Write("<p></p>")
            writer.Write("<div id=""error"" class=""alert alert-danger fade in alert-dismissable"">")
            writer.Write("<button type=""button"" class=""close"" data-dismiss=""alert"" aria-hidden=""true"">&times;</button>")
            writer.Write("<p><strong>" & m_Title & "</strong><p>")
            writer.Write("<ul>")
            writer.Write(Message)
            writer.Write("</ul>")
            writer.Write("</div>")
        End Sub
	End Class

End Namespace