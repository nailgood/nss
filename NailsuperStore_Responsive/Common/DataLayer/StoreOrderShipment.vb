Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class StoreOrderShipmentRow
		Inherits StoreOrderShipmentRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer)
			MyBase.New(DB, ShipmentId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ShipmentNo As String)
			MyBase.New(DB, ShipmentNo)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ShipmentId As Integer) As StoreOrderShipmentRow
			Dim row As StoreOrderShipmentRow

			row = New StoreOrderShipmentRow(DB, ShipmentId)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal ShipmentNo As String) As StoreOrderShipmentRow
			Dim row As StoreOrderShipmentRow

			row = New StoreOrderShipmentRow(DB, ShipmentNo)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ShipmentId As Integer)
			Dim row As StoreOrderShipmentRow

			row = New StoreOrderShipmentRow(DB, ShipmentId)
			row.Remove()
		End Sub

		'Custom Methods
		Public Function GetTrackingNumbers() As DataTable
			Return DB.GetDataTable("SELECT * FROM StoreOrderShipmentTracking WHERE ShipmentId = " & ShipmentId)
		End Function

		Public Sub CopyFromNavision(ByVal r As NavisionOrderShipmentsRow)
			If OrderId = Nothing Then
				If Trim(r.Web_Reference_No) = Nothing AndAlso Not IsNumeric(r.Web_Reference_No) Then
					If r.Order_No.ToString.ToLower.IndexOf("w") = 0 Then
						OrderId = DB.ExecuteScalar("select top 1 OrderId from StoreOrder where OrderNo = " & DB.Quote(Trim(r.Order_No)))
					Else
						OrderId = DB.ExecuteScalar("select top 1 OrderId from StoreOrder where NavisionOrderNo = " & DB.Quote(Trim(r.Order_No)))
					End If
				Else
					OrderId = r.Web_Reference_No
				End If
			End If
			'ignore error
			If OrderId = Nothing Then Exit Sub 'Throw New Exception("Order not found for OrderShipment." & vbCrLf & "OrderNo: " & r.Order_No)

			SellToCustomerId = DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(r.Sell_To_Customer_No))
			If r.Bill_to_Customer_No = r.Sell_To_Customer_No Then BillToCustomerId = SellToCustomerId Else BillToCustomerId = DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(r.Bill_to_Customer_No))

			'ignore error
			If SellToCustomerId = Nothing OrElse BillToCustomerId = Nothing Then Exit Sub

			ShipmentNo = r.Order_Shipment_No
			BillToName = r.Bill_to_Name
			BillToName2 = r.Bill_to_Name_2
			BillToAddress = r.Bill_to_Address
			BillToAddress3 = r.Bill_to_Address_3
			BillToAddress2 = r.Bill_to_Address_2
			BillToCity = r.Bill_to_City
			BillToContact = r.Bill_to_Contact
			YourReference = r.Your_Reference
			ShipToCode = r.Ship_to_Code
			ShipToName = r.Ship_to_Name
			ShipToName_2 = r.Ship_to_Name_2
			ShipToAddress = r.Ship_to_Address
			ShipToAddress3 = r.Ship_to_Address_3
			ShipToAddress2 = r.Ship_to_Address_2
			ShipToCity = r.Ship_to_City
			ShipToContact = r.Ship_to_Contact
			OrderDate = r.Order_Date
			PostingDate = r.Posting_Date
			ShipmentDate = r.Shipment_Date
			PostingDescription = r.Posting_Description
			PaymentTermsCode = r.Payment_Terms_Code
			DueDate = r.Due_Date
			PaymentDiscountPercent = r.Payment_Discount_Percent
			PmtDiscountDate = r.Pmt_Discount_Date
			ShipmentMethodCode = r.Shipment_Method_Code
			LocationCode = r.Location_Code
			CurrencyCode = r.Currency_Code
			CurrencyFactor = r.Currency_Factor
			PricesIncludingVAT = r.Prices_Including_VAT
			InvoiceDiscCode = r.Invoice_Disc_Code
			CustomerDiscGroup = r.Customer_Disc_Group
			LanguageCode = r.Language_Code
			SalespersonCode = r.Salesperson_Code
			VATRegistrationNo = r.VAT_Registration_No
			ReasonCode = r.Reason_Code
			VATCountryCode = r.VAT_Country_Code
			SellToCustomerName = r.Sell_to_Customer_Name
			SellToCustomerName2 = r.Sell_to_Customer_Name_2
			SellToCustomerAddress = r.Sell_to_Customer_Address
			SellToCustomerAddress3 = r.Sell_to_Customer_Address_3
			SellToCustomerAddress2 = r.Sell_to_Customer_Address_2
			SellToCity = r.Sell_to_City
			SellToContact = r.Sell_to_Contact
			BillToZipcode = r.Bill_to_Post_Code
			BillToCounty = r.Bill_to_County
			BillToCountry = r.Bill_to_Country_Code
			SellToPostCode = r.Sell_to_Post_Code
			SellToCounty = r.Sell_to_County
			SellToCountry = r.Sell_to_Country_Code
			ShipToPostCode = r.Ship_to_Post_Code
			ShipToCounty = r.Ship_to_County
			ShipToCountry = r.Ship_to_Country_Code
			DocumentDate = r.Document_Date
			ExternalDocumentNo = r.External_Document_No
			PaymentMethodCode = r.Payment_Method_Code
			ShippingAgentCode = r.Shipping_Agent_Code
			UserID = r.User_ID
			SourceCode = r.Source_Code
			TaxAreaCode = r.Tax_Area_Code
			TaxLiable = r.Tax_Liable
			CampaignNo = r.Campaign_No
			SellToContactNo = r.Sell_to_Contact_No
			BillToContactNo = r.Bill_to_Contact_No
			ResponsibilityCenter = r.Responsibility_Center
			RequestedDeliveryDate = r.Requested_Delivery_Date
			PromisedDeliveryDate = r.Promised_Delivery_Date
			ShippingTime = r.Shipping_Time
			OutboundWhseHandlingTime = r.Outbound_Whse_Handling_Time
			ShippingAgentServiceCode = r.Shipping_Agent_Service_Code
			AllowLineDisc = r.Allow_Line_Disc
			ShipToUPSZone = r.Ship_to_UPS_Zone
			DateTimeSent = r.Date_Time_Sent
			CompletelyShipped = r.Completely_Shipped = "Y"

			If ShipmentId = Nothing Then
				Insert()
			Else
				Update()
			End If
		End Sub

	End Class

	Public MustInherit Class StoreOrderShipmentRowBase
		Private m_DB As Database
		Private m_ShipmentId As Integer = Nothing
		Private m_SellToCustomerId As Integer = Nothing
		Private m_ShipmentNo As String = Nothing
		Private m_BillToCustomerId As Integer = Nothing
		Private m_BillToName As String = Nothing
		Private m_BillToName2 As String = Nothing
		Private m_BillToAddress As String = Nothing
		Private m_BillToAddress3 As String = Nothing
		Private m_BillToAddress2 As String = Nothing
		Private m_BillToCity As String = Nothing
		Private m_BillToContact As String = Nothing
		Private m_YourReference As String = Nothing
		Private m_ShipToCode As String = Nothing
		Private m_ShipToName As String = Nothing
		Private m_ShipToName_2 As String = Nothing
		Private m_ShipToAddress As String = Nothing
		Private m_ShipToAddress3 As String = Nothing
		Private m_ShipToAddress2 As String = Nothing
		Private m_ShipToCity As String = Nothing
		Private m_ShipToContact As String = Nothing
		Private m_OrderDate As DateTime = Nothing
		Private m_PostingDate As DateTime = Nothing
		Private m_ShipmentDate As DateTime = Nothing
		Private m_PostingDescription As String = Nothing
		Private m_PaymentTermsCode As String = Nothing
		Private m_DueDate As DateTime = Nothing
		Private m_PaymentDiscountPercent As Double = Nothing
		Private m_PmtDiscountDate As DateTime = Nothing
		Private m_ShipmentMethodCode As String = Nothing
		Private m_LocationCode As String = Nothing
		Private m_CurrencyCode As String = Nothing
		Private m_CurrencyFactor As Double = Nothing
		Private m_PricesIncludingVAT As String = Nothing
		Private m_InvoiceDiscCode As String = Nothing
		Private m_CustomerDiscGroup As String = Nothing
		Private m_LanguageCode As String = Nothing
		Private m_SalespersonCode As String = Nothing
		Private m_OrderId As Integer = Nothing
		Private m_VATRegistrationNo As String = Nothing
		Private m_ReasonCode As String = Nothing
		Private m_VATCountryCode As String = Nothing
		Private m_SellToCustomerName As String = Nothing
		Private m_SellToCustomerName2 As String = Nothing
		Private m_SellToCustomerAddress As String = Nothing
		Private m_SellToCustomerAddress2 As String = Nothing
		Private m_SellToCustomerAddress3 As String = Nothing
		Private m_SellToCity As String = Nothing
		Private m_SellToContact As String = Nothing
		Private m_BillToZipcode As String = Nothing
		Private m_BillToCounty As String = Nothing
		Private m_BillToCountry As String = Nothing
		Private m_SellToPostCode As String = Nothing
		Private m_SellToCounty As String = Nothing
		Private m_SellToCountry As String = Nothing
		Private m_ShipToPostCode As String = Nothing
		Private m_ShipToCounty As String = Nothing
		Private m_ShipToCountry As String = Nothing
		Private m_DocumentDate As DateTime = Nothing
		Private m_ExternalDocumentNo As String = Nothing
		Private m_PaymentMethodCode As String = Nothing
		Private m_ShippingAgentCode As String = Nothing
		Private m_UserID As String = Nothing
		Private m_SourceCode As String = Nothing
		Private m_TaxAreaCode As String = Nothing
		Private m_TaxLiable As String = Nothing
		Private m_CampaignNo As String = Nothing
		Private m_SellToContactNo As String = Nothing
		Private m_BillToContactNo As String = Nothing
		Private m_ResponsibilityCenter As String = Nothing
		Private m_RequestedDeliveryDate As DateTime = Nothing
		Private m_PromisedDeliveryDate As DateTime = Nothing
		Private m_ShippingTime As DateTime = Nothing
		Private m_OutboundWhseHandlingTime As DateTime = Nothing
		Private m_ShippingAgentServiceCode As String = Nothing
		Private m_AllowLineDisc As String = Nothing
		Private m_ShipToUPSZone As String = Nothing
		Private m_DateTimeSent As DateTime = Nothing
		Private m_EmailSent As Boolean = Nothing
		Private m_CompletelyShipped As Boolean = Nothing

		Public Property CompletelyShipped() As Boolean
			Get
				Return m_CompletelyShipped
			End Get
			Set(ByVal value As Boolean)
				m_CompletelyShipped = value
			End Set
		End Property


		Public Property ShipmentId() As Integer
			Get
				Return m_ShipmentId
			End Get
			Set(ByVal Value As Integer)
				m_ShipmentId = value
			End Set
		End Property

		Public Property SellToCustomerId() As Integer
			Get
				Return m_SellToCustomerId
			End Get
			Set(ByVal Value As Integer)
				m_SellToCustomerId = Value
			End Set
		End Property

		Public Property ShipmentNo() As String
			Get
				Return m_ShipmentNo
			End Get
			Set(ByVal Value As String)
				m_ShipmentNo = value
			End Set
		End Property

		Public Property BillToCustomerId() As Integer
			Get
				Return m_BillToCustomerId
			End Get
			Set(ByVal Value As Integer)
				m_BillToCustomerId = Value
			End Set
		End Property

		Public Property BillToName() As String
			Get
				Return m_BillToName
			End Get
			Set(ByVal Value As String)
				m_BillToName = value
			End Set
		End Property

		Public Property BillToName2() As String
			Get
				Return m_BillToName2
			End Get
			Set(ByVal Value As String)
				m_BillToName2 = value
			End Set
		End Property

		Public Property BillToAddress() As String
			Get
				Return m_BillToAddress
			End Get
			Set(ByVal Value As String)
				m_BillToAddress = value
			End Set
		End Property

		Public Property BillToAddress2() As String
			Get
				Return m_BillToAddress2
			End Get
			Set(ByVal Value As String)
				m_BillToAddress2 = value
			End Set
		End Property

		Public Property BillToAddress3() As String
			Get
				Return m_BillToAddress3
			End Get
			Set(ByVal Value As String)
				m_BillToAddress3 = Value
			End Set
		End Property

		Public Property BillToCity() As String
			Get
				Return m_BillToCity
			End Get
			Set(ByVal Value As String)
				m_BillToCity = Value
			End Set
		End Property

		Public Property BillToContact() As String
			Get
				Return m_BillToContact
			End Get
			Set(ByVal Value As String)
				m_BillToContact = value
			End Set
		End Property

		Public Property YourReference() As String
			Get
				Return m_YourReference
			End Get
			Set(ByVal Value As String)
				m_YourReference = value
			End Set
		End Property

		Public Property ShipToCode() As String
			Get
				Return m_ShipToCode
			End Get
			Set(ByVal Value As String)
				m_ShipToCode = value
			End Set
		End Property

		Public Property ShipToName() As String
			Get
				Return m_ShipToName
			End Get
			Set(ByVal Value As String)
				m_ShipToName = value
			End Set
		End Property

		Public Property ShipToName_2() As String
			Get
				Return m_ShipToName_2
			End Get
			Set(ByVal Value As String)
				m_ShipToName_2 = value
			End Set
		End Property

		Public Property ShipToAddress() As String
			Get
				Return m_ShipToAddress
			End Get
			Set(ByVal Value As String)
				m_ShipToAddress = value
			End Set
		End Property

		Public Property ShipToAddress2() As String
			Get
				Return m_ShipToAddress2
			End Get
			Set(ByVal Value As String)
				m_ShipToAddress2 = value
			End Set
		End Property

		Public Property ShipToAddress3() As String
			Get
				Return m_ShipToAddress3
			End Get
			Set(ByVal Value As String)
				m_ShipToAddress3 = Value
			End Set
		End Property

		Public Property ShipToCity() As String
			Get
				Return m_ShipToCity
			End Get
			Set(ByVal Value As String)
				m_ShipToCity = Value
			End Set
		End Property

		Public Property ShipToContact() As String
			Get
				Return m_ShipToContact
			End Get
			Set(ByVal Value As String)
				m_ShipToContact = value
			End Set
		End Property

		Public Property OrderDate() As DateTime
			Get
				Return m_OrderDate
			End Get
			Set(ByVal Value As DateTime)
				m_OrderDate = value
			End Set
		End Property

		Public Property PostingDate() As DateTime
			Get
				Return m_PostingDate
			End Get
			Set(ByVal Value As DateTime)
				m_PostingDate = value
			End Set
		End Property

		Public Property ShipmentDate() As DateTime
			Get
				Return m_ShipmentDate
			End Get
			Set(ByVal Value As DateTime)
				m_ShipmentDate = value
			End Set
		End Property

		Public Property PostingDescription() As String
			Get
				Return m_PostingDescription
			End Get
			Set(ByVal Value As String)
				m_PostingDescription = value
			End Set
		End Property

		Public Property PaymentTermsCode() As String
			Get
				Return m_PaymentTermsCode
			End Get
			Set(ByVal Value As String)
				m_PaymentTermsCode = value
			End Set
		End Property

		Public Property DueDate() As DateTime
			Get
				Return m_DueDate
			End Get
			Set(ByVal Value As DateTime)
				m_DueDate = value
			End Set
		End Property

		Public Property PaymentDiscountPercent() As Double
			Get
				Return m_PaymentDiscountPercent
			End Get
			Set(ByVal Value As Double)
				m_PaymentDiscountPercent = value
			End Set
		End Property

		Public Property PmtDiscountDate() As DateTime
			Get
				Return m_PmtDiscountDate
			End Get
			Set(ByVal Value As DateTime)
				m_PmtDiscountDate = value
			End Set
		End Property

		Public Property ShipmentMethodCode() As String
			Get
				Return m_ShipmentMethodCode
			End Get
			Set(ByVal Value As String)
				m_ShipmentMethodCode = value
			End Set
		End Property

		Public Property LocationCode() As String
			Get
				Return m_LocationCode
			End Get
			Set(ByVal Value As String)
				m_LocationCode = value
			End Set
		End Property

		Public Property CurrencyCode() As String
			Get
				Return m_CurrencyCode
			End Get
			Set(ByVal Value As String)
				m_CurrencyCode = value
			End Set
		End Property

		Public Property CurrencyFactor() As Double
			Get
				Return m_CurrencyFactor
			End Get
			Set(ByVal Value As Double)
				m_CurrencyFactor = value
			End Set
		End Property

		Public Property PricesIncludingVAT() As String
			Get
				Return m_PricesIncludingVAT
			End Get
			Set(ByVal Value As String)
				m_PricesIncludingVAT = value
			End Set
		End Property

		Public Property InvoiceDiscCode() As String
			Get
				Return m_InvoiceDiscCode
			End Get
			Set(ByVal Value As String)
				m_InvoiceDiscCode = value
			End Set
		End Property

		Public Property CustomerDiscGroup() As String
			Get
				Return m_CustomerDiscGroup
			End Get
			Set(ByVal Value As String)
				m_CustomerDiscGroup = value
			End Set
		End Property

		Public Property LanguageCode() As String
			Get
				Return m_LanguageCode
			End Get
			Set(ByVal Value As String)
				m_LanguageCode = value
			End Set
		End Property

		Public Property SalespersonCode() As String
			Get
				Return m_SalespersonCode
			End Get
			Set(ByVal Value As String)
				m_SalespersonCode = value
			End Set
		End Property

		Public Property OrderId() As Integer
			Get
				Return m_OrderId
			End Get
			Set(ByVal Value As Integer)
				m_OrderId = Value
			End Set
		End Property

		Public Property VATRegistrationNo() As String
			Get
				Return m_VATRegistrationNo
			End Get
			Set(ByVal Value As String)
				m_VATRegistrationNo = value
			End Set
		End Property

		Public Property ReasonCode() As String
			Get
				Return m_ReasonCode
			End Get
			Set(ByVal Value As String)
				m_ReasonCode = value
			End Set
		End Property

		Public Property VATCountryCode() As String
			Get
				Return m_VATCountryCode
			End Get
			Set(ByVal Value As String)
				m_VATCountryCode = value
			End Set
		End Property

		Public Property SellToCustomerName() As String
			Get
				Return m_SellToCustomerName
			End Get
			Set(ByVal Value As String)
				m_SellToCustomerName = value
			End Set
		End Property

		Public Property SellToCustomerName2() As String
			Get
				Return m_SellToCustomerName2
			End Get
			Set(ByVal Value As String)
				m_SellToCustomerName2 = value
			End Set
		End Property

		Public Property SellToCustomerAddress() As String
			Get
				Return m_SellToCustomerAddress
			End Get
			Set(ByVal Value As String)
				m_SellToCustomerAddress = value
			End Set
		End Property

		Public Property SellToCustomerAddress2() As String
			Get
				Return m_SellToCustomerAddress2
			End Get
			Set(ByVal Value As String)
				m_SellToCustomerAddress2 = value
			End Set
		End Property

		Public Property SellToCustomerAddress3() As String
			Get
				Return m_SellToCustomerAddress3
			End Get
			Set(ByVal Value As String)
				m_SellToCustomerAddress3 = Value
			End Set
		End Property

		Public Property SellToCity() As String
			Get
				Return m_SellToCity
			End Get
			Set(ByVal Value As String)
				m_SellToCity = Value
			End Set
		End Property

		Public Property SellToContact() As String
			Get
				Return m_SellToContact
			End Get
			Set(ByVal Value As String)
				m_SellToContact = value
			End Set
		End Property

		Public Property BillToZipcode() As String
			Get
				Return m_BillToZipcode
			End Get
			Set(ByVal Value As String)
				m_BillToZipcode = value
			End Set
		End Property

		Public Property BillToCounty() As String
			Get
				Return m_BillToCounty
			End Get
			Set(ByVal Value As String)
				m_BillToCounty = value
			End Set
		End Property

		Public Property BillToCountry() As String
			Get
				Return m_BillToCountry
			End Get
			Set(ByVal Value As String)
				m_BillToCountry = value
			End Set
		End Property

		Public Property SellToPostCode() As String
			Get
				Return m_SellToPostCode
			End Get
			Set(ByVal Value As String)
				m_SellToPostCode = value
			End Set
		End Property

		Public Property SellToCounty() As String
			Get
				Return m_SellToCounty
			End Get
			Set(ByVal Value As String)
				m_SellToCounty = value
			End Set
		End Property

		Public Property SellToCountry() As String
			Get
				Return m_SellToCountry
			End Get
			Set(ByVal Value As String)
				m_SellToCountry = value
			End Set
		End Property

		Public Property ShipToPostCode() As String
			Get
				Return m_ShipToPostCode
			End Get
			Set(ByVal Value As String)
				m_ShipToPostCode = value
			End Set
		End Property

		Public Property ShipToCounty() As String
			Get
				Return m_ShipToCounty
			End Get
			Set(ByVal Value As String)
				m_ShipToCounty = value
			End Set
		End Property

		Public Property ShipToCountry() As String
			Get
				Return m_ShipToCountry
			End Get
			Set(ByVal Value As String)
				m_ShipToCountry = value
			End Set
		End Property

		Public Property DocumentDate() As DateTime
			Get
				Return m_DocumentDate
			End Get
			Set(ByVal Value As DateTime)
				m_DocumentDate = value
			End Set
		End Property

		Public Property ExternalDocumentNo() As String
			Get
				Return m_ExternalDocumentNo
			End Get
			Set(ByVal Value As String)
				m_ExternalDocumentNo = value
			End Set
		End Property

		Public Property PaymentMethodCode() As String
			Get
				Return m_PaymentMethodCode
			End Get
			Set(ByVal Value As String)
				m_PaymentMethodCode = value
			End Set
		End Property

		Public Property ShippingAgentCode() As String
			Get
				Return m_ShippingAgentCode
			End Get
			Set(ByVal Value As String)
				m_ShippingAgentCode = value
			End Set
		End Property

		Public Property UserID() As String
			Get
				Return m_UserID
			End Get
			Set(ByVal Value As String)
				m_UserID = Value
			End Set
		End Property

		Public Property SourceCode() As String
			Get
				Return m_SourceCode
			End Get
			Set(ByVal Value As String)
				m_SourceCode = value
			End Set
		End Property

		Public Property TaxAreaCode() As String
			Get
				Return m_TaxAreaCode
			End Get
			Set(ByVal Value As String)
				m_TaxAreaCode = value
			End Set
		End Property

		Public Property TaxLiable() As String
			Get
				Return m_TaxLiable
			End Get
			Set(ByVal Value As String)
				m_TaxLiable = value
			End Set
		End Property

		Public Property CampaignNo() As String
			Get
				Return m_CampaignNo
			End Get
			Set(ByVal Value As String)
				m_CampaignNo = value
			End Set
		End Property

		Public Property SellToContactNo() As String
			Get
				Return m_SellToContactNo
			End Get
			Set(ByVal Value As String)
				m_SellToContactNo = value
			End Set
		End Property

		Public Property BillToContactNo() As String
			Get
				Return m_BillToContactNo
			End Get
			Set(ByVal Value As String)
				m_BillToContactNo = value
			End Set
		End Property

		Public Property ResponsibilityCenter() As String
			Get
				Return m_ResponsibilityCenter
			End Get
			Set(ByVal Value As String)
				m_ResponsibilityCenter = value
			End Set
		End Property

		Public Property RequestedDeliveryDate() As DateTime
			Get
				Return m_RequestedDeliveryDate
			End Get
			Set(ByVal Value As DateTime)
				m_RequestedDeliveryDate = value
			End Set
		End Property

		Public Property PromisedDeliveryDate() As DateTime
			Get
				Return m_PromisedDeliveryDate
			End Get
			Set(ByVal Value As DateTime)
				m_PromisedDeliveryDate = value
			End Set
		End Property

		Public Property ShippingTime() As DateTime
			Get
				Return m_ShippingTime
			End Get
			Set(ByVal Value As DateTime)
				m_ShippingTime = value
			End Set
		End Property

		Public Property OutboundWhseHandlingTime() As DateTime
			Get
				Return m_OutboundWhseHandlingTime
			End Get
			Set(ByVal Value As DateTime)
				m_OutboundWhseHandlingTime = value
			End Set
		End Property

		Public Property ShippingAgentServiceCode() As String
			Get
				Return m_ShippingAgentServiceCode
			End Get
			Set(ByVal Value As String)
				m_ShippingAgentServiceCode = value
			End Set
		End Property

		Public Property AllowLineDisc() As String
			Get
				Return m_AllowLineDisc
			End Get
			Set(ByVal Value As String)
				m_AllowLineDisc = value
			End Set
		End Property

		Public Property ShipToUPSZone() As String
			Get
				Return m_ShipToUPSZone
			End Get
			Set(ByVal Value As String)
				m_ShipToUPSZone = value
			End Set
		End Property

		Public Property DateTimeSent() As DateTime
			Get
				Return m_DateTimeSent
			End Get
			Set(ByVal Value As DateTime)
				m_DateTimeSent = value
			End Set
		End Property

		Public Property EmailSent() As Boolean
			Get
				Return m_EmailSent
			End Get
			Set(ByVal value As Boolean)
				m_EmailSent = value
			End Set
		End Property

		Public Property DB() As Database
			Get
				DB = m_DB
			End Get
			Set(ByVal Value As DataBase)
				m_DB = Value
			End Set
		End Property

		Public Sub New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			m_DB = DB
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer)
			m_DB = DB
			m_ShipmentId = ShipmentId
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ShipmentNo As String)
			m_DB = DB
			m_ShipmentNo = ShipmentNo
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            If ShipmentId > 0 OrElse Not String.IsNullOrEmpty(ShipmentNo) Then
                Try
                    Dim SQL As String
                    SQL = "SELECT * FROM StoreOrderShipment WHERE " & IIf(ShipmentId <> Nothing, "ShipmentId = " & DB.Number(ShipmentId), "ShipmentNo = " & DB.Quote(ShipmentNo))
                    r = m_DB.GetReader(SQL)
                    If r.Read Then
                        Me.Load(r)
                    End If
                    Core.CloseReader(r)
                Catch ex As Exception
                    Core.CloseReader(r)
                    Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
                End Try
            End If
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_ShipmentId = Convert.ToInt32(r.Item("ShipmentId"))
                m_CompletelyShipped = Convert.ToBoolean(r.Item("CompletelyShipped"))
                If IsDBNull(r.Item("SellToCustomerId")) Then
                    m_SellToCustomerId = Nothing
                Else
                    m_SellToCustomerId = Convert.ToString(r.Item("SellToCustomerId"))
                End If
                If IsDBNull(r.Item("ShipmentNo")) Then
                    m_ShipmentNo = Nothing
                Else
                    m_ShipmentNo = Convert.ToString(r.Item("ShipmentNo"))
                End If
                If IsDBNull(r.Item("BillToCustomerId")) Then
                    m_BillToCustomerId = Nothing
                Else
                    m_BillToCustomerId = Convert.ToString(r.Item("BillToCustomerId"))
                End If
                If IsDBNull(r.Item("BillToName")) Then
                    m_BillToName = Nothing
                Else
                    m_BillToName = Convert.ToString(r.Item("BillToName"))
                End If
                If IsDBNull(r.Item("BillToName2")) Then
                    m_BillToName2 = Nothing
                Else
                    m_BillToName2 = Convert.ToString(r.Item("BillToName2"))
                End If
                If IsDBNull(r.Item("BillToAddress")) Then
                    m_BillToAddress = Nothing
                Else
                    m_BillToAddress = Convert.ToString(r.Item("BillToAddress"))
                End If
                If IsDBNull(r.Item("BillToAddress2")) Then
                    m_BillToAddress2 = Nothing
                Else
                    m_BillToAddress2 = Convert.ToString(r.Item("BillToAddress2"))
                End If
                If IsDBNull(r.Item("BillToAddress3")) Then
                    m_BillToAddress3 = Nothing
                Else
                    m_BillToAddress3 = Convert.ToString(r.Item("BillToAddress3"))
                End If
                If IsDBNull(r.Item("BillToCity")) Then
                    m_BillToCity = Nothing
                Else
                    m_BillToCity = Convert.ToString(r.Item("BillToCity"))
                End If
                If IsDBNull(r.Item("BillToContact")) Then
                    m_BillToContact = Nothing
                Else
                    m_BillToContact = Convert.ToString(r.Item("BillToContact"))
                End If
                If IsDBNull(r.Item("YourReference")) Then
                    m_YourReference = Nothing
                Else
                    m_YourReference = Convert.ToString(r.Item("YourReference"))
                End If
                If IsDBNull(r.Item("ShipToCode")) Then
                    m_ShipToCode = Nothing
                Else
                    m_ShipToCode = Convert.ToString(r.Item("ShipToCode"))
                End If
                If IsDBNull(r.Item("ShipToName")) Then
                    m_ShipToName = Nothing
                Else
                    m_ShipToName = Convert.ToString(r.Item("ShipToName"))
                End If
                If IsDBNull(r.Item("ShipToName_2")) Then
                    m_ShipToName_2 = Nothing
                Else
                    m_ShipToName_2 = Convert.ToString(r.Item("ShipToName_2"))
                End If
                If IsDBNull(r.Item("ShipToAddress")) Then
                    m_ShipToAddress = Nothing
                Else
                    m_ShipToAddress = Convert.ToString(r.Item("ShipToAddress"))
                End If
                If IsDBNull(r.Item("ShipToAddress2")) Then
                    m_ShipToAddress2 = Nothing
                Else
                    m_ShipToAddress2 = Convert.ToString(r.Item("ShipToAddress2"))
                End If
                If IsDBNull(r.Item("ShipToAddress3")) Then
                    m_ShipToAddress3 = Nothing
                Else
                    m_ShipToAddress3 = Convert.ToString(r.Item("ShipToAddress3"))
                End If
                If IsDBNull(r.Item("ShipToCity")) Then
                    m_ShipToCity = Nothing
                Else
                    m_ShipToCity = Convert.ToString(r.Item("ShipToCity"))
                End If
                If IsDBNull(r.Item("ShipToContact")) Then
                    m_ShipToContact = Nothing
                Else
                    m_ShipToContact = Convert.ToString(r.Item("ShipToContact"))
                End If
                If IsDBNull(r.Item("OrderDate")) Then
                    m_OrderDate = Nothing
                Else
                    m_OrderDate = Convert.ToDateTime(r.Item("OrderDate"))
                End If
                If IsDBNull(r.Item("PostingDate")) Then
                    m_PostingDate = Nothing
                Else
                    m_PostingDate = Convert.ToDateTime(r.Item("PostingDate"))
                End If
                If IsDBNull(r.Item("ShipmentDate")) Then
                    m_ShipmentDate = Nothing
                Else
                    m_ShipmentDate = Convert.ToDateTime(r.Item("ShipmentDate"))
                End If
                If IsDBNull(r.Item("PostingDescription")) Then
                    m_PostingDescription = Nothing
                Else
                    m_PostingDescription = Convert.ToString(r.Item("PostingDescription"))
                End If
                If IsDBNull(r.Item("PaymentTermsCode")) Then
                    m_PaymentTermsCode = Nothing
                Else
                    m_PaymentTermsCode = Convert.ToString(r.Item("PaymentTermsCode"))
                End If
                If IsDBNull(r.Item("DueDate")) Then
                    m_DueDate = Nothing
                Else
                    m_DueDate = Convert.ToDateTime(r.Item("DueDate"))
                End If
                If IsDBNull(r.Item("PaymentDiscountPercent")) Then
                    m_PaymentDiscountPercent = Nothing
                Else
                    m_PaymentDiscountPercent = Convert.ToDouble(r.Item("PaymentDiscountPercent"))
                End If
                If IsDBNull(r.Item("PmtDiscountDate")) Then
                    m_PmtDiscountDate = Nothing
                Else
                    m_PmtDiscountDate = Convert.ToDateTime(r.Item("PmtDiscountDate"))
                End If
                If IsDBNull(r.Item("ShipmentMethodCode")) Then
                    m_ShipmentMethodCode = Nothing
                Else
                    m_ShipmentMethodCode = Convert.ToString(r.Item("ShipmentMethodCode"))
                End If
                If IsDBNull(r.Item("LocationCode")) Then
                    m_LocationCode = Nothing
                Else
                    m_LocationCode = Convert.ToString(r.Item("LocationCode"))
                End If
                If IsDBNull(r.Item("CurrencyCode")) Then
                    m_CurrencyCode = Nothing
                Else
                    m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
                End If
                If IsDBNull(r.Item("CurrencyFactor")) Then
                    m_CurrencyFactor = Nothing
                Else
                    m_CurrencyFactor = Convert.ToDouble(r.Item("CurrencyFactor"))
                End If
                If IsDBNull(r.Item("PricesIncludingVAT")) Then
                    m_PricesIncludingVAT = Nothing
                Else
                    m_PricesIncludingVAT = Convert.ToString(r.Item("PricesIncludingVAT"))
                End If
                If IsDBNull(r.Item("InvoiceDiscCode")) Then
                    m_InvoiceDiscCode = Nothing
                Else
                    m_InvoiceDiscCode = Convert.ToString(r.Item("InvoiceDiscCode"))
                End If
                If IsDBNull(r.Item("CustomerDiscGroup")) Then
                    m_CustomerDiscGroup = Nothing
                Else
                    m_CustomerDiscGroup = Convert.ToString(r.Item("CustomerDiscGroup"))
                End If
                If IsDBNull(r.Item("LanguageCode")) Then
                    m_LanguageCode = Nothing
                Else
                    m_LanguageCode = Convert.ToString(r.Item("LanguageCode"))
                End If
                If IsDBNull(r.Item("SalespersonCode")) Then
                    m_SalespersonCode = Nothing
                Else
                    m_SalespersonCode = Convert.ToString(r.Item("SalespersonCode"))
                End If
                If IsDBNull(r.Item("OrderId")) Then
                    m_OrderId = Nothing
                Else
                    m_OrderId = Convert.ToString(r.Item("OrderId"))
                End If
                If IsDBNull(r.Item("VATRegistrationNo")) Then
                    m_VATRegistrationNo = Nothing
                Else
                    m_VATRegistrationNo = Convert.ToString(r.Item("VATRegistrationNo"))
                End If
                If IsDBNull(r.Item("ReasonCode")) Then
                    m_ReasonCode = Nothing
                Else
                    m_ReasonCode = Convert.ToString(r.Item("ReasonCode"))
                End If
                If IsDBNull(r.Item("VATCountryCode")) Then
                    m_VATCountryCode = Nothing
                Else
                    m_VATCountryCode = Convert.ToString(r.Item("VATCountryCode"))
                End If
                If IsDBNull(r.Item("SellToCustomerName")) Then
                    m_SellToCustomerName = Nothing
                Else
                    m_SellToCustomerName = Convert.ToString(r.Item("SellToCustomerName"))
                End If
                If IsDBNull(r.Item("SellToCustomerName2")) Then
                    m_SellToCustomerName2 = Nothing
                Else
                    m_SellToCustomerName2 = Convert.ToString(r.Item("SellToCustomerName2"))
                End If
                If IsDBNull(r.Item("SellToCustomerAddress")) Then
                    m_SellToCustomerAddress = Nothing
                Else
                    m_SellToCustomerAddress = Convert.ToString(r.Item("SellToCustomerAddress"))
                End If
                If IsDBNull(r.Item("SellToCustomerAddress2")) Then
                    m_SellToCustomerAddress2 = Nothing
                Else
                    m_SellToCustomerAddress2 = Convert.ToString(r.Item("SellToCustomerAddress2"))
                End If
                If IsDBNull(r.Item("SellToCustomerAddress3")) Then
                    m_SellToCustomerAddress3 = Nothing
                Else
                    m_SellToCustomerAddress3 = Convert.ToString(r.Item("SellToCustomerAddress3"))
                End If
                If IsDBNull(r.Item("SellToCity")) Then
                    m_SellToCity = Nothing
                Else
                    m_SellToCity = Convert.ToString(r.Item("SellToCity"))
                End If
                If IsDBNull(r.Item("SellToContact")) Then
                    m_SellToContact = Nothing
                Else
                    m_SellToContact = Convert.ToString(r.Item("SellToContact"))
                End If
                If IsDBNull(r.Item("BillToZipcode")) Then
                    m_BillToZipcode = Nothing
                Else
                    m_BillToZipcode = Convert.ToString(r.Item("BillToZipcode"))
                End If
                If IsDBNull(r.Item("BillToCounty")) Then
                    m_BillToCounty = Nothing
                Else
                    m_BillToCounty = Convert.ToString(r.Item("BillToCounty"))
                End If
                If IsDBNull(r.Item("BillToCountry")) Then
                    m_BillToCountry = Nothing
                Else
                    m_BillToCountry = Convert.ToString(r.Item("BillToCountry"))
                End If
                If IsDBNull(r.Item("SellToPostCode")) Then
                    m_SellToPostCode = Nothing
                Else
                    m_SellToPostCode = Convert.ToString(r.Item("SellToPostCode"))
                End If
                If IsDBNull(r.Item("SellToCounty")) Then
                    m_SellToCounty = Nothing
                Else
                    m_SellToCounty = Convert.ToString(r.Item("SellToCounty"))
                End If
                If IsDBNull(r.Item("SellToCountry")) Then
                    m_SellToCountry = Nothing
                Else
                    m_SellToCountry = Convert.ToString(r.Item("SellToCountry"))
                End If
                If IsDBNull(r.Item("ShipToPostCode")) Then
                    m_ShipToPostCode = Nothing
                Else
                    m_ShipToPostCode = Convert.ToString(r.Item("ShipToPostCode"))
                End If
                If IsDBNull(r.Item("ShipToCounty")) Then
                    m_ShipToCounty = Nothing
                Else
                    m_ShipToCounty = Convert.ToString(r.Item("ShipToCounty"))
                End If
                If IsDBNull(r.Item("ShipToCountry")) Then
                    m_ShipToCountry = Nothing
                Else
                    m_ShipToCountry = Convert.ToString(r.Item("ShipToCountry"))
                End If
                If IsDBNull(r.Item("DocumentDate")) Then
                    m_DocumentDate = Nothing
                Else
                    m_DocumentDate = Convert.ToDateTime(r.Item("DocumentDate"))
                End If
                If IsDBNull(r.Item("ExternalDocumentNo")) Then
                    m_ExternalDocumentNo = Nothing
                Else
                    m_ExternalDocumentNo = Convert.ToString(r.Item("ExternalDocumentNo"))
                End If
                If IsDBNull(r.Item("PaymentMethodCode")) Then
                    m_PaymentMethodCode = Nothing
                Else
                    m_PaymentMethodCode = Convert.ToString(r.Item("PaymentMethodCode"))
                End If
                If IsDBNull(r.Item("ShippingAgentCode")) Then
                    m_ShippingAgentCode = Nothing
                Else
                    m_ShippingAgentCode = Convert.ToString(r.Item("ShippingAgentCode"))
                End If
                If IsDBNull(r.Item("UserID")) Then
                    m_UserID = Nothing
                Else
                    m_UserID = Convert.ToString(r.Item("UserID"))
                End If
                If IsDBNull(r.Item("SourceCode")) Then
                    m_SourceCode = Nothing
                Else
                    m_SourceCode = Convert.ToString(r.Item("SourceCode"))
                End If
                If IsDBNull(r.Item("TaxAreaCode")) Then
                    m_TaxAreaCode = Nothing
                Else
                    m_TaxAreaCode = Convert.ToString(r.Item("TaxAreaCode"))
                End If
                If IsDBNull(r.Item("TaxLiable")) Then
                    m_TaxLiable = Nothing
                Else
                    m_TaxLiable = Convert.ToString(r.Item("TaxLiable"))
                End If
                If IsDBNull(r.Item("CampaignNo")) Then
                    m_CampaignNo = Nothing
                Else
                    m_CampaignNo = Convert.ToString(r.Item("CampaignNo"))
                End If
                If IsDBNull(r.Item("SellToContactNo")) Then
                    m_SellToContactNo = Nothing
                Else
                    m_SellToContactNo = Convert.ToString(r.Item("SellToContactNo"))
                End If
                If IsDBNull(r.Item("BillToContactNo")) Then
                    m_BillToContactNo = Nothing
                Else
                    m_BillToContactNo = Convert.ToString(r.Item("BillToContactNo"))
                End If
                If IsDBNull(r.Item("ResponsibilityCenter")) Then
                    m_ResponsibilityCenter = Nothing
                Else
                    m_ResponsibilityCenter = Convert.ToString(r.Item("ResponsibilityCenter"))
                End If
                If IsDBNull(r.Item("RequestedDeliveryDate")) Then
                    m_RequestedDeliveryDate = Nothing
                Else
                    m_RequestedDeliveryDate = Convert.ToDateTime(r.Item("RequestedDeliveryDate"))
                End If
                If IsDBNull(r.Item("PromisedDeliveryDate")) Then
                    m_PromisedDeliveryDate = Nothing
                Else
                    m_PromisedDeliveryDate = Convert.ToDateTime(r.Item("PromisedDeliveryDate"))
                End If
                If IsDBNull(r.Item("ShippingTime")) Then
                    m_ShippingTime = Nothing
                Else
                    m_ShippingTime = Convert.ToDateTime(r.Item("ShippingTime"))
                End If
                If IsDBNull(r.Item("OutboundWhseHandlingTime")) Then
                    m_OutboundWhseHandlingTime = Nothing
                Else
                    m_OutboundWhseHandlingTime = Convert.ToDateTime(r.Item("OutboundWhseHandlingTime"))
                End If
                If IsDBNull(r.Item("ShippingAgentServiceCode")) Then
                    m_ShippingAgentServiceCode = Nothing
                Else
                    m_ShippingAgentServiceCode = Convert.ToString(r.Item("ShippingAgentServiceCode"))
                End If
                If IsDBNull(r.Item("AllowLineDisc")) Then
                    m_AllowLineDisc = Nothing
                Else
                    m_AllowLineDisc = Convert.ToString(r.Item("AllowLineDisc"))
                End If
                If IsDBNull(r.Item("ShipToUPSZone")) Then
                    m_ShipToUPSZone = Nothing
                Else
                    m_ShipToUPSZone = Convert.ToString(r.Item("ShipToUPSZone"))
                End If
                If IsDBNull(r.Item("DateTimeSent")) Then
                    m_DateTimeSent = Nothing
                Else
                    m_DateTimeSent = Convert.ToDateTime(r.Item("DateTimeSent"))
                End If
                m_EmailSent = Convert.ToBoolean(r.Item("EmailSent"))
            Catch ex As Exception
                Throw ex


            End Try

        End Sub 'Load
        Public Function InsertSassi(ByVal _DB As Database) As Integer
            Dim SQL As String

            SQL = " INSERT INTO StoreOrderShipment (" _
             & " SellToCustomerId" _
             & ",ShipmentNo" _
             & ",BillToCustomerId" _
             & ",OrderId" _
             & ") VALUES (" _
             & _DB.Quote(SellToCustomerId) _
             & "," & _DB.Quote(ShipmentNo) _
             & "," & _DB.Quote(BillToCustomerId) _
             & "," & _DB.Quote(OrderId) _
             & ")"

            ShipmentId = _DB.InsertSQL(SQL)

            Return ShipmentId
        End Function
		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO StoreOrderShipment (" _
			 & " SellToCustomerId" _
			 & ",ShipmentNo" _
			 & ",BillToCustomerId" _
			 & ",BillToName" _
			 & ",BillToName2" _
			 & ",BillToAddress" _
			 & ",BillToAddress3" _
			 & ",BillToAddress2" _
			 & ",BillToCity" _
			 & ",BillToContact" _
			 & ",YourReference" _
			 & ",ShipToCode" _
			 & ",ShipToName" _
			 & ",ShipToName_2" _
			 & ",ShipToAddress" _
			 & ",ShipToAddress3" _
			 & ",ShipToAddress2" _
			 & ",ShipToCity" _
			 & ",ShipToContact" _
			 & ",OrderDate" _
			 & ",PostingDate" _
			 & ",ShipmentDate" _
			 & ",PostingDescription" _
			 & ",PaymentTermsCode" _
			 & ",DueDate" _
			 & ",PaymentDiscountPercent" _
			 & ",PmtDiscountDate" _
			 & ",ShipmentMethodCode" _
			 & ",LocationCode" _
			 & ",CurrencyCode" _
			 & ",CurrencyFactor" _
			 & ",PricesIncludingVAT" _
			 & ",InvoiceDiscCode" _
			 & ",CustomerDiscGroup" _
			 & ",LanguageCode" _
			 & ",SalespersonCode" _
			 & ",OrderId" _
			 & ",VATRegistrationNo" _
			 & ",ReasonCode" _
			 & ",VATCountryCode" _
			 & ",SellToCustomerName" _
			 & ",SellToCustomerName2" _
			 & ",SellToCustomerAddress" _
			 & ",SellToCustomerAddress3" _
			 & ",SellToCustomerAddress2" _
			 & ",SellToCity" _
			 & ",SellToContact" _
			 & ",BillToZipcode" _
			 & ",BillToCounty" _
			 & ",BillToCountry" _
			 & ",SellToPostCode" _
			 & ",SellToCounty" _
			 & ",SellToCountry" _
			 & ",ShipToPostCode" _
			 & ",ShipToCounty" _
			 & ",ShipToCountry" _
			 & ",DocumentDate" _
			 & ",ExternalDocumentNo" _
			 & ",PaymentMethodCode" _
			 & ",ShippingAgentCode" _
			 & ",UserID" _
			 & ",SourceCode" _
			 & ",TaxAreaCode" _
			 & ",TaxLiable" _
			 & ",CampaignNo" _
			 & ",SellToContactNo" _
			 & ",BillToContactNo" _
			 & ",ResponsibilityCenter" _
			 & ",RequestedDeliveryDate" _
			 & ",PromisedDeliveryDate" _
			 & ",ShippingTime" _
			 & ",OutboundWhseHandlingTime" _
			 & ",ShippingAgentServiceCode" _
			 & ",AllowLineDisc" _
			 & ",ShipToUPSZone" _
			 & ",DateTimeSent" _
			 & ",EmailSent" _
			 & ",CompletelyShipped" _
			 & ") VALUES (" _
			 & m_DB.Quote(SellToCustomerId) _
			 & "," & m_DB.Quote(ShipmentNo) _
			 & "," & m_DB.Quote(BillToCustomerId) _
			 & "," & m_DB.Quote(BillToName) _
			 & "," & m_DB.Quote(BillToName2) _
			 & "," & m_DB.Quote(BillToAddress) _
			 & "," & m_DB.Quote(BillToAddress3) _
			 & "," & m_DB.Quote(BillToAddress2) _
			 & "," & m_DB.Quote(BillToCity) _
			 & "," & m_DB.Quote(BillToContact) _
			 & "," & m_DB.Quote(YourReference) _
			 & "," & m_DB.Quote(ShipToCode) _
			 & "," & m_DB.Quote(ShipToName) _
			 & "," & m_DB.Quote(ShipToName_2) _
			 & "," & m_DB.Quote(ShipToAddress) _
			 & "," & m_DB.Quote(ShipToAddress3) _
			 & "," & m_DB.Quote(ShipToAddress2) _
			 & "," & m_DB.Quote(ShipToCity) _
			 & "," & m_DB.Quote(ShipToContact) _
			 & "," & m_DB.NullQuote(OrderDate) _
			 & "," & m_DB.NullQuote(PostingDate) _
			 & "," & m_DB.NullQuote(ShipmentDate) _
			 & "," & m_DB.Quote(PostingDescription) _
			 & "," & m_DB.Quote(PaymentTermsCode) _
			 & "," & m_DB.NullQuote(DueDate) _
			 & "," & m_DB.Number(PaymentDiscountPercent) _
			 & "," & m_DB.NullQuote(PmtDiscountDate) _
			 & "," & m_DB.Quote(ShipmentMethodCode) _
			 & "," & m_DB.Quote(LocationCode) _
			 & "," & m_DB.Quote(CurrencyCode) _
			 & "," & m_DB.Number(CurrencyFactor) _
			 & "," & m_DB.Quote(PricesIncludingVAT) _
			 & "," & m_DB.Quote(InvoiceDiscCode) _
			 & "," & m_DB.Quote(CustomerDiscGroup) _
			 & "," & m_DB.Quote(LanguageCode) _
			 & "," & m_DB.Quote(SalespersonCode) _
			 & "," & m_DB.Quote(OrderId) _
			 & "," & m_DB.Quote(VATRegistrationNo) _
			 & "," & m_DB.Quote(ReasonCode) _
			 & "," & m_DB.Quote(VATCountryCode) _
			 & "," & m_DB.Quote(SellToCustomerName) _
			 & "," & m_DB.Quote(SellToCustomerName2) _
			 & "," & m_DB.Quote(SellToCustomerAddress) _
			 & "," & m_DB.Quote(SellToCustomerAddress3) _
			 & "," & m_DB.Quote(SellToCustomerAddress2) _
			 & "," & m_DB.Quote(SellToCity) _
			 & "," & m_DB.Quote(SellToContact) _
			 & "," & m_DB.Quote(BillToZipcode) _
			 & "," & m_DB.Quote(BillToCounty) _
			 & "," & m_DB.Quote(BillToCountry) _
			 & "," & m_DB.Quote(SellToPostCode) _
			 & "," & m_DB.Quote(SellToCounty) _
			 & "," & m_DB.Quote(SellToCountry) _
			 & "," & m_DB.Quote(ShipToPostCode) _
			 & "," & m_DB.Quote(ShipToCounty) _
			 & "," & m_DB.Quote(ShipToCountry) _
			 & "," & m_DB.NullQuote(DocumentDate) _
			 & "," & m_DB.Quote(ExternalDocumentNo) _
			 & "," & m_DB.Quote(PaymentMethodCode) _
			 & "," & m_DB.Quote(ShippingAgentCode) _
			 & "," & m_DB.Quote(UserID) _
			 & "," & m_DB.Quote(SourceCode) _
			 & "," & m_DB.Quote(TaxAreaCode) _
			 & "," & m_DB.Quote(TaxLiable) _
			 & "," & m_DB.Quote(CampaignNo) _
			 & "," & m_DB.Quote(SellToContactNo) _
			 & "," & m_DB.Quote(BillToContactNo) _
			 & "," & m_DB.Quote(ResponsibilityCenter) _
			 & "," & m_DB.NullQuote(RequestedDeliveryDate) _
			 & "," & m_DB.NullQuote(PromisedDeliveryDate) _
			 & "," & m_DB.NullQuote(ShippingTime) _
			 & "," & m_DB.NullQuote(OutboundWhseHandlingTime) _
			 & "," & m_DB.Quote(ShippingAgentServiceCode) _
			 & "," & m_DB.Quote(AllowLineDisc) _
			 & "," & m_DB.Quote(ShipToUPSZone) _
			 & "," & m_DB.NullQuote(DateTimeSent) _
			 & "," & CInt(EmailSent) _
			 & "," & CInt(CompletelyShipped) _
			 & ")"

			ShipmentId = m_DB.InsertSQL(SQL)

			Return ShipmentId
		End Function
        Public Sub UpdateSassi(ByVal _DB As Database)
            Dim SQL As String

            SQL = " UPDATE StoreOrderShipment SET " _
             & " SellToCustomerId = " & _DB.Quote(SellToCustomerId) _
             & ",ShipmentNo = " & _DB.Quote(ShipmentNo) _
             & ",BillToCustomerId = " & _DB.Quote(BillToCustomerId) _
             & ",OrderId = " & _DB.Quote(OrderId) _
             & " WHERE " & IIf(ShipmentId <> Nothing, "ShipmentId = " & _DB.Number(ShipmentId), "ShipmentNo = " & _DB.Quote(ShipmentNo))

            _DB.ExecuteSQL(SQL)
        End Sub
		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE StoreOrderShipment SET " _
			 & " SellToCustomerId = " & m_DB.Quote(SellToCustomerId) _
			 & ",ShipmentNo = " & m_DB.Quote(ShipmentNo) _
			 & ",BillToCustomerId = " & m_DB.Quote(BillToCustomerId) _
			 & ",BillToName = " & m_DB.Quote(BillToName) _
			 & ",BillToName2 = " & m_DB.Quote(BillToName2) _
			 & ",BillToAddress = " & m_DB.Quote(BillToAddress) _
			 & ",BillToAddress3 = " & m_DB.Quote(BillToAddress3) _
			 & ",BillToAddress2 = " & m_DB.Quote(BillToAddress2) _
			 & ",BillToCity = " & m_DB.Quote(BillToCity) _
			 & ",BillToContact = " & m_DB.Quote(BillToContact) _
			 & ",YourReference = " & m_DB.Quote(YourReference) _
			 & ",ShipToCode = " & m_DB.Quote(ShipToCode) _
			 & ",ShipToName = " & m_DB.Quote(ShipToName) _
			 & ",ShipToName_2 = " & m_DB.Quote(ShipToName_2) _
			 & ",ShipToAddress = " & m_DB.Quote(ShipToAddress) _
			 & ",ShipToAddress3 = " & m_DB.Quote(ShipToAddress3) _
			 & ",ShipToAddress2 = " & m_DB.Quote(ShipToAddress2) _
			 & ",ShipToCity = " & m_DB.Quote(ShipToCity) _
			 & ",ShipToContact = " & m_DB.Quote(ShipToContact) _
			 & ",OrderDate = " & m_DB.NullQuote(OrderDate) _
			 & ",PostingDate = " & m_DB.NullQuote(PostingDate) _
			 & ",ShipmentDate = " & m_DB.NullQuote(ShipmentDate) _
			 & ",PostingDescription = " & m_DB.Quote(PostingDescription) _
			 & ",PaymentTermsCode = " & m_DB.Quote(PaymentTermsCode) _
			 & ",DueDate = " & m_DB.NullQuote(DueDate) _
			 & ",PaymentDiscountPercent = " & m_DB.Number(PaymentDiscountPercent) _
			 & ",PmtDiscountDate = " & m_DB.NullQuote(PmtDiscountDate) _
			 & ",ShipmentMethodCode = " & m_DB.Quote(ShipmentMethodCode) _
			 & ",LocationCode = " & m_DB.Quote(LocationCode) _
			 & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
			 & ",CurrencyFactor = " & m_DB.Number(CurrencyFactor) _
			 & ",PricesIncludingVAT = " & m_DB.Quote(PricesIncludingVAT) _
			 & ",InvoiceDiscCode = " & m_DB.Quote(InvoiceDiscCode) _
			 & ",CustomerDiscGroup = " & m_DB.Quote(CustomerDiscGroup) _
			 & ",LanguageCode = " & m_DB.Quote(LanguageCode) _
			 & ",SalespersonCode = " & m_DB.Quote(SalespersonCode) _
			 & ",OrderId = " & m_DB.Quote(OrderId) _
			 & ",VATRegistrationNo = " & m_DB.Quote(VATRegistrationNo) _
			 & ",ReasonCode = " & m_DB.Quote(ReasonCode) _
			 & ",VATCountryCode = " & m_DB.Quote(VATCountryCode) _
			 & ",SellToCustomerName = " & m_DB.Quote(SellToCustomerName) _
			 & ",SellToCustomerName2 = " & m_DB.Quote(SellToCustomerName2) _
			 & ",SellToCustomerAddress = " & m_DB.Quote(SellToCustomerAddress) _
			 & ",SellToCustomerAddress3 = " & m_DB.Quote(SellToCustomerAddress3) _
			 & ",SellToCustomerAddress2 = " & m_DB.Quote(SellToCustomerAddress2) _
			 & ",SellToCity = " & m_DB.Quote(SellToCity) _
			 & ",SellToContact = " & m_DB.Quote(SellToContact) _
			 & ",BillToZipcode = " & m_DB.Quote(BillToZipcode) _
			 & ",BillToCounty = " & m_DB.Quote(BillToCounty) _
			 & ",BillToCountry = " & m_DB.Quote(BillToCountry) _
			 & ",SellToPostCode = " & m_DB.Quote(SellToPostCode) _
			 & ",SellToCounty = " & m_DB.Quote(SellToCounty) _
			 & ",SellToCountry = " & m_DB.Quote(SellToCountry) _
			 & ",ShipToPostCode = " & m_DB.Quote(ShipToPostCode) _
			 & ",ShipToCounty = " & m_DB.Quote(ShipToCounty) _
			 & ",ShipToCountry = " & m_DB.Quote(ShipToCountry) _
			 & ",DocumentDate = " & m_DB.NullQuote(DocumentDate) _
			 & ",ExternalDocumentNo = " & m_DB.Quote(ExternalDocumentNo) _
			 & ",PaymentMethodCode = " & m_DB.Quote(PaymentMethodCode) _
			 & ",ShippingAgentCode = " & m_DB.Quote(ShippingAgentCode) _
			 & ",UserID = " & m_DB.Quote(UserID) _
			 & ",SourceCode = " & m_DB.Quote(SourceCode) _
			 & ",TaxAreaCode = " & m_DB.Quote(TaxAreaCode) _
			 & ",TaxLiable = " & m_DB.Quote(TaxLiable) _
			 & ",CampaignNo = " & m_DB.Quote(CampaignNo) _
			 & ",SellToContactNo = " & m_DB.Quote(SellToContactNo) _
			 & ",BillToContactNo = " & m_DB.Quote(BillToContactNo) _
			 & ",ResponsibilityCenter = " & m_DB.Quote(ResponsibilityCenter) _
			 & ",RequestedDeliveryDate = " & m_DB.NullQuote(RequestedDeliveryDate) _
			 & ",PromisedDeliveryDate = " & m_DB.NullQuote(PromisedDeliveryDate) _
			 & ",ShippingTime = " & m_DB.NullQuote(ShippingTime) _
			 & ",OutboundWhseHandlingTime = " & m_DB.NullQuote(OutboundWhseHandlingTime) _
			 & ",ShippingAgentServiceCode = " & m_DB.Quote(ShippingAgentServiceCode) _
			 & ",AllowLineDisc = " & m_DB.Quote(AllowLineDisc) _
			 & ",ShipToUPSZone = " & m_DB.Quote(ShipToUPSZone) _
			 & ",DateTimeSent = " & m_DB.NullQuote(DateTimeSent) _
			 & ",EmailSent = " & CInt(EmailSent) _
			 & ",CompletelyShipped = " & CInt(CompletelyShipped) _
			 & " WHERE " & IIf(ShipmentId <> Nothing, "ShipmentId = " & DB.Number(ShipmentId), "ShipmentNo = " & DB.Quote(ShipmentNo))

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM StoreOrderShipment WHERE " & IIf(ShipmentId <> Nothing, "ShipmentId = " & DB.Number(ShipmentId), "ShipmentNo = " & DB.Quote(ShipmentNo))
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class StoreOrderShipmentCollection
		Inherits GenericCollection(Of StoreOrderShipmentRow)
	End Class

End Namespace


