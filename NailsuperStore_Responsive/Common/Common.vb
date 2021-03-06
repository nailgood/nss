Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports DataLayer
Imports Sgml
Imports System.Xml
Imports System.IO
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data
Imports System.Data.Common

Namespace Utility
    Public Class Address
        Public FirstName As String
        Public LastName As String
        Public AddressLn1 As String
        Public AddressLn2 As String '
        Public City As String
        Public State As String
        Public Zip As String
    End Class

    Public Class CreditCardInfo
        Public FirstName As String
        Public LastName As String
        Public CreditCardType As String
        Public CID As String
        Public CreditCardNumber As String
        Public ExpMonth As Integer
        Public ExpYear As Integer
    End Class


    Public Class Common
        Public Shared cachePrefixKey As String = "Common_"
        Public Enum AnchorPosition
            Top
            Center
            Bottom
            Left
            Right
        End Enum

        Public Enum ContentToolRegion
            CT_Left
            CT_Right
        End Enum
        Public Enum MixmatchType As Integer
            Normal = 1
            ProductCoupon = 2
        End Enum
        Public Enum ItemAcceptingStatus As Integer
            <Description("None")> None = 0
            <Description("Accepting Order")> AcceptingOrder = 1
            <Description("In stock")> InStock = 2
        End Enum
        Public Enum ShipmentValue As Integer
            Residential
            Insurance
            Signature
            DASResidential
            DASCommercial
            FuelRate
            HazMatFee
        End Enum
        Public Enum PaymentStatus As Integer
            Initial = 0
            PaypalPending = 1
            PaypalCompleted = 2
            eCheckPending = 3
            eCheckCompleted = 4
            eCheckCancelled = 5
            PaypalCancelled = 6
        End Enum
        Public Enum MixmatchLineType As Integer
            NoMixmatch = 0
            BuyDiscounted = 1
            GetDiscounted = 2
            BuyFreeItem = 3
            GetFreeItem = 4
        End Enum
        Public Enum MixmatchDefaulType As Integer
            NoneDefault = 0
            OneDefault = 1
            AllDefault = 2
        End Enum
        Public Enum AdminLogAction
            Update
            Insert
            Delete
            DoActive
        End Enum
        Public Enum ShopSaveType As Integer
            ShopNow = 1
            SaveNow = 2
            DealOfDay = 3
            ShopYourWay = 4
            TopCategory = 5
            WeeklyEmail = 6
        End Enum
        Public Enum SalesPriceType As Integer
            Item = 2
            [Case] = 3
        End Enum
        Public Enum ObjectType
            <Description("Item")> StoreItem
            <Description("System Parameters")> Sysparam
            <Description("Mix Match")> MixMatch
            <Description("Mix Match Line")> MixMatchLine
            <Description("Order Coupon")> OrderCoupon
            <Description("Product Coupon")> ProductCoupon
            <Description("Free Gift")> FreeGift
            <Description("Free Sample")> FreeSample
            <Description("Sales Price")> SalesPrice
            <Description("Member")> Member
            <Description("Cash Point")> CashPoint
            <Description("Landing Page")> LandingPage
            <Description("Product Review")> ProductReview
            <Description("Order Review")> OrderReview
            <Description("Video Review")> VideoReview
            <Description("News Review")> NewsReview
            <Description("Shop Design Review")> ShopDesignReview
            <Description("News & Events")> News
            <Description("Blog")> Blog
            <Description("Video")> Video
            <Description("Flash Banner")> FlashBanner
            <Description("Strip Banner")> StripBanner
            <Description("Category")> Category
            <Description("NailArtTrends")> NailArtTrends
            <Description("Tracking Number")> TrackingNumber
            <Description("Department")> Department
            <Description("Item Point")> ItemPoint
            <Description("Shop By Design")> ShopDesign
            <Description("Shop Design Media")> ShopDesignMedia
            <Description("Shop Design Item")> ShopDesignItem
        End Enum
        Public Enum Price
            price1 = 0
            Price2 = 50
            Price3 = 100
            Price4 = 200
            Price5 = 400

        End Enum
        Public Enum FlammableCart As Integer
            Init = -1
            None = 0
            HazMat = 1
            BlockedHazMat = 2
        End Enum

        Public Enum InforBannerType
            Main = 1
            SubBlockBanner = 2
            AboutUsHome = 3
        End Enum
        Structure DepartmentType
            Const nailsupplies = "nail-supplies"
            Const nailsupply = "nail-supply"
            Const nailcollection = "nail-collection"
        End Structure
        Public Shared Function IsiPad() As Boolean
            Try
                If (System.Web.HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad")) Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Sub GenerateMatserPageClass(ByRef form As System.Web.UI.HtmlControls.HtmlForm)

            If (System.Web.HttpContext.Current.Request.FilePath.Contains("/members/login.aspx")) Then
                Exit Sub
            End If

            Dim CssClass As String = System.Web.HttpContext.Current.Request.FilePath
            Try
                CssClass = CssClass.Substring(CssClass.LastIndexOf("/") + 1)
                CssClass = CssClass.Replace(".aspx", "")
            Catch ex As Exception

            End Try
            form.Attributes("class") = CssClass
        End Sub


        Public Shared Function IsViewFromAdmin() As Boolean
            Dim isFromAdmin As String = System.Web.HttpContext.Current.Request.QueryString("admin")
            If isFromAdmin = "1" Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function CheckShippingInternational(ByVal objDB As Database, ByVal objOrder As StoreOrderRow) As Boolean
            If (objOrder.IsSameAddress = False AndAlso objOrder.ShipToCountry = Nothing) OrElse (objOrder.IsSameAddress = True AndAlso objOrder.BillToCountry = Nothing) Then
                Return False
            End If

            Dim CountryCode As String = ""
            If Utility.Common.CheckShippingSpecialUS(objOrder) = True Then
                CountryCode = IIf(objOrder.IsSameAddress, objOrder.BillToCounty, objOrder.ShipToCounty)
            Else
                CountryCode = IIf(objOrder.IsSameAddress, objOrder.BillToCountry, objOrder.ShipToCountry)
            End If

            Dim sc As String = ShippingRangeRow.GetShippingCode(objDB, CountryCode)
            Return Not String.IsNullOrEmpty(sc)
        End Function
        Public Shared Function GetProductReviewComment(ByVal comment As String) As String
            If String.IsNullOrEmpty(comment) Then
                Return String.Empty
            End If
            Dim arrData, arrData1 As String()
            Dim result As String = String.Empty
            arrData = comment.Trim.Split(vbCrLf)
            If (arrData.Length > 0) Then
                For i As Integer = 0 To arrData.Length - 1
                    If (arrData(i).ToString().Contains("#txtComments#")) Then
                        arrData1 = arrData(i).Split("=")
                        If arrData1.Length > 1 Then
                            result = arrData1(1).ToString().Replace("\n", "<br />")
                            Return result
                        End If
                    End If
                Next
            End If
            Return comment
        End Function
        ''' <summary>
        ''' check xem address shipping cua order hien tai co ship toi 1 dia chi quoc te hay ko( ko tinh tieu bang dac biet HI,AK)
        ''' </summary>
        ''' <param name="objDB"></param>
        ''' <param name="objOrder"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckOrderShippingIsCompleteIntAddress(ByVal objDB As Database, ByVal objOrder As StoreOrderRow) As Boolean
            If (objOrder.ShippingAddressId > 0) Then
                Dim objShipping As MemberAddressRow = MemberAddressRow.GetRow(objDB, objOrder.ShippingAddressId)
                If Not objShipping Is Nothing Then
                    Return Utility.Common.IsCompleteIntAddress(objShipping)
                End If
            End If
            Return False
        End Function
        Public Shared Sub Redirect301(ByVal sURL As String)
            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently"
            System.Web.HttpContext.Current.Response.AddHeader("Location", sURL)
            System.Web.HttpContext.Current.Response.End()
        End Sub
        Public Shared Sub CheckValidURLCode(ByVal dbURLCode As String)

            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.RawUrl
                End If
            End If
            If String.IsNullOrEmpty(rawURL) Then
                Exit Sub
            End If
            If rawURL.Contains(".aspx") Then '
                Exit Sub
            End If
            Dim queryURLCode As String = String.Empty
            Dim arr() As String = rawURL.Split("/")
            If arr.Length > 0 Then
                If arr(1).Contains("blog") And arr.Length = 2 Then
                    queryURLCode = arr(1)
                ElseIf arr(1).Contains("news") Then
                    queryURLCode = arr(2)
                ElseIf System.Web.HttpContext.Current.Request.Path = "/store/department-tab.aspx" Then
                    queryURLCode = arr(arr.Length - 3)
                Else
                    queryURLCode = arr(arr.Length - 2)
                End If

                If (Not String.IsNullOrEmpty(queryURLCode) AndAlso queryURLCode <> dbURLCode) Then
                    rawURL = rawURL.Replace("/" & queryURLCode & "/", "/" & dbURLCode & "/")
                    Redirect301(rawURL)
                End If
            End If
        End Sub
        Public Shared Function ColumnExists(ByVal reader As System.Data.SqlClient.SqlDataReader, ByVal columnName As String) As Boolean
            ''Return reader.GetSchemaTable().Columns.Contains(columnName)
            For i As Integer = 0 To reader.FieldCount - 1
                If reader.GetName(i).Equals(columnName) Then
                    Return Not IsDBNull(reader(columnName))
                End If
            Next

            Return False
        End Function

        Public Shared Function EnumDescription(ByVal EnumConstant As [Enum]) As String
            Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            If aattr.Length > 0 Then
                Return aattr(0).Description
            Else
                Return EnumConstant.ToString()
            End If
        End Function
        Public Shared Function IsPromotionFreeItem(ByVal promotionType As String) As Boolean
            If promotionType = "Freeitem" Then
                Return True
            End If
        End Function
        Public Shared Function IsItemAcceptingOrder(ByVal type As Integer) As Boolean
            If type = ItemAcceptingStatus.AcceptingOrder Or type = ItemAcceptingStatus.InStock Then
                Return True
            End If
            Return False
        End Function
        Public Enum ReferStatus
            <Description("Invitation sent")> InvitationSent = 1
            <Description("Registered")> Registered = 2
            <Description("Activated Account")> ActivatedAccount = 3
            <Description("Ordered")> Ordered = 4
            <Description("Order shipped - Get {0} points")> OrderShipped = 5
        End Enum
        Public Shared Function ConvertReferFriendStatusToString(ByVal status As Integer) As String
            If (status = 1) Then
                Return "Invitation sent"
            End If
            If (status = 2) Then
                Return "Registered"
            End If
            If (status = 3) Then
                Return "Activated Account"
            End If
            If (status = 4) Then
                Return "Ordered"
            End If
            If (status = 5) Then
                Return "{0} pts &nbsp;&nbsp;<img src='/assets/uploadimage/tick.jpg' />"
            End If
            Return String.Empty
        End Function
        Public Enum ReferFriendStatus
            SentInvite = 1
            Register = 2
            ActiveAccount = 3
            Order = 4
            AddPoint = 5

        End Enum
        Public Enum ReferType
            ReferFriends = 1
            ReferProduct = 2
        End Enum
        Public Enum ReferSource
            Email = 1
            FaceBook = 2
            Twitter = 3
        End Enum
        Public Enum Rating
            Rating1 = 0
            Rating2 = 3
            Rating3 = 4
        End Enum

        Public Enum SortBy
            Price = 1
            Hot_items = 2
            On_Sale = 3
            Best_Sellers = 4
            New_Items = 5
            Top_Rated = 6
            Most_Rated = 7
        End Enum

        Public Enum ArticleType
            News = 1
            Tips = 2
            MediaPress = 3
        End Enum
        Public Enum MemberAddressType
            Shipping
            Billing
            AddressBook
        End Enum
        Public Enum SystemFunction
            ChangeOrderAddress

        End Enum
        Public Enum StandardShippingMethod
            UPS = 1
            FedEx = 2
            USPS = 3
            Truck = 4
            DHL = 5
        End Enum
        Public Shared CartItemTypeBuyPoint As String = "BuyPoint"

        Public Enum ImageDimensions
            Width
            Height
        End Enum
        Public Enum ImageAnchorPosition
            Top
            Center
            Bottom
            Left
            Right
        End Enum
        Public Enum ItemAction
            Detail = 1
            AddCart = 2
        End Enum
        Public Enum AddCartType
            WebLive = 1
            Mobile = 2
        End Enum
        Public Enum ClassifiedAwards
            Yes = 2
            No = 1
        End Enum
        Public Enum Days
            SUN = 0
            MON = 1
            TUE = 2
            WED = 3
            THU = 4
            FRI = 5
            SAT = 6
        End Enum
        Public Enum TypeReview
            ProductReview = 1
            OrderReview = 2
        End Enum
        Public Enum TypeReply
            CustomerReply = 1
            AdminReply = 2
        End Enum
        Public Enum ReviewType
            Video = 1
            News = 2
            ShopDesign = 3
        End Enum
        Public Enum CategoryType
            News = 1
            Video = 2
            MediaPress = 3
            ShopDesign = 4
        End Enum
        Public Enum ShopDesignMediaType
            Image = 0
            Video = 1
        End Enum
        ' tuong ung ten file template
        Public Enum TemplateReview
            template_loadreview
            template_loadreview_admin
            template_review
        End Enum
        Public Shared Function LoadTemplateReview(ByVal pathTemplate As String, ByVal TemplateFile As String) As String 'ByVal templatefile As [Enum]
            Dim strtemplate As String
            Dim key As String = String.Format(cachePrefixKey & TemplateFile)
            strtemplate = CType(CacheUtils.GetCache(key), String)
            If Not strtemplate Is Nothing Then
                Return strtemplate
            End If
            pathTemplate = pathTemplate & TemplateFile & ".txt"
            Dim objreader As FileStream
            Try
                If strtemplate Is Nothing Then
                    objreader = New FileStream(pathTemplate, FileMode.Open)
                    Dim sr As New StreamReader(objreader)
                    strtemplate = sr.ReadToEnd
                    sr.Dispose()
                    sr.Close()
                End If
            Catch ex As Exception

            Finally
                If Not objreader Is Nothing Then
                    objreader.Close()
                    objreader.Dispose()
                End If

            End Try
            CacheUtils.SetCache(key, strtemplate, Utility.ConfigData.TimeCacheData)
            Return strtemplate
        End Function

        Public Shared Function GetMaximunQtyAddPoint(ByVal PointAvailable As Integer, ByVal ItemRewardPoint As Integer) As Integer
            Dim modValue As Integer = PointAvailable Mod ItemRewardPoint
            Dim MaxNumberAdd As Integer = (PointAvailable - modValue) / ItemRewardPoint
            Return MaxNumberAdd
        End Function
        Public Shared Function GetSingleZipcode(ByVal zip As String) As String
            Try
                If (String.IsNullOrEmpty(zip)) Then
                    Return zip
                End If
                If Not (zip.Contains("-")) Then
                    Return zip
                End If
                Dim index As Integer = zip.IndexOf("-")
                Dim result As String = zip.Substring(0, index)
                Return result
            Catch ex As Exception

            End Try
            Return zip
        End Function
        Public Shared Function GetErrorMaximunAddPointMessage(ByVal PointAvailable As Integer, ByVal ItemRewardPoint As Integer, ByVal itemSKU As String, ByVal addQty As Integer) As String
            Dim max As Integer = GetMaximunQtyAddPoint(PointAvailable, ItemRewardPoint)
            Dim result As String = String.Empty
            If (max > 0) Then
                result = "Your current point balance is not sufficient to purchase " & addQty & " item " & itemSKU & ". You can only purchase " & max & "."
            Else
                result = "Your current point balance is not sufficient to purchase item " & itemSKU & "."
            End If
            Return result
        End Function
        Public Shared Function ConvertHTML5Video(ByVal url As String, ByVal subtitle As String, Optional ByVal videoURLImg As String = "", Optional ByVal id As String = "") As String
            If Not (url.Contains(".")) Then
                Return String.Empty
            End If
            Try
                Dim html5Video As String = String.Empty
                Dim indexExt As Integer = url.IndexOf(".")
                url = url.Substring(0, indexExt)
                html5Video = "<video " & IIf(String.IsNullOrEmpty(id), "", " id=" & id) & " autoplay='autoplay' controls='controls' " & IIf(Not String.IsNullOrWhiteSpace(videoURLImg), " poster='" & videoURLImg & " ", "") & ">"
                html5Video &= "   <source type='video/mp4'  src='" & Utility.ConfigData.CDNMediaPath & url & ".mp4'></source>"
                html5Video &= "   <source type='video/webm'  src='" & Utility.ConfigData.CDNMediaPath & url & ".webm'></source>"
                html5Video &= "   <source type='video/ogg'  src='" & Utility.ConfigData.CDNMediaPath & url & ".ogv'></source>"
                html5Video &= IIf(Not String.IsNullOrEmpty(subtitle), " <track label='English' kind='captions' srclang='en' src='" & subtitle & "' default>", "<track label='English' kind='captions' srclang='en' src='" & Utility.ConfigData.CDNMediaPath & url & ".vtt' default>")
                html5Video &= " Video not playing? <a  href='" & Utility.ConfigData.CDNMediaPath & url & ".mp4'>Download file</a> instead.</video>"
                Return html5Video
            Catch ex As Exception
            End Try
            Return String.Empty
        End Function
        Public Shared Function ConvertHTML5Video(ByVal url As String, ByVal w As Integer, ByVal h As Integer) As String
            If Not (url.Contains(".")) Then
                Return String.Empty
            End If
            Try
                Dim html5Video As String = String.Empty
                Dim indexExt As Integer = url.IndexOf(".")
                url = url.Substring(0, indexExt)
                html5Video = "<video autoplay='autoplay' width='" & w & "' height='" & h & "' controls='controls'>"
                html5Video &= "   <source type='video/mp4'  src='" & url & ".mp4'></source>"
                html5Video &= "   <source type='video/webm'  src='" & url & ".webm'></source>"
                html5Video &= "   <source type='video/ogg'  src='" & url & ".ogv'></source>"
                html5Video &= " Video not playing? <a  href='" & url & "'>Download file</a> instead.</video>"
                Return html5Video
            Catch ex As Exception

            End Try
            Return String.Empty
        End Function

        Public Shared Function ConvertObjectVideoToIframe(ByVal objVideo As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(objVideo)) Then

                Dim src As String = objVideo
                Dim oTag As Integer = src.IndexOf("<object")
                Dim tmp As String = String.Empty
                Dim tmp2 As String = String.Empty

                While oTag >= 0
                    result &= src.Substring(0, oTag)
                    tmp = src.Substring(oTag)
                    Dim cTag As Integer = tmp.IndexOf("</object>")
                    Dim compare As String = String.Empty
                    Dim link As String = String.Empty
                    Dim w As Integer = 502
                    Dim h As Integer = 282
                    If cTag >= 0 Then
                        compare = tmp.Substring(0, cTag + 9)
                        Dim url As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                        If (url.Contains("www.nss.com")) Then
                            compare = compare.Replace("http://", "https://")
                        End If
                        tmp2 = tmp.Substring(cTag + 9)
                        'If compare.ToLower().Contains("youtube.com/") Or compare.ToLower().Contains("/embed/how-to-video/") Then
                        If compare.ToLower().Contains("youtube.com/") Or compare.ToLower().Contains(".mp4") Then
                            ''Dim i As Integer = compare.IndexOf("data=""http://")
                            Dim i As Integer = compare.IndexOf("data=""")
                            If i >= 0 Then
                                link = compare.Substring(i + 6)
                                i = link.IndexOf("""")
                                If i > 0 Then
                                    link = link.Substring(0, i)
                                End If
                            Else
                                Dim s As String = "<param name=""src"" value="""
                                i = compare.IndexOf(s)
                                If i >= 0 Then
                                    link = compare.Substring(i + s.Length)
                                    i = link.IndexOf("""")
                                    If i > 0 Then
                                        link = link.Substring(0, i)
                                    End If
                                End If
                            End If
                            Try
                                Dim cw As Integer = compare.IndexOf("width=""")
                                Dim ch As Integer = compare.IndexOf("height=""")
                                Dim size As String = String.Empty
                                If cw >= 0 Then
                                    size = compare.Substring(cw + 7)
                                    cw = size.IndexOf("""")
                                    If cw > 0 Then
                                        w = Convert.ToInt32(size.Substring(0, cw))
                                    End If
                                End If
                                If ch >= 0 Then
                                    size = compare.Substring(ch + 8)
                                    ch = size.IndexOf("""")
                                    If ch > 0 Then
                                        h = Convert.ToInt32(size.Substring(0, ch))
                                    End If
                                End If
                            Catch ex As Exception
                                w = 502
                                h = 282
                            End Try
                        End If
                    End If
                    src = tmp2
                    oTag = src.IndexOf("<object")
                    If oTag >= 0 Then
                        tmp2 = String.Empty
                    End If

                    If String.IsNullOrEmpty(link) Then
                        result &= compare & tmp2
                    Else
                        If link.ToLower().Contains("youtube.com/") Then
                            link = link.Replace("/v/", "/embed/")
                            result &= "<iframe width='" & w & "' height='" & h & "' src='" & link.Trim() & "?autoplay=1&rel=0&wmode=transparent&version=3&modestbranding=1&showinfo=0&ps=docs&nologo=1' frameborder='0' allowfullscreen></iframe>" & tmp2
                        Else
                            result &= "<iframe width='" & w & "' height='" & h & "' scrolling=""no"" src='" & link.Trim() & "' frameborder='0'></iframe>" & tmp2
                        End If
                    End If
                End While
            End If
            If String.IsNullOrEmpty(result) Then
                result = objVideo
            End If
            Return result
        End Function

        Public Shared Function ConvertStartNumberToTextLevelOrderReview(ByVal star As Integer) As String
            Dim result As String = String.Empty
            Select Case star
                Case 1
                    result = "(Awful)"
                Case 2
                    result = "(Poor)"
                Case 3
                    result = "(Neutral)"
                Case 4
                    result = "(Good)"
                Case Else
                    result = "(Excellent)"
            End Select
            Return result
        End Function

        Public Shared Function ConvertTextLevelOrderReviewToStartNumber(ByVal levelstar As String) As String
            Dim result As String = levelstar
            Select Case levelstar
                Case "Awful"
                    result = 1
                Case "Poor"
                    result = 2
                Case "Neutral"
                    result = 3
                Case "Good"
                    result = 4
                Case "excellence"
                    result = 5
            End Select
            Return result
        End Function

        Public Shared Function GetValueSortBy(ByVal Sort As Integer) As String
            Dim result As String = String.Empty
            Select Case Sort
                Case 1
                    result = "price"
                Case 2
                    result = "hot-items"
                Case 3
                    result = "on-sale"
                Case 4
                    result = "best-sellers"
                Case 5
                    result = "new-items"
                Case 6
                    result = "top-rated"
                Case 7
                    result = "most-popular-review"
                Case Else
                    result = ""
            End Select
            Return result
        End Function

        Public Shared Function FormatPointPrice(ByVal point As Integer) As String
            Dim result As String = String.Empty
            Dim sysbol As String = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol
            result = FormatCurrency(point, 0).Replace(sysbol, "") & " pts"
            Return result
        End Function
        Public Shared Function FormatPointPrice(ByVal point As Integer, ByVal showUnit As Boolean) As String
            Dim result As String = String.Empty
            Dim sysbol As String = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol
            result = FormatCurrency(point, 0).Replace(sysbol, "") & ""
            Return result
        End Function
        Public Shared Function CheckSearchKeyword(ByVal strKw As String) As String
            strKw = strKw.Replace("[", "")
            strKw = strKw.Replace("]", "")
            If Not String.IsNullOrEmpty(strKw) Then
                strKw = Regex.Replace(strKw, "%20", " ")
                'strKw = Regex.Replace(strKw, "[!@#$%^*()<>?{}*+',|""]", "")
                strKw = Regex.Replace(strKw, "[!@#$%^&*()<>?{}*-+',|""]", " ")
            End If
            Return strKw
        End Function
        Public Shared Function IsShipToRussia(ByVal order As DataLayer.StoreOrderRow) As Boolean
            If (order Is Nothing) Then
                Return False
            End If
            If order.ShipToCountry = "RS" Or order.BillToCountry = "RS" Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function GetComputerLanIP() As String
            Dim strHostName As String = System.Net.Dns.GetHostName()
            Dim ipEntry As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)

            For Each ipAddress As System.Net.IPAddress In ipEntry.AddressList
                If ipAddress.AddressFamily.ToString() = "InterNetwork" Then
                    Return ipAddress.ToString()
                End If
            Next

            Return "-"
        End Function
        Public Shared Function GetHTMLOrderBillingAddress(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = String.Empty
            Dim countryBill As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.BillToCountry))
            result &= " <div class='name'>" & TrimString(o.BillToName & " " & o.BillToName2) & "</div>"
            result &= "<div class='address'>" & TrimString(o.BillToAddress) & "<br>"
            result &= TrimString(o.BillToCity)
            If Not String.IsNullOrEmpty(o.BillToCounty) Then
                result &= ", " & TrimString(o.BillToCounty)
            End If

            If Not String.IsNullOrEmpty(o.BillToZipcode) Then
                result &= ", " & TrimString(o.BillToZipcode) & ", " & TrimString(countryBill)
            Else
                result &= ", " & TrimString(countryBill)
            End If

            result &= "</div>"

            Return result
        End Function
        Public Shared Function IsCheckShippingSpecialUSState(ByVal State As String) As Boolean
            If State = "PR" Or State = "VI" Or State = "HI" Or State = "AK" Or State = "AA" Or State = "AP" Or State = "AE" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsCompleteIntAddress(ByVal objAddress As MemberAddressRow) As Boolean
            If objAddress.Country <> "US" Then
                Return True
            End If
            'If State = "PR" Or State = "VI" Or State = "HI" Or State = "AK" Then
            '    Return True
            'End If
            Return False
        End Function
        Public Shared Function IsUSAddress(ByVal address As MemberAddressRow)
            If address.Country <> "US" Or (address.Country = "US" AndAlso IsCheckShippingSpecialUSState(address.State)) Then
                Return False
            End If
            Return True
        End Function

        Public Shared Function FirstCharToUpper(input As String) As String
            If [String].IsNullOrEmpty(input) Then
                Throw New ArgumentException("FirstCharToUpper Error!")
            Else
                If input.Contains(" ") Then
                    Dim result As String = String.Empty
                    For Each s As String In input.Split(" ")
                        result &= s.First().ToString().ToUpper() + s.Substring(1) & " "
                    Next

                    Return result.Trim()
                Else
                    Return input.First().ToString().ToUpper() + input.Substring(1)
                End If
            End If
        End Function

        Public Shared Sub OrderLog(ByVal OrderId As Integer, ByVal Action As String, ByVal Description As String())
            Dim iOrderId As Integer = 0
            If OrderId <= 0 Then
                iOrderId = Utility.Common.GetCurrentOrderId()
            Else
                iOrderId = OrderId
            End If

            If iOrderId <= 0 Then
                Exit Sub
            End If

            Try
                Dim strPageName As String = Path.GetFileName(System.Web.HttpContext.Current.Request.PhysicalPath)
                strPageName = strPageName.Replace(".aspx", "")
                Select Case strPageName
                    Case "billingint"
                        strPageName = "billing-international"
                    Case "edit"
                        If System.Web.HttpContext.Current.Request.Url.ToString().Contains("addressbook") Then
                            strPageName = "add-address-book"
                        End If
                End Select
                strPageName = strPageName.Replace("-", " ")
                strPageName = FirstCharToUpper(strPageName)


                Dim strDescription As String = String.Empty

                If Description IsNot Nothing Then
                    For Each s As String In Description
                        strDescription &= "<br>" & s
                    Next
                End If

                If Not StoreOrderLogRow.Add(iOrderId, strPageName, Action, strDescription) Then
                    Components.Email.SendError("ToError500", "Add Tracking Order Error", "OrderId: " & OrderId.ToString() & "<br>Page: " & strPageName & "<br>Action: " & Action & "<br>Description: " & strDescription)
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Log Tracking Order", "OrderId: " & OrderId & "<br>" & ex.ToString())
            End Try
        End Sub

        Public Shared Function GetHTMLMailOrderShippingAddress(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = AddObjectAddress(o.ShipToSalonName)
            result &= AddObjectAddress(o.ShipToName & " " & o.ShipToName2)
            result &= AddObjectAddress(o.ShipToAddress) & AddObjectAddress(o.ShipToAddress2)
            result &= AddObjectAddress(Trim(o.ShipToCity) & ", " & Trim(o.ShipToCounty) & " " & Trim(o.ShipToZipcode))
            Dim countryShip As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.ShipToCountry))
            result &= AddObjectAddress(countryShip)
            If Not String.IsNullOrEmpty(TrimString(o.ShipToPhone)) And Not String.IsNullOrEmpty(TrimString(o.ShipToPhoneExt)) Then
                result &= AddObjectAddress(o.ShipToPhone & " ext " & o.ShipToPhoneExt)
            ElseIf String.IsNullOrEmpty(TrimString(o.ShipToPhoneExt)) Then
                result &= AddObjectAddress(o.ShipToPhone)
            End If
            Dim email As String = TrimString(o.Email)
            If Not (String.IsNullOrEmpty(email)) Then
                result &= "<span class=""bold""><a href=""mailto:" & email & """>" & email & "</a></span>"
            End If
            Return result
        End Function
        Public Shared Function GetHTMLMailOrderBillingAddress(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = AddObjectAddress(o.BillToSalonName)
            result &= AddObjectAddress(o.BillToName & " " & o.BillToName2)
            result &= AddObjectAddress(o.BillToAddress) & AddObjectAddress(o.BillToAddress2)
            result &= AddObjectAddress(TrimString(o.BillToCity) & ", " & TrimString(o.BillToCounty) & " " & TrimString(o.BillToZipcode))
            Dim countryBill As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.BillToCountry))
            result &= AddObjectAddress(countryBill)
            If Not String.IsNullOrEmpty(TrimString(o.BillToPhone)) And Not String.IsNullOrEmpty(TrimString(o.BillToPhoneExt)) Then
                result &= AddObjectAddress(o.BillToPhone & " ext " & o.BillToPhoneExt)
            ElseIf String.IsNullOrEmpty(TrimString(o.BillToPhoneExt)) Then
                result &= AddObjectAddress(o.BillToPhone)
            End If
            Dim email As String = TrimString(o.Email)
            If Not (String.IsNullOrEmpty(email)) Then
                result &= "<span class=""bold""><a href=""mailto:" & email & """>" & email & "</a></span>"
            End If
            Return result
        End Function
        Public Shared Function GetFullHTMLOrderBillingAddress(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = String.Empty
            Dim countryBill As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.BillToCountry))

            result = "<ul><li class='title'>Billing Address</li>"
            If Not String.IsNullOrEmpty(o.BillToSalonName) Then
                result &= "<li>" & TrimString(o.BillToSalonName) & "</li>"
            End If
            result &= "<li>" & TrimString(o.BillToName & " " & o.BillToName2) & "</li>"
            result &= "<li>" & TrimString(o.BillToAddress) & "</li>"
            If Not String.IsNullOrEmpty(o.BillToAddress2) Then
                result &= "<li>" & TrimString(o.BillToAddress2) & "</li>"
            End If
            result &= "<li>" & TrimString(o.BillToCity) & ", " & TrimString(o.BillToCounty) & ", " & TrimString(o.BillToZipcode) & ", " & TrimString(countryBill) & " </li>"
            result &= "<li>" & TrimString(o.BillToPhone)
            If Not String.IsNullOrEmpty(o.BillToPhoneExt) Then
                result &= " ext " & TrimString(o.BillToPhoneExt) & ""
            End If
            result &= "</li>"
            result &= "<li><a href='mailto:" & TrimString(o.Email) & "'>" & TrimString(o.Email) & "</a> </li>"
            result &= "</ul>"
            Return result
        End Function
        Public Shared Function GetFullHTMLOrderShippingAddress(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = String.Empty
            Dim country As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.ShipToCountry))

            result = "<ul><li class='title'>Shipping Address</li>"
            If Not String.IsNullOrEmpty(o.ShipToSalonName) Then
                result &= "<li>" & TrimString(o.ShipToSalonName) & "</li>"
            End If
            result &= "<li>" & TrimString(o.ShipToName & " " & o.ShipToName2) & "</li>"
            result &= "<li>" & TrimString(o.ShipToAddress) & "</li>"
            If Not String.IsNullOrEmpty(o.ShipToAddress2) Then
                result &= "<li>" & TrimString(o.ShipToAddress2) & "</li>"
            End If
            result &= "<li>" & TrimString(o.ShipToCity) & ", " & TrimString(o.ShipToCounty) & ", " & TrimString(o.ShipToZipcode) & ", " & TrimString(country) & " </li>"
            result &= "<li>" & TrimString(o.ShipToPhone)
            If Not String.IsNullOrEmpty(o.ShipToPhoneExt) Then
                result &= " ext " & TrimString(o.ShipToPhoneExt)
            End If
            result &= "</li>"
            result &= "<li><a href='mailto:" & TrimString(o.Email) & "'>" & TrimString(o.Email) & "</a> </li>"
            result &= "</ul>"
            Return result
        End Function
        Public Shared Function GetHTMLOrderShippingAddress(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = AddObjectAddress(o.ShipToSalonName)
            result &= AddObjectAddress(o.ShipToName & " " & o.ShipToName2)
            result &= AddObjectAddress(o.ShipToAddress) & AddObjectAddress(o.ShipToAddress2)
            result &= AddObjectAddress(Trim(o.ShipToCity) & ", " & Trim(o.ShipToCounty) & " " & Trim(o.ShipToZipcode))
            Dim countryShip As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.ShipToCountry))
            result &= AddObjectAddress(countryShip)
            If Not String.IsNullOrEmpty(TrimString(o.ShipToPhone)) And Not String.IsNullOrEmpty(TrimString(o.ShipToPhoneExt)) Then
                result &= AddObjectAddress(o.ShipToPhone & " ext " & o.ShipToPhoneExt)
            ElseIf String.IsNullOrEmpty(TrimString(o.ShipToPhoneExt)) Then
                result &= AddObjectAddress(o.ShipToPhone)
            End If
            Dim email As String = TrimString(o.Email)
            If Not (String.IsNullOrEmpty(email)) Then
                result &= "<span class=""bold""><a style='color:#3b76ba;' href=""mailto:" & email & """>" & email & "</a></span>"
            End If
            Return result
        End Function
        Public Shared Function GetHTMLOrderBillingAddressTracking(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = AddObjectAddressTracking(o.BillToSalonName)
            result &= AddObjectAddressTracking(o.BillToName & " " & o.BillToName2)
            result &= AddObjectAddressTracking(o.BillToAddress) & AddObjectAddressTracking(o.BillToAddress2)
            result &= AddObjectAddressTracking(TrimString(o.BillToCity) & ", " & TrimString(o.BillToCounty) & " " & TrimString(o.BillToZipcode))
            Dim countryBill As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.BillToCountry))
            result &= AddObjectAddressTracking(countryBill)
            If Not String.IsNullOrEmpty(TrimString(o.BillToPhone)) And Not String.IsNullOrEmpty(TrimString(o.BillToPhoneExt)) Then
                result &= AddObjectAddressTracking(o.BillToPhone & " ext " & o.BillToPhoneExt)
            ElseIf String.IsNullOrEmpty(TrimString(o.BillToPhoneExt)) Then
                result &= AddObjectAddressTracking(o.BillToPhone)
            End If
            Dim email As String = TrimString(o.Email)
            If Not (String.IsNullOrEmpty(email)) Then
                result &= "<span class=""bold""><a href=""mailto:" & email & """>" & email & "</a></span>"
            End If
            Return result
        End Function

        Public Shared Function GetHTMLOrderShippingAddressTracking(ByVal DB As Database, ByVal o As DataLayer.StoreOrderRow) As String
            Dim result As String = AddObjectAddressTracking(o.ShipToSalonName)
            result &= AddObjectAddressTracking(o.ShipToName & " " & o.ShipToName2)
            result &= AddObjectAddressTracking(o.ShipToAddress) & AddObjectAddressTracking(o.ShipToAddress2)
            result &= AddObjectAddressTracking(Trim(o.ShipToCity) & ", " & Trim(o.ShipToCounty) & " " & Trim(o.ShipToZipcode))
            Dim countryShip As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & DB.Quote(o.ShipToCountry))
            result &= AddObjectAddressTracking(countryShip)
            If Not String.IsNullOrEmpty(TrimString(o.ShipToPhone)) And Not String.IsNullOrEmpty(TrimString(o.ShipToPhoneExt)) Then
                result &= AddObjectAddressTracking(o.ShipToPhone & " ext " & o.ShipToPhoneExt)
            ElseIf String.IsNullOrEmpty(TrimString(o.ShipToPhoneExt)) Then
                result &= AddObjectAddressTracking(o.ShipToPhone)
            End If
            Dim email As String = TrimString(o.Email)
            If Not (String.IsNullOrEmpty(email)) Then
                result &= "<span class=""bold""><a href=""mailto:" & email & """>" & email & "</a></span>"
            End If
            Return result
        End Function
        Public Shared Function TrimString(ByVal value As String) As String
            If value Is Nothing Then
                Return Nothing
            End If
            Return value.Trim()
        End Function
        Public Shared Function Trim(ByVal value As String) As String
            If value Is Nothing Then
                Return String.Empty
            End If
            Return value.Trim()
        End Function
        Private Shared Function AddObjectAddress(ByVal value As String) As String
            value = TrimString(value)
            If String.IsNullOrEmpty(value) Then
                Return String.Empty
            End If
            Return value & "<br/>"
        End Function
        Private Shared Function AddObjectAddressTracking(ByVal value As String) As String
            value = TrimString(value)
            If String.IsNullOrEmpty(value) Then
                Return String.Empty
            End If
            Return "<b>" & value & "</b><br/>"
        End Function
        Public Shared Function FitterStringInDB(ByVal data As String) As String
            Return data.Replace("'", "''")
        End Function
        Public Shared Function GetVideoResource(ByVal link As String, ByVal w As Integer, ByVal h As Integer, ByVal autoPlay As Integer, ByVal relatedVideo As Integer, Optional ByVal IsPecent As Boolean = False) As String
            Dim listID As String = GetYoutubeListId(link)
            If String.IsNullOrEmpty(listID) Then
                link = ConvertVideoYoutube(link, autoPlay, relatedVideo) & "&wmode=transparent&version=3&modestbranding=1&showinfo=0&ps=docs&nologo=1"
            Else
                link = ConvertVideoYoutubePlayList(link, autoPlay, listID) & "&wmode=transparent&version=3&modestbranding=1&showinfo=1&ps=docs&nologo=1"
            End If

            Return "<iframe src=" & link & " frameborder='0' width='" & w & IIf(IsPecent, "%", "") & "'" & " height='" & h & IIf(IsPecent, "%", "") & "' allowfullscreen webkitallowfullscreen mozallowfullscreen wmode=""Opaque""></iframe>"
        End Function

        Public Shared Function ConvertVideoYoutube(ByVal link As String, ByVal autoPlay As Integer, ByVal relatedVideo As Integer) As String
            Dim result As String = "https://www.youtube.com/embed/{0}?autoplay={1}&rel={2}"
            Dim indexPara As Integer = link.LastIndexOf("?v=")
            Dim indexTube As Integer = link.IndexOf("youtu.be/")
            Dim indexEmb As Integer = link.LastIndexOf("/v/")
            Dim value As String = String.Empty
            If indexPara > 0 Then
                Dim indexQ As Integer = link.IndexOf("&")
                If indexQ > 0 Then
                    value = link.Substring(indexPara + 3, indexQ - indexPara - 3)
                Else
                    value = link.Substring(indexPara + 3, link.Length - indexPara - 3)
                End If

                result = String.Format(result, value, autoPlay, relatedVideo)
            ElseIf indexTube > 0 Then
                value = link.Substring(indexTube + 9)
                result = String.Format(result, value, autoPlay, relatedVideo)
            ElseIf indexEmb > 0 Then
                result = String.Format(link & "?autoplay={0}&rel={1}", autoPlay, relatedVideo)
            End If
            Return result
        End Function
        Public Shared Function ConvertVideoYoutubePlayList(ByVal link As String, ByVal autoPlay As Integer, ByVal listId As String) As String

            Dim result As String = "https://www.youtube.com/embed/videoseries?autoplay=" & autoPlay & "&list=" & listId
            Return result
        End Function
        Public Shared Function GetYoutubeListId(ByVal link As String) As String
            If String.IsNullOrEmpty(link) Then
                Return String.Empty
            End If
            If link.Contains("list=") Then
                Dim arr() As String = link.Split("&")
                If arr.Length > 0 Then
                    For Each item As String In arr
                        If item.Contains("list=") Then
                            Return item.Replace("list=", "")
                        End If
                    Next
                End If
            End If
            Return String.Empty
        End Function
        Public Shared Function ObjectToString(ByVal entity As Object) As String
            Dim s As String = ""
            Dim formatLog As String = "{0}|{1}[br]"
            For Each [property] As System.Reflection.PropertyInfo In entity.[GetType]().GetProperties()
                Dim value As Object = [property].GetValue(entity, Nothing)
                Dim fieldName As String = [property].Name
                If Not (value Is Nothing) Then
                    If value.ToString() = "Database" Then
                        Continue For
                    End If
                End If
                Try
                    s = s & String.Format(formatLog, fieldName, value)
                Catch ex As Exception

                End Try

            Next
            Return s
        End Function

        Public Shared Function GetSiteLanguage() As String
            Dim language As String = String.Empty
            Try
                language = System.Web.HttpContext.Current.Session("Language")
            Catch ex As Exception

            End Try
            If (String.IsNullOrEmpty(language)) Then
                language = "Desc"
            End If
            Return language
        End Function

        Public Shared Function GenerateCaseLanguage(ByVal column As String, ByVal language As String)
            Dim result As String = " CASE WHEN "
            Dim defaultlanguage As String = "Desc"
            If defaultlanguage = language Then
                Return column & language
            End If
            Dim columSelect As String = column & language
            Dim columDefault As String = column & defaultlanguage
            result = result & columSelect & "=''  THEN    " & columDefault & "  WHEN " & columSelect & " IS NULL   THEN " & columDefault & "   ELSE " & columSelect & " end"
            Return result
        End Function

        'Public Shared Function GetDateForLevelPoint(ByVal DB As Database, ByVal MemberId As Integer) As DateTime
        '    Dim d1 As DateTime = New DateTime(Date.Now.Year, Date.Now.Month, Date.Now.Day)
        '    Dim member As DataLayer.MemberRow = DataLayer.MemberRow.GetRow(DB, MemberId)
        '    Dim d2 As DateTime = New DateTime(member.CreateDate.Year, member.CreateDate.Month, member.CreateDate.Day)
        '    Dim ts As TimeSpan = d1 - d2
        '    If ts.Days <= 365 Then
        '        Return member.CreateDate
        '    Else
        '        Dim addYear As Integer = CInt(ts.Days) \ 365
        '        Dim d3 As DateTime = New DateTime(member.CreateDate.Year + addYear, member.CreateDate.Month, member.CreateDate.Day)
        '        Return d3
        '    End If
        'End Function

        Public Shared Function GetDateForLevelPoint(ByVal DB As Database, ByVal MemberId As Integer) As DateTime
            Dim d As DateTime = New DateTime(Date.Now.Year, 1, 1)
            Return d
        End Function
        Public Shared Function IsPasswordValid(ByVal password As String) As Boolean
            Return Regex.IsMatch(password, "^(?=.*[a-zA-Z]+|[0-9])(?!.*\s).{8,20}$")
        End Function
        Public Shared Function CheckUSPhoneValid(ByVal n1 As String, ByVal n2 As String, ByVal n3 As String) As Boolean
            If (String.IsNullOrEmpty(n1) And String.IsNullOrEmpty(n2) And String.IsNullOrEmpty(n3)) Then
                Return False
            ElseIf (String.IsNullOrEmpty(n1) Or String.IsNullOrEmpty(n2) Or String.IsNullOrEmpty(n3)) Then
                Return False
            ElseIf (n1.Trim().Length <> 3 Or n2.Trim().Length <> 3 Or n3.Trim.Length <> 4) Then
                Return False
            End If
            Dim nPhone1 As Integer = -1
            Dim nPhone2 As Integer = -1
            Dim nPhone3 As Integer = -1
            Try
                nPhone1 = CInt(n1)
            Catch ex As Exception
                nPhone1 = -1
            End Try
            Try
                nPhone2 = CInt(n2)
            Catch ex As Exception
                nPhone2 = -1
            End Try
            Try
                nPhone3 = CInt(n3)
            Catch ex As Exception
                nPhone3 = -1
            End Try
            If (nPhone1 < 0 Or nPhone2 < 0 Or nPhone3 < 0) Then
                Return False
            End If
            Return True
        End Function
        Public Shared Function GenerateCodeString(ByVal sInput As String) As String
            If String.IsNullOrEmpty(sInput) Then
                Return String.Empty
            End If
            sInput = sInput.Trim().ToLower()
            Dim sArrayRemove As String()() = New String()() {New String() {"á", "à", "ả", "ã", "ạ", "â", _
             "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", _
             "ắ", "ằ", "ẳ", "ẵ", "ặ"}, New String() {"é", "è", "ẻ", "ẽ", "ẹ", "ê", _
             "ế", "ề", "ể", "ễ", "ệ"}, New String() {"ú", "ù", "ủ", "ũ", "ụ", "ư", _
             "ứ", "ừ", "ử", "ữ", "ự"}, New String() {"í", "ì", "ỉ", "ĩ", "ị"}, New String() {"ó", "ò", "ỏ", "õ", "ọ", "ô", _
             "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", _
             "ớ", "ờ", "ở", "ỡ", "ợ"}, New String() {"đ"}}
            Dim sArrayReplace As String() = {"a", "e", "u", "i", "o", "d"}
            Dim objRegex As New Regex("^[a-z0-9]{1}$")
            Dim sItem As String = ""
            Dim iLengthRemove As Integer = sArrayReplace.Length
            For i As Integer = 0 To iLengthRemove - 1
                For Each itemRemove As String In sArrayRemove(i)
                    sInput = sInput.Replace(itemRemove, sArrayReplace(i))
                Next
            Next
            Dim cArrayInput As Char() = sInput.ToCharArray()
            Dim iLengthInput As Integer = cArrayInput.Length
            Dim iCountWhiteSpace As Integer = 0
            sInput = ""
            For i As Integer = 0 To iLengthInput - 1
                sItem = cArrayInput(i).ToString()
                If sItem = " " OrElse sItem = "-" Then
                    iCountWhiteSpace += 1
                    If iCountWhiteSpace = 1 Then
                        sInput += sItem
                    End If
                Else
                    If objRegex.IsMatch(sItem) Then
                        sInput += sItem
                        iCountWhiteSpace = 0
                    End If
                End If
            Next
            Return sInput.Replace(" ", "-")
        End Function

        Public Shared Function DecodePaypalUrl(ByVal src As String) As String
            Dim result As String = System.Web.HttpUtility.UrlDecode(src, System.Text.Encoding.Default)
            Return result
        End Function

        Public Shared Function GetMinMaxValue(ByVal value As String, ByVal MaxValue As Boolean) As Double
            Try
                value = value.Replace("(", "")
                value = value.Replace(")", "")
                Dim i As Integer = value.IndexOf(",")
                If i >= 0 Then
                    If MaxValue Then
                        value = value.Substring(i)
                    Else
                        value = value.Substring(0, i)
                    End If
                    value = value.Replace(",", "")
                    Return Convert.ToDouble(value)
                End If
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function CountWords(ByVal text As String) As Integer
            If (String.IsNullOrEmpty(text)) Then
                Return 0
            End If
            If (text.Contains(Environment.NewLine)) Then
                text = text.Replace(Environment.NewLine, " ")
            End If
            If (text.Contains("\n")) Then
                text = text.Replace("\n", " ")
            End If
            Dim fullStr As String = text + " "
            Dim splitString As String() = fullStr.Split(" ")
            If (splitString.Length < 1) Then
                Return 0
            End If

            Dim word_count As Integer = 0
            For Each word As String In splitString
                If Not String.IsNullOrEmpty(word) Then
                    word_count = word_count + 1
                End If
            Next
            Return word_count
        End Function

        Public Shared Function SetMetaDescription(ByVal strMetaDescription As String, ByVal sName As String, Optional ByVal label As String = "") As String
            If (Not String.IsNullOrEmpty(sName)) Then
                sName = sName.Replace("""", "")
                If (String.IsNullOrEmpty(strMetaDescription)) Then
                    If (String.IsNullOrEmpty(label)) Then
                        strMetaDescription = sName
                    Else
                        strMetaDescription = String.Format(label, sName)
                    End If
                Else
                    If (String.IsNullOrEmpty(label)) Then
                        strMetaDescription = strMetaDescription & ", " & sName
                    Else
                        strMetaDescription = strMetaDescription & ", " & String.Format(label, sName)
                    End If
                End If
            End If
            Return strMetaDescription
        End Function
        Public Shared Sub BindStandardShippingMethod(ByVal drl As DropDownList, ByVal addNull As Boolean)
            drl.Items.Clear()
            If (addNull) Then
                drl.Items.Add(New ListItem("- - -", ""))

            End If
            Dim enumValues As Array = System.[Enum].GetValues(GetType(StandardShippingMethod))
            Dim value As Integer
            For Each method As StandardShippingMethod In enumValues
                value = CInt(System.Enum.Parse(GetType(StandardShippingMethod), method))
                drl.Items.Add(New ListItem(method.ToString(), value.ToString))
            Next
        End Sub
        Public Shared Sub BindPromotionType(ByVal drl As DropDownList, ByVal addNull As Boolean)
            drl.Items.Clear()
            If (addNull) Then
                drl.Items.Add(New ListItem("- - -", ""))

            End If
            drl.Items.Add(New ListItem("Percent Off", "Percentage"))
            drl.Items.Add(New ListItem("Dollar Off", "Monetary"))
            drl.Items.Add(New ListItem("Free Item", "Freeitem"))
        End Sub
        Public Shared Function CheckValidEmail(ByVal sEmail As String) As Boolean
            If Not String.IsNullOrEmpty(sEmail.Trim()) Then
                'Dim objRegex As New Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
                Dim objRegex As New Regex("^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$")
                Return objRegex.IsMatch(sEmail.Trim())
            End If
            Return False
        End Function

        Public Shared Sub AddPointReferActiveAccount(ByVal DB As Database, ByVal memberUseReferId As Integer, ByVal emailRefer As String, ByVal referCode As String, ByVal memberUseReferUserName As String)
            Dim MemberReferId As Integer = 0
            Dim MemberReferEmail As String = String.Empty
            Dim dtMemberRefer = DB.GetDataTable("Select MemberId,c.Email from Member m left join Customer c on(c.CustomerId=m.CustomerId)  where ReferCode='" & referCode.Trim() & "'")
            If Not dtMemberRefer Is Nothing Then
                If (dtMemberRefer.Rows.Count > 0) Then
                    MemberReferId = dtMemberRefer.Rows(0)("MemberId").ToString()
                    MemberReferEmail = dtMemberRefer.Rows(0)("Email").ToString()
                End If
            End If
            If (memberUseReferId > 0 And Not String.IsNullOrEmpty(MemberReferEmail)) Then
                Dim statusRefer As Integer = DB.ExecuteScalar("Select COALESCE([Status],0) from MemberRefer where MemberRefer=" & MemberReferId & " and Email='" & emailRefer & "'")
                If (statusRefer = Utility.Common.ReferFriendStatus.Register) Then
                    ''update refer status
                    DB.ExecuteSQL("Update MemberRefer set [Status]=" & Utility.Common.ReferFriendStatus.ActiveAccount & ",ModifyDate=" & DB.Quote(DateTime.Now) & " where MemberRefer=" & MemberReferId & " and Email='" & emailRefer & "'")
                    ''add point for user register
                    Dim CP As DataLayer.CashPointRow = New DataLayer.CashPointRow(DB)
                    CP.PointEarned = DataLayer.SysParam.GetValue("PointUseReferFriend")
                    CP.PointDebit = 0
                    CP.Notes = "Referred from " & MemberReferEmail
                    CP.MemberId = memberUseReferId
                    CP.CreateDate = Now
                    CP.ApproveDate = Now
                    CP.Status = 1
                    CP.Insert()
                    ''send mail to user
                    Dim pathTemplate As String = Utility.ConfigData.AddPointUseReferFriendMailTemplate ''System.Web.HttpContext.Current.Server.MapPath("/includes/MailTemplate/AddPointUseReferFriend.txt")
                    Dim mailTitle As String = Utility.ConfigData.SubjectReferAddPoint
                    Dim mailBody As String = String.Empty
                    'LoadMailTxtTemplate(pathTemplate, mailTitle, mailBody)
                    mailBody = Components.Core.OpenFile(pathTemplate)
                    'If Not String.IsNullOrEmpty(mailTitle) And Not String.IsNullOrEmpty(mailBody) Then
                    If Not String.IsNullOrEmpty(mailBody) Then
                        Dim totalPoint As Integer = DataLayer.CashPointRow.GetTotalCashPointByMember(DB, memberUseReferId, Nothing)
                        mailBody = String.Format(mailBody, emailRefer, MemberReferEmail, CP.PointEarned, totalPoint)
                        Components.Email.SendHTMLMail(Components.FromEmailType.NoReply, emailRefer, emailRefer, mailTitle, mailBody)
                    End If

                    Components.Email.SendError("ToReportRefer", "Add Point Register Refer", "username:" & memberUseReferUserName & " <br>ReferCode=" & referCode & " <br>Date=" & DateTime.Now.ToString())
                End If
            End If
        End Sub
        Public Shared Sub LoadMailTxtTemplate(ByVal path As String, ByRef mailTitle As String, ByRef mailBody As String)
            Dim Lines As String() = System.IO.File.ReadAllLines(path)
            For i As Integer = 0 To Lines.Length - 1
                If i = 0 Then
                    mailTitle = Lines(i).ToString()
                Else
                    mailBody += Lines(i).ToString() & "<br/>"
                End If
            Next
        End Sub
        Public Shared Sub BindEnumLogObjectType(ByVal drp As DropDownList, ByVal addNullOption As Boolean)
            ''ObjectType()
            drp.Items.Clear()
            If (addNullOption) Then
                drp.Items.Add(New ListItem("- - -", ""))
            End If
            Dim objType As ObjectType
            For Each objType In [Enum].GetValues(GetType(ObjectType))
                Dim name As String = EnumDescription(objType)
                Dim value As String = objType.ToString()
                Dim optionItem As New ListItem(name, value)
                drp.Items.Add(optionItem)
            Next
        End Sub
        Public Shared Sub BindEnumLogAction(ByVal drl As DropDownList)
            ''ObjectType()
            drl.Items.Clear()
            drl.Items.Add(New ListItem("- - -", ""))
            For Each action In [Enum].GetValues(GetType(AdminLogAction))
                Dim strMsgType As String = action.ToString()
                Dim optionItem As New ListItem(strMsgType, strMsgType)
                drl.Items.Add(optionItem)
            Next
        End Sub

        Public Shared Function RemoveHTMLTag(ByVal source As String) As String
            source = ProcessString(source)
            Dim array As Char() = New Char(source.Length - 1) {}
            Dim arrayIndex As Integer = 0
            Dim inside As Boolean = False

            For i As Integer = 0 To source.Length - 1
                Dim [let] As Char = source(i)
                If [let] = "<"c Then
                    inside = True
                    Continue For
                End If
                If [let] = ">"c Then
                    inside = False
                    Continue For
                End If
                If Not inside Then
                    array(arrayIndex) = [let]
                    arrayIndex += 1
                End If
            Next
            Return New String(array, 0, arrayIndex)
        End Function
        Public Shared Function ConvertItemAcceptingStatusToName(ByVal value As Integer) As String
            Dim objSource As Utility.Common.ItemAcceptingStatus
            For Each objSource In [Enum].GetValues(GetType(Utility.Common.ItemAcceptingStatus))
                Dim valueEnum As Integer = Convert.ToInt32(objSource)
                If valueEnum = value Then
                    Return Utility.Common.EnumDescription(objSource)
                End If
            Next
            Return String.Empty
        End Function
        'format html
        Public Shared Function ProcessString(ByVal strInputHtml As String) As String
            Dim strOutputXhtml As String = [String].Empty
            Dim reader As New SgmlReader()
            reader.DocType = "HTML"
            Dim sr As StringReader = New System.IO.StringReader(strInputHtml)
            reader.InputStream = sr
            Dim sw As New StringWriter()
            Dim w As New XmlTextWriter(sw)
            reader.Read()
            Try
                While Not reader.EOF
                    w.WriteNode(reader, True)
                End While
            Catch
            End Try
            w.Flush()
            w.Close()

            Return sw.ToString().Trim()
        End Function
        Public Shared Function RenderUserControl(ByVal path As String, Optional ByVal bRegex As Boolean = True) As String
            Dim pageHolder As New Components.RenderPage()
            Dim viewControl As System.Web.UI.UserControl = DirectCast(pageHolder.LoadControl(path), System.Web.UI.UserControl)
            pageHolder.Controls.Add(viewControl)
            Dim output As New StringWriter()
            System.Web.HttpContext.Current.Server.Execute(pageHolder, output, False)

            If bRegex Then
                Dim s As String = Regex.Replace(output.ToString(), "\t|\n|\r", "")
                s = Regex.Replace(s, "\s{2,}", " ")
                Return s
            Else
                Return output.ToString()
            End If
        End Function
        Public Shared Function ExportHTMLControl(Of T As {System.Web.UI.Control, New})(ByVal c As T) As String
            ' get the text for the control
            Using sw As New System.IO.StringWriter()
                Using htw As New System.Web.UI.HtmlTextWriter(sw)
                    c.RenderControl(htw)
                    Return sw.ToString()
                End Using
            End Using
        End Function
#Region "Shipping"
        Public Shared Function ShippingTBD() As String
            Return "TBD"
        End Function
        Public Shared Function IsFedExValidator() As Boolean
            Dim FedExValidator As Boolean = SysParam.GetValue("FedExValidator")
            Return FedExValidator
        End Function
        Public Shared Function IsFedExCalculator() As Boolean
            Dim FedExCalculator As Boolean = SysParam.GetValue("FedExCalculator")
            Return FedExCalculator
        End Function

        Public Shared Function DefaultShippingId() As Integer
            Dim result As Integer = 14
            If IsFedExCalculator() Then
                result = 16
            End If
            Return result
        End Function
        Public Shared Function TruckShippingId() As Integer
            Return 4
        End Function
        Public Shared Function NonExpeditedShippingIds() As String
            Dim result As String = "14"
            If IsFedExCalculator() Then
                result = "16"
            End If
            Return result
        End Function
        Public Shared Function PickupShippingId() As Integer
            Return 8
        End Function

        Public Shared Function USPSPriorityShippingId() As Integer
            Dim result As Integer = 15
            Return result
        End Function

        Public Shared Function FirstClassShippingId() As Integer
            Dim result As Integer = 10
            Return result
        End Function

        Public Shared Function InternationalShippingId() As String
            Return ",15,10,11,"
        End Function

        Public Shared Function UPS3DayShippingId() As Integer
            Dim result As Integer = 1
            'If IsFedExValidator() Then
            '    result = 19
            'End If
            Return result
        End Function
        Public Shared Function UPS3DayShippingName() As String
            Dim result As String = "UPS 3-Day Service"
            'If IsFedExValidator() Then
            '    result = 19
            'End If
            Return result
        End Function
        Public Shared Function UPS2DayShippingId() As Integer
            Dim result As Integer = 2
            If IsFedExCalculator() Then
                result = 17
            End If
            Return result
        End Function
        Public Shared Function UPSNextDayShippingId() As Integer
            Dim result As Integer = 3
            If IsFedExCalculator() Then
                result = 18
            End If
            Return result
        End Function
        Public Shared Function FreightDeliveryShippingName() As String
            Dim result As String = "Freight Delivery"
            Return result
        End Function
        Public Shared Function USShippingCode() As String
            Dim result As String = "'UPS3DAY','UPS2DAY','UPSNEXTDAY','UPS'"
            If IsFedExCalculator() Then
                result = "'FED','FED2DAY','FEDNEXTDAY'"
            End If
            Return result
        End Function

        Public Shared Function PrefixShippingUPSCode() As String
            Return "UPS"
        End Function
        Public Shared Function PrefixShippingFEDCode() As String
            Return "FED"
        End Function
        Public Shared Function GetShippingInternationalMethodName() As String
            Dim result As String = "International Shipping"
            Return result
        End Function
        Public Shared Function HasPickup(ByVal DB As Database, ByVal orderId As Integer) As Boolean
            Dim sql As String = "Select COUNT(*) from StoreCartItem where OrderId=" & orderId & " and Type='carrier' and CarrierType=" & Utility.Common.PickupShippingId
            Dim count As Integer = DB.ExecuteScalar(sql)
            If (count > 0) Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function GetDefaultShippingByOrderId(ByVal DB As Database, ByVal orderId As Integer) As Integer
            If (HasPickup(DB, orderId)) Then
                Return Utility.Common.PickupShippingId
            End If
            If StoreOrderRow.OnlyOversizeItems(DB, orderId) Then
                Return Utility.Common.TruckShippingId
            End If
            Dim sql As String = "select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType)  where orderid = " & orderId & " and  type = 'item'  and Code in(" & Utility.Common.USShippingCode & ") and isFreeItem = 0"
            Dim CarrierType As Integer = DB.ExecuteScalar(sql)
            If CarrierType < 1 Then
                CarrierType = DB.ExecuteScalar("select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & orderId & " and  type = 'item'")
            End If
            Return CarrierType
        End Function
#End Region
        Public Shared Function CheckShippingSpecialUS(ByVal ShipToCountry As String, ByVal ShipToCounty As String) As Boolean
            If ShipToCountry = "US" Then
                If ShipToCounty = "PR" Or ShipToCounty = "VI" Or ShipToCounty = "HI" Or ShipToCounty = "AK" Or ShipToCounty = "AP" Or ShipToCounty = "AE" Or ShipToCounty = "AA" Then
                    Return True
                End If
            Else
                If ShipToCountry = "PR" Or ShipToCountry = "VI" Or ShipToCountry = "HI" Or ShipToCountry = "AK" Then
                    Return True
                End If
            End If
            Return False
        End Function

        Public Shared Function CheckShippingSpecialUS(ByVal objOrder As StoreOrderRow) As Boolean
            Return CheckShippingSpecialUS(objOrder.ShipToCountry, objOrder.ShipToCounty)

            'If Order.ShipToCountry = "US" Then
            '    If Order.ShipToCounty = "PR" Or Order.ShipToCounty = "VI" Or Order.ShipToCounty = "HI" Or Order.ShipToCounty = "AK" Then
            '        Return True
            '    End If
            'Else
            '    If Order.ShipToCountry = "PR" Or Order.ShipToCountry = "VI" Or Order.ShipToCountry = "HI" Or Order.ShipToCountry = "AK" Then
            '        Return True
            '    End If
            'End If
            'Return False
        End Function
        'Public Shared Function CheckShippingSpecialUS(ByVal o As StoreOrderRow) As Boolean
        '    If o.ShipToCountry = "US" Then
        '        If o.ShipToCounty = "PR" Or o.ShipToCounty = "VI" Or o.ShipToCounty = "HI" Or o.ShipToCounty = "AK" Then
        '            Return True
        '        End If
        '    Else
        '        If o.ShipToCountry = "PR" Or o.ShipToCountry = "VI" Or o.ShipToCountry = "HI" Or o.ShipToCountry = "AK" Then
        '            Return True
        '        End If
        '    End If
        '    Return False
        'End Function
        Public Shared Function GetCurrentCustomerPriceGroupId()
            Dim CustomerPriceGroupId As Integer = 0
            If Web.HttpContext.Current.Session("CustomerPriceGroupId") IsNot Nothing Then
                CustomerPriceGroupId = CInt(Web.HttpContext.Current.Session("CustomerPriceGroupId"))
            Else
                Dim memberId As Integer = GetCurrentMemberId()
                If memberId < 1 Then
                    Return 0
                End If

                CustomerPriceGroupId = MemberRow.GetCustomerPriceGroupIdByMember(memberId)
                Web.HttpContext.Current.Session("CustomerPriceGroupId") = CustomerPriceGroupId

                Return CustomerPriceGroupId
            End If
        End Function

        Public Shared Sub ResetOrderIdentityId(ByVal DB As Database, ByVal currentId As Integer)
            If currentId > 1 Then
                DB.ExecuteSQL("DBCC CHECKIDENT (StoreSequence, RESEED, " & currentId - 1 & ")")
            End If
        End Sub
        Public Shared Function GetLocationByIP(ByVal IP As String, ByVal NumIP As String, ByVal Type As String) As String
            Dim city, state, countrycode, countryName, result, condition, sql, msgEmail As String
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand
            'IP = "113.161.73.102"
            'Get from db
            msgEmail = Utility.ConfigData.GlobalRefererName & System.Web.HttpContext.Current.Request.RawUrl & "<br>IP:" & IP & "<br>NumIP:" & NumIP & "<br>Type:" & Type & "<br>Sql: " & sql
            Try
                result = String.Empty
                If Type = "countrycode" Then
                    condition = " Country"
                Else
                    condition = " City + " _
                                       & "CASE WHEN [State] = null THEN '' ELSE ', ' + State END + " _
                                       & "CASE WHEN Country = (SELECT countrycode FROM Country WHERE CountryCode = Country) " _
                                       & "THEN ' (' + (SELECT CountryName FROM Country WHERE CountryCode = Country) + ')' ELSE '' END "
                End If
                sql = "SELECT top 1 " & condition & " FROM IPLocation WHERE  FromNumber <= " & NumIP & " and ToNumber >= " & NumIP

                cmd = db.GetSqlStringCommand(sql)
                Try
                    result = db.ExecuteScalar(cmd)
                Catch
                    result = String.Empty
                End Try
                If String.IsNullOrEmpty(result) = False Then
                    Return result
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetCityLocation1-Database" & Now.ToString(), msgEmail & "<br>Error =" & ex.ToString())
            End Try

            'Get from API
            Try
                Dim apiKey As String = Utility.ConfigData.IPInfo
                Dim webRequest As System.Net.HttpWebRequest = System.Net.WebRequest.Create("http://api.ipinfodb.com/v3/ip-city/?key=" & apiKey & "&ip=" & IP & "&format=xml")
                webRequest.Timeout = 10000
                Dim webResponse As System.Net.HttpWebResponse = webRequest.GetResponse()
                Dim reader1 As New XmlTextReader(webResponse.GetResponseStream())
                Dim xmlDoc As New XmlDocument()
                xmlDoc.Load(reader1)
                For i As Integer = 0 To xmlDoc.ChildNodes.Count - 1
                    Dim node1 As XmlNode = xmlDoc.ChildNodes(i)
                    If node1.Name = "Response" Then
                        For j As Integer = 0 To node1.ChildNodes.Count - 1
                            Dim node2 As XmlNode = node1.ChildNodes(j)
                            If node2.Name = "cityName" Then
                                city = node2.InnerText
                            End If
                            If node2.Name = "regionName" Then
                                state = node2.InnerText
                            End If
                            If node2.Name = "countryCode" Then
                                countrycode = node2.InnerText
                            End If
                            If node2.Name = "countryName" Then
                                countryName = IIf(String.IsNullOrEmpty(node2.InnerText) Or node2.InnerText = "", "", " (" & node2.InnerText & ")")
                            End If
                        Next
                    End If

                Next
                If Type = "countrycode" Then
                    If countrycode = "US" Then
                        Dim arr() As String = {"AK", "HI", "PR", "VI"}
                        If arr.Contains(state) Then
                            countrycode = String.Empty
                        End If
                    End If
                    result = countrycode
                Else
                    result = city & IIf(String.IsNullOrEmpty(city), state, ", " & state) & countryName
                    result = StringReplace(result, Utility.ConfigData.TextToFind, Utility.ConfigData.TextToReplace)
                End If

            Catch ex As Exception
                'Components.Email.SendError("ToError500", "GetCityLocation1-API" & Now.ToString(), msgEmail & "<br>Error =" & ex.ToString())
            End Try
            Try
                'insert to table iplocation
                If Not String.IsNullOrEmpty(countrycode) Then
                    Dim DB1 As Database
                    sql = " INSERT INTO IPLocation (" _
                   & " IPFrom" _
                   & ",IPTo" _
                   & ",Country" _
                   & ",State" _
                   & ",City" _
                   & ",FromNumber" _
                   & ",ToNumber" _
                   & ") VALUES (" _
                   & DB1.Quote(IP) _
                   & "," & DB1.Quote(IP) _
                   & "," & DB1.Quote(countrycode) _
                   & "," & DB1.Quote(state) _
                   & "," & DB1.Quote(city) _
                   & "," & DB1.Quote(NumIP) _
                   & "," & DB1.Quote(NumIP) _
                   & ")"
                    cmd = db.GetSqlStringCommand(sql)
                    db.ExecuteNonQuery(cmd)
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetCityLocation1-insert to table iplocation" & Now.ToString(), System.Web.HttpContext.Current.Request.RawUrl & "<br>Error =" & ex.ToString())
            End Try
            Return result
        End Function
        Public Shared Function ConvertIPtoNum(ByVal IP As String) As String
            Dim arrIP As String() = IP.Split(".")
            Dim numIp As String = String.Empty
            For i As Integer = 0 To arrIP.Length - 1
                If arrIP(i).Length = 1 Then
                    numIp &= IIf(i = 0, "", "00") & arrIP(i)
                ElseIf arrIP(i).Length = 2 Then
                    numIp &= IIf(i = 0, "", "0") & arrIP(i)
                Else
                    numIp &= arrIP(i)
                End If
            Next
            Return numIp
        End Function
        Public Shared Function StringReplace(ByVal inputText As String, ByVal TextToFind As String, ByVal TextToReplace As String) As String
            If inputText Is Nothing Then
                Return String.Empty
            End If
            Dim index As Int32 = -1
            While (inputText.IndexOfAny(TextToFind.ToCharArray()) <> -1)
                index = inputText.IndexOfAny(TextToFind.ToCharArray())
                Dim index2 = TextToFind.IndexOf(inputText(index))
                inputText = inputText.Replace(inputText(index), TextToReplace(index2))
            End While
            Return inputText
        End Function
        Private Shared Function GetLocation(ByVal ipaddress As String) As DataTable
            Dim dt As New DataTable
            Dim wreq As System.Net.WebRequest = System.Net.WebRequest.Create("http://ipinfodb.com/ip_query.php?ip=" & ipaddress)
            Dim proxy As New System.Net.WebProxy("http://ipinfodb.com/ip_query.php?ip=" & ipaddress, True)
            wreq.Proxy = proxy
            wreq.Timeout = 5000
            Dim wres As System.Net.WebResponse = wreq.GetResponse()
            Dim xmlRead As New XmlTextReader(wres.GetResponseStream())
            Dim ds As New DataSet()
            ds.ReadXml(xmlRead)
            Return ds.Tables(0)
        End Function

        Public Shared Function IsUSCustomer() As Boolean
            Dim ip As String = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            If ip = "::1" Then
                Return True
            Else
                Dim numIp As String = ConvertIPtoNum(ip)
                Dim countryCode As String = GetLocationByIP(ip, numIp, "countrycode")
                Return countryCode = "US"
            End If
        End Function
        Public Shared Function GetPageTitleByCustomerType(ByVal isUSCustomer As Boolean, ByVal pageTile As String, ByVal outsideUSPageTitle As String, ByVal defaultPageTitle As String) As String
            Dim result As String = String.Empty

            If Not isUSCustomer Then
                If Not String.IsNullOrEmpty(outsideUSPageTitle) Then
                    result = outsideUSPageTitle
                Else
                    result = pageTile
                End If
            Else
                result = pageTile
            End If

            If String.IsNullOrEmpty(result) Then
                result = defaultPageTitle
            End If
            Return result
        End Function

        Public Shared Function GetMetaDescriptionByCustomerType(ByVal isUSCustomer As Boolean, ByVal MetaDescription As String, ByVal outsideUSMetaDescription As String, ByVal defaultMetaDescription As String) As String
            Dim result As String = String.Empty
            If Not isUSCustomer Then
                If Not String.IsNullOrEmpty(outsideUSMetaDescription) Then
                    result = outsideUSMetaDescription
                Else
                    result = MetaDescription
                End If
            Else
                result = MetaDescription
            End If
            If (String.IsNullOrEmpty(result)) Then
                result = defaultMetaDescription
            End If
            Return result
        End Function

        Public Shared Function StringCut(ByVal sString As String, ByVal iBeginPosition As Integer, ByVal iEndPosition As Integer) As String
            If String.IsNullOrEmpty(sString) Then
                Return String.Empty
            End If
            Dim sReturn As String = String.Empty
            Dim iLength As Integer = sString.Length
            If iLength <= (iEndPosition - iBeginPosition) Then
                Return sString
            Else
                Dim sTemp As String() = sString.Substring(iBeginPosition, iEndPosition).Split(" "c)
                Dim iCountTemp As Integer = sTemp.Length
                If iCountTemp > 0 Then
                    sString = String.Empty
                    For intI As Integer = 0 To iCountTemp - 2
                        sString += sTemp(intI).ToString() & " "
                    Next
                    Return sString.TrimEnd() & "..."
                End If
                Return sString
            End If
        End Function
        Public Shared Function GetFlammableSKU(ByVal DB As Database, ByVal OrderId As Integer) As String
            Dim SKUFlammable As String = String.Empty
            Dim lstSKUError As New List(Of String)
            Dim dtFlammable As DataTable = DB.GetDataTable("Select SKU from StoreCartItem   where OrderId=" & OrderId & " and IsFlammable=1")
            If Not dtFlammable Is Nothing Then
                Dim sku As String = String.Empty
                For Each row As DataRow In dtFlammable.Rows
                    sku = row("SKU")
                    If String.IsNullOrEmpty(sku) Then
                        Continue For
                    End If
                    If lstSKUError.Contains(sku) Then
                        Continue For
                    End If
                    lstSKUError.Add(sku)
                    If String.IsNullOrEmpty(SKUFlammable) Then
                        SKUFlammable = sku
                    Else
                        SKUFlammable = SKUFlammable & "," & sku
                    End If
                Next
            End If
            Return SKUFlammable
        End Function
        Public Shared Function GetMailOrderConfirmSubject(ByVal DB As Database, ByVal orderId As Integer, ByVal prefix As String, ByVal suffix As String) As String
            Try

                Dim dtCartItem As DataTable = DB.GetDataTable("SELECT cartitemid,ItemName FROM StoreCartItem WHERE ( [type] = 'item' or [type] = 'BuyPoint' ) and OrderId=" & orderId & " order by IsFreeSample, storecartitem.mixmatchid, isfreeitem, cartitemid")
                If dtCartItem Is Nothing Then
                    Return String.Empty
                End If
                If dtCartItem.Rows.Count < 1 Then
                    Return String.Empty
                End If
                Dim itemName As String = dtCartItem.Rows(0)("ItemName")
                If String.IsNullOrEmpty(itemName) Then
                    Return String.Empty
                End If
                Dim maximumWord As Integer = 78
                If dtCartItem.Rows.Count = 1 Then
                    itemName = StringCut(itemName, 0, maximumWord - prefix.Length - 1 - 3)
                    Return prefix & " " & itemName
                End If
                suffix = String.Format(suffix, dtCartItem.Rows.Count - 1)
                itemName = StringCut(itemName, 0, maximumWord - prefix.Length - 2 - suffix.Length - 3)
                Return prefix & " " & itemName & " " & suffix
            Catch ex As Exception

            End Try
            Return String.Empty
        End Function
        Public Shared Function IsPOBoxAddress(ByVal address As String)
            If String.IsNullOrEmpty(address) Then
                Return False
            End If
            Dim POBoxExpression As String = "\b[P|p]*(OST|ost)*\.*\s*[O|o|0]*(ffice|FFICE)*\.*\s*[B|b][O|o|0][X|x]\b"
            ''POBoxExpression = "\b((P\.O\.)|(P\. O\.)|(PO BOX)|(POBOX)|(BOX)|(POB)|(P O BOX))\b"
            If Regex.IsMatch(address, POBoxExpression) Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsOrderShippingPOBoxAddress(ByVal o As StoreOrderRow)
            Dim country As String = o.ShipToCountry
            If String.IsNullOrEmpty(country) Then
                country = o.BillToCountry
            End If
            If CheckShippingSpecialUS(o) Then
                Return False
            End If
            If country <> "US" Then
                Return False
            End If
            If o.CarrierType = Utility.Common.TruckShippingId Or o.CarrierType = Utility.Common.PickupShippingId Then
                Return False
            End If
            Dim result As Boolean = False
            Dim shippingAddress1 As String = String.Empty
            Dim shippingAddress2 As String = String.Empty
            If o.IsSameAddress Then
                shippingAddress1 = o.BillToAddress
                shippingAddress2 = o.BillToAddress2
            Else
                shippingAddress1 = o.ShipToAddress
                shippingAddress2 = o.ShipToAddress2
            End If
            If Utility.Common.IsPOBoxAddress(shippingAddress1) Then
                Return True
            End If
            Return Utility.Common.IsPOBoxAddress(shippingAddress2)
        End Function

        Public Shared Function IsRequireLogin(ByVal page As String) As Boolean

            Dim arrPageRequireLogin As New List(Of String)
            arrPageRequireLogin.Add("/members/address.aspx")
            arrPageRequireLogin.Add("/members/account.aspx")
            For Each url As String In arrPageRequireLogin
                If page = url Then
                    Return True
                End If
            Next
            Return False
        End Function


        Public Shared Function IsBannerWidthValid(ByVal ful As Controls.FileUpload, ByVal width As Integer) As Boolean
            Try
                Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ful.MyFile.InputStream)
                If (img.PhysicalDimension.Width > width) Then
                    Return False
                End If
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function CheckItemOPI(ByVal objItem As StoreItemRow) As Boolean
            ''check OPI
            Dim currentMemberId As Integer = GetCurrentMemberId()
            Dim result As Boolean = False
            If currentMemberId > 0 Then
                'Kiem tra xem item co thuoc Brand ko duoc phep ban
                Dim sie As New StoreItemEnable()
                Dim dt As DataTable = sie.ListBrands(currentMemberId)
                If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 Then
                    Dim brands As String = dt.Rows(0)("Brands").ToString()
                    Dim memberBrands As String = dt.Rows(0)("MemberBrands").ToString()
                    If memberBrands.Contains("," & objItem.BrandId & ",") Then
                        result = False
                    Else
                        If brands.Contains("," & objItem.BrandId & ",") Then
                            result = True
                        Else
                            result = False
                        End If
                    End If
                    dt.Dispose()
                End If
                'End
            End If
            Return result
        End Function
        Public Shared Function IsCreditCardNumberValid(ByVal ccnum As String) As Boolean
            Dim iChkSum = 0
            ccnum = Regex.Replace(ccnum, "\D", "", RegexOptions.IgnoreCase)

            'Check for correct card number length
            If ccnum.Length < 13 Then Return False

            If ccnum(0) = "4" Then
                'VISA
                If (ccnum.Length <> 13 And ccnum.Length <> 16) Then Return False
            ElseIf ccnum(0) = "3" Then
                'AmEx
                If ccnum.Length <> 15 Then Return False
            ElseIf ccnum(0) = "6" Then
                'Discover
                If ccnum.Length <> 16 Then Return False
            ElseIf ccnum(0) = "5" Then
                'MasterCard
                If ccnum.Length <> 16 Then Return False
            End If

            'Make an array and fill it with the individual digits of the cc number
            Dim ccnumchk() As Integer = New Integer(ccnum.Length) {}
            Dim iLoop As Integer
            For iLoop = 0 To ccnum.Length - 1
                ccnumchk(ccnum.Length - 1 - iLoop) = Int32.Parse(ccnum.Substring(iLoop, 1))
            Next

            'Perform the mathematical method (some base 10 stuff) to
            'convert the number to a two digit number
            For iLoop = 0 To ccnum.Length - 1
                'If splits an even number
                If iLoop Mod 2 <> 0 Then
                    ccnumchk(iLoop) = ccnumchk(iLoop) * 2
                    If ccnumchk(iLoop) >= 10 Then ccnumchk(iLoop) = ccnumchk(iLoop) - 9
                End If

                'Switch ccnumchk[splits] to a number
                ccnumchk(iLoop) = ccnumchk(iLoop) + 1
                ccnumchk(iLoop) = ccnumchk(iLoop) - 1

                iChkSum = iChkSum + ccnumchk(iLoop)
            Next

            'If iChkSum Mod 10 <> 0 Then Return False 'The result isn't base 10

            Return True
        End Function
        Public Shared Sub CheckPhoneInternational(ByRef sender As System.Web.UI.WebControls.CustomValidator, ByRef e As ServerValidateEventArgs)
            Dim resultCheck As Integer = Utility.Common.CheckPhoneInternational(e.Value)
            If (resultCheck = 1) Then
                e.IsValid = True
            Else
                e.IsValid = False
                If resultCheck = 0 Then
                    sender.ErrorMessage = "Phone number is not valid."
                Else
                    sender.ErrorMessage = "Phone number must be at least 10 digit."
                End If
            End If

        End Sub

        Public Shared Function CheckFaxInternational(ByVal FAX As String) As Boolean
            ''result: 1 valid, 0 invalid, -1 :  least 10 digit.
            Dim regex As Regex = New Regex(Utility.ConfigData.PhoneOutUSPattern)
            Dim match As Match = regex.Match(FAX)
            If (match.Success) Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function CheckPhoneInternational(ByVal phone As String) As Integer
            ''result: 1 valid, 0 invalid, -1 :  least 10 digit.
            Dim regex As Regex = New Regex(Utility.ConfigData.PhoneOutUSPattern)
            Dim match As Match = regex.Match(phone)
            If (match.Success) Then
                Dim number As String = phone.Trim().Replace(" ", "")
                number = number.Replace(".", "")
                number = number.Replace("(", "")
                number = number.Replace(")", "")
                number = number.Replace("-", "")
                If IsNumeric(number) Then
                    If number.Length >= 10 Then
                        Return 1
                    End If
                Else
                    Return -1
                End If
            End If
            Return 0
        End Function
        Public Shared Function CheckUSPhoneValid(ByVal phone As String) As Boolean
            Dim result As Boolean = False
            Dim array() As String = phone.Trim().Split(" ")
            Dim phone1 As String = String.Empty
            Dim phone2 As String = String.Empty
            Dim phone3 As String = String.Empty
            Dim phoneExt As String = String.Empty
            Try
                Dim tmp As String = array(0)
                If Not String.IsNullOrEmpty(tmp) Then
                    Dim lstPhone() As String = tmp.Split("-")
                    phone1 = lstPhone(0)
                    phone2 = lstPhone(1)
                    phone3 = lstPhone(2)
                End If
                If array.Length > 1 Then
                    phoneExt = array(1)
                End If

                If (String.IsNullOrEmpty(phone1) And String.IsNullOrEmpty(phone2) And String.IsNullOrEmpty(phone3)) Then
                    Return False
                End If
                result = Utility.Common.CheckUSPhoneValid(phone1, phone2, phone3)
                If (result) Then
                    result = CheckUSPhoneExt(phoneExt.Trim())
                End If

            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Sub GetUSPhoneValueFromUI(ByVal value As String, ByRef returnPhone As String, ByRef returnPhoneExt As String)
            Dim array() As String = value.Trim().Split(" ")
            Try
                returnPhone = array(0)
                If array.Length > 1 Then
                    returnPhoneExt = array(1).Trim()
                Else
                    returnPhoneExt = ""
                End If
            Catch ex As Exception
            End Try
        End Sub
        Public Shared Function CheckUSPhoneExt(ByVal ext As String) As Boolean
            If (String.IsNullOrEmpty(ext)) Then
                Return True
            End If
            If (ext.Length > 4) Then
                Return False
            Else

            End If
            Dim nPhone As Integer = -1
            Try
                nPhone = CInt(ext)
            Catch ex As Exception
                nPhone = -1
            End Try
            If (nPhone < 0) Then
                Return False
            Else
                Return True
            End If
        End Function
        Public Shared Sub ResetOrderShippingMethod(ByVal method As Integer, ByVal orderId As Integer, ByVal DB As Database)
            Dim sqlShippingMthod As String = String.Empty
            If method = Utility.Common.PickupShippingId.ToString() Then
                sqlShippingMthod = "update storecartitem set carriertype = " & method & " where type = 'item' and orderid = " & orderId
            ElseIf method = Utility.Common.TruckShippingId.ToString() Then
                sqlShippingMthod = "update storecartitem set carriertype=" & Utility.Common.TruckShippingId & " where type = 'item' and orderid = " & orderId
            Else
                'Khoa tat: Hazardous Material Fee
                'sqlShippingMthod = "update storecartitem set carriertype = case when isoversize = 1 and " & method & " not in (" & Utility.Common.PickupShippingId & ") then " & Utility.Common.TruckShippingId & " else case when ishazmat = 1 then case when " & method & " in " & DB.NumberMultiple(Utility.Common.NonExpeditedShippingIds) & " then " & method & "	else carriertype end else " & method & " end end where type = 'item' and orderid = " & orderId
                sqlShippingMthod = "update storecartitem set carriertype = case when isoversize = 1 and " & method & " not in (" & Utility.Common.PickupShippingId & ") then " & Utility.Common.TruckShippingId & " else " & method & " end where type = 'item' and orderid = " & orderId
            End If
            DB.ExecuteSQL(sqlShippingMthod)
        End Sub
        Public Shared Function IsEbayOrder(ByVal orderNo As String) As Boolean
            Dim prefix As String = String.Empty
            Try
                prefix = orderNo.Substring(0, 1)
            Catch ex As Exception

            End Try
            If (prefix = "E") Then
                Return True
            End If
            Return False
        End Function
#Region "Currency"
        Public Shared Function ViewCurrency(ByVal value As Double) As String
            Return Microsoft.VisualBasic.Strings.FormatCurrency(value, 2)
        End Function
        Public Shared Function RoundCurrency(ByVal value As Double) As Double
            Return Microsoft.VisualBasic.FormatNumber(value, 2)
        End Function
#End Region

#Region "Cart Without Login"
        Public Shared ReadOnly Property CartCookie() As String
            Get
                Return "CartCookie"
            End Get

        End Property
        Public Shared ReadOnly Property CartCookieExpire() As Integer
            Get
                Dim result As Integer = SysParam.GetValue("CartCookieExpire")
                If result < 1 Then
                    result = 7
                End If
                Return result
            End Get

        End Property
        Public Shared Function ShowMemberLoginName(ByVal m_LoggedInName As String) As String
            If String.IsNullOrEmpty(m_LoggedInName) Then
                Return String.Empty
            End If
            If m_LoggedInName.Split(" ")(0).Length > 12 Then
                m_LoggedInName = Left(m_LoggedInName.Split(" ")(0).ToString(), 12) & "..."
            Else
                m_LoggedInName = m_LoggedInName.Split(" ")(0).ToString()
            End If
            m_LoggedInName = m_LoggedInName.Replace("....", "...")
            Return m_LoggedInName
        End Function
        Public Shared Function GetCartCookieValue() As String
            Dim cookieValue As String = Utility.CookieUtil.GetTripleDESEncryptedCookieValue(CartCookie)
            Return cookieValue
        End Function
        Public Shared Function GetOrderIdFromCartCookie() As Integer
            Dim cookieValue As String = Utility.CookieUtil.GetTripleDESEncryptedCookieValue(CartCookie)
            Return GetOrderIdFromCartCookie(cookieValue)
        End Function
        Public Shared Function GetOrderIdFromCartCookie(ByVal cookieValue As String) As Integer
            If Not String.IsNullOrEmpty(cookieValue) Then
                Dim indexChar As Integer = cookieValue.IndexOf("-")
                If indexChar > 0 Then
                    Dim result As String = cookieValue.Substring(indexChar + 1, cookieValue.Length - indexChar - 1)
                    If IsNumeric(result) Then
                        Return CInt(result)
                    End If
                End If
            End If
            Return 0
        End Function

        Public Shared Function GetMemberIdFromCartCookie() As Integer
            Dim cookieValue As String = Utility.CookieUtil.GetTripleDESEncryptedCookieValue(CartCookie)
            Return GetMemberIdFromCartCookie(cookieValue)
        End Function
        Public Shared Function GetCurrentMemberId() As Integer
            Dim memberId As Integer = 0
            If System.Web.HttpContext.Current.Session("MemberId") Is Nothing Then
                memberId = GetMemberIdFromCartCookie()
                If memberId > 0 Then
                    Web.HttpContext.Current.Session("MemberId") = memberId
                End If
            Else
                memberId = Web.HttpContext.Current.Session("MemberId")
            End If

            Return memberId
        End Function
        Public Shared Function GetCurrentOrderId() As Integer
            Dim orderId As Integer = 0
            If System.Web.HttpContext.Current.Session("OrderId") Is Nothing Then
                orderId = GetOrderIdFromCartCookie()
                If orderId > 0 Then
                    Web.HttpContext.Current.Session("OrderId") = orderId
                End If
            Else
                orderId = System.Web.HttpContext.Current.Session("OrderId")
            End If

            Return orderId
        End Function
        Public Shared Function GetMemberIdFromCartCookie(ByVal cookieValue As String) As Integer
            If Not String.IsNullOrEmpty(cookieValue) Then
                Dim indexChar As Integer = cookieValue.IndexOf("-")
                If indexChar > 0 Then
                    Dim result As String = cookieValue.Substring(0, indexChar)
                    If IsNumeric(result) Then
                        If (MemberRow.CheckIdExists(CInt(result))) Then
                            Return CInt(result)
                        End If
                        Return 0
                    End If
                End If
            End If
            Return 0
        End Function
        Public Shared Sub SetCartCookieLogin(ByVal memberId As Integer, ByVal orderId As Integer)
            Dim dateExpire As DateTime = DateTime.Now
            dateExpire = dateExpire.AddDays(CartCookieExpire)
            Utility.CookieUtil.SetTripleDESEncryptedCookie(CartCookie, memberId & "-" & orderId, dateExpire)
        End Sub
        Public Shared Sub SetOrderToCartCookie(ByVal orderId As Integer)
            Dim cookieValue As String = Utility.CookieUtil.GetTripleDESEncryptedCookieValue(CartCookie)
            If String.IsNullOrEmpty(cookieValue) Then
                SetCartCookieLogin(0, orderId)
            Else
                Dim cookie As System.Web.HttpCookie = System.Web.HttpContext.Current.Request.Cookies.Get(CartCookie)
                Dim memberID As Integer = GetMemberIdFromCartCookie(cookieValue)
                Dim dateExpire As DateTime = DateTime.Now
                dateExpire = dateExpire.AddDays(CartCookieExpire)
                Utility.CookieUtil.SetTripleDESEncryptedCookie(CartCookie, memberID & "-" & orderId, dateExpire)
            End If
        End Sub
        Public Shared Sub ClearCartCookieAddCart(ByVal DB As Database)
            Dim cookie As System.Web.HttpCookie = System.Web.HttpContext.Current.Request.Cookies.Get(Utility.Common.CartCookie)
            If Not cookie Is Nothing Then
                'Dim cookieOrderId As Integer = GetOrderIdFromCartCookie()
                'If cookieOrderId > 0 Then
                '    DB.ExecuteSQL("exec sp_StoreOrder_DeleteCartCookie " & cookieOrderId)
                'End If
                cookie.Expires = Now.AddDays(-1)
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie)
            End If

        End Sub
        Public Shared Sub DeleteCachePopupCart(ByVal orderId As Integer)
            Utility.CacheUtils.RemoveCache("popupCart_" & orderId)

        End Sub

#End Region

        Public Shared Sub AddPointKeyword(ByVal itemId As Integer, ByVal action As Integer, ByVal point As Integer)
            Try
                Dim keywordSearchId As Integer = 0
                If Not System.Web.HttpContext.Current.Session Is Nothing AndAlso Not System.Web.HttpContext.Current.Session("KeywordSearchId") Then
                    keywordSearchId = CInt(System.Web.HttpContext.Current.Session("KeywordSearchId"))
                End If
                If (keywordSearchId < 1) Then
                    Exit Sub
                End If

                ''adpoint for this action 
                Dim keywordAction As New KeywordActionRow
                keywordAction.ItemId = itemId
                keywordAction.KeywordSearchId = keywordSearchId
                keywordAction.Action = action
                keywordAction.Point = point
                KeywordActionRow.Insert(keywordAction)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "AddPointKeyword", itemId.ToString())
            End Try

        End Sub

    End Class


End Namespace
