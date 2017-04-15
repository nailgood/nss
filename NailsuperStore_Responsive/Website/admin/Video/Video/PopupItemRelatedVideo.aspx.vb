Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_Video_Video_PopupItemRelatedVideo
    Inherits AdminPage
    Dim idSelect As String = String.Empty
    Dim ItemIdSelect As String = String.Empty

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

        idSelect = ";" + Request("item")
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_BrandId.DataSource = StoreBrandRow.GetAllStoreBrands(DB)
            F_BrandId.DataValueField = "BrandId"
            F_BrandId.DataTextField = "BrandName"
            F_BrandId.DataBind()
            F_BrandId.Items.Insert(0, New ListItem("--All--", ""))

            F_GroupName.DataSource = StoreItemGroupRow.GetAllItemGroups(DB)
            F_GroupName.DataValueField = "ItemGroupId"
            F_GroupName.DataTextField = "GroupName"
            F_GroupName.DataBind()
            F_GroupName.Items.Insert(0, New ListItem("--All--", ""))
            Type = Request("Type")
            F_DepartmentId.SelectedValue = Request("F_DepartmentId")
            F_ItemType.SelectedValue = Request("F_ItemType")
            F_IsActive.SelectedValue = Request("F_IsActive")
            F_IsFreeSample.SelectedValue = Request("F_IsFreeSample")
            F_ItemName.Text = Request("F_ItemName")
            F_SKU.Text = Request("F_SKU")
            F_IsFeatured.SelectedValue = Request("F_IsFeatured")
            F_IsNew.SelectedValue = Request("F_IsNew")
            F_HasSalesPrice.SelectedValue = Request("F_HasSalesPrice")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ItemId"

            BindDepartmentsDropDown(DB, F_DepartmentId, 2, "-- ALL --")

            BindList()
        End If
    End Sub

    Public Shared Sub BindDepartmentsDropDown(ByVal DB As Database, ByVal DepartmentId As DropDownList, ByVal Level As Integer, ByVal FirstField As String)
        Dim sql As String
        If Level = Nothing Then
            sql = "select * from StoreDepartment order by Name asc"
        Else
            sql = "SELECT P1.DepartmentId, REPLICATE('- ', COUNT(P2.DepartmentId)-2) + p1.NAME AS NAME" _
            & " FROM StoreDepartment P1, StoreDepartment P2" _
             & " WHERE P1.lft BETWEEN P2.lft AND P2.rgt" _
             & " GROUP BY P1.DepartmentId, P1.lft, p1.rgt, p1.NAME" _
             & " HAVING COUNT(P2.DepartmentId) > 1 AND COUNT(P2.DepartmentId) <= " & DB.Quote((Level + 1).ToString) _
             & " ORDER BY P1.lft"
        End If
        Dim ds As DataSet = DB.GetDataSet(sql)
        DepartmentId.DataSource = ds
        DepartmentId.DataValueField = "DepartmentId"
        DepartmentId.DataTextField = "NAME"
        DepartmentId.DataBind()
        If Not FirstField = "{BLANK}" Then DepartmentId.Items.Insert(0, New ListItem(FirstField, ""))
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim conn As String = " where "
        'hidVideoSelect.Value = String.Empty
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        SQLFields = "select top " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM StoreItem si left outer join storeitemgroup g on si.itemgroupid = g.itemgroupid left outer join storebrand sb on si.brandid = sb.brandid "
        If Not F_ItemName.Text = String.Empty Then
            SQL = SQL & conn & " ItemName like " & DB.FilterQuote(F_ItemName.Text)
            conn = " and "
        End If
        If Not F_SKU.Text = String.Empty Then
            SQL = SQL & conn & " SKU like " & DB.FilterQuote(F_SKU.Text)
            conn = " and "
        End If
        If Not F_DepartmentId.SelectedValue = String.Empty Then
            SQL = SQL & conn & " si.ItemId in(select ItemId from StoreDepartmentItem where DepartmentId= " & DB.Quote(F_DepartmentId.SelectedValue) & ")"
            conn = " and "
        End If
        If Not F_ItemType.Text = String.Empty Then
            If F_ItemType.Text = "0" Then SQL = SQL & conn & " g.itemgroupid is null " Else SQL = SQL & conn & " g.itemgroupid is not null "
            conn = " and "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & conn & " IsActive = " & DB.Number(F_IsActive.SelectedValue)
            conn = " and "
        End If
        If Not F_BrandId.SelectedValue = String.Empty Then
            SQL = SQL & conn & " sb.BrandId = " & DB.Quote(F_BrandId.SelectedValue)
            conn = " and "
        End If
        If Not F_GroupName.SelectedValue = String.Empty Then
            SQL = SQL & conn & " si.itemgroupid = " & DB.Number(F_GroupName.SelectedValue)
            conn = " and "
        End If
        If Not F_IsFreeSample.SelectedValue = String.Empty Then
            SQL = SQL & conn & " IsFreeSample = " & DB.Number(F_IsFreeSample.SelectedValue)
            conn = " and "
        End If
        If Not F_IsFeatured.SelectedValue = String.Empty Then
            SQL = SQL & conn & " IsFeatured = " & DB.Number(F_IsFeatured.SelectedValue)
            conn = " and "
        End If
        If Not F_IsNew.SelectedValue = String.Empty Then
            SQL = SQL & conn & " IsNew = " & DB.Number(F_IsNew.SelectedValue)
            conn = " and "
        End If
        If Not F_HasSalesPrice.SelectedValue = String.Empty Then
            SQL = SQL & conn & IIf(F_HasSalesPrice.SelectedValue = "0", "not ", "") & "exists(select top 1 itemid from salesprice sp where sp.itemid = si.itemid ) "
            conn = " and "
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("select count(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " order by " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        F_DepartmentId.SelectedIndex = 0
        F_ItemType.SelectedIndex = 0
        F_IsActive.SelectedIndex = 0
        F_IsFreeSample.SelectedIndex = 0
        F_BrandId.SelectedIndex = 0
        F_IsFeatured.SelectedIndex = 0
        F_IsNew.SelectedIndex = 0
        F_HasSalesPrice.SelectedIndex = 0
        F_ItemName.Text = String.Empty
        F_SKU.Text = String.Empty
        gvList.PageIndex = 0
        BindList()
    End Sub

    'Public Function getRadioSelect(ByVal sku As String) As String
    '    Dim ItemIdTemp As String = ItemIdSelect + ";"
    '    If ItemIdTemp.Contains(";" + sku + ";") Then
    '        Return "checked='checked'"
    '    End If
    '    Return ""
    'End Function

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim litType As Literal = e.Row.FindControl("litType")
        Dim ltlPrice As Literal = e.Row.FindControl("ltlPrice")
        If Convert.ToBoolean(e.Row.DataItem("IsOnSale")) Then
            ltlPrice.Text = "<span class=red>" & FormatCurrency(e.Row.DataItem("SalePrice")) & "</span>"
        Else
            ltlPrice.Text = FormatCurrency(e.Row.DataItem("Price"))
        End If

        Dim Departments As Repeater = CType(e.Row.FindControl("Departments"), Repeater)
        Departments.DataSource = StoreItemRow.GetDepartments(DB, e.Row.DataItem("ItemId"))
        Departments.DataBind()
        If Not IsDBNull(e.Row.DataItem("GroupName")) Then
            litType.Text = "<a href=""/admin/store/groups/items.aspx?ItemGroupId=" & e.Row.DataItem("ItemGroupId") & """>" & e.Row.DataItem("groupname") & "</a>"
        End If
        Dim ItemId As String = e.Row.DataItem("ItemId")

        Dim chk As CheckBox = CType(e.Row.FindControl("chk_ItemId"), CheckBox)
        chk.Attributes.Add("onclick", "CheckItem('" + ItemId + "',this.checked);")
        If idSelect.Contains(";" + ItemId + ";") Then
            If Not chk Is Nothing Then
                chk.Checked = True
                chk.Enabled = False
            End If
        End If
    End Sub
End Class
