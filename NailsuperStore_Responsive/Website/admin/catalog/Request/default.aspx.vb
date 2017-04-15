Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Controls
Imports Components
Imports system.IO
Imports DataLayer

Public Class admin_catalog_request_default
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BuildQuery()
            BindRepeater()
        End If
    End Sub

    Private Sub Export()
        'Dim SQL As String = BuildQuery()
        Dim res As DataTable = DB.GetDataTable(hidCon.Value & " ORDER BY " & ViewState("F_SortBy") & " " & ViewState("F_SortOrder"))

        Dim Folder As String = "/assets/catalog/"
        Dim i As Integer = 0
        Dim FileName As String = Core.GenerateFileID & ".csv"

        Dim sw As StreamWriter = New StreamWriter(Server.MapPath(Folder & FileName), False)
        sw.WriteLine("Web Orders Report")
        sw.WriteLine("Report generated on " & DateTime.Now.ToString("d"))
        sw.WriteLine(String.Empty)
        sw.WriteLine("Search Criteria")
        sw.WriteLine("First Name:," & F_Firstname.Text)
        sw.WriteLine("Last Name:," & F_LastName.Text)
        sw.WriteLine("Date Requested From:," & F_DateRequestedStart.Value)
        sw.WriteLine("Date Requested To:," & F_DateRequestedEnd.Value)
        sw.WriteLine(String.Empty)
        sw.WriteLine(String.Empty)

        sw.WriteLine("Name,Company,Address 1,Address 2,City,State,Zip,Phone,Extension,Email,Date Requested")
        For Each dr As DataRow In res.Rows
            sw.WriteLine(Core.QuoteCSV(Convert.ToString(dr("Name"))) & "," _
             & Core.QuoteCSV(Convert.ToString(dr("company"))) & "," & Core.QuoteCSV(Convert.ToString(dr("address1"))) & "," & Core.QuoteCSV(Convert.ToString(dr("address2"))) & "," & Core.QuoteCSV(Convert.ToString(dr("city"))) _
             & "," & Core.QuoteCSV(Convert.ToString(dr("state"))) & "," & Core.QuoteCSV(Convert.ToString(dr("zip"))) & "," & Core.QuoteCSV(Convert.ToString(dr("phone"))) & "," & Core.QuoteCSV(Convert.ToString(dr("phoneext"))) _
            & "," & Core.QuoteCSV(Convert.ToString(dr("email"))) & "," & Core.QuoteCSV(Convert.ToString(dr("daterequested"))))
        Next
        sw.Flush()
        sw.Close()

        lnkDownload.NavigateUrl = Folder & FileName
    End Sub
    'Long edit 05/11/2009
    Private Sub BindRepeater()
        params = GetPageParams(FilterFieldType.All)
        If Not IsPostBack Then
            ViewState("F_SortBy") = Replace(Request("F_SortBy"), ";", "")
            ViewState("F_SortOrder") = Replace(Request("F_SortOrder"), ";", "")
            ViewState("F_PG") = Request("F_PG")
        End If
        If ViewState("F_SortBy") = String.Empty Then
            ViewState("F_SortBy") = "Name"
        End If
        If ViewState("F_SortOrder") = String.Empty Then
            ViewState("F_SortOrder") = "ASC"
        End If
        If CType(ViewState("F_PG"), String) = String.Empty Then
            ViewState("F_PG") = 1
        End If

        ' BUILD QUERY
        'SQL = BuildQuery()
        If ViewState("F_SortBy").ToString.Contains("DateRequested") = False Then
            ViewState("F_SortBy") = " DateRequested "
            ViewState("F_SortOrder") = " Desc"
        End If
        hidCon.Value = hidCon.Value & " ORDER BY " & ViewState("F_SortBy") & " " & ViewState("F_SortOrder")

        'Dim res As DataSet = DB.GetDataSet(SQL)

        Dim res As DataTable = DB.GetDataTable(hidCon.Value)
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
    'End 05/11/2009
    Private Sub BuildQuery()
        Dim sConn, SQL As String
        sConn = " where "
        SQL = " SELECT RequestId, LastName + ', ' + Firstname As Name, DateRequested, State, Email, company, address1, address2,city,zip,phone, phoneext " & _
        " FROM StoreCatalogRequest scr "

        If Not DB.IsEmpty(F_Firstname.Text) Then
            SQL = SQL & sConn & " scr.FirstName like " & DB.FilterQuote(F_Firstname.Text)
            sConn = " AND "
        End If
        If Not DB.IsEmpty(F_LastName.Text) Then
            SQL = SQL & sConn & " scr.LastName like " & DB.FilterQuote(F_LastName.Text)
            sConn = " AND "
        End If
        If Not F_DateRequestedStart.Value = Nothing Then
            SQL = SQL & sConn & " scr.DateRequested >= " & DB.Quote(F_DateRequestedStart.Value)
            sConn = " AND "
        End If
        If Not F_DateRequestedEnd.Value = Nothing Then
            SQL = SQL & sConn & " scr.DateRequested <= " & DB.Quote(F_DateRequestedEnd.Value)
            sConn = " AND "
        End If

        'Return SQL
        hidCon.Value = SQL
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub ResetSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetSearch.Click
        Response.Redirect("default.aspx?")
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ViewState("F_PG") = 1

        If F_OutputAs.SelectedValue = "Excel" Then
            divDownload.Visible = True
            dgList.Visible = False
            myNavigator.Visible = False
            Export()
        Else
            divDownload.Visible = False
            dgList.Visible = True
            myNavigator.Visible = True
            BuildQuery()
            BindRepeater()
        End If
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

End Class