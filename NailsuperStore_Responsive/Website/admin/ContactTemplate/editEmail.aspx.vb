Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_emailTemplate_editEmail
    Inherits AdminPage

    Protected EmailId As Integer
    Protected SubjectTypeId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        EmailId = Convert.ToInt32(Request("EmailId"))
        If Request("F_SubjectTypeId") = Nothing Then
            SubjectTypeId = 2
        Else
            SubjectTypeId = Convert.ToInt32(Request("F_SubjectTypeId"))
        End If

        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        'drpSubjectId.DataSource = ContactUsSubjectRow.GetAllContactUsSubjects(DB)
        drpSubjectId.DataSource = ContactUsSubjectRow.GetTypeContactUsSubjects(DB, SubjectTypeId)
        drpSubjectId.DataValueField = "SubjectId"
        drpSubjectId.DataTextField = "Subject"
        drpSubjectId.Databind()
        drpSubjectId.Items.Insert(0, New ListItem("", ""))

        If EmailId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbEmailTemplet As EmailTempletRow = EmailTempletRow.GetRow(DB, EmailId)
        drpSubjectId.SelectedValue = dbEmailTemplet.SubjectID

        If drpSubjectId.SelectedItem.Value <> "" Then
            txtContents.Text = dbEmailTemplet.Contents
            txtName.Text = dbEmailTemplet.Name
            txtSubject.Text = dbEmailTemplet.Subject
            dtStartDate.Value = dbEmailTemplet.StartDate
            dtEndDate.Value = dbEmailTemplet.EndDate
            chkIsActive.Checked = dbEmailTemplet.IsActive
        Else
            txtContents.Text = ""
            txtName.Text = ""
            txtSubject.Text = ""
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbEmailTemplet As EmailTempletRow

            If EmailId <> 0 Then
                dbEmailTemplet = EmailTempletRow.GetRow(DB, EmailId)
            Else
                dbEmailTemplet = New EmailTempletRow(DB)
            End If
            dbEmailTemplet.SubjectID = drpSubjectId.SelectedValue
            dbEmailTemplet.Subject = txtSubject.Text
            dbEmailTemplet.Name = txtName.Text
            dbEmailTemplet.Contents = txtContents.Text
            dbEmailTemplet.StartDate = dtStartDate.Value
            dbEmailTemplet.EndDate = dtEndDate.Value
            dbEmailTemplet.IsActive = chkIsActive.Checked
            If EmailId <> 0 Then
                dbEmailTemplet.Update()
            Else
                EmailId = dbEmailTemplet.Insert
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
        Response.Redirect("delete.aspx?EmailId=" & EmailId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    'Protected Sub drpSubjectId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpSubjectId.SelectedIndexChanged
    '    If drpSubjectId.SelectedValue <> "" Then
    '        Dim res As DataTable = DB.GetDataTable("select * from EmailTemplet where subjectid='" & drpSubjectId.SelectedValue & "'") ' & DB.Quote(SubjectId))
    '        If res.Rows.Count > 0 Then
    '            EmailId = res.Rows(0)("EmailID")
    '            LoadFromDB()
    '        Else
    '            EmailId = 0
    '            txtContents.Text = ""
    '            txtName.Text = ""
    '            txtSubject.Text = ""
    '        End If
    '    End If

    'End Sub
End Class

