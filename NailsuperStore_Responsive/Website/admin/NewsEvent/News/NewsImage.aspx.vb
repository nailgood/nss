
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_NewsEvent_News_NewsImage
    Inherits AdminPage

    Protected NewsId As Integer
    Private Type As Integer
    Private TotalRecords As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("Id") <> Nothing AndAlso Request.QueryString("Id").Length > 0 Then
            NewsId = CInt(Request.QueryString("Id"))
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
        Dim data As New NewsImageRow
        data.Id = NewsId
        data.PageIndex = 1
        data.PageSize = Integer.MaxValue
        data.NewsId = NewsId
        data.OrderDirection = "ASC"
        data.OrderBy = "Arrange"
        data.Condition = " NewsId=" & NewsId
        hidPopUpImageId.Value = ""
        Dim collect As NewsImageCollection = NewsImageRow.ListAllByNewId(DB, data)
        TotalRecords = collect.Count
        imgPath = "/" & Utility.ConfigData.PathSmallNewsImage
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
        Dim arr As Array = Split(hidPopUpImageId.Value.Trim(), ";")
        Dim i As Integer
        Dim result As Integer = 0
        For i = 0 To arr.Length - 1
            If arr(i).ToString() <> String.Empty Then
                Dim item As New NewsImageRow
                item.ImageId = arr(i).ToString.Trim()
                item.IsActive = True
                item.NewsId = NewsId
                Try
                    NewsImageRow.Insert(DB, item, False)
                Catch ex As Exception
                End Try
            End If
        Next

        'Clear cache
        CacheUtils.ClearCacheWithPrefix(NewsImageRow.cachePrefixKey)

        LoadList()
       
    End Sub

    Protected Sub rptItem_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptItem.ItemCommand
        If e.CommandName = "Delete" Then
            NewsImageRow.Delete(DB, e.CommandArgument)
        ElseIf e.CommandName = "Active" Then
            NewsImageRow.ChangeIsActive(DB, CInt(e.CommandArgument))
        End If
        If e.CommandName = "Up" Then
            NewsImageRow.ChangeArrange(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            NewsImageRow.ChangeArrange(DB, e.CommandArgument, False)
        End If

        'Clear cache
        ''CacheUtils.RemoveCacheWithPrefix("ShopSaveItem_")

        LoadList()
    End Sub
    Dim imgPath As String
    Protected Sub rptItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrImage As Literal = CType(e.Item.FindControl("ltrImage"), Literal)

            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim tab As NewsImageRow = e.Item.DataItem
            If (System.IO.File.Exists(Server.MapPath(imgPath) & tab.FileName)) Then
                ltrImage.Text = "<img style='border:none;' src='" + imgPath & tab.FileName + "' alt='" & tab.ImageName & "'/>"
            End If
            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

            'Arrange
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)
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
           
            hidPopUpImageId.Value += tab.ImageId.ToString + ";"
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Response.Redirect("default.aspx?Type=1&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
