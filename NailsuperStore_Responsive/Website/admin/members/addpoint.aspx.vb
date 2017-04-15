Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_members_AddPoint
    Inherits AdminPage
    Public totalPoint As Double
    Public Property MemberId() As Integer

        Get
            Dim o As Object = ViewState("MemberId")
            If o IsNot Nothing Then
                Return DirectCast(o, Integer)
            End If
            Return 0
        End Get

        Set(ByVal value As Integer)
            ViewState("MemberId") = value
        End Set
    End Property
   
    Public Property CashPointId() As Integer

        Get
            Dim o As Object = ViewState("CashPointId")
            If o IsNot Nothing Then
                Return DirectCast(o, Integer)
            End If
            Return 0
        End Get

        Set(ByVal value As Integer)
            ViewState("CashPointId") = value
        End Set
    End Property

    Public Property Username() As String

        Get
            Dim o As Object = ViewState("username")
            If o IsNot Nothing Then
                Return DirectCast(o, String)
            End If
            Return 0
        End Get
        Set(ByVal value As String)
            ViewState("username") = value
        End Set
    End Property
  
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            LoadData()
        End If

        totalPoint = CashPointRow.GetTotalCashPointByMember(DB, MemberId)
        If totalPoint < 0 Then
            totalPoint = 0
        End If
    End Sub
    Private Sub LoadData()

        Try
            MemberId = CInt(Request.QueryString("MemberId"))
        Catch ex As Exception

        End Try
       
        Try
            CashPointId = CInt(Request.QueryString("CashPointId"))
        Catch ex As Exception

        End Try
        Try
            txtTransactionNo.Text = Request.QueryString("transid")
        Catch ex As Exception
            txtTransactionNo.Text = ""
        End Try
        ''  MemberId = 21476
        If (MemberId < 1) Then
            Response.Redirect("default.aspx")
        End If
        Dim member As MemberRow = MemberRow.GetRow(MemberId)
        ltrHeader.Text = ltrHeader.Text & member.Username
        Username = member.Username
        LoadListCashPoint(member.Username)
        LoadCashPointRow()
    End Sub
    Public Sub LoadCashPointRow()
        Dim objCashPoint As CashPointRow
        If CashPointId > 0 Then
            objCashPoint = CashPointRow.GetRowByCashPointId(DB, CashPointId)
        ElseIf MemberId > 0 And txtTransactionNo.Text.Trim() <> "" Then
            objCashPoint = CashPointRow.GetRowByTransID(DB, txtTransactionNo.Text.Trim(), MemberId)
            If objCashPoint Is Nothing Then
                CashPointId = 0
            Else
                CashPointId = objCashPoint.CashPointId
            End If
        Else
            objCashPoint = Nothing
        End If
        If Not objCashPoint Is Nothing Then
            txtNote.Text = objCashPoint.Notes
            If objCashPoint.PointDebit > 0 Then
                txtPointDebit.Text = objCashPoint.PointDebit
            End If
            If objCashPoint.PointEarned > 0 Then
                txtPointEarn.Text = objCashPoint.PointEarned
            End If
            txtTransactionNo.Text = objCashPoint.TransactionNo
            drlStatus.SelectedValue = objCashPoint.Status
        End If
        'If Not IsEditPoint(txtTransactionNo.Text) Then
        '    txtPointDebit.Enabled = False
        '    txtPointEarn.Enabled = False
        'End If
    End Sub
    Private Function IsEditPoint(ByVal tranNo As String) As Boolean
        If tranNo Is Nothing Then
            Return True
        End If
        If tranNo = "" Then
            Return True
        End If
        If (tranNo.Substring(0, 1).ToLower().Trim() = "w") Then
            Return False
        End If
        Return True
    End Function
    Private Sub LoadListCashPoint(ByVal username As String)
        Dim lstTrans As CashPointCollection = CashPointRow.GetListTransactionByMemberAdmin(DB, username)
        If lstTrans.Count > 0 Then
            rptTrans.Visible = True
            rptTrans.DataSource = lstTrans
            rptTrans.DataBind()
        Else
            rptTrans.Visible = False
        End If
        
    End Sub
    Private Sub SetStylePendingControl(ByVal ctr As Literal, ByVal status As Integer, ByVal transType As Integer, ByVal pointDebit As Boolean)
        If (status <> 0) Then
            Exit Sub
        End If
        If (transType = 3 And pointDebit = True) Then
            Exit Sub
        Else
            ctr.Text = "<font color='red'>" & ctr.Text & "</font>"
        End If
    End Sub
    Protected Sub rptTrans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptTrans.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrAmount As Literal = CType(e.Item.FindControl("ltrAmount"), Literal)
            Dim objData As CashPointRow = e.Item.DataItem
            If (objData.Amount > 0) Then
                ltrAmount.Text = FormatCurrency(objData.Amount)
            Else
                ltrAmount.Text = ""
            End If
            Dim ltrPointDebit As Literal = CType(e.Item.FindControl("ltrPointDebit"), Literal)
            If (objData.PointDebit > 0) Then
                ltrPointDebit.Text = "-" & SitePage.NumberToString(objData.PointDebit)
            Else
                ltrPointDebit.Text = ""
            End If
            Dim ltrPointEarn As Literal = CType(e.Item.FindControl("ltrPointEarn"), Literal)
            If (objData.PointEarned > 0) Then
                ltrPointEarn.Text = SitePage.NumberToString(objData.PointEarned)
            Else
                ltrPointEarn.Text = ""
            End If
            Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
            Dim pointType As Integer = SitePage.CashPointType(objData.TransactionNo)
            Dim link As String = "#"
            ''admin/store/orders/edit.aspx?OrderId=30774
            ''admin/store/items/reviews/review.aspx?ReviewId=256&ItemId=47113
            If (pointType = 3) Then
                link = "/admin/store/orders/edit.aspx?OrderId=" & objData.OrderId & "&"
            ElseIf (pointType = 2) Then ''return item
                '' link = "/members/creditmemo/view.aspx?MemoId=" & SalesCreditMemoHeaderRow.SalesCreditMemoHeaderIDByNo(DB, objData.TransactionNo)
            ElseIf (pointType = 1) Then ''review product

                Dim item As StoreItemRow = StoreItemRow.GetRowSku(DB, objData.TransactionNo.Substring(2, objData.TransactionNo.Length - 2))
                Dim objItemReview As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, item.ItemId, MemberId)
                link = "/store/review.aspx?ReviewId=" & objItemReview.ReviewId & "&ItemId=" & item.ItemId
            End If
            If objData.TransactionNo <> "" Then
                ltrLink.Text = "<a target='_blank' href='" & link & "'> " & objData.TransactionNo & "</a>"
            End If
            Dim ltrStaus As Literal = CType(e.Item.FindControl("ltrStaus"), Literal)
            If objData.Status = 0 Then
                ltrStaus.Text = "<font color='red'>Pending</font>"
            ElseIf objData.Status = 1 Then
                ltrStaus.Text = "Active"
            Else
                ltrStaus.Text = "Inactive"
            End If
            Dim ltrEdit As Literal = CType(e.Item.FindControl("ltrEdit"), Literal)
            ltrEdit.Text = "<a href='addpoint.aspx?MemberId=" & MemberId & "&CashPointId=" & objData.CashPointId & "&" & GetPageParams(Components.FilterFieldType.All) & "'><img style='border-width: 0px;' alt='Edit' src='/includes/theme-admin/images/edit.gif'></a>"
            SetStylePendingControl(ltrPointEarn, objData.Status, pointType, False)
            SetStylePendingControl(ltrPointDebit, objData.Status, pointType, True)
        End If

    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        txtNote.Text = ""
        txtPointDebit.Text = ""
        txtPointEarn.Text = ""
        drlStatus.SelectedIndex = 0
        txtTransactionNo.Text = ""
        CashPointId = 0
        'txtPointDebit.Enabled = True
        'txtPointEarn.Enabled = True
    End Sub
    Public Function CheckPointTotal(ByVal pointEarn As Integer, ByVal pointDebit As Integer) As Boolean
        If Not IsEditPoint(txtTransactionNo.Text) Then
            Return True
        End If
        If drlStatus.SelectedValue <> "1" Then
            Return True
        End If
        Dim currentPoint As Integer = CashPointRow.GetTotalCashPointByMember(DB, MemberId)
        If (currentPoint + pointEarn - pointDebit < 0) Then
            AddError("Points Debit must <= Points Earned + " & currentPoint)
            Return False
        End If
        Return True
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtPointEarn.Text.Trim() = "" And txtPointDebit.Text.Trim() = "" Then
            '' ltrError.Text = "You must input  Points Earned or Points Debit"
            AddError("You must enter Points Earned or Points Debit")
            Exit Sub
        End If
        If Page.IsValid Then
            Try
               
                Dim objData As CashPointRow = CashPointRow.GetRowByCashPointId(DB, CashPointId)
                Dim objDataBefore As CashPointRow = CloneObject.Clone(objData)
                If (objData Is Nothing) Then
                    objData = New CashPointRow(DB)
                    objData.CreateDate = Now
                    objData.OrderId = Nothing
                    If drlStatus.SelectedValue = "1" Then
                        objData.ApproveDate = Now
                    Else
                        objData.ApproveDate = Nothing
                    End If
                Else
                    objData.ModifyDate = Now
                    If objData.Status = 1 Then
                        If drlStatus.SelectedValue <> 1 Then
                            objData.ApproveDate = Nothing
                        End If
                    Else
                        If drlStatus.SelectedValue = 1 Then
                            objData.ApproveDate = Now
                        Else
                            objData.ApproveDate = Nothing
                        End If
                    End If
                End If
                objData.TransactionNo = txtTransactionNo.Text.Trim()
                objData.MemberId = MemberId
                If txtPointDebit.Text <> "" Then
                    objData.PointDebit = CInt(txtPointDebit.Text)
                Else
                    objData.PointDebit = Nothing
                End If
                If txtPointEarn.Text <> "" Then
                    objData.PointEarned = CInt(txtPointEarn.Text)
                Else
                    objData.PointEarned = Nothing
                End If
                objData.Status = CInt(drlStatus.SelectedValue)
                objData.AdminId = Session("AdminId")
                objData.Notes = txtNote.Text
                If Not (CheckPointTotal(objData.PointEarned, objData.PointDebit)) Then
                    Exit Sub
                End If
                Dim logDetail As New AdminLogDetailRow

                If CashPointId > 0 Then
                    objData.Update()
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                    logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.CashPoint, objDataBefore, objData)
                    WriteLogDetail("Update cash point", objData)
                Else
                    objData.Insert()
                    logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                    logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(objData, Utility.Common.ObjectType.CashPoint)
                    WriteLogDetail("Insert cash point", objData)
                End If
                logDetail.ObjectType = Utility.Common.ObjectType.CashPoint.ToString()
                logDetail.ObjectId = CashPointId
                AdminLogHelper.WriteLuceneLogDetail(logDetail)

                totalPoint = CashPointRow.GetTotalCashPointByMember(DB, MemberId)
                If totalPoint < 0 Then
                    totalPoint = 0
                End If
                '
                'WriteLogDetail("Manual Adjustment point", objData)
                ''Response.Redirect("addpoint.aspx?MemberId=" & MemberId)
                LoadListCashPoint(Username)
                txtNote.Text = ""
                txtPointDebit.Text = ""
                txtPointEarn.Text = ""
                'txtPointDebit.Enabled = True
                'txtPointEarn.Enabled = True
                drlStatus.SelectedIndex = 0
                txtTransactionNo.Text = ""
                CashPointId = 0
                UpdateOrderIsLevelPoint()
            Catch ex As Exception

                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If

    End Sub
    Protected Sub CheckSPoint(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        If (args.Value <> "") Then
            Dim number As Integer = 0
            Try
                number = CInt(args.Value)
            Catch ex As Exception

            End Try
            If (number < 1) Then
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        Else
            args.IsValid = True
        End If
    End Sub
    Private Sub UpdateOrderIsLevelPoint()
        Dim OrderId As Integer = DB.ExecuteScalar("Select isnull(OrderId,0) From StoreOrder Where MemberId = " & MemberId & " and OrderNo is null and PointLevelMessage <> null")
        If OrderId <> 0 Then
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
            LoadLevelMember(o)
        End If
    End Sub
    Public Sub LoadLevelMember(ByVal o As StoreOrderRow)
        ''''Level Point''''
        Dim Message As String = ""
        Dim PercentDiscount As Integer = 0
        Dim dt As DataTable
        'If o.IsLevelDiscount = True Then
        dt = LevelPointRow.GetDiscount(o.MemberId)
        If dt.Rows.Count > 0 Then
            PercentDiscount = dt.Rows(0)("Discount")
            Message = dt.Rows(0)("Description")
        End If
        If PercentDiscount > 0 Then
            o.PointAmountDiscount = Math.Round(o.SubTotal * PercentDiscount / 100, 2)
            'o.SubTotal = o.SubTotal - o.PointAmountDiscount
            o.PointLevelMessage = Message
            'o.IsLevelDiscount = True
        End If
        o.Update()
        'End If
        ''''End level Point'''
    End Sub
End Class
