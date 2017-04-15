Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace MasterPages

	<ToolboxData("<{0}:ContentRegion runat=server></{0}:ContentRegion>")> _
	Public Class ContentRegion
		Inherits System.Web.UI.WebControls.Panel

		Public Overrides Property Width() As Unit
			Get
				Return MyBase.Width
			End Get
			Set(ByVal Value As Unit)
				MyBase.Width = Value
			End Set
		End Property

		Public Sub New()
		End Sub

		Public Property DefultContent() As Boolean
			Get
				Return ViewState("DefaultContent")
			End Get
			Set(ByVal Value As Boolean)
				ViewState("DefaultContent") = Value
			End Set
		End Property

		Public Overrides Sub RenderBeginTag(ByVal writer As System.Web.UI.HtmlTextWriter)
		End Sub
		Public Overrides Sub RenderEndTag(ByVal writer As System.Web.UI.HtmlTextWriter)
		End Sub

	End Class

End Namespace
