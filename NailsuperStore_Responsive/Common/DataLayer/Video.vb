'Public Class Video

'End Class
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
Imports System.Text.RegularExpressions

Namespace DataLayer
    Public Class VideoRow
        Inherits VideoRowBase
        Public Shared vId As Integer = 0
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal VideoId As Integer)
            MyBase.New(database, VideoId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal VideoId As Integer) As VideoRow
            Dim row As New VideoRow(_Database, VideoId)
            Dim r As SqlDataReader = Nothing
            Dim SQL As String = "SELECT v.VideoId,[dbo].[fc_Category_ReturnCateId](v.videoid,'Video') as CategoryId,IsYoutubeImage, ThumbImage, IsActive, Title, VideoFile, ShortDescription, ViewsCount, MetaDescription, MetaKeyword, PageTitle, CreatedDate From Video v WHERE VideoId = " & _Database.Number(VideoId)

            Try
                r = _Database.GetReader(SQL)
                If r.HasRows Then
                    row = mapList(Of VideoRow)(r).Item(0)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "GetRow", ex.ToString())
            End Try

            Return row
        End Function

        Public Shared Function ListAll(ByVal data As VideoRow) As VideoCollection
            'Get cache
            Dim ss As New VideoCollection
            Dim keyData As String = cachePrefixKey & "ListAll"
            ss = CType(CacheUtils.GetCache(keyData), VideoCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New VideoCollection
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_ListAll"
            Dim dr As SqlDataReader = Nothing

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, data.PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, data.PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 32)
                dr = db.ExecuteReader(cmd)
                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataFromReader(dr))
                    End While
                    CacheUtils.SetCache(keyData, ss)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListAll", ex.ToString())
            End Try

            Return ss
        End Function


        Public Shared Function ListByCatId(ByVal Data As VideoRow) As VideoCollection
            'Get cache
            Dim ss As New VideoCollection
            Dim keyData As String = String.Format(cachePrefixKey & "ListByCatId_{0}_{1}_{2}_{3}_{4}", Data.Condition, Data.OrderBy, Data.OrderDirection, Data.PageIndex, Data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "ListByCatId_Total_" & Data.CategoryId
            ss = CType(CacheUtils.GetCache(keyData), VideoCollection)
            If Not ss Is Nothing Then
                Data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return ss
            Else
                ss = New VideoCollection
            End If

            'Get db
            Dim dr As SqlDataReader = Nothing
            Dim sp As String = "sp_Video_ListByCatId"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Condition", DbType.String, Data.Condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, Data.OrderBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, Data.OrderDirection)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, Data.PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, Data.PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataFromReader(dr))
                    End While
                    Core.CloseReader(dr)
                    Data.TotalRow = Convert.ToInt32(cmd.Parameters("@TotalRecords").Value)
                    CacheUtils.SetCache(keyData, ss)
                    CacheUtils.SetCache(keyTotal, Data.TotalRow)
                Else
                    Core.CloseReader(dr)
                End If


            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListByCatId", ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function ListSummaryByType(ByVal Data As VideoRow) As VideoCollection
            Dim ss As New VideoCollection
            Dim keyData As String = String.Format(cachePrefixKey & "ListSummaryByType_{0}_{1}_{2}_{3}_{4}", Data.Condition, Data.OrderBy, Data.OrderDirection, Data.PageIndex, Data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "ListSummaryByType_Total"
            ss = CType(CacheUtils.GetCache(keyData), VideoCollection)
            If Not ss Is Nothing Then
                ''get Total
                Data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return ss
            Else
                ss = New VideoCollection
            End If

            'Get db
            Dim dr As SqlDataReader = Nothing
            Dim sp As String = "sp_Video_ListSummaryByType"
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = DB.GetStoredProcCommand(sp)
                DB.AddInParameter(cmd, "Condition", DbType.String, Data.Condition)
                DB.AddInParameter(cmd, "OrderBy", DbType.String, Data.OrderBy)
                DB.AddInParameter(cmd, "OrderDirection", DbType.String, Data.OrderDirection)
                DB.AddInParameter(cmd, "CurrentPage", DbType.Int32, Data.PageIndex)
                DB.AddInParameter(cmd, "PageSize", DbType.Int32, Data.PageSize)
                DB.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataFromReader(dr))
                    End While
                    Data.TotalRow = Convert.ToInt32(cmd.Parameters("@TotalRecords").Value)
                    CacheUtils.SetCache(keyData, ss)
                    CacheUtils.SetCache(keyTotal, Data.TotalRow)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListSummaryByType", ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function ListTop3ByCategoryId(ByVal CategoryId As Integer) As VideoCollection
            'Get cache
            Dim ss As New VideoCollection
            Dim keyData As String = cachePrefixKey & "ListTop3ByCategoryId_{0}" & CategoryId
            ss = CType(CacheUtils.GetCache(keyData), VideoCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New VideoCollection
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_ListTop3ByCategoryId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "CategoryId", DbType.Int32, CategoryId)
            Dim dr As SqlDataReader = Nothing
            Try
                dr = db.ExecuteReader(cmd)
                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataFromReader(dr))
                    End While
                    CacheUtils.SetCache(keyData, ss)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListTop3ByCategoryId", ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function ListRelatedByVideoId(ByVal VideoId As Integer) As VideoCollection
            Dim ss As New VideoCollection
            Dim keyData As String = cachePrefixKey & "ListRelatedByVideoId_{0}" & VideoId
            ss = CType(CacheUtils.GetCache(keyData), VideoCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New VideoCollection
            End If

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_ListRelatedByVideoId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "VideoId", DbType.Int32, VideoId)
            Dim dr As SqlDataReader = Nothing
            Try
                dr = db.ExecuteReader(cmd)
                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetVideoFromReader(dr))
                    End While
                    CacheUtils.SetCache(keyData, ss)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListRelatedByVideoId", ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function GetTopByType(ByVal Type As Integer) As VideoRow
            'Get cache
            Dim ss As New VideoRow
            Dim keyData As String = cachePrefixKey & "GetTopByType_{0}" & Type
            ss = CType(CacheUtils.GetCache(keyData), VideoRow)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New VideoRow
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_GetTopByType"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "Type", DbType.Int32, Type)
            Dim dr As SqlDataReader = Nothing
            Try
                dr = db.ExecuteReader(cmd)
                If dr.HasRows Then
                    While dr.Read
                        ss.Load(dr)
                    End While
                    CacheUtils.SetCache(keyData, ss)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "GetTopByType", ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function GetTopByCategoryId(ByVal CategoryId As Integer, ByVal Type As Integer) As VideoRow
            Dim ss As New VideoRow
            Dim keyData As String = cachePrefixKey & "GetTopByCategoryId_{0}_{1}" & CategoryId & Type
            ss = CType(CacheUtils.GetCache(keyData), VideoRow)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New VideoRow
            End If

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_GetTopByCategoryId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "CategoryId", DbType.Int32, CategoryId)
            db.AddInParameter(cmd, "Type", DbType.Int32, Type)
            Dim dr As SqlDataReader = Nothing
            Try
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    ss = mapList(Of VideoRow)(dr)(0)
                    CacheUtils.SetCache(keyData, ss)
                End If

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "GetTopByCategoryId", ex.ToString())
            End Try

            Return ss
        End Function

        Public Shared Function ListVideoPopular(ByVal Type As Utility.Common.CategoryType, ByVal categroyId As Integer) As VideoCollection
            Dim ss As New VideoCollection
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_ListVideoPopular"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "Type", DbType.Int32, CInt(Type))
            db.AddInParameter(cmd, "CategoryId", DbType.Int32, categroyId)
            Dim dr As SqlDataReader = Nothing
            Try
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetVideoFromReader(dr))
                    End While
                End If

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListVideoPopular", ex.ToString())
            End Try

            Return ss
        End Function

        Private Shared Function GetVideoFromReader(ByVal reader As SqlDataReader) As VideoRow
            Dim result As New VideoRow
            If (Not reader.IsDBNull(reader.GetOrdinal("VideoId"))) Then
                result.VideoId = Convert.ToInt32(reader("VideoId"))
            Else
                result.VideoId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                result.ThumbImage = reader("ThumbImage").ToString()
            Else
                result.ThumbImage = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                result.Title = reader("Title").ToString()
            Else
                result.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ViewsCount"))) Then
                result.ViewsCount = Convert.ToInt32(reader("ViewsCount"))
            Else
                result.ViewsCount = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ReviewsCount"))) Then
                result.ReviewsCount = Convert.ToInt32(reader("ReviewsCount"))
            Else
                result.ReviewsCount = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                result.CreatedDate = Convert.ToDateTime(reader("CreatedDate").ToString())
            Else
                result.CreatedDate = ""
            End If
            Return result
        End Function

        Public Shared Function ListTop3Video() As List(Of VideoRow)
            Dim result As List(Of VideoRow) = New List(Of VideoRow)
            Dim sp As String = "sp_Video_Top3Video"
            Dim r As SqlDataReader
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                r = DB.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of VideoRow)(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "ListTop3Video", ex.ToString())
            End Try
            
            Return result
        End Function

        Public Shared Function ListTop3Media() As List(Of VideoRow)
            Dim result As List(Of VideoRow) = New List(Of VideoRow)
            Dim sp As String = "sp_Video_Top3Media"
            Dim r As SqlDataReader
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of VideoRow)(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "ListTop3Media", ex.ToString())
            End Try

            Return result
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As VideoRow
            Dim result As New VideoRow
            If (Not reader.IsDBNull(reader.GetOrdinal("VideoId"))) Then
                result.VideoId = Convert.ToInt32(reader("VideoId"))
            Else
                result.VideoId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CategoryId"))) Then
                result.CategoryId = Convert.ToInt32(reader("CategoryId"))
            Else
                result.CategoryId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CategoryName"))) Then
                result.CategoryName = reader("CategoryName").ToString()
            Else
                result.CategoryName = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                result.ThumbImage = reader("ThumbImage").ToString()
            Else
                result.ThumbImage = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                result.Arrange = Convert.ToInt32(reader("Arrange"))
            Else
                result.Arrange = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                result.Title = reader("Title").ToString()
            Else
                result.Title = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ViewsCount"))) Then
                result.ViewsCount = Convert.ToInt32(reader("ViewsCount"))
            Else
                result.ViewsCount = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ReviewsCount"))) Then
                result.ReviewsCount = Convert.ToInt32(reader("ReviewsCount"))
            Else
                result.ReviewsCount = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                result.CreatedDate = Convert.ToDateTime(reader("CreatedDate").ToString())
            Else
                result.CreatedDate = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                result.ShortDescription = reader("ShortDescription").ToString()
            Else
                result.ShortDescription = ""
            End If
            Return result
        End Function

        Public Shared Function CountRelated(ByVal _Database As Database, ByVal VideoId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Video_CountRelatedVideo"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function CountItemRelated(ByVal _Database As Database, ByVal VideoId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Video_CountRelatedItem"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal VideoId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Video_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, VideoRelatedRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As VideoRow) As Boolean
            Try
                Dim sp As String = "sp_Video_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, data.CategoryId))
                cmd.Parameters.Add(_Database.InParam("ThumbImage", SqlDbType.VarChar, 0, data.ThumbImage))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("IsYoutubeImage", SqlDbType.Bit, 0, data.IsYoutubeImage))
                cmd.Parameters.Add(_Database.InParam("Title", SqlDbType.NVarChar, 0, data.Title))
                cmd.Parameters.Add(_Database.InParam("VideoFile", SqlDbType.VarChar, 0, data.VideoFile))
                cmd.Parameters.Add(_Database.InParam("ShortDescription", SqlDbType.NVarChar, 0, data.ShortDescription))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("MetaKeyword", SqlDbType.NVarChar, 0, data.MetaKeyword))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.InParam("CreatedDate", SqlDbType.DateTime, 0, data.CreatedDate))
                cmd.Parameters.Add(_Database.InParam("SubTitle", SqlDbType.NVarChar, 0, data.SubTitle))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                vId = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If vId > 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Update(ByVal _Database As Database, ByVal data As VideoRow) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Video_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.NVarChar, 0, data.VideoId))
                cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, data.CategoryId))
                cmd.Parameters.Add(_Database.InParam("ThumbImage", SqlDbType.VarChar, 0, data.ThumbImage))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("IsYoutubeImage", SqlDbType.Bit, 0, data.IsYoutubeImage))
                cmd.Parameters.Add(_Database.InParam("Title", SqlDbType.NVarChar, 0, data.Title))
                cmd.Parameters.Add(_Database.InParam("VideoFile", SqlDbType.VarChar, 0, data.VideoFile))
                cmd.Parameters.Add(_Database.InParam("ShortDescription", SqlDbType.NVarChar, 0, data.ShortDescription))
                cmd.Parameters.Add(_Database.InParam("ViewsCount", SqlDbType.Int, 0, data.ViewsCount))
                cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, data.PageTitle))
                cmd.Parameters.Add(_Database.InParam("MetaKeyword", SqlDbType.NVarChar, 0, data.MetaKeyword))
                cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.NVarChar, 0, data.MetaDescription))
                cmd.Parameters.Add(_Database.InParam("SubTitle", SqlDbType.NVarChar, 0, data.SubTitle))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal VideoId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Video_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeChangeArrange(ByVal _Database As Database, ByVal VideoId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Video_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Sub RemoveAllVideoCategory(ByVal _Database As Database, ByVal VideoId As Integer)
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_VideoCategory_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
        End Sub

        Public Sub InsertVideoCategory(ByVal _Database As Database, ByVal sids As String, ByVal VideoId As Integer)
            Dim ids() As String = sids.Split(",")
            For i As Integer = 0 To UBound(ids)
                If IsNumeric(ids(i)) Then InsertListVideoCategory(_Database, ids(i), VideoId)
            Next
        End Sub

        Private Sub InsertListVideoCategory(ByVal _Database As Database, ByVal c As Integer, ByVal VideoId As Integer)
            Dim result As Integer = 0
            Dim sp As String = "sp_VideoCategory_Insert"
            Dim cmd As SqlCommand = _Database.CreateCommand(sp)
            cmd.Parameters.Add(_Database.InParam("VideoId", SqlDbType.Int, 0, VideoId))
            cmd.Parameters.Add(_Database.InParam("CategoryId", SqlDbType.Int, 0, c))
            cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
            cmd.ExecuteNonQuery()
            result = CInt(cmd.Parameters("result").Value)
        End Sub

        Public Shared Function GetListCategoryIdByVideoId(ByVal DB As Database, ByVal VideoId As Integer) As String
            Dim SQL As String = "Select CategoryId from VideoCategory  where VideoId=" & DB.Quote(VideoId)
            Dim result As String = String.Empty
            Dim r As SqlDataReader = Nothing
            Try
                r = DB.GetReader(SQL)
                If Not r Is Nothing Then
                    Dim CategoryId As String = String.Empty
                    While r.Read()
                        CategoryId = r.Item("CategoryId")
                        result = result & "," & CategoryId
                    End While
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return result
        End Function

        Public Shared Function ListTop4MediaByCateId(ByVal CategoryId As Integer) As VideoCollection
            'Get cache
            Dim ss As New VideoCollection
            Dim keyData As String = cachePrefixKey & "ListTop4MediaByCateId_" & CategoryId
            ss = CType(CacheUtils.GetCache(keyData), VideoCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New VideoCollection
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Video_ListTop4MediaByCateId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "CategoryId", DbType.Int32, CategoryId)
            Dim dr As SqlDataReader = Nothing
            Try
                dr = db.ExecuteReader(cmd)
                If dr.HasRows Then
                    While dr.Read
                        ss.Add(GetDataFromReader(dr))
                    End While
                    CacheUtils.SetCache(keyData, ss)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ListTop4MediaByCateId", ex.ToString())
            End Try

            Return ss
        End Function
        Public Shared Function SetXMLtag(ByVal colName As String, ByVal Value As String, ByVal cData As Boolean)

            Return vbCrLf & "<" & colName & ">" & IIf(cData, CheckCDATA(Value), Value) & "</" & colName & ">"
        End Function

        Private Shared Function CheckCDATA(ByVal strValue As String) As String

            Dim pattern As String = "[^a-zA-Z0-9]"
            If (Regex.IsMatch(strValue, pattern)) Then
                Return "<![CDATA[" & strValue & "]]>"
            End If
            Return strValue
        End Function
    End Class


    Public MustInherit Class VideoRowBase
        Private m_DB As Database
        Private m_VideoId As Integer = Nothing
        Private m_CategoryId As Integer = Nothing
        Private m_CategoryName As String = Nothing
        Private m_ThumbImage As String = Nothing
        Private m_IsYoutubeImage As Boolean = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_IsActive As Boolean = True
        Private m_Title As String = Nothing
        Private m_VideoFile As String = Nothing
        Private m_ShortDescription As String = Nothing
        Private m_ViewsCount As Integer = Nothing
        Private m_ReviewsCount As Integer = Nothing
        Private m_MetaDescription As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_PageTitle As String = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing
        Private m_ListCategoryId As String = String.Empty
        Private m_SubTitle As String = String.Empty
        'Public itemindex As Integer = 0
        Public Shared cachePrefixKey As String = "Video_"

        Public Property VideoId() As Integer
            Get
                Return m_VideoId
            End Get
            Set(ByVal Value As Integer)
                m_VideoId = Value
            End Set
        End Property
        Public Property CategoryId() As Integer
            Get
                Return m_CategoryId
            End Get
            Set(ByVal Value As Integer)
                m_CategoryId = Value
            End Set
        End Property
        Public Property CategoryName() As String
            Get
                Return m_CategoryName
            End Get
            Set(ByVal Value As String)
                m_CategoryName = Value
            End Set
        End Property
        Public Property ThumbImage() As String
            Get
                Return m_ThumbImage
            End Get
            Set(ByVal Value As String)
                m_ThumbImage = Value
            End Set
        End Property

        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(ByVal Value As Integer)
                m_Arrange = Value
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

        Public Property IsYoutubeImage() As Boolean
            Get
                Return m_IsYoutubeImage
            End Get
            Set(ByVal Value As Boolean)
                m_IsYoutubeImage = Value
            End Set
        End Property
        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = Value
            End Set
        End Property
        Public Property VideoFile() As String
            Get
                Return m_VideoFile
            End Get
            Set(ByVal Value As String)
                m_VideoFile = Value
            End Set
        End Property
        Public Property ShortDescription() As String
            Get
                Return m_ShortDescription
            End Get
            Set(ByVal Value As String)
                m_ShortDescription = Value
            End Set
        End Property
        Public Property ViewsCount() As Integer
            Get
                Return m_ViewsCount
            End Get
            Set(ByVal Value As Integer)
                m_ViewsCount = Value
            End Set
        End Property
        Public Property ReviewsCount() As String
            Get
                Return m_ReviewsCount
            End Get
            Set(ByVal Value As String)
                m_ReviewsCount = Value
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
        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
            End Set
        End Property
        Public Property MetaKeyword() As String
            Get
                Return m_MetaKeyword
            End Get
            Set(ByVal Value As String)
                m_MetaKeyword = Value
            End Set
        End Property
        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property
        Public Property TotalRow() As Integer
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As Integer)
                m_TotalRow = Value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal Value As Integer)
                m_PageIndex = Value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal Value As Integer)
                m_PageSize = Value
            End Set
        End Property

        Public Property OrderBy() As String
            Get
                Return m_OrderBy
            End Get
            Set(ByVal Value As String)
                m_OrderBy = Value
            End Set
        End Property
        Public Property OrderDirection() As String
            Get
                Return m_OrderDirection
            End Get
            Set(ByVal Value As String)
                m_OrderDirection = Value
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
        Public Property ListCategoryId() As String
            Get
                Return m_ListCategoryId
            End Get
            Set(ByVal Value As String)
                m_ListCategoryId = Value
            End Set
        End Property
        Public Property SubTitle() As String
            Get
                Return m_SubTitle
            End Get
            Set(ByVal Value As String)
                m_SubTitle = Value
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

        Public Sub New(ByVal database As Database, ByVal VideoId As Integer)
            m_DB = database
            m_VideoId = VideoId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT v.VideoId,[dbo].[fc_Category_ReturnCateId](v.videoid,'Video') as categoryid,IsYoutubeImage, ThumbImage, IsActive, Title, VideoFile, ShortDescription, ViewsCount, MetaDescription, MetaKeyword, PageTitle, CreatedDate From Video v WHERE VideoId = " & DB.Number(VideoId)
                r = m_DB.GetReader(SQL)

            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("VideoId"))) Then
                        m_VideoId = Convert.ToInt32(reader("VideoId"))
                    Else
                        m_VideoId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ThumbImage"))) Then
                        m_ThumbImage = reader("ThumbImage").ToString()
                    Else
                        m_ThumbImage = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = True
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsYoutubeImage"))) Then
                        m_IsYoutubeImage = Convert.ToBoolean(reader("IsYoutubeImage"))
                    Else
                        m_IsYoutubeImage = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        m_Title = reader("Title").ToString()
                    Else
                        m_Title = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("VideoFile"))) Then
                        m_VideoFile = reader("VideoFile").ToString()
                    Else
                        m_VideoFile = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ShortDescription"))) Then
                        m_ShortDescription = reader("ShortDescription").ToString()
                    Else
                        m_ShortDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ViewsCount"))) Then
                        m_ViewsCount = Convert.ToInt32(reader("ViewsCount"))
                    Else
                        m_ViewsCount = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaDescription"))) Then
                        m_MetaDescription = reader("MetaDescription").ToString()
                    Else
                        m_MetaDescription = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MetaKeyword"))) Then
                        m_MetaKeyword = reader("MetaKeyword").ToString()
                    Else
                        m_MetaKeyword = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageTitle"))) Then
                        m_PageTitle = reader("PageTitle").ToString()
                    Else
                        m_PageTitle = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                        m_CreatedDate = Convert.ToDateTime(reader("CreatedDate").ToString())
                    Else
                        m_CreatedDate = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ReviewsCount"))) Then
                        m_ReviewsCount = Convert.ToInt32(reader("ReviewsCount"))
                    Else
                        m_ReviewsCount = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex

            End Try


        End Sub

    End Class

    Public Class VideoCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Video As VideoRow)
            Me.List.Add(Video)
        End Sub
        Public Sub Insert(ByVal Video As VideoRow, ByVal index As Integer)
            Me.List.Insert(index, Video)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As VideoRow
            Get
                Return CType(Me.List.Item(Index), VideoRow)
            End Get

            Set(ByVal Value As VideoRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As VideoCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New VideoCollection
                For Each obj As VideoRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

