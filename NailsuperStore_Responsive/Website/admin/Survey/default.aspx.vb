Imports Components
Imports DataLayer

Partial Class admin_Survey_default
    Inherits AdminPage
    Protected IsShowOrder As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_DateLbound.Text = Request("F_DateLBound")
            F_DateUbound.Text = Request("F_DateUBound")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "Id"
                gvList.SortOrder = "DESC"
            End If
            LoadSurveyCode()
            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM SurveyResult sr  left join Member mb on(mb.MemberId=sr.MemberId)  "
        SQL = SQL & "left join Customer cus on (cus.CustomerId=mb.CustomerId) left join StoreOrder as so on (sr.OrderId = so.OrderId) "
        SQL = SQL & "left join CashPointSurveyResult csr on sr.Id = csr.SurveyResultId left join CashPoint cp on csr.CashPointId = cp.CashPointId"
        If Not String.IsNullOrEmpty(F_txtCustomerNo.Text) Then
            SQL = SQL & Conn & "cus.CustomerNo ='" & F_txtCustomerNo.Text.Trim & "' "
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(F_txtName.Text) Then
            SQL = SQL & Conn & "sr.CustomerName like " & DB.FilterQuote(F_txtName.Text.Trim())
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(F_txtEmail.Text) Then
            SQL = SQL & Conn & "sr.CustomerEmail like " & DB.FilterQuote(F_txtEmail.Text.Trim())
            Conn = " AND "
        End If
        If Not F_DateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "sr.CreatedDate >= " & DB.Quote(F_DateLbound.Text)
            Conn = " AND "
        End If
        If Not F_DateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "sr.CreatedDate < " & DB.Quote(DateAdd("d", 1, F_DateUbound.Text))
            Conn = " AND "
        End If
        If (Not String.IsNullOrEmpty(drpSurveyCode.SelectedValue) AndAlso drpSurveyCode.SelectedValue > 0) Then
            SQL = SQL & Conn & "sr.SurveyId = " & drpSurveyCode.SelectedValue
            Conn = " AND "
            If drpSurveyCode.SelectedValue = 1 Then
                gvList.Columns(0).Visible = True
                ltrTitle.Text = "Order Survey"
            Else
                gvList.Columns(0).Visible = False
                ltrTitle.Text = "Website Survey"
            End If
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sr.*, cus.CustomerNo,so.OrderNo, cp.PointEarned"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()

    End Sub
    Private Sub LoadSurveyCode()
        Dim dt As DataTable = SurveyRow.GetList()
        drpSurveyCode.DataSource = dt
        drpSurveyCode.DataTextField = "Code"
        drpSurveyCode.DataValueField = "Id"
        drpSurveyCode.DataBind()
        drpSurveyCode.Items.Insert(0, (New ListItem("--- ALL ---", "")))
        drpSurveyCode.SelectedValue = Request("F_SurveyId")
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub
        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "AddPoint" Then
            CashPointRow.AddPointSurveyResult(DB, e.CommandArgument)
            BindList()
        End If
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ltrOrder As Literal = CType(e.Row.FindControl("ltrOrder"), Literal)
            Dim ltrPoint As Literal = CType(e.Row.FindControl("ltrPoint"), Literal)
            Dim imbPoint As ImageButton = CType(e.Row.FindControl("imbPoint"), ImageButton)
            If Not String.IsNullOrEmpty(e.Row.DataItem("OrderId").ToString()) AndAlso CInt(e.Row.DataItem("OrderId")) > 0 Then
                If Not String.IsNullOrEmpty(e.Row.DataItem("OrderNo").ToString()) Then
                    ltrOrder.Text = "<a target=""_blank"" href=""/admin/store/orders/edit.aspx?OrderId=" & e.Row.DataItem("OrderId") & """>" & e.Row.DataItem("OrderNo") & "</a>"
                Else
                    ltrOrder.Text = "<a target=""_blank"" href=""/admin/store/CartDetail/edit.aspx?OrderId=" & e.Row.DataItem("OrderId") & """>" & e.Row.DataItem("OrderId") & "</a>"
                End If
            End If
            If IsDBNull(e.Row.DataItem("PointEarned")) Then
                ltrPoint.Visible = False
                imbPoint.Visible = True
            Else
                ltrPoint.Visible = True
                imbPoint.Visible = False
                ltrPoint.Text = e.Row.DataItem("PointEarned").ToString()
            End If
            '' e.Row.Cells(0).TemplateControl.Visible = False
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("default.aspx?F_Surveyid=" & Request("F_SurveyId"))
    End Sub
End Class
