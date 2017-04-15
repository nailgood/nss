Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Imports Utility.Common

Partial Class admin_store_orders_Index
    Inherits AdminPage
    '----------------------VARIABLES
    Private MemberId As Integer
    '----------------------EVENTS
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Try
            MemberId = Convert.ToInt32(Request("MemberId"))
        Catch ex As Exception

        End Try



        If Not IsPostBack Then
            pagerTop.PageIndex = 1
            pagerBottom.PageIndex = 1
            F_BillToCounty.Items.AddRange(StateRow.GetStateList().ToArray())
            F_BillToCounty.DataBind()
            F_BillToCounty.Items.Insert(0, New ListItem("-- State --", ""))

            F_BillToCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            F_BillToCountry.DataBind()
            F_BillToCountry.Items.Insert(0, New ListItem("-- Country --", ""))

            F_ShipToCounty.Items.AddRange(StateRow.GetStateList().ToArray())
            F_ShipToCounty.DataBind()
            F_ShipToCounty.Items.Insert(0, New ListItem("-- State --", ""))

            F_ShipToCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            F_ShipToCountry.DataBind()
            F_ShipToCountry.Items.Insert(0, New ListItem("-- Country --", ""))

            F_OrderNo.Text = Request("F_OrderNo")

            F_BillToSalonName.Text = Request("F_BillToSalonName")
            F_BillToName.Text = Request("F_BillToName")
            F_BillToName2.Text = Request("F_BillToName2")
            F_BillToZipcode.Text = Request("F_BillToZipcode")

            F_ShipToSalonName.Text = Request("F_ShipToSalonName")
            F_ShipToName.Text = Request("F_ShipToName")
            F_ShipToName2.Text = Request("F_ShipToName2")
            F_ShipToZipCode.Text = Request("F_ShipToZipCode")

            F_OrderFrom.SelectedValue = Request("F_OrderFrom")

            F_Email.Text = Request("F_Email")
            F_BillToCounty.SelectedValue = Request("F_BillToCounty")
            F_BillToCountry.SelectedValue = Request("F_BillToCountry")
            F_ShipToCountry.SelectedValue = Request("F_ShipToCountry")
            F_ShipToCountry.SelectedValue = Request("F_ShipToCountry")
            F_OrderDateLbound.Text = Request("F_OrderDateLBound")
            F_OrderDateUbound.Text = Request("F_OrderDateUBound")

            F_PayPalAddress.SelectedValue = Request("F_PayPalAddress")
            F_PayPalType.SelectedValue = Request("F_PayPalType")

            F_PayPalType.SelectedValue = Request("F_PayPalType")
            F_PaymentType.SelectedValue = Request("F_PayPalType")

            F_OrderDate.SelectedValue = Request("F_OrderDate")
            F_CustomerNo.Text = Request("F_CustomerNo")
            F_Total.Text = Request("F_Total")
            'F_IsEbay.SelectedValue = Request("F_IsEbay")
            ViewState("F_SortBy") = Core.ProtectParam(Request("F_SortBy"))
            ViewState("F_SortOrder") = Core.ProtectParam(Request("F_SortOrder"))
            If String.IsNullOrEmpty(ViewState("F_SortBy")) Then
                ViewState("F_SortBy") = "ProcessDate"
            End If
            If String.IsNullOrEmpty(ViewState("F_SortOrder")) Then
                ViewState("F_SortOrder") = "desc"
            End If
            divDownload.Visible = False
            gvList.Visible = True
            BuildQuery()
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim selectRow As Integer = 0
        Dim total As Integer = 0
        ''pagerTop.PageIndex, pagerTop.PageSize
        Dim lstOrder As StoreOrderCollection = StoreOrderRow.GetListOrder(pagerTop.PageIndex, pagerTop.PageSize, hidCon.Value, ViewState("F_SortBy"), ViewState("F_SortOrder"), total) 'MemberRow.GetListAdmin(DB, gvList.PageIndex + 1, gvList.PageSize, Conn, gvList.SortBy, gvList.SortOrder, total)

        If Not lstOrder Is Nothing AndAlso lstOrder.Count > 0 Then
            selectRow = lstOrder.Count
        End If
        gvList.DataSource = lstOrder
        gvList.DataBind()
        pagerTop.SetPaging(selectRow, total)
        pagerBottom.SetPaging(selectRow, total)

       
    End Sub
    Private Sub btnSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSort.Click
        SortList(hidSortField.Value)
    End Sub
    Public Sub SortList(ByVal F_SortField As String)
        If (F_SortField = ViewState("F_SortBy")) Then
            If ViewState("F_SortOrder") = "ASC" Then
                ViewState("F_SortOrder") = "DESC"
            Else
                ViewState("F_SortOrder") = "ASC"
            End If
        Else
            ViewState("F_SortBy") = F_SortField
            ViewState("F_SortOrder") = "ASC"
        End If
        BindList()
    End Sub
    Protected Sub pagerTop_PageIndexChanging(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
        pagerTop.PageIndex = e.PageIndex
        pagerBottom.PageIndex = e.PageIndex
        BindList()
    End Sub

    Protected Sub pagerTop_PageSizeChanging(ByVal obj As Object, ByVal e As PageSizeChangeEventArgs)
        pagerTop.PageSize = e.PageSize
        pagerTop.PageIndex = 1
        pagerBottom.PageSize = e.PageSize
        pagerBottom.PageIndex = 1

        If (DirectCast(obj, controls_layout_pager).ID = "pagerBottom") Then
            pagerTop.ViewAll = pagerBottom.ViewAll
        Else
            pagerBottom.ViewAll = pagerTop.ViewAll
        End If
        BindList()
    End Sub
    Private Function BuildQuery() As String
        Dim SQL As String
      
        'SQL = " FROM StoreOrder SO WHERE ProcessDate is not null "
        SQL = " ProcessDate is not null "
        Dim Conn As String = " AND "

        If Not F_BillToCounty.SelectedValue = String.Empty And F_BillToCountry.SelectedValue = "US" Then
            SQL = SQL & Conn & "BillToCounty = " & DB.Quote(F_BillToCounty.SelectedValue)
            Conn = " AND "
        End If
        If Not F_BillToCountry.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "BillToCountry = " & DB.Quote(F_BillToCountry.SelectedValue)
            Conn = " AND "
        End If

        If Not F_ShipToCounty.SelectedValue = String.Empty And F_ShipToCountry.SelectedValue = "US" Then
            SQL = SQL & Conn & "ShipToCounty = " & DB.Quote(F_ShipToCounty.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ShipToCountry.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "ShipToCountry = " & DB.Quote(F_ShipToCountry.SelectedValue)
            Conn = " AND "
        End If
        Dim arrOrderNo As String()
        If Not F_OrderNo.Text = String.Empty Then
            Try
                arrOrderNo = F_OrderNo.Text.Split(",")
                If arrOrderNo.Length > 1 Then
                    Dim listOrderNo As String = ""
                    For i As Integer = 0 To arrOrderNo.Length - 1
                        listOrderNo &= "'" & Trim(arrOrderNo(i)) & "',"
                    Next
                    SQL = SQL & Conn & "OrderNo in( " & Left(listOrderNo, listOrderNo.Length - 1) & ")"
                    Conn = " AND "
                Else
                    SQL = SQL & Conn & "OrderNo LIKE " & DB.FilterQuote(F_OrderNo.Text)
                    Conn = " AND "
                End If
            Catch ex As Exception

            End Try
        Else
            SQL = SQL & Conn & "OrderNo is not null "
            Conn = " AND "
        End If
        If Not txtTransactionID.Text = String.Empty Then
            SQL = SQL & Conn & "(SELECT COUNT(PayflowId) FROM StoreOrderPayflow WHERE OrderId = SO.OrderId AND PnRef = '" & txtTransactionID.Text & "') > 0"
            Conn = " AND "
        End If
        If Not F_BillToSalonName.Text = String.Empty Then
            SQL = SQL & Conn & "BillToSalonName LIKE " & DB.FilterQuote(F_BillToSalonName.Text)
            Conn = " AND "
        End If
        If Not F_BillToName.Text = String.Empty Then
            SQL = SQL & Conn & "BillToName LIKE " & DB.FilterQuote(F_BillToName.Text)
            Conn = " AND "
        End If
        If Not F_BillToName2.Text = String.Empty Then
            SQL = SQL & Conn & "BillToName2 LIKE " & DB.FilterQuote(F_BillToName2.Text)
            Conn = " AND "
        End If
        If Not F_BillToZipcode.Text = String.Empty Then
            SQL = SQL & Conn & "BillToZipcode LIKE " & DB.FilterQuote(F_BillToZipcode.Text)
            Conn = " AND "
        End If
        If Not F_ShipToSalonName.Text = String.Empty Then
            SQL = SQL & Conn & "ShipToSalonName LIKE " & DB.FilterQuote(F_ShipToSalonName.Text)
            Conn = " AND "
        End If
        If Not F_ShipToName.Text = String.Empty Then
            SQL = SQL & Conn & "ShipToName LIKE " & DB.FilterQuote(F_ShipToName.Text)
            Conn = " AND "
        End If
        If Not F_ShipToName2.Text = String.Empty Then
            SQL = SQL & Conn & "ShipToName2 LIKE " & DB.FilterQuote(F_ShipToName2.Text)
            Conn = " AND "
        End If
        If Not F_ShipToZipCode.Text = String.Empty Then
            SQL = SQL & Conn & "ShipToZipcode LIKE " & DB.FilterQuote(F_ShipToZipCode.Text)
            Conn = " AND "
        End If
        If Not F_Email.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
            Conn = " AND "
        End If
        If Not F_OrderDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "ProcessDate >= " & DB.Quote(F_OrderDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_OrderDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "ProcessDate < " & DB.Quote(DateAdd("d", 1, F_OrderDateUbound.Text))
            Conn = " AND "
        End If
        If MemberId > 0 Then
            SQL = SQL & Conn & "MemberId=" & MemberId
            Conn = " AND "
        End If

        If F_OrderFrom.SelectedValue = "0" Then
            SQL = SQL & Conn & "LEFT(OrderNo,1)='W'"
        ElseIf F_OrderFrom.SelectedValue = "1" Then
            SQL = SQL & Conn & "LEFT(OrderNo,1)='E'"
            Conn = " AND "
        ElseIf F_OrderFrom.SelectedValue = "2" Then
            SQL = SQL & Conn & "LEFT(OrderNo,1)='A'"
            Conn = " AND "
        End If

        Dim arrCusNo As String()
        If Not F_CustomerNo.Text = String.Empty Then
            Try
                arrCusNo = F_CustomerNo.Text.Split(",")
                If arrCusNo.Length > 1 Then
                    Dim listCusNo As String = ""
                    For i As Integer = 0 To arrCusNo.Length - 1
                        listCusNo &= "'" & Trim(arrCusNo(i)) & "',"
                    Next
                    SQL = SQL & Conn & " SO.SellToCustomerId in(Select CustomerId From Customer Where CustomerNo in ( " & Left(listCusNo, listCusNo.Length - 1) & "))"
                Else
                    SQL = SQL & Conn & " SO.SellToCustomerId in (Select CustomerId From Customer Where CustomerNo LIKE " & DB.FilterQuote(F_CustomerNo.Text) & ")"
                End If
            Catch ex As Exception

            End Try

        End If
        If Not F_Total.Text = String.Empty Then
            SQL = SQL & Conn & "SO.Total = " & DB.Number(F_Total.Text)
            Conn = " AND "
        End If
        Dim SQLStatus As String = ""
        Dim isCheck As Boolean = False
        If chkgrey.Checked Then 'Order is pending export + Cart items are pending export
            SQLStatus = " SO.OrderId in (select distinct so.OrderId from StoreOrder so join StoreCartItem sci on so.OrderId = sci.OrderId where so.DoExport =1 and sci.DoExport = 1 and not exists(select * from ExportLog ex where ex.OrderId = so.OrderId))"
            isCheck = True
        End If
        If chkblue.Checked Then 'Order is pending export or order is downloaded  and Cart items are export or Cart items are download
            SQLStatus &= IIf(isCheck, " OR ", "") & "SO.OrderId in (select OrderId from ExportLog where [dbo].[fc_StoreOrder_CheckStatusReExport](OrderId) = 1 OR ((OrderStatus >=2 OR CartItemStatus >=2 ) AND OrderId not in (select e2.OrderId from ExportLog e2 where e2.OrderStatus >=2 AND e2.CartItemStatus  >=2)))"
            isCheck = True
        End If
        If chkred.Checked Then 'Order is export + Cart items are export
            SQLStatus &= IIf(isCheck, " OR ", "") & "SO.OrderId in (select OrderId from ExportLog where(OrderStatus >= 2 And CartItemStatus >= 2) AND OrderId not in (select e2.OrderId from ExportLog e2 where e2.OrderStatus =3 AND e2.CartItemStatus  =3) )"
            isCheck = True
        End If
        If chkblack.Checked Then 'Order is downloaded + Cart items are downloaded
            SQLStatus &= IIf(isCheck, " OR ", "") & "SO.OrderId in (select OrderId from ExportLog where OrderStatus =3 and CartItemStatus = 3 and [dbo].[fc_StoreOrder_CheckStatusReExport](OrderId) = 0) "
            isCheck = True
        End If
        If SQLStatus <> "" Then
            SQL = SQL & " AND (" & SQLStatus & ")"
        End If
        If Not F_PayPalAddress.SelectedValue = String.Empty Then
            'SQL = SQL & Conn & " SO.OrderId in (Select OrderId From StoreOrderPayflow Where " & IIf(F_PayPalAddress.SelectedValue = "1", " RespMsg = 'Unmatched'", " RespMsg <> 'Unmatched'") & ")"
            SQL = SQL & Conn & " SO.OrderId " & IIf(F_PayPalAddress.SelectedValue = "1", "", "Not") & " in (Select OrderId From StoreOrderPayflow Where  (RespMsg = 'Unmatched' OR RespMsg = 'Unmatch') AND PaypalShipToAddress IS NOT NULL AND PaypalShipToCity IS NOT NULL AND PaypalShipToCountry IS NOT NULL)"
            Conn = " AND "
        End If
        If Not F_PayPalType.SelectedValue = String.Empty Then
            'SQL = SQL & Conn & " PayPalStatus " & IIf(F_PayPalType.SelectedValue = "1", " = 4 and exists(Select top 1 OrderId From StoreOrderPayflow Where PnRef is null and Result is null)", " <> 4")
            SQL = SQL & Conn & " PayPalStatus " & IIf(F_PayPalType.SelectedValue = "1", " in(" & Utility.Common.PaymentStatus.eCheckCancelled & "," & Utility.Common.PaymentStatus.eCheckCompleted & "," & Utility.Common.PaymentStatus.eCheckPending & ")", " IN (" & Utility.Common.PaymentStatus.PaypalCancelled & "," & Utility.Common.PaymentStatus.PaypalCompleted & "," & Utility.Common.PaymentStatus.PaypalPending & ")")
            Conn = " AND "
        End If

        If Not F_PaymentType.SelectedValue = String.Empty Then
            SQL = SQL & Conn & " PaymentType " & IIf(F_PaymentType.SelectedValue = "1", " = 'PAYPAL'", " = 'CC'")
            Conn = " AND "
        End If
        If Not F_OrderDate.SelectedValue = String.Empty Then
            Dim interval As String = "day"
            Dim valDate As String = F_OrderDate.SelectedValue
            If F_OrderDate.SelectedValue = "month" Then
                ' interval = "month"
                valDate = 30
            End If
            SQL = SQL & Conn & " DATEDIFF(" & interval & ",processdate,getdate()) <= " & valDate
            Conn = " AND "
        End If
        hidCon.Value = SQL
        Return SQL
    End Function
   
    Private Sub Export()
        Dim SQLFields As String = "SELECT SO.OrderId, SO.OrderNo, BillToSalonName, BillToName, BillToName2, BillToCounty, BillToCountry, Email, ProcessDate, (SELECT TOP 1 SP.PnRef FROM StoreOrderPayflow SP WHERE SP.OrderId = SO.OrderId ORDER BY SP.PayflowId DESC) AS TransactionID "
        Dim SQL As String = BuildQuery()
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & ViewState("F_SortBy") & " " & ViewState("F_SortOrder"))
        Dim Folder As String = "/assets/temp/"
        Dim i As Integer = 0
        Dim FileName As String = Core.GenerateFileID & ".csv"
        Dim sw As StreamWriter = New StreamWriter(Server.MapPath(Folder & FileName), False)
        sw.WriteLine("Web Orders Report")
        sw.WriteLine("Report generated on " & DateTime.Now.ToString("d"))
        sw.WriteLine(String.Empty)
        sw.WriteLine("Search Criteria")
        sw.WriteLine("Order No:," & F_OrderNo.Text)
        sw.WriteLine("Bill To Salon:," & F_BillToSalonName.Text)
        sw.WriteLine("Bill To Name:," & F_BillToName.Text)
        sw.WriteLine("Bill To Name 2:," & F_BillToName2.Text)
        sw.WriteLine("Bill To State:," & F_BillToCounty.Text)
        sw.WriteLine("Bill To Zip:," & F_BillToZipcode.Text)
        sw.WriteLine("Bill To Country:," & F_BillToCountry.Text)
        sw.WriteLine("Order From Date:," & F_OrderDateLbound.Value)
        sw.WriteLine("Order To Date:," & F_OrderDateUbound.Value)
        sw.WriteLine(String.Empty)
        sw.WriteLine(String.Empty)
        sw.WriteLine("Order No,Bill To Salon Name,Bill To Name,Bill To Name 2,Bill To State,Bill To Country,Email,Order Date")
        For Each dr As DataRow In res.Rows
            sw.WriteLine(Core.QuoteCSV(Convert.ToString(dr("OrderNo"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToSalonName"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToName"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToName2"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToCounty"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToCountry"))) & "," & Core.QuoteCSV(Convert.ToString(dr("Email"))) & "," & Core.QuoteCSV(Convert.ToString(dr("processdate"))))
        Next
        sw.Flush()
        sw.Close()
        lnkDownload.NavigateUrl = Folder & FileName
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        divDownload.Visible = False
        gvList.Visible = True
        BuildQuery()
        BindList()
    End Sub

    Protected Function ShowTransaction(ByVal orderNo As String, ByVal ebayOrderId As String, ByVal AmazonOrderId As String, ByVal TransactionID As Object, ByVal PaypalStatus As Object, ByVal PaypalShipToAddress As String) As String
        Dim type As String = ""
        Dim str As String = String.Empty
        Try
            type = orderNo.Substring(0, 1)
        Catch ex As Exception

        End Try
        If (type = "E") Then
            Return ebayOrderId & "<br/><img src='/includes/theme-admin/images/ebayLink.png' alt=''/>"
        ElseIf (type = "A") Then
            Return AmazonOrderId & "<br/><img src='/includes/theme-admin/images/amazonLink.png' alt=''/>"
        Else

            If Not TransactionID Is DBNull.Value AndAlso TransactionID.ToString().Length > 0 Then
                str = TransactionID.ToString()
            Else
                If Not PaypalStatus Is DBNull.Value Then
                    Dim bCheck As Boolean = Utility.ConfigData.CheckPaypalCompleted()
                    Select Case CInt(PaypalStatus)
                        Case PaymentStatus.PaypalPending
                            str = "<label class=""red"">Paypal Pending</label>"
                        Case PaymentStatus.PaypalCompleted
                            str = "Paypal Completed"
                            bCheck = False
                        Case PaymentStatus.eCheckPending
                            str = "<label class=""red"">eCheck Pending</label>"
                        Case PaymentStatus.eCheckCompleted
                            str = "eCheck Completed"
                            bCheck = False
                        Case PaymentStatus.eCheckCancelled
                            str = "<label class=""red"">eCheck Cancelled</label>"
                            bCheck = False
                        Case PaymentStatus.PaypalCancelled
                            str = "<label class=""red"">Paypal Cancelled</label>"
                            bCheck = False
                        Case Else
                            str = ""
                    End Select

                    If (Not bCheck And Not String.IsNullOrEmpty(PaypalShipToAddress)) Then
                        str += GetToolTipUnmatch(orderNo)
                    End If

                End If
            End If
            Return str
        End If
    End Function

    Protected i As Integer = 1

    Protected Function GetToolTipUnmatch(ByVal orderNo As String) As String
        Dim html As String = String.Empty
        Dim ShipToAddress As String = String.Empty
        Dim ShipToCountry As String = String.Empty
        Dim ShipToCity As String = String.Empty
        Dim ShipToZipcode As String = String.Empty
        Dim ShipToCounty As String = String.Empty

        Dim PaypalShipToAddress As String = String.Empty
        Dim PaypalShipToCountry As String = String.Empty
        Dim PaypalShipToCity As String = String.Empty
        Dim PaypalShipToZipcode As String = String.Empty
        Dim PaypalShipToCounty As String = String.Empty
        Dim PaypalShipToStatus As String = String.Empty
        Try
            Dim dt As DataTable = StoreOrderPayflowRow.GetPaypalAddressUnmatch(orderNo)
            If (Not dt Is Nothing AndAlso dt.Rows.Count > 0) Then

                If dt.Rows(0)("ShipToAddress") IsNot Nothing Then
                    ShipToAddress = dt.Rows(0)("ShipToAddress").ToString().Trim().ToUpper()
                End If

                'If o.ShipToAddress2 IsNot Nothing Then
                '    ShipToAddress &= " " & dt.Rows(0)("ShipToAddress2").ToString().Trim().ToUpper()
                'End If

                If dt.Rows(0)("ShipToCountry") IsNot Nothing Then
                    ShipToCountry = dt.Rows(0)("ShipToCountry").ToString().Trim().ToUpper()
                End If

                If dt.Rows(0)("ShipToCity") IsNot Nothing Then
                    ShipToCity = dt.Rows(0)("ShipToCity").ToString().Trim().ToUpper()
                End If

                If dt.Rows(0)("ShipToZipcode") IsNot Nothing Then
                    ShipToZipcode = dt.Rows(0)("ShipToZipcode").ToString().Trim().ToUpper().Replace("-", "")
                End If

                If dt.Rows(0)("ShipToCounty") IsNot Nothing Then
                    ShipToCounty = dt.Rows(0)("ShipToCounty").ToString().Trim().ToUpper()
                End If

                PaypalShipToAddress = dt.Rows(0)("PaypalShipToAddress").ToString()
                PaypalShipToCountry = dt.Rows(0)("PaypalShipToCountry").ToString()
                PaypalShipToCity = dt.Rows(0)("PaypalShipToCity").ToString()
                PaypalShipToZipcode = dt.Rows(0)("PaypalShipToZipcode").ToString()
                PaypalShipToCounty = dt.Rows(0)("PaypalShipToCounty").ToString()
                PaypalShipToStatus = dt.Rows(0)("PaypalShipToStatus").ToString()

                Dim FullPath As String = Server.MapPath("~/includes/MailTemplate/PayPalAddressUnmatch.htm")
                Dim objReader As New StreamReader(FullPath)
                Dim strBody As String = objReader.ReadToEnd()
                objReader.Close()

                strBody = String.Format(strBody, ShipToAddress, PaypalShipToAddress, ShipToCity, PaypalShipToCity, ShipToCounty, PaypalShipToCounty, ShipToZipcode, PaypalShipToZipcode, ShipToCountry, PaypalShipToCountry, PaypalShipToStatus)
                'strBody = String.Format(strBody, ShipToAddress, PaypalShipToAddress, ShipToCity, PaypalShipToCity, ShipToCounty, PaypalShipToCounty, ShipToZipcode, PaypalShipToZipcode, ShipToCountry, PaypalShipToCountry, PaypalShipToStatus, bCity.ToString(), bState.ToString(), bZip.ToString(), bCountry.ToString())

                html = "<br/><label class=""red"" id='tip" & i & "' paramTitle='" & orderNo & "' paramDes=""" & strBody.Replace("""", "'") & """><u>Unmatch</u></label>"
                i = i + 1

            End If

        Catch ex As Exception

        End Try
        Return html
    End Function

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim o As StoreOrderRow = e.Row.DataItem
            ''gan link member detail
            Dim lnkMember As Literal = CType(e.Row.FindControl("lnkMember"), Literal)
            Dim lbIPAddress As Label = CType(e.Row.FindControl("lbIPAddress"), Label)
            Try
                If Left(o.OrderNo, 1) = "E" Then
                    lbIPAddress.Text = ""
                Else
                    lbIPAddress.Text = o.RemoteIP
                End If
            Catch

            End Try
            Dim name As String = String.Empty
            name = o.BillToName
            If (String.IsNullOrEmpty(name)) Then
                lnkMember.Text = String.Empty
            Else
                lnkMember.Text = "<a target='_blank' href='/admin/members/edit.aspx?MemberId=" & o.MemberId & "&F_SortBy=CreateDate&F_SortOrder=desc'>" & name & "</a>"
            End If

            Dim ltrEmail As Literal = CType(e.Row.FindControl("ltrEmail"), Literal)
            Dim i As Integer = o.Email.IndexOf("marketplace.amazon.com")
            ltrEmail.Text = o.Email
            If i > 0 Then
                ltrEmail.Text = o.Email.Substring(0, i) & "..."
            End If

            Dim ex As ExportLogRow = ExportLogRow.GetRow(DB, o.OrderId)
            Dim imExportStatus As System.Web.UI.WebControls.Image = CType(e.Row.FindControl("imExportStatus"), System.Web.UI.WebControls.Image)
            ''check Order co export lai hay khong
            Dim bReExport As Boolean = StoreOrderRow.CheckStatusReExport(DB, o.OrderId)
            ''tootip status cho order
            Dim tip1 As String = String.Empty
            If bReExport Then
                tip1 = "Order is pending re-export"
            ElseIf ex.OrderStatus = 3 Then
                tip1 = "Order is downloaded"
            ElseIf ex.OrderStatus = 2 Then
                tip1 = "Order is exported"
            Else
                tip1 = "Order is pending export"
            End If

            ''tooltip status cho Cartitem
            Dim tip2 As String = String.Empty
            If bReExport Then
                tip2 = "Cart items are pending re-export"
            ElseIf ex.CartItemStatus = 3 Then
                tip2 = "Cart items are downloaded"
            ElseIf ex.CartItemStatus = 2 Then
                tip2 = "Cart items are exported"
            Else
                tip2 = "Cart items are pending export"
            End If
            imExportStatus.ToolTip = tip1 & Environment.NewLine & tip2

            ''Status cua Order va Cart Item = 3 image hien thi download
            If (ex.OrderStatus = 3 And ex.CartItemStatus = 3 And Not bReExport) Then
                imExportStatus.ImageUrl = "/includes/theme-admin/images/downloaded.png"

            ElseIf (ex.OrderStatus >= 2 And ex.CartItemStatus >= 2 And Not bReExport) Then    ''Status cua Order va Cart Item = 2 hay co 1 status = 3 se hien thi image export
                imExportStatus.ImageUrl = "/includes/theme-admin/images/exported.png"

            ElseIf (ex.OrderStatus >= 2 Or ex.CartItemStatus >= 2 Or bReExport) Then     ''Order hay Cart Item co mot status = 2 hay 3, nhung status con lai = 0 hay null se cho hien thi image chua export
                imExportStatus.ImageUrl = "/includes/theme-admin/images/reexport.png"

            Else
                Dim PaypalStatus As Integer = o.PaypalStatus
                If PaypalStatus <> 1 AndAlso PaypalStatus <> 3 Then
                    ''khong co item trong ExportLog se chi check 2 truong hop trong DB la chua export ca Order va CartItem hoac da download ca 2
                    If (StoreOrderRow.CheckExportByOrderId(DB, o.OrderId)) Then
                        imExportStatus.ImageUrl = "/includes/theme-admin/images/not_export.png"
                        imExportStatus.ToolTip = "Order is pending export" & Environment.NewLine & "Cart items are pending export"
                    Else
                        imExportStatus.ImageUrl = "/includes/theme-admin/images/downloaded.png"
                        imExportStatus.ToolTip = "Order is downloaded" & Environment.NewLine & "Cart items are downloaded"
                    End If
                Else
                    imExportStatus.ImageUrl = "/includes/theme-admin/images/not_export.png"
                    imExportStatus.ToolTip = "Order is pending export" & Environment.NewLine & "Cart items are pending export"
                End If

            End If


        End If
    End Sub

    Protected Function ShowShipVia(ByVal OrderId As Integer) As String
        Dim html As String = String.Empty
        Dim linkTracking As String = String.Empty
        Dim dt As DataTable = DB.GetDataTable("select TrackingId,trackingno,coalesce(ShipmentType,0) as ShipmentType,coalesce(Note,'') as Note from StoreOrderShipmentTracking where shipmentid in (select shipmentid from storeordershipment where orderid = " & OrderId & ")")
        If dt.Rows.Count = 0 Then
            dt = DB.GetDataTable("select trackingid,trackingno,coalesce(ShipmentType,0) as ShipmentType,coalesce(Note,'') as Note from StoreOrderShipmentTracking where  orderid = " & OrderId)
        End If
        Dim CarrierType As DataTable = DB.GetDataTable("select distinct carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & OrderId & " and  type = 'item'")

        For i = 0 To CarrierType.Rows.Count - 1
            Dim result As String = String.Empty
            Dim smt As ShipmentMethodRow
            If (Not CarrierType.Rows(i)("CarrierType") Is Nothing) AndAlso CarrierType.Rows(i)("CarrierType") > 0 Then
                smt = ShipmentMethodRow.GetRow(DB, CarrierType.Rows(i)("CarrierType"))
            Else
                Exit For
            End If

            Dim TrackingId As Integer = 0
            Dim ShipmentType As Integer = 0
            Try
                If dt.Rows.Count > 0 Then
                    result = Trim(dt.Rows(0)("TrackingNo").ToString())
                    TrackingId = CInt(dt.Rows(0)("TrackingId"))
                    ShipmentType = CInt(dt.Rows(0)("ShipmentType"))
                End If
                If ShipmentType = 5 Then 'DHL
                    smt.Name = "DHL"
                End If
                If result = "" Then
                    If LCase(Left(smt.Code, 3)) = "ups" Then
                        html &= "<img src='/includes/theme-admin/images/global/ico-ups-off.png' title='" & smt.Name & "' alt=''/> "
                    ElseIf LCase(Left(smt.Code, 3)) = "fed" AndAlso Convert.ToInt32(CarrierType.Rows(i)("CarrierType").ToString()) = 16 Then
                        html &= "<img src='/includes/theme-admin/images/global/ico-groundshipping-off.png' title='" & smt.Name & "' alt=''/> "
                    ElseIf LCase(Left(smt.Code, 3)) = "fed" Then
                        html &= "<img src='/includes/theme-admin/images/global/ico-fedex-off.png' title='" & smt.Name & "' alt=''/> "
                    ElseIf LCase(Left(smt.Code, 3)) = "int" Then
                        html &= "<img src='/includes/theme-admin/images/global/ico_international-off.png' title='" & smt.Name & "' alt=''/> "
                    ElseIf LCase(Left(smt.Code, 3)) = "tru" Then
                        html &= "<img src='/includes/theme-admin/images/global/icon-freight.png' title='" & smt.Name & "' alt=''/> "
                    ElseIf LCase(Left(smt.Code, 3)) = "pic" Then
                        html = "Pickup"
                    End If
                Else
                    If LCase(Left(smt.Code, 3)) = "ups" Or LCase(Left(smt.Code, 3)) = "fed" Then
                        If (ShipmentType = Utility.Common.StandardShippingMethod.Truck) AndAlso CarrierType.Rows.Count = 1 Then
                            linkTracking &= "{0}"
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.USPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPS
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.FedEx) Then
                            linkTracking = Resources.Msg.LinkTrackingNailSuperStore
                        ElseIf ShipmentType = Utility.Common.StandardShippingMethod.DHL Then
                            linkTracking = Resources.Msg.LinkTrackingNumberDHL
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.UPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUPS
                        End If
                        linkTracking = String.Format(linkTracking, result)
                        If linkTracking <> "" Then ''gan link
                            If LCase(Left(smt.Code, 3)) = "ups" Then
                                html &= "<a href='" & linkTracking & "' target='_blank' style=' color:#0000ed'><img src='/includes/theme-admin/images/global/ico-ups.png' title='" & smt.Name & "' alt=''/> </a>"
                            ElseIf LCase(Left(smt.Code, 3)) = "fed" AndAlso Convert.ToInt32(CarrierType.Rows(i)("CarrierType").ToString()) = 16 Then
                                html &= "<a href='" & linkTracking & "' target='_blank' style=' color:#0000ed'><img src='/includes/theme-admin/images/global/ico-groundshipping.png' title='" & smt.Name & "' alt=''/> </a>"
                            Else
                                html &= "<a href='" & linkTracking & "' target='_blank' style=' color:#0000ed'><img src='/includes/theme-admin/images/global/ico-fedex.png' title='" & smt.Name & "' alt=''/> </a>"
                            End If
                        Else ''ko link
                            If LCase(Left(smt.Code, 3)) = "ups" Then
                                html &= "<img src='/includes/theme-admin/images/global/ico-ups.png' title='" & smt.Name & "' alt=''/> "
                            ElseIf LCase(Left(smt.Code, 3)) = "fed" AndAlso Convert.ToInt32(CarrierType.Rows(i)("CarrierType").ToString()) = 16 Then
                                html &= "<img src='/includes/theme-admin/images/global/ico-groundshipping.png' title='" & smt.Name & "' alt=''/> "
                            Else
                                html &= "<img src='/includes/theme-admin/images/global/ico-fedex.png' title='" & smt.Name & "' alt=''/> "
                            End If
                        End If
                    ElseIf Convert.ToInt32(CarrierType.Rows(i)("CarrierType").ToString()) = 15 Then
                        If (ShipmentType = Utility.Common.StandardShippingMethod.Truck) AndAlso CarrierType.Rows.Count = 1 Then
                            linkTracking &= "{0}"
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.USPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPS
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.FedEx) Then
                            linkTracking = Resources.Msg.LinkTrackingNailSuperStore
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.UPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUPS
                        ElseIf ShipmentType = Utility.Common.StandardShippingMethod.DHL Then
                            linkTracking = Resources.Msg.LinkTrackingNumberDHL
                        ElseIf CarrierType.Rows.Count = 1 Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPSPriority
                        End If
                        linkTracking = String.Format(linkTracking, result)
                        If linkTracking <> "" Then ''gan link
                            html &= "<a href='" & linkTracking & "' target='_blank' style=' color:#0000ed'><img src='/includes/theme-admin/images/global/ico_international.png' title='" & smt.Name & "' alt=''/> </a>"
                        Else
                            html &= "<img src='/includes/theme-admin/images/global/ico_international.png' title='" & smt.Name & "' alt=''/> "
                        End If
                    ElseIf Convert.ToInt32(CarrierType.Rows(i)("CarrierType").ToString()) = Utility.Common.StandardShippingMethod.Truck AndAlso ShipmentType = Utility.Common.StandardShippingMethod.Truck Then
                        html &= "<a target='_blank' style='color:#0000ed'  href='" & result & "'><img src='/includes/theme-admin/images/global/icon-freight.png' title='" & smt.Name & "' alt=''/> </a>"
                    Else
                        html &= "<img src='/includes/theme-admin/images/global/icon-freight.png' title='" & smt.Name & "' alt=''/> "
                    End If
                    End If
            Catch ex As Exception

            End Try
        Next
        Return html
    End Function
End Class

