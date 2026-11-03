<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="Add_Task.aspx.cs" Inherits="Add_Task" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/Task.ascx" TagPrefix="uc1" TagName="Task" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style>
.dataTables_scrollBody {min-height:300px;}
 .files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
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
                        <uc1:Task runat="server" ID="Task" />
                        
                    </div>
                </div>
            </div>           
        </div>
    </div>
    
     <script>
function loadsrcfiles(valframe ,valsrc,strtype) {
                debugger
                
                // alert(lblname);
                count++;
                
                try{
                document.getElementById(valframe).src = "";
                  
                }
                catch
                {}
                if (valsrc.includes('?')) {
                    document.getElementById(valframe).src = valsrc +"&count=" + count;
                }
                else {
                    document.getElementById(valframe).src = valsrc + "?count=" + count;
                }

                try {
                    if(strtype=="milestone")
                    document.getElementById('<%=lblfiles.ClientID%>').innerHTML = "Upload milestone documents";
if(strtype=="task")
                    document.getElementById('<%=lblfiles.ClientID%>').innerHTML = "Upload task documents";
if(strtype=="subtask")
                    document.getElementById('<%=lblfiles.ClientID%>').innerHTML = "Upload sub task documents";
                }
                catch {
                }
            }
</script>
<div id="addfiles" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content" style="height:500px;">
           <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">
                    <asp:Label ID="lblfiles" runat="server" Text="Upload Files"></asp:Label></h5>
                <button type="button" class="btn-close" id="closfiles" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                          <iframe id='inner_popupfiles' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
            
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
</asp:Content>







