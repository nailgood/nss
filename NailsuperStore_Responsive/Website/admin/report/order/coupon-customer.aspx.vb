Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports Excel = Microsoft.Office.Interop.Excel

Partial Class admin_report_order_coupon_customer
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        btnExcel.Attributes.Add("OnClick", "return exportXls();")
        gvList.BindList = AddressOf BindList
        F_PromotionCode.Text = Core.ProtectParam(Request("PromotionCode"))
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "PromotionCode"
            'BindDepartmentsDropDown(DB, F_DepartmentId, 2, "-- ALL --")
            BindList()
        End If
        btnExcel.Visible = False
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " and "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Vie_PromotionCustomerReport si "
        SQL = SQL & " where PromotionCode is not null  "

        If Not F_PromotionCode.Text = String.Empty Then
            SQL = SQL & Conn & " PromotionCode =" & DB.Quote(F_PromotionCode.Text)
            Conn = " AND "
        End If
        If Not F_CustomerName.Text = String.Empty Then
            SQL = SQL & Conn & "Name like " & DB.FilterQuote(F_CustomerName.Text)
            Conn = " AND "
        End If
        If Not F_CustomerNo.Text = String.Empty Then
            SQL = SQL & Conn & "CustomerNo LIKE " & DB.FilterQuote(F_CustomerNo.Text)
            Conn = " AND "
        End If
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
        'If Not F_BrandId.SelectedValue = String.Empty Then
        '    SQL = SQL & Conn & "si.BrandId = " & DB.Quote(F_BrandId.SelectedValue)
        '    Conn = " AND "
        'End If
        'If Not F_ItemName.Text = String.Empty Then
        '    SQL = SQL & Conn & "ItemName LIKE " & DB.FilterQuote(F_ItemName.Text)
        '    Conn = " AND "
        'End If
        'If Not F_ItemType.Text = String.Empty Then
        '    If F_ItemType.Text = "0" Then SQL = SQL & Conn & "si.itemgroupid is null " Else SQL = SQL & Conn & "si.itemgroupid is not null "
        '    Conn = " AND "
        'End If
        'If Not F_SKU.Text = String.Empty Then
        '    SQL = SQL & Conn & "SKU LIKE " & DB.FilterQuote(F_SKU.Text)
        '    Conn = " AND "
        'End If
        'If Not F_IsActive.SelectedValue = String.Empty Then
        '    SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
        '    Conn = " AND "
        'End If
        'If Not F_IsFeatured.SelectedValue = String.Empty Then
        '    SQL = SQL & Conn & "IsFeatured  = " & DB.Number(F_IsFeatured.SelectedValue)
        '    Conn = " AND "
        'End If
        'If Not F_IsNew.SelectedValue = String.Empty Then
        '    SQL = SQL & Conn & "IsNew = " & DB.Number(F_IsNew.SelectedValue)
        '    Conn = " AND "
        'End If
        'If Not DB.IsEmpty(F_DepartmentId.SelectedValue) Then
        '    SQL = SQL & Conn & " si.ItemId IN (SELECT ItemId FROM StoreDepartmentItem WHERE DepartmentId = " & DB.Quote(F_DepartmentId.SelectedValue) & ")"
        '    Conn = " AND "
        'End If
        'If Not F_GroupName.SelectedValue = String.Empty Then
        '    SQL &= Conn & "si.itemgroupid = " & DB.Number(F_GroupName.SelectedValue)
        '    Conn = " AND "
        'End If
        'If Not F_HasSalesPrice.SelectedValue = Nothing Then
        '    SQL &= Conn & IIf(F_HasSalesPrice.SelectedValue = "0", "not ", "") & "exists(select top 1 itemid from salesprice sp where sp.itemid = si.itemid) "
        '    Conn = " AND "
        'End If
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
