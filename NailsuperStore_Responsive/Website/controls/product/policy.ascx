<%@ Control Language="VB" AutoEventWireup="false" CodeFile="policy.ascx.vb" Inherits="controls_policy" %>
<asp:Repeater ID="rptPolicy" runat="server">
    <ItemTemplate>
        <div class="policy">
            <asp:Literal ID="litPolicy" runat="server"></asp:Literal>
            <asp:HiddenField ID="hidPolicyContent" runat="server"/>
        </div>
        <div class="policyline"></div>
    </ItemTemplate>
</asp:Repeater>