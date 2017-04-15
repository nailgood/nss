Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_shopdesign_reviews_default
    Inherits AdminPage
    Dim logdetail As New AdminLogDetailRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadCategory()
            BindQuery()
            BindData()
        End If
    End Sub
    Private Sub LoadCategory()
        Dim lstcate As CategoryCollection = CategoryRow.ListByType(DB, Utility.Common.CategoryType.ShopDesign)
        If Not lstcate Is Nothing Then
            F_ShopDesignCategory.DataTextField = "CategoryName"
            F_ShopDesignCategory.DataValueField = "CategoryId"
            F_ShopDesignCategory.DataSource = lstcate
            F_ShopDesignCategory.DataBind()
            F_ShopDesignCategory.Items.Insert(0, New ListItem("-- ALL --", ""))
        End If
    End Sub
    Private Sub BindQuery()
        Dim SQL As String = ""
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        SQL = " FROM Review r JOIN ShopDesign s ON s.ShopDesignId = r.ItemReviewId left join Member m on m.MemberId = r.MemberId left join Customer cus on (cus.CustomerId=m.CustomerId)"
        If F_ShopDesignCategory.SelectedValue <> String.Empty Then
            SQL &= " LEFT JOIN shopdesigncategory sc ON s.ShopDesignId = sc.ShopDesignId  where 1=1 and Type=" & Utility.Common.ReviewType.ShopDesign & " AND sc.CategoryId= " & F_ShopDesignCategory.SelectedValue
        Else
            SQL &= "where 1=1 and Type=" & Utility.Common.ReviewType.ShopDesign
        End If
        If F_Status.SelectedValue <> String.Empty Then
            If F_Status.SelectedValue = "0" Then
                SQL &= " AND r.IsActive =0"
            ElseIf F_Status.SelectedValue = "1" Then
                SQL &= " AND r.IsActive =1"
            Else
                SQL &= " AND [dbo].[fc_Review_IsAddePoint](ReviewId)=1"
            End If
        End If
        If F_DateLbound.Text <> String.Empty Then
            SQL &= " AND r.CreatedDate >= " & DB.Quote(F_DateLbound.Text)
        End If
        If F_DateRbound.Text <> String.Empty Then
            SQL &= " AND r.CreatedDate <= " & DB.Quote(F_DateRbound.Text)
        End If
        SQL = SQL & " and (ParentReviewId is null or ParentReviewId<1)"
        hidCon.Value = SQL
    End Sub
    Private Sub BindData()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " r.*, [dbo].[fc_Review_GetPointAdded](ReviewId) as PointAdded,[dbo].[fc_Review_GetTotalLike](ReviewId) as TotalLike,[dbo].[fc_Review_GetTotalReply](ReviewId) as TotalReply, s.ShopDesignId,s.Title,m.Username,cus.Name,cus.Name2"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("CreatedDate") = False, " r.CreatedDate desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        BindQuery()
        BindData()
    End Sub

    Protected Sub AddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not e.Row.RowType = DataControlRowType.DataRow Then
            Exit Sub
        End If
        Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
        Dim ltrShopDesign As Literal = e.Row.FindControl("ltrShopDesign")
        Dim ltrComment As Literal = e.Row.FindControl("ltrComment")
        Dim ltrLike As Literal = e.Row.FindControl("ltrLike")
        Dim ltrReply As Literal = e.Row.FindControl("ltrReply")
        ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'>" & e.Row.DataItem("Name") & e.Row.DataItem("Name2") & "</a>"
        ltrShopDesign.Text = "<a href='/admin/shopdesign/edit.aspx?ShopDesignId=" & e.Row.DataItem("ShopDesignId") & "&Type=2'>" & e.Row.DataItem("Title") & "</a>"
        ltrComment.Text = Server.HtmlEncode(e.Row.DataItem("Comment").ToString()).Replace("\n", "<br/>").Replace(Environment.NewLine, "<br/>")
        ltrLike.Text = e.Row.DataItem("TotalLike")
        ltrReply.Text = "<a href='reply.aspx?ReviewId=" & e.Row.DataItem("ReviewId") & "& " & GetPageParams(Components.FilterFieldType.All) & "'>" & e.Row.DataItem("TotalReply") & "</a>"
        Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
        Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
        If active = "True" Then
            imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Active" Then
            Dim review As ReviewRow = ReviewRow.GetRow(DB, e.CommandArgument)
            If review Is Nothing Or review.ReviewId < 1 Then
                Exit Sub
            End If
            logdetail.Message = "IsActive|" & review.IsActive.ToString() & "|" & (Not review.IsActive).ToString() & "[br]"
            If review.IsActive Then
                review.IsActive = False
            Else
                review.IsActive = True
            End If
            review.ModifiedDate = Date.Now
            review.ModifiedBy = User.Identity.Name
            ReviewRow.Update(DB, review)
            logdetail.Action = Utility.Common.AdminLogAction.Update
            logdetail.ObjectId = review.ItemReviewId
        ElseIf e.CommandName = "Delete" Then
            Dim review As ReviewRow = ReviewRow.GetRow(DB, e.CommandArgument)
            ReviewRow.Delete(DB, e.CommandArgument)
            logdetail.Action = Utility.Common.AdminLogAction.Delete
            logdetail.ObjectId = review.ItemReviewId
            logdetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(review, Utility.Common.ObjectType.ShopDesignReview)
        End If
        logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignReview
        AdminLogHelper.WriteLuceneLogDetail(logdetail)
        BindData()
    End Sub

    Protected Sub gvList_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvList.RowDeleting

    End Sub
End Class
