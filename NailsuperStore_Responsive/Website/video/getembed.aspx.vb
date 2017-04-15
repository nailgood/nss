Imports Components
Imports DataLayer
Partial Class video_get_embed
    Inherits SitePage
    Private VideoId As String = String.Empty
    Public dbVideo As New VideoRow
    Dim url As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Public embeb As String = "<iframe width=""480"" height=""360"" scrolling=""no""  src=""http://192.168.41.64:2015//embed/how-to-video/218"" frameborder=""0""></iframe>"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        VideoId = Session("VideoId")
        'Session("embedIframe") = Nothing
        BindData()
    End Sub
    Private Function CreateLinkEmbedVideo(ByVal video As VideoRow) As String
        Dim result As String = "<iframe width=""{0}"" height=""{1}"" scrolling=""no""  src=""" & url & "/embed/how-to-video/" & video.VideoId & "?autoplay=0"" frameborder=""0""></iframe>"
        Return result
    End Function
    Private Sub BindData()
        dbVideo = VideoRow.GetRow(DB, VideoId)
        dbVideo.ViewsCount = dbVideo.ViewsCount + 1
        VideoRow.Update(DB, dbVideo)
        embeb = CreateLinkEmbedVideo(dbVideo)
    End Sub
End Class
