Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class Index
	Inherits AdminPage

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList

        If hidPopUpSKU.value <> "" Then
            Dim arr As Array = Split(hidPopUpSKU.Value.Trim(), ";")
            Dim dt As DataTable = DB.GetDataTable("select spi.* from salesprice spi inner join StoreItem si on spi.ItemId=si.ItemId where si.sku = '" & arr(0).ToString() & "' order by EndingDate desc")
            Dim si As StoreItemRow = StoreItemRow.GetRowSku(DB, arr(0).ToString())
            If dt.Rows.Count > 0 Then
                Response.Redirect("../../store/items/salesprice/edit.aspx?act=y&SalesPriceId=" & dt.Rows(0)("SalesPriceId"))
            End If
            If si.ItemId > 0 Then
                Response.Redirect("../../store/items/salesprice/edit.aspx?act=y&ItemId=" & si.ItemId)
            End If

        End If
		If Not IsPostBack Then

            'F_SKU.Text = Request("F_PromotionCode")
            'F_IsActive.Text = Request("F_IsActive")
            'F_StartDateLbound.Text = Request("F_StartDateLBound")
            'F_StartDateUbound.Text = Request("F_StartDateUBound")
            'F_EndDateLbound.Text = Request("F_EndDateLBound")
            'F_EndDateUbound.Text = Request("F_EndDateUBound")

			gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
			gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "SalesPriceId"

			BindList()
		End If
	End Sub

	Private Sub BindList()
		Dim SQLFields, SQL As String
        Dim Conn As String = " AND "

		ViewState("F_SortBy") = gvList.SortBy
		ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sp.*,si.SKU,si.ItemName "
        SQL = " FROM salesprice sp, Storeitem si WHERE sp.itemid=si.itemid "

        'If Not F_SKU.Text = String.Empty Then
        '    SQL = SQL & Conn & "SKU LIKE " & DB.FilterQuote(F_SKU.Text)
        '    Conn = " AND "
        'End If

        'If Not F_IsActive.SelectedValue = String.Empty Then
        '    SQL = SQL & Conn & "sp.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
        '    Conn = " AND "
        'End If
        'If Not F_StartDateLbound.Text = String.Empty Then
        '    SQL = SQL & Conn & "StartingDate >= " & DB.Quote(F_StartDateLbound.Text)
        '    Conn = " AND "
        'End If
        'If Not F_StartDateUbound.Text = String.Empty Then
        '    SQL = SQL & Conn & "StartingDate < " & DB.Quote(DateAdd("d", 1, F_StartDateUbound.Text))
        '    Conn = " AND "
        'End If
        'If Not F_EndDateLbound.Text = String.Empty Then
        '    SQL = SQL & Conn & "EndingDate >= " & DB.Quote(F_EndDateLbound.Text)
        '    Conn = " AND "
        'End If
        'If Not F_EndDateUbound.Text = String.Empty Then
        '    SQL = SQL & Conn & "EndingDate < " & DB.Quote(DateAdd("d", 1, F_EndDateUbound.Text))
        '    Conn = " AND "
        'End If
        SQL = SQL & Conn & " sp.IsActive = 1 and EndingDate > = GETDATE()"
		gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        Dim i As Integer = 0
        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        If gvList.Pager.NofRecords > 0 Then
            For i = 0 To res.Tables(0).Rows.Count - 1
                If res.Tables(0).Rows(i)("IsActive") = True Then
                    hidPopUpSKU.Value += res.Tables(0).Rows(i)("sku") + ";"
                End If
            Next

        End If
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    'Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    If Not Page.IsValid Then Exit Sub

    '    gvList.PageIndex = 0
    '    BindList()
    'End Sub
End Class

