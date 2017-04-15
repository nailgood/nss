Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Components

Namespace Controls

    Public Class GridView
        Inherits WebControls.GridView

        Public Delegate Sub BindListHandler()
        Public BindList As BindListHandler

        Public Property PageSelectIndex() As Integer
            Get
                If Not ViewState("PageSelectIndex") Is Nothing Then
                    Return CInt(ViewState("PageSelectIndex"))
                Else
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageSelectIndex") = Value
            End Set
        End Property
     
        Public Property CausesValidation() As Boolean
            Get
                If ViewState("CausesValidation") Is Nothing Then ViewState("CausesValidation") = True
                Return ViewState("CausesValidation")
            End Get
            Set(ByVal value As Boolean)
                ViewState("CausesValidation") = value
            End Set
        End Property

        Public Property HeaderText() As String
            Get
                Return IIf(ViewState("HeaderText") Is Nothing, String.Empty, ViewState("HeaderText"))
            End Get
            Set(ByVal value As String)
                ViewState("HeaderText") = value
            End Set
        End Property

        Public Property Pager() As CustomPager
            Get
                If ViewState("Pager") Is Nothing Then ViewState("Pager") = New CustomPager
                Return ViewState("Pager")
            End Get
            Set(ByVal value As CustomPager)
                ViewState("Pager") = value
            End Set
        End Property

        Public Property SortBy() As String
            Get
                Return ViewState("SortBy")
            End Get
            Set(ByVal value As String)
                ViewState("SortBy") = value
            End Set
        End Property

        Public Property SortOrder() As String
            Get
                If ViewState("SortOrder") Is Nothing OrElse ViewState("SortOrder") = String.Empty Then ViewState("SortOrder") = "ASC"
                Return ViewState("SortOrder")
            End Get
            Set(ByVal value As String)
                ViewState("SortOrder") = value
            End Set
        End Property

        Public ReadOnly Property SortByAndOrder() As String
            Get
                If SortBy = String.Empty Then Return String.Empty
                Return SortBy & " " & SortOrder
            End Get
        End Property

        Public Property SortImageAsc() As String
            Get
                If ViewState("SortImageAsc") Is Nothing Then ViewState("SortImageAsc") = "/includes/theme-admin/images/asc3.gif"
                Return ViewState("SortImageAsc")
            End Get
            Set(ByVal value As String)
                ViewState("SortImageAsc") = value
            End Set
        End Property

        Public Property SortImageDesc() As String
            Get
                If ViewState("SortImageDesc") Is Nothing Then ViewState("SortImageDesc") = "/includes/theme-admin/images/desc3.gif"
                Return ViewState("SortImageDesc")
            End Get
            Set(ByVal value As String)
                ViewState("SortImageDesc") = value
            End Set
        End Property

        Protected Overrides Sub OnRowCreated(ByVal e As GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.Header Then
                'Specify nowrap style for each header column
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("white-space", "nowrap")
                Next

        		'Display arrows	
        		If SortBy <> String.Empty Then
                    DisplaySortOrderImages(e.Row)
                End If

            ElseIf e.Row.RowType = DataControlRowType.Pager Then

        		'Create navigator control and bind hookup PagingEvent
                Dim nav As Navigator = New Navigator
                nav.CssClass = Pager.cssClass
                nav.NofRecords = Pager.NofRecords
                nav.MaxPerPage = PageSize
                If (PageIndex = 0) Then
                    PageIndex = PageSelectIndex
                End If
                nav.PageNumber = PageIndex + 1
                AddHandler nav.NavigatorEvent, AddressOf myNavigator_PagingEvent
                e.Row.Cells(0).Controls.Add(nav)
                nav.DataBind()
            End If

            MyBase.OnRowCreated(e)
        End Sub

        Protected Sub DisplaySortOrderImages(ByVal dgItem As GridViewRow)
            For i As Integer = 0 To dgItem.Cells.Count - 1
                If (dgItem.Cells(i).Controls.Count > 0) AndAlso (TypeOf dgItem.Cells(i).Controls(0) Is LinkButton) Then
                    Dim Link As LinkButton = CType(dgItem.Cells(i).Controls(0), LinkButton)
                    If SortBy = Link.CommandArgument Then
                        Dim imgSortDirection As Image = New Image()
                        If SortOrder = "DESC" Then
                            imgSortDirection.ImageUrl = SortImageDesc
                        Else
                            imgSortDirection.ImageUrl = SortImageAsc
                        End If
                        imgSortDirection.Attributes("align") = "absmiddle"
                        dgItem.Cells(i).Controls.Add(imgSortDirection)
                    End If
                End If
            Next
        End Sub

        Private Function PageValidate() As Boolean
            If CausesValidation Then
                Page.Validate()
                If Not Page.IsValid Then
                    Return False
                End If
            End If
            Return True
        End Function

        Protected Overrides Sub OnSorting(ByVal e As GridViewSortEventArgs)
            If Not PageValidate() Then Exit Sub

            If SortOrder = "ASC" And SortBy = e.SortExpression Then
                SortOrder = "DESC"
            Else
                SortOrder = "ASC"
            End If
            SortBy = Core.ProtectParam(e.SortExpression)
            MyBase.OnSorting(e)
        End Sub

        Private Sub GridView_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            'Generate blank template, so the standar Pager is not created - we are using custom pager
            PagerTemplate = New BlankTemplate

            HeaderStyle.VerticalAlign = VerticalAlign.Top
            PageSelectIndex = 0
        End Sub

        Private Sub GridView_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Make sure that the pager is displayed when only one page is returned as well
            If PagerSettings.Position = PagerPosition.Top Or PagerSettings.Position = PagerPosition.TopAndBottom Then
                On Error Resume Next
                If PageCount > 0 Then TopPagerRow.Visible = True
            End If
            If PagerSettings.Position = PagerPosition.Bottom Or PagerSettings.Position = PagerPosition.TopAndBottom Then
                On Error Resume Next
                If PageCount > 0 Then BottomPagerRow.Visible = True
            End If

        End Sub

        Protected Sub gvList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Me.Sorting
            'if BindList is hooked up, then reset page index and refresh list
            If Not BindList Is Nothing Then
                PageIndex = 0
                BindList()
            End If
        End Sub

        Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As NavigatorEventArgs)
            If Not PageValidate() Then Exit Sub

            'if BindList is hooked up, then change page index and refresh list
            If Not BindList Is Nothing Then
                PageIndex = e.PageNumber - 1
                BindList()
            End If
        End Sub

        Protected Overrides Sub RenderChildren(ByVal writer As System.Web.UI.HtmlTextWriter)
            If PageCount > 0 Then writer.Write("<span class=""smaller"">" & HeaderText & "</span>")
            MyBase.RenderChildren(writer)
        End Sub

    End Class

    Public Class BlankTemplate
        Implements ITemplate

        Public Sub New()
        End Sub

        Private Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
        End Sub
    End Class

    <Serializable()> _
    Public Class CustomPager
        Public NofRecords As Integer
        Public cssClass As String
    End Class

End Namespace
