Imports System
Imports System.Web.UI
Imports System.Text
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Globalization
Imports System.Web.UI.HtmlControls
Imports System.Configuration

Namespace Controls
    Public Class ExpDate
        Inherits System.Web.UI.WebControls.WebControl
        Implements System.Web.UI.IPostBackDataHandler

        Private _startyear As Integer = Year(Now)
        Private _endyear As Integer = Year(Now) + 10
        Private _month As Integer = 0
        Private _year As Integer = 0
        Private _date As String

        Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
			Dim m, y As String
            Dim context As HttpContext = HttpContext.Current

            m = context.Request(Me.UniqueID.ToString() & "_MONTH")
            y = context.Request(Me.UniqueID.ToString() & "_YEAR")
            If (m = String.Empty Or y = String.Empty) Then
                Value = ""
            Else
                Value = m & "/" & 1 & "/" & y
            End If
            Return False
        End Function

        Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent

        End Sub

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            Page.RegisterRequiresPostBack(Me)
            MyBase.OnInit(e)
        End Sub

        Public Property Value() As String
            Get
                Return _date
            End Get
            Set(ByVal value As String)
                Try
                    _date = value
                    Dim tmp As DateTime = DateTime.Parse(_date)
                    _month = tmp.Month
                    _year = tmp.Year
                Catch ex As Exception
                    _date = String.Empty
                    _month = 0
                    _year = 0
                End Try
            End Set
        End Property

        Public Property StartYear() As Integer
            Get
                Return _startyear
            End Get
            Set(ByVal value As Integer)
                _startyear = value
            End Set
        End Property

        Public Property EndYear() As Integer
            Get
                Return _endyear
            End Get
            Set(ByVal value As Integer)
                _endyear = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()
            Controls.Add(New System.Web.UI.LiteralControl("<table cellpadding=0 cellspacing=0 border=0><tr><td nowrap>"))
            CreateMonthList()
            Controls.Add(New System.Web.UI.LiteralControl("</td><td nowrap class=""lnpad5"">"))
            CreateYearList()
            Controls.Add(New System.Web.UI.LiteralControl("</td></tr></table>"))
        End Sub

        Private Sub CreateMonthList()
            Dim month As DropDownList = New DropDownList()
            month.EnableViewState = False
            month.ID = Me.ID.ToString() + "_MONTH"
            month.BorderColor = Me.BorderColor
			month.CssClass = Me.CssClass
            month.Attributes("style") = Me.Attributes("style")
            month.BorderStyle = Me.BorderStyle
            month.BorderWidth = Me.BorderWidth
            month.ForeColor = Me.ForeColor
            month.BackColor = Me.BackColor
            Controls.Remove(month)
            Controls.Add(month)
            month.Items.Add(New ListItem("", ""))
            For i As Integer = 0 To 11
                Dim j As Integer = i + 1
                Dim monthname As String = New DateTimeFormatInfo().MonthNames(i).ToString()
                Dim item As ListItem = New ListItem(monthname, j.ToString())
                If (_month = j) Then item.Selected = True
                month.Items.Add(item)
            Next
        End Sub

        Private Sub CreateYearList()
            Dim year As DropDownList = New DropDownList()
            year.EnableViewState = False
            year.ID = Me.ID.ToString() & "_YEAR"
            year.BorderColor = Me.BorderColor
            year.CssClass = Me.CssClass
            year.Attributes("style") = Me.Attributes("style")
            year.BorderStyle = Me.BorderStyle
            year.BorderWidth = Me.BorderWidth
            year.ForeColor = Me.ForeColor
            year.BackColor = Me.BackColor
            Controls.Remove(year)
            Controls.Add(year)
            year.Items.Add(New ListItem("", ""))
            For i As Integer = StartYear To EndYear
                Dim item As ListItem = New ListItem(i.ToString(), i.ToString())
                If (_year = i) Then item.Selected = True
                year.Items.Add(item)
            Next
        End Sub
    End Class
End Namespace