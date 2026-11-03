<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="Addnewsubtask.aspx.cs" Inherits="Addnewsubtask" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<%@ Register Src="~/pms/ucontrol/Subtasknew.ascx" TagPrefix="uc1" TagName="Subtasknew" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Subtasknew runat="server" id="Subtasknew" />
    
</asp:Content>

