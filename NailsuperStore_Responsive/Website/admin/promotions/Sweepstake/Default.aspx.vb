Imports Components
Imports DataLayer
Partial Class admin_promotions_Sweepstake_Default
    Inherits AdminPage
    Dim ToTal As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_DrawingDate.Text = Request("F_DrawingDate")
            F_Name.Text = Request("F_Name")
            F_IsActive.Text = Request("F_IsActive")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "DrawingDate"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            btnSearch_Click(sender, e)
            'BindList()
        End If
    End Sub
  
    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM Sweepstake "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & " Name LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & " IsActive  = " & F_IsActive.SelectedValue
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT * FROM (SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " SweepstakeId,Name,Result,DrawingDate,IsActive"
        ToTal = DB.ExecuteScalar("SELECT COUNT(DISTINCT SweepstakeId) " & hidCon.Value)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortBy & " DESC )tbltmp")
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

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            Dim SweepstakeId As Integer = e.Row.DataItem("SweepstakeId")
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
       If e.CommandName = "Delete" Then
            Dim objVideo As VideoRow = VideoRow.GetRow(DB, e.CommandArgument)
            SweepstakeRow.Delete(DB, e.CommandArgument)
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectId = objVideo.VideoId
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(objVideo, Utility.Common.ObjectType.Video)
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        ElseIf e.CommandName = "Active" Then
            Dim row As SweepstakeRow = SweepstakeRow.GetRow(DB, e.CommandArgument)
            With row
                SweepstakeRow.ChangeIsActive(DB, .SweepstakeId)
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logDetail.ObjectId = .SweepstakeId
                logDetail.Message = "IsActive|" & .IsActive.ToString() & "|" & (Not .IsActive).ToString() & "[br]"
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End With


        End If
        Response.Redirect("Default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class



