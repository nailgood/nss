<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="tips_default" MasterPageFile="~/includes/masterpage/interior.master"  %>
<%@ Register Src="~/controls/resource-center/tip.ascx" TagName="Tip" TagPrefix="ucTip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<div id="page">
        <h1>Expert Tips & Advice</h1>
                <div id="ShowLanguage" runat="server">
                    <span class="rtpad">
                        <asp:LinkButton runat="server" CssClass="lgry" id="lnkEnglish" style="font-weight: normal;">ENGLISH</asp:LinkButton><asp:Label
                            runat="server" cssclass="black noul" ID="lblEnglish" style="font-weight: normal;">ENGLISH</asp:Label></span>
                    <span class="rtpad">
                        <asp:LinkButton runat="server" CssClass="lgry" id="lnkVietnamese" style="font-weight: normal;">VIETNAMESE</asp:LinkButton><asp:Label
                            runat="server" cssclass="black noul" ID="lblVietnamese" style="font-weight: normal;">VIETNAMESE</asp:Label></span>
                </div>                                
                <div class="form-group tips">
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="F_Text" type="text" CssClass="form-control" placeholder="Please provide search criteria below"></asp:TextBox>
                    </div>
                    <div class="n-dropdown col-sm-4 tipscate">
                        <asp:DropDownList ID="F_TipCategoryId" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                     <div class="div-error" style="clear:both; padding-top:5px;">
                        <asp:CustomValidator ID="cusSearch" runat="server" ClientValidationFunction="CheckSearch"
                                    CssClass="text-danger" ControlToValidate="F_Text" Display="Dynamic" ErrorMessage="* Search criteria is required."
                                    ValidateEmptyText="True" ValidationGroup="valContact"></asp:CustomValidator>
                    </div>
                    <div id="dvSearchTip">
                        <asp:Button runat="server" ID="btnSearch" data-btn="submit" Text="Submit" CssClass="btn btn-submit" />  
                        <input class="btn btn-submit" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />                                      
                    </div>
                </div>
                <div id="lstTipCategory">                               
                    <asp:Repeater ID="rptCategory" runat="server">
                        <ItemTemplate>
                            <ucTip:Tip ID="dvTip" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                    </div>
                    <input type="hidden" id="hidCategoryId" value="" />
                    <input type="hidden" id="hidVideoIndex" clientidmode="Static" value="<%=hidTipsIndex %>" />
                    <input type="hidden" id="hiCountdataVideo" value="<%=TotalRecords %>" />
                    <input type="hidden" id="hidPageSizeVideo" value="<%=PageSize %>" />
                    <input type="hidden" id="hidUrlVideo" value="<%=HttpContext.Current.Request.Url.ToString() %>" />
        <div style="display:block;text-align:center" id="imgloadingVideo">
            <img id="loader" alt="" src="/includes/theme/images/loader.gif" style="display: none" />
        </div>
        <div id="loadmoreVideo" onclick="GetRecordsResourceCenter(0,pgsize);" class="see-more-items"  style="display:none">
            <%=Resources.Msg.SeeMoreData%>
        </div>
   </div>
   <script type="text/javascript">
       var pgsize = document.getElementById('hidPageSizeVideo').value,
        pageIndex = 1,
        pageCount = document.getElementById('hiCountdataVideo').value,
        isLoading = 0;
       if (pageCount - pageIndex * pgsize > 0) {
           $("#loadmoreVideo").show();
       }
       $(window).scroll(function () {
           ScrollDataResourceCenter();
       });
       $(window).load(function () {
           DelayResetHeightListVideo(false, 'tip');
       });
       $(window).resize(function () {
           DelayResetHeightListVideo(true, 'tip');
       });
       function CheckSearch(sender, args) {
           var txtsearch = document.getElementById('F_Text').value;
           var drpsearch = document.getElementById('F_TipCategoryId').value;
           if (txtsearch == '' && drpsearch == '') {
               args.IsValid = false;
               document.getElementById('cusSearch').innerHTML = '* Search criteria is required.';
               return;
           }
       }
   </script>
</asp:Content>