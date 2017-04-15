Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_sales_default
    Inherits AdminPage

    Protected params As String

    Private SumCount As Integer = 0
    Private CostSum As Double = 0

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            BindRepeater()
        End If
    End Sub

    Private Sub BindRepeater()
        params = GetPageParams(FilterFieldType.All)

        If Not IsPostBack Then
            ViewState("F_SortBy") = Replace(Request("F_SortBy"), ";", "")
            ViewState("F_SortOrder") = Replace(Request("F_SortOrder"), ";", "")
            ViewState("F_PG") = Request("F_PG")
        End If
        If ViewState("F_SortBy") = String.Empty Then
            ViewState("F_SortBy") = "NumSales"
        End If
        If ViewState("F_SortOrder") = String.Empty Then
            ViewState("F_SortOrder") = "DESC"
        End If
        If CType(ViewState("F_PG"), String) = String.Empty Then
            ViewState("F_PG") = 1
        End If

        ' BUILD QUERY
        SQL = BuildQuery(True)

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

    Public Function BuildQuery(ByVal bOrderBy As Boolean)
        Dim tmpSQL As String = " select " & _
          " sc.sku, sc.Price As UnitPrice, sc.Price*sum(quantity) As TotalPrice, sc.ItemName, coalesce(sum(sc.linediscountamount),0) as itemdiscount, sum(quantity) As NumSales from storecartitem sc, storeorder so" & _
          " where so.orderid = sc.orderid And so.processdate Is Not null "

        If Not F_FromDate.Value = Nothing Then tmpSQL = tmpSQL & " AND so.ProcessDate >= " & DB.Quote(F_FromDate.Value & " 12:00:01 AM")
        If Not F_ToDate.Value = Nothing Then tmpSQL = tmpSQL & " AND so.ProcessDate <= " & DB.Quote(F_ToDate.Value & " 11:59:59 PM")
        If Not DB.IsEmpty(F_SKU.Text) Then tmpSQL = tmpSQL & " AND sc.SKU = " & DB.Quote(F_SKU.Text)
        tmpSQL = tmpSQL & " GROUP BY sc.sku, sc.price, sc.itemname "
        If bOrderBy = True Then tmpSQL = tmpSQL & " ORDER BY " & ViewState("F_SortBy") & " " & ViewState("F_SortOrder")
        Return tmpSQL
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ViewState("F_PG") = 1
        If F_drpExportType.SelectedValue = "Excel" Then
            tblFilter.Visible = False

            dgList.PageSize = 10000
            ViewState("F_SortOrder") = LCase(ViewState("F_SortOrder"))
            Response.ContentType = "application/vnd.ms-excel"
            myNavigator.Visible = False
            Response.AddHeader("Content-Disposition", "attachment; filename=SalesExport.xls")
        End If
        BindRepeater()
    End Sub

    Private Sub btnResetSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetSearch.Click
        Response.Redirect("sale.aspx")
    End Sub

    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As Controls.NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindRepeater()
    End Sub

    Private Sub dgList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgList.SortCommand
        If ViewState("F_SortOrder") = "ASC" And ViewState("F_SortBy") = e.SortExpression Then
            ViewState("F_SortOrder") = "DESC"
        Else
            ViewState("F_SortOrder") = "ASC"
        End If
        ViewState("F_SortBy") = Replace(e.SortExpression, ";", "")
        ViewState("F_PG") = 1
        BindRepeater()
    End Sub

    Sub ComputeSum(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            SumCount += DataBinder.Eval(e.Item.DataItem, "NumSales")
            CostSum += DataBinder.Eval(e.Item.DataItem, "TotalPrice")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(0).ColumnSpan = "4"
            e.Item.Cells(0).HorizontalAlign = HorizontalAlign.Right
            e.Item.Cells(0).Text = "<b>Page Total:</b><br><b>Grand Total:</b>"
            e.Item.Cells(1).Visible = False
            e.Item.Cells(2).Visible = False
            e.Item.Cells(3).Text = "<center><b>" & FormatCurrency(CostSum) & "<br>" & FormatCurrency(DB.ExecuteScalar("SELECT Coalesce(Sum(TotalPrice),0) FROM (" & BuildQuery(False) & ") as tmp")) & "</b></center>"
            e.Item.Cells(4).Text = "<center><b>" & FormatNumber(SumCount, 0) & "<br>" & FormatNumber(DB.ExecuteScalar("SELECT Coalesce(Sum(NumSales),0) FROM (" & BuildQuery(False) & ") as tmp"), 0) & "</b></center>"
            e.Item.Cells(5).Visible = False
        End If
    End Sub
End Class
