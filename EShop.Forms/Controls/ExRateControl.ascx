<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExRateControl.ascx.cs" Inherits="EShop.Forms.Controls.ExRateControl" %>
<%@ OutputCache Duration="7200" VaryByParam="none" Shared="true"%> 

<asp:MultiView ID="multiControlView" runat="server">
    <asp:View ID="viewExRates" runat="server">
        <asp:ListView ItemPlaceholderID="trExRateRow" ID="listOfRates" runat="server">
            <LayoutTemplate>
                <table cellpadding="3" cellspacing="0" border="1">
                    <tr>
                        <td colspan="2" class="title" style="background-color: #EEEEEE; font-size: 11px"> 
                            Exchenge's rate<br />(by BNB.BY)
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="trExRateRow"></asp:PlaceHolder>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td width="25%"><%#Eval ("Exchange") %></td>
                    <td><%# Eval ("Rate", "{0:N0}") %></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <div style="width:100%; font-size: 10px">
            Updated: <asp:Label ID="lblUpdateTime" runat="server" Text="Label"></asp:Label>
        </div>
    </asp:View>

    <asp:View ID="viewErrors" runat="server">
        <table cellpadding="3" cellspacing="0>
            <tr>
                <td class="title" style="background-color:#EEEEEE">Exchange's rate (by BNB.BY)</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblControlError" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
