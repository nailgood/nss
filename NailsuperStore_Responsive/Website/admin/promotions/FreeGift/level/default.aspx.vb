
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_promotions_FreeGift_level_default
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            BindList()
        End If
    End Sub

    Private Sub BindList()

        Dim res As DataSet = DB.GetDataSet("Select Id,Name,MinValue,MaxValue,IsActive,coalesce(Banner,'') as Banner  from FreeGiftLevel order by MinValue ASC")
        If Not res Is Nothing AndAlso Not res.Tables(0) Is Nothing Then
            gvList.Pager.NofRecords = res.Tables(0).Rows.Count
        End If
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Protected Sub chkIsActive_Checked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        Dim id As Integer = chk.ToolTip
        FreeGiftLevelRow.ChangeActive(id)
        Response.Redirect(Me.Request.RawUrl)
    End Sub
    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If

        'Dim ltrImage As Literal = CType(e.Row.FindControl("ltrImage"), Literal)
        'Dim Banner As String = e.Row.DataItem("Banner")
        'If Not String.IsNullOrEmpty(Banner) Then
        '    ltrImage.Text = "<img src='" & Utility.ConfigData.PathFreeGiftLevelBanner & "/thumb/" & Banner & "?t=" & DateTime.Now.ToString() & "' width='100px'/>"
        'End If

        Dim chkIsActive As CheckBox = CType(e.Row.FindControl("chkIsActive"), CheckBox)
        Dim isActive As Boolean = CBool(e.Row.DataItem("IsActive"))
        chkIsActive.Checked = isActive

    End Sub
End Class

