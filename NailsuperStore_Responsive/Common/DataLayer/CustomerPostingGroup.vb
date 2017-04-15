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

	Public Class CustomerPostingGroupRow
		Inherits CustomerPostingGroupRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal Id As Integer)
			MyBase.New(DB, Id)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As CustomerPostingGroupRow
			Dim row As CustomerPostingGroupRow

			row = New CustomerPostingGroupRow(DB, Id)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
			Dim row As CustomerPostingGroupRow

			row = New CustomerPostingGroupRow(DB, Id)
			row.Remove()
		End Sub

		Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
			Dim SQL As String = "select * from CustomerPostingGroup"
			If Not SortBy = String.Empty Then
				SortBy = Core.ProtectParam(SortBy)
				SortOrder = Core.ProtectParam(SortOrder)

				SQL &= " order by " & SortBy & " " & SortOrder
			End If
			Return DB.GetDataTable(SQL)
		End Function
       
        'Custom Methods

	End Class

	Public MustInherit Class CustomerPostingGroupRowBase
		Private m_DB As Database
		Private m_Id As Integer = Nothing
		Private m_Code As String = Nothing


		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal Value As Integer)
				m_Id = value
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

		Public Sub New(ByVal DB As Database, ByVal Id As Integer)
			m_DB = DB
			m_Id = Id
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM CustomerPostingGroup WHERE Id = " & DB.Number(Id)
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
			m_Id = Convert.ToInt32(r.Item("Id"))
			m_Code = Convert.ToString(r.Item("Code"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO CustomerPostingGroup (" _
			 & " Code" _
			 & ") VALUES (" _
			 & m_DB.Quote(Code) _
			 & ")"

			Id = m_DB.InsertSQL(SQL)

			Return Id
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE CustomerPostingGroup SET " _
			 & " Code = " & m_DB.Quote(Code) _
			 & " WHERE Id = " & m_DB.quote(Id)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM CustomerPostingGroup WHERE Id = " & m_DB.Number(Id)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class CustomerPostingGroupCollection
		Inherits GenericCollection(Of CustomerPostingGroupRow)
	End Class

End Namespace


