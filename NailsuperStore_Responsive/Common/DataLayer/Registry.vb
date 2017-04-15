Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Database
Imports Components
Namespace DataLayer

    Public Class RegistryRow
        Inherits RegistryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal RegistryId As Integer)
            MyBase.New(database, RegistryId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal RegistryId As Integer) As RegistryRow
            Dim row As RegistryRow

            row = New RegistryRow(_Database, RegistryId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal RegistryId As Integer)
            Dim row As RegistryRow

            Dim SQL As String = "delete from RegistryItem where RegistryId = " & RegistryId
            _Database.ExecuteSQL(SQL)

            row = New RegistryRow(_Database, RegistryId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetListByMemberId(ByVal _DB As Database, ByVal MemberId As Integer) As DataSet
            Dim SQL As String = "select RegistryId, Name from Registry where MemberId = " & MemberId & " order by CreateDate desc"

            Dim ds = _DB.GetDataSet(SQL)
            Return ds
        End Function

        Public Shared Function CanMofify(ByVal _DB As Database, ByVal MemberId As Integer, ByVal Registryid As Integer) As Boolean
            Dim Result As Boolean = False
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = "select top 1 RegistryId from Registry where MemberId = " & MemberId & " and RegistryId = " & Registryid
                r = _DB.GetReader(SQL)
                If r.Read Then
                    Result = True
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

            Return Result
        End Function

    End Class

    Public MustInherit Class RegistryRowBase
        Private m_DB As Database
        Private m_RegistryId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_IsPublic As Boolean = Nothing
        Private m_CoFirstName As String = Nothing
        Private m_CoLastName As String = Nothing
        Private m_EventType As String = Nothing
        Private m_EventDate As DateTime = Nothing
        Private m_CreateDate As DateTime = Nothing


        Public Property RegistryId() As Integer
            Get
                Return m_RegistryId
            End Get
            Set(ByVal Value As Integer)
                m_RegistryId = Value
            End Set
        End Property

        Public Property IsPublic() As Boolean
            Get
                Return m_IsPublic
            End Get
            Set(ByVal Value As Boolean)
                m_IsPublic = Value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal Value As String)
                m_FirstName = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal Value As String)
                m_LastName = Value
            End Set
        End Property

        Public Property CoFirstName() As String
            Get
                Return m_CoFirstName
            End Get
            Set(ByVal Value As String)
                m_CoFirstName = Value
            End Set
        End Property

        Public Property CoLastName() As String
            Get
                Return m_CoLastName
            End Get
            Set(ByVal Value As String)
                m_CoLastName = Value
            End Set
        End Property

        Public Property EventType() As String
            Get
                Return m_EventType
            End Get
            Set(ByVal Value As String)
                m_EventType = Value
            End Set
        End Property

        Public Property EventDate() As DateTime
            Get
                Return m_EventDate
            End Get
            Set(ByVal Value As DateTime)
                m_EventDate = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreateDate = Value
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

        Public Sub New(ByVal database As Database, ByVal RegistryId As Integer)
            m_DB = database
            m_RegistryId = RegistryId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Registry WHERE RegistryId = " & DB.Quote(RegistryId)
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
            m_MemberId = Convert.ToInt32(r.Item("MemberId"))
            If r.Item("Name") Is Convert.DBNull Then
                m_Name = Nothing
            Else
                m_Name = Convert.ToString(r.Item("Name"))
            End If
            m_FirstName = Convert.ToString(r.Item("FirstName"))
            m_LastName = Convert.ToString(r.Item("LastName"))
            If r.Item("CoFirstName") Is Convert.DBNull Then
                m_CoFirstName = Nothing
            Else
                m_CoFirstName = Convert.ToString(r.Item("CoFirstName"))
            End If
            If r.Item("CoLastName") Is Convert.DBNull Then
                m_CoLastName = Nothing
            Else
                m_CoLastName = Convert.ToString(r.Item("CoLastName"))
            End If
            If r.Item("EventType") Is Convert.DBNull Then
                m_EventType = Nothing
            Else
                m_EventType = Convert.ToString(r.Item("EventType"))
            End If
            m_EventDate = Convert.ToDateTime(r.Item("EventDate"))
            m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
            m_IsPublic = Convert.ToBoolean(r.Item("IsPublic"))
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO Registry (" _
                 & " MemberId" _
                 & ",Name" _
                 & ",FirstName" _
                 & ",LastName" _
                 & ",CoFirstName" _
                 & ",CoLastName" _
                 & ",EventType" _
                 & ",EventDate" _
                 & ",CreateDate" _
                 & ",IsPublic" _
                 & ") VALUES (" _
                 & DB.Quote(MemberId) _
                 & "," & DB.Quote(Name) _
                 & "," & DB.Quote(FirstName) _
                 & "," & DB.Quote(LastName) _
                 & "," & DB.Quote(CoFirstName) _
                 & "," & DB.Quote(CoLastName) _
                 & "," & DB.Quote(EventType) _
                 & "," & DB.Quote(EventDate) _
                 & "," & DB.Quote(CreateDate) _
                 & "," & CInt(IsPublic) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            RegistryId = m_DB.InsertSQL(InsertStatement)
            Return RegistryId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Registry SET " _
             & " MemberId = " & DB.Quote(MemberId) _
             & ",Name = " & DB.Quote(Name) _
             & ",FirstName = " & DB.Quote(FirstName) _
             & ",LastName = " & DB.Quote(LastName) _
             & ",CoFirstName = " & DB.Quote(CoFirstName) _
             & ",CoLastName = " & DB.Quote(CoLastName) _
             & ",EventType = " & DB.Quote(EventType) _
             & ",EventDate = " & DB.Quote(EventDate) _
             & ",CreateDate = " & DB.Quote(CreateDate) _
             & ",IsPublic = " & CInt(IsPublic) _
             & " WHERE RegistryId = " & DB.Quote(RegistryId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM Registry WHERE RegistryId = " & DB.Quote(RegistryId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class RegistryCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Registry As RegistryRow)
            Me.List.Add(Registry)
        End Sub

        Public Function Contains(ByVal Registry As RegistryRow) As Boolean
            Return Me.List.Contains(Registry)
        End Function

        Public Function IndexOf(ByVal Registry As RegistryRow) As Integer
            Return Me.List.IndexOf(Registry)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Registry As RegistryRow)
            Me.List.Insert(Index, Registry)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As RegistryRow
            Get
                Return CType(Me.List.Item(Index), RegistryRow)
            End Get

            Set(ByVal Value As RegistryRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Registry As RegistryRow)
            Me.List.Remove(Registry)
        End Sub
    End Class

End Namespace


