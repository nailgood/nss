Imports Components
Imports DataLayer
Imports XAVWebReference
Imports System.Net
Imports ShippingValidator
Imports System.Xml
Partial Class admin_ShipValid
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ltrResult.Text = Request.ServerVariables("REMOTE_ADDR")
        GetIPLocation()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ltrResult.Text = ""

        Dim xav As New Validator(txtName.Text.Trim().ToUpper(), txtSalonName.Text.Trim().ToUpper(), txtAddress.Text.Trim().ToUpper(), txtCity.Text.Trim().ToUpper(), txtState.Text.Trim().ToUpper(), txtZip.Text.Trim().ToUpper(), txtCountry.Text.Trim().ToUpper(), IIf(radFedEx.Checked, Validator.ShippingOption.FedEx, Validator.ShippingOption.UPS))
        

        If Not xav Is Nothing Then
            ltrResult.Text = xav.Msg
            If Not xav.CandidateList Is Nothing Then
                ltrResult.Text &= "<br><br>Candidate Length: " + xav.CandidateList.Count.ToString()

                Dim i As Int16 = 0
                For Each can As ShippingValidator.Candidate In xav.CandidateList
                    Dim strAddressLine As String = ""

                    strAddressLine = "<ul class=""item"">"
                    strAddressLine &= String.Format("<li>{0}</li>", can.Street)
                    strAddressLine &= "</ul>"

                    Dim strCan As String = String.Format("<div class=""can"">Code: {0}<br>Description: {1}<br>Street: {2}<br>City: {3}<br>State: {4}<br>Zip: {5}<br>All: {6}", _
                    can.Code, CType(can.Code, AddressType).ToString(), can.Street, can.City, can.State, can.ZipCode, can.All)

                    strCan &= "</div>"
                    ltrResult.Text &= strCan
                Next
            End If

            If xav.Msg.ToUpper().Contains("ERROR") Then
                Email.SendError("ToError500", "[Billing] Shipping Validator", xav.Msg)
            End If

            ltrResult.Text = ltrResult.Text.Replace(Environment.NewLine, "<br>")
        Else

            ltrResult.Text = "Nothing"
        End If



    End Sub

    Private Sub GetIPLocation()
        Dim ip As String = Request("ip")
        Dim city, state, countrycode, countryName, result As String
        ' IP = "113.161.73.102"
        ' Try
        Dim apiKey As String = Utility.ConfigData.IPInfo
        Dim webRequest As System.Net.HttpWebRequest = System.Net.WebRequest.Create("http://api.ipinfodb.com/v3/ip-city/?key=" & apiKey & "&ip=" & ip & "&format=xml")
        webRequest.Timeout = 10000
        Dim webResponse As System.Net.HttpWebResponse = webRequest.GetResponse()
        Dim reader1 As New XmlTextReader(webResponse.GetResponseStream())
        Dim xmlDoc As New XmlDocument()
        xmlDoc.Load(reader1)
        For i As Integer = 0 To xmlDoc.ChildNodes.Count - 1
            Dim node1 As XmlNode = xmlDoc.ChildNodes(i)
            If node1.Name = "Response" Then
                For j As Integer = 0 To node1.ChildNodes.Count - 1
                    Dim node2 As XmlNode = node1.ChildNodes(j)
                    If node2.Name = "cityName" Then
                        city = node2.InnerText
                    End If
                    If node2.Name = "regionName" Then
                        state = node2.InnerText
                    End If
                    If node2.Name = "countryCode" Then
                        countrycode = node2.InnerText
                    End If
                    If node2.Name = "countryName" Then
                        countryName = IIf(String.IsNullOrEmpty(node2.InnerText) Or node2.InnerText = "", "", " (" & node2.InnerText & ")")
                    End If
                Next
            End If

        Next
        result = city & IIf(String.IsNullOrEmpty(city), state, ", " & state) & countryName
        result = Utility.Common.StringReplace(result, Utility.ConfigData.TextToFind, Utility.ConfigData.TextToReplace)
        'Catch ex As Exception
        '    Components.Email.SendError("ToError500", "GetCityLocation1-" & Now.ToString(), System.Web.HttpContext.Current.Request.RawUrl & "<br>Error =" & ex.ToString())
        'End Try
        lblocation.Text = result
    End Sub
End Class
