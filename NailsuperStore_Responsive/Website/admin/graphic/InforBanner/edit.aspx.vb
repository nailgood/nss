

Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_graphic_InforBanner_edit
    Inherits AdminPage

    Protected Id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Id = Convert.ToInt32(Request("Id"))
        fuImage.ImageDisplayFolder = Utility.ConfigData.PathMainInforBanner
        fuImage.Folder = Utility.ConfigData.PathMainInforBanner
        fuImageMobile.ImageDisplayFolder = Utility.ConfigData.PathMainInforBanner & "/mobile/"
        fuImageMobile.Folder = Utility.ConfigData.PathMainInforBanner & "/mobile/"
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If Id = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbBanner As InforBannerRow = InforBannerRow.GetRow(Id)
        If dbBanner Is Nothing Then
            Exit Sub
        End If
        chkIsActive.Checked = dbBanner.IsActive
        txtLink.Text = dbBanner.Link.Trim()
        fuImage.CurrentFileName = dbBanner.Image
        fuImageMobile.CurrentFileName = dbBanner.Image
        hpimg.ImageUrl = fuImage.Folder & dbBanner.Image & "?t=" & DateTime.Now.Millisecond.ToString
        hpimg1.ImageUrl = fuImageMobile.Folder & dbBanner.Image & "?t=" & DateTime.Now.Millisecond.ToString
        txtDescription.Text = dbBanner.Description.Trim()
        txtName.Text = dbBanner.Name.Trim()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        Try
            Dim dbBanner As InforBannerRow
            Dim dbBannerBefore As InforBannerRow
            If Id <> 0 Then
                dbBanner = InforBannerRow.GetRow(Id)
                dbBannerBefore = CloneObject.Clone(dbBanner)
            Else
                dbBanner = New InforBannerRow()
            End If
            dbBanner.Type = Utility.Common.InforBannerType.Main
            dbBanner.IsActive = chkIsActive.Checked
            dbBanner.Link = txtLink.Text
            dbBanner.Name = txtName.Text.Trim
            dbBanner.Description = txtDescription.Text.Trim
            fuImage.AutoResize = True
            fuImageMobile.AutoResize = True
            If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(dbBanner.Image) Then
                ltMssImage.Text = "Image is required."
            End If
            If String.IsNullOrEmpty(fuImageMobile.NewFileName) AndAlso String.IsNullOrEmpty(dbBanner.Image) Then
                ltmsgimage1.Text = "Image is required."
            End If
            If Not String.IsNullOrEmpty(ltmsgimage1.Text) Or Not String.IsNullOrEmpty(ltMssImage.Text) Then
                Exit Sub
            End If
            Dim arr As String()
            If Id <> 0 Then
                InforBannerRow.Update(dbBanner)
            Else
                Id = InforBannerRow.Insert(dbBanner)
            End If
            If Id <> 0 Then

                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = Id.ToString & "." & arr(1)
                    fuImage.SaveNewFile()
                    dbBanner.Image = fuImage.NewFileName
                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    dbBanner.Image = Nothing
                End If
                If (fuImage.NewFileName <> String.Empty) Then
                    dbBanner.Id = Id
                    InforBannerRow.Update(dbBanner)
                End If

                If fuImageMobile.NewFileName <> String.Empty Then
                    fuImageMobile.NewFileName = dbBanner.Image
                    fuImageMobile.SaveNewFile()
                ElseIf fuImageMobile.MarkedToDelete Then
                    fuImageMobile.RemoveOldFile()
                End If
            End If


            Response.Redirect("default.aspx")

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?Id=" & Id & "&" & GetPageParams(FilterFieldType.All))
    End Sub

End Class

