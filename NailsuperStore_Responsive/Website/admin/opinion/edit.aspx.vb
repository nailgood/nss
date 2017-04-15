Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_opinion_Edit
    Inherits AdminPage

    Protected ContactId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ContactId = Convert.ToInt32(Request("ContactId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpSubjectId.DataSource = ContactUsRow.GetAllTypeContact(DB)
        drpSubjectId.DataValueField = "TypeContact"
        drpSubjectId.DataTextField = "TypeContact"
        drpSubjectId.Databind()
        'drpSubjectId.Items.Insert(0, New ListItem("", ""))

        If ContactId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbContactUs As ContactUsRow = ContactUsRow.GetRow(DB, ContactId)
        txtFirstName.Text = dbContactUs.FirstName
        txtLastName.Text = dbContactUs.LastName
        txtEmailAddress.Text = dbContactUs.EmailAddress
        txtPhone.Text = dbContactUs.Phone
        txtOrderNumber.Text = dbContactUs.OrderNumber
        txtComments.Text = dbContactUs.Comments
        drpSubjectId.SelectedItem.Text = dbContactUs.TypeContact
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbContactUs As ContactUsRow

            If ContactId <> 0 Then
                dbContactUs = ContactUsRow.GetRow(DB, ContactId)
            Else
                dbContactUs = New ContactUsRow(DB)
            End If
            dbContactUs.FirstName = txtFirstName.Text
            dbContactUs.LastName = txtLastName.Text
            dbContactUs.EmailAddress = txtEmailAddress.Text
            dbContactUs.Phone = txtPhone.Text
            dbContactUs.OrderNumber = txtOrderNumber.Text
            dbContactUs.Comments = txtComments.Text
            dbContactUs.TypeContact = drpSubjectId.SelectedItem.Text

            If ContactId <> 0 Then
                dbContactUs.Update()
            Else
                ContactId = dbContactUs.Insert
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
        Response.Redirect("delete.aspx?ContactId=" & ContactId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

