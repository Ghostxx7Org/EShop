<%@ Page Title="" Language="C#" MasterPageFile="~/layout.master" AutoEventWireup="true" CodeBehind="ModelView.aspx.cs" Inherits="EShop.Forms.ModelView" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentLeftPanel" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentDataPanel" runat="server">
    <table width="100%" cellpadding="10" cellspacing="0">
        <tr>
            <td colspan="2">
                <h3> <%=SelectedModel != null ? SelectedModel.Title : "NaN"%></h3>
            </td>
            <tr>
                <td colspan="2">
                    <asp:Image ID="modelImage" ImageAlign="AbsMiddle" runat="server" />
                </td>
            </tr>
            <tr>
                <td width="20%">Description</td>
                <td>
                    <%=SelectedModel != null ? SelectedModel.Description : "NaN" %>
                </td>
            </tr>
            <tr>
                <td>Warranty</td>
                <td>
                    <%=SelectedModel !=null ? SelectedModel.Warranty.ToString(CultureInfo.InvariantCulture) : "NaN" %>
                </td>
            </tr>
            <tr>
                <td>Availability</td>
                <td>
                    <%=SelectedModel != null && SelectedModel.Availability != null ? SelectedModel.Availability.AvailabilityType : "NaN" %>
                </td>
            </tr>
            <tr>
                <td>Delivery</td>
                <td>
                    <%=SelectedModel != null && SelectedModel.Delivery !=null ? SelectedModel.Delivery.DeliveryType : "NaN" %>
                </td>
            </tr>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentRightPanel" runat="server">
    <ctrl:ExratesControl runat="server" ID="ctrolExRates" />
</asp:Content>
