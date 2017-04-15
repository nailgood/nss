Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class StoreOrderShipmentLineRow
        Inherits StoreOrderShipmentLineRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ShipmentLineId As Integer)
            MyBase.New(DB, ShipmentLineId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal LineNo As Integer)
            MyBase.New(DB, ShipmentId, LineNo)
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal OrderShipmentLineNo As String)
            MyBase.New(DB, ShipmentId, OrderShipmentLineNo)
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal OrderShipmentLineNo As String, ByVal CartItemId As Integer)
            MyBase.New(DB, ShipmentId, OrderShipmentLineNo, CartItemId)
        End Sub 'New
        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ShipmentLineId As Integer) As StoreOrderShipmentLineRow
            Dim row As StoreOrderShipmentLineRow

            row = New StoreOrderShipmentLineRow(DB, ShipmentLineId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal LineNo As Integer) As StoreOrderShipmentLineRow
            Dim row As StoreOrderShipmentLineRow

            row = New StoreOrderShipmentLineRow(DB, ShipmentId, LineNo)
            row.Load()

            Return row
        End Function
        Public Shared Function GetRow(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal OrderShipmentLineNo As String) As StoreOrderShipmentLineRow
            Dim row As StoreOrderShipmentLineRow

            row = New StoreOrderShipmentLineRow(DB, ShipmentId, OrderShipmentLineNo)
            row.LoadBySku()

            Return row
        End Function
        'Public Shared Function GetRow(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal OrderShipmentLineNo As String, ByVal CartItemId As Integer) As StoreOrderShipmentLineRow
        '    Dim row As StoreOrderShipmentLineRow

        '    row = New StoreOrderShipmentLineRow(DB, ShipmentId, OrderShipmentLineNo, CartItemId)
        '    row.LoadByCartItemId_Sassi()

        '    Return row
        'End Function
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ShipmentLineId As Integer)
            Dim row As StoreOrderShipmentLineRow

            row = New StoreOrderShipmentLineRow(DB, ShipmentLineId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionOrderShipmentLinesRow)
            OrderId = DB.ExecuteScalar("select top 1 OrderId from StoreOrderShipment where ShipmentNo = " & DB.Quote(r.Document_No))
            If OrderId = Nothing Then Exit Sub

            ShipmentId = DB.ExecuteScalar("select top 1 ShipmentId from StoreOrderShipment where ShipmentNo = " & DB.Quote(r.Document_No))
            If ShipmentId = Nothing Then Exit Sub

            CartItemId = DB.ExecuteScalar("select top 1 CartItemId from StoreCartItem where OrderId = " & DB.Quote(OrderId) & " and CartItemId = " & DB.Quote(r.Web_Reference_No))
            If CartItemId = Nothing Then Exit Sub

            Dim dbShipment As StoreOrderShipmentRow = StoreOrderShipmentRow.GetRow(DB, ShipmentId)

            SellToCustomerId = DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(r.Sell_to_Customer_No))
            If r.Bill_to_Customer_No = r.Sell_to_Customer_No Then BillToCustomerId = SellToCustomerId Else BillToCustomerId = DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(r.Bill_to_Customer_No))
            'ignore error
            If SellToCustomerId = Nothing OrElse BillToCustomerId = Nothing Then Exit Sub

            ShipmentLineId = DB.ExecuteScalar("select top 1 ShipmentLineId from storeordershipmentline where orderid = " & OrderId & " and cartitemid = " & CartItemId)

            [LineNo] = r.Line_No
            Type = r.Type
            OrderShipmentLineNo = r.Order_Shipment_Line_No
            LocationCode = r.Location_Code
            ShipmentDate = r.Shipment_Date
            Description = r.Description
            Description2 = r.Description_2
            UnitOfMeasure = r.Unit_of_Measure
            Quantity = r.Quantity
            UnitPrice = r.Unit_Price
            VATPercent = r.VAT_Percent
            LineDiscountPercent = r.Line_Discount_Percent
            AllowInvoiceDisc = r.Allow_Invoice_Disc
            GrossWeight = r.Gross_Weight
            NetWeight = r.Net_Weight
            UnitsPerParcel = r.Units_per_Parcel
            UnitVolume = r.Unit_Volume
            ItemShptEntryNo = r.Item_Shpt_Entry_No
            CustomerPriceGroup = r.Customer_Price_Group
            QtyShippedNotInvoiced = r.Qty_Shipped_Not_Invoiced
            QuantityInvoiced = r.Quantity_Invoiced
            PurchaseOrderNo = r.Purchase_Order_No
            PurchOrderLineNo = r.Purch_Order_Line_No
            DropShipment = r.Drop_Shipment
            CurrencyCode = r.Currency_Code
            VariantCode = r.Variant_Code
            BinCode = r.Bin_Code
            QtyPerUnitOfMeasure = r.Qty_per_Unit_of_Measure
            UnitOfMeasureCode = r.Unit_of_Measure_Code
            QuantityBase = r.Quantity_Base
            QtyInvoicedBase = r.Qty_Invoiced_Base
            ResponsibilityCenter = r.Responsibility_Center
            CrossReferenceNo = r.Cross_Reference_No
            UnitOfMeasureCrossRef = r.Unit_of_Measure_Cross_Ref
            CrossReferenceType = r.Cross_Reference_Type
            CrossReferenceTypeNo = r.Cross_Reference_Type_No
            ItemCategoryCode = r.Item_Category_Code
            Nonstock = r.Nonstock
            PurchasingCode = r.Purchasing_Code
            ProductGroupCode = r.Product_Group_Code
            RequestedDeliveryDate = r.Requested_Delivery_Date
            PromisedDeliveryDate = r.Promised_Delivery_Date
            ShippingTime = r.Shipping_Time
            OutboundWhseHandlingTime = r.Outbound_Whse_Handling_Time
            PlannedDeliveryDate = r.Planned_Delivery_Date
            PlannedShipmentDate = r.Planned_Shipment_Date
            AllowLineDisc = r.Allow_Line_Disc
            CustomerDiscGroup = r.Customer_Disc_Group
            Dim OrigPackageTrackingNo As String = PackageTrackingNo
            PackageTrackingNo = r.Package_Tracking_No
            WebOrderQty = r.Web_Order_Qty
            TotalQtyShipped = r.Total_Qty_Shipped

            If DB.ExecuteSQL("update storecartitem set quantity = " & DB.Number(WebOrderQty) & " where cartitemid = " & CartItemId & " and quantity <> " & DB.Number(WebOrderQty)) > 0 Then
            End If

            If ShipmentLineId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub

    End Class

    Public MustInherit Class StoreOrderShipmentLineRowBase
        Private m_DB As Database
        Private m_ShipmentLineId As Integer = Nothing
        Private m_SellToCustomerId As Integer = Nothing
        Private m_ShipmentId As Integer = Nothing
        Private m_LineNo As Integer = Nothing
        Private m_Type As String = Nothing
        Private m_OrderShipmentLineNo As String = Nothing
        Private m_LocationCode As String = Nothing
        Private m_ShipmentDate As DateTime = Nothing
        Private m_Description As String = Nothing
        Private m_Description2 As String = Nothing
        Private m_UnitOfMeasure As String = Nothing
        Private m_Quantity As Double = Nothing
        Private m_UnitPrice As Double = Nothing
        Private m_VATPercent As Double = Nothing
        Private m_LineDiscountPercent As Double = Nothing
        Private m_AllowInvoiceDisc As String = Nothing
        Private m_GrossWeight As Double = Nothing
        Private m_NetWeight As Double = Nothing
        Private m_UnitsPerParcel As Double = Nothing
        Private m_UnitVolume As Double = Nothing
        Private m_ItemShptEntryNo As Integer = Nothing
        Private m_CustomerPriceGroup As String = Nothing
        Private m_QtyShippedNotInvoiced As Double = Nothing
        Private m_QuantityInvoiced As Double = Nothing
        Private m_OrderId As Integer = Nothing
        Private m_CartItemId As Integer = Nothing
        Private m_BillToCustomerId As Integer = Nothing
        Private m_PurchaseOrderNo As String = Nothing
        Private m_PurchOrderLineNo As Integer = Nothing
        Private m_DropShipment As String = Nothing
        Private m_CurrencyCode As String = Nothing
        Private m_VariantCode As String = Nothing
        Private m_BinCode As String = Nothing
        Private m_QtyPerUnitOfMeasure As Double = Nothing
        Private m_UnitOfMeasureCode As String = Nothing
        Private m_QuantityBase As Double = Nothing
        Private m_QtyInvoicedBase As Double = Nothing
        Private m_ResponsibilityCenter As String = Nothing
        Private m_CrossReferenceNo As String = Nothing
        Private m_UnitOfMeasureCrossRef As String = Nothing
        Private m_CrossReferenceType As String = Nothing
        Private m_CrossReferenceTypeNo As String = Nothing
        Private m_ItemCategoryCode As String = Nothing
        Private m_Nonstock As String = Nothing
        Private m_PurchasingCode As String = Nothing
        Private m_ProductGroupCode As String = Nothing
        Private m_RequestedDeliveryDate As DateTime = Nothing
        Private m_PromisedDeliveryDate As DateTime = Nothing
        Private m_ShippingTime As String = Nothing
        Private m_OutboundWhseHandlingTime As String = Nothing
        Private m_PlannedDeliveryDate As DateTime = Nothing
        Private m_PlannedShipmentDate As DateTime = Nothing
        Private m_AllowLineDisc As String = Nothing
        Private m_CustomerDiscGroup As String = Nothing
        Private m_PackageTrackingNo As String = Nothing
        Private m_WebOrderQty As Integer = Nothing
        Private m_TotalQtyShipped As Integer = Nothing

        Public Property WebOrderQty() As Integer
            Get
                Return m_WebOrderQty
            End Get
            Set(ByVal value As Integer)
                m_WebOrderQty = value
            End Set
        End Property

        Public Property TotalQtyShipped() As Integer
            Get
                Return m_TotalQtyShipped
            End Get
            Set(ByVal value As Integer)
                m_TotalQtyShipped = value
            End Set
        End Property

        Public Property ShipmentLineId() As Integer
            Get
                Return m_ShipmentLineId
            End Get
            Set(ByVal Value As Integer)
                m_ShipmentLineId = Value
            End Set
        End Property

        Public Property SellToCustomerId() As Integer
            Get
                Return m_SellToCustomerId
            End Get
            Set(ByVal Value As Integer)
                m_SellToCustomerId = Value
            End Set
        End Property

        Public Property ShipmentId() As Integer
            Get
                Return m_ShipmentId
            End Get
            Set(ByVal Value As Integer)
                m_ShipmentId = Value
            End Set
        End Property

        Public Property LineNo() As Integer
            Get
                Return m_LineNo
            End Get
            Set(ByVal Value As Integer)
                m_LineNo = Value
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

        Public Property OrderShipmentLineNo() As String
            Get
                Return m_OrderShipmentLineNo
            End Get
            Set(ByVal Value As String)
                m_OrderShipmentLineNo = Value
            End Set
        End Property

        Public Property LocationCode() As String
            Get
                Return m_LocationCode
            End Get
            Set(ByVal Value As String)
                m_LocationCode = Value
            End Set
        End Property

        Public Property ShipmentDate() As DateTime
            Get
                Return m_ShipmentDate
            End Get
            Set(ByVal Value As DateTime)
                m_ShipmentDate = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
            End Set
        End Property

        Public Property Description2() As String
            Get
                Return m_Description2
            End Get
            Set(ByVal Value As String)
                m_Description2 = Value
            End Set
        End Property

        Public Property UnitOfMeasure() As String
            Get
                Return m_UnitOfMeasure
            End Get
            Set(ByVal Value As String)
                m_UnitOfMeasure = Value
            End Set
        End Property

        Public Property Quantity() As Double
            Get
                Return m_Quantity
            End Get
            Set(ByVal Value As Double)
                m_Quantity = Value
            End Set
        End Property

        Public Property UnitPrice() As Double
            Get
                Return m_UnitPrice
            End Get
            Set(ByVal Value As Double)
                m_UnitPrice = Value
            End Set
        End Property

        Public Property VATPercent() As Double
            Get
                Return m_VATPercent
            End Get
            Set(ByVal Value As Double)
                m_VATPercent = Value
            End Set
        End Property

        Public Property LineDiscountPercent() As Double
            Get
                Return m_LineDiscountPercent
            End Get
            Set(ByVal Value As Double)
                m_LineDiscountPercent = Value
            End Set
        End Property

        Public Property AllowInvoiceDisc() As String
            Get
                Return m_AllowInvoiceDisc
            End Get
            Set(ByVal Value As String)
                m_AllowInvoiceDisc = Value
            End Set
        End Property

        Public Property GrossWeight() As Double
            Get
                Return m_GrossWeight
            End Get
            Set(ByVal Value As Double)
                m_GrossWeight = Value
            End Set
        End Property

        Public Property NetWeight() As Double
            Get
                Return m_NetWeight
            End Get
            Set(ByVal Value As Double)
                m_NetWeight = Value
            End Set
        End Property

        Public Property UnitsPerParcel() As Double
            Get
                Return m_UnitsPerParcel
            End Get
            Set(ByVal Value As Double)
                m_UnitsPerParcel = Value
            End Set
        End Property

        Public Property UnitVolume() As Double
            Get
                Return m_UnitVolume
            End Get
            Set(ByVal Value As Double)
                m_UnitVolume = Value
            End Set
        End Property

        Public Property ItemShptEntryNo() As Integer
            Get
                Return m_ItemShptEntryNo
            End Get
            Set(ByVal Value As Integer)
                m_ItemShptEntryNo = Value
            End Set
        End Property

        Public Property CustomerPriceGroup() As String
            Get
                Return m_CustomerPriceGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerPriceGroup = Value
            End Set
        End Property

        Public Property QtyShippedNotInvoiced() As Double
            Get
                Return m_QtyShippedNotInvoiced
            End Get
            Set(ByVal Value As Double)
                m_QtyShippedNotInvoiced = Value
            End Set
        End Property

        Public Property QuantityInvoiced() As Double
            Get
                Return m_QuantityInvoiced
            End Get
            Set(ByVal Value As Double)
                m_QuantityInvoiced = Value
            End Set
        End Property

        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property

        Public Property CartItemId() As Integer
            Get
                Return m_CartItemId
            End Get
            Set(ByVal Value As Integer)
                m_CartItemId = Value
            End Set
        End Property

        Public Property BillToCustomerId() As Integer
            Get
                Return m_BillToCustomerId
            End Get
            Set(ByVal Value As Integer)
                m_BillToCustomerId = Value
            End Set
        End Property

        Public Property PurchaseOrderNo() As String
            Get
                Return m_PurchaseOrderNo
            End Get
            Set(ByVal Value As String)
                m_PurchaseOrderNo = Value
            End Set
        End Property

        Public Property PurchOrderLineNo() As Integer
            Get
                Return m_PurchOrderLineNo
            End Get
            Set(ByVal Value As Integer)
                m_PurchOrderLineNo = Value
            End Set
        End Property

        Public Property DropShipment() As String
            Get
                Return m_DropShipment
            End Get
            Set(ByVal Value As String)
                m_DropShipment = Value
            End Set
        End Property

        Public Property CurrencyCode() As String
            Get
                Return m_CurrencyCode
            End Get
            Set(ByVal Value As String)
                m_CurrencyCode = Value
            End Set
        End Property

        Public Property VariantCode() As String
            Get
                Return m_VariantCode
            End Get
            Set(ByVal Value As String)
                m_VariantCode = Value
            End Set
        End Property

        Public Property BinCode() As String
            Get
                Return m_BinCode
            End Get
            Set(ByVal Value As String)
                m_BinCode = Value
            End Set
        End Property

        Public Property QtyPerUnitOfMeasure() As Double
            Get
                Return m_QtyPerUnitOfMeasure
            End Get
            Set(ByVal Value As Double)
                m_QtyPerUnitOfMeasure = Value
            End Set
        End Property

        Public Property UnitOfMeasureCode() As String
            Get
                Return m_UnitOfMeasureCode
            End Get
            Set(ByVal Value As String)
                m_UnitOfMeasureCode = Value
            End Set
        End Property

        Public Property QuantityBase() As Double
            Get
                Return m_QuantityBase
            End Get
            Set(ByVal Value As Double)
                m_QuantityBase = Value
            End Set
        End Property

        Public Property QtyInvoicedBase() As Double
            Get
                Return m_QtyInvoicedBase
            End Get
            Set(ByVal Value As Double)
                m_QtyInvoicedBase = Value
            End Set
        End Property

        Public Property ResponsibilityCenter() As String
            Get
                Return m_ResponsibilityCenter
            End Get
            Set(ByVal Value As String)
                m_ResponsibilityCenter = Value
            End Set
        End Property

        Public Property CrossReferenceNo() As String
            Get
                Return m_CrossReferenceNo
            End Get
            Set(ByVal Value As String)
                m_CrossReferenceNo = Value
            End Set
        End Property

        Public Property UnitOfMeasureCrossRef() As String
            Get
                Return m_UnitOfMeasureCrossRef
            End Get
            Set(ByVal Value As String)
                m_UnitOfMeasureCrossRef = Value
            End Set
        End Property

        Public Property CrossReferenceType() As String
            Get
                Return m_CrossReferenceType
            End Get
            Set(ByVal Value As String)
                m_CrossReferenceType = Value
            End Set
        End Property

        Public Property CrossReferenceTypeNo() As String
            Get
                Return m_CrossReferenceTypeNo
            End Get
            Set(ByVal Value As String)
                m_CrossReferenceTypeNo = Value
            End Set
        End Property

        Public Property ItemCategoryCode() As String
            Get
                Return m_ItemCategoryCode
            End Get
            Set(ByVal Value As String)
                m_ItemCategoryCode = Value
            End Set
        End Property

        Public Property Nonstock() As String
            Get
                Return m_Nonstock
            End Get
            Set(ByVal Value As String)
                m_Nonstock = Value
            End Set
        End Property

        Public Property PurchasingCode() As String
            Get
                Return m_PurchasingCode
            End Get
            Set(ByVal Value As String)
                m_PurchasingCode = Value
            End Set
        End Property

        Public Property ProductGroupCode() As String
            Get
                Return m_ProductGroupCode
            End Get
            Set(ByVal Value As String)
                m_ProductGroupCode = Value
            End Set
        End Property

        Public Property RequestedDeliveryDate() As DateTime
            Get
                Return m_RequestedDeliveryDate
            End Get
            Set(ByVal Value As DateTime)
                m_RequestedDeliveryDate = Value
            End Set
        End Property

        Public Property PromisedDeliveryDate() As DateTime
            Get
                Return m_PromisedDeliveryDate
            End Get
            Set(ByVal Value As DateTime)
                m_PromisedDeliveryDate = Value
            End Set
        End Property

        Public Property ShippingTime() As String
            Get
                Return m_ShippingTime
            End Get
            Set(ByVal Value As String)
                m_ShippingTime = Value
            End Set
        End Property

        Public Property OutboundWhseHandlingTime() As String
            Get
                Return m_OutboundWhseHandlingTime
            End Get
            Set(ByVal Value As String)
                m_OutboundWhseHandlingTime = Value
            End Set
        End Property

        Public Property PlannedDeliveryDate() As DateTime
            Get
                Return m_PlannedDeliveryDate
            End Get
            Set(ByVal Value As DateTime)
                m_PlannedDeliveryDate = Value
            End Set
        End Property

        Public Property PlannedShipmentDate() As DateTime
            Get
                Return m_PlannedShipmentDate
            End Get
            Set(ByVal Value As DateTime)
                m_PlannedShipmentDate = Value
            End Set
        End Property

        Public Property AllowLineDisc() As String
            Get
                Return m_AllowLineDisc
            End Get
            Set(ByVal Value As String)
                m_AllowLineDisc = Value
            End Set
        End Property

        Public Property CustomerDiscGroup() As String
            Get
                Return m_CustomerDiscGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerDiscGroup = Value
            End Set
        End Property

        Public Property PackageTrackingNo() As String
            Get
                Return m_PackageTrackingNo
            End Get
            Set(ByVal Value As String)
                m_PackageTrackingNo = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ShipmentLineId As Integer)
            m_DB = DB
            m_ShipmentLineId = ShipmentLineId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal LineNo As Integer)
            m_DB = DB
            m_ShipmentId = ShipmentId
            m_LineNo = LineNo
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal OrderShipmentLineNo As String)
            m_DB = DB
            m_ShipmentId = ShipmentId
            m_OrderShipmentLineNo = OrderShipmentLineNo
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal ShipmentId As Integer, ByVal OrderShipmentLineNo As String, ByVal CartItemId As Integer)
            m_DB = DB
            m_ShipmentId = ShipmentId
            m_OrderShipmentLineNo = OrderShipmentLineNo
            m_CartItemId = CartItemId
        End Sub 'New
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreOrderShipmentLine WHERE " & IIf(ShipmentLineId <> Nothing, "ShipmentLineId = " & m_DB.Quote(ShipmentLineId), "[LineNo] = " & m_DB.Quote(LineNo) & " and ShipmentId = " & m_DB.Quote(ShipmentId))
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


        Protected Overridable Sub LoadBySku()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreOrderShipmentLine WHERE OrderShipmentLineNo = " & m_DB.Quote(OrderShipmentLineNo) & " and ShipmentId = " & m_DB.Quote(ShipmentId)
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
                m_ShipmentLineId = Convert.ToInt32(r.Item("ShipmentLineId"))
                m_WebOrderQty = Convert.ToInt32(r.Item("WebOrderQty"))
                m_TotalQtyShipped = Convert.ToInt32(r.Item("TotalQtyShipped"))
                If IsDBNull(r.Item("SellToCustomerId")) Then
                    m_SellToCustomerId = Nothing
                Else
                    m_SellToCustomerId = Convert.ToInt32(r.Item("SellToCustomerId"))
                End If
                m_ShipmentId = Convert.ToInt32(r.Item("ShipmentId"))
                If IsDBNull(r.Item("LineNo")) Then
                    m_LineNo = Nothing
                Else
                    m_LineNo = Convert.ToInt32(r.Item("LineNo"))
                End If
                If IsDBNull(r.Item("Type")) Then
                    m_Type = Nothing
                Else
                    m_Type = Convert.ToString(r.Item("Type"))
                End If
                If IsDBNull(r.Item("OrderShipmentLineNo")) Then
                    m_OrderShipmentLineNo = Nothing
                Else
                    m_OrderShipmentLineNo = Convert.ToString(r.Item("OrderShipmentLineNo"))
                End If
                If IsDBNull(r.Item("LocationCode")) Then
                    m_LocationCode = Nothing
                Else
                    m_LocationCode = Convert.ToString(r.Item("LocationCode"))
                End If
                If IsDBNull(r.Item("ShipmentDate")) Then
                    m_ShipmentDate = Nothing
                Else
                    m_ShipmentDate = Convert.ToDateTime(r.Item("ShipmentDate"))
                End If
                If IsDBNull(r.Item("Description")) Then
                    m_Description = Nothing
                Else
                    m_Description = Convert.ToString(r.Item("Description"))
                End If
                If IsDBNull(r.Item("Description2")) Then
                    m_Description2 = Nothing
                Else
                    m_Description2 = Convert.ToString(r.Item("Description2"))
                End If
                If IsDBNull(r.Item("UnitOfMeasure")) Then
                    m_UnitOfMeasure = Nothing
                Else
                    m_UnitOfMeasure = Convert.ToString(r.Item("UnitOfMeasure"))
                End If
                If IsDBNull(r.Item("Quantity")) Then
                    m_Quantity = Nothing
                Else
                    m_Quantity = Convert.ToDouble(r.Item("Quantity"))
                End If
                If IsDBNull(r.Item("UnitPrice")) Then
                    m_UnitPrice = Nothing
                Else
                    m_UnitPrice = Convert.ToDouble(r.Item("UnitPrice"))
                End If
                If IsDBNull(r.Item("VATPercent")) Then
                    m_VATPercent = Nothing
                Else
                    m_VATPercent = Convert.ToDouble(r.Item("VATPercent"))
                End If
                If IsDBNull(r.Item("LineDiscountPercent")) Then
                    m_LineDiscountPercent = Nothing
                Else
                    m_LineDiscountPercent = Convert.ToDouble(r.Item("LineDiscountPercent"))
                End If
                If IsDBNull(r.Item("AllowInvoiceDisc")) Then
                    m_AllowInvoiceDisc = Nothing
                Else
                    m_AllowInvoiceDisc = Convert.ToString(r.Item("AllowInvoiceDisc"))
                End If
                If IsDBNull(r.Item("GrossWeight")) Then
                    m_GrossWeight = Nothing
                Else
                    m_GrossWeight = Convert.ToDouble(r.Item("GrossWeight"))
                End If
                If IsDBNull(r.Item("NetWeight")) Then
                    m_NetWeight = Nothing
                Else
                    m_NetWeight = Convert.ToDouble(r.Item("NetWeight"))
                End If
                If IsDBNull(r.Item("UnitsPerParcel")) Then
                    m_UnitsPerParcel = Nothing
                Else
                    m_UnitsPerParcel = Convert.ToDouble(r.Item("UnitsPerParcel"))
                End If
                If IsDBNull(r.Item("UnitVolume")) Then
                    m_UnitVolume = Nothing
                Else
                    m_UnitVolume = Convert.ToDouble(r.Item("UnitVolume"))
                End If
                If IsDBNull(r.Item("ItemShptEntryNo")) Then
                    m_ItemShptEntryNo = Nothing
                Else
                    m_ItemShptEntryNo = Convert.ToInt32(r.Item("ItemShptEntryNo"))
                End If
                If IsDBNull(r.Item("CustomerPriceGroup")) Then
                    m_CustomerPriceGroup = Nothing
                Else
                    m_CustomerPriceGroup = Convert.ToString(r.Item("CustomerPriceGroup"))
                End If
                If IsDBNull(r.Item("QtyShippedNotInvoiced")) Then
                    m_QtyShippedNotInvoiced = Nothing
                Else
                    m_QtyShippedNotInvoiced = Convert.ToDouble(r.Item("QtyShippedNotInvoiced"))
                End If
                If IsDBNull(r.Item("QuantityInvoiced")) Then
                    m_QuantityInvoiced = Nothing
                Else
                    m_QuantityInvoiced = Convert.ToDouble(r.Item("QuantityInvoiced"))
                End If
                m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                m_CartItemId = Convert.ToInt32(r.Item("CartItemId"))
                If IsDBNull(r.Item("BillToCustomerId")) Then
                    m_BillToCustomerId = Nothing
                Else
                    m_BillToCustomerId = Convert.ToInt32(r.Item("BillToCustomerId"))
                End If
                If IsDBNull(r.Item("PurchaseOrderNo")) Then
                    m_PurchaseOrderNo = Nothing
                Else
                    m_PurchaseOrderNo = Convert.ToString(r.Item("PurchaseOrderNo"))
                End If
                If IsDBNull(r.Item("PurchOrderLineNo")) Then
                    m_PurchOrderLineNo = Nothing
                Else
                    m_PurchOrderLineNo = Convert.ToInt32(r.Item("PurchOrderLineNo"))
                End If
                If IsDBNull(r.Item("DropShipment")) Then
                    m_DropShipment = Nothing
                Else
                    m_DropShipment = Convert.ToString(r.Item("DropShipment"))
                End If
                If IsDBNull(r.Item("CurrencyCode")) Then
                    m_CurrencyCode = Nothing
                Else
                    m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
                End If
                If IsDBNull(r.Item("VariantCode")) Then
                    m_VariantCode = Nothing
                Else
                    m_VariantCode = Convert.ToString(r.Item("VariantCode"))
                End If
                If IsDBNull(r.Item("BinCode")) Then
                    m_BinCode = Nothing
                Else
                    m_BinCode = Convert.ToString(r.Item("BinCode"))
                End If
                If IsDBNull(r.Item("QtyPerUnitOfMeasure")) Then
                    m_QtyPerUnitOfMeasure = Nothing
                Else
                    m_QtyPerUnitOfMeasure = Convert.ToDouble(r.Item("QtyPerUnitOfMeasure"))
                End If
                If IsDBNull(r.Item("UnitOfMeasureCode")) Then
                    m_UnitOfMeasureCode = Nothing
                Else
                    m_UnitOfMeasureCode = Convert.ToString(r.Item("UnitOfMeasureCode"))
                End If
                If IsDBNull(r.Item("QuantityBase")) Then
                    m_QuantityBase = Nothing
                Else
                    m_QuantityBase = Convert.ToDouble(r.Item("QuantityBase"))
                End If
                If IsDBNull(r.Item("QtyInvoicedBase")) Then
                    m_QtyInvoicedBase = Nothing
                Else
                    m_QtyInvoicedBase = Convert.ToDouble(r.Item("QtyInvoicedBase"))
                End If
                If IsDBNull(r.Item("ResponsibilityCenter")) Then
                    m_ResponsibilityCenter = Nothing
                Else
                    m_ResponsibilityCenter = Convert.ToString(r.Item("ResponsibilityCenter"))
                End If
                If IsDBNull(r.Item("CrossReferenceNo")) Then
                    m_CrossReferenceNo = Nothing
                Else
                    m_CrossReferenceNo = Convert.ToString(r.Item("CrossReferenceNo"))
                End If
                If IsDBNull(r.Item("UnitOfMeasureCrossRef")) Then
                    m_UnitOfMeasureCrossRef = Nothing
                Else
                    m_UnitOfMeasureCrossRef = Convert.ToString(r.Item("UnitOfMeasureCrossRef"))
                End If
                If IsDBNull(r.Item("CrossReferenceType")) Then
                    m_CrossReferenceType = Nothing
                Else
                    m_CrossReferenceType = Convert.ToString(r.Item("CrossReferenceType"))
                End If
                If IsDBNull(r.Item("CrossReferenceTypeNo")) Then
                    m_CrossReferenceTypeNo = Nothing
                Else
                    m_CrossReferenceTypeNo = Convert.ToString(r.Item("CrossReferenceTypeNo"))
                End If
                If IsDBNull(r.Item("ItemCategoryCode")) Then
                    m_ItemCategoryCode = Nothing
                Else
                    m_ItemCategoryCode = Convert.ToString(r.Item("ItemCategoryCode"))
                End If
                If IsDBNull(r.Item("Nonstock")) Then
                    m_Nonstock = Nothing
                Else
                    m_Nonstock = Convert.ToString(r.Item("Nonstock"))
                End If
                If IsDBNull(r.Item("PurchasingCode")) Then
                    m_PurchasingCode = Nothing
                Else
                    m_PurchasingCode = Convert.ToString(r.Item("PurchasingCode"))
                End If
                If IsDBNull(r.Item("ProductGroupCode")) Then
                    m_ProductGroupCode = Nothing
                Else
                    m_ProductGroupCode = Convert.ToString(r.Item("ProductGroupCode"))
                End If
                If IsDBNull(r.Item("RequestedDeliveryDate")) Then
                    m_RequestedDeliveryDate = Nothing
                Else
                    m_RequestedDeliveryDate = Convert.ToDateTime(r.Item("RequestedDeliveryDate"))
                End If
                If IsDBNull(r.Item("PromisedDeliveryDate")) Then
                    m_PromisedDeliveryDate = Nothing
                Else
                    m_PromisedDeliveryDate = Convert.ToDateTime(r.Item("PromisedDeliveryDate"))
                End If
                If IsDBNull(r.Item("ShippingTime")) Then
                    m_ShippingTime = Nothing
                Else
                    m_ShippingTime = Convert.ToString(r.Item("ShippingTime"))
                End If
                If IsDBNull(r.Item("OutboundWhseHandlingTime")) Then
                    m_OutboundWhseHandlingTime = Nothing
                Else
                    m_OutboundWhseHandlingTime = Convert.ToString(r.Item("OutboundWhseHandlingTime"))
                End If
                If IsDBNull(r.Item("PlannedDeliveryDate")) Then
                    m_PlannedDeliveryDate = Nothing
                Else
                    m_PlannedDeliveryDate = Convert.ToDateTime(r.Item("PlannedDeliveryDate"))
                End If
                If IsDBNull(r.Item("PlannedShipmentDate")) Then
                    m_PlannedShipmentDate = Nothing
                Else
                    m_PlannedShipmentDate = Convert.ToDateTime(r.Item("PlannedShipmentDate"))
                End If
                If IsDBNull(r.Item("AllowLineDisc")) Then
                    m_AllowLineDisc = Nothing
                Else
                    m_AllowLineDisc = Convert.ToString(r.Item("AllowLineDisc"))
                End If
                If IsDBNull(r.Item("CustomerDiscGroup")) Then
                    m_CustomerDiscGroup = Nothing
                Else
                    m_CustomerDiscGroup = Convert.ToString(r.Item("CustomerDiscGroup"))
                End If
                If IsDBNull(r.Item("PackageTrackingNo")) Then
                    m_PackageTrackingNo = Nothing
                Else
                    m_PackageTrackingNo = Convert.ToString(r.Item("PackageTrackingNo"))
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO StoreOrderShipmentLine (" _
             & " SellToCustomerId" _
             & ",ShipmentId" _
             & ",[LineNo]" _
             & ",Type" _
             & ",OrderShipmentLineNo" _
             & ",LocationCode" _
             & ",ShipmentDate" _
             & ",Description" _
             & ",Description2" _
             & ",UnitOfMeasure" _
             & ",Quantity" _
             & ",UnitPrice" _
             & ",VATPercent" _
             & ",LineDiscountPercent" _
             & ",AllowInvoiceDisc" _
             & ",GrossWeight" _
             & ",NetWeight" _
             & ",UnitsPerParcel" _
             & ",UnitVolume" _
             & ",ItemShptEntryNo" _
             & ",CustomerPriceGroup" _
             & ",QtyShippedNotInvoiced" _
             & ",QuantityInvoiced" _
             & ",OrderId" _
             & ",CartItemId" _
             & ",BillToCustomerId" _
             & ",PurchaseOrderNo" _
             & ",PurchOrderLineNo" _
             & ",DropShipment" _
             & ",CurrencyCode" _
             & ",VariantCode" _
             & ",BinCode" _
             & ",QtyPerUnitOfMeasure" _
             & ",UnitOfMeasureCode" _
             & ",QuantityBase" _
             & ",QtyInvoicedBase" _
             & ",ResponsibilityCenter" _
             & ",CrossReferenceNo" _
             & ",UnitOfMeasureCrossRef" _
             & ",CrossReferenceType" _
             & ",CrossReferenceTypeNo" _
             & ",ItemCategoryCode" _
             & ",Nonstock" _
             & ",PurchasingCode" _
             & ",ProductGroupCode" _
             & ",RequestedDeliveryDate" _
             & ",PromisedDeliveryDate" _
             & ",ShippingTime" _
             & ",OutboundWhseHandlingTime" _
             & ",PlannedDeliveryDate" _
             & ",PlannedShipmentDate" _
             & ",AllowLineDisc" _
             & ",CustomerDiscGroup" _
             & ",PackageTrackingNo" _
             & ",WebOrderQty" _
             & ",TotalQtyShipped" _
             & ") VALUES (" _
             & m_DB.Number(SellToCustomerId) _
             & "," & m_DB.Number(ShipmentId) _
             & "," & m_DB.Number(LineNo) _
             & "," & m_DB.Quote(Type) _
             & "," & m_DB.Quote(OrderShipmentLineNo) _
             & "," & m_DB.Quote(LocationCode) _
             & "," & m_DB.NullQuote(ShipmentDate) _
             & "," & m_DB.Quote(Description) _
             & "," & m_DB.Quote(Description2) _
             & "," & m_DB.Quote(UnitOfMeasure) _
             & "," & m_DB.Number(Quantity) _
             & "," & m_DB.Number(UnitPrice) _
             & "," & m_DB.Number(VATPercent) _
             & "," & m_DB.Number(LineDiscountPercent) _
             & "," & m_DB.Quote(AllowInvoiceDisc) _
             & "," & m_DB.Number(GrossWeight) _
             & "," & m_DB.Number(NetWeight) _
             & "," & m_DB.Number(UnitsPerParcel) _
             & "," & m_DB.Number(UnitVolume) _
             & "," & m_DB.Number(ItemShptEntryNo) _
             & "," & m_DB.Quote(CustomerPriceGroup) _
             & "," & m_DB.Number(QtyShippedNotInvoiced) _
             & "," & m_DB.Number(QuantityInvoiced) _
             & "," & m_DB.Number(OrderId) _
             & "," & m_DB.Number(CartItemId) _
             & "," & m_DB.Number(BillToCustomerId) _
             & "," & m_DB.Quote(PurchaseOrderNo) _
             & "," & m_DB.Number(PurchOrderLineNo) _
             & "," & m_DB.Quote(DropShipment) _
             & "," & m_DB.Quote(CurrencyCode) _
             & "," & m_DB.Quote(VariantCode) _
             & "," & m_DB.Quote(BinCode) _
             & "," & m_DB.Number(QtyPerUnitOfMeasure) _
             & "," & m_DB.Quote(UnitOfMeasureCode) _
             & "," & m_DB.Number(QuantityBase) _
             & "," & m_DB.Number(QtyInvoicedBase) _
             & "," & m_DB.Quote(ResponsibilityCenter) _
             & "," & m_DB.Quote(CrossReferenceNo) _
             & "," & m_DB.Quote(UnitOfMeasureCrossRef) _
             & "," & m_DB.Quote(CrossReferenceType) _
             & "," & m_DB.Quote(CrossReferenceTypeNo) _
             & "," & m_DB.Quote(ItemCategoryCode) _
             & "," & m_DB.Quote(Nonstock) _
             & "," & m_DB.Quote(PurchasingCode) _
             & "," & m_DB.Quote(ProductGroupCode) _
             & "," & m_DB.NullQuote(RequestedDeliveryDate) _
             & "," & m_DB.NullQuote(PromisedDeliveryDate) _
             & "," & m_DB.Quote(ShippingTime) _
             & "," & m_DB.Quote(OutboundWhseHandlingTime) _
             & "," & m_DB.NullQuote(PlannedDeliveryDate) _
             & "," & m_DB.NullQuote(PlannedShipmentDate) _
             & "," & m_DB.Quote(AllowLineDisc) _
             & "," & m_DB.Quote(CustomerDiscGroup) _
             & "," & m_DB.Quote(PackageTrackingNo) _
             & "," & m_DB.Number(WebOrderQty) _
             & "," & m_DB.Number(TotalQtyShipped) _
             & ")"

            ShipmentLineId = m_DB.InsertSQL(SQL)

            Return ShipmentLineId
        End Function
        Public Function InsertSassi(ByVal _DB As Database) As Integer
            Dim SQL As String
            SQL = " INSERT INTO StoreOrderShipmentLine (" _
             & " SellToCustomerId" _
             & ",ShipmentId" _
             & ",OrderShipmentLineNo" _
             & ",Type" _
             & ",Description" _
             & ",UnitOfMeasure" _
             & ",Quantity" _
             & ",UnitPrice" _
             & ",OrderId" _
             & ",CartItemId" _
             & ",BillToCustomerId" _
             & ") VALUES (" _
             & _DB.Number(SellToCustomerId) _
             & "," & _DB.Number(ShipmentId) _
             & "," & _DB.Quote(OrderShipmentLineNo) _
             & "," & _DB.Quote(Type) _
             & "," & _DB.Quote(Description) _
             & "," & _DB.Quote(UnitOfMeasure) _
             & "," & _DB.Number(Quantity) _
             & "," & _DB.Number(UnitPrice) _
             & "," & _DB.Number(OrderId) _
             & "," & _DB.Number(CartItemId) _
             & "," & _DB.Number(BillToCustomerId) _
             & ")"

            ShipmentLineId = _DB.InsertSQL(SQL)

            Return ShipmentLineId
        End Function
        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreOrderShipmentLine SET " _
             & " SellToCustomerId = " & m_DB.Number(SellToCustomerId) _
             & ",ShipmentId = " & m_DB.Number(ShipmentId) _
             & ",[LineNo] = " & m_DB.Number(LineNo) _
             & ",Type = " & m_DB.Quote(Type) _
             & ",OrderShipmentLineNo = " & m_DB.Quote(OrderShipmentLineNo) _
             & ",LocationCode = " & m_DB.Quote(LocationCode) _
             & ",ShipmentDate = " & m_DB.NullQuote(ShipmentDate) _
             & ",Description = " & m_DB.Quote(Description) _
             & ",Description2 = " & m_DB.Quote(Description2) _
             & ",UnitOfMeasure = " & m_DB.Quote(UnitOfMeasure) _
             & ",Quantity = " & m_DB.Number(Quantity) _
             & ",UnitPrice = " & m_DB.Number(UnitPrice) _
             & ",VATPercent = " & m_DB.Number(VATPercent) _
             & ",LineDiscountPercent = " & m_DB.Number(LineDiscountPercent) _
             & ",AllowInvoiceDisc = " & m_DB.Quote(AllowInvoiceDisc) _
             & ",GrossWeight = " & m_DB.Number(GrossWeight) _
             & ",NetWeight = " & m_DB.Number(NetWeight) _
             & ",UnitsPerParcel = " & m_DB.Number(UnitsPerParcel) _
             & ",UnitVolume = " & m_DB.Number(UnitVolume) _
             & ",ItemShptEntryNo = " & m_DB.Number(ItemShptEntryNo) _
             & ",CustomerPriceGroup = " & m_DB.Quote(CustomerPriceGroup) _
             & ",QtyShippedNotInvoiced = " & m_DB.Number(QtyShippedNotInvoiced) _
             & ",QuantityInvoiced = " & m_DB.Number(QuantityInvoiced) _
             & ",OrderId = " & m_DB.Number(OrderId) _
             & ",CartItemId = " & m_DB.Number(CartItemId) _
             & ",BillToCustomerId = " & m_DB.Number(BillToCustomerId) _
             & ",PurchaseOrderNo = " & m_DB.Quote(PurchaseOrderNo) _
             & ",PurchOrderLineNo = " & m_DB.Number(PurchOrderLineNo) _
             & ",DropShipment = " & m_DB.Quote(DropShipment) _
             & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
             & ",VariantCode = " & m_DB.Quote(VariantCode) _
             & ",BinCode = " & m_DB.Quote(BinCode) _
             & ",QtyPerUnitOfMeasure = " & m_DB.Number(QtyPerUnitOfMeasure) _
             & ",UnitOfMeasureCode = " & m_DB.Quote(UnitOfMeasureCode) _
             & ",QuantityBase = " & m_DB.Number(QuantityBase) _
             & ",QtyInvoicedBase = " & m_DB.Number(QtyInvoicedBase) _
             & ",ResponsibilityCenter = " & m_DB.Quote(ResponsibilityCenter) _
             & ",CrossReferenceNo = " & m_DB.Quote(CrossReferenceNo) _
             & ",UnitOfMeasureCrossRef = " & m_DB.Quote(UnitOfMeasureCrossRef) _
             & ",CrossReferenceType = " & m_DB.Quote(CrossReferenceType) _
             & ",CrossReferenceTypeNo = " & m_DB.Quote(CrossReferenceTypeNo) _
             & ",ItemCategoryCode = " & m_DB.Quote(ItemCategoryCode) _
             & ",Nonstock = " & m_DB.Quote(Nonstock) _
             & ",PurchasingCode = " & m_DB.Quote(PurchasingCode) _
             & ",ProductGroupCode = " & m_DB.Quote(ProductGroupCode) _
             & ",RequestedDeliveryDate = " & m_DB.NullQuote(RequestedDeliveryDate) _
             & ",PromisedDeliveryDate = " & m_DB.NullQuote(PromisedDeliveryDate) _
             & ",ShippingTime = " & m_DB.Quote(ShippingTime) _
             & ",OutboundWhseHandlingTime = " & m_DB.Quote(OutboundWhseHandlingTime) _
             & ",PlannedDeliveryDate = " & m_DB.NullQuote(PlannedDeliveryDate) _
             & ",PlannedShipmentDate = " & m_DB.NullQuote(PlannedShipmentDate) _
             & ",AllowLineDisc = " & m_DB.Quote(AllowLineDisc) _
             & ",CustomerDiscGroup = " & m_DB.Quote(CustomerDiscGroup) _
             & ",PackageTrackingNo = " & m_DB.Quote(PackageTrackingNo) _
             & ",WebOrderQty = " & m_DB.Number(WebOrderQty) _
             & ",TotalQtyShipped = " & m_DB.Number(TotalQtyShipped) _
             & " WHERE " & IIf(ShipmentLineId <> Nothing, "ShipmentLineId = " & m_DB.Quote(ShipmentLineId), "[LineNo] = " & m_DB.Quote(LineNo) & " and ShipmentId = " & m_DB.Quote(ShipmentId))

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update
        Public Sub UpdateSassi(ByVal _DB As Database)
            Dim SQL As String

            SQL = " UPDATE StoreOrderShipmentLine SET " _
             & " SellToCustomerId = " & _DB.Number(SellToCustomerId) _
             & ",ShipmentId = " & _DB.Number(ShipmentId) _
             & ",[OrderShipmentLineNo] = " & _DB.Quote(OrderShipmentLineNo) _
             & ",Type = " & _DB.Quote(Type) _
             & ",Description = " & _DB.Quote(Description) _
             & ",UnitOfMeasure = " & _DB.Quote(UnitOfMeasure) _
             & ",Quantity = " & _DB.Number(Quantity) _
             & ",UnitPrice = " & _DB.Number(UnitPrice) _
             & ",OrderId = " & _DB.Number(OrderId) _
             & ",CartItemId = " & _DB.Number(CartItemId) _
             & ",BillToCustomerId = " & _DB.Number(BillToCustomerId) _
             & " WHERE " & IIf(ShipmentLineId <> Nothing, "ShipmentLineId = " & _DB.Quote(ShipmentLineId), "[LineNo] = " & _DB.Quote(LineNo) & " and ShipmentId = " & _DB.Quote(ShipmentId))

            _DB.ExecuteSQL(SQL)

        End Sub 'Update
        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreOrderShipmentLine WHERE " & IIf(ShipmentLineId <> Nothing, "ShipmentLineId = " & m_DB.Quote(ShipmentLineId), "[LineNo] = " & m_DB.Quote(LineNo) & " and ShipmentId = " & m_DB.Quote(ShipmentId))
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreOrderShipmentLineCollection
        Inherits GenericCollection(Of StoreOrderShipmentLineRow)
    End Class

End Namespace


