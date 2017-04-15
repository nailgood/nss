Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin_store_statusmessage__default
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim iCnt As Integer
        If Not Page.IsPostBack Then
            For iCnt = 1 To 23
                CType(messagelbl.FindControl("txtMessage" & iCnt), TextBox).Text = DB.ExecuteScalar("SELECT Message FROM StatusMessage WHERE StatusId=" & iCnt)
            Next
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        Dim iCnt As Integer
        If Not Page.IsValid Then Exit Sub

        Try
            DB.BeginTransaction()
            ' Manually Update Messages
            For iCnt = 1 To 23
                DB.ExecuteSQL("UPDATE StatusMessage SET Message=" & DB.Quote(CType(messagelbl.FindControl("txtMessage" & iCnt), TextBox).Text) & " WHERE StatusId=" & iCnt)
            Next
            DB.CommitTransaction()
            Response.Write("<span style='color: green'>Successfully updated all status messages</span>")
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class