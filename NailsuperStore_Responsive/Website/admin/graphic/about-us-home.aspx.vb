Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Controls
Partial Class admin_graphic_about_us_home
    Inherits AdminPage
    Public Id As Integer = 0
    Dim f As New FileUpload
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadData()
        End If
    End Sub

    Private Sub LoadData()
        Dim item As InforBannerCollection = InforBannerRow.GetAllByType(Utility.Common.InforBannerType.AboutUsHome)
        If Not item Is Nothing AndAlso item.Count > 0 Then
            Id = item(0).Id
            txtName.Text = item(0).Name
            txtDescription.Text = item(0).Description
            txtLink.Text = item(0).Link
            chkIsActive.Checked = item(0).IsActive
            hpimg.ImageUrl = Utility.ConfigData.PathBanner & item(0).Image
            fuImage.CurrentFileName = item(0).Image
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim item As New InforBannerRow
            Dim row As InforBannerCollection = InforBannerRow.GetAllByType(Utility.Common.InforBannerType.AboutUsHome)
            If Not row Is Nothing AndAlso row.Count > 0 Then
                Id = row(0).Id
            End If
            item.Name = txtName.Text
            item.Description = txtDescription.Text
            item.Link = txtLink.Text
            item.IsActive = chkIsActive.Checked
            item.Type = Utility.Common.InforBannerType.AboutUsHome
            If Id > 0 Then
                item.Id = Id
                item.Image = row(0).Image
                InforBannerRow.Update(item)
            Else
                Id = InforBannerRow.Insert(item)
            End If
            Dim arr As String()
            If Id > 0 Then
                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = Id.ToString() & "." & arr(1)
                    fuImage.SaveNewFile()
                    item.Image = fuImage.NewFileName
                ElseIf fuImage.MarkedToDelete() Then
                    Utility.File.DeleteFile(Utility.ConfigData.PathBanner & item.Image)
                    item.Image = Nothing
                End If
                item.Id = Id
                InforBannerRow.Update(item)
            End If
            Response.Redirect(Me.Request.RawUrl, False)
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As System.EventArgs) Handles btnDelete.Click
        Dim row As InforBannerCollection = InforBannerRow.GetAllByType(Utility.Common.InforBannerType.AboutUsHome)
        If Not row Is Nothing AndAlso row.Count > 0 Then
            Id = row(0).Id
        End If
        If Id > 0 Then
            Dim thumb As String = row(0).Image
            If (InforBannerRow.Delete(Id)) Then
                f.RemoveFileName(Utility.ConfigData.PathBanner, thumb)
            End If
            Response.Redirect(Me.Request.RawUrl)
        End If
    End Sub

End Class
