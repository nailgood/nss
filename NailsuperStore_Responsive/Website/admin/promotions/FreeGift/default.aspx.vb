Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_freegift_Index
    Inherits AdminPage
    Dim strIsChecked As String = ""
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'gvList.BindList = AddressOf BindList

        gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
        gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
        If gvList.SortBy = String.Empty Then gvList.SortBy = "FreeGiftId"
        If ViewState("drpLevel") Is Nothing And Not Request("drpLevel") Is Nothing Then
            ViewState("drpLevel") = Request("drpLevel")
        End If
        If Not IsPostBack Then
            LoadFreeGiftLevel()
            BindList()
        End If




    End Sub

    Private Sub LoadFreeGiftLevel()
        Dim dt As DataTable = DB.GetDataTable("Select Id, Name From FreeGiftLevel Where IsActive=1")

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            drpLevel.DataSource = dt.DefaultView
            drpLevel.DataTextField = "Name"
            drpLevel.DataValueField = "Id"
            drpLevel.DataBind()
        End If
        If ViewState("drpLevel") <> Nothing Then
            drpLevel.SelectedValue = ViewState("drpLevel")
        Else
            ViewState("drpLevel") = drpLevel.SelectedValue
        End If

    End Sub


    Protected Sub drpLevel_OnChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpLevel.SelectedIndexChanged
        ViewState("drpLevel") = drpLevel.SelectedValue
        BindList()
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " freegift.*, itemname, SKU,si.ItemId, si.IsActive as ItemIsActive, lv.Name as LevelName,cast(case si.IsFreeGift when 1 then 1 else 0 end as bit) as IsAddCart, CAST(si.ItemId as varchar(10)) + ',' + CAST(FreeGiftId as varchar(10)) as valTemp "
        SQL = " FROM FreeGift freegift inner join storeitem si on (freegift.itemid = si.itemid) "
        SQL &= " left join FreeGiftLevel lv on (freegift.LevelId = lv.Id) "
        SQL &= " WHERE freegift.LevelId=" & ViewState("drpLevel") & " "

        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("FreeGiftId") = True, "Arrange DESC", gvList.SortByAndOrder))
        gvList.DataSource = res
        gvList.DataBind()

        If (gvList.Rows.Count > 0) Then
            Dim totalPage As Integer = gvList.Pager.NofRecords \ gvList.PageSize
            If (gvList.Pager.NofRecords Mod gvList.PageSize <> 0) Then
                totalPage = totalPage + 1
            End If
            Dim imbDownLast As ImageButton = gvList.Rows(gvList.Rows.Count - 1).FindControl("imbDown")
            If totalPage = gvList.PageIndex + 1 Then
                If Not imbDownLast Is Nothing Then
                    imbDownLast.Visible = False
                End If
            End If
            Dim imbUpFirst As ImageButton = gvList.Rows(0).FindControl("imbUp")
            If gvList.PageIndex = 0 Then
                If Not imbUpFirst Is Nothing Then
                    imbUpFirst.Visible = False
                End If
            End If

        End If
    End Sub

    Protected Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim imbIsAddCart As ImageButton = CType(e.Row.FindControl("imbIsAddCart"), ImageButton)
        Dim imbIsActive As ImageButton = CType(e.Row.FindControl("imbIsActive"), ImageButton)
        Dim dr As DataRowView = e.Row.DataItem
        If dr("IsAddCart") Then
            imbIsAddCart.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbIsAddCart.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
        If dr("IsActive") Then
            imbIsActive.ImageUrl = "/includes/theme-admin/images/active.png"
        Else
            imbIsActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
        End If
        If dr("ItemIsActive") IsNot Nothing AndAlso dr("ItemIsActive") = "0" Then
            e.Row.Attributes.Add("style", "background-color:#F7AFAF")
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        Dim FreeGiftId, oldIsFreeGift, ItemId As Integer
        Dim dbFreeGift As FreeGiftRow
        Dim dbFreeGiftOld As FreeGiftRow = Nothing
        Dim si As StoreItemRow = Nothing
        If e.CommandName = "Up" Then
            FreeGiftRow.ChangeArrangeItem(e.CommandArgument, False)
        ElseIf e.CommandName = "Down" Then
            FreeGiftRow.ChangeArrangeItem(e.CommandArgument, True)
        ElseIf e.CommandName = "IsActive" Then
            FreeGiftId = CInt(e.CommandArgument)
           
            If FreeGiftId <> 0 Then
                dbFreeGift = FreeGiftRow.GetRow(DB, FreeGiftId)
                dbFreeGiftOld = CloneObject.Clone(dbFreeGift)
                si = StoreItemRow.GetRow(DB, dbFreeGift.ItemId)
                If dbFreeGift.IsActive Then
                    si.IsFreeGift = 0
                Else
                    si.IsFreeGift = 2
                End If
                si.Update()
            End If
           
            FreeGiftRow.ChangeIsActive(CInt(e.CommandArgument))
            BindList()
            dbFreeGift = FreeGiftRow.GetRow(DB, FreeGiftId)
        ElseIf e.CommandName = "IsAddCart" Then
            Dim val As String() = e.CommandArgument.ToString.Split(",")
            ItemId = CInt(val(0))
            FreeGiftId = val(1)
            If FreeGiftId <> 0 Then
                dbFreeGift = FreeGiftRow.GetRow(DB, FreeGiftId)
                dbFreeGiftOld = CloneObject.Clone(dbFreeGift)
            Else
                dbFreeGift = New FreeGiftRow(DB)
            End If
            si = StoreItemRow.GetRow(DB, ItemId)
            If Not si Is Nothing Then
                oldIsFreeGift = si.IsFreeGift
            End If
            FreeGiftRow.ChangeIsAddCart(ItemId, FreeGiftId)
            si = StoreItemRow.GetRow(DB, ItemId)
            si.CheckFreeGiftItem(Nothing, dbFreeGift, Utility.Common.AdminLogAction.Update.ToString())
            ''si.Update()
            DB.ExecuteScalar("Update StoreItem set IsFreeGift=" & si.IsFreeGift & " where ItemId=" & si.ItemId)
            StoreItemRowBase.ClearItemCache(si.ItemId)

            BindList()
        End If

        Dim logDetail As New AdminLogDetailRow
        logDetail.ObjectType = Utility.Common.ObjectType.FreeGift.ToString()
        logDetail.ObjectId = FreeGiftId
        logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.FreeGift, dbFreeGiftOld, dbFreeGift)
        If e.CommandName = "IsAddCart" Then
            If oldIsFreeGift <> si.IsFreeGift Then
                logDetail.Message = logDetail.Message & "IsAddCart|" & IIf(oldIsFreeGift = 1, "True", "False") & "|" & IIf(si.IsFreeGift = 1, "True", "False") & "[br]"
            End If
        End If

        logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()

        AdminLogHelper.WriteLuceneLogDetail(logDetail)

        Response.Redirect("default.aspx?drpLevel=" & ViewState("drpLevel") & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub btnEditMetaTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditMetaTag.Click
        Dim url As String = "/store/free-gift.aspx"
        Dim objPage As ContentToolPageRow = ContentToolPageRow.GetRowByURL(url)

        Response.Redirect("/admin/content/Pages/register.aspx?PageId=" & objPage.PageId)
    End Sub
End Class

