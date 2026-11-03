<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Subtask2.ascx.cs" Inherits="ucontrol_Subtask" %>
 <%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
  
<style>
        .hierarchy-line {
            padding: 2px;
            margin-bottom: 1px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .complete {
            background-color: #28a745; /* Green */
            color: white;
        }

        .sub-complete span {
            background-color: #d4edda; /* Light green for sub-complete parts */
            padding: 3px 6px;
            border-radius: 3px;
        }

        .label {
            margin-right: 2px;
        }
    </style>

<%--    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
<div class="card border-0">--%>
    <div class="card-header" style="display:none">
        <div class="row">
            <div class="col-sm-auto">
                <div class="inner-head">
                    <div class="opp-ico purpel">
                        <i class="ri-list-check-2"></i>
                    </div>
                    <div class="head-txt">
                        <p><%=fns.getltext("Sub Task", "2")%></p>                                                    
                        <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside" style="display:none;">
                            <h2><label id="ContentPlaceHolder1_lblowner">All Sub Task</label> <i class="bi bi-caret-down-fill"></i></h2>
                        </a>
                       <div class="dropdown-menu dropdown-menu-end">
                        <a class="dropdown-item fs-sm" href="#" onclick="GetSubTask(0)"><span class="align-middle">All Sub Task</span></a>                         
                       <a class="dropdown-item fs-sm" href="#" onclick="GetSubTask(1)"><span class="align-middle">Status 1</span></a>
                      <a class="dropdown-item fs-sm" href="#" onclick="GetSubTask(2)"><span class="align-middle">Status 2</span></a>
                      <a class="dropdown-item fs-sm" href="#" onclick="GetSubTask(3)"><span class="align-middle">Status 3</span></a>
                     </div>

                    </div>
                </div>
            </div>
            <script type="text/javascript">

            //debugger
            var count = 1;
            function loadsrcedit2(valframe ,valsrc,stredit) {
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

                    document.getElementById('<%=lbltitle.ClientID%>').innerHTML = stredit;
                }
                catch {
                }
            }

      
        </script>
            <div class="col-sm mt-1" style="display:none">
                <div class="d-flex justify-content-sm-end">
                <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                   <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addsubtask" ><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>

                    
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                    <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                    <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">

                                          <a class="dropdown-item toggle-vis" runat="server"  id="subtask0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>
                                        
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Project", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Sub Task", "2")%></a>

                                          <a class="dropdown-item toggle-vis" runat="server"  id="subtask6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timer", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timer History", "2")%></a>

                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Description", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Dependency", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask10" data-column="10"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Weightage", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask15" data-column="15"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Extended Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask16" data-column="16"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Start Date & Time", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask17" data-column="17"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("End Date & Time", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask18" data-column="18"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask19" data-column="19"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask20" data-column="20"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issues", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask21" data-column="21"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("File", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask22" data-column="22"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task Duration (Hour)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask23" data-column="23"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Stage", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask24" data-column="24"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Work Hours (W)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask25" data-column="25"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timelog Total (T)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="subtask26" data-column="26"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Difference (W-T)", "2")%></a>

                    </div>
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                    <div class="dropdown-menu">
                        <a class="dropdown-item" href="#">List</a>
                        <a class="dropdown-item" href="#">Gantt</a>
                    </div>
                </div>
                </div>
            </div>                                
        </div> 
    </div>
    <div class="card-body" style="display:none">
        <div class="table-responsive">
            <div id="divgridview" style="width:100%;">
             <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                </div>
                             <div style="display:none;">
                              <asp:Button ID="btndelete" runat="server"    Text="delete"  />
                             </div>
        </div>
    </div>
<%--</div>
            </div>
      </div>
    </div>--%>
   
<div class="offcanvas offcanvas-end" id="addsubtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
             <label id="lbltitle" runat="server"><%=fns.getltext("Add Sub Task", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimg2()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
        </div>
    </div>
    <%--<iframe id='addnewdetails' src="" width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>--%>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
                            <div class="row">
                                 <div style="display: none">
                                <asp:Button ID="Button12" runat="server" Text="" OnClick="Button12_Click" />
                                     <asp:Button ID="btnload1" runat="server" Text="" OnClick="btnload1_Click" />
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                   <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
                                 <asp:HiddenField ID="hdnmilstoneid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntasklistid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntaskid" runat="server" Value="0" />

                                   <div class="col-xxl-12 col-lg-12" id="divproject">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectId" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ProjectId_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectId" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divmilestone">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneid1" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="Milestoneid_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid1" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>

                                 <div class="col-xxl-12 col-lg-12" id="divptasklist" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task List", "2")%></label>
                                    </div>
                               
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="tasklist" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="tasklist_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklist" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                               <div class="col-xxl-12 col-lg-12" id="divtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="task" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task ..." AllowSingleDeselect="true" AutoPostBack="true" onchange="GetWeitagebindDtls()" OnSelectedIndexChanged="task_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntask" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnweightage" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task Name", "2")%></label>
                                    </div>
                                    <input type="text" id="Subtaskname" runat="server" placeholder="Sub Task Name" class="form-control">
                                </div>


                          

                                <div class="col-xxl-12 col-lg-12" id="divtasktimeline" style="display:none;">
                               <asp:Label ID="lbltasktimeline" runat="server"></asp:Label>
                              </div>

                              <div class="col-xxl-12 col-lg-12" id="divsubtasktimeline" style="display:none;">
                            <asp:Label ID="lblsubtasktimeline" runat="server"></asp:Label>
                           </div>


                               
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task Description", "2")%></label>
                                    </div>
                                    <textarea id="SubTaskDescription" runat="server" placeholder="Sub Task Description" class="form-control"></textarea>
                                </div>

                               
                                  <div class="col-xxl-12 col-lg-12" id="divweitage">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchk(this);" class="form-control">
                                </div>

                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="owner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                      
                                </div>

                                 <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>&nbsp;&nbsp;
                                            
                                     
                                        </label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                     <input type="date" id="Startdate1" runat="server" class="form-control"  onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdnStartdate1" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;
                                            
                                         
                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="StartTime" runat="server" class="form-control"  onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;
                                            
                                          

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                          <input type="date" id="Targetdate1" runat="server" class="form-control"  onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdnTargetdate1" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                 <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;
                                            
                                        

                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="Timeline" runat="server" class="form-control"  onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                     <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour"  class="form-control">
                                </div>

                               

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="P_Status" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Status" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewprojectstatus" style="display:none">
                                          <input type="text" id="txtnewprojectstatus" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Status" runat="server" Value="0" />
                                      

                                </div>




                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Priority ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
                                         </cc1:DropDownListChosen>
                                              </ContentTemplate>
                                        </asp:UpdatePanel>
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                          

                                </div>


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budget" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actual" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divreminder">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp;</label>
                                    </div>
                                    
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Reminder" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetReminderDtls()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Based on Due Date</asp:ListItem>
                                          <asp:ListItem  value="2">Specific Date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnreminder" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divremindertype">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder Type", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ReminderType" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetRemindertypeDtls()">
                                         <asp:ListItem  value="1">Daily</asp:ListItem>
                                          <asp:ListItem  value="2">On due date</asp:ListItem>
                                          <asp:ListItem  value="3">Day(s) before due date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnReminderType" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="col-xxl-12 col-lg-12"  id="divDays">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Enter Day(s)", "2")%></label>
                                    </div>
                                    <input type="text" id="Days" runat="server" placeholder="Days" class="form-control">
                                     </div>


                                <div class="col-xxl-12 col-lg-12" id="divChooseDate">
                                 <div class="d-flex align-items-center">
                                 <label class="form-label flex-grow-1"><%=fns.getltext("Choose Date", "2")%></label>
                                 </div>
                                
                                     <asp:UpdatePanel ID="UpdatePanel12" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="ChooseDate" runat="server" class="form-control"  onclick="this.showPicker()" >
                                               <asp:HiddenField ID="hdnChooseDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                    <div class="col-xxl-12 col-lg-12" id="Divtime">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Time", "2")%></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="Time" runat="server" class="form-control"  onclick="this.showPicker()" >
                                         <asp:HiddenField ID="hdnTime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                   <div class="col-xxl-12 col-lg-12"  id="divNotifyusers">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Notify users", "2")%></label>
                                    </div>
                                
                                      <asp:DropDownList ID="Notifyusers" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Notify User ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnNotifyusers" runat="server" Value="0" />
                                     
                                    </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Occurrence", "2")%></label>
                                    </div>
                                    <input type="text" id="Occurrence" runat="server" placeholder="Occurrence"  class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Tag To", "2")%></label>
                                    </div>
                                    <input type="text" id="tagto" runat="server" placeholder="Tag To" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   

                                    <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>--%>
                                         <%--<ContentTemplate>--%>
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                          <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>


                                </div>  
                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button12" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Stage" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Stage ..." AllowSingleDeselect="true" onchange="AddNewStage()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewStage" style="display:none">
                                          <input type="text" id="txtnewstage" class="form-control" placeholder="Stage"  runat="server" />
                                                  <input  data-jscolor="{}" id="txtstageprintcolor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnstage" runat="server" Value="0" />
                                        

                                </div>
                                 <div style="display:none">
                             <asp:Button ID="btnuploadimg" runat="server" Text="" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:FileUpload ID="Uploaddrawing" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreviewnew();" multiple />
                                       <a href="#" id="imgpath" target="_blank" style="display:none"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>
                                    <div id="previewContainer" style="margin-top: 10px;"></div>
                                               

                                </div>
                            

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   

      <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right" style="display:none"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img2" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>
                        </div>
                    </div>
     
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg2()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose2" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>


