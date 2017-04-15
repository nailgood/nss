Imports Components
Imports DataLayer
Imports System.Net
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices
Imports System.Data.Common
Imports ShippingValidator
Partial Class admin_fedex
    Inherits SitePage
    Private ResidentalFee As Double = 0
    Private SignatureFee As Double = 0
    Private InsuranceFee As Double = 0
    Private ExtraFedExPercent As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If chkInsurance.Checked Then
        '    tr2.Visible = True
        'Else
        '    tr2.Visible = False
        'End If

    End Sub

    Protected Sub chkResidentialAddress_Change(ByVal sender As Object, ByVal e As System.EventArgs)
        chkResidentialFee.Visible = chkResidentialAddress.Checked
        If Not chkResidentialAddress.Checked Then
            chkResidentialFee.Checked = False
        End If

    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.Remove("GetFedExRate")

        GetShippingFedex()
        GetShippingUPS()
        Session.Remove("GetFedExRate")
    End Sub
    Private Sub GetShippingFedex()
        GetRelateFeeShip(drpShippingType.SelectedValue)
        If chkInsurance.Checked Then
            ' tr1.Visible = True
            ' tr2.Visible = True
            Try
                If txtSubTotal.Text / 100 > CDbl(SysParam.GetValue("FedExMinimumInsurance")) / InsuranceFee Then
                    InsuranceFee = InsuranceFee * Math.Ceiling(txtSubTotal.Text / 100)
                Else
                    InsuranceFee = CDbl(SysParam.GetValue("FedExMinimumInsurance"))
                End If

                lbTotal.Text = txtSubTotal.Text + ResidentalFee + SignatureFee + InsuranceFee
            Catch ex As Exception

            End Try

        Else
            'ltrResult.Text = ""
            'ltrResult1.Text = ""
            InsuranceFee = 0
        End If

        If chkResidentialFee.Checked = False Then
            ResidentalFee = 0
        End If

        If chkSignature.Checked = False Then
            SignatureFee = 0
        End If

        Dim getRate As New FedexRate(txtZip.Text, txtWeight.Text, drpShippingType.SelectedValue, txtCountry.Text)
        Dim zone As Integer = getRate.Zone

        'FedEx Service
        Dim SV As Double = 0
        Dim DASSV As Double = 0
        Dim FuelSV As Double = 0
        Dim FreightDiscount As Double = 0
        Dim WebsiteSV As Double = 0
        Dim SubTotalSV As Double = 0
        Dim TotalSV As Double = 0
        SV = getRate.Rate

        If SV > 0 Then
            DASSV = Math.Round(getRate.FeeDAS, 2)
            'DASSV = ShipmentMethod.GetValue(drpShippingType.SelectedValue, IIf(chkResidential.Checked, Utility.Common.ShipmentValue.DASResidential, Utility.Common.ShipmentValue.DASCommercial))
            SV += DASSV

            FreightDiscount = Math.Round(getRate.TotalFreightDiscount, 2)
            SV -= FreightDiscount

            FuelSV = Math.Round(getRate.FeeFuel, 2)
            'FuelSV = ((SV * CInt(ShipmentMethod.GetValue(drpShippingType.SelectedValue, Utility.Common.ShipmentValue.FuelRate))) / 100)
            SV += FuelSV

            SV = Math.Round(SV, 2)
            SV += ((SV * ExtraFedExPercent) / 100)
            WebsiteSV = SV
            SubTotalSV = SV + SignatureFee + InsuranceFee
            TotalSV = SubTotalSV + ResidentalFee
        End If

        Dim RateDB As Double = BaseShoppingCart.GetFedExRate(DB, txtWeight.Text, drpShippingType.SelectedValue, txtZip.Text, txtCountry.Text, chkResidentialAddress.Checked)
        Dim WebsiteDB As Double = RateDB + (RateDB * ExtraFedExPercent) / 100
        Dim SubTotalDB As Double = WebsiteDB + SignatureFee + InsuranceFee
        Dim TotalDB As Double = SubTotalDB + ResidentalFee
        ltrResult.Text = "<div><B>Web service</B> </br>Zone: " & getRate.Zone _
        & "<br> Fedex Rate: " & getRate.Rate _
        & String.Format("<br>DAS : {0}", DASSV) _
        & String.Format("<br> Discount: {0}", FreightDiscount) _
        & String.Format("<br>Fuel Rate ({0}%): {1}", Math.Round(CInt(ShipmentMethod.GetValue(drpShippingType.SelectedValue, Utility.Common.ShipmentValue.FuelRate))), FuelSV) _
        & IIf(ExtraFedExPercent > 0, "<br> Website Rate (10%): " & Math.Round(WebsiteSV, 2), "") _
        & "<br>Signature Fee: " & SignatureFee _
        & "<br>Insurance Fee: " & InsuranceFee _
        & "<br>>> SubTotal Shipping: " & Math.Round(SubTotalSV, 2) _
        & "<br>----------------------" _
        & "<br>Residental Fee: " & ResidentalFee _
        & "<br>>> Total Shipping: " & Math.Round(TotalSV, 2) _
        & "</div><div><br><B>Database</B><br>Zone: " & BaseShoppingCart.Zone _
        & "<br>Database Rate (including " & IIf(getRate.FeeDAS > 0, "DAS,", "") & " Fuel): " & IIf(BaseShoppingCart.Zone = 0, 0, RateDB) _
        & IIf(ExtraFedExPercent > 0, "<br>Website Rate (10%): " & IIf(BaseShoppingCart.Zone = 0, 0, Math.Round(WebsiteDB, 2)), "") _
        & "<br>Signature Fee: " & SignatureFee _
        & "<br>Insurance Fee: " & InsuranceFee _
        & "<br>>> SubTotal Shipping: " & Math.Round(SubTotalDB, 2) _
        & "<br>----------------------" _
        & "<br>Residental Fee: " & ResidentalFee _
        & "<br>>>Total Shipping: " & IIf(BaseShoppingCart.Zone = 0, 0, Math.Round(TotalDB, 2))

    End Sub
    Private Sub GetRelateFeeShip(ByVal CarrierType As Integer)
        Dim SP As String = "sp_GetRelateFeeShip"
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        Dim cmd As DbCommand = db.GetStoredProcCommand(SP)

        db.AddInParameter(cmd, "MethodId", DbType.Int16, CarrierType)
        db.AddOutParameter(cmd, "Residential", DbType.Double, 0)
        db.AddOutParameter(cmd, "Signature", DbType.Double, 0)
        db.AddOutParameter(cmd, "Insurance", DbType.Double, 100)
        db.AddOutParameter(cmd, "ExtraShippingPercent", DbType.Int32, 0)
        db.ExecuteNonQuery(cmd)
        Try
            ResidentalFee = CDbl(cmd.Parameters("@Residential").Value) 'CDbl(db.ExecuteScalar(cmd))
            SignatureFee = CDbl(cmd.Parameters("@Signature").Value)
            InsuranceFee = CDbl(cmd.Parameters("@Insurance").Value)
            ExtraFedExPercent = CDbl(cmd.Parameters("@ExtraShippingPercent").Value)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GetShippingUPS()
        Dim CarrierType As Integer
        Select Case drpShippingType.SelectedValue
            Case 16
                CarrierType = 14
            Case 17
                CarrierType = 2
            Case 18
                CarrierType = 3
        End Select
        GetRelateFeeShip(CarrierType)
        If chkInsurance.Checked Then
            ' tr1.Visible = True
            ' tr2.Visible = True
            Try
                InsuranceFee = InsuranceFee * Math.Ceiling(txtSubTotal.Text / 100)
                lbTotal.Text = txtSubTotal.Text + ResidentalFee + SignatureFee + InsuranceFee
            Catch ex As Exception

            End Try
        Else
            'ltrResult.Text = ""
            'ltrResult1.Text = ""
            InsuranceFee = 0
        End If
        If chkResidentialFee.Checked = False Then
            ResidentalFee = 0
        End If
        If chkSignature.Checked = False Then
            SignatureFee = 0
        End If
        Dim Shipping As Double = DB.ExecuteScalar("select top 1 coalesce(case when " & txtWeight.Text & " < overundervalue then firstpoundunder else firstpoundover end + case when additionalthreshold = 0 then " & IIf(Math.Ceiling(txtWeight.Text - 1) < 1, 0, Math.Ceiling(txtWeight.Text - 1)) & " * additionalpound else case when " & txtWeight.Text & " - additionalthreshold > 0 then (" & txtWeight.Text & " - additionalthreshold) * additionalpound else 0 end end, 0) from shipmentmethod sm inner join shippingrange sr on sm.methodid = sr.methodid where " & DB.Quote(txtZip.Text) & " between lowvalue and highvalue and sm.methodid = " & CarrierType)
        Dim TotalShipping As Double = Shipping + ResidentalFee + SignatureFee + InsuranceFee
        ltrResult1.Text = "<div>UPS Rate: " & Shipping _
        & "<br>Residential Fee: " & ResidentalFee _
        & "<br>Signature Fee: " & SignatureFee _
        & "<br>Insurance Fee: " & InsuranceFee _
        & "<br>>>Total Shipping: " & TotalShipping _
        & "</div>"
    End Sub
End Class
