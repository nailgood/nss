Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_contacts_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_ContactNo.Text = Request("F_ContactNo")
            F_ContactName.Text = Request("F_ContactName")
            F_ContactName2.Text = Request("F_ContactName2")
            F_Login.Text = Request("F_Login")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ContactNo"

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
        SQL = " FROM CustomerContact  "

        If Not F_Login.Text = String.Empty Then
            SQL = SQL & Conn & "Login = " & DB.Quote(F_Login.Text)
            Conn = " AND "
        End If
        If Not F_ContactNo.Text = String.Empty Then
            SQL = SQL & Conn & "ContactNo LIKE " & DB.FilterQuote(F_ContactNo.Text)
            Conn = " AND "
        End If
        If Not F_ContactName.Text = String.Empty Then
            SQL = SQL & Conn & "ContactName LIKE " & DB.FilterQuote(F_ContactName.Text)
            Conn = " AND "
        End If
        If Not F_ContactName2.Text = String.Empty Then
            SQL = SQL & Conn & "ContactName2 LIKE " & DB.FilterQuote(F_ContactName2.Text)
            Conn = " AND "
        End If
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count ''CustomerContactRow.GetListCustomerContact("SELECT * " & SQL).Rows.Count

        'Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder) '' CustomerContactRow.GetListCustomerContact(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
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

