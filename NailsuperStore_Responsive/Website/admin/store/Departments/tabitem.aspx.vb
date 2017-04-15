Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_store_departments_tabitem
    Inherits AdminPage

    Protected DepartmentTabId As Integer
    Private DepartmentId As Integer
    Private TotalRecords As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("DepartmentTabId") <> Nothing AndAlso Request.QueryString("DepartmentTabId").Length > 0 Then
            DepartmentTabId = CInt(Request.QueryString("DepartmentTabId"))
        End If

        If Not IsPostBack Then
            LoadList()
            LoadDefault()
        End If
    End Sub

    Private Sub LoadDefault()
        If Request.QueryString("DepartmentName") <> Nothing AndAlso Request.QueryString("DepartmentName").Length > 0 Then
            ltrHeader.Text = "List products of " & Request.QueryString("DepartmentName").Trim()
            If Request.QueryString("TabName") <> Nothing AndAlso Request.QueryString("TabName").Length > 0 Then
                ltrHeader.Text &= " >> " & Request.QueryString("TabName")
            End If

        End If

    End Sub

    Private Sub LoadList()
        hidPopUpSKU.Value = ""
        Dim collect As DepartmentTabItemCollection = DepartmentTabItemRow.ListByTabId(DB, DepartmentTabId)
        TotalRecords = collect.Count
        rptItem.DataSource = collect
        rptItem.DataBind()

        If rptItem.Items.Count = 0 Then
            divEmpty.Visible = True
            rptItem.DataSource = Nothing
            rptItem.DataBind()
        Else
            divEmpty.Visible = False
        End If
    End Sub

  

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        If hidPopUpSKU.Value = "" Then Exit Sub
        Dim tab As New DepartmentTabItemRow(DB)

        Try
            tab.DepartmentTabId = DepartmentTabId
            Dim result As Integer
            Dim errorMess As String = String.Empty
            Dim arraySKU() As String = Split(hidPopUpSKU.Value, ";")
            For i As Integer = 0 To arraySKU.Length - 1
                If arraySKU(i).ToString() <> String.Empty Then
                    result = tab.Insert(arraySKU(i).ToString(), False)
                    If result < 0 Then
                        errorMess = arraySKU(i).ToString() + ","
                    End If
                End If
            Next
            If errorMess <> String.Empty Then
                errorMess = errorMess.Substring(0, errorMess.Length - 1)
                ltrMsg.Text = "Error! SKU " & errorMess & " is invalid or existing"
            End If
            Utility.CacheUtils.ClearCacheWithPrefix(DepartmentTabItemRow.cachePrefixKey)
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
        LoadList()
    End Sub

    Protected Sub rptItem_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptItem.ItemCommand

        If e.CommandName = "Delete" Then
            DepartmentTabItemRow.Delete(DB, DepartmentTabId, e.CommandArgument)
        ElseIf e.CommandName = "Active" Then
            DepartmentTabItemRow.ChangeIsActive(DB, DepartmentTabId, CInt(e.CommandArgument))
        End If
        If e.CommandName = "Up" Then
            DepartmentTabItemRow.ChangeArrange(DB, DepartmentTabId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            DepartmentTabItemRow.ChangeArrange(DB, DepartmentTabId, e.CommandArgument, False)
        End If
        LoadList()

    End Sub

    Protected Sub rptItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim tab As DepartmentTabItemRow = e.Item.DataItem

            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            'Arrange
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)

            If e.Item.ItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If
            hidPopUpSKU.Value += tab.SKU + ";"
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("tab.aspx?DepartmentId=" & DepartmentTabRow.GetRow(DB, DepartmentTabId).DepartmentId)
    End Sub
End Class
