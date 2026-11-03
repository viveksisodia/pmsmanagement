<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="Addtasklist.aspx.cs" Inherits="Addtasklist" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/Tasklist.ascx" TagPrefix="uc1" TagName="Tasklist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style>
.dataTables_scrollBody {min-height:300px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
    <div class="page-content" style="padding-bottom:20px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <uc1:Tasklist runat="server" ID="Tasklist" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>


