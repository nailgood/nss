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
    Public Class LevelPointRow
        Inherits LevelPointRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            MyBase.New(database, Id)
        End Sub 'New
        Public Shared Function GetDiscount(ByVal MemberId As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim sp_LevelPoint_GetDiscount As String = "sp_LevelPoint_GetDiscount"

                Dim cmd As DbCommand = db.GetStoredProcCommand(sp_LevelPoint_GetDiscount)

                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

                Return db.ExecuteDataSet(cmd).Tables(0)
            Catch ex As Exception

            End Try
            'Return 0
        End Function
        Public Shared Function GetDiscount1(ByVal MemberId As Integer, ByVal DateActive As DateTime) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim sp_LevelPoint_GetDiscount1 As String = "sp_LevelPoint_GetDiscount1"

                Dim cmd As DbCommand = db.GetStoredProcCommand(sp_LevelPoint_GetDiscount1)

                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                db.AddInParameter(cmd, "DateActive", DbType.DateTime, DateActive)

                Return db.ExecuteDataSet(cmd).Tables(0)
            Catch ex As Exception

            End Try
            'Return 0
        End Function
    End Class

    Public MustInherit Class LevelPointRowBase
        Private m_DB As Database
        Private m_LevelId As Integer = Nothing
        Private m_TypeName As String = Nothing
        Private m_Description As String = Nothing
        Private m_Discount As Integer = Nothing
        Private m_StartPoint As Integer = Nothing
        Private m_StartingDate As DateTime = Nothing
        Private m_EndingDate As DateTime = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_AdminId As Integer = Nothing
        Public Property LevelId() As Integer
            Get
                Return m_LevelId
            End Get
            Set(ByVal value As Integer)
                m_LevelId = value
            End Set
        End Property
        Public Property AdminId() As Integer
            Get
                Return m_AdminId
            End Get
            Set(ByVal value As Integer)
                m_AdminId = value
            End Set
        End Property
        Public Property TypeName() As String
            Get
                Return m_TypeName
            End Get
            Set(ByVal value As String)
                m_TypeName = value
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
        Public Property Discount() As Integer
            Get
                Return m_Discount
            End Get
            Set(ByVal value As Integer)
                m_Discount = value
            End Set
        End Property
        Public Property StartPoint() As Integer
            Get
                Return m_StartPoint
            End Get
            Set(ByVal value As Integer)
                m_StartPoint = value
            End Set
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
        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal LevelId As Integer)
            m_DB = database
            LevelId = 0
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal LevelId As Integer) As LevelPointRow
            Dim row As LevelPointRow

            row = New LevelPointRow(_Database, LevelId)
            row.LoadById(LevelId)

            Return row
        End Function

        Protected Overridable Sub LoadById(ByVal LevelId As Integer)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = ""

                SQL = "SELECT * FROM LevelPoint WHERE LevelId = " & LevelId

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
            Try
                If IsDBNull(r.Item("LevelId")) Then
                    m_LevelId = 0
                Else
                    m_LevelId = Convert.ToInt32(r.Item("LevelId"))
                End If
                If IsDBNull(r.Item("AdminId")) Then
                    m_AdminId = 0
                Else
                    m_AdminId = Convert.ToInt32(r.Item("AdminId"))
                End If
                If IsDBNull(r.Item("TypeName")) Then
                    m_TypeName = Nothing
                Else
                    m_TypeName = Convert.ToString(r.Item("TypeName"))
                End If
                If IsDBNull(r.Item("Description")) Then
                    m_Description = Nothing
                Else
                    m_Description = Convert.ToString(r.Item("Description"))
                End If
                If IsDBNull(r.Item("StartPoint")) Then
                    m_StartPoint = 0
                Else
                    m_StartPoint = Convert.ToInt32(r.Item("StartPoint"))
                End If
                If IsDBNull(r.Item("CreateDate")) Then
                    m_CreateDate = Nothing
                Else
                    m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
                End If
                If IsDBNull(r.Item("ModifyDate")) Then
                    m_ModifyDate = Nothing
                Else
                    m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub
        Public Overloads Sub Update()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_LevelPoint_Update"
                Dim cm As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cm, "LevelId", DbType.Int32, LevelId)
                db.AddInParameter(cm, "Discount", DbType.Int32, Discount)
                db.AddInParameter(cm, "Description", DbType.String, Description)
                db.AddInParameter(cm, "StartPoint", DbType.Int32, StartPoint)
                db.AddInParameter(cm, "ModifyDate", DbType.DateTime, ModifyDate)
                db.AddInParameter(cm, "AdminId", DbType.String, AdminId)
                db.ExecuteNonQuery(cm)
            Catch ex As Exception

            End Try

        End Sub

    End Class

End Namespace

