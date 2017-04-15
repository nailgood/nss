Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports System.Collections.Generic
Imports System.Web.Services

Partial Class MediaPress_Default
    Inherits SitePage
    Protected PageSize As Integer = Utility.ConfigData.PageSizeScroll
    Protected TotalRecords As Integer = 0
    Private PageIndex As Integer = 1
    Private result As VideoCollection

    Public Property CategoryId() As Integer
        Get
            Return ViewState("CategoryId")
        End Get
        Set(ByVal value As Integer)
            ViewState("CategoryId") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim Id As Integer = 0
        If Request("CateId") <> Nothing Then
            CategoryId = CInt(Request("CateId"))
        Else
            CategoryId = 10
        End If

        If Not IsPostBack Then
            LoadData()
        End If
    End Sub

    Public Sub LoadData()
        Dim objCate As CategoryRow = CategoryRow.GetRow(DB, CategoryId)
        If objCate IsNot Nothing AndAlso objCate.CategoryId > 0 Then
            Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(objCate.CategoryName.ToLower())))
            litTitle.Text = objCate.CategoryName
            Dim objMetaTag As New MetaTag
            objMetaTag.PageTitle = objCate.PageTitle
            objMetaTag.MetaDescription = objCate.MetaDescription
            objMetaTag.MetaKeywords = objCate.MetaKeyword
            SetPageMetaSocialNetwork(Me.Page, objMetaTag)
        Else
            litTitle.Text = "Media Press Center"
        End If

        Dim lstCategory As New CategoryCollection
        Dim objData As New VideoRow
        objData.CategoryId = objCate.CategoryId
        If CategoryId > 0 Then
            objData.PageIndex = PageIndex
            objData.PageSize = PageSize
            objData.Condition = " c.Type=" & Utility.Common.CategoryType.MediaPress & " and vc.CategoryId=" & CategoryId.ToString & " and vi.IsActive=1"
            objData.OrderBy = "vc.Arrange"
            objData.OrderDirection = "DESC"
            result = VideoRow.ListByCatId(objData)
            TotalRecords = objData.TotalRow
            lstCategory.Add(objCate)
        Else
            lstCategory = CategoryRow.GetAllVideoCategoryByType(DB, Utility.Common.CategoryType.MediaPress)
            ''/media-topic se da qa trang category cua no
            If Request.RawUrl.Contains("media-topic") Then
                Utility.Common.Redirect301(Request.RawUrl.ToString() & "/" & URLParameters.ReplaceUrl(HttpUtility.UrlEncode(lstCategory(0).CategoryName.ToLower())) & "/" & lstCategory(0).CategoryId.ToString())
            End If
        End If

        rptlstMedia.DataSource = lstCategory
        rptlstMedia.DataBind()
    End Sub

    Protected Sub rptlstMedia_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptlstMedia.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim cate As CategoryRow = e.Item.DataItem
            Dim countPageSize As Integer = (PageIndex - 1) * PageSize
            Dim ucMedia As controls_resource_center_media_media_list = CType(e.Item.FindControl("media"), controls_resource_center_media_media_list)
            If CategoryId = 0 Then
                Dim lstMedia As VideoCollection = VideoRow.ListTop4MediaByCateId(cate.CategoryId)
                Dim ltrCategory As Literal = e.Item.FindControl("ltrCategory")
                ltrCategory.Text = "<div class='category'><a href='" & URLParameters.MediaListUrl(cate.CategoryName, cate.CategoryId) & "'>" & cate.CategoryName & "</a></div>"
                ucMedia.countPageSize = countPageSize
                ucMedia.MediaList = lstMedia
            Else
                ucMedia.countPageSize = countPageSize
                ucMedia.MediaList = result
            End If
        End If
    End Sub

    <WebMethod()> _
    Public Shared Function GetdataVideo(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal categoryId As Integer) As String
        Dim result As New VideoCollection
        Dim objData As New VideoRow
        objData.PageIndex = pageIndex
        objData.PageSize = pageSize
        objData.Condition = " c.Type=" & Utility.Common.CategoryType.MediaPress & " and vc.CategoryId=" & categoryId.ToString & " and vi.IsActive=1"
        objData.OrderBy = "vc.Arrange"
        objData.OrderDirection = "DESC"
        result = VideoRow.ListByCatId(objData)
        Dim countPageSize As Integer = (pageIndex - 1) * pageSize
        Dim htmlMedia As String = String.Empty
        If result.Count > 0 Then
            HttpContext.Current.Session("videoCountPageSizeRender") = countPageSize
            HttpContext.Current.Session("MediaListRender") = result
            htmlMedia = Utility.Common.RenderUserControl("~/controls/resource-center/media/media-list.ascx")
            HttpContext.Current.Session("videoCountPageSizeRender") = Nothing
            HttpContext.Current.Session("MediaListRender") = Nothing
        End If
        Return htmlMedia
    End Function
End Class
