Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net

Partial Class admin_Video_Video_Edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Private strIsChecked As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Id = Convert.ToInt32(Request("Id"))
        lbUploadfile.Text = Utility.ConfigData.AllowImageUpload()
        If Not IsPostBack Then
            LoadCategory()
            GetParamater()
            LoadFromDB()
        End If
    End Sub
    Public Sub GetParamater()
        Dim cate As String = ""
        Try
            cate = Request.QueryString("F_Category")
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LoadCategory()
        Dim dt As DataTable = CategoryRow.GetCategoryById(DB, Id, Utility.Common.CategoryType.Video)
        Dim count As Integer = dt.Rows.Count

        If count > 0 Then
            cblCategory.DataSource = dt.DefaultView
            cblCategory.DataTextField = "CategoryName"
            cblCategory.DataValueField = "CategoryId"
            cblCategory.DataBind()
            For i As Integer = 0 To count - 1
                If dt.Rows(i)("IsChecked") = 1 Then
                    If strIsChecked = "" Then
                        strIsChecked &= dt.Rows(i)("CategoryId")
                    Else
                        strIsChecked &= "," & dt.Rows(i)("CategoryId")
                    End If
                End If
            Next
        End If
        cblCategory.SelectedValues = strIsChecked
    End Sub
    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim dbVideo As VideoRow = VideoRow.GetRow(DB, Id)


        txtName.Text = dbVideo.Title
        txtURL.Text = dbVideo.VideoFile
        txtShortDescription.Text = dbVideo.ShortDescription
        chkIsActive.Checked = dbVideo.IsActive
        fuThumb.CurrentFileName = dbVideo.ThumbImage
        fuThumb.Folder = Utility.ConfigData.VideoThumbPath.Substring(0, Utility.ConfigData.VideoThumbPath.Length - 1)
        fuThumb.ImageDisplayFolder = Utility.ConfigData.VideoThumbPath.Substring(0, Utility.ConfigData.VideoThumbPath.Length - 1)
        hpimg.ImageUrl = Utility.ConfigData.VideoThumbPath & dbVideo.ThumbImage
        txtMetaDescription.Text = dbVideo.MetaDescription
        txtMetaKeyword.Text = dbVideo.MetaKeyword
        txtPageTitle.Text = dbVideo.PageTitle

        divImg.Visible = True
        If dbVideo.VideoFile <> String.Empty Then
            ltrVideo.Text = Utility.Common.GetVideoResource(dbVideo.VideoFile, 300, 200, 0, 0)
            trVideo.Visible = True
        Else
            trVideo.Visible = False
        End If
        If dbVideo.IsYoutubeImage Then
            fuThumb.EnableDelete = False
        End If
       
    End Sub
    Dim newImageName As String = ""
    Dim ImagePath As String = ""
    Dim ThumbImagePath As String = ""
    Dim LargeImagePath As String = ""
    Dim RelatedThumbPath As String = ""
    Dim SmallImagePath As String = ""
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''check image available jpg, gif
        strIsChecked = cblCategory.SelectedValues
        If strIsChecked = "" Then
            lblCategory.Text = "Please select category."
            Exit Sub
        End If
        Dim sFileName As String = fuThumb.NewFileName
        If (sFileName <> "") Then
            Dim checkimage As String = sFileName.Substring(sFileName.LastIndexOf(".") + 1)
            Dim AllowImage() As String = Utility.ConfigData.AllowImageUpload().Split(",")
            Dim i As Integer = 0
            Dim check As Integer = 0
            For i = 0 To AllowImage.Length - 1
                If (AllowImage(i).Equals(checkimage.ToLower())) Then
                    check += 1
                End If
            Next
            If (check = 0) Then
                AddError("Image is not valid.")
                Exit Sub
            End If
        End If

        If Page.IsValid Then
            Try
                Dim oldImage As String = ""
                Dim oldIsYouTubeImage As Boolean = False
                Dim dbVideoRow As VideoRow
                Dim dbVideoBefore As New VideoRow
                Dim changeLog As String = String.Empty
                If Id <> 0 Then
                    dbVideoRow = VideoRow.GetRow(DB, Id)
                    oldImage = dbVideoRow.ThumbImage
                    oldIsYouTubeImage = dbVideoRow.IsYoutubeImage
                    dbVideoBefore = CloneObject.Clone(dbVideoRow)
                    dbVideoBefore.ListCategoryId = VideoRow.GetListCategoryIdByVideoId(DB, Id)
                Else
                    dbVideoRow = New VideoRow(DB)
                End If

                'dbVideoRow.CategoryId = Convert.ToInt32(cblCategory.SelectedValue)
                dbVideoRow.Title = txtName.Text.Trim.Trim()
                dbVideoRow.ShortDescription = txtShortDescription.Text.Trim()
                dbVideoRow.VideoFile = txtURL.Text.Trim()
                dbVideoRow.SubTitle = txtSubTitle.Text.Trim()
                dbVideoRow.IsActive = chkIsActive.Checked
                dbVideoRow.MetaKeyword = txtMetaKeyword.Text.Trim()
                dbVideoRow.MetaDescription = txtMetaDescription.Text.Trim()
                dbVideoRow.PageTitle = txtPageTitle.Text.Trim()
                dbVideoRow.IsYoutubeImage = False
                'newImageName = Guid.NewGuid().ToString() & fuThumb.OriginalExtension
                fuThumb.Folder = "~" & Utility.ConfigData.VideoPath
                ImagePath = Server.MapPath("~" & Utility.ConfigData.VideoPath)
                ThumbImagePath = Server.MapPath("~" & Utility.ConfigData.VideoThumbPath)
                LargeImagePath = Server.MapPath("~" & Utility.ConfigData.VideoLargePath)
                RelatedThumbPath = Server.MapPath("~" & Utility.ConfigData.VideoRelatedThumbPath)
                'SmallImagePath = Server.MapPath("~" & Utility.ConfigData.VideoSmallPath)
                If fuThumb.NewFileName <> String.Empty Then
                    If (oldImage <> String.Empty) Then
                        fuThumb.NewFileName = oldImage
                    End If
                    fuThumb.SaveNewFile()
                ElseIf fuThumb.MarkedToDelete Then
                    '' dbVideoRow.ThumbImage = Nothing
                    ''Delete Old File
                    Utility.File.DeleteFile(ImagePath & oldImage)
                    Utility.File.DeleteFile(RelatedThumbPath & oldImage)
                    Utility.File.DeleteFile(ThumbImagePath & oldImage)
                    Utility.File.DeleteFile(LargeImagePath & oldImage)
                End If
                Dim logDetail As New AdminLogDetailRow
                If Id <> 0 Then
                    newImageName = IIf(fuThumb.NewFileName <> String.Empty, dbVideoRow.VideoId & fuThumb.OriginalExtension, dbVideoRow.ThumbImage)
                    dbVideoRow.ThumbImage = newImageName
                    If fuThumb.NewFileName <> String.Empty Then

                        ''get thum nail from youtube
                        Core.CropByAnchor(ImagePath & fuThumb.NewFileName, ThumbImagePath & newImageName, 268, 150, Utility.Common.ImageAnchorPosition.Center)
                        Core.CropByAnchor(ImagePath & fuThumb.NewFileName, LargeImagePath & newImageName, 871, 492, Utility.Common.ImageAnchorPosition.Center)
                        Core.CropByAnchor(ImagePath & fuThumb.NewFileName, RelatedThumbPath & newImageName, 170, 96, Utility.Common.ImageAnchorPosition.Center)
                        Utility.File.DeleteFile(ImagePath & fuThumb.NewFileName)
                    ElseIf (txtURL.Text.Contains("www.youtube.com/")) Then
                        GetYoutubeThumbImage()
                        dbVideoRow.IsYoutubeImage = True
                    End If
                    VideoRow.Update(DB, dbVideoRow)

                    dbVideoRow.RemoveAllVideoCategory(DB, Id)
                    dbVideoRow.InsertVideoCategory(DB, strIsChecked, Id)
                    dbVideoRow.ListCategoryId = VideoRow.GetListCategoryIdByVideoId(DB, Id)
                    changeLog = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Video, dbVideoBefore, dbVideoRow)
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                Else
                    dbVideoRow.CreatedDate = DateTime.Now
                    VideoRow.Insert(DB, dbVideoRow)
                    dbVideoRow.InsertVideoCategory(DB, strIsChecked, VideoRow.vId)
                    dbVideoRow.VideoId = VideoRow.vId
                    dbVideoRow.ListCategoryId = VideoRow.GetListCategoryIdByVideoId(DB, dbVideoRow.VideoId)
                    newImageName = IIf(fuThumb.NewFileName <> String.Empty, dbVideoRow.VideoId & fuThumb.OriginalExtension, dbVideoRow.VideoId & ".jpg")
                    dbVideoRow.ThumbImage = newImageName
                    If fuThumb.NewFileName <> String.Empty Then
                        Core.CropByAnchor(ImagePath & fuThumb.NewFileName, ThumbImagePath & newImageName, 268, 150, Utility.Common.ImageAnchorPosition.Center)
                        Core.CropByAnchor(ImagePath & fuThumb.NewFileName, LargeImagePath & newImageName, 874, 492, Utility.Common.ImageAnchorPosition.Center)
                            Core.CropByAnchor(ImagePath & fuThumb.NewFileName, RelatedThumbPath & newImageName, 170, 96, Utility.Common.ImageAnchorPosition.Center)
                            Utility.File.DeleteFile(ImagePath & fuThumb.NewFileName)
                    ElseIf txtURL.Text.Contains("www.youtube.com/") Then
                        ''get thum nail from youtube
                        GetYoutubeThumbImage()
                        dbVideoRow.IsYoutubeImage = True
                    End If
                    VideoRow.Update(DB, dbVideoRow)
                    changeLog = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbVideoRow, Utility.Common.ObjectType.Video)
                    logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                End If
                logDetail.Message = changeLog
                logDetail.ObjectId = dbVideoRow.VideoId
                logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If
    End Sub
    Private Sub GetYoutubeThumbImage()
        Dim youtubeUrl As String = StoreItemVideoRow.ConvertLinkVideoToImage(txtURL.Text.Trim())
        If (youtubeUrl <> String.Empty) Then
            Dim indexExtension As String = youtubeUrl.LastIndexOf(".")
            If indexExtension > 0 Then
                Dim extension As String = youtubeUrl.Substring(indexExtension + 1, youtubeUrl.Length - (youtubeUrl.Substring(0, indexExtension).Length + 1))
                ' Create an instance of WebClient
                Dim client As New WebClient()
                ' Hookup DownloadFileCompleted Event
                '' client.DownloadFileCompleted += New System.ComponentModel.AsyncCompletedEventHandler(AddressOf client_DownloadFileCompleted)
                AddHandler client.DownloadFileCompleted, AddressOf client_DownloadFileCompleted
                ' Start the download and copy the file to c:\temp
                client.DownloadFileAsync(New Uri(youtubeUrl), ImagePath & newImageName)
            End If
        End If
    End Sub

    Private Sub client_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        ''Create Thumb Image
        Core.CropByAnchor(ImagePath & newImageName, ThumbImagePath & newImageName, 360, 202, Utility.Common.ImageAnchorPosition.Center)
        Core.CropByAnchor(ImagePath & newImageName, LargeImagePath & newImageName, 770, 280, Utility.Common.ImageAnchorPosition.Center)
        Core.CropByAnchor(ImagePath & newImageName, RelatedThumbPath & newImageName, 170, 96, Utility.Common.ImageAnchorPosition.Center)
        Utility.File.DeleteFile(ImagePath & newImageName)
    End Sub


    Private Sub ViewError(ByVal message As String)
        lblMessage.Text = "<span class='red'>" + message + "</span>"
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

End Class

