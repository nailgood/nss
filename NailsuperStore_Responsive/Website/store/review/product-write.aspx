<%@ Page Language="VB" AutoEventWireup="false" CodeFile="product-write.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="store_review" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="review">
    <h3>Write a Review for this Product: </h3>
<div class="review-box">
    <div class="review-img">
        <img src="<%=image %>" alt="<%=altImg %>" />
    </div>
    <div>
       <h1><%=title%></h1>
        <%=desc %>
        <%If get20pts <> Nothing Then%>
        <p><%=Resources.Msg.review_msgFirst%></p>
        <%End If%>
    </div>
</div>
<div id="divReviewForm" runat="server" role="form">
        <div class="form-group">
                            <div class="col-sm-6 txt-required">
                                <asp:TextBox id="txtFirstName" runat="server" class="form-control" type="text" placeholder="First Name"></asp:TextBox>
                            </div>
                            <div class="div-error">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtFirstName" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="First Name is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>
        </div>
           <div class="form-group">
                            <div class="col-sm-6 txt-required">
                               <asp:TextBox id="txtLastName" runat="server" class="form-control" type="text" placeholder="Last Name"></asp:TextBox>
                            </div>
                            <div class="div-error">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtLastName" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="Last Name is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>
        </div>
        <div class="form-group">
                            <div class="col-sm-6 txt-required">
                                <asp:TextBox id="txtTitle" runat="server" class="form-control" type="text" placeholder="Review Title"></asp:TextBox>
                            </div>
                            <div class="div-error">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtTitle" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="Reviewer Title is required" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>
        </div>
        <div class="rating">
            <div class="pull-left">Rating <span class="red">*</span>:</div>
  	        <div class="pull-left ic-review">
                 <span id="imgSelectStars">
                  <i class="fa fa-star fa-2" onclick="fnChangeStar(1);"></i><i class="fa fa-star fa-2" onclick="fnChangeStar(2);"></i><i class="fa fa-star fa-2" onclick="fnChangeStar(3);"></i><i class="fa fa-star fa-2" onclick="fnChangeStar(4);"></i><i class="fa fa-star fa-2" onclick="fnChangeStar(5);"></i>
                 </span>
                  <%--<img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/star50.png" usemap="#selectstars" border="0" id="imgSelectStars" name="imgSelectStars" />--%> </div>
			<div class="n-dropdown" style="top:-5px">
			    <%--<select id="ddlStars" runat="server" class="form-control" onchange="document.getElementById('imgSelectStars').src='/includes/theme/images/star' + this.value + '.png';fnChangeStar(this.value)">--%>
                <select id="ddlStars" runat="server" class="form-control" onchange="fnChangeStar(this.value)">
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5" selected="selected">5</option>
                </select>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6 prComment">
                <div class="re-bd">The most useful comments contain specific examples about:
                    <ul>
                        <li>How you use the product</li>
                        <li>Things that are great about it</li>
                        <li>Things that aren't so great about it</li>
                        </ul>
                        <span class="note">Please do not include: HTML, references to other retailers, pricing, personal information, or any profane, inflammatory or copyrighted comments. Also, if you were given this product as a gift or otherwise compensated in exchange for writing this review,  you are REQUIRED to disclose it in your comments above.</span>
                    </div>
            
                    </div>
                    <div class="col-sm-6 txt-required" style="top:-7px">
                       <textarea id="txtComments" runat="server" onkeyup="ShowComment(this)" name="txtComments" rows="6" class="form-control" placeholder="Product Comments" />
                    </div>
                    <div class="div-error pull-left paderr">
                        <%-- <asp:RequiredFieldValidator runat="server" ID="rqtxtComments" ControlToValidate="txtComments" ValidationGroup="review_Group" Display="Dynamic" ErrorMessage="Required minimum 10 words or more. You have typed 0 words." CssClass="text-danger"></asp:RequiredFieldValidator>--%>
                         <span id="dvCommentError" runat="server" class="text-danger"></span>
                    </div>
        </div>
 
        <div id="divSubmit" runat="server"><asp:Button ID="btnSubmit" onClientClick="return ValidateForm();" runat="server" Text="Submit" data-btn="submit" class="btn btn-submit" ValidationGroup="review_Group"/></div>
        <div id="divSave" runat="server"><asp:Button ID="btnSave" onClientClick="return ValidateForm();" runat="server" Text="Save" class="btn btn-submit" ValidationGroup="review_Group"/></div>
        <div class="note pad-note">After submitting your product review <%=DataLayer.SysParam.GetValue("CompanyName")%> will review it for approval and posting onto the site.</div>
