Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_CustomerPriceGroupId.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
            F_CustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
            F_CustomerPriceGroupId.DataTextField = "CustomerPriceGroupCode"
            F_CustomerPriceGroupId.DataBind()
            F_CustomerPriceGroupId.Items.Insert(0, New ListItem("-- ALL --", ""))
            F_CustomerPriceGroupId.SelectedValue = Request("F_CustomerPriceGroupId")
            F_PromotionName.Text = Request("F_PromotionName")
            F_PromotionCode.Text = Request("F_PromotionCode")
            F_drpPromotionType.SelectedValue = Request("F_drpPromotionType")
            F_IsActive.Text = Request("F_IsActive")
            F_StartDateLbound.Text = Request("F_StartDateLBound")
            F_StartDateUbound.Text = Request("F_StartDateUBound")
            F_EndDateLbound.Text = Request("F_EndDateLBound")
            F_EndDateUbound.Text = Request("F_EndDateUBound")
            F_isTotalProduct.Text = Request("F_IsTotalProduct")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "StartDate"
                gvList.SortOrder = "DESC"
            End If
            Utility.Common.BindPromotionType(F_drpPromotionType, True)
            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM StorePromotion sp LEFT OUTER JOIN storeitem si on (sp.promotionid = si.promotionid) left outer join customerpricegroup c on sp.customerpricegroupid = c.customerpricegroupid "

        If Not F_PromotionName.Text = String.Empty Then
            SQL = SQL & Conn & "PromotionName LIKE " & DB.FilterQuote(F_PromotionName.Text)
            Conn = " AND "
        End If
        If Not F_PromotionCode.Text = String.Empty Then
            SQL = SQL & Conn & "PromotionCode LIKE " & DB.FilterQuote(F_PromotionCode.Text)
            Conn = " AND "
        End If
        If Not F_drpPromotionType.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "PromotionType = " & DB.Quote(F_drpPromotionType.SelectedValue)
            Conn = " AND "
        End If
        If Not F_CustomerPriceGroupId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sp.CustomerPriceGroupId = " & DB.Quote(F_CustomerPriceGroupId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sp.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsOneUse.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsOneUse  = " & DB.Number(F_IsOneUse.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsUsed.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsUsed  = " & DB.Number(F_IsUsed.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsRegisterSend.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsRegisterSend  = " & DB.Number(F_IsRegisterSend.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsProductCoupon.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsProductCoupon  = " & DB.Number(F_IsProductCoupon.SelectedValue)
            Conn = " AND "
        End If
        If Not F_StartDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartDate >= " & DB.Quote(F_StartDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_StartDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartDate < " & DB.Quote(DateAdd("d", 1, F_StartDateUbound.Text))
            Conn = " AND "
        End If
        If Not F_EndDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "EndDate >= " & DB.Quote(F_EndDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_EndDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "EndDate < " & DB.Quote(DateAdd("d", 1, F_EndDateUbound.Text))
            Conn = " AND "
        End If
        If Not F_isTotalProduct.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsTotalProduct  = " & DB.Number(F_isTotalProduct.SelectedValue)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & hidCon.Value).Rows.Count '' StorePromotionRow.GetListStorePromotion("SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder) '' StorePromotionRow.GetListStorePromotion(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not Page.IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "RelatedItems" Then
            DB.Close()
            Response.Redirect("/admin/store/promotions/specific-items.aspx?PromotionId=" & e.CommandArgument & "&" & GetPageParams())
        End If
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnRelatedItems As Button = CType(e.Row.FindControl("btnRelatedItems"), Button)
            btnRelatedItems.CommandName = "RelatedItems"
            btnRelatedItems.CommandArgument = e.Row.DataItem("PromotionId")

            Dim ltlDiscount As Literal = CType(e.Row.FindControl("ltlDiscount"), Literal)
            If e.Row.DataItem("IsItemSpecific") = True Then btnRelatedItems.Visible = True Else btnRelatedItems.Visible = False
            If e.Row.DataItem("PromotionType") = "Percentage" Then ltlDiscount.Text = FormatPercent(e.Row.DataItem("Discount") / 100) Else ltlDiscount.Text = FormatCurrency(e.Row.DataItem("Discount"))
        End If
    End Sub
End Class
