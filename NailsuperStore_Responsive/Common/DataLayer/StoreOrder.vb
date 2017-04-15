Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components.Core
Imports Utility
Imports Components

Namespace DataLayer
    Public Class StoreOrderRow
        Inherits StoreOrderRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal OrderId As Integer)
            MyBase.New(database, OrderId)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal OrderNo As String, Optional ByVal IsNavisionOrderNo As Boolean = False)
            MyBase.New(database, OrderNo, IsNavisionOrderNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal OrderId As Integer) As StoreOrderRow
            Dim row As StoreOrderRow

            row = New StoreOrderRow(_Database, OrderId)
            'row.Load()
            row.LoadByID()

            Return row
        End Function

        Public Shared Function GetRow(ByVal _Database As Database, ByVal OrderNo As String, Optional ByVal IsNavisionOrderNo As Boolean = False) As StoreOrderRow
            Dim row As StoreOrderRow

            row = New StoreOrderRow(_Database, OrderNo, IsNavisionOrderNo)
            row.Load()

            Return row
        End Function
        Public Shared Sub GetPromotionCheckoutNotValid(ByVal orderId As Integer, ByVal memberId As Integer, ByRef SKU As String, ByRef promotionCode As String, ByRef typeError As Integer)

            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreOrder_GetPromotionCheckoutNotValid")
                db.AddInParameter(cmd, "OrderId", DbType.Int32, orderId)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)
                reader = db.ExecuteReader(cmd)
                If (reader.Read()) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("SKU"))) Then
                        SKU = reader("SKU").ToString()
                    Else
                        SKU = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("PromotionCode"))) Then
                        promotionCode = reader("PromotionCode").ToString()
                    Else
                        promotionCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TypeError"))) Then
                        typeError = CInt(reader("TypeError"))
                    Else
                        typeError = 0
                    End If
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetPromotionCheckoutNotValid(orderId:" & orderId & ", memberId:" & memberId & ")", ex.ToString())
            End Try
        End Sub
        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal OrderId As Integer)
            Dim row As StoreOrderRow

            row = New StoreOrderRow(_Database, OrderId)
            row.Remove()
        End Sub
        Public Shared Function UpdateValidAddress(ByVal OrderId As Integer) As Boolean
            If OrderId < 1 Then
                Return False
            End If
            Try

                Dim dbAcess As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_StoreOrder_UpdateValidAddress"
                Dim cmd As DbCommand = dbAcess.GetStoredProcCommand(SP)
                dbAcess.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                dbAcess.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                dbAcess.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(dbAcess.GetParameterValue(cmd, "return_value"))
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "StoreOrder.vb > UpdateValidAddress(ByVal OrderId:=" & OrderId & ")", ex.ToString())
            End Try
            Return False
        End Function
        Public Shared Sub ResetCartItemHandlingFee(ByVal DB As Database, ByVal OrderId As Integer)
            If OrderId < 1 Then
                Exit Sub
            End If
            Try
                Dim sqlHandling As String = "Update StoreCartItem set SpecialHandlingFee=[dbo].[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId,ItemId,AddType) where OrderId=" & OrderId & " and Type='item' and  CarrierType=" & Utility.Common.DefaultShippingId
                sqlHandling = sqlHandling & "; Update StoreCartItem set SpecialHandlingFee=0 where OrderId=" & OrderId & " and Type='item' and  CarrierType<>" & Utility.Common.DefaultShippingId
                DB.ExecuteSQL(sqlHandling)

            Catch ex As Exception
                Core.LogError("StoreItem.vb", "ResetCartItemHandlingFee(ByVal OrderId:=" & OrderId & ")", ex)
            End Try

        End Sub
        Public Shared Function GetOrderTracking(ByVal OrderId As Integer) As DataSet

            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim ds As New DataSet
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreOrder_GetOrderTracking")
                Dim pItemId As New SqlParameter()
                pItemId.ParameterName = "OrderId"
                pItemId.Value = OrderId
                pItemId.Direction = ParameterDirection.Input
                cmd.Parameters.Add(pItemId)
                ds = db.ExecuteDataSet(cmd)
                Return ds
            Catch ex As Exception

            End Try
            Return Nothing
        End Function


        Public Shared Function GetOrderNoById(ByVal Id As Integer) As String
            If Id < 1 Then
                Return Nothing
            End If
            Dim result As String = ""
            Dim sql As String = "Select OrderNo from StoreOrder where OrderId=" & Id & ""
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
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetOrderNoById", ex.ToString())
            End Try
            Return result
        End Function
        Public Shared Function GetBillingAddressType(ByVal orderId As Integer) As String
            If orderId < 1 Then
                Return String.Empty
            End If
            Dim result As String = ""
            Dim sql As String = "Select AddressType from MemberAddress address left join StoreOrder o on(o.BillingAddressId=address.AddressId) where OrderId=" & orderId
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
        Public Shared Function GetShippingAddressType(ByVal orderId As Integer) As String
            If orderId < 1 Then
                Return String.Empty
            End If
            Dim result As String = ""
            Dim sql As String = "Select AddressType from MemberAddress add left join StoreOrder o on(o.ShippingAddressId=add.AddressId) where OrderId=" & orderId
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
        Public Shared Function GetListMixMatchProductCouponId(ByVal _Database As Database, ByVal OrderId As Integer) As List(Of Integer)
            Dim dr As SqlDataReader = Nothing
            Dim result As New List(Of Integer)
            Dim mmId As Integer = 0
            Dim sql As String = String.Empty
            Try
                sql = "Select  sp.MixmatchId From StorePromotion sp where IsProductCoupon=1 and sp.PromotionId IN (SELECT PromotionId FROM StoreCartItem WHERE OrderId = " & OrderId & " and PromotionID <> 0) and MixmatchId>0"
                dr = _Database.GetReader(sql)
                While dr.Read
                    mmId = Convert.ToInt32(dr.Item("MixmatchId"))
                    If mmId > 0 Then
                        result.Add(mmId)
                    End If
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetListMixMatchProductCouponId(OrderId=" & OrderId & ")", ex.ToString())
            End Try
            Return result
        End Function
        Public Shared Function CalculateSpecialHandlingFee(ByVal DB As Database, ByVal OrderId As Integer) As Double
            Dim result As Double = 0
            Dim reader As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_StoreOrder_RecalculateSpecialHandlingFee"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    result = CDbl(reader.Item("TotalSpecialHandlingFee"))
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return result
        End Function
        Public Shared Function OnlyOversizeItems(ByVal DB As Database, ByVal OrderId As Integer) As Boolean

            Try
                Dim result As Integer = DB.ExecuteScalar("Select [dbo].[fc_StoreOrder_OnlyOversizeItem](" & OrderId & ")")
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function GetShippingTBDName(ByVal OrderId As Integer) As String
            Dim result As String = String.Empty
            Dim reader As SqlDataReader = Nothing
            Try
                Dim SP As String = "sp_StoreOrder_GetShippingTBDName"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                db.AddInParameter(cmd, "TBDCode", DbType.String, Utility.Common.ShippingTBD)
                reader = db.ExecuteReader(cmd)
                If (reader.Read()) Then

                    If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                        result = reader("Name").ToString()
                    End If
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "GetShippingTBDName", "OrderId: " & OrderId & "<br>Exception: " & ex.ToString() + "")
            End Try
            Return result
        End Function

        Public Shared Sub GetFreightShippingOption(ByVal OrderId As Integer, ByRef IsLiftGate As Boolean, ByRef IsScheduleDelivery As Boolean, ByRef IsInsideDelivery As Boolean)
            Dim result As String = String.Empty
            Dim reader As SqlDataReader = Nothing
            Try
                Dim SP As String = "sp_StoreOrder_GetFreightShippingOption"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                db.AddOutParameter(cmd, "IsLiftGate", DbType.Boolean, False)
                db.AddOutParameter(cmd, "IsScheduleDelivery", DbType.Boolean, False)
                db.AddOutParameter(cmd, "IsInsideDelivery", DbType.Boolean, False)
                reader = db.ExecuteReader(cmd)
                reader.Read()

                IsLiftGate = CBool(cmd.Parameters("@IsLiftGate").Value)
                IsScheduleDelivery = CBool(cmd.Parameters("@IsScheduleDelivery").Value)
                IsInsideDelivery = CBool(cmd.Parameters("@IsInsideDelivery").Value)
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "GetFreightShippingOption", "OrderId: " & OrderId & "<br>Exception: " & ex.ToString() + "")
            End Try

        End Sub
        'Custom Methods
        Public Shared Function GetRowByOrderNo(ByVal _Database As Database, ByVal _OrderNo As String) As StoreOrderRow
            Dim row As StoreOrderRow

            row = New StoreOrderRow(_Database)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreOrder WHERE OrderNo = " & _Database.Quote(_OrderNo)
                r = _Database.GetReader(SQL)
                If r.Read Then
                    row.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetRowByOrderNo(ByVal _Database As Database, ByVal _OrderNo As String)", ex.ToString())
            End Try

            Return row
        End Function
        Public Shared Function GetInternationalShippingRate(ByVal Weight As Double, ByVal MethodId As Integer, ByVal Country As String) As Double

            ''            db.ExecuteNonQuery(cmd)
            Dim result As Double = 0
            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                End If
            End If
            Dim reader As SqlDataReader = Nothing
            Try

                Dim msg As String = ""
                ''Dim roundWeight As Integer = Math.Ceiling(Weight)
                Weight = Math.Ceiling(Weight)
                Dim Group As Integer
                Dim SP As String = "sp_GetRateUSPS"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "CountryCode", DbType.String, Country)
                db.AddInParameter(cmd, "Weight", DbType.Double, Weight)
                reader = db.ExecuteReader(cmd)
                If (reader.Read()) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Fee"))) Then
                        result = CDbl(reader("Fee").ToString())
                    Else
                        result = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Group"))) Then
                        Group = CInt(reader("Group").ToString())
                    Else
                        Group = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Message"))) Then
                        msg = reader("Message").ToString()
                    Else
                        msg = ""
                    End If
                End If
                Core.CloseReader(reader)
                If Not String.IsNullOrEmpty(msg) Then
                    Components.Email.SendError("ToError500", "[GetRateUSPS] GetInternationalShippingRate Error", "Link: " & rawURL & "<br>Country: " & Country & "<br>Weight: " & Weight & "<br>Group: " & Group & "<br>Result: " & result & "<br>message: " & msg)
                End If
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "[GetRateUSPS] Exception", "Link: " & rawURL & "<br>Country: " & Country & "<br>Weight: " & Weight & "<br>Result: " & result & "<br>Exception: " & ex.ToString() + "")
            End Try

            Return Math.Round(result, 2)
        End Function
        Public Shared Function IsCountryHasGroupValid(ByVal CountryCode As String, ByVal DB As Database) As Boolean
            Dim count As Integer = 0
            Try
                count = DB.ExecuteScalar("Select COUNT(*)  from ShippingUSPS where CountryId=(select CountryId from Country where CountryCode='" & CountryCode & "')")

            Catch ex As Exception

            End Try
            If count > 0 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function CheckSendConfirmValid(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_CheckSendConfirmValid"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function RemoveItemPointByOrder(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_RemoveItemPoint"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function MapShippingMethod(ByVal _Database As Database, ByVal orderId As Integer) As Integer
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_MapShippingMethod"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function CheckOverrideAddress(ByVal _Database As Database, ByVal orderId As Integer, ByVal memberId As Integer) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_CheckOverrideAddress"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function InsertUniqueOrder(ByVal _Database As Database, ByVal RemoteIP As String, ByVal MemberId As Integer, ByVal sesssion As String) As Integer
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_InsertUniqueOrder"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("RemoteIP", SqlDbType.VarChar, 0, RemoteIP))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.InParam("CreateSessionID", SqlDbType.VarChar, 0, sesssion))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Sub MergerCartItem(ByVal _Database As Database, ByVal cookieOrderId As Integer, ByVal validOrderId As String)
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_MergerCart"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ValidOrderId", SqlDbType.Int, 0, validOrderId))
                cmd.Parameters.Add(_Database.InParam("CookieOrderId", SqlDbType.Int, 0, cookieOrderId))
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            End Try

        End Sub
        Public Shared Function CopyBillShipAddressFromMemberAddress(ByVal _Database As Database, ByVal MemberId As Integer) As Integer
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_CopyBillShipAddressFromMemberAddress"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function

        Public Shared Function UpdateEmailNull(ByVal _Database As Database, ByVal OrderId As Integer) As Boolean
            Try
                Dim result As Boolean = False
                Dim sp As String = "sp_StoreOrder_UpdateEmailNull"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                Return CInt(cmd.Parameters("result").Value) > 0
            Catch ex As Exception
                Throw ex
            End Try

            Return False
        End Function

        Public Shared Function CopyBillShipAddressFromMemberAddressBook(ByVal _Database As Database, ByVal addressBookId As Integer, ByVal MemberId As Integer, ByVal isShippingAddress As Boolean?) As Integer
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_CopyBillShipAddressFromMemberAddressBook"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.InParam("AddressBookId", SqlDbType.Int, 0, addressBookId))
                cmd.Parameters.Add(_Database.InParam("IsShippingAddress", SqlDbType.Bit, 2, IIf(isShippingAddress Is Nothing, DBNull.Value, isShippingAddress)))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function UpdateSendConfirm(ByVal _Database As Database, ByVal orderId As Integer, ByVal status As Boolean) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_UpdateSendConfirm"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                If status Then
                    cmd.Parameters.Add(_Database.InParam("Status", SqlDbType.Bit, 0, 1))
                Else
                    cmd.Parameters.Add(_Database.InParam("Status", SqlDbType.Bit, 0, 0))
                End If
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Sub CopyFromNavision(ByVal r As NavisionOrdersRow, ByVal sLowerCase As String, ByVal sUpperCase As String)
            BillToCustomerId = DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(r.Bill_to_Customer_No))
            If BillToCustomerId = Nothing Then Exit Sub 'Throw New Exception("Customer not found for Order." & vbCrLf & "BillToCustomerId: " & r.Bill_to_Customer_No)

            If r.Sell_To_Customer_No = r.Bill_to_Customer_No Then SellToCustomerId = BillToCustomerId Else DB.ExecuteScalar("select top 1 CustomerId from Customer where CustomerNo = " & DB.Quote(r.Sell_To_Customer_No))
            If SellToCustomerId = Nothing Then Exit Sub 'Throw New Exception("Customer not found for Order." & vbCrLf & "SellToCustomerId: " & r.Sell_To_Customer_No)
            Dim iMemberId As Integer = DB.ExecuteScalar("SELECT top 1 memberid from member where customerid = " & BillToCustomerId)
            If iMemberId = Nothing Then Exit Sub 'Throw New Exception("Member not found for BillToCustomerId: " & BillToCustomerId & "/" & r.Bill_to_Customer_No)

            If r.WebReferenceNo = Nothing Then OrderNo = Nothing
            NavisionOrderNo = r.Orders_No
            BillToName = ChangeCase(DB, r.Bill_to_Name, sLowerCase, sUpperCase)
            BillToName2 = ChangeCase(DB, r.Bill_to_Name_2, sLowerCase, sUpperCase)
            BillToAddress = ChangeCase(DB, r.Bill_to_Address, sLowerCase, sUpperCase)
            BillToAddress2 = ChangeCase(DB, r.Bill_to_Address_2, sLowerCase, sUpperCase)
            BillToCity = ChangeCase(DB, r.Bill_to_City, sLowerCase, sUpperCase)
            BillToZipcode = r.Bill_to_Post_Code
            If Not r.Bill_to_Country_Code = Nothing Then BillToCountry = r.Bill_to_Country_Code
            BillToCounty = r.Bill_to_County
            ShipToZipcode = r.Sell_to_Post_Code
            ShipToAddress = ChangeCase(DB, r.Ship_to_Address, sLowerCase, sUpperCase)
            ShipToAddress2 = ChangeCase(DB, r.Ship_to_Address_2, sLowerCase, sUpperCase)
            ShipToCity = ChangeCase(DB, r.Ship_to_City, sLowerCase, sUpperCase)
            ShipToCode = r.Ship_to_Code
            If Not r.Ship_to_Country_Code = Nothing Then ShipToCountry = r.Ship_to_Country_Code
            ShipToCounty = r.Ship_to_County
            ShipToName = ChangeCase(DB, r.Ship_to_Name, sLowerCase, sUpperCase)
            ShipToName2 = ChangeCase(DB, r.Ship_to_Name_2, sLowerCase, sUpperCase)
            ShipToZipcode = r.Ship_to_Post_Code
            OrderDate = r.Order_Date
            PostingDate = r.Posting_Date
            ShipmentDate = r.Shipment_Date
            CustomerPriceGroup = r.Customer_Price_Group
            If MemberId = Nothing Then MemberId = iMemberId
            If ProcessDate = Nothing Then
                ProcessDate = r.Order_Date
            End If
            Select Case Trim(r.Status)
                Case "Open"
                    Status = "O"
                Case "Released"
                    Status = "R"
            End Select
            PaymentTermsCode = r.Payment_Terms_Code
            CarrierType = DB.ExecuteScalar("select coalesce(methodid,1) from shipmentmethod where code = " & DB.Quote(Trim(r.Shipping_Agent_Code)))

            If CarrierType = Nothing Then CarrierType = Utility.Common.DefaultShippingId

            If RemoteIP = Nothing Then RemoteIP = "127.0.0.1"

            If OrderId = Nothing Then
                Insert()
            Else
                Update()
            End If

        End Sub
        Public Shared Function GetOrderReview(ByVal MemberId As Integer, ByVal day As Integer, ByVal lDay As Integer, ByRef TotalRecords As Integer) As StoreOrderCollection
            Dim dr As SqlDataReader = Nothing
            Dim c As New StoreOrderCollection
            Try
                Dim row As StoreOrderRow
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_StoreOrder_GetReview"
                Dim reader As SqlDataReader = Nothing
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                db.AddInParameter(cmd, "Day", DbType.Int32, day)
                db.AddInParameter(cmd, "lDay", DbType.Int32, lDay)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    row = New StoreOrderRow()
                    row.Load(dr)
                    c.Add(row)
                End While
                Core.CloseReader(dr)
                TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetOrderReview(ByVal DB As Database, ByVal MemberId As Integer, ByVal day As Integer)", ex.ToString())
            End Try
            Return c
        End Function
        Public Shared Function GetShippingInsurance(ByVal DB As Database, ByVal OrderId As Integer, ByVal MethodId As Integer) As Double
            Dim result As Double
            Try
                Dim sp As String = "sp_GetOrderShippingInsurance"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                cmd.Parameters.Add(DB.InParam("MethodId", SqlDbType.Int, 0, MethodId))
                'Freight Deilvery khong co minimum
                cmd.Parameters.Add(DB.InParam("Minimum", SqlDbType.Float, 0, IIf(Common.IsFedExCalculator() AndAlso Common.TruckShippingId <> MethodId, CDbl(SysParam.GetValue("FedExMinimumInsurance")), 0)))

                result = cmd.ExecuteScalar()
            Catch ex As Exception
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetShippingInsurance(OrderId=" & OrderId & ",MethodId=" & MethodId & ")", ex.ToString())
            End Try
            Return result
        End Function

        Public Shared Function GetCollectionForExport(ByVal DB As Database) As StoreOrderCollection
            Dim r As SqlDataReader = Nothing
            Dim row As StoreOrderRow
            Dim c As New StoreOrderCollection
            Try
                Dim SQL As String = "select * from StoreOrder where DoExport = 1 and processdate is not null"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New StoreOrderRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetCollectionForExport(ByVal DB As Database)", ex.ToString())
            End Try
            Return c
        End Function
        Public Sub UpdateInventoryQuantity()
            DB.ExecuteSQL("UPDATE StoreItem SET QtyOnHand = QtyOnHand - sc.Quantity FROM StoreCartItem sc WHERE sc.ItemId = StoreItem.ItemId AND sc.OrderId=" & DB.Number(OrderId))
        End Sub

        Public Function NumItemsAboveInventory() As Integer
            Return DB.ExecuteScalar("SELECT Count(CartItemId) FROM StoreCartItem sci, StoreItem si WHERE sci.Quantity > si.InvQuantity AND si.ItemId = sci.ItemId AND sci.OrderId=" & DB.Number(OrderId))
        End Function

        Public Shared Function FillPricing(ByVal itemId As Integer, ByVal memberId As Integer, Optional ByVal SalesType As Common.SalesPriceType = Common.SalesPriceType.Item) As DataView
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STOREORDER_GETLIST As String = "sp_StoreOrder_FillPricing"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STOREORDER_GETLIST)

            db.AddInParameter(cmd, "ItemId", DbType.Int32, itemId)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)
            db.AddInParameter(cmd, "SalesType", DbType.Int32, CInt(SalesType))
            Return db.ExecuteDataSet(cmd).Tables(0).DefaultView
        End Function


        Public Shared Function CheckExportByOrderId(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_CheckExportByOrderId"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function CheckStatusReExport(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_CheckStatusReExport"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function ProcessReExport(ByVal _Database As Database, ByVal orderId As Integer) As Boolean
            Try
                Dim result As Integer = 0
                Dim sp As String = "sp_StoreOrder_ProcessReExport"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function
        Public Shared Function ReportOrderByDay(ByVal BeginDate As DateTime, ByVal EndDate As DateTime, ByRef SumOrder As Integer, ByRef Total As Double, ByRef SumWeb As Integer, ByRef SumMobile As Integer, ByRef SumAmazon As Integer, ByRef SumEbay As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreOrder_ReportOrderByDay")
                db.AddInParameter(cmd, "BeginDate", DbType.DateTime, BeginDate)
                db.AddInParameter(cmd, "EndDate", DbType.DateTime, EndDate)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                SumOrder = Convert.ToInt32(result.Tables(1).Rows(0)(0).ToString())
                Total = Convert.ToDouble(result.Tables(1).Rows(0)(1).ToString())
                SumWeb = Convert.ToInt32(result.Tables(1).Rows(0)(2).ToString())
                SumMobile = Convert.ToInt32(result.Tables(1).Rows(0)(3).ToString())
                SumAmazon = Convert.ToInt32(result.Tables(1).Rows(0)(4).ToString())
                SumEbay = Convert.ToInt32(result.Tables(1).Rows(0)(5).ToString())
                Return result.Tables(0)
            Catch ex As Exception

            End Try
            Return New DataTable
        End Function


    End Class

    Public MustInherit Class StoreOrderRowBase
        Private m_DB As Database
        Private m_OrderId As Integer = Nothing
        Private m_PaypalStatus As Integer = Nothing
        Private m_OrderNo As String = Nothing
        Private m_NavisionOrderNo As String = Nothing
        Private m_SellToCustomerId As Integer = Nothing
        Private m_BillToSalonName As String = Nothing
        Private m_BillToName As String = Nothing
        Private m_BillToName2 As String = Nothing
        Private m_BillToCustomerId As Integer = Nothing
        Private m_BillToAddress As String = Nothing
        Private m_BillToAddress2 As String = Nothing
        Private m_BillToCity As String = Nothing
        Private m_BillToCounty As String = Nothing
        Private m_BillToZipcode As String = Nothing
        Private m_BillToPhone As String = Nothing
        Private m_BillToPhoneExt As String = Nothing
        Private m_BillToDaytimePhone As String = Nothing
        Private m_BillToDaytimePhoneExt As String = Nothing
        Private m_BillToFax As String = Nothing
        Private m_BillToCountry As String = Nothing
        Private m_Email As String = Nothing
        Private m_ShipToCode As String = Nothing
        Private m_ShipToSalonName As String = Nothing
        Private m_ShipToName As String = Nothing
        Private m_ShipToName2 As String = Nothing
        Private m_ShipToAddress As String = Nothing
        Private m_ShipToAddress2 As String = Nothing
        Private m_ShipToCity As String = Nothing
        Private m_ShipToContact As String = Nothing
        Private m_ShipToCounty As String = Nothing
        Private m_ShipToZipcode As String = Nothing
        Private m_ShipToPhone As String = Nothing
        Private m_ShipToPhoneExt As String = Nothing
        Private m_ShipToFax As String = Nothing
        Private m_ShipToCountry As String = Nothing
        Private m_CustomerPriceGroup As String = Nothing
        Private m_PaymentType As String = Nothing
        Private m_CardNumber As String = Nothing
        Private m_CIDNumber As String = Nothing
        Private m_CardTypeId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_ExpirationDate As DateTime = Nothing
        Private m_CardHolderName As String = Nothing
        Private m_AccountType As String = Nothing
        Private m_BankName As String = Nothing
        Private m_RoutingNumber As String = Nothing
        Private m_AccountNumber As String = Nothing
        Private m_CheckNumber As String = Nothing
        Private m_DLNumber As String = Nothing
        Private m_BaseSubTotal As Double = Nothing
        Private m_GiftWrapping As Double = Nothing
        Private m_SubTotal As Double = Nothing
        Private m_Shipping As Double = Nothing
        Private m_Tax As Double = Nothing
        Private m_Total As Double = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ProcessDate As DateTime = Nothing
        Private m_Status As String = Nothing
        Private m_SentDate As DateTime = Nothing
        Private m_RemoteIP As String = Nothing
        Private m_IsSameAddress As Boolean = Nothing
        Private m_IsFreeShipping As Boolean = Nothing
        Private m_IsFlammableCartItem As Common.FlammableCart = Common.FlammableCart.Init
        Private m_IsTaxExempt As Boolean = Nothing
        Private m_TaxExemptId As String = Nothing
        Private m_IsReturned As Boolean = Nothing
        Private m_Notes As String = Nothing
        Private m_Comments As String = Nothing
        Private m_OrderDate As DateTime = Nothing
        Private m_PromotionDate As DateTime = Nothing
        Private m_PostingDate As DateTime = Nothing
        Private m_ShipmentDate As DateTime = Nothing
        Private m_DoExport As Boolean = Nothing
        Private m_LastExport As DateTime = Nothing
        Private m_PaymentTermsCode As String = Nothing
        Private m_CarrierType As String = Nothing
        Private m_TotalDiscount As Double = Nothing
        Private m_RawPriceDiscountAmount As Double = Nothing
        Private m_PromotionCode As String = Nothing
        Private m_PromotionMessage As String = Nothing
        Private m_IsPromotionValid As Boolean = False
        Private m_Discount As Double = Nothing
        Private m_ShipmentInsured As Boolean = False
        Private m_ReferralCode As String = Nothing
        Private m_CheckoutPage As String = Nothing
        Private m_CreateSessionID As String = Nothing
        Private m_ProcessSessionID As String = Nothing
        Private m_ShipToAddressType As String = Nothing
        Private m_IsSignatureConfirmation As Boolean = Nothing
        Private m_IsSentConfirm As Boolean = Nothing
        Private m_SignatureConfirmation As Double = Nothing
        Private m_ResidentialFee As Double = Nothing
        Private m_SignatureDeclineCommnent As String = Nothing
        Private m_PointMessage As String = Nothing
        Private m_PurchasePoint As Integer = Nothing
        Private m_TotalRewardPoint As Integer = Nothing
        Private m_TotalPurchasePoint As Double = Nothing
        Private m_PointLevelMessage As String = Nothing
        Private m_PointAmountDiscount As Double = Nothing
        Private m_TotalProductDiscount As Double = Nothing
        Private m_IsReview As Boolean = Nothing
        Private m_IsSubmitGA As Boolean = Nothing
        Private m_IsShowReviewCart As Boolean = Nothing
        Private m_TotalSpecialHandlingFee As Double = Nothing
        Private m_HazardousMaterialFee As Double = Nothing
        Private m_Insurance As Double = Nothing
        'Property use in admin
        Private m_EbayOrderId As String = Nothing
        Private m_AmazonOrderId As String = Nothing
        Private m_TransactionID As String = Nothing
        Private m_PaypalShipToAddress As String = Nothing
        Private m_BillingAddressId As Integer = 0
        Private m_ShippingAddressId As Integer = 0
        Public itemindex As Integer = 0
        Private m_IPLocation As String = String.Empty

        '''
        Public Property CreateSessionID() As String
            Get
                Return m_CreateSessionID
            End Get
            Set(ByVal value As String)
                m_CreateSessionID = value
            End Set
        End Property

        Public Property ProcessSessionID() As String
            Get
                Return m_ProcessSessionID
            End Get
            Set(ByVal value As String)
                m_ProcessSessionID = value
            End Set
        End Property

        Public Property CheckoutPage() As String
            Get
                Return m_CheckoutPage
            End Get
            Set(ByVal value As String)
                m_CheckoutPage = value
            End Set
        End Property

        Public Property ReferralCode() As String
            Get
                Return m_ReferralCode
            End Get
            Set(ByVal value As String)
                m_ReferralCode = value
            End Set
        End Property

        Public Property ShipmentInsured() As Boolean
            Get
                Return m_ShipmentInsured
            End Get
            Set(ByVal value As Boolean)
                m_ShipmentInsured = value
            End Set
        End Property
        Public Property Insurance() As Double
            Get
                Return m_Insurance
            End Get
            Set(ByVal value As Double)
                m_Insurance = value
            End Set
        End Property
        Public Property Discount() As Double
            Get
                Return m_Discount
            End Get
            Set(ByVal value As Double)
                m_Discount = value
            End Set
        End Property
        Public Property TotalSpecialHandlingFee() As Double
            Get
                Return m_TotalSpecialHandlingFee
            End Get
            Set(ByVal value As Double)
                m_TotalSpecialHandlingFee = value
            End Set
        End Property
        Public Property HazardousMaterialFee() As Double
            Get
                Return m_HazardousMaterialFee
            End Get
            Set(ByVal value As Double)
                m_HazardousMaterialFee = value
            End Set
        End Property

        Public Property IsPromotionValid() As Boolean
            Get
                Return m_IsPromotionValid
            End Get
            Set(ByVal Value As Boolean)
                m_IsPromotionValid = Value
            End Set
        End Property

        Public Property PromotionCode() As String
            Get
                Return m_PromotionCode
            End Get
            Set(ByVal value As String)
                m_PromotionCode = value
            End Set
        End Property

        Public Property PromotionMessage() As String
            Get
                Return m_PromotionMessage
            End Get
            Set(ByVal value As String)
                m_PromotionMessage = value
            End Set
        End Property

        Public Property TotalDiscount() As Double
            Get
                Return m_TotalDiscount
            End Get
            Set(ByVal value As Double)
                m_TotalDiscount = value
            End Set
        End Property
        Public Property TotalProductDiscount() As Double
            Get
                Return m_TotalProductDiscount
            End Get
            Set(ByVal value As Double)
                m_TotalProductDiscount = value
            End Set
        End Property
        Public Property RawPriceDiscountAmount() As Double
            Get
                Return m_RawPriceDiscountAmount
            End Get
            Set(ByVal value As Double)
                m_RawPriceDiscountAmount = value
            End Set
        End Property

        Public Property CarrierType() As String
            Get
                Return m_CarrierType
            End Get
            Set(ByVal value As String)
                m_CarrierType = value
            End Set
        End Property

        Public Property SellToCustomerId() As Integer
            Get
                Return m_SellToCustomerId
            End Get
            Set(ByVal Value As Integer)
                m_SellToCustomerId = Value
            End Set
        End Property
        Public Property IsSubmitGA() As Boolean
            Get
                Return m_IsSubmitGA
            End Get
            Set(ByVal Value As Boolean)
                m_IsSubmitGA = Value
            End Set
        End Property

        Public Property PaymentTermsCode() As String
            Get
                Return m_PaymentTermsCode
            End Get
            Set(ByVal Value As String)
                m_PaymentTermsCode = Value
            End Set
        End Property

        Public Property BillToName() As String
            Get
                Return m_BillToName
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToName = Value
            End Set
        End Property

        Public Property BillToName2() As String
            Get
                Return m_BillToName2
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToName2 = Value
            End Set
        End Property

        Public Property BillToCustomerId() As Integer
            Get
                Return m_BillToCustomerId
            End Get
            Set(ByVal Value As Integer)
                m_BillToCustomerId = Value
            End Set
        End Property

        Public Property BillToAddress() As String
            Get
                Return m_BillToAddress
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToAddress = Value
            End Set
        End Property

        Public Property BillToAddress2() As String
            Get
                Return m_BillToAddress2
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToAddress2 = Value
            End Set
        End Property

        Public Property BillToCity() As String
            Get
                Return m_BillToCity
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToCity = Value
            End Set
        End Property

        Public Property BillToFax() As String
            Get
                Return m_BillToFax
            End Get
            Set(ByVal Value As String)
                m_BillToFax = Value
            End Set
        End Property

        Public Property BillToCountry() As String
            Get
                Return m_BillToCountry
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToCountry = Value
            End Set
        End Property

        Public Property ShipToCode() As String
            Get
                Return m_ShipToCode
            End Get
            Set(ByVal Value As String)
                m_ShipToCode = Value
            End Set
        End Property

        Public Property ShipToSalonName() As String
            Get
                Return m_ShipToSalonName
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToSalonName = Value
            End Set
        End Property

        Public Property BillToSalonName() As String
            Get
                Return m_BillToSalonName
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToSalonName = Value
            End Set
        End Property

        Public Property ShipToName() As String
            Get
                Return m_ShipToName
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToName = Value
            End Set
        End Property

        Public Property ShipToName2() As String
            Get
                Return m_ShipToName2
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToName2 = Value
            End Set
        End Property

        Public Property ShipToAddress() As String
            Get
                Return m_ShipToAddress
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToAddress = Value
            End Set
        End Property

        Public Property ShipToAddress2() As String
            Get
                Return m_ShipToAddress2
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToAddress2 = Value
            End Set
        End Property

        Public Property ShipToCity() As String
            Get
                Return m_ShipToCity
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToCity = Value
            End Set
        End Property

        Public Property ShipToContact() As String
            Get
                Return m_ShipToContact
            End Get
            Set(ByVal Value As String)
                m_ShipToContact = Value
            End Set
        End Property

        Public Property ShipToCounty() As String
            Get
                Return m_ShipToCounty
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToCounty = Value
            End Set
        End Property

        Public Property ShipToZipcode() As String
            Get
                Return m_ShipToZipcode
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToZipcode = Value
            End Set
        End Property

        Public Property ShipToPhone() As String
            Get
                Return m_ShipToPhone
            End Get
            Set(ByVal Value As String)
                m_ShipToPhone = Value
            End Set
        End Property

        Public Property ShipToPhoneExt() As String
            Get
                Return m_ShipToPhoneExt
            End Get
            Set(ByVal Value As String)
                m_ShipToPhoneExt = Value
            End Set
        End Property

        Public Property ShipToFax() As String
            Get
                Return m_ShipToFax
            End Get
            Set(ByVal Value As String)
                m_ShipToFax = Value
            End Set
        End Property

        Public Property ShipToCountry() As String
            Get
                Return m_ShipToCountry
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_ShipToCountry = Value
            End Set
        End Property

        Public Property CustomerPriceGroup() As String
            Get
                Return m_CustomerPriceGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerPriceGroup = Value
            End Set
        End Property

        Public Property OrderDate() As DateTime
            Get
                Return m_OrderDate
            End Get
            Set(ByVal Value As DateTime)
                m_OrderDate = Value
            End Set
        End Property

        Public Property PromotionDate() As DateTime
            Get
                Return m_PromotionDate
            End Get
            Set(ByVal Value As DateTime)
                m_PromotionDate = Value
            End Set
        End Property

        Public Property PostingDate() As DateTime
            Get
                Return m_PostingDate
            End Get
            Set(ByVal Value As DateTime)
                m_PostingDate = Value
            End Set
        End Property

        Public Property ShipmentDate() As DateTime
            Get
                Return m_ShipmentDate
            End Get
            Set(ByVal Value As DateTime)
                m_ShipmentDate = Value
            End Set
        End Property

        Public Property IsTaxExempt() As Boolean
            Get
                Return m_IsTaxExempt
            End Get
            Set(ByVal Value As Boolean)
                m_IsTaxExempt = Value
            End Set
        End Property

        Public Property IsSameAddress() As Boolean
            Get
                Return m_IsSameAddress
            End Get
            Set(ByVal Value As Boolean)
                m_IsSameAddress = Value
            End Set
        End Property

        Public Property TaxExemptId() As String
            Get
                Return m_TaxExemptId
            End Get
            Set(ByVal Value As String)
                m_TaxExemptId = Value
            End Set
        End Property

        Public Property IsReturned() As Boolean
            Get
                Return m_IsReturned
            End Get
            Set(ByVal Value As Boolean)
                m_IsReturned = Value
            End Set
        End Property

        Public Property DoExport() As Boolean
            Get
                Return m_DoExport
            End Get
            Set(ByVal Value As Boolean)
                m_DoExport = Value
            End Set
        End Property

        Public Property LastExport() As DateTime
            Get
                Return m_LastExport
            End Get
            Set(ByVal Value As DateTime)
                m_LastExport = Value
            End Set
        End Property

        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal Value As Integer)
                m_OrderId = Value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property OrderNo() As String
            Get
                Return m_OrderNo
            End Get
            Set(ByVal Value As String)
                m_OrderNo = Value
            End Set
        End Property

        Public Property PaypalStatus() As Integer
            Get
                Return m_PaypalStatus
            End Get
            Set(ByVal Value As Integer)
                m_PaypalStatus = Value
            End Set
        End Property

        Public Property NavisionOrderNo() As String
            Get
                Return m_NavisionOrderNo
            End Get
            Set(ByVal Value As String)
                m_NavisionOrderNo = Value
            End Set
        End Property

        Public Property BillToCounty() As String
            Get
                Return m_BillToCounty
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToCounty = Value
            End Set
        End Property

        Public Property BillToZipcode() As String
            Get
                Return m_BillToZipcode
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    Value = Trim(Value)
                End If
                m_BillToZipcode = Value
            End Set
        End Property

        Public Property BillToPhone() As String
            Get
                Return m_BillToPhone
            End Get
            Set(ByVal Value As String)
                m_BillToPhone = Value
            End Set
        End Property

        Public Property BillToPhoneExt() As String
            Get
                Return m_BillToPhoneExt
            End Get
            Set(ByVal Value As String)
                m_BillToPhoneExt = Value
            End Set
        End Property

        Public Property BillToDaytimePhoneExt() As String
            Get
                Return m_BillToDaytimePhoneExt
            End Get
            Set(ByVal Value As String)
                m_BillToDaytimePhoneExt = Value
            End Set
        End Property

        Public Property BillToDaytimePhone() As String
            Get
                Return m_BillToDaytimePhone
            End Get
            Set(ByVal Value As String)
                m_BillToDaytimePhone = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property PaymentType() As String
            Get
                Return m_PaymentType
            End Get
            Set(ByVal Value As String)
                m_PaymentType = Value
            End Set
        End Property

        Public Property CardNumber() As String
            Get
                Try
                    Return Crypt.DecryptTripleDes(m_CardNumber)
                Catch ex As Exception
                    Return m_CardNumber
                End Try
            End Get
            Set(ByVal Value As String)
                m_CardNumber = Value
            End Set
        End Property

        Public ReadOnly Property EncryptedCardNumber() As String
            Get
                Return Crypt.EncryptTripleDes(CardNumber)
            End Get
        End Property

        Public ReadOnly Property StarredCardNumber() As String
            Get
                If m_CardNumber = String.Empty Then
                    Return String.Empty
                Else
                    Return Right(m_CardNumber, 4).PadLeft(16, "*")
                End If
            End Get
        End Property

        Public ReadOnly Property StarredCIDNumber() As String
            Get
                If m_CIDNumber = String.Empty Then
                    Return String.Empty
                Else
                    Return Right(m_CIDNumber, 1).PadLeft(4, "*")
                End If
            End Get
        End Property

        Public Property CIDNumber() As String
            Get
                Return m_CIDNumber
            End Get
            Set(ByVal Value As String)
                m_CIDNumber = Value
            End Set
        End Property

        Public Property CardTypeId() As Integer
            Get
                Return m_CardTypeId
            End Get
            Set(ByVal Value As Integer)
                m_CardTypeId = Value
            End Set
        End Property

        Public Property ExpirationDate() As DateTime
            Get
                Return m_ExpirationDate
            End Get
            Set(ByVal Value As DateTime)
                m_ExpirationDate = Value
            End Set
        End Property

        Public Property CardHolderName() As String
            Get
                Return m_CardHolderName
            End Get
            Set(ByVal Value As String)
                m_CardHolderName = Value
            End Set
        End Property

        Public Property AccountType() As String
            Get
                Return m_AccountType
            End Get
            Set(ByVal Value As String)
                m_AccountType = Value
            End Set
        End Property

        Public Property BankName() As String
            Get
                Return m_BankName
            End Get
            Set(ByVal Value As String)
                m_BankName = Value
            End Set
        End Property

        Public Property RoutingNumber() As String
            Get
                Try
                    Return Crypt.DecryptTripleDes(m_RoutingNumber)
                Catch ex As Exception
                    Return m_RoutingNumber
                End Try
            End Get
            Set(ByVal Value As String)
                m_RoutingNumber = Value
            End Set
        End Property

        Public ReadOnly Property EncryptedRoutingNumber() As String
            Get
                Return Crypt.EncryptTripleDes(m_RoutingNumber)
            End Get
        End Property

        Public ReadOnly Property StarredRoutingNumber() As String
            Get
                If RoutingNumber = String.Empty Then
                    Return String.Empty
                Else
                    Return Right(RoutingNumber, 4).PadLeft(RoutingNumber.Length, "*")
                End If
            End Get
        End Property

        Public Property AccountNumber() As String
            Get
                Try
                    Return Crypt.DecryptTripleDes(m_AccountNumber)
                Catch ex As Exception
                    Return m_AccountNumber
                End Try
            End Get
            Set(ByVal Value As String)
                m_AccountNumber = Value
            End Set
        End Property

        Public ReadOnly Property EncryptedAccountNumber() As String
            Get
                Return Crypt.EncryptTripleDes(m_AccountNumber)
            End Get
        End Property

        Public ReadOnly Property StarredAccountNumber() As String
            Get
                If AccountNumber = String.Empty Then
                    Return String.Empty
                Else
                    Return Right(AccountNumber, 4).PadLeft(AccountNumber.Length, "*")
                End If
            End Get
        End Property

        Public Property CheckNumber() As String
            Get
                Return m_CheckNumber
            End Get
            Set(ByVal Value As String)
                m_CheckNumber = Value
            End Set
        End Property

        Public Property DLNumber() As String
            Get
                Return m_DLNumber
            End Get
            Set(ByVal Value As String)
                m_DLNumber = Value
            End Set
        End Property

        Public Property BaseSubTotal() As Double
            Get
                Return m_BaseSubTotal
            End Get
            Set(ByVal Value As Double)
                m_BaseSubTotal = Value
            End Set
        End Property

        Public Property GiftWrapping() As Double
            Get
                Return m_GiftWrapping
            End Get
            Set(ByVal Value As Double)
                m_GiftWrapping = Value
            End Set
        End Property

        Public Property SubTotal() As Double
            Get
                Return m_SubTotal
            End Get
            Set(ByVal Value As Double)
                m_SubTotal = Value
            End Set
        End Property

        Public Property Shipping() As Double
            Get
                Return m_Shipping
            End Get
            Set(ByVal Value As Double)
                m_Shipping = Value
            End Set
        End Property

        Public Property Tax() As Double
            Get
                Return m_Tax
            End Get
            Set(ByVal Value As Double)
                m_Tax = Value
            End Set
        End Property

        Public Property Total() As Double
            Get
                Return m_Total
            End Get
            Set(ByVal Value As Double)
                m_Total = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreateDate = Value
            End Set
        End Property

        Public Property ProcessDate() As DateTime
            Get
                Return m_ProcessDate
            End Get
            Set(ByVal Value As DateTime)
                m_ProcessDate = Value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal Value As String)
                m_Status = Value
            End Set
        End Property

        Public Property SentDate() As DateTime
            Get
                Return m_SentDate
            End Get
            Set(ByVal Value As DateTime)
                m_SentDate = Value
            End Set
        End Property

        Public Property RemoteIP() As String
            Get
                Return m_RemoteIP
            End Get
            Set(ByVal Value As String)
                m_RemoteIP = Value
            End Set
        End Property

        Public Property IsFreeShipping() As Boolean
            Get
                Return m_IsFreeShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeShipping = Value
            End Set
        End Property

        Public Property IsFlammableCartItem() As Common.FlammableCart
            Get
                Return m_IsFlammableCartItem
            End Get
            Set(ByVal Value As Common.FlammableCart)
                m_IsFlammableCartItem = Value
            End Set
        End Property

        Public Property Notes() As String
            Get
                Return m_Notes
            End Get
            Set(ByVal Value As String)
                m_Notes = Value
            End Set
        End Property

        Public Property Comments() As String
            Get
                Return m_Comments
            End Get
            Set(ByVal Value As String)
                m_Comments = Value
            End Set
        End Property

        Public Property ShipToAddressType() As String
            Get
                Return m_ShipToAddressType
            End Get
            Set(ByVal Value As String)
                m_ShipToAddressType = Value
            End Set
        End Property
        Public Property IsSignatureConfirmation() As Boolean
            Get
                Return m_IsSignatureConfirmation
            End Get
            Set(ByVal Value As Boolean)
                m_IsSignatureConfirmation = Value
            End Set
        End Property
        Public Property IsSentConfirm() As Boolean
            Get
                Return m_IsSentConfirm
            End Get
            Set(ByVal Value As Boolean)
                m_IsSentConfirm = Value
            End Set
        End Property

        Public Property SignatureConfirmation() As Double
            Get
                Return m_SignatureConfirmation
            End Get
            Set(ByVal Value As Double)
                m_SignatureConfirmation = Value
            End Set
        End Property

        Public Property ResidentialFee() As Double
            Get
                Return m_ResidentialFee
            End Get
            Set(ByVal Value As Double)
                m_ResidentialFee = Value
            End Set
        End Property
        Public Property SignatureDeclineCommnent() As String
            Get
                Return m_SignatureDeclineCommnent
            End Get
            Set(ByVal Value As String)
                m_SignatureDeclineCommnent = Value
            End Set
        End Property
        Public Property PointMessage() As String
            Get
                Return m_PointMessage
            End Get
            Set(ByVal Value As String)
                m_PointMessage = Value
            End Set
        End Property
        Public Property TotalRewardPoint() As Integer
            Get
                Return m_TotalRewardPoint
            End Get
            Set(ByVal Value As Integer)
                m_TotalRewardPoint = Value
            End Set
        End Property
        Public Property PurchasePoint() As Integer
            Get
                Return m_PurchasePoint
            End Get
            Set(ByVal Value As Integer)
                m_PurchasePoint = Value
            End Set
        End Property
        Public Property TotalPurchasePoint() As Double
            Get
                Return m_TotalPurchasePoint
            End Get
            Set(ByVal Value As Double)
                m_TotalPurchasePoint = Value
            End Set
        End Property
        Public Property PointLevelMessage() As String
            Get
                Return m_PointLevelMessage
            End Get
            Set(ByVal Value As String)
                m_PointLevelMessage = Value
            End Set
        End Property
        Public Property PointAmountDiscount() As Double
            Get
                Return m_PointAmountDiscount
            End Get
            Set(ByVal Value As Double)
                m_PointAmountDiscount = Value
            End Set
        End Property
        Public Property IsReview() As Boolean
            Get
                Return m_IsReview
            End Get
            Set(ByVal Value As Boolean)
                m_IsReview = Value
            End Set
        End Property
        Public Property IsShowReviewCart() As Boolean
            Get
                Return m_IsShowReviewCart
            End Get
            Set(ByVal Value As Boolean)
                m_IsShowReviewCart = Value
            End Set
        End Property
        Public Property EbayOrderId() As String
            Get
                Return m_EbayOrderId
            End Get
            Set(ByVal Value As String)
                m_EbayOrderId = Value
            End Set
        End Property
        Public Property AmazonOrderId() As String
            Get
                Return m_AmazonOrderId
            End Get
            Set(ByVal Value As String)
                m_AmazonOrderId = Value
            End Set
        End Property
        Public Property TransactionID() As String
            Get
                Return m_TransactionID
            End Get
            Set(ByVal Value As String)
                m_TransactionID = Value
            End Set
        End Property
        Public Property PaypalShipToAddress() As String
            Get
                Return m_PaypalShipToAddress
            End Get
            Set(ByVal Value As String)
                m_PaypalShipToAddress = Value
            End Set
        End Property
        Public Property BillingAddressId() As Integer
            Get
                Return m_BillingAddressId
            End Get
            Set(ByVal Value As Integer)
                m_BillingAddressId = Value
            End Set
        End Property
        Public Property ShippingAddressId() As Integer
            Get
                Return m_ShippingAddressId
            End Get
            Set(ByVal Value As Integer)
                m_ShippingAddressId = Value
            End Set
        End Property
        Public Property IPLocation() As String
            Get
                Return m_IPLocation
            End Get
            Set(ByVal Value As String)
                m_IPLocation = Value
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

        Public Sub New(ByVal database As Database, ByVal OrderId As Integer)
            m_DB = database
            m_OrderId = OrderId
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal OrderNo As String, ByVal IsNavisionOrderNo As Boolean)
            m_DB = database
            If IsNavisionOrderNo Then
                m_NavisionOrderNo = OrderNo
            Else
                m_OrderNo = OrderNo
            End If
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal OrderNo As String)
            m_DB = database
            m_OrderNo = OrderNo
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Dim SQL As String = String.Empty

            Try
                SQL = "SELECT * FROM StoreOrder WHERE " & IIf(OrderId <> Nothing, "OrderId = " & DB.Quote(OrderId), IIf(NavisionOrderNo <> Nothing, "NavisionOrderNo = " & DB.Quote(NavisionOrderNo), "OrderNo = " & DB.Quote(OrderNo)))
                r = m_DB.GetReader(SQL)

                If r.HasRows Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "StoreOrder.vb  > Load()", "SQL: " & SQL & "<br>Exception: " & ex.ToString())
            End Try

        End Sub

        Protected Overridable Sub LoadByID()
            Dim bError = False
            If (OrderId > 0) Then
                Dim r As SqlDataReader = Nothing
                Dim SQL As String = String.Empty
                Try
                    SQL = "SELECT * FROM StoreOrder WHERE OrderId = " & DB.Quote(OrderId)
                    r = m_DB.GetReader(SQL)
                    If r.HasRows Then
                        If r.Read Then
                            Me.Load(r)
                        End If
                    End If
                    Core.CloseReader(r)
                Catch ex As Exception
                    Core.CloseReader(r)
                    bError = True
                    'Components.Email.SendError("ToError500", "StoreOrder.vb > LoadByID()", "SQL: " & SQL & "<br>Exception: " & ex.ToString() & "<br>Url: " & System.Web.HttpContext.Current.Request.Url.ToString() & BaseShoppingCart.GetSessionList())
                End Try

                If bError Then
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Try
                        r = db.ExecuteReader(CommandType.Text, SQL)
                        If r.HasRows AndAlso r.Read Then
                            Me.Load(r)
                        End If
                    Catch ex As Exception
                        Core.CloseReader(r)
                        Components.Email.SendError("ToError500", "StoreOrder.vb > LoadByID()", "SQL: " & SQL & "<br>Exception: " & ex.ToString() & "<br>Url: " & System.Web.HttpContext.Current.Request.Url.ToString() & BaseShoppingCart.GetSessionList())
                    End Try
                End If

            End If
        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                m_ShipmentInsured = Convert.ToBoolean(r.Item("ShipmentInsured"))
                m_ReferralCode = Convert.ToString(r.Item("ReferralCode"))

                If IsDBNull(r.Item("OrderNo")) Then
                    m_OrderNo = Nothing
                Else
                    m_OrderNo = Convert.ToString(r.Item("OrderNo"))
                End If

                If IsDBNull(r.Item("PaypalStatus")) Then
                    m_PaypalStatus = 0
                Else
                    m_PaypalStatus = Convert.ToInt32(r.Item("PaypalStatus"))
                End If

                If IsDBNull(r.Item("NavisionOrderNo")) Then
                    m_NavisionOrderNo = Nothing
                Else
                    m_NavisionOrderNo = Convert.ToString(r.Item("NavisionOrderNo"))
                End If
                If r.Item("CardNumber") Is Convert.DBNull Then
                    m_CardNumber = Nothing
                Else
                    Try
                        m_CardNumber = Crypt.DecryptTripleDes(Convert.ToString(r.Item("CardNumber")))
                    Catch ex As Exception
                        m_CardNumber = Convert.ToString(r.Item("CardNumber"))
                    End Try
                End If
                If IsDBNull(r.Item("SellToCustomerId")) Then
                    m_SellToCustomerId = Nothing
                Else
                    m_SellToCustomerId = Convert.ToInt32(r.Item("SellToCustomerId"))
                End If
                If IsDBNull(r.Item("PaymentTermsCode")) Then
                    m_PaymentTermsCode = Nothing
                Else
                    m_PaymentTermsCode = Convert.ToString(r.Item("PaymentTermsCode"))
                End If
                If IsDBNull(r.Item("IsSubmitGA")) Then
                    m_IsSubmitGA = Nothing
                Else
                    m_IsSubmitGA = Convert.ToDouble(r.Item("IsSubmitGA"))
                End If

                If IsDBNull(r.Item("BillToName")) Then
                    m_BillToName = Nothing
                Else
                    m_BillToName = Convert.ToString(r.Item("BillToName"))
                End If
                If IsDBNull(r.Item("BillToName2")) Then
                    m_BillToName2 = Nothing
                Else
                    m_BillToName2 = Convert.ToString(r.Item("BillToName2"))
                End If
                If IsDBNull(r.Item("BillToCustomerId")) Then
                    m_BillToCustomerId = Nothing
                Else
                    m_BillToCustomerId = Convert.ToInt32(r.Item("BillToCustomerId"))
                End If
                If IsDBNull(r.Item("BillToAddress")) Then
                    m_BillToAddress = Nothing
                Else
                    m_BillToAddress = Convert.ToString(r.Item("BillToAddress"))
                End If
                If IsDBNull(r.Item("BillToAddress2")) Then
                    m_BillToAddress2 = Nothing
                Else
                    m_BillToAddress2 = Convert.ToString(r.Item("BillToAddress2"))
                End If
                If IsDBNull(r.Item("BillToCity")) Then
                    m_BillToCity = ""
                Else
                    m_BillToCity = Convert.ToString(r.Item("BillToCity"))
                End If
                If IsDBNull(r.Item("BillToCounty")) Then
                    m_BillToCounty = ""
                Else
                    m_BillToCounty = Convert.ToString(r.Item("BillToCounty"))
                End If
                If IsDBNull(r.Item("BillToZipcode")) Then
                    m_BillToZipcode = Nothing
                Else
                    m_BillToZipcode = Convert.ToString(r.Item("BillToZipcode"))
                End If
                If IsDBNull(r.Item("BillToPhone")) Then
                    m_BillToPhone = Nothing
                Else
                    m_BillToPhone = Convert.ToString(r.Item("BillToPhone"))
                End If
                If IsDBNull(r.Item("BillToPhoneExt")) Then
                    m_BillToPhoneExt = Nothing
                Else
                    m_BillToPhoneExt = Convert.ToString(r.Item("BillToPhoneExt"))
                End If
                If IsDBNull(r.Item("BillToDaytimePhone")) Then
                    m_BillToDaytimePhone = Nothing
                Else
                    m_BillToDaytimePhone = Convert.ToString(r.Item("BillToDaytimePhone"))
                End If
                If IsDBNull(r.Item("BillToDaytimePhoneExt")) Then
                    m_BillToDaytimePhoneExt = Nothing
                Else
                    m_BillToDaytimePhoneExt = Convert.ToString(r.Item("BillToDaytimePhoneExt"))
                End If
                If IsDBNull(r.Item("BillToFax")) Then
                    m_BillToFax = Nothing
                Else
                    m_BillToFax = Convert.ToString(r.Item("BillToFax"))
                End If
                If IsDBNull(r.Item("BillToCountry")) Then
                    m_BillToCountry = ""
                Else
                    m_BillToCountry = Convert.ToString(r.Item("BillToCountry"))
                End If
                If IsDBNull(r.Item("Email")) Then
                    m_Email = Nothing
                Else
                    m_Email = Convert.ToString(r.Item("Email"))
                End If
                If IsDBNull(r.Item("ShipToCode")) Then
                    m_ShipToCode = Nothing
                Else
                    m_ShipToCode = Convert.ToString(r.Item("ShipToCode"))
                End If
                If IsDBNull(r.Item("ShipToSalonName")) Then
                    m_ShipToSalonName = Nothing
                Else
                    m_ShipToSalonName = Convert.ToString(r.Item("ShipToSalonName"))
                End If
                If IsDBNull(r.Item("BillToSalonName")) Then
                    m_BillToSalonName = Nothing
                Else
                    m_BillToSalonName = Convert.ToString(r.Item("BillToSalonName"))
                End If
                If IsDBNull(r.Item("ShipToName")) Then
                    m_ShipToName = Nothing
                Else
                    m_ShipToName = Convert.ToString(r.Item("ShipToName"))
                End If
                If IsDBNull(r.Item("ShipToName2")) Then
                    m_ShipToName2 = Nothing
                Else
                    m_ShipToName2 = Convert.ToString(r.Item("ShipToName2"))
                End If
                If IsDBNull(r.Item("ShipToAddress")) Then
                    m_ShipToAddress = Nothing
                Else
                    m_ShipToAddress = Convert.ToString(r.Item("ShipToAddress"))
                End If
                If IsDBNull(r.Item("ShipToAddress2")) Then
                    m_ShipToAddress2 = Nothing
                Else
                    m_ShipToAddress2 = Convert.ToString(r.Item("ShipToAddress2"))
                End If
                If IsDBNull(r.Item("ShipToCity")) Then
                    m_ShipToCity = Nothing
                Else
                    m_ShipToCity = Convert.ToString(r.Item("ShipToCity"))
                End If
                If IsDBNull(r.Item("ShipToContact")) Then
                    m_ShipToContact = Nothing
                Else
                    m_ShipToContact = Convert.ToString(r.Item("ShipToContact"))
                End If
                If IsDBNull(r.Item("ShipToCounty")) Then
                    m_ShipToCounty = String.Empty
                Else
                    m_ShipToCounty = Convert.ToString(r.Item("ShipToCounty"))
                End If
                If IsDBNull(r.Item("ShipToZipcode")) Then
                    m_ShipToZipcode = Nothing
                Else
                    m_ShipToZipcode = Convert.ToString(r.Item("ShipToZipcode"))
                End If
                If IsDBNull(r.Item("PromotionCode")) Then
                    m_PromotionCode = Nothing
                Else
                    m_PromotionCode = Convert.ToString(r.Item("PromotionCode"))
                End If
                If IsDBNull(r.Item("PromotionMessage")) Then
                    m_PromotionMessage = Nothing
                Else
                    m_PromotionMessage = Convert.ToString(r.Item("PromotionMessage"))
                End If
                If IsDBNull(r.Item("ShipToZipcode")) Then
                    m_ShipToZipcode = Nothing
                Else
                    m_ShipToZipcode = Convert.ToString(r.Item("ShipToZipcode"))
                End If
                If IsDBNull(r.Item("ShipToPhone")) Then
                    m_ShipToPhone = Nothing
                Else
                    m_ShipToPhone = Convert.ToString(r.Item("ShipToPhone"))
                End If
                If IsDBNull(r.Item("ShipToPhoneExt")) Then
                    m_ShipToPhoneExt = Nothing
                Else
                    m_ShipToPhoneExt = Convert.ToString(r.Item("ShipToPhoneExt"))
                End If
                If IsDBNull(r.Item("ShipToFax")) Then
                    m_ShipToFax = Nothing
                Else
                    m_ShipToFax = Convert.ToString(r.Item("ShipToFax"))
                End If
                If IsDBNull(r.Item("ShipToCountry")) Then
                    m_ShipToCountry = Nothing
                Else
                    m_ShipToCountry = Convert.ToString(r.Item("ShipToCountry"))
                End If
                If IsDBNull(r.Item("CustomerPriceGroup")) Then
                    m_CustomerPriceGroup = Nothing
                Else
                    m_CustomerPriceGroup = Convert.ToString(r.Item("CustomerPriceGroup"))
                End If
                If IsDBNull(r.Item("PaymentType")) Then
                    m_PaymentType = Nothing
                Else
                    m_PaymentType = Convert.ToString(r.Item("PaymentType"))
                End If
                If IsDBNull(r.Item("CIDNumber")) Then
                    m_CIDNumber = Nothing
                Else
                    m_CIDNumber = Convert.ToString(r.Item("CIDNumber"))
                End If
                If IsDBNull(r.Item("CardTypeId")) Then
                    m_CardTypeId = Nothing
                Else
                    m_CardTypeId = Convert.ToInt32(r.Item("CardTypeId"))
                End If
                If IsDBNull(r.Item("MemberId")) Then
                    m_MemberId = Nothing
                Else
                    m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                End If
                If IsDBNull(r.Item("ExpirationDate")) Then
                    m_ExpirationDate = Nothing
                Else
                    m_ExpirationDate = Convert.ToDateTime(r.Item("ExpirationDate"))
                End If
                If IsDBNull(r.Item("CardHolderName")) Then
                    m_CardHolderName = Nothing
                Else
                    m_CardHolderName = Convert.ToString(r.Item("CardHolderName"))
                End If
                If IsDBNull(r.Item("AccountType")) Then
                    m_AccountType = Nothing
                Else
                    m_AccountType = Convert.ToString(r.Item("AccountType"))
                End If
                If IsDBNull(r.Item("BankName")) Then
                    m_BankName = Nothing
                Else
                    m_BankName = Convert.ToString(r.Item("BankName"))
                End If
                If IsDBNull(r.Item("RoutingNumber")) Then
                    m_RoutingNumber = Nothing
                Else
                    m_RoutingNumber = Convert.ToString(r.Item("RoutingNumber"))
                End If
                If IsDBNull(r.Item("AccountNumber")) Then
                    m_AccountNumber = Nothing
                Else
                    m_AccountNumber = Convert.ToString(r.Item("AccountNumber"))
                End If
                If IsDBNull(r.Item("CheckNumber")) Then
                    m_CheckNumber = Nothing
                Else
                    m_CheckNumber = Convert.ToString(r.Item("CheckNumber"))
                End If
                If IsDBNull(r.Item("DLNumber")) Then
                    m_DLNumber = Nothing
                Else
                    m_DLNumber = Convert.ToString(r.Item("DLNumber"))
                End If
                m_BaseSubTotal = Convert.ToDouble(r.Item("BaseSubTotal"))
                If IsDBNull(r.Item("GiftWrapping")) Then
                    m_GiftWrapping = Nothing
                Else
                    m_GiftWrapping = Convert.ToDouble(r.Item("GiftWrapping"))
                End If
                m_SubTotal = Convert.ToDouble(r.Item("SubTotal"))
                m_Shipping = Convert.ToDouble(r.Item("Shipping"))
                m_Tax = Convert.ToDouble(r.Item("Tax"))
                m_Total = Convert.ToDouble(r.Item("Total"))
                m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
                If IsDBNull(r.Item("ProcessDate")) Then
                    m_ProcessDate = Nothing
                Else
                    m_ProcessDate = Convert.ToDateTime(r.Item("ProcessDate"))
                End If
                m_Status = Convert.ToString(r.Item("Status"))
                If IsDBNull(r.Item("SentDate")) Then
                    m_SentDate = Nothing
                Else
                    m_SentDate = Convert.ToDateTime(r.Item("SentDate"))
                End If
                m_RemoteIP = Convert.ToString(r.Item("RemoteIP"))
                m_IsSameAddress = Convert.ToBoolean(r.Item("IsSameAddress"))
                m_IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                m_IsReturned = Convert.ToBoolean(r.Item("IsReturned"))
                m_IsTaxExempt = Convert.ToBoolean(r.Item("IsTaxExempt"))
                m_IsPromotionValid = Convert.ToBoolean(r.Item("IsPromotionValid"))
                If IsDBNull(r.Item("TaxExemptId")) Then
                    m_TaxExemptId = Nothing
                Else
                    m_TaxExemptId = Convert.ToString(r.Item("TaxExemptId"))
                End If
                If IsDBNull(r.Item("Notes")) Then
                    m_Notes = Nothing
                Else
                    m_Notes = Convert.ToString(r.Item("Notes"))
                End If
                If IsDBNull(r.Item("Comments")) Then
                    m_Comments = Nothing
                Else
                    m_Comments = Convert.ToString(r.Item("Comments"))
                End If
                If IsDBNull(r.Item("OrderDate")) Then
                    m_OrderDate = Nothing
                Else
                    m_OrderDate = Convert.ToDateTime(r.Item("OrderDate"))
                End If

                If IsDBNull(r.Item("PromotionDate")) Then
                    m_PromotionDate = Nothing
                Else
                    m_PromotionDate = Convert.ToDateTime(r.Item("PromotionDate"))
                End If

                If IsDBNull(r.Item("PostingDate")) Then
                    m_PostingDate = Nothing
                Else
                    m_PostingDate = Convert.ToDateTime(r.Item("PostingDate"))
                End If
                If IsDBNull(r.Item("ShipmentDate")) Then
                    m_ShipmentDate = Nothing
                Else
                    m_ShipmentDate = Convert.ToDateTime(r.Item("ShipmentDate"))
                End If
                m_DoExport = Convert.ToBoolean(r.Item("DoExport"))
                m_TotalDiscount = Convert.ToDouble(r.Item("TotalDiscount"))
                If IsDBNull(r.Item("RawPriceDiscountAmount")) Then
                    m_RawPriceDiscountAmount = Nothing
                Else
                    m_RawPriceDiscountAmount = Convert.ToDouble(r.Item("RawPriceDiscountAmount"))
                End If
                If IsDBNull(r.Item("CarrierType")) Then
                    m_CarrierType = Nothing
                Else
                    m_CarrierType = Convert.ToString(r.Item("CarrierType"))
                End If

                If IsDBNull(r.Item("Insurance")) Then
                    m_Insurance = Nothing
                Else
                    m_Insurance = Convert.ToDouble(r.Item("Insurance"))
                End If

                If IsDBNull(r.Item("Discount")) Then
                    m_Discount = Nothing
                Else
                    m_Discount = Convert.ToDouble(r.Item("Discount"))
                End If
                If IsDBNull(r.Item("TotalProductDiscount")) Then
                    m_TotalProductDiscount = Nothing
                Else
                    m_TotalProductDiscount = Convert.ToDouble(r.Item("TotalProductDiscount"))
                End If
                If IsDBNull(r.Item("LastExport")) Then
                    m_LastExport = Nothing
                Else
                    m_LastExport = Convert.ToDateTime(r.Item("LastExport"))
                End If
                If IsDBNull(r.Item("ShipToAddressType")) Then
                    m_ShipToAddressType = Nothing
                Else
                    m_ShipToAddressType = Convert.ToString(r.Item("ShipToAddressType"))
                End If
                If IsDBNull(r.Item("IsSignatureConfirmation")) Then
                    m_IsSignatureConfirmation = Nothing
                Else
                    m_IsSignatureConfirmation = Convert.ToBoolean(r.Item("IsSignatureConfirmation"))
                End If
                If IsDBNull(r.Item("IsSentConfirm")) Then
                    m_IsSentConfirm = True
                Else
                    m_IsSentConfirm = Convert.ToBoolean(r.Item("IsSentConfirm"))
                End If
                If IsDBNull(r.Item("SignatureConfirmation")) Then
                    m_SignatureConfirmation = Nothing
                Else
                    m_SignatureConfirmation = Convert.ToDouble(r.Item("SignatureConfirmation"))
                End If
                If IsDBNull(r.Item("ResidentialFee")) Then
                    m_ResidentialFee = Nothing
                Else
                    m_ResidentialFee = Convert.ToDouble(r.Item("ResidentialFee"))
                End If
                If IsDBNull(r.Item("SignatureDeclineCommnent")) Then
                    m_SignatureDeclineCommnent = Nothing
                Else
                    m_SignatureDeclineCommnent = Convert.ToString(r.Item("SignatureDeclineCommnent"))
                End If
                m_CheckoutPage = Convert.ToString(r.Item("CheckoutPage"))
                m_CreateSessionID = Convert.ToString(r.Item("CreateSessionID"))
                m_ProcessSessionID = Convert.ToString(r.Item("ProcessSessionID"))

                If IsDBNull(r.Item("PointMessage")) Then
                    m_PointMessage = Nothing
                Else
                    m_PointMessage = Convert.ToString(r.Item("PointMessage"))
                End If
                If IsDBNull(r.Item("TotalRewardPoint")) Then
                    m_TotalRewardPoint = Nothing
                Else
                    m_TotalRewardPoint = Convert.ToInt32(r.Item("TotalRewardPoint"))
                End If
                If IsDBNull(r.Item("PurchasePoint")) Then
                    m_PurchasePoint = Nothing
                Else
                    m_PurchasePoint = Convert.ToInt32(r.Item("PurchasePoint"))
                End If
                If IsDBNull(r.Item("TotalPurchasePoint")) Then
                    m_TotalPurchasePoint = Nothing
                Else
                    m_TotalPurchasePoint = Convert.ToDouble(r.Item("TotalPurchasePoint"))
                End If
                If IsDBNull(r.Item("PointLevelMessage")) Then
                    m_PointLevelMessage = Nothing
                Else
                    m_PointLevelMessage = Convert.ToString(r.Item("PointLevelMessage"))
                End If
                If IsDBNull(r.Item("PointAmountDiscount")) Then
                    m_PointAmountDiscount = Nothing
                Else
                    m_PointAmountDiscount = Convert.ToDouble(r.Item("PointAmountDiscount"))
                End If
                Try
                    If IsDBNull(r.Item("IsReview")) Then
                        m_IsReview = Nothing
                    Else
                        m_IsReview = Convert.ToBoolean(r.Item("IsReview"))
                    End If
                Catch ex As Exception
                    m_IsReview = Nothing
                End Try
                Try
                    If IsDBNull(r.Item("IsShowReviewCart")) Then
                        m_IsShowReviewCart = Nothing
                    Else
                        m_IsShowReviewCart = Convert.ToBoolean(r.Item("IsShowReviewCart"))
                    End If
                Catch ex As Exception
                    m_IsReview = Nothing
                End Try
                If IsDBNull(r.Item("TotalSpecialHandlingFee")) Then
                    m_TotalSpecialHandlingFee = Nothing
                Else
                    m_TotalSpecialHandlingFee = Convert.ToDouble(r.Item("TotalSpecialHandlingFee"))
                End If
                If IsDBNull(r.Item("HazardousMaterialFee")) Then
                    m_HazardousMaterialFee = Nothing
                Else
                    m_HazardousMaterialFee = Convert.ToDouble(r.Item("HazardousMaterialFee"))
                End If
                If IsDBNull(r.Item("BillingAddressId")) Then
                    m_BillingAddressId = Nothing
                Else
                    m_BillingAddressId = Convert.ToInt32(r.Item("BillingAddressId"))
                End If

                If IsDBNull(r.Item("ShippingAddressId")) Then
                    m_ShippingAddressId = Nothing
                Else
                    m_ShippingAddressId = Convert.ToInt32(r.Item("ShippingAddressId"))
                End If
                If IsDBNull(r.Item("IPLocation")) Then
                    m_IPLocation = Nothing
                Else
                    m_IPLocation = Convert.ToString(r.Item("IPLocation"))
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load
        Public Shared Function GetListOrder(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal condition As String, ByVal sortBy As String, ByVal sortExp As String, ByRef totalRecord As Integer) As StoreOrderCollection
            Dim co As New StoreOrderCollection
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim r As SqlDataReader
            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StoreOrder_GetListAdmin")
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                r = db.ExecuteReader(cmd)
                While r.Read
                    Dim o As StoreOrderRow = LoadListAdmin(r)
                    co.Add(o)
                End While
                Core.CloseReader(r)
                totalRecord = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "StoreOrder.vb > GetListOrder", "pageIndex: " & pageIndex & "<br>pageSize: " & pageSize & "<br>condition: " & condition & "<br>sortBy: " & sortBy & "<br>sortExp: " & sortExp & "<br>" & ex.ToString())
            End Try
            Return co
        End Function

        Public Shared Function LoadListAdmin(ByVal r As SqlDataReader) As StoreOrderRow
            Dim o As New StoreOrderRow
            Try
                If Not String.IsNullOrEmpty(r.Item("OrderId")) Then
                    o.OrderId = Convert.ToInt32(r.Item("OrderId"))
                Else
                    o.OrderId = Nothing
                End If
                Try
                    o.EbayOrderId = Convert.ToString(r.Item("EbayOrderId"))
                Catch
                    o.EbayOrderId = Nothing
                End Try
                Try
                    o.AmazonOrderId = Convert.ToString(r.Item("AmazonOrderId"))
                Catch
                    o.AmazonOrderId = Nothing
                End Try
                Try
                    o.OrderNo = Convert.ToString(r.Item("OrderNo"))
                Catch ex As Exception
                    o.OrderNo = Nothing
                End Try
                Try
                    o.PaypalStatus = Convert.ToInt16(r.Item("PaypalStatus"))
                Catch
                    o.PaypalStatus = Nothing
                End Try
                Try
                    o.BillToSalonName = Convert.ToString(r.Item("BillToSalonName"))
                Catch
                    o.BillToSalonName = Nothing
                End Try
                Try
                    o.BillToName = Convert.ToString(r.Item("BillToName"))
                Catch
                    o.BillToName = Nothing
                End Try

                Try
                    o.BillToName2 = Convert.ToString(r.Item("BillToName2"))
                Catch
                    o.BillToName2 = Nothing
                End Try

                Try
                    o.BillToCountry = Convert.ToString(r.Item("BillToCountry"))
                Catch ex As Exception
                    o.BillToCountry = Nothing
                End Try

                Try
                    o.BillToCounty = Convert.ToString(r.Item("BillToCounty"))
                Catch
                    o.BillToCounty = Nothing
                End Try

                Try
                    o.RemoteIP = Convert.ToString(r.Item("remoteip"))
                Catch ex As Exception
                    o.RemoteIP = Nothing
                End Try

                Try
                    o.Email = Convert.ToString(r.Item("Email"))
                Catch
                    o.Email = Nothing
                End Try

                Try
                    o.ProcessDate = Convert.ToDateTime(r.Item("ProcessDate"))
                Catch
                    o.ProcessDate = Nothing
                End Try


                If Not String.IsNullOrEmpty(r.Item("Total")) Then
                    o.Total = Convert.ToDouble(r.Item("Total"))
                Else
                    o.Total = Nothing
                End If
                Try
                    o.TransactionID = Convert.ToString(r.Item("TransactionID"))
                Catch
                    o.TransactionID = Nothing
                End Try

                Try
                    o.PaypalShipToAddress = Convert.ToString(r.Item("PaypalShipToAddress"))
                Catch
                    o.PaypalShipToAddress = Nothing
                End Try

                Try
                    o.MemberId = Convert.ToInt32(r.Item("MemberId"))
                Catch
                    o.MemberId = Nothing
                End Try

                Return o
            Catch ex As Exception
                Throw ex
            End Try
        End Function 'Load
        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreOrder (" _
                 & " OrderNo" _
                 & ",NavisionOrderNo" _
                 & ",CarrierType" _
                 & ",SellToCustomerId" _
                 & ",PromotionCode" _
                 & ",PromotionMessage" _
                 & ",Discount" _
                 & ",TotalProductDiscount" _
                 & ",BillToSalonName" _
                 & ",PaymentTermsCode" _
                 & ",BillToName" _
                 & ",BillToName2" _
                 & ",BillToCustomerId" _
                 & ",BillToAddress" _
                 & ",BillToAddress2" _
                 & ",BillToCity" _
                 & ",BillToCounty" _
                 & ",BillToZipcode" _
                 & ",BillToPhone" _
                 & ",BillToPhoneExt" _
                 & ",BillToDaytimePhone" _
                 & ",BillToDaytimePhoneExt" _
                 & ",BillToFax" _
                 & ",BillToCountry" _
                 & ",Email" _
                 & ",ShipToCode" _
                 & ",ShipToSalonName" _
                 & ",ShipToName" _
                 & ",ShipToName2" _
                 & ",ShipToAddress" _
                 & ",ShipToAddress2" _
                 & ",ShipToCity" _
                 & ",ShipToContact" _
                 & ",ShipToCounty" _
                 & ",ShipToZipcode" _
                 & ",ShipToPhone" _
                 & ",ShipToPhoneExt" _
                 & ",ShipToFax" _
                 & ",ShipToCountry" _
                 & ",CustomerPriceGroup" _
                 & ",PaymentType" _
                 & ",CardNumber" _
                 & ",CIDNumber" _
                 & ",CardTypeId" _
                 & ",ExpirationDate" _
                 & ",CardHolderName" _
                 & ",BaseSubTotal" _
                 & ",GiftWrapping" _
                 & ",SubTotal" _
                 & ",TotalDiscount" _
                 & ",RawPriceDiscountAmount" _
                 & ",Shipping" _
                 & ",Tax" _
                 & ",Total" _
                 & ",CreateDate" _
                 & ",ProcessDate" _
                 & ",Status" _
                 & ",SentDate" _
                 & ",RemoteIP" _
                 & ",IsPromotionValid" _
                 & ",IsFreeShipping" _
                 & ",Notes" _
                 & ",Comments" _
                 & ",OrderDate" _
                & ",PromotionDate" _
                 & ",PostingDate" _
                 & ",ShipmentDate" _
                 & ",DoExport" _
                 & ",LastExport" _
                 & ",IsReturned" _
                 & ",AccountType" _
                 & ",BankName" _
                 & ",RoutingNumber" _
                 & ",AccountNumber" _
                 & ",CheckNumber" _
                 & ",DLNumber" _
                 & ",IsSameAddress" _
                 & ",TaxExemptId" _
                 & ",ShipmentInsured" _
                 & ",ReferralCode" _
                 & ",CheckoutPage" _
                 & ",CreateSessionID" _
                 & ",ProcessSessionID" _
                 & ",ShipToAddressType" _
                 & ",IsSignatureConfirmation" _
                 & ",SignatureConfirmation" _
                 & ",ResidentialFee" _
                 & ",SignatureDeclineCommnent" _
                 & ",PointMessage" _
                 & ",PurchasePoint" _
                 & ",TotalPurchasePoint" _
                 & ",TotalRewardPoint" _
                 & ",PointLevelMessage" _
                 & ",PointAmountDiscount" _
                 & ") VALUES (" _
                 & m_DB.Quote(OrderNo) _
                 & "," & m_DB.Quote(NavisionOrderNo) _
                 & "," & m_DB.Number(CarrierType) _
                 & "," & m_DB.Quote(SellToCustomerId) _
                 & "," & m_DB.Quote(PromotionCode) _
                 & "," & m_DB.Quote(PromotionMessage) _
                 & "," & m_DB.Number(Discount) _
                 & "," & m_DB.Number(TotalProductDiscount) _
                 & "," & m_DB.Quote(BillToSalonName) _
                 & "," & m_DB.Quote(PaymentTermsCode) _
                 & "," & m_DB.NQuote(BillToName) _
                 & "," & m_DB.NQuote(BillToName2) _
                 & "," & m_DB.Quote(BillToCustomerId) _
                 & "," & m_DB.NQuote(BillToAddress) _
                 & "," & m_DB.NQuote(BillToAddress2) _
                 & "," & m_DB.NQuote(BillToCity) _
                 & "," & m_DB.Quote(BillToCounty) _
                 & "," & m_DB.Quote(BillToZipcode) _
                 & "," & m_DB.Quote(BillToPhone) _
                 & "," & m_DB.Quote(BillToPhoneExt) _
                 & "," & m_DB.Quote(BillToDaytimePhone) _
                 & "," & m_DB.Quote(BillToDaytimePhoneExt) _
                 & "," & m_DB.Quote(BillToFax) _
                 & "," & m_DB.Quote(BillToCountry) _
                 & "," & m_DB.Quote(Email) _
                 & "," & m_DB.Quote(ShipToCode) _
                 & "," & m_DB.NQuote(ShipToSalonName) _
                 & "," & m_DB.NQuote(ShipToName) _
                 & "," & m_DB.NQuote(ShipToName2) _
                 & "," & m_DB.NQuote(ShipToAddress) _
                 & "," & m_DB.NQuote(ShipToAddress2) _
                 & "," & m_DB.Quote(ShipToCity) _
                 & "," & m_DB.Quote(ShipToContact) _
                 & "," & m_DB.Quote(ShipToCounty) _
                 & "," & m_DB.Quote(ShipToZipcode) _
                 & "," & m_DB.Quote(ShipToPhone) _
                 & "," & m_DB.Quote(ShipToPhoneExt) _
                 & "," & m_DB.Quote(ShipToFax) _
                 & "," & m_DB.Quote(ShipToCountry) _
                 & "," & m_DB.Quote(CustomerPriceGroup) _
                 & "," & m_DB.Quote(PaymentType) _
                 & "," & m_DB.Quote(EncryptedCardNumber) _
                 & "," & m_DB.Quote(CIDNumber) _
                 & "," & m_DB.Number(CardTypeId) _
                 & "," & m_DB.Quote(ExpirationDate) _
                 & "," & m_DB.Quote(CardHolderName) _
                 & "," & m_DB.Number(BaseSubTotal) _
                 & "," & m_DB.Number(GiftWrapping) _
                 & "," & m_DB.Number(SubTotal) _
                 & "," & m_DB.Number(TotalDiscount) _
                 & "," & m_DB.Number(RawPriceDiscountAmount) _
                 & "," & m_DB.Number(Shipping) _
                 & "," & m_DB.Number(Tax) _
                 & "," & m_DB.Number(Total) _
                 & "," & m_DB.Quote(DateTime.Now) _
                 & "," & m_DB.Quote(ProcessDate) _
                 & "," & m_DB.Quote(Status) _
                 & "," & m_DB.Quote(SentDate) _
                 & "," & m_DB.Quote(RemoteIP) _
                 & "," & CInt(IsPromotionValid) _
                 & "," & CInt(IsFreeShipping) _
                 & "," & m_DB.Quote(Notes) _
                 & "," & m_DB.Quote(Comments) _
                 & "," & m_DB.Quote(OrderDate) _
                 & "," & m_DB.Quote(PromotionDate) _
                 & "," & m_DB.Quote(PostingDate) _
                 & "," & m_DB.Quote(ShipmentDate) _
                 & "," & CInt(DoExport) _
                 & "," & m_DB.Quote(LastExport) _
                 & "," & CInt(IsReturned) _
                 & "," & m_DB.Quote(AccountType) _
                 & "," & m_DB.Quote(BankName) _
                 & "," & m_DB.Quote(RoutingNumber) _
                 & "," & m_DB.Quote(AccountNumber) _
                 & "," & m_DB.Quote(CheckNumber) _
                 & "," & m_DB.Quote(DLNumber) _
                 & "," & CInt(IsSameAddress) _
                 & "," & m_DB.Quote(TaxExemptId) _
                 & "," & CInt(ShipmentInsured) _
                 & "," & m_DB.Quote(ReferralCode) _
                 & "," & m_DB.Quote(CheckoutPage) _
                 & "," & IIf(CreateSessionID = Nothing, "newid()", m_DB.Quote(CreateSessionID)) _
                 & "," & m_DB.Quote(ProcessSessionID) _
                 & "," & m_DB.Quote(ShipToAddressType) _
                 & "," & m_DB.Quote(IsSignatureConfirmation) _
                 & "," & m_DB.Quote(SignatureConfirmation) _
                 & "," & m_DB.Quote(ResidentialFee) _
                 & "," & m_DB.Quote(SignatureDeclineCommnent) _
                 & "," & m_DB.Quote(PointMessage) _
                 & "," & m_DB.Quote(PurchasePoint) _
                 & "," & m_DB.Quote(TotalPurchasePoint) _
                  & "," & m_DB.Quote(m_TotalRewardPoint) _
                 & "," & m_DB.Quote(PointLevelMessage) _
                 & "," & m_DB.Quote(PointAmountDiscount) _
                 & ")"

                Return SQL
            End Get
        End Property

        Private Function Quote(ByVal input As String) As String
            Dim str As String = input
            If String.IsNullOrEmpty(input) Then
                str = " IS NULL "
            Else
                str = " = " & DB.Quote(input)
            End If

            Return str
        End Function
        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            OrderId = m_DB.InsertSQL(InsertStatement)
            Return OrderId
        End Function
        Public Overridable Function UpdateCheckout(ByVal CheckOutGuest As Boolean) As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_StoreOrder_UpdateCheckout"
            Dim cmd As SqlCommand = DB.CreateCommand(sp)
            cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
            cmd.Parameters.Add(DB.InParam("PaymentType", SqlDbType.VarChar, 0, PaymentType))
            cmd.Parameters.Add(DB.InParam("CardNumber", SqlDbType.VarChar, 0, CardNumber))
            cmd.Parameters.Add(DB.InParam("CIDNumber", SqlDbType.VarChar, 0, CIDNumber))
            cmd.Parameters.Add(DB.InParam("CardHolderName", SqlDbType.VarChar, 100, CardHolderName))
            cmd.Parameters.Add(DB.InParam("CardTypeId", SqlDbType.Int, 0, CardTypeId))
            cmd.Parameters.Add(DB.InParam("ExpirationDate", SqlDbType.DateTime, 0, ExpirationDate))
            cmd.Parameters.Add(DB.InParam("Notes", SqlDbType.Text, 0, Notes))
            cmd.Parameters.Add(DB.InParam("Comments", SqlDbType.Text, 0, IIf(String.IsNullOrEmpty(Comments), String.Empty, Comments)))
            cmd.Parameters.Add(DB.InParam("SellToCustomerId", SqlDbType.Int, 0, SellToCustomerId))
            cmd.Parameters.Add(DB.InParam("DoExport", SqlDbType.Bit, 0, DoExport))
            cmd.Parameters.Add(DB.InParam("ProcessDate", SqlDbType.DateTime, 0, ProcessDate))
            'cmd.Parameters.Add(DB.InParam("OrderNo", SqlDbType.VarChar, 0, OrderNo))
            cmd.Parameters.Add(DB.InParam("ProcessSessionID", SqlDbType.VarChar, 0, ProcessSessionID))
            cmd.Parameters.Add(DB.InParam("IPLocation", SqlDbType.VarChar, 0, IPLocation))
            cmd.Parameters.Add(DB.InParam("ReferralCode", SqlDbType.VarChar, 0, ReferralCode))
            cmd.Parameters.Add(DB.InParam("CheckOutGuest", SqlDbType.Bit, 0, IIf(CheckOutGuest, True, False)))

            result = CInt(cmd.ExecuteScalar())
            Return result = 0
        End Function

        Public Overridable Function UpdatePaypal(ByVal first As Boolean) As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_StoreOrder_UpdatePayPal"
            Dim cmd As SqlCommand = DB.CreateCommand(sp)
            cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
            cmd.Parameters.Add(DB.InParam("PaypalStatus", SqlDbType.Int, 0, PaypalStatus))
            cmd.Parameters.Add(DB.InParam("PaymentType", SqlDbType.VarChar, 0, PaymentType))
            cmd.Parameters.Add(DB.InParam("SellToCustomerId", SqlDbType.Int, 0, SellToCustomerId))
            cmd.Parameters.Add(DB.InParam("DoExport", SqlDbType.Bit, 0, DoExport))
            cmd.Parameters.Add(DB.InParam("ProcessDate", SqlDbType.DateTime, 0, ProcessDate))
            cmd.Parameters.Add(DB.InParam("ProcessSessionID", SqlDbType.VarChar, 0, ProcessSessionID))
            cmd.Parameters.Add(DB.InParam("Comments", SqlDbType.Text, 0, IIf(String.IsNullOrEmpty(Comments), String.Empty, Comments)))
            cmd.Parameters.Add(DB.InParam("First", SqlDbType.Bit, 0, first)) 'Neu first=TRUE thi sinh ra OrderNo

            result = CInt(cmd.ExecuteScalar())
            Return result = 0
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreOrder SET " _
             & " OrderNo = " & m_DB.Quote(OrderNo) _
             & ",NavisionOrderNo = " & m_DB.Quote(NavisionOrderNo) _
             & ",SellToCustomerId = " & m_DB.Quote(SellToCustomerId) _
             & ",PaymentTermsCode = " & m_DB.Quote(PaymentTermsCode) _
             & ",PromotionCode = " & m_DB.Quote(PromotionCode) _
             & ",PromotionMessage = " & m_DB.Quote(PromotionMessage) _
             & ",Discount = " & m_DB.Quote(Discount) _
             & ",TotalProductDiscount = " & m_DB.Quote(TotalProductDiscount) _
            & ",BillToSalonName = " & m_DB.NQuote(BillToSalonName) _
            & ",BillToName = " & m_DB.NQuote(BillToName) _
            & ",CarrierType = " & m_DB.Number(CarrierType) _
            & ",BillToName2 = " & m_DB.NQuote(BillToName2) _
            & ",BillToCustomerId = " & m_DB.Quote(BillToCustomerId) _
            & ",BillToAddress = " & m_DB.NQuote(BillToAddress) _
            & ",BillToAddress2 = " & m_DB.NQuote(BillToAddress2) _
            & ",BillToCity = " & m_DB.NQuote(BillToCity) _
            & ",BillToCounty = " & m_DB.NQuote(BillToCounty) _
            & ",BillToZipcode = " & m_DB.Quote(BillToZipcode) _
            & ",BillToPhone = " & m_DB.NQuote(BillToPhone) _
            & ",BillToPhoneExt = " & m_DB.Quote(BillToPhoneExt) _
            & ",BillToDaytimePhone = " & m_DB.Quote(BillToDaytimePhone) _
            & ",BillToDaytimePhoneExt = " & m_DB.Quote(BillToDaytimePhoneExt) _
            & ",BillToFax = " & m_DB.Quote(BillToFax) _
            & ",BillToCountry = " & m_DB.Quote(BillToCountry) _
            & ",Email = " & m_DB.Quote(Email) _
            & ",ShipToCode = " & m_DB.Quote(ShipToCode) _
            & ",ShipToSalonName = " & m_DB.NQuote(ShipToSalonName) _
            & ",ShipToName = " & m_DB.NQuote(ShipToName) _
            & ",ShipToName2 = " & m_DB.NQuote(ShipToName2) _
            & ",ShipToAddress = " & m_DB.NQuote(ShipToAddress) _
            & ",ShipToAddress2 = " & m_DB.NQuote(ShipToAddress2) _
            & ",ShipToCity = " & m_DB.NQuote(ShipToCity) _
            & ",ShipToContact = " & m_DB.Quote(ShipToContact) _
            & ",ShipToCounty = " & m_DB.NQuote(ShipToCounty) _
            & ",ShipToZipcode = " & m_DB.Quote(ShipToZipcode) _
            & ",ShipToPhone = " & m_DB.NQuote(ShipToPhone) _
            & ",ShipToPhoneExt = " & m_DB.Quote(ShipToPhoneExt) _
            & ",ShipToFax = " & m_DB.Quote(ShipToFax) _
            & ",ShipToCountry = " & m_DB.Quote(ShipToCountry) _
            & ",CustomerPriceGroup = " & m_DB.Quote(CustomerPriceGroup) _
            & ",PaymentType = " & m_DB.Quote(PaymentType) _
            & ",CardNumber = " & m_DB.Quote(EncryptedCardNumber) _
            & ",CIDNumber = " & m_DB.Quote(CIDNumber) _
            & ",CardTypeId = " & m_DB.Number(CardTypeId) _
            & ",ExpirationDate = " & m_DB.Quote(ExpirationDate) _
            & ",CardHolderName = " & m_DB.Quote(CardHolderName) _
            & ",BaseSubTotal = " & m_DB.Number(BaseSubTotal) _
            & ",GiftWrapping = " & m_DB.Number(GiftWrapping) _
            & ",SubTotal = " & m_DB.Number(SubTotal) _
            & ",TotalDiscount = " & m_DB.Number(TotalDiscount) _
            & ",RawPriceDiscountAmount = " & m_DB.Number(RawPriceDiscountAmount) _
            & ",Shipping = " & m_DB.Number(Shipping) _
            & ",Tax = " & m_DB.Number(Tax) _
            & ",Total = " & m_DB.Number(Total) _
            & ",ProcessDate = " & m_DB.Quote(ProcessDate) _
            & ",Status = " & m_DB.Quote(Status) _
            & ",SentDate = " & m_DB.Quote(SentDate) _
            & ",RemoteIP = " & m_DB.Quote(RemoteIP) _
            & ",IsPromotionValid = " & CInt(IsPromotionValid) _
            & ",IsFreeShipping = " & CInt(IsFreeShipping) _
            & ",ShipmentInsured = " & CInt(ShipmentInsured) _
            & ",Notes = " & m_DB.NQuote(Notes) _
            & ",Comments = " & m_DB.NQuote(Comments) _
            & ",OrderDate = " & m_DB.Quote(OrderDate) _
            & ",PromotionDate = " & m_DB.Quote(PromotionDate) _
            & ",PostingDate = " & m_DB.Quote(PostingDate) _
            & ",ShipmentDate = " & m_DB.Quote(ShipmentDate) _
            & ",IsSameAddress = " & CInt(IsSameAddress) _
            & ",DoExport = " & CInt(DoExport) _
            & ",LastExport = " & m_DB.Quote(LastExport) _
            & ",IsReturned = " & CInt(IsReturned) _
            & ",AccountType = " & m_DB.Quote(AccountType) _
            & ",BankName = " & m_DB.Quote(BankName) _
            & ",RoutingNumber = " & m_DB.Quote(RoutingNumber) _
            & ",AccountNumber = " & m_DB.Quote(AccountNumber) _
            & ",CheckNumber = " & m_DB.Quote(CheckNumber) _
            & ",DLNumber = " & m_DB.Quote(DLNumber) _
            & ",TaxExemptId = " & m_DB.Quote(TaxExemptId) _
            & ",ReferralCode = " & m_DB.Quote(ReferralCode) _
            & ",CheckoutPage = " & m_DB.Quote(CheckoutPage) _
            & ",CreateSessionID = " & m_DB.Quote(CreateSessionID) _
            & ",ProcessSessionID = " & m_DB.Quote(ProcessSessionID) _
            & ",MemberId = " & m_DB.Quote(MemberId) _
            & ",PaypalStatus = " & CInt(PaypalStatus) _
            & ",ShipToAddressType = " & m_DB.Quote(ShipToAddressType) _
            & ",IsSignatureConfirmation = " & m_DB.Quote(IsSignatureConfirmation) _
            & ",SignatureConfirmation = " & m_DB.Quote(SignatureConfirmation) _
            & ",ResidentialFee = " & m_DB.Quote(ResidentialFee) _
            & ",SignatureDeclineCommnent = " & m_DB.Quote(SignatureDeclineCommnent) _
            & ",PointMessage = " & m_DB.Quote(PointMessage) _
            & ",PurchasePoint = " & m_DB.Quote(PurchasePoint) _
            & ",TotalPurchasePoint = " & m_DB.Quote(TotalPurchasePoint) _
            & ",PointLevelMessage = " & m_DB.Quote(PointLevelMessage) _
            & ",PointAmountDiscount = " & m_DB.Quote(PointAmountDiscount) _
            & ",TotalRewardPoint = " & m_DB.Quote(TotalRewardPoint) _
            & ",TotalSpecialHandlingFee = " & m_DB.Quote(TotalSpecialHandlingFee) _
            & ",HazardousMaterialFee = " & m_DB.Quote(HazardousMaterialFee) _
            & ",Insurance = " & m_DB.Quote(Insurance) _
            & ",BillingAddressId = " & m_DB.Quote(BillingAddressId) _
            & ",ShippingAddressId = " & m_DB.Quote(ShippingAddressId) _
            & ",IPLocation = " & m_DB.Quote(IPLocation) _
            & " WHERE OrderId = " & m_DB.Quote(OrderId)

            If Not String.IsNullOrEmpty(OrderNo) And PaymentType = "CC" Then
                Dim s As String = String.Empty

                s = String.Format("SELECT COUNT(OrderId) FROM StoreOrder WHERE OrderId {0} AND BillToCity {1} AND BillToCounty {2} AND BillToZipcode {3} AND ShipToCity {4} AND ShipToCounty {5} AND ShipToZipcode {6}", Quote(OrderId), Quote(BillToCity), Quote(BillToCounty), Quote(BillToZipcode), Quote(ShipToCity), Quote(ShipToCounty), Quote(ShipToZipcode))
                Dim i As Integer = m_DB.ExecuteScalar(s)

                If i <= 0 Then
                    Components.Email.SendError("ToError500", "[Urgent] Update OrderNo Wrong Address", "OrderNo = " & OrderNo & "<br>OrderId = " & OrderId & "<br>Url: " & Web.HttpContext.Current.Request.RawUrl & "<br>DateTime: " & Now.ToString() & "<br>SQL: " & SQL & "<br>s: " & s)
                    Throw New Exception("Order update wrong address after checkout. Please contact webmaster@nss.com")
                End If

            End If

            m_DB.ExecuteSQL(SQL)
        End Sub

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreOrder WHERE OrderId = " & m_DB.Quote(OrderId)
            m_DB.ExecuteSQL(SQL)
            ''Remove related cash point
            SQL = "DELETE FROM CashPoint WHERE OrderId = " & m_DB.Quote(OrderId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreOrderCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreOrder As StoreOrderRow)
            Me.List.Add(StoreOrder)
        End Sub

        Public Function Contains(ByVal StoreOrder As StoreOrderRow) As Boolean
            Return Me.List.Contains(StoreOrder)
        End Function

        Public Function IndexOf(ByVal StoreOrder As StoreOrderRow) As Integer
            Return Me.List.IndexOf(StoreOrder)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreOrder As StoreOrderRow)
            Me.List.Insert(Index, StoreOrder)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreOrderRow
            Get
                Return CType(Me.List.Item(Index), StoreOrderRow)
            End Get

            Set(ByVal Value As StoreOrderRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreOrder As StoreOrderRow)
            Me.List.Remove(StoreOrder)
        End Sub
    End Class

End Namespace

