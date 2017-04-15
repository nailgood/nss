Imports Components
Imports DataLayer

Partial Class admin_members_Refer
    Inherits AdminPage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        gvList.BindList = AddressOf BindList
        If (Not Page.IsPostBack) Then
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreatedDate"
                gvList.SortOrder = "desc"
            End If
            'BindList()
            btnSearch_Click(sender, e)
            BindEnumReferStatus()
            BindEnumReferSource()
        End If
    End Sub

    Private Sub BindQuery()
        Dim condition As String = " 1=1 "

        If Not String.IsNullOrEmpty(F_MemberRefer.Text) Then
            condition &= " AND m.Username like " & DB.FilterQuote(F_MemberRefer.Text)
        End If

        If Not String.IsNullOrEmpty(F_ReferCode.Text) Then
            condition &= " AND m.ReferCode like " & DB.FilterQuote(F_ReferCode.Text)
        End If

        If Not String.IsNullOrEmpty(F_Username.Text) Then
            condition &= " AND m2.Username like " & DB.FilterQuote(F_Username.Text)
        End If
        If Not String.IsNullOrEmpty(F_Email.Text) Then
            condition &= " AND mr.Email like " & DB.FilterQuote(F_Email.Text)
        End If
        If Not String.IsNullOrEmpty(F_CreatedDate.Text) Then
            Try
                Dim CreatedDate As DateTime = Convert.ToDateTime(F_CreatedDate.Text)
                condition &= " AND CAST(mr.CreatedDate as DATE) = " & DB.Quote(CreatedDate.ToShortDateString())
            Catch ex As Exception
            End Try
        End If
        If Not String.IsNullOrEmpty(F_Status.SelectedValue) AndAlso F_Status.SelectedValue > 0 Then
            condition &= " AND mr.Status = " & DB.Quote(F_Status.SelectedValue)
        End If
        If Not String.IsNullOrEmpty(F_Type.SelectedValue) AndAlso F_Type.SelectedValue > 0 Then
            condition &= " AND mr.Source = " & DB.Quote(F_Type.SelectedValue)
        End If
        hidCon.Value = condition
    End Sub

    Private Sub BindList()
        Dim total As Integer = 0
        Dim dt As DataTable = MemberReferRow.GetList(hidCon.Value, gvList.SortBy, gvList.SortOrder, gvList.PageIndex + 1, gvList.PageSize, total)
        gvList.Pager.NofRecords = total
        gvList.PageSelectIndex = gvList.PageIndex

        gvList.DataSource = dt
        gvList.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ltrStatus As Literal = CType(e.Row.FindControl("ltrStatus"), Literal)
            Dim ltrSource As Literal = CType(e.Row.FindControl("ltrSource"), Literal)
            'ltrStatus.Text = [Enum].GetName(GetType(Utility.Common.ReferStatus), e.Row.DataItem("Status"))
            Dim status As Integer = Convert.ToInt32(e.Row.DataItem("Status"))

            If status = 5 Then
                ltrStatus.Text = String.Format(Utility.Common.EnumDescription(DirectCast(Convert.ToInt32(e.Row.DataItem("Status")), Utility.Common.ReferStatus)), SysParam.GetValue("PointReferFriend"))
            Else
                ltrStatus.Text = Utility.Common.EnumDescription(DirectCast(Convert.ToInt32(e.Row.DataItem("Status")), Utility.Common.ReferStatus))
            End If
            If status = Utility.Common.ReferStatus.OrderShipped Or status = Utility.Common.ReferStatus.Ordered Then
                If (Not IsDBNull(e.Row.DataItem("FirstOrderId")) AndAlso Not String.IsNullOrEmpty(e.Row.DataItem("FirstOrderId"))) Then
                    ltrStatus.Text = "<a href=""/admin/store/orders/edit.aspx?OrderId=" & e.Row.DataItem("FirstOrderId") & """>" & ltrStatus.Text & "</a>"
                End If
            End If

            Dim source As Integer = Convert.ToInt32(e.Row.DataItem("Source"))
            ltrSource.Text = getImageType(source)
        End If
    End Sub

    Private Sub BindEnumReferStatus()
        F_Status.Items.Clear()
        F_Status.Items.Add(New ListItem("- - -", ""))
        Dim objStatus As Utility.Common.ReferStatus
        For Each objStatus In [Enum].GetValues(GetType(Utility.Common.ReferStatus))
            Dim name As String = Utility.Common.EnumDescription(objStatus)
            Dim value As Integer = Convert.ToInt32(objStatus)
            If value = 5 Then
                name = String.Format(name, SysParam.GetValue("PointReferFriend"))
            End If
            Dim optionItem As New ListItem(name, value)
            F_Status.Items.Add(optionItem)
        Next
    End Sub
    Private Sub BindEnumReferSource()
        F_Type.Items.Clear()
        F_Type.Items.Add(New ListItem("All", ""))
        Dim objSource As Utility.Common.ReferSource
        For Each objSource In [Enum].GetValues(GetType(Utility.Common.ReferSource))
            Dim value As Integer = Convert.ToInt32(objSource)
            Dim name As String = getImageType(value)
            If String.IsNullOrEmpty(name) Then
                name = objSource.ToString()
            End If
            Dim optionItem As New ListItem(name, value)
            F_Type.Items.Add(optionItem)
        Next
        F_Type.Items(0).Selected = True
    End Sub

    Private Function getImageType(ByVal Source As Integer) As String
        Dim result As String = String.Empty
        If Source = 1 Then
            result = "<img title=""Email"" src=""/includes/theme-admin/images/icon-email.gif"">"
        ElseIf Source = 2 Then
            result = "<img title=""Facebook"" src=""/includes/theme-admin/images/icon-facebook.png"">"
        ElseIf Source = 3 Then
            result = "<img title=""Twitter"" src=""/includes/theme-admin/images/icon-twitter.png"">"
        End If
        Return result
    End Function
End Class
