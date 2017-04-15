

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_promotions_FreeGift_level_edit
    Inherits AdminPage
    Private LevelId As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Request.QueryString("id") Then
            LevelId = Request.QueryString("id")
        End If
        'fuImage.ImageDisplayFolder = Utility.ConfigData.PathFreeGiftLevelBanner
        'fuImage.Folder = Utility.ConfigData.PathFreeGiftLevelBanner
        If Not Page.IsPostBack Then
            LoadDB()
        End If
    End Sub
    Private Sub LoadDB()
        If LevelId > 0 Then
            Dim objLevel As FreeGiftLevelRow = FreeGiftLevelRow.GetRow(LevelId)
            If Not objLevel Is Nothing Then
                txtName.Text = objLevel.Name
                txtMinValue.Text = objLevel.MinValue
                If (objLevel.MaxValue > 0) Then
                    txtMaxValue.Text = objLevel.MaxValue
                End If

                If objLevel.IsActive Then
                    chkIsActive.Checked = True
                End If
                'If Not String.IsNullOrEmpty(objLevel.Banner) Then
                '    fuImage.CurrentFileName = objLevel.Banner & "?t=" & DateTime.Now.Millisecond.ToString()
                'End If

                'hpimg.ImageUrl = fuImage.Folder & "/thumb/" & objLevel.Banner
            End If
        End If
    End Sub

    Private Sub Back()
        Response.Redirect("default.aspx")
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Page.IsValid Then
            Dim defaultW As Integer = 155
            Dim defaultH As Integer = 88
            Dim objLevel As FreeGiftLevelRow
            If (LevelId > 0) Then
                objLevel = FreeGiftLevelRow.GetRow(LevelId)
            Else
                objLevel = New FreeGiftLevelRow
            End If
            objLevel.Name = txtName.Text.Trim
            objLevel.MinValue = txtMinValue.Text.Trim
            If String.IsNullOrEmpty(txtMaxValue.Text) Then
                objLevel.MaxValue = 0
            Else
                objLevel.MaxValue = txtMaxValue.Text.Trim
            End If
            If chkIsActive.Checked Then
                objLevel.IsActive = True
            End If
            'fuImage.AutoResize = True
            'If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(objLevel.Banner) Then
            '    ltMssImage.Text = "Banner is required."
            '    Exit Sub
            'End If

            If LevelId > 0 Then
                objLevel.Id = LevelId
                FreeGiftLevelRow.Update(objLevel)
            Else
                LevelId = FreeGiftLevelRow.Insert(objLevel)
            End If
            'If LevelId <> 0 Then
            '    Dim arr As String()
            '    If fuImage.NewFileName <> String.Empty Then
            '        arr = fuImage.NewFileName.Split(".")
            '        fuImage.NewFileName = LevelId & "-over-" & CInt(objLevel.MinValue).ToString() & "." & arr(1)
            '        fuImage.SaveNewFile()
            '        objLevel.Banner = fuImage.NewFileName
            '        Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(fuImage.MyFile.InputStream)
            '        Dim ImagePath As String = Server.MapPath(fuImage.Folder & "/")
            '        If (img.PhysicalDimension.Width > defaultW Or img.PhysicalDimension.Height > defaultH) Then
            '            Using imgPhoto As System.Drawing.Image = System.Drawing.Image.FromFile(ImagePath & "" & objLevel.Banner)
            '                Dim imgPhotoSub As System.Drawing.Image = Core.ScaleCrop(imgPhoto, defaultW, defaultH, Utility.Common.AnchorPosition.Center, True, False)
            '                imgPhotoSub.Save(ImagePath & "/thumb/" & objLevel.Banner, System.Drawing.Imaging.ImageFormat.Jpeg)
            '                imgPhotoSub.Dispose()
            '            End Using
            '        Else
            '            Utility.File.DeleteFile(ImagePath & "/thumb/" & objLevel.Banner)
            '            System.IO.File.Copy(ImagePath & "/" & objLevel.Banner, ImagePath & "/thumb/" & objLevel.Banner)
            '        End If
            '        ''delete image
            '        Utility.File.DeleteFile(ImagePath & "/" & objLevel.Banner)
            '        ''   Dim ImagePath As String = Server.MapPath(fuImage.Folder & "/")
            '        '' Core.ResizeImage(ImagePath & "" & fuImage.NewFileName, ImagePath & "thumb/" & fuImage.NewFileName, 0, 100)

            '    ElseIf fuImage.MarkedToDelete Then
            '        fuImage.RemoveOldFile()
            '        objLevel.Banner = Nothing
            '    End If
            '    If (fuImage.NewFileName <> String.Empty) Then
            '        objLevel.Id = LevelId
            '        FreeGiftLevelRow.UpdateBanner(objLevel)
            '    End If
            'End If
            Back()
        End If
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Back()

    End Sub
End Class
