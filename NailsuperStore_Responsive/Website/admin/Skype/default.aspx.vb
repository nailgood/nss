Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient
Imports Utility

Partial Class admin_skype_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList

        If Not IsPostBack Then

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Sort"


            BindList()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM ContactSkype  "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "Name LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_Skype.Text = String.Empty Then
            SQL = SQL & Conn & "Skype LIKE " & DB.FilterQuote(F_Skype.Text)
            Conn = " AND "
        End If
        'If Not F_Sort.Text = String.Empty Then
        '    SQL = SQL & Conn & "sort = " & F_Sort.Text
        '    Conn = " AND "
        'End If

        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count ''ContactSkypeRow.GetListContactSkype(DB, "SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder) ''ContactSkypeRow.GetListContactSkype(DB, SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub ClearCache_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearCache.Click
        CacheUtils.ClearCacheWithPrefix("ContactSkype_")
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Save.Click

        If Not IsValid Then Exit Sub

        Try
            DB.BeginTransaction()
            Dim dbContactSkype As ContactSkypeRow
            dbContactSkype = New ContactSkypeRow(DB)
            dbContactSkype.Skype = F_Skype.Text
            dbContactSkype.Name = F_Name.Text
            Try
                dbContactSkype.Sort = GetSort()
            Catch ex As Exception
                dbContactSkype.Sort = 0
            End Try

            dbContactSkype.Insert()
            DB.CommitTransaction()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    Public Function GetSort() As Integer
        Dim res As DataTable = DB.GetDataTable("select top 1 * from contactskype order by sort desc")
        Try
            Return res.Rows(0)("sort") + 1
        Catch ex As Exception
            Return 1
        End Try
    End Function
End Class

