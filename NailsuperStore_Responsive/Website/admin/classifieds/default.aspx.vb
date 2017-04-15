Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_classifieds_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_ClassifiedCategoryId.Datasource = ClassifiedCategoryRow.GetAllClassifiedCategories(DB)
            F_ClassifiedCategoryId.DataValueField = "ClassifiedCategoryId"
            F_ClassifiedCategoryId.DataTextField = "Category"
            F_ClassifiedCategoryId.Databind()
            F_ClassifiedCategoryId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_Title.Text = Request("F_Title")
            F_IsActive.Text = Request("F_IsActive")
            F_ClassifiedCategoryId.SelectedValue = Request("F_ClassifiedCategoryId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "ExpirationDate"
                gvList.SortOrder = "desc"
            End If

            BindList()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " c.*, cc.category "
        SQL = " FROM Classified c inner join classifiedcategory cc on c.classifiedcategoryid = cc.classifiedcategoryid "

        If Not F_ClassifiedCategoryId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "c.ClassifiedCategoryId = " & DB.Quote(F_ClassifiedCategoryId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & "Title LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count '' ClassifiedRow.GetListClassified(DB, "SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder) '' ClassifiedRow.GetListClassified(DB, SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
End Class

