Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_ShippingCountry_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_CountryId.Items.AddRange(CountryRow.GetCountries().ToArray())
            F_CountryId.DataBind()
            F_CountryId.Items.Insert(0, New ListItem("-- ALL --", ""))
            F_CountryId.Items.RemoveAt(F_CountryId.Items.IndexOf(F_CountryId.Items.FindByText("United States")))
            F_ShippingCode.Text = Request("F_ShippingCode")
            'F_IsActive.Text = Request("F_LastName")
            'F_EmailAddress.Text = Request("F_EmailAddress")
            'F_Phone.Text = Request("F_Phone")
            'F_OrderNumber.Text = Request("F_OrderNumber")
            F_CountryId.SelectedValue = Request("F_CountryId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "CountryId"

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " AND "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM Country WHERE countryname<>'United States' "

        If Not F_CountryId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "CountryId = " & DB.Quote(F_CountryId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ShippingCode.Text = String.Empty Then
            SQL = SQL & Conn & "ShippingCode LIKE " & DB.FilterQuote(F_ShippingCode.Text)
            Conn = " AND "
        End If
        If Not F_IsShippingActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsShippingActive  = " & DB.Number(F_IsShippingActive.SelectedValue)
            Conn = " AND "
        End If
        'If Not F_EmailAddress.Text = String.Empty Then
        '	SQL = SQL & Conn & "EmailAddress LIKE " & DB.FilterQuote(F_EmailAddress.Text)
        '	Conn = " AND "
        'End If
        'If Not F_Phone.Text = String.Empty Then
        '	SQL = SQL & Conn & "Phone LIKE " & DB.FilterQuote(F_Phone.Text)
        '	Conn = " AND "
        'End If
        'If Not F_OrderNumber.Text = String.Empty Then
        '	SQL = SQL & Conn & "OrderNumber LIKE " & DB.FilterQuote(F_OrderNumber.Text)
        '	Conn = " AND "
        'End If
        'If Not F_CreateDateLBound.Text = String.Empty Then
        '	SQL = SQL & Conn & "CreateDate >= " & DB.Quote(F_CreateDateLBound.Text)
        '	Conn = " AND "
        'End If
        'If Not F_CreateDateUBound.Text = String.Empty Then
        '	SQL = SQL & Conn & "CreateDate < " & DB.Quote(DateAdd("d", 1, F_CreateDateUBound.Text))
        '	Conn = " AND "
        'End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        'Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
End Class

