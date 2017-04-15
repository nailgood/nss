<%@ Page AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_members_edit"
    Language="VB" MasterPageFile="~/includes/masterpage/admin.master" Title="Member" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <asp:RequiredFieldValidator ID="rqtxtPassword" runat="server" Display="none" EnableClientScript="False"
        ControlToValidate="txtPassword" CssClass="msgError" ErrorMessage="You must provide a password for your membership" />
    <CC:PasswordValidator ID="pvtxtPassword" runat="server" Display="none" EnableClientScript="False"
        ControlToValidate="txtPassword" CssClass="msgError" ErrorMessage="Password must be between 4 and 10 characters, contain at least one letter and at least one number." />
    <CC:CompareValidatorFront ID="cvtxtPassword" runat="server" Display="none" EnableClientScript="False"
        ControlToValidate="txtPassword" ControlToCompare="txtConfirmPassword" Operator="Equal"
        Type="String" CssClass="msgError" ErrorMessage="The passwords you entered do not match" />
    <h4>
        <% If MemberId = 0 Then%>
        Add<% Else%>Edit<% End If%>
        Member</h4>
    <table border="0" cellpadding="2" cellspacing="1">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Username:
            </td>
            <td class="field">
                <asp:TextBox ID="txtUsername" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Username' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Password: <a target="_blank" href="password.aspx?MemberId=<%=MemberId %>&F_SortBy=Email&F_SortOrder=ASC">
                    View</a>
            </td>
            <td class="field">
                <asp:TextBox ID="txtPassword" runat="server" size="50" MaxLength="100" TextMode="Password" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Password confirmation:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtConfirmPassword" runat="server" size="50" MaxLength="100" TextMode="Password" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Member Type:
            </td>
            <td class="field">
                <asp:DropDownList runat="server" ID="MemberTypeId" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Is Same Default Address?</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkIsSameDefaultAddress" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Active Register</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkIsActive" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Deactivate Account</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkDeActive" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Customer No:
            </td>
            <td class="field">
                <asp:TextBox ID="txtCustomerNo" runat="server" Columns="20" MaxLength="20" Style="width: 150px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                License Number:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLicenseNumber" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                License State:
            </td>
            <td class="field">
                <asp:TextBox ID="txtLicenseState" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                License Expiration Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtLicenseExpirationDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator ID="dtvLicenseExpirationDate" runat="server" ControlToValidate="dtLicenseExpirationDate"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Date Field 'License Expiration Date' is invalid">
                </CC:DateValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Student Number:
            </td>
            <td class="field">
                <asp:TextBox ID="txtStudentNumber" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                School Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtSchoolName" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Contact Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtContactName" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Contact Phone:
            </td>
            <td class="field">
                <asp:TextBox ID="txtContactPhone" runat="server" Columns="50" MaxLength="50" Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Expected Graduation Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtExpectedGraduationDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator ID="dtvExpectedGraduationDate" runat="server" ControlToValidate="dtExpectedGraduationDate"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Date Field 'Expected Graduation Date' is invalid">
                </CC:DateValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Sales Tax Exemption Number:
            </td>
            <td class="field">
                <asp:TextBox ID="txtSalesTaxExemptionNumber" runat="server" Columns="50" MaxLength="50"
                    Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Dept Of Revenue Registered?</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkDeptOfRevenueRegistered" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Resale Acceptance?</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkResaleAcceptance" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Information Accuracy Agreement?</b>
            </td>
            <td class="field">
                <asp:CheckBox ID="chkInformationAccuracyAgreement" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Authorized Signature Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtAuthorizedSignatureName" runat="server" Columns="50" MaxLength="100"
                    Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Authorized Signature Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtAuthorizedSignatureTitle" runat="server" Columns="50" MaxLength="50"
                    Style="width: 319px;"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Authorized Signature Date:
            </td>
            <td class="field">
                <CC:DatePicker ID="dtAuthorizedSignatureDate" runat="server"></CC:DatePicker>
            </td>
            <td>
                <CC:DateValidator ID="dtvAuthorizedSignatureDate" runat="server" ControlToValidate="dtAuthorizedSignatureDate"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Date Field 'Authorized Signature Date' is invalid">
                </CC:DateValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Can Post Job?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkCanPostJob" />
            </td>
        </tr>
        <tr>
            <td class="required">
                <b>Can Post Classified?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkCanPostClassified" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Customer Price Group</b>
            </td>
            <td class="field">
                <asp:DropDownList runat="server" ID="drpCustomerPriceGroupId" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" CausesValidation="False" runat="server" CssClass="btn"
        Text="Save" />
    <CC:ConfirmButton ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn"
        Message="Are you sure want to delete this Member?" Text="Delete" />
    <CC:OneClickButton ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn"
        Text="Cancel" />
    <CC:OneClickButton ID="btnViewOrder" CausesValidation="False" runat="server" CssClass="btn"
        Text="View Order History" />
    <p>
    </p>
    <span class="smaller">Navision Customer/Contact Data - controlled by Customer or through
        Navision</span>
    <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <table border="0" cellspacing="1" cellpadding="2">
                    <tr>
                        <td class="optional">
                            Customer No:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblCustomerNo2" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Company / Salon:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblSalonName" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Name:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtName" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Name 2:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtName2" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Customer Posting Group:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtCustomerPostingGroup" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Address:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtAddress" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Address 2:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtAddress2" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            City:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtCity" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Zipcode:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtZipcode" runat="server" Style="width: 139px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            County:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtCounty" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Phone:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtPhone" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContact" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Email:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtEmail" runat="server" Style="width: 319px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Website:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtWebsite" runat="server" Style="width: 319px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Sales Tax Exemption Number:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblSalesTaxExemptionNumber" runat="server" Style="width: 319px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Currency Code:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtCurrencyCode" runat="server" Style="width: 79px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Customer Price Group:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtCustomerPriceGroup" runat="server" Style="width: 79px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Customer Discount Group:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtCustomerDiscountGroup" runat="server" Style="width: 79px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Language Code:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtLanguageCode" runat="server" Style="width: 79px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Payment Terms Code:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtPaymentTermsCode" runat="server" Style="width: 79px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Last Date Modified:
                        </td>
                        <td class="field">
                            <asp:Label ID="dtLastDateModified" runat="server"></asp:Label>
                        </td>
                        <td>
                            <CC:DateValidator Display="Dynamic" runat="server" ID="dtvLastDateModified" ControlToValidate="dtLastDateModified"
                                CssClass="msgError" ErrorMessage="Date Field 'Last Date Modified' is invalid" />
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Last Import:
                        </td>
                        <td class="field">
                            <asp:Label ID="dtLastImport" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Last Export:
                        </td>
                        <td class="field">
                            <asp:Label ID="dtLastExport" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <img src="/includes/theme-admin/images/spacer.gif" width="10" height="1" />
            </td>
            <td valign="top">
                <table border="0" cellspacing="1" cellpadding="2">
                    <tr>
                        <td class="optional">
                            Contact No:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactNo" runat="server" Style="width: 139px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Name:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblContactName" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Name 2:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactName2" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Address:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactAddress" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Address 2:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactAddress2" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact City:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactCity" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Zipcode:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactZipcode" runat="server" Style="width: 139px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact County:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactCounty" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Country:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactCountry" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Phone:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblContactPhone" runat="server" Style="width: 199px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Email:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactEmail" runat="server" Style="width: 319px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Contact Website:
                        </td>
                        <td class="field">
                            <asp:Label ID="txtContactWebsite" runat="server" Style="width: 319px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Sales Tax Exemption Number:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblSalesTaxExemptionNumber2" runat="server" Style="width: 319px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Last Export:
                        </td>
                        <td class="field">
                            <asp:Label ID="lblLastExport" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <div id="dvMsg"><asp:Literal runat="server" ID="ltrMsgError"></asp:Literal></div>
    <table runat="server" id="tblExport" visible="false" width="810px">
	    <tr>
	        <th style="width:130px;">Date</th>
	        <th style="width:230px;">Customer File</th>
	        <th style="width:100px;">Customer Status</th>  
	        <th style="width:230px;">Customer Address File</th>      
	        <th style="width:100px;">Address Status</th>    
	    </tr>
	    <tr style="min-height:30px; vertical-align:top;">
	    	<td><asp:Literal runat="server" ID="ltrDate"></asp:Literal></td>  
	        <td><asp:LinkButton OnClientClick="clearMsgError();"  runat="server" ID="lbCustomerFile"></asp:LinkButton> <span runat="server" id="spPendingCustomer" visible="false" style="font-style:italic;">(pending)</span></td>
	        <td><asp:Literal runat="server" ID="ltrCustomerStatus"></asp:Literal></td>  
	        <td><asp:LinkButton OnClientClick="clearMsgError();"  runat="server" ID="lbAddressFile"></asp:LinkButton> <span runat="server" id="spPendingAddress" visible="false" style="font-style:italic;">(pending)</span></td>	        
	        <td><asp:Literal runat="server" ID="ltrAddressStatus"></asp:Literal></td>	     
	    </tr>
	</table>	
	    <asp:Repeater runat="server" ID="rptNoteFile">
	        <HeaderTemplate>
	            <table width="810px">
	    	        <tr runat="server" id="trNote">
	    	        	<th style="width:130px;"></th>
	                    <th style="width:230px;">Note Customer File</th>
	                    <th style="width:100px;"></th>
	                    <th style="width:230px;">Note Customer Address File</th>	
	                    <th style="width:100px;"></th> 
	                </tr>
	        </HeaderTemplate>
	        
	        <ItemTemplate>
	                <tr>	    
	                    <td></td>               
	                    <td><asp:LinkButton OnClientClick="clearMsgError();" CommandName="DownloadExport" runat="server" ID="lbNoteCustomerFile"></asp:LinkButton></td>
	                    <td><asp:Literal runat="server" ID="ltrNoteCustStatus"></asp:Literal></td>
	                    <td><asp:LinkButton OnClientClick="clearMsgError();" CommandName="DownloadExport" runat="server" ID="lbNoteAddressFile"></asp:LinkButton></td>
	                    <td><asp:Literal runat="server" ID="ltrNoteAddrStatus"></asp:Literal></td>	                    
	                </tr>	    	        
	        </ItemTemplate>
	        
	        <FooterTemplate>
	            </table>
	        </FooterTemplate>
	    </asp:Repeater>
	    
	<script type="text/javascript">
	    function clearMsgError() {
	        var dvMsg = document.getElementById("dvMsg");
	        dvMsg.innerHTML = '';
	        return false;
	    }
	</script>
</asp:Content>
