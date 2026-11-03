<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="MultiProjectTaskPopup.aspx.cs" Inherits="MultiProjectTaskPopup" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/MultipleData.ascx" TagPrefix="uc1" TagName="MultipleData" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" Runat="Server">
      <style>
.dataTables_scrollBody {min-height:250px;}
</style>
    <script>
function edittext(strlblid)
{
debugger
var txtid='txtm_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
}
function addrowUpDown(straddon,currentindex,strpid,strorderno)
{
//alert("yes");
debugger
$.ajax({
                    url: '/pms/MultiProjectTask.aspx/addnewmilestoneblank',
                    data: "{ 'addon': '" + straddon + "','currentindex': '" + currentindex + "','pid': '" + strpid + "','orderno': '" + strorderno + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success:function (response) { 
debugger                      
                       // alert(response.d);
                        if(response.d!="0"&&response.d!="")
                        {
document.getElementById('add2').style.display='none';
document.getElementById('add1').style.display='';
                        Bindtask_data();
                        }
                    },
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
}


    </script>
 </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title"><%=Projectname%></h5>

       <%-- <div class="offcanvas-header border-bottom" style="padding:22px 24px;">
        <h5 class="offcanvas-title"></h5>--%>

        <div class="hstack gap-2 cls_btn">
            <button type="button" id="zoominidmulti" onclick="zoominmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutidmulti" onclick="zoomoutmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="closepopupmainmulti()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
    <div class="offcanvas-body p-0 overflow-hidden">
       <%-- <div data-simplebar style="height: calc(100vh - 57px);">   --%>
        <div style="height:100%;">   
            <div class="acitivity-timeline p-0">
                <div class="card border-0">
                     <uc1:MultipleData runat="server" ID="MultipleData" />
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



