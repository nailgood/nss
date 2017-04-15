
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Keyword_Synonym_Default
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(GetQueryString("SynonymType")) Then
                dblSynonymType.SelectedValue = GetQueryString("SynonymType")
            End If
            ''  txtKeyword.Text = Request("F_Keyword")

            ''pagerTop.PageSize = 48
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
    Private Sub btnAddnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddnew.Click
        Dim link As String = "edit.aspx?F_Keyword=" & txtKeyword.Text.Trim & "&SynonymType=" + dblSynonymType.SelectedValue
        Response.Redirect(link)
    End Sub
    Private Sub BindList()
        
        hidSortField.Value = ViewState("F_SortBy")
        Dim selectRow As Integer = 0
        Dim total As Integer = 0
        Dim dt As DataTable = KeywordRow.GetListKeyword(txtKeyword.Text.Trim(), 1, -1, ViewState("F_SortBy"), ViewState("F_SortOrder"), pagerTop.PageIndex, pagerTop.PageSize, total, dblSynonymType.SelectedValue)
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
    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
       If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim ltrOneWay As Literal = CType(e.Row.FindControl("ltrOneWay"), Literal)
        Dim ltrRoundtrip As Literal = CType(e.Row.FindControl("ltrRoundtrip"), Literal)
        Dim ltrArrange As Literal = CType(e.Row.FindControl("ltrArrange"), Literal)

        Dim link As String = "<a href='edit.aspx?KeywordId=" & e.Row.DataItem("KeywordId") & "&r={0}&F_hsk={1}&F_Keyword={2}&SynonymType=" & dblSynonymType.SelectedValue & "'>{3}</a>"
        Dim lstKeywordSynonym As KeywordSynonymCollection = KeywordSynonymRow.GetListKeywordSynonym(e.Row.DataItem("KeywordName"), dblSynonymType.SelectedValue)
        If Not lstKeywordSynonym Is Nothing AndAlso lstKeywordSynonym.Count > 0 Then

            For Each keyword As KeywordSynonymRow In lstKeywordSynonym
                If (keyword.IsRoundTrip) Then
                    ltrRoundtrip.Text = ltrRoundtrip.Text & keyword.KeywordSynonymName & ", "
                Else
                    ltrOneWay.Text = ltrOneWay.Text & keyword.KeywordSynonymName & ", "
                End If
            Next
            If Not String.IsNullOrEmpty(ltrOneWay.Text) Then
                ltrOneWay.Text = String.Format(link, 0, "1", txtKeyword.Text.Trim(), TrimEndKeyword(ltrOneWay.Text.Trim()))
            End If
            If Not String.IsNullOrEmpty(ltrRoundtrip.Text) Then
                ltrRoundtrip.Text = String.Format(link, 1, "1", txtKeyword.Text.Trim(), TrimEndKeyword(ltrRoundtrip.Text.Trim()))
            End If
        End If
        'If (String.IsNullOrEmpty(ltrOneWay.Text)) Then
        '    ltrOneWay.Text = String.Format(link, 0, "1", txtKeyword.Text.Trim(), "<img src='/includes/theme-admin/images/edit.gif'/>")
        'End If
        'If (String.IsNullOrEmpty(ltrRoundtrip.Text)) Then
        '    ltrRoundtrip.Text = String.Format(link, 1, "1", txtKeyword.Text.Trim(), "<img src='/includes/theme-admin/images/edit.gif'/>")
        'End If
        ltrArrange.Text = String.Format(link.Replace("edit.aspx", "arrange.aspx"), 1, "1", txtKeyword.Text.Trim(), "Arrange")
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
    Protected Sub dblSynonymType_CheckedChanged(sender As Object, e As EventArgs)
        BindList()
    End Sub
End Class

