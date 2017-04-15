


Imports System.ComponentModel

Partial Class controls_layout_pager
    Inherits System.Web.UI.UserControl

#Region "Property"
    Private _defaultShowSummary As Integer = 5
    Public SumaryIndex As String = "Page {0} of {1}"
    Private _viewAll As Boolean = False
    Private _showPageSize As Boolean = True
    Private _TotalRecord As Integer
    Private _countPageIndex As Integer = 5
    Private indexPrevPostBack As Integer = 7
    Private indexNextPostBack As Integer = 9
    Private indexViewAllPostBack As Integer = 1
    Private indexPageSizePostBack As Integer = 4
    Private _defaultPageSizeValid As Boolean = False
    Public show2LineViewPagingTextMode As Boolean = False
    Public queryViewPaging As String = "vpg" ''1: pager dang hien thi text: view paging, else: hien thi pager binh thuong
    Public queryEnableViewPaging As String = "evpg" ''1: cho phep pager hien thi che do  view paging hay khong

    Public Property QueryPageIndex() As String
        Get
            Dim o As Object = ViewState("QueryPageIndex")
            If o IsNot Nothing Then
                Return o.ToString()
            End If
            Return "pi"
        End Get
        Set(ByVal value As String)
            ViewState("QueryPageIndex") = value
        End Set
    End Property
    Public Property QueryPageSize() As String
        Get
            Dim o As Object = ViewState("QueryPageSize")
            If o IsNot Nothing Then
                Return o.ToString()
            End If
            Return "ps"

        End Get
        Set(ByVal value As String)
            ViewState("QueryPageSize") = value
        End Set
    End Property
    Public Property QueryPageSizeDefault() As String
        Get
            Dim o As Object = ViewState("QueryPageSizeDefault")
            If o IsNot Nothing Then
                Return o.ToString()
            End If
            Return "pdf"

        End Get
        Set(ByVal value As String)
            ViewState("QueryPageSizeDefault") = value
        End Set
    End Property
    <ComponentModel.Browsable(False)> _
    Public Property PageIndex() As Integer
        Get
            Dim o As Object = ViewState("PageIndex")
            If o IsNot Nothing Then
                Return CInt(o)
            End If
            Return 1
        End Get
        Set(ByVal value As Integer)
            ViewState("PageIndex") = value
        End Set
    End Property
    ''1: post back, else: query string
    <ComponentModel.Browsable(False)> _
    Public Property ViewMode() As Integer
        Get
            Dim o As Object = ViewState("ViewMode")
            If o IsNot Nothing Then
                Return CInt(o)
            End If
            Return 1
        End Get
        Set(ByVal value As Integer)
            ViewState("ViewMode") = value
        End Set
    End Property
    <ComponentModel.Browsable(False)> _
    Public Property PageSize() As Integer
        Get
            Try
                If ViewAll Then
                    Return Int32.MaxValue
                End If
                If (ViewMode = 1) Then
                    If Not ShowTwoLine Then
                        Return Convert.ToInt32(drlPageSizePostBack.SelectedValue.ToString())
                    Else
                        Return Convert.ToInt32(drlPageSizePostBack2Line.SelectedValue.ToString())
                    End If
                Else
                    If Not ShowTwoLine Then
                        Return Convert.ToInt32(drlPageSizeQuery.SelectedValue.ToString())
                    Else
                        Return Convert.ToInt32(drlPageSizeQueryShow2Line.SelectedValue.ToString())
                    End If

                End If

            Catch
            End Try
            Return 10
        End Get
        Set(ByVal value As Integer)
            Try

                If ViewMode = 1 Then
                    If DefaultPageSize < 1 Then
                        DefaultPageSize = value
                    End If
                    If Not ViewAll Then
                        If Not ShowTwoLine Then
                            drlPageSizePostBack.SelectedValue = value.ToString()
                        Else
                            drlPageSizePostBack2Line.SelectedValue = value.ToString()
                        End If
                    End If
                Else
                    If Not ShowTwoLine Then
                        If CheckPageSizeInList(value, drlPageSizeQuery) Then

                            drlPageSizeQuery.SelectedValue = value.ToString()
                        Else
                            ViewAll = True
                        End If
                    Else
                        If CheckPageSizeInList(value, drlPageSizeQueryShow2Line) Then

                            drlPageSizeQueryShow2Line.SelectedValue = value.ToString()
                        Else
                            ViewAll = True
                        End If
                    End If

                End If

            Catch
            End Try
        End Set
    End Property
    <Browsable(False)> _
    Public Property TotalPage() As Integer
        Get
            Dim o As Object = ViewState("TotalPage")
            If o IsNot Nothing Then
                Return CInt(o)
            End If
            Return 1
        End Get
        Set(ByVal value As Integer)
            ViewState("TotalPage") = value
        End Set
    End Property
    <Browsable(False)> _
    Public Property DefaultPageSize() As Integer
        Get
            Dim o As Object = ViewState("DefaultPageSize")
            If o IsNot Nothing Then
                Return CInt(o)
            End If
            Return 0
        End Get
        Set(ByVal value As Integer)
            ViewState("DefaultPageSize") = value
        End Set
    End Property


    <Browsable(False)> _
    Public Property DefaultPageSizeQuery() As Integer
        Get
            Dim o As Object = ViewState("DefaultPageSizeQuery")
            If o IsNot Nothing Then
                Return CInt(o)
            End If
            Return 0
        End Get
        Set(ByVal value As Integer)
            ViewState("DefaultPageSizeQuery") = value
        End Set
    End Property

    Public Property EnableViewPagingTextQuery() As Boolean
        Get
            Dim o As Object = ViewState("EnableViewPagingTextQuery")
            If o IsNot Nothing Then
                Return CBool(o)
            End If
            Return False
        End Get
        Set(ByVal value As Boolean)
            ViewState("EnableViewPagingTextQuery") = value
        End Set
    End Property


    <Browsable(False)> _
    Public Property EnableViewPagingTextPostBack() As Boolean
        Get
            Dim o As Object = ViewState("EnableViewPagingTextPostBack")
            If o IsNot Nothing Then
                Return CBool(o)
            End If
            Return False
        End Get
        Set(ByVal value As Boolean)
            ViewState("EnableViewPagingTextPostBack") = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("Total record of result")> _
    Public Property TotalRecord() As Integer
        Get
            Return _TotalRecord
        End Get
        Set(ByVal value As Integer)
            _TotalRecord = value
        End Set
    End Property
    <Browsable(True)> _
    <Description("Url which use for paging")> _
    Public Property PagingUrl() As String
        Get
            Dim o As Object = ViewState("PagingUrl")
            Try
                Return o.ToString()
            Catch ex As Exception

            End Try
            Return String.Empty
        End Get
        Set(ByVal value As String)
            ViewState("PagingUrl") = GetPagingRawURL(value)
        End Set
    End Property
    <Browsable(False)> _
    <Description("current view in ViewAll mode?")> _
    Public Property ViewAll() As [Boolean]
        Get
            Dim o As Object = ViewState("ViewAll")
            Try
                Return Convert.ToBoolean(o)
            Catch ex As Exception

            End Try
            Return False
        End Get
        Set(ByVal value As [Boolean])
            If Not ShowViewAll Then
                ViewState("ViewAll") = False
            Else
                ViewState("ViewAll") = value
            End If
        End Set
    End Property
    <Browsable(False)> _
    Public Property DefaultPageSizeValid() As [Boolean]
        Get
            Return _defaultPageSizeValid
        End Get
        Set(ByVal value As [Boolean])
            _defaultPageSizeValid = value
        End Set
    End Property
    <Browsable(True)> _
   <Description("Show Pager in 2 line")> _
    Public Property ShowTwoLine() As [Boolean]
        Get
            Dim o As Object = ViewState("ShowTwoLine")
            Try
                Return Convert.ToBoolean(o)
            Catch ex As Exception

            End Try
            Return False
        End Get
        Set(ByVal value As [Boolean])
            ViewState("ShowTwoLine") = value
        End Set
    End Property
    <Browsable(True)> _
    <Description("Show or Hidden control ViewAll")> _
    Public Property ShowViewAll() As [Boolean]
        Get
            Dim o As Object = ViewState("ShowViewAll")
            Try
                Return Convert.ToBoolean(o)
            Catch ex As Exception

            End Try
            Return False
        End Get
        Set(ByVal value As [Boolean])
            ViewState("ShowViewAll") = value
        End Set
    End Property
    <Browsable(True)> _
    <Description("Show or Hidden control PageSize")> _
    Public Property ShowPageSize() As [Boolean]
        Get
            Dim o As Object = ViewState("ShowPageSize")
            Try
                Return Convert.ToBoolean(o)
            Catch ex As Exception

            End Try
            Return False
        End Get
        Set(ByVal value As [Boolean])
            ViewState("ShowPageSize") = value
        End Set
    End Property
