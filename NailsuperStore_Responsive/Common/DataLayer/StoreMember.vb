Option Explicit On 

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Utility

Namespace DataLayer

    Public Class StoreMemberRow
        Inherits StoreMemberRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal MemberId As Integer)
            MyBase.New(database, MemberId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal MemberId As Integer) As StoreMemberRow
            Dim row As StoreMemberRow

            row = New StoreMemberRow(_Database, MemberId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal MemberId As Integer)
            Dim row As StoreMemberRow

            row = New StoreMemberRow(_Database, MemberId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function ValidateMemberCredentials(ByVal _Database As Database, ByVal Username As String, ByVal Password As String) As Integer
            Dim Encrypted As String = CryptData.Crypt.DecryptTripleDes(Password)

            Dim SQL As String = "select MemberId FROM StoreMember where Username = " & _Database.Quote(Username) _
                & " and Password = " & _Database.Quote(Encrypted)

            Dim iMemberId As Integer = CType(_Database.ExecuteScalar(SQL), Integer)
            Return iMemberId
        End Function

        Public Shared Function GetRowByUsername(ByVal _database As Database, ByVal Username As String) As StoreMemberRow
            Dim r As SqlDataReader = Nothing
            Dim row As StoreMemberRow = New StoreMemberRow(_database)
            Try
                Dim SQL As String = "SELECT * FROM StoreMember WHERE Username = " & _database.Quote(Username)
                r = _database.GetReader(SQL)
                If r.Read Then
                    row.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return row
        End Function
    End Class

    Public MustInherit Class StoreMemberRowBase
        Private m_DB As Database
        Private m_MemberId As Integer = Nothing
        Private m_Email As String = Nothing
        Private m_Username As String = Nothing
        Private m_Password As String = Nothing
        Private m_BillingFirstName As String = Nothing
        Private m_BillingLastName As String = Nothing
        Private m_BillingCompany As String = Nothing
        Private m_BillingAddress1 As String = Nothing
        Private m_BillingAddress2 As String = Nothing
        Private m_BillingState As String = Nothing
        Private m_BillingCity As String = Nothing
        Private m_BillingZip As String = Nothing
        Private m_BillingDaytimePhone As String = Nothing
        Private m_EdpNo As String = Nothing
        Private m_IsCatalog As Boolean = Nothing
        Private m_IsNewsletter As Boolean = Nothing
        Private m_IsDesigner As Boolean = Nothing
        Private m_DoNotRent As Boolean = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_DateRegistered As DateTime = Nothing

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return m_Username
            End Get
            Set(ByVal Value As String)
                m_Username = Value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return m_Password
            End Get
            Set(ByVal Value As String)
                m_Password = Value
            End Set
        End Property

        Public ReadOnly Property EncryptedPassword() As String
            Get
                If m_Password = String.Empty Then
                    Return String.Empty
                End If
                Return CryptData.Crypt.DecryptTripleDes(Password)
            End Get
        End Property

        Public Property BillingFirstName() As String
            Get
                Return m_BillingFirstName
            End Get
            Set(ByVal Value As String)
                m_BillingFirstName = Value
            End Set
        End Property

        Public Property BillingLastName() As String
            Get
                Return m_BillingLastName
            End Get
            Set(ByVal Value As String)
                m_BillingLastName = Value
            End Set
        End Property

        Public Property BillingCompany() As String
            Get
                Return m_BillingCompany
            End Get
            Set(ByVal Value As String)
                m_BillingCompany = Value
            End Set
        End Property

        Public Property BillingAddress1() As String
            Get
                Return m_BillingAddress1
            End Get
            Set(ByVal Value As String)
                m_BillingAddress1 = Value
            End Set
        End Property

        Public Property BillingAddress2() As String
            Get
                Return m_BillingAddress2
            End Get
            Set(ByVal Value As String)
                m_BillingAddress2 = Value
            End Set
        End Property

        Public Property BillingState() As String
            Get
                Return m_BillingState
            End Get
            Set(ByVal Value As String)
                m_BillingState = Value
            End Set
        End Property

        Public Property BillingCity() As String
            Get
                Return m_BillingCity
            End Get
            Set(ByVal Value As String)
                m_BillingCity = Value
            End Set
        End Property

        Public Property BillingZip() As String
            Get
                Return m_BillingZip
            End Get
            Set(ByVal Value As String)
                m_BillingZip = Value
            End Set
        End Property

        Public Property BillingDaytimePhone() As String
            Get
                Return m_BillingDaytimePhone
            End Get
            Set(ByVal Value As String)
                m_BillingDaytimePhone = Value
            End Set
        End Property

        Public Property EdpNo() As String
            Get
                Return m_EdpNo
            End Get
            Set(ByVal Value As String)
                m_EdpNo = Value
            End Set
        End Property

        Public Property IsCatalog() As Boolean
            Get
                Return m_IsCatalog
            End Get
            Set(ByVal Value As Boolean)
                m_IsCatalog = Value
            End Set
        End Property

        Public Property IsNewsletter() As Boolean
            Get
                Return m_IsNewsletter
            End Get
            Set(ByVal Value As Boolean)
                m_IsNewsletter = Value
            End Set
        End Property

        Public Property IsDesigner() As Boolean
            Get
                Return m_IsDesigner
            End Get
            Set(ByVal Value As Boolean)
                m_IsDesigner = Value
            End Set
        End Property

        Public Property DoNotRent() As Boolean
            Get
                Return m_DoNotRent
            End Get
            Set(ByVal Value As Boolean)
                m_DoNotRent = Value
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

        Public Property DateRegistered() As DateTime
            Get
                Return m_DateRegistered
            End Get
            Set(ByVal Value As DateTime)
                m_DateRegistered = Value
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

        Public Sub New(ByVal database As Database, ByVal MemberId As Integer)
            m_DB = database
            m_MemberId = MemberId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreMember WHERE MemberId = " & DB.Quote(MemberId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                m_Username = Convert.ToString(r.Item("Username"))
                m_Password = CryptData.Crypt.DecryptTripleDes(r.Item("Password"))
                m_BillingFirstName = Convert.ToString(r.Item("BillingFirstName"))
                m_BillingLastName = Convert.ToString(r.Item("BillingLastName"))
                m_BillingState = Convert.ToString(r.Item("BillingState"))
                If r.Item("BillingCompany") Is Convert.DBNull Then
                    m_BillingCompany = Nothing
                Else
                    m_BillingCompany = Convert.ToString(r.Item("BillingCompany"))
                End If
                If r.Item("Email") Is Convert.DBNull Then
                    m_Email = Nothing
                Else
                    m_Email = Convert.ToString(r.Item("Email"))
                End If
                m_BillingAddress1 = Convert.ToString(r.Item("BillingAddress1"))
                If r.Item("BillingAddress2") Is Convert.DBNull Then
                    m_BillingAddress2 = Nothing
                Else
                    m_BillingAddress2 = Convert.ToString(r.Item("BillingAddress2"))
                End If
                m_BillingCity = Convert.ToString(r.Item("BillingCity"))
                m_BillingZip = Convert.ToString(r.Item("BillingZip"))
                If r.Item("BillingDaytimePhone") Is Convert.DBNull Then
                    m_BillingDaytimePhone = Nothing
                Else
                    m_BillingDaytimePhone = Convert.ToString(r.Item("BillingDaytimePhone"))
                End If
                If Not r.Item("EdpNo") Is Convert.DBNull Then
                    m_EdpNo = Convert.ToString(r.Item("EdpNo"))
                End If
                m_IsCatalog = Convert.ToBoolean(r.Item("IsCatalog"))
                m_IsNewsletter = Convert.ToBoolean(r.Item("IsNewsletter"))
                m_IsDesigner = Convert.ToBoolean(r.Item("IsDesigner"))
                m_DoNotRent = Convert.ToBoolean(r.Item("DoNotRent"))
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                m_DateRegistered = Convert.ToDateTime(r.Item("DateRegistered"))
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreMember (" _
                 & "Username" _
                 & ",Email" _
                 & ",Password" _
                 & ",BillingFirstName" _
                 & ",BillingLastName" _
                 & ",BillingCompany" _
                 & ",BillingAddress1" _
                 & ",BillingAddress2" _
                 & ",BillingState" _
                 & ",BillingCity" _
                 & ",BillingZip" _
                 & ",BillingDaytimePhone" _
                 & ",EdpNo" _
                 & ",IsCatalog" _
                 & ",IsNewsletter" _
                 & ",IsDesigner" _
                 & ",DoNotRent" _
                 & ",IsActive" _
                 & ") VALUES (" _
                 & m_DB.Quote(Username) _
                 & "," & m_DB.Quote(Email) _
                 & "," & m_DB.Quote(EncryptedPassword) _
                 & "," & m_DB.Quote(BillingFirstName) _
                 & "," & m_DB.Quote(BillingLastName) _
                 & "," & m_DB.Quote(BillingCompany) _
                 & "," & m_DB.Quote(BillingAddress1) _
                 & "," & m_DB.Quote(BillingAddress2) _
                 & "," & m_DB.Quote(BillingState) _
                 & "," & m_DB.Quote(BillingCity) _
                 & "," & m_DB.Quote(BillingZip) _
                 & "," & m_DB.Quote(BillingDaytimePhone) _
                 & "," & m_DB.Quote(EdpNo) _
                 & "," & CInt(IsCatalog) _
                 & "," & CInt(IsNewsletter) _
                 & "," & CInt(IsDesigner) _
                 & "," & CInt(DoNotRent) _
                 & "," & CInt(IsActive) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            Try
                m_MemberId = m_DB.InsertSQL(InsertStatement)
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & InsertStatement & vbCrLf & vbCrLf)
            End Try
        End Sub 'Insert

        Function AutoInsert() As Integer
            Try
                MemberId = m_DB.InsertSQL(InsertStatement)
                Return MemberId
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & InsertStatement & vbCrLf & vbCrLf)
            End Try
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreMember SET " _
             & " Email = " & m_DB.Quote(Email) _
             & ",Username = " & m_DB.Quote(Username) _
             & ",Password = " & m_DB.Quote(EncryptedPassword) _
             & ",BillingFirstName = " & m_DB.Quote(BillingFirstName) _
             & ",BillingLastName = " & m_DB.Quote(BillingLastName) _
             & ",BillingCompany = " & m_DB.Quote(BillingCompany) _
             & ",BillingAddress1 = " & m_DB.Quote(BillingAddress1) _
             & ",BillingAddress2 = " & m_DB.Quote(BillingAddress2) _
             & ",BillingState = " & m_DB.Quote(BillingState) _
             & ",BillingCity = " & m_DB.Quote(BillingCity) _
             & ",BillingZip = " & m_DB.Quote(BillingZip) _
             & ",BillingDaytimePhone = " & m_DB.Quote(BillingDaytimePhone) _
             & ",EdpNo = " & m_DB.Quote(EdpNo) _
             & ",IsCatalog = " & CInt(IsCatalog) _
             & ",IsNewsletter = " & CInt(IsNewsletter) _
             & ",IsDesigner = " & CInt(IsDesigner) _
             & ",DoNotRent = " & CInt(DoNotRent) _
             & ",IsActive = " & CInt(IsActive) _
             & " WHERE MemberId = " & m_DB.Quote(MemberId)


            Try
                m_DB.ExecuteSQL(SQL)
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & SQL & vbCrLf & vbCrLf)
            End Try
        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreMember WHERE MemberId = " & m_DB.Quote(MemberId)
            Try
                m_DB.ExecuteSQL(SQL)
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & SQL & vbCrLf & vbCrLf)
            End Try
        End Sub 'Remove
    End Class

    Public Class StoreMemberCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreMember As StoreMemberRow)
            Me.List.Add(StoreMember)
        End Sub

        Public Function Contains(ByVal StoreMember As StoreMemberRow) As Boolean
            Return Me.List.Contains(StoreMember)
        End Function

        Public Function IndexOf(ByVal StoreMember As StoreMemberRow) As Integer
            Return Me.List.IndexOf(StoreMember)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreMember As StoreMemberRow)
            Me.List.Insert(Index, StoreMember)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreMemberRow
            Get
                Return CType(Me.List.Item(Index), StoreMemberRow)
            End Get

            Set(ByVal Value As StoreMemberRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreMember As StoreMemberRow)
            Me.List.Remove(StoreMember)
        End Sub
    End Class

End Namespace


