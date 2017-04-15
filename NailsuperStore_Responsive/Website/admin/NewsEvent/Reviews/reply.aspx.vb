Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_NewsEvent_Reviews_reply
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            If Not Request.QueryString("reviewid") Is Nothing Then
                ReviewId = Request.QueryString("reviewid")
            End If
            If ReviewId < 1 Then
                Response.Redirect("default.aspx")
            End If
            LoadNewsReview()
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ReviewId"
            BindList()
        End If
    End Sub

    Private Sub LoadNewsReview()
         Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
            If review Is Nothing Then
                Response.Redirect("default.aspx")
            End If
        lblComment.Text = review.Comment
        Dim news As NewsRow = NewsRow.GetRow(DB, review.ItemReviewId)
        If Not news Is Nothing Then
            lblNews.Text = "<a href='/admin/newsevent/news/default.aspx?NewsId=" & news.NewsId & "&Type=2'>" & news.Title & "</a>"
        End If
        'LoadNews(review.ItemReviewId)
    End Sub

    Private Sub BindList()
        Dim SQL, SQLFields As String
        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " rv.*,[dbo].[fc_Review_GetPointAdded](ReviewId) as PointAdded,[dbo].[fc_Review_GetTotalLike](ReviewId) as TotalLike, mb.Username,cus.Name,cus.Name2"
        SQL = " FROM Review rv  left join Member mb on(mb.MemberId=rv.MemberId)  left join Customer cus on (cus.CustomerId=mb.CustomerId) where 1=1 "
        If Not String.IsNullOrWhiteSpace(txtUserName.Text) Then
            SQL = SQL & " and mb.Username = '" & txtUserName.Text & "'"
        End If
        If Not String.IsNullOrWhiteSpace(txtCustomerNo.Text) Then
            SQL = SQL & " and cus.CustomerNo = " & txtCustomerNo.Text
        End If
        If Not String.IsNullOrWhiteSpace(dtpDateLbound.Text) Then
            SQL = SQL & " and rv.CreatedDate >= " & DB.Quote(dtpDateLbound.Text)
        End If
        If Not String.IsNullOrWhiteSpace(dtpDateRbound.Text) Then
            SQL = SQL & " and rv.CreatedDate < " & DB.Quote(dtpDateRbound.Text)
        End If
        If Not drlStatus.SelectedValue = String.Empty Then
            If drlStatus.SelectedValue = "0" Then
                SQL = SQL & " and rv.IsActive = 0"
            ElseIf drlStatus.SelectedValue = "1" Then
                SQL = SQL & " and rv.IsActive = 1"
            ElseIf drlStatus.SelectedValue = "2" Then
                SQL = SQL & " and [dbo].[fc_Review_IsAddePoint](ReviewId)=1"
            End If
        End If



        SQL = SQL & " and ParentReviewId=" & ReviewId
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("CreatedDate") = False, " rv.CreatedDate desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 1
        BindList()
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
            Dim review As ReviewRow = ReviewRow.GetRow(DB, id)
            Dim logDetail As New AdminLogDetailRow
            If review Is Nothing Or review.ReviewId < 1 Then
                Exit Sub
            End If
            If review Is Nothing Then
                Response.Redirect("default.aspx")
            End If
            logDetail.Message = "IsActive|" & review.IsActive.ToString() & "|" & (Not review.IsActive).ToString() & "[br]"
            If review.IsActive = True Then
                review.IsActive = False
            Else
                review.IsActive = True
            End If
            Dim userLogin As AdminPrincipal = Context.User
            review.ModifiedBy = userLogin.Username
            review.ModifiedDate = Date.Now
            ReviewRow.Update(DB, review)
            logDetail.ObjectId = review.ReviewId
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectType = Utility.Common.ObjectType.NewsReview
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            'Dim resultAddPoint As Boolean = CashPointRow.AddPointReview(DB, id)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AddPoint(ByVal id As Integer)
    End Sub

    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        Response.Redirect(Me.Request.RawUrl)
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
        Dim ltrComment As Literal = e.Row.FindControl("ltrComment")
        Dim ltrLike As Literal = e.Row.FindControl("ltrLike")
        ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'>" & e.Row.DataItem("Name") & e.Row.DataItem("Name2") & "</a>"
        ltrComment.Text = e.Row.DataItem("Comment")
        ltrLike.Text = e.Row.DataItem("TotalLike")
        Dim imbActive As ImageButton = e.Row.FindControl("imbActive")
        If e.Row.DataItem("IsActive") = True Then
            imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
    End Sub
End Class
