Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionShippingRatesRow
		Inherits NavisionShippingRatesRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Shared Function GetCollection(ByVal DB As Database) As NavisionShippingRatesCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New NavisionShippingRatesCollection
            Try
                Dim row As NavisionShippingRatesRow
                Dim SQL As String = "select * from _NAVISION_SHIPPING_RATES"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionShippingRatesRow(DB)
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

	Public MustInherit Class NavisionShippingRatesRowBase
		Private m_DB As Database
		Private m_Shipping_No As Integer
		Private m_Method_Id As Integer
		Private m_Low_Value As Double
		Private m_High_Value As Double
		Private m_Over_Under_Value As Double
		Private m_First_Pound_Over As Double
		Private m_First_Pound_Under As Double
		Private m_Additional_Pound As Double
		Private m_Additional_Threshold As Double

		Public Property Shipping_No() As Integer
			Get
				Return m_Shipping_No
			End Get
			Set(ByVal value As Integer)
				m_Shipping_No = value
			End Set
		End Property

		Public Property Method_Id() As Integer
			Get
				Return m_Method_Id
			End Get
			Set(ByVal value As Integer)
				m_Method_Id = value
			End Set
		End Property

		Public Property Low_Value() As Double
			Get
				Return m_Low_Value
			End Get
			Set(ByVal value As Double)
				m_Low_Value = value
			End Set
		End Property

		Public Property High_Value() As Double
			Get
				Return High_Value
			End Get
			Set(ByVal value As Double)
				m_High_Value = value
			End Set
		End Property

		Public Property Over_Under_Value() As Double
			Get
				Return m_Over_Under_Value
			End Get
			Set(ByVal value As Double)
				m_Over_Under_Value = value
			End Set
		End Property

		Public Property First_Pound_Over() As Double
			Get
				Return m_First_Pound_Over
			End Get
			Set(ByVal value As Double)
				m_First_Pound_Over = value
			End Set
		End Property

		Public Property First_Pound_Under() As Double
			Get
				Return m_First_Pound_Under
			End Get
			Set(ByVal value As Double)
				m_First_Pound_Under = value
			End Set
		End Property

		Public Property Additional_Pound() As Double
			Get
				Return m_Additional_Pound
			End Get
			Set(ByVal value As Double)
				m_Additional_Pound = value
			End Set
		End Property

		Public Property Additional_Threshold() As Double
			Get
				Return m_Additional_Threshold
			End Get
			Set(ByVal value As Double)
				m_Additional_Threshold = value
			End Set
		End Property


		Public Property DB() As Database
			Get
				DB = m_DB
			End Get
			Set(ByVal Value As Database)
				m_DB = Value
			End Set
		End Property

		Public Sub New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			m_DB = DB
		End Sub	'New

		Protected Overridable Sub Load(ByVal r As SqlDataReader)
			If IsDBNull(r.Item("Shipping_No")) Then
				m_Shipping_No = Nothing
			Else
				m_Shipping_No = Convert.ToInt32(r.Item("Shipping_No"))
			End If
			If IsDBNull(r.Item("Method_Id")) Then
				m_Method_Id = Nothing
			Else
				m_Method_Id = Convert.ToInt32(r.Item("Method_Id"))
			End If
			If IsDBNull(r.Item("Low_Value")) Then
				m_Low_Value = Nothing
			Else
				m_Low_Value = Convert.ToDouble(r.Item("Low_Value"))
			End If
			If IsDBNull(r.Item("High_Value")) Then
				m_High_Value = Nothing
			Else
				m_High_Value = Convert.ToDouble(r.Item("High_Value"))
			End If
			If IsDBNull(r.Item("Over_Under_Value")) Then
				m_Over_Under_Value = Nothing
			Else
				m_Over_Under_Value = Convert.ToDouble(r.Item("Over_Under_Value"))
			End If
			If IsDBNull(r.Item("First_Pound_Over")) Then
				m_First_Pound_Over = Nothing
			Else
				m_First_Pound_Over = Convert.ToDouble(r.Item("First_Pound_Over"))
			End If
			If IsDBNull(r.Item("First_Pound_Under")) Then
				m_First_Pound_Under = Nothing
			Else
				m_First_Pound_Under = Convert.ToDouble(r.Item("First_Pound_Under"))
			End If
			If IsDBNull(r.Item("Additional_Pound")) Then
				m_Additional_Pound = Nothing
			Else
				m_Additional_Pound = Convert.ToDouble(r.Item("Additional_Pound"))
			End If
			If IsDBNull(r.Item("Additional_Threshold")) Then
				m_Additional_Threshold = Nothing
			Else
				m_Additional_Threshold = Convert.ToDouble(r.Item("Additional_Threshold"))
			End If
		End Sub	'Load
	End Class

	Public Class NavisionShippingRatesCollection
		Inherits GenericCollection(Of NavisionShippingRatesRow)
	End Class

End Namespace


