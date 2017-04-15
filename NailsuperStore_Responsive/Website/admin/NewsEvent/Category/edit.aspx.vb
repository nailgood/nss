

Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_NewsEvent_Category_edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadCategoryFromDB()
        End If

    End Sub

    Private Sub LoadCategoryFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim dbCategory As CategoryRow = CategoryRow.GetRow(DB, Id)
        txtName.Text = dbCategory.CategoryName
        chkIsActive.Checked = dbCategory.IsActive
        txtMetaDescription.Text = dbCategory.MetaDescription
        txtMetaKeyword.Text = dbCategory.MetaKeyword
        txtPageTitle.Text = dbCategory.PageTitle
        'txtLinkDetail.Text = dbCategory.LinkDetail
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try

                Dim dbCategoryRow As CategoryRow
                Dim dbCategoryBefore As New CategoryRow
                Dim logDetail As New AdminLogDetailRow
                If Id <> 0 Then
                    dbCategoryRow = CategoryRow.GetRow(DB, Id)
                    dbCategoryBefore = CloneObject.Clone(dbCategoryRow)
                Else
                    dbCategoryRow = New CategoryRow(DB)
                End If
                dbCategoryRow.CategoryName = txtName.Text.Trim()
                dbCategoryRow.IsActive = chkIsActive.Checked
                dbCategoryRow.MetaKeyword = txtMetaKeyword.Text.Trim()
                dbCategoryRow.MetaDescription = txtMetaDescription.Text.Trim()
                dbCategoryRow.PageTitle = txtPageTitle.Text.Trim()
                'dbCategoryRow.LinkDetail = txtLinkDetail.Text.Trim.Trim()
                dbCategoryRow.Type = 1
                dbCategoryRow.Banner = String.Empty
                If Id <> 0 Then
                    CategoryRow.Update(DB, dbCategoryRow)
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                    logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Category, dbCategoryBefore, dbCategoryRow)
                Else
                    CategoryRow.Insert(DB, dbCategoryRow)
                    logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                    logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbCategoryRow, Utility.Common.ObjectType.Category)
                End If
                logDetail.ObjectId = dbCategoryRow.CategoryId
                logDetail.ObjectType = Utility.Common.ObjectType.Category.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
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
