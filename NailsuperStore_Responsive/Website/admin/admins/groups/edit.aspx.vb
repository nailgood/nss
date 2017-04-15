Imports Components
Imports DataLayer
Imports System.Data
Imports System.Data.SqlClient

Partial Class admin_admins_groups_edit
    Inherits AdminPage

    Private GroupId As Integer
    Private dbAdminGroup As AdminGroupRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        GroupId = Convert.ToInt32(Request("GroupId"))
        If Not IsPostBack Then
            If GroupId = 0 Then
                Delete.Visible = False
            Else
                LoadFromDB()
            End If
            BindPrivileges()
        End If
    End Sub

    Private Sub LoadFromDB()
        dbAdminGroup = AdminGroupRow.GetRow(GroupId)
        DESCRIPTION.Text = dbAdminGroup.Description
    End Sub

    Private Sub BindPrivileges()
        'Bind Left List
        Dim dsLeft As DataSet = AdminAccessRow.LoadSectionsWithoutPrivileges(DB, GroupId)
        lbLeft.DataSource = dsLeft.Tables(0).DefaultView
        lbLeft.DataValueField = "SectionId"
        lbLeft.DataTextField = "Description"
        lbLeft.DataBind()

        'Bind Right List
        If GroupId <> 0 Then
            Dim dsRight As DataSet = AdminAccessRow.LoadSectionsWithPrivileges(DB, GroupId)
            lbRight.DataSource = dsRight.Tables(0).DefaultView
            lbRight.DataValueField = "SectionId"
            lbRight.DataTextField = "Description"
            lbRight.DataBind()
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            If GroupId <> 0 Then
                Dim dbAdminGroup As AdminGroupRow = AdminGroupRow.GetRow(GroupId)
                dbAdminGroup.Description = DESCRIPTION.Text
                dbAdminGroup.Update()
            Else
                Dim dbAdminGroup As AdminGroupRow = New AdminGroupRow(DB)
                dbAdminGroup.Description = DESCRIPTION.Text
                GroupId = dbAdminGroup.AutoInsert()
            End If

            'DELETE ALL PRIVILEGES
            AdminAccessRow.RemoveByGroup(GroupId)

            'INSERT ONLY SELECTED PRIVILEGES
            For Each item As ListItem In lbRight.Items
                Dim dbAdminAccess As AdminAccessRow = New AdminAccessRow(DB)

                dbAdminAccess.GroupId = GroupId
                dbAdminAccess.SectionId = CInt(item.Value)
                dbAdminAccess.Insert()
            Next
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Finally
            If Not DB Is Nothing Then DB.Close()
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        If GroupId <> 0 Then
            Response.Redirect("delete.aspx?GroupId=" & GroupId & "&" & GetPageParams(FilterFieldType.All))
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
