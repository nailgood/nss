Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_promotions_ShopSave_SearchItem
    Inherits AdminPage
    Dim skuSelect As String = String.Empty
    Public Property Type() As String

        Get
            Dim o As Object = ViewState("Type")
            If o IsNot Nothing Then
                Return DirectCast(o, String)
            End If
            Return String.Empty
        End Get

        Set(ByVal value As String)
            ViewState("Type") = value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        skuSelect = ";" + Request("item")

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            ''hidSKUSelect.Value = skuSelect
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
            Type = Request("Type")
            F_ItemName.Text = Request("F_ItemName")
            F_ItemType.Text = Request("F_ItemType")
            F_SKU.Text = Request("F_SKU")
            F_IsActive.Text = Request("F_IsActive")
            F_IsFeatured.SelectedValue = Request("F_IsFeatured")
            F_IsNew.SelectedValue = Request("F_IsNew")
            F_DepartmentId.SelectedValue = Request("F_DepartmentId")
            F_BrandId.SelectedValue = Request("F_BrandId")
            F_HasSalesPrice.SelectedValue = Request("F_HasSalesPrice")
            F_IsFreeSample.SelectedValue = Request("F_IsFreeSample")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ItemId"

            BindDepartmentsDropDown(DB, F_DepartmentId, 2, "-- ALL --")

            BindList()
        End If
    End Sub

    Private Sub BindList()

        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " si.ItemId, si.SKU, si.ItemName, si.IsActive, si.IsFeatured, si.IsOnSale, si.Price, sb.BrandName, g.GroupName, g.ItemGroupId"
        SQL = " FROM StoreItem si left outer join storeitemgroup g on si.itemgroupid = g.itemgroupid left outer join storebrand sb on si.brandid = sb.brandid "

        If Not F_BrandId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sb.BrandId = " & DB.Quote(F_BrandId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ItemName.Text = String.Empty Then
            SQL = SQL & Conn & "ItemName LIKE " & DB.FilterQuote(F_ItemName.Text)
            Conn = " AND "
        End If
        If Not F_ItemType.Text = String.Empty Then
            If F_ItemType.Text = "0" Then SQL = SQL & Conn & "g.itemgroupid is null " Else SQL = SQL & Conn & " g.itemgroupid is not null "
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
            SQL &= Conn & IIf(F_HasSalesPrice.SelectedValue = "0", "not ", "") & "exists(select top 1 itemid from salesprice sp where sp.itemid = si.itemid ) "
            Conn = " AND "
        End If
        If Not F_IsFreeSample.SelectedValue = Nothing Then
            SQL = SQL & Conn & "IsFreeSample  = " & DB.Number(F_IsFreeSample.SelectedValue)
            Conn = " AND "
        End If
        If Not Request("itemid") = Nothing Then
            SQL = SQL & Conn & "si.itemid not in (" & Request("itemid") & ")"
            Conn = " AND "
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub


    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        F_ItemName.Text = String.Empty
        F_SKU.Text = String.Empty
        F_BrandId.SelectedIndex = 0
        F_DepartmentId.SelectedIndex = 0
        F_GroupName.SelectedIndex = 0
        F_HasSalesPrice.SelectedIndex = 0
        F_IsActive.SelectedIndex = 0
        F_IsFeatured.SelectedIndex = 0
        F_IsFreeSample.SelectedIndex = 0
        F_IsNew.SelectedIndex = 0
        F_ItemType.SelectedIndex = 0
        gvList.PageIndex = 0
        BindList()
    End Sub
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

        'CType(e.Row.FindControl("CollectionLink"), HyperLink).Visible = False

        Dim ltlPrice As Literal = CType(e.Row.FindControl("ltlPrice"), Literal)
        If Convert.ToBoolean(e.Row.DataItem("IsOnSale")) Then
            ltlPrice.Text = "<span class=red>" & FormatCurrency(e.Row.DataItem("SalePrice")) & "</span>"
        Else
            ltlPrice.Text = FormatCurrency(e.Row.DataItem("Price"))
        End If

        Dim Departments As Repeater = CType(e.Row.FindControl("Departments"), Repeater)
        Departments.DataSource = StoreItemRow.GetDepartments(DB, e.Row.DataItem("ItemId"))
        Departments.DataBind()

        If Not IsDBNull(e.Row.DataItem("groupname")) Then
            litType.Text = "<a href=""/admin/store/groups/items.aspx?ItemGroupId=" & e.Row.DataItem("ItemGroupId") & """>" & e.Row.DataItem("groupname") & "</a>"
        End If
        Dim sku As String = e.Row.DataItem("SKU")


        If Type = "1" Then ''write checkbox for multi Selected
            Dim chk As CheckBox = CType(e.Row.FindControl("chk_SKU"), CheckBox)
            chk.Attributes.Add("onclick", String.Format("CheckItem('{0}','{1}', this.checked);", sku, e.Row.DataItem("IsActive")))
            If skuSelect.Contains(";" + sku + ";") Then
                If Not chk Is Nothing Then
                    chk.Checked = True
                    chk.Enabled = False
                End If
            End If

        End If

    End Sub
    Public Function getRadioSelect(ByVal sku As String) As String
        Dim skuTMP As String = skuSelect + ";"
        If skuTMP.Contains(";" + sku + ";") Then
            Return "checked='checked'"
        End If
        Return ""
    End Function
End Class
