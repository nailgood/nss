Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Partial Class Popup_ShopDesignVideo

    Inherits SitePage
    Protected VideoId As Integer
    Public isLocalVideo As Boolean = False
    Public videoURLImg As String = ""
    Public html5Video As String = String.Empty
    Public flashVideoURL As String = String.Empty
    Public videoTitle As String = String.Empty

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim videoId As Integer = 0
        Try
            If (Not Request.QueryString("id") Is Nothing) Then
                videoId = Request.QueryString("id")
                If (videoId > 0) Then
                    BindData(videoId)
                    ShowError(False)
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "Popup_ShopDesignVideo,url=" & Request.RawUrl, "Error:" & ex.Message)
        End Try

        ShowError(True)
    End Sub

    Public Sub ShowError(ByVal Status As Boolean)
        If (Status) Then
            tblError.Visible = True
            videoPopup.Visible = False
            divTitle.Visible = False
        Else
            tblError.Visible = False
            videoPopup.Visible = True
            divTitle.Visible = True
        End If
    End Sub
    Private Sub BindData(ByVal VideoId As Integer)
        Dim row As ShopDesignMediaRow = ShopDesignMediaRow.GetRow(DB, VideoId)
        If row.Id > 0 Then
            Dim url As String = row.Url
            If (url.Contains("youtube.com/") Or url.Contains("youtu.be/")) Then
                isLocalVideo = False
                Dim w As Integer = "99"
                Dim h As Integer = "99"
                Dim IsPecent As Boolean = True
                Try
                    If Not Request.QueryString("vH") Is Nothing Then
                        h = CInt(Request.QueryString("vH"))
                    End If
                    If Not Request.QueryString("vW") Is Nothing Then
                        w = CInt(Request.QueryString("vW"))
                    End If
                    If Not Request.QueryString("vP") Is Nothing Then
                        IsPecent = CBool(Request.QueryString("vP"))
                    End If
                Catch ex As Exception

                End Try
                ltVideo.Text = Utility.Common.GetVideoResource(url, w, h, 1, 0, IsPecent)
                ltVideo.Visible = True
            Else
                html5Video = Utility.Common.ConvertHTML5Video(url, "")
            End If
            divTitle.InnerText = row.Tag
        End If

    End Sub

End Class
