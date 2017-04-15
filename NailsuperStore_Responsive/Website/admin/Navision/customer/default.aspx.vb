Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_navision_customer_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_CustomerNo.Text = Request("F_CustomerNo")
            F_Name.Text = Request("F_Name")
            F_Name2.Text = Request("F_Name2")
            F_City.Text = Request("F_City")
            F_Zipcode.Text = Request("F_Zipcode")
            F_County.Text = Request("F_County")
            F_Email.Text = Request("F_Email")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "CustomerId"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Customer  "

        If Not F_CustomerNo.Text = String.Empty Then
            SQL = SQL & Conn & "CustomerNo LIKE " & DB.FilterQuote(F_CustomerNo.Text)
            Conn = " AND "
        End If
        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "Name LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_Name2.Text = String.Empty Then
            SQL = SQL & Conn & "Name2 LIKE " & DB.FilterQuote(F_Name2.Text)
            Conn = " AND "
        End If
        If Not F_City.Text = String.Empty Then
            SQL = SQL & Conn & "City LIKE " & DB.FilterQuote(F_City.Text)
            Conn = " AND "
        End If
        If Not F_Zipcode.Text = String.Empty Then
            SQL = SQL & Conn & "Zipcode LIKE " & DB.FilterQuote(F_Zipcode.Text)
            Conn = " AND "
        End If
        If Not F_County.Text = String.Empty Then
            SQL = SQL & Conn & "County LIKE " & DB.FilterQuote(F_County.Text)
            Conn = " AND "
        End If
        If Not F_Email.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
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

