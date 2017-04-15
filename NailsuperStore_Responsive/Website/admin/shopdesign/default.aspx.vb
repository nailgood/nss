Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Collections.Generic

Partial Class admin_shopdesign_default
    Inherits AdminPage
    Public Total As Integer = 0
    Public categoryId As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindCategory()
            BindList()
        End If
    End Sub

    Private Sub BindCategory()
        Dim lstCate As CategoryCollection = CategoryRow.ListByType(DB, Utility.Common.CategoryType.ShopDesign)
        Dim lstDropDown As New CategoryCollection
        Dim strParent As String = ""
        For i As Integer = 0 To lstCate.Count() - 1
            If lstCate(i).ParentId = 0 Then
                strParent &= lstCate(i).CategoryId & ","
            End If
        Next
        Dim arr As String() = strParent.Substring(0, strParent.Length - 1).Split(",")
        For k As Integer = 0 To arr.Length - 1
            For t As Integer = 0 To lstCate.Count() - 1
                If CInt(arr(k).ToString()) = lstCate(t).CategoryId Then
                    lstDropDown.Add(lstCate(t))
                End If
                If CInt(arr(k).ToString()) = lstCate(t).ParentId Then
                    lstCate(t).CategoryName = "- " & lstCate(t).CategoryName
                    lstDropDown.Add(lstCate(t))
                End If
            Next
        Next
        F_CatId.DataSource = lstDropDown
        F_CatId.DataTextField = "CategoryName"
        F_CatId.DataValueField = "CategoryId"
        F_CatId.DataBind()
        F_CatId.Items.Insert(0, New ListItem("-- All --", String.Empty))
        If Request("F_CatId") <> Nothing Then
            F_CatId.SelectedValue = Request("F_CatId")
        End If
    End Sub


    Private Sub BindList()
        Dim data As New ShopDesignRow
        Dim conditions As String = "c.Type = " & Utility.Common.CategoryType.ShopDesign
        If F_Title.Text <> Nothing Then
            conditions &= " AND s.Title like N'%" & F_Title.Text & "%'"
        End If
        If F_IsActive.SelectedValue <> Nothing Then
            conditions &= " AND s.IsActive = " & F_IsActive.SelectedValue
        End If
        If F_CatId.SelectedValue <> Nothing Then
            categoryId = CInt(F_CatId.SelectedValue)
        End If
        Dim result As List(Of ShopDesignRow) = ShopDesignRow.ListAllAdmin(conditions, categoryId)
        gvList.Pager.NofRecords = result.Count
        Total = result.Count
        gvList.DataSource = result
        gvList.DataBind()
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Response.Redirect("edit.aspx")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data As ShopDesignRow = e.Row.DataItem
            Dim ShopDesignId As Integer = data.ShopDesignId
            Dim ltItem As Literal = e.Row.FindControl("ltItem")
            Dim ltImage As Literal = e.Row.FindControl("ltImage")
            Dim ltVideo As Literal = e.Row.FindControl("ltVideo")
            Dim imbActive As ImageButton = e.Row.FindControl("imbActive")
            Dim imbUp As ImageButton = e.Row.FindControl("imbUp")
            Dim imbDown As ImageButton = e.Row.FindControl("imbDown")

            ltItem.Text = "<a href='item.aspx?ShopDesignId=" & ShopDesignId & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & ShopDesignRow.CountItem(DB, ShopDesignId) & "</a>"
            ltImage.Text = "<a href='media.aspx?ShopDesignId=" & ShopDesignId & "&Type=" & Utility.Common.ShopDesignMediaType.Image & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & ShopDesignRow.CountMedia(DB, ShopDesignId, Utility.Common.ShopDesignMediaType.Image) & "</a>"
            ltVideo.Text = "<a href='media.aspx?ShopDesignId=" & ShopDesignId & "&Type=" & Utility.Common.ShopDesignMediaType.Video & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & ShopDesignRow.CountMedia(DB, ShopDesignId, Utility.Common.ShopDesignMediaType.Video) & "</a>"
            If data.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            If categoryId > 0 AndAlso e.Row.RowIndex = 0 AndAlso Total > 1 Then
                imbUp.Visible = False
            ElseIf categoryId > 0 AndAlso e.Row.RowIndex = Total - 1 AndAlso Total > 1 Then
                imbDown.Visible = False
            ElseIf categoryId = 0 Or Total < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            End If
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        Dim logdetail As New AdminLogDetailRow
        If e.CommandName = "Active" Then
            Dim item As ShopDesignRow = ShopDesignRow.GetRow(DB, e.CommandArgument)
            ShopDesignRow.ChangeActive(e.CommandArgument)
            logdetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logdetail.Message = "IsActive|" & item.IsActive.ToString() & "|" & (Not item.IsActive).ToString() & "[br]"
            logdetail.ObjectId = e.CommandArgument
            logdetail.ObjectType = Utility.Common.ObjectType.ShopDesign.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logdetail)
        ElseIf e.CommandName = "Delete" Then
            Dim itemBefore As ShopDesignRow = ShopDesignRow.GetRow(DB, e.CommandArgument)
            If (ShopDesignRow.Delete(e.CommandArgument)) Then
                logdetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
                logdetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(itemBefore, Utility.Common.ObjectType.ShopDesign)
                logdetail.ObjectId = e.CommandArgument
                logdetail.ObjectType = Utility.Common.ObjectType.ShopDesign.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logdetail)
            End If
        ElseIf e.CommandName = "Up" Then
            ShopDesignRow.ChangeSortOrder(e.CommandArgument, CInt(F_CatId.SelectedValue), False)
        ElseIf e.CommandName = "Down" Then
            ShopDesignRow.ChangeSortOrder(e.CommandArgument, CInt(F_CatId.SelectedValue), True)
        End If
        Response.Redirect("default.aspx?F_CatId=" & F_CatId.SelectedValue)
    End Sub


End Class
