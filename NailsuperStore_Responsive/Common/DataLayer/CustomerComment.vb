Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

	Public Class CustomerCommentRow
		Inherits CustomerCommentRowBase

		Public Sub New()
			MyBase.New()
		End Sub	'New

		Public Sub New(ByVal DB As Database)
			MyBase.New(DB)
		End Sub	'New

		Public Sub New(ByVal DB As Database, ByVal CustomerId As Integer, ByVal LineNo As Integer)
			MyBase.New(DB, CustomerId, LineNo)
		End Sub	'New

		'Shared function to get one row
		Public Shared Function GetRow(ByVal DB As Database, ByVal CustomerNo As String, ByVal LineNo As Integer) As CustomerCommentRow
			Dim row As CustomerCommentRow

			Dim CustomerId As Integer = DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(CustomerNo))
			If CustomerId = Nothing Then Throw New Exception("Customer not found for CustomerComment." & vbCrLf & "CustomerNo: " & CustomerNo)

			row = New CustomerCommentRow(DB, CustomerId, LineNo)
			row.Load()

			Return row
		End Function

		'Custom Methods
		Public Sub CopyFromNavision(ByVal r As NavisionCustomerCommentsRow)

			LineNo = r.Line_No
			Code = r.Code
			Comment = r.Comment
			CommentDate = r.Date_Date

			If Id = Nothing Then
				Insert()
			Else
				Update()
			End If
		End Sub

	End Class

	Public MustInherit Class CustomerCommentRowBase
		Private m_DB As Database
		Private m_Id As Integer = Nothing
		Private m_CustomerId As Integer = Nothing
		Private m_LineNo As Integer = Nothing
		Private m_Code As String = Nothing
		Private m_Comment As String = Nothing
		Private m_CommentDate As DateTime = Nothing

		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal Value As Integer)
				m_Id = Value
			End Set
		End Property

		Public Property CustomerId() As Integer
			Get
				Return m_CustomerId
			End Get
			Set(ByVal Value As Integer)
				m_CustomerId = Value
			End Set
		End Property

		Public Property LineNo() As Integer
			Get
				Return m_LineNo
			End Get
			Set(ByVal Value As Integer)
				m_LineNo = value
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

		Public Property Comment() As String
			Get
				Return m_Comment
			End Get
			Set(ByVal Value As String)
				m_Comment = value
			End Set
		End Property

		Public Property CommentDate() As DateTime
			Get
				Return m_CommentDate
			End Get
			Set(ByVal Value As DateTime)
				m_CommentDate = value
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

		Public Sub New(ByVal DB As Database, ByVal CustomerId As Integer, ByVal LineNo As Integer)
			m_DB = DB
			m_CustomerId = CustomerId
			m_LineNo = LineNo
		End Sub	'New

		Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM CustomerComment WHERE " & IIf(Id <> Nothing, "Id = " & m_DB.Quote(Id), "CustomerId = " & m_DB.Quote(CustomerId) & " and [LineNo] = " & m_DB.Quote(LineNo))
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
			m_Id = Convert.ToInt32(r.Item("Id"))
			m_CustomerId = Convert.ToInt32(r.Item("CustomerId"))
			m_LineNo = Convert.ToInt32(r.Item("LineNo"))
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
			If IsDBNull(r.Item("CommentDate")) Then
				m_CommentDate = Nothing
			Else
				m_CommentDate = Convert.ToDateTime(r.Item("CommentDate"))
			End If
		End Sub	'Load

		Public Overridable Function Insert() As Integer
			Dim SQL As String

			SQL = " INSERT INTO CustomerComment (" _
			 & " CustomerId" _
			 & ",[LineNo]" _
			 & ",Code" _
			 & ",Comment" _
			 & ",CommentDate" _
			 & ") VALUES (" _
			 & m_DB.Quote(CustomerId) _
			 & "," & m_DB.Number(LineNo) _
			 & "," & m_DB.Quote(Code) _
			 & "," & m_DB.Quote(Comment) _
			 & "," & m_DB.NullQuote(CommentDate) _
			 & ")"

			Id = m_DB.InsertSQL(SQL)

			Return Id
		End Function

		Public Overridable Sub Update()
			Dim SQL As String

			SQL = " UPDATE CustomerComment SET " _
			 & " CustomerId = " & m_DB.Quote(CustomerId) _
			 & ",[LineNo] = " & m_DB.Number(LineNo) _
			 & ",Code = " & m_DB.Quote(Code) _
			 & ",Comment = " & m_DB.Quote(Comment) _
			 & ",CommentDate = " & m_DB.NullQuote(CommentDate) _
			 & " WHERE " & IIf(Id <> Nothing, "Id = " & m_DB.Quote(Id), "CustomerId = " & m_DB.Quote(CustomerId) & " and [LineNo] = " & m_DB.Quote(LineNo))

			m_DB.ExecuteSQL(SQL)

		End Sub	'Update
	End Class

	Public Class CustomerCommentCollection
		Inherits GenericCollection(Of CustomerCommentRow)
	End Class

End Namespace


