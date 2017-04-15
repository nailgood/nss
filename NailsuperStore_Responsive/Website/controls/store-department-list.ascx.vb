Imports DataLayer
Imports Components

Partial Class modules_StoreDepartmentList
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Protected m_DepartmentId As Integer = Nothing
    Protected Department As StoreDepartmentRow = Nothing
    Protected ParentDepartment As StoreDepartmentRow = Nothing
    Protected PageParams, PageParamsFull As String
    Protected MaxPerPage As Integer = 24
    Private filter As DepartmentFilterFields

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        PageParamsFull = CType(Page, BasePage).GetPageParams(FilterFieldType.All)
        If IsNumeric(Request("DepartmentId")) Then m_DepartmentId = Request("DepartmentId") Else m_DepartmentId = Nothing
      
        If m_DepartmentId = 0 Then m_DepartmentId = StoreDepartmentRow.GetDefaultDepartment(DB)
        If IsNumeric(Request("ItemId")) AndAlso m_DepartmentId = 23 Then m_DepartmentId = StoreItemRow.GetRow(DB, CInt(Request("ItemId"))).GetDefaultDepartmentId
        If m_DepartmentId = 0 Then m_DepartmentId = StoreDepartmentRow.GetDefaultDepartment(DB)
        ParentDepartment = StoreDepartmentRow.GetMainLevelDepartment(DB, m_DepartmentId)
        Department = StoreDepartmentRow.GetRow(DB, m_DepartmentId)
        BindData()
    End Sub

    Private Sub BindData()
        rptDepartments.DataSource = ParentDepartment.GetChildrenDepartments()
        rptDepartments.DataBind()

        If Not Request.Path.ToLower = "/home.aspx" Then
            filter = New DepartmentFilterFields
            filter.DepartmentId = Department.DepartmentId
            filter.All = Request("F_All") <> String.Empty
            filter.IsOnSale = Request("F_Sale") <> String.Empty
            filter.ToneId = IIf(Not IsNumeric(Request("ToneId")), Nothing, Request("ToneId"))
            filter.CollectionId = IIf(Not IsNumeric(Request("CollectionId")), Nothing, Request("CollectionId"))

            filter.MaxPerPage = MaxPerPage
            filter.pg = 1
            Dim tmp As Boolean = filter.All
            filter.All = False

            filter.All = tmp
        End If

    End Sub

    Private Sub rptDepartments_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptDepartments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lit As Literal = CType(e.Item.FindControl("lit"), Literal)
            Dim rpt As Repeater = CType(e.Item.FindControl("rpt"), Repeater)

            If e.Item.DataItem("lft") <= Department.Lft AndAlso e.Item.DataItem("rgt") >= Department.Rgt Then
                Dim tree As TreeView = CType(e.Item.FindControl("tree"), TreeView)
                Dim ParentNode As TreeNode = Nothing
                Dim tNode As TreeNode
                Dim dv As DataView, drv As DataRowView
                Dim SQL As String

                tNode = New TreeNode
                tNode.Value = e.Item.DataItem("DepartmentId")
                If e.Item.DataItem("DepartmentId") = m_DepartmentId Then
                    If LCase(Request.ServerVariables("URL")) = "/store/default.aspx" Or LCase(Request.ServerVariables("URL")) = "/store/sub-category.aspx" Then
                        tNode.Text = "<div class=""strpt"">" & e.Item.DataItem("Name") & "</div>"
                    Else
                        tNode.Text = "<div class=""strpt""><a href=""/store/default.aspx?DepartmentId=" & e.Item.DataItem("DepartmentId") & "&" & PageParams & """>" & e.Item.DataItem("Name") & "</a></div>" & vbCrLf
                    End If
                Else
                    tNode.Text = "<div class=""strpt""><a href=""/store/default.aspx?DepartmentId=" & e.Item.DataItem("DepartmentId") & "&" & PageParams & """ class=""lgry"">" & e.Item.DataItem("Name") & "</a></div>" & vbCrLf
                End If
                tNode.SelectAction = TreeNodeSelectAction.None
                tree.Nodes.Add(tNode)
                ParentNode = tNode
                SQL = "select * from StoreDepartment where lft > " & DB.Number(e.Item.DataItem("lft")) & " and rgt < " & DB.Number(e.Item.DataItem("rgt")) & " order by lft"
                dv = DB.GetDataSet(SQL).Tables(0).DefaultView
                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)
                    Dim ParentId As Integer = IIf(IsDBNull(drv("ParentId")), Nothing, drv("ParentId"))
                    Dim DepartmentId As Integer = drv("DepartmentId")
                    Dim DepartmentName As String = drv("NAME")

                    tNode = New TreeNode
                    tNode.Value = DepartmentId
                    If DepartmentId = m_DepartmentId AndAlso LCase(Request.ServerVariables("URL")) = "/store/default.aspx" Then
                        tNode.Text = "<div class=""strpt"">" & DepartmentName & "</div>"
                    Else
                        tNode.Text = "<div class=""strpt""><a href=""/store/default.aspx?DepartmentId=" & DepartmentId & """>" & DepartmentName & "</a></div>" & vbCrLf
                    End If
                    tNode.SelectAction = TreeNodeSelectAction.None

                    If ParentId = Nothing Then
                        tree.Nodes.Add(tNode)
                        tNode.Expanded = True
                        ParentNode = tNode
                    Else
                        While ParentNode.Value <> ParentId
                            ParentNode = ParentNode.Parent
                        End While
                        ParentNode.ChildNodes.Add(tNode)
                        ParentNode = tNode
                    End If
                Next
            Else
                lit.Text = "<li><a href='/nail-supplies/" & e.Item.DataItem("URLCode") & "/" & e.Item.DataItem("DepartmentId") & "' >" & e.Item.DataItem("Name") & "</a></li>" & vbCrLf
            End If
        End If
    End Sub

    

End Class
