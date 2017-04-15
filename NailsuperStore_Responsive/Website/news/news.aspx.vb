Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports Utility.Common
Imports System.Web.Services

Partial Class News_List
    Inherits SitePage
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------
    Public objCate As CategoryRow

    Private rpNewsCount As Integer = 0
    Protected PathThumbNewsImg As String = Utility.ConfigData.PathThumbNewsImage
    Protected PathThumbBlogImage As String = Utility.ConfigData.PathThumbBlogImage
    'Protected PathLargeNewsImg As String = Utility.ConfigData.PathLargeNewsImage
    Public arrCategoryID As New ArrayList
    Public typecate As Integer = Utility.Common.CategoryType.News
    Protected Shared PageSize As Integer = 8
    Private Shared PageIndex As Integer = 1
    Protected Shared TotalRecords As Integer = 0
    Private objMetaTag As New MetaTag
    '-------------------------------------------------------------------
    ' METHODS
    '-------------------------------------------------------------------
    Public Property CategoryId() As Integer
        Get
            Return ViewState("CategoryId")
        End Get
        Set(ByVal value As Integer)
            ViewState("CategoryId") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'InitPager()
            LoadDefault()
        End If

    End Sub

    Private Sub LoadDefault()

        If Request.QueryString("cateId") <> Nothing AndAlso IsNumeric(Request.QueryString("cateId")) Then
            CategoryId = CInt(Request.QueryString("cateId"))
        ElseIf Request.RawUrl.Contains("blog") Then
            CategoryId = Utility.ConfigData.BlogId
        End If

        If CategoryId > 0 Then
            objCate = CategoryRow.GetRow(DB, CategoryId)
            Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(objCate.CategoryName.ToLower())))
            objMetaTag.PageTitle = objCate.PageTitle
            objMetaTag.MetaDescription = objCate.MetaDescription
            objMetaTag.MetaKeywords = objCate.MetaKeyword
            objMetaTag.MetaDescription = SetMetaDescription(objMetaTag.MetaDescription, objCate.CategoryName)
        End If

        LoadListNew()
    End Sub

    Public Sub LoadListNew()
        Dim result As NewsCollection
        Dim data As New NewsRow
        data.PageIndex = PageIndex
        data.PageSize = PageSize
        If CategoryId > 0 Then '' list by category
            data.Condition = "nc.CategoryId=" & CategoryId.ToString & " and ni.IsActive=1"
            data.OrderBy = "nc.Arrange"
            data.OrderDirection = "DESC"
            If Request.QueryString("y") <> Nothing AndAlso IsNumeric(Request.QueryString("y")) Then
                Dim arr As String = Request.QueryString("y")
                Dim year As String = Left(arr, 4)
                Dim month As String = Right(arr, arr.Length - 4)
                objMetaTag.PageTitle = String.Format("{0} {1} - {2}", MonthName(CInt(month)), year, objMetaTag.PageTitle)

                result = NewsRow.ListByCategoryYearMonth(CategoryId, CInt(year), CInt(month))
                TotalRecords = 0
            Else
                Dim topNews As NewsRow = NewsRow.GetTopByCategoryId(CategoryId)
                Dim linkTop As String = IIf(CategoryId = 12, URLParameters.BlogDetailUrl(topNews.Title, topNews.NewsId), URLParameters.NewsDetailUrl(topNews.Title, topNews.NewsId))
                Dim thumbImg As String = IIf(CategoryId = 12, PathThumbBlogImage, PathThumbNewsImg)
                If (System.IO.File.Exists(Server.MapPath("~" & thumbImg & topNews.ThumbImage))) Then
                    ltrDivImageTop.Text = "<div id='image-top'><a href=""" & linkTop & """><img src=""" & Utility.ConfigData.CDNMediaPath & thumbImg & topNews.ThumbImage & """ alt=""" & topNews.Title & """ /></a></div>"
                End If
                hlTitleTop.Text = "<div id='title-top'><a href='" & linkTop & "'>" & topNews.Title & "</a></div>"
                ltrDescTop.Text = "<div class='desc'>" & topNews.ShortDescription & "</div>"

                result = NewsRow.ListByCategoryId(data)
                For i As Integer = 0 To result.Count() - 1
                    If result(i).CategoryId <> CategoryId Then
                        result.RemoveAt(i)
                    End If
                Next
                TotalRecords = data.TotalRow
            End If
        Else
            If Request.QueryString("y") <> Nothing AndAlso IsNumeric(Request.QueryString("y")) Then
                Dim arr As String = Request.QueryString("y")
                Dim year As String = Left(arr, 4)
                Dim month As String = Right(arr, arr.Length - 4)
                objMetaTag.PageTitle = String.Format("{0} {1} - {2} News & Events", MonthName(CInt(month)), year, objMetaTag.PageTitle)


                data.Condition &= " and YEAR(CreatedDate) = " & year & " and MONTH(CreatedDate) = " & month

                result = NewsRow.ListByCategoryYearMonth(CategoryId, CInt(year), CInt(month))
                TotalRecords = 0
            Else
                Dim old As NewsCollection
                old = NewsRow.ListSummary(data)
                Dim linkTop As String = URLParameters.NewsDetailUrl(old(0).Title, old(0).NewsId)
                Dim thumbImg As String = IIf(old(0).CategoryId = 12, PathThumbBlogImage, PathThumbNewsImg)
                If (System.IO.File.Exists(Server.MapPath("~" & thumbImg & old(0).ThumbImage))) Then
                    ltrDivImageTop.Text = "<div id='image-top'><a href=""" & linkTop & """><img src=""" & Utility.ConfigData.CDNMediaPath & thumbImg & old(0).ThumbImage & """ alt=""" & old(0).Title & """ /></a></div>"
                End If
                hlTitleTop.Text = "<div id='title-top'><a href='" & linkTop & "'>" & old(0).Title & "</a></div>"
                ltrDescTop.Text = "<div class='desc'>" & old(0).ShortDescription & "</div>"
                result = old.Clone()
                result.RemoveAt(0)
            End If
        End If

        rpNewsCount = result.Count
        rptNews.DataSource = result
        rptNews.DataBind()

        If result.Count >= 1 Then
            objMetaTag.MetaDescription = SetMetaDescription(objMetaTag.MetaDescription, result(0).Title)
        Else
            objMetaTag.MetaDescription = String.Empty
        End If
        SetPageMetaSocialNetwork(Page, objMetaTag)
    End Sub

    Protected Sub rptNews_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptNews.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim news As NewsRow = CType(e.Item.DataItem, NewsRow)
            Dim ucNews As controls_resource_center_news_news = CType(e.Item.FindControl("ucNews"), controls_resource_center_news_news)
            Dim ltrCategory As String = ""
            '' list by summary or year
            If Not ExistCateID(arrCategoryID, news.CategoryId) AndAlso CategoryId = 0 Then
                arrCategoryID.Add(news.CategoryId)

                Dim arrUrl As String() = Request.RawUrl.Split("/")
                If arrUrl.Length > 2 Then ''list by year
                    Dim ltrmCategory As Literal = e.Item.FindControl("ltrmCategory")
                    If (news.CategoryId = 12) Then
                        ltrmCategory.Text = "<div class='mcategory'><a href='/blog'>" & CategoryRow.GetCategoryNameByCategoryId(news.CategoryId) & "</a></div><div class='clear'></div>"
                    Else
                        ltrmCategory.Text = "<div class='mcategory'><a href='" & URLParameters.NewsListlUrl(CategoryRow.GetCategoryNameByCategoryId(news.CategoryId), news.CategoryId) & "'>" & CategoryRow.GetCategoryNameByCategoryId(news.CategoryId) & "</a></div><div class='clear'></div>"
                    End If
                Else
                    If (news.CategoryId > 0) Then
                        Dim category As String = CategoryRow.GetCategoryNameByCategoryId(news.CategoryId).ToString()
                        Dim url As String = IIf(category.ToLower.Contains("blog"), "/blog", URLParameters.NewsListlUrl(category, news.CategoryId))
                        ltrCategory = "<div class='category'><a href='" & url & "'>" & category & "</a></div>"
                    End If
                End If
            End If

            ucNews.News = news
            ucNews.htmlCategory = ltrCategory
        End If
    End Sub

    Private Function ExistCateID(ByVal listID As ArrayList, ByVal cateID As Integer) As Boolean
        For id As Integer = 0 To listID.Count - 1
            If listID(id) = cateID Then
                Return True
            End If
        Next
        Return False
    End Function

    <WebMethod()> _
    Public Shared Function GetDataVideo(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal categoryId As Integer) As String
        Dim xmlData As String = ""
        Dim result = New NewsCollection
        Dim data As New NewsRow
        data.PageIndex = pageIndex
        data.PageSize = pageSize
        data.Condition = "nc.CategoryId=" & CategoryId.ToString & " and ni.IsActive=1"
        data.OrderBy = "nc.Arrange"
        data.OrderDirection = "DESC"
        result = NewsRow.ListByCategoryId(data)
        Dim htmlNews As String = String.Empty
        If result.Count > 0 Then

            For i As Integer = 0 To result.Count - 1
                HttpContext.Current.Session("newsRender") = result(i)
                HttpContext.Current.Session("htmlCategoryRender") = Nothing
                htmlNews &= Utility.Common.RenderUserControl("~/controls/resource-center/news/news.ascx")
                HttpContext.Current.Session("newsRender") = Nothing
                HttpContext.Current.Session("htmlCategoryRender") = Nothing
            Next
        End If
        Return htmlNews
    End Function

    'Public Shared Function GetXmlData(ByVal result As NewsCollection, ByVal TotalRecords As Integer) As String
    '    Dim xmlData As String = String.Empty
    '    Dim controlPath As String = "~/controls/resource-center/news/news.ascx"
    '    Dim pageHolder As New Page()
    '    Dim ucNews As controls_resource_center_news_news = DirectCast(pageHolder.LoadControl(controlPath), controls_resource_center_news_news)
    '    If result.Count > 0 Then
    '        xmlData = BindList(result, TotalRecords, ucNews)
    '        result = Nothing
    '    End If
    '    Return xmlData
    'End Function

    'Public Shared Function BindList(ByVal result As NewsCollection, ByVal countData As Integer, ByVal uc As Object) As String
    '    Dim pageHolder As New Page()
    '    Dim strXmlData As String = ""
    '    strXmlData = "<Data>"
    '    For i As Integer = 0 To result.Count - 1
    '        Dim o As New NewsRow
    '        o = result(i)
    '        uc.Fill(o, "")
    '        pageHolder.Controls.Add(uc)
    '        Dim output As New System.IO.StringWriter()
    '        HttpContext.Current.Server.Execute(pageHolder, output, False)
    '        strXmlData += vbCrLf & "<Items>"
    '        strXmlData += NewsRow.SetXMLtag("content", output.ToString, True)
    '        strXmlData += NewsRow.SetXMLtag("RowNum", i + 1, True)
    '        strXmlData += "</Items>"
    '    Next
    '    strXmlData += vbCrLf & "<PageCount>" & vbCrLf & "<PageCount>" & countData & "</PageCount>" & vbCrLf & "</PageCount>" & vbCrLf & "</Data>"
    '    Return strXmlData
    'End Function
End Class
