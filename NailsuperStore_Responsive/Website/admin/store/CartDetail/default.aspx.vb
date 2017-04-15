Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO

Partial Class admin_store_cartDetail_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_BillToCounty.Items.AddRange(StateRow.GetStateList().ToArray())
            F_BillToCounty.Databind()
            F_BillToCounty.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_BillToCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            F_BillToCountry.DataBind()
            F_BillToCountry.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_OrderNo.Text = Request("F_OrderNo")
            F_BillToSalonName.Text = Request("F_BillToSalonName")
            F_BillToName.Text = Request("F_BillToName")
            F_BillToName2.Text = Request("F_BillToName2")
            F_BillToZipcode.Text = Request("F_BillToZipcode")
            F_Email.Text = Request("F_Email")
            F_BillToCounty.SelectedValue = Request("F_BillToCounty")
            F_BillToCountry.SelectedValue = Request("F_BillToCountry")
            F_OrderDateLbound.Text = Request("F_OrderDateLBound")
            F_OrderDateUBound.Text = Request("F_OrderDateUBound")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreateDate"
                gvList.SortOrder = "desc"
            End If
            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub ExportList()
    End Sub

    Private Sub BindList()
        Dim SQLFields As String

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " OrderId, OrderNo, BillToSalonName, BillToName, BillToName2, BillToCounty, BillToCountry,remoteip, Email, ProcessDate,CreateDate "
        'SQL = BuildQuery()
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataSet = DB.GetDataSet(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub BuildQuery()
        Dim SQL As String = " FROM Vie_StoreOrder WHERE ProcessDate is null "
        Dim Conn As String = " AND "

        If Not F_BillToCounty.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "BillToCounty = " & DB.Quote(F_BillToCounty.SelectedValue)
            Conn = " AND "
        End If
        If Not F_BillToRegion.Text = String.Empty Then
            SQL = SQL & Conn & "BillToRegion = " & DB.Quote(F_BillToRegion.Text)
            Conn = " AND "
        End If
        If Not F_BillToCountry.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "BillToCountry = " & DB.Quote(F_BillToCountry.SelectedValue)
            Conn = " AND "
        End If
        If Not F_OrderNo.Text = String.Empty Then
            SQL = SQL & Conn & "OrderNo LIKE " & DB.FilterQuote(F_OrderNo.Text)
            Conn = " AND "
        End If
        If Not F_BillToSalonName.Text = String.Empty Then
            SQL = SQL & Conn & "BillToSalonName LIKE " & DB.FilterQuote(F_BillToSalonName.Text)
            Conn = " AND "
        End If
        If Not F_BillToName.Text = String.Empty Then
            SQL = SQL & Conn & "BillToName LIKE " & DB.FilterQuote(F_BillToName.Text)
            Conn = " AND "
        End If
        If Not F_BillToName2.Text = String.Empty Then
            SQL = SQL & Conn & "BillToName2 LIKE " & DB.FilterQuote(F_BillToName2.Text)
            Conn = " AND "
        End If
        If Not F_BillToZipcode.Text = String.Empty Then
            SQL = SQL & Conn & "BillToZipcode LIKE " & DB.FilterQuote(F_BillToZipcode.Text)
            Conn = " AND "
        End If
        If Not F_Email.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
            Conn = " AND "
        End If
        If Not F_OrderDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "CreateDate >= " & DB.Quote(F_OrderDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_OrderDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "CreateDate < " & DB.Quote(DateAdd("d", 1, F_OrderDateUbound.Text))
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub Export()
        Dim SQLFields As String = "SELECT OrderId, OrderNo, BillToSalonName, BillToName, BillToName2, BillToCounty, BillToCountry, Email, ProcessDate "
        'Dim SQL As String = BuildQuery()
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & ViewState("F_SortBy") & " " & ViewState("F_SortOrder"))

        Dim Folder As String = "/assets/temp/"
        Dim i As Integer = 0
        Dim FileName As String = Core.GenerateFileID & ".csv"

        Dim sw As StreamWriter = New StreamWriter(Server.MapPath(Folder & FileName), False)
        sw.WriteLine("Web Orders Report")
        sw.WriteLine("Report generated on " & DateTime.Now.ToString("d"))
        sw.WriteLine(String.Empty)
        sw.WriteLine("Search Criteria")
        sw.WriteLine("Order No:," & F_OrderNo.Text)
        sw.WriteLine("Bill To Salon:," & F_BillToSalonName.Text)
        sw.WriteLine("Bill To Name:," & F_BillToName.Text)
        sw.WriteLine("Bill To Name 2:," & F_BillToName2.Text)
        sw.WriteLine("Bill To State:," & F_BillToCounty.Text)
        sw.WriteLine("Bill To Zip:," & F_BillToZipcode.Text)
        sw.WriteLine("Bill To Country:," & F_BillToCountry.Text)
        sw.WriteLine("Order From Date:," & F_OrderDateLbound.Value)
        sw.WriteLine("Order To Date:," & F_OrderDateUbound.Value)
        sw.WriteLine(String.Empty)
        sw.WriteLine(String.Empty)

        sw.WriteLine("Order No,Bill To Salon Name,Bill To Name,Bill To Name 2,Bill To State,Bill To Country,Email,Order Date")
        For Each dr As DataRow In res.Rows
            sw.WriteLine(Core.QuoteCSV(Convert.ToString(dr("OrderNo"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToSalonName"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToName"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToName2"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToCounty"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToCountry"))) & "," & Core.QuoteCSV(Convert.ToString(dr("Email"))) & "," & Core.QuoteCSV(Convert.ToString(dr("processdate"))))
        Next
        sw.Flush()
        sw.Close()

        lnkDownload.NavigateUrl = Folder & FileName
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub
        gvList.PageIndex = 0
        gvList.Visible = True
        BuildQuery()
        BindList()
    End Sub
End Class

