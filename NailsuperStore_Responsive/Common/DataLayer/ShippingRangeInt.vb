Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class ShippingRangeIntRow
		Inherits ShippingRangeIntRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal ShippingRangeIntId As Integer)
			MyBase.New(DB, ShippingRangeIntId)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal ShippingRangeIntId As Integer) As ShippingRangeIntRow
			Dim row As ShippingRangeIntRow

			row = New ShippingRangeIntRow(DB, ShippingRangeIntId)
			row.Load()

			Return row
		End Function

		Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ShippingRangeIntId As Integer)
			Dim row As ShippingRangeIntRow

			row = New ShippingRangeIntRow(DB, ShippingRangeIntId)
			row.Remove()
		End Sub

		'Custom Methods

	End Class

	Public MustInherit Class ShippingRangeIntRowBase
		Private m_DB As Database
		Private m_ShippingRangeIntId As Integer = Nothing
		Private m_MethodId As Integer = Nothing
		Private m_CountryCode As String = Nothing
		Private m_OverUnderValue As Double = Nothing
		Private m_FirstPoundOver As Double = Nothing
		Private m_FirstPoundUnder As Double = Nothing
		Private m_AdditionalPound As Double = Nothing
		Private m_AdditionalThreshold As Double = Nothing


		Public Property ShippingRangeIntId() As Integer
			Get
				Return m_ShippingRangeIntId
			End Get
			Set(ByVal Value As Integer)
				m_ShippingRangeIntId = value
			End Set
		End Property

		Public Property MethodId() As Integer
			Get
				Return m_MethodId
			End Get
			Set(ByVal Value As Integer)
				m_MethodId = value
			End Set
		End Property

		Public Property CountryCode() As String
			Get
				Return m_CountryCode
			End Get
			Set(ByVal Value As String)
				m_CountryCode = value
			End Set
		End Property

		Public Property OverUnderValue() As Double
			Get
				Return m_OverUnderValue
			End Get
			Set(ByVal Value As Double)
				m_OverUnderValue = value
			End Set
		End Property

		Public Property FirstPoundOver() As Double
			Get
				Return m_FirstPoundOver
			End Get
			Set(ByVal Value As Double)
				m_FirstPoundOver = value
			End Set
		End Property

		Public Property FirstPoundUnder() As Double
			Get
				Return m_FirstPoundUnder
			End Get
			Set(ByVal Value As Double)
				m_FirstPoundUnder = value
			End Set
		End Property

		Public Property AdditionalPound() As Double
			Get
				Return m_AdditionalPound
			End Get
			Set(ByVal Value As Double)
				m_AdditionalPound = value
			End Set
		End Property

		Public Property AdditionalThreshold() As Double
			Get
				Return m_AdditionalThreshold
			End Get
			Set(ByVal Value As Double)
				m_AdditionalThreshold = value
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

		Public Sub New(ByVal DB As Database, ByVal ShippingRangeIntId As Integer)
			m_DB = DB
			m_ShippingRangeIntId = ShippingRangeIntId
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM ShippingRangeInt WHERE ShippingRangeIntId = " & DB.Number(ShippingRangeIntId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
			
		End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_ShippingRangeIntId = Convert.ToInt32(r.Item("ShippingRangeIntId"))
                m_MethodId = Convert.ToInt32(r.Item("MethodId"))
                m_CountryCode = Convert.ToString(r.Item("CountryCode"))
                m_OverUnderValue = Convert.ToDouble(r.Item("OverUnderValue"))
                m_FirstPoundOver = Convert.ToDouble(r.Item("FirstPoundOver"))
                m_FirstPoundUnder = Convert.ToDouble(r.Item("FirstPoundUnder"))
                m_AdditionalPound = Convert.ToDouble(r.Item("AdditionalPound"))
                m_AdditionalThreshold = Convert.ToDouble(r.Item("AdditionalThreshold"))
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String


			SQL = " INSERT INTO ShippingRangeInt (" _
			 & " MethodId" _
			 & ",CountryCode" _
			 & ",OverUnderValue" _
			 & ",FirstPoundOver" _
			 & ",FirstPoundUnder" _
			 & ",AdditionalPound" _
			 & ",AdditionalThreshold" _
			 & ") VALUES (" _
			 & m_DB.NullNumber(MethodId) _
			 & "," & m_DB.Quote(CountryCode) _
			 & "," & m_DB.Number(OverUnderValue) _
			 & "," & m_DB.Number(FirstPoundOver) _
			 & "," & m_DB.Number(FirstPoundUnder) _
			 & "," & m_DB.Number(AdditionalPound) _
			 & "," & m_DB.Number(AdditionalThreshold) _
			 & ")"

			ShippingRangeIntId = m_DB.InsertSQL(SQL)

			Return ShippingRangeIntId
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE ShippingRangeInt SET " _
			 & " MethodId = " & m_DB.NullNumber(MethodId) _
			 & ",CountryCode = " & m_DB.Quote(CountryCode) _
			 & ",OverUnderValue = " & m_DB.Number(OverUnderValue) _
			 & ",FirstPoundOver = " & m_DB.Number(FirstPoundOver) _
			 & ",FirstPoundUnder = " & m_DB.Number(FirstPoundUnder) _
			 & ",AdditionalPound = " & m_DB.Number(AdditionalPound) _
			 & ",AdditionalThreshold = " & m_DB.Number(AdditionalThreshold) _
			 & " WHERE ShippingRangeIntId = " & m_DB.quote(ShippingRangeIntId)

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update

		Public Sub Remove()
			Dim SQL As String

			SQL = "DELETE FROM ShippingRangeInt WHERE ShippingRangeIntId = " & m_DB.Quote(ShippingRangeIntId)
			m_DB.ExecuteSQL(SQL)
		End Sub	'Remove
	End Class

	Public Class ShippingRangeIntCollection
		Inherits GenericCollection(Of ShippingRangeIntRow)
	End Class

End Namespace


