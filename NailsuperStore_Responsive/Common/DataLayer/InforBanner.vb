
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Components.Core
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices
Imports Utility
Imports System.Data.SqlClient
Namespace DataLayer
    Public Class InforBannerRow
        Inherits InforBannerRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New


        Public Shared Function Insert(ByVal objData As InforBannerRow) As Integer
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_InforBanner_Insert")
                db.AddInParameter(cmd, "Type", DbType.Int32, objData.Type)
                db.AddInParameter(cmd, "Name", DbType.String, objData.Name)
                db.AddInParameter(cmd, "Image", DbType.String, objData.Image)
                db.AddInParameter(cmd, "Link", DbType.String, objData.Link)
                db.AddInParameter(cmd, "Description", DbType.String, objData.Description)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, objData.IsActive)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Email.SendError("ToError500", "Infor.vb-Insert()", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return 0
        End Function

        Public Shared Function Update(ByVal objData As InforBannerRow) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_InforBanner_Update")
                db.AddInParameter(cmd, "Id", DbType.Int32, objData.Id)
                db.AddInParameter(cmd, "Type", DbType.Int32, objData.Type)
                db.AddInParameter(cmd, "Name", DbType.String, objData.Name)
                db.AddInParameter(cmd, "Image", DbType.String, objData.Image)
                db.AddInParameter(cmd, "Link", DbType.String, objData.Link)
                db.AddInParameter(cmd, "Description", DbType.String, objData.Description)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, objData.IsActive)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "Infor.vb-Update()", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return False
        End Function
        Public Shared Function ChangeArrange(ByVal Id As Integer, ByVal IsUp As Boolean) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Inforbanner_ChangeArrange")
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                db.AddInParameter(cmd, "IsUp", DbType.Boolean, IsUp)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "Infor.vb-ChangeArrange(id=" & Id & ",IsUp=" & IsUp & ")", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return False
        End Function
        Public Shared Function ChangeIsActive(ByVal Id As Integer) As Boolean
            Dim result As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Update Inforbanner set IsActive=~IsActive where Id=" & Id)
                result = db.ExecuteNonQuery(cmd)
                If (result > 0) Then
                    Return True
                End If
            Catch ex As Exception

                Email.SendError("ToError500", "Infor.vb-ChangeIsActive(id=" & Id & ")", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return False
        End Function
        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim result As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Delete from Inforbanner where Id=" & Id)
                result = db.ExecuteNonQuery(cmd)
                If (result > 0) Then
                    Return True
                End If
            Catch ex As Exception

                Email.SendError("ToError500", "Infor.vb-Delete(id=" & Id & ")", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return False
        End Function
        Public Shared Function GetAllByType(ByVal type As Integer) As InforBannerCollection
            Dim ss As New InforBannerCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select id,Type,Name,Image,Link,Description,IsActive,CreatedDate,ModifyDate from Inforbanner where [Type]=" & type & " order by Arrange DESC")
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim objData As InforBannerRow = GetData(dr)
                    ss.Add(objData)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Infor.vb-GetAllByType(type=" & type & ")", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return ss
        End Function
        Public Shared Function GetRow(ByVal id As Integer) As InforBannerRow
            Dim result As InforBannerRow = Nothing
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select id,Type,Name,Image,Link,Description,IsActive,CreatedDate,ModifyDate from Inforbanner where Id=" & id)
                dr = db.ExecuteReader(cmd)
                If dr.Read Then
                    result = GetData(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Infor.vb-GetRow(id=" & id & ")", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetMainPage(ByVal type As Integer, ByVal count As Integer) As InforBannerCollection
            Dim ss As New InforBannerCollection
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select top " & count & " id,Name,Image,Link,Description from Inforbanner where IsActive=1 and [Type]=" & type & " order by Arrange DESC")
                reader = db.ExecuteReader(cmd)
                While reader.Read
                    Dim result As New InforBannerRow
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

                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        result.Description = reader("Description").ToString()
                    Else
                        result.Description = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Link"))) Then
                        result.Link = reader("Link").ToString()
                    Else
                        result.Link = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                        result.Image = reader("Image").ToString()
                    Else
                        result.Image = ""
                    End If
                    ss.Add(result)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "Infor.vb-GetMainPage(type=" & type & ")", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return ss
        End Function

        Private Shared Function GetData(ByVal reader As SqlDataReader) As InforBannerRow
            Dim result As New InforBannerRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    result.Id = Convert.ToInt32(reader("Id"))
                Else
                    result.Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                    result.Type = Convert.ToInt32(reader("Type"))
                Else
                    result.Type = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    result.IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    result.IsActive = True
                End If
                'If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                '    result.Arrange = Convert.ToInt32(reader("Arrange"))
                'Else
                '    result.Arrange = 0
                'End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    result.Description = reader("Description").ToString()
                Else
                    result.Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Link"))) Then
                    result.Link = reader("Link").ToString()
                Else
                    result.Link = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Image"))) Then
                    result.Image = reader("Image").ToString()
                Else
                    result.Image = ""
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return result
        End Function
    End Class
    Public MustInherit Class InforBannerRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_Type As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_Image As String = Nothing
        Private m_Link As String = Nothing
        Private m_Description As String = Nothing
        Private m_IsActive As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal value As Integer)
                m_Id = value
            End Set
        End Property
        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal value As Integer)
                m_Type = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property
        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal value As String)
                m_Image = value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal value As Boolean)
                m_IsActive = value
            End Set
        End Property
        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(ByVal value As Integer)
                m_Arrange = value
            End Set
        End Property
        Public Property Link() As String
            Get
                Return m_Link
            End Get
            Set(ByVal value As String)
                m_Link = value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal value As DateTime)
                m_CreateDate = value
            End Set
        End Property

        Public Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifyDate = value
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



    End Class
    Public Class InforBannerCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal banner As InforBannerRow)
            Me.List.Add(banner)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As InforBannerRow
            Get
                Return CType(Me.List.Item(Index), InforBannerRow)
            End Get

            Set(ByVal Value As InforBannerRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As InforBannerCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New InforBannerCollection
                For Each obj As InforBannerRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

