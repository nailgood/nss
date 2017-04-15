
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Public Class admin_shopdesign_Category_default
    Inherits AdminPage

    Public Total As Integer = 0
    Public html As String = ""
    Dim logdetail As New AdminLogDetailRow
    Dim pathImg As String = "/includes/theme-admin/images/"
    Dim parentId As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CategoryId As Integer = CInt(Request.QueryString("CategoryId"))
        Dim ACTION As String = Request.QueryString("ACTION")
        parentId = CInt(Request.QueryString("ParentId"))
        If ACTION = "active" Then
            ChangeIsActive(CategoryId)
        ElseIf ACTION = "delete" Then
            Delete(CategoryId)
        ElseIf ACTION = "up" Or ACTION = "down" Then
            ChangeArrange(CategoryId, parentId, ACTION)
        End If
        If Not IsPostBack Then
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim condition As String
        condition = "ParentId = 0 AND Type = " & Utility.Common.CategoryType.ShopDesign
        'If Not F_Name.Text = String.Empty Then
        '    data.Condition &= " AND CategoryName like " & DB.FilterQuote(F_Name.Text)
        'End If
        'If Not F_IsActive.Text = String.Empty Then
        '    data.Condition &= " AND IsActive = " & DB.Number(F_IsActive.SelectedValue)
        'End If
        Dim result As CategoryCollection = CategoryRow.ListAllParent(condition)
        'Total = result.Count
        'gvList.DataSource = result
        'gvList.DataBind()
        'html = "<table border='0' cellspacing='0' cellpadding='0'>"
        BuiltHTML(result, False)
        'html &= "</table>"
        lrtHTML.Text = html
    End Sub

    Private Sub BuiltHTML(ByVal lstParent As CategoryCollection, ByVal isChild As Boolean)
        Dim td As String = ""
        For i As Integer = 0 To lstParent.Count - 1
            Dim condition As String = "Type = " & Utility.Common.CategoryType.ShopDesign & " AND ParentId = " & lstParent(i).CategoryId
            Dim lstchild As CategoryCollection = CategoryRow.ListAllParent(condition)
            html &= "<table border='0' cellspacing='0' cellpadding='0'><tr>"
            If lstchild.Count > 0 AndAlso parentId > 0 AndAlso parentId = lstParent(i).CategoryId Then '
                td = "<td onclick='expandit(""F"", this, """ & lstParent(i).CategoryId.ToString() & """)' style='cursor:pointer;'><img width='19' height='22' src='" & pathImg & "tminus2.gif' id='FIMG" & lstParent(i).CategoryId.ToString() & "' /></td>"
            ElseIf lstchild.Count > 0 Then
                td = "<td onclick='expandit(""F"", this, """ & lstParent(i).CategoryId.ToString() & """)' style='cursor:pointer;'><img width='19' height='22' src='" & pathImg & "tplus2.gif' id='FIMG" & lstParent(i).CategoryId.ToString() & "' /></td>"
            Else
                td = "<td><img width='19' height='22' src='" & pathImg & "t2.gif' /></td>"
            End If
            If isChild Then
                td = "<td><img width='19' height='22' src='" & pathImg & "i2.gif'></td>" & td
            End If

            td &= "<td><img width='16' height='22' src='" & pathImg & "folderplus2.gif' /></td>"
            'icon active
            If lstParent(i).IsActive Then
                td &= "<td width='25' align='center'><a href='default.aspx?ACTION=active&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='16' height='16' src='" & pathImg & "active.png' /></a></td>"
            Else
                td &= "<td width='25' align='center'><a href='default.aspx?ACTION=active&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='12' height='12' src='" & pathImg & "inactive.png' /></a></td>"
            End If
            'icon delete
            td &= "<td width='25' align='center'><a href='default.aspx?ACTION=delete&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='16' height='16' src='" & pathImg & "delete.gif' /></a></td>"
            'icon arrange
            If i = 0 AndAlso lstParent.Count > 1 Then
                td &= "<td width='38' align='center'><a href='default.aspx?ACTION=down&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='16' height='16' src='" & pathImg & "movedown.gif' /></a></td>"
            ElseIf i = lstParent.Count - 1 AndAlso lstParent.Count > 1 Then
                td &= "<td width='38' align='center'><a href='default.aspx?ACTION=up&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='16' height='16' src='" & pathImg & "moveup.gif' /></a></td>"
            ElseIf lstParent.Count > 1 Then
                td &= "<td width='38' align='center'><a href='default.aspx?ACTION=up&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='16' height='16' src='" & pathImg & "moveup.gif' /></a>"
                td &= "<img width='1' height='1' src='" & pathImg & "spacer.gif' /><a href='default.aspx?ACTION=down&CategoryId=" & lstParent(i).CategoryId.ToString() & "&ParentId=" & lstParent(i).ParentId.ToString() & "'><img width='16' height='16' src='" & pathImg & "movedown.gif' /></a></td>"
            End If
            'name
            td &= "<td><a href='edit.aspx?id=" & lstParent(i).CategoryId & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & lstParent(i).CategoryName & "</a> "
            td &= "<sup>(<a href='/admin/shopdesign/default.aspx?F_CatId=" & lstParent(i).CategoryId & "'>" & CategoryRow.CountItemShopDesign(DB, lstParent(i).CategoryId) & "</a>)</sup></td>"
            html &= td & "</tr></table>"
            If lstchild.Count > 0 Then
                'Check open child
                If parentId > 0 AndAlso parentId = lstParent(i).CategoryId Then
                    html &= "<span id='FSPAN" & lstParent(i).CategoryId & "' style='display:block'>"
                Else
                    html &= "<span id='FSPAN" & lstParent(i).CategoryId & "' style='display:none'>"
                End If
                BuiltHTML(lstchild, True)
                html &= "</span>"
            End If

        Next
        'html &= tr
    End Sub

    Private Sub ChangeIsActive(ByVal CategoryId As Integer)
        CategoryRow.ChangeIsActive(DB, CategoryId)
        Dim category As CategoryRow = CategoryRow.GetRow(DB, CategoryId)
        logdetail.Action = Utility.Common.AdminLogAction.Update.ToString()
        logdetail.Message = "IsActive|" & category.IsActive.ToString() & "|" & (Not category.IsActive).ToString() & "[br]"
        logdetail.ObjectId = category.CategoryId
        logdetail.ObjectType = Utility.Common.ObjectType.Category.ToString()
        AdminLogHelper.WriteLuceneLogDetail(logdetail)
    End Sub

    Private Sub Delete(ByVal CategoryId As Integer)
        CategoryRow.Delete(DB, CategoryId)
        Dim category As CategoryRow = CategoryRow.GetRow(DB, CategoryId)
        logdetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
        logdetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(category, Utility.Common.ObjectType.Category)
        logdetail.ObjectId = category.CategoryId
        logdetail.ObjectType = Utility.Common.ObjectType.Category.ToString()
        AdminLogHelper.WriteLuceneLogDetail(logdetail)
    End Sub

    Private Sub ChangeArrange(ByVal CategoryId As Integer, ByVal parentId As Integer, ByVal action As String)
        If action = "up" Then
            CategoryRow.ChangeArrange(DB, CategoryId, parentId, True)
        Else
            CategoryRow.ChangeArrange(DB, CategoryId, parentId, False)
        End If
    End Sub

    Protected Sub AddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx")
    End Sub

End Class
