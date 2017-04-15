Imports Components
Imports DataLayer
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing
Imports System.Collections.Generic

Public Class DataInfo
    Private m_Day As DateTime
    Private m_Total As Double

    Public Property Day() As DateTime
        Get
            Return m_Day
        End Get
        Set(ByVal value As DateTime)
            m_Day = value
        End Set
    End Property

    Public Property Total() As Double
        Get
            Return m_Total
        End Get
        Set(ByVal value As Double)
            m_Total = value
        End Set
    End Property
End Class

Partial Class admin_report_order_default
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            dtpFromDate.Text = DateTime.Now.Subtract(New TimeSpan(7, 0, 0, 0)).ToShortDateString
            dtpToDate.Text = DateTime.Now.ToShortDateString
            BindList()
        End If
    End Sub
    Private Sub BindList()
        If Not String.IsNullOrEmpty(dtpFromDate.Text) Then
            If String.IsNullOrEmpty(dtpToDate.Text) Then
                dtpToDate.Text = DateTime.Now.ToShortDateString
            End If
            Dim sumOrder As Integer = 0
            Dim Total As Double = 0
            Dim sumWeb As Integer = 0
            Dim sumMobile As Integer = 0
            Dim sumAmazon As Integer = 0
            Dim sumEbay As Integer = 0
            Dim data As DataTable = StoreOrderRow.ReportOrderByDay(dtpFromDate.Text, dtpToDate.Text, sumOrder, Total, sumWeb, sumMobile, sumAmazon, sumEbay)

            ltrTotal.Text = String.Format("{0:C}", Total)
            ltrOrder.Text = sumOrder
            ltrOrderWeb.Text = sumWeb
            ltrOrderMobile.Text = sumMobile
            ltrOrderAmazon.Text = sumAmazon
            ltrOrderEbay.Text = sumEbay

            rptReportList.DataSource = data
            rptReportList.DataBind()
            If (Not data Is Nothing) Then
                DrawChart(data)
            End If
        End If


    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindList()
    End Sub

    Private Sub DrawChart(ByVal dt As DataTable)
        Dim lstDataInfo As New List(Of DataInfo)
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim data As New DataInfo
            data.Day = Convert.ToDateTime(dt.Rows(i)("Date"))
            data.Total = Utility.Common.RoundCurrency(Convert.ToDouble(dt.Rows(i)("Total")))
            lstDataInfo.Add(data)
        Next
        lstDataInfo.Reverse()
        ChartReport.DataSource = lstDataInfo
        ChartReport.DataBind()

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Dim area As ChartArea = ChartReport.ChartAreas(0)
        area.AxisX.IsMarginVisible = False
        area.AxisX.MajorGrid.Enabled = False
        area.AxisX.LabelStyle.Format = "{0:MMM d}"

        area.Area3DStyle.Enable3D = False
        area.AxisY.LabelStyle.Format = "C"
        area.AxisY.MajorGrid.LineColor = Color.LightGray
        area.AxisY.LineDashStyle = ChartDashStyle.NotSet


        Dim series As Series = ChartReport.Series(0)
        series.XValueMember = "Day"
        series.XValueType = ChartValueType.DateTimeOffset
        series("ShowMarkerLines") = "True"
        series.YValueMembers = "Total"
        series.MarkerStyle = MarkerStyle.Circle

        Dim showValue As Boolean = True
        If Not String.IsNullOrEmpty(dtpFromDate.Text) Then
            Dim dStart As DateTime = Convert.ToDateTime(dtpFromDate.Text)
            Dim dEnd As DateTime = DateTime.Now
            If Not String.IsNullOrEmpty(dtpToDate.Text) Then
                dEnd = Convert.ToDateTime(dtpToDate.Text)
            End If
            If dEnd.Subtract(dStart).Days > 31 Then
                showValue = False
            End If
        End If
        If showValue Then
            series.ToolTip = "Date: #VALX{d}"
        Else
            series.ToolTip = "Date: #VALX{d}" & Environment.NewLine & "Total: #VALY{C}"
        End If
        series.IsValueShownAsLabel = showValue
        series.ChartType = SeriesChartType.Area
        series("LabelStyle") = "Top"
    End Sub

End Class
