Imports Microsoft.VisualBasic
Imports DataLayer
Imports System.Threading
Imports System.Xml

Namespace Components

    Public Class AdminPage
        Inherits BasePage

        Protected LoggedInAdminId As Integer
        Protected LoggedInIsInternal As Boolean
        Protected LoggedInUsername As String

        Public Function ImportIsRunning() As Boolean
            ImportIsRunning = False
            Try
                Dim grantedOwnership As Boolean

                Dim singleInstanceMutex As Mutex = New Mutex(False, "NavisionImport", grantedOwnership)
                If Not grantedOwnership Then ImportIsRunning = True
                singleInstanceMutex.Close()
            Catch ex As Exception
                ImportIsRunning = True
                Exit Function
            End Try
        End Function

        Protected Sub CheckAccess(ByVal action As String)
            If Not HasRights(action, False) Then
                Response.Redirect("/admin/Unauthorized.aspx")
            End If

            'CLEAR FrameURL FROM SESSION
            If Not LCase(Request.ServerVariables("URL")) = "/admin/default.aspx" Then
                Session("FrameURL") = String.Empty
            End If
        End Sub

        Protected Function HasRights(ByVal action As String, ByVal fromMenu As Boolean) As Boolean

            If Not fromMenu Then
                Return True
            End If
            If Context.User.Identity.IsAuthenticated Then
                Dim principal As AdminPrincipal = CType(Context.User, AdminPrincipal)
                Dim aAction() As String
                Dim bHasPermission As Boolean

                aAction = action.Split(","c)
                For Each s As String In aAction
                    bHasPermission = principal.HasPermission(s)
                    If bHasPermission Then Exit For
                Next
                Return bHasPermission
            Else
                Return False
            End If

        End Function
        Protected Function HasRights(ByVal action As String) As Boolean

            If Context.User.Identity.IsAuthenticated Then
                Dim principal As AdminPrincipal = CType(Context.User, AdminPrincipal)
                Dim aAction() As String
                Dim bHasPermission As Boolean

                aAction = action.Split(","c)
                For Each s As String In aAction
                    bHasPermission = principal.HasPermission(s)
                    If bHasPermission Then Exit For
                Next
                Return bHasPermission
            Else
                Return False
            End If

        End Function
        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim lastURL As String = String.Empty
            'If not logged in then redirect to login page
            If Not Context.User.Identity.IsAuthenticated Then
                lastURL = Utility.ConfigData.GlobalRefererName & Request.RawUrl
                'If lastURL.Contains("/admin/default.aspx") Then
                '    lastURL = String.Empty
                'End If
                Session("LastAdminURL") = lastURL
                '' Session("FrameURL") = lastURL
                Response.Redirect("/admin/Login.aspx")
                Exit Sub
            End If

            If Request.RawUrl.Contains("/admin/default.aspx") Then
                ''get FrameURL
                Dim frameURL As String
                If Not Request.QueryString("fr") Is Nothing Then
                    frameURL = Request.QueryString("fr").Trim().ToLower
                    Select Case frameURL
                        Case "/admin/store/items/reviews/edit.aspx"
                            Dim reviewId As Integer = CInt(Request.QueryString("ReviewId"))
                            Dim itemId As Integer = CInt(Request.QueryString("ItemId"))
                            If reviewId < 1 Then
                                Exit Sub
                            End If
                            If itemId > 0 Then
                                lastURL = Utility.ConfigData.GlobalRefererName & frameURL & "?ReviewId=" & reviewId & "&ItemId=" & itemId & "&editcomment=1"
                            Else
                                lastURL = Utility.ConfigData.GlobalRefererName & frameURL & "?ReviewId=" & reviewId & "&editEmail=1"
                            End If
                        Case "/admin/store/orders/reviews/edit.aspx"
                            Dim orderId As Integer = CInt(Request.QueryString("OrderId"))
                            If orderId > 0 Then
                                lastURL = Utility.ConfigData.GlobalRefererName & frameURL & "?OrderId=" & orderId & "&editFromEmail=1"
                            End If
                        Case "/admin/store/orders/edit.aspx"
                            Dim OrderId As Integer = CInt(Request.QueryString("OrderId"))
                            If OrderId > 0 Then
                                lastURL = Utility.ConfigData.GlobalRefererName & frameURL & "?OrderId=" & OrderId
                            End If
                        Case Else


                    End Select
                    Session("FrameURL") = lastURL
                    Response.Redirect("/admin/default.aspx")
                    Exit Sub
                End If
            End If

            Dim newUser As AdminPrincipal = Nothing
            If TypeOf Context.User Is AdminPrincipal Then
                newUser = CType(Context.User, AdminPrincipal)
            Else
                Try
                    newUser = New AdminPrincipal(DB, Context.User.Identity.Name)
                    Context.User = newUser

                    ' Save AdminLog, but only once per session
                    If Session("AdminId") Is Nothing Then
                        Dim newIdentity As AdminIdentity = CType(newUser.Identity, AdminIdentity)

                        Dim dbAdminLog As New AdminLogRow()
                        dbAdminLog.AdminId = newIdentity.AdminId
                        dbAdminLog.Username = newIdentity.Username
                        dbAdminLog.RemoteIP = Request.ServerVariables("REMOTE_ADDR")
                        dbAdminLog.LoginDate = Now()
                        dbAdminLog.ComputerName = System.Net.Dns.GetHostName()
                        dbAdminLog.LanIP = Utility.Common.GetComputerLanIP()
                        Session("LogId") = AdminRow.DoLogin(DB, dbAdminLog)
                        Session("AdminId") = newIdentity.AdminId
                        Session("Track_AdminName") = newIdentity.Username
                    End If

                Catch ex As Exception
                    Response.Redirect("/admin/logout.aspx")
                End Try

            End If
            LoggedInAdminId = CType(newUser.Identity, AdminIdentity).AdminId
            LoggedInIsInternal = CType(newUser.Identity, AdminIdentity).IsInternal
            LoggedInUsername = CType(newUser.Identity, AdminIdentity).Username

            CheckAllowPage()

            If IsPostBack AndAlso Not Request.Path.ToLower = "/admin/login.aspx" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "if(typeof extendSession=='function'){clearTimeout(loginTimer); clearTimeout(sessionTimer); sessionTimer = setTimeout(""extendSession()"", timeoutInterval);}", True)
            End If

        End Sub

        Public Sub CheckAllowPage()
            Dim checkUrl As String = Utility.ConfigData.GetPhysicalPageName(Utility.ConfigData.GlobalRefererName, Request.RawUrl.ToLower())
            Dim checkUrlAccess As String = Utility.ConfigData.CheckUrlAccess().ToLower()
            'If (Not checkUrl.Contains("/admin/default.aspx") And Not checkUrl.Contains("/admin/login.aspx") And Not checkUrl.Contains("/admin/menu.aspx") And Not checkUrl.Contains("/admin/top.aspx") And Not checkUrl.Contains("/admin/main.aspx") And Not checkUrl.Contains("/admin/unauthorized.aspx")) Then
            If (Not checkUrlAccess.Contains(checkUrl + ";")) Then
                checkUrl = Request.RawUrl.ToLower()
                Dim iCheck As Integer = checkUrl.IndexOf("?")
                Dim param As String() = Nothing
                If (iCheck > 0) Then
                    param = checkUrl.Substring(iCheck + 1).Split("&")
                    checkUrl = checkUrl.Substring(0, iCheck)
                End If

                Dim checkUrl2 As String = ""
                If (checkUrl.Contains("default.aspx")) Then
                    checkUrl2 = checkUrl.Replace("default.aspx", "")
                End If
                Dim lstmenu As MenuCollection = ListMenu()
                Dim Code As String = ""
                For Each item As MenuRow In lstmenu
                    If (Not String.IsNullOrEmpty(item.Href)) Then
                        Dim sLink As String = item.Href.ToLower()
                        Dim iCheckLink As Integer = sLink.IndexOf("?")
                        Dim ParamLink As String() = Nothing
                        If (iCheckLink > 0) Then
                            ParamLink = sLink.Substring(iCheckLink + 1).Split("&")
                            sLink = sLink.Substring(0, iCheckLink)
                        End If
                        Dim m1 As Integer = 0
                        Dim m2 As Integer = 0
                        If (Not param Is Nothing And Not ParamLink Is Nothing) Then
                            m1 = ParamLink.Length
                            For Each tmp As String In ParamLink
                                For Each tmp2 As String In param
                                    If (tmp = tmp2) Then
                                        m2 = m2 + 1
                                    End If
                                Next
                            Next
                        End If

                        If (m1 = m2 And (checkUrl = sLink Or checkUrl2 = sLink)) Then
                            CheckAccessAllowGroup(True)
                            Exit Sub
                        End If
                    End If
                Next
                CheckAccessAllowGroup(False)
            End If
        End Sub

        Protected Sub CheckAccessAllowGroup(ByVal AllowGroup As Boolean)
            If (Not AllowGroup) Then
                Response.Redirect("/admin/Unauthorized.aspx?page=" & Request.RawUrl)
            End If

            'CLEAR FrameURL FROM SESSION
            If Not LCase(Request.ServerVariables("URL")) = "/admin/default.aspx" Then
                Session("FrameURL") = String.Empty
            End If
        End Sub

        Public Sub WriteLogDetail(ByVal subject As String, ByVal objData As Object)
            Dim pageURL As String
            Try
                pageURL = Me.Request.RawUrl
            Catch ex As Exception

            End Try
            Dim logId As Integer = IIf(Session("LogId") Is Nothing, 0, Session("LogId"))
            Dim objLogDetail As New AdminLogDetailRow
            objLogDetail.LogId = logId
            objLogDetail.Message = Utility.Common.ObjectToString(objData)
            If String.IsNullOrEmpty(pageURL) Then
                objLogDetail.Subject = subject
            Else
                objLogDetail.Subject = subject & "(" & Me.Request.RawUrl & ")"
            End If

            objLogDetail.CreatedDate = Now
            AdminLogDetailRow.Insert(DB, objLogDetail)
        End Sub
       
        Public Sub WriteLogDetail(ByVal subject As String, ByVal addMessage As String, ByVal objData As Object)
            Dim pageURL As String
            Try
                pageURL = Me.Request.RawUrl
            Catch ex As Exception

            End Try
            Dim logId As Integer = IIf(Session("LogId") Is Nothing, 0, Session("LogId"))
            Dim objLogDetail As New AdminLogDetailRow
            objLogDetail.LogId = logId
            objLogDetail.Message = Utility.Common.ObjectToString(objData)
            If Not String.IsNullOrEmpty(addMessage) Then
                objLogDetail.Message = objLogDetail.Message & "," & addMessage
            End If
            If String.IsNullOrEmpty(pageURL) Then
                objLogDetail.Subject = subject
            Else
                objLogDetail.Subject = subject & "(" & Me.Request.RawUrl & ")"
            End If

            objLogDetail.CreatedDate = Now
            AdminLogDetailRow.Insert(DB, objLogDetail)
        End Sub
        Public Sub WriteLogDetail(ByVal subject As String, ByVal logMessage As String)
            Dim pageURL As String
            Try
                pageURL = Me.Request.RawUrl
            Catch ex As Exception

            End Try

            Dim logId As Integer = IIf(Session("LogId") Is Nothing, 0, Session("LogId"))
            Dim objLogDetail As New AdminLogDetailRow
            objLogDetail.LogId = logId
            objLogDetail.Message = logMessage
            If String.IsNullOrEmpty(pageURL) Then
                objLogDetail.Subject = subject
            Else
                objLogDetail.Subject = subject & "(" & Me.Request.RawUrl & ")"
            End If
            objLogDetail.CreatedDate = Now
            AdminLogDetailRow.Insert(DB, objLogDetail)
        End Sub
        Public Sub WriteLogDetail(ByVal objLogDetail As AdminLogDetailRow)
            Dim pageURL As String = String.Empty
            Try
                pageURL = Me.Request.RawUrl
            Catch ex As Exception

            End Try

            Dim logId As Integer = IIf(Session("LogId") Is Nothing, 0, Session("LogId"))
            objLogDetail.LogId = logId
            If Not String.IsNullOrEmpty(pageURL) Then

                objLogDetail.Subject = objLogDetail.Subject & "(" & Me.Request.RawUrl & ")"
            End If
            objLogDetail.CreatedDate = Now
            AdminLogDetailRow.Insert(DB, objLogDetail)
        End Sub
      
        Public Function ListMenu() As MenuCollection
            If Not Session("adminMenuList") Is Nothing Then
                Return DirectCast(Session("adminMenuList"), MenuCollection)
            End If
            Dim lstMenu As New MenuCollection
            Try
                Dim xmlPath As String = HttpContext.Current.Server.MapPath("~/admin/" + Utility.ConfigData.PathAdminMenu)
                Dim doc As New XmlDocument
                doc.Load(xmlPath)
                Dim root As XmlNode = doc.DocumentElement
                Dim html As String = ""
                Dim Id As Integer = 0
                Dim SectionAdded As Boolean = False
                'parent menu
                For i As Integer = 0 To root.SelectNodes("group").Count - 1
                    Dim ParentMenu As New MenuRow
                    Dim GroupNode As XmlNode = root.ChildNodes(i)
                    ParentMenu.MenuName = GroupNode.Attributes("name").Value.ToString()
                    ParentMenu.Level = 0
                    Id = Id + 1
                    ParentMenu.Id = Id
                    lstMenu.Add(ParentMenu)
                    'section menu
                    For j As Integer = 0 To GroupNode.SelectNodes("section").Count - 1
                        Id = Id + 1
                        Dim SectionMenu As New MenuRow
                        Dim SectionNode As XmlNode = GroupNode.ChildNodes(j)
                        SectionMenu.MenuName = SectionNode.Attributes("name").Value.ToString()
                        SectionMenu.Level = 1
                        SectionMenu.Id = Id
                        SectionMenu.ParentId = ParentMenu.Id
                        Try
                            SectionMenu.IsActive = CBool(SectionNode.Attributes("isActive").Value)
                        Catch ex As Exception
                            SectionMenu.IsActive = True
                        End Try
                        Try
                            SectionMenu.Href = SectionNode.Attributes("link").Value.ToString()
                        Catch ex As Exception

                        End Try
                        Try
                            Dim SectionAllowGroup As String = SectionNode.Attributes("allowGroup").Value
                            SectionMenu.AllowGroup = CheckAllowGroup(SectionAllowGroup)
                        Catch ex As Exception
                            SectionMenu.AllowGroup = False
                        End Try

                        If (SectionMenu.IsActive) Then
                            lstMenu.Add(SectionMenu)
                            Dim ItemAdded As Boolean = False
                            ''check last itemmenu
                            Dim bLastItem As Boolean = False
                            Dim last As Integer = SectionNode.SelectNodes("menu").Count - 1
                            While (Not bLastItem And last >= 0)
                                Dim LastItem As XmlNode = SectionNode.ChildNodes(last)
                                Dim lastHref As String = Nothing
                                Try
                                    lastHref = LastItem.Attributes("link").Value.ToString()
                                Catch ex As Exception
                                End Try
                                Dim bLastActive As Boolean = True
                                Try
                                    bLastActive = CBool(LastItem.Attributes("isActive").Value)
                                Catch ex As Exception
                                    bLastActive = True
                                End Try
                                ''check last item allow group
                                Dim bLastAllowGroup As Boolean = False
                                Try
                                    Dim tmp As String = LastItem.Attributes("allowGroup").Value
                                    bLastAllowGroup = CBool(CheckAllowGroup(tmp))
                                Catch ex As Exception
                                    bLastAllowGroup = False
                                End Try

                                If (bLastActive And bLastAllowGroup And Not String.IsNullOrEmpty(lastHref)) Then
                                    bLastItem = True
                                Else
                                    last = last - 1
                                End If
                            End While
                            ''set item menu
                            For n As Integer = 0 To SectionNode.SelectNodes("menu").Count - 1
                                Id = Id + 1
                                Dim MenuItem As XmlNode = SectionNode.ChildNodes(n)
                                Dim Menu As New MenuRow
                                Menu.Id = Id
                                Menu.ParentId = SectionMenu.Id
                                Try
                                    Menu.IsActive = CBool(MenuItem.Attributes("isActive").Value)
                                Catch ex As Exception
                                    Menu.IsActive = True
                                End Try
                                ''check item allow group
                                Try
                                    Dim tmp As String = MenuItem.Attributes("allowGroup").Value
                                    Menu.AllowGroup = CBool(CheckAllowGroup(tmp))
                                Catch ex As Exception
                                    Menu.AllowGroup = False
                                End Try
                                Try
                                    Menu.Href = MenuItem.Attributes("link").Value.ToString()
                                Catch ex As Exception

                                End Try
                                Try
                                    Menu.MenuName = MenuItem.Attributes("name").Value.ToString()
                                Catch ex As Exception
                                End Try
                                If (n = last) Then
                                    Menu.LastItem = True
                                End If
                                Menu.Level = 2

                                ''Check Item added to remove section and add menu to check link
                                If (Menu.IsActive And Not String.IsNullOrEmpty(Menu.Href) And Menu.AllowGroup) Then
                                    lstMenu.Add(Menu)
                                    ItemAdded = True
                                End If
                                If (Not ItemAdded And Not String.IsNullOrEmpty(SectionMenu.Href) And SectionMenu.AllowGroup) Then
                                    ItemAdded = True
                                End If
                                ''add child menu check link
                                If (ItemAdded) Then
                                    For t As Integer = 0 To MenuItem.SelectNodes("child").Count - 1
                                        Id = Id + 1
                                        Dim ChildItem As XmlNode = MenuItem.ChildNodes(t)
                                        Dim Child As New MenuRow
                                        Child.Id = Id
                                        Child.ParentId = Menu.Id
                                        Try
                                            Child.IsActive = CBool(ChildItem.Attributes("isActive").Value)
                                        Catch ex As Exception
                                            Child.IsActive = True
                                        End Try
                                        ''check child allow group
                                        Try
                                            Dim tmp As String = ChildItem.Attributes("allowGroup").Value
                                            Child.AllowGroup = CBool(CheckAllowGroup(tmp))
                                        Catch ex As Exception
                                            Child.AllowGroup = False
                                        End Try

                                        Child.Href = ChildItem.Attributes("link").Value.ToString()
                                        Child.Level = 3
                                        If (Child.IsActive And Child.AllowGroup And Not String.IsNullOrEmpty(Child.Href)) Then
                                            lstMenu.Add(Child)
                                        End If
                                    Next
                                End If

                            Next
                            If Not ItemAdded Then
                                If (Not String.IsNullOrEmpty(SectionMenu.Href) And SectionMenu.AllowGroup) Then
                                    SectionAdded = True
                                Else
                                    lstMenu.Remove(SectionMenu)
                                End If
                            Else
                                If (Not SectionAdded) Then
                                    SectionAdded = True
                                End If
                            End If

                        End If
                    Next
                    If (Not SectionAdded) Then
                        lstMenu.Remove(ParentMenu)
                    End If
                Next
                If (lstMenu.Count > 0) Then
                    Session("adminMenuList") = lstMenu
                End If
            Catch ex As Exception
            End Try
            Return lstMenu
        End Function

        Protected Function CheckAllowGroup(ByVal group As String) As Boolean
            Try
                Dim AdminId As Integer = Convert.ToInt32(Session("AdminId"))
                If (Not String.IsNullOrEmpty(group) And AdminId > 0) Then
                    Dim arrGroup() As String = group.Split(",")
                    Dim dt As DataTable
                    Dim count As Integer = 0
                    If Not Session("ListGroup") Is Nothing Then
                        dt = (DirectCast(Session("ListGroup"), DataTable))
                    Else
                        Dim ds As DataSet = AdminAdminGroupRow.LoadGroupsWithPrivileges(AdminId)
                        dt = ds.Tables(0)
                        If (dt.Rows.Count > 0) Then
                            Session("ListGroup") = dt
                        End If
                    End If
                    If (dt.Rows.Count > 0) Then
                        For i As Integer = 0 To dt.Rows.Count - 1
                            Dim item As String = dt.Rows(i)("Description").ToString()
                            For Each arr As String In arrGroup
                                If (Not String.IsNullOrEmpty(arr.Trim())) Then
                                    If (item.Trim().ToLower() = arr.Trim().ToLower()) Then
                                        Return True
                                    End If
                                End If
                            Next
                        Next
                    End If
                End If

            Catch ex As Exception

            End Try

            Return False
        End Function

    End Class
End Namespace