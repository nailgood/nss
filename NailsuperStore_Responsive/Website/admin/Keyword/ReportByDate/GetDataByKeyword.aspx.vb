

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_KeyWord_ReportByDate_GetDataByKeyword
    Inherits AdminPage


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindData
        If Not Page.IsPostBack Then
            BindData()
        End If
    End Sub
  
    Private Sub BindData()
        Dim dateSearch As DateTime
        Try
            dateSearch = Request.QueryString("d")
        Catch ex As Exception

        End Try

        Dim CountItem As Integer = 0
        Dim total As Integer = 0
        Dim sortBy As String = gvList.SortBy
        Dim data As DataTable = KeywordRow.GetListDataKeywordByDate(dateSearch, sortBy, gvList.SortOrder, gvList.PageIndex + 1, gvList.PageSize, total)
        CountItem = data.Rows.Count
        gvList.Pager.NofRecords = total
        If (total > 20) Then
            gvList.AllowPaging = True
        Else
            gvList.AllowPaging = False
        End If
        gvList.PageSelectIndex = gvList.PageIndex
        gvList.DataSource = data
        gvList.DataBind()
    End Sub



End Class
