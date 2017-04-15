Imports Components
Imports DataLayer
Imports System.Data

Partial Class MasterPage_Interior
    Inherits System.Web.UI.MasterPage
    Public ContainerCss As String = "container"
    Dim url As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Private Sub MasterPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim form As HtmlForm = CType(Me.FindControl("formMain"), HtmlForm)
        If Request.Url.AbsolutePath.Contains("store/default.aspx") Or Request.Url.AbsolutePath.Contains("store/sub-category.aspx") Then
            form.Action = HttpContext.Current.Request.Url.AbsolutePath + Request.Url.Query
        Else
            form.Action = HttpContext.Current.Request.RawUrl
        End If
        If Request.Url.AbsolutePath.Contains("store/search-result.aspx") Then
            Utility.Common.GenerateMatserPageClass(form)
        End If
        ''  Utility.Common.GenerateMatserPageClass(form)
        If Not IsPostBack Then
            If Utility.Common.IsiPad Then
                ltriPadScrollBar.Text = " <link href='/includes/scripts/nyroModal/styles/ipad-scollbar.css' rel='stylesheet' type='text/css' />"
            End If
            SetMetaTags()
            If Request.RawUrl.Contains("detail") Then
                ltrCanonical.Text = "<link rel='canonical' href='" & url & Request.RawUrl.Split("?")(0) & "' />"
            End If
        End If
    End Sub

    Private Sub SetMetaTags()
        Dim path = Request.Path

        Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL(path)
        If row IsNot Nothing AndAlso row.PageId > 0 Then
            If Not path.Contains("pageinfo.aspx") And String.IsNullOrEmpty(Page.Header.Title) Then

                Dim objMeta As New MetaTag
                objMeta.PageTitle = row.Title
                If Request.RawUrl.Contains("/referrals/") Then
                    objMeta.MetaDescription = Resources.Msg.ReferralDescription
                Else
                    objMeta.MetaDescription = row.MetaDescription
                End If
                objMeta.MetaKeywords = row.MetaKeywords
                Dim page As System.Web.UI.Page = Me.Page
                Dim sp As New SitePage()
                sp.SetPageMetaSocialNetwork(page, objMeta)

            End If

            If row.IsFullScreen Then
                ContainerCss &= "-fluid"
                ucHeader.IsFullScreen = row.IsFullScreen
            End If
            Try
                If Not row.IsIndexed Or Not row.IsFollowed Then
                    Dim sp As New SitePage()
                    sp.SetIndexFollow(row, ltIndexFollow, False)
                End If
            Catch

            End Try

            'Dim strIndex As String = String.Empty
            'Dim strFollow As String = String.Empty
            'If Not row.IsIndexed Then
            '    strIndex = "NOINDEX"
            'End If
            'If Not row.IsFollowed Then
            '    strFollow = "NOFOLLOW"
            '    If Not String.IsNullOrEmpty(strIndex) Then
            '        strIndex &= ", "
            '    End If
            'End If
            'If Not String.IsNullOrEmpty(strIndex) And Not String.IsNullOrEmpty(strFollow) Then
            '    ltIndexFollow.Text = String.Format(strIndex, strIndex, strFollow)
            'End If
        End If
    End Sub

   
    Protected Overloads Sub OnInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        SitePage.LoadRegionControl(phdLeft, Utility.Common.ContentToolRegion.CT_Left.ToString, Page)
    End Sub

   
End Class

