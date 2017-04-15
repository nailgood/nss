Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Collections.Generic

Public Class admin_store_departments__default
    Inherits AdminPage

    Dim DepartmentId As Integer
    Dim SourceId As Integer
    Dim TargetId As Integer

    Public Shared fplus As String = "/includes/theme-admin/images/folderplus2.gif"
    Public Shared fminus As String = "includes/theme-admin/images/folderminus2.gif"
    Public Shared tplus As String = "/includes/theme-admin/images/tplus2.gif"
    Public Shared lplus As String = "/includes/theme-admin/images/lplus2.gif"
    Public Shared tminus As String = "/includes/theme-admin/images/tminus2.gif"
    Public Shared lminus As String = "/includes/theme-admin/images/lminus2.gif"
    Public Shared t2 As String = "/includes/theme-admin/images/t2.gif"
    Public Shared i2 As String = "/includes/theme-admin/images/i2.gif"
    Public Shared l2 As String = "/includes/theme-admin/images/l2.gif"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Dim sDepartments As String = ""
        Dim sSourceDepartments As String = ""
        Dim sTargetDepartments As String = ""

        F.EnableViewState = False
        'T.EnableViewState = False
        'S.EnableViewState = False

        If Not Page.IsPostBack Then
            DepartmentId = CType(Request.QueryString("DepartmentId"), Integer)
            'SourceId = CType(Request.QueryString("SourceId"), Integer)
            'TargetId = CType(Request.QueryString("TargetId"), Integer)
        Else
            DepartmentId = CType(Request.Form("DepartmentId"), Integer)
            'SourceId = CType(Request.Form("SourceId"), Integer)
            'TargetId = CType(Request.Form("TargetId"), Integer)
        End If

        'Define javascript
        btnAdd.Attributes("onClick") = "return Add();"
        btnRename.Attributes("onClick") = "return Rename();"
        btnDelete.Attributes("onClick") = "return Delete();"
        'btnMove.Attributes("onClick") = "return Move();"

        ' GET ALL PARENTS FOR DEPARTMENT
        If DepartmentId <> 0 Then sDepartments = StoreDepartmentRow.GetDepartmentsToKeepOpened(DB, DepartmentId)

        ' GET ALL PARENTS FOR SOURCE DEPARTMENT
        'If SourceId <> 0 Then sSourceDepartments = StoreDepartmentRow.GetDepartmentsToKeepOpened(DB, SourceId)

        ' GET ALL PARENTS FOR TARGET DEPARTMENT
        'If TargetId <> 0 Then sTargetDepartments = StoreDepartmentRow.GetDepartmentsToKeepOpened(DB, TargetId)

        'F.Text = StoreDepartmentRow.GetDepartments(DB, "F", "/admin/store/departments/edit.aspx", sDepartments, "DepartmentId", DepartmentId, False, True)
        'S.Text = StoreDepartmentRow.GetDepartments(DB, "S", "", sSourceDepartments, "SourceId", SourceId, True, True)
        'T.Text = StoreDepartmentRow.GetDepartments(DB, "T", "", sTargetDepartments, "TargetId", TargetId, False, False)
        F.Text = GetDepartments(23, "DepartmentId", "F", "/admin/store/departments/edit.aspx")
    End Sub

    Private Shared Function GetDepartments(ByVal DepartmentId As Integer, ByVal sFieldName As String, ByVal sPrefix As String, ByVal sLink As String) As String
        Dim sChecked = ""
        Dim result As String = "<table border='0' cellpadding='0' cellspacing='0'><tr>" _
                               & "<td><img id='" & sPrefix & "FLD" & DepartmentId & "' src='" & fplus & "' width='16' height='22' /></td>" _
                               & "<td><input type=radio name=""" & sFieldName & """ value=""" & DepartmentId & """ " & sChecked & " /></td>" _
                                & "<td nowrap><a href=""" & sLink & "?DepartmentId=" & DepartmentId & """>Store Home</a></td></tr></table>"

        Dim lstMainDepartment As List(Of StoreDepartmentRow) = StoreDepartmentRow.GetDepartmentByParentId(DepartmentId)
        If Not lstMainDepartment Is Nothing Then
            If lstMainDepartment.Count > 0 Then
                result &= BuiltHTMLDepartment(0, lstMainDepartment, DepartmentId, False, sPrefix, sLink, sFieldName)
            End If
        End If
        Return result
    End Function

    Private Shared Function BuiltHTMLDepartment(ByVal level As Integer, ByVal lstParent As List(Of StoreDepartmentRow), ByVal DepartmentId As Integer, ByVal expand As Boolean, ByVal sPrefix As String, ByVal sLink As String, ByVal sFieldName As String) As String
        Dim html As String = ""
        Dim sChecked = ""
        For i As Integer = 0 To lstParent.Count - 1
            html &= "<table border='0' cellpadding='0' cellspacing='0'><tr>"
            If DepartmentId = lstParent(i).DepartmentId Then sChecked = "checked"
            Dim lstChildDepartment As List(Of StoreDepartmentRow) = StoreDepartmentRow.GetDepartmentByParentId(lstParent(i).DepartmentId)
            If level > 0 Then
                For k As Integer = 1 To level
                    html &= "<td><img src='" & i2 & "' width='19' height='22' /></td>"
                Next
            End If
            If expand Then
                html &= "<td style=""cursor:hand"" onClick=""expandit('" & sPrefix & "', this, '" & lstParent(i).DepartmentId & "')""><img id='" & sPrefix & "IMG" & lstParent(i).DepartmentId & "' src='" & tminus & "' width='19' height='22' /></td>"
                html &= "<td><img id='" & sPrefix & "FLD" & lstParent(i).DepartmentId & "' src='" & fminus & "' width='16' height='22' /></td>"
            Else
                html &= "<td style=""cursor:hand"" onClick=""expandit('" & sPrefix & "', this, '" & lstParent(i).DepartmentId & "')"">"
                If lstChildDepartment.Count > 0 Then
                    html &= "<img id='" & sPrefix & "IMG" & lstParent(i).DepartmentId & "' src='" & tplus & "' width='19' height='22' /></td>"
                ElseIf i < lstParent.Count - 1 Then
                    html &= "<img id='" & sPrefix & "IMG" & lstParent(i).DepartmentId & "' src='" & t2 & "' width='19' height='22' /></td>"
                Else
                    html &= "<img id='" & sPrefix & "IMG" & lstParent(i).DepartmentId & "' src='" & l2 & "' width='19' height='22' /></td>"
                End If
                html &= "<td><img id='" & sPrefix & "FLD" & lstParent(i).DepartmentId & "' src='" & fplus & "' width='16' height='22' /></td>"
            End If
            html &= "<td width='4'><img src='/includes/theme-admin/images/spacer.gif' width='4' height='16' /></td>" & BuiltHTMLArrange(lstParent(i), lstParent.Count, i)
            html &= "<td><img src='/includes/theme-admin/images/spacer.gif' width='4' height='16' /></td>"
            html &= "<td><input type='radio' name='" & sFieldName & "' value='" & lstParent(i).DepartmentId & "' " & sChecked & " /></td>"
            html &= IIf(sPrefix = "F", "<td nowrap><a href='" & sLink & "?DepartmentId=" & lstParent(i).DepartmentId & "'>" & lstParent(i).Name & "</a>", "<td nowrap>" & lstParent(i).Name)
            html &= "<sup>(<a href='/admin/store/departments/ArrangeItems.aspx?F_DepartmentId=" & lstParent(i).DepartmentId & "'>" & lstParent(i).CountItem & "</a>)</sup></td></tr></table>"
            If Not lstChildDepartment Is Nothing AndAlso lstChildDepartment.Count > 0 Then
                Dim sublevel As Integer = level + 1
                html &= "<span id='" & sPrefix & "SPAN" & lstParent(i).DepartmentId & "' style='display:none'>"
                html &= BuiltHTMLDepartment(sublevel, lstChildDepartment, lstParent(i).DepartmentId, False, sPrefix, sLink, sFieldName)
                html &= "</span>"
            End If
        Next
        Return html
    End Function

    Private Shared Function BuiltHTMLArrange(ByVal item As StoreDepartmentRow, ByVal countList As Integer, ByVal index As Integer) As String
        Dim html As String = ""
        If index = 0 AndAlso countList > 1 Then
            html &= "<td width='16'></td>"
            html &= "<td align='right'><a href='move.aspx?ACTION=DOWN&DepartmentId=" & item.DepartmentId & "&ParentId=" & item.ParentId & "'><img src='/includes/theme-admin/images/movedown.gif' width='16' height='16' border='0' alt='Move Down'></a></td>"
        ElseIf index = countList - 1 AndAlso countList > 1 Then
            html &= "<td align='right'><a href='move.aspx?ACTION=UP&DepartmentId=" & item.DepartmentId & "&ParentId=" & item.ParentId & "'><img src='/includes/theme-admin/images/moveup.gif' width='16' height='16' border='0' alt='Move Up'></a></td>"
            html &= "<td width='16'></td>"
        ElseIf countList > 0 Then
            html &= "<td align='right'><a href='move.aspx?ACTION=UP&DepartmentId=" & item.DepartmentId & "&ParentId=" & item.ParentId & "'><img src='/includes/theme-admin/images/moveup.gif' width='16' height='16' border='0' alt='Move Up'></a></td>"
            html &= "<td align='right'><img src='/includes/theme-admin/images/spacer.gif' width='1' height='1' /><a href='move.aspx?ACTION=DOWN&DepartmentId=" & item.DepartmentId & "&ParentId=" & item.ParentId & "'><img src='/includes/theme-admin/images/movedown.gif' width='16' height='16' border='0' alt='Move Down'></a></td>"
        End If
        Return html
    End Function

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If DepartmentId = 0 Then
                Throw New ApplicationException("Parent Department has not been selected")
            End If
            'If NewDepartment.Text = String.Empty Then
            '    Throw New ApplicationException("Department Name cannot be empty")
            'End If

            'DB.BeginTransaction()
            'DepartmentId = StoreDepartmentRow.DepartmentInsert(DB, DepartmentId, NewDepartment.Text)

            ''Invalidate cached menu

            'DB.CommitTransaction()

            Response.Redirect("edit.aspx?ParentId=" & DepartmentId)

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub btnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRename.Click
        Try
            If DepartmentId = 0 Then
                Throw New ApplicationException("Department to rename has not been selected")
            End If
            If NAME.Text = String.Empty Then
                Throw New ApplicationException("New Name for the department cannot be empty")
            End If
            Dim dbStoreDepartment As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, DepartmentId)
            If (Not dbStoreDepartment Is Nothing AndAlso Not String.IsNullOrEmpty(dbStoreDepartment.Name)) Then
                DB.BeginTransaction()
                SQL = "exec sp_StoreDepartmentRename " & DB.Quote(DepartmentId) & "," & DB.Quote(NAME.Text)
                DB.ExecuteSQL(SQL)

                'Invalidate cached menu
                Context.Cache.Remove("HeaderMenuCache")
                DB.CommitTransaction()

                Dim logDetail As New AdminLogDetailRow
                logDetail.Message = "Name|" & dbStoreDepartment.Name & "|" & NAME.Text & "[br]"
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logDetail.ObjectType = Utility.Common.ObjectType.Department.ToString()
                logDetail.ObjectId = DepartmentId
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            End If

            Response.Redirect("default.aspx?DepartmentId=" & DepartmentId)

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
        End Try

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If DepartmentId = 0 Then
                Throw New Exception("Department to delete has not been selected")
            End If

            Dim dbDepartment As DataLayer.StoreDepartmentRow = DataLayer.StoreDepartmentRow.GetRow(DB, DepartmentId)
            If dbDepartment.IsInactive Then
                Throw New ApplicationException("Inactive root department cannot be deleted")
            End If

            ' GET PARENT_ID
            Dim iParent As Integer = dbDepartment.ParentId
            Dim iLft As Integer = dbDepartment.Lft

            If iLft = 1 Then
                Throw New ApplicationException("Main department cannot be deleted")
            End If

            DB.BeginTransaction()
            SQL = "exec sp_StoreDepartmentDelete " & DB.Quote(DepartmentId)
            DB.ExecuteSQL(SQL)
            DB.CommitTransaction()

            Dim logDetail As New AdminLogDetailRow
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(dbDepartment, Utility.Common.ObjectType.Department)
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectType = Utility.Common.ObjectType.Department.ToString()
            logDetail.ObjectId = DepartmentId
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("default.aspx?DepartmentId=" & iParent)
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
        End Try
    End Sub

    'Private Sub btnMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMove.Click
    '    Try
    '        If SourceId = 0 Then
    '            Throw New Exception("Please select the Department to Move")
    '        End If
    '        If TargetId = 0 Then
    '            Throw New Exception("Please select the Destination Department")
    '        End If

    '        TaskDB.BeginTransaction()

    '        SQL = "exec sp_StoreDepartmentMove " & DB.Quote(SourceId) & "," & DB.Quote(TargetId)
    '        TaskDB.ExecuteSQL(SQL)

    '        'Invalidate cached menu
    '        Context.Cache.Remove("HeaderMenuCache")

    '        TaskDB.CommitTransaction()

    '        Response.Redirect("default.aspx?SourceId=" & SourceId)

    '    Catch ex As SqlException
    '        AddError(ErrHandler.ErrorText(ex))
    '    Catch ex As ApplicationException
    '        AddError(ex.Message)
    '    Finally
    '    End Try
    'End Sub
End Class