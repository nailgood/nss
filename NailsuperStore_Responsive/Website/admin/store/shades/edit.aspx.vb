Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_shades_Edit
    Inherits AdminPage

    Protected ShadeId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ShadeId = Convert.ToInt32(Request("ShadeId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ShadeId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreShade As StoreShadeRow = StoreShadeRow.GetRow(DB, ShadeId)
        txtShade.Text = dbStoreShade.Shade
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreShade As StoreShadeRow

            If ShadeId <> 0 Then
                dbStoreShade = StoreShadeRow.GetRow(DB, ShadeId)
            Else
                dbStoreShade = New StoreShadeRow(DB)
            End If
            dbStoreShade.Shade = txtShade.Text

            If ShadeId <> 0 Then
                dbStoreShade.Update()
            Else
                ShadeId = dbStoreShade.Insert
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
        Response.Redirect("delete.aspx?ShadeId=" & ShadeId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

