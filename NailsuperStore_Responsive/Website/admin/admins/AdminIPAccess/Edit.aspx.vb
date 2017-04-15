Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_admins_AdminIPAccess_Edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadFromDB()
        End If

    End Sub
    Public Function UserName() As String
        Try
            Return Request("username")
        Catch ex As Exception

        End Try
        Response.Redirect("../admins/default.aspx")
        Return String.Empty
    End Function

    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim objData As AdminIPAccessRow = AdminIPAccessRow.GetRow(DB, Id)
        txtIP.Text = objData.IP
       
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try
                If Not IsValidIP(txtIP.Text.Trim()) Then
                    AddError("IP Address is not valid")
                    Exit Sub
                End If
                Dim objData As AdminIPAccessRow
                If Id <> 0 Then
                    objData = AdminIPAccessRow.GetRow(DB, Id)
                Else
                    objData = New AdminIPAccessRow(DB)
                End If
                objData.Username = UserName()
                objData.IP = txtIP.Text.Trim()
                Dim result As Integer
                If Id <> 0 Then
                    result = AdminIPAccessRow.Update(DB, objData)
                    If result = 1 Then
                        Response.Redirect("default.aspx?username=" & UserName())
                    ElseIf result = -1 Then
                        AddError("IP Address is exists")
                    Else
                        AddError("System error. Please try again!")
                    End If
                Else
                    result = AdminIPAccessRow.Insert(DB, objData)
                    If result = 1 Then
                        Response.Redirect("default.aspx?username=" & UserName())
                    ElseIf result = -1 Then
                        AddError("IP Address is exists")
                    Else
                        AddError("System error. Please try again!")
                    End If
                End If

            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            End Try
        End If
    End Sub
    Public Function IsValidIP(ByVal addr As String) As Boolean
        'create our match pattern
        Dim pattern As String = "^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$"
        'create our Regular Expression object
        Dim check As New Regex(pattern)
        'boolean variable to hold the status
        Dim valid As Boolean = False
        'check to make sure an ip address was provided
        If addr = "" Then
            'no address provided so return false
            valid = False
        Else
            'address provided so use the IsMatch Method
            'of the Regular Expression object
            valid = check.IsMatch(addr, 0)
        End If
        'return the results
        Return valid
    End Function

    Private Sub ViewError(ByVal message As String)
        lblMessage.Text = "<span class='red'>" + message + "</span>"
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?username=" & UserName())
    End Sub
End Class
