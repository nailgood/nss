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

    Public Class MemberWishlistItemRow
        Inherits MemberWishlistItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal WishlistItemId As Integer)
            MyBase.New(DB, WishlistItemId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String)
            MyBase.New(DB, MemberId, ItemId, Attributes, Swatches)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal WishlistItemId As Integer) As MemberWishlistItemRow
            Dim row As MemberWishlistItemRow

            row = New MemberWishlistItemRow(DB, WishlistItemId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal MemberId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String) As MemberWishlistItemRow
            Dim row As MemberWishlistItemRow

            row = New MemberWishlistItemRow(DB, MemberId, ItemId, Attributes, Swatches)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal WishlistItemId As Integer)
            Dim row As MemberWishlistItemRow

            row = New MemberWishlistItemRow(DB, WishlistItemId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class MemberWishlistItemRowBase
        Private m_DB As Database
        Private m_WishlistItemId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_Quantity As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_AttributeSKU As String = Nothing
        Private m_Attributes As String = Nothing
        Private m_Swatches As String = Nothing
        Private m_EmailSent As Boolean = Nothing

        Public Property EmailSent() As Boolean
            Get
                Return m_EmailSent
            End Get
            Set(ByVal value As Boolean)
                m_EmailSent = value
            End Set
        End Property

        Public Property AttributeSKU() As String
            Get
                Return m_AttributeSKU
            End Get
            Set(ByVal Value As String)
                m_AttributeSKU = Value
            End Set
        End Property

        Public Property Attributes() As String
            Get
                Return m_Attributes
            End Get
            Set(ByVal Value As String)
                m_Attributes = Value
            End Set
        End Property

        Public Property Swatches() As String
            Get
                Return m_Swatches
            End Get
            Set(ByVal Value As String)
                m_Swatches = Value
            End Set
        End Property

        Public Property WishlistItemId() As Integer
            Get
                Return m_WishlistItemId
            End Get
            Set(ByVal Value As Integer)
                m_WishlistItemId = Value
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

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return m_Quantity
            End Get
            Set(ByVal Value As Integer)
                m_Quantity = Value
            End Set
        End Property

        Public ReadOnly Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
        End Property

        Public ReadOnly Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
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

        Public Sub New(ByVal DB As Database, ByVal WishlistItemId As Integer)
            m_DB = DB
            m_WishlistItemId = WishlistItemId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberId As Integer, ByVal ItemId As Integer, ByVal Attributes As String, ByVal Swatches As String)
            m_DB = DB
            m_MemberId = MemberId
            m_ItemId = ItemId
            m_Attributes = Attributes
            m_Swatches = Swatches
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:54 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_MEMBERWISHLISTITEM_OBJECT As String = "sp_MemberWishlistItem_GetObjectByOptional"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERWISHLISTITEM_OBJECT)
                db.AddInParameter(cmd, "WishlistItemId", DbType.Int32, WishlistItemId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "Attributes", DbType.String, Attributes)
                db.AddInParameter(cmd, "Swatches", DbType.String, Swatches)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try


            '------------------------------------------------------------------------
        End Sub


        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:16:03 PM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("WishlistItemId"))) Then
                    WishlistItemId = Convert.ToInt32(reader("WishlistItemId"))
                Else
                    WishlistItemId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                    m_MemberId = Convert.ToInt32(reader("MemberId"))
                Else
                    m_MemberId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                    m_ItemId = Convert.ToInt32(reader("ItemId"))
                Else
                    m_ItemId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("AttributeSKU"))) Then
                    m_AttributeSKU = reader("AttributeSKU").ToString()
                Else
                    m_AttributeSKU = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Attributes"))) Then
                    m_Attributes = reader("Attributes").ToString()
                Else
                    m_Attributes = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Swatches"))) Then
                    m_Swatches = reader("Swatches").ToString()
                Else
                    m_Swatches = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Quantity"))) Then
                    m_Quantity = Convert.ToInt32(reader("Quantity"))
                Else
                    m_Quantity = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreateDate"))) Then
                    m_CreateDate = Convert.ToDateTime(reader("CreateDate"))
                Else
                    m_CreateDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ModifyDate"))) Then
                    m_ModifyDate = Convert.ToDateTime(reader("ModifyDate"))
                Else
                    m_ModifyDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EmailSent"))) Then
                    m_EmailSent = Convert.ToBoolean(reader("EmailSent"))
                Else
                    m_EmailSent = False
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERWISHLISTITEM_INSERT As String = "sp_MemberWishlistItem_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERWISHLISTITEM_INSERT)

            db.AddOutParameter(cmd, "WishlistItemId", DbType.Int32, 32)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "AttributeSKU", DbType.String, AttributeSKU)
            db.AddInParameter(cmd, "Attributes", DbType.String, Attributes)
            db.AddInParameter(cmd, "Swatches", DbType.String, Swatches)
            db.AddInParameter(cmd, "Quantity", DbType.Int32, Quantity)
            db.AddInParameter(cmd, "CreateDate", DbType.DateTime, DateTime.Now)
            db.AddInParameter(cmd, "ModifyDate", DbType.DateTime, DateTime.Now)
            db.AddInParameter(cmd, "EmailSent", DbType.Boolean, EmailSent)

            db.ExecuteNonQuery(cmd)

            WishlistItemId = Convert.ToInt32(db.GetParameterValue(cmd, "WishlistItemId"))

            '------------------------------------------------------------------------
            Return WishlistItemId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERWISHLISTITEM_UPDATE As String = "sp_MemberWishlistItem_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERWISHLISTITEM_UPDATE)

            db.AddInParameter(cmd, "WishlistItemId", DbType.Int32, WishlistItemId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "AttributeSKU", DbType.String, AttributeSKU)
            db.AddInParameter(cmd, "Attributes", DbType.String, Attributes)
            db.AddInParameter(cmd, "Swatches", DbType.String, Swatches)
            db.AddInParameter(cmd, "Quantity", DbType.Int32, Quantity)
            db.AddInParameter(cmd, "CreateDate", DbType.DateTime, CreateDate)
            db.AddInParameter(cmd, "ModifyDate", DbType.DateTime, ModifyDate)
            db.AddInParameter(cmd, "EmailSent", DbType.Boolean, EmailSent)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERWISHLISTITEM_DELETE As String = "sp_MemberWishlistItem_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERWISHLISTITEM_DELETE)

            db.AddInParameter(cmd, "WishlistItemId", DbType.Int32, WishlistItemId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class MemberWishlistItemCollection
        Inherits GenericCollection(Of MemberWishlistItemRow)
    End Class

End Namespace

