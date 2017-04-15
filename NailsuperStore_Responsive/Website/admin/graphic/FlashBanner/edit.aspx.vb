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

        Id = Convert.ToInt32(Request("Id"))
        fuImage.ImageDisplayFolder = Utility.ConfigData.PathBanner
        fuImage.Folder = Utility.ConfigData.PathBanner

        fuImageMobile.ImageDisplayFolder = Utility.ConfigData.PathBanner & "mobile/"
        fuImageMobile.Folder = Utility.ConfigData.PathBanner & "mobile/"

        If Not IsPostBack Then
            LoadDepartment()
            LoadFromDB()

            ''SetViewMobileImage()
        End If
        LoadRadBackground()
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
        fuImageMobile.CurrentFileName = dbBanner.BannerName
        hpimg.ImageUrl = fuImage.Folder & dbBanner.BannerName
        hpimg1.ImageUrl = fuImageMobile.Folder & dbBanner.BannerName
        If Not String.IsNullOrEmpty(dbBanner.Background) Then
            If dbBanner.Background.Contains("#") Then
                txtColor.Text = dbBanner.Background
                radColor.Checked = True
                radImage.Checked = False
            Else
                imgbg.ImageUrl = fuBackground.Folder & dbBanner.Background
                radColor.Checked = False
                radImage.Checked = True
            End If
        End If
        If dbBanner.DepartmentId = Utility.ConfigData.RootDepartmentID Then
            trBackground.Attributes.Add("style", "display:none")
        End If

        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbBanner.BannerName) & """></script>"
        End If
        If fuImageMobile.CurrentFileName <> Nothing Then
            divImg1.Visible = True
            If Right(fuImageMobile.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg1.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbBanner.BannerName) & """></script>"
        End If
    End Sub
    Private Sub LoadDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartment.DataSource = ds
        ddlDepartment.DataTextField = "AlternateName"
        ddlDepartment.DataValueField = "DepartmentId"
        ddlDepartment.DataBind()
        ddlDepartment.Items.Insert(0, New ListItem("Home Page", 23))
        If Request("DepartmentId") Is Nothing = False Then
            ddlDepartment.SelectedValue = Request("DepartmentId")
        End If

        ds.Dispose()

        ''ddlDepartment.Items.Insert(0, New ListItem("Home Page", 23))

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        Try

            Dim dbBanner As BannerRow
            Dim dbBannerBefore As BannerRow
            Dim logDetail As New AdminLogDetailRow
            Dim DeptId As Integer = ddlDepartment.SelectedValue
            If Id <> 0 Then
                dbBanner = BannerRow.GetRow(Id)
                dbBannerBefore = CloneObject.Clone(dbBanner)
            Else
                dbBanner = New BannerRow(DB)
            End If
            dbBanner.DepartmentId = DeptId
            dbBanner.StartingDate = dtStartingDate.Value
            dbBanner.EndingDate = dtEndingDate.Value
            dbBanner.IsActive = chkIsActive.Checked
            dbBanner.Url = txtUrl.Text
            If radColor.Checked Then
                dbBanner.Background = txtColor.Text
            End If
            fuImage.AutoResize = True
            fuImageMobile.AutoResize = True
            If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(dbBanner.BannerName) Then
                ltMssImage.Text = "Image is required."
                'Exit Sub
            End If
            If String.IsNullOrEmpty(fuImageMobile.NewFileName) AndAlso String.IsNullOrEmpty(dbBanner.BannerName) Then
                ltmsgimage1.Text = "Image is required."
                'Exit Sub
            End If
            If Not String.IsNullOrEmpty(ltmsgimage1.Text) Or Not String.IsNullOrEmpty(ltMssImage.Text) Then
                Exit Sub
            End If

            Dim arr As String()
            If Id <> 0 Then
                dbBanner.Update()
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                Id = dbBanner.Insert()
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            If Id <> 0 Then

                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = Id.ToString & "." & arr(1)
                    fuImage.SaveNewFile()
                    dbBanner.BannerName = fuImage.NewFileName
                    Dim ImagePath As String = Server.MapPath(fuImage.Folder & "/")
                    If dbBanner.DepartmentId <> Utility.ConfigData.RootDepartmentID Then
                        Core.ResizeImage(ImagePath & "" & fuImage.NewFileName, ImagePath & "small/" & fuImage.NewFileName, 183, 78)
                    End If



                    '' Core.CropByAnchor(ImagePath & "" & fuImage.NewFileName, ImagePath & "small/" & fuImage.NewFileName, 170, 96, Utility.Common.ImageAnchorPosition.Center)

                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    dbBanner.BannerName = Nothing
                End If
                If fuBackground.NewFileName <> String.Empty Then
                    fuBackground.SaveNewFile()
                    dbBanner.Background = fuBackground.NewFileName
                ElseIf fuBackground.MarkedToDelete Then
                    fuBackground.RemoveOldFile()
                    dbBanner.Background = Nothing
                End If
                If fuImage.NewFileName <> String.Empty Or fuBackground.NewFileName <> String.Empty Then
                    dbBanner.Update()
                End If
                If fuImageMobile.NewFileName <> String.Empty Then
                    fuImageMobile.NewFileName = dbBanner.BannerName
                    fuImageMobile.SaveNewFile()
                    'Dim ImagePath As String = Server.MapPath(fuImageMobile.Folder & "/")
                    'If dbBanner.DepartmentId <> Utility.ConfigData.RootDepartmentID Then
                    '    Core.ResizeImage(ImagePath & "" & fuImage.NewFileName, ImagePath & fuImageMobile.NewFileName, 540, 190)
                    'End If

                ElseIf fuImageMobile.MarkedToDelete Then
                    fuImageMobile.RemoveOldFile()
                    'Else
                    '    Dim ImagePath As String = Server.MapPath(fuImage.Folder & "/")
                    '    Core.ResizeImage(ImagePath & "" & fuImage.NewFileName, ImagePath & "mobile\" & fuImageMobile.NewFileName, 540, 190)
                End If
            End If
            If (logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()) Then
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbBanner, Utility.Common.ObjectType.FlashBanner)
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.FlashBanner, dbBannerBefore, dbBanner)
            End If
            logDetail.ObjectId = Id
            logDetail.ObjectType = Utility.Common.ObjectType.FlashBanner.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

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
    Private Sub LoadRadBackground()
        If radColor.Checked Then
            dtxt.Visible = True
            dimage.Visible = False
            fuBackground.NewFileName = String.Empty
        Else
            dtxt.Visible = False
            txtColor.Text = String.Empty
            dimage.Visible = True
        End If
    End Sub
End Class

