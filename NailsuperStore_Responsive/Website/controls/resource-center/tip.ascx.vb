Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class controls_resource_center_tip
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_TipsCategoryRow As TipCategoryRow = Nothing
    Private m_index As Integer = Nothing
    Private m_sSearch As String = Nothing
    Public Property TipsCategoryRow() As TipCategoryRow
        Set(ByVal value As TipCategoryRow)
            m_TipsCategoryRow = value
        End Set
        Get
            Return m_TipsCategoryRow
        End Get
    End Property
    Public Property index() As Integer
        Set(ByVal value As Integer)
            m_index = value
        End Set
        Get
            Return m_index
        End Get
    End Property
    Public Property sSearch() As String
        Set(ByVal value As String)
            m_sSearch = value
        End Set
        Get
            Return m_sSearch
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindData()
        End If

    End Sub
    Public Sub Fill()
        BindData()
    End Sub
    Public Sub BindData()
        'If TipsCategoryRow Is Nothing Then
        '    Exit Sub
        'End If

        Dim condition As String = String.Empty
        Dim TipCategoryId As Integer = 0

        Try
            If index < 1 AndAlso Not Session("indexRender") Is Nothing Then
                index = Session("indexRender")
            End If
            If TipsCategoryRow Is Nothing AndAlso Not Session("tipsRender") Is Nothing Then
                TipsCategoryRow = Session("tipsRender")
            End If
            If sSearch Is Nothing AndAlso Not Session("searchRender") Is Nothing Then
                sSearch = Session("searchRender")
            End If
            TipCategoryId = TipsCategoryRow.TipCategoryId
            Dim html As String = ""
            If Not Trim(sSearch) = String.Empty Then
                condition = " AND (TipId in (select [key] from CONTAINSTABLE(Tip, * , '" & sSearch & "')))"
            End If

            Dim lstTip As DataTable = TipRow.GetTipsByCategoryId(DB, TipCategoryId, condition)
            html = "<div id='tipItem_" & index.ToString() & "' class='dvTipCategory'>"
            html &= "<div class='title'>" & TipsCategoryRow.TipCategory & "</div>"
            html &= "<div class='shortdesc'>" & TipsCategoryRow.Description & "</div>"

            Dim ltrDvTips As String = ""
            If lstTip.Rows.Count > 0 Then
                ltrDvTips = "<ul class='divTip'>"
                For i As Integer = 0 To lstTip.Rows.Count - 1
                    ltrDvTips &= "<li><a href='" & URLParameters.TipDetailUrl(lstTip.Rows(i)("Title"), lstTip.Rows(i)("TipId")) & "'>" & lstTip.Rows(i)("Title") & "</a>"
                    ltrDvTips &= "<p>" & lstTip.Rows(i)("Summary") & "</p></li>"
                Next
            End If
            html &= ltrDvTips
            html &= "</ul></div>"
            dvTipCategory.Text = html

        Catch ex As Exception
            Email.SendError("ToError500", "[Tip]", "Url: " & Request.RawUrl & "<br>TipCategoryId: " & TipCategoryId & "<br>Condition: " & condition & "<br>Error: " & ex.ToString() & "<br>" & SitePage.GetSessionList())
        End Try
    End Sub
End Class
