Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_shippingint_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_MethodId.Datasource = ShipmentMethodRow.GetAllShipmentMethods(DB)
            F_MethodId.DataValueField = "MethodId"
            F_MethodId.DataTextField = "Name"
            F_MethodId.Databind()
            F_MethodId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_CountryCode.Items.AddRange(CountryRow.GetCountries().ToArray())
            F_CountryCode.DataBind()
            F_CountryCode.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_MethodId.SelectedValue = Request("F_MethodId")
            F_CountryCode.SelectedValue = Request("F_CountryCode")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ShippingRangeIntId"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM ShippingRangeInt  "

        If Not F_MethodId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "MethodId = " & DB.Quote(F_MethodId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_CountryCode.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "CountryCode = " & DB.Quote(F_CountryCode.SelectedValue)
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

