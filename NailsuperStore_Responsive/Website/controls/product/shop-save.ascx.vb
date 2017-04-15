Imports DataLayer
Imports Components
Imports Utility.Common
Partial Class controls_ShopSave
    Inherits BaseControl
    Public title As String = String.Empty
    Public Type As Integer = 0
    Public strclass As String = String.Empty
    Public lstCollection As New ShopSaveCollection
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            LoadData()

        End If
    End Sub
    Private Sub LoadData()
        If Type = ShopSaveType.ShopYourWay Then
            title = SysParam.GetValue("ShopExploreOurStore")
        ElseIf Type = ShopSaveType.TopCategory Then
            title = SysParam.GetValue("TopRecommendedProducts")
            strclass = " topcategory"
        End If
        Dim uc As New UserControl
        lstCollection = ShopSaveRow.ListByType(DB, Type, Convert.ToInt32(True))

        If Not lstCollection Is Nothing AndAlso lstCollection.Count > 0 Then
            For i As Integer = 0 To lstCollection.Count - 1
                ulSlideContent.ID = "ulSlideContent_" & Guid.NewGuid().ToString()
                ulSlideContent.ClientIDMode = ClientIDMode.Static
                '' lstCollection(i).HomeBanner = "elegant-gel-acrylic-nail-brush-holder-10slo.jpg"
                Dim htmlControl As String = RenderReviewControl(lstCollection(i), i)
                ltrData.Text = ltrData.Text & htmlControl
                If (i > 0 AndAlso i Mod 2 <> 0) Then
                    ltrData.Text = ltrData.Text & "<div class='clearfix visible-sm'></div>"
                End If
                ulSlideContent.InnerHtml = ulSlideContent.InnerHtml & htmlControl
            Next
        End If

        '' ulSlideContent.InnerHtml = Utility.Common.ExportHTMLControl(phdItem)
    End Sub
    Public Shared Function RenderReviewControl(ByVal si As ShopSaveRow, ByVal itemIndex As Integer) As String
        Dim controlPath As String = "~/controls/product/shop-save-item.ascx"
        Dim pageHolder As New Page()
        Dim viewControl As controls_product_shop_save_item = DirectCast(pageHolder.LoadControl(controlPath), controls_product_shop_save_item)
        viewControl.itemIndex = itemIndex
        viewControl.ShopSaveItem = si
        pageHolder.Controls.Add(viewControl)
        Dim output As New System.IO.StringWriter()
        HttpContext.Current.Server.Execute(pageHolder, output, False)
        Return output.ToString()
    End Function


End Class
