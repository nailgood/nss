Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components

Namespace DataLayer

	Public Class JobRow
		Inherits JobRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal JobId As Integer)
			MyBase.New(DB, JobId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal JobId As Integer) As JobRow
			Dim row As JobRow

			row = New JobRow(DB, JobId)
			row.Load()

			Return row
		End Function
       
		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal JobId As Integer)
			Dim row As JobRow

			row = New JobRow(DB, JobId)
			row.Remove()
		End Sub

		'Custom Methods

	End Class

	Public MustInherit Class JobRowBase
		Private m_DB As Database
		Private m_JobId As Integer = Nothing
		Private m_City As String = Nothing
		Private m_State As Integer = Nothing
		Private m_Zip As String = Nothing
		Private m_Company As String = Nothing
		Private m_Email As String = Nothing
		Private m_Phone As String = Nothing
		Private m_ShortDescription As String = Nothing
		Private m_Description As String = Nothing
		Private m_Title As String = Nothing
		Private m_IsActive As Boolean = Nothing
		Private m_CreateDate As DateTime = Nothing
		Private m_CategoryId As Integer = Nothing
		Private m_ExpirationDate As DateTime = Nothing
		Private m_FullPartTime As String = Nothing
		Private m_Requirements As String = Nothing
		Private m_Comments As String = Nothing
		Private m_Compensation As String = Nothing
		Private m_Benefits As String = Nothing

		Public Property FullPartTime() As String
			Get
				Return m_FullPartTime
			End Get
			Set(ByVal Value As String)
				m_FullPartTime = value
			End Set
		End Property

		Public Property Requirements() As String
			Get
				Return m_Requirements
			End Get
			Set(ByVal Value As String)
				m_Requirements = value
			End Set
		End Property

		Public Property Comments() As String
			Get
				Return m_Comments
			End Get
			Set(ByVal Value As String)
				m_Comments = value
			End Set
		End Property

		Public Property Compensation() As String
			Get
				Return m_Compensation
			End Get
			Set(ByVal Value As String)
				m_Compensation = value
			End Set
		End Property

		Public Property Benefits() As String
			Get
				Return m_Benefits
			End Get
			Set(ByVal Value As String)
				m_Benefits = value
			End Set
		End Property

		Public Property Title() As String
			Get
				Return m_Title
			End Get
			Set(ByVal Value As String)
				m_Title = Value
			End Set
		End Property

		Public Property CategoryId() As Integer
			Get
				Return m_CategoryId
			End Get
			Set(ByVal Value As Integer)
				m_CategoryId = Value
			End Set
		End Property

		Public Property JobId() As Integer
			Get
				Return m_JobId
			End Get
			Set(ByVal Value As Integer)
				m_JobId = value
			End Set
		End Property

		Public Property City() As String
			Get
				Return m_City
			End Get
			Set(ByVal Value As String)
				m_City = Value
			End Set
		End Property

		Public Property State() As Integer
			Get
				Return m_State
			End Get
			Set(ByVal Value As Integer)
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

		Public Property Company() As String
			Get
				Return m_Company
			End Get
			Set(ByVal Value As String)
				m_Company = value
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

		Public Property Phone() As String
			Get
				Return m_Phone
			End Get
			Set(ByVal Value As String)
				m_Phone = value
			End Set
		End Property

		Public Property ShortDescription() As String
			Get
				Return m_ShortDescription
			End Get
			Set(ByVal Value As String)
				m_ShortDescription = value
			End Set
		End Property

		Public Property Description() As String
			Get
				Return m_Description
			End Get
			Set(ByVal Value As String)
				m_Description = value
			End Set
		End Property

		Public Property IsActive() As Boolean
			Get
				Return m_IsActive
			End Get
			Set(ByVal Value As Boolean)
				m_IsActive = value
			End Set
		End Property

		Public ReadOnly Property CreateDate() As DateTime
			Get
				Return m_CreateDate
			End Get
		End Property

		Public Property ExpirationDate() As DateTime
			Get
				Return m_ExpirationDate
			End Get
			Set(ByVal Value As DateTime)
				m_ExpirationDate = value
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

		Public Sub New(ByVal DB As Database, ByVal JobId As Integer)
			m_DB = DB
			m_JobId = JobId
		End Sub	'New
       
        'end 23/10/2009
		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Job WHERE JobId = " & DB.Number(JobId)
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
			m_JobId = Convert.ToInt32(r.Item("JobId"))
			If IsDBNull(r.Item("FullPartTime")) Then
				m_FullPartTime = Nothing
			Else
				m_FullPartTime = Convert.ToString(r.Item("FullPartTime"))
			End If
			If IsDBNull(r.Item("Requirements")) Then
				m_Requirements = Nothing
			Else
				m_Requirements = Convert.ToString(r.Item("Requirements"))
			End If
			If IsDBNull(r.Item("Comments")) Then
				m_Comments = Nothing
			Else
				m_Comments = Convert.ToString(r.Item("Comments"))
			End If
			If IsDBNull(r.Item("Compensation")) Then
				m_Compensation = Nothing
			Else
				m_Compensation = Convert.ToString(r.Item("Compensation"))
			End If
			If IsDBNull(r.Item("Benefits")) Then
				m_Benefits = Nothing
			Else
				m_Benefits = Convert.ToString(r.Item("Benefits"))
			End If
			If IsDBNull(r.Item("ExpirationDate")) Then
				m_ExpirationDate = Nothing
			Else
				m_ExpirationDate = Convert.ToDateTime(r.Item("ExpirationDate"))
			End If
			If IsDBNull(r.Item("CategoryId")) Then
				m_CategoryId = Nothing
			Else
				m_CategoryId = Convert.ToInt32(r.Item("CategoryId"))
			End If
			If IsDBNull(r.Item("City")) Then
				m_City = Nothing
			Else
				m_City = Convert.ToString(r.Item("City"))
			End If
			If IsDBNull(r.Item("State")) Then
				m_State = Nothing
			Else
				m_State = Convert.ToInt32(r.Item("State"))
			End If
			If IsDBNull(r.Item("Title")) Then
				m_Title = Nothing
			Else
				m_Title = Convert.ToString(r.Item("Title"))
			End If
			If IsDBNull(r.Item("Zip")) Then
				m_Zip = Nothing
			Else
				m_Zip = Convert.ToString(r.Item("Zip"))
			End If
			If IsDBNull(r.Item("Company")) Then
				m_Company = Nothing
			Else
				m_Company = Convert.ToString(r.Item("Company"))
			End If
			If IsDBNull(r.Item("Email")) Then
				m_Email = Nothing
			Else
				m_Email = Convert.ToString(r.Item("Email"))
			End If
			If IsDBNull(r.Item("Phone")) Then
				m_Phone = Nothing
			Else
				m_Phone = Convert.ToString(r.Item("Phone"))
			End If
			If IsDBNull(r.Item("ShortDescription")) Then
				m_ShortDescription = Nothing
			Else
				m_ShortDescription = Convert.ToString(r.Item("ShortDescription"))
			End If
			If IsDBNull(r.Item("Description")) Then
				m_Description = Nothing
			Else
				m_Description = Convert.ToString(r.Item("Description"))
			End If
			m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
			m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


            SQL = " INSERT INTO Job (" _
             & " CategoryId" _
             & ",City" _
             & ",State" _
             & ",Zip" _
             & ",Company" _
             & ",Email" _
             & ",Phone" _
             & ",Title" _
             & ",ShortDescription" _
             & ",Description" _
             & ",IsActive" _
             & ",CreateDate" _
             & ",ExpirationDate" _
             & ",FullPartTime" _
             & ",Requirements" _
             & ",Comments" _
             & ",Compensation" _
             & ",Benefits" _
             & ") VALUES (" _
             & m_DB.NullNumber(CategoryId) _
             & "," & m_DB.NQuote(City) _
             & "," & m_DB.Number(State) _
             & "," & m_DB.Quote(Zip) _
             & "," & m_DB.NQuote(Company) _
             & "," & m_DB.Quote(Email) _
             & "," & m_DB.Quote(Phone) _
             & "," & m_DB.NQuote(Title) _
             & "," & m_DB.NQuote(ShortDescription) _
             & "," & m_DB.NQuote(Description) _
             & "," & CInt(IsActive) _
             & "," & m_DB.NullQuote(Now) _
             & "," & m_DB.NullQuote(ExpirationDate) _
             & "," & m_DB.NQuote(FullPartTime) _
             & "," & m_DB.NQuote(Requirements) _
             & "," & m_DB.NQuote(Comments) _
             & "," & m_DB.NQuote(Compensation) _
             & "," & m_DB.NQuote(Benefits) _
             & ")"

			JobId = m_DB.InsertSQL(SQL)

			Return JobId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

            SQL = " UPDATE Job SET " _
             & " CategoryId = " & m_DB.NullNumber(CategoryId) _
             & ",City = " & m_DB.NQuote(City) _
             & ",State = " & m_DB.Number(State) _
             & ",Zip = " & m_DB.Quote(Zip) _
             & ",Company = " & m_DB.NQuote(Company) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",Phone = " & m_DB.Quote(Phone) _
             & ",Title = " & m_DB.NQuote(Title) _
             & ",ShortDescription = " & m_DB.NQuote(ShortDescription) _
             & ",Description = " & m_DB.NQuote(Description) _
             & ",IsActive = " & CInt(IsActive) _
             & ",ExpirationDate = " & m_DB.NullQuote(ExpirationDate) _
             & ",FullPartTime = " & m_DB.NQuote(FullPartTime) _
             & ",Requirements = " & m_DB.NQuote(Requirements) _
             & ",Comments = " & m_DB.NQuote(Comments) _
             & ",Compensation = " & m_DB.NQuote(Compensation) _
             & ",Benefits = " & m_DB.NQuote(Benefits) _
             & " WHERE JobId = " & m_DB.NQuote(JobId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM Job WHERE JobId = " & m_DB.Quote(JobId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class JobCollection
		Inherits GenericCollection(Of JobRow)
	End Class

End Namespace


