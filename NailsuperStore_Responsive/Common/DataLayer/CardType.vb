Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Namespace DataLayer

    Public Class CardTypeRow
        Inherits CardTypeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal CardTypeId As Integer)
            MyBase.New(database, CardTypeId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal CardTypeId As Integer) As CardTypeRow
            Dim row As CardTypeRow

            row = New CardTypeRow(_Database, CardTypeId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal CardTypeId As Integer)
            Dim row As CardTypeRow

            row = New CardTypeRow(_Database, CardTypeId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class CardTypeRowBase
        Private m_DB As Database
        Private m_CardTypeId As Integer = Nothing
        Private m_Code As String = Nothing
        Private m_Name As String = Nothing

        Public Property CardTypeId() As Integer
            Get
                Return m_CardTypeId
            End Get
            Set(ByVal Value As Integer)
                m_CardTypeId = value
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

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = value
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

        Public Sub New(ByVal database As Database, ByVal CardTypeId As Integer)
            m_DB = database
            m_CardTypeId = CardTypeId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Dim SQL As String
            Try
                SQL = "SELECT * FROM CreditCardType WHERE CardTypeId = " & DB.Quote(CardTypeId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "CardType.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                If r.Item("Code") Is Convert.DBNull Then
                    m_Code = Nothing
                Else
                    m_Code = Convert.ToString(r.Item("Code"))
                End If
                If r.Item("Name") Is Convert.DBNull Then
                    m_Name = Nothing
                Else
                    m_Name = Convert.ToString(r.Item("Name"))
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO CreditCardType (" _
                 & " Code" _
                 & ",Name" _
                 & ") VALUES (" _
                 & m_DB.Quote(Code) _
                 & "," & m_DB.Quote(Name) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            CardTypeId = m_DB.InsertSQL(InsertStatement)
            Return CardTypeId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE CreditCardType SET " _
             & " Code = " & m_DB.Quote(Code) _
             & ",Name = " & m_DB.Quote(Name) _
             & " WHERE CardTypeId = " & m_DB.Quote(CardTypeId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM CreditCardType WHERE CardTypeId = " & m_DB.Quote(CardTypeId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class CardTypeCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal CardType As CardTypeRow)
            Me.List.Add(CardType)
        End Sub

        Public Function Contains(ByVal CardType As CardTypeRow) As Boolean
            Return Me.List.Contains(CardType)
        End Function

        Public Function IndexOf(ByVal CardType As CardTypeRow) As Integer
            Return Me.List.IndexOf(CardType)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal CardType As CardTypeRow)
            Me.List.Insert(Index, CardType)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As CardTypeRow
            Get
                Return CType(Me.List.Item(Index), CardTypeRow)
            End Get

            Set(ByVal Value As CardTypeRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal CardType As CardTypeRow)
            Me.List.Remove(CardType)
        End Sub
    End Class

End Namespace


