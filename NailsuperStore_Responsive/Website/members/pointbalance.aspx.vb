Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class members_pointbalance
    Inherits SitePage
    Public m_PointAvailable As Integer
    Public m_MoneyAvailable As String
    Public m_PointPending As Integer
    Public m_PointInMonth As Integer
    Public m_PointInYear As Integer
    Public countTrans As Integer
    Public m_PointsEarnedUpToDate As Integer
    Private xslabel As String = "<span class=""xs-label bold pull-left"">{0}:</span>"


    Public Property IsPopUp() As Boolean
        Get
            Dim value As String = Request.QueryString("isPopUp")
            Dim valueBol As Boolean = False
            If Boolean.TryParse(value, valueBol) Then
                Return valueBol
            End If
            Return False
        End Get
        Set(ByVal value As Boolean)
            Request.QueryString("isPopUp") = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HasAccess() And Not IsPopUp Then
            Response.Redirect("/members/login.aspx?act=checkout")
        End If
        If Not Page.IsPostBack Then
            LoadData()
        End If
    End Sub
    Public Sub LoadData()

        Dim lstTrans As CashPointCollection = Nothing
        Dim dt As DataTable = CashPointRow.GetTotalCashPointAndDetailByMember(DB, Utility.Common.GetCurrentMemberId(), Not IsPopUp, lstTrans)

        If dt IsNot Nothing Then
            Dim drDetail As DataRow = dt.Rows(0)

            m_PointAvailable = Convert.ToInt32(drDetail("CurrentTotalPoint"))
            If m_PointAvailable > 0 Then
                Dim MoneyEachPoint As Double = CDbl(SysParam.GetValue("MoneyEachPoint"))
                Dim money As Double = m_PointAvailable * MoneyEachPoint
                If money >= 1 Then
                    m_MoneyAvailable = String.Format("{0:#.##}", money)
                ElseIf (money > 0) Then
                    m_MoneyAvailable = String.Format("{0:#,##0.00;(#,##0.00);Nothing}", money)
                End If
            End If

            If drDetail("PendingPoint") Is DBNull.Value Then
                m_PointPending = 0
            Else
                m_PointPending = Convert.ToInt32(drDetail("PendingPoint"))
            End If

            If drDetail("PointInMonth") Is DBNull.Value Then
                m_PointInMonth = 0
            Else
                m_PointInMonth = Convert.ToInt32(drDetail("PointInMonth"))
            End If

            If drDetail("PointInYear") Is DBNull.Value Then
                m_PointInYear = 0
            Else
                m_PointInYear = Convert.ToInt32(drDetail("PointInYear"))
            End If

            If drDetail("PointsEarnedUpToDate") Is DBNull.Value Then
                m_PointsEarnedUpToDate = 0
            Else
                m_PointsEarnedUpToDate = Convert.ToInt32(drDetail("PointsEarnedUpToDate"))
            End If

            If Not IsPopUp And lstTrans IsNot Nothing Then
                countTrans = lstTrans.Count
                rptTrans.DataSource = lstTrans
                rptTrans.DataBind()
            End If
        End If

    End Sub
    Protected Sub rptTrans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptTrans.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrAmount As Literal = CType(e.Item.FindControl("ltrAmount"), Literal)
            Dim ltrnotes As Literal = CType(e.Item.FindControl("ltrnotes"), Literal)
            Dim objData As CashPointRow = e.Item.DataItem
            If (objData.Amount > 0) Then
                ltrAmount.Text = String.Format(xslabel, "Amount") & FormatCurrency(objData.Amount)
            Else
                ltrAmount.Text = ""
            End If

            Dim ltrPointDebit As Literal = CType(e.Item.FindControl("ltrPointDebit"), Literal)
            If (objData.PointDebit > 0) Then
                ltrPointDebit.Text = String.Format(xslabel, "Point Debit") & " -" & SitePage.NumberToString(objData.PointDebit)
            Else
                ltrPointDebit.Text = ""
            End If
            Dim ltrPointEarn As Literal = CType(e.Item.FindControl("ltrPointEarn"), Literal)
            If (objData.PointEarned > 0) Then
                ltrPointEarn.Text = String.Format(xslabel, "Point Earn") & SitePage.NumberToString(objData.PointEarned)
            Else
                ltrPointEarn.Text = ""
            End If
            'Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
            Dim pointType As Integer = SitePage.CashPointType(objData.TransactionNo)
            Dim link As String = "<a href=""{0}"">{1}</a>"
            If (pointType = 3) Then
                link = String.Format(link, "/members/orderhistory/view.aspx?OrderId=" & objData.OrderId, "#" & objData.TransactionNo)
                ltrnotes.Text = objData.Notes.Replace("#" & objData.TransactionNo, link)
            ElseIf (pointType = 2) Then ''return item
                link = String.Format(link, "/members/creditmemo/view.aspx?MemoId=" & SalesCreditMemoHeaderRow.SalesCreditMemoHeaderIDByNo(DB, objData.TransactionNo), "#" & objData.TransactionNo)
                ltrnotes.Text = objData.Notes.Replace("#" & objData.TransactionNo, link)
            ElseIf (pointType = 1) Then ''review product
                Dim urlCode As String = StoreItemRow.GetRowURLBySKU(objData.TransactionNo.Substring(2, objData.TransactionNo.Length - 2))  'StoreItemRow.GetRowURLCodeBySKU(objData.TransactionNo.Substring(2, objData.TransactionNo.Length - 2))
                ltrnotes.Text = String.Format(link, "/nail-products/" & urlCode, objData.Notes)
                ltrnotes.Text = ltrnotes.Text.Replace("Product Review-", "Product Review ")
            Else
                ltrnotes.Text = objData.Notes.Replace("Video Review-", "Video Review ")
            End If
            'If objData.TransactionNo <> "" Then
            '    ltrLink.Text = "<a href='" & link & "'> " & objData.TransactionNo & "</a>"
            'End If

        End If

    End Sub

End Class
