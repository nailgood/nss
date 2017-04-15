Imports DataLayer
Imports Components
Imports System.IO
Imports Utility.Common

Partial Class controls_Review
    Inherits ModuleControl
    Private m_Type As Integer
    Private m_ItemReviewId As Integer
    Private TotalRecords As Integer = 1

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Public Property Type() As ReviewType
        Get
            Return m_Type
        End Get
        Set(ByVal value As ReviewType)
            m_Type = value
        End Set
    End Property

    Public Property ItemReviewId() As Integer
        Get
            Return m_ItemReviewId
        End Get
        Set(ByVal value As Integer)
            m_ItemReviewId = value
        End Set
    End Property

    Private Sub InitPager()
        pagerBottom.ShowTwoLine = False
        pagerBottom.PageSize = 4
        pagerBottom.PageIndex = 1
        pagerBottom.ShowViewAll = True
        pagerBottom.ShowPageSize = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            InitPager()

            If ItemReviewId > 0 And Type > 0 Then
                BindData()

                Dim sp As New SitePage
                If Session("MemberId") Is Nothing Then
                    litWriteReview.Text = "<a href=""/members/login.aspx"">Login to write a review</a>"
                Else
                    litWriteReview.Text = "<a href=""#"" onclick=""Review('" & ItemReviewId.ToString() & "','0','" & Type & "');"">Write a review</a>"
                End If
            End If
        End If

    End Sub

    Private Sub BindData()
        Dim dt As DataTable = ReviewRow.GetListByItemReviewId(Type, ItemReviewId, "CreatedDate", "Desc", pagerBottom.PageIndex, pagerBottom.PageSize, TotalRecords)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            rptCommnents.DataSource = dt
            rptCommnents.DataBind()
            pagerBottom.SetPaging(dt.Rows.Count, TotalRecords)
        Else
            pagerBottom.Visible = False
        End If
        
        ltrReviewCount.Text = ReviewRow.GetTotalReviewByItemReviewId(Type, ItemReviewId) & " Review"
        If TotalRecords > 1 Then
            ltrReviewCount.Text &= "s"
        End If

    End Sub

    Protected WithEvents rptReplyCommnents As Repeater
#Region "Comment"
    Protected Sub rptCommnents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCommnents.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbCustName As Label = CType(e.Item.FindControl("lbCustName"), Label)
            Dim lbPostDate As Label = CType(e.Item.FindControl("lbPostDate"), Label)
            Dim ltrComment As Literal = CType(e.Item.FindControl("ltrComment"), Literal)
            Dim ltrCountLike As Literal = CType(e.Item.FindControl("ltrCountLike"), Literal)
            Dim ltrCountUnLike As Literal = CType(e.Item.FindControl("ltrCountUnLike"), Literal)

            lbCustName.Text = e.Item.DataItem("CustName").ToString()
            lbPostDate.Text = ConvertDateString(Convert.ToDateTime(e.Item.DataItem("CreatedDate")))
            ltrComment.Text = Server.HtmlEncode(e.Item.DataItem("Comment").ToString())
            Dim countlike As Integer = Convert.ToInt32(e.Item.DataItem("TotalLike"))
            Dim countUnlike As Integer = Convert.ToInt32(e.Item.DataItem("TotalUnLike"))
            ltrCountLike.Text = IIf(countlike > 0, countlike, 0)
            ltrCountUnLike.Text = IIf(countUnlike > 0, countUnlike, 0)
            Dim ReviewId As Integer = Convert.ToInt32(e.Item.DataItem("ReviewId"))

            Dim listChild As DataTable = ReviewRow.GetListChildByParentReviewId(ReviewId)
            If Not listChild Is Nothing AndAlso listChild.Rows.Count > 0 Then
                rptReplyCommnents = CType(e.Item.FindControl("rptReplyCommnents"), Repeater)
                rptReplyCommnents.DataSource = listChild
                rptReplyCommnents.DataBind()
            End If
            Dim imgLike As ImageButton = CType(e.Item.FindControl("imgLike"), ImageButton)
            Dim imgUnLike As ImageButton = CType(e.Item.FindControl("imgUnLike"), ImageButton)
            imgLike.CommandArgument = ReviewId
            imgUnLike.CommandArgument = ReviewId
            Dim litReply As Literal = CType(e.Item.FindControl("litReply"), Literal)
            litReply.Text = String.Format("<a href=""#"" onclick=""Review('{0}','{1}','{2}');"">Reply</a>", ItemReviewId, ReviewId, CInt(Type))

        End If
    End Sub

    Protected Sub rptCommnents_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptCommnents.ItemCommand
        If Not Session("MemberId") Is Nothing Then
            If (e.CommandName = "like") Then
                LikeReviewRow.UpdateLike(DB, e.CommandArgument, Session("MemberId"))
                BindData()
            ElseIf (e.CommandName = "unlike") Then
                LikeReviewRow.UpdateUnLike(DB, e.CommandArgument, Session("MemberId"))
                BindData()
            End If
        Else
            Response.Redirect("/members/login.aspx")
        End If
    End Sub
