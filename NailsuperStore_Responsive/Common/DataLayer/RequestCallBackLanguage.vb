Option Explicit On

'Author: Lam Le
'Date: 9/28/2009 9:48:43 AM

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

    Public Class RequestCallBackLanguageRow
        Inherits RequestCallBackLanguageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal LanguageId As Integer)
            MyBase.New(DB, LanguageId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal LanguageId As Integer) As RequestCallBackLanguageRow
            Dim row As RequestCallBackLanguageRow

            row = New RequestCallBackLanguageRow(DB, LanguageId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal LanguageId As Integer)
            Dim row As RequestCallBackLanguageRow

            row = New RequestCallBackLanguageRow(DB, LanguageId)
            row.Remove()
        End Sub


        'Custom Methods
        Public Shared Function GetAllRequestCallBackLanguages(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:43 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_REQUESTCALLBACKLANGUAGE_GETLIST As String = "sp_RequestCallBackLanguage_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_REQUESTCALLBACKLANGUAGE_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)

            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetTypeRequestCallBackLanguages(ByVal DB As Database, ByVal LanguageID As Integer) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from RequestCallBackLanguage WHERE LanguageId = " & DB.Number(LanguageID))
            Return dt
        End Function
        Public Shared Function GetEmailLanguages(ByVal _DB As Database) As DataSet
            Return _DB.GetDataSet("select R.* from RequestCallBackLanguage R,vie_emaillanguage V where R.languageid = V.languageid group by R.languageid, R.language, R.languagecode")
        End Function
    End Class

    Public MustInherit Class RequestCallBackLanguageRowBase
        Private m_DB As Database
        Private m_LanguageId As Integer = Nothing
        Private m_Language As String = Nothing
        Private m_LanguageCode As String = Nothing
        Private m_EmailId As String = Nothing

        Public Property LanguageId() As Integer
            Get
                Return m_LanguageId
            End Get
            Set(ByVal Value As Integer)
                m_LanguageId = Value
            End Set
        End Property

        Public Property Language() As String
            Get
                Return m_Language
            End Get
            Set(ByVal Value As String)
                m_Language = Value
            End Set
        End Property

        Public Property LanguageCode() As String
            Get
                Return m_LanguageCode
            End Get
            Set(ByVal Value As String)
                m_LanguageCode = Value
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

        Public Sub New(ByVal DB As Database, ByVal LanguageId As Integer)
            m_DB = DB
            m_LanguageId = LanguageId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:43 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_REQUESTCALLBACKLANGUAGE_GETOBJECT As String = "sp_RequestCallBackLanguage_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_REQUESTCALLBACKLANGUAGE_GETOBJECT)
                db.AddInParameter(cmd, "LanguageId", DbType.Int32, LanguageId)
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
            'Date: September 28, 2009 09:48:43 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("LanguageId"))) Then
                    m_LanguageId = Convert.ToInt32(reader("LanguageId"))
                Else
                    m_LanguageId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Language"))) Then
                    m_Language = reader("Language").ToString()
                Else
                    m_Language = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LanguageCode"))) Then
                    m_LanguageCode = reader("LanguageCode").ToString()
                Else
                    m_LanguageCode = ""
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:43 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_REQUESTCALLBACKLANGUAGE_INSERT As String = "sp_RequestCallBackLanguage_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_REQUESTCALLBACKLANGUAGE_INSERT)

            db.AddOutParameter(cmd, "LanguageId", DbType.Int32, 32)
            db.AddInParameter(cmd, "Language", DbType.String, Language)
            db.AddInParameter(cmd, "LanguageCode", DbType.String, LanguageCode)

            db.ExecuteNonQuery(cmd)

            LanguageId = Convert.ToInt32(db.GetParameterValue(cmd, "LanguageId"))

            '------------------------------------------------------------------------

            Return LanguageId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:43 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_REQUESTCALLBACKLANGUAGE_UPDATE As String = "sp_RequestCallBackLanguage_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_REQUESTCALLBACKLANGUAGE_UPDATE)

            db.AddInParameter(cmd, "LanguageId", DbType.Int32, LanguageId)
            db.AddInParameter(cmd, "Language", DbType.String, Language)
            db.AddInParameter(cmd, "LanguageCode", DbType.String, LanguageCode)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update
        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:43 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_REQUESTCALLBACKLANGUAGE_DELETE As String = "sp_RequestCallBackLanguage_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_REQUESTCALLBACKLANGUAGE_DELETE)

            db.AddInParameter(cmd, "LanguageId", DbType.Int32, LanguageId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class RequestCallBackLanguageCollection
        Inherits GenericCollection(Of RequestCallBackLanguageRow)
    End Class

End Namespace



