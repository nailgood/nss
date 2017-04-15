Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_jobs_applications_Edit
    Inherits AdminPage

    Protected ApplicationId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ApplicationId = Convert.ToInt32(Request("ApplicationId"))
        If ApplicationId = Nothing Then Response.Redirect("default.aspx")
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ApplicationId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbPostJobApplication As PostJobApplicationRow = PostJobApplicationRow.GetRow(DB, ApplicationId)
        txtCompanyName.Text = dbPostJobApplication.CompanyName
        txtMember.Text = DB.ExecuteScalar("select top 1 username from member where memberid = " & dbPostJobApplication.MemberId)
        txtWebsite.Text = dbPostJobApplication.Website
        txtEmail.Text = dbPostJobApplication.Email
        fuImage.CurrentFileName = dbPostJobApplication.Image
        chkIsApproved.Checked = dbPostJobApplication.IsApproved
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbPostJobApplication As PostJobApplicationRow

            If ApplicationId <> 0 Then
                dbPostJobApplication = PostJobApplicationRow.GetRow(DB, ApplicationId)
            Else
                dbPostJobApplication = New PostJobApplicationRow(DB)
            End If
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbPostJobApplication.Image = fuImage.NewFileName
                Core.ResizeImage(Server.MapPath(fuImage.Folder) & fuImage.NewFileName, Server.MapPath(fuImage.Folder) & fuImage.NewFileName, 120, 200)
            ElseIf fuImage.MarkedToDelete Then
                dbPostJobApplication.Image = Nothing
            End If
            dbPostJobApplication.CompanyName = Trim(txtCompanyName.Text)
            dbPostJobApplication.Website = Trim(txtWebsite.Text)
            dbPostJobApplication.Email = Trim(txtEmail.Text)
            If Not dbPostJobApplication.IsApproved AndAlso chkIsApproved.Checked Then
                dbPostJobApplication.ApproveDate = Now()
                If DB.ExecuteScalar("select top 1 employerid from employer where employername = " & DB.Quote(dbPostJobApplication.CompanyName)) = Nothing Then
                    Dim er As New EmployerRow(DB)
                    er.EmployerName = dbPostJobApplication.CompanyName
                    er.Image = dbPostJobApplication.Image
                    er.Insert()
                End If
                Dim dbMember As MemberRow = MemberRow.GetRow(dbPostJobApplication.MemberId)

                If dbMember.MemberId > 0 AndAlso Not dbMember.CanPostJob Then
                    dbMember.CanPostJob = True
                    dbMember.Update(DB)
                    Core.SendSimpleMail("info@nss.com", "info@nss.com", dbPostJobApplication.Email, dbPostJobApplication.Email, "Nail Superstore: Post Jobs Approved", "You have been granted the ability to post jobs and review resume submissions." & vbCrLf & "Visit us at http://www.nss.com/ today to begin utilizing this service." & vbCrLf & vbCrLf & "Thank you," & vbCrLf & "The Nail Superstore")
                End If
            End If
            dbPostJobApplication.IsApproved = chkIsApproved.Checked

            If ApplicationId <> 0 Then
                dbPostJobApplication.Update()
            Else
                ApplicationId = dbPostJobApplication.Insert
            End If

            DB.CommitTransaction()

            If fuImage.NewFileName <> String.Empty OrElse fuImage.MarkedToDelete Then fuImage.RemoveOldFile()

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
        Response.Redirect("delete.aspx?ApplicationId=" & ApplicationId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

