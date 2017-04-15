Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility

Namespace DataLayer

    Public Class StoreOrderPayflowRow
        Inherits StoreOrderPayflowRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal PayflowId As Integer)
            MyBase.New(DB, PayflowId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal PayflowId As Integer) As StoreOrderPayflowRow
            Dim row As StoreOrderPayflowRow

            row = New StoreOrderPayflowRow(DB, PayflowId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderNo As String) As StoreOrderPayflowRow
            Dim row As StoreOrderPayflowRow

            Dim OrderId As Integer = DB.ExecuteScalar("select top 1 OrderId from StoreOrder where OrderNo = " & DB.Quote(OrderNo))

            row = New StoreOrderPayflowRow(DB)
            row.LoadByOrderID(OrderId)

            Return row
        End Function

        Public Shared Function GetRowByOrderId(ByVal DB As Database, ByVal OrderId As Integer) As StoreOrderPayflowRow
            Dim row As New StoreOrderPayflowRow(DB)
            row.LoadByOrderID(OrderId)

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal PayflowId As Integer)
            Dim row As StoreOrderPayflowRow

            row = New StoreOrderPayflowRow(DB, PayflowId)
            row.Remove()
        End Sub

        Public Shared Sub RemoveByOrderId(ByVal DB As Database, ByVal OrderId As Integer)
            Dim SQL As String = ""

            SQL = "DELETE FROM StoreOrderPayflow WHERE OrderId = " & DB.Number(OrderId)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Shared Function GetPaypalAddressUnmatch(ByVal OrderNo As String) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreOrderPayflow_GetPaypalAddressUnmatch")
                db.AddInParameter(cmd, "OrderNo", DbType.String, OrderNo)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                Return result.Tables(0)
            Catch ex As Exception

            End Try
            Return New DataTable
        End Function

        Public Shared Function InsertPaypalUnmatch(ByVal _Database As Database, ByVal data As StoreOrderPayflowRow) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_StoreOrderPayflow_InsertPaypalUnmatch"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, data.OrderId))
                cmd.Parameters.Add(_Database.InParam("RespMsg", SqlDbType.VarChar, 0, data.RespMsg))
                cmd.Parameters.Add(_Database.InParam("CreatedDate", SqlDbType.DateTime, 0, data.CreatedDate))
                cmd.Parameters.Add(_Database.InParam("PaypalShipToAddress", SqlDbType.NVarChar, 0, data.PaypalShipToAddress))
                'cmd.Parameters.Add(_Database.InParam("PaypalShipToAddress2", SqlDbType.NVarChar, 0, data.PaypalShipToAddress2))
                cmd.Parameters.Add(_Database.InParam("PaypalShipToCity", SqlDbType.NVarChar, 0, data.PaypalShipToCity))
                cmd.Parameters.Add(_Database.InParam("PaypalShipToCounty", SqlDbType.NVarChar, 0, data.PaypalShipToCounty))
                cmd.Parameters.Add(_Database.InParam("PaypalShipToZipcode", SqlDbType.VarChar, 0, data.PaypalShipToZipcode))
                cmd.Parameters.Add(_Database.InParam("PaypalShipToCountry", SqlDbType.VarChar, 0, data.PaypalShipToCountry))
                cmd.Parameters.Add(_Database.InParam("PaypalShipToStatus", SqlDbType.VarChar, 0, data.PaypalShipToStatus))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function CheckPaypalCountryCode(ByVal address_country_code As String, ByVal address_state As String)
            If address_country_code = "AU" Then
                address_country_code = "AS"
            ElseIf address_country_code = "GB" Then
                address_country_code = "UK"
            ElseIf address_country_code = "ES" Then
                address_country_code = "SP"
            ElseIf address_country_code = "LT" Then
                address_country_code = "BO"
            ElseIf address_country_code = "IE" Then
                address_country_code = "EI"
            ElseIf address_country_code = "US" And address_state = "PR" Then
                address_country_code = "PR"
            ElseIf address_country_code = "AT" Then
                address_country_code = "AU"
            ElseIf address_country_code = "SE" Then
                address_country_code = "SW"
            ElseIf address_country_code = "JP" Then
                address_country_code = "JA"
            ElseIf address_country_code = "DE" Then
                address_country_code = "GM"
            ElseIf address_country_code = "DM" Then
                address_country_code = "DO"
            ElseIf address_country_code = "DO" Then
                address_country_code = "DR"
            End If
            Return address_country_code
        End Function
    End Class

    Public MustInherit Class StoreOrderPayflowRowBase
        Private m_DB As Database
        Private m_PayflowId As Integer = Nothing
        Private m_OrderId As Integer = Nothing
        Private m_PnRef As String = Nothing
        Private m_Result As Integer = Nothing
        Private m_RespMsg As String = Nothing
        Private m_AvsAddr As String = Nothing
        Private m_AvsZip As String = Nothing
        Private m_Cvv2Match As String = Nothing
        Private m_CreatedDate As DateTime = Nothing
        ''paypal
        Private m_PaypalShipToAddress As String = Nothing
        Private m_PaypalShipToAddress2 As String = Nothing
        Private m_PaypalShipToCity As String = Nothing
        Private m_PaypalShipToCounty As String = Nothing
        Private m_PaypalShipToZipcode As String = Nothing
        Private m_PaypalShipToCountry As String = Nothing
        Private m_PaypalShipToStatus As String = Nothing


        Public Property PayflowId() As Integer
            Get
                Return m_PayflowId
            End Get
            Set(ByVal Value As Integer)
                m_PayflowId = Value
            End Set
        End Property

        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property

        Public Property PnRef() As String
            Get
                Return m_PnRef
            End Get
            Set(ByVal Value As String)
                m_PnRef = Value
            End Set
        End Property

        Public Property Result() As Integer
            Get
                Return m_Result
            End Get
            Set(ByVal Value As Integer)
                m_Result = Value
            End Set
        End Property

        Public Property RespMsg() As String
            Get
                Return m_RespMsg
            End Get
            Set(ByVal Value As String)
                m_RespMsg = Value
            End Set
        End Property

        Public Property AvsAddr() As String
            Get
                Return m_AvsAddr
            End Get
            Set(ByVal Value As String)
                m_AvsAddr = Value
            End Set
        End Property

        Public Property AvsZip() As String
            Get
                Return m_AvsZip
            End Get
            Set(ByVal Value As String)
                m_AvsZip = Value
            End Set
        End Property

        Public Property Cvv2Match() As String
            Get
                Return m_Cvv2Match
            End Get
            Set(ByVal Value As String)
                m_Cvv2Match = Value
            End Set
        End Property

        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreatedDate = Value
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

        ''paypal
        Public Property PaypalShipToAddress() As String
            Get
                Return m_PaypalShipToAddress
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToAddress = Value
            End Set
        End Property
        Public Property PaypalShipToAddress2() As String
            Get
                Return m_PaypalShipToAddress2
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToAddress2 = Value
            End Set
        End Property
        Public Property PaypalShipToCity() As String
            Get
                Return m_PaypalShipToCity
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToCity = Value
            End Set
        End Property
        Public Property PaypalShipToCounty() As String
            Get
                Return m_PaypalShipToCounty
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToCounty = Value
            End Set
        End Property
        Public Property PaypalShipToZipcode() As String
            Get
                Return m_PaypalShipToZipcode
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToZipcode = Value
            End Set
        End Property
        Public Property PaypalShipToCountry() As String
            Get
                Return m_PaypalShipToCountry
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToCountry = Value
            End Set
        End Property
        Public Property PaypalShipToStatus() As String
            Get
                Return m_PaypalShipToStatus
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToStatus = Value
            End Set
        End Property


        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal PayflowId As Integer)
            m_DB = DB
            m_PayflowId = PayflowId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreOrderPayflow WHERE PayflowId = " & DB.Number(PayflowId)
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

        Protected Overridable Sub LoadByOrderID(ByVal orderId As Integer)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT TOP 1 * FROM StoreOrderPayflow WHERE OrderId = " & DB.Quote(orderId) & "  ORDER BY CreatedDate DESC "
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
                m_PayflowId = Convert.ToInt32(r.Item("PayflowId"))
                If IsDBNull(r.Item("OrderId")) Then
                    m_OrderId = Nothing
                Else
                    m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                End If

                If IsDBNull(r.Item("PnRef")) Then
                    m_PnRef = Nothing
                Else
                    m_PnRef = Convert.ToString(r.Item("PnRef"))
                End If

                If IsDBNull(r.Item("Result")) Then
                    m_Result = Nothing
                Else
                    m_Result = Convert.ToString(r.Item("Result"))
                End If

                If IsDBNull(r.Item("RespMsg")) Then
                    m_RespMsg = Nothing
                Else
                    m_RespMsg = Convert.ToString(r.Item("RespMsg"))
                End If

                If IsDBNull(r.Item("AvsAddr")) Then
                    m_AvsAddr = Nothing
                Else
                    m_AvsAddr = Convert.ToString(r.Item("AvsAddr"))
                End If

                If IsDBNull(r.Item("AvsZip")) Then
                    m_AvsZip = Nothing
                Else
                    m_AvsZip = Convert.ToString(r.Item("AvsZip"))
                End If

                If IsDBNull(r.Item("Cvv2Match")) Then
                    m_Cvv2Match = Nothing
                Else
                    m_Cvv2Match = Convert.ToString(r.Item("Cvv2Match"))
                End If

                If IsDBNull(r.Item("PaypalShipToAddress")) Then
                    m_PaypalShipToAddress = Nothing
                Else
                    m_PaypalShipToAddress = Convert.ToString(r.Item("PaypalShipToAddress"))
                End If
                If IsDBNull(r.Item("PaypalShipToAddress2")) Then
                    m_PaypalShipToAddress2 = Nothing
                Else
                    m_PaypalShipToAddress2 = Convert.ToString(r.Item("PaypalShipToAddress"))
                End If
                If IsDBNull(r.Item("PaypalShipToCity")) Then
                    m_PaypalShipToCity = Nothing
                Else
                    m_PaypalShipToCity = Convert.ToString(r.Item("PaypalShipToCity"))
                End If
                If IsDBNull(r.Item("PaypalShipToCounty")) Then
                    m_PaypalShipToCounty = Nothing
                Else
                    m_PaypalShipToCounty = Convert.ToString(r.Item("PaypalShipToCounty"))
                End If
                If IsDBNull(r.Item("PaypalShipToZipcode")) Then
                    m_PaypalShipToZipcode = Nothing
                Else
                    m_PaypalShipToZipcode = Convert.ToString(r.Item("PaypalShipToZipcode"))
                End If
                If IsDBNull(r.Item("PaypalShipToCountry")) Then
                    m_PaypalShipToCountry = Nothing
                Else
                    m_PaypalShipToCountry = Convert.ToString(r.Item("PaypalShipToCountry"))
                End If
                If IsDBNull(r.Item("PaypalShipToStatus")) Then
                    m_PaypalShipToStatus = Nothing
                Else
                    m_PaypalShipToStatus = Convert.ToString(r.Item("PaypalShipToStatus"))
                End If
            Catch ex As Exception
                Throw ex

            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String = ""

            SQL = " INSERT INTO dbo.StoreOrderPayflow ("
            SQL &= " OrderId"
            SQL &= ",PnRef"
            SQL &= ",Result"
            SQL &= ",RespMsg"
            SQL &= ",AvsAddr"
            SQL &= ",AvsZip"
            SQL &= ",Cvv2Match" _
            & ",CreatedDate"
            SQL &= ") VALUES ("
            SQL &= m_DB.Number(OrderId.ToString())
            SQL &= "," & m_DB.Quote(PnRef)
            SQL &= "," & m_DB.Number(Result.ToString())
            SQL &= "," & m_DB.Quote(RespMsg)
            SQL &= "," & m_DB.Quote(AvsAddr)
            SQL &= "," & m_DB.Quote(AvsZip)
            SQL &= "," & m_DB.Quote(Cvv2Match) _
            & "," & m_DB.Quote(CreatedDate.ToString())
            SQL &= ")"

            PayflowId = m_DB.InsertSQL(SQL)

            Return PayflowId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE dbo.StoreOrderPayflow SET " _
             & " OrderId = " & m_DB.Number(OrderId) _
             & ",PnRef = " & m_DB.Quote(PnRef) _
             & ",Result = " & m_DB.Number(Result) _
             & ",RespMsg = " & m_DB.Quote(RespMsg) _
             & ",AvsAddr = " & m_DB.Quote(AvsAddr) _
             & ",AvsZip = " & m_DB.Quote(AvsZip) _
             & ",Cvv2Match = " & m_DB.Quote(Cvv2Match) _
             & " WHERE PayflowId = " & m_DB.Quote(PayflowId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String
            SQL = "DELETE FROM StoreOrderPayflow WHERE PayflowId = " & m_DB.Quote(PayflowId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove

    End Class


    Public Class StoreOrderPayflowCollection
        Inherits GenericCollection(Of StoreOrderPayflowRow)
    End Class

End Namespace