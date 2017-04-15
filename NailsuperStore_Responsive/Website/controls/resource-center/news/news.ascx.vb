Imports Components
Imports DataLayer
Imports System.IO
Partial Class controls_resource_center_news_news
    Inherits BaseControl
    Protected PathThumbBlogImage As String = Utility.ConfigData.PathThumbBlogImage
    Protected PathThumbNewsImg As String = Utility.ConfigData.PathThumbNewsImage
    Private m_News As NewsRow = Nothing
    Private m_htmlCategory As String = Nothing
    Public Property News() As NewsRow
        Set(ByVal value As NewsRow)
            m_News = value
        End Set
        Get
            Return m_News
        End Get
    End Property
    Public Property htmlCategory() As String
        Set(ByVal value As String)
            m_htmlCategory = value
        End Set
        Get
            Return m_htmlCategory
        End Get
    End Property
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BinData()
    End Sub

    Public Sub BinData()
        If News Is Nothing AndAlso Not Session("newsRender") Is Nothing Then
            News = Session("newsRender")
        End If
        If htmlCategory Is Nothing AndAlso Not Session("htmlCategoryRender") Is Nothing Then
            htmlCategory = Session("htmlCategoryRender")
        End If
        ltrCategory.Text = htmlCategory
        Dim link As String = IIf(News.CategoryId = 12, URLParameters.BlogDetailUrl(News.Title, News.NewsId), URLParameters.NewsDetailUrl(News.Title, News.NewsId))
        If Not ltrDesc Is Nothing Then
            Dim des As String = News.ShortDescription
            des = des.Replace("https", "http")
            des = des.Replace("http", "https")
            ltrDesc.Text = "<a href='" & link & "'>" & des.Trim() & "......<span>read more</span></a>"
        End If

        hlTitle.Text = News.Title
        hlTitle.NavigateUrl = link
        ltrDate.Text = String.Format("{0:MMM d, yyyy}", Convert.ToDateTime(News.CreatedDate))
        Dim thumbImg As String = IIf(News.CategoryId = 12, PathThumbBlogImage, PathThumbNewsImg)
        If (System.IO.File.Exists(Server.MapPath("~/" & thumbImg & News.ThumbImage))) Then
            ltrDivImage.Text = "<div class=""image""><a href=""" & link & """><img src=" & Utility.ConfigData.CDNMediaPath & thumbImg & News.ThumbImage & " alt=""" & News.Title & """ /></a></div>"
        End If
    End Sub
End Class
