<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="Add_Task_old.aspx.cs" Inherits="Add_Task" %>
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
                                        <div class="opp-ico purpel">
                                            <i class="ri-list-check-2"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p>Task</p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Task</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end">
                                                <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">All Task</span></a>                                             
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
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addtask"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">
                                            <a class="dropdown-item toggle-vis" data-column="0"><i class="ri-check-line align-middle fs-lg"></i> Select</a>
                                            <a class="dropdown-item toggle-vis" data-column="1"><i class="ri-check-line align-middle fs-lg"></i> S.No.</a>
                                            <a class="dropdown-item toggle-vis" data-column="2"><i class="ri-check-line align-middle fs-lg"></i> Task</a>
                                            <a class="dropdown-item toggle-vis" data-column="3"><i class="ri-check-line align-middle fs-lg"></i> Team</a>
                                            <a class="dropdown-item toggle-vis" data-column="4"><i class="ri-check-line align-middle fs-lg"></i> Status</a>
                                            <a class="dropdown-item toggle-vis" data-column="5"><i class="ri-check-line align-middle fs-lg"></i> Priority</a>
                                            <a class="dropdown-item toggle-vis" data-column="6"><i class="ri-check-line align-middle fs-lg"></i> Budget</a>
                                            <a class="dropdown-item toggle-vis" data-column="7"><i class="ri-check-line align-middle fs-lg"></i> Actual</a>
                                            <a class="dropdown-item toggle-vis" data-column="8"><i class="ri-check-line align-middle fs-lg"></i> Start Date</a>
                                            <a class="dropdown-item toggle-vis" data-column="9"><i class="ri-check-line align-middle fs-lg"></i> Target Date</a>
                                            <a class="dropdown-item toggle-vis" data-column="10"><i class="ri-check-line align-middle fs-lg"></i> Timeline</a>
                                            <a class="dropdown-item toggle-vis" data-column="11"><i class="ri-check-line align-middle fs-lg"></i> Completion Status</a>
                                            <a class="dropdown-item toggle-vis" data-column="12"><i class="ri-check-line align-middle fs-lg"></i> Sub Task</a>
                                            <a class="dropdown-item toggle-vis" data-column="13"><i class="ri-check-line align-middle fs-lg"></i> Issues</a>
                                            <a class="dropdown-item toggle-vis" data-column="14"><i class="ri-check-line align-middle fs-lg"></i> File</a>
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

