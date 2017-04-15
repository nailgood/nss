Imports System
Imports System.Collections
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Imports DataLayer
Imports System.Web.SessionState
Imports Database
Imports Utility
Imports Components
Imports Components.Core
Imports System.Data.SqlClient
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Text.RegularExpressions
Imports ShippingValidator
Imports System.Security.Cryptography.X509Certificates
Imports System.Net

Namespace Components
    Public Class BaseShoppingCart
        Protected ReadOnly DefaultShippingId As Integer = Utility.Common.DefaultShippingId
        Protected ReadOnly TruckShippingId As Integer = Utility.Common.TruckShippingId
        Protected ReadOnly NonExpeditedShippingIds As String = Utility.Common.NonExpeditedShippingIds
        Protected ReadOnly PickupShippingId As Integer = Utility.Common.PickupShippingId

        Protected m_DB As Database
        Protected Session As HttpSessionState
        Protected m_OrderId As Integer
        Protected m_ConnectionString As String
        Protected m_Order As StoreOrderRow
        Protected m_Context As System.Web.HttpContext
        Protected m_MemberId As Integer
        Protected m_FromAdmin As Boolean
        Protected ProductDiscount As Double
        Protected ParentIdAtt As Integer
        Private m_WebType As Integer
        Private m_TotalFreeAllowed As Integer
        Private m_FreeItemIds As String
        Private Shared m_Zone As Integer
#Region "Constructor"
        Public Sub New(ByVal _webType As Integer, ByVal context As System.Web.HttpContext)
            WebType = _webType
            m_Context = context
            Session = context.Session
        End Sub 'New

        Public Sub New(ByVal _DB As Database, ByVal _webType As Integer, ByVal context As System.Web.HttpContext)
            m_DB = _DB
            WebType = _webType
            m_Context = context
            Session = context.Session
        End Sub 'New

        Public Sub New(ByVal _DB As Database, ByVal _OrderId As String, ByVal _webType As Integer, ByVal context As System.Web.HttpContext)
            m_DB = _DB
            OrderId = _OrderId
            WebType = _webType
            m_Context = context
            Session = context.Session
        End Sub 'New

        Public Sub New(ByVal _DB As Database, ByVal _OrderId As String, ByVal _bFromAdmin As Boolean, ByVal _webType As Integer, ByVal context As System.Web.HttpContext)
            m_DB = _DB
            OrderId = _OrderId
            m_FromAdmin = _bFromAdmin
            WebType = _webType
            m_Context = context
            Session = context.Session
        End Sub 'New
#End Region

#Region "Property"
        Protected Property WebType() As Integer
            Get
                Return m_WebType
            End Get
            Set(ByVal Value As Integer)
                m_WebType = Value
            End Set
        End Property
        Protected Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property
        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property
        Protected Property Context() As System.Web.HttpContext
            Get
                Return m_Context
            End Get
            Set(ByVal Value As System.Web.HttpContext)
                m_Context = Value
            End Set
        End Property
        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property
        Public Property Order() As StoreOrderRow
            Get
                Return m_Order
            End Get
            Set(ByVal Value As StoreOrderRow)
                m_Order = Value
            End Set
        End Property
        Public Shared Property Zone() As Integer
            Get
                Return m_Zone
            End Get
            Set(ByVal Value As Integer)
                m_Zone = Value
            End Set
        End Property
        Protected Property TotalFreeAllowed() As Integer
            Get
                Return m_TotalFreeAllowed
            End Get
            Set(ByVal Value As Integer)
                m_TotalFreeAllowed = Value
            End Set
        End Property
        Protected Property FreeItemIds() As String
            Get
                Return m_FreeItemIds
            End Get
            Set(ByVal Value As String)
                m_FreeItemIds = Value
            End Set
        End Property
#End Region

