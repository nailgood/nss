Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_collections_Edit
    Inherits AdminPage

    Protected CollectionId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CollectionId = Convert.ToInt32(Request("CollectionId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If CollectionId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreCollection As StoreCollectionRow = StoreCollectionRow.GetRow(DB, CollectionId)
        txtCollectionName.Text = dbStoreCollection.CollectionName
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreCollection As StoreCollectionRow

            If CollectionId <> 0 Then
                dbStoreCollection = StoreCollectionRow.GetRow(DB, CollectionId)
            Else
                dbStoreCollection = New StoreCollectionRow(DB)
            End If
            dbStoreCollection.CollectionName = txtCollectionName.Text

            If CollectionId <> 0 Then
                dbStoreCollection.Update()
            Else
                CollectionId = dbStoreCollection.Insert
            End If

            DB.CommitTransaction()


            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?CollectionId=" & CollectionId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

