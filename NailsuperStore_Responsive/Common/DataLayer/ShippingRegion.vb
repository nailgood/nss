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
Namespace DataLayer
    Public Class ShippingRegionRow
        Inherits ShippingRegionRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal RegionID As Integer)
            MyBase.New(DB, RegionID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal RegionID As Integer) As ShippingRegionRow
            Dim row As ShippingRegionRow

            row = New ShippingRegionRow(DB, RegionID)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal RegionID As Integer)
            Dim row As ShippingRegionRow

            row = New ShippingRegionRow(DB, RegionID)
            row.Remove()
        End Sub
        'Long add 23/10/2009
        Public Shared Function GetListAdminShippingRegion(ByVal Condition As String) As DataTable

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_AdminShippingRegion"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "Condition", DbType.String, Condition)

            Return db.ExecuteDataSet(cmd).Tables(0)

        End Function
        'end 23/10/2009
        'Custom Methods
        Public Shared Function GetAllShippingRegions(ByVal DB1 As Database) As DataTable
            '---------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_ShippingRegion_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)
            '--------------------------------------------------------------------
        End Function

        Public Shared Function GetListByRegionCode(ByVal RegionCode As String) As DataTable

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_ShippingRegion_GetListByRegionCode"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "RegionCode", DbType.String, RegionCode)

            Return db.ExecuteDataSet(cmd).Tables(0)

        End Function

    End Class

    Public MustInherit Class ShippingRegionRowBase
        Private m_DB As Database
        Private m_RegionID As Integer = Nothing
        Private m_RegionName As String = Nothing
        Private m_RegionCode As String = Nothing

        Public Property RegionID() As Integer
            Get
                Return m_RegionID
            End Get
            Set(ByVal Value As Integer)
                m_RegionID = Value
            End Set
        End Property

        Public Property RegionName() As String
            Get
                Return m_RegionName
            End Get
            Set(ByVal Value As String)
                m_RegionName = Value
            End Set
        End Property

        Public Property RegionCode() As String
            Get
                Return m_RegionCode
            End Get
            Set(ByVal Value As String)
                m_RegionCode = Value
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

        Public Sub New(ByVal DB As Database, ByVal RegionID As Integer)
            m_DB = DB
            m_RegionID = RegionID
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_ShippingRegion_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "RegionID", DbType.Int32, RegionID)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_RegionID = Convert.ToInt32(r.Item("RegionID"))
                m_RegionName = Convert.ToString(r.Item("RegionName"))
                m_RegionCode = Convert.ToString(r.Item("RegionCode"))
            Catch ex As Exception
                Throw ex
            End Try


        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_ShippingRegion_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddOutParameter(cmd, "RegionID", DbType.Int32, 16)
            db.AddInParameter(cmd, "RegionName", DbType.String, RegionName)
            db.AddInParameter(cmd, "RegionCode", DbType.String, RegionCode)

            db.ExecuteNonQuery(cmd)

            RegionID = Convert.ToInt32(db.GetParameterValue(cmd, "RegionID"))

            '----------------------------------------------------------------------

            Return RegionID
        End Function

        Public Overridable Sub Update()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_ShippingRegion_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddInParameter(cmd, "RegionID", DbType.Int32, RegionID)
            db.AddInParameter(cmd, "RegionName", DbType.String, RegionName)
            db.AddInParameter(cmd, "RegionCode", DbType.String, RegionCode)
            'db.AddInParameter(cmd, "Sort", DbType.Int32, Sort)

            db.ExecuteNonQuery(cmd)

            '----------------------------------------------------------------------

        End Sub 'Update      

        Public Sub Remove()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_ShippingRegion_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "RegionID", DbType.Int32, RegionID)

            db.ExecuteNonQuery(cmd)

            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ShippingRegionCollection
        Inherits GenericCollection(Of ShippingRegionRow)
    End Class

End Namespace

