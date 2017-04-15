Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class admin_AutoRespond_delete
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim DayId As Integer = Convert.ToInt32(Request("DayId"))
        Try
            AutoRespondRow.Delete(DB, DayId)
            Response.Redirect("default.aspx")
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class
