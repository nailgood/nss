Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_tips_categories_items
    Inherits AdminPage

    Protected params As String
    Protected TipCategoryId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        params = GetPageParams(FilterFieldType.All)
        TipCategoryId = Request("TipCategoryId")
        If Not IsPostBack Then
            BindDataGrid()
        End If
    End Sub

    Private Sub BindDataGrid()

        If Not IsPostBack Then
            ViewState("F_PG") = Request("F_PG")
        End If
        If CType(ViewState("F_PG"), String) = String.Empty Then
            ViewState("F_PG") = 1
        End If

        ' BUILD QUERY
        SQL = ""
        SQL &= " SELECT ti.Id, ti.TipCategoryid, si.ItemId, si.SKU, si.ItemName, si.IsActive FROM StoreItem si, TipCategoryItem ti"
        SQL &= " where si.ItemId = ti.ItemId"
        SQL &= "   and ti.TipCategoryid = " & DB.Quote(TipCategoryId)

        lblItemName.Text = DB.ExecuteScalar("SELECT tipcategory FROM TipCategory WHERE TipCategoryId = " & TipCategoryId)
        Trace.Write(SQL)
        Dim res As DataSet = DB.GetDataSet(SQL)

        myNavigator.NofRecords = res.Tables(0).Rows.Count
        myNavigator.MaxPerPage = dgList.PageSize
        myNavigator.PageNumber = Math.Max(Math.Min(CType(ViewState("F_PG"), Integer), myNavigator.NofPages), 1)
        myNavigator.DataBind()

        ViewState("F_PG") = myNavigator.PageNumber
        tblList.Visible = (myNavigator.NofRecords <> 0)
        plcNoRecords.Visible = (myNavigator.NofRecords = 0)

        dgList.DataSource = res.Tables(0).DefaultView
        dgList.CurrentPageIndex = ViewState("F_PG") - 1
        dgList.DataBind()
    End Sub

    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As Controls.NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindDataGrid()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            DB.BeginTransaction()
            Dim dbTipCategory As TipCategoryRow = TipCategoryRow.GetRow(DB, TipCategoryId)
            dbTipCategory.InsertTipCategoryItem(Request.Form("ItemId"))
            DB.CommitTransaction()
            Response.Redirect("items.aspx?TipCategoryId=" & TipCategoryId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
            BindDataGrid()
        End Try
    End Sub
End Class