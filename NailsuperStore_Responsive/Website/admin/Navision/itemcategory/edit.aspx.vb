Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_itemCategory_Edit
    Inherits AdminPage

    Protected id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        id = Convert.ToInt32(Request("id"))
        If Not IsPostBack Then
            LoadFromDB()
            LoadDepartments()
        End If
    End Sub

    Private Sub LoadFromDB()
        If id = 0 Then
            Exit Sub
        End If

        Dim dbStoreItemCategory As StoreItemCategoryRow = StoreItemCategoryRow.GetRow(DB, id)
        lblCategory.Text = dbStoreItemCategory.Category
        lblName.Text = dbStoreItemCategory.Name
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreItemCategory As StoreItemCategoryRow

            If id <> 0 Then
                dbStoreItemCategory = StoreItemCategoryRow.GetRow(DB, id)
            Else
                dbStoreItemCategory = New StoreItemCategoryRow(DB)
            End If

            If id <> 0 Then
                dbStoreItemCategory.Update()
            Else
                id = dbStoreItemCategory.Insert
            End If

            dbStoreItemCategory.RemoveDepartmentItems()
            dbStoreItemCategory.InsertDepartmentItems(treeDepartments.CheckedList)

            DB.CommitTransaction()


            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Private Sub LoadDepartments()
        Dim Result As String = ""

        SQL = "SELECT DepartmentId FROM StoreItemCategoryDepartment WHERE CategoryId = " & DB.Quote(id)
        Trace.Write(SQL)
        Dim sConn As String = ""
        Dim res As SqlDataReader = Nothing
        Try
            res = DB.GetReader(SQL)
            While res.Read
                Result = Result & sConn & res("DepartmentId")
                sConn = ","
            End While
        Catch ex As Exception

        End Try
        Core.CloseReader(res)
        treeDepartments.CheckedList = Result
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

