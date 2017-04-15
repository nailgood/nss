
Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_shipping_option
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Private m_Order As StoreOrderRow
    Private m_ShippingMethod As Integer
    Public styleLabelDisable As String = ""
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Public Property Order() As StoreOrderRow
        Set(ByVal value As StoreOrderRow)
            m_Order = value
        End Set
        Get
            Return m_Order
        End Get
    End Property
    Public Property ShippingMethod() As Integer
        Set(ByVal value As Integer)
            m_ShippingMethod = value
        End Set
        Get
            Return m_ShippingMethod
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Cart Is Nothing AndAlso Not Session("cartRender") Is Nothing Then
            Cart = Session("cartRender")

        End If

        If Not Cart Is Nothing Then
            If Order Is Nothing Then
                Order = Cart.Order
                If ShippingMethod < 1 Then
                    ShippingMethod = Cart.Order.CarrierType
                End If
            End If
        End If

        If Not Cart Is Nothing AndAlso Not Order Is Nothing AndAlso Cart.GetCartItemCount() > 0 Then
            
            Dim sm As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, ShippingMethod)
            'If sm.Insurance > 0 Then
            '    pnlInsurance.Visible = True
            '    lblInsurance.Text = "<span class='checkbox-label'>Shipping insurance <span  class='number'>" & FormatCurrency(sm.Insurance) & " </span>"
            '    If Order.ShipmentInsured Then chkInsurance.Checked = True
            'Else
            pnlInsurance.Visible = False
            'End If
            Dim UPSAddress As New UPS
            Dim isUPS As Boolean = False
            If ShippingMethod = Utility.Common.TruckShippingId Then
                isUPS = False
            Else
                isUPS = True
            End If
            Dim Signature As Double = 0
            Dim Sql As String = String.Empty
            If Cart.CheckWeightCartItem() > 0 Then
                Sql = "select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType)  " & vbCrLf & _
                        "where orderid = " & Order.OrderId & " and  type = 'item'  and Code in(" & Utility.Common.USShippingCode & ")"
                Signature = ShipmentMethod.GetValue(DB.ExecuteScalar(Sql), Utility.Common.ShipmentValue.Signature)
                If (Order.ShipToAddressType = 2 Or Order.ShipToAddressType = 0) And Signature > 0 And isUPS = True Then
                    pnlSignature.Visible = True
                    ltrSignature.Text = "Signature Confirmation Service <span class=""number"">" & FormatCurrency(sm.Signature) & "</span>"
                    If Order.IsSignatureConfirmation = False Then
                        ''  rdSignature.Checked = False
                        chkSignature.Checked = False
                        chkSignatureDisable.Checked = False
                    Else
                        chkSignature.Checked = True
                        chkSignatureDisable.Checked = True

                    End If
                    If Order.SubTotal <= Utility.ConfigData.TotalEnableSignature Then
                        chkSignatureDisable.Visible = False
                        chkSignature.Visible = True
                        ''chkSignature.Attributes.Add("onchange", "CheckShippingSignature(this.checked);")
                    Else
                        chkSignature.Visible = False
                        chkSignatureDisable.Enabled = False
                        styleLabelDisable = "style='cursor:default;'"
                    End If
                Else
                    pnlSignature.Visible = False
                End If
            Else
                pnlInsurance.Visible = False
            End If
            If pnlInsurance.Visible = False And pnlSignature.Visible = False Then
                ulOption.InnerHtml = String.Empty
            End If
        End If

    End Sub
  
End Class
