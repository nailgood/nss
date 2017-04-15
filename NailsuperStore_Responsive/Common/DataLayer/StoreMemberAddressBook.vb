Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text

Namespace DataLayer

    Public Class StoreMemberAddressBookRow
        Inherits StoreMemberAddressBookRowBase

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
        Public Shared Function GetRow(ByVal _Database As Database, ByVal AddressId As Integer) As StoreMemberAddressBookRow
            Dim row As StoreMemberAddressBookRow

            row = New StoreMemberAddressBookRow(_Database, AddressId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetDefaultMemberRow(ByVal database As Database, ByVal MemberId As Integer)
            Dim row As StoreMemberAddressBookRow, AddressId As Integer
            AddressId = database.ExecuteScalar("SELECT AddressId FROM StoreMemberAddressBook WHERE MemberId=" & MemberId & " AND IsDefault = 1")
            row = New StoreMemberAddressBookRow(database, AddressId)
            row.Load()
            Return row
        End Function


        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal AddressId As Integer)
            Dim row As StoreMemberAddressBookRow

            row = New StoreMemberAddressBookRow(_Database, AddressId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class StoreMemberAddressBookRowBase
        Private m_DB As Database
        Private m_AddressId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_BelongsTo As String = Nothing
        Private m_ShippingLabel As String = Nothing
        Private m_ShippingFirstName As String = Nothing
        Private m_ShippingLastName As String = Nothing
        Private m_ShippingCompany As String = Nothing
        Private m_ShippingAddress1 As String = Nothing
        Private m_ShippingAddress2 As String = Nothing
        Private m_ShippingCity As String = Nothing
        Private m_ShippingState As String = Nothing
        Private m_ShippingZip As String = Nothing
        Private m_ShippingDaytimePhone As String = Nothing
        Private m_ShippingEveningPhone As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_IsDefault As Boolean = Nothing


        Public Property AddressId() As Integer
            Get
                Return m_AddressId
            End Get
            Set(ByVal Value As Integer)
                m_AddressId = value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = value
            End Set
        End Property

        Public Property BelongsTo() As String
            Get
                Return m_BelongsTo
            End Get
            Set(ByVal Value As String)
                m_BelongsTo = Value
            End Set
        End Property

        Public Property ShippingLabel() As String
            Get
                Return m_ShippingLabel
            End Get
            Set(ByVal Value As String)
                m_ShippingLabel = Value
            End Set
        End Property

        Public Property ShippingFirstName() As String
            Get
                Return m_ShippingFirstName
            End Get
            Set(ByVal Value As String)
                m_ShippingFirstName = Value
            End Set
        End Property

        Public Property ShippingLastName() As String
            Get
                Return m_ShippingLastName
            End Get
            Set(ByVal Value As String)
                m_ShippingLastName = Value
            End Set
        End Property

        Public Property ShippingCompany() As String
            Get
                Return m_ShippingCompany
            End Get
            Set(ByVal Value As String)
                m_ShippingCompany = Value
            End Set
        End Property

        Public Property ShippingAddress1() As String
            Get
                Return m_ShippingAddress1
            End Get
            Set(ByVal Value As String)
                m_ShippingAddress1 = Value
            End Set
        End Property

        Public Property ShippingAddress2() As String
            Get
                Return m_ShippingAddress2
            End Get
            Set(ByVal Value As String)
                m_ShippingAddress2 = Value
            End Set
        End Property

        Public Property ShippingCity() As String
            Get
                Return m_ShippingCity
            End Get
            Set(ByVal Value As String)
                m_ShippingCity = Value
            End Set
        End Property

        Public Property ShippingState() As String
            Get
                Return m_ShippingState
            End Get
            Set(ByVal Value As String)
                m_ShippingState = Value
            End Set
        End Property

        Public Property ShippingZip() As String
            Get
                Return m_ShippingZip
            End Get
            Set(ByVal Value As String)
                m_ShippingZip = Value
            End Set
        End Property

        Public Property ShippingDaytimePhone() As String
            Get
                Return m_ShippingDaytimePhone
            End Get
            Set(ByVal Value As String)
                m_ShippingDaytimePhone = Value
            End Set
        End Property

        Public Property ShippingEveningPhone() As String
            Get
                Return m_ShippingEveningPhone
            End Get
            Set(ByVal Value As String)
                m_ShippingEveningPhone = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property

        Public Property IsDefault() As Boolean
            Get
                Return m_IsDefault
            End Get
            Set(ByVal Value As Boolean)
                m_IsDefault = Value
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

                SQL = "SELECT * FROM StoreMemberAddressBook WHERE AddressId = " & DB.Quote(AddressId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                m_BelongsTo = Convert.ToString(r.Item("BelongsTo"))
                m_ShippingLabel = Convert.ToString(r.Item("ShippingLabel"))
                m_ShippingFirstName = Convert.ToString(r.Item("ShippingFirstName"))
                m_ShippingLastName = Convert.ToString(r.Item("ShippingLastName"))
                If r.Item("ShippingCompany") Is Convert.DBNull Then
                    m_ShippingCompany = Nothing
                Else
                    m_ShippingCompany = Convert.ToString(r.Item("ShippingCompany"))
                End If
                m_ShippingAddress1 = Convert.ToString(r.Item("ShippingAddress1"))
                If r.Item("ShippingAddress2") Is Convert.DBNull Then
                    m_ShippingAddress2 = Nothing
                Else
                    m_ShippingAddress2 = Convert.ToString(r.Item("ShippingAddress2"))
                End If
                m_ShippingCity = Convert.ToString(r.Item("ShippingCity"))
                m_ShippingState = Convert.ToString(r.Item("ShippingState"))

                m_ShippingZip = Convert.ToString(r.Item("ShippingZip"))
                If r.Item("ShippingDaytimePhone") Is Convert.DBNull Then
                    m_ShippingDaytimePhone = Nothing
                Else
                    m_ShippingDaytimePhone = Convert.ToString(r.Item("ShippingDaytimePhone"))
                End If
                If r.Item("ShippingEveningPhone") Is Convert.DBNull Then
                    m_ShippingEveningPhone = Nothing
                Else
                    m_ShippingEveningPhone = Convert.ToString(r.Item("ShippingEveningPhone"))
                End If
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                m_IsDefault = Convert.ToBoolean(r.Item("IsDefault"))
            Catch ex As Exception
                Throw ex
                '' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreMemberAddressBook (" _
                 & " MemberId" _
                 & ",BelongsTo" _
                 & ",ShippingLabel" _
                 & ",ShippingFirstName" _
                 & ",ShippingLastName" _
                 & ",ShippingCompany" _
                 & ",ShippingAddress1" _
                 & ",ShippingAddress2" _
                 & ",ShippingCity" _
                 & ",ShippingState" _
                 & ",ShippingZip" _
                 & ",ShippingDaytimePhone" _
                 & ",ShippingEveningPhone" _
                 & ",IsActive" _
                 & ",IsDefault" _
                 & ") VALUES (" _
                 & m_DB.Quote(MemberId) _
                 & "," & m_DB.Quote(BelongsTo) _
                 & "," & m_DB.Quote(ShippingLabel) _
                 & "," & m_DB.Quote(ShippingFirstName) _
                 & "," & m_DB.Quote(ShippingLastName) _
                 & "," & m_DB.Quote(ShippingCompany) _
                 & "," & m_DB.Quote(ShippingAddress1) _
                 & "," & m_DB.Quote(ShippingAddress2) _
                 & "," & m_DB.Quote(ShippingCity) _
                 & "," & m_DB.Quote(ShippingState) _
                 & "," & m_DB.Quote(ShippingZip) _
                 & "," & m_DB.Quote(ShippingDaytimePhone) _
                 & "," & m_DB.Quote(ShippingEveningPhone) _
                 & "," & CInt(IsActive) _
                 & "," & CInt(IsDefault) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Public Overridable Sub UpdateDefaultMemberAddress()
            Dim iRowsAffected As Integer
            iRowsAffected = m_DB.ExecuteSQL("UPDATE StoreMemberAddressBook SET " _
             & "ShippingFirstName = " & m_DB.Quote(ShippingFirstName) _
             & ",BelongsTo = " & m_DB.Quote(BelongsTo) _
             & ",ShippingLabel = " & m_DB.Quote(ShippingLabel) _
             & ",ShippingLastName = " & m_DB.Quote(ShippingLastName) _
             & ",ShippingCompany = " & m_DB.Quote(ShippingCompany) _
             & ",ShippingAddress1 = " & m_DB.Quote(ShippingAddress1) _
             & ",ShippingAddress2 = " & m_DB.Quote(ShippingAddress2) _
             & ",ShippingCity = " & m_DB.Quote(ShippingCity) _
             & ",ShippingState = " & m_DB.Quote(ShippingState) _
             & ",ShippingZip = " & m_DB.Quote(ShippingZip) _
             & ",ShippingDaytimePhone = " & m_DB.Quote(ShippingDaytimePhone) _
             & ",ShippingEveningPhone = " & m_DB.Quote(ShippingEveningPhone) _
             & ",IsActive = " & CInt(IsActive) _
             & " WHERE MemberId = " & m_DB.Quote(MemberId) & " AND IsDefault = 1")

            If iRowsAffected <> 1 Then Insert()
        End Sub

        Function AutoInsert() As Integer
            AddressId = m_DB.InsertSQL(InsertStatement)
            Return AddressId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreMemberAddressBook SET " _
             & " MemberId = " & m_DB.Quote(MemberId) _
             & ",ShippingLabel = " & m_DB.Quote(ShippingLabel) _
             & ",BelongsTo = " & m_DB.Quote(BelongsTo) _
             & ",ShippingFirstName = " & m_DB.Quote(ShippingFirstName) _
             & ",ShippingLastName = " & m_DB.Quote(ShippingLastName) _
             & ",ShippingCompany = " & m_DB.Quote(ShippingCompany) _
             & ",ShippingAddress1 = " & m_DB.Quote(ShippingAddress1) _
             & ",ShippingAddress2 = " & m_DB.Quote(ShippingAddress2) _
             & ",ShippingCity = " & m_DB.Quote(ShippingCity) _
             & ",ShippingState = " & m_DB.Quote(ShippingState) _
             & ",ShippingZip = " & m_DB.Quote(ShippingZip) _
             & ",ShippingDaytimePhone = " & m_DB.Quote(ShippingDaytimePhone) _
             & ",ShippingEveningPhone = " & m_DB.Quote(ShippingEveningPhone) _
             & ",IsActive = " & CInt(IsActive) _
             & ",IsDefault = " & CInt(IsDefault) _
             & " WHERE AddressId = " & m_DB.Quote(AddressId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreMemberAddressBook WHERE AddressId = " & m_DB.Quote(AddressId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreMemberAddressBookCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreMemberAddressBook As StoreMemberAddressBookRow)
            Me.List.Add(StoreMemberAddressBook)
        End Sub

        Public Function Contains(ByVal StoreMemberAddressBook As StoreMemberAddressBookRow) As Boolean
            Return Me.List.Contains(StoreMemberAddressBook)
        End Function

        Public Function IndexOf(ByVal StoreMemberAddressBook As StoreMemberAddressBookRow) As Integer
            Return Me.List.IndexOf(StoreMemberAddressBook)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreMemberAddressBook As StoreMemberAddressBookRow)
            Me.List.Insert(Index, StoreMemberAddressBook)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreMemberAddressBookRow
            Get
                Return CType(Me.List.Item(Index), StoreMemberAddressBookRow)
            End Get

            Set(ByVal Value As StoreMemberAddressBookRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreMemberAddressBook As StoreMemberAddressBookRow)
            Me.List.Remove(StoreMemberAddressBook)
        End Sub
    End Class

End Namespace