#Region "Methods"
        Public Sub RecalculateCartFreeItem(ByVal mixmatchId As Integer)
            Dim c As StoreCartItemCollection = StoreCartItemRow.GetListFreeCartItemByMixmatch(DB, Order.OrderId, mixmatchId)
            For Each ci As StoreCartItemRow In c
                FillPricing(DB, ci, False, Common.SalesPriceType.Item)
                If ci.LowSalePrice > 0 AndAlso ci.LowSalePrice < ci.Price Then
                    ci.LineDiscountAmountCust = ci.Price - ci.LowSalePrice
                    ci.CustomerPrice = ci.LowSalePrice
                    ci.LineDiscountAmount = ci.Price - ci.LowSalePrice
                Else
                    ci.LineDiscountAmountCust = Nothing
                    ci.CustomerPrice = Nothing
                    ci.LineDiscountAmount = Nothing
                End If
                'Update Quantity Price
                If Not ci.Pricing.PPU Is Nothing AndAlso ci.Pricing.PPU.Rows.Count > 1 Then
                    Dim dv As New DataView()
                    dv.Table = ci.Pricing.PPU.Copy
                    dv.RowFilter = "minimumquantity <= " & ci.Quantity
                    If dv.Count > 0 Then
                        ci.QuantityPrice = Utility.Common.RoundCurrency(dv(dv.Count - 1)("UnitPrice")) + ci.AttributePrice
                    Else
                        ci.QuantityPrice = Nothing
                    End If
                Else
                    ci.QuantityPrice = Nothing
                End If

                ci.LineDiscountAmount = ci.Price - ci.QuantityPrice
                ''ci.SubTotal = FormatNumber(ci.Price * ci.Quantity, 2)
                If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                    ci.SubTotalPoint = ci.RewardPoints * ci.Quantity
                    ci.Total = 0
                Else
                    ci.SubTotal = FormatNumber(ci.Price * ci.Quantity, 2)
                End If

                If ci.IsFreeItem Then
                    ci.Total = 0
                Else
                    If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                        ci.Total = 0
                    Else
                        ci.Total = FormatNumber(ci.GetLowestPrice * ci.Quantity, 2)
                        If ci.Promotion IsNot Nothing AndAlso (ci.LowSalePrice > 0 And ci.LowSalePrice < ci.GetLowestPrice) Then
                            ci.Total = FormatNumber(ci.LowSalePrice * ci.Quantity, 2)
                        End If

                        If ci.PromotionIsLowestPrice() Then
                            ci.Total = FormatNumber(ci.PromotionPrice * ci.DiscountQuantity + (ci.GetLowestPrice(False) * (ci.Quantity - ci.DiscountQuantity)), 2)
                        End If
                    End If

                End If
                If ci.IsFreeShipping = True And Order.CarrierType = DefaultShippingId Then
                    ci.IsFreeShipping = True
                Else
                    ci.IsFreeShipping = False
                End If
                If ci.PromotionID = Nothing Then
                    ci.LineDiscountAmount = FormatNumber(ci.SubTotal - ci.Total, 2)
                End If
                If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                    ci.LineDiscountAmount = Nothing
                    ci.LineDiscountAmountCust = Nothing
                    ci.CustomerPrice = Nothing
                    ci.Total = Nothing
                    ci.SubTotal = Nothing
                    ci.PromotionPrice = Nothing
                    ci.QuantityPrice = Nothing
                    ci.DiscountQuantity = Nothing
                    ci.LineDiscountPercent = Nothing
                End If
                ci.Update()
            Next
        End Sub

        Public Shared Sub SendOrderConfirmation(ByVal orderId As Integer, Optional ByVal isMobileCheckout As Boolean = False)
            Dim PrefixSubjectMailConfirmation As String = "nss.com order of "
            Dim SuffixSubjectMailConfirmation As String = "and {0} more item(s)"
            Dim DB As Database = New Database
            DB.Open(System.Configuration.ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)

            Dim strTrack As String = String.Empty
            Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, orderId)
            strTrack &= "OrderNo=" & dbOrder.OrderNo
            ''Dim URL As String = Utility.ConfigData.ReferrenceConfirmOrderUrl & "?OrderId=" & System.Web.HttpUtility.UrlEncode(CryptData.Crypt.EncryptTripleDes(dbOrder.OrderId.ToString)) & "&print=y&c=y"
            Dim URL As String = Utility.ConfigData.ReferrenceConfirmOrderUrl & "?OrderId=" & dbOrder.OrderId.ToString & "&print=y&c=y"
            strTrack &= "<br><br>Url=" & URL

            Dim r As System.Net.HttpWebRequest
            If (isMobileCheckout) Then
                r = System.Net.WebRequest.Create(URL + "&mb=1")
            Else
                r = System.Net.WebRequest.Create(URL)
            End If

            r.Method = "GET"
            r.KeepAlive = False
            r.ProtocolVersion = System.Net.HttpVersion.Version10
            r.ServicePoint.ConnectionLimit = 1

            'ServicePointManager.CertificatePolicy = New TrustAllCertificatePolicy()
            'r.Method = "GET"
            r.KeepAlive = False
            r.ProtocolVersion = Net.HttpVersion.Version10
            r.ServicePoint.ConnectionLimit = 1
            'ServicePointManager.MaxServicePoints = 4
            'ServicePointManager.UseNagleAlgorithm = True
            'ServicePointManager.Expect100Continue = True
            'ServicePointManager.CheckCertificateRevocationList = True
            'ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit

            r.Headers.Add("UserAgent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)")

            strTrack &= "<br><br>HttpWebRequest"

            'Dim myCache As New System.Net.CredentialCache()
            'myCache.Add(New Uri(URL), "Basic", New System.Net.NetworkCredential("ameagle", "design"))
            'r.Credentials = myCache
            'strTrack &= "<br><br>myCache"

            Try
                'Get the data as an HttpWebResponse object
                Dim resp As System.Net.HttpWebResponse = r.GetResponse()
                Dim sr As New System.IO.StreamReader(resp.GetResponseStream())
                Dim HTML As String = sr.ReadToEnd()
                strTrack &= "<br><br>ReadToEnd"
                sr.Close()

                HTML = Replace(HTML, "href=""/", "href=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/")
                HTML = Replace(HTML, "src=""/", "src=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/")
                Dim bccConfirm As String = SysParam.GetValue("ToReportOrderConfirmation")
                Dim SiteName As String = System.Configuration.ConfigurationManager.AppSettings("GlobalWebsiteName")
                strTrack &= "<br><br>SiteName=" & SiteName
                strTrack &= "<br><br>HTML=" & HTML
                Dim mailSubject As String = Utility.Common.GetMailOrderConfirmSubject(DB, orderId, PrefixSubjectMailConfirmation, SuffixSubjectMailConfirmation)

                If (Email.SendHTMLMail(FromEmailType.Sales, dbOrder.Email, Core.BuildFullName(dbOrder.BillToName, String.Empty, dbOrder.BillToName2), mailSubject, HTML, bccConfirm)) Then
                    StoreOrderRow.UpdateSendConfirm(DB, orderId, True)
                Else
                    Email.SendError("ToErrorWebService", "BaseShoppingCart.SendConfirmation: " & orderId, "Core.SendHTMLMail send HTML fail" & HTML)
                End If
                SendTrackingOrder(orderId)

                'Clear OrderId Cookie
                Utility.Common.SetOrderToCartCookie(0)
                Dim bGuest As Boolean = MemberRow.UpdateAfterSendOrderConfirmation(dbOrder.MemberId)
                If bGuest Then
                    Dim mem As MemberRow = MemberRow.GetRow(dbOrder.MemberId)
                    If mem.IsActive Then
                        Exit Sub
                    End If

                    Dim sActiveCode As String = Guid.NewGuid().ToString()
                    DB.ExecuteSQL("UPDATE Member SET ActiveCode='" & sActiveCode & "' where memberid = " & dbOrder.MemberId)

                    Dim sName As String = "The Nail Superstore"
                    Dim sSubject As String = "Activate Your Account"
                    Dim strContents As String = String.Empty
                    Dim objReader As IO.StreamReader

                    Dim cus As CustomerRow = mem.Customer

                    Try
                        If mem.GuestStatus > 0 Then
                            objReader = New IO.StreamReader(Utility.ConfigData.ActiveAccountGuestTemplatePath)
                        Else
                            objReader = New IO.StreamReader(Utility.ConfigData.ActiveAccountTemplatePath)
                        End If

                        strContents = objReader.ReadToEnd()
                        objReader.Close()

                        Dim password As String = IIf(mem.Username.Contains("@"), cus.Email.Substring(0, cus.Email.IndexOf("@")), cus.City)
                        strContents = String.Format(strContents, cus.Email, Utility.ConfigData.GlobalRefererName, sActiveCode, password)
                        Email.SendHTMLMail(FromEmailType.NoReply, cus.Email, mem.ContactName, sSubject, strContents)
                    Catch
                    End Try
                End If



            Catch ex As Exception
                Email.SendError("ToError500", "[ShoppingCart] SendConfirmation", strTrack & "<br>Exception: " & ex.ToString())
            End Try
        End Sub

        Public Shared Sub SendTrackingOrder(ByVal orderId As Integer)
            Try
                Dim ds As DataSet = StoreOrderRow.GetOrderTracking(orderId)
                If (ds Is Nothing) Then
                    Exit Sub
                End If

                If (ds.Tables.Count > 1) Then
                    Dim dtOrder As DataTable = ds.Tables(0)
                    Dim dtCartItem As DataTable = ds.Tables(1)
                    Dim orderExport As Boolean = False
                    Try
                        orderExport = CBool(ds.Tables(2).Rows(0)(0).ToString())
                    Catch ex As Exception

                    End Try

                    'Generate HTML Order
                    Dim url As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
                    Dim mailTitle As String = "Tracking Order"
                    If (url <> String.Empty) Then
                        If (url.Contains("test.nss.com") Or url.Contains("new.nss.com")) Then
                            mailTitle = mailTitle & " Test"
                        End If
                    End If

                    Dim orderNo As String = String.Empty
                    Try
                        orderNo = dtOrder.Rows(0)("OrderNo").ToString()
                    Catch ex As Exception
                    End Try

                    mailTitle = mailTitle & ",OrderId:" & orderId & ",OrderNo:" & orderNo
                    Dim orderHTML As String = ConvertDataFromDataTableToHTML(dtOrder, mailTitle, False)
                    Dim cartHTML As String = ConvertDataFromDataTableToHTML(dtCartItem, mailTitle, orderExport)
                    Dim html As String = "<table><tr><td>" & orderHTML & "</td></tr><tr><td><hr /></td></tr><tr><td>" & cartHTML & "</td></tr></table>"
                    Email.SendReport("ToReportPayment", mailTitle, html)
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetShippingMethodPrices(ByVal strZipcode As String, ByVal shipToState As String) As DataTable
            Dim dtShippingPrice As DataTable = Nothing
            Dim i As Integer = 0
            Dim SQL As String
            Dim bOnlyOversizeItems = OnlyOversizeItems()
            Dim isSpecialUS As Boolean = Common.CheckShippingSpecialUS("US", shipToState)

            If isSpecialUS And bOnlyOversizeItems = False Then
                SQL = "select * from shipmentmethod sm where sm.methodid = " & Utility.Common.USPSPriorityShippingId
                dtShippingPrice = DB.GetDataSet(SQL).Tables(0)
                Return dtShippingPrice 'DB.GetDataSet(SQL)

            End If
            If bOnlyOversizeItems Then
                SQL = "select methodid,Name,Code,sortorder,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + strZipcode + "') else Days end as Days from shipmentmethod where  methodid=" & TruckShippingId & " or methodid=" & PickupShippingId & " order by sortorder"
                Return DB.GetDataSet(SQL).Tables(0)
            Else
                SQL = "select methodid,Name,Code,sortorder,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + strZipcode + "') else Days end as Days from shipmentmethod  where Code like '" & Utility.Common.PrefixShippingFEDCode & "%'"
                'bCartItemFlammable = IsFlammableUsInt()

                'Khoa tat: Hazardous Material Fee
                'If bCartItemFlammable Then
                '    SQL &= " and methodid not in (" & Utility.Common.UPSNextDayShippingId & "," & Utility.Common.UPS2DayShippingId & ")"
                'End If
            End If

            Dim IsFedExCalculator As Boolean = Common.IsFedExCalculator()

            If IsFedExCalculator Then
                SQL &= "  and MethodId not in (Select MethodId from ShippingFedExRestricted where '" & strZipcode & "' between LowValue and HighValue)"
            End If
            Dim Conn As String = " and "

            If HasOversizeItems() Then
                SQL = SQL & "  union select methodid,Name,Code,sortorder,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + strZipcode + "') else Days end as Days from shipmentmethod where methodid=" & TruckShippingId
            End If

            If shipToState = "IL" Then
                SQL = SQL & "  union select methodid,Name,Code,sortorder, case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + strZipcode + "') else Days end as Days from shipmentmethod where  methodid=" & PickupShippingId
            End If

            SQL = "Select Methodid,Name,Code,Sortorder,isnull(Days, 0) as Days from ( " & SQL & ") subQ order by sortorder"

            dtShippingPrice = DB.GetDataTable(SQL)
            Return dtShippingPrice
        End Function
        Public Function RecalculateShippingCartPrice(ByVal strCountryID As String, ByVal strZipCode As String, ByVal MethodID As Integer, ByVal orderId As Integer, ByVal shippingCode As String, ByVal weight As Double, ByVal freeWeight As Double, ByVal isFreeShip As Boolean) As Double

            Dim WeightOV As Double = 0
            Dim MethodName As String = String.Empty
            Dim SQL As String
            Dim feeShipOversize As Double = 0
            Dim IsOverSize As Boolean = False
            Dim Shipping As Double = 0

            If MethodID = Utility.Common.TruckShippingId Then
                IsOverSize = True
                WeightOV = WeightFlatFee()
                weight = weight - WeightOV
                feeShipOversize = ShippingOversize(Order)
            Else
                IsOverSize = False
                If MethodID = Utility.Common.DefaultShippingId Then
                    If Not isFreeShip Then
                        weight = weight - freeWeight
                    End If
                End If


            End If

            weight = Math.Round(weight, 3)
            If weight > 0 Then
                If Common.IsFedExCalculator() AndAlso Common.USShippingCode().Contains(shippingCode) Then
                    Shipping = GetFedExRate(DB, weight, MethodID, strZipCode, strCountryID, (Order.ShipToAddressType <> 1))
                    If (Shipping > 0) Then
                        '' Shipping += (Shipping * CInt(SysParam.GetValue("ExtraFedExPercent"))) / 100
                        Dim extraPercent As Integer = DB.ExecuteScalar("Select coalesce(ExtraShippingPercent,0) from ShipmentMethod where MethodId=" & MethodID)
                        Shipping += (Shipping * extraPercent) / 100
                    End If
                Else

                    If MethodID = Utility.Common.TruckShippingId Then
                        SQL = "select top 1 coalesce(case when " & weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(weight - 1) < 1, 0, Math.Ceiling(weight - 1)) & " * additionalpound else case when " & weight & " - additionalthreshold > 0 then (" & weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(strZipCode) & " between lowvalue and highvalue and sm.methodid = " & MethodID.ToString()
                        Shipping = DB.ExecuteScalar(SQL)
                    Else
                        If Utility.ConfigData.EnableUSPSRate Then
                            Dim country As String = ""
                            If (Order.ShipToCountry = "US") Then
                                country = strZipCode
                            Else
                                country = Order.ShipToCountry
                            End If

                            Dim iType As Integer = 0
                            Shipping = GetUSPSRate(weight, country, iType, orderId)
                            If (Shipping > 0) Then
                                Dim ExtraUSPSPercent As Double = SysParam.GetValue("ExtraUSPSPercent")
                                ExtraUSPSPercent = ExtraUSPSPercent / 100
                                Dim ExtraUSPS As Double = Shipping * ExtraUSPSPercent
                                Shipping = Shipping + ExtraUSPS
                            End If
                        Else
                            SQL = "select top 1 coalesce(case when " & weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(weight - 1) < 1, 0, Math.Ceiling(weight - 1)) & " * additionalpound else case when " & weight & " - additionalthreshold > 0 then (" & weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(strZipCode) & " between lowvalue and highvalue and sm.methodid = " & MethodID.ToString()
                            Shipping = DB.ExecuteScalar(SQL)
                        End If
                    End If
                End If
            End If



            Return Shipping
        End Function
        Protected Sub CalculateWeightShipping(ByRef Weight As Double, ByVal MethodId As Integer)
            If OnlyOversizeItems() Then
                Exit Sub
            ElseIf HasOversizeItems() Then
                If MethodId = TruckShippingId Then
                    Exit Sub
                End If
            End If
            If MethodId = DefaultShippingId Then
                If Order.IsFreeShipping Then
                    Weight = 0
                End If
            End If
        End Sub
        Public Sub RecalculateCartSelectShipping()

            'If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") Then
            Dim dv As DataView, drv As DataRowView
            Dim additional As Double = 0
            Dim ci As StoreCartItemRow
            Dim Weight As Double = Nothing
            Dim WeightOV As Double = 0
            Dim _methodId As Integer = 0
            Dim MethodIds As String = String.Empty
            Dim ShippingZero As Boolean = False
            Dim MethodName As String = String.Empty
            Dim Ids As String = String.Empty
            Dim SQL As String
            Dim feeShipOversize As Double = 0
            SQL = "select sm.Code, coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = ca.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype from storecartitem ca inner join shipmentmethod sm on sm.MethodId = ca.carriertype where orderid = " & Order.OrderId & " and  type = 'item' group by carriertype, sm.Code"
            dv = DB.GetDataView(SQL)
            Dim IsOverSize As Boolean = False
            Dim ShippingTotal As Double = 0
            Dim AppliedHazMatFee As Boolean = False
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                Dim Shipping As Double = 0
                _methodId = CInt(drv("carriertype"))
                If _methodId = Utility.Common.TruckShippingId Then
                    IsOverSize = True
                    WeightOV = WeightFlatFee()
                    Weight = drv("weight") - WeightOV
                    feeShipOversize = ShippingOversize(Order)
                Else
                    IsOverSize = False
                    If _methodId = Utility.Common.DefaultShippingId Then
                        If drv("weight") = drv("freeweight") Then
                            Order.IsFreeShipping = True
                        End If
                        If Order.IsFreeShipping Then
                            Weight = drv("weight")
                        Else
                            Weight = drv("weight") - drv("freeweight")
                        End If
                    Else
                        Weight = drv("weight")
                    End If

                End If

                If Weight <= 0 And Order.IsFreeShipping = False Then
                    Order.IsFreeShipping = True
                    Order.Update()
                End If
                Weight = Math.Round(Weight, 3)
                If Ids <> String.Empty Then Ids &= ","
                Ids &= _methodId.ToString()
                ci = StoreCartItemRow.GetRow(DB, OrderId, _methodId)
                ci.Prefix = Nothing
                If OnlyOversizeItems() And _methodId = Utility.Common.TruckShippingId And CheckShippingSpecialUS() Then
                    ci.Prefix = Utility.Common.ShippingTBD
                Else
                    If Weight > 0 Then
                        If Common.IsFedExCalculator() AndAlso Common.USShippingCode().Contains(drv("Code").ToString()) Then
                            Shipping = GetFedExRate(DB, Weight, _methodId, Order.ShipToZipcode, Order.ShipToCountry, (Order.ShipToAddressType <> 1))
                            If (Shipping <= 0) Then
                                ci.Prefix = Utility.Common.ShippingTBD
                            Else

                                Dim extraPercent As Integer = DB.ExecuteScalar("Select coalesce(ExtraShippingPercent,0) from ShipmentMethod where MethodId=" & _methodId)
                                Shipping += (Shipping * extraPercent) / 100
                            End If
                        Else
                            SQL = "select top 1 coalesce(case when " & Weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(Weight - 1) < 1, 0, Math.Ceiling(Weight - 1)) & " * additionalpound else case when " & Weight & " - additionalthreshold > 0 then (" & Weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(IIf(Order.IsSameAddress, Order.BillToZipcode, Order.ShipToZipcode)) & " between lowvalue and highvalue and sm.methodid = " & _methodId.ToString()
                            Shipping = DB.ExecuteScalar(SQL)
                        End If


                    End If
                End If



                If Order.IsFreeShipping AndAlso ci.CarrierType = DefaultShippingId Then
                    ci.FreeShipping = Shipping
                    ci.Total = 0
                    ci.SubTotal = 0
                Else
                    ci.SubTotal = Shipping
                    ci.Total = ci.SubTotal
                End If

                'ci.SubTotal = Shipping
                'ci.Total = IIf(Order.IsFreeShipping AndAlso ci.CarrierType = DefaultShippingId, 0, ci.SubTotal)
                If ci.CartItemId = Nothing Then ci.Insert() Else ci.Update()
                ShippingTotal += ci.Total
            Next
            'end
            Order.Shipping = ShippingTotal
            ' End If

        End Sub
        Public Function RawPriceDiscountAmount() As Double
            Return Order.RawPriceDiscountAmount
        End Function
        Public Function IsEmpty() As Integer
            Try
                Dim result As Integer = DB.ExecuteScalar("Select count(*) From StoreCartItem Where OrderId = " & OrderId & " and Type = 'item'")
                If (result > 0) Then
                    Return False
                End If

            Catch ex As Exception
            End Try
            Return True
        End Function
        Public Function GetCurrentBalancePoint(ByVal pointavailable As Integer) As Integer
            Dim TotalBuyPointBy As Integer = GetTotalBuyPointByOrder(OrderId)
            Dim result As Integer = pointavailable - Order.TotalRewardPoint + TotalBuyPointBy - Order.PurchasePoint
            Return result
        End Function
        Public Function GetTotalBuyPointMoneyByOrder(ByVal OrderId As Integer) As Double
            Return DB.ExecuteScalar("Select isnull(sum(Price),0) from StoreCartItem where [Type]='" & Common.CartItemTypeBuyPoint & "' and OrderId=" & OrderId)
        End Function
        Public Function GetTotalBuyPointByOrder(ByVal OrderId As Integer) As Integer
            Dim buyPointMoney As Integer = GetTotalBuyPointMoneyByOrder(OrderId)
            Dim moneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
            Dim result As Integer = buyPointMoney / moneyEachPoint
            Return result
        End Function
        Public Sub CheckValidItemPoint(ByVal PointAvailable As Integer)
            If (Order Is Nothing) Then
                Exit Sub
            End If
            Dim currentbalance As Integer = GetCurrentBalancePoint(PointAvailable)
            If (currentbalance < 0) Then
                Dim removeItemPoint As Boolean = StoreOrderRow.RemoveItemPointByOrder(DB, Order.OrderId)
                Order.PurchasePoint = 0
                Order.TotalPurchasePoint = 0
                Order.PointMessage = ""
                Order.PointAmountDiscount = 0
                Order.PointLevelMessage = ""
                Order.TotalRewardPoint = DB.ExecuteScalar("Select [dbo].[fc_StoreCartItem_GetSubTotalItemRewardPointPrice](" & Order.OrderId & ")")
                Order.Update()
            End If
        End Sub
        Public Function RemoveItemBuyPoint(ByVal OrderId As Integer, ByVal PointAvailable As Integer) As Boolean
            Dim result As Boolean = StoreCartItemRow.RemoveItemBuyPoint(DB, OrderId)
            CheckValidItemPoint(PointAvailable)
            Return result
        End Function

        Public Function AddBuyPoint2Cart(ByVal ItemId As Integer, ByVal PointAvailable As Integer) As Integer
            If ItemId = 0 Then Throw New ApplicationException("Invalid Parameters")
            Dim dbStoreItem As StoreItemRow = StoreItemRow.GetItemBuyPoint(DB, ItemId)
            Dim dbStoreCartItem As StoreCartItemRow = StoreCartItemRow.GetRowBuyPointByItemId(DB, ItemId, OrderId)
            If (dbStoreCartItem.ItemId = ItemId) Then
                Return dbStoreCartItem.CartItemId
            End If
            dbStoreCartItem.AddType = WebType
            dbStoreCartItem.Type = Common.CartItemTypeBuyPoint
            dbStoreCartItem.Price = dbStoreItem.Price
            dbStoreCartItem.Quantity = 1
            dbStoreCartItem.SKU = dbStoreItem.SKU
            dbStoreCartItem.ItemId = dbStoreItem.ItemId
            dbStoreCartItem.ItemName = dbStoreItem.ItemName
            dbStoreCartItem.SubTotal = dbStoreItem.Price * 1
            dbStoreCartItem.Total = dbStoreCartItem.SubTotal
            dbStoreCartItem.OrderId = OrderId
            dbStoreCartItem.PriceDesc = "Each"
            dbStoreCartItem.DB = DB
            DB.ExecuteSQL("Delete from StoreCartItem where OrderId= " & OrderId & " and Type='" & Utility.Common.CartItemTypeBuyPoint & "'")
            dbStoreCartItem.CartItemId = dbStoreCartItem.Insert()
            ''check valid item point
            CheckValidItemPoint(PointAvailable)
            Utility.Common.DeleteCachePopupCart(Order.OrderId)
            Return dbStoreCartItem.CartItemId
        End Function

        ''' <summary>
        ''' cong so point KH da mua vao tai khoan point cua KH
        ''' </summary>
        Public Sub AddBuyPointFromOrder(ByVal PointAvailable As Integer)
            If (Order Is Nothing) Then
                Exit Sub
            End If
            Dim buyPoint As Integer = GetTotalBuyPointByOrder(Order.OrderId)
            If (buyPoint < 1) Then
                Exit Sub
            End If
            Dim pointEarn As Integer = 0

            pointEarn = buyPoint
            If (pointEarn > 0) Then
                Dim CP As CashPointRow = New CashPointRow(DB)
                CP.PointEarned = pointEarn
                CP.PointDebit = 0
                CP.Notes = "Buy Point from order #" & Order.OrderNo
                CP.MemberId = Order.MemberId
                CP.OrderId = Order.OrderId
                CP.TransactionNo = "BP" & Order.OrderNo
                CP.CreateDate = Now 'Order.CreateDate
                CP.ApproveDate = Now
                CP.Status = 1
                CP.Insert()
            End If
        End Sub

        Public Sub UpdateStatusReferFriendFromOrder(ByVal memberCheckOutId As Integer)
            Dim referCode As String = DB.ExecuteScalar("Select COALESCE(UseReferCode,'') as UseReferCode from Member where MemberId=" & memberCheckOutId)
            If String.IsNullOrEmpty(referCode) Then
                Exit Sub
            End If
            Dim memberReferId As Integer = MemberReferRow.CheckAllowAddPointForUserRefer(DB, memberCheckOutId)
            If (memberReferId < 1) Then
                Exit Sub
            End If
            ''update refer status
            DB.ExecuteSQL("Update MemberRefer set Status=" & Utility.Common.ReferFriendStatus.Order & ",ModifyDate=" & DB.Quote(DateTime.Now) & " where MemberUseRefer=" & memberCheckOutId & " and MemberRefer=" & memberReferId)
        End Sub

        Public Function ValidateOrderId(ByVal OrderNo As String) As Boolean
            Dim OrderId As Integer = DB.ExecuteScalar("SELECT OrderId FROM StoreOrder WHERE OrderId = " & DB.Quote(Session("OrderId")) & " and MemberId = " & DB.Number(Common.GetCurrentMemberId()) & " AND (ProcessDate IS NULL OR (ProcessDate IS NOT NULL AND (PayPalStatus = 3 OR PayPalStatus = 1)))")
            If OrderId = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function IsInCart(ByVal ItemId As Integer) As Boolean
            Return CBool(DB.ExecuteScalar("select case when exists (select ItemId from storecartitem where orderid = " & OrderId & " and itemid = " & ItemId & " and [type] = 'item') then 1 else 0 end"))
        End Function

        Public Function GenerateUniqueOrderId(ByVal MemberId As Integer) As Integer
            If MemberId < 1 Then
                GenerateUniqueOrderId = StoreOrderRow.InsertUniqueOrder(DB, Context.Request.ServerVariables("REMOTE_ADDR"), 0, Session.SessionID)
            Else
                GenerateUniqueOrderId = StoreOrderRow.InsertUniqueOrder(DB, Context.Request.ServerVariables("REMOTE_ADDR"), MemberId, Session.SessionID)
            End If

        End Function

        Public Function GetNextSequenceNo() As Integer
            Dim SQL As String = String.Empty
            SQL = "INSERT INTO StoreSequence (Dummy) VALUES (NULL)"
            GetNextSequenceNo = DB.InsertSQL(SQL)
        End Function
        Public Shared Function CheckTotalFreeSample(ByVal OrderId As Integer) As Boolean
            Dim result As Boolean = False

            Dim SP As String = "sp_CheckTotalFreeSample"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)

            Try
                result = CBool(db.ExecuteScalar(cmd))
            Catch ex As Exception
                Email.SendError("ToError500", "CheckTotalFreeSample", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function
        Public Shared Function GetMinFreeGiftLevelInValid(ByVal OrderId As Integer) As Double
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim result As Double = 0
            Dim cmd As DbCommand = db.GetSqlStringCommand("Select [dbo].[fc_StoreOrder_GetMinFreeGiftLevelInValid](" & OrderId & ")")
            Try
                result = CInt(db.ExecuteScalar(cmd))
            Catch ex As Exception
                Email.SendError("ToError500", "GetMinFreeGiftLevelInValid(" & OrderId & ")", "Exception: " & ex.ToString())
            End Try
            Return result
        End Function

        Public Shared Function CheckIsFirstClassPackage(ByVal OrderId As Integer) As Boolean 'True = Cho phep dung First Class Package
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim result As Boolean = False
            Dim cmd As DbCommand = db.GetSqlStringCommand("Select [dbo].[fc_StoreCartItem_IsFirstClassPackage](" & OrderId & ")")
            Try
                result = CBool(db.ExecuteScalar(cmd))
            Catch ex As Exception
                Email.SendError("ToError500", "CheckIsFirstClassPackage(" & OrderId & ")", "Exception: " & ex.ToString())
            End Try
            Return result
        End Function

        Public Sub DeleteCartItemMixMatch(ByVal ItemId As Integer, ByVal Orderid As Integer)
            DB.ExecuteSQL("DELETE FROM StoreCartitem WHERE IsFreeItem = 1 AND OrderId = " & DB.Quote(Orderid) & " AND ItemId = " & DB.Quote(ItemId))
        End Sub


        'Public Sub RemoveCartItemOnly(ByVal CartItemId As Integer)
        '    Dim SQL As String
        '    SQL = "delete from StoreCartitem where OrderId = " & DB.Quote(OrderId) & " and CartItemId = " & DB.Quote(CartItemId)
        '    Dim totalPoint As Integer = DB.ExecuteScalar("SELECT COALESCE(SUM(SubTotalPoint), 0)  FROM StoreCartItem WHERE [Type] = 'item'  AND OrderId =" & OrderId & "   and IsRewardPoints =1 and RewardPoints>0")
        '    DB.ExecuteSQL("Update StoreOrder set PurchasePoint=" & totalPoint & " where OrderId=" & OrderId)
        '    DB.ExecuteSQL(SQL)
        'End Sub

        Public Function GetListProductReview() As StoreCartItemCollection
            Return StoreCartItemRow.GetListProductReview(DB, OrderId)
        End Function
        Public Function GetCartItems() As StoreCartItemCollection
            Return StoreCartItemRow.GetCartItems(DB, OrderId)
        End Function

        Public Function GetCartItemsForPaypalProcess() As StoreCartItemCollection
            Return StoreCartItemRow.GetCartItemsInPaypalProcess(DB, OrderId)
        End Function

        Public Function GetCartItemCount() As Integer
            Return StoreCartItemRow.ItemCount(Utility.Common.GetCurrentMemberId(), OrderId)
        End Function

        Public Function AllFreeItems() As Boolean
            Dim result As Boolean = True
            Try
                result = DB.ExecuteScalar("SELECT top 1 coalesce(cartitemid, 0) FROM StoreCartItem WHERE [type] = 'item' and isfreeitem = 0 and OrderId=" & OrderId.ToString()) = 0
            Catch ex As Exception
                result = True
            End Try
            Return result
        End Function

        Public Function OnlyOversizeItems() As Boolean
            'Dim b As Boolean = False
            'b = DB.ExecuteScalar("select top 1 coalesce(cartitemid,0) from storecartitem sci where orderid = " & OrderId & " and isoversize = 0 and type = 'item'  and isfreeitem=0") = 0

            'If b Then
            '    b = DB.ExecuteScalar("select count(cartitemid) from storecartitem where orderid = " & OrderId & " and type = 'item'") > 0
            'End If
            'Dim test As Boolean = StoreOrderRow.OnlyOversizeItems(DB, OrderId)
            'If test <> b Then
            '    Dim log As String = ""
            '    log = "OnlyOversizeItems invalid"
            'End If
            Return StoreOrderRow.OnlyOversizeItems(DB, OrderId)
        End Function

        Public Sub RemoveCartItem(ByVal CartItemId As Integer)
            Dim dt As DataTable = DB.GetDataTable("Select ItemId,od.MemberId from StoreCartItem item join StoreOrder od on(od.OrderId=item.OrderId) where CartItemId=" & CartItemId)
            Dim SQL As String

            SQL = "delete from StoreCartitem where OrderId = " & DB.Quote(OrderId) & " and CartItemId = " & DB.Quote(CartItemId)
            DB.ExecuteSQL(SQL)
            'Dim totalPoint As Integer = DB.ExecuteScalar("SELECT COALESCE(SUM(SubTotalPoint), 0)  FROM StoreCartItem WHERE [Type] = 'item'  AND OrderId =" & OrderId & "   and IsRewardPoints =1 and RewardPoints>0")
            'DB.ExecuteSQL("Update StoreOrder set PurchasePoint=" & totalPoint & " where OrderId=" & OrderId)

            SQL = "delete from StoreOrderRecipient where OrderId = " & DB.Quote(OrderId) & " and RecipientId not in (select top 1 RecipientId from StoreCartItem where OrderId = " & DB.Quote(OrderId) & " and RecipientId = StoreOrderRecipient.RecipientId)"
            DB.ExecuteSQL(SQL)
            Dim ItemId As Integer
            Dim MemberId As Integer
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    ItemId = dt.Rows(0)("ItemId")
                    MemberId = dt.Rows(0)("MemberId")
                    ''clear cache
                    ''  CacheUtils.RemoveCacheWithPrefix(String.Format("GetRow_{0}_{1}", ItemId, MemberId))
                    ''GetRowByMemberLogin_
                    CacheUtils.RemoveCache(String.Format(StoreItemRow.cachePrefixKey & "GetRowByMemberLogin_{0}_{1}", ItemId, MemberId))
                End If
            End If

            If CheckWeightCartItem() = 0 Then
                DB.ExecuteSQL("DELETE FROM StoreCartItem WHERE (IsFreeItem = 1 OR IsFreeSample = 1) AND OrderId=" & OrderId)
            End If
            Utility.Common.DeleteCachePopupCart(OrderId)
        End Sub
        Public Sub RemoveItemAttribute(ByVal AttSku As String, ByVal orderid As Integer)
            Try
                If AttSku <> Nothing Then
                    'remove item attribute
                    DB.ExecuteSQL("Delete StoreCartItem Where Sku in(" & AttSku & ") and AttributeSKU is not null and OrderId = " & orderid)
                End If
            Catch ex As Exception

            End Try
        End Sub
        Public Sub RemoveFreeSample()
            DB.ExecuteSQL("delete from StoreCartItem where OrderId = " & DB.Quote(OrderId) & " and IsFreeSample = 1")
        End Sub
        Public Sub RemoveFreeGift()
            DB.ExecuteSQL("delete from StoreCartItem where OrderId = " & DB.Quote(OrderId) & " and IsFreeGift = 1")
        End Sub
        Public Function CheckWeightCartItem() As Double
            Try
                Return DB.ExecuteScalar("Select Sum(Weight) From StoreCartItem Where OrderId = " & OrderId & " and Type = 'item' and IsFreeItem <> 1 and IsFreeSample <> 1")
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Function CheckWeightCartItem(ByVal currentOrderId As Integer) As Double
            Try
                Return DB.ExecuteScalar("Select Sum(Weight) From StoreCartItem Where OrderId = " & currentOrderId & " and Type = 'item' and IsFreeItem <> 1 and IsFreeSample <> 1")
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Function HasFlammableCartItem() As Common.FlammableCart
            Try
                Return CType(DB.ExecuteScalar("SELECT dbo.[fc_StoreCartItem_CheckFlammable](" & OrderId & ")"), Common.FlammableCart)
            Catch ex As Exception
                Return Common.FlammableCart.None
            End Try
        End Function
        Public Function HasCountryHazMat(ByVal CountryCode As String) As Boolean
            Dim list As String = ",AF,AR,AA,AS,AU,BA,BB,BE,BR,BU,CA,CJ,CI,HR,EZ,DA,FI,FR,GM,GJ,HK,HU,EI,IS,IT,JA,KU,KS,LS,LH,LU,MY,MN,NL,NO,RP,PL,PO,PR,QA,SM,SN,LO,SI,SP,SC,ST,VC,SW,SZ,TW,TH,TD,AE,UK,US,VI,HI,AK,"
            If list.Contains("," & CountryCode & ",") Then
                Return True
            End If
            Return False
        End Function

        Public Function IsHazardousMaterialFee(Optional ByVal methodId As Integer = 0) As Boolean
            Dim id As Integer = IIf(methodId > 0, methodId, Order.CarrierType)

            If Common.InternationalShippingId().Contains(id) Or Common.UPSNextDayShippingId() = id Or Common.UPS2DayShippingId() = id Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function CheckShippingValues() As Boolean
            Return DB.ExecuteScalar("SELECT CASE WHEN EXISTS(SELECT TOP 1 RecipientId FROM StoreOrderRecipient WHERE OrderId=" & OrderId & " AND Shipping < 0) Then 0 Else 1 End")
        End Function

        Public Function HasOversizeItems() As Boolean
            Return DB.ExecuteScalar("select count(cartitemid) from storecartitem where isoversize = 1 and orderid = " & OrderId) > 0
        End Function

        Public Function HasExpeditedItems() As Boolean
            Return DB.ExecuteScalar("select count(cartitemid) from storecartitem where type = 'item' and carriertype in " & DB.NumberMultiple(NonExpeditedShippingIds) & " and orderid = " & OrderId) > 0
        End Function
        Public Function GetShippingMethods() As DataSet
            Try

                Dim SQL As String = String.Empty
                If CheckWeightCartItem() > 0 Then
                    Dim zipCode As String = String.Empty
                    If (Order.IsSameAddress) Then
                        zipCode = Order.BillToZipcode.Trim()
                    Else
                        zipCode = Order.ShipToZipcode.Trim()
                    End If
                    zipCode = Common.GetSingleZipcode(zipCode)
                    Dim bOnlyOversizeItems = OnlyOversizeItems()

                    If bOnlyOversizeItems Then
                        SQL = "select methodid,Name,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + zipCode + "') else Days end as Days, sortorder from shipmentmethod where  methodid=" & TruckShippingId & " or methodid=" & PickupShippingId & " order by sortorder"
                        ''SQL = SQL & "  union all select methodid,Name,sortorder from shipmentmethod where  methodid=" & PickupShippingId
                        Return DB.GetDataSet(SQL)
                    Else
                        SQL = "select methodid,Name,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + zipCode + "') else Days end as Days, sortorder from shipmentmethod  where Code like '" & Utility.Common.PrefixShippingFEDCode & "%'"

                        'Khoa tat: Hazardous Material Fee
                        'If IsFlammableUsInt() Then
                        '    SQL &= " and methodid not in (" & Utility.Common.UPSNextDayShippingId & "," & Utility.Common.UPS2DayShippingId & ")"
                        'End If

                    End If

                    If Common.IsFedExCalculator() Then
                        Dim Conn As String = " and "
                        Conn = " and "
                        Dim CurrentRetrictId As String = String.Empty
                        If Not (Session("GetFedExRate") Is Nothing) Then
                            Dim strSession As String = Session("GetFedExRate")
                            Dim arr As String() = strSession.Split("|")
                            Dim restrictZipCode As String = arr(0)
                            If restrictZipCode = zipCode Then
                                If (arr(4) = "1") Then
                                    CurrentRetrictId = arr(1)
                                End If

                            End If
                        End If
                        If (String.IsNullOrEmpty(CurrentRetrictId)) Then
                            SQL &= " and MethodId not in (Select MethodId from ShippingFedExRestricted where '" & zipCode & "' between LowValue and HighValue)"
                        Else
                            SQL &= " and MethodId not in (Select MethodId from ShippingFedExRestricted where '" & zipCode & "' between LowValue and HighValue and MethodId<>" & CurrentRetrictId & ")"
                        End If
                    End If
                    If HasOversizeItems() Then
                        SQL = SQL & "  union select methodid,Name,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + zipCode + "') else Days end as Days, sortorder from shipmentmethod where methodid=" & TruckShippingId
                    End If

                    If Order.ShipToCountry = "US" AndAlso Order.ShipToCounty = "IL" Then
                        SQL = SQL & "  union select methodid,Name,case when methodid = 16 then (select top 1 isnull(ShippingDay, 6) from Zipcode where methodid = 16 and Zipcode = '" + zipCode + "') else Days end as Days, sortorder from shipmentmethod where  methodid=" & PickupShippingId
                    End If

                    SQL = "Select methodid,Name,sortorder, isnull(Days, 0) as Days from ( " & SQL & ") subQ order by sortorder"
                Else
                    SQL = "Select MethodId,Name, isnull(Days, 0) as Days from ShipmentMethod where MethodId = " & DefaultShippingId
                End If

                Return DB.GetDataSet(SQL)
            Catch ex As Exception
                Email.SendError("ToError500", "BaseShoppingCart.GetShippingMethods()", IIf(Order Is Nothing, "Order=Nothing", "Order=Data") & "<br>" & ex.ToString())
                Return Nothing
            End Try
        End Function

        Public Function CheckItemDifferenceAddType(ByVal cartItem As StoreCartItemRow, ByVal addByPoint As Boolean) As Boolean
            If (cartItem Is Nothing) Then
                Return True
            End If
            Dim oldAddType As String = "2" ''1: addbyPointPrice,2: AddByPrice $
            If (cartItem.IsRewardPoints And cartItem.RewardPoints > 0) Then
                oldAddType = "1"
            End If
            If (oldAddType = "1" And addByPoint) Then
                Return False
            End If
            If (oldAddType = "2" And Not addByPoint) Then
                Return False
            End If
            Return True
        End Function


        ''Edit for Product Coupon
        Public Function GetPriceTotal() As Double
            Dim s As String = DB.ExecuteScalar("select coalesce(sum(total), 0) from storecartitem where type = 'item' and carriertype <> " & TruckShippingId & " and orderid = " & OrderId)
            If Not String.IsNullOrEmpty(s) AndAlso s <> "0" Then
                Return CDbl(s)
            Else
                Return 0
            End If
        End Function

        Public Function GetWeightTotal(ByVal IsDefaultShipping As Boolean, Optional ByVal IsFreeShipping As Boolean = False) As Double
            Dim s = DB.ExecuteScalar("SELECT COALESCE(SUM(weight*quantity), 0) FROM StoreCartItem sci WHERE type = 'item' " & IIf(IsDefaultShipping, IIf(IsFreeShipping, "", " AND IsFreeShipping <> 1  AND (SELECT IsFreeShipping FROM StoreItem WHERE ItemId = sci.ItemId) <> 1 "), "") & " and carriertype <> " & TruckShippingId.ToString() & " and orderid = " & OrderId.ToString())
            If Not String.IsNullOrEmpty(s) AndAlso s <> "0" Then
                Return CDbl(s)
            Else
                Return 0
            End If
        End Function

        Public Function GetWeightTruckTotal() As Double
            Dim s As String = DB.ExecuteScalar("select coalesce(sum(weight*quantity), 0) from storecartitem where type = 'item' and carriertype = " & TruckShippingId & " and orderid = " & OrderId)
            If Not String.IsNullOrEmpty(s) AndAlso s <> "0" Then
                Return CDbl(s)
            Else
                Return 0
            End If
        End Function

        Public Sub RemoveZeroQuantityCartItems()
            DB.ExecuteSQL("delete from storecartitem where orderid = " & OrderId & " and quantity = 0")
        End Sub

        Public Function GetAdditionalFreightCharges() As Double
            Dim s As String = DB.ExecuteScalar("select coalesce(sum(additionalshipping), 0) from storecartitem where carriertype = " & TruckShippingId & " and orderid = " & OrderId)
            If Not String.IsNullOrEmpty(s) AndAlso s <> "0" Then
                Return CDbl(s)
            Else
                Return 0
            End If
        End Function

        Public Function GetAmountExempt()
            If Order.BillToCountry = "US" Then
                If (Order.BillToCounty.Contains("IL") AndAlso Order.IsSameAddress) OrElse Order.ShipToCounty.Contains("IL") Then
                    Return 0
                End If
            End If
            Return Order.SubTotal
        End Function

        Public Function SubTotalPuChasePoint() As Double
            Dim SubTotal As Double = 0
            If Order.TotalPurchasePoint > 0 Then
                SubTotal = Order.SubTotal + Order.TotalPurchasePoint
            Else
                SubTotal = Order.SubTotal
            End If
            If Order.PointAmountDiscount > 0 Then
                SubTotal = SubTotal + Order.PointAmountDiscount
            End If
            Return SubTotal
        End Function
        Public Sub RecalculateDiscount()
            Order.TotalDiscount = DB.ExecuteScalar("select coalesce(sum(linediscountamount),0) from storecartitem where type = 'item' and orderid = " & OrderId) - ProductDiscount
        End Sub

        Public Sub RecalculateTotal()
            Order.SubTotal = Order.SubTotal - Order.Discount
            Order.Total = Order.SubTotal + Order.Shipping + Order.Tax + Order.TotalSpecialHandlingFee + Order.HazardousMaterialFee
        End Sub

        Protected Function WeightFlatFee() As Double
            Dim SQL As String
            Dim weight As Double = 0
            Dim dv As DataView
            SQL = "Select si.ItemId, coalesce(sum(sci.weight * sci.quantity),0) as weight, si.sku from StoreCartItem sci inner join storeitem si on sci.itemid= si.itemid where si.isFlatFee = 1 and sci.CarrierType = " & Utility.Common.TruckShippingId & " and Type = 'Item' and orderid= " & OrderId & " group by si.ItemId, si.SKU"
            dv = DB.GetDataView(SQL)
            If dv.Count > 0 Then
                For i As Integer = 0 To dv.Count - 1
                    weight += dv(i)("weight")
                Next
            End If

            If weight > 0 Then
                Email.SendError("ToError500", "[Tracking] BaseShoppingCart.vb > WeightFlatFee", "OrderId = " & OrderId & "<br>Weight=" & weight)
            End If
            Return weight
        End Function

        Protected Function ShippingOversize(ByVal order As StoreOrderRow) As Double
            Dim FeeShippingSate As FeeShippingStateRow
            Dim Sql As String
            Dim dv As DataView, drv As DataRowView
            Dim FeeShipping As Double = 0
            Dim MaxQty As Integer = 0
            Sql = "Select si.ItemId, si.IsFlatFee,FeeShipOversize,si.weight, sci.quantity  from StoreCartItem sci inner join storeitem si on sci.itemid= si.itemid where si.isFlatFee = 1 and sci.CarrierType = " & Utility.Common.TruckShippingId & " and Type = 'Item' and orderid= " & order.OrderId
            dv = DB.GetDataView(Sql)
            If dv.Count > 0 Then

                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)
                    FeeShippingSate = FeeShippingStateRow.GetDetailByStateCode(DB, drv("ItemId"), order.ShipToCounty)
                    If FeeShippingSate.NextItemFeeShipping > 0 Then
                        If FeeShippingSate.FirstItemFeeShipping > 0 Then
                            FeeShipping += FeeShippingSate.FirstItemFeeShipping + ((CInt(drv("Quantity") - 1) * FeeShippingSate.NextItemFeeShipping))
                        ElseIf drv("FeeShipOversize") > 0 Then
                            FeeShipping += CDbl(drv("FeeShipOversize")) + ((CInt(drv("Quantity") - 1) * FeeShippingSate.NextItemFeeShipping))
                        Else
                            FeeShipping += CInt(drv("Quantity")) * FeeShippingSate.NextItemFeeShipping
                        End If
                    Else
                        If FeeShippingSate.FirstItemFeeShipping > 0 Then
                            FeeShipping += FeeShippingSate.FirstItemFeeShipping * CInt(drv("Quantity"))
                        Else
                            FeeShipping += CDbl(drv("FeeShipOversize")) * CInt(drv("Quantity"))
                        End If
                    End If
                Next
            Else
                FeeShipping = 0
            End If
            Return FeeShipping
        End Function


        Public Sub GetPoints(ByVal PointDebit As Integer)
            Dim CP As CashPointRow = New CashPointRow(DB)
            Dim MoneySpend As Double = SysParam.GetValue("MoneySpend")
            Dim GetPoint As Integer = SysParam.GetValue("GetPoint")
            Dim MaxPoint As Integer = SysParam.GetValue("MaxPoint")
            If Order.SubTotal > 0 And GetPoint > 0 Then
                CP.PointEarned = Order.SubTotal / GetPoint
                CP.PointDebit = PointDebit
                CP.Notes = "Cash Point from order #" & Order.OrderNo
                CP.MemberId = Order.MemberId
                CP.OrderId = Order.OrderId
                CP.TransactionNo = Order.OrderNo
                CP.CreateDate = Now 'Order.CreateDate
                CP.Status = 0
                CP.Insert()
            End If
        End Sub

        Protected Sub _CheckSaleTax()
            Order.Tax = 0
            Dim AmountExempt As Double = 0
            Dim TotalNotWeight As Double = 0
            If Order.BillToCountry = "US" AndAlso Not String.IsNullOrEmpty(Order.BillToCounty) Then
                Try
                    If (Order.BillToCounty.Contains("IL") AndAlso Order.IsSameAddress) OrElse Order.ShipToCounty.Contains("IL") Then
                        'SubTotal
                        Dim SubTotal As Double = 0
                        If Order.TotalPurchasePoint > 0 Then
                            SubTotal = Order.SubTotal + Order.TotalPurchasePoint
                        Else
                            SubTotal = Order.SubTotal
                        End If

                        If Order.PointAmountDiscount > 0 Then
                            SubTotal = SubTotal + Order.PointAmountDiscount
                        End If

                        'AmountExempt
                        If Order.BillToCountry = "US" Then
                            AmountExempt = DB.ExecuteScalar("SELECT COALESCE(SUM(Total),0) FROM StoreCartItem WHERE IsTaxFree = 1 AND OrderId = " & OrderId)
                        End If

                        'Long edit for item template
                        'Order.Tax = (SubTotal - AmountExempt) * SysParam.GetValue("SaleTax") / 100
                        TotalNotWeight = DB.ExecuteScalar("select isnull(SUM(total),0) from StoreCartItem where Type='item' and Weight = 0 and OrderId = " & OrderId)

                        If TotalNotWeight > 0 Then
                            Order.Tax = (SubTotal - AmountExempt - TotalNotWeight) * SysParam.GetValue("SaleTax") / 100
                        Else
                            Order.Tax = (SubTotal - AmountExempt) * SysParam.GetValue("SaleTax") / 100
                        End If
                    End If
                Catch ex As Exception
                    'Email.SendError("ToError500", "[Error] Tax=0", "OrderId=" & Order.OrderId & "<br>Order.SubTotal" & Order.SubTotal & "<br>Order.TotalPurchasePoint=" & Order.TotalPurchasePoint & "<br>Order.PointAmountDiscount=" & Order.PointAmountDiscount & "<br>Order.Tax=" & Order.Tax & "<br>SysParam=" & SysParam.GetValue("SaleTax") & "Exception" & ex.ToString())
                End Try
            End If

        End Sub
        Public Shared Sub AwardedPoint(ByVal DB As Database, ByVal customerNo As String, ByVal memberId As Integer)
            Dim CP As CashPointRow = New CashPointRow(DB)
            CP.MemberId = memberId
            CP.PointEarned = SysParam.GetValue("AwardedPoint")
            CP.Notes = "Member Register"
            CP.CreateDate = Date.Now
            CP.ApproveDate = Date.Now
            CP.Status = 1
            CP.TransactionNo = "R" & customerNo
            CP.Insert()
        End Sub
        Public Sub LoadLevelMember(ByVal o As StoreOrderRow)
            Exit Sub
            'If o.MemberId <> 0 And Not MemberInGroupWHS() = True Then
            '    Dim Message As String = ""
            '    Dim PercentDiscount As Integer = 0

            '    Dim dt As DataTable = LevelPointRow.GetDiscount1(o.MemberId, Utility.Common.GetDateForLevelPoint(DB, o.MemberId))

            '    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            '        PercentDiscount = dt.Rows(0)("Discount")
            '        Message = dt.Rows(0)("Description")
            '    End If

            '    If PercentDiscount > 0 Then
            '        Dim currentSubtotal = o.SubTotal - o.Discount
            '        o.PointAmountDiscount = Math.Round((currentSubtotal) * PercentDiscount / 100, 2)
            '        o.SubTotal = currentSubtotal - o.PointAmountDiscount
            '        o.PointLevelMessage = Message
            '    Else
            '        ''o.SubTotal = o.SubTotal + o.PointAmountDiscount
            '        o.PointAmountDiscount = 0
            '        o.PointLevelMessage = ""
            '    End If

            'End If
        End Sub

        Public Function CheckShippingSpecialUS() As Boolean
            If Not Order Is Nothing Then
                Return Utility.Common.CheckShippingSpecialUS(Order)
            Else
                Return False
            End If
        End Function

        Function GetZipCodeSpecialUS(ByVal strZipCode As String) As String
            Dim dbZipCode As ZipCodeRow = ZipCodeRow.GetRow(DB, strZipCode)
            If dbZipCode.StateCode = "PR" Then
                strZipCode = "PR"
            ElseIf dbZipCode.StateCode = "VI" Then
                strZipCode = "VI"
            ElseIf dbZipCode.StateCode = "HI" Then
                strZipCode = "HI"
            ElseIf dbZipCode.StateCode = "AK" Then
                strZipCode = "AK"
            ElseIf dbZipCode.StateCode = "AP" Then
                strZipCode = "AP"
            ElseIf dbZipCode.StateCode = "AE" Then
                strZipCode = "AE"
            ElseIf dbZipCode.StateCode = "AA" Then
                strZipCode = "AA"
            End If

            Return strZipCode
        End Function

        ''' ''''''''''''''''''''''''''''''''''''''''''''''''
        Public Function GetFreeShipping() As Boolean
            Dim SQL As String = ""
            Dim count As Integer = 0
            ''Dim dt As DataTable
            'SQL = "select * from storecartitem where isfreeshipping=1 and promotionid is not null and promotionid<>0 and orderid = " & Order.OrderId
            ''SQL = "select * from storecartitem where isfreeshipping=1 and orderid = " & Order.OrderId
            ''dt = DB.GetDataTable(SQL)
            SQL = "select count(*) from storecartitem where isfreeshipping=1 and orderid = " & Order.OrderId
            count = DB.ExecuteScalar(SQL)
            If count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetUPSRate() As Double
            Return 0
        End Function

        Public Function GetFedExRate() As Double
            Return 0
        End Function

        Public Sub CalculateTax()
            Order.Tax = 0

            If Order.BillToCountry = "US" Then
                If (Order.BillToCounty = "IL" AndAlso Order.IsSameAddress) OrElse Order.ShipToCounty = "IL" Then
                    Dim SubTotal As Double = 0
                    Order.Tax = (SubTotalPuChasePoint() - GetAmountExempt()) * SysParam.GetValue("SaleTax") / 100
                End If
            End If
        End Sub
        Function CheckItemAttribute(ByVal ItemId As Integer, ByVal CartItemId As Integer, ByVal orderid As Integer) As Boolean
            Try

                Dim AttSKU As String
                Try
                    If CartItemId = 0 Then
                        AttSKU = DB.ExecuteScalar("Select sku from storecartitem where itemid = " & ItemId & " and orderid = " & orderid & " and (AttributeSku like 'Discount%' or AttributeSku = 'FREE')")
                    Else
                        AttSKU = DB.ExecuteScalar("Select sku from storecartitem where itemid = " & ItemId & " and orderid = " & orderid & " and CartItemId = " & CartItemId & " and (AttributeSku like 'Discount%' or AttributeSku = 'FREE')")
                    End If
                Catch ex As Exception
                    AttSKU = ""
                End Try
                If AttSKU <> "" Then
                    Try
                        ParentIdAtt = DB.ExecuteScalar("Select ItemId from StoreAttribute where sku like '%" & AttSKU & "%'")
                    Catch ex As Exception
                        ParentIdAtt = 0
                    End Try
                    If ParentIdAtt <> 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return (False)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Sub ProcessProductCoupon()
            Dim SQL As String = ""
            'The primary DataView for looping through promotions
            Dim dv As DataView
            Dim FreeItemsId As Integer = Nothing
            Dim dtStoPro As DataTable
            Dim sci As StoreCartItemRow
            Dim si As StoreItemRow
            'Add Free items if a promotion is satisfied items in cart
            'SQL = "select * from Vie_ProductCoupon,storecartitem where Vie_ProductCoupon.itemid = storecartitem.itemid and sci.orderid = " & OrderId
            SQL = "select cartitemid,promotionid from storecartitem where itemid in (select itemid from storeitem where promotionid is not null) and promotionid is not null and orderid = " & OrderId

            dv = DB.GetDataView(SQL)
            'If no promotions, remove all free items
            If dv.Count = 0 Then
                DB.ExecuteSQL("update storecartitem set promotionid = null, CouponPrice = 0 where orderid = " & OrderId)
            Else
                'Loop through promotions
                For i As Integer = 0 To dv.Count - 1
                    FreeItemsId = dv(i)("cartitemid")
                    'Load the free item
                    sci = StoreCartItemRow.GetRow(DB, FreeItemsId)
                    si = StoreItemRow.GetRow(DB, sci.ItemId)
                    dtStoPro = StorePromotionRow.GetStorePromotionRow(DB, dv(i)("promotionid"))
                    If dtStoPro.Rows.Count = 0 Then
                        DB.ExecuteSQL("update storecartitem set promotionid = null, CouponPrice = 0,couponmessage=null, IsFreeShipping = " & CInt(si.IsFreeShipping) & " where cartitemid=" & FreeItemsId & " and orderid = " & OrderId)
                        'Else
                        '    sci.Total = sci.Total - sci.CouponPrice
                        '    sci.Update()
                    End If

                Next

            End If
        End Sub
        Public Sub ProcessItemUpdateIsFreeShipping()
            Dim SQL As String = ""
            Dim dv As DataView
            Dim FreeItemsId As Integer = Nothing
            Dim sci As StoreCartItemRow
            Dim si As StoreItemRow
            Dim pro As StorePromotionRow
            SQL = "select cartitemid from storecartitem where Type = 'item' and orderid = " & OrderId
            dv = DB.GetDataView(SQL)
            If dv.Count > 0 Then
                For i As Integer = 0 To dv.Count - 1
                    FreeItemsId = dv(i)("cartitemid")
                    sci = StoreCartItemRow.GetRow(DB, FreeItemsId)
                    si = StoreItemRow.GetRow(DB, sci.ItemId)
                    If sci.PromotionID > 0 And sci.CarrierType = DefaultShippingId Then
                        pro = StorePromotionRow.GetRow(DB, sci.PromotionID)
                        If si.IsFreeShipping = True Then
                            DB.ExecuteSQL("update storecartitem set  IsFreeShipping = " & CInt(si.IsFreeShipping) & " where cartitemid=" & FreeItemsId & " and orderid = " & OrderId)
                        Else
                            DB.ExecuteSQL("update storecartitem set  IsFreeShipping = " & CInt(pro.IsFreeShipping) & " where cartitemid=" & FreeItemsId & " and orderid = " & OrderId)
                        End If

                    Else
                        DB.ExecuteSQL("update storecartitem set  IsFreeShipping = " & CInt(si.IsFreeShipping) & " where cartitemid=" & FreeItemsId & " and orderid = " & OrderId)
                    End If

                Next
            End If

        End Sub


        Public Function GetProductCouponMessage(ByVal Coupon As String) As String
            Dim memberId As Integer = Common.GetCurrentMemberId()
            If memberId <= 0 Then
                memberId = 0
            End If

            Dim dbStoPro As StorePromotionRow = StorePromotionRow.GetRow(DB, Coupon)
            Dim itemId As Integer = DB.ExecuteScalar("Select ItemId from StoreItem where PromotionId=" & dbStoPro.PromotionId)
            Dim dbStoreItem As StoreItemRow = StoreItemRow.GetRow(DB, itemId)
            Dim dbStoreCartItem As StoreCartItemRow = StoreCartItemRow.GetCartItem(DB, Order.OrderId, itemId, False, False)
            Dim cartItemId As Integer = 0
            If Not dbStoreCartItem Is Nothing Then
                cartItemId = dbStoreCartItem.CartItemId
                'Else
                '    Return "Although you've entered a valid promo code " & Coupon & ", your order does not currently meet the code's usage criteria."
            End If
            Dim Msg As String = ""
            Dim isRemoveFreeItem As Boolean = False
            Dim isValid As Boolean = StorePromotionRow.ValidateProductCoupon(DB, dbStoPro, Coupon, Msg, memberId, cartItemId)
            If isValid = False Then
                isRemoveFreeItem = True
            End If
            If (String.IsNullOrEmpty(Msg)) Then
                ''check valid group
                isRemoveFreeItem = True
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Dim couponGroupId As Integer = DB.ExecuteScalar("Select coalesce (CustomerPriceGroupId,0) from StorePromotion where PromotionCode='" & Coupon & "'")
                If couponGroupId > 0 And couponGroupId <> customerPriceGroupId Then
                    Return "The promotion code you entered does not exist"
                    ''Exit Function
                End If
            End If
            If isRemoveFreeItem Then
                DB.ExecuteSQL("Delete from StoreCartItem where MixMatchId=" & dbStoPro.MixmatchId & " and IsFreeItem=1")
            End If
            Return Msg
        End Function


        Public Function AddProductCoupon(ByVal Coupon As String) As String
            Dim memberId As Integer = Common.GetCurrentMemberId()
            If memberId <= 0 Then
                memberId = 0
            End If


            Dim dbStoPro As StorePromotionRow = StorePromotionRow.GetRow(DB, Coupon)
            Dim itemId As Integer = DB.ExecuteScalar("Select ItemId from StoreItem where PromotionId=" & dbStoPro.PromotionId)
            Dim dbStoreItem As StoreItemRow = StoreItemRow.GetRow(DB, itemId)
            Dim dbStoreCartItem As StoreCartItemRow = StoreCartItemRow.GetCartItem(DB, Order.OrderId, itemId, False, False)
            Dim cartItemId As Integer = 0
            If Not dbStoreCartItem Is Nothing Then
                cartItemId = dbStoreCartItem.CartItemId
                'Else
                '    Return "Although you've entered a valid promo code " & Coupon & ", your order does not currently meet the code's usage criteria."
            End If
            Dim Msg As String = ""
            Dim isAddFreeItemCoupon As Boolean = False
            If StorePromotionRow.ValidateProductCoupon(DB, dbStoPro, Coupon, Msg, memberId, cartItemId) = True Then
                System.Web.HttpContext.Current.Session("PromotionAddID") = dbStoPro.PromotionId
                dbStoreCartItem.PromotionID = dbStoPro.PromotionId
                dbStoreCartItem.CouponMessage = Msg
                If dbStoPro.IsProductCoupon Then
                    dbStoreCartItem.MixMatchId = dbStoPro.MixmatchId
                End If
                If Order.IsFreeShipping = False Then
                    If dbStoreItem.IsFreeShipping = False Then
                        dbStoreCartItem.IsFreeShipping = dbStoPro.IsFreeShipping
                    Else
                        dbStoreCartItem.IsFreeShipping = dbStoreItem.IsFreeShipping
                    End If
                Else
                    dbStoreCartItem.IsFreeShipping = Order.IsFreeShipping
                End If
                isAddFreeItemCoupon = True




            End If
            If Not dbStoreCartItem Is Nothing Then
                If dbStoPro.PromotionType = "Monetary" Then
                    dbStoreCartItem.CouponPrice = dbStoPro.Discount
                    If dbStoreCartItem.SubTotal < 0 Then dbStoreCartItem.SubTotal = 0

                ElseIf dbStoPro.PromotionType = "Percentage" Then
                    Dim disPrice As Double
                    Try
                        disPrice = DB.ExecuteScalar("Select sp.UnitPrice From StoreItem si inner join SalesPrice sp on si.ItemId = sp.ItemId where si.ItemId = " & dbStoreCartItem.ItemId & " and sp.MinimumQuantity <= " & dbStoreCartItem.Quantity & " order by MinimumQuantity desc")
                    Catch ex As Exception
                        disPrice = 0
                    End Try
                    If disPrice > 0 Then
                        dbStoreCartItem.CouponPrice = disPrice * (dbStoPro.Discount / 100)
                    Else
                        dbStoreCartItem.CouponPrice = dbStoreCartItem.Price * (dbStoPro.Discount / 100)
                    End If

                    If dbStoreCartItem.SubTotal < 0 Then dbStoreCartItem.SubTotal = 0

                End If
                dbStoreCartItem.Update()
                If isAddFreeItemCoupon And dbStoreCartItem.MixMatchId > 0 Then
                    Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                    AddFreeItem(dbStoreCartItem.MixMatchId, customerPriceGroupId, memberId)
                End If
            End If

            If (String.IsNullOrEmpty(Msg)) Then
                ''check valid group
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Dim couponGroupId As Integer = DB.ExecuteScalar("Select coalesce (CustomerPriceGroupId,0) from StorePromotion where PromotionCode='" & Coupon & "'")
                If couponGroupId > 0 And couponGroupId <> customerPriceGroupId Then
                    Return "The promotion code you entered does not exist"
                    ''Exit Function
                End If
            End If
            Return Msg
        End Function
        Public Function RemoveProductCoupon(ByVal Coupon As String) As String
            StoreCartItemRow.RemoveProductCoupon(DB, OrderId, Coupon)
            Return String.Empty
        End Function


        Public Function CoupontDiscount() As Double
            Try
                If ProductDiscount = 0 Then
                    ProductDiscount = Order.TotalProductDiscount
                End If
                Return Order.Discount + ProductDiscount
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Function CheckCarrier() As Boolean
            Dim result As Integer = DB.ExecuteScalar("Select isnull(CarrierType,0) From StoreCartItem Where Type = 'carrier' and OrderId = " & Order.OrderId)
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Sub RemoveCouponAndRecalculateAllOrderValues(ByVal Coupon As String, ByVal order As StoreOrderRow)
            If Not order Is Nothing Then
                Dim Pro As StorePromotionRow = StorePromotionRow.GetRow(DB, Coupon)
                If Pro.PromotionId > 0 Then
                    If Pro.IsProductCoupon = True Then 'remove coupon code item
                        RemoveProductCoupon(Coupon)
                    Else ' remove coupon code order
                        order.PromotionCode = Nothing
                        order.IsFreeShipping = False
                        order.SubTotal = order.SubTotal + order.Discount
                        order.Discount = 0
                        DB.ExecuteSQL("Delete from StoreCartItem where IsFreeItem=1 and PromotionId=" & Pro.PromotionId & " and OrderId=" & order.OrderId)
                    End If
                    order.Update()
                End If
            End If
        End Sub
        Protected Sub RecalculateOrderBaseSubTotal(ByVal o As StoreOrderRow, ByVal ProductDiscount As Double)
            'Dim result = DB.ExecuteScalar("SELECT [dbo].[fc_StoreCartItem_GetSubTotalItemMemberPrice](" & o.OrderId & ")")
            'o.BaseSubTotal = result

            Dim dv As DataView
            dv = DB.GetDataView("SELECT [dbo].[fc_StoreCartItem_GetSubTotalItemMemberPrice](" & o.OrderId & ") AS SubTotal, COALESCE(SUM(LineDiscountAmount),0) AS TotalDiscount FROM StoreCartItem WHERE [Type] = 'item'  AND OrderId = " & o.OrderId)
            If dv.Count > 0 Then
                o.BaseSubTotal = dv(0)("SubTotal")
                o.TotalDiscount = dv(0)("TotalDiscount") - ProductDiscount
            End If
            '' o.SubTotal = o.SubTotal - o.Discount
        End Sub
        Protected Sub RecalculateOrderTotal(ByVal o As StoreOrderRow, ByVal ProductDiscount As Double)
            Dim dv As DataView
            o.Discount = 0
            o.PointAmountDiscount = 0
            dv = DB.GetDataView("SELECT [dbo].[fc_StoreCartItem_GetSubTotalItemMemberPrice](" & o.OrderId & ") AS SubTotal,[dbo].[fc_StoreCartItem_GetSubTotalItemRewardPointPrice](" & o.OrderId & ") AS SubTotalPoint, [dbo].[fc_StoreCartItem_GetTotalItemMemberPrice](" & o.OrderId & ") AS Total, COALESCE(SUM(LineDiscountAmount),0) AS TotalDiscount FROM StoreCartItem WHERE [Type] = 'item'  AND OrderId = " & o.OrderId)
            If dv.Count > 0 Then
                o.BaseSubTotal = dv(0)("SubTotal")
                o.TotalRewardPoint = dv(0)("SubTotalPoint")
                If (o.PurchasePoint < 1) Then
                    o.PurchasePoint = 0
                    o.TotalPurchasePoint = 0
                Else
                    Dim MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
                    o.TotalPurchasePoint = o.PurchasePoint * MoneyEachPoint
                End If
                o.SubTotal = dv(0)("Total") - o.TotalPurchasePoint
                'Long: Split Coupon Discount (28/08/2012)
                o.TotalDiscount = dv(0)("TotalDiscount") - ProductDiscount
            End If
            '' Order.SubTotal = Order.SubTotal - Order.Discount
        End Sub
        Public Sub RecalculateOrderSignatureConfirmation(ByVal o As StoreOrderRow, ByVal isUS As Boolean)
            If isUS Then
                Dim isUPS As Boolean = False
                If o.CarrierType = Utility.Common.TruckShippingId Then
                    isUPS = False
                Else
                    isUPS = True
                End If
                Dim Signature As Double = 0
                If CheckWeightCartItem(o.OrderId) > 0 Then
                    Dim Sql As String = "select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType)  " & vbCrLf &
                            "where orderid = " & o.OrderId & " and  type = 'item'  and Code in(" & Utility.Common.USShippingCode & ")"
                    Signature = ShipmentMethod.GetValue(DB.ExecuteScalar(Sql), Utility.Common.ShipmentValue.Signature)
                End If
                If (o.ShipToAddressType = 2 Or o.ShipToAddressType = 0) And Signature > 0 And isUPS = True Then
                    If o.IsSignatureConfirmation = False Then
                        o.SignatureDeclineCommnent = "Without Signature Confirmation, FedEx driver will drop off your package(s) by your front door. We will not take any responsibility in case your package(s) are stolen."
                        o.IsSignatureConfirmation = False
                        o.SignatureConfirmation = 0

                    Else
                        o.SignatureDeclineCommnent = ""
                        o.IsSignatureConfirmation = True
                        o.SignatureConfirmation = Signature

                    End If
                Else
                    o.IsSignatureConfirmation = False
                    o.SignatureConfirmation = 0
                    o.ResidentialFee = 0

                End If
            Else
                o.IsSignatureConfirmation = False
                o.SignatureConfirmation = 0
                o.SignatureDeclineCommnent = ""
                o.ResidentialFee = 0
                o.ShipmentInsured = 0
                o.TotalSpecialHandlingFee = 0
            End If

        End Sub


        Protected Sub RecalculateShippingInternational()
            If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = Nothing) OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = Nothing) Then Exit Sub
            Dim CountryCode As String = IIf(Order.IsSameAddress, Order.BillToCountry, Order.ShipToCountry)

            If CheckShippingSpecialUS() Then
                If Order.BillToCounty = "AE" And Order.ShipToCountry = "US" Then
                    CountryCode = "APE"
                ElseIf (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") Then
                    CountryCode = Order.ShipToCounty
                ElseIf Order.BillToZipcode = "VI" Or Order.BillToZipcode = "PR" Then
                    CountryCode = Order.ShipToCounty
                End If
            End If

            Dim dt As DataTable = Nothing
            Dim ShippingCode As String = ""
            If (Order.BillToCounty = "HI" And Order.ShipToCountry = "US") Or (Order.BillToCounty = "AK" And Order.ShipToCountry = "US") Or (Order.BillToCounty = "AP" And Order.ShipToCountry = "US") Or (Order.BillToCounty = "AA" And Order.ShipToCountry = "US") Then
                ShippingCode = Order.BillToCounty
            ElseIf Order.BillToCounty = "AE" And Order.ShipToCountry = "US" Then
                ShippingCode = "APE"
            Else
                Dim sc As String = ShippingRangeRow.GetShippingCode(DB, CountryCode)
                If String.IsNullOrEmpty(sc) Then
                    Exit Sub
                Else
                    ShippingCode = sc
                End If
            End If

            Dim dv As DataView, drv As DataRowView
            Dim additional As Double = 0
            Dim ci As StoreCartItemRow
            Dim Weight As Double = Nothing
            Dim MethodId As Integer = Nothing
            Dim Ids As String = String.Empty
            Dim SQL As String
            Dim AlreadyAppliedLiftGate As Boolean = False
            Dim AlreadyAppliedScheduledDelivery As Boolean = False
            Dim AlreadyAppliedInsideDelivery As Boolean = False
            Dim AppliedHazMatFee As Boolean = False

            'Update Item/OverSize
            SQL = String.Format("DELETE StoreCartItem WHERE [Type] = 'carrier' AND OrderId = {1}; " _
                                & " UPDATE StoreCartItem SET CarrierType={0} WHERE OrderId = {1} AND IsOversize<>1 and [Type]='item'; " _
                                & "UPDATE StoreCartItem SET CarrierType={2}, LiftGateCharge=0, InsideDeliveryCharge=0,ScheduleDeliveryCharge=0,IsLiftGate=0,IsInsideDelivery=0, IsScheduleDelivery = 0  WHERE IsOversize=1 AND [Type]='item' AND OrderId = {1} " _
                                , Utility.Common.USPSPriorityShippingId, OrderId, Utility.Common.TruckShippingId)
            DB.ExecuteSQL(SQL)
            Order.CarrierType = Utility.Common.USPSPriorityShippingId

            'Khoa: Update Item FREE GIFT/FREE ITEM with OnlyOversizeItems
            If OnlyOversizeItems() Then
                SQL = "UPDATE storecartitem SET carriertype=" & Utility.Common.TruckShippingId & " WHERE IsFreeItem=1 AND type='item' AND orderid = " & OrderId
                DB.ExecuteSQL(SQL)
            End If

            SQL = "SELECT coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight FROM storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = storecartitem.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype from storecartitem where orderid = " & Order.OrderId & " and type = 'item'  group by carriertype"
            dv = DB.GetDataView(SQL)
            Dim ShippingTotal As Double = 0
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                If CheckShippingSpecialUS() = False Then
                    Weight = drv("weight") - drv("freeweight")
                    MethodId = drv("carriertype")
                    If drv("carriertype") = Utility.Common.TruckShippingId Then
                        Weight = 0
                    Else
                        If OnlyOversizeItems() Then
                            Weight = 0
                        End If
                    End If
                Else
                    If HasOversizeItems() = False Then
                        Weight = drv("weight") - drv("freeweight")
                        MethodId = drv("carriertype")
                    Else
                        If drv("carriertype") = Utility.Common.USPSPriorityShippingId Then ''15 Then
                            Weight = drv("weight")
                            ShippingCode = Order.ShipToCounty
                            MethodId = drv("carriertype")
                            If OnlyOversizeItems() Then
                                Weight = 0
                            End If
                        Else
                            Weight = drv("weight")
                            MethodId = drv("carriertype")
                            If Common.CheckShippingSpecialUS(Order.ShipToCountry, Order.ShipToCounty) Then
                                If Order.ShipToCounty = "VI" Or Order.ShipToCounty = "PR" Then
                                    ShippingCode = "00000"
                                End If

                                If MethodId = Utility.Common.TruckShippingId Then ' And HasOversizeItems() Da co check o phia tren
                                    Weight = 0
                                End If
                            Else
                                ShippingCode = Order.ShipToZipcode
                            End If
                            If OnlyOversizeItems() Then
                                Weight = 0
                            End If
                        End If
                    End If
                End If

                Dim Shipping As Double = 0
                ci = StoreCartItemRow.GetRow(DB, OrderId, MethodId)

                If Weight > 0 Then
                    If Utility.ConfigData.EnableUSPSRate Then
                        Dim country As String = ""
                        If (Order.ShipToCountry = "US") Then
                            country = Order.ShipToCounty
                        Else
                            country = Order.ShipToCountry
                        End If

                        Dim iType As Integer = 0
                        Shipping = GetUSPSRate(Weight, CountryCode, iType, OrderId)
                        'Check condition and change USPS Priority to First Class Package
                        If MethodId = Common.USPSPriorityShippingId() AndAlso iType = 1 AndAlso Weight <= ConfigData.FirstClassLimitWeight() Then
                            MethodId = Common.FirstClassShippingId()
                            ci.CarrierType = MethodId
                            Order.CarrierType = MethodId
                        End If

                        If (Shipping > 0) Then
                            Dim ExtraUSPSPercent As Double = SysParam.GetValue("ExtraUSPSPercent")
                            ExtraUSPSPercent = ExtraUSPSPercent / 100
                            Dim ExtraUSPS As Double = Shipping * ExtraUSPSPercent
                            Shipping = Shipping + ExtraUSPS
                        End If
                    Else
                        SQL = "select top 1 coalesce(case when " & Weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(Weight - 1) < 1, 0, Math.Ceiling(Weight - 1)) & " * additionalpound else case when " & Weight & " - additionalthreshold > 0 then (" & Weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(ShippingCode) & " between lowvalue and highvalue and sm.methodid = " & drv("carriertype")
                        Shipping = DB.ExecuteScalar(SQL)
                    End If
                End If

                If Ids <> String.Empty Then Ids &= ","
                Ids &= MethodId

                SQL = "select coalesce(sum(rushdeliverycharge),0) from storecartitem where isrushdelivery = 1 and orderid = " & OrderId & " and carriertype = " & MethodId
                Shipping += DB.ExecuteScalar(SQL)

                SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and isliftgate = 1"
                If Not AlreadyAppliedLiftGate AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                    Shipping += SysParam.GetValue("LiftGateCharge")
                    AlreadyAppliedLiftGate = True
                End If

                SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and IsInsideDelivery = 1"
                If Not AlreadyAppliedInsideDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                    Shipping += SysParam.GetValue("InsideDeliveryService")
                    AlreadyAppliedInsideDelivery = True
                End If

                SQL = "select top 1 cartitemid from storecartitem where isscheduledelivery = 1 and orderid = " & OrderId
                If Not AlreadyAppliedScheduledDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                    Shipping += SysParam.GetValue("ScheduleDeliveryCharge")
                    AlreadyAppliedScheduledDelivery = True
                End If

                Shipping = Utility.Common.RoundCurrency(Shipping)
                ci.SubTotal = Shipping
                ci.Total = Shipping

                If Order.ShipmentInsured Then
                    Dim Insurance As Double = StoreOrderRow.GetShippingInsurance(DB, OrderId, MethodId)
                    ci.SubTotal += Insurance
                    ci.Total += Insurance
                End If

                If ci.CartItemId = Nothing Then
                    If ci.Insert() Then
                    End If
                Else
                    ci.Update()
                End If

                ShippingTotal += ci.Total
            Next

            SQL = String.Format("UPDATE StoreCartItem SET CarrierType={0} WHERE OrderId = {1} AND [Type]='item' AND CarrierType = {3}; UPDATE StoreOrder SET CarrierType={0} WHERE OrderId = {1}; DELETE FROM StoreCartItem WHERE OrderId = {1} and [Type]='carrier' and CarrierType NOT IN {2}", MethodId, OrderId, DB.NumberMultiple(Ids), Common.USPSPriorityShippingId)
            DB.ExecuteSQL(SQL)

            Order.Shipping = ShippingTotal
            If Order.CarrierType <> TruckShippingId AndAlso OnlyOversizeItems() Then Order.CarrierType = TruckShippingId
        End Sub
        Public Function RecalculateCartSelectShippingInternational(ByVal strCountryID As String, ByVal strZipcode As String, ByVal strMethodId As String, ByVal bEstimate As Boolean) As String
            If Not bEstimate Then
                Email.SendError("ToError500", "RecalculateCartSelectShippingInternational >> Tracking no estimate?", "Page: " & Context.Request.Url.ToString() & "<br><br>" & "OrderId = " & Order.OrderId & GetSessionList())
            End If

            Dim dv As DataView, drv As DataRowView
            Dim additional As Double = 0
            Dim ci As StoreCartItemRow
            Dim Weight As Double = Nothing
            Dim WeightOV As Double = 0
            Dim feeShipOversize As Double = 0
            Dim MethodId As Integer = Nothing
            Dim Ids As String = String.Empty
            Dim SQL As String
            Dim AlreadyAppliedLiftGate As Boolean = False
            Dim AlreadyAppliedScheduledDelivery As Boolean = False
            Dim AlreadyAppliedInsideDelivery As Boolean = False
            Dim AppliedHazMatFee As Boolean = False

            SQL = "select coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = storecartitem.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype "
            SQL += " from storecartitem where orderid = " & Order.OrderId & " and type = 'item' "
            SQL += " group by carriertype"
            dv = DB.GetDataView(SQL)

            'Check zip code cua state HI, AK, VI, PR
            'If Not String.IsNullOrEmpty(strZipcode) Then
            '    Dim strZipcodeNum As String = strZipcode

            '    If strZipcode.Length = 5 Then
            '        Dim i As Integer = 0
            '        i = CInt(strZipcode)
            '        Dim dbZipCode As ZipCodeRow = ZipCodeRow.GetRow(DB, strZipcode)
            '        strZipcode = GetZipCodeSpecialUS(strZipcode)
            '    End If
            '    Dim strZipcodeCode As String = strZipcode
            'End If

            Dim ShippingTotal As Double = 0
            Dim IsFreeShipping As Boolean = False
            For i As Integer = 0 To dv.Count - 1
                drv = dv(i)
                If strCountryID <> "US" Then
                    MethodId = drv("carriertype")
                Else
                    If CheckShippingSpecialUS() = False Then
                        Weight = drv("weight") - drv("freeweight")
                        MethodId = drv("carriertype")
                    Else
                        If HasOversizeItems() = False Then
                            If MethodId = drv("carriertype") Then
                                Weight = drv("weight") - drv("freeweight")
                            Else
                                Weight = 0
                                MethodId = drv("carriertype")
                            End If
                        Else
                            Weight = drv("weight")
                            MethodId = drv("carriertype")
                        End If
                    End If
                End If

                If strMethodId = DefaultShippingId Then
                    Weight = drv("weight") - drv("freeweight")
                    CalculateWeightShipping(Weight, MethodId)
                Else
                    If Not Utility.Common.InternationalShippingId.Contains(MethodId) And HasOversizeItems() = True Then
                        Weight = 0
                    Else
                        Weight = drv("weight")
                    End If
                End If

                If HasOversizeItems() = True Then
                    If drv("carriertype") = strMethodId Then
                        MethodId = drv("carriertype")
                        If MethodId = Utility.Common.TruckShippingId Then
                            WeightOV = WeightFlatFee()
                            Weight = drv("weight") - drv("freeweight") - WeightOV
                            feeShipOversize = ShippingOversize(Order)
                        Else
                            Weight = drv("weight") - drv("freeweight")
                        End If
                        CalculateWeightShipping(Weight, MethodId)

                    ElseIf Not Utility.Common.InternationalShippingId.Contains(MethodId) Then
                        Weight = 0
                        MethodId = drv("carriertype")
                    End If
                End If

                If OnlyOversizeItems() Then
                    If CheckShippingSpecialUS() Then
                        Weight = 0
                    End If
                    If strCountryID <> "US" Then
                        Weight = 0
                    End If
                End If

                If Ids <> String.Empty Then Ids &= ","
                Ids &= MethodId

                Dim Shipping As Double = 0
                If Weight > 0 Then
                    If Utility.ConfigData.EnableUSPSRate Then
                        Dim iType As Integer = 0
                        Shipping = GetUSPSRate(Weight, strCountryID, iType, OrderId)
                        If (Shipping > 0) Then
                            Dim ExtraUSPSPercent As Double = SysParam.GetValue("ExtraUSPSPercent")
                            ExtraUSPSPercent = ExtraUSPSPercent / 100
                            Dim ExtraUSPS As Double = Shipping * ExtraUSPSPercent
                            Shipping = Shipping + ExtraUSPS
                        End If
                    Else
                        Dim ShippingCode As String = strCountryID
                        SQL = "select top 1 coalesce(case when " & Weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(Weight - 1) < 1, 0, Math.Ceiling(Weight - 1)) & " * additionalpound else case when " & Weight & " - additionalthreshold > 0 then (" & Weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(ShippingCode) & " between lowvalue and highvalue and sm.methodid = " & MethodId
                        Shipping = DB.ExecuteScalar(SQL)
                    End If
                End If

                If Not bEstimate Then
                    ci = StoreCartItemRow.GetRow(DB, OrderId, MethodId)
                    ci.SubTotal = Shipping
                    ci.Total = Shipping
                    If ci.CartItemId = Nothing Then ci.Insert() Else ci.Update()
                End If

                ShippingTotal += Shipping
            Next

            Return ShippingTotal.ToString()
        End Function

