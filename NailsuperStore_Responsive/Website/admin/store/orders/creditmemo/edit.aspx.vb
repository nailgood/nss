Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_orders_creditmemo_Edit
    Inherits AdminPage

    Protected MemoId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        MemoId = Convert.ToInt32(Request("MemoId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If MemoId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbSalesCreditMemoHeader As SalesCreditMemoHeaderRow = SalesCreditMemoHeaderRow.GetRow(DB, MemoId)
        txtSellToCustomerNo.Text = dbSalesCreditMemoHeader.SellToCustomerNo
        txtNo.Text = dbSalesCreditMemoHeader.No
        txtBillToCustomerNo.Text = dbSalesCreditMemoHeader.BillToCustomerNo
        txtBillToName.Text = dbSalesCreditMemoHeader.BillToName
        txtBillToName2.Text = dbSalesCreditMemoHeader.BillToName2
        txtBillToAddress.Text = dbSalesCreditMemoHeader.BillToAddress
        txtBillToAddress2.Text = dbSalesCreditMemoHeader.BillToAddress2
        txtBillToCity.Text = dbSalesCreditMemoHeader.BillToCity
        txtBillToContact.Text = dbSalesCreditMemoHeader.BillToContact
        txtYourReference.Text = dbSalesCreditMemoHeader.YourReference
        txtShipToCode.Text = dbSalesCreditMemoHeader.ShipToCode
        txtShipToName.Text = dbSalesCreditMemoHeader.ShipToName
        txtShipToName2.Text = dbSalesCreditMemoHeader.ShipToName2
        txtShipToAddress.Text = dbSalesCreditMemoHeader.ShipToAddress
        txtShipToAddress2.Text = dbSalesCreditMemoHeader.ShipToAddress2
        txtShipToCity.Text = dbSalesCreditMemoHeader.ShipToCity
        txtShipToContact.Text = dbSalesCreditMemoHeader.ShipToContact
        txtPostingDescription.Text = dbSalesCreditMemoHeader.PostingDescription
        txtPaymentTermsCode.Text = dbSalesCreditMemoHeader.PaymentTermsCode
        txtPaymentDiscountPercent.Text = dbSalesCreditMemoHeader.PaymentDiscountPercent
        txtShipmentMethodCode.Text = dbSalesCreditMemoHeader.ShipmentMethodCode
        txtLocationCode.Text = dbSalesCreditMemoHeader.LocationCode
        txtCurrencyCode.Text = dbSalesCreditMemoHeader.CurrencyCode
        txtCustomerPriceGroup.Text = dbSalesCreditMemoHeader.CustomerPriceGroup
        txtInvoiceDiscCode.Text = dbSalesCreditMemoHeader.InvoiceDiscCode
        txtCustomerDiscGroup.Text = dbSalesCreditMemoHeader.CustomerDiscGroup
        txtLanguageCode.Text = dbSalesCreditMemoHeader.LanguageCode
        txtSalespersonCode.Text = dbSalesCreditMemoHeader.SalespersonCode
        txtAppliestoDocType.Text = dbSalesCreditMemoHeader.AppliestoDocType
        txtAppliestoDocNo.Text = dbSalesCreditMemoHeader.AppliestoDocNo
        txtBalAccountNo.Text = dbSalesCreditMemoHeader.BalAccountNo
        txtJobNo.Text = dbSalesCreditMemoHeader.JobNo
        txtAmount.Text = dbSalesCreditMemoHeader.Amount
        txtAmountIncludingVAT.Text = dbSalesCreditMemoHeader.AmountIncludingVAT
        txtVATRegistrationNo.Text = dbSalesCreditMemoHeader.VATRegistrationNo
        txtReasonCode.Text = dbSalesCreditMemoHeader.ReasonCode
        txtTransactionType.Text = dbSalesCreditMemoHeader.TransactionType
        txtTransportMethod.Text = dbSalesCreditMemoHeader.TransportMethod
        txtSellToCustomerName.Text = dbSalesCreditMemoHeader.SellToCustomerName
        txtSellToCustomerName2.Text = dbSalesCreditMemoHeader.SellToCustomerName2
        txtSellToAddress.Text = dbSalesCreditMemoHeader.SellToAddress
        txtSellToAddress2.Text = dbSalesCreditMemoHeader.SellToAddress2
        txtSellToCity.Text = dbSalesCreditMemoHeader.SellToCity
        txtSellToContact.Text = dbSalesCreditMemoHeader.SellToContact
        txtBillToPostCode.Text = dbSalesCreditMemoHeader.BillToPostCode
        txtBillToCounty.Text = dbSalesCreditMemoHeader.BillToCounty
        txtBillToCountryCode.Text = dbSalesCreditMemoHeader.BillToCountryCode
        txtSellToPostCode.Text = dbSalesCreditMemoHeader.SellToPostCode
        txtSellToCounty.Text = dbSalesCreditMemoHeader.SellToCounty
        txtSellToCountryCode.Text = dbSalesCreditMemoHeader.SellToCountryCode
        txtShipToPostCode.Text = dbSalesCreditMemoHeader.ShipToPostCode
        txtShipToCounty.Text = dbSalesCreditMemoHeader.ShipToCounty
        txtShipToCountryCode.Text = dbSalesCreditMemoHeader.ShipToCountryCode
        txtBalAccountType.Text = dbSalesCreditMemoHeader.BalAccountType
        txtExternalDocumentNo.Text = dbSalesCreditMemoHeader.ExternalDocumentNo
        txtPaymentMethodCode.Text = dbSalesCreditMemoHeader.PaymentMethodCode
        txtUserID.Text = dbSalesCreditMemoHeader.UserID
        txtSourceCode.Text = dbSalesCreditMemoHeader.SourceCode
        txtTaxAreaCode.Text = dbSalesCreditMemoHeader.TaxAreaCode
        txtCampaignNo.Text = dbSalesCreditMemoHeader.CampaignNo
        txtSellToContactNo.Text = dbSalesCreditMemoHeader.SellToContactNo
        txtBillToContactNo.Text = dbSalesCreditMemoHeader.BillToContactNo
        txtResponsibilityCenter.Text = dbSalesCreditMemoHeader.ResponsibilityCenter
        dtPosting.Value = dbSalesCreditMemoHeader.Posting
        dtShipment.Value = dbSalesCreditMemoHeader.Shipment
        dtDue.Value = dbSalesCreditMemoHeader.Due
        dtPmtDiscount.Value = dbSalesCreditMemoHeader.PmtDiscount
        dtDocumentdatetime.Value = dbSalesCreditMemoHeader.Documentdatetime
        rblPricesIncludingVAT.SelectedValue = dbSalesCreditMemoHeader.PricesIncludingVAT
        rblTaxLiable.SelectedValue = dbSalesCreditMemoHeader.TaxLiable
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbSalesCreditMemoHeader As SalesCreditMemoHeaderRow

            If MemoId <> 0 Then
                dbSalesCreditMemoHeader = SalesCreditMemoHeaderRow.GetRow(DB, MemoId)
            Else
                dbSalesCreditMemoHeader = New SalesCreditMemoHeaderRow(DB)
            End If
            dbSalesCreditMemoHeader.SellToCustomerNo = txtSellToCustomerNo.Text
            dbSalesCreditMemoHeader.No = txtNo.Text
            dbSalesCreditMemoHeader.BillToCustomerNo = txtBillToCustomerNo.Text
            dbSalesCreditMemoHeader.BillToName = txtBillToName.Text
            dbSalesCreditMemoHeader.BillToName2 = txtBillToName2.Text
            dbSalesCreditMemoHeader.BillToAddress = txtBillToAddress.Text
            dbSalesCreditMemoHeader.BillToAddress2 = txtBillToAddress2.Text
            dbSalesCreditMemoHeader.BillToCity = txtBillToCity.Text
            dbSalesCreditMemoHeader.BillToContact = txtBillToContact.Text
            dbSalesCreditMemoHeader.YourReference = txtYourReference.Text
            dbSalesCreditMemoHeader.ShipToCode = txtShipToCode.Text
            dbSalesCreditMemoHeader.ShipToName = txtShipToName.Text
            dbSalesCreditMemoHeader.ShipToName2 = txtShipToName2.Text
            dbSalesCreditMemoHeader.ShipToAddress = txtShipToAddress.Text
            dbSalesCreditMemoHeader.ShipToAddress2 = txtShipToAddress2.Text
            dbSalesCreditMemoHeader.ShipToCity = txtShipToCity.Text
            dbSalesCreditMemoHeader.ShipToContact = txtShipToContact.Text
            dbSalesCreditMemoHeader.PostingDescription = txtPostingDescription.Text
            dbSalesCreditMemoHeader.PaymentTermsCode = txtPaymentTermsCode.Text
            dbSalesCreditMemoHeader.PaymentDiscountPercent = txtPaymentDiscountPercent.Text
            dbSalesCreditMemoHeader.ShipmentMethodCode = txtShipmentMethodCode.Text
            dbSalesCreditMemoHeader.LocationCode = txtLocationCode.Text
            dbSalesCreditMemoHeader.CurrencyCode = txtCurrencyCode.Text
            dbSalesCreditMemoHeader.CustomerPriceGroup = txtCustomerPriceGroup.Text
            dbSalesCreditMemoHeader.InvoiceDiscCode = txtInvoiceDiscCode.Text
            dbSalesCreditMemoHeader.CustomerDiscGroup = txtCustomerDiscGroup.Text
            dbSalesCreditMemoHeader.LanguageCode = txtLanguageCode.Text
            dbSalesCreditMemoHeader.SalespersonCode = txtSalespersonCode.Text
            dbSalesCreditMemoHeader.AppliestoDocType = txtAppliestoDocType.Text
            dbSalesCreditMemoHeader.AppliestoDocNo = txtAppliestoDocNo.Text
            dbSalesCreditMemoHeader.BalAccountNo = txtBalAccountNo.Text
            dbSalesCreditMemoHeader.JobNo = txtJobNo.Text
            dbSalesCreditMemoHeader.Amount = txtAmount.Text
            dbSalesCreditMemoHeader.AmountIncludingVAT = txtAmountIncludingVAT.Text
            dbSalesCreditMemoHeader.VATRegistrationNo = txtVATRegistrationNo.Text
            dbSalesCreditMemoHeader.ReasonCode = txtReasonCode.Text
            dbSalesCreditMemoHeader.TransactionType = txtTransactionType.Text
            dbSalesCreditMemoHeader.TransportMethod = txtTransportMethod.Text
            dbSalesCreditMemoHeader.SellToCustomerName = txtSellToCustomerName.Text
            dbSalesCreditMemoHeader.SellToCustomerName2 = txtSellToCustomerName2.Text
            dbSalesCreditMemoHeader.SellToAddress = txtSellToAddress.Text
            dbSalesCreditMemoHeader.SellToAddress2 = txtSellToAddress2.Text
            dbSalesCreditMemoHeader.SellToCity = txtSellToCity.Text
            dbSalesCreditMemoHeader.SellToContact = txtSellToContact.Text
            dbSalesCreditMemoHeader.BillToPostCode = txtBillToPostCode.Text
            dbSalesCreditMemoHeader.BillToCounty = txtBillToCounty.Text
            dbSalesCreditMemoHeader.BillToCountryCode = txtBillToCountryCode.Text
            dbSalesCreditMemoHeader.SellToPostCode = txtSellToPostCode.Text
            dbSalesCreditMemoHeader.SellToCounty = txtSellToCounty.Text
            dbSalesCreditMemoHeader.SellToCountryCode = txtSellToCountryCode.Text
            dbSalesCreditMemoHeader.ShipToPostCode = txtShipToPostCode.Text
            dbSalesCreditMemoHeader.ShipToCounty = txtShipToCounty.Text
            dbSalesCreditMemoHeader.ShipToCountryCode = txtShipToCountryCode.Text
            dbSalesCreditMemoHeader.BalAccountType = txtBalAccountType.Text
            dbSalesCreditMemoHeader.ExternalDocumentNo = txtExternalDocumentNo.Text
            dbSalesCreditMemoHeader.PaymentMethodCode = txtPaymentMethodCode.Text
            dbSalesCreditMemoHeader.UserID = txtUserID.Text
            dbSalesCreditMemoHeader.SourceCode = txtSourceCode.Text
            dbSalesCreditMemoHeader.TaxAreaCode = txtTaxAreaCode.Text
            dbSalesCreditMemoHeader.CampaignNo = txtCampaignNo.Text
            dbSalesCreditMemoHeader.SellToContactNo = txtSellToContactNo.Text
            dbSalesCreditMemoHeader.BillToContactNo = txtBillToContactNo.Text
            dbSalesCreditMemoHeader.ResponsibilityCenter = txtResponsibilityCenter.Text
            dbSalesCreditMemoHeader.Posting = dtPosting.Value
            dbSalesCreditMemoHeader.Shipment = dtShipment.Value
            dbSalesCreditMemoHeader.Due = dtDue.Value
            dbSalesCreditMemoHeader.PmtDiscount = dtPmtDiscount.Value
            dbSalesCreditMemoHeader.Documentdatetime = dtDocumentdatetime.Value
            dbSalesCreditMemoHeader.PricesIncludingVAT = rblPricesIncludingVAT.SelectedValue
            dbSalesCreditMemoHeader.TaxLiable = rblTaxLiable.SelectedValue

            If MemoId <> 0 Then
                dbSalesCreditMemoHeader.Update()
            Else
                MemoId = dbSalesCreditMemoHeader.Insert
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
        Response.Redirect("delete.aspx?MemoId=" & MemoId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
