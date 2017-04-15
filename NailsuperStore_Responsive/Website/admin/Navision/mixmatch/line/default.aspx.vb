Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_navision_mixmatch_line_Index
    Inherits AdminPage
    Private id As Integer = 0
    Private ItemId As Integer = 0
    Public Property Type() As Integer
        Get
            If ViewState("Type") Is Nothing Then
                Return Utility.Common.MixmatchType.Normal
            End If
            Return CInt(ViewState("Type"))
        End Get
        Set(ByVal value As Integer)
            ViewState("Type") = value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'If ItemId.Value <> 0 Then
        '    id = ItemId.Value
        'End If

        If txtSku.Text <> "" Then
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, txtSku.Text.ToString())
            ItemId = si.ItemId
        End If

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            Type = Request.QueryString("type")
            F_MixMatchId.DataSource = MixMatchRow.GetAllMixMatches(DB)
            F_MixMatchId.DataValueField = "Id"
            F_MixMatchId.DataTextField = "MixMatchNo"
            F_MixMatchId.DataBind()
            F_MixMatchId.Items.Insert(0, New ListItem("-- ALL --", ""))



            F_DiscountType.Text = Request("F_DiscountType")
            F_IsActive.Text = Request("F_IsActive")
            F_MixMatchId.SelectedValue = Request("F_MixMatchId")
            F_ValueLBound.Text = Request("F_ValueLBound")
            F_ValueUbound.Text = Request("F_ValueUBound")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Id"

            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM MixMatchLine l inner join storeitem i on l.itemid = i.itemid inner join mixmatch m on l.mixmatchid = m.id "

        If Not F_MixMatchId.SelectedValue = String.Empty Then
            'Dim objMixmatch As MixMatchRow = MixMatchRow.GetRow(DB, F_MixMatchId.SelectedValue)
            'Type = objMixmatch.Type
            SQL = SQL & Conn & "MixMatchId = " & DB.Quote(F_MixMatchId.SelectedValue)
            Conn = " AND "
        End If
        If Type = 0 Then
            Type = Utility.Common.MixmatchType.Normal
        End If
        'If Not F_ItemId.SelectedValue = String.Empty Then
        '    SQL = SQL & Conn & "i.ItemId = " & DB.Quote(F_ItemId.SelectedValue)
        '    Conn = " AND "
        'End If
        If ItemId <> 0 Then
            SQL = SQL & Conn & "i.ItemId = " & ItemId
            Conn = " AND "
        End If
        If Not F_DiscountType.Text = String.Empty Then
            SQL = SQL & Conn & "l.DiscountType LIKE " & DB.FilterQuote(F_DiscountType.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ValueLBound.Text = String.Empty Then
            SQL = SQL & Conn & "Value >= " & DB.Number(F_ValueLBound.Text)
            Conn = " AND "
        End If
        If Not F_ValueUbound.Text = String.Empty Then
            SQL = SQL & Conn & "Value <= " & DB.Number(F_ValueUbound.Text)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " l.*, m.mixmatchno, (i.SKU + ' - ' + i.itemname) as itemname"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?type=" & Type & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
End Class

