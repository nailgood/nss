Imports System.Collections.Generic
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Linq
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Data.SqlClient

Partial Class admin_Keyword_FilterKeyword_default
    Inherits AdminPage

    Private _dataSource As List(Of KeywordFilter)
    Public Property DataSource() As List(Of KeywordFilter)
        Get
            Return _dataSource
        End Get
        Set(ByVal value As List(Of KeywordFilter))
            _dataSource = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If DataSource Is Nothing Then
            DataSource = KeywordFilter.GetAll()
        End If


        'gvList.BindList = AddressOf BindList()
        If Not IsPostBack Then
            If Request.QueryString("KwSearch") IsNot Nothing Then
                txtSearch.Text = Request.QueryString("KwSearch")
            Else
                txtSearch.Text = String.Empty
            End If
            BindList(DataSource)
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindList(DataSource)
    End Sub
    Private Sub BindList(ByVal list As List(Of KeywordFilter))
        Dim newDataSource As List(Of KeywordFilter) = Nothing
        If Not String.IsNullOrEmpty(txtSearch.Text) Then
            newDataSource = DataSource.Where(Function(i) (i.OriginalKeyword IsNot Nothing AndAlso i.OriginalKeyword.Contains(txtSearch.Text)) Or (i.KeyWordName IsNot Nothing AndAlso i.KeyWordName.Contains(txtSearch.Text)) Or (i.FilterProperty IsNot Nothing AndAlso i.FilterProperty.Contains(txtSearch.Text)) Or (i.FilterValue IsNot Nothing AndAlso i.FilterValue.Contains(txtSearch.Text)) Or i.KeywordId.ToString() = txtSearch.Text Or i.OriginalKeywordId.ToString() = txtSearch.Text).ToList()
        Else
            newDataSource = DataSource
        End If
        hidSortField.Value = ViewState("F_SortBy")
        gvList.Pager.NofRecords = newDataSource.Count
        If hidSortField.Value = "OriginalKeyword" OrElse String.IsNullOrEmpty(hidSortField.Value) Then
            If ViewState("F_SortOrder") = "ASC" Then
                gvList.DataSource = newDataSource.OrderBy(Function(i) i.OriginalKeyword).ToList()
            Else
                gvList.DataSource = newDataSource.OrderByDescending(Function(i) i.OriginalKeyword).ToList()
            End If

        Else
            If ViewState("F_SortOrder") = "ASC" Then
                gvList.DataSource = newDataSource.OrderBy(Function(i) i.KeyWordName).ToList()
            Else
                gvList.DataSource = newDataSource.OrderByDescending(Function(i) i.KeyWordName).ToList()
            End If

        End If

        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?kwSearch=" & txtSearch.Text & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If



    End Sub
    Private Sub btnSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSort.Click
        SortList(hidSortField.Value)
    End Sub
    Public Sub SortList(ByVal F_SortField As String)
        If (F_SortField = ViewState("F_SortBy")) Then
            If ViewState("F_SortOrder") = "ASC" Then
                ViewState("F_SortOrder") = "DESC"
            Else
                ViewState("F_SortOrder") = "ASC"
            End If
        Else
            ViewState("F_SortBy") = F_SortField
            ViewState("F_SortOrder") = "ASC"
        End If

        BindList(DataSource)
    End Sub

    Private Sub btnSynKwImport_Click(sender As Object, e As System.EventArgs) Handles btnSynKwImport.Click
        Try
            Dim dt = New DataTable()
            Dim defaultDB = DatabaseFactory.CreateDatabase()
            Using cmd As DbCommand = defaultDB.GetSqlStringCommand("select Id, KeywordName from KeywordImport where isnull(KeywordName, '') <> ''")
                Dim reader = defaultDB.ExecuteReader(cmd)
                dt.Load(reader)

                If dt.Rows.Count = 0 Then
                    Return
                End If

                For index = 0 To dt.Rows.Count - 1
                    Dim temp = Nothing
                    Try
                        temp = SolrHelper.SearchItem(dt.Rows(index)("KeywordName"), Nothing, String.Empty, 1, 1, 1, String.Empty, 0, String.Empty, String.Empty, False, String.Empty, 0, 0, 0, String.Empty, False)
                    Catch ex As Exception

                    End Try
                    If temp Is Nothing OrElse (temp IsNot Nothing AndAlso temp.Count = 0) Then
                        Dim SQL = "update KeywordImport set HasResult = 0 where Id =" + dt.Rows(index)("Id").ToString()
                        Dim cmdCm As DbCommand = New SqlCommand(SQL)
                        defaultDB.ExecuteNonQuery(cmdCm)
                    End If
                Next
            End Using

        Catch ex As Exception
            Dim a As String = String.Empty
        End Try
    End Sub
End Class

