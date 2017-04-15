Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_product_free_gift
    Inherits BaseControl
    Protected m_isAllowAddCart As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not Session("FreeGiftLevelRender") Is Nothing Then
                Try
                    Dim str As String() = Session("FreeGiftLevelRender").ToString().Split("|")
                    Dim orderId As Integer = str(0)
                    Dim levelId As Integer = str(1)

                    Dim lstItem As StoreItemCollection = StoreItemRow.GetFreeGiftColectionByLevel(orderId, levelId)
                    m_isAllowAddCart = FreeGiftLevelRow.CheckAllowAddCart(orderId, levelId)

                    rptFreeGift.DataSource = lstItem
                    rptFreeGift.DataBind()
                Catch ex As Exception

                End Try

            End If
        End If

    End Sub

    Protected Sub rptFreeGift_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFreeGift.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim si As StoreItemRow = CType(e.Item.DataItem, StoreItemRow)

            Dim lit As Literal = CType(e.Item.FindControl("litImage"), Literal)
            If lit IsNot Nothing Then
                lit.Text = String.Format("<img src=""{0}/assets/items/featured/{1}"" />", Utility.ConfigData.CDNMediaPath, IIf(si.Image = Nothing, "na.jpg", si.Image))
            End If

            lit = CType(e.Item.FindControl("litName"), Literal)
            If lit IsNot Nothing Then
                lit.Text = String.Format("{0}", si.ItemName)
            End If

            If m_isAllowAddCart Then
                lit = CType(e.Item.FindControl("litAddCart"), Literal)
                If lit IsNot Nothing Then
                    lit.Text = String.Format("<input type=""button"" id=""btnAddcart{0}"" ", si.ItemId)

                    If si.IsInCart Then
                        lit.Text &= String.Format("value=""Added"" class=""selected"" />")
                    Else
                        lit.Text &= String.Format("value=""Add"" onclick=""return AddFreeGift({0});"" />", si.ItemId)
                    End If
                End If
            End If



        End If
    End Sub

End Class
