Imports System
Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility
Namespace DataLayer
    Public Class FeeShippingStateRow
        Inherits FeeShippingStateRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Integer) As FeeShippingStateRow
            Dim row As FeeShippingStateRow
            row = New FeeShippingStateRow(_Database, id)
            row.Load()
            Return row
        End Function

        Public Shared Function ListByItem(ByVal itemId As String) As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STATE_GETLIST As String = "sp_FeeShippingState_ListByItemId"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_GETLIST)
            db.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)
            Return db.ExecuteDataSet(cmd)


        End Function
        Public Shared Function ListByItemState(ByVal itemId As String, ByVal stateCode As String) As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FeeShippingState_ListByItemState")
            db.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)
            db.AddInParameter(cmd, "StateCode", DbType.String, stateCode)
            Return db.ExecuteDataSet(cmd)


        End Function
        Public Shared Function GetDetailByStateCode(ByVal DB As Database, ByVal itemId As Integer, ByVal stateCode As String) As FeeShippingStateRow
            Dim r As SqlDataReader = Nothing
            Try
                Dim result As New FeeShippingStateRow()
                Dim SQL As String
                SQL = "SELECT * FROM FeeShippingState WHERE ItemId = " & itemId & " and StateCode='" & stateCode & "'"

                r = DB.GetReader(SQL)
                If r.Read Then
                    result = GetDataFromReader(r)
                End If
                Core.CloseReader(r)
                Return result
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return New FeeShippingStateRow()
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As FeeShippingStateRow
            Dim result As New FeeShippingStateRow
            If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                result.Id = Convert.ToInt32(reader("Id"))
            Else
                result.Id = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                result.ItemId = Convert.ToInt32(reader("ItemId"))
            Else
                result.ItemId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("StateCode"))) Then
                result.StateCode = reader("StateCode").ToString()
            Else
                result.StateCode = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("FirstItemFeeShipping"))) Then
                result.FirstItemFeeShipping = Convert.ToDouble(reader("FirstItemFeeShipping"))
            Else
                result.FirstItemFeeShipping = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NextItemFeeShipping"))) Then
                result.NextItemFeeShipping = Convert.ToInt32(reader("NextItemFeeShipping"))
            Else
                result.NextItemFeeShipping = 0
            End If
            Return result
        End Function

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FeeShippingState_Delete")
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function Insert(ByVal data As FeeShippingStateRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FeeShippingState_Insert")
                db.AddInParameter(cmd, "ItemId", DbType.Int32, data.ItemId)
                db.AddInParameter(cmd, "StateCode", DbType.String, data.StateCode)
                db.AddInParameter(cmd, "FirstItemFeeShipping", DbType.Double, data.FirstItemFeeShipping)
                db.AddInParameter(cmd, "NextItemFeeShipping", DbType.Double, data.NextItemFeeShipping)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function Update(ByVal data As FeeShippingStateRow) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FeeShippingState_Update")
                db.AddInParameter(cmd, "Id", DbType.Int32, data.Id)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, data.ItemId)
                db.AddInParameter(cmd, "StateCode", DbType.String, data.StateCode)
                db.AddInParameter(cmd, "FirstItemFeeShipping", DbType.Double, data.FirstItemFeeShipping)
                db.AddInParameter(cmd, "NextItemFeeShipping", DbType.Double, data.NextItemFeeShipping)
                Dim outPara As New SqlParameter("returnVal", SqlDbType.Int)
                outPara.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(outPara)
                db.ExecuteNonQuery(cmd)
                result = CInt(cmd.Parameters("returnVal").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
    End Class


    Public MustInherit Class FeeShippingStateRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_StateCode As String = Nothing
        Private m_FirstItemFeeShipping As Double = Nothing
        Private m_NextItemFeeShipping As Double = Nothing
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Property StateCode() As String
            Get
                Return m_StateCode
            End Get
            Set(ByVal Value As String)
                m_StateCode = Value
            End Set
        End Property
        Public Property FirstItemFeeShipping() As Double
            Get
                Return m_FirstItemFeeShipping
            End Get
            Set(ByVal Value As Double)
                m_FirstItemFeeShipping = Value
            End Set
        End Property
        Public Property NextItemFeeShipping() As Double
            Get
                Return m_NextItemFeeShipping
            End Get
            Set(ByVal Value As Double)
                m_NextItemFeeShipping = Value
            End Set
        End Property
        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM FeeShippingState WHERE Id = " & m_DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then

                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                    m_ItemId = Convert.ToInt32(reader("ItemId"))
                Else
                    m_ItemId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("StateCode"))) Then
                    m_StateCode = reader("StateCode").ToString()
                Else
                    m_StateCode = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("FirstItemFeeShipping"))) Then
                    m_FirstItemFeeShipping = Convert.ToDouble(reader("FirstItemFeeShipping"))
                Else
                    m_FirstItemFeeShipping = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NextItemFeeShipping"))) Then
                    m_NextItemFeeShipping = Convert.ToInt32(reader("NextItemFeeShipping"))
                Else
                    m_NextItemFeeShipping = 0
                End If
            End If
        End Sub

    End Class

    Public Class FeeShippingStateCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As FeeShippingStateRow)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As FeeShippingStateRow
            Get
                Return CType(Me.List.Item(Index), FeeShippingStateRow)
            End Get

            Set(ByVal Value As FeeShippingStateRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


