Imports System.Configuration.ConfigurationManager
Imports Components
Imports Utility
Imports System.Data
Imports System.Globalization

Partial Class controls_GoogleTracking
    Inherits BaseControl

    Public GoogleTrackingNo As String = String.Empty
    Public ListOrderId As String = String.Empty

    Private OrderId As Integer = 0
    Private ListFinalOrderId As String = String.Empty
    Private NumberOrder As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If DataLayer.SysParam.GetValue("IsGoogleAnalyticsEcommerceTracking") = "1" Then
                If InStr(Request.RawUrl.ToLower, "/store/confirmation.aspx") > 0 Then
                    OrderId = GetOrderId()
                    GenerateOrderDetails()
                    GetVariableForGTS()
                ElseIf InStr(Request.Url.ToString().ToLower, "/store/ga.aspx") > 0 Then
                    If ListOrderId.Length > 0 Then
                        GenerateOrderDetails()
                        GetVariableForGTS()
                    End If
                End If
            End If

            If Not String.IsNullOrEmpty(litGoogleOrderTracking.Text) And (Not String.IsNullOrEmpty(ListOrderId) AndAlso NumberOrder > 0) Then
                ListFinalOrderId = ListFinalOrderId.Substring(1)

                If InStr(Request.Url.ToString().ToLower, "/store/ga.aspx") > 0 Then
                    litlGooglePageView.Text = "ga('send', 'pageview', { 'hitCallback': function() { submitGA('" + ListFinalOrderId + "'); } });"
                Else
                    litlGooglePageView.Text = "ga('send', 'pageview', { 'hitCallback': function() { mainScreen.ExecuteCommand('SubmitGA', '', [" & ListFinalOrderId & "]); } });"
                End If
            Else
                litlGooglePageView.Text = "ga('send', 'pageview');"
            End If
        End If
    End Sub
    Private Function GetOrderId() As Integer

        If Request("OrderId") IsNot Nothing Then
            If IsNumeric(Request("OrderId")) Then
                OrderId = CInt(Request("OrderId"))
            Else
                Try
                    OrderId = Utility.Crypt.DecryptTripleDes(Request("OrderId"))
                Catch ex As Exception
                    OrderId = CInt(Request("OrderId"))
                End Try
            End If
        End If
        Return OrderId
    End Function

    Private Sub GenerateOrderDetails()
        If InStr(Request.RawUrl.ToLower, "/store/confirmation.aspx") > 0 Then ' AndAlso so.PaymentType = "PAYPAL" Then
            Exit Sub
        End If

        Dim sb As New StringBuilder
        If OrderId < 1 And String.IsNullOrEmpty(ListOrderId) Then
            Exit Sub
        Else
            If String.IsNullOrEmpty(ListOrderId) Then
                ListOrderId = OrderId.ToString()
            End If
        End If

        pnScript.Visible = False
        sb.AppendLine("ga('require', 'ecommerce', 'ecommerce.js');")
        For Each s As String In ListOrderId.Split(",")
            Dim iOrderId As Integer = CInt(s)
            Try
                Dim so As DataLayer.StoreOrderRow = DataLayer.StoreOrderRow.GetRow(DB, iOrderId)


                If so Is Nothing Then
                    Email.SendError("ToError500", "GoogleAnalytics > StoreOrderRow Is Nothing", "Url: " & Request.Url.ToString())
                    Continue For
                ElseIf so IsNot Nothing AndAlso so.OrderNo Is Nothing Then
                    If so.PaymentType <> "PAYPAL" Then
                        Email.SendError("ToError500", "GoogleAnalytics > OrderNo Is Nothing", "Url: " & Request.Url.ToString())
                    End If
                    Continue For
                ElseIf so IsNot Nothing AndAlso so.IsSubmitGA Then
                    Email.SendError("ToError500", "GoogleAnalytics > IsSubmitGA = TRUE", "Url: " & Request.Url.ToString())
                    Continue For
                Else
                    ListFinalOrderId &= "," & iOrderId.ToString()
                    NumberOrder += 1
                End If


                sb.AppendLine(" ga('ecommerce:addTransaction', {")
                sb.AppendLine(" 'id': '" & so.OrderNo & "',") 'Transaction ID. Required.

                Dim billToName As String = String.Empty, billToName2 As String = String.Empty, shipToName As String = String.Empty, shipToName2 As String = String.Empty
                If Not String.IsNullOrEmpty(so.BillToName) Then
                    billToName = so.BillToName
                End If

                If Not String.IsNullOrEmpty(so.BillToName2) Then
                    billToName2 = so.BillToName2
                End If

                If Not String.IsNullOrEmpty(so.ShipToName) Then
                    shipToName = so.ShipToName
                End If

                If Not String.IsNullOrEmpty(so.ShipToName2) Then
                    shipToName2 = so.ShipToName2
                End If
                If String.IsNullOrEmpty(so.ShipToName) Then
                    sb.AppendLine(" 'affiliation': '" & billToName.Replace("'", "") & IIf(String.IsNullOrEmpty(billToName2), "", " " & billToName2.Replace("'", "")) & "',") 'Affiliation or store name.
                Else
                    sb.AppendLine(" 'affiliation': '" & shipToName.Replace("'", "") & IIf(String.IsNullOrEmpty(shipToName2), "", " " & shipToName2.Replace("'", "")) & "',") 'Affiliation or store name.
                End If

                sb.AppendLine(" 'revenue': '" & so.Total & "',") 'Grand Total.
                sb.AppendLine(" 'shipping': '" & (so.Shipping + so.TotalSpecialHandlingFee) & "',") 'Shipping.
                sb.AppendLine(" 'tax': '" & so.Tax & "'") 'Tax
                sb.AppendLine(" });")

                Dim ca As DataLayer.StoreCartItemCollection = DataLayer.StoreCartItemRow.GetCartItems(DB, so.OrderId)
                For Each c As DataLayer.StoreCartItemRow In ca
                    If c.Type = "item" Then
                        sb.AppendLine("ga('ecommerce:addItem', {")
                        sb.AppendLine(" 'id': '" & so.OrderNo & "',") 'Transaction ID. Required.
                        Dim strName As String = c.ItemName.Replace("'", "\'")
                        If strName.Contains("|") Then
                            strName = strName.Substring(0, strName.LastIndexOf("|"))
                        End If
                        sb.AppendLine(" 'name': '" & strName.Trim() & "',") 'Product name. Required.
                        sb.AppendLine(" 'sku': '" & c.SKU & "',") 'SKU/code.

                        If c.DepartmentId > 0 AndAlso c.DepartmentId <> 23 Then
                            Try
                                sb.AppendLine(" 'category': '" & DataLayer.StoreDepartmentRow.GetRowById(DB, c.DepartmentId).Name.Replace("'", "\'") & "',") 'Category or variation.
                            Catch ex As Exception
                                sb.AppendLine(" 'category': 'Unknown',") 'Category or variation.
                            End Try
                        Else
                            sb.AppendLine(" 'category': '',") 'Category or variation.
                        End If

                        If c.IsFreeGift Or c.IsFreeSample Then 'Unit price.
                            sb.AppendLine(" 'price': '0',")
                        Else
                            sb.AppendLine(" 'price': '" & IIf(c.CustomerPrice <= 0, c.Price, c.CustomerPrice) & "',") 'Unit price.
                        End If
                        sb.AppendLine(" 'quantity': '" & c.Quantity & "'") 'Quantity.
                        sb.AppendLine(" });")
                    End If
                Next

            Catch ex As Exception
                Email.SendError("ToError500", "GoogleAnalytics", "Url: " & Request.RawUrl & "<br>ListOrderId: " & ListOrderId & "<br>Exception: " & ex.ToString() & "<br>" & sb.ToString())
            End Try
        Next

        sb.AppendLine(" ga('ecommerce:send');")
        litGoogleOrderTracking.Text = sb.ToString()
    End Sub


    Private Sub GetVariableForGTS()
        Dim sb As New StringBuilder
        If OrderId < 1 Then
            Exit Sub
        End If

        sb.AppendLine("<div id=""gts-order"" style=""display:none;"" translate=""no"">")
        Try
            Dim so As DataLayer.StoreOrderRow = DataLayer.StoreOrderRow.GetRow(DB, OrderId)
            If so Is Nothing Then
                Email.SendError("ToError500", "GoogleTrustedStore > StoreOrderRow Is Nothing", "Url: " & Request.Url.ToString())
                Exit Sub
            End If

            'Google trusted store script
            sb.AppendLine("<span id='gts-o-id'> " & so.OrderNo & "</span>")
            sb.AppendLine("<span id='gts-o-email'>" & so.Email & "</span>")
            sb.AppendLine("<span id='gts-o-country'>" & so.ShipToCountry & "</span>")
            sb.AppendLine("<span id='gts-o-currency'> USD </span>")
            sb.AppendLine("<span id='gts-o-total'>" & so.Total & "</span>")
            sb.AppendLine("<span id='gts-o-discounts'>" & so.TotalDiscount & "</span>")
            sb.AppendLine("<span id='gts-o-shipping-total'>" & (so.Shipping + so.TotalSpecialHandlingFee) & "</span>")
            sb.AppendLine("<span id='gts-o-tax-total'>" & so.Tax & "</span>")
            sb.AppendLine("<span id='gts-o-est-ship-date'>" & so.ProcessDate.AddDays(2).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "</span>")
            sb.AppendLine("<span id='gts-o-est-delivery-date'>" & so.ProcessDate.AddDays(14).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "</span>")
            sb.AppendLine("<span id='gts-o-has-preorder'>N</span>")
            sb.AppendLine("<span id='gts-o-has-digital'>N</span>")


            Dim ca As DataLayer.StoreCartItemCollection = DataLayer.StoreCartItemRow.GetCartItems(DB, so.OrderId)
            For Each c As DataLayer.StoreCartItemRow In ca
                If c.Type = "item" Then

                    Dim strName As String = c.ItemName.Replace("'", "\'")
                    If strName.Contains("|") Then
                        strName = strName.Substring(0, strName.LastIndexOf("|"))
                    End If

                    sb.AppendLine("<span class='gts-item'>")
                    sb.AppendLine("<span class='gts-i-name'>" & c.ItemName & "</span>")
                    sb.AppendLine("<span class='gts-i-price'>" & IIf(c.CustomerPrice <= 0, c.Price, c.CustomerPrice) & "</span>")
                    sb.AppendLine("<span class='gts-i-quantity'>" & c.Quantity & "</span>")
                    sb.AppendLine("<span class='gts-i-prodsearch-id'></span>")
                    sb.AppendLine("<span class='gts-i-prodsearch-store-id'></span>")
                    sb.AppendLine("</span>")
                End If
            Next
        Catch ex As Exception
            Email.SendError("ToError500", "Google Trusted Store", "Url: " & Request.RawUrl & "<br>OrderId: " & OrderId & "<br>Exception: " & ex.ToString() & "<br>" & sb.ToString())
        End Try

        sb.AppendLine("</div>")
        litGoogleTrustedStore.Text = sb.ToString()
    End Sub
End Class
