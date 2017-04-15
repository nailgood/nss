Imports Components
Imports DataLayer
Imports System.Collections.Generic
Partial Class controls_resource_center_media_top3_media_list
    Inherits BaseControl
    Public Shared sclass As String = ""
    Public index As Integer = 0
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ltrmCategory.Text = "<div class='category'><a href='/media-topic'>As Seen In Media</a></div>"
        ltrViewMore.Text = "<div class='viewmore'><a href='/media-topic'>View More</a></div>"
        Dim lstTop3Media As List(Of VideoRow) = VideoRow.ListTop3Media()
        rptMedia.DataSource = lstTop3Media
        rptMedia.DataBind()
    End Sub

    Protected Sub rptNews_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptMedia.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            index = index + 1
            If (index Mod 3 = 0) Then
                sclass = " vlast"
            Else
                sclass = ""
            End If
            Dim item As VideoRow = e.Item.DataItem
            Dim link As String = URLParameters.MediaDetailUrl(item.Title, item.VideoId)
            Dim thumbImg As String = IIf(System.IO.File.Exists(Server.MapPath("~/" + Utility.ConfigData.MediaThumbPath & item.ThumbImage)), Utility.ConfigData.CDNMediaPath & Utility.ConfigData.MediaThumbPath & item.ThumbImage, Utility.ConfigData.CDNMediaPath & Utility.ConfigData.MediaThumbNoimage)
            Dim ltrdvMedia As Literal = e.Item.FindControl("ltrdvMedia")
            ltrdvMedia.Text &= "<div class='dvVideoItem" & sclass & "'><article>"
            ltrdvMedia.Text &= "<div class='image'><a href='" & link & "'><img src='" & thumbImg & "' alt='" & item.Title & "' /></a></div>"
            ltrdvMedia.Text &= "<div class='title'><a href='" & link & "'>" & item.Title & "</a></div>"
            ltrdvMedia.Text &= "<div class='dvShortDesc'>" & Utility.Common.StringCut(item.ShortDescription, 0, 150) & "</div></article></div>"
        End If
    End Sub
End Class
