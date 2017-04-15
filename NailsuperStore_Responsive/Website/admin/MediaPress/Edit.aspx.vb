Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net
Partial Class admin_MediaPress_Edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Id = Convert.ToInt32(Request("Id"))
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
        ddlCategory.SelectedValue = cate
    End Sub
    Private Sub LoadCategory()
        Dim collection As CategoryCollection = CategoryRow.ListByType(DB, Utility.Common.CategoryType.MediaPress)
        ddlCategory.DataSource = collection
        ddlCategory.DataTextField = "CategoryName"
        ddlCategory.DataValueField = "CategoryId"
        ddlCategory.DataBind()
        ddlCategory.Items.Insert(0, New ListItem("-- ALL --", String.Empty))
    End Sub
    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim dbVideo As VideoRow = VideoRow.GetRow(DB, Id)
        ddlCategory.SelectedValue = dbVideo.CategoryId
        txtTitle.Text = dbVideo.Title
        txtShortDescription.Text = dbVideo.ShortDescription
        chkIsActive.Checked = dbVideo.IsActive
        fuThumb.CurrentFileName = dbVideo.ThumbImage
        '' fuThumb.Folder = "/" & Utility.ConfigData.VideoThumbPath.Substring(0, Utility.ConfigData.VideoThumbPath.Length - 1)
        ''fuThumb.ImageDisplayFolder = "/" & Utility.ConfigData.VideoThumbPath.Substring(0, Utility.ConfigData.VideoThumbPath.Length - 1)
        hpimg.ImageUrl = Utility.ConfigData.MediaThumbPath & dbVideo.ThumbImage
        txtMetaDescription.Text = dbVideo.MetaDescription
        txtMetaKeyword.Text = dbVideo.MetaKeyword
        txtPageTitle.Text = dbVideo.PageTitle

        divImg.Visible = True
        'If dbVideo.VideoFile <> String.Empty Then
        '    ltrVideo.Text = Utility.Common.GetVideoResource(dbVideo.VideoFile, 300, 200, 0, 0)
        '    trVideo.Visible = True
        'Else
        '    trVideo.Visible = False
        'End If
        'If dbVideo.IsYoutubeImage Then
        '    fuThumb.EnableDelete = False
        'End If

    End Sub
    Dim newImageName As String = ""
    Dim ImagePath As String = ""
    Dim SmallImagePath As String = ""
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try
                Dim oldImage As String = ""
                DB.BeginTransaction()
                Dim dbMediaPressRow As VideoRow
                If Id <> 0 Then
                    dbMediaPressRow = VideoRow.GetRow(DB, Id)
                    oldImage = dbMediaPressRow.ThumbImage
                Else
                    dbMediaPressRow = New VideoRow(DB)
                End If
                dbMediaPressRow.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue)
                dbMediaPressRow.Title = txtTitle.Text.Trim.Trim()
                dbMediaPressRow.ShortDescription = txtShortDescription.Text.Trim()
                dbMediaPressRow.VideoFile = ""
                dbMediaPressRow.IsActive = chkIsActive.Checked
                dbMediaPressRow.MetaKeyword = txtMetaKeyword.Text.Trim()
                dbMediaPressRow.MetaDescription = txtMetaDescription.Text.Trim()
                dbMediaPressRow.PageTitle = txtPageTitle.Text.Trim()
                dbMediaPressRow.CreatedDate = DateTime.Now
                fuThumb.Folder = "~" & Utility.ConfigData.MediaPath
                ImagePath = Server.MapPath("~" & Utility.ConfigData.MediaPath)
                SmallImagePath = Server.MapPath("~" & Utility.ConfigData.MediaThumbPath)
                If fuThumb.NewFileName <> String.Empty Then
                    If (oldImage <> String.Empty) Then
                        newImageName = oldImage
                    Else
                        newImageName = Guid.NewGuid().ToString() & fuThumb.OriginalExtension
                    End If
                    fuThumb.NewFileName = newImageName
                    fuThumb.SaveNewFile()
                   
                ElseIf fuThumb.MarkedToDelete Then
                    Utility.File.DeleteFile(ImagePath & oldImage)
                    Utility.File.DeleteFile(SmallImagePath & oldImage)
                    dbMediaPressRow.ThumbImage = Nothing
                End If
                dbMediaPressRow.IsYoutubeImage = Nothing
                If Id <> 0 Then
                    If fuThumb.NewFileName <> String.Empty Then
                        dbMediaPressRow.ThumbImage = Id & fuThumb.OriginalExtension
                        Core.CropByAnchor(ImagePath & newImageName, SmallImagePath & dbMediaPressRow.ThumbImage, 515, 515, Utility.Common.ImageAnchorPosition.Center)
                    End If
                    VideoRow.Update(DB, dbMediaPressRow)
                Else
                    VideoRow.Insert(DB, dbMediaPressRow)
                    Id = VideoRow.vId
                    If fuThumb.NewFileName <> String.Empty Then
                        dbMediaPressRow.VideoId = Id
                        dbMediaPressRow.ThumbImage = Id & fuThumb.OriginalExtension
                        Core.CropByAnchor(ImagePath & newImageName, SmallImagePath & dbMediaPressRow.ThumbImage, 515, 515, Utility.Common.ImageAnchorPosition.Center)
                        VideoRow.Update(DB, dbMediaPressRow)
                    End If
                End If
                Utility.File.DeleteFile(ImagePath & newImageName)
                If (Id > 0) Then
                    dbMediaPressRow.RemoveAllVideoCategory(DB, Id)
                    dbMediaPressRow.InsertVideoCategory(DB, dbMediaPressRow.CategoryId, Id)
                End If
                DB.CommitTransaction()
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            Catch ex As SqlException
                If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If
    End Sub

    Private Sub ViewError(ByVal message As String)
        lblMessage.Text = "<span class='red'>" + message + "</span>"
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

End Class
