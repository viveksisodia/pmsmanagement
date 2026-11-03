<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultipleDataSubtask.ascx.cs" Inherits="ucontrol_MultipleDataSubtask" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<%--    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
<div class="card border-0">--%>
    <div class="card-header">
        <div class="row">
            <%--<div class="col-sm-auto">
                <div class="inner-head">
                    <div class="opp-ico purpel">
                        <i class="ri-list-check-2"></i>
                    </div>
                    <div class="head-txt">
                        <p><%=fns.getltext("Sub Task", "2")%></p>                                                    
                        <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside">
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
            </div>--%>
            <div class="col-sm mt-1">
                <div class="d-flex justify-content-sm-end">
                <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                    <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addtask"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                    <%--<button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
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
                    </div>--%>
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
<%--</div>
            </div>
      </div>
    </div>--%>
    
<div class="offcanvas offcanvas-end" id="addtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
             <label id="lbltitle" runat="server"><%=fns.getltext("Add Sub Task", "2")%></label>  

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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="Milestoneid_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>

                                 <div class="col-xxl-12 col-lg-12" id="divptasklist" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task List", "2")%></label>
                                    </div>
                               
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="task" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task ..." AllowSingleDeselect="true" AutoPostBack="true" onchange="GetWeightageDtls()" OnSelectedIndexChanged="task_SelectedIndexChanged">
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
                               
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task Description", "2")%></label>
                                    </div>
                                    <textarea id="SubTaskDescription" runat="server" placeholder="Sub Task Description" class="form-control"></textarea>
                                </div>


                                  <div class="col-xxl-12 col-lg-12" id="divWeightage">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" class="form-control">
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Startdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Startdate" runat="server" class="form-control">

                                               <asp:HiddenField ID="hdnStartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="StartTime" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="StartTime" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Targetdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Targetdate" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnTargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                 <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Timeline" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="Timeline" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                    <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour" class="form-control">
                                </div>

                               

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="P_Status" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budget" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actual" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divreminder">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" 
                                                ControlToValidate="Reminder" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                
                                     <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="ChooseDate" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnChooseDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                    <div class="col-xxl-12 col-lg-12" id="Divtime">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Time", "2")%></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="Time" runat="server" class="form-control">
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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
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
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Extendatestrt_subtask" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnextndstrt_subtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="Extendateend_subtask" runat="server" class="form-control">
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
<script>
    const table = new DataTable('.datatable', {  
    scrollX:  true,
    scrollCollapse: true,
    paging:         true,
    searching: true,
    //columnDefs : [
    //        { 'bSortable': false, 'targets': [ 1, 17 ] } 
    //    ]
    });
</script>


 <asp:HiddenField ID="hdnsearchstaus" runat="server" />
                            <script type="text/javascript">
                                function GetSubTask(statusid)
                                {
                                    debugger
                                    document.getElementById('<%=hdnsearchstaus.ClientID%>').value = statusid;
                                if (statusid == "0")
                                {
                                   BindSubtask_data_dropsearchstatus();
                                }
                                else if (statusid == "1") 
                                {
                                   
                                    BindSubtask_data_dropsearchstatus();

                                }
                               else if (statusid == "2")
                                {
                                   
                                    BindSubtask_data_dropsearchstatus();

                                }
                                else if (statusid == "3")
                                {
                                   
                                    BindSubtask_data_dropsearchstatus();

                               }
                                else
                                {
                                   
                                    BindSubtask_data_dropsearchstatus();

                               }
                             }
                            </script>

<asp:HiddenField ID="hdntype" runat="server" />
<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
            document.getElementById("divproject").style.display = "none";
            document.getElementById("divmilestone").style.display = "none";
            document.getElementById("divptasklist").style.display = "none";
             document.getElementById("divtask").style.display = "none";

      }
    

</script>


