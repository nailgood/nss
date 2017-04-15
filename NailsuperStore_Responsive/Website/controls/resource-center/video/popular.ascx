<%@ Control Language="VB" AutoEventWireup="false" CodeFile="popular.ascx.vb" Inherits="controls_resource_center_video_popular" %>

<section id="vpopular" runat="server">
    <div class="title">Newest Video</div>
    <div class="boxcontent">
        <asp:Repeater ID="lstVideoPopular" runat="server">
            <ItemTemplate>
                <div class="dvPopular">
                    <div class="mpImg">
                        <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
                    </div>
                    <div class="mpbox">
                        <div class="mTitle">
                        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
                        </div>                
                        <div class="dvDate">
                            <div class="viewTotal"><span id="iconview"></span><asp:Literal ID="ltrViewCount" runat="server"></asp:Literal></div>
                            <div class="viewTotal"><span id="ivote"></span><asp:Literal ID="ltrVoteCount" runat="server"></asp:Literal></div>
                        </div>  
                    </div>                              
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>

