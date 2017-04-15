Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_Video_Video_VideoRelated
    Inherits AdminPage

    Protected VideoId As Integer
    Private Type As Integer
    Private TotalRecords As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("VideoId") <> Nothing AndAlso Request.QueryString("VideoId").Length > 0 Then
            VideoId = CInt(Request.QueryString("VideoId"))
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
        Dim data As New VideoRelatedRow
        data.VideoId = VideoId
        data.PageIndex = 1
        data.PageSize = Integer.MaxValue
        data.OrderDirection = "ASC"
        data.OrderBy = "related.Arrange"
        hidPopUpVideoId.Value = ""
        Dim collect As VideoRelatedCollection = VideoRelatedRow.GetByVideoId(DB, data)
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



        ''  ltrHeader.Text = String.Format("List products of {0} >> {1}", ltrHeader.Text, shopsave.Name)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        Dim arr As Array = Split(hidPopUpVideoId.Value.Trim(), ";")
        Dim i As Integer
        Dim result As Integer = 0
        For i = 0 To arr.Length - 1
            If arr(i).ToString() <> String.Empty Then
                Dim item As New VideoRelatedRow
                item.VideoRelatedId = arr(i).ToString.Trim()
                item.VideoId = VideoId
                Try
                    If (item.VideoId <> item.VideoRelatedId) Then
                        VideoRelatedRow.Insert(DB, item, False)
                    End If
                Catch ex As Exception
                End Try
            End If
        Next

        'Clear cache
        Utility.CacheUtils.ClearCacheWithPrefix(VideoRelatedRow.cachePrefixKey)

        LoadList()

    End Sub

    Protected Sub rptItem_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptItem.ItemCommand
        If e.CommandName = "Delete" Then
            VideoRelatedRow.Delete(DB, VideoId, e.CommandArgument)
        ElseIf  e.CommandName = "Up" Then
            VideoRelatedRow.ChangeArrange(DB, VideoId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            VideoRelatedRow.ChangeArrange(DB, VideoId, e.CommandArgument, False)
        End If

        LoadList()
    End Sub
    Dim imgPath As String
    Protected Sub rptItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrImage As Literal = CType(e.Item.FindControl("ltrImage"), Literal)

            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim tab As VideoRelatedRow = e.Item.DataItem
            Dim video As VideoRow = VideoRow.GetRow(DB, tab.VideoRelatedId)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)
            'Arrange
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim ltrName As Literal = CType(e.Item.FindControl("ltrName"), Literal)
            ltrName.Text = video.Title
            If TotalRecords < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                If e.Item.ItemIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                    imbDown.Visible = False
                End If
            End If

            hidPopUpVideoId.Value += tab.VideoRelatedId.ToString + ";"
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Response.Redirect("default.aspx?Type=1&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

