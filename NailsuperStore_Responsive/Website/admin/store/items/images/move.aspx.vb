Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_items_images_move
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ImageId As Integer = Convert.ToInt32(Request("ImageId"))
        Dim ItemId As Integer = Convert.ToInt32(Request("F_ItemId"))
        Dim Action As String = Request("ACTION")
        Try
            Dim isUp As Boolean = True
            If (Action = "DOWN") Then
                isUp = False
            End If
            StoreItemImageRow.ChangeOrder(DB, ImageId, isUp)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

