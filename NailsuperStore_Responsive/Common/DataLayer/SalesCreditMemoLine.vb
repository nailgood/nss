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
Imports Components.Core

Namespace DataLayer

    Public Class SalesCreditMemoLineRow
        Inherits SalesCreditMemoLineRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemoLineId As Integer)
            MyBase.New(DB, MemoLineId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal DocumentNo As String, ByVal LineNo As String)
            MyBase.New(DB, DocumentNo, LineNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal MemoLineId As Integer) As SalesCreditMemoLineRow
            Dim row As SalesCreditMemoLineRow

            row = New SalesCreditMemoLineRow(DB, MemoLineId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal DocumentNo As String, ByVal LineNo As String) As SalesCreditMemoLineRow
            Dim row As SalesCreditMemoLineRow

            row = New SalesCreditMemoLineRow(DB, DocumentNo, LineNo)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MemoLineId As Integer)
            Dim row As SalesCreditMemoLineRow

            row = New SalesCreditMemoLineRow(DB, MemoLineId)
            row.Remove()
        End Sub

        Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
            Dim SQL As String = "select * from SalesCreditMemoLine"
            If Not SortBy = String.Empty Then
                SortBy = Core.ProtectParam(SortBy)
                SortOrder = Core.ProtectParam(SortOrder)

                SQL &= " order by " & SortBy & " " & SortOrder
            End If
            Return DB.GetDataTable(SQL)
        End Function

        'Custom Methods
        Public Sub CopyFromNavision(ByVal DB As Database, ByVal n As NavisionSalesCreditMemoLinesRow)
            Dim MemoId As Integer = DB.ExecuteScalar("select top 1 memoid from salescreditmemoheader where no = " & DB.Quote(Trim(n.Document_No)))
            If MemoId = Nothing Then Exit Sub

            Dim ItemId As Integer
            If Trim(n.Type).ToLower = "item" Then
                ItemId = DB.ExecuteScalar("select top 1 itemid from storeitem where sku = " & DB.Quote(Trim(n.No)))
            End If

            Me.MemoId = MemoId
            Me.AllowInvoiceDisc = n.Allow_Invoice_Disc
            Me.AllowLineDisc = n.Allow_Line_Disc
            Me.Amount = n.Amount
            Me.AmountIncludingVAT = n.Amount_Including_VAT
            Me.ApplToItemEntry = n.Appl_To_Item_Entry
            Me.BillToCustomerNo = n.Bill_To_Customer_No
            Me.BinCode = n.Bin_Code
            Me.BlanketOrderLineNo = n.Blanket_Order_Line_No
            Me.BlanketOrderNo = n.Blanket_Order_No
            Me.CrossReferenceNo = n.Cross_Reference_No
            Me.CrossReferenceType = n.Cross_Reference_Type
            Me.CrossReferenceTypeNo = n.Cross_Reference_Type_No
            Me.CustomerDiscGroup = n.Customer_Disc_Group
            Me.Description = n.Description
            Me.Description2 = n.Description_2
            Me.DocumentNo = n.Document_No
            Me.GrossWeight = n.Gross_Weight
            Me.InvDiscountAmount = n.Inv_Discount_Amount
            Me.ItemCategoryCode = n.Item_Category_Code
            Me.ItemId = ItemId
            Me.JobAppliesToID = n.Job_Applies_To_ID
            Me.LineAmount = n.Line_Amount
            Me.LineDiscountAmount = n.Line_Discount_Amount
            Me.LineDiscountPercent = n.Line_Discount_Percent
            Me.LineNo = n.Line_No
            Me.LocationCode = n.Location_Code
            Me.NetWeight = n.Net_Weight
            Me.No = n.No
            Me.Nonstock = n.Nonstock
            Me.PackageTrackingNo = n.Package_Tracking_No
            Me.ProductGroupCode = n.Product_Group_Code
            Me.PurchasingCode = n.Purchasing_Code
            Me.QtyPerUnitOfMeasure = n.Qty_per_Unit_of_Measure
            Me.Quantity = n.Quantity
            Me.QuantityBase = n.Quantity_Base
            Me.ResponsibilityCenter = n.Responsibility_Center
            Me.ReturnReasonCode = n.Return_Reason_Code
            Me.ReturnReceiptLineNo = n.Return_Receipt_Line_No
            Me.ReturnReceiptNo = n.Return_Receipt_No
            Me.SellToCustomerNo = n.Sell_To_Customer_No
            Me.ShipmentDate = n.Shipment_Date
            Me.TaxAreaCode = n.Tax_Area_Code
            Me.TaxGroupCode = n.Tax_Group_Code
            Me.Type = n.Type
            Me.UnitCost = n.Unit_Cost
            Me.UnitofMeasure = n.Unit_of_Measure
            Me.UnitOfMeasureCode = n.Unit_of_Measure_Code
            Me.UnitofMeasureCrossRef = n.Unit_of_Measure_Cross_Ref
            Me.UnitPrice = n.Unit_Price
            Me.UnitsperParcel = n.Units_per_Parcel
            Me.VariantCode = n.Variant_Code
            Me.VATBaseAmount = n.VAT_Base_Amount
            Me.VATPercent = n.VAT_Percent
            Me.WorkTypeCode = n.Work_Type_Code

            If Me.MemoLineId = Nothing Then
                Me.Insert()
            Else
                Me.Update()
            End If
        End Sub
    End Class

    Public MustInherit Class SalesCreditMemoLineRowBase
        Private m_DB As Database
        Private m_MemoLineId As Integer = Nothing
        Private m_MemoId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_SellToCustomerNo As String = Nothing
        Private m_DocumentNo As String = Nothing
        Private m_LineNo As Integer = Nothing
        Private m_Type As String = Nothing
        Private m_No As String = Nothing
        Private m_LocationCode As String = Nothing
        Private m_ShipmentDate As DateTime = Nothing
        Private m_Description As String = Nothing
        Private m_Description2 As String = Nothing
        Private m_UnitofMeasure As String = Nothing
        Private m_Quantity As Double = Nothing
        Private m_UnitPrice As Double = Nothing
        Private m_VATPercent As Double = Nothing
        Private m_LineDiscountPercent As Double = Nothing
        Private m_LineDiscountAmount As Double = Nothing
        Private m_Amount As Double = Nothing
        Private m_AmountIncludingVAT As Double = Nothing
        Private m_AllowInvoiceDisc As String = Nothing
        Private m_GrossWeight As Double = Nothing
        Private m_NetWeight As Double = Nothing
        Private m_UnitsperParcel As Double = Nothing
        Private m_UnitVolume As Double = Nothing
        Private m_ApplToItemEntry As Integer = Nothing
        Private m_JobAppliesToID As String = Nothing
        Private m_WorkTypeCode As String = Nothing
        Private m_BillToCustomerNo As String = Nothing
        Private m_InvDiscountAmount As Double = Nothing
        Private m_TaxAreaCode As String = Nothing
        Private m_TaxLiable As String = Nothing
        Private m_TaxGroupCode As String = Nothing
        Private m_BlanketOrderNo As String = Nothing
        Private m_BlanketOrderLineNo As Integer = Nothing
        Private m_VATBaseAmount As Double = Nothing
        Private m_UnitCost As Double = Nothing
        Private m_LineAmount As Double = Nothing
        Private m_VariantCode As String = Nothing
        Private m_BinCode As String = Nothing
        Private m_QtyPerUnitOfMeasure As Double = Nothing
        Private m_UnitOfMeasureCode As String = Nothing
        Private m_QuantityBase As Double = Nothing
        Private m_ResponsibilityCenter As String = Nothing
        Private m_CrossReferenceNo As String = Nothing
        Private m_UnitofMeasureCrossRef As String = Nothing
        Private m_CrossReferenceType As String = Nothing
        Private m_CrossReferenceTypeNo As String = Nothing
        Private m_ItemCategoryCode As String = Nothing
        Private m_Nonstock As String = Nothing
        Private m_PurchasingCode As String = Nothing
        Private m_ProductGroupCode As String = Nothing
        Private m_ReturnReceiptNo As String = Nothing
        Private m_ReturnReceiptLineNo As Integer = Nothing
        Private m_ReturnReasonCode As String = Nothing
        Private m_AllowLineDisc As String = Nothing
        Private m_CustomerDiscGroup As String = Nothing
        Private m_PackageTrackingNo As String = Nothing


        Public Property MemoLineId() As Integer
            Get
                Return m_MemoLineId
            End Get
            Set(ByVal Value As Integer)
                m_MemoLineId = Value
            End Set
        End Property

        Public Property MemoId() As Integer
            Get
                Return m_MemoId
            End Get
            Set(ByVal Value As Integer)
                m_MemoId = Value
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

        Public Property SellToCustomerNo() As String
            Get
                Return m_SellToCustomerNo
            End Get
            Set(ByVal Value As String)
                m_SellToCustomerNo = Value
            End Set
        End Property

        Public Property DocumentNo() As String
            Get
                Return m_DocumentNo
            End Get
            Set(ByVal Value As String)
                m_DocumentNo = Value
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

        Public Property No() As String
            Get
                Return m_No
            End Get
            Set(ByVal Value As String)
                m_No = Value
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

        Public Property UnitofMeasure() As String
            Get
                Return m_UnitofMeasure
            End Get
            Set(ByVal Value As String)
                m_UnitofMeasure = Value
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

        Public Property LineDiscountAmount() As Double
            Get
                Return m_LineDiscountAmount
            End Get
            Set(ByVal Value As Double)
                m_LineDiscountAmount = Value
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

        Public Property AmountIncludingVAT() As Double
            Get
                Return m_AmountIncludingVAT
            End Get
            Set(ByVal Value As Double)
                m_AmountIncludingVAT = Value
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

        Public Property UnitsperParcel() As Double
            Get
                Return m_UnitsperParcel
            End Get
            Set(ByVal Value As Double)
                m_UnitsperParcel = Value
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

        Public Property ApplToItemEntry() As Integer
            Get
                Return m_ApplToItemEntry
            End Get
            Set(ByVal Value As Integer)
                m_ApplToItemEntry = Value
            End Set
        End Property

        Public Property JobAppliesToID() As String
            Get
                Return m_JobAppliesToID
            End Get
            Set(ByVal Value As String)
                m_JobAppliesToID = Value
            End Set
        End Property

        Public Property WorkTypeCode() As String
            Get
                Return m_WorkTypeCode
            End Get
            Set(ByVal Value As String)
                m_WorkTypeCode = Value
            End Set
        End Property

        Public Property BillToCustomerNo() As String
            Get
                Return m_BillToCustomerNo
            End Get
            Set(ByVal Value As String)
                m_BillToCustomerNo = Value
            End Set
        End Property

        Public Property InvDiscountAmount() As Double
            Get
                Return m_InvDiscountAmount
            End Get
            Set(ByVal Value As Double)
                m_InvDiscountAmount = Value
            End Set
        End Property

        Public Property TaxAreaCode() As String
            Get
                Return m_TaxAreaCode
            End Get
            Set(ByVal Value As String)
                m_TaxAreaCode = Value
            End Set
        End Property

        Public Property TaxLiable() As String
            Get
                Return m_TaxLiable
            End Get
            Set(ByVal Value As String)
                m_TaxLiable = Value
            End Set
        End Property

        Public Property TaxGroupCode() As String
            Get
                Return m_TaxGroupCode
            End Get
            Set(ByVal Value As String)
                m_TaxGroupCode = Value
            End Set
        End Property

        Public Property BlanketOrderNo() As String
            Get
                Return m_BlanketOrderNo
            End Get
            Set(ByVal Value As String)
                m_BlanketOrderNo = Value
            End Set
        End Property

        Public Property BlanketOrderLineNo() As Integer
            Get
                Return m_BlanketOrderLineNo
            End Get
            Set(ByVal Value As Integer)
                m_BlanketOrderLineNo = Value
            End Set
        End Property

        Public Property VATBaseAmount() As Double
            Get
                Return m_VATBaseAmount
            End Get
            Set(ByVal Value As Double)
                m_VATBaseAmount = Value
            End Set
        End Property

        Public Property UnitCost() As Double
            Get
                Return m_UnitCost
            End Get
            Set(ByVal Value As Double)
                m_UnitCost = Value
            End Set
        End Property

        Public Property LineAmount() As Double
            Get
                Return m_LineAmount
            End Get
            Set(ByVal Value As Double)
                m_LineAmount = Value
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

        Public Property UnitofMeasureCrossRef() As String
            Get
                Return m_UnitofMeasureCrossRef
            End Get
            Set(ByVal Value As String)
                m_UnitofMeasureCrossRef = Value
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

        Public Property ReturnReceiptNo() As String
            Get
                Return m_ReturnReceiptNo
            End Get
            Set(ByVal Value As String)
                m_ReturnReceiptNo = Value
            End Set
        End Property

        Public Property ReturnReceiptLineNo() As Integer
            Get
                Return m_ReturnReceiptLineNo
            End Get
            Set(ByVal Value As Integer)
                m_ReturnReceiptLineNo = Value
            End Set
        End Property

        Public Property ReturnReasonCode() As String
            Get
                Return m_ReturnReasonCode
            End Get
            Set(ByVal Value As String)
                m_ReturnReasonCode = Value
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

        Public Sub New(ByVal DB As Database, ByVal MemoLineId As Integer)
            m_DB = DB
            m_MemoLineId = MemoLineId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal DocumentNo As String, ByVal LineNo As String)
            m_DB = DB
            m_DocumentNo = DocumentNo
            m_LineNo = LineNo
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM SalesCreditMemoLine WHERE " & IIf(MemoLineId = Nothing, " documentno = " & DB.Quote(DocumentNo) & " and [lineno] = " & DB.Quote(LineNo), "MemoLineId = " & DB.Number(MemoLineId))
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
            m_MemoLineId = Convert.ToInt32(r.Item("MemoLineId"))
            m_MemoId = Convert.ToInt32(r.Item("MemoId"))
            If IsDBNull(r.Item("ItemId")) Then
                m_ItemId = Nothing
            Else
                m_ItemId = Convert.ToInt32(r.Item("ItemId"))
            End If
            If IsDBNull(r.Item("SellToCustomerNo")) Then
                m_SellToCustomerNo = Nothing
            Else
                m_SellToCustomerNo = Convert.ToString(r.Item("SellToCustomerNo"))
            End If
            If IsDBNull(r.Item("DocumentNo")) Then
                m_DocumentNo = Nothing
            Else
                m_DocumentNo = Convert.ToString(r.Item("DocumentNo"))
            End If
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
            If IsDBNull(r.Item("No")) Then
                m_No = Nothing
            Else
                m_No = Convert.ToString(r.Item("No"))
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
            If IsDBNull(r.Item("UnitofMeasure")) Then
                m_UnitofMeasure = Nothing
            Else
                m_UnitofMeasure = Convert.ToString(r.Item("UnitofMeasure"))
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
            If IsDBNull(r.Item("LineDiscountAmount")) Then
                m_LineDiscountAmount = Nothing
            Else
                m_LineDiscountAmount = Convert.ToDouble(r.Item("LineDiscountAmount"))
            End If
            If IsDBNull(r.Item("Amount")) Then
                m_Amount = Nothing
            Else
                m_Amount = Convert.ToDouble(r.Item("Amount"))
            End If
            If IsDBNull(r.Item("AmountIncludingVAT")) Then
                m_AmountIncludingVAT = Nothing
            Else
                m_AmountIncludingVAT = Convert.ToDouble(r.Item("AmountIncludingVAT"))
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
            If IsDBNull(r.Item("UnitsperParcel")) Then
                m_UnitsperParcel = Nothing
            Else
                m_UnitsperParcel = Convert.ToDouble(r.Item("UnitsperParcel"))
            End If
            If IsDBNull(r.Item("UnitVolume")) Then
                m_UnitVolume = Nothing
            Else
                m_UnitVolume = Convert.ToDouble(r.Item("UnitVolume"))
            End If
            If IsDBNull(r.Item("ApplToItemEntry")) Then
                m_ApplToItemEntry = Nothing
            Else
                m_ApplToItemEntry = Convert.ToInt32(r.Item("ApplToItemEntry"))
            End If
            If IsDBNull(r.Item("JobAppliesToID")) Then
                m_JobAppliesToID = Nothing
            Else
                m_JobAppliesToID = Convert.ToString(r.Item("JobAppliesToID"))
            End If
            If IsDBNull(r.Item("WorkTypeCode")) Then
                m_WorkTypeCode = Nothing
            Else
                m_WorkTypeCode = Convert.ToString(r.Item("WorkTypeCode"))
            End If
            If IsDBNull(r.Item("BillToCustomerNo")) Then
                m_BillToCustomerNo = Nothing
            Else
                m_BillToCustomerNo = Convert.ToString(r.Item("BillToCustomerNo"))
            End If
            If IsDBNull(r.Item("InvDiscountAmount")) Then
                m_InvDiscountAmount = Nothing
            Else
                m_InvDiscountAmount = Convert.ToDouble(r.Item("InvDiscountAmount"))
            End If
            If IsDBNull(r.Item("TaxAreaCode")) Then
                m_TaxAreaCode = Nothing
            Else
                m_TaxAreaCode = Convert.ToString(r.Item("TaxAreaCode"))
            End If
            If IsDBNull(r.Item("TaxLiable")) Then
                m_TaxLiable = Nothing
            Else
                m_TaxLiable = Convert.ToString(r.Item("TaxLiable"))
            End If
            If IsDBNull(r.Item("TaxGroupCode")) Then
                m_TaxGroupCode = Nothing
            Else
                m_TaxGroupCode = Convert.ToString(r.Item("TaxGroupCode"))
            End If
            If IsDBNull(r.Item("BlanketOrderNo")) Then
                m_BlanketOrderNo = Nothing
            Else
                m_BlanketOrderNo = Convert.ToString(r.Item("BlanketOrderNo"))
            End If
            If IsDBNull(r.Item("BlanketOrderLineNo")) Then
                m_BlanketOrderLineNo = Nothing
            Else
                m_BlanketOrderLineNo = Convert.ToInt32(r.Item("BlanketOrderLineNo"))
            End If
            If IsDBNull(r.Item("VATBaseAmount")) Then
                m_VATBaseAmount = Nothing
            Else
                m_VATBaseAmount = Convert.ToDouble(r.Item("VATBaseAmount"))
            End If
            If IsDBNull(r.Item("UnitCost")) Then
                m_UnitCost = Nothing
            Else
                m_UnitCost = Convert.ToDouble(r.Item("UnitCost"))
            End If
            If IsDBNull(r.Item("LineAmount")) Then
                m_LineAmount = Nothing
            Else
                m_LineAmount = Convert.ToDouble(r.Item("LineAmount"))
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
            If IsDBNull(r.Item("UnitofMeasureCrossRef")) Then
                m_UnitofMeasureCrossRef = Nothing
            Else
                m_UnitofMeasureCrossRef = Convert.ToString(r.Item("UnitofMeasureCrossRef"))
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
            If IsDBNull(r.Item("ReturnReceiptNo")) Then
                m_ReturnReceiptNo = Nothing
            Else
                m_ReturnReceiptNo = Convert.ToString(r.Item("ReturnReceiptNo"))
            End If
            If IsDBNull(r.Item("ReturnReceiptLineNo")) Then
                m_ReturnReceiptLineNo = Nothing
            Else
                m_ReturnReceiptLineNo = Convert.ToInt32(r.Item("ReturnReceiptLineNo"))
            End If
            If IsDBNull(r.Item("ReturnReasonCode")) Then
                m_ReturnReasonCode = Nothing
            Else
                m_ReturnReasonCode = Convert.ToString(r.Item("ReturnReasonCode"))
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
        End Sub 'Load
        Public Shared Function GetMemoLine(ByVal DB As Database, ByVal MemoId As Integer, ByVal ItemId As Integer, ByVal CartItemId As Integer) As Integer
            Try

                Dim result As Integer = CInt(DB.ExecuteScalar("Select MemoLineId from SalesCreditMemoLine where MemoId  = " & MemoId & " and ItemId = " & ItemId & " and LineNo = " & CartItemId))
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function GetAmount(ByVal DB As Database, ByVal OrderNo As String) As Double
            Try
                Dim result As Double = DB.ExecuteScalar("Select Sum(isnull(Amount,0)) From SalesCreditMemoLine Where DocumentNo = " & DB.Quote(OrderNo) & " and Amount > 0")
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function CheckItemReturn(ByVal DB As Database, ByVal OrderNo As String) As Integer
            Try
                Dim result As Integer = DB.ExecuteScalar("Select sum(isnull(Quantity,0)) From SalesCreditMemoLine Where DocumentNo = " & DB.Quote(OrderNo) & " and Type = 'item'")
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function GetMemoLine(ByVal DB As Database, ByVal MemoId As String, ByVal ItemId As Integer) As Integer
            Try
                Dim result As Integer = CInt(DB.ExecuteScalar("Select MemoLineId from SalesCreditMemoLine where MemoId  = " & MemoId & " and ItemId = " & ItemId))
                Return result
            Catch ex As Exception

            End Try
            Return 0


        End Function
        Public Function InsertSassi(ByVal _DB As Database) As Integer
            Dim SQL As String


            SQL = " INSERT INTO SalesCreditMemoLine (" _
             & " MemoId" _
             & ",ItemId" _
             & ",SellToCustomerNo" _
             & ",DocumentNo" _
             & ",[LineNo]" _
             & ",[Type]" _
             & ",[No]" _
             & ",[Description]" _
             & ",UnitofMeasure" _
             & ",Quantity" _
             & ",UnitPrice" _
             & ",Amount" _
             & ") VALUES (" _
             & _DB.NullNumber(MemoId) _
             & "," & _DB.NullNumber(ItemId) _
             & "," & _DB.Quote(SellToCustomerNo) _
             & "," & _DB.Quote(DocumentNo) _
             & "," & _DB.Number(LineNo) _
             & "," & _DB.Quote(Type) _
             & "," & _DB.Quote(No) _
             & "," & _DB.Quote(Description) _
             & "," & _DB.Quote(UnitofMeasure) _
             & "," & _DB.Number(Quantity) _
             & "," & _DB.Number(UnitPrice) _
             & "," & _DB.Number(Amount) _
             & ")"

            MemoLineId = _DB.InsertSQL(SQL)

            Return MemoLineId
        End Function
        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO SalesCreditMemoLine (" _
             & " MemoId" _
             & ",ItemId" _
             & ",SellToCustomerNo" _
             & ",DocumentNo" _
             & ",[LineNo]" _
             & ",Type" _
             & ",No" _
             & ",LocationCode" _
             & ",ShipmentDate" _
             & ",Description" _
             & ",Description2" _
             & ",UnitofMeasure" _
             & ",Quantity" _
             & ",UnitPrice" _
             & ",VATPercent" _
             & ",LineDiscountPercent" _
             & ",LineDiscountAmount" _
             & ",Amount" _
             & ",AmountIncludingVAT" _
             & ",AllowInvoiceDisc" _
             & ",GrossWeight" _
             & ",NetWeight" _
             & ",UnitsperParcel" _
             & ",UnitVolume" _
             & ",ApplToItemEntry" _
             & ",JobAppliesToID" _
             & ",WorkTypeCode" _
             & ",BillToCustomerNo" _
             & ",InvDiscountAmount" _
             & ",TaxAreaCode" _
             & ",TaxLiable" _
             & ",TaxGroupCode" _
             & ",BlanketOrderNo" _
             & ",BlanketOrderLineNo" _
             & ",VATBaseAmount" _
             & ",UnitCost" _
             & ",LineAmount" _
             & ",VariantCode" _
             & ",BinCode" _
             & ",QtyPerUnitOfMeasure" _
             & ",UnitOfMeasureCode" _
             & ",QuantityBase" _
             & ",ResponsibilityCenter" _
             & ",CrossReferenceNo" _
             & ",UnitofMeasureCrossRef" _
             & ",CrossReferenceType" _
             & ",CrossReferenceTypeNo" _
             & ",ItemCategoryCode" _
             & ",Nonstock" _
             & ",PurchasingCode" _
             & ",ProductGroupCode" _
             & ",ReturnReceiptNo" _
             & ",ReturnReceiptLineNo" _
             & ",ReturnReasonCode" _
             & ",AllowLineDisc" _
             & ",CustomerDiscGroup" _
             & ",PackageTrackingNo" _
             & ") VALUES (" _
             & m_DB.NullNumber(MemoId) _
             & "," & m_DB.NullNumber(ItemId) _
             & "," & m_DB.Quote(SellToCustomerNo) _
             & "," & m_DB.Quote(DocumentNo) _
             & "," & m_DB.Number(LineNo) _
             & "," & m_DB.Quote(Type) _
             & "," & m_DB.Quote(No) _
             & "," & m_DB.Quote(LocationCode) _
             & "," & m_DB.NullQuote(ShipmentDate) _
             & "," & m_DB.Quote(Description) _
             & "," & m_DB.Quote(Description2) _
             & "," & m_DB.Quote(UnitofMeasure) _
             & "," & m_DB.Number(Quantity) _
             & "," & m_DB.Number(UnitPrice) _
             & "," & m_DB.Number(VATPercent) _
             & "," & m_DB.Number(LineDiscountPercent) _
             & "," & m_DB.Number(LineDiscountAmount) _
             & "," & m_DB.Number(Amount) _
             & "," & m_DB.Number(AmountIncludingVAT) _
             & "," & m_DB.Quote(AllowInvoiceDisc) _
             & "," & m_DB.Number(GrossWeight) _
             & "," & m_DB.Number(NetWeight) _
             & "," & m_DB.Number(UnitsperParcel) _
             & "," & m_DB.Number(UnitVolume) _
             & "," & m_DB.Number(ApplToItemEntry) _
             & "," & m_DB.Quote(JobAppliesToID) _
             & "," & m_DB.Quote(WorkTypeCode) _
             & "," & m_DB.Quote(BillToCustomerNo) _
             & "," & m_DB.Number(InvDiscountAmount) _
             & "," & m_DB.Quote(TaxAreaCode) _
             & "," & m_DB.Quote(TaxLiable) _
             & "," & m_DB.Quote(TaxGroupCode) _
             & "," & m_DB.Quote(BlanketOrderNo) _
             & "," & m_DB.Number(BlanketOrderLineNo) _
             & "," & m_DB.Number(VATBaseAmount) _
             & "," & m_DB.Number(UnitCost) _
             & "," & m_DB.Number(LineAmount) _
             & "," & m_DB.Quote(VariantCode) _
             & "," & m_DB.Quote(BinCode) _
             & "," & m_DB.Number(QtyPerUnitOfMeasure) _
             & "," & m_DB.Quote(UnitOfMeasureCode) _
             & "," & m_DB.Number(QuantityBase) _
             & "," & m_DB.Quote(ResponsibilityCenter) _
             & "," & m_DB.Quote(CrossReferenceNo) _
             & "," & m_DB.Quote(UnitofMeasureCrossRef) _
             & "," & m_DB.Quote(CrossReferenceType) _
             & "," & m_DB.Quote(CrossReferenceTypeNo) _
             & "," & m_DB.Quote(ItemCategoryCode) _
             & "," & m_DB.Quote(Nonstock) _
             & "," & m_DB.Quote(PurchasingCode) _
             & "," & m_DB.Quote(ProductGroupCode) _
             & "," & m_DB.Quote(ReturnReceiptNo) _
             & "," & m_DB.Number(ReturnReceiptLineNo) _
             & "," & m_DB.Quote(ReturnReasonCode) _
             & "," & m_DB.Quote(AllowLineDisc) _
             & "," & m_DB.Quote(CustomerDiscGroup) _
             & "," & m_DB.Quote(PackageTrackingNo) _
             & ")"

            MemoLineId = m_DB.InsertSQL(SQL)

            Return MemoLineId
        End Function
        Public Sub UpdateSassi(ByVal _DB As Database)
            Dim SQL As String

            SQL = " UPDATE SalesCreditMemoLine SET " _
             & " MemoId = " & _DB.NullNumber(MemoId) _
             & ",ItemId = " & _DB.NullNumber(ItemId) _
             & ",SellToCustomerNo = " & _DB.Quote(SellToCustomerNo) _
             & ",[DocumentNo] = " & _DB.Quote(DocumentNo) _
             & ",[LineNo] = " & _DB.Quote(LineNo) _
             & ",[Type] = " & _DB.Quote(Type) _
             & ",[No] = " & _DB.Quote(No) _
             & ",Description = " & _DB.Quote(Description) _
             & ",UnitofMeasure = " & _DB.Quote(UnitofMeasure) _
             & ",Quantity = " & _DB.Number(Quantity) _
             & ",UnitPrice = " & _DB.Number(UnitPrice) _
             & ",Amount = " & _DB.Number(Amount) _
             & " WHERE MemoLineId = " & _DB.Quote(MemoLineId)

            _DB.ExecuteSQL(SQL)

        End Sub 'Update
        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE SalesCreditMemoLine SET " _
             & " MemoId = " & m_DB.NullNumber(MemoId) _
             & ",ItemId = " & m_DB.NullNumber(ItemId) _
             & ",SellToCustomerNo = " & m_DB.Quote(SellToCustomerNo) _
             & ",DocumentNo = " & m_DB.Quote(DocumentNo) _
             & ",[LineNo] = " & m_DB.Number(LineNo) _
             & ",Type = " & m_DB.Quote(Type) _
             & ",No = " & m_DB.Quote(No) _
             & ",LocationCode = " & m_DB.Quote(LocationCode) _
             & ",ShipmentDate = " & m_DB.NullQuote(ShipmentDate) _
             & ",Description = " & m_DB.Quote(Description) _
             & ",Description2 = " & m_DB.Quote(Description2) _
             & ",UnitofMeasure = " & m_DB.Quote(UnitofMeasure) _
             & ",Quantity = " & m_DB.Number(Quantity) _
             & ",UnitPrice = " & m_DB.Number(UnitPrice) _
             & ",VATPercent = " & m_DB.Number(VATPercent) _
             & ",LineDiscountPercent = " & m_DB.Number(LineDiscountPercent) _
             & ",LineDiscountAmount = " & m_DB.Number(LineDiscountAmount) _
             & ",Amount = " & m_DB.Number(Amount) _
             & ",AmountIncludingVAT = " & m_DB.Number(AmountIncludingVAT) _
             & ",AllowInvoiceDisc = " & m_DB.Quote(AllowInvoiceDisc) _
             & ",GrossWeight = " & m_DB.Number(GrossWeight) _
             & ",NetWeight = " & m_DB.Number(NetWeight) _
             & ",UnitsperParcel = " & m_DB.Number(UnitsperParcel) _
             & ",UnitVolume = " & m_DB.Number(UnitVolume) _
             & ",ApplToItemEntry = " & m_DB.Number(ApplToItemEntry) _
             & ",JobAppliesToID = " & m_DB.Quote(JobAppliesToID) _
             & ",WorkTypeCode = " & m_DB.Quote(WorkTypeCode) _
             & ",BillToCustomerNo = " & m_DB.Quote(BillToCustomerNo) _
             & ",InvDiscountAmount = " & m_DB.Number(InvDiscountAmount) _
             & ",TaxAreaCode = " & m_DB.Quote(TaxAreaCode) _
             & ",TaxLiable = " & m_DB.Quote(TaxLiable) _
             & ",TaxGroupCode = " & m_DB.Quote(TaxGroupCode) _
             & ",BlanketOrderNo = " & m_DB.Quote(BlanketOrderNo) _
             & ",BlanketOrderLineNo = " & m_DB.Number(BlanketOrderLineNo) _
             & ",VATBaseAmount = " & m_DB.Number(VATBaseAmount) _
             & ",UnitCost = " & m_DB.Number(UnitCost) _
             & ",LineAmount = " & m_DB.Number(LineAmount) _
             & ",VariantCode = " & m_DB.Quote(VariantCode) _
             & ",BinCode = " & m_DB.Quote(BinCode) _
             & ",QtyPerUnitOfMeasure = " & m_DB.Number(QtyPerUnitOfMeasure) _
             & ",UnitOfMeasureCode = " & m_DB.Quote(UnitOfMeasureCode) _
             & ",QuantityBase = " & m_DB.Number(QuantityBase) _
             & ",ResponsibilityCenter = " & m_DB.Quote(ResponsibilityCenter) _
             & ",CrossReferenceNo = " & m_DB.Quote(CrossReferenceNo) _
             & ",UnitofMeasureCrossRef = " & m_DB.Quote(UnitofMeasureCrossRef) _
             & ",CrossReferenceType = " & m_DB.Quote(CrossReferenceType) _
             & ",CrossReferenceTypeNo = " & m_DB.Quote(CrossReferenceTypeNo) _
             & ",ItemCategoryCode = " & m_DB.Quote(ItemCategoryCode) _
             & ",Nonstock = " & m_DB.Quote(Nonstock) _
             & ",PurchasingCode = " & m_DB.Quote(PurchasingCode) _
             & ",ProductGroupCode = " & m_DB.Quote(ProductGroupCode) _
             & ",ReturnReceiptNo = " & m_DB.Quote(ReturnReceiptNo) _
             & ",ReturnReceiptLineNo = " & m_DB.Number(ReturnReceiptLineNo) _
             & ",ReturnReasonCode = " & m_DB.Quote(ReturnReasonCode) _
             & ",AllowLineDisc = " & m_DB.Quote(AllowLineDisc) _
             & ",CustomerDiscGroup = " & m_DB.Quote(CustomerDiscGroup) _
             & ",PackageTrackingNo = " & m_DB.Quote(PackageTrackingNo) _
             & " WHERE MemoLineId = " & m_DB.Quote(MemoLineId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM SalesCreditMemoLine WHERE MemoLineId = " & m_DB.Number(MemoLineId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class SalesCreditMemoLineCollection
        Inherits GenericCollection(Of SalesCreditMemoLineRow)
    End Class

End Namespace


