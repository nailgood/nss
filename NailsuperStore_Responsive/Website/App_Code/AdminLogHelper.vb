Imports Microsoft.VisualBasic
Imports DataLayer
Imports Lucene.Net
Imports Lucene.Net.Analysis
Imports Lucene.Net.Analysis.Standard
Imports Lucene.Net.Documents
Imports Lucene.Net.Index
Imports Lucene.Net.QueryParsers
Imports Lucene.Net.Search
Imports Lucene.Net.Store
Imports Version = Lucene.Net.Util.Version
Imports System.IO

Namespace Components
    Public Class AdminLogHelper

#Region "Common"
        Public Shared ReadOnly Property AllowStoreItemLog() As String
            Get
                Dim result As String = ",CasePrice,ItemName,SKU,URLCode,PageTitle,OutsideUSPageTitle,MetaDescription,OutsideUSMetaDescription,MetaKeywords,"
                result = result & "Price,Weight,CaseQty,QtyOnHand,InventoryStockNotification,LowStockMsg,LowStockThreshold,"
                result = result & "IsRewardPoints,RewardPoints,MaximumQuantity,PriceDesc,Measurement,Image,ImageAltTag,"
                result = result & "ListDepartmentId,BrandId,ListCollectionId,ListToneId,ListShapeId,IsRushDelivery,RushDeliveryCharge,"
                result = result & "ShortDesc,LongDesc,ShortViet,LongViet,ShortFrench,LongFrench,ShortSpanish,LongSpanish,ShortKorea,LongKorea,AdditionalInfo,Specifications,ShippingInfo,"
                result = result & "Status,BODate,IsActive,IsFreeShipping,IsFreeSample,IsFeatured,IsEbayAllow,IsFlammable,IsBestSeller,IsHot,IsSpecialOrder,AcceptingOrder,"
                result = result & "IsTaxFree,ListPostingGroupCode,IsHazMat,IsOversize,ListBaseColorId,ListCusionColorId,ListLaminateColorId,IsFlatFee,OutsideUSPageTitle,OutsideUSMetaDescription,EbayShippingType"
                Return result
            End Get
        End Property

        Public Shared AdminLogSeparateChar As String = "|"
        Public Shared AdminLogSeparateCharNew As String = "**+|+**"

        Public Shared Function ConvertObjectUpdateToLogMesssageString(ByVal DB As Database, ByVal objectType As Utility.Common.ObjectType, ByVal obj1 As Object, ByVal obj2 As Object) As String
            Dim s As String = ""
            Dim allPropertyname As String = String.Empty
            Dim formatLog As String = "{0}" & AdminLogSeparateCharNew & "{1}" & AdminLogSeparateCharNew & "{2}[br]"
            For Each [property] As System.Reflection.PropertyInfo In obj1.[GetType]().GetProperties()
                Dim value1 As Object = [property].GetValue(obj1, Nothing)
                Dim value2 As Object = [property].GetValue(obj2, Nothing)
                Dim fieldName As String = [property].Name
                If Not (value1 Is Nothing) Then
                    If value1.ToString() = "Database" Then
                        Continue For
                    End If
                End If
                Try
                    If value1 Is Nothing Then
                        value1 = String.Empty
                    End If
                    If value2 Is Nothing Then
                        value2 = String.Empty
                    End If
                    If (value1.ToString().Trim() <> value2.ToString.Trim()) Then
                        ''s = s & "Change '" & fieldName & "' from '" & value1 & "' to '" & value2 & "'[br]"
                        s = s & String.Format(formatLog, fieldName, value1, value2)
                    End If
                Catch ex As Exception
                End Try
                allPropertyname = allPropertyname & "," & fieldName
            Next
            Return s
        End Function
        Public Shared Function ConvertObjectDeleteToLogMesssageString(ByVal entity As Object, ByVal objType As Utility.Common.ObjectType) As String
            Return ConvertObjectInsertToLogMesssageString(entity, objType)
        End Function
        Public Shared Function ConvertObjectInsertToLogMesssageString(ByVal entity As Object, ByVal objType As Utility.Common.ObjectType) As String
            Dim s As String = ""
            Dim formatLog As String = "{0}" & AdminLogSeparateCharNew & "{1}[br]"
            For Each [property] As System.Reflection.PropertyInfo In entity.[GetType]().GetProperties()
                Dim value As Object = [property].GetValue(entity, Nothing)
                Dim fieldName As String = [property].Name
                If Not (value Is Nothing) Then
                    If value.ToString() = "Database" Then
                        Continue For
                    End If
                End If
                Try
                    If Not (String.IsNullOrEmpty(value)) Then
                        s = s & String.Format(formatLog, fieldName, value)
                    End If
                Catch ex As Exception
                End Try
            Next
            Return s
        End Function
        Public Shared Function AllowLogProperty(ByVal propertyCode As String, ByVal objType As Utility.Common.ObjectType) As Boolean
            If (objType = Utility.Common.ObjectType.StoreItem) Then
                If Not (AllowStoreItemLog.Contains("," & propertyCode & ",")) Then
                    Return False
                End If
            ElseIf (objType = Utility.Common.ObjectType.TrackingNumber) Then
                If Not (AllowTrackingNumberLog.Contains("," & propertyCode & ",")) Then
                    Return False
                End If
            ElseIf (objType = Utility.Common.ObjectType.Department) Then
                If Not (AllowDepartmentLog.Contains("," & propertyCode & ",")) Then
                    Return False
                End If
            End If
            Return True
        End Function
        Public Shared Function ConvertMessageLogUpdateToText(ByVal messsage As String, ByVal ObjectType As Utility.Common.ObjectType, ByVal DB As Database) As String
            '' Return String.Empty
            Dim result As String = String.Empty
            If String.IsNullOrEmpty(messsage) Then
                Return String.Empty
            End If
            messsage = messsage.Replace("[br]", "\")        ''Dim arrMess As Array

            Dim arrMess As String() = messsage.Split("\")
            Dim msg As String = String.Empty
            For i As Integer = 0 To arrMess.Length - 1
                msg = arrMess(i).ToString()
                If (String.IsNullOrEmpty(msg)) Then
                    Continue For
                End If
                If Not msg.Contains("|") Then
                    Continue For
                End If
                result = result & FormatTextUpdatetoView(ObjectType, msg, DB, messsage)
            Next
            Return "<table cellspacing='1' cellpadding='3' class='tblLog'>" & result & "</table>"
        End Function
        Public Shared Function GetPropertyValueByCodeInMessageInsert(ByVal messsage As String, ByVal code As String) As String
            '' Return String.Empty
            messsage = messsage.Replace("[br]", "\")        ''Dim arrMess As Array
            Dim result As String = String.Empty
            Dim arrMess As String() = messsage.Split("\")
            Dim isLast As Boolean = True
            Dim msg As String = String.Empty
            For i As Integer = 0 To arrMess.Length - 1
                msg = arrMess(i).ToString()
                If msg.Contains(code & AdminLogSeparateCharNew) Then
                    msg = msg.Replace(code & AdminLogSeparateCharNew, "")
                    Return msg
                End If
            Next
            Return String.Empty
        End Function
        Public Shared Function GetPropertyValueByCodeInMessageUpdate(ByVal messsage As String, ByVal code As String) As String
            '' Return String.Empty
            messsage = messsage.Replace("[br]", "\")        ''Dim arrMess As Array
            Dim result As String = String.Empty
            Dim arrMess As String() = messsage.Split("\")
            Dim isLast As Boolean = True
            Dim msg As String = String.Empty
            For i As Integer = 0 To arrMess.Length - 1
                msg = arrMess(i).ToString()
                If msg.Contains(code & AdminLogSeparateCharNew) Then
                    msg = msg.Replace(code & AdminLogSeparateCharNew, "")
                    ''msg = msg.Replace(AdminLogSeparateCharNew, "\")
                    Dim indexChar As Integer = msg.IndexOf(AdminLogSeparateCharNew)
                    If indexChar > 0 Then
                        msg = msg.Substring(indexChar + AdminLogSeparateCharNew.Length, msg.Length - indexChar - AdminLogSeparateCharNew.Length)
                    End If
                    Return msg
                End If
            Next
            Return String.Empty
        End Function
        Public Shared Function ConvertMessageLogDeleteToText(ByVal messsage As String, ByVal ObjectType As Utility.Common.ObjectType, ByVal DB As Database) As String
            Return ConvertMessageLogInsertToText(messsage, ObjectType, DB)
        End Function
        Public Shared Function ConvertMessageLogInsertToText(ByVal messsage As String, ByVal ObjectType As Utility.Common.ObjectType, ByVal DB As Database) As String
           
            Dim tmpLogMsg As String = messsage
            If String.IsNullOrEmpty(messsage) Then
                Return String.Empty
            End If
            messsage = messsage.Replace("[br]", "\")        ''Dim arrMess As Array
            Dim result As String = String.Empty
            Dim arrMess As String() = messsage.Split("\")
            Dim isLast As Boolean = True
            Dim msg As String = String.Empty
            For i As Integer = 0 To arrMess.Length - 1
                isLast = True
                msg = arrMess(i).ToString()
                If String.IsNullOrEmpty(msg) Then
                    Continue For
                End If
                If Not msg.Contains("|") Then
                    Continue For
                End If
                Dim msgInsert As String = FormatTextInsertView(DB, ObjectType, msg, tmpLogMsg)
                If Not String.IsNullOrEmpty(msgInsert) Then
                    result = result & msgInsert
                End If
            Next
            Return "<table cellspacing='1' cellpadding='3' class='tblLog'>" & result & "</table>"

        End Function
        Public Shared Function ConvertMessageLogInsertToTextOrderByForm(ByVal messsage As String, ByVal ObjectType As Utility.Common.ObjectType, ByVal DB As Database) As String
            Dim tmpMessage As String = messsage
            If String.IsNullOrEmpty(messsage) Then
                Return String.Empty
            End If
            Dim listProperty As String = String.Empty
            If (ObjectType = Utility.Common.ObjectType.TrackingNumber) Then
                listProperty = AllowTrackingNumberLog
            ElseIf (ObjectType = Utility.Common.ObjectType.Department) Then
                listProperty = AllowDepartmentLog
            End If
            Dim removeProperty As String = String.Empty
            Dim arrProperty As String() = listProperty.Split(",")
            Dim result As String = String.Empty
            If arrProperty.Length > 0 Then
                Dim propertyCode As String = String.Empty
                For i As Integer = 0 To arrProperty.Length - 1
                    propertyCode = arrProperty(i)
                    If String.IsNullOrEmpty(propertyCode) Then
                        Continue For
                    End If

                    If Not String.IsNullOrEmpty(removeProperty) Then
                        tmpMessage = tmpMessage.Replace("\" & removeProperty & "\", "\")
                        tmpMessage = tmpMessage.Replace(removeProperty & "\", "\")
                    End If

                    tmpMessage = tmpMessage.Replace("[br]", "\")        ''Dim arrMess As Array
                    Dim arrMess As String() = tmpMessage.Split("\")
                    Dim isLast As Boolean = True
                    Dim msg As String = String.Empty
                    For j As Integer = 0 To arrMess.Length - 1
                        msg = arrMess(j).ToString()
                        If msg.Contains(propertyCode & AdminLogSeparateCharNew) Then
                            Dim msgInsert As String = FormatTextInsertView(DB, ObjectType, msg, messsage)
                            If Not String.IsNullOrEmpty(msgInsert) Then
                                result = result & msgInsert
                            End If
                            removeProperty = msg
                            Exit For
                        End If
                    Next
                Next
            End If
            Return "<table cellspacing='1' cellpadding='3' class='tblLog'>" & result & "</table>"
        End Function
        Private Shared Function RemoveFirstSeperateCharacter(ByVal value As String, ByVal ch As String) As String
            Dim indexOf As Integer = value.IndexOf(ch)
            If (indexOf = 0) Then
                value = value.Substring(1, value.Length - 1)
            End If
            Return value
        End Function
        Private Shared Function FormatTextUpdatetoView(ByVal ObjectType As Utility.Common.ObjectType, ByVal msg As String, ByVal DB As Database, ByVal fullLogmessage As String) As String
            Dim result As String = String.Empty
            If String.IsNullOrEmpty(msg) Then
                Return String.Empty
            End If
            ''MaximumQuantity|0|12
            Dim splitChar As String = ""
            If msg.Contains(AdminLogSeparateCharNew) Then
                splitChar = AdminLogSeparateCharNew
            Else
                splitChar = AdminLogSeparateChar
            End If

            Dim arrMess() As String = msg.Split(New String() {splitChar}, StringSplitOptions.None)

            ''Dim arrMess As String() = msg.Split("|")
            Dim propertyCode As String = String.Empty
            Dim value1 As String = String.Empty
            Dim value2 As String = String.Empty
            Try
                propertyCode = arrMess(0).ToString()
            Catch ex As Exception

            End Try
            Try
                value1 = arrMess(1).ToString()
            Catch ex As Exception

            End Try
            Try
                value2 = arrMess(2).ToString()
            Catch ex As Exception

            End Try
            Dim fromValue As String = String.Empty
            Dim toValue As String = String.Empty
            Dim propertyName As String = propertyCode
            If (ObjectType = Utility.Common.ObjectType.StoreItem Or ObjectType = Utility.Common.ObjectType.FreeSample) Then
                If Not AllowLogProperty(propertyCode, ObjectType) Then
                    Return String.Empty
                End If
                Dim resultFormat As String = ConvertMessageLogUpdateToTextStoreItem(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
                If propertyCode = "AcceptingOrder" Then
                    Dim html As String = String.Empty
                    Dim acceptingOrderText As String = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.AcceptingOrder)
                    Dim inStockText As String = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.InStock)

                    If toValue = CInt(Utility.Common.ItemAcceptingStatus.AcceptingOrder).ToString Then
                        If fromValue = CInt(Utility.Common.ItemAcceptingStatus.None).ToString Then
                            html = CreateViewLogUpdateHTML(acceptingOrderText, "False", "True")
                        Else
                            html = CreateViewLogUpdateHTML(acceptingOrderText, "False", "True") & CreateViewLogUpdateHTML(inStockText, "True", "False")
                        End If
                        ''html = CreateViewLogHTML()
                    ElseIf toValue = CInt(Utility.Common.ItemAcceptingStatus.InStock).ToString Then
                        If fromValue = CInt(Utility.Common.ItemAcceptingStatus.None).ToString Then
                            html = CreateViewLogUpdateHTML(inStockText, "False", "True")
                        Else
                            html = CreateViewLogUpdateHTML(inStockText, "False", "True") & CreateViewLogUpdateHTML(acceptingOrderText, "True", "False")
                        End If
                    Else
                        If fromValue = CInt(Utility.Common.ItemAcceptingStatus.AcceptingOrder).ToString Then
                            html = CreateViewLogUpdateHTML(acceptingOrderText, "True", "False")
                        Else
                            html = CreateViewLogUpdateHTML(inStockText, "True", "False")
                        End If
                    End If
                    Return html
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.MixMatch) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextMixMatch(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.MixMatchLine) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextMixMatchLine(DB, propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.OrderCoupon) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextOrderCoupon(DB, propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.ProductCoupon) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextProductCoupon(DB, propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.FreeGift) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextFreeGift(DB, propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.SalesPrice) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextSalesPrice(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.Member) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextMember(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.CashPoint) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextCashPoint(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.LandingPage) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextLandingPage(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.News Or ObjectType = Utility.Common.ObjectType.Blog) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextNews(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.ProductReview) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextProductReview(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.OrderReview) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextOrderReview(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.FlashBanner) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextFlashBanner(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.StripBanner) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextStripBanner(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.Sysparam) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextSysparam(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.Video) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextVideo(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.Category) Then
                Dim resultFormat As String = ConvertMessageLogUpdateToTextCategory(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.TrackingNumber) Then
                If Not AllowLogProperty(propertyCode, ObjectType) Then
                    Return String.Empty
                End If
                Dim resultFormat As String = ConvertMessageLogUpdateToTextTrackingNumber(propertyCode, value1, value2, propertyName, fromValue, toValue, fullLogmessage)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            ElseIf (ObjectType = Utility.Common.ObjectType.Department) Then
                If Not AllowLogProperty(propertyCode, ObjectType) Then
                    Return String.Empty
                End If
                Dim resultFormat As String = ConvertMessageLogUpdateToTextDepartment(propertyCode, value1, value2, propertyName, fromValue, toValue)
                If Not String.IsNullOrEmpty(resultFormat) Then
                    Return resultFormat
                End If
            Else
                fromValue = value1
                toValue = value2
            End If
            '' Return String.Format(formatResult, "'" & propertyName & "'", fromValue, toValue)
            'Dim classField As String = "field"
            'Dim classValue1 As String = "value1"
            'Dim classValue2 As String = "value2"
            'If Not String.IsNullOrEmpty(fromValue) Or Not String.IsNullOrEmpty(toValue) Then
            '    result = "<tr><td  width='180px' class='" & classField & "'>" & propertyName & ":</td><td class='" & classValue1 & "' width='200px'>" & fromValue & "</td><td  width='200px' class='" & classValue2 & "'>" & toValue & "</td></tr>"
            'End If
            'Return result
            Return CreateViewLogUpdateHTML(propertyName, fromValue, toValue)

        End Function
        Private Shared Function CreateViewLogUpdateHTML(ByVal propertyName As String, ByVal fromValue As String, ByVal toValue As String) As String
            Dim classField As String = "field"
            Dim classValue1 As String = "value1"
            Dim classValue2 As String = "value2"
            Dim result As String = String.Empty
            If Not String.IsNullOrEmpty(fromValue) Or Not String.IsNullOrEmpty(toValue) Then
                result = "<tr><td  width='180px' class='" & classField & "'>" & propertyName & ":</td><td class='" & classValue1 & "' width='200px'>" & fromValue & "</td><td  width='200px' class='" & classValue2 & "'>" & toValue & "</td></tr>"
            End If
            Return result
        End Function
        Private Shared Function FormatTextInsertView(ByVal DB As Database, ByVal ObjectType As Utility.Common.ObjectType, ByVal msg As String, ByVal fullLogMsg As String) As String
            Dim result As String = String.Empty
            If String.IsNullOrEmpty(msg) Then
                Return String.Empty
            End If
            ''MaximumQuantity|0|12
            Dim splitChar As String = ""
            If msg.Contains(AdminLogSeparateCharNew) Then
                splitChar = AdminLogSeparateCharNew
            Else
                splitChar = AdminLogSeparateChar
            End If

            Dim arrMess() As String = msg.Split(New String() {splitChar}, StringSplitOptions.None)

            ''Dim arrMess As String() = msg.Split("|")
            Dim propertyCode As String = arrMess(0).ToString()
            Dim value As String = arrMess(1).ToString()
            Dim returnValue As String = String.Empty
            Dim propertyName As String = propertyCode

            If (ObjectType = Utility.Common.ObjectType.StoreItem) Then
                If Not AllowLogProperty(propertyCode, ObjectType) Then
                    Return String.Empty
                End If
                If (propertyCode = "AcceptingOrder") Then
                    Dim html As String = String.Empty
                    If value = CInt(Utility.Common.ItemAcceptingStatus.AcceptingOrder).ToString Then
                        propertyName = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.InStock)
                        returnValue = "False"
                        html = CreateViewLogInsertHTML(propertyName, returnValue)
                        propertyName = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.AcceptingOrder)
                        returnValue = "True"
                        html = html & CreateViewLogInsertHTML(propertyName, returnValue)
                    ElseIf value = CInt(Utility.Common.ItemAcceptingStatus.InStock).ToString Then
                        propertyName = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.InStock)
                        returnValue = "True"
                        html = CreateViewLogInsertHTML(propertyName, returnValue)
                        propertyName = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.AcceptingOrder)
                        returnValue = "False"
                        html = html & CreateViewLogInsertHTML(propertyName, returnValue)
                    Else
                        propertyName = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.InStock)
                        returnValue = "False"
                        html = CreateViewLogInsertHTML(propertyName, returnValue)
                        propertyName = Utility.Common.ConvertItemAcceptingStatusToName(Utility.Common.ItemAcceptingStatus.AcceptingOrder)
                        returnValue = "False"
                        html = html & CreateViewLogInsertHTML(propertyName, returnValue)
                    End If
                    Return html
                End If
                ConvertMessageLogInsertToTextStoreItem(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.MixMatch) Then
                ConvertMessageLogInsertToTextMixMatch(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.MixMatchLine) Then
                ConvertMessageLogInsertToTextMixMatchLine(DB, propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.OrderCoupon) Then
                ConvertMessageLogInsertToTextOrderCoupon(DB, propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.ProductCoupon) Then
                ConvertMessageLogInsertToTextProductCoupon(DB, propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''     
            ElseIf (ObjectType = Utility.Common.ObjectType.FreeGift) Then
                ConvertMessageLogInsertToTextFreeGift(DB, propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''     
            ElseIf (ObjectType = Utility.Common.ObjectType.SalesPrice) Then
                ConvertMessageLogInsertToTextSalesPrice(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.Member) Then
                ConvertMessageLogInsertToTextMember(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.CashPoint) Then
                ConvertMessageLogInsertToTextCashPoint(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.LandingPage) Then
                ConvertMessageLogInsertToTextLandingPage(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.News Or ObjectType = Utility.Common.ObjectType.Blog) Then
                ConvertMessageLogInsertToTextNews(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''            returnValue = value
            ElseIf (ObjectType = Utility.Common.ObjectType.ProductReview) Then
                ConvertMessageLogInsertToTextProductReview(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''     
            ElseIf (ObjectType = Utility.Common.ObjectType.OrderReview) Then
                ConvertMessageLogInsertToTextOrderReview(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If '' 
            ElseIf (ObjectType = Utility.Common.ObjectType.FlashBanner) Then
                ConvertMessageLogInsertToTextFlashBanner(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If '' 
            ElseIf (ObjectType = Utility.Common.ObjectType.StripBanner) Then
                ConvertMessageLogInsertToTextStripBanner(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If '' 
            ElseIf (ObjectType = Utility.Common.ObjectType.Video) Then
                ConvertMessageLogInsertToTextVideo(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If '' 
            ElseIf (ObjectType = Utility.Common.ObjectType.Category) Then
                ConvertMessageLogInsertToTextCategory(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''
            ElseIf (ObjectType = Utility.Common.ObjectType.TrackingNumber) Then
                If Not AllowLogProperty(propertyCode, ObjectType) Then
                    Return String.Empty
                End If
                ConvertMessageLogInsertToTextTrackingNumber(propertyCode, value, propertyName, returnValue, fullLogMsg)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''
            ElseIf (ObjectType = Utility.Common.ObjectType.Department) Then
                If Not AllowLogProperty(propertyCode, ObjectType) Then
                    Return String.Empty
                End If
                ConvertMessageLogInsertToTextDepartment(propertyCode, value, propertyName, returnValue)
                If (String.IsNullOrEmpty(propertyName) And String.IsNullOrEmpty(returnValue)) Then
                    Return String.Empty
                End If ''
            Else
                returnValue = value
            End If
            If String.IsNullOrEmpty(returnValue) Then
                Return String.Empty
            End If
            ''Return propertyName & ": " & returnValue
            '' Return String.Format(formatResult, "'" & propertyName & "'", fromValue, toValue)
            'Dim classField As String = "field"
            'Dim classValue1 As String = "value1"
            'result = "<tr><td  width='180px' class='" & classField & "'>" & propertyName & ":</td><td class='" & classValue1 & "' width='406px'>" & returnValue & "</td></tr>"
            'Return result
            Return CreateViewLogInsertHTML(propertyName, returnValue)
        End Function
        Private Shared Function CreateViewLogInsertHTML(ByVal propertyName As String, ByVal returnValue As String) As String
            Dim classField As String = "field"
            Dim classValue1 As String = "value1"
            Dim result As String = "<tr><td  width='180px' class='" & classField & "'>" & propertyName & ":</td><td class='" & classValue1 & "' width='406px'>" & returnValue & "</td></tr>"
            Return result
        End Function
        Private Shared Function ConvertValueDate(ByVal value As String) As String
            ''1/1/0001 12:00:00 AM
            If (value.Contains("1/1/0001")) Then
                Return String.Empty
            End If
            Return value
        End Function
        Private Shared Function ConvertPropertyCodeToName(ByVal propertyCode As String, ByVal lstCode As String, ByVal lstName As String) As String
            Try
                Dim arrCode() As String = lstCode.Split(",")
                Dim arrName() As String = lstName.Split(",")
                Dim i As Integer = 0
                For Each code As String In arrCode
                    If (code.Trim().ToLower() = propertyCode.Trim().ToLower()) Then
                        Return arrName(i).ToString().Trim()
                    End If
                    i = i + 1
                Next
            Catch ex As Exception

            End Try

            Return propertyCode ' String.Empty

        End Function
#End Region

#Region "StoreItem"

        Private Shared Function ConvertMessageLogUpdateToTextStoreItem(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertStoreItemPropertyCodeToName(propertyCode)
            If (propertyCode = "ListDepartmentId") Then
                returnFromValue = GetListNameItemDepartment(fromValue)
                returnToValue = GetListNameItemDepartment(toValue)
                ''Return "Change Department from " & returnFromValue & " to " & returnToValue
            ElseIf (propertyCode = "Measurement") Then
                returnFromValue = ConvertMeasurementValue(fromValue)
                returnToValue = ConvertMeasurementValue(toValue)
            ElseIf (propertyCode = "NewUntil") Then
                returnFromValue = ConvertDateTimeValue(fromValue)
                returnToValue = ConvertDateTimeValue(toValue)
            ElseIf (propertyCode = "NewUntil") Then
                returnFromValue = ConvertDateTimeValue(fromValue)
                returnToValue = ConvertDateTimeValue(toValue)
            ElseIf (propertyCode = "BrandId") Then
                If (String.IsNullOrEmpty(fromValue)) Then
                    returnFromValue = String.Empty
                Else
                    returnFromValue = StoreBrandRow.GetBrandNameById(fromValue)
                End If
                If (String.IsNullOrEmpty(toValue)) Then
                    returnToValue = String.Empty
                Else
                    returnToValue = StoreBrandRow.GetBrandNameById(toValue)
                End If
            ElseIf (propertyCode = "ListCollectionId") Then
                If (String.IsNullOrEmpty(fromValue)) Then
                    returnFromValue = String.Empty
                Else
                    returnFromValue = StoreCollectionRow.GetCollectionNameById(fromValue)
                End If
                If (String.IsNullOrEmpty(toValue)) Then
                    returnToValue = String.Empty
                Else
                    returnToValue = StoreCollectionRow.GetCollectionNameById(toValue)
                End If
            ElseIf (propertyCode = "ListToneId") Then
                If (String.IsNullOrEmpty(fromValue)) Then
                    returnFromValue = String.Empty
                Else
                    returnFromValue = StoreToneRow.GetToneNameById(fromValue)
                End If
                If (String.IsNullOrEmpty(toValue)) Then
                    returnToValue = String.Empty
                Else
                    returnToValue = StoreToneRow.GetToneNameById(toValue)
                End If
            ElseIf (propertyCode = "ListShapeId") Then
                If (String.IsNullOrEmpty(fromValue)) Then
                    returnFromValue = String.Empty
                Else
                    returnFromValue = StoreShadeRow.GetShadeNameById(fromValue)
                End If
                If (String.IsNullOrEmpty(toValue)) Then
                    returnToValue = String.Empty
                Else
                    returnToValue = StoreShadeRow.GetShadeNameById(toValue)
                End If
            ElseIf (propertyCode = "Status") Then

                If (String.IsNullOrEmpty(fromValue)) Then
                    returnFromValue = String.Empty
                Else
                    returnFromValue = StoreItemStatusRow.GetStatusNameByCode(fromValue)
                End If
                If (String.IsNullOrEmpty(toValue)) Then
                    returnToValue = String.Empty
                Else
                    returnToValue = StoreItemStatusRow.GetStatusNameByCode(toValue)
                End If
            ElseIf (propertyCode = "BODate") Then
                returnFromValue = ConvertValueDate(fromValue)
                returnToValue = ConvertValueDate(toValue)
            ElseIf (propertyCode = "ListPostingGroupCode") Then
                returnFromValue = RemoveFirstSeperateCharacter(fromValue, ",")
                returnToValue = RemoveFirstSeperateCharacter(toValue, ",")
            ElseIf (propertyCode = "ListBaseColorId") Then
                returnFromValue = GetListNameItemBaseColor(fromValue)
                returnToValue = GetListNameItemBaseColor(toValue)
            ElseIf (propertyCode = "ListCusionColorId") Then
                returnFromValue = GetListNameItemCusionColor(fromValue)
                returnToValue = GetListNameItemCusionColor(toValue)
            ElseIf (propertyCode = "ListLaminateColorId") Then
                returnFromValue = GetListNameItemLaminateColor(fromValue)
                returnToValue = GetListNameItemLaminateColor(toValue)
            Else
                returnFromValue = fromValue
                returnToValue = toValue
            End If
            Return String.Empty
        End Function
        Private Shared Sub ConvertMessageLogInsertToTextStoreItem(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            
            propertyName = ConvertStoreItemPropertyCodeToName(propertyCode)
            If (propertyCode = "ListDepartmentId") Then
                returnValue = GetListNameItemDepartment(Value)
                '' Return "Department:" & returnValue
            ElseIf (propertyCode = "Measurement") Then
                returnValue = ConvertMeasurementValue(Value)
            ElseIf (propertyCode = "NewUntil") Then
                returnValue = ConvertDateTimeValue(Value)
            ElseIf (propertyCode = "NewUntil") Then
                returnValue = ConvertDateTimeValue(Value)
            ElseIf (propertyCode = "BrandId") Then
                If (String.IsNullOrEmpty(Value)) Then
                    returnValue = String.Empty
                Else
                    returnValue = StoreBrandRow.GetBrandNameById(Value)
                End If
            ElseIf (propertyCode = "ListCollectionId") Then
                If (String.IsNullOrEmpty(Value)) Then
                    returnValue = String.Empty
                Else
                    returnValue = StoreCollectionRow.GetCollectionNameById(Value)
                End If

            ElseIf (propertyCode = "ListToneId") Then
                If (String.IsNullOrEmpty(Value)) Then
                    returnValue = String.Empty
                Else
                    returnValue = StoreToneRow.GetToneNameById(Value)
                End If
            ElseIf (propertyCode = "ListShapeId") Then
                If (String.IsNullOrEmpty(Value)) Then
                    returnValue = String.Empty
                Else
                    returnValue = StoreShadeRow.GetShadeNameById(Value)
                End If
            ElseIf (propertyCode = "Status") Then
                If (String.IsNullOrEmpty(Value)) Then
                    returnValue = String.Empty
                Else
                    returnValue = StoreItemStatusRow.GetStatusNameByCode(Value)
                End If
            ElseIf (propertyCode = "BODate") Then
                returnValue = ConvertValueDate(Value)
            ElseIf (propertyCode = "ListPostingGroupCode") Then
                returnValue = RemoveFirstSeperateCharacter(Value, ",")
            ElseIf (propertyCode = "ListBaseColorId") Then
                returnValue = GetListNameItemBaseColor(Value)
            ElseIf (propertyCode = "ListCusionColorId") Then
                returnValue = GetListNameItemCusionColor(Value)
            ElseIf (propertyCode = "ListLaminateColorId") Then
                returnValue = GetListNameItemLaminateColor(Value)
            Else
                returnValue = Value
            End If

        End Sub
        Private Shared Function GetListNameItemDepartment(ByVal lstValue As String) As String
            If (String.IsNullOrEmpty(lstValue)) Then
                Return "''"
            End If
            If Not lstValue.Contains(",") Then
                lstValue = "," & lstValue
            End If
            Dim count As Integer = 0
            Dim arrValue As String() = lstValue.Split(",")
            Dim result As String = String.Empty
            Dim departmentName As String = String.Empty
            For Each departmentId As String In arrValue
                If (String.IsNullOrEmpty(departmentId)) Then
                    Continue For
                End If
                departmentName = StoreDepartmentRow.GetDepartmentNameByDepertmentId(departmentId)
                count = count + 1
                If (String.IsNullOrEmpty(result)) Then
                    result = "<ul class=""itemcategory""><li> " & departmentName & "</li>"
                Else
                    result = result & "<li>" & departmentName & "</li>"
                End If
            Next
            If (count < 2) Then
                Return departmentName
            End If
            Return result & "</ul>"
        End Function
        Private Shared Function GetListNameItemBaseColor(ByVal lstValue As String) As String
            If (String.IsNullOrEmpty(lstValue)) Then
                Return ""
            End If
            If Not lstValue.Contains(",") Then
                lstValue = "," & lstValue
            End If
            Dim count As Integer = 0
            Dim arrValue As String() = lstValue.Split(",")
            Dim result As String = String.Empty
            Dim colorName As String = String.Empty
            For Each colorId As String In arrValue
                If (String.IsNullOrEmpty(colorId)) Then
                    Continue For
                End If
                colorName = StoreBaseColorRow.GetBaseColorNameById(colorId)
                count = count + 1
                If (String.IsNullOrEmpty(result)) Then
                    result = "<ul class=""itemcategory""><li> " & colorName & "</li>"
                Else
                    result = result & "<li>" & colorName & "</li>"
                End If
            Next
            If (count < 2) Then
                Return colorName
            End If
            Return result & "</ul>"
        End Function
        Private Shared Function GetListNameItemCusionColor(ByVal lstValue As String) As String
            If (String.IsNullOrEmpty(lstValue)) Then
                Return ""
            End If
            If Not lstValue.Contains(",") Then
                lstValue = "," & lstValue
            End If
            Dim count As Integer = 0
            Dim arrValue As String() = lstValue.Split(",")
            Dim result As String = String.Empty
            Dim colorName As String = String.Empty
            For Each colorId As String In arrValue
                If (String.IsNullOrEmpty(colorId)) Then
                    Continue For
                End If
                colorName = StoreCusionColorRow.GetCusionColorNameById(colorId)
                count = count + 1
                If (String.IsNullOrEmpty(result)) Then
                    result = "<ul class=""itemcategory""><li> " & colorName & "</li>"
                Else
                    result = result & "<li>" & colorName & "</li>"
                End If
            Next
            If (count < 2) Then
                Return colorName
            End If
            If (count < 2) Then
                Return colorName
            End If
            Return result & "</ul>"
        End Function
        Private Shared Function GetListNameItemLaminateColor(ByVal lstValue As String) As String
            If (String.IsNullOrEmpty(lstValue)) Then
                Return ""
            End If
            If Not lstValue.Contains(",") Then
                lstValue = "," & lstValue
            End If
            Dim count As Integer = 0
            Dim arrValue As String() = lstValue.Split(",")
            Dim result As String = String.Empty
            Dim colorName As String = String.Empty
            For Each colorId As String In arrValue
                If (String.IsNullOrEmpty(colorId)) Then
                    Continue For
                End If
                colorName = StoreLaminateTrimRow.GetNameById(colorId)
                count = count + 1
                If (String.IsNullOrEmpty(result)) Then
                    result = "<ul class=""itemcategory""><li> " & colorName & "</li>"
                Else
                    result = result & "<li>" & colorName & "</li>"
                End If

            Next
            If (count < 2) Then
                Return colorName
            End If
            Return result & "</ul>"

        End Function
        Private Shared Function ConvertMeasurementValue(ByVal value As String) As String
            If (value = "0") Then
                Return "liquid"
            End If
            Return "solid"
        End Function
        Private Shared Function ConvertDateTimeValue(ByVal value As String) As String
            If (value = "1/1/0001 12:00:00 AM") Then
                Return ""
            End If
            Return value
        End Function
        Private Shared Function ConvertStoreItemPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "CasePrice,CaseQty,ListBaseColorId      ,ListCusionColorId      ,ListLaminateColorId,      Pricing,Promotions,Promotion,ListDepartmentId,ListPostingGroupCode           ,ListCollectionId,ListToneId,ListShapeId,FreeSampleArrange,IsVariance,PermissionBuyBrand,TaxGroupCode,IsSpecialOrder,AcceptingOrder ,IsHot,MaximumQuantity       ,HighPrice,LowPrice,HighSalePrice,LowSalePrice,Weight,BrandId,AdditionalInfo  ,Specifications ,ShippingInfo  ,HelpfulTips,ItemName2,Category,LastImport,LastExport,IsOversize,IsRushDelivery  ,LiftGateCharge,ScheduleDeliveryCharge,RushDeliveryCharge     ,IsHazMat           ,IsCollection,IsOnSale,IsFeatured ,IsFreeShipping ,IsFreeSample,ItemId,MixMatchId,MixMatchDescription,IsInCart,ItemGroupId,ItemName ,ItemType,Prefix,SKU,Price,SalePrice,PageTitle ,MetaDescription ,MetaKeywords ,IsActive  ,IsNew  ,IsBestSeller,DoExport,NewUntil,IsTaxFree  ,ShipmentDate,PriceDesc         ,Image,ImageAltTag   ,DeliveryTime,CarrierType,Status,InvMsgId,QtyOnHand  ,InventoryStockNotification  ,BODate        ,LowStockMsg      ,LowStockThreshold   ,QtyReserved,LastUpdated,ShortDesc        ,LongDesc         ,ShortViet                    ,LongViet                    ,ShortFrench              ,LongFrench              ,ShortSpanish              ,LongSpanish              ,ShortKorea,LongKorea,Size,CreateDate,ModifyDate,PromotionId,Measurement,Final,URLCode ,IsFreeGift,IsFlatFee,IsEbayAllow        ,FeeShipOversize,IsFlammable             ,IsEbay,EbayShippingType   ,IsRewardPoints   ,RewardPoints  ,CountReview,AverageReview,ShowPrice,OutsideUSPageTitle    ,OutsideUSMetaDescription"
            Dim lstName As String = "Case Price,Case Qty,Available Base Colors,Available Cusion Colors,Available Laminate Colors,Pricing,Promotions,Promotion,Department      ,Hide from these Posting Groups ,Collection      ,Tone      ,Shade      ,FreeSampleArrange,IsVariance,PermissionBuyBrand,TaxGroupCode,Special Order ,Accepting Order  ,Hot  ,Maximum Qty per Order ,HighPrice,LowPrice,HighSalePrice,LowSalePrice,Weight,Brand  ,Additional Info ,Specifications ,Shipping Info ,HelpfulTips,ItemName2,Category,LastImport,LastExport,Oversize  ,Is Rush Delivery,LiftGateCharge,ScheduleDeliveryCharge,Rush Delivery / Charge ,Flammable US ,     IsCollection, IsOnSale,Featured   ,Free Shipping  ,Free Sample ,ItemId,MixMatchId,MixMatchDescription,IsInCart,ItemGroupId,Item Name,ItemType,Prefix,SKU,Price,SalePrice,Page Title,Meta Description,Meta Keywords,Is Active ,Is New ,Best Seller ,DoExport,Until   ,Tax Exempt ,ShipmentDate,Price Description ,Image,Image Alt Tag ,DeliveryTime,CarrierType,Status,InvMsgId,Qty On Hand,Inventory Stock Notification,Backorder Date,Low Stock Message,Low Stock Threshold ,QtyReserved,LastUpdated,Short Description,Long Description ,Short Vietnamese Description ,Long Vietnamese Description ,Short French Description ,Long French Description ,Short Spanish Description ,Long Spanish Description ,ShortKorea,LongKorea,Size,CreateDate,ModifyDate,PromotionId,Measurement,Final,URL Code,IsFreeGift,IsFlatFee,Allow post to ebay ,FeeShipOversize,Flammable International ,IsEbay,Ebay Shipping Type ,Is Rewards point ,Rewards point ,CountReview,AverageReview,ShowPrice,Outside US Page Title ,Outside US Meta Description"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function

#End Region

#Region "MixMatch"
        Private Shared Sub ConvertMessageLogInsertToTextMixMatch(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertMixMatchPropertyCodeToName(propertyCode)

            returnValue = Value
            If (propertyCode = "Id") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertMixMatchPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "Id,CustomerPriceGroupId,StartingDate ,EndingDate , MixMatchNo  ,Description,IsActive ,DiscountType ,LinesToTrigger  ,TimesApplicable ,Mandatory,Optional,Type"
            Dim lstName As String = "Id,Customer Price Group,Starting Date,Ending Date, Mix Match No,Description,Is Active,Discount Type,Lines To Trigger,Times Applicable,Mandatory,Optional,Type"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextMixMatch(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertMixMatchPropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "CustomerPriceGroupId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "MixMatchLine"
        Private Shared Function ConvertMessageLogUpdateToTextMixMatchLine(ByVal DB As Database, ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertMixMatchLinePropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "MixMatchId") Then
                Dim mixMatchFrom As MixMatchRow = MixMatchRow.GetRow(DB, CInt(returnFromValue))
                returnFromValue = mixMatchFrom.MixMatchNo & " - " & mixMatchFrom.Description
                Dim mixMatchTo As MixMatchRow = MixMatchRow.GetRow(DB, CInt(returnToValue))
                returnToValue = mixMatchTo.MixMatchNo & " - " & mixMatchTo.Description
            ElseIf (propertyCode = "ItemId") Then
                Dim itemFrom As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(returnFromValue))
                returnFromValue = itemFrom.SKU & " - " & itemFrom.ItemName
                Dim itemTo As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(returnToValue))
                returnToValue = itemTo.SKU & " - " & itemTo.ItemName
            End If
            Return String.Empty
        End Function
        Private Shared Sub ConvertMessageLogInsertToTextMixMatchLine(ByVal DB As Database, ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertMixMatchLinePropertyCodeToName(propertyCode)
            If (propertyCode = "MixMatchId") Then
                Dim mixMatch As MixMatchRow = MixMatchRow.GetRow(DB, CInt(Value))
                returnValue = mixMatch.MixMatchNo & " - " & mixMatch.Description
            ElseIf (propertyCode = "ItemId") Then
                Dim item As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(Value))
                returnValue = item.SKU & " - " & item.ItemName
            ElseIf (propertyCode = "Id") Then
                returnValue = ""
            Else
                returnValue = Value
            End If
        End Sub
        Private Shared Function ConvertMixMatchLinePropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "LineNo ,MixMatchId  ,ItemId      ,DiscountType, Value,IsActive,Id"
            Dim lstName As String = "Line No,Mix Match   ,SKU/Product ,Discount Type, Value,IsActive,Id"

            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function

#End Region

#Region "Order Coupon"
        Private Shared Function ConvertMessageLogUpdateToTextOrderCoupon(ByVal DB As Database, ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            If (propertyCode = "PromotionId" Or propertyCode = "MinimumQuantityPurchase" Or propertyCode = "IsItemSpecific" Or propertyCode = "IsUsed" Or propertyCode = "IsProductCoupon" Or propertyCode = "IsTotalProduct" Or propertyCode = "IsActiveImage") Then
                Return String.Empty
            End If
            propertyName = ConvertOrderCouponPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "MaximumPurchase") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function

        Private Shared Sub ConvertMessageLogInsertToTextOrderCoupon(ByVal DB As Database, ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            If (propertyCode = "PromotionId" Or propertyCode = "MinimumQuantityPurchase" Or propertyCode = "IsItemSpecific" Or propertyCode = "IsUsed" Or propertyCode = "IsProductCoupon" Or propertyCode = "IsTotalProduct" Or propertyCode = "IsActiveImage") Then
                propertyName = ""
                returnValue = ""
                Exit Sub
            End If
            propertyName = ConvertOrderCouponPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "MaximumPurchase") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If

            End If
        End Sub
        Private Shared Function ConvertOrderCouponPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "PromotionId,PromotionName ,PromotionCode ,PromotionType ,Message,Discount,StartDate  ,EndDate ,MinimumPurchase ,MaximumPurchase ,MinimumQuantityPurchase,IsItemSpecific,IsFreeShipping         ,IsActive ,IsOneUse  ,IsUsed,IsRegisterSend  ,IsProductCoupon,IsActiveImage,IsTotalProduct,Image"
            Dim lstName As String = "PromotionId,Promotion Name,Promotion Code,Promotion Type,Message,Discount,Start Date ,End Date,Minimum Purchase,Maximum Purchase,MinimumQuantityPurchase,IsItemSpecific,Is Free Ground Shipping,Is Active,Is One Use,IsUsed,Is Register Send,IsProductCoupon,IsActiveImage,IsTotalProduct,Image"

            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function

#End Region

#Region "Product Coupon"
        Private Shared Function ConvertMessageLogUpdateToTextProductCoupon(ByVal DB As Database, ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            If (propertyCode = "IsProductCoupon" Or propertyCode = "IsActiveImage") Then
                Return String.Empty
            End If
            propertyName = ConvertProductCouponPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "MaximumPurchase") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function

        Private Shared Sub ConvertMessageLogInsertToTextProductCoupon(ByVal DB As Database, ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            If (propertyCode = "IsProductCoupon" Or propertyCode = "IsActiveImage") Then
                propertyName = ""
                returnValue = ""
                Exit Sub
            End If
            propertyName = ConvertProductCouponPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "MaximumPurchase") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If

            End If
        End Sub
        Private Shared Function ConvertProductCouponPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "SKU,PromotionId,PromotionName ,PromotionCode ,PromotionType ,Message,Discount,StartDate  ,EndDate ,MinimumPurchase ,MaximumPurchase ,MinimumQuantityPurchase,IsItemSpecific,IsFreeShipping         ,IsActive ,IsSingleUse  ,IsUsed,IsRegisterSend  ,IsProductCoupon,IsActiveImage,IsTotalProduct,Image  ,IsOneUse"
            Dim lstName As String = "SKU,PromotionId,Promotion Name,Promotion Code,Promotion Type,Message,Discount,Start Date ,End Date,Min Dollards    ,Max Dollards    ,Min Quantity           ,IsItemSpecific,Is Free Ground Shipping,Is Active,Is Single Use,IsUsed,Is Register Send,IsProductCoupon,IsActiveImage,Is Total Product,Image,Is One Use"

            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function

#End Region

#Region "Free Gift "
        Private Shared Function ConvertMessageLogUpdateToTextFreeGift(ByVal DB As Database, ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String

            propertyName = ConvertFreeGiftPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "MinimumAmount") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "ItemId") Then
                Dim itemFrom As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(returnFromValue))
                returnFromValue = itemFrom.SKU & " - " & itemFrom.ItemName
                Dim itemTo As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(returnToValue))
                returnToValue = itemTo.SKU & " - " & itemTo.ItemName
            End If
            Return String.Empty
        End Function

        Private Shared Sub ConvertMessageLogInsertToTextFreeGift(ByVal DB As Database, ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)

            propertyName = ConvertFreeGiftPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "MinimumAmount") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ItemId") Then
                Dim item As StoreItemRow = StoreItemRow.GetRowLogAdminById(CInt(Value))
                returnValue = item.SKU & " - " & item.ItemName
            End If
        End Sub
        Private Shared Function ConvertFreeGiftPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "FreeGiftId,Image,Banner,ItemId,MinimumAmount,IsActive,IsAddCart"
            Dim lstName As String = "FreeGiftId,Image,Banner,Item  ,Minimum Amount,Is Active,Add to cart"


            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function

#End Region

#Region "SalesPrice"
        Private Shared Sub ConvertMessageLogInsertToTextSalesPrice(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertSalesPricePropertyCodeToName(propertyCode)

            returnValue = Value
            If (propertyCode = "SalesPriceId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "CustomerPriceGroupId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "MemberId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertSalesPricePropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "SalesPriceId,ItemId,MemberId,CustomerPriceGroupId,StartingDate,EndingDate,UnitPrice,MinimumQuantity,PriceGroupDescription,Image,IsActive,PriceIncludesVAT,AllowInvoiceDisc,SalesType,AllowLineDisc,UnitPriceIncludingVAT,ItemName"
            Dim lstName As String = "SalesPriceId,ItemId,MemberId,Customer Price Group,Starting Date,Ending Date,Sale Price,Minimum Quantity,Price Group Description,Image,Is Active,Price Includes VAT,Allow Invoice Disc,Sales Type,Allow Line Disc,Unit Price Including VAT, Item Name"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextSalesPrice(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertSalesPricePropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "CustomerPriceGroupId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "MemberId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "Member"
        Private Shared Sub ConvertMessageLogInsertToTextMember(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertMemberPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "MemberId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "MemberNavigationString") Then
                returnValue = ""
            ElseIf (propertyCode = "MemberTypeId") Then
                returnValue = MemberTypeRow.GetMemberTypeById(returnValue)
            ElseIf (propertyCode = "EncryptedPassword") Then
                returnValue = ""
            ElseIf (propertyCode = "LicenseExpirationDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ExpectedGraduationDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "AuthorizedSignatureDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "CreateDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ModifyDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertMemberPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "MemberId,Username,Password,LicenseNumber,LicenseState,StudentNumber,SchoolName,MemberTypeId,ContactName,ContactPhone,SalesTaxExemptionNumber,AuthorizedSignatureName,AuthorizedSignatureTitle,LicenseExpirationDate,ExpectedGraduationDate,AuthorizedSignatureDate,IsSameDefaultAddress,IsActive,DeActive,DeptOfRevenueRegistered,ResaleAcceptance,InformationAccuracyAgreement,CanPostJob,CanPostClassified,CustomerId,LastOrderId,ProfessionalStatus,ReferCode,PercentageDiscount,CreateDate,ModifyDate,IsInternational,MemberNavigationString,EncryptedPassword"
            Dim lstName As String = "MemberId,Username,Password,License Number,License State,Student Number,School Name,Member Type,Contact Name,Contact Phone,Sales Tax Exemption Number,Authorized Signature Name,Authorized Signature Title,License Expiration Date,Expected Graduation Date,Authorized Signature Date,IsSameDefaultAddress,Is Active,De Active,Dept OfRevenue Registered,Resale Acceptance,Information Accuracy Agreement,Can Post Job,Can Post Classified,CustomerId,Last OrderId,Professional Status,Refer Code,Percentage Discount,Create Date,Modify Date,Is International,Member Navigation String,Encrypted Password"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextMember(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertMemberPropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "MemberTypeId") Then
                returnFromValue = MemberTypeRow.GetMemberTypeById(fromValue)
                returnToValue = MemberTypeRow.GetMemberTypeById(toValue)
            ElseIf (propertyCode = "EncryptedPassword") Then
                returnFromValue = ""
                returnToValue = ""
            ElseIf (propertyCode = "LicenseExpirationDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "ExpectedGraduationDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "AuthorizedSignatureDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "CreateDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "ModifyDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "CashPoint"
        Private Shared Sub ConvertMessageLogInsertToTextCashPoint(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertCashPointPropertyCodeToName(propertyCode)

            returnValue = Value
            If (propertyCode = "CashPointId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ModifyDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ApproveDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertCashPointPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "CashPoitntId,MemberId,OrderId,PointEarned,Status,PointDebit,Notes,CreateDate,ModifyDate,ApproveDate,AdminId,TotalPointAvailable,PointPending,PointDebitinMonth,PointsaccumulatedinMonth,PointsaccumulatedinYear,Pointsearneduptodate,Pointsdebituptodate,Amount"
            Dim lstName As String = "CashPoitntId,MemberId,OrderId,Point Earned,Status,Point Debit,Notes,Create Date,Modify Date,Approve Date,AdminId,Total Point Available,Point Pending,Point Debit in Month,Points accumulated in Month,Points accumulated in Year,Pointsearneduptodate,Pointsdebituptodate,Amount"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextCashPoint(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertCashPointPropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "ModifyDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "ApproveDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "News & events"
        Private Shared Sub ConvertMessageLogInsertToTextNews(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertNewsPropertyCodeToName(propertyCode)
            If (propertyCode = "NewsId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                Else
                    returnValue = Value
                End If
            ElseIf (propertyCode = "CategoryId") Then
                returnValue = ""
            ElseIf (propertyCode = "ListCategoryId") Then
                returnValue = GetListCategoryName(Value)
            ElseIf (propertyCode = "TotalRow") Then
                returnValue = ""
            ElseIf (propertyCode = "PageIndex") Then
                returnValue = ""
            ElseIf (propertyCode = "PageSize") Then
                returnValue = ""
            ElseIf (propertyCode = "CreatedDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "Description") Then
                returnValue = ConvertLinkVideo(Value)
            Else
                returnValue = Value
            End If

        End Sub
        Private Shared Function ConvertNewsPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "NewsId,CategoryId,ThumbImage,Arrange,IsActive,MetaDescription,MetaKeyword,PageTitle,Title,ShortDescription,Description,CreatedDate,IsFacebook,ListCategoryId"
            Dim lstName As String = "NewsId,CategoryId,Thumb Image,Arrange,Is Active,Meta Description,Meta Keyword,Page Title,Title,Short Description,Description,Created Date,Is Facebook,Category"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextNews(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertNewsPropertyCodeToName(propertyCode)

            If (propertyCode = "CategoryId") Then
                returnFromValue = ""
                returnToValue = ""
            ElseIf (propertyCode = "ListCategoryId") Then
                returnFromValue = GetListCategoryName(fromValue)
                returnToValue = GetListCategoryName(toValue)
            ElseIf (propertyCode = "CreatedDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "Description") Then
                returnFromValue = ConvertLinkVideo(fromValue)
                returnToValue = ConvertLinkVideo(toValue)
            Else
                returnFromValue = fromValue
                returnToValue = toValue
            End If
            Return String.Empty
        End Function
        Private Shared Function GetListCategoryName(ByVal lstValue As String) As String
            If (String.IsNullOrEmpty(lstValue)) Then
                Return "''"
            End If
            If Not lstValue.Contains(",") Then
                lstValue = "," & lstValue
            End If
            Dim count As Integer = 0
            Dim arrValue As String() = lstValue.Split(",")
            Dim result As String = String.Empty
            Dim CategoryName As String = String.Empty
            For Each CategoryId As String In arrValue
                If (String.IsNullOrEmpty(CategoryId)) Then
                    Continue For
                End If
                CategoryName = CategoryRow.GetCategoryNameByCategoryId(CategoryId)
                count = count + 1
                If (String.IsNullOrEmpty(result)) Then
                    result = "<ul class=""itemcategory""><li> " & CategoryName & "</li>"
                Else
                    result = result & "<li>" & CategoryName & "</li>"
                End If
            Next
            If (count < 2) Then
                Return CategoryName
            End If
            Return result & "</ul>"
        End Function

        Private Shared Function ConvertLinkVideo(ByVal objVideo As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(objVideo)) Then
                Dim VideoTag As String = "object"
                Dim src As String = objVideo
                Dim oTag As Integer = src.IndexOf("<" & VideoTag)
                Dim tmp As String = String.Empty
                Dim tmp2 As String = String.Empty
                Dim IsIframe As Boolean = False
                If oTag < 0 Then
                    IsIframe = True
                    VideoTag = "iframe"
                    oTag = src.IndexOf("<" & VideoTag)
                End If
                While oTag >= 0
                    result &= src.Substring(0, oTag)
                    tmp = src.Substring(oTag)
                    Dim cTag As Integer = tmp.IndexOf("</" & VideoTag & ">")
                    Dim compare As String = String.Empty
                    Dim link As String = String.Empty
                    Dim w As Integer = 502
                    Dim h As Integer = 282
                    If cTag >= 0 Then
                        compare = tmp.Substring(0, cTag + 9)
                        compare = compare.Replace("'", """")
                        tmp2 = tmp.Substring(cTag + 9)
                        If compare.ToLower().Contains("youtube.com/") Or compare.ToLower().Contains("/embed/how-to-video/") Then
                            ''Dim i As Integer = compare.IndexOf("data=""http://")
                            Dim i As Integer = -1
                            Dim len As Integer = 0
                            If IsIframe Then
                                i = compare.IndexOf("src=""")
                                len = 5
                            Else
                                i = compare.IndexOf("data=""")
                                len = 6
                            End If

                            If i >= 0 Then
                                link = compare.Substring(i + len)
                                i = link.IndexOf("""")
                                If i > 0 Then
                                    link = link.Substring(0, i)
                                End If
                            ElseIf Not IsIframe Then
                                Dim s As String = "<param name=""src"" value="""
                                i = compare.IndexOf(s)
                                If i >= 0 Then
                                    link = compare.Substring(i + s.Length)
                                    i = link.IndexOf("""")
                                    If i > 0 Then
                                        link = link.Substring(0, i)
                                    End If
                                End If
                            End If
                            Try
                                Dim cw As Integer = compare.IndexOf("width=""")
                                Dim ch As Integer = compare.IndexOf("height=""")
                                Dim size As String = String.Empty
                                If cw >= 0 Then
                                    size = compare.Substring(cw + 7)
                                    cw = size.IndexOf("""")
                                    If cw > 0 Then
                                        w = Convert.ToInt32(size.Substring(0, cw))
                                    End If
                                End If
                                If ch >= 0 Then
                                    size = compare.Substring(ch + 8)
                                    ch = size.IndexOf("""")
                                    If ch > 0 Then
                                        h = Convert.ToInt32(size.Substring(0, ch))
                                    End If
                                End If
                            Catch ex As Exception
                                w = 502
                                h = 282
                            End Try
                        End If
                    End If
                    src = tmp2
                    oTag = src.IndexOf("<" & VideoTag)
                    If oTag >= 0 Then
                        tmp2 = String.Empty
                    End If

                    If String.IsNullOrEmpty(link) Then
                        result &= compare & tmp2
                    Else
                        result &= "[video width='" & w & "' height='" & h & "'] <a href='" & link.Trim() & "' target='_blank'>" & link.Trim() & "</a> [/video]" & tmp2
                    End If

                    If oTag < 0 AndAlso Not IsIframe Then
                        src = result
                        IsIframe = True
                        VideoTag = "iframe"
                        oTag = src.IndexOf("<" & VideoTag)
                        If oTag >= 0 Then
                            tmp2 = String.Empty
                            result = String.Empty
                        End If
                    End If
                End While
            End If
            If String.IsNullOrEmpty(result) Then
                result = objVideo
            End If
            Return result
        End Function

#End Region

#Region "LandingPage"
        Private Shared Sub ConvertMessageLogInsertToTextLandingPage(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertLandingPagePropertyCodeToName(propertyCode)

            returnValue = Value
            If (propertyCode = "Id") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ItemId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "CustomerPriceGroupId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "GoogleABCode") Then
                returnValue = ""
            End If

        End Sub
        Private Shared Function ConvertLandingPagePropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "Id,Title,URLCode,ItemId,CustomerPriceGroupId,FileLocation,StartingDate,EndingDate,IsActive,PageTitle,MetaDescription,MetaKeywords,GoogleABCode,UrlReturn"
            Dim lstName As String = "Id,Title,URL Code,Item Id,Customer Price Group,File Location,Starting Date,Ending Date,Is Active,Page Title,Meta Description,Meta Keywords,Google A/B Code,URL Return"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextLandingPage(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertLandingPagePropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "ItemId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "CustomerPriceGroupId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "GoogleABCode") Then
                returnFromValue = ""
                returnToValue = ""
            End If
            Return String.Empty
        End Function
#End Region

#Region "Product Review"

        Private Shared Sub ConvertMessageLogInsertToTextProductReview(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertProductReviewPropertyCodeToName(propertyCode)

            returnValue = Value
            If (propertyCode = "ReviewId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            End If

        End Sub
        Private Shared Function ConvertProductReviewPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "ReviewId,Content,TypeReview,TypeReply,CreateDate,IsActive,IsFacebook,AddPoint,ItemId,MemberId,CartItemId,ReviewTitle,ReviewerName,NumStars,DateAdded,IsRecommendFriend,IsFeatured,IsExported,Comment,IsAddPoint,ItemName,Image,URLCode"
            Dim lstName As String = "ReviewId,Admin Reply,Type Review,Type Reply,Create Date,Is Active,Is Facebook,Add Point,Item Id,Member Id,Cart Item Id,Review Title,Reviewer Name,Num Stars,Date Added,Is Recommend Friend,Is Featured,Is Exported,Comment,Is Add Point,Item Name,Image,URL Code"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextProductReview(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertProductReviewPropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "ReviewId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "TypeReview") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                Else
                    returnFromValue = [Enum].Parse(GetType(Utility.Common.TypeReview), returnFromValue).ToString()
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                Else
                    returnToValue = [Enum].Parse(GetType(Utility.Common.TypeReview), returnToValue).ToString()
                End If
            ElseIf (propertyCode = "TypeReply") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                Else
                    returnFromValue = [Enum].Parse(GetType(Utility.Common.TypeReply), returnFromValue).ToString()
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                Else
                    returnToValue = [Enum].Parse(GetType(Utility.Common.TypeReply), returnToValue).ToString()
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "Order Review"
        Private Shared Sub ConvertMessageLogInsertToTextOrderReview(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertOrderReviewPropertyCodeToName(propertyCode)
            returnValue = Value
        End Sub
        Private Shared Function ConvertOrderReviewPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "OrderId,ItemArrived,ItemDescribed,ServicePrompt,NumStars,Comment,IsShared,IsActive,AddPoint,CreateDate,TypeReply,TypeReview,ReviewId,Content,Name,OrderNo,MemberId,DateAdded,IsFacebook"
            Dim lstName As String = "OrderId,Item Arrived,Item Described,Service Prompt,Num Stars,Comment,Is Shared,Is Active,Add Point,Create Date,Type Reply,Type Review,ReviewId,Admin Reply,Name,Order No,Member Id,Date Added,Is Facebook"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextOrderReview(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertOrderReviewPropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "ReviewId") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "TypeReview") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                Else
                    returnFromValue = [Enum].Parse(GetType(Utility.Common.TypeReview), returnFromValue).ToString()
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                Else
                    returnToValue = [Enum].Parse(GetType(Utility.Common.TypeReview), returnToValue).ToString()
                End If
            ElseIf (propertyCode = "TypeReply") Then
                If (returnFromValue = "0") Then
                    returnFromValue = ""
                Else
                    returnFromValue = [Enum].Parse(GetType(Utility.Common.TypeReply), returnFromValue).ToString()
                End If
                If (returnToValue = "0") Then
                    returnToValue = ""
                Else
                    returnToValue = [Enum].Parse(GetType(Utility.Common.TypeReply), returnToValue).ToString()
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "Flash Banner"
        Private Shared Sub ConvertMessageLogInsertToTextFlashBanner(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertFlashBannerPropertyCodeToName(propertyCode)
            returnValue = Value
            If propertyCode = "DepartmentId" Then
                If Value = "0" Then
                    returnValue = ""
                Else
                    returnValue = StoreDepartmentRow.GetDepartmentNameByDepertmentId(Value)
                End If
            ElseIf (propertyCode = "CreateDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "ModifyDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertFlashBannerPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "Id,BannerName,MobileBannerName,Url,DepartmentId,IsActive,SortOrder,CreateDate,ModifyDate,StartingDate,EndingDate"
            Dim lstName As String = "Id,Banner Name,Mobile Banner Name,Url,Department,Is Active,Sort Order,Create Date,Modify Date,Starting Date,Ending Date"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextFlashBanner(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertFlashBannerPropertyCodeToName(propertyCode)

            returnFromValue = fromValue
            returnToValue = toValue
            If propertyCode = "DepartmentId" Then
                If fromValue = "0" Then
                    returnFromValue = ""
                Else
                    returnFromValue = StoreDepartmentRow.GetDepartmentNameByDepertmentId(fromValue)
                End If
                If toValue = "0" Then
                    returnToValue = ""
                Else
                    returnToValue = StoreDepartmentRow.GetDepartmentNameByDepertmentId(toValue)
                End If
            ElseIf (propertyCode = "CreateDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "ModifyDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function
#End Region

#Region "Strip Banner"
        Private Shared Sub ConvertMessageLogInsertToTextStripBanner(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertStripBannerPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "Type") Then
                returnValue = GetTypeStripBanner(Value)
            ElseIf (propertyCode = "DepartmentID") Then
                If returnValue = "0" Then
                    returnValue = ""
                Else
                    returnValue = StoreDepartmentRow.GetDepartmentNameByDepertmentId(Value)
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnValue = DateTime.MinValue.ToString()) Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertStripBannerPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "Id,SubTitle,MainTitle,Image,IsActive,StartingDate,EndingDate,DepartmentID,Type,LinkPage,TextHtml"
            Dim lstName As String = "Id,Sub Title,Main Title,Image,Is Active,Starting Date,Ending Date,Department,Type,Link Page,Text Html"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextStripBanner(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertStripBannerPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "Type") Then
                returnFromValue = GetTypeStripBanner(fromValue)
                returnToValue = GetTypeStripBanner(toValue)
            ElseIf (propertyCode = "DepartmentID") Then
                If returnFromValue = "0" Then
                    returnFromValue = ""
                Else
                    returnFromValue = StoreDepartmentRow.GetDepartmentNameByDepertmentId(fromValue)
                End If
                If returnToValue = "0" Then
                    returnToValue = ""
                Else
                    returnToValue = StoreDepartmentRow.GetDepartmentNameByDepertmentId(toValue)
                End If
            ElseIf (propertyCode = "StartingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            ElseIf (propertyCode = "EndingDate") Then
                If (returnFromValue = DateTime.MinValue.ToString()) Then
                    returnFromValue = ""
                End If
                If (returnToValue = DateTime.MinValue.ToString()) Then
                    returnToValue = ""
                End If
            End If
            Return String.Empty
        End Function
        Public Shared Function GetTypeStripBanner(ByVal iType As Integer) As String
            Dim result As String = String.Empty
            If iType = 2 Then
                result = "Strip banner"
            ElseIf iType = 1 Then
                result = "Left banner"
            ElseIf iType = 0 Then
                result = "Home banner"
            ElseIf iType = 3 Then
                result = "Exclusive Offers"
            ElseIf iType = 4 Then
                result = "Bonus Offers"
            End If
            Return result
        End Function
#End Region

#Region "Sysparam"
        Private Shared Function ConvertSysparamPropertyCodeToName(ByVal propertyCode As String) As String
            Dim tmp As String = String.Empty
            Dim i As Integer = propertyCode.IndexOf(")")
            If i > 0 Then
                tmp = propertyCode.Substring(0, i + 1)
                propertyCode = propertyCode.Substring(i + 1)
            End If
            Dim lstCode As String = "LowWeightValue,HighWeightValue,SpecialHandlingFee"
            Dim lstName As String = "Low Weight Value,High Weight Value,Special Handling Fee"
            Dim CodeConverted As String = ConvertPropertyCodeToName(propertyCode, lstCode, lstName)
            If Not String.IsNullOrEmpty(tmp) Then
                CodeConverted = String.Format("{0}</td><td></td></tr><tr><td width='180px' class='field'>{1}", tmp.Replace("(", "(Weight Range "), CodeConverted)
            End If
            Return CodeConverted

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextSysparam(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertSysparamPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue

            Return String.Empty
        End Function
#End Region

#Region "Video"
        Private Shared Sub ConvertMessageLogInsertToTextVideo(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertVideoPropertyCodeToName(propertyCode)
            If (propertyCode = "VideoId") Then
                If (Value = "0") Then
                    returnValue = ""
                Else
                    returnValue = Value
                End If
            ElseIf (propertyCode = "CategoryId") Then
                returnValue = ""
            ElseIf (propertyCode = "ListCategoryId") Then
                returnValue = GetListCategoryName(Value)
            ElseIf (propertyCode = "TotalRow") Then
                returnValue = ""
            ElseIf (propertyCode = "PageIndex") Then
                returnValue = ""
            ElseIf (propertyCode = "PageSize") Then
                returnValue = ""
            Else
                returnValue = Value
            End If

        End Sub
        Private Shared Function ConvertVideoPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "VideoId,CategoryId,ThumbImage,IsYoutubeImage,Arrange,IsActive,MetaDescription,MetaKeyword,PageTitle,Title,ShortDescription,CreatedDate,ListCategoryId,VideoFile,ViewsCount"
            Dim lstName As String = "VideoId,CategoryId,Thumb Image,Is Youtube Image,Arrange,Is Active,Meta Description,Meta Keyword,Page Title,Title,Short Description,Created Date,Category,Video File,Views Count"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextVideo(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertVideoPropertyCodeToName(propertyCode)

            If (propertyCode = "CategoryId") Then
                returnFromValue = ""
                returnToValue = ""
            ElseIf (propertyCode = "ListCategoryId") Then
                returnFromValue = GetListCategoryName(fromValue)
                returnToValue = GetListCategoryName(toValue)
            Else
                returnFromValue = fromValue
                returnToValue = toValue
            End If
            Return String.Empty
        End Function
#End Region

#Region "Category"
        Private Shared Sub ConvertMessageLogInsertToTextCategory(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertCategoryPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "CategoryId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            ElseIf (propertyCode = "Type") Then
                returnValue = GetTypeCategory(Int32.Parse(returnValue))
            End If
        End Sub
        Public Shared Function GetTypeCategory(ByVal Type As Integer) As String
            Dim result As String = String.Empty
            If (Type = Utility.Common.CategoryType.News) Then
                result = "News & Events"
            ElseIf (Type = Utility.Common.CategoryType.MediaPress) Then
                result = "Media Press"
            ElseIf (Type = Utility.Common.CategoryType.Video) Then
                result = "Video"
            Else
                result = "Shop By Design"
            End If
            Return result
        End Function
        Private Shared Function ConvertCategoryPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "CategoryId,Banner,Arrange,IsActive,MetaDescription,MetaKeyword,PageTitle,CategoryName"
            Dim lstName As String = "CategoryId,Banner,Arrange,Is Active,Meta Description,Meta Keyword,Page Title,Category Name"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextCategory(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertCategoryPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            Return String.Empty
        End Function
#End Region

#Region "Department"
        Private Shared Sub ConvertMessageLogInsertToTextDepartment(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String)
            propertyName = ConvertDepartmentPropertyCodeToName(propertyCode)
            returnValue = Value
            If (propertyCode = "DepartmentId") Then
                If (returnValue = "0") Then
                    returnValue = ""
                End If
            End If
        End Sub
        Private Shared Function ConvertDepartmentPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "DepartmentId,Name,AlternateName,NameRewriteUrl,URLCode,LargeImage,LargeImageAltTag,LargeImageUrl,Image,ImageAltTag,NameImage,MetaDescription,MetaKeywords,PageTitle,OutsideUSPageTitle,OutsideUSMetaDescription,IsInactive,IsQuickOrder,IsFilter,ListEffectCode"
            Dim lstName As String = "Department Id,Name,Alternate Name,Name Rewrite Url,URL Code,Large Image,Large Image AltTag,Large Image Url,Image,Image Alt Tag,Name Image,Meta Description,Meta Keywords,Page Title,Outside US Page Title,Outside US Meta Description,Is Inactive,Is QuickOrder,Is Filter,List Effect"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextDepartment(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String) As String
            propertyName = ConvertDepartmentPropertyCodeToName(propertyCode)
            returnFromValue = fromValue
            returnToValue = toValue
            If (propertyCode = "DepartmentId") Then
                If (fromValue = "0") Then
                    returnFromValue = ""
                End If
                If (toValue = "0") Then
                    returnToValue = ""
                End If
            elseif (propertyCode = "ListEffectCode") Then
                returnFromValue = GetListEffectCode(fromValue)
                returnToValue = GetListEffectCode(toValue)
            End If


            Return String.Empty
        End Function
        Private Shared Function GetListEffectCode(ByVal lstValue As String) As String
            If (String.IsNullOrEmpty(lstValue)) Then
                Return "''"
            End If
            If Not lstValue.Contains(",") Then
                lstValue = "," & lstValue
            End If
            Dim count As Integer = 0
            Dim arrValue As String() = lstValue.Split(",")
            Dim result As String = String.Empty
            Dim EffectCode As String = String.Empty
            For Each tmp As String In arrValue
                If (String.IsNullOrEmpty(tmp)) Then
                    Continue For
                End If
                EffectCode = tmp
                count = count + 1
                If (String.IsNullOrEmpty(result)) Then
                    result = "<ul class=""itemcategory""><li> " & EffectCode & "</li>"
                Else
                    result = result & "<li>" & EffectCode & "</li>"
                End If
            Next
            If (count < 2) Then
                Return EffectCode
            End If
            Return result & "</ul>"
        End Function
        Public Shared ReadOnly Property AllowDepartmentLog() As String
            Get
                Dim result As String = ",DepartmentId,Name,AlternateName,NameRewriteUrl,URLCode,LargeImage,LargeImageAltTag,LargeImageUrl,Image,ImageAltTag,NameImage,MetaDescription,MetaKeywords,PageTitle,OutsideUSPageTitle,OutsideUSMetaDescription,IsInactive,IsQuickOrder,IsFilter,ListEffectCode,"
                Return result
            End Get
        End Property

#End Region

#Region "Lucene.Net"
        Private Shared _luceneAdminLogDir As String = Utility.ConfigData.LuceneAdminLogIndexPath
        Private Shared _directoryTemp As FSDirectory

        Private Shared Function _directory() As FSDirectory
            Dim luceneDir As String = _luceneAdminLogDir

            _directoryTemp = FSDirectory.Open(New DirectoryInfo(luceneDir))

            If (IndexWriter.IsLocked(_directoryTemp)) Then
                IndexWriter.Unlock(_directoryTemp)
            End If

            Dim lockFilePath As String = Path.Combine(luceneDir, "write.lock")

            If (File.Exists(lockFilePath)) Then
                File.Delete(lockFilePath)
            End If
            Return _directoryTemp
        End Function

        Public Shared Sub WriteLuceneLogDetail(ByVal objLogDetail As AdminLogDetailRow)
            If objLogDetail.ObjectId Is Nothing Then
                objLogDetail.ObjectId = ""
            End If
            Dim pageURL As String = String.Empty
            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)
            Dim writer As IndexWriter
            Try
                Dim logId As Integer = IIf(HttpContext.Current.Session("LogId") Is Nothing, 0, HttpContext.Current.Session("LogId"))
                Dim objLog As AdminLogRow = AdminLogRow.GetRow(logId)
                objLogDetail.LogId = objLog.LogId
                If objLog.LogId < 1 Then
                    objLog.RemoteIP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                    objLog.Username = String.Empty
                End If
                If Not (HttpContext.Current.Request Is Nothing) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.RawUrl) Then
                        pageURL = HttpContext.Current.Request.RawUrl
                    End If
                End If
                objLogDetail.CreatedDate = Now
                writer = New IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)
                Dim doc As New Document()
                doc.Add(New Field("LogId", objLog.LogId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED))
                doc.Add(New Field("IP", objLog.RemoteIP, Field.Store.YES, Field.Index.NOT_ANALYZED))
                doc.Add(New Field("UserName", objLog.Username, Field.Store.YES, Field.Index.ANALYZED))
                doc.Add(New Field("ItemKey", objLogDetail.ObjectId, Field.Store.YES, Field.Index.NOT_ANALYZED))
                doc.Add(New Field("CashPointKey", objLogDetail.ObjectId, Field.Store.YES, Field.Index.NOT_ANALYZED))
                doc.Add(New Field("OrderTrackingKey", objLogDetail.ObjectId, Field.Store.YES, Field.Index.NOT_ANALYZED))
                doc.Add(New Field("Date", objLogDetail.CreatedDate.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED))

                Dim nuDate As New NumericField("filter-range-date", Field.Store.YES, True)
                nuDate.SetIntValue(Convert.ToInt32(objLogDetail.CreatedDate.ToString("yyyyMMdd")))
                doc.Add(nuDate)

                doc.Add(New Field("sort-date", objLogDetail.CreatedDate.ToString("yyyyMMddHHmmss"), Field.Store.YES, Field.Index.NOT_ANALYZED))

                doc.Add(New Field("Action", objLogDetail.Action, Field.Store.YES, Field.Index.ANALYZED))
                doc.Add(New Field("ObjectType", objLogDetail.ObjectType, Field.Store.YES, Field.Index.ANALYZED))
                doc.Add(New Field("PageURL", pageURL, Field.Store.YES, Field.Index.NOT_ANALYZED))
                doc.Add(New Field("LogMessage", objLogDetail.Message, Field.Store.YES, Field.Index.NOT_ANALYZED))
                If (Not String.IsNullOrEmpty(objLogDetail.LoggedEmail)) Then
                    doc.Add(New Field("LoggedEmail", objLogDetail.LoggedEmail, Field.Store.YES, Field.Index.NOT_ANALYZED))
                End If
                Dim browserDetection As System.Web.HttpBrowserCapabilities = HttpContext.Current.Request.Browser
                Dim browser As String = String.Empty
                If browserDetection.Browser.ToLower().Contains("safari") Then
                    Dim tmp As String = HttpContext.Current.Request.UserAgent
                    Dim i As Integer = tmp.LastIndexOf(" ")
                    If i > 0 Then
                        tmp = tmp.Substring(0, i)
                        i = tmp.LastIndexOf(" ")
                        If i > 0 Then
                            tmp = tmp.Substring(i + 1)
                            i = tmp.IndexOf("/")

                            If i > 0 Then
                                Dim name As String = tmp.Substring(0, i)
                                tmp = tmp.Substring(i + 1)
                                Dim j As Integer = tmp.IndexOf(".")
                                Dim version As String = String.Empty
                                If j > 0 Then
                                    version = tmp.Substring(0, j)
                                End If
                                If name.ToLower().Equals("version") Then
                                    browser = "Safari " & version
                                Else
                                    browser = name & " " & version
                                End If
                            End If
                        End If
                    End If

                Else
                    browser = browserDetection.Browser & " " & browserDetection.Version
                End If
                doc.Add(New Field("Browser", browser, Field.Store.YES, Field.Index.NOT_ANALYZED))
                writer.AddDocument(doc)
            Catch ex As Exception
            End Try
            analyzer.Close()
            If Not writer Is Nothing Then
                ''writer.Optimize()
                writer.Close()
            End If

        End Sub

        Public Shared Function TypeSort(ByVal sortBy As String, ByVal sortOrder As String) As SortField
            Dim sort As SortField
            Dim revert As Boolean = True
            If Not String.IsNullOrEmpty(sortBy) Then
                If sortOrder.ToLower() = "desc" Then
                    revert = True
                Else
                    revert = False
                End If
            End If

            Select Case sortBy
                Case "UserName"
                    sort = New SortField("UserName", SortField.STRING, revert)
                Case "Action"
                    sort = New SortField("Action", SortField.STRING, revert)
                Case "ObjectType"
                    sort = New SortField("ObjectType", SortField.STRING, revert)
                Case "IP"
                    sort = New SortField("IP", SortField.STRING, revert)
                Case Else
                    sort = New SortField("sort-date", SortField.DOUBLE, revert)
            End Select
            Return sort
        End Function

        Public Shared Function SearchAdminLog(ByVal username As String, ByVal FromDate As String, ByVal ToDate As String, ByVal action As String, ByVal objName As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal sortBy As String, ByVal sortOrder As String) As DataTable
            Return SearchAdminLog(username, FromDate, ToDate, action, objName, pageIndex, pageSize, total, sortBy, sortOrder, Nothing, Nothing, Nothing, Nothing)
        End Function

        Public Shared Function SearchAdminLog(ByVal username As String, ByVal FromDate As String, ByVal ToDate As String, ByVal action As String, ByVal objName As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, ByVal sortBy As String, ByVal sortOrder As String, ByVal DB As Database, ByVal SKU As String, ByVal TransID As String, ByVal OrderNo As String) As DataTable
            Dim dt As New DataTable
            dt.Columns.Add("LogId")
            dt.Columns.Add("IP")
            dt.Columns.Add("UserName")
            dt.Columns.Add("ObjectId")
            dt.Columns.Add("CreatedDate", GetType(DateTime))
            dt.Columns.Add("Action")
            dt.Columns.Add("ObjectType")
            dt.Columns.Add("PageURL")
            dt.Columns.Add("Message")
            dt.Columns.Add("LoggedEmail")
            dt.Columns.Add("Browser")

            Dim analyzer As StandardAnalyzer = New StandardAnalyzer(Version.LUCENE_29)
            Dim searcher As IndexSearcher
            Try
                searcher = New IndexSearcher(_directory, True)
                Dim viewAll As Boolean = True
                Dim bq As New BooleanQuery
                Dim Parser As QueryParser = Nothing
                Dim query As Query = Nothing
                If Not String.IsNullOrEmpty(username) Then
                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"UserName"}, analyzer)
                    Parser.SetAllowLeadingWildcard(True)
                    query = parseQuery(username, Parser)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                End If
                If Not String.IsNullOrEmpty(action) Then
                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"Action"}, analyzer)
                    query = parseQuery(ForSearchLuceneMatch(action.Replace("','", " ")), Parser)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                End If
                If Not String.IsNullOrEmpty(objName) Then
                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"ObjectType"}, analyzer)
                    query = parseQuery(objName, Parser)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                Else
                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"ObjectType"}, analyzer)
                    query = parseQuery("Image", Parser)
                    bq.Add(query, BooleanClause.Occur.MUST_NOT)
                    ''hien tai thay Type=Image( chac TH nay ai lam ben ShopDesin nhung gan Type=Image) se xay ra loi, trong khi do minh kho co ObjectType nao la Image nen se loai bo Type =Image ra khoi ket qua search



                End If
                If Not String.IsNullOrEmpty(SKU) And Not String.IsNullOrEmpty(objName) And objName = Utility.Common.ObjectType.StoreItem.ToString() Then
                    Dim Id As Integer = StoreItemRow.GetRowSku(DB, SKU).ItemId
                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"ItemKey"}, analyzer)
                    query = parseQuery(Id, Parser)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                End If
                If Not String.IsNullOrEmpty(TransID) And Not String.IsNullOrEmpty(objName) And objName = Utility.Common.ObjectType.CashPoint.ToString() Then
                    Dim IdTrans As Integer = CashPointRow.GetRowByTransID(DB, TransID).CashPointId

                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"CashPointKey"}, analyzer)
                    query = parseQuery(IdTrans, Parser)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                End If
                If Not String.IsNullOrEmpty(OrderNo) And Not String.IsNullOrEmpty(objName) And objName = Utility.Common.ObjectType.TrackingNumber.ToString() Then
                    Dim Id As Integer = StoreOrderShipmentTrackingRow.GetTrakcingIdFromOrderNo(OrderNo)
                    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"OrderTrackingKey"}, analyzer)
                    query = parseQuery(Id, Parser)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                End If
                If Not String.IsNullOrEmpty(FromDate) And Not String.IsNullOrEmpty(ToDate) Then
                    Dim beginDate As Integer = Convert.ToInt32(Convert.ToDateTime(FromDate).ToString("yyyyMMdd"))
                    Dim endDate As Integer = Convert.ToInt32(Convert.ToDateTime(ToDate).ToString("yyyyMMdd"))
                    query = NumericRangeQuery.NewIntRange("filter-range-date", beginDate, endDate, True, True)
                    bq.Add(query, BooleanClause.Occur.MUST)
                    viewAll = False
                ElseIf Not String.IsNullOrEmpty(FromDate) Or Not String.IsNullOrEmpty(ToDate) Then
                    query = Nothing
                    If Not String.IsNullOrEmpty(FromDate) Then
                        Dim beginDate As Integer = Convert.ToInt32(Convert.ToDateTime(FromDate).ToString("yyyyMMdd"))
                        query = NumericRangeQuery.NewIntRange("filter-range-date", beginDate, Nothing, True, True)
                        viewAll = False
                    End If
                    If Not String.IsNullOrEmpty(ToDate) Then
                        Dim endDate As Integer = Convert.ToInt32(Convert.ToDateTime(ToDate).ToString("yyyyMMdd"))
                        query = NumericRangeQuery.NewIntRange("filter-range-date", Nothing, endDate, True, True)
                        viewAll = False
                    End If
                    bq.Add(query, BooleanClause.Occur.MUST)
                End If
                Dim filter As New RangeFilter("LogMessage", String.Empty, Nothing, False, True)
                Dim hit As Hits = Nothing
                If viewAll Then
                    bq.Add(New MatchAllDocsQuery, BooleanClause.Occur.MUST)
                End If
                Dim sort As New Sort(New SortField("sort-date", SortField.DOUBLE, True))
                hit = searcher.Search(bq, filter, New Sort(TypeSort(sortBy, sortOrder)))
                total = hit.Length
                Dim beginRow As Integer = pageIndex * pageSize
                Dim endRow As Integer = beginRow + pageSize - 1

                For i As Integer = 0 To hit.Length - 1
                    If (i >= beginRow And i <= endRow) Then
                        Dim doc As Document = hit.Doc(i)
                        Dim dr As DataRow = dt.NewRow
                        dr("LogId") = doc.Get("LogId")
                        dr("IP") = doc.Get("IP")
                        dr("UserName") = doc.Get("UserName")
                        dr("LoggedEmail") = doc.Get("LoggedEmail")
                        dr("ObjectId") = doc.Get("ItemKey")
                        dr("CreatedDate") = Convert.ToDateTime(doc.Get("Date"))
                        dr("Action") = doc.Get("Action")
                        dr("ObjectType") = doc.Get("ObjectType")
                        dr("PageURL") = doc.Get("PageURL")
                        dr("Message") = doc.Get("LogMessage")
                        dr("Browser") = doc.Get("Browser")
                        dt.Rows.Add(dr)
                    End If
                Next
            Catch ex As Exception

            End Try
            analyzer.Close()
            If Not searcher Is Nothing Then
                searcher.Close()
            End If
            Return dt
        End Function

        Private Shared Function parseQuery(ByVal searchQuery As String, ByVal parser As QueryParser) As Query
            Dim query As Query
            Try
                query = parser.Parse(searchQuery.Trim())
            Catch ex As ParseException
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()))
            End Try
            Return query
        End Function
        Private Shared Function ForSearchLuceneMatch(ByVal str As String) As String
            Dim search As String() = str.Trim().Split(" ")
            For i As Integer = 0 To search.Length() - 1
                If (Not String.IsNullOrEmpty(search(i))) Then
                    search(i) = search(i).Trim() + "*"
                End If
            Next
            str = String.Join(" ", search)
            Return str
        End Function
        Private Shared Function ForSearchLucene(ByVal str As String) As String
            Dim search As String() = str.Trim().Split(" ")
            For i As Integer = 0 To search.Length() - 1
                If (Not String.IsNullOrEmpty(search(i))) Then
                    search(i) = "*" + search(i).Trim() + "*"
                End If
            Next
            str = String.Join(" ", search)
            Return str
        End Function

        Private Shared Function Escape(ByVal s As String) As String
            Dim t As String

            t = Replace(s, "'", "\'")
            t = Replace(t, vbCrLf, "<br/>")
            t = Trim(t)

            Return "'" & t & "'"
        End Function


        'Public Shared Sub ClearLuceneIndexRecord()
        '    ' init lucene
        '    Dim analyzer = New StandardAnalyzer(Version.LUCENE_29)
        '    Dim writer As IndexWriter = New IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)
        '    Dim bq As New BooleanQuery
        '    ' remove older index entry
        '    Dim Parser As QueryParser = Nothing
        '    Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"ObjectType"}, analyzer)
        '    Dim query As Query = Nothing
        '    query = parseQuery(Utility.Common.ObjectType.LandingPage.ToString(), Parser)
        '    bq.Add(query, BooleanClause.Occur.MUST)

        '    Dim beginDate As Integer = 20130909 '' Convert.ToInt32(Convert.ToDateTime(FromDate).ToString("yyyyMMdd"))
        '    Dim endDate As Integer = 20130910 '' Convert.ToInt32(Convert.ToDateTime(ToDate).ToString("yyyyMMdd"))
        '    query = NumericRangeQuery.NewIntRange("filter-range-date", beginDate, endDate, True, True)
        '    bq.Add(query, BooleanClause.Occur.MUST)

        '    'Parser = New MultiFieldQueryParser(Version.LUCENE_29, New String() {"LogMessage"}, analyzer)
        '    'query = parseQuery(ForSearchLucene("GoogleABCode"), Parser)
        '    'bq.Add(query, BooleanClause.Occur.MUST)
        '    ''Dim searchQuery As TermQuery = New TermQuery(New Term("ObjectType", Utility.Common.ObjectType.LandingPage.ToString()))
        '    writer.DeleteDocuments(bq)

        '    ' close handles
        '    analyzer.Close()
        '    writer.Close()
        'End Sub
#End Region


#Region "Tracking Number"
        Private Shared Sub ConvertMessageLogInsertToTextTrackingNumber(ByVal propertyCode As String, ByVal Value As String, ByRef propertyName As String, ByRef returnValue As String, ByVal fullLogMessage As String)
            propertyName = ConvertTrackingNumberPropertyCodeToName(propertyCode)
            If (propertyCode = "TrackingId") Then
                If (Value = "0") Then
                    returnValue = ""
                Else
                    returnValue = Value
                End If
            ElseIf (propertyCode = "OrderId") Then
                returnValue = StoreOrderRow.GetOrderNoById(Value)
            ElseIf (propertyCode = "ShipmentType") Then
                returnValue = [Enum].GetName(GetType(Utility.Common.StandardShippingMethod), CInt(Value)) ''ShipmentMethodRow.GetNameById(Value)
            ElseIf (propertyCode = "TrackingNo") Then
                ''Dim fullMessage As String = "TrackingId**+|+**12414\ShipmentId**+|+**0\TrackingNo**+|+**UPPS222\OrderId**+|+**86613\CreatedDate**+|+**4/2/2014 10:58:57 AM\ModifiedDate**+|+**4/2/2014 10:58:57 AM\ShipmentType**+|+**3\P_O_Number**+|+**0\InvoiceNumber**+|+**0\NumberOfPackage**+|+**0\Weight**+|+**0\Note**+|+**UPPS222 Note\EmailToCustomer|True\ShipVia|FedEx Ground\"
                Dim ShipmentType As String = GetPropertyValueByCodeInMessageInsert(fullLogMessage, "ShipmentType")
                If CInt(ShipmentType) = Utility.Common.StandardShippingMethod.Truck Then
                    propertyName = "Link"
                End If
                returnValue = Value

            ElseIf (propertyCode = "Note") Then
                '' Dim fullMessage As String = "TrackingId**+|+**12414\ShipmentId**+|+**0\TrackingNo**+|+**UPPS222\OrderId**+|+**86613\CreatedDate**+|+**4/2/2014 10:58:57 AM\ModifiedDate**+|+**4/2/2014 10:58:57 AM\ShipmentType**+|+**3\P_O_Number**+|+**0\InvoiceNumber**+|+**0\NumberOfPackage**+|+**0\Weight**+|+**0\Note**+|+**UPPS222 Note\EmailToCustomer|True\ShipVia|FedEx Ground\"
                Dim ShipmentType As String = GetPropertyValueByCodeInMessageInsert(fullLogMessage, "ShipmentType")
                If CInt(ShipmentType) = Utility.Common.StandardShippingMethod.Truck Then
                    propertyName = "Label"
                End If
                returnValue = Value
            Else
                returnValue = Value
            End If

        End Sub
        Private Shared Function ConvertTrackingNumberPropertyCodeToName(ByVal propertyCode As String) As String

            Dim lstCode As String = "TrackingId,ShipmentId,TrackingNo,OrderId,CreatedDate,ModifiedDate,ShipmentType,Note,IsSent,CustomerNo,P_O_Number,InvoiceNumber,ServiceType,NumberOfPackage,Weight,EmailToCustomer,ShipVia"

            Dim lstName As String = "TrackingId,ShipmentId,Tracking Number,Order No,CreatedDate,ModifiedDate,Shipping Detail,Note,IsSent,CustomerNo,P_O_Number,InvoiceNumber,ServiceType,NumberOfPackage,Weight,Email Tracking To Customer,Ship Via / Options"
            Return ConvertPropertyCodeToName(propertyCode, lstCode, lstName)

        End Function
        Private Shared Function ConvertMessageLogUpdateToTextTrackingNumber(ByVal propertyCode As String, ByVal fromValue As String, ByVal toValue As String, ByRef propertyName As String, ByRef returnFromValue As String, ByRef returnToValue As String, ByVal fullLogMessage As String) As String
            propertyName = ConvertTrackingNumberPropertyCodeToName(propertyCode)
            If (propertyCode = "TrackingNo") Then
                Dim ShipmentType As String = GetPropertyValueByCodeInMessageUpdate(fullLogMessage, "ShipmentType")
                If Not String.IsNullOrEmpty(ShipmentType) Then
                    If CInt(ShipmentType) = Utility.Common.StandardShippingMethod.Truck Then
                        propertyName = "Link"
                    End If
                End If
                returnFromValue = fromValue
                returnToValue = toValue
            ElseIf (propertyCode = "Note") Then
                Dim ShipmentType As String = GetPropertyValueByCodeInMessageUpdate(fullLogMessage, "ShipmentType")
                If Not String.IsNullOrEmpty(ShipmentType) Then
                    If CInt(ShipmentType) = Utility.Common.StandardShippingMethod.Truck Then
                        propertyName = "Label"
                    End If
                End If
                returnFromValue = fromValue
                returnToValue = toValue
            ElseIf (propertyCode = "ShipmentType") Then
                If Not String.IsNullOrEmpty(toValue) Then
                    returnFromValue = [Enum].GetName(GetType(Utility.Common.StandardShippingMethod), CInt(fromValue))
                    returnToValue = [Enum].GetName(GetType(Utility.Common.StandardShippingMethod), CInt(toValue))
                    '' returnValue = [Enum].GetName(GetType(Utility.Common.StandardShippingMethod), CInt(Value))
                Else
                    returnFromValue = String.Empty
                    returnToValue = String.Empty
                End If
                
            Else
                returnFromValue = fromValue
                returnToValue = toValue
            End If
           
            Return String.Empty
        End Function
        Public Shared ReadOnly Property AllowTrackingNumberLog() As String
            Get
                Dim result As String = ",OrderId,ShipVia,ShipmentType,TrackingNo,Note,EmailToCustomer,"
                Return result
            End Get
        End Property
#End Region

      

    End Class

End Namespace
