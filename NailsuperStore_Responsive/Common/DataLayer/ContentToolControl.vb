
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
    Public Class ContentToolControlRow
        Inherits ContentToolControlRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Shared Function LoadListByRegionPage(ByVal pageURL As String, ByVal region As String) As ContentToolControlCollection
            Dim ss As New ContentToolControlCollection
            Dim key As String = cachePrefixKey & "LoadListByRegionPage_" & pageURL & "_" & region
            ss = CType(CacheUtils.GetCache(key), ContentToolControlCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New ContentToolControlCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPage_LoadControlByRegion"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PageUrl", DbType.String, pageURL)
                db.AddInParameter(cmd, "Region", DbType.String, region)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ContentToolControl.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return ss
        End Function

       

        Public Shared Function ListAll() As ContentToolControlCollection
            Dim ss As New ContentToolControlCollection
            Dim key As String = cachePrefixKey & "ListAll"
            ss = CType(CacheUtils.GetCache(key), ContentToolControlCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New ContentToolControlCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolControl_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataControlFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ContentToolControl.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return ss
        End Function

        Private Shared Function GetDataControlFromReader(ByVal reader As SqlDataReader) As ContentToolControlRow
            Dim result As New ContentToolControlRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    result.Id = Convert.ToInt32(reader("Id"))
                Else
                    result.Id = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("URL"))) Then
                    result.URL = reader("URL").ToString()
                Else
                    result.URL = ""
                End If
                Return result
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As ContentToolControlRow
            Dim result As New ContentToolControlRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    result.Id = Convert.ToInt32(reader("Id"))
                Else
                    result.Id = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Param"))) Then
                    result.Args = reader("Param").ToString()
                Else
                    result.Args = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("URL"))) Then
                    result.URL = reader("URL").ToString()
                Else
                    result.URL = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ControlId"))) Then
                    result.ControlId = reader("ControlId").ToString()
                Else
                    result.ControlId = ""
                End If
                Return result
            Catch ex As Exception
                Throw ex
            End Try
        End Function

       

    End Class


    Public MustInherit Class ContentToolControlRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_URL As String = Nothing
        Private m_Args As String = Nothing
        Private m_ControlId As String = Nothing
        Public Shared cachePrefixKey As String = "ContentToolControl_"

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
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
        Public Property Args() As String
            Get
                Return m_Args
            End Get
            Set(ByVal Value As String)
                m_Args = Value
            End Set
        End Property
        Public Property URL() As String
            Get
                Return m_URL
            End Get
            Set(ByVal Value As String)
                m_URL = Value
            End Set
        End Property
        Public Property ControlId() As String
            Get
                Return m_ControlId
            End Get
            Set(ByVal Value As String)
                m_ControlId = Value
            End Set
        End Property
        Public Sub New()
        End Sub 'New  

    End Class

    Public Class ContentToolControlCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Category As ContentToolControlRowBase)
            Me.List.Add(Category)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ContentToolControlRow
            Get
                Return CType(Me.List.Item(Index), ContentToolControlRow)
            End Get

            Set(ByVal Value As ContentToolControlRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As ContentToolControlCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New ContentToolControlCollection
                For Each obj In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

