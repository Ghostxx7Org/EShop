<%@ Page Title="" Language="C#" MasterPageFile="~/layout.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EShop.Forms.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentLeftPanel" runat="server">
    <asp:ListView ID="categoryList" ItemPlaceholderID="rowPlaceHolder" runat="server">
        <LayoutTemplate>
            <div width="100%">
                <asp:PlaceHolder ID="rowPlaceHolder" runat="server"></asp:PlaceHolder>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div style="text-align: left; font-size: 12px">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# string.Format("~/default.aspx?category={0}", Eval("Id")) %>'><%#Eval("Name") %>
                </asp:HyperLink> 
            </div>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentDataPanel" runat="server">
    <asp:DataPager PagedControlID="modelList" PageSize="10" QueryStringField="page" ID="modelListPager" runat="server">
        <Fields>
            <asp:NumericPagerField ButtonCount="10" PreviousPageText="<-" NextPageText="->" />
        </Fields>
    </asp:DataPager>

    <asp:ListView runat="server" ID="modelList" ItemPlaceholderID="modelItemView" EnableViewState="false">
        <LayoutTemplate>
            <div style="overflow: hidden">
                <ul class="model_list">
                    <asp:PlaceHolder ID="modelItemView" runat="server"></asp:PlaceHolder>
                </ul>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <div style="width: 140px; height: 180px; text-align: center; border: solid 1px silver; padding: 5px;">
                    <div style="height: 100px; vertical-align: top;">
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# string.Format("~/modelview.aspx?model={0}", Eval("Id")) %>'>
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# string.Format("~/model.img?id={0}&mode=prev",Eval("ImageId")) %>' />
                        </asp:HyperLink>
                    </div>
                    <div style="font-size: 10px; text-align: left;">
                        <b><%# Eval("Title").ToString().Length > 45 ? string.Concat(Eval("Title").ToString().Substring(0,45),"...") : Eval("Title") %></b>
                    </div>
                    <div>
                        <%# string.Format("$ {0:f2}", Eval("Price")) %>
                    </div>
                </div>
            </li>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentRightPanel" runat="server">
    <ctrl:ExratesControl runat="server" ID="ctrolExRates" />
</asp:Content>
