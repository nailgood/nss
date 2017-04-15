Imports Components

Partial Class service_default
    Inherits SitePage

    Protected dvOrders As New DataView
    Protected dvMyAccount As New DataView
    Protected dvLegal As New DataView
    Protected dvReturns As New DataView
    Protected dvAbout As New DataView

    Private Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Orders
        AddRow(dvOrders, "/service/order.aspx", " Ordering & Shipping")
        '' AddRow(dvOrders, "/services/order-ach payment.aspx", "ACH Payment")
        AddRow(dvOrders, "/services/order-catalog-quick-order.aspx", "Catalog Quick Order")
        AddRow(dvOrders, "/services/order-changing-cancelling.aspx", "Changing & Cancelling")
        AddRow(dvOrders, "/services/order-damaged-shipment.aspx", "Damaged Shipment")
        AddRow(dvOrders, "/services/order-delivery-time.aspx", "Delivery Time")
        AddRow(dvOrders, "/services/order-truck delivery.aspx", "Truck Delivery")
        AddRow(dvOrders, "/services/order-international-shipment.aspx", "International Shipment")
        AddRow(dvOrders, "/services/order-warranty.aspx", "Warranty")
        AddRow(dvOrders, "/services/order-status.aspx", "Order Status")
        AddRow(dvOrders, "/services/order-payment.aspx", "Payment")
        AddRow(dvOrders, "/services/order-price-match.aspx", "Price Match")
        AddRow(dvOrders, "/services/order-sales-tax.aspx", "Sales Tax")
        AddRow(dvOrders, "/services/order-shipping-policies.aspx", "Shipping Policies")
        AddRow(dvOrders, "/services/order-shipping-restrictions.aspx", "Shipping Restrictions")
        dvOrders.Sort = "Text"

        'My Account
        AddRow(dvMyAccount, "/members/default.aspx", " My Account")
        AddRow(dvMyAccount, "/services/ma-forgotpassword.aspx", "Forgot Password")
        AddRow(dvMyAccount, "/services/ma-editlogin.aspx", "Edit Login")
        AddRow(dvMyAccount, "/services/ma-myusername.aspx", "My Username")
        AddRow(dvMyAccount, "/services/ma-order-history.aspx", "Order History")
        AddRow(dvMyAccount, "/services/ma-unsubscribe.aspx", "Unsubscribe")
        dvMyAccount.Sort = "Text"

        'Legal
        AddRow(dvLegal, "/service/privacy.aspx", " Legal Notice & Privacy")
        AddRow(dvLegal, "/services/legal-forum.aspx", "Forums")
        AddRow(dvLegal, "/services/legal-privacy-policies.aspx", "Privacy Policies")
        AddRow(dvLegal, "/services/legal-jobs-classifieds-tips.aspx", "Jobs, Classifieds, & Tips ")
        AddRow(dvLegal, "/services/legal-copyrights-infringe.aspx", "Copyright Infringement")
        AddRow(dvLegal, "/services/legal-content-complaint.aspx", "Content Complaints")
        AddRow(dvLegal, "/services/legal-user agreement.aspx", "User Agreement")
        dvLegal.Sort = "Text"

        'Returns
        AddRow(dvReturns, "/service/return.aspx", " Returning & Exchange")
        AddRow(dvReturns, "/services/returns-policies.aspx", "Return Policies")
        dvReturns.Sort = "Text"

        'About
        AddRow(dvAbout, "/service/about.aspx", " About Us")
        AddRow(dvAbout, "/services/about-company.aspx", "Company")
        dvAbout.Sort = "Text"
    End Sub

    Protected Sub AddRow(ByRef dv As DataView, ByVal Url As String, ByVal Text As String)
        If dv.Table Is Nothing Then
            Dim dt As New DataTable
            dt.TableName = "tbl"
            dt.Columns.Add("Url", GetType(String))
            dt.Columns.Add("Text", GetType(String))
            dv.Table = dt
        End If

        Dim row As DataRow = dv.Table.NewRow
        row("Url") = Url
        row("Text") = Text
        dv.Table.Rows.Add(row)
    End Sub

    Protected Function UrlIn(ByVal dv As DataView) As Boolean
        For Each row As DataRowView In dv
            If row("Url").ToLower = Request.Path.ToLower Then
                Return True
            End If
        Next
    End Function

End Class
