Imports Components
Imports DataLayer
Imports System.Net
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices
Imports System.Data.Common
Imports ShippingValidator
Partial Class admin_api_USPS
    Inherits SitePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim RateDB As Double = BaseShoppingCart.GetUSPSRate(txtWeight.Text, txtCountry.Text, 0, 0)
        Dim FinalRate As Double = RateDB + (RateDB * CDbl(SysParam.GetValue("ExtraUSPSPercent"))) / 100
        ltrResult1.Text = "USPS Rate: " & RateDB _
        & "<br> Final Rate (10%): " & Math.Round(FinalRate, 2)
     
    End Sub
End Class
