<%@ Page Language="VB" AutoEventWireup="false" CodeFile="survey.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="store_survey" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="review">
<h1>Survey</h1>
<asp:Panel id="dvSurveyForm" runat="server" class="form" DefaultButton="btnSubmit" >
   <div class="BlockOrderReview"><asp:Literal runat="server" ID="ltrDescription"></asp:Literal></div>
   <div class="reviewform">
        <div class="form-group">
                            <div class="col-sm-6 txt-required">
                                <asp:TextBox id="txtName" runat="server" class="form-control" type="text" placeholder="Your Name"></asp:TextBox>
                             </div>
                            <div class="div-error">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtName" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="Reviewer Name is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>
        </div>
        <div class="form-group">
                            <div class="col-sm-6 txt-required">
                                <asp:TextBox id="txtEmail" runat="server" class="form-control" type="text" placeholder="Review Title"></asp:TextBox>
                            </div>
                            <div class="div-error">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtEmail" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="Reviewer Title is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                                <asp:Label class="red" Id="lbrqEmail" visible="False"  runat="server">Field "Your Email" is blank</asp:Label>
		                        <asp:Label class="red" Id="lbValidateEmail" visible="False"  runat="server">Your email address is invalid.</asp:Label>
		                        <asp:Label class="red" Id="lbExistEmail" visible="False"  runat="server">Your email joined this survey.</asp:Label>
                            </div>
        </div>
        <div id="group-radio">
                         
                    <asp:DataList runat="server" id="dlQuestion" CssClass="tblQuestion">
		            <ItemTemplate>
		                <asp:HiddenField runat="server" Id="hdQuestionId"></asp:HiddenField>
		                <div class="tbl-row">
                                <div class="col-l"><asp:Literal runat="server" Id="ltrQuestion"></asp:Literal></div>
			                    <div class="col-r pad-r">
                                    <asp:RadioButtonList id="rdlAnswer" runat="server" RepeatDirection="Horizontal" CssClass="radio-node">
<%--			                        <asp:ListItem Value="1" Selected="true"><i class="ico-radio"></i>Yes</asp:ListItem>
                                    <asp:ListItem Value="0"><i class="ico-radio"></i>No</asp:ListItem>--%>
			                        </asp:RadioButtonList>
			                    </div>
                         </div>
                         <asp:TextBox runat="server"  TextMode="MultiLine" ID="txtQNote" rows="6" CssClass="placeholder"
		                onfocus="if(this.value=='Leave a suggestion') {this.value='';this.setAttribute('class','');}" onblur="if(this.value=='') {this.value='Leave a suggestion';this.setAttribute('class','placeholder');}" 
		                Text="Leave a suggestion" MaxLength="2000"></asp:TextBox>
		            </ItemTemplate>
		        </asp:DataList>
        </div>
   </div>
   
     <div class="form-group">
            <div class="col-sm-6 prComment" runat="server" id="trComment">
                <div class="re-bd">The most useful comments contain specific examples about:
                    <ul>
                        <li>Another wholesaler</li>
                        <li>Things that are great about us or our products</li>
                        <li>Things that you don't like about us or our products</li>
                    </ul>
                        <span class="note">Please do not include: HTML, references to other retailers, pricing, personal information, or any profane, inflammatory or copyrighted comments. Also, if you were given this product as a gift or otherwise compensated in exchange for writing this review,  you are REQUIRED to disclose it in your comments above.</span>
                    </div>
            
                    </div>
                    <div class="col-sm-6" style="top:-7px">
                       <textarea id="txtComment" runat="server" onkeyup="ShowComment(this)" name="txtComments" rows="6" class="form-control" placeholder="Comments" />
                    </div>
                    <div class="div-error pull-left paderr">
<%--                         <asp:RequiredFieldValidator runat="server" ID="rqtxtComments" ControlToValidate="txtComment" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="Required minimum 10 words or more. You have typed 0 words." CssClass="text-danger"></asp:RequiredFieldValidator>--%>
                         <span id="dvCommentError" runat="server" class="text-danger"></span>
                    </div>
        </div>
         <div id="divSubmit" runat="server"><asp:Button ID="btnSubmit" onClientClick="return ValidateForm();" runat="server" Text="Submit" data-btn="submit" class="btn btn-submit" ValidationGroup="review_Group"/></div>
</asp:Panel>
 <div id="divConfirm" runat="server" visible="false">
    <p><b><asp:Literal runat="Server" id="ltlEmail"/></b> has successfully submitted this survey.</p>
    <p>Thank you for your feedback. We will record your opinion so we can serve you better in the future.</p>
</div>

 <div id="divCompleted" runat="server" visible="false">
    <p><b><asp:Literal runat="Server" id="ltrInformation"/></b> successfully submitted this survey.</p>
    <p>Thank you for your interest.</p>
</div>
<script type="text/javascript">
    function pageLoad() {
        var arr = document.getElementsByTagName("textarea");
        for (var i = 0; i < arr.length; i++) {
            var j = arr[i].id.indexOf('txtQNote');
            if (j >= 0) {
                if (arr[i].value != '' && arr[i].value != "Leave a suggestion") {
                    arr[i].setAttribute('class', '');
                }
            }
        }
    }
    $(window).load(function () {
        fnAddIconRad();
    });
    fnAddIconRad = function () {
        var ctRad = $("#rdlAnswer label").prepend("<i class='ico-radio'></i>");
    }
</script>

</div>

</asp:Content>