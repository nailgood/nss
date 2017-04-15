Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Components.Core
Imports Utility

Namespace DataLayer

    Public Class CustomerRow
        Inherits CustomerRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CustomerId As Integer)
            MyBase.New(DB, CustomerId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CustomerNo As String)
            MyBase.New(DB, CustomerNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal CustomerId As Integer) As CustomerRow
            Dim row As CustomerRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_CustomerId_{0}", CustomerId)
            row = CloneObject.Clone(CType(CacheUtils.GetCache(key), CustomerRow))
            If Not row Is Nothing Then
                row.DB = _Database
                Return row
            End If

            row = New CustomerRow(_Database, CustomerId)
            row.Load()

            CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheDataItem)
            Return row
        End Function

        Public Shared Function GetRow(ByVal _Database As Database, ByVal CustomerNo As String) As CustomerRow
            Dim row As CustomerRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_CustomerNo_{0}", CustomerNo)
            row = CType(CacheUtils.GetCache(key), CustomerRow)
            If Not row Is Nothing Then
                row.DB = _Database
                Return row
            End If

            row = New CustomerRow(_Database, CustomerNo)
            row.Load()

            CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheDataItem)
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CustomerId As Integer)
            Dim row As CustomerRow

            row = New CustomerRow(DB, CustomerId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionCustomerRow, ByVal sLowerCase As String, ByVal sUpperCase As String)
            Address = ChangeCase(DB, Trim(r.Address), sLowerCase, sUpperCase)
            Address2 = ChangeCase(DB, Trim(r.Address_2), sLowerCase, sUpperCase)
            City = ChangeCase(DB, Trim(r.City), sLowerCase, sUpperCase)
            Contact = ChangeCase(DB, Trim(r.Contact), sLowerCase, sUpperCase)
            ContactNo = Trim(r.Primary_Contact_No)
            ContactId = DB.ExecuteScalar("select top 1 coalesce(ContactId, 0) from CustomerContact where ContactNo = " & DB.Quote(r.Primary_Contact_No))
            County = Trim(r.County_Text)
            SalesTaxExemptionNumber = Trim(r.VAT_Registration_No)
            CurrencyCode = Trim(r.Currency_Code)
            CustomerNo = Trim(r.Cust_No)
            CustomerDiscountGroup = Trim(r.Customer_Disc_Group)
            CustomerPriceGroupId = DB.ExecuteScalar("select top 1 customerpricegroupid from customerpricegroup where customerpricegroupcode = " & DB.Quote(Trim(r.Customer_Price_Group)))
            Email = Trim(r.Email_Text)
            LastDateModified = IIf(r.Last_Date_Modified = Nothing OrElse Not IsDate(r.Last_Date_Modified), Nothing, r.Last_Date_Modified)
            LastImport = Now()
            Name = ChangeCase(DB, Trim(r.Name), sLowerCase, sUpperCase)
            Name2 = ChangeCase(DB, Trim(r.Name_2), sLowerCase, sUpperCase)
            PaymentTermsCode = r.Payment_Terms_Code
            Phone = Trim(r.Phone_No)
            Website = Trim(r.Home_Page_Text)
            Zipcode = Trim(r.Post_Code)
            AutoDoExportOff = True
            CustomerPostingGroup = Trim(r.Customer_Posting_Group)
            If CustomerPostingGroup = Nothing Then CustomerPostingGroup = "VN"

            If CustomerId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub

        Public Shared Function GetCollectionForExport(ByVal DB As Database) As CustomerCollection
            Dim r As SqlDataReader = Nothing
            Dim row As CustomerRow
            Dim c As New CustomerCollection
            Try
                Dim SQL As String = "select * from Customer where DoExport = 1"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New CustomerRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return c
        End Function

    End Class

    Public MustInherit Class CustomerRowBase
        Private m_DB As Database
        Private m_CustomerId As Integer = Nothing
        Private m_CustomerNo As String = Nothing
        Private m_Name As String = Nothing
        Private m_Name2 As String = Nothing
        Private m_Address As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_City As String = Nothing
        Private m_Zipcode As String = Nothing
        Private m_County As String = Nothing
        Private m_Phone As String = Nothing
        Private m_PhoneExt As String = Nothing
        Private m_Contact As String = Nothing
        Private m_Email As String = Nothing
        Private m_Website As String = Nothing
        Private m_ContactNo As String = Nothing
        Private m_ContactId As Integer = Nothing
        Private m_CurrencyCode As String = Nothing
        Private m_CustomerPriceGroupId As Integer = Nothing
        Private m_CustomerDiscountGroup As String = Nothing
        Private m_LanguageCode As String = Nothing
        Private m_PaymentTermsCode As String = Nothing
        Private m_LastDateModified As DateTime = Nothing
        Private m_LastImport As DateTime = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_LastExport As DateTime = Nothing
        Private m_SalesTaxExemptionNumber As String = Nothing
        Private m_AutoDoExportOff As Boolean = False
        Private m_CustomerPostingGroup As String = Nothing
        Public Shared cachePrefixKey As String = "Customer_"

        Public Property CustomerPostingGroup() As String
            Get
                Return m_CustomerPostingGroup
            End Get
            Set(ByVal value As String)
                m_CustomerPostingGroup = value
            End Set
        End Property

        Public Property AutoDoExportOff() As Boolean
            Get
                Return m_AutoDoExportOff
            End Get
            Set(ByVal value As Boolean)
                m_AutoDoExportOff = value
            End Set
        End Property

        Public Property SalesTaxExemptionNumber() As String
            Get
                Return m_SalesTaxExemptionNumber
            End Get
            Set(ByVal value As String)
                m_SalesTaxExemptionNumber = value
            End Set
        End Property
        Public Property PhoneExt() As String
            Get
                Return m_PhoneExt
            End Get
            Set(ByVal value As String)
                m_PhoneExt = value
            End Set
        End Property
        Public Property CustomerId() As Integer
            Get
                Return m_CustomerId
            End Get
            Set(ByVal Value As Integer)
                m_CustomerId = Value
            End Set
        End Property

        Public Property ContactId() As Integer
            Get
                Return m_ContactId
            End Get
            Set(ByVal Value As Integer)
                m_ContactId = Value
            End Set
        End Property

        Public Property CustomerNo() As String
            Get
                Return m_CustomerNo
            End Get
            Set(ByVal Value As String)
                m_CustomerNo = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property Name2() As String
            Get
                Return m_Name2
            End Get
            Set(ByVal Value As String)
                m_Name2 = Value
            End Set
        End Property

        Public Property Address() As String
            Get
                Return m_Address
            End Get
            Set(ByVal Value As String)
                m_Address = Value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return m_Address2
            End Get
            Set(ByVal Value As String)
                m_Address2 = Value
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

        Public Property Zipcode() As String
            Get
                Return m_Zipcode
            End Get
            Set(ByVal Value As String)
                m_Zipcode = Value
            End Set
        End Property

        Public Property County() As String
            Get
                Return m_County
            End Get
            Set(ByVal Value As String)
                m_County = Value
            End Set
        End Property

        Public Property Phone() As String
            Get
                Return m_Phone
            End Get
            Set(ByVal Value As String)
                m_Phone = Value
            End Set
        End Property

        Public Property Contact() As String
            Get
                Return m_Contact
            End Get
            Set(ByVal Value As String)
                m_Contact = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property Website() As String
            Get
                Return m_Website
            End Get
            Set(ByVal Value As String)
                m_Website = Value
            End Set
        End Property

        Public Property ContactNo() As String
            Get
                Return m_ContactNo
            End Get
            Set(ByVal Value As String)
                m_ContactNo = Value
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

        Public Property CustomerPriceGroupId() As Integer
            Get
                Return m_CustomerPriceGroupId
            End Get
            Set(ByVal Value As Integer)
                m_CustomerPriceGroupId = Value
            End Set
        End Property

        Public Property CustomerDiscountGroup() As String
            Get
                Return m_CustomerDiscountGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerDiscountGroup = Value
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

        Public Property PaymentTermsCode() As String
            Get
                Return m_PaymentTermsCode
            End Get
            Set(ByVal Value As String)
                m_PaymentTermsCode = Value
            End Set
        End Property

        Public Property LastDateModified() As DateTime
            Get
                Return m_LastDateModified
            End Get
            Set(ByVal Value As DateTime)
                m_LastDateModified = Value
            End Set
        End Property

        Public Property LastImport() As DateTime
            Get
                Return m_LastImport
            End Get
            Set(ByVal Value As DateTime)
                m_LastImport = Value
            End Set
        End Property

        Public Property DoExport() As Boolean
            Get
                Return m_DoExport
            End Get
            Set(ByVal Value As Boolean)
                m_DoExport = Value
            End Set
        End Property

        Public Property LastExport() As DateTime
            Get
                Return m_LastExport
            End Get
            Set(ByVal Value As DateTime)
                m_LastExport = Value
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

        Public Sub New(ByVal DB As Database, ByVal CustomerId As Integer)
            m_DB = DB
            m_CustomerId = CustomerId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CustomerNo As String)
            m_DB = DB
            m_CustomerNo = CustomerNo
        End Sub 'New

        Protected Overridable Sub Load()
            If (CustomerId = Nothing Or CustomerId < 1) And (CustomerNo Is Nothing Or CustomerNo = String.Empty) Then
                Exit Sub
            End If
            Dim reader As SqlDataReader = Nothing
            Try
                Dim SP_GETOBJECT As String = "sp_Customer_GetObject"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId)
                db.AddInParameter(cmd, "CustomerNo", DbType.String, CustomerNo)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_CustomerId = Convert.ToInt32(r.Item("CustomerId"))
            m_CustomerNo = Convert.ToString(r.Item("CustomerNo"))
            If IsDBNull(r.Item("SalesTaxExemptionNumber")) Then
                m_SalesTaxExemptionNumber = Nothing
            Else
                m_SalesTaxExemptionNumber = Convert.ToString(r.Item("SalesTaxExemptionNumber"))
            End If
            If IsDBNull(r.Item("Name")) Then
                m_Name = Nothing
            Else
                m_Name = Convert.ToString(r.Item("Name"))
            End If
            If IsDBNull(r.Item("Name2")) Then
                m_Name2 = Nothing
            Else
                m_Name2 = Convert.ToString(r.Item("Name2"))
            End If
            If IsDBNull(r.Item("Address")) Then
                m_Address = Nothing
            Else
                m_Address = Convert.ToString(r.Item("Address"))
            End If
            If IsDBNull(r.Item("Address2")) Then
                m_Address2 = Nothing
            Else
                m_Address2 = Convert.ToString(r.Item("Address2"))
            End If
            If IsDBNull(r.Item("City")) Then
                m_City = Nothing
            Else
                m_City = Convert.ToString(r.Item("City"))
            End If
            If IsDBNull(r.Item("Zipcode")) Then
                m_Zipcode = Nothing
            Else
                m_Zipcode = Convert.ToString(r.Item("Zipcode"))
            End If
            If IsDBNull(r.Item("County")) Then
                m_County = Nothing
            Else
                m_County = Convert.ToString(r.Item("County"))
            End If
            If IsDBNull(r.Item("Phone")) Then
                m_Phone = Nothing
            Else
                m_Phone = Convert.ToString(r.Item("Phone"))
            End If
            If IsDBNull(r.Item("PhoneExt")) Then
                m_PhoneExt = Nothing
            Else
                m_PhoneExt = Convert.ToString(r.Item("PhoneExt"))
            End If
            If IsDBNull(r.Item("Contact")) Then
                m_Contact = Nothing
            Else
                m_Contact = Convert.ToString(r.Item("Contact"))
            End If
            If IsDBNull(r.Item("Email")) Then
                m_Email = Nothing
            Else
                m_Email = Convert.ToString(r.Item("Email"))
            End If
            If IsDBNull(r.Item("Website")) Then
                m_Website = Nothing
            Else
                m_Website = Convert.ToString(r.Item("Website"))
            End If
            If IsDBNull(r.Item("ContactNo")) Then
                m_ContactNo = Nothing
            Else
                m_ContactNo = Convert.ToString(r.Item("ContactNo"))
            End If
            If IsDBNull(r.Item("CurrencyCode")) Then
                m_CurrencyCode = Nothing
            Else
                m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
            End If
            If IsDBNull(r.Item("CustomerPriceGroupId")) Then
                m_CustomerPriceGroupId = Nothing
            Else
                m_CustomerPriceGroupId = Convert.ToInt32(r.Item("CustomerPriceGroupId"))
            End If
            If IsDBNull(r.Item("CustomerDiscountGroup")) Then
                m_CustomerDiscountGroup = Nothing
            Else
                m_CustomerDiscountGroup = Convert.ToString(r.Item("CustomerDiscountGroup"))
            End If
            If IsDBNull(r.Item("LanguageCode")) Then
                m_LanguageCode = Nothing
            Else
                m_LanguageCode = Convert.ToString(r.Item("LanguageCode"))
            End If
            If IsDBNull(r.Item("ContactId")) Then
                m_ContactId = Nothing
            Else
                m_ContactId = Convert.ToInt32(r.Item("ContactId"))
            End If
            If IsDBNull(r.Item("PaymentTermsCode")) Then
                m_PaymentTermsCode = Nothing
            Else
                m_PaymentTermsCode = Convert.ToString(r.Item("PaymentTermsCode"))
            End If
            If IsDBNull(r.Item("LastDateModified")) Then
                m_LastDateModified = Nothing
            Else
                m_LastDateModified = Convert.ToDateTime(r.Item("LastDateModified"))
            End If
            If IsDBNull(r.Item("LastImport")) Then
                m_LastImport = Nothing
            Else
                m_LastImport = Convert.ToDateTime(r.Item("LastImport"))
            End If
            m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
            If IsDBNull(r.Item("LastExport")) Then
                m_LastExport = Nothing
            Else
                m_LastExport = Convert.ToDateTime(r.Item("LastExport"))
            End If
            m_CustomerPostingGroup = Convert.ToString(r.Item("CustomerPostingGroup"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            ''If Not AutoDoExportOff Then DoExport = True

            If m_CustomerNo = Nothing Then
                Dim Gen As New CustomerNoGenRow(DB)
                Gen.Insert()
                m_CustomerNo = "W" & Gen.Id
                DB.ExecuteSQL("Delete from CustomerNoGen where Id<>" & Gen.Id)
                ''Gen.RemoveAll()
            End If

            SQL = " INSERT INTO Customer (" _
             & " CustomerNo" _
             & ",Name" _
             & ",Name2" _
             & ",Address" _
             & ",Address2" _
             & ",City" _
             & ",Zipcode" _
             & ",County" _
             & ",Phone" _
             & ",PhoneExt" _
             & ",Contact" _
             & ",Email" _
             & ",Website" _
             & ",ContactNo" _
             & ",ContactId" _
             & ",SalesTaxExemptionNumber" _
             & ",CurrencyCode" _
             & ",CustomerPriceGroupId" _
             & ",CustomerDiscountGroup" _
             & ",LanguageCode" _
             & ",PaymentTermsCode" _
             & ",LastDateModified" _
             & ",LastImport" _
             & ",DoExport" _
             & ",LastExport" _
             & ",CustomerPostingGroup" _
             & ") VALUES (" _
             & m_DB.Quote(m_CustomerNo) _
             & "," & m_DB.NQuote(Name) _
             & "," & m_DB.NQuote(Name2) _
             & "," & m_DB.NQuote(Address) _
             & "," & m_DB.NQuote(Address2) _
             & "," & m_DB.NQuote(City) _
             & "," & m_DB.Quote(Zipcode) _
             & "," & m_DB.NQuote(County) _
             & "," & m_DB.Quote(Phone) _
             & "," & m_DB.Quote(PhoneExt) _
             & "," & m_DB.Quote(Contact) _
             & "," & m_DB.Quote(Email) _
             & "," & m_DB.Quote(Website) _
             & "," & m_DB.Quote(ContactNo) _
             & "," & m_DB.Quote(ContactId) _
             & "," & m_DB.Quote(SalesTaxExemptionNumber) _
             & "," & m_DB.Quote(CurrencyCode) _
             & "," & m_DB.NullNumber(CustomerPriceGroupId) _
             & "," & m_DB.Quote(CustomerDiscountGroup) _
             & "," & m_DB.Quote(LanguageCode) _
             & "," & m_DB.Quote(PaymentTermsCode) _
             & "," & m_DB.NullQuote(LastDateModified) _
             & "," & m_DB.NullQuote(LastImport) _
             & "," & CInt(DoExport) _
             & "," & m_DB.NullQuote(LastExport) _
             & "," & m_DB.Quote(CustomerPostingGroup) _
             & ")"

            CustomerId = m_DB.InsertSQL(SQL)

            Return CustomerId
        End Function

        Public Overridable Sub Update()
            CacheUtils.ClearCacheWithPrefix("Customer_GetRow_" & IIf(CustomerId <> Nothing, "CustomerId_" & CustomerId, "CustomerNo_" & CustomerNo))
            Dim SQL As String
            ''If Not AutoDoExportOff Then DoExport = True

            SQL = " UPDATE Customer SET " _
        & " CustomerNo = " & m_DB.Quote(CustomerNo) _
        & ",Name = " & m_DB.NQuote(Name) _
        & ",Name2 = " & m_DB.NQuote(Name2) _
        & ",Address = " & m_DB.NQuote(Address) _
        & ",Address2 = " & m_DB.NQuote(Address2) _
        & ",City = " & m_DB.NQuote(City) _
        & ",Zipcode = " & m_DB.Quote(Zipcode) _
        & ",County = " & m_DB.NQuote(County) _
        & ",Phone = " & m_DB.Quote(Phone) _
        & ",PhoneExt = " & m_DB.Quote(PhoneExt) _
        & ",Contact = " & m_DB.NQuote(Contact) _
        & ",Email = " & m_DB.Quote(Email) _
        & ",Website = " & m_DB.Quote(Website) _
        & ",ContactNo = " & m_DB.Quote(ContactNo) _
        & ",ContactId = " & m_DB.Quote(ContactId) _
        & ",SalesTaxExemptionNumber = " & m_DB.Quote(SalesTaxExemptionNumber) _
        & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
        & ",CustomerPriceGroupId = " & m_DB.NullNumber(CustomerPriceGroupId) _
        & ",CustomerDiscountGroup = " & m_DB.Quote(CustomerDiscountGroup) _
        & ",LanguageCode = " & m_DB.Quote(LanguageCode) _
        & ",PaymentTermsCode = " & m_DB.Quote(PaymentTermsCode) _
        & ",LastDateModified = " & m_DB.NullQuote(LastDateModified) _
        & ",LastImport = " & m_DB.NullQuote(LastImport) _
        & ",DoExport = " & CInt(DoExport) _
        & ",LastExport = " & m_DB.NullQuote(LastExport) _
        & ",CustomerPostingGroup = " & m_DB.Quote(CustomerPostingGroup) _
        & " WHERE " & IIf(CustomerId <> Nothing, "CustomerId = " & m_DB.Quote(CustomerId), "CustomerNo = " & DB.Quote(CustomerNo))

            m_DB.ExecuteSQL(SQL)



        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: August 29, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_Customer_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class CustomerCollection
        Inherits GenericCollection(Of CustomerRow)
    End Class

End Namespace


