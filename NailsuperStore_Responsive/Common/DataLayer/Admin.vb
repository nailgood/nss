Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility
Imports System.Web
Imports Components
Imports CryptData

Namespace DataLayer

    Public Class AdminRow
        Inherits AdminRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal AdminId As Integer)
            MyBase.New(DB, AdminId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal AdminId As Integer) As AdminRow
            Dim row As AdminRow

            row = New AdminRow(DB, AdminId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal AdminId As Integer)
            Dim row As AdminRow

            row = New AdminRow(DB, AdminId)
            row.Remove()
        End Sub

        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("Admin.vb", func, ex)
        End Sub

        Public Function GetPermissionList(ByVal DB As Database) As AdminSectionCollection
            Return AdminSectionRow.GetPermissionList(DB, AdminId)
        End Function

        Public Shared Function GetRowByUsername(ByVal UserName As String) As AdminRow
            Dim row As AdminRow = New AdminRow()
            Dim reader As SqlDataReader = Nothing

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_ADMIN_GETOBJECT As String = "sp_Admin_GetObjectByUserName"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMIN_GETOBJECT)
                db.AddInParameter(cmd, "UserName", DbType.String, UserName)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    row.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetRowByUsername(ByVal UserName As String)", ex)
            End Try

            Return row

        End Function

        Public Shared Function GetRowByEmail(ByVal Email As String) As String
            Dim str As String = String.Empty
            Dim key As String = String.Format("Admin_GetRowByEmail_{0}_", Email)
            str = CType(CacheUtils.GetCache(key), String)
            If Not str Is Nothing Then
                Return str
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "select top 1 (FirstName + ' ' + LastName) FROM Admin WHERE Email = '" & Email & "'"
                str = db.ExecuteScalar(sql)
                CacheUtils.SetCache(key, str, Utility.ConfigData.TimeCacheDataItem)
            Catch ex As Exception
                str = ""
            End Try
            Return str
        End Function

        Public Shared Function ValidateAdminCredentials(ByVal DB As Database, ByVal UserName As String, ByVal Password As String) As Integer
            Dim Encrypted As String = CryptData.Crypt.EncryptTripleDes(Password)

            ' CHECK IF USER EXISTS AND HAS ADMIN PROVILEGES
            Dim SQL As String = "select AdminId FROM Admin where IsActive = 1 and Username = " & DB.Quote(UserName) & " and Password = " & DB.Quote(Encrypted)
            Dim AdminId As Integer = CType(DB.ExecuteScalar(SQL), Integer)
            Return AdminId

        End Function
        Public Shared Function CheckAdminIpAcces(ByVal DB As Database, ByVal UserName As String, ByVal ip As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminIPAccess_CheckAcessValid"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("Username", SqlDbType.VarChar, 0, UserName))
                cmd.Parameters.Add(DB.InParam("IP", SqlDbType.VarChar, 0, ip))
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                SendMailLog("CheckAdminIpAcces(ByVal DB As Database, ByVal UserName As String, ByVal ip As String)", ex)
            End Try

            If result = 1 Then
                Return True
            End If
            Return False
        End Function

       
        Public Shared Function DoLogin(ByVal DB As Database, ByVal dbAdminLog As AdminLogRow) As Integer
            Return AdminLogRow.Insert(DB, dbAdminLog)
        End Function
        Public Shared Function GetAllAdmins(ByVal DB1 As Database) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMIN_GETLIST As String = "sp_Admin_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMIN_GETLIST)
            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function

        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal AdminId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Admin_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AdminId", SqlDbType.Int, 0, AdminId))
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

    Public MustInherit Class AdminRowBase
        Private m_DB As Database
        Private m_AdminId As Integer = Nothing
        Private m_Username As String = Nothing
        Private m_Password As String = Nothing
        Private m_Email As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_IsInternal As Boolean = Nothing
        Private m_IsActive As Boolean = Nothing

        Public Property AdminId() As Integer
            Get
                Return m_AdminId
            End Get
            Set(ByVal Value As Integer)
                m_AdminId = Value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return m_Username
            End Get
            Set(ByVal Value As String)
                m_Username = Value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return CryptData.Crypt.DecryptTripleDes(m_Password)
            End Get
            Set(ByVal Value As String)
                m_Password = CryptData.Crypt.EncryptTripleDes(Value)
            End Set
        End Property

        Public ReadOnly Property EncryptedPassword() As String
            Get
                If m_Password = String.Empty Then
                    Return String.Empty
                End If
                Return m_Password
            End Get
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal Value As String)
                m_FirstName = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal Value As String)
                m_LastName = Value
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

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal AdminId As Integer)
            m_DB = database
            m_AdminId = AdminId
        End Sub 'New

        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("Admin.vb", func, ex)
        End Sub

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_ADMIN_GETOBJECT As String = "sp_Admin_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMIN_GETOBJECT)
                db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("Load()", ex)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("AdminId"))) Then
                        m_AdminId = Convert.ToInt32(reader("AdminId"))
                    Else
                        m_AdminId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Username"))) Then
                        m_Username = reader("Username").ToString()
                    Else
                        m_Username = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Password"))) Then
                        m_Password = reader("Password").ToString()
                    Else
                        m_Password = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Email"))) Then
                        m_Email = reader("Email").ToString()
                    Else
                        m_Email = ""
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsInternal"))) Then
                        m_IsInternal = Convert.ToBoolean(reader("IsInternal"))
                    Else
                        m_IsInternal = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load

        Public Overridable Function AutoInsert() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMIN_INSERT As String = "sp_Admin_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMIN_INSERT)
            db.AddOutParameter(cmd, "AdminId", DbType.Int32, 32)
            db.AddInParameter(cmd, "Username", DbType.String, Username)
            db.AddInParameter(cmd, "Password", DbType.String, EncryptedPassword)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.AddInParameter(cmd, "FirstName", DbType.String, FirstName)
            db.AddInParameter(cmd, "LastName", DbType.String, LastName)
            db.AddInParameter(cmd, "IsInternal", DbType.Boolean, IsInternal)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.ExecuteNonQuery(cmd)
            AdminId = Convert.ToInt32(db.GetParameterValue(cmd, "AdminId"))
            '------------------------------------------------------------------------
            Return AdminId
        End Function

        Public Overridable Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMIN_UPDATE As String = "sp_Admin_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMIN_UPDATE)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.AddInParameter(cmd, "Username", DbType.String, Username)
            db.AddInParameter(cmd, "Password", DbType.String, EncryptedPassword)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.AddInParameter(cmd, "FirstName", DbType.String, FirstName)
            db.AddInParameter(cmd, "LastName", DbType.String, LastName)
            db.AddInParameter(cmd, "IsInternal", DbType.Boolean, IsInternal)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMIN_DELETE As String = "sp_Admin_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMIN_DELETE)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class AdminCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Admin As AdminRow)
            Me.List.Add(Admin)
        End Sub

        Public Function Contains(ByVal Admin As AdminRow) As Boolean
            Return Me.List.Contains(Admin)
        End Function

        Public Function IndexOf(ByVal Admin As AdminRow) As Integer
            Return Me.List.IndexOf(Admin)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Admin As AdminRow)
            Me.List.Insert(Index, Admin)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminRow
            Get
                Return CType(Me.List.Item(Index), AdminRow)
            End Get

            Set(ByVal Value As AdminRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Admin As AdminRow)
            Me.List.Remove(Admin)
        End Sub

        Public ReadOnly Property Clone() As AdminCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminCollection
                For Each obj As AdminRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace
