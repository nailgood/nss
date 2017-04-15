
Imports System.Configuration.ConfigurationManager
Imports System.Web
Imports System.IO
Imports System.Globalization

Namespace Utility
    Public Class ConfigData

        Public Shared Function USerClearCache() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("UserClearCache")
            Catch ex As Exception
            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "trinh"
            End If
            Return result
        End Function
        Public Shared Function MSDSFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MSDSFolder")
            Catch ex As Exception
            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "D:\Website\NailSuperstore\www\upload\msds"
            End If
            Return result
        End Function

        Public Shared Function MaxMindUriService() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MaxMindUriService")
            Catch ex As Exception
            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "https://minfraud.maxmind.com/app/ccv2r"
            End If
            Return result
        End Function
        Public Shared Function MaxMindLicense() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MaxMindLicense")
            Catch ex As Exception
            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "ND7xHAZ9aAUP"
            End If
            Return result
        End Function

        Public Shared Function HolidayDate() As Date()
            Dim resultStr As String = String.Empty
            Try
                resultStr = AppSettings("HolidayDate")
                If Not String.IsNullOrEmpty(resultStr) Then
                    Dim provider As CultureInfo = CultureInfo.InvariantCulture
                    Dim formatString As String = "MM/dd/yyyy"
                    Dim dateStr As String() = resultStr.Split(";")
                    If dateStr.Count > 0 Then
                        Dim result(dateStr.Count) As Date
                        For index = 0 To dateStr.Count - 1
                            Dim holiday As Date = Nothing
                            Date.TryParseExact(dateStr(index), formatString, provider, DateTimeStyles.None, holiday)
                            result(index) = holiday
                        Next
                        Return result
                    End If
                End If
            Catch ex As Exception
                Return Nothing
            End Try
            Return Nothing
        End Function
        Public Shared Function HourShipping() As Integer
            Dim result As Integer = 13
            Try
                Integer.TryParse(AppSettings("HourShipping"), result)
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function GetPhysicalPageName(ByVal WebRoot As String, ByVal url As String) As String
            Dim root As String = WebRoot
            Dim page As String = url.Replace(WebRoot, String.Empty)
            Dim indexParam As Integer = page.IndexOf("?")
            If (indexParam >= 0) Then
                page = page.Substring(0, indexParam)
            End If
            Return page
        End Function
        Public Shared Function PathPromotionMobile() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathPromotionMobile")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/SalesPrice/mobile/"
            End If
            Return result
        End Function
        Public Shared Function TotalEnableSignature() As Double
            Dim result As Double = 0
            Try
                result = DataLayer.SysParam.GetValue("TotalEnableSignature")
            Catch ex As Exception

            End Try
            If (result <= 0) Then
                result = 100
            End If
            Return result
        End Function
        Public Shared Function FreeShippingOrderAmount() As Double
            Dim result As Double = 0
            Try
                result = DataLayer.SysParam.GetValue("FreeShippingOrderAmount")
            Catch ex As Exception

            End Try
            If (result <= 0) Then
                result = 99
            End If
            Return result
        End Function
        Public Shared Function GAProperty() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("GAProperty")
            Catch ex As Exception

            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "UA-4783679-10"
            End If
            Return result
        End Function
        Public Shared Function AuthorMetaTag() As String
            Dim result As String = String.Empty
            Try
                result = DataLayer.SysParam.GetValue("AuthorMetaTag")
            Catch ex As Exception

            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "Kevin Huynh, The Nail Superstore"
            End If
            Return result
        End Function
        Public Shared Function IsEnableInforBanner() As Boolean
            Dim result As String = DataLayer.SysParam.GetValue("EnableInforBanner")
            If Not (String.IsNullOrEmpty(result)) AndAlso (CBool(result)) Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function IsEnableBlockBanner() As Boolean
            Dim result As String = DataLayer.SysParam.GetValue("EnableBlockBanner")
            If Not (String.IsNullOrEmpty(result)) AndAlso (CBool(result)) Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function NoImageItem() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("NoImageItem")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "no_image.gif"
            End If
            Return result
        End Function
        Public Shared Function NoImageDepartment() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("NoImageDepartment")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "noimage.gif"
            End If
            Return result
        End Function
        Public Shared Function PathPromotion() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathPromotion")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/SalesPrice/"
            End If
            Return result
        End Function
        Public Shared Function ActiveAccountTemplatePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ActiveAccountTemplatePath")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "D:\Website\NailSuperstore\www\includes\MailTemplate\ActiveAccount.htm"
            End If
            Return result
        End Function

        Public Shared Function ActiveAccountGuestTemplatePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ActiveAccountGuestTemplatePath")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "D:\Website\NailSuperstore\www\includes\MailTemplate\ActiveAccountGuest.htm"
            End If
            Return result
        End Function

        Public Shared Function GroupChoiceThumbPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("GroupChoiceThumbPath")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/groupchoice"
            End If
            Return result
        End Function
        Public Shared Function DefaultMobileBannerWidth() As Integer
            Dim result As Integer
            Try
                result = CInt(AppSettings("DefaultMobileBannerWidth"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                Return 768
            End If
        End Function

        Public Shared Function ShopSaveBannerFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ShopSaveBannerFolder")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/shopsave/"
            End If
            Return result
        End Function
        Public Shared Function DepartmentTabImageFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DepartmentTabImageFolder")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/departments/tab/"
            End If
            Return result
        End Function
        Public Shared Function DepartmentMainImageFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DepartmentMainImageFolder")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/departments/large/"
            End If
            Return result
        End Function
        Public Shared Function DepartmentMainNoImagePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DepartmentMainNoImagePath")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/departments/noimage.gif"
            End If
            Return result
        End Function
        Public Shared Function DepartmentPopupImageFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DepartmentPopupImageFolder")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/departments/"
            End If
            Return result
        End Function
        Public Shared Function DefaultVideoBannerName() As String
            Return "video-center"
        End Function
        Public Shared Function PageCalculateShipping() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageCalculateShipping")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "payment.aspx,revise-cart.aspx,billing.aspx,billingint.aspx,address.aspx"
            End If
            Return result
        End Function
        Public Shared Function SubjectSendTracking() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("SubjectSendTracking")
            Catch ex As Exception
            End Try
            If String.IsNullOrEmpty(result) Then
                result = "Your Order Tracking Number!"
            End If
            Return result
        End Function
        Public Shared Function VideoBannerFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("VideoBannerFolder")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/video/banner/"
            End If
            Return result
        End Function
        Public Shared Function VideoCategoryBannerFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("VideoCategoryBannerFolder")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/video/banner/category/"
            End If
            Return result
        End Function

        Public Shared Function EnableUSPSRate() As Boolean
            Dim result As String = String.Empty
            Try
                result = AppSettings("EnableUSPSRate")
            Catch ex As Exception

            End Try
            If result = "1" Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function FirstClassLimitWeight() As Double
            Dim result As Double = 3.5
            Try
                If Not String.IsNullOrEmpty(AppSettings("FirstClassLimitWeight")) Then
                    result = CDbl(AppSettings("FirstClassLimitWeight"))
                End If
            Catch ex As Exception
                result = 3.5
            End Try

            Return result
        End Function

        Public Shared Function AddPointUseReferFriendMailTemplate() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("AddPointUseReferFriendMailTemplate")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "D:\Website\NailSuperstore\www\includes\MailTemplate\AddPointUseReferFriend.htm"
            End If
            Return result
        End Function
        Public Shared Function DefaultMailErrorFrom() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DefaultMailErrorFrom")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "error@nss.com"
            End If
            Return result
        End Function
        Public Shared Function PathBanner() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathBanner")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/Banner/"
            End If
            Return result
        End Function
        Public Shared Function DefaultMailErrorFromName() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DefaultMailErrorFromName")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "Error"
            End If
            Return result
        End Function
        Public Shared Function DefaultMailErrorTo() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DefaultMailErrorTo")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "webmaster@nss.com"
            End If
            Return result
        End Function
        Public Shared Function DefaultMailErrorToName() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DefaultMailErrorToName")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "Webmaster"
            End If
            Return result
        End Function
        ''' <summary>
        ''' ''''''''''''''
        ''' </summary>
        ''' <param name="url"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRawPageName(ByVal url As String) As String
            Dim arr As String() = Split(url, "/")
            Dim result As String = String.Empty
            Try
                result = arr(1).ToString()
            Catch ex As Exception

            End Try
            If (result.Contains(".aspx")) Then
                Return result
            End If
            If (result.Contains("?")) Then
                Dim indexParam As Integer = result.IndexOf("?")
                If (indexParam >= 0) Then
                    result = result.Substring(0, indexParam)
                End If
            End If
            Return "/" + result
        End Function

        Public Shared Function CssPagePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("CssPagePath")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "includes/theme/css/css.xml"
            End If
            Return result
        End Function

        Public Shared Function SessionPagePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("SessionPagePath")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/store/SessionPage.xml"
            End If
            Return result
        End Function

        Public Shared Function LiveDomain() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LiveDomain")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "https://www.nss.com/"
            End If
            Return result
        End Function
        Public Shared Function MinimumWordReview() As Integer
            Dim result As Integer = 0
            Try
                result = CInt(AppSettings("MinimumWordReview"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 10
            End If
            Return result
        End Function
        Public Shared Function AppliedHazMatFee() As Integer
            Dim result As Integer = 0
            Try
                If AppSettings("AppliedHazMatFee") Is Nothing Then
                    Return 20
                End If
                result = CInt(AppSettings("AppliedHazMatFee"))
            Catch ex As Exception
                result = 20
            End Try

            Return result
        End Function

        Public Shared Function UKAllowViewPage() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("UKAllowViewPage")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function CacheTimeDepartment() As Integer
            Dim result As Integer = 10
            Try
                result = Convert.ToInt32(AppSettings("DepartmentCacheTime"))
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function AllowImageUpload() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("AllowImageUpload")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "jpg, gif"
            End If
            Return result
        End Function
        Public Shared Function PageWriteReview() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageWriteReview")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "https://www.nss.com/store/review.aspx"
            End If
            Return result
        End Function
        'Public Shared Function ReviewTemplateFile() As String
        '    Dim result As String = String.Empty
        '    Try
        '        result = AppSettings("ReviewTemplateFile")
        '    Catch ex As Exception

        '    End Try
        '    If result = String.Empty Then
        '        result = "/includes/Template/reviewitem.htm"
        '    End If
        '    Return result
        'End Function

        Public Shared Function ReviewTemplateFile() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ReviewTemplateFile")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/includes/ViewTemplate/"
            End If
            Return result
        End Function


        Public Shared Function DefaultLanguage() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("DefaultLanguage")
            Catch ex As Exception

            End Try
            If (String.IsNullOrEmpty(result)) Then
                result = "desc"
            End If
            Return result
        End Function
        Public Shared Function GlobalRefererName() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("GlobalRefererName")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "https://www.nss.com"
            End If
            Return result
        End Function

        Public Shared Function LogMailPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("sLogMailPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\admin\EmailLog"
            End If
            Return result
        End Function
        Public Shared Function LogSearchFilePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LogSearchFilePath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\admin\EmailLog\logsearch.txt"
            End If
            Return result
        End Function
        Public Shared Function RequestCatalogEmail() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("RequestCatalogEmail")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "no-reply@nss.com"
            End If
            Return result
        End Function

        Public Shared Function FreesampleFolderUpload() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("freesampleFolderUpload")
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function RequireSelectJobPosting() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("RequireSelectJobPosting")
                If result <> "0" And result <> "1" Then
                    result = "0"
                End If
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function BlogId() As Integer
            Dim result As Integer = 12
            Try
                result = Convert.ToInt32(AppSettings("Blog"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function DefaultBrandShow() As Integer
            Dim result As Integer = 7
            Try
                result = Convert.ToInt32(AppSettings("DefaultBrandShow"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 7
            End If
            Return result
        End Function
        Public Shared Function MainPageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("mainPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 8
            End If
            Return result
        End Function
        Public Shared Function ProductReviewPageSize() As Integer
            Dim result As Integer = 4
            Try
                result = Convert.ToInt32(AppSettings("ProductReviewPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 4
            End If
            Return result
        End Function
        Public Shared Function FreeSamplePageSize() As Integer
            Dim result As Integer = 12
            Try
                result = Convert.ToInt32(AppSettings("freeSamplePageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function RelatedItemPageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("relatedItemPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 8
            End If
            Return result
        End Function
        Public Shared Function RecentlyviewItemPageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("recentlyviewItemPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 8
            End If
            Return result
        End Function
        Public Shared Function CategoryPageSize() As Integer
            Dim result As Integer = 12
            Try
                result = Convert.ToInt32(AppSettings("categoryPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function ShopNowPageSize() As Integer
            Dim result As Integer = 12
            Try
                result = Convert.ToInt32(AppSettings("shopNowPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function HomePageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("homePageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function BlogPageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("BlogPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 8
            End If
            Return result
        End Function
        Public Shared Function PageSizeMobile() As Integer
            Dim result As Integer = 12
            Try
                result = Convert.ToInt32(AppSettings("PagesizeMobile"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function PageSizeScroll() As Integer
            Dim result As Integer = 12
            Try
                result = Convert.ToInt32(AppSettings("PageSizeScroll"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function ScrollPurchaseProduct() As Integer
            Dim result As Integer = 5
            Try
                result = Convert.ToInt32(AppSettings("PageSizeScroll"))
            Catch ex As Exception
                result = 0
            End Try
            If (result < 1) Then
                result = 5
            End If
            Return result
        End Function
        Public Shared Function PageSizeCollection() As Integer
            Dim result As Integer = 18
            Try
                result = Convert.ToInt32(AppSettings("PageSizeCollection"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 18
            End If
            Return result
        End Function
        Public Shared Function TimeCacheData() As Integer
            Dim result As Integer = 43200
            Try
                result = Convert.ToInt32(AppSettings("timeCacheData"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 43200
            End If
            Return result
        End Function
        Public Shared Function TimeCacheMemberData() As Integer
            Dim result As Integer = 3600
            Try
                result = Convert.ToInt32(AppSettings("TimeCacheMemberData"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 3600
            End If
            Return result
        End Function
        Public Shared Function PathNewImage() As String
            Dim result As String = "/assets/NewsImage/"
            Try
                result = AppSettings("PathNewImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/NewsImage/"
            End If
            Return result
        End Function
        Public Shared Function PathThumbNewsImage() As String
            Dim result As String = "/assets/NewsImage/MainThumb"
            Try
                result = AppSettings("PathThumbNewsImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/NewsImage/MainThumb"
            End If
            Return result
        End Function
        Public Shared Function PathSmallNewsImage() As String
            Dim result As String = "/assets/newsimage/small/"
            Try
                result = AppSettings("PathSmallNewsImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/NewsImage/small/"
            End If
            Return result
        End Function
        Public Shared Function PathLargeNewsImage() As String
            Dim result As String = "/assets/newsimage/large/"
            Try
                result = AppSettings("PathLargeNewsImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/NewsImage/large/"
            End If
            Return result
        End Function
        Public Shared Function PathBlogImage() As String
            Dim result As String = "/assets/blog/"
            Try
                result = AppSettings("PathBlogImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/Blog/"
            End If
            Return result
        End Function
        Public Shared Function PathThumbBlogImage() As String
            Dim result As String = "/assets/blog/thumb/"
            Try
                result = AppSettings("PathThumbBlogImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/Blog/Thumb/"
            End If
            Return result
        End Function

        Public Shared Function PathNewDocument() As String
            Dim result As String = "/assets/document/"
            Try
                result = AppSettings("PathNewDocument")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/document/"
            End If
            Return result
        End Function

        Public Shared Function SharefaceBookPath() As String
            Dim result As String = "assets/facebookpage/"
            Try
                result = AppSettings("SharefaceBookPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "assets/facebookpage/"
            End If
            Return result
        End Function
        Public Shared Function ShopDesignVideoThumbNoImage() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ShopDesignVideoThumbNoImage")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/shopdesign/video/thumb/noimage.gif"
            End If
            Return result
        End Function
        Public Shared Function ShopDesignVideoThumbPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ShopDesignVideoThumbPath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/shopdesign/video/"
            End If
            Return result
        End Function
        Public Shared Function ShopDesignPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("shopdesignPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/assets/shopdesign/"
            End If
            Return result
        End Function
        Public Shared Function ShopDesignThumbPath() As String
            Dim result As String = ""
            Try
                result = AppSettings("shopdesignThumbPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/assets/shopdesign/thumb/"
            End If
            Return result
        End Function
        Public Shared Function ShopDesignSmallPath() As String
            Dim result As String = ""
            Try
                result = AppSettings("shopdesignSmallPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/assets/shopdesign/small/"
            End If
            Return result
        End Function
        Public Shared Function ShopDesignLargePath() As String
            Dim result As String = ""
            Try
                result = AppSettings("shopdesignLargePath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/assets/shopdesign/large/"
            End If
            Return result
        End Function

        Public Shared Function VideoRelatedThumbPath() As String
            Dim result As String = "/assets/video/relatedthumb/"
            Try
                result = AppSettings("videoRelatedThumbPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/relatedthumb/"
            End If
            Return result
        End Function
        Public Shared Function VideoSmallNoImage() As String
            Dim result As String = "/assets/video/small/noimage_small.gif"
            Try
                result = AppSettings("VideoSmallNoImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/small/noimage_small.gif"
            End If
            Return result
        End Function
        Public Shared Function VideoSmallPath() As String
            Dim result As String = "/assets/video/small/"
            Try
                result = AppSettings("VideoSmallPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/small/"
            End If
            Return result
        End Function
        Public Shared Function VideoPath() As String
            Dim result As String = "/assets/video/"
            Try
                result = AppSettings("videoPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/"
            End If
            Return result
        End Function
        Public Shared Function VideoThumbPath() As String
            Dim result As String = "/assets/video/Thumb/"
            Try
                result = AppSettings("videoThumbPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/thumb/"
            End If
            Return result
        End Function
        Public Shared Function VideoLargePath() As String
            Dim result As String = "/assets/video/large/"
            Try
                result = AppSettings("VideoLargePath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/large/"
            End If
            Return result
        End Function
        Public Shared Function VideoLargeNoImage() As String
            Dim result As String = "/assets/video/large/noimage_large.gif"
            Try
                result = AppSettings("VideoLargeNoImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/large/noimage_large.gif"
            End If
            Return result
        End Function
        Public Shared Function VideoThumbNoImage() As String
            Dim result As String = "/assets/video/thumb/noimage.gif"
            Try
                result = AppSettings("VideoThumbNoimage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/thumb/noimage.gif"
            End If
            Return result
        End Function
        Public Shared Function VideoRelatedThumbNoImage() As String
            Dim result As String = "/assets/video/relatedThumb/noimage.gif"
            Try
                result = AppSettings("VideoRelatedThumbNoimage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/assets/video/relatedthumb/noimage.gif"
            End If
            Return result
        End Function
        Public Shared Function VideoPageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("videoPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 8
            End If
            Return result
        End Function
        Public Shared Function ItemVideoPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("itemVideoPath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "assets/items/video/"
            End If
            Return result
        End Function
        Public Shared Function ItemVideoThumbPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("itemVideoThumbPath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "assets/items/video/thumb/"
            End If
            Return result
        End Function
        Public Shared Function MaxCartCount() As Integer
            Dim result As Integer = 0
            Try
                result = AppSettings("MaxCartCount")
            Catch ex As Exception
            End Try
            If (result < 1) Then
                result = 50
            End If
            Return result
        End Function
        Public Shared Function ItemVideoThumbNoimage() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("itemVideoThumbNoimage")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/items/video/thumb/noimage.jpg"
            End If
            Return result
        End Function
        Public Shared Function ItemVideoNoimage() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ItemVideoNoimage")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/items/video/noimage.jpg"
            End If
            Return result
        End Function



        Public Shared Function AudioPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("AudioPath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "assets/audio/"
            End If
            Return result
        End Function
        Public Shared Function DefaultViewBrand() As Integer
            Dim result As Integer = 5
            Try
                result = Convert.ToInt32(AppSettings("defaultViewBrand"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 5
            End If
            Return result
        End Function
        Public Shared Function BrandItemPageSize() As Integer
            Dim result As Integer = 4
            Try
                result = Convert.ToInt32(AppSettings("BrandItemPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 4
            End If
            Return result
        End Function
        Public Shared Function ReferrenceConfirmOrderUrl() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ReferrenceConfirmOrderUrl")
                If result = "" Then
                    result = "https://www.nss.com/store/confirmation_send.aspx"
                End If
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function LinkFullSite() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LinkFullSite")
                If result = "" Then
                    result = "https://www.nss.com"
                End If
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function CouponItemPageSize() As Integer
            Dim result As Integer = 4
            Try
                result = Convert.ToInt32(AppSettings("CouponItemPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 4
            End If
            Return result
        End Function

        Public Shared Function SampleItemPageSize() As Integer
            Dim result As Integer = 4
            Try
                result = Convert.ToInt32(AppSettings("SampleItemPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 4
            End If
            Return result
        End Function
        Public Shared Function ImageLink() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ImageLink")
                If result = "" Then
                    result = "http://www.m.nss.com/"
                End If
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function LiveWebRootImageFolder() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LiveWebRootImageFolder")
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function MediaPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MediaPath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/media/"
            End If
            Return result
        End Function
        Public Shared Function MediaThumbPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MediaThumbPath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/media/Thumb"
            End If
            Return result
        End Function
        Public Shared Function MediaThumbNoimage() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MediaThumbNoimage")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "/assets/media/thumb/noimage.gif"
            End If
            Return result
        End Function
        Public Shared Function MediaPageSize() As Integer
            Dim result As Integer = 8
            Try
                result = Convert.ToInt32(AppSettings("MediaPageSize"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 8
            End If
            Return result
        End Function

        Public Shared Function FaceBookAppId() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("FaceBookAppId")
            Catch ex As Exception
            End Try
            If result = "" Then
                result = "139203732825969"
            End If
            Return result
        End Function
        Public Shared Function FaceBookPageId() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("FaceBookPageId")
            Catch ex As Exception
            End Try
            If result = "" Then
                result = "685318908202171"
            End If
            Return result
        End Function

        Public Shared Function FaceBookSecretId() As String
            Dim result As String = ""
            Try
                result = AppSettings("FaceBookSecretId")
            Catch ex As Exception
            End Try
            Return result
        End Function


        Public Shared Function FaceBookPageAccessToken() As String
            Dim result As String = ""
            Try
                result = AppSettings("FaceBookPageAccessToken")
            Catch ex As Exception
            End Try
            Return result
        End Function


        Public Shared Function RootDepartmentCode() As String
            Return "store-home"
        End Function
        Public Shared Function RootDepartmentID() As Integer
            Return 23
        End Function

        Public Shared Function TimeCacheDataItem() As Integer
            Dim result As Integer = 0
            Try
                result = Convert.ToInt32(AppSettings("timeCacheDataItem"))
            Catch ex As Exception
            End Try
            If result < 1 Then
                result = 86400
            End If
            Return result
        End Function
        Public Shared Function EbayLinkItemDetail() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("EbayLinkItemDetail")
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function AmazonLinkItemDetail() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("AmazonLinkItemDetail")
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function StepMoney() As Integer
            Dim result As Integer = 5
            Try
                result = Convert.ToInt32(AppSettings("StepMoney"))
            Catch ex As Exception

            End Try
            If (result < 1) Then
                result = 5
            End If
            Return result
        End Function

        Public Shared Function LimitItemsPaypal() As Integer
            Dim result As Integer = 90
            Try
                result = Convert.ToInt32(AppSettings("LimitItemsPaypal"))
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function AlbumNoImagePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("AlbumNoImagePath")
            Catch ex As Exception
            End Try
            If result = String.Empty Then
                result = "assets/Album/Image/noimage.jpg"
            End If
            Return result
        End Function
        Public Shared Function AlbumThumbPath() As String
            Dim result As String = "assets/Album/image/"
            Try
                result = AppSettings("AlbumPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "assets/Album/image/"
            End If
            Return result
        End Function
        Public Shared Function AlbumCopyPath() As String
            Dim result As String = "assets/Album/"
            Try
                result = AppSettings("AlbumCopyPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "assets/Album/"
            End If
            Return result
        End Function
        Public Shared Function SongPath() As String
            Dim result As String = "assets/Album/Song/"
            Try
                result = AppSettings("SongPath")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "assets/Album/Song/"
            End If
            Return result
        End Function
        Public Shared Function urlProduct() As String
            Dim result As String = "/nail-products/"
            Try
                result = AppSettings("urlProduct")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "/nail-products/"
            End If
            Return result
        End Function
        Public Shared Function SkypeDefault() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("Skype")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "nailsuperstore"
            End If
            Return result
        End Function

        Public Shared Function SendByGmail() As Boolean
            Try
                If (AppSettings("SendByGmail").ToString() = "1") Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function BCCEmailConfirmPayment() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("BCCEmailConfirmPayment")
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function MaxBoostValue() As Single
            Dim result As String = String.Empty
            Try
                result = AppSettings("MaxBoostValue")
                Return CSng(result)
            Catch ex As Exception

            End Try
            Return CSng("4.2949673E+9")
        End Function

        Public Shared Function SolrServerURL() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("SolrServerURL")
            Catch ex As Exception
                result = String.Empty
            End Try
            Return result
        End Function
        Public Shared Function UseSolr() As Boolean
            Dim result As Boolean = False
            Try
                Boolean.TryParse(AppSettings("UseSolr"), result)
                Return result
            Catch ex As Exception
                result = False
            End Try
            Return result
        End Function

        Public Shared Function LuceneFuzzyFactor() As Single
            Dim result As String = String.Empty
            Try
                result = CSng(AppSettings("LuceneFuzzyFactor"))
            Catch ex As Exception
                result = 0.7
            End Try
            Return result
        End Function
        Public Shared Function DefaultLuceneBoost() As Single
            Dim result As Single = 0
            Try
                result = CSng(AppSettings("DefaultLuceneBoost"))
            Catch ex As Exception

            End Try
            If (result <= 0) Then
                result = 0.0001
            End If
            Return result
        End Function
        Public Shared Function LuceneBoostFactor() As Single
            Dim result As Single = 0
            Try
                result = CSng(AppSettings("LuceneBoostFactor"))
            Catch ex As Exception
                result = 1.5
            End Try
            Return result
        End Function
        Public Shared Function LuceneKeywordIndexPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneKeywordIndexPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\lucene_index\Keyword"
            End If
            Return result
        End Function
        Public Shared Function LuceneItemIndexPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneItemIndexPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\lucene_index\Item"
            End If
            Return result
        End Function
        Public Shared Function LuceneArticleIndexPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneArticleIndexPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\lucene_index\Article"
            End If
            Return result
        End Function
        Public Shared Function LuceneSpellCheckIndexPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneSpellCheckIndexPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\lucene_index\Spell"
            End If
            Return result
        End Function

        Public Shared Function LuceneAdminLogIndexPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneAdminLogIndexPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\lucene_index\AdminLog"
            End If
            Return result
        End Function
        Public Shared Function LuceneVideoIndexPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneVideoIndexPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\WebSite\NailSuperStore\www\Public_web\lucene_index\Video"
            End If
            Return result
        End Function

        Public Shared Function NumberOfDisplayKeyword() As Integer
            Dim result As Integer = 5
            Try
                result = Convert.ToInt32(AppSettings("NumberOfDisplayKeyword"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 5
            End If
            Return result
        End Function
        Public Shared Function ActiveSearchKeyword() As Boolean
            Dim result As Boolean = False
            Try
                result = CBool(AppSettings("ActiveSearchKeyword"))
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function NumberOfDisplayItem() As Integer
            Dim result As Integer = 10
            Try
                result = Convert.ToInt32(AppSettings("NumberOfDisplayItem"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 10
            End If
            Return result
        End Function
        Public Shared Function GetBrandSearchPriority() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("GetBrandSearchPriority")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = ""
            End If
            Return result
        End Function

        Public Shared Function PathAdminMenu() As String
            Dim result As String = "Menu.xml"
            Try
                result = AppSettings("PathAdminMenu")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "Menu.xml"
            End If
            Return result
        End Function
        Public Shared Function StarType(ByVal star As Integer) As String
            Dim result As String = ""
            Select Case star
                Case 1
                    result = "Awful"
                Case 2
                    result = "Poor"
                Case 3
                    result = "Neutral"
                Case 4
                    result = "Good"
                Case 5
                    result = "Excellent"
            End Select
            Return result
        End Function
        Public Shared Function OrderReviewUrl() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("OrderReviewUrl")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/store/review/order-write.aspx"
            End If
            Return result
        End Function
        Public Shared Function ProductReviewUrl() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ProductReviewUrl")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/store/review/product-write.aspx"
            End If
            Return result
        End Function
        Public Shared Function ReviewDay() As Integer
            Dim result As Integer = 0
            Try
                result = AppSettings("ReviewDay")
            Catch ex As Exception
                result = 0
            End Try
            Return result
        End Function
        Public Shared Function CheckUrlAccess() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("CheckUrlAccess")
            Catch ex As Exception
            End Try
            If (result = Nothing) Then
                result = "/admin/unauthorized.aspx;"
            End If
            Return result
        End Function
        Public Shared Function NotNavigation() As String
            Dim result As String = ""
            Try
                result = AppSettings("NotNavigation")
            Catch ex As Exception
                result = "0"
            End Try
            Return result
        End Function
        Public Shared Function CheckPaypalCompleted() As Boolean
            Dim result As Integer = 1
            Try
                result = Convert.ToInt32(AppSettings("CheckPaypalCompleted"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function ShowNarrowSearchSubCate() As Boolean
            Dim result As Integer = 1
            Try
                result = Convert.ToInt32(AppSettings("ShowNarrowSearchSubCate"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function ShowBrandSubCate() As Boolean
            Dim result As Integer = 0
            Try
                result = Convert.ToInt32(AppSettings("ShowBrandSubCate"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function PageSizeTipSearch() As Integer
            Dim result As Integer = 12
            Try
                result = AppSettings("PageSizeTipSearch")
            Catch ex As Exception
            End Try
            If result < 1 Then
                result = 12
            End If
            Return result
        End Function
        Public Shared Function LinkRefer() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LinkRefer")
            Catch ex As Exception
            End Try
            If String.IsNullOrEmpty(result) Then
                result = "https://www.nss.com/referrals/{0}"
            End If
            Return result
        End Function
        Public Shared Function TemplateEmailRefer() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("TemplateEmailRefer")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\Website\NailSuperstore\www\includes\MailTemplate\EmailRefer.htm"
            End If
            Return result
        End Function

        Public Shared Function LuceneAppPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("LuceneAppPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\Website\NailSuperstore\Tools\ExportLuceneItem\ExportLuceneItem.exe"
            End If
            Return result
        End Function
        Public Shared Function LuceneAppWaitMinute() As Integer
            Dim result As Integer = 5
            Try
                result = AppSettings("LuceneAppWaitMinute")
            Catch ex As Exception
            End Try
            If result < 1 Then
                result = 5
            End If
            Return result
        End Function

        Public Shared Function ArchivePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ArchivePath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\Website\NailSuperstore\ftp\export\Archive\"
            End If
            Return result
        End Function
        Public Shared Function FolderCopyArchivePath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("FolderCopyArchivePath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "\admin\tmp\"
            End If
            Return result
        End Function
        Public Shared Function MediaUrl() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("MediaUrl")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/assets"
            End If
            Return result
        End Function

        Public Shared Function PathUploadArtTrend() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathUploadArtTrend")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/upload/nail-art-trends/"
            End If
            Return result
        End Function
        Public Shared Function PathArtTrends() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathArtTrends")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "\upload\nail-art-trends\"
            End If
            Return result
        End Function
        Public Shared Function NewsFeedImg() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("NewsFeedImg")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "\assets\NewsImage\newsfeed\"
            End If
            Return result
        End Function
        Public Shared Function MobilePageExistMixmatch() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageExistMixmatch")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "bonus-offers"
            End If
            Return result
        End Function
        Public Shared Function SendReminderAppPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("SendReminderAppPath")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "D:\Website\NailSuperstore\Tools\SendAvailabilityReminders\SendAvailabilityReminders.exe"
            End If
            Return result
        End Function
        Public Shared Function ProductPages() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ProductPages")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = ",/store/search-result.aspx,/store/default.aspx,/store/category.aspx,/store/shop-save.aspx,/store/main.aspx,/store/item.aspx,/store/products-collection.aspx,/store/product-collection-detail.aspx,/store/reward-point.aspx"
            End If
            Return result
        End Function
        Public Shared Function PageNotScroll() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageNotScroll")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = ",/store/product-collection-detail.aspx"
            End If
            Return result
        End Function
        Public Shared Function ProductTemplateFile() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("ProductTemplateFile")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "/includes/template/"
            End If
            Return result
        End Function
        Public Shared Function NoloadDept() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("NoloadDept")
            Catch ex As Exception

            End Try
            If result = String.Empty Then
                result = "250,"
            End If
            Return result
        End Function
        Public Shared Function MaxFreeGiftLevel() As Double
            Dim result As Double
            Try
                If Not String.IsNullOrEmpty(AppSettings("MaxFreeGiftLevel")) Then
                    result = CDbl(AppSettings("MaxFreeGiftLevel"))
                End If
            Catch ex As Exception
                result = 10000
            End Try
            Return result
        End Function
        Public Shared Function PathMainInforBanner() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathMainInforBanner")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/inforbanner/"
            End If
            Return result
        End Function
        Public Shared Function PathSubBlockBanner() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathSubBlockBanner")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/sub-block-banner/"
            End If
            Return result
        End Function
        Public Shared Function PathFreeGiftLevelBanner() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PathFreeGiftLevelBanner")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "/assets/FreeGiftLevel/"
            End If
            Return result
        End Function
        Public Shared Function DefaultTopSearchTip() As Integer
            Dim result As String = String.Empty
            Try
                result = CInt(AppSettings("DefaultTopSearchTip"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 20
            End If
            Return result
        End Function
        Public Shared Function DefaultTopSearchNewsEvent() As Integer
            Dim result As String = String.Empty
            Try
                result = CInt(AppSettings("DefaultTopSearchNewsEvent"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 13
            End If
            Return result
        End Function
        Public Shared Function DefaultTopSearchMediaPress() As Integer
            Dim result As String = String.Empty
            Try
                result = CInt(AppSettings("DefaultTopSearchMediaPress"))
            Catch ex As Exception

            End Try
            If result < 1 Then
                result = 18
            End If
            Return result
        End Function
        Public Shared Function TextToFind() As String
            Dim result As String = String.Empty
            Try
                result = CInt(AppSettings("TextToFind"))
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐėÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴçÇÑñäØøöüşćåš¨бžÖæıßČΠΣų°ěřżłŠūėșğ℅Å👽'î™ūāÃÄöÖ"
            End If
            Return result
        End Function
        Public Shared Function TextToReplace() As String
            Dim result As String = String.Empty
            Try
                result = CInt(AppSettings("TextToReplace"))
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADeEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYcCNnaOoouscas ozOaiBCNWuoerzlSuesg%A   i uaAAoO"
            End If
            Return result
        End Function
        Public Shared Function IPInfo() As String
            Dim result As String = String.Empty
            Try
                result = CInt(AppSettings("IPInfo"))
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = "153267c35b2f2a5ceddaac785eddb827f6b03daa05e24cfbc2a740912238be49"
            End If
            Return result
        End Function
        Public Shared Function PageFixMenu() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageFixMenu")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = ",home.aspx,/store/main-category.aspx,/store/sub-category.aspx,"
            End If
            Return result
        End Function
        Public Shared Function PageNotPopup() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageNotPopup")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = ",/store/cart.aspx,/store/free-gift.aspx,/store/free-sample.aspx,/store/reward-point.aspx,"
            End If
            Return result
        End Function
        Public Shared Function PageNotWebResource() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PageNotWebResource")
            Catch ex As Exception

            End Try
            If String.IsNullOrEmpty(result) Then
                result = ",/,home.aspx,/store/main-category.aspx,/store/item.aspx,/store/shop-save.aspx,/store/scollection.aspx,/store/product-collection-detail.aspx,/store/main-category.aspx,/store/main-shop-save.aspx"
            End If
            Return result
        End Function
        Public Shared Function SendmailCartUrl() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("SendMailCartUrl")
                If result = "" Then
                    result = "http://nss.com/store/send-cart.aspx"
                End If
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function CDNMediaPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("CDNMediaPath")

            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function CDNCssScriptPath() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("CDNCssScriptPath")

            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function BrowserLocation() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("BrowserLocation")
            Catch ex As Exception
                result = String.Empty
            End Try
            Return result
        End Function
        Public Shared Function SubjectReferAddPoint() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("SubjectReferAddPoint")
            Catch ex As Exception
                result = String.Empty
            End Try
            If String.IsNullOrEmpty(result) Then
                result = "Thanks - Nail Superstore referral complete!"
            End If
            Return result
        End Function
        Public Shared Function PointFirstReview() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PointFirstReview")
            Catch ex As Exception
                result = 20
            End Try
            If String.IsNullOrEmpty(result) Then
                result = 20
            End If
            Return result
        End Function
        Public Shared Function PointOldReview() As String
            Dim result As String = String.Empty
            Try
                result = AppSettings("PointOldReview")
            Catch ex As Exception
                result = 10
            End Try
            If String.IsNullOrEmpty(result) Then
                result = 10
            End If
            Return result
        End Function
#Region "Mail"
        Public Shared Function MailServer() As String
            Dim result As String = "NSSemail.vn"
            Try
                result = AppSettings("MailServer")
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function MailSendUsing() As Integer
            Dim result As Integer = 2
            Try
                result = Integer.Parse(AppSettings("MailSendUsing"))
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function MailServerPort() As Integer
            Dim result As Integer = 25
            Try
                result = Convert.ToInt32(AppSettings("MailServerPort"))
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function MailSSL() As Boolean
            Dim result As Boolean = False
            Try
                result = Convert.ToBoolean(AppSettings("MailSSL"))
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function MailAuthenticate() As Integer
            Dim result As Integer = 1
            Try
                result = Integer.Parse(AppSettings("MailAuthenticate"))
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailUsername() As String
            Dim result As String = "no-reply@nssemail.vn"
            Try
                result = AppSettings("MailUsername")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailPassword() As String
            Dim result As String = "654123@nails"
            Try
                result = AppSettings("MailPassword")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailErrorServer() As String
            Dim result As String = "smtp.gmail.com"
            Try
                result = AppSettings("ErrorMailServer")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailErrorPort() As Integer
            Dim result As Integer = 465
            Try
                result = Convert.ToInt32(AppSettings("ErrorMailServerPort"))
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailErrorUsername() As String
            Dim result As String = "noreply.@nss.com"
            Try
                result = AppSettings("ErrorMailUsername")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailErrorPassword() As String
            Dim result As String = "F@ck3804"
            Try
                result = AppSettings("ErrorMailPassword")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailErrorSSL() As Boolean
            Dim result As String = "0"
            Try
                result = AppSettings("ErrorMailSMTPSSL")
            Catch ex As Exception

            End Try
            If result = "1" Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function MailErrorCC() As String
            Dim result As String = ""
            Try
                result = AppSettings("ErrorMailCC")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function MailErrorTo() As String
            Dim result As String = "k@nss.com"
            Try
                result = AppSettings("ErrorMailTo")
            Catch ex As Exception

            End Try
            Return result
        End Function

        Public Shared Function AllowSendMailLog() As Boolean
            Dim result As Integer = 0
            Try
                result = Integer.Parse(AppSettings("AllowSendMailLog"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function MemberAllowSendMailLog() As Boolean
            Dim result As Integer = 0
            Try
                result = Integer.Parse(AppSettings("MemberAllowSendMailLog"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function PathSweepstakeImage() As String
            Dim result As String = "assets/Sweepstake/"
            Try
                result = AppSettings("PathSweepstakeImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "assets/Sweepstake/"
            End If
            Return result
        End Function
        Public Shared Function PathSweepstakeDetailImage() As String
            Dim result As String = "assets/Sweepstake/Detail/"
            Try
                result = AppSettings("PathSweepstakeDetailImage")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "assets/Sweepstake/Detail/"
            End If
            Return result
        End Function
        Public Shared Function PhoneOutUSPattern() As String
            Dim result As String = ""
            Try
                result = AppSettings("PhoneOutUSPattern")
            Catch ex As Exception

            End Try
            If (result = String.Empty) Then
                result = "^([0-9\(\)\/\+ \-]*)$"
            End If
            Return result
        End Function
#End Region

#Region "Twitter Token"
        Public Shared Function TwitterConsumerKey() As String
            Dim result As String = "MZbj6b2sioIMuR30a5hSA"
            Try
                result = AppSettings("TwitterConsumerKey")
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function TwitterConsumerSecret() As String
            Dim result As String = "zcOzk6glkkMKhMahS9rMzZ62zk5xpCWPzw9TrMiohw"
            Try
                result = AppSettings("TwitterConsumerSecret")
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function TwitterUserID() As Integer
            Dim result As Integer = 17897799
            Try
                result = Int32.Parse(AppSettings("TwitterUserID"))
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function TwitterScreenName() As String
            Dim result As String = "nailsuperstore"
            Try
                result = AppSettings("TwitterScreenName")
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function TwitterToken() As String
            Dim result As String = "17897799-L3fKprKd9VEY38XSkqR8yscZ9MurdPYno1Wzt3sx4"
            Try
                result = AppSettings("TwitterToken")
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function TwitterTokenSecret() As String
            Dim result As String = "4pHERtDVZFBrQq9abHCZek9tn7bdPhc5u6AfgOjU"
            Try
                result = AppSettings("TwitterTokenSecret")
            Catch ex As Exception

            End Try
            Return result
        End Function
#End Region
    End Class
End Namespace
