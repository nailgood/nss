

Imports DataLayer
Imports Components

Partial Class controls_banner_infor_banner_item
    Inherits BaseControl
    Public itemIndex As Integer = 0
    Public BannerItem As New InforBannerRow
    'Public image As String = String.Empty
    Private picture As String = "<picture>" _
& "<source srcset=""" & Utility.ConfigData.CDNMediaPath & "/assets/inforbanner/{0}"" media=""(min-width: 992px)"">" _
& "<source srcset=""" & Utility.ConfigData.CDNMediaPath & "/assets/inforbanner/{1}{0}"" media=""(max-width: 480px)"">" _
& "<img src=""" & Utility.ConfigData.CDNMediaPath & "/assets/inforbanner/{0}"" alt=""{2}"">" _
& "</picture>"
    Private htmlimg As String = "<img src='" & Utility.ConfigData.CDNMediaPath & "/assets/inforbanner/{0}' alt='{1}' />"
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim path As String = Server.MapPath("~/assets/inforbanner/")
        Dim img, pathmobile As String
        pathmobile = "mobile/"
        If BannerItem IsNot Nothing Then
            img = BannerItem.Image
            ''ShopSaveItem.HomeBanner = "elegant-gel-acrylic-nail-brush-holder-10slo.jpg"
            'Banner
            If (Not BannerItem.Image Is Nothing) And System.IO.File.Exists(path & img) Then
                If Not System.IO.File.Exists(path & pathmobile & img) Then
                    ltimg.Text = String.Format(htmlimg, img, BannerItem.Name)
                Else
                    ltimg.Text = String.Format(picture, img, pathmobile, BannerItem.Name)
                End If
            End If
            End If
    End Sub

    Public Function GetItemClass() As String
        If (itemIndex Mod 2 <> 0) Then
            Return "infor-banner-item smalllast"
        End If
        Return "infor-banner-item smallfirst"
    End Function

End Class
