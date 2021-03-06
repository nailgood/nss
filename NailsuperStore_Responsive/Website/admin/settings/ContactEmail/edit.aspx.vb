Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_contactEmail_Edit
    Inherits AdminPage

    Protected EmailId As Integer
    Private strIsChecked As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EmailId = Convert.ToInt32(Request("EmailId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        cblSubject.DataSource = ContactUsSubjectRow.GetAllContactUsSubjects(DB)
        cblSubject.DataValueField = "SubjectId"
        cblSubject.DataTextField = "Subject"
        cblSubject.DataBind()

        If EmailId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        If EmailId > 0 Then
            Dim dbContactUs As ContactUsSubjectEmailRow = ContactUsSubjectEmailRow.GetRow(DB, EmailId)
            txtEmailAddress.Text = dbContactUs.Email
            txtName.Text = dbContactUs.Name
            CheckSubjects()
        Else
            txtEmailAddress.Text = ""
            txtName.Text = ""
        End If

    End Sub
    Private Sub CheckSubjects()
        Dim dt As DataTable = ContactUsSubjectDetailRow.GetByEmailId(DB, EmailId)
        Dim count As Integer = dt.Rows.Count

        If count > 0 Then
            For i As Integer = 0 To count - 1
                If strIsChecked = "" Then
                    strIsChecked &= dt.Rows(i)("SubjectID")
                Else
                    strIsChecked &= "," & dt.Rows(i)("SubjectID")
                End If
            Next
        End If
        cblSubject.SelectedValues = strIsChecked
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        strIsChecked = cblSubject.SelectedValues
        lblSubject.Text = ""
        lblEmail.Text = ""
        Dim bError As Boolean = False
        If strIsChecked = "" Then
            lblSubject.Text = "Please select at least one subject."
            bError = True
        End If
        ''check email exist in ContactUsSubjectEmail
        Dim exist As Boolean = ContactUsSubjectEmailRow.CheckEmailExist(DB, EmailId, txtEmailAddress.Text)
        If exist Then
            lblEmail.Text = "This email exists."
            bError = True
        End If

        If Not IsValid Or bError Then Exit Sub
        Try
            DB.BeginTransaction()
            Dim dbContactUs As ContactUsSubjectEmailRow
            dbContactUs = New ContactUsSubjectEmailRow(DB)
            dbContactUs.Email = txtEmailAddress.Text
            dbContactUs.Name = txtName.Text
            If EmailId > 0 Then
                dbContactUs.EmailID = EmailId
                dbContactUs.Update()
                ContactUsSubjectDetailRow.DeleteAllByEmail(DB, EmailId)
            Else
                dbContactUs.Insert()
            End If

            Dim dbCustomerContact As New ContactUsSubjectDetailRow(DB)
            dbCustomerContact.EmailID = dbContactUs.EmailID
            dbCustomerContact.SubjectID = cblSubject.SelectedValue

            Dim ids() As String = strIsChecked.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then
                    dbCustomerContact.SubjectID = Convert.ToInt32(ids(i))
                    dbCustomerContact.Insert()
                End If
            Next

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
        Response.Redirect("delete.aspx?EmailId=" & EmailId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