</div>
<div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>

<div id="changeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" id="btnclsowner" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="hdnchangeid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                           <%-- <select class="form-control">
                                <option value="1">Christi</option>
                                <option value="2">Frank Hook</option>
                                <option value="3">Ruby</option>
                                <option value="4">Vivek Sisodia</option>
                                <option value="5">Avneesh</option>
                                <option value="6">Mohit Sharma</option>
                                <option value="7">Amit Kumar</option>
                                <option value="8">Jone Doe</option>
                                <option value="9">Avdhesh Singh</option>
                                <option value="10">Ranjay Varun</option>
                                <option value="11">Sachin Gupta</option>
                                <option value="12">Sanjeev</option>                                
                                <option value="13" selected="selected">Lewis Arnold</option>
                            </select>--%>


                               <asp:DropDownList ID="ddlchangeowner" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdncahngeowner" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnchnownrghtml" runat="server" Value="0" />




                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveowner" class="btn btn-primary" onclick="SvaeDataowner('saveowner');">Save</button>
            </div>
        </div>
    </div>
</div>


<div id="addteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" id="btnclsTeam" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                         <asp:HiddenField ID="hdnteameid" runat="server" Value="0" />
                        <div class="col-sm-12 mb-1">
                         <%--   <select id="select_emp1" class="form-control" multiple="multiple">
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

                             <asp:DropDownList ID="ddladdteam" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnaddteam" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnaddteamhtml" runat="server" Value="0" />
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveteam" class="btn btn-primary" onclick="SvaeDatateam('saveteam');">Save</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>

  <asp:HiddenField ID="hdnprojec_tid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnmilestone_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasklist_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntask_id" runat="server" Value="0" />
                         <asp:HiddenField ID="hdnsubtask_id" runat="server" Value="0" />
                         <asp:HiddenField ID="hdntransaction_id" runat="server" Value="" />
                          <asp:HiddenField ID="hdnstoptimer_id" runat="server" Value="" />

                          <asp:HiddenField ID="hdnprojecnots_tid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnmilestonenotes_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasklistnotes_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasknotes_id" runat="server" Value="0" />
                           <asp:HiddenField ID="hdnsubtasidknotes_id" runat="server" Value="0" />

                         <asp:HiddenField ID="hdnnotestransaction_id" runat="server" Value="" />
                          <asp:HiddenField ID="hdnpause_id" runat="server" Value="" />

<div id="addnotes" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Notes</h5>
                <button type="button" class="btn-close" id="btnclsNotes" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                        <div class="col-sm-12 mb-1">
                            <textarea id="Notes" runat="server" class="form-control"></textarea>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
               <button type="button" id="btnpausesaveNotes" class="btn btn-primary"  onclick="SvaePauseDetialss('savepauseNotes');">Save</button>
             
            </div>
        </div>
    </div>
</div>

<div id="timer_stop" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Timer has stopped</h5>
                <button type="button" class="btn-close" id="btnclsTimer" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <%--<h5 class="text-center">Timer has stopped</h5>--%>

                    <asp:HiddenField ID="hdnhourcount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnminutecount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnsecondcount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnmilisecondcount" runat="server" Value="0" />
                   

                    <div class="row"> 
                        <div class="col-sm-12 mb-2">
                            <div class="d-flex align-items-center">
                            </div>
                            <div class="timer-stopped">
                                <div class="startendhrs">
                                    <h4 class="mb-0"><span id="spnhr">00</span>:<span id="spnmin">00</span>:<span id="spnsec">00</span>:<span id="spnmil">00</span></h4>
                                    <span>Hours</span>
                                </div>
                                <div class="startendtime" style="display:none">
                                   <%-- Start Time - 04-01-2024 02:23 pm<br />
                                    End Time - 04-01-2024 03:37 pm--%>

                                      Start Time - <span id="spnStarttime"></span><br />
                                    End Time - <span id="spnEndtime"></span>

                                </div>
                                
                            </div>
                        </div>
                        <div class="col-xxl-3 col-lg-3" style="display:none">
                            <div class="form-check form-radio-primary">
                                <label class="form-check-label">
                                    <input value="" name="Billable" type="radio" id="" class="form-check-input" checked="checked">
                                    Billable
                                </label>
                            </div>
                        </div>
                        <div class="col-xxl-3 col-lg-3" style="display:none">
                            <div class="form-check form-radio-primary">
                                <label class="form-check-label">
                                    <input value="" name="Billable" type="radio" id="" class="form-check-input">
                                    Non Billable
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1 mt-2">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">Notes</label>
                            </div>
                        <%--    <textarea class="form-control">Timesheet log details:
Start Time - 04-01-2024 02:23 PM End time 04-01-2024 03:37 PM
Time spent - 00:05</textarea>--%>

                                <textarea id="TimerNotes" runat="server" class="form-control"></textarea>

                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveTimer" class="btn btn-primary" onclick="SvaeDataTimer('saveTimer');">Update</button>
                <button type="button" id="btnRemoveTimer" style="display:none" onclick="RemoveDataTimer('removeTimer');" class="btn btn-dark">Remove Timer</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>


<div id="adddependency" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Dependency</h5>
                <button type="button" class="btn-close" id="btnclsdependancy" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <div class="col-sm-12 mb-1" style="display:none">
                            
                            <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>
                                    <input type="text" disabled="disabled" id="txtdependencymilestone" placeholder="Milestone" runat="server"  class="form-control">
                                </div>
                        </div> 
                        
                         <asp:HiddenField ID="hdnchngdependencyid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">Task</label>
                            </div>
                            <%--<select id="select_Dtask" class="form-control" multiple="multiple">
                                <option value="1" selected="selected">Task Dependency</option>
                                <option value="2" selected="selected">Mohit Task</option>
                                <option value="3">Ruby Task</option>
                                <option value="4">Task New</option>
                                <option value="5" selected="selected">Sanjeev Task</option>
                                <option value="6">Task Old</option>
                                <option value="7">Task Design</option>
                                <option value="8">Task Layout</option>
                            </select>--%>


                             <asp:DropDownList ID="ddldependancy" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Dependecy ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdndependancy" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnchngdependcyhtml" runat="server" Value="0" />

                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
             <button type="button" id="btnsavedependancy"  class="btn btn-primary" onclick="Svaedependancy('savedepndcy');">Add</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>


<div id="ExtendedTimline_subtask" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended_subtask" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button5" runat="server" Text="" OnClick="Button5_Click" />
   </div>
                      <asp:HiddenField ID="hdnextendid_subtask" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml_subtask" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Extendatestrt_subtask" runat="server" class="form-control"  onclick="this.showPicker()" >
                                               <asp:HiddenField ID="hdnextndstrt_subtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Extendateend_subtask" runat="server" class="form-control"   onclick="this.showPicker()">
                                              <asp:HiddenField ID="hdnextndend_subtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveextended_subtask" class="btn btn-primary" onclick="SvaeExtended_subtask('saveExtended_subtask');">Save</button>
            </div>
        </div>
    </div>
</div>



<link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
   <%-- <script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
 <script src="/pms/assets/js/jscolor.js"></script>

<script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>


    <%--<script src="https://code.jquery.com/jquery-3.7.1.js"></script>
    <script src="https://cdn.datatables.net/2.0.1/js/dataTables.js"></script>
    <script src="https://cdn.datatables.net/staterestore/1.4.0/js/dataTables.stateRestore.js"></script>
    <script src="https://cdn.datatables.net/staterestore/1.4.0/js/stateRestore.dataTables.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.0/js/dataTables.buttons.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.0/js/buttons.dataTables.js"></script>--%>

        
    
    
    
    
    
<script>
    function closepopupmain() {
        window.parent.document.getElementById("btnclosemainpopup").click();            
    }
</script>
<%--<script>
    const table = new DataTable('.datatable', {  
    scrollX:  true,
    scrollCollapse: true,
    paging:         true,
    searching: true,
   
    });
</script>--%>




<asp:HiddenField ID="hdntype" runat="server" />
<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
            document.getElementById("divproject").style.display = "none";
            document.getElementById("divmilestone").style.display = "none";
            document.getElementById("divptasklist").style.display = "none";
            document.getElementById("divtask").style.display = "";

       // alert(document.getElementById("<%=task.ClientID%>").value);

         var RecordId=document.getElementById("<%=task.ClientID%>").value;
         
         $.ajax({
                    url: 'AddSubtask.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });




              function OnSuccessweitagedtlschkpagewise(response) {
            
            debugger
            try {
                var waitagechkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtls.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

             }



      //Owner Bind Task Wise;

        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiOwner_new',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwner = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

                                                         });

            },
            error: function (response) {
                //alert(response.responseText);
                alert(response.responseText);
            },
            failure: function (response) {
                alert(response.responseText);
            }
        });



       //Team Bind Task Wise;
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiTeam_new',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteam = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteam).select2({
                                                             data: data,
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



             //  Task Timeline //
            debugger
            document.getElementById("divtasktimeline").style.display = "block";
             var tski_d = document.getElementById("<%=task.ClientID%>").value;
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }

            //Sub Task Timeline //
             debugger
             document.getElementById("divsubtasktimeline").style.display = "block";
             var tski_d = document.getElementById("<%=task.ClientID%>").value;
             //alert(projec_id);
              debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindSubtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssubtask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesssubtask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblsubtasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }


      }
    

</script>


