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

    Public Class AdminAccessRow
        Inherits AdminAccessRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New
        Public Sub New(ByVal Id As Integer)
            MyBase.New(Id)
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal Id As Integer) As AdminAccessRow
            Dim row As AdminAccessRow

            row = New AdminAccessRow(Id)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal Id As Integer)
            Dim row As AdminAccessRow

            row = New AdminAccessRow(Id)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Sub RemoveByGroup(ByVal GroupId As Integer)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:05 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_ADMINACCESS_DELETE As String = "sp_AdminAccess_DeleteByGroup"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINACCESS_DELETE)

            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub

        Public Shared Function LoadSectionsWithoutPrivileges(ByVal DB As Database, ByVal GroupId As Integer) As DataSet
            Dim SQL As New StringBuilder

            'Bind Left List
            If GroupId <> 0 Then
                SQL.Append("SELECT * FROM AdminSection WHERE SectionId NOT IN")
                SQL.Append("(")
                SQL.Append("SELECT SectionId FROM AdminAccess WHERE GroupId = " & DB.Quote(GroupId.ToString))
                SQL.Append(") ORDER BY DESCRIPTION")
            Else
                SQL.Append("SELECT * FROM AdminSection ORDER BY DESCRIPTION")
            End If
            Return DB.GetDataSet(SQL.ToString)
        End Function

        Public Shared Function LoadSectionsWithPrivileges(ByVal DB As Database, ByVal GroupId As Integer) As DataSet
            Dim SQL As New StringBuilder
            SQL.Append("SELECT * FROM AdminSection WHERE SectionId IN ")
            SQL.Append("(")
            SQL.Append("SELECT SectionId FROM AdminAccess WHERE GroupId = " & DB.Quote(GroupId))
            SQL.Append(")")
            Return DB.GetDataSet(SQL.ToString)
        End Function
    End Class

    Public MustInherit Class AdminAccessRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_SectionId As Integer = Nothing
        Private m_GroupId As Integer = Nothing

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = value
            End Set
        End Property

        Public Property SectionId() As Integer
            Get
                Return m_SectionId
            End Get
            Set(ByVal Value As Integer)
                m_SectionId = value
            End Set
        End Property

        Public Property GroupId() As Integer
            Get
                Return m_GroupId
            End Get
            Set(ByVal Value As Integer)
                m_GroupId = value
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
        Public Sub New(ByVal Id As Integer)
            m_Id = Id
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
           
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_ADMINACCESS_GETOBJECT As String = "sp_AdminAccess_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINACCESS_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "AdminAccess.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SectionId"))) Then
                        m_SectionId = Convert.ToInt32(reader("SectionId"))
                    Else
                        m_SectionId = 0
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
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINACCESS_INSERT As String = "sp_AdminAccess_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINACCESS_INSERT)
            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "SectionId", DbType.Int32, SectionId)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Insert

        Function AutoInsert() As Integer
            
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINACCESS_INSERT As String = "sp_AdminAccess_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINACCESS_INSERT)
            db.AddOutParameter(cmd, "Id", DbType.Int32, 32)
            db.AddInParameter(cmd, "SectionId", DbType.Int32, SectionId)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)
            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            '------------------------------------------------------------------------
            Return Id
        End Function

        Public Overridable Sub Update()
          
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINACCESS_UPDATE As String = "sp_AdminAccess_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINACCESS_UPDATE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.AddInParameter(cmd, "SectionId", DbType.Int32, SectionId)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINACCESS_DELETE As String = "sp_AdminAccess_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINACCESS_DELETE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class AdminAccessCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal AdminAccess As AdminAccessRow)
            Me.List.Add(AdminAccess)
        End Sub

        Public Function Contains(ByVal AdminAccess As AdminAccessRow) As Boolean
            Return Me.List.Contains(AdminAccess)
        End Function

        Public Function IndexOf(ByVal AdminAccess As AdminAccessRow) As Integer
            Return Me.List.IndexOf(AdminAccess)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal AdminAccess As AdminAccessRow)
            Me.List.Insert(Index, AdminAccess)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminAccessRow
            Get
                Return CType(Me.List.Item(Index), AdminAccessRow)
            End Get

            Set(ByVal Value As AdminAccessRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal AdminAccess As AdminAccessRow)
            Me.List.Remove(AdminAccess)
        End Sub
        Public ReadOnly Property Clone() As AdminAccessCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminAccessCollection
                For Each obj As AdminAccessRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace


