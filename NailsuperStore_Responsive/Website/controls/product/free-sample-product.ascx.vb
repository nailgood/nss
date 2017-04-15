

Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_product_free_sample_product
    Inherits BaseControl

    Private m_Item As StoreItemRow = Nothing
    Public Property Item() As StoreItemRow
        Set(ByVal value As StoreItemRow)
            m_Item = value
        End Set
        Get
            Return m_Item
        End Get
    End Property
    Private m_IsCustomerInternational As Boolean = False
    Public Property IsCustomerInternational() As Boolean
        Set(ByVal value As Boolean)
            m_IsCustomerInternational = value
        End Set
        Get
            Return m_IsCustomerInternational
        End Get
    End Property
    Private m_AllowAddCart As Boolean = False
    Public Property AllowAddCart() As Boolean
        Set(ByVal value As Boolean)
            m_AllowAddCart = value
        End Set
        Get
            Return m_AllowAddCart
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Item Is Nothing Then
            Dim strItemName As String = Item.ItemName ''System.Web.HttpUtility.UrlEncode(objItem.ItemName)
            ''strItemName = RewriteUrl.ReplaceUrl(strItemName)
            Dim strAlt As String = System.Web.HttpUtility.UrlEncode(strItemName)
            strAlt = RewriteUrl.ReplaceUrl(strAlt)
            ltrName.Text = strItemName
            Dim img As String = Item.Image
            If String.IsNullOrEmpty(img) Then
                img = Utility.ConfigData.NoImageItem
            End If
            ''img = "119006.jpg"
            '' ltrImage.Text = "<img class='lazy' src='/assets/nobg.gif' data-original='/assets/items/medium/" & img & "' height='100%' width='100%' style='border-style:none;' alt='" & strItemName & "'/>"
            ltrImage.Text = "<img src='" & Utility.ConfigData.CDNmediapath & "/assets/items/medium/" & img & "' alt='" & strAlt & "' />"

            If Not Item.ShortDesc Is Nothing Then
                ltrDes.Text = Item.ShortDesc
            End If

            chkSelect.ID = chkSelect.ID & "_" & Item.ItemId
            chkSelect.ClientIDMode = UI.ClientIDMode.Static
          


            If Utility.Common.CheckItemOPI(Item) Then
                ltrError.Text = Resources.Msg.OPI
            Else
                If IsCustomerInternational = True And Item.IsFlammable = True Then
                    ltrError.Text = "Item is not available for customer outsite of 48 states within continental USA."
                End If
            End If
          

            If Me.Request.FilePath.Contains("/store/free-gift.aspx") Then
                divFreeGiftSelect.Visible = True
                divSampleSelect.Visible = False
                Dim si As StoreItemRow = StoreItemRow.GetRow(DB, Item.ItemId)
                Dim objItemPrice As ItemPrice = BaseShoppingCart.GetItemPrice(DB, si, 1, 0, 0, False)
                divPrice.Visible = True
                Dim lastPrice As Double = 0
                If (Not objItemPrice.MultiPriceColection Is Nothing AndAlso objItemPrice.MultiPriceColection.Count) Then
                    divPrice.InnerHtml = objItemPrice.MultiPriceHTML
                    lastPrice = objItemPrice.MultiPriceColection(0).Price
                Else
                    If (objItemPrice.SalePrice > 0) Then
                        lastPrice = objItemPrice.SalePrice
                    Else
                        lastPrice = objItemPrice.NormalPrice
                    End If
                End If
                divPrice.InnerHtml = "<span class='strike bold'>" & Utility.Common.ViewCurrency(lastPrice) & "</span>&nbsp;&nbsp;<span class='red bold'>FREE</span>"
                If AllowAddCart Then
                    If Item.IsInCart Then
                        btnAddcart.CssClass = "in-cart"
                        btnAddcart.Text = "Selected"
                    Else
                        btnAddcart.CssClass = "add-cart"
                        btnAddcart.Text = "Select"
                    End If

                    If Not String.IsNullOrEmpty(ltrError.Text) Then
                        divFreeGiftSelect.Visible = False
                    Else
                        btnAddcart.Attributes.Add("onclick", "return AddFreeGift(" & Item.ItemId & ")")
                        btnAddcart.ID &= "_" & Item.ItemId
                    End If
                Else
                    divFreeGiftSelect.Visible = False
                End If


            Else
                divFreeGiftSelect.Visible = False
                divPrice.Visible = False
                divSampleSelect.Visible = True
                If Item.IsInCart Then
                    chkSelect.Checked = True
                End If
                If Not String.IsNullOrEmpty(ltrError.Text) Then

                    divSampleSelect.Visible = False

                Else
                    ltrSpanSelect.Text = "<i onclick='CheckItem(" & Item.ItemId & ")' class='fa fa-check checkbox-font'></i>"
                    If Not spLabel Is Nothing Then
                        spLabel.Attributes.Add("onclick", "CheckItem(" & Item.ItemId & ")")
                    End If
                End If
            End If
        End If
    End Sub
End Class
