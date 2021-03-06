Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient
Partial Class admin_ImExLog_Index
    Inherits AdminPage
    Dim LogID As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LogID = Convert.ToInt32(Request("LogID"))
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_LogTypeId.Items.Insert(0, New ListItem("Export Log", "0"))
            F_LogTypeId.Items.Insert(1, New ListItem("Import Log", "1"))
            If LogID <> 0 Then

                Dim strPath As String = ""
                Dim filename As String = ""
                Dim sContents As String = ""
                Dim dbImExLog As ImExLogRow = ImExLogRow.GetRow(DB, LogID)
                filename = dbImExLog.LogFile
                ' Trung nguyen add - fix bug return Logtype to default when view logfile content
                F_LogTypeId.SelectedIndex = F_LogTypeId.Items.IndexOf(F_LogTypeId.Items.FindByValue(dbImExLog.LogTypeID))

                If dbImExLog.LogTypeID = 1 Then
                    strPath = System.Configuration.ConfigurationManager.AppSettings("sImLogPath")
                Else
                    strPath = System.Configuration.ConfigurationManager.AppSettings("sExLogPath")
                End If
                F_LogTypeId.DataBind()
                Try
                    If Core.FileExists(strPath & filename) = True Then
                        sContents = Core.OpenFile(strPath & filename)
                    End If
                    lblContents.Text = dbImExLog.LogFile
                    F_LogTypeId.SelectedValue = dbImExLog.LogTypeID
                    F_Contents.Text = sContents
                Catch ex As Exception
                    F_Contents.Text = sContents
                End Try

            End If
            ' Trung Nguyen fix sort bug
            If Not Core.ProtectParam(Request("F_SortBy")) = String.Empty Then
                gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            End If
            If Not Core.ProtectParam(Request("F_SortOrder")) = String.Empty Then
                gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            End If
            'If gvList.SortBy = String.Empty Then gvList.SortBy = "LogDate"
            BindList()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM ImExLog  "

        If Not F_LogTypeId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "LogType = " & DB.Quote(F_LogTypeId.SelectedValue)
            Conn = " AND "
        End If

        If Not F_StartDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "LogDate >= " & DB.Quote(F_StartDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_StartDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "LogDate < " & DB.Quote(DateAdd("d", 1, F_StartDateLbound.Text))
            Conn = " AND "
        End If

        'If Not F_Login.Text = String.Empty Then
        '    SQL = SQL & Conn & "Login = " & DB.Quote(F_Login.Text)
        '    Conn = " AND "
        'End If
        'If Not F_ContactNo.Text = String.Empty Then
        '    SQL = SQL & Conn & "ContactNo LIKE " & DB.FilterQuote(F_ContactNo.Text)
        '    Conn = " AND "
        'End If
        'If Not F_ContactName.Text = String.Empty Then
        '    SQL = SQL & Conn & "ContactName LIKE " & DB.FilterQuote(F_ContactName.Text)
        '    Conn = " AND "
        'End If
        'If Not F_ContactName2.Text = String.Empty Then
        '    SQL = SQL & Conn & "ContactName2 LIKE " & DB.FilterQuote(F_ContactName2.Text)
        '    Conn = " AND "
        'End If

        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub
        '      Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings("sLogMailPath")
        '      Dim filename As String = "EmailLog_" & Date.Now().Year & "_" & Date.Now().Month & "_" & Date.Now().Day & ".txt"
        '      Dim sContents As String = ""
        '      Dim sfilechose As String = ""
        '      Dim dDate As Date
        '      Try
        '          dDate = CDate(F_StartDateLbound.Text)
        '          sfilechose = "EmailLog_" & dDate.Year & "_" & dDate.Month & "_" & dDate.Day & ".txt"
        '          If Core.FileExists(strPath & sfilechose) = True Then
        '              sContents = Core.OpenFile(strPath & sfilechose)
        '          End If
        '          'sContents = sContents & "Date: " & Date.Now() & " Send Email: " & sEmail & " Subject: " & sSubject & " Status: "
        '          F_Contents.Text = sContents
        '      Catch ex As Exception
        '          F_Contents.Text = sContents
        '      End Try
        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub F_LogTypeId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles F_LogTypeId.SelectedIndexChanged
        lblContents.Text = ""
        F_Contents.Text = ""
        BindList()
    End Sub
End Class

