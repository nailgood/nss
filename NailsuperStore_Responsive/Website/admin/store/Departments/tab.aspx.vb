Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Controls

Public Class admin_store_departments_tab
    Inherits AdminPage

    Protected DepartmentTabId As Integer
    Private TotalRecords As Integer
    '' Private PathFolderImage As String = Utility.ConfigData.DepartmentTabFolder
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If Not IsPostBack Then
            LoadDefault()
        
        End If
    End Sub

    Private Sub LoadDefault()

        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartmentTab.DataSource = ds
        ddlDepartmentTab.DataTextField = "AlternateName"
        ddlDepartmentTab.DataValueField = "DepartmentId"
        ddlDepartmentTab.DataBind()
        ddlDepartmentTab.Items.Insert(0, "")

       
        If Request.QueryString("DepartmentId") <> Nothing AndAlso Request.QueryString("DepartmentId").Length > 0 Then
            ddlDepartmentTab.SelectedValue = Request.QueryString("DepartmentId")
            ddlDepartmentTab_SelectedIndexChanged(Nothing, Nothing)
        End If

        ds.Dispose()
    End Sub


    Protected Sub ddlDepartmentTab_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartmentTab.SelectedIndexChanged

        If ddlDepartmentTab.SelectedValue <> "" Then
            Dim collect As DepartmentTabCollection = DepartmentTabRow.ListByDepartmentId(DB, CInt(ddlDepartmentTab.SelectedValue))
            TotalRecords = collect.Count
            rptDepartmentTab.DataSource = collect
            rptDepartmentTab.DataBind()

            If rptDepartmentTab.Items.Count = 0 Then
                divEmpty.Visible = True
                rptDepartmentTab.DataSource = Nothing
                rptDepartmentTab.DataBind()

            Else
                divEmpty.Visible = False
            End If

            btnAddNew.Visible = True
        Else
            rptDepartmentTab.DataSource = Nothing
            rptDepartmentTab.DataBind()

            btnAddNew.Visible = False
            divEmpty.Visible = False
        End If

    End Sub



  

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Response.Redirect("tabEdit.aspx?DepartmentId=" & ddlDepartmentTab.SelectedValue.ToString())
    End Sub

    Protected Sub rptDepartmentTab_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptDepartmentTab.ItemCommand
        If e.CommandName = "Delete" Then
            Dim img As String = DB.ExecuteScalar("Select COALESCE(Image,'') from DepartmentTab where DepartmentTabId=" & e.CommandArgument)
            If (DepartmentTabRow.Delete(e.CommandArgument)) Then

                ''delete image
                If Not String.IsNullOrEmpty(img) Then
                    Dim f As New FileUpload
                    f.RemoveFileName(Utility.ConfigData.DepartmentTabImageFolder, img)
                End If
                Utility.CacheUtils.RemoveCache(StoreDepartmentRow.cachePrefixKey & "LoadListMainPage_" & ddlDepartmentTab.SelectedValue.ToString())
                ddlDepartmentTab_SelectedIndexChanged(Nothing, Nothing)
                '' f.RemoveFileName(Utility.ConfigData.PathPromotionMobile, PmSalse.MobileImage)
            End If

        ElseIf e.CommandName = "Edit" Then

            DepartmentTabId = e.CommandArgument
            Response.Redirect("tabEdit.aspx?DepartmentTabId=" & DepartmentTabId)
        ElseIf e.CommandName = "Active" Then

            DepartmentTabRow.ChangeIsActive(DB, e.CommandArgument)
            Utility.CacheUtils.RemoveCache(StoreDepartmentRow.cachePrefixKey & "LoadListMainPage_" & ddlDepartmentTab.SelectedValue.ToString())

            ddlDepartmentTab_SelectedIndexChanged(Nothing, Nothing)
        ElseIf e.CommandName = "Down" Then
            DepartmentTabRow.ChangeArrange(DB, e.CommandArgument, False)
            Utility.CacheUtils.RemoveCache(StoreDepartmentRow.cachePrefixKey & "LoadListMainPage_" & ddlDepartmentTab.SelectedValue.ToString())

            ddlDepartmentTab_SelectedIndexChanged(Nothing, Nothing)
        ElseIf e.CommandName = "Up" Then
            DepartmentTabRow.ChangeArrange(DB, e.CommandArgument, True)
            Utility.CacheUtils.RemoveCache(StoreDepartmentRow.cachePrefixKey & "LoadListMainPage_" & ddlDepartmentTab.SelectedValue.ToString())
            ddlDepartmentTab_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Protected Sub rptDepartmentTab_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDepartmentTab.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim tab As DepartmentTabRow = e.Item.DataItem

            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)

            If e.Item.ItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If
        End If
    End Sub
End Class
