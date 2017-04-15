Imports Components
Imports Controls
Imports DataLayer
Imports System.IO
Imports System.Data
Partial Class admin_KeyWord_ReportByKeyword_Default
    Inherits AdminPage
    Private dt As DataTable
    Private Path As String = "/assets/export/keyword/"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtKeyword.Text = Request("F_Keyword")
            pagerTop.PageIndex = 1
            pagerBottom.PageIndex = 1
            If String.IsNullOrEmpty(ViewState("F_SortBy")) Then
                ViewState("F_SortBy") = "KeywordName"
            End If
            If String.IsNullOrEmpty(ViewState("F_SortOrder")) Then
                ViewState("F_SortOrder") = "ASC"
            End If
            LoadOldFile()
            BindList()
        End If
        txtKeyword.Focus()
    End Sub

    Private Sub BindList()

        hidSortField.Value = ViewState("F_SortBy")
        Dim selectRow As Integer = 0
        Dim total As Integer = 0
        dt = KeywordRow.GetListKeyword(txtKeyword.Text.Trim(), -1, -1, ViewState("F_SortBy"), ViewState("F_SortOrder"), pagerTop.PageIndex, pagerTop.PageSize, total)

        gvList.DataSource = dt.DefaultView
        gvList.DataBind()
        If (Not dt Is Nothing) Then
            selectRow = dt.Rows.Count
        End If
        pagerTop.SetPaging(selectRow, total)
        pagerBottom.SetPaging(selectRow, total)

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not Page.IsValid Then Exit Sub
        ViewState("F_SortBy") = "KeywordName"
        ViewState("F_SortOrder") = "ASC"
        pagerTop.PageIndex = 1
        pagerBottom.PageIndex = 1
        BindList()
    End Sub
    Protected Sub pagerTop_PageIndexChanging(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
        pagerTop.PageIndex = e.PageIndex
        pagerBottom.PageIndex = e.PageIndex
        BindList()
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
        BindList()
    End Sub
    Private Sub btnSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSort.Click
        SortList(hidSortField.Value)
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
        BindList()
    End Sub
    Private Sub ExportExcel(Optional ByVal isNoResult As Boolean = False)
        DeleteOldFile(isNoResult)
        If dt Is Nothing Then
            Dim countrows As Integer = 0
            If Not isNoResult Then
                dt = KeywordRow.GetListKeyword(txtKeyword.Text.Trim(), -1, -1, ViewState("F_SortBy"), ViewState("F_SortOrder"), pagerTop.PageIndex, Integer.MaxValue, countrows)
            Else
                dt = KeywordRow.GetListKeywordNoResult(txtKeyword.Text.Trim(), -1, -1, ViewState("F_SortBy"), ViewState("F_SortOrder"), pagerTop.PageIndex, Integer.MaxValue, countrows)
            End If

        End If
        Dim dr As DataRow
        Dim FileName As String = IIf(isNoResult, "Keywords_NoResult_", "Keywords_") & Replace(Replace(Replace(Date.Now(), ":", "_"), " ", "_"), "/", "_") & ".csv"
        Dim sw As StreamWriter = New StreamWriter(Server.MapPath(Path & FileName), False)
        sw.WriteLine("Keyword Name, Total Search, Total Detail, Total AddCart, Total Point")
        For Each dr In dt.Rows
            Dim KeywordName As String = IIf(IsDBNull(dr("KeywordName")), String.Empty, dr("KeywordName"))
            Dim TotalPoint As String = IIf(IsDBNull(dr("TotalPoint")), String.Empty, dr("TotalPoint"))
            Dim TotalSearch As String = IIf(IsDBNull(dr("TotalSearch")), String.Empty, dr("TotalSearch"))
            Dim TotalDetail As String = IIf(IsDBNull(dr("TotalDetail")), String.Empty, dr("TotalDetail"))
            Dim TotalAddCart As String = IIf(IsDBNull(dr("TotalAddCart")), String.Empty, dr("TotalAddCart"))
            sw.WriteLine(Core.QuoteCSV(KeywordName) & "," & Core.QuoteCSV(TotalSearch) & "," & Core.QuoteCSV(TotalDetail) & "," & Core.QuoteCSV(TotalAddCart) & "," & Core.QuoteCSV(TotalPoint))
        Next
        sw.Flush()
        sw.Close()

        If isNoResult Then
            lnkDownloadKwNoResult.NavigateUrl = Path & FileName
            lnkDownloadKwNoResult.Text = "Download file: " & FileName
        Else
            lnkDownload.NavigateUrl = Path & FileName
            lnkDownload.Text = "Download file: " & FileName
        End If
    End Sub
    'Private Sub ExportExcel(ByVal dt As DataTable, ByVal headerExcell As String, ByVal ColsData As String, ByVal folder As String, ByVal filename As String) 'dat o common dung chung
    '    Dim sw As StreamWriter = New StreamWriter(Server.MapPath(Path & filename), False)
    '    Try
    '        sw.WriteLine(headerExcell)
    '        Dim dr As DataRow
    '        Dim arrCols As String() = ColsData.Split(",")
    '        Dim rowsdata As String = String.Empty
    '        For Each dr In dt.Rows
    '            'Dim KeywordName As String = IIf(IsDBNull(dr("KeywordName")), String.Empty, dr("KeywordName"))
    '            'Dim TotalPoint As String = IIf(IsDBNull(dr("TotalPoint")), String.Empty, dr("TotalPoint"))
    '            'Dim TotalSearch As String = IIf(IsDBNull(dr("TotalSearch")), String.Empty, dr("TotalSearch"))
    '            'Dim TotalDetail As String = IIf(IsDBNull(dr("TotalDetail")), String.Empty, dr("TotalDetail"))
    '            'Dim TotalAddCart As String = IIf(IsDBNull(dr("TotalAddCart")), String.Empty, dr("TotalAddCart"))
    '            For i As Integer = 0 To arrCols.Length - 1
    '                If Not rowsdata.Contains(",") Then
    '                    rowsdata &= Core.QuoteCSV(arrCols(i))
    '                Else
    '                    rowsdata &= "," & Core.QuoteCSV(arrCols(i))
    '                End If
    '            Next
    '            'sw.WriteLine(Core.QuoteCSV(KeywordName) & "," & Core.QuoteCSV(TotalSearch) & "," & Core.QuoteCSV(TotalDetail) & "," & Core.QuoteCSV(TotalAddCart) & "," & Core.QuoteCSV(TotalPoint))
    '            sw.WriteLine(rowsdata)
    '        Next
    '    Catch ex As Exception
    '    Finally
    '        sw.Flush()
    '        sw.Close()
    '    End Try

    'End Sub
    Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportExcel.Click
        ExportExcel()
    End Sub
    Protected Sub btnExportExcelNoResult_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportExcelNoResult.Click
        ExportExcel(True)
    End Sub
    Private Sub DeleteOldFile(ByVal isNoResult As Boolean)
        Dim d As DirectoryInfo = New DirectoryInfo(Server.MapPath(Path))
        Dim Files() As FileInfo = d.GetFiles()
        For Each f As FileInfo In Files
            If isNoResult And f.FullName.Contains("NoResult") Then
                f.Delete()
            End If
            If Not isNoResult And Not f.FullName.Contains("NoResult") Then
                f.Delete()
            End If
        Next
    End Sub
    Private Sub LoadOldFile()
        Dim d As DirectoryInfo = New DirectoryInfo(Server.MapPath(Path))
        Dim Files() As FileInfo = d.GetFiles()
        For Each f As FileInfo In Files

            If f.FullName.Contains("NoResult") Then
                lnkDownloadKwNoResult.NavigateUrl = Path & f.Name
                lnkDownloadKwNoResult.Text = "Download file: " & f.Name
            Else
                lnkDownload.NavigateUrl = Path & f.Name
                lnkDownload.Text = "Download file: " & f.Name
            End If
        Next
    End Sub
End Class
