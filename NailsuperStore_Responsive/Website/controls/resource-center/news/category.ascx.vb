

Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class controls_NewsCategory
    Inherits ModuleControl
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------
    Public title As String
    Public urlID As String
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Private CateId As Integer
    '-------------------------------------------------------------------
    ' METHODS
    '-------------------------------------------------------------------
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("cateId") <> Nothing AndAlso IsNumeric(Request.QueryString("cateId")) Then
            CateId = CInt(Request.QueryString("cateId"))
        End If
        If Not IsPostBack Then
            If CateId = 0 Then
                If Request.RawUrl.Contains("blog") Then
                    CateId = Utility.ConfigData.BlogId
                ElseIf LCase(Request.Url.ToString).Contains("news.aspx") = True Or LCase(Request.Url.ToString).Contains("video.aspx") = True Then
                    Session("NewsCateId") = Nothing
                    Session("VideoCateId") = Nothing
                ElseIf Request.RawUrl.Contains("news") Then
                    CateId = Session("NewsCateId")
                Else
                    CateId = Session("VideoCateId")
                End If
            ElseIf (Request.Url.ToString).Contains("video.aspx") Then
                Session("VideoCateId") = CateId
            Else
                Session("NewsCateId") = CateId
            End If
            LoadList()
        End If
    End Sub

    Private Sub LoadList()
        Dim result As New CategoryCollection
        If LCase(Request.Path.ToString).Contains("news") Then
            result = CategoryRow.ListByType(DB, Utility.Common.CategoryType.News)
            title = "News & Events"
            urlID = "news-topic"
        ElseIf LCase(Request.RawUrl.ToString).Contains("video") Then
            result = CategoryRow.ListByType(DB, Utility.Common.CategoryType.Video)
            title = "Video"
            urlID = "video-topic"
        ElseIf LCase(Request.RawUrl.ToString).Contains("media") Then
            result = CategoryRow.ListByType(DB, Utility.Common.CategoryType.MediaPress)
            title = "Media Press"
            urlID = "media-topic"
        End If
        divTitle.Attributes.Add("class", "title")
        divTitle.InnerHtml = "<a href='/" & urlID & "'>" & title & " Center</a>"

        If result.Count < 1 Then
            rptCategoryNews.DataSource = Nothing
        Else
            rptCategoryNews.DataSource = result
        End If
        rptCategoryNews.DataBind()
    End Sub


    Protected Sub rptCategoryNews_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCategoryNews.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            '' Dim text As String = ""
            Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
            Dim tab As CategoryRow = e.Item.DataItem
            If CateId = tab.CategoryId Then
                If LCase(tab.CategoryName) = "blog" Then
                    ltrLink.Text = "<li class=""select""><b class=""arrow-left""></b><a href=""/blog"">" & tab.CategoryName & "</a></li>"
                Else
                    ltrLink.Text = String.Format("<li class=""select""><b class=""arrow-left""></b><a href=""/" & urlID & "/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(tab.CategoryName.ToLower())) & "/" & tab.CategoryId & """>{0}</a></li>", tab.CategoryName)
                End If
            Else
                If tab.LinkDetail = String.Empty Then
                    If LCase(tab.CategoryName) = "blog" Then
                        ltrLink.Text = "<li><b class=""arrow-left""></b><a href=""/blog"">" & tab.CategoryName & "</a></li>"
                    Else
                        ltrLink.Text = "<li><b class=""arrow-left""></b><a href=""/" & urlID & "/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(tab.CategoryName.ToLower())) & "/" & tab.CategoryId & """>" & tab.CategoryName & "</a></li>"
                    End If

                ElseIf (tab.Type = 1) Then
                    ltrLink.Text = "<li><b class=""arrow-left""></b><a href=" & tab.LinkDetail & ">" & tab.CategoryName & "</a></li>"
                End If
                End If
            '' ltrLink.Text = e.Item.ItemIndex ''& text
        End If
    End Sub
End Class

