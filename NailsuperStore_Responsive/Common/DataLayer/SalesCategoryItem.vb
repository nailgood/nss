Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class SalesCategoryItemRow
		Inherits SalesCategoryItemRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal SalesCategoryId As Integer)
			MyBase.New(DB, SalesCategoryId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SalesCategoryId As Integer, ByVal ItemId As Integer)
            MyBase.New(DB, SalesCategoryId, ItemId)
        End Sub 'New

		'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SalesCategoryId As Integer) As SalesCategoryItemRow
            Dim row As SalesCategoryItemRow

            row = New SalesCategoryItemRow(DB, SalesCategoryId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SalesCategoryId As Integer, ByVal ItemId As Integer)
            Dim row As SalesCategoryItemRow

            row = New SalesCategoryItemRow(DB, SalesCategoryId, ItemId)
            row.Remove()
        End Sub

		'Custom Methods

	End Class

	Public MustInherit Class SalesCategoryItemRowBase
		Private m_DB As Database
		Private m_SalesCategoryId As Integer = Nothing
		Private m_ItemId As Integer = Nothing
		Private m_SortOrder As Integer = Nothing
        Private m_Id As Integer = Nothing
        Public Shared cachePrefixKey As String = "SalesCategoryItem_"

		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal value As Integer)
				m_Id = value
			End Set
		End Property

		Public Property SalesCategoryId() As Integer
			Get
				Return m_SalesCategoryId
			End Get
			Set(ByVal Value As Integer)
				m_SalesCategoryId = value
			End Set
		End Property

		Public Property ItemId() As Integer
			Get
				Return m_ItemId
			End Get
			Set(ByVal Value As Integer)
				m_ItemId = value
			End Set
		End Property

		Public Property SortOrder() As Integer
			Get
				Return m_SortOrder
			End Get
			Set(ByVal Value As Integer)
				m_SortOrder = value
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

		Public Sub New(ByVal DB As Database, ByVal SalesCategoryId As Integer)
			m_DB = DB
			m_SalesCategoryId = SalesCategoryId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SalesCategoryId As Integer, ByVal ItemId As Integer)
            m_DB = DB
            m_SalesCategoryId = SalesCategoryId
            m_ItemId = ItemId
        End Sub 'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM SalesCategoryItem WHERE " & IIf(Id <> Nothing, "Id = " & DB.Number(Id), "SalesCategoryId = " & SalesCategoryId & " and itemid = " & ItemId)
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
			m_SalesCategoryId = Convert.ToInt32(r.Item("SalesCategoryId"))
			m_ItemId = Convert.ToInt32(r.Item("ItemId"))
			m_Id = Convert.ToInt32(r.Item("Id"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String

			Dim MaxSortOrder As Integer = DB.ExecuteScalar("select top 1 SortOrder from SalesCategoryItem order by SortOrder desc")
			MaxSortOrder += 1

			SQL = " INSERT INTO SalesCategoryItem (" _
			 & " ItemId" _
			 & ",SalesCategoryid" _
			 & ",SortOrder" _
			 & ") VALUES (" _
			 & m_DB.NullNumber(ItemId) _
			 & "," & SalesCategoryId _
			 & "," & MaxSortOrder _
			 & ")"

			SalesCategoryId = m_DB.InsertSQL(SQL)
            Utility.CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
			Return SalesCategoryId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE SalesCategoryItem SET " _
			 & " ItemId = " & m_DB.NullNumber(ItemId) _
			 & ",SalesCategoryId = " & m_DB.NullNumber(SalesCategoryId) _
			 & " WHERE Id = " & m_DB.Quote(Id)

            m_DB.ExecuteSQL(SQL)
            Utility.CacheUtils.ClearCacheWithPrefix(cachePrefixKey)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

            SQL = "DELETE FROM SalesCategoryItem WHERE SalesCategoryId = " & m_DB.Quote(SalesCategoryId) & " and ItemId=  " & m_DB.Quote(ItemId)
            m_DB.ExecuteSQL(SQL)
            Utility.CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
		End Sub	'Remove
	End Class

	Public Class SalesCategoryItemCollection
		Inherits GenericCollection(Of SalesCategoryItemRow)
	End Class

End Namespace


