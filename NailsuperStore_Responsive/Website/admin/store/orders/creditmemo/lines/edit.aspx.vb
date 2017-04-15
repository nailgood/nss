Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_orders_creditmemo_lines_Edit
    Inherits AdminPage

    Protected MemoLineId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        MemoLineId = Convert.ToInt32(Request("MemoLineId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If MemoLineId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbSalesCreditMemoLine As SalesCreditMemoLineRow = SalesCreditMemoLineRow.GetRow(DB, MemoLineId)
        txtMemoId.Text = dbSalesCreditMemoLine.MemoId
        txtSellToCustomerNo.Text = dbSalesCreditMemoLine.SellToCustomerNo
        txtDocumentNo.Text = dbSalesCreditMemoLine.DocumentNo
        txtLineNo.Text = dbSalesCreditMemoLine.LineNo
        txtType.Text = dbSalesCreditMemoLine.Type
        txtNo.Text = dbSalesCreditMemoLine.No
        txtLocationCode.Text = dbSalesCreditMemoLine.LocationCode
        txtDescription.Text = dbSalesCreditMemoLine.Description
        txtDescription2.Text = dbSalesCreditMemoLine.Description2
        txtUnitofMeasure.Text = dbSalesCreditMemoLine.UnitofMeasure
        txtQuantity.Text = dbSalesCreditMemoLine.Quantity
        txtUnitPrice.Text = dbSalesCreditMemoLine.UnitPrice
        txtVATPercent.Text = dbSalesCreditMemoLine.VATPercent
        txtLineDiscountPercent.Text = dbSalesCreditMemoLine.LineDiscountPercent
        txtLineDiscountAmount.Text = dbSalesCreditMemoLine.LineDiscountAmount
        txtAmount.Text = dbSalesCreditMemoLine.Amount
        txtAmountIncludingVAT.Text = dbSalesCreditMemoLine.AmountIncludingVAT
        txtGrossWeight.Text = dbSalesCreditMemoLine.GrossWeight
        txtNetWeight.Text = dbSalesCreditMemoLine.NetWeight
        txtUnitsperParcel.Text = dbSalesCreditMemoLine.UnitsperParcel
        txtUnitVolume.Text = dbSalesCreditMemoLine.UnitVolume
        txtApplToItemEntry.Text = dbSalesCreditMemoLine.ApplToItemEntry
        txtJobAppliesToID.Text = dbSalesCreditMemoLine.JobAppliesToID
        txtWorkTypeCode.Text = dbSalesCreditMemoLine.WorkTypeCode
        txtBillToCustomerNo.Text = dbSalesCreditMemoLine.BillToCustomerNo
        txtInvDiscountAmount.Text = dbSalesCreditMemoLine.InvDiscountAmount
        txtTaxAreaCode.Text = dbSalesCreditMemoLine.TaxAreaCode
        txtTaxGroupCode.Text = dbSalesCreditMemoLine.TaxGroupCode
        txtBlanketOrderNo.Text = dbSalesCreditMemoLine.BlanketOrderNo
        txtBlanketOrderLineNo.Text = dbSalesCreditMemoLine.BlanketOrderLineNo
        txtVATBaseAmount.Text = dbSalesCreditMemoLine.VATBaseAmount
        txtUnitCost.Text = dbSalesCreditMemoLine.UnitCost
        txtLineAmount.Text = dbSalesCreditMemoLine.LineAmount
        txtVariantCode.Text = dbSalesCreditMemoLine.VariantCode
        txtBinCode.Text = dbSalesCreditMemoLine.BinCode
        txtQtyPerUnitOfMeasure.Text = dbSalesCreditMemoLine.QtyPerUnitOfMeasure
        txtUnitOfMeasureCode.Text = dbSalesCreditMemoLine.UnitOfMeasureCode
        txtQuantityBase.Text = dbSalesCreditMemoLine.QuantityBase
        txtResponsibilityCenter.Text = dbSalesCreditMemoLine.ResponsibilityCenter
        txtCrossReferenceNo.Text = dbSalesCreditMemoLine.CrossReferenceNo
        txtUnitofMeasureCrossRef.Text = dbSalesCreditMemoLine.UnitofMeasureCrossRef
        txtCrossReferenceType.Text = dbSalesCreditMemoLine.CrossReferenceType
        txtCrossReferenceTypeNo.Text = dbSalesCreditMemoLine.CrossReferenceTypeNo
        txtItemCategoryCode.Text = dbSalesCreditMemoLine.ItemCategoryCode
        txtPurchasingCode.Text = dbSalesCreditMemoLine.PurchasingCode
        txtProductGroupCode.Text = dbSalesCreditMemoLine.ProductGroupCode
        txtReturnReceiptNo.Text = dbSalesCreditMemoLine.ReturnReceiptNo
        txtReturnReceiptLineNo.Text = dbSalesCreditMemoLine.ReturnReceiptLineNo
        txtReturnReasonCode.Text = dbSalesCreditMemoLine.ReturnReasonCode
        txtCustomerDiscGroup.Text = dbSalesCreditMemoLine.CustomerDiscGroup
        txtPackageTrackingNo.Text = dbSalesCreditMemoLine.PackageTrackingNo
        dtShipmentDate.Value = dbSalesCreditMemoLine.ShipmentDate
        rblAllowInvoiceDisc.SelectedValue = dbSalesCreditMemoLine.AllowInvoiceDisc
        rblTaxLiable.SelectedValue = dbSalesCreditMemoLine.TaxLiable
        rblNonstock.SelectedValue = dbSalesCreditMemoLine.Nonstock
        rblAllowLineDisc.SelectedValue = dbSalesCreditMemoLine.AllowLineDisc
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbSalesCreditMemoLine As SalesCreditMemoLineRow

            If MemoLineId <> 0 Then
                dbSalesCreditMemoLine = SalesCreditMemoLineRow.GetRow(DB, MemoLineId)
            Else
                dbSalesCreditMemoLine = New SalesCreditMemoLineRow(DB)
            End If
            dbSalesCreditMemoLine.MemoId = txtMemoId.Text
            dbSalesCreditMemoLine.SellToCustomerNo = txtSellToCustomerNo.Text
            dbSalesCreditMemoLine.DocumentNo = txtDocumentNo.Text
            dbSalesCreditMemoLine.LineNo = txtLineNo.Text
            dbSalesCreditMemoLine.Type = txtType.Text
            dbSalesCreditMemoLine.No = txtNo.Text
            dbSalesCreditMemoLine.LocationCode = txtLocationCode.Text
            dbSalesCreditMemoLine.Description = txtDescription.Text
            dbSalesCreditMemoLine.Description2 = txtDescription2.Text
            dbSalesCreditMemoLine.UnitofMeasure = txtUnitofMeasure.Text
            dbSalesCreditMemoLine.Quantity = txtQuantity.Text
            dbSalesCreditMemoLine.UnitPrice = txtUnitPrice.Text
            dbSalesCreditMemoLine.VATPercent = txtVATPercent.Text
            dbSalesCreditMemoLine.LineDiscountPercent = txtLineDiscountPercent.Text
            dbSalesCreditMemoLine.LineDiscountAmount = txtLineDiscountAmount.Text
            dbSalesCreditMemoLine.Amount = txtAmount.Text
            dbSalesCreditMemoLine.AmountIncludingVAT = txtAmountIncludingVAT.Text
            dbSalesCreditMemoLine.GrossWeight = txtGrossWeight.Text
            dbSalesCreditMemoLine.NetWeight = txtNetWeight.Text
            dbSalesCreditMemoLine.UnitsperParcel = txtUnitsperParcel.Text
            dbSalesCreditMemoLine.UnitVolume = txtUnitVolume.Text
            dbSalesCreditMemoLine.ApplToItemEntry = txtApplToItemEntry.Text
            dbSalesCreditMemoLine.JobAppliesToID = txtJobAppliesToID.Text
            dbSalesCreditMemoLine.WorkTypeCode = txtWorkTypeCode.Text
            dbSalesCreditMemoLine.BillToCustomerNo = txtBillToCustomerNo.Text
            dbSalesCreditMemoLine.InvDiscountAmount = txtInvDiscountAmount.Text
            dbSalesCreditMemoLine.TaxAreaCode = txtTaxAreaCode.Text
            dbSalesCreditMemoLine.TaxGroupCode = txtTaxGroupCode.Text
            dbSalesCreditMemoLine.BlanketOrderNo = txtBlanketOrderNo.Text
            dbSalesCreditMemoLine.BlanketOrderLineNo = txtBlanketOrderLineNo.Text
            dbSalesCreditMemoLine.VATBaseAmount = txtVATBaseAmount.Text
            dbSalesCreditMemoLine.UnitCost = txtUnitCost.Text
            dbSalesCreditMemoLine.LineAmount = txtLineAmount.Text
            dbSalesCreditMemoLine.VariantCode = txtVariantCode.Text
            dbSalesCreditMemoLine.BinCode = txtBinCode.Text
            dbSalesCreditMemoLine.QtyPerUnitOfMeasure = txtQtyPerUnitOfMeasure.Text
            dbSalesCreditMemoLine.UnitOfMeasureCode = txtUnitOfMeasureCode.Text
            dbSalesCreditMemoLine.QuantityBase = txtQuantityBase.Text
            dbSalesCreditMemoLine.ResponsibilityCenter = txtResponsibilityCenter.Text
            dbSalesCreditMemoLine.CrossReferenceNo = txtCrossReferenceNo.Text
            dbSalesCreditMemoLine.UnitofMeasureCrossRef = txtUnitofMeasureCrossRef.Text
            dbSalesCreditMemoLine.CrossReferenceType = txtCrossReferenceType.Text
            dbSalesCreditMemoLine.CrossReferenceTypeNo = txtCrossReferenceTypeNo.Text
            dbSalesCreditMemoLine.ItemCategoryCode = txtItemCategoryCode.Text
            dbSalesCreditMemoLine.PurchasingCode = txtPurchasingCode.Text
            dbSalesCreditMemoLine.ProductGroupCode = txtProductGroupCode.Text
            dbSalesCreditMemoLine.ReturnReceiptNo = txtReturnReceiptNo.Text
            dbSalesCreditMemoLine.ReturnReceiptLineNo = txtReturnReceiptLineNo.Text
            dbSalesCreditMemoLine.ReturnReasonCode = txtReturnReasonCode.Text
            dbSalesCreditMemoLine.CustomerDiscGroup = txtCustomerDiscGroup.Text
            dbSalesCreditMemoLine.PackageTrackingNo = txtPackageTrackingNo.Text
            dbSalesCreditMemoLine.ShipmentDate = dtShipmentDate.Value
            dbSalesCreditMemoLine.AllowInvoiceDisc = rblAllowInvoiceDisc.SelectedValue
            dbSalesCreditMemoLine.TaxLiable = rblTaxLiable.SelectedValue
            dbSalesCreditMemoLine.Nonstock = rblNonstock.SelectedValue
            dbSalesCreditMemoLine.AllowLineDisc = rblAllowLineDisc.SelectedValue

            If MemoLineId <> 0 Then
                dbSalesCreditMemoLine.Update()
            Else
                MemoLineId = dbSalesCreditMemoLine.Insert
            End If

            DB.CommitTransaction()


            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?MemoLineId=" & MemoLineId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