#End Region

#Region "Child Comment"
    Protected Sub rptReplyCommnents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptReplyCommnents.ItemDataBound
        Dim ParentReviewId As Integer = 0
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbReplyCustName As Label = CType(e.Item.FindControl("lbReplyCustName"), Label)
            Dim lbReplyPostDate As Label = CType(e.Item.FindControl("lbReplyPostDate"), Label)
            Dim ltrReplyComment As Literal = CType(e.Item.FindControl("ltrReplyComment"), Literal)
            Dim ltrReplyCountLike As Literal = CType(e.Item.FindControl("ltrReplyCountLike"), Literal)
            Dim ltrReplyCountUnLike As Literal = CType(e.Item.FindControl("ltrReplyCountUnLike"), Literal)

            lbReplyCustName.Text = e.Item.DataItem("CustName").ToString()
            lbReplyPostDate.Text = ConvertDateString(Convert.ToDateTime(e.Item.DataItem("CreatedDate")))
            ltrReplyComment.Text = Server.HtmlEncode(e.Item.DataItem("Comment").ToString())
            Dim countlike As Integer = Convert.ToInt32(e.Item.DataItem("TotalLike"))
            Dim countUnlike As Integer = Convert.ToInt32(e.Item.DataItem("TotalUnLike"))
            ltrReplyCountLike.Text = IIf(countlike > 0, countlike, 0)
            ltrReplyCountUnLike.Text = IIf(countUnlike > 0, countUnlike, 0)
            ParentReviewId = Convert.ToInt32(e.Item.DataItem("ParentReviewId"))

            Dim ReviewId As Integer = Convert.ToInt32(e.Item.DataItem("ReviewId"))
            Dim imgLike As ImageButton = CType(e.Item.FindControl("imgLike"), ImageButton)
            Dim imgUnLike As ImageButton = CType(e.Item.FindControl("imgUnLike"), ImageButton)
            imgLike.CommandArgument = ReviewId
            imgUnLike.CommandArgument = ReviewId
            Dim litReplyComment As Literal = CType(e.Item.FindControl("litReplyComment"), Literal)
            litReplyComment.Text = String.Format("<a href=""#"" onclick=""Review('{0}','{1}','{2}');"">Reply</a>", ItemReviewId, ReviewId, CInt(Type))
        End If
    End Sub

    Protected Sub rptReplyCommnents_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptReplyCommnents.ItemCommand
        If Not Session("MemberId") Is Nothing Then
            If (e.CommandName = "like") Then
                LikeReviewRow.UpdateLike(DB, e.CommandArgument, Session("MemberId"))
                BindData()
            ElseIf (e.CommandName = "unlike") Then
                LikeReviewRow.UpdateUnLike(DB, e.CommandArgument, Session("MemberId"))
                BindData()
            End If
        Else
            Response.Redirect("/members/login.aspx")
        End If
    End Sub

#End Region

    Protected Sub pagerBottom_PageIndexChanging(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
        pagerBottom.PageIndex = e.PageIndex
        BindData()
    End Sub

    Protected Sub pagerBottom_PageSizeChanging(ByVal obj As Object, ByVal e As PageSizeChangeEventArgs)
        pagerBottom.PageSize = e.PageSize
        pagerBottom.PageIndex = 1
        BindData()
    End Sub

    Private Function ConvertDateString(ByVal dt As DateTime) As String
        Dim result As String = String.Empty
        Dim ts As TimeSpan = DateTime.Now.Subtract(dt)
        Dim d As Integer = ts.Days
        Dim m As Integer = d / 30
        Dim y As Integer = d / 30 / 12
        If y > 0 Then
            If y > 1 Then
                result = y & " years ago"
            Else
                result = y & " year ago"
            End If
        ElseIf m > 0 Then
            If m > 1 Then
                result = m & " months ago"
            Else
                result = m & " month ago"
            End If
        ElseIf d > 0 Then
            If d > 1 Then
                result = d & " days ago"
            Else
                result = d & " day ago"
            End If
        Else
            If ts.Hours > 0 Then
                If ts.Hours > 1 Then
                    result = ts.Hours & " hours ago"
                Else
                    result = ts.Hours & " hour ago"
                End If
            Else
                If ts.Minutes > 1 Then
                    result = ts.Minutes & " minutes ago"
                Else
                    result = "0 minute ago"
                End If
            End If
        End If
        Return result
    End Function

End Class
