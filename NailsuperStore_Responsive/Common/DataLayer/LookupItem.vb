Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text

Namespace DataLayer

    Public Class LookupItemRow
        Inherits LookupItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ItemId As Integer)
            MyBase.New(database, ItemId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ItemId As Integer) As LookupItemRow
            Dim row As LookupItemRow

            row = New LookupItemRow(_Database, ItemId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal ItemId As Integer)
            Dim row As LookupItemRow

            row = New LookupItemRow(_Database, ItemId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class LookupItemRowBase
        Private m_DB As Database
        Private m_ItemId As Integer = Nothing
        Private m_ListId As Integer = Nothing
        Private m_Value As String = Nothing
        Private m_SortOrder As Integer = Nothing


        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = value
            End Set
        End Property

        Public Property ListId() As Integer
            Get
                Return m_ListId
            End Get
            Set(ByVal Value As Integer)
                m_ListId = value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Set(ByVal Value As String)
                m_Value = value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = value
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
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ItemId As Integer)
            m_DB = database
            m_ItemId = ItemId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM LookupItem WHERE ItemId = " & DB.Quote(ItemId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As sqlDataReader)
            m_ItemId = Convert.ToInt32(r.Item("ItemId"))
            m_ListId = Convert.ToInt32(r.Item("ListId"))
            m_Value = Convert.ToString(r.Item("Value"))
            m_SortOrder = Convert.ToInt32(r.Item("SortOrder"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            SQL = " INSERT INTO LookupItem (" _
             & " ListId" _
             & ",Value" _
             & ",SortOrder" _
             & ") VALUES (" _
             & m_DB.Quote(ListId) _
             & "," & m_DB.Quote(Value) _
             & "," & m_DB.Quote(SortOrder) _
             & ")"

            ItemId = m_DB.InsertSQL(SQL)

            Return ItemId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE LookupItem SET " _
             & " ListId = " & m_DB.quote(ListId) _
             & ",Value = " & m_DB.Quote(Value) _
             & ",SortOrder = " & m_DB.quote(SortOrder) _
             & " WHERE ItemId = " & m_DB.quote(ItemId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM LookupItem WHERE ItemId = " & m_DB.Quote(ItemId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class LookupItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal LookupItem As LookupItemRow)
            Me.List.Add(LookupItem)
        End Sub

        Public Function Contains(ByVal LookupItem As LookupItemRow) As Boolean
            Return Me.List.Contains(LookupItem)
        End Function

        Public Function IndexOf(ByVal LookupItem As LookupItemRow) As Integer
            Return Me.List.IndexOf(LookupItem)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal LookupItem As LookupItemRow)
            Me.List.Insert(Index, LookupItem)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As LookupItemRow
            Get
                Return CType(Me.List.Item(Index), LookupItemRow)
            End Get

            Set(ByVal Value As LookupItemRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal LookupItem As LookupItemRow)
            Me.List.Remove(LookupItem)
        End Sub
    End Class

End Namespace