Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_reviews_Index
    Inherits AdminPage

    Protected GroupPros As String = ConfigurationManager.AppSettings("GroupPros")
    Protected GroupCons As String = ConfigurationManager.AppSettings("GroupCons")
    Protected GroupExperienceLevel As String = ConfigurationManager.AppSettings("GroupExperienceLevel")
    Private ItemId As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If IsNumeric(ItemId1.Value) Then
            ItemId = ItemId1.Value
        End If
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            'F_ItemId.DataSource = DB.GetDataSet("SELECT ItemId, ItemName + '(' + SKU + ')' As ItemName FROM StoreItem ORDER BY ItemName ASC")
            'F_ItemId.DataTextField = "ItemName"
            'F_ItemId.DataValueField = "ItemId"
            'F_ItemId.DataBind()
            'F_ItemId.Items.Insert(0, New ListItem("", ""))

            F_ReviewTitle.Text = Request("F_ReviewTitle")
            F_FirstName.Text = Request("F_FirstName")
            F_LastName.Text = Request("F_LastName")
            F_Status.Text = IIf(Request("F_Status") = Nothing, "0", Request("F_Status"))
            F_IsFeatured.Text = Request("F_IsFeatured")
            'F_ItemId.SelectedValue = Request("F_ItemId")

            F_NumStarsLBound.Text = Request("F_NumStarsLBound")
            F_NumStarsUBound.Text = Request("F_NumStarsUBound")
            F_DateAddedLbound.Text = Request("F_DateAddedLBound")
            F_DateAddedUbound.Text = Request("F_DateAddedUBound")
            F_Bought.Text = Request("F_Bought")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = "DESC" 'Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "DateAdded"

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub

        End If
        Dim ltrItemName As Literal = e.Row.FindControl("ltrItemName")
        Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
        Dim ltrStar As Literal = e.Row.FindControl("ltrStar")
        Dim ltrPoint As Literal = e.Row.FindControl("ltrPoint")
        Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
        Dim facebook As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsFacebook")
        Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
        Dim imbPoint As ImageButton = CType(e.Row.FindControl("imbPoint"), ImageButton)
        Dim imbFacebook As ImageButton = CType(e.Row.FindControl("imbFacebook"), ImageButton)
        Dim imgFb As Literal = e.Row.FindControl("imgFb")
        ltrItemName.Text = "<a href='/admin/store/items/edit.aspx?ItemId=" & e.Row.DataItem("ItemId") & "'> " & e.Row.DataItem("ItemName") & "</a>"
        ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & e.Row.DataItem("MemberId") & "'> " & e.Row.DataItem("FirstName") & " " & e.Row.DataItem("LastName") & "</a>"
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

        If e.Row.DataItem("PointAdded") = 0 Then
           
            ltrPoint.Visible = False
            Dim AllowAddPoint As Integer = 0
            Try
                AllowAddPoint = e.Row.DataItem("AllowAddPoint")
            Catch ex As Exception

            End Try
            If (AllowAddPoint = 1) Then
                imbPoint.Visible = True
            Else
                imbPoint.Visible = False
            End If

        Else
            imbPoint.Visible = False
            ltrPoint.Visible = True
            ltrPoint.Text = e.Row.DataItem("PointAdded")
        End If

        ''get order No
        Dim orderNo As String = DB.ExecuteScalar("Select OrderNo from StoreOrder where OrderId=(Select OrderId from StoreCartItem where CartItemId =(Select CartItemId from StoreItemReview where ReviewId=" & e.Row.DataItem("ReviewId") & "))")

        If Not String.IsNullOrEmpty(orderNo) Then
            Dim ltrOrderNo As Literal = e.Row.FindControl("ltrOrderNo")
            ltrOrderNo.Text = "<a href='/admin/store/orders/default.aspx?F_OrderNo=" & orderNo & "'>" & orderNo & "</a>"
        End If

    End Sub

    Private Sub BindQuery()

        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM StoreItemReview sir inner join storeitem si on(sir.itemid = si.itemid) left join Member mb on(mb.MemberId=sir.MemberId)  "
        If Not String.IsNullOrEmpty(F_txtCustomerNo.Text) Then
            SQL = SQL & "left join Customer cus on (cus.CustomerId=mb.CustomerId)"
            SQL = SQL & Conn & "CustomerNo ='" & F_txtCustomerNo.Text & "' "
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(F_txtUserName.Text) Then
            SQL = SQL & Conn & "mb.Username ='" & F_txtUserName.Text & "' "
            Conn = " AND "
        End If
        If ItemId > 0 Then
            SQL = SQL & Conn & "sir.ItemId = " & ItemId
            Conn = " AND "
        ElseIf LookupField.Value <> Nothing Then
            ItemId = StoreItemRow.GetRow(DB, LookupField.Value.ToString).ItemId
            SQL = SQL & Conn & "sir.ItemId = " & ItemId
            Conn = " AND "
        End If
        If Not F_ReviewTitle.Text = String.Empty Then
            SQL = SQL & Conn & "ReviewTitle LIKE " & DB.FilterQuote(F_ReviewTitle.Text)
            Conn = " AND "
        End If
        If Not F_FirstName.Text = String.Empty Then
            SQL = SQL & Conn & "FirstName LIKE " & DB.FilterQuote(F_FirstName.Text)
            Conn = " AND "
        End If
        If Not F_LastName.Text = String.Empty Then
            SQL = SQL & Conn & "LastName LIKE " & DB.FilterQuote(F_LastName.Text)
            Conn = " AND "
        End If
        If Not F_Status.SelectedValue = String.Empty Then
            If F_Status.SelectedValue = "0" Then
                SQL = SQL & Conn & "sir.IsActive  =0"
            ElseIf F_Status.SelectedValue = "1" Then
                SQL = SQL & Conn & "sir.IsActive  =1"
            ElseIf F_Status.SelectedValue = "2" Then
                SQL = SQL & Conn & "sir.IsAddPoint  =1"
            End If

            Conn = " AND "
        End If
        If Not F_IsFeatured.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "sir.IsFeatured  = " & DB.Number(F_IsFeatured.SelectedValue)
            Conn = " AND "
        End If


        If Not F_Bought.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "[dbo].[fc_StoreItemReview_HasBoughtItem](ReviewId) =" & DB.Number(F_Bought.SelectedValue)
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
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sir.*,[dbo].[fc_StoreItemReview_GetPointAddedReview](ReviewId) as PointAdded, si.Itemname, si.SKU,si.itemid,mb.Username,[dbo].[fc_StoreItemReview_IsAllowAddPoint](ReviewId) as AllowAddPoint"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder) 'IIf(gvList.SortByAndOrder.Contains("DateAdded") = False, " Dateadded desc", gvList.SortByAndOrder))
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
        ElseIf e.CommandName = "Facebook" Then
            GetReviewToFaceBook(e.CommandArgument)
        ElseIf e.CommandName = "AddPoint" Then
            AddPoint(e.CommandArgument)
        End If

        BindList()
    End Sub
    Private Sub ChangeActive(ByVal id As Integer)
        Try
            Dim sir As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, CInt(id))
            Dim addPoint As Integer = IIf(sir.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
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
            sir.Update()

            Dim resultAddPoint As Boolean = CashPointRow.AddPointProductReview(DB, sir.MemberId, sir.ItemId, addPoint)
            If (resultAddPoint) Then
                logDetail.Message &= "AddPoint|False|True[br]"
            End If
            logDetail.ObjectId = sir.ReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AddPoint(ByVal id As Integer)
        Try
            Dim sir As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, CInt(id))
            Dim addPoint As Integer = IIf(sir.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
            Dim logDetail As New AdminLogDetailRow
            Dim resultAddPoint As Boolean = CashPointRow.AddPointProductReview(DB, sir.MemberId, sir.ItemId, addPoint)
            If (resultAddPoint) Then
                logDetail.Message &= "AddPoint|False|True[br]"
                logDetail.ObjectId = sir.ReviewId
                logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetReviewToFaceBook(ByVal ReviewId As Integer)
        If ReviewId > 0 Then
            Dim sir As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ReviewId)
            Dim logDetail As New AdminLogDetailRow
            Dim result As Integer = SocialHelper.PostItemReviewToFB(DB, sir)
            If (result <> 0) Then
                sir.IsFacebook = True
                sir.Update()
                logDetail.Message = "IsFacebook|False|True|[br]"
                logDetail.ObjectId = sir.ReviewId
                logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
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
    Function LoadGroup(ByVal str As String, ByVal Template As String) As String
        If GroupPros.Contains(str) Then
            Template = Template.Replace("<div id=""GPros"" class=""groupChk hidediv"">", "<div id=""GPros"" class=""groupChk showdiv"">")
        End If
        If GroupCons.Contains(str) Then
            Template = Template.Replace("<div id=""GCons"" class=""groupChk hidediv"">", "<div id=""GCons"" class=""groupChk showdiv"">")
        End If
        If GroupExperienceLevel.Contains(str) Then
            Template = Template.Replace("<div id=""GExperienceLevel"" class=""groupChk hidediv"">", "<div id=""GExperienceLevel"" class=""groupChk showdiv"">")
        End If
        Return Template
    End Function
    Function FindDiv(ByVal DivId As String, ByVal Status As String) As String
        Dim str As String = "<div id=""" & DivId.Trim.Replace(" ", "").Replace("+", "").Replace("#", "") & """ class=""" & Status & """>"
        Return str
    End Function
End Class