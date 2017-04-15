Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Controls
Imports Components
Imports DataLayer

Public Class admin_catalog_default
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            BindRepeater()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindRepeater()
        params = GetPageParams(FilterFieldType.All)

        ' BUILD QUERY
        Dim sConn As String
        sConn = " where "
        SQL = " SELECT CatalogId, Title, CASE WHEN IsActive = 1 Then '<b><font color=green>YES</font></b>' Else '<b><font color=red>NO</font></b>' End As IsActive, CatalogImage, SortOrder FROM StoreCatalog s"
        SQL = SQL & " ORDER BY SortOrder ASC "

        'Dim res As DataSet = DB.GetDataSet(SQL)
        Dim res As DataTable = DB.GetDataTable(SQL)

        myNavigator.NofRecords = res.Rows.Count
        myNavigator.MaxPerPage = dgList.PageSize
        myNavigator.PageNumber = Math.Max(Math.Min(CType(ViewState("F_PG"), Integer), myNavigator.NofPages), 1)
        myNavigator.DataBind()

        ViewState("F_PG") = myNavigator.PageNumber
        tblList.Visible = (myNavigator.NofRecords <> 0)
        plcNoRecords.Visible = (myNavigator.NofRecords = 0)

        dgList.DataSource = res.DefaultView
        dgList.CurrentPageIndex = ViewState("F_PG") - 1
        dgList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As Controls.NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindRepeater()
    End Sub
End Class