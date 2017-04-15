Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_resume_edit
    Inherits AdminPage
    Protected Resumeid As Integer
    Protected Memberid As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Memberid = Convert.ToInt32(Request("memberid"))
        Resumeid = Convert.ToInt32(Request("resumeid"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        F_JobCategory.DataSource = MemberResumeRow.GetJobCategory(DB)
        F_JobCategory.DataValueField = "jobcategoryid"
        F_JobCategory.DataTextField = "jobcategory"
        F_JobCategory.DataBind()
        F_JobCategory.Items.Insert(0, New ListItem("", ""))

        If Memberid = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbResume As MemberResumeRow = MemberResumeRow.GetRow(DB, Memberid)
        txtFirstName.Text = dbResume.FirstName
        txtLastName.Text = dbResume.LastName
        txtEmailAddress.Text = dbResume.Email
        txtPhone.Text = dbResume.Phone
        txtAddress1.Text = dbResume.Address1
        txtAddress2.Text = dbResume.Address2
        txtCity.Text = dbResume.City
        txtZipCode.Text = dbResume.Zipcode
        F_JobCategory.SelectedValue = dbResume.JobCategoryId
        chkRelocate.Checked = dbResume.Relocate
        txtHourRequirement.Text = dbResume.HourRequirement
        chkStudent.Checked = dbResume.Student
        txtYearsExperience.Text = dbResume.YearsExperience
        txtResume.Text = dbResume.Resume
        txtCoverLetter.Text = dbResume.CoverLetter
        txtProfileType.Text = dbResume.ProfileType
        chkIsAgreed.Checked = dbResume.IsAgreed
        chkIsActive.Text = dbResume.IsActive

        ' txtOrderNumber.Text = dbResume.OrderNumber
        ' txtComments.Text = dbResume.Comments
        'drpSubjectId.SelectedValue = dbResume.SubjectId
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbResume As MemberResumeRow

            If Memberid <> 0 Then
                dbResume = MemberResumeRow.GetRow(DB, Memberid)
            Else
                dbResume = New MemberResumeRow(DB)
            End If
            dbResume.FirstName = txtFirstName.Text
            dbResume.LastName = txtLastName.Text
            dbResume.Email = txtEmailAddress.Text
            dbResume.Phone = txtPhone.Text
            dbResume.Address1 = txtAddress1.Text
            dbResume.Address2 = txtAddress2.Text
            dbResume.City = txtCity.Text
            dbResume.Zipcode = txtZipCode.Text
            dbResume.JobCategoryId = F_JobCategory.SelectedValue
            dbResume.Relocate = chkRelocate.Checked
            dbResume.HourRequirement = txtHourRequirement.Text
            dbResume.Student = chkStudent.Checked
            dbResume.YearsExperience = txtYearsExperience.Text
            dbResume.Resume = txtResume.Text
            dbResume.CoverLetter = txtCoverLetter.Text
            dbResume.ProfileType = txtProfileType.Text
            dbResume.IsAgreed = chkIsAgreed.Checked
            dbResume.IsActive = chkIsActive.Text

            'dbContactUs.OrderNumber = txtOrderNumber.Text
            'dbContactUs.Comments = txtComments.Text
            'dbContactUs.SubjectId = drpSubjectId.SelectedValue

            If Memberid <> 0 Then
                dbResume.Update()
            Else
                Memberid = dbResume.Insert
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
        Response.Redirect("delete.aspx?Memberid=" & Memberid & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
