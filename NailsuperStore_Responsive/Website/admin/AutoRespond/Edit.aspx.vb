Imports DataLayer
Imports Components

Partial Class admin_AutoRespond_Edit
    Inherits AdminPage
    Protected DayId As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("DayId") <> Nothing Then
            DayId = Request("DayId")
        End If
        If Not Page.IsPostBack Then
            LoadDepartment()
            LoadData()
        End If
    End Sub
    Private Sub LoadData()
        Dim Au As AutoRespondRow = AutoRespondRow.GetRow(DB, DayId)
        txtDayName.Text = Au.DayName
        dtStartingDate.Value = Au.StartingDate.Date
        dtEndingDate.Value = Au.EndingDate.Date
        ddlStart.SelectedValue = IIf(Au.StartingDate.Hour = 0, 12, Au.StartingDate.Hour)
        ddlEnd.SelectedValue = IIf(Au.EndingDate.Hour = 0, 12, Au.EndingDate.Hour)
    End Sub
    Private Sub LoadDepartment()
        Dim i As Integer = 0
        For i = 0 To 24
            If i < 12 Then
                ddlStart.Items.Insert(i, New ListItem(i + 1 & " AM", i + 1))
                ddlEnd.Items.Insert(i, New ListItem(i + 1 & " AM", i + 1))
            Else
                If i - 11 < 13 Then
                    ddlStart.Items.Insert(i, New ListItem(i - 11 & " PM", i + 1))
                    ddlEnd.Items.Insert(i, New ListItem(i - 11 & " PM", i + 1))
                End If

            End If
        Next
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Dim Au As New AutoRespondRow
        'Try
        Au.AdminId = Session("AdminId")
        Au.StartingDate = dtStartingDate.Value.AddHours(CDbl(ddlStart.SelectedValue))
        If (dtEndingDate.Text <> Nothing) Then
            Au.EndingDate = dtEndingDate.Value.AddHours(CDbl(ddlEnd.SelectedValue))
        Else
            Au.EndingDate = dtEndingDate.Value.AddHours(0)
        End If

        Au.DayName = txtDayName.Text
        If DayId = 0 Then
            Au.Insert()
        Else
            Au.DayId = DayId
            Au.Update()
        End If
        Response.Redirect("default.aspx")
        'Catch ex As Exception

        'End Try

    End Sub
End Class
