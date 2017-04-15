Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class StoreItemDiscountGroupRow
		Inherits StoreItemDiscountGroupRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ItemDiscountGroupId As Integer)
			MyBase.New(DB, ItemDiscountGroupId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ItemDiscountGroupCode As String)
			MyBase.New(DB, ItemDiscountGroupCode)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ItemDiscountGroupId As Integer) As StoreItemDiscountGroupRow
			Dim row As StoreItemDiscountGroupRow

			row = New StoreItemDiscountGroupRow(DB, ItemDiscountGroupId)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal ItemDiscountGroupCode As String) As StoreItemDiscountGroupRow
			Dim row As StoreItemDiscountGroupRow

			row = New StoreItemDiscountGroupRow(DB, ItemDiscountGroupCode)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ItemDiscountGroupId As Integer)
			Dim row As StoreItemDiscountGroupRow

			row = New StoreItemDiscountGroupRow(DB, ItemDiscountGroupId)
			row.Remove()
		End Sub

		'Custom Methods
		Public Sub CopyFromNavision(ByVal r As NavisionItemDiscountGroupRow)
			ItemDiscountGroupCode = r.Code
			Description = r.Description

			If ItemDiscountGroupId = Nothing Then
				Insert()
			Else
				Update()
			End If
		End Sub

	End Class

	Public MustInherit Class StoreItemDiscountGroupRowBase
		Private m_DB As Database
		Private m_ItemDiscountGroupId As Integer = Nothing
		Private m_ItemDiscountGroupCode As String = Nothing
		Private m_Description As String = Nothing


		Public Property ItemDiscountGroupId() As Integer
			Get
				Return m_ItemDiscountGroupId
			End Get
			Set(ByVal Value As Integer)
				m_ItemDiscountGroupId = value
			End Set
		End Property

		Public Property ItemDiscountGroupCode() As String
			Get
				Return m_ItemDiscountGroupCode
			End Get
			Set(ByVal Value As String)
				m_ItemDiscountGroupCode = value
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

		Public Sub New(ByVal DB As Database, ByVal ItemDiscountGroupId As Integer)
			m_DB = DB
			m_ItemDiscountGroupId = ItemDiscountGroupId
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ItemDiscountGroupCode As String)
			m_DB = DB
			m_ItemDiscountGroupCode = ItemDiscountGroupCode
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreItemDiscountGroup WHERE " & IIf(ItemDiscountGroupId <> Nothing, "ItemDiscountGroupId = " & DB.Number(ItemDiscountGroupId), "ItemDiscountGroupCode = " & m_DB.Quote(ItemDiscountGroupCode))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
			

		End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_ItemDiscountGroupId = Convert.ToInt32(r.Item("ItemDiscountGroupId"))
                If IsDBNull(r.Item("ItemDiscountGroupCode")) Then
                    m_ItemDiscountGroupCode = Nothing
                Else
                    m_ItemDiscountGroupCode = Convert.ToString(r.Item("ItemDiscountGroupCode"))
                End If
                If IsDBNull(r.Item("Description")) Then
                    m_Description = Nothing
                Else
                    m_Description = Convert.ToString(r.Item("Description"))
                End If
            Catch ex As Exception
                Throw ex
                ''  Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO StoreItemDiscountGroup (" _
			 & " ItemDiscountGroupCode" _
			 & ",Description" _
			 & ") VALUES (" _
			 & m_DB.Quote(ItemDiscountGroupCode) _
			 & "," & m_DB.Quote(Description) _
			 & ")"

			ItemDiscountGroupId = m_DB.InsertSQL(SQL)

			Return ItemDiscountGroupId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE StoreItemDiscountGroup SET " _
			 & " ItemDiscountGroupCode = " & m_DB.Quote(ItemDiscountGroupCode) _
			 & ",Description = " & m_DB.Quote(Description) _
			 & " WHERE ItemDiscountGroupId = " & m_DB.quote(ItemDiscountGroupId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM StoreItemDiscountGroup WHERE ItemDiscountGroupId = " & m_DB.Quote(ItemDiscountGroupId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class StoreItemDiscountGroupCollection
		Inherits GenericCollection(Of StoreItemDiscountGroupRow)
	End Class

End Namespace


