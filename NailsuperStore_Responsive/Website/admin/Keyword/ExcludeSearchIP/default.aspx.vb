Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Collections.Generic

Partial Class admin_Keyword_ExcludeSearchIP_default
    Inherits AdminPage
    Dim Id As Integer = 0
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Id = CInt(Request("Id"))
        If Not IsPostBack Then
            LoadDetail()
            LoadList()
        End If
    End Sub

    Private Sub LoadDetail()
        If Id < 1 Then
            Exit Sub
        End If
        Dim item As KeywordIPExcludeRow = KeywordIPExcludeRow.GetRow(Id)
        If Not item Is Nothing Then
            txtIP.Text = item.IP
        End If
    End Sub

    Private Sub LoadList()
        Dim list As List(Of KeywordIPExcludeRow) = KeywordIPExcludeRow.ListIP()
        gvList.DataSource = list
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            KeywordIPExcludeRow.Delete(e.CommandArgument)
            Response.Redirect("default.aspx")
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim item As New KeywordIPExcludeRow
        Dim result As Integer
        Try
            If Not String.IsNullOrEmpty(txtIP.Text) Then
                item.IP = txtIP.Text
            End If
            If Id > 0 Then
                item.Id = Id
                result = KeywordIPExcludeRow.Update(item)
            Else
                result = KeywordIPExcludeRow.Insert(item)
            End If
            If (result = -1) Then
                AddError("IP Address is exists")
                Exit Sub
            ElseIf result = 0 Then
                AddError("System error. Please try again!")
                Exit Sub
            End If
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
        Response.Redirect("default.aspx")
    End Sub

    
    Protected Sub gvList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim index As Literal = e.Row.FindControl("index")
            index.Text = e.Row.DataItemIndex + 1
        End If
    End Sub
End Class
