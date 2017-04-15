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
    Public Class MemberReferRow
        Inherits MemberReferRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal ReferId As Integer)
            MyBase.New(database, ReferId)
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ReferId As Integer) As MemberReferRow
            Dim row As MemberReferRow
            row = New MemberReferRow(_Database, ReferId)
            row.Load()
            Return row
        End Function
        Public Shared Sub UpdateStatusReferFriendFromOrder(ByVal DB As Database, ByVal memberCheckOutId As Integer, ByVal orderId As Integer)
           
            Dim memberReferId As Integer = MemberReferRow.CheckAllowAddPointForUserRefer(DB, memberCheckOutId)
            If (memberReferId < 1) Then
                Exit Sub
            End If
            ''update refer status
            DB.ExecuteSQL("Update MemberRefer set Status=" & Utility.Common.ReferFriendStatus.Order & ",ModifyDate=" & DB.Quote(DateTime.Now) & ",FirstOrderId=" & orderId & "  where MemberUseRefer=" & memberCheckOutId & " and MemberRefer=" & memberReferId)

        End Sub
        Public Shared Function GetTotalPointReferFriend(ByVal _Database As Database, ByVal memberId As Integer) As Integer
            If memberId < 1 Then
                Return 0
            End If
            Dim result As Integer = 0
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_CashPoint_GetTotalPointReferFriend"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.VarChar, 0, memberId))
                dr = cmd.ExecuteReader()
                If dr.Read Then
                    result = Convert.ToInt32(dr.GetValue(0).ToString())
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetTotalPointReferFriend(ByVal _Database As Database, ByVal memberId As Integer) As Integer", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function UpdateRegisterReferFriend(ByVal _Database As Database, ByVal objRefer As MemberReferRow) As Boolean
            Dim result As Integer
            Try
                Dim sp As String = "sp_MemberRefer_UpdateRegisterReferFriend"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("TypeRefer", SqlDbType.Int, 0, objRefer.TypeRefer))
                cmd.Parameters.Add(_Database.InParam("MemberRefer", SqlDbType.Int, 0, objRefer.MemberRefer))
                cmd.Parameters.Add(_Database.InParam("MemberUseRefer", SqlDbType.Int, 0, objRefer.MemberUseRefer))
                cmd.Parameters.Add(_Database.InParam("Email", SqlDbType.VarChar, 0, objRefer.Email))
                cmd.Parameters.Add(_Database.InParam("Source", SqlDbType.Int, 0, objRefer.Source))
                cmd.Parameters.Add(_Database.InParam("Status", SqlDbType.Int, 0, objRefer.Status))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "RegisterRefer(ByVal _Database As Database, ByVal objRefer As MemberReferRow)", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        ''' <summary>
        ''' khi 1 KH checkout  order,  kiem tra va add point cho nguoi refer, ket qua tra ve la MemberId cua nguoi refer
        ''' </summary>
        ''' <param name="_Database"></param>
        ''' <param name="MemberUseRefer">Member checkout order</param>
        ''' <returns>MemberId cua nguoi refer</returns>
        ''' <remarks></remarks>
        Public Shared Function CheckAllowAddPointForUserRefer(ByVal _Database As Database, ByVal MemberUseRefer As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_MemberRefer_CheckAllowAddPointForUserRefer"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberUseRefer", SqlDbType.Int, 0, MemberUseRefer))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CheckAllowAddPointForUserRefer(ByVal _Database As Database, ByVal MemberUseRefer As Integer)", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetHistoryReferFriend(ByVal memberId As Integer) As MemberReferCollection
            Dim result As New MemberReferCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_MemberRefer_GetHistoryReferFriend"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim mr As MemberReferRow = LoadData(dr)
                    result.Add(mr)
                End While
                Core.CloseReader(dr)
                Return result
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetHistoryReferFriend(ByVal memberId=" & memberId & " As Integer)", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return Nothing
        End Function

        Public Shared Function CheckStatusEmailRefer(ByVal _Database As Database, ByVal memberId As Integer, ByVal Email As String) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_MemberRefer_CheckStatusEmailRefer"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.VarChar, 0, memberId))
                cmd.Parameters.Add(_Database.InParam("Email", SqlDbType.VarChar, 0, Email))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CheckStatusEmailRefer(ByVal _Database As Database, ByVal memberId=" & memberId & " As Integer, , ByVal Email=" & Email & " As String) As Integer", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function

        Public Shared Function GetPointEarnedByMemberRefered(ByVal _Database As Database, ByVal memberId As Integer) As Integer
            If memberId < 1 Then
                Return 0
            End If
            Dim result As Integer = 0
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_MemberRefer_GetPointEarnedByMemberRefered"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                dr = cmd.ExecuteReader()
                If dr.Read Then
                    result = Convert.ToInt32(dr.GetValue(0).ToString())
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetPointEarnedByMemberRefered(ByVal _Database As Database, ByVal memberId=" & memberId & " As Integer) As Integer", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetList(ByVal condition As String, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_MemberRefer_GetList")
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                If Not result Is Nothing Then
                    If result.Tables.Count > 0 AndAlso result.Tables(0).Rows.Count > 0 Then
                        total = result.Tables(0).Rows(0)("Total")
                    End If
                End If
                Return result.Tables(0)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetList(ByVal condition=" & condition & " As String, ByVal sortField=" & sortField & " As String, ByVal sortExp=" & sortExp & " As String, ByVal pageIndex=" & pageIndex & " As Integer, ByVal pageSize=" & pageSize & " As Integer, ByRef total As Integer) As DataTable", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return New DataTable
        End Function

        Private Shared Function LoadData(ByVal r As IDataReader) As MemberReferRow
            Dim mr As New MemberReferRow
            Try
                mr.ReferId = r.Item("ReferId")
                If r.Item("TypeRefer") Is Convert.DBNull Then
                    mr.TypeRefer = Nothing
                Else
                    mr.TypeRefer = Convert.ToInt32(r.Item("TypeRefer"))
                End If
                If r.Item("MemberRefer") Is Convert.DBNull Then
                    mr.MemberRefer = Nothing
                Else
                    mr.MemberRefer = Convert.ToInt32(r.Item("MemberRefer"))
                End If
                If r.Item("MemberUseRefer") Is Convert.DBNull Then
                    mr.MemberUseRefer = Nothing
                Else
                    mr.MemberUseRefer = Convert.ToInt32(r.Item("MemberUseRefer"))
                End If
                If r.Item("Email") Is Convert.DBNull Then
                    mr.Email = Nothing
                Else
                    mr.Email = Convert.ToString(r.Item("Email"))
                End If
                If r.Item("ProductId") Is Convert.DBNull Then
                    mr.ProductId = Nothing
                Else
                    mr.ProductId = Convert.ToInt32(r.Item("ProductId"))
                End If
                If r.Item("FirstOrderId") Is Convert.DBNull Then
                    mr.FirstOrderId = Nothing
                Else
                    mr.FirstOrderId = Convert.ToInt32(r.Item("FirstOrderId"))
                End If
                If r.Item("Source") Is Convert.DBNull Then
                    mr.Source = Nothing
                Else
                    mr.Source = Convert.ToString(r.Item("Source"))
                End If
                If r.Item("Status") Is Convert.DBNull Then
                    mr.Status = Nothing
                Else
                    mr.Status = Convert.ToInt32(r.Item("Status"))
                End If
                If r.Item("CreatedDate") Is Convert.DBNull Then
                    mr.CreatedDate = Nothing
                Else
                    mr.CreatedDate = Convert.ToDateTime(r.Item("CreatedDate"))
                End If
                If r.Item("ModifyDate") Is Convert.DBNull Then
                    mr.ModifyDate = Nothing
                Else
                    mr.ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If

                Return mr
            Catch ex As Exception
                Throw ex
            End Try
        End Function 'Load
    End Class

    Public MustInherit Class MemberReferRowBase
        Private m_DB As Database
        Private m_ReferId As Integer = Nothing
        Private m_TypeRefer As Integer = Nothing
        Private m_MemberRefer As Integer = Nothing
        Private m_MemberUseRefer As Integer = Nothing
        Private m_Email As String = Nothing
        Private m_ProductId As Integer = Nothing
        Private m_Status As Integer = Nothing
        Private m_Source As Integer = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_FirstOrderId As Integer = Nothing
        Public Property ReferId() As Integer
            Get
                Return m_ReferId
            End Get
            Set(ByVal value As Integer)
                m_ReferId = value
            End Set
        End Property
        Public Property TypeRefer() As Integer
            Get
                Return m_TypeRefer
            End Get
            Set(ByVal value As Integer)
                m_TypeRefer = value
            End Set
        End Property
        Public Property MemberRefer() As Integer
            Get
                Return m_MemberRefer
            End Get
            Set(ByVal value As Integer)
                m_MemberRefer = value
            End Set
        End Property
        Public Property MemberUseRefer() As Integer
            Get
                Return m_MemberUseRefer
            End Get
            Set(ByVal value As Integer)
                m_MemberUseRefer = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal value As String)
                m_Email = value
            End Set
        End Property

        Public Property ProductId() As Integer
            Get
                Return m_ProductId
            End Get
            Set(ByVal value As Integer)
                m_ProductId = value
            End Set
        End Property
        Public Property FirstOrderId() As Integer
            Get
                Return m_FirstOrderId
            End Get
            Set(ByVal value As Integer)
                m_FirstOrderId = value
            End Set
        End Property
        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal value As String)
                m_Status = value
            End Set
        End Property
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal value As DateTime)
                m_CreatedDate = value
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
        Public Property Source() As Integer
            Get
                Return m_Source
            End Get
            Set(ByVal value As Integer)
                m_Source = value
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
        Public Sub New(ByVal database As Database, ByVal ReferId As Integer)
            m_DB = database
            ReferId = 0
        End Sub 'New
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                If IsDBNull(r.Item("ReferId")) Then
                    m_ReferId = 0
                Else
                    m_ReferId = Convert.ToInt32(r.Item("ReferId"))
                End If

                If IsDBNull(r.Item("TypeRefer")) Then
                    m_TypeRefer = 0
                Else
                    m_TypeRefer = Convert.ToInt32(r.Item("TypeRefer"))
                End If
                If IsDBNull(r.Item("MemberRefer")) Then
                    m_MemberRefer = 0
                Else
                    m_MemberRefer = Convert.ToInt32(r.Item("MemberRefer"))
                End If
                If IsDBNull(r.Item("MemberUseRefer")) Then
                    m_MemberUseRefer = Nothing
                Else
                    m_MemberUseRefer = Convert.ToInt32(r.Item("MemberUseRefer"))
                End If
                If IsDBNull(r.Item("Email")) Then
                    m_Email = 0
                Else
                    m_Email = Convert.ToString(r.Item("Email"))
                End If
                If IsDBNull(r.Item("ProductId")) Then
                    m_ProductId = 0
                Else
                    m_ProductId = Convert.ToInt32(r.Item("ProductId"))
                End If
                If IsDBNull(r.Item("FirstOrderId")) Then
                    m_FirstOrderId = 0
                Else
                    m_FirstOrderId = Convert.ToInt32(r.Item("FirstOrderId"))
                End If
                If IsDBNull(r.Item("Source")) Then
                    m_Source = Nothing
                Else
                    m_Source = Convert.ToString(r.Item("Source"))
                End If
                If IsDBNull(r.Item("Status")) Then
                    m_Status = Nothing
                Else
                    m_Status = Convert.ToInt32(r.Item("Status"))
                End If
                If IsDBNull(r.Item("CreatedDate")) Then
                    m_CreatedDate = Nothing
                Else
                    m_CreatedDate = Convert.ToDateTime(r.Item("CreatedDate"))
                End If
                If IsDBNull(r.Item("ModifyDate")) Then
                    m_ModifyDate = Nothing
                Else
                    m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        Protected Overridable Sub Load()
            Dim reader As SqlDataReader

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_MemberRefer_GetObject"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            db.AddInParameter(cmd, "ReferId", DbType.Int32, ReferId)

            reader = CType(db.ExecuteReader(cmd), SqlDataReader)

            If reader.Read() Then
                Me.Load(reader)
            End If

            If Not reader.IsClosed Then
                reader.Close()
            End If
        End Sub
        Public Overridable Sub Insert()
            Dim result As Integer = 0
            Dim sp As String = "sp_MemberRefer_Insert"
            Dim cmd As SqlCommand = DB.CreateCommand(sp)

            cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
            cmd.Parameters.Add(DB.InParam("TypeRefer", SqlDbType.Int, 0, TypeRefer))
            cmd.Parameters.Add(DB.InParam("MemberRefer", SqlDbType.Int, 0, MemberRefer))
            cmd.Parameters.Add(DB.InParam("MemberUseRefer", SqlDbType.Int, 0, MemberUseRefer))
            cmd.Parameters.Add(DB.InParam("Email", SqlDbType.VarChar, 0, Email))
            cmd.Parameters.Add(DB.InParam("ProductId", SqlDbType.Int, 0, ProductId))
            cmd.Parameters.Add(DB.InParam("Source", SqlDbType.Int, 0, Source))
            cmd.Parameters.Add(DB.InParam("Status", SqlDbType.Int, 0, Status))
            cmd.Parameters.Add(DB.InParam("CreatedDate", SqlDbType.DateTime, 0, CreatedDate))
            cmd.ExecuteNonQuery()
            result = CInt(cmd.Parameters("result").Value)
        End Sub
        Public Overloads Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP As String = "sp_MemberRefer_Update"
            Dim cm As DbCommand = db.GetStoredProcCommand(SP)
            db.AddInParameter(cm, "ReferId", DbType.Int32, ReferId)
            db.AddInParameter(cm, "TypeRefer", SqlDbType.Int, TypeRefer)
            db.AddInParameter(cm, "MemberRefer", SqlDbType.Int, MemberRefer)
            db.AddInParameter(cm, "MemberUseRefer", SqlDbType.Int, MemberUseRefer)
            db.AddInParameter(cm, "Email", SqlDbType.VarChar, Email)
            db.AddInParameter(cm, "ProductId", SqlDbType.Int, ProductId)
            db.AddInParameter(cm, "FirstOrderId", SqlDbType.Int, FirstOrderId)
            db.AddInParameter(cm, "Source", SqlDbType.VarChar, Source)
            db.AddInParameter(cm, "Status", SqlDbType.Int, Status)
            db.AddInParameter(cm, "Modifydate", SqlDbType.DateTime, ModifyDate)
            db.ExecuteNonQuery(cm)
        End Sub
        Public Sub Delete(ByVal ReferId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_MemberRefer_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "ReferId", DbType.Int32, ReferId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class
    Public Class MemberReferCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal MemberRefer As MemberReferRow)
            Me.List.Add(MemberRefer)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As MemberReferRow
            Get
                Return CType(Me.List.Item(Index), MemberReferRow)
            End Get

            Set(ByVal Value As MemberReferRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As MemberReferCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New MemberReferCollection
                For Each obj As MemberReferRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
        Public Sub Remove(ByVal MemberRefer As MemberReferRow)
            Me.List.Remove(MemberRefer)
        End Sub

        Private m_TotalRecords As Integer

        Public Property TotalRecords() As Integer
            Get
                Return m_TotalRecords
            End Get
            Set(ByVal value As Integer)
                m_TotalRecords = value
            End Set
        End Property
    End Class
End Namespace

