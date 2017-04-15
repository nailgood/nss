Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_navision_mixmatch_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            Dim type As Integer = 0
            If Not Request.QueryString("type") Then
                type = CInt(Request.QueryString("type"))
            End If
            If type <> Utility.Common.MixmatchType.Normal AndAlso type <> Utility.Common.MixmatchType.ProductCoupon Then
                type = Utility.Common.MixmatchType.Normal
            End If
           
            F_CustomerPriceGroupId.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
            F_CustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
            F_CustomerPriceGroupId.DataTextField = "CustomerPriceGroupCode"
            F_CustomerPriceGroupId.DataBind()
            F_CustomerPriceGroupId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_MixMatchNo.Text = Request("F_MixMatchNo")
            F_Description.Text = Request("F_Description")
            F_Product.Text = Request("F_Product")
            F_IsActive.Text = Request("F_IsActive")
            F_DiscountType.Text = Request("F_DiscountType")
            F_CustomerPriceGroupId.SelectedValue = Request("F_CustomerPriceGroupId")
            F_StartingDateLbound.Text = Request("F_StartingDateLBound")
            F_StartingDateUbound.Text = Request("F_StartingDateUBound")
            F_EndingDateLbound.Text = Request("F_EndingDateLBound")
            F_EndingDateUbound.Text = Request("F_EndingDateUBound")
            F_SKU.Text = Request("F_SKU")
            F_drlType.Text = Request("F_drlType")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "StartingDate "
                gvList.SortOrder = "desc"
            End If

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        SQL = " FROM MixMatch m left outer join customerpricegroup c on m.customerpricegroupid = c.customerpricegroupid "
        If Not String.IsNullOrEmpty(F_drlType.SelectedValue) Then
            SQL = SQL & Conn & "m.[Type] = " & F_drlType.SelectedValue
            Conn = " AND "
        End If

        If Not F_CustomerPriceGroupId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "m.CustomerPriceGroupId = " & DB.Quote(F_CustomerPriceGroupId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_MixMatchNo.Text = String.Empty Then
            SQL = SQL & Conn & "MixMatchNo LIKE " & DB.FilterQuote(F_MixMatchNo.Text)
            Conn = " AND "
        End If
        If Not F_Product.Text = String.Empty Then
            SQL = SQL & Conn & "m.Product LIKE " & DB.FilterQuote(F_Product.Text)
            Conn = " AND "
        End If
        If Not F_Description.Text = String.Empty Then
            SQL = SQL & Conn & "m.Description LIKE " & DB.FilterQuote(F_Description.Text)
            Conn = " AND "
        End If
        If Not F_DiscountType.Text = String.Empty Then
            SQL = SQL & Conn & "DiscountType LIKE " & DB.FilterQuote(F_DiscountType.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_StartingDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartingDate >= " & DB.Quote(F_StartingDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_StartingDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartingDate < " & DB.Quote(DateAdd("d", 1, F_StartingDateUbound.Text))
            Conn = " AND "
        End If
        If Not F_EndingDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "EndingDate >= " & DB.Quote(F_EndingDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_EndingDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "EndingDate < " & DB.Quote(DateAdd("d", 1, F_EndingDateUbound.Text))
            Conn = " AND "
        End If
        If Not F_SKU.Text = String.Empty Then
            SQL &= Conn & "EXISTS (select top 1 i.itemid from mixmatchline inner join storeitem i on mixmatchline.itemid = i.itemid where mixmatchid = m.id and i.sku like " & DB.FilterQuote(F_SKU.Text) & ")"
            Conn = " AND "
        End If
        hidCon.Value = SQL

    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " m.*, c.customerpricegroupcode "
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        '' Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY  " & IIf(gvList.SortByAndOrder.Contains("StartingDate") = False, " StartingDate desc", gvList.SortByAndOrder))
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)

        gvList.DataSource = res.DefaultView
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
   
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Type As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("Type")
            Dim ltrType As Literal = CType(e.Row.FindControl("ltrType"), Literal)
            If Type = Utility.Common.MixmatchType.ProductCoupon Then
                ltrType.Text = String.Format("<img src=""/includes/theme-admin/images/icon-private.png"" title=""Product Coupon"" />")
            Else
                ltrType.Text = String.Format("<img src=""/includes/theme-admin/images/icon-public.png"" title=""Public"" />")
            End If
        End If
    End Sub
End Class

