Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_shipping_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_MethodId.Datasource = ShipmentMethodRow.GetAllShipmentMethods(DB)
            F_MethodId.DataValueField = "MethodId"
            F_MethodId.DataTextField = "Name"
            F_MethodId.Databind()
            F_MethodId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_MethodId.SelectedValue = Request("F_MethodId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ShippingRangeId"

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM ShippingRange sr inner join shipmentmethod sm on sr.methodid = sm.methodid "

        If Not F_MethodId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sm.MethodId = " & DB.Quote(F_MethodId.SelectedValue)
            Conn = " AND "
        End If
        If Not Trim(F_Zipcode.Text) = String.Empty Then
            SQL = SQL & Conn & DB.Quote(F_Zipcode.Text) & " between lowvalue and highvalue "
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sr.*, sm.[name] "
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataSet = DB.GetDataSet(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
End Class

