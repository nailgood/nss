Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports System.Text
Imports DataLayer
Imports System.Collections.Generic

Partial Class DepartmentTree
    Inherits System.Web.UI.UserControl

    Public Property Link() As String
        Get
            Return ViewState("LINK")
        End Get
        Set(ByVal Value As String)
            ViewState("LINK") = Value
        End Set
    End Property

    Private Function GetCheckedNodes(ByVal input As TreeNode) As String
        Dim sNodes As String = ""
        Dim sConn As String = ""
        For Each node As TreeNode In input.ChildNodes
            If node.Checked Then
                sNodes = sNodes & sConn & node.Value
                sConn = ","
            End If
            Dim Checked As String = GetCheckedNodes(node)
            If Not Checked = String.Empty Then
                sNodes = sNodes & sConn & Checked
                sConn = ","
            End If
        Next
        Return sNodes
    End Function

    Public Property CheckedList() As String
        Get
            If Page.IsPostBack Then
                Return GetCheckedNodes(Tree.Nodes.Item(0))
            Else
                Return ViewState("CHECKED")
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("CHECKED") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DB As New Database
        Dim ParentNode As TreeNode = Nothing
        If IsPostBack Then
            Exit Sub
        End If
        'Open database connection
        DB.Open(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
        Dim lstMainDepartment As List(Of StoreDepartmentRow) = StoreDepartmentRow.GetDepartmentByParentId(0)
        If (Not lstMainDepartment Is Nothing AndAlso lstMainDepartment.Count > 0) Then
            BuiltTreeDepartment(DB, lstMainDepartment, ParentNode)

        End If
        DB.Close()
    End Sub
    Private Sub BuiltTreeDepartment(ByVal DB As Database, ByVal lstchild As List(Of StoreDepartmentRow), ByVal ParentNode As TreeNode)
        Dim ExpandedList As String = StoreDepartmentRow.GetDepartmentsToKeepOpened(DB, CheckedList)
        Dim aExpanded() As String = Split(ExpandedList, ",")
        Dim aChecked() As String = Split(CheckedList, ",")
        If ParentNode Is Nothing Then
            Dim rootNode As TreeNode = New TreeNode
            rootNode.Value = lstchild(0).DepartmentId
            rootNode.Text = lstchild(0).Name
            'rootNode.ImageUrl = "/includes/theme-admin/images/folderminus.gif" 
            'rootNode.ImageUrl = "/includes/theme-admin/images/folderplus.gif"
            rootNode.SelectAction = TreeNodeSelectAction.None
            Tree.Nodes.Add(rootNode)
            ParentNode = rootNode
            Dim index As Integer = lstchild.IndexOf(lstchild(0))
            lstchild.RemoveAt(index)
        End If
        For Each item As StoreDepartmentRow In lstchild
            Dim DepartmentName As String = IIf(IsDBNull(item.AlternateName) OrElse String.IsNullOrEmpty(item.AlternateName), item.Name, item.AlternateName)
            Dim tNode As TreeNode = New TreeNode
            tNode.Value = item.DepartmentId
            tNode.Text = DepartmentName
            tNode.ImageUrl = "/includes/theme-admin/images/folderminus.gif"
            tNode.ImageUrl = "/includes/theme-admin/images/folderplus.gif"
            tNode.SelectAction = TreeNodeSelectAction.None
            'Keep expanded nodes
            If Array.IndexOf(aExpanded, item.DepartmentId.ToString) > -1 Then
                tNode.Expanded = True
            Else
                tNode.Expanded = False
            End If
            If Array.IndexOf(aChecked, item.DepartmentId.ToString) > -1 Then
                tNode.Checked = True
            End If
            If Not item.ParentId = Nothing Then
                If Not item.ParentId = Nothing Then
                    ParentNode.ChildNodes.Add(tNode)
                End If
            End If
            Dim lstChildDepartment As List(Of StoreDepartmentRow) = StoreDepartmentRow.GetDepartmentByParentId(tNode.Value)
            If (Not lstChildDepartment Is Nothing AndAlso lstChildDepartment.Count > 0) Then
                BuiltTreeDepartment(DB, lstChildDepartment, tNode)
            End If
        Next
    End Sub

End Class
