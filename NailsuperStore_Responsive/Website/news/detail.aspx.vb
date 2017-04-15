Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System

Partial Class News_Detail
    Inherits SitePage
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------
    Public result As New NewsCollection
    Public objNew As NewsRow
    Public showAudio As Boolean = True
    Public ID As Integer
    Protected TotalComment As Integer = 0
    Private ReviewType As Integer = Utility.Common.ReviewType.News
    Public newsImg As String
    Public pathImg As String
    Public newsDate As String
    '-------------------------------------------------------------------
    ' METHODS
    '-------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("NewId") <> Nothing AndAlso IsNumeric(Request.QueryString("NewId")) Then
            ID = CInt(IIf(Request.QueryString("NewId").Contains(","), Request.QueryString("NewId").Split(",")(0), Request.QueryString("NewId")))
        Else
            Response.Redirect("/")
        End If
        If Not IsPostBack Then
            LoadDefault()
        End If
        rvComment.Type = ReviewType
        rvComment.ItemReviewId = ID
        TotalComment = ReviewRow.GetTotalReviewByItemReviewId(ReviewType, ID)
    End Sub
    Private Function LoadNewsAudio(ByVal newsID As Integer) As String
        Dim result As NewsAudioCollection = NewsAudioRow.GetByNewId(DB, newsID)
        If result.Count < 1 Then
            Return ""
        End If
        If (result(0).AudioId > 0) Then
            Dim audio As AudioRow = AudioRow.GetRow(DB, result(0).AudioId)
            If System.IO.File.Exists(Server.MapPath("/" & Utility.ConfigData.AudioPath) & audio.FileUrl) Then
                Return audio.FileUrl
            End If
        End If
        Return ""
    End Function
    Private Sub LoadDefault()
        'Dim ID As Integer
      
        Dim CateId As Integer = 0

        
        ''check blog
        CateId = DB.ExecuteScalar("Select CategoryId from NewsCategory where NewsId=" & ID)
        'If (CateId = Utility.ConfigData.BlogId) Then
        '    Response.Redirect("/news-topic")
        'End If
        objNew = NewsRow.GetRow(DB, ID)
        If Not objNew Is Nothing AndAlso Not String.IsNullOrEmpty(objNew.Title) Then
            Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(objNew.Title.ToLower())))
            Session("NewsCateId") = CateId
            objNew.CategoryId = CateId

            litTitle.Text = Server.HtmlEncode(objNew.Title)
            If objNew.Description Is Nothing Then
                divContent.InnerHtml = String.Empty
            Else
                divContent.InnerHtml = objNew.Description
            End If

            divContent.InnerHtml = Utility.Common.ConvertObjectVideoToIframe(divContent.InnerHtml)

            pathImg = IIf(CateId = 12, Utility.ConfigData.PathThumbBlogImage, Utility.ConfigData.PathThumbNewsImage)
            newsImg = pathImg & objNew.ThumbImage
            newsDate = String.Format("{0:yyyy-MM-dd}", objNew.CreatedDate)

            'Set MetaTag
            SetSocialNetwork(objNew, pathImg)
        End If
    End Sub
    Private Sub SetSocialNetwork(ByVal objItem As NewsRow, ByVal pathImg As String)
        Dim objMetaTag As New MetaTag
        Dim strUrl As String
        objMetaTag.MetaKeywords = objItem.MetaKeyword
        objMetaTag.PageTitle = objItem.PageTitle
        objMetaTag.MetaDescription = objItem.MetaDescription

        strUrl = IIf(objItem.CategoryId = 12, URLParameters.BlogDetailUrl(objItem.Title, objItem.NewsId), URLParameters.NewsDetailUrl(objItem.Title, objItem.NewsId))
        Dim shareURL As String = GlobalSecureName & strUrl
        Dim shareTitle As String = objItem.Title
        Dim shareDesc As String = Utility.Common.RemoveHTMLTag(objItem.ShortDescription)
        objMetaTag.TypeShare = "news"
        objMetaTag.ImageName = objItem.ThumbImage
        objMetaTag.ImagePath = pathImg
        objMetaTag.ImgHeight = 202
        objMetaTag.ImgWidth = 360
        objMetaTag.ShareDesc = shareDesc
        objMetaTag.ShareTitle = shareTitle
        objMetaTag.ShareURL = shareURL
        objMetaTag.Canonical = shareURL

        SetPageMetaSocialNetwork(Page, objMetaTag)
    End Sub

End Class
