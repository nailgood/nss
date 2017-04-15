Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class PostJobApplicationRow
		Inherits PostJobApplicationRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ApplicationId As Integer)
			MyBase.New(DB, ApplicationId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ApplicationId As Integer) As PostJobApplicationRow
			Dim row As PostJobApplicationRow

			row = New PostJobApplicationRow(DB, ApplicationId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ApplicationId As Integer)
			Dim row As PostJobApplicationRow

			row = New PostJobApplicationRow(DB, ApplicationId)
			row.Remove()
		End Sub

		'Custom Methods

	End Class

	Public MustInherit Class PostJobApplicationRowBase
		Private m_DB As Database
		Private m_ApplicationId As Integer = Nothing
		Private m_CompanyName As String = Nothing
		Private m_MemberId As Integer = Nothing
		Private m_Website As String = Nothing
		Private m_Email As String = Nothing
		Private m_IsApproved As Boolean = Nothing
		Private m_CreateDate As DateTime = Nothing
		Private m_ApproveDate As DateTime = Nothing
		Private m_Image As String = Nothing


		Public Property ApplicationId() As Integer
			Get
				Return m_ApplicationId
			End Get
			Set(ByVal Value As Integer)
				m_ApplicationId = value
			End Set
		End Property

		Public Property CompanyName() As String
			Get
				Return m_CompanyName
			End Get
			Set(ByVal Value As String)
				m_CompanyName = value
			End Set
		End Property

		Public Property MemberId() As Integer
			Get
				Return m_MemberId
			End Get
			Set(ByVal Value As Integer)
				m_MemberId = value
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

		Public Property Email() As String
			Get
				Return m_Email
			End Get
			Set(ByVal Value As String)
				m_Email = value
			End Set
		End Property

		Public Property IsApproved() As Boolean
			Get
				Return m_IsApproved
			End Get
			Set(ByVal Value As Boolean)
				m_IsApproved = value
			End Set
		End Property

		Public Property ApproveDate() As DateTime
			Get
				Return m_ApproveDate
			End Get
			Set(ByVal Value As DateTime)
				m_ApproveDate = value
			End Set
		End Property

		Public Property Image() As String
			Get
				Return m_Image
			End Get
			Set(ByVal Value As String)
				m_Image = value
			End Set
		End Property

		Public ReadOnly Property CreateDate() As DateTime
			Get
				Return m_CreateDate
			End Get
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

		Public Sub New(ByVal DB As Database, ByVal ApplicationId As Integer)
			m_DB = DB
			m_ApplicationId = ApplicationId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM PostJobApplication WHERE ApplicationId = " & DB.Number(ApplicationId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                ''mail error here
            End Try
			
		End Sub


		Protected Overridable Sub Load(ByVal r As sqlDataReader)
			m_ApplicationId = Convert.ToInt32(r.Item("ApplicationId"))
			m_CompanyName = Convert.ToString(r.Item("CompanyName"))
			If IsDBNull(r.Item("MemberId")) Then
				m_MemberId = Nothing
			Else
				m_MemberId = Convert.ToInt32(r.Item("MemberId"))
			End If
			If IsDBNull(r.Item("Website")) Then
				m_Website = Nothing
			Else
				m_Website = Convert.ToString(r.Item("Website"))
			End If
			m_Email = Convert.ToString(r.Item("Email"))
			m_IsApproved = Convert.ToBoolean(r.Item("IsApproved"))
			m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
			If IsDBNull(r.Item("ApproveDate")) Then
				m_ApproveDate = Nothing
			Else
				m_ApproveDate = Convert.ToDateTime(r.Item("ApproveDate"))
			End If
			If IsDBNull(r.Item("Image")) Then
				m_Image = Nothing
			Else
				m_Image = Convert.ToString(r.Item("Image"))
			End If
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO PostJobApplication (" _
			 & " CompanyName" _
			 & ",MemberId" _
			 & ",Website" _
			 & ",Email" _
			 & ",IsApproved" _
			 & ",CreateDate" _
			 & ",ApproveDate" _
			 & ",Image" _
			 & ") VALUES (" _
			 & m_DB.Quote(CompanyName) _
			 & "," & m_DB.NullNumber(MemberId) _
			 & "," & m_DB.Quote(Website) _
			 & "," & m_DB.Quote(Email) _
			 & "," & CInt(IsApproved) _
			 & "," & m_DB.NullQuote(Now) _
			 & "," & m_DB.NullQuote(ApproveDate) _
			 & "," & m_DB.Quote(Image) _
			 & ")"

			ApplicationId = m_DB.InsertSQL(SQL)

			Return ApplicationId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE PostJobApplication SET " _
			 & " CompanyName = " & m_DB.Quote(CompanyName) _
			 & ",MemberId = " & m_DB.NullNumber(MemberId) _
			 & ",Website = " & m_DB.Quote(Website) _
			 & ",Email = " & m_DB.Quote(Email) _
			 & ",IsApproved = " & CInt(IsApproved) _
			 & ",ApproveDate = " & m_DB.NullQuote(ApproveDate) _
			 & ",Image = " & m_DB.Quote(Image) _
			 & " WHERE ApplicationId = " & m_DB.quote(ApplicationId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM PostJobApplication WHERE ApplicationId = " & m_DB.Quote(ApplicationId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class PostJobApplicationCollection
		Inherits GenericCollection(Of PostJobApplicationRow)
	End Class

End Namespace


