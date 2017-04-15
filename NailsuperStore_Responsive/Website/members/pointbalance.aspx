<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pointbalance.aspx.vb" Inherits="members_pointbalance" MasterPageFile="~/includes/masterpage/interior.master" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Import Namespace="Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <%="" %>
   <%If Not IsPopUp Then%>
     <div id="pointbalance">
        <h1 class="page-title">Cash Reward Points Balance</h1>
        <div class="title-line">Points Summary</div>
        <ul class="reward-point">
            <li>
                <div><strong>Total points available:</strong></div>
                <div><span class="point">
                    <%If (m_PointAvailable >= 0) Then%>
                    <%=SitePage.NumberToString(m_PointAvailable)%></span>
                <%=IIf(m_MoneyAvailable > 0, "(worth " & m_MoneyAvailable & " USD)", "")%>
                <%Else%>
                Invalid
                <%End If%>
                </div>
            </li>
            <li>
                <div>Total points pending:</div>
                <div><%=SitePage.NumberToString(m_PointPending)%></div>
            </li>
            <li>
                <div>Points accumulated in <%=String.Format("{0:Y}", DateTime.Now)%>:</div>
                <div><%=SitePage.NumberToString(m_PointInMonth)%></div>
            </li>            
            <li>
                <div>Points accumulated in <%=DateTime.Now.Year%>:</div>
                <div><%=SitePage.NumberToString(m_PointInYear)%></div>
            </li>            
            <li>
                <div>Points earned up to date:</div>
                <div><%=SitePage.NumberToString(m_PointsEarnedUpToDate)%></div>
            </li>
        </ul>
        <div class="note">
           <strong>Please Note:</strong> Points will be available 30 days after the invoice date
       </div>
        <%  If (countTrans > 0) Then%>
        <div class="title-line">Transactions</div>
        <asp:Repeater ID="rptTrans" runat="server">
            <HeaderTemplate>
                <div id="history">
                    <div class="header-row">
                        <div class="header h-group-name">
                            Date
                        </div>
                      <%--  <div class="header hidden h-group-name">
                            Trans ID
                        </div>--%>
                        <div class="header h-group-name">
                            <span class="hidden-xs">Transaction</span> Description
                        </div>
                        <div class="header h-group-name">
                            Amount
                        </div>
                        <div class="header h-group-name" style="max-width:100px;">
                            <span class="hidden-xs">Points</span> Debit
                        </div>
                        <div class="header h-group-name" style="max-width:110px;">
                            <span class="hidden-xs">Points</span> Earned
                        </div>
                    </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="header-row">
                    <div class="group-name text-center">
                        <%#DataBinder.Eval(Container.DataItem, "CreateDate", "{0:MM/dd/yyyy}")%>
                    </div>
                    <%--<div class="group-name text-center hidden">
                        <u><asp:Literal ID="ltrLink" runat="server"></asp:Literal></u>
                    </div>--%>
                    <div class="group-name text-left">
                       <%-- <%#Eval("Notes")%>--%>
                        <asp:Literal ID="ltrnotes" runat="server"></asp:Literal>
                    </div>
                    <div class="group-name text-right">
                       <asp:Literal ID="ltrAmount" runat="server"></asp:Literal>
                    </div>
                    <div class="group-name text-right debit">
                      <asp:Literal ID="ltrPointDebit" runat="server"></asp:Literal>
                    </div>
                    <div class=" group-name text-right">
                        <asp:Literal ID="ltrPointEarn" runat="server"></asp:Literal>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        <%  End If%>
    </div>
    <% Else%>
    <div id="popup">
        <ul class="reward-point" style="width:100%">
            <li>
                <div><strong>Total points available:</strong></div>
                <div><span class="point">
                    <%If (m_PointAvailable >= 0) Then%>
                    <%=SitePage.NumberToString(m_PointAvailable)%></span>
                <%=IIf(m_MoneyAvailable > 0, "(worth $" & m_MoneyAvailable & ")", "")%>
                <%Else%>
                Invalid
                <%End If%>
                </div>
            </li>
            <li>
                <div>Total points pending:</div>
                <div><%=SitePage.NumberToString(m_PointPending)%></div>
            </li>
            <li>
                <div>Points accumulated in <%=String.Format("{0:Y}", DateTime.Now)%>:</div>
                <div><%=SitePage.NumberToString(m_PointInMonth)%></div>
            </li>            
            <li>
                <div>Points accumulated in <%=DateTime.Now.Year%>:</div>
                <div><%=SitePage.NumberToString(m_PointInYear)%></div>
            </li>            
            <li>
                <div>Points earned up to date:</div>
                <div><%=SitePage.NumberToString(m_PointsEarnedUpToDate)%></div>
            </li>
        </ul>
    </div>
    <%End If%>

</asp:Content>
