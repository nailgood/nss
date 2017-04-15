Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_tips_Edit
    Inherits AdminPage

    Protected TipId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        TipId = Convert.ToInt32(Request("TipId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpTipCategoryId.Datasource = TipCategoryRow.GetAllTipCategories(DB)
        drpTipCategoryId.DataValueField = "TipCategoryId"
        drpTipCategoryId.DataTextField = "TipCategory"
        drpTipCategoryId.Databind()
        drpTipCategoryId.Items.Insert(0, New ListItem("", ""))

        If TipId = 0 Then
            btnDelete.Visible = False
            drpTipCategoryId.SelectedValue = Request("F_TipCategoryId")
            Exit Sub
        End If

        Dim dbTip As TipRow = TipRow.GetRow(DB, TipId)
        txtTitle.Text = dbTip.Title
        txtSummary.Text = dbTip.Summary
        txtFullText.Text = dbTip.FullText
        txtVietTitle.Text = dbTip.VietTitle
        txtVietSummary.Text = dbTip.VietSummary
        txtVietText.Text = dbTip.VietText
        drpTipCategoryId.SelectedValue = dbTip.TipCategoryId
        txtMetaDescription.Text = dbTip.MetaDescription
        txtMetaKeywords.Text = dbTip.MetaKeywords
        txtMetaTitle.Text = dbTip.MetaTitle
        txtPageTitle.Text = dbTip.PageTitle
        chkIsActive.Checked = dbTip.IsActive
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbTip As TipRow

            If TipId <> 0 Then
                dbTip = TipRow.GetRow(DB, TipId)
            Else
                dbTip = New TipRow(DB)
            End If
            dbTip.Title = txtTitle.Text
            dbTip.Summary = txtSummary.Text
            dbTip.FullText = RemoveStyleFontSmall(txtFullText.Text)
            dbTip.TipCategoryId = drpTipCategoryId.SelectedValue
            dbTip.VietTitle = txtVietTitle.Text
            dbTip.VietSummary = txtVietSummary.Text
            dbTip.VietText = RemoveStyleFontSmall(txtVietText.Text)
            dbTip.MetaDescription = txtMetaDescription.Text
            dbTip.MetaTitle = txtMetaTitle.Text
            dbTip.MetaKeywords = txtMetaKeywords.Text
            dbTip.PageTitle = txtPageTitle.Text
            dbTip.IsActive = chkIsActive.Checked
            If TipId <> 0 Then
                dbTip.Update()
            Else
                TipId = dbTip.Insert
            End If

            DB.CommitTransaction()


            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Private Function RemoveStyleFontSmall(ByVal content As String) As String
        If (Not String.IsNullOrEmpty(content)) Then
            content = content.Replace("font-size: x-small;", "")
        End If
        Return content
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?TipId=" & TipId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

