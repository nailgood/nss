Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility

Public Class admin_News
    Inherits AdminPage
    Private TotalRecords As Integer
    Private Type As Integer = 1
    Dim ToTal As Integer
    Protected NewsId As Integer
    Public index As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            NewsId = IIf(Request("NewsId") <> Nothing, Request("NewsId"), Nothing)
            If NewsId <> Nothing Then
                Session("NewsId") = NewsId
            End If
            BindList()
        End If
    End Sub

    Private Sub BindList()

        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " id, NewsId,DocumentId,IsActive"
        SQL = " FROM NewsDocument nd "
        If Session("NewsId") <> Nothing Then
            SQL = SQL & Conn & " nd.NewsId = " & Session("NewsId")
            Conn = " AND "
        End If
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY arrange asc ")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            index = index + 1
            Dim ltNewsTitle As Literal = CType(e.Row.FindControl("ltNewsTitle"), Literal)
            Dim ltDocument As Literal = CType(e.Row.FindControl("ltDocument"), Literal)
            Dim ltrIndex As Literal = CType(e.Row.FindControl("ltrIndex"), Literal)
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
            Dim rDoc As DocumentRow = DocumentRow.GetRow(DB, e.Row.DataItem("DocumentId"))
            ltDocument.Text = rDoc.DocumentName
            ltrIndex.Text = index
            Dim rNews As NewsRow = NewsRow.GetRow(DB, e.Row.DataItem("NewsId"))
            ltNewsTitle.Text = rNews.Title
            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            ''Dim data As NewsRow = DirectCast(e.Row.DataItem, NewsRow)
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)

            ''imbDown.CommandArgument=
            If e.Row.RowIndex = 0 AndAlso ToTal > 1 Then
                imbUp.Visible = False
            ElseIf e.Row.RowIndex = ToTal - 1 AndAlso ToTal > 1 Then
                imbDown.Visible = False
            ElseIf ToTal < 2 Then
                imbDown.Visible = False
                imbUp.Visible = False
            End If
            hidPopUpDoc.Value &= e.Row.DataItem("DocumentId") & ";"
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            NewsDocumentRow.ChangeChangeArrange(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            NewsDocumentRow.ChangeChangeArrange(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "Delete" Then
            NewsDocumentRow.Delete(DB, e.CommandArgument)
        ElseIf e.CommandName = "Active" Then
            NewsDocumentRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        Response.Redirect("Default.aspx?Type=1&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        ''Dim tab As New ShopSaveItemRow(DB)
        Dim DocIdIsNotValid As String = String.Empty
        Dim arr As Array = Split(hidPopUpDoc.Value.Trim(), ";")
        Dim i As Integer
        Dim NewsDoc As New NewsDocumentRow
        Dim result As Integer = 0
        If arr.Length > 0 Then
            For i = 0 To arr.Length - 1
                If arr(i).ToString() <> String.Empty Then
                    NewsDoc.NewsId = Session("NewsId")
                    NewsDoc.DocumentId = CInt(arr(i))
                    NewsDoc.IsActive = True
                    Try
                        result = NewsDoc.Insert(False)
                    Catch ex As Exception
                        result = 0
                        AddError(ErrHandler.ErrorText(ex))
                    End Try
                    If result < 1 Then ''insert faild
                        DocIdIsNotValid = DocIdIsNotValid & NewsDoc.DocumentId
                    End If
                End If
            Next
        End If
        'Clear cache
        Utility.CacheUtils.ClearCacheWithPrefix(NewsDocumentRow.cachePrefixKey)

        BindList()
        If DocIdIsNotValid <> String.Empty Then

            ltrMsg.Text = "Error! DocumentId: " & DocIdIsNotValid.Substring(0, DocIdIsNotValid.Length - 1) & " is invalid or existing"
        Else
            ltrMsg.Text = String.Empty
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("../News/default.aspx?Type=1&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
