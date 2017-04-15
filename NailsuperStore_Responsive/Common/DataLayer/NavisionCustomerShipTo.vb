Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionCustomerShipToRow
		Inherits NavisionCustomerShipToRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionCustomerShipToCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionCustomerShipToRow
                Dim c As New NavisionCustomerShipToCollection
                Dim SQL As String = "select * from _NAVISION_CUSTOMER_SHIPTO"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionCustomerShipToRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionCustomerShipToCollection
        End Function

	End Class

	Public MustInherit Class NavisionCustomerShipToRowBase
		Private m_DB As Database
		Private m_Cust_No As String = Nothing
		Private m_Code As String = Nothing
		Private m_Cust_Name As String = Nothing
		Private m_Cust_Name_2 As String = Nothing
		Private m_Address As String = Nothing
		Private m_Address_2 As String = Nothing
		Private m_City As String = Nothing
		Private m_Contact As String = Nothing
		Private m_Phone As String = Nothing
		Private m_Shipment_Method_Code As String = Nothing
		Private m_Shipping_Agent_Code As String = Nothing
		Private m_Country_Code As String = Nothing
		Private m_Last_Date_Modified As DateTime = Nothing
		Private m_Location_Code As String = Nothing
		Private m_Fax_No As String = Nothing
		Private m_Post_Code As String = Nothing
		Private m_County As String = Nothing
		Private m_Email As String = Nothing
		Private m_Home_Page As String = Nothing
		Private m_Shipping_Agent_Service_Code As String = Nothing
		Private m_ID As Integer = Nothing

		Public Property Cust_No() As String
			Get
				Return Trim(m_Cust_No)
			End Get
			Set(ByVal Value As String)
				m_Cust_No = Trim(Value)
			End Set
		End Property

		Public Property Code() As String
			Get
				Return Trim(m_Code)
			End Get
			Set(ByVal Value As String)
				m_Code = Trim(Value)
			End Set
		End Property

		Public Property Cust_Name() As String
			Get
				Return Trim(m_Cust_Name)
			End Get
			Set(ByVal Value As String)
				m_Cust_Name = Trim(Value)
			End Set
		End Property

		Public Property Cust_Name_2() As String
			Get
				Return Trim(m_Cust_Name_2)
			End Get
			Set(ByVal Value As String)
				m_Cust_Name_2 = Trim(Value)
			End Set
		End Property

		Public Property Address() As String
			Get
				Return Trim(m_Address)
			End Get
			Set(ByVal Value As String)
				m_Address = Trim(Value)
			End Set
		End Property

		Public Property Address_2() As String
			Get
				Return Trim(m_Address_2)
			End Get
			Set(ByVal Value As String)
				m_Address_2 = Trim(Value)
			End Set
		End Property

		Public Property City() As String
			Get
				Return Trim(m_City)
			End Get
			Set(ByVal Value As String)
				m_City = Trim(Value)
			End Set
		End Property

		Public Property Contact() As String
			Get
				Return Trim(m_Contact)
			End Get
			Set(ByVal Value As String)
				m_Contact = Trim(Value)
			End Set
		End Property

		Public Property Phone() As String
			Get
				Return Trim(m_Phone)
			End Get
			Set(ByVal Value As String)
				m_Phone = Trim(Value)
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

		Public Property Shipping_Agent_Code() As String
			Get
				Return Trim(m_Shipping_Agent_Code)
			End Get
			Set(ByVal Value As String)
				m_Shipping_Agent_Code = Trim(Value)
			End Set
		End Property

		Public Property Country_Code() As String
			Get
				Return Trim(m_Country_Code)
			End Get
			Set(ByVal Value As String)
				m_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Last_Date_Modified() As DateTime
			Get
				Return Trim(m_Last_Date_Modified)
			End Get
			Set(ByVal Value As DateTime)
				m_Last_Date_Modified = Trim(Value)
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

		Public Property Fax_No() As String
			Get
				Return Trim(m_Fax_No)
			End Get
			Set(ByVal Value As String)
				m_Fax_No = Trim(Value)
			End Set
		End Property

		Public Property Post_Code() As String
			Get
				Return Trim(m_Post_Code)
			End Get
			Set(ByVal Value As String)
				m_Post_Code = Trim(Value)
			End Set
		End Property

		Public Property County() As String
			Get
				Return Trim(m_County)
			End Get
			Set(ByVal Value As String)
				m_County = Trim(Value)
			End Set
		End Property

		Public Property Email() As String
			Get
				Return Trim(m_Email)
			End Get
			Set(ByVal Value As String)
				m_Email = Trim(Value)
			End Set
		End Property

		Public Property Home_Page() As String
			Get
				Return Trim(m_Home_Page)
			End Get
			Set(ByVal Value As String)
				m_Home_Page = Trim(Value)
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
			If IsDBNull(r.Item("Cust_No")) Then
				m_Cust_No = Nothing
			Else
				m_Cust_No = Convert.ToString(r.Item("Cust_No"))
			End If
			If IsDBNull(r.Item("Code")) Then
				m_Code = Nothing
			Else
				m_Code = Convert.ToString(r.Item("Code"))
			End If
			If IsDBNull(r.Item("Cust_Name")) Then
				m_Cust_Name = Nothing
			Else
				m_Cust_Name = Convert.ToString(r.Item("Cust_Name"))
			End If
			If IsDBNull(r.Item("Cust_Name_2")) Then
				m_Cust_Name_2 = Nothing
			Else
				m_Cust_Name_2 = Convert.ToString(r.Item("Cust_Name_2"))
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
			If IsDBNull(r.Item("Phone")) Then
				m_Phone = Nothing
			Else
				m_Phone = Convert.ToString(r.Item("Phone"))
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
			If IsDBNull(r.Item("Country_Code")) Then
				m_Country_Code = Nothing
			Else
				m_Country_Code = Convert.ToString(r.Item("Country_Code"))
			End If
			If IsDBNull(r.Item("Last_Date_Modified")) Then
				m_Last_Date_Modified = Nothing
			Else
				m_Last_Date_Modified = Convert.ToDateTime(r.Item("Last_Date_Modified"))
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
			If IsDBNull(r.Item("Post_Code")) Then
				m_Post_Code = Nothing
			Else
				m_Post_Code = Convert.ToString(r.Item("Post_Code"))
			End If
			If IsDBNull(r.Item("County")) Then
				m_County = Nothing
			Else
				m_County = Convert.ToString(r.Item("County"))
			End If
			If IsDBNull(r.Item("Email")) Then
				m_Email = Nothing
			Else
				m_Email = Convert.ToString(r.Item("Email"))
			End If
			If IsDBNull(r.Item("Home_Page")) Then
				m_Home_Page = Nothing
			Else
				m_Home_Page = Convert.ToString(r.Item("Home_Page"))
			End If
			If IsDBNull(r.Item("Shipping_Agent_Service_Code")) Then
				m_Shipping_Agent_Service_Code = Nothing
			Else
				m_Shipping_Agent_Service_Code = Convert.ToString(r.Item("Shipping_Agent_Service_Code"))
			End If
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionCustomerShipToCollection
		Inherits GenericCollection(Of NavisionCustomerShipToRow)
	End Class

End Namespace


