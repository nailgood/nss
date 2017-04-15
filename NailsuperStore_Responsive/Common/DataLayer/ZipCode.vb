Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components

Namespace DataLayer
    Public Class ZipCodeRow
        Inherits ZipCodeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ZipCode As String)
            MyBase.New(DB, ZipCode)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ZipCode As String) As ZipCodeRow
            Dim row As ZipCodeRow

            row = New ZipCodeRow(DB, ZipCode)
            row.Load()
            Return row
        End Function

        Public Shared Function CheckAddress(ByVal ZipCode As String, ByVal City As String, ByVal State As String, ByRef Result As Integer) As ZipCodeCollection
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim zip As New ZipCodeCollection
            Dim reader As SqlDataReader = Nothing
            Dim SP As String = "[sp_ZipCode_CheckAddress]"
            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddOutParameter(cmd, "Result", DbType.Int32, 32)
                db.AddInParameter(cmd, "ZipCode", DbType.String, ZipCode)
                db.AddInParameter(cmd, "City", DbType.String, City)
                db.AddInParameter(cmd, "State", DbType.String, State)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                Result = Convert.ToInt32(db.GetParameterValue(cmd, "Result"))
                If reader.HasRows Then
                    While reader.Read
                        Dim z As New ZipCodeRow
                        z.ZipCode = Convert.ToString(reader.Item("ZipCode"))
                        z.StateCode = Convert.ToString(reader.Item("StateCode"))
                        z.CityName = Convert.ToString(reader.Item("CityName"))
                        zip.Add(z)
                    End While
                End If
                
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "ZipCode > CheckAddress", "ZipCode: " & ZipCode & "<br>City: " & City & "<br>State: " & State & "<br>Exception: " & ex.ToString())
            End Try
            
            Return zip
        End Function

        Public Shared Function GetRow(ByVal ZipCode As String) As ZipCodeCollection
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim zip As New ZipCodeCollection
            Dim reader As SqlDataReader = Nothing
            Dim SP As String = "[sp_ZipCode_ListByZipCode]"
            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "ZipCode", DbType.String, ZipCode)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.HasRows Then
                    While reader.Read
                        Dim z As New ZipCodeRow
                        z.ZipCode = Convert.ToString(reader.Item("ZipCode"))
                        z.StateCode = Convert.ToString(reader.Item("StateCode"))
                        z.CityName = Convert.ToString(reader.Item("CityName"))
                        zip.Add(z)
                    End While
                End If

                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "ZipCode > GetRow(" & ZipCode & ")", "Exception: " & ex.ToString())
            End Try

            Return zip
        End Function



        Public Shared Function GetZipCode(ByVal ZipCode As String) As String
            Dim s As String = ""
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETLIST As String = "sp_ZipCode_GetZipCode"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
                db.AddInParameter(cmd, "ZipCode", DbType.String, ZipCode)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.Read() Then
                    s = reader(0).ToString()
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return s
        End Function
    End Class

    Public MustInherit Class ZipCodeRowBase
        Private m_DB As Database
        Private m_ZipCode As String = Nothing
        Private m_StateCode As String = Nothing
        Private m_CityName As String = Nothing

        Public Property ZipCode() As String
            Get
                Return m_ZipCode
            End Get
            Set(ByVal Value As String)
                m_ZipCode = Value
            End Set
        End Property

        Public Property StateCode() As String
            Get
                Return m_StateCode
            End Get
            Set(ByVal Value As String)
                m_StateCode = Value
            End Set
        End Property

        Public Property CityName() As String
            Get
                Return m_CityName
            End Get
            Set(ByVal Value As String)
                m_CityName = Value
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

        Public Sub New(ByVal DB As Database, ByVal ZipCode As String)
            m_DB = DB
            m_ZipCode = ZipCode
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_GETOBJECT As String = "sp_ZipCode_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

                db.AddInParameter(cmd, "ZipCode", DbType.String, ZipCode)

                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_ZipCode = Convert.ToString(r.Item("ZipCode"))
                m_StateCode = Convert.ToString(r.Item("StateCode"))
                m_CityName = Convert.ToString(r.Item("CityName"))
            Catch ex As Exception
                Components.Email.SendError("ToError500", "ZipCode.vb > Load", ex.ToString())
            End Try

        End Sub

    End Class

    Public Class ZipCodeCollection
        Inherits GenericCollection(Of ZipCodeRow)
    End Class

End Namespace
