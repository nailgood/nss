Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_content_navigation_Edit
    Inherits AdminPage

    Protected NavigationId As Integer
    Protected ParentId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        NavigationId = Convert.ToInt32(Request("NavigationId"))
        ParentId = Convert.ToInt32(Request("ParentId"))
        If Not IsPostBack Then
            LoadParentID()
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()

        If NavigationId > 0 Then
            Dim dbContentToolNavigation As ContentToolNavigationRow = ContentToolNavigationRow.GetRow(DB, NavigationId)
            txtTitle.Text = dbContentToolNavigation.Title
            txtURL.Text = dbContentToolNavigation.URL
            drpParentID.SelectedValue = dbContentToolNavigation.ParentId
            drpTarget.SelectedValue = dbContentToolNavigation.Target
            chkIsHidden.Checked = dbContentToolNavigation.IsHidden
        Else
            chkIsHidden.Checked = True
            btnDelete.Visible = False
        End If

        If ParentId > 0 Then
            drpParentID.SelectedValue = ParentId
        End If

    End Sub

    Private Sub LoadParentID()
        Dim collection As ContentToolNavigationCollection = ContentToolNavigationRow.GetListMain()
        If collection IsNot Nothing AndAlso collection.Count > 0 Then
            For Each row As ContentToolNavigationRow In collection
                AddNode(row)
            Next
        End If

        drpParentID.Items.Insert(0, New System.Web.UI.WebControls.ListItem("", String.Empty))
    End Sub

    Private Sub AddNode(ByVal nav As ContentToolNavigationRow)
        If nav.NavigationId = NavigationId Then
            Exit Sub
        End If

        Dim prefix As String = String.Empty
        Dim i As Integer = 0
        For i = 0 To nav.Level Step i + 1
            prefix += " -"
        Next

        drpParentID.Items.Add(New ListItem(prefix + " " + nav.Title, nav.NavigationId.ToString()))
        Dim lstChild As ContentToolNavigationCollection = ContentToolNavigationRow.GetListByParentId(nav.NavigationId)
        If lstChild IsNot Nothing AndAlso lstChild.Count > 0 Then
            For Each row As ContentToolNavigationRow In lstChild
                AddNode(row)
            Next
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbContentToolNavigation As ContentToolNavigationRow

            If NavigationId <> 0 Then
                dbContentToolNavigation = ContentToolNavigationRow.GetRow(DB, NavigationId)
            Else
                dbContentToolNavigation = New ContentToolNavigationRow(DB)
            End If
            dbContentToolNavigation.ParentId = drpParentID.SelectedValue
            dbContentToolNavigation.Title = txtTitle.Text
            dbContentToolNavigation.URL = txtURL.Text
            dbContentToolNavigation.Target = drpTarget.SelectedValue
            dbContentToolNavigation.Level = 1

            If NavigationId <> 0 Then
                dbContentToolNavigation.Update()
            Else
                NavigationId = dbContentToolNavigation.Insert()
            End If
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?NavigationId=" & NavigationId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

End Class

