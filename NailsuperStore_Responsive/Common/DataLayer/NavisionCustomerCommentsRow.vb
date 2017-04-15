Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionCustomerCommentsRow
		Inherits NavisionCustomerCommentsRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionCustomerCommentsCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionCustomerCommentsRow
                Dim c As New NavisionCustomerCommentsCollection
                Dim SQL As String = "select * from _NAVISION_CUSTOMER_COMMENTS"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionCustomerCommentsRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionCustomerCommentsCollection
        End Function

	End Class

	Public MustInherit Class NavisionCustomerCommentsRowBase
		Private m_DB As Database
		Private m_Table_Name As String = Nothing
		Private m_Cust_No As String = Nothing
		Private m_Line_No As Integer = Nothing
		Private m_Date_Date As DateTime = Nothing
		Private m_Code As String = Nothing
		Private m_Comment As String = Nothing
		Private m_ID As Integer = Nothing

		Public Property Table_Name() As String
			Get
				Return Trim(m_Table_Name)
			End Get
			Set(ByVal Value As String)
				m_Table_Name = Trim(Value)
			End Set
		End Property

		Public Property Cust_No() As String
			Get
				Return Trim(m_Cust_No)
			End Get
			Set(ByVal Value As String)
				m_Cust_No = Trim(Value)
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

		Public Property Date_Date() As DateTime
			Get
				Return Trim(m_Date_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Date_Date = Trim(Value)
			End Set
		End Property

		Public Property Code() As String
			Get
				Return Trim(m_Code)
			End Get
			Set(ByVal Value As String)
				m_Code = Trim(Value)
			End Set
		End Property

		Public Property Comment() As String
			Get
				Return Trim(m_Comment)
			End Get
			Set(ByVal Value As String)
				m_Comment = Trim(Value)
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
			If IsDBNull(r.Item("Table_Name")) Then
				m_Table_Name = Nothing
			Else
				m_Table_Name = Convert.ToString(r.Item("Table_Name"))
			End If
			If IsDBNull(r.Item("Cust_No")) Then
				m_Cust_No = Nothing
			Else
				m_Cust_No = Convert.ToString(r.Item("Cust_No"))
			End If
			If IsDBNull(r.Item("Line_No")) Then
				m_Line_No = Nothing
			Else
				m_Line_No = Convert.ToInt32(r.Item("Line_No"))
			End If
			If IsDBNull(r.Item("Date_Date")) Then
				m_Date_Date = Nothing
			Else
				m_Date_Date = Convert.ToDateTime(r.Item("Date_Date"))
			End If
			If IsDBNull(r.Item("Code")) Then
				m_Code = Nothing
			Else
				m_Code = Convert.ToString(r.Item("Code"))
			End If
			If IsDBNull(r.Item("Comment")) Then
				m_Comment = Nothing
			Else
				m_Comment = Convert.ToString(r.Item("Comment"))
			End If
			m_ID = Convert.ToInt32(r.Item("ID"))
		End Sub	'Load
	End Class

	Public Class NavisionCustomerCommentsCollection
		Inherits GenericCollection(Of NavisionCustomerCommentsRow)
	End Class

End Namespace


