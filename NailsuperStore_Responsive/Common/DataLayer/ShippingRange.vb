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

    Public Class ShippingRangeRow
        Inherits ShippingRangeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ShippingRangeId As Integer)
            MyBase.New(DB, ShippingRangeId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ShippingRangeId As Integer) As ShippingRangeRow
            Dim row As ShippingRangeRow

            row = New ShippingRangeRow(DB, ShippingRangeId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal MethodId As Integer, ByVal LowValue As String, ByVal HighValue As String) As ShippingRangeRow
            Return GetRow(DB, DB.ExecuteScalar("select top 1 shippingrangeid from shippingrange where methodid = " & DB.Number(MethodId) & " and lowvalue = " & DB.Quote(LowValue) & " and highvalue = " & DB.Quote(HighValue)))
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ShippingRangeId As Integer)
            Dim row As ShippingRangeRow

            row = New ShippingRangeRow(DB, ShippingRangeId)
            row.Remove()
        End Sub
       
        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionShippingRatesRow)
            Dim MethodId As Integer = DB.ExecuteScalar("select top 1 methodid from shipmentmethod where methodid = " & MethodId)
            If MethodId = Nothing Then Exit Sub

            LowValue = r.Low_Value
            HighValue = r.High_Value
            OverUnderValue = r.Over_Under_Value
            FirstPoundOver = r.First_Pound_Over
            FirstPoundUnder = r.First_Pound_Under
            AdditionalPound = r.Additional_Pound
            AdditionalThreshold = r.Additional_Threshold

            If ShippingRangeId = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub

        'Public Shared Function GetShippingCode(ByVal _DB As Database, ByVal strCountryCode As String) As DataSet
        '    Return _DB.GetDataSet("select * from Country, ShippingRange where Country.countryid = ShippingRange.countryid and isshippingactive=1 and CountryCode='" & strCountryCode & "'")
        'End Function

        Public Shared Function GetShippingCode(ByVal _DB As Database, ByVal strCountryCode As String) As String
            Dim ShippingCode As String = _DB.ExecuteScalar("SELECT ShippingCode FROM Country WHERE IsShippingActive=1 and CountryCode='" & strCountryCode & "'")
            Return ShippingCode
        End Function

        Public Shared Function GetShippingRangeId(ByVal _DB As Database, ByVal strCountryCode As String) As String
            Dim ShippingRangeId As Integer = 0
            Try
                ShippingRangeId = CInt(_DB.ExecuteScalar("SELECT ShippingRangeId FROM Country, ShippingRange where Country.countryid = ShippingRange.countryid and IsShippingActive=1 and CountryCode='" & strCountryCode & "'"))
            Catch ex As Exception
            End Try

            Return ShippingRangeId
        End Function
    End Class

    Public MustInherit Class ShippingRangeRowBase
        Private m_DB As Database
        Private m_ShippingRangeId As Integer = Nothing
        Private m_MethodId As Integer = Nothing
        Private m_LowValue As String = Nothing
        Private m_HighValue As String = Nothing
        Private m_OverUnderValue As Double = Nothing
        Private m_FirstPoundOver As Double = Nothing
        Private m_FirstPoundUnder As Double = Nothing
        Private m_AdditionalPound As Double = Nothing
        Private m_AdditionalThreshold As Double = Nothing
        Private m_CountryId As Integer = Nothing
        Private m_RegionId As Integer = Nothing

        Public Property ShippingRangeId() As Integer
            Get
                Return m_ShippingRangeId
            End Get
            Set(ByVal Value As Integer)
                m_ShippingRangeId = Value
            End Set
        End Property

        Public Property MethodId() As Integer
            Get
                Return m_MethodId
            End Get
            Set(ByVal Value As Integer)
                m_MethodId = Value
            End Set
        End Property

        Public Property LowValue() As String
            Get
                Return m_LowValue
            End Get
            Set(ByVal Value As String)
                m_LowValue = Value
            End Set
        End Property

        Public Property HighValue() As String
            Get
                Return m_HighValue
            End Get
            Set(ByVal Value As String)
                m_HighValue = Value
            End Set
        End Property

        Public Property OverUnderValue() As Double
            Get
                Return m_OverUnderValue
            End Get
            Set(ByVal Value As Double)
                m_OverUnderValue = Value
            End Set
        End Property

        Public Property FirstPoundOver() As Double
            Get
                Return m_FirstPoundOver
            End Get
            Set(ByVal Value As Double)
                m_FirstPoundOver = Value
            End Set
        End Property

        Public Property FirstPoundUnder() As Double
            Get
                Return m_FirstPoundUnder
            End Get
            Set(ByVal Value As Double)
                m_FirstPoundUnder = Value
            End Set
        End Property

        Public Property AdditionalPound() As Double
            Get
                Return m_AdditionalPound
            End Get
            Set(ByVal Value As Double)
                m_AdditionalPound = Value
            End Set
        End Property

        Public Property AdditionalThreshold() As Double
            Get
                Return m_AdditionalThreshold
            End Get
            Set(ByVal Value As Double)
                m_AdditionalThreshold = Value
            End Set
        End Property

        Public Property CountryId() As Integer
            Get
                Return m_CountryId
            End Get
            Set(ByVal Value As Integer)
                m_CountryId = Value
            End Set
        End Property

        Public Property RegionId() As Integer
            Get
                Return m_RegionId
            End Get
            Set(ByVal Value As Integer)
                m_RegionId = Value
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

        Public Sub New(ByVal DB As Database, ByVal ShippingRangeId As Integer)
            m_DB = DB
            m_ShippingRangeId = ShippingRangeId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM ShippingRange WHERE ShippingRangeId = " & DB.Number(ShippingRangeId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_ShippingRangeId = Convert.ToInt32(r.Item("ShippingRangeId"))
                m_MethodId = Convert.ToInt32(r.Item("MethodId"))
                m_LowValue = Convert.ToString(r.Item("LowValue"))
                m_HighValue = Convert.ToString(r.Item("HighValue"))
                m_OverUnderValue = Convert.ToDouble(r.Item("OverUnderValue"))
                m_FirstPoundOver = Convert.ToDouble(r.Item("FirstPoundOver"))
                m_FirstPoundUnder = Convert.ToDouble(r.Item("FirstPoundUnder"))
                m_AdditionalPound = Convert.ToDouble(r.Item("AdditionalPound"))
                m_AdditionalThreshold = Convert.ToDouble(r.Item("AdditionalThreshold"))

                If (Not r.IsDBNull(r.GetOrdinal("CountryId"))) Then
                    m_CountryId = Convert.ToInt32(r("CountryId"))
                Else
                    m_CountryId = 0
                End If

                If (Not r.IsDBNull(r.GetOrdinal("RegionId"))) Then
                    m_RegionId = Convert.ToInt32(r("RegionId"))
                Else
                    m_RegionId = 0
                End If


            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO ShippingRange (" _
             & " MethodId" _
             & ",LowValue" _
             & ",HighValue" _
             & ",OverUnderValue" _
             & ",FirstPoundOver" _
             & ",FirstPoundUnder" _
             & ",AdditionalPound" _
             & ",AdditionalThreshold" _
             & ",CountryId" _
             & ",RegionId" _
             & ") VALUES (" _
             & m_DB.NullNumber(MethodId) _
             & "," & m_DB.Quote(LowValue) _
             & "," & m_DB.Quote(HighValue) _
             & "," & m_DB.Number(OverUnderValue) _
             & "," & m_DB.Number(FirstPoundOver) _
             & "," & m_DB.Number(FirstPoundUnder) _
             & "," & m_DB.Number(AdditionalPound) _
             & "," & m_DB.Number(AdditionalThreshold) _
             & "," & m_DB.Number(CountryId) _
             & "," & m_DB.Number(RegionId) _
             & ")"

            ShippingRangeId = m_DB.InsertSQL(SQL)

            Return ShippingRangeId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE ShippingRange SET " _
             & " MethodId = " & m_DB.NullNumber(MethodId) _
             & ",LowValue = " & m_DB.Quote(LowValue) _
             & ",HighValue = " & m_DB.Quote(HighValue) _
             & ",OverUnderValue = " & m_DB.Number(OverUnderValue) _
             & ",FirstPoundOver = " & m_DB.Number(FirstPoundOver) _
             & ",FirstPoundUnder = " & m_DB.Number(FirstPoundUnder) _
             & ",AdditionalPound = " & m_DB.Number(AdditionalPound) _
             & ",AdditionalThreshold = " & m_DB.Number(AdditionalThreshold) _
             & ",CountryId = " & m_DB.Number(CountryId) _
             & ",RegionId = " & m_DB.Number(RegionId) _
             & " WHERE ShippingRangeId = " & m_DB.Quote(ShippingRangeId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM ShippingRange WHERE ShippingRangeId = " & m_DB.Quote(ShippingRangeId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ShippingRangeCollection
        Inherits GenericCollection(Of ShippingRangeRow)
    End Class

End Namespace


