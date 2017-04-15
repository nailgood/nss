Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Components
Imports Microsoft.Practices
Imports System.Data.Common
Imports System.Web.SessionState
Imports Utility

Namespace DataLayer
    Public Class ShippingFedExRow
        Inherits ShippingFedExRowBase
        Public Shared Function CheckMethodRestricted(ByVal zipCode As String, ByVal methodId As Integer) As Boolean
           
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetSqlStringCommand("Select COUNT(*) from ShippingFedExRestricted where LowValue='" & zipCode & "' and HighValue='" & zipCode & "' and MethodId=" & methodId)
            cmd.CommandType = CommandType.Text
            Dim countRestrict As Integer = db.ExecuteScalar(cmd)
            If countRestrict > 0 Then
                Return True
            End If
            Return False

        End Function
        Public Shared Function GetShippingRateFedEx(ByVal shipping As ShippingFedExRow, ByRef Zone As Integer, ByRef msg As String) As Double
            Dim result As Double = 0
            Dim SP As String = "sp_GetShippingRateFedex"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP)

            db.AddInParameter(cmd, "ZipCode", DbType.String, shipping.ZipCode)
            db.AddInParameter(cmd, "Weight", DbType.Double, shipping.Weight)
            db.AddInParameter(cmd, "MethodId", DbType.Int16, shipping.MethodId)
            db.AddOutParameter(cmd, "Fee", DbType.Double, 0)
            db.AddOutParameter(cmd, "Zone", DbType.Int16, 0)
            db.AddOutParameter(cmd, "Message", DbType.String, 1000)
            db.ExecuteNonQuery(cmd)
            Try
                result = CDbl(cmd.Parameters("@Fee").Value) 'CDbl(db.ExecuteScalar(cmd))
                Zone = CInt(cmd.Parameters("@Zone").Value)
                If result = 0 Then
                    msg = CStr(cmd.Parameters("@Message").Value)
                End If
            Catch ex As Exception
                Dim rawURL As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                Email.SendReport("ToReportFedEx", "[GetFedExRate] Exception", "Link: " & rawURL & "<br>ZipCode: " & shipping.ZipCode & "<br>Weight: " & shipping.Weight & "<br>MethodId: " & shipping.MethodId & "<br>Country: " & shipping.Country & "<br>Result: " & result & "<br>Exception: " & ex.ToString() + "")
            End Try
            Return result
        End Function

        Public Shared Sub ShippingFedExRestricted(ByVal shipping As ShippingFedExRow, ByVal methodId As Integer)
            Try
                Dim SP As String = "[sp_ShippingFedExRestricted]"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)

                db.AddInParameter(cmd, "LowValue", DbType.String, shipping.ZipCode)
                db.AddInParameter(cmd, "HighValue", DbType.String, shipping.ZipCode)
                db.AddInParameter(cmd, "MethodId", DbType.Int32, methodId)
                db.ExecuteNonQuery(cmd)
                'strRestricted = "1"
            Catch ex As Exception
                Throw ex
                'Dim rawURL As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                'Email.SendReport("ToError500", String.Format("[GetFedExRate] Exception sp_ShippingFedExRestricted"), "Link: " & rawURL & "<br>ZipCode: " & shipping.ZipCode & "<br>Weight: " & shipping.Weight & "<br>MethodId: " & shipping.MethodId & "<br>Country: " & shipping.Country & "<br>Result: " & result & "<br>Exception: " & ex.ToString() + "")
            End Try
        End Sub

        Public Shared Sub Insert(ByVal shipping As ShippingFedExRow, ByVal getZone As Integer, ByVal result As Double, ByRef Zone As Integer)
            Try
                Dim SP As String = "[sp_ShippingFedEx_Insert]"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)

                db.AddInParameter(cmd, "LowValue", DbType.Int32, shipping.ZipCode)
                db.AddInParameter(cmd, "HighValue", DbType.Int32, shipping.ZipCode)
                db.AddInParameter(cmd, "Zone", DbType.Int32, getZone)
                db.AddInParameter(cmd, "CountryId", DbType.Int32, 248)
                db.ExecuteNonQuery(cmd)
                Zone = getZone
            Catch ex As Exception
                Dim rawURL As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                Email.SendReport("ToError500", String.Format("[GetFedExRate] Exception sp_ShippingFedEx_Insert"), "Link: " & rawURL & "<br>ZipCode: " & shipping.ZipCode & "<br>Weight: " & shipping.Weight & "<br>MethodId: " & shipping.MethodId & "<br>Zone: " & getZone & "<br>Result: " & result & "<br>Exception: " & ex.ToString() + "")
            End Try
        End Sub

    End Class
    Public MustInherit Class ShippingFedExRowBase
        Private m_ZipCode As String
        Private m_Weight As Double
        Private m_MethodId As Integer
        Private m_Country As String

        Public Property ZipCode() As String
            Get
                Return m_ZipCode
            End Get
            Set(ByVal value As String)
                m_ZipCode = value
            End Set
        End Property
        Public Property Weight() As Double
            Get
                Return m_Weight
            End Get
            Set(ByVal value As Double)
                m_Weight = value
            End Set
        End Property
        Public Property MethodId() As Integer
            Get
                Return m_MethodId
            End Get
            Set(ByVal value As Integer)
                m_MethodId = value
            End Set
        End Property
        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal value As String)
                m_Country = value
            End Set
        End Property


    End Class
End Namespace