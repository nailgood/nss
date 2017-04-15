Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class SalesLineDiscountRow
		Inherits SalesLineDiscountRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal SalesLineDiscountId As Integer)
			MyBase.New(DB, SalesLineDiscountId)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal Code As String)
			MyBase.New(DB, Code)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal SalesLineDiscountId As Integer) As SalesLineDiscountRow
			Dim row As SalesLineDiscountRow

			row = New SalesLineDiscountRow(DB, SalesLineDiscountId)
			row.Load()

			Return row
		End Function

		Public Shared Function GetRow(ByVal DB As Database, ByVal Code As String) As SalesLineDiscountRow
			Dim row As SalesLineDiscountRow

			row = New SalesLineDiscountRow(DB, Code)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SalesLineDiscountId As Integer)
			Dim row As SalesLineDiscountRow

			row = New SalesLineDiscountRow(DB, SalesLineDiscountId)
			row.Remove()
		End Sub

		'Custom Methods
		Public Sub CopyFromNavision(ByVal r As NavisionSalesLineDiscountRow)
			Code = r.Code
			SalesCode = r.SalesCode
			CurrencyCode = r.CurrencyCode
			StartingDate = r.StartingDate
			LineDiscountPercent = r.LineDiscountPercent
			SalesType = r.SalesType
			MinimumQuantity = r.MinimumQuantity
			EndingDate = r.EndingDate
			Type = r.Type
			UnitOfMeasureCode = r.UnitOfMeasureCode
			VariantCode = r.VariantCode

			If SalesLineDiscountId = Nothing Then
				Insert()
			Else
				Update()
			End If
		End Sub
	End Class

	Public MustInherit Class SalesLineDiscountRowBase
		Private m_DB As Database
		Private m_SalesLineDiscountId As Integer = Nothing
		Private m_Code As String = Nothing
		Private m_SalesCode As String = Nothing
		Private m_CurrencyCode As String = Nothing
		Private m_StartingDate As DateTime = Nothing
		Private m_LineDiscountPercent As Double = Nothing
		Private m_SalesType As String = Nothing
		Private m_MinimumQuantity As Integer = Nothing
		Private m_EndingDate As DateTime = Nothing
		Private m_Type As String = Nothing
		Private m_UnitOfMeasureCode As String = Nothing
		Private m_VariantCode As String = Nothing


		Public Property SalesLineDiscountId() As Integer
			Get
				Return m_SalesLineDiscountId
			End Get
			Set(ByVal Value As Integer)
				m_SalesLineDiscountId = value
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

		Public Property SalesCode() As String
			Get
				Return m_SalesCode
			End Get
			Set(ByVal Value As String)
				m_SalesCode = value
			End Set
		End Property

		Public Property CurrencyCode() As String
			Get
				Return m_CurrencyCode
			End Get
			Set(ByVal Value As String)
				m_CurrencyCode = value
			End Set
		End Property

		Public Property StartingDate() As DateTime
			Get
				Return m_StartingDate
			End Get
			Set(ByVal Value As DateTime)
				m_StartingDate = value
			End Set
		End Property

		Public Property LineDiscountPercent() As Double
			Get
				Return m_LineDiscountPercent
			End Get
			Set(ByVal Value As Double)
				m_LineDiscountPercent = value
			End Set
		End Property

		Public Property SalesType() As String
			Get
				Return m_SalesType
			End Get
			Set(ByVal Value As String)
				m_SalesType = value
			End Set
		End Property

		Public Property MinimumQuantity() As Integer
			Get
				Return m_MinimumQuantity
			End Get
			Set(ByVal Value As Integer)
				m_MinimumQuantity = value
			End Set
		End Property

		Public Property EndingDate() As DateTime
			Get
				Return m_EndingDate
			End Get
			Set(ByVal Value As DateTime)
				m_EndingDate = value
			End Set
		End Property

		Public Property Type() As String
			Get
				Return m_Type
			End Get
			Set(ByVal Value As String)
				m_Type = value
			End Set
		End Property

		Public Property UnitOfMeasureCode() As String
			Get
				Return m_UnitOfMeasureCode
			End Get
			Set(ByVal Value As String)
				m_UnitOfMeasureCode = value
			End Set
		End Property

		Public Property VariantCode() As String
			Get
				Return m_VariantCode
			End Get
			Set(ByVal Value As String)
				m_VariantCode = value
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

		Public Sub New(ByVal DB As Database, ByVal SalesLineDiscountId As Integer)
			m_DB = DB
			m_SalesLineDiscountId = SalesLineDiscountId
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal Code As String)
			m_DB = DB
			m_Code = Code
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM SalesLineDiscount WHERE " & IIf(SalesLineDiscountId <> Nothing, "SalesLineDiscountId = " & DB.Number(SalesLineDiscountId), "Code = " & m_DB.Quote(Code))
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
			m_SalesLineDiscountId = Convert.ToInt32(r.Item("SalesLineDiscountId"))
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
				m_StartingDate = Convert.ToDateTime(r.Item("StartingDate"))
			End If
			If IsDBNull(r.Item("LineDiscountPercent")) Then
				m_LineDiscountPercent = Nothing
			Else
				m_LineDiscountPercent = Convert.ToDouble(r.Item("LineDiscountPercent"))
			End If
			If IsDBNull(r.Item("SalesType")) Then
				m_SalesType = Nothing
			Else
				m_SalesType = Convert.ToString(r.Item("SalesType"))
			End If
			If IsDBNull(r.Item("MinimumQuantity")) Then
				m_MinimumQuantity = Nothing
			Else
				m_MinimumQuantity = Convert.ToInt32(r.Item("MinimumQuantity"))
			End If
			If IsDBNull(r.Item("EndingDate")) Then
				m_EndingDate = Nothing
			Else
				m_EndingDate = Convert.ToDateTime(r.Item("EndingDate"))
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

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO SalesLineDiscount (" _
			 & " Code" _
			 & ",SalesCode" _
			 & ",CurrencyCode" _
			 & ",StartingDate" _
			 & ",LineDiscountPercent" _
			 & ",SalesType" _
			 & ",MinimumQuantity" _
			 & ",EndingDate" _
			 & ",Type" _
			 & ",UnitOfMeasureCode" _
			 & ",VariantCode" _
			 & ") VALUES (" _
			 & m_DB.Quote(Code) _
			 & "," & m_DB.Quote(SalesCode) _
			 & "," & m_DB.Quote(CurrencyCode) _
			 & "," & m_DB.NullQuote(StartingDate) _
			 & "," & m_DB.Number(LineDiscountPercent) _
			 & "," & m_DB.Quote(SalesType) _
			 & "," & m_DB.Number(MinimumQuantity) _
			 & "," & m_DB.NullQuote(EndingDate) _
			 & "," & m_DB.Quote(Type) _
			 & "," & m_DB.Quote(UnitOfMeasureCode) _
			 & "," & m_DB.Quote(VariantCode) _
			 & ")"

			SalesLineDiscountId = m_DB.InsertSQL(SQL)

			Return SalesLineDiscountId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE SalesLineDiscount SET " _
			 & " Code = " & m_DB.Quote(Code) _
			 & ",SalesCode = " & m_DB.Quote(SalesCode) _
			 & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
			 & ",StartingDate = " & m_DB.NullQuote(StartingDate) _
			 & ",LineDiscountPercent = " & m_DB.Number(LineDiscountPercent) _
			 & ",SalesType = " & m_DB.Quote(SalesType) _
			 & ",MinimumQuantity = " & m_DB.Number(MinimumQuantity) _
			 & ",EndingDate = " & m_DB.NullQuote(EndingDate) _
			 & ",Type = " & m_DB.Quote(Type) _
			 & ",UnitOfMeasureCode = " & m_DB.Quote(UnitOfMeasureCode) _
			 & ",VariantCode = " & m_DB.Quote(VariantCode) _
			 & " WHERE SalesLineDiscountId = " & m_DB.quote(SalesLineDiscountId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM SalesLineDiscount WHERE SalesLineDiscountId = " & m_DB.Quote(SalesLineDiscountId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class SalesLineDiscountCollection
		Inherits GenericCollection(Of SalesLineDiscountRow)
	End Class

End Namespace


