Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class controls_ResourceCenter
    Inherits ModuleControl

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Utility.ConfigData.IsEnableInforBanner() Then
                ucInforBanner.Visible = True
            Else
                ucInforBanner.Visible = False
            End If
        End If
    End Sub

End Class
