Imports System.Web.UI
Imports System.web.Caching

Namespace Components

	Public Class BaseControl
		Inherits System.Web.UI.UserControl

        Private Shared m_Db As Database

		Public Sub New()
		End Sub

        Protected Property DB() As Database
            Get
                If m_Db Is Nothing Then
                    'open database connection
                    m_Db = New Database

                    'm_DB.Open(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
                    m_Db.Open(System.Configuration.ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                Else
                    If m_Db.Connection Is Nothing Then
                        m_Db.Open(System.Configuration.ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                    End If
                End If

                Return m_Db
            End Get
            Set(ByVal value As Database)
                m_Db = value
            End Set
        End Property

		Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
			If TypeOf Page Is BasePage Then
				DB = CType(Me.Page, BasePage).DB
			End If
		End Sub

		Protected Sub CheckPostData(ByVal controls As ControlCollection)
			For Each ctrl As Control In controls
				If TypeOf ctrl Is IPostBackDataHandler Then
					Dim hnd As IPostBackDataHandler = CType(ctrl, IPostBackDataHandler)
					On Error Resume Next
					hnd.LoadPostData(ctrl.UniqueID, Request.Form)
					On Error GoTo 0
				End If
				If ctrl.HasControls Then
					CheckPostData(ctrl.Controls)
				End If
			Next
		End Sub

		Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			If InStr(Request.Path, "/admin/") <= 0 Then Exit Sub

			If Parent Is Nothing Then Exit Sub
			If Not TypeOf Parent Is BasePartialCachingControl Then Exit Sub

			Dim c As BasePartialCachingControl = Parent
			If Not c Is Nothing Then
				Dim dep As New CacheDependency(Nothing, New String() {""}, Now.AddMinutes(-1))
				c.Dependency = dep
			End If
        End Sub

        Public Shared Function GetQueryString(ByVal QueryStringName) As String
            Return BasePage.GetQueryString(QueryStringName)
        End Function

        Public Shared Function ChecNumeric(ByVal QueryStringName) As String
            Dim i As Integer
            Return (Integer.TryParse(QueryStringName, i))

        End Function
    End Class

End Namespace