Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_SpecificItems
    Inherits AdminPage

    Protected ItemGroupId As Integer
    Protected row As StoreItemRow

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ItemGroupId = CInt(Request("ItemGroupId"))
        row = StoreItemRow.GetRow(DB, ItemGroupId)
        ltlTitle.Text = "Group Items for '" & row.ItemName & "'"

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            gvList.SortBy = "ItemName"
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim sSQL As String

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        sSQL = ""
        sSQL &= " SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize
        sSQL &= " g.ID, si.sku, g.ItemGroupId, si.ItemId As RecordId, si.ItemName, 'Item' As ItemType FROM StoreItem si inner join StoreItemGroupRel g on si.itemid = g.itemid "
        sSQL &= " where g.ItemGroupid = " & DB.Quote(ItemGroupId)
        sSQL &= " ORDER BY " & gvList.SortByAndOrder

        Dim res As DataSet = DB.GetDataSet(sSQL)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.Pager.NofRecords = res.Tables(0).DefaultView.Count
        gvList.DataBind()

        rptOptions.DataSource = row.GetStoreItemGroupOptions()
        rptOptions.DataBind()
    End Sub

    Private Sub gvlist_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rpt As Repeater = CType(e.Row.FindControl("rpt"), Repeater)
            Dim SQL As String = "select o.optionname, c.choicename from storeitemgroupoption o inner join storeitemgroupchoice c on o.optionid = c.optionid inner join storeitemgroupchoicerel r on c.choiceid = r.choiceid where r.itemid = " & e.Row.DataItem("RecordId")
            rpt.DataSource = DB.GetDataSet(SQL)
            rpt.DataBind()
        End If
    End Sub

    Private Sub rptOptions_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptOptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ddl As DropDownList = CType(e.Item.FindControl("ddlChoice"), DropDownList)
            ddl.DataSource = StoreItemGroupChoiceRow.GetRowsByOption(DB, e.Item.DataItem("OptionId"))
            ddl.DataValueField = "ChoiceId"
            ddl.DataTextField = "ChoiceName"
            ddl.DataBind()
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            DB.BeginTransaction()
            row.InsertToStoreItemGroupRel(Request.Form("ItemId"))
            StoreItemRow.DeleteAllStoreItemGroupChoiceRel(DB, Request.Form("ItemId"))
            For Each i As RepeaterItem In rptOptions.Items
                StoreItemRow.InsertToStoreItemGroupChoiceRel(DB, Request.Form("ItemId"), CType(i.FindControl("ddlChoice"), DropDownList).SelectedValue, CType(i.FindControl("lblOptionId"), Label).Text)
            Next
            DB.CommitTransaction()
            Response.Redirect("groupitems.aspx?ItemGroupId=" & ItemGroupId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
            BindList()
        End Try
    End Sub
End Class