Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Public Class admin_dealday_default
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_Title.Text = Request("F_Title")
            F_IsActive.Text = Request("F_IsActive")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "StartDate"
                gvList.SortOrder = "DESC"
            End If
            'BindList()
            btnSearch_Click(sender, e)
            lblNow.Text = "Now is " & Now.ToString()
        End If
    End Sub

    Private Sub BindQuery()

        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM DealDay d  left join StoreItem item on (item.ItemId=d.ItemId) "

        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & "d.Title LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "d.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_SKU.Text = String.Empty Then
            SQL = SQL & Conn & "item.SKU  = '" & F_SKU.Text.Trim() & "'"
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " d.DealDayId,d.Title,item.ItemName,d.StartDate,d.EndDate,d.IsActive,item.ItemId "
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
    Public Function GetItemName(ByVal id As String) As String
        Dim ItemSku As StoreItemRow = StoreItemRow.GetRow(DB, Convert.ToInt32(id))
        Return ItemSku.ItemName
    End Function
End Class
