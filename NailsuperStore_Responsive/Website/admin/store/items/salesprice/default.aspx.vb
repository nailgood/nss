Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_salesprice_Index
    Inherits AdminPage

    Protected ItemId As Integer = Nothing
    Protected dbItem As StoreItemRow
    Private salestype, cprice, cqty As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ItemId = Request("ItemId")
        salestype = Request("salestype")
        cprice = Request("cprice")
        cqty = Request("cqty")
        If salestype = 3 Then
            gvList.Columns(4).Visible = False
            tbCase.Visible = True
        Else
            gvList.Columns(5).Visible = False
        End If
        Dim con As String = " SalesType = {0} And "
       
        If ItemId = Nothing Then Response.Redirect("/admin/store/items/")
        dbItem = StoreItemRow.GetRow(DB, ItemId)
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_MemberId.DataSource = DB.GetDataTable("select distinct m.memberid, username from member m inner join salesprice s on m.memberid = s.memberid where s.itemid = " & ItemId)
            F_MemberId.DataValueField = "MemberId"
            F_MemberId.DataTextField = "Username"
            F_MemberId.DataBind()
            F_MemberId.Items.Insert(0, New ListItem("-- ALL --", ""))

            Dim dv As DataView = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB).DefaultView
            Dim GroupIds As String = String.Empty
            Dim dt As DataTable = DB.GetDataTable("select distinct customerpricegroupid from salesprice where itemid = " & ItemId)
            For Each row As DataRow In dt.Rows
                GroupIds &= IIf(GroupIds = String.Empty, "", ",") & row("customerpricegroupid")
            Next

            dv.RowFilter = "customerpricegroupid in " & DB.NumberMultiple(GroupIds)
            F_CustomerPriceGroupId.DataSource = dv
            F_CustomerPriceGroupId.DataValueField = "customerpricegroupid"
            F_CustomerPriceGroupId.DataTextField = "codewithcount"
            F_CustomerPriceGroupId.DataBind()
            F_CustomerPriceGroupId.Items.Insert(0, New ListItem("-- ALL --", ""))

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "SalesPriceId"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " and "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " s.*, i.itemname, (select top 1 username from member where memberid = s.memberid) as username, (select top 1 CustomerPriceGroupCode from customerpricegroup where CustomerPriceGroupId = s.CustomerPriceGroupId) as CustomerPriceGroupCode "
        SQL = " from salesprice s inner join storeitem i on s.itemid = i.itemid where i.itemid = " & ItemId

        If Not F_MemberId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "MemberId = " & DB.Quote(F_MemberId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_CustomerPriceGroupId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "customerpricegroupid = " & DB.Quote(F_CustomerPriceGroupId.SelectedValue)
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(salestype) Then
            SQL = SQL & Conn & " SalesType = " & salestype
            Conn = " AND "
        Else
            SQL = SQL & Conn & " SalesType <> 3"
            Conn = " AND "
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        txtCasePrice.Text = dbItem.CasePrice 'DB.ExecuteScalar("select CasePrice from StoreItem where ItemId=" & ItemId)
        txtCaseQty.Text = dbItem.CaseQty     'DB.ExecuteScalar("select CaseQty from StoreItem where ItemId=" & ItemId)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?cqty=" & cqty & "&cprice=" & cprice & "&salestype=" & salestype & "&ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim CasePrice As Integer = DB.ExecuteScalar("Select CasePrice from StoreItem where ItemId=" & ItemId)
        Dim dbStoreItemBeforUpdate As StoreItemRow = Nothing
        Try
            If Not String.IsNullOrEmpty(txtCasePrice.Text) AndAlso CInt(txtCasePrice.Text) > 0 Then
                dbStoreItemBeforUpdate = StoreItemRow.GetRow(DB, ItemId)
                Dim changeLog As String = String.Empty
                DB.ExecuteSQL("Update StoreItem set CasePrice = " & txtCasePrice.Text & ", ModifyDate=GetDate() where ItemId = " & ItemId)
                StoreItemRowBase.ClearItemCache(ItemId)
                Dim dbStoreItem As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                changeLog = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.StoreItem, dbStoreItemBeforUpdate, dbStoreItem)
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectId = dbStoreItem.ItemId
                logDetail.ObjectType = Utility.Common.ObjectType.StoreItem.ToString()
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
        Catch ex As Exception

        End Try

        Response.Redirect("default.aspx?cprice=0&cqty=0&salestype=3&ItemId=" & ItemId)
    End Sub

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click
        Dim CaseQty As Integer = DB.ExecuteScalar("Select CaseQty from StoreItem where ItemId=" & ItemId)
        Dim dbStoreItemBeforUpdate As StoreItemRow = Nothing
        Try
            dbStoreItemBeforUpdate = StoreItemRow.GetRow(DB, ItemId)
            If Not String.IsNullOrEmpty(txtCaseQty.Text) AndAlso CInt(txtCaseQty.Text) > 0 Then
                Dim changeLog As String = String.Empty
                DB.ExecuteSQL("Update StoreItem set CaseQty = " & txtCaseQty.Text & ", ModifyDate=GetDate() where ItemId = " & ItemId)
                StoreItemRowBase.ClearItemCache(ItemId)

                Dim dbStoreItem As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                changeLog = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.StoreItem, dbStoreItemBeforUpdate, dbStoreItem)
                Dim logDetail As New AdminLogDetailRow
                logDetail.ObjectId = dbStoreItem.ItemId
                logDetail.ObjectType = Utility.Common.ObjectType.StoreItem.ToString()
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If
        Catch ex As Exception

        End Try
        Response.Redirect("default.aspx?cprice=0&cqty=0&salestype=3&ItemId=" & ItemId)
    End Sub

End Class