<div class="offcanvas offcanvas-end" id="addtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
             <label id="lbltitle" runat="server">Add Task</label>   

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
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

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Task List &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" 
                                                ControlToValidate="tasklist" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   <%-- <select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Task List 1</option>
                                        <option>Task List 2</option>
                                    </select>--%>

                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="tasklist" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklist" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>
                                <div class="col-xxl-10 col-lg-10">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Task Name</label>
                                    </div>
                                    <input type="text" id="taskname" runat="server" placeholder="Task Name" class="form-control">
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
                                        <label class="form-label flex-grow-1">Task Description</label>
                                    </div>
                                    <textarea id="taskdescripton" runat="server" class="form-control" placeholder="Task Description"></textarea>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Owner &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Owner" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                  <%--  <input type="text" class="form-control">--%>

                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="owner" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Start Date</label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Startdate" runat="server" class="form-control">

                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Target Date</label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Targetdate" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Status  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="P_Status" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                  <%--  <select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Status 1</option>
                                        <option>Status 2</option>
                                        <option>Status 3</option>
                                    </select>--%>

                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Status" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Status 1</asp:ListItem>
                                          <asp:ListItem  value="2">Status 2</asp:ListItem>
                                          <asp:ListItem  value="3">Status 3</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnP_Status" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Priority &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 <%--   <select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Low</option>
                                        <option>High</option>
                                        <option>Medium</option>
                                    </select>--%>

                                       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Low</asp:ListItem>
                                          <asp:ListItem  value="2">High</asp:ListItem>
                                          <asp:ListItem  value="3">Medium</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Reminder</label>
                                    </div>
                                    <input type="text" id="Reminder" runat="server" placeholder="Reminder" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Occurrence</label>
                                    </div>
                                    <input type="text" id="Occurrence" runat="server" placeholder="Occurrence"  class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Tag To</label>
                                    </div>
                                    <input type="text" id="tagto" runat="server" placeholder="Tag To" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Team</label>
                                    </div>
                                    <%--<select id="select_emp2" class="form-control" multiple="multiple">
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
                                    </select>--%>


                                     <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
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

   <%-- <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
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
    function fsFunction(){
        var element = document.getElementById("addtask");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunction(){
        var element = document.getElementById("addtask");
        element.classList.remove("canvas-fullscreen");
    }
</script>
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
    <%--<script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
<script>
    const table = new DataTable('.datatable', {  
        scrollX:  true,
        scrollCollapse: true,
        paging:         true,
        searching: true,
    //    columnDefs : [
    //    { 'visible': false, 'targets': [18,19,20,21,22] }
    //]
    });
</script>
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
<script>
    $('#select_emp2').select2({
        dropdownParent: $("#addtask"),
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
                        
                        var taskdetails = {};
                        taskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; ;  
                        taskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        taskdetails.taskname = document.getElementById('<%=taskname.ClientID%>').value; 
                        taskdetails.taskdescripton = document.getElementById('<%=taskdescripton.ClientID%>').value; 
                        taskdetails.owner = document.getElementById('<%=owner.ClientID%>').value; 
                        taskdetails.Startdate = document.getElementById('<%=Startdate.ClientID%>').value; 
                        taskdetails.Targetdate = document.getElementById('<%=Targetdate.ClientID%>').value; 
                        taskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        taskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; 
                        taskdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 
                        taskdetails.Occurrence = document.getElementById('<%=Occurrence.ClientID%>').value; 
                        taskdetails.tagto = document.getElementById('<%=tagto.ClientID%>').value; 
                        taskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                        
                        $.ajax({
                            type: "POST",
                            url: 'Add_Task.aspx/SaveTask',
                            data: '{taskdetails: ' + JSON.stringify(taskdetails) + '}',
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
                    $("[id*=tasklist]").val("0");
                    $("[id*=taskname]").val("");
                    $("[id*=taskdescripton]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                    $("[id*=Targetdate]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("");
                   
                    document.getElementById("pnlimg").style.display = "none";
                    document.getElementById("divsub").style.display = "block";
                    document.getElementById("btncls").click();
                    Bindtask_data();
                   

               
                    
                }



          function Cleardata() {
                        
              
                   $("[id*=tasklist]").val("0");
                    $("[id*=taskname]").val("");
                    $("[id*=taskdescripton]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                    $("[id*=Targetdate]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("");
              
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "";
                    document.getElementById('<%=hdnowner.ClientID%>').value ="";
                    document.getElementById('<%=hdnstartdate.ClientID%>').value ="";
                 document.getElementById('<%=hdntargetdate.ClientID%>').value = "";
                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
                document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                document.getElementById('<%=hdnteam.ClientID%>').value ="";

                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Task";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'Add_Task.aspx/Bindtaskdetail',
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
                var Bindtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = Bindtaskdetail.find("id").text();
                    
                    
                    $("[id*=tasklist]").val(Bindtaskdetail.find("tasklist").text());
                    $("[id*=taskname]").val(Bindtaskdetail.find("taskname").text());
                    $("[id*=taskdescripton]").val(Bindtaskdetail.find("taskdescripton").text());
                    $("[id*=owner]").val(Bindtaskdetail.find("owner").text());
                    $("[id*=Startdate]").val(Bindtaskdetail.find("Startdate").text());
                    $("[id*=Targetdate]").val(Bindtaskdetail.find("Targetdate").text()); 
                    $("[id*=P_Status]").val(Bindtaskdetail.find("P_Status").text()); 
                    $("[id*=Priority]").val(Bindtaskdetail.find("Priority").text()); 
                    $("[id*=Reminder]").val(Bindtaskdetail.find("Reminder").text()); 
                    $("[id*=Occurrence]").val(Bindtaskdetail.find("Occurrence").text()); 
                    $("[id*=tagto]").val(Bindtaskdetail.find("tagto").text()); 
                    $("[id*=team]").val(Bindtaskdetail.find("team").text()); 

                     document.getElementById('<%=hdntasklist.ClientID%>').value = Bindtaskdetail.find("tasklist").text();
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = Bindtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = Bindtaskdetail.find("Targetdate").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindtaskdetail.find("team").text();
                  
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit  Task";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       



    <script>
        $( document ).ready(function() {
            Bindtask_data();
});
         function Bindtask_data() {
            
                debugger;
                  $.ajax({
                    url: 'Add_Task.aspx/bindtaskdata',                  
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
                    url: 'Add_Task.aspx/Deletetaskdata',
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
            Bindtask_data();
        }

    </script>

  

    <%--save data end  --%> 


     <%--Multi dropdown select start --%> 
    <script>
    debugger
    var ddLOwner = $(document.getElementById("<%=team.ClientID%>"));
   

    $(ddLOwner).select2({
        data: [
    <%=Multiteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        width: "100%"
    });

   
     $(document.getElementById("<%=team.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=team.ClientID%>")).val();
        document.getElementById("<%=hdnteam.ClientID %>").value = ids;
        document.getElementById("<%=team.ClientID%>").click();

    });
   
  
    </script>

    <%--Multi dropdown select start end--%> 
</asp:Content>







