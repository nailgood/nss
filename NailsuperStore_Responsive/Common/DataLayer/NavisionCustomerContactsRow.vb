Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionCustomerContactsRow
		Inherits NavisionCustomerContactsRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionCustomerContactsCollection
            Dim c As New NavisionCustomerContactsCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionCustomerContactsRow
                Dim SQL As String = "select * from _NAVISION_CUSTOMER_CONTACTS"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionCustomerContactsRow(DB)
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

	Public MustInherit Class NavisionCustomerContactsRowBase
		Private m_DB As Database
		Private m_Contact_No As String = Nothing
		Private m_Cust_Name As String = Nothing
		Private m_Cust_Name_2 As String = Nothing
		Private m_Address As String = Nothing
		Private m_Address_2 As String = Nothing
		Private m_City As String = Nothing
		Private m_Phone As String = Nothing
		Private m_Territory_Code As String = Nothing
		Private m_Currency_Code As String = Nothing
		Private m_Language_Code As String = Nothing
		Private m_Salesperson_Code As String = Nothing
		Private m_Country_Code As String = Nothing
		Private m_Comment As String = Nothing
		Private m_Last_Date_Modified As String = Nothing
		Private m_Fax_No As String = Nothing
		Private m_VAT_Registration_No As String = Nothing
		Private m_Post_Code As String = Nothing
		Private m_County As String = Nothing
		Private m_Email As String = Nothing
		Private m_Home_Page As String = Nothing
		Private m_Type As String = Nothing
		Private m_Company_No As String = Nothing
		Private m_Company_Name As String = Nothing
		Private m_First_Name As String = Nothing
		Private m_Middle_Name As String = Nothing
		Private m_Surname As String = Nothing
		Private m_Job_Title As String = Nothing
		Private m_Initials As String = Nothing
		Private m_Extension_No As String = Nothing
		Private m_Mobile_Phone_No As String = Nothing
		Private m_Pager As String = Nothing
		Private m_Salutation_Code As String = Nothing
		Private m_Last_Time_Modified As String = Nothing
		Private m_WebReferenceNo As Integer = Nothing
		Private m_ID As Integer = Nothing

		Public Property Contact_No() As String
			Get
				Return Trim(m_Contact_No)
			End Get
			Set(ByVal Value As String)
				m_Contact_No = Trim(Value)
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

		Public Property Phone() As String
			Get
				Return Trim(m_Phone)
			End Get
			Set(ByVal Value As String)
				m_Phone = Trim(Value)
			End Set
		End Property

		Public Property Territory_Code() As String
			Get
				Return Trim(m_Territory_Code)
			End Get
			Set(ByVal Value As String)
				m_Territory_Code = Trim(Value)
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

		Public Property Country_Code() As String
			Get
				Return Trim(m_Country_Code)
			End Get
			Set(ByVal Value As String)
				m_Country_Code = Trim(Value)
			End Set
		End Property

		Public Property Comment() As String
			Get
				Return Trim(m_Comment)
			End Get
			Set(ByVal Value As String)
				m_Comment = Trim(Value)
			End Set
		End Property

		Public Property Last_Date_Modified() As String
			Get
				Return Trim(m_Last_Date_Modified)
			End Get
			Set(ByVal Value As String)
				m_Last_Date_Modified = Trim(Value)
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

		Public Property VAT_Registration_No() As String
			Get
				Return Trim(m_VAT_Registration_No)
			End Get
			Set(ByVal Value As String)
				m_VAT_Registration_No = Trim(Value)
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

		Public Property Type() As String
			Get
				Return Trim(m_Type)
			End Get
			Set(ByVal Value As String)
				m_Type = Trim(Value)
			End Set
		End Property

		Public Property Company_No() As String
			Get
				Return Trim(m_Company_No)
			End Get
			Set(ByVal Value As String)
				m_Company_No = Trim(Value)
			End Set
		End Property

		Public Property Company_Name() As String
			Get
				Return Trim(m_Company_Name)
			End Get
			Set(ByVal Value As String)
				m_Company_Name = Trim(Value)
			End Set
		End Property

		Public Property First_Name() As String
			Get
				Return Trim(m_First_Name)
			End Get
			Set(ByVal Value As String)
				m_First_Name = Trim(Value)
			End Set
		End Property

		Public Property Middle_Name() As String
			Get
				Return Trim(m_Middle_Name)
			End Get
			Set(ByVal Value As String)
				m_Middle_Name = Trim(Value)
			End Set
		End Property

		Public Property Surname() As String
			Get
				Return Trim(m_Surname)
			End Get
			Set(ByVal Value As String)
				m_Surname = Trim(Value)
			End Set
		End Property

		Public Property Job_Title() As String
			Get
				Return Trim(m_Job_Title)
			End Get
			Set(ByVal Value As String)
				m_Job_Title = Trim(Value)
			End Set
		End Property

		Public Property Initials() As String
			Get
				Return Trim(m_Initials)
			End Get
			Set(ByVal Value As String)
				m_Initials = Trim(Value)
			End Set
		End Property

		Public Property Extension_No() As String
			Get
				Return Trim(m_Extension_No)
			End Get
			Set(ByVal Value As String)
				m_Extension_No = Trim(Value)
			End Set
		End Property

		Public Property Mobile_Phone_No() As String
			Get
				Return Trim(m_Mobile_Phone_No)
			End Get
			Set(ByVal Value As String)
				m_Mobile_Phone_No = Trim(Value)
			End Set
		End Property

		Public Property Pager() As String
			Get
				Return Trim(m_Pager)
			End Get
			Set(ByVal Value As String)
				m_Pager = Trim(Value)
			End Set
		End Property

		Public Property Salutation_Code() As String
			Get
				Return Trim(m_Salutation_Code)
			End Get
			Set(ByVal Value As String)
				m_Salutation_Code = Trim(Value)
			End Set
		End Property

		Public Property Last_Time_Modified() As String
			Get
				Return Trim(m_Last_Time_Modified)
			End Get
			Set(ByVal Value As String)
				m_Last_Time_Modified = Trim(Value)
			End Set
		End Property

		Public Property WebReferenceNo() As Integer
			Get
				Return m_WebReferenceNo
			End Get
			Set(ByVal Value As Integer)
				m_WebReferenceNo = Value
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
			m_Contact_No = Convert.ToString(r.Item("Contact_No"))
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
			If IsDBNull(r.Item("Phone")) Then
				m_Phone = Nothing
			Else
				m_Phone = Convert.ToString(r.Item("Phone"))
			End If
			If IsDBNull(r.Item("Territory_Code")) Then
				m_Territory_Code = Nothing
			Else
				m_Territory_Code = Convert.ToString(r.Item("Territory_Code"))
			End If
			If IsDBNull(r.Item("Currency_Code")) Then
				m_Currency_Code = Nothing
			Else
				m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
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
			If IsDBNull(r.Item("Country_Code")) Then
				m_Country_Code = Nothing
			Else
				m_Country_Code = Convert.ToString(r.Item("Country_Code"))
			End If
			If IsDBNull(r.Item("Comment")) Then
				m_Comment = Nothing
			Else
				m_Comment = Convert.ToString(r.Item("Comment"))
			End If
			If IsDBNull(r.Item("Last_Date_Modified")) Then
				m_Last_Date_Modified = Nothing
			Else
				m_Last_Date_Modified = Convert.ToString(r.Item("Last_Date_Modified"))
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
			If IsDBNull(r.Item("Type")) Then
				m_Type = Nothing
			Else
				m_Type = Convert.ToString(r.Item("Type"))
			End If
			If IsDBNull(r.Item("Company_No")) Then
				m_Company_No = Nothing
			Else
				m_Company_No = Convert.ToString(r.Item("Company_No"))
			End If
			If IsDBNull(r.Item("Company_Name")) Then
				m_Company_Name = Nothing
			Else
				m_Company_Name = Convert.ToString(r.Item("Company_Name"))
			End If
			If IsDBNull(r.Item("First_Name")) Then
				m_First_Name = Nothing
			Else
				m_First_Name = Convert.ToString(r.Item("First_Name"))
			End If
			If IsDBNull(r.Item("Middle_Name")) Then
				m_Middle_Name = Nothing
			Else
				m_Middle_Name = Convert.ToString(r.Item("Middle_Name"))
			End If
			If IsDBNull(r.Item("Surname")) Then
				m_Surname = Nothing
			Else
				m_Surname = Convert.ToString(r.Item("Surname"))
			End If
			If IsDBNull(r.Item("Job_Title")) Then
				m_Job_Title = Nothing
			Else
				m_Job_Title = Convert.ToString(r.Item("Job_Title"))
			End If
			If IsDBNull(r.Item("Initials")) Then
				m_Initials = Nothing
			Else
				m_Initials = Convert.ToString(r.Item("Initials"))
			End If
			If IsDBNull(r.Item("Extension_No")) Then
				m_Extension_No = Nothing
			Else
				m_Extension_No = Convert.ToString(r.Item("Extension_No"))
			End If
			If IsDBNull(r.Item("Mobile_Phone_No")) Then
				m_Mobile_Phone_No = Nothing
			Else
				m_Mobile_Phone_No = Convert.ToString(r.Item("Mobile_Phone_No"))
			End If
			If IsDBNull(r.Item("Pager")) Then
				m_Pager = Nothing
			Else
				m_Pager = Convert.ToString(r.Item("Pager"))
			End If
			If IsDBNull(r.Item("Salutation_Code")) Then
				m_Salutation_Code = Nothing
			Else
				m_Salutation_Code = Convert.ToString(r.Item("Salutation_Code"))
			End If
			If IsDBNull(r.Item("Last_Time_Modified")) Then
				m_Last_Time_Modified = Nothing
			Else
				m_Last_Time_Modified = Convert.ToString(r.Item("Last_Time_Modified"))
			End If
			If IsDBNull(r.Item("WebReferenceNo")) OrElse Not IsNumeric(r.Item("WebReferenceNo")) Then
				m_WebReferenceNo = Nothing
			Else
				m_WebReferenceNo = Convert.ToInt32(r.Item("WebReferenceNo"))
			End If
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionCustomerContactsCollection
		Inherits GenericCollection(Of NavisionCustomerContactsRow)
	End Class

End Namespace