#End Region

#Region "Delegate"
    Public Delegate Sub ChangePageSize(ByVal obj As Object, ByVal e As PageSizeChangeEventArgs)
    Public Event PageSizeChanging As ChangePageSize
    Public Delegate Sub ChangePageIndex(ByVal obj As Object, ByVal e As PageIndexChangeEventArgs)
    Public Event PageIndexChanging As ChangePageIndex
#End Region

#Region "Init,load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        ''InitDefaultPager()
    End Sub
    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)
    End Sub
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)

    End Sub
#End Region

#Region "Methods"
    Public Function CheckPageSizeInList(ByVal ps As Integer, ByVal drp As DropDownList) As Boolean
        For i As Integer = 0 To drp.Items.Count - 1
            If (drp.Items(i).Value.Trim() = ps.ToString()) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub InitDefaultPager()

        If (ViewMode <> 1) Then
            Dim pIndex As Integer
            Dim pSize As Integer
            Dim pSizeDefault As Integer
            Dim viewPaging As String = ""
            Dim enableViewPaging As String = ""
            'Try
            '    '' viewPaging = Convert.ToInt32(Request.QueryString(queryViewPaging))
            '    '' viewPaging = Convert.ToInt32(GetValueFromQuery(queryViewPaging, "0"))
            'Catch ex As Exception

            'End Try
            Try

                enableViewPaging = Convert.ToInt32(GetValueFromQuery(queryEnableViewPaging, "0"))
            Catch ex As Exception

            End Try
            Try

                pSize = Convert.ToInt32(GetValueFromQuery(QueryPageSize, "0"))
            Catch ex As Exception

            End Try
            If (enableViewPaging = "1" Or pSize = Int32.MaxValue) Then
                EnableViewPagingTextQuery = True
            Else
                EnableViewPagingTextQuery = False
            End If
            Try

                pIndex = Convert.ToInt32(GetValueFromQuery(QueryPageIndex, "0"))
            Catch ex As Exception

            End Try
            If (pIndex <= 0) Then
                pIndex = 1
            End If
            PageIndex = pIndex

            If (viewPaging <> "1") Then

                Try

                    pSizeDefault = Convert.ToInt32(GetValueFromQuery(QueryPageSizeDefault, "0"))
                Catch ex As Exception

                End Try
                If Not ShowTwoLine Then
                    If CheckPageSizeInList(pSize, drlPageSizeQuery) Or pSize = Int32.MaxValue Then
                        PageSize = pSize
                        DefaultPageSizeValid = True
                    Else
                        DefaultPageSizeValid = False
                    End If
                    If CheckPageSizeInList(pSizeDefault, drlPageSizeQuery) Then
                        DefaultPageSizeQuery = pSizeDefault
                    ElseIf (DefaultPageSizeValid) Then
                        DefaultPageSizeQuery = pSize
                    Else
                        DefaultPageSizeQuery = Convert.ToInt32(drlPageSizeQuery.SelectedValue)
                    End If
                Else
                    If CheckPageSizeInList(pSize, drlPageSizeQueryShow2Line) Or pSize = Int32.MaxValue Then
                        PageSize = pSize
                        DefaultPageSizeValid = True
                    Else
                        DefaultPageSizeValid = False
                    End If
                    If CheckPageSizeInList(pSizeDefault, drlPageSizeQueryShow2Line) Then
                        DefaultPageSizeQuery = pSizeDefault
                    ElseIf (DefaultPageSizeValid) Then
                        DefaultPageSizeQuery = pSize
                    Else
                        DefaultPageSizeQuery = Convert.ToInt32(drlPageSizeQueryShow2Line.SelectedValue)
                    End If
                End If


            Else
                Try
                    '' pSizeDefault = Convert.ToInt32(Request.QueryString(QueryPageSizeDefault))
                    pSizeDefault = Convert.ToInt32(GetValueFromQuery(QueryPageSizeDefault, "0"))
                Catch ex As Exception

                End Try
                If Not ShowTwoLine Then
                    If CheckPageSizeInList(pSizeDefault, drlPageSizePostBack) Then
                        DefaultPageSizeQuery = pSizeDefault
                    Else
                        DefaultPageSizeQuery = Convert.ToInt32(drlPageSizePostBack.SelectedValue)
                    End If
                Else
                    If CheckPageSizeInList(pSizeDefault, drlPageSizePostBack2Line) Then
                        DefaultPageSizeQuery = pSizeDefault
                    Else
                        DefaultPageSizeQuery = Convert.ToInt32(drlPageSizePostBack2Line.SelectedValue)
                    End If
                End If

                Me.PageSize = DefaultPageSizeQuery
            End If


        End If

    End Sub
    Sub InitPageIndexPostBackShow2Line()

        lbtnViewPagingPostBack2Line.Attributes.Add("style", "display:none")
        Dim wCell As Integer = 0
        Dim addPageIndex As Integer = 1
        While addPageIndex <= _countPageIndex
            'addd demo open page
            Dim PageIndex As New LinkButton()
            ' PageIndex.= "lbtnIndex_1";
            PageIndex.Text = addPageIndex.ToString()
            PageIndex.CssClass = "pageindexText"
            PageIndex.ID = Convert.ToString(Me.ID) & "_" & "PageIndex_" & addPageIndex.ToString()
            AddHandler PageIndex.Click, AddressOf PageIndex_Click
            tdPageIndexPostBack2Line.Controls.Add(PageIndex)
            addPageIndex += 1
        End While
    End Sub
    Private Sub SetInitPageData(ByRef left As Integer, ByRef right As Integer)
        Dim StartLoop As Integer = Math.Max(PageIndex - _countPageIndex, 1)
        Dim EndLoop As Integer = Math.Min(PageIndex + _countPageIndex, TotalPage)
        Dim bEnd As Boolean = False
        While EndLoop - StartLoop >= _countPageIndex
            If EndLoop - PageIndex > PageIndex - StartLoop Then
                EndLoop -= 1
            Else
                StartLoop += 1
            End If
        End While
        left = StartLoop
        right = EndLoop
    End Sub
    Public Sub SetPagingModeQuery(ByVal selectRow As Integer, ByVal totalRow As Integer)

        Me.TotalRecord = totalRow
        Me.TotalPage = Me._TotalRecord \ Me.PageSize
        If Me._TotalRecord Mod Me.PageSize <> 0 Then
            Me.TotalPage += 1
        End If
        Dim cell1 As New HtmlTableCell()
        cell1 = Me.tblParentQuery.Rows(0).Cells(5)
        If (Me.TotalPage <= _defaultShowSummary) Then
            cell1.Attributes.Add("style", "display:none")
        Else
            cell1.InnerText = String.Format(SumaryIndex, Me.PageIndex, Me.TotalPage)
        End If

        Dim style As String = Me.tblParentQuery.Rows(0).Cells(8).Style.Item("display")
        If (style <> "none") Then
            Dim result As String = ""
            Dim left As Integer = 1
            Dim right As Integer = _countPageIndex
            SetInitPageData(left, right)

            For i As Integer = left To right
                If i = PageIndex Then
                    result &= "<a  href=""javascript:void(0)"" class=""pageindexTextActive"">" & i & "</a>"
                Else
                    ''result &= "<a  href=" & PagingUrl & "&pi=" & i & "&pz=" & PageSize & "" & " class=""pageindexText"">" & i & "</a>"
                    result &= BuildPagingUrl(PagingUrl, i, PageSize, i, "pageindexText")
                End If
            Next
            tdPageIndexQuery.InnerHtml = result
        End If
        ''set value for Prev and Next button
        tdPrevQuery.InnerHtml = BuildPagingUrl(PagingUrl, PageIndex - 1, PageSize, "« Previous", "")
        tdNextQuery.InnerHtml = BuildPagingUrl(PagingUrl, PageIndex + 1, PageSize, "Next »", "")
        ''set value for View All button

        If IsFirstPage() Then
            Me.tblParentQuery.Rows(0).Cells(indexPrevPostBack).Attributes.Add("style", "display:none")
            Me.tblParentQuery.Rows(0).Cells(indexPrevPostBack - 1).Attributes.Add("class", "pageline1")
        ElseIf Me.TotalPage < 2 Then
            Me.tblParentQuery.Rows(0).Cells(indexPrevPostBack).Attributes.Add("style", "display:none")

        Else
            Me.tblParentQuery.Rows(0).Cells(indexPrevPostBack).Attributes.Add("style", "display:")
            Me.tblParentQuery.Rows(0).Cells(indexPrevPostBack - 1).Attributes.Add("class", "pageline")
        End If
        If IsLastPage() Then
            Me.tblParentQuery.Rows(0).Cells(indexNextPostBack).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then

            Me.tblParentQuery.Rows(0).Cells(indexNextPostBack).Attributes.Add("style", "display:none")
        Else
            Me.tblParentQuery.Rows(0).Cells(indexNextPostBack).Attributes.Add("style", "display:")
        End If
        If (PageSize = Int32.MaxValue Or Me.TotalPage < 2) Then
            If (EnableViewPagingTextQuery And DefaultPageSizeQuery < PageSize) Then

                tdViewAllQuery.InnerHtml = BuildPagingUrl(PagingUrl, 1, DefaultPageSizeQuery, "View Paging", "", True)
            Else
                tblParentQuery.Attributes.Add("style", "display:none")
            End If

            Dim i As Integer = 2
            While (i <= 9)
                Me.tblParentQuery.Rows(0).Cells(i).Attributes.Add("style", "display:none")
                i = i + 1
            End While
        Else
            If ShowViewAll Then
                tdViewAllQuery.InnerHtml = BuildPagingUrl(PagingUrl, 1, Int32.MaxValue, "View All", "")
            Else
                Me.tblParentQuery.Rows(0).Cells(1).Attributes.Add("style", "display:none")
                Me.tblParentQuery.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            End If

        End If
        If Not ShowPageSize Then
            Me.tblParentQuery.Rows(0).Cells(3).Attributes.Add("style", "display:none")
            Me.tblParentQuery.Rows(0).Cells(4).Attributes.Add("style", "display:none")
        Else
            If (PageSize <> Int32.MaxValue And Me.TotalPage > 1 And CheckPageSizeInList(PageSize, drlPageSizeQuery) = True) Then
                Me.tblParentQuery.Rows(0).Cells(3).Attributes.Add("style", "display:")
                Me.tblParentQuery.Rows(0).Cells(4).Attributes.Add("style", "display:")
            End If
        End If

    End Sub
    Public Sub SetPagingModeQueryShow2Line(ByVal selectRow As Integer, ByVal totalRow As Integer)

        Me.TotalRecord = totalRow
        Me.TotalPage = Me._TotalRecord \ Me.PageSize
        If Me._TotalRecord Mod Me.PageSize <> 0 Then
            Me.TotalPage += 1
        End If
        If (Me.TotalPage <= _defaultShowSummary) Then
            totalPageTextQueryShow2Line.InnerText = String.Empty
        Else
            totalPageTextQueryShow2Line.InnerText = String.Format(SumaryIndex, Me.PageIndex, Me.TotalPage)
        End If

        Dim style As String = tdPageIndexQueryShow2Line.Style.Item("display")
        If (style <> "none") Then
            Dim result As String = ""
            Dim left As Integer = 1
            Dim right As Integer = _countPageIndex
            SetInitPageData(left, right)

            For i As Integer = left To right
                If i = PageIndex Then
                    result &= "<a  href=""javascript:void(0)"" class=""pageindexTextActive"">" & i & "</a>"
                Else
                    ''result &= "<a  href=" & PagingUrl & "&pi=" & i & "&pz=" & PageSize & "" & " class=""pageindexText"">" & i & "</a>"
                    result &= BuildPagingUrl(PagingUrl, i, PageSize, i, "pageindexText")
                End If
            Next
            tdPageIndexQueryShow2Line.InnerHtml = result
        End If
        ''set value for Prev and Next button
        tdPrevQueryShow2Line.InnerHtml = BuildPagingUrl(PagingUrl, PageIndex - 1, PageSize, "« Previous", "")
        tdNextQueryShow2Line.InnerHtml = BuildPagingUrl(PagingUrl, PageIndex + 1, PageSize, "Next »", "")
        ''set value for View All button

        If IsFirstPage() Then
            Me.tblParentQueryShow2Line2.Rows(0).Cells(3).Attributes.Add("style", "display:none")
            Me.tblParentQueryShow2Line2.Rows(0).Cells(3).Attributes.Add("class", "pageline1")
        ElseIf Me.TotalPage < 2 Then
            Me.tblParentQueryShow2Line2.Rows(0).Cells(3).Attributes.Add("style", "display:none")

        Else
            Me.tblParentQueryShow2Line2.Rows(0).Cells(3).Attributes.Add("style", "display:")
            Me.tblParentQueryShow2Line2.Rows(0).Cells(2).Attributes.Add("class", "pageline")
        End If
        If IsLastPage() Then
            Me.tblParentQueryShow2Line2.Rows(0).Cells(5).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then

            Me.tblParentQueryShow2Line2.Rows(0).Cells(5).Attributes.Add("style", "display:none")
        Else
            Me.tblParentQueryShow2Line2.Rows(0).Cells(5).Attributes.Add("style", "display:")
        End If
        If (PageSize = Int32.MaxValue Or Me.TotalPage < 2) Then
            If EnableViewPagingTextQuery And DefaultPageSizeQuery < PageSize Then
                tdViewAllQueryShow2Line.InnerHtml = BuildPagingUrl(PagingUrl, 1, DefaultPageSizeQuery, "View Paging", "", True)
                tblParentQueryShow2Line1.Attributes.Add("style", "display:")
                Dim i As Integer = 2
                While (i <= 4)
                    Me.tblParentQueryShow2Line1.Rows(0).Cells(i).Attributes.Add("style", "display:none")
                    i = i + 1
                End While
                tblParentQueryShow2Line2.Attributes.Add("style", "display:none")
                show2LineViewPagingTextMode = True
            Else
                tblParentQueryShow2Line2.Attributes.Add("style", "display:none")
                tblParentQueryShow2Line1.Attributes.Add("style", "display:none")
            End If
            '' tdViewAllQueryShow2Line.InnerHtml = BuildPagingUrl(PagingUrl, 1, DefaultPageSizeQuery, "View Paging", "")

            ''Me.tblParentQueryShow2Line1.Attributes.Add("style", "display:none")
        Else
            If ShowViewAll Then
                tdViewAllQueryShow2Line.InnerHtml = BuildPagingUrl(PagingUrl, 1, Int32.MaxValue, "View All", "")
            Else
                Me.tblParentQueryShow2Line1.Rows(0).Cells(1).Attributes.Add("style", "display:none")
                Me.tblParentQueryShow2Line1.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            End If

        End If
        If Not ShowPageSize Then
            Me.tblParentQueryShow2Line1.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            Me.tblParentQueryShow2Line1.Rows(0).Cells(3).Attributes.Add("style", "display:none")
            Me.tblParentQueryShow2Line1.Rows(0).Cells(4).Attributes.Add("style", "display:none")
        Else
            If (PageSize <> Int32.MaxValue And Me.TotalPage > 1 And CheckPageSizeInList(PageSize, drlPageSizeQueryShow2Line) = True) Then
                Me.tblParentQuery.Rows(0).Cells(3).Attributes.Add("style", "display:")
                Me.tblParentQuery.Rows(0).Cells(4).Attributes.Add("style", "display:")
                Me.tblParentQueryShow2Line1.Rows(0).Cells(2).Attributes.Add("style", "display:")
            End If
        End If
        If (ShowPageSize And ShowViewAll) Then
            Me.tblParentQueryShow2Line1.Rows(0).Cells(2).Attributes.Add("style", "display:")
        Else
            Me.tblParentQueryShow2Line1.Rows(0).Cells(2).Attributes.Add("style", "display:none")
        End If
    End Sub
    Public Function BuildPagingUrl(ByVal url As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal text As String, ByVal cssClass As String) As String
        Return BuildPagingUrl(url, pageIndex, pageSize, text, cssClass, False)
    End Function
    ''' <summary>
    ''' Remove paging paramater from URL
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPagingRawURL(ByVal url As String) As String
        Return url
        'Dim result As String = url
        'Dim indexPageSize As Integer = url.IndexOf(QueryPageIndex)
        'If indexPageSize > 0 Then
        '    result = url.Substring(0, indexPageSize)
        'End If
        'If result.Contains("?") Then
        '    result = result.Replace("?", "")
        'End If
        'Return result
    End Function
    Public Function BuildPagingUrl(ByVal url As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal text As String, ByVal cssClass As String, ByVal viewPaging As Boolean) As String
        Dim qs As URLParameters = New URLParameters(Request.QueryString, pagingParamaterTemplate, False)
        Dim link As String = AddPagingParamater(qs, PagingUrl, pageIndex, pageSize, DefaultPageSizeQuery, EnableViewPagingTextQuery, viewPaging)
        Return "<a  href=""" & Server.HtmlEncode(link) & """ class=""" & cssClass & """>" & text & "</a>"
    End Function
    Public Sub SetPagingModePostBack2Line(ByVal selectRow As Integer, ByVal totalRow As Integer)

        If ViewAll Then ''SHOW view all mode
            If EnableViewPagingTextPostBack Then
                lbtnViewAllPostBack2Line.Text = "View Paging"
                Dim i As Integer = 2
                While (i <= 4)
                    Me.tblParentPostBack2Line.Rows(0).Cells(i).Attributes.Add("style", "display:none")
                    i = i + 1
                End While
            Else
                Me.tblParentPostBack2Line.Rows(0).Attributes.Add("style", "display:none")
            End If
            Me.tblParentPostBack2Line.Rows(1).Attributes.Add("style", "display:none")
            Exit Sub
        Else
            Me.TotalRecord = totalRow
            Me.TotalPage = Me._TotalRecord \ Me.PageSize
            If Me._TotalRecord Mod Me.PageSize <> 0 Then
                Me.TotalPage += 1
            End If
            If Me.TotalPage < 2 Then
                If EnableViewPagingTextPostBack Then
                    lbtnViewAllPostBack2Line.Attributes.Add("style", "display:none")
                    lbtnViewPagingPostBack2Line.Attributes.Add("style", "display:")
                    Dim i As Integer = 2
                    While (i <= 4)
                        Me.tblParentPostBack2Line.Rows(0).Cells(i).Attributes.Add("style", "display:none")
                        i = i + 1
                    End While
                Else
                    Me.tblParentPostBack2Line.Rows(0).Attributes.Add("style", "display:none")
                End If
                Me.tblParentPostBack2Line.Rows(1).Attributes.Add("style", "display:none")
            Else
                lbtnViewAllPostBack2Line.Attributes.Add("style", "display:")
                lbtnViewPagingPostBack2Line.Attributes.Add("style", "display:none")
                Dim i As Integer = 2
                While (i <= 4)
                    Me.tblParentPostBack.Rows(0).Cells(i).Attributes.Add("style", "display:")
                    i = i + 1
                End While
                Me.tblParentPostBack2Line.Rows(1).Attributes.Add("style", "display:")
            End If
            lbtnViewAllPostBack.Text = "View All"
        End If


        Dim style As String = tdPageIndexPostBack2Line.Style.Item("display")
        If (style <> "none") Then
            Dim lstControl As ControlCollection = tdPageIndexPostBack2Line.Controls
            Dim count As Integer = 1
            ''Dim cellW As Integer = CInt(cell.Width.Value)
            Dim currentIndexPage As Integer = Me.PageIndex
            'cacula left and right
            Dim left As Integer = 1
            Dim right As Integer = _countPageIndex
            SetInitPageData(left, right)
            Dim point As Integer = left
            Dim indexControl As Integer = 0
            While count <= _countPageIndex
                Dim lbtn As LinkButton = Nothing
                Try
                    lbtn = DirectCast(lstControl(indexControl), LinkButton)
                    If lbtn IsNot Nothing And lbtn.ID.Contains("lbtnPageIndexPostBack2Line") Then
                        If count <= (right - left + 1) Then
                            lbtn.Attributes.Add("style", "display:")
                            lbtn.Text = point.ToString()
                            If Me.PageIndex = point Then
                                lbtn.CssClass = "pageindexTextActive"
                                lbtn.Attributes.Add("onclick", "javascript:return false;")
                            Else
                                lbtn.CssClass = "pageindexText"
                                lbtn.Attributes.Add("onclick", "javascript:return true;")

                            End If
                        Else
                            lbtn.Attributes.Add("style", "display:none")
                            ''cellW = cellW - 10
                        End If
                    End If
                    count += 1
                    point += 1
                Catch

                End Try
                indexControl += 1

            End While
        End If

        If Me.TotalPage <= _defaultShowSummary Then
            totalPageTextShow2Line.InnerText = String.Empty
        Else
            totalPageTextShow2Line.InnerText = String.Format(SumaryIndex, Me.PageIndex, Me.TotalPage)
        End If

        If IsFirstPage() Then
            Me.tblSubPagingShow2Line.Rows(0).Cells(3).Attributes.Add("style", "display:none")
            Me.tblSubPagingShow2Line.Rows(0).Cells(2).Attributes.Add("class", "pageline1")
        ElseIf Me.TotalPage < 2 Then
            Me.tblSubPagingShow2Line.Rows(0).Cells(3).Attributes.Add("style", "display:none")
        Else
            Me.tblSubPagingShow2Line.Rows(0).Cells(3).Attributes.Add("style", "display:")
            Me.tblSubPagingShow2Line.Rows(0).Cells(2).Attributes.Add("class", "pageline")
        End If
        If IsLastPage() Then
            Me.tblSubPagingShow2Line.Rows(0).Cells(5).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then

            Me.tblSubPagingShow2Line.Rows(0).Cells(5).Attributes.Add("style", "display:none")
        Else
            Me.tblSubPagingShow2Line.Rows(0).Cells(5).Attributes.Add("style", "display:")
        End If
        If Not ShowPageSize Then
            Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            Me.tblParentPostBack2Line.Rows(0).Cells(3).Attributes.Add("style", "display:none")
            Me.tblParentPostBack2Line.Rows(0).Cells(4).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then
            Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            Me.tblParentPostBack2Line.Rows(0).Cells(3).Attributes.Add("style", "display:none")
            Me.tblParentPostBack2Line.Rows(0).Cells(4).Attributes.Add("style", "display:none")
        Else
            Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:")
            Me.tblParentPostBack2Line.Rows(0).Cells(3).Attributes.Add("style", "display:")
            Me.tblParentPostBack2Line.Rows(0).Cells(4).Attributes.Add("style", "display:")
        End If
        If Not ShowViewAll Then
            lbtnViewAllPostBack2Line.Attributes.Add("style", "display:none")
            Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then
            lbtnViewAllPostBack2Line.Attributes.Add("style", "display:none")
            Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:none")
        Else
            lbtnViewAllPostBack2Line.Attributes.Add("style", "display:")
            Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:")
            If Not ShowPageSize Then
                Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            ElseIf Me.TotalPage < 2 Then
                Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:none")
            Else
                Me.tblParentPostBack2Line.Rows(0).Cells(2).Attributes.Add("style", "display:")
            End If
        End If
    End Sub
    Public Sub SetPagingModePostBack(ByVal selectRow As Integer, ByVal totalRow As Integer)
        tblParentPostBack.Attributes.Add("style", "display:")
        lbtnViewAllPostBack.Attributes.Add("style", "display:")
        lbtnViewPagingPostBack.Attributes.Add("style", "display:none")
        ''ViewAll = False
        Dim i As Integer = 0
        While (i <= 9)
            Me.tblParentPostBack.Rows(0).Cells(i).Attributes.Add("style", "display:")
            i = i + 1
        End While

        If ViewAll Then
            If EnableViewPagingTextPostBack Then
                lbtnViewAllPostBack.Text = "View Paging"
                i = 2
                While (i <= 9)
                    Me.tblParentPostBack.Rows(0).Cells(i).Attributes.Add("style", "display:none")
                    i = i + 1
                End While
            Else
                Me.tblParentPostBack.Attributes.Add("style", "display:none")
            End If

            Exit Sub
        Else
            Me.TotalRecord = totalRow
            Me.TotalPage = Me._TotalRecord \ Me.PageSize
            If Me._TotalRecord Mod Me.PageSize <> 0 Then
                Me.TotalPage += 1
            End If
            If Me.TotalPage < 2 Then
                If EnableViewPagingTextPostBack Then
                    lbtnViewAllPostBack.Attributes.Add("style", "display:none")
                    lbtnViewPagingPostBack.Attributes.Add("style", "display:")
                    i = 2
                    While (i <= 9)
                        Me.tblParentPostBack.Rows(0).Cells(i).Attributes.Add("style", "display:none")
                        i = i + 1
                    End While
                Else
                    Me.tblParentPostBack.Attributes.Add("style", "display:none")
                End If

            Else
                lbtnViewAllPostBack.Attributes.Add("style", "display:")
                lbtnViewPagingPostBack.Attributes.Add("style", "display:none")
                i = 2
                While (i <= 9)
                    Me.tblParentPostBack.Rows(0).Cells(i).Attributes.Add("style", "display:")
                    i = i + 1
                End While

            End If
            lbtnViewAllPostBack.Text = "View All"
        End If
        ' this.PageIndex = pageIndex;           

        Dim style As String = Me.tblParentPostBack.Rows(0).Cells(8).Style.Item("display")
        If (style <> "none") Then
            Dim lstControl As ControlCollection = Me.tblParentPostBack.Rows(0).Cells(8).Controls
            Dim count As Integer = 1
            ''Dim cellW As Integer = CInt(cell.Width.Value)
            Dim currentIndexPage As Integer = Me.PageIndex
            'cacula left and right
            Dim left As Integer = 1
            Dim right As Integer = _countPageIndex
            SetInitPageData(left, right)
            Dim point As Integer = left
            Dim indexControl As Integer = 0
            While count <= _countPageIndex
                Dim lbtn As LinkButton = Nothing
                Try
                    lbtn = DirectCast(lstControl(indexControl), LinkButton)
                    If lbtn IsNot Nothing And lbtn.ID.Contains("lbtnPageIndexPostBack_") Then
                        If count <= (right - left + 1) Then
                            lbtn.Attributes.Add("style", "display:")
                            lbtn.Text = point.ToString()
                            If Me.PageIndex = point Then
                                lbtn.CssClass = "pageindexTextActive"
                                lbtn.Attributes.Add("onclick", "javascript:return false;")
                            Else
                                lbtn.CssClass = "pageindexText"
                                lbtn.Attributes.Add("onclick", "javascript:return true;")

                            End If
                        Else
                            lbtn.Attributes.Add("style", "display:none")
                            ''cellW = cellW - 10
                        End If
                    End If
                    count += 1
                    point += 1
                Catch

                End Try
                indexControl += 1

            End While
        End If
        Dim cell1 As New HtmlTableCell()
        cell1 = Me.tblParentPostBack.Rows(0).Cells(5)

        cell1.InnerText = String.Format(SumaryIndex, Me.PageIndex, Me.TotalPage)
        If (Me.TotalPage <= _defaultShowSummary) Then
            cell1.InnerText = String.Empty
        Else
            cell1.InnerText = String.Format(SumaryIndex, Me.PageIndex, Me.TotalPage)
        End If
        If IsFirstPage() Then
            Me.tblParentPostBack.Rows(0).Cells(indexPrevPostBack).Attributes.Add("style", "display:none")
            Me.tblParentPostBack.Rows(0).Cells(indexPrevPostBack - 1).Attributes.Add("class", "pageline1")
        ElseIf Me.TotalPage < 2 Then
            Me.tblParentPostBack.Rows(0).Cells(indexPrevPostBack).Attributes.Add("style", "display:none")
        Else
            Me.tblParentPostBack.Rows(0).Cells(indexPrevPostBack).Attributes.Add("style", "display:")
            Me.tblParentPostBack.Rows(0).Cells(indexPrevPostBack - 1).Attributes.Add("class", "pageline")
        End If
        If IsLastPage() Then
            Me.tblParentPostBack.Rows(0).Cells(indexNextPostBack).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then

            Me.tblParentPostBack.Rows(0).Cells(indexNextPostBack).Attributes.Add("style", "display:none")
        Else
            Me.tblParentPostBack.Rows(0).Cells(indexNextPostBack).Attributes.Add("style", "display:")
        End If
        If Not ShowPageSize Then
            Me.tblParentPostBack.Rows(0).Cells(indexPageSizePostBack).Attributes.Add("style", "display:none")
            Me.tblParentPostBack.Rows(0).Cells(indexPageSizePostBack - 1).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then
            Me.tblParentPostBack.Rows(0).Cells(indexPageSizePostBack).Attributes.Add("style", "display:none")
            Me.tblParentPostBack.Rows(0).Cells(indexPageSizePostBack - 1).Attributes.Add("style", "display:none")
        Else
            Me.tblParentPostBack.Rows(0).Cells(indexPageSizePostBack).Attributes.Add("style", "display:")
            Me.tblParentPostBack.Rows(0).Cells(indexPageSizePostBack - 1).Attributes.Add("style", "display:")
        End If
        If Not ShowViewAll Then
            lbtnViewAllPostBack.Attributes.Add("style", "display:none")
            Me.tblParentPostBack.Rows(0).Cells(indexViewAllPostBack + 1).Attributes.Add("style", "display:none")
        ElseIf Me.TotalPage < 2 Then
            lbtnViewAllPostBack.Attributes.Add("style", "display:none")
            Me.tblParentPostBack.Rows(0).Cells(indexViewAllPostBack + 1).Attributes.Add("style", "display:none")
        Else
            lbtnViewAllPostBack.Attributes.Add("style", "display:")
            Me.tblParentPostBack.Rows(0).Cells(indexViewAllPostBack + 1).Attributes.Add("style", "display:")
        End If
    End Sub
    Public Sub SetPaging(ByVal selectRow As Integer, ByVal totalRow As Integer)
        ''  Dim lstControl As ControlCollection = tdPageIndexPostBack.Controls

        If (ViewMode <> 1) Then
            If Not ShowTwoLine Then
                SetPagingModeQuery(selectRow, totalRow)
            Else
                SetPagingModeQueryShow2Line(selectRow, totalRow)
            End If

            Exit Sub
        End If
        If Not ShowTwoLine Then
            SetPagingModePostBack(selectRow, totalRow)
        Else
            SetPagingModePostBack2Line(selectRow, totalRow)
        End If

    End Sub
    ''' <summary>
    ''' Check if current page is the last page
    ''' </summary>
    ''' <returns></returns>
    Public Function IsLastPage() As Boolean
        If TotalPage = 0 Then
            Return True
        End If
        If PageIndex = TotalPage Then
            Return True
        End If
        Return False
    End Function
    ''' <summary>
    ''' Check if current page is the first page
    ''' </summary>
    ''' <returns></returns>
    Public Function IsFirstPage() As Boolean
        If TotalPage = 0 Then
            Return True
        End If
        If PageIndex = 1 Then
            Return True
        End If
        Return False
    End Function
    Public Function GetValueFromQuery(ByVal key As String, ByVal defaultValue As String) As String
        'Dim value As String = Request.QueryString("pg") ''Get from Query
        'If value = Nothing Then
        '    Return defaultValue
        'End If
        'Dim arr() As String = Split(value, "/")
        'If (arr.Length > 0) Then
        '    For Each s As String In arr
        '        If (s.Contains(key & "*")) Then
        '            Dim index As Integer = s.IndexOf("*")
        '            Return s.Substring(index, s.Length)
        '        End If
        '    Next
        'End If
        'Return defaultValue
        Return Request.QueryString(key)
    End Function
#End Region

#Region "Events"
    Private Function AddParamaterToURL(ByVal _qs As URLParameters, ByVal url As String, ByVal addParamName As String, ByVal addParamValue As String) As String
        Dim result As String = String.Empty
        If Not (url.Contains("?")) Then
            result = url & _qs.ToString(addParamName, addParamValue)
            Return result
        End If
        Dim lstParam As String = String.Empty
        Dim indexOfQ As Integer = url.IndexOf("?")
        Dim rootURL As String = String.Empty
        lstParam = url.Substring(indexOfQ + 1, url.Length - (indexOfQ + 1))
        rootURL = url.Substring(0, indexOfQ)
        If Not (lstParam.Contains("&")) Then
            lstParam = lstParam & "&"
        End If
        Dim arrParam As String() = lstParam.Split("&")
        ''check duplicate
        Dim buildURL As String = String.Empty
        Dim hasAdd As Boolean = False
        For Each tmpParam As String In arrParam
            If (String.IsNullOrEmpty(tmpParam)) Then
                Continue For
            End If
            If (tmpParam = addParamName & "=" & addParamValue) Then
                Return url
            End If
            Dim paramName As String = String.Empty
            Dim paramValue As String = String.Empty
            If Not (tmpParam.Contains("=")) Then
                paramName = tmpParam
            Else
                Dim arrTmp As String() = tmpParam.Split("=")
                paramName = arrTmp(0).ToString()
                paramValue = arrTmp(1).ToString()
            End If
            If (paramName = addParamName) Then
                buildURL &= "&" & paramName & "=" & addParamValue
                hasAdd = True
            Else
                buildURL &= "&" & paramName & "=" & paramValue
            End If
        Next
        ''add value
        If Not hasAdd Then
            buildURL &= "&" & addParamName & "=" & addParamValue
        End If
        If (buildURL.Substring(0, 1) = "&") Then
            buildURL = buildURL.Substring(1, buildURL.Length - 1)
        End If
        Return rootURL & "?" & buildURL
    End Function

    Private Function AddPagingParamater(ByVal _qs As URLParameters, ByVal _url As String, ByVal _pageIndex As Integer, ByVal _pageSize As Integer, ByVal _pageDefault As Integer, ByVal _enableViewPaging As Boolean, ByVal _viewPaging As Boolean) As String
        Dim link As String = String.Empty
        'If (_url.Contains("?")) Then
        '    link = _url & _qs.ToString(QueryPageIndex, _pageIndex).Replace("?", "&")
        'Else
        '    link = _url & _qs.ToString(QueryPageIndex, _pageIndex)
        'End If
        link = AddParamaterToURL(_qs, _url, QueryPageIndex, _pageIndex)
        _qs.Clear()
        link = AddParamaterToURL(_qs, link, QueryPageSize, _pageSize) '' _qs.ToString(QueryPageSize, _pageSize).Replace("?", "&")
        _qs.Clear()
        link = AddParamaterToURL(_qs, link, QueryPageSizeDefault, _pageDefault) '' _qs.ToString(QueryPageSizeDefault, _pageDefault).Replace("?", "&")
        _qs.Clear()
        If _enableViewPaging Then
            link = AddParamaterToURL(_qs, link, queryEnableViewPaging, "1") '' _qs.ToString(queryEnableViewPaging, "1").Replace("?", "&")
        Else
            '' link = link & _qs.ToString(queryEnableViewPaging, "0").Replace("?", "&")
        End If
        _qs.Clear()
        If _viewPaging Then
            link = AddParamaterToURL(_qs, link, queryViewPaging, "1") '' _qs.ToString(queryViewPaging, "1").Replace("?", "&")
        Else
            '' link = link & _qs.ToString(queryViewPaging, "0").Replace("?", "&")
        End If
        _qs.Clear()
        'Dim link As String = _url & "/" & _pageIndex & "/" & _pageSize & "/" & _pageDefault & "/"
        'If _enableViewPagting Then
        '    link &= "1/"
        'Else
        '    link &= "0/"
        'End If
        'If _viewPaging Then
        '    link &= "1"
        'Else
        '    link &= "0"
        'End If
        ''Dim link As String = _url & "/" & QueryPageIndex & "=" & _pageIndex & "/" & QueryPageSize & "=" & _pageSize & "/" & QueryPageSizeDefault & "=" & _pageDefault & "/" & queryEnableViewPaging & "=" & Convert.ToInt32(_enableViewPaging) & "/" & queryViewPaging & "=" & Convert.ToInt32(_viewPaging)
        ''check duplicate ?

        Return link
    End Function
    Private pagingParamaterTemplate As String = QueryPageIndex & ";" & QueryPageSize & ";" & QueryPageSizeDefault & ";" & queryEnableViewPaging & ";" & queryViewPaging
    Protected Sub drlPageSizeQuery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drlPageSizeQuery.SelectedIndexChanged
        Dim qs As URLParameters = New URLParameters(Request.QueryString, pagingParamaterTemplate, False)
        Dim link As String = AddPagingParamater(qs, PagingUrl, 1, PageSize, DefaultPageSizeQuery, 1, 0)
        Response.Redirect(link)
    End Sub
    Protected Sub drlPageSizeQueryShow2Line_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drlPageSizeQueryShow2Line.SelectedIndexChanged

        Dim qs As URLParameters = New URLParameters(Request.QueryString, pagingParamaterTemplate, False)
        Dim link As String = AddPagingParamater(qs, PagingUrl, 1, PageSize, DefaultPageSizeQuery, 1, 0)
        Response.Redirect(link)
    End Sub
    Protected Sub drlPageSizePostBack_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drlPageSizePostBack.SelectedIndexChanged
        EnableViewPagingTextPostBack = True
        PageSize = Convert.ToInt32(drlPageSizePostBack.SelectedValue)
        RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(PageSize))
    End Sub
    Protected Sub drlPageSizePostBack2Line_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drlPageSizePostBack2Line.SelectedIndexChanged
        EnableViewPagingTextPostBack = True
        PageSize = Convert.ToInt32(drlPageSizePostBack2Line.SelectedValue)
        RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(PageSize))
    End Sub
    Protected Sub lbtnNextPostBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnNextPostBack.Click
        Me.PageIndex = Me.PageIndex + 1
        If Me.PageIndex > TotalPage Then
            Me.PageIndex = TotalPage
        End If
        RaiseEvent PageIndexChanging(Me, New PageIndexChangeEventArgs(Me.PageIndex))
    End Sub
    Protected Sub lbtnPrevPostBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnPrevPostBack.Click
        Me.PageIndex = Me.PageIndex - 1
        If Me.PageIndex < 1 Then
            Me.PageIndex = 1
        End If
        RaiseEvent PageIndexChanging(Me, New PageIndexChangeEventArgs(Me.PageIndex))
    End Sub

    Protected Sub lbtnNextPostBack2Line_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnNextPostBack2Line.Click
        Me.PageIndex = Me.PageIndex + 1
        If Me.PageIndex > TotalPage Then
            Me.PageIndex = TotalPage
        End If
        RaiseEvent PageIndexChanging(Me, New PageIndexChangeEventArgs(Me.PageIndex))
    End Sub
    Protected Sub lbtnPrevPostBack2Line_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnPrevPostBack2Line.Click
        Me.PageIndex = Me.PageIndex - 1
        If Me.PageIndex < 1 Then
            Me.PageIndex = 1
        End If
        RaiseEvent PageIndexChanging(Me, New PageIndexChangeEventArgs(Me.PageIndex))
    End Sub

    Private Sub lbtnViewAllPostBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnViewAllPostBack.Click
        EnableViewPagingTextPostBack = True
        Me.PageIndex = 1
        If ViewAll = True Then
            ViewAll = False
            Me.PageSize = DefaultPageSize
            RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(Me.PageSize))
        Else
            lbtnViewPagingPostBack.Attributes.Add("style", "display:none")
            ViewAll = True
            RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(Int32.MaxValue))
        End If

    End Sub
    Private Sub lbtnViewAllPostBack2Line_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnViewAllPostBack2Line.Click
        EnableViewPagingTextPostBack = True
        Me.PageIndex = 1
        If ViewAll = True Then
            ViewAll = False
            Me.PageSize = DefaultPageSize
            RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(Me.PageSize))
        Else
            lbtnViewPagingPostBack2Line.Attributes.Add("style", "display:none")
            ViewAll = True
            RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(Int32.MaxValue))
        End If

    End Sub

    Private Sub lbtnViewPagingPostBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnViewPagingPostBack.Click
        Me.PageIndex = 1
        Me.PageSize = DefaultPageSize
        ViewAll = False
        ''lbtnViewAll.Attributes.Add("style", "display:none")
        ''  lbtnViewAll.Attributes.Add("style", "display:none")
        RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(Me.PageSize))
    End Sub
    Private Sub lbtnViewPagingPostBack2Line_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnViewPagingPostBack2Line.Click
        Me.PageIndex = 1
        Me.PageSize = DefaultPageSize
        ViewAll = False
        ''lbtnViewAll.Attributes.Add("style", "display:none")
        ''  lbtnViewAll.Attributes.Add("style", "display:none")
        RaiseEvent PageSizeChanging(Me, New PageSizeChangeEventArgs(Me.PageSize))
    End Sub
    Protected Sub PageIndex_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbtn As LinkButton = DirectCast(sender, LinkButton)
        Me.PageIndex = Integer.Parse(lbtn.Text)
        RaiseEvent PageIndexChanging(Me, New PageIndexChangeEventArgs(Me.PageIndex))
    End Sub
#End Region

End Class
