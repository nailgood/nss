Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Collections
Imports System.Web.Services.Protocols
Imports ShippingValidator.FedexRate
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Configuration

Public Class FedexRate
    Private m_Country As String
    Private m_Code As String
    Private m_ShippingType As Integer
    Private m_Weight As Double
    Private m_Zone As Integer
    Private m_Rate As Double
    Private m_TotalNetCharge As Double
    Private m_TotalSurcharges As Double
    Private m_Msg As String
    Private m_FeeDAS As Double
    Private m_FeeFuel As Double
    Private m_FeeCOD As Double
    Private m_TotalFreightDiscount As Double
    Public Property FeeDAS() As Double
        Get
            Return m_FeeDAS
        End Get
        Set(ByVal Value As Double)
            m_FeeDAS = Value
        End Set
    End Property
    Public Property FeeFuel() As Double
        Get
            Return m_FeeFuel
        End Get
        Set(ByVal Value As Double)
            m_FeeFuel = Value
        End Set
    End Property
    Public Property FeeCOD() As Double
        Get
            Return m_FeeCOD
        End Get
        Set(ByVal Value As Double)
            m_FeeCOD = Value
        End Set
    End Property
    
    Public Property TotalFreightDiscount() As Double
        Get
            Return m_TotalFreightDiscount
        End Get
        Set(ByVal Value As Double)
            m_TotalFreightDiscount = Value
        End Set
    End Property
    Public Property Country() As String
        Get
            Return m_Country
        End Get
        Set(ByVal Value As String)
            m_Country = Value
        End Set
    End Property
    Public Property Code() As String
        Get
            Return m_Code
        End Get
        Set(ByVal Value As String)
            m_Code = Value
        End Set
    End Property
    Public Property ShippingType() As Integer
        Get
            Return m_ShippingType
        End Get
        Set(ByVal Value As Integer)
            m_ShippingType = Value
        End Set
    End Property
    Public Property Weight() As Double
        Get
            Return m_Weight
        End Get
        Set(ByVal Value As Double)
            m_Weight = Value
        End Set
    End Property
    Public Property Zone() As Integer
        Get
            Return m_Zone
        End Get
        Set(ByVal Value As Integer)
            m_Zone = Value
        End Set
    End Property
    Public Property Rate() As Double
        Get
            Return m_Rate
        End Get
        Set(ByVal Value As Double)
            m_Rate = Value
        End Set
    End Property
    Public Property TotalNetCharge() As Double
        Get
            Return m_TotalNetCharge
        End Get
        Set(ByVal Value As Double)
            m_TotalNetCharge = Value
        End Set
    End Property
    Public Property TotalSurcharges() As Double
        Get
            Return m_TotalSurcharges
        End Get
        Set(ByVal Value As Double)
            m_TotalSurcharges = Value
        End Set
    End Property
    Public Property Msg() As String
        Get
            Return m_Msg
        End Get
        Set(ByVal Value As String)
            m_Msg = Value
        End Set
    End Property

    Public Sub New(ByVal inputCode As String, ByVal inputweight As Double, ByVal inputShippingType As Integer, ByVal inputCountry As String)
        Code = inputCode
        Weight = inputweight
        Weight = Math.Ceiling(Weight)
        Country = inputCountry
        ShippingType = inputShippingType
        Dim request As FedexGetRate.RateRequest = CreateRateRequest()
        '
        Dim service As FedexGetRate.RateService = New FedexGetRate.RateService() ' Initialize the service

        Try
            ' Call the web service passing in a RateRequest and returning a RateReply
            Dim reply As FedexGetRate.RateReply = service.getRates(request)

            If ((reply.HighestSeverity = FedexGetRate.NotificationSeverityType.SUCCESS) Or (reply.HighestSeverity = FedexGetRate.NotificationSeverityType.NOTE) Or (reply.HighestSeverity = FedexGetRate.NotificationSeverityType.WARNING)) Then
                ShowRateReply(reply)
            Else
                m_Msg &= ("---------ERROR----------") & Environment.NewLine
                For Each notification As FedExGetRate.Notification In reply.Notifications
                    m_Msg &= notification.Message & Environment.NewLine
                Next
            End If
            'ShowNotifications(reply)
        Catch e As SoapException
            m_Msg &= ("---------ERROR----------") & Environment.NewLine
            m_Msg &= ("---------FedEx SoapException ----------------") & Environment.NewLine
            m_Msg &= e.Detail.InnerText
        Catch e As Exception
            m_Msg = ("---------ERROR----------") & Environment.NewLine
            m_Msg &= ("---------FedEx Exception----------------") & Environment.NewLine
            m_Msg &= e.Message
        End Try

    End Sub
    Function CreateRateRequest() As FedexGetRate.RateRequest
        ' Build a RateRequest

        Dim request As FedexGetRate.RateRequest = New FedexGetRate.RateRequest()
        '
        request.WebAuthenticationDetail = New FedexGetRate.WebAuthenticationDetail()
        request.WebAuthenticationDetail.UserCredential = New FedexGetRate.WebAuthenticationCredential()
        request.WebAuthenticationDetail.UserCredential.Key = ConfigurationSettings.AppSettings("FedexKey") ' Replace "XXX" with the Key
        request.WebAuthenticationDetail.UserCredential.Password = ConfigurationSettings.AppSettings("FedexPassword").ToString() ' Replace "XXX" with the Password
        '
        request.ClientDetail = New FedexGetRate.ClientDetail()
        request.ClientDetail.AccountNumber = ConfigurationSettings.AppSettings("FedexAccountNumber").ToString() ' Replace "XXX" with client's account number
        request.ClientDetail.MeterNumber = ConfigurationSettings.AppSettings("FedexMeterNumber").ToString() ' Replace "XXX" with client's meter number
        '
        request.TransactionDetail = New FedexGetRate.TransactionDetail()
        request.TransactionDetail.CustomerTransactionId = "***Rate Available Services Request v13 using VB.NET ***" ' This is a reference field for the customer.  Any value can be used and will be provided in the response.
        '
        request.Version = New FedexGetRate.VersionId() ' WSDL version information, value is automatically set from wsdl
        '
        request.ReturnTransitAndCommit = True
        request.ReturnTransitAndCommitSpecified = True
        '
        SetShipmentDetails(request)
        '
        Return request
    End Function
    Sub ShowRateReply(ByRef reply As FedexGetRate.RateReply)
        If (reply.RateReplyDetails Is Nothing) Then
            Return
        End If

        ' For i As Integer = 0 To reply.RateReplyDetails.Length - 1
        For i As Integer = 0 To reply.RateReplyDetails.Length - 1
            Dim rateReplyDetail As FedexGetRate.RateReplyDetail = reply.RateReplyDetails(i)
            'If i + 1 = 3 Or i + 1 = 5 Then
            m_Msg &= String.Format("<br>Rate Reply Detail for Service {0} service {1} ", i + 1, rateReplyDetail.ServiceType)

            'If (rateReplyDetail.ServiceTypeSpecified) Then
            '    Console.WriteLine("Service Type: {0}", rateReplyDetail.ServiceType)
            'End If
            'If (rateReplyDetail.PackagingTypeSpecified) Then
            '    Console.WriteLine("Packaging Type: {0}", rateReplyDetail.PackagingType)
            'End If

            If (rateReplyDetail.RatedShipmentDetails IsNot Nothing) Then
                For j As Integer = 0 To rateReplyDetail.RatedShipmentDetails.Length - 1
                    ' For j As Integer = 0 To 1
                    'If j = 0 Then
                    Dim shipmentDetail As FedexGetRate.RatedShipmentDetail = rateReplyDetail.RatedShipmentDetails(j)
                    Dim strType As String = rateReplyDetail.ServiceType.ToString
                    ' Console.WriteLine("---Rated Shipment Detail for Rate Type {0}---", j + 1)
                    If strType = GetValueShippingType(ShippingType) Then
                        ShowShipmentRateDetails(shipmentDetail)
                        Exit Sub
                    Else
                        Zone = 0
                        Rate = 0
                        TotalNetCharge = 0
                    End If

                    'End If

                    'ShowPackageRateDetails(shipmentDetail.RatedPackages)
                Next j
            End If
            'ShowDeliveryDetails(rateReplyDetail)
            'Console.WriteLine("**********************************************************")
            ' End If


        Next i
    End Sub
    Sub ShowShipmentRateDetails(ByRef shipmentDetail As FedexGetRate.RatedShipmentDetail)
        If (shipmentDetail Is Nothing) Then Return
        If (shipmentDetail.ShipmentRateDetail Is Nothing) Then Return
        Dim rateDetail As FedexGetRate.ShipmentRateDetail = shipmentDetail.ShipmentRateDetail
        m_Msg &= String.Format("<br>--- Shipment Rate Detail ---")
        '
        'Console.WriteLine("RateType: {0}", rateDetail.RateType)
        If (rateDetail.TotalBillingWeight IsNot Nothing) Then
            m_Msg &= String.Format("Total Billing Weight: {0} {1}", rateDetail.TotalBillingWeight.Value, rateDetail.TotalBillingWeight.Units)
        End If
        'insert to data shipping rate

        If (rateDetail.TotalBaseCharge IsNot Nothing) Then
            m_Msg &= String.Format("<br>Total Base Charge: {0} {1}", rateDetail.TotalBaseCharge.Amount, rateDetail.TotalBaseCharge.Currency)
            '  Console.WriteLine("reatezone:" & rateDetail.RateZone & "---ZipCode: " & zipCode & " Shipping rates: {0} {1}", rateDetail.TotalBaseCharge.Amount, rateDetail.TotalBaseCharge.Currency)
            Zone = CInt(rateDetail.RateZone)
            Rate = CDbl(rateDetail.TotalBaseCharge.Amount)
            TotalNetCharge = CDbl(rateDetail.TotalNetCharge.Amount)

            ' arrStr &= rateDetail.RateZone & "," & strCountry & "," & rateDetail.TotalBaseCharge.Amount & "," & rateDetail.TotalNetCharge.Amount & Environment.NewLine

        End If

        If (rateDetail.TotalFreightDiscounts IsNot Nothing) Then
            m_Msg &= String.Format("Total Freight Discounts: {0} {1}", rateDetail.TotalFreightDiscounts.Amount, rateDetail.TotalFreightDiscounts.Currency)
            TotalFreightDiscount = CDbl(rateDetail.TotalFreightDiscounts.Amount)
        End If

        If (rateDetail.TotalSurcharges IsNot Nothing) Then
            m_Msg &= String.Format("<br>Total Surcharges: {0} {1}", rateDetail.TotalSurcharges.Amount, rateDetail.TotalSurcharges.Currency)
            TotalSurcharges = CDbl(rateDetail.TotalSurcharges.Amount)
        End If

        If (rateDetail.Surcharges IsNot Nothing) Then

            ' Individual surcharge for each package
            For Each surcharge As FedExGetRate.Surcharge In rateDetail.Surcharges
                If surcharge.SurchargeType = FedExGetRate.SurchargeType.DELIVERY_AREA Then
                    FeeDAS = CDbl(surcharge.Amount.Amount)
                End If
                If surcharge.SurchargeType = FedExGetRate.SurchargeType.FUEL Then
                    FeeFuel = CDbl(surcharge.Amount.Amount)
                End If
                If surcharge.SurchargeType = FedExGetRate.SurchargeType.COD Then
                    FeeCOD = CDbl(surcharge.Amount.Amount)
                End If

                m_Msg &= String.Format("{0} surcharge {1} {2}", surcharge.SurchargeType, surcharge.Amount.Amount, surcharge.Amount.Currency)
            Next surcharge
        End If

        If (rateDetail.TotalNetCharge IsNot Nothing) Then
            m_Msg &= String.Format("<br>Total Net Charge: {0} {1}", rateDetail.TotalNetCharge.Amount, rateDetail.TotalNetCharge.Currency)
            TotalNetCharge = CDbl(rateDetail.TotalNetCharge.Amount)
        End If
    End Sub
    Sub SetShipmentDetails(ByRef request As FedexGetRate.RateRequest)
        request.RequestedShipment = New FedexGetRate.RequestedShipment()
        request.RequestedShipment.ShipTimestamp = DateTime.Now ' Ship date and time
        request.RequestedShipment.ShipTimestampSpecified = True
        request.RequestedShipment.DropoffType = FedexGetRate.DropoffType.REGULAR_PICKUP 'Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
        request.RequestedShipment.DropoffTypeSpecified = True
        request.RequestedShipment.PackagingType = FedexGetRate.PackagingType.YOUR_PACKAGING
        request.RequestedShipment.PackagingTypeSpecified = True
        '
        SetOrigin(request)
        '
        SetDestination(request)
        '
        SetPackageLineItems(request)
        '
        request.RequestedShipment.RateRequestTypes = New FedexGetRate.RateRequestType(1) {} ' Rate types requested LIST, MULTIWEIGHT, ...
        request.RequestedShipment.RateRequestTypes(0) = FedexGetRate.RateRequestType.ACCOUNT
        request.RequestedShipment.RateRequestTypes(1) = FedexGetRate.RateRequestType.LIST
        request.RequestedShipment.PackageCount = "1"
        ' set to true to request COD shipment
        Dim isCodShipment As Boolean = True
        If (isCodShipment) Then
            SetCOD(request)
        End If

      
    End Sub
    Sub SetOrigin(ByRef request As FedexGetRate.RateRequest)
        request.RequestedShipment.Shipper = New FedexGetRate.Party()
        request.RequestedShipment.Shipper.Address = New FedexGetRate.Address()
        request.RequestedShipment.Shipper.Address.StreetLines = New String(0) {"SHIPPER ADDRESS LINE 1"}
        request.RequestedShipment.Shipper.Address.City = "Franklin Park"
        request.RequestedShipment.Shipper.Address.StateOrProvinceCode = "IL"
        request.RequestedShipment.Shipper.Address.PostalCode = "60131"
        request.RequestedShipment.Shipper.Address.CountryCode = "US"
    End Sub

    Sub SetDestination(ByRef request As FedExGetRate.RateRequest)
        request.RequestedShipment.Recipient = New FedexGetRate.Party()
        request.RequestedShipment.Recipient.Address = New FedexGetRate.Address()
        request.RequestedShipment.Recipient.Address.StreetLines = New String(0) {"RECIPIENT ADDRESS LINE 1"}
        request.RequestedShipment.Recipient.Address.City = ""
        request.RequestedShipment.Recipient.Address.StateOrProvinceCode = ""


        If LCase(Country) <> "us" Then
            request.RequestedShipment.Recipient.Address.PostalCode = "" 'zipcode
            request.RequestedShipment.Recipient.Address.CountryCode = UCase(Country) '"VI"
        Else
            request.RequestedShipment.Recipient.Address.PostalCode = Code 'zipcode
            request.RequestedShipment.Recipient.Address.CountryCode = "US" '"VI"
        End If

    End Sub
    Sub SetCOD(ByRef request As FedexGetRate.RateRequest)
        ' To get all COD rates, set both COD details at both package and shipment level
        ' Set COD at Package level for Ground Services
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested = New FedexGetRate.PackageSpecialServicesRequested()
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.SpecialServiceTypes = New FedexGetRate.PackageSpecialServiceType(0) {FedexGetRate.PackageSpecialServiceType.COD}
        '
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.CodDetail = New FedexGetRate.CodDetail()
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.CodDetail.CollectionType = FedexGetRate.CodCollectionType.GUARANTEED_FUNDS
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.CodDetail.CodCollectionAmount = New FedexGetRate.Money()
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.CodDetail.CodCollectionAmount.Amount = 250
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.CodDetail.CodCollectionAmount.AmountSpecified = True
        request.RequestedShipment.RequestedPackageLineItems(0).SpecialServicesRequested.CodDetail.CodCollectionAmount.Currency = "USD"
        '
        ' Set COD at Shipment level for Express Services
        request.RequestedShipment.SpecialServicesRequested = New FedexGetRate.ShipmentSpecialServicesRequested() ' Special service requested
        request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = New FedexGetRate.ShipmentSpecialServiceType(0) {FedexGetRate.ShipmentSpecialServiceType.COD}
        '
        request.RequestedShipment.SpecialServicesRequested.CodDetail = New FedexGetRate.CodDetail()
        request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount = New FedexGetRate.Money()
        request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount.Amount = 150
        request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount.AmountSpecified = True
        request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount.Currency = "USD"
        request.RequestedShipment.SpecialServicesRequested.CodDetail.CollectionType = FedexGetRate.CodCollectionType.GUARANTEED_FUNDS ' ANY, CASH, GUARANTEED_FUNDS
    End Sub
    Sub SetPackageLineItems(ByRef request As FedexGetRate.RateRequest)
        request.RequestedShipment.RequestedPackageLineItems = New FedexGetRate.RequestedPackageLineItem(0) {New FedexGetRate.RequestedPackageLineItem()}
        request.RequestedShipment.RequestedPackageLineItems(0).SequenceNumber = "1" ' package sequence number
        request.RequestedShipment.RequestedPackageLineItems(0).GroupPackageCount = "1"
        ' Package weight
        request.RequestedShipment.RequestedPackageLineItems(0).Weight = New FedexGetRate.Weight()
        request.RequestedShipment.RequestedPackageLineItems(0).Weight.Units = FedexGetRate.WeightUnits.LB
        request.RequestedShipment.RequestedPackageLineItems(0).Weight.Value = CDec(Weight)
        ' package dimensions
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions = New FedexGetRate.Dimensions()
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Length = "0"
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Width = "0"
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Height = "0"
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Units = FedExGetRate.LinearUnits.IN


    End Sub
    Private Function GetValueShippingType(ByVal ShippingType As Integer) As String
        Select Case ShippingType
            Case 16 '"FED"
                Return "FEDEX_GROUND"
            Case 17 '"FED2DAY"
                Return "FEDEX_2_DAY"
            Case 18 '"FEDNEXTDAY"
                Return "STANDARD_OVERNIGHT"
            Case 19 '"FEDINT"
                Return "INTERNATIONAL_PRIORITY"
        End Select
        Return ""
    End Function
