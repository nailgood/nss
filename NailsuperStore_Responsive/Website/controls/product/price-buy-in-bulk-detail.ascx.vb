Imports DataLayer
Imports Components

Partial Class controls_product_price_buy_in_bulk_detail
    Inherits BaseControl
    Public hidUnitPoint As String = String.Empty
    Public hidPricePoint As String = String.Empty
    Private m_Item As StoreItemRow = Nothing

    Public Property Item() As StoreItemRow
        Set(ByVal value As StoreItemRow)
            m_Item = value
        End Set
        Get
            Return m_Item
        End Get
    End Property
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Item Is Nothing Then
            Me.Visible = False
            Exit Sub
        End If

        btnAddCartCase.Attributes.Add("onclick", "return AddCartDetail(" & Item.ItemId & ", 'case');")
        LoadPriceItem(Item)
    End Sub

    Private Sub LoadPriceItem(ByVal objItemShowPrice As StoreItemRow)
        Dim casePrice As Double = 0
        Dim dtPrice As New DataTable()
        ltrPrice.Text = BaseShoppingCart.GetBuyInBulkItemPrice(DB, objItemShowPrice, 1, dtPrice)
        If String.IsNullOrEmpty(ltrPrice.Text) Then
            Me.Visible = False
            Exit Sub
        End If

        hidPricePoint = String.Empty
        hidUnitPoint = SysParam.GetValue("GetPoint").ToString()

        If dtPrice.Rows.Count > 0 Then
            casePrice = CDbl(dtPrice.Rows(0)("Price"))
        End If

        casePrice = Utility.Common.RoundCurrency(casePrice)
        Dim pointPurchaseItem As Double = CDbl(hidUnitPoint) * casePrice
        If pointPurchaseItem < 1 Then
            pointPurchaseItem = 1
        Else
            pointPurchaseItem = Math.Ceiling(pointPurchaseItem)
        End If

        divPoint.InnerHtml = String.Format(Resources.Msg.ItemPointCase, pointPurchaseItem)

        If dtPrice.Rows.Count = 1 Then
            hidPricePoint = dtPrice.Rows(0)("Price").ToString()
        Else
            For index = 0 To dtPrice.Rows.Count - 1
                Dim MinQty As String = String.Empty
                If dtPrice.Rows.Count - 1 = index Then
                    MinQty = "9999"
                Else
                    MinQty = dtPrice.Rows(index + 1)("MinQty").ToString()
                End If

                hidPricePoint &= dtPrice.Rows(index)("MinQty").ToString() + "-" & _
                    MinQty + "-" & _
                    dtPrice.Rows(index)("Price").ToString() + "-" & _
                    dtPrice.Rows(index)("SavePc").ToString() + "-" & _
                    dtPrice.Rows(index)("UnitPrice").ToString() + ","
            Next
        End If
       
        txtQtyCase.Attributes.Add("onblur", "CalculatePointCase('" & hidUnitPoint & "','" & hidPricePoint & "')")
        divCaseDetail.InnerHtml = String.Format("$<span id='buyInBulkPrice'>{0}</span>/case | $<span id='buyInBulkUnitPrice'>{1}</span>/{3} </br><span  class='textSaveCase'>(save <span id='buyInBulkSavePc'>{2}</span>)</span>", dtPrice.Rows(0)("Price").ToString(), dtPrice.Rows(0)("UnitPrice").ToString(), dtPrice.Rows(0)("SavePc").ToString(), IIf(objItemShowPrice.PriceDesc.Contains("pcs"), "box", "pc"))

        divInCart.Visible = False
        Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
        If orderId > 0 Then
            divInCart.Visible = CDbl(DB.ExecuteScalar("SELECT dbo.fc_StoreCartItem_CheckItemCaseInCart(" & orderId.ToString() & ", " & m_Item.ItemId & ") "))
        End If

        divHandlingFee.Visible = False
        Dim iTotalWeight As Integer = Math.Ceiling(objItemShowPrice.Weight * objItemShowPrice.CaseQty)
        If iTotalWeight >= 8 Then 'Tam thoi hardcode 8 lbs de giam truy xuat db

            Dim handlingFee As Double = StoreItemRow.GetHandlingFee(DB, objItemShowPrice.ItemId, True, True)
            If handlingFee > 0 Then
                ltrHandlingFee.Text = FormatCurrency(handlingFee) & "/case"
                divHandlingFee.Visible = True
            End If
        End If
    End Sub
End Class
