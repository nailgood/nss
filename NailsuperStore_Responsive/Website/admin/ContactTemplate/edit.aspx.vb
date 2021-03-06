Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_emailTemplate_Edit
    Inherits AdminPage

    Protected EmailId As Integer
    Protected SubjectTypeId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        EmailId = Convert.ToInt32(Request("EmailId"))
        If EmailId = ConfigurationManager.AppSettings("EmailID") Then
            dTemplateItem.Visible = True
            dTemplate.Visible = False
        Else
            dTemplateItem.Visible = False
            dTemplate.Visible = True
        End If

        If Request("F_SubjectTypeId") = Nothing Then
            SubjectTypeId = 2
        Else
            SubjectTypeId = Convert.ToInt32(Request("F_SubjectTypeId"))
            If SubjectTypeId = 1 Then
                Response.Redirect("editEmail.aspx?EmailID=" & Request("EmailId") & "&" & GetPageParams(FilterFieldType.All))
            End If
        End If
        If Not IsPostBack Then
            txtSubjectId.Text = Request("EmailId")
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

        If txtSubjectId.Text = "" Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbEmailTemplet As EmailTempletRow = EmailTempletRow.GetRow(DB, txtSubjectId.Text)
        drpSubjectId.SelectedValue = dbEmailTemplet.SubjectID

        If drpSubjectId.SelectedItem.Value <> "" Then
            txtContents.Text = dbEmailTemplet.Contents
            txtName.Text = dbEmailTemplet.Name
            txtSubject.Text = dbEmailTemplet.Subject
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

            If txtSubjectId.Text <> "" Then
                dbEmailTemplet = EmailTempletRow.GetRow(DB, txtSubjectId.Text)
            Else
                dbEmailTemplet = New EmailTempletRow(DB)
            End If
            dbEmailTemplet.SubjectID = drpSubjectId.SelectedValue
            dbEmailTemplet.Subject = txtSubject.Text
            dbEmailTemplet.Name = txtName.Text
            dbEmailTemplet.Contents = txtContents.Text
            If txtSubjectId.Text <> "" Then
                dbEmailTemplet.Update()
            Else
                txtSubjectId.Text = dbEmailTemplet.Insert
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

    Protected Sub drpSubjectId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpSubjectId.SelectedIndexChanged
        If drpSubjectId.SelectedValue <> "" Then
            Dim res As DataTable = DB.GetDataTable("select * from EmailTemplet where subjectid='" & drpSubjectId.SelectedValue & "'") ' & DB.Quote(SubjectId))
            If res.Rows.Count > 0 Then
                txtSubjectId.Text = res.Rows(0)("EmailID")
                LoadFromDB()
            Else
                txtSubjectId.Text = ""
                txtContents.Text = ""
                txtName.Text = ""
                txtSubject.Text = ""
            End If
        End If

    End Sub
End Class

