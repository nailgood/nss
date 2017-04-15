Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_images_Index
    Inherits AdminPage

    Protected F_ItemId As Integer
    Protected dbStoreItem As StoreItemRow
    Dim ToTal As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        F_ItemId = CInt(Request("F_ItemId"))
        dbStoreItem = StoreItemRow.GetRow(DB, F_ItemId)

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            If gvList.SortBy = String.Empty Then gvList.SortBy = "SortOrder"
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " and "

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM StoreItemImage where ItemId = " & F_ItemId

        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        ToTal = gvList.Pager.NofRecords
        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
           
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)

            If ToTal < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                If e.Row.RowIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.RowIndex = ToTal - 1 Then
                    imbDown.Visible = False
                End If
            End If
            
        End If

    End Sub
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            StoreItemImageRow.ChangeOrder(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            StoreItemImageRow.ChangeOrder(DB, e.CommandArgument, False)
        End If
        Response.Redirect("Default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

