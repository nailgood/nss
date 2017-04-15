Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionCustomerRow
		Inherits NavisionCustomerRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionCustomerCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionCustomerRow
                Dim c As New NavisionCustomerCollection
                Dim SQL As String = "select * from _NAVISION_CUSTOMER"

                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionCustomerRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)

                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionCustomerCollection
        End Function

	End Class

	Public MustInherit Class NavisionCustomerRowBase
		Private m_DB As Database
		Private m_Cust_No As String = Nothing
		Private m_Name As String = Nothing
		Private m_Name_2 As String = Nothing
		Private m_Address As String = Nothing
		Private m_Address_2 As String = Nothing
		Private m_City As String = Nothing
		Private m_Contact As String = Nothing
		Private m_Phone_No As String = Nothing
		Private m_Our_Account_No As String = Nothing
		Private m_Territor_Code As String = Nothing
		Private m_Chain_Name As String = Nothing
		Private m_Budget_Amount As Double = Nothing
		Private m_Credit_Limit_LCY As Double = Nothing
		Private m_Currency_Code As String = Nothing
		Private m_Customer_Price_Group As String = Nothing
		Private m_Language_Code As String = Nothing
		Private m_Payment_Terms_Code As String = Nothing
		Private m_Salesperson_Code As String = Nothing
		Private m_Shipment_Method_Code As String = Nothing
		Private m_Shipping_Agent_Code As String = Nothing
		Private m_Customer_Disc_Group As String = Nothing
		Private m_Country_Code As String = Nothing
		Private m_Blocked As String = Nothing
		Private m_Invoice_Copies As Integer = Nothing
		Private m_Last_Statement_No As Integer = Nothing
		Private m_Print_Statements As String = Nothing
		Private m_Bill_to_Customer_No As String = Nothing
		Private m_Priority As Integer = Nothing
		Private m_Payment_Method_Code As String = Nothing
		Private m_Last_Date_Modified As DateTime = Nothing
		Private m_Balance_LCY As Double = Nothing
		Private m_Sales_LCY As Double = Nothing
		Private m_Balance_Due_LCY As Double = Nothing
		Private m_Shipped_Not_Invoiced As Double = Nothing
		Private m_Application_Method As String = Nothing
		Private m_Location_Code As String = Nothing
		Private m_Fax_No As String = Nothing
		Private m_VAT_Registration_No As String = Nothing
		Private m_Combine_Shipments As String = Nothing
		Private m_Post_Code As String = Nothing
		Private m_County_Text As String = Nothing
		Private m_Email_Text As String = Nothing
		Private m_Home_Page_Text As String = Nothing
		Private m_Outstanding_Orders_LCY As Double = Nothing
		Private m_Shipped_Not_Invoiced_LCY As Double = Nothing
		Private m_Primary_Contact_No As String = Nothing
		Private m_Responsibility_Center As String = Nothing
		Private m_Shipping_Advice As String = Nothing
		Private m_Shipping_Time As String = Nothing
		Private m_Shipping_Agent As String = Nothing
		Private m_WebReferenceNo As Integer = Nothing
		Private m_Customer_Posting_Group As String = Nothing
		Private m_ID As Integer = Nothing

		Public Property Customer_Posting_Group() As String
			Get
				Return m_Customer_Posting_Group
			End Get
			Set(ByVal value As String)
				m_Customer_Posting_Group = value
			End Set
		End Property

		Public Property Cust_No() As String
			Get
				Return m_Cust_No
			End Get
			Set(ByVal Value As String)
				m_Cust_No = Trim(Value)
			End Set
		End Property

		Public Property Name() As String
			Get
				Return m_Name
			End Get
			Set(ByVal Value As String)
				m_Name = Trim(Value)
			End Set
		End Property

		Public Property Name_2() As String
			Get
				Return m_Name_2
			End Get
			Set(ByVal Value As String)
				m_Name_2 = Trim(Value)
			End Set
		End Property

		Public Property Address() As String
			Get
				Return m_Address
			End Get
			Set(ByVal Value As String)
				m_Address = Trim(Value)
			End Set
		End Property

		Public Property Address_2() As String
			Get
				Return m_Address_2
			End Get
			Set(ByVal Value As String)
				m_Address_2 = Trim(Value)
			End Set
		End Property

		Public Property City() As String
			Get
				Return m_City
			End Get
			Set(ByVal Value As String)
				m_City = Trim(Value)
			End Set
		End Property

		Public Property Contact() As String
			Get
				Return m_Contact
			End Get
			Set(ByVal Value As String)
				m_Contact = Trim(Value)
			End Set
		End Property

		Public Property Phone_No() As String
			Get
				Return m_Phone_No
			End Get
			Set(ByVal Value As String)
				m_Phone_No = Trim(Value)
			End Set
		End Property

		Public Property Our_Account_No() As String
			Get
				Return m_Our_Account_No
			End Get
			Set(ByVal Value As String)
				m_Our_Account_No = Trim(Value)
			End Set
		End Property

		Public Property Territor_Code() As String
			Get
				Return m_Territor_Code
			End Get
			Set(ByVal Value As String)
				m_Territor_Code = Trim(Value)
			End Set
		End Property

		Public Property Chain_Name() As String
			Get
				Return m_Chain_Name
			End Get
			Set(ByVal Value As String)
				m_Chain_Name = Trim(Value)
			End Set
		End Property

		Public Property Budget_Amount() As Double
			Get
				Return m_Budget_Amount
			End Get
			Set(ByVal Value As Double)
				m_Budget_Amount = Trim(Value)
			End Set
		End Property

		Public Property Credit_Limit_LCY() As Double
			Get
				Return m_Credit_Limit_LCY
			End Get
			Set(ByVal Value As Double)
				m_Credit_Limit_LCY = Trim(Value)
			End Set
		End Property

		Public Property Currency_Code() As String
			Get
				Return m_Currency_Code
			End Get
			Set(ByVal Value As String)
				m_Currency_Code = Trim(Value)
			End Set
		End Property

		Public Property Customer_Price_Group() As String
			Get
				Return m_Customer_Price_Group
			End Get
			Set(ByVal Value As String)
				m_Customer_Price_Group = Trim(Value)
			End Set
		End Property

		Public Property Language_Code() As String
			Get
				Return m_Language_Code
			End Get
			Set(ByVal Value As String)
				m_Language_Code = Trim(Value)
			End Set
		End Property

		Public Property Payment_Terms_Code() As String
			Get
				Return m_Payment_Terms_Code
			End Get
			Set(ByVal Value As String)
				m_Payment_Terms_Code = Trim(Value)
			End Set
		End Property

		Public Property Salesperson_Code() As String
			Get
				Return m_Salesperson_Code
			End Get
			Set(ByVal Value As String)
				m_Salesperson_Code = Trim(Value)
			End Set
		End Property

		Public Property Shipment_Method_Code() As String
			Get
				Return m_Shipment_Method_Code
			End Get
			Set(ByVal Value As String)
				m_Shipment_Method_Code = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Agent_Code() As String
			Get
				Return m_Shipping_Agent_Code
			End Get
			Set(ByVal Value As String)
				m_Shipping_Agent_Code = Trim(Value)
			End Set
		End Property

		Public Property Customer_Disc_Group() As String
			Get
				Return m_Customer_Disc_Group
			End Get
			Set(ByVal Value As String)
				m_Customer_Disc_Group = Trim(Value)
			End Set
		End Property

		Public Property Country_Code() As String
			Get
				Return m_Country_Code
			End Get
			Set(ByVal Value As String)
				m_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Blocked() As String
			Get
				Return m_Blocked
			End Get
			Set(ByVal Value As String)
				m_Blocked = Trim(Value)
			End Set
		End Property

		Public Property Invoice_Copies() As Integer
			Get
				Return m_Invoice_Copies
			End Get
			Set(ByVal Value As Integer)
				m_Invoice_Copies = Trim(Value)
			End Set
		End Property

		Public Property Last_Statement_No() As Integer
			Get
				Return m_Last_Statement_No
			End Get
			Set(ByVal Value As Integer)
				m_Last_Statement_No = Trim(Value)
			End Set
		End Property

		Public Property Print_Statements() As String
			Get
				Return m_Print_Statements
			End Get
			Set(ByVal Value As String)
				m_Print_Statements = Trim(Value)
			End Set
		End Property

		Public Property Bill_to_Customer_No() As String
			Get
				Return m_Bill_to_Customer_No
			End Get
			Set(ByVal Value As String)
				m_Bill_to_Customer_No = Trim(Value)
			End Set
		End Property

		Public Property Priority() As Integer
			Get
				Return m_Priority
			End Get
			Set(ByVal Value As Integer)
				m_Priority = Trim(Value)
			End Set
		End Property

		Public Property Payment_Method_Code() As String
			Get
				Return m_Payment_Method_Code
			End Get
			Set(ByVal Value As String)
				m_Payment_Method_Code = Trim(Value)
			End Set
		End Property

		Public Property Last_Date_Modified() As DateTime
			Get
				Return m_Last_Date_Modified
			End Get
			Set(ByVal Value As DateTime)
				m_Last_Date_Modified = Trim(Value)
			End Set
		End Property

		Public Property Balance_LCY() As Double
			Get
				Return m_Balance_LCY
			End Get
			Set(ByVal Value As Double)
				m_Balance_LCY = Trim(Value)
			End Set
		End Property

		Public Property Sales_LCY() As Double
			Get
				Return m_Sales_LCY
			End Get
			Set(ByVal Value As Double)
				m_Sales_LCY = Trim(Value)
			End Set
		End Property

		Public Property Balance_Due_LCY() As Double
			Get
				Return m_Balance_Due_LCY
			End Get
			Set(ByVal Value As Double)
				m_Balance_Due_LCY = Trim(Value)
			End Set
		End Property

		Public Property Shipped_Not_Invoiced() As Double
			Get
				Return m_Shipped_Not_Invoiced
			End Get
			Set(ByVal Value As Double)
				m_Shipped_Not_Invoiced = Trim(Value)
			End Set
		End Property

		Public Property Application_Method() As String
			Get
				Return m_Application_Method
			End Get
			Set(ByVal Value As String)
				m_Application_Method = Trim(Value)
			End Set
		End Property

		Public Property Location_Code() As String
			Get
				Return m_Location_Code
			End Get
			Set(ByVal Value As String)
				m_Location_Code = Trim(Value)
			End Set
		End Property

		Public Property Fax_No() As String
			Get
				Return m_Fax_No
			End Get
			Set(ByVal Value As String)
				m_Fax_No = Trim(Value)
			End Set
		End Property

		Public Property VAT_Registration_No() As String
			Get
				Return m_VAT_Registration_No
			End Get
			Set(ByVal Value As String)
				m_VAT_Registration_No = Trim(Value)
			End Set
		End Property

		Public Property Combine_Shipments() As String
			Get
				Return m_Combine_Shipments
			End Get
			Set(ByVal Value As String)
				m_Combine_Shipments = Trim(Value)
			End Set
		End Property

		Public Property Post_Code() As String
			Get
				Return m_Post_Code
			End Get
			Set(ByVal Value As String)
				m_Post_Code = Trim(Value)
			End Set
		End Property

		Public Property County_Text() As String
			Get
				Return m_County_Text
			End Get
			Set(ByVal Value As String)
				m_County_Text = Trim(Value)
			End Set
		End Property

		Public Property Email_Text() As String
			Get
				Return m_Email_Text
			End Get
			Set(ByVal Value As String)
				m_Email_Text = Trim(Value)
			End Set
		End Property

		Public Property Home_Page_Text() As String
			Get
				Return m_Home_Page_Text
			End Get
			Set(ByVal Value As String)
				m_Home_Page_Text = Trim(Value)
			End Set
		End Property

		Public Property Outstanding_Orders_LCY() As Double
			Get
				Return m_Outstanding_Orders_LCY
			End Get
			Set(ByVal Value As Double)
				m_Outstanding_Orders_LCY = Trim(Value)
			End Set
		End Property

		Public Property Shipped_Not_Invoiced_LCY() As Double
			Get
				Return m_Shipped_Not_Invoiced_LCY
			End Get
			Set(ByVal Value As Double)
				m_Shipped_Not_Invoiced_LCY = Trim(Value)
			End Set
		End Property

		Public Property Primary_Contact_No() As String
			Get
				Return m_Primary_Contact_No
			End Get
			Set(ByVal Value As String)
				m_Primary_Contact_No = Trim(Value)
			End Set
		End Property

		Public Property Responsibility_Center() As String
			Get
				Return m_Responsibility_Center
			End Get
			Set(ByVal Value As String)
				m_Responsibility_Center = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Advice() As String
			Get
				Return m_Shipping_Advice
			End Get
			Set(ByVal Value As String)
				m_Shipping_Advice = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Time() As String
			Get
				Return m_Shipping_Time
			End Get
			Set(ByVal Value As String)
				m_Shipping_Time = Trim(Value)
			End Set
		End Property

		Public Property Shipping_Agent() As String
			Get
				Return m_Shipping_Agent
			End Get
			Set(ByVal Value As String)
				m_Shipping_Agent = Trim(Value)
			End Set
		End Property

		Public Property WebReferenceNo() As Integer
			Get
				Return m_WebReferenceNo
			End Get
			Set(ByVal Value As Integer)
				m_WebReferenceNo = Trim(Value)
			End Set
		End Property

		Public Property ID() As Integer
			Get
				Return m_ID
			End Get
			Set(ByVal Value As Integer)
				m_ID = Value
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

		Public Sub New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			m_DB = DB
		End Sub	'New

		Protected Overridable Sub Load(ByVal r As SqlDataReader)
			m_Cust_No = Convert.ToString(r.Item("Cust_No"))
			If IsDBNull(r.Item("Name")) Then
				m_Name = Nothing
			Else
				m_Name = Convert.ToString(r.Item("Name"))
			End If
			If IsDBNull(r.Item("Name_2")) Then
				m_Name_2 = Nothing
			Else
				m_Name_2 = Convert.ToString(r.Item("Name_2"))
			End If
			If IsDBNull(r.Item("Address")) Then
				m_Address = Nothing
			Else
				m_Address = Convert.ToString(r.Item("Address"))
			End If
			If IsDBNull(r.Item("Address_2")) Then
				m_Address_2 = Nothing
			Else
				m_Address_2 = Convert.ToString(r.Item("Address_2"))
			End If
			If IsDBNull(r.Item("City")) Then
				m_City = Nothing
			Else
				m_City = Convert.ToString(r.Item("City"))
			End If
			If IsDBNull(r.Item("Contact")) Then
				m_Contact = Nothing
			Else
				m_Contact = Convert.ToString(r.Item("Contact"))
			End If
			If IsDBNull(r.Item("Phone_No")) Then
				m_Phone_No = Nothing
			Else
				m_Phone_No = Convert.ToString(r.Item("Phone_No"))
			End If
			If IsDBNull(r.Item("Our_Account_No")) Then
				m_Our_Account_No = Nothing
			Else
				m_Our_Account_No = Convert.ToString(r.Item("Our_Account_No"))
			End If
			If IsDBNull(r.Item("Territor_Code")) Then
				m_Territor_Code = Nothing
			Else
				m_Territor_Code = Convert.ToString(r.Item("Territor_Code"))
			End If
			If IsDBNull(r.Item("Chain_Name")) Then
				m_Chain_Name = Nothing
			Else
				m_Chain_Name = Convert.ToString(r.Item("Chain_Name"))
			End If
			If IsDBNull(r.Item("Budget_Amount")) Then
				m_Budget_Amount = Nothing
			Else
				m_Budget_Amount = Convert.ToDouble(r.Item("Budget_Amount"))
			End If
			If IsDBNull(r.Item("Credit_Limit_LCY")) Then
				m_Credit_Limit_LCY = Nothing
			Else
				m_Credit_Limit_LCY = Convert.ToDouble(r.Item("Credit_Limit_LCY"))
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
			If IsDBNull(r.Item("Language_Code")) Then
				m_Language_Code = Nothing
			Else
				m_Language_Code = Convert.ToString(r.Item("Language_Code"))
			End If
			If IsDBNull(r.Item("Payment_Terms_Code")) Then
				m_Payment_Terms_Code = Nothing
			Else
				m_Payment_Terms_Code = Convert.ToString(r.Item("Payment_Terms_Code"))
			End If
			If IsDBNull(r.Item("Salesperson_Code")) Then
				m_Salesperson_Code = Nothing
			Else
				m_Salesperson_Code = Convert.ToString(r.Item("Salesperson_Code"))
			End If
			If IsDBNull(r.Item("Shipment_Method_Code")) Then
				m_Shipment_Method_Code = Nothing
			Else
				m_Shipment_Method_Code = Convert.ToString(r.Item("Shipment_Method_Code"))
			End If
			If IsDBNull(r.Item("Shipping_Agent_Code")) Then
				m_Shipping_Agent_Code = Nothing
			Else
				m_Shipping_Agent_Code = Convert.ToString(r.Item("Shipping_Agent_Code"))
			End If
			If IsDBNull(r.Item("Customer_Disc_Group")) Then
				m_Customer_Disc_Group = Nothing
			Else
				m_Customer_Disc_Group = Convert.ToString(r.Item("Customer_Disc_Group"))
			End If
			If IsDBNull(r.Item("Country_Code")) Then
				m_Country_Code = Nothing
			Else
				m_Country_Code = Convert.ToString(r.Item("Country_Code"))
			End If
			If IsDBNull(r.Item("Blocked")) Then
				m_Blocked = Nothing
			Else
				m_Blocked = Convert.ToString(r.Item("Blocked"))
			End If
			If IsDBNull(r.Item("Invoice_Copies")) Then
				m_Invoice_Copies = Nothing
			Else
				m_Invoice_Copies = Convert.ToInt32(r.Item("Invoice_Copies"))
			End If
			If IsDBNull(r.Item("Last_Statement_No")) Then
				m_Last_Statement_No = Nothing
			Else
				m_Last_Statement_No = Convert.ToInt32(r.Item("Last_Statement_No"))
			End If
			If IsDBNull(r.Item("Print_Statements")) Then
				m_Print_Statements = Nothing
			Else
				m_Print_Statements = Convert.ToString(r.Item("Print_Statements"))
			End If
			If IsDBNull(r.Item("Bill_to_Customer_No")) Then
				m_Bill_to_Customer_No = Nothing
			Else
				m_Bill_to_Customer_No = Convert.ToString(r.Item("Bill_to_Customer_No"))
			End If
			If IsDBNull(r.Item("Priority")) Then
				m_Priority = Nothing
			Else
				m_Priority = Convert.ToInt32(r.Item("Priority"))
			End If
			If IsDBNull(r.Item("Payment_Method_Code")) Then
				m_Payment_Method_Code = Nothing
			Else
				m_Payment_Method_Code = Convert.ToString(r.Item("Payment_Method_Code"))
			End If
			If IsDBNull(r.Item("Last_Date_Modified")) Then
				m_Last_Date_Modified = Nothing
			Else
				If IsDate(r.Item("Last_Date_Modified")) Then
					m_Last_Date_Modified = Convert.ToDateTime(r.Item("Last_Date_Modified"))
				End If
			End If
			If IsDBNull(r.Item("Balance_LCY")) Then
				m_Balance_LCY = Nothing
			Else
				m_Balance_LCY = Convert.ToDouble(r.Item("Balance_LCY"))
			End If
			If IsDBNull(r.Item("Sales_LCY")) Then
				m_Sales_LCY = Nothing
			Else
				m_Sales_LCY = Convert.ToDouble(r.Item("Sales_LCY"))
			End If
			If IsDBNull(r.Item("Balance_Due_LCY")) Then
				m_Balance_Due_LCY = Nothing
			Else
				m_Balance_Due_LCY = Convert.ToDouble(r.Item("Balance_Due_LCY"))
			End If
			If IsDBNull(r.Item("Shipped_Not_Invoiced")) Then
				m_Shipped_Not_Invoiced = Nothing
			Else
				m_Shipped_Not_Invoiced = Convert.ToDouble(r.Item("Shipped_Not_Invoiced"))
			End If
			If IsDBNull(r.Item("Application_Method")) Then
				m_Application_Method = Nothing
			Else
				m_Application_Method = Convert.ToString(r.Item("Application_Method"))
			End If
			If IsDBNull(r.Item("Location_Code")) Then
				m_Location_Code = Nothing
			Else
				m_Location_Code = Convert.ToString(r.Item("Location_Code"))
			End If
			If IsDBNull(r.Item("Fax_No")) Then
				m_Fax_No = Nothing
			Else
				m_Fax_No = Convert.ToString(r.Item("Fax_No"))
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
			If IsDBNull(r.Item("Post_Code")) Then
				m_Post_Code = Nothing
			Else
				m_Post_Code = Convert.ToString(r.Item("Post_Code"))
			End If
			If IsDBNull(r.Item("County_Text")) Then
				m_County_Text = Nothing
			Else
				m_County_Text = Convert.ToString(r.Item("County_Text"))
			End If
			If IsDBNull(r.Item("Email_Text")) Then
				m_Email_Text = Nothing
			Else
				m_Email_Text = Convert.ToString(r.Item("Email_Text"))
			End If
			If IsDBNull(r.Item("Home_Page_Text")) Then
				m_Home_Page_Text = Nothing
			Else
				m_Home_Page_Text = Convert.ToString(r.Item("Home_Page_Text"))
			End If
			If IsDBNull(r.Item("Outstanding_Orders_LCY")) Then
				m_Outstanding_Orders_LCY = Nothing
			Else
				m_Outstanding_Orders_LCY = Convert.ToDouble(r.Item("Outstanding_Orders_LCY"))
			End If
			If IsDBNull(r.Item("Shipped_Not_Invoiced_LCY")) Then
				m_Shipped_Not_Invoiced_LCY = Nothing
			Else
				m_Shipped_Not_Invoiced_LCY = Convert.ToDouble(r.Item("Shipped_Not_Invoiced_LCY"))
			End If
			If IsDBNull(r.Item("Primary_Contact_No")) Then
				m_Primary_Contact_No = Nothing
			Else
				m_Primary_Contact_No = Convert.ToString(r.Item("Primary_Contact_No"))
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
			If IsDBNull(r.Item("Shipping_Time")) Then
				m_Shipping_Time = Nothing
			Else
				m_Shipping_Time = Convert.ToString(r.Item("Shipping_Time"))
			End If
			If IsDBNull(r.Item("Shipping_Agent")) Then
				m_Shipping_Agent = Nothing
			Else
				m_Shipping_Agent = Convert.ToString(r.Item("Shipping_Agent"))
			End If
			If IsDBNull(r.Item("WebReferenceNo")) OrElse IsNumeric(r.Item("WebReferenceNo")) Then
				m_WebReferenceNo = Nothing
			Else
				m_WebReferenceNo = Convert.ToInt32(r.Item("WebReferenceNo"))
			End If
			m_Customer_Posting_Group = Convert.ToString(r.Item("Customer_Posting_Group"))
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionCustomerCollection
		Inherits GenericCollection(Of NavisionCustomerRow)
	End Class

End Namespace


