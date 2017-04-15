Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_shopsave_arange
    Inherits AdminPage

    Private TotalRecords As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ListShopSave()
            ListShopNow()
            ListSaveNow()
        End If
    End Sub

    Private Sub ListShopSave()
        Dim collect As ShopSaveCollection = ShopSaveRow.ListAll(DB)
        TotalRecords = collect.Count

        rptShopSave.DataSource = collect
        rptShopSave.DataBind()


        If TotalRecords = 0 Then
            rptShopSave.DataSource = Nothing
            rptShopSave.DataBind()
        End If

    End Sub

    Private Sub ListShopNow()
        Dim collect As ShopSaveCollection = ShopSaveRow.ListByType(DB, 1, -1) '1 = Shop Now
        TotalRecords = collect.Count

        rptShopNow.DataSource = collect
        rptShopNow.DataBind()


        If TotalRecords = 0 Then
            rptShopNow.DataSource = Nothing
            rptShopNow.DataBind()
        End If

    End Sub


    Private Sub ListSaveNow()
        Dim collect As ShopSaveCollection = ShopSaveRow.ListByType(DB, 2, -1) '2 = Save Now
        TotalRecords = collect.Count

        rptSaveNow.DataSource = collect
        rptSaveNow.DataBind()


        If TotalRecords = 0 Then
            rptSaveNow.DataSource = Nothing
            rptSaveNow.DataBind()
        End If

    End Sub



    Protected Sub rptShopSave_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptShopSave.ItemCommand
        If e.CommandName = "Up" Then
            ShopSaveRow.ChangeArrangeTab(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "Down" Then
            ShopSaveRow.ChangeArrangeTab(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "ActiveHomeTab" Then
            ShopSaveRow.ChangeIsTab(DB, e.CommandArgument)
        ElseIf e.CommandName = "Active" Then
            ShopSaveRow.ChangeIsActive(DB, e.CommandArgument)
        End If

        ListShopSave()
        ListSaveNow()
        ListShopNow()
    End Sub

    Protected Sub rptShopSave_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptShopSave.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)
            Dim tab As ShopSaveRow = e.Item.DataItem

            If e.Item.ItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If
            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)


            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

            Dim imbActiveHomeTab As ImageButton = CType(e.Item.FindControl("imbActiveHomeTab"), ImageButton)


            If tab.IsTab Then
                imbActiveHomeTab.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActiveHomeTab.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If
    End Sub


    Protected Sub rptShopNow_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptShopNow.ItemCommand
        If e.CommandName = "Up" Then
            ShopSaveRow.ChangeArrange(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            ShopSaveRow.ChangeArrange(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "ActiveHomeTab" Then
            ShopSaveRow.ChangeIsTab(DB, e.CommandArgument)
        ElseIf e.CommandName = "Active" Then
            ShopSaveRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        ListShopSave()
        ListShopNow()
    End Sub

    Protected Sub rptShopNow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptShopNow.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)
            Dim tab As ShopSaveRow = e.Item.DataItem

            If e.Item.ItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If

            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)

            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

            Dim imbActiveHomeTab As ImageButton = CType(e.Item.FindControl("imbActiveHomeTab"), ImageButton)


            If tab.IsTab Then
                imbActiveHomeTab.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActiveHomeTab.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If
    End Sub

    Protected Sub rptSaveNow_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptSaveNow.ItemCommand
        If e.CommandName = "Up" Then
            ShopSaveRow.ChangeArrange(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            ShopSaveRow.ChangeArrange(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "ActiveHomeTab" Then
            ShopSaveRow.ChangeIsTab(DB, e.CommandArgument)
        ElseIf e.CommandName = "Active" Then
            ShopSaveRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        ListShopSave()
        ListSaveNow()
    End Sub

    Protected Sub rptSaveNow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptSaveNow.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imbUp As ImageButton = CType(e.Item.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Item.FindControl("imbDown"), ImageButton)
            Dim tab As ShopSaveRow = e.Item.DataItem

            If e.Item.ItemIndex = 0 Then
                imbUp.Visible = False
            ElseIf e.Item.ItemIndex = TotalRecords - 1 Then
                imbDown.Visible = False
            End If
            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)


            If tab.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

            Dim imbActiveHomeTab As ImageButton = CType(e.Item.FindControl("imbActiveHomeTab"), ImageButton)


            If tab.IsTab Then
                imbActiveHomeTab.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActiveHomeTab.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If
    End Sub
End Class
