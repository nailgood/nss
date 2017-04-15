Option Explicit On

'Author: Lam Le
'Date: 9/25/2009 3:48:44 PM

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

    Public Class AdminAdminGroupRow
        Inherits AdminAdminGroupRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New
        Public Sub New(ByVal Id As Integer)
            MyBase.New(Id)
        End Sub 'New
        'Shared function to get one row
        Public Shared Function GetRow(ByVal Id As Integer) As AdminAdminGroupRow
            Dim row As AdminAdminGroupRow
            row = New AdminAdminGroupRow(Id)
            row.Load()
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal Id As Integer)
            Dim row As AdminAdminGroupRow
            row = New AdminAdminGroupRow(Id)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Sub RemoveByAdmin(ByVal AdminId As Integer)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_DELETE As String = "sp_AdminAdminGroup_DeleteByAdmin"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_DELETE)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub

        Public Shared Function LoadGroupsWithoutPrivileges(ByVal AdminId As Integer) As DataSet
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_GETLIST As String = "sp_AdminAdminGroup_LoadGroupWithoutPrivileges"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_GETLIST)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

        Public Shared Function LoadGroupsWithPrivileges(ByVal AdminId As Integer) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_GETLIST As String = "sp_AdminAdminGroup_LoadGroupWithPrivileges"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_GETLIST)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function
    End Class

    Public MustInherit Class AdminAdminGroupRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_AdminId As Integer = Nothing
        Private m_GroupId As Integer = Nothing


        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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

        Public Property GroupId() As Integer
            Get
                Return m_GroupId
            End Get
            Set(ByVal Value As Integer)
                m_GroupId = Value
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

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New
        Public Sub New(ByVal Id As Integer)
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_ADMINADMINGROUP_GETOBJECT As String = "sp_AdminAdminGroup_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "AdminAdminGroup.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("AdminId"))) Then
                        m_AdminId = Convert.ToInt32(reader("AdminId"))
                    Else
                        m_AdminId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("GroupId"))) Then
                        m_GroupId = Convert.ToInt32(reader("GroupId"))
                    Else
                        m_GroupId = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load

        Public Overridable Sub Insert()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_INSERT As String = "sp_AdminAdminGroup_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_INSERT)
            db.AddOutParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Insert

        Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_INSERT As String = "sp_AdminAdminGroup_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_INSERT)
            db.AddOutParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)
            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            '------------------------------------------------------------------------
            Return Id
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_UPDATE As String = "sp_AdminAdminGroup_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_UPDATE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINADMINGROUP_DELETE As String = "sp_AdminAdminGroup_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINADMINGROUP_DELETE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class AdminAdminGroupCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal AdminAdminGroup As AdminAdminGroupRow)
            Me.List.Add(AdminAdminGroup)
        End Sub

        Public Function Contains(ByVal AdminAdminGroup As AdminAdminGroupRow) As Boolean
            Return Me.List.Contains(AdminAdminGroup)
        End Function

        Public Function IndexOf(ByVal AdminAdminGroup As AdminAdminGroupRow) As Integer
            Return Me.List.IndexOf(AdminAdminGroup)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal AdminAdminGroup As AdminAdminGroupRow)
            Me.List.Insert(Index, AdminAdminGroup)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminAdminGroupRow
            Get
                Return CType(Me.List.Item(Index), AdminAdminGroupRow)
            End Get

            Set(ByVal Value As AdminAdminGroupRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal AdminAdminGroup As AdminAdminGroupRow)
            Me.List.Remove(AdminAdminGroup)
        End Sub
        Public ReadOnly Property Clone() As AdminAdminGroupCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminAdminGroupCollection
                For Each obj As AdminAdminGroupRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace


