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

	Public Class EmployerRow
		Inherits EmployerRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal EmployerId As Integer)
			MyBase.New(DB, EmployerId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal EmployerId As Integer) As EmployerRow
			Dim row As EmployerRow

			row = New EmployerRow(DB, EmployerId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal EmployerId As Integer)
			Dim row As EmployerRow

			row = New EmployerRow(DB, EmployerId)
			row.Remove()
		End Sub
       
        'Custom Methods
		Public Shared Function GetFeaturedEmployers(ByVal DB As Database) As DataSet
			Return DB.GetDataSet("select top 4 * from employer where isfeatured = 1 order by newid()")
		End Function
	End Class

	Public MustInherit Class EmployerRowBase
		Private m_DB As Database
		Private m_EmployerId As Integer = Nothing
		Private m_EmployerName As String = Nothing
		Private m_Image As String = Nothing
		Private m_IsFeatured As Boolean = Nothing


		Public Property EmployerId() As Integer
			Get
				Return m_EmployerId
			End Get
			Set(ByVal Value As Integer)
				m_EmployerId = value
			End Set
		End Property

		Public Property EmployerName() As String
			Get
				Return m_EmployerName
			End Get
			Set(ByVal Value As String)
				m_EmployerName = value
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

		Public Property IsFeatured() As Boolean
			Get
				Return m_IsFeatured
			End Get
			Set(ByVal Value As Boolean)
				m_IsFeatured = value
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

		Public Sub New(ByVal DB As Database, ByVal EmployerId As Integer)
			m_DB = DB
			m_EmployerId = EmployerId
		End Sub	'New
       
        'end 23/10/2009
		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Employer WHERE EmployerId = " & DB.Number(EmployerId)
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
			m_EmployerId = Convert.ToInt32(r.Item("EmployerId"))
			m_EmployerName = Convert.ToString(r.Item("EmployerName"))
			If IsDBNull(r.Item("Image")) Then
				m_Image = Nothing
			Else
				m_Image = Convert.ToString(r.Item("Image"))
			End If
			m_IsFeatured = Convert.ToBoolean(r.Item("IsFeatured"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO Employer (" _
			 & " EmployerName" _
			 & ",Image" _
			 & ",IsFeatured" _
			 & ") VALUES (" _
			 & m_DB.Quote(EmployerName) _
			 & "," & m_DB.Quote(Image) _
			 & "," & CInt(IsFeatured) _
			 & ")"

			EmployerId = m_DB.InsertSQL(SQL)

			Return EmployerId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE Employer SET " _
			 & " EmployerName = " & m_DB.Quote(EmployerName) _
			 & ",Image = " & m_DB.Quote(Image) _
			 & ",IsFeatured = " & CInt(IsFeatured) _
			 & " WHERE EmployerId = " & m_DB.quote(EmployerId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM Employer WHERE EmployerId = " & m_DB.Quote(EmployerId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class EmployerCollection
		Inherits GenericCollection(Of EmployerRow)
	End Class

End Namespace


