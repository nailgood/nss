Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Namespace DataLayer

    Public Class LocationRow
        Inherits LocationRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal LocationId As Integer)
            MyBase.New(database, LocationId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal LocationId As Integer) As LocationRow
            Dim row As LocationRow

            row = New LocationRow(_Database, LocationId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal LocationId As Integer)
            Dim row As LocationRow

            row = New LocationRow(_Database, LocationId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetLocations(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select * from Location order by SortOrder")
            Return ds
        End Function

    End Class

    Public MustInherit Class LocationRowBase
        Private m_DB As Database
        Private m_LocationId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_AddressLn1 As String = Nothing
        Private m_AddressLn2 As String = Nothing
        Private m_City As String = Nothing
        Private m_State As String = Nothing
        Private m_Zip As String = Nothing
        Private m_Email As String = Nothing
        Private m_Latitude As String = Nothing
        Private m_Longitude As String = Nothing
        Private m_Phone As String = Nothing
        Private m_Fax As String = Nothing


        Public Property LocationId() As Integer
            Get
                Return m_LocationId
            End Get
            Set(ByVal Value As Integer)
                m_LocationId = value
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

        Public Property AddressLn1() As String
            Get
                Return m_AddressLn1
            End Get
            Set(ByVal Value As String)
                m_AddressLn1 = value
            End Set
        End Property

        Public Property AddressLn2() As String
            Get
                Return m_AddressLn2
            End Get
            Set(ByVal Value As String)
                m_AddressLn2 = value
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

        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(ByVal Value As String)
                m_State = value
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

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = value
            End Set
        End Property

        Public Property Latitude() As String
            Get
                Return m_Latitude
            End Get
            Set(ByVal Value As String)
                m_Latitude = value
            End Set
        End Property

        Public Property Longitude() As String
            Get
                Return m_Longitude
            End Get
            Set(ByVal Value As String)
                m_Longitude = value
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

        Public Property Fax() As String
            Get
                Return m_Fax
            End Get
            Set(ByVal Value As String)
                m_Fax = value
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

        Public Sub New(ByVal database As Database, ByVal LocationId As Integer)
            m_DB = database
            m_LocationId = LocationId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Location WHERE LocationId = " & DB.Quote(LocationId)
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
            m_Name = Convert.ToString(r.Item("Name"))
            m_AddressLn1 = Convert.ToString(r.Item("AddressLn1"))
            If IsDBNull(r.Item("AddressLn2")) Then
                m_AddressLn2 = Nothing
            Else
                m_AddressLn2 = Convert.ToString(r.Item("AddressLn2"))
            End If
            m_City = Convert.ToString(r.Item("City"))
            m_State = Convert.ToString(r.Item("State"))
            m_Zip = Convert.ToString(r.Item("Zip"))
            m_Email = Convert.ToString(r.Item("Email"))
            m_Latitude = Convert.ToString(r.Item("Latitude"))
            m_Longitude = Convert.ToString(r.Item("Longitude"))
            If IsDBNull(r.Item("Phone")) Then
                m_Phone = Nothing
            Else
                m_Phone = Convert.ToString(r.Item("Phone"))
            End If
            If IsDBNull(r.Item("Fax")) Then
                m_Fax = Nothing
            Else
                m_Fax = Convert.ToString(r.Item("Fax"))
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            Dim SortOrder As Integer = DB.ExecuteScalar("select coalesce(max(sortorder),0) from Location")
            SortOrder += 1

            SQL = " INSERT INTO Location (" _
             & " Name" _
             & ",AddressLn1" _
             & ",AddressLn2" _
             & ",City" _
             & ",State" _
             & ",Zip" _
             & ",Email" _
             & ",Latitude" _
             & ",Longitude" _
             & ",SortOrder" _
             & ",Phone" _
             & ",Fax" _
             & ") VALUES (" _
             & m_DB.Quote(Name) _
             & "," & m_DB.Quote(AddressLn1) _
             & "," & m_DB.Quote(AddressLn2) _
             & "," & m_DB.Quote(City) _
             & "," & m_DB.Quote(State) _
             & "," & m_DB.Quote(Zip) _
             & "," & m_DB.Quote(Email) _
             & "," & m_DB.Quote(Latitude) _
             & "," & m_DB.Quote(Longitude) _
             & "," & m_DB.Quote(SortOrder) _
             & "," & m_DB.Quote(Phone) _
             & "," & m_DB.Quote(Fax) _
             & ")"

            LocationId = m_DB.InsertSQL(SQL)

            Return LocationId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Location SET " _
             & " Name = " & m_DB.Quote(Name) _
             & ",AddressLn1 = " & m_DB.Quote(AddressLn1) _
             & ",AddressLn2 = " & m_DB.Quote(AddressLn2) _
             & ",City = " & m_DB.Quote(City) _
             & ",State = " & m_DB.Quote(State) _
             & ",Zip = " & m_DB.Quote(Zip) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",Latitude = " & m_DB.Quote(Latitude) _
             & ",Longitude = " & m_DB.Quote(Longitude) _
             & ",Phone = " & m_DB.Quote(Phone) _
             & ",Fax = " & m_DB.Quote(Fax) _
             & " WHERE LocationId = " & m_DB.Quote(LocationId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM Location WHERE LocationId = " & m_DB.Quote(LocationId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class LocationCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Location As LocationRow)
            Me.List.Add(Location)
        End Sub

        Public Function Contains(ByVal Location As LocationRow) As Boolean
            Return Me.List.Contains(Location)
        End Function

        Public Function IndexOf(ByVal Location As LocationRow) As Integer
            Return Me.List.IndexOf(Location)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Location As LocationRow)
            Me.List.Insert(Index, Location)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As LocationRow
            Get
                Return CType(Me.List.Item(Index), LocationRow)
            End Get

            Set(ByVal Value As LocationRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Location As LocationRow)
            Me.List.Remove(Location)
        End Sub
    End Class

End Namespace


