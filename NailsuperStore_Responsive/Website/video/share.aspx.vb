
Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Partial Class video_share
    Inherits SitePage
    Protected VideoId As Integer
    Public dbVideo As VideoRow
    Public isLocalVideo As Boolean = False
    Public videoURLImg As String = ""
    Public html5Video As String = String.Empty
    Public flashVideoURL As String = String.Empty
    Public videoTitle As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ResetAction()
        If Not IsPostBack Then
            ltrScript.Text = "<script>GetFrameSize();</script>"
        Else
            If Request("VideoId") <> Nothing Then
                VideoId = Request("VideoId")
            End If
            BindData()

        End If

    End Sub
    Private Sub BindData()
        Dim vW As Integer = 700
        Dim vH As Integer = 426
        If Not String.IsNullOrEmpty(hidFrameW.Value) Then
            vW = CInt(hidFrameW.Value)
            vW = vW - 10
        End If
        If Not String.IsNullOrEmpty(hidFrameH.Value) Then
            vH = CInt(hidFrameH.Value)
            vH = vH - 10
        End If
        dbVideo = VideoRow.GetRow(DB, VideoId)
        If (dbVideo.VideoId < 1) Then
            Exit Sub
        End If
        videoTitle = dbVideo.Title
        Dim autoplay As String = "autoplay=1"
        Dim url As String = Request.RawUrl 'sua khi load link embed video trong iframe se play video neu autoplay
        If url.Contains("?") Then
            ltCanonical.Text = "<link rel=""canonical"" href=""" & Utility.ConfigData.GlobalRefererName & url.Split("?")(0) & """/>"            autoplay = "autoplay=0"        End If
        If (dbVideo.VideoFile.Contains("youtube.com/")) Then
            isLocalVideo = False
            ltVideo.Text = Utility.Common.GetVideoResource(dbVideo.VideoFile, vW, vH, 1, 0).Replace("autoplay=1", autoplay)
            ltVideo.Visible = True
        Else
            Dim imagePath As String = Server.MapPath(Utility.ConfigData.VideoThumbPath)
            If System.IO.File.Exists(imagePath & dbVideo.ThumbImage) Then
                videoURLImg = Utility.ConfigData.VideoThumbPath & dbVideo.ThumbImage
            End If
            If (IsiPad() Or IsiPhone()) Then
                html5Video = "<video autoplay='autoplay' width='" & vW & "' height='" & vH & "' controls='controls'>"
                html5Video &= "   <source type='video/mp4'  src='" & dbVideo.VideoFile & "'></source>"
                html5Video &= " Video not playing? <a  href='" & dbVideo.VideoFile & "'>Download file</a> instead.</video>"
            Else
                flashVideoURL = dbVideo.VideoFile.Replace(".mp4", ".flv")
                isLocalVideo = True
                ltVideo.Visible = False
            End If
        End If
        ltrScript.Text = "<script>BuildFlashVideo(" & vW & "," & vH & ");</script>"
    End Sub
    Public Sub ResetAction()
        Dim f As System.Web.UI.HtmlControls.HtmlForm = Me.Page.Form
        f.Action = Me.Request.RawUrl
    End Sub

End Class
