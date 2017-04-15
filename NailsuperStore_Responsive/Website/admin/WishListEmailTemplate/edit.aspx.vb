Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer


Partial Class admin_WishListEmailTemplate_edit
    Inherits AdminPage
    Private EmailTemplateId As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        EmailTemplateId = Convert.ToInt32(Request("EmailTemplateId"))
        If Not Page.IsPostBack Then
            If EmailTemplateId <> 0 Then
                Dim dbEmailTemplate As WishListEmailTemplateRow = WishListEmailTemplateRow.GetRow(DB, EmailTemplateId)
                txtSubject.Text = dbEmailTemplate.EmailSubject
                lblPurpose.Text = dbEmailTemplate.EmailPurpose
                txtEmailBodyText.Text = dbEmailTemplate.EmailBodyText
            End If
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Not Page.IsValid Then Exit Sub
        Try
            DB.BeginTransaction()
            Dim dbEmailTemplate As WishListEmailTemplateRow = WishListEmailTemplateRow.GetRow(DB, EmailTemplateId)
            dbEmailTemplate.EmailBodyText = txtEmailBodyText.Text
            dbEmailTemplate.EmailSubject = txtSubject.Text
            dbEmailTemplate.Update()
            DB.CommitTransaction()
            Response.Redirect("default.aspx")
        Catch ex As ApplicationException
            AddError(ex.Message)
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx")
    End Sub



End Class
