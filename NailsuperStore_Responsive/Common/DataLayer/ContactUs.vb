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

    Public Class ContactUsRow
        Inherits ContactUsRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ContactId As Integer)
            MyBase.New(DB, ContactId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ContactId As Integer) As ContactUsRow
            Dim row As ContactUsRow

            row = New ContactUsRow(DB, ContactId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ContactId As Integer)
            Dim row As ContactUsRow
            row = New ContactUsRow(DB, ContactId)
            row.Remove()
        End Sub
       
        Public Shared Function GetList(ByVal DB1 As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
            '---------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_ContactUs_GetList"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            db.AddInParameter(cmd, "SortBy", DbType.String, SortBy)
            db.AddInParameter(cmd, "SortOrder", DbType.String, SortOrder)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '--------------------------------------------------------------------
        End Function
       
        'Custom Methods
        Public Shared Function GetAllTypeContact(ByVal DB1 As Database) As DataTable
            '---------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_ContactUs_GetAllTypeContact"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '--------------------------------------------------------------------
        End Function

    End Class

    Public MustInherit Class ContactUsRowBase
        Private m_DB As Database
        Private m_ContactId As Integer = Nothing
        Private m_SubjectId As Integer = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_EmailAddress As String = Nothing
        Private m_Phone As String = Nothing
        Private m_OrderNumber As String = Nothing
        Private m_Comments As String = Nothing
        'Return Status
        Private m_SalonName As String = Nothing
        Private m_ShippingAddress As String = Nothing
        Private m_DaytimePhone As String = Nothing
        Private m_InvoiceNumber As String = Nothing
        Private m_City As String = Nothing
        Private m_State As String = Nothing
        Private m_ZipCode As String = Nothing
        Private m_TypeContact As String = Nothing
        'item not received
        Private m_TypeShipping As String = Nothing
        Private m_ItemNotReceived As String = Nothing
        'Detail warranty infomation
        Private m_ItemNumber As String = Nothing
        Private m_ProductDescription As String = Nothing
        'Damaged Item
        Private m_DamagedorDefective As String = Nothing
        Private m_ItemDamaged As String = Nothing
        Private m_PieceOfMerchandise As String = Nothing
        Private m_DamagedCarton As String = Nothing
        Private m_DscPackaging As String = Nothing
        Private m_DscMaterial As String = Nothing
        Private m_AdjustmentType As Integer = Nothing
        'End Damaged
        Public Property ContactId() As Integer
            Get
                Return m_ContactId
            End Get
            Set(ByVal Value As Integer)
                m_ContactId = value
            End Set
        End Property

        Public Property SubjectId() As Integer
            Get
                Return m_SubjectId
            End Get
            Set(ByVal Value As Integer)
                m_SubjectId = value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal Value As String)
                m_FirstName = value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal Value As String)
                m_LastName = value
            End Set
        End Property

        Public Property EmailAddress() As String
            Get
                Return m_EmailAddress
            End Get
            Set(ByVal Value As String)
                m_EmailAddress = value
            End Set
        End Property

        Public Property Phone() As String
            Get
                Return m_Phone
            End Get
            Set(ByVal Value As String)
                m_Phone = value
            End Set
        End Property

        Public Property OrderNumber() As String
            Get
                Return m_OrderNumber
            End Get
            Set(ByVal Value As String)
                m_OrderNumber = value
            End Set
        End Property

        Public Property Comments() As String
            Get
                Return m_Comments
            End Get
            Set(ByVal Value As String)
                m_Comments = value
            End Set
        End Property
        'return status
        Public Property SalonName() As String
            Get
                Return m_SalonName
            End Get
            Set(ByVal Value As String)
                m_SalonName = Value
            End Set
        End Property
        Public Property ShippingAddress() As String
            Get
                Return m_ShippingAddress
            End Get
            Set(ByVal Value As String)
                m_ShippingAddress = Value
            End Set
        End Property
        Public Property DaytimePhone() As String
            Get
                Return m_DaytimePhone
            End Get
            Set(ByVal Value As String)
                m_DaytimePhone = Value
            End Set
        End Property
        Public Property InvoiceNumber() As String
            Get
                Return m_InvoiceNumber
            End Get
            Set(ByVal Value As String)
                m_InvoiceNumber = Value
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
        Public Property TypeContact() As String
            Get
                Return m_TypeContact
            End Get
            Set(ByVal Value As String)
                m_TypeContact = Value
            End Set
        End Property
        'End Return
        'item not received
        Public Property TypeShipping() As String
            Get
                Return m_TypeShipping
            End Get
            Set(ByVal Value As String)
                m_TypeShipping = Value
            End Set
        End Property
        Public Property ItemNotReceived() As String
            Get
                Return m_ItemNotReceived
            End Get
            Set(ByVal Value As String)
                m_ItemNotReceived = Value
            End Set
        End Property
        'end item not received
        'Datail warranty information
        Public Property ItemNumber() As String
            Get
                Return m_ItemNumber
            End Get
            Set(ByVal Value As String)
                m_ItemNumber = Value
            End Set
        End Property
        Public Property ProductDescription() As String
            Get
                Return m_ProductDescription
            End Get
            Set(ByVal Value As String)
                m_ProductDescription = Value
            End Set
        End Property
        'End Datail warranty information
        'Damaged item
        Public Property DamagedOrDefective() As String
            Get
                Return m_DamagedorDefective
            End Get
            Set(ByVal Value As String)
                m_DamagedorDefective = Value
            End Set
        End Property
        Public Property ItemDamaged() As String
            Get
                Return m_ItemDamaged
            End Get
            Set(ByVal Value As String)
                m_ItemDamaged = Value
            End Set
        End Property
        Public Property PieceOfMerchandise() As String
            Get
                Return m_PieceOfMerchandise
            End Get
            Set(ByVal Value As String)
                m_PieceOfMerchandise = Value
            End Set
        End Property
        Public Property DamagedCarton() As String
            Get
                Return m_DamagedCarton
            End Get
            Set(ByVal Value As String)
                m_DamagedCarton = Value
            End Set
        End Property
        Public Property DscPackaging() As String
            Get
                Return m_DscPackaging
            End Get
            Set(ByVal Value As String)
                m_DscPackaging = Value
            End Set
        End Property
        Public Property DscMaterial() As String
            Get
                Return m_DscMaterial
            End Get
            Set(ByVal Value As String)
                m_DscMaterial = Value
            End Set
        End Property
        'End damaged item
        Public Property AdjustmentType() As Integer
            Get
                Return m_AdjustmentType
            End Get
            Set(ByVal Value As Integer)
                m_AdjustmentType = Value
            End Set
        End Property
        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As DataBase)
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

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_ContactUs_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "ContactId", DbType.Int32, ContactId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:06 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("ContactId"))) Then
                    m_ContactId = Convert.ToInt32(reader("ContactId"))
                Else
                    m_ContactId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SubjectId"))) Then
                    m_SubjectId = Convert.ToInt32(reader("SubjectId"))
                Else
                    m_SubjectId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("FirstName"))) Then
                    m_FirstName = reader("FirstName").ToString()
                Else
                    m_FirstName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LastName"))) Then
                    m_LastName = reader("LastName").ToString()
                Else
                    m_LastName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EmailAddress"))) Then
                    m_EmailAddress = reader("EmailAddress").ToString()
                Else
                    m_EmailAddress = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Phone"))) Then
                    m_Phone = reader("Phone").ToString()
                Else
                    m_Phone = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OrderNumber"))) Then
                    m_OrderNumber = reader("OrderNumber").ToString()
                Else
                    m_OrderNumber = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Comments"))) Then
                    m_Comments = reader("Comments").ToString()
                Else
                    m_Comments = ""
                End If
                'If (Not reader.IsDBNull(reader.GetOrdinal("CreateDate"))) Then
                '    m_CreateDate = Convert.ToDateTime(reader("CreateDate"))
                'Else
                '    m_CreateDate = DateTime.Now
                'End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SalonName"))) Then
                    m_SalonName = reader("SalonName").ToString()
                Else
                    m_SalonName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ShippingAddress"))) Then
                    m_ShippingAddress = reader("ShippingAddress").ToString()
                Else
                    m_ShippingAddress = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DaytimePhone"))) Then
                    m_DaytimePhone = reader("DaytimePhone").ToString()
                Else
                    m_DaytimePhone = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("InvoiceNumber"))) Then
                    m_InvoiceNumber = reader("InvoiceNumber").ToString()
                Else
                    m_InvoiceNumber = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("City"))) Then
                    m_City = reader("City").ToString()
                Else
                    m_City = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("State"))) Then
                    m_State = reader("State").ToString()
                Else
                    m_State = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ZipCode"))) Then
                    m_ZipCode = reader("ZipCode").ToString()
                Else
                    m_ZipCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TypeShipping"))) Then
                    m_TypeShipping = reader("TypeShipping").ToString()
                Else
                    m_TypeShipping = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemNotReceived"))) Then
                    m_ItemNotReceived = reader("ItemNotReceived").ToString()
                Else
                    m_ItemNotReceived = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemNumber"))) Then
                    m_ItemNumber = reader("ItemNumber").ToString()
                Else
                    m_ItemNumber = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ProductDescription"))) Then
                    m_ProductDescription = reader("ProductDescription").ToString()
                Else
                    m_ProductDescription = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DamagedorDefective"))) Then
                    m_DamagedorDefective = reader("DamagedorDefective").ToString()
                Else
                    m_DamagedorDefective = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemDamaged"))) Then
                    m_ItemDamaged = reader("ItemDamaged").ToString()
                Else
                    m_ItemDamaged = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("PieceOfMerchandise"))) Then
                    m_PieceOfMerchandise = reader("PieceOfMerchandise").ToString()
                Else
                    m_PieceOfMerchandise = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DamagedCarton"))) Then
                    m_DamagedCarton = reader("DamagedCarton").ToString()
                Else
                    m_DamagedCarton = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DscPackaging"))) Then
                    m_DscPackaging = reader("DscPackaging").ToString()
                Else
                    m_DscPackaging = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DscMaterial"))) Then
                    m_DscMaterial = reader("DscMaterial").ToString()
                Else
                    m_DscMaterial = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TypeContact"))) Then
                    m_TypeContact = reader("TypeContact").ToString()
                Else
                    m_TypeContact = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("AdjustmentType"))) Then
                    m_AdjustmentType = Convert.ToInt32(reader("AdjustmentType"))
                Else
                    m_AdjustmentType = 0
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_INSERT As String = "sp_ContactUs_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_INSERT)
            db.AddOutParameter(cmd, "ContactId", DbType.Int32, 16)
            db.AddInParameter(cmd, "SubjectId", DbType.Int32, SubjectId)
            db.AddInParameter(cmd, "FirstName", DbType.String, FirstName)
            db.AddInParameter(cmd, "LastName", DbType.String, LastName)
            db.AddInParameter(cmd, "EmailAddress", DbType.String, EmailAddress)
            db.AddInParameter(cmd, "Phone", DbType.String, Phone)
            db.AddInParameter(cmd, "OrderNumber", DbType.String, OrderNumber)
            db.AddInParameter(cmd, "Comments", DbType.String, Comments)
            db.AddInParameter(cmd, "SalonName", DbType.String, SalonName)
            db.AddInParameter(cmd, "ShippingAddress", DbType.String, ShippingAddress)
            db.AddInParameter(cmd, "DaytimePhone", DbType.String, DaytimePhone)
            db.AddInParameter(cmd, "InvoiceNumber", DbType.String, InvoiceNumber)
            db.AddInParameter(cmd, "City", DbType.String, City)
            db.AddInParameter(cmd, "State", DbType.String, State)
            db.AddInParameter(cmd, "Zipcode", DbType.String, ZipCode)
            db.AddInParameter(cmd, "TypeShipping", DbType.String, TypeShipping)
            db.AddInParameter(cmd, "ItemNotReceived", DbType.String, ItemNotReceived)
            db.AddInParameter(cmd, "ItemNumber", DbType.String, ItemNumber)
            db.AddInParameter(cmd, "ProductDescription", DbType.String, ProductDescription)
            db.AddInParameter(cmd, "DamagedorDefective", DbType.String, DamagedOrDefective)
            db.AddInParameter(cmd, "ItemDamaged", DbType.String, ItemDamaged)
            db.AddInParameter(cmd, "PieceOfMerchandise", DbType.String, PieceOfMerchandise)
            db.AddInParameter(cmd, "DamagedCarton", DbType.String, DamagedCarton)
            db.AddInParameter(cmd, "DscPackaging", DbType.String, DscPackaging)
            db.AddInParameter(cmd, "DscMaterial", DbType.String, DscMaterial)
            db.AddInParameter(cmd, "TypeContact", DbType.String, TypeContact)
            db.AddInParameter(cmd, "AdjustmentType", DbType.Int32, AdjustmentType)
            db.ExecuteNonQuery(cmd)
            ContactId = CType(db.GetParameterValue(cmd, "ContactId"), Integer)
            '----------------------------------------------------------------------
            Return ContactId
        End Function

        Public Overridable Sub Update()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_UPDATE As String = "sp_ContactUs_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)
            db.AddInParameter(cmd, "ContactId", DbType.Int32, ContactId)
            db.AddInParameter(cmd, "SubjectId", DbType.Int32, SubjectId)
            db.AddInParameter(cmd, "FirstName", DbType.String, FirstName)
            db.AddInParameter(cmd, "LastName", DbType.String, LastName)
            db.AddInParameter(cmd, "EmailAddress", DbType.String, EmailAddress)
            db.AddInParameter(cmd, "Phone", DbType.String, Phone)
            db.AddInParameter(cmd, "OrderNumber", DbType.String, OrderNumber)
            db.AddInParameter(cmd, "Comments", DbType.String, Comments)
            db.AddInParameter(cmd, "ProductDescription", DbType.String, ProductDescription)
            db.AddInParameter(cmd, "AdjustmentType", DbType.Int32, AdjustmentType)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_ContactUs_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "ContactId", DbType.Int32, ContactId)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ContactUsCollection
        Inherits GenericCollection(Of ContactUsRow)
    End Class

End Namespace


