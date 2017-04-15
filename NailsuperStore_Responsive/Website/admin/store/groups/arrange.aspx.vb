Imports Components
Imports DataLayer

Partial Class admin_store_groups_arrange
    Inherits AdminPage

    Protected ItemGroupdId As Integer
    Protected TotalRecords As Integer
    Protected params As String

    Protected Sub Page_Load1(sender As Object, e As System.EventArgs) Handles Me.Load
        params = GetPageParams(FilterFieldType.All)
        ItemGroupdId = Request("ItemGroupId")
        If Not IsPostBack Then
            'gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            'gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "[SortOrder]"
                gvList.SortOrder = "ASC"
            End If
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String

        'ViewState("F_SortBy") = gvList.SortBy
        'ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " op1.OptionId, op2.OptionName, op1.SortOrder "
        SQL = " FROM StoreItemGroupOptionRel op1 join StoreItemGroupOption op2 on op1.OptionId = op2.OptionId  WHERE op1.ItemGroupId = " & ItemGroupdId

        TotalRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = TotalRecords

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub



    Protected Sub gvList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand

        Try
            If e.CommandName = "Down" Then
                Dim OptionId As Integer = Convert.ToInt32(e.CommandArgument)
                StoreItemGroupRow.ChangeArrangeStoreItemGroupOptionRel(ItemGroupdId, OptionId, e.CommandName)
            ElseIf (e.CommandName = "Up") Then
                Dim OptionId As Integer = Convert.ToInt32(e.CommandArgument)
                StoreItemGroupRow.ChangeArrangeStoreItemGroupOptionRel(ItemGroupdId, OptionId, e.CommandName)
            End If
        Catch ex As Exception

        End Try

        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            If (TotalRecords > 1) Then
                If e.Row.DataItemIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.DataItemIndex = TotalRecords - 1 Then
                    imbDown.Visible = False
                End If
            Else
                imbUp.Visible = False
                imbDown.Visible = False
            End If
        End If
    End Sub
End Class
