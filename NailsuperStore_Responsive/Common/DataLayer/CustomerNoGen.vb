Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class CustomerNoGenRow
        Inherits CustomerNoGenRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As CustomerNoGenRow
            Dim row As CustomerNoGenRow

            row = New CustomerNoGenRow(DB, Id)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
            Dim row As CustomerNoGenRow

            row = New CustomerNoGenRow(DB, Id)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class CustomerNoGenRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_Void As Boolean = Nothing


        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property Void() As Boolean
            Get
                Return m_Void
            End Get
            Set(ByVal Value As Boolean)
                m_Void = Value
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

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            m_DB = DB
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM CustomerNoGen WHERE Id = " & DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_Id = Convert.ToInt32(r.Item("Id"))
            m_Void = Convert.ToBoolean(r.Item("Void"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO CustomerNoGen (" _
             & " Void" _
             & ") VALUES (" _
             & CInt(Void) _
             & ")"

            Id = m_DB.InsertSQL(SQL)

            Return Id
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE CustomerNoGen SET " _
             & " Void = " & CInt(Void) _
             & " WHERE Id = " & m_DB.Quote(Id)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM CustomerNoGen WHERE Id = " & m_DB.Quote(Id)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove

        Public Sub RemoveAll()
            Dim SQL As String

            SQL = "DELETE FROM CustomerNoGen "
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove All
    End Class

    Public Class CustomerNoGenCollection
        Inherits GenericCollection(Of CustomerNoGenRow)
    End Class

End Namespace


