Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class controls_banner_infor_banner
    Inherits BaseControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub
    Private Sub LoadData()
        If Not Utility.ConfigData.IsEnableInforBanner() Then
            Exit Sub
        End If
        Dim uc As New UserControl
        Dim lstCollection As InforBannerCollection = InforBannerRow.GetMainPage(Utility.Common.InforBannerType.Main, 4)
        If Not lstCollection Is Nothing AndAlso lstCollection.Count > 0 Then
            For index As Integer = 0 To lstCollection.Count - 1
                Dim ctr As controls_banner_infor_banner_item
                ctr = uc.LoadControl("~/controls/banner/infor-banner-item.ascx")
                ctr.BannerItem = lstCollection(index)
                ctr.itemIndex = index
                phdItem.Controls.Add(ctr)
            Next
        Else
            Me.Visible = False
        End If
    End Sub
    

End Class
