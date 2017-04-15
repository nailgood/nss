Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_salesBanner_Index
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckAccess("STORE")

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            Session("deptId") = Request("DepartmentId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "[SortOrder]"
                gvList.SortOrder = "ASC"
            End If
            LoadDepartment()
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Banner  "

        If ddlDepartment.SelectedValue <> 0 Or Request("DepartmentId") Is Nothing = False Then
            SQL = SQL & Conn & "DepartmentId  = " & IIf(ddlDepartment.SelectedValue <> 0, DB.Number(ddlDepartment.SelectedValue), Request("DepartmentId"))
            Conn = " AND "
        End If

        If (chkActive.Checked) Then
            SQL = SQL & Conn & "IsActive  = 1"
        Else
            SQL = SQL & Conn & "IsActive  = 0"
        End If

        Conn = " AND "
        If (chkRunning.Checked) Then
            SQL = SQL & Conn & " GETDATE() BETWEEN StartingDate AND EndingDate"
        End If


        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        If Not res Is Nothing Then
            TotalRecords = res.Rows.Count
        End If
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
        If (Not chkActive.Checked Or TotalRecords < 2 Or gvList.SortBy <> "[SortOrder]") Then
            gvList.Columns(gvList.Columns.Count - 1).Visible = False
        Else
            gvList.Columns(gvList.Columns.Count - 1).Visible = True
        End If
    End Sub
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand

        Try
            If e.CommandName = "Down" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim id1 As String = gvList.DataKeys(index).Value
                Dim id2 As String = gvList.DataKeys(index + 1).Value
                BannerRow.SwapOrderBanner(DB, id1, id2)
            ElseIf (e.CommandName = "Up") Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim id1 As String = gvList.DataKeys(index).Value
                Dim id2 As String = gvList.DataKeys(index - 1).Value
                BannerRow.SwapOrderBanner(DB, id1, id2)
            End If
        Catch ex As Exception

        End Try
        
        BindList()
    End Sub
   
    Private Sub LoadDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartment.DataSource = ds
        ddlDepartment.DataTextField = "Name"
        ddlDepartment.DataValueField = "DepartmentId"
        ddlDepartment.DataBind()
        ddlDepartment.Items.Insert(0, New ListItem("Home Page", 23))

        If Not Request("DepartmentId") Is Nothing Then
            ddlDepartment.SelectedValue = Request("DepartmentId")
        End If
        If (String.IsNullOrEmpty(ddlDepartment.SelectedValue)) Then
            ddlDepartment.SelectedIndex = 0
        End If

        ds.Dispose()
        EnableEffect(True)
    End Sub
    Private Sub EnableEffect(ByVal enable As Boolean)
        trEffect.Visible = enable
        trSaveEffect.Visible = enable
    End Sub
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?DepartmentId=" & Session("deptId")) ' & "&" & GetPageParams(FilterFieldType.All))
    End Sub
    Private TotalRecords As Integer = 0
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim litDepartment As Literal = CType(e.Row.FindControl("litDepartment"), Literal)
            Dim dtb As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, e.Row.DataItem("DepartmentID"))
            litDepartment.Text = dtb.Name
            If (chkActive.Checked) Then
                Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
                Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
                If e.Row.DataItemIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.DataItemIndex = TotalRecords - 1 Then
                    imbDown.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        If ddlDepartment.SelectedValue = 0 Then
            Session("deptId") = Nothing
            Response.Redirect("default.aspx")
        Else
            Session("deptId") = ddlDepartment.SelectedValue
            gvList.PageIndex = 0
        End If


        If ddlDepartment.SelectedValue = 0 Then
            EnableEffect(False)
        Else
            EnableEffect(True)
            ''get list effect here
            Dim dt As DataTable = DB.GetDataTable("Select Code,Name from SlideEffect order by Arrange ASC")
            chkEffect.DataSource = dt
            chkEffect.DataTextField = "Name"
            chkEffect.DataValueField = "Code"
            chkEffect.DataBind()
            ''get list selected
            Dim lstEffectSelect As DepartmentSlideEffectCollection = DepartmentSlideEffectRow.ListByDepartment(DB, ddlDepartment.SelectedValue)
            If Not lstEffectSelect Is Nothing Then
                If lstEffectSelect.Count > 0 Then
                    Dim isSelect As Boolean = False
                    For Each item As ListItem In chkEffect.Items
                        isSelect = False
                        For Each itemselected As DepartmentSlideEffectRow In lstEffectSelect
                            If itemselected.EffectCode.ToLower().Trim() = item.Value.Trim().ToLower() Then
                                isSelect = True
                                Exit For
                            End If
                        Next
                        item.Selected = isSelect
                    Next
                End If
            End If
        End If
        gvList.SortBy = "[SortOrder]"
        gvList.SortOrder = "ASC"
        BindList()
    End Sub

    'Protected Sub ddlEffect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEffect.SelectedIndexChanged
    '    Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, CInt(ddlDepartment.SelectedValue))
    '    dep.BannerEffect = ddlEffect.SelectedValue
    '    dep.Update()
    'End Sub

    Protected Sub btnSaveEffect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveEffect.Click
        If Page.IsValid Then
            Dim departmentId As Integer = 0
            Try
                departmentId = CInt(ddlDepartment.SelectedValue)
            Catch ex As Exception

            End Try
            If departmentId < 1 Then
                AddError("You must select Department")
                Exit Sub
            End If
            ''GET LIST SELECT
            Dim arr As New ArrayList
            For Each item As ListItem In chkEffect.Items
                If item.Selected Then
                    arr.Add(item.Value)
                End If
            Next
            If arr.Count < 1 Then
                AddError("You must select effect")
                Exit Sub
            End If
            ''delete 
            DepartmentSlideEffectRow.DeleteByDepartment(DB, departmentId)
            For Each item As String In arr
                Dim data As New DepartmentSlideEffectRow
                data.EffectCode = item
                data.DepartmentId = departmentId
                DepartmentSlideEffectRow.Insert(DB, data)
            Next
        End If
    End Sub

    Protected Sub chkActive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkActive.CheckedChanged
        BindList()
    End Sub
    Protected Sub chkRunning_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRunning.CheckedChanged
        BindList()
    End Sub
End Class

