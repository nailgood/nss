Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_members_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim MemberId As Integer = Convert.ToInt32(Request("MemberId"))
        Try
            Dim logDeatail As New AdminLogDetailRow
            Dim obj As MemberRow = MemberRow.GetRow(MemberId)
            logDeatail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(obj, Utility.Common.ObjectType.Member)
            DB.BeginTransaction()
            MemberRow.RemoveRow(DB, MemberId)
            DB.CommitTransaction()
            logDeatail.ObjectId = MemberId
            logDeatail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDeatail.ObjectType = Utility.Common.ObjectType.Member.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDeatail)

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

