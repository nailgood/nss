

Imports DataLayer
Imports Components
Partial Class members_referfriend_manager
    Inherits SitePage
    Public countHistory As Integer = 0
    Private PointReferFriend As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        PointReferFriend = SysParam.GetValue("PointReferFriend")
        If Not Page.IsPostBack Then
            Dim memberId As Integer = Convert.ToInt32(Session("memberId"))
            If (memberId < 1) Then
                Response.Redirect("/members/login.aspx")
            End If
            Dim dbMember As MemberRow = MemberRow.GetRow(memberId)
            If (String.IsNullOrEmpty(dbMember.ReferCode)) Then
                ltrReferCode.Text = dbMember.Customer.CustomerNo
                DB.ExecuteSQL("Update Member set ReferCode='" & ltrReferCode.Text & "' where MemberId=" & memberId)
            Else
                ltrReferCode.Text = dbMember.ReferCode
            End If
            ltrReferPoint.Text = MemberReferRow.GetTotalPointReferFriend(DB, dbMember.MemberId)
            LoadReferHistory(dbMember.MemberId)
            barstatus1.LoadData("<a href=""/members/referfriend/refer.aspx"">Invite a friend</a>")
            'ltrAwardReferPoint.Text = PointReferFriend
        End If
    End Sub
    Private Sub LoadReferHistory(ByVal memberId As Integer)
        countHistory = 0
        ulHistory.Visible = False
        rptReferHistory.Visible = False
        Dim lstHistory As MemberReferCollection = MemberReferRow.GetHistoryReferFriend(memberId)
        If (lstHistory Is Nothing) Then
            Exit Sub
        End If
        countHistory = lstHistory.Count()
        If (countHistory < 1) Then
            Exit Sub
        End If
        ulHistory.Visible = True
        rptReferHistory.Visible = True
        rptReferHistory.DataSource = lstHistory
        rptReferHistory.DataBind()
    End Sub
    Protected Sub rptReferHistory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptReferHistory.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrEmail As Literal = CType(e.Item.FindControl("ltrEmail"), Literal)
            Dim objData As MemberReferRow = e.Item.DataItem
            ltrEmail.Text = objData.Email
            Dim ltrStatus As Literal = CType(e.Item.FindControl("ltrStatus"), Literal)
            Dim sStatus As String = Utility.Common.ConvertReferFriendStatusToString(objData.Status)
            If objData.Status = 5 Then
                ''Dim point As Integer = MemberReferRow.GetPointEarnedByMemberRefered(DB, objData.MemberUseRefer)
                sStatus = String.Format(sStatus, PointReferFriend)
            End If
            ltrStatus.Text = sStatus

        End If

    End Sub

End Class
