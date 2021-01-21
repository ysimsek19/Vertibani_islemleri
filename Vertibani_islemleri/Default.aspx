<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Vertibani_islemleri.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul id="test" runat="server">


    </ul>
    <input type="text" id="no" runat="server" />
    <input type="text" id="ad" runat="server" />
    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Kaydet</asp:LinkButton>
</asp:Content>
