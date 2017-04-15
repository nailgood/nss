Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionSalesCreditMemoHeaderRow
        Inherits NavisionSalesCreditMemoHeaderRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Shared Function GetCollection(ByVal DB As Database) As NavisionSalesCreditMemoHeaderCollection
            Dim r As SqlDataReader = Nothing
            Dim row As NavisionSalesCreditMemoHeaderRow
            Dim c As New NavisionSalesCreditMemoHeaderCollection
            Dim SQL As String = "select * from _NAVISION_SALES_CREDIT_MEMO_HEADER"
            Try
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionSalesCreditMemoHeaderRow(DB)
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

    Public MustInherit Class NavisionSalesCreditMemoHeaderRowBase
        Private m_DB As Database
        Private m_Sell_To_Customer_No As String = Nothing
        Private m_No As String = Nothing
        Private m_Bill_To_Customer_No As String = Nothing
        Private m_Bill_To_Name As String = Nothing
        Private m_Bill_To_Name_2 As String = Nothing
        Private m_Bill_To_Address As String = Nothing
        Private m_Bill_To_Address_2 As String = Nothing
        Private m_Bill_To_City As String = Nothing
        Private m_Bill_To_Contact As String = Nothing
        Private m_Your_Reference As String = Nothing
        Private m_Ship_To_Code As String = Nothing
        Private m_Ship_To_Name As String = Nothing
        Private m_Ship_To_Name_2 As String = Nothing
        Private m_Ship_To_Address As String = Nothing
        Private m_Ship_To_Address_2 As String = Nothing
        Private m_Ship_To_City As String = Nothing
        Private m_Ship_To_Contact As String = Nothing
        Private m_Posting As DateTime = Nothing
        Private m_Shipment As DateTime = Nothing
        Private m_Posting_Description As String = Nothing
        Private m_Payment_Terms_Code As String = Nothing
        Private m_Due As DateTime = Nothing
        Private m_Payment_Discount_Percent As Double = Nothing
        Private m_Pmt_Discount As DateTime = Nothing
        Private m_Shipment_Method_Code As String = Nothing
        Private m_Location_Code As String = Nothing
        Private m_Currency_Code As String = Nothing
        Private m_Customer_Price_Group As String = Nothing
        Private m_Prices_Including_VAT As String = Nothing
        Private m_Invoice_Disc_Code As String = Nothing
        Private m_Customer_Disc_Group As String = Nothing
        Private m_Language_Code As String = Nothing
        Private m_Salesperson_Code As String = Nothing
        Private m_Applies_to_Doc_Type As String = Nothing
        Private m_Applies_to_Doc_No As String = Nothing
        Private m_Bal_Account_No As String = Nothing
        Private m_Job_No As String = Nothing
        Private m_Amount As Double = Nothing
        Private m_Amount_Including_VAT As Double = Nothing
        Private m_VAT_Registration_No As String = Nothing
        Private m_Reason_Code As String = Nothing
        Private m_Transaction_Type As String = Nothing
        Private m_Transport_Method As String = Nothing
        Private m_Sell_To_Customer_Name As String = Nothing
        Private m_Sell_To_Customer_Name_2 As String = Nothing
        Private m_Sell_To_Address As String = Nothing
        Private m_Sell_To_Address_2 As String = Nothing
        Private m_Sell_To_City As String = Nothing
        Private m_Sell_To_Contact As String = Nothing
        Private m_Bill_To_Post_Code As String = Nothing
        Private m_Bill_To_County As String = Nothing
        Private m_Bill_To_Country_Code As String = Nothing
        Private m_Sell_To_Post_Code As String = Nothing
        Private m_Sell_To_County As String = Nothing
        Private m_Sell_To_Country_Code As String = Nothing
        Private m_Ship_To_Post_Code As String = Nothing
        Private m_Ship_To_County As String = Nothing
        Private m_Ship_To_Country_Code As String = Nothing
        Private m_Bal_Account_Type As String = Nothing
        Private m_Document_datetime As DateTime = Nothing
        Private m_External_Document_No As String = Nothing
        Private m_Payment_Method_Code As String = Nothing
        Private m_User_ID As String = Nothing
        Private m_Source_Code As String = Nothing
        Private m_Tax_Area_Code As String = Nothing
        Private m_Tax_Liable As String = Nothing
        Private m_Campaign_No As String = Nothing
        Private m_Sell_To_Contact_No As String = Nothing
        Private m_Bill_To_Contact_No As String = Nothing
        Private m_Responsibility_Center As String = Nothing
        Private m_Id As Integer = Nothing


        Public Property Sell_To_Customer_No() As String
            Get
                Return m_Sell_To_Customer_No
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Customer_No = Value
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

        Public Property Bill_To_Customer_No() As String
            Get
                Return m_Bill_To_Customer_No
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Customer_No = Value
            End Set
        End Property

        Public Property Bill_To_Name() As String
            Get
                Return m_Bill_To_Name
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Name = Value
            End Set
        End Property

        Public Property Bill_To_Name_2() As String
            Get
                Return m_Bill_To_Name_2
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Name_2 = Value
            End Set
        End Property

        Public Property Bill_To_Address() As String
            Get
                Return m_Bill_To_Address
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Address = Value
            End Set
        End Property

        Public Property Bill_To_Address_2() As String
            Get
                Return m_Bill_To_Address_2
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Address_2 = Value
            End Set
        End Property

        Public Property Bill_To_City() As String
            Get
                Return m_Bill_To_City
            End Get
            Set(ByVal Value As String)
                m_Bill_To_City = Value
            End Set
        End Property

        Public Property Bill_To_Contact() As String
            Get
                Return m_Bill_To_Contact
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Contact = Value
            End Set
        End Property

        Public Property Your_Reference() As String
            Get
                Return m_Your_Reference
            End Get
            Set(ByVal Value As String)
                m_Your_Reference = Value
            End Set
        End Property

        Public Property Ship_To_Code() As String
            Get
                Return m_Ship_To_Code
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Code = Value
            End Set
        End Property

        Public Property Ship_To_Name() As String
            Get
                Return m_Ship_To_Name
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Name = Value
            End Set
        End Property

        Public Property Ship_To_Name_2() As String
            Get
                Return m_Ship_To_Name_2
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Name_2 = Value
            End Set
        End Property

        Public Property Ship_To_Address() As String
            Get
                Return m_Ship_To_Address
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Address = Value
            End Set
        End Property

        Public Property Ship_To_Address_2() As String
            Get
                Return m_Ship_To_Address_2
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Address_2 = Value
            End Set
        End Property

        Public Property Ship_To_City() As String
            Get
                Return m_Ship_To_City
            End Get
            Set(ByVal Value As String)
                m_Ship_To_City = Value
            End Set
        End Property

        Public Property Ship_To_Contact() As String
            Get
                Return m_Ship_To_Contact
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Contact = Value
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

        Public Property Posting_Description() As String
            Get
                Return m_Posting_Description
            End Get
            Set(ByVal Value As String)
                m_Posting_Description = Value
            End Set
        End Property

        Public Property Payment_Terms_Code() As String
            Get
                Return m_Payment_Terms_Code
            End Get
            Set(ByVal Value As String)
                m_Payment_Terms_Code = Value
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

        Public Property Payment_Discount_Percent() As Double
            Get
                Return m_Payment_Discount_Percent
            End Get
            Set(ByVal Value As Double)
                m_Payment_Discount_Percent = Value
            End Set
        End Property

        Public Property Pmt_Discount() As DateTime
            Get
                Return m_Pmt_Discount
            End Get
            Set(ByVal Value As DateTime)
                m_Pmt_Discount = Value
            End Set
        End Property

        Public Property Shipment_Method_Code() As String
            Get
                Return m_Shipment_Method_Code
            End Get
            Set(ByVal Value As String)
                m_Shipment_Method_Code = Value
            End Set
        End Property

        Public Property Location_Code() As String
            Get
                Return m_Location_Code
            End Get
            Set(ByVal Value As String)
                m_Location_Code = Value
            End Set
        End Property

        Public Property Currency_Code() As String
            Get
                Return m_Currency_Code
            End Get
            Set(ByVal Value As String)
                m_Currency_Code = Value
            End Set
        End Property

        Public Property Customer_Price_Group() As String
            Get
                Return m_Customer_Price_Group
            End Get
            Set(ByVal Value As String)
                m_Customer_Price_Group = Value
            End Set
        End Property

        Public Property Prices_Including_VAT() As String
            Get
                Return m_Prices_Including_VAT
            End Get
            Set(ByVal Value As String)
                m_Prices_Including_VAT = Value
            End Set
        End Property

        Public Property Invoice_Disc_Code() As String
            Get
                Return m_Invoice_Disc_Code
            End Get
            Set(ByVal Value As String)
                m_Invoice_Disc_Code = Value
            End Set
        End Property

        Public Property Customer_Disc_Group() As String
            Get
                Return m_Customer_Disc_Group
            End Get
            Set(ByVal Value As String)
                m_Customer_Disc_Group = Value
            End Set
        End Property

        Public Property Language_Code() As String
            Get
                Return m_Language_Code
            End Get
            Set(ByVal Value As String)
                m_Language_Code = Value
            End Set
        End Property

        Public Property Salesperson_Code() As String
            Get
                Return m_Salesperson_Code
            End Get
            Set(ByVal Value As String)
                m_Salesperson_Code = Value
            End Set
        End Property

        Public Property Applies_to_Doc_Type() As String
            Get
                Return m_Applies_to_Doc_Type
            End Get
            Set(ByVal Value As String)
                m_Applies_to_Doc_Type = Value
            End Set
        End Property

        Public Property Applies_to_Doc_No() As String
            Get
                Return m_Applies_to_Doc_No
            End Get
            Set(ByVal Value As String)
                m_Applies_to_Doc_No = Value
            End Set
        End Property

        Public Property Bal_Account_No() As String
            Get
                Return m_Bal_Account_No
            End Get
            Set(ByVal Value As String)
                m_Bal_Account_No = Value
            End Set
        End Property

        Public Property Job_No() As String
            Get
                Return m_Job_No
            End Get
            Set(ByVal Value As String)
                m_Job_No = Value
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

        Public Property Amount_Including_VAT() As Double
            Get
                Return m_Amount_Including_VAT
            End Get
            Set(ByVal Value As Double)
                m_Amount_Including_VAT = Value
            End Set
        End Property

        Public Property VAT_Registration_No() As String
            Get
                Return m_VAT_Registration_No
            End Get
            Set(ByVal Value As String)
                m_VAT_Registration_No = Value
            End Set
        End Property

        Public Property Reason_Code() As String
            Get
                Return m_Reason_Code
            End Get
            Set(ByVal Value As String)
                m_Reason_Code = Value
            End Set
        End Property

        Public Property Transaction_Type() As String
            Get
                Return m_Transaction_Type
            End Get
            Set(ByVal Value As String)
                m_Transaction_Type = Value
            End Set
        End Property

        Public Property Transport_Method() As String
            Get
                Return m_Transport_Method
            End Get
            Set(ByVal Value As String)
                m_Transport_Method = Value
            End Set
        End Property

        Public Property Sell_To_Customer_Name() As String
            Get
                Return m_Sell_To_Customer_Name
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Customer_Name = Value
            End Set
        End Property

        Public Property Sell_To_Customer_Name_2() As String
            Get
                Return m_Sell_To_Customer_Name_2
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Customer_Name_2 = Value
            End Set
        End Property

        Public Property Sell_To_Address() As String
            Get
                Return m_Sell_To_Address
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Address = Value
            End Set
        End Property

        Public Property Sell_To_Address_2() As String
            Get
                Return m_Sell_To_Address_2
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Address_2 = Value
            End Set
        End Property

        Public Property Sell_To_City() As String
            Get
                Return m_Sell_To_City
            End Get
            Set(ByVal Value As String)
                m_Sell_To_City = Value
            End Set
        End Property

        Public Property Sell_To_Contact() As String
            Get
                Return m_Sell_To_Contact
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Contact = Value
            End Set
        End Property

        Public Property Bill_To_Post_Code() As String
            Get
                Return m_Bill_To_Post_Code
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Post_Code = Value
            End Set
        End Property

        Public Property Bill_To_County() As String
            Get
                Return m_Bill_To_County
            End Get
            Set(ByVal Value As String)
                m_Bill_To_County = Value
            End Set
        End Property

        Public Property Bill_To_Country_Code() As String
            Get
                Return m_Bill_To_Country_Code
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Country_Code = Value
            End Set
        End Property

        Public Property Sell_To_Post_Code() As String
            Get
                Return m_Sell_To_Post_Code
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Post_Code = Value
            End Set
        End Property

        Public Property Sell_To_County() As String
            Get
                Return m_Sell_To_County
            End Get
            Set(ByVal Value As String)
                m_Sell_To_County = Value
            End Set
        End Property

        Public Property Sell_To_Country_Code() As String
            Get
                Return m_Sell_To_Country_Code
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Country_Code = Value
            End Set
        End Property

        Public Property Ship_To_Post_Code() As String
            Get
                Return m_Ship_To_Post_Code
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Post_Code = Value
            End Set
        End Property

        Public Property Ship_To_County() As String
            Get
                Return m_Ship_To_County
            End Get
            Set(ByVal Value As String)
                m_Ship_To_County = Value
            End Set
        End Property

        Public Property Ship_To_Country_Code() As String
            Get
                Return m_Ship_To_Country_Code
            End Get
            Set(ByVal Value As String)
                m_Ship_To_Country_Code = Value
            End Set
        End Property

        Public Property Bal_Account_Type() As String
            Get
                Return m_Bal_Account_Type
            End Get
            Set(ByVal Value As String)
                m_Bal_Account_Type = Value
            End Set
        End Property

        Public Property Document_datetime() As DateTime
            Get
                Return m_Document_datetime
            End Get
            Set(ByVal Value As DateTime)
                m_Document_datetime = Value
            End Set
        End Property

        Public Property External_Document_No() As String
            Get
                Return m_External_Document_No
            End Get
            Set(ByVal Value As String)
                m_External_Document_No = Value
            End Set
        End Property

        Public Property Payment_Method_Code() As String
            Get
                Return m_Payment_Method_Code
            End Get
            Set(ByVal Value As String)
                m_Payment_Method_Code = Value
            End Set
        End Property

        Public Property User_ID() As String
            Get
                Return m_User_ID
            End Get
            Set(ByVal Value As String)
                m_User_ID = Value
            End Set
        End Property

        Public Property Source_Code() As String
            Get
                Return m_Source_Code
            End Get
            Set(ByVal Value As String)
                m_Source_Code = Value
            End Set
        End Property

        Public Property Tax_Area_Code() As String
            Get
                Return m_Tax_Area_Code
            End Get
            Set(ByVal Value As String)
                m_Tax_Area_Code = Value
            End Set
        End Property

        Public Property Tax_Liable() As String
            Get
                Return m_Tax_Liable
            End Get
            Set(ByVal Value As String)
                m_Tax_Liable = Value
            End Set
        End Property

        Public Property Campaign_No() As String
            Get
                Return m_Campaign_No
            End Get
            Set(ByVal Value As String)
                m_Campaign_No = Value
            End Set
        End Property

        Public Property Sell_To_Contact_No() As String
            Get
                Return m_Sell_To_Contact_No
            End Get
            Set(ByVal Value As String)
                m_Sell_To_Contact_No = Value
            End Set
        End Property

        Public Property Bill_To_Contact_No() As String
            Get
                Return m_Bill_To_Contact_No
            End Get
            Set(ByVal Value As String)
                m_Bill_To_Contact_No = Value
            End Set
        End Property

        Public Property Responsibility_Center() As String
            Get
                Return m_Responsibility_Center
            End Get
            Set(ByVal Value As String)
                m_Responsibility_Center = Value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Public Sub New(ByVal DB As Database, ByVal Sell_To_Customer_No As String)
            m_DB = DB
            m_Sell_To_Customer_No = Sell_To_Customer_No
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM _NAVISION_SALES_CREDIT_MEMO_HEADER WHERE Sell_To_Customer_No = " & DB.Number(Sell_To_Customer_No)
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
            m_Sell_To_Customer_No = Convert.ToString(r.Item("Sell_To_Customer_No"))
            If IsDBNull(r.Item("No")) Then
                m_No = Nothing
            Else
                m_No = Convert.ToString(r.Item("No"))
            End If
            If IsDBNull(r.Item("Bill_To_Customer_No")) Then
                m_Bill_To_Customer_No = Nothing
            Else
                m_Bill_To_Customer_No = Convert.ToString(r.Item("Bill_To_Customer_No"))
            End If
            If IsDBNull(r.Item("Bill_To_Name")) Then
                m_Bill_To_Name = Nothing
            Else
                m_Bill_To_Name = Convert.ToString(r.Item("Bill_To_Name"))
            End If
            If IsDBNull(r.Item("Bill_To_Name_2")) Then
                m_Bill_To_Name_2 = Nothing
            Else
                m_Bill_To_Name_2 = Convert.ToString(r.Item("Bill_To_Name_2"))
            End If
            If IsDBNull(r.Item("Bill_To_Address")) Then
                m_Bill_To_Address = Nothing
            Else
                m_Bill_To_Address = Convert.ToString(r.Item("Bill_To_Address"))
            End If
            If IsDBNull(r.Item("Bill_To_Address_2")) Then
                m_Bill_To_Address_2 = Nothing
            Else
                m_Bill_To_Address_2 = Convert.ToString(r.Item("Bill_To_Address_2"))
            End If
            If IsDBNull(r.Item("Bill_To_City")) Then
                m_Bill_To_City = Nothing
            Else
                m_Bill_To_City = Convert.ToString(r.Item("Bill_To_City"))
            End If
            If IsDBNull(r.Item("Bill_To_Contact")) Then
                m_Bill_To_Contact = Nothing
            Else
                m_Bill_To_Contact = Convert.ToString(r.Item("Bill_To_Contact"))
            End If
            If IsDBNull(r.Item("Your_Reference")) Then
                m_Your_Reference = Nothing
            Else
                m_Your_Reference = Convert.ToString(r.Item("Your_Reference"))
            End If
            If IsDBNull(r.Item("Ship_To_Code")) Then
                m_Ship_To_Code = Nothing
            Else
                m_Ship_To_Code = Convert.ToString(r.Item("Ship_To_Code"))
            End If
            If IsDBNull(r.Item("Ship_To_Name")) Then
                m_Ship_To_Name = Nothing
            Else
                m_Ship_To_Name = Convert.ToString(r.Item("Ship_To_Name"))
            End If
            If IsDBNull(r.Item("Ship_To_Name_2")) Then
                m_Ship_To_Name_2 = Nothing
            Else
                m_Ship_To_Name_2 = Convert.ToString(r.Item("Ship_To_Name_2"))
            End If
            If IsDBNull(r.Item("Ship_To_Address")) Then
                m_Ship_To_Address = Nothing
            Else
                m_Ship_To_Address = Convert.ToString(r.Item("Ship_To_Address"))
            End If
            If IsDBNull(r.Item("Ship_To_Address_2")) Then
                m_Ship_To_Address_2 = Nothing
            Else
                m_Ship_To_Address_2 = Convert.ToString(r.Item("Ship_To_Address_2"))
            End If
            If IsDBNull(r.Item("Ship_To_City")) Then
                m_Ship_To_City = Nothing
            Else
                m_Ship_To_City = Convert.ToString(r.Item("Ship_To_City"))
            End If
            If IsDBNull(r.Item("Ship_To_Contact")) Then
                m_Ship_To_Contact = Nothing
            Else
                m_Ship_To_Contact = Convert.ToString(r.Item("Ship_To_Contact"))
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
            If IsDBNull(r.Item("Posting_Description")) Then
                m_Posting_Description = Nothing
            Else
                m_Posting_Description = Convert.ToString(r.Item("Posting_Description"))
            End If
            If IsDBNull(r.Item("Payment_Terms_Code")) Then
                m_Payment_Terms_Code = Nothing
            Else
                m_Payment_Terms_Code = Convert.ToString(r.Item("Payment_Terms_Code"))
            End If
            If IsDBNull(r.Item("Due")) Then
                m_Due = Nothing
            Else
                m_Due = Convert.ToDateTime(r.Item("Due"))
            End If
            If IsDBNull(r.Item("Payment_Discount_Percent")) Then
                m_Payment_Discount_Percent = Nothing
            Else
                m_Payment_Discount_Percent = Convert.ToDouble(r.Item("Payment_Discount_Percent"))
            End If
            If IsDBNull(r.Item("Pmt_Discount")) Then
                m_Pmt_Discount = Nothing
            Else
                m_Pmt_Discount = Convert.ToDateTime(r.Item("Pmt_Discount"))
            End If
            If IsDBNull(r.Item("Shipment_Method_Code")) Then
                m_Shipment_Method_Code = Nothing
            Else
                m_Shipment_Method_Code = Convert.ToString(r.Item("Shipment_Method_Code"))
            End If
            If IsDBNull(r.Item("Location_Code")) Then
                m_Location_Code = Nothing
            Else
                m_Location_Code = Convert.ToString(r.Item("Location_Code"))
            End If
            If IsDBNull(r.Item("Currency_Code")) Then
                m_Currency_Code = Nothing
            Else
                m_Currency_Code = Convert.ToString(r.Item("Currency_Code"))
            End If
            If IsDBNull(r.Item("Customer_Price_Group")) Then
                m_Customer_Price_Group = Nothing
            Else
                m_Customer_Price_Group = Convert.ToString(r.Item("Customer_Price_Group"))
            End If
            If IsDBNull(r.Item("Prices_Including_VAT")) Then
                m_Prices_Including_VAT = Nothing
            Else
                m_Prices_Including_VAT = Convert.ToString(r.Item("Prices_Including_VAT"))
            End If
            If IsDBNull(r.Item("Invoice_Disc_Code")) Then
                m_Invoice_Disc_Code = Nothing
            Else
                m_Invoice_Disc_Code = Convert.ToString(r.Item("Invoice_Disc_Code"))
            End If
            If IsDBNull(r.Item("Customer_Disc_Group")) Then
                m_Customer_Disc_Group = Nothing
            Else
                m_Customer_Disc_Group = Convert.ToString(r.Item("Customer_Disc_Group"))
            End If
            If IsDBNull(r.Item("Language_Code")) Then
                m_Language_Code = Nothing
            Else
                m_Language_Code = Convert.ToString(r.Item("Language_Code"))
            End If
            If IsDBNull(r.Item("Salesperson_Code")) Then
                m_Salesperson_Code = Nothing
            Else
                m_Salesperson_Code = Convert.ToString(r.Item("Salesperson_Code"))
            End If
            If IsDBNull(r.Item("Applies_to_Doc_Type")) Then
                m_Applies_to_Doc_Type = Nothing
            Else
                m_Applies_to_Doc_Type = Convert.ToString(r.Item("Applies_to_Doc_Type"))
            End If
            If IsDBNull(r.Item("Applies_to_Doc_No")) Then
                m_Applies_to_Doc_No = Nothing
            Else
                m_Applies_to_Doc_No = Convert.ToString(r.Item("Applies_to_Doc_No"))
            End If
            If IsDBNull(r.Item("Bal_Account_No")) Then
                m_Bal_Account_No = Nothing
            Else
                m_Bal_Account_No = Convert.ToString(r.Item("Bal_Account_No"))
            End If
            If IsDBNull(r.Item("Job_No")) Then
                m_Job_No = Nothing
            Else
                m_Job_No = Convert.ToString(r.Item("Job_No"))
            End If
            If IsDBNull(r.Item("Amount")) Then
                m_Amount = Nothing
            Else
                m_Amount = Convert.ToDouble(r.Item("Amount"))
            End If
            If IsDBNull(r.Item("Amount_Including_VAT")) Then
                m_Amount_Including_VAT = Nothing
            Else
                m_Amount_Including_VAT = Convert.ToDouble(r.Item("Amount_Including_VAT"))
            End If
            If IsDBNull(r.Item("VAT_Registration_No")) Then
                m_VAT_Registration_No = Nothing
            Else
                m_VAT_Registration_No = Convert.ToString(r.Item("VAT_Registration_No"))
            End If
            If IsDBNull(r.Item("Reason_Code")) Then
                m_Reason_Code = Nothing
            Else
                m_Reason_Code = Convert.ToString(r.Item("Reason_Code"))
            End If
            If IsDBNull(r.Item("Transaction_Type")) Then
                m_Transaction_Type = Nothing
            Else
                m_Transaction_Type = Convert.ToString(r.Item("Transaction_Type"))
            End If
            If IsDBNull(r.Item("Transport_Method")) Then
                m_Transport_Method = Nothing
            Else
                m_Transport_Method = Convert.ToString(r.Item("Transport_Method"))
            End If
            If IsDBNull(r.Item("Sell_To_Customer_Name")) Then
                m_Sell_To_Customer_Name = Nothing
            Else
                m_Sell_To_Customer_Name = Convert.ToString(r.Item("Sell_To_Customer_Name"))
            End If
            If IsDBNull(r.Item("Sell_To_Customer_Name_2")) Then
                m_Sell_To_Customer_Name_2 = Nothing
            Else
                m_Sell_To_Customer_Name_2 = Convert.ToString(r.Item("Sell_To_Customer_Name_2"))
            End If
            If IsDBNull(r.Item("Sell_To_Address")) Then
                m_Sell_To_Address = Nothing
            Else
                m_Sell_To_Address = Convert.ToString(r.Item("Sell_To_Address"))
            End If
            If IsDBNull(r.Item("Sell_To_Address_2")) Then
                m_Sell_To_Address_2 = Nothing
            Else
                m_Sell_To_Address_2 = Convert.ToString(r.Item("Sell_To_Address_2"))
            End If
            If IsDBNull(r.Item("Sell_To_City")) Then
                m_Sell_To_City = Nothing
            Else
                m_Sell_To_City = Convert.ToString(r.Item("Sell_To_City"))
            End If
            If IsDBNull(r.Item("Sell_To_Contact")) Then
                m_Sell_To_Contact = Nothing
            Else
                m_Sell_To_Contact = Convert.ToString(r.Item("Sell_To_Contact"))
            End If
            If IsDBNull(r.Item("Bill_To_Post_Code")) Then
                m_Bill_To_Post_Code = Nothing
            Else
                m_Bill_To_Post_Code = Convert.ToString(r.Item("Bill_To_Post_Code"))
            End If
            If IsDBNull(r.Item("Bill_To_County")) Then
                m_Bill_To_County = Nothing
            Else
                m_Bill_To_County = Convert.ToString(r.Item("Bill_To_County"))
            End If
            If IsDBNull(r.Item("Bill_To_Country_Code")) Then
                m_Bill_To_Country_Code = Nothing
            Else
                m_Bill_To_Country_Code = Convert.ToString(r.Item("Bill_To_Country_Code"))
            End If
            If IsDBNull(r.Item("Sell_To_Post_Code")) Then
                m_Sell_To_Post_Code = Nothing
            Else
                m_Sell_To_Post_Code = Convert.ToString(r.Item("Sell_To_Post_Code"))
            End If
            If IsDBNull(r.Item("Sell_To_County")) Then
                m_Sell_To_County = Nothing
            Else
                m_Sell_To_County = Convert.ToString(r.Item("Sell_To_County"))
            End If
            If IsDBNull(r.Item("Sell_To_Country_Code")) Then
                m_Sell_To_Country_Code = Nothing
            Else
                m_Sell_To_Country_Code = Convert.ToString(r.Item("Sell_To_Country_Code"))
            End If
            If IsDBNull(r.Item("Ship_To_Post_Code")) Then
                m_Ship_To_Post_Code = Nothing
            Else
                m_Ship_To_Post_Code = Convert.ToString(r.Item("Ship_To_Post_Code"))
            End If
            If IsDBNull(r.Item("Ship_To_County")) Then
                m_Ship_To_County = Nothing
            Else
                m_Ship_To_County = Convert.ToString(r.Item("Ship_To_County"))
            End If
            If IsDBNull(r.Item("Ship_To_Country_Code")) Then
                m_Ship_To_Country_Code = Nothing
            Else
                m_Ship_To_Country_Code = Convert.ToString(r.Item("Ship_To_Country_Code"))
            End If
            If IsDBNull(r.Item("Bal_Account_Type")) Then
                m_Bal_Account_Type = Nothing
            Else
                m_Bal_Account_Type = Convert.ToString(r.Item("Bal_Account_Type"))
            End If
            If IsDBNull(r.Item("Document_datetime")) Then
                m_Document_datetime = Nothing
            Else
                m_Document_datetime = Convert.ToDateTime(r.Item("Document_datetime"))
            End If
            If IsDBNull(r.Item("External_Document_No")) Then
                m_External_Document_No = Nothing
            Else
                m_External_Document_No = Convert.ToString(r.Item("External_Document_No"))
            End If
            If IsDBNull(r.Item("Payment_Method_Code")) Then
                m_Payment_Method_Code = Nothing
            Else
                m_Payment_Method_Code = Convert.ToString(r.Item("Payment_Method_Code"))
            End If
            If IsDBNull(r.Item("User_ID")) Then
                m_User_ID = Nothing
            Else
                m_User_ID = Convert.ToString(r.Item("User_ID"))
            End If
            If IsDBNull(r.Item("Source_Code")) Then
                m_Source_Code = Nothing
            Else
                m_Source_Code = Convert.ToString(r.Item("Source_Code"))
            End If
            If IsDBNull(r.Item("Tax_Area_Code")) Then
                m_Tax_Area_Code = Nothing
            Else
                m_Tax_Area_Code = Convert.ToString(r.Item("Tax_Area_Code"))
            End If
            If IsDBNull(r.Item("Tax_Liable")) Then
                m_Tax_Liable = Nothing
            Else
                m_Tax_Liable = Convert.ToString(r.Item("Tax_Liable"))
            End If
            If IsDBNull(r.Item("Campaign_No")) Then
                m_Campaign_No = Nothing
            Else
                m_Campaign_No = Convert.ToString(r.Item("Campaign_No"))
            End If
            If IsDBNull(r.Item("Sell_To_Contact_No")) Then
                m_Sell_To_Contact_No = Nothing
            Else
                m_Sell_To_Contact_No = Convert.ToString(r.Item("Sell_To_Contact_No"))
            End If
            If IsDBNull(r.Item("Bill_To_Contact_No")) Then
                m_Bill_To_Contact_No = Nothing
            Else
                m_Bill_To_Contact_No = Convert.ToString(r.Item("Bill_To_Contact_No"))
            End If
            If IsDBNull(r.Item("Responsibility_Center")) Then
                m_Responsibility_Center = Nothing
            Else
                m_Responsibility_Center = Convert.ToString(r.Item("Responsibility_Center"))
            End If
            m_Id = Convert.ToInt32(r.Item("Id"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO _NAVISION_SALES_CREDIT_MEMO_HEADER (" _
             & " No" _
             & ",Bill_To_Customer_No" _
             & ",Bill_To_Name" _
             & ",Bill_To_Name_2" _
             & ",Bill_To_Address" _
             & ",Bill_To_Address_2" _
             & ",Bill_To_City" _
             & ",Bill_To_Contact" _
             & ",Your_Reference" _
             & ",Ship_To_Code" _
             & ",Ship_To_Name" _
             & ",Ship_To_Name_2" _
             & ",Ship_To_Address" _
             & ",Ship_To_Address_2" _
             & ",Ship_To_City" _
             & ",Ship_To_Contact" _
             & ",Posting" _
             & ",Shipment" _
             & ",Posting_Description" _
             & ",Payment_Terms_Code" _
             & ",Due" _
             & ",Payment_Discount_Percent" _
             & ",Pmt_Discount" _
             & ",Shipment_Method_Code" _
             & ",Location_Code" _
             & ",Currency_Code" _
             & ",Customer_Price_Group" _
             & ",Prices_Including_VAT" _
             & ",Invoice_Disc_Code" _
             & ",Customer_Disc_Group" _
             & ",Language_Code" _
             & ",Salesperson_Code" _
             & ",Applies_to_Doc_Type" _
             & ",Applies_to_Doc_No" _
             & ",Bal_Account_No" _
             & ",Job_No" _
             & ",Amount" _
             & ",Amount_Including_VAT" _
             & ",VAT_Registration_No" _
             & ",Reason_Code" _
             & ",Transaction_Type" _
             & ",Transport_Method" _
             & ",Sell_To_Customer_Name" _
             & ",Sell_To_Customer_Name_2" _
             & ",Sell_To_Address" _
             & ",Sell_To_Address_2" _
             & ",Sell_To_City" _
             & ",Sell_To_Contact" _
             & ",Bill_To_Post_Code" _
             & ",Bill_To_County" _
             & ",Bill_To_Country_Code" _
             & ",Sell_To_Post_Code" _
             & ",Sell_To_County" _
             & ",Sell_To_Country_Code" _
             & ",Ship_To_Post_Code" _
             & ",Ship_To_County" _
             & ",Ship_To_Country_Code" _
             & ",Bal_Account_Type" _
             & ",Document_datetime" _
             & ",External_Document_No" _
             & ",Payment_Method_Code" _
             & ",User_ID" _
             & ",Source_Code" _
             & ",Tax_Area_Code" _
             & ",Tax_Liable" _
             & ",Campaign_No" _
             & ",Sell_To_Contact_No" _
             & ",Bill_To_Contact_No" _
             & ",Responsibility_Center" _
             & ",Id" _
             & ") VALUES (" _
             & m_DB.Quote(No) _
             & "," & m_DB.Quote(Bill_To_Customer_No) _
             & "," & m_DB.Quote(Bill_To_Name) _
             & "," & m_DB.Quote(Bill_To_Name_2) _
             & "," & m_DB.Quote(Bill_To_Address) _
             & "," & m_DB.Quote(Bill_To_Address_2) _
             & "," & m_DB.Quote(Bill_To_City) _
             & "," & m_DB.Quote(Bill_To_Contact) _
             & "," & m_DB.Quote(Your_Reference) _
             & "," & m_DB.Quote(Ship_To_Code) _
             & "," & m_DB.Quote(Ship_To_Name) _
             & "," & m_DB.Quote(Ship_To_Name_2) _
             & "," & m_DB.Quote(Ship_To_Address) _
             & "," & m_DB.Quote(Ship_To_Address_2) _
             & "," & m_DB.Quote(Ship_To_City) _
             & "," & m_DB.Quote(Ship_To_Contact) _
             & "," & m_DB.NullQuote(Posting) _
             & "," & m_DB.NullQuote(Shipment) _
             & "," & m_DB.Quote(Posting_Description) _
             & "," & m_DB.Quote(Payment_Terms_Code) _
             & "," & m_DB.NullQuote(Due) _
             & "," & m_DB.Number(Payment_Discount_Percent) _
             & "," & m_DB.NullQuote(Pmt_Discount) _
             & "," & m_DB.Quote(Shipment_Method_Code) _
             & "," & m_DB.Quote(Location_Code) _
             & "," & m_DB.Quote(Currency_Code) _
             & "," & m_DB.Quote(Customer_Price_Group) _
             & "," & m_DB.Quote(Prices_Including_VAT) _
             & "," & m_DB.Quote(Invoice_Disc_Code) _
             & "," & m_DB.Quote(Customer_Disc_Group) _
             & "," & m_DB.Quote(Language_Code) _
             & "," & m_DB.Quote(Salesperson_Code) _
             & "," & m_DB.Quote(Applies_to_Doc_Type) _
             & "," & m_DB.Quote(Applies_to_Doc_No) _
             & "," & m_DB.Quote(Bal_Account_No) _
             & "," & m_DB.Quote(Job_No) _
             & "," & m_DB.Number(Amount) _
             & "," & m_DB.Number(Amount_Including_VAT) _
             & "," & m_DB.Quote(VAT_Registration_No) _
             & "," & m_DB.Quote(Reason_Code) _
             & "," & m_DB.Quote(Transaction_Type) _
             & "," & m_DB.Quote(Transport_Method) _
             & "," & m_DB.Quote(Sell_To_Customer_Name) _
             & "," & m_DB.Quote(Sell_To_Customer_Name_2) _
             & "," & m_DB.Quote(Sell_To_Address) _
             & "," & m_DB.Quote(Sell_To_Address_2) _
             & "," & m_DB.Quote(Sell_To_City) _
             & "," & m_DB.Quote(Sell_To_Contact) _
             & "," & m_DB.Quote(Bill_To_Post_Code) _
             & "," & m_DB.Quote(Bill_To_County) _
             & "," & m_DB.Quote(Bill_To_Country_Code) _
             & "," & m_DB.Quote(Sell_To_Post_Code) _
             & "," & m_DB.Quote(Sell_To_County) _
             & "," & m_DB.Quote(Sell_To_Country_Code) _
             & "," & m_DB.Quote(Ship_To_Post_Code) _
             & "," & m_DB.Quote(Ship_To_County) _
             & "," & m_DB.Quote(Ship_To_Country_Code) _
             & "," & m_DB.Quote(Bal_Account_Type) _
             & "," & m_DB.NullQuote(Document_datetime) _
             & "," & m_DB.Quote(External_Document_No) _
             & "," & m_DB.Quote(Payment_Method_Code) _
             & "," & m_DB.Quote(User_ID) _
             & "," & m_DB.Quote(Source_Code) _
             & "," & m_DB.Quote(Tax_Area_Code) _
             & "," & m_DB.Quote(Tax_Liable) _
             & "," & m_DB.Quote(Campaign_No) _
             & "," & m_DB.Quote(Sell_To_Contact_No) _
             & "," & m_DB.Quote(Bill_To_Contact_No) _
             & "," & m_DB.Quote(Responsibility_Center) _
             & "," & m_DB.NullNumber(Id) _
             & ")"

            Sell_To_Customer_No = m_DB.InsertSQL(SQL)

            Return Sell_To_Customer_No
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE _NAVISION_SALES_CREDIT_MEMO_HEADER SET " _
             & " No = " & m_DB.Quote(No) _
             & ",Bill_To_Customer_No = " & m_DB.Quote(Bill_To_Customer_No) _
             & ",Bill_To_Name = " & m_DB.Quote(Bill_To_Name) _
             & ",Bill_To_Name_2 = " & m_DB.Quote(Bill_To_Name_2) _
             & ",Bill_To_Address = " & m_DB.Quote(Bill_To_Address) _
             & ",Bill_To_Address_2 = " & m_DB.Quote(Bill_To_Address_2) _
             & ",Bill_To_City = " & m_DB.Quote(Bill_To_City) _
             & ",Bill_To_Contact = " & m_DB.Quote(Bill_To_Contact) _
             & ",Your_Reference = " & m_DB.Quote(Your_Reference) _
             & ",Ship_To_Code = " & m_DB.Quote(Ship_To_Code) _
             & ",Ship_To_Name = " & m_DB.Quote(Ship_To_Name) _
             & ",Ship_To_Name_2 = " & m_DB.Quote(Ship_To_Name_2) _
             & ",Ship_To_Address = " & m_DB.Quote(Ship_To_Address) _
             & ",Ship_To_Address_2 = " & m_DB.Quote(Ship_To_Address_2) _
             & ",Ship_To_City = " & m_DB.Quote(Ship_To_City) _
             & ",Ship_To_Contact = " & m_DB.Quote(Ship_To_Contact) _
             & ",Posting = " & m_DB.NullQuote(Posting) _
             & ",Shipment = " & m_DB.NullQuote(Shipment) _
             & ",Posting_Description = " & m_DB.Quote(Posting_Description) _
             & ",Payment_Terms_Code = " & m_DB.Quote(Payment_Terms_Code) _
             & ",Due = " & m_DB.NullQuote(Due) _
             & ",Payment_Discount_Percent = " & m_DB.Number(Payment_Discount_Percent) _
             & ",Pmt_Discount = " & m_DB.NullQuote(Pmt_Discount) _
             & ",Shipment_Method_Code = " & m_DB.Quote(Shipment_Method_Code) _
             & ",Location_Code = " & m_DB.Quote(Location_Code) _
             & ",Currency_Code = " & m_DB.Quote(Currency_Code) _
             & ",Customer_Price_Group = " & m_DB.Quote(Customer_Price_Group) _
             & ",Prices_Including_VAT = " & m_DB.Quote(Prices_Including_VAT) _
             & ",Invoice_Disc_Code = " & m_DB.Quote(Invoice_Disc_Code) _
             & ",Customer_Disc_Group = " & m_DB.Quote(Customer_Disc_Group) _
             & ",Language_Code = " & m_DB.Quote(Language_Code) _
             & ",Salesperson_Code = " & m_DB.Quote(Salesperson_Code) _
             & ",Applies_to_Doc_Type = " & m_DB.Quote(Applies_to_Doc_Type) _
             & ",Applies_to_Doc_No = " & m_DB.Quote(Applies_to_Doc_No) _
             & ",Bal_Account_No = " & m_DB.Quote(Bal_Account_No) _
             & ",Job_No = " & m_DB.Quote(Job_No) _
             & ",Amount = " & m_DB.Number(Amount) _
             & ",Amount_Including_VAT = " & m_DB.Number(Amount_Including_VAT) _
             & ",VAT_Registration_No = " & m_DB.Quote(VAT_Registration_No) _
             & ",Reason_Code = " & m_DB.Quote(Reason_Code) _
             & ",Transaction_Type = " & m_DB.Quote(Transaction_Type) _
             & ",Transport_Method = " & m_DB.Quote(Transport_Method) _
             & ",Sell_To_Customer_Name = " & m_DB.Quote(Sell_To_Customer_Name) _
             & ",Sell_To_Customer_Name_2 = " & m_DB.Quote(Sell_To_Customer_Name_2) _
             & ",Sell_To_Address = " & m_DB.Quote(Sell_To_Address) _
             & ",Sell_To_Address_2 = " & m_DB.Quote(Sell_To_Address_2) _
             & ",Sell_To_City = " & m_DB.Quote(Sell_To_City) _
             & ",Sell_To_Contact = " & m_DB.Quote(Sell_To_Contact) _
             & ",Bill_To_Post_Code = " & m_DB.Quote(Bill_To_Post_Code) _
             & ",Bill_To_County = " & m_DB.Quote(Bill_To_County) _
             & ",Bill_To_Country_Code = " & m_DB.Quote(Bill_To_Country_Code) _
             & ",Sell_To_Post_Code = " & m_DB.Quote(Sell_To_Post_Code) _
             & ",Sell_To_County = " & m_DB.Quote(Sell_To_County) _
             & ",Sell_To_Country_Code = " & m_DB.Quote(Sell_To_Country_Code) _
             & ",Ship_To_Post_Code = " & m_DB.Quote(Ship_To_Post_Code) _
             & ",Ship_To_County = " & m_DB.Quote(Ship_To_County) _
             & ",Ship_To_Country_Code = " & m_DB.Quote(Ship_To_Country_Code) _
             & ",Bal_Account_Type = " & m_DB.Quote(Bal_Account_Type) _
             & ",Document_datetime = " & m_DB.NullQuote(Document_datetime) _
             & ",External_Document_No = " & m_DB.Quote(External_Document_No) _
             & ",Payment_Method_Code = " & m_DB.Quote(Payment_Method_Code) _
             & ",User_ID = " & m_DB.Quote(User_ID) _
             & ",Source_Code = " & m_DB.Quote(Source_Code) _
             & ",Tax_Area_Code = " & m_DB.Quote(Tax_Area_Code) _
             & ",Tax_Liable = " & m_DB.Quote(Tax_Liable) _
             & ",Campaign_No = " & m_DB.Quote(Campaign_No) _
             & ",Sell_To_Contact_No = " & m_DB.Quote(Sell_To_Contact_No) _
             & ",Bill_To_Contact_No = " & m_DB.Quote(Bill_To_Contact_No) _
             & ",Responsibility_Center = " & m_DB.Quote(Responsibility_Center) _
             & ",Id = " & m_DB.NullNumber(Id) _
             & " WHERE Sell_To_Customer_No = " & m_DB.Quote(Sell_To_Customer_No)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM _NAVISION_SALES_CREDIT_MEMO_HEADER WHERE Sell_To_Customer_No = " & m_DB.Number(Sell_To_Customer_No)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class NavisionSalesCreditMemoHeaderCollection
        Inherits GenericCollection(Of NavisionSalesCreditMemoHeaderRow)
    End Class

End Namespace


