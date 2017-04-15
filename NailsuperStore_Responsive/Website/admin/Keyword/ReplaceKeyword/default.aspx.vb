

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Keyword_ReplaceKeyword_default
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            txtKeyword.Text = Request("F_Keyword")
            If Not Request("F_hrk") Is Nothing Then
                If Request("F_hrk") = "1" Then
                    chkHasReplaceKeyword.Checked = True
                Else
                    chkHasReplaceKeyword.Checked = False
                End If
            End If
            pagerTop.PageIndex = 1
            pagerBottom.PageIndex = 1
            If String.IsNullOrEmpty(ViewState("F_SortBy")) Then
                ViewState("F_SortBy") = "KeywordName"
            End If
            If String.IsNullOrEmpty(ViewState("F_SortOrder")) Then
                ViewState("F_SortOrder") = "ASC"
            End If
            BindList()
        End If
        txtKeyword.Focus()
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
    Private Sub BindList()
        hidSortField.Value = ViewState("F_SortBy")
        Dim selectRow As Integer = 0
        Dim total As Integer = 0

        Dim dt As DataTable = KeywordRow.GetListKeyword(txtKeyword.Text.Trim(), -1, IIf(chkHasReplaceKeyword.Checked, 1, 0), ViewState("F_SortBy"), ViewState("F_SortOrder"), pagerTop.PageIndex, pagerTop.PageSize, total)
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
    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim linkEdit As String = "<a href='edit.aspx?KeywordId=" & e.Row.DataItem("KeywordId") & "&F_hrk={0}&F_Keyword={1}'><img src='/includes/theme-admin/images/edit.gif'/></a>"
        Dim linkDelete As String = "<a href='delete.aspx?KeywordId=" & e.Row.DataItem("KeywordId") & "&F_hrk={0}&F_Keyword={1}'><img src='/includes/theme-admin/images/delete.gif'/></a>"
        Dim linkKwFilter As String = "<a href='../FilterKeyword/edit.aspx?keywordId=" & e.Row.DataItem("KeywordId") & "&keywordSynonymId=" & e.Row.DataItem("ReplaceKeywordId") & "'>Filter</a>"
        linkEdit = String.Format(linkEdit, IIf(chkHasReplaceKeyword.Checked, "1", "0"), txtKeyword.Text.Trim())
        linkDelete = String.Format(linkDelete, IIf(chkHasReplaceKeyword.Checked, "1", "0"), txtKeyword.Text.Trim())
        Dim ltrReplaceKeyword As Literal = CType(e.Row.FindControl("ltrReplaceKeyword"), Literal)
        Dim replaceKeywordId As Integer = e.Row.DataItem("ReplaceKeywordId")
        If (replaceKeywordId > 0) Then
            Dim objKeyword As KeywordRow = KeywordRow.GetRow(DB, replaceKeywordId)
            If Not (objKeyword Is Nothing) Then
                ltrReplaceKeyword.Text = "<table><tr><td>" & objKeyword.KeywordName & "</td><td>" & linkEdit & "</td><td>" & linkDelete & "</td><td>" & linkKwFilter & "</td></tr></table>"
            End If


        End If
        If String.IsNullOrEmpty(ltrReplaceKeyword.Text) Then
            ltrReplaceKeyword.Text = linkEdit
        End If
    End Sub
    Protected Sub pagerTop_PageIndexChanging(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
        pagerTop.PageIndex = e.PageIndex
        pagerBottom.PageIndex = e.PageIndex
        BindList()
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
    Private Function TrimEndKeyword(ByVal keyword As String) As String
        If String.IsNullOrEmpty(keyword) Then
            Return String.Empty
        End If
        If (keyword.Contains(",")) Then
            keyword = keyword.Substring(0, keyword.Length - 1)
        End If
        Return keyword
    End Function
End Class

