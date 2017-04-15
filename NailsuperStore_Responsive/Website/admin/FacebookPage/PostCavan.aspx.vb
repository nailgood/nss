Imports System.IO
Imports System.Net

Imports System.Text

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports Twitter
Partial Class admin_FacebookPage_PostCavan
    Inherits System.Web.UI.Page
  
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Dim isCavan As String = ""
            Try
                isCavan = Request.QueryString("isCavan")
            Catch ex As Exception
            End Try
            If (isCavan = "1" Or isCavan = "1/") Then
                Exit Sub
            End If
            Dim webRoot As String = ConfigurationManager.AppSettings("GlobalRefererName")
            Dim AppSecret As String = Utility.ConfigData.FaceBookSecretId
            'Your Oauth Secret Key
            Dim AppID As String = Utility.ConfigData.FaceBookAppId
            'Your app ID
            Dim Redirect As String = ""
            ' Your redirect URL, which is this page
            Redirect = webRoot & "/admin/FacebookPage/PostCavan.aspx/"
            ''Redirect = "http://localhost:2010/admin/FacebookPage/PostCavan.aspx/"
            Dim Code As String = IIf(Request.QueryString("code") IsNot Nothing, Request.QueryString("code"), Nothing)

            Response.ContentType = "text/html"
            Response.ContentEncoding = Encoding.UTF8

            If String.IsNullOrEmpty(Code) Then
                Dim codeRedirect As String = "http://www.facebook.com/dialog/oauth?client_id=" & AppID & "&redirect_uri=" & Redirect & "&scope=offline_access,email,read_stream,publish_stream"
                Response.Redirect(codeRedirect)
                Response.End()
            End If

            Dim Token_URL As String = "https://graph.facebook.com/oauth/access_token?client_id=" & AppID & "&redirect_uri=" & Redirect & "&client_secret=" & AppSecret & "&code=" & Code

            Dim shortUrl As String = New StreamReader(WebRequest.Create(Token_URL).GetResponse().GetResponseStream()).ReadToEnd()


            Dim writer As New StreamWriter(Response.OutputStream)
            Dim data As NameValueCollection = HttpUtility.ParseQueryString(shortUrl)
            Dim token As String = data("access_token")

            Dim SessionGetFacebookTokenAccess As String = String.Empty
            Try
                SessionGetFacebookTokenAccess = Session("GetFacebookTokenAccess")
            Catch ex As Exception

            End Try
            Session("GetFacebookTokenAccess") = Nothing
            If SessionGetFacebookTokenAccess = "1" Then
                Response.Redirect("GetFacebookToken.aspx?FacebookTokenAccess=" & token)
            Else
                Response.Write(token)
                '' GetListPagePostToFaceBook(token, webRoot)
                writer.Flush()
                Response.End()
            End If
           

        End If
    End Sub
    Private Sub PostLinkFaceBook(ByVal objData As FacebookPageRow, ByVal token As String)
        Try
            Dim myFBAPI As New Facebook.FacebookAPI(token)
            Dim data As New System.Collections.Generic.Dictionary(Of String, String)()
            data.Add("message", objData.PageTitle)
            data.Add("link", objData.Link)
            data.Add("picture", objData.Thumb)
            data.Add("description", objData.MetaDescription)
            myFBAPI.Post("/me/feed", data)
        Catch ex As Exception

        End Try
        
    End Sub
    Private Sub GetListPagePostToFaceBook(ByVal token As String, ByVal webRoot As String)
        Email.SendError("ToErrorFacebook", "Offline Token Access from Facebook", token)

        ''get Twitter token
        Dim objTwitterToken As New OAuthData(Utility.ConfigData.TwitterConsumerKey, Utility.ConfigData.TwitterConsumerSecret, Utility.ConfigData.TwitterUserID, Utility.ConfigData.TwitterScreenName, Utility.ConfigData.TwitterToken, Utility.ConfigData.TwitterTokenSecret)

        Dim res As DataTable = DirectCast(Session("PostFaceBookData"), DataTable)
        If Not res Is Nothing And token <> "" Then
            For Each row As DataRow In res.Rows
                Dim objData As New FacebookPageRow
                objData.Link = row("Link").ToString()
                objData.PageTitle = row("PageTitle").ToString()
                objData.Thumb = webRoot & "/" & Utility.ConfigData.SharefaceBookPath & row("Thumb").ToString()
                objData.MetaDescription = row("MetaDescription").ToString()
                PostLinkFaceBook(objData, token)
                PostToTwitter(objTwitterToken, objData)
            Next
        End If
        Session("PostFaceBookData") = Nothing
        Session("IsPostData") = Nothing
        Response.Redirect("Default.aspx?postsuccessfull=1")
    End Sub
    Private Sub PostToTwitter(ByVal objTwitterToken As OAuthData, ByVal objData As FacebookPageRow)
        Try
            Dim twitter As New Twitter.TwitterPush(objTwitterToken)
            Dim response As Twitter.TwitterResponse = twitter.UpdateStatus(objData.PageTitle & " " & objData.Link)
        Catch ex As Exception

        End Try


    End Sub
End Class
