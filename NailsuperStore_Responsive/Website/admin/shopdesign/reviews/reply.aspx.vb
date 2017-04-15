Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_shopdesign_reviews_reply
    Inherits AdminPage
    Public ReviewId As Integer
    Dim logdetail As New AdminLogDetailRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReviewId = Request("ReviewId")
        If Not IsPostBack Then
            LoadDetail()
            BindQuery()
            BindData()
        End If
    End Sub
    Private Sub LoadDetail()
        Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
        If review Is Nothing Then
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If
        lblComment.Text = review.Comment
        Dim row As ShopDesignRow = ShopDesignRow.GetRow(DB, review.ItemReviewId)
        If Not row Is Nothing Then
            lblShopDesign.Text = "<a href='/admin/shopdesign/edit.aspx?ShopDesignId=" & row.ShopDesignId & "'>" & row.Title & "</a>"
        End If
    End Sub
    Private Sub BindQuery()
        Dim SQL As String = ""
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        SQL = " FROM Review r JOIN ShopDesign s ON s.ShopDesignId = r.ItemReviewId left join Member m on m.MemberId = r.MemberId left join Customer cus on (cus.CustomerId=m.CustomerId) where 1=1 and Type=" & Utility.Common.ReviewType.ShopDesign
        If txtUserName.Text <> String.Empty Then
            SQL &= " AND m.UserName = " & DB.Quote(txtUserName.Text)
        End If
        If txtCustomerNo.Text <> String.Empty Then
            SQL &= " AND cus.CustomerNo = " & DB.Quote(txtCustomerNo.Text)
        End If
        If drlStatus.SelectedValue <> String.Empty Then
            If drlStatus.SelectedValue = "0" Then
                SQL &= " AND r.IsActive =0"
            ElseIf drlStatus.SelectedValue = "1" Then
                SQL &= " AND r.IsActive =1"
            Else
                SQL &= " AND [dbo].[fc_Review_IsAddePoint](ReviewId)=1"
            End If
        End If
        If dtpDateLbound.Text <> String.Empty Then
            SQL &= " AND r.CreatedDate >= " & DB.Quote(dtpDateLbound.Text)
        End If
        If dtpDateRbound.Text <> String.Empty Then
            SQL &= " AND r.CreatedDate <= " & DB.Quote(dtpDateRbound.Text)
        End If
        SQL = SQL & " and ParentReviewId =" & ReviewId
        hidCon.Value = SQL
    End Sub
    Private Sub BindData()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " r.*, [dbo].[fc_Review_GetPointAdded](ReviewId) as PointAdded,[dbo].[fc_Review_GetTotalLike](ReviewId) as TotalLike, s.ShopDesignId,s.Title,m.Username,cus.Name,cus.Name2"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("CreatedDate") = False, " r.CreatedDate desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 1
        BindQuery()
        BindData()
    End Sub

    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        Response.Redirect("reply.aspx?ReviewId=" & ReviewId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Active" Then
            Dim review As ReviewRow = ReviewRow.GetRow(DB, e.CommandArgument)
            review.ModifiedBy = User.Identity.Name
            review.ModifiedDate = Date.Now
            logdetail.Message = "IsActive|" & review.IsActive.ToString() & "|" & (Not review.IsActive).ToString() & "[br]"
            If review.IsActive Then
                review.IsActive = False
            Else
                review.IsActive = True
            End If
            ReviewRow.Update(DB, review)
            logdetail.ObjectId = review.ItemReviewId
            logdetail.Action = Utility.Common.AdminLogAction.Update
        ElseIf e.CommandName = "Delete" Then
            Dim review As ReviewRow = ReviewRow.GetRow(DB, e.CommandArgument)
            ReviewRow.Delete(DB, e.CommandArgument)
            logdetail.ObjectId = review.ItemReviewId
            logdetail.Action = Utility.Common.AdminLogAction.Delete
            logdetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(review, Utility.Common.ObjectType.ShopDesignReview)
        End If
        logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignReview
        AdminLogHelper.WriteLuceneLogDetail(logdetail)
        BindData()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
            Dim ltrComment As Literal = e.Row.FindControl("ltrComment")
            Dim ltrLike As Literal = e.Row.FindControl("ltrLike")
            Dim imbActive As ImageButton = e.Row.FindControl("imbActive")
            ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'>" & e.Row.DataItem("Username") & "</a>"
            ltrComment.Text = e.Row.DataItem("Comment")
            ltrLike.Text = e.Row.DataItem("TotalLike")
            If e.Row.DataItem("IsActive") Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If
    End Sub
End Class
