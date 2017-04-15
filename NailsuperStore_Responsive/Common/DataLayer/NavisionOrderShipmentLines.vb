Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionOrderShipmentLinesRow
        Inherits NavisionOrderShipmentLinesRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionOrderShipmentLinesCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New NavisionOrderShipmentLinesCollection
            Try
                Dim row As NavisionOrderShipmentLinesRow
                Dim SQL As String = "select * from _NAVISION_ORDER_SHIPMENT_LINES where [Type] = 'Item' and ltrim(rtrim(Order_Shipment_Line_No)) <> '' and exists (select ShipmentNo from StoreOrderShipment where _NAVISION_ORDER_SHIPMENT_LINES.Document_No = StoreOrderShipment.ShipmentNo)"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionOrderShipmentLinesRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return c
        End Function

    End Class

    Public MustInherit Class NavisionOrderShipmentLinesRowBase
        Private m_DB As Database
        Private m_Sell_to_Customer_No As String = Nothing
        Private m_Document_No As String = Nothing
        Private m_Line_No As Integer = Nothing
        Private m_Type As String = Nothing
        Private m_Order_Shipment_Line_No As String = Nothing
        Private m_Location_Code As String = Nothing
        Private m_Shipment_Date As DateTime = Nothing
        Private m_Description As String = Nothing
        Private m_Description_2 As String = Nothing
        Private m_Unit_of_Measure As String = Nothing
        Private m_Quantity As Double = Nothing
        Private m_Unit_Price As Double = Nothing
        Private m_VAT_Percent As Double = Nothing
        Private m_Line_Discount_Percent As Double = Nothing
        Private m_Allow_Invoice_Disc As String = Nothing
        Private m_Gross_Weight As Double = Nothing
        Private m_Net_Weight As Double = Nothing
        Private m_Units_per_Parcel As Double = Nothing
        Private m_Unit_Volume As Double = Nothing
        Private m_Item_Shpt_Entry_No As Integer = Nothing
        Private m_Customer_Price_Group As String = Nothing
        Private m_Qty_Shipped_Not_Invoiced As Double = Nothing
        Private m_Quantity_Invoiced As Double = Nothing
        Private m_Order_No As String = Nothing
        Private m_Order_Line_No As Integer = Nothing
        Private m_Bill_to_Customer_No As String = Nothing
        Private m_Purchase_Order_No As String = Nothing
        Private m_Purch_Order_Line_No As Integer = Nothing
        Private m_Drop_Shipment As String = Nothing
        Private m_Currency_Code As String = Nothing
        Private m_Variant_Code As String = Nothing
        Private m_Bin_Code As String = Nothing
        Private m_Qty_per_Unit_of_Measure As Double = Nothing
        Private m_Unit_of_Measure_Code As String = Nothing
        Private m_Quantity_Base As Double = Nothing
        Private m_Qty_Invoiced_Base As Double = Nothing
        Private m_Responsibility_Center As String = Nothing
        Private m_Cross_Reference_No As String = Nothing
        Private m_Unit_of_Measure_Cross_Ref As String = Nothing
        Private m_Cross_Reference_Type As String = Nothing
        Private m_Cross_Reference_Type_No As String = Nothing
        Private m_Item_Category_Code As String = Nothing
        Private m_Nonstock As String = Nothing
        Private m_Purchasing_Code As String = Nothing
        Private m_Product_Group_Code As String = Nothing
        Private m_Requested_Delivery_Date As DateTime = Nothing
        Private m_Promised_Delivery_Date As DateTime = Nothing
        Private m_Shipping_Time As String = Nothing
        Private m_Outbound_Whse_Handling_Time As String = Nothing
        Private m_Planned_Delivery_Date As DateTime = Nothing
        Private m_Planned_Shipment_Date As DateTime = Nothing
        Private m_Allow_Line_Disc As String = Nothing
        Private m_Customer_Disc_Group As String = Nothing
        Private m_Package_Tracking_No As String = Nothing
        Private m_Web_Order_Qty As Integer = Nothing
        Private m_Web_Reference_No As String = Nothing
        Private m_Total_Qty_Shipped As Integer = Nothing
        Private m_ID As Integer = Nothing

        Public Property Web_Reference_No() As String
            Get
                Return m_Web_Reference_No
            End Get
            Set(ByVal value As String)
                m_Web_Reference_No = value
            End Set
        End Property

        Public Property Web_Order_Qty() As Integer
            Get
                Return m_Web_Order_Qty
            End Get
            Set(ByVal value As Integer)
                m_Web_Order_Qty = value
            End Set
        End Property

        Public Property Total_Qty_Shipped() As Integer
            Get
                Return m_Total_Qty_Shipped
            End Get
            Set(ByVal value As Integer)
                m_Total_Qty_Shipped = value
            End Set
        End Property

        Public Property Sell_to_Customer_No() As String
            Get
                Return Trim(m_Sell_to_Customer_No)
            End Get
            Set(ByVal Value As String)
                m_Sell_to_Customer_No = Trim(Value)
            End Set
        End Property

        Public Property Document_No() As String
            Get
                Return Trim(m_Document_No)
            End Get
            Set(ByVal Value As String)
                m_Document_No = Trim(Value)
            End Set
        End Property

        Public Property Line_No() As Integer
            Get
                Return Trim(m_Line_No)
            End Get
            Set(ByVal Value As Integer)
                m_Line_No = Trim(Value)
            End Set
        End Property

        Public Property Type() As String
            Get
                Return Trim(m_Type)
            End Get
            Set(ByVal Value As String)
                m_Type = Trim(Value)
            End Set
        End Property

        Public Property Order_Shipment_Line_No() As String
            Get
                Return Trim(m_Order_Shipment_Line_No)
            End Get
            Set(ByVal Value As String)
                m_Order_Shipment_Line_No = Trim(Value)
            End Set
        End Property

        Public Property Location_Code() As String
            Get
                Return Trim(m_Location_Code)
            End Get
            Set(ByVal Value As String)
                m_Location_Code = Trim(Value)
            End Set
        End Property

        Public Property Shipment_Date() As DateTime
            Get
                Return Trim(m_Shipment_Date)
            End Get
            Set(ByVal Value As DateTime)
                m_Shipment_Date = Trim(Value)
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Trim(m_Description)
            End Get
            Set(ByVal Value As String)
                m_Description = Trim(Value)
            End Set
        End Property

        Public Property Description_2() As String
            Get
                Return Trim(m_Description_2)
            End Get
            Set(ByVal Value As String)
                m_Description_2 = Trim(Value)
            End Set
        End Property

        Public Property Unit_of_Measure() As String
            Get
                Return Trim(m_Unit_of_Measure)
            End Get
            Set(ByVal Value As String)
                m_Unit_of_Measure = Trim(Value)
            End Set
        End Property

        Public Property Quantity() As Double
            Get
                Return Trim(m_Quantity)
            End Get
            Set(ByVal Value As Double)
                m_Quantity = Trim(Value)
            End Set
        End Property

        Public Property Unit_Price() As Double
            Get
                Return Trim(m_Unit_Price)
            End Get
            Set(ByVal Value As Double)
                m_Unit_Price = Trim(Value)
            End Set
        End Property

        Public Property VAT_Percent() As Double
            Get
                Return Trim(m_VAT_Percent)
            End Get
            Set(ByVal Value As Double)
                m_VAT_Percent = Trim(Value)
            End Set
        End Property

        Public Property Line_Discount_Percent() As Double
            Get
                Return Trim(m_Line_Discount_Percent)
            End Get
            Set(ByVal Value As Double)
                m_Line_Discount_Percent = Trim(Value)
            End Set
        End Property

        Public Property Allow_Invoice_Disc() As String
            Get
                Return Trim(m_Allow_Invoice_Disc)
            End Get
            Set(ByVal Value As String)
                m_Allow_Invoice_Disc = Trim(Value)
            End Set
        End Property

        Public Property Gross_Weight() As Double
            Get
                Return Trim(m_Gross_Weight)
            End Get
            Set(ByVal Value As Double)
                m_Gross_Weight = Trim(Value)
            End Set
        End Property

        Public Property Net_Weight() As Double
            Get
                Return Trim(m_Net_Weight)
            End Get
            Set(ByVal Value As Double)
                m_Net_Weight = Trim(Value)
            End Set
        End Property

        Public Property Units_per_Parcel() As Double
            Get
                Return Trim(m_Units_per_Parcel)
            End Get
            Set(ByVal Value As Double)
                m_Units_per_Parcel = Trim(Value)
            End Set
        End Property

        Public Property Unit_Volume() As Double
            Get
                Return Trim(m_Unit_Volume)
            End Get
            Set(ByVal Value As Double)
                m_Unit_Volume = Trim(Value)
            End Set
        End Property

        Public Property Item_Shpt_Entry_No() As Integer
            Get
                Return Trim(m_Item_Shpt_Entry_No)
            End Get
            Set(ByVal Value As Integer)
                m_Item_Shpt_Entry_No = Trim(Value)
            End Set
        End Property

        Public Property Customer_Price_Group() As String
            Get
                Return Trim(m_Customer_Price_Group)
            End Get
            Set(ByVal Value As String)
                m_Customer_Price_Group = Trim(Value)
            End Set
        End Property

        Public Property Qty_Shipped_Not_Invoiced() As Double
            Get
                Return Trim(m_Qty_Shipped_Not_Invoiced)
            End Get
            Set(ByVal Value As Double)
                m_Qty_Shipped_Not_Invoiced = Trim(Value)
            End Set
        End Property

        Public Property Quantity_Invoiced() As Double
            Get
                Return Trim(m_Quantity_Invoiced)
            End Get
            Set(ByVal Value As Double)
                m_Quantity_Invoiced = Trim(Value)
            End Set
        End Property

        Public Property Order_No() As String
            Get
                Return Trim(m_Order_No)
            End Get
            Set(ByVal Value As String)
                m_Order_No = Trim(Value)
            End Set
        End Property

        Public Property Order_Line_No() As Integer
            Get
                Return Trim(m_Order_Line_No)
            End Get
            Set(ByVal Value As Integer)
                m_Order_Line_No = Trim(Value)
            End Set
        End Property

        Public Property Bill_to_Customer_No() As String
            Get
                Return Trim(m_Bill_to_Customer_No)
            End Get
            Set(ByVal Value As String)
                m_Bill_to_Customer_No = Trim(Value)
            End Set
        End Property

        Public Property Purchase_Order_No() As String
            Get
                Return Trim(m_Purchase_Order_No)
            End Get
            Set(ByVal Value As String)
                m_Purchase_Order_No = Trim(Value)
            End Set
        End Property

        Public Property Purch_Order_Line_No() As Integer
            Get
                Return Trim(m_Purch_Order_Line_No)
            End Get
            Set(ByVal Value As Integer)
                m_Purch_Order_Line_No = Trim(Value)
            End Set
        End Property

        Public Property Drop_Shipment() As String
            Get
                Return Trim(m_Drop_Shipment)
            End Get
            Set(ByVal Value As String)
                m_Drop_Shipment = Trim(Value)
            End Set
        End Property

        Public Property Currency_Code() As String
            Get
                Return Trim(m_Currency_Code)
            End Get
            Set(ByVal Value As String)
                m_Currency_Code = Trim(Value)
            End Set
        End Property

        Public Property Variant_Code() As String
            Get
                Return Trim(m_Variant_Code)
            End Get
            Set(ByVal Value As String)
                m_Variant_Code = Trim(Value)
            End Set
        End Property

        Public Property Bin_Code() As String
            Get
                Return Trim(m_Bin_Code)
            End Get
            Set(ByVal Value As String)
                m_Bin_Code = Trim(Value)
            End Set
        End Property

        Public Property Qty_per_Unit_of_Measure() As Double
            Get
                Return Trim(m_Qty_per_Unit_of_Measure)
            End Get
            Set(ByVal Value As Double)
                m_Qty_per_Unit_of_Measure = Trim(Value)
            End Set
        End Property

        Public Property Unit_of_Measure_Code() As String
            Get
                Return Trim(m_Unit_of_Measure_Code)
            End Get
            Set(ByVal Value As String)
                m_Unit_of_Measure_Code = Trim(Value)
            End Set
        End Property

        Public Property Quantity_Base() As Double
            Get
                Return Trim(m_Quantity_Base)
            End Get
            Set(ByVal Value As Double)
                m_Quantity_Base = Trim(Value)
            End Set
        End Property

        Public Property Qty_Invoiced_Base() As Double
            Get
                Return Trim(m_Qty_Invoiced_Base)
            End Get
            Set(ByVal Value As Double)
                m_Qty_Invoiced_Base = Trim(Value)
            End Set
        End Property

        Public Property Responsibility_Center() As String
            Get
                Return Trim(m_Responsibility_Center)
            End Get
            Set(ByVal Value As String)
                m_Responsibility_Center = Trim(Value)
            End Set
        End Property

        Public Property Cross_Reference_No() As String
            Get
                Return Trim(m_Cross_Reference_No)
            End Get
            Set(ByVal Value As String)
                m_Cross_Reference_No = Trim(Value)
            End Set
        End Property

        Public Property Unit_of_Measure_Cross_Ref() As String
            Get
                Return Trim(m_Unit_of_Measure_Cross_Ref)
            End Get
            Set(ByVal Value As String)
                m_Unit_of_Measure_Cross_Ref = Trim(Value)
            End Set
        End Property

        Public Property Cross_Reference_Type() As String
            Get
                Return Trim(m_Cross_Reference_Type)
            End Get
            Set(ByVal Value As String)
                m_Cross_Reference_Type = Trim(Value)
            End Set
        End Property

        Public Property Cross_Reference_Type_No() As String
            Get
                Return Trim(m_Cross_Reference_Type_No)
            End Get
            Set(ByVal Value As String)
                m_Cross_Reference_Type_No = Trim(Value)
            End Set
        End Property

        Public Property Item_Category_Code() As String
            Get
                Return Trim(m_Item_Category_Code)
            End Get
            Set(ByVal Value As String)
                m_Item_Category_Code = Trim(Value)
            End Set
        End Property

        Public Property Nonstock() As String
            Get
                Return Trim(m_Nonstock)
            End Get
            Set(ByVal Value As String)
                m_Nonstock = Trim(Value)
            End Set
        End Property

        Public Property Purchasing_Code() As String
            Get
                Return Trim(m_Purchasing_Code)
            End Get
            Set(ByVal Value As String)
                m_Purchasing_Code = Trim(Value)
            End Set
        End Property

        Public Property Product_Group_Code() As String
            Get
                Return Trim(m_Product_Group_Code)
            End Get
            Set(ByVal Value As String)
                m_Product_Group_Code = Trim(Value)
            End Set
        End Property

        Public Property Requested_Delivery_Date() As DateTime
            Get
                Return Trim(m_Requested_Delivery_Date)
            End Get
            Set(ByVal Value As DateTime)
                m_Requested_Delivery_Date = Trim(Value)
            End Set
        End Property

        Public Property Promised_Delivery_Date() As DateTime
            Get
                Return Trim(m_Promised_Delivery_Date)
            End Get
            Set(ByVal Value As DateTime)
                m_Promised_Delivery_Date = Trim(Value)
            End Set
        End Property

        Public Property Shipping_Time() As String
            Get
                Return Trim(m_Shipping_Time)
            End Get
            Set(ByVal Value As String)
                m_Shipping_Time = Trim(Value)
            End Set
        End Property

        Public Property Outbound_Whse_Handling_Time() As String
            Get
                Return Trim(m_Outbound_Whse_Handling_Time)
            End Get
            Set(ByVal Value As String)
                m_Outbound_Whse_Handling_Time = Trim(Value)
            End Set
        End Property

        Public Property Planned_Delivery_Date() As DateTime
            Get
                Return Trim(m_Planned_Delivery_Date)
            End Get
            Set(ByVal Value As DateTime)
                m_Planned_Delivery_Date = Trim(Value)
            End Set
        End Property

        Public Property Planned_Shipment_Date() As DateTime
            Get
                Return Trim(m_Planned_Shipment_Date)
            End Get
            Set(ByVal Value As DateTime)
                m_Planned_Shipment_Date = Trim(Value)
            End Set
        End Property

        Public Property Allow_Line_Disc() As String
            Get
                Return Trim(m_Allow_Line_Disc)
            End Get
            Set(ByVal Value As String)
                m_Allow_Line_Disc = Trim(Value)
            End Set
        End Property

        Public Property Customer_Disc_Group() As String
            Get
                Return Trim(m_Customer_Disc_Group)
            End Get
            Set(ByVal Value As String)
                m_Customer_Disc_Group = Trim(Value)
            End Set
        End Property

        Public Property Package_Tracking_No() As String
            Get
                Return Trim(m_Package_Tracking_No)
            End Get
            Set(ByVal Value As String)
                m_Package_Tracking_No = Trim(Value)
            End Set
        End Property

        Public Property ID() As Integer
            Get
                Return m_ID
            End Get
            Set(ByVal Value As Integer)
                m_ID = Value
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

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            If IsDBNull(r.Item("Sell_to_Customer_No")) Then
                m_Sell_to_Customer_No = Nothing
            Else
                m_Sell_to_Customer_No = Convert.ToString(r.Item("Sell_to_Customer_No"))
            End If
            If IsDBNull(r.Item("Document_No")) Then
                m_Document_No = Nothing
            Else
                m_Document_No = Convert.ToString(r.Item("Document_No"))
            End If
            If IsDBNull(r.Item("Line_No")) Then
                m_Line_No = Nothing
            Else
                m_Line_No = Convert.ToInt32(r.Item("Line_No"))
            End If
            If IsDBNull(r.Item("Type")) Then
                m_Type = Nothing
            Else
                m_Type = Convert.ToString(r.Item("Type"))
            End If
            If IsDBNull(r.Item("Order_Shipment_Line_No")) Then
                m_Order_Shipment_Line_No = Nothing
            Else
                m_Order_Shipment_Line_No = Convert.ToString(r.Item("Order_Shipment_Line_No"))
            End If
            If IsDBNull(r.Item("Location_Code")) Then
                m_Location_Code = Nothing
            Else
                m_Location_Code = Convert.ToString(r.Item("Location_Code"))
            End If
            If IsDBNull(r.Item("Shipment_Date")) OrElse Not IsDate(r.Item("Shipment_Date")) Then
                m_Shipment_Date = Nothing
            Else
                m_Shipment_Date = Convert.ToDateTime(r.Item("Shipment_Date"))
            End If
            If IsDBNull(r.Item("Description")) Then
                m_Description = Nothing
            Else
                m_Description = Convert.ToString(r.Item("Description"))
            End If
            If IsDBNull(r.Item("Description_2")) Then
                m_Description_2 = Nothing
            Else
                m_Description_2 = Convert.ToString(r.Item("Description_2"))
            End If
            If IsDBNull(r.Item("Unit_of_Measure")) Then
                m_Unit_of_Measure = Nothing
            Else
                m_Unit_of_Measure = Convert.ToString(r.Item("Unit_of_Measure"))
            End If
            If IsDBNull(r.Item("Quantity")) Then
                m_Quantity = Nothing
            Else
                m_Quantity = Convert.ToDouble(r.Item("Quantity"))
            End If
            If IsDBNull(r.Item("Unit_Price")) Then
                m_Unit_Price = Nothing
            Else
                m_Unit_Price = Convert.ToDouble(r.Item("Unit_Price"))
            End If
            If IsDBNull(r.Item("VAT_Percent")) Then
                m_VAT_Percent = Nothing
            Else
                m_VAT_Percent = Convert.ToDouble(r.Item("VAT_Percent"))
            End If
            If IsDBNull(r.Item("Line_Discount_Percent")) Then
                m_Line_Discount_Percent = Nothing
            Else
                m_Line_Discount_Percent = Convert.ToDouble(r.Item("Line_Discount_Percent"))
            End If
            If IsDBNull(r.Item("Allow_Invoice_Disc")) Then
                m_Allow_Invoice_Disc = Nothing
            Else
                m_Allow_Invoice_Disc = Convert.ToString(r.Item("Allow_Invoice_Disc"))
            End If
            If IsDBNull(r.Item("Gross_Weight")) Then
                m_Gross_Weight = Nothing
            Else
                m_Gross_Weight = Convert.ToDouble(r.Item("Gross_Weight"))
            End If
            If IsDBNull(r.Item("Net_Weight")) Then
                m_Net_Weight = Nothing
            Else
                m_Net_Weight = Convert.ToDouble(r.Item("Net_Weight"))
            End If
            If IsDBNull(r.Item("Units_per_Parcel")) Then
                m_Units_per_Parcel = Nothing
            Else
                m_Units_per_Parcel = Convert.ToDouble(r.Item("Units_per_Parcel"))
            End If
            If IsDBNull(r.Item("Unit_Volume")) Then
                m_Unit_Volume = Nothing
            Else
                m_Unit_Volume = Convert.ToDouble(r.Item("Unit_Volume"))
            End If
            If IsDBNull(r.Item("Item_Shpt_Entry_No")) Then
                m_Item_Shpt_Entry_No = Nothing
            Else
                m_Item_Shpt_Entry_No = Convert.ToInt32(r.Item("Item_Shpt_Entry_No"))
            End If
            If IsDBNull(r.Item("Customer_Price_Group")) Then
                m_Customer_Price_Group = Nothing
            Else
                m_Customer_Price_Group = Convert.ToString(r.Item("Customer_Price_Group"))
            End If
            If IsDBNull(r.Item("Qty_Shipped_Not_Invoiced")) Then
                m_Qty_Shipped_Not_Invoiced = Nothing
            Else
                m_Qty_Shipped_Not_Invoiced = Convert.ToDouble(r.Item("Qty_Shipped_Not_Invoiced"))
            End If
            If IsDBNull(r.Item("Quantity_Invoiced")) Then
                m_Quantity_Invoiced = Nothing
            Else
                m_Quantity_Invoiced = Convert.ToDouble(r.Item("Quantity_Invoiced"))
            End If
            If IsDBNull(r.Item("Order_No")) Then
                m_Order_No = Nothing
            Else
                m_Order_No = Convert.ToString(r.Item("Order_No"))
            End If
            If IsDBNull(r.Item("Order_Line_No")) Then
                m_Order_Line_No = Nothing
            Else
                m_Order_Line_No = Convert.ToInt32(r.Item("Order_Line_No"))
            End If
            If IsDBNull(r.Item("Bill_to_Customer_No")) Then
                m_Bill_to_Customer_No = Nothing
            Else
                m_Bill_to_Customer_No = Convert.ToString(r.Item("Bill_to_Customer_No"))
            End If
            If IsDBNull(r.Item("Purchase_Order_No")) Then
                m_Purchase_Order_No = Nothing
            Else
                m_Purchase_Order_No = Convert.ToString(r.Item("Purchase_Order_No"))
            End If
            If IsDBNull(r.Item("Purch_Order_Line_No")) Then
                m_Purch_Order_Line_No = Nothing
            Else
                m_Purch_Order_Line_No = Convert.ToInt32(r.Item("Purch_Order_Line_No"))
            End If
            If IsDBNull(r.Item("Drop_Shipment")) Then
                m_Drop_Shipment = Nothing
            Else
                m_Drop_Shipment = Convert.ToString(r.Item("Drop_Shipment"))
            End If
            If IsDBNull(r.Item("Currency_Code")) Then
                m_Currency_Code = Nothing
            Else
                m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
            End If
            If IsDBNull(r.Item("Variant_Code")) Then
                m_Variant_Code = Nothing
            Else
                m_Variant_Code = Convert.ToString(r.Item("Variant_Code"))
            End If
            If IsDBNull(r.Item("Bin_Code")) Then
                m_Bin_Code = Nothing
            Else
                m_Bin_Code = Convert.ToString(r.Item("Bin_Code"))
            End If
            If IsDBNull(r.Item("Qty_per_Unit_of_Measure")) Then
                m_Qty_per_Unit_of_Measure = Nothing
            Else
                m_Qty_per_Unit_of_Measure = Convert.ToDouble(r.Item("Qty_per_Unit_of_Measure"))
            End If
            If IsDBNull(r.Item("Unit_of_Measure_Code")) Then
                m_Unit_of_Measure_Code = Nothing
            Else
                m_Unit_of_Measure_Code = Convert.ToString(r.Item("Unit_of_Measure_Code"))
            End If
            If IsDBNull(r.Item("Quantity_Base")) Then
                m_Quantity_Base = Nothing
            Else
                m_Quantity_Base = Convert.ToDouble(r.Item("Quantity_Base"))
            End If
            If IsDBNull(r.Item("Qty_Invoiced_Base")) Then
                m_Qty_Invoiced_Base = Nothing
            Else
                m_Qty_Invoiced_Base = Convert.ToDouble(r.Item("Qty_Invoiced_Base"))
            End If
            If IsDBNull(r.Item("Responsibility_Center")) Then
                m_Responsibility_Center = Nothing
            Else
                m_Responsibility_Center = Convert.ToString(r.Item("Responsibility_Center"))
            End If
            If IsDBNull(r.Item("Cross_Reference_No")) Then
                m_Cross_Reference_No = Nothing
            Else
                m_Cross_Reference_No = Convert.ToString(r.Item("Cross_Reference_No"))
            End If
            If IsDBNull(r.Item("Unit_of_Measure_Cross_Ref")) Then
                m_Unit_of_Measure_Cross_Ref = Nothing
            Else
                m_Unit_of_Measure_Cross_Ref = Convert.ToString(r.Item("Unit_of_Measure_Cross_Ref"))
            End If
            If IsDBNull(r.Item("Cross_Reference_Type")) Then
                m_Cross_Reference_Type = Nothing
            Else
                m_Cross_Reference_Type = Convert.ToString(r.Item("Cross_Reference_Type"))
            End If
            If IsDBNull(r.Item("Cross_Reference_Type_No")) Then
                m_Cross_Reference_Type_No = Nothing
            Else
                m_Cross_Reference_Type_No = Convert.ToString(r.Item("Cross_Reference_Type_No"))
            End If
            If IsDBNull(r.Item("Item_Category_Code")) Then
                m_Item_Category_Code = Nothing
            Else
                m_Item_Category_Code = Convert.ToString(r.Item("Item_Category_Code"))
            End If
            If IsDBNull(r.Item("Nonstock")) Then
                m_Nonstock = Nothing
            Else
                m_Nonstock = Convert.ToString(r.Item("Nonstock"))
            End If
            If IsDBNull(r.Item("Purchasing_Code")) Then
                m_Purchasing_Code = Nothing
            Else
                m_Purchasing_Code = Convert.ToString(r.Item("Purchasing_Code"))
            End If
            If IsDBNull(r.Item("Product_Group_Code")) Then
                m_Product_Group_Code = Nothing
            Else
                m_Product_Group_Code = Convert.ToString(r.Item("Product_Group_Code"))
            End If
            If IsDBNull(r.Item("Requested_Delivery_Date")) OrElse Not IsDate(r.Item("Requested_Delivery_Date")) Then
                m_Requested_Delivery_Date = Nothing
            Else
                m_Requested_Delivery_Date = Convert.ToDateTime(r.Item("Requested_Delivery_Date"))
            End If
            If IsDBNull(r.Item("Promised_Delivery_Date")) OrElse Not IsDate(r.Item("Promised_Delivery_Date")) Then
                m_Promised_Delivery_Date = Nothing
            Else
                m_Promised_Delivery_Date = Convert.ToDateTime(r.Item("Promised_Delivery_Date"))
            End If
            If IsDBNull(r.Item("Shipping_Time")) Then
                m_Shipping_Time = Nothing
            Else
                m_Shipping_Time = Convert.ToString(r.Item("Shipping_Time"))
            End If
            If IsDBNull(r.Item("Outbound_Whse_Handling_Time")) Then
                m_Outbound_Whse_Handling_Time = Nothing
            Else
                m_Outbound_Whse_Handling_Time = Convert.ToString(r.Item("Outbound_Whse_Handling_Time"))
            End If
            If IsDBNull(r.Item("Planned_Delivery_Date")) OrElse Not IsDate(r.Item("Planned_Delivery_Date")) Then
                m_Planned_Delivery_Date = Nothing
            Else
                m_Planned_Delivery_Date = Convert.ToDateTime(r.Item("Planned_Delivery_Date"))
            End If
            If IsDBNull(r.Item("Planned_Shipment_Date")) OrElse Not IsDate(r.Item("Planned_Shipment_Date")) Then
                m_Planned_Shipment_Date = Nothing
            Else
                m_Planned_Shipment_Date = Convert.ToDateTime(r.Item("Planned_Shipment_Date"))
            End If
            If IsDBNull(r.Item("Allow_Line_Disc")) Then
                m_Allow_Line_Disc = Nothing
            Else
                m_Allow_Line_Disc = Convert.ToString(r.Item("Allow_Line_Disc"))
            End If
            If IsDBNull(r.Item("Customer_Disc_Group")) Then
                m_Customer_Disc_Group = Nothing
            Else
                m_Customer_Disc_Group = Convert.ToString(r.Item("Customer_Disc_Group"))
            End If
            If IsDBNull(r.Item("Package_Tracking_No")) Then
                m_Package_Tracking_No = Nothing
            Else
                m_Package_Tracking_No = Convert.ToString(r.Item("Package_Tracking_No"))
            End If
            m_ID = Convert.ToInt32(r.Item("ID"))
            m_Web_Order_Qty = Convert.ToInt32(r.Item("Web_Order_Qty"))
            If IsDBNull(r.Item("Total_Qty_Shipped")) Then
                m_Total_Qty_Shipped = Nothing
            Else
                m_Total_Qty_Shipped = Convert.ToInt32(r.Item("Total_Qty_Shipped"))
            End If
            m_Web_Reference_No = Convert.ToString(r.Item("Web_Reference_No"))
        End Sub 'Load
    End Class

    Public Class NavisionOrderShipmentLinesCollection
        Inherits GenericCollection(Of NavisionOrderShipmentLinesRow)
    End Class

End Namespace


