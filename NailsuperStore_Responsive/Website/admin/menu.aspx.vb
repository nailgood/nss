Imports Components
Imports DataLayer
Imports System.Xml
Imports System.Collections.Generic

Partial Class MenuPage
    Inherits AdminPage

    Protected bSmartBug As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        bSmartBug = HasRights("USERS")
        lblMenu.Text = LoadMenu()
    End Sub

    Protected Function LoadMenu() As String
        Dim html As String = ""
        Dim m As New AdminMenu
        Dim lstmenu As MenuCollection = ListMenu()
        For Each item As MenuRow In lstmenu
            If (item.Level.Equals(0)) Then
                Dim tmp As String = BuildSection(lstmenu, m, item.Id)
                If (Not String.IsNullOrEmpty(tmp)) Then
                    html += m.WriteGroupText(item.MenuName) + tmp + m.WriteEndGroupText
                End If
            End If
        Next
        Return html
    End Function

    Protected Function BuildSection(ByVal lstMenu As MenuCollection, ByVal m As AdminMenu, ByVal Parentid As Integer) As String
        Dim html As String = ""
        For Each item As MenuRow In lstMenu
            If (item.Level.Equals(1) And item.ParentId = Parentid) Then
                Dim tmp As String = BuildItem(lstMenu, m, item.Id)
                If (Not String.IsNullOrEmpty(tmp)) Then
                    html += m.WriteRootText(item.MenuName) + tmp
                Else
                    If (Not String.IsNullOrEmpty(item.Href)) Then
                        html += m.WriteEmptyRootText(item.Href, item.MenuName)
                    End If
                End If
            End If
        Next
        Return html
    End Function

    Protected Function BuildItem(ByVal lstMenu As MenuCollection, ByVal m As AdminMenu, ByVal Parentid As Integer) As String
        Dim html As String = ""
        For Each item As MenuRow In lstMenu
            If (item.Level.Equals(2) And item.ParentId = Parentid) Then
                If (Not String.IsNullOrEmpty(item.Href)) Then
                    html += m.WriteLeafText(item.Href, item.MenuName, item.LastItem)
                End If
            End If
        Next
        Return html
    End Function
End Class
