
Imports DataLayer
Imports Components
Partial Class controls_product_video_item_detail
    Inherits BaseControl

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
        LoadData()
    End Sub
    Private Sub LoadData()
        If Not Item Is Nothing AndAlso Not ListVideo Is Nothing AndAlso ListVideo.Rows.Count > 0 Then
            rptVideo.DataSource = ListVideo
            rptVideo.DataBind()
        Else
            Me.Visible = False
        End If
        
    End Sub
    Protected Sub rptVideo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptVideo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim ltrVideo As System.Web.UI.WebControls.Literal = DirectCast(e.Item.FindControl("ltrVideo"), System.Web.UI.WebControls.Literal)
            If Not ltrVideo Is Nothing Then
                Dim drv As DataRowView = e.Item.DataItem
                Dim des As String = drv("description")
                Dim name As String = drv("Name")
                If String.IsNullOrEmpty(name) Then
                    name = des
                End If
                ltrVideo.Text = "<li class='video-item {0}'>"
                If ((e.Item.ItemIndex + 1) Mod 2 <> 0) Then
                    ltrVideo.Text = String.Format(ltrVideo.Text, "smallfirst")
                Else
                    ltrVideo.Text = String.Format(ltrVideo.Text, "smalllast")
                End If
                ltrVideo.Text &= "<a onclick=""return PlayVideo(" & drv("ItemVideoId") & ");"" href='javascript:void(0)'><div class='img'>"
                ltrVideo.Text &= " <img src='" & Utility.ConfigData.CDNMediaPath & GetVideoImage(drv("ThumbImage"), Utility.ConfigData.ItemVideoNoimage) & "' />"
                ltrVideo.Text &= "<div class='play'></div></div></a>"
                ltrVideo.Text &= "<div class='name'><a onclick=""return PlayVideo(" & drv("ItemVideoId") & ");"" href='javascript:void(0)'>" & name & "</a></div></li>"
            End If
        End If
    End Sub


    Public Function GetVideoImage(ByVal thumb As String, ByVal defaultNoImage As String) As String
        Dim path As String = Server.MapPath(Utility.ConfigData.ItemVideoThumbPath) & thumb
        If System.IO.File.Exists(path) Then
            Return Utility.ConfigData.ItemVideoThumbPath & thumb
        Else
            Return defaultNoImage
        End If
    End Function
End Class
