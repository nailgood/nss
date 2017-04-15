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

    Public Class SplashPageRow
        Inherits SplashPageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SplashPageImageId As Integer)
            MyBase.New(DB, SplashPageImageId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SplashPageImageId As Integer) As SplashPageRow
            Dim row As SplashPageRow

            row = New SplashPageRow(DB, SplashPageImageId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SplashPageImageId As Integer)
            Dim row As SplashPageRow

            row = New SplashPageRow(DB, SplashPageImageId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetActiveRow(ByVal _DB As Database) As SplashPageRow
            Dim SplashPageImageId As Integer = GetActiveID()
            Return SplashPageRow.GetRow(_DB, SplashPageImageId)
        End Function

        Private Shared Function GetActiveID() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------

            Dim pageImageId As Integer

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SPLASHPAGE_GETOBJECT As String = "sp_SplashPage_GetActiveID"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SPLASHPAGE_GETOBJECT)

            pageImageId = Convert.ToInt32(db.ExecuteScalar(cmd))

            Return pageImageId

            '------------------------------------------------------------------------
        End Function

    End Class

    Public MustInherit Class SplashPageRowBase
        Private m_DB As Database
        Private m_SplashPageImageId As Integer = Nothing
        Private m_ImageName As String = Nothing
        Private m_Image As String = Nothing
        Private m_ImageMap As String = Nothing
        Private m_IsActive As Boolean = Nothing

        Public Property SplashPageImageId() As Integer
            Get
                Return m_SplashPageImageId
            End Get
            Set(ByVal Value As Integer)
                m_SplashPageImageId = Value
            End Set
        End Property

        Public Property ImageName() As String
            Get
                Return m_ImageName
            End Get
            Set(ByVal Value As String)
                m_ImageName = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
            End Set
        End Property

        Public Property ImageMap() As String
            Get
                Return m_ImageMap
            End Get
            Set(ByVal Value As String)
                m_ImageMap = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SplashPageImageId As Integer)
            m_DB = DB
            m_SplashPageImageId = SplashPageImageId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_SPLASHPAGE_GETOBJECT As String = "sp_SplashPage_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SPLASHPAGE_GETOBJECT)
                db.AddInParameter(cmd, "SplashPageImageId", DbType.Int32, SplashPageImageId)
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("SplashPageImageId"))) Then
                        m_SplashPageImageId = Convert.ToInt32(reader("SplashPageImageId"))
                    Else
                        m_SplashPageImageId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ImageName"))) Then
                        m_ImageName = reader("ImageName").ToString()
                    Else
                        m_ImageName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ImageMap"))) Then
                        m_ImageMap = reader("ImageMap").ToString()
                    Else
                        m_ImageMap = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                        m_Image = reader("Image").ToString()
                    Else
                        m_Image = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                End If
            Catch ex As Exception
                Throw ex
                ''Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SPLASHPAGE_INSERT As String = "sp_SplashPage_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SPLASHPAGE_INSERT)

            db.AddOutParameter(cmd, "SplashPageImageId", DbType.Int32, 32)
            db.AddInParameter(cmd, "ImageName", DbType.String, ImageName)
            db.AddInParameter(cmd, "ImageMap", DbType.String, ImageMap)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)

            db.ExecuteNonQuery(cmd)

            SplashPageImageId = Convert.ToInt32(db.GetParameterValue(cmd, "SplashPageImageId"))

            '------------------------------------------------------------------------

            Return SplashPageImageId
        End Function

        Private Sub ClearActiveImages()
            If IsActive Then
                'DB.ExecuteSQL("update splashpage set isactive = 0 where splashpageimageid <> " & SplashPageImageId)
            End If
        End Sub

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SPLASHPAGE_UPDATE As String = "sp_SplashPage_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SPLASHPAGE_UPDATE)

            db.AddInParameter(cmd, "SplashPageImageId", DbType.Int32, SplashPageImageId)
            db.AddInParameter(cmd, "ImageName", DbType.String, ImageName)
            db.AddInParameter(cmd, "ImageMap", DbType.String, ImageMap)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_SPLASHPAGE_DELETE As String = "sp_SplashPage_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_SPLASHPAGE_DELETE)

            db.AddInParameter(cmd, "SplashPageImageId", DbType.Int32, SplashPageImageId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

End Namespace


