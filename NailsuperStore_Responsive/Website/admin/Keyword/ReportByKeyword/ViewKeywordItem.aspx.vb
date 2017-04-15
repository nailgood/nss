Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_KeyWord_ReportByKeyword_ViewKeywordItem
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim KeywordId As Int64 = Convert.ToInt64(Core.ProtectParam(Request("Id")))
        If (KeywordId > 0) Then
            Dim objKeyword As KeywordRow = KeywordRow.GetRow(DB, KeywordId)
            lbKeyword.Text = objKeyword.KeywordName
        End If
        Dim total As Integer = 0
        Dim dt As DataTable = KeywordItemRow.GetItemForKeyword(KeywordId, gvList.SortBy, gvList.SortOrder, gvList.PageIndex + 1, gvList.PageSize, total)
        gvList.DataSource = dt.DefaultView
        gvList.Pager.NofRecords = total
        gvList.PageSelectIndex = gvList.PageIndex

        gvList.DataBind()
    End Sub

End Class
