Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports Utility.Common
Imports System.IO
Imports System.Web.Services

Partial Class admin_pending_order_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_IsGuest.SelectedValue = Request("F_IsGuest")
            F_OrderDateLbound.Text = Request("F_OrderDateLBound")
            F_OrderDateUbound.Text = Request("F_OrderDateUBound")

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

    'Private Sub ExportList()
    'End Sub

    Private Sub BindList()
        Dim SQLFields As String

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * (gvList.PageSize) & " m.MemberId, so.OrderId, so.ShipToName, so.ShipToName2, so.ShipToCounty, so.ShipToCountry, so.Email, so.CreateDate, so.Total,  m.IsActive, (SELECT TOP 1 [Action] FROM StoreOrderLog WHERE OrderId = so.OrderId ORDER BY CreatedDate DESC) As 'Action', (SELECT TOP 1 PageName FROM StoreOrderLog WHERE OrderId = so.OrderId ORDER BY CreatedDate DESC) As 'PageName' "
        'SQL = BuildQuery()
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)

        Dim res As DataSet = DB.GetDataSet(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub


    Private Sub BuildQuery()
        Dim SQL As String = "FROM StoreOrder so LEFT JOIN Member m ON so.MemberId = m.MemberId WHERE so.OrderNo IS NULL and so.OrderId in (SELECT DISTINCT OrderId FROM StoreOrderLog)"
        Dim Conn As String = " AND "

        If F_IsGuest.SelectedValue = "1" Then
            SQL = SQL & " and m.GuestStatus = 0 and so.MemberID > 0 and m.IsActive = 1"
        ElseIf F_IsGuest.SelectedValue = "0" Then
            SQL = SQL & " and m.GuestStatus > 0 and so.MemberId > 0"
        ElseIf F_IsGuest.SelectedValue = "-1" Then
            SQL = SQL & "and so.MemberId = 0"
        End If

        If Not F_OrderDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "so.CreateDate >= " & DB.Quote(F_OrderDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_OrderDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "so.CreateDate < " & DB.Quote(DateAdd("d", 1, F_OrderDateUbound.Text))
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        'dinh dang cot Total'
        Dim ltTotal As Literal = CType(e.Row.FindControl("ltTotal"), Literal)
        ltTotal.Text = FormatCurrency(e.Row.DataItem("Total"))

        If e.Row.RowType = DataControlRowType.DataRow Then
            'dinh dang cot Country'
            e.Row.Cells(3).Text = "<center>" & e.Row.Cells(3).Text & "</center>"

            ''gan link member detail
            Dim p As System.Data.DataRowView = e.Row.DataItem
            Dim ltMember As Literal = CType(e.Row.FindControl("ltMember"), Literal)
            Dim name As String = String.Empty
            name = p("ShipToName").ToString()
            If (String.IsNullOrEmpty(name)) Then
                ltMember.Text = String.Empty
            Else
                ltMember.Text = "<a style='color: blue;' target='_blank' href='/admin/members/edit.aspx?MemberId=" & p("MemberId").ToString() & "&F_SortBy=CreateDate&F_SortOrder=desc'>" & name & "</a>"
            End If
        End If

    End Sub

    'Private Sub Export()
    '    Dim SQLFields As String = "SELECT so.ShipToName, so.ShipToName2, so.ShipToCounty, so.ShipToCountry, so.Email, sod.CreatedDate, so.Total, sod.Action  "
    '    'Dim SQL As String = BuildQuery()
    '    Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & ViewState("F_SortBy") & " " & ViewState("F_SortOrder"))

    '    Dim Folder As String = "/assets/temp/"
    '    Dim i As Integer = 0
    '    Dim FileName As String = Core.GenerateFileID & ".csv"

    '    Dim sw As StreamWriter = New StreamWriter(Server.MapPath(Folder & FileName), False)
    '    sw.WriteLine("Web Orders Report")
    '    sw.WriteLine("Report generated on " & DateTime.Now.ToString("d"))
    '    sw.WriteLine(String.Empty)
    '    sw.WriteLine("Search Criteria")
    '    sw.WriteLine("Order No:," & F_OrderNo.Text)
    '    sw.WriteLine("Bill To Salon:," & F_BillToSalonName.Text)
    '    sw.WriteLine("Bill To Name:," & F_BillToName.Text)
    '    sw.WriteLine("Bill To Name 2:," & F_BillToName2.Text)
    '    sw.WriteLine("Bill To State:," & F_BillToCounty.Text)
    '    sw.WriteLine("Bill To Zip:," & F_BillToZipcode.Text)
    '    sw.WriteLine("Bill To Country:," & F_BillToCountry.Text)
    '    sw.WriteLine("Order From Date:," & F_OrderDateLbound.Value)
    '    sw.WriteLine("Order To Date:," & F_OrderDateUbound.Value)
    '    sw.WriteLine(String.Empty)
    '    sw.WriteLine(String.Empty)

    '    sw.WriteLine("Order No,Bill To Salon Name,Bill To Name,Bill To Name 2,Bill To State,Bill To Country,Email,Order Date")
    '    For Each dr As DataRow In res.Rows
    '        sw.WriteLine(Core.QuoteCSV(Convert.ToString(dr("OrderNo"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToSalonName"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToName"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToName2"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToCounty"))) & "," & Core.QuoteCSV(Convert.ToString(dr("BillToCountry"))) & "," & Core.QuoteCSV(Convert.ToString(dr("Email"))) & "," & Core.QuoteCSV(Convert.ToString(dr("processdate"))))
    '    Next
    '    sw.Flush()
    '    sw.Close()

    '    lnkDownload.NavigateUrl = Folder & FileName
    'End Sub

    <WebMethod()> _
    Public Shared Function GetLatestStep(ByVal param1 As Integer) As String
        Dim html As String = String.Empty
        Dim iOrderId As Integer = 0
        iOrderId = param1
        Dim result As String = StoreOrderLogRow.GetLatestStep(iOrderId)
        html = "" & result & ""
        Return html
    End Function


    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub
        gvList.PageIndex = 0
        gvList.Visible = True
        BuildQuery()
        BindList()
    End Sub

End Class

