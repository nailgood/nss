Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Member_creditmemo_view
    Inherits SitePage

    Protected MemoId As Integer
    Protected c As SalesCreditMemoHeaderRow
    Protected TaxAmount As Double = Nothing
    Protected ShipAmount As Double = Nothing
    Protected TotalLineAmount As Double = Nothing

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            Response.Redirect("/members/login.aspx")
        End If
        If Not IsPostBack Then
            Dim dbMember As MemberRow = MemberRow.GetRow(Session("memberId"))
            MemoId = DB.ExecuteScalar("SELECT MemoId FROM salescreditmemoheader WHERE MemberId=" & dbMember.MemberId & " AND MemoId=" & Convert.ToInt32(Request.QueryString("MemoId")))
            If MemoId = 0 Then
                Response.Redirect("/members/creditmemo/")
            End If
            c = SalesCreditMemoHeaderRow.GetRow(DB, MemoId)
            ltrPageTitle.Text = "View Credit #" & c.No
            ltrBilling.Text = GetBillingAddress(c)
            ltrShipping.Text = GetShippingAddress(c)
            LoadMemmoLine(c)
        End If


    End Sub
    Private Function GetBillingAddress(ByVal c As SalesCreditMemoHeaderRow) As String
        Dim result As String = String.Empty
        Dim country As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & Database.Quote(c.BillToCountryCode))
        result = "<li>" & c.BillToName & " " & c.BillToName2 & "</li>"
        If Not String.IsNullOrEmpty(c.BillToAddress) Then
            result &= "<li>" & c.BillToAddress.Trim() & "</li>"
        End If
        If Not String.IsNullOrEmpty(c.BillToAddress2) Then
            result &= "<li>" & c.BillToAddress2.Trim() & "</li>"
        End If
        result &= "<li>" & c.BillToCity & ", " & c.BillToCounty & "," & c.BillToPostCode & "," & country & "</li>"
        Return result
    End Function
    Private Function GetShippingAddress(ByVal c As SalesCreditMemoHeaderRow) As String
        Dim result As String = String.Empty
        Dim country As String = DB.ExecuteScalar("select top 1 countryname from country where countrycode = " & Database.Quote(c.BillToCountryCode))
        result = "<li>" & c.ShipToName & " " & c.ShipToName2 & "</li>"
        If Not String.IsNullOrEmpty(c.ShipToAddress) Then
            result &= "<li>" & c.ShipToAddress.Trim() & "</li>"
        End If
        If Not String.IsNullOrEmpty(c.ShipToAddress2) Then
            result &= "<li>" & c.ShipToAddress2.Trim() & "</li>"
        End If
        result &= "<li>" & c.ShipToCity & ", " & c.ShipToCounty & "," & c.ShipToPostCode & "," & country & "</li>"
        Return result
    End Function
    Private Sub LoadMemmoLine(ByVal c As SalesCreditMemoHeaderRow)
        rptLine.DataSource = c.GetLineItems()
        rptLine.DataBind()
        Dim dtDes As DataTable = DB.GetDataTable("select * from salescreditmemoline where memoid = " & c.MemoId & " and type = ''")
        If Not dtDes Is Nothing Then
            If dtDes.Rows.Count > 0 Then
                For Each row As DataRow In dtDes.Rows
                    ltrDesc.Text = row("Description").ToString.Trim() & "<br/>"
                Next
            End If
        End If
        If (TotalLineAmount > 0) Then
            ltrSubTotal.Text = FormatCurrency(TotalLineAmount)
            liSubtotal.Visible = True
        End If
        TaxAmount = DB.ExecuteScalar("select coalesce(sum(lineamount),0) from salescreditmemoline where memoid = " & c.MemoId & " and type = 'g/l account'")
        ShipAmount = DB.ExecuteScalar("select coalesce(sum(lineamount),0) from salescreditmemoline where memoid = " & c.MemoId & " and type = 'charge (item)'")
        If TaxAmount > 0 Then
            liSaleTax.Visible = True
            ltrTax.Text = FormatCurrency(TaxAmount)
        End If
        If ShipAmount > 0 Then
            liLessShipping.Visible = True
            ltrLessShipping.Text = FormatCurrency(ShipAmount)
        End If
        liCreditTotal.Visible = True
        ltrCreditTotal.Text = FormatCurrency(TotalLineAmount + TaxAmount + ShipAmount)
    End Sub

    Private Sub rptLine_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptLine.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim si As New StoreItemRow
            If Not IsDBNull(e.Item.DataItem("ItemId")) Then si = StoreItemRow.GetRow(DB, e.Item.DataItem("ItemId"))
            Dim unit As String = e.Item.DataItem("unitofmeasure")
            Dim strItemName As String = si.ItemName
            Dim ltrName As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrName"), System.Web.UI.WebControls.Literal)
            Dim ltrSKU As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrSKU"), System.Web.UI.WebControls.Literal)
            ltrName.Text = strItemName
            ltrSKU.Text = si.SKU
            If Not String.IsNullOrEmpty(unit) Then
                ltrName.Text = ltrName.Text & "<span class='unit'> - " & unit & "</span>"
                Dim ltrUnit As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrUnit"), System.Web.UI.WebControls.Literal)
                ltrUnit.Text = unit
            End If
            Dim ltrImage As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrImage"), System.Web.UI.WebControls.Literal)
            ltrImage.Text = IIf(String.IsNullOrEmpty(si.Image), "na.jpg", si.Image)
            ltrImage.Text = "<img src='" & Utility.ConfigData.GlobalRefererName & "/assets/items/cart/" & ltrImage.Text & "' alt='" & si.ItemName & "'/>"
            Dim ltrQty As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrQty"), System.Web.UI.WebControls.Literal)
            Dim ltrSmallQty As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrSmallQty"), System.Web.UI.WebControls.Literal)
            ltrQty.Text = e.Item.DataItem("Quantity").ToString()
            ltrSmallQty.Text = ltrQty.Text

            Dim ltrSmallUnitPrice As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrSmallUnitPrice"), System.Web.UI.WebControls.Literal)
            Dim ltrUnitPrice As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrUnitPrice"), System.Web.UI.WebControls.Literal)
            If Not String.IsNullOrEmpty(e.Item.DataItem("unitprice").ToString()) Then
                ltrSmallUnitPrice.Text = e.Item.DataItem("unitprice")
                ltrUnitPrice.Text = e.Item.DataItem("unitprice")
            Else
                ltrSmallUnitPrice.Text = 0
                ltrUnitPrice.Text = 0
            End If
            Dim ltrSmallRestock As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrSmallRestock"), System.Web.UI.WebControls.Literal)
            Dim ltrRestock As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrRestock"), System.Web.UI.WebControls.Literal)
            If Not String.IsNullOrEmpty(e.Item.DataItem("linediscountpercent").ToString()) Then
                Dim Restock As Double = e.Item.DataItem("linediscountpercent")

                If Restock > 0 Then
                    ltrSmallRestock.Text = Restock & "%"
                    ltrRestock.Text = Restock & "%"
                End If

            End If

            Dim ltrTotal As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrTotal"), System.Web.UI.WebControls.Literal)
            ltrTotal.Text = FormatCurrency(e.Item.DataItem("LineAmount"))

            TotalLineAmount += e.Item.DataItem("lineamount")
        End If
    End Sub
End Class