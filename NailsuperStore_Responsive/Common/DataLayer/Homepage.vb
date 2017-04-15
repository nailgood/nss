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

    Public Class HomepageRow
        Inherits HomepageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal HomepageImageId As Integer)
            MyBase.New(DB, HomepageImageId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal HomepageImageId As Integer) As HomepageRow
            Dim row As HomepageRow

            row = New HomepageRow(DB, HomepageImageId)
            row.Load()

            Return row
        End Function
        
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal HomepageImageId As Integer)
            Dim row As HomepageRow

            row = New HomepageRow(DB, HomepageImageId)
            row.Remove()
        End Sub
       
        'Custom Methods
        Public Shared Function GetActiveRow(ByVal _DB As Database) As HomepageRow
            Dim HomePageImageId = GetActiveID()

            Return HomepageRow.GetRow(_DB, HomePageImageId)
        End Function

        Private Shared Function GetActiveID() As Integer
            '-----------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 27, 2009
            '-----------------------------------------------------------------------

            Dim activeId As Integer = 0

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_HomePage_GetActiveID"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            activeId = Convert.ToInt32(db.ExecuteScalar(cmd))

            Return activeId
            '-----------------------------------------------------------------------
        End Function
    End Class


    Public MustInherit Class HomepageRowBase
        Private m_DB As Database
        Private m_HomepageImageId As Integer = Nothing
        Private m_ImageName As String = Nothing
        Private m_Image As String = Nothing
        Private m_ImageMap As String = Nothing
        Private m_IsActive As Boolean = Nothing

        Public Property HomepageImageId() As Integer
            Get
                Return m_HomepageImageId
            End Get
            Set(ByVal Value As Integer)
                m_HomepageImageId = Value
            End Set
        End Property

        Public Property ImageName() As String
            Get
                Return m_ImageName
            End Get
            Set(ByVal Value As String)
                m_ImageName = value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = value
            End Set
        End Property

        Public Property ImageMap() As String
            Get
                Return m_ImageMap
            End Get
            Set(ByVal Value As String)
                m_ImageMap = value
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

        Public Sub New(ByVal DB As Database, ByVal HomepageImageId As Integer)
            m_DB = DB
            m_HomepageImageId = HomepageImageId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:30 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_HOMEPAGE_GETOBJECT As String = "sp_Homepage_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_HOMEPAGE_GETOBJECT)
                db.AddInParameter(cmd, "HomepageImageId", DbType.Int32, HomepageImageId)
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
            'Date: September 25, 2009 01:20:30 PM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("HomepageImageId"))) Then
                    m_HomepageImageId = Convert.ToInt32(reader("HomepageImageId"))
                Else
                    m_HomepageImageId = 0
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
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:30 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_HOMEPAGE_INSERT As String = "sp_Homepage_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_HOMEPAGE_INSERT)

            db.AddOutParameter(cmd, "HomepageImageId", DbType.Int32, HomepageImageId)
            db.AddInParameter(cmd, "ImageName", DbType.String, ImageName)
            db.AddInParameter(cmd, "ImageMap", DbType.String, ImageMap)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)

            db.ExecuteNonQuery(cmd)

            HomepageImageId = Convert.ToInt32(db.GetParameterValue(cmd, "HomepageImageId"))

            '------------------------------------------------------------------------
            Return HomepageImageId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:30 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_HOMEPAGE_UPDATE As String = "sp_Homepage_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_HOMEPAGE_UPDATE)

            db.AddInParameter(cmd, "HomepageImageId", DbType.Int32, HomepageImageId)
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
            'Date: September 25, 2009 01:20:30 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_HOMEPAGE_DELETE As String = "sp_Homepage_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_HOMEPAGE_DELETE)

            db.AddInParameter(cmd, "HomepageImageId", DbType.Int32, HomepageImageId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

End Namespace


