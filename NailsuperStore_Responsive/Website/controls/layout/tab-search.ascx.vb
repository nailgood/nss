

Option Strict Off

Imports Components
Imports DataLayer
Imports System.Data

Partial Class controls_layout_tab_search
    Inherits ModuleControl

    Private m_ActiveTab As String
    Private m_CountProduct As Integer
    Private m_CountArticle As Integer
    Private m_CountVideo As Integer
    Private m_Args As String = String.Empty
    Private m_Keyword As String = String.Empty
    Private m_rawURL As String = String.Empty
    Public Property RawURL() As String
        Get
            Return m_rawURL
        End Get
        Set(ByVal value As String)
            m_rawURL = value
        End Set
    End Property
    Public Property CountProduct() As Integer
        Get
            Return m_CountProduct
        End Get
        Set(ByVal value As Integer)
            m_CountProduct = value
        End Set
    End Property
    Public Property CountArticle() As Integer
        Get
            Return m_CountArticle
        End Get
        Set(ByVal value As Integer)
            m_CountArticle = value
        End Set
    End Property
    Public Property CountVideo() As Integer
        Get
            Return m_CountVideo
        End Get
        Set(ByVal value As Integer)
            m_CountVideo = value
        End Set
    End Property
    Public Property ActiveTab() As String
        Get
            Return m_ActiveTab
        End Get
        Set(ByVal value As String)
            m_ActiveTab = value
        End Set
    End Property
    Public Property Keyword() As String
        Get
            Return m_Keyword
        End Get
        Set(ByVal value As String)
            m_Keyword = value
        End Set
    End Property
    Public Overrides Property Args() As String
        Get
            Return m_Args
        End Get
        Set(ByVal Value As String)
            m_Args = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        BindData()

    End Sub

    Private Sub BindData()
        If String.IsNullOrEmpty(RawURL) AndAlso Not Session("RawURLTabRender") Is Nothing Then
            RawURL = Session("RawURLTabRender")
        End If
        If String.IsNullOrEmpty(RawURL) Then
            RawURL = Request.RawUrl
        End If
        If String.IsNullOrEmpty(ActiveTab) AndAlso Not Session("ActiveTabRender") Is Nothing Then
            ActiveTab = Session("ActiveTabRender")
        End If
        If String.IsNullOrEmpty(Keyword) AndAlso Not Session("KeywordRender") Is Nothing Then
            Keyword = Session("KeywordRender")
        End If
        If CountProduct < 1 AndAlso Not Session("CountProductGender") Is Nothing Then
            CountProduct = Session("CountProductGender")
        End If
        If CountArticle < 1 AndAlso Not Session("CountArticleGender") Is Nothing Then
            CountArticle = Session("CountArticleGender")
        End If
        If CountVideo < 1 AndAlso Not Session("CountVideoGender") Is Nothing Then
            CountVideo = Session("CountVideoGender")
        End If
        Dim sUrl As String = RawURL
        '' sUrl = Me.Request.RawUrl
        If ActiveTab = "product" Then '' tab item on
            ltrTabContent.Text = "<li class='active'><a>Product (<span id='tabProductCount'>" & CountProduct & "</span>)</a></li>"
            If CountArticle > 0 Then
                ltrTabContent.Text &= "<li><a onclick='SearchArticle(" & CountProduct & "," & CountVideo & ");' href='javascript:void(0)'>Related Articles (" & CountArticle & ")</a></li>"

            End If
            If CountVideo > 0 Then
                ltrTabContent.Text &= "<li><a onclick='SearchVideo(" & CountProduct & "," & CountArticle & ",1," & Utility.ConfigData.PageSizeScroll & ");' href='javascript:void(0)'>Related Video (" & CountVideo & ")</a></li>"
            End If
        ElseIf ActiveTab = "article" Then
            ltrTabContent.Text = "<li><a href='" & URLParameters.AddParamaterToURL(sUrl, "tab", "", False) & "'>Product (" & CountProduct & ")</a></li>"
            ltrTabContent.Text &= "<li class='active'><a>Related Articles (" & CountArticle & ")</a></li>"
            If CountVideo > 0 Then
                ltrTabContent.Text &= "<li><a onclick='SearchVideo(" & CountProduct & "," & CountArticle & ",1," & Utility.ConfigData.PageSizeScroll & ");' href='javascript:void(0)'>Related Video (" & CountVideo & ")</a></li>"
            End If
            ltrTabContent.Text &= "<li class='back hidden-xs' id='lnkBackArticle'><a href='javascript:void(0);' onclick='SearchArticle(" & CountProduct & "," & CountVideo & ");'>Back</a></li>"

        Else
            ltrTabContent.Text = "<li><a href='" & URLParameters.AddParamaterToURL(sUrl, "tab", "", False) & "'>Product (" & CountProduct & ")</a></li>"
            If CountArticle > 0 Then
                ltrTabContent.Text &= "<li><a onclick='SearchArticle(" & CountProduct & "," & CountVideo & ");' href='javascript:void(0)'>Related Articles (" & CountArticle & ")</a></li>"
            End If
            ltrTabContent.Text &= "<li class='active'><a>Related Video (" & CountVideo & ")</a></li>"
        End If
    End Sub


End Class