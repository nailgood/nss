

Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_Departments_tabEdit
    Inherits AdminPage

    Protected DepartmentTabId As Integer
  

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DepartmentTabId = Convert.ToInt32(Request("DepartmentTabId"))

        If Not IsPostBack Then
            ViewState("DepartmentId") = Convert.ToInt32(Request("DepartmentId"))
            LoadDefault()
            LoadFromDB()
        End If
    End Sub
    Public Sub ServerValidationTabURLCode(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Dim sql As String = ""

        If DepartmentTabId > 0 Then
            sql = "Select * from DepartmentTab where URLCode='" & args.Value.Trim() & "' and DepartmentId=" & ddlDepartment.SelectedValue & " and DepartmentTabId<>" & DepartmentTabId
        Else
            sql = "Select * from DepartmentTab where URLCode='" & args.Value.Trim() & "' and DepartmentId=" & ddlDepartment.SelectedValue & ""
        End If



        Dim data As DataTable = DB.GetDataTable(sql)
        If data Is Nothing Then
            args.IsValid = True
            Exit Sub
        End If
        If data.Rows.Count > 0 Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If

    End Sub

    Private Sub LoadDefault()

        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartment.DataSource = ds
        ddlDepartment.DataTextField = "AlternateName"
        ddlDepartment.DataValueField = "DepartmentId"
        ddlDepartment.DataBind()
        ddlDepartment.Items.Insert(0, "")
        ddlDepartment.SelectedValue = ViewState("DepartmentId").ToString()
       
    End Sub
    Private Sub LoadFromDB()
        If DepartmentTabId > 0 Then
             Dim tab As DepartmentTabRow = DepartmentTabRow.GetRow(DB, DepartmentTabId)
            txtName.Text = tab.Name
            chkIsActive.Checked = tab.IsActive
            ddlDepartment.SelectedValue = tab.DepartmentId
            txtPageTitle.Text = tab.PageTitle
            txtOutsideUSPageTitle.Text = tab.OutsideUSPageTitle
            txtMetaDescription.Text = tab.MetaDescription
            txtOutsideUSMetaDescription.Text = tab.OutsideUSMetaDescription
            txtMetaKeyword.Text = tab.MetaKeyword
            txtURLCode.Text = tab.URLCode
            fuImage.CurrentFileName = tab.Image
            txtDescription.Text = tab.Description
            ViewState("DepartmentId") = tab.DepartmentId
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Dim tab As New DepartmentTabRow(DB)
        Dim isRedirect As Boolean = False
        Try
            fuImage.Folder = Utility.ConfigData.DepartmentTabImageFolder
            fuImage.ImageDisplayFolder = Utility.ConfigData.DepartmentTabImageFolder
            tab.IsActive = chkIsActive.Checked
            tab.Name = txtName.Text.Trim()
            tab.Image = fuImage.CurrentFileName
            tab.DepartmentId = ddlDepartment.SelectedValue
            tab.PageTitle = txtPageTitle.Text.Trim()
            tab.OutsideUSPageTitle = txtOutsideUSPageTitle.Text.Trim()
            tab.MetaDescription = txtMetaDescription.Text.Trim()
            tab.OutsideUSMetaDescription = txtOutsideUSMetaDescription.Text.Trim()
            tab.MetaKeyword = txtMetaKeyword.Text.Trim()
            tab.URLCode = txtURLCode.Text.Trim()
            tab.Description = txtDescription.Text
            If (DepartmentTabId > 0) Then
                tab.DepartmentTabId = DepartmentTabId
                DepartmentTabItemRow.UpdateDepartmentTab(tab)
                WriteLogDetail("Update  Department Tab", tab)
            Else
                DepartmentTabId = DepartmentTabItemRow.InsertDepartmentTab(tab)
                WriteLogDetail("Insert  Department Tab", tab)
            End If

            If DepartmentTabId > 0 Then

                ''update image
                '' Dim ImageNameNew As String = GetImageName(ddlDepartment.SelectedItem.Text & " " & tab.Name)
                Dim arr As String()
                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = DepartmentTabId.ToString & "." & arr(1)
                    fuImage.SaveNewFile()
                    tab.Image = fuImage.NewFileName
                    Dim sql As String = "Update DepartmentTab set Image='" & tab.Image & "' where DepartmentTabId=" & DepartmentTabId
                    DB.ExecuteSQL(sql)
                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    tab.Image = String.Empty
                    Dim sql As String = "Update DepartmentTab set Image='' where DepartmentTabId=" & DepartmentTabId
                    DB.ExecuteSQL(sql)
                End If
                ''Response.Redirect("tab.aspx?" & GetPageParams(FilterFieldType.All))
                isRedirect = True
                ViewState("DepartmentId") = tab.DepartmentId
                Utility.CacheUtils.RemoveCache(StoreDepartmentRow.cachePrefixKey & "LoadListMainPage_" & tab.DepartmentId)
            End If
        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
        If isRedirect Then
            GoBack()
        End If

    End Sub
    Private Sub GoBack()
        Response.Redirect("tab.aspx?DepartmentId=" & ViewState("DepartmentId"))
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        GoBack()
    End Sub
End Class
