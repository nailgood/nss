Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_tips_categories_Edit
    Inherits AdminPage

    Protected TipCategoryId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        TipCategoryId = Convert.ToInt32(Request("TipCategoryId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If TipCategoryId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbTipCategory As TipCategoryRow = TipCategoryRow.GetRow(DB, TipCategoryId)
        txtTipCategory.Text = dbTipCategory.TipCategory
        txtDescription.Text = dbTipCategory.Description
        txtVietCategory.Text = dbTipCategory.VietCategory
        txtVietDescription.Text = dbTipCategory.VietDescription
        txtMetaDescription.Text = dbTipCategory.MetaDescription
        txtMetaKeywords.Text = dbTipCategory.MetaKeywords
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbTipCategory As TipCategoryRow

            If TipCategoryId <> 0 Then
                dbTipCategory = TipCategoryRow.GetRow(DB, TipCategoryId)
            Else
                dbTipCategory = New TipCategoryRow(DB)
            End If
            dbTipCategory.TipCategory = txtTipCategory.Text
            dbTipCategory.Description = txtDescription.Text
            dbTipCategory.VietCategory = txtVietCategory.Text
            dbTipCategory.VietDescription = txtVietDescription.Text
            dbTipCategory.MetaDescription = txtMetaDescription.Text
            dbTipCategory.MetaKeywords = txtMetaKeywords.Text

            If TipCategoryId <> 0 Then
                dbTipCategory.Update()
            Else
                TipCategoryId = dbTipCategory.Insert
            End If

            DB.CommitTransaction()


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
        Response.Redirect("delete.aspx?TipCategoryId=" & TipCategoryId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