<script>


     function GetOwnerempdetails() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiOwner_new',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwner = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

                                                         });

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





     function GetTeamempdetails() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiTeam_new',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteam = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteam).select2({
                                                             data: data,
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

   
    function GetWeitagebindDtls() {

                  var RecordId=document.getElementById("<%=task.ClientID%>").value;

                  $.ajax({
                    url: 'AddSubtask.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschk,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschk(response) {
            
            debugger
            try {
                var waitagechkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtls.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }

            GetOwnerempdetails();
            GetTeamempdetails();

             document.getElementById("divtasktimeline").style.display = "block";
            Bindtasktimeline_data();
            document.getElementById("divsubtasktimeline").style.display = "block";
            BindSubtasktimeline_data();


         }


       
    }
    

</script>


<script>
   
    function waitagepercentchk(input)
    {
        
        if (input.value > 100)
        {
            
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>




<script>
   
    function checkdurationcalculate(input)
    {

        var ts_kid = document.getElementById("<%=task.ClientID%>").value;

       

                  $.ajax({
                    url: 'AddSubtask.aspx/BindcheckSubTaskdurationdetails',
                    data: "{ 'ts_kid': '" + ts_kid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessChksubtaskdurationlschk,
                    error: function (response) {
                     alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessChksubtaskdurationlschk(response) {
            
            debugger
           // alert(response.d);
            try {
                var SubTaskduraionchkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (SubTaskduraionchkdtls.find("Durationhourcal").text() != "")
                {

                    var Task_Duration = SubTaskduraionchkdtls.find("TaskDuration").text();
                    var Duration_hourcal = SubTaskduraionchkdtls.find("Durationhourcal").text();

                  
                    //alert(Task_Duration);
                    //alert(Duration_hourcal);
                    //alert(input.value);

                    if (Duration_hourcal > Task_Duration)
                    {
                        //alert('yes');
                        if (Task_Duration != 0)
                        {
                            if (parseInt(input.value) > Task_Duration) {

                                alert('Task hour can not greater Subtask hour');
                                input.value = 0;

                            }

                            var chktsk = parseInt(Task_Duration) + parseInt(input.value);
                            //alert(chktsk);
                            if (chktsk > Duration_hourcal) {

                                alert('Task hour can not greater Subtask hour');
                                input.value = 0;

                            }
                        }
                    }

                    if (Duration_hourcal < parseInt(input.value))
                    {
                         alert('Task hour can not greater Subtask hour');
                         input.value = 0;

                    }

                    if (Duration_hourcal == Task_Duration )
                    {
                        if (parseInt(input.value) > 0)
                        {
                            
                            alert('Task hour can not greater Subtask hour');
                            input.value = 0;
                        }
                    }

                       
                  
                    
                    
                }
                   
            }
            catch

            {

            }

           

         }





      

  }
    

</script>



<script>
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }

</script>

<script>
  function GetReminderDtls()
  {
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }


    if (document.getElementById('<%=Reminder.ClientID%>').value == "1")
    {
            document.getElementById("divremindertype").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            document.getElementById("divChooseDate").style.display = "none";

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "2")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            document.getElementById("divDays").style.display = "none";

      }


  } 

</script>

<script>
  function GetRemindertypeDtls()
  {
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "2")
    {
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divDays").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "1" && document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divChooseDate").style.display = "none";
            document.getElementById("divDays").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
  } 

</script>

<script>
    function checkdate(enterdate)
    {
        if (document.getElementById("<%=Startdate1.ClientID%>").value != "" && document.getElementById("<%=Targetdate1.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=Startdate1.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=Targetdate1.ClientID%>").value);
                              var todt = new Date(today2);

                               var t1 = new Date();
                               t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                               var t2 = new Date();
            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
            if (t1 > t2) {
                 alert("Enter Valid Date");
                document.getElementById(enterdate).value = "";
                document.getElementById(enterdate).focus();
                return false;
            }
            else {
return true;
            }
        }
        else
        {
            return true;
        }
    }
    function checkdatecomparisiontaskdate(enterdate) {
        debugger
        if (enterdate != "") {

            if (checkdate(enterdate)) {
                var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

               var taski_d = document.getElementById("<%=task.ClientID%>").value;



                $.ajax({
                    url: 'AddSubtask.aspx/Bindcheckstrtdatetaskdetails',
                    data: "{ 'taski_d': '" + taski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var subtaskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (subtaskstrtdatechkdtls.find("id").text() != "")
                        {
                            const today = new Date(subtaskstrtdatechkdtls.find("Startdate").text());
                            var fdt = new Date(today);
                            const today2 = new Date(subtaskstrtdatechkdtls.find("Targetdate").text());
                            var todt = new Date(today2);

                            var t1 = new Date();
                            t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                            var t2 = new Date();
                            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
                            if (fcompdate >= t1 && fcompdate <= t2)
                            {
                                return true
                            }
                            else
                            {
                                alert("Enter Valid Date");
                                document.getElementById(enterdate).value = "";
                                document.getElementById(enterdate).focus();
                                return false;
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });


            }

           
        }
    }

    
    

</script>




<asp:HiddenField ID="hdnsavetype" runat="server" />
 <%--save data start --%> 

    <script type="text/javascript">
                function showdivimg2() {
debugger
                    if (Page_ClientValidate("btnclose2")) {
                        if (confirm('Do you want Leave this page ?')) {   

                           try
{
                        document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/pms/assets/images/user.jpg";
                   
}
catch
{}

                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid1.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
                    document.getElementById('<%=hdntask.ClientID%>').value = "0";
                    document.getElementById('<%=Subtaskname.ClientID%>').value = "";
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                    $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=Startdate1.ClientID%>').value ="";
                    document.getElementById('<%=Targetdate1.ClientID%>').value = "";
                       document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "";
                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                     document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                  document.getElementById('<%=Reminder.ClientID%>').value = "";
                    document.getElementById('<%=Occurrence.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
                  document.getElementById('<%=tagto.ClientID%>').value = "";
                document.getElementById('<%=hdnteam.ClientID%>').value = "0";

                     document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";

                      $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
                
            
             $(document.getElementById("<%=team.ClientID%>")).val(0);
             $(document.getElementById("<%=team.ClientID%>")).change();

                  
                            document.getElementById("btnclose2").click();
                                
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
        }


         function AddNewProjectstatus()
        {
                debugger
                document.getElementById('<%=hdnP_Status.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatus").style.display = "none";
               if (document.getElementById('<%=P_Status.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatus").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatus.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColor.ClientID%>').value = "";
               }
        }


        function AddNewpriority()
        {
                debugger
                document.getElementById('<%=hdnPriority.ClientID%>').value = "0";
                 document.getElementById("dvnewpriority").style.display = "none";
               if (document.getElementById('<%=Priority.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "1";
                    document.getElementById("dvnewpriority").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewpriority.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').value = "";
               }
        }


       function AddNewStage()
        {
                debugger
                document.getElementById('<%=hdnstage.ClientID%>').value = "0";
                 document.getElementById("dvnewStage").style.display = "none";
               if (document.getElementById('<%=Stage.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnstage.ClientID%>').value = "1";
                    document.getElementById("dvnewStage").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewstage.ClientID%>').value = "";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').value = "";
               }
        }


      


          function SvaeData(val1A) {
                    debugger
              if (Page_ClientValidate("btnsave"))
              {
                        
                        document.getElementById('<%=hdnsavetype.ClientID%>').value = val1A;  

                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimg").style.display = "block";
                            document.getElementById("divsub").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }


                         //status save
                        
                        var statusid = "0";
                        if (document.getElementById('<%=hdnP_Status.ClientID%>').value != "" && document.getElementById('<%=hdnP_Status.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "AddSubtask.aspx/statussave",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatus.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        statusid = response.d;
                                    

                            },
                            error: function (response) {
                                debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            statusid = document.getElementById('<%=P_Status.ClientID%>').value;
                        }

                        // Priority type save

                         var Prioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "AddSubtask.aspx/AddPrioritytype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewpriority.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        Prioritytypeid = response.d;
                                    

                            },
                            error: function (response) {
                                debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            Prioritytypeid = document.getElementById('<%=Priority.ClientID%>').value;
                        }



                      // Stage save

                         var stageid = "0";
                        if (document.getElementById('<%=hdnstage.ClientID%>').value != "" && document.getElementById('<%=hdnstage.ClientID%>').value != "0" && document.getElementById('<%=txtnewstage.ClientID%>').value != "" && document.getElementById('<%=txtstageprintcolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "AddSubtask.aspx/AddStage",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewstage.ClientID%>').value) + ',Stagecolor: ' + JSON.stringify(document.getElementById('<%=txtstageprintcolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        stageid = response.d;
                                    

                            },
                            error: function (response) {
                                debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            stageid = document.getElementById('<%=Stage.ClientID%>').value;
                        }



                        // save all details
                        
                        var subtaskdetails = {};
                        subtaskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value;  
                        subtaskdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        subtaskdetails.Milestoneid = document.getElementById('<%=Milestoneid1.ClientID%>').value; 
                        subtaskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        subtaskdetails.task = document.getElementById('<%=task.ClientID%>').value; 
                        subtaskdetails.Subtaskname = document.getElementById('<%=Subtaskname.ClientID%>').value; 
                        subtaskdetails.SubTaskDescription = document.getElementById('<%=SubTaskDescription.ClientID%>').value; 
                        subtaskdetails.owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        subtaskdetails.Startdate = document.getElementById('<%=Startdate1.ClientID%>').value; 
                       subtaskdetails.Targetdate = document.getElementById('<%=Targetdate1.ClientID%>').value; 
                  

                        subtaskdetails.StartTime = document.getElementById('<%=StartTime.ClientID%>').value; 
                        subtaskdetails.Timeline = document.getElementById('<%=Timeline.ClientID%>').value; 
                        subtaskdetails.TaskDuration  = document.getElementById('<%=taskDuration.ClientID%>').value; 

                        <%--subtaskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        subtaskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>

                        subtaskdetails.P_Status = statusid;
                        subtaskdetails.Priority = Prioritytypeid;
                        subtaskdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        subtaskdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;

                        subtaskdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 

                        subtaskdetails.ReminderType = document.getElementById('<%=ReminderType.ClientID%>').value; 
                        subtaskdetails.Days = document.getElementById('<%=Days.ClientID%>').value; 
                        subtaskdetails.Time = document.getElementById('<%=Time.ClientID%>').value; 
                        subtaskdetails.Notifyusers = document.getElementById('<%=hdnNotifyusers.ClientID%>').value; 
                        subtaskdetails.ChooseDate = document.getElementById('<%=ChooseDate.ClientID%>').value; 

                        subtaskdetails.Occurrence = document.getElementById('<%=Occurrence.ClientID%>').value; 
                        subtaskdetails.tagto = document.getElementById('<%=tagto.ClientID%>').value; 
                       subtaskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                       subtaskdetails.Stage = stageid;
                       subtaskdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value; 
                       subtaskdetails.Uploaddrawing = document.getElementById('<%=hdnUploaddrawing.ClientID%>').value;
            
                        $.ajax({
                            type: "POST",
                            url: 'AddSubtask.aspx/SaveSubTask',
                            data: '{subtaskdetails: ' + JSON.stringify(subtaskdetails) + '}',
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
UploadFiles(response.d);
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=btnload1.ClientID%>').click();


                    //$("[id*=ProjectId]").val("0");
                    //$("[id*=Milestoneid]").val("0");
                    //$("[id*=tasklist]").val("0");
                    //$("[id*=task]").val("0");
                    //$("[id*=Subtaskname]").val("");
                    //$("[id*=SubTaskDescription]").val("");
                    //$("[id*=owner]").val("0");
                    //$("[id*=Startdate]").val("");
                    //$("[id*=Targetdate]").val("");
                    //$("[id*=P_Status]").val("0");
                    //$("[id*=Priority]").val("0");
                    //$("[id*=Reminder]").val("");
                    //$("[id*=Occurrence]").val("");
                    //$("[id*=tagto]").val("");
                    //$("[id*=team]").val("");

try
{
 document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/pms/assets/images/user.jpg";
                   
}
catch
{}

                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid1.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
                    document.getElementById('<%=hdntask.ClientID%>').value = "0";
                    document.getElementById('<%=Subtaskname.ClientID%>').value = "";
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                    $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=Startdate1.ClientID%>').value ="";
                    document.getElementById('<%=Targetdate1.ClientID%>').value = "";
                       document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "";
                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                     document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                  document.getElementById('<%=Reminder.ClientID%>').value = "";
                    document.getElementById('<%=Occurrence.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
                  document.getElementById('<%=tagto.ClientID%>').value = "";
                document.getElementById('<%=hdnteam.ClientID%>').value = "0";

                     document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";

                      $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
                
            
             $(document.getElementById("<%=team.ClientID%>")).val(0);
             $(document.getElementById("<%=team.ClientID%>")).change();

                  
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        BindSubtask_data();
                    }
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                       // document.getElementById("btncls").click();
                        BindSubtask_data();

                    }
                   

               
                    
                }



          function Cleardata() {
                        
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid1]").val("0");
                    $("[id*=tasklist]").val("0");
                    $("[id*=task]").val("0");
                    $("[id*=Subtaskname]").val("");
                    $("[id*=SubTaskDescription]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate1]").val("");
                    $("[id*=Targetdate1]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("0");

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid1.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
              document.getElementById('<%=hdntask.ClientID%>').value = "0";

                      document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnStartdate1.ClientID%>').value ="";
               document.getElementById('<%=hdnTargetdate1.ClientID%>').value = "";
             

                 document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "";

                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
              document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                document.getElementById('<%=Budget.ClientID%>').value = "";
                document.getElementById('<%=Actual.ClientID%>').value = "";
              document.getElementById('<%=hdnteam.ClientID%>').value = "0";


               document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";
              document.getElementById('<%=Weightage.ClientID%>').value = "";

                $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
                
            
            $(document.getElementById("<%=team.ClientID%>")).val(0);
             $(document.getElementById("<%=team.ClientID%>")).change();
           

                   document.getElementById('<%=Button12.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Sub Task";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'AddSubtask.aspx/Bindsubtaskdetail',
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
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                   
try
{
if (Bindsubtaskdetail.find("Uploaddrawing").text() != "" && Bindsubtaskdetail.find("Uploaddrawing").text() != "0")
                    {
                        
                        bindUploadedDrawings(Bindsubtaskdetail.find("Uploaddrawing").text());
                    }
}
catch
{}
                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindsubtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid1.ClientID%>').value = Bindsubtaskdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist.ClientID%>').value = Bindsubtaskdetail.find("tasklist").text();
                    document.getElementById('<%=hdntask.ClientID%>').value = Bindsubtaskdetail.find("task").text();
                    document.getElementById('<%=Subtaskname.ClientID%>').value = Bindsubtaskdetail.find("Subtaskname").text();
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = Bindsubtaskdetail.find("SubTaskDescription").text();
                    document.getElementById('<%=Budget.ClientID%>').value = Bindsubtaskdetail.find("Budget").text();
                    document.getElementById('<%=Actual.ClientID%>').value = Bindsubtaskdetail.find("Actual").text();
                    document.getElementById('<%=hdnreminder.ClientID%>').value = Bindsubtaskdetail.find("Reminder").text();
                    document.getElementById('<%=Occurrence.ClientID%>').value = Bindsubtaskdetail.find("Occurrence").text();
                    document.getElementById('<%=tagto.ClientID%>').value = Bindsubtaskdetail.find("tagto").text();
                    document.getElementById('<%=team.ClientID%>').value = Bindsubtaskdetail.find("team").text();
                    document.getElementById('<%=ReminderType.ClientID%>').value = Bindsubtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=Days.ClientID%>').value = Bindsubtaskdetail.find("Days").text();
                    document.getElementById('<%=Button12.ClientID%>').click();
                    $("[id*=Notifyusers]").val(Bindsubtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDate]").val(Bindsubtaskdetail.find("ChooseDate").text()); 
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindsubtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnStartdate1.ClientID%>').value = Bindsubtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdnTargetdate1.ClientID%>').value = Bindsubtaskdetail.find("Targetdate").text();
                    
                    document.getElementById('<%=hdnstarttime.ClientID%>').value = Bindsubtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = Bindsubtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDuration.ClientID%>').value = Bindsubtaskdetail.find("TaskDuration").text();

                    document.getElementById('<%=P_Status.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                     document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";


                    document.getElementById('<%=Priority.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindsubtaskdetail.find("team").text();


                    document.getElementById('<%=Stage.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
                    document.getElementById('<%=hdnstage.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
                    document.getElementById('<%=txtnewstage.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=Weightage.ClientID%>').value = Bindsubtaskdetail.find("Weightage").text();

                     document.getElementById('<%=hdnReminderType.ClientID%>').value = Bindsubtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=hdnTime.ClientID%>').value = Bindsubtaskdetail.find("Time").text();
                    document.getElementById('<%=hdnNotifyusers.ClientID%>').value = Bindsubtaskdetail.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDate.ClientID%>').value = Bindsubtaskdetail.find("ChooseDate").text();


                     // time and reminder fill and edit strt//

                     if (Bindsubtaskdetail.find("Reminder").text() == "0")
                     {
                       document.getElementById("divremindertype").style.display = "none";
                       document.getElementById("divDays").style.display = "none";
                       document.getElementById("Divtime").style.display = "none";
                       document.getElementById("divNotifyusers").style.display = "none";
                       document.getElementById("divChooseDate").style.display = "none";

                    }


             // Bindtasktimeline_data();
            //  Task Timeline //
            debugger
            document.getElementById("divtasktimeline").style.display = "block";
             var tski_d = Bindsubtaskdetail.find("task").text();
             //alert(tski_d);
               debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }


              // BindSubtasktimeline_data();
             //Sub Task Timeline //
             debugger
             document.getElementById("divsubtasktimeline").style.display = "block";
             var tski_d = Bindsubtaskdetail.find("task").text();
             //alert(tski_d);
              debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindSubtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssubtask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesssubtask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblsubtasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }





                    GetReminderDtls();
                    GetRemindertypeDtls();
                    // time and reminder fill and edit end//


             if (Bindsubtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindsubtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }
          

           

             GetOwnerempdetails();
             if (Bindsubtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
                  else
                  {

                   var idsemp = Bindsubtaskdetail.find("owner").text();
                  $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
             
             }



              GetTeamempdetails();
             if (Bindsubtaskdetail.find("team").text() != "")
             {
                 debugger
                 if (Bindsubtaskdetail.find("team").text().includes(','))
                 {
                      var idsemp = Bindsubtaskdetail.find("team").text().split(',');
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = Bindsubtaskdetail.find("team").text();
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 

            }




                    
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Sub Task";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       

<asp:HiddenField ID="hdnbtns" runat="server" />

    <script>
         function autoplay() {

             if (document.getElementById('<%=hdnbtns.ClientID%>').value != "") {
                 var txtids = document.getElementById('<%=hdnbtns.ClientID%>').value.split(',');
                 if (txtids.length > 0) {
                     for (var i = 1; i <= txtids.length; i++) {
                         document.getElementById('btnauto_' + i.toString()).click();
                     }
                 }
             }
                //alert("ji");
       
       
        }

        $( document ).ready(function() {
           // BindSubtask_data();
});
         function BindSubtask_data() {
            
             debugger;
               //  alert($("[id*=hdiprojid]").val());
               //alert($("[id*=hdnmilstoneid]").val());
               //alert($("[id*=hdntasklistid]").val());
               //alert($("[id*=hdntaskid]").val());

                  $.ajax({
                    url: 'AddSubtask.aspx/bindSubtaskdata',                  
                      //data: '{}',
                   //  data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                           data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "' ,'milstoneid': '" + $("[id*=hdnmilstoneid]").val() + "' ,'tasklistid': '" + $("[id*=hdntasklistid]").val() + "' ,'taskid': '" + $("[id*=hdntaskid]").val() + "' }",

                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetailssubtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetailssubtsk(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
            try {
                table = new DataTable('.datatable', {
                    scrollX: true,
                    scrollY: 300,
                    scrollCollapse: true,
                    paging: true,
                    searching: true,

                         fixedColumns: {
            leftColumns: 6
        }

                });
            }

             catch
            { }
            //alert("hi");
            //autoplay(); 
              //hidecolumngrid();

             
           
        }

    </script>

<script>
    
    $(document).ready(function ()
    {
            Bindtasktimeline_data();
});
    function Bindtasktimeline_data()
    {
             debugger
            
             
             var tski_d = document.getElementById("<%=task.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccesstask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }

    </script>

<script>
    
    $(document).ready(function ()
    {
            BindSubtasktimeline_data();
});
    function BindSubtasktimeline_data()
    {
             debugger
            
             
             var tski_d = document.getElementById("<%=task.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindSubtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssubtask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccesssubtask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblsubtasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
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
                    url: 'AddSubtask.aspx/Deletesubtaskdata',
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
            BindSubtask_data();
        }

    </script>

  

    <%--save data end  --%> 



 <%--Owner details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeDataowner(val12A) {

            if (Page_ClientValidate("btnsaveowner")) {
               
                debugger


                // save all details

               <%-- var Bindsubtaskdetail = {};
                Bindsubtaskdetail.id = document.getElementById('<%=hdnchangeid.ClientID%>').value;
                Bindsubtaskdetail.owner = document.getElementById('<%=hdncahngeowner.ClientID%>').value;--%>

                    $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/Save_Owner',
                   // data: '{Bindsubtaskdetail: ' + JSON.stringify(Bindsubtaskdetail) + '}',
                    data: "{ 'id': '" + document.getElementById('<%=hdnchangeid.ClientID%>').value + "' ,'ownerdropdownid': '" + document.getElementById('<%=hdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessownerchange,
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
                function OnSuccessownerchange(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=ddlchangeowner]").val("");
                   
                     document.getElementById('<%=hdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();
                    document.getElementById("btnclsowner").click();
                   // BindSubtask_data();
                    document.getElementById("lblowner_" + document.getElementById("<%=hdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Owner(RecordId,lblownerid) {
              
               debugger;
               document.getElementById("<%=hdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
             
                $.ajax({
                    url: 'AddSubtask.aspx/Binddetail_Owner',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1owner,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1owner(response) {
            
            debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = Bindsubtaskdetail.find("owner").text();

              document.getElementById('<%=multichngtaskid.ClientID%>').value = Bindsubtaskdetail.find("task").text();
              Multichangeowner();
              if (Bindsubtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text();
                $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();
                  }
             
            }






             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    
      <%--Owner details bind and update data end  --%>


<%--Team details bind and update data start --%>

    <script type="text/javascript">
             
        function SvaeDatateam(val13A) {

            if (Page_ClientValidate("btnsaveteam")) {
               
                debugger


                // save all details

             <%--   var Bindsubtaskdetail = {};
                Bindsubtaskdetail.id = document.getElementById('<%=hdnteameid.ClientID%>').value;;
                Bindsubtaskdetail.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;--%>

                $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/Save_Team',
                    //data: '{Bindsubtaskdetail: ' + JSON.stringify(Bindsubtaskdetail) + '}',
                    data: "{ 'id': '" + document.getElementById('<%=hdnteameid.ClientID%>').value + "' ,'teamdropdownid': '" + document.getElementById('<%=hdnaddteam.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTeam,
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
                function OnSuccessTeam(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=ddladdteam]").val("");
                   
                     document.getElementById('<%=hdnaddteam.ClientID%>').value = "";
                     $(document.getElementById("<%=ddladdteam.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddladdteam.ClientID%>")).change();
                    document.getElementById("btnclsTeam").click();
                  //  BindSubtask_data();
                     document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Team(RecordId,lbluserid) {

          
                debugger;
               document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];

                $.ajax({
                    url: 'AddSubtask.aspx/Bindetail_Team',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1team,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1team(response) {
            
            debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = Bindsubtaskdetail.find("team").text();
                    
              document.getElementById('<%=multichngtaskid.ClientID%>').value = Bindsubtaskdetail.find("task").text();
              MultichangeTeam();
              if (Bindsubtaskdetail.find("team").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("team").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("team").text().split(',');
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindsubtaskdetail.find("team").text();
                $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=ddladdteam.ClientID%>")).change();
                  }
             
            }


             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>

    <%--Team details bind and update data end  --%>


 <%--status and propery  update start--%>
    <asp:HiddenField ID="hdsubtaskid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
                   var subtaskid=document.getElementById("<%=hdsubtaskid.ClientID%>").value;
                   //alert(statusid);
                   //alert(iscomplete);
                    $.ajax({
                    url: 'AddSubtask.aspx/Update_Status',
                    data: "{ 'subtaskid': '" + subtaskid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
function OnSuccessStatusupdt(response) {
            if (response.d.includes("~~")) {
                alert('Data Saved successfully.');
               var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value;
                var arrcol = response.d.split("~~");
                document.getElementById(btnid).innerHTML = arrcol[0];
                document.getElementById(btnid).style.backgroundColor = arrcol[1];
            }
            else {
                alert(response.d);
            }
        }
       <%-- function OnSuccessStatusupdt(response) {
            alert('Data Saved successfully.');
            BindSubtask_data();
            var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }--%>
        
        function setsubtaskstustid(subtaskid,btnststusid)
        {
            debugger
            document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdsubtaskid.ClientID%>").value = subtaskid;
        }
            </script>


      <asp:HiddenField ID="hdnpropertID" runat="server" Value="0" />
<asp:HiddenField ID="hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var taskid=document.getElementById("<%=hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'AddSubtask.aspx/UpdatePriority_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdt(response) {
            alert('Data Saved successfully.');
           // BindSubtask_data();
            var btnid = document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setpriorityid(tasklistid,btnpriortyid)
        {
            debugger
            document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=hdnpropertID.ClientID%>").value = tasklistid;
        }
            </script>

    <%--status and  proprety update end--%>


<%--Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeExtended_subtask(val225A) {

            if (Page_ClientValidate("btnsaveextended_subtask")) {
               
                debugger
                
                // save all details


                var subtask_id = document.getElementById('<%=hdnextendid_subtask.ClientID%>').value;
                var extendStrtdate_subtask = document.getElementById('<%=Extendatestrt_subtask.ClientID%>').value;
                var extendEnddate_subtask = document.getElementById('<%=Extendateend_subtask.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/UpdateExtendeddatedtls_subtask',
                    data: "{ 'subtask_id': '" + subtask_id + "' ,'extendStrtdate_subtask': '" + extendStrtdate_subtask + "','extendEnddate_subtask': '" + extendEnddate_subtask + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessupdtexdate_subtask,
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
                function OnSuccessupdtexdate_subtask(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=hdnextndstrt_subtask.ClientID%>').value = "";
                    document.getElementById('<%=hdnextendid_subtask.ClientID%>').value = "";
                    document.getElementById("btnclsextended_subtask").click();
                    //Bindproject_data();
                    debugger
                    
                   
                    document.getElementById("lblExtnddtsubtask_" + document.getElementById("<%=hdnextnhtml_subtask.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Bindextenddatesubtask_Details(RecordId,lblExtndid) {
              
              debugger;

              document.getElementById("<%=hdnextnhtml_subtask.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'AddSubtask.aspx/Bindsubtaskdetail_extendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate_subtask,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate_subtask(response) {
            // alert(response.d);
            debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnextendid_subtask.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    
                    document.getElementById('<%=hdnextndstrt_subtask.ClientID%>').value = Bindsubtaskdetail.find("StartDate").text();
                    document.getElementById('<%=hdnextndend_subtask.ClientID%>').value = Bindsubtaskdetail.find("TargetDate").text();
                     document.getElementById('<%=Button5.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }

         

            </script>
    
      <%--Extended date details bind and update data end  --%>




<%--All stage update start--%>

 <asp:HiddenField ID="hdntaskstageid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtn_stageid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStagedeatils(Stageid)
        {
            debugger
                   var taskid=document.getElementById("<%=hdntaskstageid.ClientID%>").value;
                    //alert(taskid);
                    //alert(Stageid);
                    $.ajax({
                    url: 'AddSubtask.aspx/Update_Stagge',
                    data: "{ 'taskid': '" + taskid + "' ,'Stageid': '" + Stageid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStageupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStageupdt(response) {
            alert('Data Saved successfully.');
           // Bindtask_data();
            var btnstgid = document.getElementById("<%=hdnbtn_stageid.ClientID%>").value; 
            var arrstgcol = response.d.split("~~");
            document.getElementById(btnstgid).innerHTML = arrstgcol[0];
            document.getElementById(btnstgid).style.backgroundColor = arrstgcol[1];
        }
        
        function setStageid(taskid,btnStageid)
        {
            debugger
             document.getElementById("<%=hdnbtn_stageid.ClientID%>").value = btnStageid;
            document.getElementById("<%=hdntaskstageid.ClientID%>").value = taskid;
        }
            </script>

<%--All stage update end--%>


<%--Dependancy details bind and update data start --%>

     <script type="text/javascript">
             
        function Svaedependancy(val12A) {

            if (Page_ClientValidate("btnsavedependancy")) {
               

                //if (confirm('Are You Sure You want to Submit This Entry!'))
                //{
                    

                //}
                //else
                //{
                //    return false;
                //}
                debugger


                // save all details
                
                  var Subtaskid = document.getElementById('<%=hdnchngdependencyid.ClientID%>').value; 
                  var dependencyid = document.getElementById('<%=hdndependancy.ClientID%>').value;
        
                $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/UpdateSubTask_Dependency',
                    data: "{ 'Subtaskid': '" + Subtaskid + "' ,'dependencyid': '" + dependencyid + "'}",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessDependancy,
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
                function OnSuccessDependancy(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=ddldependancy]").val("");
                   
                     document.getElementById('<%=hdndependancy.ClientID%>').value = "";
                     $(document.getElementById("<%=ddldependancy.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddldependancy.ClientID%>")).change();
                    document.getElementById("btnclsdependancy").click();
                    BindSubtask_data();
                    document.getElementById("lblDependancy_" + document.getElementById("<%=hdnchngdependcyhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Dependency(RecordId,lbldependencyid) {
              
              debugger;
              document.getElementById("<%=hdnchngdependcyhtml.ClientID%>").value = lbldependencyid.split('_')[1];
             
                 
                $.ajax({
                    url: 'AddSubtask.aspx/Bindsubtask_Dependancy',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1dependency,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1dependency(response) {
            
            debugger
            try {
                var taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnchngdependencyid.ClientID%>').value = taskdetails.find("id").text();
                   document.getElementById('<%=hdndependancy.ClientID%>').value = taskdetails.find("dependancy").text();

               if (taskdetails.find("dependancy").text() != "")
             {
               var idsemp = taskdetails.find("dependancy").text().split(',');
               $(document.getElementById("<%=ddldependancy.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=ddldependancy.ClientID%>")).change();
              }

                else {

                     document.getElementById('<%=hdndependancy.ClientID%>').value = "";
                     $(document.getElementById("<%=ddldependancy.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddldependancy.ClientID%>")).change();

                    }

             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    
      <%--Dependancy details bind and update data end  --%>


<%-- add timer notes start--%>

<script type="text/javascript">
    
        function SvaeDataTimer(val15A) {

            if (Page_ClientValidate("btnsaveTimer")) {
               
                debugger
                
                // save all details
                var ctrlid = document.getElementById('<%=hdnsubtask_id.ClientID%>').value;
                
                    varsubtaskid = ctrlid;
                   //document.getElementById('hour_' + vartaskid).innerHTML = "00";
                   // document.getElementById('minute_' + vartaskid).innerHTML = "00";
                   // document.getElementById('second_' + vartaskid).innerHTML = "00";
                   // document.getElementById('millisecond_' + vartaskid).innerHTML="000";
                 // alert(document.getElementById('hour_' + vartaskid).innerHTML);
                
                
                $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/SaveTimer',
                    data: "{'ProjectId': '" + document.getElementById('<%=hdnprojec_tid.ClientID%>').value + "','Milestoneid': '" + document.getElementById('<%=hdnmilestone_id.ClientID%>').value + "' ,'tasklistid': '" + document.getElementById('<%=hdntasklist_id.ClientID%>').value + "','taskid': '" + document.getElementById('<%=hdntask_id.ClientID%>').value + "','subtaskid': '" + document.getElementById('<%=hdnsubtask_id.ClientID%>').value + "','TimerNotes': '" + document.getElementById('<%=TimerNotes.ClientID%>').value + "','hour': '" + document.getElementById('hour_' + varsubtaskid).innerHTML + "','minute': '" + document.getElementById('minute_' + varsubtaskid).innerHTML + "','second': '" + document.getElementById('second_' + varsubtaskid).innerHTML + "','millisecond': '" + document.getElementById('millisecond_' + varsubtaskid).innerHTML + "','Transactionid': '" + document.getElementById('<%=hdntransaction_id.ClientID%>').value + "'}",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTimer,
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
                function OnSuccessTimer(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    var ctrlid = document.getElementById('<%=hdnsubtask_id.ClientID%>').value;
                   // vartaskid = ctrlid.id.split('_')[1];
                    stop(document.getElementById('<%=hdnstoptimer_id.ClientID%>').value);
                    document.getElementById('divs_' + ctrlid).style.display = '';
                    document.getElementById('dinv_' + ctrlid).style.display = 'none';

                    document.getElementById('hour_' + ctrlid).innerHTML = "00";
                    document.getElementById('minute_' + ctrlid).innerHTML = "00";
                    document.getElementById('second_' + ctrlid).innerHTML = "00";
                    document.getElementById('millisecond_' + ctrlid).innerHTML="000";

                     document.getElementById('<%=TimerNotes.ClientID%>').value = "";
                     document.getElementById("btnclsTimer").click();
                    BindSubtask_data();
                    
        
                    
                }

        

          function Binddetails_Timer(ProjectId,Milestoneid,tasklistid,taskid,subtask,hours,minutes,seconds,milliseconds,transactionid,varid) {
              
              debugger;

                    document.getElementById('<%=hdnprojec_tid.ClientID%>').value = ProjectId;
                    document.getElementById('<%=hdnmilestone_id.ClientID%>').value = Milestoneid;
                    document.getElementById('<%=hdntasklist_id.ClientID%>').value = tasklistid;
              document.getElementById('<%=hdntask_id.ClientID%>').value = taskid;
                document.getElementById('<%=hdnsubtask_id.ClientID%>').value = subtask;

              document.getElementById('<%=hdnstoptimer_id.ClientID%>').value = varid;
              document.getElementById('<%=hdntransaction_id.ClientID%>').value = transactionid;


               document.getElementById("spnhr").innerHTML = document.getElementById(hours.id).innerHTML;
               document.getElementById("spnmin").innerHTML = document.getElementById(minutes.id).innerHTML;
               document.getElementById("spnsec").innerHTML = document.getElementById(seconds.id).innerHTML;
               document.getElementById("spnmil").innerHTML = document.getElementById(milliseconds.id).innerHTML;
               

               document.getElementById("<%=hdnhourcount.ClientID%>").value = document.getElementById("spnhr").innerHTML;
                document.getElementById("<%=hdnminutecount.ClientID%>").value = document.getElementById("spnmin").innerHTML;
                document.getElementById("<%=hdnsecondcount.ClientID%>").value = document.getElementById("spnsec").innerHTML;
                document.getElementById("<%=hdnmilisecondcount.ClientID%>").value = document.getElementById("spnmil").innerHTML;
                //  alert(document.getElementById("<%=hdnminutecount.ClientID%>").innerHTML);
           

        }


         function OnSuccess1Timer(response) {
            
            }


        
        


</script>


<%-- add timer notes end--%>



<%-- timer start --%>
<script>
   
        function hideshow(div1,div2)
        {
            debugger
            //alert(div1.id);
            //alert(div2.id);
            document.getElementById(div1.id).style.display = "none";
            document.getElementById(div2.id).style.display = "block";
       }

        function hideshowplaybutton(btnpause,btnresum)
        {
            debugger
            //alert(btnpause.id);
            document.getElementById(btnpause.id).style.display = "none";
            document.getElementById(btnresum.id).style.display = "inline-block";
        }
        function hideshowPausebutton(btnpause,btnresum)
        {
            debugger
            //alert(btnpause.id);
            document.getElementById(btnpause.id).style.display = "inline-block";
            document.getElementById(btnresum.id).style.display = "none";
    }

       

   
    

</script>



<asp:HiddenField ID="hdnprojectsubtaskid" runat="server" Value="0" />
<script>
    
    "use strict";

let hour = 0;
let minute = 0;
let second = 0;
let millisecond = 0;

let cron;

//document.form1.start.onclick = () => start();
//document.form1.pause.onclick = () => pause();
//document.form1.stop.onclick = () => stop();
//document.form1.reset.onclick = () => reset();

    function start(hours, minutes, seconds, milliseconds,valid) {
        debugger;
      
        hour = parseInt(document.getElementById(hours.id).innerHTML);
        minute = parseInt(document.getElementById(minutes.id).innerHTML);
        second = parseInt(document.getElementById(seconds.id).innerHTML);
        millisecond = parseInt(document.getElementById(milliseconds.id).innerHTML);
        //var strid=hours.id.
  pause();
        cron = setInterval(() => { timer(hours.id, minutes.id, seconds.id, milliseconds.id); }, 10);

  //      hour = 4;
  //minute = 35;
  //second = 22;
  //millisecond = 100;
}
    function savestartdetails(projectid,milstoneid,tasklistid,taskid,Subtaskid,chkdepandancy)
    {
          debugger
        if (chkdepandancy != "") {
            alert("There is already a dependency on this Subtask " + chkdepandancy);
            return false;
        }

        else {

            document.getElementById("<%=hdnprojectsubtaskid.ClientID%>").value = Subtaskid;
            //alert(Subtaskid);
            $.ajax({
                type: "POST",
                url: 'AddSubtask.aspx/SaveTimerstartdetails',
                data: "{'ProjectId': '" + projectid + "','Milestoneid': '" + milstoneid + "' ,'tasklistid': '" + tasklistid + "','taskid': '" + taskid + "','Subtaskid': '" + Subtaskid + "'}",
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: OnSuccessTimerdetails,
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

        }

    }
    function OnSuccessTimerdetails(response)
    {
        debugger
       var cid= document.getElementById("hdntrans_" + document.getElementById("<%=hdnprojectsubtaskid.ClientID%>").value)
        cid.value = response.d;
    }

     
        function SvaePauseDetialss(val12A) {

            if (Page_ClientValidate("btnpausesaveNotes")) {

               
                debugger


                var ProjectId = document.getElementById('<%=hdnprojecnots_tid.ClientID%>').value;
                var Milestoneid = document.getElementById('<%=hdnmilestonenotes_id.ClientID%>').value;
                var tasklistid = document.getElementById('<%=hdntasklistnotes_id.ClientID%>').value;
                var taskid = document.getElementById('<%=hdntasknotes_id.ClientID%>').value;
                var subtaskid = document.getElementById('<%=hdnsubtasidknotes_id.ClientID%>').value;

                var varid = document.getElementById('<%=hdnpause_id.ClientID%>').value;
                var transactionid = document.getElementById('<%=hdnnotestransaction_id.ClientID%>').value;
                  
            var cid = document.getElementById("hdntrans_" + subtaskid)
            //cid.value = response.d;
            if (transactionid == "" || transactionid == "0") {
                transactionid = cid.value;
            }

            //alert(cid.value);
            //return false;
                
                
                $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/SaveTimerpausedetails',
                    data: "{'ProjectId': '" + ProjectId + "','Milestoneid': '" + Milestoneid + "' ,'tasklistid': '" + tasklistid + "','taskid': '" + taskid + "','subtaskid': '" + subtaskid + "','transactionid': '" + transactionid + "','Notes': '" + document.getElementById('<%=Notes.ClientID%>').value + "'}",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessPauseNotes,
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
                function OnSuccessPauseNotes(response) {
                    debugger
                   // alert(response.d);
                    alert("Data Saved successfully."); 
                    pause(document.getElementById('<%=hdnpause_id.ClientID%>').value);

                    var ctrlid = document.getElementById('<%=hdnsubtasidknotes_id.ClientID%>').value;
                    document.getElementById('btnpause_' + ctrlid).style.display ="none";
                    document.getElementById('btnresum_' + ctrlid).style.display = "inline-block";

                    document.getElementById('<%=Notes.ClientID%>').value = "";
                    document.getElementById("btnclsNotes").click();
                    debugger
                    
        
                    
                }

        

          function BindPausedetails_Notes(ProjectId,Milestoneid,tasklistid,taskid,subtaskid,transactionid,varid) {
              
              debugger;

                    document.getElementById('<%=hdnprojecnots_tid.ClientID%>').value = ProjectId;
                    document.getElementById('<%=hdnmilestonenotes_id.ClientID%>').value = Milestoneid;
                    document.getElementById('<%=hdntasklistnotes_id.ClientID%>').value = tasklistid;
                   document.getElementById('<%=hdntasknotes_id.ClientID%>').value = taskid;
                   document.getElementById('<%=hdnsubtasidknotes_id.ClientID%>').value = subtaskid;

                   document.getElementById('<%=hdnpause_id.ClientID%>').value = varid;
                   document.getElementById('<%=hdnnotestransaction_id.ClientID%>').value = transactionid;
                   document.getElementById('<%=Notes.ClientID%>').value = "";
               

        }


    function OnSuccess1Notes(response)
    {
            
    }


    

 function saveResumeDetails(projectid, milstoneid, tasklistid,taskid,subtaskid,transactionid)
 {
     debugger
         var cid= document.getElementById("hdntrans_" + subtaskid)
         //cid.value = response.d;
         if (transactionid == "" || transactionid == "0") {
             transactionid = cid.value;
         }
         //alert(cid.value);
         //return false;
         $.ajax({
                    type: "POST",
                    url: 'AddSubtask.aspx/SaveTimerresumedetails',
                    data: "{'ProjectId': '" + projectid + "','Milestoneid': '" + milstoneid + "' ,'tasklistid': '" + tasklistid + "','taskid': '" + taskid + "','subtaskid': '" + subtaskid + "','transactionid': '" + transactionid + "'}",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessresumedeatils,
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

    }
    function OnSuccessresumedeatils(response)
    {
        
    }

function pause(varcornid) {
    //clearInterval(cron);
    //var
    debugger
    clearInterval(varcornid);
}
function stop(varcornid) {
  //clearInterval(cron);
    clearInterval(varcornid);
}

function reset(hours,minutes,seconds,milliseconds) {
  hour = 0;
  minute = 0;
  second = 0;
    millisecond = 0;

    document.getElementById(hours).innerText = '00';
   document.getElementById(minutes).innerText = '00';
   document.getElementById(seconds).innerText = '00';
    document.getElementById(milliseconds).innerText ='000';

 
}

function timer(hours,minutes,seconds,milliseconds) {
  if ((millisecond += 10) == 1000) {
    millisecond = 0;
    second++;
  }
  if (second == 60) {
    second = 0;
    minute++;
  }
  if (minute == 60) {
    minute = 0;
    hour++;
  }
  document.getElementById(hours).innerText = returnData(hour);
  document.getElementById(minutes).innerText = returnData(minute);
  document.getElementById(seconds).innerText = returnData(second);
  document.getElementById(milliseconds).innerText = returnData(millisecond);
    }
 <%=setstartf%>
<%=settimers%>
    //timer_1('00', '00', '00', '000');
    //document.form1.start.click();
    //function autoplay_1() {
    //    document.getElementById('btnauto_1').click()
    //}
    //autoplay_1();
function returnData(input) {
  return input >= 10 ? input : `0${input}`
    }
    //$(document).ready(function () {
    //    debugger
    //    timer_1('00', '00', '00', '000');
    //}
</script>

<script type="text/javascript" language="javascript">
        function isnumeric(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Please enter only Number value');
                return false;
            }
            else {
                //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
                var parts = event.srcElement.value.split('.');
                if (parts.length > 1 && charCode == 46) {
                    return false;
                }
                return true;
            }
        }
    </script>


     <%--Multi dropdown select start --%> 

 <%--<script>
    debugger
    var ddLOwner = $(document.getElementById("<%=owner.ClientID%>"));
     $(ddLOwner).select2({
        data: [
    <%=MultiOwner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        width: "100%"
    });

   
     $(document.getElementById("<%=owner.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
        document.getElementById("<%=hdnowner.ClientID%>").click();

    });
   
  
    </script>


    <script>
    debugger
    var ddLteam = $(document.getElementById("<%=team.ClientID%>"));
   

    $(ddLteam).select2({
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
   
  
    </script>--%>

<script>
    debugger
    var ddLOwner = $(document.getElementById("<%=Notifyusers.ClientID%>"));
   

    $(ddLOwner).select2({
        data: [
    <%=Multiusers%>
        ],
        multiple: true,
        placeholder: "Search Notify users",
        width: "100%"
    });

   
     $(document.getElementById("<%=Notifyusers.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=Notifyusers.ClientID%>")).val();
        document.getElementById("<%=hdnNotifyusers.ClientID %>").value = ids;
        document.getElementById("<%=Notifyusers.ClientID%>").click();

    });
   
  
    </script>


 <asp:HiddenField ID="multichngtaskid" runat="server" Value="0" />
<script>
   <%-- debugger
    var ddLchangeowner = $(document.getElementById("<%=ddlchangeowner.ClientID%>"));
   

    $(ddLchangeowner).select2({
        data: [
    <%=Multichangeowner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        dropdownParent: $("#changeowner"),
        width: "100%"
    });

   
     $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val();
        document.getElementById("<%=hdncahngeowner.ClientID %>").value = ids;
        document.getElementById("<%=ddlchangeowner.ClientID%>").click();

    });--%>



     function Multichangeowner() {
        debugger
        var tsk_id = document.getElementById("<%=multichngtaskid.ClientID%>").value;
        
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiChangeOwner_new',
            data: "{'tsk_id': '" + tsk_id + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLchangeowner = $(document.getElementById("<%=ddlchangeowner.ClientID%>"));
                                                         $(ddLchangeowner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             dropdownParent: $("#changeowner"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val();
                                                        document.getElementById("<%=hdncahngeowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=ddlchangeowner.ClientID%>").click();

                                                         });

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


<script>
   <%-- debugger
    var ddLAddteam = $(document.getElementById("<%=ddladdteam.ClientID%>"));
   

    $(ddLAddteam).select2({
        data: [
    <%=Multiaddteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        dropdownParent: $("#addteam"),
        width: "100%"
    });

   
     $(document.getElementById("<%=ddladdteam.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=ddladdteam.ClientID%>")).val();
        document.getElementById("<%=hdnaddteam.ClientID %>").value = ids;
        document.getElementById("<%=ddladdteam.ClientID%>").click();

    });--%>



     function MultichangeTeam() {
        debugger
        var tsk_id = document.getElementById("<%=multichngtaskid.ClientID%>").value;
        
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiChangeTeam_new',
            data: "{'tsk_id': '" + tsk_id + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLAddteam = $(document.getElementById("<%=ddladdteam.ClientID%>"));
                                                         $(ddLAddteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             dropdownParent: $("#addteam"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=ddladdteam.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=ddladdteam.ClientID%>")).val();
                                                        document.getElementById("<%=hdnaddteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=ddladdteam.ClientID%>").click();

                                                         });

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


<script>
    debugger
    var ddLdependancy = $(document.getElementById("<%=ddldependancy.ClientID%>"));
   

    $(ddLdependancy).select2({
        data: [
    <%=MultiDependancy%>
        ],
        multiple: true,
        placeholder: "Search Dependancy",
        dropdownParent: $("#adddependency"),
        width: "100%"
    });

   
     $(document.getElementById("<%=ddldependancy.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=ddldependancy.ClientID%>")).val();
        document.getElementById("<%=hdndependancy.ClientID %>").value = ids;
        document.getElementById("<%=ddldependancy.ClientID%>").click();

    });
   
  
    </script>

    <%--Multi dropdown select start end--%>



<%-- show column hide show start--%>

    <asp:HiddenField ID="showidcolumnid" runat="server" Value="0" />
     
    <script type="text/javascript"> 
        document.querySelectorAll('a.toggle-vis').forEach((el) =>
        {      debugger
            el.addEventListener('click', function (e) {
                    debugger
                    e.preventDefault();

                    let columnIdx = e.target.getAttribute('data-column');
                    let column = table.column(columnIdx);

                    // Toggle the visibility
                    column.visible(!column.visible());

                    var showhide_column = {};
                    //alert(column.visible());
                    showhide_column.id = document.getElementById('<%=showidcolumnid.ClientID%>').value;
                    showhide_column.columnnameid = columnIdx;

                    if (!column.visible()) {
                        showhide_column.isshow = "0";
                      
                    }
                    else {
                        showhide_column.isshow = "1";
                        
                    }
                    
                
                    $.ajax({
                        type: "POST",
                        url: 'AddSubtask.aspx/Savecolumndata',
                        data: '{columndetails: ' + JSON.stringify(showhide_column) + '}',
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        //success: OnSuccess,
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
                    
                });
            });
         

    </script>



<script type="text/javascript">
        function hidecolumn() {
            $('.dropdown-item.toggle-vis').click(function (e) {
                if ($(this).hasClass("text-mute")) {
                    $(this).removeClass("text-mute");
                }
                else {
                    $(this).addClass("text-mute");
                }
            });
        }
    </script>

   
      <asp:HiddenField ID="hdn0" runat="server" />
    <asp:HiddenField ID="hdn1" runat="server" />
     <asp:HiddenField ID="hdn2" runat="server" />
     <asp:HiddenField ID="hdn3" runat="server" />
     <asp:HiddenField ID="hdn4" runat="server" />
     <asp:HiddenField ID="hdn5" runat="server" />
     <asp:HiddenField ID="hdn6" runat="server" />
     <asp:HiddenField ID="hdn7" runat="server" />
     <asp:HiddenField ID="hdn8" runat="server" />
     <asp:HiddenField ID="hdn9" runat="server" />
     <asp:HiddenField ID="hdn10" runat="server" />
     <asp:HiddenField ID="hdn11" runat="server" />
    <asp:HiddenField ID="hdn12" runat="server" />
    <asp:HiddenField ID="hdn13" runat="server" />
    <asp:HiddenField ID="hdn14" runat="server" />
    <asp:HiddenField ID="hdn15" runat="server" />
    <asp:HiddenField ID="hdn16" runat="server" />
    <asp:HiddenField ID="hdn17" runat="server" />
    <asp:HiddenField ID="hdn18" runat="server" />
  <asp:HiddenField ID="hdn19" runat="server" />
 <asp:HiddenField ID="hdn20" runat="server" />
 <asp:HiddenField ID="hdn21" runat="server" />
 <asp:HiddenField ID="hdn22" runat="server" />
 <asp:HiddenField ID="hdn23" runat="server" />
 <asp:HiddenField ID="hdn24" runat="server" />
 <asp:HiddenField ID="hdn25" runat="server" />
 <asp:HiddenField ID="hdn26" runat="server" />


    

 <%-- show column hide show end--%>

<div id="dependencyD" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Dependency Details</h5>
                <button type="button" class="btn-close" id="btnclsD" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">

                            <asp:Label ID="lbldependency" runat="server" Text=""></asp:Label>
                            


                        </div>                        
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</div>
<script>
function Binddetails_D(stid,ctrlid)
{
 debugger
                  $.ajax({
                    url: '/pms/AddSubtask.aspx/Binddependencydata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'stid': '" +stid+ "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetailsD,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
}
function OnSuccessgetdetailsD(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbldependency.ClientID%>");
                    lbl.innerHTML = '';                    
            lbl.innerHTML = todolist;
      
                

       

        }
</script>
<script>
        

      function UploadFiles(id)
      {
        debugger
        if (document.getElementById("<%=fileupload.ClientID%>").files.length >0)
        {       
            
            let fileUpload = document.getElementById("<%=fileupload.ClientID%>");
            let files = fileUpload.files;
            
  
            let data = new FormData();  
            for (let i = 0; i < files.length; i++) {
               
                data.append(files[i].name, files[i]);  
                
            }
           
            $.ajax({  
                url: "FileUploadHandlersubtask.ashx?id="+id+"", 
                type: "POST",  
                data: data,  
                contentType: false,  
                processData: false, 
                async: false,
                success:OnSuccessFiles,
               
                error: function (err) {  
                    alert(err.statusText)  
                }  
            });  
  
        }
        };
        function OnSuccessFiles(response) {
            document.getElementById("<%=fileupload.ClientID%>").value = null;
         debugger
        }
function ShowPreviewnew() {
    debugger
    var input = document.getElementById("<%= fileupload.ClientID %>");
    var previewContainer = document.getElementById("previewContainer");
    previewContainer.innerHTML = ''; // Clear previous previews

    if (input.files && input.files.length > 0) {
        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];

            // Check file type
            var isImage = file.type.startsWith('image/');
            var isPDF = file.type === 'application/pdf';

            if (!isImage && !isPDF) {
                alert("Only image or PDF files are allowed.");
                continue;
            }

            // Size checks
            if (isImage && file.size > 25000) {
                alert("Image '" + file.name + "' exceeds 25KB.");
                continue;
            }

            if (isPDF && file.size > 1048576) {
                alert("PDF '" + file.name + "' exceeds 1MB.");
                continue;
            }

            var reader = new FileReader();
            reader.onload = (function (theFile) {
                return function (e) {
                    if (theFile.type.startsWith('image/')) {
                        var img = document.createElement("img");
                        img.src = e.target.result;
                        img.style.maxWidth = "100px";
                        img.style.margin = "5px";
                        img.title = theFile.name;
img.className = "preview-item";
                        previewContainer.appendChild(img);
                    } else if (theFile.type === 'application/pdf') {
                        // === OPTION 1: Show Emoji ===
                        var icon = document.createElement("div");
                        icon.textContent = "📄"; // PDF emoji
                        icon.style.fontSize = "40px";
                        icon.style.margin = "5px";
                        icon.title = theFile.name;
icon.className = "preview-item";
                        previewContainer.appendChild(icon);

                        // === OPTION 2: Show Image Icon ===
                        /*
                        var img = document.createElement("img");
                        img.src = "/images/pdf-icon.png"; // Adjust path as needed
                        img.style.width = "40px";
                        img.style.margin = "5px";
                        img.title = theFile.name;
                        previewContainer.appendChild(img);
                        */
                    }
                };
            })(file);

            reader.readAsDataURL(file);
        }
    }
}
function bindUploadedDrawings(strfiles) {
    var uploadData =strfiles; 
    var basePath = "/pms/SubtaskImages/";
    var previewContainer = document.getElementById("previewContainer");
    
    previewContainer.innerHTML = ""; // Clear previous content

    if (uploadData && uploadData !== "0") {
        var files = uploadData.split("^");

        files.forEach(function(file) {
            var filePath = basePath + file.trim();
            var fileName = file.split("/").pop();
            var ext = fileName.split('.').pop().toLowerCase();

            if (["jpg", "jpeg", "png", "gif", "bmp"].includes(ext)) {
                // It's an image
                var imgLink = document.createElement("a");
                imgLink.href = filePath;
                imgLink.target = "_blank";
                
                var img = document.createElement("img");
                img.src = filePath;
                img.style.maxWidth = "100px";
                img.style.margin = "5px";
                img.title = fileName;
                img.className = "preview-item";
                imgLink.appendChild(img);
                previewContainer.appendChild(imgLink);
            } else if (ext === "pdf") {
                // It's a PDF
                var pdfLink = document.createElement("a");
                pdfLink.href = filePath;
                pdfLink.target = "_blank";
                pdfLink.title = fileName;
                pdfLink.style.display = "inline-block";
                pdfLink.style.margin = "5px";
                pdfLink.style.textDecoration = "none";
                pdfLink.style.fontSize = "40px";

                // Option 1: Emoji icon
                pdfLink.textContent = "📄";
pdfLink.className = "preview-item";
                previewContainer.appendChild(pdfLink);
            }
        });
    }
}
        function ShowPreview()
        {
           debugger
            var input = document.getElementById("<%=fileupload.ClientID%>");
            if (input.files && input.files[0]) {
                if (input.files.item(0).size < 30000) {
                    var ImageDir = new FileReader();
                    ImageDir.onload = function (e) {
                        //Jquery
                        //$('#impPrev').attr('src', e.target.result);  
                        var image = document.getElementById("empimg");
                        image.src = e.target.result;
                    }
                    ImageDir.readAsDataURL(input.files[0]);
                }
                else {
                    alert("image size should be less then 25 kb");
                   input.value = '';
                    return;
                }
            }  
        }  
</script>