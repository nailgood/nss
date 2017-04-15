Imports Microsoft.VisualBasic
Imports Controls
Imports System.Web.UI
Imports System.Collections.Specialized
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Components
    Public Class RenderPage
        Inherits System.Web.UI.Page
        Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
            ''MyBase.VerifyRenderingInServerForm(control)
        End Sub
    End Class
    Public Class BasePage
        Inherits System.Web.UI.Page

        Private m_DB As Database
        Private m_ErrHandler As ErrorHandler
        Private m_Theme As DataLayer.ThemeRow

        Public ReadOnly Property SiteTheme() As DataLayer.ThemeRow
            Get
                If m_Theme Is Nothing OrElse IsNumeric(Session("ThemeId")) AndAlso CInt(Session("ThemeId")) <> m_Theme.ThemeId Then
                    m_Theme = DataLayer.ThemeRow.GetActiveRow(DB)
                End If
                Return m_Theme
            End Get
        End Property

        Protected SQL As String

        Protected Sub RefreshValueOnPostback(ByVal ctrl As Control)
            If IsPostBack Then
                Dim hnd As IPostBackDataHandler = CType(ctrl, IPostBackDataHandler)
                hnd.LoadPostData(ctrl.UniqueID, Request.Form)
            End If
        End Sub



        Public ReadOnly Property DB() As Database
            Get
                If m_DB Is Nothing Then
                    'open database connection
                    m_DB = New Database

                    'm_DB.Open(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
                    m_DB.Open(System.Configuration.ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
                End If
                Return m_DB
            End Get
        End Property

        Public ReadOnly Property ErrHandler() As ErrorHandler
            Get
                If m_ErrHandler Is Nothing Then m_ErrHandler = New ErrorHandler(DB)
                Return m_ErrHandler
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Shared Function KillChars(ByVal strInput) As String
            Dim badChars(16) As String
            badChars(0) = "select "
            badChars(1) = "drop "
            badChars(2) = "insert "
            badChars(3) = "delete "
            badChars(4) = "select%20"
            badChars(5) = "drop%20"
            badChars(6) = "insert%20"
            badChars(7) = "delete%20"
            badChars(8) = "union "
            badChars(9) = "union%20"

            badChars(10) = ";"
            badChars(11) = "--"
            badChars(12) = "xp_"
            badChars(13) = "*/"
            badChars(14) = "/*"
            badChars(15) = "/trackback/"

            Dim newChars As String = strInput
            For i = 0 To UBound(badChars)
                newChars = Replace(newChars, badChars(i), "")
            Next
            Return newChars
        End Function

        Public Shared Function EscapeQuotes(ByVal strInput) As String
            strInput = Replace(strInput, "'", Chr(34))
            EscapeQuotes = strInput
        End Function

        Public Shared Function GetQueryString(ByVal QueryStringName) As String
            Try
                Dim str As String = KillChars(System.Web.HttpContext.Current.Request.QueryString(QueryStringName))
                Return EscapeQuotes(str)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public ReadOnly Property GetPageParams() As String
            Get
                Dim Params As New FilterFields
                Return Params.ToString(Components.FilterFieldType.All, "", ViewState)
            End Get
        End Property

        Public ReadOnly Property GetPageParams(ByVal preserve As FilterFieldType) As String
            Get
                Dim Params As New FilterFields
                Return Params.ToString(preserve, "", ViewState)
            End Get
        End Property

        Public ReadOnly Property GetPageParams(ByVal preserve As FilterFieldType, ByVal removeList As String) As String
            Get
                Dim Params As New FilterFields
                Return Params.ToString(preserve, removeList, ViewState)
            End Get
        End Property

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            If Not m_DB Is Nothing Then
                If Not m_DB.Transaction Is Nothing Then m_DB.RollbackTransaction()
                m_DB.Dispose()
                m_DB = Nothing
            End If
        End Sub

        Public Sub AddError(ByVal ErrorMessage As String)
            'Find Error Placeholder and update with error (only for front end)
            Dim ph As MasterPages.ErrorMessage = ErrorPlaceHolder
            If Not ph Is Nothing Then ph.AddError(ErrorMessage)
        End Sub
        Public Sub AddErrorTitle(ByVal title As String)
            'Find Error Placeholder and update with error (only for front end)
            Dim ph As MasterPages.ErrorMessage = ErrorPlaceHolder
            If Not ph Is Nothing Then ph.AddErrorTitle(title)
        End Sub
        Private ReadOnly Property ErrorPlaceHolder() As MasterPages.ErrorMessage
            Get
                Dim eph As MasterPages.ErrorMessage = FindControl("ErrorPlaceHolder")

                'If this is admin section, then find the ErrorPlaceHolder in the master page
                If eph Is Nothing AndAlso Not Master Is Nothing Then
                    eph = Master.FindControl("ErrorPlaceHolder")
                    '' tim ErrorPlaceHolder o ben trong ContentPlaceHolder co Id: cphContent
                    If eph Is Nothing Then
                        eph = Master.FindControl("cphContent").FindControl("ErrorPlaceHolder")
                    End If
                End If
                Return eph
            End Get
        End Property

        Public Overrides Sub Validate()
            MyBase.Validate()

            Dim ph As MasterPages.ErrorMessage = ErrorPlaceHolder
            If Not ph Is Nothing Then ph.UpdateSummary()
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            Dim ph As MasterPages.ErrorMessage = ErrorPlaceHolder
            If Not ph Is Nothing Then ph.UpdateVisibility()

            MyBase.OnPreRender(e)
        End Sub

        Public Shared Function NumberToString(ByVal value As Integer) As String
            If value = 0 Then
                Return "0"
            End If
            Return String.Format("{0:###,###}", value)
        End Function
        Private Shared aspNetFormElements As String() = New String() {"__EVENTTARGET", "__EVENTARGUMENT", "__VIEWSTATE", "__EVENTVALIDATION", "__VIEWSTATEENCRYPTED"}
        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            Dim url As String = Request.RawUrl
            If url.Contains("/members/login.aspx") Or url.Contains("/store/cart.aspx") Then
                MyBase.Render(writer)
                Exit Sub
            End If
            Dim stringWriter As New StringWriter()
            Dim htmlWriter As New HtmlTextWriter(stringWriter)
            MyBase.Render(htmlWriter)
            Dim webresouce As String = "<script src=""/WebResource.axd?d=keUJLeRjuFRJLTZeLO50wfTz9-IQ3Uhj5FB-Acfjxmnhx81DYiTDWyVvPEG94yo4M9RUgWdGjCewXUH0fGcjYbWSCSg1&amp;t=635418424260000000"" type=""text/javascript""></script>"
            Dim pageurl As String = Request.Url.ToString()
            Dim html As String = stringWriter.ToString()
            Try
                Dim isHomepage As Boolean = False
                If pageurl <> Utility.ConfigData.GlobalRefererName Then
                    pageurl = pageurl.Replace(Utility.ConfigData.GlobalRefererName, "")
                    If pageurl.Contains("?") Then
                        pageurl = pageurl.Split("?")(0)
                    End If
                Else
                    isHomepage = True
                End If

                If (html.ToString().Contains(webresouce) And Utility.ConfigData.PageNotWebResource.Contains(pageurl)) Or isHomepage Then
                    html = html.ToString().Replace(webresouce, "")
                    'html = html.ToString().Replace("<script type=""text/javascript"" src=""/includes/scripts/layout.js"" defer=""defer""></script>", "<script src=""/WebResource.axd?d=keUJLeRjuFRJLTZeLO50wfTz9-IQ3Uhj5FB-Acfjxmnhx81DYiTDWyVvPEG94yo4M9RUgWdGjCewXUH0fGcjYbWSCSg1&amp;t=634773866700000000"" type=""text/javascript""></script>" & Environment.NewLine & "<script type=""text/javascript"" src=""/includes/scripts/layout.js"" defer=""defer""></script>")
                    'remove whitespace
                    'html = REGEX_BETWEEN_TAGS.Replace(html, "> <")
                    'html = REGEX_LINE_BREAKS.Replace(html, String.Empty)
                    '''''''''''''
                End If
            Catch
            End Try

            Dim formStart As Integer = html.IndexOf("<form")
            Dim endForm As Integer = -1
            If formStart >= 0 Then
                endForm = html.IndexOf(">", formStart)
            End If
            If endForm >= 0 Then
                Dim viewStateBuilder As New StringBuilder()
                For Each element As String In aspNetFormElements
                    Dim startPoint As Integer = html.IndexOf("<input type=""hidden"" name=""" & element & """")
                    If startPoint >= 0 AndAlso startPoint > endForm Then
                        Dim endPoint As Integer = html.IndexOf("/>", startPoint)
                        If endPoint >= 0 Then
                            endPoint += 2
                            Dim viewStateInput As String = html.Substring(startPoint, endPoint - startPoint)
                            html = html.Remove(startPoint, endPoint - startPoint)
                            viewStateBuilder.Append(viewStateInput).Append(vbCr & vbLf)
                        End If
                    End If
                Next
                If viewStateBuilder.Length > 0 Then
                    viewStateBuilder.Insert(0, vbCr & vbLf)
                    html = html.Insert(endForm + 1, viewStateBuilder.ToString())
                End If
            End If
            writer.Write(html)
        End Sub
        Private Shared ReadOnly REGEX_BETWEEN_TAGS As New Regex(">\s+<", RegexOptions.Compiled)
        Private Shared ReadOnly REGEX_LINE_BREAKS As New Regex("\n\s+", RegexOptions.Compiled)
        Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object

            Dim vState As String = String.Empty
            Try
                Dim pageStatePersister1 As System.Web.UI.PageStatePersister = Me.PageStatePersister
                pageStatePersister1.Load()
                vState = pageStatePersister1.ViewState.ToString()
                Dim pBytes As Byte() = System.Convert.FromBase64String(vState)
                pBytes = Decompress(pBytes)
                Dim mFormat As New LosFormatter()
                Dim ViewState As Object = mFormat.Deserialize(System.Convert.ToBase64String(pBytes))
                Return New Pair(pageStatePersister1.ControlState, ViewState)
            Catch ex As Exception
                Dim url As String = Request.RawUrl
                If url.Contains("/video/share.aspx") Then
                    Email.SendError("ToError500", "LoadPageStateFromPersistenceMedium Error", vState & "<br>Exception: " & ex.ToString())
                End If
            End Try

        End Function
        Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal pViewState As Object)
            'Dim url As String = Request.RawUrl
            'If url.Contains("/members/login.aspx") Then
            '    Exit Sub
            'End If
            Dim pair1 As Pair
            Dim pageStatePersister1 As System.Web.UI.PageStatePersister = Me.PageStatePersister
            Dim ViewState As Object
            If TypeOf pViewState Is Pair Then
                pair1 = DirectCast(pViewState, Pair)
                pageStatePersister1.ControlState = pair1.First
                ViewState = pair1.Second
            Else
                ViewState = pViewState
            End If
            Dim mFormat As New LosFormatter()
            Dim mWriter As New StringWriter()
            mFormat.Serialize(mWriter, ViewState)
            Dim mViewStateStr As String = mWriter.ToString()
            Dim pBytes As Byte() = System.Convert.FromBase64String(mViewStateStr)
            pBytes = Compress(pBytes)
            Dim vStateStr As String = System.Convert.ToBase64String(pBytes)
            pageStatePersister1.ViewState = vStateStr
            pageStatePersister1.Save()
        End Sub
        Private Function Compress(ByVal b As Byte()) As Byte()
            Dim ms As MemoryStream = New MemoryStream()
            Dim zs As GZipStream = New GZipStream(ms, CompressionMode.Compress, True)
            zs.Write(b, 0, b.Length)
            zs.Close()
            Return ms.ToArray()
        End Function
        Private Function Decompress(ByVal b As Byte()) As Byte()
            Dim ms As MemoryStream = New MemoryStream()
            Dim zs As GZipStream = New GZipStream(New MemoryStream(b), CompressionMode.Decompress, True)
            Dim buffer As Byte() = New Byte(4095) {}
            Dim size As Integer
            While True
                size = zs.Read(buffer, 0, buffer.Length)
                If size > 0 Then
                    ms.Write(buffer, 0, size)
                Else
                    Exit While
                End If
            End While
            zs.Close()
            Return ms.ToArray()

        End Function
    End Class
End Namespace
