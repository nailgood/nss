Imports System.Xml
Imports System.Net
Imports System.io
Imports System.Text
Imports System.Threading
Imports Utility
Imports DataLayer
Public Class UPS

	Private m_FromCity As String = "Grand Rapids"
	Private m_FromState As String = "MI"
	Private m_FromZip As String = "49534"
	Private m_ErrorMsg As String = ""
	Private Shared XMLRateString As String = ""
	Public Shared allDone As New ManualResetEvent(False)

	Public Const RATE_GROUND As String = "03"
	Public Const RATE_SECOND_DAY As String = "02"
	Public Const RATE_NEXT_DAY As String = "01"
	Public Const RATE_STANDARD As String = "11"
	Public Const RATE_3DAY As String = "12"
	Public Const RATE_NEXT_DAY_AIR_SAVER As String = "13"
	Public Const RATE_NEXT_DAY_EARLY_AM As String = "14"
	Public Const RATE_WORLDWIDE_EXPRESS As String = "07"
	Public Const RATE_WORLDWIDE_EXPRESS_PLUS As String = "54"
	Public Const RATE_2DAY_AIR_AM As String = "59"
	Public Const RATE_WORLDWIDE_EXPEDITED As String = "08"

	Public ReadOnly Property ErrorMsg() As String
		Get
			Return m_ErrorMsg
		End Get
	End Property

	Public Sub New(ByVal FromCity As String, ByVal FromState As String, ByVal FromZip As String)
		m_FromCity = FromCity
		m_FromState = FromState
		m_FromZip = FromZip
	End Sub

	Public Sub New()
	End Sub

    Function UPSValidateAddress(ByVal ToCity As String, ByVal ToState As String, ByVal ToZip As String, ByVal ToCountry As String, ByRef ErrorDesc As String, ByVal MaxTimeWait As Integer) As Boolean
        If ConfigData.GlobalRefererName.Contains("localhost") Or ConfigData.GlobalRefererName.Contains("192.168.41") Then
            Return True
        End If

        Components.Email.SendError("ToError500", "[Tracking] UPSValidateAddress", System.Web.HttpContext.Current.Request.RawUrl() & "<br>ToCity: " & ToCity & "<br>ToState: " & ToState & "<br>ToZip: " & ToCountry & "<br>MaxTimeWait: " & MaxTimeWait)
        Dim doc As XmlDocument = New XmlDocument
        Dim AccessRequest, License, UserId, Password As XmlElement

        AccessRequest = doc.CreateElement("AccessRequest")
        AccessRequest.SetAttribute("xml:lang", "en-US")

        License = doc.CreateNode("element", "AccessLicenseNumber", "")
        License.InnerText = SysParam.GetValue("UPSLicenseNo")
        AccessRequest.AppendChild(License)

        UserId = doc.CreateNode("element", "UserId", "")
        UserId.InnerText = SysParam.GetValue("UPSUserId")
        AccessRequest.AppendChild(UserId)

        Password = doc.CreateNode("element", "Password", "")
        Password.InnerText = SysParam.GetValue("UPSPassword")
        AccessRequest.AppendChild(Password)
        doc.AppendChild(AccessRequest)

        Dim addr As XmlDocument = New XmlDocument
        Dim RequestType, RequestNode, Reference, Customer As XmlElement
        Dim Version, RequestAction, Address, PostalCode, CountryCode As XmlElement
        Dim City, State As XmlElement

        RequestType = addr.CreateElement("AddressValidationRequest")
        RequestType.SetAttribute("xml:lang", "en-US")
        RequestNode = addr.CreateNode("element", "Request", "")
        Reference = addr.CreateNode("element", "TransactionReference", "")
        Customer = addr.CreateNode("element", "CustomerContext", "")
        Customer.InnerText = "Customer Data"
        Reference.AppendChild(Customer)

        Version = addr.CreateNode("element", "XpciVersion", "")
        Version.SetAttribute("version", "1.0001")
        Reference.AppendChild(Version)
        RequestNode.AppendChild(Reference)

        RequestAction = addr.CreateNode("element", "RequestAction", "")
        RequestAction.InnerText = "AV"
        RequestNode.AppendChild(RequestAction)
        RequestType.AppendChild(RequestNode)

        Address = addr.CreateNode("element", "Address", "")
        City = addr.CreateNode("element", "City", "")
        City.InnerText = ToCity
        Address.AppendChild(City)

        State = addr.CreateNode("element", "StateProvinceCode", "")
        State.InnerText = ToState
        Address.AppendChild(State)

        PostalCode = addr.CreateNode("element", "PostalCode", "")
        PostalCode.InnerText = ToZip
        Address.AppendChild(PostalCode)

        CountryCode = addr.CreateNode("element", "CountryCode", "")
        CountryCode.InnerText = ToCountry
        Address.AppendChild(CountryCode)
        RequestType.AppendChild(Address)

        addr.AppendChild(RequestType)

        Dim Result As String = ""
        Try
            Dim XMLString As String = "<?xml version=""1.0""?>" & doc.InnerXml & "<?xml version=""1.0""?>" & addr.InnerXml
            Dim r As HttpWebRequest = CType(WebRequest.Create("https://www.ups.com/ups.app/xml/AV"), HttpWebRequest)
            r.Timeout = MaxTimeWait
            Dim encodedData As ASCIIEncoding = New ASCIIEncoding
            Dim byteArray() As Byte = encodedData.GetBytes(XMLString)

            r.Method = "POST"
            r.ContentType = "application/x-www-form-urlencoded"
            r.ContentLength = XMLString.Length()
            r.KeepAlive = False

            Dim SendStream As Stream = r.GetRequestStream
            SendStream.Write(byteArray, 0, byteArray.Length)
            SendStream.Close()

            Dim wr As HttpWebResponse = CType(r.GetResponse(), HttpWebResponse)

            Dim reader As New IO.StreamReader(wr.GetResponseStream())
            Result = reader.ReadToEnd()
            reader.Close()
        Catch ex As WebException
            ErrorDesc = "The website timed out when attempting to connect to UPS Online Tools, please try again later."
            Return False
        Catch ex As Exception
            ErrorDesc = "There was a problem communicating with UPS Online Tools, please try again later."
            Return False
        End Try

        'Parse response
        Dim xmlDoc As XmlDocument = New XmlDocument
        xmlDoc.LoadXml(Result)

        Dim Errors As XmlNodeList = xmlDoc.SelectNodes("AddressValidationResponse/Response")
        Dim Status As String = Errors.Item(0).SelectSingleNode("ResponseStatusDescription").InnerText
        If Status = "Success" Then
            Dim Locations As XmlNodeList = xmlDoc.SelectNodes("AddressValidationResponse/AddressValidationResult")
            If Locations.Count > 1 Then
                ErrorDesc = "The address is not valid. Please verify the City, State and Zipcode are correct."
                Return False
            End If
        Else
            ErrorDesc = Errors.Item(0).SelectSingleNode("Error/ErrorDescription").InnerText
            Return False
        End If
        Return True
    End Function

	Public Function GetRate(ByVal ToCity As String, ByVal ToState As String, ByVal ToZip As String, ByVal ToCountry As String, ByVal Weight As Double, ByVal ServiceRating As String, ByVal sIsCommercial As String, ByVal MaxTimeWait As Integer) As Double

		' For UPS Rate/Service Utility, a minimum weight of 1lb is required.
		If Weight < 1 Then Weight = 1

        XMLRateString = "<?xml version=""1.0""?>" & vbCrLf & "<AccessRequest><AccessLicenseNumber>" & SysParam.GetValue("UPSLicenseNo") & "</AccessLicenseNumber><UserId>" & SysParam.GetValue("UPSUserId") & "</UserId><Password>" & SysParam.GetValue("UPSPassword") & "</Password></AccessRequest>" & vbCrLf
		XMLRateString &= "<?xml version=""1.0""?>" & vbCrLf
		XMLRateString &= "<RatingServiceSelectionRequest xml:lang=""en-US"">" & vbCrLf
		XMLRateString &= "	<Request>" & vbCrLf
		XMLRateString &= "			<TransactionReference>" & vbCrLf
		XMLRateString &= "			<CustomerContext>Rating and Service</CustomerContext>" & vbCrLf
		XMLRateString &= "				<XpciVersion Version=""1.0001""/>" & vbCrLf
		XMLRateString &= "			</TransactionReference>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "			<RequestAction>rate</RequestAction>" & vbCrLf
		XMLRateString &= "			<RequestOption>rate</RequestOption>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "	</Request>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "	<Shipment>" & vbCrLf
		XMLRateString &= "			<Shipper>" & vbCrLf
		XMLRateString &= "				<Address>" & vbCrLf
		XMLRateString &= "					<City>" & m_FromCity & "</City>" & vbCrLf
		XMLRateString &= "					<StateProvinceCode>" & m_FromState & "</StateProvinceCode>" & vbCrLf
		XMLRateString &= "					<PostalCode>" & m_FromZip & "</PostalCode>" & vbCrLf
		XMLRateString &= "					<CountryCode>US</CountryCode>" & vbCrLf
		XMLRateString &= "				</Address>" & vbCrLf
		XMLRateString &= "			</Shipper>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "			<ShipTo>" & vbCrLf
		XMLRateString &= "				<Address>" & vbCrLf
		XMLRateString &= "					<City>" & ToCity & "</City>" & vbCrLf
		XMLRateString &= "					<StateProvinceCode>" & ToState & "</StateProvinceCode>" & vbCrLf
		XMLRateString &= "					<PostalCode>" & ToZip & "</PostalCode>" & vbCrLf
		XMLRateString &= "					<CountryCode>" & ToCountry & "</CountryCode>" & vbCrLf
		If sIsCommercial = "Y" Then
			XMLRateString &= "					<ResidentialAddress>0</ResidentialAddress>" & vbCrLf
		Else
			XMLRateString &= "					<ResidentialAddress>1</ResidentialAddress>" & vbCrLf
		End If
		XMLRateString &= "				</Address>" & vbCrLf
		XMLRateString &= "			</ShipTo>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "			<ShipmentWeight>" & vbCrLf
		XMLRateString &= "				<UnitOfMeasurement>" & vbCrLf
		XMLRateString &= "					<Code>lbs</Code>" & vbCrLf
		XMLRateString &= "					<Description>pounds</Description>" & vbCrLf
		XMLRateString &= "				</UnitOfMeasurement>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "			<Weight>" & FormatNumber(Weight, 0, TriState.True, TriState.False, TriState.False) & "</Weight>" & vbCrLf
		XMLRateString &= "			</ShipmentWeight>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "			<Service>" & vbCrLf
		XMLRateString &= "				<Code>" & ServiceRating & "</Code>" & vbCrLf
		XMLRateString &= "			</Service>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "			<Package>" & vbCrLf
		XMLRateString &= "				<PackagingType>" & vbCrLf
		XMLRateString &= "					<Code>02</Code>" & vbCrLf
		XMLRateString &= "				</PackagingType>" & vbCrLf
		XMLRateString &= "" & vbCrLf
		XMLRateString &= "				<PackageWeight>" & vbCrLf
		XMLRateString &= "					<UnitOfMeasurement>" & vbCrLf
		XMLRateString &= "						<Code>lbs</Code>" & vbCrLf
		XMLRateString &= "					</UnitOfMeasurement>" & vbCrLf
		XMLRateString &= "	" & vbCrLf
		XMLRateString &= "					<Weight>" & FormatNumber(Weight, 0, TriState.True, TriState.False, TriState.False) & "</Weight>" & vbCrLf
		XMLRateString &= "				</PackageWeight>" & vbCrLf & vbCrLf
		XMLRateString &= "			</Package>" & vbCrLf
		XMLRateString &= "	</Shipment>" & vbCrLf
		XMLRateString &= "</RatingServiceSelectionRequest>"

		Dim strResult As String = ""
		Try
			Dim req As HttpWebRequest = CType(WebRequest.Create("https://www.ups.com/ups.app/xml/Rate"), HttpWebRequest)
			req.Method = "POST"
			req.Timeout = MaxTimeWait
			req.ContentType = "application/x-www-form-urlencoded"
			req.ContentLength = XMLRateString.Length()
			Dim outStream As Stream = req.GetRequestStream()
			For x As Integer = 0 To XMLRateString.Length() - 1
				outStream.WriteByte(Convert.ToByte(XMLRateString.Chars(x)))
			Next
			outStream.Flush()
			outStream.Close()

			Dim res As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
			Dim reader As New IO.StreamReader(res.GetResponseStream())
			strResult = reader.ReadToEnd()
			reader.Close()
		Catch ex As WebException
			m_ErrorMsg = "The website timed out when attempting to connect to UPS Online Tools, please try again later."
			Return -1
		Catch ex As Exception
			m_ErrorMsg = "There was a problem communicating with UPS Online Tools, please try again later."
			Return -1
		End Try

		Dim xmlDoc As XmlDocument = New XmlDocument
		xmlDoc.LoadXml(strResult)

		Dim responseNode As XmlNode = xmlDoc.SelectSingleNode("//RatingServiceSelectionResponse/Response")
		Dim ResponseValueNode As XmlNode = responseNode.SelectSingleNode("//ResponseStatusDescription")
		Dim PassFail As String = ResponseValueNode.InnerText

		If PassFail = "Failure" Then
			Dim ErrorNode As XmlNode = responseNode.SelectSingleNode("//Error")
			Dim ErrorValueNode As XmlNode = ErrorNode.SelectSingleNode("//ErrorDescription")
			m_ErrorMsg = ErrorValueNode.InnerText
			Return -1.0
		End If

		Dim TotalChargesNode As XmlNode = responseNode.SelectSingleNode("//TotalCharges/MonetaryValue")
		Dim TotalCharges As String = TotalChargesNode.InnerText
		If IsNumeric(TotalCharges) Then
			Return Double.Parse(TotalCharges)
		End If

		m_ErrorMsg = "Returned value is not numeric."
		Return -1.0
	End Function

	Private Shared Sub ReadCallback(ByVal asynchronousResult As IAsyncResult)
		Dim request As HttpWebRequest = CType(asynchronousResult.AsyncState, HttpWebRequest)

		Dim outStream As Stream = request.EndGetRequestStream(asynchronousResult)
		For x As Integer = 0 To XMLRateString.Length() - 1
			outStream.WriteByte(Convert.ToByte(XMLRateString.Chars(x)))
		Next
		outStream.Flush()
		outStream.Close()
		allDone.Set()
	End Sub	' ReadCallback
End Class