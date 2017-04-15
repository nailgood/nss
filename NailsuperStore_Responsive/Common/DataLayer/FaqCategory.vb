Option Explicit On

'Author: Lam Le
'Date: 9/29/2009 10:07:52 AM

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

    Public Class FaqCategoryRow
        Inherits FaqCategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal FaqCategoryId As Integer)
            MyBase.New(DB, FaqCategoryId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal FaqCategoryId As Integer) As FaqCategoryRow
            Dim row As FaqCategoryRow

            row = New FaqCategoryRow(DB, FaqCategoryId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal FaqCategoryId As Integer)
            Dim row As FaqCategoryRow

            row = New FaqCategoryRow(DB, FaqCategoryId)
            row.Remove()
        End Sub

        Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
            Dim SQL As String = "select * from FaqCategory"
            If Not SortBy = String.Empty Then
                SortBy = Core.ProtectParam(SortBy)
                SortOrder = Core.ProtectParam(SortOrder)

                SQL &= " order by " & SortBy & " " & SortOrder
            End If
            Return DB.GetDataTable(SQL)
        End Function

        'Custom Methods
        Public Shared Function GetAllFaqCategorys(ByVal DB1 As Database) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_FAQCATEGORY_GETLIST As String = "sp_FaqCategory_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQCATEGORY_GETLIST)

            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function ChangeChangeArrange(ByVal _Database As Database, ByVal FaqCategoryId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_FaqCategory_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("FaqCategoryId", SqlDbType.Int, 0, FaqCategoryId))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class FaqCategoryRowBase
        Private m_DB As Database
        Private m_FaqCategoryId As Integer = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_CategoryName As String = Nothing
        Private m_AdminId As Integer = Nothing


        Public Property FaqCategoryId() As Integer
            Get
                Return m_FaqCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_FaqCategoryId = Value
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

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property

        Public Property CategoryName() As String
            Get
                Return m_CategoryName
            End Get
            Set(ByVal Value As String)
                m_CategoryName = Value
            End Set
        End Property

        Public Property AdminId() As Integer
            Get
                Return m_AdminId
            End Get
            Set(ByVal Value As Integer)
                m_AdminId = Value
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

        Public Sub New(ByVal DB As Database, ByVal FaqCategoryId As Integer)
            m_DB = DB
            m_FaqCategoryId = FaqCategoryId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_FAQCATEGORY_GETOBJECT As String = "sp_FaqCategory_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQCATEGORY_GETOBJECT)

                db.AddInParameter(cmd, "FaqCategoryId", DbType.Int32, FaqCategoryId)

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
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("FaqCategoryId"))) Then
                    m_FaqCategoryId = Convert.ToInt32(reader("FaqCategoryId"))
                Else
                    m_FaqCategoryId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                    m_SortOrder = Convert.ToInt32(reader("SortOrder"))
                Else
                    m_SortOrder = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CategoryName"))) Then
                    m_CategoryName = reader("CategoryName").ToString()
                Else
                    m_CategoryName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("AdminId"))) Then
                    m_AdminId = Convert.ToInt32(reader("AdminId"))
                Else
                    m_AdminId = 0
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer


            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_FAQCATEGORY_INSERT As String = "sp_FaqCategory_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQCATEGORY_INSERT)
            db.AddOutParameter(cmd, "FaqCategoryId", DbType.Int32, 32)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "CategoryName", DbType.String, CategoryName)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.ExecuteNonQuery(cmd)
            FaqCategoryId = Convert.ToInt32(db.GetParameterValue(cmd, "FaqCategoryId"))

            '------------------------------------------------------------------------

            Return FaqCategoryId
        End Function

        Private Function GetMaxSortOrder() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_FaqCategory_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_FAQCATEGORY_UPDATE As String = "sp_FaqCategory_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQCATEGORY_UPDATE)

            db.AddInParameter(cmd, "FaqCategoryId", DbType.Int32, FaqCategoryId)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "CategoryName", DbType.String, CategoryName)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_FAQCATEGORY_DELETE As String = "sp_FaqCategory_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQCATEGORY_DELETE)

            db.AddInParameter(cmd, "FaqCategoryId", DbType.Int32, FaqCategoryId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class FaqCategoryCollection
        Inherits GenericCollection(Of FaqCategoryRow)
    End Class

End Namespace
