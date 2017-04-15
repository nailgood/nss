Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin__faq_edit
    Inherits AdminPage

    Private FaqId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        FaqId = Convert.ToInt32(Request("FaqId"))

        If FaqId <> 0 Then Delete.Visible = True Else Delete.Visible = False

        If Not IsPostBack Then
            If FaqId <> 0 Then
                Dim dbFaq As FaqRow = FaqRow.GetRow(DB, FaqId)
                txtQuestion.Text = dbFaq.Question
                chkIsActive.Checked = dbFaq.IsActive
                txtAnswer.Text = dbFaq.Answer
            Else
                chkIsActive.Checked = True
            End If
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbFaq As FaqRow
            If FaqId <> 0 Then dbFaq = FaqRow.GetRow(DB, FaqId) Else dbFaq = New FaqRow(DB)
            dbFaq.Question = txtQuestion.Text
            dbFaq.IsActive = chkIsActive.Checked
            dbFaq.Answer = txtAnswer.Text
            If FaqId <> 0 Then dbFaq.Update() Else dbFaq.Insert()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        If FaqId <> 0 Then
            Response.Redirect("delete.aspx?FaqId=" & FaqId.ToString & "&" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
End Class

