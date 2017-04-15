Option Strict On

Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports ShippingValidator.UPSValidator
Imports System.Configuration

Public Class Validator

    Private m_CandidateList As CandidateCollection
    Private m_Type As AddressType
    Private m_Msg As String

    Public Property CandidateList() As CandidateCollection
        Get
            Return m_CandidateList
        End Get
        Set(ByVal Value As CandidateCollection)
            m_CandidateList = Value
        End Set
    End Property

    Public Property Type() As AddressType
        Get
            Return m_Type
        End Get
        Set(ByVal Value As AddressType)
            m_Type = Value
        End Set
    End Property

    Public Property Msg() As String
        Get
            Return m_Msg
        End Get
        Set(ByVal Value As String)
            m_Msg = Value
        End Set
    End Property

    Public Enum ShippingOption
        UPS
        FedEx
    End Enum

    Public Sub New(ByVal FullName As String, ByVal SalonName As String, ByVal StreetAddress As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal Country As String, ByVal ShippingOption As ShippingOption)
        If FullName Is Nothing Then
            FullName = String.Empty
        End If
        If SalonName Is Nothing Then
            SalonName = String.Empty
        End If
        If StreetAddress Is Nothing Then
            StreetAddress = String.Empty
        End If
        If City Is Nothing Then
            City = String.Empty
        End If
        If State Is Nothing Then
            State = String.Empty
        End If
        If ZipCode Is Nothing Then
            ZipCode = String.Empty
        End If
        If Country Is Nothing Then
            Country = String.Empty
        End If

        SalonName = CStr(SalonName)

        If ShippingOption = ShippingOption.FedEx Then
            Dim request As FedExValidator.AddressValidationRequest = CreateAddressValidationRequest(FullName, SalonName, StreetAddress, City, State, ZipCode, Country)
            Dim service As New FedExValidator.AddressValidationService
            '
            Try
                'Call the AddressValidationService passing in an AddressValidationRequest and returning an AddressValidationReply
                Dim reply As FedExValidator.AddressValidationReply = service.addressValidation(request)

                If (Not reply.HighestSeverity = FedExValidator.NotificationSeverityType.ERROR) And (Not reply.HighestSeverity = FedExValidator.NotificationSeverityType.FAILURE) Then
                    ShowAddressValidationReply(reply)
                Else
                    m_Msg &= ("---------ERROR----------") & Environment.NewLine
                    For Each notification As FedExValidator.Notification In reply.Notifications
                        m_Msg &= notification.Message & Environment.NewLine
                    Next
                End If
            Catch e As SoapException
                m_Msg &= ("---------ERROR----------") & Environment.NewLine
                m_Msg &= ("---------FedEx SoapException ----------------") & Environment.NewLine
                m_Msg &= e.Detail.InnerText
            Catch e As Exception
                m_Msg = ("---------ERROR----------") & Environment.NewLine
                m_Msg &= ("---------FedEx Exception----------------") & Environment.NewLine
                m_Msg &= e.Message
            End Try
        Else
            Try
                Dim xavSvc As New XAVService()
                Dim xavRequest As New XAVRequest()
                Dim upss As New UPSSecurity()
                Dim upssSvcAccessToken As New UPSSecurityServiceAccessToken()
                upssSvcAccessToken.AccessLicenseNumber = "EC6EB1E3DE89AE18" 'ConfigurationSettings.AppSettings("UPSSLicenseNo") 
                upss.ServiceAccessToken = upssSvcAccessToken
                Dim upssUsrNameToken As New UPSSecurityUsernameToken()
                upssUsrNameToken.Username = "nailsuperstore" 'ConfigurationSettings.AppSettings("UPSSUsername")
                upssUsrNameToken.Password = "F@ck3804" 'ConfigurationSettings.AppSettings("UPSSPassword")
                upss.UsernameToken = upssUsrNameToken
                xavSvc.UPSSecurityValue = upss
                Dim request As New RequestType()

                'Below code contains dummy data for reference. Please update as required.
                Dim requestOption As String() = {"3"}
                request.RequestOption = requestOption
                xavRequest.Request = request
                Dim addressKeyFormat As New AddressKeyFormatType()
                Dim addressLine As String() = {StreetAddress.Trim()}
                addressKeyFormat.ItemsElementName = New ItemsChoiceType() {ItemsChoiceType.PoliticalDivision1, ItemsChoiceType.PoliticalDivision2, ItemsChoiceType.PostcodePrimaryLow}
                Dim addressKeyFormatItems As String() = {State.Trim(), City.Trim(), ZipCode.Trim()}
                addressKeyFormat.Items = addressKeyFormatItems
                addressKeyFormat.AddressLine = addressLine
                addressKeyFormat.Urbanization = SalonName.Trim()
                addressKeyFormat.ConsigneeName = FullName.Trim()
                addressKeyFormat.CountryCode = Country.Trim()
                xavRequest.AddressKeyFormat = addressKeyFormat
                'Dim ServicePointManager.CertificatePolicy AS New TrustAllCertificatePolicy()

                Dim xavResponse As XAVResponse = xavSvc.ProcessXAV(xavRequest)
                m_Type = CType(xavResponse.AddressClassification.Code, AddressType)

                m_Msg &= "Status Code: " + xavResponse.Response.ResponseStatus.Code & Environment.NewLine
                m_Msg &= "Status Description: " + xavResponse.Response.ResponseStatus.Description & Environment.NewLine

                m_Msg &= "AddressClassification Code: " + xavResponse.AddressClassification.Code & Environment.NewLine
                m_Msg &= "AddressClassification Description: " + xavResponse.AddressClassification.Description & Environment.NewLine

                If Not xavResponse.Candidate Is Nothing Then
                    m_CandidateList = New CandidateCollection

                    For Each can As CandidateType In xavResponse.Candidate
                        Dim c As New Candidate()
                        Dim bBreak As Boolean = False
                        Dim strAddressLine As String = ""

                        If can.AddressKeyFormat.AddressLine.Length > 0 Then

                            For Each s As String In can.AddressKeyFormat.AddressLine
                                strAddressLine &= String.Format("{0} ", s)
                            Next

                            bBreak = strAddressLine.Contains("-")
                        End If

                        'Check range address to quit
                        If bBreak Then
                            Continue For
                        End If

                        Dim strCity As String = ""
                        Dim strState As String = ""
                        Dim strZip As String = ""

                        If can.AddressKeyFormat.Items.Length > 0 Then
                            If can.AddressKeyFormat.Items.Length >= 5 Then
                                strCity = can.AddressKeyFormat.Items(0)
                                strState = can.AddressKeyFormat.Items(1)
                                strZip = can.AddressKeyFormat.Items(2) & "-" & can.AddressKeyFormat.Items(3)
                            End If
                        End If

                        c.Code = CInt(can.AddressClassification.Code)
                        c.Street = strAddressLine
                        c.State = strState
                        c.ZipCode = strZip
                        c.City = strCity
                        c.All = String.Format("{0}|{1}|{2}|{3}|{4}", c.Street, c.City, c.State, c.ZipCode, c.Code)

                        If c.Street = StreetAddress And c.City = City And c.State = State And c.ZipCode = ZipCode Then
                            m_CandidateList.Clear()
                            m_CandidateList.Add(c)
                            Exit For
                        End If

                        m_CandidateList.Add(c)
                    Next
                End If

            Catch ex As System.Web.Services.Protocols.SoapException
                m_Msg &= ("---------ERROR----------") & Environment.NewLine
                m_Msg &= ("---------UPS SoapException----------------") & Environment.NewLine
                m_Msg &= ("---------""Hard"" is user error ""Transient"" is system error----------------") & Environment.NewLine
                m_Msg &= ("SoapException Message= " + ex.Message) & Environment.NewLine
                m_Msg &= ("") & Environment.NewLine
                m_Msg &= ("SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText) & Environment.NewLine
                m_Msg &= ("") & Environment.NewLine
                m_Msg &= ("SoapException XML String for all= " + ex.Detail.LastChild.OuterXml) & Environment.NewLine
                m_Msg &= ("") & Environment.NewLine
                m_Msg &= ("SoapException StackTrace= " + ex.StackTrace) & Environment.NewLine
                m_Msg &= ("-------------------------") & Environment.NewLine
                m_Msg &= ("")

            Catch ex As Exception
                m_Msg &= ("---------ERROR----------") & Environment.NewLine
                m_Msg &= ("---------UPS Exception----------------") & Environment.NewLine
                m_Msg &= (" Generaal Exception= " + ex.Message) & Environment.NewLine
                m_Msg &= (" Generaal Exception-StackTrace= " + ex.StackTrace) & Environment.NewLine
                m_Msg &= ("-------------------------") & Environment.NewLine
            End Try
        End If

    End Sub

    Public Sub New(ByVal FullName As String, ByVal SalonName As String, ByVal StreetAddress As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal Country As String)
        If FullName Is Nothing Then
            FullName = String.Empty
        End If
        If SalonName Is Nothing Then
            SalonName = String.Empty
        End If
        If StreetAddress Is Nothing Then
            StreetAddress = String.Empty
        End If
        If City Is Nothing Then
            City = String.Empty
        End If
        If State Is Nothing Then
            State = String.Empty
        End If
        If ZipCode Is Nothing Then
            ZipCode = String.Empty
        End If
        If Country Is Nothing Then
            Country = String.Empty
        End If

        SalonName = CStr(SalonName)


        Try
            Dim xavSvc As New XAVService()
            Dim xavRequest As New XAVRequest()
            Dim upss As New UPSSecurity()
            Dim upssSvcAccessToken As New UPSSecurityServiceAccessToken()
            upssSvcAccessToken.AccessLicenseNumber = "EC6EB1E3DE89AE18" 'ConfigurationSettings.AppSettings("UPSSLicenseNo") 
            upss.ServiceAccessToken = upssSvcAccessToken
            Dim upssUsrNameToken As New UPSSecurityUsernameToken()
            upssUsrNameToken.Username = "nailsuperstore" 'ConfigurationSettings.AppSettings("UPSSUsername")
            upssUsrNameToken.Password = "F@ck3804" 'ConfigurationSettings.AppSettings("UPSSPassword")
            upss.UsernameToken = upssUsrNameToken
            xavSvc.UPSSecurityValue = upss
            Dim request As New RequestType()

            'Below code contains dummy data for reference. Please update as required.
            Dim requestOption As String() = {"3"}
            request.RequestOption = requestOption
            xavRequest.Request = request
            Dim addressKeyFormat As New AddressKeyFormatType()
            Dim addressLine As String() = {StreetAddress.Trim()}
            addressKeyFormat.ItemsElementName = New ItemsChoiceType() {ItemsChoiceType.PoliticalDivision1, ItemsChoiceType.PoliticalDivision2, ItemsChoiceType.PostcodePrimaryLow}
            Dim addressKeyFormatItems As String() = {State.Trim(), City.Trim(), ZipCode.Trim()}
            addressKeyFormat.Items = addressKeyFormatItems
            addressKeyFormat.AddressLine = addressLine
            addressKeyFormat.Urbanization = SalonName.Trim()
            addressKeyFormat.ConsigneeName = FullName.Trim()
            addressKeyFormat.CountryCode = Country.Trim()
            xavRequest.AddressKeyFormat = addressKeyFormat
            'Dim ServicePointManager.CertificatePolicy AS New TrustAllCertificatePolicy()

            Dim xavResponse As XAVResponse = xavSvc.ProcessXAV(xavRequest)
            m_Type = CType(xavResponse.AddressClassification.Code, AddressType)

            m_Msg &= "Status Code: " + xavResponse.Response.ResponseStatus.Code & Environment.NewLine
            m_Msg &= "Status Description: " + xavResponse.Response.ResponseStatus.Description & Environment.NewLine

            m_Msg &= "AddressClassification Code: " + xavResponse.AddressClassification.Code & Environment.NewLine
            m_Msg &= "AddressClassification Description: " + xavResponse.AddressClassification.Description & Environment.NewLine

            If Not xavResponse.Candidate Is Nothing Then
                m_CandidateList = New CandidateCollection

                For Each can As CandidateType In xavResponse.Candidate
                    Dim c As New Candidate()
                    Dim bBreak As Boolean = False
                    Dim strAddressLine As String = ""

                    If can.AddressKeyFormat.AddressLine.Length > 0 Then

                        For Each s As String In can.AddressKeyFormat.AddressLine
                            strAddressLine &= String.Format("{0} ", s)
                        Next

                        bBreak = strAddressLine.Contains("-")
                    End If

                    'Check range address to quit
                    If bBreak Then
                        Continue For
                    End If

                    Dim strCity As String = ""
                    Dim strState As String = ""
                    Dim strZip As String = ""

                    If can.AddressKeyFormat.Items.Length > 0 Then
                        If can.AddressKeyFormat.Items.Length >= 5 Then
                            strCity = can.AddressKeyFormat.Items(0)
                            strState = can.AddressKeyFormat.Items(1)
                            strZip = can.AddressKeyFormat.Items(2) & "-" & can.AddressKeyFormat.Items(3)
                        End If
                    End If

                    c.Code = CInt(can.AddressClassification.Code)
                    c.Street = strAddressLine
                    c.State = strState
                    c.ZipCode = strZip
                    c.City = strCity
                    c.All = String.Format("{0}|{1}|{2}|{3}|{4}", c.Street, c.City, c.State, c.ZipCode, c.Code)

                    If c.Street = StreetAddress And c.City = City And c.State = State And c.ZipCode = ZipCode Then
                        m_CandidateList.Clear()
                        m_CandidateList.Add(c)
                        Exit For
                    End If

                    m_CandidateList.Add(c)
                Next
            End If

        Catch ex As System.Web.Services.Protocols.SoapException
            m_Msg &= ("---------ERROR----------") & Environment.NewLine
            m_Msg &= ("---------UPS SoapException----------------") & Environment.NewLine
            m_Msg &= ("---------""Hard"" is user error ""Transient"" is system error----------------") & Environment.NewLine
            m_Msg &= ("SoapException Message= " + ex.Message) & Environment.NewLine
            m_Msg &= ("") & Environment.NewLine
            m_Msg &= ("SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText) & Environment.NewLine
            m_Msg &= ("") & Environment.NewLine
            m_Msg &= ("SoapException XML String for all= " + ex.Detail.LastChild.OuterXml) & Environment.NewLine
            m_Msg &= ("") & Environment.NewLine
            m_Msg &= ("SoapException StackTrace= " + ex.StackTrace) & Environment.NewLine
            m_Msg &= ("-------------------------") & Environment.NewLine
            m_Msg &= ("")

        Catch ex As Exception
            m_Msg &= ("---------ERROR----------") & Environment.NewLine
            m_Msg &= ("---------UPS Exception----------------") & Environment.NewLine
            m_Msg &= (" Generaal Exception= " + ex.Message) & Environment.NewLine
            m_Msg &= (" Generaal Exception-StackTrace= " + ex.StackTrace) & Environment.NewLine
            m_Msg &= ("-------------------------") & Environment.NewLine
        End Try


        If m_Type = AddressType.Unknown Then
            m_Msg &= ("---CHECK STEP 2 WITH FEDEX---") & Environment.NewLine
            Dim request As FedExValidator.AddressValidationRequest = CreateAddressValidationRequest(FullName, SalonName, StreetAddress, City, State, ZipCode, Country)
            Dim service As New FedExValidator.AddressValidationService
            '
            Try
                'Call the AddressValidationService passing in an AddressValidationRequest and returning an AddressValidationReply
                Dim reply As FedExValidator.AddressValidationReply = service.addressValidation(request)

                If (Not reply.HighestSeverity = FedExValidator.NotificationSeverityType.ERROR) And (Not reply.HighestSeverity = FedExValidator.NotificationSeverityType.FAILURE) Then
                    If (Not reply.AddressResults Is Nothing) Then
                        m_CandidateList = New CandidateCollection

                        m_Msg &= "CustomerTransactionId: " & reply.TransactionDetail.CustomerTransactionId & Environment.NewLine

                        If reply.TransactionDetail.Localization IsNot Nothing Then
                            m_Msg &= "Localization.LanguageCode: " & reply.TransactionDetail.Localization.LanguageCode & Environment.NewLine
                            m_Msg &= "Localization.LocaleCode: " & reply.TransactionDetail.Localization.LocaleCode & Environment.NewLine
                        End If

                        For Each result As FedExValidator.AddressValidationResult In reply.AddressResults

                            m_Msg &= "AddressId: " & result.AddressId & Environment.NewLine

                            If (Not result.ProposedAddressDetails Is Nothing) Then
                                For Each detail As FedExValidator.ProposedAddressDetail In result.ProposedAddressDetails
                                    'Get Type
                                    If detail.ResidentialStatus = FedExValidator.ResidentialStatusType.BUSINESS Then
                                        m_Type = AddressType.Commercial
                                    ElseIf detail.ResidentialStatus = FedExValidator.ResidentialStatusType.RESIDENTIAL Then
                                        m_Type = AddressType.Residential
                                    ElseIf detail.ResidentialStatus = FedExValidator.ResidentialStatusType.INSUFFICIENT_DATA Then
                                        m_Type = AddressType.Insufficient
                                    Else
                                        m_Type = AddressType.Unknown
                                    End If
                                Next
                            End If
                        Next
                    End If
                Else
                    m_Msg &= ("---------ERROR----------") & Environment.NewLine
                    For Each notification As FedExValidator.Notification In reply.Notifications
                        m_Msg &= notification.Message & Environment.NewLine
                    Next
                End If
            Catch e As SoapException
                m_Msg &= ("---------ERROR----------") & Environment.NewLine
                m_Msg &= ("---------FedEx SoapException ----------------") & Environment.NewLine
                m_Msg &= e.Detail.InnerText
            Catch e As Exception
                m_Msg = ("---------ERROR----------") & Environment.NewLine
                m_Msg &= ("---------FedEx Exception----------------") & Environment.NewLine
                m_Msg &= e.Message
            End Try
        End If

    End Sub

#Region "FEDEX"
    Private Function CreateAddressValidationRequest(ByVal FullName As String, ByVal SalonName As String, ByVal StreetAddress As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal Country As String) As FedExValidator.AddressValidationRequest
        ' Build the AddressValidationRequest
        Dim request As FedExValidator.AddressValidationRequest = New FedExValidator.AddressValidationRequest()
        '
        request.WebAuthenticationDetail = New FedExValidator.WebAuthenticationDetail()
        request.WebAuthenticationDetail.UserCredential = New FedExValidator.WebAuthenticationCredential()
        request.WebAuthenticationDetail.UserCredential.Key = "g4KZiO8Ylayemanp" '"TS7VhBCSmnsFSofc" ' Replace "XXX" with the Key
        request.WebAuthenticationDetail.UserCredential.Password = "pEf3h70F8XjedLpRHE2zLC1Mu" ' Replace "XXX" with the Password
        '
        request.ClientDetail = New FedExValidator.ClientDetail()
        request.ClientDetail.AccountNumber = "253217383" ' Replace "XXX" with clients account number
        request.ClientDetail.MeterNumber = "105760425" '"118582878" ' Replace "XXX" with clients meter number
        '
        request.TransactionDetail = New FedExValidator.TransactionDetail()
        request.TransactionDetail.CustomerTransactionId = "NAILSUPERSTORE" 'The client will get the same value back in the reply
        request.Version = New FedExValidator.VersionId()
        request.RequestTimestamp = DateTime.Now

        'SetOptions(request)
        request.Options = New FedExValidator.AddressValidationOptions()
        request.Options.CheckResidentialStatus = True
        request.Options.CheckResidentialStatusSpecified = True
        request.Options.VerifyAddresses = True
        request.Options.VerifyAddressesSpecified = True
        request.Options.MaximumNumberOfMatches = "5"
        request.Options.StreetAccuracy = FedExValidator.AddressValidationAccuracyType.MEDIUM
        request.Options.DirectionalAccuracy = FedExValidator.AddressValidationAccuracyType.MEDIUM
        request.Options.CompanyNameAccuracy = FedExValidator.AddressValidationAccuracyType.MEDIUM
        request.Options.ConvertToUpperCase = True
        request.Options.ConvertToUpperCaseSpecified = True
        request.Options.RecognizeAlternateCityNames = True
        request.Options.RecognizeAlternateCityNamesSpecified = True
        request.Options.ReturnParsedElements = True
        request.Options.ReturnParsedElementsSpecified = True

        'SetAddress(request)
        request.AddressesToValidate = New FedExValidator.AddressToValidate() {New FedExValidator.AddressToValidate()}
        request.AddressesToValidate(0).AddressId = FullName
        request.AddressesToValidate(0).Address = New FedExValidator.Address()
        request.AddressesToValidate(0).Address.StreetLines = New String(0) {StreetAddress}
        request.AddressesToValidate(0).Address.CountryCode = Country
        request.AddressesToValidate(0).Address.PostalCode = ZipCode
        request.AddressesToValidate(0).Address.City = City
        request.AddressesToValidate(0).CompanyName = SalonName

        Return request
    End Function

    Private Sub ShowAddressValidationReply(ByRef reply As FedExValidator.AddressValidationReply)
        If (Not reply.AddressResults Is Nothing) Then
            m_CandidateList = New CandidateCollection

            m_Msg &= "CustomerTransactionId: " & reply.TransactionDetail.CustomerTransactionId & Environment.NewLine

            If reply.TransactionDetail.Localization IsNot Nothing Then
                m_Msg &= "Localization.LanguageCode: " & reply.TransactionDetail.Localization.LanguageCode & Environment.NewLine
                m_Msg &= "Localization.LocaleCode: " & reply.TransactionDetail.Localization.LocaleCode & Environment.NewLine
            End If
            

            For Each result As FedExValidator.AddressValidationResult In reply.AddressResults

                m_Msg &= "AddressId: " & result.AddressId & Environment.NewLine

                If (Not result.ProposedAddressDetails Is Nothing) Then
                    For Each detail As FedExValidator.ProposedAddressDetail In result.ProposedAddressDetails
                        'Get Type
                        If detail.ResidentialStatus = FedExValidator.ResidentialStatusType.BUSINESS Then
                            m_Type = AddressType.Commercial
                        ElseIf detail.ResidentialStatus = FedExValidator.ResidentialStatusType.RESIDENTIAL Then
                            m_Type = AddressType.Residential
                        ElseIf detail.ResidentialStatus = FedExValidator.ResidentialStatusType.INSUFFICIENT_DATA Then
                            m_Type = AddressType.Insufficient
                        Else
                            m_Type = AddressType.Unknown
                        End If

                        Dim c As New Candidate()
                        c.Street = detail.Address.StreetLines(0)

                        c.State = detail.Address.StateOrProvinceCode
                        If c.State Is Nothing Then
                            c.State = String.Empty
                        End If

                        c.ZipCode = detail.Address.PostalCode()
                        If c.ZipCode Is Nothing Then
                            c.ZipCode = String.Empty
                        End If

                        c.City = detail.Address.City
                        If c.City Is Nothing Then
                            c.City = String.Empty
                        End If

                        c.Code = CInt(detail.ResidentialStatus)
                        c.All = String.Format("{0}|{1}|{2}|{3}|{4}", c.Street, c.City, c.State, c.ZipCode, c.Code)
                        m_CandidateList.Add(c)

                    Next
                End If
            Next
        End If
    End Sub
#End Region
End Class


Public Class Candidate
    Private m_Code As Integer
    Private m_Street As String
    Private m_City As String
    Private m_State As String
    Private m_ZipCode As String
    Private m_All As String

    Public Property Code() As Integer
        Get
            Return m_Code
        End Get
        Set(ByVal Value As Integer)
            m_Code = Value
        End Set
    End Property

    Public Property Street() As String
        Get
            Return m_Street
        End Get
        Set(ByVal Value As String)
            m_Street = Value
        End Set
    End Property

    Public Property City() As String
        Get
            Return m_City
        End Get
        Set(ByVal Value As String)
            m_City = Value
        End Set
    End Property

    Public Property State() As String
        Get
            Return m_State
        End Get
        Set(ByVal Value As String)
            m_State = Value
        End Set
    End Property

    Public Property ZipCode() As String
        Get
            Return m_ZipCode
        End Get
        Set(ByVal Value As String)
            m_ZipCode = Value
        End Set
    End Property

    Public Property All() As String
        Get
            Return m_All
        End Get
        Set(ByVal Value As String)
            m_All = Value
        End Set
    End Property

End Class

Public Class CandidateCollection
    Inherits CollectionBase

    Public Sub New()
    End Sub

    Public Sub Add(ByVal can As Candidate)
        Me.List.Add(can)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As Candidate
        Get
            Return CType(Me.List.Item(Index), Candidate)
        End Get

        Set(ByVal Value As Candidate)
            Me.List(Index) = Value
        End Set
    End Property
End Class

Public Enum AddressType As Integer
    Unknown = 0
    Commercial = 1
    Residential = 2
    Insufficient = 3
End Enum