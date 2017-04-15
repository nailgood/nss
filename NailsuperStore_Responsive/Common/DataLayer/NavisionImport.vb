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

	Public Class NavisionImportRow
		Inherits NavisionImportRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal FileName As String)
			MyBase.New(DB, FileName)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal FileName As String) As NavisionImportRow
			Dim row As NavisionImportRow

			row = New NavisionImportRow(DB, FileName)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal FileName As String)
			Dim row As NavisionImportRow

			row = New NavisionImportRow(DB, FileName)
			row.Remove()
		End Sub
        
        'Custom Methods

	End Class

	Public MustInherit Class NavisionImportRowBase
		Private m_DB As Database
		Private m_FileName As String = Nothing
		Private m_ImportDate As DateTime = Nothing
		Private m_BCPStart As DateTime = Nothing
		Private m_BCPDate As DateTime = Nothing
		Private m_BCPFail As Boolean = Nothing
		Private m_RowsImported As Integer = Nothing
		Private m_SortOrder As Integer = Nothing

		Public Property FileName() As String
			Get
				Return m_FileName
			End Get
			Set(ByVal Value As String)
				m_FileName = Value
			End Set
		End Property

		Public Property ImportDate() As DateTime
			Get
				Return m_ImportDate
			End Get
			Set(ByVal Value As DateTime)
				m_ImportDate = value
			End Set
		End Property

		Public Property SortOrder() As Integer
			Get
				Return m_SortOrder
			End Get
			Set(ByVal Value As Integer)
				m_SortOrder = Value
			End Set
		End Property

		Public Property BCPStart() As DateTime
			Get
				Return m_BCPStart
			End Get
			Set(ByVal Value As DateTime)
				m_BCPStart = value
			End Set
		End Property

		Public Property BCPDate() As DateTime
			Get
				Return m_BCPDate
			End Get
			Set(ByVal Value As DateTime)
				m_BCPDate = Value
			End Set
		End Property

		Public Property BCPFail() As Boolean
			Get
				Return m_BCPFail
			End Get
			Set(ByVal Value As Boolean)
				m_BCPFail = value
			End Set
		End Property

		Public Property RowsImported() As Integer
			Get
				Return m_RowsImported
			End Get
			Set(ByVal Value As Integer)
				m_RowsImported = value
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

		Public Sub New(ByVal DB As Database, ByVal FileName As String)
			m_DB = DB
			m_FileName = FileName
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM NavisionImport WHERE FileName = " & DB.Quote(FileName)
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
			m_FileName = Convert.ToString(r.Item("FileName"))
			If IsDBNull(r.Item("SortOrder")) Then
				m_SortOrder = Nothing
			Else
				m_SortOrder = Convert.ToInt32(r.Item("SortOrder"))
			End If
			If IsDBNull(r.Item("ImportDate")) Then
				m_ImportDate = Nothing
			Else
				m_ImportDate = Convert.ToDateTime(r.Item("ImportDate"))
			End If
			If IsDBNull(r.Item("BCPStart")) Then
				m_BCPStart = Nothing
			Else
				m_BCPStart = Convert.ToDateTime(r.Item("BCPStart"))
			End If
			If IsDBNull(r.Item("BCPDate")) Then
				m_BCPDate = Nothing
			Else
				m_BCPDate = Convert.ToDateTime(r.Item("BCPDate"))
			End If
			m_BCPFail = Convert.ToBoolean(r.Item("BCPFail"))
			If IsDBNull(r.Item("RowsImported")) Then
				m_RowsImported = Nothing
			Else
				m_RowsImported = Convert.ToInt32(r.Item("RowsImported"))
			End If
		End Sub	'Load

		Public Overridable Sub Insert()
			Dim SQL As String


			SQL = " INSERT INTO NavisionImport (" _
			 & " ImportDate" _
			 & ",FileName" _
			 & ",SortOrder" _
			 & ",BCPStart" _
			 & ",BCPDate" _
			 & ",BCPFail" _
			 & ",RowsImported" _
			 & ") VALUES (" _
			 & m_DB.NullQuote(ImportDate) _
			 & "," & m_DB.Quote(FileName) _
			 & "," & m_DB.Number(SortOrder) _
			 & "," & m_DB.NullQuote(BCPStart) _
			 & "," & m_DB.NullQuote(BCPDate) _
			 & "," & CInt(BCPFail) _
			 & "," & m_DB.Number(RowsImported) _
			 & ")"

			m_DB.ExecuteSQL(SQL)
		End Sub

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE NavisionImport SET " _
			 & " ImportDate = " & m_DB.NullQuote(ImportDate) _
			 & ",FileName = " & m_DB.Quote(FileName) _
			 & ",SortOrder = " & m_DB.Number(SortOrder) _
			 & ",BCPStart = " & m_DB.NullQuote(BCPStart) _
			 & ",BCPDate = " & m_DB.NullQuote(BCPDate) _
			 & ",BCPFail = " & CInt(BCPFail) _
			 & ",RowsImported = " & m_DB.Number(RowsImported) _
			 & " WHERE FileName = " & m_DB.Quote(FileName)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM NavisionImport WHERE FileName = " & m_DB.Quote(FileName)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class NavisionImportCollection
		Inherits GenericCollection(Of NavisionImportRow)
	End Class

End Namespace


