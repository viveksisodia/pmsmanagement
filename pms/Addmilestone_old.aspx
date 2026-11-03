<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="Addmilestone_old.aspx.cs" Inherits="Addmilestone" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<script type="text/javascript" language="javascript">
window.onload = function() {
  document.getElementById('myclass').className = 'dashboard-topbar-wrapper';
};
      </script> --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico green">
                                            <i class="ri-flag-2-line"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p>Milestone</p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Milestone</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end">
                                                <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">All Milestone</span></a>                                             
                                                <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Status 1</span></a>
                                                <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Status 2</span></a>
                                                <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Status 3</span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addmilestone"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">
                                            <a class="dropdown-item toggle-vis" data-column="0"><i class="ri-check-line align-middle fs-lg"></i> Select</a>
                                            <a class="dropdown-item toggle-vis" data-column="1"><i class="ri-check-line align-middle fs-lg"></i> S.No.</a>
                                            <a class="dropdown-item toggle-vis" data-column="2"><i class="ri-check-line align-middle fs-lg"></i> Milestone</a>
                                            <a class="dropdown-item toggle-vis" data-column="3"><i class="ri-check-line align-middle fs-lg"></i> Owner</a>
                                            <a class="dropdown-item toggle-vis" data-column="4"><i class="ri-check-line align-middle fs-lg"></i> Team</a>
                                            <a class="dropdown-item toggle-vis" data-column="5"><i class="ri-check-line align-middle fs-lg"></i> Status</a>
                                            <a class="dropdown-item toggle-vis" data-column="6"><i class="ri-check-line align-middle fs-lg"></i> Priority</a>
                                            <a class="dropdown-item toggle-vis" data-column="7"><i class="ri-check-line align-middle fs-lg"></i> Budget</a>
                                            <a class="dropdown-item toggle-vis" data-column="8"><i class="ri-check-line align-middle fs-lg"></i> Actual</a>
                                            <a class="dropdown-item toggle-vis" data-column="9"><i class="ri-check-line align-middle fs-lg"></i> Start Date</a>
                                            <a class="dropdown-item toggle-vis" data-column="10"><i class="ri-check-line align-middle fs-lg"></i> Target Date</a>
                                            <a class="dropdown-item toggle-vis" data-column="11"><i class="ri-check-line align-middle fs-lg"></i> Timeline</a>                        
                                            <a class="dropdown-item toggle-vis" data-column="12"><i class="ri-check-line align-middle fs-lg"></i> Dependency</a>
                                            <a class="dropdown-item toggle-vis" data-column="13"><i class="ri-check-line align-middle fs-lg"></i> Completion Status</a>
                                            <a class="dropdown-item toggle-vis" data-column="14"><i class="ri-check-line align-middle fs-lg"></i> Task</a>
                                            <a class="dropdown-item toggle-vis" data-column="15"><i class="ri-check-line align-middle fs-lg"></i> Issues</a>
                                            <a class="dropdown-item toggle-vis" data-column="16"><i class="ri-check-line align-middle fs-lg"></i> File</a>
                                            <a class="dropdown-item toggle-vis" data-column="17"><i class="ri-check-line align-middle fs-lg"></i> Add Columns</a>

                                        </div>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                                        <div class="dropdown-menu">
                                            <a class="dropdown-item" href="#">List</a>
                                            <a class="dropdown-item" href="#">Gantt</a>
                                        </div>
                                    </div>
                                    </div>
                                </div>                                
                            </div> 
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                            <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                             <div style="display:none;">
                              <asp:Button ID="btndelete" runat="server"    Text="delete"  />
                             </div>

                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>           
        </div>
    </div>

