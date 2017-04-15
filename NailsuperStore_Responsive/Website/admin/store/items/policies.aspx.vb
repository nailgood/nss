Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility
Partial Class admin_store_items_policies
    Inherits AdminPage
    Protected itemId As Integer
    Protected itemName As String

    Private TotalRecords As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf LoadList
        If Request.QueryString("itemId") <> Nothing Then
            itemId = CInt(Request.QueryString("itemId"))
        End If
        If Request.QueryString("itemName") <> Nothing Then
            itemName = Request.QueryString("itemName")
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
        If Request.QueryString("itemName") <> Nothing Then
            ltrHeader.Text = "List policies of " & Request.QueryString("itemName").Trim()
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
            imbUp.CommandArgument = rowView("policyId")
            imbDown.CommandArgument = rowView("policyId")
            imbActive.CommandArgument = rowView("policyId")
            imbDelete.CommandArgument = rowView("policyId")
            If Not (isActive) Then
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            End If
            'hidID.Value = hidID.Value & ItemId & ";"
            '' hidPopUpSKU.Value += rowView.SKU + ";"

            hidID.Value = hidID.Value & rowView("policyId") & ";"
        End If

    End Sub
    Dim rowIndex As Integer = 0

    Private Sub LoadList()
        hidIDSelect.Value = ""
        hidID.Value = ""
        rowIndex = 0
        hidPopUpItemId.Value = ""

        Dim ds = DB.GetDataSet(String.Format("select * from" & _
                                "( " & _
                                   " select row_number() over (order by Arrange, Title) as id" & _
                                        ", a.PolicyId, a.ItemId, Arrange, b.Title, b.IsActive" & _
                                   " from PolicyItem a " & _
                                      "  inner join Policy b on a.PolicyId = b.PolicyId " & _
                                   " where a.ItemId = {0} " & _
                               " ) a " & _
                           " where a.id BETWEEN {1} * {2} + 1 and {1} * {2} + {2} " & _
                          "  order by a.id; select count(*) from PolicyItem where ItemId = {0}", itemId, gvList.PageIndex, gvList.PageSize))


        TotalRecords = 0
        If Not ds Is Nothing Then
            If Not ds.Tables(1) Is Nothing Then
                TotalRecords = CInt(ds.Tables(1).Rows(0)(0))
            End If
        End If
        gvList.Pager.NofRecords = TotalRecords
        '' TotalRecords = gvList.Pager.NofRecords
        gvList.DataSource = ds.Tables(0)
        gvList.DataBind()

        If gvList.Rows.Count > 0 Then
            Dim totalPage As Integer = 1
            Dim imbDownLast As ImageButton = gvList.Rows(gvList.Rows.Count - 1).FindControl("imbDown")
            imbDownLast.Visible = False

            Dim imbUpFirst As ImageButton = gvList.Rows(0).FindControl("imbUp")
            imbUpFirst.Visible = False

            btnDelete.Visible = True
        End If
        'If (gvList.Rows.Count > 0) Then
        '    Dim totalPage As Integer = gvList.Pager.NofRecords \ gvList.PageSize
        '    If (gvList.Pager.NofRecords Mod gvList.PageSize <> 0) Then
        '        totalPage = totalPage + 1
        '    End If
        '    Dim imbDownLast As ImageButton = gvList.Rows(gvList.Rows.Count - 1).FindControl("imbDown")
        '    If totalPage = gvList.PageIndex + 1 Then
        '        If Not imbDownLast Is Nothing Then
        '            imbDownLast.Visible = False
        '        End If
        '    End If
        '    Dim imbUpFirst As ImageButton = gvList.Rows(0).FindControl("imbUp")
        '    If gvList.PageIndex = 0 Then
        '        If Not imbUpFirst Is Nothing Then
        '            imbUpFirst.Visible = False
        '        End If
        '    End If

        'End If

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If (hidIDSelect.Value <> "") Then
            Dim arr() As String = Split(hidIDSelect.Value, ";")
            DB.BeginTransaction()
            If arr.Length > 0 Then
                Try
                    For Each id As String In arr
                        If id <> "" Then
                            Dim policyId = CInt(id)
                            PolicyItemRow.Delete(DB, policyId, itemId, True)
                        End If
                    Next
                    DB.CommitTransaction()
                Catch
                    DB.RollbackTransaction()
                End Try
            End If
        End If
        Response.Redirect("policies.aspx?itemId=" & itemId & "&itemName=" & itemName & "&pIndex=" & gvList.PageIndex & "&pSize=" & gvList.PageSize)
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        Dim policyId As Integer = e.CommandArgument
        If e.CommandName = "Delete" Then
            PolicyItemRow.Delete(DB, policyId, itemId, True)
        ElseIf e.CommandName = "Up" Then
            PolicyItemRow.ChangeArrangeItem(DB, policyId, itemId, True)
        ElseIf e.CommandName = "Down" Then
            PolicyItemRow.ChangeArrangeItem(DB, policyId, itemId, False)
        End If
        Response.Redirect("policies.aspx?itemId=" & itemId & "&itemName=" & itemName & "&pIndex=" & gvList.PageIndex & "&pSize=" & gvList.PageSize)
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        ''Dim tab As New ShopSaveItemRow(DB)
        Dim policyIDs = hidPopUpItemId.Value
        Try
            DB.BeginTransaction()
            PolicyItemRow.InsertItemByPolicyIDsItemId(DB, "'" & policyIDs & "'", itemId, True)
            DB.CommitTransaction()
        Catch
            DB.RollbackTransaction()
        End Try
        'Clear cache

        LoadList()
        'If skuIsNotValid <> String.Empty Then

        '    ltrMsg.Text = "Error! SKU: " & skuIsNotValid.Substring(0, skuIsNotValid.Length - 1) & " is invalid or existing"
        'Else
        '    ltrMsg.Text = String.Empty
        'End If
    End Sub
End Class