</div>
<div class="thanks"><asp:Literal id="litMsg" runat="server"></asp:Literal></div>
   <%-- <asp:Literal id="litTemplate" runat="server"></asp:Literal>--%>
 <%--<asp:Literal id="litJS" runat="server"></asp:Literal>
                            <map name="selectstars"><area shape="rect" coords="0,0,17,20" href="javascript:void(0);" alt="1 Star" 
                            onclick="document.getElementById('imgSelectStars').src='/includes/theme/images/star10.png';document.getElementById('ddlStars').value='10';" />
                            <area shape="rect" coords="18,0,34,20" href="javascript:void(0);" 
                            alt="2 Stars" 
                            onclick="document.getElementById('imgSelectStars').src='/includes/theme/images/star20.png';document.getElementById('ddlStars').value='20';getValueStar('2')" />
	                        <area shape="rect" coords="35,0,52,20" href="javascript:void(0);" 
                            alt="3 Stars" 
                            onclick="document.getElementById('imgSelectStars').src='/includes/theme/images/star30.png';document.getElementById('ddlStars').value='30';getValueStar('3')" />
		                    <area shape="rect" coords="53,0,69,20" href="javascript:void(0);" 
                            alt="4 Stars" 
                            onclick="document.getElementById('imgSelectStars').src='/includes/theme/images/star40.png';document.getElementById('ddlStars').value='40';getValueStar('4')" />
                            <area shape="rect" coords="70,0,86,20" href="javascript:void(0);" 
                            alt="5 Stars" 
                            onclick="document.getElementById('imgSelectStars').src='/includes/theme/images/star50.png';document.getElementById('ddlStars').value='50';getValueStar('5')" />
		                    </map>--%>

<asp:HiddenField ID="hidStar" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hidStarLoad" runat="server"></asp:HiddenField>
<input type="hidden" id="hidCountMessage" name="hidCountMessage" runat="server" />
<input type="hidden" id="hidMinimumWordComment" name="hidMinimumWordComment" runat="server" />
<script type="text/javascript" language="javascript">
    fnChangeStar = function (val) {
        // alert(val);
        var html = "";
        for (i = 1; i <= 5; i++) {
            if (i <= val)
            {
                html += "<i class='fa fa-star fa-2' onclick='fnChangeStar(" + i + ");'></i>";
            }
            else
            {
                html += "<i class='fa fa-star-o fa-1' onclick='fnChangeStar(" + i + ");'></i>";
            }
        }
        $("#imgSelectStars").html(html);
        hidStar.value = val;
        $("#ddlStars").val(val);
    }

    function ShowComment(ctr) {
         if (document.getElementById('hidCountMessage')) {
            var message = document.getElementById('hidCountMessage').value;
            if (document.getElementById('dvCommentError')) {
                var minimumWord = document.getElementById('hidMinimumWordComment').value;
                var commentCount = countWords(ctr.value);
                if (parseInt(commentCount) < parseInt(minimumWord)) {
                    var text = FormatString(message + "," + minimumWord + "," + commentCount);
                    document.getElementById('dvCommentError').innerHTML = text;
                    document.getElementById('dvCommentError').style.display = '';
                    document.getElementById("rqtxtComments").style.display = 'none';
                }
                else {
                    document.getElementById('dvCommentError').style.display = 'none';

                }
            }

        }
        var comment = '';
        if (document.getElementById('txtComments')) {
            comment = document.getElementById('txtComments').value;
        }
        if (comment == '') {
            document.getElementById('dvCommentError').style.display = 'none';
            document.getElementById("rqtxtComments").style.display = 'block';
        }
    }
    var hidStar = document.getElementById('hidStar');
    //if (hidStar.value > 0) {

    //    var s = document.getElementById("ddlStars");
    //    var valueload = hidStar.value.replace(0, "") - 1;
    //    s.options[valueload].selected = true;
    //    document.getElementById('imgSelectStars').src = '/includes/theme/images/star' + hidStar.value.replace(0, "") + '0.png';
    //}
    //function getValueStar(str) {
    //    if (str == "") {
    //        hidStar.value = 5;
    //    }
    //    else {
    //        hidStar.value = str.replace(0, "");
    //    }

    //}
    function ValidateForm() {
        CheckForm();
        //return true;

    }
    function CheckForm() {
        document.getElementById('dvCommentError').style.display = 'none';
        var comment = '';
        var error = false;
        if (document.getElementById('txtComments')) {
            comment = document.getElementById('txtComments').value;
        }
        var commentCount = 0
        if (comment != '') {
            commentCount = countWords(comment);
        }
      
        if (commentCount < 10 && commentCount > 0) {
            ShowComment(document.getElementById('txtComments'))
            error = true;
        }
        if (error)
            return false;
        return true;
    }
</script>
<asp:Literal id="ltScript" runat="server"></asp:Literal>
</div>
</asp:Content>
