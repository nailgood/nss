Imports System.Web.UI

Namespace Controls

    <ToolboxData("<{0}:ConfirmButton runat=server></{0}:ConfirmButton>")> _
    Public Class ConfirmButton
        Inherits System.Web.UI.WebControls.Button

        Public Sub New()
            MyBase.New()
        End Sub

        Public Property Message() As String
            Get
                Return CType(ViewState("Message"), String)
            End Get
            Set(ByVal Value As String)
                ViewState("Message") = Value
            End Set
        End Property

        Protected Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
            Attributes.Add("onclick", "if (!confirm('" + Message.Replace("'", "\\'") + "')) return false;")
            MyBase.AddAttributesToRender(writer)
        End Sub

    End Class

End Namespace
