Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_PolicyItem
    Inherits AdminPage

    Protected PolicyId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf LoadList

        If Request.QueryString("id") <> Nothing AndAlso Request.QueryString("id").Length > 0 Then
            PolicyId = CInt(Request.QueryString("id"))
        End If

        If Not IsPostBack Then
            'Load header
            Dim p As PolicyRow = PolicyRow.GetRow(PolicyId)
            ltrHeader.Text = String.Format("List Item of {0}", p.Title)
            hidPopUpSKU.Value = ""

            LoadList()
        End If
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim rowView As DataRowView = e.Row.DataItem
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
            Dim imbDelete As ImageButton = CType(e.Row.FindControl("imbDelete"), ImageButton)
            Dim ItemId As Integer = Convert.ToInt32(rowView("ItemId"))
            imbUp.CommandArgument = ItemId.ToString
            imbDown.CommandArgument = ItemId.ToString()
            imbDelete.CommandArgument = ItemId.ToString()
            hidID.Value = hidID.Value & ItemId & ";"
        End If
    End Sub
    
    Private Sub LoadList()
        Dim dt As DataTable = PolicyItemRow.ListByPolicyId(PolicyId)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gvList.DataSource = dt 'ValidateSource(dt)
            gvList.DataBind()
            'btnDelete.Visible = True
        Else
            divEmpty.Visible = True
            btnDelete.Visible = False
        End If

        If gvList.Rows.Count > 0 Then
            Dim totalPage As Integer = 1
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

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If (hidIDSelect.Value <> "") Then
            Dim arr() As String = Split(hidIDSelect.Value, ";")
            For Each id As String In arr
                If id <> "" Then
                    PolicyItemRow.Delete(DB, PolicyId, id, False)
                End If
            Next
        End If
        Utility.CacheUtils.ClearCacheWithPrefix(PolicyItemRow.cachePrefixKey)
        Response.Redirect("items.aspx?id=" & PolicyId)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        Dim skuIsNotValid As String = String.Empty
        Dim arr As Array = Split(hidPopUpSKU.Value.Trim(), ";")
        Dim i As Integer
        Dim result As Integer = 0
        For i = 0 To arr.Length - 1
            If arr(i).ToString() <> String.Empty Then
                Dim item As New PolicyItemRow
                item.SKU = arr(i).ToString.Trim()
                item.IsActive = True
                item.PolicyId = PolicyId
                item.CreatedDate = DateTime.Now
                Try
                    result = PolicyItemRow.InsertItem(DB, item, False)
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
        Utility.CacheUtils.ClearCacheWithPrefix(PolicyItemRow.cachePrefixKey, "Policy_")

        LoadList()
        If skuIsNotValid <> String.Empty Then

            ltrMsg.Text = "Error! SKU: " & skuIsNotValid.Substring(0, skuIsNotValid.Length - 1) & " is invalid/existing"
        Else
            ltrMsg.Text = String.Empty
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            PolicyItemRow.Delete(DB, PolicyId, e.CommandArgument, True)
        ElseIf e.CommandName = "Up" Then
            PolicyItemRow.ChangeArrangeItem(DB, PolicyId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            PolicyItemRow.ChangeArrangeItem(DB, PolicyId, e.CommandArgument, False)
        End If

        gvList.DataSource = Nothing
        gvList.DataBind()

        LoadList()
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx")
    End Sub

End Class
