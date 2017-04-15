Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_navision_customer_contact_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_ContactNo.Text = Request("F_ContactNo")
            F_ContactName.Text = Request("F_ContactName")
            F_ContactName2.Text = Request("F_ContactName2")
            F_ContactCity.Text = Request("F_ContactCity")
            F_ContactZipcode.Text = Request("F_ContactZipcode")
            F_ContactCounty.Text = Request("F_ContactCounty")
            F_ContactCountry.Text = Request("F_ContactCountry")
            F_ContactEmail.Text = Request("F_ContactEmail")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ContactId"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM CustomerContact  "

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
        If Not F_ContactCity.Text = String.Empty Then
            SQL = SQL & Conn & "ContactCity LIKE " & DB.FilterQuote(F_ContactCity.Text)
            Conn = " AND "
        End If
        If Not F_ContactZipcode.Text = String.Empty Then
            SQL = SQL & Conn & "ContactZipcode LIKE " & DB.FilterQuote(F_ContactZipcode.Text)
            Conn = " AND "
        End If
        If Not F_ContactCounty.Text = String.Empty Then
            SQL = SQL & Conn & "ContactCounty LIKE " & DB.FilterQuote(F_ContactCounty.Text)
            Conn = " AND "
        End If
        If Not F_ContactCountry.Text = String.Empty Then
            SQL = SQL & Conn & "ContactCountry LIKE " & DB.FilterQuote(F_ContactCountry.Text)
            Conn = " AND "
        End If
        If Not F_ContactEmail.Text = String.Empty Then
            SQL = SQL & Conn & "ContactEmail LIKE " & DB.FilterQuote(F_ContactEmail.Text)
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
End Class

