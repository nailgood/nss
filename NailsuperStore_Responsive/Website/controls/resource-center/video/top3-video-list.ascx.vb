Imports Components
Imports DataLayer
Imports System.Collections.Generic
Partial Class controls_resource_center_video_top3_video_list
    Inherits BaseControl
    Public Shared sclass As String = ""
    Public index As Integer = 0
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ltrmCategory.Text = "<div class='category'><a href='/video-topic'>How To Video</a></div>"
        ltrViewMore.Text = "<div class='viewmore'><a href='/video-topic'>View More</a></div>"
        Dim lstTop3Video As List(Of VideoRow) = VideoRow.ListTop3Video()
        rptVideo.DataSource = lstTop3Video
        rptVideo.DataBind()
    End Sub

    Protected Sub rptNews_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptVideo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            index = index + 1
            If (index Mod 3 = 0) Then
                sclass = " vlast"
            Else
                sclass = ""
            End If
            Dim item As VideoRow = e.Item.DataItem
            Dim link As String = URLParameters.VideoDetailUrl(item.Title, item.VideoId)
            Dim thumbImg As String = IIf(System.IO.File.Exists(Server.MapPath("~/" + Utility.ConfigData.VideoThumbPath & item.ThumbImage)), Utility.ConfigData.VideoThumbPath & item.ThumbImage, Utility.ConfigData.VideoThumbNoImage)
            Dim ltrdvVideo As Literal = e.Item.FindControl("ltrdvVideo")
            ltrdvVideo.Text &= "<div class='dvVideoItem" & sclass & "'><article>"
            ltrdvVideo.Text &= "<div class='image'><a href='" & link & "'><img src='" & Utility.ConfigData.CDNMediaPath & thumbImg & "' alt='" & item.Title & "' /></a></div>"
            ltrdvVideo.Text &= "<div class='title'><a href='" & link & "'>" & item.Title & "</a></div>"
            ltrdvVideo.Text &= "<div class='dvShortDesc'>" & Utility.Common.StringCut(item.ShortDescription, 0, 150) & "</div></article></div>"
        End If
    End Sub
End Class
