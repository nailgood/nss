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
    Public Class BannerRow
        Inherits BannerRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New
        Public Sub New(ByVal Id As Integer)
            MyBase.New(Id)
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal Banner As String)
            MyBase.New(DB, Banner)
        End Sub 'New

        Public Shared Function GetRow(ByVal Id As Integer) As BannerRow
            Dim row As BannerRow
            row = New BannerRow(Id)
            row.Load()
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal Id As Integer)
            Dim row As BannerRow
            row = New BannerRow(Id)
            row.Remove()
        End Sub
        Public Shared Function GetStaticBanner() As BannerRow
            Dim result As New BannerRow
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select top 1 Id,BannerName,Url,Background from Banner where IsBlock=1")
                dr = db.ExecuteReader(cmd)
                If dr.Read Then
                    result.BannerName = dr("BannerName").ToString()
                    result.Url = dr("Url").ToString()
                    result.Id = CInt(dr("Id"))
                    Try
                        result.Background = IIf(String.IsNullOrEmpty(dr("Background")), "", dr("Background"))
                    Catch
                        result.Background = ""
                    End Try
                End If
                Core.CloseReader(dr)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Banner.vb-GetStaticBanner", ex.Message & "<br><br>Stack trace: " & ex.StackTrace)
            End Try

            Return Nothing
        End Function
        Public Shared Function ListByDepartmentId(ByVal DepartmentId As String) As BannerCollection
            Dim banners As New BannerCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_Banner_GETLIST As String = "sp_Banner_ListByDepartmentId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_Banner_GETLIST)
                db.AddInParameter(cmd, "DepartmentId", DbType.String, DepartmentId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim banner As New BannerRow()
                    banner.BannerName = dr("BannerName").ToString()
                    banner.MobileBannerName = dr("MobileBannerName").ToString()
                    banner.Url = dr("Url").ToString()
                    banner.IsActive = CBool(dr("IsActive"))
                    banner.SortOrder = CInt(dr("SortOrder"))
                    banner.DepartmentId = CInt(dr("DepartmentId"))
                    Try
                        banner.Background = IIf(String.IsNullOrEmpty(dr("Background")), "", dr("Background"))
                    Catch
                        banner.Background = ""
                    End Try

                    banners.Add(banner)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Banner.vb", ex.Message & "<br><br>Stack trace: " & ex.StackTrace)
            End Try

            Return banners
        End Function

        Public Shared Function ChangSortOrder(ByVal _Database As Database, ByVal Id As Integer, ByVal isUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Banner_ChangeSortOrder"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                If (isUp) Then
                    cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, 1))
                Else
                    cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, 0))
                End If

                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function SwapOrderBanner(ByVal _Database As Database, ByVal Id1 As Integer, ByVal Id2 As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Banner_SwapSortOrder"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id1", SqlDbType.Int, 0, Id1))
                cmd.Parameters.Add(_Database.InParam("Id2", SqlDbType.Int, 0, Id2))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class BannerRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_BannerName As String = Nothing
        Private m_MobileBannerName As String = Nothing
        Private m_Url As String = Nothing
        Private m_DepartmentId As Integer = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_IsBlock As Boolean = False
        Private m_SortOrder As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_StartingDate As DateTime = Nothing
        Private m_EndingDate As DateTime = Nothing
        Private m_Background As String = Nothing
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property BannerName() As String
            Get
                Return m_BannerName
            End Get
            Set(ByVal Value As String)
                m_BannerName = Value
            End Set
        End Property
        Public Property MobileBannerName() As String
            Get
                Return m_MobileBannerName
            End Get
            Set(ByVal Value As String)
                m_MobileBannerName = Value
            End Set
        End Property
        Public Property Url() As String
            Get
                Return m_Url
            End Get
            Set(ByVal Value As String)
                m_Url = Value
            End Set
        End Property

        Public Property DepartmentId() As Integer
            Get
                Return m_DepartmentId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentId = Value
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
        Public Property IsBlock() As Boolean
            Get
                Return m_IsBlock
            End Get
            Set(ByVal Value As Boolean)
                m_IsBlock = Value
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
        Public ReadOnly Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
        End Property

        Public ReadOnly Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
        End Property
        Public Property StartingDate() As DateTime
            Get
                Return m_StartingDate
            End Get
            Set(ByVal value As DateTime)
                m_StartingDate = value
            End Set
        End Property

        Public Property EndingDate() As DateTime
            Get
                Return m_EndingDate
            End Get
            Set(ByVal value As DateTime)
                m_EndingDate = value
            End Set
        End Property
        Public Property Background() As String
            Get
                Return m_Background
            End Get
            Set(ByVal value As String)
                m_Background = value
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

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            m_DB = DB
            m_Id = Id
        End Sub 'New
        Public Sub New(ByVal Id As Integer)
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_Banner_GETOBJECT As String = "sp_Banner_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_Banner_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.String, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "Banner.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("BannerName"))) Then
                        m_BannerName = reader("BannerName").ToString()
                    Else
                        m_BannerName = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("MobileBannerName"))) Then
                        m_MobileBannerName = reader("MobileBannerName").ToString()
                    Else
                        m_MobileBannerName = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Url"))) Then
                        m_Url = reader("Url").ToString()
                    Else
                        m_Url = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                        m_DepartmentId = Convert.ToInt32(reader("DepartmentId"))
                    Else
                        m_DepartmentId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsBlock"))) Then
                        m_IsBlock = Convert.ToBoolean(reader("IsBlock"))
                    Else
                        m_IsBlock = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                        m_SortOrder = Convert.ToInt32(reader("SortOrder"))
                    Else
                        m_SortOrder = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("StartingDate"))) Then
                        m_StartingDate = Convert.ToDateTime(reader("StartingDate"))
                    Else
                        m_StartingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EndingDate"))) Then
                        m_EndingDate = Convert.ToDateTime(reader("EndingDate"))
                    Else
                        m_EndingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Background"))) Then
                        m_Background = Convert.ToString(reader("Background"))
                    Else
                        m_Background = Nothing
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load
        Private Function GetMaxSortOrder() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_Banner_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))
        End Function
        Public Overridable Function Insert() As Integer

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim i As Integer = StartingDate.ToString.Length
            Dim SP_Banner_INSERT As String = "sp_Banner_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_Banner_INSERT)
            db.AddOutParameter(cmd, "Id", DbType.Int32, 1)
            db.AddInParameter(cmd, "BannerName", DbType.String, BannerName)
            db.AddInParameter(cmd, "MobileBannerName", DbType.String, MobileBannerName)
            db.AddInParameter(cmd, "Url", DbType.String, Url)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
            db.AddInParameter(cmd, "CreateDate", DbType.Date, DateTime.Now)
            db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
            db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "IsBlock", DbType.Boolean, IsBlock)
            db.AddInParameter(cmd, "Background", DbType.String, Background)
            '' db.AddInParameter(cmd, "SortOrder", DbType.Int32, maxSortOrder)

            db.ExecuteNonQuery(cmd)

            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            Return Id
            '------------------------------------------------------------------------
        End Function

        Public Overridable Sub Update()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_Banner_UPDATE As String = "sp_Banner_Update"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_Banner_UPDATE)

                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                db.AddInParameter(cmd, "BannerName", DbType.String, BannerName)
                db.AddInParameter(cmd, "MobileBannerName", DbType.String, MobileBannerName)
                db.AddInParameter(cmd, "Url", DbType.String, Url)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                db.AddInParameter(cmd, "ModifyDate", DbType.DateTime, DateTime.Now)
                db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
                db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
                db.AddInParameter(cmd, "IsBlock", DbType.Boolean, IsBlock)
                db.AddInParameter(cmd, "Background", DbType.String, Background)
                ''  db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
                db.ExecuteNonQuery(cmd)
                '------------------------------------------------------------------------
            Catch ex As Exception

            End Try


        End Sub 'Update

        Public Sub Remove()

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_Banner_DELETE As String = "sp_Banner_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_Banner_DELETE)

            db.AddInParameter(cmd, "Id", DbType.Int32, Id)

            db.ExecuteNonQuery(cmd)
        End Sub 'Remove
    End Class

    Public Class BannerCollection
        Inherits GenericCollection(Of BannerRow)
        Public ReadOnly Property Clone() As BannerCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New BannerCollection
                For Each obj As BannerRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace
