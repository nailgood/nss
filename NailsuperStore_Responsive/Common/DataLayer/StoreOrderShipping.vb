Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text

Namespace DataLayer

    Public Class StoreOrderShippingRow
        Inherits StoreOrderShippingRowBase

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
        Public Shared Function GetRow(ByVal _Database As Database, ByVal AddressId As Integer) As StoreOrderShippingRow
            Dim row As StoreOrderShippingRow

            row = New StoreOrderShippingRow(_Database, AddressId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal AddressId As Integer)
            Dim row As StoreOrderShippingRow

            row = New StoreOrderShippingRow(_Database, AddressId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetListByOrderNo(ByVal _DB As Database, ByVal OrderNo As String) As DataSet
            Dim SQL As String = "select distinct AddressId, NickName from StoreOrderShipping where OrderNo = " & _DB.Quote(OrderNo) & " and NickName <> 'myself' order by NickName"

            Dim ds = _DB.GetDataSet(SQL)
            Return ds
        End Function

        Public Shared Function GetListByOrderNo(ByVal _DB As Database, ByVal OrderNo As String, ByVal RegistryId As Integer) As DataSet
            Dim SQL As String = "select distinct AddressId, NickName from StoreOrderShipping where OrderNo = " & _DB.Quote(OrderNo) & " and NickName <> 'myself' and RegistryId <> " & RegistryId & " order by NickName"

            Dim ds = _DB.GetDataSet(SQL)
            Return ds
        End Function

        Public Shared Function GetAddressIdByNickName(ByVal _DB As Database, ByVal OrderNo As String) As Integer
            Dim SQL As String = "select AddressId from StoreOrderShipping where OrderNo = " & _DB.Quote(OrderNo)
            Dim iResult As Integer = _DB.ExecuteScalar(SQL)

            Return iResult
        End Function

        Public Shared Function GetCollectionByOrderNo(ByVal _DB As Database, ByVal OrderNo As String)
            Dim r As SqlDataReader = Nothing
            Dim collection As New StoreOrderShippingCollection
            Try
                Dim row As StoreOrderShippingRow
                Dim SQL As String = "select * from StoreOrderShipping where OrderNo = " & _DB.Quote(OrderNo)
                r = _DB.GetReader(SQL)
                While r.Read()
                    row = New StoreOrderShippingRow(_DB)
                    row.Load(r)
                    collection.Add(row)
                End While
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return collection
        End Function

    End Class

    Public MustInherit Class StoreOrderShippingRowBase
        Private m_DB As Database
        Private m_AddressId As Integer = Nothing
        Private m_OrderNo As String = Nothing
        Private m_NickName As String = Nothing
        Private m_RegistryId As Integer = Nothing
        Private m_ShippingOption As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_CompanyName As String = Nothing
        Private m_Address1 As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_State As String = Nothing
        Private m_City As String = Nothing
        Private m_Zip As String = Nothing
        Private m_Email As String = Nothing
        Private m_GiftMessageLn1 As String = Nothing
        Private m_GiftMessageLn2 As String = Nothing
        Private m_IsSameAsBilling As Boolean = Nothing
        Private m_IsOffshore As Boolean = Nothing
        Private m_BaseSubtotal As Double = Nothing
        Private m_Discount As Double = Nothing
        Private m_GiftWrapping As Double = Nothing
        Private m_Subtotal As Double = Nothing
        Private m_Shipping As Double = Nothing
        Private m_DeliveryUpgrade As Double = Nothing
        Private m_DeliverySurcharge As Double = Nothing
        Private m_OffshoreShipping As Double = Nothing
        Private m_Tax As Double = Nothing
        Private m_Total As Double = Nothing

        Public Property AddressId() As Integer
            Get
                Return m_AddressId
            End Get
            Set(ByVal Value As Integer)
                m_AddressId = Value
            End Set
        End Property

        Public Property OrderNo() As String
            Get
                Return m_OrderNo
            End Get
            Set(ByVal Value As String)
                m_OrderNo = Value
            End Set
        End Property

        Public Property NickName() As String
            Get
                Return m_NickName
            End Get
            Set(ByVal Value As String)
                m_NickName = Value
            End Set
        End Property

        Public Property RegistryId() As Integer
            Get
                Return m_RegistryId
            End Get
            Set(ByVal Value As Integer)
                m_RegistryId = Value
            End Set
        End Property


        Public Property ShippingOption() As String
            Get
                Return m_ShippingOption
            End Get
            Set(ByVal Value As String)
                m_ShippingOption = Value
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

        Public Property CompanyName() As String
            Get
                Return m_CompanyName
            End Get
            Set(ByVal Value As String)
                m_CompanyName = Value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return m_Address1
            End Get
            Set(ByVal Value As String)
                m_Address1 = Value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return m_Address2
            End Get
            Set(ByVal Value As String)
                m_Address2 = Value
            End Set
        End Property

        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(ByVal Value As String)
                m_State = Value
            End Set
        End Property

        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(ByVal Value As String)
                m_City = Value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Return m_Zip
            End Get
            Set(ByVal Value As String)
                m_Zip = Value
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

        Public Property GiftMessageLn1() As String
            Get
                Return m_GiftMessageLn1
            End Get
            Set(ByVal Value As String)
                m_GiftMessageLn1 = Value
            End Set
        End Property

        Public Property GiftMessageLn2() As String
            Get
                Return m_GiftMessageLn2
            End Get
            Set(ByVal Value As String)
                m_GiftMessageLn2 = Value
            End Set
        End Property

        Public Property IsSameAsBilling() As Boolean
            Get
                Return m_IsSameAsBilling
            End Get
            Set(ByVal Value As Boolean)
                m_IsSameAsBilling = Value
            End Set
        End Property

        Public Property BaseSubtotal() As Double
            Get
                Return m_BaseSubtotal
            End Get
            Set(ByVal Value As Double)
                m_BaseSubtotal = Value
            End Set
        End Property

        Public Property Discount() As Double
            Get
                Return m_Discount
            End Get
            Set(ByVal Value As Double)
                m_Discount = Value
            End Set
        End Property

        Public Property GiftWrapping() As Double
            Get
                Return m_GiftWrapping
            End Get
            Set(ByVal Value As Double)
                m_GiftWrapping = Value
            End Set
        End Property

        Public Property Subtotal() As Double
            Get
                Return m_Subtotal
            End Get
            Set(ByVal Value As Double)
                m_Subtotal = Value
            End Set
        End Property

        Public Property Shipping() As Double
            Get
                Return m_Shipping
            End Get
            Set(ByVal Value As Double)
                m_Shipping = Value
            End Set
        End Property

        Public Property DeliveryUpgrade() As Double
            Get
                Return m_DeliveryUpgrade
            End Get
            Set(ByVal Value As Double)
                m_DeliveryUpgrade = Value
            End Set
        End Property

        Public Property DeliverySurcharge() As Double
            Get
                Return m_DeliverySurcharge
            End Get
            Set(ByVal Value As Double)
                m_DeliverySurcharge = Value
            End Set
        End Property

        Public Property OffshoreShipping() As Double
            Get
                Return m_OffshoreShipping
            End Get
            Set(ByVal Value As Double)
                m_OffshoreShipping = Value
            End Set
        End Property

        Public Property Tax() As Double
            Get
                Return m_Tax
            End Get
            Set(ByVal Value As Double)
                m_Tax = Value
            End Set
        End Property

        Public Property IsOffshore() As Boolean
            Get
                Return m_IsOffshore
            End Get
            Set(ByVal Value As Boolean)
                m_IsOffshore = Value
            End Set
        End Property

        Public Property Total() As Double
            Get
                Return m_Total
            End Get
            Set(ByVal Value As Double)
                m_Total = Value
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

                SQL = "SELECT * FROM StoreOrderShipping WHERE AddressId = " & DB.Quote(AddressId)
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
                m_AddressId = Convert.ToInt32(r.Item("AddressId"))
                m_OrderNo = Convert.ToString(r.Item("OrderNo"))
                m_NickName = Convert.ToString(r.Item("NickName"))
                If r.Item("RegistryId") Is Convert.DBNull Then
                    m_RegistryId = Nothing
                Else
                    m_RegistryId = Convert.ToString(r.Item("RegistryId"))
                End If
                If r.Item("ShippingOption") Is Convert.DBNull Then
                    m_ShippingOption = Nothing
                Else
                    m_ShippingOption = Convert.ToString(r.Item("ShippingOption"))
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
                If r.Item("CompanyName") Is Convert.DBNull Then
                    m_CompanyName = Nothing
                Else
                    m_CompanyName = Convert.ToString(r.Item("CompanyName"))
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
                m_State = Convert.ToString(r.Item("State"))
                If r.Item("City") Is Convert.DBNull Then
                    m_City = Nothing
                Else
                    m_City = Convert.ToString(r.Item("City"))
                End If
                If r.Item("Zip") Is Convert.DBNull Then
                    m_Zip = Nothing
                Else
                    m_Zip = Convert.ToString(r.Item("Zip"))
                End If
                If r.Item("Email") Is Convert.DBNull Then
                    m_Email = Nothing
                Else
                    m_Email = Convert.ToString(r.Item("Email"))
                End If
                If r.Item("GiftMessageLn1") Is Convert.DBNull Then
                    m_GiftMessageLn1 = Nothing
                Else
                    m_GiftMessageLn1 = Convert.ToString(r.Item("GiftMessageLn1"))
                End If
                If r.Item("GiftMessageLn2") Is Convert.DBNull Then
                    m_GiftMessageLn2 = Nothing
                Else
                    m_GiftMessageLn2 = Convert.ToString(r.Item("GiftMessageLn2"))
                End If
                m_IsSameAsBilling = Convert.ToBoolean(r.Item("IsSameAsBilling"))
                m_IsOffshore = Convert.ToBoolean(r.Item("IsOffshore"))
                m_BaseSubtotal = Convert.ToDouble(r.Item("BaseSubtotal"))
                m_Discount = Convert.ToDouble(r.Item("Discount"))
                m_GiftWrapping = Convert.ToDouble(r.Item("GiftWrapping"))
                m_Subtotal = Convert.ToDouble(r.Item("Subtotal"))
                If Not r.Item("Shipping") Is Convert.DBNull Then
                    m_Shipping = Convert.ToDouble(r.Item("Shipping"))
                End If
                If Not r.Item("DeliveryUpgrade") Is Convert.DBNull Then
                    m_DeliveryUpgrade = Convert.ToDouble(r.Item("DeliveryUpgrade"))
                End If
                If Not r.Item("DeliverySurcharge") Is Convert.DBNull Then
                    m_DeliverySurcharge = Convert.ToDouble(r.Item("DeliverySurcharge"))
                End If
                If Not r.Item("OffshoreShipping") Is Convert.DBNull Then
                    m_OffshoreShipping = Convert.ToDouble(r.Item("OffshoreShipping"))
                End If
                If r.Item("Tax") Is Convert.DBNull Then
                    m_Tax = Nothing
                Else
                    m_Tax = Convert.ToDouble(r.Item("Tax"))
                End If
                If r.Item("Total") Is Convert.DBNull Then
                    m_Total = Nothing
                Else
                    m_Total = Convert.ToDouble(r.Item("Total"))
                End If
            Catch ex As Exception
                Throw ex
                '' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreOrderShipping (" _
                 & " OrderNo" _
                 & ",NickName" _
                 & ",RegistryId" _
                 & ",ShippingOption" _
                 & ",FirstName" _
                 & ",LastName" _
                 & ",CompanyName" _
                 & ",Address1" _
                 & ",Address2" _
                 & ",State" _
                 & ",City" _
                 & ",Zip" _
                 & ",Email" _
                 & ",GiftMessageLn1" _
                 & ",GiftMessageLn2" _
                 & ",IsSameAsBilling" _
                 & ",BaseSubtotal" _
                 & ",Discount" _
                 & ",GiftWrapping" _
                 & ",Subtotal" _
                 & ",Shipping" _
                 & ",DeliveryUpgrade" _
                 & ",DeliverySurcharge" _
                 & ",OffshoreShipping" _
                 & ",Tax" _
                 & ",Total" _
                 & ") VALUES (" _
                 & m_DB.Quote(OrderNo) _
                 & "," & m_DB.Quote(NickName) _
                 & "," & m_DB.NullQuote(RegistryId) _
                 & "," & m_DB.Quote(ShippingOption) _
                 & "," & m_DB.Quote(FirstName) _
                 & "," & m_DB.Quote(LastName) _
                 & "," & m_DB.Quote(CompanyName) _
                 & "," & m_DB.Quote(Address1) _
                 & "," & m_DB.Quote(Address2) _
                 & "," & m_DB.Quote(State) _
                 & "," & m_DB.Quote(City) _
                 & "," & m_DB.Quote(Zip) _
                 & "," & m_DB.Quote(Email) _
                 & "," & m_DB.Quote(GiftMessageLn1) _
                 & "," & m_DB.Quote(GiftMessageLn2) _
                 & "," & CInt(IsSameAsBilling) _
                 & "," & m_DB.Quote(BaseSubtotal) _
                 & "," & m_DB.Quote(Discount) _
                 & "," & m_DB.Quote(GiftWrapping) _
                 & "," & m_DB.Quote(Subtotal) _
                 & "," & m_DB.Quote(Shipping) _
                 & "," & m_DB.Quote(DeliveryUpgrade) _
                 & "," & m_DB.Quote(DeliverySurcharge) _
                 & "," & m_DB.Quote(OffshoreShipping) _
                 & "," & m_DB.Quote(Tax) _
                 & "," & m_DB.Quote(Total) _
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

            SQL = " UPDATE StoreOrderShipping WITH (ROWLOCK) SET " _
             & " OrderNo = " & m_DB.Quote(OrderNo) _
             & ",NickName = " & m_DB.Quote(NickName) _
             & ",RegistryId = " & m_DB.NullQuote(RegistryId) _
             & ",ShippingOption = " & m_DB.Quote(ShippingOption) _
             & ",FirstName = " & m_DB.Quote(FirstName) _
             & ",LastName = " & m_DB.Quote(LastName) _
             & ",CompanyName = " & m_DB.Quote(CompanyName) _
             & ",Address1 = " & m_DB.Quote(Address1) _
             & ",Address2 = " & m_DB.Quote(Address2) _
             & ",State = " & m_DB.Quote(State) _
             & ",City = " & m_DB.Quote(City) _
             & ",Zip = " & m_DB.Quote(Zip) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",GiftMessageLn1 = " & m_DB.Quote(GiftMessageLn1) _
             & ",GiftMessageLn2 = " & m_DB.Quote(GiftMessageLn2) _
             & ",IsSameAsBilling = " & CInt(IsSameAsBilling) _
             & ",BaseSubtotal = " & m_DB.Quote(BaseSubtotal) _
             & ",Discount = " & m_DB.Quote(Discount) _
             & ",GiftWrapping = " & m_DB.Quote(GiftWrapping) _
             & ",Subtotal = " & m_DB.Quote(Subtotal) _
             & ",Shipping = " & m_DB.Quote(Shipping) _
             & ",DeliveryUpgrade = " & m_DB.Quote(DeliveryUpgrade) _
             & ",DeliverySurcharge = " & m_DB.Quote(DeliverySurcharge) _
             & ",OffshoreShipping = " & m_DB.Quote(OffshoreShipping) _
             & ",Tax = " & m_DB.Quote(Tax) _
             & ",Total = " & m_DB.Quote(Total) _
             & " WHERE AddressId = " & m_DB.Quote(AddressId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreOrderShipping WHERE AddressId = " & m_DB.Quote(AddressId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreOrderShippingCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreOrderShipping As StoreOrderShippingRow)
            Me.List.Add(StoreOrderShipping)
        End Sub

        Public Function Contains(ByVal StoreOrderShipping As StoreOrderShippingRow) As Boolean
            Return Me.List.Contains(StoreOrderShipping)
        End Function

        Public Function IndexOf(ByVal StoreOrderShipping As StoreOrderShippingRow) As Integer
            Return Me.List.IndexOf(StoreOrderShipping)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreOrderShipping As StoreOrderShippingRow)
            Me.List.Insert(Index, StoreOrderShipping)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreOrderShippingRow
            Get
                Return CType(Me.List.Item(Index), StoreOrderShippingRow)
            End Get

            Set(ByVal Value As StoreOrderShippingRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreOrderShipping As StoreOrderShippingRow)
            Me.List.Remove(StoreOrderShipping)
        End Sub
    End Class

End Namespace


