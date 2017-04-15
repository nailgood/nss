Imports System.Xml

Namespace Utility

    Public Class ConfigFile

        Dim m_strConfigFile As String
        Dim oDoc As New XmlDocument

        Public Sub New(ByVal ConfigFile As String)
            m_strConfigFile = ConfigFile
            oDoc.Load(m_strConfigFile)
        End Sub

        ' Adds a configuration section
        Public Sub AddConfigSection(ByVal SectionName As String, ByVal HandlerClass As String)

            Dim rootNode As XmlNode = oDoc.GetElementsByTagName("configuration").Item(0)

            ' create the configSections node if it doesn't exist as config sections
            ' need an entry in this node
            Dim oNode As XmlNode
            oNode = oDoc.DocumentElement("configSections")
            If oNode Is Nothing Then
                oNode = oDoc.CreateElement("configSections")
                If rootNode.ChildNodes.Count > 0 Then
                    Dim oFirstChild As XmlNode = rootNode.FirstChild
                    rootNode.InsertBefore(oNode, oFirstChild)
                Else
                    rootNode.AppendChild(oNode)
                End If
            End If

            ' add the section to the configSectionsNode
            Dim oSubNode As XmlNode
            oSubNode = oDoc.CreateElement("section")
            With oSubNode.Attributes.Append(oDoc.CreateAttribute("name"))
                .Value = SectionName
            End With
            With oSubNode.Attributes.Append(oDoc.CreateAttribute("type"))
                .Value = HandlerClass
            End With
            oNode.AppendChild(oSubNode)

            ' now create the actual section, if it's not there
            oNode = oDoc.DocumentElement(SectionName)
            If oNode Is Nothing Then
                oNode = oDoc.CreateElement(SectionName)
                rootNode.AppendChild(oNode)
            End If

            oDoc.Save(m_strConfigFile)

        End Sub

        ' Checks whether a configuration section exists
        Public Function ConfigSectionExists(ByVal SectionName As String) As Boolean
            If Not oDoc.DocumentElement(SectionName) Is Nothing Then
                Return True
            End If
        End Function

        Public Sub SetConfigAttribute(ByVal SectionName As String, ByVal AttributeName As String, ByVal Value As String, Optional ByVal Create As Boolean = True)
            ' get the section node
            Dim oNode As XmlNode = oDoc.DocumentElement(SectionName)
            Dim oAttr As XmlAttribute
            oAttr = oNode.Attributes.ItemOf(AttributeName)
            If oAttr Is Nothing Then
                oAttr = oDoc.CreateAttribute(AttributeName)
                oNode.Attributes.Append(oAttr)
            End If
            oAttr.Value = Value
            oDoc.Save(m_strConfigFile)
        End Sub

        Public Sub SetAppSetting(ByVal SettingName As String, ByVal Value As String)

            ' get the node that corresponds to the setting 
            Dim oSettingNode As XmlNode = GetAppSettingNode(SettingName)

            ' encrypt the value if requested and then set it

            oSettingNode.Attributes.ItemOf("value").Value = Value

            ' save changes
            oDoc.Save(m_strConfigFile)

        End Sub

        Private Function AddAppSetting(ByVal SettingName As String) As XmlNode

            ' get the appSettings node
            Dim oNode As XmlNode = oDoc.DocumentElement("appSettings")

            ' create the key attribute 
            Dim oKey As XmlAttribute = oDoc.CreateAttribute("key")
            oKey.Value = SettingName

            ' create the value attribute
            Dim oValue As XmlAttribute = oDoc.CreateAttribute("value")
            oValue.Value = ""

            ' create the node for the setting
            Dim oChild As XmlNode = oDoc.CreateElement("add")
            oChild.Attributes.Append(oKey)
            oChild.Attributes.Append(oValue)

            ' add the node to the appSettings section
            oNode.AppendChild(oChild)
            Return oChild

        End Function

        Private Function GetAppSettingNode(ByVal SettingName As String, Optional ByVal CreateIfNotFound As Boolean = True) As XmlNode

            ' get the appSettings node
            Dim oNode As XmlNode = oDoc.DocumentElement("appSettings")
            Dim oChild As XmlNode = Nothing
            Dim blnFound As Boolean

            ' find the node corresponding to the setting
            For Each oChild In oNode.ChildNodes
                If oChild.Attributes.ItemOf("key").Value = SettingName Then
                    blnFound = True
                    Exit For
                End If
            Next

            ' if the node was not found we need to create it if it was requested
            If Not blnFound Then
                If CreateIfNotFound Then
                    oChild = AddAppSetting(SettingName)
                Else
                    oChild = Nothing
                End If
            End If

            ' return the node
            Return oChild

        End Function

        Public Function GetAppSetting(ByVal SettingName As String) As String

            ' get the node that corresponds to the setting 
            Dim oSettingNode As XmlNode = GetAppSettingNode(SettingName, False)

            ' if the node for the setting does not exist the just return an empty string
            If oSettingNode Is Nothing Then
                Return ""
            Else
                ' get the value and decrypt it if requested 
                GetAppSetting = oSettingNode.Attributes.ItemOf("value").Value

            End If

        End Function

        Public Function GetConfigAttribute(ByVal SectionName As String, ByVal AttributeName As String, Optional ByVal Create As Boolean = True) As String
            ' get the section node
            Dim oNode As XmlNode = oDoc.DocumentElement(SectionName)
            Dim oAttr As XmlAttribute
            oAttr = oNode.Attributes.ItemOf(AttributeName)
            If oAttr Is Nothing Then
                oAttr = oDoc.CreateAttribute(AttributeName)
                oNode.Attributes.Append(oAttr)
                oDoc.Save(m_strConfigFile)
            End If
            Return oAttr.Value
        End Function

    End Class
End Namespace
