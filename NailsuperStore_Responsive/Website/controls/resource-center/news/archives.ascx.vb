Option Strict Off
Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class controls_NewsArchives
    Inherits ModuleControl

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadData()
        End If
    End Sub
    Private Sub LoadData()
        Dim cateID As Integer = 0
        If Request.QueryString("cateId") <> Nothing AndAlso IsNumeric(Request.QueryString("cateId")) Then
            cateID = CInt(Request.QueryString("cateId"))
        End If
        Dim dt As New DataTable

        If Request.RawUrl.Contains("/blog") Or cateID > 0 Then
            dt = DB.GetDataTable("Select CreatedDate From News ni left join NewsCategory nc on ni.NewsId = nc.NewsId Where ni.IsActive = 1 and nc.CategoryId = " & IIf(cateID = 0, Utility.ConfigData.BlogId, cateID) & " order by Year(CreatedDate) desc, Month(CreatedDate) asc")
        Else
            dt = DB.GetDataTable("Select CreatedDate From News ni left join NewsCategory nc on ni.NewsId = nc.NewsId inner join Category c on nc.CategoryId = c.CategoryId Where c.IsActive = 1 and ni.IsActive = 1 and nc.CategoryId <> " & Utility.ConfigData.BlogId & " order by Year(CreatedDate) desc, Month(CreatedDate) asc")
        End If
        Dim r As DataRow
        Dim d As DateTime
        Dim strMonth As String = ""
        Dim strYear As String = ""
        Dim strHtml As String = ""
        Dim arr As String = ""
        Dim month As String = ""
        Dim alink As String = ""
        Dim arrYear As String()
        Dim slink As String = ""
        Dim pathLink As String = ""
        Dim ckChecked = String.Empty

        Dim index As Integer = Request.RawUrl.LastIndexOf("/") + 1
        Dim selectmonth As String = Request.RawUrl.Substring(index, Request.RawUrl.Length - index)

        If Request.RawUrl.Contains("/blog") Or cateID = Utility.ConfigData.BlogId Then
            'slink = "<a href=""/blog/{0}"">{1}</a>"
            pathLink = "/blog"
        Else
            If cateID > 0 Then
                Dim cate As CategoryRow = CategoryRow.GetRow(DB, cateID)
                'slink = "<a href=""/news-topic/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(cate.CategoryName.ToLower())) & "/" & cateID & "/{0}"">{1}</a>"
                pathLink = "/news-topic/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(cate.CategoryName.ToLower())) & "/" & cateID
            Else
                'slink = "<a href=""/news-topic/{0}"">{1}</a>"
                pathLink = "/news-topic"
            End If
        End If
        slink = "<a href='" & pathLink & "/{0}'>{1}</a>"
        Dim k As Integer = 0
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                r = dt.Rows(i)
                d = CDate(r(0))
                month = MonthName(d.Month)
                If strHtml.Contains(d.Year.ToString) = False Then
                    strYear &= "-" & d.Year.ToString & ";"
                    strHtml &= IIf(k > 0, "</li></ul></li><li><div class='cate-title'><b class=""arrow-down""></b>" & d.Year.ToString & "</div><ul class='parent'>", "<li><div class='cate-title'><b class=""arrow-down""></b>" & d.Year.ToString & "</div><ul class='parent'>")
                    k = k + 1
                End If
                If strHtml.Contains(month & "-" & d.Year.ToString) = False Then
                    If selectmonth = (d.Year & d.Month) Then
                        ckChecked = "checked"
                    Else
                        ckChecked = String.Empty
                    End If
                    alink = String.Format(slink, d.Year.ToString & d.Month.ToString, month & "-" & d.Year.ToString)
                    strHtml &= "<li class='checkbox'><label for='chkYear" & d.Year.ToString & d.Month.ToString & "'><input type=""checkbox"" id='chkYear" & d.Year.ToString & d.Month.ToString & "'" & ckChecked & " onclick=""window.location='" & pathLink & "/" & d.Year.ToString & d.Month.ToString & "'"" /><span class='checkbox-icon'></span>" & alink & "</label></li>"
                End If
            Next
        End If
        arrYear = strYear.Split(";")
        If arrYear.Length > 0 Then
            For j As Integer = 0 To arrYear.Length - 1
                strHtml = Replace(strHtml, arrYear(j), "")
            Next
        End If

        ltDate.Text = strHtml & "</li>"
    End Sub


    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
