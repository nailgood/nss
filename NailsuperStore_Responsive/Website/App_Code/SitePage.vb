Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Diagnostics
Imports System.Text
Imports System.Data.SqlClient
Imports MasterPages
Imports Utility
Imports DataLayer
Imports System.Configuration.ConfigurationManager
Imports System.Xml
Imports System.Collections.Generic
Imports Utility.Common
Imports ShippingValidator
Imports System.Xml.Linq
Imports System.Linq

Namespace Components

    Public Class SitePage
        Inherits BasePage

        Protected GlobalRefererName As String
        Protected GlobalSecureName As String
        Protected LoggedInMemberId As Integer
        Public LoggedInEmail As String
        Protected LoggedInFirstName As String
        Protected LoggedInLastName As String
        Public LoggedInLanguageCode As String
        Protected ConnectionString As String
        Public IsSecurePage As Boolean = False
        Public PointAvailable As Integer = 0
        Public LevelPoint As String = String.Empty
        Private m_Cart As ShoppingCart
        Public Shared cssVersion As String = String.Empty


        Public Shared Function Trim(ByVal str As String) As String
            If Not String.IsNullOrEmpty(str) Then
                Return str.Trim()
            Else
                Return String.Empty
            End If
        End Function

        Public Sub SaveAuthCookie(ByVal Username As String, ByVal bPermanent As Boolean)
            Dim ticket As New FormsAuthenticationTicket(1, Username, Now, Now.AddHours(1), bPermanent, "Everyone")
            Dim encTicket As String = FormsAuthentication.Encrypt(ticket)
            Dim cookie As HttpCookie = New HttpCookie(".CommunityServer", encTicket)
            HttpContext.Current.Response.Cookies.Set(cookie)
        End Sub

        Public Sub ClearAuthCookie()
            Dim cookie As HttpCookie = New HttpCookie(".CommunityServer", "")
            HttpContext.Current.Response.Cookies.Set(cookie)
        End Sub

        Public Sub GoMsg(ByVal msg As String)
            Session("Msg") = msg
            Response.Redirect("/msg.aspx")
        End Sub
        Public ReadOnly Property WebRoot() As String
            Get
                Dim result As String
                Dim request As System.Web.HttpRequest = HttpContext.Current.Request
                If Not request Is Nothing Then
                    result = request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath + request.Url.Query, String.Empty) + request.ApplicationPath
                    result = result.Substring(0, result.Length - 1)
                    Return result
                End If
                Return String.Empty
            End Get
        End Property

        Public Function CheckCaptcha(ByVal input As String) As Boolean
            If Not String.IsNullOrEmpty(HttpContext.Current.Session("Captcha")) Then
                Return (HttpContext.Current.Session("Captcha").ToString() = input)
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Tra ve Qty luon luon > 0
        ''' </summary>
        Public Shared Function QtyInput(ByVal Qty As String) As Integer
            Dim iQty As Integer = 1
            Try
                iQty = Convert.ToInt32(Qty)
            Catch ex As Exception

            End Try

            Return iQty
        End Function

        ''' <summary>
        ''' Tra ve Qty >= 0
        ''' </summary>
        Public Shared Function QtyInput0(ByVal Qty As String) As Integer
            Dim iQty As Integer = 0
            Try
                iQty = Convert.ToInt32(Qty)
            Catch ex As Exception

            End Try

            Return iQty
        End Function

        Public Shared Function ShowMeasurement(ByVal desc As String, ByVal Measure As Integer) As String
            Dim str As String = ""
            If desc Is Nothing Then Return str

            If desc.Contains("oz") Then
                'Remove text unused
                str = desc.Replace("oz", "")
                str = str.Replace("-", "")
                str = str.Trim()

                Try
                    'Check thap phan
                    If str.Contains("/") Then
                        Dim arr As String() = str.Split("/")
                        Dim a As Double = CDbl(arr(0))
                        Dim b As Double = CDbl(arr(1))
                        Dim c As Double = a / b
                        str = c.ToString()
                    End If

                    If Measure = 0 Then
                        str = Math.Round(Convert.ToDouble(str) * 29.574, 2) & " ml"
                    Else
                        str = Math.Round(Convert.ToDouble(str) * 28.349, 2) & " gr"
                    End If
                Catch ex As Exception
                    str = ""
                End Try
            ElseIf desc.ToLower().Contains("gal") Then
                Dim sl As Double = 1
                Try
                    sl = CDbl(desc.Trim().Split(" ")(0))
                Catch ex As Exception

                End Try

                str = Math.Round(Convert.ToDouble(sl) * 3.79, 2) & "L"
            End If

            Return str
        End Function

        Public Property Cart(ByVal Initialize As Boolean) As ShoppingCart
            Get
                If m_Cart Is Nothing Then
                    Dim memberId As Integer = Utility.Common.GetCurrentMemberId
                    Dim orderId As Integer = Utility.Common.GetCurrentOrderId

                    If orderId < 1 AndAlso memberId > 0 Then
                        orderId = DB.ExecuteScalar("Select LastOrderId from Member where MemberId=" & memberId & " and LastOrderId in(Select OrderId from StoreOrder where MemberId=" & memberId & " and OrderNo is null and ProcessDate is null)")
                        If orderId > 0 Then
                            Utility.Common.SetCartCookieLogin(memberId, orderId)
                        End If
                    End If

                    If (Initialize And orderId > 0) Then
                        Session("OrderId") = orderId
                        m_Cart = New ShoppingCart(DB, orderId)
                    End If
                End If
                Return m_Cart
            End Get
            Set(ByVal value As ShoppingCart)
                m_Cart = value
            End Set
        End Property

        Public Property Cart() As ShoppingCart
            Get
                Return Cart(True)
            End Get
            Set(ByVal value As ShoppingCart)
                m_Cart = value
            End Set
        End Property

        Public ReadOnly Property HasOrder() As Boolean
            Get
                If Not m_Cart Is Nothing AndAlso m_Cart.Order IsNot Nothing Then
                    Return True
                End If
                Return False
            End Get
        End Property



        Public Shared Function IsLocal() As Boolean
            Dim domain(3) As String
            domain(0) = "localhost"
            domain(1) = "127.0.0.1"
            domain(2) = "192.168.41"

            Try
                For Each s In domain
                    If HttpContext.Current.Request.ServerVariables("server_name").Contains(s) Then
                        Return True
                    End If
                Next
            Catch ex As Exception

            End Try

            Return False
        End Function

        Public Shared Function IsWebTest() As Boolean
            Dim domain(7) As String
            domain(0) = "localhost"
            domain(1) = "responsive.nss.com"
            domain(2) = "192.168.41"
            domain(3) = "test.nss.com"
            domain(4) = "old.nss.com"
            domain(5) = "new.nss.com"
            domain(6) = "mtest.nss.com"
            domain(7) = "cdn.nss.com"

            Try
                For Each s In domain
                    If HttpContext.Current.Request.ServerVariables("server_name").Contains(s) Then
                        Return True
                    End If
                Next
            Catch ex As Exception

            End Try

            Return False
        End Function

        Public Sub CheckIsRequireLogin()
            Dim url As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
            url = url.Replace(WebRoot, "")
            If url = "/members/login.aspx" Then
                Exit Sub
            End If
            If (url.Contains("includes/ajax.aspx") Or url.Contains("/404.aspx")) Then
                Exit Sub
            End If
            If (IsRequireLogin(url)) Then
                If Not HasAccess() Then
                    Me.Response.Redirect("/members/login.aspx")
                End If
            End If
        End Sub

        Public Sub SetPageMetaSocialNetwork(ByVal page As System.Web.UI.Page, ByVal objMetaTag As MetaTag)
            page.Header.Title = objMetaTag.PageTitle
            '''''''set MetaTag danh cho mang xa hoi khi share
            Dim ltrMetaSocial As Literal = Nothing
            Dim master As System.Web.UI.MasterPage = page.Master
            If Not master Is Nothing Then
                ltrMetaSocial = CType(master.FindControl("ltrMetaSocialNetwork"), Literal)
            Else
                ltrMetaSocial = CType(page.FindControl("ltrMetaSocialNetwork"), Literal)
            End If

            If Not ltrMetaSocial Is Nothing Then
                If String.IsNullOrEmpty(objMetaTag.ShareDesc) Then
                    objMetaTag.ShareDesc = objMetaTag.MetaDescription
                End If
                Dim imageURL As String = GlobalSecureName & objMetaTag.ImagePath & objMetaTag.ImageName
                If Not Core.FileExists(Server.MapPath(objMetaTag.ImagePath & objMetaTag.ImageName)) Then
                    ''GlobalSecureName & MediaUrl & "/items/" & objItem.Image
                    imageURL = Utility.ConfigData.CDNMediaPath & "/includes/theme/images/logo.png"
                    objMetaTag.ImgWidth = 136
                    objMetaTag.ImgHeight = 97
                End If





                Dim sMetaSocial As String = String.Empty ''"<title>" & objMetaTag.PageTitle & "</title>" & Environment.NewLine
                '' sMetaSocial = "<meta name=""author"" content=""" & Utility.ConfigData.AuthorMetaTag & """ />" & Environment.NewLine

                If Not String.IsNullOrEmpty(objMetaTag.MetaDescription) Then
                    sMetaSocial = sMetaSocial & "<meta name=""description"" content=""" & objMetaTag.MetaDescription & """ />" & Environment.NewLine
                End If

                If Not String.IsNullOrEmpty(objMetaTag.MetaKeywords) Then
                    sMetaSocial = sMetaSocial & "<meta name=""keywords"" content=""" & objMetaTag.MetaKeywords & """ />" & Environment.NewLine
                End If

                sMetaSocial = sMetaSocial & "<meta property=""fb:admin"" content=""nailsuperstore""/>" & Environment.NewLine
                'sMetaSocial = sMetaSocial & "<meta name=""twitter:site"" content=""@nailsuperstore""/>" & Environment.NewLine
                'sMetaSocial = sMetaSocial & "<meta name=""twitter:title"" content=""" & shareTitle & """/>" & Environment.NewLine
                'sMetaSocial = sMetaSocial & "<meta name=""twitter:description"" content=""" & shareDesc & """/>" & Environment.NewLine
                'sMetaSocial = sMetaSocial & "<meta name=""twitter:image"" content=""" & imageURL & """/>" & Environment.NewLine
                'If imgWidth > 0 Then
                '    sMetaSocial = sMetaSocial & "<meta name=""twitter:image:width"" content=""" & imgWidth & """ />" & Environment.NewLine
                'End If
                'If imgHeight > 0 Then
                '    sMetaSocial = sMetaSocial & "<meta name=""twitter:image:height"" content=""" & imgHeight & """ />" & Environment.NewLine
                'End If
                If objMetaTag.TypeShare = "item" Then
                    If Not String.IsNullOrEmpty(objMetaTag.Price) Then
                        sMetaSocial = sMetaSocial & "<meta name=""twitter:card"" content=""product""/>" & Environment.NewLine
                        sMetaSocial = sMetaSocial & "<meta name=""twitter:data1"" content=""" & Utility.Common.ViewCurrency(objMetaTag.Price) & """/>" & Environment.NewLine
                        sMetaSocial = sMetaSocial & "<meta name=""twitter:label1"" content=""Price""/>" & Environment.NewLine

                        sMetaSocial = sMetaSocial & "<meta property=""og:price:amount"" content=""" & String.Format("{0:0.00}", Utility.Common.RoundCurrency(objMetaTag.Price)) & """ />" & Environment.NewLine
                        sMetaSocial = sMetaSocial & "<meta property=""og:price:currency"" content=""USD"" />" & Environment.NewLine
                    End If
                    sMetaSocial = sMetaSocial & "<meta property=""og:type"" content=""product""/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta name=""twitter:data2"" content=""Yes""/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta name=""twitter:label2"" content=""In Stock""/>" & Environment.NewLine
                ElseIf objMetaTag.TypeShare = "video" Then
                    sMetaSocial = sMetaSocial & "<meta name=""twitter:card"" content=""player""/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta name=""twitter:player"" content=""" & objMetaTag.EmbedUrl & """/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta name=""twitter:player:width"" content=""" & objMetaTag.ImgWidth & """/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta name=""twitter:player:height"" content=""" & objMetaTag.ImgHeight & """/>" & Environment.NewLine
                    If Not objMetaTag.VideoFile.ToLower().Contains("youtube.com/") AndAlso objMetaTag.VideoFile.ToLower().Contains(".mp4") Then
                        sMetaSocial = sMetaSocial & "<meta name=""twitter:player:stream"" content=""" & objMetaTag.VideoFile & """/>" & Environment.NewLine
                        sMetaSocial = sMetaSocial & "<meta name=""twitter:player:stream:content_type"" content=""video/mp4""/>" & Environment.NewLine
                    End If

                    sMetaSocial = sMetaSocial & "<meta property=""og:type"" content=""video""/>" & Environment.NewLine
                    If Not objMetaTag.VideoFile.ToLower().Contains("youtube.com/") Then
                        sMetaSocial = sMetaSocial & "<meta property=""og:video"" content=""" & objMetaTag.VideoFile & """/>" & Environment.NewLine
                    Else
                        sMetaSocial = sMetaSocial & "<meta property=""og:video"" content=""" & objMetaTag.EmbedUrl & """/>" & Environment.NewLine
                    End If
                    sMetaSocial = sMetaSocial & "<meta property=""og:video:width"" content=""" & objMetaTag.ImgWidth & """ />" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta property=""og:video:height"" content=""" & objMetaTag.ImgHeight & """ />" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta property=""og:video:type"" content=""application/x-shockwave-flash""/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<link rel=""canonical"" href=""" & objMetaTag.ShareURL & """/>"
                ElseIf objMetaTag.TypeShare = "news" Or objMetaTag.TypeShare = "category" Then
                    sMetaSocial = sMetaSocial & "<meta property=""og:type"" content=""article""/>" & Environment.NewLine
                End If
                If Not String.IsNullOrEmpty(objMetaTag.TypeShare) Then '' detail
                    sMetaSocial = sMetaSocial & "<meta property=""og:site_name"" content=""The Nail Superstore""/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta property=""og:url"" itemprop='url' content=""" & objMetaTag.ShareURL & """/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta property=""og:image"" itemprop='thumbnailUrl' content=""" & imageURL & """/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta property=""og:title"" itemprop='headline' content=""" & objMetaTag.ShareTitle & """/>" & Environment.NewLine
                    sMetaSocial = sMetaSocial & "<meta property=""og:description"" itemprop='description' content=""" & objMetaTag.ShareDesc & """/>" & Environment.NewLine

                End If
                If Not String.IsNullOrEmpty(objMetaTag.Canonical) Then
                    sMetaSocial = sMetaSocial & "<link rel=""canonical"" href=""" & objMetaTag.Canonical & """ />" & Environment.NewLine
                End If
                ltrMetaSocial.Text = sMetaSocial
            Else

                page.MetaDescription = objMetaTag.MetaDescription
                page.MetaKeywords = objMetaTag.MetaKeywords
            End If
        End Sub
        Public Sub SetIndexFollow(row As ContentToolPageRow, ByRef lt As Literal, ByVal isNo As Boolean)
            Try
                Dim strMetatag As String = Resources.Msg.IndexFollow
                Dim strIndex As String = String.Empty
                Dim strFollow As String = String.Empty
                Dim isIndex, isFollow As Boolean
                If Not row Is Nothing Then
                    isIndex = row.IsIndexed
                    isFollow = row.IsFollowed
                Else
                    isIndex = True
                    isFollow = True
                End If
                If Not isIndex Or isNo Then
                    strIndex = "NOINDEX"
                End If
                If Not isFollow Or isNo Then
                    strFollow = "NOFOLLOW"
                    If Not String.IsNullOrEmpty(strIndex) Then
                        strIndex &= ", "
                    End If
                End If
                If Not String.IsNullOrEmpty(strIndex) Or Not String.IsNullOrEmpty(strFollow) Then
                    lt.Text = String.Format(strMetatag, strIndex, strFollow)
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "SetIndexFollow - " & Now.ToString(), ex.ToString())
            End Try

        End Sub
        Protected Overrides Sub OnInit(ByVal e As EventArgs)

            If (Page404ImageLoad()) Then
                Me.Page.Controls.Clear()
                Exit Sub
            End If
            MyBase.OnInit(e)

            Dim strUrl As String = Request.RawUrl.ToString().ToLower()
            'If Not strUrl.Contains("/store/review/order-write.aspx") And Not strUrl.Contains("/store/OrderReview.aspx") And Not strUrl.Contains("/store/cart.aspx") And Not strUrl.Contains("/store/billing.aspx") And Not strUrl.Contains("/store/billingint.aspx") And Not strUrl.Contains("/store/revise-cart.aspx") And Not strUrl.Contains("/store/payment.aspx") And Not strUrl.Contains("/addressbook/edit.aspx") And Not strUrl.Contains("/store/ipn.aspx") Then
            If strUrl.Contains("/contact/") Or strUrl.Contains("/service/") Or strUrl.Contains("/services/") Then
                If Request.RawUrl.ToString().Any(Function(c) Char.IsUpper(c)) Then
                    Common.Redirect301(strUrl.ToLower())
                End If
            End If

            'Kiem tra nguoi la vao test site
            'If Request.ServerVariables("server_name").Contains("new.nss.com") Or Request.ServerVariables("server_name").Contains("test.nss.com") Or Request.ServerVariables("server_name").Contains("responsive.nss.com") Then
            '    Dim strIP As String = "74.222.39.162;74.222.39.163;74.222.39.164;74.222.39.165;74.222.39.166;75.144.111.78;198.0.241.41;198.0.241.42;198.0.241.43;198.0.241.44;198.0.241.45;113.161.73.102;"
            '    If Not strIP.Contains(Request.ServerVariables("REMOTE_ADDR")) Then
            '        If Session("AdminId") Is Nothing Then
            '            Response.Redirect("/admin/login.aspx")
            '        End If
            '    End If
            'End If

            Dim bHttps As Boolean = Convert.ToBoolean(CInt(SysParam.GetValue("HTTPS")))
            ChangeWWWnonWWW(bHttps)


            DisposeSession()
            If HttpContext.Current.Session("ReferralURL") Is Nothing Then
                HttpContext.Current.Session("ReferralURL") = Request.ServerVariables("HTTP_REFERER")
            End If

            If HttpContext.Current.Session("ref") Is Nothing AndAlso HttpContext.Current.Request("ref") <> String.Empty Then
                HttpContext.Current.Session("ref") = HttpContext.Current.Request("ref")
                DataLayer.ReferralRow.AddClick(DB, HttpContext.Current.Request("ref"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"))
                If HasOrder Then
                    Cart.Order.PromotionCode = StorePromotionRow.GetRow(DB, ReferralRow.GetRow(DB, DB.ExecuteScalar("select top 1 referralid from referral where code = " & DB.Quote(HttpContext.Current.Session("ref")))).PromotionId).PromotionCode
                    Cart.Order.ReferralCode = HttpContext.Current.Session("ref")
                    Cart.Order.Update()
                    Cart.RecalculateOrderDetail("Sitepage.OnInit")
                    HttpContext.Current.Session("ref") = Nothing
                End If
            ElseIf Not HttpContext.Current.Session("ref") Is Nothing Then
                If HasOrder Then
                    Cart.Order.PromotionCode = StorePromotionRow.GetRow(DB, ReferralRow.GetRow(DB, DB.ExecuteScalar("select top 1 referralid from referral where code = " & DB.Quote(HttpContext.Current.Session("ref")))).PromotionId).PromotionCode
                    Cart.Order.ReferralCode = HttpContext.Current.Session("ref")
                    Cart.Order.Update()
                    Cart.RecalculateOrderDetail("Sitepage.OnInit2")
                    HttpContext.Current.Session("ref") = Nothing
                End If
            End If

            GlobalRefererName = ConfigurationManager.AppSettings("GlobalRefererName")
            GlobalSecureName = ConfigurationManager.AppSettings("GlobalSecureName")
            ConnectionString = System.Configuration.ConfigurationManager.AppSettings("ConnectionString")

            If Not Request.Path.Contains("msg.aspx") Then
                Session.Remove("Msg")
            End If
        End Sub

        Public Shared Function GetSessionList() As String
            Return ShoppingCart.GetSessionList()
        End Function

        Private Sub ValidateSesssionSearchKeyword()
            Dim fileURL As String = HttpContext.Current.Request.ServerVariables("URL").ToLower()
            If fileURL.Contains("/includes/ajax.aspx") Then
                Exit Sub
            End If
            If Not Session("KeywordSearchId") Is Nothing Then
                If fileURL.Contains("members/login.aspx") Or fileURL.Contains("store/item.aspx") Then
                    Exit Sub
                End If
                If fileURL.Contains("store/search-result.aspx") Then
                    Dim kw As String = GetQueryString("kw")
                    If (String.IsNullOrEmpty(kw)) Then
                        Session.Remove("KeywordSearchId")
                    End If
                    Exit Sub
                ElseIf Request IsNot Nothing AndAlso Request.UrlReferrer IsNot Nothing AndAlso Request.UrlReferrer.OriginalString.Contains("aspx") AndAlso (Not Request.UrlReferrer.OriginalString.Contains("store/search-result.aspx") AndAlso (Not Request.UrlReferrer.OriginalString.Contains("/nail-products/")) AndAlso ((Not Request.UrlReferrer.OriginalString.Contains("/store/cart.aspx")))) Then
                    Session.Remove("KeywordSearchId")
                End If
            End If

        End Sub

        Public Function IsiPhone() As Boolean
            Try
                If (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone")) Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Function IsiPad() As Boolean
            Try
                If (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad")) Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Function IsWindowPhone() As Boolean
            Try
                ''Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; Lumia 525)
                If (HttpContext.Current.Request.UserAgent.Contains("Windows Phone") Or HttpContext.Current.Request.UserAgent.Contains("IEMobile")) Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Private Sub btn_Click(ByVal sender As Object, ByVal e As EventArgs)
            If Session("AdminId") = Nothing Then Exit Sub
            Try
                DB.BeginTransaction()
                SiteTheme.Activate()
                DB.CommitTransaction()
            Catch ex As Exception
                DB.RollbackTransaction()
                AddError(ErrHandler.ErrorText(ex))
            End Try
            Response.Redirect(Request.Url.PathAndQuery)
        End Sub

        Protected Sub SitePageLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            If (Page404ImageLoad()) Then
                Exit Sub
            End If

            Dim bUnCheckLogin As Boolean
            If Request.RawUrl.Contains("login.aspx") Or Request.RawUrl.Contains("register.aspx") Or Request.Url.ToString().Contains("?act=guest") Then
                bUnCheckLogin = True
            End If

            If Not bUnCheckLogin Then
                Dim Principal As SitePrincipal = Nothing

                If (HttpContext.Current.Session("MemberId") = Nothing OrElse HttpContext.Current.Session("Username") = Nothing) AndAlso Not CookieUtil.GetTripleDESEncryptedCookieValue("MemberId") = Nothing Then
                    HttpContext.Current.Session("MemberId") = CookieUtil.GetTripleDESEncryptedCookieValue("MemberId")
                    Dim Member As MemberRow = MemberRow.GetRow(Session("MemberId"))
                    HttpContext.Current.Session("Language") = LanguageCode.GetLanguageCode(Member.Customer.LanguageCode)
                    HttpContext.Current.Session("Username") = Member.Username
                    SaveAuthCookie(Member.Username, False)
                End If

                If Not HttpContext.Current.Session("MemberId") = Nothing Then
                    DB.Open(ConnectionString)
                    Try
                        Principal = New SitePrincipal(DB, CInt(Session("MemberId")))
                    Catch ex As NullReferenceException

                        Components.Email.SendError("ToError500", "Principal Error-" & Now.ToString(), Request.RawUrl & "-MemberId=" & Session("MemberId"))
                        HttpContext.Current.Response.Redirect("/members/logout.aspx?act=Principal")
                    Finally
                        DB.Close()
                    End Try

                    If Principal Is Nothing Then Exit Sub
                    If Not Principal.Member.IsActive AndAlso Principal.Member.GuestStatus = 0 Then
                        HttpContext.Current.Session.Remove("MemberId")
                        HttpContext.Current.Session.Remove("Username")
                        ClearAuthCookie()
                        CookieUtil.SetTripleDESEncryptedCookie("MemberId", Nothing)
                        CookieUtil.SetTripleDESEncryptedCookie("Username", Nothing)
                        Exit Sub
                    End If

                    HttpContext.Current.Session("Username") = Principal.Member.Username

                    LoggedInMemberId = Principal.Member.MemberId
                    LoggedInLanguageCode = Principal.Member.Customer.LanguageCode
                    LoggedInEmail = Principal.Member.Customer.Email
                    LoggedInFirstName = Principal.Member.Customer.Name
                    LoggedInLastName = Principal.Member.Customer.Name
                    LevelPoint = GetLevelPoint(LoggedInMemberId)
                Else
                    HttpContext.Current.Session.Remove("Username")
                End If

                PointAvailable = CashPointRow.GetTotalCashPointByMember(DB, Utility.Common.GetMemberIdFromCartCookie(), Session("OrderId"))
                If (PointAvailable < 0 And (Session("MailPointAvailable") Is Nothing Or Session("MailPointAvailable") = "0")) Then
                    If (Principal.Member.Customer.CustomerPostingGroup <> "WHS") Then
                        Email.SendError("ToErrorCashPoint", "CashPoint Invalid", String.Format("Username: {0}\r\nPoint Available: {1}\r\nMail Point Available: {2},\r\nOrder Id:{3}", Session("Username"), PointAvailable, Session("MailPointAvailable"), Session("OrderId")))
                        Session("MailPointAvailable") = "1"
                    End If
                End If
            End If

            If Not IsPostBack Then
                SetCSS()
                ClearSession()
                SetLastWebsiteURL("")
            End If

            CheckParamToSetNoIndex()
        End Sub

        Public Sub SetLastWebsiteURL(ByVal sUrl As String)
            Dim Url As String = IIf(String.IsNullOrEmpty(sUrl), Request.RawUrl, sUrl)
            Dim PathUrl As String = Request.Path
            If Not Url.Contains("/login.aspx") And Not Url.Contains("/includes/") And Not PathUrl.Contains("404.aspx") And Not PathUrl.Contains("500.aspx") And Not PathUrl.Contains("301.aspx") And Not Url.Contains("/popup/") And Not Url.Contains("/assets/") And Not Url.Contains("/theme/") Then
                Session("LastWebsiteURL") = WebRoot & Url
            End If
        End Sub

        Public Function Page404ImageLoad() As Boolean

            Dim url As String = Request.RawUrl
            If (url.Contains("404.aspx")) Then
                If (url.Contains(".gif") Or url.Contains(".jpg") Or url.Contains(".jpeg") Or url.Contains(".ico") Or url.Contains("/assets/")) Then
                    Return True
                End If
            End If
            Return False
        End Function


        Private Sub SetCSS()
            Dim strUrl As String = Request.Url.ToString().ToLower().Trim()
            Dim strUrlMasterPage As String = String.Empty
            If Not Me.Master Is Nothing Then
                strUrlMasterPage = Me.MasterPageFile.ToLower()
            End If

            Dim strRawUrl As String = Request.RawUrl().ToLower.Trim()
            If strRawUrl = "/" Then
                strRawUrl = "/home.aspx"
            End If
            Try
                'Get from Cache
                Dim lstCss As New CssPageCollection
                Dim lstScript As New ScriptPageCollection

                lstCss = CType(CacheUtils.GetCache("CssPage"), CssPageCollection)
                lstScript = CType(CacheUtils.GetCache("ScriptPage"), ScriptPageCollection)
                'lstCss = Nothing
                'lstScript = Nothing
                If lstCss Is Nothing OrElse lstScript Is Nothing Then
                    lstCss = New CssPageCollection
                    lstScript = New ScriptPageCollection
                    Dim xmlPath As String = HttpContext.Current.Server.MapPath("~/" & Utility.ConfigData.CssPagePath)
                    Dim doc As New XmlDocument
                    doc.Load(xmlPath)
                    Dim root As XmlNode = doc.DocumentElement
                    For Each CssNode As XmlNode In root.ChildNodes
                        If CssNode.Name = "css" Then
                            If CssNode.Attributes IsNot Nothing AndAlso CssNode.Attributes("name") IsNot Nothing Then
                                Dim Css As New CssPageRow
                                Css.CssName = CssNode.Attributes("name").Value.ToString()
                                Css.TypePage = String.Empty
                                Css.PageList = New List(Of NodeAttribute)
                                For Each pageNode As XmlNode In CssNode.ChildNodes
                                    Dim node As NodeAttribute = New NodeAttribute()
                                    If Not (pageNode.Attributes("url") Is Nothing) Then
                                        node.URL = pageNode.Attributes("url").Value
                                        If (Css.TypePage = String.Empty) Then
                                            If (pageNode.Name = "page") Then
                                                Css.TypePage = "page"
                                            Else
                                                Css.TypePage = "remove"
                                            End If
                                        End If
                                    End If
                                    Css.PageList.Add(node)
                                Next

                                lstCss.Add(Css)
                            End If
                        ElseIf CssNode.Name = "script" Then
                            If CssNode.Attributes IsNot Nothing AndAlso CssNode.Attributes("name") IsNot Nothing Then
                                Dim script As New ScriptPageRow
                                script.ScriptName = CssNode.Attributes("name").Value.ToString()
                                script.TypePage = String.Empty
                                script.PageList = New List(Of NodeAttribute)
                                script.Defer = String.Empty
                                For Each pageNode As XmlNode In CssNode.ChildNodes
                                    If Not (pageNode.Attributes("url") Is Nothing) Then
                                        Dim node As NodeAttribute = New NodeAttribute()
                                        node.URL = pageNode.Attributes("url").Value

                                        If (script.TypePage = String.Empty) Then
                                            If (pageNode.Name = "page") Then
                                                script.TypePage = "page"
                                            Else
                                                script.TypePage = "remove"
                                            End If
                                        End If
                                        If Not pageNode.Attributes("defer") Is Nothing Then
                                            node.Defer = "defer"
                                            script.Defer = "defer"
                                        End If
                                        If Not pageNode.Attributes("async") Is Nothing Then
                                            node.Async = "async"
                                            script.Async = "async"
                                        End If
                                        If Not pageNode.Attributes("custom") Is Nothing Then
                                            Dim custom As Boolean = False
                                            Boolean.TryParse(pageNode.Attributes("custom").Value, custom)
                                            node.Custom = custom
                                        End If
                                        script.PageList.Add(node)
                                    End If
                                Next
                                lstScript.Add(script)
                            End If
                        End If
                    Next

                    CacheUtils.SetCache("CssPage", lstCss)
                    CacheUtils.SetCache("ScriptPage", lstScript)
                End If

                SetCssScriptPage(lstCss, lstScript, strUrl, strRawUrl, strUrlMasterPage)
            Catch ex As Exception
                Dim s As String = ex.ToString()
            End Try
        End Sub

        Private Sub SetCssScriptPage(ByVal lstCss As CssPageCollection, ByVal lstScript As ScriptPageCollection, ByVal strUrl As String, ByVal strRawUrl As String, ByVal strUrlMasterPage As String)
            Dim css As String = String.Empty
            Dim script As String = String.Empty
            Dim scriptPositionCustom As String = String.Empty
            'Dim cssVersion As String = SysParam.GetValue("CssScriptVersion")
            cssVersion = SysParam.GetValue("CssScriptVersion")

            For Each objCss As CssPageRow In lstCss
                Dim bCss As Boolean = False
                If (objCss.TypePage = "page") Then
                    For Each page As NodeAttribute In objCss.PageList
                        If strUrl.Contains(page.URL.ToLower().Trim()) Or strRawUrl.Contains(page.URL.ToLower().Trim()) Or strUrlMasterPage.Contains(page.URL.ToLower().Trim()) Then
                            bCss = True
                            Exit For
                        End If
                    Next
                End If

                If (bCss) Then
                    css &= String.Format("<link rel=""stylesheet"" type=""text/css"" href=""{0}?v={1}"" />", IIf(objCss.CssName.Contains("/includes/"), objCss.CssName, "/includes/theme/css/" & objCss.CssName), cssVersion)
                    'Add css vao
                End If
            Next

            For Each objScript As ScriptPageRow In lstScript
                Dim bScript As Boolean = False
                Dim custom As Boolean = False
                If (objScript.TypePage = "page") Then
                    For Each page As NodeAttribute In objScript.PageList
                        If strUrl.Contains(page.URL.Trim()) Or strRawUrl.Contains(page.URL.ToLower().Trim()) Or strUrlMasterPage.Contains(page.URL.ToLower().Trim()) Then
                            bScript = True
                            custom = page.Custom
                            Exit For
                        End If
                    Next
                End If

                If (bScript) Then
                    If custom Then
                        scriptPositionCustom &= String.Format("<script type=""text/javascript"" {3} src=""{0}?v={2}"" {1}></script>", objScript.ScriptName, objScript.Defer, cssVersion, objScript.Async)
                    Else
                        script &= String.Format("<script type=""text/javascript"" {3} src=""{0}?v={2}"" {1}></script>", objScript.ScriptName, objScript.Defer, cssVersion, objScript.Async)
                        'Add script vao
                    End If
                End If
            Next

            Dim master As System.Web.UI.MasterPage = Page.Master
            If Not String.IsNullOrEmpty(css) Then
                Dim litCss As Literal = Nothing

                If Not master Is Nothing Then
                    litCss = CType(master.FindControl("litCSS"), Literal)
                Else
                    litCss = CType(Page.FindControl("litCSS"), Literal)
                End If

                If litCss IsNot Nothing Then
                    litCss.Text = "<!-- LITCSS -->" & css
                End If
            End If

            If Not String.IsNullOrEmpty(script) Then
                Dim litScript As Literal = Nothing
                If Not master Is Nothing Then
                    litScript = CType(master.FindControl("litScript"), Literal)
                Else
                    litScript = CType(Page.FindControl("litScript"), Literal)
                End If

                If litScript IsNot Nothing Then
                    litScript.Text = "<!-- LITSCRIPT -->" & script
                End If
            End If

            If Not String.IsNullOrEmpty(scriptPositionCustom) Then
                Dim litScript As Literal = Nothing
                If Not master Is Nothing Then
                    litScript = CType(master.FindControl("litScriptCustom"), Literal)
                Else
                    litScript = CType(Page.FindControl("litScriptCustom"), Literal)
                End If

                If litScript IsNot Nothing Then
                    litScript.Text = "<!-- LITSCRIPTCUSTOM -->" & scriptPositionCustom
                End If
            End If
        End Sub


        Private Sub ClearSession()
            If Session.Count <= 2 Then
                Exit Sub
            End If

            Dim strUrl As String = Request.Url.ToString().ToLower().Trim()
            Dim strRawUrl As String = Request.RawUrl().ToLower.Trim()
            If strUrl.Contains("/includes/scripts/ajax.aspx") Or strUrl.Contains("/404.aspx") Or strUrl.Contains("/500.aspx") Or strUrl.Contains("/301.aspx") Then
                Exit Sub
            End If

            Try
                'Get from Cache
                Dim lstSession As New SessionPageCollection
                Dim key As String = "SessionPage"

                lstSession = CType(CacheUtils.GetCache(key), SessionPageCollection)
                If (lstSession Is Nothing) Then
                    lstSession = New SessionPageCollection
                    Dim xmlPath As String = HttpContext.Current.Server.MapPath("~/" & Utility.ConfigData.SessionPagePath)
                    Dim doc As New XmlDocument
                    doc.Load(xmlPath)
                    Dim root As XmlNode = doc.DocumentElement
                    For Each sessionNode As XmlNode In root.ChildNodes
                        If sessionNode.Attributes IsNot Nothing AndAlso sessionNode.Attributes("name") IsNot Nothing Then
                            Dim session As New SessionPageRow
                            session.SessionName = sessionNode.Attributes("name").Value.ToString()
                            session.TypePage = String.Empty
                            session.PageList = New List(Of String)
                            For Each pageNode As XmlNode In sessionNode.ChildNodes
                                If Not (pageNode.Attributes("url") Is Nothing) Then
                                    session.PageList.Add(pageNode.Attributes("url").Value)
                                    If (session.TypePage = String.Empty) Then
                                        If (pageNode.Name = "page") Then
                                            session.TypePage = "page"
                                        Else
                                            session.TypePage = "remove"
                                        End If
                                    End If
                                End If
                            Next
                            lstSession.Add(session)
                        End If
                    Next
                    CacheUtils.SetCache(key, lstSession)
                End If

                CheckAndClearSessionPage(lstSession, strUrl, strRawUrl)
            Catch ex As Exception
                Dim s As String = ex.ToString()
            End Try
        End Sub

        Private Sub CheckAndClearSessionPage(ByVal lstSession As SessionPageCollection, ByVal strUrl As String, ByVal strRawUrl As String)
            ''lay list session hien tai
            Dim lstCurrentSession As New List(Of String)
            For i As Integer = 0 To Session.Count - 1
                Try
                    lstCurrentSession.Add(Session.Keys(i))
                Catch ex As Exception
                End Try
            Next

            For Each objSession As SessionPageRow In lstSession
                'kiem tra session co trong list session hien tai khong
                If (lstCurrentSession.Contains(objSession.SessionName)) Then
                    If Not (Session(objSession.SessionName) Is Nothing) Then
                        Dim clearSession As Boolean = True
                        If (objSession.TypePage = "page") Then
                            For Each page As String In objSession.PageList
                                If strUrl.Contains(page.ToLower().Trim()) Or strRawUrl.Contains(page.ToLower().Trim()) Then
                                    clearSession = False
                                    Exit For
                                End If
                            Next
                        ElseIf (objSession.TypePage = "remove") Then
                            clearSession = False
                            For Each page As String In objSession.PageList
                                If strUrl.Contains(page.ToLower().Trim()) Or strRawUrl.Contains(page.ToLower().Trim()) Then
                                    clearSession = True
                                    Exit For
                                End If
                            Next
                        End If
                        If (clearSession) Then
                            Session.Remove(objSession.SessionName)
                        End If
                    Else
                        Session.Remove(objSession.SessionName)
                    End If
                End If
            Next
        End Sub

        Public Shared Function CashPointType(ByVal transNo As String) As Integer
            If transNo Is Nothing Then
                Return 0 ''khong xac ding
            End If
            If transNo = "" Or transNo.Length < 2 Then
                Return 0 ''khong xac ding
            End If
            Dim prefix As String = transNo.Substring(0, 2)
            If (prefix = "PR") Then
                Return 1 ''Product review
            ElseIf prefix = "CM" Then
                Return 2 ''Return item
            ElseIf (prefix.IndexOf("E") = 0 Or prefix.IndexOf("W") = 0) Then
                Return 3 ''Order
            End If

        End Function
        Public Function GetLevelPoint(ByVal memberId As Integer) As String
            Dim dt As DataTable = LevelPointRow.GetDiscount1(memberId, Utility.Common.GetDateForLevelPoint(DB, memberId))
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)("TypeName")
            Else
                Return ""
            End If
        End Function

        Public Function GetLoggedInName() As String
            If LoggedInFirstName <> Nothing Then Return LoggedInFirstName Else Return LoggedInLastName
        End Function

        Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
            ErrHandler.LogError(Server.GetLastError)
        End Sub

        'Public Property MetaKeyword() As String
        '    Get
        '        Return Me.Page.MetaKeywords
        '    End Get
        '    Set(ByVal Value As String)
        '        Me.Page.MetaKeywords = Value
        '    End Set
        'End Property

        'Public Overrides Property MetaDescription() As String
        '    Get
        '        Return Me.Page.MetaDescription
        '    End Get

        '    Set(ByVal Value As String)
        '        Me.Page.MetaDescription = Value
        '    End Set
        'End Property




        Public WriteOnly Property MetaTitle() As String
            Set(ByVal Value As String)
                Dim metaKey As New HtmlMeta
                metaKey.Name = "title"
                metaKey.ID = "title"
                metaKey.Content = Value
                Page.Header.Controls.Add(metaKey)
                metaKey.Dispose()

            End Set
        End Property

        Public Sub LoadMetaData(ByVal DB As Database, ByVal pageUrl As String)
            Dim pageDB As ContentToolPageRow = ContentToolPageRow.GetRowByURL(pageUrl)
            If Page IsNot Nothing Then
                Dim objMeta As New MetaTag
                objMeta.PageTitle = pageDB.MetaTitle
                objMeta.MetaDescription = pageDB.MetaDescription
                objMeta.MetaKeywords = pageDB.MetaKeywords
                SetPageMetaSocialNetwork(Me.Page, objMeta)
            End If
        End Sub
        Public Sub LoadMetaData(ByVal DB As Database, ByVal objPage As ContentToolPageRow)

            If Page IsNot Nothing Then
                Dim objMeta As New MetaTag
                objMeta.PageTitle = objPage.Title
                objMeta.MetaDescription = objPage.MetaDescription
                objMeta.MetaKeywords = objPage.MetaKeywords
                SetPageMetaSocialNetwork(Me.Page, objMeta)
            End If
        End Sub
        Public Property IsIndexed() As Boolean
            Get
                Dim mp As System.Web.UI.MasterPage = Me.Page.Master
                ''  If Not mp Is Nothing Then Return mp.IsIndexed Else Return True
                Return True
            End Get

            Set(ByVal Value As Boolean)
                Dim mp As System.Web.UI.MasterPage = Me.Page.Master
                '' If Not mp Is Nothing Then mp.IsIndexed = Value

            End Set
        End Property
        Public Property IsFollowed() As Boolean
            Get
                Dim mp As System.Web.UI.MasterPage = Me.Page.Master
                '' If Not mp Is Nothing Then Return mp.IsFollowed Else Return True
                Return True
            End Get

            Set(ByVal Value As Boolean)
                Dim mp As System.Web.UI.MasterPage = Me.Page.Master
                ''If Not mp Is Nothing Then mp.IsFollowed = Value
            End Set
        End Property

        Protected Function CheckDoubleLogin() As Boolean
            If LoggedInMemberId = Nothing Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function HasAccess() As Boolean
            If LoggedInMemberId = Nothing Then
                Return False
            Else
                Return True
            End If
        End Function

        Protected Function CheckAccess(ByVal sRedir As String) As Boolean
            If Not HasAccess() Then
                HttpContext.Current.Session("Redirect") = sRedir
                HttpContext.Current.Response.Redirect("/login.aspx")
            End If
        End Function
        ''check SSL
        Protected Sub DirectSSL()
            Dim SecureURL As String = System.Configuration.ConfigurationManager.AppSettings("GlobalSecureName")
            If Not SecureURL = String.Empty Then
                If Not HttpContext.Current.Request.IsSecureConnection AndAlso Not HttpContext.Current.Request.Url.ToString.ToLower.IndexOf(SecureURL.ToLower) = 0 Then
                    SecureURL = SecureURL & HttpContext.Current.Request.Url.PathAndQuery
                    SecureURL = Replace(LCase(SecureURL), "default.aspx", "")

                    HttpContext.Current.Response.Clear()
                    HttpContext.Current.Response.Status = "301 Moved Permanently"
                    HttpContext.Current.Response.AddHeader("Location", SecureURL)
                    HttpContext.Current.Response.End()
                End If
            End If
        End Sub

        Function GetPageName() As String
            Dim strPage As String = HttpContext.Current.Request.ServerVariables("PATH_INFO")
            strPage = strPage.Replace(".aspx", "")

            If strPage.Contains("/") Then
                strPage = strPage.Substring(strPage.LastIndexOf("/") + 1)
            End If

            Return strPage
        End Function

        Protected Sub ChangeWWWnonWWW(ByVal Https As Boolean)
            If IsWebTest() Then
                Exit Sub
            End If

            If Not ConfigData.GlobalRefererName().Contains(Request.Url.Host) Then
                Redirect301(ConfigData.GlobalRefererName())
                Exit Sub
            End If

            Dim svrHttps As String = HttpContext.Current.Request.ServerVariables("HTTPS")
            Dim svrHost As String = HttpContext.Current.Request.ServerVariables("HTTP_HOST")
            Dim svrUrl As String = HttpContext.Current.Request.ServerVariables("URL")
            Dim svrQueryString As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")
            Dim svrNewUrl As String = ""
            Dim rawURL As String = Request.RawUrl


            If Left(svrHost, 4) <> "www." Then
                'Check if the page is secure or not
                If Https Then
                    svrNewUrl = "https://www."
                Else
                    svrNewUrl = "http://www."
                End If

                'Add the domain, folder(s) and page requested as well as remove directory indexes
                svrNewUrl &= svrHost & rawURL
                svrNewUrl = Replace(LCase(svrNewUrl), "/home.aspx", "")
                svrNewUrl = Replace(LCase(svrNewUrl), "home.aspx", "")

                'Do the actual 301 redirect to the newly constructed url
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.Status = "301 Moved Permanently"
                HttpContext.Current.Response.AddHeader("Location", svrNewUrl)
                HttpContext.Current.Response.End()
            ElseIf svrHttps = "off" AndAlso Https Then
                If Not svrHost.Contains("www") Then
                    svrNewUrl = "https://www."
                Else
                    svrNewUrl = "https://"
                End If

                svrNewUrl &= svrHost & rawURL
                svrNewUrl = Replace(LCase(svrNewUrl), "/home.aspx", "")
                svrNewUrl = Replace(LCase(svrNewUrl), "home.aspx", "")

                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.Status = "301 Moved Permanently"
                HttpContext.Current.Response.AddHeader("Location", svrNewUrl)
                HttpContext.Current.Response.End()
            End If


        End Sub

        Private Sub DisposeSession()
            If Not Request("F_BrandId") Is Nothing OrElse (HttpContext.Current.Request.ServerVariables("URL").ToString.Contains("default.aspx") = False And HttpContext.Current.Request.ServerVariables("URL").ToString.Contains("item.aspx") = False And HttpContext.Current.Request.ServerVariables("URL").ToString.Contains("main-category.aspx") = False) OrElse (HttpContext.Current.Request.RawUrl.Contains("nail-sales")) Then
                Session.Remove("DepartmentURLCode")
            End If
            If LCase(Request.Url.ToString).Contains("video") = False And LCase(Request.Url.ToString).Contains("news") = False And LCase(Request.Url.ToString).Contains("include") = False Then
                Session.Remove("VideoCateId")
                Session.Remove("NewsCateId")
            End If
            ValidateSesssionSearchKeyword()
        End Sub


        Public Sub SaveMember2()
            If Session("RegisterMemberBillingAddress") Is Nothing Or Session("RegisterMemberShippingAddress") Is Nothing Or Session("RegisterMember") Is Nothing Then
                Response.Redirect("RegisterAccount.aspx")
            End If
            Dim logMessage As String = String.Empty
            Dim MemberBillingAddress As MemberAddressRow = Nothing
            Dim MemberShippingAddress As MemberAddressRow = Nothing
            Dim Member As MemberRow = Nothing
            Dim isError As Boolean = True
            Try
                MemberBillingAddress = DirectCast(Session("RegisterMemberBillingAddress"), MemberAddressRow)
                MemberShippingAddress = DirectCast(Session("RegisterMemberShippingAddress"), MemberAddressRow)
                Member = DirectCast(Session("RegisterMember"), MemberRow)
                Member.DB = DB

                logMessage = "Begin insert member data:" & "Username=" & Member.Username & ",Pass=" & Member.Password & ",Email=" & MemberBillingAddress.Email & ",Date=" & DateTime.Now.ToString()
                Dim isMailingMember As Boolean = DirectCast(Session("RegisterIsMailingMember"), Boolean)
                Dim bRedirect As Boolean = True, bError As Boolean = False, bAtLeastOne As Boolean = False
                Dim sRightsOnlyItems As String = ""
                Dim SetupBoards As Boolean = False
                Dim sCheckUsername As String = DB.ExecuteScalar("SELECT Username FROM Member WHERE Username = " & DB.Quote(Member.Username))
                If sCheckUsername <> "" Then
                    Throw New Exception("The username you entered is already in use.  Please visit the <a href='/members/forgotpassword.aspx'>forgot your password</a> page if you no longer remember your password, otherwise please use a different username.")
                End If
                sCheckUsername = DB.ExecuteScalar("SELECT * FROM Member WHERE CustomerId = (select top 1 customerid from customer where email = " & DB.Quote(MemberBillingAddress.Email) & ")")
                If sCheckUsername <> "" Then
                    Throw New Exception("The email address you entered is already in use.  Please visit the <a href='/forgotpassword.aspx'>forgot your password</a> page if you no longer remember your password, otherwise please use a different email address.")
                End If
                Dim CurrentMemberId As Integer
                DB.BeginTransaction()
                Member.CreateDate = Now
                Member.IsActive = True
                CurrentMemberId = Member.Insert(False)
                logMessage = logMessage & "<br><br>" & "MemberId=" & CurrentMemberId
                If CurrentMemberId < 1 Then
                    Throw New Exception("An error has occurred in process register account.Please try again later!")
                End If
                MemberBillingAddress.MemberId = CurrentMemberId
                MemberBillingAddress.Label = "Default Billing Address"
                MemberBillingAddress.AddressType = "Billing"
                ' Update Customer
                Member.Customer.Address = MemberBillingAddress.Address1
                Member.Customer.Address2 = MemberBillingAddress.Address2
                Member.Customer.Name = MemberBillingAddress.FirstName
                Member.Customer.Name2 = MemberBillingAddress.LastName
                Member.Customer.City = MemberBillingAddress.City
                Member.Customer.Zipcode = MemberBillingAddress.Zip
                If MemberBillingAddress.Country <> "US" Then
                    Member.Customer.County = MemberBillingAddress.Region
                Else
                    Member.Customer.County = MemberBillingAddress.State
                End If
                Member.Customer.Phone = MemberBillingAddress.Phone
                If MemberBillingAddress.Email <> Nothing Then Member.Customer.Email = MemberBillingAddress.Email
                Member.Customer.ContactNo = Member.CustomerContact.ContactNo
                Member.Customer.ContactId = Member.CustomerContact.ContactId
                Member.Customer.DoExport = True
                Member.Customer.Update()

                ' Update CustomerContact
                Member.CustomerContact.Address = MemberBillingAddress.Address1
                Member.CustomerContact.Address2 = MemberBillingAddress.Address2
                Member.CustomerContact.CustName = MemberBillingAddress.FirstName
                Member.CustomerContact.CustName2 = MemberBillingAddress.LastName
                Member.CustomerContact.Phone = MemberBillingAddress.Phone
                Member.CustomerContact.City = MemberBillingAddress.City
                Member.CustomerContact.PostCode = MemberBillingAddress.Zip
                If MemberBillingAddress.Country = "US" Then Member.CustomerContact.County = MemberBillingAddress.State
                Member.CustomerContact.CountryCode = MemberBillingAddress.Country
                Member.CustomerContact.Email = MemberBillingAddress.Email
                Member.CustomerContact.DoExport = True
                Member.CustomerContact.Update()

                MemberShippingAddress.MemberId = CurrentMemberId
                MemberShippingAddress.Label = "Default Shipping Address"
                MemberShippingAddress.AddressType = "Shipping"
                MemberShippingAddress.DB = DB
                MemberBillingAddress.DB = DB
                MemberShippingAddress.Insert()
                MemberBillingAddress.Insert()

                Dim MailingMemberId As Integer = DB.ExecuteScalar("SELECT MemberId FROM MailingMember WHERE Email=" & DB.Quote(Member.Customer.Email))
                Dim dbMailingMember As MailingMemberRow

                If isMailingMember = True Then
                    ' Add new mailing member subscription
                    If MailingMemberId = 0 Then
                        dbMailingMember = New MailingMemberRow(DB)
                        dbMailingMember.Email = Member.Customer.Email
                        dbMailingMember.MimeType = "HTML"
                        dbMailingMember.Name = MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName
                        dbMailingMember.Status = "ACTIVE"
                        dbMailingMember.DB = DB
                        dbMailingMember.Insert()
                    Else
                        dbMailingMember = MailingMemberRow.GetRow(DB, MailingMemberId)
                        dbMailingMember.MimeType = "HTML"
                        dbMailingMember.Email = Member.Customer.Email
                        dbMailingMember.Name = MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName
                        dbMailingMember.Status = "ACTIVE"
                        dbMailingMember.DB = DB
                        dbMailingMember.Update()
                    End If

                    dbMailingMember.DeleteFromAllLists()
                    dbMailingMember.InsertToList(1)
                ElseIf Not isMailingMember And MailingMemberId > 0 Then
                    dbMailingMember = MailingMemberRow.GetRow(DB, MailingMemberId)
                    dbMailingMember.Status = "DELETED"
                    dbMailingMember.DB = DB
                    dbMailingMember.Update()
                End If

                DB.CommitTransaction()
                logMessage = logMessage & "<br><br>" & "Insert sucessfull"

                isError = False
                If Not HasAccess() Then
                    Session("MemberId") = CurrentMemberId
                    Session("Username") = Member.Username
                End If
            Catch ex As Exception
                DB.RollbackTransaction()
                logMessage = logMessage & "<br><br>" & "Insert error:" & ex.Message
                Throw ex
            End Try
            Components.Email.SendError("ToError500", "Register Account from New Page", logMessage)
            If isError Then
                Exit Sub
            End If

            Dim dbMember As MemberRow = MemberRow.GetRow(Session("MemberId"))
            If Not dbMember.LastOrderId = Nothing Then
                Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, dbMember.LastOrderId)
                If dbOrder.ProcessDate = Nothing AndAlso dbOrder.MemberId = dbMember.MemberId Then
                    Session("OrderId") = dbOrder.OrderId
                    Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", dbOrder.OrderId)
                Else
                    Session("OrderId") = Nothing
                    Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
                    dbMember.LastOrderId = Nothing
                    dbMember.Update(DB)
                End If
            Else
                Session("OrderId") = Nothing
                Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
            End If

            Dim sActiveCode As String = ""
            If MemberBillingAddress.Email <> "" Then
                sActiveCode = Guid.NewGuid().ToString()
                DB.ExecuteSQL("UPDATE Member SET isActive ='false', ActiveCode='" & sActiveCode & "' where memberid = " & DB.Number(Session("MemberId")))
            End If
            If Not HasAccess() Then
                Dim sMsg As String
                Dim sName As String = "forgotpassword@nss.com"
                Dim sSubject As String = "Your password for the nss.com website"
                Dim dbEmailTemplet As DataTable
                Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, dbMember.CustomerId)
                dbEmailTemplet = EmailTempletRow.GetOutboundEmailTemplets(DB, "11")
                Dim sCoupon As String = ""
                If dbEmailTemplet.Rows.Count > 0 Then
                    sName = dbEmailTemplet.Rows(0)("Name")
                    sSubject = dbEmailTemplet.Rows(0)("Subject")
                    sMsg = dbEmailTemplet.Rows(0)("Contents")

                    If MemberBillingAddress.Email <> "" Then
                        GetCoupon(sCoupon)
                        EmailTempletRow.FormatOutboundEmailTemplet(MemberBillingAddress.FirstName, MemberBillingAddress.LastName, Member.Username, Member.Password, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sCoupon, sActiveCode, sMsg, "", "")
                        Email.SendHTMLMail(FromEmailType.NoReply, MemberBillingAddress.Email, MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName, sSubject, sMsg)
                        BaseShoppingCart.AwardedPoint(DB, Member.Customer.CustomerNo, Member.MemberId)
                    Else
                        If dbCustomer.Email <> "" Then
                            EmailTempletRow.FormatOutboundEmailTemplet(MemberBillingAddress.FirstName, MemberBillingAddress.LastName, dbMember.Username, dbMember.Password, dbCustomer.Email, MemberBillingAddress.FirstName, MemberBillingAddress.LastName, MemberBillingAddress.Address1, MemberBillingAddress.Address2, MemberBillingAddress.Phone, MemberBillingAddress.Country, MemberBillingAddress.Company, MemberBillingAddress.City, MemberBillingAddress.State, MemberBillingAddress.Region, MemberBillingAddress.Zip, MemberBillingAddress.Fax, MemberShippingAddress.FirstName, MemberShippingAddress.LastName, MemberShippingAddress.Address1, MemberShippingAddress.Address2, MemberShippingAddress.Phone, MemberShippingAddress.Country, MemberShippingAddress.Company, MemberShippingAddress.City, MemberShippingAddress.State, MemberShippingAddress.Region, MemberShippingAddress.Zip, "", "", sMsg, "", "")
                            Email.SendHTMLMail(FromEmailType.NoReply, dbCustomer.Email, MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName, sSubject, sMsg)
                        End If
                    End If
                Else
                    ''  Email.SendError("ToError500", MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName, sSubject, MemberBillingAddress.Email)

                    Email.SendHTMLMail(FromEmailType.NoReply, "webmaster@nss.com", MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName, sSubject, MemberBillingAddress.Email)

                End If
            End If
            Session("RegisterMemberBillingAddress") = Nothing
            Session("RegisterMemberShippingAddress") = Nothing
            Session("RegisterMember") = Nothing
            Session("RegisterIsMailingMember") = Nothing
        End Sub

        Private Sub GetCoupon(ByRef Msg As String)
            Dim res As DataTable = DB.GetDataTable("select * from StorePromotion where IsRegisterSend='true'")
            If res.Rows.Count > 0 Then
                Dim i As Integer
                Dim PromotionCode As String = ""
                For i = 0 To res.Rows.Count - 1
                    PromotionCode = res.Rows(i)("PromotionCode")
                    If PromotionCode <> "" Then
                        Msg = Msg & " PromotionCode :" & PromotionCode & " "
                    End If
                Next
            End If

        End Sub

        Private Sub CheckParamToSetNoIndex()
            If ((GetQueryString("brand") <> Nothing AndAlso Not Request.RawUrl.Contains("nail-brand")) Or GetQueryString("price") <> Nothing Or GetQueryString("rating") <> Nothing Or GetQueryString("collection") <> Nothing Or GetQueryString("tone") <> Nothing Or GetQueryString("shade") <> Nothing Or GetQueryString("sort") <> Nothing Or GetQueryString("pros") <> Nothing Or GetQueryString("cons") <> Nothing Or GetQueryString("exp") <> Nothing) Then
                IsIndexed = False
                IsFollowed = False
            End If
        End Sub

        Public Sub RecalculateSignatureConfirmation(ByVal DB As Database, ByVal o As StoreOrderRow)
            o.IsSignatureConfirmation = Not (o.ShipToAddressType = AddressType.Commercial)
            Dim methodID As Integer = DB.ExecuteScalar("SELECT TOP 1 CarrierType FROM StoreCartItem item LEFT JOIN ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & o.OrderId & " and  type = 'item' AND Code IN (" & Utility.Common.USShippingCode & ")")
            'Pickup from warehouse & Freight Delivery = 0
            If methodID = 0 Then
                methodID = DB.ExecuteScalar("SELECT TOP 1 CarrierType FROM StoreCartItem item LEFT JOIN ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & o.OrderId & " and  type = 'item' AND Code NOT IN (" & Utility.Common.USShippingCode & ")")
            End If
            If methodID = 0 AndAlso (o.CarrierType = 15) Then
                methodID = Common.DefaultShippingId
            ElseIf methodID = 0 Then
                methodID = o.CarrierType
            End If
            If o.SubTotal < SysParam.GetValue("USTotalOrderResidential") Then
                If o.IsSignatureConfirmation Then
                    o.ResidentialFee = ShipmentMethod.GetValue(methodID, ShipmentValue.Residential)
                    o.SignatureConfirmation = ShipmentMethod.GetValue(methodID, ShipmentValue.Signature)
                Else
                    o.SignatureConfirmation = 0
                End If
            Else
                o.ResidentialFee = 0
                If o.IsSignatureConfirmation Then
                    o.SignatureConfirmation = ShipmentMethod.GetValue(methodID, ShipmentValue.Signature)
                Else
                    o.SignatureConfirmation = 0
                End If
            End If
            If o.SignatureConfirmation = 0 Then
                o.IsSignatureConfirmation = False
            End If

        End Sub
        Public Function CheckAddressType(ByVal DB As Database, ByVal o As StoreOrderRow) As Validator

            If (o.ShipToCountry = "US") AndAlso (o.ShipToCounty <> "PR" And o.ShipToCounty <> "VI" And o.ShipToCounty <> "HI" And o.ShipToCounty <> "AK") Then
                Dim addressCheck As String = o.ShipToName & " " & o.ShipToName2 & "," & o.ShipToSalonName & "," & (o.ShipToAddress & " " & o.ShipToAddress2).ToUpper() & "," & o.ShipToCity.ToUpper() & "," & o.ShipToCounty.ToUpper() & "," & o.ShipToZipcode.ToUpper() & "," & o.ShipToCountry.ToUpper()

                'Tam thoi check UPS
                Dim valid As New Validator(o.ShipToName & " " & o.ShipToName2, o.ShipToSalonName, (o.ShipToAddress & " " & o.ShipToAddress2).ToUpper(), o.ShipToCity.ToUpper(), o.ShipToCounty.ToUpper(), o.ShipToZipcode.ToUpper(), o.ShipToCountry.ToUpper())
                o.ShipToAddressType = IIf(CInt(valid.Type) = 3, "0", CInt(valid.Type))

                'Remove Residential text
                If Not String.IsNullOrEmpty(o.Comments) Then
                    While o.Comments.Contains("|Residential")
                        o.Comments = o.Comments.Replace("|Residential", "")
                    End While

                    o.Comments &= IIf(o.ShipToAddressType <> 1, "|Residential", "")
                Else
                    o.Comments = IIf(o.ShipToAddressType <> 1, "|Residential", "")
                End If
                RecalculateSignatureConfirmation(DB, o)
                Return valid
            Else
                Return Nothing
            End If
        End Function
        Public Shared Sub LoadRegionControl(ByRef holder As System.Web.UI.WebControls.PlaceHolder, ByVal region As String, ByVal page As System.Web.UI.Page)
            Dim url As String = page.Request.Path
            Dim lstControl As ContentToolControlCollection = ContentToolControlRow.LoadListByRegionPage(url, region)
            If lstControl Is Nothing Then
                Exit Sub
            End If
            Dim ctrl As Control = Nothing
            For Each objCtr As ContentToolControlRow In lstControl
                Try
                    ctrl = page.LoadControl(objCtr.URL)
                Catch ex As Exception

                End Try
                If ctrl Is Nothing Then
                    Continue For
                End If
                If Not String.IsNullOrEmpty(objCtr.ControlId) Then
                    ctrl.ID = objCtr.ControlId
                End If
                Dim c As IModule = Nothing
                If (TypeOf ctrl Is IModule) Then
                    c = CType(ctrl, IModule)
                End If
                If Not c Is Nothing Then
                    c.Args = objCtr.Args
                End If
                holder.Controls.Add(ctrl)
            Next
        End Sub
        Public Shared Sub LoadRegionControl(ByRef holder As HtmlGenericControl, ByVal region As String, ByVal page As System.Web.UI.Page)
            Dim url As String = page.Request.Path
            Dim lstControl As ContentToolControlCollection = ContentToolControlRow.LoadListByRegionPage(url, region)
            If lstControl Is Nothing Then
                Exit Sub
            End If
            Dim ctrl As Control = Nothing
            For Each objCtr As ContentToolControlRow In lstControl
                Try
                    ctrl = page.LoadControl(objCtr.URL)
                Catch ex As Exception

                End Try
                If ctrl Is Nothing Then
                    Continue For
                End If
                If Not String.IsNullOrEmpty(objCtr.ControlId) Then
                    ctrl.ID = objCtr.ControlId
                End If
                Dim c As IModule = Nothing
                If (TypeOf ctrl Is IModule) Then
                    c = CType(ctrl, IModule)
                End If
                If Not c Is Nothing Then
                    c.Args = objCtr.Args
                End If
                holder.Controls.Add(ctrl)
            Next
        End Sub
        <System.Web.Services.WebMethod()>
        Public Shared Function ExecuteCommand(ByVal commandName As String, ByVal targetMethod As String, ByVal data1 As Object, ByVal data2 As Object) As Object()
            Try
                Dim result As Object() = Command.Create(commandName).Execute(data1, data2)
                result(result.Length - 1) = targetMethod

                Return result
            Catch ex As Exception
                ' TODO: add logging functionality 
                Throw
            End Try
        End Function

        <System.Web.Services.WebMethod()>
        Public Shared Function ExecuteCommand(ByVal commandName As String, ByVal targetMethod As String, ByVal obj As Object()) As Object()
            Try
                Dim result As Object() = Command.Create(commandName).Execute(obj)
                'result(result.Length - 1) = targetMethod

                Dim resultHTML As New List(Of Object)()
                Dim resultParam As New List(Of Object)()

                For Each o As Object In result
                    If o Is Nothing Then
                        o = String.Empty
                    End If
                    If o.GetType().FullName.Contains("HtmlString") Then
                        resultHTML.Add(CType(o, HtmlString).ToHtmlString())
                    Else
                        resultParam.Add(o)
                    End If
                Next

                Dim resultReturn As Object() = New Object(2) {}
                resultReturn(0) = resultHTML
                resultReturn(1) = resultParam
                resultReturn(2) = targetMethod

                Return resultReturn
                'Return result
            Catch ex As Exception
                ' TODO: add logging functionality 
                Throw
            End Try
        End Function
        Public Function GetCardTypeId(ByVal CardCode As String) As String
            Dim Id As Integer = Integer.MinValue
            Select Case CardCode
                Case "V"
                    Id = 1
                Case "M"
                    Id = 2
                Case "A"
                    Id = 3
                Case "D"
                    Id = 4
            End Select

            Return Id
        End Function
        Public Function GetCityLocation(ByVal RemoteIP As String) As String

            If String.IsNullOrEmpty(RemoteIP) Then Return String.Empty
            Dim ipLocation As String = String.Empty

            Try
                Dim oremoteIP As String = RemoteIP
                If Not String.IsNullOrEmpty(oremoteIP) Then
                    ipLocation = GetLocationByIP(oremoteIP, ConvertIPtoNum(RemoteIP), "location")
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetCityLocation", System.Web.HttpContext.Current.Request.RawUrl & "<br>IP: " & RemoteIP & "<br>Error: " & ex.ToString())
            End Try

            Return IIf(String.IsNullOrEmpty(ipLocation), String.Empty, ipLocation)

        End Function
        Public Shared Function IsHasFilterSearch() As Boolean
            Dim queryBrand As String = IIf(GetQueryString("brandid") Is Nothing, String.Empty, GetQueryString("brandid")) 'IIf(GetQueryString("brand") Is Nothing, String.Empty, GetQueryString("brand"))
            If (Not String.IsNullOrEmpty(queryBrand)) Then
                Return True
            End If
            Dim queryPrice As String = IIf(GetQueryString("price") Is Nothing, String.Empty, GetQueryString("price"))
            If (Not String.IsNullOrEmpty(queryPrice)) Then
                Return True
            End If
            Dim queryRating As String = IIf(GetQueryString("rating") Is Nothing, String.Empty, GetQueryString("rating"))
            If (Not String.IsNullOrEmpty(queryRating)) Then
                Return True
            End If
            Dim queryPros As String = IIf(GetQueryString("pros") Is Nothing, String.Empty, GetQueryString("pros"))
            If (Not String.IsNullOrEmpty(queryPros)) Then
                Return True
            End If
            Dim queryCons As String = IIf(GetQueryString("cons") Is Nothing, String.Empty, GetQueryString("cons"))
            If (Not String.IsNullOrEmpty(queryCons)) Then
                Return True
            End If
            Dim queryExp As String = IIf(GetQueryString("exp") Is Nothing, String.Empty, GetQueryString("exp"))
            If (Not String.IsNullOrEmpty(queryBrand)) Then
                Return True
            End If
            Dim qrCollection As String = IIf(String.IsNullOrEmpty("CollectionId"), String.Empty, GetQueryString("CollectionId"))
            If (Not String.IsNullOrEmpty(qrCollection)) Then
                Return True
            End If
            Dim qrTone As String = IIf(String.IsNullOrEmpty("ToneId"), String.Empty, GetQueryString("ToneId"))
            If (Not String.IsNullOrEmpty(qrTone)) Then
                Return True
            End If
            Dim qrShade As String = IIf(String.IsNullOrEmpty("ShadeId"), String.Empty, GetQueryString("ShadeId"))
            If (Not String.IsNullOrEmpty(qrShade)) Then
                Return True
            End If
            Dim qrCategoryid As String = IIf(String.IsNullOrEmpty("DepartmentId"), String.Empty, GetQueryString("DepartmentId"))
            If (Not String.IsNullOrEmpty(qrCategoryid)) Then
                Return True
            End If
            Return False
        End Function

        'Khoa add methods list product GetProductData into Sitepage
        Public Shared Function GetProductData(ByVal ItemCollection As StoreItemCollection, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isInternational As Boolean, ByVal isQuickOrder As Boolean, Optional ByVal isMedium As Boolean = True) As IEnumerable
            If ItemCollection IsNot Nothing AndAlso ItemCollection.Count > 0 Then
                Dim lst As New List(Of Product)()
                Dim DB As New Database()
                DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                Dim strError As String = String.Empty
                Dim i As Integer = 0
                Dim orderid As Integer = Utility.Common.GetCurrentOrderId()
                'Get list item in cart
                Dim lstItem As String = StoreCartItemRow.GetItemListIdByOrderId(orderid)

                For Each si As StoreItemRow In ItemCollection
                    Try
                        Dim p As New Product()
                        If si.ItemId = 0 Then
                            p.Index = ((PageIndex * PageSize) - PageSize) + i + 1
                            p.Title = si.ItemName
                            p.Url = si.URLCode
                            lst.Add(p)
                            i += 1
                            Continue For
                        End If
                        p.Title = IIf(isQuickOrder, IIf(String.IsNullOrEmpty(si.ItemNameNew), si.ItemName, si.ItemNameNew), si.ItemName2)

                        If String.IsNullOrEmpty(si.URLCode) Then
                            p.Url = URLParameters.ProductUrl(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(si.ItemName.ToLower())), si.ItemId)
                        Else
                            p.Url = URLParameters.ProductUrl(si.URLCode, si.ItemId)
                        End If

                        p.SKU = si.SKU
                        p.Image = Utility.ConfigData.CDNMediaPath & "/assets/items/" & IIf(isMedium, "medium", "featured") & "/" & IIf(si.Image = Nothing, "na.jpg", si.Image)
                        p.Promotion = si.MixMatchDescription
                        p.ItemId = si.ItemId
                        p.Index = ((PageIndex * PageSize) - PageSize) + i + 1

                        Dim strAddCartEvent As String = String.Empty
                        If Not isQuickOrder Then
                            strAddCartEvent = "AddCartFromList({0});"
                        End If

                        Dim strAddCart As String = "<input value=""{0}"" id=""btnAddCart{3}"" type=""button"" class=""{1}"" onclick=""{2}"" style=""display:{4}"" />"
                        Dim strInCart As String = "<input value=""View Cart"" id=""btnViewCart{0}"" type=""button"" class=""view-cart"" onclick=""window.location.href='/store/cart.aspx'"" style=""display:{1}"" />"
                        Dim imgReview As String = String.Empty
                        Dim averageStars As Double = si.AverageReview
                        If (si.CountReview >= 1) Then
                            'If averageStars.ToString().Contains(".5") Then
                            '    averageStars = averageStars.ToString().Replace(".", "")
                            'Else
                            '    averageStars = averageStars.ToString().Replace(".", "") & "0"
                            'End If
                            'If HttpContext.Current.Request.Url.ToString.Contains("reward-point.aspx") Then
                            '    p.Review = String.Format("<div class=""review""><img alt=""{3}"" src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><span>{1} review{2}</span></div>", averageStars, si.CountReview, IIf(si.CountReview > 1, "s", ""), "")
                            'Else
                            '    p.Review = String.Format("<div class=""review""><a href=""{4}#review-section""><img alt=""{3}"" src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><span>{1} review{2}</span></a></div>", averageStars, si.CountReview, IIf(si.CountReview > 1, "s", ""), "", URLParameters.ProductUrl(si.URLCode, si.ItemId))
                            'End If
                            If HttpContext.Current.Request.Url.ToString.Contains("reward-point.aspx") Then
                                'p.Review = String.Format("<div class=""review""><img alt=""{3}"" src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><span>{1} review{2}</span></div>", averageStars, si.CountReview, IIf(si.CountReview > 1, "s", ""), "")
                                p.Review = String.Format("<div class=""review"">" & BindIconStar(averageStars.ToString()) & "<span>{1} review{2}</span></div>", averageStars, si.CountReview, IIf(si.CountReview > 1, "s", ""))
                            Else
                                ' p.Review = String.Format("<div class=""review""><a href=""{4}#review-section""><img alt=""{3}"" src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><span>{1} review{2}</span></a></div>", averageStars, si.CountReview, IIf(si.CountReview > 1, "s", ""), "", URLParameters.ProductUrl(si.URLCode, si.ItemId))
                                p.Review = String.Format("<div class=""review""><a href=""{3}#review-section"">" & BindIconStar(averageStars.ToString()) & "<span>{1} review{2}</span></a></div>", averageStars, si.CountReview, IIf(si.CountReview > 1, "s", ""), URLParameters.ProductUrl(si.URLCode, si.ItemId))
                            End If
                        End If

                        'icon new, hot, bestseller
                        If si.IsFreeShipping Then
                            p.Icon = String.Format("<span class='ico-freeshipping'></span>")
                        ElseIf si.IsNewTrue Then
                            p.Icon = String.Format("<span class='ico-new'></span>")
                        ElseIf si.IsBestSeller Then
                            p.Icon = String.Format("<span class='ico-bestseller'></span>")
                        ElseIf si.IsHot Then
                            p.Icon = String.Format("<span class='ico-hot'></span>")
                        End If
                        p.IsFeatured = si.IsFeatured
                        p.IsBestSeller = si.IsBestSeller
                        p.IsNew = IIf(ItemCollection.EnableHasNewItem, ItemCollection.HasNewItem, si.IsNew)
                        p.IsHot = si.IsHot

                        'in cart
                        If orderid > 0 AndAlso lstItem.Contains(si.ItemId.ToString()) Then
                            p.InCart = True
                            'Try
                            '    p.InCart = DB.ExecuteScalar("SELECT  dbo.fc_StoreCartItem_CheckItemInCart(" & Utility.Common.GetOrderIdFromCartCookie() & ", " & si.ItemId & "," & IIf(si.IsRewardPoints, 1, 0) & ")")
                            'Catch ex As Exception
                            '    p.InCart = False
                            'End Try
                        Else
                            p.InCart = False
                        End If

                        'price
                        Dim isItemMulti As Boolean = False
                        p.Price = StoreItemRow.ReturnPrice(BaseShoppingCart.GetItemPrice(DB, si, 1, 0, 0, isItemMulti))
                        If si.youSave > 0 And p.Price.Contains("As low as") = False Then
                            p.YouSave = "<li class=""save"">You Save: <span class=""yousave"">" & FormatCurrency(si.youSave) & "</span> (<span class=""savepercent"">" & si.savePercent & "</span>%)</li>"
                        End If

                        Dim isPageItemPoint As Boolean = HttpContext.Current.Request.Url.ToString.Contains("reward-point.aspx")
                        If Not (isPageItemPoint) Then
                            si.IsRewardPoints = False
                            si.RewardPoints = 0
                        End If
                        Dim IsActive, IsRecent As Boolean
                        Try
                            If HttpContext.Current.Request.Url.ToString.Contains("recently-viewed.aspx") Then
                                IsRecent = True
                            End If
                            IsActive = si.IsActive
                        Catch ex As Exception
                            IsActive = False
                            IsRecent = False
                        End Try

                        'textbox qty & button add cart
                        If IsActive = False And IsRecent Then
                            p.Price = "<span class=""red"">It is no longer available. Please select a different item.</span>"
                            p.YouSave = String.Empty
                        ElseIf si.IsFreeSample Or si.IsFreeGift >= 2 Then
                            p.Price = String.Empty
                            p.YouSave = String.Empty
                        ElseIf si.IsLoginViewPrice = True Then
                            p.Price = String.Format("<a href=""/members/login.aspx"" class=""loginviewprice"">Log In | Register</a> <a class=""loginviewpricetip"" id=""aLoginViewPrice{0}"" title="""">(?)</a><div class=""ShowLoginViewPriceTip"">Required registered professional log in to purchase OPI products.</div>", si.SKU)
                            p.YouSave = String.Empty
                            'ElseIf si.IsFlammable = True And isInternational Then
                            '    p.Price = "<span class=""red"">This item is not available for customer outside of 48 states within continental USA.</span>"
                            '    p.YouSave = String.Empty
                        Else
                            If si.QtyOnHand <= 0 And Not Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) And Not si.IsSpecialOrder Then
                                p.AddCart = String.Format("<div id=""divCart"" class=""red"">Currently Out of Stock <br><a href=""#"" onclick=""NotifyInStock('{0}');"" class=""notifystock"">Notify me when in stock</a></div>", si.ItemId)
                                If (isPageItemPoint) Then
                                    p.Price = "<span class='pointPrice'>Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</span>"
                                Else
                                    If (si.IsRewardPoints And si.RewardPoints > 0) Then
                                        Dim priceTable As String = String.Format("<table class='itemprice' cellpadding='0' cellspacing='0'><tr><td>Redeem Points: {0}</td></tr><tr><td>{1}</td></tr></table>", Utility.Common.FormatPointPrice(si.RewardPoints), p.Price)
                                        p.Price = priceTable
                                    End If

                                End If
                            Else

                                If isItemMulti Then 'si.IsVariance Or 
                                    If (isPageItemPoint) Then
                                        p.Price = "<span class='pointPrice'>Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</span>"

                                        p.AddCart += String.Format("<div class=""qty"" id=""divQty{0}"" style=""display:{2}""><div class=""plus""><a href=""javascript:void(Increase('txtQtyItem{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))"">&ndash;</a></div><div class=""pull-left""><input type=""tel"" class=""txt-qty"" name=""txtQtyItem{0}"" id=""txtQtyItem{0}"" value=""{1}"" onkeypress=""return numbersonly(txtQtyItem{0},event);"" maxlength=""4"" /></div>" &
                                                                    "<div class=""bao-arrow""><ul><a href=""javascript:void(Increase('txtQtyItem{0}'))""><li><b class=""arrow-up arrow-down-act"" id=""""></b></li></a>" &
                                                                    "<a href=""javascript:void(Decrease('txtQtyItem{0}',1))""><li class=""bd-qty""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></li></a></ul></div></div>", si.ItemId, IIf(isQuickOrder, "0", "1"), IIf(p.InCart = False, "block", "none"))
                                        If Not isQuickOrder Then
                                            p.AddCart += String.Format(strAddCart, "Add to Cart", "bt-add-cart bg-add-cart", String.Format(strAddCartEvent, si.ItemId), si.ItemId, IIf(p.InCart = False, "block", "none"))
                                        End If
                                    Else
                                        If (si.CaseQty > 0 And si.CasePrice > 0) AndAlso (si.QtyOnHand >= si.CaseQty Or si.AcceptingOrder = 2) Then
                                            p.AddCart += String.Format(strAddCart, "Buy in Bulk", "bt-buy-case bg-more-info", "window.location.href='" & p.Url & "';", si.ItemId, "block")
                                            p.YouSave = String.Empty
                                            p.Price = String.Empty
                                        Else
                                            If Not isQuickOrder Then
                                                p.AddCart += String.Format(strAddCart, "More Info", "bt-add-cart bg-more-info", "window.location.href='" & p.Url & "';", si.ItemId, IIf(p.InCart = False, "block", "none"))
                                            End If

                                            If (si.IsRewardPoints And si.RewardPoints > 0) Then
                                                Dim priceTable As String = String.Format("<table class='itemprice' cellpadding='0' cellspacing='0'><tr><td>Redeem Points: {0}</td></tr><tr><td>{1}</td></tr></table>", Utility.Common.FormatPointPrice(si.RewardPoints), p.Price)
                                                p.Price = priceTable
                                            End If
                                        End If
                                    End If
                                Else
                                    If (isPageItemPoint) Then
                                        p.Price = "<span class='pointPrice'>Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</span'>"
                                    End If

                                    If (si.CaseQty > 0 And si.CasePrice > 0) AndAlso (si.QtyOnHand >= si.CaseQty Or si.AcceptingOrder = 2) Then
                                        p.AddCart += String.Format(strAddCart, "Buy in Bulk", "bt-buy-case bg-more-info", "window.location.href='" & p.Url & "';", si.ItemId, "block")
                                        p.Price = String.Empty
                                        p.YouSave = String.Empty
                                    ElseIf p.Price.Contains("As low as") Then
                                        p.AddCart += String.Format(strAddCart, "More Info", "bt-add-cart bg-more-info", "window.location.href='" & p.Url & "';", si.ItemId, IIf(p.InCart = False, "block", "none"))
                                    Else
                                        If Not isQuickOrder Then
                                            p.AddCart += String.Format(strAddCart, "Add to Cart", "bt-add-cart bg-add-cart", String.Format(strAddCartEvent, si.ItemId), si.ItemId, IIf(p.InCart = False, "block", "none"))
                                        End If

                                        p.AddCart = String.Format("<div class=""qty"" id=""divQty{0}"" style=""display:{2}""><div class=""plus""><a href=""javascript:void(Increase('txtQtyItem{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))"">&ndash;</a></div><div class=""pull-left""><input type=""tel"" class=""txt-qty"" name=""txtQtyItem{0}"" id=""txtQtyItem{0}"" value=""{1}"" onkeypress=""return numbersonly(txtQtyItem{0},event);"" maxlength=""4"" /></div>" &
                                                                        "<div class=""bao-arrow""><ul><li><a href=""javascript:void(Increase('txtQtyItem{0}'))""><b class=""arrow-up arrow-down-act"" id=""imgIntxtQty{0}""></b></a></li>" &
                                                                        "<li class=""bd-qty""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></a></li></ul></div></div>", si.ItemId, IIf(isQuickOrder, "0", "1"), IIf(p.InCart = False, "block", "none")) + p.AddCart
                                    End If
                                End If

                                If (si.CaseQty > 0 And si.CasePrice > 0) AndAlso (si.QtyOnHand >= si.CaseQty Or si.AcceptingOrder = 2) Then
                                    p.InCart = False
                                Else
                                    p.AddCart += String.Format(strInCart, si.ItemId, IIf(p.InCart = True, "block", "none"))
                                End If

                            End If
                        End If

                        lst.Add(p)
                        i += 1
                    Catch ex As Exception
                        strError &= "<br>-------------<br>" & si.ItemId & "<br>" & ex.ToString()
                    End Try

                Next

                If Not String.IsNullOrEmpty(strError) Then
                    Email.SendError("ToError500", "SitePage > GetProductData", "Url: " & HttpContext.Current.Request.RawUrl & "<br>--------------<br>" & strError)
                End If
                Return lst
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function GetCurrentFilterActive(ByVal query As String) As String
            Dim sortBy As String = String.Empty
            Dim currentURL As String = query

            If (currentURL.Contains("&")) Then
                Dim arr() As String = currentURL.Split("&")
                If (arr.Length > 0) Then
                    Dim i As Integer = arr.Length - 1
                    Dim key As String = String.Empty

                    While (i > 0)
                        key = arr(i).ToLower()
                        If (key.Contains("brandid=")) Then
                            Return "brandid"
                        End If
                        If (key.Contains("price=")) Then
                            Return "price"
                        End If
                        If (key.Contains("rating=")) Then
                            Return "rating"
                        End If
                        If (key.Contains("departmentid=")) Then
                            Return "DepartmentId"
                        End If
                        If (key.Contains("collectionid=")) Then
                            Return "collectionid"
                        End If
                        If (key.Contains("toneid=")) Then
                            Return "toneid"
                        End If
                        If (key.Contains("shadeid=")) Then
                            Return "shadeid"
                        End If
                        i = i - 1
                    End While
                End If
            End If
            Return String.Empty
        End Function
        Private Shared Function GetProductDataFilter(ByVal filter As DepartmentFilterFields, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isQuickOrder As Boolean) As IEnumerable
            Dim isInternational As Boolean = False
            Dim ItemCollection As New StoreItemCollection
            StoreItemRow.SetMember(filter, isInternational)
            filter.PromotionId = IIf(GetQueryString("PromotionId") <> Nothing AndAlso IsNumeric(GetQueryString("PromotionId")), GetQueryString("PromotionId"), Nothing)

            If filter.ShopSaveId > 0 Then
                ItemCollection = StoreItemRow.ListByShopSaveId(filter.ShopSaveId, filter)
            ElseIf filter.DepartmentId > 0 And filter.DepartmentTabId > 0 Then
                ItemCollection = StoreItemRow.ListByDepartmentTabId(filter.DepartmentTabId, filter)
            ElseIf filter.DepartmentId > 0 Or filter.BrandId > 0 Then
                Dim currentFlterActive = String.Empty
                If HttpContext.Current.Request.RawUrl.Contains("?") Then
                    Dim queryString = HttpContext.Current.Request.RawUrl.Substring(HttpContext.Current.Request.RawUrl.LastIndexOf("?") + 1)
                    currentFlterActive = GetCurrentFilterActive(queryString)
                End If
                ItemCollection = SolrHelper.SearchItem(filter, PageIndex, PageSize, filter.SortBy, currentFlterActive)
                'ItemCollection = StoreItemRow.ListBySubCategory(filter)
            ElseIf Not String.IsNullOrEmpty(filter.ItemSku) Then
                ItemCollection = StoreItemRow.ListByItemSku(filter.ItemSku, filter)
                Dim count_row As Integer = ItemCollection.Count
            End If
            Return GetProductData(ItemCollection, PageIndex, PageSize, isInternational, isQuickOrder)
        End Function

        <System.Web.Services.WebMethod()>
        Public Shared Function GetProductList(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal DepartmentId As Integer, ByVal DepartmentTabId As Integer, ByVal ShopSaveId As Integer, ByVal BrandId As Integer, ByVal queryString As String) As String
            Dim filter As New DepartmentFilterFields
            If pageIndex < 1 Or pageSize < 1 Then
                Return Nothing
            End If

            If queryString.Length > 1 Then
                For Each s As String In queryString.Split("&")
                    Try
                        Dim arr As String() = s.Split("=")
                        Dim key As String = arr(0)
                        Dim value As String = arr(1).Replace("|", ",")

                        Select Case key
                            Case "SortOrder"
                                filter.SortOrder = value
                            Case "featureid"
                                filter.Feature = value
                            Case "F_Sale"
                                filter.IsOnSale = True
                            Case "F_IsNew"
                                filter.IsNew = True
                            Case "sort"
                                filter.SortBy = value
                            Case "price"
                                filter.PriceRange = value
                            Case "pricehigh"
                                filter.PriceHigh = value
                            Case "rating"
                                filter.RatingRange = value
                            Case "brandid"
                                filter.BrandId = CInt(value)
                            Case "collectionid"
                                filter.CollectionId = CInt(value)
                            Case "shadeid"
                                filter.ShadeId = CInt(value)
                            Case "toneid"
                                filter.ToneId = CInt(value)
                            Case "sku"
                                filter.ItemSku = value
                        End Select
                    Catch ex As Exception

                    End Try
                Next
            End If

            filter.MaxPerPage = pageSize
            filter.pg = pageIndex

            If DepartmentId > 0 Then
                filter.DepartmentId = DepartmentId
            End If
            If DepartmentTabId > 0 Then
                filter.DepartmentTabId = DepartmentTabId
            End If
            If ShopSaveId > 0 Then
                filter.ShopSaveId = ShopSaveId
            End If
            If BrandId > 0 Then
                filter.BrandId = BrandId
            End If

            Dim isQuickOrder As Boolean = HttpContext.Current.Request.Path.Contains("collection.aspx")

            Return Newtonsoft.Json.JsonConvert.SerializeObject(GetProductDataFilter(filter, pageIndex, pageSize, isQuickOrder))

        End Function

        <System.Web.Services.WebMethod()>
        Public Shared Function GetRelatedProduct(ByVal ItemId As Integer, ByVal ItemGroupId As Integer, ByVal VideoId As Integer) As String
            Dim filter As New DepartmentFilterFields
            filter.pg = 1
            filter.MaxPerPage = Integer.MaxValue
            filter.OrderId = Utility.Common.GetOrderIdFromCartCookie()
            filter.MemberId = Utility.Common.GetCurrentMemberId()
            If VideoId > 0 Then
                filter.SortBy = "related.Arrange"
                filter.SortOrder = "ASC"

            End If

            Dim isInternational As Boolean = False
            Dim ItemCollection As New StoreItemCollection
            StoreItemRow.SetMember(filter, isInternational)

            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

            If VideoId > 0 Then
                ItemCollection = StoreItemRow.GetItemByVideoId(DB, VideoId, filter, 0)
            Else
                ItemCollection = StoreItemRow.GetRelatedItemsColection(DB, ItemId, ItemGroupId, filter, 0)
            End If
            Return Newtonsoft.Json.JsonConvert.SerializeObject(GetProductData(ItemCollection, 1, Integer.MaxValue, isInternational, False))
        End Function

        <System.Web.Services.WebMethod()>
        Public Shared Function GetRecentlyView(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal OrderId As Integer, ByVal MemberId As Integer, ByVal SessionId As String) As String
            If pageIndex > 1 Then
                Return Nothing
            End If
            Dim filter As New DepartmentFilterFields
            filter.pg = pageIndex
            filter.MaxPerPage = pageSize
            filter.OrderId = Utility.Common.GetOrderIdFromCartCookie()
            filter.MemberId = Utility.Common.GetCurrentMemberId()

            Dim isInternational As Boolean = False
            Dim ItemCollection As StoreItemCollection = ViewedItemRow.GetRecentlyViewed(OrderId, MemberId, SessionId, "right")
            StoreItemRow.SetMember(filter, isInternational)

            Return Newtonsoft.Json.JsonConvert.SerializeObject(GetProductData(ItemCollection, pageIndex, pageSize, isInternational, False))
        End Function
        Public Shared Function getTextFreeShip(ByVal subTotal As Double) As String
            Dim textFreeShip As String = String.Empty
            Try
                Dim orderPriceMin As Double = SysParam.GetValue("FreeShippingOrderAmount")
                orderPriceMin = orderPriceMin - subTotal

                Dim memberId As Integer = Common.GetCurrentMemberId()
                If memberId > 0 Then
                    If orderPriceMin > 0 Then
                        textFreeShip = String.Format(Resources.Msg.MiniCart_FreeShipNoQualify, FormatCurrency(orderPriceMin))
                    Else
                        textFreeShip = Resources.Msg.MiniCart_FreeShipQualify
                    End If
                Else
                    textFreeShip = Resources.Msg.MiniCart_FreeShipUnLogin
                End If

            Catch ex As Exception
                Email.SendError("ToError500", "SitePage.getTextFreeShip(ByVal subTotal As Double)", ex.ToString())
            End Try
            Return textFreeShip
        End Function
        Public Shared Function BindIconStar(rate As String) As String
            Dim strIcon As String = String.Empty
            Dim halfIcon As String = String.Empty
            If rate.Contains(".5") Then
                halfIcon = "<i class=""fa fa-star-half-o fa-2""></i>"
                rate = rate.Replace(".5", "")
            End If
            For i As Integer = 1 To 5
                If i <= CInt(rate) Then
                    strIcon &= "<i class=""fa fa-star fa-2""></i>"
                Else
                    If i - CInt(rate) = 1 And Not String.IsNullOrEmpty(halfIcon) Then
                        strIcon &= halfIcon
                    Else
                        strIcon &= "<i class=""fa fa-star-o fa-1""></i>"
                    End If
                End If
            Next
            Return strIcon
        End Function
    End Class
    Public Class MetaTag
        Public TypeShare As String
        Public ShareTitle As String
        Public ShareDesc As String
        Public Price As String
        Public ShareURL As String
        Public ImageName As String
        Public ImagePath As String
        Public ImgWidth As Integer
        Public ImgHeight As Integer
        Public VideoFile As String
        Public EmbedUrl As String
        Public PageTitle As String
        Public MetaKeywords As String
        Public MetaDescription As String
        Public Canonical As String
    End Class
End Namespace
