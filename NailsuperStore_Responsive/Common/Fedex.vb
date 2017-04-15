Imports Components
Imports DataLayer

Public Class Fedex

	'Public Sub New(ByVal _DB As Database)
	'	m_DB = _DB
	'End Sub

	'Private m_DB As Database
	'Public Property DB() As Database
	'	Get
	'		Return m_DB
	'	End Get
	'	Set(ByVal value As Database)
	'		m_DB = value
	'	End Set
	'End Property

	''/\/\ Constants used for FEDEX Validation
	'Private Const C_FEDEX_ACCOUNT As String = ""
	'Private Const C_FEDEX_METER As String = ""

	''/\/\ Constants used for FedEx Services
	''If these change they must change in the database
	'Private Const C_FEDEX_PRIORITYOVERNIGHT As String = "PRIORITYOVERNIGHT"
	'Private Const C_FEDEX_STANDARDOVERNIGHT As String = "STANDARDOVERNIGHT"
	'Private Const C_FEDEX_FIRSTOVERNIGHT As String = "FIRSTOVERNIGHT"
	'Private Const C_FEDEX_FEDEX2DAY As String = "FEDEX2DAY"
	'Private Const C_FEDEX_FEDEXEXPRESSSAVER As String = "FEDEXEXPRESSSAVER"
	'Private Const C_FEDEX_INTERNATIONALPRIORITY As String = "INTERNATIONALPRIORITY"
	'Private Const C_FEDEX_INTERNATIONALECONOMY As String = "INTERNATIONALECONOMY"
	'Private Const C_FEDEX_INTERNATIONALFIRST As String = "INTERNATIONALFIRST"
	'Private Const C_FEDEX_FEDEX1DAYFREIGHT As String = "FEDEX1DAYFREIGHT"
	'Private Const C_FEDEX_FEDEX2DAYFREIGHT As String = "FEDEX2DAYFREIGHT"
	'Private Const C_FEDEX_FEDEX3DAYFREIGHT As String = "FEDEX3DAYFREIGHT"
	'Private Const C_FEDEX_FEDEXGROUND As String = "FEDEXGROUND"
	'Private Const C_FEDEX_GROUNDHOMEDELIVERY As String = "GROUNDHOMEDELIVERY"
	'Private Const C_FEDEX_INTERNATIONALPRIORITY_FREIGHT As String = "INTERNATIONALPRIORITY FREIGHT"
	'Private Const C_FEDEX_INTERNATIONALECONOMY_FREIGHT As String = "INTERNATIONALECONOMY FREIGHT"

	'Private Function RemoveSpecialXMLCharacters(ByVal sInput) As String
	'	Dim sResult As String

	'	If DB.IsEmpty(sInput) Then
	'		Return String.Empty
	'	End If
	'	sResult = sInput
	'	sResult = Replace(sResult, "#", "")
	'	Return sResult
	'End Function

	''/\/\
	''Function: 		FEDEXGetRate (FROM_STATE, FROM_ZIP, TO_STATE, TO_ZIP, IN_WEIGHT, RATING_SERVICE_PLAN, sErrorDesc)
	''Description:	This function passes the shippers address, along with the customers address and shipping option and package weight, and returns the shipping rate.
	''Input:			FROM_STATE is the passed shippers state
	''				FROM_ZIP is the passed shippers zip
	''				TO_STATE is the passed customers state
	''				TO_ZIP is the passed customers zip
	''				IN_WEIGHT is the passed package weight
	''				RATING_SERVICE_PLAN is the passed package plan
	''Output:		Day the package is to arrive, and shipping price
	''Assumptions:	Passed Variables, FEDEX connection
	''/\/\
	'Function FEDEXGetRate(ByVal ORDER_NR, ByVal TO_ADDRESS1, ByVal TO_ADDRESS2, ByVal TO_CITY, ByVal TO_STATE, ByVal TO_ZIP, ByVal TO_COUNTRY, ByVal IN_WEIGHT, ByVal IN_LENGTH, ByVal IN_WIDTH, ByVal IN_HEIGHT, ByVal RATING_SERVICE_PLAN, ByVal sErrorDesc)

	'	Dim FEDEXRateString, anXMLhttpObject, strResult, xmlDoc
	'	Dim ResponseNode, ResponseValueNode, PassFail, Error
	'	Dim TotalChargesNode, TotalCharges
	'	Dim xmlstring, PACKAGING
	'	Dim ErrorNode, ErrorValueNode
	'	Dim TodayDate, DATE_STRING, CARRIER_CODE

	'	PACKAGING = "YOURPACKAGING"

	'	If RATING_SERVICE_PLAN = "FEDEXGROUND" Then
	'		CARRIER_CODE = "FDXG"
	'	Else
	'		CARRIER_CODE = "FDXE"
	'	End If

	'TodayDate = Date()
	'	DATE_STRING = Year(TodayDate) & "-"
	'	If Month(TodayDate) < 10 Then
	'		DATE_STRING = DATE_STRING & "0"
	'	End If
	'	DATE_STRING = DATE_STRING & Month(TodayDate) & "-"
	'	If Day(TodayDate) < 10 Then
	'		DATE_STRING = DATE_STRING & "0"
	'	End If
	'	DATE_STRING = DATE_STRING & Day(TodayDate)
	'	TodayDate = DATE_STRING

	'	FEDEXGetRate = -1

	'	If DB.Number(IN_WEIGHT) < 1 Then IN_WEIGHT = 1

	'	FEDEXRateString = _
	'	   "<?xml version=""1.0"" encoding=""UTF-8"" ?>" & vbCrLf & _
	'	  "<FDXRateRequest xmlns:api=""http://www.fedex.com/fsmapi"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""FDXRateRequest.xsd"">" & vbCrLf & _
	'	 "<RequestHeader>" & vbCrLf & _
	'	   "<CustomerTransactionIdentifier>" & ORDER_NR & "</CustomerTransactionIdentifier>" & vbCrLf & _
	'	 "<AccountNumber>" & C_FEDEX_ACCOUNT & "</AccountNumber>" & vbCrLf & _
	'	   "<MeterNumber>" & C_FEDEX_METER & "</MeterNumber>" & vbCrLf & _
	'	   "<CarrierCode>" & CARRIER_CODE & "</CarrierCode>" & vbCrLf & _
	'	 "</RequestHeader>" & vbCrLf & _
	'	 "<ShipDate>" & TodayDate & "</ShipDate>" & vbCrLf & _
	'	 "<DropoffType>REGULARPICKUP</DropoffType>" & vbCrLf & _
	'	 "<Service>" & RATING_SERVICE_PLAN & "</Service>" & vbCrLf & _
	'	 "<Packaging>" & PACKAGING & "</Packaging>" & vbCrLf & _
	'	 "<WeightUnits>LBS</WeightUnits>" & vbCrLf & _
	'	 "<Weight>" & FormatNumber(IN_WEIGHT, 1) & "</Weight>" & vbCrLf & _
	'	 "<OriginAddress>" & vbCrLf & _
	'	  "	<Line1>" & RemoveSpecialXMLCharacters(SysParam.GetValue(DB, "SHIPPING_ADDRESS1")) & "</Line1>"

	'	If Not DB.IsEmpty(RemoveSpecialXMLCharacters(SysParam.GetValue(DB, "FEDEX_ADDRESS2"))) Then
	'		FEDEXRateString = FEDEXRateString & "	<Line2>" & RemoveSpecialXMLCharacters(SysParam.GetValue(DB, "SHIPPING_ADDRESS2")) & "</Line2>"
	'	End If

	'	FEDEXRateString = FEDEXRateString & _
	'	   "	<City>" & RemoveSpecialXMLCharacters(SysParam.GetValue(DB, "SHIPPING_CITY")) & "</City>" & _
	'	   "	<StateOrProvinceCode>" & RemoveSpecialXMLCharacters(SysParam.GetValue(DB, "SHIPPING_STATE")) & "</StateOrProvinceCode>" & _
	'	   "	<PostalCode>" & RemoveSpecialXMLCharacters(SysParam.GetValue(DB, "SHIPPING_ZIP")) & "</PostalCode>" & _
	'	   "	<CountryCode>US</CountryCode>" & _
	'	   "</OriginAddress>" & vbCrLf & _
	'	  "<DestinationAddress>" & vbCrLf & _
	'	   "	<Line1>" & RemoveSpecialXMLCharacters(TO_ADDRESS1) & "</Line1>"

	'	If Not DB.IsEmpty(RemoveSpecialXMLCharacters(TO_ADDRESS2)) Then
	'		FEDEXRateString = FEDEXRateString & "	<Line2>" & RemoveSpecialXMLCharacters(TO_ADDRESS2) & "</Line2>"
	'	End If

	'	FEDEXRateString = FEDEXRateString & _
	'	  "	<City>" & RemoveSpecialXMLCharacters(TO_CITY) & "</City>" & vbCrLf & _
	'	  "	<StateOrProvinceCode>" & RemoveSpecialXMLCharacters(TO_STATE) & "</StateOrProvinceCode>" & vbCrLf & _
	'	  "	<PostalCode>" & RemoveSpecialXMLCharacters(TO_ZIP) & "</PostalCode>" & vbCrLf & _
	'	  "	<CountryCode>" & RemoveSpecialXMLCharacters(TO_COUNTRY) & "</CountryCode>" & vbCrLf & _
	'	  "</DestinationAddress>" & vbCrLf & _
	'	  "<Payment>" & vbCrLf & _
	'	  "<PayorType>SENDER</PayorType>" & vbCrLf & _
	'	  "</Payment>" & vbCrLf

	'	If ((IN_WIDTH <> 0) Or (IN_LENGTH <> 0) Or (IN_HEIGHT <> 0)) Then
	'		FEDEXRateString = FEDEXRateString & _
	'		   " <Dimensions>" & vbCrLf & _
	'		   "   <Length>" & IN_LENGTH & "</Length>" & vbCrLf & _
	'		   "   <Width>" & IN_WIDTH & "</Width>" & vbCrLf & _
	'		   "   <Height>" & IN_HEIGHT & "</Height>" & vbCrLf & _
	'		   "   <Units>IN</Units>" & vbCrLf & _
	'		   " </Dimensions>" & vbCrLf
	'	End If

	'	FEDEXRateString = FEDEXRateString & _
	'	  "<PackageCount>1</PackageCount>" & vbCrLf & _
	'	"</FDXRateRequest>"

	'	anXMLhttpObject = System.Web.HttpContext.Current.Server.CreateObject("microsoft.XMLHttp")
	'	anXMLhttpObject.open("POST", "https://gateway.fedex.com/GatewayDC", False)
	'	anXMLhttpObject.setRequestHeader("Request", "POST")
	'	anXMLhttpObject.setRequestHeader("Content-type", "application/x-www-form-urlencoded")
	'	anXMLhttpObject.setRequestHeader("Content-Length", Len(xmlstring) + 1)

	'	anXMLhttpObject.send(FEDEXRateString)

	'	If (UCase(anXMLhttpObject.statusText) <> "OK") Then
	'		sErrorDesc = "There was an Error Communicating with FEDEX Online Tools"
	'		Exit Function
	'	End If

	'	strResult = anXMLhttpObject.responseText

	'	xmlDoc = System.Web.HttpContext.Current.Server.CreateObject("Microsoft.XMLDOM")
	'	xmlDoc.loadXML(strResult)

	'	On Error Resume Next

	'	TotalChargesNode = xmlDoc.selectSingleNode("//EstimatedCharges/DiscountedCharges/NetCharge")
	'	TotalCharges = TotalChargesNode.text

	'	If Err() Then
	'		ErrorNode = xmlDoc.selectSingleNode("//Error/Code")
	'		PassFail = ErrorNode.text
	'		If PassFail <> 0 Then
	'			ErrorValueNode = ErrorNode.selectSingleNode("//Error/Message")
	'		Error = ErrorValueNode.text
	'		sErrorDEsc = Error
	'			Exit Function
	'		Else
	'			sErrorDesc = "There was an Error Communicating with FEDEX Online Tools"
	'			Exit Function
	'		End If
	'	End If

	'	FEDEXGetRate = TotalCharges
	'End Function

End Class
