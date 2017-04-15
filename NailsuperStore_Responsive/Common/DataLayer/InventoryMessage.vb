Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Namespace DataLayer
    Public Class InventoryMessageRow
        Inherits InventoryMessageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal InvMsgId As Integer)
            MyBase.New(database, InvMsgId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal InvMsgId As Integer) As InventoryMessageRow
            Dim row As InventoryMessageRow

            row = New InventoryMessageRow(_Database, InvMsgId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal InvMsgId As Integer)
            Dim row As InventoryMessageRow

            row = New InventoryMessageRow(_Database, InvMsgId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class InventoryMessageRowBase
        Private m_DB As Database
        Private m_InvMsgId As Integer = Nothing
        Private m_Message As String = Nothing


        Public Property InvMsgId() As Integer
            Get
                Return m_InvMsgId
            End Get
            Set(ByVal Value As Integer)
                m_InvMsgId = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_Message
            End Get
            Set(ByVal Value As String)
                m_Message = value
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

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal InvMsgId As Integer)
            m_DB = database
            m_InvMsgId = InvMsgId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM InventoryMessage WHERE InvMsgId = " & DB.Quote(InvMsgId)
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
            m_Message = Convert.ToString(r.Item("Message"))
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO InventoryMessage (" _
                 & " Message" _
                 & ") VALUES (" _
                 & m_DB.Quote(Message) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            InvMsgId = m_DB.InsertSQL(InsertStatement)
            Return InvMsgId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE InventoryMessage SET " _
             & " Message = " & m_DB.Quote(Message) _
             & " WHERE InvMsgId = " & m_DB.quote(InvMsgId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM InventoryMessage WHERE InvMsgId = " & m_DB.Quote(InvMsgId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class InventoryMessageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal InventoryMessage As InventoryMessageRow)
            Me.List.Add(InventoryMessage)
        End Sub

        Public Function Contains(ByVal InventoryMessage As InventoryMessageRow) As Boolean
            Return Me.List.Contains(InventoryMessage)
        End Function

        Public Function IndexOf(ByVal InventoryMessage As InventoryMessageRow) As Integer
            Return Me.List.IndexOf(InventoryMessage)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal InventoryMessage As InventoryMessageRow)
            Me.List.Insert(Index, InventoryMessage)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As InventoryMessageRow
            Get
                Return CType(Me.List.Item(Index), InventoryMessageRow)
            End Get

            Set(ByVal Value As InventoryMessageRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal InventoryMessage As InventoryMessageRow)
            Me.List.Remove(InventoryMessage)
        End Sub
    End Class
End Namespace