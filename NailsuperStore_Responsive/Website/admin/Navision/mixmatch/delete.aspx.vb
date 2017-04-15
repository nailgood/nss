Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_navision_mixmatch_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try
            Dim type As Integer = 0
            If Not Request.QueryString("type") Then
                type = CInt(Request.QueryString("type"))
            End If

            Dim mixMatchRow As MixMatchRow = mixMatchRow.GetRow(DB, Id)
            DB.BeginTransaction()
            mixMatchRow.RemoveRow(DB, Id)
            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = Id
            logDetail.ObjectType = Utility.Common.ObjectType.MixMatch.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(mixMatchRow, Utility.Common.ObjectType.MixMatch)
            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("default.aspx?type=" & type & "&" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

