
Imports DataLayer
Imports Components

Partial Class controls_product_image_item_detail
    Inherits BaseControl
    Protected MediaUrl As String = Utility.ConfigData.MediaUrl
    Public ZoomImagePath As String = String.Empty
    Public ImagePath As String = String.Empty
    Private m_ListVideo As DataTable = Nothing

    Public Property ListVideo() As DataTable
        Set(ByVal value As DataTable)
            m_ListVideo = value
        End Set
        Get
            Return m_ListVideo
        End Get
    End Property
    Private m_Item As StoreItemRow = Nothing

    Public Property Item() As StoreItemRow
        Set(ByVal value As StoreItemRow)
            m_Item = value
        End Set
        Get
            Return m_Item
        End Get
    End Property
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Item Is Nothing AndAlso Not Session("itemRender") Is Nothing Then
            Item = Session("itemRender")
            ListVideo = Session("itemVideoRender")
        End If
        If Not Item Is Nothing Then
            LoadImage(Item)
        Else
            Me.Visible = False
        End If


    End Sub
    Public Sub LoadImage(ByVal objItem As StoreItemRow)
        Dim isAllowZoomImage As Boolean = False

        ''Item.Image = "119019.jpg"
        Dim imgFile As String = objItem.Image

        If Not Core.FileExists(Server.MapPath(MediaUrl & "/items/large/") & imgFile) Then
            imgFile = Utility.ConfigData.NoImageItem
            isAllowZoomImage = False
            ImagePath = Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/"
        Else

            ImagePath = Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/large/"
            ''check big image
            If Core.FileExists(Server.MapPath(MediaUrl & "/items/big/") & objItem.Image) Then
                isAllowZoomImage = True
                '' imgSource.Attributes.Add("style", "display:none")
                ZoomImagePath = Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/big/"
            End If
        End If

        ''show image
        Dim imageHTML As String = "<div id='imgSource' style='display:{0}' allowzoom='{1}' ><img src='" & ImagePath & imgFile & "' alt='" & objItem.ItemName & "' /><meta itemprop='image' content='" & Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/" & imgFile & "'/> </div>"
        If (isAllowZoomImage) Then
            imageHTML = String.Format(imageHTML, "none", "1")
            imageHTML = imageHTML & "<div id='wrap-zoom-image' style='top: 0px;  position: relative;'>"
        Else
            imageHTML = String.Format(imageHTML, "block", "0")
            imageHTML = imageHTML & "<div id='wrap-zoom-image' style='top: 0px;  position: relative;display:none;'>"
        End If
        imageHTML = imageHTML & "<a href='" & ZoomImagePath & imgFile & "' id='azoom' class='cloud-zoom' rel=""position:'right', zoomWidth:510, zoomHeight:510, adjustX:10, adjustY:-1"">"
        imageHTML = imageHTML & "<img style='display: block; border:none;' id='largeImage' src='" & ImagePath & imgFile & "'> </a><div style='' id='imgZoomTip' class='imgZoomTip'><img src='" & Utility.ConfigData.CDNMediaPath & "/includes/scripts/zoom/img_zoom.png'></div></div>"
        divImage.InnerHtml = imageHTML
        'Load Alternate Image
        Dim TotalImage As Integer = 0
        Dim lstImg As StoreItemImageCollection = StoreItemImageRow.GetListImageByItem(objItem.ItemId, 1, Integer.MaxValue, TotalImage)
        Dim liHTML As String = String.Empty
        If Not lstImg Is Nothing AndAlso lstImg.Count() > 0 Then
            For Each objImage As StoreItemImageRow In lstImg
                If Core.FileExists(Server.MapPath(MediaUrl & "/items/featured/") & objImage.Image) Then
                    Dim isZoom As Integer = 0
                    If Core.FileExists(Server.MapPath(MediaUrl & "/items/big/") & objImage.Image) Then
                        isZoom = 1
                    End If
                    liHTML &= "<li " & IIf(lstImg.Count() = 1, "[0]", "") & "><a href='javascript:void(0)' onClick=""updateAlternateimg('" & Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/large/" & "','" & Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/big/" & "', '" & objImage.Image & "'," & isZoom & ")""><img src=" & Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/featured/" & objImage.Image & " alt='" & objImage.ImageAltTag & "' /></a></li>"
                End If
            Next
        End If

        If Not ListVideo Is Nothing AndAlso ListVideo.Rows.Count > 0 Then
            Dim videoImage As String = GetVideoImage(ListVideo.Rows(0)("ThumbImage"), Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ItemVideoThumbNoimage)
            Dim liVideo As String = String.Empty
            liVideo &= "<li class='videothumbnail'><div><a onclick=""{0}"" href='javascript:void(0)'>"
            liVideo &= "     <img src='" & videoImage & "' />"
            liVideo &= "       <div class='play'></div>"
            liVideo &= "    </a></div></li>"
            If ListVideo.Rows.Count > 1 Then
                liVideo = String.Format(liVideo, "return ScrollToSection('#item-detail .data .video');")
            Else
                liVideo = String.Format(liVideo, "return PlayVideo(" & ListVideo.Rows(0)("ItemVideoId") & ");")
            End If
            liHTML = liHTML & liVideo

            If liHTML.Contains("[0]") Then
                liHTML = liHTML.Replace("[0]", "")
            End If
        Else
            If liHTML.Contains("[0]") Then
                liHTML = liHTML.Replace("[0]", "class='thumb1'")
            End If
        End If

        If (Not String.IsNullOrEmpty(liHTML)) Then
            ltrImageList.Text = "<ul class='image-list'>" & liHTML & "</ul>"
        Else
            ltrImageList.Text = String.Empty
        End If
    End Sub
    Public Function GetVideoImage(ByVal thumb As String, ByVal defaultNoImage As String) As String
        Dim path As String = Server.MapPath(Utility.ConfigData.ItemVideoThumbPath) & thumb
        If System.IO.File.Exists(path) Then
            Return Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ItemVideoThumbPath & thumb
        Else
            Return defaultNoImage
        End If
    End Function
End Class
