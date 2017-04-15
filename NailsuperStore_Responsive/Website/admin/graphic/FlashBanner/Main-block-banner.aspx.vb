



Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_graphic_FlashBanner_Main_block_banner
    Inherits AdminPage

    Protected Id As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        fuImage.ImageDisplayFolder = Utility.ConfigData.PathBanner
        fuImage.Folder = Utility.ConfigData.PathBanner
        If Not IsPostBack Then

            LoadFromDB()
        End If

    End Sub

    Private Sub LoadFromDB()

        Dim dbBanner As BannerRow = BannerRow.GetStaticBanner()
        Id = dbBanner.Id
        txtUrl.Text = dbBanner.Url
        fuImage.CurrentFileName = dbBanner.BannerName
        hpimg.ImageUrl = fuImage.Folder & dbBanner.BannerName & "?i=" & DateTime.Now.ToString()
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



    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        Try
            Dim dbBanner As BannerRow = BannerRow.GetStaticBanner()
            Id = dbBanner.Id
            Dim dbBannerBefore As BannerRow
            Dim logDetail As New AdminLogDetailRow
            If dbBanner.Id <> 0 Then
                dbBannerBefore = CloneObject.Clone(dbBanner)
            Else
                dbBanner = New BannerRow(DB)
            End If
            dbBanner.DepartmentId = Nothing
            dbBanner.StartingDate = Nothing
            dbBanner.EndingDate = Nothing
            dbBanner.IsActive = True
            dbBanner.Url = txtUrl.Text
            dbBanner.IsBlock = True
            If radColor.Checked Then
                dbBanner.Background = txtColor.Text
            End If
            fuImage.AutoResize = True
            If String.IsNullOrEmpty(fuImage.NewFileName) AndAlso String.IsNullOrEmpty(dbBanner.BannerName) Then
                ltMssImage.Text = "Image is required."
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
                    Dim h As Integer = fuImage.ImageHeight
                    fuImage.SaveNewFile()
                    dbBanner.BannerName = fuImage.NewFileName

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
            End If
            If (logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()) Then
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbBanner, Utility.Common.ObjectType.FlashBanner)
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.FlashBanner, dbBannerBefore, dbBanner)
            End If
            logDetail.ObjectId = Id
            logDetail.ObjectType = Utility.Common.ObjectType.FlashBanner.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)


            Response.Redirect(Me.Request.RawUrl)

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
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

