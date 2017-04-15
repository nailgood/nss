Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Utility
Imports Components
Namespace DataLayer

    Public Class StoreMemberPaymentInfoRow
        Inherits StoreMemberPaymentInfoRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal PaymentId As Integer)
            MyBase.New(database, PaymentId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal PaymentId As Integer) As StoreMemberPaymentInfoRow
            Dim row As StoreMemberPaymentInfoRow

            row = New StoreMemberPaymentInfoRow(_Database, PaymentId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal PaymentId As Integer)
            Dim row As StoreMemberPaymentInfoRow

            row = New StoreMemberPaymentInfoRow(_Database, PaymentId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class StoreMemberPaymentInfoRowBase
        Private m_DB As Database
        Private m_PaymentId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_MethodId As Integer = Nothing
        Private m_Cardholdername As String = Nothing
        Private m_CardTypeId As Integer = Nothing
        Private m_CardNumber As String = Nothing
        Private m_ExpirationMonth As Integer = Nothing
        Private m_ExpirationYear As Integer = Nothing


        Public Property PaymentId() As Integer
            Get
                Return m_PaymentId
            End Get
            Set(ByVal Value As Integer)
                m_PaymentId = value
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

        Public Property MethodId() As Integer
            Get
                Return m_MethodId
            End Get
            Set(ByVal Value As Integer)
                m_MethodId = value
            End Set
        End Property

        Public Property Cardholdername() As String
            Get
                Return m_Cardholdername
            End Get
            Set(ByVal Value As String)
                m_Cardholdername = value
            End Set
        End Property

        Public Property CardTypeId() As Integer
            Get
                Return m_CardTypeId
            End Get
            Set(ByVal Value As Integer)
                m_CardTypeId = value
            End Set
        End Property

        Public Property CardNumber() As String
            Get
                Return m_CardNumber
            End Get
            Set(ByVal Value As String)
                m_CardNumber = value
            End Set
        End Property

        Public ReadOnly Property EncryptedCardNumber() As String
            Get
                Return Crypt.EncryptTripleDes(CardNumber)
            End Get
        End Property

        Public Property ExpirationMonth() As Integer
            Get
                Return m_ExpirationMonth
            End Get
            Set(ByVal Value As Integer)
                m_ExpirationMonth = Value
            End Set
        End Property

        Public Property ExpirationYear() As Integer
            Get
                Return m_ExpirationYear
            End Get
            Set(ByVal Value As Integer)
                m_ExpirationYear = Value
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

        Public Sub New(ByVal database As Database, ByVal PaymentId As Integer)
            m_DB = database
            m_PaymentId = PaymentId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreMemberPaymentInfo WHERE PaymentId = " & DB.Quote(PaymentId)
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
                m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                If r.Item("MethodId") Is Convert.DBNull Then
                    m_MethodId = Nothing
                Else
                    m_MethodId = Convert.ToInt32(r.Item("MethodId"))
                End If
                If r.Item("Cardholdername") Is Convert.DBNull Then
                    m_Cardholdername = Nothing
                Else
                    m_Cardholdername = Convert.ToString(r.Item("Cardholdername"))
                End If
                If r.Item("CardTypeId") Is Convert.DBNull Then
                    m_CardTypeId = Nothing
                Else
                    m_CardTypeId = Convert.ToInt32(r.Item("CardTypeId"))
                End If
                If r.Item("CardNumber") Is Convert.DBNull Then
                    m_CardNumber = Nothing
                Else
                    m_CardNumber = Crypt.DecryptTripleDes(Convert.ToString(r.Item("CardNumber")))
                End If
                If r.Item("ExpirationMonth") Is Convert.DBNull Then
                    m_ExpirationMonth = Nothing
                Else
                    m_ExpirationMonth = Convert.ToInt32(r.Item("ExpirationMonth"))
                End If
                If r.Item("ExpirationYear") Is Convert.DBNull Then
                    m_ExpirationYear = Nothing
                Else
                    m_ExpirationYear = Convert.ToInt32(r.Item("ExpirationYear"))
                End If
            Catch ex As Exception
                Throw ex
                ''Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreMemberPaymentInfo (" _
                 & " MemberId" _
                 & ",MethodId" _
                 & ",Cardholdername" _
                 & ",CardTypeId" _
                 & ",CardNumber" _
                 & ",ExpirationMonth" _
                 & ",ExpirationYear" _
                 & ") VALUES (" _
                 & m_DB.Quote(MemberId) _
                 & "," & m_DB.Quote(MethodId) _
                 & "," & m_DB.Quote(Cardholdername) _
                 & "," & m_DB.Quote(CardTypeId) _
                 & "," & m_DB.Quote(EncryptedCardNumber) _
                 & "," & m_DB.Quote(ExpirationMonth) _
                 & "," & m_DB.Quote(ExpirationYear) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            PaymentId = m_DB.InsertSQL(InsertStatement)
            Return PaymentId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreMemberPaymentInfo SET " _
             & " MemberId = " & m_DB.Quote(MemberId) _
             & ",MethodId = " & m_DB.Quote(MethodId) _
             & ",Cardholdername = " & m_DB.Quote(Cardholdername) _
             & ",CardTypeId = " & m_DB.Quote(CardTypeId) _
             & ",CardNumber = " & m_DB.Quote(EncryptedCardNumber) _
             & ",ExpirationMonth = " & m_DB.Quote(ExpirationMonth) _
             & ",ExpirationYear = " & m_DB.Quote(ExpirationYear) _
             & " WHERE PaymentId = " & m_DB.Quote(PaymentId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreMemberPaymentInfo WHERE PaymentId = " & m_DB.Quote(PaymentId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreMemberPaymentInfoCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreMemberPaymentInfo As StoreMemberPaymentInfoRow)
            Me.List.Add(StoreMemberPaymentInfo)
        End Sub

        Public Function Contains(ByVal StoreMemberPaymentInfo As StoreMemberPaymentInfoRow) As Boolean
            Return Me.List.Contains(StoreMemberPaymentInfo)
        End Function

        Public Function IndexOf(ByVal StoreMemberPaymentInfo As StoreMemberPaymentInfoRow) As Integer
            Return Me.List.IndexOf(StoreMemberPaymentInfo)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreMemberPaymentInfo As StoreMemberPaymentInfoRow)
            Me.List.Insert(Index, StoreMemberPaymentInfo)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreMemberPaymentInfoRow
            Get
                Return CType(Me.List.Item(Index), StoreMemberPaymentInfoRow)
            End Get

            Set(ByVal Value As StoreMemberPaymentInfoRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreMemberPaymentInfo As StoreMemberPaymentInfoRow)
            Me.List.Remove(StoreMemberPaymentInfo)
        End Sub
    End Class
End Namespace