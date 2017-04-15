

Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class admin_promotions_FreeGift_level_delete
    Inherits AdminPage
    Dim f As New FileUpload
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try
            Dim objDB As FreeGiftLevelRow = FreeGiftLevelRow.GetRow(Id)
            If Not objDB Is Nothing Then
                f.RemoveFileName(Utility.ConfigData.PathFreeGiftLevelBanner & "/thumb/", objDB.Banner)
            End If

            FreeGiftLevelRow.Delete(Id)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

