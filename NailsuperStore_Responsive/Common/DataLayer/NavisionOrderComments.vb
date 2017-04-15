Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class NavisionOrderCommentsRow
		Inherits NavisionOrderCommentsRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionOrderCommentsCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionOrderCommentsRow
                Dim c As New NavisionOrderCommentsCollection
                Dim SQL As String = "select * from _NAVISION_ORDER_COMMENTS"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionOrderCommentsRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionOrderCommentsCollection
        End Function

	End Class

	Public MustInherit Class NavisionOrderCommentsRowBase
		Private m_DB As Database
		Private m_Document_Type As String = Nothing
		Private m_Sales_Comment_No As String = Nothing
		Private m_Line_No As Integer = Nothing
		Private m_Sales_Comment_Date As DateTime = Nothing
		Private m_Code As String = Nothing
		Private m_Comment As String = Nothing
		Private m_ID As Integer = Nothing

		Public Property Document_Type() As String
			Get
				Return Trim(m_Document_Type)
			End Get
			Set(ByVal Value As String)
				m_Document_Type = Trim(Value)
			End Set
		End Property

		Public Property Sales_Comment_No() As String
			Get
				Return Trim(m_Sales_Comment_No)
			End Get
			Set(ByVal Value As String)
				m_Sales_Comment_No = Trim(Value)
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

		Public Property Sales_Comment_Date() As DateTime
			Get
				Return Trim(m_Sales_Comment_Date)
			End Get
			Set(ByVal Value As DateTime)
				m_Sales_Comment_Date = Trim(Value)
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
				m_ID = Value
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
			If IsDBNull(r.Item("Document_Type")) Then
				m_Document_Type = Nothing
			Else
				m_Document_Type = Convert.ToString(r.Item("Document_Type"))
			End If
			If IsDBNull(r.Item("Sales_Comment_No")) Then
				m_Sales_Comment_No = Nothing
			Else
				m_Sales_Comment_No = Convert.ToString(r.Item("Sales_Comment_No"))
			End If
			If IsDBNull(r.Item("Line_No")) Then
				m_Line_No = Nothing
			Else
				m_Line_No = Convert.ToInt32(r.Item("Line_No"))
			End If
			If IsDBNull(r.Item("Sales_Comment_Date")) Then
				m_Sales_Comment_Date = Nothing
			Else
				m_Sales_Comment_Date = Convert.ToDateTime(r.Item("Sales_Comment_Date"))
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

	Public Class NavisionOrderCommentsCollection
		Inherits GenericCollection(Of NavisionOrderCommentsRow)
	End Class

End Namespace