End Class
'Public Class varFedexRate
'    Private m_Country As String
'    Private m_Code As String
'    Private m_Weight As Double
'    Private m_Zone As Integer
'    Private m_Rate As Double
'    Private m_TotalRate As Double
'    Public Property Country() As String
'        Get
'            Return m_Country
'        End Get
'        Set(ByVal Value As String)
'            m_Country = Value
'        End Set
'    End Property
'    Public Property Code() As String
'        Get
'            Return m_Code
'        End Get
'        Set(ByVal Value As String)
'            m_Code = Value
'        End Set
'    End Property
'    Public Property Weight() As Double
'        Get
'            Return m_Weight
'        End Get
'        Set(ByVal Value As Double)
'            m_Weight = Value
'        End Set
'    End Property
'    Public Property Zone() As Integer
'        Get
'            Return m_Zone
'        End Get
'        Set(ByVal Value As Integer)
'            m_Zone = Value
'        End Set
'    End Property
'    Public Property Rate() As Double
'        Get
'            Return m_Rate
'        End Get
'        Set(ByVal Value As Double)
'            m_Rate = Value
'        End Set
'    End Property
'    Public Property TotalRate() As Double
'        Get
'            Return m_TotalRate
'        End Get
'        Set(ByVal Value As Double)
'            m_TotalRate = Value
'        End Set
'    End Property
'End Class


