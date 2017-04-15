Imports System.Net
Imports System.IO
Imports Components
Imports DataLayer
Imports NailCache

Public Class admin_complete_order
    '-----------------------------------
    ' VARIABLES
    '-----------------------------------
    Inherits AdminPage
    Private o As StoreOrderRow = Nothing
    Private Member As MemberRow
    Private PaymentStatus As String = String.Empty
    Private PaymentType As String = String.Empty
    Dim sp As SitePage
    '-----------------------------------
    ' EVENTS
    '-----------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSubmitCon_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim nc As New NailCache()
        Dim so As StoreOrderRow = StoreOrderRow.GetRowByOrderNo(DB, txtOrderNo.Text.Trim())
        nc.SendOrderConfirmation(Utility.Crypt.EncryptTripleDes(so.OrderId))
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Session.Remove("OrderId")
        Session.Remove("MemberId")
        Dim OrderId As String = txtOrderId.Text
        Dim MemberId As String = txtMemberId.Text
        Dim PaymentStatus As String = drpPaymentStatus.SelectedValue
        Dim MemberPaymentTypeId As String = drpPaymentType.SelectedValue
        'Dim Total As String = txtTotal.Text

        Dim log As New StringBuilder()
        Dim flag As Boolean = False
        Dim first As Boolean = False
        Dim str As String = ""

        Try
            Session("OrderId") = OrderId
            Session("MemberId") = MemberId
            sp = New SitePage()
            If sp.Cart(True) Is Nothing Then
                log.Append("<br>Cart(True) Nothing")
            Else
                log.Append("<br>Cart(True) Yes")
            End If

            'Check and revise Cart from Paypal
            sp.Cart.ReviseCartItem(OrderId, MemberId)

            o = sp.Cart().Order
            If o.LastExport > DateTime.MinValue Then
                log.Append("<br>LastExport= " & o.LastExport.ToString())
                Exit Try
            End If

            log.Append("<br>o.Total: " + o.Total.ToString())

            Dim bValid As Boolean = False
            If o.OrderNo Is Nothing AndAlso o.PaypalStatus = 0 AndAlso PaymentStatus = "Pending" Then
                bValid = True
            ElseIf PaymentStatus = "Completed" Then
                bValid = True
            End If

            log.Append("<br>bValid= " & bValid.ToString())

            If bValid Then
                Member = MemberRow.GetRow(Session("MemberId"))
                Dim quantityerror As Boolean = False
                Dim arr As New ArrayList()

                Try
                    log.Append("<br>Member.CustomerId= " + Member.CustomerId.ToString())
                    log.Append("<br>Session.SessionID= " + Session.SessionID.ToString())

                    DB.BeginTransaction()
                    If PaymentStatus = "Completed" Then
                        o.DoExport = True
                        o.PaypalStatus = IIf(PaymentType = "echeck", 4, 2) '2=Paypal Completed, 4=Echeck Completed

                        SQL = "update storecartitem set doexport = 1 where orderid = " & o.OrderId
                        DB.ExecuteSQL(SQL)
                    Else
                        o.DoExport = False
                        o.PaypalStatus = IIf(PaymentType = "echeck", 3, 1) '0=Initial, 1=Paypal Pending, 3=Echeck Pending
                    End If

                    log.Append("<br>PaypalStatus= " & o.PaypalStatus)

                    If o.OrderNo Is Nothing Then
                        o.OrderNo = "W" & sp.Cart.GetNextSequenceNo()
                        log.Append("<br>OrderNo= " & o.OrderNo)
                        first = True
                    End If

                    o.SellToCustomerId = Member.CustomerId
                    o.PaymentType = "PAYPAL"
                    o.ProcessDate = Now()
                    o.ReferralCode = Nothing
                    o.ProcessSessionID = Session.SessionID
                    o.Update()
                    log.Append("<br>o.Update()")

                    'Update item Quantities
                    Dim dv As DataView = DB.GetDataView("SELECT sc.ItemId, sc.SKU, sc.Quantity, si.QtyOnHand, (si.QtyOnHand - sc.Quantity) AS QtyRevise, si.ItemName, ISNULL(si.InventoryStockNotification, 0) AS InventoryStockNotification " _
                        & " FROM StoreCartItem sc " _
                        & " INNER JOIN StoreItem si on si.ItemId = sc.ItemId " _
                        & " WHERE sc.OrderId = " & o.OrderId & " AND sc.[Type] = 'item'")

                    For i As Integer = 0 To dv.Count - 1
                        Try
                            Dim qtyRevise As Integer = CInt(dv(i)("QtyRevise"))
                            Dim qtyOnHand As Integer = CInt(dv(i)("QtyOnHand"))
                            Dim isn As Integer = CInt(dv(i)("InventoryStockNotification"))

                            quantityerror = qtyRevise < 0
                            log.Append(String.Format("<br>UpdateQty No.={0} | SKU={1} | Quantity={2} | QtyOnHand={3} | QtyRevise={4} | QuantityError={5}", i.ToString(), dv(i)("SKU"), dv(i)("Quantity"), dv(i)("QtyOnHand"), dv(i)("QtyRevise"), quantityerror.ToString()))

                            StoreItemRow.UpdateQtyOnHand(DB, dv(i)("ItemId"), qtyRevise)
                            log.Append(String.Format("<br>UpdateQtyOnHand={0}", i.ToString()))

                            If SysParam.GetValue("SendInventoryStockNotifications") = "1" AndAlso qtyOnHand <= SysParam.GetValue("InventoryStockNotification") OrElse isn > 0 AndAlso qtyOnHand <= isn Then
                                Dim sBody As String = "Item: " & dv(i)("SKU") & vbCrLf & _
                                 "Item Name: " & dv(i)("ItemName") & vbCrLf & _
                                 "Current Quantity: " & qtyOnHand & vbCrLf & _
                                 "Item Stock Threshold: " & IIf(isn = 0, "Not Set", isn & vbCrLf & _
                                 "Global Stock Threshold: " & SysParam.GetValue("InventoryStockNotification"))

                                Dim sar() As String = {dv(i)("SKU"), sBody}
                                arr.Add(sar)
                                log.Append(String.Format("<br>InventoryStockNotification={0} | {1}", i.ToString(), dv(i)("SKU")))
                            End If

                        Catch ex As Exception
                            Email.SendError("ToErrorPayment", "[IPN] Inventory Stock Notification!", ex.ToString)
                            log.Append(String.Format("<br>InventoryStockNotification Error={0}", i.ToString()))
                        End Try
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
                    Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
                    log.Append("<br>SetTripleDESEncryptedCookie")

                Catch ex As Exception
                    DB.RollbackTransaction()
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

                If (flag And first) Then
                    If chkSend.Checked Then
                        ''sp.Cart.SendConfirmation(False)
                        Components.ShoppingCart.SendOrderConfirmation(o.OrderId)
                        log.Append("<br>First= Cart.SendConfirmation()")
                    End If

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

        lblResult.Text = log.ToString()
    End Sub

    Public Sub InsertBuyPoint()
        If (o Is Nothing) Then
            Exit Sub
        End If
        Dim buyPoint As Integer = sp.Cart.GetTotalBuyPointByOrder(o.OrderId)
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

    Protected Sub btnCredit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCredit.Click

        Session.Remove("OrderId")
        Session.Remove("MemberId")
        Dim arrMail As New ArrayList
        Dim quantityerror As Boolean = False
        Dim OrderId As String = txtOrderIdCr.Text
        Dim MemberId As String = txtMemberIdCr.Text
        Try
            Session("OrderId") = OrderId
            Session("MemberId") = MemberId
            Member = MemberRow.GetRow(Session("MemberId"))
            DB.BeginTransaction()

            o = sp.Cart().Order
            o.PaymentType = "CC"
            ProcessCC()

            o.Notes = ""
            o.SellToCustomerId = Member.CustomerId
            o.DoExport = True
            o.ProcessDate = Now()
            o.OrderNo = "W" & sp.Cart.GetNextSequenceNo()
            'Cart.GetPoints(o.PurchasePoint)
            If o.PurchasePoint = 0 Then
                Dim PointDebit As Integer = DB.ExecuteScalar("Select isnull(PointDebit,0) from CashPoint where [Status] = 0 and OrderId = " & OrderId)
                Try
                    If PointDebit > 0 Then
                        sp.Cart.GetPurchasePoint(o, PointDebit, PointDebit & " points save USD " & PointDebit * SysParam.GetValue("MoneyEachPoint"))
                    ElseIf CInt(txtPurchasePointCr.Text) > 0 Then
                        sp.Cart.GetPurchasePoint(o, CInt(txtPurchasePointCr.Text), CInt(txtPurchasePointCr.Text) & " points save USD " & CInt(txtPurchasePointCr.Text) * SysParam.GetValue("MoneyEachPoint"))
                        sp.Cart.GetPoints(CInt(txtPurchasePointCr.Text))
                    End If
                Catch ex As Exception

                End Try


            End If

            'Cash point
            If Session("ref") <> Nothing Then o.ReferralCode = Session("ref")
            o.ProcessSessionID = Session.SessionID
            o.Update()
            SQL = "update storecartitem set doexport = 1 where orderid = " & o.OrderId
            DB.ExecuteSQL(SQL)
            Dim email As String = SysParam.GetValue("InventoryStockEmail")
            Dim si As StoreItemRow = Nothing
            Dim dv As DataView = DB.GetDataView("select itemid, quantity, itemname from storecartitem where orderid = " & o.OrderId & " and [type] = 'item'")
            Dim qtyUpdate As Integer
            For i As Integer = 0 To dv.Count - 1
                si = StoreItemRow.GetRow(DB, Convert.ToInt32(dv(i)("itemid")))
                qtyUpdate = si.QtyOnHand
                If qtyUpdate < dv(i)("quantity") Then
                    quantityerror = True
                    qtyUpdate = 0
                Else
                    qtyUpdate -= dv(i)("quantity")
                End If
                ''si.Update()
                StoreItemRow.UpdateQtyOnHand(DB, si.ItemId, qtyUpdate)

                '' CacheUtils.RemoveCacheWithPrefix("StoreItem_GetRow_" & si.ItemId & "_")
                Try
                    If SysParam.GetValue("SendInventoryStockNotifications") = "1" AndAlso qtyUpdate <= SysParam.GetValue("InventoryStockNotification") OrElse si.InventoryStockNotification > 0 AndAlso qtyUpdate <= si.InventoryStockNotification Then
                        Dim sBody As String

                        sBody = si.SKU & "|Item: " & si.SKU & vbCrLf & _
                         "Item Name: " & si.ItemName & vbCrLf & _
                         "Current Quantity: " & qtyUpdate & vbCrLf & _
                         "Item Stock Threshold: " & IIf(si.InventoryStockNotification = 0, "Not Set", si.InventoryStockNotification) & vbCrLf & _
                         "Global Stock Threshold: " & SysParam.GetValue("InventoryStockNotification")
                        arrMail.Add(sBody)

                    End If
                Catch ex As Exception
                    Components.Email.SendError("ToErrorPayment", "[Payment] Inventory Stock Notification!", ex.ToString)
                End Try
            Next

            InsertCashPoint(o.PurchasePoint + o.TotalRewardPoint)
            InsertBuyPoint()
            DB.CommitTransaction()
            lblResult.Text = "Complete"
        Catch ex As Exception
            If quantityerror Then
                AddError(ex.ToString())
                Components.Email.SendError("ToErrorPayment", "[CreditCard] Quantity Error!", ex.ToString)
            Else
                AddError("There was an error while processing your order. Please contact our Customer Service department.")
                Components.Email.SendError("ToErrorPayment", "[CreditCard] Order Processing Error!", ex.ToString)
            End If

            lblResult.Text = "Error: " & ex.ToString()
            DB.RollbackTransaction()
        End Try
        '' sp.Cart.SendConfirmation(False)
        Components.ShoppingCart.SendOrderConfirmation(o.OrderId)

        Session("OrderId") = Nothing
        Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)


        ''send mail
        If arrMail.Count > 0 Then
            Dim email As String = SysParam.GetValue("InventoryStockEmail")
            Dim index As Integer = -1
            Dim sku As String = String.Empty
            Dim sBody As String = String.Empty
            For Each body As String In arrMail
                index = body.IndexOf("|")
                sku = body.Substring(0, index)
                sBody = body.Substring(index + 1, body.Length - index - 1)
                Core.SendSimpleMail(email, email, email, email, "Inventory Stock Warning! - " & sku, sBody)
            Next
        End If


        DB.Close()

    End Sub

    Private Sub ProcessCC()
        o.CardNumber = txtCardNum.Text
        o.CIDNumber = txtCardID.Text
        o.CardHolderName = txtCardName.Text
        o.CardTypeId = DB.ExecuteScalar("select top 1 cardtypeid from creditcardtype where code = " & DB.Quote(drType.SelectedValue))
        Dim Expireation As DateTime = New DateTime(drYear.SelectedValue, drMonth.SelectedValue, 1)
        o.ExpirationDate = Expireation
    End Sub


End Class
