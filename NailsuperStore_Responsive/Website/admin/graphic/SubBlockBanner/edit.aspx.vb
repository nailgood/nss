

Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_graphic_SubBlockBanner_edit
    Inherits AdminPage

    Protected Id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Id = Convert.ToInt32(Request("Id"))
        fuImage.ImageDisplayFolder = Utility.ConfigData.PathSubBlockBanner
        fuImage.Folder = Utility.ConfigData.PathSubBlockBanner
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
        fuImage.CurrentFileName = dbBanner.Image & "?t=" & DateTime.Now.Millisecond.ToString()
        hpimg.ImageUrl = fuImage.Folder & "/thumb/" & dbBanner.Image
        txtDescription.Text = dbBanner.Description.Trim()
        txtName.Text = dbBanner.Name.Trim()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim defaultW As Integer = 87
        Dim defaultH As Integer = 100

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
            dbBanner.Type = Utility.Common.InforBannerType.SubBlockBanner
            dbBanner.IsActive = chkIsActive.Checked
            dbBanner.Link = txtLink.Text
            dbBanner.Name = txtName.Text.Trim
            dbBanner.Description = txtDescription.Text.Trim
            fuImage.AutoResize = True
            If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(dbBanner.Image) Then
                ltMssImage.Text = "Image is required."
                Exit Sub
            End If
            ''check width height

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
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(fuImage.MyFile.InputStream)
                    Dim ImagePath As String = Server.MapPath(fuImage.Folder & "/")
                    If Not (img.PhysicalDimension.Width = defaultW AndAlso img.PhysicalDimension.Height = defaultH) Then
                        Using imgPhoto As System.Drawing.Image = System.Drawing.Image.FromFile(ImagePath & "" & dbBanner.Image)
                            Dim imgPhotoSub As System.Drawing.Image = Core.ScaleCrop(imgPhoto, defaultW, defaultH, Utility.Common.AnchorPosition.Center, True, False)
                            imgPhotoSub.Save(ImagePath & "/thumb/" & dbBanner.Image, System.Drawing.Imaging.ImageFormat.Jpeg)
                            imgPhotoSub.Dispose()
                        End Using
                    Else
                        Utility.File.DeleteFile(ImagePath & "/thumb/" & dbBanner.Image)
                        System.IO.File.Copy(ImagePath & "/" & dbBanner.Image, ImagePath & "/thumb/" & dbBanner.Image)
                    End If
                    ''delete image
                    Utility.File.DeleteFile(ImagePath & "/" & dbBanner.Image)
                    ''   Dim ImagePath As String = Server.MapPath(fuImage.Folder & "/")
                    '' Core.ResizeImage(ImagePath & "" & fuImage.NewFileName, ImagePath & "thumb/" & fuImage.NewFileName, 0, 100)

                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    dbBanner.Image = Nothing
                End If
                If (fuImage.NewFileName <> String.Empty) Then
                    dbBanner.Id = Id
                    InforBannerRow.Update(dbBanner)
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

