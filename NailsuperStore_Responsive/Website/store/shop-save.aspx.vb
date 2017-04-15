Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common

Partial Class Store_ShopSave
    Inherits SitePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadList()
        End If
    End Sub

    Private Sub LoadList()
        Dim ShopSaveId As Integer = 0

        'Check valid urlcode
        If String.IsNullOrEmpty(GetQueryString("ShopSaveCode")) = False Then 'Shopsave havent urlcode
            Utility.Common.CheckValidURLCode(GetQueryString("ShopSaveCode"))
        End If

        'Check valid id
        If GetQueryString("ShopSaveId") <> Nothing AndAlso IsNumeric(GetQueryString("ShopSaveId")) Then
            ShopSaveId = CInt(GetQueryString("ShopSaveId"))
        Else
            Response.Redirect("/deals-center")
        End If

        Dim row As ShopSaveRow = ShopSaveRow.GetRow(DB, ShopSaveId)
        If row IsNot Nothing AndAlso Not String.IsNullOrEmpty(row.Name) Then
            Dim sUrl As String = LCase(Request.RawUrl)
            Dim urlCode As String = LCase(URLParameters.ReplaceUrl(row.Name))
            If sUrl.Contains(urlCode) = False Then
                If row.Type = 6 Then
                    Utility.Common.Redirect301(URLParameters.ShopSaveUrl(row.Name, row.ShopSaveId, Utility.Common.ShopSaveType.WeeklyEmail))
                End If
                If row.Type = 1 Then
                    Utility.Common.Redirect301(URLParameters.ShopSaveUrl(row.Name, row.ShopSaveId, Utility.Common.ShopSaveType.ShopNow))
                End If
            End If

            litDepTitle.Text = row.Name
            Dim desc As String = String.Empty
            Dim hidedesc As String = String.Empty

            If Not String.IsNullOrEmpty(row.Content) Then
                desc = row.Content
                If (desc.Contains("[break]")) Then
                    Dim arr() As String = desc.Split(New String() {"[break]"}, StringSplitOptions.None)
                    If (arr.Length > 0) Then
                        desc = arr(0)
                        hidedesc = arr(1)
                    End If
                End If
            Else
                desc = row.ShortContent
            End If

            litDescription.Text = IIf(hidedesc <> String.Empty, desc & "<span class='morecontent'>" & hidedesc & "</span> <span class='moreellipses'>...<img src='/includes/theme/images/plus.png'><span></span>&nbsp;</span>", desc)
            litDescription.Text = BBCodeHelper.ConvertBBCodeToHTML(litDescription.Text)

            'Banner
            If Not String.IsNullOrEmpty(row.Banner) Then
                Dim path As String = Server.MapPath("~" & Utility.ConfigData.ShopSaveBannerFolder & "banner/")
                If System.IO.File.Exists(path & row.Banner) Then
                    Dim picture As String = "<div id=""ssbanner""><picture>" _
                    & "<source srcset=""" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ShopSaveBannerFolder & "banner/{0}"" media=""(min-width: 992px)"">" _
                    & "<source srcset=""" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ShopSaveBannerFolder & "banner/mobile/{0}"" media=""(max-width: 540px)"">" _
                    & "<img src=""" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ShopSaveBannerFolder & "banner/{0}"" alt=""{1}"">" _
                    & "</picture></div>"
                    litBanner.Text = String.Format(picture, row.Banner, row.Name)
                End If
            End If

            'Meta Tags
            Dim pagetitle As String = Utility.Common.GetPageTitleByCustomerType(Utility.Common.IsUSCustomer(), row.PageTitle, row.OutsideUSPageTitle, row.Name)

            Dim objMetaTag As New MetaTag
            objMetaTag.PageTitle = pagetitle
            objMetaTag.MetaDescription = row.MetaDescription
            objMetaTag.MetaKeywords = row.MetaKeyword

            If sUrl.Contains("/promotion/") And sUrl.Contains("?") Then
                objMetaTag.Canonical = Utility.ConfigData.GlobalRefererName & sUrl.Split("?")(0)
            End If
            SetPageMetaSocialNetwork(Page, objMetaTag)
            'ucListProduct.ShopSaveId = ShopSaveId
        Else
            litDepTitle.Text = Resources.Msg.NotFoundShopSave
        End If
    End Sub

End Class
