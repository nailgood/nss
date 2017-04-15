Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionMixMatchLinesRow
		Inherits NavisionMixMatchLinesRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionMixMatchLinesCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionMixMatchLinesRow
                Dim c As New NavisionMixMatchLinesCollection
                Dim SQL As String = "select * from _NAVISION_MIX_MATCH_LINES WHERE Offer_No in (select Mix_Match_No from _NAVISION_MIX_MATCH union select mixmatchno from mixmatch)"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionMixMatchLinesRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionMixMatchLinesCollection
        End Function

	End Class

	Public MustInherit Class NavisionMixMatchLinesRowBase
		Private m_DB As Database
		Private m_Offer_No As String = Nothing
		Private m_Line_No As Integer = Nothing
		Private m_Mix_Match_No As String = Nothing
		Private m_Description As String = Nothing
		Private m_Standard_Price_Including_VAT As Double = Nothing
		Private m_Standard_Price As Double = Nothing
		Private m_Deal_Price As Double = Nothing
		Private m_No_Items_Needed As Integer = Nothing
		Private m_Disc_Type As String = Nothing
		Private m_Status As String = Nothing
		Private m_ID As Integer = Nothing

		Public Property Offer_No() As String
			Get
				Return Trim(m_Offer_No)
			End Get
			Set(ByVal Value As String)
				m_Offer_No = Trim(Value)
			End Set
		End Property

		Public Property Line_No() As Integer
			Get
				Return Trim(m_Line_No)
			End Get
			Set(ByVal Value As Integer)
				m_Line_No = Trim(Value)
			End Set
		End Property

		Public Property Mix_Match_No() As String
			Get
				Return Trim(m_Mix_Match_No)
			End Get
			Set(ByVal Value As String)
				m_Mix_Match_No = Trim(Value)
			End Set
		End Property

		Public Property Description() As String
			Get
				Return Trim(m_Description)
			End Get
			Set(ByVal Value As String)
				m_Description = Trim(Value)
			End Set
		End Property

		Public Property Standard_Price_Including_VAT() As Double
			Get
				Return Trim(m_Standard_Price_Including_VAT)
			End Get
			Set(ByVal Value As Double)
				m_Standard_Price_Including_VAT = Trim(Value)
			End Set
		End Property

		Public Property Standard_Price() As Double
			Get
				Return Trim(m_Standard_Price)
			End Get
			Set(ByVal Value As Double)
				m_Standard_Price = Trim(Value)
			End Set
		End Property

		Public Property Deal_Price() As Double
			Get
				Return Trim(m_Deal_Price)
			End Get
			Set(ByVal Value As Double)
				m_Deal_Price = Trim(Value)
			End Set
		End Property

		Public Property No_Items_Needed() As Integer
			Get
				Return Trim(m_No_Items_Needed)
			End Get
			Set(ByVal Value As Integer)
				m_No_Items_Needed = Trim(Value)
			End Set
		End Property

		Public Property Disc_Type() As String
			Get
				Return Trim(m_Disc_Type)
			End Get
			Set(ByVal Value As String)
				m_Disc_Type = Trim(Value)
			End Set
		End Property

		Public Property Status() As String
			Get
				Return Trim(m_Status)
			End Get
			Set(ByVal Value As String)
				m_Status = Trim(Value)
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
			m_Offer_No = Convert.ToString(r.Item("Offer_No"))
			m_Line_No = Convert.ToInt32(r.Item("Line_No"))
			If IsDBNull(r.Item("Mix_Match_No")) Then
				m_Mix_Match_No = Nothing
			Else
				m_Mix_Match_No = Convert.ToString(r.Item("Mix_Match_No"))
			End If
			If IsDBNull(r.Item("Description")) Then
				m_Description = Nothing
			Else
				m_Description = Convert.ToString(r.Item("Description"))
			End If
			If IsDBNull(r.Item("Standard_Price_Including_VAT")) Then
				m_Standard_Price_Including_VAT = Nothing
			Else
				m_Standard_Price_Including_VAT = Convert.ToDouble(r.Item("Standard_Price_Including_VAT"))
			End If
			If IsDBNull(r.Item("Standard_Price")) Then
				m_Standard_Price = Nothing
			Else
				m_Standard_Price = Convert.ToDouble(r.Item("Standard_Price"))
			End If
			If IsDBNull(r.Item("Deal_Price")) Then
				m_Deal_Price = Nothing
			Else
				m_Deal_Price = Convert.ToDouble(r.Item("Deal_Price"))
			End If
			If IsDBNull(r.Item("No_Items_Needed")) Then
				m_No_Items_Needed = Nothing
			Else
				m_No_Items_Needed = Convert.ToInt32(r.Item("No_Items_Needed"))
			End If
			If IsDBNull(r.Item("Disc_Type")) Then
				m_Disc_Type = Nothing
			Else
				m_Disc_Type = Convert.ToString(r.Item("Disc_Type"))
			End If
			If IsDBNull(r.Item("Status")) Then
				m_Status = Nothing
			Else
				m_Status = Convert.ToString(r.Item("Status"))
			End If
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionMixMatchLinesCollection
		Inherits GenericCollection(Of NavisionMixMatchLinesRow)
	End Class

End Namespace


