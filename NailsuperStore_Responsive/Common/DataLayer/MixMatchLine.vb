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
Imports Components.Core
Imports Utility


Namespace DataLayer

    Public Class MixMatchLineRow
        Inherits MixMatchLineRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MixMatchId As Integer, ByVal LineNo As Integer)
            MyBase.New(DB, MixMatchId, LineNo)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal MixMatchNo As String, ByVal LineNo As Integer) As MixMatchLineRow
            Dim row As MixMatchLineRow

            Dim MixMatchId As Integer = DB.ExecuteScalar("select top 1 Id from MixMatch where MixMatchNo = " & DB.Quote(MixMatchNo))
            If MixMatchId = Nothing Then Return New MixMatchLineRow(DB)

            row = New MixMatchLineRow(DB, MixMatchId, LineNo)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As MixMatchLineRow
            Dim row As MixMatchLineRow

            row = New MixMatchLineRow(DB, Id)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MixMatchNo As String, ByVal LineNo As Integer)
            Dim row As MixMatchLineRow

            row = New MixMatchLineRow(DB, MixMatchNo, LineNo)
            row.Remove()
        End Sub

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
            Dim row As MixMatchLineRow

            row = New MixMatchLineRow(DB, Id)
            row.Remove()
        End Sub
       
        'Custom Methods
        
        Public Shared Function InsertLine(ByVal DB As Database, ByVal data As MixMatchLineRow) As Int32
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_MixMatchLine_Insert"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("MixMatchId", SqlDbType.Int, 0, data.MixMatchId))
                cmd.Parameters.Add(DB.InParam("LineNo", SqlDbType.Int, 0, data.LineNo))
                cmd.Parameters.Add(DB.InParam("ItemId", SqlDbType.Int, 0, data.ItemId))
                cmd.Parameters.Add(DB.InParam("DiscountType", SqlDbType.VarChar, 0, data.DiscountType))
                cmd.Parameters.Add(DB.InParam("Value", SqlDbType.Float, 0, data.Value))
                cmd.Parameters.Add(DB.InParam("DefaultQty", SqlDbType.Float, 0, data.DefaultSelectQty))
                cmd.Parameters.Add(DB.InParam("IsDefault", SqlDbType.Float, 0, data.IsDefaultSelect))
                If data.IsActive Then
                    cmd.Parameters.Add(DB.InParam("IsActive", SqlDbType.Bit, 0, 1))
                Else
                    cmd.Parameters.Add(DB.InParam("IsActive", SqlDbType.Bit, 0, 0))
                End If
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If (result = 1) Then
                    Dim URLCode As String = DB.ExecuteScalar("select URLCode from StoreItem WHERE ItemId = " & data.ItemId)
                    CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)

                End If
            Catch ex As Exception
                Components.Core.LogError("MixMatchLine.vb", String.Empty, ex)
            End Try
            Return result
        End Function
        Public Shared Function UpdateLine(ByVal DB As Database, ByVal data As MixMatchLineRow) As Int32
            Dim result As Integer = 0
            Try
                Dim ItemOldId As String = DB.ExecuteScalar("select ItemId from MixMatchLine WHERE MixMatchId = " & data.MixMatchId & " and Id = " & data.Id)
                Dim sp As String = "sp_MixMatchLine_Update"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("Id", SqlDbType.Int, 0, data.Id))
                cmd.Parameters.Add(DB.InParam("MixMatchId", SqlDbType.Int, 0, data.MixMatchId))
                cmd.Parameters.Add(DB.InParam("LineNo", SqlDbType.Int, 0, data.LineNo))
                cmd.Parameters.Add(DB.InParam("ItemId", SqlDbType.Int, 0, data.ItemId))
                cmd.Parameters.Add(DB.InParam("DiscountType", SqlDbType.VarChar, 0, data.DiscountType))
                cmd.Parameters.Add(DB.InParam("Value", SqlDbType.Float, 0, data.Value))
                cmd.Parameters.Add(DB.InParam("DefaultQty", SqlDbType.Float, 0, data.DefaultSelectQty))
                cmd.Parameters.Add(DB.InParam("IsDefault", SqlDbType.Float, 0, data.IsDefaultSelect))

                If data.IsActive Then
                    cmd.Parameters.Add(DB.InParam("IsActive", SqlDbType.Bit, 0, 1))
                Else
                    cmd.Parameters.Add(DB.InParam("IsActive", SqlDbType.Bit, 0, 0))
                End If
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If (result = 1) Then
                         CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
                End If
            Catch ex As Exception
                Components.Core.LogError("MixMatchLine.vb", String.Empty, ex)
            End Try
            Return result
        End Function
    End Class

    Public MustInherit Class MixMatchLineRowBase
        Private m_DB As Database
        Private m_LineNo As Integer = Nothing
        Private m_MixMatchId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_DiscountType As String = Nothing
        Private m_Value As Double = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_Id As Integer = Nothing
        Private m_DefaultSelectQty As Integer = Nothing
        Private m_IsDefaultSelect As Boolean = Nothing
        Public Property LineNo() As Integer
            Get
                Return m_LineNo
            End Get
            Set(ByVal Value As Integer)
                m_LineNo = Value
            End Set
        End Property

        Public Property MixMatchId() As Integer
            Get
                Return m_MixMatchId
            End Get
            Set(ByVal Value As Integer)
                m_MixMatchId = Value
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

        Public Property DiscountType() As String
            Get
                Return m_DiscountType
            End Get
            Set(ByVal Value As String)
                m_DiscountType = Value
            End Set
        End Property

        Public Property Value() As Double
            Get
                Return m_Value
            End Get
            Set(ByVal Value As Double)
                m_Value = Value
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

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property
        Public Property DefaultSelectQty() As Integer
            Get
                Return m_DefaultSelectQty
            End Get
            Set(ByVal Value As Integer)
                m_DefaultSelectQty = Value
            End Set
        End Property
        Public Property IsDefaultSelect() As Boolean
            Get
                Return m_IsDefaultSelect
            End Get
            Set(ByVal Value As Boolean)
                m_IsDefaultSelect = Value
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

        Public Sub New(ByVal DB As Database, ByVal MixMatchId As Integer, ByVal LineNo As Integer)
            m_DB = DB
            m_LineNo = LineNo
            m_MixMatchId = MixMatchId
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            m_DB = DB
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM MixMatchLine WHERE " & IIf(Id <> Nothing, "Id = " & Id, "MixMatchId = " & DB.Quote(MixMatchId) & " and [LineNo] = " & DB.Number(LineNo))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_LineNo = Convert.ToInt32(r.Item("LineNo"))
            m_MixMatchId = Convert.ToInt32(r.Item("MixMatchId"))
            m_ItemId = Convert.ToInt32(r.Item("ItemId"))
            m_DiscountType = Convert.ToString(r.Item("DiscountType"))
            m_Value = Convert.ToDouble(r.Item("Value"))
            m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
            m_Id = Convert.ToInt32(r.Item("Id"))
            m_DefaultSelectQty = Convert.ToInt32(r.Item("DefaultSelectQty"))
        End Sub 'Load

     
        Public Sub Remove()
            Dim SQL As String
            Dim ItemId As String = m_DB.ExecuteScalar("select ItemId from MixMatchLine where " & IIf(Id <> Nothing, "Id = " & Id, "MixMatchId = " & DB.Quote(MixMatchId) & " and [LineNo] = " & m_DB.Quote(LineNo)))


            SQL = "DELETE FROM MixMatchLine WHERE " & IIf(Id <> Nothing, "Id = " & Id, "MixMatchId = " & DB.Quote(MixMatchId) & " and [LineNo] = " & m_DB.Quote(LineNo))
            m_DB.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
        End Sub 'Remove
    End Class

    Public Class MixMatchLineCollection
        Inherits GenericCollection(Of MixMatchLineRow)
    End Class

End Namespace


