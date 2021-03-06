Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_contactEmail_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim EmailId As Integer = Convert.ToInt32(Request("EmailId"))
        Try
            DB.BeginTransaction()
            ContactUsSubjectDetailRow.DeleteAllByEmail(DB, EmailId)
            ContactUsSubjectEmailRow.RemoveRow(DB, EmailId)
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

