Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_faq_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_FaqCategoryId.Datasource = FaqCategoryRow.GetAllFaqCategorys(DB)
            F_FaqCategoryId.DataValueField = "FaqCategoryId"
            F_FaqCategoryId.DataTextField = "CategoryName"
            F_FaqCategoryId.Databind()
            F_FaqCategoryId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_Question.Text = Request("F_Question")
            F_IsActive.SelectedValue = Request("F_IsActive")
            F_FaqCategoryId.SelectedValue = Request("F_FaqCategoryId")

            F_IsMyOrderPage.SelectedValue = Request("F_IsMyOrderPage")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "f.SortOrder"

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM Faq as f join FaqCategory as fc on fc.FaqCategoryId = f.FaqCategoryId "

        If Not F_FaqCategoryId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "f.FaqCategoryId = " & DB.Quote(F_FaqCategoryId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_Question.Text = String.Empty Then
            SQL = SQL & Conn & "Question LIKE " & DB.FilterQuote(F_Question.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "f.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsMyOrderPage.SelectedValue = String.Empty Then
            SQL &= Conn & "f.IsMyOrderPage = " & DB.Number(F_IsMyOrderPage.SelectedValue)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " f.*, fc.CategoryName "
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT COUNT(*) " & hidCon.Value).Rows.Count

        'Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()

        If F_FaqCategoryId.SelectedValue = Nothing OrElse F_FaqCategoryId.SelectedValue = String.Empty Then
            Me.gvList.Columns(6).Visible = False
            Me.gvList.Columns(7).Visible = False
        Else
            Me.gvList.Columns(6).Visible = True
            Me.gvList.Columns(7).Visible = True
        End If
    End Sub
    'end 05/11/2009

    Private Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not e.Row.RowType = DataControlRowType.DataRow Then
            Exit Sub
        End If

        If Not IsDBNull(e.Row.DataItem("Email")) AndAlso Not CBool(e.Row.DataItem("IsActive")) Then
            e.Row.Style.Add("background", "#99ccff")
        End If
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
End Class
