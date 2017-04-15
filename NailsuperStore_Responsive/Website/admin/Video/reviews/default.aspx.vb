
Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Video_reviews_default
    Inherits AdminPage


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

       
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            LoadCategory()
            LoadVideo()
            F_Status.SelectedValue = IIf(Request("F_Status") = Nothing, "", Request("F_Status"))
            F_DateLbound.Text = Request("F_DateLbound")
            F_DateRbound.Text = Request("F_DateRbound")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ReviewId"
            F_VideoCategory.SelectedValue = Request("F_VideoCategory")
            'F_Video.SelectedValue = Request("F_Video")

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
    Private Sub LoadCategory()
        Dim collection As CategoryCollection = CategoryRow.ListByType(DB, Utility.Common.CategoryType.Video)
        F_VideoCategory.DataSource = collection
        F_VideoCategory.DataTextField = "CategoryName"
        F_VideoCategory.DataValueField = "CategoryId"
        F_VideoCategory.DataBind()
        F_VideoCategory.Items.Insert(0, New ListItem("-- ALL --", String.Empty))
    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub

        End If
        Dim ltrVideoName As Literal = e.Row.FindControl("ltrVideoName")
        Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
        Dim ltrLike As Literal = e.Row.FindControl("ltrLike")
        Dim ltrReply As Literal = e.Row.FindControl("ltrReply")
        Dim ltrComment As Literal = e.Row.FindControl("ltrComment")
        Dim ltrPoint As Literal = e.Row.FindControl("ltrPoint")
        Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

        Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
        Dim imbPoint As ImageButton = CType(e.Row.FindControl("imbPoint"), ImageButton)


        ltrVideoName.Text = "<a href='/admin/Video/Video/edit.aspx?id=" & e.Row.DataItem("VideoId") & "'> " & e.Row.DataItem("Title") & "</a>"
        ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'> " & e.Row.DataItem("Name") & " " & e.Row.DataItem("Name2") & "</a>"
        ltrLike.Text = e.Row.DataItem("TotalLike")
        ltrReply.Text = "<a href='reply.aspx?reviewid=" & e.Row.DataItem("ReviewId") & "& " & GetPageParams(Components.FilterFieldType.All) & "'>" & e.Row.DataItem("TotalReply") & "</a>"
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

    Private Sub BindQuery()
        Dim s As String = ""

        Dim SQL As String

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM Review rv inner join Video vi on(rv.ItemReviewId = vi.VideoId) LEFT JOIN VideoCategory vc ON vi.VideoId = vc.VideoId left join Member mb on(mb.MemberId=rv.MemberId)  left join Customer cus on (cus.CustomerId=mb.CustomerId) where 1=1 "
        'If Not String.IsNullOrEmpty(F_txtCustomerNo.Text) Then
        '    SQL = SQL & " and CustomerNo ='" & F_txtCustomerNo.Text & "' "
        'End If
        'If Not String.IsNullOrEmpty(F_txtUserName.Text) Then
        '    SQL = SQL & " and mb.Username ='" & F_txtUserName.Text & "' "
        'End If
        'If Not String.IsNullOrEmpty(F_Video.SelectedValue) Then
        '    SQL = SQL & " and rv.ItemReviewId = " & F_Video.SelectedValue
        'End If

        If Not F_Status.SelectedValue = String.Empty Then
            If F_Status.SelectedValue = "0" Then
                SQL = SQL & " and rv.IsActive  =0"
            ElseIf F_Status.SelectedValue = "1" Then
                SQL = SQL & " and rv.IsActive  =1"
            ElseIf F_Status.SelectedValue = "2" Then
                SQL = SQL & " and [dbo].[fc_Review_IsAddePoint](ReviewId)=1"
            End If
        End If
        If Not F_DateLbound.Text = String.Empty Then
            SQL = SQL & " and rv.CreatedDate >= " & DB.Quote(F_DateLbound.Text)
        End If
        If Not F_DateRbound.Text = String.Empty Then
            SQL = SQL & "and rv.CreatedDate < " & DB.Quote(DateAdd("d", 1, F_DateRbound.Text))
        End If
        If Not F_VideoCategory.SelectedValue = String.Empty Then
            SQL = SQL & " and vc.CategoryId=" & F_VideoCategory.SelectedValue
        End If
        SQL = SQL & " and (ParentReviewId is null or ParentReviewId<1)"
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " rv.*,[dbo].[fc_Review_GetPointAdded](ReviewId) as PointAdded,[dbo].[fc_Review_GetTotalLike](ReviewId) as TotalLike,[dbo].[fc_Review_GetTotalReply](ReviewId) as TotalReply,vi.VideoId, vi.Title, mb.Username,cus.Name,cus.Name2"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("CreatedDate") = False, " rv.CreatedDate desc", gvList.SortByAndOrder))
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
            logDetail.ObjectId = sir.ReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.VideoReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AddPoint(ByVal id As Integer)
        Try
            Dim sir As ReviewRow = ReviewRow.GetRow(DB, CInt(id))
            Dim logDetail As New AdminLogDetailRow
            Dim resultAddPoint As Boolean = CashPointRow.AddPointReview(DB, id)
            If (resultAddPoint) Then
                'logDetail.Message &= "AddPoint|False|True[br]"
                'logDetail.ObjectId = sir.ReviewId
                'logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
                'logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                'AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
        Catch ex As Exception

        End Try
    End Sub

  

   
   
    Protected Sub F_VideoCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles F_VideoCategory.SelectedIndexChanged
        LoadVideo()
    End Sub
    Private Sub LoadVideo()
        'F_Video.Items.Clear()
        'Dim objData As New VideoRow
        'Dim lstResult As New VideoCollection
        'objData.OrderBy = "vc.Arrange"
        'objData.OrderDirection = "ASC"
        'objData.PageIndex = 1
        'objData.PageSize = Integer.MaxValue
        'objData.Condition = "vc.CategoryId=" & F_VideoCategory.SelectedValue & " and vi.IsActive=1"
        'objData.Condition = objData.Condition & " and cate.[Type]=2 and [dbo].[VideoInCategoryActive](vi.VideoId)=1"
        'objData.CategoryId = F_VideoCategory.SelectedValue
        'lstResult = VideoRow.ListByCatId(objData)
        'For Each v As VideoRow In lstResult
        '    F_Video.Items.Add(New ListItem(v.Title, v.VideoId))
        'Next
        'F_Video.Items.Insert(0, New ListItem("- - -", ""))
       
    End Sub
End Class