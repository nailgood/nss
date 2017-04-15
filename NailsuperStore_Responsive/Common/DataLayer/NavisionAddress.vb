Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionAddressRow
		Inherits NavisionAddressRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

        Public Shared Function GetCollection(ByVal DB As Database) As NavisionAddressCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionAddressRow
                Dim c As New NavisionAddressCollection
                Dim SQL As String = "select * from _NAVISION_ADDRESS"

                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionAddressRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)

                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionAddressCollection
        End Function
	End Class

	Public MustInherit Class NavisionAddressRowBase
		Private m_DB As Database
		Private m_Customer_No As String = Nothing
		Private m_Label As String = Nothing
		Private m_Action As String = Nothing
		Private m_First_Name As String = Nothing
		Private m_Last_Name As String = Nothing
		Private m_Company As String = Nothing
		Private m_Company_2 As String = Nothing
		Private m_Address_1 As String = Nothing
		Private m_Address_2 As String = Nothing
		Private m_City As String = Nothing
		Private m_State As String = Nothing
		Private m_Zip As String = Nothing
		Private m_Region As String = Nothing
		Private m_Country As String = Nothing
		Private m_Phone As String = Nothing
		Private m_AddressId As Integer = Nothing
		Private m_Id As Integer = Nothing


		Public Property Customer_No() As String
			Get
				Return m_Customer_No
			End Get
			Set(ByVal Value As String)
				m_Customer_No = Value
			End Set
		End Property

		Public Property Label() As String
			Get
				Return m_Label
			End Get
			Set(ByVal Value As String)
				m_Label = value
			End Set
		End Property

		Public Property Action() As String
			Get
				Return m_Action
			End Get
			Set(ByVal Value As String)
				m_Action = value
			End Set
		End Property

		Public Property First_Name() As String
			Get
				Return m_First_Name
			End Get
			Set(ByVal Value As String)
				m_First_Name = value
			End Set
		End Property

		Public Property Last_Name() As String
			Get
				Return m_Last_Name
			End Get
			Set(ByVal Value As String)
				m_Last_Name = value
			End Set
		End Property

		Public Property Company() As String
			Get
				Return m_Company
			End Get
			Set(ByVal Value As String)
				m_Company = value
			End Set
		End Property

		Public Property Company_2() As String
			Get
				Return m_Company_2
			End Get
			Set(ByVal Value As String)
				m_Company_2 = value
			End Set
		End Property

		Public Property Address_1() As String
			Get
				Return m_Address_1
			End Get
			Set(ByVal Value As String)
				m_Address_1 = value
			End Set
		End Property

		Public Property Address_2() As String
			Get
				Return m_Address_2
			End Get
			Set(ByVal Value As String)
				m_Address_2 = value
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

		Public Property State() As String
			Get
				Return m_State
			End Get
			Set(ByVal Value As String)
				m_State = value
			End Set
		End Property

		Public Property Zip() As String
			Get
				Return m_Zip
			End Get
			Set(ByVal Value As String)
				m_Zip = value
			End Set
		End Property

		Public Property Region() As String
			Get
				Return m_Region
			End Get
			Set(ByVal Value As String)
				m_Region = value
			End Set
		End Property

		Public Property Country() As String
			Get
				Return m_Country
			End Get
			Set(ByVal Value As String)
				m_Country = value
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

		Public Property AddressId() As Integer
			Get
				Return m_AddressId
			End Get
			Set(ByVal Value As Integer)
				m_AddressId = value
			End Set
		End Property

		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal Value As Integer)
				m_Id = value
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

		Public Sub New(ByVal DB As Database, ByVal Customer_No As Integer)
			m_DB = DB
			m_Customer_No = Customer_No
		End Sub	'New


		Protected Overridable Sub Load(ByVal r As SqlDataReader)
			m_Customer_No = Convert.ToString(r.Item("Customer_No"))
			If IsDBNull(r.Item("Label")) Then
				m_Label = Nothing
			Else
				m_Label = Convert.ToString(r.Item("Label"))
			End If
			If IsDBNull(r.Item("Action")) Then
				m_Action = Nothing
			Else
				m_Action = Convert.ToString(r.Item("Action"))
			End If
			If IsDBNull(r.Item("First_Name")) Then
				m_First_Name = Nothing
			Else
				m_First_Name = Convert.ToString(r.Item("First_Name"))
			End If
			If IsDBNull(r.Item("Last_Name")) Then
				m_Last_Name = Nothing
			Else
				m_Last_Name = Convert.ToString(r.Item("Last_Name"))
			End If
			If IsDBNull(r.Item("Company")) Then
				m_Company = Nothing
			Else
				m_Company = Convert.ToString(r.Item("Company"))
			End If
			If IsDBNull(r.Item("Company_2")) Then
				m_Company_2 = Nothing
			Else
				m_Company_2 = Convert.ToString(r.Item("Company_2"))
			End If
			If IsDBNull(r.Item("Address_1")) Then
				m_Address_1 = Nothing
			Else
				m_Address_1 = Convert.ToString(r.Item("Address_1"))
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
			If IsDBNull(r.Item("State")) Then
				m_State = Nothing
			Else
				m_State = Convert.ToString(r.Item("State"))
			End If
			If IsDBNull(r.Item("Zip")) Then
				m_Zip = Nothing
			Else
				m_Zip = Convert.ToString(r.Item("Zip"))
			End If
			If IsDBNull(r.Item("Region")) Then
				m_Region = Nothing
			Else
				m_Region = Convert.ToString(r.Item("Region"))
			End If
			If IsDBNull(r.Item("Country")) Then
				m_Country = Nothing
			Else
				m_Country = Convert.ToString(r.Item("Country"))
			End If
			If IsDBNull(r.Item("Phone")) Then
				m_Phone = Nothing
			Else
				m_Phone = Convert.ToString(r.Item("Phone"))
			End If
			If IsDBNull(r.Item("AddressId")) OrElse Not IsNumeric(r.Item("AddressId")) Then
				m_AddressId = Nothing
			Else
				m_AddressId = Convert.ToInt32(r.Item("AddressId"))
			End If
		End Sub	'Load
	End Class

	Public Class NavisionAddressCollection
		Inherits GenericCollection(Of NavisionAddressRow)
	End Class

End Namespace


