Imports Components
Imports DataLayer
Imports System.Data

Partial Class Masterpage_Main
    Inherits System.Web.UI.MasterPage
    Dim url As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Private Sub MasterPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim form As HtmlForm = CType(Me.FindControl("formMain"), HtmlForm)
        form.Action = HttpContext.Current.Request.RawUrl

        If Not IsPostBack Then
            If Utility.Common.IsiPad Then
                ltriPadScrollBar.Text = " <link href='/includes/scripts/nyroModal/styles/ipad-scollbar.css' rel='stylesheet' type='text/css' />"
            End If

            SetMetaTags()
            Utility.Common.GenerateMatserPageClass(form)
            If Request.RawUrl.Contains("detail") Or Request.RawUrl.Contains("shop-by-design/") Then
                ltrCanonical.Text = "<link rel='canonical' href='" & url & Request.RawUrl.Split("?")(0) & "' />"
            End If
        End If
    End Sub

    Private Sub SetMetaTags()


        Dim path = Request.Path
        Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL(path)
        If Not path.Contains("pageinfo.aspx") And String.IsNullOrEmpty(Page.Header.Title) Then

            If row.PageId > 0 Then
                'Page.Title = row.MetaTitle
                'Page.MetaDescription = row.MetaDescription
                'Page.MetaKeywords = row.MetaKeywords
                Dim objMeta As New MetaTag
                objMeta.PageTitle = row.Title
                objMeta.MetaDescription = row.MetaDescription
                objMeta.MetaKeywords = row.MetaKeywords
                Dim page As System.Web.UI.Page = Me.Page
                Dim sp As New SitePage()
                sp.SetPageMetaSocialNetwork(page, objMeta)
            End If
       
        End If
        Try
            If Not row.IsIndexed Or Not row.IsFollowed Then
                Dim sp As New SitePage()
                sp.SetIndexFollow(row, ltIndexFollow, False)
            End If
        Catch

        End Try
    End Sub

End Class

