<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="Addmilestone.aspx.cs" Inherits="Addmilestone" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/Milestone.ascx" TagPrefix="uc1" TagName="Milestone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <style>
.dataTables_scrollBody {min-height:300px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div id="loading" style="
    position: fixed;
    width: 100%;
    height: 100%;
    background: rgba(255,255,255,0.8);
    z-index: 9999;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
">
    <img src="/pms/image/loading8.gif" alt="Loading..." />
    
</div>
    <script>
$(document).ready(function() {
    $('#loading').fadeOut('slow');
});
$(window).on('load', function() {
    // Hide the loading overlay when page + all resources have loaded
    $('#loading').fadeOut('slow');
});
    </script>
    <div class="page-content" style="padding-bottom:20px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <uc1:Milestone runat="server" ID="Milestone" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


