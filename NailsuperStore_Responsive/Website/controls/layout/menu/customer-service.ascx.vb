
Option Strict Off

Imports Components

Partial Class controls_layout_customer_service_menu
    Inherits ModuleControl

    Public Overrides Property Args() As String
        Get
            Return m_Args
        End Get
        Set(ByVal Value As String)
            m_Args = Value
        End Set
    End Property
    Private m_Args As String
    Public htmlMenu As String
   

    Protected dvOrders As DataView
    Protected dvShipping As DataView
    Protected dvLegal As DataView
    Protected dvReturns As DataView
    Protected dvAbout As DataView
    Protected dvContact As DataView
    Protected dvSalon As DataView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitData()
    End Sub
    Private Sub InitData()
        'Orders
        If dvOrders Is Nothing Then
            dvOrders = New DataView
            AddRow(dvOrders, "/service/order.aspx", "")
            AddRow(dvOrders, "/services/order-status.aspx", "Order Status")
            AddRow(dvOrders, "/services/order-changing-cancelling.aspx", "Changing & Canceling")
            AddRow(dvOrders, "/services/order-catalog-quick-order.aspx", "Catalog Quick Order")
            AddRow(dvOrders, "/services/order-payment.aspx", "Payment")
            AddRow(dvOrders, "/services/order-sales-tax.aspx", "Sales Tax")
            AddRow(dvOrders, "/services/order-warranty.aspx", "Warranty")
            AddRow(dvOrders, "/services/order-price-match.aspx", "Price Match")
            AddRow(dvOrders, "/services/order-samples-promotional-items.aspx", "Samples & Promotional Items")
            AddRow(dvOrders, "/services/order-equipment-international-use.aspx", "Equipment for International Use")
            'dvOrders.Sort = "Text"
        End If

        'Shipping
        If dvShipping Is Nothing Then
            dvShipping = New DataView
            AddRow(dvShipping, "/service/shipping.aspx", "")
            AddRow(dvShipping, "/services/free-shipping-policies.aspx", "Free Shipping Info")
            AddRow(dvShipping, "/services/order-shipping-policies.aspx", "Shipping Policies")
            AddRow(dvShipping, "/services/order-delivery-time.aspx", "Delivery Time")
            AddRow(dvShipping, "/services/order-residential-delivery.aspx", "Residential Deliveries")
            AddRow(dvShipping, "/services/order-damaged-shipment.aspx", "Damaged Shipment")
            AddRow(dvShipping, "/services/order-shipping-restrictions.aspx", "Shipping Restrictions")
            AddRow(dvShipping, "/services/order-international-shipment.aspx", "International Shipment")
            AddRow(dvShipping, "/services/order-truck-delivery.aspx", "Truck Deliveries")
            'dvShipping.Sort = "Text"
        End If

        'Legal
        ''  dvLegal = CType(CacheUtils.GetCache("dvLegal"), DataView)
        If dvLegal Is Nothing Then
            dvLegal = New DataView
            AddRow(dvLegal, "/service/privacy.aspx", "")
            AddRow(dvLegal, "/services/legal-forum.aspx", "Forums")
            AddRow(dvLegal, "/services/legal-privacy-policies.aspx", "Privacy Policies")
            AddRow(dvLegal, "/services/legal-jobs-classifieds-tips.aspx", "Jobs, Classifieds, & Tips ")
            AddRow(dvLegal, "/services/legal-copyrights-infringe.aspx", "Copyright Infringement")
            AddRow(dvLegal, "/services/legal-content-complaint.aspx", "Content Complaints")
            AddRow(dvLegal, "/services/legal-user-agreement.aspx", "User Agreement")
            dvLegal.Sort = "Text"
            ''   CacheUtils.SetCache("dvLegal", dvLegal)
        End If


        'Returns
        ''  dvReturns = CType(CacheUtils.GetCache("dvReturns"), DataView)
        If dvReturns Is Nothing Then
            dvReturns = New DataView
            AddRow(dvReturns, "/service/return.aspx", "")
            ' AddRow(dvReturns, "/service/msds.aspx", "")
            AddRow(dvReturns, "/services/returns-policies.aspx", "Return Policies")
            AddRow(dvReturns, "/services/return-restocking-fee-policies.aspx", "Restocking Fee Policies")
            dvReturns.Sort = "Text"
            ''   CacheUtils.SetCache("dvReturns", dvReturns)
        End If


        'About
        ''  dvAbout = CType(CacheUtils.GetCache("dvAbout"), DataView)
        If dvAbout Is Nothing Then
            dvAbout = New DataView
            AddRow(dvAbout, "/service/about.aspx", "")
            AddRow(dvAbout, "/services/about-company.aspx", "About Us")
            AddRow(dvAbout, "/services/our-founder.aspx", "Our Founder")
            AddRow(dvAbout, "/service/about-directions-to-our-warehouse.aspx", "Directions")
            AddRow(dvAbout, "/services/about-hour-of-operation.aspx", "Hours of Operation")
            'dvAbout.Sort = "Text"
            '' CacheUtils.SetCache("dvAbout", dvAbout)
        End If


        'Contact Us
        '' dvContact = CType(CacheUtils.GetCache("dvContact"), DataView)
        If dvContact Is Nothing Then
            dvContact = New DataView
            AddRow(dvContact, "/contact/default.aspx", "")
            AddRow(dvContact, "/contact/generalquestion.aspx", "General Question")
            AddRow(dvContact, "/contact/returnauthorizationrequest.aspx", "Return Authorization Request")
            AddRow(dvContact, "/contact/wholesaleinquiry.aspx", "Wholesale Inquiry")
            AddRow(dvContact, "/contact/itemnotreceived.aspx", "Item Not Received")
            AddRow(dvContact, "/contact/damagedshipment.aspx", "Damaged Shipment")
            AddRow(dvContact, "/contact/returnorderstatus.aspx", "Return Order Status")
            AddRow(dvContact, "/contact/billinginquiry.aspx", "Billing Inquiry")
            If Me.Request.RawUrl.Contains("/contact/priceadjustmentrequest.aspx") Then
                AddRow(dvContact, "/contact/priceadjustmentrequest.aspx", "Price Adjustment Request")
            Else
                AddRow(dvContact, "/contact/priceadjustment.aspx", "Price Adjustment Request")
            End If
            AddRow(dvContact, "/contact/pricematch.aspx", "Price Match Request")
            AddRow(dvContact, "/contact/productwarrantyinformation.aspx", "Product Warranty Information")
            AddRow(dvContact, "/contact/submitanidea.aspx", "Submit an Idea")
            'dvContact.Sort = "Text"
            ''    CacheUtils.SetCache("dvContact", dvContact)
        End If
        If dvSalon Is Nothing Then
            dvSalon = New DataView
            AddRow(dvSalon, "/service/salon-template.aspx", "")
            AddRow(dvSalon, "/services/salon-introduction.aspx", "Introduction")
            AddRow(dvSalon, "/services/salon-get-started.aspx", "Get Started")
            AddRow(dvSalon, "/services/salon-price-list.aspx", "Price List")
            AddRow(dvSalon, "/services/salon-terms-of-use.aspx", "Terms Of Use")
            dvSalon.Sort = "Text"
        End If
    End Sub
    Protected ReadOnly Property HasAccess() As Boolean
        Get
            Return Session("MemberId") <> Nothing
        End Get
    End Property
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

    Protected Function UrlIn(ByVal dv As DataView, ByVal currentFilePath As String) As Boolean
        For Each row As DataRowView In dv
            If row("Url").ToLower = currentFilePath Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Function ListSub(ByVal dvName As String, ByVal className As String) As String
        Dim dv As New DataView
        Select Case dvName
            Case "order"
                dv = dvOrders
            Case "shipping"
                dv = dvShipping
            Case "return"
                dv = dvReturns
            Case "about"
                dv = dvAbout
            Case "privacy"
                dv = dvLegal
            Case "contact"
                dv = dvContact
            Case "salon-template"
                dv = dvSalon
        End Select
        Dim currentPathFile As String = Request.RawUrl.ToLower
        'If (currentPathFile = "/members/referfriend/refer.aspx") Then
        '    currentPathFile = "/members/referfriend/manager.aspx"
        'End If
        Dim str As String = String.Empty
        If dv Is Nothing Then
            Return String.Empty
        End If
        If dv.Count < 1 Then
            Return String.Empty
        End If
        'If UrlIn(dv, currentPathFile) Then
        str &= "<ul class='" & className & "'>"
        For Each row As DataRowView In dv
            If Not row("Url") = "/service/" & dvName & ".aspx" And Not row("Url") = "/contact/default.aspx" Then
                If Not currentPathFile = row("Url") Then
                    str &= "<li ><span><b class='glyphicon arrow-right'></b></span><span><a href=""" & row("Url") & """>" & row("Text") & "</a></span></li>"
                Else
                    str &= "<li class=""select""><span><b class='glyphicon arrow-right'></b></span><span><a>" & row("Text") & "</a></span></li>"
                End If
            End If
        Next
        str &= "</ul>"
        '' End If

        Return str
    End Function
    Public Function GenerateGroupMenu(ByVal groupCode As String, ByVal groupName As String, ByVal linkDefault As String) As String

        Dim result As String = String.Empty
        Dim dv As DataView = GetMenuSource(groupCode)
        Dim childClass As String = "sub"
        If (Request.RawUrl.ToLower = linkDefault) Then
            result = result & "<li class='active'><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        ElseIf (ChildActive(groupCode)) Then
            result = result & "<li class='active'><a class='link' href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        Else
            result = result & "<li><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
            childClass = childClass & " hiddenshow"
        End If
        result = result & ListSub(groupCode, childClass) & "</li>"
        Return result
    End Function
    Private Function GetMenuSource(ByVal groupCode As String) As DataView
        Dim dv As New DataView
        Select Case groupCode
            Case "order"
                dv = dvOrders
            Case "shipping"
                dv = dvShipping
            Case "return"
                dv = dvReturns
            Case "about"
                dv = dvAbout
            Case "privacy"
                dv = dvLegal
            Case "contact"
                dv = dvContact
            Case "salon-template"
                dv = dvSalon
        End Select
        Return dv
    End Function
    Protected Function ChildActive(ByVal groupCode As String) As Boolean
        If (String.IsNullOrEmpty(groupCode)) Then
            Return False
        End If
        Dim dv As DataView = GetMenuSource(groupCode)
        Dim str As String = String.Empty
        If UrlIn(dv, Request.RawUrl.ToLower) Then
            For Each row As DataRowView In dv
                If Not row("Url") = "/service/" & groupCode & ".aspx" And Not row("Url") = "/contact/default.aspx" Then
                    If Not Request.RawUrl.ToLower = row("Url") Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function
End Class