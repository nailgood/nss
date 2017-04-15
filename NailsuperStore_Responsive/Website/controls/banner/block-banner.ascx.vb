
Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class controls_banner_block_banner
    Inherits BaseControl
    Protected background As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _Load()
    End Sub

    '---------------------------------------------
    Private Sub _Load()
        If Not Utility.ConfigData.IsEnableBlockBanner() Then
            Me.Visible = False
            Exit Sub
        End If
        'Load MainBlockBanner 
        Dim dbBanner As BannerRow = BannerRow.GetStaticBanner()
        If Not dbBanner Is Nothing Then
            Dim path As String = Server.MapPath("~/assets/banner/")
            Dim img As String = Utility.ConfigData.CDNMediaPath & "/assets/banner/"
            Dim pathmobile As String = String.Empty
            Dim pathdesk As String = String.Empty
            Dim strImg As String = "<img src=""{0}"" {1} alt=""The Nail Superstore"">"

            If System.IO.File.Exists(path & "m_" & dbBanner.BannerName) Then
                pathmobile = String.Format(strImg, img & "m_" & dbBanner.BannerName, " data-big='" & img & dbBanner.BannerName & "' class='visible-xs'")
            End If
            If System.IO.File.Exists(path & dbBanner.BannerName) Then
                pathdesk = String.Format(strImg, img & dbBanner.BannerName, IIf(String.IsNullOrEmpty(pathmobile), "", " class='hidden-xs'"))
                If String.IsNullOrEmpty(dbBanner.Url) Then
                    'divBanner.InnerHtml = "<img src='" & img & "' class=""lazy"" />"
                    divBanner.InnerHtml = pathdesk & pathmobile
                Else
                    divBanner.InnerHtml = "<a href='" & dbBanner.Url & "'>" & pathdesk & pathmobile & "</a>"
                End If
            End If
            If Not String.IsNullOrEmpty(dbBanner.Background) Then
                If dbBanner.Background.Contains(".") Then
                    background = String.Format("background: url('" & Utility.ConfigData.CDNMediaPath & "/assets/banner/background/{0}') fixed center", dbBanner.Background)
                Else
                    background = String.Format("background-color:{0}", dbBanner.Background)
                End If
                divBanner.Attributes.Add("style", background)
            End If

        End If
        ''load SubBlockBanner
        Dim lstSubBlockBanner As InforBannerCollection = InforBannerRow.GetMainPage(Utility.Common.InforBannerType.SubBlockBanner, 3)
        If Not lstSubBlockBanner Is Nothing AndAlso lstSubBlockBanner.Count > 0 Then
            Dim html As String = "<ul>"
            Dim image As String = String.Empty
            For Each objSubBlockBanner As InforBannerRow In lstSubBlockBanner
                If (Not objSubBlockBanner.Image Is Nothing) Then
                    image = Utility.ConfigData.CDNMediaPath & Utility.ConfigData.PathSubBlockBanner & "/thumb/" & objSubBlockBanner.Image
                Else
                    image = String.Empty
                End If
                html &= "<li><a href='" & objSubBlockBanner.Link & "'>"
                If Not String.IsNullOrEmpty(image) Then
                    html &= "<div class='icon'><img src='" & image & "'></div>"
                End If
                html &= "<div class='title'>" & objSubBlockBanner.Name & "</div>"
                html &= "<div class='desc'>" & objSubBlockBanner.Description & "</div></a></li>"
            Next
            divSubBlockBanner.InnerHtml = html & "</ul>"

        End If
    End Sub
End Class
