Imports Components

Partial Class admin_store_items_policies_new
    Inherits AdminPage
    Dim itemId As Integer
    
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        itemId = Request("itemId")

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            BindList()
        End If
    End Sub

    Private Sub BindList()

        Dim dt As DataTable = DB.GetDataTable("select a.PolicyId, Title, IsActive, case when b.ItemId is not null then 1 else 0 end as Selected from Policy a " & _
                                                "left join PolicyItem b on b.PolicyId = a.PolicyId and b.ItemId = " & itemId.ToString() & _
                                                " where isnull(isActive, 0) = 1")

        gvList.Pager.NofRecords = dt.Rows.Count
        'Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        'gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataSource = dt
        gvList.DataBind()
    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If

        Dim policyId As String = e.Row.DataItem("PolicyId")
        Dim selected As Boolean = e.Row.DataItem("Selected")

        Dim chk As CheckBox = CType(e.Row.FindControl("chk_policySel"), CheckBox)
        chk.Enabled = Not selected
        chk.Checked = selected
        chk.Attributes.Add("onclick", String.Format("CheckItem('{0}','{1}', this.checked);", policyId, e.Row.DataItem("Selected")))
       

    End Sub
End Class
