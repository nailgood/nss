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
Imports Database
Imports System.Text.RegularExpressions

Namespace DataLayer

    Public Class TipCategoryRow
        Inherits TipCategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal TipCategoryId As Integer)
            MyBase.New(DB, TipCategoryId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal TipCategoryId As Integer) As TipCategoryRow
            Dim row As TipCategoryRow

            row = New TipCategoryRow(DB, TipCategoryId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal TipCategoryId As Integer)
            Dim row As TipCategoryRow

            row = New TipCategoryRow(DB, TipCategoryId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllTipCategories(ByVal DB As Database) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from TipCategory order by TipCategory")
            Return dt
        End Function

        Public Shared Function GetAllTipCategoriesWithTips(ByVal DB As Database) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select " & IIf(System.Web.HttpContext.Current.Session("Language") = "VN", "tipcategoryid, sortorder, coalesce(vietcategory,tipcategory) as tipcategory, coalesce(vietdescription,description) as description ", " * ") & " from TipCategory where tipcategoryid in (select tipcategoryid from tip) order by TipCategory")
            Return dt
        End Function

        Public Sub InsertTipCategoryItem(ByVal ItemId As Integer)
            Dim SQL As String = "if (select top 1 id from TipCategoryitem where TipCategoryid = " & TipCategoryId & " and itemid = " & ItemId & ") is null begin insert into TipCategoryitem (TipCategoryid,itemid) values (" & TipCategoryId & "," & ItemId & ") end"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub DeleteTipCategoryItem(ByVal DB As Database, ByVal TipCategoryId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "delete from TipCategoryitem where TipCategoryid = " & TipCategoryId & " and itemid = " & ItemId
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertTipCategoryDepartment(ByVal DepartmentId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_INSERT As String = "sp_TipCategoryDepartment_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_INSERT)

            db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)

            db.ExecuteNonQuery(cmd)

        End Sub

        Public Shared Sub DeleteTipCategoryDepartment(ByVal DB1 As Database, ByVal TipCategoryId As Integer, ByVal DepartmentId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_TipCategoryDepartment_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)

            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub

        Public Shared Function ListTipCategory(ByVal data As TipCategoryRow) As List(Of TipCategoryRow)
            Dim result As New List(Of TipCategoryRow)
            Dim dr As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_TipCategory_ListTipCategory"

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Condition", DbType.String, data.Condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, data.OrderBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, data.OrderDirection)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, data.PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, data.PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    result = mapList(Of TipCategoryRow)(dr)
                    data.TotalRow = Convert.ToInt32(cmd.Parameters("@TotalRecords").Value)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ListTipCategory", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function FullByCategoryId(ByVal cateId As Integer) As String
            Dim result As String = String.Empty
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_TipCategory_FullByCategoryId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, cateId)
                result = db.ExecuteScalar(cmd)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "FullByCategoryId", "Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function SetXMLtag(ByVal colName As String, ByVal Value As String, ByVal cData As Boolean)
            Return vbCrLf & "<" & colName & ">" & IIf(cData, CheckCDATA(Value), Value) & "</" & colName & ">"
        End Function

        Private Shared Function CheckCDATA(ByVal strValue As String) As String

            Dim pattern As String = "[^a-zA-Z0-9]"
            If (Regex.IsMatch(strValue, pattern)) Then
                Return "<![CDATA[" & strValue & "]]>"
            End If
            Return strValue
        End Function
    End Class

    Public MustInherit Class TipCategoryRowBase
        Private m_DB As Database
        Private m_TipCategoryId As Integer = Nothing
        Private m_TipCategory As String = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_VietCategory As String = Nothing
        Private m_Description As String = Nothing
        Private m_VietDescription As String = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeywords As String = Nothing

        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing
        Private m_Condition As String = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_TotalRow As Integer = Nothing
        Public itemindex As Integer = 0
        Public Property TipCategoryId() As Integer
            Get
                Return m_TipCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_TipCategoryId = value
            End Set
        End Property

        Public Property TipCategory() As String
            Get
                Return m_TipCategory
            End Get
            Set(ByVal Value As String)
                m_TipCategory = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = value
            End Set
        End Property

        Public Property VietCategory() As String
            Get
                Return m_VietCategory
            End Get
            Set(ByVal Value As String)
                m_VietCategory = Value
            End Set
        End Property

        Public Property VietDescription() As String
            Get
                Return m_VietDescription
            End Get
            Set(ByVal Value As String)
                m_VietDescription = Value
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

        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal value As String)
                m_MetaDescription = value
            End Set
        End Property

        Public Property MetaKeywords() As String
            Get
                Return m_MetaKeywords
            End Get
            Set(ByVal value As String)
                m_MetaKeywords = value
            End Set
        End Property
        Public Property OrderBy() As String
            Get
                Return m_OrderBy
            End Get
            Set(ByVal value As String)
                m_OrderBy = value
            End Set
        End Property
        Public Property OrderDirection() As String
            Get
                Return m_OrderDirection
            End Get
            Set(ByVal value As String)
                m_OrderDirection = value
            End Set
        End Property
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal value As String)
                m_Condition = value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal value As Integer)
                m_PageIndex = value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal value As Integer)
                m_PageSize = value
            End Set
        End Property
        Public Property TotalRow() As Integer
            Get
                Return m_TotalRow
            End Get
            Set(ByVal value As Integer)
                m_TotalRow = value
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

        Public Sub New(ByVal DB As Database, ByVal TipCategoryId As Integer)
            m_DB = DB
            m_TipCategoryId = TipCategoryId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_GETOBJECT As String = "sp_TipCategory_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

                db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)

                reader = db.ExecuteReader(cmd)

                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_TipCategoryId = Convert.ToInt32(r.Item("TipCategoryId"))
                m_TipCategory = Convert.ToString(r.Item("TipCategory"))
                If IsDBNull(r.Item("Description")) Then
                    m_Description = Nothing
                Else
                    m_Description = Convert.ToString(r.Item("Description"))
                End If
                If IsDBNull(r.Item("VietCategory")) Then
                    m_VietCategory = Nothing
                Else
                    m_VietCategory = Convert.ToString(r.Item("VietCategory"))
                End If
                If IsDBNull(r.Item("VietDescription")) Then
                    m_VietDescription = Nothing
                Else
                    m_VietDescription = Convert.ToString(r.Item("VietDescription"))
                End If
                m_MetaDescription = Convert.ToString(r.Item("MetaDescription"))
                m_MetaKeywords = Convert.ToString(r.Item("MetaKeywords"))
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim maxSortOrder As Integer = GetMaxSortOrder()

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_INSERT As String = "sp_TipCategory_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_INSERT)

            db.AddInParameter(cmd, "TipCategory", DbType.String, TipCategory)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, maxSortOrder)
            db.AddInParameter(cmd, "VietCategory", DbType.String, VietCategory)
            db.AddInParameter(cmd, "VietDescription", DbType.String, VietDescription)
            db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cmd, "MetaKeywords", DbType.String, MetaKeywords)
            db.AddOutParameter(cmd, "TipCategoryId", DbType.Int32, 16)

            db.ExecuteNonQuery(cmd)

            TipCategoryId = CType(db.GetParameterValue(cmd, "TipCategoryId"), Integer)

            Return TipCategoryId
        End Function

        Private Function GetMaxSortOrder() As Integer

            Dim result As Integer = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETMAX As String = "sp_TipCategory_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETMAX)

            Dim reader As IDataReader = db.ExecuteReader(cmd)

            If (reader.Read()) Then
                result = CType(reader(0), Integer)
            End If

            If Not reader.IsClosed Then
                reader.Close()
                reader.Dispose()
            End If

            Return result
        End Function

        Public Overridable Sub Update()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_TipCategory_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)
            db.AddInParameter(cmd, "TipCategory", DbType.String, TipCategory)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.AddInParameter(cmd, "VietCategory", DbType.String, VietCategory)
            db.AddInParameter(cmd, "VietDescription", DbType.String, VietDescription)
            db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cmd, "MetaKeywords", DbType.String, MetaKeywords)

            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_TipCategory_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)

            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class TipCategoryCollection
        Inherits GenericCollection(Of TipCategoryRow)
    End Class

End Namespace


