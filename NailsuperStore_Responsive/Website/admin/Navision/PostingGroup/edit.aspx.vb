Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_postingGroup_Edit
    Inherits AdminPage

    Protected Id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If Id = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbCustomerPostingGroup As CustomerPostingGroupRow = CustomerPostingGroupRow.GetRow(DB, Id)
        txtCode.Text = dbCustomerPostingGroup.Code
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCustomerPostingGroup As CustomerPostingGroupRow

            If Id <> 0 Then
                dbCustomerPostingGroup = CustomerPostingGroupRow.GetRow(DB, Id)
            Else
                dbCustomerPostingGroup = New CustomerPostingGroupRow(DB)
            End If
            dbCustomerPostingGroup.Code = txtCode.Text

            If Id <> 0 Then
                dbCustomerPostingGroup.Update()
            Else
                Id = dbCustomerPostingGroup.Insert
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
        Response.Redirect("delete.aspx?Id=" & Id & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
