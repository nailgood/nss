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

Namespace DataLayer

    Public Class CustomerContactRow
        Inherits CustomerContactRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ContactId As Integer)
            MyBase.New(DB, ContactId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ContactNo As String)
            MyBase.New(DB, ContactNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ContactId As Integer) As CustomerContactRow
            Dim row As CustomerContactRow

            row = New CustomerContactRow(DB, ContactId)
            row.Load()

            Return row
        End Function

        'end 22/10/2009
        Public Shared Function GetRow(ByVal DB As Database, ByVal ContactNo As String) As CustomerContactRow
            Dim row As CustomerContactRow

            row = New CustomerContactRow(DB, ContactNo)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ContactId As Integer)
            Dim row As CustomerContactRow

            row = New CustomerContactRow(DB, ContactId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionCustomerContactsRow, ByVal sLowerCase As String, ByVal sUpperCase As String)
            ContactNo = r.Contact_No
            CustName = r.Cust_Name
            CustName2 = r.Cust_Name_2
            Address = r.Address
            Address2 = r.Address_2
            City = r.City
            Phone = r.Phone
            TerritoryCode = r.Territory_Code
            CurrencyCode = r.Currency_Code
            LanguageCode = r.Language_Code
            SalespersonCode = r.Salesperson_Code
            CountryCode = r.Country_Code
            Comment = r.Comment
            LastDateModified = r.Last_Date_Modified
            FaxNo = r.Fax_No
            VATRegistrationNo = r.VAT_Registration_No
            PostCode = r.Post_Code
            County = r.County
            Email = r.Email
            HomePage = r.Home_Page
            Type = r.Type
            CompanyNo = r.Company_No
            CompanyName = r.Company_Name
            FirstName = r.First_Name
            MiddleName = r.Middle_Name
            Surname = r.Surname
            JobTitle = r.Job_Title
            Initials = r.Initials
            ExtensionNo = r.Extension_No
            MobilePhoneNo = r.Mobile_Phone_No
            Pager = r.Pager
            SalutationCode = r.Salutation_Code
            LastTimeModified = r.Last_Date_Modified

            If ContactId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub

        Public Shared Function GetCollectionForExport(ByVal DB As Database) As CustomerContactCollection
            Dim c As New CustomerContactCollection
            Dim r As SqlDataReader = Nothing
            Try
                Dim row As CustomerContactRow
                Dim SQL As String = "select * from CustomerContact where DoExport = 1"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New CustomerContactRow(DB)
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

    Public MustInherit Class CustomerContactRowBase
        Private m_DB As Database
        Private m_ContactId As Integer = Nothing
        Private m_ContactNo As String = Nothing
        Private m_CustName As String = Nothing
        Private m_CustName2 As String = Nothing
        Private m_Address As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_City As String = Nothing
        Private m_Phone As String = Nothing
        Private m_TerritoryCode As String = Nothing
        Private m_CurrencyCode As String = Nothing
        Private m_LanguageCode As String = Nothing
        Private m_SalespersonCode As String = Nothing
        Private m_CountryCode As String = Nothing
        Private m_Comment As String = Nothing
        Private m_LastDateModified As String = Nothing
        Private m_FaxNo As String = Nothing
        Private m_VATRegistrationNo As String = Nothing
        Private m_PostCode As String = Nothing
        Private m_County As String = Nothing
        Private m_Email As String = Nothing
        Private m_HomePage As String = Nothing
        Private m_Type As String = Nothing
        Private m_CompanyNo As String = Nothing
        Private m_CompanyName As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_MiddleName As String = Nothing
        Private m_Surname As String = Nothing
        Private m_JobTitle As String = Nothing
        Private m_Initials As String = Nothing
        Private m_ExtensionNo As String = Nothing
        Private m_MobilePhoneNo As String = Nothing
        Private m_Pager As String = Nothing
        Private m_SalutationCode As String = Nothing
        Private m_LastTimeModified As String = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_LastExport As DateTime = Nothing


        Public Property ContactId() As Integer
            Get
                Return m_ContactId
            End Get
            Set(ByVal Value As Integer)
                m_ContactId = Value
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

        Public Property CustName() As String
            Get
                Return m_CustName
            End Get
            Set(ByVal Value As String)
                m_CustName = Value
            End Set
        End Property

        Public Property CustName2() As String
            Get
                Return m_CustName2
            End Get
            Set(ByVal Value As String)
                m_CustName2 = Value
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

        Public Property Phone() As String
            Get
                Return m_Phone
            End Get
            Set(ByVal Value As String)
                m_Phone = Value
            End Set
        End Property

        Public Property TerritoryCode() As String
            Get
                Return m_TerritoryCode
            End Get
            Set(ByVal Value As String)
                m_TerritoryCode = Value
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

        Public Property CountryCode() As String
            Get
                Return m_CountryCode
            End Get
            Set(ByVal Value As String)
                m_CountryCode = Value
            End Set
        End Property

        Public Property Comment() As String
            Get
                Return m_Comment
            End Get
            Set(ByVal Value As String)
                m_Comment = Value
            End Set
        End Property

        Public Property LastDateModified() As String
            Get
                Return m_LastDateModified
            End Get
            Set(ByVal Value As String)
                m_LastDateModified = Value
            End Set
        End Property

        Public Property FaxNo() As String
            Get
                Return m_FaxNo
            End Get
            Set(ByVal Value As String)
                m_FaxNo = Value
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

        Public Property PostCode() As String
            Get
                Return m_PostCode
            End Get
            Set(ByVal Value As String)
                m_PostCode = Value
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

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property HomePage() As String
            Get
                Return m_HomePage
            End Get
            Set(ByVal Value As String)
                m_HomePage = Value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal Value As String)
                m_Type = Value
            End Set
        End Property

        Public Property CompanyNo() As String
            Get
                Return m_CompanyNo
            End Get
            Set(ByVal Value As String)
                m_CompanyNo = Value
            End Set
        End Property

        Public Property CompanyName() As String
            Get
                Return m_CompanyName
            End Get
            Set(ByVal Value As String)
                m_CompanyName = Value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal Value As String)
                m_FirstName = Value
            End Set
        End Property

        Public Property MiddleName() As String
            Get
                Return m_MiddleName
            End Get
            Set(ByVal Value As String)
                m_MiddleName = Value
            End Set
        End Property

        Public Property Surname() As String
            Get
                Return m_Surname
            End Get
            Set(ByVal Value As String)
                m_Surname = Value
            End Set
        End Property

        Public Property JobTitle() As String
            Get
                Return m_JobTitle
            End Get
            Set(ByVal Value As String)
                m_JobTitle = Value
            End Set
        End Property

        Public Property Initials() As String
            Get
                Return m_Initials
            End Get
            Set(ByVal Value As String)
                m_Initials = Value
            End Set
        End Property

        Public Property ExtensionNo() As String
            Get
                Return m_ExtensionNo
            End Get
            Set(ByVal Value As String)
                m_ExtensionNo = Value
            End Set
        End Property

        Public Property MobilePhoneNo() As String
            Get
                Return m_MobilePhoneNo
            End Get
            Set(ByVal Value As String)
                m_MobilePhoneNo = Value
            End Set
        End Property

        Public Property Pager() As String
            Get
                Return m_Pager
            End Get
            Set(ByVal Value As String)
                m_Pager = Value
            End Set
        End Property

        Public Property SalutationCode() As String
            Get
                Return m_SalutationCode
            End Get
            Set(ByVal Value As String)
                m_SalutationCode = Value
            End Set
        End Property

        Public Property LastTimeModified() As String
            Get
                Return m_LastTimeModified
            End Get
            Set(ByVal Value As String)
                m_LastTimeModified = Value
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

        Public Sub New(ByVal DB As Database, ByVal ContactId As Integer)
            m_DB = DB
            m_ContactId = ContactId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ContactNo As String)
            m_DB = DB
            m_ContactNo = ContactNo
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM CustomerContact WHERE " & IIf(ContactId <> Nothing, "ContactId = " & DB.Number(ContactId), "ContactNo = " & DB.Quote(ContactNo))
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
            m_ContactId = Convert.ToInt32(r.Item("ContactId"))
            If IsDBNull(r.Item("ContactNo")) Then
                m_ContactNo = Nothing
            Else
                m_ContactNo = Convert.ToString(r.Item("ContactNo"))
            End If
            If IsDBNull(r.Item("CustName")) Then
                m_CustName = Nothing
            Else
                m_CustName = Convert.ToString(r.Item("CustName"))
            End If
            If IsDBNull(r.Item("CustName2")) Then
                m_CustName2 = Nothing
            Else
                m_CustName2 = Convert.ToString(r.Item("CustName2"))
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
            If IsDBNull(r.Item("Phone")) Then
                m_Phone = Nothing
            Else
                m_Phone = Convert.ToString(r.Item("Phone"))
            End If
            If IsDBNull(r.Item("TerritoryCode")) Then
                m_TerritoryCode = Nothing
            Else
                m_TerritoryCode = Convert.ToString(r.Item("TerritoryCode"))
            End If
            If IsDBNull(r.Item("CurrencyCode")) Then
                m_CurrencyCode = Nothing
            Else
                m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
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
            If IsDBNull(r.Item("CountryCode")) Then
                m_CountryCode = Nothing
            Else
                m_CountryCode = Convert.ToString(r.Item("CountryCode"))
            End If
            If IsDBNull(r.Item("Comment")) Then
                m_Comment = Nothing
            Else
                m_Comment = Convert.ToString(r.Item("Comment"))
            End If
            If IsDBNull(r.Item("LastDateModified")) Then
                m_LastDateModified = Nothing
            Else
                m_LastDateModified = Convert.ToString(r.Item("LastDateModified"))
            End If
            If IsDBNull(r.Item("FaxNo")) Then
                m_FaxNo = Nothing
            Else
                m_FaxNo = Convert.ToString(r.Item("FaxNo"))
            End If
            If IsDBNull(r.Item("VATRegistrationNo")) Then
                m_VATRegistrationNo = Nothing
            Else
                m_VATRegistrationNo = Convert.ToString(r.Item("VATRegistrationNo"))
            End If
            If IsDBNull(r.Item("PostCode")) Then
                m_PostCode = Nothing
            Else
                m_PostCode = Convert.ToString(r.Item("PostCode"))
            End If
            If IsDBNull(r.Item("County")) Then
                m_County = Nothing
            Else
                m_County = Convert.ToString(r.Item("County"))
            End If
            If IsDBNull(r.Item("Email")) Then
                m_Email = Nothing
            Else
                m_Email = Convert.ToString(r.Item("Email"))
            End If
            If IsDBNull(r.Item("HomePage")) Then
                m_HomePage = Nothing
            Else
                m_HomePage = Convert.ToString(r.Item("HomePage"))
            End If
            If IsDBNull(r.Item("Type")) Then
                m_Type = Nothing
            Else
                m_Type = Convert.ToString(r.Item("Type"))
            End If
            If IsDBNull(r.Item("CompanyNo")) Then
                m_CompanyNo = Nothing
            Else
                m_CompanyNo = Convert.ToString(r.Item("CompanyNo"))
            End If
            If IsDBNull(r.Item("CompanyName")) Then
                m_CompanyName = Nothing
            Else
                m_CompanyName = Convert.ToString(r.Item("CompanyName"))
            End If
            If IsDBNull(r.Item("FirstName")) Then
                m_FirstName = Nothing
            Else
                m_FirstName = Convert.ToString(r.Item("FirstName"))
            End If
            If IsDBNull(r.Item("MiddleName")) Then
                m_MiddleName = Nothing
            Else
                m_MiddleName = Convert.ToString(r.Item("MiddleName"))
            End If
            If IsDBNull(r.Item("Surname")) Then
                m_Surname = Nothing
            Else
                m_Surname = Convert.ToString(r.Item("Surname"))
            End If
            If IsDBNull(r.Item("JobTitle")) Then
                m_JobTitle = Nothing
            Else
                m_JobTitle = Convert.ToString(r.Item("JobTitle"))
            End If
            If IsDBNull(r.Item("Initials")) Then
                m_Initials = Nothing
            Else
                m_Initials = Convert.ToString(r.Item("Initials"))
            End If
            If IsDBNull(r.Item("ExtensionNo")) Then
                m_ExtensionNo = Nothing
            Else
                m_ExtensionNo = Convert.ToString(r.Item("ExtensionNo"))
            End If
            If IsDBNull(r.Item("MobilePhoneNo")) Then
                m_MobilePhoneNo = Nothing
            Else
                m_MobilePhoneNo = Convert.ToString(r.Item("MobilePhoneNo"))
            End If
            If IsDBNull(r.Item("Pager")) Then
                m_Pager = Nothing
            Else
                m_Pager = Convert.ToString(r.Item("Pager"))
            End If
            If IsDBNull(r.Item("SalutationCode")) Then
                m_SalutationCode = Nothing
            Else
                m_SalutationCode = Convert.ToString(r.Item("SalutationCode"))
            End If
            If IsDBNull(r.Item("LastTimeModified")) Then
                m_LastTimeModified = Nothing
            Else
                m_LastTimeModified = Convert.ToString(r.Item("LastTimeModified"))
            End If
            m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
            If IsDBNull(r.Item("LastExport")) Then
                m_LastExport = Nothing
            Else
                m_LastExport = Convert.ToDateTime(r.Item("LastExport"))
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO CustomerContact (" _
             & " ContactNo" _
             & ",CustName" _
             & ",CustName2" _
             & ",Address" _
             & ",Address2" _
             & ",City" _
             & ",Phone" _
             & ",TerritoryCode" _
             & ",CurrencyCode" _
             & ",LanguageCode" _
             & ",SalespersonCode" _
             & ",CountryCode" _
             & ",Comment" _
             & ",LastDateModified" _
             & ",FaxNo" _
             & ",VATRegistrationNo" _
             & ",PostCode" _
             & ",County" _
             & ",Email" _
             & ",HomePage" _
             & ",Type" _
             & ",CompanyNo" _
             & ",CompanyName" _
             & ",FirstName" _
             & ",MiddleName" _
             & ",Surname" _
             & ",JobTitle" _
             & ",Initials" _
             & ",ExtensionNo" _
             & ",MobilePhoneNo" _
             & ",Pager" _
             & ",SalutationCode" _
             & ",LastTimeModified" _
             & ",DoExport" _
             & ",LastExport" _
             & ") VALUES (" _
             & m_DB.Quote(ContactNo) _
             & "," & m_DB.NQuote(CustName) _
             & "," & m_DB.NQuote(CustName2) _
             & "," & m_DB.NQuote(Address) _
             & "," & m_DB.NQuote(Address2) _
             & "," & m_DB.NQuote(City) _
             & "," & m_DB.Quote(Phone) _
             & "," & m_DB.Quote(TerritoryCode) _
             & "," & m_DB.Quote(CurrencyCode) _
             & "," & m_DB.Quote(LanguageCode) _
             & "," & m_DB.Quote(SalespersonCode) _
             & "," & m_DB.Quote(CountryCode) _
             & "," & m_DB.Quote(Comment) _
             & "," & m_DB.Quote(LastDateModified) _
             & "," & m_DB.Quote(FaxNo) _
             & "," & m_DB.Quote(VATRegistrationNo) _
             & "," & m_DB.Quote(PostCode) _
             & "," & m_DB.Quote(County) _
             & "," & m_DB.Quote(Email) _
             & "," & m_DB.Quote(HomePage) _
             & "," & m_DB.Quote(Type) _
             & "," & m_DB.Quote(CompanyNo) _
             & "," & m_DB.NQuote(CompanyName) _
             & "," & m_DB.NQuote(FirstName) _
             & "," & m_DB.NQuote(MiddleName) _
             & "," & m_DB.NQuote(Surname) _
             & "," & m_DB.NQuote(JobTitle) _
             & "," & m_DB.Quote(Initials) _
             & "," & m_DB.Quote(ExtensionNo) _
             & "," & m_DB.Quote(MobilePhoneNo) _
             & "," & m_DB.Quote(Pager) _
             & "," & m_DB.Quote(SalutationCode) _
             & "," & m_DB.Quote(LastTimeModified) _
             & "," & CInt(DoExport) _
             & "," & m_DB.NullQuote(LastExport) _
             & ")"

            ContactId = m_DB.InsertSQL(SQL)

            Return ContactId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE CustomerContact SET " _
             & " ContactNo = " & m_DB.Quote(ContactNo) _
             & ",CustName = " & m_DB.NQuote(CustName) _
             & ",CustName2 = " & m_DB.NQuote(CustName2) _
             & ",Address = " & m_DB.NQuote(Address) _
             & ",Address2 = " & m_DB.NQuote(Address2) _
             & ",City = " & m_DB.NQuote(City) _
             & ",Phone = " & m_DB.Quote(Phone) _
             & ",TerritoryCode = " & m_DB.Quote(TerritoryCode) _
             & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
             & ",LanguageCode = " & m_DB.Quote(LanguageCode) _
             & ",SalespersonCode = " & m_DB.Quote(SalespersonCode) _
             & ",CountryCode = " & m_DB.Quote(CountryCode) _
             & ",Comment = " & m_DB.NQuote(Comment) _
             & ",LastDateModified = " & m_DB.Quote(LastDateModified) _
             & ",FaxNo = " & m_DB.Quote(FaxNo) _
             & ",VATRegistrationNo = " & m_DB.Quote(VATRegistrationNo) _
             & ",PostCode = " & m_DB.Quote(PostCode) _
             & ",County = " & m_DB.Quote(County) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",HomePage = " & m_DB.Quote(HomePage) _
             & ",Type = " & m_DB.Quote(Type) _
             & ",CompanyNo = " & m_DB.Quote(CompanyNo) _
             & ",CompanyName = " & m_DB.NQuote(CompanyName) _
             & ",FirstName = " & m_DB.NQuote(FirstName) _
             & ",MiddleName = " & m_DB.NQuote(MiddleName) _
             & ",Surname = " & m_DB.NQuote(Surname) _
             & ",JobTitle = " & m_DB.NQuote(JobTitle) _
             & ",Initials = " & m_DB.Quote(Initials) _
             & ",ExtensionNo = " & m_DB.Quote(ExtensionNo) _
             & ",MobilePhoneNo = " & m_DB.Quote(MobilePhoneNo) _
             & ",Pager = " & m_DB.Quote(Pager) _
             & ",SalutationCode = " & m_DB.Quote(SalutationCode) _
             & ",LastTimeModified = " & m_DB.Quote(LastTimeModified) _
             & ",DoExport = " & CInt(DoExport) _
             & ",LastExport = " & m_DB.NullQuote(LastExport) _
             & " WHERE ContactId = " & m_DB.Quote(ContactId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM CustomerContact WHERE ContactId = " & m_DB.Number(ContactId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class CustomerContactCollection
        Inherits GenericCollection(Of CustomerContactRow)
    End Class

End Namespace


