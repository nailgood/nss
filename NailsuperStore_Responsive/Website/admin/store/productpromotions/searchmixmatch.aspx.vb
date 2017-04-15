
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_productpromotions_searchmixmatch
    Inherits AdminPage
    Dim mixmatchNoSelect As String = String.Empty
   
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        mixmatchNoSelect = ";" + Request("no")

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            hidMMSelect.Value = Request("no")
            BindList()
        End If
    End Sub
    
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
    Public Function getRadioSelect(ByVal mixmatchNo As String) As String
        Dim skuTMP As String = mixmatchNoSelect + ";"
        If skuTMP.Contains(";" + mixmatchNo + ";") Then
            Return "checked='checked'"
        End If
        Return ""
    End Function
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "


        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " m.*, c.customerpricegroupcode "
        SQL = " FROM MixMatch m left outer join customerpricegroup c on m.customerpricegroupid = c.customerpricegroupid "

        SQL = SQL & Conn & "m.[Type] = " & Utility.Common.MixmatchType.ProductCoupon & " and m.IsActive=1"
        If Not String.IsNullOrEmpty(txtMixmatchno.Text) Then
            SQL = SQL & " and m.[MixmatchNo] = '" & txtMixmatchno.Text.Trim() & "'"
        End If



        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        '' Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY  " & IIf(gvList.SortByAndOrder.Contains("StartingDate") = False, " StartingDate desc", gvList.SortByAndOrder))
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)

        gvList.DataSource = res.DefaultView
        gvList.DataBind()
        btnSave.Visible = False
        If Not res Is Nothing Then
            If res.Rows.Count > 0 Then
                btnSave.Visible = True
            End If
        End If
    End Sub




End Class
