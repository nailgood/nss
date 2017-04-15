

Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Video_reviews_reply
    Inherits AdminPage

    Public Property ReviewId() As Integer
        Get
            If ViewState("ReviewId") Is Nothing Then
                Return 0
            End If
            Return ViewState("ReviewId")
        End Get
        Set(ByVal value As Integer)
            ViewState("ReviewId") = value
        End Set
    End Property
    Private ItemId As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then


            If Not Request.QueryString("ReviewId") Is Nothing Then
                ReviewId = Request.QueryString("ReviewId")
            End If
            If ReviewId < 1 Then
                Response.Redirect("default.aspx")
            End If
            LoadReview()
            'F_Status.Text = IIf(Request("F_Status") = Nothing, "0", Request("F_Status"))
            'F_DateLbound.Text = Request("F_DateLbound")
            'F_DateRbound.Text = Request("F_DateRbound")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ReviewId"
            BindList()
        End If
    End Sub
    Private Sub LoadReview()
        Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
        If review Is Nothing Then
            Response.Redirect("default.aspx")
        End If
        lblComment.Text = review.Comment.Replace(Environment.NewLine, "<br/>")
        LoadVideo(review.ItemReviewId)
    End Sub


    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub

        End If

        Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
        Dim ltrLike As Literal = e.Row.FindControl("ltrLike")

        Dim ltrComment As Literal = e.Row.FindControl("ltrComment")
        Dim ltrPoint As Literal = e.Row.FindControl("ltrPoint")

        Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

        Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
        Dim imbPoint As ImageButton = CType(e.Row.FindControl("imbPoint"), ImageButton)


        ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'> " & e.Row.DataItem("Name") & " " & e.Row.DataItem("Name2") & "</a>"
        ltrLike.Text = e.Row.DataItem("TotalLike")

        ltrComment.Text = Server.HtmlEncode(e.Row.DataItem("Comment").ToString()).Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>")
        If active Then
            imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If

        If e.Row.DataItem("PointAdded") = 0 Then
            ltrPoint.Visible = False
        Else
            imbPoint.Visible = False
            ltrPoint.Visible = True
            ltrPoint.Text = e.Row.DataItem("PointAdded")
        End If

    End Sub

    Private Sub BindList()

        Dim SQLFields, SQL As String

        'ViewState("F_SortBy") = gvList.SortBy
        'ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " rv.*,[dbo].[fc_Review_GetPointAdded](ReviewId) as PointAdded,[dbo].[fc_Review_GetTotalLike](ReviewId) as TotalLike, mb.Username,cus.Name,cus.Name2"
        SQL = " FROM Review rv  left join Member mb on(mb.MemberId=rv.MemberId)  left join Customer cus on (cus.CustomerId=mb.CustomerId) where 1=1 "
        If Not String.IsNullOrEmpty(txtCustomerNo.Text) Then
            SQL = SQL & " and CustomerNo ='" & txtCustomerNo.Text & "' "
        End If
        If Not String.IsNullOrEmpty(txtUserName.Text) Then
            SQL = SQL & " and mb.Username ='" & txtUserName.Text & "' "
        End If


        If Not drlStatus.SelectedValue = String.Empty Then
            If drlStatus.SelectedValue = "0" Then
                SQL = SQL & " and rv.IsActive  =0"
            ElseIf drlStatus.SelectedValue = "1" Then
                SQL = SQL & " and rv.IsActive  =1"
            ElseIf drlStatus.SelectedValue = "2" Then
                SQL = SQL & " and [dbo].[fc_Review_IsAddePoint](ReviewId)=1"
            End If
        End If
        If Not dtpDateLbound.Text = String.Empty Then
            SQL = SQL & " and rv.CreatedDate >= " & DB.Quote(dtpDateLbound.Text)
        End If
        If Not dtpDateRbound.Text = String.Empty Then
            SQL = SQL & "and rv.CreatedDate < " & DB.Quote(DateAdd("d", 1, dtpDateRbound.Text))
        End If
        SQL = SQL & " and ParentReviewId=" & ReviewId
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("CreatedDate") = False, " rv.CreatedDate desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnreset.Click
        Response.Redirect(Me.Request.RawUrl)
    End Sub
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Active" Then
            ChangeActive(e.CommandArgument)

        ElseIf e.CommandName = "AddPoint" Then
            AddPoint(e.CommandArgument)
        End If

        BindList()
    End Sub
    Private Sub ChangeActive(ByVal id As Integer)
        Try
            Dim sir As ReviewRow = ReviewRow.GetRow(DB, CInt(id))
            Dim logDetail As New AdminLogDetailRow
            If sir Is Nothing Then
                Exit Sub
            End If
            If sir.ReviewId < 0 Then
                Exit Sub
            End If
            logDetail.Message = "IsActive|" & sir.IsActive.ToString() & "|" & (Not sir.IsActive).ToString() & "[br]"
            If sir.IsActive = True Then
                sir.IsActive = False
            Else
                sir.IsActive = True
            End If
            Dim userLogin As AdminPrincipal = Context.User
            sir.ModifiedBy = userLogin.Username
            sir.ModifiedDate = Now
            ReviewRow.Update(DB, sir)

            Dim resultAddPoint As Boolean = CashPointRow.AddPointReview(DB, id)
            'If (resultAddPoint) Then
            '    logDetail.Message &= "AddPoint|False|True[br]"
            'End If
            'logDetail.ObjectId = sir.ReviewId
            'logDetail.ObjectType = Utility.Common.ObjectType.VideoReview.ToString()
            'logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            'AdminLogHelper.WriteLuceneLogDetail(logDetail)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AddPoint(ByVal id As Integer)
        Try
            Dim sir As ReviewRow = ReviewRow.GetRow(DB, CInt(id))
            Dim logDetail As New AdminLogDetailRow
            Dim resultAddPoint As Boolean = CashPointRow.AddPointReview(DB, id)
            'If (resultAddPoint) Then
            '    logDetail.Message &= "AddPoint|False|True[br]"
            '    logDetail.ObjectId = sir.ReviewId
            '    logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            '    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            '    AdminLogHelper.WriteLuceneLogDetail(logDetail)
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoadVideo(ByVal videoId As Integer)
        Dim video As VideoRow = VideoRow.GetRow(DB, videoId)
        If video Is Nothing Then
            Exit Sub
        End If
        lblVideo.Text = "<a href='/admin/Video/Video/edit.aspx?id=" & videoId & "'>" & video.Title & "</a>"

    End Sub
End Class