Imports Components
Imports DataLayer

Partial Class admin_LandingPage_Default
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            Dim pi As Integer = 1
            If Not Request("F_PageIndex") = Nothing Then
                pi = Convert.ToInt32(Request("F_PageIndex"))
                If (pi < 1) Then
                    pi = 1
                End If
            End If
            F_Title.Text = Request("F_Title")
            F_StartDate.Text = Request("F_StartDate")
            F_EndDate.Text = Request("F_EndDate")
            F_IsActive.Text = Request("F_IsActive")
            gvList.PageIndex = pi - 1
            If (Not Request("F_SortBy") = Nothing) Then
                gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            End If
            If (Not Request("F_SortOrder") = Nothing) Then
                gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            End If
            BindList()

        End If
        F_Title.Focus()
    End Sub

    Private Sub BindList()
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        ViewState("F_PageIndex") = gvList.PageIndex + 1


        Dim Condition As String = "1=1"
        If (Not F_Title.Text = String.Empty) Then
            Condition = Condition & " and lp.Title like " & DB.FilterQuote(F_Title.Text)
        End If
        If Not F_StartDate.Text = String.Empty Then
            Condition = Condition & " and Cast(lp.StartingDate as Date) >= " & DB.Quote(F_StartDate.Text)
        End If
        If Not F_EndDate.Text = String.Empty Then
            Condition = Condition & " and Cast(lp.EndingDate as Date) <= " & DB.Quote(F_EndDate.Text)
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            Condition = Condition & " and lp.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
        End If

        Dim total As Integer = 0
        Dim dt As DataTable = LandingPageRow.GetList(Condition, gvList.SortBy, gvList.SortOrder, gvList.PageIndex + 1, gvList.PageSize, total)
        gvList.DataSource = dt.DefaultView
        gvList.Pager.NofRecords = total
        gvList.PageSelectIndex = gvList.PageIndex

        gvList.DataBind()

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not Page.IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        Dim sCmdName As String = e.CommandName
        If (sCmdName = "Delete" Or sCmdName = "Active") Then
            If sCmdName = "Delete" Then
                Response.Redirect("delete.aspx?Id=" & e.CommandArgument & "&" & GetPageParams(FilterFieldType.All))
            ElseIf sCmdName = "Active" Then
                Dim obj As LandingPageRow = LandingPageRow.GetRow(DB, e.CommandArgument)
                LandingPageRow.ChangeIsActive(DB, e.CommandArgument)

                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectType = Utility.Common.ObjectType.LandingPage.ToString()
                logDetail.ObjectId = e.CommandArgument
                logDetail.Message = "IsActive|" & obj.IsActive.ToString() & "|" & (Not obj.IsActive).ToString() & "[br]"

                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If

            BindList()
        End If
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
            Dim imbDelete As ImageButton = CType(e.Row.FindControl("imbDelete"), ImageButton)
            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

        End If
    End Sub

    Protected Sub AddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
