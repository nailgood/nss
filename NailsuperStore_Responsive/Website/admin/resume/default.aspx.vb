Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_resume_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_SubjectId.DataSource = MemberResumeRow.GetJobCategory(DB)
            F_SubjectId.DataValueField = "jobcategoryid"
            F_SubjectId.DataTextField = "jobcategory"
            F_SubjectId.DataBind()
            F_SubjectId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_CreateDateLbound.Text = Request("F_CreateDateLBound")
            F_CreateDateUbound.Text = Request("F_CreateDateUBound")

            F_FirstName.Text = Request("F_FirstName")
            F_LastName.Text = Request("F_LastName")
            F_EmailAddress.Text = Request("F_EmailAddress")
            F_Phone.Text = Request("F_Phone")
            F_OrderNumber.Text = Request("F_OrderNumber")
            F_SubjectId.SelectedValue = Request("F_SubjectId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))

            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "AgreeDate"
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

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM MemberResume  "

        If Not F_SubjectId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "jobcategoryid = " & DB.Quote(F_SubjectId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_FirstName.Text = String.Empty Then
            SQL = SQL & Conn & "FirstName LIKE " & DB.FilterQuote(F_FirstName.Text)
            Conn = " AND "
        End If
        If Not F_LastName.Text = String.Empty Then
            SQL = SQL & Conn & "LastName LIKE " & DB.FilterQuote(F_LastName.Text)
            Conn = " AND "
        End If
        If Not F_EmailAddress.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_EmailAddress.Text)
            Conn = " AND "
        End If
        If Not F_Phone.Text = String.Empty Then
            SQL = SQL & Conn & "Phone LIKE " & DB.FilterQuote(F_Phone.Text)
            Conn = " AND "
        End If
        'If Not F_OrderNumber.Text = String.Empty Then
        '    SQL = SQL & Conn & "OrderNumber LIKE " & DB.FilterQuote(F_OrderNumber.Text)
        '    Conn = " AND "
        'End If
        If Not F_CreateDateLBound.Text = String.Empty Then
            SQL = SQL & Conn & "AgreeDate >= " & DB.Quote(F_CreateDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_CreateDateUBound.Text = String.Empty Then
            SQL = SQL & Conn & "AgreeDate < " & DB.Quote(DateAdd("d", 1, F_CreateDateUbound.Text))
            Conn = " AND "
        End If
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    ' Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
    '    Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    'End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
End Class
