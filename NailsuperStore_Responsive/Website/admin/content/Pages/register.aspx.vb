Option Strict Off

Imports System.Data
Imports System.Data.SqlClient
Imports Components
Imports DataLayer
Imports System.IO

Partial Class admin_content_pages_register
    Inherits AdminPage
    Protected PageId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageId = Convert.ToInt32(Request("PageId"))
        If Not Page.IsPostBack Then
            LoadNavigate()
            LoadPageDB()
        End If
    End Sub

    Private Sub LoadPageDB()
        chkIsShowContent.Checked = False
        trContent.Visible = False
        txtMetaKeywords.Text = SysParam.GetValue("DefaultMetaKeywords")
        txtMetaDescription.Text = SysParam.GetValue("DefaultMetaDescription")
        If (PageId > 0) Then
            Dim objPage As ContentToolPageRow = ContentToolPageRow.GetRow(DB, PageId)
            If Not (objPage Is Nothing) Then
                txtName.Text = objPage.Name
                txtContent.Text = objPage.Content
                txtMetaDescription.Text = objPage.MetaDescription
                txtMetaKeywords.Text = objPage.MetaKeywords
                txtPageURL.Text = objPage.PageURL
                txtNavigationText.Text = objPage.NavigationText
                txtPageTitle.Text = objPage.Title
                txtMetaTitle.Text = objPage.MetaTitle
                If objPage.IsShowContent Then
                    chkIsShowContent.Checked = True
                    trContent.Visible = True
                End If
                chkIsFollowed.Checked = objPage.IsFollowed
                chkIsIndexed.Checked = objPage.IsIndexed
                chkIsFullScreen.Checked = objPage.IsFullScreen
                drlNavigation.SelectedValue = objPage.NavigationId.ToString()
            End If
        End If
    End Sub

    Private Sub LoadNavigate()
        Dim collection As ContentToolNavigationCollection = ContentToolNavigationRow.GetListMain()
        If collection IsNot Nothing AndAlso collection.Count > 0 Then
            For Each row As ContentToolNavigationRow In collection
                AddNode(row)
            Next
        End If

        drlNavigation.Items.Insert(0, New System.Web.UI.WebControls.ListItem("", String.Empty))
    End Sub

    Private Sub AddNode(ByVal nav As ContentToolNavigationRow)
        Dim prefix As String = String.Empty
        Dim i As Integer = 0
        For i = 0 To nav.Level Step i + 1
            prefix += " -"
        Next

        drlNavigation.Items.Add(New ListItem(prefix + " " + nav.Title, nav.NavigationId.ToString()))
        Dim lstChild As ContentToolNavigationCollection = ContentToolNavigationRow.GetListByParentId(nav.NavigationId)
        If lstChild IsNot Nothing AndAlso lstChild.Count > 0 Then
            For Each row As ContentToolNavigationRow In lstChild
                AddNode(row)
            Next
        End If

    End Sub

    Public Function FileExists(ByVal FileFullPath As String) As Boolean
        Dim f As New IO.FileInfo(FileFullPath)
        Return f.Exists
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then
            Exit Sub
        End If

        Dim dbPage As New ContentToolPageRow(DB)
        dbPage.PageURL = txtPageURL.Text.Trim().ToLower()
        dbPage.Name = txtName.Text.Trim()
        dbPage.Title = txtPageTitle.Text.Trim()
        dbPage.MetaTitle = txtMetaTitle.Text.Trim()
        dbPage.MetaKeywords = txtMetaKeywords.Text.Trim()
        dbPage.MetaDescription = txtMetaDescription.Text.Trim()
        dbPage.IsIndexed = chkIsIndexed.Checked
        dbPage.IsFollowed = chkIsFollowed.Checked
        dbPage.IsShowContent = chkIsShowContent.Checked
        dbPage.NavigationText = txtNavigationText.Text.Trim()
        dbPage.IsFullScreen = chkIsFullScreen.Checked
        If chkIsShowContent.Checked Then
            dbPage.Content = txtContent.Text
        End If
        If Not String.IsNullOrEmpty(drlNavigation.SelectedValue) Then
            dbPage.NavigationId = drlNavigation.SelectedValue
        End If

        Dim result As Integer = 0
        If (PageId > 0) Then
            dbPage.PageId = PageId
            result = ContentToolPageRow.Update(dbPage)
        Else
            result = ContentToolPageRow.Insert(dbPage)
        End If
        If (result = -1) Then '' duplicate
            AddError("Page URL is exists.")
        ElseIf (result = 0) Then
            AddError("An error occurred when processing. Please try again.")
        Else
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub chkIsShowContent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsShowContent.CheckedChanged
        If chkIsShowContent.Checked Then
            trContent.Visible = True
        Else
            trContent.Visible = False
        End If
    End Sub
End Class
