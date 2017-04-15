Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_store_salescategory_Edit
    Inherits AdminPage

    Protected SalesCategoryId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        SalesCategoryId = Convert.ToInt32(Request("SalesCategoryId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If SalesCategoryId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbSalesCategory As SalesCategoryRow = SalesCategoryRow.GetRow(DB, SalesCategoryId)
        txtCategory.Text = dbSalesCategory.Category
        txtUrlCode.Text = dbSalesCategory.URLCode
        chkIsActive.Checked = dbSalesCategory.IsActive
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbSalesCategory As SalesCategoryRow

            If SalesCategoryId <> 0 Then
                dbSalesCategory = SalesCategoryRow.GetRow(DB, SalesCategoryId)
            Else
                dbSalesCategory = New SalesCategoryRow(DB)
            End If
            dbSalesCategory.Category = txtCategory.Text
            dbSalesCategory.URLCode = txtUrlCode.Text
            dbSalesCategory.IsActive = chkIsActive.Checked

            If SalesCategoryId <> 0 Then
                dbSalesCategory.Update()
                WriteLogDetail("Update Sales Category", dbSalesCategory)
            Else
                SalesCategoryId = dbSalesCategory.Insert
                WriteLogDetail("Insert Sales Category", dbSalesCategory)
            End If

            DB.CommitTransaction()

            'Invalidate cached menu
            Context.Cache.Remove("HeaderMenuCache")
            Utility.CacheUtils.RemoveCache(Utility.enmCache.SaleMenu)
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
        Response.Redirect("delete.aspx?SalesCategoryId=" & SalesCategoryId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

