Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionSalesPriceRow
		Inherits NavisionSalesPriceRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
		Public Shared Function GetCollection(ByVal DB As Database) As NavisionSalesPriceCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New NavisionSalesPriceCollection
            Try
                Dim row As NavisionSalesPriceRow
                Dim SQL As String = "select * from _NAVISION_SALES_PRICE"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionSalesPriceRow(DB)
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

	Public MustInherit Class NavisionSalesPriceRowBase
		Private m_DB As Database
		Private m_Id As Integer = Nothing
		Private m_ItemNo As String = Nothing
		Private m_SalesCode As String = Nothing
		Private m_CurrencyCode As String = Nothing
		Private m_StartingDate As String = Nothing
		Private m_UnitPrice As String = Nothing
		Private m_PriceIncludesVAT As String = Nothing
		Private m_AllowInvoiceDisc As String = Nothing
		Private m_SalesType As String = Nothing
		Private m_MinimumQuantity As String = Nothing
		Private m_EndingDate As String = Nothing
		Private m_UnitOfMeasureCode As String = Nothing
		Private m_VariantCode As String = Nothing
		Private m_AllowLineDisc As String = Nothing
		Private m_UnitPriceIncludingVAT As String = Nothing
		Private m_PriceGroupDescription As String = Nothing

		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal Value As Integer)
				m_Id = Value
			End Set
		End Property

		Public Property ItemNo() As String
			Get
				Return m_ItemNo
			End Get
			Set(ByVal Value As String)
				m_ItemNo = Value
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

		Public Property UnitPrice() As String
			Get
				Return m_UnitPrice
			End Get
			Set(ByVal Value As String)
				m_UnitPrice = Value
			End Set
		End Property

		Public Property PriceIncludesVAT() As String
			Get
				Return m_PriceIncludesVAT
			End Get
			Set(ByVal Value As String)
				m_PriceIncludesVAT = Value
			End Set
		End Property

		Public Property AllowInvoiceDisc() As String
			Get
				Return m_AllowInvoiceDisc
			End Get
			Set(ByVal Value As String)
				m_AllowInvoiceDisc = Value
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

		Public Property AllowLineDisc() As String
			Get
				Return m_AllowLineDisc
			End Get
			Set(ByVal Value As String)
				m_AllowLineDisc = Value
			End Set
		End Property

		Public Property UnitPriceIncludingVAT() As String
			Get
				Return m_UnitPriceIncludingVAT
			End Get
			Set(ByVal Value As String)
				m_UnitPriceIncludingVAT = Value
			End Set
		End Property

		Public Property PriceGroupDescription() As String
			Get
				Return m_PriceGroupDescription
			End Get
			Set(ByVal Value As String)
				m_PriceGroupDescription = Value
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
			If IsDBNull(r.Item("ItemNo")) Then
				m_ItemNo = Nothing
			Else
				m_ItemNo = Convert.ToString(r.Item("ItemNo"))
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
                If (r.Item("StartingDate").ToString().Trim() = "") Then
                    m_StartingDate = Nothing
                Else
                    m_StartingDate = Convert.ToString(r.Item("StartingDate"))
                End If
            End If

            If IsDBNull(r.Item("UnitPrice")) Then
                m_UnitPrice = Nothing
            Else
                m_UnitPrice = Convert.ToString(r.Item("UnitPrice"))
            End If
            If IsDBNull(r.Item("PriceIncludesVAT")) Then
                m_PriceIncludesVAT = Nothing
            Else
                m_PriceIncludesVAT = Convert.ToString(r.Item("PriceIncludesVAT"))
            End If
            If IsDBNull(r.Item("AllowInvoiceDisc")) Then
                m_AllowInvoiceDisc = Nothing
            Else
                m_AllowInvoiceDisc = Convert.ToString(r.Item("AllowInvoiceDisc"))
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
                If (r.Item("EndingDate").ToString().Trim() = "") Then
                    m_EndingDate = Nothing
                Else
                    m_EndingDate = Convert.ToString(r.Item("EndingDate"))
                End If
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
            If IsDBNull(r.Item("AllowLineDisc")) Then
                m_AllowLineDisc = Nothing
            Else
                m_AllowLineDisc = Convert.ToString(r.Item("AllowLineDisc"))
            End If
            If IsDBNull(r.Item("UnitPriceIncludingVAT")) Then
                m_UnitPriceIncludingVAT = Nothing
            Else
                m_UnitPriceIncludingVAT = Convert.ToString(r.Item("UnitPriceIncludingVAT"))
            End If
            If IsDBNull(r.Item("PriceGroupDescription")) Then
                m_PriceGroupDescription = Nothing
            Else
                m_PriceGroupDescription = Convert.ToString(r.Item("PriceGroupDescription"))
            End If
        End Sub 'Load
	End Class

	Public Class NavisionSalesPriceCollection
		Inherits GenericCollection(Of NavisionSalesPriceRow)
	End Class

End Namespace


