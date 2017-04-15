Option Explicit On 

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Database

Namespace DataLayer

    Public Class RegistryAddressRow
        Inherits RegistryAddressRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal AddressId As Integer)
            MyBase.New(database, AddressId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal AddressId As Integer) As RegistryAddressRow
            Dim row As RegistryAddressRow

            row = New RegistryAddressRow(_Database, AddressId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRowByRegistryType(ByVal _Database As Database, ByVal RegistryId As Integer, ByVal AddressType As String) As RegistryAddressRow
            Dim row As RegistryAddressRow
            Dim r As SqlDataReader = Nothing
            Try
                row = New RegistryAddressRow(_Database)
                Dim SQL As String = "select * from RegistryAddress where RegistryId = " & RegistryId & " and AddressType = " & _Database.Quote(AddressType)
                r = _Database.GetReader(SQL)
                If r.Read Then
                    row.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
                ''mail error here
            End Try

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal AddressId As Integer)
            Dim row As RegistryAddressRow

            row = New RegistryAddressRow(_Database, AddressId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class RegistryAddressRowBase
        Private m_DB As Database
        Private m_AddressId As Integer = Nothing
        Private m_RegistryId As Integer = Nothing
        Private m_AddressType As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_CompanyName As String = Nothing
        Private m_Address1 As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_State As String = Nothing
        Private m_City As String = Nothing
        Private m_Zip As String = Nothing
        Private m_Phone As String = Nothing


        Public Property AddressId() As Integer
            Get
                Return m_AddressId
            End Get
            Set(ByVal Value As Integer)
                m_AddressId = value
            End Set
        End Property

        Public Property RegistryId() As Integer
            Get
                Return m_RegistryId
            End Get
            Set(ByVal Value As Integer)
                m_RegistryId = value
            End Set
        End Property

        Public Property AddressType() As String
            Get
                Return m_AddressType
            End Get
            Set(ByVal Value As String)
                m_AddressType = value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal Value As String)
                m_FirstName = value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal Value As String)
                m_LastName = value
            End Set
        End Property

        Public Property CompanyName() As String
            Get
                Return m_CompanyName
            End Get
            Set(ByVal Value As String)
                m_CompanyName = value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return m_Address1
            End Get
            Set(ByVal Value As String)
                m_Address1 = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return m_Address2
            End Get
            Set(ByVal Value As String)
                m_Address2 = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(ByVal Value As String)
                m_State = value
            End Set
        End Property

        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(ByVal Value As String)
                m_City = value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Return m_Zip
            End Get
            Set(ByVal Value As String)
                m_Zip = value
            End Set
        End Property

        Public Property Phone() As String
            Get
                Return m_Phone
            End Get
            Set(ByVal Value As String)
                m_Phone = value
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

        Public Sub New(ByVal database As Database, ByVal AddressId As Integer)
            m_DB = database
            m_AddressId = AddressId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM RegistryAddress WHERE AddressId = " & DB.Quote(AddressId)
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
            m_AddressId = Convert.ToInt32(r.Item("AddressId"))
            m_RegistryId = Convert.ToInt32(r.Item("RegistryId"))
            m_AddressType = Convert.ToString(r.Item("AddressType"))
            m_FirstName = Convert.ToString(r.Item("FirstName"))
            m_LastName = Convert.ToString(r.Item("LastName"))
            If r.Item("CompanyName") Is Convert.DBNull Then
                m_CompanyName = Nothing
            Else
                m_CompanyName = Convert.ToString(r.Item("CompanyName"))
            End If
            m_Address1 = Convert.ToString(r.Item("Address1"))
            If r.Item("Address2") Is Convert.DBNull Then
                m_Address2 = Nothing
            Else
                m_Address2 = Convert.ToString(r.Item("Address2"))
            End If
            m_State = Convert.ToString(r.Item("State"))
            m_City = Convert.ToString(r.Item("City"))
            m_Zip = Convert.ToString(r.Item("Zip"))
            m_Phone = Convert.ToString(r.Item("Phone"))
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO RegistryAddress (" _
                 & " RegistryId" _
                 & ",AddressType" _
                 & ",FirstName" _
                 & ",LastName" _
                 & ",CompanyName" _
                 & ",Address1" _
                 & ",Address2" _
                 & ",State" _
                 & ",City" _
                 & ",Zip" _
                 & ",Phone" _
                 & ") VALUES (" _
                 & DB.Quote(RegistryId) _
                 & "," & DB.Quote(AddressType) _
                 & "," & DB.Quote(FirstName) _
                 & "," & DB.Quote(LastName) _
                 & "," & DB.Quote(CompanyName) _
                 & "," & DB.Quote(Address1) _
                 & "," & DB.Quote(Address2) _
                 & "," & DB.Quote(State) _
                 & "," & DB.Quote(City) _
                 & "," & DB.Quote(Zip) _
                 & "," & DB.Quote(Phone) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            AddressId = m_DB.InsertSQL(InsertStatement)
            Return AddressId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE RegistryAddress SET " _
             & " RegistryId = " & DB.Quote(RegistryId) _
             & ",AddressType = " & DB.Quote(AddressType) _
             & ",FirstName = " & DB.Quote(FirstName) _
             & ",LastName = " & DB.Quote(LastName) _
             & ",CompanyName = " & DB.Quote(CompanyName) _
             & ",Address1 = " & DB.Quote(Address1) _
             & ",Address2 = " & DB.Quote(Address2) _
             & ",State = " & DB.Quote(State) _
             & ",City = " & DB.Quote(City) _
             & ",Zip = " & DB.Quote(Zip) _
             & ",Phone = " & DB.Quote(Phone) _
             & " WHERE AddressId = " & DB.Quote(AddressId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM RegistryAddress WHERE AddressId = " & DB.Quote(AddressId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class RegistryAddressCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal RegistryAddress As RegistryAddressRow)
            Me.List.Add(RegistryAddress)
        End Sub

        Public Function Contains(ByVal RegistryAddress As RegistryAddressRow) As Boolean
            Return Me.List.Contains(RegistryAddress)
        End Function

        Public Function IndexOf(ByVal RegistryAddress As RegistryAddressRow) As Integer
            Return Me.List.IndexOf(RegistryAddress)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal RegistryAddress As RegistryAddressRow)
            Me.List.Insert(Index, RegistryAddress)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As RegistryAddressRow
            Get
                Return CType(Me.List.Item(Index), RegistryAddressRow)
            End Get

            Set(ByVal Value As RegistryAddressRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal RegistryAddress As RegistryAddressRow)
            Me.List.Remove(RegistryAddress)
        End Sub
    End Class

End Namespace


