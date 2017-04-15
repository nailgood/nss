Imports Components
Imports DataLayer
Imports System.Collections.Generic
Imports Utility

Partial Class services_msds
    Inherits SitePage
    Protected cdn As String = Utility.ConfigData.CDNMediaPath
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If ViewState("SortSku") Is Nothing Then
                ViewState("SortSku") = "ASC"
            End If
            LoadData()
        End If
    End Sub
    Private Sub LoadData()
        Dim list As List(Of StoreItemRow) = StoreItemRow.GetListMSDS(ViewState("SortSku"), ViewState("SortItemName"))
        rpMSDS.DataSource = list
        rpMSDS.DataBind()
    End Sub

    Private Sub rpMSDS_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpMSDS.ItemCommand
        If e.CommandName = "SKU" Then
            If ViewState("SortSku") = "ASC" Then
                ViewState("SortSku") = "DESC"
            Else
                ViewState("SortSku") = "ASC"
            End If
            ViewState("SortItemName") = Nothing
        ElseIf e.CommandName = "ItemName" Then
            If ViewState("SortItemName") = "ASC" Then
                ViewState("SortItemName") = "DESC"
            Else
                ViewState("SortItemName") = "ASC"
            End If
            ViewState("SortSku") = Nothing
        End If
        LoadData()
    End Sub

End Class
