Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 9:57:06 AM

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
Imports Utility
Imports System.Web.UI.WebControls

Namespace DataLayer

    Public Class CountryRow
        Inherits CountryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal CountryId As Integer)
            MyBase.New(DB, CountryId)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Country As String)
            MyBase.New(DB, Country)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal CountryId As Integer) As CountryRow
            Dim row As CountryRow

            row = New CountryRow(DB, CountryId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal Country As String) As CountryRow
            Dim row As CountryRow

            row = New CountryRow(DB, Country)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal CountryId As Integer)
            Dim row As CountryRow

            row = New CountryRow(DB, CountryId)
            row.Remove()
        End Sub

        Private Shared _cacheKey As String = "Country_"
        Public Shared Function GetCountries() As List(Of ListItem)
            Dim listItem As List(Of ListItem) = Nothing

            Dim key As String = _cacheKey & "GetCountries"
            listItem = CType(CacheUtils.GetCache(key), List(Of ListItem))

            If listItem Is Nothing Then
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Country_GetListAll")

                Dim reader As SqlDataReader = db.ExecuteReader(cmd)
                If reader.HasRows Then
                    listItem = New List(Of ListItem)()
                    While reader.Read()
                        listItem.Add(New Web.UI.WebControls.ListItem(reader("CountryName"), reader("CountryCode")))
                    End While
                End If
                CacheUtils.SetCache(key, listItem)
            End If
            Return listItem
        End Function
        ''Custom Methods
        'Public Shared Function GetCountries(ByVal _DB As Database) As DataSet
        '    '------------------------------------------------------------------------
        '    'Author: Lam Le
        '    'Date: October 26, 2009 09:57:06 AM
        '    '------------------------------------------------------------------------
        '    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

        '    Dim SP_COUNTRY_GETLIST As String = "sp_Country_GetListAll"

        '    Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_GETLIST)

        '    Return db.ExecuteDataSet(cmd)

        '    '------------------------------------------------------------------------
        'End Function

        Public Shared Function GetCountriesShipping(ByVal _DB As Database) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_COUNTRY_GETLIST As String = "sp_Country_GetListShipping"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_GETLIST)
            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetCountryByCountryCode(ByVal _DB As Database, ByVal CoutryCode As String) As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_COUNTRY_GETLIST As String = "sp_Country_GetCountryByCountryCode"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_GETLIST)
            db.AddInParameter(cmd, "CountryCode", DbType.String, CoutryCode)
            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function

    End Class

    Public MustInherit Class CountryRowBase
        Private m_DB As Database
        Private m_CountryId As Integer = Nothing
        Private m_CountryCode As String = Nothing
        Private m_CountryName As String = Nothing
        Private m_Shipping As Double = Nothing
        Private m_ShippingCode As String = Nothing
        Private m_IsShippingActive As Boolean = Nothing

        Public Property CountryId() As Integer
            Get
                Return m_CountryId
            End Get
            Set(ByVal Value As Integer)
                m_CountryId = Value
            End Set
        End Property

        Public Property CountryCode() As String
            Get
                Return m_CountryCode
            End Get
            Set(ByVal Value As String)
                m_CountryCode = Value
            End Set
        End Property

        Public Property CountryName() As String
            Get
                Return m_CountryName
            End Get
            Set(ByVal Value As String)
                m_CountryName = Value
            End Set
        End Property

        Public Property Shipping() As Double
            Get
                Return m_Shipping
            End Get
            Set(ByVal Value As Double)
                m_Shipping = Value
            End Set
        End Property

        Public Property ShippingCode() As String
            Get
                Return m_ShippingCode
            End Get
            Set(ByVal Value As String)
                m_ShippingCode = Value
            End Set
        End Property

        Public Property IsShippingActive() As Boolean
            Get
                Return m_IsShippingActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsShippingActive = Value
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

        Public Sub New(ByVal DB As Database, ByVal CountryId As Integer)
            m_DB = DB
            m_CountryId = CountryId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Country As String)
            m_DB = DB
            m_CountryCode = Country
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_COUNTRY_GETOBJECT As String = "sp_Country_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_GETOBJECT)
                db.AddInParameter(cmd, "CountryCode", DbType.String, CountryCode)
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
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("CountryId"))) Then
                    m_CountryId = Convert.ToInt32(reader("CountryId"))
                Else
                    m_CountryId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CountryCode"))) Then
                    m_CountryCode = reader("CountryCode").ToString()
                Else
                    m_CountryCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CountryName"))) Then
                    m_CountryName = reader("CountryName").ToString()
                Else
                    m_CountryName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Shipping"))) Then
                    m_Shipping = Convert.ToDouble(reader("Shipping"))
                Else
                    m_Shipping = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ShippingCode"))) Then
                    m_ShippingCode = reader("ShippingCode").ToString()
                Else
                    m_ShippingCode = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsShippingActive"))) Then
                    m_IsShippingActive = Convert.ToBoolean(reader("IsShippingActive"))
                Else
                    m_IsShippingActive = False
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_COUNTRY_INSERT As String = "sp_Country_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_INSERT)
            db.AddOutParameter(cmd, "CountryId", DbType.Int32, 32)
            db.AddInParameter(cmd, "CountryCode", DbType.String, CountryCode)
            db.AddInParameter(cmd, "CountryName", DbType.String, CountryName)
            db.AddInParameter(cmd, "Shipping", DbType.Double, Shipping)
            db.AddInParameter(cmd, "ShippingCode", DbType.String, ShippingCode)
            db.AddInParameter(cmd, "IsShippingActive", DbType.Boolean, IsShippingActive)
            db.ExecuteNonQuery(cmd)
            CountryId = Convert.ToInt32(db.GetParameterValue(cmd, "CountryId"))
            '------------------------------------------------------------------------
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_COUNTRY_UPDATE As String = "sp_Country_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_UPDATE)
            db.AddInParameter(cmd, "CountryId", DbType.Int32, CountryId)
            db.AddInParameter(cmd, "CountryCode", DbType.String, CountryCode)
            db.AddInParameter(cmd, "CountryName", DbType.String, CountryName)
            db.AddInParameter(cmd, "Shipping", DbType.Double, Shipping)
            db.AddInParameter(cmd, "ShippingCode", DbType.String, ShippingCode)
            db.AddInParameter(cmd, "IsShippingActive", DbType.Boolean, IsShippingActive)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 09:57:06 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_COUNTRY_DELETE As String = "sp_Country_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_COUNTRY_DELETE)
            db.AddInParameter(cmd, "CountryId", DbType.Int32, CountryId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class CountryCollection
        Inherits GenericCollection(Of CountryRow)
    End Class

End Namespace


