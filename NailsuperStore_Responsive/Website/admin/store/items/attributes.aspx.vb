Imports Components
Imports DataLayer

Partial Class admin_store_items_attributes
	Inherits AdminPage

	Const Attributes As Integer = 5
	Private ItemId As Integer
	Protected Item As StoreItemRow

	Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
		ItemId = Request("ItemId")
		Item = StoreItemRow.GetRow(DB, ItemId)

		If Not IsPostBack Then
			BindData()
		End If
	End Sub

	Private Sub BindData()
		Dim c As StoreAttributeCollection = StoreAttributeRow.GetRowsByItem(DB, ItemId)
		If c.Count < Attributes Then
			For i As Integer = c.Count To Attributes - 1
				c.Add(New StoreAttributeRow(DB))
			Next
		End If

		gv.DataSource = c
		gv.DataBind()

		txtPrefix.Text = Item.Prefix
	End Sub

	Private Sub gv_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gv.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			Dim txtValue As TextBox = e.Row.FindControl("txtValue")
			Dim txtSKU As TextBox = e.Row.FindControl("txtSKU")
			Dim txtPrice As TextBox = e.Row.FindControl("txtPrice")

			txtValue.Text = IIf(Left(e.Row.DataItem.Value, 1) = vbCr, vbCrLf, "") & e.Row.DataItem.Value
			txtSKU.Text = IIf(Left(e.Row.DataItem.SKU, 1) = vbCr, vbCrLf, "") & e.Row.DataItem.SKU
			txtPrice.Text = IIf(Left(e.Row.DataItem.Price, 1) = vbCr, vbCrLf, "") & e.Row.DataItem.Price
		End If
	End Sub

	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
		Try
			DB.BeginTransaction()

			StoreAttributeRow.RemoveRowsByItem(DB, ItemId)

			For Each row As GridViewRow In gv.Rows
				Dim att As StoreAttributeRow = New StoreAttributeRow(DB)
				att.ItemId = ItemId
				att.Name = CType(row.FindControl("txtName"), TextBox).Text
				att.Value = CType(row.FindControl("txtValue"), TextBox).Text
				att.SKU = CType(row.FindControl("txtSKU"), TextBox).Text
				att.Price = CType(row.FindControl("txtPrice"), TextBox).Text
				If Trim(att.Name) <> Nothing AndAlso Trim(att.Value) <> Nothing Then
					att.Insert()
				End If
			Next

			Item.Prefix = txtPrefix.Text
			Item.Update()

			DB.CommitTransaction()
		Catch ex As Exception
			DB.RollbackTransaction()
			AddError(ErrHandler.ErrorText(ex))
			Exit Sub
		End Try
		Response.Redirect("/admin/store/items/edit.aspx?ItemId=" & ItemId)
	End Sub

	Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
		Response.Redirect("/admin/store/items/edit.aspx?ItemId=" & ItemId)
	End Sub

End Class
