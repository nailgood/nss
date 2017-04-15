Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System

Partial Class Media_Detail
    Inherits SitePage
    Protected MediaId As Integer
    Public dbVideo As VideoRow
    Public mediaDate As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("MediaId") <> Nothing Then
                MediaId = Request("MediaId")
            End If
            BindData()
        End If

    End Sub
    Private Sub BindData()
        img.Visible = False
        dbVideo = VideoRow.GetRow(DB, MediaId)
        If dbVideo.Title Is Nothing Then
            ltTitle.Text = Resources.Msg.NotFoundMedia
            Dim ltIndexFollow As Literal = CType(Master.FindControl("ltIndexFollow"), Literal)

            SetIndexFollow(Nothing, ltIndexFollow, True)
            'SetPageMetaSocialNetwork(Page, Nothing)
        Else
            Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(dbVideo.Title.ToLower())))
            Dim CateId As Integer = DB.ExecuteScalar("Select CategoryId from VideoCategory where VideoId=" & MediaId)
            Session("MediaCateId") = CateId

            If dbVideo.ThumbImage <> "" Then
                Dim pathImg As String = Server.MapPath("~" & Utility.ConfigData.MediaPath)
                If System.IO.File.Exists(pathImg & dbVideo.ThumbImage) Then
                    img.ImageUrl = "~" & Utility.ConfigData.MediaPath & dbVideo.ThumbImage
                    img.Visible = True
                    ltrLinkImg.Text = "<a href='" & WebRoot & Utility.ConfigData.MediaPath & dbVideo.ThumbImage & "'>Link to image</a>"
                End If
            End If
            ltShortDes.Text = BBCodeHelper.ConvertBBCodeToHTML(dbVideo.ShortDescription.Replace(vbCrLf, "<br/>"))
            ltTitle.Text = Server.HtmlEncode(dbVideo.Title)
            dbVideo.ViewsCount = dbVideo.ViewsCount + 1
            VideoRow.Update(DB, dbVideo)
            mediaDate = String.Format("{0:yyyy-MM-dd}", dbVideo.CreatedDate)
            '''''''set MetaTag danh cho mang xa hoi khi share
            Dim objMetaTag As New MetaTag
            objMetaTag.MetaKeywords = dbVideo.MetaKeyword
            objMetaTag.PageTitle = dbVideo.PageTitle
            objMetaTag.MetaDescription = dbVideo.MetaDescription
            Dim shareURL As String = GlobalSecureName & URLParameters.NewsDetailUrl(dbVideo.Title, dbVideo.VideoId)
            Dim shareTitle As String = dbVideo.Title
            Dim shareDesc As String = Utility.Common.RemoveHTMLTag(dbVideo.ShortDescription)
            objMetaTag.TypeShare = "news"
            objMetaTag.ImageName = dbVideo.ThumbImage
            objMetaTag.ImagePath = Utility.ConfigData.MediaThumbPath
            objMetaTag.ImgHeight = 202
            objMetaTag.ImgWidth = 360
            objMetaTag.ShareDesc = shareDesc
            objMetaTag.ShareTitle = shareTitle
            objMetaTag.ShareURL = shareURL

            SetPageMetaSocialNetwork(Page, objMetaTag)
        End If
    End Sub

End Class
