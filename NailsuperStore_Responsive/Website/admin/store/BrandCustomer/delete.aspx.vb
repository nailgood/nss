Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_itemenable_delete
    Inherits AdminPage

    Private EnableId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        EnableId = Convert.ToInt32(Request("EnableId"))
        Try
            DB.BeginTransaction()
            Dim SEI As New StoreItemEnable
            SEI.Remove(DB, EnableId)
            'SQL = "delete from StoreItemEnable where Id =" & MEMBER_ID
            'DB.ExecuteSQL(SQL)

            DB.CommitTransaction()

            Response.Redirect("related.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class