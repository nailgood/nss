Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class PageInfo
    Inherits SitePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            _LoadData()
        End If

    End Sub

    Private Sub _LoadData()
        Dim url As String = Request.QueryString("url")
        If String.IsNullOrEmpty(url) Then
            url = Request.RawUrl
        End If

        Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL(url)
        If row.PageId > 0 Then
            'Content Page
            litContent.Text = row.Content.Trim()
            litTitle.Text = row.Title

            Dim str As String = row.MetaTitle.Replace(" - The Nail Superstore", "")
            If str.Length > 55 Then
                row.MetaTitle = str
            End If

            'SEO
            litMetaRobots.Text = String.Format("<meta name=""robots"" content=""{0}"" />", IIf(row.IsIndexed, "index", "noindex") & "," & IIf(row.IsFollowed, "follow", "nofollow"))
            LoadMetaData(DB, url)
            'Page.Title = row.MetaTitle
            'Page.MetaDescription = row.MetaDescription
            'Page.MetaKeywords = row.MetaKeywords
        End If
    End Sub


End Class