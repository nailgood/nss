Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_jobs_categories_Edit
    Inherits AdminPage

    Protected JobCategoryId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        JobCategoryId = Convert.ToInt32(Request("JobCategoryId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If JobCategoryId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbJobCategory As JobCategoryRow = JobCategoryRow.GetRow(DB, JobCategoryId)
        txtJobCategory.Text = dbJobCategory.JobCategory
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbJobCategory As JobCategoryRow

            If JobCategoryId <> 0 Then
                dbJobCategory = JobCategoryRow.GetRow(DB, JobCategoryId)
            Else
                dbJobCategory = New JobCategoryRow(DB)
            End If
            dbJobCategory.JobCategory = txtJobCategory.Text

            If JobCategoryId <> 0 Then
                dbJobCategory.Update()
            Else
                JobCategoryId = dbJobCategory.Insert
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
        Response.Redirect("delete.aspx?JobCategoryId=" & JobCategoryId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

