Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_tones_Edit
    Inherits AdminPage

    Protected ToneId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ToneId = Convert.ToInt32(Request("ToneId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ToneId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreTone As StoreToneRow = StoreToneRow.GetRow(DB, ToneId)
        txtTone.Text = dbStoreTone.Tone
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreTone As StoreToneRow

            If ToneId <> 0 Then
                dbStoreTone = StoreToneRow.GetRow(DB, ToneId)
            Else
                dbStoreTone = New StoreToneRow(DB)
            End If
            dbStoreTone.Tone = txtTone.Text

            If ToneId <> 0 Then
                dbStoreTone.Update()
            Else
                ToneId = dbStoreTone.Insert
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
        Response.Redirect("delete.aspx?ToneId=" & ToneId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

