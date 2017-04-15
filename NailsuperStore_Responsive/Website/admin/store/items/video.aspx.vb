Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Net
Partial Class admin_store_items_video
    Inherits AdminPage
    Protected params As String
    Private TotalRecords As Integer
    Public Property ItemVideoId() As Int32

        Get
            Dim o As Object = ViewState("ItemVideoId")
            If o IsNot Nothing Then
                Return DirectCast(o, Int32)
            End If
            Return Int32.MinValue
        End Get

        Set(ByVal value As Int32)
            ViewState("ItemVideoId") = value
        End Set
    End Property
    Public Property ItemId() As Int32

        Get
            Dim o As Object = ViewState("ItemId")
            If o IsNot Nothing Then
                Return DirectCast(o, Int32)
            End If
            Return Int32.MinValue
        End Get

        Set(ByVal value As Int32)
            ViewState("ItemId") = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load



        ItemId = Request("ItemId")
        params = "ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All)
        fuThumb.Folder = "/" & Utility.ConfigData.ItemVideoPath.Substring(0, Utility.ConfigData.ItemVideoPath.Length - 1)
        fuThumb.ImageDisplayFolder = "/" & Utility.ConfigData.ItemVideoThumbPath.Substring(0, Utility.ConfigData.ItemVideoThumbPath.Length - 1)
        If Not IsPostBack Then
            If IsNumeric(ItemId) AndAlso ItemId > 0 Then
                BindDataGrid()
                LoadItem()
            End If
            
        End If
    End Sub
    Private Sub LoadItem()
        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
        lblItemName.Text = item.ItemName
    End Sub
    Private Sub BindDataGrid()
        Dim lstResult As StoreItemVideoCollection = StoreItemVideoRow.ListByItemId(ItemId)
        TotalRecords = lstResult.Count
        rptList.DataSource = lstResult
        rptList.DataBind()
        If TotalRecords > 0 Then
            plcHasRecords.Visible = True
            plcNoRecords.Visible = False
        Else
            plcHasRecords.Visible = False
            plcNoRecords.Visible = True
        End If
    End Sub

   
    Private Sub InsertVideo(ByVal item As StoreItemVideoRow)
        'If StoreItemVideoRow.Insert(DB, item) Then
        StoreItemVideoRow.Insert(item)
        Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
        'Else
        'AddError("Insert Error")
        'End If
    End Sub
    Private Sub UpdateVideo(ByVal item As StoreItemVideoRow)
        If StoreItemVideoRow.Update(item) Then
            Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
        Else
            AddError("")
        End If
    End Sub
    Private Sub LoadDetail()
        Dim video As StoreItemVideoRow = StoreItemVideoRow.GetRow(DB, ItemVideoId)
        txtUrl.Text = video.Url
        txtDescription.Text = video.Description
        chkIsActive.Checked = video.IsActive
        fuThumb.CurrentFileName = video.ThumbImage
        hpimg.ImageUrl = "/" & Utility.ConfigData.ItemVideoThumbPath & video.ThumbImage



    End Sub
    Protected Sub rptList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptList.ItemCommand
        Dim id As Integer = Convert.ToInt32(e.CommandArgument)
        If e.CommandName = "Delete" Then
            ''Delete ThumbImage
            DeleteThumbvideo(id)
            StoreItemVideoRow.Delete(DB, id)
            Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))

        ElseIf e.CommandName = "Edit" Then
            ItemVideoId = id
            LoadDetail()
        ElseIf e.CommandName = "Active" Then
            StoreItemVideoRow.ChangeIsActive(DB, id)
            Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
        ElseIf e.CommandName = "Up" Then
            StoreItemVideoRow.ChangeArrangeItem(DB, id, True)
            Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
        ElseIf e.CommandName = "Down" Then
            StoreItemVideoRow.ChangeArrangeItem(DB, id, False)
            Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
    Private Sub DeleteThumbvideo(ByVal itemId As Integer)
        Dim objItem As StoreItemVideoRow = StoreItemVideoRow.GetRow(DB, itemId)
        Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.ItemVideoPath) & objItem.ThumbImage
        Dim SmallImagePath As String = Server.MapPath("~/" & Utility.ConfigData.ItemVideoThumbPath) & objItem.ThumbImage
        Utility.File.DeleteFile(ImagePath)
        Utility.File.DeleteFile(SmallImagePath)
    End Sub
    Dim ImagePath As String = String.Empty
    Dim SmallImagePath As String = String.Empty
    Dim newImageName As String = String.Empty
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect(Me.Request.RawUrl)
    End Sub


    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ImagePath = Server.MapPath("~/" & Utility.ConfigData.ItemVideoPath)
        SmallImagePath = Server.MapPath("~/" & Utility.ConfigData.ItemVideoThumbPath)
        Try
            Dim dbItem As New StoreItemVideoRow
            Dim oldImage As String = String.Empty
            Dim oldLink As String = String.Empty
            If (ItemVideoId > 0) Then
                dbItem = StoreItemVideoRow.GetRow(DB, ItemVideoId)
                oldImage = dbItem.ThumbImage
                oldLink = dbItem.Url
            End If
            dbItem.Url = txtUrl.Text.Trim()
            dbItem.Description = txtDescription.Text.Trim()
            dbItem.IsActive = chkIsActive.Checked
            dbItem.CreatedDate = DateTime.Now
            dbItem.ItemId = ItemId

            If fuThumb.NewFileName <> String.Empty Then
                ''newImageName = newImageName & fuThumb.OriginalExtension
                newImageName = GetImagename(dbItem.Description, fuThumb.OriginalExtension)
                If (oldImage <> String.Empty) Then
                    Utility.File.DeleteFile(SmallImagePath & oldImage)
                End If
                fuThumb.NewFileName = newImageName
                dbItem.ThumbImage = newImageName
                fuThumb.SaveNewFile()
                Core.CropByAnchor(ImagePath & newImageName, SmallImagePath & newImageName, 195, 115, Utility.Common.ImageAnchorPosition.Center)
                Utility.File.DeleteFile(ImagePath & newImageName)
            Else
                If (oldLink.Trim.ToLower() <> txtUrl.Text.Trim.ToLower And txtUrl.Text.Contains("youtube.com")) Then
                    dbItem.ThumbImage = GetYoutubeThumbImage()
                    Utility.File.DeleteFile(SmallImagePath & oldImage)
                End If
                
            End If
            If ItemVideoId > 0 Then
                dbItem.ItemVideoId = ItemVideoId
                UpdateVideo(dbItem)
            Else
                InsertVideo(dbItem)
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
        End Try
    End Sub
    Private Function GetImagename(ByVal imageName As String, ByVal extFile As String) As String
        If (String.IsNullOrEmpty(imageName)) Then
            Return Guid.NewGuid().ToString() & extFile
        End If
        Dim result As String = Utility.Common.GenerateCodeString(imageName)
        Dim img1 As String = ImagePath & result & extFile
        Dim img2 As String = SmallImagePath & result & extFile
        Dim index As Integer = 0
        Dim isExists As Boolean = True
        While (isExists)
            If (System.IO.File.Exists(img1) Or System.IO.File.Exists(img2)) Then
                index = index + 1
                result = result & "_" & DateTime.Now.ToString("HH_mm_ss")
                img1 = ImagePath & result & extFile
                img2 = SmallImagePath & result & extFile
            Else
                isExists = False
            End If
        End While
        Return result & extFile
    End Function
    Private Function GetYoutubeThumbImage() As String
        Dim youtubeUrl As String = StoreItemVideoRow.ConvertLinkVideoToImage(txtURL.Text.Trim())
        If (youtubeUrl <> String.Empty) Then
            Dim indexExtension As String = youtubeUrl.LastIndexOf(".")
            If indexExtension > 0 Then
                ''  newImageName = Guid.NewGuid().ToString()
                Dim extension As String = youtubeUrl.Substring(indexExtension + 1, youtubeUrl.Length - (youtubeUrl.Substring(0, indexExtension).Length + 1))
                newImageName = GetImagename(txtDescription.Text.Trim(), "." & extension)
                ' Create an instance of WebClient
                Dim client As New WebClient()
                ' Hookup DownloadFileCompleted Event
                '' client.DownloadFileCompleted += New System.ComponentModel.AsyncCompletedEventHandler(AddressOf client_DownloadFileCompleted)
                AddHandler client.DownloadFileCompleted, AddressOf client_DownloadFileCompleted
                ' Start the download and copy the file to c:\temp
                client.DownloadFileAsync(New Uri(youtubeUrl), ImagePath & newImageName)
                Return newImageName
            End If
        End If
        Return String.Empty
    End Function
    Private Sub client_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        ''Create Thumb Image
        Core.CropByAnchor(ImagePath & newImageName, SmallImagePath & newImageName, 195, 115, Utility.Common.ImageAnchorPosition.Center)
        Utility.File.DeleteFile(ImagePath & newImageName)
    End Sub
    Protected Sub rptList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptList.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim tab As StoreItemVideoRow = e.Item.DataItem

            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)

            If TotalRecords < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                If e.Item.ItemIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                    imbDown.Visible = False
                End If
            End If
            Dim ltrImg As Literal = CType(e.Item.FindControl("ltrImg"), Literal)
            If Not ltrImg Is Nothing Then
                ltrImg.Text = "<img src='" & Utility.ConfigData.ItemVideoThumbPath & tab.ThumbImage & " '/>"
            End If
        End If
    End Sub
End Class