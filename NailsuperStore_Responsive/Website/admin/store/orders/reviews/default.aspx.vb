Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_orders_reviews_Index
    Inherits AdminPage

    Protected GroupPros As String = ConfigurationManager.AppSettings("GroupPros")
    Protected GroupCons As String = ConfigurationManager.AppSettings("GroupCons")
    Protected GroupExperienceLevel As String = ConfigurationManager.AppSettings("GroupExperienceLevel")
    Private OrderId As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If IsNumeric(OrderId1.Value) Then
            OrderId = OrderId1.Value
        End If
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            'F_ItemId.DataSource = DB.GetDataSet("SELECT ItemId, ItemName + '(' + SKU + ')' As ItemName FROM StoreItem ORDER BY ItemName ASC")
            'F_ItemId.DataTextField = "ItemName"
            'F_ItemId.DataValueField = "ItemId"
            'F_ItemId.DataBind()
            'F_ItemId.Items.Insert(0, New ListItem("", ""))


            F_IsActive.Text = IIf(Request("F_IsActive") = Nothing, "0", Request("F_IsActive"))
            F_IsShared.Text = Request("F_IsShared")
            'F_ItemId.SelectedValue = Request("F_ItemId")

            F_NumStarsLBound.Text = Request("F_NumStarsLBound")
            F_NumStarsUBound.Text = Request("F_NumStarsUBound")
            F_DateAddedLbound.Text = Request("F_DateAddedLBound")
            F_DateAddedUbound.Text = Request("F_DateAddedUBound")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ReviewId"

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub

        End If
        Dim ltrOrderNo As Literal = e.Row.FindControl("ltrOrderNo")
        Dim ltrStar As Literal = e.Row.FindControl("ltrStar")
        Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
        Dim facebook As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsFacebook")
        Dim Share As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsShared")
        Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
        Dim imbFacebook As ImageButton = CType(e.Row.FindControl("imbFacebook"), ImageButton)
        Dim imbShared As ImageButton = CType(e.Row.FindControl("imbShared"), ImageButton)
        Dim imgFb As Literal = e.Row.FindControl("imgFb")
        Dim imgSh As Literal = e.Row.FindControl("imgSh")
        ltrOrderNo.Text = "<a href='/admin/store/orders/edit.aspx?OrderId=" & e.Row.DataItem("OrderId") & "'> " & e.Row.DataItem("OrderNo") & "</a>"

        ltrStar.Text = "<img src=""/includes/theme/images/star" & e.Row.DataItem("NumStars") & "0.png"">"
        If active Then
            imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
        If facebook Then
            imbFacebook.Visible = False
            ' imbFacebook.ImageUrl = "/includes/theme-admin/images/active.png"
            imgFb.Text = "<img src=""/includes/theme-admin/images/active.png"" />"
        Else
            imbFacebook.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
        If Share Then
            imbShared.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbShared.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM StoreOrderReview sor inner join StoreOrder so on(sor.OrderId = so.OrderId) left join Member mb on(mb.MemberId=so.MemberId)  "
        SQL = SQL & "left join Customer cus on (cus.CustomerId=mb.CustomerId)"
        If Not String.IsNullOrEmpty(F_txtCustomerNo.Text) Then
            SQL = SQL & Conn & "CustomerNo ='" & F_txtCustomerNo.Text.Trim & "' "
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(F_txtOrderNo.Text) Then
            SQL = SQL & Conn & "so.OrderNo ='" & F_txtOrderNo.Text.Trim & "' "
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(F_txtFirstName.Text) Then
            SQL = SQL & Conn & "cus.Name like'%" & F_txtFirstName.Text.Trim() & "%'"
            Conn = " AND "
        End If
        If OrderId > 0 Then
            SQL = SQL & Conn & "sor.OrderId = " & OrderId
            Conn = " AND "

        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sor.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_IsShared.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sor.IsShared  = " & DB.Number(F_IsShared.SelectedValue)
            Conn = " AND "
        End If
        If Not F_DateAddedLbound.Text = String.Empty Then
            SQL = SQL & Conn & "DateAdded >= " & DB.Quote(F_DateAddedLbound.Text)
            Conn = " AND "
        End If
        If Not F_DateAddedUbound.Text = String.Empty Then
            SQL = SQL & Conn & "DateAdded < " & DB.Quote(DateAdd("d", 1, F_DateAddedUbound.Text))
            Conn = " AND "
        End If

        If Not F_NumStarsLBound.Text = String.Empty Then
            SQL = SQL & Conn & "NumStars >= " & DB.Number(F_NumStarsLBound.Text)
            Conn = " AND "
        End If
        If Not F_NumStarsUBound.Text = String.Empty Then
            SQL = SQL & Conn & "NumStars <= " & DB.Number(F_NumStarsUBound.Text)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sor.*, OrderNo, mb.Username"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("DateAdded") = False, " Dateadded desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
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
        ElseIf e.CommandName = "Facebook" Then
            GetReviewToFaceBook(e.CommandArgument)
        ElseIf e.CommandName = "Shared" Then
            ChangeShared(e.CommandArgument)
        End If
        BindList()
    End Sub
    Private Sub ChangeActive(ByVal id As Integer)
        Try
            Dim sor As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, CInt(id))
            If sor Is Nothing Then
                Exit Sub
            End If
            If sor.OrderId < 0 Then
                Exit Sub
            End If
            Dim sorBefore As StoreOrderReviewRow = CloneObject.Clone(sor)
            Dim logDetail As New AdminLogDetailRow
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, sor.OrderId)
            Dim cp As CashPointRow = New CashPointRow(DB)
            If (sor.IsActive = False) Then
                sor.IsActive = True
                ''insert point
                cp = cp.SetValueCashPoint(cp, o, CInt(Session("AdminId")), 1)
                If cp.CheckTransactionNoExists(DB, cp.MemberId, cp.TransactionNo) = False Then
                    cp.Insert()
                    logDetail.Message &= "AddPoint|False|True[br]"
                End If
            Else
                sor.IsActive = False
                sor.IsShared = False
            End If
            sor.Update()
            logDetail.Message &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderReview, sorBefore, sor)
            logDetail.ObjectId = sor.OrderId
            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub ChangeShared(ByVal id As Integer)
        Try
            Dim sor As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, CInt(id))
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, sor.OrderId)
            If sor Is Nothing Then
                Exit Sub
            End If
            If sor.OrderId < 0 Then
                Exit Sub
            End If
            Dim sorBefore As StoreOrderReviewRow = CloneObject.Clone(sor)
            Dim logDetail As New AdminLogDetailRow
            If sor.IsShared Then
                sor.IsShared = False
            Else
                sor.IsShared = True
                sor.IsActive = True
                ''check point exits
                Dim cp As CashPointRow = New CashPointRow(DB)
                cp = cp.SetValueCashPoint(cp, o, CInt(Session("AdminId")), 1)
                If cp.CheckTransactionNoExists(DB, cp.MemberId, cp.TransactionNo) = False Then
                    cp.Insert()
                    logDetail.Message &= "AddPoint|False|True[br]"
                End If
            End If
            sor.Update()

            logDetail.Message &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderReview, sorBefore, sor)
            logDetail.ObjectId = sor.OrderId
            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetReviewToFaceBook(ByVal OrderId As Integer)
        If OrderId > 0 Then
            Dim sor As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, OrderId)
            Dim result As Integer = SocialHelper.PostOrderReviewToFB(DB, sor)
            If (result <> 0) Then
                sor.IsFacebook = True
                sor.Update()

                Dim logDetail As New AdminLogDetailRow
                logDetail.Message = "IsFacebook|False|True[br]"
                logDetail.ObjectId = sor.OrderId
                logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
        End If
    End Sub

    Function addString(ByVal ct As String, ByVal arr As String(), ByVal cons As String, ByVal k As Integer) As String
        If k = 1 Then
            For i As Integer = 0 To arr.Length - 1
                If arr(i).ToString.Contains("#") = False Then
                    If LCase(ct).ToString.Contains(LCase(cons)) Then
                        ct &= arr(i).ToString
                    End If
                End If
            Next
        End If
        If k = 2 Then
            For i As Integer = 0 To arr.Length - 1
                If arr(i).ToString.Contains("#") = False Then
                    If LCase(ct).ToString.Contains(LCase(cons)) Then
                        ct &= ", " & arr(i).ToString
                    End If
                End If
            Next
        End If
        Return ct
    End Function


End Class