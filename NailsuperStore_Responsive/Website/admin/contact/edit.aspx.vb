Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_contact_Edit
    Inherits AdminPage

    Protected ContactId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ContactId = Convert.ToInt32(Request("ContactId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpSubjectId.Datasource = ContactUsSubjectRow.GetAllContactUsSubjects(DB)
        drpSubjectId.DataValueField = "SubjectId"
        drpSubjectId.DataTextField = "Subject"
        drpSubjectId.Databind()
        drpSubjectId.Items.Insert(0, New ListItem("", ""))

        If ContactId = 0 Then
            btnDelete.Visible = False
            trItemAdjust.Visible = False
            trAdjustmenttype.Visible = False
            rfvItemAdjust.Enabled = False
            Exit Sub
        End If

        Dim dbContactUs As ContactUsRow = ContactUsRow.GetRow(DB, ContactId)
        txtFirstName.Text = dbContactUs.FirstName
        txtLastName.Text = dbContactUs.LastName
        txtEmailAddress.Text = dbContactUs.EmailAddress
        txtPhone.Text = dbContactUs.Phone
        txtOrderNumber.Text = dbContactUs.OrderNumber
        txtComments.Text = dbContactUs.Comments
        rblAdjustmentType.SelectedValue = dbContactUs.AdjustmentType
        txtItemAdjust.Text = dbContactUs.ProductDescription
        drpSubjectId.SelectedValue = dbContactUs.SubjectId
        If drpSubjectId.SelectedValue = "17" Then
            trItemAdjust.Visible = True
            trAdjustmenttype.Visible = True
            rfvItemAdjust.Enabled = True
        Else
            trItemAdjust.Visible = False
            trAdjustmenttype.Visible = False
            rfvItemAdjust.Enabled = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
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
            dbContactUs.SubjectId = drpSubjectId.SelectedValue
            If drpSubjectId.SelectedValue = "17" Then
                dbContactUs.AdjustmentType = rblAdjustmentType.SelectedValue
                dbContactUs.ProductDescription = txtItemAdjust.Text
            End If
            If ContactId <> 0 Then
                dbContactUs.Update()
            Else
                ContactId = dbContactUs.Insert
            End If

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?ContactId=" & ContactId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub drpSubjectId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpSubjectId.SelectedIndexChanged
        If drpSubjectId.SelectedValue = "17" Then
            trItemAdjust.Visible = True
            trAdjustmenttype.Visible = True
            rfvItemAdjust.Enabled = True
        Else
            trItemAdjust.Visible = False
            trAdjustmenttype.Visible = False
            rfvItemAdjust.Enabled = False
        End If
    End Sub
End Class

