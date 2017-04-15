Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports Excel = Microsoft.Office.Interop.Excel

Partial Class admin_store_items_inventory
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        btnExcel.Attributes.Add("OnClick", "return exportXls();")
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_BrandId.DataSource = StoreBrandRow.GetAllStoreBrands(DB)
            F_BrandId.DataValueField = "BrandId"
            F_BrandId.DataTextField = "BrandName"
            F_BrandId.DataBind()
            F_BrandId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_GroupName.DataSource = StoreItemGroupRow.GetAllItemGroups(DB)
            F_GroupName.DataValueField = "ItemGroupId"
            F_GroupName.DataTextField = "GroupName"
            F_GroupName.DataBind()
            F_GroupName.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_ItemName.Text = Request("F_ItemName")
            F_ItemType.Text = Request("F_ItemType")
            F_SKU.Text = Request("F_SKU")
            F_IsActive.Text = Request("F_IsActive")
            F_IsFeatured.SelectedValue = Request("F_IsFeatured")
            F_IsNew.SelectedValue = Request("F_IsNew")
            F_DepartmentId.SelectedValue = Request("F_DepartmentId")
            F_BrandId.SelectedValue = Request("F_BrandId")
            F_HasSalesPrice.SelectedValue = Request("F_HasSalesPrice")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ItemId"

            BindDepartmentsDropDown(DB, F_DepartmentId, 2, "-- ALL --")

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " and "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        'SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " si.*, sb.brandname, g.groupname, (select count(*) from salesprice where itemid = si.itemid) as iCount,qtr.QtRemain,qtr.qtOrder,qtr.qtyonhand "
        'SQL = " FROM StoreItem si left outer join storeitemgroup g on si.itemgroupid = g.itemgroupid left outer join storebrand sb on si.brandid = sb.brandid, QtItem qtr "
        'SQL = SQL & " where si.itemid=qtr.itemid "
        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Vie_InventoryReportGroup si "
        SQL = SQL & " where itemid is not null  "
        If Not F_BrandId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "si.BrandId = " & DB.Quote(F_BrandId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ItemName.Text = String.Empty Then
            SQL = SQL & Conn & "ItemName LIKE " & DB.FilterQuote(F_ItemName.Text)
            Conn = " AND "
        End If
        If Not F_ItemType.Text = String.Empty Then
            If F_ItemType.Text = "0" Then SQL = SQL & Conn & "si.itemgroupid is null " Else SQL = SQL & Conn & "si.itemgroupid is not null "
            Conn = " AND "
        End If
        If Not F_SKU.Text = String.Empty Then
            SQL = SQL & Conn & "SKU LIKE " & DB.FilterQuote(F_SKU.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsFeatured.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsFeatured  = " & DB.Number(F_IsFeatured.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsNew.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsNew = " & DB.Number(F_IsNew.SelectedValue)
            Conn = " AND "
        End If
        '''''''''''''''''
        If F_IsOutOfStock.SelectedValue = "1" Then
            SQL = SQL & Conn & "QtyOnHand<1 "
            Conn = " AND "
        ElseIf F_IsOutOfStock.SelectedValue = "0" Then
            SQL = SQL & Conn & "QtyOnHand>0 "
            Conn = " AND "
        End If
        If F_IsDropShip.SelectedValue = "1" Then
            SQL = SQL & Conn & " (AcceptingOrder=1 or AcceptingOrder=2 or IsSpecialOrder=1)"
            Conn = " AND "
        ElseIf F_IsDropShip.SelectedValue = "0" Then
            SQL = SQL & Conn & " IsAcceptingOrder<>1 and IsSpecialOrder<>1 "
            Conn = " AND "
        End If
        ''''''''''''''''''''
        If Not DB.IsEmpty(F_DepartmentId.SelectedValue) Then
            SQL = SQL & Conn & " si.ItemId IN (SELECT ItemId FROM StoreDepartmentItem WHERE DepartmentId = " & DB.Quote(F_DepartmentId.SelectedValue) & ")"
            Conn = " AND "
        End If
        If Not F_GroupName.SelectedValue = String.Empty Then
            SQL &= Conn & "si.itemgroupid = " & DB.Number(F_GroupName.SelectedValue)
            Conn = " AND "
        End If
        If Not F_HasSalesPrice.SelectedValue = Nothing Then
            SQL &= Conn & IIf(F_HasSalesPrice.SelectedValue = "0", "not ", "") & "exists(select top 1 itemid from salesprice sp where sp.itemid = si.itemid) "
            Conn = " AND "
        End If
        'SQL = SQL & " group by itemid,groupname,itemname,qtyonhand,SKU,price,BrandName,IsActive,IsFeatured,brandid,itemgroupid,isnew,icount,image,isonsale "
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    'Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
    '    Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    'End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Public Shared Sub BindDepartmentsDropDown(ByVal DB As Database, ByVal DepartmentId As DropDownList, ByVal Level As Integer, ByVal FirstField As String)
        Dim SQL As String

        If Level = Nothing Then
            SQL = "SELECT * FROM StoreDepartment ORDER BY NAME ASC"
        Else
            SQL = " SELECT P1.DepartmentId, REPLICATE('- ', COUNT(P2.DepartmentId)-2) + p1.NAME AS NAME" _
             & " FROM StoreDepartment P1, StoreDepartment P2" _
             & " WHERE P1.lft BETWEEN P2.lft AND P2.rgt" _
             & " GROUP BY P1.DepartmentId, P1.lft, p1.rgt, p1.NAME" _
             & " HAVING COUNT(P2.DepartmentId) > 1 AND COUNT(P2.DepartmentId) <= " & DB.Quote((Level + 1).ToString) _
             & " ORDER BY P1.lft"
        End If

        Dim ds As DataSet = DB.GetDataSet(SQL)
        DepartmentId.DataSource = ds
        DepartmentId.DataTextField = "NAME"
        DepartmentId.DataValueField = "DepartmentId"
        DepartmentId.DataBind()
        If Not FirstField = "{BLANK}" Then DepartmentId.Items.Insert(0, New ListItem(FirstField, ""))
    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If

        Dim litType As Literal = e.Row.FindControl("litType")
        Dim img As Literal
        img = CType(e.Row.FindControl("img"), Literal)
        Dim imglink As Label
        imglink = CType(e.Row.FindControl("imglink"), Label)
        Dim noimg As Label
        noimg = CType(e.Row.FindControl("noimg"), Label)
        If Convert.ToString(e.Row.DataItem("IMAGE")) <> String.Empty Then
            img.Text = "<img src=""/assets/items/medium/" & e.Row.DataItem("IMAGE") & """>"
            imglink.Visible = True
            noimg.Visible = False
        Else
            imglink.Visible = False
            noimg.Visible = True
        End If

        'CType(e.Row.FindControl("CollectionLink"), HyperLink).Visible = False

        Dim ltlPrice As Literal = CType(e.Row.FindControl("ltlPrice"), Literal)
        If Convert.ToBoolean(e.Row.DataItem("IsOnSale")) Then
            ltlPrice.Text = "<span class=red>" & FormatCurrency(e.Row.DataItem("SalePrice")) & "</span>"
        Else
            ltlPrice.Text = FormatCurrency(e.Row.DataItem("Price"))
        End If

        Dim ltlTotalPrice As Literal = CType(e.Row.FindControl("ltlTotalPrice"), Literal)
        If Convert.ToBoolean(e.Row.DataItem("IsOnSale")) Then
            ltlTotalPrice.Text = "<span class=red>" & FormatCurrency(e.Row.DataItem("SalePrice")) & "</span>"
        Else
            ltlTotalPrice.Text = FormatCurrency(e.Row.DataItem("TotalPrice"))
        End If

        Dim Departments As Repeater = CType(e.Row.FindControl("Departments"), Repeater)
        Departments.DataSource = StoreItemRow.GetDepartments(DB, e.Row.DataItem("ItemId"))
        Departments.DataBind()

        If Not IsDBNull(e.Row.DataItem("groupname")) Then
            litType.Text = e.Row.DataItem("groupname")
        End If
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Dim SQLFields, SQL As String
        Dim Conn As String = " and "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        'SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " Groupname [Group Name],Itemname [Item Name],QtyOnHand [Quantity On Hand],qtremain [Quantity Remain],qtorder [Quantity Order], SKU,Price,Totalprice,BrandName,IsActive,IsFeatured   "
        SQLFields = "SELECT Groupname [Group Name],Itemname [Item Name],QtyOnHand [Quantity On Hand],qtremain [Quantity Remain],qtorder [Quantity Order], SKU,Price,Totalprice,BrandName,IsActive,IsFeatured   "
        SQL = " FROM Vie_InventoryReportGroup si "
        SQL = SQL & " where itemid is not null  "
        If Not F_BrandId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "si.BrandId = " & DB.Quote(F_BrandId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ItemName.Text = String.Empty Then
            SQL = SQL & Conn & "ItemName LIKE " & DB.FilterQuote(F_ItemName.Text)
            Conn = " AND "
        End If
        If Not F_ItemType.Text = String.Empty Then
            If F_ItemType.Text = "0" Then SQL = SQL & Conn & "si.itemgroupid is null " Else SQL = SQL & Conn & "si.itemgroupid is not null "
            Conn = " AND "
        End If
        If Not F_SKU.Text = String.Empty Then
            SQL = SQL & Conn & "SKU LIKE " & DB.FilterQuote(F_SKU.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsFeatured.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsFeatured  = " & DB.Number(F_IsFeatured.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsNew.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsNew = " & DB.Number(F_IsNew.SelectedValue)
            Conn = " AND "
        End If
        If Not DB.IsEmpty(F_DepartmentId.SelectedValue) Then
            SQL = SQL & Conn & " si.ItemId IN (SELECT ItemId FROM StoreDepartmentItem WHERE DepartmentId = " & DB.Quote(F_DepartmentId.SelectedValue) & ")"
            Conn = " AND "
        End If
        If Not F_GroupName.SelectedValue = String.Empty Then
            SQL &= Conn & "si.itemgroupid = " & DB.Number(F_GroupName.SelectedValue)
            Conn = " AND "
        End If
        If Not F_HasSalesPrice.SelectedValue = Nothing Then
            SQL &= Conn & IIf(F_HasSalesPrice.SelectedValue = "0", "not ", "") & "exists(select top 1 itemid from salesprice sp where sp.itemid = si.itemid) "
            Conn = " AND "
        End If
        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim dtt As DataTable = res.Tables(0)
        'gvList.DataSource = dtt
        'gvList.DataBind()
        Dim Filename As String
        Filename = "InventoryReportExcel_" & Now.Year & Now.Month & Now.Day & "_" & Now.Hour & Now.Minute & Now.Second
        Response.ContentType = "Application/x-msexcel"
        Response.AddHeader("content-disposition", "attachment;filename=" & Filename & ".csv")
        Response.Write(ToCSV(dtt))
        Response.End()
    End Sub

    Private Function ToCSV(ByVal dataTable As DataTable) As String

        'create the stringbuilder that would hold the data
        Dim sb As StringBuilder = New StringBuilder()
        'check if there are columns in the datatable
        If (dataTable.Columns.Count <> 0) Then

            'loop thru each of the columns for headers
            For Each column As DataColumn In dataTable.Columns
                'append the column name followed by the separator
                sb.Append(column.ColumnName & ",")
            Next column
            'append a carriage return
            'sb.Append("\r\n")
            sb.Append(Environment.NewLine)

            'loop thru each row of the datatable
            For Each row As DataRow In dataTable.Rows
                'loop thru each column in the datatable
                For Each column As DataColumn In dataTable.Columns
                    'get the value for the row on the specified column
                    ' and append the separator
                    sb.Append(row(column).ToString() & ",")
                Next column
                'append a carriage return
                'sb.Append("\r\n")
                sb.Append(Environment.NewLine)
            Next row
        End If
        Return sb.ToString()
    End Function

End Class
