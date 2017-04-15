Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Partial Class admin_resume_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim memberid As Integer = Convert.ToInt32(Request("memberid"))
        Try
            DB.BeginTransaction()
            MemberResumeRow.RemoveRow1(DB, memberid)
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class
