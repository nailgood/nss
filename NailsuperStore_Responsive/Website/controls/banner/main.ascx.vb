Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class controls_MainBanner
    Inherits BaseControl
    
    Protected SecondAnniversary As String = String.Empty
    'Protected isSlider As Boolean = True
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            _Load()
        End If
    End Sub

    '---------------------------------------------
    'Private picture As String = "<picture>" _
    '     & "<source srcset=""/assets/banner/{0}"" media=""(min-width: 992px)"">" _
    '     & "<source srcset=""/assets/banner/{1}"" media=""(min-width: 541px)"">" _
    '     & "<source srcset=""/assets/banner/{2}"" media=""(max-width: 540px)"">" _
    '     & "<img srcset=""/assets/banner/{2}"" alt=""test"">" _
    '     & "</picture>"
    'Private picture As String = "<picture>" _
    '   & "<source srcset=""/assets/banner/{0}"" media=""(min-width: 992px)"">" _
    '   & "<source srcset=""/assets/banner/mobile/{0}"" media=""(max-width: 540px)"">" _
    '   & "<img srcset=""/assets/banner/{0}"" alt=""{1}"">" _
    '   & "</picture>"
    Private picture As String = "<picture>" _
    & "<source srcset=""" & Utility.ConfigData.CDNMediaPath & "/assets/banner/{0}"" media=""(min-width: 992px)"">" _
    & "<source srcset=""" & Utility.ConfigData.CDNMediaPath & "/assets/banner/{1}{0}"" media=""(max-width: 540px)"">" _
    & "<img src=""" & Utility.ConfigData.CDNMediaPath & "/assets/banner/{0}"" alt=""Banner-{0}"">" _
    & "</picture>"
    Private htmlimg As String = "<img src='" & Utility.ConfigData.CDNMediaPath & "/assets/banner/{0}' alt='Banner-{0}' />"
    Private Sub _Load()

        If Utility.ConfigData.IsEnableBlockBanner() Then
            Me.Visible = False
            Exit Sub
        End If

        Dim lstBannerData As BannerCollection = BannerRow.ListByDepartmentId(Utility.ConfigData.RootDepartmentID)
        If Not lstBannerData Is Nothing AndAlso lstBannerData.Count > 0 Then
            Dim html As String = String.Empty
            Dim typeimg, img, pathmobile As String
            pathmobile = "mobile/"
            Dim path As String = Server.MapPath("~/assets/banner/")
            'If lstBannerData.Count = 1 Then
            '    isSlider = False
            '    ulData.Attributes("class") = Nothing
            '    Dim litJs As Literal = CType(Page.Master.FindControl("litScriptCustom"), Literal)
            '    litJs.Text = litJs.Text.Replace("<script type=""text/javascript""  src=""/includes/scripts/bxslider/jquery.bxslider.js?v=" & SitePage.cssVersion & """ ></script>", "")
            'End If
            For Each objBanner As BannerRow In lstBannerData
                If objBanner.IsBlock Then
                    Continue For
                End If
                'img = "/assets/banner/" & objBanner.BannerName
                img = objBanner.BannerName
                If System.IO.File.Exists(path & objBanner.BannerName) Then
                    If Not System.IO.File.Exists(path & pathmobile & img) Then
                        typeimg = String.Format(htmlimg, img)
                    Else
                        typeimg = String.Format(picture, img, pathmobile)
                    End If
                    If String.IsNullOrEmpty(objBanner.Url) Then
                        'html &= "<li><img src='" & img & "' /></li>"
                        html &= "<li>" & typeimg & "</li>"
                    Else
                        'html &= "<li><a href='" & objBanner.Url & "'><img src='" & img & "' /></a></li"
                        html &= "<li><a href='" & objBanner.Url & "'>" & typeimg & "</a></li>"
                    End If
                End If
            Next
            If Not String.IsNullOrEmpty(html) Then
                ulData.InnerHtml = html
            Else
                Me.Visible = False
            End If
        Else
            Me.Visible = False
        End If

        SecondAnniversary = DateTime.Now.ToString("yyyy-MM-dd") & "T" & DateTime.Now.ToString("HH:mm:ss")
    End Sub
End Class
