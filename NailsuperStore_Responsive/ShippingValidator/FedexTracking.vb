Option Strict On

Imports System.Web.Services.Protocols
'Imports TrackWebServiceClient.TrackServiceWebReference
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Collections
Imports ShippingValidator.FedexTracking
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Configuration

Public Class FedexTracking
    Public Message As String
    Public replyTracking As FedexGetTracking.TrackReply
    Public msgError As String
    Private ShipBeginDate As String
    Private Ship_Days As Integer
    Private StatusCode As String
    Private ExceptionDescription As String
    Public Sub New(ByVal TrackingNo As String, ByVal BeginDate As String, ByVal ShippingType As String)
        ShipBeginDate = BeginDate
        Ship_Days = ReturnShipDays(ShippingType)
        Dim request As FedexGetTracking.TrackRequest = CreateTrackRequest(TrackingNo)
        '
        Dim service As FedexGetTracking.TrackService = New FedexGetTracking.TrackService()
        '
        Try
            ' Call the Track web service passing in a TrackRequest and returning a TrackReply
            Dim reply As FedexGetTracking.TrackReply = service.track(request)
            msgError = reply.HighestSeverity.ToString
            If ((Not reply.HighestSeverity = FedexGetTracking.NotificationSeverityType.ERROR) And (Not reply.HighestSeverity = FedexGetTracking.NotificationSeverityType.FAILURE)) Then
                replyTracking = reply
            End If
        Catch se As SoapException
            Console.WriteLine(se.Detail.ToString())
        Catch e As Exception
            Console.WriteLine(e.Message)
        End Try

    End Sub
    Function CreateTrackRequest(ByVal TrackingNo As String) As FedexGetTracking.TrackRequest
        'Build the TrackRequest
        Dim request As FedexGetTracking.TrackRequest = New FedexGetTracking.TrackRequest()
        '
        request.WebAuthenticationDetail = New FedexGetTracking.WebAuthenticationDetail()
        request.WebAuthenticationDetail.UserCredential = New FedexGetTracking.WebAuthenticationCredential()

        request.WebAuthenticationDetail.UserCredential.Key = ConfigurationSettings.AppSettings("FedexKey") ' "48Nijq6exNJvKi10" ' Replace "XXX" with the Key
        request.WebAuthenticationDetail.UserCredential.Password = ConfigurationSettings.AppSettings("FedexPassword").ToString() '"Chy4yrnMtAHHVzGMlNvl9D15F" ' Replace "XXX" with the Password

        request.ClientDetail = New FedexGetTracking.ClientDetail()
        request.ClientDetail.AccountNumber = ConfigurationSettings.AppSettings("FedexAccountNumber").ToString() '"253217383" ' Replace "XXX" with client's account number
        request.ClientDetail.MeterNumber = ConfigurationSettings.AppSettings("FedexMeterNumber").ToString() '"105663917" ' Replace "XXX" with client's meter number
        request.TransactionDetail = New FedexGetTracking.TransactionDetail()
        request.TransactionDetail.CustomerTransactionId = "***Track v6 Request using VB.NET***" 'This is a reference field for the customer.  Any value can be used and will be provided in the response.
        '        
        request.Version = New FedexGetTracking.VersionId()
        '
        'Tracking information
        request.PackageIdentifier = New FedexGetTracking.TrackPackageIdentifier()
        request.PackageIdentifier.Value = TrackingNo ' Replace "XXX" with tracking number or door tag
        request.PackageIdentifier.Type = FedexGetTracking.TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG
        ' Date range is optional.
        ' If omitted, set to false
        request.ShipDateRangeBegin = DateTime.Parse(ShipBeginDate) 'DateTime.Parse("06/18/2012") ' MM/DD/YYYY
        request.ShipDateRangeEnd = request.ShipDateRangeBegin.AddDays(Ship_Days) 'request.ShipDateRangeBegin.AddDays(0)
        request.ShipDateRangeBeginSpecified = True
        request.ShipDateRangeEndSpecified = True
        '
        ' Include detailed scans is optional.
        ' If omitted, set to false
        request.IncludeDetailedScans = True
        request.IncludeDetailedScansSpecified = True
        '
        Return request
    End Function

    Private Function ReturnShipDays(ByVal Type As String) As Integer
        Select Case LCase(Type)
            Case "fedex ground"
                '578916797349 processdate = 03/11/2014 , ship 03/28/2014
                Return 17 '6 ' 3 to 5 day
            Case "fedex two day"
                Return 3 'two day 
            Case "fedex next day"
                Return 2 'next day
        End Select
    End Function
End Class
