Option Explicit On

'Author: Lam Le
'Date: 9/28/2009 9:48:49 AM

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

    Public Class TipRow
        Inherits TipRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal TipId As Integer)
            MyBase.New(DB, TipId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal TipId As Integer) As TipRow
            Dim row As TipRow

            row = New TipRow(DB, TipId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal TipId As Integer)
            Dim row As TipRow

            row = New TipRow(DB, TipId)
            row.Remove()
        End Sub

        'end 23/10/2009
        'Custom Methods
        Public Sub InsertTipItem(ByVal ItemId As Integer)
            Dim SQL As String = "if (select top 1 id from tipitem where tipid = " & TipId & " and itemid = " & ItemId & ") is null begin insert into tipitem (tipid,itemid) values (" & TipId & "," & ItemId & ") end"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub DeleteTipItem(ByVal DB As Database, ByVal TipId As Integer, ByVal ItemId As Integer)
            Dim SQL As String = "delete from tipitem where tipid = " & TipId & " and itemid = " & ItemId
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertTipDepartment(ByVal DepartmentId As Integer)
            Dim SQL As String = "if (select top 1 id from tipdepartment where TipId = " & TipId & " and departmentid = " & DepartmentId & ") is null begin insert into tipdepartment (TipId,departmentId) values (" & TipId & "," & DepartmentId & ") end"
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Sub DeleteTipDepartment(ByVal DB As Database, ByVal TipId As Integer, ByVal DepartmentId As Integer)
            Dim SQL As String = "delete from tipdepartment where TipId = " & TipId & " and departmentid = " & DepartmentId
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Function GetAllTips(ByVal DB As Database) As DataSet
            Dim SQL As String = "select t.*, tc.TipCategory from Tip t inner join TipCategory tc on t.TipCategoryId = tc.TipCategoryId order by tc.sortorder, t.sortorder"
            Return DB.GetDataSet(SQL)
        End Function

        Public Shared Function GetTipsByCategoryId(ByVal DB As Database, ByVal CategoryId As Integer, ByVal Condition As String) As DataTable
            Dim SQL As String = "select * from Tip where TipCategoryId = " & CategoryId & Condition & " and IsActive = 1 order by sortorder"
            Return DB.GetDataTable(SQL)
        End Function
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal TipId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Tip_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("TipId", SqlDbType.Int, 0, TipId))
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

    Public MustInherit Class TipRowBase
        Private m_DB As Database
        Private m_TipId As Integer = Nothing
        Private m_TipCategoryId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_Summary As String = Nothing
        Private m_FullText As String = Nothing
        Private m_VietTitle As String = Nothing
        Private m_VietSummary As String = Nothing
        Private m_VietText As String = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaTitle As String = Nothing
        Private m_MetaKeywords As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_IsActive As Boolean = True

        Public Property TipId() As Integer
            Get
                Return m_TipId
            End Get
            Set(ByVal Value As Integer)
                m_TipId = Value
            End Set
        End Property

        Public Property TipCategoryId() As Integer
            Get
                Return m_TipCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_TipCategoryId = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = Value
            End Set
        End Property

        Public Property Summary() As String
            Get
                Return m_Summary
            End Get
            Set(ByVal Value As String)
                m_Summary = Value
            End Set
        End Property

        Public Property FullText() As String
            Get
                Return m_FullText
            End Get
            Set(ByVal Value As String)
                m_FullText = Value
            End Set
        End Property

        Public Property VietTitle() As String
            Get
                Return m_VietTitle
            End Get
            Set(ByVal Value As String)
                m_VietTitle = Value
            End Set
        End Property

        Public Property VietSummary() As String
            Get
                Return m_VietSummary
            End Get
            Set(ByVal Value As String)
                m_VietSummary = Value
            End Set
        End Property

        Public Property VietText() As String
            Get
                Return m_VietText
            End Get
            Set(ByVal Value As String)
                m_VietText = Value
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

        Public Property MetaTitle() As String
            Get
                Return m_MetaTitle
            End Get
            Set(ByVal value As String)
                m_MetaTitle = value
            End Set
        End Property

        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal value As String)
                m_PageTitle = value
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

        Public Sub New(ByVal DB As Database, ByVal TipId As Integer)
            m_DB = DB
            m_TipId = TipId
        End Sub 'New

        'end 23/10/2009
        Protected Overridable Sub Load()
            'Dim r As SqlDataReader
            'Dim SQL As String

            'SQL = "SELECT * FROM Tip WHERE TipId = " & DB.Number(TipId)
            'r = m_DB.GetReader(SQL)
            'If r.Read Then
            '	Me.Load(r)
            'End If
            'r.Close()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:49 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_TIP_GETOBJECT As String = "sp_Tip_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_TIP_GETOBJECT)

                db.AddInParameter(cmd, "TipId", DbType.Int32, TipId)

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
            'Date: September 28, 2009 09:48:49 AM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("TipId"))) Then
                        m_TipId = Convert.ToInt32(reader("TipId"))
                    Else
                        m_TipId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TipCategoryId"))) Then
                        m_TipCategoryId = Convert.ToInt32(reader("TipCategoryId"))
                    Else
                        m_TipCategoryId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        m_Title = reader("Title").ToString()
                    Else
                        m_Title = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Summary"))) Then
                        m_Summary = reader("Summary").ToString()
                    Else
                        m_Summary = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("FullText"))) Then
                        m_FullText = reader("FullText").ToString()
                    Else
                        m_FullText = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("VietTitle"))) Then
                        m_VietTitle = reader("VietTitle").ToString()
                    Else
                        m_VietTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("VietSummary"))) Then
                        m_VietSummary = reader("VietSummary").ToString()
                    Else
                        m_VietSummary = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("VietText"))) Then
                        m_VietText = reader("VietText").ToString()
                    Else
                        m_VietText = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                        m_SortOrder = Convert.ToInt32(reader("SortOrder"))
                    Else
                        m_SortOrder = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        m_MetaDescription = reader("MetaDescription").ToString()
                    Else
                        m_MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeywords"))) Then
                        m_MetaKeywords = reader("MetaKeywords").ToString()
                    Else
                        m_MetaKeywords = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaTitle"))) Then
                        m_MetaTitle = reader("MetaTitle").ToString()
                    Else
                        m_MetaTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        m_PageTitle = reader("PageTitle").ToString()
                    Else
                        m_PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = True
                    End If
                End If
            Catch ex As Exception
                Throw ex
                ''Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:49 AM
            '------------------------------------------------------------------------
            Dim maxSortOrder As Integer = GetMaxSortOrder()

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_TIP_INSERT As String = "sp_Tip_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_TIP_INSERT)

            db.AddOutParameter(cmd, "TipId", DbType.Int32, TipId)
            db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "Summary", DbType.String, Summary)
            db.AddInParameter(cmd, "FullText", DbType.String, FullText)
            db.AddInParameter(cmd, "VietTitle", DbType.String, VietTitle)
            db.AddInParameter(cmd, "VietSummary", DbType.String, VietSummary)
            db.AddInParameter(cmd, "VietText", DbType.String, VietText)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, maxSortOrder)
            db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cmd, "MetaKeywords", DbType.String, MetaKeywords)
            db.AddInParameter(cmd, "MetaTitle", DbType.String, MetaTitle)
            db.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)

            db.ExecuteNonQuery(cmd)

            TipId = Convert.ToInt32(db.GetParameterValue(cmd, "TipId"))

            '------------------------------------------------------------------------
            Return TipId
        End Function

        Private Function GetMaxSortOrder() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_Tip_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:49 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_TIP_UPDATE As String = "sp_Tip_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_TIP_UPDATE)

            db.AddInParameter(cmd, "TipId", DbType.Int32, TipId)
            db.AddInParameter(cmd, "TipCategoryId", DbType.Int32, TipCategoryId)
            db.AddInParameter(cmd, "Title", DbType.String, Title)
            db.AddInParameter(cmd, "Summary", DbType.String, Summary)
            db.AddInParameter(cmd, "FullText", DbType.String, FullText)
            db.AddInParameter(cmd, "VietTitle", DbType.String, VietTitle)
            db.AddInParameter(cmd, "VietSummary", DbType.String, VietSummary)
            db.AddInParameter(cmd, "VietText", DbType.String, VietText)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
            db.AddInParameter(cmd, "MetaDescription", DbType.String, MetaDescription)
            db.AddInParameter(cmd, "MetaKeywords", DbType.String, MetaKeywords)
            db.AddInParameter(cmd, "MetaTitle", DbType.String, MetaTitle)
            db.AddInParameter(cmd, "PageTitle", DbType.String, PageTitle)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:49 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_TIP_DELETE As String = "sp_Tip_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_TIP_DELETE)

            db.AddInParameter(cmd, "TipId", DbType.Int32, TipId)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class TipCollection
        Inherits GenericCollection(Of TipRow)
    End Class

End Namespace


