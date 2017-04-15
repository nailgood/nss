Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text

Namespace DataLayer
    Public Class StoreItemInspiredRow
        Inherits StoreItemInspiredRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal InspiredId As Integer)
            MyBase.New(database, InspiredId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal InspiredId As Integer) As StoreItemInspiredRow
            Dim row As StoreItemInspiredRow

            row = New StoreItemInspiredRow(_Database, InspiredId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal InspiredId As Integer)
            Dim row As StoreItemInspiredRow

            row = New StoreItemInspiredRow(_Database, InspiredId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class StoreItemInspiredRowBase
        Private m_DB As Database
        Private m_InspiredId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_TitleImage As String = Nothing
        Private m_StartDate As DateTime = Nothing
        Private m_EndDate As DateTime = Nothing
        Private m_IsBilling As Boolean = Nothing
        Private m_IsBag As Boolean = Nothing
        Private m_IsDelivery As Boolean = Nothing
        Private m_IsPayment As Boolean = Nothing
        Private m_IsQuickShop As Boolean = Nothing
        Private m_IsAddressBook As Boolean = Nothing
        Private m_IsPaymentInfo As Boolean = Nothing
        Private m_IsReminderEdit As Boolean = Nothing
        Private m_IsReminderConfirm As Boolean = Nothing
        Private m_IsEmptyBag As Boolean = Nothing
        Private m_IsProfileEdit As Boolean = Nothing

        Public Property InspiredId() As Integer
            Get
                Return m_InspiredId
            End Get
            Set(ByVal Value As Integer)
                m_InspiredId = Value
            End Set
        End Property

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Property TitleImage() As String
            Get
                Return m_TitleImage
            End Get
            Set(ByVal Value As String)
                m_TitleImage = Value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return m_StartDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartDate = Value
            End Set
        End Property

        Public Property EndDate() As DateTime
            Get
                Return m_EndDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndDate = Value
            End Set
        End Property

        Public Property IsBilling() As Boolean
            Get
                Return m_IsBilling
            End Get
            Set(ByVal Value As Boolean)
                m_IsBilling = Value
            End Set
        End Property

        Public Property IsBag() As Boolean
            Get
                Return m_IsBag
            End Get
            Set(ByVal Value As Boolean)
                m_IsBag = Value
            End Set
        End Property

        Public Property IsAddressBook() As Boolean
            Get
                Return m_IsAddressBook
            End Get
            Set(ByVal Value As Boolean)
                m_IsAddressBook = Value
            End Set
        End Property

        Public Property IsReminderEdit() As Boolean
            Get
                Return m_IsReminderEdit
            End Get
            Set(ByVal Value As Boolean)
                m_IsReminderEdit = Value
            End Set
        End Property

        Public Property IsReminderConfirm() As Boolean
            Get
                Return m_IsReminderConfirm
            End Get
            Set(ByVal Value As Boolean)
                m_IsReminderConfirm = Value
            End Set
        End Property

        Public Property IsEmptyBag() As Boolean
            Get
                Return m_IsEmptyBag
            End Get
            Set(ByVal Value As Boolean)
                m_IsEmptyBag = Value
            End Set
        End Property

        Public Property IsProfileEdit() As Boolean
            Get
                Return m_IsProfileEdit
            End Get
            Set(ByVal Value As Boolean)
                m_IsProfileEdit = Value
            End Set
        End Property

        Public Property IsPaymentInfo() As Boolean
            Get
                Return m_IsPaymentInfo
            End Get
            Set(ByVal Value As Boolean)
                m_IsPaymentInfo = Value
            End Set
        End Property

        Public Property IsDelivery() As Boolean
            Get
                Return m_IsDelivery
            End Get
            Set(ByVal Value As Boolean)
                m_IsDelivery = Value
            End Set
        End Property

        Public Property IsPayment() As Boolean
            Get
                Return m_IsPayment
            End Get
            Set(ByVal Value As Boolean)
                m_IsPayment = Value
            End Set
        End Property

        Public Property IsQuickShop() As Boolean
            Get
                Return m_IsQuickShop
            End Get
            Set(ByVal Value As Boolean)
                m_IsQuickShop = Value
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

        Public Sub New(ByVal database As Database, ByVal InspiredId As Integer)
            m_DB = database
            m_InspiredId = InspiredId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreItemInspired WHERE InspiredId = " & DB.Quote(InspiredId)
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
                m_ItemId = Convert.ToInt32(r.Item("ItemId"))
                m_StartDate = Convert.ToDateTime(r.Item("StartDate"))
                m_EndDate = Convert.ToDateTime(r.Item("EndDate"))
                m_IsBilling = Convert.ToBoolean(r.Item("IsBilling"))
                m_IsBag = Convert.ToBoolean(r.Item("IsBag"))
                m_IsDelivery = Convert.ToBoolean(r.Item("IsDelivery"))
                If r.Item("TitleImage") Is Convert.DBNull Then m_TitleImage = Nothing Else m_TitleImage = Convert.ToString(r.Item("TitleImage"))
                m_IsPayment = Convert.ToBoolean(r.Item("IsPayment"))
                m_IsQuickShop = Convert.ToBoolean(r.Item("IsQuickShop"))
                m_IsAddressBook = Convert.ToBoolean(r.Item("IsAddressBook"))
                m_IsPaymentInfo = Convert.ToBoolean(r.Item("IsPaymentInfo"))
                m_IsReminderEdit = Convert.ToBoolean(r.Item("IsReminderEdit"))
                m_IsReminderConfirm = Convert.ToBoolean(r.Item("IsReminderConfirm"))
                m_IsEmptyBag = Convert.ToBoolean(r.Item("IsEmptyBag"))
                m_IsProfileEdit = Convert.ToBoolean(r.Item("IsProfileEdit"))
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreItemInspired (" _
                 & " ItemId" _
                 & ",StartDate" _
                 & ",EndDate" _
                 & ",TitleImage" _
                 & ",IsBilling" _
                 & ",IsBag" _
                 & ",IsDelivery" _
                 & ",IsPayment" _
                 & ",IsQuickShop" _
                 & ",IsAddressBook" _
                 & ",IsPaymentInfo" _
                 & ",IsReminderEdit" _
                 & ",IsReminderConfirm" _
                 & ",IsEmptyBag" _
                 & ",IsProfileEdit" _
                 & ") VALUES (" _
                 & m_DB.Quote(ItemId) _
                 & "," & m_DB.Quote(StartDate) _
                 & "," & m_DB.Quote(EndDate) _
                 & "," & m_DB.Quote(TitleImage) _
                 & "," & IIf(IsBilling = True, "1", "0") _
                 & "," & IIf(IsBag = True, "1", "0") _
                 & "," & IIf(IsDelivery = True, "1", "0") _
                 & "," & IIf(IsPayment = True, "1", "0") _
                 & "," & IIf(IsQuickShop = True, "1", "0") _
                 & "," & IIf(IsAddressBook = True, "1", "0") _
                 & "," & IIf(IsPaymentInfo = True, "1", "0") _
                 & "," & IIf(IsReminderEdit = True, "1", "0") _
                 & "," & IIf(IsReminderConfirm = True, "1", "0") _
                 & "," & IIf(IsEmptyBag = True, "1", "0") _
                 & "," & IIf(IsProfileEdit = True, "1", "0") _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            InspiredId = m_DB.InsertSQL(InsertStatement)
            Return InspiredId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreItemInspired SET " _
             & " ItemId = " & m_DB.Quote(ItemId) _
             & ",StartDate = " & m_DB.Quote(StartDate) _
             & ",EndDate = " & m_DB.Quote(EndDate) _
             & ",TitleImage = " & m_DB.Quote(TitleImage) _
             & ",IsBilling = " & IIf(IsBilling = True, "1", "0") _
             & ",IsBag = " & IIf(IsBag = True, "1", "0") _
             & ",IsDelivery = " & IIf(IsDelivery = True, "1", "0") _
             & ",IsPayment = " & IIf(IsPayment = True, "1", "0") _
             & ",IsQuickShop = " & IIf(IsQuickShop = True, "1", "0") _
             & ",IsAddressBook = " & IIf(IsAddressBook = True, "1", "0") _
             & ",IsPaymentInfo = " & IIf(IsPaymentInfo = True, "1", "0") _
             & ",IsReminderEdit = " & IIf(IsReminderEdit = True, "1", "0") _
             & ",IsReminderConfirm = " & IIf(IsReminderConfirm = True, "1", "0") _
             & ",IsEmptyBag = " & IIf(IsEmptyBag = True, "1", "0") _
             & ",IsProfileEdit = " & IIf(IsProfileEdit = True, "1", "0") _
             & " WHERE InspiredId = " & m_DB.Quote(InspiredId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreItemInspired WHERE InspiredId = " & m_DB.Quote(InspiredId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreItemInspiredCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreItemInspired As StoreItemInspiredRow)
            Me.List.Add(StoreItemInspired)
        End Sub

        Public Function Contains(ByVal StoreItemInspired As StoreItemInspiredRow) As Boolean
            Return Me.List.Contains(StoreItemInspired)
        End Function

        Public Function IndexOf(ByVal StoreItemInspired As StoreItemInspiredRow) As Integer
            Return Me.List.IndexOf(StoreItemInspired)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreItemInspired As StoreItemInspiredRow)
            Me.List.Insert(Index, StoreItemInspired)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreItemInspiredRow
            Get
                Return CType(Me.List.Item(Index), StoreItemInspiredRow)
            End Get

            Set(ByVal Value As StoreItemInspiredRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreItemInspired As StoreItemInspiredRow)
            Me.List.Remove(StoreItemInspired)
        End Sub
    End Class
End Namespace