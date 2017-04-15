<%@ Page Language="VB" AutoEventWireup="false" CodeFile="policy.aspx.vb" Inherits="Policy_Detail" MasterPageFile="~/includes/masterpage/interior.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="dContent">
        <h1><asp:Literal ID="litTitle" runat="server"></asp:Literal></h1>

        <div class="desc">              
            <div id="divContent" runat="server"></div>
        </div>
    </div>
<style type="text/css">
#my-soon-counter {background-color:#ffffff;}
#my-soon-counter .soon-reflection {background-color:#ffffff;background-image:linear-gradient(#ffffff 25%,rgba(255,255,255,0));}
#my-soon-counter {background-position:top;}
#my-soon-counter {color:#929292; height:100px; width:100%; }


</style>
</asp:Content>


