Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_store_itembuywithpoints_default
    Inherits AdminPage
    Protected ItemId As Integer
    Private TotalRecords As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        ItemId = Convert.ToInt32(Request("ItemId"))
        If hidPopUpSKU.Value <> "" Then
            txtSku.Text = hidPopUpSKU.Value
        End If
        If ItemId > 0 AndAlso Not IsPostBack Then
            Dim si = StoreItemRow.GetRow(DB, ItemId)
            If Not si Is Nothing Then
                txtSku.Text = si.SKU
                txtpoint.Text = si.RewardPoints
                chkIsActive.Checked = si.IsActive
            End If
        End If
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "ArrangeRewardPoints"
            End If
            BindList()
        End If

    End Sub
    Private Sub BindList()
        Dim TotalRecord As Integer = 0
        Dim selectRow As Integer = 36
        Dim SQL, SQLFields As String

        SQLFields = "SELECT ItemId,ItemName,SKU,IsActive,RewardPoints,ArrangeRewardPoints,IsRewardPoints,QtyOnHand "
        SQL = " FROM StoreItem WHERE IsRewardPoints = 1"

        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("ArrangeRewardPoints") = True, "ArrangeRewardPoints Desc", gvList.SortByAndOrder))
        'Dim res As StoreItemCollection = StoreItemRow.GetListItemPoint(gvList.PageIndex + 1, gvList.PageSize, "", gvList.SortBy, gvList.SortOrder, TotalRecord)
        TotalRecords = res.Tables(0).DefaultView.Count
        gvList.DataSource = res
        gvList.DataBind()
        pagerTop.SetPaging(selectRow, TotalRecord)
        pagerBottom.SetPaging(selectRow, TotalRecord)
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            ' Dim rowView As StoreItemRow = e.Row.DataItem
            Dim rowView As DataRowView = e.Row.DataItem
            'Arrange
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            Dim ItemId As Integer = Convert.ToInt32(rowView("ItemId"))
            imbUp.CommandArgument = ItemId.ToString
            imbDown.CommandArgument = ItemId.ToString()
            If e.Row.DataItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Row.DataItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If
            'hidPopUpSKU.Value = hidPopUpSKU.Value & rowView("sku") & ";"
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            StoreItemRow.ChangeArrangeItemPoint(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "Down" Then
            StoreItemRow.ChangeArrangeItemPoint(DB, e.CommandArgument, True)
        End If
        BindList()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim siOld As StoreItemRow = Nothing
            Dim si As StoreItemRow = Nothing
            If ItemId <> 0 Then
                si = StoreItemRow.GetRow(DB, ItemId)
            End If
            If txtSku.Text <> "" AndAlso si Is Nothing Then
                si = StoreItemRow.GetRowSku(DB, txtSku.Text)
            Else
                Response.Redirect("default.aspx")
            End If
            siOld = CloneObject.Clone(si)
            If txtpoint.Text <> "" Then
                si.RewardPoints = Convert.ToInt32(txtpoint.Text.ToString())
            Else
                Response.Redirect("default.aspx")
            End If
            si.IsRewardPoints = True
            If ItemId > 0 Then
                si.Update()
            Else
                si.IPNUpdateItemPoint()
            End If

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = si.ItemId
            logDetail.ObjectType = Utility.Common.ObjectType.ItemPoint.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ItemPoint, si, siOld)
            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            btnCancel_Click(sender, e)
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    Protected Sub pagerTop_PageIndexChanging(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
        pagerTop.PageIndex = e.PageIndex
        pagerBottom.PageIndex = e.PageIndex
        'hidItemIds.Value = ""
        BindList()
    End Sub

    Protected Sub pagerTop_PageSizeChanging(ByVal obj As Object, ByVal e As PageSizeChangeEventArgs)
        pagerTop.PageSize = e.PageSize
        pagerTop.PageIndex = 1
        pagerBottom.PageSize = e.PageSize
        pagerBottom.PageIndex = 1

        If (DirectCast(obj, controls_layout_pager).ID = "pagerBottom") Then
            pagerTop.ViewAll = pagerBottom.ViewAll
        Else
            pagerBottom.ViewAll = pagerTop.ViewAll
        End If
        BindList()
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx")
    End Sub
End Class
