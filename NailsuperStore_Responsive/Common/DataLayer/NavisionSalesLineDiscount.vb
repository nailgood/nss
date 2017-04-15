Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionSalesLineDiscountRow
		Inherits NavisionSalesLineDiscountRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
		Public Shared Function GetCollection(ByVal DB As Database) As NavisionSalesLineDiscountCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New NavisionSalesLineDiscountCollection
            Try
                Dim row As NavisionSalesLineDiscountRow
                Dim SQL As String = "select * from _NAVISION_SALES_LINE_DISCOUNT"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionSalesLineDiscountRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
			Return c
		End Function
	End Class

	Public MustInherit Class NavisionSalesLineDiscountRowBase
		Private m_DB As Database
		Private m_Id As Integer = Nothing
		Private m_Code As String = Nothing
		Private m_SalesCode As String = Nothing
		Private m_CurrencyCode As String = Nothing
		Private m_StartingDate As String = Nothing
		Private m_LineDiscountPercent As String = Nothing
		Private m_SalesType As String = Nothing
		Private m_MinimumQuantity As String = Nothing
		Private m_EndingDate As String = Nothing
		Private m_Type As String = Nothing
		Private m_UnitOfMeasureCode As String = Nothing
		Private m_VariantCode As String = Nothing

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

		Public Property SalesCode() As String
			Get
				Return m_SalesCode
			End Get
			Set(ByVal Value As String)
				m_SalesCode = Value
			End Set
		End Property

		Public Property CurrencyCode() As String
			Get
				Return m_CurrencyCode
			End Get
			Set(ByVal Value As String)
				m_CurrencyCode = Value
			End Set
		End Property

		Public Property StartingDate() As String
			Get
				Return m_StartingDate
			End Get
			Set(ByVal Value As String)
				m_StartingDate = Value
			End Set
		End Property

		Public Property LineDiscountPercent() As String
			Get
				Return m_LineDiscountPercent
			End Get
			Set(ByVal Value As String)
				m_LineDiscountPercent = Value
			End Set
		End Property

		Public Property SalesType() As String
			Get
				Return m_SalesType
			End Get
			Set(ByVal Value As String)
				m_SalesType = Value
			End Set
		End Property

		Public Property MinimumQuantity() As String
			Get
				Return m_MinimumQuantity
			End Get
			Set(ByVal Value As String)
				m_MinimumQuantity = Value
			End Set
		End Property

		Public Property EndingDate() As String
			Get
				Return m_EndingDate
			End Get
			Set(ByVal Value As String)
				m_EndingDate = Value
			End Set
		End Property

		Public Property Type() As String
			Get
				Return m_Type
			End Get
			Set(ByVal Value As String)
				m_Type = Value
			End Set
		End Property

		Public Property UnitOfMeasureCode() As String
			Get
				Return m_UnitOfMeasureCode
			End Get
			Set(ByVal Value As String)
				m_UnitOfMeasureCode = Value
			End Set
		End Property

		Public Property VariantCode() As String
			Get
				Return m_VariantCode
			End Get
			Set(ByVal Value As String)
				m_VariantCode = Value
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
			If IsDBNull(r.Item("SalesCode")) Then
				m_SalesCode = Nothing
			Else
				m_SalesCode = Convert.ToString(r.Item("SalesCode"))
			End If
			If IsDBNull(r.Item("CurrencyCode")) Then
				m_CurrencyCode = Nothing
			Else
				m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
			End If
			If IsDBNull(r.Item("StartingDate")) Then
				m_StartingDate = Nothing
			Else
				m_StartingDate = Convert.ToString(r.Item("StartingDate"))
			End If
			If IsDBNull(r.Item("LineDiscountPercent")) Then
				m_LineDiscountPercent = Nothing
			Else
				m_LineDiscountPercent = Convert.ToString(r.Item("LineDiscountPercent"))
			End If
			If IsDBNull(r.Item("SalesType")) Then
				m_SalesType = Nothing
			Else
				m_SalesType = Convert.ToString(r.Item("SalesType"))
			End If
			If IsDBNull(r.Item("MinimumQuantity")) Then
				m_MinimumQuantity = Nothing
			Else
				m_MinimumQuantity = Convert.ToString(r.Item("MinimumQuantity"))
			End If
			If IsDBNull(r.Item("EndingDate")) Then
				m_EndingDate = Nothing
			Else
				m_EndingDate = Convert.ToString(r.Item("EndingDate"))
			End If
			If IsDBNull(r.Item("Type")) Then
				m_Type = Nothing
			Else
				m_Type = Convert.ToString(r.Item("Type"))
			End If
			If IsDBNull(r.Item("UnitOfMeasureCode")) Then
				m_UnitOfMeasureCode = Nothing
			Else
				m_UnitOfMeasureCode = Convert.ToString(r.Item("UnitOfMeasureCode"))
			End If
			If IsDBNull(r.Item("VariantCode")) Then
				m_VariantCode = Nothing
			Else
				m_VariantCode = Convert.ToString(r.Item("VariantCode"))
			End If
		End Sub	'Load
	End Class

	Public Class NavisionSalesLineDiscountCollection
		Inherits GenericCollection(Of NavisionSalesLineDiscountRow)
	End Class

End Namespace


