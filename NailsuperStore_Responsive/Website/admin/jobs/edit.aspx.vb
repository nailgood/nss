Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_jobs_Edit
    Inherits AdminPage
    Protected JobId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        JobId = Convert.ToInt32(Request("JobId"))

        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpState.Items.AddRange(StateRow.GetStateList().ToArray())
        drpState.DataBind()
        drpState.Items.Insert(0, New ListItem("", ""))

        drpCategoryId.Datasource = JobCategoryRow.GetAllJobCategories(DB)
        drpCategoryId.DataValueField = "JobCategoryId"
        drpCategoryId.DataTextField = "JobCategory"
        drpCategoryId.Databind()
        drpCategoryId.Items.Insert(0, New ListItem("", ""))

        If JobId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbJob As JobRow = JobRow.GetRow(DB, JobId)
        txtCity.Text = dbJob.City
        txtZip.Text = dbJob.Zip
        txtCompany.Text = dbJob.Company
        txtTitle.Text = dbJob.Title
        txtEmail.Text = dbJob.Email
        txtPhone.Text = dbJob.Phone
        txtShortDescription.Text = dbJob.ShortDescription
        txtDescription.Text = dbJob.Description
        drpState.SelectedValue = dbJob.State
        drpCategoryId.SelectedValue = dbJob.CategoryId
        dtExpirationDate.Value = dbJob.ExpirationDate
        txtFullPartTime.Text = dbJob.FullPartTime
        txtRequirements.Text = dbJob.Requirements
        txtComments.Text = dbJob.Comments
        txtCompensation.Text = dbJob.Compensation
        txtBenefits.Text = dbJob.Benefits
        chkIsActive.Checked = dbJob.IsActive
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbJob As JobRow

            If JobId <> 0 Then
                dbJob = JobRow.GetRow(DB, JobId)
            Else
                dbJob = New JobRow(DB)
            End If
            dbJob.City = txtCity.Text

            dbJob.Title = txtTitle.Text
            dbJob.Zip = txtZip.Text
            dbJob.Company = txtCompany.Text
            dbJob.Email = txtEmail.Text
            dbJob.Phone = txtPhone.Text
            dbJob.ShortDescription = txtShortDescription.Text
            dbJob.ExpirationDate = dtExpirationDate.Value
            dbJob.Description = txtDescription.Text
            dbJob.CategoryId = drpCategoryId.SelectedValue
            dbJob.State = drpState.SelectedValue
            dbJob.FullPartTime = txtFullPartTime.Text
            dbJob.Requirements = txtRequirements.Text
            dbJob.Comments = txtComments.Text
            dbJob.Compensation = txtCompensation.Text
            dbJob.Benefits = txtBenefits.Text

            If Not dbJob.IsActive AndAlso chkIsActive.Checked Then
                Core.SendSimpleMail("info@nss.com", "info@nss.com", dbJob.Email, dbJob.Email, "Nail Superstore: Job Approved", "The job you recently posted, '" & dbJob.ShortDescription & "', has been approved for listing." & vbCrLf & vbCrLf & "Thank you," & vbCrLf & "The Nail Superstore" & vbCrLf & "http://www.nss.com/")
            End If
            dbJob.IsActive = chkIsActive.Checked

            If JobId <> 0 Then
                dbJob.Update()
            Else
                JobId = dbJob.Insert
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
        Response.Redirect("delete.aspx?JobId=" & JobId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

