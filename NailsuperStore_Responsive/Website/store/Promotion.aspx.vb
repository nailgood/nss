Imports Components

Partial Class store_Promotion
	Inherits SitePage

	Protected Promotion As PromotionRow
	Protected MixMatch As DataLayer.MixMatchRow
    Protected ReWriteURL As RewriteUrl
	Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
		Try
            'Trung fix duplicate Promote items
            Promotion = PromotionRow.GetDistinctRow(DB, CInt(Request("MixMatchId")))
            'Promotion = PromotionRow.GetRow(DB, CInt(Request("MixMatchId")), False)
			MixMatch = DataLayer.MixMatchRow.GetRow(DB, Promotion.MixMatchId)
			Dim dv As DataView = New DataView
			dv.Table = Promotion.PurchaseItems.Table.Copy
			dv.RowFilter = "(percentoff = 0 or percentoff is null) and (setprice = 0 or setprice is null)"
			If dv.Count > 0 Then
				dv.Sort = "itemname"
				dl.DataSource = dv
				dl.DataBind()
			Else
				div1.Visible = False
			End If

			If Promotion.Type = PromotionType.LeastExpensive Then
				div2.Visible = False
			Else
				dv.Table = Promotion.PurchaseItems.Table.Copy
				dv.RowFilter = "percentoff > 0 or setprice > 0"
				Dim dv2 As New DataView
				dv2.Table = Promotion.GetItems.Table.Copy
				If dv.Count > 0 Then
					For i As Integer = 0 To dv.Count - 1
						Dim tmp As New DataView
						tmp.Table = dv2.Table.Copy
						tmp.RowFilter = "itemid = " & dv(i)("ItemId")
						If tmp.Count = 0 Then
							Dim r As DataRow = dv2.Table.NewRow
							r("ItemId") = dv(i)("ItemId")
							r("PercentOff") = dv(i)("PercentOff")
							r("SetPrice") = dv(i)("SetPrice")
                            r("ItemName") = System.Web.HttpUtility.UrlEncode(dv(i)("ItemName").ToString.Replace("/", "_"))
				r("ItemName") =ReWriteURL.ReplaceUrl(r("ItemName"))
                            r("ItemNameNew") = dv(i)("ItemNameNew")
                            If dv(i)("ItemNameNew").ToString <> "" And IsDBNull(dv(i)("ItemNameNew")) = False Then
				r("ItemName") = System.Web.HttpUtility.UrlEncode(dv(i)("ItemNameNew").ToString.Replace("/", "_"))
r("ItemName") =ReWriteURL.ReplaceUrl(r("ItemName"))
                            End If
                            r("Image") = dv(i)("Image")
                            r("SKU") = dv(i)("SKU")
                            r("Price") = dv(i)("Price")
                            r("HighPrice") = dv(i)("HighPrice")
                            r("LowPrice") = dv(i)("LowPrice")
                            r("DepartmentId") = dv(i)("DepartmentId")
                            dv2.Table.Rows.Add(r)
                        End If
                    Next
				End If
				If dv2.Count > 0 Then
					dv2.Sort = "itemname"
					dl2.DataSource = dv2
					dl2.DataBind()
				Else
					div2.Visible = False
				End If
			End If
		Catch ex As Exception
		End Try
	End Sub

End Class
