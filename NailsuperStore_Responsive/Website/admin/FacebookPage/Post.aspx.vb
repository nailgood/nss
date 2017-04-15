Imports System.IO
Imports System.Text
Imports System.Net
Imports DataLayer
Partial Class admin_FacebookPage_Post
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Dim post As String = Session("IsPostData")
            If post = "1" Then
                ''Get token
                Dim token As String = ""
                Try
                    token = Request.QueryString("token")
                Catch ex As Exception

                End Try
                If token Is Nothing Or token = "" Then
                    Response.Redirect("PostCavan.aspx")
                End If
                Dim webRoot As String = ConfigurationManager.AppSettings("GlobalRefererName")
                GetListPagePostToFaceBook(token, webRoot)
            End If
        End If
    End Sub
    Private Sub PostLinkFaceBook(ByVal objData As FacebookPageRow, ByVal token As String)
        Dim myFBAPI As New Facebook.FacebookAPI(token)
        Dim data As New System.Collections.Generic.Dictionary(Of String, String)()
        data.Add("message", objData.PageTitle)
        data.Add("link", objData.Link)
        data.Add("picture", objData.Thumb)
        data.Add("description", objData.MetaDescription)
        myFBAPI.Post("/me/feed", data)
    End Sub
    Private Sub GetListPagePostToFaceBook(ByVal token As String, ByVal webRoot As String)
        Dim res As DataTable = DirectCast(Session("PostFaceBookData"), DataTable)
        If Not res Is Nothing And token <> "" Then
            For Each row As DataRow In res.Rows
                Dim objData As New FacebookPageRow
                objData.Link = row("Link").ToString()
                objData.PageTitle = row("PageTitle").ToString()
                objData.Thumb = webRoot & "/" & Utility.ConfigData.SharefaceBookPath & row("Thumb").ToString()
                objData.MetaDescription = row("MetaDescription").ToString()
                PostLinkFaceBook(objData, token)
            Next
        End If
        Session("PostFaceBookData") = Nothing
        Session("IsPostData") = Nothing
        Response.Redirect("Default.aspx?postsuccessfull=1")
    End Sub
  
End Class
