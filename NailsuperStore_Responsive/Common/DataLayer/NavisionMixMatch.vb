Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionMixMatchRow
		Inherits NavisionMixMatchRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionMixMatchCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionMixMatchRow
                Dim c As New NavisionMixMatchCollection
                Dim SQL As String = "select * from _NAVISION_MIX_MATCH"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionMixMatchRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionMixMatchCollection
        End Function

	End Class

	Public MustInherit Class NavisionMixMatchRowBase
		Private m_DB As Database
		Private m_Mix_Match_No As String = Nothing
		Private m_Description As String = Nothing
		Private m_Status As String = Nothing
		Private m_Price_Group As String = Nothing
		Private m_Currency_Code As String = Nothing
		Private m_Disc_Validation_Period_ID As String = Nothing
		Private m_Disc_Validation_Descr As String = Nothing
		Private m_Disc_Starting_Date As DateTime = Nothing
		Private m_Disc_Ending_Date As DateTime = Nothing
		Private m_Discount_Type As String = Nothing
		Private m_Same_Diff_MM_Lines As String = Nothing
		Private m_No_Lines_to_Trigger As Integer = Nothing
		Private m_No_Times_Applicable As Integer = Nothing
		Private m_No_Non_Mandatory As Integer = Nothing
		Private m_No_Mandatory As Integer = Nothing
		Private m_ID As Integer = Nothing

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

		Public Property Status() As String
			Get
				Return Trim(m_Status)
			End Get
			Set(ByVal Value As String)
				m_Status = Trim(Value)
			End Set
		End Property

		Public Property Price_Group() As String
			Get
				Return Trim(m_Price_Group)
			End Get
			Set(ByVal Value As String)
				m_Price_Group = Trim(Value)
			End Set
		End Property

		Public Property Currency_Code() As String
			Get
				Return Trim(m_Currency_Code)
			End Get
			Set(ByVal Value As String)
				m_Currency_Code = Trim(Value)
			End Set
		End Property

		Public Property Disc_Validation_Period_ID() As String
			Get
				Return Trim(m_Disc_Validation_Period_ID)
			End Get
			Set(ByVal Value As String)
				m_Disc_Validation_Period_ID = Trim(Value)
			End Set
		End Property

		Public Property Disc_Validation_Descr() As String
			Get
				Return Trim(m_Disc_Validation_Descr)
			End Get
			Set(ByVal Value As String)
				m_Disc_Validation_Descr = Trim(Value)
			End Set
		End Property

		Public Property Disc_Starting_Date() As DateTime
			Get
				Return Trim(m_Disc_Starting_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Disc_Starting_Date = Trim(Value)
			End Set
		End Property

		Public Property Disc_Ending_Date() As DateTime
			Get
				Return Trim(m_Disc_Ending_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Disc_Ending_Date = Trim(Value)
			End Set
		End Property

		Public Property Discount_Type() As String
			Get
				Return Trim(m_Discount_Type)
			End Get
			Set(ByVal Value As String)
				m_Discount_Type = Trim(Value)
			End Set
		End Property

		Public Property Same_Diff_MM_Lines() As String
			Get
				Return Trim(m_Same_Diff_MM_Lines)
			End Get
			Set(ByVal Value As String)
				m_Same_Diff_MM_Lines = Trim(Value)
			End Set
		End Property

		Public Property No_Lines_to_Trigger() As Integer
			Get
				Return Trim(m_No_Lines_to_Trigger)
			End Get
			Set(ByVal Value As Integer)
				m_No_Lines_to_Trigger = Trim(Value)
			End Set
		End Property

		Public Property No_Times_Applicable() As Integer
			Get
				Return Trim(m_No_Times_Applicable)
			End Get
			Set(ByVal Value As Integer)
				m_No_Times_Applicable = Trim(Value)
			End Set
		End Property

		Public Property No_Non_Mandatory() As Integer
			Get
				Return Trim(m_No_Non_Mandatory)
			End Get
			Set(ByVal Value As Integer)
				m_No_Non_Mandatory = Trim(Value)
			End Set
		End Property

		Public Property No_Mandatory() As Integer
			Get
				Return Trim(m_No_Mandatory)
			End Get
			Set(ByVal Value As Integer)
				m_No_Mandatory = Trim(Value)
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
			m_Mix_Match_No = Convert.ToString(r.Item("Mix_Match_No"))
			If IsDBNull(r.Item("Description")) Then
				m_Description = Nothing
			Else
				m_Description = Convert.ToString(r.Item("Description"))
			End If
			If IsDBNull(r.Item("Status")) Then
				m_Status = Nothing
			Else
				m_Status = Convert.ToString(r.Item("Status"))
			End If
			If IsDBNull(r.Item("Price_Group")) Then
				m_Price_Group = Nothing
			Else
				m_Price_Group = Convert.ToString(r.Item("Price_Group"))
			End If
			If IsDBNull(r.Item("Currency_Code")) Then
				m_Currency_Code = Nothing
			Else
				m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
			End If
			If IsDBNull(r.Item("Disc_Validation_Period_ID")) Then
				m_Disc_Validation_Period_ID = Nothing
			Else
				m_Disc_Validation_Period_ID = Convert.ToString(r.Item("Disc_Validation_Period_ID"))
			End If
			If IsDBNull(r.Item("Disc_Validation_Descr")) Then
				m_Disc_Validation_Descr = Nothing
			Else
				m_Disc_Validation_Descr = Convert.ToString(r.Item("Disc_Validation_Descr"))
			End If
			If IsDBNull(r.Item("Disc_Starting_Date")) Then
				m_Disc_Starting_Date = Nothing
			Else
				m_Disc_Starting_Date = Convert.ToDateTime(r.Item("Disc_Starting_Date"))
			End If
			If IsDBNull(r.Item("Disc_Ending_Date")) Then
				m_Disc_Ending_Date = Nothing
			Else
				m_Disc_Ending_Date = Convert.ToDateTime(r.Item("Disc_Ending_Date"))
			End If
			If IsDBNull(r.Item("Discount_Type")) Then
				m_Discount_Type = Nothing
			Else
				m_Discount_Type = Convert.ToString(r.Item("Discount_Type"))
			End If
			If IsDBNull(r.Item("Same_Diff_MM_Lines")) Then
				m_Same_Diff_MM_Lines = Nothing
			Else
				m_Same_Diff_MM_Lines = Convert.ToString(r.Item("Same_Diff_MM_Lines"))
			End If
			If IsDBNull(r.Item("No_Lines_to_Trigger")) Then
				m_No_Lines_to_Trigger = Nothing
			Else
				m_No_Lines_to_Trigger = Convert.ToInt32(r.Item("No_Lines_to_Trigger"))
			End If
			If IsDBNull(r.Item("No_Times_Applicable")) Then
				m_No_Times_Applicable = Nothing
			Else
				m_No_Times_Applicable = Convert.ToInt32(r.Item("No_Times_Applicable"))
			End If
			If IsDBNull(r.Item("No_Non_Mandatory")) Then
				m_No_Non_Mandatory = Nothing
			Else
				m_No_Non_Mandatory = Convert.ToInt32(r.Item("No_Non_Mandatory"))
			End If
			If IsDBNull(r.Item("No_Mandatory")) Then
				m_No_Mandatory = Nothing
			Else
				m_No_Mandatory = Convert.ToInt32(r.Item("No_Mandatory"))
			End If
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionMixMatchCollection
		Inherits GenericCollection(Of NavisionMixMatchRow)
	End Class

End Namespace


