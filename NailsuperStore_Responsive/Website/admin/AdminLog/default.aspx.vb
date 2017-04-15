Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO

Partial Class admin_AdminLog_Default
    Inherits AdminPage
    Dim ToTal As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindListData

        If Not IsPostBack Then
            BindListData()
        End If
    End Sub
    Private Sub BindListData()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "
        ''Dim dateVariable As DateTime = New DateTime(F_FromDate.Value.Year, dtStartingDate.Value.Month, dtStartingDate.Value.Day, CInt(txtStartHour.Text), CInt(txtStartMinute.Text), 0)

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " log.*,logdt.*,(am.FirstName + ' ' + am.LastName) as FullName"
        SQL = " FROM AdminLog log left join AdminLogDetail logdt on(log.LogId=logdt.LogId) left join Admin am on(am.AdminId=log.AdminId) where 1=1 "

        If Not F_FromDate.Text = String.Empty Then
            SQL = SQL & " and DATEDIFF(DD,'" & F_FromDate.Value & "',LoginDate)>=0"
        End If
        If Not F_ToDate.Text = String.Empty Then
            SQL = SQL & " and DATEDIFF(DD,LoginDate,'" & F_ToDate.Value & "')>=0"
        End If
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY [LoginDate] DESC")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not Page.IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindListData()
    End Sub
End Class
