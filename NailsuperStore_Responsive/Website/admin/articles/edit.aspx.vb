Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_articles_edit
    Inherits AdminPage

    Protected ArticleId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ArticleId = Convert.ToInt32(Request("ArticleId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ArticleId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbArticle As ArticleRow = ArticleRow.GetRow(DB, ArticleId)
        txtTitle.Text = dbArticle.Title
        txtOtherTitle.Text = dbArticle.OtherTitle
        txtShortAbstract.Text = dbArticle.ShortAbstract
        fuImage.CurrentFileName = dbArticle.Image
        fuImage.DisplayImage = True
        txtLink.Text = dbArticle.Link
        txtShortVersion.Text = dbArticle.ShortVersion
        txtFullVersion.Text = dbArticle.FullVersion
        dtPostDate.Value = dbArticle.PostDate
        dtPastNewsDate.Value = dbArticle.PastNewsDate
        chkIsActive.Checked = dbArticle.IsActive
        chkHasFullVersion.Checked = dbArticle.HasFullVersion
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbArticle As ArticleRow

            If ArticleId <> 0 Then
                dbArticle = ArticleRow.GetRow(DB, ArticleId)
            Else
                dbArticle = New ArticleRow(DB)
            End If
            dbArticle.Title = txtTitle.Text
            dbArticle.OtherTitle = txtOtherTitle.Text
            dbArticle.ShortAbstract = txtShortAbstract.Text
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbArticle.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbArticle.Image = Nothing
            End If
            dbArticle.Link = txtLink.Text
            dbArticle.ShortVersion = txtShortVersion.Text
            dbArticle.FullVersion = txtFullVersion.Text
            dbArticle.PostDate = dtPostDate.Text
            dbArticle.PastNewsDate = dtPastNewsDate.Value
            dbArticle.IsActive = chkIsActive.Checked
            dbArticle.HasFullVersion = chkHasFullVersion.Checked

            If ArticleId <> 0 Then
                dbArticle.Update()
            Else
                ArticleId = dbArticle.Insert
            End If

            DB.CommitTransaction()

            If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?ArticleId=" & ArticleId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

