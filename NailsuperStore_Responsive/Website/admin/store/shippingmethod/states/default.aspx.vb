Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Controls
Imports Components

Public Class admin_store_states__default
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            BindRepeater()
        End If
    End Sub

    Private Sub BindRepeater()
        params = GetPageParams(FilterFieldType.All)

        ' BUILD QUERY
        Dim sConn As String
        sConn = " where "
        SQL = " SELECT StateId, StateCode, StateName, case when IncludeDelivery = 1 Then '<font color=green>yes</font>' else '<font color=red>no</font>' end As IncludeDelivery, case when IncludeGiftWrap = 1 Then '<font color=green>yes</font>' else '<font color=red>no</font>' end as IncludeGiftWrap, case when TaxRate is null then '' else cast(TaxRate As varchar(10)) + '%' end As TaxRate FROM State s "
        SQL = SQL & " ORDER BY StateName ASC "

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
        BindRepeater()
    End Sub
End Class