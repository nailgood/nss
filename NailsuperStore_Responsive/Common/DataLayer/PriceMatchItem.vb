Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class PriceMatchItemRow
		Inherits PriceMatchItemRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal PriceMatchItemId As Integer)
			MyBase.New(DB, PriceMatchItemId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal PriceMatchItemId As Integer) As PriceMatchItemRow
			Dim row As PriceMatchItemRow

			row = New PriceMatchItemRow(DB, PriceMatchItemId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal PriceMatchItemId As Integer)
			Dim row As PriceMatchItemRow

			row = New PriceMatchItemRow(DB, PriceMatchItemId)
			row.Remove()
		End Sub

		'Custom Methods

	End Class

	Public MustInherit Class PriceMatchItemRowBase
		Private m_DB As Database
		Private m_PriceMatchItemId As Integer = Nothing
		Private m_PriceMatchId As Integer = Nothing
		Private m_ItemNumberProductName As String = Nothing
		Private m_CompetitorPrice As Double = Nothing


		Public Property PriceMatchItemId() As Integer
			Get
				Return m_PriceMatchItemId
			End Get
			Set(ByVal Value As Integer)
				m_PriceMatchItemId = value
			End Set
		End Property

		Public Property PriceMatchId() As Integer
			Get
				Return m_PriceMatchId
			End Get
			Set(ByVal Value As Integer)
				m_PriceMatchId = value
			End Set
		End Property

		Public Property ItemNumberProductName() As String
			Get
				Return m_ItemNumberProductName
			End Get
			Set(ByVal Value As String)
				m_ItemNumberProductName = value
			End Set
		End Property

		Public Property CompetitorPrice() As Double
			Get
				Return m_CompetitorPrice
			End Get
			Set(ByVal Value As Double)
				m_CompetitorPrice = value
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

		Public Sub New(ByVal DB As Database, ByVal PriceMatchItemId As Integer)
			m_DB = DB
			m_PriceMatchItemId = PriceMatchItemId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM PriceMatchItem WHERE PriceMatchItemId = " & DB.Number(PriceMatchItemId)
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
			m_PriceMatchItemId = Convert.ToInt32(r.Item("PriceMatchItemId"))
			m_PriceMatchId = Convert.ToInt32(r.Item("PriceMatchId"))
			m_ItemNumberProductName = Convert.ToString(r.Item("ItemNumberProductName"))
			m_CompetitorPrice = Convert.ToDouble(r.Item("CompetitorPrice"))
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO PriceMatchItem (" _
			 & " PriceMatchId" _
			 & ",ItemNumberProductName" _
			 & ",CompetitorPrice" _
			 & ") VALUES (" _
			 & m_DB.NullNumber(PriceMatchId) _
			 & "," & m_DB.Quote(ItemNumberProductName) _
			 & "," & m_DB.Number(CompetitorPrice) _
			 & ")"

			PriceMatchItemId = m_DB.InsertSQL(SQL)

			Return PriceMatchItemId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE PriceMatchItem SET " _
			 & " PriceMatchId = " & m_DB.NullNumber(PriceMatchId) _
			 & ",ItemNumberProductName = " & m_DB.Quote(ItemNumberProductName) _
			 & ",CompetitorPrice = " & m_DB.Number(CompetitorPrice) _
			 & " WHERE PriceMatchItemId = " & m_DB.quote(PriceMatchItemId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM PriceMatchItem WHERE PriceMatchItemId = " & m_DB.Quote(PriceMatchItemId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class PriceMatchItemCollection
		Inherits GenericCollection(Of PriceMatchItemRow)
	End Class

End Namespace


