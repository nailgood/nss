<%@ Page Language="VB" AutoEventWireup="false" CodeFile="order-write.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="ReviewOrderWrite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="review">
<div id="divReviewForm" runat="server">
<h1><label id="lbExcellence" runat="server">Write review for your recent purchase:</label> order #<label id="lblOrderNo" runat="server"></label></h1>
<div class="reviewform">
    <div class="BlockOrderReview">How was your experience?</div>
    <div id="group-radio">
        <div class="tbl-row">
            <div class="col-l">Item arrived on time?</div>
			<div class="col-r pad-r">
                <asp:RadioButtonList id="radArrived" runat="server" RepeatDirection="Horizontal" CssClass="radio-node">
			    <asp:ListItem Value="1" Selected="true"><i class="ico-radio"></i>Yes</asp:ListItem>
                <asp:ListItem Value="0"><i class="ico-radio"></i>No</asp:ListItem>
			    </asp:RadioButtonList>
			</div>
        </div>
        <div class="tbl-row">
            <div class="col-l">Item as describled?</div>
			<div class="col-r pad-r"><asp:RadioButtonList id="radDescribled" runat="server" RepeatDirection="Horizontal">
			    <asp:ListItem Value="1" Selected="true"><i class="ico-radio"></i>Yes</asp:ListItem>
                <asp:ListItem Value="0"><i class="ico-radio"></i>No</asp:ListItem>
			    </asp:RadioButtonList>
			</div>
        </div>
        <div class="tbl-row">
            <div class="col-l">Prompt and courteous service?</div>
			<div class="col-r" align="left">
			    <asp:RadioButtonList id="radServicelist" runat="server" RepeatDirection="Horizontal">
			        <asp:ListItem Value="1" Selected="true"><i class="ico-radio"></i>Yes</asp:ListItem>
                    <asp:ListItem Value="0"><i class="ico-radio"></i>No</asp:ListItem>
                    <asp:ListItem Value="2"><i class="ico-radio"></i>Did not contact</asp:ListItem>
			    </asp:RadioButtonList>
			</div>
        </div>
	</div>
    <div class="rating">
    <div class="pull-left">Rating <span class="red">*</span>:</div>
            <div class="pull-left">
		            <div class="pull-left ic-review">
                        <span id="imgSelectStars"><i class="fa fa-star fa-2" onmouseover="fnChangeStar(1);"></i><i class="fa fa-star fa-2" onmouseover="fnChangeStar(2);"></i><i class="fa fa-star fa-2" onmouseover="fnChangeStar(3);"></i><i class="fa fa-star fa-2" onmouseover="fnChangeStar(4);"></i><i class="fa fa-star fa-2" onmouseover="fnChangeStar(5);"></i></span>
<%--                        <img src="<%=Utility.ConfigData.CDNMediaPath %>/includes/theme/images/star<%=Rating %>.png" usemap="#selectstars" border="0" id="imgSelectStars" name="imgSelectStars" /> --%>

		            </div>
			        <div class="pull-left">
			        <label id="lbStar" runat="server"></label>
			        <label id="lbStarType" runat="server"></label>
                    <label id="lbMsgRating" runat="server" class="text-danger"  style="display:none;font-weight:normal;float:right">Rating is required.</label>
                    </div>
             </div>
        </div>
     <div class="form-group">
            <div class="col-sm-6 prComment">
                    <div class="re-bd">
                        <span class="note">Please do not include: HTML, references to other retailers, pricing, personal information, or any profane, inflammatory or copyrighted comments. Also, if you were given this product as a gift or otherwise compensated in exchange for writing this review,  you are REQUIRED to disclose it in your comments above.</span>
                    </div>
            </div>
            <div class="col-sm-6 txt-required" style="top:-7px">
                    <textarea id="txtComments" onkeyup="ShowComment(this)" name="txtComments" runat="server" class="form-control" placeholder="Order Comments" rows="6"></textarea>
            </div>
            <div class="div-error pull-left paderr">
                     <div id="dvMsgComment"  runat="server" class="text-danger"  style="display:none;"></div>
            </div>
           
    </div>
