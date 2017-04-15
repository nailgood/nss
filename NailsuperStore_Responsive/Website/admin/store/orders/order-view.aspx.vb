Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports Utility

Partial Class admin_store_orders_order_view
    Inherits AdminPage

    Protected params As String
    Protected DisplayOrderNo As String
    Protected OrderNo As String
    Protected sc As ShoppingCart
    Protected dbOrder As StoreOrderRow
    Private dsCartItems As DataSet
    Private dsRecipients As DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        OrderNo = CType(Request("OrderNo"), String)
        DisplayOrderNo = DB.ExecuteScalar("SELECT Coalesce(SequentialOrderNo,OrderNo) As DisplayOrderNo FROM StoreOrder WHERE OrderNo = " & DB.Quote(OrderNo))
        dbOrder = StoreOrderRow.GetRowByOrderNo(DB, OrderNo)
        If Not IsPostBack Then
            LoadFromDb()
        End If
    End Sub

    Private Sub LoadFromDb()
        BillingState.Items.AddRange(StateRow.GetStateList().ToArray())
        BillingState.DataBind()

        SQL = ""
        SQL &= " select * from StoreCartItem where OrderId = " & DB.Quote(dbOrder.OrderId)
        dsCartItems = DB.GetDataSet(SQL)

        SQL = ""
        SQL &= "select case when sor.label = 'myself' then '0' else sor.label end as sortname, * from StoreOrderRecipient sor where sor.OrderId = " & DB.Quote(dbOrder.OrderId) & " order by sortname"
        dsRecipients = DB.GetDataSet(SQL)
        rptRecipients.DataSource = dsRecipients.Tables(0).DefaultView
        rptRecipients.DataBind()

        ltlCardType.Text = CardTypeRow.GetRow(DB, dbOrder.CardTypeId).Name
        ltlCardHolderName.Text = dbOrder.CardHolderName
        'ltlCardNumber.Text = dbOrder.StarredCardNumber
        ltlCardNumber.Text = dbOrder.CardNumber
        ltlCIDNumber.Text = dbOrder.StarredCIDNumber
        ltlExpirationDate.Text = Month(dbOrder.ExpirationDate) & "/" & Year(dbOrder.ExpirationDate)

        BillingFirstName.Text = dbOrder.BillToName
        BillingLastName.Text = dbOrder.BillToName2
        Email.Text = dbOrder.Email
        BillingAddress1.Text = dbOrder.BillToAddress
        BillingAddress2.Text = dbOrder.BillToAddress2
        BillingCity.Text = dbOrder.BillToCity
        BillingState.SelectedValue = dbOrder.BillToCounty
        BillingZip.Value = dbOrder.BillToZipcode
        BillingPhone.Value = dbOrder.BillToPhone
    End Sub

    Private Sub rptRecipients_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptRecipients.ItemDataBound
        If Not (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Exit Sub
        End If

        Dim rptBag As Repeater = CType(e.Item.FindControl("rptBag"), Repeater)
        dsCartItems.Tables(0).DefaultView.RowFilter = "AddressId = " & e.Item.DataItem("AddressId")

        AddHandler rptBag.ItemDataBound, AddressOf rptBag_ItemDataBound
        rptBag.DataSource = dsCartItems.Tables(0).DefaultView
        rptBag.DataBind()

        Dim trGiftWrapping As HtmlTableRow = CType(e.Item.FindControl("trGiftWrapping"), HtmlTableRow)
        If e.Item.DataItem("GiftWrapping") = 0 Then
            trGiftWrapping.Visible = False
        End If

        Dim trDeliveryUpgrade As HtmlTableRow = CType(e.Item.FindControl("trDeliveryUpgrade"), HtmlTableRow)
        If e.Item.DataItem("DeliveryUpgrade") = 0 Then
            trDeliveryUpgrade.Visible = False
        End If
        Dim trDeliverySurcharge As HtmlTableRow = CType(e.Item.FindControl("trDeliverySurcharge"), HtmlTableRow)
        If e.Item.DataItem("DeliverySurcharge") = 0 Then
            trDeliverySurcharge.Visible = False
        End If
        Dim trOffshoreShipping As HtmlTableRow = CType(e.Item.FindControl("trOffshoreShipping"), HtmlTableRow)
        If e.Item.DataItem("OffshoreShipping") = 0 Then
            trOffshoreShipping.Visible = False
        End If

        Dim trDivider As HtmlTableRow = CType(e.Item.FindControl("trDivider"), HtmlTableRow)
        Dim trSubtotal As HtmlTableRow = CType(e.Item.FindControl("trSubtotal"), HtmlTableRow)
        Dim trShipping As HtmlTableRow = CType(e.Item.FindControl("trShipping"), HtmlTableRow)
        Dim trTax As HtmlTableRow = CType(e.Item.FindControl("trTax"), HtmlTableRow)
        Dim trGrandTotal As HtmlTableRow = CType(e.Item.FindControl("trGrandTotal"), HtmlTableRow)

        If dsRecipients.Tables(0).Rows.Count = 1 Then
            trGiftWrapping.Visible = False
            trDeliveryUpgrade.Visible = False
            trDeliverySurcharge.Visible = False
            trOffshoreShipping.Visible = False
            trDivider.Visible = False
            trSubtotal.Visible = False
            trShipping.Visible = False
            trTax.Visible = False
            trGrandTotal.Visible = False
        End If
    End Sub

    Private Sub rptBag_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If Not (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Exit Sub
        End If
        Dim ltlBackorderDate As Literal = e.Item.FindControl("ltlBackorderDate")
        If IsDate(e.Item.DataItem("ShipmentDate")) And Convert.ToInt32(e.Item.DataItem("IsBackorder")) = 1 Then ltlBackorderDate.Text = "<br><span class=blkten>estimated ship date: " & Convert.ToString(FormatDateTime(e.Item.DataItem("ShipmentDate"), vbShortDate)) & "</span>"
        Dim divGiftWrap As HtmlGenericControl = e.Item.FindControl("divGiftWrap")
        divGiftWrap.Visible = e.Item.DataItem("IsGiftWrap")
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRowByOrderNo(DB, OrderNo)
        dbOrder.BillToName = BillingFirstName.Text
        dbOrder.BillToName2 = BillingLastName.Text
        dbOrder.Email = Email.Text
        dbOrder.BillToAddress = BillingAddress1.Text
        dbOrder.BillToAddress2 = BillingAddress2.Text
        dbOrder.BillToCity = BillingCity.Text
        dbOrder.BillToCounty = BillingState.SelectedValue
        dbOrder.BillToZipcode = BillingZip.Value
        dbOrder.BillToPhone = BillingPhone.Value
        Try
            DB.BeginTransaction()
            'ShoppingCart.UpdateAdminOrder(DB, dbOrder)
            DB.CommitTransaction()
        Catch ex As Exception
            DB.RollbackTransaction()
        End Try
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
