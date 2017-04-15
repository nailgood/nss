
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_KeyWord_ReportByDate_Default
    Inherits AdminPage
  

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnExport.Attributes.Add("OnClick", "return;")

        If Not Page.IsPostBack Then
            pagerTop.PageIndex = 1
            pagerBottom.PageIndex = 1
            If String.IsNullOrEmpty(ViewState("F_SortBy")) Then
                ViewState("F_SortBy") = "SearchedDate"
            End If
            If String.IsNullOrEmpty(ViewState("F_SortOrder")) Then
                ViewState("F_SortOrder") = "DESC"
            End If
            BindData()
        End If
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
        BindData()
    End Sub
    Private Sub btnSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSort.Click
        SortList(hidSortField.Value)
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ViewState("F_SortBy") = "SearchedDate"
        ViewState("F_SortOrder") = "DESC"
        pagerTop.PageIndex = 1
        pagerBottom.PageIndex = 1

        BindData()
        hidFromDate.Value = dtpFromDate.Text
        hidToDate.Value = dtpToDate.Text
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ltrDetail As Literal = CType(e.Row.FindControl("ltrDetail"), Literal)
            Dim ltrExport As Literal = CType(e.Row.FindControl("ltrExport"), Literal)
            Dim dateSearch As DateTime = Nothing
            Try
                dateSearch = DateTime.Parse(e.Row.DataItem("SearchedDate").ToString())
            Catch ex As Exception

            End Try
            ltrDetail.Text = "<a style='cursor:pointer;' onClick=OpenPopUp('" + dateSearch.ToString("MM/dd/yyyy") + "')>Detail</a>"
            ltrExport.Text = "<a style='cursor:pointer;' onClick=ExportByDate('" + dateSearch.ToString("MM/dd/yyyy") + "')>Export</a>"

        End If
    End Sub
    Private Sub BindData()
       
        hidSortField.Value = ViewState("F_SortBy")
        Dim total As Integer = 0
        Dim CountItem As Integer = 0
        Dim sortBy As String = gvList.SortBy
        Dim condition As String = "1=1"
        If Not dtpFromDate.Text = String.Empty Then
            condition = condition & " and Cast(SearchedDate as Date) >= " & DB.Quote(dtpFromDate.Text)
        End If
        If Not dtpToDate.Text = String.Empty Then
            condition = condition & " and Cast(SearchedDate as Date) <= " & DB.Quote(dtpToDate.Text)
        End If
        Dim data As DataTable = KeywordRow.GetDataReportByDate(condition, ViewState("F_SortBy"), ViewState("F_SortOrder"), pagerTop.PageIndex, pagerTop.PageSize, total)
        If Not data Is Nothing Then
            CountItem = data.Rows.Count
        End If
        gvList.DataSource = data.DefaultView
        gvList.DataBind()
       
        pagerTop.SetPaging(CountItem, total)
        pagerBottom.SetPaging(CountItem, total)
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim ex As String = Request("hidExport")
        Dim dt As DataTable
        Dim Filename As String = ""
        If String.IsNullOrEmpty(ex) Then
            Dim total As Integer = 0
            Dim sortBy As String = gvList.SortBy
            Dim condition As String = "1=1"
            If Not hidFromDate.Value = String.Empty Then
                condition = condition & " and Cast(SearchedDate as Date) >= " & DB.Quote(hidFromDate.Value)
            End If
            If Not hidToDate.Value = String.Empty Then
                condition = condition & " and Cast(SearchedDate as Date) <= " & DB.Quote(hidToDate.Value)
            End If
            ' dt = KeywordRow.GetDataReportByListExport(condition, gvList.PageIndex + 1, gvList.PageSize)
            dt = KeywordRow.GetDataReportByListExport(condition, gvList.PageIndex + 1, Integer.MaxValue)
            Filename = "ReportByListToExcel_" & Now.Month & "_" & Now.Day & "_" & Now.Year
        Else
            dt = KeywordRow.GetListDataKeywordByDateExport(Convert.ToDateTime(ex))
            Filename = "ReportByDateToExcel_" & ex.Replace("/", "_")

        End If
        Response.ContentType = "Application/vnd.ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=" & Filename & ".xls")
        Dim style As String = "<style> .textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(ExportExcel(dt))
        Response.End()

    End Sub
    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim lblDate As Label = CType(e.Row.FindControl("lblDate"), Label)
        Dim SearchedDate As String = e.Row.DataItem("SearchedDate")
        If Not String.IsNullOrEmpty(SearchedDate) Then
            lblDate.Text = SearchedDate
        End If
    End Sub
    Private Function ExportExcel(ByVal dt As DataTable) As String
        Dim tw As StringWriter = New StringWriter()
        Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
        Dim gvGrid As GridView = New GridView()
        gvGrid.DataSource = dt
        gvGrid.DataBind()
        For i As Integer = 0 To gvGrid.Rows.Count - 1
            gvGrid.Rows(i).Attributes.Add("class", "textmode")
        Next
        gvGrid.RenderControl(hw)
        Return tw.ToString()
    End Function

    Protected Sub pagerTop_PageIndexChanging(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
        pagerTop.PageIndex = e.PageIndex
        pagerBottom.PageIndex = e.PageIndex
        BindData()
    End Sub

    Protected Sub pagerTop_PageSizeChanging(ByVal obj As Object, ByVal e As PageSizeChangeEventArgs)
        pagerTop.PageSize = e.PageSize
        pagerTop.PageIndex = 1
        pagerBottom.PageSize = e.PageSize
        pagerBottom.PageIndex = 1

        If (DirectCast(obj, controls_layout_pager).ID = "pagerBottom") Then
            pagerTop.ViewAll = pagerBottom.ViewAll
        Else
            pagerBottom.ViewAll = pagerTop.ViewAll
        End If
        BindData()
    End Sub
End Class
