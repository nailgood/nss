Option Explicit On 

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text

Namespace DataLayer

    Public Class OccasionRow
        Inherits OccasionRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal OccasionId As Integer)
            MyBase.New(database, OccasionId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal OccasionId As Integer) As OccasionRow
            Dim row As OccasionRow

            row = New OccasionRow(_Database, OccasionId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal OccasionId As Integer)
            Dim row As OccasionRow

            row = New OccasionRow(_Database, OccasionId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Sub RemoveByItem(ByVal _Database As Database, ByVal ItemId As Integer)
            Dim SQL As String = "delete from StoreItemOccasion where ItemId = " & _Database.Quote(ItemId.ToString)
            _Database.ExecuteSQL(SQL)
        End Sub
    End Class

    Public MustInherit Class OccasionRowBase
        Private m_DB As Database
        Private m_OccasionId As Integer = Nothing
        Private m_OccasionName As String = Nothing
        Private m_SortOrder As Integer = Nothing


        Public Property OccasionId() As Integer
            Get
                Return m_OccasionId
            End Get
            Set(ByVal Value As Integer)
                m_OccasionId = value
            End Set
        End Property

        Public Property OccasionName() As String
            Get
                Return m_OccasionName
            End Get
            Set(ByVal Value As String)
                m_OccasionName = value
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
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal OccasionId As Integer)
            m_DB = database
            m_OccasionId = OccasionId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Occasion WHERE OccasionId = " & DB.Quote(OccasionId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
                ''mail error here
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As sqlDataReader)
            m_OccasionName = Convert.ToString(r.Item("OccasionName"))
            m_SortOrder = Convert.ToInt32(r.Item("SortOrder"))
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO Occasion (" _
                 & " OccasionName" _
                 & ",SortOrder" _
                 & ") VALUES (" _
                 & m_DB.Quote(OccasionName) _
                 & "," & m_DB.Quote(SortOrder) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL("UPDATE Occasion SET SortOrder = SortOrder+1")
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            m_DB.ExecuteSQL("UPDATE Occasion SET SortOrder = SortOrder+1")
            OccasionId = m_DB.InsertSQL(InsertStatement)
            Return OccasionId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Occasion SET " _
             & " OccasionName = " & m_DB.Quote(OccasionName) _
             & ",SortOrder = " & m_DB.Quote(SortOrder) _
             & " WHERE OccasionId = " & m_DB.Quote(OccasionId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            m_DB.ExecuteSQL("DELETE FROM StoreItemOccasion WHERE OccasionId=" & m_DB.Quote(OccasionId))
            SQL = "DELETE FROM Occasion WHERE OccasionId = " & m_DB.Quote(OccasionId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class OccasionCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Occasion As OccasionRow)
            Me.List.Add(Occasion)
        End Sub

        Public Function Contains(ByVal Occasion As OccasionRow) As Boolean
            Return Me.List.Contains(Occasion)
        End Function

        Public Function IndexOf(ByVal Occasion As OccasionRow) As Integer
            Return Me.List.IndexOf(Occasion)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Occasion As OccasionRow)
            Me.List.Insert(Index, Occasion)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As OccasionRow
            Get
                Return CType(Me.List.Item(Index), OccasionRow)
            End Get

            Set(ByVal Value As OccasionRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Occasion As OccasionRow)
            Me.List.Remove(Occasion)
        End Sub
    End Class

End Namespace


