Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Namespace Components

    Partial Class Popup_ProductVideo
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
                If (Not Request.QueryString("vid") Is Nothing) Then
                    videoId = Request.QueryString("vid")
                    If (videoId > 0) Then
                        BindData(videoId)
                        ShowError(False)
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                '' Email.SendError("ToError500", "PopupVideo,url=" & Request.RawUrl, "Error:" & ex.Message)
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
            Dim dtVideo As DataTable = DB.GetDataTable("select si.ItemName,siv.url,isnull(siv.ThumbImage,'') as ThumbImage,isnull(siv.description,'') as description from StoreItem si inner join storeitemvideo siv on si.ItemId = siv.itemid where  siv.ItemVideoId = " & VideoId)
            If (dtVideo Is Nothing) Then
                Exit Sub
            End If
            If (dtVideo.Rows.Count < 1) Then
                Exit Sub
            End If
            Dim url As String = dtVideo.Rows(0)("url")
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
                'Dim img As String = dtVideo.Rows(0)("ThumbImage")
                'Dim path As String = Server.MapPath("/" & Utility.ConfigData.ItemVideoThumbPath) & img
                'If System.IO.File.Exists(path) Then
                '    videoURLImg = "/" & Utility.ConfigData.ItemVideoThumbPath & img
                'Else
                '    videoURLImg = "/includes/theme/images/play.png" '' & Utility.ConfigData.ItemVideoThumbNoimage
                'End If
                ''Dim imagePath As String = Server.MapPath("/" & Utility.ConfigData.ItemVideoThumbPath)
                ''If System.IO.File.Exists(imagePath & img) Then
                ''    videoURLImg = "/" & Utility.ConfigData.ItemVideoThumbPath & img
                ''End If
                html5Video = Utility.Common.ConvertHTML5Video(url, "")
            End If
            divTitle.InnerText = dtVideo.Rows(0)("description")
        End Sub

    End Class
End Namespace