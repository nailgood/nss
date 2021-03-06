Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_requestcallbacklanguage_Edit
    Inherits AdminPage

    Protected DetailId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        DetailId = Convert.ToInt32(Request("DetailId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpSubjectId.DataSource = RequestCallBackLanguageRow.GetAllRequestCallBackLanguages(DB)
        drpSubjectId.DataValueField = "LanguageId"
        drpSubjectId.DataTextField = "Language"
        drpSubjectId.Databind()
        drpSubjectId.Items.Insert(0, New ListItem("", ""))

        drpEmailId.DataSource = ContactUsSubjectEmailRow.GetAllContactUsSubjectEmailsGroup(DB, "14")
        drpEmailId.DataValueField = "EmailId"
        drpEmailId.DataTextField = "Email"
        drpEmailId.DataBind()
        drpEmailId.Items.Insert(0, New ListItem("", ""))
        If DetailId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbCustomerContact As EmailLanguageRow = EmailLanguageRow.GetRow(DB, DetailId)
        drpEmailId.SelectedValue = dbCustomerContact.EmailID
        drpSubjectId.SelectedValue = dbCustomerContact.LanguageID

        If drpEmailId.SelectedItem.Value <> "" Then
            Dim dbContactUs As ContactUsSubjectEmailRow = ContactUsSubjectEmailRow.GetRow(DB, drpEmailId.SelectedItem.Value)
            txtEmailAddress.Text = dbContactUs.Email
            txtName.Text = dbContactUs.Name
        Else
            txtEmailAddress.Text = ""
            txtName.Text = ""
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCustomerContact As EmailLanguageRow

            If DetailId <> 0 Then
                dbCustomerContact = EmailLanguageRow.GetRow(DB, DetailId)
            Else
                dbCustomerContact = New EmailLanguageRow(DB)
            End If
            dbCustomerContact.EmailID = drpEmailId.SelectedValue
            dbCustomerContact.LanguageID = drpSubjectId.SelectedValue

            If DetailId <> 0 Then
                dbCustomerContact.Update()
            Else
                DetailId = dbCustomerContact.Insert
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
        Response.Redirect("delete.aspx?DetailId=" & DetailId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub drpEmailId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpEmailId.SelectedIndexChanged
        If drpEmailId.SelectedItem.Value <> "" Then
            Dim dbContactUs As ContactUsSubjectEmailRow = ContactUsSubjectEmailRow.GetRow(DB, drpEmailId.SelectedItem.Value)
            txtEmailAddress.Text = dbContactUs.Email
            txtName.Text = dbContactUs.Name
        Else
            txtEmailAddress.Text = ""
            txtName.Text = ""
        End If
    End Sub
End Class

