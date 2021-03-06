﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.4927
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.4927.
'
Namespace UPSValidator
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="XAVBinding", [Namespace]:="http://www.ups.com/WSDL/XOLTWS/XAV/v1.0")>  _
    Partial Public Class XAVService
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private uPSSecurityValueField As UPSSecurity
        
        Private ProcessXAVOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.ShippingValidator.My.MySettings.Default.ShippingValidator_UPSValidator_XAVService
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Property UPSSecurityValue() As UPSSecurity
            Get
                Return Me.uPSSecurityValueField
            End Get
            Set
                Me.uPSSecurityValueField = value
            End Set
        End Property
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event ProcessXAVCompleted As ProcessXAVCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("UPSSecurityValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://onlinetools.ups.com/webservices/XAVBinding/v1.0", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Bare)>  _
        Public Function ProcessXAV(<System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")> ByVal XAVRequest As XAVRequest) As <System.Xml.Serialization.XmlElementAttribute("XAVResponse", [Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")> XAVResponse
            Dim results() As Object = Me.Invoke("ProcessXAV", New Object() {XAVRequest})
            Return CType(results(0),XAVResponse)
        End Function
        
        '''<remarks/>
        Public Overloads Sub ProcessXAVAsync(ByVal XAVRequest As XAVRequest)
            Me.ProcessXAVAsync(XAVRequest, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub ProcessXAVAsync(ByVal XAVRequest As XAVRequest, ByVal userState As Object)
            If (Me.ProcessXAVOperationCompleted Is Nothing) Then
                Me.ProcessXAVOperationCompleted = AddressOf Me.OnProcessXAVOperationCompleted
            End If
            Me.InvokeAsync("ProcessXAV", New Object() {XAVRequest}, Me.ProcessXAVOperationCompleted, userState)
        End Sub
        
        Private Sub OnProcessXAVOperationCompleted(ByVal arg As Object)
            If (Not (Me.ProcessXAVCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent ProcessXAVCompleted(Me, New ProcessXAVCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/UPSS/v1.0"),  _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/UPSS/v1.0", IsNullable:=false)>  _
    Partial Public Class UPSSecurity
        Inherits System.Web.Services.Protocols.SoapHeader
        
        Private usernameTokenField As UPSSecurityUsernameToken
        
        Private serviceAccessTokenField As UPSSecurityServiceAccessToken
        
        '''<remarks/>
        Public Property UsernameToken() As UPSSecurityUsernameToken
            Get
                Return Me.usernameTokenField
            End Get
            Set
                Me.usernameTokenField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ServiceAccessToken() As UPSSecurityServiceAccessToken
            Get
                Return Me.serviceAccessTokenField
            End Get
            Set
                Me.serviceAccessTokenField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/UPSS/v1.0")>  _
    Partial Public Class UPSSecurityUsernameToken
        
        Private usernameField As String
        
        Private passwordField As String
        
        '''<remarks/>
        Public Property Username() As String
            Get
                Return Me.usernameField
            End Get
            Set
                Me.usernameField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Password() As String
            Get
                Return Me.passwordField
            End Get
            Set
                Me.passwordField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")>  _
    Partial Public Class CandidateType
        
        Private addressClassificationField As AddressClassificationType
        
        Private addressKeyFormatField As AddressKeyFormatType
        
        '''<remarks/>
        Public Property AddressClassification() As AddressClassificationType
            Get
                Return Me.addressClassificationField
            End Get
            Set
                Me.addressClassificationField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property AddressKeyFormat() As AddressKeyFormatType
            Get
                Return Me.addressKeyFormatField
            End Get
            Set
                Me.addressKeyFormatField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")>  _
    Partial Public Class AddressClassificationType
        
        Private codeField As String
        
        Private descriptionField As String
        
        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")>  _
    Partial Public Class AddressKeyFormatType
        
        Private consigneeNameField As String
        
        Private attentionNameField As String
        
        Private addressLineField() As String
        
        Private itemsField() As String
        
        Private itemsElementNameField() As ItemsChoiceType
        
        Private urbanizationField As String
        
        Private countryCodeField As String
        
        '''<remarks/>
        Public Property ConsigneeName() As String
            Get
                Return Me.consigneeNameField
            End Get
            Set
                Me.consigneeNameField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property AttentionName() As String
            Get
                Return Me.attentionNameField
            End Get
            Set
                Me.attentionNameField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressLine")>  _
        Public Property AddressLine() As String()
            Get
                Return Me.addressLineField
            End Get
            Set
                Me.addressLineField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PoliticalDivision1", GetType(String)),  _
         System.Xml.Serialization.XmlElementAttribute("PoliticalDivision2", GetType(String)),  _
         System.Xml.Serialization.XmlElementAttribute("PostcodeExtendedLow", GetType(String)),  _
         System.Xml.Serialization.XmlElementAttribute("PostcodePrimaryLow", GetType(String)),  _
         System.Xml.Serialization.XmlElementAttribute("Region", GetType(String)),  _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")>  _
        Public Property Items() As String()
            Get
                Return Me.itemsField
            End Get
            Set
                Me.itemsField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ItemsElementName"),  _
         System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property ItemsElementName() As ItemsChoiceType()
            Get
                Return Me.itemsElementNameField
            End Get
            Set
                Me.itemsElementNameField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Urbanization() As String
            Get
                Return Me.urbanizationField
            End Get
            Set
                Me.urbanizationField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CountryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set
                Me.countryCodeField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0", IncludeInSchema:=false)>  _
    Public Enum ItemsChoiceType
        
        '''<remarks/>
        PoliticalDivision1
        
        '''<remarks/>
        PoliticalDivision2
        
        '''<remarks/>
        PostcodeExtendedLow
        
        '''<remarks/>
        PostcodePrimaryLow
        
        '''<remarks/>
        Region
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/Common/v1.0")>  _
    Partial Public Class CodeDescriptionType
        
        Private codeField As String
        
        Private descriptionField As String
        
        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/Common/v1.0")>  _
    Partial Public Class ResponseType
        
        Private responseStatusField As CodeDescriptionType
        
        Private alertField() As CodeDescriptionType
        
        Private transactionReferenceField As TransactionReferenceType
        
        '''<remarks/>
        Public Property ResponseStatus() As CodeDescriptionType
            Get
                Return Me.responseStatusField
            End Get
            Set
                Me.responseStatusField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Alert")>  _
        Public Property Alert() As CodeDescriptionType()
            Get
                Return Me.alertField
            End Get
            Set
                Me.alertField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property TransactionReference() As TransactionReferenceType
            Get
                Return Me.transactionReferenceField
            End Get
            Set
                Me.transactionReferenceField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/Common/v1.0")>  _
    Partial Public Class TransactionReferenceType
        
        Private customerContextField As String
        
        Private transactionIdentifierField As String
        
        '''<remarks/>
        Public Property CustomerContext() As String
            Get
                Return Me.customerContextField
            End Get
            Set
                Me.customerContextField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property TransactionIdentifier() As String
            Get
                Return Me.transactionIdentifierField
            End Get
            Set
                Me.transactionIdentifierField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/Common/v1.0")>  _
    Partial Public Class RequestType
        
        Private requestOptionField() As String
        
        Private transactionReferenceField As TransactionReferenceType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RequestOption")>  _
        Public Property RequestOption() As String()
            Get
                Return Me.requestOptionField
            End Get
            Set
                Me.requestOptionField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property TransactionReference() As TransactionReferenceType
            Get
                Return Me.transactionReferenceField
            End Get
            Set
                Me.transactionReferenceField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/UPSS/v1.0")>  _
    Partial Public Class UPSSecurityServiceAccessToken
        
        Private accessLicenseNumberField As String
        
        '''<remarks/>
        Public Property AccessLicenseNumber() As String
            Get
                Return Me.accessLicenseNumberField
            End Get
            Set
                Me.accessLicenseNumberField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")>  _
    Partial Public Class XAVRequest
        
        Private requestField As RequestType
        
        Private regionalRequestIndicatorField As String
        
        Private maximumCandidateListSizeField As String
        
        Private addressKeyFormatField As AddressKeyFormatType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/Common/v1.0")>  _
        Public Property Request() As RequestType
            Get
                Return Me.requestField
            End Get
            Set
                Me.requestField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property RegionalRequestIndicator() As String
            Get
                Return Me.regionalRequestIndicatorField
            End Get
            Set
                Me.regionalRequestIndicatorField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property MaximumCandidateListSize() As String
            Get
                Return Me.maximumCandidateListSizeField
            End Get
            Set
                Me.maximumCandidateListSizeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property AddressKeyFormat() As AddressKeyFormatType
            Get
                Return Me.addressKeyFormatField
            End Get
            Set
                Me.addressKeyFormatField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0")>  _
    Partial Public Class XAVResponse
        
        Private responseField As ResponseType
        
        Private itemField As String
        
        Private itemElementNameField As ItemChoiceType
        
        Private addressClassificationField As AddressClassificationType
        
        Private candidateField() As CandidateType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/Common/v1.0")>  _
        Public Property Response() As ResponseType
            Get
                Return Me.responseField
            End Get
            Set
                Me.responseField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AmbiguousAddressIndicator", GetType(String)),  _
         System.Xml.Serialization.XmlElementAttribute("NoCandidatesIndicator", GetType(String)),  _
         System.Xml.Serialization.XmlElementAttribute("ValidAddressIndicator", GetType(String)),  _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")>  _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set
                Me.itemField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set
                Me.itemElementNameField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property AddressClassification() As AddressClassificationType
            Get
                Return Me.addressClassificationField
            End Get
            Set
                Me.addressClassificationField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Candidate")>  _
        Public Property Candidate() As CandidateType()
            Get
                Return Me.candidateField
            End Get
            Set
                Me.candidateField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.ups.com/XMLSchema/XOLTWS/xav/v1.0", IncludeInSchema:=false)>  _
    Public Enum ItemChoiceType
        
        '''<remarks/>
        AmbiguousAddressIndicator
        
        '''<remarks/>
        NoCandidatesIndicator
        
        '''<remarks/>
        ValidAddressIndicator
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")>  _
    Public Delegate Sub ProcessXAVCompletedEventHandler(ByVal sender As Object, ByVal e As ProcessXAVCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ProcessXAVCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As XAVResponse
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),XAVResponse)
            End Get
        End Property
    End Class
End Namespace
