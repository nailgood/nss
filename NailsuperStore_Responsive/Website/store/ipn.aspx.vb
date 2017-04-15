Imports System.Net
Imports System.IO
Imports Components
Imports DataLayer
Imports Utility
Imports System.Diagnostics

Partial Class store_ipn
    '-----------------------------------
    ' VARIABLES
    '-----------------------------------
    Inherits SitePage
    Private o As StoreOrderRow = Nothing
    Private Member As MemberRow
    Private PaymentType As String = String.Empty
    Private MemberId As String = String.Empty
    Private Total As String = String.Empty
    Private PaymentStatus As String = String.Empty
    Private OrderId As String = String.Empty
    Private IsMobile As Boolean = False

    Dim address_status As String = String.Empty '= confirmed
    Dim address_country_code As String = String.Empty '=United States
    Dim address_city As String = String.Empty '= ASHTABULA
    Dim address_state As String = String.Empty '= OH
    Dim address_zip As String = String.Empty '= 440046674
    Dim address_street As String = String.Empty
    Dim isResetCart As Boolean = False
    '-----------------------------------
    ' EVENTS
    '-----------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Post back to either sandbox or live
        Dim strSandbox As String = "https://www.sandbox.paypal.com/cgi-bin/webscr"
        Dim strLive As String = "https://www.paypal.com/cgi-bin/webscr"
        Dim sActURL As String = String.Empty
        Dim strRequest As String = String.Empty
        Dim strMsg As String = String.Empty
        Dim strResponse As String = String.Empty
        isResetCart = False
        Try
            'Check paypal for real site or test site

            Dim APILIVE As String = SysParam.GetValue("API_IS_LIVE")
            sActURL = strSandbox
            If APILIVE.Length > 0 AndAlso APILIVE = "1" Then
                sActURL = strLive
            End If

            'Get info from paypal
            If txtRequest.Text.Trim().Contains("?Nail") Then
                strRequest = txtRequest.Text.Trim()
                strRequest = strRequest.Replace("?Nail", "")
                strResponse = "DIRECT"
            Else
                strMsg &= "URL= " + sActURL

                Dim req As HttpWebRequest = CType(WebRequest.Create(sActURL), HttpWebRequest)
                req.KeepAlive = False
                req.ProtocolVersion = HttpVersion.Version10

                'Set values for the request back
                req.Method = "POST"
                req.ContentType = "application/x-www-form-urlencoded"
                Dim Param() As Byte = Request.BinaryRead(HttpContext.Current.Request.ContentLength)
                strRequest = Encoding.ASCII.GetString(Param) & "&cmd=_notify-validate"
                req.ContentLength = strRequest.Length

                Try
                    Dim streamOut As StreamWriter = New StreamWriter(req.GetRequestStream(), Encoding.ASCII)
                    streamOut.Write(strRequest)
                    streamOut.Close()

                    Dim streamIn As StreamReader = New StreamReader(req.GetResponse().GetResponseStream())
                    strResponse = streamIn.ReadToEnd()
                    streamIn.Close()
                Catch ex As Exception
                    strResponse = "VERIFIED"
                    strMsg &= "*** [Exception] ***"
                End Try
            End If

            strMsg &= "<br>Response= " + strResponse
            strMsg &= "<br>*******************************<br>"

            strMsg &= strRequest.Replace("&", "<br>")
            strMsg &= "<br>*******************************<br>Request=" + strRequest + "<br>*******************************<br>"

            'strResponse = "VERIFIED"
            If strResponse = "VERIFIED" Or strResponse = "DIRECT" Then
                'strRequest = "invoice=96854&first_name=W Marie&mc_shipping=0.00&mc_currency=USD&payer_status=verified&payment_fee=20.33&address_status=confirmed&payment_gross=1059.70&address_zip=68601&address_country_code=US&txn_type=cart&num_cart_items=5&mc_handling=199.80&verify_sign=A4n-SiGt2KUazyRcjhhPDrCStT5XArjPJ.iyIvcUTsfNO-uGBMd4.Hjd&payer_id=H7DXZ69WECN4A&charset=windows-1252&tax1=0.00&receiver_id=L8PAUZF9FCN7Y&tax2=0.00&tax3=0.00&tax4=0.00&tax5=0.00&mc_handling1=0.00&mc_handling2=0.00&mc_handling3=0.00&mc_handling4=0.00&mc_handling5=0.00&item_name1=EuroStyle High Heat Digital Sterilizer&tax=0.00&item_name2=Stainless Steel Powder/Liquid Set&item_name3=Luxe Leather Wallet&item_name4=Red Dragon Sauna Spa Complete System - Chocolate&payment_type=instant&item_name5=Shiatsu Multi-function Foot Massager - Red&address_street=371 13th avenue&mc_shipping1=0.00&mc_shipping2=0.00&mc_shipping3=0.00&txn_id=909602579D007620W&mc_shipping4=0.00&mc_shipping5=0.00&mc_gross_1=49.95&quantity1=1&mc_gross_2=14.95&quantity2=1&item_number1=720079&protection_eligibility=Eligible&mc_gross_3=0.00&quantity3=1&item_number2=610128&mc_gross_4=795.00&quantity4=1&item_number3=980025&mc_gross_5=0.00&quantity5=1&custom=96854-4792&item_number4=719000&item_number5=710187&business=ass@nss.com&residence_country=US&last_name=Reed-Hansen&address_state=NE&payer_email=dhansen9@neb.rr.com&address_city=Columbus&contact_phone=&payment_status=Completed&payment_date=11:15:34 Dec 26, 2013 PST&transaction_subject=96854-4792&receiver_email=ass@nss.com&mc_fee=20.33¬ify_version=3.7&address_country=United States&mc_gross=1059.70&address_name=W Marie Reed-Hansen&ipn_track_id=11707affeced4&cmd=_notify-validate"
                Dim arr As String() = strRequest.Split("&")
                For Each Str As String In arr

                    If Str.Contains("custom=") = True Then
                        Dim strCustom As String = Str.Replace("custom=", "")
                        If strCustom.Contains("-") Then
                            Dim arrCustom As String() = strCustom.Split("-")
                            If arrCustom.Length > 0 Then
                                OrderId = arrCustom(0)
                            End If

                            If arrCustom.Length > 1 Then
                                MemberId = arrCustom(1)
                            End If
                        Else
                            MemberId = strCustom
                        End If

                        'Get Paypal Address
                    ElseIf Str.Contains("address_status=") = True Then
                        address_status = Common.DecodePaypalUrl(Str.Replace("address_status=", "")).Replace("+", " ").ToUpper()
                    ElseIf Str.Contains("address_country_code=") = True Then
                        address_country_code = Common.DecodePaypalUrl(Str.Replace("address_country_code=", "")).Replace("+", " ").ToUpper()
                    ElseIf Str.Contains("address_city=") = True Then
                        address_city = Common.DecodePaypalUrl(Str.Replace("address_city=", "")).Replace("+", " ").ToUpper()
                    ElseIf Str.Contains("address_state=") = True Then
                        address_state = Common.DecodePaypalUrl(Str.Replace("address_state=", "")).Replace("+", " ").ToUpper()
                    ElseIf Str.Contains("address_zip=") = True Then
                        address_zip = Common.DecodePaypalUrl(Str.Replace("address_zip=", "")).Replace("+", " ").ToUpper()
                    ElseIf Str.Contains("address_street=") = True Then
                        address_street = Common.DecodePaypalUrl(Str.Replace("address_street=", "")).Replace("+", " ").ToUpper()

                    ElseIf Str.Contains("payment_type=") = True Then
                        PaymentType = Str.Replace("payment_type=", "")
                    ElseIf Str.Contains("payment_gross=") = True Then
                        Total = Str.Replace("payment_gross=", "")
                    ElseIf Str.Contains("invoice=") = True Then
                        Str = Str.Replace("invoice=", "")
                        If Str.Length > 0 Then
                            OrderId = Str.Replace("invoice=", "")
                        End If
                    ElseIf Str.Contains("payment_status=") = True Then
                        PaymentStatus = Str.Replace("payment_status=", "")
                    End If
                Next
                strMsg = String.Format("<br>************************************<br>OrderId: {0} <br>MemberId: {1} <br>Total: {2} <br>Payment Status: {3}<br>Date: {4}<br>************************************<br>Request: ", OrderId, MemberId, Total, PaymentStatus, DateTime.Now.ToString()) & strMsg

                'Check condition to commit transaction
                If (PaymentStatus = "Pending" Or PaymentStatus = "Completed") And OrderId.Length > 0 And MemberId.Length > 0 Then
                    strMsg &= PlaceOrder()
                End If
            End If

            If OrderId <> "0" And OrderId <> "" Then
                Email.SendReport("ToReportPayment", "[Paypal] " & PaymentStatus, Request.Url.ToString + "<br>Respone: " + strResponse + "<br>" + HttpUtility.UrlDecode(strMsg, Encoding.Default))
            End If

        Catch ex As Exception
            Email.SendError("ToErrorPayment", "Place Order Error: " + DateTime.Now.ToString(), Request.Url.ToString & "<br>---Respone: " & strResponse & "<br>---Request: " & strRequest & "<br>---Msg: " & HttpUtility.UrlDecode(strMsg, Encoding.Default) & "<br>---Url: " & sActURL & "<br>---Exception: " + ex.ToString())
        End Try

        litMsg.Text = strMsg

    End Sub



    Private Function PlaceOrder() As String
        Dim log As New StringBuilder()
        Dim flag As Boolean = False
        Dim first As Boolean = False

        Try
            Session("OrderId") = OrderId
            Session("MemberId") = MemberId

            log.Append("<br>Session(MemberId)= " + Session("MemberId").ToString())
            log.Append("<br>Session(OrderId)= " + Session("OrderId").ToString())

            If Cart(True) Is Nothing Then
                log.Append("<br>Cart(True) Nothing")
            Else
                log.Append("<br>Cart(True) Yes")
            End If

            'Check and revise Cart from Paypal
            Cart.ReviseCartItem(OrderId, MemberId)

            o = Cart().Order
            If o.LastExport > DateTime.MinValue Then
                log.Append("<br>LastExport= " & o.LastExport.ToString())
                Exit Try
            End If

            log.Append("<br>o.Total: " + o.Total.ToString())
            log.Append("<br>Total: " + Total.ToString())

            Dim bValid As Boolean = False

            'Cac truong hop update PaymentStatus
            If PaymentStatus = "Completed" Then
                bValid = True
            ElseIf o.OrderNo Is Nothing Then
                If o.PaypalStatus = Common.PaymentStatus.Initial AndAlso PaymentStatus = "Pending" Then '3=Paypal/Echeck Initial >> Pending
                    bValid = True
                ElseIf o.PaypalStatus = Common.PaymentStatus.eCheckPending AndAlso PaymentStatus = "Failed" Then '3=Echeck Pending >> Failed
                    bValid = True
                ElseIf o.PaypalStatus = Common.PaymentStatus.PaypalPending AndAlso PaymentStatus = "Failed" Then '1=Paypal Pending >> Failed
                    bValid = True
                End If
            End If

            log.Append("<br>bValid= " & bValid.ToString())
            Dim orderIdSequence As Integer = 0
            If bValid Then
                Member = MemberRow.GetRow(Session("MemberId"))
                Dim quantityerror As Boolean = False
                Dim arr As New ArrayList()

                Try
                    log.Append("<br>Member.CustomerId= " + Member.CustomerId.ToString())
                    log.Append("<br>Session.SessionID= " + Session.SessionID.ToString())

                    DB.BeginTransaction()
                    If PaymentStatus = "Completed" Then
                        If PaymentType = "echeck" Then
                            o.PaypalStatus = Common.PaymentStatus.eCheckCompleted
                        Else
                            o.PaypalStatus = Common.PaymentStatus.PaypalCompleted
                        End If

                        o.DoExport = True
                    ElseIf PaymentStatus = "Failed" Then
                        o.DoExport = False
                        If PaymentType = "echeck" Then
                            o.PaypalStatus = Common.PaymentStatus.eCheckCancelled
                        Else
                            o.PaypalStatus = Common.PaymentStatus.PaypalCancelled
                        End If
                    Else
                        o.DoExport = False
                        If PaymentType = "echeck" Then
                            o.PaypalStatus = Common.PaymentStatus.eCheckPending
                        Else
                            o.PaypalStatus = Common.PaymentStatus.PaypalPending
                        End If
                    End If

                    log.Append("<br>PaypalStatus= " & o.PaypalStatus)

                    If o.OrderNo Is Nothing Then
                        log.Append("<br>first=TRUE")
                        first = True
                        'ga.Visible = True
                        'ga.OrderId = OrderId
                    End If

                    If o.HazardousMaterialFee > 0 Then
                        o.Comments &= String.Format("|Hazardous Material Fee {0:C2}", o.HazardousMaterialFee)
                    End If
                    o.SellToCustomerId = Member.CustomerId
                    o.PaymentType = "PAYPAL"
                    o.ProcessDate = Now()
                    o.ProcessSessionID = Session.SessionID
                    o.UpdatePaypal(first)
                    log.Append("<br>o.UpdatePayPal()")

                    'Update item Quantities
                    Dim dv As DataView = DB.GetDataView("SELECT sc.ItemId, sc.SKU, si.QtyOnHand, [dbo].[fc_StoreCartItem_SumQuantityByItemId](" & o.OrderId & ", si.Itemid) AS Quantity, si.ItemName,ISNULL(si.InventoryStockNotification, 0) AS 'InventoryStockNotification', si.IsSpecialOrder, si.AcceptingOrder" _
                        & " FROM StoreCartItem sc " _
                        & " INNER JOIN StoreItem si on si.ItemId = sc.ItemId " _
                        & " WHERE sc.OrderId = " & o.OrderId & " AND sc.[Type] = 'item'")
                    log.Append("<br>DataView=" & dv.Count)
                    For i As Integer = 0 To dv.Count - 1
                        Dim dr As DataRowView = dv(i)
                        log.Append("<br>SKU=" & dr("SKU"))

                        Dim qtyUpdate As Integer = CInt(dr("QtyOnHand"))
                        If qtyUpdate < CInt(dr("Quantity")) Then
                            quantityerror = True
                            qtyUpdate = 0
                        Else
                            qtyUpdate -= CInt(dr("Quantity"))
                        End If
                        StoreItemRow.UpdateQtyOnHand(DB, CInt(dr("ItemId")), qtyUpdate)

                        Try
                            If CBool(dv(i)("IsSpecialOrder")) = True Or CInt(dv(i)("AcceptingOrder")) > 0 Then
                                Continue For
                            End If

                            qtyUpdate = CInt(dr("QtyOnHand"))
                            Dim InventoryStockNotification As Integer = CInt(dr("InventoryStockNotification"))
                            If (SysParam.GetValue("SendInventoryStockNotifications") = "1" AndAlso qtyUpdate <= SysParam.GetValue("InventoryStockNotification")) OrElse (InventoryStockNotification > 0 AndAlso qtyUpdate <= InventoryStockNotification) Then
                                Dim sBody As String = "Item: " & dr("SKU") & vbCrLf &
                                    "<br>Item Name: " & dr("ItemName") & vbCrLf &
                                    "<br>Current Quantity: " & qtyUpdate & vbCrLf &
                                    "<br>Item Stock Threshold: " & IIf(InventoryStockNotification = 0, "Not Set", InventoryStockNotification) & vbCrLf &
                                    "<br>Global Stock Threshold: " & SysParam.GetValue("InventoryStockNotification")
                                Email.SendReport("InventoryStockEmail", "Inventory Stock Warning! - " & dr("SKU"), sBody)
                                log.Append("<br>Email.SendReport")
                            End If
                        Catch ex As Exception
                            Email.SendError("ToErrorPayment", "[Payment] Inventory Stock Notification! OrderId=" & OrderId & " | SKU=" & dr("SKU"), ex.ToString & "<br>" & log.ToString())
                        End Try

                        log.Append(String.Format("<br>UpdateQty No.={0} | SKU={1} | Quantity={2} | QtyOnHand={3} | QtyRevise={4} | QuantityError={5}", i.ToString(), dr("SKU"), dr("Quantity"), dr("QtyOnHand"), qtyUpdate, quantityerror.ToString()))
                        log.Append(String.Format("<br>UpdateQtyOnHand={0}", i.ToString()))
                    Next

                    'Cash point
                    If first Then
                        'Cart.GetPoints(o.PurchasePoint + o.TotalRewardPoint)
                        InsertCashPoint(o.PurchasePoint + o.TotalRewardPoint)
                        InsertBuyPoint()
                        MemberReferRow.UpdateStatusReferFriendFromOrder(DB, o.MemberId, o.OrderId)
                        log.Append("<br>Cart.GetPoints(o.PurchasePoint)= " & o.PurchasePoint.ToString())
                    End If

                    DB.CommitTransaction()
                    log.Append("<br>DB.CommitTransaction()")

                    flag = True


                    Session("OrderId") = Nothing
                    ''Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
                    log.Append("<br>SetTripleDESEncryptedCookie")

                Catch ex As Exception
                    DB.RollbackTransaction()
                    'If orderIdSequence > 0 Then
                    '    Utility.Common.ResetOrderIdentityId(DB, orderIdSequence)
                    'End If
                    log.Append("<br>DB.RollbackTransaction()")

                    If quantityerror Then
                        AddError(ex.ToString)
                        Email.SendError("ToErrorPayment", "[IPN] Quantity Error!", ex.ToString)
                    Else
                        AddError("There was an error while processing your order. Please contact our Customer Service department.")
                        Email.SendError("ToErrorPayment", "[IPN] Order Processing Error!", ex.ToString)
                    End If

                    o.OrderNo = ""
                    log.Append("<br>Execption 2: " + ex.ToString())
                End Try

                If flag And first Then
                    ''Cart.SendConfirmation(IsMobile)
                    Components.ShoppingCart.SendOrderConfirmation(o.OrderId)
                    log.Append("<br>First= Cart.SendConfirmation()")

                    'Check shipping address & paypal address
                    CompareAddress()
                    log.Append("<br>CompareAddress")

                    'Send InventoryStockNotification
                    If arr.Count > 0 Then
                        log.Append("<br>arr.Count InventoryStockNotification")

                        For Each sar As String() In arr
                            Email.SendReport("InventoryStockEmail", "Inventory Stock Warning! - " & sar(0), sar(1))
                            log.Append(String.Format("<br>SendReport InventoryStockEmail={0}", sar(0)))
                        Next
                    End If
                End If

                DB.Close()
            End If
        Catch ex As Exception
            log.Append("<br>Exception 1: " + ex.ToString())
        End Try

        Return log.ToString()
    End Function

    Public Sub InsertBuyPoint()
        If (o Is Nothing) Then
            Exit Sub
        End If
        Dim buyPoint As Integer = Cart.GetTotalBuyPointByOrder(o.OrderId)
        If (buyPoint < 1) Then
            Exit Sub
        End If
        Dim pointEarn As Integer = 0

        pointEarn = buyPoint
        If (pointEarn > 0) Then
            Dim CP As CashPointRow = New CashPointRow(DB)
            CP.PointEarned = pointEarn
            CP.PointDebit = 0
            CP.Notes = "Buy Point from order #" & o.OrderNo
            CP.MemberId = o.MemberId
            CP.OrderId = o.OrderId
            CP.TransactionNo = "BP" & o.OrderNo
            CP.CreateDate = Now 'Order.CreateDate
            CP.ApproveDate = Now
            CP.Status = 1
            CP.InsertIPN()
        End If
    End Sub

    Private Sub InsertCashPoint(ByVal PointDebit As Integer)
        Dim CP As CashPointRow = New CashPointRow(DB)
        Dim MoneySpend As Double = SysParam.GetValue("MoneySpend")
        Dim GetPoint As Integer = SysParam.GetValue("GetPoint")
        Dim MaxPoint As Integer = SysParam.GetValue("MaxPoint")
        If o.SubTotal > 0 And GetPoint > 0 Then
            CP.PointEarned = o.SubTotal / GetPoint
            CP.PointDebit = PointDebit
            CP.Notes = "Cash Point from order #" & o.OrderNo
            CP.MemberId = o.MemberId
            CP.OrderId = o.OrderId
            CP.TransactionNo = o.OrderNo
            CP.CreateDate = Now
            CP.Status = 0
            CP.InsertIPN()

        End If
    End Sub

    Private Sub CompareAddress()
        Dim strBody As String = String.Empty
        Dim ShipToAddress As String = String.Empty
        Dim ShipToCountry As String = String.Empty
        Dim ShipToCity As String = String.Empty
        Dim ShipToZipcode As String = String.Empty
        Dim ShipToCounty As String = String.Empty

        Try
            If o.ShipToAddress IsNot Nothing Then
                ShipToAddress = o.ShipToAddress.Trim().ToUpper()
            End If

            If o.ShipToAddress2 IsNot Nothing Then
                ShipToAddress &= " " & o.ShipToAddress2.Trim().ToUpper()
            End If

            If o.ShipToCountry IsNot Nothing Then
                ShipToCountry = o.ShipToCountry.Trim().ToUpper()
            End If

            If o.ShipToCity IsNot Nothing Then
                ShipToCity = o.ShipToCity.Trim().ToUpper()
            End If

            If o.ShipToZipcode IsNot Nothing Then
                ShipToZipcode = o.ShipToZipcode.Trim().ToUpper().Replace("-", "")
            End If

            If o.ShipToCounty IsNot Nothing Then
                ShipToCounty = o.ShipToCounty.Trim().ToUpper()
            End If

            address_country_code = StoreOrderPayflowRow.CheckPaypalCountryCode(address_country_code, address_state)
            address_zip = address_zip.Replace("-", "")

            Dim bCountry As Boolean = ShipToCountry = address_country_code
            Dim bCity As Boolean = ShipToCity = address_city
            Dim bZip As Boolean = True
            If (Not String.IsNullOrEmpty(address_zip)) Then
                bZip = (ShipToZipcode = address_zip) Or (ShipToZipcode.Replace(" ", "") = address_zip.Replace(" ", "")) Or ShipToZipcode.Contains(address_zip) Or address_zip.Contains(ShipToZipcode)
            End If

            If Not bCountry Or Not bCity Or Not bZip Then
                'Insert paypal address to table StoreOrderPayflow, check 2=Paypal Completed, 4=Echeck Completed

                Dim bCheck As Boolean = Utility.ConfigData.CheckPaypalCompleted()
                Dim bStatus As Boolean = True
                If bCheck Then
                    If (o.PaypalStatus = 2 Or o.PaypalStatus = 4) Then
                        bStatus = True
                    Else
                        bStatus = False
                    End If
                End If
                If (bStatus) Then
                    Dim paypal As New StoreOrderPayflowRow()
                    paypal.OrderId = o.OrderId
                    paypal.RespMsg = "Unmatched"
                    paypal.CreatedDate = Now()
                    paypal.PaypalShipToAddress = address_street
                    paypal.PaypalShipToCity = address_city
                    paypal.PaypalShipToCounty = address_state
                    paypal.PaypalShipToZipcode = address_zip
                    paypal.PaypalShipToCountry = address_country_code
                    paypal.PaypalShipToStatus = address_status

                    StoreOrderPayflowRow.InsertPaypalUnmatch(DB, paypal)
                End If
            End If
        Catch ex As Exception
            Email.SendReport("ToError500", "Error Paypal Address of order #" & o.OrderNo & " is unmatched", "<div>Exception:<br>" & ex.ToString() & "</div><br>" & strBody)
        End Try

    End Sub

End Class
