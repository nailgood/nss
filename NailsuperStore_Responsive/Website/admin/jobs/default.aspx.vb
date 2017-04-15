Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_jobs_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_State.Items.AddRange(StateRow.GetStateList().ToArray())
            F_State.Databind()
            F_State.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_CategoryId.Datasource = JobCategoryRow.GetAllJobCategories(DB)
            F_CategoryId.DataValueField = "JobCategoryId"
            F_CategoryId.DataTextField = "JobCategory"
            F_CategoryId.Databind()
            F_CategoryId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_CategoryId.SelectedValue = Request("F_CategoryId")
            F_City.Text = Request("F_City")
            F_Zip.Text = Request("F_Zip")
            F_Company.Text = Request("F_Company")
            F_State.SelectedValue = Request("F_State")
            F_IsActive.SelectedValue = Request("F_IsActive")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreateDate"
                gvList.SortOrder = "Desc"
            End If
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Job j inner join jobcategory c on j.categoryid = c.jobcategoryid "

        If Not F_CategoryId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "CategoryId = " & DB.Quote(F_CategoryId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_State.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "State = " & DB.Quote(F_State.SelectedValue)
            Conn = " AND "
        End If
        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & "Title like " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        If Not F_City.Text = String.Empty Then
            SQL = SQL & Conn & "City LIKE " & DB.FilterQuote(F_City.Text)
            Conn = " AND "
        End If
        If Not F_Zip.Text = String.Empty Then
            SQL = SQL & Conn & "Zip LIKE " & DB.FilterQuote(F_Zip.Text)
            Conn = " AND "
        End If
        If Not F_Company.Text = String.Empty Then
            SQL = SQL & Conn & "Company LIKE " & DB.FilterQuote(F_Company.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL &= Conn & "isactive = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " and"
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
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
End Class

