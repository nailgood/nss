Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text

Namespace DataLayer

    Public Class DiscountRow
        Inherits DiscountRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DiscountId As Integer)
            MyBase.New(database, DiscountId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DiscountId As Integer) As DiscountRow
            Dim row As DiscountRow

            row = New DiscountRow(_Database, DiscountId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal DiscountId As Integer)
            Dim row As DiscountRow

            row = New DiscountRow(_Database, DiscountId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetRowByCode(ByVal _DB As Database, ByVal Code As String) As DiscountRow
            Dim row As DiscountRow

            row = New DiscountRow(_DB)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Discount WHERE DiscountCode = " & _DB.Quote(Code)
                r = _DB.GetReader(SQL)
                If r.Read Then
                    row.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
            End Try
            Return row
        End Function

        Public Sub InsertRelatedItem(ByVal ItemId As Integer)
            If ItemId = Nothing Then Exit Sub
            Dim SQL As String = "INSERT INTO DiscountItem (DiscountId, ItemId) Select " & DB.Quote(DiscountId) & ", ItemId FROM StoreItem WHERE ItemId = " & DB.Quote(ItemId)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertRelatedDepartment(ByVal DepartmentId As Integer)
            If DepartmentId = Nothing Then Exit Sub
            Dim SQL As String = "INSERT INTO DiscountItem (DiscountId, DepartmentId) Select " & DB.Quote(DiscountId) & ", P2.DepartmentId FROM StoreDepartment p1, StoreDepartment p2 WHERE P1.lft <= P2.lft AND P1.rgt >= P2.rgt and P1.DepartmentID = " & DB.Quote(DepartmentId) & " AND P2.DepartmentId NOT IN (SELECT DepartmentId FROM DiscountItem WHERE DiscountId=" & DB.Quote(DiscountId) & " and DepartmentId Is not null)"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub RemoveRelatedItem(ByVal _Database As Database, ByVal DiscountItemId As Integer)
            Dim SQL As String = "delete from DiscountItem where DiscountItemId = " & _Database.Quote(DiscountItemId.ToString)
            _Database.ExecuteSQL(SQL)
        End Sub
    End Class

    Public MustInherit Class DiscountRowBase
        Private m_DB As Database
        Private m_DiscountId As Integer = Nothing
        Private m_DiscountCode As String = Nothing
        Private m_AlternateCodes As String = Nothing
        Private m_Type As String = Nothing
        Private m_MethodId As String = Nothing
        Private m_IsFreeStandardShipping As Boolean = Nothing
        Private m_IsFreeUpgradeShipping As Boolean = Nothing
        Private m_FreeUpgrade As String = Nothing
        Private m_Amount As Double = Nothing
        Private m_Limit As Double = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_IsSale As Boolean = Nothing
        Private m_Name As String = Nothing
        Private m_Message As String = Nothing
        Private m_ShippingMessage As String = Nothing
        Private m_QtyLimit As Integer = Nothing
        Private m_QtyUsed As Integer = Nothing
        Private m_StartDate As DateTime = Nothing
        Private m_EndDate As DateTime = Nothing
        Private m_IsItemSpecific As Boolean = Nothing
        Private m_CardType As String = Nothing

        Public Property DiscountId() As Integer
            Get
                Return m_DiscountId
            End Get
            Set(ByVal Value As Integer)
                m_DiscountId = Value
            End Set
        End Property

        Public Property DiscountCode() As String
            Get
                Return m_DiscountCode
            End Get
            Set(ByVal Value As String)
                m_DiscountCode = Value
            End Set
        End Property

        Public Property MethodId() As String
            Get
                Return m_MethodId
            End Get
            Set(ByVal Value As String)
                m_MethodId = Value
            End Set
        End Property

        Public Property AlternateCodes() As String
            Get
                Return m_AlternateCodes
            End Get
            Set(ByVal Value As String)
                m_AlternateCodes = Value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal Value As String)
                m_Type = Value
            End Set
        End Property

        Public Property IsFreeStandardShipping() As Boolean
            Get
                Return m_IsFreeStandardShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeStandardShipping = Value
            End Set
        End Property

        Public Property IsSale() As Boolean
            Get
                Return m_IsSale
            End Get
            Set(ByVal Value As Boolean)
                m_IsSale = Value
            End Set
        End Property

        Public Property IsFreeUpgradeShipping() As Boolean
            Get
                Return m_IsFreeUpgradeShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeUpgradeShipping = Value
            End Set
        End Property

        Public Property FreeUpgrade() As String
            Get
                Return m_FreeUpgrade
            End Get
            Set(ByVal Value As String)
                m_FreeUpgrade = Value
            End Set
        End Property

        Public Property Amount() As Double
            Get
                Return m_Amount
            End Get
            Set(ByVal Value As Double)
                m_Amount = Value
            End Set
        End Property

        Public Property Limit() As Double
            Get
                Return m_Limit
            End Get
            Set(ByVal Value As Double)
                m_Limit = Value
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

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_Message
            End Get
            Set(ByVal Value As String)
                m_Message = Value
            End Set
        End Property

        Public Property ShippingMessage() As String
            Get
                Return m_ShippingMessage
            End Get
            Set(ByVal Value As String)
                m_ShippingMessage = Value
            End Set
        End Property

        Public Property QtyLimit() As Integer
            Get
                Return m_QtyLimit
            End Get
            Set(ByVal Value As Integer)
                m_QtyLimit = Value
            End Set
        End Property

        Public Property QtyUsed() As Integer
            Get
                Return m_QtyUsed
            End Get
            Set(ByVal Value As Integer)
                m_QtyUsed = Value
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

        Public Property IsItemSpecific() As Boolean
            Get
                Return m_IsItemSpecific
            End Get
            Set(ByVal Value As Boolean)
                m_IsItemSpecific = Value
            End Set
        End Property

        Public Property CardType() As String
            Get
                Return m_CardType
            End Get
            Set(ByVal Value As String)
                m_CardType = Value
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

        Public Sub New(ByVal database As Database, ByVal DiscountId As Integer)
            m_DB = database
            m_DiscountId = DiscountId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Discount WHERE DiscountId = " & DB.Quote(DiscountId)
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
            m_DiscountId = Convert.ToInt32(r.Item("DiscountId"))
            If r.Item("DiscountCode") Is Convert.DBNull Then
                m_DiscountCode = Nothing
            Else
                m_DiscountCode = Convert.ToString(r.Item("DiscountCode"))
            End If
            If r.Item("MethodId") Is Convert.DBNull Then
                m_MethodId = Nothing
            Else
                m_MethodId = Convert.ToString(r.Item("MethodId"))
            End If
            If r.Item("AlternateCodes") Is Convert.DBNull Then
                m_AlternateCodes = Nothing
            Else
                m_AlternateCodes = Convert.ToString(r.Item("AlternateCodes"))
            End If
            If r.Item("Type") Is Convert.DBNull Then
                m_Type = Nothing
            Else
                m_Type = Convert.ToString(r.Item("Type"))
            End If
            m_IsFreeStandardShipping = Convert.ToBoolean(r.Item("IsFreeStandardShipping"))
            m_IsFreeUpgradeShipping = Convert.ToBoolean(r.Item("IsFreeUpgradeShipping"))
            If r.Item("FreeUpgrade") Is Convert.DBNull Then
                m_FreeUpgrade = Nothing
            Else
                m_FreeUpgrade = Convert.ToString(r.Item("FreeUpgrade"))
            End If
            If r.Item("Amount") Is Convert.DBNull Then
                m_Amount = Nothing
            Else
                m_Amount = Convert.ToDouble(r.Item("Amount"))
            End If
            If r.Item("Limit") Is Convert.DBNull Then
                m_Limit = Nothing
            Else
                m_Limit = Convert.ToDouble(r.Item("Limit"))
            End If
            m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
            If r.Item("Name") Is Convert.DBNull Then
                m_Name = Nothing
            Else
                m_Name = Convert.ToString(r.Item("Name"))
            End If
            If r.Item("Message") Is Convert.DBNull Then
                m_Message = Nothing
            Else
                m_Message = Convert.ToString(r.Item("Message"))
            End If
            If r.Item("ShippingMessage") Is Convert.DBNull Then
                m_ShippingMessage = Nothing
            Else
                m_ShippingMessage = Convert.ToString(r.Item("ShippingMessage"))
            End If
            m_QtyLimit = Convert.ToInt32(r.Item("QtyLimit"))
            m_QtyUsed = Convert.ToInt32(r.Item("QtyUsed"))
            If r.Item("StartDate") Is Convert.DBNull Then
                m_StartDate = Nothing
            Else
                m_StartDate = Convert.ToDateTime(r.Item("StartDate"))
            End If
            If r.Item("EndDate") Is Convert.DBNull Then
                m_EndDate = Nothing
            Else
                m_EndDate = Convert.ToDateTime(r.Item("EndDate"))
            End If
            m_IsItemSpecific = Convert.ToBoolean(r.Item("IsItemSpecific"))
            m_IsSale = Convert.ToBoolean(r.Item("IsSale"))
            If r.Item("CardType") Is Convert.DBNull Then
                m_CardType = Nothing
            Else
                m_CardType = Convert.ToString(r.Item("CardType"))
            End If
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO Discount (" _
                 & " DiscountCode" _
                 & ",AlternateCodes" _
                 & ",Type" _
                 & ",IsFreeStandardShipping" _
                 & ",IsFreeUpgradeShipping" _
                 & ",FreeUpgrade" _
                 & ",Amount" _
                 & ",Limit" _
                 & ",IsActive" _
                 & ",Name" _
                 & ",Message" _
                 & ",ShippingMessage" _
                 & ",QtyLimit" _
                 & ",QtyUsed" _
                 & ",StartDate" _
                 & ",EndDate" _
                 & ",IsItemSpecific" _
                 & ",IsSale" _
                 & ",MethodId" _
                 & ",CardType" _
                 & ") VALUES (" _
                 & m_DB.Quote(DiscountCode) _
                 & "," & m_DB.Quote(AlternateCodes) _
                 & "," & m_DB.Quote(Type) _
                 & "," & CInt(IsFreeStandardShipping) _
                 & "," & CInt(IsFreeUpgradeShipping) _
                 & "," & m_DB.Quote(FreeUpgrade) _
                 & "," & m_DB.Quote(Amount) _
                 & "," & m_DB.Quote(Limit) _
                 & "," & CInt(IsActive) _
                 & "," & m_DB.Quote(Name) _
                 & "," & m_DB.Quote(Message) _
                 & "," & m_DB.Quote(ShippingMessage) _
                 & "," & m_DB.Quote(QtyLimit) _
                 & "," & m_DB.Quote(QtyUsed) _
                 & "," & m_DB.Quote(StartDate) _
                 & "," & m_DB.Quote(EndDate) _
                 & "," & CInt(IsItemSpecific) _
                 & "," & CInt(IsSale) _
                 & "," & m_DB.Quote(MethodId) _
                 & "," & m_DB.Quote(CardType) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            DiscountId = m_DB.InsertSQL(InsertStatement)
            Return DiscountId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Discount SET " _
             & " DiscountCode = " & m_DB.Quote(DiscountCode) _
             & ",AlternateCodes = " & m_DB.Quote(AlternateCodes) _
             & ",Type = " & m_DB.Quote(Type) _
             & ",IsFreeStandardShipping = " & CInt(IsFreeStandardShipping) _
             & ",IsFreeUpgradeShipping = " & CInt(IsFreeUpgradeShipping) _
             & ",FreeUpgrade = " & m_DB.Quote(FreeUpgrade) _
             & ",Amount = " & m_DB.Quote(Amount) _
             & ",Limit = " & m_DB.Quote(Limit) _
             & ",IsActive = " & CInt(IsActive) _
             & ",Name = " & m_DB.Quote(Name) _
             & ",Message = " & m_DB.Quote(Message) _
             & ",ShippingMessage = " & m_DB.Quote(ShippingMessage) _
             & ",QtyLimit = " & m_DB.Quote(QtyLimit) _
             & ",QtyUsed = " & m_DB.Quote(QtyUsed) _
             & ",StartDate = " & m_DB.Quote(StartDate) _
             & ",EndDate = " & m_DB.Quote(EndDate) _
             & ",IsItemSpecific = " & CInt(IsItemSpecific) _
             & ",IsSale = " & CInt(IsSale) _
             & ",CardType = " & m_DB.Quote(CardType) _
             & ",MethodId = " & m_DB.Quote(MethodId) _
             & " WHERE DiscountId = " & m_DB.Quote(DiscountId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM Discount WHERE DiscountId = " & m_DB.Quote(DiscountId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class DiscountCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Discount As DiscountRow)
            Me.List.Add(Discount)
        End Sub

        Public Function Contains(ByVal Discount As DiscountRow) As Boolean
            Return Me.List.Contains(Discount)
        End Function

        Public Function IndexOf(ByVal Discount As DiscountRow) As Integer
            Return Me.List.IndexOf(Discount)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Discount As DiscountRow)
            Me.List.Insert(Index, Discount)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DiscountRow
            Get
                Return CType(Me.List.Item(Index), DiscountRow)
            End Get

            Set(ByVal Value As DiscountRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Discount As DiscountRow)
            Me.List.Remove(Discount)
        End Sub
    End Class

End Namespace


