Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO

Partial Class admin_members_reportpoint
    Inherits AdminPage
    Protected TotalPavailable As Integer = 0
    Protected Worth As String = ""
    Protected PointPending As Integer = 0
    Protected PointsAccumulatedMonth As Integer = 0
    Protected PointsAccumulatedYear As Integer = 0
    Protected PointEarnedUptodate As Integer = 0
    Protected PointDebitUptodate As Integer = 0
    Protected PointDebitInMonth As Integer = 0
    Protected Month As Integer = 0
    Protected Year As Integer = 0
    Protected MonthN As String = ""
   
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindData
        If Not Page.IsPostBack Then
            LoadMonthYear()
            BindData()
        End If
    End Sub
   
    Private Sub LoadMonthYear()
        If Not Page.IsPostBack Then
            Dim i, j As Integer
            For i = 0 To 11
                drpMonth.Items.Insert(i, New ListItem(i + 1, i + 1))
            Next
            drpMonth.SelectedValue = Now.Month
            For j = 0 To Now.Year - 2010
                drpYear.Items.Insert(j, New ListItem(2010 + j, 2010 + j))
            Next
            drpYear.SelectedValue = Now.Year
        End If

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        BindData()
    End Sub
    Private Sub LoadTotalPoint()
        Dim CashPoint As CashPointCollection = CashPointRow.GetTotalPoint(Month, Year)
        TotalPavailable = CashPoint.TTPointAvailable
        PointPending = CashPoint.TTPointPending
        PointsAccumulatedMonth = CashPoint.TTPointAvailableInMonth
        PointsAccumulatedYear = CashPoint.TTPointAvailableInYear
        PointEarnedUptodate = CashPoint.TTPointEarnedUptodate
        PointDebitUptodate = CashPoint.TTPointDebit
        PointDebitInMonth = CashPoint.TTPointDebitInMonth
        Worth = "(worth $" & TotalPavailable * SysParam.GetValue("MoneyEachPoint") & ")"
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ca As CashPointRow = e.Row.DataItem
            Dim ltName As Literal = CType(e.Row.FindControl("ltName"), Literal)
            Dim ltTotalPointavailable As Literal = CType(e.Row.FindControl("ltTotalPointavailable"), Literal)
            ltTotalPointavailable.Text = ca.TotalPointAvailable & " " & ca.Worth
            ltName.Text = String.Format("{0}<br><a href=""mailto:{1}"">{1}</a>", ca.FirstName & " " & ca.LastName, ca.Email)
        End If
    End Sub
    Private Sub BindData()
        Year = drpYear.SelectedValue
        Month = drpMonth.SelectedValue
        MonthN = MonthName(Month, False)
        Dim data As CashPointCollection
        Dim CountItem As Integer = 0
        Dim total As Integer = 0
        Dim sortBy As String = gvList.SortBy
        If (sortBy = "Pointsaccumulatedinmonth") Then
            sortBy = "dbo.fc_CashPoint_GetPointInMonthByMember(M.MemberId," + Month.ToString + "," + Year.ToString + ")"
        ElseIf (sortBy = "Pointsaccumulatedinyear") Then
            sortBy = "dbo.fc_CashPoint_GetPointInYearByMember(M.MemberId," + Year.ToString() + ")"
        ElseIf (sortBy = "Pointsearneduptodate") Then
            sortBy = "dbo.fc_CashPoint_GetPointsEarnedUpToDateByMember(M.MemberId)"
        ElseIf (sortBy = "PointDebitinMonth") Then
            sortBy = "dbo.fc_CashPoint_GetPointDebitInMonthByMember(M.MemberId," + Month.ToString() + "," + Year.ToString() + ")"
        ElseIf (sortBy = "Pointsdebituptodate") Then
            sortBy = "dbo.fc_CashPoint_GetPointDebitUptoDateByMember(M.MemberId)"
        End If
        data = CashPointRow.GetReportPoint1(Month, Year, gvList.PageIndex + 1, gvList.PageSize, sortBy, gvList.SortOrder, total)
        CountItem = data.TotalRecords
        gvList.Pager.NofRecords = total
        gvList.PageSelectIndex = gvList.PageIndex
        gvList.DataSource = data
        gvList.DataBind()
        ''load point total
        LoadTotalPoint()
    End Sub

   
    
End Class
