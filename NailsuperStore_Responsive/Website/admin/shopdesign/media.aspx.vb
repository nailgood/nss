Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Net

Partial Class admin_shopdesign_media
    Inherits AdminPage

    Public Id As Integer = 0
    Public ShopDesignId As Integer = 0
    Public TypeMedia As Integer
    Dim Total As Integer = 0
    Protected lblFile As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ShopDesignId = CInt(Request("ShopDesignId"))
        TypeMedia = CInt(Request("Type"))
        lblFile = IIf(TypeMedia = Utility.Common.ShopDesignMediaType.Image, "<td class='required'>File</td>", "<td class='optional'>Image</td>")
        If Not Request.QueryString("Id") Is Nothing AndAlso IsNumeric(Request.QueryString("Id")) Then
            Id = CInt(Request.QueryString("Id"))
        End If

        If Not IsPostBack Then
            If Id > 0 Then
                LoadDetail()
            End If
            BindList()
        End If
    End Sub

    Private Sub LoadDetail()
        Dim row As ShopDesignMediaRow = ShopDesignMediaRow.GetRow(DB, Id)

        If row IsNot Nothing Then
            ltrHeader.Text &= [Enum].GetName(GetType(Utility.Common.ShopDesignMediaType), TypeMedia).ToString()
            txtTag.Text = row.Tag
            txtUrl.Text = row.Url

            If row.Type = Utility.Common.ShopDesignMediaType.Image Then
                hpimg.ImageUrl = Utility.ConfigData.ShopDesignSmallPath & row.Url
                file.CurrentFileName = row.Url
            Else
                divVideo.Visible = True
                file.CurrentFileName = row.Id & ".jpg"
                hpimg.ImageUrl = Utility.ConfigData.ShopDesignVideoThumbPath & row.Id & ".jpg"
                ltrVideo.Text = Utility.Common.GetVideoResource(row.Url, 300, 200, 0, 0)
                txtDesc.Text = row.Description
            End If

            If file.CurrentFileName Is Nothing Then
                divImg.Visible = False
            End If
        End If

    End Sub

    Private Sub BindList()
        Dim list As ShopDesignMediaCollection = ShopDesignMediaRow.ListByShopDesignId(ShopDesignId, TypeMedia)
        If list IsNot Nothing AndAlso list.Count > 0 Then
            Total = list.Count
            gvList.Pager.NofRecords = Total
            gvList.PageSelectIndex = gvList.PageIndex
            gvList.DataSource = list
            gvList.DataBind()
        End If
       
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("media.aspx?ShopDesignId=" & ShopDesignId & "&Type=" & TypeMedia)
    End Sub

    Dim oldImg As String = ""
    Dim newNameImg As String = ""
    Dim ImgPath As String = Server.MapPath("~" & Utility.ConfigData.ShopDesignPath)
    Dim smallImg As String = Server.MapPath("~" & Utility.ConfigData.ShopDesignSmallPath)
    Dim largeImg As String = Server.MapPath("~" & Utility.ConfigData.ShopDesignLargePath)
    Dim videoImg As String = Server.MapPath("~" & Utility.ConfigData.ShopDesignVideoThumbPath)
    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then
            Exit Sub
        End If

        Dim media As New ShopDesignMediaRow
        Dim mediaBefore As New ShopDesignMediaRow
        Dim logdetail As New AdminLogDetailRow

        Try
            If Id > 0 Then
                media = ShopDesignMediaRow.GetRow(DB, Id)
                mediaBefore = CloneObject.Clone(media)
                If TypeMedia = Utility.Common.ShopDesignMediaType.Image Then
                    oldImg = media.Url
                Else
                    oldImg = media.Id & ".jpg"
                End If
            End If

            media.ShopDesignId = ShopDesignId
            media.Tag = txtTag.Text
            media.Type = TypeMedia
            Dim filename = file.NewFileName
            Dim formatFile As String = file.OriginalExtension.Replace(".", "")
            If file.NewFileName <> String.Empty Then
                If Not Utility.ConfigData.AllowImageUpload().Contains(formatFile) Then
                    AddError("Image is invalid format.")
                    Exit Sub
                End If
                'filename = filename & "." & formatFile
                file.SaveNewFile()
            End If

            If TypeMedia = Utility.Common.ShopDesignMediaType.Image Then
                If (String.IsNullOrEmpty(file.CurrentFileName) And String.IsNullOrEmpty(file.NewFileName)) Then
                    lblImgError.Visible = True
                    lblImgError.Text = "File is required."
                    Exit Sub
                End If

                'Neu update thi dung lai ten cu, khong can delete
                If oldImg <> Nothing Then
                    newNameImg = oldImg
                Else
                    newNameImg = Guid.NewGuid().ToString & "." & formatFile
                End If
                Core.CropByAnchor(ImgPath & file.NewFileName, smallImg & newNameImg, 80, 80, Utility.Common.ImageAnchorPosition.Center)
                Core.CropByAnchor(ImgPath & file.NewFileName, largeImg & newNameImg, 748, 420, Utility.Common.ImageAnchorPosition.Center)
                media.Url = newNameImg
                Utility.File.DeleteFile(ImgPath & filename)
            Else
                If String.IsNullOrEmpty(txtUrl.Text) Then
                    lblUrlError.Visible = True
                    lblUrlError.Text = "Please input Video URL."
                    Exit Sub
                End If
                media.Url = txtUrl.Text
                media.Description = txtDesc.Text
            End If
            If Id > 0 Then
                media.Update()
                logdetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logdetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ShopDesignMedia, mediaBefore, media)
            Else
                media.Id = media.Insert()
                logdetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logdetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(media, Utility.Common.ObjectType.ShopDesignMedia)
            End If

            newNameImg = media.Id & ".jpg"
            If (TypeMedia = Utility.Common.ShopDesignMediaType.Video) AndAlso txtUrl.Text.Contains("www.youtube.com/") Then
                GetYoutubeThumbImage()
            ElseIf TypeMedia = Utility.Common.ShopDesignMediaType.Video AndAlso file.NewFileName <> String.Empty Then
                Core.CropByAnchor(ImgPath & file.NewFileName, smallImg & newNameImg, 80, 80, Utility.Common.ImageAnchorPosition.Center)
                Core.CropByAnchor(ImgPath & file.NewFileName, videoImg & newNameImg, 195, 115, Utility.Common.ImageAnchorPosition.Center)
                Utility.File.DeleteFile(ImgPath & filename)
            End If

            logdetail.ObjectId = media.Id.ToString()
            logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignMedia.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logdetail)
            Response.Redirect("media.aspx?ShopDesignId=" & ShopDesignId & "&Type=" & TypeMedia)
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Private Sub GetYoutubeThumbImage()
        Dim youtubeUrl As String = StoreItemVideoRow.ConvertLinkVideoToImage(txtUrl.Text.Trim())
        If (youtubeUrl <> String.Empty) Then
            Dim indexExtension As String = youtubeUrl.LastIndexOf(".")
            If indexExtension > 0 Then
                Dim extension As String = youtubeUrl.Substring(indexExtension + 1, youtubeUrl.Length - (youtubeUrl.Substring(0, indexExtension).Length + 1))
                newNameImg = IIf(newNameImg.LastIndexOf(".") > 0, newNameImg, newNameImg & "." & extension)
                ' Create an instance of WebClient
                Dim client As New WebClient()
                ' Hookup DownloadFileCompleted Event
                '' client.DownloadFileCompleted += New System.ComponentModel.AsyncCompletedEventHandler(AddressOf client_DownloadFileCompleted)
                AddHandler client.DownloadFileCompleted, AddressOf client_DownloadFileCompleted
                ' Start the download and copy the file to c:\temp
                client.DownloadFileAsync(New Uri(youtubeUrl), ImgPath & newNameImg)
            End If
        End If
    End Sub

    Private Sub client_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        ''Create Thumb Image
        Core.CropByAnchor(ImgPath & newNameImg, smallImg & newNameImg, 80, 80, Utility.Common.ImageAnchorPosition.Center)
        Core.CropByAnchor(ImgPath & newNameImg, videoImg & newNameImg, 195, 115, Utility.Common.ImageAnchorPosition.Center)
        Utility.File.DeleteFile(ImgPath & newNameImg)
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As ShopDesignMediaRow = e.Row.DataItem
            Dim lrtUrl As Literal = e.Row.FindControl("lrtUrl")
            Dim imbUp As ImageButton = e.Row.FindControl("imbUp")
            Dim imbDown As ImageButton = e.Row.FindControl("imbDown")
            If TypeMedia = Utility.Common.ShopDesignMediaType.Image Then
                lrtUrl.Text = "<img src='" & Utility.ConfigData.ShopDesignSmallPath & row.Url & "' alt='' />"
            Else
                lrtUrl.Text = "<a href='/embed/how-to-video/" & row.Id & "'>" & "/embed/how-to-video/" & row.Id & "</a>"
            End If
            If e.Row.RowIndex = 0 AndAlso Total > 1 Then
                imbUp.Visible = False
            ElseIf e.Row.RowIndex = Total - 1 AndAlso Total > 1 Then
                imbDown.Visible = False
            ElseIf Total < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            End If
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        Dim logdetail As New AdminLogDetailRow
        If e.CommandName = "Delete" Then
            Dim row As ShopDesignMediaRow = ShopDesignMediaRow.GetRow(DB, e.CommandArgument)
            ShopDesignMediaRow.Delete(e.CommandArgument)
            logdetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logdetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(row, Utility.Common.ShopDesignMediaType.Image)
            logdetail.ObjectId = e.CommandArgument
            logdetail.ObjectType = Utility.Common.ShopDesignMediaType.Image.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logdetail)
        ElseIf e.CommandName = "Up" Then
            ShopDesignMediaRow.ChangeSortOrder(e.CommandArgument, ShopDesignId, TypeMedia, True)
        ElseIf e.CommandName = "Down" Then
            ShopDesignMediaRow.ChangeSortOrder(e.CommandArgument, ShopDesignId, TypeMedia, False)
        End If
        Response.Redirect("media.aspx?ShopDesignId=" & ShopDesignId & "&Type=" & TypeMedia)
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?" & GetPageParams(Components.FilterFieldType.All))
    End Sub
End Class