<div class="offcanvas offcanvas-end" id="addmilestone" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
          <label id="lbltitle" runat="server">Add Milestone</label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fs12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fse12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
             <div style="display:none"><button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
                            <div class="row">
                                <div style="display: none">
                                <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                <div class="col-xxl-10 col-lg-10">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Milestone &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Milestone" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Milestone" placeholder="Milestone" runat="server"  class="form-control">
                                </div>
                                <div class="col-xxl-2 col-lg-2">
                                    <div class="d-block align-items-center">
                                        <label class="form-label">&nbsp;</label>
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Copy From Existing">
                                            <label class="form-label">
                                                <input class="form-check-input" type="checkbox" />  Copy                                               
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Weightage %  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="Weightage" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" runat="server" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Start Date</label>
                                    </div>

                                       <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="StartDate" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnStartDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">End Date</label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="EndDate" runat="server" class="form-control">
                                             <asp:HiddenField ID="hdnEndDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Owner   &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Owner" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                  <%--  <input type="text" class="form-control">--%>
                                      <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Owner" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Access  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Access" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <%--<select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Staff</option>
                                        <option>Client</option>
                                    </select>--%>

                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Access" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Staff</asp:ListItem>
                                          <asp:ListItem  value="2">Client</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnAccess" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
  <%--  <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Save</a>
        <a href="javascript:void(0);" class="btn btn-secondary">Save & Add More</a>
    </div>--%>

    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img2" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                            
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>


</div>
<link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
   <%-- <script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
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
<script>
    function fs12Function(){
        var element = document.getElementById("addmilestone");
        element.classList.add("canvas-fullscreen");  
    }
    function fse12Function(){
        var element = document.getElementById("addmilestone");
        element.classList.remove("canvas-fullscreen");
    }
</script>
<%--<script>
    const table = new DataTable('.datatable', {  
    scrollX:  true,
    scrollCollapse: true,
    paging:         true,
    searching: true,
    columnDefs : [
            { 'bSortable': false, 'targets': [ 17 ] } 
        ]
    });
</script>--%>

    <script>
    const table = new DataTable('.datatable', {  
        scrollX:  false,
        scrollCollapse: true,
        paging:         true,
        searching: true
        
    });
</script>

<div id="addteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <div class="col-sm-12 mb-1">
                            <select id="select_emp1" class="form-control" multiple="multiple">
                                <option value="1" selected="selected">Christi</option>
                                <option value="2" selected="selected">Frank Hook</option>
                                <option value="3" selected="selected">Ruby</option>
                                <option value="4" selected="selected">Vivek Sisodia</option>
                                <option value="5" selected="selected">Avneesh</option>
                                <option value="6">Mohit Sharma</option>
                                <option value="7">Amit Kumar</option>
                                <option value="8">Jone Doe</option>
                                <option value="9">Avdhesh Singh</option>
                                <option value="10">Ranjay Varun</option>
                                <option value="11">Sachin Gupta</option>
                                <option value="12">Sanjeev</option>
                            </select>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="actualbudget" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Project Cost Sheet Actual Details</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                                              
                    </div>
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>

<script>    
document.querySelectorAll('a.toggle-vis').forEach((el) => {
    el.addEventListener('click', function (e) {
        e.preventDefault();
 
        let columnIdx = e.target.getAttribute('data-column');
        let column = table.column(columnIdx);
 
        // Toggle the visibility
        column.visible(!column.visible());
    });
});
$('.dropdown-item.toggle-vis').click(function (e) {
    if ($(this).hasClass("text-mute")) {
        $(this).removeClass("text-mute");
    }
    else {
        $(this).addClass("text-mute");
    }
});
</script>
<script>
    $('#select_emp1').select2({
        dropdownParent: $("#addteam"),
        placeholder: 'Select Employee',
	    width: '100%',
        closeOnSelect: false,
        allowClear: true,
    });   
