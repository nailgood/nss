Imports System.Web.UI.WebControls
Imports System.Data
Imports Components
Imports DataLayer
Imports System.Collections.Generic

Partial Class controls_layout_shop_by_design
    Inherits ModuleControl

    Private cateId As Integer = 0
    Private parentId As Integer = 0
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Request.QueryString("cateId") <> Nothing AndAlso IsNumeric(Request.QueryString("cateId"))) Then
            cateId = Request.QueryString("cateId")
            parentId = CategoryRow.GetRow(DB, cateId).ParentId
        End If
        If Not IsPostBack Then
            LoadBind()
        End If
    End Sub

    Private Sub LoadBind()
        Dim arrIDParent As New List(Of Integer)
        Dim lstParent As List(Of CategoryRow) = CategoryRow.ListByItem("c.ParentId=0 AND c.Type=" & Utility.Common.CategoryType.ShopDesign)
        If Not lstParent Is Nothing Then
            ltrLink.Text = "<ul id='shop-by-design'>"
            For i As Integer = 0 To lstParent.Count() - 1
                If lstParent(i).CategoryId = cateId Or lstParent(i).CategoryId = parentId Then
                    ltrLink.Text &= "<li class='active'><a href='" & URLParameters.ShopDesignListUrl(lstParent(i).CategoryName, lstParent(i).CategoryId) & "'>" & lstParent(i).CategoryName & "<span class='arrow'></span></a>"
                Else
                    ltrLink.Text &= "<li><a href='" & URLParameters.ShopDesignListUrl(lstParent(i).CategoryName, lstParent(i).CategoryId) & "'>" & lstParent(i).CategoryName & "<span class='arrow'></span></a>"
                End If
                Dim lstChild As List(Of CategoryRow) = CategoryRow.ListByItem("c.ParentId=" & lstParent(i).CategoryId & " AND c.Type=" & Utility.Common.CategoryType.ShopDesign)
                If lstChild.Count > 0 Then
                    BuiltHTML(lstChild, lstParent(i).CategoryId)
                End If
                ltrLink.Text &= "</li>"
                'If Not IsExistCategoryId(arrIDParent, lstParent(i).ParentId) Then
                '    arrIDParent.Add(lstChild(i).ParentId)
                '    Dim nameParent As String = CategoryRow.GetCategoryNameByCategoryId(lstChild(i).ParentId)
                '    If lstChild(i).ParentId = parentId Or lstChild(i).ParentId = cateId Then
                '        ltrLink.Text &= "<li class='active'><a href='" & URLParameters.ShopDesignListUrl(nameParent, lstChild(i).ParentId) & "'>" & nameParent & "<span class='arrow'></span></a>"
                '    Else
                '        ltrLink.Text &= "<li><a href='" & URLParameters.ShopDesignListUrl(nameParent, lstChild(i).ParentId) & "'>" & nameParent & "<span class='arrow'></span></a>"
                '    End If

                '    If Not lstChild Is Nothing Then
                '        BuiltHTML(lstChild, lstChild(i).ParentId)
                '    End If
                '    BuiltHTML(lstChild, lstChild(i).ParentId)
                '    ltrLink.Text &= "</li>"
                'End If
            Next
            ltrLink.Text &= "</ul>"
        End If
    End Sub

    Private Function IsExistCategoryId(ByVal list As List(Of Integer), ByVal ParentId As Integer) As Boolean
        For i As Integer = 0 To list.Count() - 1
            If (list(i) = ParentId) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub BuiltHTML(ByVal lstchild As List(Of CategoryRow), ByVal parentID As Integer)
        ltrLink.Text &= "<ul class='subshop'>"
        For Each child As CategoryRow In lstchild
            If child.CategoryId = cateId Then
                ltrLink.Text &= "<li class='select'><span><b class='glyphicon arrow-right'></b></span><span><a href='" & URLParameters.ShopDesignListUrl(child.CategoryName, child.CategoryId) & "'>" & child.CategoryName & "</a></span></li>"
            Else
                ltrLink.Text &= "<li><span><b class='glyphicon arrow-right'></b></span><span><a href='" & URLParameters.ShopDesignListUrl(child.CategoryName, child.CategoryId) & "'>" & child.CategoryName & "</a></span></li>"
            End If
        Next
        ltrLink.Text &= "</ul>"
    End Sub
End Class
