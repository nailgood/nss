
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

    Public Class ShippingFedExFeeRow
        Inherits ShippingFedExFeeBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("ShippingFedExFee.vb", func, ex)
        End Sub
        Public Shared Function Insert(ByVal objFee As ShippingFedExFeeRow) As Integer
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_ShippingFedExFee_Insert")
                db.AddInParameter(cmd, "MethodID", DbType.String, objFee.MethodID)
                db.AddInParameter(cmd, "ZipCode", DbType.String, objFee.ZipCode)
                db.AddInParameter(cmd, "IsDAS", DbType.Boolean, objFee.IsDAS)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If (result > 0) Then
                    Return True
                End If
            Catch ex As Exception
                SendMailLog("Insert", ex)
            End Try
            Return False
        End Function

    End Class

    Public MustInherit Class ShippingFedExFeeBase
        Private m_FedExFeeID As Integer = Nothing
        Private m_MethodID As Integer = Nothing
        Private m_ZipCode As String = String.Empty
        Private m_IsDAS As Boolean = False
        Private m_IsFuel As Boolean = False
        Public Property FedExFeeID() As Integer
            Get
                Return m_FedExFeeID
            End Get
            Set(ByVal Value As Integer)
                m_FedExFeeID = Value
            End Set
        End Property
        Public Property MethodID() As Integer
            Get
                Return m_MethodID
            End Get
            Set(ByVal Value As Integer)
                m_MethodID = Value
            End Set
        End Property
        Public Property ZipCode() As String
            Get
                Return m_ZipCode
            End Get
            Set(ByVal Value As String)
                m_ZipCode = Value
            End Set
        End Property

        Public Property IsDAS() As Boolean
            Get
                Return m_IsDAS
            End Get
            Set(ByVal Value As Boolean)
                m_IsDAS = Value
            End Set
        End Property
        Public Property IsFuel() As Boolean
            Get
                Return m_IsFuel
            End Get
            Set(ByVal Value As Boolean)
                m_IsFuel = Value
            End Set
        End Property
        Public Sub New()
        End Sub 'New

    End Class

    Public Class ShippingFedExFeeCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal objItem As ShippingFedExFeeRow)
            Me.List.Add(objItem)
        End Sub

        Public Function Contains(ByVal objItem As ShippingFedExFeeRow) As Boolean
            Return Me.List.Contains(objItem)
        End Function

        Public Function IndexOf(ByVal objItem As ShippingFedExFeeRow) As Integer
            Return Me.List.IndexOf(objItem)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal objItem As ShippingFedExFeeRow)
            Me.List.Insert(Index, objItem)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ShippingFedExFeeRow
            Get
                If Me.List.Item(Index) Is Nothing Then
                    Return Nothing
                End If
                Return CType(Me.List.Item(Index), ShippingFedExFeeRow)
            End Get

            Set(ByVal Value As ShippingFedExFeeRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal objItem As ShippingFedExFeeRow)
            Me.List.Remove(objItem)
        End Sub
    End Class

End Namespace


