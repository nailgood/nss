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

    Public Class MemberTypeRow
        Inherits MemberTypeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MemberTypeId As Integer)
            MyBase.New(DB, MemberTypeId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal MemberTypeId As Integer) As MemberTypeRow
            Dim row As MemberTypeRow

            row = New MemberTypeRow(DB, MemberTypeId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MemberTypeId As Integer)
            Dim row As MemberTypeRow

            row = New MemberTypeRow(DB, MemberTypeId)
            row.Remove()
        End Sub

        'Custom Methods

        Public Shared Function GetMemberTypes(ByVal db1 As Database) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERTYPE_GETLIST As String = "sp_MemberType_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_GETLIST)

            Return db.ExecuteDataSet(cmd)

            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetAllMemberTypes(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERTYPE_GETLIST As String = "sp_MemberType_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)

            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetAllMemberTypes1(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERTYPE_GETLIST As String = "sp_MemberType_GetListAll1"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetMemberTypeById(ByVal id As Integer) As String
            Dim result As String = String.Empty
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select MemberType from MemberType where memberTypeId=" & id
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = reader.GetValue(0)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
    End Class

    Public MustInherit Class MemberTypeRowBase
        Private m_DB As Database
        Private m_MemberTypeId As Integer = Nothing
        Private m_MemberType As String = Nothing
        Private m_Active As Boolean = Nothing
        Private m_NavisionCode As String

        Public Property NavisionCode() As String
            Get
                Return m_NavisionCode
            End Get
            Set(ByVal value As String)
                m_NavisionCode = value
            End Set
        End Property


        Public Property MemberTypeId() As Integer
            Get
                Return m_MemberTypeId
            End Get
            Set(ByVal Value As Integer)
                m_MemberTypeId = Value
            End Set
        End Property

        Public Property MemberType() As String
            Get
                Return m_MemberType
            End Get
            Set(ByVal Value As String)
                m_MemberType = Value
            End Set
        End Property

        Public Property Active() As Boolean
            Get
                Return m_Active
            End Get
            Set(ByVal Value As Boolean)
                m_Active = Value
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

        Public Sub New(ByVal DB As Database, ByVal MemberTypeId As Integer)
            m_DB = DB
            m_MemberTypeId = MemberTypeId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_MEMBERTYPE_GETOBJECT As String = "sp_MemberType_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_GETOBJECT)
                db.AddInParameter(cmd, "MemberTypeId", DbType.Int32, MemberTypeId)
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
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("MemberTypeId"))) Then
                    MemberTypeId = Convert.ToInt32(reader("MemberTypeId"))
                Else
                    MemberTypeId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MemberType"))) Then
                    MemberType = reader("MemberType").ToString()
                Else
                    MemberType = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Active"))) Then
                Else
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NavisionCode"))) Then
                    NavisionCode = reader("NavisionCode").ToString()
                Else
                    NavisionCode = ""
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERTYPE_INSERT As String = "sp_MemberType_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_INSERT)

            db.AddOutParameter(cmd, "MemberTypeId", DbType.Int32, MemberTypeId)
            db.AddInParameter(cmd, "MemberType", DbType.String, MemberType)
            db.AddInParameter(cmd, "Active", DbType.Boolean, Active)
            db.AddInParameter(cmd, "NavisionCode", DbType.String, NavisionCode)

            db.ExecuteNonQuery(cmd)

            MemberTypeId = Convert.ToInt32(db.GetParameterValue(cmd, "MemberTypeId"))

            '------------------------------------------------------------------------
            Return MemberTypeId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERTYPE_UPDATE As String = "sp_MemberType_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_UPDATE)

            db.AddInParameter(cmd, "MemberTypeId", DbType.Int32, MemberTypeId)
            db.AddInParameter(cmd, "MemberType", DbType.String, MemberType)
            db.AddInParameter(cmd, "Active", DbType.Boolean, Active)
            db.AddInParameter(cmd, "NavisionCode", DbType.String, NavisionCode)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:50 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MEMBERTYPE_DELETE As String = "sp_MemberType_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MEMBERTYPE_DELETE)

            db.AddInParameter(cmd, "MemberTypeId", DbType.Int32, MemberTypeId)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class MemberTypeCollection
        Inherits GenericCollection(Of MemberTypeRow)
    End Class

End Namespace


