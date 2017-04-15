Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_jobs_applications_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_CompanyName.Text = Request("F_CompanyName")
            F_Website.Text = Request("F_Website")
            F_Email.Text = Request("F_Email")
            F_IsApproved.Text = Request("F_IsApproved")
            F_CreateDateLBound.Text = Request("F_CreateDateLBound")
            F_CreateDateUBound.Text = Request("F_CreateDateUBound")
            F_ApproveDateLBound.Text = Request("F_ApproveDateLBound")
            F_ApproveDateUBound.Text = Request("F_ApproveDateUBound")
            If F_IsApproved.SelectedValue = String.Empty Then F_IsApproved.SelectedValue = "0"

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))

            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreateDate"
                gvList.SortOrder = "Desc"
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

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " a.*, m.username "
        SQL = " FROM PostJobApplication a inner join member m on a.memberid = m.memberid "

        If Not F_Website.Text = String.Empty Then
            SQL = SQL & Conn & "Website = " & DB.Quote(F_Website.Text)
            Conn = " AND "
        End If
        If Not F_CompanyName.Text = String.Empty Then
            SQL = SQL & Conn & "CompanyName LIKE " & DB.FilterQuote(F_CompanyName.Text)
            Conn = " AND "
        End If
        If Not F_Email.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
            Conn = " AND "
        End If
        If Not F_IsApproved.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsApproved  = " & DB.Number(F_IsApproved.SelectedValue)
            Conn = " AND "
        End If
        If Not F_CreateDateLBound.Text = String.Empty Then
            SQL = SQL & Conn & "a.CreateDate >= " & DB.Quote(F_CreateDateLBound.Text)
            Conn = " AND "
        End If
        If Not F_CreateDateUBound.Text = String.Empty Then
            SQL = SQL & Conn & "a.CreateDate < " & DB.Quote(DateAdd("d", 1, F_CreateDateUBound.Text))
            Conn = " AND "
        End If
        If Not F_ApproveDateLBound.Text = String.Empty Then
            SQL = SQL & Conn & "ApproveDate >= " & DB.Quote(F_ApproveDateLBound.Text)
            Conn = " AND "
        End If
        If Not F_ApproveDateUBound.Text = String.Empty Then
            SQL = SQL & Conn & "ApproveDate < " & DB.Quote(DateAdd("d", 1, F_ApproveDateUBound.Text))
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

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
End Class

