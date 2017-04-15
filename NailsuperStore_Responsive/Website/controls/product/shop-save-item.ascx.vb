

Imports DataLayer
Imports Components

Partial Class controls_product_shop_save_item
    Inherits BaseControl
    Public itemIndex As Integer = 0
    Public ShopSaveItem As New ShopSaveRow
    Public image As String = String.Empty
    Private picture As String = "<picture>" _
& "<source srcset=""" & Utility.ConfigData.CDNMediaPath & "/assets/shopsave/home/{0}"" media=""(min-width: 992px)"">" _
& "<source srcset=""" & Utility.ConfigData.CDNMediaPath & "/assets/shopsave/home/{1}{0}"" media=""(max-width: 480px)"">" _
& "<img src=""" & Utility.ConfigData.CDNMediaPath & "/assets/shopsave/home/{0}"" alt=""{2}"">" _
& "</picture>"
    Private htmlimg As String = "<img src='" & Utility.ConfigData.CDNMediaPath & "/assets/shopsave/home/{0}' alt='{1}' />"
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If ShopSaveItem IsNot Nothing Then
            Dim path As String = Server.MapPath("~/assets/shopsave/home/")
            Dim img, pathmobile As String
            pathmobile = "mobile/"
            ''ShopSaveItem.HomeBanner = "elegant-gel-acrylic-nail-brush-holder-10slo.jpg"
            'Banner
            img = ShopSaveItem.HomeBanner
            If (Not ShopSaveItem.HomeBanner Is Nothing) And System.IO.File.Exists(path & img) Then
                If Not System.IO.File.Exists(path & pathmobile & img) Then
                    ltimg.Text = String.Format(htmlimg, img, ShopSaveItem.Name)
                Else
                    ltimg.Text = String.Format(picture, img, pathmobile, ShopSaveItem.Name)
                End If
                'image = Utility.ConfigData.ShopSaveBannerFolder & "home/" & ShopSaveItem.HomeBanner
            End If

            'Url
            If String.IsNullOrEmpty(ShopSaveItem.Url) Then
                ShopSaveItem.Url = URLParameters.ShopSaveUrl(ShopSaveItem.Name, ShopSaveItem.ShopSaveId, CType(ShopSaveItem.Type, Utility.Common.ShopSaveType))
            End If
            If String.IsNullOrEmpty(ShopSaveItem.ShortContent) Then
                divDes.Visible = False
            Else
                divDes.Text = ShopSaveItem.ShortContent
            End If
        End If
    End Sub

    Public Function GetItemClass() As String
        If (itemIndex Mod 2 <> 0) Then
            Return "shop-save-item smalllast"
        End If
        Return "shop-save-item smallfirst"
    End Function

End Class
