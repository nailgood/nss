Option Explicit On

Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Utility
Namespace DataLayer
    Public Class ContactSkypeRow
        Inherits ContactSkypeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SkypeID As Integer)
            MyBase.New(DB, SkypeID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SkypeID As Integer) As ContactSkypeRow
            Dim row As ContactSkypeRow

            row = New ContactSkypeRow(DB, SkypeID)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SkypeID As Integer)
            Dim row As ContactSkypeRow

            row = New ContactSkypeRow(DB, SkypeID)
            row.Remove()
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
        End Sub
       
        'Custom Methods
        Public Shared Function GetAllContactSkypes() As DataTable
            Dim key As String = cachePrefixKey & "GetAllContactSkypes"
            Dim result As DataTable
            result = CType(CacheUtils.GetCache(key), DataTable)
            If Not result Is Nothing AndAlso result.Rows.Count > 0 Then
                Return result
            End If

            '---------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_ContactSkype_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            result = db.ExecuteDataSet(cmd).Tables(0)
            CacheUtils.SetCache(key, result)
            Return result

        End Function
    End Class

    Public MustInherit Class ContactSkypeRowBase
        Private m_DB As Database
        Private m_SkypeID As Integer = Nothing
        Private m_Skype As String = Nothing
        Private m_Name As String = Nothing
        Private m_Sort As Integer = Nothing
        Public Shared cachePrefixKey As String = "ContactSkype_"
        Public Property SkypeID() As Integer
            Get
                Return m_SkypeID
            End Get
            Set(ByVal Value As Integer)
                m_SkypeID = Value
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

        Public Property Skype() As String
            Get
                Return m_Skype
            End Get
            Set(ByVal Value As String)
                m_Skype = Value
            End Set
        End Property

        Public Property Sort() As Integer
            Get
                Return m_Sort
            End Get
            Set(ByVal Value As Integer)
                m_Sort = Value
            End Set
        End Property

        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As DataBase)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SkypeID As Integer)
            m_DB = DB
            m_SkypeID = SkypeID
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_ContactSkype_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "SkypeID", DbType.Int32, SkypeID)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            
        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_SkypeID = Convert.ToInt32(r.Item("SkypeID"))
            m_Skype = Convert.ToString(r.Item("Skype"))
            m_Name = Convert.ToString(r.Item("Name"))
            m_Sort = Convert.ToInt32(r.Item("Sort"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            'Dim SQL As String

            ''Dim MaxSortOrder As Integer = DB.ExecuteScalar("select top 1 SortOrder from ContactSkype order by SortOrder desc")
            ''MaxSortOrder += 1

            'SQL = " INSERT INTO ContactSkype (" _
            ' & " Name" _
            ' & ",Skype" _
            ' & ",Sort" _
            ' & ") VALUES (" _
            ' & m_DB.Quote(Name) _
            ' & "," & m_DB.Quote(Skype) _
            ' & "," & Sort _
            ' & ")"

            'SkypeID = m_DB.InsertSQL(SQL)

            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_ContactSkype_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddOutParameter(cmd, "SkypeID", DbType.Int32, 16)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Skype", DbType.String, Skype)
            db.AddInParameter(cmd, "Sort", DbType.String, Sort)

            db.ExecuteNonQuery(cmd)

            SkypeID = Convert.ToInt32(db.GetParameterValue(cmd, "SkypeID"))

            '----------------------------------------------------------------------
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            Return SkypeID
        End Function

        Public Overridable Sub Update()
            'Dim SQL As String

            'SQL = " UPDATE ContactSkype SET " _
            ' & " Name = " & m_DB.Quote(Name) _
            ' & ", Skype = " & m_DB.Quote(Skype) _
            ' & ", Sort = " & Sort _
            ' & " WHERE SkypeID = " & m_DB.Quote(SkypeID)

            'm_DB.ExecuteSQL(SQL)
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_ContactSkype_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddInParameter(cmd, "SkypeID", DbType.Int32, SkypeID)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Skype", DbType.String, Skype)
            'db.AddInParameter(cmd, "Sort", DbType.Int32, Sort)

            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            '----------------------------------------------------------------------

        End Sub 'Update      

        Public Sub Remove()
            'Dim SQL As String

            'SQL = "DELETE FROM ContactSkype WHERE SkypeID = " & m_DB.Quote(SkypeID)
            'm_DB.ExecuteSQL(SQL)

            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_ContactSkype_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "SkypeID", DbType.Int32, SkypeID)

            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ContactSkypeCollection
        Inherits GenericCollection(Of ContactSkypeRow)
    End Class

End Namespace
