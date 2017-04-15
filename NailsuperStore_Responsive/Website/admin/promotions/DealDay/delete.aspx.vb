Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_promotions_dealday_Delete
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try
            ''Delete Iamges
            Dim itemDB As DealDayRow = DealDayRow.GetRow(DB, Id)
            DB.BeginTransaction()
            DealDayRow.Delete(DB, Id)
            RemoveFileName("/assets/dealday/", itemDB.BannerImage)
            DB.CommitTransaction()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    Public Sub RemoveFileName(ByVal Path As String, ByVal FileName As String)
        Try
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(Path & FileName))
        Catch ex As Exception
        End Try
    End Sub
End Class
