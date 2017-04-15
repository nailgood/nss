Imports Components
Imports DataLayer
Imports System.IO
Partial Class controls_resource_center_video_video
    Inherits BaseControl
    'Private bErrorCountLike As Boolean = False
    'Public likeFB As New NailCache()
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Fill(ByVal item As VideoRow, ByVal itemindex As Integer)
        '' item.ThumbImage = "1.jpg"
        Dim linkTop As String = URLParameters.VideoDetailUrl(item.Title, item.VideoId)
       
        Dim cssClass As String = "dvVideoItem"
        If (itemindex Mod 3 = 0) AndAlso (Not Request.QueryString("cateId") Is Nothing Or Request.RawUrl.Contains("search-result.aspx")) Then
            cssClass = cssClass & " vlast"
        ElseIf (itemindex Mod 3 = 0) Then
            cssClass = cssClass & " slast"
        End If
        divVideo.Text = "<div id='videoItem_" & itemindex.ToString() & "' class='" & cssClass & "'>"
        divVideo.Text &= "<article><div class='image' id='imgVideo_" & itemindex.ToString() & "'><a href='" & linkTop & "'><img src='" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.VideoThumbPath & item.ThumbImage & "' alt='" & item.Title & "' /><div class='vplay'></div></a></div>"
        divVideo.Text &= "<div class='title'><a href='" & linkTop & "'>" & item.Title & "</a></div>"
        divVideo.Text &= "<div class='dvDate'><div class='date'>" & String.Format("{0:MMM d, yyyy}", item.CreatedDate) & "</div><div class='viewTotal'><span id='iconview'></span>" & item.ViewsCount & "</div>"
        'divVideo.Text &= "<div class='viewTotal'><span id='ivote'></span>" & iCountLike & "</div>"
        divVideo.Text &= "<div class='viewTotal'><span id='icomment'></span>" & item.ReviewsCount & "</div></div>"
        divVideo.Text &= "<div class='dvShortDesc'>" & Utility.Common.StringCut(item.ShortDescription, 0, 150) & "</div>"
        divVideo.Text &= "</article></div>"
    End Sub
End Class
