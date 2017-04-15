Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls

Partial Class admin_navision_mixmatch_delete
    Inherits AdminPage
    Dim f As New FileUpload
    Protected iType As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Dim PmSalse As PromotionSalespriceRow
        Dim logDetail As New AdminLogDetailRow
        iType = Request("Type")
        Try

            PmSalse = PromotionSalespriceRow.GetRow(DB, Id)
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(PmSalse, Utility.Common.ObjectType.StripBanner)
            PromotionSalespriceRow.RemoveRow(DB, Id)
            f.RemoveFileName(Utility.ConfigData.PathPromotion, PmSalse.Image)
            f.RemoveFileName(Utility.ConfigData.PathPromotionMobile, PmSalse.MobileImage)


            logDetail.ObjectId = Id
            logDetail.ObjectType = Utility.Common.ObjectType.StripBanner.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("default.aspx?Type=" & iType & "&" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
   
End Class

