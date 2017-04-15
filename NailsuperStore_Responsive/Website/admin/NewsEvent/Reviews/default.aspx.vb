Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_NewsEvent_Reviews_default
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            LoadCategory()
            F_Status.SelectedValue = IIf(Request("F_Status") = Nothing, "", Request("F_Status"))
            F_DateLbound.Text = Request("F_DateLbound")
            F_DateRbound.Text = Request("F_DateRbound")
            F_NewsCategory.SelectedValue = Request("F_NewsCategory")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            btnSearch_Click(sender, e)
        End If

    End Sub

    Private Sub LoadCategory()
        Dim collection As CategoryCollection = CategoryRow.ListByType(DB, Utility.Common.CategoryType.News)
        F_NewsCategory.DataSource = collection
        F_NewsCategory.DataTextField = "CategoryName"
        F_NewsCategory.DataValueField = "CategoryId"
        F_NewsCategory.DataBind()
        F_NewsCategory.Items.Insert(0, New ListItem("--All--", String.Empty))
    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        SQL = " FROM Review rv inner join News n on rv.ItemReviewId=n.NewsId left join Member m on m.MemberId = rv.MemberId left join Customer cus on (cus.CustomerId=m.CustomerId) LEFT JOIN newscategory nc ON n.NewsId = nc.NewsId where 1=1 and Type=" & Utility.Common.ReviewType.News
        If Not F_Status.SelectedValue = String.Empty Then
            If F_Status.SelectedValue = "0" Then
                SQL = SQL & " and rv.IsActive=0"
            ElseIf F_Status.SelectedValue = "1" Then
                SQL = SQL & " and rv.IsActive=1"
            ElseIf F_Status.SelectedValue = "2" Then
                SQL = SQL & " and [dbo].[fc_Review_IsAddePoint](ReviewId)=1"
            End If
        End If
        If Not F_NewsCategory.SelectedValue = String.Empty Then
            SQL = SQL & " and nc.CategoryId=" & F_NewsCategory.SelectedValue
        End If
        If Not F_DateLbound.Text = String.Empty Then
            SQL = SQL & " and rv.CreatedDate >=" & DB.Quote(F_DateLbound.Text)
        End If
        If Not F_DateRbound.Text = String.Empty Then
            SQL = SQL & " and rv.CreatedDate <" & DB.Quote(F_DateRbound.Text)
        End If
        SQL = SQL & " and (ParentReviewId is null or ParentReviewId<1)"
        hidCon.Value = SQL

    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " rv.*, [dbo].[fc_Review_GetPointAdded](ReviewId) as PointAdded,[dbo].[fc_Review_GetTotalLike](ReviewId) as TotalLike,[dbo].[fc_Review_GetTotalReply](ReviewId) as TotalReply, n.NewsId,n.Title,m.Username,cus.Name,cus.Name2"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("CreatedDate") = False, " rv.CreatedDate desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        BindQuery()
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
            Dim item As ReviewRow = ReviewRow.GetRow(DB, id)
            Dim logDetail As New AdminLogDetailRow
            If item Is Nothing Or item.ReviewId < 1 Then
                Exit Sub
            End If
            logDetail.Message = "IsActive|" & item.IsActive.ToString() & "|" & (Not item.IsActive).ToString() & "[br]"
            If item.IsActive = "True" Then
                item.IsActive = "False"
            Else
                item.IsActive = "True"
            End If
            Dim userLogin As AdminPrincipal = Context.User
            item.ModifiedBy = userLogin.Username
            item.ModifiedDate = Date.Now
            ReviewRow.Update(DB, item)
            logDetail.ObjectId = item.ItemReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.NewsReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            'Dim resultAddPoint As Boolean = CashPointRow.AddPointReview(DB, id)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AddPoint(ByVal id As Integer)

    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
        Dim ltrNewsName As Literal = e.Row.FindControl("ltrNewsName")
        Dim ltrComment As Literal = e.Row.FindControl("ltrComment")
        Dim ltrLike As Literal = e.Row.FindControl("ltrLike")
        Dim ltrReply As Literal = e.Row.FindControl("ltrReply")
        ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'>" & e.Row.DataItem("Name") & e.Row.DataItem("Name2") & "</a>"
        ltrNewsName.Text = "<a href='/admin/newsevent/news/default.aspx?NewsId=" & e.Row.DataItem("NewsId") & "&Type=2'>" & e.Row.DataItem("Title") & "</a>"
        ltrComment.Text = Server.HtmlEncode(e.Row.DataItem("Comment").ToString()).Replace("\n", "<br/>").Replace(Environment.NewLine, "<br/>")
        ltrLike.Text = e.Row.DataItem("TotalLike")
        ltrReply.Text = "<a href='reply.aspx?reviewid=" & e.Row.DataItem("ReviewId") & "& " & GetPageParams(Components.FilterFieldType.All) & "'>" & e.Row.DataItem("TotalReply") & "</a>"
        Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
        Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
        If active = "True" Then
            imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If

    End Sub
End Class
