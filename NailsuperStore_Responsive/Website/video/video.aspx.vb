Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports Utility.Common
Imports System.IO
Imports System.Collections.Generic
Imports System.Web.Services

Partial Class Video_List
    Inherits SitePage
    Public objCate As CategoryRow

    Private result As New VideoCollection
    Protected PathThumbVideoImg As String = Utility.ConfigData.VideoThumbPath
    Protected PathLargeVideoImg As String = Utility.ConfigData.VideoLargePath
    Public arrCategory As New ArrayList()
    Public likeFB As New NailCache()
    Protected PageIndex As Integer = 1
    Protected PageSize As Integer = Utility.ConfigData.PageSizeScroll
    Protected TotalRecords As Integer = 0

    Public Property CategoryId() As Integer
        Get
            Return ViewState("CategoryId")
        End Get
        Set(ByVal value As Integer)
            ViewState("CategoryId") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Request.QueryString("cateId") <> Nothing AndAlso IsNumeric(Request.QueryString("cateId").ToString()) Then
                CategoryId = CInt(Request.QueryString("cateId").ToString())
            End If
            objCate = CategoryRow.GetRow(DB, CategoryId)
            If CategoryId > 0 Then
                Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(objCate.CategoryName.ToLower())))
                'litTitle.Text = objCate.CategoryName
                Dim objMetaTag As New MetaTag
                objMetaTag.PageTitle = objCate.PageTitle
                objMetaTag.MetaKeywords = objCate.MetaKeyword
                objMetaTag.MetaDescription = SetMetaDescription(objCate.MetaDescription, objCate.CategoryName)
                SetPageMetaSocialNetwork(Page, objMetaTag)
                'Else
                '    litTitle.Text = "Video Center"
            End If
            'InitPager()
            LoadData()
            ' LoadBanner()
        End If
    End Sub

    Public Function GetDefaultImage() As String
        Try
            Dim di As New DirectoryInfo(Server.MapPath(Utility.ConfigData.VideoBannerFolder))
            Dim rgFiles As FileInfo() = di.GetFiles()
            For Each fi As FileInfo In rgFiles
                If fi.Name.ToLower().Contains(Utility.ConfigData.DefaultVideoBannerName) Then
                    Return fi.Name
                End If
            Next
        Catch ex As Exception

        End Try
        Return String.Empty
    End Function

    Private Sub LoadData()
        Dim data As New VideoRow
        Dim lstCategory As New CategoryCollection
        Dim topVideo As VideoRow
        Dim linkTop As String = ""
        If CategoryId > 0 Then
            data.PageIndex = PageIndex
            data.PageSize = PageSize
            Data.Condition = " c.Type=" & Utility.Common.CategoryType.Video & " AND vi.IsActive=1  AND vc.CategoryId = " & CategoryId
            Data.OrderBy = "vc.Arrange"
            Data.OrderDirection = "DESC"
            topVideo = VideoRow.GetTopByCategoryId(CategoryId, Utility.Common.CategoryType.Video)

            ''top 1
            linkTop = URLParameters.VideoDetailUrl(topVideo.Title, topVideo.VideoId)
            If System.IO.File.Exists(Server.MapPath("~/" & PathThumbVideoImg & topVideo.ThumbImage)) Then
                ltrDivImageTop.Text = "<div id='image-top'><a href='" & linkTop & "'><img src='" & Utility.ConfigData.CDNMediaPath & PathLargeVideoImg & topVideo.ThumbImage & "' alt='" & topVideo.Title & "' /><div id='vlplay'></div></a></div>"
            Else
                ltrDivImageTop.Text = "<div id='image-top'><a href='" & linkTop & "'><img src='" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.VideoLargeNoImage & "' alt='" & topVideo.Title & "' /><div id='vlplay'></div></a></div>"
            End If
            hlTitleTop.Text = "<div id='title-top'><a href='" & linkTop & "'>" & topVideo.Title & "</a></div>"
            ltrDate.Text = String.Format("{0:MMM d, yyyy}", topVideo.CreatedDate)
            ltrView.Text = "<span id='iconview'></span>" & topVideo.ViewsCount
            ltrComment.Text = "<span id='icomment'></span>" & topVideo.ReviewsCount
            ltrVote.Text = "<span id='ivote'></span>" & likeFB.BindCountLikeFB(ConfigurationManager.AppSettings("GlobalSecureName") & URLParameters.VideoDetailUrl(topVideo.Title, topVideo.VideoId))
            ltrShortDesc.Text = Utility.Common.StringCut(topVideo.ShortDescription, 0, 380)

            result = VideoRow.ListByCatId(Data)
            objCate = IIf(objCate Is Nothing, CategoryRow.GetRow(DB, CategoryId), objCate)
            lstCategory.Add(objCate)
            TotalRecords = Data.TotalRow
        Else
            lstCategory = CategoryRow.GetAllVideoCategoryByType(DB, Utility.Common.CategoryType.Video)
            topVideo = VideoRow.GetTopByType(Utility.Common.CategoryType.Video)
            linkTop = URLParameters.VideoDetailUrl(topVideo.Title, topVideo.VideoId)
            If System.IO.File.Exists(Server.MapPath("~/" & PathThumbVideoImg & topVideo.ThumbImage)) Then
                ltrDivImageTop.Text = "<div id='image-top'><a href='" & linkTop & "'><img src='" & Utility.ConfigData.CDNMediaPath & PathLargeVideoImg & topVideo.ThumbImage & "' alt='" & topVideo.Title & "' /><div id='vlplay'></div></a></div>"
            End If
            hlTitleTop.Text = "<div id='title-top'><a href='" & linkTop & "'>" & topVideo.Title & "</a></div>"
            ltrDate.Text = String.Format("{0:MMM d, yyyy}", topVideo.CreatedDate)
            ltrView.Text = "<span id='iconview'></span>" & topVideo.ViewsCount
            ltrComment.Text = "<span id='icomment'></span>" & topVideo.ReviewsCount
            ltrVote.Text = "<span id='ivote'></span>" & likeFB.BindCountLikeFB(ConfigurationManager.AppSettings("GlobalSecureName") & URLParameters.VideoDetailUrl(topVideo.Title, topVideo.VideoId))
            ltrShortDesc.Text = Utility.Common.StringCut(topVideo.ShortDescription, 0, 380)
        End If

        rptVideo.DataSource = lstCategory
        rptVideo.DataBind()
    End Sub

    Protected Sub rptVideo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptVideo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim countPageSize As Integer = 0
            Dim item As CategoryRow = e.Item.DataItem
            Dim ucvideo As controls_resource_center_video_video_list = e.Item.FindControl("ucVideo")
            If CategoryId = 0 Then
                result = VideoRow.ListTop3ByCategoryId(item.CategoryId)
                Dim ltrmCategory As Literal = e.Item.FindControl("ltrmCategory")
                ltrmCategory.Text = "<div class='category'><a href='" & String.Format("/video-topic/{0}/{1}", URLParameters.ReplaceUrl(item.CategoryName.ToLower()), item.CategoryId) & "'>" & item.CategoryName & "</a></div>"
                Dim ltrViewMore As Literal = e.Item.FindControl("ltrViewMore")
                ltrViewMore.Text = "<div class='viewmore'><a href='" & String.Format("/video-topic/{0}/{1}", URLParameters.ReplaceUrl(item.CategoryName.ToLower()), item.CategoryId) & "'>View More</a></div>"
                countPageSize = e.Item.ItemIndex * 3
            Else
                countPageSize = (PageIndex - 1) * PageSize
            End If
            '' ucvideo.Fill(result, countPageSize)
            ucvideo.VideoList = result
            ucvideo.countPageSize = countPageSize
        End If
    End Sub

    Private Shared Function IsExistCategory(ByVal arrCategory As ArrayList, ByVal cateID As Integer) As Boolean
        For Each Str As Integer In arrCategory
            If Str = cateID Then
                Return True
            End If
        Next
        Return False
    End Function

    'Public Function LoadVideoByCategory(ByVal categoryId As Integer) As VideoCollection
    '    Dim objData As New VideoRow
    '    Dim lstResult As VideoCollection
    '    objData.OrderBy = "vc.Arrange"
    '    objData.OrderDirection = "ASC"
    '    objData.PageIndex = 1
    '    objData.PageSize = Integer.MaxValue
    '    objData.Condition = "vc.CategoryId=" & categoryId & " and vi.IsActive=1"
    '    objData.Condition = objData.Condition & " and cate.[Type]=2 and [dbo].[VideoInCategoryActive](vi.VideoId)=1"
    '    objData.CategoryId = categoryId
    '    lstResult = VideoRow.ListByCatId(objData)
    '    Return lstResult
    'End Function

    'Private Function GenerateVideoSlide(ByVal lstVideo As VideoCollection, ByVal categoryId As Integer) As String
    '    Dim result As String = "<div id='slide_" & categoryId & "'>"
    '    For Each v As VideoRow In lstVideo
    '        Dim detailURL As String = URLParameters.VideoDetailUrl(v.Title, v.VideoId)
    '        Dim img As String = GetImage(v.ThumbImage)
    '        Dim dateString As String = String.Format("{0:MMM d, yyyy} &nbsp;|&nbsp; {1} views", Convert.ToDateTime(v.CreatedDate), v.ViewsCount)
    '        result = result & "<div><a href='" & detailURL & "'><img alt='" & v.Title & "' src='" & img & "' /></a><br />"
    '        result = result & "<span class='thumbnail-text'><a href='" & detailURL & "'>" & Utility.Common.StringCut(v.Title, 0, 76) & "</a></span>"
    '        result = result & "<span class='thumbnail-date'>" & dateString & "</span></div>"

    '    Next
    '    result = result & "</div>"
    '    Return result
    'End Function

    'Protected Sub dlVideos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlVideos.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim cate As CategoryRow = e.Item.DataItem
    '        Dim ltrTitle As Literal
    '        ltrTitle = CType(e.Item.FindControl("ltrTitle"), Literal)
    '        If Not ltrTitle Is Nothing Then
    '            Dim link As String = "/video-topic/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(cate.CategoryName.ToLower())) & "/" & cate.CategoryId


    '            ltrTitle.Text = "<a href='" & link & "'>" & cate.CategoryName & "</a>"
    '        End If
    '        Dim ltrVideo As Literal = CType(e.Item.FindControl("ltrVideo"), Literal)
    '        If Not ltrVideo Is Nothing Then
    '            Dim lstVideo As VideoCollection = LoadVideoByCategory(cate.CategoryId)
    '            If Not lstVideo Is Nothing Then
    '                If lstVideo.Count > 0 Then
    '                    hidCategoryId.Value = hidCategoryId.Value & "," & cate.CategoryId
    '                    ltrVideo.Text = GenerateVideoSlide(lstVideo, cate.CategoryId)
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Function GetImage(ByVal image As String) As String
    '    Dim imagePath As String = Server.MapPath("/" & Utility.ConfigData.VideoThumbPath)
    '    Dim imagePathRelated As String = Server.MapPath("/" & Utility.ConfigData.VideoRelatedThumbPath)
    '    If File.Exists(imagePath & image) And File.Exists(imagePathRelated & image) Then
    '        Return "/" & Utility.ConfigData.VideoThumbPath & image
    '    End If
    '    Return "/" & Utility.ConfigData.VideoThumbNoImage
    'End Function

    <WebMethod()> _
    Public Shared Function GetdataVideo(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal categoryId As Integer) As String
        Dim data As New VideoRow
        Dim countPageSize As Integer = (pageIndex - 1) * pageSize
        Dim result As VideoCollection = New VideoCollection
        data.PageIndex = pageIndex
        data.PageSize = pageSize
        data.Condition = " c.Type=" & Utility.Common.CategoryType.Video & " AND vi.IsActive=1  AND vc.CategoryId = " & categoryId
        data.OrderBy = "vc.Arrange"
        data.OrderDirection = "DESC"
        result = VideoRow.ListByCatId(data)
        Dim htmlVideo As String = String.Empty
        If result.Count > 0 Then
            HttpContext.Current.Session("videoCountPageSizeRender") = countPageSize
            HttpContext.Current.Session("videoListRender") = result
            htmlVideo = Utility.Common.RenderUserControl("~/controls/resource-center/video/video-list.ascx")
            HttpContext.Current.Session("videoCountPageSizeRender") = Nothing
            HttpContext.Current.Session("videoListRender") = Nothing
        End If
        Return htmlVideo
    End Function

End Class
