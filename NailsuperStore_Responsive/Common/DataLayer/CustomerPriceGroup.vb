Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class CustomerPriceGroupRow
        Inherits CustomerPriceGroupRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CustomerPriceGroupId As Integer)
            MyBase.New(DB, CustomerPriceGroupId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CustomerPriceGroupCode As String)
            MyBase.New(DB, CustomerPriceGroupCode)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CustomerPriceGroupId As Integer) As CustomerPriceGroupRow
            Dim row As CustomerPriceGroupRow

            row = New CustomerPriceGroupRow(DB, CustomerPriceGroupId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal CustomerPriceGroupCode As String) As CustomerPriceGroupRow
            Dim row As CustomerPriceGroupRow

            row = New CustomerPriceGroupRow(DB, CustomerPriceGroupCode)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CustomerPriceGroupId As Integer)
            Dim row As CustomerPriceGroupRow

            row = New CustomerPriceGroupRow(DB, CustomerPriceGroupId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionCustomerPriceGroupRow)
            CustomerPriceGroupCode = r.Code
            PriceIncludesVAT = IIf(r.Price_Includes_VAT = "Y", True, False)
            AllowInvoiceDisc = IIf(r.Allow_Invoice_Disc = "Y", True, False)
            VATBusPostingGroup = Trim(r.VAT_Bus_Posting_Gr)
            Description = Trim(r.Description)
            AllowLineDiscount = IIf(r.Allow_Line_Disc = "Y", True, False)
            CurrencyCode = Trim(r.Currency_Code)
            DistributionGroupCode = Trim(r.Distribution_Group_Code)
            DistributionSubgroupCode = Trim(r.Distribution_Subgroup_Code)
            RetailPriceGroup = IIf(r.Retail_Price_Group = "Y", True, False)

            If CustomerPriceGroupId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub

        Public Shared Function GetAllCustomerPriceGroups(ByVal DB As Database) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select *, ltrim(rtrim(customerpricegroupcode)) as codewithcount from CustomerPriceGroup g order by customerpricegroupcode")
            Return dt
        End Function
    End Class

    Public MustInherit Class CustomerPriceGroupRowBase
        Private m_DB As Database
        Private m_CustomerPriceGroupId As Integer = Nothing
        Private m_CustomerPriceGroupCode As String = Nothing
        Private m_PriceIncludesVAT As Boolean = Nothing
        Private m_AllowInvoiceDisc As Boolean = Nothing
        Private m_VATBusPostingGroup As String = Nothing
        Private m_Description As String = Nothing
        Private m_AllowLineDiscount As Boolean = Nothing
        Private m_CurrencyCode As String = Nothing
        Private m_DistributionGroupCode As String = Nothing
        Private m_DistributionSubgroupCode As String = Nothing
        Private m_RetailPriceGroup As Boolean = Nothing

        Public Property CustomerPriceGroupId() As Integer
            Get
                Return m_CustomerPriceGroupId
            End Get
            Set(ByVal Value As Integer)
                m_CustomerPriceGroupId = Value
            End Set
        End Property

        Public Property CustomerPriceGroupCode() As String
            Get
                Return m_CustomerPriceGroupCode
            End Get
            Set(ByVal Value As String)
                m_CustomerPriceGroupCode = Value
            End Set
        End Property

        Public Property PriceIncludesVAT() As Boolean
            Get
                Return m_PriceIncludesVAT
            End Get
            Set(ByVal Value As Boolean)
                m_PriceIncludesVAT = Value
            End Set
        End Property

        Public Property AllowInvoiceDisc() As Boolean
            Get
                Return m_AllowInvoiceDisc
            End Get
            Set(ByVal Value As Boolean)
                m_AllowInvoiceDisc = Value
            End Set
        End Property

        Public Property VATBusPostingGroup() As String
            Get
                Return m_VATBusPostingGroup
            End Get
            Set(ByVal Value As String)
                m_VATBusPostingGroup = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
            End Set
        End Property

        Public Property AllowLineDiscount() As Boolean
            Get
                Return m_AllowLineDiscount
            End Get
            Set(ByVal Value As Boolean)
                m_AllowLineDiscount = Value
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

        Public Property DistributionGroupCode() As String
            Get
                Return m_DistributionGroupCode
            End Get
            Set(ByVal Value As String)
                m_DistributionGroupCode = Value
            End Set
        End Property

        Public Property DistributionSubgroupCode() As String
            Get
                Return m_DistributionSubgroupCode
            End Get
            Set(ByVal Value As String)
                m_DistributionSubgroupCode = Value
            End Set
        End Property

        Public Property RetailPriceGroup() As Boolean
            Get
                Return m_RetailPriceGroup
            End Get
            Set(ByVal Value As Boolean)
                m_RetailPriceGroup = Value
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

        Public Sub New(ByVal DB As Database, ByVal CustomerPriceGroupId As Integer)
            m_DB = DB
            m_CustomerPriceGroupId = CustomerPriceGroupId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CustomerPriceGroupCode As String)
            m_DB = DB
            m_CustomerPriceGroupCode = CustomerPriceGroupCode
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM CustomerPriceGroup WHERE " & IIf(CustomerPriceGroupId <> Nothing, "CustomerPriceGroupId = " & DB.Number(CustomerPriceGroupId), "CustomerPriceGroupCode = " & DB.Quote(CustomerPriceGroupCode))
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
            m_CustomerPriceGroupId = Convert.ToInt32(r.Item("CustomerPriceGroupId"))
            If IsDBNull(r.Item("CustomerPriceGroupCode")) Then
                m_CustomerPriceGroupCode = Nothing
            Else
                m_CustomerPriceGroupCode = Convert.ToString(r.Item("CustomerPriceGroupCode"))
            End If
            m_PriceIncludesVAT = Convert.ToBoolean(r.Item("PriceIncludesVAT"))
            m_AllowInvoiceDisc = Convert.ToBoolean(r.Item("AllowInvoiceDisc"))
            If IsDBNull(r.Item("VATBusPostingGroup")) Then
                m_VATBusPostingGroup = Nothing
            Else
                m_VATBusPostingGroup = Convert.ToString(r.Item("VATBusPostingGroup"))
            End If
            If IsDBNull(r.Item("Description")) Then
                m_Description = Nothing
            Else
                m_Description = Convert.ToString(r.Item("Description"))
            End If
            m_AllowLineDiscount = Convert.ToBoolean(r.Item("AllowLineDiscount"))
            If IsDBNull(r.Item("CurrencyCode")) Then
                m_CurrencyCode = Nothing
            Else
                m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
            End If
            If IsDBNull(r.Item("DistributionGroupCode")) Then
                m_DistributionGroupCode = Nothing
            Else
                m_DistributionGroupCode = Convert.ToString(r.Item("DistributionGroupCode"))
            End If
            If IsDBNull(r.Item("DistributionSubgroupCode")) Then
                m_DistributionSubgroupCode = Nothing
            Else
                m_DistributionSubgroupCode = Convert.ToString(r.Item("DistributionSubgroupCode"))
            End If
            m_RetailPriceGroup = Convert.ToBoolean(r.Item("RetailPriceGroup"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO CustomerPriceGroup (" _
             & " CustomerPriceGroupCode" _
             & ",PriceIncludesVAT" _
             & ",AllowInvoiceDisc" _
             & ",VATBusPostingGroup" _
             & ",Description" _
             & ",AllowLineDiscount" _
             & ",CurrencyCode" _
             & ",DistributionGroupCode" _
             & ",DistributionSubgroupCode" _
             & ",RetailPriceGroup" _
             & ") VALUES (" _
             & m_DB.Quote(CustomerPriceGroupCode) _
             & "," & CInt(PriceIncludesVAT) _
             & "," & CInt(AllowLineDiscount) _
             & "," & m_DB.Quote(VATBusPostingGroup) _
             & "," & m_DB.Quote(Description) _
             & "," & CInt(AllowLineDiscount) _
             & "," & m_DB.Quote(CurrencyCode) _
             & "," & m_DB.Quote(DistributionGroupCode) _
             & "," & m_DB.Quote(DistributionSubgroupCode) _
             & "," & CInt(RetailPriceGroup) _
             & ")"

            CustomerPriceGroupId = m_DB.InsertSQL(SQL)

            Return CustomerPriceGroupId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE CustomerPriceGroup SET " _
             & " CustomerPriceGroupCode = " & m_DB.Quote(CustomerPriceGroupCode) _
             & ",PriceIncludesVAT = " & CInt(PriceIncludesVAT) _
             & ",AllowInvoiceDisc = " & CInt(AllowInvoiceDisc) _
             & ",VATBusPostingGroup = " & m_DB.Quote(VATBusPostingGroup) _
             & ",Description = " & m_DB.Quote(Description) _
             & ",AllowLineDiscount = " & CInt(AllowLineDiscount) _
             & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
             & ",DistributionGroupCode = " & m_DB.Quote(DistributionGroupCode) _
             & ",DistributionSubgroupCode = " & m_DB.Quote(DistributionSubgroupCode) _
             & ",RetailPriceGroup = " & CInt(RetailPriceGroup) _
             & " WHERE CustomerPriceGroupId = " & m_DB.Quote(CustomerPriceGroupId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM CustomerPriceGroup WHERE CustomerPriceGroupId = " & m_DB.Quote(CustomerPriceGroupId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class CustomerPriceGroupCollection
        Inherits GenericCollection(Of CustomerPriceGroupRow)
    End Class

End Namespace


