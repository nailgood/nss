Option Explicit On 

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components

Namespace DataLayer

    Public Class StoreCatalogRequestRow
        Inherits StoreCatalogRequestRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal RequestId As Integer)
            MyBase.New(database, RequestId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal RequestId As Integer) As StoreCatalogRequestRow
            Dim row As StoreCatalogRequestRow

            row = New StoreCatalogRequestRow(_Database, RequestId)
            row.Load()

            Return row
        End Function
       
        'end 23/10/2009
        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal RequestId As Integer)
            Dim row As StoreCatalogRequestRow

            row = New StoreCatalogRequestRow(_Database, RequestId)
            row.Remove()
        End Sub
        
        'Custom Methods

    End Class

    Public MustInherit Class StoreCatalogRequestRowBase
        Private m_DB As Database
        Private m_RequestId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_Salutation As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_Title As String = Nothing
        Private m_Company As String = Nothing
        Private m_Address1 As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_City As String = Nothing
        Private m_Country As String = Nothing
        Private m_State As String = Nothing
        Private m_Zip As String = Nothing
        Private m_Phone As String = Nothing
        Private m_PhoneExt As String = Nothing
        Private m_Email As String = Nothing
        Private m_HowHeard As String = Nothing
        Private m_DateRequested As DateTime = Nothing
        Private m_Status As String = Nothing
        Private m_EcometryDate As DateTime = Nothing


        Public Property RequestId() As Integer
            Get
                Return m_RequestId
            End Get
            Set(ByVal Value As Integer)
                m_RequestId = value
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

        Public Property Salutation() As String
            Get
                Return m_Salutation
            End Get
            Set(ByVal Value As String)
                m_Salutation = value
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

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = value
            End Set
        End Property

        Public Property Company() As String
            Get
                Return m_Company
            End Get
            Set(ByVal Value As String)
                m_Company = value
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
        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal Value As String)
                m_Country = Value
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

        Public Property PhoneExt() As String
            Get
                Return m_PhoneExt
            End Get
            Set(ByVal Value As String)
                m_PhoneExt = value
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

        Public Property HowHeard() As String
            Get
                Return m_HowHeard
            End Get
            Set(ByVal Value As String)
                m_HowHeard = value
            End Set
        End Property

        Public Property DateRequested() As DateTime
            Get
                Return m_DateRequested
            End Get
            Set(ByVal Value As DateTime)
                m_DateRequested = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal Value As String)
                m_Status = value
            End Set
        End Property

        Public Property EcometryDate() As DateTime
            Get
                Return m_EcometryDate
            End Get
            Set(ByVal Value As DateTime)
                m_EcometryDate = value
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

        Public Sub New(ByVal database As Database, ByVal RequestId As Integer)
            m_DB = database
            m_RequestId = RequestId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreCatalogRequest WHERE RequestId = " & DB.Quote(RequestId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                If r.Item("MemberId") Is Convert.DBNull Then
                    m_MemberId = Nothing
                Else
                    m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                End If
                If r.Item("Salutation") Is Convert.DBNull Then
                    m_Salutation = Nothing
                Else
                    m_Salutation = Convert.ToString(r.Item("Salutation"))
                End If
                If r.Item("FirstName") Is Convert.DBNull Then
                    m_FirstName = Nothing
                Else
                    m_FirstName = Convert.ToString(r.Item("FirstName"))
                End If
                If r.Item("LastName") Is Convert.DBNull Then
                    m_LastName = Nothing
                Else
                    m_LastName = Convert.ToString(r.Item("LastName"))
                End If
                If r.Item("Title") Is Convert.DBNull Then
                    m_Title = Nothing
                Else
                    m_Title = Convert.ToString(r.Item("Title"))
                End If
                If r.Item("Company") Is Convert.DBNull Then
                    m_Company = Nothing
                Else
                    m_Company = Convert.ToString(r.Item("Company"))
                End If
                If r.Item("Address1") Is Convert.DBNull Then
                    m_Address1 = Nothing
                Else
                    m_Address1 = Convert.ToString(r.Item("Address1"))
                End If
                If r.Item("Address2") Is Convert.DBNull Then
                    m_Address2 = Nothing
                Else
                    m_Address2 = Convert.ToString(r.Item("Address2"))
                End If
                If r.Item("City") Is Convert.DBNull Then
                    m_City = Nothing
                Else
                    m_City = Convert.ToString(r.Item("City"))
                End If
                If r.Item("State") Is Convert.DBNull Then
                    m_State = Nothing
                Else
                    m_State = Convert.ToString(r.Item("State"))
                End If
                If r.Item("Country") Is Convert.DBNull Then
                    m_Country = Nothing
                Else
                    m_Country = Convert.ToString(r.Item("Country"))
                End If
                If r.Item("Zip") Is Convert.DBNull Then
                    m_Zip = Nothing
                Else
                    m_Zip = Convert.ToString(r.Item("Zip"))
                End If
                If r.Item("Phone") Is Convert.DBNull Then
                    m_Phone = Nothing
                Else
                    m_Phone = Convert.ToString(r.Item("Phone"))
                End If
                If r.Item("PhoneExt") Is Convert.DBNull Then
                    m_PhoneExt = Nothing
                Else
                    m_PhoneExt = Convert.ToString(r.Item("PhoneExt"))
                End If
                If r.Item("Email") Is Convert.DBNull Then
                    m_Email = Nothing
                Else
                    m_Email = Convert.ToString(r.Item("Email"))
                End If
                If r.Item("HowHeard") Is Convert.DBNull Then
                    m_HowHeard = Nothing
                Else
                    m_HowHeard = Convert.ToString(r.Item("HowHeard"))
                End If
                m_DateRequested = Convert.ToDateTime(r.Item("DateRequested"))
                m_Status = Convert.ToString(r.Item("Status"))
                If r.Item("EcometryDate") Is Convert.DBNull Then
                    m_EcometryDate = Nothing
                Else
                    m_EcometryDate = Convert.ToDateTime(r.Item("EcometryDate"))
                End If
            Catch ex As Exception
                Throw ex
                ''   Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
           
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreCatalogRequest (" _
                 & " MemberId" _
                 & ",Salutation" _
                 & ",FirstName" _
                 & ",LastName" _
                 & ",Title" _
                 & ",Company" _
                 & ",Address1" _
                 & ",Address2" _
                 & ",City" _
                 & ",State" _
                 & ",Country" _
                 & ",Zip" _
                 & ",Phone" _
                 & ",PhoneExt" _
                 & ",Email" _
                 & ",HowHeard" _
                 & ",DateRequested" _
                 & ",Status" _
                 & ",EcometryDate" _
                 & ") VALUES (" _
                 & m_DB.Quote(MemberId) _
                 & "," & m_DB.Quote(Salutation) _
                 & "," & m_DB.Quote(FirstName) _
                 & "," & m_DB.Quote(LastName) _
                 & "," & m_DB.Quote(Title) _
                 & "," & m_DB.Quote(Company) _
                 & "," & m_DB.Quote(Address1) _
                 & "," & m_DB.Quote(Address2) _
                 & "," & m_DB.Quote(City) _
                 & "," & m_DB.Quote(State) _
                 & "," & m_DB.Quote(Country) _
                 & "," & m_DB.Quote(Zip) _
                 & "," & m_DB.Quote(Phone) _
                 & "," & m_DB.Quote(PhoneExt) _
                 & "," & m_DB.Quote(Email) _
                 & "," & m_DB.Quote(HowHeard) _
                 & "," & m_DB.Quote(DateRequested) _
                 & "," & m_DB.Quote(Status) _
                 & "," & m_DB.Quote(EcometryDate) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            Try
                RequestId = m_DB.InsertSQL(InsertStatement)
                Return RequestId
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & InsertStatement & vbCrLf & vbCrLf)
            End Try
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreCatalogRequest SET " _
             & " MemberId = " & m_DB.Quote(MemberId) _
             & ",Salutation = " & m_DB.Quote(Salutation) _
             & ",FirstName = " & m_DB.Quote(FirstName) _
             & ",LastName = " & m_DB.Quote(LastName) _
             & ",Title = " & m_DB.Quote(Title) _
             & ",Company = " & m_DB.Quote(Company) _
             & ",Address1 = " & m_DB.Quote(Address1) _
             & ",Address2 = " & m_DB.Quote(Address2) _
             & ",City = " & m_DB.Quote(City) _
             & ",State = " & m_DB.Quote(State) _
             & ",Country = " & m_DB.Quote(Country) _
             & ",Zip = " & m_DB.Quote(Zip) _
             & ",Phone = " & m_DB.Quote(Phone) _
             & ",PhoneExt = " & m_DB.Quote(PhoneExt) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",HowHeard = " & m_DB.Quote(HowHeard) _
             & ",DateRequested = " & m_DB.Quote(DateRequested) _
             & ",Status = " & m_DB.Quote(Status) _
             & ",EcometryDate = " & m_DB.Quote(EcometryDate) _
             & " WHERE RequestId = " & m_DB.Quote(RequestId)

            Try
                m_DB.ExecuteSQL(SQL)
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & SQL & vbCrLf & vbCrLf)
            End Try
        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreCatalogRequest WHERE RequestId = " & m_DB.Quote(RequestId)
            Try
                m_DB.ExecuteSQL(SQL)
            Catch ex As SqlException
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & "SQL Source: " & SQL & vbCrLf & vbCrLf)
            End Try
        End Sub 'Remove
    End Class

    Public Class StoreCatalogRequestCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreCatalogRequest As StoreCatalogRequestRow)
            Me.List.Add(StoreCatalogRequest)
        End Sub

        Public Function Contains(ByVal StoreCatalogRequest As StoreCatalogRequestRow) As Boolean
            Return Me.List.Contains(StoreCatalogRequest)
        End Function

        Public Function IndexOf(ByVal StoreCatalogRequest As StoreCatalogRequestRow) As Integer
            Return Me.List.IndexOf(StoreCatalogRequest)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreCatalogRequest As StoreCatalogRequestRow)
            Me.List.Insert(Index, StoreCatalogRequest)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreCatalogRequestRow
            Get
                Return CType(Me.List.Item(Index), StoreCatalogRequestRow)
            End Get

            Set(ByVal Value As StoreCatalogRequestRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreCatalogRequest As StoreCatalogRequestRow)
            Me.List.Remove(StoreCatalogRequest)
        End Sub
    End Class

End Namespace


