Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_groups_related
    Inherits AdminPage

    Protected params As String
    Protected ItemGroupId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        params = GetPageParams(FilterFieldType.All)
        ItemGroupId = Request("ItemGroupId")
        If Not IsPostBack Then
            BindDataGrid()
        End If
    End Sub

    Private Sub BindDataGrid()

        If Not IsPostBack Then
            ViewState("F_PG") = Request("F_PG")
        End If
        If CType(ViewState("F_PG"), String) = String.Empty Then
            ViewState("F_PG") = 1
        End If

        ' BUILD QUERY
        SQL = ""
        SQL &= " SELECT si.ItemId, si.SKU, si.ItemGroupId, si.ItemName, si.IsActive FROM StoreItem si "
        SQL &= " where si.ItemgroupId = " & ItemGroupId

        lblItemName.Text = DB.ExecuteScalar("SELECT groupName FROM Storeitemgroup WHERE Itemgroupid=" & ItemGroupId)
        Trace.Write(SQL)
        Dim res As DataSet = DB.GetDataSet(SQL)

        myNavigator.NofRecords = res.Tables(0).Rows.Count
        myNavigator.MaxPerPage = dgList.PageSize
        myNavigator.PageNumber = Math.Max(Math.Min(CType(ViewState("F_PG"), Integer), myNavigator.NofPages), 1)
        myNavigator.DataBind()

        ViewState("F_PG") = myNavigator.PageNumber
        tblList.Visible = (myNavigator.NofRecords <> 0)
        plcNoRecords.Visible = (myNavigator.NofRecords = 0)

        dgList.DataSource = res.Tables(0).DefaultView
        dgList.CurrentPageIndex = ViewState("F_PG") - 1
        dgList.DataBind()
    End Sub

    Private Sub dglist_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dgList.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim rpt As Repeater = e.Item.FindControl("rpt")
            Dim itemId As Integer = e.Item.DataItem("ItemId")
            Dim sql As String = "select distinct gor.SortOrder,o.optionid, o.optionname, c.choiceid, c.choicename from storeitemgroupoption o "
            sql &= " inner join storeitemgroupchoice c on o.optionid = c.optionid "
            sql &= " inner join storeitemgroupchoicerel r on c.choiceid = r.choiceid"
            sql &= " left join StoreItemGroupOptionRel gor on gor.OptionId=o.OptionId"
            sql &= " where r.itemid = " & itemId & " and gor.ItemGroupId=(Select ItemGroupId from StoreItem where ItemId=" & itemId & ")"
            sql &= " order by gor.SortOrder,o.optionid, o.optionname, c.choiceid, c.choicename ASC"
            rpt.DataSource = DB.GetDataTable(sql)
            rpt.DataBind()
        End If
    End Sub

    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As Controls.NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindDataGrid()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim itemId As Integer = 0
        Try
            itemId = CInt(Request.Form("ItemId"))
            DB.BeginTransaction()
            DB.ExecuteSQL("Update StoreItem set ItemGroupId=" & ItemGroupId & " where ItemId=" & itemId)
            DB.ExecuteSQL("Update StoreItemReview set ItemGroupId=" & ItemGroupId & " where ItemId=" & itemId)
            DB.ExecuteSQL("Delete from StoreItemGroupChoiceRel where ItemId =" & itemId & " and OptionId in(Select OptionId from StoreItemGroupOptionRel where ItemGroupId=" & ItemGroupId & ")")
            '' dbItem.RemoveOptionChoices()
            Dim Options As String = ""
            For Each s As String In Request.Form.Keys
                If Left(s, 8) = "OPTIONS_" Then
                    Options &= IIf(Options = String.Empty, "", ",") & Request(s)
                End If
            Next
            If Options = String.Empty Then Throw New Exception("Invalid Parameters")
            StoreItemRow.InsertOptionChoices(DB, itemId, Options)
            DB.CommitTransaction()
            StoreItemRow.ClearItemCache(itemId)
            Response.Redirect("items.aspx?ItemGroupId=" & ItemGroupId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            DB.RollbackTransaction()
            AddError(ex.Message)
        Finally
            BindDataGrid()
        End Try
    End Sub
End Class