Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_product_free_samples
    Inherits BaseControl
  
    Protected m_isAllowAddCart As Boolean = True
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Try
                Dim filter As DepartmentFilterFields = New DepartmentFilterFields()
                filter.pg = 1
                filter.SortBy = String.Empty
                filter.SortOrder = String.Empty
                filter.MaxPerPage = Int32.MaxValue
                filter.MemberId = Utility.Common.GetCurrentMemberId()
                filter.OrderId = Utility.Common.GetCurrentOrderId()

                Dim so As StoreOrderRow = StoreOrderRow.GetRow(DB, filter.OrderId)
                If so.SubTotal < CDbl(SysParam.GetValue("FreeSampleOrderMin")) Then
                    m_isAllowAddCart = False
                End If

                Dim iTotalRecords As Integer = Integer.MinValue
                Dim lstItem As StoreItemCollection = StoreItemRow.GetFreeSampleColection(DB, filter, iTotalRecords)

                rptFreeSamples.DataSource = lstItem
                rptFreeSamples.DataBind()

                
            Catch ex As Exception

            End Try
        End If

    End Sub

    Protected Sub rptFreeSamples_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFreeSamples.ItemDataBound
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
                        lit.Text = String.Format("<input type=""button"" id=""btnAdded{0}"" value=""Added"" class=""selected"" />", si.ItemId) & lit.Text & String.Format("value=""Delete"" class=""delete"" onclick=""DeleteFreeSamples({0});"" />", si.ItemId)
                    Else
                        lit.Text &= String.Format("value=""Add"" onclick=""return AddFreeSamples({0});"" />", si.ItemId)
                    End If
                End If
            End If



        End If
    End Sub

End Class
