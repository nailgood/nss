Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_shopdesign_item
    Inherits AdminPage

    Dim ShopDesignId As Integer
    Private ItemId As Integer
    Dim Total As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ShopDesignId = CInt(Request("ShopDesignId"))

        ItemId = CInt(Request("ItemId"))
        If hidPopUpSKU.Value <> String.Empty And ItemId = Nothing Then
            txtSku.Text = hidPopUpSKU.Value
            'ItemId = StoreItemRow.GetRow(DB, txtSku.Text.ToString()).ItemId
        End If
        If Not IsPostBack Then
            LoadDetail()
            LoadList()
        End If
    End Sub

    Private Sub LoadDetail()
        If ItemId < 1 Then
            Exit Sub
        End If
        Dim detail As ShopDesignItemRow = ShopDesignItemRow.GetRow(DB, ItemId, ShopDesignId)
        txtQty.Text = detail.QtyDefault
        txtSku.Text = detail.SKU
        chkIsActive.Checked = detail.IsActive
        btnSelectItem.Visible = False
    End Sub

    Private Sub LoadList()
        Dim result As ShopDesignItemRowCollection = ShopDesignItemRow.AdminListItemByShopDesignId(ShopDesignId)
        Total = result.Count
        gvList.Pager.NofRecords = Total
        gvList.DataSource = result
        gvList.DataBind()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then
            Exit Sub
        End If
        Dim item As New ShopDesignItemRow
        Dim itemBefore As New ShopDesignItemRow
        Dim logdetail As New AdminLogDetailRow
        Try
            If ItemId > 0 Then
                item = ShopDesignItemRow.GetRow(DB, ItemId, ShopDesignId)
                itemBefore = CloneObject.Clone(item)
            End If
            'item.SKU = txtSku.Text
            item.IsActive = chkIsActive.Checked
            item.QtyDefault = CInt(txtQty.Text)
            item.ShopDesignId = ShopDesignId
            If ItemId > 0 Then
                item.Update()
                logdetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logdetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ShopDesignItem, itemBefore, item)
                logdetail.ObjectId = item.ItemId
                logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignItem.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logdetail)
            Else
                'If item.ItemId > 0 Then
                '    lbError.Text = "This item is in list. Please select other item!"
                '    Exit Sub
                'Else
                Dim arrsku As String() = txtSku.Text.Split(";")
                If (arrsku.Length > 0) Then
                    For Each sku As String In arrsku
                        If Not String.IsNullOrEmpty(sku) Then
                            item.SKU = sku
                            item.ItemId = item.Insert()
                            logdetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                            logdetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(item, Utility.Common.ObjectType.ShopDesignItem)
                            logdetail.ObjectId = item.ItemId
                            logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignItem.ToString()
                            AdminLogHelper.WriteLuceneLogDetail(logdetail)
                        End If
                    Next
                End If
                'End If

            End If
            Response.Redirect("item.aspx?ShopDesignId=" & ShopDesignId)
        Catch ex As Exception
            If LCase(ex.ToString()).Contains("cannot insert duplicate") Then
                lbError.Text = "This item is in list. Please select other item!"
            Else
                AddError(ErrHandler.ErrorText(ex))
            End If

        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?" & GetPageParams(Components.FilterFieldType.All))
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("item.aspx?ShopDesignId=" & ShopDesignId)
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As ShopDesignItemRow = e.Row.DataItem
            Dim imbUp As ImageButton = e.Row.FindControl("imbUp")
            Dim imbDown As ImageButton = e.Row.FindControl("imbDown")
            hidPopUpSKU.Value &= row.SKU & ";"
            If Total > 1 AndAlso e.Row.RowIndex = 0 Then
                imbUp.Visible = False
            ElseIf Total > 1 AndAlso e.Row.RowIndex = Total - 1 Then
                imbDown.Visible = False
            ElseIf Total < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            End If
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            ShopDesignItemRow.Delete(ShopDesignId, CInt(e.CommandArgument))
        ElseIf e.CommandName = "Up" Then
            ShopDesignItemRow.ChangeSortOrder(ShopDesignId, CInt(e.CommandArgument), False)
        ElseIf e.CommandName = "Down" Then
            ShopDesignItemRow.ChangeSortOrder(ShopDesignId, CInt(e.CommandArgument), True)
        End If
        Response.Redirect("item.aspx?ShopDesignId=" & ShopDesignId)
    End Sub
End Class
