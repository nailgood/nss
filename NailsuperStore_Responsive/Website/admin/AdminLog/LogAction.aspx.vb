

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Public Class admin_AdminLog_LogAction
    Inherits AdminPage
    Dim ToTal As Integer
    Dim m_isAllowProcessLuceneMsg As Boolean = False
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            m_isAllowProcessLuceneMsg = IsAllowProcessValidLuceneAdminLogMsg()
            gvList.SortBy = "CreatedDate"
            gvList.SortOrder = "DESC"
            Utility.Common.BindEnumLogObjectType(drlObject, True)
            Utility.Common.BindEnumLogAction(drpAction)
            LoadUsername()
            BindList()
        End If
    End Sub

    Private Sub BindList()


        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        Dim actionSelect As String = String.Empty

        actionSelect = drpAction.SelectedValue
        Dim res As DataTable = AdminLogHelper.SearchAdminLog(drpUsername.SelectedValue, F_FromDate.Text, F_ToDate.Text, actionSelect, drlObject.SelectedValue, gvList.PageIndex, gvList.PageSize, ToTal, gvList.SortBy, gvList.SortOrder, DB, txtSKU.Text, txtTransID.Text, txtOrderNo.Text)
        gvList.Pager.NofRecords = ToTal
        gvList.DataSource = res.DefaultView
        gvList.PageSelectIndex = gvList.PageIndex
        gvList.DataBind()

        If (drpAction.SelectedValue.ToString() = Utility.Common.AdminLogAction.Update.ToString() Or drpAction.SelectedValue.ToString() = String.Empty) Then
            gvList.Columns(3).Visible = True
        Else
            gvList.Columns(3).Visible = False
        End If
        '' Me.RegisterStartupScript("resetType", "changeObject('" & drlObject.SelectedValue.ToString() & "')")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "resetType", "changeObject('" & drlObject.SelectedValue.ToString() & "');", True)
    End Sub



    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        m_isAllowProcessLuceneMsg = IsAllowProcessValidLuceneAdminLogMsg()
        gvList.SortBy = "CreatedDate"
        gvList.SortOrder = "DESC"
        gvList.PageIndex = 0
        BindList()
    End Sub
    Public Function IsAllowProcessValidLuceneAdminLogMsg() As String
        Dim result As String = String.Empty
        Try
            result = System.Configuration.ConfigurationManager.AppSettings("IsAllowProcessValidLuceneAdminLogMsg")
        Catch ex As Exception

        End Try
        If result = "1" Then
            Return True
        End If
        Return False
    End Function
    Private Function ProcessValidLuceneMsg(ByVal msgLucene As String, ByVal actionType As String, ByVal objectType As Utility.Common.ObjectType) As String
        If Not m_isAllowProcessLuceneMsg Then
            Return msgLucene
        End If
        If (String.IsNullOrEmpty(msgLucene)) Then
            Return String.Empty
        End If
        If objectType <> Utility.Common.ObjectType.StoreItem Then
            Return msgLucene
        End If
        Dim result As String = String.Empty
        ''actionType = Utility.Common.AdminLogAction.Update.ToString()
        Dim arrMess() As String = msgLucene.Split(New String() {AdminLogHelper.AdminLogSeparateCharNew}, StringSplitOptions.None)
        Dim msg As String = String.Empty
        Dim nextPropertyName As String = String.Empty
        Dim currentPropertyName As String = String.Empty
        For i As Integer = 0 To arrMess.Length - 1
            nextPropertyName = String.Empty
            msg = arrMess(i).ToString()
            If (i = 0) Then
                currentPropertyName = msg
            Else
                If (i Mod 2 <> 0) Then ''khog can xet TH nay
                    If (currentPropertyName = "LongDesc") Then
                        msg = BBCodeHelper.ConvertBBCodeToHTML(msg)
                    End If
                Else
                    Dim arrSubMess As String() = msg.Split(New String() {"[br]"}, StringSplitOptions.None)
                    If (arrSubMess.Length > 0) Then
                        ''msg = String.Empty
                        Dim lastMsg As String = arrSubMess(arrSubMess.Length - 1).ToString()
                        If Not String.IsNullOrEmpty(lastMsg) AndAlso Not lastMsg.Contains("<") Then
                            ''check xem day co phai la propersty cua object hay khong
                            Dim objEntry As New StoreItemRow
                            For Each [property] As System.Reflection.PropertyInfo In objEntry.GetType.GetProperties()
                                Dim fieldName As String = [property].Name
                                If (fieldName.ToLower.ToLower().Trim() = lastMsg.ToLower().Trim()) Then ''la property
                                    nextPropertyName = "[br]" & [property].Name
                                    Exit For
                                End If
                            Next
                        End If
                        If Not String.IsNullOrEmpty(nextPropertyName) Then
                            msg = msg.Substring(0, msg.Length - nextPropertyName.Length)
                        End If
                        If (currentPropertyName = "LongDesc") Then
                            msg = BBCodeHelper.ConvertBBCodeToHTML(msg)
                        End If
                        currentPropertyName = nextPropertyName.Replace("[br]", "")
                        msg = msg & nextPropertyName
                    End If
                End If
            End If
            If (i < arrMess.Length - 1) Then
                result = result & msg & AdminLogHelper.AdminLogSeparateCharNew
            Else
                result = result & msg
            End If
        Next
        'If (actionType = Utility.Common.AdminLogAction.Update.ToString()) Then '' format msg: Property**+|+**valueBeforeUpdate**+|+**valueAfterUpdate[br]Property**+|+**valueBeforeUpdate**+|+**valueAfterUpdate

        'End If
        Return result
    End Function
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Action As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("Action")
            Dim type As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("ObjectType")
            Dim ObjectType As Utility.Common.ObjectType = DirectCast([Enum].Parse(GetType(Utility.Common.ObjectType), type), Utility.Common.ObjectType)

            'If ObjectType = Utility.Common.ObjectType.LandingPage Then
            '    e.Row.Visible = False
            'End If
            Dim sUsername As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("Username")
            If String.IsNullOrEmpty(sUsername) Then
                sUsername = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("LoggedEmail")
                Dim i As Integer = sUsername.IndexOf("@")
                If i > 0 Then
                    sUsername = sUsername.Substring(0, i)
                End If
            End If
            Dim ltrUsername As Literal = CType(e.Row.FindControl("ltrUsername"), Literal)
            ltrUsername.Text = sUsername
            Dim ltrAction As Literal = CType(e.Row.FindControl("ltrAction"), Literal)
            ltrAction.Text = Action
            Dim Message As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("Message")
            Message = ProcessValidLuceneMsg(Message, Action, Utility.Common.ObjectType.StoreItem)
            Dim ltrActionDetail As Literal = CType(e.Row.FindControl("ltrActionDetail"), Literal)
            If (Action = Utility.Common.AdminLogAction.Update.ToString()) Then
                ltrActionDetail.Text = AdminLogHelper.ConvertMessageLogUpdateToText(Message, ObjectType, DB)
                Dim ltrObjectName As Literal = CType(e.Row.FindControl("ltrObjectName"), Literal)
                Dim objid As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("ObjectID")

                If ObjectType = Utility.Common.ObjectType.StoreItem Or ObjectType = Utility.Common.ObjectType.FreeSample Or ObjectType = Utility.Common.ObjectType.ItemPoint Then
                    Dim item As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(objid))
                    If Not item Is Nothing And item.ItemId > 0 Then
                        If (Not String.IsNullOrEmpty(item.SKU) And Not String.IsNullOrEmpty(item.ItemName)) Then
                            ltrObjectName.Text = "<b>SKU:</b> " & item.SKU & "<br/>"
                            ltrObjectName.Text &= "<b>Item Name:</b> " & item.ItemName
                        End If
                    End If

                ElseIf ObjectType = Utility.Common.ObjectType.MixMatch Then
                    Dim mixmatch As MixMatchRow = MixMatchRow.GetRow(DB, CInt(objid))
                    If Not mixmatch Is Nothing Then
                        ltrObjectName.Text = mixmatch.Description
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.MixMatchLine Then
                    Dim line As MixMatchLineRow = MixMatchLineRow.GetRow(DB, CInt(objid))
                    If Not line Is Nothing Then
                        Dim mixmatch As MixMatchRow = MixMatchRow.GetRow(DB, line.MixMatchId)
                        If Not mixmatch Is Nothing Then
                            ltrObjectName.Text = "<b>Mixmatch:</b> " & mixmatch.MixMatchNo & " - " & mixmatch.Description & "<br/>"
                            ltrObjectName.Text = ltrObjectName.Text & "<br><b>Line No:</b> " & line.LineNo
                        End If
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.OrderCoupon Then
                    Dim orderCoupon As StorePromotionRow = StorePromotionRow.GetRow(DB, CInt(objid))
                    If Not orderCoupon Is Nothing Then
                        ltrObjectName.Text = orderCoupon.PromotionCode & " - " & orderCoupon.Message
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.ProductCoupon Then
                    Dim orderCoupon As StorePromotionRow = StorePromotionRow.GetRow(DB, CInt(objid))
                    If Not orderCoupon Is Nothing Then
                        ltrObjectName.Text = orderCoupon.PromotionCode & " - " & orderCoupon.Message
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.FreeGift Then
                    Dim objFreeGift As FreeGiftRow = FreeGiftRow.GetRow(DB, CInt(objid))
                    If Not objFreeGift Is Nothing Then
                        Dim item As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(objFreeGift.ItemId))
                        If Not item Is Nothing And item.ItemId > 0 Then
                            If (Not String.IsNullOrEmpty(item.SKU) And Not String.IsNullOrEmpty(item.ItemName)) Then
                                ltrObjectName.Text = "<b>SKU:</b> " & item.SKU & "<br/>"
                                ltrObjectName.Text &= "<b>Item Name:</b> " & item.ItemName
                            End If
                        End If
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.SalesPrice Then
                    Dim salesprice As SalesPriceRow = SalesPriceRow.GetRowById(DB, CInt(objid))
                    If Not salesprice Is Nothing Then
                        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, salesprice.ItemId)
                        If Not item Is Nothing And item.ItemId > 0 And salesprice.ItemId > 0 Then
                            ltrObjectName.Text = "<b>Sales Price:</b> " & objid & "<br/>"
                            ltrObjectName.Text &= "<br/><b>SKU: </b>" & item.SKU
                            ltrObjectName.Text &= "<br/><b>Item Name: </b>" & item.ItemName
                        End If
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.Member Then
                    Dim member As MemberRow = MemberRow.GetRow(CInt(objid))
                    If Not member Is Nothing Then
                        ltrObjectName.Text = "<b>Username</b> " & member.Username
                        ltrObjectName.Text &= "<br/><b>Email: </b>" & CustomerRow.GetRow(DB, member.CustomerId).Email
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.CashPoint Then
                    Dim cashpoint As CashPointRow = CashPointRow.GetRowByCashPointId(DB, CInt(objid))
                    If Not cashpoint Is Nothing Then
                        Dim member As MemberRow = MemberRow.GetRow(cashpoint.MemberId)
                        ltrObjectName.Text = "<b>CashPointId</b> " & objid & "<br/>"
                        ltrObjectName.Text &= "<br/><b>Username: </b>" & member.Username
                        ltrObjectName.Text &= "<br/><b>Email: </b>" & CustomerRow.GetRow(DB, member.CustomerId).Email
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.LandingPage Then
                    Dim landing As LandingPageRow = LandingPageRow.GetRow(DB, objid)
                    If Not landing Is Nothing AndAlso Not String.IsNullOrEmpty(landing.Title) Then
                        ltrObjectName.Text = "<b>Title:</b> " & landing.Title
                        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, landing.ItemId)
                        If Not item Is Nothing And item.ItemId > 0 Then
                            ltrObjectName.Text &= "<br/><br/><b>SKU: </b>" & item.SKU
                            ltrObjectName.Text &= "<br/><b>Item Name: </b>" & item.ItemName
                        End If
                    Else
                        ltrObjectName.Text = "<b>Id:</b> " & objid
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.News Or ObjectType = Utility.Common.ObjectType.Blog Then
                    Dim news As NewsRow = NewsRow.GetRow(DB, objid)
                    If Not news Is Nothing AndAlso Not String.IsNullOrEmpty(news.Title) Then
                        ltrObjectName.Text = "<b>Title:</b> " & news.Title
                    Else
                        ltrObjectName.Text = "<b>Id:</b> " & objid
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.ProductReview Then
                    Dim sir As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, objid)
                    If Not sir Is Nothing AndAlso Not String.IsNullOrEmpty(sir.ReviewTitle) Then
                        ltrObjectName.Text = "<b>Title:</b> " & sir.ReviewTitle
                        Dim member As MemberRow = MemberRow.GetRow(sir.MemberId)
                        If Not member Is Nothing And member.MemberId > 0 Then
                            ltrObjectName.Text &= "<br/><br/><b>Username: </b>" & member.Username
                            ltrObjectName.Text &= "<br/><b>Email: </b>" & member.Customer.Email
                        End If
                        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, sir.ItemId)
                        If Not item Is Nothing And item.ItemId > 0 Then
                            ltrObjectName.Text &= "<br/><br/><b>SKU: </b>" & item.SKU
                            ltrObjectName.Text &= "<br/><b>Item Name: </b>" & item.ItemName
                        End If
                    Else
                        ltrObjectName.Text = "<b>Id:</b> " & objid
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.OrderReview Then
                    Dim sor As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, objid)
                    If Not sor Is Nothing AndAlso Not String.IsNullOrEmpty(sor.OrderNo) Then
                        ltrObjectName.Text = "<b>OrderNo:</b> " & sor.OrderNo
                        Dim member As MemberRow = MemberRow.GetRow(sor.MemberId)
                        If Not member Is Nothing And member.MemberId > 0 Then
                            ltrObjectName.Text &= "<br/><br/><b>Username: </b>" & member.Username
                            ltrObjectName.Text &= "<br/><b>Email: </b>" & member.Customer.Email
                        End If
                    Else
                        ltrObjectName.Text = "<b>OrderId:</b> " & objid
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.FlashBanner Then
                    Dim dbBanner As BannerRow = BannerRow.GetRow(objid)
                    If dbBanner IsNot Nothing AndAlso dbBanner.DepartmentId > 0 Then
                        ltrObjectName.Text = "<b>Department:</b> " & StoreDepartmentRow.GetDepartmentNameByDepertmentId(dbBanner.DepartmentId)
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.StripBanner Then
                    Dim dbProSales As PromotionSalespriceRow = PromotionSalespriceRow.GetRow(DB, objid)
                    If Not dbProSales Is Nothing AndAlso Not String.IsNullOrEmpty(dbProSales.MainTitle) Then
                        ltrObjectName.Text = "<b>Type:</b> " & AdminLogHelper.GetTypeStripBanner(CInt(dbProSales.Type))
                        ltrObjectName.Text &= "<br/><br/><b>Main Title:</b> " & dbProSales.MainTitle
                        ltrObjectName.Text &= "<br/><b>SubTitle:</b> " & dbProSales.SubTitle
                        If dbProSales.DepartmentID > 0 Then
                            ltrObjectName.Text &= "<br/></br><b>Department:</b> " & StoreDepartmentRow.GetDepartmentNameByDepertmentId(dbProSales.DepartmentID)
                        End If
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.Video Then
                    Dim video As VideoRow = VideoRow.GetRow(DB, objid)
                    If Not video Is Nothing AndAlso Not String.IsNullOrEmpty(video.Title) Then
                        ltrObjectName.Text = "<b>Title:</b> " & video.Title
                    Else
                        ltrObjectName.Text = "<b>Id:</b> " & objid
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.Category Then
                    Dim category As CategoryRow = CategoryRow.GetRow(DB, objid)
                    If Not category Is Nothing AndAlso Not String.IsNullOrEmpty(category.CategoryName) Then
                        ltrObjectName.Text = "<b>Category Name:</b> " & category.CategoryName
                        ltrObjectName.Text &= "<br/><b>Type:</b> " & AdminLogHelper.GetTypeCategory(category.Type)
                    Else
                        ltrObjectName.Text = "<b>Id:</b> " & objid
                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.TrackingNumber Then
                    Dim orderNo As String = DB.ExecuteScalar("Select top 1 OrderNo from StoreOrder o left join StoreOrderShipmentTracking tk on (tk.OrderId=o.OrderId) where TrackingId=" & objid)
                    ltrObjectName.Text = "<b>Order No:</b> " & orderNo
                    Dim dtShipping As DataTable = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = (Select top 1 OrderId from StoreOrder where OrderNo='" & orderNo & "') and  type = 'item' and LEFT(Code,3)='UPS')")
                    If Not (dtShipping Is Nothing) Then
                        Dim Name As String = String.Empty
                        If dtShipping.Rows.Count < 1 Then
                            dtShipping = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = (Select top 1 OrderId from StoreOrder where OrderNo='" & orderNo & "') and  type = 'item')")
                        End If
                        If Not dtShipping Is Nothing Then
                            If dtShipping.Rows.Count > 0 Then
                                ltrObjectName.Text &= "<br/><b>Ship Via/ Options:</b> " & dtShipping.Rows(0)("Name")
                            End If

                        End If

                    End If
                ElseIf ObjectType = Utility.Common.ObjectType.Department Then
                    Dim department As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, objid)
                    If Not department Is Nothing AndAlso Not String.IsNullOrEmpty(department.Name) Then
                        ltrObjectName.Text = "<b>Name:</b> " & department.Name
                    Else
                        ltrObjectName.Text = "<b>Id:</b> " & objid
                    End If
                End If

            ElseIf (Action = Utility.Common.AdminLogAction.Insert.ToString()) Then
                If ObjectType = Utility.Common.ObjectType.TrackingNumber Or ObjectType = Utility.Common.ObjectType.Department Then
                    ltrActionDetail.Text = AdminLogHelper.ConvertMessageLogInsertToTextOrderByForm(Message, ObjectType, DB)
                ElseIf ObjectType = Utility.Common.ObjectType.ItemPoint Then
                    Dim objid As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("ObjectID")
                    Dim item As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(objid))
                    If Not item Is Nothing Then
                        If (Not String.IsNullOrEmpty(item.SKU) And Not String.IsNullOrEmpty(item.ItemName)) Then
                            ltrActionDetail.Text = "<b>SKU:</b> " & item.SKU & "<br/>"
                            ltrActionDetail.Text &= "<b>Item Name:</b> " & item.ItemName & "<br/>"
                        End If
                    End If
                    ltrActionDetail.Text &= AdminLogHelper.ConvertMessageLogInsertToText(Message, ObjectType, DB)
                Else
                    ltrActionDetail.Text = AdminLogHelper.ConvertMessageLogInsertToText(Message, ObjectType, DB)
                End If

            ElseIf (Action = Utility.Common.AdminLogAction.Delete.ToString()) Then
                If ObjectType = Utility.Common.ObjectType.FreeSample Or ObjectType = Utility.Common.ObjectType.ItemPoint Then
                    Dim objid As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("ObjectID")
                    Dim item As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(objid))
                    If Not item Is Nothing Then
                        If (Not String.IsNullOrEmpty(item.SKU) And Not String.IsNullOrEmpty(item.ItemName)) Then
                            ltrActionDetail.Text = "<b>SKU:</b> " & item.SKU & "<br/>"
                            ltrActionDetail.Text &= "<b>Item Name:</b> " & item.ItemName & "<br/>"
                        End If
                    End If
                    ltrActionDetail.Text &= AdminLogHelper.ConvertMessageLogUpdateToText(Message, ObjectType, DB)
                Else
                    ltrActionDetail.Text = AdminLogHelper.ConvertMessageLogDeleteToText(Message, ObjectType, DB)
                End If
            End If

            Dim ltrObjectType As Literal = CType(e.Row.FindControl("ltrObjectType"), Literal)
            ltrObjectType.Text = Utility.Common.EnumDescription([Enum].Parse(GetType(Utility.Common.ObjectType), ObjectType))
        End If

    End Sub



    Protected Sub drpAction_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAction.SelectedIndexChanged
        gvList.SortBy = "CreatedDate"
        gvList.SortOrder = "DESC"
        gvList.PageIndex = 0
        BindList()
    End Sub


    Private Sub LoadUsername()
        drpUsername.DataSource = AdminRow.GetAllAdmins(DB)
        drpUsername.DataTextField = "Username"
        drpUsername.DataValueField = "Username"
        drpUsername.DataBind()

        drpUsername.Items.Insert(0, New ListItem("==== ALL ====", ""))

    End Sub

    Protected Sub drpUsername_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpUsername.SelectedIndexChanged
        gvList.SortBy = "CreatedDate"
        gvList.SortOrder = "DESC"
        gvList.PageIndex = 0
        BindList()
    End Sub
End Class



