<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_reminders_edit" CodeFile="edit.aspx.vb"  MasterPageFile="~/includes/masterpage/interior.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
<%--<link rel="stylesheet" href="../../includes/scripts/datepicker/jquery-ui.css">
  <script src="../../includes/scripts/datepicker/jquery-ui.js"></script> Edit css.xml--%>
     <asp:Literal ID="litCSS" runat="server"></asp:Literal>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
  <script>
      $(document).ready(
  function () {
      $("#ctrlEventDate").datepicker({
          changeMonth: true, //this option for allowing user to select month
          changeYear: true //this option for allowing user to select from year range
      });
  }

);
  </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<%--<CC:RequiredFieldValidatorFront ID="rfvtxtEmail" runat="server" Display="None" EnableClientScript="False" ControlToValidate="txtEmail" ErrorMessage="Field 'Email' is blank" />
<CC:EmailValidatorFront ID="evftxtEmail" runat="server" Display="None" EnableClientScript="False" ControlToValidate="txtEmail" ErrorMessage="Field 'Email' is not a valid email address" />
<CC:RequiredDateValidatorFront ID="rqdvctrlEventDate" runat="server" Display="none" EnableClientScript="False" ControlToValidate="ctrlEventDate" ErrorMessage="Field 'Event Date' is not valid." />
<CC:DateValidatorFront ID="dvctrlEventDate" runat="server" Display="none" EnableClientScript="False" ControlToValidate="ctrlEventDate" ErrorMessage="Field 'Event Date' is not valid." />--%>

<div class="panel-content">
    <div class="title"><asp:literal id="ltlPageTitle" runat="server" /></div>
   <div  id="reminder" class="content">
			<div class="form-group">
                <div class="col-sm-12"><span class="required">Event Name</span></div>
			    <div class="col-sm-6"><asp:textbox id="txtName" runat="server" CssClass="form-control" placeholder="Event Name"/></div>
                <div class="div-error"><asp:RequiredFieldValidator runat="server" ID="rfvtxtName" ValidationGroup="valReminder" ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Event name is required." CssClass="text-danger"></asp:RequiredFieldValidator></div>
      		</div>
    	    <div class="form-group col-sm-12" style="padding-top:10px;margin-bottom:0">
                    <span class="pull-left">Recurring Event?</span>
                    <div class="checkbox">
                       <label for="chkIsRecurring">
                         <asp:checkbox id="chkIsRecurring" runat="server" />
                                    <i class="fa fa-check checkbox-font" ></i>
                                    Yes
                       </label>
                    </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12"><span class="required">Event Date</span></div>
	            <div class="col-sm-6"><input type="text" ID="ctrlEventDate" runat="server" class="form-control" placeholder="Event Date" /></div>
                <div class="div-error"><asp:RequiredFieldValidator ID="rqdvctrlEventDate" runat="server" Display="Dynamic" ValidationGroup="valReminder" CssClass="text-danger" ControlToValidate="ctrlEventDate" ErrorMessage="Event Date is required."></asp:RequiredFieldValidator></div>
    		</div>
    		
			<div class="form-group">
			    <div class="col-sm-12">When to send 1st reminder?</div>
			    <div class="n-dropdown col-sm-6"><asp:dropdownlist id="drpFirstReminder" CssClass="form-control" runat="server" /></div>
			</div>
    	    <div class="form-group">
			   <div class="col-sm-12">When to send 2nd reminder?</div>
			    <div class="n-dropdown col-sm-6"><asp:dropdownlist id="drpSecondReminder" CssClass="form-control" runat="server" /></div>
    		</div>
    		<div class="form-group">
			    <div class="col-sm-12"><span class="required">Email</span></div>
			    <div class="col-sm-6"><asp:textbox id="txtEmail" runat="server" class="form-control" /></div>
                <div class="div-error">
                        <asp:RequiredFieldValidator runat="server" ID="rqtxtEmail" ControlToValidate="txtEmail" ValidationGroup="valReminder" Display="Dynamic" ErrorMessage="Email is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvEmail" ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="valReminder" ErrorMessage="" CssClass="text-danger" ClientValidationFunction="CheckEmailValidate" runat="server" />
                    </div>
    		</div>
			<div class="form-group">
			    <div class="col-sm-12">Comments</div>
			    <div class="col-sm-6"><asp:textbox id="txtComments" TextMode="MultiLine" runat="server" class="form-control" cols="15" /></div>
			</div>
			
			     <br /><asp:Button id="btnSave" runat="server" CssClass="btn btn-submit" data-btn="submit" ValidationGroup="valReminder" Text="Submit" CausesValidation="true" />
			
      </div>
</div>
<script type="text/javascript">
$(window).resize(function () {
$("#ui-datepicker-div").css("display","none");
});
             function CheckEmailValidate(source, arguments) {

                 var str = ''; //1: Email is not valid, 2: Email is exists, 3 User name is exists

                 $.ajax({

                     contentType: "application/json; charset=utf-8",
                     url: "/members/register_check.ashx?act=email&key=" + arguments.Value,
                     dataType: "json",
                     async: false,
                     success: function (result) {
                         str = result;
                     }
                 });

                 if (str == "1") {

                     arguments.IsValid = false;
                     source.textContent = 'Email is invalid.';
                 }
                 else if (str == "2") {
                     arguments.IsValid = false;
                     source.textContent = "The email is already in use!";
                 }
                 else if (str == "3") {
                     arguments.IsValid = false;
                     source.textContent = "The username is already in use!";
                 }
                 else if (str == "4") {
                     arguments.IsValid = true;
                 }
                 else {
                     arguments.IsValid = true;
                 }
             }

    </script> 
</asp:Content>