Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionOrderLinesRow
        Inherits NavisionOrderLinesRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionOrderLinesCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New NavisionOrderLinesCollection
            Try
                Dim row As NavisionOrderLinesRow
                Dim SQL As String = "select * from _NAVISION_ORDER_LINES where Line_Type in ('Item','G/L Accoun','G/L Account','Charge (Item)') and Document_Type = 'Order' and ltrim(rtrim(Order_Line_No)) <> ''"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionOrderLinesRow(DB)
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

    Public MustInherit Class NavisionOrderLinesRowBase
        Private m_DB As Database
        Private m_Document_Type As String = Nothing
        Private m_Sell_To_Customer_No As String = Nothing
        Private m_Document_No As String = Nothing
        Private m_Line_No As Integer = Nothing
        Private m_Line_Type As String = Nothing
        Private m_Order_Line_No As String = Nothing
        Private m_Location_Code As String = Nothing
        Private m_Shipment_Date As DateTime = Nothing
        Private m_Description As String = Nothing
        Private m_Description_2 As String = Nothing
        Private m_Unit_of_Measure As String = Nothing
        Private m_Quantity As Double = Nothing
        Private m_Outstanding_Quantity As Double = Nothing
        Private m_Qty_to_Invoice As Double = Nothing
        Private m_Qty_to_Ship As Double = Nothing
        Private m_Unit_Price As Double = Nothing
        Private m_Unit_Cost_LCY As Double = Nothing
        Private m_VAT_Percent As Double = Nothing
        Private m_Line_Discount_Percent As Double = Nothing
        Private m_Line_Discount_Amount As Double = Nothing
        Private m_Amount As Double = Nothing
        Private m_Amount_Including_VAT As Double = Nothing
        Private m_Allow_Invoice_Disc As String = Nothing
        Private m_Gross_Weight As Double = Nothing
        Private m_Net_Weight As Double = Nothing
        Private m_Units_per_Parcel As Double = Nothing
        Private m_Unit_Volume As Double = Nothing
        Private m_Customer_Price_Group As String = Nothing
        Private m_Outstanding_Amount As Double = Nothing
        Private m_Qty_Shipped_Not_Invoiced As Double = Nothing
        Private m_Shipped_Not_Invoiced As Double = Nothing
        Private m_Quantity_Shipped As Double = Nothing
        Private m_Quantity_Invoiced As Double = Nothing
        Private m_Shipment_No As String = Nothing
        Private m_Shipment_Line_No As Integer = Nothing
        Private m_Profit_Percent As Double = Nothing
        Private m_Bill_to_Customer_No As String = Nothing
        Private m_Inv_Discount_Amount As Double = Nothing
        Private m_Drop_Shipment As String = Nothing
        Private m_VAT_Calculation_Type As String = Nothing
        Private m_Tax_Area_Code As String = Nothing
        Private m_Tax_Liable As String = Nothing
        Private m_Tax_Group_Code As String = Nothing
        Private m_Currency_Code As String = Nothing
        Private m_Outstanding_Amount_LCY As Double = Nothing
        Private m_Shipped_Not_Invoiced_LCY As Double = Nothing
        Private m_VAT_Base_Amount As Double = Nothing
        Private m_Unit_Cost As Double = Nothing
        Private m_System_Created_Entry As String = Nothing
        Private m_Line_Amount As Double = Nothing
        Private m_VAT_Difference As Double = Nothing
        Private m_Inv_Disc_Amount_to_Invoice As Double = Nothing
        Private m_VAT_Identifier As String = Nothing
        Private m_Variant_Code As String = Nothing
        Private m_Bin_Code As String = Nothing
        Private m_Qty_per_Unit_of_Measure As Double = Nothing
        Private m_Planned As String = Nothing
        Private m_Unit_of_Measure_Code As String = Nothing
        Private m_Quantity_Base As Double = Nothing
        Private m_Outstanding_Qty_Base As Double = Nothing
        Private m_Qty_to_Invoice_Base As Double = Nothing
        Private m_Qty_to_Ship_Base As Double = Nothing
        Private m_Qty_Shipped_Not_Invd_Base As Double = Nothing
        Private m_Qty_Shipped_Base As Double = Nothing
        Private m_Qty_Invoiced_Base As Double = Nothing
        Private m_Use_Duplication_List As String = Nothing
        Private m_Responsibility_Center As String = Nothing
        Private m_Out_of_Stock_Substitution As String = Nothing
        Private m_Substitution_Available As String = Nothing
        Private m_Originally_Ordered_No As String = Nothing
        Private m_Originally_Ordered_Var_Code As String = Nothing
        Private m_Cross_Reference_No As String = Nothing
        Private m_Unit_of_Measure_Cross_Ref As String = Nothing
        Private m_Cross_Reference_Type As String = Nothing
        Private m_Cross_Reference_Type_No As String = Nothing
        Private m_Item_Category_Code As String = Nothing
        Private m_Nonstock As String = Nothing
        Private m_Purchasing_Code As String = Nothing
        Private m_Product_Group_Code As String = Nothing
        Private m_Special_Order As String = Nothing
        Private m_Special_Order_Purchase_No As String = Nothing
        Private m_Special_Order_Purch_Line_No As Integer = Nothing
        Private m_Completely_Shipped As String = Nothing
        Private m_Requested_Delivery_Date As DateTime = Nothing
        Private m_Promised_Delivery_Date As DateTime = Nothing
        Private m_Shipping_Time As String = Nothing
        Private m_Outbound_Whse_Handling_Time As String = Nothing
        Private m_Planned_Delivery_Date As DateTime = Nothing
        Private m_Planned_Shipment_Date As DateTime = Nothing
        Private m_Shipping_Agent_Code As String = Nothing
        Private m_Shipping_Agent_Service_Code As String = Nothing
        Private m_BOM_Item_No As String = Nothing
        Private m_Allow_Line_Disc As String = Nothing
        Private m_Customer_Disc_Group As String = Nothing
        Private m_Current_Cust_Price_Group As String = Nothing
        Private m_Offer_No As String = Nothing
        Private m_WebReferenceNo As Integer = Nothing
        Private m_ID As Integer = Nothing


        Public Property Document_Type() As String
            Get
                Return Trim(m_Document_Type)
            End Get
            Set(ByVal Value As String)
                m_Document_Type = Trim(Value)
            End Set
        End Property

        Public Property Sell_To_Customer_No() As String
            Get
                Return Trim(m_Sell_To_Customer_No)
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Customer_No = Trim(Value)
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

        Public Property Line_Type() As String
            Get
                Return Trim(m_Line_Type)
            End Get
            Set(ByVal Value As String)
                m_Line_Type = Trim(Value)
            End Set
        End Property

        Public Property Order_Line_No() As String
            Get
                Return Trim(m_Order_Line_No)
            End Get
            Set(ByVal Value As String)
                m_Order_Line_No = Trim(Value)
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

        Public Property Outstanding_Quantity() As Double
            Get
                Return Trim(m_Outstanding_Quantity)
            End Get
            Set(ByVal Value As Double)
                m_Outstanding_Quantity = Trim(Value)
            End Set
        End Property

        Public Property Qty_to_Invoice() As Double
            Get
                Return Trim(m_Qty_to_Invoice)
            End Get
            Set(ByVal Value As Double)
                m_Qty_to_Invoice = Trim(Value)
            End Set
        End Property

        Public Property Qty_to_Ship() As Double
            Get
                Return Trim(m_Qty_to_Ship)
            End Get
            Set(ByVal Value As Double)
                m_Qty_to_Ship = Trim(Value)
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

        Public Property Unit_Cost_LCY() As Double
            Get
                Return Trim(m_Unit_Cost_LCY)
            End Get
            Set(ByVal Value As Double)
                m_Unit_Cost_LCY = Trim(Value)
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

        Public Property Line_Discount_Amount() As Double
            Get
                Return Trim(m_Line_Discount_Amount)
            End Get
            Set(ByVal Value As Double)
                m_Line_Discount_Amount = Trim(Value)
            End Set
        End Property

        Public Property Amount() As Double
            Get
                Return Trim(m_Amount)
            End Get
            Set(ByVal Value As Double)
                m_Amount = Trim(Value)
            End Set
        End Property

        Public Property Amount_Including_VAT() As Double
            Get
                Return Trim(m_Amount_Including_VAT)
            End Get
            Set(ByVal Value As Double)
                m_Amount_Including_VAT = Trim(Value)
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

        Public Property Customer_Price_Group() As String
            Get
                Return Trim(m_Customer_Price_Group)
            End Get
            Set(ByVal Value As String)
                m_Customer_Price_Group = Trim(Value)
            End Set
        End Property

        Public Property Outstanding_Amount() As Double
            Get
                Return Trim(m_Outstanding_Amount)
            End Get
            Set(ByVal Value As Double)
                m_Outstanding_Amount = Trim(Value)
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

        Public Property Shipped_Not_Invoiced() As Double
            Get
                Return Trim(m_Shipped_Not_Invoiced)
            End Get
            Set(ByVal Value As Double)
                m_Shipped_Not_Invoiced = Trim(Value)
            End Set
        End Property

        Public Property Quantity_Shipped() As Double
            Get
                Return Trim(m_Quantity_Shipped)
            End Get
            Set(ByVal Value As Double)
                m_Quantity_Shipped = Trim(Value)
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

        Public Property Shipment_No() As String
            Get
                Return Trim(m_Shipment_No)
            End Get
            Set(ByVal Value As String)
                m_Shipment_No = Trim(Value)
            End Set
        End Property

        Public Property Shipment_Line_No() As Integer
            Get
                Return Trim(m_Shipment_Line_No)
            End Get
            Set(ByVal Value As Integer)
                m_Shipment_Line_No = Trim(Value)
            End Set
        End Property

        Public Property Profit_Percent() As Double
            Get
                Return Trim(m_Profit_Percent)
            End Get
            Set(ByVal Value As Double)
                m_Profit_Percent = Trim(Value)
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

        Public Property Inv_Discount_Amount() As Double
            Get
                Return Trim(m_Inv_Discount_Amount)
            End Get
            Set(ByVal Value As Double)
                m_Inv_Discount_Amount = Trim(Value)
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

        Public Property VAT_Calculation_Type() As String
            Get
                Return Trim(m_VAT_Calculation_Type)
            End Get
            Set(ByVal Value As String)
                m_VAT_Calculation_Type = Trim(Value)
            End Set
        End Property

        Public Property Tax_Area_Code() As String
            Get
                Return Trim(m_Tax_Area_Code)
            End Get
            Set(ByVal Value As String)
                m_Tax_Area_Code = Trim(Value)
            End Set
        End Property

        Public Property Tax_Liable() As String
            Get
                Return Trim(m_Tax_Liable)
            End Get
            Set(ByVal Value As String)
                m_Tax_Liable = Trim(Value)
            End Set
        End Property

        Public Property Tax_Group_Code() As String
            Get
                Return Trim(m_Tax_Group_Code)
            End Get
            Set(ByVal Value As String)
                m_Tax_Group_Code = Trim(Value)
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

        Public Property Outstanding_Amount_LCY() As Double
            Get
                Return Trim(m_Outstanding_Amount_LCY)
            End Get
            Set(ByVal Value As Double)
                m_Outstanding_Amount_LCY = Trim(Value)
            End Set
        End Property

        Public Property Shipped_Not_Invoiced_LCY() As Double
            Get
                Return Trim(m_Shipped_Not_Invoiced_LCY)
            End Get
            Set(ByVal Value As Double)
                m_Shipped_Not_Invoiced_LCY = Trim(Value)
            End Set
        End Property

        Public Property VAT_Base_Amount() As Double
            Get
                Return Trim(m_VAT_Base_Amount)
            End Get
            Set(ByVal Value As Double)
                m_VAT_Base_Amount = Trim(Value)
            End Set
        End Property

        Public Property Unit_Cost() As Double
            Get
                Return Trim(m_Unit_Cost)
            End Get
            Set(ByVal Value As Double)
                m_Unit_Cost = Trim(Value)
            End Set
        End Property

        Public Property System_Created_Entry() As String
            Get
                Return Trim(m_System_Created_Entry)
            End Get
            Set(ByVal Value As String)
                m_System_Created_Entry = Trim(Value)
            End Set
        End Property

        Public Property Line_Amount() As Double
            Get
                Return Trim(m_Line_Amount)
            End Get
            Set(ByVal Value As Double)
                m_Line_Amount = Trim(Value)
            End Set
        End Property

        Public Property VAT_Difference() As Double
            Get
                Return Trim(m_VAT_Difference)
            End Get
            Set(ByVal Value As Double)
                m_VAT_Difference = Trim(Value)
            End Set
        End Property

        Public Property Inv_Disc_Amount_to_Invoice() As Double
            Get
                Return Trim(m_Inv_Disc_Amount_to_Invoice)
            End Get
            Set(ByVal Value As Double)
                m_Inv_Disc_Amount_to_Invoice = Trim(Value)
            End Set
        End Property

        Public Property VAT_Identifier() As String
            Get
                Return Trim(m_VAT_Identifier)
            End Get
            Set(ByVal Value As String)
                m_VAT_Identifier = Trim(Value)
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

        Public Property Planned() As String
            Get
                Return Trim(m_Planned)
            End Get
            Set(ByVal Value As String)
                m_Planned = Trim(Value)
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

        Public Property Outstanding_Qty_Base() As Double
            Get
                Return Trim(m_Outstanding_Qty_Base)
            End Get
            Set(ByVal Value As Double)
                m_Outstanding_Qty_Base = Trim(Value)
            End Set
        End Property

        Public Property Qty_to_Invoice_Base() As Double
            Get
                Return Trim(m_Qty_to_Invoice_Base)
            End Get
            Set(ByVal Value As Double)
                m_Qty_to_Invoice_Base = Trim(Value)
            End Set
        End Property

        Public Property Qty_to_Ship_Base() As Double
            Get
                Return Trim(m_Qty_to_Ship_Base)
            End Get
            Set(ByVal Value As Double)
                m_Qty_to_Ship_Base = Trim(Value)
            End Set
        End Property

        Public Property Qty_Shipped_Not_Invd_Base() As Double
            Get
                Return Trim(m_Qty_Shipped_Not_Invd_Base)
            End Get
            Set(ByVal Value As Double)
                m_Qty_Shipped_Not_Invd_Base = Trim(Value)
            End Set
        End Property

        Public Property Qty_Shipped_Base() As Double
            Get
                Return Trim(m_Qty_Shipped_Base)
            End Get
            Set(ByVal Value As Double)
                m_Qty_Shipped_Base = Trim(Value)
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

        Public Property Use_Duplication_List() As String
            Get
                Return Trim(m_Use_Duplication_List)
            End Get
            Set(ByVal Value As String)
                m_Use_Duplication_List = Trim(Value)
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

        Public Property Out_of_Stock_Substitution() As String
            Get
                Return Trim(m_Out_of_Stock_Substitution)
            End Get
            Set(ByVal Value As String)
                m_Out_of_Stock_Substitution = Trim(Value)
            End Set
        End Property

        Public Property Substitution_Available() As String
            Get
                Return Trim(m_Substitution_Available)
            End Get
            Set(ByVal Value As String)
                m_Substitution_Available = Trim(Value)
            End Set
        End Property

        Public Property Originally_Ordered_No() As String
            Get
                Return Trim(m_Originally_Ordered_No)
            End Get
            Set(ByVal Value As String)
                m_Originally_Ordered_No = Trim(Value)
            End Set
        End Property

        Public Property Originally_Ordered_Var_Code() As String
            Get
                Return Trim(m_Originally_Ordered_Var_Code)
            End Get
            Set(ByVal Value As String)
                m_Originally_Ordered_Var_Code = Trim(Value)
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

        Public Property Special_Order() As String
            Get
                Return Trim(m_Special_Order)
            End Get
            Set(ByVal Value As String)
                m_Special_Order = Trim(Value)
            End Set
        End Property

        Public Property Special_Order_Purchase_No() As String
            Get
                Return Trim(m_Special_Order_Purchase_No)
            End Get
            Set(ByVal Value As String)
                m_Special_Order_Purchase_No = Trim(Value)
            End Set
        End Property

        Public Property Special_Order_Purch_Line_No() As Integer
            Get
                Return Trim(m_Special_Order_Purch_Line_No)
            End Get
            Set(ByVal Value As Integer)
                m_Special_Order_Purch_Line_No = Trim(Value)
            End Set
        End Property

        Public Property Completely_Shipped() As String
            Get
                Return Trim(m_Completely_Shipped)
            End Get
            Set(ByVal Value As String)
                m_Completely_Shipped = Trim(Value)
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

        Public Property Shipping_Agent_Code() As String
            Get
                Return Trim(m_Shipping_Agent_Code)
            End Get
            Set(ByVal Value As String)
                m_Shipping_Agent_Code = Trim(Value)
            End Set
        End Property

        Public Property Shipping_Agent_Service_Code() As String
            Get
                Return Trim(m_Shipping_Agent_Service_Code)
            End Get
            Set(ByVal Value As String)
                m_Shipping_Agent_Service_Code = Trim(Value)
            End Set
        End Property

        Public Property BOM_Item_No() As String
            Get
                Return Trim(m_BOM_Item_No)
            End Get
            Set(ByVal Value As String)
                m_BOM_Item_No = Trim(Value)
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

        Public Property Current_Cust_Price_Group() As String
            Get
                Return Trim(m_Current_Cust_Price_Group)
            End Get
            Set(ByVal Value As String)
                m_Current_Cust_Price_Group = Trim(Value)
            End Set
        End Property

        Public Property Offer_No() As String
            Get
                Return Trim(m_Offer_No)
            End Get
            Set(ByVal Value As String)
                m_Offer_No = Trim(Value)
            End Set
        End Property

        Public Property WebReferenceNo() As Integer
            Get
                Return Trim(m_WebReferenceNo)
            End Get
            Set(ByVal Value As Integer)
                m_WebReferenceNo = Trim(Value)
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
            If IsDBNull(r.Item("Document_Type")) Then
                m_Document_Type = Nothing
            Else
                m_Document_Type = Convert.ToString(r.Item("Document_Type"))
            End If
            If IsDBNull(r.Item("Sell_To_Customer_No")) Then
                m_Sell_To_Customer_No = Nothing
            Else
                m_Sell_To_Customer_No = Convert.ToString(r.Item("Sell_To_Customer_No"))
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
            If IsDBNull(r.Item("Line_Type")) Then
                m_Line_Type = Nothing
            Else
                m_Line_Type = Convert.ToString(r.Item("Line_Type"))
            End If
            If IsDBNull(r.Item("Order_Line_No")) Then
                m_Order_Line_No = Nothing
            Else
                m_Order_Line_No = Convert.ToString(r.Item("Order_Line_No"))
            End If
            If IsDBNull(r.Item("Location_Code")) Then
                m_Location_Code = Nothing
            Else
                m_Location_Code = Convert.ToString(r.Item("Location_Code"))
            End If
            If IsDBNull(r.Item("Shipment_Date")) Then
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
            If IsDBNull(r.Item("Outstanding_Quantity")) Then
                m_Outstanding_Quantity = Nothing
            Else
                m_Outstanding_Quantity = Convert.ToDouble(r.Item("Outstanding_Quantity"))
            End If
            If IsDBNull(r.Item("Qty_to_Invoice")) Then
                m_Qty_to_Invoice = Nothing
            Else
                m_Qty_to_Invoice = Convert.ToDouble(r.Item("Qty_to_Invoice"))
            End If
            If IsDBNull(r.Item("Qty_to_Ship")) Then
                m_Qty_to_Ship = Nothing
            Else
                m_Qty_to_Ship = Convert.ToDouble(r.Item("Qty_to_Ship"))
            End If
            If IsDBNull(r.Item("Unit_Price")) Then
                m_Unit_Price = Nothing
            Else
                m_Unit_Price = Convert.ToDouble(r.Item("Unit_Price"))
            End If
            If IsDBNull(r.Item("Unit_Cost_LCY")) Then
                m_Unit_Cost_LCY = Nothing
            Else
                m_Unit_Cost_LCY = Convert.ToDouble(r.Item("Unit_Cost_LCY"))
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
            If IsDBNull(r.Item("Line_Discount_Amount")) Then
                m_Line_Discount_Amount = Nothing
            Else
                m_Line_Discount_Amount = Convert.ToDouble(r.Item("Line_Discount_Amount"))
            End If
            If IsDBNull(r.Item("Amount")) Then
                m_Amount = Nothing
            Else
                m_Amount = Convert.ToDouble(r.Item("Amount"))
            End If
            If IsDBNull(r.Item("Amount_Including_VAT")) Then
                m_Amount_Including_VAT = Nothing
            Else
                m_Amount_Including_VAT = Convert.ToDouble(r.Item("Amount_Including_VAT"))
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
            If IsDBNull(r.Item("Customer_Price_Group")) Then
                m_Customer_Price_Group = Nothing
            Else
                m_Customer_Price_Group = Convert.ToString(r.Item("Customer_Price_Group"))
            End If
            If IsDBNull(r.Item("Outstanding_Amount")) Then
                m_Outstanding_Amount = Nothing
            Else
                m_Outstanding_Amount = Convert.ToDouble(r.Item("Outstanding_Amount"))
            End If
            If IsDBNull(r.Item("Qty_Shipped_Not_Invoiced")) Then
                m_Qty_Shipped_Not_Invoiced = Nothing
            Else
                m_Qty_Shipped_Not_Invoiced = Convert.ToDouble(r.Item("Qty_Shipped_Not_Invoiced"))
            End If
            If IsDBNull(r.Item("Shipped_Not_Invoiced")) Then
                m_Shipped_Not_Invoiced = Nothing
            Else
                m_Shipped_Not_Invoiced = Convert.ToDouble(r.Item("Shipped_Not_Invoiced"))
            End If
            If IsDBNull(r.Item("Quantity_Shipped")) Then
                m_Quantity_Shipped = Nothing
            Else
                m_Quantity_Shipped = Convert.ToDouble(r.Item("Quantity_Shipped"))
            End If
            If IsDBNull(r.Item("Quantity_Invoiced")) Then
                m_Quantity_Invoiced = Nothing
            Else
                m_Quantity_Invoiced = Convert.ToDouble(r.Item("Quantity_Invoiced"))
            End If
            If IsDBNull(r.Item("Shipment_No")) Then
                m_Shipment_No = Nothing
            Else
                m_Shipment_No = Convert.ToString(r.Item("Shipment_No"))
            End If
            If IsDBNull(r.Item("Shipment_Line_No")) Then
                m_Shipment_Line_No = Nothing
            Else
                m_Shipment_Line_No = Convert.ToInt32(r.Item("Shipment_Line_No"))
            End If
            If IsDBNull(r.Item("Profit_Percent")) Then
                m_Profit_Percent = Nothing
            Else
                m_Profit_Percent = Convert.ToDouble(r.Item("Profit_Percent"))
            End If
            If IsDBNull(r.Item("Bill_to_Customer_No")) Then
                m_Bill_to_Customer_No = Nothing
            Else
                m_Bill_to_Customer_No = Convert.ToString(r.Item("Bill_to_Customer_No"))
            End If
            If IsDBNull(r.Item("Inv_Discount_Amount")) Then
                m_Inv_Discount_Amount = Nothing
            Else
                m_Inv_Discount_Amount = Convert.ToDouble(r.Item("Inv_Discount_Amount"))
            End If
            If IsDBNull(r.Item("Drop_Shipment")) Then
                m_Drop_Shipment = Nothing
            Else
                m_Drop_Shipment = Convert.ToString(r.Item("Drop_Shipment"))
            End If
            If IsDBNull(r.Item("VAT_Calculation_Type")) Then
                m_VAT_Calculation_Type = Nothing
            Else
                m_VAT_Calculation_Type = Convert.ToString(r.Item("VAT_Calculation_Type"))
            End If
            If IsDBNull(r.Item("Tax_Area_Code")) Then
                m_Tax_Area_Code = Nothing
            Else
                m_Tax_Area_Code = Convert.ToString(r.Item("Tax_Area_Code"))
            End If
            If IsDBNull(r.Item("Tax_Liable")) Then
                m_Tax_Liable = Nothing
            Else
                m_Tax_Liable = Convert.ToString(r.Item("Tax_Liable"))
            End If
            If IsDBNull(r.Item("Tax_Group_Code")) Then
                m_Tax_Group_Code = Nothing
            Else
                m_Tax_Group_Code = Convert.ToString(r.Item("Tax_Group_Code"))
            End If
            If IsDBNull(r.Item("Currency_Code")) Then
                m_Currency_Code = Nothing
            Else
                m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
            End If
            If IsDBNull(r.Item("Outstanding_Amount_LCY")) Then
                m_Outstanding_Amount_LCY = Nothing
            Else
                m_Outstanding_Amount_LCY = Convert.ToDouble(r.Item("Outstanding_Amount_LCY"))
            End If
            If IsDBNull(r.Item("Shipped_Not_Invoiced_LCY")) Then
                m_Shipped_Not_Invoiced_LCY = Nothing
            Else
                m_Shipped_Not_Invoiced_LCY = Convert.ToDouble(r.Item("Shipped_Not_Invoiced_LCY"))
            End If
            If IsDBNull(r.Item("VAT_Base_Amount")) Then
                m_VAT_Base_Amount = Nothing
            Else
                m_VAT_Base_Amount = Convert.ToDouble(r.Item("VAT_Base_Amount"))
            End If
            If IsDBNull(r.Item("Unit_Cost")) Then
                m_Unit_Cost = Nothing
            Else
                m_Unit_Cost = Convert.ToDouble(r.Item("Unit_Cost"))
            End If
            If IsDBNull(r.Item("System_Created_Entry")) Then
                m_System_Created_Entry = Nothing
            Else
                m_System_Created_Entry = Convert.ToString(r.Item("System_Created_Entry"))
            End If
            If IsDBNull(r.Item("Line_Amount")) Then
                m_Line_Amount = Nothing
            Else
                m_Line_Amount = Convert.ToDouble(r.Item("Line_Amount"))
            End If
            If IsDBNull(r.Item("VAT_Difference")) Then
                m_VAT_Difference = Nothing
            Else
                m_VAT_Difference = Convert.ToDouble(r.Item("VAT_Difference"))
            End If
            If IsDBNull(r.Item("Inv_Disc_Amount_to_Invoice")) Then
                m_Inv_Disc_Amount_to_Invoice = Nothing
            Else
                m_Inv_Disc_Amount_to_Invoice = Convert.ToDouble(r.Item("Inv_Disc_Amount_to_Invoice"))
            End If
            If IsDBNull(r.Item("VAT_Identifier")) Then
                m_VAT_Identifier = Nothing
            Else
                m_VAT_Identifier = Convert.ToString(r.Item("VAT_Identifier"))
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
            If IsDBNull(r.Item("Planned")) Then
                m_Planned = Nothing
            Else
                m_Planned = Convert.ToString(r.Item("Planned"))
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
            If IsDBNull(r.Item("Outstanding_Qty_Base")) Then
                m_Outstanding_Qty_Base = Nothing
            Else
                m_Outstanding_Qty_Base = Convert.ToDouble(r.Item("Outstanding_Qty_Base"))
            End If
            If IsDBNull(r.Item("Qty_to_Invoice_Base")) Then
                m_Qty_to_Invoice_Base = Nothing
            Else
                m_Qty_to_Invoice_Base = Convert.ToDouble(r.Item("Qty_to_Invoice_Base"))
            End If
            If IsDBNull(r.Item("Qty_to_Ship_Base")) Then
                m_Qty_to_Ship_Base = Nothing
            Else
                m_Qty_to_Ship_Base = Convert.ToDouble(r.Item("Qty_to_Ship_Base"))
            End If
            If IsDBNull(r.Item("Qty_Shipped_Not_Invd_Base")) Then
                m_Qty_Shipped_Not_Invd_Base = Nothing
            Else
                m_Qty_Shipped_Not_Invd_Base = Convert.ToDouble(r.Item("Qty_Shipped_Not_Invd_Base"))
            End If
            If IsDBNull(r.Item("Qty_Shipped_Base")) Then
                m_Qty_Shipped_Base = Nothing
            Else
                m_Qty_Shipped_Base = Convert.ToDouble(r.Item("Qty_Shipped_Base"))
            End If
            If IsDBNull(r.Item("Qty_Invoiced_Base")) Then
                m_Qty_Invoiced_Base = Nothing
            Else
                m_Qty_Invoiced_Base = Convert.ToDouble(r.Item("Qty_Invoiced_Base"))
            End If
            If IsDBNull(r.Item("Use_Duplication_List")) Then
                m_Use_Duplication_List = Nothing
            Else
                m_Use_Duplication_List = Convert.ToString(r.Item("Use_Duplication_List"))
            End If
            If IsDBNull(r.Item("Responsibility_Center")) Then
                m_Responsibility_Center = Nothing
            Else
                m_Responsibility_Center = Convert.ToString(r.Item("Responsibility_Center"))
            End If
            If IsDBNull(r.Item("Out_of_Stock_Substitution")) Then
                m_Out_of_Stock_Substitution = Nothing
            Else
                m_Out_of_Stock_Substitution = Convert.ToString(r.Item("Out_of_Stock_Substitution"))
            End If
            If IsDBNull(r.Item("Substitution_Available")) Then
                m_Substitution_Available = Nothing
            Else
                m_Substitution_Available = Convert.ToString(r.Item("Substitution_Available"))
            End If
            If IsDBNull(r.Item("Originally_Ordered_No")) Then
                m_Originally_Ordered_No = Nothing
            Else
                m_Originally_Ordered_No = Convert.ToString(r.Item("Originally_Ordered_No"))
            End If
            If IsDBNull(r.Item("Originally_Ordered_Var_Code")) Then
                m_Originally_Ordered_Var_Code = Nothing
            Else
                m_Originally_Ordered_Var_Code = Convert.ToString(r.Item("Originally_Ordered_Var_Code"))
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
            If IsDBNull(r.Item("Special_Order")) Then
                m_Special_Order = Nothing
            Else
                m_Special_Order = Convert.ToString(r.Item("Special_Order"))
            End If
            If IsDBNull(r.Item("Special_Order_Purchase_No")) Then
                m_Special_Order_Purchase_No = Nothing
            Else
                m_Special_Order_Purchase_No = Convert.ToString(r.Item("Special_Order_Purchase_No"))
            End If
            If IsDBNull(r.Item("Special_Order_Purch_Line_No")) Then
                m_Special_Order_Purch_Line_No = Nothing
            Else
                m_Special_Order_Purch_Line_No = Convert.ToInt32(r.Item("Special_Order_Purch_Line_No"))
            End If
            If IsDBNull(r.Item("Completely_Shipped")) Then
                m_Completely_Shipped = Nothing
            Else
                m_Completely_Shipped = Convert.ToString(r.Item("Completely_Shipped"))
            End If
            If IsDBNull(r.Item("Requested_Delivery_Date")) Then
                m_Requested_Delivery_Date = Nothing
            Else
                m_Requested_Delivery_Date = Convert.ToDateTime(r.Item("Requested_Delivery_Date"))
            End If
            If IsDBNull(r.Item("Promised_Delivery_Date")) Then
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
            If IsDBNull(r.Item("Planned_Delivery_Date")) Then
                m_Planned_Delivery_Date = Nothing
            Else
                m_Planned_Delivery_Date = Convert.ToDateTime(r.Item("Planned_Delivery_Date"))
            End If
            If IsDBNull(r.Item("Planned_Shipment_Date")) Then
                m_Planned_Shipment_Date = Nothing
            Else
                m_Planned_Shipment_Date = Convert.ToDateTime(r.Item("Planned_Shipment_Date"))
            End If
            If IsDBNull(r.Item("Shipping_Agent_Code")) Then
                m_Shipping_Agent_Code = Nothing
            Else
                m_Shipping_Agent_Code = Convert.ToString(r.Item("Shipping_Agent_Code"))
            End If
            If IsDBNull(r.Item("Shipping_Agent_Service_Code")) Then
                m_Shipping_Agent_Service_Code = Nothing
            Else
                m_Shipping_Agent_Service_Code = Convert.ToString(r.Item("Shipping_Agent_Service_Code"))
            End If
            If IsDBNull(r.Item("BOM_Item_No")) Then
                m_BOM_Item_No = Nothing
            Else
                m_BOM_Item_No = Convert.ToString(r.Item("BOM_Item_No"))
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
            If IsDBNull(r.Item("Current_Cust_Price_Group")) Then
                m_Current_Cust_Price_Group = Nothing
            Else
                m_Current_Cust_Price_Group = Convert.ToString(r.Item("Current_Cust_Price_Group"))
            End If
            If IsDBNull(r.Item("Offer_No")) Then
                m_Offer_No = Nothing
            Else
                m_Offer_No = Convert.ToString(r.Item("Offer_No"))
            End If
            If IsDBNull(r.Item("WebReferenceNo")) OrElse Not IsNumeric(r.Item("WebReferenceNo")) Then
                m_WebReferenceNo = Nothing
            Else
                m_WebReferenceNo = Convert.ToInt32(r.Item("WebReferenceNo"))
            End If
            m_ID = Convert.ToInt32(r.Item("ID"))
        End Sub 'Load
    End Class

    Public Class NavisionOrderLinesCollection
        Inherits GenericCollection(Of NavisionOrderLinesRow)
    End Class

End Namespace


