Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Database
Imports Components
Imports Components.Core
Imports System.Web
Imports Utility

Namespace DataLayer

    Public Class SalesCreditMemoHeaderRow
        Inherits SalesCreditMemoHeaderRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemoId As Integer)
            MyBase.New(DB, MemoId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal No As String)
            MyBase.New(DB, No)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal MemoId As Integer) As SalesCreditMemoHeaderRow
            Dim row As SalesCreditMemoHeaderRow

            row = New SalesCreditMemoHeaderRow(DB, MemoId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal No As String) As SalesCreditMemoHeaderRow
            Dim row As SalesCreditMemoHeaderRow

            row = New SalesCreditMemoHeaderRow(DB, No)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MemoId As Integer)
            Dim row As SalesCreditMemoHeaderRow

            row = New SalesCreditMemoHeaderRow(DB, MemoId)
            row.Remove()
        End Sub

        Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
            Dim SQL As String = "select * from SalesCreditMemoHeader"
            If Not SortBy = String.Empty Then
                SortBy = Core.ProtectParam(SortBy)
                SortOrder = Core.ProtectParam(SortOrder)

                SQL &= " order by " & SortBy & " " & SortOrder
            End If
            Return DB.GetDataTable(SQL)
        End Function

        'Custom Methods
        Public Sub CopyFromNavision(ByVal DB As Database, ByVal n As NavisionSalesCreditMemoHeaderRow)
            Dim MemberId As Integer = DB.ExecuteScalar("select top 1 memberid from customer c inner join member m on c.customerid = m.customerid where c.CustomerNo = " & DB.Quote(n.Bill_To_Customer_No))
            If MemberId = Nothing Then Exit Sub

            Me.MemberId = MemberId
            Me.Amount = n.Amount
            Me.AmountIncludingVAT = n.Amount_Including_VAT
            Me.AppliestoDocNo = n.Applies_to_Doc_No
            Me.AppliestoDocType = n.Applies_to_Doc_Type
            Me.BalAccountNo = n.Bal_Account_No
            Me.BalAccountType = n.Bal_Account_Type
            Me.BillToAddress = n.Bill_To_Address
            Me.BillToAddress2 = n.Bill_To_Address_2
            Me.BillToCity = n.Bill_To_City
            Me.BillToContact = n.Bill_To_Contact
            Me.BillToContactNo = n.Bill_To_Contact_No
            Me.BillToCountryCode = n.Bill_To_Country_Code
            Me.BillToCounty = n.Bill_To_County
            Me.BillToCustomerNo = n.Bill_To_Customer_No
            Me.BillToName = n.Bill_To_Name
            Me.BillToName2 = n.Bill_To_Name_2
            Me.BillToPostCode = n.Bill_To_Post_Code
            Me.CampaignNo = n.Campaign_No
            Me.CurrencyCode = n.Currency_Code
            Me.CustomerDiscGroup = n.Customer_Disc_Group
            Me.CustomerPriceGroup = n.Customer_Price_Group
            Me.Documentdatetime = n.Document_datetime
            Me.Due = n.Due
            Me.ExternalDocumentNo = n.External_Document_No
            Me.InvoiceDiscCode = n.Invoice_Disc_Code
            Me.JobNo = n.Job_No
            Me.LanguageCode = n.Language_Code
            Me.LocationCode = n.Location_Code
            Me.No = n.No
            Me.PaymentDiscountPercent = n.Payment_Discount_Percent
            Me.PaymentMethodCode = n.Payment_Method_Code
            Me.PaymentTermsCode = n.Payment_Terms_Code
            Me.PmtDiscount = n.Pmt_Discount
            Me.Posting = n.Posting
            Me.PostingDescription = n.Posting_Description
            Me.PricesIncludingVAT = n.Prices_Including_VAT
            Me.ReasonCode = n.Reason_Code
            Me.ResponsibilityCenter = n.Responsibility_Center
            Me.SalespersonCode = n.Salesperson_Code
            Me.SellToAddress = n.Sell_To_Address
            Me.SellToAddress2 = n.Sell_To_Address_2
            Me.SellToCity = n.Sell_To_City
            Me.SellToContact = n.Sell_To_Contact
            Me.SellToContactNo = n.Sell_To_Contact_No
            Me.SellToCountryCode = n.Sell_To_Country_Code
            Me.SellToCounty = n.Sell_To_County
            Me.SellToCustomerName = n.Sell_To_Customer_Name
            Me.SellToCustomerName2 = n.Sell_To_Customer_Name_2
            Me.SellToCustomerNo = n.Sell_To_Customer_No
            Me.SellToPostCode = n.Sell_To_Post_Code
            Me.ShipToAddress = n.Ship_To_Address
            Me.ShipToAddress2 = n.Ship_To_Address_2
            Me.ShipToCity = n.Ship_To_City
            Me.ShipToCode = n.Ship_To_Code
            Me.ShipToContact = n.Ship_To_Contact
            Me.ShipToCountryCode = n.Ship_To_Country_Code
            Me.ShipToCounty = n.Ship_To_County
            Me.ShipToName = n.Ship_To_Name
            Me.ShipToName2 = n.Ship_To_Name_2
            Me.ShipToPostCode = n.Ship_To_Post_Code
            Me.Shipment = n.Shipment
            Me.ShipmentMethodCode = n.Shipment_Method_Code
            Me.SourceCode = n.Source_Code
            Me.TaxAreaCode = n.Tax_Area_Code
            Me.TaxLiable = n.Tax_Liable
            Me.TransactionType = n.Transaction_Type
            Me.TransportMethod = n.Transport_Method
            Me.VATRegistrationNo = n.VAT_Registration_No
            Me.YourReference = n.Your_Reference

            If Me.MemoId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub
        Public Shared Function GetAmount(ByVal DB As Database, ByVal OrderNo As String) As Double
            Return DB.ExecuteScalar("Select Amount From SalesCreditMemoHeader Where No = '" & OrderNo & "'")
        End Function
        Public Function GetLineItems() As DataTable
            Return DB.GetDataTable("select * from salescreditmemoline where type='item' and memoid = " & MemoId & " order by [lineno]")
        End Function
        Public Shared Function SalesCreditMemoHeaderIDByNo(ByVal DB As Database, ByVal no As String) As Integer
            Try
                Dim result As Integer = CInt(DB.ExecuteScalar("Select MemoId from SalesCreditMemoHeader where No='" & no & "'"))
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
    End Class

    Public MustInherit Class SalesCreditMemoHeaderRowBase
        Private m_DB As Database
        Private m_MemoId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_SellToCustomerNo As String = Nothing
        Private m_No As String = Nothing
        Private m_BillToCustomerNo As String = Nothing
        Private m_BillToName As String = Nothing
        Private m_BillToName2 As String = Nothing
        Private m_BillToAddress As String = Nothing
        Private m_BillToAddress2 As String = Nothing
        Private m_BillToCity As String = Nothing
        Private m_BillToContact As String = Nothing
        Private m_YourReference As String = Nothing
        Private m_ShipToCode As String = Nothing
        Private m_ShipToName As String = Nothing
        Private m_ShipToName2 As String = Nothing
        Private m_ShipToAddress As String = Nothing
        Private m_ShipToAddress2 As String = Nothing
        Private m_ShipToCity As String = Nothing
        Private m_ShipToContact As String = Nothing
        Private m_Posting As DateTime = Nothing
        Private m_Shipment As DateTime = Nothing
        Private m_PostingDescription As String = Nothing
        Private m_PaymentTermsCode As String = Nothing
        Private m_Due As DateTime = Nothing
        Private m_PaymentDiscountPercent As Double = Nothing
        Private m_PmtDiscount As DateTime = Nothing
        Private m_ShipmentMethodCode As String = Nothing
        Private m_LocationCode As String = Nothing
        Private m_CurrencyCode As String = Nothing
        Private m_CustomerPriceGroup As String = Nothing
        Private m_PricesIncludingVAT As String = Nothing
        Private m_InvoiceDiscCode As String = Nothing
        Private m_CustomerDiscGroup As String = Nothing
        Private m_LanguageCode As String = Nothing
        Private m_SalespersonCode As String = Nothing
        Private m_AppliestoDocType As String = Nothing
        Private m_AppliestoDocNo As String = Nothing
        Private m_BalAccountNo As String = Nothing
        Private m_JobNo As String = Nothing
        Private m_Amount As Double = Nothing
        Private m_AmountIncludingVAT As Double = Nothing
        Private m_VATRegistrationNo As String = Nothing
        Private m_ReasonCode As String = Nothing
        Private m_TransactionType As String = Nothing
        Private m_TransportMethod As String = Nothing
        Private m_SellToCustomerName As String = Nothing
        Private m_SellToCustomerName2 As String = Nothing
        Private m_SellToAddress As String = Nothing
        Private m_SellToAddress2 As String = Nothing
        Private m_SellToCity As String = Nothing
        Private m_SellToContact As String = Nothing
        Private m_BillToPostCode As String = Nothing
        Private m_BillToCounty As String = Nothing
        Private m_BillToCountryCode As String = Nothing
        Private m_SellToPostCode As String = Nothing
        Private m_SellToCounty As String = Nothing
        Private m_SellToCountryCode As String = Nothing
        Private m_ShipToPostCode As String = Nothing
        Private m_ShipToCounty As String = Nothing
        Private m_ShipToCountryCode As String = Nothing
        Private m_BalAccountType As String = Nothing
        Private m_Documentdatetime As DateTime = Nothing
        Private m_ExternalDocumentNo As String = Nothing
        Private m_PaymentMethodCode As String = Nothing
        Private m_UserID As String = Nothing
        Private m_SourceCode As String = Nothing
        Private m_TaxAreaCode As String = Nothing
        Private m_TaxLiable As String = Nothing
        Private m_CampaignNo As String = Nothing
        Private m_SellToContactNo As String = Nothing
        Private m_BillToContactNo As String = Nothing
        Private m_ResponsibilityCenter As String = Nothing


        Public Property MemoId() As Integer
            Get
                Return m_MemoId
            End Get
            Set(ByVal Value As Integer)
                m_MemoId = Value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property SellToCustomerNo() As String
            Get
                Return m_SellToCustomerNo
            End Get
            Set(ByVal Value As String)
                m_SellToCustomerNo = Value
            End Set
        End Property

        Public Property No() As String
            Get
                Return m_No
            End Get
            Set(ByVal Value As String)
                m_No = Value
            End Set
        End Property

        Public Property BillToCustomerNo() As String
            Get
                Return m_BillToCustomerNo
            End Get
            Set(ByVal Value As String)
                m_BillToCustomerNo = Value
            End Set
        End Property

        Public Property BillToName() As String
            Get
                Return m_BillToName
            End Get
            Set(ByVal Value As String)
                m_BillToName = Value
            End Set
        End Property

        Public Property BillToName2() As String
            Get
                Return m_BillToName2
            End Get
            Set(ByVal Value As String)
                m_BillToName2 = Value
            End Set
        End Property

        Public Property BillToAddress() As String
            Get
                Return m_BillToAddress
            End Get
            Set(ByVal Value As String)
                m_BillToAddress = Value
            End Set
        End Property

        Public Property BillToAddress2() As String
            Get
                Return m_BillToAddress2
            End Get
            Set(ByVal Value As String)
                m_BillToAddress2 = Value
            End Set
        End Property

        Public Property BillToCity() As String
            Get
                Return m_BillToCity
            End Get
            Set(ByVal Value As String)
                m_BillToCity = Value
            End Set
        End Property

        Public Property BillToContact() As String
            Get
                Return m_BillToContact
            End Get
            Set(ByVal Value As String)
                m_BillToContact = Value
            End Set
        End Property

        Public Property YourReference() As String
            Get
                Return m_YourReference
            End Get
            Set(ByVal Value As String)
                m_YourReference = Value
            End Set
        End Property

        Public Property ShipToCode() As String
            Get
                Return m_ShipToCode
            End Get
            Set(ByVal Value As String)
                m_ShipToCode = Value
            End Set
        End Property

        Public Property ShipToName() As String
            Get
                Return m_ShipToName
            End Get
            Set(ByVal Value As String)
                m_ShipToName = Value
            End Set
        End Property

        Public Property ShipToName2() As String
            Get
                Return m_ShipToName2
            End Get
            Set(ByVal Value As String)
                m_ShipToName2 = Value
            End Set
        End Property

        Public Property ShipToAddress() As String
            Get
                Return m_ShipToAddress
            End Get
            Set(ByVal Value As String)
                m_ShipToAddress = Value
            End Set
        End Property

        Public Property ShipToAddress2() As String
            Get
                Return m_ShipToAddress2
            End Get
            Set(ByVal Value As String)
                m_ShipToAddress2 = Value
            End Set
        End Property

        Public Property ShipToCity() As String
            Get
                Return m_ShipToCity
            End Get
            Set(ByVal Value As String)
                m_ShipToCity = Value
            End Set
        End Property

        Public Property ShipToContact() As String
            Get
                Return m_ShipToContact
            End Get
            Set(ByVal Value As String)
                m_ShipToContact = Value
            End Set
        End Property

        Public Property Posting() As DateTime
            Get
                Return m_Posting
            End Get
            Set(ByVal Value As DateTime)
                m_Posting = Value
            End Set
        End Property

        Public Property Shipment() As DateTime
            Get
                Return m_Shipment
            End Get
            Set(ByVal Value As DateTime)
                m_Shipment = Value
            End Set
        End Property

        Public Property PostingDescription() As String
            Get
                Return m_PostingDescription
            End Get
            Set(ByVal Value As String)
                m_PostingDescription = Value
            End Set
        End Property

        Public Property PaymentTermsCode() As String
            Get
                Return m_PaymentTermsCode
            End Get
            Set(ByVal Value As String)
                m_PaymentTermsCode = Value
            End Set
        End Property

        Public Property Due() As DateTime
            Get
                Return m_Due
            End Get
            Set(ByVal Value As DateTime)
                m_Due = Value
            End Set
        End Property

        Public Property PaymentDiscountPercent() As Double
            Get
                Return m_PaymentDiscountPercent
            End Get
            Set(ByVal Value As Double)
                m_PaymentDiscountPercent = Value
            End Set
        End Property

        Public Property PmtDiscount() As DateTime
            Get
                Return m_PmtDiscount
            End Get
            Set(ByVal Value As DateTime)
                m_PmtDiscount = Value
            End Set
        End Property

        Public Property ShipmentMethodCode() As String
            Get
                Return m_ShipmentMethodCode
            End Get
            Set(ByVal Value As String)
                m_ShipmentMethodCode = Value
            End Set
        End Property

        Public Property LocationCode() As String
            Get
                Return m_LocationCode
            End Get
            Set(ByVal Value As String)
                m_LocationCode = Value
            End Set
        End Property

        Public Property CurrencyCode() As String
            Get
                Return m_CurrencyCode
            End Get
            Set(ByVal Value As String)
                m_CurrencyCode = Value
            End Set
        End Property

        Public Property CustomerPriceGroup() As String
            Get
                Return m_CustomerPriceGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerPriceGroup = Value
            End Set
        End Property

        Public Property PricesIncludingVAT() As String
            Get
                Return m_PricesIncludingVAT
            End Get
            Set(ByVal Value As String)
                m_PricesIncludingVAT = Value
            End Set
        End Property

        Public Property InvoiceDiscCode() As String
            Get
                Return m_InvoiceDiscCode
            End Get
            Set(ByVal Value As String)
                m_InvoiceDiscCode = Value
            End Set
        End Property

        Public Property CustomerDiscGroup() As String
            Get
                Return m_CustomerDiscGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerDiscGroup = Value
            End Set
        End Property

        Public Property LanguageCode() As String
            Get
                Return m_LanguageCode
            End Get
            Set(ByVal Value As String)
                m_LanguageCode = Value
            End Set
        End Property

        Public Property SalespersonCode() As String
            Get
                Return m_SalespersonCode
            End Get
            Set(ByVal Value As String)
                m_SalespersonCode = Value
            End Set
        End Property

        Public Property AppliestoDocType() As String
            Get
                Return m_AppliestoDocType
            End Get
            Set(ByVal Value As String)
                m_AppliestoDocType = Value
            End Set
        End Property

        Public Property AppliestoDocNo() As String
            Get
                Return m_AppliestoDocNo
            End Get
            Set(ByVal Value As String)
                m_AppliestoDocNo = Value
            End Set
        End Property

        Public Property BalAccountNo() As String
            Get
                Return m_BalAccountNo
            End Get
            Set(ByVal Value As String)
                m_BalAccountNo = Value
            End Set
        End Property

        Public Property JobNo() As String
            Get
                Return m_JobNo
            End Get
            Set(ByVal Value As String)
                m_JobNo = Value
            End Set
        End Property

        Public Property Amount() As Double
            Get
                Return m_Amount
            End Get
            Set(ByVal Value As Double)
                m_Amount = Value
            End Set
        End Property

        Public Property AmountIncludingVAT() As Double
            Get
                Return m_AmountIncludingVAT
            End Get
            Set(ByVal Value As Double)
                m_AmountIncludingVAT = Value
            End Set
        End Property

        Public Property VATRegistrationNo() As String
            Get
                Return m_VATRegistrationNo
            End Get
            Set(ByVal Value As String)
                m_VATRegistrationNo = Value
            End Set
        End Property

        Public Property ReasonCode() As String
            Get
                Return m_ReasonCode
            End Get
            Set(ByVal Value As String)
                m_ReasonCode = Value
            End Set
        End Property

        Public Property TransactionType() As String
            Get
                Return m_TransactionType
            End Get
            Set(ByVal Value As String)
                m_TransactionType = Value
            End Set
        End Property

        Public Property TransportMethod() As String
            Get
                Return m_TransportMethod
            End Get
            Set(ByVal Value As String)
                m_TransportMethod = Value
            End Set
        End Property

        Public Property SellToCustomerName() As String
            Get
                Return m_SellToCustomerName
            End Get
            Set(ByVal Value As String)
                m_SellToCustomerName = Value
            End Set
        End Property

        Public Property SellToCustomerName2() As String
            Get
                Return m_SellToCustomerName2
            End Get
            Set(ByVal Value As String)
                m_SellToCustomerName2 = Value
            End Set
        End Property

        Public Property SellToAddress() As String
            Get
                Return m_SellToAddress
            End Get
            Set(ByVal Value As String)
                m_SellToAddress = Value
            End Set
        End Property

        Public Property SellToAddress2() As String
            Get
                Return m_SellToAddress2
            End Get
            Set(ByVal Value As String)
                m_SellToAddress2 = Value
            End Set
        End Property

        Public Property SellToCity() As String
            Get
                Return m_SellToCity
            End Get
            Set(ByVal Value As String)
                m_SellToCity = Value
            End Set
        End Property

        Public Property SellToContact() As String
            Get
                Return m_SellToContact
            End Get
            Set(ByVal Value As String)
                m_SellToContact = Value
            End Set
        End Property

        Public Property BillToPostCode() As String
            Get
                Return m_BillToPostCode
            End Get
            Set(ByVal Value As String)
                m_BillToPostCode = Value
            End Set
        End Property

        Public Property BillToCounty() As String
            Get
                Return m_BillToCounty
            End Get
            Set(ByVal Value As String)
                m_BillToCounty = Value
            End Set
        End Property

        Public Property BillToCountryCode() As String
            Get
                Return m_BillToCountryCode
            End Get
            Set(ByVal Value As String)
                m_BillToCountryCode = Value
            End Set
        End Property

        Public Property SellToPostCode() As String
            Get
                Return m_SellToPostCode
            End Get
            Set(ByVal Value As String)
                m_SellToPostCode = Value
            End Set
        End Property

        Public Property SellToCounty() As String
            Get
                Return m_SellToCounty
            End Get
            Set(ByVal Value As String)
                m_SellToCounty = Value
            End Set
        End Property

        Public Property SellToCountryCode() As String
            Get
                Return m_SellToCountryCode
            End Get
            Set(ByVal Value As String)
                m_SellToCountryCode = Value
            End Set
        End Property

        Public Property ShipToPostCode() As String
            Get
                Return m_ShipToPostCode
            End Get
            Set(ByVal Value As String)
                m_ShipToPostCode = Value
            End Set
        End Property

        Public Property ShipToCounty() As String
            Get
                Return m_ShipToCounty
            End Get
            Set(ByVal Value As String)
                m_ShipToCounty = Value
            End Set
        End Property

        Public Property ShipToCountryCode() As String
            Get
                Return m_ShipToCountryCode
            End Get
            Set(ByVal Value As String)
                m_ShipToCountryCode = Value
            End Set
        End Property

        Public Property BalAccountType() As String
            Get
                Return m_BalAccountType
            End Get
            Set(ByVal Value As String)
                m_BalAccountType = Value
            End Set
        End Property

        Public Property Documentdatetime() As DateTime
            Get
                Return m_Documentdatetime
            End Get
            Set(ByVal Value As DateTime)
                m_Documentdatetime = Value
            End Set
        End Property

        Public Property ExternalDocumentNo() As String
            Get
                Return m_ExternalDocumentNo
            End Get
            Set(ByVal Value As String)
                m_ExternalDocumentNo = Value
            End Set
        End Property

        Public Property PaymentMethodCode() As String
            Get
                Return m_PaymentMethodCode
            End Get
            Set(ByVal Value As String)
                m_PaymentMethodCode = Value
            End Set
        End Property

        Public Property UserID() As String
            Get
                Return m_UserID
            End Get
            Set(ByVal Value As String)
                m_UserID = Value
            End Set
        End Property

        Public Property SourceCode() As String
            Get
                Return m_SourceCode
            End Get
            Set(ByVal Value As String)
                m_SourceCode = Value
            End Set
        End Property

        Public Property TaxAreaCode() As String
            Get
                Return m_TaxAreaCode
            End Get
            Set(ByVal Value As String)
                m_TaxAreaCode = Value
            End Set
        End Property

        Public Property TaxLiable() As String
            Get
                Return m_TaxLiable
            End Get
            Set(ByVal Value As String)
                m_TaxLiable = Value
            End Set
        End Property

        Public Property CampaignNo() As String
            Get
                Return m_CampaignNo
            End Get
            Set(ByVal Value As String)
                m_CampaignNo = Value
            End Set
        End Property

        Public Property SellToContactNo() As String
            Get
                Return m_SellToContactNo
            End Get
            Set(ByVal Value As String)
                m_SellToContactNo = Value
            End Set
        End Property

        Public Property BillToContactNo() As String
            Get
                Return m_BillToContactNo
            End Get
            Set(ByVal Value As String)
                m_BillToContactNo = Value
            End Set
        End Property

        Public Property ResponsibilityCenter() As String
            Get
                Return m_ResponsibilityCenter
            End Get
            Set(ByVal Value As String)
                m_ResponsibilityCenter = Value
            End Set
        End Property


        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemoId As Integer)
            m_DB = DB
            m_MemoId = MemoId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal No As String)
            m_DB = DB
            m_No = No
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM SalesCreditMemoHeader WHERE " & IIf(MemoId = Nothing, "[No] = " & DB.Quote(No), "MemoId = " & DB.Number(MemoId))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_MemoId = Convert.ToInt32(r.Item("MemoId"))
            m_MemberId = Convert.ToInt32(r.Item("MemberId"))
            If IsDBNull(r.Item("SellToCustomerNo")) Then
                m_SellToCustomerNo = Nothing
            Else
                m_SellToCustomerNo = Convert.ToString(r.Item("SellToCustomerNo"))
            End If
            If IsDBNull(r.Item("No")) Then
                m_No = Nothing
            Else
                m_No = Convert.ToString(r.Item("No"))
            End If
            If IsDBNull(r.Item("BillToCustomerNo")) Then
                m_BillToCustomerNo = Nothing
            Else
                m_BillToCustomerNo = Convert.ToString(r.Item("BillToCustomerNo"))
            End If
            If IsDBNull(r.Item("BillToName")) Then
                m_BillToName = Nothing
            Else
                m_BillToName = Convert.ToString(r.Item("BillToName"))
            End If
            If IsDBNull(r.Item("BillToName2")) Then
                m_BillToName2 = Nothing
            Else
                m_BillToName2 = Convert.ToString(r.Item("BillToName2"))
            End If
            If IsDBNull(r.Item("BillToAddress")) Then
                m_BillToAddress = Nothing
            Else
                m_BillToAddress = Convert.ToString(r.Item("BillToAddress"))
            End If
            If IsDBNull(r.Item("BillToAddress2")) Then
                m_BillToAddress2 = Nothing
            Else
                m_BillToAddress2 = Convert.ToString(r.Item("BillToAddress2"))
            End If
            If IsDBNull(r.Item("BillToCity")) Then
                m_BillToCity = Nothing
            Else
                m_BillToCity = Convert.ToString(r.Item("BillToCity"))
            End If
            If IsDBNull(r.Item("BillToContact")) Then
                m_BillToContact = Nothing
            Else
                m_BillToContact = Convert.ToString(r.Item("BillToContact"))
            End If
            If IsDBNull(r.Item("YourReference")) Then
                m_YourReference = Nothing
            Else
                m_YourReference = Convert.ToString(r.Item("YourReference"))
            End If
            If IsDBNull(r.Item("ShipToCode")) Then
                m_ShipToCode = Nothing
            Else
                m_ShipToCode = Convert.ToString(r.Item("ShipToCode"))
            End If
            If IsDBNull(r.Item("ShipToName")) Then
                m_ShipToName = Nothing
            Else
                m_ShipToName = Convert.ToString(r.Item("ShipToName"))
            End If
            If IsDBNull(r.Item("ShipToName2")) Then
                m_ShipToName2 = Nothing
            Else
                m_ShipToName2 = Convert.ToString(r.Item("ShipToName2"))
            End If
            If IsDBNull(r.Item("ShipToAddress")) Then
                m_ShipToAddress = Nothing
            Else
                m_ShipToAddress = Convert.ToString(r.Item("ShipToAddress"))
            End If
            If IsDBNull(r.Item("ShipToAddress2")) Then
                m_ShipToAddress2 = Nothing
            Else
                m_ShipToAddress2 = Convert.ToString(r.Item("ShipToAddress2"))
            End If
            If IsDBNull(r.Item("ShipToCity")) Then
                m_ShipToCity = Nothing
            Else
                m_ShipToCity = Convert.ToString(r.Item("ShipToCity"))
            End If
            If IsDBNull(r.Item("ShipToContact")) Then
                m_ShipToContact = Nothing
            Else
                m_ShipToContact = Convert.ToString(r.Item("ShipToContact"))
            End If
            If IsDBNull(r.Item("Posting")) Then
                m_Posting = Nothing
            Else
                m_Posting = Convert.ToDateTime(r.Item("Posting"))
            End If
            If IsDBNull(r.Item("Shipment")) Then
                m_Shipment = Nothing
            Else
                m_Shipment = Convert.ToDateTime(r.Item("Shipment"))
            End If
            If IsDBNull(r.Item("PostingDescription")) Then
                m_PostingDescription = Nothing
            Else
                m_PostingDescription = Convert.ToString(r.Item("PostingDescription"))
            End If
            If IsDBNull(r.Item("PaymentTermsCode")) Then
                m_PaymentTermsCode = Nothing
            Else
                m_PaymentTermsCode = Convert.ToString(r.Item("PaymentTermsCode"))
            End If
            If IsDBNull(r.Item("Due")) Then
                m_Due = Nothing
            Else
                m_Due = Convert.ToDateTime(r.Item("Due"))
            End If
            If IsDBNull(r.Item("PaymentDiscountPercent")) Then
                m_PaymentDiscountPercent = Nothing
            Else
                m_PaymentDiscountPercent = Convert.ToDouble(r.Item("PaymentDiscountPercent"))
            End If
            If IsDBNull(r.Item("PmtDiscount")) Then
                m_PmtDiscount = Nothing
            Else
                m_PmtDiscount = Convert.ToDateTime(r.Item("PmtDiscount"))
            End If
            If IsDBNull(r.Item("ShipmentMethodCode")) Then
                m_ShipmentMethodCode = Nothing
            Else
                m_ShipmentMethodCode = Convert.ToString(r.Item("ShipmentMethodCode"))
            End If
            If IsDBNull(r.Item("LocationCode")) Then
                m_LocationCode = Nothing
            Else
                m_LocationCode = Convert.ToString(r.Item("LocationCode"))
            End If
            If IsDBNull(r.Item("CurrencyCode")) Then
                m_CurrencyCode = Nothing
            Else
                m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
            End If
            If IsDBNull(r.Item("CustomerPriceGroup")) Then
                m_CustomerPriceGroup = Nothing
            Else
                m_CustomerPriceGroup = Convert.ToString(r.Item("CustomerPriceGroup"))
            End If
            If IsDBNull(r.Item("PricesIncludingVAT")) Then
                m_PricesIncludingVAT = Nothing
            Else
                m_PricesIncludingVAT = Convert.ToString(r.Item("PricesIncludingVAT"))
            End If
            If IsDBNull(r.Item("InvoiceDiscCode")) Then
                m_InvoiceDiscCode = Nothing
            Else
                m_InvoiceDiscCode = Convert.ToString(r.Item("InvoiceDiscCode"))
            End If
            If IsDBNull(r.Item("CustomerDiscGroup")) Then
                m_CustomerDiscGroup = Nothing
            Else
                m_CustomerDiscGroup = Convert.ToString(r.Item("CustomerDiscGroup"))
            End If
            If IsDBNull(r.Item("LanguageCode")) Then
                m_LanguageCode = Nothing
            Else
                m_LanguageCode = Convert.ToString(r.Item("LanguageCode"))
            End If
            If IsDBNull(r.Item("SalespersonCode")) Then
                m_SalespersonCode = Nothing
            Else
                m_SalespersonCode = Convert.ToString(r.Item("SalespersonCode"))
            End If
            If IsDBNull(r.Item("AppliestoDocType")) Then
                m_AppliestoDocType = Nothing
            Else
                m_AppliestoDocType = Convert.ToString(r.Item("AppliestoDocType"))
            End If
            If IsDBNull(r.Item("AppliestoDocNo")) Then
                m_AppliestoDocNo = Nothing
            Else
                m_AppliestoDocNo = Convert.ToString(r.Item("AppliestoDocNo"))
            End If
            If IsDBNull(r.Item("BalAccountNo")) Then
                m_BalAccountNo = Nothing
            Else
                m_BalAccountNo = Convert.ToString(r.Item("BalAccountNo"))
            End If
            If IsDBNull(r.Item("JobNo")) Then
                m_JobNo = Nothing
            Else
                m_JobNo = Convert.ToString(r.Item("JobNo"))
            End If
            If IsDBNull(r.Item("Amount")) Then
                m_Amount = Nothing
            Else
                m_Amount = Convert.ToDouble(r.Item("Amount"))
            End If
            If IsDBNull(r.Item("AmountIncludingVAT")) Then
                m_AmountIncludingVAT = Nothing
            Else
                m_AmountIncludingVAT = Convert.ToDouble(r.Item("AmountIncludingVAT"))
            End If
            If IsDBNull(r.Item("VATRegistrationNo")) Then
                m_VATRegistrationNo = Nothing
            Else
                m_VATRegistrationNo = Convert.ToString(r.Item("VATRegistrationNo"))
            End If
            If IsDBNull(r.Item("ReasonCode")) Then
                m_ReasonCode = Nothing
            Else
                m_ReasonCode = Convert.ToString(r.Item("ReasonCode"))
            End If
            If IsDBNull(r.Item("TransactionType")) Then
                m_TransactionType = Nothing
            Else
                m_TransactionType = Convert.ToString(r.Item("TransactionType"))
            End If
            If IsDBNull(r.Item("TransportMethod")) Then
                m_TransportMethod = Nothing
            Else
                m_TransportMethod = Convert.ToString(r.Item("TransportMethod"))
            End If
            If IsDBNull(r.Item("SellToCustomerName")) Then
                m_SellToCustomerName = Nothing
            Else
                m_SellToCustomerName = Convert.ToString(r.Item("SellToCustomerName"))
            End If
            If IsDBNull(r.Item("SellToCustomerName2")) Then
                m_SellToCustomerName2 = Nothing
            Else
                m_SellToCustomerName2 = Convert.ToString(r.Item("SellToCustomerName2"))
            End If
            If IsDBNull(r.Item("SellToAddress")) Then
                m_SellToAddress = Nothing
            Else
                m_SellToAddress = Convert.ToString(r.Item("SellToAddress"))
            End If
            If IsDBNull(r.Item("SellToAddress2")) Then
                m_SellToAddress2 = Nothing
            Else
                m_SellToAddress2 = Convert.ToString(r.Item("SellToAddress2"))
            End If
            If IsDBNull(r.Item("SellToCity")) Then
                m_SellToCity = Nothing
            Else
                m_SellToCity = Convert.ToString(r.Item("SellToCity"))
            End If
            If IsDBNull(r.Item("SellToContact")) Then
                m_SellToContact = Nothing
            Else
                m_SellToContact = Convert.ToString(r.Item("SellToContact"))
            End If
            If IsDBNull(r.Item("BillToPostCode")) Then
                m_BillToPostCode = Nothing
            Else
                m_BillToPostCode = Convert.ToString(r.Item("BillToPostCode"))
            End If
            If IsDBNull(r.Item("BillToCounty")) Then
                m_BillToCounty = Nothing
            Else
                m_BillToCounty = Convert.ToString(r.Item("BillToCounty"))
            End If
            If IsDBNull(r.Item("BillToCountryCode")) Then
                m_BillToCountryCode = Nothing
            Else
                m_BillToCountryCode = Convert.ToString(r.Item("BillToCountryCode"))
            End If
            If IsDBNull(r.Item("SellToPostCode")) Then
                m_SellToPostCode = Nothing
            Else
                m_SellToPostCode = Convert.ToString(r.Item("SellToPostCode"))
            End If
            If IsDBNull(r.Item("SellToCounty")) Then
                m_SellToCounty = Nothing
            Else
                m_SellToCounty = Convert.ToString(r.Item("SellToCounty"))
            End If
            If IsDBNull(r.Item("SellToCountryCode")) Then
                m_SellToCountryCode = Nothing
            Else
                m_SellToCountryCode = Convert.ToString(r.Item("SellToCountryCode"))
            End If
            If IsDBNull(r.Item("ShipToPostCode")) Then
                m_ShipToPostCode = Nothing
            Else
                m_ShipToPostCode = Convert.ToString(r.Item("ShipToPostCode"))
            End If
            If IsDBNull(r.Item("ShipToCounty")) Then
                m_ShipToCounty = Nothing
            Else
                m_ShipToCounty = Convert.ToString(r.Item("ShipToCounty"))
            End If
            If IsDBNull(r.Item("ShipToCountryCode")) Then
                m_ShipToCountryCode = Nothing
            Else
                m_ShipToCountryCode = Convert.ToString(r.Item("ShipToCountryCode"))
            End If
            If IsDBNull(r.Item("BalAccountType")) Then
                m_BalAccountType = Nothing
            Else
                m_BalAccountType = Convert.ToString(r.Item("BalAccountType"))
            End If
            If IsDBNull(r.Item("Documentdatetime")) Then
                m_Documentdatetime = Nothing
            Else
                m_Documentdatetime = Convert.ToDateTime(r.Item("Documentdatetime"))
            End If
            If IsDBNull(r.Item("ExternalDocumentNo")) Then
                m_ExternalDocumentNo = Nothing
            Else
                m_ExternalDocumentNo = Convert.ToString(r.Item("ExternalDocumentNo"))
            End If
            If IsDBNull(r.Item("PaymentMethodCode")) Then
                m_PaymentMethodCode = Nothing
            Else
                m_PaymentMethodCode = Convert.ToString(r.Item("PaymentMethodCode"))
            End If
            If IsDBNull(r.Item("UserID")) Then
                m_UserID = Nothing
            Else
                m_UserID = Convert.ToString(r.Item("UserID"))
            End If
            If IsDBNull(r.Item("SourceCode")) Then
                m_SourceCode = Nothing
            Else
                m_SourceCode = Convert.ToString(r.Item("SourceCode"))
            End If
            If IsDBNull(r.Item("TaxAreaCode")) Then
                m_TaxAreaCode = Nothing
            Else
                m_TaxAreaCode = Convert.ToString(r.Item("TaxAreaCode"))
            End If
            If IsDBNull(r.Item("TaxLiable")) Then
                m_TaxLiable = Nothing
            Else
                m_TaxLiable = Convert.ToString(r.Item("TaxLiable"))
            End If
            If IsDBNull(r.Item("CampaignNo")) Then
                m_CampaignNo = Nothing
            Else
                m_CampaignNo = Convert.ToString(r.Item("CampaignNo"))
            End If
            If IsDBNull(r.Item("SellToContactNo")) Then
                m_SellToContactNo = Nothing
            Else
                m_SellToContactNo = Convert.ToString(r.Item("SellToContactNo"))
            End If
            If IsDBNull(r.Item("BillToContactNo")) Then
                m_BillToContactNo = Nothing
            Else
                m_BillToContactNo = Convert.ToString(r.Item("BillToContactNo"))
            End If
            If IsDBNull(r.Item("ResponsibilityCenter")) Then
                m_ResponsibilityCenter = Nothing
            Else
                m_ResponsibilityCenter = Convert.ToString(r.Item("ResponsibilityCenter"))
            End If
        End Sub 'Load

        Public Function InsertSassi(ByVal db As Database) As Integer
            Dim SQL As String
            Dim str As String = MemberId & SellToCustomerNo & No & BillToCustomerNo & BillToName & BillToName2 & BillToAddress & BillToAddress2 & BillToCity & ShipToName & ShipToName2 & ShipToAddress & ShipToAddress2 & ShipToCity & ShipmentMethodCode & Amount

            SQL = " INSERT INTO SalesCreditMemoHeader (" _
             & " MemberId" _
             & ",SellToCustomerNo" _
             & ",No" _
             & ",BillToCustomerNo" _
             & ",Amount" _
             & ") VALUES (" _
             & db.Number(MemberId) _
             & "," & db.Quote(SellToCustomerNo) _
             & "," & db.Quote(No) _
             & "," & db.Quote(BillToCustomerNo) _
             & "," & db.Number(Amount) _
             & ")"

            MemoId = db.InsertSQL(SQL)

            Return MemoId
        End Function
        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO SalesCreditMemoHeader (" _
             & " MemberId" _
             & ",SellToCustomerNo" _
             & ",No" _
             & ",BillToCustomerNo" _
             & ",BillToName" _
             & ",BillToName2" _
             & ",BillToAddress" _
             & ",BillToAddress2" _
             & ",BillToCity" _
             & ",BillToContact" _
             & ",YourReference" _
             & ",ShipToCode" _
             & ",ShipToName" _
             & ",ShipToName2" _
             & ",ShipToAddress" _
             & ",ShipToAddress2" _
             & ",ShipToCity" _
             & ",ShipToContact" _
             & ",Posting" _
             & ",Shipment" _
             & ",PostingDescription" _
             & ",PaymentTermsCode" _
             & ",Due" _
             & ",PaymentDiscountPercent" _
             & ",PmtDiscount" _
             & ",ShipmentMethodCode" _
             & ",LocationCode" _
             & ",CurrencyCode" _
             & ",CustomerPriceGroup" _
             & ",PricesIncludingVAT" _
             & ",InvoiceDiscCode" _
             & ",CustomerDiscGroup" _
             & ",LanguageCode" _
             & ",SalespersonCode" _
             & ",AppliestoDocType" _
             & ",AppliestoDocNo" _
             & ",BalAccountNo" _
             & ",JobNo" _
             & ",Amount" _
             & ",AmountIncludingVAT" _
             & ",VATRegistrationNo" _
             & ",ReasonCode" _
             & ",TransactionType" _
             & ",TransportMethod" _
             & ",SellToCustomerName" _
             & ",SellToCustomerName2" _
             & ",SellToAddress" _
             & ",SellToAddress2" _
             & ",SellToCity" _
             & ",SellToContact" _
             & ",BillToPostCode" _
             & ",BillToCounty" _
             & ",BillToCountryCode" _
             & ",SellToPostCode" _
             & ",SellToCounty" _
             & ",SellToCountryCode" _
             & ",ShipToPostCode" _
             & ",ShipToCounty" _
             & ",ShipToCountryCode" _
             & ",BalAccountType" _
             & ",Documentdatetime" _
             & ",ExternalDocumentNo" _
             & ",PaymentMethodCode" _
             & ",UserID" _
             & ",SourceCode" _
             & ",TaxAreaCode" _
             & ",TaxLiable" _
             & ",CampaignNo" _
             & ",SellToContactNo" _
             & ",BillToContactNo" _
             & ",ResponsibilityCenter" _
             & ") VALUES (" _
             & m_DB.NullNumber(MemberId) _
             & "," & m_DB.Quote(SellToCustomerNo) _
             & "," & m_DB.Quote(No) _
             & "," & m_DB.Quote(BillToCustomerNo) _
             & "," & m_DB.Quote(BillToName) _
             & "," & m_DB.Quote(BillToName2) _
             & "," & m_DB.Quote(BillToAddress) _
             & "," & m_DB.Quote(BillToAddress2) _
             & "," & m_DB.Quote(BillToCity) _
             & "," & m_DB.Quote(BillToContact) _
             & "," & m_DB.Quote(YourReference) _
             & "," & m_DB.Quote(ShipToCode) _
             & "," & m_DB.Quote(ShipToName) _
             & "," & m_DB.Quote(ShipToName2) _
             & "," & m_DB.Quote(ShipToAddress) _
             & "," & m_DB.Quote(ShipToAddress2) _
             & "," & m_DB.Quote(ShipToCity) _
             & "," & m_DB.Quote(ShipToContact) _
             & "," & m_DB.NullQuote(Posting) _
             & "," & m_DB.NullQuote(Shipment) _
             & "," & m_DB.Quote(PostingDescription) _
             & "," & m_DB.Quote(PaymentTermsCode) _
             & "," & m_DB.NullQuote(Due) _
             & "," & m_DB.Number(PaymentDiscountPercent) _
             & "," & m_DB.NullQuote(PmtDiscount) _
             & "," & m_DB.Quote(ShipmentMethodCode) _
             & "," & m_DB.Quote(LocationCode) _
             & "," & m_DB.Quote(CurrencyCode) _
             & "," & m_DB.Quote(CustomerPriceGroup) _
             & "," & m_DB.Quote(PricesIncludingVAT) _
             & "," & m_DB.Quote(InvoiceDiscCode) _
             & "," & m_DB.Quote(CustomerDiscGroup) _
             & "," & m_DB.Quote(LanguageCode) _
             & "," & m_DB.Quote(SalespersonCode) _
             & "," & m_DB.Quote(AppliestoDocType) _
             & "," & m_DB.Quote(AppliestoDocNo) _
             & "," & m_DB.Quote(BalAccountNo) _
             & "," & m_DB.Quote(JobNo) _
             & "," & m_DB.Number(Amount) _
             & "," & m_DB.Number(AmountIncludingVAT) _
             & "," & m_DB.Quote(VATRegistrationNo) _
             & "," & m_DB.Quote(ReasonCode) _
             & "," & m_DB.Quote(TransactionType) _
             & "," & m_DB.Quote(TransportMethod) _
             & "," & m_DB.Quote(SellToCustomerName) _
             & "," & m_DB.Quote(SellToCustomerName2) _
             & "," & m_DB.Quote(SellToAddress) _
             & "," & m_DB.Quote(SellToAddress2) _
             & "," & m_DB.Quote(SellToCity) _
             & "," & m_DB.Quote(SellToContact) _
             & "," & m_DB.Quote(BillToPostCode) _
             & "," & m_DB.Quote(BillToCounty) _
             & "," & m_DB.Quote(BillToCountryCode) _
             & "," & m_DB.Quote(SellToPostCode) _
             & "," & m_DB.Quote(SellToCounty) _
             & "," & m_DB.Quote(SellToCountryCode) _
             & "," & m_DB.Quote(ShipToPostCode) _
             & "," & m_DB.Quote(ShipToCounty) _
             & "," & m_DB.Quote(ShipToCountryCode) _
             & "," & m_DB.Quote(BalAccountType) _
             & "," & m_DB.NullQuote(Documentdatetime) _
             & "," & m_DB.Quote(ExternalDocumentNo) _
             & "," & m_DB.Quote(PaymentMethodCode) _
             & "," & m_DB.Quote(UserID) _
             & "," & m_DB.Quote(SourceCode) _
             & "," & m_DB.Quote(TaxAreaCode) _
             & "," & m_DB.Quote(TaxLiable) _
             & "," & m_DB.Quote(CampaignNo) _
             & "," & m_DB.Quote(SellToContactNo) _
             & "," & m_DB.Quote(BillToContactNo) _
             & "," & m_DB.Quote(ResponsibilityCenter) _
             & ")"

            MemoId = m_DB.InsertSQL(SQL)

            Return MemoId
        End Function
        Public Sub UpdateSassi(ByVal _DB As Database)
            Dim SQL As String

            SQL = " UPDATE SalesCreditMemoHeader SET " _
             & " MemberId = " & _DB.NullNumber(MemberId) _
             & ",SellToCustomerNo = " & _DB.Quote(SellToCustomerNo) _
             & ",No = " & _DB.Quote(No) _
             & ",BillToCustomerNo = " & _DB.Quote(BillToCustomerNo) _
             & ",Amount = " & _DB.Number(Amount) _
             & " WHERE MemoId = " & _DB.Quote(MemoId)

            _DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE SalesCreditMemoHeader SET " _
             & " MemberId = " & m_DB.NullNumber(MemberId) _
             & ",SellToCustomerNo = " & m_DB.Quote(SellToCustomerNo) _
             & ",No = " & m_DB.Quote(No) _
             & ",BillToCustomerNo = " & m_DB.Quote(BillToCustomerNo) _
             & ",BillToName = " & m_DB.Quote(BillToName) _
             & ",BillToName2 = " & m_DB.Quote(BillToName2) _
             & ",BillToAddress = " & m_DB.Quote(BillToAddress) _
             & ",BillToAddress2 = " & m_DB.Quote(BillToAddress2) _
             & ",BillToCity = " & m_DB.Quote(BillToCity) _
             & ",BillToContact = " & m_DB.Quote(BillToContact) _
             & ",YourReference = " & m_DB.Quote(YourReference) _
             & ",ShipToCode = " & m_DB.Quote(ShipToCode) _
             & ",ShipToName = " & m_DB.Quote(ShipToName) _
             & ",ShipToName2 = " & m_DB.Quote(ShipToName2) _
             & ",ShipToAddress = " & m_DB.Quote(ShipToAddress) _
             & ",ShipToAddress2 = " & m_DB.Quote(ShipToAddress2) _
             & ",ShipToCity = " & m_DB.Quote(ShipToCity) _
             & ",ShipToContact = " & m_DB.Quote(ShipToContact) _
             & ",Posting = " & m_DB.NullQuote(Posting) _
             & ",Shipment = " & m_DB.NullQuote(Shipment) _
             & ",PostingDescription = " & m_DB.Quote(PostingDescription) _
             & ",PaymentTermsCode = " & m_DB.Quote(PaymentTermsCode) _
             & ",Due = " & m_DB.NullQuote(Due) _
             & ",PaymentDiscountPercent = " & m_DB.Number(PaymentDiscountPercent) _
             & ",PmtDiscount = " & m_DB.NullQuote(PmtDiscount) _
             & ",ShipmentMethodCode = " & m_DB.Quote(ShipmentMethodCode) _
             & ",LocationCode = " & m_DB.Quote(LocationCode) _
             & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
             & ",CustomerPriceGroup = " & m_DB.Quote(CustomerPriceGroup) _
             & ",PricesIncludingVAT = " & m_DB.Quote(PricesIncludingVAT) _
             & ",InvoiceDiscCode = " & m_DB.Quote(InvoiceDiscCode) _
             & ",CustomerDiscGroup = " & m_DB.Quote(CustomerDiscGroup) _
             & ",LanguageCode = " & m_DB.Quote(LanguageCode) _
             & ",SalespersonCode = " & m_DB.Quote(SalespersonCode) _
             & ",AppliestoDocType = " & m_DB.Quote(AppliestoDocType) _
             & ",AppliestoDocNo = " & m_DB.Quote(AppliestoDocNo) _
             & ",BalAccountNo = " & m_DB.Quote(BalAccountNo) _
             & ",JobNo = " & m_DB.Quote(JobNo) _
             & ",Amount = " & m_DB.Number(Amount) _
             & ",AmountIncludingVAT = " & m_DB.Number(AmountIncludingVAT) _
             & ",VATRegistrationNo = " & m_DB.Quote(VATRegistrationNo) _
             & ",ReasonCode = " & m_DB.Quote(ReasonCode) _
             & ",TransactionType = " & m_DB.Quote(TransactionType) _
             & ",TransportMethod = " & m_DB.Quote(TransportMethod) _
             & ",SellToCustomerName = " & m_DB.Quote(SellToCustomerName) _
             & ",SellToCustomerName2 = " & m_DB.Quote(SellToCustomerName2) _
             & ",SellToAddress = " & m_DB.Quote(SellToAddress) _
             & ",SellToAddress2 = " & m_DB.Quote(SellToAddress2) _
             & ",SellToCity = " & m_DB.Quote(SellToCity) _
             & ",SellToContact = " & m_DB.Quote(SellToContact) _
             & ",BillToPostCode = " & m_DB.Quote(BillToPostCode) _
             & ",BillToCounty = " & m_DB.Quote(BillToCounty) _
             & ",BillToCountryCode = " & m_DB.Quote(BillToCountryCode) _
             & ",SellToPostCode = " & m_DB.Quote(SellToPostCode) _
             & ",SellToCounty = " & m_DB.Quote(SellToCounty) _
             & ",SellToCountryCode = " & m_DB.Quote(SellToCountryCode) _
             & ",ShipToPostCode = " & m_DB.Quote(ShipToPostCode) _
             & ",ShipToCounty = " & m_DB.Quote(ShipToCounty) _
             & ",ShipToCountryCode = " & m_DB.Quote(ShipToCountryCode) _
             & ",BalAccountType = " & m_DB.Quote(BalAccountType) _
             & ",Documentdatetime = " & m_DB.NullQuote(Documentdatetime) _
             & ",ExternalDocumentNo = " & m_DB.Quote(ExternalDocumentNo) _
             & ",PaymentMethodCode = " & m_DB.Quote(PaymentMethodCode) _
             & ",UserID = " & m_DB.Quote(UserID) _
             & ",SourceCode = " & m_DB.Quote(SourceCode) _
             & ",TaxAreaCode = " & m_DB.Quote(TaxAreaCode) _
             & ",TaxLiable = " & m_DB.Quote(TaxLiable) _
             & ",CampaignNo = " & m_DB.Quote(CampaignNo) _
             & ",SellToContactNo = " & m_DB.Quote(SellToContactNo) _
             & ",BillToContactNo = " & m_DB.Quote(BillToContactNo) _
             & ",ResponsibilityCenter = " & m_DB.Quote(ResponsibilityCenter) _
             & " WHERE MemoId = " & m_DB.Quote(MemoId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM SalesCreditMemoHeader WHERE MemoId = " & m_DB.Number(MemoId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class SalesCreditMemoHeaderCollection
        Inherits GenericCollection(Of SalesCreditMemoHeaderRow)
    End Class

End Namespace


