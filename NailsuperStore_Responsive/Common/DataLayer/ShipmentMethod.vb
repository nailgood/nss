Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Imports Utility
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility.Common

Namespace DataLayer

    Public Class ShipmentMethod
        Public Shared Function GetValue(ByVal methodId As Integer, ByVal type As ShipmentValue) As String
            Dim result As String = Nothing
            Dim dr As SqlDataReader = Nothing
            Dim paramName As String = String.Format("{0}|{1}", methodId, type.ToString().ToLower())

            Try
                Dim key As String = String.Format("ShipmentMethod_ListAll")
                Dim ht As New Hashtable()
                ht = CType(CacheUtils.GetCache(key), Hashtable)
                If Not ht Is Nothing Then
                    If ht.ContainsKey(paramName) Then
                        result = ht.Item(paramName).ToString()
                    End If
                Else
                    ht = New Hashtable()
                    dr = ShipmentMethodRow.GetListAll()
                    While dr.Read()

                        Dim Name As String = dr("Name").ToString().ToLower()
                        ht.Add(Name, dr("Value"))
                        If Name = paramName.Trim().ToLower() Then
                            result = dr("Value").ToString()
                        End If

                    End While
                    Core.CloseReader(dr)

                    CacheUtils.SetCache(key, ht)
                End If

            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return result
        End Function
    End Class

    Public Class ShipmentMethodRow
        Inherits ShipmentMethodRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MethodId As Integer)
            MyBase.New(DB, MethodId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal MethodId As Integer) As ShipmentMethodRow
            Dim result As New ShipmentMethodRow()
            Dim key As String = String.Format("ShipmentMethod_GetRow_{0}", MethodId.ToString())
            result = CType(CacheUtils.GetCache(key), ShipmentMethodRow)
            If result IsNot Nothing Then
                Return result
            Else
                result = New ShipmentMethodRow(DB, MethodId)
                result.Load()
                CacheUtils.SetCache(key, result)
                Return result
            End If

        End Function
        Public Shared Function GetNameById(ByVal Id As Integer) As String
            If Id < 1 Then
                Return Nothing
            End If
            Dim result As String = ""
            Dim sql As String = "Select Name from ShipmentMethod where MethodId=" & Id & ""
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim reader As SqlDataReader = Nothing
            Try
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MethodId As Integer)
            Dim row As ShipmentMethodRow

            row = New ShipmentMethodRow(DB, MethodId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllShipmentMethods(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select * from ShipmentMethod where methodid not in (7) order by sortorder")
            Return ds
        End Function

        Public Shared Function GetListAll() As SqlDataReader

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_ShipmentMethod_ListAll")

            Return db.ExecuteReader(cmd)
        End Function
    End Class

    Public MustInherit Class ShipmentMethodRowBase
        Private m_DB As Database
        Private m_MethodId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_WhenArrives As String = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_Code As String = Nothing
        Private m_IsDropDown As Boolean = Nothing
        Private m_Image As String = Nothing
        Private m_Insurance As Double = Nothing
        Private m_Signature As Double = Nothing
        Private m_Residential As Double = Nothing
        Private m_DASResidential As Double = Nothing
        Private m_DASCommercial As Double = Nothing
        Private m_FuelRate As Integer = Nothing

        Public Property Insurance() As Double
            Get
                Return m_Insurance
            End Get
            Set(ByVal value As Double)
                m_Insurance = value
            End Set
        End Property
        Public Property Signature() As Double
            Get
                Return m_Signature
            End Get
            Set(ByVal value As Double)
                m_Signature = value
            End Set
        End Property
        Public Property Residential() As Double
            Get
                Return m_Residential
            End Get
            Set(ByVal value As Double)
                m_Residential = value
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

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property WhenArrives() As String
            Get
                Return m_WhenArrives
            End Get
            Set(ByVal Value As String)
                m_WhenArrives = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = Value
            End Set
        End Property

        Public Property Code() As String
            Get
                Return m_Code
            End Get
            Set(ByVal Value As String)
                m_Code = Value
            End Set
        End Property

        Public Property IsDropDown() As Boolean
            Get
                Return m_IsDropDown
            End Get
            Set(ByVal Value As Boolean)
                m_IsDropDown = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
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

        Public Sub New(ByVal DB As Database, ByVal MethodId As Integer)
            m_DB = DB
            m_MethodId = MethodId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim SQL As String = "SELECT * FROM ShipmentMethod WHERE MethodId = " & MethodId.ToString()
                r = db.ExecuteReader(CommandType.Text, SQL)
                If r.HasRows AndAlso r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & "<br>Stack trace:" & ex.StackTrace & "<br>MethodId: " & MethodId.ToString())
            End Try
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)

            m_MethodId = Convert.ToInt32(r.Item("MethodId"))
            m_Name = Convert.ToString(r.Item("Name"))
            m_WhenArrives = Convert.ToString(r.Item("WhenArrives"))
            If IsDBNull(r.Item("Code")) Then
                m_Code = Nothing
            Else
                m_Code = Convert.ToString(r.Item("Code"))
            End If
            If IsDBNull(r.Item("insurance")) Then
                m_Insurance = Nothing
            Else
                m_Insurance = Convert.ToDouble(r.Item("insurance"))
            End If
            If IsDBNull(r.Item("signature")) Then
                m_Signature = Nothing
            Else
                m_Signature = Convert.ToDouble(r.Item("Signature"))
            End If
            If IsDBNull(r.Item("Residential")) Then
                m_Residential = Nothing
            Else
                m_Residential = Convert.ToDouble(r.Item("Residential"))
            End If
            If IsDBNull(r.Item("DASResidential")) Then
                m_DASResidential = Nothing
            Else
                m_DASResidential = Convert.ToDouble(r.Item("DASResidential"))
            End If
            If IsDBNull(r.Item("DASCommercial")) Then
                m_DASCommercial = Nothing
            Else
                m_DASCommercial = Convert.ToDouble(r.Item("DASCommercial"))
            End If
            If IsDBNull(r.Item("FuelRate")) Then
                m_FuelRate = Nothing
            Else
                m_FuelRate = Convert.ToDouble(r.Item("FuelRate"))
            End If
            m_IsDropDown = Convert.ToBoolean(r.Item("IsDropDown"))
            If IsDBNull(r.Item("Image")) Then
                m_Image = Nothing
            Else
                m_Image = Convert.ToString(r.Item("Image"))
            End If

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            Dim MaxSortOrder As Integer = DB.ExecuteScalar("select top 1 SortOrder from ShipmentMethod order by SortOrder desc")
            MaxSortOrder += 1

            SQL = " INSERT INTO ShipmentMethod (" _
             & " Name" _
             & ",WhenArrives" _
             & ",SortOrder" _
             & ",Code" _
             & ",IsDropDown" _
             & ",Image" _
             & ",insurance" _
             & ",signature" _
             & ",residential" _
             & ") VALUES (" _
             & m_DB.Quote(Name) _
             & "," & m_DB.Quote(WhenArrives) _
             & "," & MaxSortOrder _
             & "," & m_DB.Quote(Code) _
             & "," & CInt(IsDropDown) _
             & "," & m_DB.Quote(Image) _
             & "," & m_DB.Number(Insurance) _
             & "," & m_DB.Number(Signature) _
             & "," & m_DB.Number(Residential) _
             & ")"

            MethodId = m_DB.InsertSQL(SQL)

            Return MethodId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE ShipmentMethod SET " _
             & " Name = " & m_DB.Quote(Name) _
             & ",WhenArrives = " & m_DB.Quote(WhenArrives) _
             & ",Code = " & m_DB.Quote(Code) _
             & ",IsDropDown = " & CInt(IsDropDown) _
             & ",Image = " & m_DB.Quote(Image) _
             & ",insurance = " & m_DB.Quote(Insurance) _
             & ",signature = " & m_DB.Quote(Signature) _
             & ",Residential = " & m_DB.Quote(Residential) _
             & " WHERE MethodId = " & m_DB.Quote(MethodId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM ShipmentMethod WHERE MethodId = " & m_DB.Quote(MethodId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ShipmentMethodCollection
        Inherits GenericCollection(Of ShipmentMethodRow)
    End Class

End Namespace


