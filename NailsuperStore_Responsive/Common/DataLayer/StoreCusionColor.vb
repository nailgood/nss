Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:13:01 PM

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

    Public Class StoreCusionColorRow
        Inherits StoreCusionColorRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CusionColorId As Integer)
            MyBase.New(DB, CusionColorId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CusionColorId As Integer) As StoreCusionColorRow
            Dim row As StoreCusionColorRow

            row = New StoreCusionColorRow(DB, CusionColorId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CusionColorId As Integer)
            Dim row As StoreCusionColorRow

            row = New StoreCusionColorRow(DB, CusionColorId)
            row.Remove()
        End Sub


        'Custom Methods
        Public Shared Function GetAllRows(ByVal DB1 As Database) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORECUSIONCOLOR_GETLIST As String = "sp_StoreCusionColor_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECUSIONCOLOR_GETLIST)

            Return db.ExecuteDataSet(cmd)

            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetCusionColorNameById(ByVal id As Integer) As String
            If id < 1 Then
                Return String.Empty
            End If
            Dim result As String = String.Empty
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select CusionColor from StoreCusionColor where CusionColorId=" & id
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
    End Class

    Public MustInherit Class StoreCusionColorRowBase
        Private m_DB As Database
        Private m_CusionColorId As Integer = Nothing
        Private m_CusionColor As String = Nothing
        Private m_Swatch As String = Nothing


        Public Property CusionColorId() As Integer
            Get
                Return m_CusionColorId
            End Get
            Set(ByVal Value As Integer)
                m_CusionColorId = Value
            End Set
        End Property

        Public Property CusionColor() As String
            Get
                Return m_CusionColor
            End Get
            Set(ByVal Value As String)
                m_CusionColor = Value
            End Set
        End Property

        Public Property Swatch() As String
            Get
                Return m_Swatch
            End Get
            Set(ByVal Value As String)
                m_Swatch = Value
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

        Public Sub New(ByVal DB As Database, ByVal CusionColorId As Integer)
            m_DB = DB
            m_CusionColorId = CusionColorId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STORECUSIONCOLOR_GETOBJECT As String = "sp_StoreCusionColor_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECUSIONCOLOR_GETOBJECT)
                db.AddInParameter(cmd, "CusionColorId", DbType.Int32, CusionColorId)
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
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("CusionColorId"))) Then
                        m_CusionColorId = Convert.ToInt32(reader("CusionColorId"))
                    Else
                        m_CusionColorId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CusionColor"))) Then
                        m_CusionColor = reader("CusionColor").ToString()
                    Else
                        m_CusionColor = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Swatch"))) Then
                        m_Swatch = reader("Swatch").ToString()
                    Else
                        m_Swatch = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
                '' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORECUSIONCOLOR_INSERT As String = "sp_StoreCusionColor_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECUSIONCOLOR_INSERT)

            db.AddOutParameter(cmd, "CusionColorId", DbType.Int32, 32)
            db.AddInParameter(cmd, "CusionColor", DbType.String, CusionColor)
            db.AddInParameter(cmd, "Swatch", DbType.String, Swatch)

            db.ExecuteNonQuery(cmd)

            CusionColorId = Convert.ToInt32(db.GetParameterValue(cmd, "CusionColorId"))

            '------------------------------------------------------------------------
            Return CusionColorId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORECUSIONCOLOR_UPDATE As String = "sp_StoreCusionColor_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECUSIONCOLOR_UPDATE)

            db.AddInParameter(cmd, "CusionColorId", DbType.Int32, CusionColorId)
            db.AddInParameter(cmd, "CusionColor", DbType.String, CusionColor)
            db.AddInParameter(cmd, "Swatch", DbType.String, Swatch)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:13:01 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORECUSIONCOLOR_DELETE As String = "sp_StoreCusionColor_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECUSIONCOLOR_DELETE)

            db.AddInParameter(cmd, "CusionColorId", DbType.Int32, CusionColorId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StoreCusionColorCollection
        Inherits GenericCollection(Of StoreCusionColorRow)
    End Class

End Namespace


