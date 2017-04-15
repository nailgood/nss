Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports Utility.Common
Partial Class home
    Inherits SitePage
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Utility.ConfigData.IsEnableBlockBanner() Then
                ucBlockBanner.Visible = True
                ucMainBanner.Visible = False
            Else
                ucBlockBanner.Visible = False
                ucMainBanner.Visible = True
            End If
        End If

        Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL("/home.aspx")
        If row.PageId > 0 Then
            Dim objMeta As New MetaTag
            objMeta.PageTitle = row.Title
            objMeta.MetaDescription = row.MetaDescription
            objMeta.MetaKeywords = row.MetaKeywords
            Dim page As System.Web.UI.Page = Me.Page
            Dim sp As New SitePage()
            sp.SetPageMetaSocialNetwork(page, objMeta)
            objMeta.Canonical = "https://www.nss.com"
            SetPageMetaSocialNetwork(page, objMeta)
        End If
       
    End Sub
End Class
