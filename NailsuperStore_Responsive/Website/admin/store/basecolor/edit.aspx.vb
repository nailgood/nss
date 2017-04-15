Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_basecolor_Edit
    Inherits AdminPage

    Protected BaseColorId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        BaseColorId = Convert.ToInt32(Request("BaseColorId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If BaseColorId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreBaseColor As StoreBaseColorRow = StoreBaseColorRow.GetRow(DB, BaseColorId)
        txtBaseColor.Text = dbStoreBaseColor.BaseColor
        fuSwatch.CurrentFileName = dbStoreBaseColor.Swatch
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreBaseColor As StoreBaseColorRow

            If BaseColorId <> 0 Then
                dbStoreBaseColor = StoreBaseColorRow.GetRow(DB, BaseColorId)
            Else
                dbStoreBaseColor = New StoreBaseColorRow(DB)
            End If
            dbStoreBaseColor.BaseColor = txtBaseColor.Text
            If fuSwatch.NewFileName <> String.Empty Then
                fuSwatch.SaveNewFile()
                dbStoreBaseColor.Swatch = fuSwatch.NewFileName
                Core.ResizeImage(Server.MapPath(fuSwatch.Folder & fuSwatch.NewFileName), Server.MapPath(fuSwatch.Folder & fuSwatch.NewFileName), 40, 40)
            ElseIf fuSwatch.MarkedToDelete Then
                dbStoreBaseColor.Swatch = Nothing
            End If

            If BaseColorId <> 0 Then
                dbStoreBaseColor.Update()
            Else
                BaseColorId = dbStoreBaseColor.Insert
            End If

            DB.CommitTransaction()

            If fuSwatch.NewFileName <> String.Empty OrElse fuSwatch.MarkedToDelete Then fuSwatch.RemoveOldFile()

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
        Response.Redirect("delete.aspx?BaseColorId=" & BaseColorId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

