Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_estimate_shipping
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Cart Is Nothing Then
            Cart = Session("cartRender")
        End If

        If Cart Is Nothing Then
            Dim orderId As Integer = Session("OrderId")
            If orderId < 1 Then
                orderId = Utility.Common.GetOrderIdFromCartCookie()
            End If
            If orderId > 0 Then
                Cart = New ShoppingCart(DB, orderId, False)
            End If
        End If

        If Not Cart Is Nothing AndAlso Not Cart.Order Is Nothing AndAlso Cart.GetCartItemCount() > 0 Then
            GetCountry()

            Try
                Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
                If memberId > 0 AndAlso Cart IsNot Nothing Then
                    Dim so As StoreOrderRow = Cart.Order
                    If so.ShipToCountry <> "" Then
                        If so.ShipToCountry = "US" Then
                            txtZipCode.Text = so.ShipToZipcode
                            Session("EstimateShipping") = so.ShipToCountry & "|" & so.ShipToZipcode
                        Else
                            drpCountry.SelectedValue = so.ShipToCountry
                            Session("EstimateShipping") = so.ShipToCountry
                            trZipCode.Attributes.Add("style", "display:none")
                        End If
                    End If
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "Estimate Shipping >> Fill Member Information", ex.ToString())
            End Try
        Else
            Me.Visible = False
        End If

    End Sub
    Public Sub GetCountry()

        drpCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpCountry.DataBind()
        drpCountry.Items.Remove(drpCountry.Items.FindByValue("AK"))
        drpCountry.Items.Remove(drpCountry.Items.FindByValue("HI"))
        drpCountry.SelectedValue = "US"
    End Sub
End Class
