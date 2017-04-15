Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:13:06 PM

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Web
Imports Components
Imports Utility

Namespace DataLayer

    Public Class SysParam
        Inherits SysparamRowBase

        Public Shared Function GetValue(ByVal DB As Database, ByVal ParamName As String) As String
            Dim ParamCollection As SysparamCollection = Nothing

            Dim context As HttpContext = HttpContext.Current
            If Not context Is Nothing Then
                ParamCollection = CType(context.Cache(cachePrefixKey), SysparamCollection)
            End If
            If ParamCollection Is Nothing Then
                ParamCollection = SysparamRow.GetCollection(DB)
                If Not context Is Nothing Then
                    context.Cache.Insert(cachePrefixKey, ParamCollection, Nothing, DateTime.Now.AddSeconds(10), TimeSpan.Zero)
                End If
            End If
            For Each row As SysparamRow In ParamCollection
                If row.Name = ParamName Then
                    If row.Type = "ENCRYPTEDSTRING" Then
                        If row.Value <> Nothing Then
                            Return Crypt.DecryptTripleDes(row.Value)
                        End If
                        Return Nothing
                    End If
                    Return row.Value
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function GetValue(ByVal ParamName As String) As String
            Dim result As String = Nothing
            Dim dr As SqlDataReader = Nothing
            Try
                Dim key As String = String.Format(cachePrefixKey & "_ListAll")
                Dim ht As New Hashtable()
                ht = CType(CacheUtils.GetCache(key), Hashtable)
                If Not ht Is Nothing Then
                    If ht.ContainsKey(ParamName) Then
                        result = ht.Item(ParamName).ToString()
                    End If
                Else
                    ht = New Hashtable()
                    dr = SysparamRow.GetListAll()
                    While dr.Read()
                        ht.Add(dr("Name"), dr("Value"))
                        Dim Name As String = dr("Name").ToString().ToLower()
                        If Name = ParamName.Trim().ToLower() Then
                            result = dr("Value").ToString()
                        End If

                    End While
                    Core.CloseReader(dr)

                    CacheUtils.SetCache(key, ht)
                End If

            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return result
        End Function
    End Class

    Public Class SysparamRow
        Inherits SysparamRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ParamId As Integer)
            MyBase.New(DB, ParamId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Paramname As String)
            MyBase.New(DB, Paramname)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ParamId As Integer) As SysparamRow
            Dim row As SysparamRow

            row = New SysparamRow(DB, ParamId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal ParamName As String) As SysparamRow
            Dim row As SysparamRow

            row = New SysparamRow(DB, ParamName)
            row.Load(ParamName)

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ParamId As Integer)
            Dim row As SysparamRow

            row = New SysparamRow(DB, ParamId)
            row.Remove()
        End Sub

        Public Shared Function GetListAll() As SqlDataReader

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Sysparam_GetListAll")

            Return db.ExecuteReader(cmd)
        End Function

        Public Shared Function GetList(ByVal DB1 As Database, ByVal IsInternal As Boolean, ByVal GroupName As String) As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SYSPARAM_GETLIST As String = "sp_Sysparam_GetList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_GETLIST)
            db.AddInParameter(cmd, "GroupName", DbType.String, GroupName)
            db.AddInParameter(cmd, "IsInternal", DbType.Boolean, IsInternal)

            Return db.ExecuteDataSet(cmd)
        End Function
       
        Public Shared Function GetCollection(ByVal DB As Database) As SysparamCollection
            Dim SQL As String
            Dim r As SqlDataReader = Nothing
            Dim collection As New SysparamCollection
            Try

                Dim row As SysparamRow
                SQL = "select * from Sysparam"
                r = DB.GetReader(SQL)
                While r.Read()
                    row = New SysparamRow(DB)
                    row.Load(r)
                    collection.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                '' SqlDataReader()
            End Try

            Return collection
        End Function

    End Class

    Public MustInherit Class SysparamRowBase
        Private m_DB As Database
        Private m_ParamId As Integer = Nothing
        Private m_GroupName As String = Nothing
        Private m_Name As String = Nothing
        Private m_Value As String = Nothing
        Private m_Type As String = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_IsInternal As Boolean = Nothing
        Private m_Comments As String = Nothing
        Public Shared cachePrefixKey As String = "SysParam_"

        Public Property ParamId() As Integer
            Get
                Return m_ParamId
            End Get
            Set(ByVal Value As Integer)
                m_ParamId = Value
            End Set
        End Property

        Public Property GroupName() As String
            Get
                Return m_GroupName
            End Get
            Set(ByVal Value As String)
                m_GroupName = Value
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

        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal Value As String)
                m_Type = Value
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

        Public Property IsInternal() As Boolean
            Get
                Return m_IsInternal
            End Get
            Set(ByVal Value As Boolean)
                m_IsInternal = Value
            End Set
        End Property

        Public Property Comments() As String
            Get
                Return m_Comments
            End Get
            Set(ByVal Value As String)
                m_Comments = Value
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

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ParamId As Integer)
            m_DB = database
            m_ParamId = ParamId
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Paramname As String)
            m_DB = database
            m_Name = Paramname
        End Sub 'New

        Protected Overridable Sub Load(ByVal paramName As String)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_SYSPARAM_GETOBJECT As String = "sp_Sysparam_GetObjectByName"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_GETOBJECT)
                db.AddInParameter(cmd, "ParamName", DbType.String, paramName)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_SYSPARAM_GETOBJECT As String = "sp_Sysparam_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_GETOBJECT)

                db.AddInParameter(cmd, "ParamId", DbType.Int32, ParamId)

                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ParamId"))) Then
                        m_ParamId = Convert.ToInt32(reader("ParamId"))
                    Else
                        m_ParamId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("GroupName"))) Then
                        m_GroupName = reader("GroupName").ToString()
                    Else
                        m_GroupName = ""
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                        m_Type = reader("Type").ToString()
                    Else
                        m_Type = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                        m_SortOrder = Convert.ToInt32(reader("SortOrder"))
                    Else
                        m_SortOrder = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsInternal"))) Then
                        m_IsInternal = Convert.ToBoolean(reader("IsInternal"))
                    Else
                        m_IsInternal = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Comments"))) Then
                        m_Comments = reader("Comments").ToString()
                    Else
                        m_Comments = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
                ''  Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Sub Insert()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SYSPARAM_INSERT As String = "sp_Sysparam_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_INSERT)

            db.AddOutParameter(cmd, "ParamId", DbType.Int32, 32)
            db.AddInParameter(cmd, "GroupName", DbType.String, GroupName)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Value", DbType.String, Value)
            db.AddInParameter(cmd, "Type", DbType.String, Type)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
            db.AddInParameter(cmd, "IsInternal", DbType.Boolean, IsInternal)
            db.AddInParameter(cmd, "Comments", DbType.String, Comments)

            db.ExecuteNonQuery(cmd)

            CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
            '------------------------------------------------------------------------
        End Sub 'Insert

        Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SYSPARAM_INSERT As String = "sp_Sysparam_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_INSERT)

            db.AddOutParameter(cmd, "ParamId", DbType.Int32, 32)
            db.AddInParameter(cmd, "GroupName", DbType.String, GroupName)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Value", DbType.String, Value)
            db.AddInParameter(cmd, "Type", DbType.String, Type)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
            db.AddInParameter(cmd, "IsInternal", DbType.Boolean, IsInternal)
            db.AddInParameter(cmd, "Comments", DbType.String, Comments)

            db.ExecuteNonQuery(cmd)

            ParamId = Convert.ToInt32(db.GetParameterValue(cmd, "ParamId"))
            '------------------------------------------------------------------------

            Return ParamId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SYSPARAM_UPDATE As String = "sp_Sysparam_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_UPDATE)

            db.AddInParameter(cmd, "ParamId", DbType.Int32, ParamId)
            db.AddInParameter(cmd, "GroupName", DbType.String, GroupName)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Value", DbType.String, Value)
            db.AddInParameter(cmd, "Type", DbType.String, Type)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
            db.AddInParameter(cmd, "IsInternal", DbType.Boolean, IsInternal)
            db.AddInParameter(cmd, "Comments", DbType.String, Comments)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
            CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:06 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SYSPARAM_DELETE As String = "sp_Sysparam_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SYSPARAM_DELETE)

            db.AddInParameter(cmd, "ParamId", DbType.Int32, ParamId)

            db.ExecuteNonQuery(cmd)
            CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class SysparamCollection
        Inherits GenericCollection(Of SysparamRow)
    End Class

End Namespace


