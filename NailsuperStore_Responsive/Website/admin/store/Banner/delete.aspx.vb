Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class admin_store_salescategory_delete
    Inherits AdminPage
    Dim f As New FileUpload
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckAccess("STORE")

        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try
            DB.BeginTransaction()

            Dim Banner As BannerRow = BannerRow.GetRow(Id)
            Dim DepartmentId As Integer = Banner.DepartmentId
            BannerRow.RemoveRow(Id)
            f.RemoveFileName(ConfigurationManager.AppSettings("PathBanner"), Banner.BannerName)
            DB.CommitTransaction()

            Response.Redirect("default.aspx?Departmentid=" & DepartmentId) '& GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

