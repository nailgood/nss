Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_cusioncolor_Edit
    Inherits AdminPage

    Protected CusionColorId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CusionColorId = Convert.ToInt32(Request("CusionColorId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If CusionColorId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreCusionColor As StoreCusionColorRow = StoreCusionColorRow.GetRow(DB, CusionColorId)
        txtCusionColor.Text = dbStoreCusionColor.CusionColor
        fuSwatch.CurrentFileName = dbStoreCusionColor.Swatch
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreCusionColor As StoreCusionColorRow

            If CusionColorId <> 0 Then
                dbStoreCusionColor = StoreCusionColorRow.GetRow(DB, CusionColorId)
            Else
                dbStoreCusionColor = New StoreCusionColorRow(DB)
            End If
            dbStoreCusionColor.CusionColor = txtCusionColor.Text
            If fuSwatch.NewFileName <> String.Empty Then
                fuSwatch.SaveNewFile()
                dbStoreCusionColor.Swatch = fuSwatch.NewFileName
                Core.ResizeImage(Server.MapPath(fuSwatch.Folder & fuSwatch.NewFileName), Server.MapPath(fuSwatch.Folder & fuSwatch.NewFileName), 40, 40)
            ElseIf fuSwatch.MarkedToDelete Then
                dbStoreCusionColor.Swatch = Nothing
            End If

            If CusionColorId <> 0 Then
                dbStoreCusionColor.Update()
            Else
                CusionColorId = dbStoreCusionColor.Insert
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
        Response.Redirect("delete.aspx?CusionColorId=" & CusionColorId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

