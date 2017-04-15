Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class JobCategoryRow
		Inherits JobCategoryRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal JobCategoryId As Integer)
			MyBase.New(DB, JobCategoryId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal JobCategoryId As Integer) As JobCategoryRow
			Dim row As JobCategoryRow

			row = New JobCategoryRow(DB, JobCategoryId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal JobCategoryId As Integer)
			Dim row As JobCategoryRow

			row = New JobCategoryRow(DB, JobCategoryId)
			row.Remove()
		End Sub

		'Custom Methods
		Public Shared Function GetAllJobCategories(ByVal DB As Database) As DataSet
			Dim ds As DataSet = DB.GetDataSet("select * from JobCategory order by JobCategory")
			Return ds
		End Function

	End Class

	Public MustInherit Class JobCategoryRowBase
		Private m_DB As Database
		Private m_JobCategoryId As Integer = Nothing
		Private m_JobCategory As String = Nothing


		Public Property JobCategoryId() As Integer
			Get
				Return m_JobCategoryId
			End Get
			Set(ByVal Value As Integer)
				m_JobCategoryId = value
			End Set
		End Property

		Public Property JobCategory() As String
			Get
				Return m_JobCategory
			End Get
			Set(ByVal Value As String)
				m_JobCategory = value
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

		Public Sub New(ByVal DB As Database, ByVal JobCategoryId As Integer)
			m_DB = DB
			m_JobCategoryId = JobCategoryId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM JobCategory WHERE JobCategoryId = " & DB.Number(JobCategoryId)
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
			m_JobCategoryId = Convert.ToInt32(r.Item("JobCategoryId"))
			If IsDBNull(r.Item("JobCategory")) Then
				m_JobCategory = Nothing
			Else
				m_JobCategory = Convert.ToString(r.Item("JobCategory"))
			End If
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO JobCategory (" _
			 & " JobCategory" _
			 & ") VALUES (" _
			 & m_DB.Quote(JobCategory) _
			 & ")"

			JobCategoryId = m_DB.InsertSQL(SQL)

			Return JobCategoryId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE JobCategory SET " _
			 & " JobCategory = " & m_DB.Quote(JobCategory) _
			 & " WHERE JobCategoryId = " & m_DB.quote(JobCategoryId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM JobCategory WHERE JobCategoryId = " & m_DB.Quote(JobCategoryId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class JobCategoryCollection
		Inherits GenericCollection(Of JobCategoryRow)
	End Class

End Namespace


