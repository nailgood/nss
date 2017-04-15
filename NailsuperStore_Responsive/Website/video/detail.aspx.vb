Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports System.IO
Imports System.Collections.Generic

Partial Class videos_Detail
    Inherits SitePage
    Protected VideoId As Integer
    Public dbVideo As New VideoRow
    Public videoURLImg As String = String.Empty
    Public embedUrl As String = String.Empty
    Public html5Video As String = String.Empty
    Public flashVideoURL As String = String.Empty
    Private ReviewType As Integer = Utility.Common.ReviewType.Video
    Public AddthisAssociatedUrl As String = Utility.ConfigData.GlobalRefererName
    Public AddthisAssociatedName As String
    Protected TotalComment As Integer = 0
    Protected TotalLike As Integer = 0
    Public likeFB As New NailCache()
    Dim url As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request("VideoId") <> Nothing Then
            VideoId = Request("VideoId")
            Session("VideoId") = VideoId
        End If

        'If Not IsPostBack Then
        '    BindData()
        'End If
        BindData()
        rvComment.Type = ReviewType
        rvComment.ItemReviewId = VideoId
        TotalComment = ReviewRow.GetTotalReviewByItemReviewId(ReviewType, VideoId)

    End Sub

    Private Function CreateLinkEmbedVideo(ByVal video As VideoRow) As String
        Dim result As String = "<iframe width=""{0}"" height=""{1}"" scrolling=""no""  src=""" & url & "/embed/how-to-video/" & video.VideoId & """ frameborder=""0""></iframe>"
        'hidVideoEmbedLink.Value = result
        Dim w As Integer = 560
        Dim h As Integer = 315
        Dim size As String = drlSize.SelectedValue
        If Not String.IsNullOrEmpty(size) Then
            Dim arr() As String = size.Split("-")
            If arr.Length > 0 Then
                w = arr(0).ToString()
                h = arr(1).ToString
            End If
        End If
        Return String.Format(result, w, h)
    End Function
    'Private Sub SetValhidembed(ByVal video As VideoRow)
    '    'hidVideoEmbedLink.Value = "<iframe width=""{0}"" height=""{1}"" scrolling=""no""  src=""" & url & "/embed/how-to-video/" & video.VideoId & """ frameborder=""0""></iframe>"
    'End Sub
    Private Sub BindData()

        dbVideo = VideoRow.GetRow(DB, VideoId)
        If Not dbVideo Is Nothing AndAlso Not String.IsNullOrEmpty(dbVideo.Title) Then

            Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(dbVideo.Title.ToLower())))
            Dim CateId As Integer = DB.ExecuteScalar("Select CategoryId from VideoCategory where VideoId=" & VideoId)
            Session("VideoCateId") = CateId '' use in breadcrumb

            'Hien edit: url img video
            Dim imagePath As String = Server.MapPath(Utility.ConfigData.VideoThumbPath)
            If System.IO.File.Exists(imagePath & dbVideo.ThumbImage) Then
                videoURLImg = Utility.ConfigData.VideoThumbPath & dbVideo.ThumbImage
            End If

            If (dbVideo.VideoFile.Contains("youtube.com/")) Then
                ltVideo.Text = Utility.Common.GetVideoResource(dbVideo.VideoFile, 754, 434, 1, 0)
            Else
                flashVideoURL = dbVideo.VideoFile
                ltVideo.Text = Utility.Common.ConvertHTML5Video(flashVideoURL, dbVideo.SubTitle, "videodetail")
            End If

            ltDate.Text = String.Format("{0:MMM d, yyyy}", Convert.ToDateTime(dbVideo.CreatedDate)) & "<meta itemprop='uploadDate' content='" & String.Format("{0:yyyy-MM-ddThh:mm:ss}", Convert.ToDateTime(dbVideo.CreatedDate)) & "' />"
            ltrViewCount.Text = dbVideo.ViewsCount.ToString()
            TotalLike = likeFB.BindCountLikeFB(ConfigurationManager.AppSettings("GlobalSecureName") & URLParameters.VideoDetailUrl(dbVideo.Title, dbVideo.VideoId))
            ltFullDesc.Text = dbVideo.ShortDescription.Replace(Environment.NewLine, "<Br>")

            ltTitle.Text = Server.HtmlEncode(dbVideo.Title)

            'Related Video
            Dim Related As VideoCollection = VideoRow.ListRelatedByVideoId(VideoId)
            If Related.Count < 1 Then
                tabnav_video.Style.Value = "display:none;"
            Else
                ''rlVideo.Fill(Related, 0)
                rlVideo.countPageSize = 0
                rlVideo.VideoList = Related
            End If

            'Related Item
            ucRelatedItem.VideoId = dbVideo.VideoId

            dbVideo.ViewsCount = dbVideo.ViewsCount + 1
            VideoRow.Update(DB, dbVideo)
            'txtEmbed.Text = CreateLinkEmbedVideo(dbVideo)
            ltEmbedUrl.Text = "<meta content=""" & url & "/embed/how-to-video/" & dbVideo.VideoId & """ itemprop=""embedURL""/>"
            '''''''set MetaTag danh cho mang xa hoi khi share
            SetSocialNetwork(dbVideo)
        Else
            Utility.Common.Redirect301("/video-topic")
        End If
    End Sub

    Private Sub SetSocialNetwork(ByVal objItem As VideoRow)
        Dim objMetaTag As New MetaTag
        objMetaTag.MetaKeywords = objItem.MetaKeyword
        objMetaTag.PageTitle = objItem.PageTitle
        objMetaTag.MetaDescription = objItem.MetaDescription

        Dim shareURL As String = GlobalSecureName & URLParameters.VideoDetailUrl(objItem.Title, objItem.VideoId)
        Dim shareTitle As String = objItem.Title
        Dim shareDesc As String = Utility.Common.RemoveHTMLTag(objItem.ShortDescription)
        Dim videoFile As String = GlobalSecureName & objItem.VideoFile
        Dim embedUrl As String = GlobalSecureName & "/embed/how-to-video/" & objItem.VideoId

        objMetaTag.TypeShare = "video"
        objMetaTag.EmbedUrl = embedUrl
        objMetaTag.ImageName = objItem.ThumbImage
        objMetaTag.ImagePath = Utility.ConfigData.VideoThumbPath
        objMetaTag.ImgHeight = 202
        objMetaTag.ImgWidth = 360
        objMetaTag.ShareDesc = shareDesc
        objMetaTag.ShareTitle = shareTitle
        objMetaTag.ShareURL = shareURL
        objMetaTag.VideoFile = videoFile

        SetPageMetaSocialNetwork(Page, objMetaTag)
        ushare.shareURL = shareURL
        'ushare.shareDescription = shareDesc
    End Sub

    'Private Sub LoadRelatedItem(ByVal VideoId As Integer)
    '    Dim filter As New DepartmentFilterFields()
    '    Filter.pg = 1
    '    Filter.MaxPerPage = Integer.MaxValue
    '    Filter.OrderId = IIf(Session("OrderId") <> Nothing, Session("OrderId"), 0)
    '    Filter.MemberId = Utility.Common.GetCurrentMemberId()
    '    Filter.SortBy = "related.Arrange"
    '    Filter.SortOrder = "ASC"

    '    Dim RecordCount As Integer = 0
    '    Dim lstRelated As StoreItemCollection = New StoreItemCollection

    '    lstRelated = StoreItemRow.GetItemByVideoId(DB, VideoId, filter, RecordCount)
    '    If lstRelated.Count < 1 Then
    '        tabnav_item.Style.Value = "display:none;"
    '        ucItem.Visible = False
    '    Else
    '        ucItem.VideoId = VideoId
    '        ucItem.ItemCollection = lstRelated
    '    End If
    'End Sub

End Class
