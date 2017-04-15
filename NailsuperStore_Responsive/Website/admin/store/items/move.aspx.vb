Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_move
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = CType(Request("Id"), Integer)
        Dim ParentId As Integer = CType(Request("ParentId"), Integer)
        Dim ACTION As String = Request("ACTION")
        Dim sTable As String = Request("sTable")
        params = GetPageParams(FilterFieldType.All)

        Try
            DB.BeginTransaction()
            If sTable = "CollectionItem" Then
                If Core.ChangeSortOrder(DB, "Id", "CollectionItem", "SortOrder", "ParentId = " & DB.Quote(ParentId), Id, ACTION) Then
                    DB.CommitTransaction()
                Else
                    DB.RollbackTransaction()
                End If
            ElseIf sTable = "RelatedItem" Then
                If Core.ChangeSortOrder(DB, "Id", "RelatedItem", "SortOrder", "ParentId = " & DB.Quote(ParentId), Id, ACTION) Then
                    DB.CommitTransaction()
                Else
                    DB.RollbackTransaction()
                End If
            ElseIf sTable = "RelatedSwatch" Then
                If Core.ChangeSortOrder(DB, "Id", "RelatedSwatch", "SortOrder", "ParentId = " & DB.Quote(ParentId), Id, ACTION) Then
                    DB.CommitTransaction()
                Else
                    DB.RollbackTransaction()
                End If
            End If

            If sTable = "CollectionItem" Then
                Response.Redirect("collection.aspx?ItemId=" & ParentId & "&" & params)
            ElseIf sTable = "RelatedItem" Then
                Response.Redirect("related.aspx?ItemId=" & ParentId & "&" & params)
            ElseIf sTable = "RelatedSwatch" Then
                Response.Redirect("related-swatch.aspx?ItemId=" & ParentId & "&" & params)
            End If

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class