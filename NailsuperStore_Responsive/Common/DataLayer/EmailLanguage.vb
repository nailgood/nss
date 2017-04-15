Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class EmailLanguageRow
        Inherits EmailLanguageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal DetailID As Integer)
            MyBase.New(DB, DetailID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal DetailID As Integer) As EmailLanguageRow
            Dim row As EmailLanguageRow

            row = New EmailLanguageRow(DB, DetailID)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal DetailID As Integer)
            Dim row As EmailLanguageRow

            row = New EmailLanguageRow(DB, DetailID)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllEmailLanguages(ByVal DB As Database) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from EmailLanguage order by DetailID")
            Return dt
        End Function
     
    End Class

    Public MustInherit Class EmailLanguageRowBase
        Private m_DB As Database
        Private m_DetailID As Integer = Nothing
        Private m_EmailID As Integer = Nothing
        Private m_LanguageID As Integer = Nothing
        Public Property DetailID() As Integer
            Get
                Return m_DetailID
            End Get
            Set(ByVal Value As Integer)
                m_DetailID = Value
            End Set
        End Property

        Public Property LanguageID() As Integer
            Get
                Return m_LanguageID
            End Get
            Set(ByVal Value As Integer)
                m_LanguageID = Value
            End Set
        End Property

        Public Property EmailID() As Integer
            Get
                Return m_EmailID
            End Get
            Set(ByVal Value As Integer)
                m_EmailID = Value
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

        Public Sub New(ByVal DB As Database, ByVal DetailID As Integer)
            m_DB = DB
            m_DetailID = DetailID
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM EmailLanguage WHERE DetailID = " & DB.Number(DetailID)
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
            m_DetailID = Convert.ToInt32(r.Item("DetailID"))
            m_LanguageID = Convert.ToString(r.Item("LanguageID"))
            m_EmailID = Convert.ToString(r.Item("EmailID"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            'Dim MaxSortOrder As Integer = DB.ExecuteScalar("select top 1 SortOrder from EmailLanguage order by SortOrder desc")
            'MaxSortOrder += 1

            SQL = " INSERT INTO EmailLanguage (" _
             & " LanguageID" _
             & ",EmailID" _
             & ") VALUES (" _
             & m_DB.Quote(LanguageID) _
             & "," & m_DB.Quote(EmailID) _
             & ")"

            DetailID = m_DB.InsertSQL(SQL)

            Return DetailID
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE EmailLanguage SET " _
             & " LanguageID = " & m_DB.Quote(LanguageID) _
             & ", EmailID = " & m_DB.Quote(EmailID) _
             & " WHERE DetailID = " & m_DB.Quote(DetailID)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update      


        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM EmailLanguage WHERE DetailID = " & m_DB.Quote(DetailID)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class EmailLanguageCollection
        Inherits GenericCollection(Of EmailLanguageRow)
    End Class

End Namespace


