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
Imports Database

Namespace DataLayer
    Public Class SaveCartRow
        Inherits SaveCartRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer)
            MyBase.New(database, ItemId, MemberId)
        End Sub 'New

        Public Shared Function ListByMemberId(ByVal MemberId As Integer) As List(Of SaveCartRow)
            Dim result As New List(Of SaveCartRow)
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase
                Dim sp As String = "sp_SaveCart_ListByMemberId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    result = mapList(Of SaveCartRow)(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "SaveCart.vb > ListByMemberId", "MemberId: " & MemberId & "<br><br>Exception: " & ex.ToString())
            End Try

            Return result
        End Function

        Public Shared Function Insert(ByVal data As SaveCartRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_SaveCart_Insert"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, data.ItemId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, data.MemberId)
                db.AddInParameter(cmd, "Type", DbType.String, data.Type)
                db.AddInParameter(cmd, "Qty", DbType.Int32, data.Qty)
                db.AddInParameter(cmd, "CreatedDate", DbType.String, DateTime.Now)
                result = db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Email.SendError("ToError500", "SaveCart.vb > Insert", DateTime.Now.ToString() & "<br>MemberId: " & data.MemberId & "<br>ItemId: " & data.ItemId & "<br>Exception" & ex.ToString())
            End Try

            If result > 0 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & data.MemberId & "_")
                Return True
            End If
            Return False
        End Function

        Public Shared Function Delete(ByVal ItemId As Integer, ByVal MemberId As Integer, Optional ByVal isCase As Boolean = False) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_SaveCart_Delete"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                db.AddInParameter(cmd, "IsCase", DbType.Boolean, isCase)
                result = db.ExecuteNonQuery(cmd)
            Catch ex As Exception
                Email.SendError("ToError500", "SaveCart.vb > Delete", "MemberId: " & MemberId & "<br>ItemId: " & ItemId & "<br>Exception" & ex.ToString())
            End Try

            If result > 0 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & MemberId & "_")
                Return True
            End If
            Return False
        End Function

        Public Shared Function CountSaveCart(ByVal MemberId As Integer) As Integer
            Dim result As Integer = 0
            If MemberId < 1 Then
                Return result
            End If


            Dim sql As String = "SELECT COUNT(ItemId) FROM SaveCart WHERE MemberId=" & MemberId.ToString()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                result = CInt(db.ExecuteScalar(CommandType.Text, sql))
            Catch ex As Exception
                Email.SendError("ToError500", "SaveCart.vb > CountSaveCart", "MemberId: " & MemberId & "<br>Exception" & ex.ToString())
            End Try
            Return result
        End Function

    End Class


    Public MustInherit Class SaveCartRowBase
        Private m_DB As Database
        Private m_ItemId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_Qty As Integer = 0
        Private m_CreatedDate As DateTime = Nothing
        Private m_Type As String = String.Empty
        Private m_Condition As String = Nothing
        Private m_TotalRow As Integer = Nothing
        Public Shared cachePrefixKey As String = "SaveCart_"

        Public m_CasePriceCart As Double
        Public Property CasePriceCart() As Double
            Get
                Return m_CasePriceCart
            End Get
            Set(ByVal value As Double)
                m_CasePriceCart = value
            End Set
        End Property

        Public m_CasePriceCartNotSale As Double
        Public Property CasePriceCartNotSale() As Double
            Get
                Return m_CasePriceCartNotSale
            End Get
            Set(ByVal value As Double)
                m_CasePriceCartNotSale = value
            End Set
        End Property

        Public m_ItemPriceNotSale As Double
        Public Property ItemPriceNotSale() As Double
            Get
                Return m_ItemPriceNotSale
            End Get
            Set(ByVal value As Double)
                m_ItemPriceNotSale = value
            End Set
        End Property

        Public m_ItemPriceSale As Double
        Public Property ItemPriceSale() As Double
            Get
                Return m_ItemPriceSale
            End Get
            Set(ByVal value As Double)
                m_ItemPriceSale = value
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
        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
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
        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal Value As String)
                m_Type = Value
            End Set
        End Property

        Public Property Qty() As Integer
            Get
                Return m_Qty
            End Get
            Set(ByVal Value As Integer)
                m_Qty = Value
            End Set
        End Property


        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
            End Set
        End Property
        Public Property TotalRow() As String
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As String)
                m_TotalRow = Value
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

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ItemId As Integer, ByVal MemberId As Integer)
            m_DB = database
            m_ItemId = ItemId
            m_MemberId = MemberId
        End Sub 'New

        Protected Overridable Sub Load()
            If (ItemId < 1 And MemberId < 1) Then
                Exit Sub
            End If

            Dim dr As SqlDataReader = Nothing
            Dim SQL As String = String.Empty
            Try
                SQL = "SELECT * FROM SaveCart WHERE ItemId = " & DB.Number(ItemId) & " AND MemberId = " & DB.Number(MemberId)
                dr = m_DB.GetReader(SQL)
                If dr IsNot Nothing AndAlso dr.Read Then
                    Me.Load(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "SaveCart.vb", "SQL: " & SQL & "<br><br>Function: Protected Overridable Sub Load()" & "<br><br>Exception: " & ex.ToString())
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt32(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                        m_MemberId = Convert.ToInt32(reader("MemberId"))
                    Else
                        m_MemberId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                        m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                    Else
                        m_CreatedDate = DateTime.MinValue
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Qty"))) Then
                        m_Qty = Convert.ToInt32(reader("Qty"))
                    Else
                        m_Qty = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                        m_Type = reader("Type").ToString()
                    Else
                        m_Type = String.Empty
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

    End Class

    Public Class SaveCartCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal SaveCart As SaveCartRow)
            Me.List.Add(SaveCart)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As SaveCartRow
            Get
                Return CType(Me.List.Item(Index), SaveCartRow)
            End Get

            Set(ByVal Value As SaveCartRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As SaveCartCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New SaveCartCollection
                For Each obj In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

