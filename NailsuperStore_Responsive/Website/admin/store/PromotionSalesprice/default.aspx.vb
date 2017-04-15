Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_promotionsalesprice_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then


            SubTitle.Text = Request("SubTitle")
            MainTitle.Text = Request("MainTitle")
            F_IsActive.Text = Request("F_IsActive")


            F_StartingDateLbound.Text = Request("F_StartingDateLBound")
            F_StartingDateUbound.Text = Request("F_StartingDateUBound")
            F_EndingDateLbound.Text = Request("F_EndingDateLBound")
            F_EndingDateUbound.Text = Request("F_EndingDateUBound")


            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Id"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM PromotionSalesprice "


        If Not SubTitle.Text = String.Empty Then
            SQL = SQL & Conn & "SubTitle LIKE " & DB.FilterQuote(SubTitle.Text)
            Conn = " AND "
        End If
        If Not MainTitle.Text = String.Empty Then
            SQL = SQL & Conn & "MainTitle LIKE " & DB.FilterQuote(MainTitle.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_StartingDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartingDate >= " & DB.Quote(F_StartingDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_StartingDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartingDate < " & DB.Quote(DateAdd("d", 1, F_StartingDateUbound.Text))
            Conn = " AND "
        End If
        If Not F_EndingDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "EndingDate >= " & DB.Quote(F_EndingDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_EndingDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "EndingDate < " & DB.Quote(DateAdd("d", 1, F_EndingDateUbound.Text))
            Conn = " AND "
        End If
        SQL = SQL & Conn & "EndingDate > = " & DB.Quote(Date.Now)
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then

        '    Dim litDepartment As Literal = CType(e.Row.FindControl("litDepartment"), Literal)
        '    Dim dtb As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, e.Row.DataItem("DepartmentID"))
        '    litDepartment.Text = dtb.Name
        'End If
    End Sub
End Class

