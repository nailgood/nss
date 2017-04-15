Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionCustomerPriceGroupRow
		Inherits NavisionCustomerPriceGroupRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionCustomerPriceGroupCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionCustomerPriceGroupRow
                Dim c As New NavisionCustomerPriceGroupCollection
                Dim SQL As String = "select * from _NAVISION_CUSTOMER_PRICE_GROUP"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionCustomerPriceGroupRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionCustomerPriceGroupCollection
        End Function
	End Class

	Public MustInherit Class NavisionCustomerPriceGroupRowBase
		Private m_DB As Database
		Private m_Id As Integer = Nothing
		Private m_Code As String = Nothing
		Private m_Price_Includes_VAT As String = Nothing
		Private m_Allow_Invoice_Disc As String = Nothing
		Private m_VAT_Bus_Posting_Gr As String = Nothing
		Private m_Description As String = Nothing
		Private m_Allow_Line_Disc As String = Nothing
		Private m_Currency_Code As String = Nothing
		Private m_Distribution_Group_Code As String = Nothing
		Private m_Distribution_Subgroup_Code As String = Nothing
		Private m_Retail_Price_Group As String = Nothing

		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal Value As Integer)
				m_Id = Value
			End Set
		End Property

		Public Property Code() As String
			Get
				Return m_Code
			End Get
			Set(ByVal Value As String)
				m_Code = Value
			End Set
		End Property

		Public Property Price_Includes_VAT() As String
			Get
				Return m_Price_Includes_VAT
			End Get
			Set(ByVal Value As String)
				m_Price_Includes_VAT = Value
			End Set
		End Property

		Public Property Allow_Invoice_Disc() As String
			Get
				Return m_Allow_Invoice_Disc
			End Get
			Set(ByVal Value As String)
				m_Allow_Invoice_Disc = Value
			End Set
		End Property

		Public Property VAT_Bus_Posting_Gr() As String
			Get
				Return m_VAT_Bus_Posting_Gr
			End Get
			Set(ByVal Value As String)
				m_VAT_Bus_Posting_Gr = Value
			End Set
		End Property

		Public Property Description() As String
			Get
				Return m_Description
			End Get
			Set(ByVal Value As String)
				m_Description = Value
			End Set
		End Property

		Public Property Allow_Line_Disc() As String
			Get
				Return m_Allow_Line_Disc
			End Get
			Set(ByVal Value As String)
				m_Allow_Line_Disc = Value
			End Set
		End Property

		Public Property Currency_Code() As String
			Get
				Return m_Currency_Code
			End Get
			Set(ByVal Value As String)
				m_Currency_Code = Value
			End Set
		End Property

		Public Property Distribution_Group_Code() As String
			Get
				Return m_Distribution_Group_Code
			End Get
			Set(ByVal Value As String)
				m_Distribution_Group_Code = Value
			End Set
		End Property

		Public Property Distribution_Subgroup_Code() As String
			Get
				Return m_Distribution_Subgroup_Code
			End Get
			Set(ByVal Value As String)
				m_Distribution_Subgroup_Code = Value
			End Set
		End Property

		Public Property Retail_Price_Group() As String
			Get
				Return m_Retail_Price_Group
			End Get
			Set(ByVal Value As String)
				m_Retail_Price_Group = Value
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
			m_Id = Convert.ToInt32(r.Item("Id"))
			If IsDBNull(r.Item("Code")) Then
				m_Code = Nothing
			Else
				m_Code = Convert.ToString(r.Item("Code"))
			End If
			If IsDBNull(r.Item("Price_Includes_VAT")) Then
				m_Price_Includes_VAT = Nothing
			Else
				m_Price_Includes_VAT = Convert.ToString(r.Item("Price_Includes_VAT"))
			End If
			If IsDBNull(r.Item("Allow_Invoice_Disc")) Then
				m_Allow_Invoice_Disc = Nothing
			Else
				m_Allow_Invoice_Disc = Convert.ToString(r.Item("Allow_Invoice_Disc"))
			End If
			If IsDBNull(r.Item("VAT_Bus_Posting_Gr")) Then
				m_VAT_Bus_Posting_Gr = Nothing
			Else
				m_VAT_Bus_Posting_Gr = Convert.ToString(r.Item("VAT_Bus_Posting_Gr"))
			End If
			If IsDBNull(r.Item("Description")) Then
				m_Description = Nothing
			Else
				m_Description = Convert.ToString(r.Item("Description"))
			End If
			If IsDBNull(r.Item("Allow_Line_Disc")) Then
				m_Allow_Line_Disc = Nothing
			Else
				m_Allow_Line_Disc = Convert.ToString(r.Item("Allow_Line_Disc"))
			End If
			If IsDBNull(r.Item("Currency_Code")) Then
				m_Currency_Code = Nothing
			Else
				m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
			End If
			If IsDBNull(r.Item("Distribution_Group_Code")) Then
				m_Distribution_Group_Code = Nothing
			Else
				m_Distribution_Group_Code = Convert.ToString(r.Item("Distribution_Group_Code"))
			End If
			If IsDBNull(r.Item("Distribution_Subgroup_Code")) Then
				m_Distribution_Subgroup_Code = Nothing
			Else
				m_Distribution_Subgroup_Code = Convert.ToString(r.Item("Distribution_Subgroup_Code"))
			End If
			If IsDBNull(r.Item("Retail_Price_Group")) Then
				m_Retail_Price_Group = Nothing
			Else
				m_Retail_Price_Group = Convert.ToString(r.Item("Retail_Price_Group"))
			End If
		End Sub	'Load
	End Class

	Public Class NavisionCustomerPriceGroupCollection
		Inherits GenericCollection(Of NavisionCustomerPriceGroupRow)
	End Class

End Namespace


