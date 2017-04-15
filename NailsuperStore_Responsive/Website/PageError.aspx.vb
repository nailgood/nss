Imports Components
Partial Class PageError
    Inherits System.Web.UI.Page
    Public backURL As String = String.Empty
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session("LastWebsiteURL") Is Nothing) Then
            backURL = "/"
        Else
            backURL = Trim(Session("LastWebsiteURL"))
        End If
    End Sub

End Class
