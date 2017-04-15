Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_tips_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_TipCategoryId.Datasource = TipCategoryRow.GetAllTipCategories(DB)
            F_TipCategoryId.DataValueField = "TipCategoryId"
            F_TipCategoryId.DataTextField = "TipCategory"
            F_TipCategoryId.Databind()
            F_TipCategoryId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_Title.Text = Request("F_Title")
            F_TipCategoryId.SelectedValue = Request("F_TipCategoryId")

            If F_TipCategoryId.SelectedValue = "" Then
                Response.Redirect("categories/")
            Else
                lit.Text = "'" & F_TipCategoryId.SelectedItem.Text & "'"
            End If

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "SortOrder"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " t.*, c.tipcategory "
        SQL = " FROM Tip t inner join tipcategory c on t.tipcategoryid = c.tipcategoryid "

        If Not F_TipCategoryId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "t.TipCategoryId = " & DB.Quote(F_TipCategoryId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & "Title LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
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

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Active" Then
            TipRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim active As Boolean = True
            Try
                active = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
            Catch ex As Exception
            End Try

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If
    End Sub
End Class

