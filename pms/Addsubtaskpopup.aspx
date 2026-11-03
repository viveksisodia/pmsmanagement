<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="Addsubtaskpopup.aspx.cs" Inherits="Addsubtaskpopup" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/Subtask.ascx" TagPrefix="uc1" TagName="Subtask" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <style>
.dataTables_scrollBody {min-height:300px;}
</style>
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title"><%=Projectname%></h5>
        <div class="hstack gap-2 cls_btn">
            <button type="button" id="zoominid" onclick="zoomin()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutid" onclick="zoomout()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="closepopupmain()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
    <div class="offcanvas-body p-0 overflow-hidden">
        <%--<div data-simplebar style="height: calc(100vh - 57px);"> --%>  
              <div style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
                <div class="card border-0">
                    <uc1:Subtask runat="server" ID="Subtask" />
                </div>
            </div>
      </div>
    </div>

    <script>
        function zoomin() {
            window.parent.document.getElementById("btnclosemainpopupzoomin").click();
            document.getElementById("zoomoutid").style.display = "block";
            document.getElementById("zoominid").style.display = "none";            
        }
        function zoomout() {
            window.parent.document.getElementById("btnclosemainpopupzoomout").click();
            document.getElementById("zoominid").style.display = "block";
             document.getElementById("zoomoutid").style.display="none";            
        }
        function closepopupmain() {
            window.parent.document.getElementById("btnclosemainpopup").click();            
        }
    </script>

</asp:Content>
