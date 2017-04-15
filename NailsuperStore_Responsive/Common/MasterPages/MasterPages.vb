Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Configuration
Imports System.Web
Imports System.Web.UI
Imports DataLayer
Imports Components
Imports System.ComponentModel.Design
Imports System.Web.UI.WebControls
Imports System.Web.UI.Design.WebControls
Imports System.Web.UI.Design
Imports System.Web.UI.HtmlControls
Imports Utility

Namespace MasterPages

    Public Class MyContainerControlDesigner
        Inherits ContainerControlDesigner

        Public Overrides ReadOnly Property AllowResize() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    <Designer(GetType(MyContainerControlDesigner))> _
    <ParseChildren(False)> _
    <ToolboxData("<{0}:MasterPage runat=server></{0}:MasterPage>")> _
    Public Class MasterPage
        Inherits System.Web.UI.WebControls.WebControl

        Private m_TemplateContent As String
        Private m_DefaultContent As String

        Private Template As Control = Nothing
        Private Defaults As ContentRegion = New ContentRegion
        Private Contents As ArrayList = New ArrayList

        Private p As ContentToolPageRow
        Private t As ContentToolTemplateRow
        Private tr As ContentToolTemplateRegionRow
        Private Modules As DataView
        Private DB As Database

        Private m_PageTitle As String
        Private m_MetaKeywords As String
        Private m_MetaDescription As String
        Private m_MetaTitle As String
        Private m_IsIndexed As Boolean
        Private m_IsFollowed As Boolean

        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property

        Public Property MetaKeywords() As String
            Get
                Return m_MetaKeywords
            End Get
            Set(ByVal Value As String)
                m_MetaKeywords = Value
            End Set
        End Property

        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
            End Set
        End Property

        Public Property MetaTitle() As String
            Get
                Return m_MetaTitle
            End Get
            Set(ByVal Value As String)
                m_MetaTitle = Value
            End Set
        End Property

        Public Property IsIndexed() As Boolean
            Get
                Return m_IsIndexed
            End Get
            Set(ByVal Value As Boolean)
                m_IsIndexed = Value
            End Set
        End Property

        Public Property IsFollowed() As Boolean
            Get
                Return m_IsFollowed
            End Get
            Set(ByVal Value As Boolean)
                m_IsFollowed = Value
            End Set
        End Property

        Protected Property TemplateContent() As String
            Get
                Return m_TemplateContent
            End Get
            Set(ByVal Value As String)
                m_TemplateContent = Value
            End Set
        End Property

        <Category("MasterPage"), Description("Control ID for Default Content")> _
        Public Property DefaultContent() As String
            Get
                Return Me.m_DefaultContent
            End Get
            Set(ByVal Value As String)
                Me.m_DefaultContent = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Protected Overrides Sub AddParsedSubObject(ByVal obj As Object)
            If TypeOf obj Is ContentRegion Then
                Me.Contents.Add(obj)
            Else
                Me.Defaults.Controls.Add(CType(obj, Control))
            End If
        End Sub
        'Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        '    form.Action = Request.RawUrl
        '    MyBase.OnLoad(e)
        'End Sub
        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            If DesignMode Then Exit Sub

            Dim bp As BasePage = CType(Me.Page, BasePage)
            DB = bp.DB

            p = ContentToolPageRow.GetRowByURL(DB, HttpContext.Current.Request("URL"))
            'If (p.PageId < 1) Then
            '    HttpContext.Current.Response.Clear()
            '    HttpContext.Current.Response.Status = "301 Moved Permanently"
            '    HttpContext.Current.Response.AddHeader("Location", "/")
            '    HttpContext.Current.Response.End()
            'End If
            t = ContentToolTemplateRow.GetRow(DB, p.TemplateId)
            tr = ContentToolTemplateRegionRow.GetRow(DB, t.DefaultContentId)
            Modules = p.GetPageModules(ModuleLevel.All)

            Me.LoadModulesFromDB()
            Me.BuildMasterPage()
            Me.BuildContents()

            'Load default values for Title and Meta tags
            PageTitle = p.Title




            IsIndexed = p.IsIndexed
            IsFollowed = p.IsFollowed
            MetaKeywords = p.MetaKeywords
            MetaDescription = p.MetaDescription
            MetaTitle = p.MetaTitle

            MyBase.OnInit(e)
        End Sub

        Private Function GetRegion(ByVal ID As String) As ContentRegion
            For Each item As ContentRegion In Contents
                If item.ID = ID Then
                    Return item
                End If
            Next
            Return Nothing
        End Function

        Private Sub LoadModulesFromDB()
            If Modules Is Nothing Then
                Exit Sub
            End If
            For Each row As DataRowView In Modules
                Try
                    Dim ID As String = row("ContentRegion")
                    Dim ControlURL As String = IIf(IsDBNull(row("ControlURL")), String.Empty, row("ControlURL"))
                    Dim Args As String = IIf(IsDBNull(row("Args")), String.Empty, row("Args"))
                    Dim HTML As String = IIf(IsDBNull(row("HTML")), String.Empty, row("HTML"))

                    'create new region (if doesn't exist)
                    Dim region As ContentRegion = GetRegion(ID)
                    If region Is Nothing Then
                        region = New ContentRegion
                        region.ID = ID
                        Me.Contents.Add(region)
                    End If
                    Dim ctrl As Control = Nothing
                    'If ControlURL = "/modules/Menu.ascx" Then
                    '    Dim key As String = "mainMenu"
                    '    ctrl = CType(CacheUtils.GetCache(key), Control)
                    '    If (ctrl Is Nothing) Then
                    '        ctrl = Page.LoadControl(ControlURL)
                    '        CacheUtils.SetCache(key, ctrl, 900)
                    '    End If
                    'Else
                    '    ctrl = Page.LoadControl(ControlURL)
                    'End If
                    ctrl = Page.LoadControl(ControlURL)
                    region.Controls.Add(ctrl)
                    If TypeOf ctrl Is System.Web.UI.PartialCachingControl Then
                        Dim cached As Control = CType(ctrl, PartialCachingControl).CachedControl
                        If Not cached Is Nothing Then
                            ctrl = cached
                        End If
                    End If

                    Dim c As IModule = Nothing
                    If (TypeOf ctrl Is IModule) Then
                        c = CType(ctrl, IModule)
                    End If
                    If Not c Is Nothing Then
                        c.Args = Args
                        If Not HTML = String.Empty Then
                            c.HTMLContent = HTML
                        End If
                    End If
                Catch ex As Exception

                End Try

            Next

        End Sub

        Private Sub BuildMasterPage()
            TemplateContent = t.TemplateHTML
            DefaultContent = tr.ContentRegion

            If (Me.TemplateContent = String.Empty) Then
                'If page has not been registered then just leave the sub
                Return
            End If

            Me.Template = Page.ParseControl(TemplateContent)
            Me.Template.ID = Me.ID + "_Template"

            Dim count As Integer = Me.Template.Controls.Count
            Dim index As Integer
            For index = 0 To count - 1
                Dim control As Control = Me.Template.Controls(0)
                Me.Template.Controls.Remove(control)
                If control.Visible Then Me.Controls.Add(control)
            Next
            Me.Controls.AddAt(0, Me.Template)
        End Sub

        Private Sub BuildContents()
            Dim content As ContentRegion
            For Each content In Me.Contents
                Dim region As Control = Me.FindControl(content.ID)
                If region Is Nothing Or Not (TypeOf region Is ContentRegion) Then
                    ''Throw New Exception("ContentRegion with ID '" + content.ID + "' must be Defined")

                    HttpContext.Current.Response.Clear()
                    HttpContext.Current.Response.Status = "301 Moved Permanently"
                    HttpContext.Current.Response.AddHeader("Location", "/")
                    HttpContext.Current.Response.End()

                End If
                region.Controls.Clear()
                If (region.ID = "CT_RightPane") Then
                    Dim count As Integer = content.Controls.Count
                    Dim index As Integer
                    For index = 0 To count - 1
                        Dim control As Control = content.Controls(0)
                        If Not (control Is Nothing) Then
                            If Not control.TemplateControl Is Nothing Then
                                If (control.TemplateControl.AppRelativeVirtualPath.Contains("RightShipping.ascx")) Then
                                    control.ID = "ucRightShipping"
                                End If
                            End If
                        End If
                        content.Controls.Remove(control)
                        region.Controls.Add(control)
                    Next
                ElseIf (region.ID = "CT_Header") Then
                    Dim count As Integer = content.Controls.Count
                    Dim index As Integer
                    For index = 0 To count - 1
                        Dim control As Control = content.Controls(0)
                        If Not (control Is Nothing) Then
                            If Not control.TemplateControl Is Nothing Then
                                If (control.TemplateControl.AppRelativeVirtualPath.Contains("Header.ascx")) Then
                                    control.ID = "ucHeader"
                                End If
                            End If
                        End If
                        content.Controls.Remove(control)
                        region.Controls.Add(control)
                    Next
                Else
                    Dim count As Integer = content.Controls.Count
                    Dim index As Integer
                    For index = 0 To count - 1
                        Dim control As Control = content.Controls(0)
                        content.Controls.Remove(control)
                        region.Controls.Add(control)
                    Next
                End If

            Next

            'Add page controls to default control
            If Defaults.HasControls() Then
                Dim region As Control
                'If DefaultContent is blank, then page has not been registered
                'In that case add controls to this control
                If DefaultContent = Nothing Then
                    region = Me
                Else
                    region = FindControl(DefaultContent)
                End If
                If p.IsContentBefore Then
                    region.Controls.AddAt(0, Defaults)
                Else
                    region.Controls.Add(Defaults)
                End If
            End If

        End Sub

        Public Overrides Sub RenderBeginTag(ByVal writer As System.Web.UI.HtmlTextWriter)
        End Sub

        Public Overrides Sub RenderEndTag(ByVal writer As System.Web.UI.HtmlTextWriter)
        End Sub

        Private Sub MasterPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Change the title and meta tags
            On Error Resume Next
            Dim form As HtmlForm = CType(Me.FindControl("main"), HtmlForm)
            form.Action = HttpContext.Current.Request.RawUrl

            Dim Keywords As Literal = CType(Me.FindControl("MetaKeywords"), System.Web.UI.WebControls.Literal)
            Dim Description As Literal = CType(Me.FindControl("MetaDescription"), System.Web.UI.WebControls.Literal)
            Dim Title As Literal = CType(Me.FindControl("MetaTitle"), System.Web.UI.WebControls.Literal)
            Dim Robots As Literal = CType(Me.FindControl("MetaRobots"), System.Web.UI.WebControls.Literal)
            Dim GoogleInclude As Literal = CType(Me.FindControl("GoogleInclude"), System.Web.UI.WebControls.Literal)

            CType(Me.FindControl("PageTitle"), HtmlGenericControl).InnerText = PageTitle
            Title.Text = "<meta name=""title"" content=""" & MetaTitle & """ />"
            Keywords.Text = "<meta name=""keywords"" content=""" & MetaKeywords & """ />"
            Description.Text = "<meta name=""description"" content=""" & MetaDescription & """ />"

            If ConfigData.GlobalRefererName.Contains("test") Then
                Robots.Text = "<meta name=""robots"" content=""noindex,nofollow"" />"
            Else
                Robots.Text = "<meta name=""robots"" content=""" & IIf(IsIndexed, "index", "noindex") & "," & IIf(IsFollowed, "follow", "nofollow") & """ />"
            End If

            GoogleInclude.Text = "<script src=""http://maps.google.com/maps?file=api&amp;v=2&amp;key=" & System.Configuration.ConfigurationManager.AppSettings("GoogleAPIKey") & """ type=""text/javascript""></script>"
            On Error GoTo 0
        End Sub
    End Class

End Namespace