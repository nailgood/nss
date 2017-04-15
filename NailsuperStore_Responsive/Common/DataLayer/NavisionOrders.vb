Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionOrdersRow
		Inherits NavisionOrdersRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
		Public Shared Function GetCollection(ByVal DB As Database) As NavisionOrdersCollection
            Dim r As SqlDataReader = Nothing
            Dim row As NavisionOrdersRow
			Dim c As New NavisionOrdersCollection
			Dim SQL As String = "select * from _NAVISION_ORDERS where Document_Type = 'Order' and ltrim(rtrim(Bill_To_Customer_No)) <> ''"
            Try
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionOrdersRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
			Return c
		End Function

	End Class

	Public MustInherit Class NavisionOrdersRowBase
		Private m_DB As Database
		Private m_Document_Type As String = Nothing
		Private m_Sell_To_Customer_No As String = Nothing
		Private m_Orders_No As String = Nothing
		Private m_Bill_to_Customer_No As String = Nothing
		Private m_Bill_to_Name As String = Nothing
		Private m_Bill_to_Name_2 As String = Nothing
		Private m_Bill_to_Address As String = Nothing
		Private m_Bill_to_Address_2 As String = Nothing
		Private m_Bill_to_City As String = Nothing
		Private m_Bill_to_Contact As String = Nothing
		Private m_Your_Reference As String = Nothing
		Private m_Ship_to_Code As String = Nothing
		Private m_Ship_to_Name As String = Nothing
		Private m_Ship_to_Name_2 As String = Nothing
		Private m_Ship_to_Address As String = Nothing
		Private m_Ship_to_Address_2 As String = Nothing
		Private m_Ship_to_City As String = Nothing
		Private m_Ship_to_Contact As String = Nothing
		Private m_Order_Date As DateTime = Nothing
		Private m_Posting_Date As DateTime = Nothing
		Private m_Shipment_Date As DateTime = Nothing
		Private m_Posting_Description As String = Nothing
		Private m_Payment_Terms_Code As String = Nothing
		Private m_Due_Date As DateTime = Nothing
		Private m_Payment_Discount_Percent As Double = Nothing
		Private m_Pmt_Discount_Date As DateTime = Nothing
		Private m_Shipment_Method_Code As String = Nothing
		Private m_Location_Code As String = Nothing
		Private m_Currency_Code As String = Nothing
		Private m_Customer_Price_Group As String = Nothing
		Private m_Prices_Including_VAT As String = Nothing
		Private m_Invoice_Disc_Code As String = Nothing
		Private m_Customer_Disc_Group As String = Nothing
		Private m_Language_Code As String = Nothing
		Private m_Salesperson_Code As String = Nothing
		Private m_Order_Class As String = Nothing
		Private m_Amount As Double = Nothing
		Private m_Amount_Including_VAT As Double = Nothing
		Private m_Shipping_No As String = Nothing
		Private m_Posting_No As String = Nothing
		Private m_Last_Shipping_No As String = Nothing
		Private m_Last_Posting_No As String = Nothing
		Private m_VAT_Registration_No As String = Nothing
		Private m_Combine_Shipments As String = Nothing
		Private m_Reason_Code As String = Nothing
		Private m_VAT_Country_Code As String = Nothing
		Private m_Sell_to_Customer_Name As String = Nothing
		Private m_Sell_to_Customer_Name_2 As String = Nothing
		Private m_Sell_to_Customer_Address As String = Nothing
		Private m_Sell_to_Customer_Address_2 As String = Nothing
		Private m_Sell_to_City As String = Nothing
		Private m_Sell_to_Contact As String = Nothing
		Private m_Bill_to_Post_Code As String = Nothing
		Private m_Bill_to_County As String = Nothing
		Private m_Bill_to_Country_Code As String = Nothing
		Private m_Sell_to_Post_Code As String = Nothing
		Private m_Sell_to_County As String = Nothing
		Private m_Sell_to_Country_Code As String = Nothing
		Private m_Ship_to_Post_Code As String = Nothing
		Private m_Ship_to_County As String = Nothing
		Private m_Ship_to_Country_Code As String = Nothing
		Private m_Document_Date As DateTime = Nothing
		Private m_External_Document_No As String = Nothing
		Private m_Payment_Method_Code As String = Nothing
		Private m_Shipping_Agent_Code As String = Nothing
		Private m_Package_Tracking_No As String = Nothing
		Private m_Tax_Area_Code As String = Nothing
		Private m_Tax_Liable As String = Nothing
		Private m_Status As String = Nothing
		Private m_Sell_to_Customer_Template_Code As String = Nothing
		Private m_Sell_to_Contact_No As String = Nothing
		Private m_Bill_to_Contact_No As String = Nothing
		Private m_Bill_to_Customer_Template_Code As String = Nothing
		Private m_Responsibility_Center As String = Nothing
		Private m_Shipping_Advice As String = Nothing
		Private m_Completely_Shipped As String = Nothing
		Private m_Requested_Delivery_Date As DateTime = Nothing
		Private m_Promised_Delivery_Date As DateTime = Nothing
		Private m_Shipping_Time As String = Nothing
		Private m_Outbound_Whse_Handling_Time As String = Nothing
		Private m_Shipping_Agent_Service_Code As String = Nothing
		Private m_Late_Order_Shipping As String = Nothing
		Private m_Auto_Created As String = Nothing
		Private m_Login_ID As String = Nothing
		Private m_Web_Site_Code As String = Nothing
		Private m_Date_Time_Received As String = Nothing
		Private m_Date_Time_Sent As String = Nothing
		Private m_WebReferenceNo As String = Nothing
		Private m_ID As Integer = Nothing

		Public Property Document_Type() As String
			Get
				Return Trim(m_Document_Type)
			End Get
			Set(ByVal Value As String)
				m_Document_Type = Trim(Value)
			End Set
		End Property

		Public Property Sell_To_Customer_No() As String
			Get
				Return Trim(m_Sell_To_Customer_No)
			End Get
			Set(ByVal Value As String)
				m_Sell_To_Customer_No = Trim(Value)
			End Set
		End Property

		Public Property Orders_No() As String
			Get
				Return Trim(m_Orders_No)
			End Get
			Set(ByVal Value As String)
				m_Orders_No = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Customer_No() As String
			Get
				Return Trim(m_Bill_to_Customer_No)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Customer_No = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Name() As String
			Get
				Return Trim(m_Bill_to_Name)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Name = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Name_2() As String
			Get
				Return Trim(m_Bill_to_Name_2)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Name_2 = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Address() As String
			Get
				Return Trim(m_Bill_to_Address)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Address = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Address_2() As String
			Get
				Return Trim(m_Bill_to_Address_2)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Address_2 = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_City() As String
			Get
				Return Trim(m_Bill_to_City)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_City = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Contact() As String
			Get
				Return Trim(m_Bill_to_Contact)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Contact = Trim(Value)
			End Set
		End Property

		Public Property Your_Reference() As String
			Get
				Return Trim(m_Your_Reference)
			End Get
			Set(ByVal Value As String)
				m_Your_Reference = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Code() As String
			Get
				Return Trim(m_Ship_to_Code)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Code = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Name() As String
			Get
				Return Trim(m_Ship_to_Name)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Name = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Name_2() As String
			Get
				Return Trim(m_Ship_to_Name_2)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Name_2 = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Address() As String
			Get
				Return Trim(m_Ship_to_Address)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Address = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Address_2() As String
			Get
				Return Trim(m_Ship_to_Address_2)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Address_2 = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_City() As String
			Get
				Return Trim(m_Ship_to_City)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_City = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Contact() As String
			Get
				Return Trim(m_Ship_to_Contact)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Contact = Trim(Value)
			End Set
		End Property

		Public Property Order_Date() As DateTime
			Get
				Return Trim(m_Order_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Order_Date = Trim(Value)
			End Set
		End Property

		Public Property Posting_Date() As DateTime
			Get
				Return Trim(m_Posting_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Posting_Date = Trim(Value)
			End Set
		End Property

		Public Property Shipment_Date() As DateTime
			Get
				Return Trim(m_Shipment_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Shipment_Date = Trim(Value)
			End Set
		End Property

		Public Property Posting_Description() As String
			Get
				Return Trim(m_Posting_Description)
			End Get
			Set(ByVal Value As String)
				m_Posting_Description = Trim(Value)
			End Set
		End Property

		Public Property Payment_Terms_Code() As String
			Get
				Return Trim(m_Payment_Terms_Code)
			End Get
			Set(ByVal Value As String)
				m_Payment_Terms_Code = Trim(Value)
			End Set
		End Property

		Public Property Due_Date() As DateTime
			Get
				Return Trim(m_Due_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Due_Date = Trim(Value)
			End Set
		End Property

		Public Property Payment_Discount_Percent() As Double
			Get
				Return Trim(m_Payment_Discount_Percent)
			End Get
			Set(ByVal Value As Double)
				m_Payment_Discount_Percent = Trim(Value)
			End Set
		End Property

		Public Property Pmt_Discount_Date() As DateTime
			Get
				Return Trim(m_Pmt_Discount_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Pmt_Discount_Date = Trim(Value)
			End Set
		End Property

		Public Property Shipment_Method_Code() As String
			Get
				Return Trim(m_Shipment_Method_Code)
			End Get
			Set(ByVal Value As String)
				m_Shipment_Method_Code = Trim(Value)
			End Set
		End Property

		Public Property Location_Code() As String
			Get
				Return Trim(m_Location_Code)
			End Get
			Set(ByVal Value As String)
				m_Location_Code = Trim(Value)
			End Set
		End Property

		Public Property Currency_Code() As String
			Get
				Return Trim(m_Currency_Code)
			End Get
			Set(ByVal Value As String)
				m_Currency_Code = Trim(Value)
			End Set
		End Property

		Public Property Customer_Price_Group() As String
			Get
				Return Trim(m_Customer_Price_Group)
			End Get
			Set(ByVal Value As String)
				m_Customer_Price_Group = Trim(Value)
			End Set
		End Property

		Public Property Prices_Including_VAT() As String
			Get
				Return Trim(m_Prices_Including_VAT)
			End Get
			Set(ByVal Value As String)
				m_Prices_Including_VAT = Trim(Value)
			End Set
		End Property

		Public Property Invoice_Disc_Code() As String
			Get
				Return Trim(m_Invoice_Disc_Code)
			End Get
			Set(ByVal Value As String)
				m_Invoice_Disc_Code = Trim(Value)
			End Set
		End Property

		Public Property Customer_Disc_Group() As String
			Get
				Return Trim(m_Customer_Disc_Group)
			End Get
			Set(ByVal Value As String)
				m_Customer_Disc_Group = Trim(Value)
			End Set
		End Property

		Public Property Language_Code() As String
			Get
				Return Trim(m_Language_Code)
			End Get
			Set(ByVal Value As String)
				m_Language_Code = Trim(Value)
			End Set
		End Property

		Public Property Salesperson_Code() As String
			Get
				Return Trim(m_Salesperson_Code)
			End Get
			Set(ByVal Value As String)
				m_Salesperson_Code = Trim(Value)
			End Set
		End Property

		Public Property Order_Class() As String
			Get
				Return Trim(m_Order_Class)
			End Get
			Set(ByVal Value As String)
				m_Order_Class = Trim(Value)
			End Set
		End Property

		Public Property Amount() As Double
			Get
				Return Trim(m_Amount)
			End Get
			Set(ByVal Value As Double)
				m_Amount = Trim(Value)
			End Set
		End Property

		Public Property Amount_Including_VAT() As Double
			Get
				Return Trim(m_Amount_Including_VAT)
			End Get
			Set(ByVal Value As Double)
				m_Amount_Including_VAT = Trim(Value)
			End Set
		End Property

		Public Property Shipping_No() As String
			Get
				Return Trim(m_Shipping_No)
			End Get
			Set(ByVal Value As String)
				m_Shipping_No = Trim(Value)
			End Set
		End Property

		Public Property Posting_No() As String
			Get
				Return Trim(m_Posting_No)
			End Get
			Set(ByVal Value As String)
				m_Posting_No = Trim(Value)
			End Set
		End Property

		Public Property Last_Shipping_No() As String
			Get
				Return Trim(m_Last_Shipping_No)
			End Get
			Set(ByVal Value As String)
				m_Last_Shipping_No = Trim(Value)
			End Set
		End Property

		Public Property Last_Posting_No() As String
			Get
				Return Trim(m_Last_Posting_No)
			End Get
			Set(ByVal Value As String)
				m_Last_Posting_No = Trim(Value)
			End Set
		End Property

		Public Property VAT_Registration_No() As String
			Get
				Return Trim(m_VAT_Registration_No)
			End Get
			Set(ByVal Value As String)
				m_VAT_Registration_No = Trim(Value)
			End Set
		End Property

		Public Property Combine_Shipments() As String
			Get
				Return Trim(m_Combine_Shipments)
			End Get
			Set(ByVal Value As String)
				m_Combine_Shipments = Trim(Value)
			End Set
		End Property

		Public Property Reason_Code() As String
			Get
				Return Trim(m_Reason_Code)
			End Get
			Set(ByVal Value As String)
				m_Reason_Code = Trim(Value)
			End Set
		End Property

		Public Property VAT_Country_Code() As String
			Get
				Return Trim(m_VAT_Country_Code)
			End Get
			Set(ByVal Value As String)
				m_VAT_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Customer_Name() As String
			Get
				Return Trim(m_Sell_to_Customer_Name)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Customer_Name = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Customer_Name_2() As String
			Get
				Return Trim(m_Sell_to_Customer_Name_2)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Customer_Name_2 = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Customer_Address() As String
			Get
				Return Trim(m_Sell_to_Customer_Address)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Customer_Address = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Customer_Address_2() As String
			Get
				Return Trim(m_Sell_to_Customer_Address_2)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Customer_Address_2 = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_City() As String
			Get
				Return Trim(m_Sell_to_City)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_City = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Contact() As String
			Get
				Return Trim(m_Sell_to_Contact)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Contact = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Post_Code() As String
			Get
				Return Trim(m_Bill_to_Post_Code)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Post_Code = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_County() As String
			Get
				Return Trim(m_Bill_to_County)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_County = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Country_Code() As String
			Get
				Return Trim(m_Bill_to_Country_Code)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Post_Code() As String
			Get
				Return Trim(m_Sell_to_Post_Code)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Post_Code = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_County() As String
			Get
				Return Trim(m_Sell_to_County)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_County = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Country_Code() As String
			Get
				Return Trim(m_Sell_to_Country_Code)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Post_Code() As String
			Get
				Return Trim(m_Ship_to_Post_Code)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Post_Code = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_County() As String
			Get
				Return Trim(m_Ship_to_County)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_County = Trim(Value)
			End Set
		End Property

		Public Property Ship_to_Country_Code() As String
			Get
				Return Trim(m_Ship_to_Country_Code)
			End Get
			Set(ByVal Value As String)
				m_Ship_to_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Document_Date() As DateTime
			Get
				Return Trim(m_Document_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Document_Date = Trim(Value)
			End Set
		End Property

		Public Property External_Document_No() As String
			Get
				Return Trim(m_External_Document_No)
			End Get
			Set(ByVal Value As String)
				m_External_Document_No = Trim(Value)
			End Set
		End Property

		Public Property Payment_Method_Code() As String
			Get
				Return Trim(m_Payment_Method_Code)
			End Get
			Set(ByVal Value As String)
				m_Payment_Method_Code = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Agent_Code() As String
			Get
				Return Trim(m_Shipping_Agent_Code)
			End Get
			Set(ByVal Value As String)
				m_Shipping_Agent_Code = Trim(Value)
			End Set
		End Property

		Public Property Package_Tracking_No() As String
			Get
				Return Trim(m_Package_Tracking_No)
			End Get
			Set(ByVal Value As String)
				m_Package_Tracking_No = Trim(Value)
			End Set
		End Property

		Public Property Tax_Area_Code() As String
			Get
				Return Trim(m_Tax_Area_Code)
			End Get
			Set(ByVal Value As String)
				m_Tax_Area_Code = Trim(Value)
			End Set
		End Property

		Public Property Tax_Liable() As String
			Get
				Return Trim(m_Tax_Liable)
			End Get
			Set(ByVal Value As String)
				m_Tax_Liable = Trim(Value)
			End Set
		End Property

		Public Property Status() As String
			Get
				Return Trim(m_Status)
			End Get
			Set(ByVal Value As String)
				m_Status = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Customer_Template_Code() As String
			Get
				Return Trim(m_Sell_to_Customer_Template_Code)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Customer_Template_Code = Trim(Value)
			End Set
		End Property

		Public Property Sell_to_Contact_No() As String
			Get
				Return Trim(m_Sell_to_Contact_No)
			End Get
			Set(ByVal Value As String)
				m_Sell_to_Contact_No = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Contact_No() As String
			Get
				Return Trim(m_Bill_to_Contact_No)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Contact_No = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Customer_Template_Code() As String
			Get
				Return Trim(m_Bill_to_Customer_Template_Code)
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Customer_Template_Code = Trim(Value)
			End Set
		End Property

		Public Property Responsibility_Center() As String
			Get
				Return Trim(m_Responsibility_Center)
			End Get
			Set(ByVal Value As String)
				m_Responsibility_Center = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Advice() As String
			Get
				Return Trim(m_Shipping_Advice)
			End Get
			Set(ByVal Value As String)
				m_Shipping_Advice = Trim(Value)
			End Set
		End Property

		Public Property Completely_Shipped() As String
			Get
				Return Trim(m_Completely_Shipped)
			End Get
			Set(ByVal Value As String)
				m_Completely_Shipped = Trim(Value)
			End Set
		End Property

		Public Property Requested_Delivery_Date() As DateTime
			Get
				Return Trim(m_Requested_Delivery_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Requested_Delivery_Date = Trim(Value)
			End Set
		End Property

		Public Property Promised_Delivery_Date() As DateTime
			Get
				Return Trim(m_Promised_Delivery_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Promised_Delivery_Date = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Time() As String
			Get
				Return Trim(m_Shipping_Time)
			End Get
			Set(ByVal Value As String)
				m_Shipping_Time = Trim(Value)
			End Set
		End Property

		Public Property Outbound_Whse_Handling_Time() As String
			Get
				Return Trim(m_Outbound_Whse_Handling_Time)
			End Get
			Set(ByVal Value As String)
				m_Outbound_Whse_Handling_Time = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Agent_Service_Code() As String
			Get
				Return Trim(m_Shipping_Agent_Service_Code)
			End Get
			Set(ByVal Value As String)
				m_Shipping_Agent_Service_Code = Trim(Value)
			End Set
		End Property

		Public Property Late_Order_Shipping() As String
			Get
				Return Trim(m_Late_Order_Shipping)
			End Get
			Set(ByVal Value As String)
				m_Late_Order_Shipping = Trim(Value)
			End Set
		End Property

		Public Property Auto_Created() As String
			Get
				Return Trim(m_Auto_Created)
			End Get
			Set(ByVal Value As String)
				m_Auto_Created = Trim(Value)
			End Set
		End Property

		Public Property Login_ID() As String
			Get
				Return Trim(m_Login_ID)
			End Get
			Set(ByVal Value As String)
				m_Login_ID = Trim(Value)
			End Set
		End Property

		Public Property Web_Site_Code() As String
			Get
				Return Trim(m_Web_Site_Code)
			End Get
			Set(ByVal Value As String)
				m_Web_Site_Code = Trim(Value)
			End Set
		End Property

		Public Property Date_Time_Received() As String
			Get
				Return Trim(m_Date_Time_Received)
			End Get
			Set(ByVal Value As String)
				m_Date_Time_Received = Trim(Value)
			End Set
		End Property

		Public Property Date_Time_Sent() As String
			Get
				Return Trim(m_Date_Time_Sent)
			End Get
			Set(ByVal Value As String)
				m_Date_Time_Sent = Trim(Value)
			End Set
		End Property

		Public Property WebReferenceNo() As String
			Get
				Return Trim(m_WebReferenceNo)
			End Get
			Set(ByVal Value As String)
				m_WebReferenceNo = Trim(Value)
			End Set
		End Property

		Public Property ID() As Integer
			Get
				Return m_ID
			End Get
			Set(ByVal Value As Integer)
				m_ID = value
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

		Protected Overridable Sub Load(ByVal r As SqlDataReader)
			If IsDBNull(r.Item("Document_Type")) Then
				m_Document_Type = Nothing
			Else
				m_Document_Type = Convert.ToString(r.Item("Document_Type"))
			End If
			If IsDBNull(r.Item("Sell_To_Customer_No")) Then
				m_Sell_To_Customer_No = Nothing
			Else
				m_Sell_To_Customer_No = Convert.ToString(r.Item("Sell_To_Customer_No"))
			End If
			If IsDBNull(r.Item("Orders_No")) Then
				m_Orders_No = Nothing
			Else
				m_Orders_No = Convert.ToString(r.Item("Orders_No"))
			End If
			If IsDBNull(r.Item("Bill_to_Customer_No")) Then
				m_Bill_to_Customer_No = Nothing
			Else
				m_Bill_to_Customer_No = Convert.ToString(r.Item("Bill_to_Customer_No"))
			End If
			If IsDBNull(r.Item("Bill_to_Name")) Then
				m_Bill_to_Name = Nothing
			Else
				m_Bill_to_Name = Convert.ToString(r.Item("Bill_to_Name"))
			End If
			If IsDBNull(r.Item("Bill_to_Name_2")) Then
				m_Bill_to_Name_2 = Nothing
			Else
				m_Bill_to_Name_2 = Convert.ToString(r.Item("Bill_to_Name_2"))
			End If
			If IsDBNull(r.Item("Bill_to_Address")) Then
				m_Bill_to_Address = Nothing
			Else
				m_Bill_to_Address = Convert.ToString(r.Item("Bill_to_Address"))
			End If
			If IsDBNull(r.Item("Bill_to_Address_2")) Then
				m_Bill_to_Address_2 = Nothing
			Else
				m_Bill_to_Address_2 = Convert.ToString(r.Item("Bill_to_Address_2"))
			End If
			If IsDBNull(r.Item("Bill_to_City")) Then
				m_Bill_to_City = Nothing
			Else
				m_Bill_to_City = Convert.ToString(r.Item("Bill_to_City"))
			End If
			If IsDBNull(r.Item("Bill_to_Contact")) Then
				m_Bill_to_Contact = Nothing
			Else
				m_Bill_to_Contact = Convert.ToString(r.Item("Bill_to_Contact"))
			End If
			If IsDBNull(r.Item("Your_Reference")) Then
				m_Your_Reference = Nothing
			Else
				m_Your_Reference = Convert.ToString(r.Item("Your_Reference"))
			End If
			If IsDBNull(r.Item("Ship_to_Code")) Then
				m_Ship_to_Code = Nothing
			Else
				m_Ship_to_Code = Convert.ToString(r.Item("Ship_to_Code"))
			End If
			If IsDBNull(r.Item("Ship_to_Name")) Then
				m_Ship_to_Name = Nothing
			Else
				m_Ship_to_Name = Convert.ToString(r.Item("Ship_to_Name"))
			End If
			If IsDBNull(r.Item("Ship_to_Name_2")) Then
				m_Ship_to_Name_2 = Nothing
			Else
				m_Ship_to_Name_2 = Convert.ToString(r.Item("Ship_to_Name_2"))
			End If
			If IsDBNull(r.Item("Ship_to_Address")) Then
				m_Ship_to_Address = Nothing
			Else
				m_Ship_to_Address = Convert.ToString(r.Item("Ship_to_Address"))
			End If
			If IsDBNull(r.Item("Ship_to_Address_2")) Then
				m_Ship_to_Address_2 = Nothing
			Else
				m_Ship_to_Address_2 = Convert.ToString(r.Item("Ship_to_Address_2"))
			End If
			If IsDBNull(r.Item("Ship_to_City")) Then
				m_Ship_to_City = Nothing
			Else
				m_Ship_to_City = Convert.ToString(r.Item("Ship_to_City"))
			End If
			If IsDBNull(r.Item("Ship_to_Contact")) Then
				m_Ship_to_Contact = Nothing
			Else
				m_Ship_to_Contact = Convert.ToString(r.Item("Ship_to_Contact"))
			End If
			If IsDBNull(r.Item("Order_Date")) Then
				m_Order_Date = Nothing
			Else
				m_Order_Date = Convert.ToDateTime(r.Item("Order_Date"))
			End If
			If IsDBNull(r.Item("Posting_Date")) Then
				m_Posting_Date = Nothing
			Else
				m_Posting_Date = Convert.ToDateTime(r.Item("Posting_Date"))
			End If
			If IsDBNull(r.Item("Shipment_Date")) Then
				m_Shipment_Date = Nothing
			Else
				m_Shipment_Date = Convert.ToDateTime(r.Item("Shipment_Date"))
			End If
			If IsDBNull(r.Item("Posting_Description")) Then
				m_Posting_Description = Nothing
			Else
				m_Posting_Description = Convert.ToString(r.Item("Posting_Description"))
			End If
			If IsDBNull(r.Item("Payment_Terms_Code")) Then
				m_Payment_Terms_Code = Nothing
			Else
				m_Payment_Terms_Code = Convert.ToString(r.Item("Payment_Terms_Code"))
			End If
			If IsDBNull(r.Item("Due_Date")) Then
				m_Due_Date = Nothing
			Else
				m_Due_Date = Convert.ToDateTime(r.Item("Due_Date"))
			End If
			If IsDBNull(r.Item("Payment_Discount_Percent")) Then
				m_Payment_Discount_Percent = Nothing
			Else
				m_Payment_Discount_Percent = Convert.ToDouble(r.Item("Payment_Discount_Percent"))
			End If
			If IsDBNull(r.Item("Pmt_Discount_Date")) Then
				m_Pmt_Discount_Date = Nothing
			Else
				m_Pmt_Discount_Date = Convert.ToDateTime(r.Item("Pmt_Discount_Date"))
			End If
			If IsDBNull(r.Item("Shipment_Method_Code")) Then
				m_Shipment_Method_Code = Nothing
			Else
				m_Shipment_Method_Code = Convert.ToString(r.Item("Shipment_Method_Code"))
			End If
			If IsDBNull(r.Item("Location_Code")) Then
				m_Location_Code = Nothing
			Else
				m_Location_Code = Convert.ToString(r.Item("Location_Code"))
			End If
			If IsDBNull(r.Item("Currency_Code")) Then
				m_Currency_Code = Nothing
			Else
				m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
			End If
			If IsDBNull(r.Item("Customer_Price_Group")) Then
				m_Customer_Price_Group = Nothing
			Else
				m_Customer_Price_Group = Convert.ToString(r.Item("Customer_Price_Group"))
			End If
			If IsDBNull(r.Item("Prices_Including_VAT")) Then
				m_Prices_Including_VAT = Nothing
			Else
				m_Prices_Including_VAT = Convert.ToString(r.Item("Prices_Including_VAT"))
			End If
			If IsDBNull(r.Item("Invoice_Disc_Code")) Then
				m_Invoice_Disc_Code = Nothing
			Else
				m_Invoice_Disc_Code = Convert.ToString(r.Item("Invoice_Disc_Code"))
			End If
			If IsDBNull(r.Item("Customer_Disc_Group")) Then
				m_Customer_Disc_Group = Nothing
			Else
				m_Customer_Disc_Group = Convert.ToString(r.Item("Customer_Disc_Group"))
			End If
			If IsDBNull(r.Item("Language_Code")) Then
				m_Language_Code = Nothing
			Else
				m_Language_Code = Convert.ToString(r.Item("Language_Code"))
			End If
			If IsDBNull(r.Item("Salesperson_Code")) Then
				m_Salesperson_Code = Nothing
			Else
				m_Salesperson_Code = Convert.ToString(r.Item("Salesperson_Code"))
			End If
			If IsDBNull(r.Item("Order_Class")) Then
				m_Order_Class = Nothing
			Else
				m_Order_Class = Convert.ToString(r.Item("Order_Class"))
			End If
			If IsDBNull(r.Item("Amount")) Then
				m_Amount = Nothing
			Else
				m_Amount = Convert.ToDouble(r.Item("Amount"))
			End If
			If IsDBNull(r.Item("Amount_Including_VAT")) Then
				m_Amount_Including_VAT = Nothing
			Else
				m_Amount_Including_VAT = Convert.ToDouble(r.Item("Amount_Including_VAT"))
			End If
			If IsDBNull(r.Item("Shipping_No")) Then
				m_Shipping_No = Nothing
			Else
				m_Shipping_No = Convert.ToString(r.Item("Shipping_No"))
			End If
			If IsDBNull(r.Item("Posting_No")) Then
				m_Posting_No = Nothing
			Else
				m_Posting_No = Convert.ToString(r.Item("Posting_No"))
			End If
			If IsDBNull(r.Item("Last_Shipping_No")) Then
				m_Last_Shipping_No = Nothing
			Else
				m_Last_Shipping_No = Convert.ToString(r.Item("Last_Shipping_No"))
			End If
			If IsDBNull(r.Item("Last_Posting_No")) Then
				m_Last_Posting_No = Nothing
			Else
				m_Last_Posting_No = Convert.ToString(r.Item("Last_Posting_No"))
			End If
			If IsDBNull(r.Item("VAT_Registration_No")) Then
				m_VAT_Registration_No = Nothing
			Else
				m_VAT_Registration_No = Convert.ToString(r.Item("VAT_Registration_No"))
			End If
			If IsDBNull(r.Item("Combine_Shipments")) Then
				m_Combine_Shipments = Nothing
			Else
				m_Combine_Shipments = Convert.ToString(r.Item("Combine_Shipments"))
			End If
			If IsDBNull(r.Item("Reason_Code")) Then
				m_Reason_Code = Nothing
			Else
				m_Reason_Code = Convert.ToString(r.Item("Reason_Code"))
			End If
			If IsDBNull(r.Item("VAT_Country_Code")) Then
				m_VAT_Country_Code = Nothing
			Else
				m_VAT_Country_Code = Convert.ToString(r.Item("VAT_Country_Code"))
			End If
			If IsDBNull(r.Item("Sell_to_Customer_Name")) Then
				m_Sell_to_Customer_Name = Nothing
			Else
				m_Sell_to_Customer_Name = Convert.ToString(r.Item("Sell_to_Customer_Name"))
			End If
			If IsDBNull(r.Item("Sell_to_Customer_Name_2")) Then
				m_Sell_to_Customer_Name_2 = Nothing
			Else
				m_Sell_to_Customer_Name_2 = Convert.ToString(r.Item("Sell_to_Customer_Name_2"))
			End If
			If IsDBNull(r.Item("Sell_to_Customer_Address")) Then
				m_Sell_to_Customer_Address = Nothing
			Else
				m_Sell_to_Customer_Address = Convert.ToString(r.Item("Sell_to_Customer_Address"))
			End If
			If IsDBNull(r.Item("Sell_to_Customer_Address_2")) Then
				m_Sell_to_Customer_Address_2 = Nothing
			Else
				m_Sell_to_Customer_Address_2 = Convert.ToString(r.Item("Sell_to_Customer_Address_2"))
			End If
			If IsDBNull(r.Item("Sell_to_City")) Then
				m_Sell_to_City = Nothing
			Else
				m_Sell_to_City = Convert.ToString(r.Item("Sell_to_City"))
			End If
			If IsDBNull(r.Item("Sell_to_Contact")) Then
				m_Sell_to_Contact = Nothing
			Else
				m_Sell_to_Contact = Convert.ToString(r.Item("Sell_to_Contact"))
			End If
			If IsDBNull(r.Item("Bill_to_Post_Code")) Then
				m_Bill_to_Post_Code = Nothing
			Else
				m_Bill_to_Post_Code = Convert.ToString(r.Item("Bill_to_Post_Code"))
			End If
			If IsDBNull(r.Item("Bill_to_County")) Then
				m_Bill_to_County = Nothing
			Else
				m_Bill_to_County = Convert.ToString(r.Item("Bill_to_County"))
			End If
			If IsDBNull(r.Item("Bill_to_Country_Code")) Then
				m_Bill_to_Country_Code = Nothing
			Else
				m_Bill_to_Country_Code = Convert.ToString(r.Item("Bill_to_Country_Code"))
			End If
			If IsDBNull(r.Item("Sell_to_Post_Code")) Then
				m_Sell_to_Post_Code = Nothing
			Else
				m_Sell_to_Post_Code = Convert.ToString(r.Item("Sell_to_Post_Code"))
			End If
			If IsDBNull(r.Item("Sell_to_County")) Then
				m_Sell_to_County = Nothing
			Else
				m_Sell_to_County = Convert.ToString(r.Item("Sell_to_County"))
			End If
			If IsDBNull(r.Item("Sell_to_Country_Code")) Then
				m_Sell_to_Country_Code = Nothing
			Else
				m_Sell_to_Country_Code = Convert.ToString(r.Item("Sell_to_Country_Code"))
			End If
			If IsDBNull(r.Item("Ship_to_Post_Code")) Then
				m_Ship_to_Post_Code = Nothing
			Else
				m_Ship_to_Post_Code = Convert.ToString(r.Item("Ship_to_Post_Code"))
			End If
			If IsDBNull(r.Item("Ship_to_County")) Then
				m_Ship_to_County = Nothing
			Else
				m_Ship_to_County = Convert.ToString(r.Item("Ship_to_County"))
			End If
			If IsDBNull(r.Item("Ship_to_Country_Code")) Then
				m_Ship_to_Country_Code = Nothing
			Else
				m_Ship_to_Country_Code = Convert.ToString(r.Item("Ship_to_Country_Code"))
			End If
			If IsDBNull(r.Item("Document_Date")) Then
				m_Document_Date = Nothing
			Else
				m_Document_Date = Convert.ToDateTime(r.Item("Document_Date"))
			End If
			If IsDBNull(r.Item("External_Document_No")) Then
				m_External_Document_No = Nothing
			Else
				m_External_Document_No = Convert.ToString(r.Item("External_Document_No"))
			End If
			If IsDBNull(r.Item("Payment_Method_Code")) Then
				m_Payment_Method_Code = Nothing
			Else
				m_Payment_Method_Code = Convert.ToString(r.Item("Payment_Method_Code"))
			End If
			If IsDBNull(r.Item("Shipping_Agent_Code")) Then
				m_Shipping_Agent_Code = Nothing
			Else
				m_Shipping_Agent_Code = Convert.ToString(r.Item("Shipping_Agent_Code"))
			End If
			If IsDBNull(r.Item("Package_Tracking_No")) Then
				m_Package_Tracking_No = Nothing
			Else
				m_Package_Tracking_No = Convert.ToString(r.Item("Package_Tracking_No"))
			End If
			If IsDBNull(r.Item("Tax_Area_Code")) Then
				m_Tax_Area_Code = Nothing
			Else
				m_Tax_Area_Code = Convert.ToString(r.Item("Tax_Area_Code"))
			End If
			If IsDBNull(r.Item("Tax_Liable")) Then
				m_Tax_Liable = Nothing
			Else
				m_Tax_Liable = Convert.ToString(r.Item("Tax_Liable"))
			End If
			If IsDBNull(r.Item("Status")) Then
				m_Status = Nothing
			Else
				m_Status = Convert.ToString(r.Item("Status"))
			End If
			If IsDBNull(r.Item("Sell_to_Customer_Template_Code")) Then
				m_Sell_to_Customer_Template_Code = Nothing
			Else
				m_Sell_to_Customer_Template_Code = Convert.ToString(r.Item("Sell_to_Customer_Template_Code"))
			End If
			If IsDBNull(r.Item("Sell_to_Contact_No")) Then
				m_Sell_to_Contact_No = Nothing
			Else
				m_Sell_to_Contact_No = Convert.ToString(r.Item("Sell_to_Contact_No"))
			End If
			If IsDBNull(r.Item("Bill_to_Contact_No")) Then
				m_Bill_to_Contact_No = Nothing
			Else
				m_Bill_to_Contact_No = Convert.ToString(r.Item("Bill_to_Contact_No"))
			End If
			If IsDBNull(r.Item("Bill_to_Customer_Template_Code")) Then
				m_Bill_to_Customer_Template_Code = Nothing
			Else
				m_Bill_to_Customer_Template_Code = Convert.ToString(r.Item("Bill_to_Customer_Template_Code"))
			End If
			If IsDBNull(r.Item("Responsibility_Center")) Then
				m_Responsibility_Center = Nothing
			Else
				m_Responsibility_Center = Convert.ToString(r.Item("Responsibility_Center"))
			End If
			If IsDBNull(r.Item("Shipping_Advice")) Then
				m_Shipping_Advice = Nothing
			Else
				m_Shipping_Advice = Convert.ToString(r.Item("Shipping_Advice"))
			End If
			If IsDBNull(r.Item("Completely_Shipped")) Then
				m_Completely_Shipped = Nothing
			Else
				m_Completely_Shipped = Convert.ToString(r.Item("Completely_Shipped"))
			End If
			If IsDBNull(r.Item("Requested_Delivery_Date")) Then
				m_Requested_Delivery_Date = Nothing
			Else
				m_Requested_Delivery_Date = Convert.ToDateTime(r.Item("Requested_Delivery_Date"))
			End If
			If IsDBNull(r.Item("Promised_Delivery_Date")) Then
				m_Promised_Delivery_Date = Nothing
			Else
				m_Promised_Delivery_Date = Convert.ToDateTime(r.Item("Promised_Delivery_Date"))
			End If
			If IsDBNull(r.Item("Shipping_Time")) Then
				m_Shipping_Time = Nothing
			Else
				m_Shipping_Time = Convert.ToString(r.Item("Shipping_Time"))
			End If
			If IsDBNull(r.Item("Outbound_Whse_Handling_Time")) Then
				m_Outbound_Whse_Handling_Time = Nothing
			Else
				m_Outbound_Whse_Handling_Time = Convert.ToString(r.Item("Outbound_Whse_Handling_Time"))
			End If
			If IsDBNull(r.Item("Shipping_Agent_Service_Code")) Then
				m_Shipping_Agent_Service_Code = Nothing
			Else
				m_Shipping_Agent_Service_Code = Convert.ToString(r.Item("Shipping_Agent_Service_Code"))
			End If
			If IsDBNull(r.Item("Late_Order_Shipping")) Then
				m_Late_Order_Shipping = Nothing
			Else
				m_Late_Order_Shipping = Convert.ToString(r.Item("Late_Order_Shipping"))
			End If
			If IsDBNull(r.Item("Auto_Created")) Then
				m_Auto_Created = Nothing
			Else
				m_Auto_Created = Convert.ToString(r.Item("Auto_Created"))
			End If
			If IsDBNull(r.Item("Login_ID")) Then
				m_Login_ID = Nothing
			Else
				m_Login_ID = Convert.ToString(r.Item("Login_ID"))
			End If
			If IsDBNull(r.Item("Web_Site_Code")) Then
				m_Web_Site_Code = Nothing
			Else
				m_Web_Site_Code = Convert.ToString(r.Item("Web_Site_Code"))
			End If
			If IsDBNull(r.Item("Date_Time_Received")) Then
				m_Date_Time_Received = Nothing
			Else
				m_Date_Time_Received = Convert.ToString(r.Item("Date_Time_Received"))
			End If
			If IsDBNull(r.Item("Date_Time_Sent")) Then
				m_Date_Time_Sent = Nothing
			Else
				m_Date_Time_Sent = Convert.ToString(r.Item("Date_Time_Sent"))
			End If
			If IsDBNull(r.Item("WebReferenceNo")) OrElse Not IsNumeric(r.Item("WebReferenceNo")) Then
				m_WebReferenceNo = Nothing
			Else
				m_WebReferenceNo = Convert.ToString(r.Item("WebReferenceNo"))
			End If
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionOrdersCollection
		Inherits GenericCollection(Of NavisionOrdersRow)
	End Class

End Namespace


