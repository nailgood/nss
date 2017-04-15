Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_members_LevelPoint
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            'LoadYear()
            LoadData()
        End If
    End Sub
    Private Sub LoadData()
        Dim dt As DataTable = DB.GetDataTable("Select * From LevelPoint")
        If dt.Rows.Count > 2 Then
            txtSMember.Text = dt.Rows(0)("Discount")
            txtSMsg.Text = dt.Rows(0)("Description")
            txtSStartPoint.Text = dt.Rows(0)("StartPoint")
            txtGMember.Text = dt.Rows(1)("Discount")
            txtGMsg.Text = dt.Rows(1)("Description")
            txtGStartPoint.Text = dt.Rows(1)("StartPoint")
            txtPMember.Text = dt.Rows(2)("Discount")
            txtPMsg.Text = dt.Rows(2)("Description")
            txtPStartPoint.Text = dt.Rows(2)("StartPoint")
        End If
    End Sub

    'Private Sub LoadYear()
    '    Dim j As Integer
    '    For j = 0 To (Now.Year + 1) - 2010
    '        drpYear.Items.Insert(j, New ListItem(2010 + j, 2010 + j))
    '    Next
    '    drpYear.SelectedValue = Now.Year
    'End Sub

    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    '    txtSMember.Text = ""
    '    txtGMember.Text = ""
    '    txtPMember.Text = ""
    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Dim lvp As New LevelPointRow
            Try
                lvp.LevelId = 1
                lvp.Discount = txtSMember.Text
                lvp.Description = txtSMsg.Text
                lvp.StartPoint = txtSStartPoint.Text
                lvp.AdminId = Session("AdminId")
                lvp.ModifyDate = Date.Now
                lvp.Update()
            Catch ex As Exception
                lblMsg.Text = "Update " & txtSMsg.Text & " error"
                Exit Sub
            End Try
            Try
                lvp.LevelId = 2
                lvp.Discount = txtGMember.Text
                lvp.Description = txtGMsg.Text
                lvp.StartPoint = txtGStartPoint.Text
                lvp.AdminId = Session("AdminId")
                lvp.ModifyDate = Date.Now
                lvp.Update()
            Catch ex As Exception
                lblMsg.Text = "Update " & txtGMsg.Text & " error"
                Exit Sub
            End Try
            Try
                lvp.LevelId = 3
                lvp.Discount = txtPMember.Text
                lvp.Description = txtPMsg.Text
                lvp.StartPoint = txtPStartPoint.Text
                lvp.AdminId = Session("AdminId")
                lvp.ModifyDate = Date.Now
                lvp.Update()
            Catch ex As Exception
                lblMsg.Text = "Update " & txtPMsg.Text & " error"
                Exit Sub
            End Try
            lblMsg.Text = "Update success"
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        LoadData()
        lblMsg.Text = ""
    End Sub
End Class
