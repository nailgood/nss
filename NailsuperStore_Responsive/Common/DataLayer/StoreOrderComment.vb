Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class StoreOrderCommentRow
        Inherits StoreOrderCommentRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CommentId As Integer)
            MyBase.New(DB, CommentId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As String, ByVal LineNo As Integer)
            MyBase.New(DB, OrderId, LineNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CommentId As Integer) As StoreOrderCommentRow
            Dim row As StoreOrderCommentRow

            row = New StoreOrderCommentRow(DB, CommentId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderNo As String, ByVal LineNo As Integer) As StoreOrderCommentRow
            Dim row As StoreOrderCommentRow

            Dim OrderId As Integer = DB.ExecuteScalar("select top 1 OrderId from StoreOrder where OrderNo = " & DB.Quote(OrderNo))

            row = New StoreOrderCommentRow(DB, OrderId, LineNo)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CommentId As Integer)
            Dim row As StoreOrderCommentRow

            row = New StoreOrderCommentRow(DB, CommentId)
            row.Remove()
        End Sub

        Public Shared Sub RemoveByOrderId(ByVal DB As Database, ByVal OrderId As Integer)
            Dim SQL As String = ""

            SQL = "DELETE FROM StoreOrderComment WHERE OrderId = " & DB.Number(OrderId)
            DB.ExecuteSQL(SQL)
        End Sub

        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionOrderCommentsRow)
            'ignore error
            If OrderId = Nothing Then Exit Sub

            LineNo = r.Line_No
            CommentDate = r.Sales_Comment_Date
            Code = r.Code
            Comment = r.Comment

            If CommentId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub

        Public Shared Function GetCollectionForExport(ByVal DB As Database) As StoreOrderCommentCollection
            Dim c As New StoreOrderCommentCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As StoreOrderCommentRow
                Dim SQL As String = "select soc.* from StoreOrderComment soc INNER JOIN StoreOrder so on soc.OrderId = so.OrderId where soc.DoExport = 1 and so.OrderDate is not null and so.ProcessDate is not null"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New StoreOrderCommentRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return c
        End Function

    End Class

    Public MustInherit Class StoreOrderCommentRowBase
        Private m_DB As Database
        Private m_CommentId As Integer = Nothing
        Private m_OrderId As Integer = Nothing
        Private m_LineNo As Integer = Nothing
        Private m_CommentDate As DateTime = Nothing
        Private m_Code As String = Nothing
        Private m_Comment As String = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_LastExport As DateTime = Nothing

        Public Property CommentId() As Integer
            Get
                Return m_CommentId
            End Get
            Set(ByVal Value As Integer)
                m_CommentId = Value
            End Set
        End Property

        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property

        Public Property LineNo() As Integer
            Get
                Return m_LineNo
            End Get
            Set(ByVal Value As Integer)
                m_LineNo = Value
            End Set
        End Property

        Public Property CommentDate() As DateTime
            Get
                Return m_CommentDate
            End Get
            Set(ByVal Value As DateTime)
                m_CommentDate = Value
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

        Public Property Comment() As String
            Get
                Return m_Comment
            End Get
            Set(ByVal Value As String)
                m_Comment = Value
            End Set
        End Property

        Public Property DoExport() As Boolean
            Get
                Return m_DoExport
            End Get
            Set(ByVal Value As Boolean)
                m_DoExport = Value
            End Set
        End Property

        Public Property LastExport() As DateTime
            Get
                Return m_LastExport
            End Get
            Set(ByVal Value As DateTime)
                m_LastExport = Value
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
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CommentId As Integer)
            m_DB = DB
            m_CommentId = CommentId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As String, ByVal LineNo As Integer)
            m_DB = DB
            m_OrderId = OrderId
            m_LineNo = LineNo
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreOrderComment WHERE " & IIf(CommentId <> Nothing, "CommentId = " & DB.Number(CommentId), "OrderId = " & m_DB.Quote(OrderId) & " and [LineNo] = " & m_DB.Quote(LineNo))
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
                m_CommentId = Convert.ToInt32(r.Item("CommentId"))
                m_OrderId = Convert.ToString(r.Item("OrderId"))
                m_LineNo = Convert.ToInt32(r.Item("LineNo"))
                If IsDBNull(r.Item("CommentDate")) Then
                    m_CommentDate = Nothing
                Else
                    m_CommentDate = Convert.ToDateTime(r.Item("CommentDate"))
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
                m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
                If IsDBNull(r.Item("LastExport")) Then
                    m_LastExport = Nothing
                Else
                    m_LastExport = Convert.ToDateTime(r.Item("LastExport"))
                End If
            Catch ex As Exception
                Throw ex
                ''  Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO StoreOrderComment (" _
             & " OrderId" _
             & ",[LineNo]" _
             & ",CommentDate" _
             & ",Code" _
             & ",Comment" _
             & ",DoExport" _
             & ",LastExport" _
             & ") VALUES (" _
             & m_DB.Quote(OrderId) _
             & "," & m_DB.Number(LineNo) _
             & "," & m_DB.NullQuote(CommentDate) _
             & "," & m_DB.Quote(Code) _
             & "," & m_DB.Quote(Comment) _
             & "," & CInt(DoExport) _
             & "," & m_DB.Quote(LastExport) _
             & ")"

            CommentId = m_DB.InsertSQL(SQL)

            Return CommentId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreOrderComment SET " _
             & " OrderId = " & m_DB.Quote(OrderId) _
             & ",[LineNo] = " & m_DB.Number(LineNo) _
             & ",CommentDate = " & m_DB.NullQuote(CommentDate) _
             & ",Code = " & m_DB.Quote(Code) _
             & ",Comment = " & m_DB.Quote(Comment) _
             & ",DoExport = " & CInt(DoExport) _
             & ",LastExport = " & m_DB.Quote(LastExport) _
             & " WHERE " & IIf(CommentId <> Nothing, "CommentId = " & m_DB.Quote(CommentId), "OrderId = " & m_DB.Quote(OrderId) & " and [LineNo] = " & m_DB.Quote(LineNo))

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreOrderComment WHERE " & IIf(CommentId <> Nothing, "CommentId = " & m_DB.Quote(CommentId), "OrderId = " & m_DB.Quote(OrderId) & " and [LineNo] = " & m_DB.Quote(LineNo))
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreOrderCommentCollection
        Inherits GenericCollection(Of StoreOrderCommentRow)
    End Class

End Namespace