Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class CustomerShipToRow
		Inherits CustomerShipToRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ShipToId As Integer)
			MyBase.New(DB, ShipToId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal CustomerNo As String)
			MyBase.New(DB, CustomerNo)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ShipToId As Integer) As CustomerShipToRow
			Dim row As CustomerShipToRow

			row = New CustomerShipToRow(DB, ShipToId)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal CustomerNo As String) As CustomerShipToRow
			Dim row As CustomerShipToRow

			row = New CustomerShipToRow(DB, CustomerNo)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ShipToId As Integer)
			Dim row As CustomerShipToRow

			row = New CustomerShipToRow(DB, ShipToId)
			row.Remove()
		End Sub

		'Custom Methods
		Public Sub CopyFromNavision(ByVal r As NavisionCustomerShipToRow)
			CustomerId = DB.ExecuteScalar("select top 1 coalesce(CustomerId, 0) from Customer where CustomerNo = " & DB.Quote(CustomerNo))
			If CustomerId = Nothing Then Throw New Exception("Customer not found for CustomerShipTo record." & vbCrLf & "CustomerNo: " & CustomerNo)

			Address = r.Address
			Address2 = r.Address_2
			City = r.City
			Code = r.Code
			Contact = r.Contact
			Country = r.Country_Code
			County = r.County
			Name = r.Cust_Name
			Name2 = r.Cust_Name_2
			CustomerNo = r.Cust_No
			Email = r.Email
			FaxNo = r.Fax_No
			Website = r.Home_Page
			LastDateModified = r.Last_Date_Modified
			LocationCode = r.Location_Code
			Phone = r.Phone
			Zipcode = r.Post_Code
			ShipmentMethodCode = r.Shipment_Method_Code
			ShippingAgentCode = r.Shipping_Agent_Code
			ShippingAgentServiceCode = r.Shipping_Agent_Service_Code

			If ShipToId = Nothing Then
				Insert()
			Else
				Update()
			End If
		End Sub

	End Class

	Public MustInherit Class CustomerShipToRowBase
		Private m_DB As Database
		Private m_ShipToId As Integer = Nothing
		Private m_CustomerId As Integer = Nothing
		Private m_CustomerNo As String = Nothing
		Private m_Code As String = Nothing
		Private m_Name As String = Nothing
		Private m_Name2 As String = Nothing
		Private m_Address As String = Nothing
		Private m_Address2 As String = Nothing
		Private m_City As String = Nothing
		Private m_Contact As String = Nothing
		Private m_Phone As String = Nothing
		Private m_ShipmentMethodCode As String = Nothing
		Private m_ShippingAgentCode As String = Nothing
		Private m_Country As String = Nothing
		Private m_LastDateModified As DateTime = Nothing
		Private m_LocationCode As String = Nothing
		Private m_FaxNo As String = Nothing
		Private m_Zipcode As String = Nothing
		Private m_County As String = Nothing
		Private m_Email As String = Nothing
		Private m_Website As String = Nothing
		Private m_ShippingAgentServiceCode As String = Nothing

		Public Property ShipToId() As Integer
			Get
				Return m_ShipToId
			End Get
			Set(ByVal Value As Integer)
				m_ShipToId = Value
			End Set
		End Property

		Public Property CustomerId() As Integer
			Get
				Return m_CustomerId
			End Get
			Set(ByVal Value As Integer)
				m_CustomerId = value
			End Set
		End Property

		Public Property CustomerNo() As String
			Get
				Return m_CustomerNo
			End Get
			Set(ByVal Value As String)
				m_CustomerNo = value
			End Set
		End Property

		Public Property Code() As String
			Get
				Return m_Code
			End Get
			Set(ByVal Value As String)
				m_Code = value
			End Set
		End Property

		Public Property Name() As String
			Get
				Return m_Name
			End Get
			Set(ByVal Value As String)
				m_Name = value
			End Set
		End Property

		Public Property Name2() As String
			Get
				Return m_Name2
			End Get
			Set(ByVal Value As String)
				m_Name2 = value
			End Set
		End Property

		Public Property Address() As String
			Get
				Return m_Address
			End Get
			Set(ByVal Value As String)
				m_Address = value
			End Set
		End Property

		Public Property Address2() As String
			Get
				Return m_Address2
			End Get
			Set(ByVal Value As String)
				m_Address2 = value
			End Set
		End Property

		Public Property City() As String
			Get
				Return m_City
			End Get
			Set(ByVal Value As String)
				m_City = value
			End Set
		End Property

		Public Property Contact() As String
			Get
				Return m_Contact
			End Get
			Set(ByVal Value As String)
				m_Contact = value
			End Set
		End Property

		Public Property Phone() As String
			Get
				Return m_Phone
			End Get
			Set(ByVal Value As String)
				m_Phone = value
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

		Public Property ShippingAgentCode() As String
			Get
				Return m_ShippingAgentCode
			End Get
			Set(ByVal Value As String)
				m_ShippingAgentCode = value
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

		Public Property LastDateModified() As DateTime
			Get
				Return m_LastDateModified
			End Get
			Set(ByVal Value As DateTime)
				m_LastDateModified = value
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

		Public Property FaxNo() As String
			Get
				Return m_FaxNo
			End Get
			Set(ByVal Value As String)
				m_FaxNo = value
			End Set
		End Property

		Public Property Zipcode() As String
			Get
				Return m_Zipcode
			End Get
			Set(ByVal Value As String)
				m_Zipcode = value
			End Set
		End Property

		Public Property County() As String
			Get
				Return m_County
			End Get
			Set(ByVal Value As String)
				m_County = value
			End Set
		End Property

		Public Property Email() As String
			Get
				Return m_Email
			End Get
			Set(ByVal Value As String)
				m_Email = value
			End Set
		End Property

		Public Property Website() As String
			Get
				Return m_Website
			End Get
			Set(ByVal Value As String)
				m_Website = value
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

		Public Sub New(ByVal DB As Database, ByVal ShipToId As Integer)
			m_DB = DB
			m_ShipToId = ShipToId
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal CustomerNo As String)
			m_DB = DB
			m_CustomerNo = CustomerNo
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM CustomerShipTo WHERE " & IIf(ShipToId <> Nothing, "ShipToId = " & DB.Number(ShipToId), "CustomerNo = " & DB.Quote(CustomerNo))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
		End Sub


		Protected Overridable Sub Load(ByVal r As sqlDataReader)
			m_ShipToId = Convert.ToInt32(r.Item("ShipToId"))
			m_CustomerId = Convert.ToInt32(r.Item("CustomerId"))
			m_CustomerNo = Convert.ToString(r.Item("CustomerNo"))
			If IsDBNull(r.Item("Code")) Then
				m_Code = Nothing
			Else
				m_Code = Convert.ToString(r.Item("Code"))
			End If
			If IsDBNull(r.Item("Name")) Then
				m_Name = Nothing
			Else
				m_Name = Convert.ToString(r.Item("Name"))
			End If
			If IsDBNull(r.Item("Name2")) Then
				m_Name2 = Nothing
			Else
				m_Name2 = Convert.ToString(r.Item("Name2"))
			End If
			If IsDBNull(r.Item("Address")) Then
				m_Address = Nothing
			Else
				m_Address = Convert.ToString(r.Item("Address"))
			End If
			If IsDBNull(r.Item("Address2")) Then
				m_Address2 = Nothing
			Else
				m_Address2 = Convert.ToString(r.Item("Address2"))
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
			If IsDBNull(r.Item("ShipmentMethodCode")) Then
				m_ShipmentMethodCode = Nothing
			Else
				m_ShipmentMethodCode = Convert.ToString(r.Item("ShipmentMethodCode"))
			End If
			If IsDBNull(r.Item("ShippingAgentCode")) Then
				m_ShippingAgentCode = Nothing
			Else
				m_ShippingAgentCode = Convert.ToString(r.Item("ShippingAgentCode"))
			End If
			If IsDBNull(r.Item("Country")) Then
				m_Country = Nothing
			Else
				m_Country = Convert.ToString(r.Item("Country"))
			End If
			If IsDBNull(r.Item("LastDateModified")) Then
				m_LastDateModified = Nothing
			Else
				m_LastDateModified = Convert.ToDateTime(r.Item("LastDateModified"))
			End If
			If IsDBNull(r.Item("LocationCode")) Then
				m_LocationCode = Nothing
			Else
				m_LocationCode = Convert.ToString(r.Item("LocationCode"))
			End If
			If IsDBNull(r.Item("FaxNo")) Then
				m_FaxNo = Nothing
			Else
				m_FaxNo = Convert.ToString(r.Item("FaxNo"))
			End If
			If IsDBNull(r.Item("Zipcode")) Then
				m_Zipcode = Nothing
			Else
				m_Zipcode = Convert.ToString(r.Item("Zipcode"))
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
			If IsDBNull(r.Item("Website")) Then
				m_Website = Nothing
			Else
				m_Website = Convert.ToString(r.Item("Website"))
			End If
			If IsDBNull(r.Item("ShippingAgentServiceCode")) Then
				m_ShippingAgentServiceCode = Nothing
			Else
				m_ShippingAgentServiceCode = Convert.ToString(r.Item("ShippingAgentServiceCode"))
			End If
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO CustomerShipTo (" _
			 & " CustomerId" _
			 & ",CustomerNo" _
			 & ",Code" _
			 & ",[Name]" _
			 & ",Name2" _
			 & ",Address" _
			 & ",Address2" _
			 & ",City" _
			 & ",Contact" _
			 & ",Phone" _
			 & ",ShipmentMethodCode" _
			 & ",ShippingAgentCode" _
			 & ",Country" _
			 & ",LastDateModified" _
			 & ",LocationCode" _
			 & ",FaxNo" _
			 & ",Zipcode" _
			 & ",County" _
			 & ",Email" _
			 & ",Website" _
			 & ",ShippingAgentServiceCode" _
			 & ") VALUES (" _
			 & m_DB.Number(CustomerId) _
			 & "," & m_DB.Quote(CustomerNo) _
			 & "," & m_DB.Quote(Code) _
			 & "," & m_DB.Quote(Name) _
			 & "," & m_DB.Quote(Name2) _
			 & "," & m_DB.Quote(Address) _
			 & "," & m_DB.Quote(Address2) _
			 & "," & m_DB.Quote(City) _
			 & "," & m_DB.Quote(Contact) _
			 & "," & m_DB.Quote(Phone) _
			 & "," & m_DB.Quote(ShipmentMethodCode) _
			 & "," & m_DB.Quote(ShippingAgentCode) _
			 & "," & m_DB.Quote(Country) _
			 & "," & m_DB.NullQuote(LastDateModified) _
			 & "," & m_DB.Quote(LocationCode) _
			 & "," & m_DB.Quote(FaxNo) _
			 & "," & m_DB.Quote(Zipcode) _
			 & "," & m_DB.Quote(County) _
			 & "," & m_DB.Quote(Email) _
			 & "," & m_DB.Quote(Website) _
			 & "," & m_DB.Quote(ShippingAgentServiceCode) _
			 & ")"

			ShipToId = m_DB.InsertSQL(SQL)

			Return ShipToId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE CustomerShipTo SET " _
			 & " CustomerId = " & m_DB.Number(CustomerId) _
			 & ",CustomerNo = " & m_DB.Quote(CustomerNo) _
			 & ",Code = " & m_DB.Quote(Code) _
			 & ",[Name] = " & m_DB.Quote(Name) _
			 & ",Name2 = " & m_DB.Quote(Name2) _
			 & ",Address = " & m_DB.Quote(Address) _
			 & ",Address2 = " & m_DB.Quote(Address2) _
			 & ",City = " & m_DB.Quote(City) _
			 & ",Contact = " & m_DB.Quote(Contact) _
			 & ",Phone = " & m_DB.Quote(Phone) _
			 & ",ShipmentMethodCode = " & m_DB.Quote(ShipmentMethodCode) _
			 & ",ShippingAgentCode = " & m_DB.Quote(ShippingAgentCode) _
			 & ",Country = " & m_DB.Quote(Country) _
			 & ",LastDateModified = " & m_DB.NullQuote(LastDateModified) _
			 & ",LocationCode = " & m_DB.Quote(LocationCode) _
			 & ",FaxNo = " & m_DB.Quote(FaxNo) _
			 & ",Zipcode = " & m_DB.Quote(Zipcode) _
			 & ",County = " & m_DB.Quote(County) _
			 & ",Email = " & m_DB.Quote(Email) _
			 & ",Website = " & m_DB.Quote(Website) _
			 & ",ShippingAgentServiceCode = " & m_DB.Quote(ShippingAgentServiceCode) _
			 & " WHERE " & IIf(ShipToId <> Nothing, "ShipToId = " & DB.Number(ShipToId), "CustomerNo = " & DB.Quote(CustomerNo))

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM CustomerShipTo WHERE " & IIf(ShipToId <> Nothing, "ShipToId = " & DB.Number(ShipToId), "CustomerNo = " & DB.Quote(CustomerNo))
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class CustomerShipToCollection
		Inherits GenericCollection(Of CustomerShipToRow)
	End Class

End Namespace


