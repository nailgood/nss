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

    Public Class SalesCategoryRow
        Inherits SalesCategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SalesCategoryId As Integer)
            MyBase.New(DB, SalesCategoryId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SalesCategoryId As Integer) As SalesCategoryRow
            Dim row As SalesCategoryRow

            row = New SalesCategoryRow(DB, SalesCategoryId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SalesCategoryId As Integer)
            Dim row As SalesCategoryRow

            row = New SalesCategoryRow(DB, SalesCategoryId)
            row.Remove()
        End Sub

        Public Shared Function GetByURLCode(ByVal Code As String) As SalesCategoryRow
            Dim result As New SalesCategoryRow
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select * from SalesCategory where URLCode='" & Code & "'"
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read Then
                    result = LoadByDataReader(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
        Protected Shared Function LoadByDataReader(ByVal r As SqlDataReader) As SalesCategoryRow
            Dim resutl As New SalesCategoryRow
            resutl.SalesCategoryId = Convert.ToInt32(r.Item("SalesCategoryId"))
            resutl.Category = Convert.ToString(r.Item("Category"))
            resutl.NameReWriteUrl = Convert.ToString(r.Item("NameReWriteUrl"))
            resutl.IsActive = Convert.ToBoolean(r.Item("IsActive"))
            Return resutl
        End Function 'Load
        'end 23/10/2009
        'Custom Methods
        Public Sub InsertSalesCategoryItem(ByVal ItemId As Integer)
            Dim i As New SalesCategoryItemRow(DB)
            i.ItemId = ItemId
            i.SalesCategoryId = SalesCategoryId
            i.Insert()
        End Sub

        Public Shared Function GetCategoriesForMenu() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_SalesCategory_GetCategoryForMenu"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            Return db.ExecuteDataSet(cmd)
        End Function

    End Class

    Public MustInherit Class SalesCategoryRowBase
        Private m_DB As Database
        Private m_SalesCategoryId As Integer = Nothing
        Private m_Category As String = Nothing
        Private m_NameReWriteUrl As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_URLCode As String = Nothing
        Public Property SalesCategoryId() As Integer
            Get
                Return m_SalesCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_SalesCategoryId = Value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return m_Category
            End Get
            Set(ByVal Value As String)
                m_Category = Value
            End Set
        End Property
        Public Property URLCode() As String
            Get
                Return m_URLCode
            End Get
            Set(ByVal Value As String)
                m_URLCode = Value
            End Set
        End Property
        Public Property NameReWriteUrl() As String
            Get
                Return m_NameReWriteUrl
            End Get
            Set(ByVal Value As String)
                m_NameReWriteUrl = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = Value
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

        Public Sub New(ByVal DB As Database, ByVal SalesCategoryId As Integer)
            m_DB = DB
            m_SalesCategoryId = SalesCategoryId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: August 31, 2009
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_SalesCategory_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "SalesCategoryId", DbType.Int32, SalesCategoryId)
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


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_SalesCategoryId = Convert.ToInt32(r.Item("SalesCategoryId"))
            m_Category = Convert.ToString(r.Item("Category"))
            m_NameReWriteUrl = Convert.ToString(r.Item("NameReWriteUrl"))
            m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
            m_URLCode = r.Item("URLCode")
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: August 31, 2009
            '------------------------------------------------------------------------
            Dim maxSortOrder As Integer = GetMaxSortOrder() + 1

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_SalesCategory_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddOutParameter(cmd, "SalesCategoryId", DbType.Int32, 16)
            db.AddInParameter(cmd, "Category", DbType.String, Category)
            db.AddInParameter(cmd, "UrlCode", DbType.String, URLCode)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, maxSortOrder)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------

            Return SalesCategoryId
        End Function

        Private Function GetMaxSortOrder() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_SalesCategory_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: August 31, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_SalesCategory_UpdateV1"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddInParameter(cmd, "SalesCategoryId", DbType.Int32, SalesCategoryId)
            db.AddInParameter(cmd, "Category", DbType.String, Category)
            db.AddInParameter(cmd, "NameRewriteUrl", DbType.String, NameReWriteUrl)
            db.AddInParameter(cmd, "UrlCode", DbType.String, URLCode)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: August 31, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_SalesCategory_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "SalesCategoryId", DbType.Int32, SalesCategoryId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class SalesCategoryCollection
        Inherits GenericCollection(Of SalesCategoryRow)
    End Class

End Namespace


