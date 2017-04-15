Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionSalesCreditMemoLinesRow
        Inherits NavisionSalesCreditMemoLinesRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Shared Function GetCollection(ByVal DB As Database) As NavisionSalesCreditMemoLinesCollection
            Dim r As SqlDataReader = Nothing
            Dim row As NavisionSalesCreditMemoLinesRow
            Dim c As New NavisionSalesCreditMemoLinesCollection
            Dim SQL As String = "select * from _NAVISION_SALES_CREDIT_MEMO_LINES"

            Try
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionSalesCreditMemoLinesRow(DB)
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

    Public MustInherit Class NavisionSalesCreditMemoLinesRowBase
        Private m_DB As Database
        Private m_Sell_To_Customer_No As String = Nothing
        Private m_Document_No As String = Nothing
        Private m_Line_No As Integer = Nothing
        Private m_Type As String = Nothing
        Private m_No As String = Nothing
        Private m_Location_Code As String = Nothing
        Private m_Shipment_Date As DateTime = Nothing
        Private m_Description As String = Nothing
        Private m_Description_2 As String = Nothing
        Private m_Unit_of_Measure As String = Nothing
        Private m_Quantity As Double = Nothing
        Private m_Unit_Price As Double = Nothing
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
        Private m_Appl_To_Item_Entry As Integer = Nothing
        Private m_Job_Applies_To_ID As String = Nothing
        Private m_Work_Type_Code As String = Nothing
        Private m_Bill_To_Customer_No As String = Nothing
        Private m_Inv_Discount_Amount As Double = Nothing
        Private m_Tax_Area_Code As String = Nothing
        Private m_Tax_Liable As String = Nothing
        Private m_Tax_Group_Code As String = Nothing
        Private m_Blanket_Order_No As String = Nothing
        Private m_Blanket_Order_Line_No As Integer = Nothing
        Private m_VAT_Base_Amount As Double = Nothing
        Private m_Unit_Cost As Double = Nothing
        Private m_Line_Amount As Double = Nothing
        Private m_Variant_Code As String = Nothing
        Private m_Bin_Code As String = Nothing
        Private m_Qty_per_Unit_of_Measure As Double = Nothing
        Private m_Unit_of_Measure_Code As String = Nothing
        Private m_Quantity_Base As Double = Nothing
        Private m_Responsibility_Center As String = Nothing
        Private m_Cross_Reference_No As String = Nothing
        Private m_Unit_of_Measure_Cross_Ref As String = Nothing
        Private m_Cross_Reference_Type As String = Nothing
        Private m_Cross_Reference_Type_No As String = Nothing
        Private m_Item_Category_Code As String = Nothing
        Private m_Nonstock As String = Nothing
        Private m_Purchasing_Code As String = Nothing
        Private m_Product_Group_Code As String = Nothing
        Private m_Return_Receipt_No As String = Nothing
        Private m_Return_Receipt_Line_No As Integer = Nothing
        Private m_Return_Reason_Code As String = Nothing
        Private m_Allow_Line_Disc As String = Nothing
        Private m_Customer_Disc_Group As String = Nothing
        Private m_Package_Tracking_No As String = Nothing
        Private m_Id As Integer = Nothing


        Public Property Sell_To_Customer_No() As String
            Get
                Return m_Sell_To_Customer_No
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Customer_No = Value
            End Set
        End Property

        Public Property Document_No() As String
            Get
                Return m_Document_No
            End Get
            Set(ByVal Value As String)
                m_Document_No = Value
            End Set
        End Property

        Public Property Line_No() As Integer
            Get
                Return m_Line_No
            End Get
            Set(ByVal Value As Integer)
                m_Line_No = Value
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

        Public Property No() As String
            Get
                Return m_No
            End Get
            Set(ByVal Value As String)
                m_No = Value
            End Set
        End Property

        Public Property Location_Code() As String
            Get
                Return m_Location_Code
            End Get
            Set(ByVal Value As String)
                m_Location_Code = Value
            End Set
        End Property

        Public Property Shipment_Date() As DateTime
            Get
                Return m_Shipment_Date
            End Get
            Set(ByVal Value As DateTime)
                m_Shipment_Date = Value
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

        Public Property Description_2() As String
            Get
                Return m_Description_2
            End Get
            Set(ByVal Value As String)
                m_Description_2 = Value
            End Set
        End Property

        Public Property Unit_of_Measure() As String
            Get
                Return m_Unit_of_Measure
            End Get
            Set(ByVal Value As String)
                m_Unit_of_Measure = Value
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

        Public Property Unit_Price() As Double
            Get
                Return m_Unit_Price
            End Get
            Set(ByVal Value As Double)
                m_Unit_Price = Value
            End Set
        End Property

        Public Property VAT_Percent() As Double
            Get
                Return m_VAT_Percent
            End Get
            Set(ByVal Value As Double)
                m_VAT_Percent = Value
            End Set
        End Property

        Public Property Line_Discount_Percent() As Double
            Get
                Return m_Line_Discount_Percent
            End Get
            Set(ByVal Value As Double)
                m_Line_Discount_Percent = Value
            End Set
        End Property

        Public Property Line_Discount_Amount() As Double
            Get
                Return m_Line_Discount_Amount
            End Get
            Set(ByVal Value As Double)
                m_Line_Discount_Amount = Value
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

        Public Property Amount_Including_VAT() As Double
            Get
                Return m_Amount_Including_VAT
            End Get
            Set(ByVal Value As Double)
                m_Amount_Including_VAT = Value
            End Set
        End Property

        Public Property Allow_Invoice_Disc() As String
            Get
                Return m_Allow_Invoice_Disc
            End Get
            Set(ByVal Value As String)
                m_Allow_Invoice_Disc = Value
            End Set
        End Property

        Public Property Gross_Weight() As Double
            Get
                Return m_Gross_Weight
            End Get
            Set(ByVal Value As Double)
                m_Gross_Weight = Value
            End Set
        End Property

        Public Property Net_Weight() As Double
            Get
                Return m_Net_Weight
            End Get
            Set(ByVal Value As Double)
                m_Net_Weight = Value
            End Set
        End Property

        Public Property Units_per_Parcel() As Double
            Get
                Return m_Units_per_Parcel
            End Get
            Set(ByVal Value As Double)
                m_Units_per_Parcel = Value
            End Set
        End Property

        Public Property Unit_Volume() As Double
            Get
                Return m_Unit_Volume
            End Get
            Set(ByVal Value As Double)
                m_Unit_Volume = Value
            End Set
        End Property

        Public Property Appl_To_Item_Entry() As Integer
            Get
                Return m_Appl_To_Item_Entry
            End Get
            Set(ByVal Value As Integer)
                m_Appl_To_Item_Entry = Value
            End Set
        End Property

        Public Property Job_Applies_To_ID() As String
            Get
                Return m_Job_Applies_To_ID
            End Get
            Set(ByVal Value As String)
                m_Job_Applies_To_ID = Value
            End Set
        End Property

        Public Property Work_Type_Code() As String
            Get
                Return m_Work_Type_Code
            End Get
            Set(ByVal Value As String)
                m_Work_Type_Code = Value
            End Set
        End Property

        Public Property Bill_To_Customer_No() As String
            Get
                Return m_Bill_To_Customer_No
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Customer_No = Value
            End Set
        End Property

        Public Property Inv_Discount_Amount() As Double
            Get
                Return m_Inv_Discount_Amount
            End Get
            Set(ByVal Value As Double)
                m_Inv_Discount_Amount = Value
            End Set
        End Property

        Public Property Tax_Area_Code() As String
            Get
                Return m_Tax_Area_Code
            End Get
            Set(ByVal Value As String)
                m_Tax_Area_Code = Value
            End Set
        End Property

        Public Property Tax_Liable() As String
            Get
                Return m_Tax_Liable
            End Get
            Set(ByVal Value As String)
                m_Tax_Liable = Value
            End Set
        End Property

        Public Property Tax_Group_Code() As String
            Get
                Return m_Tax_Group_Code
            End Get
            Set(ByVal Value As String)
                m_Tax_Group_Code = Value
            End Set
        End Property

        Public Property Blanket_Order_No() As String
            Get
                Return m_Blanket_Order_No
            End Get
            Set(ByVal Value As String)
                m_Blanket_Order_No = Value
            End Set
        End Property

        Public Property Blanket_Order_Line_No() As Integer
            Get
                Return m_Blanket_Order_Line_No
            End Get
            Set(ByVal Value As Integer)
                m_Blanket_Order_Line_No = Value
            End Set
        End Property

        Public Property VAT_Base_Amount() As Double
            Get
                Return m_VAT_Base_Amount
            End Get
            Set(ByVal Value As Double)
                m_VAT_Base_Amount = Value
            End Set
        End Property

        Public Property Unit_Cost() As Double
            Get
                Return m_Unit_Cost
            End Get
            Set(ByVal Value As Double)
                m_Unit_Cost = Value
            End Set
        End Property

        Public Property Line_Amount() As Double
            Get
                Return m_Line_Amount
            End Get
            Set(ByVal Value As Double)
                m_Line_Amount = Value
            End Set
        End Property

        Public Property Variant_Code() As String
            Get
                Return m_Variant_Code
            End Get
            Set(ByVal Value As String)
                m_Variant_Code = Value
            End Set
        End Property

        Public Property Bin_Code() As String
            Get
                Return m_Bin_Code
            End Get
            Set(ByVal Value As String)
                m_Bin_Code = Value
            End Set
        End Property

        Public Property Qty_per_Unit_of_Measure() As Double
            Get
                Return m_Qty_per_Unit_of_Measure
            End Get
            Set(ByVal Value As Double)
                m_Qty_per_Unit_of_Measure = Value
            End Set
        End Property

        Public Property Unit_of_Measure_Code() As String
            Get
                Return m_Unit_of_Measure_Code
            End Get
            Set(ByVal Value As String)
                m_Unit_of_Measure_Code = Value
            End Set
        End Property

        Public Property Quantity_Base() As Double
            Get
                Return m_Quantity_Base
            End Get
            Set(ByVal Value As Double)
                m_Quantity_Base = Value
            End Set
        End Property

        Public Property Responsibility_Center() As String
            Get
                Return m_Responsibility_Center
            End Get
            Set(ByVal Value As String)
                m_Responsibility_Center = Value
            End Set
        End Property

        Public Property Cross_Reference_No() As String
            Get
                Return m_Cross_Reference_No
            End Get
            Set(ByVal Value As String)
                m_Cross_Reference_No = Value
            End Set
        End Property

        Public Property Unit_of_Measure_Cross_Ref() As String
            Get
                Return m_Unit_of_Measure_Cross_Ref
            End Get
            Set(ByVal Value As String)
                m_Unit_of_Measure_Cross_Ref = Value
            End Set
        End Property

        Public Property Cross_Reference_Type() As String
            Get
                Return m_Cross_Reference_Type
            End Get
            Set(ByVal Value As String)
                m_Cross_Reference_Type = Value
            End Set
        End Property

        Public Property Cross_Reference_Type_No() As String
            Get
                Return m_Cross_Reference_Type_No
            End Get
            Set(ByVal Value As String)
                m_Cross_Reference_Type_No = Value
            End Set
        End Property

        Public Property Item_Category_Code() As String
            Get
                Return m_Item_Category_Code
            End Get
            Set(ByVal Value As String)
                m_Item_Category_Code = Value
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

        Public Property Purchasing_Code() As String
            Get
                Return m_Purchasing_Code
            End Get
            Set(ByVal Value As String)
                m_Purchasing_Code = Value
            End Set
        End Property

        Public Property Product_Group_Code() As String
            Get
                Return m_Product_Group_Code
            End Get
            Set(ByVal Value As String)
                m_Product_Group_Code = Value
            End Set
        End Property

        Public Property Return_Receipt_No() As String
            Get
                Return m_Return_Receipt_No
            End Get
            Set(ByVal Value As String)
                m_Return_Receipt_No = Value
            End Set
        End Property

        Public Property Return_Receipt_Line_No() As Integer
            Get
                Return m_Return_Receipt_Line_No
            End Get
            Set(ByVal Value As Integer)
                m_Return_Receipt_Line_No = Value
            End Set
        End Property

        Public Property Return_Reason_Code() As String
            Get
                Return m_Return_Reason_Code
            End Get
            Set(ByVal Value As String)
                m_Return_Reason_Code = Value
            End Set
        End Property

        Public Property Allow_Line_Disc() As String
            Get
                Return m_Allow_Line_Disc
            End Get
            Set(ByVal Value As String)
                m_Allow_Line_Disc = Value
            End Set
        End Property

        Public Property Customer_Disc_Group() As String
            Get
                Return m_Customer_Disc_Group
            End Get
            Set(ByVal Value As String)
                m_Customer_Disc_Group = Value
            End Set
        End Property

        Public Property Package_Tracking_No() As String
            Get
                Return m_Package_Tracking_No
            End Get
            Set(ByVal Value As String)
                m_Package_Tracking_No = Value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Public Sub New(ByVal DB As Database, ByVal Sell_To_Customer_No As String)
            m_DB = DB
            m_Sell_To_Customer_No = Sell_To_Customer_No
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing

            Dim SQL As String

            SQL = "SELECT * FROM _NAVISION_SALES_CREDIT_MEMO_LINES WHERE Sell_To_Customer_No = " & DB.Number(Sell_To_Customer_No)
            Try
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_Sell_To_Customer_No = Convert.ToString(r.Item("Sell_To_Customer_No"))
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
            If IsDBNull(r.Item("No")) Then
                m_No = Nothing
            Else
                m_No = Convert.ToString(r.Item("No"))
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
            If IsDBNull(r.Item("Appl_To_Item_Entry")) Then
                m_Appl_To_Item_Entry = Nothing
            Else
                m_Appl_To_Item_Entry = Convert.ToInt32(r.Item("Appl_To_Item_Entry"))
            End If
            If IsDBNull(r.Item("Job_Applies_To_ID")) Then
                m_Job_Applies_To_ID = Nothing
            Else
                m_Job_Applies_To_ID = Convert.ToString(r.Item("Job_Applies_To_ID"))
            End If
            If IsDBNull(r.Item("Work_Type_Code")) Then
                m_Work_Type_Code = Nothing
            Else
                m_Work_Type_Code = Convert.ToString(r.Item("Work_Type_Code"))
            End If
            If IsDBNull(r.Item("Bill_To_Customer_No")) Then
                m_Bill_To_Customer_No = Nothing
            Else
                m_Bill_To_Customer_No = Convert.ToString(r.Item("Bill_To_Customer_No"))
            End If
            If IsDBNull(r.Item("Inv_Discount_Amount")) Then
                m_Inv_Discount_Amount = Nothing
            Else
                m_Inv_Discount_Amount = Convert.ToDouble(r.Item("Inv_Discount_Amount"))
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
            If IsDBNull(r.Item("Blanket_Order_No")) Then
                m_Blanket_Order_No = Nothing
            Else
                m_Blanket_Order_No = Convert.ToString(r.Item("Blanket_Order_No"))
            End If
            If IsDBNull(r.Item("Blanket_Order_Line_No")) Then
                m_Blanket_Order_Line_No = Nothing
            Else
                m_Blanket_Order_Line_No = Convert.ToInt32(r.Item("Blanket_Order_Line_No"))
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
            If IsDBNull(r.Item("Line_Amount")) Then
                m_Line_Amount = Nothing
            Else
                m_Line_Amount = Convert.ToDouble(r.Item("Line_Amount"))
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
            If IsDBNull(r.Item("Return_Receipt_No")) Then
                m_Return_Receipt_No = Nothing
            Else
                m_Return_Receipt_No = Convert.ToString(r.Item("Return_Receipt_No"))
            End If
            If IsDBNull(r.Item("Return_Receipt_Line_No")) Then
                m_Return_Receipt_Line_No = Nothing
            Else
                m_Return_Receipt_Line_No = Convert.ToInt32(r.Item("Return_Receipt_Line_No"))
            End If
            If IsDBNull(r.Item("Return_Reason_Code")) Then
                m_Return_Reason_Code = Nothing
            Else
                m_Return_Reason_Code = Convert.ToString(r.Item("Return_Reason_Code"))
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
            m_Id = Convert.ToInt32(r.Item("Id"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO _NAVISION_SALES_CREDIT_MEMO_LINES (" _
             & " Document_No" _
             & ",Line_No" _
             & ",Type" _
             & ",No" _
             & ",Location_Code" _
             & ",Shipment_Date" _
             & ",Description" _
             & ",Description_2" _
             & ",Unit_of_Measure" _
             & ",Quantity" _
             & ",Unit_Price" _
             & ",VAT_Percent" _
             & ",Line_Discount_Percent" _
             & ",Line_Discount_Amount" _
             & ",Amount" _
             & ",Amount_Including_VAT" _
             & ",Allow_Invoice_Disc" _
             & ",Gross_Weight" _
             & ",Net_Weight" _
             & ",Units_per_Parcel" _
             & ",Unit_Volume" _
             & ",Appl_To_Item_Entry" _
             & ",Job_Applies_To_ID" _
             & ",Work_Type_Code" _
             & ",Bill_To_Customer_No" _
             & ",Inv_Discount_Amount" _
             & ",Tax_Area_Code" _
             & ",Tax_Liable" _
             & ",Tax_Group_Code" _
             & ",Blanket_Order_No" _
             & ",Blanket_Order_Line_No" _
             & ",VAT_Base_Amount" _
             & ",Unit_Cost" _
             & ",Line_Amount" _
             & ",Variant_Code" _
             & ",Bin_Code" _
             & ",Qty_per_Unit_of_Measure" _
             & ",Unit_of_Measure_Code" _
             & ",Quantity_Base" _
             & ",Responsibility_Center" _
             & ",Cross_Reference_No" _
             & ",Unit_of_Measure_Cross_Ref" _
             & ",Cross_Reference_Type" _
             & ",Cross_Reference_Type_No" _
             & ",Item_Category_Code" _
             & ",Nonstock" _
             & ",Purchasing_Code" _
             & ",Product_Group_Code" _
             & ",Return_Receipt_No" _
             & ",Return_Receipt_Line_No" _
             & ",Return_Reason_Code" _
             & ",Allow_Line_Disc" _
             & ",Customer_Disc_Group" _
             & ",Package_Tracking_No" _
             & ",Id" _
             & ") VALUES (" _
             & m_DB.Quote(Document_No) _
             & "," & m_DB.Number(Line_No) _
             & "," & m_DB.Quote(Type) _
             & "," & m_DB.Quote(No) _
             & "," & m_DB.Quote(Location_Code) _
             & "," & m_DB.NullQuote(Shipment_Date) _
             & "," & m_DB.Quote(Description) _
             & "," & m_DB.Quote(Description_2) _
             & "," & m_DB.Quote(Unit_of_Measure) _
             & "," & m_DB.Number(Quantity) _
             & "," & m_DB.Number(Unit_Price) _
             & "," & m_DB.Number(VAT_Percent) _
             & "," & m_DB.Number(Line_Discount_Percent) _
             & "," & m_DB.Number(Line_Discount_Amount) _
             & "," & m_DB.Number(Amount) _
             & "," & m_DB.Number(Amount_Including_VAT) _
             & "," & m_DB.Quote(Allow_Invoice_Disc) _
             & "," & m_DB.Number(Gross_Weight) _
             & "," & m_DB.Number(Net_Weight) _
             & "," & m_DB.Number(Units_per_Parcel) _
             & "," & m_DB.Number(Unit_Volume) _
             & "," & m_DB.Number(Appl_To_Item_Entry) _
             & "," & m_DB.Quote(Job_Applies_To_ID) _
             & "," & m_DB.Quote(Work_Type_Code) _
             & "," & m_DB.Quote(Bill_To_Customer_No) _
             & "," & m_DB.Number(Inv_Discount_Amount) _
             & "," & m_DB.Quote(Tax_Area_Code) _
             & "," & m_DB.Quote(Tax_Liable) _
             & "," & m_DB.Quote(Tax_Group_Code) _
             & "," & m_DB.Quote(Blanket_Order_No) _
             & "," & m_DB.Number(Blanket_Order_Line_No) _
             & "," & m_DB.Number(VAT_Base_Amount) _
             & "," & m_DB.Number(Unit_Cost) _
             & "," & m_DB.Number(Line_Amount) _
             & "," & m_DB.Quote(Variant_Code) _
             & "," & m_DB.Quote(Bin_Code) _
             & "," & m_DB.Number(Qty_per_Unit_of_Measure) _
             & "," & m_DB.Quote(Unit_of_Measure_Code) _
             & "," & m_DB.Number(Quantity_Base) _
             & "," & m_DB.Quote(Responsibility_Center) _
             & "," & m_DB.Quote(Cross_Reference_No) _
             & "," & m_DB.Quote(Unit_of_Measure_Cross_Ref) _
             & "," & m_DB.Quote(Cross_Reference_Type) _
             & "," & m_DB.Quote(Cross_Reference_Type_No) _
             & "," & m_DB.Quote(Item_Category_Code) _
             & "," & m_DB.Quote(Nonstock) _
             & "," & m_DB.Quote(Purchasing_Code) _
             & "," & m_DB.Quote(Product_Group_Code) _
             & "," & m_DB.Quote(Return_Receipt_No) _
             & "," & m_DB.Number(Return_Receipt_Line_No) _
             & "," & m_DB.Quote(Return_Reason_Code) _
             & "," & m_DB.Quote(Allow_Line_Disc) _
             & "," & m_DB.Quote(Customer_Disc_Group) _
             & "," & m_DB.Quote(Package_Tracking_No) _
             & "," & m_DB.NullNumber(Id) _
             & ")"

            Sell_To_Customer_No = m_DB.InsertSQL(SQL)

            Return Sell_To_Customer_No
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE _NAVISION_SALES_CREDIT_MEMO_LINES SET " _
             & " Document_No = " & m_DB.Quote(Document_No) _
             & ",Line_No = " & m_DB.Number(Line_No) _
             & ",Type = " & m_DB.Quote(Type) _
             & ",No = " & m_DB.Quote(No) _
             & ",Location_Code = " & m_DB.Quote(Location_Code) _
             & ",Shipment_Date = " & m_DB.NullQuote(Shipment_Date) _
             & ",Description = " & m_DB.Quote(Description) _
             & ",Description_2 = " & m_DB.Quote(Description_2) _
             & ",Unit_of_Measure = " & m_DB.Quote(Unit_of_Measure) _
             & ",Quantity = " & m_DB.Number(Quantity) _
             & ",Unit_Price = " & m_DB.Number(Unit_Price) _
             & ",VAT_Percent = " & m_DB.Number(VAT_Percent) _
             & ",Line_Discount_Percent = " & m_DB.Number(Line_Discount_Percent) _
             & ",Line_Discount_Amount = " & m_DB.Number(Line_Discount_Amount) _
             & ",Amount = " & m_DB.Number(Amount) _
             & ",Amount_Including_VAT = " & m_DB.Number(Amount_Including_VAT) _
             & ",Allow_Invoice_Disc = " & m_DB.Quote(Allow_Invoice_Disc) _
             & ",Gross_Weight = " & m_DB.Number(Gross_Weight) _
             & ",Net_Weight = " & m_DB.Number(Net_Weight) _
             & ",Units_per_Parcel = " & m_DB.Number(Units_per_Parcel) _
             & ",Unit_Volume = " & m_DB.Number(Unit_Volume) _
             & ",Appl_To_Item_Entry = " & m_DB.Number(Appl_To_Item_Entry) _
             & ",Job_Applies_To_ID = " & m_DB.Quote(Job_Applies_To_ID) _
             & ",Work_Type_Code = " & m_DB.Quote(Work_Type_Code) _
             & ",Bill_To_Customer_No = " & m_DB.Quote(Bill_To_Customer_No) _
             & ",Inv_Discount_Amount = " & m_DB.Number(Inv_Discount_Amount) _
             & ",Tax_Area_Code = " & m_DB.Quote(Tax_Area_Code) _
             & ",Tax_Liable = " & m_DB.Quote(Tax_Liable) _
             & ",Tax_Group_Code = " & m_DB.Quote(Tax_Group_Code) _
             & ",Blanket_Order_No = " & m_DB.Quote(Blanket_Order_No) _
             & ",Blanket_Order_Line_No = " & m_DB.Number(Blanket_Order_Line_No) _
             & ",VAT_Base_Amount = " & m_DB.Number(VAT_Base_Amount) _
             & ",Unit_Cost = " & m_DB.Number(Unit_Cost) _
             & ",Line_Amount = " & m_DB.Number(Line_Amount) _
             & ",Variant_Code = " & m_DB.Quote(Variant_Code) _
             & ",Bin_Code = " & m_DB.Quote(Bin_Code) _
             & ",Qty_per_Unit_of_Measure = " & m_DB.Number(Qty_per_Unit_of_Measure) _
             & ",Unit_of_Measure_Code = " & m_DB.Quote(Unit_of_Measure_Code) _
             & ",Quantity_Base = " & m_DB.Number(Quantity_Base) _
             & ",Responsibility_Center = " & m_DB.Quote(Responsibility_Center) _
             & ",Cross_Reference_No = " & m_DB.Quote(Cross_Reference_No) _
             & ",Unit_of_Measure_Cross_Ref = " & m_DB.Quote(Unit_of_Measure_Cross_Ref) _
             & ",Cross_Reference_Type = " & m_DB.Quote(Cross_Reference_Type) _
             & ",Cross_Reference_Type_No = " & m_DB.Quote(Cross_Reference_Type_No) _
             & ",Item_Category_Code = " & m_DB.Quote(Item_Category_Code) _
             & ",Nonstock = " & m_DB.Quote(Nonstock) _
             & ",Purchasing_Code = " & m_DB.Quote(Purchasing_Code) _
             & ",Product_Group_Code = " & m_DB.Quote(Product_Group_Code) _
             & ",Return_Receipt_No = " & m_DB.Quote(Return_Receipt_No) _
             & ",Return_Receipt_Line_No = " & m_DB.Number(Return_Receipt_Line_No) _
             & ",Return_Reason_Code = " & m_DB.Quote(Return_Reason_Code) _
             & ",Allow_Line_Disc = " & m_DB.Quote(Allow_Line_Disc) _
             & ",Customer_Disc_Group = " & m_DB.Quote(Customer_Disc_Group) _
             & ",Package_Tracking_No = " & m_DB.Quote(Package_Tracking_No) _
             & ",Id = " & m_DB.NullNumber(Id) _
             & " WHERE Sell_To_Customer_No = " & m_DB.Quote(Sell_To_Customer_No)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM _NAVISION_SALES_CREDIT_MEMO_LINES WHERE Sell_To_Customer_No = " & m_DB.Number(Sell_To_Customer_No)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class NavisionSalesCreditMemoLinesCollection
        Inherits GenericCollection(Of NavisionSalesCreditMemoLinesRow)
    End Class

End Namespace


