Imports Components
Imports DataLayer
Imports System.IO
Partial Class controls_resource_center_video_popular
    Inherits BaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadPopular()
        End If
    End Sub
    Private Sub LoadPopular()
        Dim categroyId As Integer = CInt(Session("VideoCateId"))
        Dim popular As VideoCollection = VideoRow.ListVideoPopular(Utility.Common.CategoryType.Video, categroyId)
        lstVideoPopular.DataSource = popular
        lstVideoPopular.DataBind()
    End Sub

    Public likeFB As New NailCache()
    Protected Sub lstVideoPopular_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles lstVideoPopular.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim video As VideoRow = e.Item.DataItem
            Dim ltrImage As Literal = e.Item.FindControl("ltrImage")
            Dim ltrTitle As Literal = e.Item.FindControl("ltrTitle")
            Dim ltrViewCount As Literal = e.Item.FindControl("ltrViewCount")
            Dim ltrVoteCount As Literal = e.Item.FindControl("ltrVoteCount")

            ltrImage.Text = "<a href='" & URLParameters.VideoDetailUrl(video.Title, video.VideoId) & "'><img src='" & GetImage(video.ThumbImage) & "' alt='" & video.Title & "' /><div class='smplay'></div></a>"
            ltrTitle.Text = "<a href='" & URLParameters.VideoDetailUrl(video.Title, video.VideoId) & "'>" & video.Title & "</a>"
            ltrViewCount.Text = video.ViewsCount
            ltrVoteCount.Text = likeFB.BindCountLikeFB(ConfigurationManager.AppSettings("GlobalSecureName") & URLParameters.VideoDetailUrl(video.Title, video.VideoId))
        End If
    End Sub

    Private Function GetImage(ByVal image As String) As String
        Dim imagePath As String = Server.MapPath(Utility.ConfigData.VideoThumbPath)
        If File.Exists(imagePath & image) Then
            Return Utility.ConfigData.CDNMediaPath & Utility.ConfigData.VideoThumbPath & image
        End If
        Return Utility.ConfigData.CDNMediaPath & Utility.ConfigData.VideoThumbNoImage
    End Function
End Class
