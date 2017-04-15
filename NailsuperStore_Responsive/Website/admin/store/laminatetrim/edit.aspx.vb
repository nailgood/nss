Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_laminatetrim_Edit
    Inherits AdminPage

    Protected LaminateTrimId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        LaminateTrimId = Convert.ToInt32(Request("LaminateTrimId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If LaminateTrimId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreLaminateTrim As StoreLaminateTrimRow = StoreLaminateTrimRow.GetRow(DB, LaminateTrimId)
        txtLaminateTrim.Text = dbStoreLaminateTrim.LaminateTrim
        fuSwatch.CurrentFileName = dbStoreLaminateTrim.Swatch
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreLaminateTrim As StoreLaminateTrimRow

            If LaminateTrimId <> 0 Then
                dbStoreLaminateTrim = StoreLaminateTrimRow.GetRow(DB, LaminateTrimId)
            Else
                dbStoreLaminateTrim = New StoreLaminateTrimRow(DB)
            End If
            dbStoreLaminateTrim.LaminateTrim = txtLaminateTrim.Text
            If fuSwatch.NewFileName <> String.Empty Then
                fuSwatch.SaveNewFile()
                dbStoreLaminateTrim.Swatch = fuSwatch.NewFileName
                Core.ResizeImage(Server.MapPath(fuSwatch.Folder & fuSwatch.NewFileName), Server.MapPath(fuSwatch.Folder & fuSwatch.NewFileName), 40, 40)
            ElseIf fuSwatch.MarkedToDelete Then
                dbStoreLaminateTrim.Swatch = Nothing
            End If

            If LaminateTrimId <> 0 Then
                dbStoreLaminateTrim.Update()
            Else
                LaminateTrimId = dbStoreLaminateTrim.Insert
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
        Response.Redirect("delete.aspx?LaminateTrimId=" & LaminateTrimId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

