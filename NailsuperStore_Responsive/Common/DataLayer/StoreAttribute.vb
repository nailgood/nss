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

Namespace DataLayer

    Public Class StoreAttributeRow
        Inherits StoreAttributeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As StoreAttributeRow
            Dim row As StoreAttributeRow

            row = New StoreAttributeRow(DB, Id)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
            Dim row As StoreAttributeRow

            row = New StoreAttributeRow(DB, Id)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetRowsByItem(ByVal _DB As Database, ByVal ItemId As Integer) As StoreAttributeCollection
            Dim c As StoreAttributeCollection = New StoreAttributeCollection
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREATTRIBUTE_GETLIST As String = "sp_StoreAttribute_GetRowsByItem"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREATTRIBUTE_GETLIST)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                '------------------------------------------------------------------------
                While reader.Read()
                    Dim att As StoreAttributeRow = New StoreAttributeRow(_DB)
                    att.Load(reader)
                    c.Add(att)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return c
        End Function

        Public Shared Function GetRowsByItemSku(ByVal _DB As Database, ByVal SKU As String) As StoreAttributeCollection
            Dim c As New StoreAttributeCollection
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreAttribute_GetRowsByItemSku")
                db.AddInParameter(cmd, "SKU", DbType.String, SKU)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                While reader.Read
                    Dim att As New StoreAttributeRow(_DB)
                    att.Load(reader)
                    c.Add(att)
                End While
                Core.CloseReader(reader)

            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.ToString())
            End Try
            Return c
        End Function

        Public Shared Sub RemoveRowsByItem(ByVal _DB As Database, ByVal ItemId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STOREATTRIBUTE_DELETE As String = "sp_StoreAttribute_RemoveRowsByItem"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREATTRIBUTE_DELETE)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.ExecuteNonQuery(cmd)
        End Sub

    End Class

    Public MustInherit Class StoreAttributeRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_Value As String = Nothing
        Private m_SKU As String = Nothing
        Private m_Price As String = Nothing

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Set(ByVal Value As String)
                m_Value = Value
            End Set
        End Property

        Public Property SKU() As String
            Get
                Return m_SKU
            End Get
            Set(ByVal Value As String)
                m_SKU = Value
            End Set
        End Property

        Public Property Price() As String
            Get
                Return m_Price
            End Get
            Set(ByVal Value As String)
                m_Price = Value
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

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            m_DB = DB
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREATTRIBUTE_GETOBJECT As String = "sp_StoreAttribute_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREATTRIBUTE_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            '------------------------------------------------------------------------
        End Sub


        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                        m_Name = reader("Name").ToString()
                    Else
                        m_Name = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Value"))) Then
                        m_Value = reader("Value").ToString()
                    Else
                        m_Value = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SKU"))) Then
                        m_SKU = reader("SKU").ToString()
                    Else
                        m_SKU = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Price"))) Then
                        m_Price = reader("Price").ToString()
                    Else
                        m_Price = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex


            End Try
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREATTRIBUTE_INSERT As String = "sp_StoreAttribute_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREATTRIBUTE_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Value", DbType.String, Value)
            db.AddInParameter(cmd, "SKU", DbType.String, SKU)
            db.AddInParameter(cmd, "Price", DbType.String, Price)

            db.ExecuteNonQuery(cmd)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))

            '------------------------------------------------------------------------
            Return Id
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREATTRIBUTE_UPDATE As String = "sp_StoreAttribute_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREATTRIBUTE_UPDATE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Value", DbType.String, Value)
            db.AddInParameter(cmd, "SKU", DbType.String, SKU)
            db.AddInParameter(cmd, "Price", DbType.String, Price)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREATTRIBUTE_DELETE As String = "sp_StoreAttribute_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREATTRIBUTE_DELETE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StoreAttributeCollection
        Inherits GenericCollection(Of StoreAttributeRow)
    End Class

End Namespace


