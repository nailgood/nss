
Partial Class admin_FacebookPage_GetFacebookToken
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Dim token As String = String.Empty
            Try
                token = Request.QueryString("FacebookTokenAccess")
            Catch ex As Exception

            End Try
            If token Is Nothing Then
                Session("GetFacebookTokenAccess") = "1"
                Response.Redirect("PostCavan.aspx")
            End If
            If token = String.Empty Then
                Session("GetFacebookTokenAccess") = "1"
                Response.Redirect("PostCavan.aspx")
            End If
            Response.Write(token)
        End If
    End Sub
End Class
