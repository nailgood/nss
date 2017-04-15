Option Strict Off
Imports Components
Imports DataLayer
Imports System.Data

Partial Class controls_StripBanner
    Inherits ModuleControl
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public BannerId As Integer = 0
    '-------------------------------------------------------------------
    ' VIEWSTATE
    '-------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim dt As New DataTable
        Dim DepartmentId As Integer
        Dim path As String = HttpContext.Current.Request.Path
        If (BannerId > 0) Then
            dt = DB.GetDataTable("SELECT Image,MobileImage,MainTitle,TextHtml,SubTitle,LinkPage FROM PromotionSalesprice WHERE Id = " & BannerId & " AND IsActive = 1 AND GETDATE() BETWEEN StartingDate AND EndingDate")
        Else
            If Request.QueryString("DepartmentId") <> Nothing AndAlso IsNumeric(Request.QueryString("DepartmentId")) Then
                DepartmentId = CInt(Request.QueryString("DepartmentId"))
                dt = PromotionSalespriceRow.GetListPromotionSalesprices(DepartmentId, 2)

            ElseIf LCase(path) = "/home.aspx" Then
                dt = DB.GetDataTable("SELECT TOP 1 Image,MobileImage,MainTitle,TextHtml,SubTitle,LinkPage FROM PromotionSalesprice WHERE type = 0 AND IsActive = 1 AND GETDATE() BETWEEN StartingDate AND EndingDate ORDER BY Id DESC") ''StorePromotionRow.GetListStorePromotion("SELECT TOP 1 * FROM PromotionSalesprice WHERE type = 0 AND IsActive = 1 AND GETDATE() BETWEEN StartingDate AND EndingDate ORDER BY Id DESC")
            End If
        End If
        

        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            Dim mobileImage As String = String.Empty
            Dim Image As String = String.Empty
            Dim MainTitle As String = String.Empty
            If IsDBNull(dr("Image")) = False Then
                Image = dr("Image")
            End If
            If IsDBNull(dr("MobileImage")) = False Then
                mobileImage = dr("MobileImage")
            End If
            If IsDBNull(dr("MainTitle")) = False Then
                MainTitle = dr("MainTitle")
            End If

            If (Not String.IsNullOrEmpty(Image) Or Not Not String.IsNullOrEmpty(mobileImage)) Then
                If (Not String.IsNullOrEmpty(mobileImage)) Then
                    litMobilePromotion.Text = String.Format("<img src=""" & Utility.ConfigData.PathPromotionMobile & "{0}"" style=""border:none"" alt=""{1}"" />", mobileImage, MainTitle)
                End If
                If (Not String.IsNullOrEmpty(Image)) Then
                    litPromotion.Text = String.Format("<img src=""" & Utility.ConfigData.CDNMediaPath & "/assets/salesprice/{0}"" style=""border:none"" alt=""{1}"" />", Image, MainTitle)
                End If
                If Not String.IsNullOrEmpty(litPromotion.Text) Then
                    litPromotion.Text = String.Format("<a href=""{0}"" style=""text-decoration:none"">{1}</a>", dr("LinkPage"), litPromotion.Text)
                End If
            ElseIf IsDBNull(dr("TextHtml")) = False And dr("TextHtml").ToString <> "<p>&#160;</p>" Then
                litPromotion.Text = dr("TextHtml").ToString().Trim()
            Else
                litPromotion.Text = "<span>" & dr("SubTitle").ToString & "</span <span>" & dr("MainTitle").ToString & "</span>"
                litPromotion.Text = String.Format("<a href=""{0}"" style=""text-decoration:none"">{1}</a>", dr("LinkPage"), litPromotion.Text)
            End If
        End If

    End Sub
End Class
