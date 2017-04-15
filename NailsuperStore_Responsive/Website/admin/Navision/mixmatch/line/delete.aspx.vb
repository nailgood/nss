Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_navision_mixmatch_line_delete
    Inherits AdminPage
    Public Property Type() As Integer
        Get
            If ViewState("Type") Is Nothing Then
                Return Utility.Common.MixmatchType.Normal
            End If
            Return CInt(ViewState("Type"))
        End Get
        Set(ByVal value As Integer)
            ViewState("Type") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim iId As Integer = Convert.ToInt32(Request("Id"))
        Try
            Type = Request.QueryString("type")
            Dim mixMatchLineRow As MixMatchLineRow = mixMatchLineRow.GetRow(DB, iId)
            Dim mixMatchRow As MixMatchRow = mixMatchRow.GetRow(DB, mixMatchLineRow.MixMatchId)
            DB.BeginTransaction()
            mixMatchLineRow.RemoveRow(DB, CInt(iId))
            If (mixMatchRow.Type = Utility.Common.MixmatchType.ProductCoupon AndAlso mixMatchLineRow.Value = 0) Then
                DB.ExecuteSQL("Update StoreItem set PromotionID=NULL where ItemId=" & mixMatchLineRow.ItemId)
            End If
            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = iId
            logDetail.ObjectType = Utility.Common.ObjectType.MixMatchLine.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(mixMatchLineRow, Utility.Common.ObjectType.MixMatchLine)

            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Utility.CacheUtils.RemoveCacheItemWithPrefix(StoreCartItemRow.cachePrefixKey & "ItemCount_")
            Utility.CacheUtils.RemoveCacheItemWithPrefix("popupCart_")
            Response.Redirect("default.aspx?type=" & Type & "&" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

