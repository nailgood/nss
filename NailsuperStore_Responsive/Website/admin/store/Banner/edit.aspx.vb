Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_Banner_Edit
    Inherits AdminPage

    Protected Id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckAccess("STORE")
        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadDepartment()
            LoadFromDB()
            ''SetViewMobileImage()
        End If
    End Sub

    Private Sub LoadFromDB()
        If Id = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbBanner As BannerRow = BannerRow.GetRow(Id)
        ddlDepartment.SelectedValue = dbBanner.DepartmentId
        dtStartingDate.Value = dbBanner.StartingDate
        dtEndingDate.Value = dbBanner.EndingDate
        chkIsActive.Checked = dbBanner.IsActive
        txtUrl.Text = dbBanner.Url
        fuImage.CurrentFileName = dbBanner.BannerName
        hpimg.ImageUrl = fuImage.Folder & dbBanner.BannerName
        hpimgMobile.ImageUrl = fuImageMobile.Folder & dbBanner.MobileBannerName
        fuImageMobile.CurrentFileName = dbBanner.MobileBannerName
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbBanner.BannerName) & """></script>"
        End If
    End Sub
    Private Sub LoadDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartment.DataSource = ds
        ddlDepartment.DataTextField = "Name"
        ddlDepartment.DataValueField = "DepartmentId"
        ddlDepartment.DataBind()
        ddlDepartment.Items.Insert(0, New ListItem("Home Page", 23))
        If Request("DepartmentId") Is Nothing = False Then
            ddlDepartment.SelectedValue = Request("DepartmentId")
        End If

        ds.Dispose()
    End Sub
    Protected Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        SetViewMobileImage()
    End Sub
    Private Sub SetViewMobileImage()
        If ddlDepartment.SelectedValue = "23" Then
            trMobileImage.Visible = True
        Else
            trMobileImage.Visible = False
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            
            Dim dbBanner As BannerRow
            Dim DeptId As Integer = ddlDepartment.SelectedValue
            If Id <> 0 Then
                dbBanner = BannerRow.GetRow(Id)
            Else
                dbBanner = New BannerRow(DB)
            End If
            dbBanner.DepartmentId = DeptId
            dbBanner.StartingDate = dtStartingDate.Value
            dbBanner.EndingDate = dtEndingDate.Value
            dbBanner.IsActive = chkIsActive.Checked
            dbBanner.Url = txtUrl.Text
            fuImage.AutoResize = True

            Dim arr As String()
            If Id <> 0 Then
                dbBanner.Update()
            Else
                Id = dbBanner.Insert()
            End If
            If Id <> 0 Then

                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = Id.ToString & "." & arr(1)
                    fuImage.SaveNewFile()
                    dbBanner.BannerName = fuImage.NewFileName
                    ''  dbBanner.Update()
                    ''Save Banner for Mobile Web

                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    dbBanner.BannerName = Nothing
                    ''   dbBanner.Update()
                ElseIf dbBanner.BannerName = "" Then
                    ltMssImage.Text = "Image is required."
                    Exit Sub
                End If
                If dbBanner.DepartmentId = 23 Then
                    If fuImageMobile.NewFileName <> String.Empty Then
                        arr = fuImageMobile.NewFileName.Split(".")
                        fuImageMobile.NewFileName = Id.ToString & "." & arr(1)
                        fuImageMobile.SaveNewFile()
                        dbBanner.MobileBannerName = fuImageMobile.NewFileName
                    ElseIf fuImageMobile.MarkedToDelete Then
                        fuImageMobile.RemoveOldFile()
                        dbBanner.MobileBannerName = Nothing
                    End If
                Else
                    fuImageMobile.RemoveOldFile()
                    dbBanner.MobileBannerName = Nothing
                End If
                If (fuImage.NewFileName <> String.Empty Or fuImageMobile.NewFileName <> String.Empty) Then
                    dbBanner.Update()
                End If
            End If


            Session("deptId") = Nothing
            'Invalidate cached menu
            Context.Cache.Remove("HeaderMenuCache")

            Response.Redirect("default.aspx?DepartmentId=" & DeptId) '& GetPageParams(FilterFieldType.All))

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?DepartmentId=" & ddlDepartment.SelectedValue & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?Id=" & Id & "&" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub DeleteImage(ByVal strPath As String)


    End Sub
End Class

