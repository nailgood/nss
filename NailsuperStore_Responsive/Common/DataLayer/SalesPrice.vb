Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:12:55 PM

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
Imports Utility

Namespace DataLayer

    Public Class SalesPriceRow
        Inherits SalesPriceRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SalesPriceId As Integer)
            MyBase.New(DB, SalesPriceId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal ItemId As Integer, ByVal SalesType As Integer, ByVal MinimumQuantity As Integer)
            MyBase.New(DB, MemberId, CustomerPriceGroupId, ItemId, SalesType, MinimumQuantity)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal ItemId As Integer, ByVal SalesType As Integer, ByVal MinimumQuantity As Integer, ByVal startingDate As DateTime)
            MyBase.New(DB, MemberId, CustomerPriceGroupId, ItemId, SalesType, MinimumQuantity, startingDate)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SalesPriceId As Integer) As SalesPriceRow
            Dim row As SalesPriceRow

            row = New SalesPriceRow(DB, SalesPriceId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal MemberId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal ItemId As Integer, ByVal SalesType As Integer, ByVal MinimumQuantity As Integer) As SalesPriceRow
            Dim row As SalesPriceRow

            row = New SalesPriceRow(DB, MemberId, CustomerPriceGroupId, ItemId, SalesType, MinimumQuantity)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal MemberId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal ItemId As Integer, ByVal SalesType As Integer, ByVal MinimumQuantity As Integer, ByVal startingDate As DateTime) As SalesPriceRow
            Dim row As SalesPriceRow

            row = New SalesPriceRow(DB, MemberId, CustomerPriceGroupId, ItemId, SalesType, MinimumQuantity, startingDate)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SalesPriceId As Integer)
            Dim row As SalesPriceRow

            row = New SalesPriceRow(DB, SalesPriceId)
            row.Remove()
        End Sub

        Public Shared Sub RemoveRow(ByVal SKU As String)
            Dim row As SalesPriceRow

            row = New SalesPriceRow()
            row.Remove(SKU)
        End Sub

        Public Shared Sub RemoveRow(ByVal itemId As Integer)
            Dim row As SalesPriceRow

            row = New SalesPriceRow()
            row.Remove(itemId)
        End Sub

        Public Shared Function GetRowById(ByVal DB As Database, ByVal SalesPriceId As Integer) As SalesPriceRow
            Dim row As SalesPriceRow

            row = New SalesPriceRow(DB, SalesPriceId)
            row.LoadById()

            Return row
        End Function

        'Custom Methods
        Public Shared Sub CopyFromNavision(ByVal DB As Database, ByVal n As NavisionSalesPriceRow)
            Dim ItemId, MemberId, SalesType, CustomerPriceGroupId As Integer
            Dim startingDate As DateTime

            ItemId = StoreItemRow.GetRow(DB, n.ItemNo.ToString).ItemId
            SalesType = n.SalesType
            If ItemId = Nothing Then Exit Sub
            If n.SalesCode <> Nothing AndAlso SalesType = 0 Then MemberId = DB.ExecuteScalar("select top 1 memberid from member m inner join customer c on m.customerid = c.customerid where c.customerno = " & DB.Quote(n.SalesCode))
            If n.SalesCode <> Nothing AndAlso SalesType = 1 Then CustomerPriceGroupId = DB.ExecuteScalar("select top 1 CustomerPriceGroupId from customerpricegroup where c.customerpricegroupcode = " & DB.Quote(n.SalesCode))
            If MemberId = Nothing AndAlso SalesType = 0 Then Exit Sub
            If CustomerPriceGroupId = Nothing AndAlso SalesType = 1 Then Exit Sub

            'Dim p As SalesPriceRow = SalesPriceRow.GetRow(DB, MemberId, CustomerPriceGroupId, ItemId, SalesType, n.MinimumQuantity)
            If n.StartingDate = Nothing Then
                startingDate = Nothing
            Else
                If n.StartingDate.Trim() = "" Then
                    startingDate = Nothing
                Else
                    startingDate = DB.NullDate(n.StartingDate)
                End If
            End If

            'Modified by Lam Le
            Dim p As SalesPriceRow = SalesPriceRow.GetRow(DB, MemberId, CustomerPriceGroupId, ItemId, SalesType, n.MinimumQuantity, startingDate)

            p.ItemId = ItemId
            p.MemberId = MemberId
            p.SalesCode = Trim(n.SalesCode)
            p.SalesType = SalesType
            p.AllowInvoiceDisc = IIf(n.AllowInvoiceDisc = "Y", True, False)
            p.AllowLineDisc = IIf(n.AllowLineDisc = "Y", True, False)
            p.CurrencyCode = Trim(n.CurrencyCode)
            p.EndingDate = IIf(IsDate(n.EndingDate), n.EndingDate, Nothing)
            p.MinimumQuantity = n.MinimumQuantity
            p.PriceGroupDescription = Trim(n.PriceGroupDescription)
            p.CustomerPriceGroupId = CustomerPriceGroupId
            p.PriceIncludesVAT = IIf(n.PriceIncludesVAT = "Y", True, False)
            p.StartingDate = IIf(IsDate(n.StartingDate), n.StartingDate, Nothing)
            p.UnitOfMeasureCode = Trim(n.UnitOfMeasureCode)
            p.UnitPrice = n.UnitPrice
            p.UnitPriceIncludingVAT = n.UnitPriceIncludingVAT
            p.VariantCode = Trim(n.VariantCode)

            If p.SalesPriceId = Nothing Then
                p.Type = 0 'NAVISION
                p.Insert()
            Else
                p.Update()
            End If
        End Sub
        'vuphuong modify: 30/03/2010
        Public Shared Function GetListSalesPriceCollection(ByVal Condition As String) As SalesPriceCollection

            Dim c As New SalesPriceCollection

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_Admin_GetListSalesPrice"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            db.AddInParameter(cmd, "Condition", DbType.String, Condition)
            Dim r As SqlDataReader = db.ExecuteReader(cmd)
            Try
                While r.Read
                    Dim i As New SalesPriceRow()
                    i.SalesPriceId = r("SalesPriceId")
                    i.Image = IIf(IsDBNull(r("image")), Nothing, r("Image"))
                    i.ImageMap = IIf(IsDBNull(r("ImageMap")), Nothing, r("ImageMap"))
                    i.ItemId = r("ItemId")
                    i.ItemName = r("ItemName")
                    'i.ItemNameNew = IIf(IsDBNull(r("ItemNameNew")), Nothing, r("ItemNameNew"))
                    c.Add(i)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return c
        End Function
    End Class



    Public MustInherit Class SalesPriceRowBase
        Private m_DB As Database
        Private m_SalesPriceId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_SalesCode As String = Nothing
        Private m_CurrencyCode As String = Nothing
        Private m_StartingDate As DateTime = Nothing
        Private m_UnitPrice As Double = Nothing
        Private m_PriceIncludesVAT As Boolean = Nothing
        Private m_AllowInvoiceDisc As Boolean = Nothing
        Private m_SalesType As Integer = Nothing
        Private m_Type As Integer = 1
        Private m_MinimumQuantity As Integer = Nothing
        Private m_EndingDate As DateTime = Nothing
        Private m_UnitOfMeasureCode As String = Nothing
        Private m_VariantCode As String = Nothing
        Private m_AllowLineDisc As Boolean = Nothing
        Private m_UnitPriceIncludingVAT As Double = Nothing
        Private m_PriceGroupDescription As String = Nothing
        Private m_CustomerPriceGroupId As Integer = Nothing
        Private m_Image As String = Nothing
        Private m_ImageMap As String = Nothing
        Private m_ItemName As String = Nothing

        Public Property SalesPriceId() As Integer
            Get
                Return m_SalesPriceId
            End Get
            Set(ByVal Value As Integer)
                m_SalesPriceId = Value
            End Set
        End Property

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
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

        Public Property CustomerPriceGroupId() As Integer
            Get
                Return m_CustomerPriceGroupId
            End Get
            Set(ByVal Value As Integer)
                m_CustomerPriceGroupId = Value
            End Set
        End Property

        Public Property SalesCode() As String
            Get
                Return m_SalesCode
            End Get
            Set(ByVal Value As String)
                m_SalesCode = Value
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

        Public Property StartingDate() As DateTime
            Get
                Return m_StartingDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartingDate = Value
            End Set
        End Property

        Public Property UnitPrice() As Double
            Get
                Return m_UnitPrice
            End Get
            Set(ByVal Value As Double)
                m_UnitPrice = Value
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

        Public Property SalesType() As Integer
            Get
                Return m_SalesType
            End Get
            Set(ByVal Value As Integer)
                m_SalesType = Value
            End Set
        End Property

        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal Value As Integer)
                m_Type = Value
            End Set
        End Property

        Public Property MinimumQuantity() As Integer
            Get
                Return m_MinimumQuantity
            End Get
            Set(ByVal Value As Integer)
                m_MinimumQuantity = Value
            End Set
        End Property

        Public Property EndingDate() As DateTime
            Get
                Return m_EndingDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndingDate = Value
            End Set
        End Property

        Public Property UnitOfMeasureCode() As String
            Get
                Return m_UnitOfMeasureCode
            End Get
            Set(ByVal Value As String)
                m_UnitOfMeasureCode = Value
            End Set
        End Property

        Public Property VariantCode() As String
            Get
                Return m_VariantCode
            End Get
            Set(ByVal Value As String)
                m_VariantCode = Value
            End Set
        End Property

        Public Property AllowLineDisc() As Boolean
            Get
                Return m_AllowLineDisc
            End Get
            Set(ByVal Value As Boolean)
                m_AllowLineDisc = Value
            End Set
        End Property

        Public Property UnitPriceIncludingVAT() As Double
            Get
                Return m_UnitPriceIncludingVAT
            End Get
            Set(ByVal Value As Double)
                m_UnitPriceIncludingVAT = Value
            End Set
        End Property

        Public Property PriceGroupDescription() As String
            Get
                Return m_PriceGroupDescription
            End Get
            Set(ByVal Value As String)
                m_PriceGroupDescription = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
            End Set
        End Property

        Public Property ImageMap() As String
            Get
                Return m_ImageMap
            End Get
            Set(ByVal Value As String)
                m_ImageMap = Value
            End Set
        End Property
        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(ByVal Value As String)
                m_ItemName = Value
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

        Public Sub New(ByVal DB As Database, ByVal SalesPriceId As Integer)
            m_DB = DB
            m_SalesPriceId = SalesPriceId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal ItemId As Integer, ByVal SalesType As Integer, ByVal MinimumQuantity As Integer)
            m_DB = DB
            m_MemberId = MemberId
            m_CustomerPriceGroupId = CustomerPriceGroupId
            m_ItemId = ItemId
            m_SalesType = SalesType
            m_MinimumQuantity = MinimumQuantity
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberId As Integer, ByVal CustomerPriceGroupId As Integer, ByVal ItemId As Integer, ByVal SalesType As Integer, ByVal MinimumQuantity As Integer, ByVal startingDate As DateTime)
            m_DB = DB
            m_MemberId = MemberId
            m_CustomerPriceGroupId = CustomerPriceGroupId
            m_ItemId = ItemId
            m_SalesType = SalesType
            m_MinimumQuantity = MinimumQuantity
            m_StartingDate = startingDate
        End Sub 'New

        Protected Overridable Sub LoadById()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM SalesPrice sp inner join StoreItem si on sp.ItemId = si.ItemId  WHERE sp.SalesPriceId = " & DB.Number(SalesPriceId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try

        End Sub

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Dim SQL As String

            Try
                SQL = "SELECT * FROM SalesPrice sp, storeitem si WHERE "
                If SalesPriceId <> Nothing Then
                    SQL &= "sp.itemid = si.itemid and SalesPriceId = " & DB.Number(SalesPriceId) & " and "
                End If

                SQL &= " MemberId " & IIf(MemberId = Nothing, " is ", " = ") & m_DB.NullNumber(MemberId)
                SQL &= " and CustomerPriceGroupId " & IIf(CustomerPriceGroupId = Nothing, " is ", " = ") & m_DB.NullNumber(CustomerPriceGroupId)
                SQL &= " and sp.ItemId " & IIf(ItemId = 0, " <> ", " = ") & ItemId
                SQL &= " and SalesType " & IIf(SalesType = 0, " <> ", " = ") & SalesType
                SQL &= " and MinimumQuantity " & IIf(MinimumQuantity = 0, " <> ", " = ") & MinimumQuantity

                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "SalePrice.vb >> Load", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_SalesPriceId = Convert.ToInt32(r.Item("SalesPriceId"))
                If IsDBNull(r.Item("ItemId")) Then
                    m_ItemId = Nothing
                Else
                    m_ItemId = Convert.ToInt32(r.Item("ItemId"))
                End If
                If IsDBNull(r.Item("MemberId")) Then
                    m_MemberId = Nothing
                Else
                    m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                End If
                If IsDBNull(r.Item("CustomerPriceGroupId")) Then
                    m_CustomerPriceGroupId = Nothing
                Else
                    m_CustomerPriceGroupId = r.Item("CustomerPriceGroupId")
                End If
                If IsDBNull(r.Item("SalesCode")) Then
                    m_SalesCode = Nothing
                Else
                    m_SalesCode = Convert.ToString(r.Item("SalesCode"))
                End If
                If IsDBNull(r.Item("CurrencyCode")) Then
                    m_CurrencyCode = Nothing
                Else
                    m_CurrencyCode = Convert.ToString(r.Item("CurrencyCode"))
                End If
                If IsDBNull(r.Item("StartingDate")) Then
                    m_StartingDate = Nothing
                Else
                    m_StartingDate = Convert.ToDateTime(r.Item("StartingDate"))
                End If
                If IsDBNull(r.Item("UnitPrice")) Then
                    m_UnitPrice = Nothing
                Else
                    m_UnitPrice = Convert.ToDouble(r.Item("UnitPrice"))
                End If
                If IsDBNull(r.Item("PriceIncludesVAT")) Then
                    m_PriceIncludesVAT = False
                Else
                    m_PriceIncludesVAT = Convert.ToBoolean(r.Item("PriceIncludesVAT"))
                End If

                If IsDBNull(r.Item("AllowInvoiceDisc")) Then
                    m_AllowInvoiceDisc = False
                Else
                    m_AllowInvoiceDisc = Convert.ToBoolean(r.Item("AllowInvoiceDisc"))
                End If

                If IsDBNull(r.Item("SalesType")) Then
                    m_SalesType = Nothing
                Else
                    m_SalesType = Convert.ToInt32(r.Item("SalesType"))
                End If
                If IsDBNull(r.Item("MinimumQuantity")) Then
                    m_MinimumQuantity = Nothing
                Else
                    m_MinimumQuantity = Convert.ToInt32(r.Item("MinimumQuantity"))
                End If
                If IsDBNull(r.Item("EndingDate")) Then
                    m_EndingDate = Nothing
                Else
                    m_EndingDate = Convert.ToDateTime(r.Item("EndingDate"))
                End If
                If IsDBNull(r.Item("UnitOfMeasureCode")) Then
                    m_UnitOfMeasureCode = Nothing
                Else
                    m_UnitOfMeasureCode = Convert.ToString(r.Item("UnitOfMeasureCode"))
                End If
                If IsDBNull(r.Item("VariantCode")) Then
                    m_VariantCode = Nothing
                Else
                    m_VariantCode = Convert.ToString(r.Item("VariantCode"))
                End If
                If IsDBNull(r.Item("AllowLineDisc")) Then
                    m_AllowLineDisc = False
                Else
                    m_AllowLineDisc = Convert.ToBoolean(r.Item("AllowLineDisc"))
                End If
                If IsDBNull(r.Item("UnitPriceIncludingVAT")) Then
                    m_UnitPriceIncludingVAT = Nothing
                Else
                    m_UnitPriceIncludingVAT = Convert.ToDouble(r.Item("UnitPriceIncludingVAT"))
                End If
                If IsDBNull(r.Item("PriceGroupDescription")) Then
                    m_PriceGroupDescription = Nothing
                Else
                    m_PriceGroupDescription = Convert.ToString(r.Item("PriceGroupDescription"))
                End If

                If IsDBNull(r.Item("Image")) Then
                    m_Image = Nothing
                Else
                    m_Image = Convert.ToString(r.Item("Image"))
                End If
                If IsDBNull(r.Item("ImageMap")) Then
                    m_ImageMap = Nothing
                Else
                    m_ImageMap = Convert.ToString(r.Item("ImageMap"))
                End If
                If IsDBNull(r.Item("ItemName")) Then
                    m_ItemName = Nothing
                Else
                    m_ItemName = Convert.ToString(r.Item("ItemName"))
                End If
                'If IsDBNull(r.Item("ItemNameNew")) Then
                '    m_ItemNameNew = Nothing
                'Else
                '    m_ItemNameNew = Convert.ToString(r.Item("ItemNameNew"))
                'End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            SQL = " INSERT INTO SalesPrice (" _
             & " ItemId" _
             & ",MemberId" _
             & ",SalesCode" _
             & ",CurrencyCode" _
             & ",StartingDate" _
             & ",UnitPrice" _
             & ",PriceIncludesVAT" _
             & ",AllowInvoiceDisc" _
             & ",CustomerPriceGroupId" _
             & ",SalesType" _
             & ",MinimumQuantity" _
             & ",EndingDate" _
             & ",UnitOfMeasureCode" _
             & ",VariantCode" _
             & ",AllowLineDisc" _
             & ",UnitPriceIncludingVAT" _
             & ",PriceGroupDescription" _
             & ",Type" _
             & ") VALUES (" _
             & m_DB.NullNumber(ItemId) _
             & "," & m_DB.NullNumber(MemberId) _
             & "," & m_DB.Quote(SalesCode) _
             & "," & m_DB.Quote(CurrencyCode) _
             & "," & m_DB.NullQuote(StartingDate) _
             & "," & m_DB.Number(UnitPrice) _
             & "," & CInt(PriceIncludesVAT) _
             & "," & CInt(AllowInvoiceDisc) _
             & "," & m_DB.NullNumber(CustomerPriceGroupId) _
             & "," & m_DB.Number(SalesType) _
             & "," & m_DB.Number(MinimumQuantity) _
             & "," & m_DB.NullQuote(EndingDate) _
             & "," & m_DB.Quote(UnitOfMeasureCode) _
             & "," & m_DB.Quote(VariantCode) _
             & "," & CInt(AllowLineDisc) _
             & "," & m_DB.Number(UnitPriceIncludingVAT) _
             & "," & m_DB.Quote(PriceGroupDescription) _
             & "," & CInt(Type) _
             & ")"

            SalesPriceId = m_DB.InsertSQL(SQL)
            '' CacheUtils.RemoveCacheWithPrefix("StoreItem_GetRow_" & ItemId & "_")
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            '         Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            '         Dim SP_SALESPRICE_INSERT As String = "sp_SalesPrice_Insert"

            '         Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SALESPRICE_INSERT)

            '         db.AddOutParameter(cmd, "SalesPriceId", DbType.Int32, 32)
            '         db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            '         db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            '         db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
            '         db.AddInParameter(cmd, "SalesCode", DbType.String, SalesCode)
            '         db.AddInParameter(cmd, "CurrencyCode", DbType.String, CurrencyCode)
            '         db.AddInParameter(cmd, "StartingDate", DbType.DateTime, StartingDate)
            '         db.AddInParameter(cmd, "UnitPrice", DbType.Double, UnitPrice)
            '         db.AddInParameter(cmd, "PriceIncludesVAT", DbType.Boolean, PriceIncludesVAT)
            '         db.AddInParameter(cmd, "AllowInvoiceDisc", DbType.Boolean, AllowInvoiceDisc)
            '         db.AddInParameter(cmd, "SalesType", DbType.Int32, SalesType)
            '         db.AddInParameter(cmd, "MinimumQuantity", DbType.Int32, MinimumQuantity)
            '         db.AddInParameter(cmd, "EndingDate", DbType.DateTime, EndingDate)
            '         db.AddInParameter(cmd, "UnitOfMeasureCode", DbType.String, UnitOfMeasureCode)
            '         db.AddInParameter(cmd, "VariantCode", DbType.String, VariantCode)
            '         db.AddInParameter(cmd, "AllowLineDisc", DbType.Boolean, AllowLineDisc)
            '         db.AddInParameter(cmd, "UnitPriceIncludingVAT", DbType.Double, UnitPriceIncludingVAT)
            '         db.AddInParameter(cmd, "PriceGroupDescription", DbType.String, PriceGroupDescription)

            '         db.ExecuteNonQuery(cmd)

            '         SalesPriceId = Convert.ToInt32(db.GetParameterValue(cmd, "SalesPriceId"))

            '         '------------------------------------------------------------------------

            Return SalesPriceId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE SalesPrice SET " _
             & " ItemId = " & m_DB.NullNumber(ItemId) _
             & ",MemberId = " & m_DB.NullNumber(MemberId) _
             & ",SalesCode = " & m_DB.Quote(SalesCode) _
             & ",CurrencyCode = " & m_DB.Quote(CurrencyCode) _
             & ",StartingDate = " & m_DB.NullQuote(StartingDate) _
             & ",UnitPrice = " & m_DB.Number(UnitPrice) _
             & ",PriceIncludesVAT = " & CInt(PriceIncludesVAT) _
             & ",AllowInvoiceDisc = " & CInt(AllowInvoiceDisc) _
             & ",CustomerPriceGroupId = " & m_DB.NullNumber(CustomerPriceGroupId) _
             & ",SalesType = " & m_DB.Number(SalesType) _
             & ",MinimumQuantity = " & m_DB.Number(MinimumQuantity) _
             & ",EndingDate = " & m_DB.NullQuote(EndingDate) _
             & ",UnitOfMeasureCode = " & m_DB.Quote(UnitOfMeasureCode) _
             & ",VariantCode = " & m_DB.Quote(VariantCode) _
             & ",AllowLineDisc = " & CInt(AllowLineDisc) _
             & ",UnitPriceIncludingVAT = " & m_DB.Number(UnitPriceIncludingVAT) _
             & ",PriceGroupDescription = " & m_DB.Quote(PriceGroupDescription) _
             & ",Image = " & m_DB.Quote(Image) _
             & " WHERE SalesPriceId = " & m_DB.Quote(SalesPriceId)

            m_DB.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)

            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            'Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            'Dim SP_SALESPRICE_UPDATE As String = "sp_SalesPrice_Update"

            'Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SALESPRICE_UPDATE)

            'db.AddInParameter(cmd, "SalesPriceId", DbType.Int32, SalesPriceId)
            'db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            'db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            'db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
            'db.AddInParameter(cmd, "SalesCode", DbType.String, SalesCode)
            'db.AddInParameter(cmd, "CurrencyCode", DbType.String, CurrencyCode)
            'db.AddInParameter(cmd, "StartingDate", DbType.DateTime, StartingDate)
            'db.AddInParameter(cmd, "UnitPrice", DbType.Double, UnitPrice)
            'db.AddInParameter(cmd, "PriceIncludesVAT", DbType.Boolean, PriceIncludesVAT)
            'db.AddInParameter(cmd, "AllowInvoiceDisc", DbType.Boolean, AllowInvoiceDisc)
            'db.AddInParameter(cmd, "SalesType", DbType.Int32, SalesType)
            'db.AddInParameter(cmd, "MinimumQuantity", DbType.Int32, MinimumQuantity)
            'db.AddInParameter(cmd, "EndingDate", DbType.DateTime, EndingDate)
            'db.AddInParameter(cmd, "UnitOfMeasureCode", DbType.String, UnitOfMeasureCode)
            'db.AddInParameter(cmd, "VariantCode", DbType.String, VariantCode)
            'db.AddInParameter(cmd, "AllowLineDisc", DbType.Boolean, AllowLineDisc)
            'db.AddInParameter(cmd, "UnitPriceIncludingVAT", DbType.Double, UnitPriceIncludingVAT)
            'db.AddInParameter(cmd, "PriceGroupDescription", DbType.String, PriceGroupDescription)

            'db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Overridable Sub UpdateImage()
            Dim SQL As String

            SQL = " UPDATE SalesPrice SET " _
             & "Image = " & m_DB.Quote(Image) _
             & ",ImageMap = " & m_DB.Quote(ImageMap) _
             & " WHERE SalesPriceId = " & m_DB.Quote(SalesPriceId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'UpdateImage


        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SALESPRICE_DELETE As String = "sp_SalesPrice_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SALESPRICE_DELETE)

            db.AddInParameter(cmd, "SalesPriceId", DbType.Int32, SalesPriceId)

            db.ExecuteNonQuery(cmd)

            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
            '------------------------------------------------------------------------
        End Sub 'Remove

        Public Sub Remove(ByVal SKU As String)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SALESPRICE_DELETE As String = "sp_SalesPrice_DeleteBySKU"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SALESPRICE_DELETE)

            db.AddInParameter(cmd, "SKU", DbType.String, SKU)

            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
        End Sub 'Remove

        Public Sub Remove(ByVal itemId As Integer)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SALESPRICE_DELETE As String = "sp_SalesPrice_DeleteByItemId"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SALESPRICE_DELETE)

            db.AddInParameter(cmd, "ItemID", DbType.Int32, itemId)

            db.ExecuteNonQuery(cmd)
               CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
        End Sub 'Remove
    End Class

    Public Class SalesPriceCollection
        Inherits GenericCollection(Of SalesPriceRow)
    End Class

End Namespace


