<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="MultiprojectSubtaskpopup.aspx.cs" Inherits="MultiprojectSubtaskpopup" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/MultipleDataSubtask.ascx" TagPrefix="uc1" TagName="MultipleDataSubtask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
          <h5 class="offcanvas-title"><%=Projectname%></h5>
      
        <div class="hstack gap-2 cls_btn">
            <button type="button" id="zoominidmulti" onclick="zoominmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutidmulti" onclick="zoomoutmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="closepopupmainmulti()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
    <div class="offcanvas-body p-0 overflow-hidden">
       <div style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
                <div class="card border-0">
                <uc1:MultipleDataSubtask runat="server" ID="MultipleDataSubtask" />
                </div>
            </div>
      </div>
    </div>

    <script>
        function zoominmulti() {
            window.parent.document.getElementById("btnclosemainpopupzoominmulti").click();
            document.getElementById("zoomoutidmulti").style.display = "block";
            document.getElementById("zoominidmulti").style.display = "none";            
        }
        function zoomoutmulti() {
            window.parent.document.getElementById("btnclosemainpopupzoomoutmulti").click();
            document.getElementById("zoominidmulti").style.display = "block";
             document.getElementById("zoomoutidmulti").style.display="none";            
        }
        function closepopupmainmulti() {
            window.parent.document.getElementById("btnclosemainpopupmulti").click();            
        }
    </script>


</asp:Content>



