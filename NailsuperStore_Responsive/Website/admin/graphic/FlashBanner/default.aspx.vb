Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_salesBanner_Index
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


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




    Protected Sub chkActive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkActive.CheckedChanged
        BindList()
    End Sub
    Protected Sub chkRunning_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRunning.CheckedChanged
        BindList()
    End Sub
End Class

