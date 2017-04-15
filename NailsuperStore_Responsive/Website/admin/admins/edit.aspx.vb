Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_admins_edit
    Inherits AdminPage

    Protected AdminId As Integer
    Protected sUsername As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        AdminId = Convert.ToInt32(Request("AdminId"))
        If AdminId <> 0 Then
            'If Edit Mode, then turn off initially validators
            PASSWORDVALIDATOR1.Enabled = False
            PASSWORDVALIDATOR2.Enabled = False
            PasswordLenghtValidator.Enabled = False
            PASSWORDCOMPAREVALIDATOR.Enabled = False
            trPassword.Visible = False
            trPassword1.Visible = False
            trPassword2.Visible = False
        Else
            'If Add mode, then don't display delete and password row
            Delete.Visible = False
            trPassword.Visible = False
        End If

        If Not IsPostBack Then
            If AdminId <> 0 Then
                Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, AdminId)
                Username.Text = dbAdmin.Username
                sUsername = dbAdmin.Username
                FIRSTNAME.Text = dbAdmin.FirstName
                LASTNAME.Text = dbAdmin.LastName
                Email.Text = dbAdmin.Email
                chkIsActive.Checked = dbAdmin.IsActive
                If (sUsername = CType(Context.User.Identity, AdminIdentity).Username) Then
                    chkIsActive.Enabled = False
                    Delete.Enabled = False
                    Delete.Visible = False
                End If
            End If
            BindPrivileges()
        Else
            PASSWORD1.Attributes.Add("value", Request("PASSWORD1"))
            PASSWORD2.Attributes.Add("value", Request("PASSWORD2"))
        End If
    End Sub

    Private Sub BindPrivileges()
        'Bind Left List
        Dim dsLeft As DataSet = AdminAdminGroupRow.LoadGroupsWithoutPrivileges(AdminId)
        lbLeft.DataSource = dsLeft.Tables(0).DefaultView
        lbLeft.DataValueField = "GroupId"
        lbLeft.DataTextField = "Description"
        lbLeft.DataBind()

        'Bind Right List
        If AdminId <> 0 Then
            Dim dsRight As DataSet = AdminAdminGroupRow.LoadGroupsWithPrivileges(AdminId)
            lbRight.DataSource = dsRight.Tables(0).DefaultView
            lbRight.DataValueField = "GroupId"
            lbRight.DataTextField = "Description"
            lbRight.DataBind()
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        'If the user entered anything in the password fields
        'then turn on password validators
        'If Not PASSWORD1.Text = String.Empty Or Not PASSWORD2.Text = String.Empty Then
        '    PASSWORDVALIDATOR1.Enabled = True
        '    PASSWORDVALIDATOR2.Enabled = True
        '    PasswordLenghtValidator.Enabled = True

        '    'Force Page Validation	
        '    Page.Validate()
        'End If
        'If Not IsValid Then Exit Sub

        Try
            DB.BeginTransaction()

            If AdminId <> 0 Then
                Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, AdminId)
                dbAdmin.Username = Username.Text
                'If Not DB.IsEmpty(PASSWORD1.Text) Then
                '    dbAdmin.Password = PASSWORD1.Text
                'End If
                dbAdmin.FirstName = FIRSTNAME.Text
                dbAdmin.LastName = LASTNAME.Text
                dbAdmin.Email = EMAIL.Text
                If (Not sUsername = CType(Context.User.Identity, AdminIdentity).Username) Then
                    dbAdmin.IsActive = chkIsActive.Checked
                End If
                dbAdmin.Update()
                WriteLogDetail("Update Admin User", dbAdmin)
            Else
                Dim dbAdmin As AdminRow = New AdminRow(DB)
                dbAdmin.Username = Username.Text
                dbAdmin.Password = PASSWORD1.Text
                dbAdmin.FirstName = FIRSTNAME.Text
                dbAdmin.LastName = LASTNAME.Text
                dbAdmin.Email = EMAIL.Text
                dbAdmin.IsActive = chkIsActive.Checked
                AdminId = dbAdmin.AutoInsert()
                WriteLogDetail("Insert Admin User", dbAdmin)
            End If

            AdminAdminGroupRow.RemoveByAdmin(AdminId)

            'INSERT ONLY SELECTED PRIVILEGES
            For Each item As ListItem In lbRight.Items
                Dim dbAdminAdminGroup As AdminAdminGroupRow = New AdminAdminGroupRow(DB)
                dbAdminAdminGroup.AdminId = AdminId
                dbAdminAdminGroup.GroupId = CInt(item.Value)
                dbAdminAdminGroup.Insert()
            Next
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        If AdminId <> 0 Then
            Response.Redirect("delete.aspx?AdminId=" & AdminId & "&" & GetPageParams(FilterFieldType.All))
        End If
    End Sub

    Private Sub btnRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRight.Click
        Dim item As ListItem
        Dim i As Integer = 0

        While i < lbLeft.Items.Count
            If lbLeft.Items(i).Selected Then
                item = lbLeft.Items(i)
                item.Selected = False
                Dim bInserted As Boolean = False
                For j As Integer = 0 To lbRight.Items.Count - 1
                    If lbRight.Items(j).Text > item.Text Then
                        lbRight.Items.Insert(j, item)
                        bInserted = True
                        Exit For
                    End If
                Next
                If Not bInserted Then
                    lbRight.Items.Add(item)
                End If
                lbLeft.Items.RemoveAt(i)
            Else
                i += 1
            End If
        End While
    End Sub

    Private Sub btnLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        Dim item As ListItem
        Dim i As Integer = 0

        While i < lbRight.Items.Count
            If lbRight.Items(i).Selected Then
                item = lbRight.Items(i)
                item.Selected = False
                Dim bInserted As Boolean = False
                For j As Integer = 0 To lbLeft.Items.Count - 1
                    If lbLeft.Items(j).Text > item.Text Then
                        lbLeft.Items.Insert(j, item)
                        bInserted = True
                        Exit For
                    End If
                Next
                If Not bInserted Then
                    lbLeft.Items.Add(item)
                End If
                lbRight.Items.RemoveAt(i)
            Else
                i += 1
            End If
        End While
    End Sub

End Class