#End Region
#Region "NSS"
        Public Const LoginToViewPrices As String = "Please <a href=""/members/login.aspx"" class=""maglnk"">login</a> to see prices."

        Public Shared Function Inventory(ByVal Status As String, ByRef BODate As String, ByVal AcceptingOrder As Integer, ByVal QtyOnHand As Integer, ByVal IsSpecialOrder As Boolean, ByVal LowStockThreshold As Integer, ByVal LowStockMsg As String) As String
            Dim strInventory As String = ""
            Select Case Status
                Case "BD"
                    If BODate <> Nothing Then
                        strInventory = "<span class=""red rtpad"">Expected in stock on " & Convert.ToDateTime(BODate).ToShortDateString & "</span>"
                    Else
                        strInventory = "<span class=""red rtpad"">Backordered</span>"
                    End If
                Case "BO"
                    strInventory = "<span class=""red rtpad"">Backordered</span>"
                Case "DC"
                    strInventory = "<span class=""red bold rtpad"">Discontinued</span>"
                Case Else
                    If AcceptingOrder = Utility.Common.ItemAcceptingStatus.None And Not IsSpecialOrder Then
                        If QtyOnHand <= 0 Then
                            strInventory = "<span class=""red"">Out of Stock</span>"
                        Else
                            If (QtyOnHand > 0) Then
                                strInventory = ""
                                If LowStockThreshold > 0 AndAlso QtyOnHand <= LowStockThreshold Then
                                    strInventory &= "<span class=""red"">"
                                    If LowStockMsg <> Nothing Then
                                        strInventory &= LowStockMsg.Replace("[QTY]", QtyOnHand)
                                    Else
                                        strInventory &= SysParam.GetValue("HurryMessage").Replace("[QTY]", QtyOnHand)
                                    End If
                                    strInventory &= "</span>"
                                ElseIf LowStockThreshold = 0 AndAlso SysParam.GetValue("HurryMessageThreshold") <> Nothing AndAlso QtyOnHand <= SysParam.GetValue("HurryMessageThreshold") Then
                                    strInventory &= "<span class=""red"">"
                                    If LowStockMsg <> Nothing Then
                                        strInventory &= LowStockMsg.Replace("[QTY]", QtyOnHand)
                                    End If
                                    strInventory &= SysParam.GetValue("HurryMessage").Replace("[QTY]", QtyOnHand)
                                    strInventory &= "</span>"
                                End If

                            End If
                        End If
                    End If
            End Select
            Return strInventory
        End Function
        Public Shared Function IsItemMultiPrice(ByVal itemId As Integer, ByVal memberId As Integer) As Boolean
            Dim dv As DataView = StoreOrderRow.FillPricing(itemId, memberId)
            If Not dv Is Nothing Then
                If (dv.Count > 0) Then
                    For i As Integer = 0 To dv.Count - 1
                        If (CInt(dv(i)("minimumquantity")) > 1) Then
                            Return True
                        End If
                    Next
                End If
            End If
            Return False
        End Function
        Public Shared Function DisplayListPricing(ByVal DB As Database, ByVal si As StoreItemRow, ByVal IsVertical As Boolean, ByVal Quantity As Integer, ByVal LineDiscountAmount As Double, ByVal memberId As Integer, ByVal IsList As Boolean) As String
            If si Is Nothing Then Return ""
            If si.Pricing Is Nothing Then FillPricing(DB, si, False, Common.SalesPriceType.Item)
            If si.Pricing Is Nothing Then Return LoginToViewPrices

            Dim s, stmp As String
            Dim Prefix As String = ""
            Dim LowestPrice As Double = si.LowPrice
            Dim tmpPrice As Double = Nothing

            Dim dt As DataTable = si.Pricing.PPU

            If Not dt Is Nothing Then
                If dt.Rows.Count > 1 Then
                    If (System.Web.HttpContext.Current.Request.Path.ToLower = "/store/item.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/includes/ajax.aspx") And IsList = False Then 'OrElse HttpContext.Current.Request.Path.ToLower = "/store/recent.aspx"
                        s = "<div class="""">" & vbCrLf &
                         "<table cellspacing=""0"" cellpadding=""0"" border=""0"" class=""lnpadbt3"">" & vbCrLf &
                         "<tr>" & vbCrLf

                        For i As Integer = 0 To dt.Rows.Count - 1
                            Dim qty As Integer = dt.Rows(i)("minimumquantity")
                            s &= "<td class=""ppuhead"">"
                            If i = dt.Rows.Count - 1 Then
                                s &= qty & "+"
                            Else
                                s &= qty & "-" & dt.Rows(i + 1)("minimumquantity") - 1
                            End If
                            s &= "</td>"
                        Next

                        s &= "</tr>" & vbCrLf

                        For i As Integer = 0 To dt.Rows.Count - 1
                            s &= "<td class=""pputd"">" & FormatCurrency(dt.Rows(i)("unitprice")) & "</td>"
                        Next

                        s &= "</table></div></div>" & vbCrLf
                    ElseIf (IsItemMultiPrice(si.ItemId, memberId)) Then
                        s = "<div class=""mag"" style=""font-weight:normal;"">As low as <strong>" & FormatCurrency(dt.Rows(dt.Rows.Count - 1)("unitprice")) & "</strong></div>"
                    Else
                        '' s = FormatCurrency(dt.Rows(0)("unitprice") * Quantity)
                        s = "<span class=""pputd2"" style=""font-weight:bold"">" & FormatCurrency(dt.Rows(0)("unitprice") * Quantity) & "</span>"
                    End If
                    Return s
                Else
                    Prefix = "<b>Our Price:</b> "
                End If
            End If

            If si.Pricing.IsRangedPricing Then
                If si.LowPrice <> si.HighPrice Then
                    If si.Pricing.LowSellPrice < si.LowPrice Then
                        Return Prefix & "<span class=""strike pputd2"">" & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span><br /><span class=""red bold"">" & FormatCurrency(si.Pricing.LowSellPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span>"
                    Else
                        Return Prefix & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice)
                    End If
                Else
                    Return Prefix & FormatCurrency(si.LowPrice)
                End If
            End If

            If (si.LowPrice <> si.LowSalePrice AndAlso si.LowSalePrice <> Nothing) OrElse LineDiscountAmount > 0 Then
                s = "<span class=""strike pputd2"">" & FormatCurrency(si.Pricing.BasePrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(si.Pricing.SellPrice * Quantity) & "</span>"
                LowestPrice = si.LowSalePrice
            Else
                s = "<span class=""pputd2"" style=""font-weight:bold"">" & FormatCurrency((si.LowPrice - LineDiscountAmount) * Quantity) & "</span>"
            End If

            If si.MixMatchId <> Nothing Then
                Dim Promotion As PromotionRow
                If Not si.Promotion Is Nothing Then
                    Promotion = si.Promotion
                Else
                    Promotion = PromotionRow.GetRow(DB, si.MixMatchId, False)
                End If

                If Promotion.Type = PromotionType.LineSpecific Then
                    Dim dv As DataView = New DataView
                    dv.Table = Promotion.GetItems.Table.Copy
                    dv.RowFilter = "ItemId = " & si.ItemId

                    stmp = String.Empty

                    If dv.Count <> 0 AndAlso Promotion.LinesToTrigger = Promotion.[Optional] Then
                        stmp &= "<span class=""strike"">" & FormatCurrency(si.LowPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">"
                        If Not IsDBNull(dv(0)("SetPrice")) Then
                            tmpPrice = dv(0)("SetPrice")
                            stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
                        ElseIf Not IsDBNull(dv(0)("PercentOff")) Then
                            tmpPrice = si.LowPrice * (1 - (dv(0)("PercentOff") / 100))
                            stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
                        End If
                        stmp &= "</span>"
                        If tmpPrice < LowestPrice Then
                            LowestPrice = tmpPrice
                            s = stmp
                        End If
                    End If
                ElseIf Promotion.LinesToTrigger = Promotion.Optional Then
                    LowestPrice = si.LowPrice
                    If LineDiscountAmount > 0 Then
                        s = "<span class=""strike"">" & FormatCurrency(LowestPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(LowestPrice * Quantity - LineDiscountAmount) & "</span>"
                    Else
                        s = FormatCurrency(LowestPrice * Quantity)
                    End If
                End If
            End If

            Return Prefix & s
        End Function
        'FillPricing for list item
        Public Shared Sub FillPricing(ByVal DB As Database, ByRef o As Object, ByRef isMultiPrice As Boolean, ByVal st As Common.SalesPriceType)
            Dim Item As Object
            Dim SalesType As Common.SalesPriceType = st

            'Neu la StoreItem lay gia tri tu Parameter
            If TypeOf o Is StoreItemRow Then
                Item = CType(o, StoreItemRow)
            End If

            'Neu la StoreCartItem lay gia tri tu db CartItem
            If TypeOf o Is StoreCartItemRow Then
                Item = CType(o, StoreCartItemRow)
                SalesType = IIf(Item.AddType = 1, Common.SalesPriceType.Item, Common.SalesPriceType.Case)
            End If

            If Item Is Nothing Then Exit Sub
            If Not Item.Pricing Is Nothing Then Exit Sub

            Dim LowestPrice As Double = FormatNumber(Item.LowPrice, 2)
            Item.Pricing = New ItemPricing()
            Dim dv As DataView = StoreOrderRow.FillPricing(Item.ItemId, Common.GetCurrentMemberId(), SalesType)
            Dim drv As DataRowView
            Dim HasMember As Boolean = False
            Dim HasGroup As Boolean = False
            For i As Integer = 0 To dv.Count - 1
                If Not IsDBNull(dv(i)("MemberId")) Then
                    HasMember = True
                    Exit For
                ElseIf Not IsDBNull(dv(i)("CustomerPriceGroupId")) Then
                    HasGroup = True
                End If
            Next

            If HasMember Then
                For i As Integer = dv.Count - 1 To 0 Step -1
                    If IsDBNull(dv(i)("MemberId")) Then dv(i).Delete()
                Next
            ElseIf HasGroup Then
                For i As Integer = dv.Count - 1 To 0 Step -1
                    If IsDBNull(dv(i)("CustomerPriceGroupId")) Then dv(i).Delete()
                Next
            End If

            If dv.Count > 0 Then
                isMultiPrice = dv.Count > 1
                Dim dt As DataTable = New DataTable("PPU")
                dt.Columns.Add("unitprice", GetType(Double))
                dt.Columns.Add("minimumquantity", GetType(Integer))
                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)
                    Dim dr As DataRow
                    If i = 0 AndAlso drv("minimumquantity") > 1 Then
                        dr = dt.NewRow
                        dr("unitprice") = Item.price
                        dr("minimumquantity") = 1
                        dt.Rows.Add(dr)
                    End If
                    dr = dt.NewRow
                    dr("unitprice") = drv("unitprice")
                    dr("minimumquantity") = drv("minimumquantity")
                    dt.Rows.Add(dr)
                Next
                If dt.Rows.Count > 1 Then
                    Item.Pricing.PPU = dt
                End If
            End If

            If Item.ItemGroupId <> Nothing AndAlso (System.Web.HttpContext.Current.Request.Path = "/store/default.aspx" Or System.Web.HttpContext.Current.Request.Path = "/home.aspx") Then
                If Item.LowPrice <> Item.HighPrice Then
                    Item.Pricing.IsRangedPricing = True
                    Item.Pricing.LowPrice = Item.LowPrice
                    Item.Pricing.HighPrice = Item.HighPrice
                End If
                If Item.LowPrice <> Item.LowSalePrice AndAlso Item.LowSalePrice <> Nothing Then
                    Item.Pricing.LowSellPrice = Item.LowSalePrice
                Else
                    Item.Pricing.LowSellPrice = Item.LowPrice
                End If
                If Item.HighPrice <> Item.HighSalePrice AndAlso Item.HighSalePrice <> Nothing Then
                    Item.Pricing.HighSellPrice = Item.HighSalePrice
                Else
                    Item.Pricing.HighSellPrice = Item.HighPrice
                End If
                Item.Pricing.BasePrice = LowestPrice
                Item.Pricing.SellPrice = LowestPrice

            Else
                Item.Pricing.BasePrice = Item.Price
            End If

            If Item.MixMatchId <> Nothing Then
                Dim Promotion As PromotionRow
                If Not Item.Promotion Is Nothing Then
                    Promotion = Item.Promotion
                Else
                    Promotion = PromotionRow.GetRow(DB, Item.MixMatchId, False)
                End If

                If Promotion.Type = PromotionType.LineSpecific Then
                    dv = New DataView
                    dv.Table = Promotion.GetItems.Table.Copy
                    dv.RowFilter = "ItemId = " & Item.ItemId

                    Dim tmpPrice As Double
                    If dv.Count <> 0 AndAlso Promotion.LinesToTrigger = Promotion.[Optional] Then
                        If Not IsDBNull(dv(0)("SetPrice")) Then
                            tmpPrice = dv(0)("SetPrice")
                        ElseIf Not IsDBNull(dv(0)("PercentOff")) Then
                            tmpPrice = Item.LowPrice * (1 - (dv(0)("PercentOff") / 100))
                        End If
                        If tmpPrice < LowestPrice Then
                            LowestPrice = FormatNumber(tmpPrice, 2)
                        End If
                    End If
                    Item.Pricing.IsMixMatchPromotion = True
                Else
                    LowestPrice = FormatNumber(Item.LowPrice, 2)
                    Item.Pricing.IsMixMatchPromotion = True
                End If
                'Item.Pricing.PPU = Nothing
                'Trung Nguyen modify
                If Item.LowPrice <> Item.LowSalePrice AndAlso Item.LowSalePrice <> Nothing Then
                    LowestPrice = Utility.Common.RoundCurrency(Item.LowSalePrice)
                End If
            Else
                If Item.LowPrice <> Item.LowSalePrice AndAlso Item.LowSalePrice <> Nothing Then
                    LowestPrice = Utility.Common.RoundCurrency(Item.LowSalePrice)
                End If
            End If

            Item.Pricing.SellPrice = LowestPrice
            If TypeOf Item Is StoreCartItemRow AndAlso Item.IsFreeItem Then Item.Pricing.SellPrice = 0
        End Sub

        Public Function Add2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String) As Integer
            Return Add2Cart(ItemId, RegistryItemId, Quantity, DepartmentId, Label, String.Empty, String.Empty, Nothing, String.Empty)
        End Function
        Public Function AddRewardPoint2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String) As Integer
            Return AddRewardPoint2Cart(ItemId, RegistryItemId, Quantity, DepartmentId, Label, String.Empty, String.Empty, Nothing, String.Empty)
        End Function

        ''''''''''''''''
        Public Function Add2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String, ByVal Attributes As String, ByVal AttributeSKU As String, ByVal ExtraPrice As Double, ByVal Swatches As String) As Integer
            Return Add2Cart(ItemId, RegistryItemId, Quantity, DepartmentId, Label, Attributes, AttributeSKU, ExtraPrice, Swatches, False, False, Nothing, False)
        End Function
        Public Function AddRewardPoint2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String, ByVal Attributes As String, ByVal AttributeSKU As String, ByVal ExtraPrice As Double, ByVal Swatches As String) As Integer
            Return Add2Cart(ItemId, RegistryItemId, Quantity, DepartmentId, Label, Attributes, AttributeSKU, ExtraPrice, Swatches, False, False, Nothing, True)
        End Function

        ''''''''''
        Public Function Add2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String, ByVal Attributes As String, ByVal AttributeSKU As String, ByVal ExtraPrice As Double, ByVal Swatches As String, ByVal IsFree As Boolean, ByVal IsPromo As Boolean, ByVal MixMatchId As Integer) As Integer
            Return Add2Cart(ItemId, RegistryItemId, Quantity, DepartmentId, Label, Attributes, AttributeSKU, ExtraPrice, Swatches, IsFree, IsPromo, MixMatchId, False)
        End Function
        Public Function AddRewardPoint2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String, ByVal Attributes As String, ByVal AttributeSKU As String, ByVal ExtraPrice As Double, ByVal Swatches As String, ByVal IsFree As Boolean, ByVal IsPromo As Boolean, ByVal MixMatchId As Integer) As Integer
            Return Add2Cart(ItemId, RegistryItemId, Quantity, DepartmentId, Label, Attributes, AttributeSKU, ExtraPrice, Swatches, IsFree, IsPromo, MixMatchId, True)
        End Function
        '''''''

        Public Function Add2Cart(ByVal ItemId As Integer, ByVal RegistryItemId As Integer, ByVal Quantity As Integer, ByVal DepartmentId As Integer, ByVal Label As String, ByVal Attributes As String, ByVal AttributeSKU As String, ByVal ExtraPrice As Double, ByVal Swatches As String, ByVal IsFreeItem As Boolean, ByVal IsPromo As Boolean, ByVal MixMatchId As Integer, ByVal addByPoint As Boolean) As Integer
            If ItemId = 0 Then Throw New ApplicationException("Invalid Parameters")
            Dim dbStoreItem As StoreItemRow
            Dim dbStoreCartItem As StoreCartItemRow
            Dim type As Integer = Order.CarrierType 'UPS by default
            Dim point As Integer = 0
            Dim adminPrice As Double = 0

            'Check order NO
            If Not String.IsNullOrEmpty(Order.OrderNo) Then
                Dim err As String = "ItemId:" & ItemId & "<br>OrderId:" & Order.OrderId & "<br>OrderNo:" & Order.OrderNo & "<br>MemberId:" & MemberId
                Email.SendError("ToError500", "Add Cart Cookie Error-" & DateTime.Now.ToString(), err)
                Throw New ApplicationException(err)
            End If

            'Now add cart
            If Label = "ItemAtt" Then
                dbStoreCartItem = StoreCartItemRow.GetRow(DB, OrderId, ItemId, AttributeSKU, Swatches, IsFreeItem, IsPromo, IIf(Label.Contains("Case"), 2, 1))
            ElseIf Label = "IsFreeSample" Then
                dbStoreCartItem = StoreCartItemRow.GetRow(DB, OrderId, ItemId, "IsFreeSample", Swatches, IsFreeItem, IsPromo, IIf(Label.Contains("Case"), 2, 1))
            ElseIf addByPoint Then
                dbStoreCartItem = StoreCartItemRow.GetRow(DB, OrderId, ItemId, "IsItemPoint", Swatches, IsFreeItem, IsPromo, IIf(Label.Contains("Case"), 2, 1))
            ElseIf Label.Contains("Admin-") Then 'Insert Cart from Admin
                Dim arrCart As String() = Label.Split("-")
                adminPrice = arrCart(2)
                OrderId = arrCart(1)
                dbStoreCartItem = StoreCartItemRow.GetRow(DB, OrderId, ItemId, "Admin", Swatches, IsFreeItem, IsPromo, IIf(Label.Contains("Case"), 2, 1))
            ElseIf IsFreeItem AndAlso MixMatchId > 0 Then
                dbStoreCartItem = StoreCartItemRow.GetFreeCartItemByMixMamatch(DB, OrderId, ItemId, MixMatchId)
            Else
                dbStoreCartItem = StoreCartItemRow.GetRow(DB, OrderId, ItemId, Attributes, Swatches, IsFreeItem, IsPromo, IIf(Label.Contains("FreeGift"), True, False), IIf(Label.Contains("Case"), 2, 1))

                'Check MixMatch and FREE Gift same item
                If dbStoreCartItem.IsFreeGift And Not Label.Contains("FreeGift") Then
                    dbStoreCartItem.CartItemId = Nothing
                End If
            End If

            Dim customerPriceGroupId As Integer = 0
            If Not Label.Contains("Case") Then
                If Not Label.Contains("FreeGift") Then
                    customerPriceGroupId = Utility.Common.GetCurrentCustomerPriceGroupId()
                    If MixMatchId < 1 AndAlso Not addByPoint Then
                        MixMatchId = DB.ExecuteScalar("Select [dbo].[fc_StoreItem_GetMixMatchIdByItem](" & dbStoreCartItem.ItemId & "," & customerPriceGroupId & ",1) --BaseShoppingCart.Add2Cart>ItemId:" & ItemId)
                    End If

                    If Not MixMatchId = Nothing Then
                        dbStoreCartItem.MixMatchId = MixMatchId
                    End If
                Else
                    dbStoreCartItem.CartItemId = StoreCartItemRow.GetCartItemIdFreeGift(DB, OrderId)
                End If
            End If


            dbStoreItem = StoreItemRow.GetRow(DB, ItemId)
            point = dbStoreItem.RewardPoints
            If (dbStoreCartItem.CartItemId = Nothing Or CheckItemDifferenceAddType(dbStoreCartItem, addByPoint)) Then
                dbStoreCartItem = New StoreCartItemRow(DB, dbStoreItem)
                If dbStoreItem.IsOversize Then
                    type = TruckShippingId
                    dbStoreCartItem.AdditionalType = "DeliveryByAppointment"
                    dbStoreCartItem.AdditionalShipping = 0
                End If

                If dbStoreItem.IsHazMat Then
                    If Array.IndexOf(NonExpeditedShippingIds.Split(","), type.ToString) > -1 AndAlso Array.IndexOf(NonExpeditedShippingIds.Split(","), Order.CarrierType.ToString) > -1 Then type = DefaultShippingId
                End If

                dbStoreCartItem.IsFreeShipping = dbStoreItem.IsFreeShipping
            Else
                Quantity += dbStoreCartItem.Quantity
                If Label = "ItemAtt" OrElse CheckItemAttribute(ItemId, 0, OrderId) = True Then
                    ExtraPrice = dbStoreCartItem.AttributePrice
                Else
                    ExtraPrice = 0
                End If
            End If

            With dbStoreCartItem
                .MixMatchId = MixMatchId
                If Label = "ItemAtt" Then
                    If AttributeSKU = "FREE" And ExtraPrice = 0 Then
                        .Price = 0
                    Else
                        .Price = ExtraPrice
                    End If
                ElseIf Label = "IsFreeSample" Then
                    .Price = 0
                    Quantity = 1
                    .IsFreeSample = True
                ElseIf Label = "Case" Then
                    'If dbStoreCartItem.PriceDesc.Contains("of") AndAlso (dbStoreCartItem.PriceDesc.Contains("Case") Or dbStoreCartItem.PriceDesc.Contains("Box") Or dbStoreCartItem.PriceDesc.Contains("Pack") Or dbStoreCartItem.PriceDesc.Contains("Set")) Then
                    '    Try
                    '        .PriceDesc = String.Format("Case of {0}x{1}", dbStoreItem.CaseQty, dbStoreItem.PriceDesc.Substring(dbStoreCartItem.PriceDesc.IndexOf("of") + 2).Trim())
                    '    Catch ex As Exception
                    '        .PriceDesc = String.Format("{0} (Case of {1})", dbStoreCartItem.PriceDesc, dbStoreItem.CaseQty)
                    '    End Try
                    'ElseIf dbStoreCartItem.PriceDesc.Contains("Pack") Then
                    '    .PriceDesc = String.Format("Case of {0}x{1}", dbStoreItem.CaseQty, dbStoreItem.PriceDesc.Replace("Pack", ""))
                    'Else
                    '    Try
                    '        Dim s As String() = dbStoreItem.PriceDesc.Split(" ")
                    '        If IsNumeric(s(0).Trim()) AndAlso (Not dbStoreItem.PriceDesc.Contains("oz") And Not dbStoreItem.PriceDesc.Contains("lbs") And Not dbStoreItem.PriceDesc.Contains("gallon")) Then
                    '            .PriceDesc = String.Format("Case of {0}x{1} {2}", dbStoreItem.CaseQty, s(0), IIf(s.Length >= 1, s(1), ""))
                    '        Else
                    '            .PriceDesc &= String.Format(" (Case of {0})", dbStoreItem.CaseQty)
                    '        End If
                    '    Catch ex As Exception
                    '        .PriceDesc &= String.Format(" (Case of {0})", dbStoreItem.CaseQty)
                    '    End Try
                    'End If

                    .PriceDesc = dbStoreItem.PriceDesc
                    .Price = dbStoreItem.CasePrice
                    .Weight = dbStoreItem.Weight * dbStoreItem.CaseQty
                    .Weight += (.Weight * 10) / 100
                ElseIf Label.Contains("Admin-") Then
                    .Price = adminPrice
                Else
                    .Price += ExtraPrice
                    .IsFreeSample = False
                End If

                .AttributePrice = ExtraPrice
                .Attributes = Attributes
                .AttributeSKU = AttributeSKU
                .Swatches = Swatches
                .Type = "item"
                .CarrierType = type
                .OrderId = OrderId
                .IsFreeItem = IsFreeItem
                .RegistryItemId = RegistryItemId
                .DepartmentId = DepartmentId
                .Quantity = Quantity
                .IsFreeGift = Label.Contains("FreeGift")
            End With

            Dim maximumqty As Integer = DB.ExecuteScalar("Select top 1 coalesce(maximumquantity,0) from storeitem where itemid = " & dbStoreCartItem.ItemId)
            If maximumqty > 0 AndAlso maximumqty < Quantity Then
                Session("ShowCartMessage") = "Item " & dbStoreCartItem.ItemName & " has a maximum purchase quantity Of " & maximumqty & " per order"
                DB.ExecuteSQL("update storecartitem Set quantity = " & CInt(maximumqty) & " where cartitemid = " & dbStoreCartItem.CartItemId)
                dbStoreCartItem.Quantity = maximumqty
            End If

            If dbStoreCartItem.LowSalePrice > 0 AndAlso dbStoreCartItem.LowSalePrice < dbStoreCartItem.Price Then
                dbStoreCartItem.LineDiscountAmountCust = dbStoreCartItem.Price - dbStoreCartItem.LowSalePrice
                dbStoreCartItem.CustomerPrice = dbStoreCartItem.LowSalePrice
                dbStoreCartItem.LineDiscountAmount = dbStoreCartItem.Price - dbStoreCartItem.LowSalePrice
            Else
                dbStoreCartItem.LineDiscountAmountCust = Nothing
                dbStoreCartItem.CustomerPrice = Nothing
                dbStoreCartItem.LineDiscountAmount = Nothing
            End If

            If (addByPoint) Then
                dbStoreCartItem.IsRewardPoints = True
                dbStoreCartItem.RewardPoints = point
                dbStoreCartItem.Price = Nothing
                dbStoreCartItem.SalePrice = Nothing
                dbStoreCartItem.SubTotalPoint = dbStoreCartItem.Quantity * point
                dbStoreCartItem.Total = 0
                dbStoreCartItem.SubTotal = 0
                dbStoreCartItem.LineDiscountAmountCust = Nothing
                dbStoreCartItem.CustomerPrice = Nothing
                dbStoreCartItem.LineDiscountAmount = Nothing
                dbStoreCartItem.QuantityPrice = Nothing
            Else
                dbStoreCartItem.IsRewardPoints = False
                dbStoreCartItem.SubTotalPoint = Nothing
                dbStoreCartItem.RewardPoints = Nothing
            End If

            If Label.Contains("Case") Then 'Buy Case
                dbStoreCartItem.AddType = 2
            Else
                dbStoreCartItem.AddType = WebType
            End If

            RecalculateCartItem(dbStoreCartItem, False)
            If dbStoreCartItem.IsFreeGift Then ' FREE Gift
                dbStoreCartItem.SubTotal = dbStoreCartItem.Price * dbStoreCartItem.Quantity
                dbStoreCartItem.LineDiscountAmount = dbStoreCartItem.SubTotal
                dbStoreCartItem.Quantity = 1
                dbStoreCartItem.MixMatchId = 0
                dbStoreCartItem.CustomerPrice = Nothing
                dbStoreCartItem.LineDiscountAmountCust = Nothing
                dbStoreCartItem.QuantityPrice = Nothing
                dbStoreCartItem.Total = 0
            ElseIf IsFreeItem Then 'FREE Item
                dbStoreCartItem.TotalFreeAllowed = TotalFreeAllowed
                dbStoreCartItem.FreeItemIds = FreeItemIds
            End If

            If dbStoreCartItem.CartItemId = Nothing Then dbStoreCartItem.Insert() Else dbStoreCartItem.Update()
            If Not dbStoreCartItem.IsFreeItem AndAlso Order.MemberId > 0 Then
                StoreItemRow.CountAddCartItem(DB, Order.MemberId, dbStoreCartItem.ItemId)
            End If

            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                End If
            End If
            Dim strLogHandling As String = String.Empty

            strLogHandling = "dbStoreCartItem.CarrierType=" & dbStoreCartItem.CarrierType & "<br>"
            If dbStoreCartItem.CarrierType = DefaultShippingId Then
                Dim sqlHandling As String = "Update StoreCartItem Set SpecialHandlingFee=[dbo].[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId,ItemId,AddType) where CartItemId=" & dbStoreCartItem.CartItemId
                DB.ExecuteSQL(sqlHandling)
                strLogHandling = strLogHandling & sqlHandling & "<br/>"
            End If

            If dbStoreCartItem.MixMatchId > 0 AndAlso dbStoreCartItem.IsFreeItem = False Then ' AndAlso MixMatchRow.IsDiscountPercent(dbStoreCartItem.MixMatchId) = False 'Khoa: ko cho MixMatch discount % add cart tu dong
                AddFreeItem(dbStoreCartItem.MixMatchId, customerPriceGroupId, MemberId)
            End If

            CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & Session("OrderId"))
            CacheUtils.RemoveCache(String.Format(StoreItemRow.cachePrefixKey & "GetRowByMemberLogin_{0}_{1}", dbStoreCartItem.ItemId, Order.MemberId))
            Utility.Common.DeleteCachePopupCart(Order.OrderId)
            Return dbStoreCartItem.CartItemId
        End Function

        Public Function ResetCartMixMatch(ByVal lstMixMatch As List(Of Integer)) As Boolean
            Dim result As Boolean = False
            If Not lstMixMatch Is Nothing Then
                If lstMixMatch.Count > 0 Then
                    Dim sql As String = ""
                    Dim dv As DataView = Nothing
                    Dim TotalInCart As Integer = 0
                    For Each mmId As Integer In lstMixMatch
                        If mmId < 1 Then
                            Continue For
                        End If
                        'If MixMatchRow.IsDiscountPercent(mmId) Then 'Khoa:  cho MixMatch discount % add cart tu dong
                        '    Continue For
                        'End If
                        TotalInCart = DB.ExecuteScalar("Select coalesce(sum(quantity), 0) from storecartitem cit left join MixmatchLine mml On(mml.MixmatchId=cit.MixmatchId And mml.ItemId=cit.ItemId) where Value=0 And cit.MixmatchId = " & mmId & " And isfreeitem = 0 And orderid = " & OrderId)
                        If TotalInCart < 1 Then
                            ''remove
                            DB.ExecuteSQL("Delete from StoreCartItem where MixmatchId=" & mmId & " And isfreeitem = 1 And orderid = " & OrderId)
                        Else
                            Dim p As PromotionRow = PromotionRow.GetRow(DB, mmId, False)
                            If (p.MixMatchId = mmId And p.MixMatchId > 0) Then
                                If TotalInCart >= p.Mandatory Then
                                    DB.ExecuteSQL("Delete from StoreCartItem where MixmatchId=" & mmId & " And isfreeitem = 1 And orderid = " & OrderId)
                                    AddFreeItemForMixMatch(TotalInCart, p)
                                Else
                                    If DB.ExecuteSQL("DELETE FROM StoreCartItem WHERE MixmatchId=" & mmId & " And IsFreeItem = 1 And OrderId = " & OrderId) > 0 Then
                                        DB.ExecuteSQL("UPDATE StoreCartItem Set TotalFreeAllowed = 0, FreeItemIds = NULL WHERE MixmatchId=" & mmId & " And IsFreeItem = 0 And OrderId = " & OrderId)
                                        result = True
                                    End If
                                End If
                            Else
                                DB.ExecuteSQL("DELETE FROM StoreCartItem WHERE MixmatchId=" & mmId & " And isfreeitem = 1 And orderid = " & OrderId)
                            End If
                        End If
                    Next
                End If
            End If
            Return result
        End Function
        Public Sub ResetCartMixMatchLogin(ByVal DB As Database, ByVal OrderId As Integer, ByVal memberID As Integer)
            Dim lstMM As List(Of Integer) = StoreCartItemRow.GetListMixMatchIdLogin(DB, OrderId)
            If Not lstMM Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For Each mmId As Integer In lstMM
                    If mmId < 1 Then
                        Continue For
                    End If
                    'If MixMatchRow.IsDiscountPercent(mmId) Then 'Khoa:  cho MixMatch discount % add cart tu dong
                    '    'DB.ExecuteSQL("Delete from StoreCartItem where OrderId=" & OrderId & " And MixmatchId=" & mmId & " And isfreeitem=1")
                    '    Continue For
                    'End If
                    AddFreeItem(mmId, customerPriceGroupId, memberID)
                    DB.ExecuteSQL("exec sp_StoreCartItem_ValidateDeleteMixMatch " & OrderId & "," & mmId & "," & customerPriceGroupId)
                Next
            End If
        End Sub


        Public Sub ResetMixmatchProductCouponLogin(ByVal DB As Database, ByVal OrderId As Integer, ByVal memberId As Integer)
            Dim lstMM As List(Of Integer) = StoreOrderRow.GetListMixMatchProductCouponId(DB, OrderId)
            If Not lstMM Is Nothing Then
                Dim customerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                For Each mmId As Integer In lstMM
                    AddFreeItem(mmId, customerPriceGroupId, memberId)
                Next
            End If
        End Sub
        Private Sub AddFreeItemForMixMatchDefault(ByVal TotalInCart As Integer, ByVal p As PromotionRow)
            Dim dtListItemFree As DataTable = DB.GetDataTable("Select ItemId,Value,IsDefaultSelect,DefaultSelectQty from MixMatchLine where MixMatchId=" & p.MixMatchId & " And Value=100")
            If dtListItemFree Is Nothing Then
                Exit Sub
            End If
            If dtListItemFree.Rows.Count < 1 Then
                Exit Sub
            End If
            Dim value As Double = 0
            Dim AllFreeIds As String = String.Empty
            '' p = PromotionRow.GetRow(DB, dv(0)("Id"), False)
            For x As Integer = 0 To p.GetItems.Count - 1
                If Not IsDBNull(p.GetItems(x)("SetPrice")) AndAlso p.GetItems(x)("SetPrice") = 0 Then
                    If Array.IndexOf(AllFreeIds.Split(","), p.GetItems(x)("ItemId")) = -1 Then AllFreeIds &= IIf(AllFreeIds = String.Empty, "", ",") & p.GetItems(x)("ItemId")
                ElseIf Not IsDBNull(p.GetItems(x)("PercentOff")) AndAlso p.GetItems(x)("PercentOff") = 100 Then
                    If Array.IndexOf(AllFreeIds.Split(","), p.GetItems(x)("ItemId").ToString) = -1 Then AllFreeIds &= IIf(AllFreeIds = String.Empty, "", ",") & p.GetItems(x)("ItemId")
                End If
            Next
            Dim countCurrentFreeItem As Integer = DB.ExecuteScalar("Select coalesce(sum(quantity), 0) from storecartitem where MixmatchId = " & p.MixMatchId & " And isfreeitem = 1 And orderid = " & OrderId)

            ''   Dim itemDefaultSelectId As Integer = DB.ExecuteScalar("Select mml.ItemId from MixMatchLine mml left join StoreItem it On(it.ItemId=mml.ItemId) where it.IsActive=1 And it.QtyOnHand>0 And MixMatchId=" & p.MixMatchId & " And Value=100 And IsDefaultSelect=1")
            Dim countGiveFree As Integer = Math.Floor(TotalInCart / p.Mandatory)
            If (countGiveFree > p.TimesApplicable And p.TimesApplicable > 0) Then
                countGiveFree = p.TimesApplicable
            End If
            Dim MaxQtyToAdd As Integer = countGiveFree * p.Optional
            'If we still have items to add
            Dim QtyToAdd As Integer = MaxQtyToAdd - countCurrentFreeItem
            Dim Sql As String = ""
            Dim gItemId As Integer = 0

            If QtyToAdd > 0 Then
                For Each dtRow As DataRow In dtListItemFree.Rows
                    gItemId = dtRow("ItemId")
                    Dim countItemActive As Integer = DB.ExecuteScalar("Select COUNT(*) from StoreItem where ItemId=" & gItemId & " And IsActive=1 And (QtyOnHand>0 Or  (AcceptingOrder=1 Or AcceptingOrder=2))")
                    If countItemActive < 1 Then
                        Continue For
                    End If
                    Dim defaultQty As Integer = dtRow("DefaultSelectQty") * countGiveFree
                    Dim cartItemIdFree As Integer = Add2Cart(gItemId, Nothing, defaultQty, 0, "Myself", "", "", 0, "", True, True, p.MixMatchId)
                Next
                Sql = "update storecartitem Set TotalFreeAllowed = " & MaxQtyToAdd & ", FreeItemIds = " & DB.Quote(AllFreeIds) & " where orderid = " & OrderId & " And mixmatchid = " & p.MixMatchId '& " And isfreeitem = 1 "
                DB.ExecuteSQL(Sql)
            End If
        End Sub
        Private Sub AddFreeItemForMixMatch(ByVal TotalInCart As Integer, ByVal p As PromotionRow)
            Dim isDefaultAllItem As Boolean = CBool(DB.ExecuteScalar("Select [dbo].[fc_MixMatch_IsDefaultAllItem](" & p.MixMatchId & ")"))
            If isDefaultAllItem Then
                AddFreeItemForMixMatchDefault(TotalInCart, p)
                Exit Sub
            End If

            Dim value As Double = 0
            FreeItemIds = String.Empty
            '' p = PromotionRow.GetRow(DB, dv(0)("Id"), False)
            For x As Integer = 0 To p.GetItems.Count - 1
                If Not IsDBNull(p.GetItems(x)("SetPrice")) AndAlso p.GetItems(x)("SetPrice") = 0 Then
                    If Array.IndexOf(FreeItemIds.Split(","), p.GetItems(x)("ItemId")) = -1 Then FreeItemIds &= IIf(FreeItemIds = String.Empty, "", ",") & p.GetItems(x)("ItemId")
                ElseIf Not IsDBNull(p.GetItems(x)("PercentOff")) AndAlso p.GetItems(x)("PercentOff") > 0 Then
                    If Array.IndexOf(FreeItemIds.Split(","), p.GetItems(x)("ItemId").ToString) = -1 Then FreeItemIds &= IIf(FreeItemIds = String.Empty, "", ",") & p.GetItems(x)("ItemId")
                End If
            Next
            Dim countCurrentFreeItem As Integer = DB.ExecuteScalar("Select coalesce(sum(quantity), 0) from storecartitem where MixmatchId = " & p.MixMatchId & " And isfreeitem = 1 And orderid = " & OrderId)

            Dim itemDefaultSelectId As Integer = DB.ExecuteScalar("Select mml.ItemId from MixMatchLine mml left join StoreItem it On(it.ItemId=mml.ItemId) where it.IsActive=1 And it.QtyOnHand>0 And MixMatchId=" & p.MixMatchId & " And Value>0 And IsDefaultSelect=1")
            Dim countGiveFree As Integer = Math.Floor(TotalInCart / p.Mandatory)
            If (countGiveFree > p.TimesApplicable And p.TimesApplicable > 0) Then
                countGiveFree = p.TimesApplicable
            End If
            TotalFreeAllowed = countGiveFree * p.Optional

            'If we still have items to add
            Dim QtyToAdd As Integer = TotalFreeAllowed - countCurrentFreeItem
            Dim Sql As String = ""
            Dim gItemId As Integer = 0
            If QtyToAdd > 0 Then
                If itemDefaultSelectId > 0 Then ''Mixmatch  is set default item
                    Dim DefaultSelectFreeQty As Integer = DB.ExecuteScalar("Select DefaultSelectQty from MixMatchLine where MixMatchId=" & p.MixMatchId & " And Value>0 And ItemId=" & itemDefaultSelectId)
                    If (DefaultSelectFreeQty < 1) Then
                        DefaultSelectFreeQty = 1
                    End If
                    DefaultSelectFreeQty = DefaultSelectFreeQty * countGiveFree
                    If (DefaultSelectFreeQty > QtyToAdd) Then
                        DefaultSelectFreeQty = QtyToAdd
                    End If
                    If DefaultSelectFreeQty > 0 Then
                        ''add item default
                        ''check exists

                        Dim cartItemIdFree As Integer = 0
                        cartItemIdFree = DB.ExecuteScalar("Select CartItemId from StoreCartItem where  orderid = " & OrderId & " And mixmatchid = " & p.MixMatchId & " And isfreeitem = 1 ")
                        If cartItemIdFree < 1 Then
                            cartItemIdFree = Add2Cart(itemDefaultSelectId, Nothing, DefaultSelectFreeQty, 0, "Myself", "", "", 0, "", True, True, p.MixMatchId)
                            'Sql = "update storecartitem Set TotalFreeAllowed = " & MaxQtyToAdd & ", FreeItemIds = " & DB.Quote(AllFreeIds) & " where orderid = " & OrderId & " And mixmatchid = " & p.MixMatchId & " And isfreeitem = 1 "
                            DB.ExecuteSQL(Sql)
                        Else
                            'DB.ExecuteSQL("Update StoreCartItem Set Quantity=" & DefaultSelectFreeQty & ",TotalFreeAllowed = " & MaxQtyToAdd & ", FreeItemIds = " & DB.Quote(AllFreeIds) & " where CartItemId=" & cartItemIdFree)
                        End If

                        Dim addOtherQty As Integer = QtyToAdd - DefaultSelectFreeQty
                        If (addOtherQty > 0) Then '' add them free item neu item default ko du qty
                            For k As Integer = 0 To p.GetItems.Count - 1
                                'The current ItemId
                                value = p.GetItems(k)("Value")
                                If (value > 0) Then
                                    gItemId = p.GetItems(k)("ItemId")
                                    If gItemId <> itemDefaultSelectId Then
                                        ''check item is Active
                                        Dim countItemActive As Integer = DB.ExecuteScalar("Select COUNT(*) from StoreItem where ItemId=" & gItemId & " And IsActive=1 And (QtyOnHand>0 Or  (AcceptingOrder=1 Or AcceptingOrder=2))")
                                        If countItemActive < 1 Then
                                            Continue For
                                        End If
                                        cartItemIdFree = Add2Cart(gItemId, Nothing, addOtherQty, 0, "Myself", "", "", 0, "", True, True, p.MixMatchId)
                                        'Sql = "update storecartitem Set TotalFreeAllowed = " & MaxQtyToAdd & ", FreeItemIds = " & DB.Quote(AllFreeIds) & " where orderid = " & OrderId & " And mixmatchid = " & p.MixMatchId & " And isfreeitem = 1 "
                                        DB.ExecuteSQL(Sql)
                                        Exit For
                                    End If

                                End If
                            Next
                        End If
                    End If
                Else '' Mixmatch not set default item
                    For k As Integer = 0 To p.GetItems.Count - 1
                        'The current ItemId
                        value = p.GetItems(k)("Value")
                        If (value > 0) Then
                            gItemId = p.GetItems(k)("ItemId")
                            ''check item is Active
                            Dim countItemActive As Integer = DB.ExecuteScalar("Select COUNT(*) from StoreItem where ItemId=" & gItemId & " And IsActive=1 And (QtyOnHand>0 Or  (AcceptingOrder=1 Or AcceptingOrder=2))")
                            If countItemActive < 1 Then
                                Continue For
                            End If

                            Dim cartItemIdFree As Integer = Add2Cart(gItemId, Nothing, QtyToAdd, 0, "Myself", "", "", 0, "", True, True, p.MixMatchId)
                            Sql = "UPDATE StoreCartItem Set TotalFreeAllowed = (Select TotalFreeAllowed FROM StoreCartItem WHERE CartitemId = " & cartItemIdFree & "), FreeItemIds = (Select FreeItemIds FROM StoreCartItem WHERE CartitemId = " & cartItemIdFree & ")  WHERE OrderId = " & OrderId & " And MixMatchId = " & p.MixMatchId & " And IsFreeItem = 0"
                            DB.ExecuteSQL(Sql)
                            Exit For
                        End If
                    Next
                End If
            End If

        End Sub
        Private Sub AddFreeItem(ByVal MixMatchId As Integer, ByVal customerPriceGroupId As Integer, ByVal memberId As Integer)
            DB.ExecuteSQL("DELETE FROM StoreCartItem WHERE OrderId=" & OrderId & " And MixmatchId=" & MixMatchId & " And IsFreeItem=1")
            Dim SQL As String = ""
            Dim dv As DataView
            Dim p As PromotionRow
            Dim ItemId As Integer = 0
            Dim TotalInCart As Integer

            Dim MaxQtyToAdd As Integer = 0
            SQL = "Select mm.id, 1 As haslineactive, mm.isactive, coalesce(mm.startingdate," & DB.Quote(Now.ToShortDateString) & ") As startingdate, coalesce(mm.endingdate+1," & DB.Quote(Now.AddDays(1).ToShortDateString) & ") As endingdate,coalesce(mm.CustomerPriceGroupId,0) As CustomerPriceGroupId,mm.Type from mixmatchline mml inner join mixmatch mm On mml.mixmatchid = mm.id inner join storecartitem sci On mml.itemid = sci.itemid where sci.orderid = " & OrderId & " And mm.id =" & MixMatchId & " group by mm.id, mm.isactive, coalesce(mm.startingdate," & DB.Quote(Now.ToShortDateString) & "), coalesce(mm.endingdate+1," & DB.Quote(Now.AddDays(1).ToShortDateString) & "),mm.CustomerPriceGroupId,mm.Type "
            dv = DB.GetDataView(SQL)
            If dv.Count > 0 Then

                Dim mixMatchGroup As Integer = dv(0)("CustomerPriceGroupId")
                Dim mixMatchType As Integer = dv(0)("Type")

                If CBool(dv(0)("isactive")) AndAlso CBool(dv(0)("haslineactive")) AndAlso (dv(0)("startingdate") <= Now.ToShortDateString AndAlso Now.ToShortDateString < dv(0)("endingdate")) Then
                    If mixMatchGroup > 0 And mixMatchGroup <> customerPriceGroupId Then
                        Exit Sub
                    End If
                End If
                If mixMatchType = Utility.Common.MixmatchType.ProductCoupon Then
                    Dim mixMatchValid As Boolean = False
                    ''get list cart item
                    Dim dtCartItem As DataTable = DB.GetDataTable("Select CartItemId from StoreCartItem where IsFreeItem<>1 And  mixmatchId=" & MixMatchId & " And OrderID=" & Order.OrderId)
                    If Not dtCartItem Is Nothing Then
                        Dim countCartItem As Integer = dtCartItem.Rows.Count
                        If countCartItem = 1 Then
                            Dim cartItemId As Integer = dtCartItem.Rows(0)("CartItemId")
                            Dim CartPromotionID As Integer = DB.ExecuteScalar("Select PromotionID from StoreCartItem where CartItemId=" & cartItemId)
                            Dim dtPromotion As DataTable = DB.GetDataTable("Select PromotionCode,PromotionId from StorePromotion where mixmatchId=" & MixMatchId)
                            Dim PromotionCode As String = String.Empty
                            Dim PromotionID As Integer = 0
                            If Not dtPromotion Is Nothing Then
                                If dtPromotion.Rows.Count > 0 Then
                                    PromotionCode = dtPromotion.Rows(0)("PromotionCode")
                                    PromotionID = dtPromotion.Rows(0)("PromotionId")
                                End If
                            End If
                            If Not String.IsNullOrEmpty(PromotionCode) Then
                                If PromotionID = CartPromotionID Then

                                    If StorePromotionRow.ValidateProductPromotion(DB, PromotionCode, memberId, cartItemId) = 1 Then
                                        mixMatchValid = True
                                    End If
                                End If
                            Else
                                mixMatchValid = False
                            End If
                        End If
                    End If
                    If Not mixMatchValid Then
                        Exit Sub
                    End If
                End If

                Dim countItemFreeAvailable As Integer = MixMatchRow.CountFreeItem(DB, MixMatchId, memberId, OrderId)
                If countItemFreeAvailable < 1 Then
                    Exit Sub
                End If
                Dim AllFreeIds As String = String.Empty
                p = PromotionRow.GetRow(DB, dv(0)("Id"), False)
                If p.Type = PromotionType.LineSpecific Then
                    If p.LinesToTrigger <> p.Optional AndAlso Not p.PurchaseItems.Count = 0 Then
                        '' TotalInCart = DB.ExecuteScalar("Select coalesce(sum(quantity), 0) from storecartitem where MixmatchId = " & dbStoreCartItem.MixMatchId & " And isfreeitem = 0 And orderid = " & OrderId)
                        TotalInCart = DB.ExecuteScalar("Select coalesce(sum(quantity), 0) from storecartitem cit left join MixmatchLine mml On(mml.MixmatchId=cit.MixmatchId And mml.ItemId=cit.ItemId) where Value=0 And cit.MixmatchId = " & MixMatchId & " And IsFreeItem = 0  And AddType <> 2 And orderid = " & OrderId)
                        If TotalInCart >= p.Mandatory Then
                            AddFreeItemForMixMatch(TotalInCart, p)
                        End If
                    End If
                End If
            End If
        End Sub
        Public Shared Function DeleteCartMixMatchNotValid(ByVal DB As Database, ByVal _OrderID As Integer, ByVal _customerPriceGroupId As Integer) As Boolean
            Dim lstMM As List(Of Integer) = StoreCartItemRow.GetListMixMatchId(DB, _OrderID)
            For Each mmId As Integer In lstMM
                If Not CheckCartMixmatchValid(DB, mmId, _customerPriceGroupId) Then
                    StoreCartItemRow.DeleteMixMatchNotValid(DB, _OrderID, mmId)
                End If
            Next
        End Function

        Public Shared Function CheckCartMixmatchValid(ByVal DB As Database, ByVal _MixMatchId As Integer, ByVal _customerPriceGroupId As Integer) As Boolean
            Dim SQL As String = ""
            Dim dv As DataView
            Dim ItemId As Integer = 0
            Dim MaxQtyToAdd As Integer = 0
            SQL = "Select  isactive, coalesce(startingdate," & DB.Quote(Now.ToShortDateString) & ") As startingdate, coalesce(endingdate+1," & DB.Quote(Now.AddDays(1).ToShortDateString) & ") As endingdate,coalesce(CustomerPriceGroupId,0) As CustomerPriceGroupId from  mixmatch  where id =" & _MixMatchId
            dv = DB.GetDataView(SQL)
            If dv.Count > 0 Then
                Dim mixMatchGroup As Integer = dv(0)("CustomerPriceGroupId")
                If mixMatchGroup > 0 And mixMatchGroup <> _customerPriceGroupId Then
                    Return False
                End If
                If CBool(dv(0)("isactive")) AndAlso (dv(0)("startingdate") <= Now.ToShortDateString AndAlso Now.ToShortDateString < dv(0)("endingdate")) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Public Sub GetPurchasePoint(ByVal order As StoreOrderRow, ByVal PurChasePoint As Integer)
            Dim TotalPP As Double = 0
            RecalculateOrderUpdate()
            Dim MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
            order.PurchasePoint = PurChasePoint
            TotalPP = PurChasePoint * MoneyEachPoint
            order.TotalPurchasePoint = TotalPP
            order.SubTotal = order.SubTotal + order.PointAmountDiscount - TotalPP
            order.Update()
            RecalculateTotal()
        End Sub

        Public Sub GetPurchasePoint(ByVal order As StoreOrderRow, ByVal PurChasePoint As Integer, ByVal PointMessage As String)
            If (PurChasePoint > 0) Then
                Dim TotalPP As Double = 0
                Dim MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
                order.PurchasePoint = PurChasePoint
                TotalPP = PurChasePoint * MoneyEachPoint
                order.TotalPurchasePoint = TotalPP
                order.SubTotal = order.SubTotal + order.PointAmountDiscount - TotalPP
                order.PointMessage = PointMessage
            Else
                order.PointMessage = ""
                order.TotalPurchasePoint = 0
                order.PurchasePoint = 0
            End If

            order.Update()
            RecalculateOrderUpdate()
        End Sub

        Public Function GetFreightShippingCharges(ByVal strCountryID As String, ByVal strZipcode As String, ByVal strMethodId As String) As String
            If strCountryID = "" Then
                If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") Then
                    Dim SQL As String
                    SQL = "delete from storecartitem where orderid = " & OrderId & " And type='carrier'"
                    DB.ExecuteSQL(SQL)
                    Order.Shipping = 0
                    RecalculateShippingInternational()
                    Return ""
                End If
            Else
                Dim dv As DataView, drv As DataRowView
                Dim additional As Double = 0
                Dim ci As StoreCartItemRow
                Dim Weight As Double = Nothing
                Dim WeightOV As Double = 0
                Dim feeShipOversize As Double = 0
                Dim MethodId As Integer = Nothing
                Dim Ids As String = String.Empty
                Dim SQL As String
                Dim AlreadyAppliedLiftGate As Boolean = False
                Dim AlreadyAppliedScheduledDelivery As Boolean = False
                Dim AlreadyAppliedInsideDelivery As Boolean = False
                Dim AppliedHazMatFee As Boolean = False

                SQL = "select coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = storecartitem.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype "
                SQL += " from storecartitem where orderid = " & Order.OrderId & " and type = 'item' and isfreeitem = 0 "
                SQL += " group by carriertype"
                dv = DB.GetDataView(SQL)
                Dim strZipcodeNum As String = strZipcode
                Dim strZipcodeCode As String = ""
                If strZipcode.Length = 5 Then
                    Dim i As Integer = 0
                    i = CInt(strZipcode)
                    Dim dbZipCode As ZipCodeRow = ZipCodeRow.GetRow(DB, strZipcode)
                    strZipcode = GetZipCodeSpecialUS(strZipcode)
                End If
                strZipcodeCode = strZipcode
                Dim ShippingTotal As Double = 0
                Dim IsFreeShipping As Boolean = False
                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)
                    If strCountryID <> "US" Then
                        MethodId = 15
                    Else
                        If CheckShippingSpecialUS() = False Then
                            Weight = drv("weight") - drv("freeweight")
                            MethodId = drv("carriertype")
                        Else
                            If HasOversizeItems() = False Then
                                If MethodId = drv("carriertype") Then
                                    Weight = drv("weight") - drv("freeweight")
                                Else
                                    Weight = 0
                                    MethodId = drv("carriertype")
                                End If
                            Else
                                If drv("carriertype") = 15 Then
                                    Weight = drv("weight")
                                    MethodId = drv("carriertype")
                                    strZipcode = strZipcodeCode
                                Else
                                    Weight = drv("weight")
                                    strZipcode = strZipcodeNum
                                    MethodId = drv("carriertype")
                                End If
                            End If
                        End If
                    End If

                    If strMethodId = DefaultShippingId Then
                        Weight = drv("weight") - drv("freeweight")
                    Else
                        If MethodId = 15 And drv("carriertype") = 4 Then
                            Weight = 0
                        Else
                            Weight = drv("weight")
                            '''''Long add Round weight
                            'If Weight < 1 Then
                            '    Weight = 1
                            'Else
                            '    Weight = Math.Round(Weight)
                            'End If
                            '''''
                        End If
                    End If

                    If HasOversizeItems() = True Then
                        If drv("carriertype") = strMethodId Then
                            MethodId = drv("carriertype")
                            If MethodId = 4 Then
                                WeightOV = WeightFlatFee()
                                Weight = drv("weight") - drv("freeweight") - WeightOV
                                feeShipOversize = ShippingOversize(Order)
                            Else
                                Weight = drv("weight") - drv("freeweight")
                            End If
                        ElseIf MethodId <> 15 Then
                            Weight = 0
                            MethodId = drv("carriertype")
                        End If
                    End If

                    If OnlyOversizeItems() Then
                        If CheckShippingSpecialUS() Then
                            Weight = 0
                        End If
                        If strCountryID <> "US" Then
                            Weight = 0
                        End If
                    End If

                    If Ids <> String.Empty Then Ids &= ","
                    Ids &= MethodId

                    Dim Shipping As Double = 0
                    ci = StoreCartItemRow.GetRow(DB, OrderId, MethodId)

                    If Weight > 0 Then
                        SQL = "select top 1 coalesce(case when " & Weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(Weight - 1) < 1, 0, Math.Ceiling(Weight - 1)) & " * additionalpound else case when " & Weight & " - additionalthreshold > 0 then (" & Weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(strZipcode) & " between lowvalue and highvalue and sm.methodid = " & MethodId
                        Shipping = DB.ExecuteScalar(SQL)
                    End If
                    If MethodId = 4 Then
                        Shipping += feeShipOversize
                    End If
                    SQL = "select coalesce(sum(rushdeliverycharge),0) from storecartitem where isrushdelivery = 1 and orderid = " & OrderId & " and carriertype = " & MethodId
                    Shipping += DB.ExecuteScalar(SQL)

                    SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and isliftgate = 1"
                    If Not AlreadyAppliedLiftGate AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                        Shipping += SysParam.GetValue("LiftGateCharge")
                        AlreadyAppliedLiftGate = True
                    End If

                    SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and IsInsideDelivery = 1"
                    If Not AlreadyAppliedInsideDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                        Shipping += SysParam.GetValue("InsideDeliveryService")
                        AlreadyAppliedInsideDelivery = True
                    End If

                    SQL = "select top 1 cartitemid from storecartitem where isscheduledelivery = 1 and orderid = " & OrderId
                    If Not AlreadyAppliedScheduledDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                        Shipping += SysParam.GetValue("ScheduleDeliveryCharge")
                        AlreadyAppliedScheduledDelivery = True
                    End If

                    'SQL = "select count(cartitemid) from storecartitem where ishazmat = 1 and carriertype = " & MethodId & " and orderid = " & OrderId
                    'If Not AppliedHazMatFee AndAlso DB.ExecuteScalar(SQL) > 0 Then
                    '    Shipping += 20
                    '    AppliedHazMatFee = True
                    'End If

                    ci.SubTotal = Shipping
                    ci.Total = IIf(Order.IsFreeShipping AndAlso MethodId = DefaultShippingId, 0, Shipping)

                    If Order.ShipmentInsured Then
                        Dim Insurance As Double = StoreOrderRow.GetShippingInsurance(DB, OrderId, MethodId)
                        ci.SubTotal += Insurance
                        ci.Total += Insurance
                    End If
                    ShippingTotal += ci.Total
                Next
                Return ShippingTotal.ToString
            End If
            Return ""
        End Function




        Public Function GetCalculateTaxPrices(ByVal strZipcode As String) As String
            Dim i As Integer = 0
            Dim dTax As Double = 0
            Dim dAmountTax As Double = DB.ExecuteScalar("select coalesce(sum(total),0) from storecartitem where istaxfree = 1 and orderid = " & OrderId)
            If strZipcode.Length = 5 Then
                If IsNumeric(strZipcode) = False Then
                    Return "0"
                End If

                Dim dbZipCode As ZipCodeRow = ZipCodeRow.GetRow(DB, strZipcode)

                If dbZipCode.StateCode = "IL" Then
                    'Long edit CashPoint'
                    dTax = (SubTotalPuChasePoint() - dAmountTax) * SysParam.GetValue("SaleTax") / 100
                    'dTax = (Order.SubTotal - dAmountTax) * SysParam.GetValue("SaleTax") / 100
                    Return dTax.ToString
                Else
                    Return "0"
                End If
            End If
            Return "0"
        End Function

        Public Function BuildUSShippingFee(ByVal MethodId As Integer, ByVal ShippingFee As Double, ByVal IsFreeShipping As Boolean) As String

            Dim s As String = String.Empty
            Dim strFreightShipping As String = ""

            Try
                Dim total As Double = 0


                s &= "<tr>" & vbCrLf &
                        " <td class='left'></td><td  class=""label-text"">" & IIf(MethodId = PickupShippingId, "*", "") & ShipmentMethodRow.GetNameById(MethodId) & "</td>" & vbCrLf

                If ShippingFee = 0 Then
                    s &= "<td class=""label-data"">" & IIf(MethodId = PickupShippingId, "No Charge", IIf(MethodId = DefaultShippingId, "<div class=""redbold"">FREE!</div>", FormatCurrency(ShippingFee))) & "</td>" & vbCrLf &
                                "<td class='right'></td></tr>"
                Else
                    s &= "<td class=""label-data"">" & IIf(MethodId = PickupShippingId, "No Charge", IIf(MethodId = DefaultShippingId AndAlso IsFreeShipping, "<div class=""redbold"">FREE!</div><span class=""strike"">" & FormatCurrency(ShippingFee) & "</span>", FormatCurrency(ShippingFee))) & "</td>" & vbCrLf &
                                "<td class='right'></td></tr>"
                End If


            Catch ex As Exception
                Email.SendError("ToError500", "BaseShoppingCart.vb - GetShippingLinesPrices", "MemberId: " & Common.GetCurrentMemberId() & "><br>BaseShoppingCart.vb > GetUSShippingFee<br>Exception: " & ex.ToString())
            End Try

            Return s
        End Function

        Public Function BuildFreightDelivery(ByVal ShippingFee As Double) As String

            Dim s As String = String.Empty
            Dim strFreightShipping As String = ""

            Try
                Dim total As Double = 0


                s &= "<tr>" & vbCrLf &
                        " <td class='left'></td><td  class=""label-text"">Freight Delivery</td>" & vbCrLf

                s &= "<td class=""label-data"">" & FormatCurrency(ShippingFee) & "</td>" & vbCrLf &
                                "<td class='right'></td></tr>"

            Catch ex As Exception
                Email.SendError("ToError500", "BaseShoppingCart.vb - GetShippingLinesPrices", "MemberId: " & Common.GetCurrentMemberId() & "><br>BaseShoppingCart.vb > GetUSShippingFee<br>Exception: " & ex.ToString())
            End Try

            Return s
        End Function

        Public Function GetShippingLinesPrices(ByVal strCountryID As String, ByVal strZipcode As String, ByVal strMethodId As String) As String

            Dim s As String = String.Empty
            Dim FreeShipping As Boolean = False
            Dim bFreeShipping As Boolean = False
            Dim strFreightShipping As String = ""

            FreeShipping = Order.IsFreeShipping
            bFreeShipping = FreeShipping

            If CheckShippingSpecialUS() Then
                s = GetShippingLineDetails()
            ElseIf strCountryID = "US" Then
                Dim dr As SqlDataReader = Nothing
                Dim SQL As String = String.Empty
                Try
                    SQL = "select carriertype, subtotal, total, FreeShipping,[name], 0 as insurance, 0 as signature from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"
                    dr = DB.GetReader(SQL)
                    While dr.Read
                        FreeShipping = bFreeShipping
                        If FormatCurrency(dr("subtotal")) = 0 And FreeShipping Then
                            FreeShipping = True
                        Else
                            FreeShipping = Order.IsFreeShipping
                        End If
                        Dim total As Double = 0


                        s &= "<tr>" & vbCrLf &
                             " <td class='left'></td><td  class=""label-text"">" & IIf(dr("carriertype") = PickupShippingId, "*", "") & dr("name") & "</td>" & vbCrLf

                        s &= "<td class=""label-data"">" & IIf(dr("carriertype") = PickupShippingId, "No Charge", IIf(dr("carriertype") = DefaultShippingId AndAlso FreeShipping, "<div class=""redbold"">FREE!</div><span class=""strike"">" & FormatCurrency(dr("FreeShipping")) & "</span>", FormatCurrency(dr("total")))) & "</td>" & vbCrLf &
                                     "<td class='right'></td></tr>"
                    End While
                    Core.CloseReader(dr)
                Catch ex As Exception
                    Core.CloseReader(dr)
                    Email.SendError("ToError500", "BaseShoppingCart.vb - GetShippingLinesPrices", "MemberId: " & Common.GetCurrentMemberId() & "<br>UserName: " & Session("Username") & "<br><br>Public Function GetShippingLinesPrices(ByVal strCountryID As String, ByVal strZipcode As String, ByVal strMethodId As String) As String<br><br>Exception: " & ex.ToString())
                End Try

            Else
                s = "<tr>" & vbCrLf &
                  "<td class='left'></td><td colspan='2' class=""label-text"">" & GetShippingTBDLine("USPS Priority") & "</td>" & vbCrLf &
                  "<td class='right'></td></tr>"

                If OnlyOversizeItems() Then
                    s = "<tr>" & vbCrLf &
                          "<td class='left'></td><td colspan='2' class=""label-text"">" & GetShippingTBDLine("USPS Priority") & "</td>" & vbCrLf &
                          "<td class='right'></td></tr>"
                    Return s
                End If

                'Khoa tat: Hazardous Material Fee
                'If HasFlammableCartItem() Then
                '    s = "<tr>" & vbCrLf &
                '          "<td class='left'></td><td colspan='2' class=""label-text"" >" & GetShippingTBDLine("USPS Priority") & "</td>" & vbCrLf &
                '          "<td class='right'></td></tr>"
                '    Return s
                'End If

                If Utility.Common.CheckShippingInternational(DB, Order) = False Then
                    Return s
                Else
                    s = ""
                End If
                s = GetShippingLineDetails()
            End If
            Return s
        End Function

        Public Function GetShippingTBDLine(ByVal methodName As String) As String
            methodName = "Shipping"
            Dim result As String = "<span class='TBDMethod'>" & methodName & " - To Be Determined</span><br/><span class='TBDComment'>We will contact you for shipping cost</span>"
            Return result
        End Function
        Public Function GetMailShippingTBDLine(ByVal MethodId As Integer) As String
            Dim MethodName As String = "Shipping"
            If Common.InternationalShippingId.Contains(MethodId) Then
                MethodName = "International Shipping"
            End If

            Return "<span style=""float:left;color:Red;padding-bottom: 0px;padding-top: 4px"">" & MethodName & " - To Be Determined</span><br/><span style=""float:left;padding-bottom: 0px;padding-top: 4px"">We will contact you for shipping cost</span>"
        End Function
        Public Function GetShippingLines() As String
            Dim s As String = String.Empty
            Dim FreeShipping As Boolean = False
            Dim bFreeShipping As String = False
            Dim strFreightShipping As String = ""

            FreeShipping = Order.IsFreeShipping
            bFreeShipping = FreeShipping

            If CheckShippingSpecialUS() Then
                s = GetShippingLineDetails()
            ElseIf Order.BillToCountry = "US" AndAlso Order.IsSameAddress OrElse Order.ShipToCountry = "US" Then
                Dim dr As SqlDataReader = Nothing
                Dim SQL As String = String.Empty
                Dim shippingCode As String = String.Empty
                Dim prefix As String = String.Empty
                Try
                    Dim strSignature As String = ""
                    Dim strInsurance As String = ""
                    Dim insurance As Double = 0

                    Dim shipping As Double = 0

                    Dim isViewAdmin As Boolean = Utility.Common.IsViewFromAdmin
                    If Not isViewAdmin Then
                        SQL = "select coalesce(Prefix,'') as Prefix,sm.Code,carriertype,COALESCE(FreeShipping,0) as FreeShipping, subtotal, total, [name], coalesce(insurance,0) as insurance,[dbo].[fc_StoreOrder_GetOrderShippingInsurance](OrderId,carriertype) as TotalInsurance from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"
                    Else
                        SQL = "select coalesce(Prefix,'') as Prefix,sm.Code,carriertype,COALESCE(FreeShipping,0) as FreeShipping, subtotal, total, [name], coalesce(insurance,0) as insurance,[dbo].[fc_StoreOrder_GetOrderShippingInsurance](OrderId,carriertype) as TotalInsurance, (select convert(decimal(15, 2), CEILING(SUM(a.Weight*a.Quantity) * 100) / 100) from StoreCartItem a where a.CarrierType = sci.CarrierType and a.Type = 'item' and a.OrderId = " & OrderId & ") as sum from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"
                    End If

                    dr = DB.GetReader(SQL)
                    While dr.Read
                        prefix = dr("Prefix")
                        shippingCode = dr("Code")
                        If (prefix = Utility.Common.ShippingTBD) Then
                            strSignature = ""
                            strInsurance = ""
                            insurance = dr("insurance")

                            If (Order.SignatureConfirmation > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1 And dr("carriertype") <> Utility.Common.TruckShippingId) Then

                                ''strSignature = "<tr><td class=""titprice"">Signature Confirmation </td>" & vbCrLf

                                strSignature = "<tr><td class='left'></td><td class=""label-text""><div class='signconfirm'>Signature Confirmation</div>"
                                'If System.Web.HttpContext.Current.Request.RawUrl.Contains("/store/submitorder.aspx") Then
                                '    strSignature = strSignature & "<div class='signconfirmIcon'><a id=""aHelpSignature1"" href=""javascript:void(0);"" class=""smaller maglnk""><img src=""/App_Themes/Default/images/help.gif""> </a></div></td>"
                                'End If

                                strSignature &= "<td class=""label-data"">" & FormatCurrency(Order.SignatureConfirmation) & "</td><td class='right'></td></tr>"
                            End If
                            If Order.ShipmentInsured AndAlso Common.USShippingCode().Contains(shippingCode) Then
                                If (Order.Insurance > 0) Then
                                    strInsurance = "<tr><td class='left'></td><td class=""label-text"">Insurance Included</td>" & vbCrLf
                                    strInsurance &= "<td class=""label-data"">" & FormatCurrency(Order.Insurance) & "</td><td class='right'></td></tr>"
                                ElseIf (insurance > 0) Then
                                    strInsurance = "<tr><td class='left'></td><td class=""label-text"">Insurance Included</td>" & vbCrLf
                                    Dim InsuranceValue As Double = dr("TotalInsurance")
                                    strInsurance &= "<td class=""label-data"">" & FormatCurrency(InsuranceValue) & "</td><td class='right'></td></tr>"
                                End If
                            End If


                            s &= "<tr>" & vbCrLf &
                        "<td class='left'></td><td colspan='2' class=""label-text"">" & GetShippingTBDLine(dr("name")) & "</td><td class='right'></td>" & vbCrLf

                            s &= "</tr>"
                            s &= strInsurance & strSignature
                        Else
                            Dim shippingMethod As Integer = dr("carriertype")
                            FreeShipping = bFreeShipping
                            'If FormatCurrency(dr("subtotal")) = 0 And FreeShipping Then
                            If FormatCurrency(dr("subtotal")) = 0 Then
                                FreeShipping = True
                            Else
                                FreeShipping = Order.IsFreeShipping
                            End If
                            strSignature = ""
                            strInsurance = ""
                            insurance = dr("insurance")

                            shipping = FormatCurrency(dr("total"))
                            If (Order.SignatureConfirmation > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1 And dr("carriertype") <> Utility.Common.TruckShippingId) Then

                                strSignature = "<tr><td class='left'></td><td class=""label-text""><div class='signconfirm'>Signature Confirmation</div>"
                                'If System.Web.HttpContext.Current.Request.RawUrl.Contains("/store/submitorder.aspx") Then
                                '    strSignature = strSignature & "<div class='signconfirmIcon'><a id=""aHelpSignature1"" href=""javascript:void(0);"" class=""smaller maglnk""><img src=""/App_Themes/Default/images/help.gif""> </a></div></td>"
                                'End If
                                strSignature &= "<td class=""label-data"">" & FormatCurrency(Order.SignatureConfirmation) & "</td><td class='right'></td></tr>"
                                shipping = shipping - Order.SignatureConfirmation
                            End If
                            If Order.ShipmentInsured AndAlso Common.USShippingCode().Contains(shippingCode) Then
                                If (Order.Insurance > 0) Then
                                    strInsurance = "<tr><td class='left'></td><td class=""label-text"">Insurance Included</td>" & vbCrLf
                                    strInsurance &= "<td class=""label-data"">" & FormatCurrency(Order.Insurance) & "</td><td class='right'></td></tr>"
                                    shipping = shipping - Order.Insurance
                                ElseIf (insurance > 0) Then
                                    strInsurance = "<tr><td class='left'></td><td class=""label-text"">Insurance Included</td>" & vbCrLf
                                    Dim InsuranceValue As Double = dr("TotalInsurance")
                                    strInsurance &= "<td class=""label-data"">" & FormatCurrency(InsuranceValue) & "</td><td class='right'></td></tr>"
                                    shipping = shipping - InsuranceValue
                                End If
                            End If

                            If (shipping < 0) Then
                                shipping = 0
                            End If


                            s &= "<tr>" & vbCrLf &
                         "<td class='left'></td><td class=""label-text"">" & IIf(dr("carriertype") = PickupShippingId, "*", "") & dr("name") & "</td>" & vbCrLf

                            s &= "<td class=""label-data"">"
                            If (dr("carriertype") = PickupShippingId) Then
                                s &= "No Charge"
                            ElseIf dr("carriertype") = DefaultShippingId AndAlso FreeShipping Then
                                Dim totalFreeShip As Double = dr("FreeShipping")
                                If totalFreeShip > 0 Then
                                    s &= "<span class=""redbold"">FREE!</span><br/><span class=""strike"">" & FormatCurrency(totalFreeShip) & "</span>"
                                Else
                                    s &= "<span class=""redbold"">FREE!</span>"
                                End If

                                If isViewAdmin AndAlso Not String.IsNullOrEmpty(dr("sum").ToString()) Then
                                    s &= "<div>" & IIf(dr("sum") Mod 1 = 0, CDbl(dr("sum")).ToString("C0").ToString().Replace("$", ""), CDbl(dr("sum")).ToString("C2").ToString().Replace("$", "")) & " lbs</div>"
                                End If


                            ElseIf dr("carriertype") = TruckShippingId AndAlso FreeShipping AndAlso FormatCurrency(dr("subtotal")) = 0 Then
                                s &= "<span class=""strike"">" & FormatCurrency(shipping) & "</span><br /><span class=""redbold"">FREE!</span>"
                            Else
                                s &= FormatCurrency(shipping)
                                If isViewAdmin AndAlso Not String.IsNullOrEmpty(dr("sum").ToString()) Then
                                    s &= "<div>" & IIf(dr("sum") Mod 1 = 0, CDbl(dr("sum")).ToString("C0").ToString().Replace("$", ""), CDbl(dr("sum")).ToString("C2").ToString().Replace("$", "")) & " lbs</div>"
                                End If
                            End If
                            s &= "</td><td class='right'></td></tr>"

                            s &= strInsurance & strSignature
                            '     s &= "<tr>" & vbCrLf & _
                            '"<td class=""titprice"">" & IIf(dr("carriertype") = PickupShippingId, "*", "") & dr("name") & IIf(dr("insurance") > 0 AndAlso Order.ShipmentInsured, "<br /><span class=""smallermag"" style=""font-weight:normal"">Insurance Included</span>", "") & IIf(dr("signature") > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1, "<br /><span class=""smallermag"" style=""font-weight:normal"">Signature Confirmation Included</span>", "") & "</td>" & vbCrLf
                            '     s &= "<td class=""price2"">" & IIf(dr("carriertype") = PickupShippingId, "No Charge", IIf(dr("carriertype") = DefaultShippingId AndAlso FreeShipping, "<span class=""redbold"">FREE!</span><br />" & IIf(dr("subtotal") <= 0, "<span class=""strike"">", "<span>") & FormatCurrency(dr("subtotal")) & "</span>", IIf(dr("carriertype") = TruckShippingId AndAlso FreeShipping AndAlso FormatCurrency(dr("subtotal")) = 0, "<span class=""strike"">" & FormatCurrency(dr("subtotal")) & "</span><br /><span class=""redbold"">FREE!</span>", FormatCurrency(dr("total"))))) & "</td>" & vbCrLf & _
                            '      "</tr>"
                            If (shippingMethod = Utility.Common.TruckShippingId) Then
                                If System.Web.HttpContext.Current.Request.Path.Contains("store/confirmation.aspx") Or System.Web.HttpContext.Current.Request.Path.Contains("members/orderhistory/view.aspx") Then
                                    Try
                                        System.Web.HttpContext.Current.Session("orderIdRender") = Order.OrderId
                                        Dim htmlFreight As String = Utility.Common.RenderUserControl("~/controls/checkout/confirm-freight-option.ascx")
                                        System.Web.HttpContext.Current.Session("orderIdRender") = Nothing
                                        If Not String.IsNullOrEmpty(htmlFreight) Then
                                            s &= htmlFreight

                                        End If
                                    Catch ex As Exception

                                    End Try
                                End If

                            End If
                        End If

                    End While
                    Core.CloseReader(dr)
                Catch ex As Exception
                    Core.CloseReader(dr)
                    Email.SendError("ToError500", "BaseShoppingCart.vb > GetShippingLines", System.Web.HttpContext.Current.Request.RawUrl & "<br>MemberId: " & Common.GetCurrentMemberId() & "<br>UserName: " & Session("Username") & "<br><br>SQL: " & SQL & "<br><br>Public Function GetShippingLines() As String<br><br>Exception: " & ex.ToString())
                End Try
            Else

                s = "<tr>" & vbCrLf &
                        "<td class='left'></td><td colspan='2' class=""label-text"">" & GetShippingTBDLine(Utility.Common.GetShippingInternationalMethodName()) & "</td><td class='right'></td>" & vbCrLf &
                  "</tr>"

                If OnlyOversizeItems() Then
                    s = "<tr>" & vbCrLf &
                          "<td class='left'></td><td colspan='2' class=""label-text"">" & GetShippingTBDLine(Utility.Common.GetShippingInternationalMethodName()) & "</td><td class='right'></td>" & vbCrLf &
                          "</tr>"
                    Return s
                End If
                'If HasHazMatItems() Then
                '    s = "<tr>" & vbCrLf & _
                '          "<td colspan='2' class=""titprice"">" & GetShippingTBDLine(Utility.Common.GetShippingInternationalMethodName()) & "</td>" & vbCrLf & _
                '          "</tr>"
                '    Return s
                'End If

                If Utility.Common.CheckShippingInternational(DB, Order) = False Then
                    Return s
                Else
                    s = ""
                End If

                s = GetShippingLineDetails()
            End If
            Return s
        End Function

        Public Function GetMailShippingLines() As String
            Dim s As String = String.Empty
            Dim strFreightShipping As String = String.Empty
            Dim FreeShipping As Boolean = False
            Dim bFreeShipping As String = False

            FreeShipping = Order.IsFreeShipping
            bFreeShipping = FreeShipping

            If CheckShippingSpecialUS() Then
                s = GetMailShippingLineDetails()
            ElseIf Utility.Common.CheckShippingInternational(DB, Order) = False Then 'Order.BillToCountry = "US" AndAlso Order.IsSameAddress OrElse Order.ShipToCountry = "US" Then
                Dim dr As SqlDataReader = Nothing
                Dim SQL As String = String.Empty
                Dim prefix As String = String.Empty
                Dim shippingCode As String = String.Empty


                Try
                    Dim strSignature As String = ""
                    Dim strInsurance As String = ""
                    Dim insurance As Double = 0

                    Dim shipping As Double = 0

                    SQL = "select coalesce(Prefix,'') as Prefix,sm.Code, carriertype, COALESCE(FreeShipping,0) as FreeShipping,subtotal, total, [name], coalesce(insurance,0) as insurance,[dbo].[fc_StoreOrder_GetOrderShippingInsurance](OrderId,carriertype) as TotalInsurance from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"

                    dr = DB.GetReader(SQL)
                    While dr.Read
                        shippingCode = dr("Code")
                        prefix = dr("Prefix")
                        If (prefix = Utility.Common.ShippingTBD) Then
                            s &= "<tr style='height: 35px'>" & vbCrLf &
                         "<td colspan='2' style=""border-bottom: dotted 1px #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;height: 35px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">" & GetMailShippingTBDLine(dr("carriertype")) & "</td>" & vbCrLf &
                             "</tr>"

                        Else
                            FreeShipping = bFreeShipping
                            If FormatCurrency(dr("subtotal")) = 0 And FreeShipping Then
                                FreeShipping = True
                            Else
                                FreeShipping = Order.IsFreeShipping
                            End If
                            strSignature = ""
                            strInsurance = ""
                            insurance = dr("insurance")
                            shipping = FormatCurrency(dr("total"))

                            If Common.USShippingCode().Contains(shippingCode) AndAlso (Order.SignatureConfirmation > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1) Then
                                strSignature = "<tr><td style=""border-bottom: dotted 1px #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;height: 22px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">Signature Confirmation </td>" & vbCrLf
                                strSignature &= "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;text-align: right;vertical-align: bottom;color:#333333;"">" & FormatCurrency(Order.SignatureConfirmation) & "</td></tr>"
                                shipping = shipping - Order.SignatureConfirmation
                            End If

                            If Common.USShippingCode().Contains(shippingCode) AndAlso Order.ShipmentInsured Then
                                If Order.Insurance > 0 Then
                                    shipping = shipping - Order.Insurance
                                    strInsurance = "<tr><td style=""border-bottom: dotted 1px #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;height: 22px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">Insurance Included</td>" & vbCrLf
                                    strInsurance &= "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;text-align: right;vertical-align: bottom;color:#333333;"">" & FormatCurrency(Order.Insurance) & "</td></tr>"
                                ElseIf (insurance > 0) Then
                                    Dim InsuranceValue As Double = dr("TotalInsurance")
                                    shipping = shipping - InsuranceValue
                                    strInsurance = "<tr><td style=""border-bottom: dotted 1px #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;height: 22px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">Insurance Included</td>" & vbCrLf
                                    strInsurance &= "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;text-align: right;vertical-align: bottom;color:#333333;"">" & FormatCurrency(InsuranceValue) & "</td></tr>"
                                End If
                            End If
                            If (shipping < 0) Then
                                shipping = 0
                            End If

                            s &= "<tr>" & vbCrLf &
                        "<td style=""border-bottom: dotted 1px #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;height: 22px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">" & IIf(dr("carriertype") = PickupShippingId, "*", "")
                            If Common.InternationalShippingId.Contains(Order.CarrierType) Then
                                s &= "International Shipping"
                            Else
                                s &= dr("name").ToString()
                            End If

                            s &= "</td>" & vbCrLf
                            s &= "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;padding-top: 5px;text-align: right;vertical-align: bottom;color:#333333;"">"
                            If (dr("carriertype") = PickupShippingId) Then
                                s &= "No Charge"
                            ElseIf dr("carriertype") = DefaultShippingId AndAlso FreeShipping Then
                                Dim totalFreeShip As Double = dr("FreeShipping")
                                If totalFreeShip > 0 Then
                                    s &= "<span style=""font-weight: bold;color: #bb0008;"">FREE!</span><br /><span style=""text-decoration: line-through;color:#333333;"">" & FormatCurrency(totalFreeShip) & "</span>"
                                Else
                                    s &= "<span style=""font-weight: bold;color: #bb0008;"">FREE!</span>"

                                End If


                            ElseIf dr("carriertype") = TruckShippingId AndAlso FreeShipping AndAlso FormatCurrency(dr("subtotal")) = 0 Then
                                s &= "<span style=""text-decoration: line-through;color:#333333;"">" & FormatCurrency(shipping) & "</span><br /><span style=""font-weight: bold;color: #bb0008;"">FREE!</span>"
                            Else
                                s &= FormatCurrency(shipping)
                            End If
                            s &= "</td></tr>"
                            s &= strInsurance & strSignature
                        End If
                    End While
                    Core.CloseReader(dr)
                Catch ex As Exception
                    Core.CloseReader(dr)
                    Email.SendError("ToError500", "BaseShoppingCart.vb - GetMailShippingLines", "MemberId: " & Common.GetCurrentMemberId() & "<br>UserName: " & Session("Username") & "<br><br>Public Function GetMailShippingLines() As String<br><br>Exception: " & ex.ToString())
                End Try

            Else
                If OnlyOversizeItems() Then
                    s = "<tr style=""height: 35px"">" & vbCrLf &
                          "<td  colspan='2' style=""border-bottom: dotted 1px #CCCCCC;font: bold 14px/25px Open Sans;padding-bottom: 5px;height: 35px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">" & GetMailShippingTBDLine(Common.USPSPriorityShippingId()) & "</td>" & vbCrLf &
                          "</tr>"
                Else
                    s = GetMailShippingLineDetails()
                End If
            End If
            Return s
        End Function

        Public Function GetShippingLineDetails() As String
            Dim s As String = String.Empty
            Dim FreeShipping As Boolean = False
            Dim strFreightShipping As String = ""
            If GetFreeShipping() = True Then
                FreeShipping = True
            End If

            Dim dr As SqlDataReader = Nothing
            Dim SQL As String = String.Empty
            Dim totalFreight As Double = 0

            Try
                Dim isShowSignatureConfirm As Boolean = False
                Dim isViewAdmin As Boolean = Utility.Common.IsViewFromAdmin
                If Not isViewAdmin Then
                    SQL = "select carriertype, subtotal, total, [name], coalesce(insurance,0) as insurance,coalesce(signature,0) as signature from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"
                Else
                    SQL = "select carriertype, subtotal, total, [name], coalesce(insurance,0) as insurance,coalesce(signature,0) as signature, (select convert(decimal(15, 2), CEILING(SUM(a.Weight*a.Quantity) * 100) / 100) from StoreCartItem a where a.CarrierType = sci.CarrierType and a.Type = 'item' and a.OrderId = " & OrderId & ") as sum from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"

                End If

                dr = DB.GetReader(SQL)
                While dr.Read
                    isShowSignatureConfirm = False
                    If Not String.IsNullOrEmpty(Order.OrderNo) Then
                        If Order.SignatureConfirmation > 0 And Order.ResidentialFee > 0 Then
                            isShowSignatureConfirm = True
                        End If
                    Else
                        If dr("signature") > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1 Then
                            isShowSignatureConfirm = True
                        End If
                    End If
                    If FormatCurrency(dr("subtotal")) = 0 And FreeShipping Then
                        FreeShipping = True
                    Else
                        FreeShipping = Order.IsFreeShipping
                    End If
                    If dr("carriertype") = TruckShippingId Then
                        totalFreight = dr("total")
                        If totalFreight > 0 Then
                            strFreightShipping = "<tr>" & vbCrLf &
                        "<td class='left'></td><td  class=""label-text"">" & dr("name") & "</td>" & vbCrLf &
                        "<td class=""label-data"" style=""border-bottom:1px dotted #cccccc;padding:5px 0px 5px 15px;text-align:right"" >" & FormatCurrency(totalFreight) & "</td><td class='right'></td></tr>"
                        Else

                            strFreightShipping = "<tr>" & vbCrLf &
                                                     "<td class='left'></td><td colspan='2' class=""label-text"" align=""left"">" & GetShippingTBDLine("Freight Delivery") & " </td><td class='right'></td>" & vbCrLf &
                                                     "</tr>"
                        End If
                    Else
                        Dim total As Double = dr("total")
                        Dim strTotal As String = String.Empty
                        If (total <= 0) Then
                            s &= "<tr>" & vbCrLf &
                        "<td class='left'></td><td  colspan='2' class=""label-text"">" & GetShippingTBDLine(dr("name")) & "</td><td class='right'></td>" & vbCrLf &
                        "</tr>"

                        Else
                            strTotal = FormatCurrency(total)
                            'Hardcode text international shipping
                            Dim MethodName As String = "International Shipping"
                            If Not Common.InternationalShippingId.Contains("," & dr("carriertype") & ",") Then
                                MethodName = dr("name")
                            End If

                            Dim strWeight As String = String.Empty
                            If isViewAdmin Then
                                strWeight = "<div>" & IIf(dr("sum") Mod 1 = 0, CDbl(dr("sum")).ToString("C0").ToString().Replace("$", ""), CDbl(dr("sum")).ToString("C0").ToString().Replace("$", "")) & " lbs</div>"
                            End If

                            s &= "<tr>" & vbCrLf &
                        "<td class='left'></td><td  class=""label-text"">" & IIf(dr("carriertype") = PickupShippingId, "*", "") & MethodName & IIf(dr("insurance") > 0 AndAlso Order.ShipmentInsured, "<br /><span class=""smallestmag"" style=""font-weight:normal"">Insurance Included</span>", "") & IIf(isShowSignatureConfirm, "<br /><span class=""smallestmag"" style=""font-weight:normal"">Signature Confirmation Included</span>", "") & "</td>" & vbCrLf &
                        "<td class=""label-data"" style=""border-bottom:1px dotted #cccccc;padding:5px 0px 5px 15px;text-align:right"" >" & IIf(dr("carriertype") = PickupShippingId, "No Charge", IIf(dr("carriertype") = DefaultShippingId AndAlso FreeShipping, "<span class=""strike"">" & FormatCurrency(dr("subtotal")) & "</span><br /><span class=""redbold"">FREE!</span>", strTotal)) & strWeight & "</td>" & vbCrLf &
                        "<td class='right'></td></tr>"
                        End If
                    End If

                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "BaseShoppingCart.vb - GetShippingLineDetails", "MemberId: " & Common.GetCurrentMemberId() & "<br>UserName: " & Session("Username") & "<br><br>Public Function GetShippingLineDetails() As String<br><br>Exception: " & ex.ToString())
            End Try

            If s.Contains("To Be Determined") And strFreightShipping.Contains("To Be Determined") Then
                s = strFreightShipping
            Else
                s += strFreightShipping
            End If

            Return s
        End Function

        Public Function GetMailShippingLineDetails() As String
            Dim s As String = String.Empty
            Dim FreeShipping As Boolean = False
            Dim strFreightShipping As String = ""
            If GetFreeShipping() = True Then
                FreeShipping = True
            End If

            Dim dr As SqlDataReader = Nothing
            Dim SQL As String = String.Empty
            Try
                SQL = "select carriertype, subtotal, total, [name], coalesce(insurance,0) as insurance,coalesce(signature,0) as signature from storecartitem sci inner join shipmentmethod sm on sci.carriertype = sm.methodid where type = 'carrier' and orderid = " & OrderId & " order by sm.sortorder asc"
                dr = DB.GetReader(SQL)
                While dr.Read
                    If FormatCurrency(dr("subtotal")) = 0 And FreeShipping Then
                        FreeShipping = True
                    Else
                        FreeShipping = Order.IsFreeShipping
                    End If
                    If dr("carriertype") = TruckShippingId Then
                        strFreightShipping = "<tr>" & vbCrLf &
                                                "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;height: 22px;padding-bottom: 5px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">" & IIf(dr("carriertype") = PickupShippingId, "*", "") & dr("name") & IIf(dr("insurance") > 0 AndAlso Order.ShipmentInsured, "<br /><span  style=""font-size: 10px;color: #be048d;line-height: 13px;font-weight:normal"">Insurance Included</span>", "") & IIf(dr("signature") > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1, "<br /><span style=""font-size: 10px;color: #be048d;line-height: 13px;font-weight:normal"">Signature Confirmation Included</span>", "") & "</td>" & vbCrLf &
                                                "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;height: 22px;padding-bottom: 5px;text-align: right;vertical-align: bottom;color:#454545"">" & IIf(dr("carriertype") = PickupShippingId, "No Charge", IIf(dr("carriertype") = DefaultShippingId AndAlso FreeShipping, "<span style=""text-decoration: line-through;color:#333333;"">" & FormatCurrency(dr("subtotal")) & "</span><br /><span style=""font-weight: bold;color: #bb0008;"">FREE!</span>", FormatCurrency(dr("total")))) & "</td>" & vbCrLf &
                                                "</tr>"
                    Else
                        Dim total As Double = dr("total")
                        Dim strTotal As String = String.Empty
                        If (total > 0) Then
                            strTotal = FormatCurrency(dr("total"))
                            s &= "<tr>" & vbCrLf &
                        "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;height: 22px;padding-bottom: 5px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">" & IIf(dr("carriertype") = PickupShippingId, "*", "") & IIf(Common.InternationalShippingId.Contains(dr("carriertype")), "International Shipping", dr("name")) & IIf(dr("insurance") > 0 AndAlso Order.ShipmentInsured, "<br /><span  style=""font-weight:normal;font-size: 10px;color: #be048d;line-height: 13px;"">Insurance Included</span>", "") & IIf(dr("signature") > 0 AndAlso Order.IsSignatureConfirmation And Order.ShipToAddressType <> 1, "<br /><span  style=""font-weight:normal;font-size: 10px;color: #be048d;line-height: 13px;"">Signature Confirmation Included</span>", "") & "</td>" & vbCrLf &
                        "<td style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;height: 22px;padding-bottom: 5px;text-align: right;vertical-align: bottom;color:#454545"">" & IIf(dr("carriertype") = PickupShippingId, "No Charge", IIf(dr("carriertype") = DefaultShippingId AndAlso FreeShipping, "<span style=""text-decoration: line-through;color:#333333;"">" & FormatCurrency(dr("subtotal")) & "</span><br /><span style=""font-weight: bold;color: #bb0008;"">FREE!</span>", strTotal)) & "</td>" & vbCrLf &
                        "</tr>"
                        Else
                            s &= "<tr style=""height: 35px"">" & vbCrLf &
                                "   <td colspan='2' style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;height: 35px;padding-bottom: 5px;text-align: left;vertical-align: bottom;width: 250px;color:#333333;"">" & vbCrLf &
                                "       " & GetMailShippingTBDLine(dr("carriertype")) & vbCrLf &
                                "  </td>" & vbCrLf &
                                "</tr>"
                        End If
                    End If

                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "BaseShoppingCart.vb - GetMailShippingLineDetails", "MemberId: " & Common.GetCurrentMemberId() & "<br>UserName: " & Session("Username") & "<br><br>Public Function GetMailShippingLineDetails() As String<br><br>Exception: " & ex.ToString())

            End Try

            If HasOversizeItems() Then
                strFreightShipping = "<tr style=""height: 35px"">" & vbCrLf &
                                               "<td  colspan='2' class=""larger bold border-bottom"" style=""border-bottom: 1px dotted #CCCCCC;font: bold 14px/25px Open Sans;height: 35px;padding-bottom: 5px;text-align: left;vertical-align: bottom;color:#454545"" align=""left""> " & GetMailShippingTBDLine(4) & "</td>" & vbCrLf &
                                               "</tr>"
            End If
            s += strFreightShipping
            Return s
        End Function

        Protected Shared Function ConvertDataFromDataTableToHTML(ByVal dt As DataTable, ByRef title As String, ByVal isCheckDoExportCartItem As Boolean) As String
            Dim result As String = String.Empty
            If (dt Is Nothing) Then
                Return String.Empty
            End If
            result = "<table width=""2800"" border=""1"" cellpadding=""2"" cellspacing=""2""><tr>"
            For Each c As DataColumn In dt.Columns
                result = result & "<td style='border: solid 1px #BE048C;'><b>" & c.ColumnName & "</b></td>"
            Next
            result = result & "</tr>"
            Dim value As String = String.Empty
            Dim lstItemDoexportFlase As String = String.Empty
            If (isCheckDoExportCartItem) Then
                Dim doExport As String = String.Empty
                Dim cartItemId As String = String.Empty
                Dim sku As String = String.Empty
                For Each r As DataRow In dt.Rows
                    Try
                        doExport = r("DoExport")
                        If (doExport = "False") Then
                            ''get CartItemId, SKU
                            Try
                                cartItemId = r("CartItemId")
                            Catch ex As Exception

                            End Try
                            Try
                                sku = r("sku")
                            Catch ex As Exception
                                sku = String.Empty
                            End Try
                            lstItemDoexportFlase = lstItemDoexportFlase & ",CartId:" & cartItemId & "-sku:" & sku
                        End If
                    Catch ex As Exception

                    End Try
                    result = result & "<tr>"
                    For Each c As DataColumn In dt.Columns
                        value = r(c.ColumnName).ToString()
                        If (String.IsNullOrEmpty(value)) Then
                            value = "&nbsp;"
                        End If
                        result = result & "<td style='border: solid 1px #BE048C;'>" & value & "</td>"
                    Next
                    result = result & "</tr>"
                Next
            Else
                For Each r As DataRow In dt.Rows
                    result = result & "<tr>"
                    For Each c As DataColumn In dt.Columns
                        value = r(c.ColumnName).ToString()
                        If (String.IsNullOrEmpty(value)) Then
                            value = "&nbsp;"
                        End If
                        result = result & "<td style='border: solid 1px #BE048C;'>" & value & "</td>"
                    Next
                    result = result & "</tr>"
                Next
            End If
            If Not (String.IsNullOrEmpty(lstItemDoexportFlase)) Then
                title = title & ",DoExport=False" & lstItemDoexportFlase
            End If
            result = result & "</table>"
            Return result
        End Function

        Public Sub ReviseCartItem(ByVal OrderId As Integer, ByVal memberId As Integer)
            If (StoreCartItemRow.ReviseCartItem(DB, OrderId)) Then
                'Dim dvCartItem As DataView = DB.GetDataView("select ItemId from storecartitem where orderid = " & OrderId & " and [type] = 'item'")
                'If Not (dvCartItem Is Nothing) Then
                '    Dim itemId As Integer
                '    For i As Integer = 0 To dvCartItem.Count - 1
                '        itemId = CInt(dvCartItem(i)("ItemId"))
                '        CacheUtils.RemoveCache(String.Format(StoreItemRow.cachePrefixKey & "GetRowByMemberLogin_{0}_{1}", itemId, memberId))
                '    Next
                'End If
                RecalculateOrderDetail("")
            End If
        End Sub
        Public Sub ReviseCart(ByVal QtyEdit As String, ByVal SkuRemove As String)
            Exit Sub
            Try
                Dim arrUpdate As String() = QtyEdit.Replace(Right(QtyEdit, 1), "").Split(",")
                Dim arrRemove As String() = SkuRemove.Replace(Right(QtyEdit, 1), "").Split(",")
                Dim i As Integer
                If arrRemove.Length > 0 Then
                    For i = 0 To arrRemove.Length - 1
                        RemoveCartItem(arrRemove(i))
                    Next
                    RecalculateOrderDetail("PaymentStatus")
                End If
                Dim ci As StoreCartItemRow
                If arrUpdate.Length > 0 Then
                    Dim upCartId, upQty As Integer
                    For i = 0 To arrUpdate.Length - 1
                        upCartId = arrUpdate(i).Split("|")(0)
                        upQty = arrUpdate(i).Split("|")(1)
                        ci = StoreCartItemRow.GetRow(DB, upCartId)
                        ci.Quantity = upQty
                        ci.Update()
                    Next
                    RecalculateOrderDetail("PaymentStatus")
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "BaseShoppingCart.vb - ReviseCart", "MemberId: " & Common.GetCurrentMemberId() & "<br>UserName: " & Session("Username") & "<br><br>Public Function ReviseCart(ByVal " & QtyEdit & ", " & SkuRemove & ")<br><br>Exception: " & ex.ToString())
            End Try
        End Sub


#End Region

#Region "Khoa"

        Private Sub _AdminRecalculateCartItem()
            Dim c As StoreCartItemCollection = GetCartItems()
            For Each ci As StoreCartItemRow In c
                ci.SubTotal = FormatNumber(ci.Price * ci.Quantity, 2)
                ci.Total = ci.SubTotal
                ci.Update()
            Next
        End Sub

        Public Sub AddOrderCoupon(ByVal couponCode As String)
            Dim currentcoupon As String = Order.PromotionCode
            If Not String.IsNullOrEmpty(currentcoupon) Then
                Dim dbOldPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, currentcoupon)
                DB.ExecuteSQL("Delete from StoreCartItem where IsFreeItem=1 and PromotionId=" & dbOldPromotion.PromotionId & " and OrderId=" & Order.OrderId)
            End If
            Dim sErrorMsg As String = ""
            Dim fSubtotal As Double = 0
            Dim isCheckFreeShip As Boolean = False
            Order.IsPromotionValid = True
            Dim FreeItemIds As String = String.Empty
            Dim dbPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, couponCode)
            If Utility.Common.IsPromotionFreeItem(dbPromotion.PromotionType) Then
                ''check remove free item
                DB.ExecuteSQL("Delete from StoreCartItem where IsFreeItem=1 and PromotionId=" & dbPromotion.PromotionId & " and OrderId=" & Order.OrderId)
                Order.Discount = 0
                Dim lstPromotionItem As StorePromotionItemCollection = StorePromotionItemRow.GetListByPromotion(DB, dbPromotion.PromotionId)
                For Each pitem As StorePromotionItemRow In lstPromotionItem
                    If String.IsNullOrEmpty(FreeItemIds) Then
                        FreeItemIds = pitem.ItemId
                    Else
                        FreeItemIds = FreeItemIds & "," & pitem.ItemId
                    End If
                    Dim cartitemid As Integer = Add2Cart(pitem.ItemId, Nothing, 1, 0, "Myself", "", "", 0, "", True, True, Nothing)
                    If cartitemid > 0 Then
                        Dim sql As String = "Update StoreCartItem set PromotionId=" & dbPromotion.PromotionId & ",Total=0 where CartItemId=" & cartitemid
                        DB.ExecuteSQL(sql)
                    End If
                Next
                If Not String.IsNullOrEmpty(FreeItemIds) Then
                    DB.ExecuteSQL("Update StoreCartItem  set FreeItemIds='" & FreeItemIds & "' where OrderId=" & Order.OrderId & " and IsFreeItem=1 and PromotionId=" & dbPromotion.PromotionId)
                End If
            End If
            DB.ExecuteSQL("Update StoreOrder set PromotionCode='" & couponCode & "' where OrderId=" & Order.OrderId)



        End Sub
        Private Sub _ProcessWebPromotion(ByVal bOrderDetail As Boolean)

            Dim sErrorMsg As String = ""
            Dim fSubtotal As Double = 0
            Dim isCheckFreeShip As Boolean = False
            If m_FromAdmin Then
                Order.IsPromotionValid = StorePromotionRow.ValidatePromotionAdmin(DB, Nothing, Order.PromotionCode, sErrorMsg, Order.SubTotal + Order.Discount)
            Else
                Order.IsPromotionValid = StorePromotionRow.ValidatePromotion(DB, Nothing, Order.PromotionCode, sErrorMsg, Order.SubTotal + Order.Discount)
            End If
            Dim dbPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, Order.PromotionCode)

            If Order.IsPromotionValid Then

                Order.PromotionMessage = dbPromotion.Message
                Order.PromotionCode = dbPromotion.PromotionCode
                Order.IsFreeShipping = dbPromotion.IsFreeShipping
                ''check has free item
                If Utility.Common.IsPromotionFreeItem(dbPromotion.PromotionType) Then
                    Dim countFreeItem As Integer = DB.ExecuteScalar("Select count(*) from StoreCartItem where OrderId=" & Order.OrderId & " and  IsFreeItem=1 and PromotionId=" & dbPromotion.PromotionId)
                    If countFreeItem < 1 Then
                        Dim FreeItemIds As String = String.Empty
                        Dim lstPromotionItem As StorePromotionItemCollection = StorePromotionItemRow.GetListByPromotion(DB, dbPromotion.PromotionId)
                        For Each pitem As StorePromotionItemRow In lstPromotionItem
                            If String.IsNullOrEmpty(FreeItemIds) Then
                                FreeItemIds = pitem.ItemId
                            Else
                                FreeItemIds = FreeItemIds & "," & pitem.ItemId
                            End If
                            Dim cartitemid As Integer = Add2Cart(pitem.ItemId, Nothing, 1, 0, "Myself", "", "", 0, "", True, True, Nothing)
                            If cartitemid > 0 Then
                                DB.ExecuteSQL("Update StoreCartItem set PromotionId=" & dbPromotion.PromotionId & " where CartItemId=" & cartitemid)
                            End If
                        Next
                        If Not String.IsNullOrEmpty(FreeItemIds) Then
                            DB.ExecuteSQL("Update StoreCartItem  set FreeItemIds='" & FreeItemIds & "' where OrderId=" & Order.OrderId & " and IsFreeItem=1 and PromotionId=" & dbPromotion.PromotionId)
                        End If

                    End If
                End If
                If dbPromotion.IsItemSpecific Then
                    fSubtotal = DB.ExecuteScalar("SELECT COALESCE(SUM(CASE WHEN IsOnSale = 1 Then SalePrice Else Price End * Quantity),0) FROM StoreCartItem WHERE OrderId=" & OrderId & " AND (itemid in (select itemid from storeitem where brandid in (select brandid from storepromotionitem where promotionid = " & dbPromotion.PromotionId & ")) or ItemId IN (SELECT ItemId FROM StorePromotionItem WHERE PromotionId=" & dbPromotion.PromotionId & ") OR ItemId IN (SELECT ItemId FROM StoreDepartmentItem WHERE DepartmentId IN (SELECT Coalesce(DepartmentId,0) FROM StorePromotionItem WHERE PromotionId=" & dbPromotion.PromotionId & ")))")
                Else
                    fSubtotal = Order.SubTotal
                End If

                If Order.IsFreeShipping And Order.CarrierType = DefaultShippingId Then 'Check freeshipping theo Promotion
                    Order.IsFreeShipping = True
                    DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 1 WHERE Type = 'item' AND CarrierType = " & DefaultShippingId & " AND OrderId = " & OrderId)
                ElseIf Order.IsFreeShipping = False And Order.CarrierType = DefaultShippingId Then 'Check freeshipping theo SysParam
                    isCheckFreeShip = True
                    _CheckFreeShippingWithPromotionItem()
                    '' _CheckFreeShippingWithTotal(Order)
                Else '(Order.IsFreeShipping And Order.CarrierType <> DefaultShippingId) Then
                    Order.IsFreeShipping = False
                End If

                If dbPromotion.PromotionType = "Monetary" Then
                    If dbPromotion.Discount > fSubtotal Then Order.Discount = fSubtotal Else Order.Discount = dbPromotion.Discount
                    If Order.SubTotal < 0 Then Order.SubTotal = 0

                ElseIf dbPromotion.PromotionType = "Percentage" Then

                    Order.Discount = Utility.Common.RoundCurrency(fSubtotal * (dbPromotion.Discount / 100))

                    If Order.Discount < 0 Then Order.Discount = fSubtotal
                    If Order.SubTotal < 0 Then Order.SubTotal = 0
                End If
            Else
                If (dbPromotion.PromotionId > 0) Then
                    DB.ExecuteSQL("Delete from StoreCartItem where IsFreeItem=1 and PromotionId=" & dbPromotion.PromotionId & " and OrderId=" & Order.OrderId)
                End If

                'Kiem tra neu PromotionCode da add nhung ko con gia tri
                If sErrorMsg.Contains("This promotion has a minimum purchase amount of") Or sErrorMsg.Contains("This promotion has a maximum purchase amount of") Or sErrorMsg.Contains("The promotion code you entered is only valid from") Or sErrorMsg.Contains("The promotion you entered is no longer active on the website") Then
                    Order.IsFreeShipping = False
                    Order.IsPromotionValid = False
                End If

                If Order.CarrierType = DefaultShippingId Then
                    isCheckFreeShip = True
                    _CheckFreeShippingWithPromotionItem()
                    '' _CheckFreeShippingWithTotal(Order)
                ElseIf Not String.IsNullOrEmpty(Order.PromotionCode) Then
                    _CheckFreeShippingWithPromotionItem()
                Else
                    Order.IsFreeShipping = False
                End If

                Order.Discount = 0
                Order.PromotionMessage = IIf(sErrorMsg IsNot Nothing, sErrorMsg, String.Empty)


            End If
            Order.SubTotal = Order.SubTotal - Order.Discount

            If Not bOrderDetail Then
                Order.Update()
            End If

            If isCheckFreeShip Then
                ''  _CheckFreeShippingWithPromotionItem()
                _CheckFreeShippingWithTotal(Order)
            End If

        End Sub

        Private Sub _CheckFreeShippingWithPromotionItem()
            Dim memberId As Integer = Common.GetCurrentMemberId()
            If memberId <= 0 Then
                memberId = 0
            End If

            Dim dv As DataView

            dv = DB.GetDataView("SELECT sc.CartItemId, sc.ItemId, ISNULL(sc.PromotionId, 0) AS PromotionId, sc.CarrierType, sc.IsFreeShipping, si.IsFreeShipping AS 'IsFreeShippingItem' , sp.IsFreeShipping AS 'IsFreeShippingPromotion', [dbo].[fc_StorePromotion_ValidateProductPromotion](sp.PromotionCode," & memberId & ",sc.CartItemId) as ValidateProductPromotion  FROM StoreCartItem sc " &
                        "INNER JOIN StoreItem si ON si.ItemId = sc.ItemId " &
                        "LEFT JOIN StorePromotion sp ON sp.PromotionId = sc.PromotionId " &
                        "WHERE Type = 'item' and sc.PromotionId>0 AND OrderId = " & OrderId)



            If dv.Count > 0 Then
                Dim strTrueItemId As String = String.Empty
                Dim strFalseItemId As String = String.Empty
                Dim CartItemId As Integer = 0
                Dim bFreeShipping As Boolean = False
                Dim bFreeShippingPromotion As Boolean = False
                Dim bFreeShippingItem As Boolean = False
                Dim ValidateProductPromotion As Integer = 0
                For Each drv As DataRowView In dv
                    CartItemId = CInt(drv("CartItemId"))
                    ValidateProductPromotion = CInt(drv("ValidateProductPromotion"))

                    Try
                        bFreeShippingItem = CBool(drv("IsFreeShippingItem"))
                    Catch ex As Exception
                    End Try
                    Try
                        bFreeShippingPromotion = CBool(drv("IsFreeShippingPromotion"))
                    Catch ex As Exception
                    End Try
                    If ValidateProductPromotion = 1 Then
                        'Long: freeshipping only standard shipping
                        If drv("CarrierType") = DefaultShippingId Then
                            bFreeShipping = bFreeShippingPromotion
                        Else
                            bFreeShipping = False
                        End If
                    Else
                        bFreeShipping = bFreeShippingItem
                    End If
                    If bFreeShipping Then
                        strTrueItemId &= "," & CartItemId
                    Else
                        strFalseItemId &= "," & CartItemId
                    End If
                Next

                If Not String.IsNullOrEmpty(strTrueItemId) Then
                    DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 1 WHERE CartItemId IN (" & strTrueItemId.Substring(1) & ")")
                End If

                If Not String.IsNullOrEmpty(strFalseItemId) Then
                    DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 0 WHERE CartItemId IN (" & strFalseItemId.Substring(1) & ")")
                End If
                'Else
                '    DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 0 WHERE OrderId =" & OrderId)
            End If

        End Sub

        Public Sub _CheckFreeShippingWithTotal(ByVal Order As StoreOrderRow)
            If Order.CarrierType <> DefaultShippingId Then
                Exit Sub
            End If
            Order.IsFreeShipping = False
            Dim strFreeShippingOrderAmount As String = SysParam.GetValue("FreeShippingOrderAmount")
            If String.IsNullOrEmpty(strFreeShippingOrderAmount) Then
                Order.IsFreeShipping = False
            Else
                Dim FreeShippingOrderAmount As Double = CDbl(strFreeShippingOrderAmount)
                Dim PriceTotal As Double = Order.SubTotal ''GetPriceTotal()             
                If PriceTotal >= FreeShippingOrderAmount Then
                    Order.IsFreeShipping = True
                End If
            End If

            If Order.IsFreeShipping = True Then
                DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 1 WHERE Type = 'item' AND CarrierType = " & DefaultShippingId & " AND OrderId = " & OrderId)
            Else
                DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = (Select IsFreeShipping from StoreItem where ItemId=StoreCartItem.ItemId) WHERE Type = 'item' and PromotionId<1 AND OrderId = " & OrderId)
            End If
        End Sub

        Public Shared Function GetUSPSRate(ByVal Weight As Double, ByVal Country As String, ByRef iType As Integer, ByVal OrderId As Integer) As Double
            Dim result As Double = 0
            Dim Group As Integer = 0
            If String.IsNullOrEmpty(Country) Then
                Return result
            End If

            Dim lstCountrySpecial As String = ",AA,APE,KN,TV,IR,GQ,CQ,AQ,AY,AT,FQ,BS,BV,IO,BM,KT,IP,CK,CG,CR,IV,CU,EU,FK,FM,FS,GZ,GO,GK,HM,HQ,IM,JN,DQ,JE,JQ,JU,KQ,RM,MF,MQ,MN,MH,BQ,NE,NF,LQ,PF,PC,PS,SX,PG,SH,SC,SB,SV,TL,TE,US,WQ,WF,WE,WI,NS,"
            Dim WeightRound As Double = Math.Ceiling(Weight)

            Try
                Dim strSession As String = Web.HttpContext.Current.Session("GetUSPSRate")
                If strSession IsNot Nothing AndAlso strSession.Contains(String.Format("{0}|{1}", Weight, Country)) Then
                    result = strSession.Split("|")(2)
                    iType = strSession.Split("|")(3)
                    Return result
                End If
            Catch ex As Exception
            End Try

            If lstCountrySpecial.Contains("," & Country & ",") Then
                Web.HttpContext.Current.Session("GetUSPSRate") = String.Format("{0}|{1}|{2}", Weight, Country, Math.Round(result, 2), 0)
                Return result
            End If

            Dim reader As SqlDataReader = Nothing
            Dim SP As String = "sp_GetRateUSPS"
            Dim msg As String = ""
            Try
                iType = IIf(Weight > ConfigData.FirstClassLimitWeight(), 0, 1) 'Type 0=USPS Priority, 1=First Class Package
                If iType = 1 Then
                    iType = IIf(CheckIsFirstClassPackage(OrderId), 1, 0) ' CheckIsFirstClassPackage cho phep dung First Class Package (TRUE = duoc xai)
                End If
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "CountryCode", DbType.String, Country)
                db.AddInParameter(cmd, "Weight", DbType.Double, WeightRound)
                db.AddInParameter(cmd, "Type", DbType.Int16, iType)
                reader = db.ExecuteReader(cmd)
                If (reader.Read()) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Fee"))) Then
                        result = CDbl(reader("Fee").ToString())
                    Else
                        result = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Group"))) Then
                        Group = CInt(reader("Group").ToString())
                    Else
                        Group = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Message"))) Then
                        msg = reader("Message").ToString()
                    Else
                        msg = ""
                    End If
                End If
                Core.CloseReader(reader)

                If Not (iType = 1 AndAlso (Group > 0 And Group <= 9)) Then
                    iType = 0
                End If

                If result = 0 Then
                    If Country <> "AF" And Country <> "AC" And Country <> "SO" And Country <> "FQ" And Country <> "WI" And Country <> "LE" And Country <> "BS" And Country <> "GI" And Country <> "CG" Then
                        Components.Email.SendError("ToError500", "[GetRateUSPS] GetUSPSRate result = 0", "Link: " & Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString() & "<br>Country: " & Country & "<br>Weight: " & Weight & "<br>Group: " & Group & "<br>Result: " & result & "<br>OrderId : " & System.Web.HttpContext.Current.Session("OrderId") & "<br>MemberId : " & Common.GetCurrentMemberId() & "<br>message: " & msg)
                    End If
                End If
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "[GetRateUSPS] Exception", "Link: " & Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString() & "<br>Country: " & Country & "<br>Weight: " & Weight & "<br>Result: " & result & "<br>Exception: " & ex.ToString() + "")
            End Try

            Web.HttpContext.Current.Session("GetUSPSRate") = String.Format("{0}|{1}|{2}|{3}", Weight, Country, Math.Round(result, 2), iType)
            Return Math.Round(result, 2)
        End Function
        Public Shared Function GetFedExRate(ByVal DB As Database, ByVal Weight As Double, ByVal MethodId As Integer, ByVal ZipCode As String, ByVal Country As String, ByVal Residential As Boolean) As Double
            Dim strLog As String = String.Empty
            Dim result As Double = 0
            Dim msg As String = ""
            Weight = Math.Ceiling(Weight)
            Dim strSession As String = Web.HttpContext.Current.Session("GetFedExRate")
            Dim strRestricted As String = String.Empty

            If strSession IsNot Nothing AndAlso strSession.Contains(String.Format("{0}|{1}|{2}", ZipCode, MethodId, Weight)) Then
                Try
                    result = strSession.Split("|")(3)
                    Return result
                Catch ex As Exception

                End Try
            End If


            If Not String.IsNullOrEmpty(ZipCode) Then
                If ZipCode.Split("-").Length > 1 Then
                    ZipCode = ZipCode.Split("-")(0)
                End If
            End If


            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                End If
            End If
            Dim shipping As New ShippingFedExRow
            shipping.ZipCode = ZipCode
            shipping.Weight = Weight
            shipping.MethodId = MethodId
            shipping.Country = Country
            result = ShippingFedExRow.GetShippingRateFedEx(shipping, Zone, msg)
            strLog = "Shipping from DB:" & result & "<br/>"
            Dim isDAS As Boolean = 0
            If result = 0 Then
                Dim getRate As New FedexRate(ZipCode, Weight, MethodId, Country)
                result = getRate.Rate
                strLog = strLog & "Shipping from FedEx:" & result & "<br/>"
                If result = 0 Then
                    If getRate.Zone = 0 And getRate.Msg IsNot Nothing And getRate.Weight <= 150 Then
                        Try
                            ShippingFedExRow.ShippingFedExRestricted(shipping, shipping.MethodId)
                            strRestricted = "1"
                        Catch ex As Exception
                            Email.SendReport("ToError500", String.Format("[GetFedExRate] Exception sp_ShippingFedExRestricted"), "Link: " & rawURL & "<br>OrderId=" & System.Web.HttpContext.Current.Session("OrderId") & "<br>ZipCode: " & ZipCode & "<br>Weight: " & Weight & "<br>MethodId: " & MethodId & "<br>Country: " & Country & "<br>Result: " & result & "<br>Exception: " & ex.ToString() + "")
                        End Try
                    ElseIf Weight <= 150 Then
                        Email.SendReport("ToReportFedEx", String.Format("[GetFedExRate] WebService Fee = 0"), "Link: " & rawURL & "<br>OrderId=" & System.Web.HttpContext.Current.Session("OrderId") & "<br>ZipCode: " & ZipCode & "<br>Weight: " & Weight & "<br>MethodId: " & MethodId & "<br>Country: " & Country & "<br>WS Zone:" & getRate.Zone & "<br>WS Fee:" & getRate.Rate & "<br>WS Msg:" & getRate.Msg & "<br>*****<br>" & msg)
                    End If

                Else
                    ShippingFedExRow.Insert(shipping, getRate.Zone, result, Zone)
                    'Kiem tra nhung Method FedEx khac co active Zone khong
                    Dim MethodIds() As Integer = {16, 17, 18}
                    For Each i As Integer In MethodIds
                        If i = MethodId Then
                            Continue For
                        End If
                        ''check exists restrict
                        If Not ShippingFedExRow.CheckMethodRestricted(shipping.ZipCode, i) Then
                            Try
                                Dim fed As New FedexRate(ZipCode, Weight, i, Country)
                                If fed.Rate = 0 And fed.Zone = 0 And fed.Msg IsNot Nothing And fed.Weight <= 150 Then
                                    ShippingFedExRow.ShippingFedExRestricted(shipping, i)
                                End If
                            Catch ex As Exception
                                Email.SendReport("ToError500", String.Format("[GetFedExRate] Exception 2 sp_ShippingFedExRestricted"), "Link: " & rawURL & "<br>ZipCode: " & ZipCode & "<br>Weight: " & Weight & "<br>MethodId: " & i & "<br>Exception: " & ex.ToString() + "")
                            End Try
                        End If

                    Next


                End If
                isDAS = IIf(getRate.FeeDAS > 0, True, False)
            Else

                'Check DAS
                Dim isConectFedEx As Boolean = True
                Dim dtDAS As DataTable = DB.GetDataTable("Select IsDAS from ShippingFedExFee where MethodID=" & MethodId & " and zipCode='" & ZipCode & "'")
                If Not dtDAS Is Nothing Then
                    If dtDAS.Rows.Count > 0 Then
                        isConectFedEx = False
                        isDAS = CBool(dtDAS.Rows(0)("IsDAS"))
                    End If
                End If
                If isConectFedEx Then
                    Dim getRate As New FedexRate(ZipCode, Weight, MethodId, Country)
                    If getRate.FeeDAS > 0 Then
                        isDAS = True
                    Else
                        isDAS = False
                    End If

                    Dim objShippingFee As New ShippingFedExFeeRow
                    objShippingFee.ZipCode = ZipCode
                    objShippingFee.MethodID = MethodId
                    objShippingFee.IsDAS = isDAS
                    ShippingFedExFeeRow.Insert(objShippingFee)
                End If
            End If

            If result > 0 Then
                'DAS
                If isDAS Then
                    If Not Residential Then
                        result += CDbl(ShipmentMethod.GetValue(MethodId, Common.ShipmentValue.DASCommercial))
                    Else
                        result += CDbl(ShipmentMethod.GetValue(MethodId, Common.ShipmentValue.DASResidential))
                    End If
                End If
                result += ((CDbl(ShipmentMethod.GetValue(MethodId, Common.ShipmentValue.FuelRate)) * result) / 100)
            End If

            Web.HttpContext.Current.Session("GetFedExRate") = String.Format("{0}|{1}|{2}|{3}|{4}", ZipCode, MethodId, Weight, Math.Round(result, 2), strRestricted)
            Return Math.Round(result, 2)
        End Function

        Public Sub _RecalculateShipping(ByVal FromFunction As String)
            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                End If
            End If
            Dim isAllow As Boolean = False
            If Not String.IsNullOrEmpty(Utility.ConfigData.PageCalculateShipping) Then
                Dim arr() As String = Utility.ConfigData.PageCalculateShipping.Split(New String() {","}, StringSplitOptions.None)
                If arr.Length > 0 Then
                    For Each page As String In arr
                        If rawURL.Contains(page) Then
                            isAllow = True
                            Exit For
                        End If
                    Next
                End If
            End If
            If Not isAllow AndAlso FromFunction <> "EstimateShipping" Then
                Exit Sub
            End If
            If (Order.IsSameAddress = False AndAlso Order.ShipToZipcode = Nothing) OrElse (Order.IsSameAddress = True AndAlso Order.BillToZipcode = Nothing AndAlso Order.BillToCountry = "US") Then
                Exit Sub
            End If

            If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = Nothing) OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = Nothing) Then
                Exit Sub
            End If

            If CheckShippingSpecialUS() Then
                If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") Or (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") OrElse (Order.BillToCountry = "VI" Or Order.BillToCountry = "PR") Then
                    If HasOversizeItems() = False Then
                        DB.ExecuteSQL("DELETE FROM StoreCartItem WHERE OrderId = " & OrderId & " AND [Type] = 'carrier'")
                    End If

                    Order.Shipping = 0
                    RecalculateShippingInternational()
                End If
            ElseIf (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") Then
                Dim dv As DataView, drv As DataRowView
                Dim additional As Double = 0
                Dim ci As StoreCartItemRow
                Dim Weight As Double = Nothing
                Dim WeightOV As Double = 0
                Dim _methodId As Integer = 0
                Dim MethodIds As String = String.Empty
                Dim ShippingZero As Boolean = False
                Dim MethodName As String = String.Empty
                Dim Ids As String = String.Empty
                Dim SQL As String
                Dim AlreadyAppliedLiftGate As Boolean = False
                Dim AlreadyAppliedScheduledDelivery As Boolean = False
                Dim AlreadyAppliedInsideDelivery As Boolean = False
                Dim AppliedHazMatFee As Boolean = False
                Dim feeShipOversize As Double = 0
                Dim bFromFirstClass As Boolean = False

                SQL = "select sm.Code, coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = ca.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype from storecartitem ca inner join shipmentmethod sm on sm.MethodId = ca.carriertype where orderid = " & Order.OrderId & " and  type = 'item' group by carriertype, sm.Code"
                dv = DB.GetDataView(SQL)

                Dim IsOverSize As Boolean = False
                Dim ShippingTotal As Double = 0
                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)
                    Dim Shipping As Double = 0
                    _methodId = CInt(drv("carriertype"))


                    If _methodId = Utility.Common.TruckShippingId Then
                        IsOverSize = True
                        WeightOV = WeightFlatFee()
                        Weight = drv("weight") - WeightOV
                        feeShipOversize = ShippingOversize(Order)
                    Else
                        If _methodId = Common.FirstClassShippingId Then
                            Order.CarrierType = DefaultShippingId
                            _methodId = DefaultShippingId
                            bFromFirstClass = True
                        ElseIf _methodId = Common.DefaultShippingId AndAlso Order.CarrierType = Common.FirstClassShippingId Then
                            Order.CarrierType = DefaultShippingId
                        End If

                        IsOverSize = False
                        If _methodId = Utility.Common.DefaultShippingId Then
                            If Order.IsFreeShipping Then
                                Weight = drv("weight")
                            Else
                                Weight = drv("weight") - drv("freeweight")
                            End If
                        Else
                            Weight = drv("weight")
                        End If

                    End If


                    If Weight <= 0 And Order.IsFreeShipping = False And _methodId = Utility.Common.DefaultShippingId Then
                        Order.IsFreeShipping = True
                        Order.Update()
                        Weight = drv("weight")
                    End If
                    Weight = Math.Round(Weight, 3)
                    If Ids <> String.Empty Then Ids &= ","
                    Ids &= _methodId.ToString()


                    ci = StoreCartItemRow.GetRow(DB, OrderId, _methodId)
                    ci.Prefix = Nothing
                    If Weight > 0 Then
                        If Common.IsFedExCalculator() AndAlso Common.USShippingCode().Contains(drv("Code").ToString()) Then
                            Shipping = GetFedExRate(DB, Weight, _methodId, Order.ShipToZipcode, Order.ShipToCountry, (Order.ShipToAddressType <> 1))
                            If (Shipping <= 0 And Order.IsFreeShipping = False) Then
                                ci.Prefix = Utility.Common.ShippingTBD
                            Else
                                Dim extraPercent As Integer = DB.ExecuteScalar("Select coalesce(ExtraShippingPercent,0) from ShipmentMethod where MethodId=" & _methodId)
                                If extraPercent > 0 Then
                                    Shipping += (Shipping * extraPercent) / 100
                                End If

                            End If
                            ShippingZero = IIf(Shipping = 0 AndAlso Not Order.IsFreeShipping, True, False)

                        Else
                            SQL = "select top 1 coalesce(case when " & Weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(Weight - 1) < 1, 0, Math.Ceiling(Weight - 1)) & " * additionalpound else case when " & Weight & " - additionalthreshold > 0 then (" & Weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(IIf(Order.IsSameAddress, Order.BillToZipcode, Order.ShipToZipcode)) & " between lowvalue and highvalue and sm.methodid = " & _methodId.ToString()
                            Shipping = DB.ExecuteScalar(SQL)
                        End If
                    End If
                    If _methodId = Utility.Common.TruckShippingId Then
                        Shipping += feeShipOversize
                    End If

                    SQL = "select coalesce(sum(rushdeliverycharge),0) from storecartitem where isrushdelivery = 1 and orderid = " & OrderId & " and carriertype = " & _methodId
                    Shipping += DB.ExecuteScalar(SQL)

                    SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and isliftgate = 1"
                    If Not AlreadyAppliedLiftGate AndAlso Not DB.ExecuteScalar(SQL) = Nothing AndAlso _methodId = Utility.Common.TruckShippingId Then
                        Shipping += SysParam.GetValue("LiftGateCharge")
                        AlreadyAppliedLiftGate = True
                    End If

                    SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and IsInsideDelivery = 1"
                    If Not AlreadyAppliedInsideDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing AndAlso _methodId = Utility.Common.TruckShippingId Then
                        Shipping += SysParam.GetValue("InsideDeliveryService")
                        AlreadyAppliedInsideDelivery = True
                    End If

                    SQL = "select top 1 cartitemid from storecartitem where isscheduledelivery = 1 and orderid = " & OrderId
                    If Not AlreadyAppliedScheduledDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing AndAlso _methodId = Utility.Common.TruckShippingId Then
                        Shipping += SysParam.GetValue("ScheduleDeliveryCharge")
                        AlreadyAppliedScheduledDelivery = True
                    End If

                    Shipping = Utility.Common.RoundCurrency(Shipping)
                    If Order.IsFreeShipping AndAlso ci.CarrierType = DefaultShippingId Then
                        ci.FreeShipping = Shipping
                        ci.Total = 0
                        ci.SubTotal = 0
                    Else
                        ci.SubTotal = Shipping
                        ci.Total = ci.SubTotal
                    End If


                    Dim Residential As Double = 0
                    If (_methodId = PickupShippingId Or Order.CarrierType = TruckShippingId) Then
                        Order.ShipmentInsured = False
                    End If

                    'Neu phi Shipping > 0
                    If Order.ShipmentInsured Then
                        Order.Insurance = StoreOrderRow.GetShippingInsurance(DB, OrderId, _methodId)
                        ci.SubTotal += Order.Insurance
                        ci.Total += Order.Insurance
                    End If

                    Dim UPSAddress As New UPS
                    SQL = "Select top 1 code from ShipmentMethod where MethodId = " & _methodId
                    MethodName = DB.ExecuteScalar(SQL)

                    If (Order.ShipToAddressType <> 1) AndAlso Utility.Common.USShippingCode.Contains(MethodName) Then
                        'Kiem tra SignatureConfirmation la bao nhieu va cong vao Type = Carrier
                        If Order.IsSignatureConfirmation And CheckWeightCartItem() > 0 Then
                            If Order.SignatureConfirmation = 0 Then
                                Order.SignatureConfirmation = ShipmentMethod.GetValue(_methodId, Common.ShipmentValue.Signature)

                            End If

                            ci.SubTotal += Order.SignatureConfirmation
                            ci.Total += Order.SignatureConfirmation
                        Else
                            Order.SignatureConfirmation = 0
                            Order.IsSignatureConfirmation = False
                        End If

                        If CheckWeightCartItem() > 0 And SubTotalPuChasePoint() < SysParam.GetValue("USTotalOrderResidential") Then
                            Residential = ShipmentMethod.GetValue(_methodId, Common.ShipmentValue.Residential)
                        End If

                        Order.ResidentialFee = Residential
                    End If

                    If ci.CartItemId = Nothing Then
                        ci.Insert()
                    Else
                        DB.ExecuteSQL("Delete from StoreCartItem where OrderId=" & ci.OrderId & " and Type='carrier' and CarrierType=" & ci.CarrierType & " and CartItemId<>" & ci.CartItemId)
                        ci.Update()
                    End If

                    ShippingTotal += ci.Total + Residential
                Next

                SQL = "delete from storecartitem where orderid = " & OrderId & " and type='carrier' and carriertype not in " & DB.NumberMultiple(Ids)
                If bFromFirstClass Then
                    SQL &= "; UPDATE StoreCartItem SET CarrierType = " & Common.DefaultShippingId & " WHERE Type = 'item' AND CarrierType = " & Common.FirstClassShippingId & " AND  OrderId = " & OrderId
                End If
                DB.ExecuteSQL(SQL)

                'Update load label shipping
                If Weight <= 0 Or Order.IsFreeShipping = True Then
                    DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 1 WHERE Type = 'item' AND CarrierType = " & DefaultShippingId & " AND OrderId = " & OrderId)
                End If
                'end
                Order.Shipping = ShippingTotal
                If Order.CarrierType <> TruckShippingId AndAlso OnlyOversizeItems() Then Order.CarrierType = TruckShippingId

            ElseIf (Order.IsSameAddress = False AndAlso Order.ShipToCountry <> "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry <> "US") Then
                Dim SQL As String
                SQL = "delete from storecartitem where orderid = " & OrderId & " and type='carrier'"
                DB.ExecuteSQL(SQL)

                Order.Shipping = 0
                RecalculateShippingInternational()
            End If
            DB.ExecuteScalar("Update StoreCartItem set CarrierType=" & Order.CarrierType & " where [type] = '" & Utility.Common.CartItemTypeBuyPoint & "' and OrderId=" & OrderId)

        End Sub
        Public Function IsAllowShowHandlingFee(ByVal itemid As Integer) As Boolean
            If Order.CarrierType = DefaultShippingId Then
                Dim shipping As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, Common.GetCurrentMemberId(), Utility.Common.MemberAddressType.Shipping.ToString())
                If shipping.Country = "US" Then
                    If Common.CheckShippingSpecialUS(shipping.Country, shipping.State) Then
                        Return False
                    End If
                Else
                    Return False
                End If

                Dim isFreeShip As Boolean = False
                Dim strFreeShippingOrderAmount As String = SysParam.GetValue("FreeShippingOrderAmount")
                If Not String.IsNullOrEmpty(strFreeShippingOrderAmount) Then
                    Dim FreeShippingOrderAmount As Double = CDbl(strFreeShippingOrderAmount)
                    Dim PriceTotal As Double = Order.SubTotal
                    If PriceTotal > FreeShippingOrderAmount Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function
        Public Function CheckOrderPriceMin(ByVal o As StoreOrderRow, ByVal messageTemplate As String) As String
            Dim result As String = String.Empty
            'Check minimun price for international order
            If o.BillToCountry = "US" And CheckShippingSpecialUS() = False Then
                Dim USOrderPriceMin As Double = SysParam.GetValue("USOrderPriceMin")
                If USOrderPriceMin <> Nothing AndAlso o.SubTotal < USOrderPriceMin Then
                    result = String.Format(messageTemplate, USOrderPriceMin, o.SubTotal)

                    Return result
                End If
            Else 'Two Puerto Rico and US Virgin Islands 
                Dim InternationalOrderPriceMin As Double = SysParam.GetValue("InternationalOrderPriceMin")
                If InternationalOrderPriceMin <> Nothing AndAlso o.SubTotal < InternationalOrderPriceMin Then
                    result = String.Format(messageTemplate, InternationalOrderPriceMin, o.SubTotal)
                    Return result
                End If
            End If
            Return String.Empty
        End Function
        Public Sub MapShippingMethod()
            Dim OrderId As Integer = Common.GetCurrentOrderId()
            Dim resultMapping As Integer = StoreOrderRow.MapShippingMethod(DB, OrderId)
            If (resultMapping > 0) Then
                Dim currentOrderCarrierType As Integer = DB.ExecuteScalar("Select CarrierType from StoreOrder where OrderId=" & Order.OrderId)
                Order.CarrierType = currentOrderCarrierType
            End If
        End Sub

        Private Sub _RecalculateProductCoupon()
            If Order Is Nothing Then
                Exit Sub
            End If

            Dim ProDiscount As Double = 0
            ''Dim dtCart As DataTable = DB.GetDataTable("Select CartItemId,ItemId,PromotionID,Quantity,SubTotal,Total,Price from StoreCartItem where OrderId=" & Order.OrderId & " and Type<>'carrier' and PromotionID>0")
            Dim dtCart As DataTable = DB.GetDataTable("Select CartItemId,PromotionID from StoreCartItem where OrderId=" & Order.OrderId & " and Type<>'carrier' and PromotionID>0")
            If Not dtCart Is Nothing Then
                If dtCart.Rows.Count Then
                    Dim dbStoPro As StorePromotionRow

                    Dim memberId As Integer = Common.GetCurrentMemberId()
                    If memberId <= 0 Then
                        memberId = 0
                    End If

                    ProductDiscount = 0
                    Dim PromotionAddID As Integer = 0
                    If Not Session("PromotionAddID") Is Nothing Then
                        PromotionAddID = Session("PromotionAddID")
                    End If
                    For Each row As DataRow In dtCart.Rows
                        ProDiscount = 0
                        Dim ci As New StoreCartItemRow
                        If IsDBNull(row("PromotionID")) Then
                            ci.PromotionID = 0
                        Else
                            ci.PromotionID = row("PromotionID")
                        End If

                        ci.CartItemId = row("CartItemId")
                        ci.DB = DB
                        dbStoPro = StorePromotionRow.GetRow(DB, ci.PromotionID)
                        If (dbStoPro.IsProductCoupon) Then

                            ci = StoreCartItemRow.GetRow(DB, ci.CartItemId)
                            FillPricing(DB, ci, False, Common.SalesPriceType.Item)


                            Dim validatePromotion As Integer = 0
                            If (ci.PromotionID = PromotionAddID) Then
                                validatePromotion = 1
                            Else
                                validatePromotion = StorePromotionRow.ValidateProductPromotion(DB, dbStoPro.PromotionCode, memberId, ci.CartItemId)
                            End If

                            If validatePromotion = 1 Then
                                If ci.LowSalePrice > 0 AndAlso ci.LowSalePrice < ci.Price Then
                                    ci.LineDiscountAmountCust = ci.Price - ci.LowSalePrice
                                    ci.CustomerPrice = ci.LowSalePrice
                                    ci.LineDiscountAmount = ci.Price - ci.LowSalePrice
                                Else
                                    ci.LineDiscountAmountCust = Nothing
                                    ci.CustomerPrice = Nothing
                                    ci.LineDiscountAmount = Nothing
                                End If
                                'Update Quantity Price
                                If Not ci.Pricing.PPU Is Nothing AndAlso ci.Pricing.PPU.Rows.Count > 1 Then
                                    Dim dv As New DataView()
                                    dv.Table = ci.Pricing.PPU.Copy
                                    dv.RowFilter = "minimumquantity <= " & ci.Quantity
                                    If dv.Count > 0 Then
                                        ci.QuantityPrice = Utility.Common.RoundCurrency(dv(dv.Count - 1)("UnitPrice")) + ci.AttributePrice
                                    Else
                                        ci.QuantityPrice = Nothing
                                    End If
                                Else
                                    ci.QuantityPrice = Nothing
                                End If

                                ci.LineDiscountAmount = ci.Price - ci.QuantityPrice
                                ''ci.SubTotal = FormatNumber(ci.Price * ci.Quantity, 2)
                                If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                                    ci.SubTotalPoint = ci.RewardPoints * ci.Quantity
                                    ci.Total = 0
                                Else
                                    ci.SubTotal = Utility.Common.RoundCurrency(ci.Price * ci.Quantity)
                                End If

                                If ci.IsFreeItem Then
                                    ci.Total = 0
                                Else
                                    If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                                        ci.Total = 0
                                    Else
                                        ci.Total = Utility.Common.RoundCurrency(ci.GetLowestPrice * ci.Quantity)
                                        If ci.Promotion IsNot Nothing AndAlso (ci.LowSalePrice > 0 And ci.LowSalePrice < ci.GetLowestPrice) Then
                                            ci.Total = Utility.Common.RoundCurrency(ci.LowSalePrice * ci.Quantity)
                                        End If

                                        If ci.PromotionIsLowestPrice() Then
                                            ci.Total = Utility.Common.RoundCurrency(ci.PromotionPrice * ci.DiscountQuantity + (ci.GetLowestPrice(False) * (ci.Quantity - ci.DiscountQuantity)))
                                        End If
                                    End If

                                End If
                                If ci.IsFreeShipping = True And Order.CarrierType = DefaultShippingId Then
                                    ci.IsFreeShipping = True
                                Else
                                    ci.IsFreeShipping = False
                                End If
                                ci.LineDiscountAmount = Utility.Common.RoundCurrency(ci.SubTotal - ci.Total)
                                '' ci.Update()
                                If Not (Utility.Common.IsPromotionFreeItem(dbStoPro.PromotionType)) Then
                                    If dbStoPro.PromotionType = "Monetary" Then
                                        ci.CouponPrice = dbStoPro.Discount
                                    ElseIf dbStoPro.PromotionType = "Percentage" Then
                                        Dim disPrice As Double
                                        Try
                                            disPrice = DB.ExecuteScalar("Select sp.UnitPrice From StoreItem si inner join SalesPrice sp on si.ItemId = sp.ItemId where si.ItemId = " & ci.ItemId & " and sp.MinimumQuantity <= " & ci.Quantity & " and [dbo].[fc_IsSalePriceValid](sp.SalesPriceId)=1 order by MinimumQuantity desc")
                                        Catch ex As Exception
                                            disPrice = 0
                                        End Try
                                        If (disPrice <= 0) Then
                                            disPrice = ci.Price
                                        End If
                                        ci.CouponPrice = disPrice * (dbStoPro.Discount / 100)
                                    End If
                                    ci.CouponPrice = Utility.Common.RoundCurrency(ci.CouponPrice)
                                    If dbStoPro.IsTotalProduct = True Then
                                        If ci.CustomerPrice = 0 And ci.Total >= ci.CouponPrice Then
                                            ci.CustomerPrice = IIf(dbStoPro.PromotionType = "Monetary", ci.Total - ci.CouponPrice, ci.Total - (ci.CouponPrice * ci.Quantity))
                                        ElseIf ci.Price < ci.CouponPrice Then
                                            ci.CustomerPrice = 0
                                        End If
                                        ci.CustomerPrice = Utility.Common.RoundCurrency(ci.CustomerPrice)
                                        If ci.Total >= dbStoPro.Discount Then

                                            If dbStoPro.PromotionType = "Monetary" Then
                                                ci.Total = ci.Total - ci.CouponPrice
                                                ProDiscount = ci.CouponPrice
                                            Else
                                                ci.Total = ci.Total - (ci.CouponPrice * ci.Quantity)
                                                ProDiscount = ci.CouponPrice * ci.Quantity
                                            End If
                                        Else

                                            If dbStoPro.PromotionType = "Percentage" Then
                                                ci.Total = ci.Total - (ci.CouponPrice * ci.Quantity)
                                                ProDiscount = ci.CouponPrice * ci.Quantity
                                            Else
                                                ProDiscount = ci.Total
                                                ci.Total = 0
                                            End If
                                        End If
                                    Else
                                        If ci.CustomerPrice = 0 And ci.Price >= ci.CouponPrice Then
                                            ci.CustomerPrice = ci.Price - ci.CouponPrice
                                        ElseIf ci.Price < ci.CouponPrice Then
                                            ci.CustomerPrice = 0
                                        End If
                                        ci.CustomerPrice = Utility.Common.RoundCurrency(ci.CustomerPrice)
                                        If ci.Price >= ci.CouponPrice Then
                                            Dim oldTotal As Double = ci.Total
                                            ci.Total = ci.Total - (ci.CouponPrice * ci.Quantity)
                                            If (ci.Total < 0) Then
                                                ci.Total = 0
                                            End If
                                            'Discount only Product Coupon
                                            ProDiscount = (ci.CouponPrice * ci.Quantity)
                                            If (ProDiscount > oldTotal) Then
                                                ProDiscount = oldTotal
                                            End If
                                        Else
                                            ProDiscount = ci.Total
                                            ci.Total = 0 'ci.Price * ci.Quantity
                                        End If
                                    End If
                                Else
                                    ci.MixMatchId = dbStoPro.MixmatchId

                                End If
                            Else
                                ci.IsPromoItem = Nothing
                                If (validatePromotion = 10 Or validatePromotion = 11) Then
                                    ci.MixMatchId = 0
                                    ci.PromotionID = 0
                                End If
                                ci.IsFreeShipping = False
                                ''ci.CouponMessage = String.Empty
                                ci.Total = ci.Total + (ci.CouponPrice * ci.Quantity)
                                ci.CouponPrice = 0
                            End If
                            ProductDiscount += ProDiscount
                            ci.LineDiscountAmount = ci.SubTotal - ci.Total
                            ci.Update()
                        Else
                            ''                            ci.MixMatchId = 0
                            DB.ExecuteSQL("Update StoreCartItem set MixMatchId=0 where CartItemId=" & ci.CartItemId)
                        End If
                    Next
                End If
            End If

            Order.TotalProductDiscount = ProductDiscount
            'Update RawPriceDiscountAmount
            DB.ExecuteSQL("UPDATE StoreOrder SET RawPriceDiscountAmount = (SELECT COALESCE(SUM(CASE WHEN RawPrice > 0 THEN rawprice ELSE price end * quantity) - sum(price * quantity),0) FROM StoreCartItem WHERE OrderId = " & OrderId & ") where OrderId = " & OrderId)
        End Sub
        Public Sub RefreshCartItemInforFromItem(ByVal lstCart As StoreCartItemCollection, ByVal DB As Database)
            If lstCart Is Nothing Then
                Exit Sub
            End If

            Dim IsMergeCart As Boolean = IIf(System.Web.HttpContext.Current.Request.RawUrl.Contains("login.aspx"), True, False)
            For Each ci As StoreCartItemRow In lstCart
                If (ci.IsFreeGift Or ci.IsFreeSample) Then 'Free gift or Free Sample
                    Continue For
                End If

                Dim si As StoreItemRow = StoreItemRow.GetRowFromCart(DB, ci.ItemId, Utility.Common.GetCurrentMemberId(), ci.AddType, ci.Quantity)
                If Not IsMergeCart Then
                    If Not ci.MixMatchId > 0 AndAlso ci.LineDiscountAmount = 0 AndAlso ((ci.ModifiedDate > DateTime.MinValue AndAlso (ci.ModifiedDate > si.ModifyDate And ci.ModifiedDate > si.LastImport)) OrElse (ci.CreatedDate > si.ModifyDate And ci.CreatedDate > si.LastImport)) OrElse (ci.LineDiscountAmount > 0 AndAlso ci.CustomerPrice = si.LowSalePrice) Then
                        Continue For
                    End If
                End If

                If ci.AddType = 2 Then
                    ci.Weight = CDbl(si.Weight * si.CaseQty) + ((CDbl(si.Weight * si.CaseQty) * 10) / 100)
                Else
                    ci.Weight = si.Weight
                End If

                ci.PriceDesc = si.PriceDesc
                ci.IsTaxFree = si.IsTaxFree
                ci.IsOversize = si.IsOversize
                ci.IsFreeShipping = si.IsFreeShipping
                ci.IsHazMat = si.IsHazMat
                ci.IsFlammable = si.IsFlammable
                ci.ItemName = si.ItemName
                If (si.IsRewardPoints AndAlso si.RewardPoints > 0) And ci.IsRewardPoints Then
                    ci.RewardPoints = si.RewardPoints
                ElseIf Not ci.IsFreeItem Then
                End If

                FillPricing(DB, ci, False, Common.SalesPriceType.Item)
                'Dim mmValue As Double = 100
                'If ci.IsFreeItem Then
                '    mmValue = DB.ExecuteScalar("Select Value from MixmatchLine where MixMatchId=" & ci.MixMatchId & " and Itemid=" & ci.ItemId & " and Value>0")
                '    If mmValue < 100 AndAlso mmValue > 0 Then
                '        If ci.LowSalePrice > 0 Then
                '            ci.Price = ci.LowSalePrice
                '            ci.LowSalePrice = ci.LowSalePrice - (mmValue * ci.LowSalePrice) / 100
                '        Else
                '            ci.Price = ci.Price - (mmValue * ci.Price) / 100
                '        End If
                '    End If
                'End If

                'If ci.LowSalePrice > 0 AndAlso ci.LowSalePrice < ci.Price Then
                '    ci.LineDiscountAmountCust = ci.Price - ci.LowSalePrice
                '    ci.CustomerPrice = ci.LowSalePrice
                '    ci.LineDiscountAmount = ci.Price - ci.LowSalePrice
                'Else
                '    ci.LineDiscountAmountCust = Nothing
                '    ci.CustomerPrice = Nothing
                '    ci.LineDiscountAmount = Nothing
                'End If

                'Update Quantity Price
                If Not ci.Pricing.PPU Is Nothing AndAlso ci.Pricing.PPU.Rows.Count > 1 Then
                    Dim dv As New DataView()
                    dv.Table = ci.Pricing.PPU.Copy
                    dv.RowFilter = "minimumquantity <= " & ci.Quantity
                    If dv.Count > 0 Then
                        ci.QuantityPrice = Utility.Common.RoundCurrency(dv(dv.Count - 1)("UnitPrice")) + ci.AttributePrice
                    Else
                        ci.QuantityPrice = Nothing
                    End If
                Else
                    ci.QuantityPrice = Nothing
                End If

                If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                    ci.SubTotalPoint = ci.RewardPoints * ci.Quantity
                    ci.Total = 0
                    ci.MixMatchId = 0
                    ci.Price = Nothing
                    ci.SalePrice = Nothing
                    ci.SubTotalPoint = ci.Quantity * ci.RewardPoints
                    ci.SubTotal = 0
                    ci.LineDiscountAmountCust = Nothing
                    ci.CustomerPrice = Nothing
                    ci.LineDiscountAmount = Nothing
                    ci.QuantityPrice = Nothing

                Else
                    If ci.AddType = 2 Then
                        ci.Price = si.CasePrice
                    Else
                        ci.Price = si.Price
                    End If

                    If ci.IsFreeItem = True Then
                        If MixMatchRow.IsDiscountPercent(ci.MixMatchId) > 0 Then
                            Exit For
                        Else
                            ci.LineDiscountAmountCust = 0
                            ci.CustomerPrice = 0
                            ci.Total = 0
                            ci.SubTotal = Utility.Common.RoundCurrency(ci.Price * ci.Quantity)
                            ci.LineDiscountAmount = Utility.Common.RoundCurrency(ci.Price * ci.Quantity)
                        End If
                    Else

                        If si.LowSalePrice > 0 AndAlso si.LowSalePrice < si.Price Then
                            ci.LineDiscountAmountCust = si.Price - si.LowSalePrice
                            ci.CustomerPrice = si.LowSalePrice
                            ci.LineDiscountAmount = Utility.Common.RoundCurrency((si.Price - si.LowSalePrice) * ci.Quantity)
                        Else
                            ci.LineDiscountAmountCust = Nothing
                            ci.CustomerPrice = Nothing
                            ci.LineDiscountAmount = Nothing
                            ci.QuantityPrice = IIf(ci.AddType = 2, si.CasePrice, si.Price)
                        End If

                        ci.SubTotal = Utility.Common.RoundCurrency(ci.Price * ci.Quantity)
                        ci.Total = Utility.Common.RoundCurrency(ci.GetLowestPrice * ci.Quantity)
                        If ci.Promotion IsNot Nothing AndAlso (ci.LowSalePrice > 0 And ci.LowSalePrice < ci.GetLowestPrice) Then
                            ci.Total = Utility.Common.RoundCurrency(ci.LowSalePrice * ci.Quantity)
                        End If

                        If ci.PromotionIsLowestPrice() Then
                            ci.Total = Utility.Common.RoundCurrency(ci.PromotionPrice * ci.DiscountQuantity + (ci.GetLowestPrice(False) * (ci.Quantity - ci.DiscountQuantity)))
                        End If
                    End If

                    ci.IsRewardPoints = False
                    ci.SubTotalPoint = Nothing
                    ci.RewardPoints = Nothing
                    ci.IsFreeShipping = ci.IsFreeShipping = True And Order.CarrierType = DefaultShippingId
                    If (ci.CouponPrice > 0) Then
                        ci.Total = ci.Total - ci.CouponPrice * ci.Quantity
                    End If
                End If

                ci.Update()
            Next
        End Sub

        Public Sub RecalculateCartItemLogin()
            RefreshCartItemInforFromItem(GetCartItems(), DB)
        End Sub

        Public Sub RecalculateCartItem(ByVal ci As StoreCartItemRow, ByVal isUpdate As Boolean)

            Dim mmValue As Double = 100
            Dim tmp As String = StoreItemRow.GetCustomerDiscountWithQuantity(ci.ItemId, Common.GetCurrentMemberId(), ci.Quantity, ci.AddType)
            If IsNumeric(tmp) Then ci.LowSalePrice = Utility.Common.RoundCurrency(tmp)

            ci.LowPrice = ci.Price
            ci.HighPrice = ci.Price

            FillPricing(DB, ci, False, Common.SalesPriceType.Item)

            If ci.IsFreeItem Then
                mmValue = DB.ExecuteScalar("Select Value from MixmatchLine where MixMatchId=" & ci.MixMatchId & " and Itemid=" & ci.ItemId & " and Value>0")
                If mmValue < 100 AndAlso mmValue > 0 Then
                    If ci.LowSalePrice > 0 Then
                        ci.Price = ci.LowSalePrice
                        '' ci.LowSalePrice = ci.LowSalePrice - (mmValue * ci.LowSalePrice) / 100
                        ''ci.CustomerPrice = ci.Price - (mmValue * ci.Price) / 100

                    End If
                    ci.CustomerPrice = CDbl(Utility.Common.ViewCurrency(ci.Price - (mmValue * ci.Price) / 100))
                End If
            Else
                If ci.LowSalePrice > 0 AndAlso ci.LowSalePrice < ci.Price Then
                    ci.LineDiscountAmountCust = ci.Price - ci.LowSalePrice
                    ci.CustomerPrice = ci.LowSalePrice
                    ci.LineDiscountAmount = ci.Price - ci.LowSalePrice
                Else
                    ci.LineDiscountAmountCust = Nothing
                    ci.CustomerPrice = Nothing
                    ci.LineDiscountAmount = Nothing
                End If
            End If



            'Update Quantity Price
            If Not ci.Pricing.PPU Is Nothing AndAlso ci.Pricing.PPU.Rows.Count > 1 Then
                Dim dv As New DataView()
                dv.Table = ci.Pricing.PPU.Copy
                dv.RowFilter = "minimumquantity <= " & ci.Quantity
                If dv.Count > 0 Then
                    ci.QuantityPrice = Utility.Common.RoundCurrency(dv(dv.Count - 1)("UnitPrice")) + ci.AttributePrice
                Else
                    ci.QuantityPrice = Nothing
                End If
            Else
                ci.QuantityPrice = Nothing
            End If

            ci.LineDiscountAmount = ci.Price - ci.QuantityPrice
            ''ci.LineDiscountAmount = Utility.Common.RoundCurrency(ci.LineDiscountAmount)
            ''ci.SubTotal = FormatNumber(ci.Price * ci.Quantity, 2)
            If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                ci.SubTotalPoint = ci.RewardPoints * ci.Quantity
                ci.Total = 0
            Else
                ci.SubTotal = Utility.Common.RoundCurrency(ci.Price * ci.Quantity)
            End If



            If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                ci.Total = 0
            ElseIf ci.IsFreeItem AndAlso mmValue = 100 Then
                ci.Total = 0
                ''ci.SubTotal = 0
            Else
                ci.Total = Utility.Common.RoundCurrency(ci.GetLowestPrice * ci.Quantity)
                If ci.Promotion IsNot Nothing AndAlso (ci.LowSalePrice > 0 And ci.LowSalePrice < ci.GetLowestPrice) Then
                    ci.Total = Utility.Common.RoundCurrency(ci.LowSalePrice * ci.Quantity)
                End If

                If ci.PromotionIsLowestPrice() Then
                    ci.Total = Utility.Common.RoundCurrency(ci.PromotionPrice * ci.DiscountQuantity + (ci.GetLowestPrice(False) * (ci.Quantity - ci.DiscountQuantity)))
                End If
            End If

            If ci.IsFreeShipping = True And Order.CarrierType = DefaultShippingId Then
                ci.IsFreeShipping = True
            Else
                ci.IsFreeShipping = False
            End If
            ci.LineDiscountAmount = Utility.Common.RoundCurrency(ci.SubTotal - ci.Total)
            If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                ci.LineDiscountAmount = Nothing
                ci.LineDiscountAmountCust = Nothing
                ci.CustomerPrice = Nothing
                ci.Total = Nothing
                ci.SubTotal = Nothing
                ci.PromotionPrice = Nothing
                ci.QuantityPrice = Nothing
                ci.DiscountQuantity = Nothing
                ci.LineDiscountPercent = Nothing
            End If
            If isUpdate Then
                ci.Update()
            End If
        End Sub

        Public Sub ResetDefaultData()
            If Order IsNot Nothing Then
                Dim SQL As String = String.Empty
                If Order.CarrierType = Common.TruckShippingId() AndAlso Not HasOversizeItems() Then
                    If Order.ShipToCountry <> "US" Or CheckShippingSpecialUS() Then
                        SQL = ";UPDATE StoreOrder SET CarrierType = " & Common.USPSPriorityShippingId() & " WHERE OrderId = " & Order.OrderId & ";UPDATE StoreCartItem SET CarrierType = " & Common.USPSPriorityShippingId() & " WHERE OrderId = " & Order.OrderId
                    Else
                        SQL = ";UPDATE StoreOrder SET CarrierType = " & Common.DefaultShippingId() & " WHERE OrderId = " & Order.OrderId & ";UPDATE StoreCartItem SET CarrierType = " & Common.DefaultShippingId() & " WHERE OrderId = " & Order.OrderId
                    End If

                End If

                SQL = "UPDATE StoreCartItem SET IsFreeShipping = (CASE WHEN StoreCartItem.CarrierType = " & Common.DefaultShippingId() & " THEN (SELECT ISNULL(IsFreeShipping,0) FROM StoreItem WHERE StoreItem.ItemId = StoreCartItem.ItemId) ELSE 0 END) WHERE OrderId=" & Order.OrderId & SQL
                DB.ExecuteSQL(SQL)
            End If
        End Sub

        'Public Sub RecalculateOrderDetail()
        '    Dim Sql As String = ""
        '    MapShippingMethod()
        '    ResetDefaultData()
        '    _RecalculateProductCoupon()

        '    RecalculateOrderTotal(Order, ProductDiscount)
        '    _ProcessWebPromotion("")
        '    RecalculateOrderBaseSubTotal(Order, ProductDiscount)
        '    _CheckSaleTax()

        '    _RecalculateShipping("")

        '    LoadLevelMember(Order)

        '    ''Order.Total = Order.SubTotal + Order.Shipping + Order.Tax - Order.Discount
        '    '' RecalculateOrderSpecialHandlingFee(Order)
        '    Order.TotalSpecialHandlingFee = DB.ExecuteScalar("select coalesce(SUM(SpecialHandlingFee*Quantity),0) from StoreCartItem where OrderId=" & Order.OrderId & "  and Type<>'carrier'")
        '    Order.Total = Order.SubTotal + Order.Shipping + Order.Tax + Order.TotalSpecialHandlingFee
        '    Order.Update()
        '    Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & Order.OrderId)
        'End Sub
        Public Sub AdminRecalculateOrderDetail()
            Dim Sql As String = ""
            MapShippingMethod()
            ResetDefaultData()
            _AdminRecalculateCartItem()

            _CheckSaleTax()

            _AdminRecalculateShipping("")

            LoadLevelMember(Order)

            Order.Total = Order.SubTotal + Order.Shipping + Order.Tax
            Order.Update()
            Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & Order.OrderId)
        End Sub
        Private Sub _AdminRecalculateShipping(ByVal FromFunction As String)

            If (Order.IsSameAddress = False AndAlso Order.ShipToZipcode = Nothing) OrElse (Order.IsSameAddress = True AndAlso Order.BillToZipcode = Nothing AndAlso Order.BillToCountry = "US") Then
                Exit Sub
            End If

            If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = Nothing) OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = Nothing) Then
                Exit Sub
            End If

            If CheckShippingSpecialUS() Then
                If (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") Or (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") OrElse (Order.BillToCountry = "VI" Or Order.BillToCountry = "PR") Then
                    If HasOversizeItems() = False Then
                        ' DB.ExecuteSQL("DELETE FROM StoreCartItem WHERE OrderId = " & OrderId & " AND [Type] = 'carrier'")
                    End If

                    Order.Shipping = 0
                    RecalculateShippingInternational()
                End If
            ElseIf (Order.IsSameAddress = False AndAlso Order.ShipToCountry = "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry = "US") Then
                Dim dv As DataView, drv As DataRowView
                Dim additional As Double = 0
                Dim ci As StoreCartItemRow
                Dim Weight As Double = Nothing
                Dim WeightOV As Double = 0
                Dim MethodId As Integer = Nothing
                Dim MethodName As String = String.Empty
                Dim Ids As String = String.Empty
                Dim SQL As String
                Dim AlreadyAppliedLiftGate As Boolean = False
                Dim AlreadyAppliedScheduledDelivery As Boolean = False
                Dim AlreadyAppliedInsideDelivery As Boolean = False
                Dim AppliedHazMatFee As Boolean = False
                Dim feeShipOversize As Double = 0

                'SQL = "select coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = storecartitem.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype from storecartitem where orderid = " & Order.OrderId & " and type = 'item' and isfreeitem = 0 group by carriertype"
                ''Include weight item free
                'Order CarrierType = 0 When the first insert to DB
                If Order.CarrierType = 1 Then
                    'Email.SendError("ToError500", "ShoppingCart UPS 3-Day Service", "OrderId: " & Order.OrderId & "<br>Session List:<br>" & GetSessionList())
                    Order.CarrierType = Utility.Common.USPSPriorityShippingId
                    Order.Update()
                    If IsDBNull(Order.CarrierType) = False And Order.CarrierType <> "" Then
                        SQL = "update storecartitem set carriertype=" & Order.CarrierType & " where carriertype=" & Utility.Common.USPSPriorityShippingId & " and orderid = " & OrderId & " --" & FromFunction
                        DB.ExecuteSQL(SQL)
                    End If
                End If
                SQL = "select coalesce(sum(weight * quantity),0) as weight, (select coalesce(sum(weight * quantity),0) as weight from storecartitem sci where isfreeshipping = 1 and orderid = " & Order.OrderId & " and sci.carriertype = storecartitem.carriertype and type = 'item' and isfreeitem = 0) as freeweight, carriertype, IsOverSize from storecartitem where orderid = " & Order.OrderId & " and  type = 'item' group by carriertype, IsOverSize"
                dv = DB.GetDataView(SQL)
                'If dv.Count > 0 Then
                'If Order.CarrierType = 1 Then
                '    'Email.SendError("ToError500", "ShoppingCart UPS 3-Day Service", "OrderId: " & Order.OrderId & "<br>Session List:<br>" & GetSessionList())
                '    Order.CarrierType = Utility.Common.USPSPriorityShippingId
                'End If

                'If IsDBNull(Order.CarrierType) = False And Order.CarrierType <> "" Then
                '    SQL = "update storecartitem set carriertype=" & Order.CarrierType & " where carriertype=" & Utility.Common.USPSPriorityShippingId & " and orderid = " & OrderId & " --" & FromFunction
                '    DB.ExecuteSQL(SQL)
                'End If
                'End If

                Dim ShippingTotal As Double = 0
                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)

                    MethodId = drv("carriertype")
                    If MethodId = Utility.Common.TruckShippingId Then
                        WeightOV = WeightFlatFee()
                        Weight = drv("weight") - drv("freeweight") - WeightOV
                        feeShipOversize = ShippingOversize(Order)
                    Else
                        Weight = drv("weight") - drv("freeweight")
                    End If

                    If Order.IsFreeShipping = True And drv("IsOverSize") = False Then
                        Weight = 0
                    Else
                        CalculateWeightShipping(Weight, MethodId)
                    End If

                    If Weight <= 0 And Order.IsFreeShipping = False Then
                        Order.IsFreeShipping = True
                        Order.Update()
                    End If
                    Weight = Math.Round(Weight, 3)
                    If Ids <> String.Empty Then Ids &= ","
                    Ids &= MethodId

                    Dim Shipping As Double = 0
                    ci = StoreCartItemRow.GetRow(DB, OrderId, MethodId)
                    If Weight > 0 Then
                        SQL = "select top 1 coalesce(case when " & Weight & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(Weight - 1) < 1, 0, Math.Ceiling(Weight - 1)) & " * additionalpound else case when " & Weight & " - additionalthreshold > 0 then (" & Weight & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(IIf(Order.IsSameAddress, Order.BillToZipcode, Order.ShipToZipcode)) & " between lowvalue and highvalue and sm.methodid = " & drv("carriertype")
                        Shipping = DB.ExecuteScalar(SQL)
                    End If

                    If MethodId = Utility.Common.TruckShippingId Then
                        Shipping += feeShipOversize
                    End If

                    SQL = "select coalesce(sum(rushdeliverycharge),0) from storecartitem where isrushdelivery = 1 and orderid = " & OrderId & " and carriertype = " & MethodId
                    Shipping += DB.ExecuteScalar(SQL)

                    SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and isliftgate = 1"
                    If Not AlreadyAppliedLiftGate AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                        Shipping += SysParam.GetValue("LiftGateCharge")
                        AlreadyAppliedLiftGate = True
                    End If

                    SQL = "select top 1 cartitemid from storecartitem where orderid = " & OrderId & " and IsInsideDelivery = 1"
                    If Not AlreadyAppliedInsideDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                        Shipping += SysParam.GetValue("InsideDeliveryService")
                        AlreadyAppliedInsideDelivery = True
                    End If

                    SQL = "select top 1 cartitemid from storecartitem where isscheduledelivery = 1 and orderid = " & OrderId
                    If Not AlreadyAppliedScheduledDelivery AndAlso Not DB.ExecuteScalar(SQL) = Nothing Then
                        Shipping += SysParam.GetValue("ScheduleDeliveryCharge")
                        AlreadyAppliedScheduledDelivery = True
                    End If



                    ci.SubTotal = Shipping
                    ci.Total = IIf(Order.IsFreeShipping AndAlso ci.CarrierType = DefaultShippingId, 0, Shipping)

                    If Order.ShipmentInsured Then
                        Dim Insurance As Double = StoreOrderRow.GetShippingInsurance(DB, OrderId, MethodId)
                        ci.SubTotal += Insurance
                        ci.Total += Insurance
                    End If

                    Dim UPSAddress As New UPS
                    SQL = "Select top 1 code from ShipmentMethod where MethodId = " & MethodId
                    MethodName = DB.ExecuteScalar(SQL)

                    Dim Residential As Double = 0
                    If (Order.ShipToAddressType <> 1) AndAlso Utility.Common.USShippingCode.Contains(MethodName) Then ''Left(LCase(MethodName), 3) = "ups" Then

                        'Kiem tra SignatureConfirmation la bao nhieu va cong vao Type = Carrier
                        If Order.IsSignatureConfirmation And CheckWeightCartItem() > 0 Then
                            If Order.SignatureConfirmation = 0 Then
                                SQL = "Select top 1 coalesce(Signature,0) from ShipmentMethod where MethodId = " & MethodId
                                Dim Signature As Double = DB.ExecuteScalar(SQL)
                                Order.SignatureConfirmation = Signature
                                Dim strPage As String = Context.Request.Url.ToString()
                                Email.SendError("ToError500", "_RecalculateShipping = Order.SignatureConfirmation = 0", "Page: " & strPage & "<br><br>" & "OrderId = " & Order.OrderId & GetSessionList())
                            End If

                            ci.SubTotal += Order.SignatureConfirmation
                            ci.Total += Order.SignatureConfirmation
                        Else
                            Order.SignatureConfirmation = 0
                            Order.IsSignatureConfirmation = False
                        End If

                        If CheckWeightCartItem() > 0 And SubTotalPuChasePoint() < SysParam.GetValue("USTotalOrderResidential") Then
                            SQL = "Select top 1 coalesce(Residential,0) from shipmentmethod where methodid = " & MethodId
                            Residential = DB.ExecuteScalar(SQL)
                        End If

                        Order.ResidentialFee = Residential
                    End If

                    If ci.CartItemId = Nothing Then ci.Insert() Else ci.Update()
                    ShippingTotal += ci.Total + Residential
                Next

                If Weight <= 0 Or Order.IsFreeShipping = True Then
                    DB.ExecuteSQL("UPDATE StoreCartItem SET IsFreeShipping = 1 WHERE Type = 'item' AND CarrierType = " & DefaultShippingId & " AND OrderId = " & OrderId)
                End If
                Order.Shipping = ShippingTotal
                If Order.CarrierType <> TruckShippingId AndAlso OnlyOversizeItems() Then Order.CarrierType = TruckShippingId

            ElseIf (Order.IsSameAddress = False AndAlso Order.ShipToCountry <> "US") OrElse (Order.IsSameAddress = True AndAlso Order.BillToCountry <> "US") Then
                Order.Shipping = 0
            End If
        End Sub
        Public Sub RecalculateOrderDetail(ByVal FromFunction As String)
            If Not (FromFunction = Utility.Common.SystemFunction.ChangeOrderAddress.ToString()) Then
                MapShippingMethod()
            End If

            ResetDefaultData()
            If Not (FromFunction = Utility.Common.SystemFunction.ChangeOrderAddress.ToString()) Then
                _RecalculateProductCoupon()
            End If

            RecalculateOrderTotal(Order, ProductDiscount)
            _ProcessWebPromotion(True) 'Tat ham Order.Update tai function nay
            'RecalculateOrderBaseSubTotal(Order, ProductDiscount)
            _CheckSaleTax()
            _RecalculateShipping(FromFunction)
            LoadLevelMember(Order)

            'Check flammable
            If Order.IsFlammableCartItem = Common.FlammableCart.Init Then
                Order.IsFlammableCartItem = HasFlammableCartItem()
            End If

            If (Order.IsFlammableCartItem = Common.FlammableCart.HazMat Or Order.IsFlammableCartItem = Common.FlammableCart.BlockedHazMat) AndAlso IsHazardousMaterialFee(Order.CarrierType) Then
                Order.HazardousMaterialFee = ShipmentMethod.GetValue(Order.CarrierType, Utility.Common.ShipmentValue.HazMatFee)
            Else
                Order.HazardousMaterialFee = 0
            End If

            Order.TotalSpecialHandlingFee = DB.ExecuteScalar("SELECT COALESCE(SUM(dbo.[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId,ItemId,AddType)*Quantity),0) from StoreCartItem WHERE OrderId=" & Order.OrderId & " AND [Type] <> 'carrier' AND IsOversize = 0 AND CarrierType=" & Utility.Common.DefaultShippingId & " AND Weight >= 1")
            Order.Total = Order.SubTotal + Order.Shipping + Order.Tax + Order.TotalSpecialHandlingFee + Order.HazardousMaterialFee
            Order.Update()

            Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & Order.OrderId)
        End Sub

        Public Sub RecalculateOrderUpdate()
            Dim Sql As String = ""

            _RecalculateProductCoupon()

            RecalculateOrderTotal(Order, ProductDiscount)
            _ProcessWebPromotion(False)
            RecalculateOrderBaseSubTotal(Order, ProductDiscount)
            LoadLevelMember(Order)
            Order.Total = Order.SubTotal + Order.Shipping + Order.Tax + Order.TotalSpecialHandlingFee + Order.HazardousMaterialFee
            Order.Update()
        End Sub

#End Region
        Public Shared Function GetSessionList() As String
            'Write list session
            Dim strSession As String = String.Empty
            Dim strName As String = String.Empty
            Dim errorInfo As String = String.Empty
            Dim iLoop As Integer

            Try
                For Each strName In System.Web.HttpContext.Current.Session.Contents
                    If IsArray(System.Web.HttpContext.Current.Session(strName)) Then
                        For iLoop = LBound(System.Web.HttpContext.Current.Session(strName)) To UBound(System.Web.HttpContext.Current.Session(strName))
                            strSession &= strName & "(" & iLoop & ") - " & System.Web.HttpContext.Current.Session(strName)(iLoop) & "<BR>"
                        Next
                    Else
                        If Not strName.Contains("cartRender") And Not strName.Contains("FacetedSearchRatingLevel") And Not strName.Contains("querySearch") Then
                            strSession &= strName & " - " & System.Web.HttpContext.Current.Session.Contents(strName) & "<BR>"
                        End If

                    End If
                Next
            Catch ex As Exception
                strSession &= "<br><br>Exception: " & ex.ToString()
            End Try

            Try
                If Not String.IsNullOrEmpty(strSession) Then
                    Dim ip As String = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString()
                    errorInfo &= "<br><br>Session List<br>-------------------------<br>" & strSession & "<br>Date: " & DateTime.Now.ToString("d/M/yyyy HH:mm:ss") & "<br>IP: " & ip
                End If
            Catch ex As Exception
                errorInfo = "<br><br>Session List<br>-------------------------<br>" & strSession & "<br><br>Exception 2: " & ex.ToString()
            End Try

            Return errorInfo
        End Function
        Public Shared Sub ResetAllCartData(ByVal DB As Database, ByVal objCart As BaseShoppingCart)
            StoreOrderRow.ResetCartItemHandlingFee(DB, objCart.Order.OrderId)
            objCart.RecalculateCartItemLogin()
            Utility.CacheUtils.RemoveCache(StoreCartItemRow.cachePrefixKey & "ItemCount_" & objCart.Order.OrderId)
            Utility.Common.DeleteCachePopupCart(objCart.Order.OrderId)
        End Sub

        Public Shared Sub ResetAllCartDataLogin(ByVal DB As Database, ByVal objCart As BaseShoppingCart)
            Dim sqlHandling As String = String.Empty '"Update StoreCartItem set SpecialHandlingFee=[dbo].[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId) where OrderId=" & objCart.Order.OrderId & " and Type='item' and  CarrierType=" & Utility.Common.DefaultShippingId
            sqlHandling = sqlHandling & "Update StoreCartItem set SpecialHandlingFee=0 where OrderId=" & objCart.Order.OrderId & " and Type='item' and  CarrierType<>" & Utility.Common.DefaultShippingId & "; Delete from StoreCartItem where IsFreeItem=1 and MixmatchId>0 and OrderId=" & objCart.Order.OrderId
            DB.ExecuteSQL(sqlHandling)
            objCart.RecalculateCartItemLogin()
            objCart.ResetCartMixMatchLogin(DB, objCart.Order.OrderId, objCart.MemberId)
            objCart.ResetMixmatchProductCouponLogin(DB, objCart.Order.OrderId, objCart.MemberId)
            objCart.RecalculateOrderDetail("ResetAllCartDataLogin")
            Utility.CacheUtils.RemoveCache(StoreCartItemRow.cachePrefixKey & "ItemCount_" & objCart.Order.OrderId)
            Utility.Common.DeleteCachePopupCart(objCart.Order.OrderId)
        End Sub

        Public Shared Sub DeleteMixMatchFreeItemIsNotValid(ByVal DB As Database, ByVal orderId As Integer)
            Dim sql As String = "select CartItemId,ItemId,SKU,COALESCE(MixMatchId,0) as MixMatchId,COALESCE(PromotionID,0) as PromotionID from StoreCartItem where IsFreeItem=1 and OrderId=" & orderId & "  and FreeItemIds is not null and FreeItemIds<>''"
            Dim dtFreeItemInValid As DataTable = DB.GetDataTable(sql)
            If Not (dtFreeItemInValid Is Nothing) Then
                If (dtFreeItemInValid.Rows.Count > 0) Then
                    Dim cartItemId As Integer = 0
                    Dim promotionId As Integer = 0
                    Dim mixmatchId As Integer = 0
                    Dim sqlDelete As String = String.Empty
                    For Each row As DataRow In dtFreeItemInValid.Rows
                        cartItemId = row("CartItemId")
                        mixmatchId = row("MixMatchId")
                        promotionId = row("PromotionID")
                        If (promotionId < 1 AndAlso mixmatchId < 1) Then ''invalid, delete
                            If (String.IsNullOrEmpty(sqlDelete)) Then
                                sqlDelete = cartItemId
                            Else
                                sqlDelete = sqlDelete & "," & cartItemId
                            End If
                        End If
                    Next
                    If Not String.IsNullOrEmpty(sqlDelete) Then
                        sqlDelete = "Delete from StoreCartItem where OrderId=" & orderId & " and CartItemId in(" & sqlDelete & ")"
                        DB.ExecuteSQL(sqlDelete)
                    End If
                End If
            End If
        End Sub

        Public Shared Function GetItemPrice(ByVal DB As Database, ByVal si As StoreItemRow, ByVal Quantity As Integer, ByVal LineDiscountAmount As Double, ByVal memberId As Integer, ByRef isItemMulti As Boolean) As ItemPrice
            ''Return GetItemPrice1(DB, si, Quantity, LineDiscountAmount, memberId)
            Dim objItemPrice As New ItemPrice
            If si Is Nothing Then
                Return Nothing
            End If
            If si.Pricing Is Nothing Then
                FillPricing(DB, si, isItemMulti, Common.SalesPriceType.Item)
            End If
            If si.Pricing Is Nothing Then
                Return Nothing
            End If
            Dim dt As DataTable = si.Pricing.PPU
            If Not dt Is Nothing Then
                If dt.Rows.Count > 1 Then

                    objItemPrice.MultiPriceColection = New List(Of ItemMultiPrice)
                    objItemPrice.MultiPriceHTML = "<table cellspacing=""0"" align=""center"" cellpadding=""0"" border=""0"" class=""multi"">" & vbCrLf &
                         "<caption class='title'>Buy More & Save</caption><colgroup><col width=""30%""></col><col width=""50%""></col></colgroup" & vbCrLf
                    objItemPrice.MultiPriceHTML &= "<tbody><tr><th class='head'>Qty</th><th class='head' >Price</th></tr></tbody>" & vbCrLf
                    Dim savePercent As Double = 0
                    Dim unitPrice As Double = 0
                    Dim originalPrice As Double = 0
                    Dim savePrice As Double = 0
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim objItemMultiPrice As New ItemMultiPrice
                        unitPrice = Utility.Common.RoundCurrency(dt.Rows(i)("unitprice"))
                        objItemPrice.MultiPriceHTML &= "<tbody>"
                        If (i > 0) Then
                            savePercent = ((originalPrice - unitPrice) / originalPrice) * 100
                            savePercent = Utility.Common.RoundCurrency(savePercent)
                            objItemPrice.MultiPriceHTML &= "<tr class='line' id='trPrice" & i & "' >" & vbCrLf
                        Else

                            originalPrice = unitPrice
                            objItemPrice.MultiPriceHTML &= "<tr class='line select' id='trPrice" & i & "'>" & vbCrLf
                        End If

                        If (objItemPrice.MinMultiPrice > 0 And objItemPrice.MinMultiPrice > unitPrice) Or objItemPrice.MinMultiPrice = 0 Then
                            objItemPrice.MinMultiPrice = unitPrice
                        End If


                        '' objItemPrice.MultiPriceColection(i).Price = unitPrice

                        Dim qty As Integer = dt.Rows(i)("minimumquantity")
                        objItemMultiPrice.MinQty = qty
                        objItemPrice.MultiPriceHTML &= "<td class=""multiprice-qty value"">"
                        If i = dt.Rows.Count - 1 Then
                            objItemPrice.MultiPriceHTML &= qty & "+"
                            objItemMultiPrice.MaxQty = Integer.MaxValue
                        Else
                            objItemPrice.MultiPriceHTML &= qty & "-" & dt.Rows(i + 1)("minimumquantity") - 1
                            objItemMultiPrice.MaxQty = dt.Rows(i + 1)("minimumquantity") - 1
                        End If
                        objItemMultiPrice.Price = unitPrice
                        objItemPrice.MultiPriceColection.Add(objItemMultiPrice)
                        objItemPrice.MultiPriceHTML &= "</td>"
                        '' objItemPrice.MultiPriceHTML &= "<td  class=""save value"">" & IIf(savePercent > 0, "-" & savePercent, "0") & "%</td>"

                        objItemPrice.MultiPriceHTML &= "<td  class=""price value""><span itemprop=""price"">" & FormatCurrency(dt.Rows(i)("unitprice")) & "</span>"
                        If (savePercent > 0) Then
                            objItemPrice.MultiPriceHTML &= "<span class='save'>(Save " & savePercent & "%)</span>"
                        End If
                        objItemPrice.MultiPriceHTML &= "</td>"
                        objItemPrice.MultiPriceHTML &= "</tr></tbody>" & vbCrLf
                    Next
                    objItemPrice.MultiPriceHTML &= "<meta itemprop=""priceCurrency"" content=""USD"" /></table>"
                    Return objItemPrice
                End If
            End If
            If (si.LowPrice <> si.LowSalePrice AndAlso si.LowSalePrice <> Nothing) OrElse LineDiscountAmount > 0 Then
                objItemPrice.RegularPrice = si.Pricing.BasePrice * Quantity
                objItemPrice.SalePrice = si.Pricing.SellPrice * Quantity
                objItemPrice.YouSave = objItemPrice.RegularPrice - objItemPrice.SalePrice
                objItemPrice.PercentSave = 1 - (objItemPrice.SalePrice / objItemPrice.RegularPrice)
                objItemPrice.PercentSave = Utility.Common.RoundCurrency(objItemPrice.PercentSave * 100)
                Return objItemPrice
            Else
                objItemPrice.NormalPrice = (si.LowPrice - LineDiscountAmount) * Quantity
            End If
            Return objItemPrice
        End Function

        Public Shared Function GetBuyInBulkItemPrice(ByVal DB As Database, ByVal si As StoreItemRow, ByVal memberId As Integer, ByRef dtPrice As DataTable) As String
            ''Return GetItemPrice1(DB, si, Quantity, LineDiscountAmount, memberId)
            Dim objItemPrice As New ItemPrice
            Dim buyInBulk As String = String.Empty

            Dim ds As DataSet = DB.GetDataSet(String.Format("sp_SalesPrice_BuyInBulk {0}, {1}", si.ItemId, memberId))
            If ds Is Nothing Or ds.Tables.Count = 0 Then
                Return String.Empty
            End If

            Dim dt As DataTable = ds.Tables(0)
            Dim psc As String = String.Empty
            If Not ds.Tables(2) Is Nothing Then
                psc = ds.Tables(2).Rows(0)(0).ToString()
            End If
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    buyInBulk &= "<table cellspacing=""0"" align=""center"" cellpadding=""0"" border=""0"" class=""multi"">" & vbCrLf
                    ' "<tr><td colspan='2' class='title'>Buy More & Save </td></tr>" & vbCrLf
                    buyInBulk &= "<thead><tr><th class='head'> Case of " & psc & "</th><th class='head'>Price per Case</th></thead>" & vbCrLf
                    buyInBulk &= "<tbody>"
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim objItemMultiPrice As New ItemMultiPrice

                        If dt.Rows(i)("Cases").ToString().StartsWith("1+") Or dt.Rows(i)("Cases").ToString().StartsWith("1 -") Then
                            buyInBulk &= "<tr class='line selectCase' id='trPriceCase" & i & "' >" & vbCrLf
                        Else
                            buyInBulk &= "<tr class='line' id='trPriceCase" & i & "' >" & vbCrLf
                        End If

                        buyInBulk &= "<td class=""multiprice-qty value"">" & dt.Rows(i)(0).ToString() & "</td>" _
                                    & String.Format("<td class=""price value"">{0:C2}</td>", (dt.Rows(i)(1).ToString())) _
                                    & "</tr>" & vbCrLf
                    Next

                    buyInBulk &= "</tbody><tfoot></tfoot></table>"
                    If Not ds.Tables(1) Is Nothing Then
                        dtPrice = ds.Tables(1)
                    End If


                End If
            End If
            ds.Dispose()
            ds = Nothing

            Return buyInBulk
        End Function
    End Class

End Namespace



