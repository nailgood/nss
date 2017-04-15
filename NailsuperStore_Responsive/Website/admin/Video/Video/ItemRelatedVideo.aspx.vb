Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility
Partial Class admin_Video_Video_ItemRelatedVideo
    Inherits AdminPage

    Protected VideoId As Integer
    ''Private Type As Integer
    Private TotalRecords As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("VideoId") <> Nothing AndAlso Request.QueryString("VideoId").Length > 0 Then
            VideoId = CInt(Request.QueryString("VideoId"))
        End If
        If Not IsPostBack Then
            LoadList()
        End If
    End Sub

    Private Sub LoadList()
        Dim data As New ItemRelatedVideoRow
        data.VideoId = VideoId
        data.PageIndex = 1
        data.PageSize = Integer.MaxValue
        data.OrderBy = "related.Arrange"
        data.OrderDirection = "ASC"
        hidPopUpVideoId.Value = ""
        Dim collect As ItemRelatedVideoCollection = ItemRelatedVideoRow.GetItemByVideoId(DB, data)
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


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?Type=1&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Dim arr As Array = Split(hidPopUpVideoId.Value.Trim(), ";")
        Dim i As Integer = 0
        For i = 0 To arr.Length - 1
            If arr(i).ToString <> "undefined" AndAlso arr(i).ToString() <> String.Empty Then
                Dim item As New ItemRelatedVideoRow
                item.ItemId = arr(i).ToString().Trim()
                item.VideoId = VideoId
                Try
                    If (item.VideoId <> item.ItemId) Then
                        ItemRelatedVideoRow.Insert(DB, item, False)
                    End If
                Catch ex As Exception
                End Try
            End If
        Next
        'clear cache
        Utility.CacheUtils.ClearCacheWithPrefix(ItemRelatedVideoRow.cachePrefixKey)
        LoadList()
    End Sub

    Protected Sub rptItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim tab As ItemRelatedVideoRow = e.Item.DataItem
            Dim item As StoreItemRow = StoreItemRow.GetRow(DB, tab.ItemId)

            Dim ltrSKU As Literal = CType(e.Item.FindControl("ltrSKU"), Literal)
            ltrSKU.Text = item.SKU
            Dim ltrName As Literal = CType(e.Item.FindControl("ltrName"), Literal)
            ltrName.Text = item.ItemName
            Dim chkIsActive As CheckBox = CType(e.Item.FindControl("chkIsActive"), CheckBox)
            If item.IsActive = True Then
                chkIsActive.Checked = True
            Else
                chkIsActive.Checked = False
            End If

            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)

            If TotalRecords < 2 Then
                imbDown.Visible = False
                imbUp.Visible = False
            Else
                If e.Item.ItemIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                    imbDown.Visible = False
                End If
            End If
            'hidPopUpVideoId.Value += TAB.ItemId.ToString() + ";"
            hidPopUpVideoId.Value = hidPopUpVideoId.Value & tab.ItemId.ToString() + ";"
        End If
    End Sub

    Protected Sub rptItem_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptItem.ItemCommand
        If e.CommandName = "Delete" Then
            ItemRelatedVideoRow.Delete(DB, VideoId, e.CommandArgument)
        ElseIf e.CommandName = "Up" Then
            ItemRelatedVideoRow.ChangeArrange(DB, VideoId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            ItemRelatedVideoRow.ChangeArrange(DB, VideoId, e.CommandArgument, False)
        End If
        LoadList()
    End Sub
End Class
