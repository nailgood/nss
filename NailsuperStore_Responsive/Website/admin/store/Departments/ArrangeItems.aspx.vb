Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_Departments_ArrangeItems
    Inherits AdminPage
    Protected deptId As Integer = 0
    Protected deptName As String = String.Empty
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            Dim LowPriceExp As String = "[dbo].[fc_StoreItem_GetLowprice](si.itemgroupid,si.price)"
            Dim lowSalePriceExp As String = "coalesce(sp.unitprice,si.price)"
            If gvList.SortBy = String.Empty Then gvList.SortBy = " sdi.Arrange ASC, ItemName, " & lowSalePriceExp & " asc, " & LowPriceExp

     
            BindList()
        End If
    End Sub

    Private Sub BindList()
        If Not String.IsNullOrEmpty(Request("F_DepartmentId")) Then
            deptId = Request("F_DepartmentId")
            deptName = StoreDepartmentRow.GetRow(DB, deptId).Name
        End If
        Dim SQLFields, SQL As String
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        Dim Conn As String = " where "
        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sdi.*,si.isactive, itemname + ' (' + SKU + ')' as itemname "
        SQL = " FROM StoreItem si  left outer join (select min(unitprice) as unitprice, itemid from salesprice with (nolock) where minimumquantity = 1 and getdate() between coalesce(startingdate,convert(datetime, convert(varchar(10), getdate(),121 ) + ' 00:00:00.001')) and coalesce(endingdate+1,getdate()+1) and ((salestype = 0 and memberid = 0) or (memberid is null and salestype = 2) or (salestype = 1 and 0 = CustomerPriceGroupId)) group by itemid) sp on sp.itemid = si.itemid inner join StoreDepartmentItem sdi on si.itemid = sdi.itemid "
        SQL &= Conn & " sdi.DepartmentId = " & deptId
        Dim orderbyArrange As String = ""
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        'If String.IsNullOrEmpty(gvList.SortBy) Then
        '    gvList.SortBy = " ISNULL(arrange,100000) asc,arrange "
        '    gvList.SortOrder = ""
        'End If
        'If gvList.SortByAndOrder.Contains("Arrange") Then
        '    If gvList.SortOrder = "DESC" Then
        '        orderbyArrange = " arrange desc, ISNULL(arrange,100000) asc"
        '    Else
        '        orderbyArrange = " ISNULL(arrange,100000) asc,arrange asc"
        '    End If
        'End If
        'Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("Arrange"), orderbyArrange, gvList.SortByAndOrder))
        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub Update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Update.Click
        Dim isUpdate As Boolean = False
        Try
            If Not String.IsNullOrEmpty(hlistItem.Value) Then
                If Not String.IsNullOrEmpty(deptId) Then
                    deptId = Request("F_DepartmentId")
                End If
                Dim getValue As String = hlistItem.Value.Substring(0, hlistItem.Value.Length - 1)
                Dim arrItems As String() = getValue.Split("|")
                Dim arrVals As String()
                Dim Sql As String = "Update StoreDepartmentItem Set Arrange = Case ItemId {0} End Where ItemId in({1}) And DepartmentId = {2}"
                Dim val1, val2 As String
                For i As Integer = 0 To arrItems.Length - 1
                    arrVals = arrItems(i).Split("-")
                    val1 &= " When " & arrVals(0) & " Then " & IIf(String.IsNullOrEmpty(arrVals(1)), 0, arrVals(1))
                    val2 &= arrVals(0) & ","
                    'DB.ExecuteSQL("Update StoreDepartmentItem Set Arrange = " & IIf(String.IsNullOrEmpty(arrVals(1)), "null", arrVals(1)) & " Where ItemId = " & arrVals(0) & " And DepartmentId = " & deptId)
                Next
                DB.ExecuteSQL(String.Format(Sql, val1, val2.Substring(0, val2.Length - 1), deptId))
            End If
            isUpdate = True
        Catch ex As Exception
            isUpdate = False
        End Try
        If isUpdate Then
            Response.Redirect(Request.RawUrl)
        Else
            AddError("Update Fail!")
        End If

    End Sub
End Class
