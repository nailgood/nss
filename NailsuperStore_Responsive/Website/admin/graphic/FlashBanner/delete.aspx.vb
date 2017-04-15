Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class admin_store_salescategory_delete
    Inherits AdminPage
    Dim f As New FileUpload
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try

            Dim logDetail As New AdminLogDetailRow
            Dim Banner As BannerRow = BannerRow.GetRow(Id)
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(Banner, Utility.Common.ObjectType.FlashBanner)
            Dim DepartmentId As Integer = Banner.DepartmentId
            BannerRow.RemoveRow(Id)
            f.RemoveFileName(Utility.ConfigData.PathBanner, Banner.BannerName)
            f.RemoveFileName(Utility.ConfigData.PathBanner & "small/", Banner.BannerName)

            logDetail.ObjectId = Id
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectType = Utility.Common.ObjectType.FlashBanner.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("default.aspx?Departmentid=" & DepartmentId) '& GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

