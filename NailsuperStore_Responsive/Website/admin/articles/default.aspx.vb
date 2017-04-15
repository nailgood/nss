Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_articles_index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_Title.Text = Request("F_Title")
            F_IsActive.Text = Request("F_IsActive")
            F_PostDateLbound.Text = Request("F_PostDateLBound")
            F_PostDateUbound.Text = Request("F_PostDateUBound")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "PostDate" : gvList.SortOrder = "DESC"

            BindList()
        End If
    End Sub
    'Long edit 30/10/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Article  "

        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & "Title LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & F_IsActive.SelectedValue
            Conn = " AND "
        End If
        If Not F_PostDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "PostDate >= " & DB.Quote(F_PostDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_PostDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "PostDate < " & DB.Quote(DateAdd("d", 1, F_PostDateUbound.Text))
            Conn = " AND "
        End If
        ' gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count

        'Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 30/10/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
End Class

