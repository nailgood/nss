Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System

Partial Class _404
    Inherits SitePage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GoPage()
        End If
    End Sub

    Private Sub GoPage()
        Dim url As String = Request.RawUrl.ToString()
        If url.Contains("/embed/how-to-video/") Then
            URLParameters.ChangeUrlError(url)
        End If
        If Request.QueryString("aspxerrorpath") <> Nothing Then
            Dim strOldUrl As String = Request.QueryString("aspxerrorpath")
            Dim strNewUrl As String = String.Empty
            Dim arr As String() = Nothing

            If strOldUrl = "/services/about-directions-to-our-warehouse.aspx" Then
                strNewUrl = "/service/about-directions-to-our-warehouse.aspx"
            ElseIf strOldUrl = "/services/order-catalog quick order.aspx" Then
                strNewUrl = "/services/order-catalog-quick-order.aspx"
            ElseIf strOldUrl = "/service/pricematch.aspx" Then
                strNewUrl = "/contact/pricematch.aspx"
            ElseIf strOldUrl = "/services/order-damaged shipment.aspx" Then
                strNewUrl = "/services/order-damaged-shipment.aspx"
            ElseIf strOldUrl = "/service/returns.aspx" Then
                strNewUrl = "/services/returns-policies.aspx"
            ElseIf strOldUrl = "/services/order-international shipment.aspx" Then
                strNewUrl = "/services/order-international-shipment.aspx"
            ElseIf strOldUrl = "/services/order-sales tax.aspx" Then
                strNewUrl = "/services/order-sales-tax.aspx"
            ElseIf strOldUrl = "/services/order-shipping restrictions.aspx" Then
                strNewUrl = "/services/order-shipping-restrictions.aspx"
            ElseIf strOldUrl = "/forgotpassword.aspx" Then
                strNewUrl = "/members/forgotpassword.aspx"
            ElseIf strOldUrl = "/cs/forums/" Then
                strNewUrl = "/"
            ElseIf strOldUrl.Contains("/includes/") Then
                strNewUrl = strOldUrl.Substring(0, strOldUrl.IndexOf("/includes/"))
            ElseIf strOldUrl.Contains("/assets/") Then
                strNewUrl = strOldUrl.Substring(0, strOldUrl.IndexOf("/assets/"))
            ElseIf strOldUrl.Contains("/images/") Then
                strNewUrl = strOldUrl.Substring(0, strOldUrl.IndexOf("/images/"))
            ElseIf strOldUrl.Contains("/ScriptResource.axd") Then
                strNewUrl = strOldUrl.Substring(0, strOldUrl.IndexOf("/ScriptResource.axd"))
            ElseIf strOldUrl.Contains("/WebResource.axd") Then
                strNewUrl = strOldUrl.Substring(0, strOldUrl.IndexOf("/WebResource.axd"))
            ElseIf strOldUrl.Contains("listproductreview.aspx") Then
                strNewUrl = "/product-reviews"
            ElseIf strOldUrl.Contains("listorderreview.aspx") Then
                strNewUrl = "/order-reviews"
            Else
                ' Else
                'Old url category ex:URL: /Bugs-and-Animals/287.aspx
                If strOldUrl.Contains("/") And strOldUrl.Contains(".aspx") Then
                    strOldUrl = strOldUrl.Replace(".aspx", "")
                    strOldUrl = strOldUrl.Substring(strOldUrl.LastIndexOf("/") + 1)

                    If IsNumeric(strOldUrl) Then
                        strNewUrl = "/301.aspx?DepartmentId=" & strOldUrl
                    End If
                End If
            End If

            If strNewUrl.Length > 0 Then
                Utility.Common.Redirect301(strNewUrl)
            End If

            'Response.Write(strOldUrl)
        End If

    End Sub


End Class
