Imports Components
Imports DataLayer
Partial Class admin_store_orders_AddTrackingNumber
    Inherits AdminPage
    Protected OrderId As Integer
    Protected TrackingId As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        OrderId = IIf(Request("OrderId") <> Nothing, Request("OrderId"), 0)
        If Request("TrackingId") <> Nothing Then
            TrackingId = Request("TrackingId")
        End If
        If Not Page.IsPostBack Then
            Utility.Common.BindStandardShippingMethod(drlShippingDetail, False)
            LoadData()

        End If
    End Sub
    Private Sub LoadData()
        Dim Tracking As StoreOrderShipmentTrackingRow = StoreOrderShipmentTrackingRow.GetRow(DB, TrackingId)
        Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
        Dim countTruckOutsiteUS As Integer = 0
        Dim IsShippingSpecialUS As Boolean = Utility.Common.CheckShippingSpecialUS(o)
        If o.ShipToCountry <> "US" Or IsShippingSpecialUS Then
            countTruckOutsiteUS = DB.ExecuteScalar("Select count(CartItemId) from StoreCartItem where orderid = " & OrderId & " and  type = 'item' and CarrierType = " & Utility.Common.StandardShippingMethod.Truck)
        End If
        lblOrderNo.Text = o.OrderNo
        Dim methodid As Integer = 0
        '' Dim dtShipping As DataTable = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (Select CarrierType from StoreOrder where OrderId = " & OrderId & ")")
        Dim dtShipping As DataTable = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & OrderId & " and  type = 'item' and LEFT(Code,3)='UPS')")
        If Not (dtShipping Is Nothing) Then
            Dim Name As String = String.Empty
            If dtShipping.Rows.Count < 1 Then
                ''dtShipping = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & OrderId & " and  type = 'item')")
                If countTruckOutsiteUS > 0 Then
                    dtShipping = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId = " & Utility.Common.StandardShippingMethod.Truck)
                Else
                    dtShipping = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & OrderId & " and  type = 'item')")
                End If

            End If
            methodid = dtShipping.Rows(0)("MethodId")
            Name = dtShipping.Rows(0)("Name")
            Dim ShippingMethod As Integer = 0
            If Tracking.ShipmentType > 0 Then
                ShippingMethod = Tracking.ShipmentType
            ElseIf methodid > 0 Then
                Dim sm As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, methodid)
                If Not sm Is Nothing AndAlso Not String.IsNullOrEmpty(sm.Code) Then
                    If LCase(Left(sm.Code, 3)).Equals("int") Then
                        ShippingMethod = Utility.Common.StandardShippingMethod.USPS
                    ElseIf LCase(Left(sm.Code, 3)).Equals("fed") Then
                        ShippingMethod = Utility.Common.StandardShippingMethod.FedEx
                    ElseIf LCase(Left(sm.Code, 3)).Equals("tru") Then
                        ShippingMethod = Utility.Common.StandardShippingMethod.Truck
                    ElseIf LCase(Left(sm.Code, 3)).Equals("ups") Then
                        ShippingMethod = Utility.Common.StandardShippingMethod.UPS
                    End If
                End If
            End If

            trShippingDetail.Visible = True
            drlShippingDetail.SelectedValue = ShippingMethod
            hdDefaultvalue.Value = ShippingMethod
            If (Tracking.ShipmentType = Utility.Common.StandardShippingMethod.Truck) Then
                lblNote.Text = "Label"
                lblTrackingNumber.Text = "Link"
            End If


            lblShippingType.Text = Name
            txtNote.Text = Tracking.Note

        End If

        F_TrackingNumber.Text = Tracking.TrackingNo
        ' If (methodid = Utility.Common.StandardShippingMethod.Truck) Then
        'lblNote.Text = "Label"
        'lblTrackingNumber.Text = "Link"
        'drlShippingDetail.SelectedValue = Utility.Common.StandardShippingMethod.Truck
        'Dim count As Integer = DB.ExecuteScalar("Select count(CartItemId) from StoreCartItem where orderid = " & OrderId & " and  type = 'item' and CarrierType <> " & Utility.Common.StandardShippingMethod.Truck)
        'If (count < 1 Or o.ShipToCountry <> "US" Or IsShippingSpecialUS) Then
        '    drlShippingDetail.Enabled = False
        'End If
        'End If
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim Tracking As StoreOrderShipmentTrackingRow
        Try
            If (drlShippingDetail.SelectedValue <> Utility.Common.StandardShippingMethod.Truck) Then
                Dim cTrackNumber As Int32 = DB.ExecuteScalar("Select count(isnull(TrackingId,0)) from StoreOrderShipmentTracking where TrackingNo = '" & F_TrackingNumber.Text & "' and TrackingId<>" & TrackingId)
                If cTrackNumber > 0 Then
                    lbMsg.Text = "This Tracking number is already used"
                    Exit Sub
                End If
            End If
          
            Dim ShipId As Integer = DB.ExecuteScalar("select isnull(shipmentid,0) from storeordershipment where orderid = " & OrderId)
            Tracking = New StoreOrderShipmentTrackingRow(DB)
            Tracking.TrackingNo = F_TrackingNumber.Text

            If (drlShippingDetail.SelectedValue <> Nothing) Then
                Tracking.ShipmentType = drlShippingDetail.SelectedValue
                If (Tracking.ShipmentType = Utility.Common.StandardShippingMethod.Truck) Then
                    If (String.IsNullOrEmpty(txtNote.Text)) Then
                        lbMsg.Text = "Label is required"
                        lblNote.Text = "Label"
                        Exit Sub
                    End If
                End If
            Else
                Tracking.ShipmentType = Nothing
            End If


            Tracking.Note = txtNote.Text.Trim()
            Tracking.CreatedDate = DateTime.Now
            Tracking.ModifiedDate = DateTime.Now
            If ShipId <> 0 Then
                Tracking.ShipmentId = ShipId
            End If
            Tracking.OrderId = OrderId
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = TrackingId
            logDetail.ObjectType = Utility.Common.ObjectType.TrackingNumber.ToString()
            ''logSubject = "Insert"
            Dim changeLog As String = String.Empty
            If TrackingId = 0 Then
                Tracking.Insert()
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(Tracking, Utility.Common.ObjectType.TrackingNumber)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logDetail.Message = logDetail.Message & "ShipVia" & AdminLogHelper.AdminLogSeparateCharNew & lblShippingType.Text & "[br]"
            Else
                Tracking.TrackingId = TrackingId
                Dim TrackingBeforeUpdate As StoreOrderShipmentTrackingRow = StoreOrderShipmentTrackingRow.GetRow(DB, TrackingId)
                changeLog = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.TrackingNumber, TrackingBeforeUpdate, Tracking)
                If Not changeLog.Contains("ShipmentType") Then
                    changeLog = changeLog & "ShipmentType" & AdminLogHelper.AdminLogSeparateCharNew & Tracking.ShipmentType & "[br]"
                End If
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                Tracking.Update()
            End If
            If chkSendTracking.Checked Then
                logDetail.Message = logDetail.Message & "EmailToCustomer" & AdminLogHelper.AdminLogSeparateCharNew & "True[br]"
                SendTrackingToCustomer(OrderId, Tracking.TrackingNo)
            Else
                logDetail.Message = logDetail.Message & "EmailToCustomer" & AdminLogHelper.AdminLogSeparateCharNew & "False[br]"
            End If


            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("/admin/store/orders/edit.aspx?OrderId=" & OrderId)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub SendTrackingToCustomer(ByVal OrderId As Integer, ByVal TrackingNo As String)

        'OrderId = 111274
        'TrackingNo = "578916760720"
        Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
        Dim URL As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/store/send-tracking.aspx?orderid=" & HttpUtility.UrlEncode(CryptData.Crypt.EncryptTripleDes(OrderId)) & "&trackingid=" & HttpUtility.UrlEncode(CryptData.Crypt.EncryptTripleDes(TrackingNo))
        Dim r As System.Net.HttpWebRequest = System.Net.WebRequest.Create(URL)
        Dim myCache As New System.Net.CredentialCache()
        myCache.Add(New Uri(URL), "Basic", New System.Net.NetworkCredential("ameagle", "design"))
        r.Credentials = myCache
        Try

            'Get the data as an HttpWebResponse object
            Dim resp As System.Net.HttpWebResponse = r.GetResponse()
            Dim sr As New System.IO.StreamReader(resp.GetResponseStream())
            Dim HTML As String = sr.ReadToEnd()
            sr.Close()
            HTML = Replace(HTML, "href=""/", "href=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/")
            HTML = Replace(HTML, "src=""/", "src=""" & System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName") & "/")
            Dim bccConfirm As String = SysParam.GetValue("ToReportTrackingNumber")
            If (Email.SendHTMLMail(FromEmailType.Sales, dbOrder.Email, dbOrder.BillToName & " " & dbOrder.BillToName2, Utility.ConfigData.SubjectSendTracking, HTML, bccConfirm)) Then
                DB.ExecuteSQL("Update StoreOrderShipmentTracking Set IsSend = 1 Where OrderId = " & OrderId)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