</script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/momentjs/latest/moment.min.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
<link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />
<script>
$(function() {
  $('#daterange').daterangepicker({
    opens: 'center'
  });
});
</script>

    <%--save data start --%> 

    <script type="text/javascript">
                function showdivimg() {
                    if (Page_ClientValidate("btnclose")) {
                        if (confirm('Do you want Leave this page ?')) {   

                           Cleardata();
                            document.getElementById("btnclose").click();
                                
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
         }

      


          function SvaeData(val1A) {
                    debugger
                    if (Page_ClientValidate("btnsave")) {
                        
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimg").style.display = "block";
                            document.getElementById("divsub").style.display = "none";

                        }
                        else {
                            return false;
                        }
                        
                        var milestonedetails = {};
                        milestonedetails.id = document.getElementById('<%=hdnid.ClientID%>').value; ;  
                        milestonedetails.Milestone = document.getElementById('<%=Milestone.ClientID%>').value; 
                        milestonedetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value; 
                        milestonedetails.StartDate = document.getElementById('<%=StartDate.ClientID%>').value; 
                        milestonedetails.EndDate = document.getElementById('<%=EndDate.ClientID%>').value; 
                        milestonedetails.Owner = document.getElementById('<%=Owner.ClientID%>').value; 
                        milestonedetails.Access = document.getElementById('<%=Access.ClientID%>').value; 
                        $.ajax({
                            type: "POST",
                            url: 'Addmilestone.aspx/SaveMilestone',
                            data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: OnSuccess,
                            error: function (response) {
                                debugger
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                debugger
                                alert(response.responseText);
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                        return false;
                        //});


                    };
                }
                function OnSuccess(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=Milestone]").val("");
                    $("[id*=Weightage]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=EndDate]").val("");
                    $("[id*=Owner]").val("0");
                    $("[id*=Access]").val("0");
                   
                    document.getElementById("pnlimg").style.display = "none";
                    document.getElementById("divsub").style.display = "block";
                    document.getElementById("btncls").click();
                    Bindmilestone_data();
                   

               
                    
                }



          function Cleardata() {
                        
              
                   $("[id*=Milestone]").val("");
                    $("[id*=Weightage]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=EndDate]").val("");
                    $("[id*=Owner]").val("0");
                    $("[id*=Access]").val("0");
              
                    document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDate.ClientID%>').value ="";
                    document.getElementById('<%=hdnowner.ClientID%>').value ="";
                    document.getElementById('<%=hdnAccess.ClientID%>').value ="";

                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Milestone";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'Addmilestone.aspx/Bindmilestonedetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1(response) {
            
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = milestonedetails.find("id").text();
                    
                    
                    $("[id*=Milestone]").val(milestonedetails.find("Milestone").text());
                    $("[id*=Weightage]").val(milestonedetails.find("Weightage").text());
                    $("[id*=StartDate]").val(milestonedetails.find("StartDate").text());
                    $("[id*=EndDate]").val(milestonedetails.find("EndDate").text());
                    $("[id*=Owner]").val(milestonedetails.find("Owner").text());
                    $("[id*=Access]").val(milestonedetails.find("Access").text()); 
                    

                     document.getElementById('<%=hdnStartDate.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=hdnEndDate.ClientID%>').value = milestonedetails.find("EndDate").text();
                    document.getElementById('<%=hdnowner.ClientID%>').value = milestonedetails.find("Owner").text();
                    document.getElementById('<%=hdnAccess.ClientID%>').value = milestonedetails.find("Access").text();
                  
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Milestone";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       



    <script>
        $( document ).ready(function() {
            Bindmilestone_data();
});
         function Bindmilestone_data() {
            
                debugger;
                  $.ajax({
                    url: 'Addmilestone.aspx/bindMilestonedata',                  
                      data: '{}',
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetails(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
            table = new DataTable('.datatable', {
                scrollX: true,
        scrollCollapse: true,
        paging:         true,
        searching: true
        
    });
           
        }

    </script>


    <script>
        function deletedata(RecordId) {
            
                 if (confirm('Do you really want to delete the Entery?'))
                 {
                      
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                    url: 'Addmilestone.aspx/DeleteMilestonedata',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessdelete,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessdelete(response) {
            alert('Data Deleted !');
            Bindmilestone_data();
        }

    </script>

  

    <%--save data end  --%> 


</asp:Content>


