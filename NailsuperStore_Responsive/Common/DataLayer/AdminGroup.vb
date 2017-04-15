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

    Public Class AdminGroupRow
        Inherits AdminGroupRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal GroupId As Integer)
            MyBase.New(DB, GroupId)
        End Sub 'New

        Public Sub New(ByVal GroupId As Integer)
            MyBase.New(GroupId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal GroupId As Integer) As AdminGroupRow
            Dim row As AdminGroupRow
            row = New AdminGroupRow(GroupId)
            row.Load()
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal GroupId As Integer)
            Dim row As AdminGroupRow
            row = New AdminGroupRow(GroupId)
            row.Remove()
        End Sub

        'Custom Methods
    End Class

    Public MustInherit Class AdminGroupRowBase
        Private m_DB As Database
        Private m_GroupId As Integer = Nothing
        Private m_Description As String = Nothing

        Public Property GroupId() As Integer
            Get
                Return m_GroupId
            End Get
            Set(ByVal Value As Integer)
                m_GroupId = Value
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

        Public Sub New(ByVal DB As Database, ByVal GroupId As Integer)
            m_DB = DB
            m_GroupId = GroupId
        End Sub 'New
        Public Sub New(ByVal GroupId As Integer)
            m_GroupId = GroupId
        End Sub 'New
        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_ADMINGROUP_GETOBJECT As String = "sp_AdminGroup_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINGROUP_GETOBJECT)
                db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "AdminGroup.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("GroupId"))) Then
                        m_GroupId = Convert.ToInt32(reader("GroupId"))
                    Else
                        m_GroupId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        m_Description = reader("Description").ToString()
                    Else
                        m_Description = ""
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
            Dim SP_ADMINGROUP_INSERT As String = "sp_AdminGroup_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINGROUP_INSERT)
            db.AddOutParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Insert

        Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINGROUP_INSERT As String = "sp_AdminGroup_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINGROUP_INSERT)
            db.AddOutParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.ExecuteNonQuery(cmd)
            GroupId = Convert.ToInt32(db.GetParameterValue(cmd, "GroupId"))
            '------------------------------------------------------------------------
            Return GroupId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINGROUP_UPDATE As String = "sp_AdminGroup_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINGROUP_UPDATE)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.AddInParameter(cmd, "Description", DbType.String, Description)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 03:48:44 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINGROUP_DELETE As String = "sp_AdminGroup_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINGROUP_DELETE)
            db.AddInParameter(cmd, "GroupId", DbType.Int32, GroupId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class AdminGroupCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal AdminGroup As AdminGroupRow)
            Me.List.Add(AdminGroup)
        End Sub

        Public Function Contains(ByVal AdminGroup As AdminGroupRow) As Boolean
            Return Me.List.Contains(AdminGroup)
        End Function

        Public Function IndexOf(ByVal AdminGroup As AdminGroupRow) As Integer
            Return Me.List.IndexOf(AdminGroup)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal AdminGroup As AdminGroupRow)
            Me.List.Insert(Index, AdminGroup)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminGroupRow
            Get
                Return CType(Me.List.Item(Index), AdminGroupRow)
            End Get

            Set(ByVal Value As AdminGroupRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal AdminGroup As AdminGroupRow)
            Me.List.Remove(AdminGroup)
        End Sub
        Public ReadOnly Property Clone() As AdminGroupCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminGroupCollection
                For Each obj As AdminGroupRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class


End Namespace


