Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_shopsave_items
    Inherits AdminPage

    Protected ShopSaveId As Integer
    Public Property Name() As String

        Get
            Dim o As Object = ViewState("Name")
            If o IsNot Nothing Then
                Return DirectCast(o, String)
            End If
            Return String.Empty
        End Get

        Set(ByVal value As String)
            ViewState("Name") = value
        End Set
    End Property
    Private Type As Integer
    Private TotalRecords As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf LoadList
        If Request.QueryString("ShopSaveId") <> Nothing AndAlso Request.QueryString("ShopSaveId").Length > 0 Then
            ShopSaveId = CInt(Request.QueryString("ShopSaveId"))
        End If

        If Not IsPostBack Then
            InitParamater()
            LoadList()
            LoadDefault()
        End If
    End Sub
    Private Sub InitParamater()
        Dim pIndex As Integer
        Dim pSize As Integer
        Try
            pIndex = Request.QueryString("pIndex")
        Catch ex As Exception
            pIndex = 0
        End Try
        Try
            pSize = Request.QueryString("pSize")
        Catch ex As Exception
            pSize = 20
        End Try
        If pSize < 1 Then
            pSize = 20
        End If
        ''   pSize = 4
        gvList.PageIndex = pIndex
        gvList.PageSize = pSize
    End Sub
    Private Sub LoadDefault()
        If Request.QueryString("DepartmentName") <> Nothing AndAlso Request.QueryString("DepartmentName").Length > 0 Then
            ltrHeader.Text = "List products of " & Request.QueryString("DepartmentName").Trim()
            If Request.QueryString("TabName") <> Nothing AndAlso Request.QueryString("TabName").Length > 0 Then
                ltrHeader.Text &= " >> " & Request.QueryString("TabName")
                Name = Request.QueryString("TabName")
            End If

        End If

    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            ''  Dim rowView As ShopSaveItemRow = DirectCast(DirectCast(e.Row.DataItem, System.Object), DataLayer.ShopSaveItemRow)
            Dim rowView As DataRowView = e.Row.DataItem
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
            Dim imbDelete As ImageButton = CType(e.Row.FindControl("imbDelete"), ImageButton)
            Dim ItemId As Integer = Convert.ToInt32(rowView("ItemId"))
            Dim isActive As Boolean = Convert.ToBoolean(rowView("IsActive"))
            '' Dim sku As Integer = Convert.ToInt32(rowView("sku"))
            imbUp.CommandArgument = ItemId.ToString
            imbDown.CommandArgument = ItemId.ToString()
            imbActive.CommandArgument = ItemId.ToString()
            imbDelete.CommandArgument = ItemId.ToString()
            If Not (isActive) Then
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            End If
            hidID.Value = hidID.Value & ItemId & ";"
            '' hidPopUpSKU.Value += rowView.SKU + ";"

        End If

    End Sub
    Dim rowIndex As Integer = 0
    Private Function ValidateSource(ByVal ds As DataSet) As DataTable
        If ds Is Nothing Then
            Return Nothing
        End If
        If ds.Tables(0).Rows.Count < 1 Then
            Return Nothing
        End If
        Dim result As New DataTable
        result = ds.Tables(0).Clone()
        Dim startRow As Integer
        Dim endRow As Integer
        startRow = 1
        endRow = (gvList.PageIndex + 1) * gvList.PageSize
        Dim indexRow As Integer = 1
        For Each row As DataRow In ds.Tables(0).Rows
            hidPopUpSKU.Value = hidPopUpSKU.Value & row("sku") & ";"
            If indexRow >= startRow And indexRow <= endRow Then
                Dim copyRow As DataRow = result.NewRow()
                copyRow.ItemArray = row.ItemArray
                result.Rows.Add(copyRow)
            End If
            indexRow = indexRow + 1
        Next
        Return result
    End Function
    Private Sub LoadList()
        hidIDSelect.Value = ""
        hidID.Value = ""
        rowIndex = 0
        hidPopUpSKU.Value = ""
        Dim SQLFields, SQL As String
        Dim Conn As String = " AND "
        ''SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " SSI.*, SI.SKU, SI.ItemName "
        SQLFields = "SELECT SSI.*, SI.SKU, SI.ItemName "
        SQL = " FROM ShopSaveItem SSI INNER JOIN StoreItem SI ON SI.ItemId = SSI.ItemId 	WHERE  SSI.ShopSaveId = " & ShopSaveId & ""

        Dim i As Integer = 0
        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY SSI.Arrange DESC")
        TotalRecords = 0
        If Not res Is Nothing Then
            If Not res.Tables(0) Is Nothing Then
                TotalRecords = res.Tables(0).Rows.Count
            End If
        End If
        gvList.Pager.NofRecords = TotalRecords
        '' TotalRecords = gvList.Pager.NofRecords
        gvList.DataSource = ValidateSource(res)
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

        If TotalRecords > 0 Then
            btnDelete.Visible = True
            btnDeActive.Visible = True
            btnActive.Visible = True
        Else
            btnDelete.Visible = False
            btnDeActive.Visible = False
            btnActive.Visible = False
        End If
        ''Load header
        Dim shopsave As ShopSaveRow = ShopSaveRow.GetRow(DB, ShopSaveId)
        Select Case shopsave.Type
            Case 1
                ltrHeader.Text = "Shop Now"
            Case 2
                ltrHeader.Text = "Save Now"
            Case 6
                ltrHeader.Text = "Weekly Email"
            Case 4
                ltrHeader.Text = "Promotion Section 1"
            Case 5
                ltrHeader.Text = "Promotion Section 2"
            Case Else
                ltrHeader.Text = "Deal of day"
        End Select

        ltrHeader.Text = String.Format("List products of {0} >> {1}", ltrHeader.Text, shopsave.Name)
    End Sub
    Protected Sub btnActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActive.Click
        If (hidIDSelect.Value <> "") Then
            Dim arr() As String = Split(hidIDSelect.Value, ";")
            For Each id As String In arr
                If id <> "" Then
                    ShopSaveItemRow.ChangeIsActiveByValue(DB, ShopSaveId, id, True)
                End If
            Next
        End If
        Utility.CacheUtils.ClearCacheWithPrefix(ShopSaveItemRow.cachePrefixKey)
        Response.Redirect("items.aspx?ShopSaveId=" & ShopSaveId & "&TabName=" & Name & "&pIndex=" & gvList.PageIndex & "&pSize=" & gvList.PageSize)
    End Sub
    Protected Sub btnDeActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeActive.Click
        If (hidIDSelect.Value <> "") Then
            Dim arr() As String = Split(hidIDSelect.Value, ";")
            For Each id As String In arr
                If id <> "" Then
                    ShopSaveItemRow.ChangeIsActiveByValue(DB, ShopSaveId, id, False)
                End If
            Next
        End If
        Utility.CacheUtils.ClearCacheWithPrefix(ShopSaveItemRow.cachePrefixKey)
        Response.Redirect("items.aspx?ShopSaveId=" & ShopSaveId & "&TabName=" & Name & "&pIndex=" & gvList.PageIndex & "&pSize=" & gvList.PageSize)
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If (hidIDSelect.Value <> "") Then
            Dim arr() As String = Split(hidIDSelect.Value, ";")
            For Each id As String In arr
                If id <> "" Then
                    ShopSaveItemRow.Delete(DB, ShopSaveId, id, False)
                End If
            Next
        End If
        Utility.CacheUtils.ClearCacheWithPrefix(ShopSaveItemRow.cachePrefixKey)
        Response.Redirect("items.aspx?ShopSaveId=" & ShopSaveId & "&TabName=" & Name & "&pIndex=" & gvList.PageIndex & "&pSize=" & gvList.PageSize)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        ''Dim tab As New ShopSaveItemRow(DB)
        Dim skuIsNotValid As String = String.Empty
        Dim arr As Array = Split(hidPopUpSKU.Value.Trim(), ";")
        Dim i As Integer
        Dim result As Integer = 0
        For i = 0 To arr.Length - 1
            If arr(i).ToString() <> String.Empty Then
                Dim item As New ShopSaveItemRow
                item.SKU = arr(i).ToString.Trim()
                item.IsActive = True
                item.ShopSaveId = ShopSaveId
                item.CreatedDate = DateTime.Now
                Try
                    result = ShopSaveItemRow.InsertItem(DB, item, False)
                Catch ex As Exception
                    result = 0
                    AddError(ErrHandler.ErrorText(ex))
                End Try
                If result < 1 Then ''insert faild
                    skuIsNotValid = skuIsNotValid + item.SKU + ","
                End If
            End If
        Next

        'Clear cache
        Utility.CacheUtils.ClearCacheWithPrefix(ShopSaveItemRow.cachePrefixKey, "ShopSave_")

        LoadList()
        If skuIsNotValid <> String.Empty Then

            ltrMsg.Text = "Error! SKU: " & skuIsNotValid.Substring(0, skuIsNotValid.Length - 1) & " is invalid or existing"
        Else
            ltrMsg.Text = String.Empty
        End If
    End Sub
  
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        '' System.Threading.Thread.Sleep(3000)
        If e.CommandName = "Delete" Then
            ShopSaveItemRow.Delete(DB, ShopSaveId, e.CommandArgument, True)
        ElseIf e.CommandName = "Active" Then
            ShopSaveItemRow.ChangeIsActive(DB, ShopSaveId, CInt(e.CommandArgument))
        End If
        If e.CommandName = "Up" Then
            ShopSaveItemRow.ChangeArrangeItem(DB, ShopSaveId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            ShopSaveItemRow.ChangeArrangeItem(DB, ShopSaveId, e.CommandArgument, False)
        End If
        Response.Redirect("items.aspx?ShopSaveId=" & ShopSaveId & "&TabName=" & Name & "&pIndex=" & gvList.PageIndex & "&pSize=" & gvList.PageSize)
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?Type=" & ShopSaveRow.GetRow(DB, ShopSaveId).Type)
    End Sub

End Class
