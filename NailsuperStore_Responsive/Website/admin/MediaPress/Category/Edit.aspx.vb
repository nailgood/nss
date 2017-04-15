Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_MediaPress_Category_Edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim dbCategory As CategoryRow = CategoryRow.GetRow(DB, Id)
        txtName.Text = dbCategory.CategoryName
        chkIsActive.Checked = dbCategory.IsActive
        txtMetaDescription.Text = dbCategory.MetaDescription
        txtMetaKeyword.Text = dbCategory.MetaKeyword
        txtPageTitle.Text = dbCategory.PageTitle
        ''txtLinkDetail.Text = dbCategory.LinkDetail
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try

                Dim dbCategoryRow As CategoryRow
                If Id <> 0 Then
                    dbCategoryRow = CategoryRow.GetRow(DB, Id)
                Else
                    dbCategoryRow = New CategoryRow(DB)
                End If
                dbCategoryRow.CategoryName = txtName.Text.Trim()
                dbCategoryRow.IsActive = chkIsActive.Checked
                dbCategoryRow.MetaKeyword = txtMetaKeyword.Text.Trim()
                dbCategoryRow.MetaDescription = txtMetaDescription.Text.Trim()
                dbCategoryRow.PageTitle = txtPageTitle.Text.Trim()
                dbCategoryRow.LinkDetail = Nothing
                dbCategoryRow.Type = Utility.Common.CategoryType.MediaPress
                dbCategoryRow.Banner = String.Empty
                If Id <> 0 Then
                    CategoryRow.Update(DB, dbCategoryRow)
                Else
                    CategoryRow.Insert(DB, dbCategoryRow)
                End If
                Response.Redirect("default.aspx")
            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If
    End Sub
    Private Sub ViewError(ByVal message As String)
        lblMessage.Text = "<span class='red'>" + message + "</span>"
    End Sub
    Private Function ValidateData() As Boolean
        ''check reuire StartDate
        Dim message As String = String.Empty
        If txtName.Text = String.Empty Then
            message = "Field 'Category Name' is blank"
            ViewError(message)
            Return False
        End If
        Return True
    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

End Class