</div>
<br />
<div id="divSave" runat="server"><asp:Button ID="btnSave" runat="server" Text="Submit" onClientClick='return CheckForm();' class="btn btn-submit" ValidationGroup="review_Group"/></div>
<div class="note pad-note">After submitting your order review <%=DataLayer.SysParam.GetValue("CompanyName")%> will review it for approval and posting onto the site.</div>
			</div>
        <%--<map name="selectstars"><area shape="rect" coords="0,0,17,20" href="javascript:void(0);" alt="1 Star" 
        onmouseover="document.getElementById('imgSelectStars').src='/includes/theme/images/star10.png';document.getElementById('lbStar').innerHTML='10';getValueStar('1')" />
        <area shape="rect" coords="18,0,34,20" href="javascript:void(0);" 
        alt="2 Stars" 
        onmouseover="document.getElementById('imgSelectStars').src='/includes/theme/images/star20.png';document.getElementById('lbStar').innerHTML='20';getValueStar('2')" />
	    <area shape="rect" coords="35,0,52,20" href="javascript:void(0);" 
        alt="3 Stars" 
        onmouseover="document.getElementById('imgSelectStars').src='/includes/theme/images/star30.png';document.getElementById('lbStar').innerHTML='30';getValueStar('3')" />
		<area shape="rect" coords="53,0,69,20" href="javascript:void(0);" 
        alt="4 Stars" 
        onmouseover="document.getElementById('imgSelectStars').src='/includes/theme/images/star40.png';document.getElementById('lbStar').innerHTML='40';getValueStar('4')" />
        <area shape="rect" coords="70,0,86,20" href="javascript:void(0);" 
        alt="5 Stars" 
        onmouseover="document.getElementById('imgSelectStars').src='/includes/theme/images/star50.png';document.getElementById('lbStar').innerHTML='50';getValueStar('5')" />
		</map>--%>
<asp:Literal id="litMsg" runat="server"></asp:Literal>
<asp:HiddenField ID="hidStar" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hidStarLoad" runat="server"></asp:HiddenField>
<input type="hidden" id="hidCountMessage" name="hidCountMessage" runat="server" />
<input type="hidden" id="hidMinimumWordComment" name="hidMinimumWordComment" runat="server" />
<div id="dexist" runat="server" visible="false" class="thanks"><%=Resources.Msg.review_thanks%></div>
<script language="javascript">

    function ShowComment(ctr) {

        if (document.getElementById('hidCountMessage')) {
            var message = document.getElementById('hidCountMessage').value;
            if (document.getElementById('dvMsgComment')) {
                var minimumWord = document.getElementById('hidMinimumWordComment').value;
                var commentCount = countWords(ctr.value);
                if (parseInt(commentCount) < parseInt(minimumWord)) {
                    var text = FormatString(message + "," + minimumWord + "," + commentCount);
                    document.getElementById('dvMsgComment').innerHTML = text;
                    document.getElementById('dvMsgComment').style.display = '';
                    
                }
                else {
                    document.getElementById('dvMsgComment').style.display = 'none';

                }
            }

        }
    }
    function CheckForm() {

        document.getElementById('lbMsgRating').style.display = 'none';
        document.getElementById('dvMsgComment').style.display = 'none';
        var start = '';
        var comment = '';
        var error = false;
        if (document.getElementById('hidStar')) {
            start = document.getElementById('hidStar').value;
        }
        if (document.getElementById('txtComments')) {
            comment = document.getElementById('txtComments').value;
        }
        if (start == '') {
            document.getElementById('lbMsgRating').style.display = '';
            error = true;
        }
        var commentCount = 0
        if (comment != '') {
            commentCount = countWords(comment);
        }
        if (commentCount < 10) {
            ShowComment(document.getElementById('txtComments'))
            error = true;
        }
       
        if (error)
            return false;
        return true;
    }

    var hidStar = document.getElementById('hidStar');
    fnChangeStar = function (val) {
        // alert(val);
        var html = "";
        for (i = 1; i <= 5; i++) {
            if (i <= val) {
                html += "<i class='fa fa-star fa-2' onmouseover='fnChangeStar(" + i + ");'></i>";
            }
            else {
                html += "<i class='fa fa-star-o fa-1' onmouseover='fnChangeStar(" + i + ");'></i>";
            }
        }
        getValueStar(val);
        $("#imgSelectStars").html(html);
        hidStar.value = val;

    }
    $(document).ready(function () {
        getValueStar("");
    });

    function getValueStar(str) {
        if (str == "") {
            hidStar.value = 5;
            document.getElementById('lbStar').innerHTML = 5;
            document.getElementById('lbStarType').innerHTML = "(Excellent)";
            document.getElementById('lbExcellence').innerHTML = "We're glad you had a good experience with your "
        }
        else {
            var strType = '';
            hidStar.value = str;
            if (document.getElementById('lbMsgRating')) {
                document.getElementById('lbMsgRating').style.display = 'none';
            }
            document.getElementById('lbStar').innerHTML = str;
            switch (str) {
                case "1": strType = "(Awful)"; break;
                case "2": strType = "(Poor)"; break;
                case "3": strType = "(Neutral)"; break;
                case "4": strType = "(Good)"; break;
                case "5": strType = "(Excellent)"; break;
                default: strType = "(Excellent)";
            }
            document.getElementById('lbStarType').innerHTML = strType;
            if (str == 5)
                document.getElementById('lbExcellence').innerHTML = "We're glad you had a good experience with your "
            else
                document.getElementById('lbExcellence').innerHTML = "Write review for your recent purchase:"

        }

    }
</script>
</div>

</asp:Content>
