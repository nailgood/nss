Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_groups_options_Edit
    Inherits AdminPage

    Protected OptionId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        OptionId = Convert.ToInt32(Request("OptionId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If OptionId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreItemGroupOption As StoreItemGroupOptionRow = StoreItemGroupOptionRow.GetRow(DB, OptionId)
        txtOptionName.Text = dbStoreItemGroupOption.OptionName
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreItemGroupOption As StoreItemGroupOptionRow

            If OptionId <> 0 Then
                dbStoreItemGroupOption = StoreItemGroupOptionRow.GetRow(DB, OptionId)
            Else
                dbStoreItemGroupOption = New StoreItemGroupOptionRow(DB)
            End If
            dbStoreItemGroupOption.OptionName = txtOptionName.Text

            If OptionId <> 0 Then
                dbStoreItemGroupOption.Update()
            Else
                OptionId = dbStoreItemGroupOption.Insert
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
        Response.Redirect("delete.aspx?OptionId=" & OptionId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

