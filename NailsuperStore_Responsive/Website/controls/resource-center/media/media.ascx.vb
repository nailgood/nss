Imports Components
Imports DataLayer
Imports System.IO
Partial Class controls_resource_center_media_media
    Inherits BaseControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Fill(ByVal Media As VideoRow, ByVal itemindex As Integer)
        Dim detailURL As String = URLParameters.MediaDetailUrl(Media.Title, Media.VideoId)
        Dim img As String = GetImage(Media.ThumbImage)
        dvMediaItem.Text = IIf((itemindex Mod 3) = 0, "<div id='mediaItem_" & itemindex.ToString() & "' class='dvMediaItem vlast'><article>", "<div id='mediaItem_" & itemindex.ToString() & "' class='dvMediaItem'><article>")
        dvMediaItem.Text &= "<div id='imgMedia_" & itemindex.ToString() & "' class='image'>" & String.Format(" <a href=""{0}""><img src='" & img & "' alt='" & Media.Title & "' /></a>", detailURL) & "</div>"
        dvMediaItem.Text &= "<div class='title'>" & String.Format("<a href=""{0}"">{1}</a>", detailURL, Media.Title) & "</div>"
        dvMediaItem.Text &= "<div class='dvShortDesc'>" & Utility.Common.StringCut(Media.ShortDescription, 0, 100) & "</div>"
        dvMediaItem.Text &= "</article></div>"
    End Sub

    Private Function GetImage(ByVal image As String) As String
        Dim imagePath As String = Server.MapPath(Utility.ConfigData.MediaThumbPath)
        If File.Exists(imagePath & image) Then
            Return Utility.ConfigData.CDNMediaPath & Utility.ConfigData.MediaThumbPath & image
        End If
        Return Utility.ConfigData.CDNMediaPath & Utility.ConfigData.MediaThumbNoimage
    End Function
End Class