<script>
   
    function GetWeightageDtls() {
       // alert(document.getElementById('<%=hdnweightage.ClientID%>').value);
        if (document.getElementById('<%=hdnweightage.ClientID%>').value == 0)
        {
            document.getElementById("divWeightage").style.display = "none";

        }
        if (document.getElementById('<%=hdnweightage.ClientID%>').value == 1) {
            document.getElementById("divWeightage").style.display = "block";

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


<asp:HiddenField ID="hdnsavetype" runat="server" />
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
                            url: "MultiProjectSubtask.aspx/statussave",
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
                            url: "MultiProjectSubtask.aspx/AddPrioritytype",
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
                            url: "MultiProjectSubtask.aspx/AddStage",
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
                        subtaskdetails.Milestoneid = document.getElementById('<%=Milestoneid.ClientID%>').value; 
                        subtaskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        subtaskdetails.task = document.getElementById('<%=task.ClientID%>').value; 
                        subtaskdetails.Subtaskname = document.getElementById('<%=Subtaskname.ClientID%>').value; 
                        subtaskdetails.SubTaskDescription = document.getElementById('<%=SubTaskDescription.ClientID%>').value; 
                        subtaskdetails.owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        subtaskdetails.Startdate = document.getElementById('<%=Startdate.ClientID%>').value; 
                       subtaskdetails.Targetdate = document.getElementById('<%=Targetdate.ClientID%>').value; 
                  

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
                        
                        $.ajax({
                            type: "POST",
                            url: 'MultiProjectSubtask.aspx/SaveSubTask',
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
                    alert("Data Saved successfully."); 
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


                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
                    document.getElementById('<%=hdntask.ClientID%>').value = "0";
                    document.getElementById('<%=Subtaskname.ClientID%>').value = "";
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                    $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=Startdate.ClientID%>').value ="";
                    document.getElementById('<%=Targetdate.ClientID%>').value = "";
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
                    $("[id*=Milestoneid]").val("0");
                    $("[id*=tasklist]").val("0");
                    $("[id*=task]").val("0");
                    $("[id*=Subtaskname]").val("");
                    $("[id*=SubTaskDescription]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                    $("[id*=Targetdate]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("0");

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
              document.getElementById('<%=hdntask.ClientID%>').value = "0";

                      document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnStartdate.ClientID%>').value ="";
               document.getElementById('<%=hdnTargetdate.ClientID%>').value = "";
             

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
           

                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Sub Task";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'MultiProjectSubtask.aspx/Bindsubtaskdetail',
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
                    
                   // $("[id*=ProjectId]").val(Bindsubtaskdetail.find("ProjectId").text());
                   // $("[id*=Milestoneid]").val(Bindsubtaskdetail.find("Milestoneid").text());
                   // $("[id*=tasklist]").val(Bindsubtaskdetail.find("tasklist").text());
                   // $("[id*=task]").val(Bindsubtaskdetail.find("task").text());
                   // $("[id*=Subtaskname]").val(Bindsubtaskdetail.find("Subtaskname").text());
                   // $("[id*=SubTaskDescription]").val(Bindsubtaskdetail.find("SubTaskDescription").text());
                   // $("[id*=owner]").val(Bindsubtaskdetail.find("owner").text());
                   // $("[id*=Startdate]").val(Bindsubtaskdetail.find("Startdate").text());
                   // $("[id*=Targetdate]").val(Bindsubtaskdetail.find("Targetdate").text()); 
                   // $("[id*=P_Status]").val(Bindsubtaskdetail.find("P_Status").text()); 
                   // $("[id*=Priority]").val(Bindsubtaskdetail.find("Priority").text()); 
                   // $("[id*=Budget]").val(Bindsubtaskdetail.find("Budget").text()); 
                   // $("[id*=Actual]").val(Bindsubtaskdetail.find("Actual").text()); 
                   // $("[id*=Reminder]").val(Bindsubtaskdetail.find("Reminder").text()); 
                   // $("[id*=Occurrence]").val(Bindsubtaskdetail.find("Occurrence").text()); 
                   // $("[id*=tagto]").val(Bindsubtaskdetail.find("tagto").text()); 
                   // $("[id*=team]").val(Bindsubtaskdetail.find("team").text()); 
                   // $("[id*=ReminderType]").val(Bindsubtaskdetail.find("ReminderType").text()); 
                   // $("[id*=Days]").val(Bindsubtaskdetail.find("Days").text()); 
                   //// $("[id*=Time]").val(Bindsubtaskdetail.find("team").text()); 
                   // $("[id*=Notifyusers]").val(Bindsubtaskdetail.find("Notifyusers").text()); 
                   // $("[id*=ChooseDate]").val(Bindsubtaskdetail.find("ChooseDate").text()); 



                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindsubtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Bindsubtaskdetail.find("Milestoneid").text();
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
                   
                    $("[id*=Notifyusers]").val(Bindsubtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDate]").val(Bindsubtaskdetail.find("ChooseDate").text()); 
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindsubtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnStartdate.ClientID%>').value = Bindsubtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdnTargetdate.ClientID%>').value = Bindsubtaskdetail.find("Targetdate").text();
                    
                    document.getElementById('<%=hdnstarttime.ClientID%>').value = Bindsubtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = Bindsubtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDuration.ClientID%>').value = Bindsubtaskdetail.find("TaskDuration").text();


                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindsubtaskdetail.find("team").text();

                    document.getElementById('<%=hdnstage.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
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

                    GetReminderDtls();
                    GetRemindertypeDtls();
                    // time and reminder fill and edit end//


             if (Bindsubtaskdetail.find("owner").text() != "")
             {
             var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
            $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=owner.ClientID%>")).change();
            }



              if (Bindsubtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindsubtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }

              if (Bindsubtaskdetail.find("team").text() != "")
               {
             var idsemp = Bindsubtaskdetail.find("team").text().split(',');
            $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=team.ClientID%>")).change();
            }


                     document.getElementById('<%=Button1.ClientID%>').click();
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
            BindSubtask_data();
});
         function BindSubtask_data() {
            
             debugger;
               //  alert($("[id*=hdiprojid]").val());
               //alert($("[id*=hdnmilstoneid]").val());
               //alert($("[id*=hdntasklistid]").val());
               //alert($("[id*=hdntaskid]").val());

                  $.ajax({
                    url: 'MultiProjectSubtask.aspx/bindSubtaskdata',                  
                      //data: '{}',
                     data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                           //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "' ,'milstoneid': '" + $("[id*=hdnmilstoneid]").val() + "' ,'tasklistid': '" + $("[id*=hdntasklistid]").val() + "' ,'taskid': '" + $("[id*=hdntaskid]").val() + "' }",

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

                });
            }

             catch
            { }
            //alert("hi");
            autoplay(); 

             
           
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
                    url: 'MultiProjectSubtask.aspx/Deletesubtaskdata',
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
                    url: 'MultiProjectSubtask.aspx/Save_Owner',
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
                    url: 'MultiProjectSubtask.aspx/Binddetail_Owner',
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

               if (Bindsubtaskdetail.find("owner").text() != "")
             {
               var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
               $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();
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
                    url: 'MultiProjectSubtask.aspx/Save_Team',
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
                    url: 'MultiProjectSubtask.aspx/Bindetail_Team',
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

               if (Bindsubtaskdetail.find("team").text() != "")
             {
               var idsemp = Bindsubtaskdetail.find("team").text().split(',');
               $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=ddladdteam.ClientID%>")).change();
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
             
        function UpdateDataStatusdeatils(statusid)
        {
            debugger
                   var subtaskid=document.getElementById("<%=hdsubtaskid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectSubtask.aspx/Update_Status',
                    data: "{ 'subtaskid': '" + subtaskid + "' ,'statusid': '" + statusid + "' }",
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
            alert('Data Saved successfully.');
            //BindSubtask_data();
            var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
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
                    url: 'MultiProjectSubtask.aspx/UpdatePriority_Status',
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
                    url: 'MultiProjectSubtask.aspx/UpdateExtendeddatedtls_subtask',
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
                    url: 'MultiProjectSubtask.aspx/Bindsubtaskdetail_extendatedate',
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
                    url: 'MultiProjectSubtask.aspx/Update_Stagge',
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
                    url: 'MultiProjectSubtask.aspx/UpdateSubTask_Dependency',
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
                    url: 'MultiProjectSubtask.aspx/Bindsubtask_Dependancy',
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
                    url: 'MultiProjectSubtask.aspx/SaveTimer',
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
                     debugger
                    
        
                    
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
                url: 'MultiProjectSubtask.aspx/SaveTimerstartdetails',
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
                    url: 'MultiProjectSubtask.aspx/SaveTimerpausedetails',
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
                    url: 'MultiProjectSubtask.aspx/SaveTimerresumedetails',
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

 <script>
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
   
  
    </script>

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


<script>
    debugger
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

    });
   
  
    </script>


<script>
    debugger
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

    });
   
  
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
    

  




   



 









    


  


 





