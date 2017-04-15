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

    Public Class StoreCouponRow
        Inherits StoreCouponRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CouponId As Integer)
            MyBase.New(DB, CouponId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CouponId As Integer) As StoreCouponRow
            Dim row As StoreCouponRow

            row = New StoreCouponRow(DB, CouponId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CouponId As Integer)
            Dim row As StoreCouponRow

            row = New StoreCouponRow(DB, CouponId)
            row.Remove()
        End Sub

        Public Shared Function GetList(ByVal DB1 As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
            '---------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_StoreCoupon_GetList"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            db.AddInParameter(cmd, "SortBy", DbType.String, SortBy)
            db.AddInParameter(cmd, "SortOrder", DbType.String, SortOrder)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '--------------------------------------------------------------------
        End Function
       
        'Custom Methods
        Public Shared Function GetRandomCoupon(ByVal DB As Database) As StoreCouponRow
            Dim dt As DateTime = Now
            Dim SQL As String = "select top 1 couponid from storecoupon where " & DB.Quote(dt) & " between coalesce(startdate," & DB.Quote(dt) & ") and coalesce(enddate," & DB.Quote(dt.AddDays(1)) & ") and isactive = 1 order by newid()"
            Return StoreCouponRow.GetRow(DB, DB.ExecuteScalar(SQL))
        End Function
    End Class

    Public MustInherit Class StoreCouponRowBase
        Private m_DB As Database
        Private m_CouponId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_Image As String = Nothing
        Private m_ReferralId As Integer = Nothing
        Private m_StartDate As DateTime = Nothing
        Private m_EndDate As DateTime = Nothing
        Private m_IsActive As Boolean = Nothing


        Public Property CouponId() As Integer
            Get
                Return m_CouponId
            End Get
            Set(ByVal Value As Integer)
                m_CouponId = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
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

        Public Property ReferralId() As Integer
            Get
                Return m_ReferralId
            End Get
            Set(ByVal Value As Integer)
                m_ReferralId = value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return m_StartDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartDate = value
            End Set
        End Property

        Public Property EndDate() As DateTime
            Get
                Return m_EndDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndDate = value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = value
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

        Public Sub New(ByVal DB As Database, ByVal CouponId As Integer)
            m_DB = DB
            m_CouponId = CouponId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_StoreCoupon_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "CouponId", DbType.Int32, CouponId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As sqlDataReader)
            m_CouponId = Convert.ToInt32(r.Item("CouponId"))
            m_Name = Convert.ToString(r.Item("Name"))
            m_Image = Convert.ToString(r.Item("Image"))
            m_ReferralId = Convert.ToInt32(r.Item("ReferralId"))
            If IsDBNull(r.Item("StartDate")) Then
                m_StartDate = Nothing
            Else
                m_StartDate = Convert.ToDateTime(r.Item("StartDate"))
            End If
            If IsDBNull(r.Item("EndDate")) Then
                m_EndDate = Nothing
            Else
                m_EndDate = Convert.ToDateTime(r.Item("EndDate"))
            End If
            m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_INSERT As String = "sp_StoreCoupon_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_INSERT)
            db.AddOutParameter(cmd, "CouponId", DbType.Int32, 16)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "ReferralId", DbType.String, ReferralId)
            db.AddInParameter(cmd, "StartDate", DbType.String, StartDate)
            db.AddInParameter(cmd, "EndDate", DbType.String, EndDate)
            db.AddInParameter(cmd, "IsActive", DbType.String, IsActive)
            db.ExecuteNonQuery(cmd)
            CouponId = Convert.ToInt32(db.GetParameterValue(cmd, "CouponId"))
            '----------------------------------------------------------------------
            Return CouponId
        End Function

        Public Overridable Sub Update()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_UPDATE As String = "sp_StoreCoupon_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)
            db.AddInParameter(cmd, "CouponId", DbType.Int32, CouponId)
            db.AddInParameter(cmd, "Name", DbType.Int32, Name)
            db.AddInParameter(cmd, "Image", DbType.String, Image)
            db.AddInParameter(cmd, "ReferralId", DbType.String, ReferralId)
            db.AddInParameter(cmd, "StartDate", DbType.String, StartDate)
            db.AddInParameter(cmd, "EndDate", DbType.String, EndDate)
            db.AddInParameter(cmd, "IsActive", DbType.String, IsActive)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_Coupon_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "CouponId", DbType.Int32, CouponId)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StoreCouponCollection
        Inherits GenericCollection(Of StoreCouponRow)
    End Class

End Namespace

