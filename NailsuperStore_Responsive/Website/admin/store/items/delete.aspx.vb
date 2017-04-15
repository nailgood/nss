Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_delete
    Inherits AdminPage

    Private ITEM_ID As Integer


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ITEM_ID = Convert.ToInt32(Request("ItemId"))
        ''check item is sell in ebay
        If Not StoreItemRow.IsSellInEbay(ITEM_ID) Then
            Dim item As StoreItemRow = StoreItemRow.GetRow(DB, ITEM_ID)
            If (item Is Nothing) Then
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If

            Try
                DB.BeginTransaction()

                SQL = "delete from StoreCollectionItem where ItemId=" & ITEM_ID
                DB.ExecuteSQL(SQL)

                SQL = "delete from StoreToneItem where ItemId=" & ITEM_ID
                DB.ExecuteSQL(SQL)

                SQL = "delete from RelatedItem where itemId = " & ITEM_ID
                DB.ExecuteSQL(SQL)

                SQL = "delete from ItemRelatedVideo where ItemId=" & ITEM_ID
                DB.ExecuteSQL(SQL)

                SQL = "delete from storeitemfeature where itemId = " & ITEM_ID
                DB.ExecuteSQL(SQL)

                StoreItemRow.RemoveRow(DB, ITEM_ID)
                DB.CommitTransaction()
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectId = ITEM_ID
                logDetail.ObjectType = Utility.Common.ObjectType.StoreItem.ToString()
                Dim changeLog As String = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(item, Utility.Common.ObjectType.StoreItem)
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            Finally
            End Try

        End If
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class