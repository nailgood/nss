Imports DataLayer
Imports Components
Imports System.Collections.Generic
Imports System.Linq

Partial Class controls_Footer
    Inherits ModuleControl

    Public ItemCollection As List(Of Product)
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildMainDepartment()

        Dim pageHasRecentlyView As String() = New String() {
            "/store/item.aspx", "/store/search-result.aspx", "/store/sub-category.aspx", "/store/collection.aspx"
        }
        If pageHasRecentlyView.Contains(Request.Path) Then
            getRecentlyView()
        End If
    End Sub
    Private Sub getRecentlyView()
        Try
            Dim recenltyViewItem = ViewedItemRow.GetRecentlyViewed(Utility.Common.GetOrderIdFromCartCookie(), Utility.Common.GetCurrentMemberId(), Session.SessionID, "inner")
            If recenltyViewItem.Count > 0 Then
                ItemCollection = SitePage.GetProductData(recenltyViewItem, 0, 1000, False, False, True)
            End If
        Catch ex As Exception
            ItemCollection = New List(Of Product)()
        End Try

    End Sub

    Private Sub BuildMainDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        Dim lnkDepartment As String = "<li><a href=""{0}"">{1}</a></li>"
        Dim html As String = String.Empty
        Try
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                If Not ds.Tables(0) Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    'Dim bBeginGroup As Boolean = True
                    'Dim iEndGroup = 5
                    'Dim arrClass() As String = New String() {"left", "mid", "right"}
                    'Dim iClass As Integer = 0
                    For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1
                        Dim dr As DataRow = ds.Tables(0).Rows(i)
                        Dim mainURL As String = URLParameters.MainDepartmentUrl(dr("URLCode"), CInt(dr("DepartmentId")))
                        'If bBeginGroup Then
                        '    html += String.Format("<ul class=""{0}"">", arrClass(iClass))
                        '    bBeginGroup = False
                        '    iClass = iClass + 1
                        'End If
                        html += String.Format(lnkDepartment, mainURL, dr("Name"))
                        'iEndGroup = iEndGroup - 1
                        'If (iEndGroup = 0 Or i = ds.Tables(0).Rows.Count - 2) Then
                        '    html += "</ul>"
                        '    iEndGroup = 5
                        '    bBeginGroup = True
                        'End If
                    Next
                End If
            End If
            ltrAltMainDepartment.Text = html
        Catch ex As Exception
            Email.SendError("ToError500", "footer.ascx _BuildMainDepartment-" & Me.Request.RawUrl, ex.ToString())
        Finally
            ds.Dispose()
        End Try
    End Sub

    Protected Sub btnSubscribe_Click(sender As Object, e As System.EventArgs) Handles btnSubscribe.Click
        Response.Redirect("/subscribe.aspx?e=" & txtEmail.Text)
    End Sub
End Class
