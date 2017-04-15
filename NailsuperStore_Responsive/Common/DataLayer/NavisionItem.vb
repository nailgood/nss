Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionItemRow
        Inherits NavisionItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        'Custom Methods
        Public Shared Function GetCollection(ByVal DB As Database) As NavisionItemCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As NavisionItemRow
                Dim c As New NavisionItemCollection
                Dim SQL As String = "select * from _NAVISION_ITEM where item_category_code not in ('BOT','LAB','SAM')"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionItemRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
                Return c
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New NavisionItemCollection
        End Function

    End Class

    Public MustInherit Class NavisionItemRowBase
        Private m_DB As Database
        Private m_Item_No As String = Nothing
        Private m_Item_No_2 As String = Nothing
        Private m_Description As String = Nothing
        Private m_Search_Description As String = Nothing
        Private m_Description_2 As String = Nothing
        Private m_Bill_of_Materials As String = Nothing
        Private m_Class As String = Nothing
        Private m_Base_Unit_of_Measure As String = Nothing
        Private m_Inventory_Posting_Group As String = Nothing
        Private m_Item_Disc_Group As String = Nothing
        Private m_Allow_Invoice_Disc As String = Nothing
        Private m_Unit_Price As Double = Nothing
        Private m_Unit_Cost As Double = Nothing
        Private m_Standard_Cost As Double = Nothing
        Private m_Lead_Time_Calculation As String = Nothing
        Private m_Alternative_Item_No As String = Nothing
        Private m_Gross_Weight As Double = Nothing
        Private m_Net_Weight As Double = Nothing
        Private m_Units_per_Parcel As Double = Nothing
        Private m_Unit_Volume As Double = Nothing
        Private m_Durability As String = Nothing
        Private m_Country_Purchased_Code As String = Nothing
        Private m_Blocked As String = Nothing
        Private m_Last_Date_Modified As DateTime = Nothing
        Private m_Global_Dimension_1_Filter As String = Nothing
        Private m_Global_Dimension_2_Filter As String = Nothing
        Private m_Inventory As Double = Nothing
        Private m_Qty_on_Purch_Order As Double = Nothing
        Private m_Qty_on_Sales_Order As Double = Nothing
        Private m_Price_Includes_VAT As String = Nothing
        Private m_Country_of_Origin_Code As String = Nothing
        Private m_Sales_Unit_of_Measure As String = Nothing
        Private m_Manufacturer_Code As String = Nothing
        Private m_Item_Category_Code As String = Nothing
        Private m_Product_Group_Code As String = Nothing
        Private m_Substitutes_Exist As String = Nothing
        Private m_Item_Created_On As DateTime = Nothing
        Private m_Inventory_Available As Double = Nothing
        Private m_Item_Type As String = Nothing
        Private m_Preorder_Date As DateTime = Nothing
        Private m_Drop_Ship_Item As String = Nothing
        Private m_Tax_Group_Code As String = Nothing
        Private m_Hazmat As String = Nothing
        Private m_BlockedWeb As String = Nothing
        Private m_ID As Integer = Nothing

        Public Property BlockedWeb() As String
            Get
                Return m_BlockedWeb
            End Get
            Set(ByVal value As String)
                m_BlockedWeb = value
            End Set
        End Property

        Public Property Hazmat() As String
            Get
                Return m_Hazmat
            End Get
            Set(ByVal value As String)
                m_Hazmat = value
            End Set
        End Property

        Public Property Tax_Group_Code() As String
            Get
                Return m_Tax_Group_Code
            End Get
            Set(ByVal value As String)
                m_Tax_Group_Code = value
            End Set
        End Property

        Public Property Item_No() As String
            Get
                Return Trim(m_Item_No)
            End Get
            Set(ByVal Value As String)
                m_Item_No = Trim(Value)
            End Set
        End Property

        Public Property Item_No_2() As String
            Get
                Return Trim(m_Item_No_2)
            End Get
            Set(ByVal Value As String)
                m_Item_No_2 = Trim(Value)
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

        Public Property Search_Description() As String
            Get
                Return Trim(m_Search_Description)
            End Get
            Set(ByVal Value As String)
                m_Search_Description = Trim(Value)
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

        Public Property Bill_of_Materials() As String
            Get
                Return Trim(m_Bill_of_Materials)
            End Get
            Set(ByVal Value As String)
                m_Bill_of_Materials = Trim(Value)
            End Set
        End Property

        Public Property [Class]() As String
            Get
                Return Trim(m_Class)
            End Get
            Set(ByVal Value As String)
                m_Class = Trim(Value)
            End Set
        End Property

        Public Property Base_Unit_of_Measure() As String
            Get
                Return Trim(m_Base_Unit_of_Measure)
            End Get
            Set(ByVal Value As String)
                m_Base_Unit_of_Measure = Trim(Value)
            End Set
        End Property

        Public Property Inventory_Posting_Group() As String
            Get
                Return Trim(m_Inventory_Posting_Group)
            End Get
            Set(ByVal Value As String)
                m_Inventory_Posting_Group = Trim(Value)
            End Set
        End Property

        Public Property Item_Disc_Group() As String
            Get
                Return Trim(m_Item_Disc_Group)
            End Get
            Set(ByVal Value As String)
                m_Item_Disc_Group = Trim(Value)
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

        Public Property Unit_Price() As Double
            Get
                Return Trim(m_Unit_Price)
            End Get
            Set(ByVal Value As Double)
                m_Unit_Price = Trim(Value)
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

        Public Property Standard_Cost() As Double
            Get
                Return Trim(m_Standard_Cost)
            End Get
            Set(ByVal Value As Double)
                m_Standard_Cost = Trim(Value)
            End Set
        End Property

        Public Property Lead_Time_Calculation() As String
            Get
                Return Trim(m_Lead_Time_Calculation)
            End Get
            Set(ByVal Value As String)
                m_Lead_Time_Calculation = Trim(Value)
            End Set
        End Property

        Public Property Alternative_Item_No() As String
            Get
                Return Trim(m_Alternative_Item_No)
            End Get
            Set(ByVal Value As String)
                m_Alternative_Item_No = Trim(Value)
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

        Public Property Durability() As String
            Get
                Return Trim(m_Durability)
            End Get
            Set(ByVal Value As String)
                m_Durability = Trim(Value)
            End Set
        End Property

        Public Property Country_Purchased_Code() As String
            Get
                Return Trim(m_Country_Purchased_Code)
            End Get
            Set(ByVal Value As String)
                m_Country_Purchased_Code = Trim(Value)
            End Set
        End Property

        Public Property Blocked() As String
            Get
                Return Trim(m_Blocked)
            End Get
            Set(ByVal Value As String)
                m_Blocked = Trim(Value)
            End Set
        End Property

        Public Property Last_Date_Modified() As DateTime
            Get
                Return Trim(m_Last_Date_Modified)
            End Get
            Set(ByVal Value As DateTime)
                m_Last_Date_Modified = Trim(Value)
            End Set
        End Property

        Public Property Global_Dimension_1_Filter() As String
            Get
                Return Trim(m_Global_Dimension_1_Filter)
            End Get
            Set(ByVal Value As String)
                m_Global_Dimension_1_Filter = Trim(Value)
            End Set
        End Property

        Public Property Global_Dimension_2_Filter() As String
            Get
                Return Trim(m_Global_Dimension_2_Filter)
            End Get
            Set(ByVal Value As String)
                m_Global_Dimension_2_Filter = Trim(Value)
            End Set
        End Property

        Public Property Inventory() As Double
            Get
                Return Trim(m_Inventory)
            End Get
            Set(ByVal Value As Double)
                m_Inventory = Trim(Value)
            End Set
        End Property

        Public Property Qty_on_Purch_Order() As Double
            Get
                Return Trim(m_Qty_on_Purch_Order)
            End Get
            Set(ByVal Value As Double)
                m_Qty_on_Purch_Order = Trim(Value)
            End Set
        End Property

        Public Property Qty_on_Sales_Order() As Double
            Get
                Return Trim(m_Qty_on_Sales_Order)
            End Get
            Set(ByVal Value As Double)
                m_Qty_on_Sales_Order = Trim(Value)
            End Set
        End Property

        Public Property Price_Includes_VAT() As String
            Get
                Return Trim(m_Price_Includes_VAT)
            End Get
            Set(ByVal Value As String)
                m_Price_Includes_VAT = Trim(Value)
            End Set
        End Property

        Public Property Country_of_Origin_Code() As String
            Get
                Return Trim(m_Country_of_Origin_Code)
            End Get
            Set(ByVal Value As String)
                m_Country_of_Origin_Code = Trim(Value)
            End Set
        End Property

        Public Property Sales_Unit_of_Measure() As String
            Get
                Return Trim(m_Sales_Unit_of_Measure)
            End Get
            Set(ByVal Value As String)
                m_Sales_Unit_of_Measure = Trim(Value)
            End Set
        End Property

        Public Property Manufacturer_Code() As String
            Get
                Return Trim(m_Manufacturer_Code)
            End Get
            Set(ByVal Value As String)
                m_Manufacturer_Code = Trim(Value)
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

        Public Property Product_Group_Code() As String
            Get
                Return Trim(m_Product_Group_Code)
            End Get
            Set(ByVal Value As String)
                m_Product_Group_Code = Trim(Value)
            End Set
        End Property

        Public Property Substitutes_Exist() As String
            Get
                Return Trim(m_Substitutes_Exist)
            End Get
            Set(ByVal Value As String)
                m_Substitutes_Exist = Trim(Value)
            End Set
        End Property

        Public Property Item_Created_On() As DateTime
            Get
                Return Trim(m_Item_Created_On)
            End Get
            Set(ByVal Value As DateTime)
                m_Item_Created_On = Trim(Value)
            End Set
        End Property

        Public Property Inventory_Available() As Double
            Get
                Return Trim(m_Inventory_Available)
            End Get
            Set(ByVal Value As Double)
                m_Inventory_Available = Trim(Value)
            End Set
        End Property

        Public Property Item_Type() As String
            Get
                Return Trim(m_Item_Type)
            End Get
            Set(ByVal Value As String)
                m_Item_Type = Trim(Value)
            End Set
        End Property

        Public Property Preorder_Date() As DateTime
            Get
                Return Trim(m_Preorder_Date)
            End Get
            Set(ByVal Value As DateTime)
                m_Preorder_Date = Trim(Value)
            End Set
        End Property

        Public Property Drop_Ship_Item() As String
            Get
                Return Trim(m_Drop_Ship_Item)
            End Get
            Set(ByVal Value As String)
                m_Drop_Ship_Item = Trim(Value)
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
            If IsDBNull(r.Item("Item_No")) Then
                m_Item_No = Nothing
            Else
                m_Item_No = Convert.ToString(r.Item("Item_No"))
            End If
            If IsDBNull(r.Item("Item_No_2")) Then
                m_Item_No_2 = Nothing
            Else
                m_Item_No_2 = Convert.ToString(r.Item("Item_No_2"))
            End If
            If IsDBNull(r.Item("Description")) Then
                m_Description = Nothing
            Else
                m_Description = Convert.ToString(r.Item("Description"))
            End If
            If IsDBNull(r.Item("Search_Description")) Then
                m_Search_Description = Nothing
            Else
                m_Search_Description = Convert.ToString(r.Item("Search_Description"))
            End If
            If IsDBNull(r.Item("Description_2")) Then
                m_Description_2 = Nothing
            Else
                m_Description_2 = Convert.ToString(r.Item("Description_2"))
            End If
            If IsDBNull(r.Item("Bill_of_Materials")) Then
                m_Bill_of_Materials = Nothing
            Else
                m_Bill_of_Materials = Convert.ToString(r.Item("Bill_of_Materials"))
            End If
            If IsDBNull(r.Item("Class")) Then
                m_Class = Nothing
            Else
                m_Class = Convert.ToString(r.Item("Class"))
            End If
            If IsDBNull(r.Item("Base_Unit_of_Measure")) Then
                m_Base_Unit_of_Measure = Nothing
            Else
                m_Base_Unit_of_Measure = Convert.ToString(r.Item("Base_Unit_of_Measure"))
            End If
            If IsDBNull(r.Item("Inventory_Posting_Group")) Then
                m_Inventory_Posting_Group = Nothing
            Else
                m_Inventory_Posting_Group = Convert.ToString(r.Item("Inventory_Posting_Group"))
            End If
            If IsDBNull(r.Item("Item_Disc_Group")) Then
                m_Item_Disc_Group = Nothing
            Else
                m_Item_Disc_Group = Convert.ToString(r.Item("Item_Disc_Group"))
            End If
            If IsDBNull(r.Item("Allow_Invoice_Disc")) Then
                m_Allow_Invoice_Disc = Nothing
            Else
                m_Allow_Invoice_Disc = Convert.ToString(r.Item("Allow_Invoice_Disc"))
            End If
            If IsDBNull(r.Item("Unit_Price")) Then
                m_Unit_Price = Nothing
            Else
                m_Unit_Price = Convert.ToDouble(r.Item("Unit_Price"))
            End If
            If IsDBNull(r.Item("Unit_Cost")) Then
                m_Unit_Cost = Nothing
            Else
                m_Unit_Cost = Convert.ToDouble(r.Item("Unit_Cost"))
            End If
            If IsDBNull(r.Item("Standard_Cost")) Then
                m_Standard_Cost = Nothing
            Else
                m_Standard_Cost = Convert.ToDouble(r.Item("Standard_Cost"))
            End If
            If IsDBNull(r.Item("Lead_Time_Calculation")) Then
                m_Lead_Time_Calculation = Nothing
            Else
                m_Lead_Time_Calculation = Convert.ToString(r.Item("Lead_Time_Calculation"))
            End If
            If IsDBNull(r.Item("Alternative_Item_No")) Then
                m_Alternative_Item_No = Nothing
            Else
                m_Alternative_Item_No = Convert.ToString(r.Item("Alternative_Item_No"))
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
            If IsDBNull(r.Item("Durability")) Then
                m_Durability = Nothing
            Else
                m_Durability = Convert.ToString(r.Item("Durability"))
            End If
            If IsDBNull(r.Item("Country_Purchased_Code")) Then
                m_Country_Purchased_Code = Nothing
            Else
                m_Country_Purchased_Code = Convert.ToString(r.Item("Country_Purchased_Code"))
            End If
            If IsDBNull(r.Item("Blocked")) Then
                m_Blocked = Nothing
            Else
                m_Blocked = Convert.ToString(r.Item("Blocked"))
            End If
            If IsDBNull(r.Item("Last_Date_Modified")) Then
                m_Last_Date_Modified = Nothing
            Else
                m_Last_Date_Modified = Convert.ToDateTime(r.Item("Last_Date_Modified"))
            End If
            If IsDBNull(r.Item("Global_Dimension_1_Filter")) Then
                m_Global_Dimension_1_Filter = Nothing
            Else
                m_Global_Dimension_1_Filter = Convert.ToString(r.Item("Global_Dimension_1_Filter"))
            End If
            If IsDBNull(r.Item("Global_Dimension_2_Filter")) Then
                m_Global_Dimension_2_Filter = Nothing
            Else
                m_Global_Dimension_2_Filter = Convert.ToString(r.Item("Global_Dimension_2_Filter"))
            End If
            If IsDBNull(r.Item("Inventory")) Then
                m_Inventory = Nothing
            Else
                m_Inventory = Convert.ToDouble(r.Item("Inventory"))
            End If
            If IsDBNull(r.Item("Qty_on_Purch_Order")) Then
                m_Qty_on_Purch_Order = Nothing
            Else
                m_Qty_on_Purch_Order = Convert.ToDouble(r.Item("Qty_on_Purch_Order"))
            End If
            If IsDBNull(r.Item("Qty_on_Sales_Order")) Then
                m_Qty_on_Sales_Order = Nothing
            Else
                m_Qty_on_Sales_Order = Convert.ToDouble(r.Item("Qty_on_Sales_Order"))
            End If
            If IsDBNull(r.Item("Price_Includes_VAT")) Then
                m_Price_Includes_VAT = Nothing
            Else
                m_Price_Includes_VAT = Convert.ToString(r.Item("Price_Includes_VAT"))
            End If
            If IsDBNull(r.Item("Country_of_Origin_Code")) Then
                m_Country_of_Origin_Code = Nothing
            Else
                m_Country_of_Origin_Code = Convert.ToString(r.Item("Country_of_Origin_Code"))
            End If
            If IsDBNull(r.Item("Sales_Unit_of_Measure")) Then
                m_Sales_Unit_of_Measure = Nothing
            Else
                m_Sales_Unit_of_Measure = Convert.ToString(r.Item("Sales_Unit_of_Measure"))
            End If
            If IsDBNull(r.Item("Manufacturer_Code")) Then
                m_Manufacturer_Code = Nothing
            Else
                m_Manufacturer_Code = Convert.ToString(r.Item("Manufacturer_Code"))
            End If
            If IsDBNull(r.Item("Item_Category_Code")) Then
                m_Item_Category_Code = Nothing
            Else
                m_Item_Category_Code = Convert.ToString(r.Item("Item_Category_Code"))
            End If
            If IsDBNull(r.Item("Product_Group_Code")) Then
                m_Product_Group_Code = Nothing
            Else
                m_Product_Group_Code = Convert.ToString(r.Item("Product_Group_Code"))
            End If
            If IsDBNull(r.Item("Substitutes_Exist")) Then
                m_Substitutes_Exist = Nothing
            Else
                m_Substitutes_Exist = Convert.ToString(r.Item("Substitutes_Exist"))
            End If
            If IsDBNull(r.Item("Item_Created_On")) Then
                m_Item_Created_On = Nothing
            Else
                m_Item_Created_On = Convert.ToDateTime(r.Item("Item_Created_On"))
            End If
            If IsDBNull(r.Item("Inventory_Available")) Then
                m_Inventory_Available = Nothing
            Else
                m_Inventory_Available = Convert.ToDouble(r.Item("Inventory_Available"))
            End If
            If IsDBNull(r.Item("Item_Type")) Then
                m_Item_Type = Nothing
            Else
                m_Item_Type = Convert.ToString(r.Item("Item_Type"))
            End If
            If IsDBNull(r.Item("Preorder_Date")) Then
                m_Preorder_Date = Nothing
            Else
                m_Preorder_Date = Convert.ToDateTime(r.Item("Preorder_Date"))
            End If
            If IsDBNull(r.Item("Drop_Ship_Item")) Then
                m_Drop_Ship_Item = Nothing
            Else
                m_Drop_Ship_Item = Convert.ToString(r.Item("Drop_Ship_Item"))
            End If
            m_Tax_Group_Code = Convert.ToString(r.Item("Tax_Group_Code"))
            m_Hazmat = Convert.ToString(r.Item("Hazmat"))

            If IsDBNull(r.Item("BlockedWeb")) Then
                m_BlockedWeb = Nothing
            Else
                m_BlockedWeb = Convert.ToString(r.Item("BlockedWeb"))
            End If

            m_ID = Convert.ToInt32(r.Item("ID"))
        End Sub 'Load
    End Class

    Public Class NavisionItemCollection
        Inherits GenericCollection(Of NavisionItemRow)
    End Class

End Namespace


