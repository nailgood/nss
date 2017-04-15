Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_itembuywithpoints_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ItemId As Integer = Convert.ToInt32(Request("ItemId"))
        Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
        Dim siold As StoreItemRow = CloneObject.Clone(si)
        Try
            si.IsRewardPoints = False
            si.RewardPoints = 0
            si.ArrangeRewardPoints = 0
            si.Update()

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = ItemId
            logDetail.ObjectType = Utility.Common.ObjectType.ItemPoint.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ItemPoint, siold, si)
            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As Exception

        End Try

    End Sub
End Class
