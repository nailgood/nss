Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:12:55 PM

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

    Public Class StoreBaseColorRow
        Inherits StoreBaseColorRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal BaseColorId As Integer)
            MyBase.New(DB, BaseColorId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal BaseColorId As Integer) As StoreBaseColorRow
            Dim row As StoreBaseColorRow

            row = New StoreBaseColorRow(DB, BaseColorId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal BaseColorId As Integer)
            Dim row As StoreBaseColorRow

            row = New StoreBaseColorRow(DB, BaseColorId)
            row.Remove()
        End Sub

        'end 23/10/2009
        'Custom Methods
        Public Shared Function GetAllRows(ByVal DB1 As Database) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREBASECOLOR_GETLIST As String = "sp_StoreBaseColor_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREBASECOLOR_GETLIST)

            Return db.ExecuteDataSet(cmd)

            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetBaseColorNameById(ByVal id As Integer) As String
            If id < 1 Then
                Return String.Empty
            End If
            Dim result As String = String.Empty
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select BaseColor from StoreBaseColor where BaseColorId=" & id
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

    Public MustInherit Class StoreBaseColorRowBase
        Private m_DB As Database
        Private m_BaseColorId As Integer = Nothing
        Private m_BaseColor As String = Nothing
        Private m_Swatch As String = Nothing


        Public Property BaseColorId() As Integer
            Get
                Return m_BaseColorId
            End Get
            Set(ByVal Value As Integer)
                m_BaseColorId = Value
            End Set
        End Property

        Public Property BaseColor() As String
            Get
                Return m_BaseColor
            End Get
            Set(ByVal Value As String)
                m_BaseColor = Value
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

        Public Sub New(ByVal DB As Database, ByVal BaseColorId As Integer)
            m_DB = DB
            m_BaseColorId = BaseColorId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STOREBASECOLOR_GETOBJECT As String = "sp_StoreBaseColor_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREBASECOLOR_GETOBJECT)
                db.AddInParameter(cmd, "BaseColorId", DbType.Int32, BaseColorId)
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
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("BaseColorId"))) Then
                        m_BaseColorId = Convert.ToInt32(reader("BaseColorId"))
                    Else
                        m_BaseColorId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("BaseColor"))) Then
                        m_BaseColor = reader("BaseColor").ToString()
                    Else
                        m_BaseColor = ""
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
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREBASECOLOR_INSERT As String = "sp_StoreBaseColor_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREBASECOLOR_INSERT)

            db.AddOutParameter(cmd, "BaseColorId", DbType.Int32, 32)
            db.AddInParameter(cmd, "BaseColor", DbType.String, BaseColor)
            db.AddInParameter(cmd, "Swatch", DbType.String, Swatch)

            db.ExecuteNonQuery(cmd)

            BaseColorId = Convert.ToInt32(db.GetParameterValue(cmd, "BaseColorId"))
            '------------------------------------------------------------------------
            Return BaseColorId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREBASECOLOR_UPDATE As String = "sp_StoreBaseColor_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREBASECOLOR_UPDATE)

            db.AddInParameter(cmd, "BaseColorId", DbType.Int32, BaseColorId)
            db.AddInParameter(cmd, "BaseColor", DbType.String, BaseColor)
            db.AddInParameter(cmd, "Swatch", DbType.String, Swatch)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREBASECOLOR_DELETE As String = "sp_StoreBaseColor_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREBASECOLOR_DELETE)

            db.AddInParameter(cmd, "BaseColorId", DbType.Int32, BaseColorId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StoreBaseColorCollection
        Inherits GenericCollection(Of StoreBaseColorRow)
    End Class

End Namespace


