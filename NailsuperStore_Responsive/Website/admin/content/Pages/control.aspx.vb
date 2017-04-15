Imports Components
Imports System.Data
Imports DataLayer
Partial Class admin_content_Pages_control
    Inherits AdminPage

    Public PageId As Integer
    Public region As String
    Public ltrtitle As String = ""
    Public pageURL As String = ""
    Dim total As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("PageId") Is Nothing AndAlso Not Request.QueryString("Region") Is Nothing Then
            PageId = Request.QueryString("PageId")
            region = Request.QueryString("Region")
        End If
        If Not IsPostBack Then
            LoadData()
        End If
        Dim page As ContentToolPageRow = ContentToolPageRow.GetRow(DB, PageId)
        PageURL = page.PageURL
        If region = "CT_Left" Then
            ltrtitle = "left column of " & pageURL
        Else
            ltrtitle = "right column of " & pageURL
        End If
    End Sub

    Private Sub LoadData()
        hidIDSelect.Value = ""
        hidControlId.Value = ""
        Dim lstcontrol As ContentToolControlCollection = ContentToolControlRow.ListAll()
        dlControl.DataTextField = "Name"
        dlControl.DataValueField = "Id"
        dlControl.DataSource = lstcontrol
        dlControl.DataBind()
        dlControl.Items.Insert(0, New ListItem("-- All --", String.Empty))
        Dim result As ContentToolPageRegionControlCollection = ContentToolPageRegionControlRow.LoadListByPageId(PageId, region)
        total = result.Count()
        gvList.DataSource = result
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Active" Then
            ContentToolPageRegionControlRow.ChangeActive(e.CommandArgument, pageURL, region)
        ElseIf e.CommandName = "Delete" Then
            ContentToolPageRegionControlRow.Delete(e.CommandArgument, pageURL, region)
        ElseIf e.CommandName = "Up" Then
            ContentToolPageRegionControlRow.ChangeSortOrder(e.CommandArgument, True, pageURL, region)
        ElseIf e.CommandName = "Down" Then
            ContentToolPageRegionControlRow.ChangeSortOrder(e.CommandArgument, False, pageURL, region)
        End If
        Response.Redirect("control.aspx?PageId=" & PageId & "&Region=" & region & "&" & GetPageParams())
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim item As ContentToolPageRegionControlRow = e.Row.DataItem
            Dim ltControls As Literal = e.Row.FindControl("ltControls")
            Dim imbActive As ImageButton = e.Row.FindControl("imbActive")
            Dim imbUp As ImageButton = e.Row.FindControl("imbUp")
            Dim imbDown As ImageButton = e.Row.FindControl("imbDown")
            ltControls.Text = item.Name
            hidControlId.Value = hidControlId.Value & item.PageRegionControlId & ","
            If (item.IsActive) Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            If e.Row.RowIndex = 0 AndAlso total > 1 Then
                imbUp.Visible = False
            ElseIf e.Row.RowIndex = total - 1 AndAlso total > 1 Then
                imbDown.Visible = False
            ElseIf total < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim item As New ContentToolPageRegionControlRow
            If dlControl.SelectedValue <> Nothing Then
                item.ControlId = dlControl.SelectedValue
            Else
                lblControl.Text = "Please select control!"
                Exit Sub
            End If
            If txtParam.Text <> Nothing Then
                item.Param = txtParam.Text
            End If
            item.PageId = PageId
            item.Region = region
            item.IsActive = True
            ContentToolPageRegionControlRow.Insert(item, pageURL, region)
            Response.Redirect("control.aspx?PageId=" & PageId & "&Region=" & region & "&" & GetPageParams())
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim arr As String() = hidIDSelect.Value.Split(",")
        If arr.Length > 0 Then
            For i As Integer = 0 To arr.Length - 1
                If arr(i) <> String.Empty Then
                    ContentToolPageRegionControlRow.Delete(CInt(arr(i)), pageURL, region)
                End If
            Next
        End If
        LoadData()
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?" & GetPageParams())
    End Sub

    Protected Sub btnActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActive.Click
        Dim arr As String() = hidIDSelect.Value.Split(",")
        If arr.Length > 0 Then
            For i As Integer = 0 To arr.Length - 1
                If arr(i) <> String.Empty Then
                    ContentToolPageRegionControlRow.ChangeIsActive(CInt(arr(i)), True, pageURL, region)
                End If
            Next
        End If
        LoadData()
    End Sub

    Protected Sub btnDeActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeActive.Click
        Dim arr As String() = hidIDSelect.Value.Split(",")
        If arr.Length > 0 Then
            For i As Integer = 0 To arr.Length - 1
                If arr(i) <> String.Empty Then
                    ContentToolPageRegionControlRow.ChangeIsActive(CInt(arr(i)), False, pageURL, region)
                End If
            Next
        End If
        LoadData()
    End Sub
End Class
