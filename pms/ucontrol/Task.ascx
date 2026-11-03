<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Task.ascx.cs" Inherits="ucontrol_Task" %>
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
        .preview-item {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    object-fit: cover;
    margin: 5px;
    display: inline-block;
    box-shadow: 0 0 2px rgba(0,0,0,0.2);
}
    </style>
<%-- <div class="page-content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">--%>
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico purpel">
                                            <i class="ri-list-check-2"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p><%=fns.getltext("Task", "2")%></p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside" style="display:none;">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Task</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end">
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTask(0)"><span class="align-middle">All Task</span></a>                         
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTask(1)"><span class="align-middle">Status 1</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTask(2)"><span class="align-middle">Status 2</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTask(3)"><span class="align-middle">Status 3</span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                               


                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <%if (ischeckCreate == 1)
                                            {%> 
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addtask"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                        <%}%>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                        <button type="button"  class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">
                                            
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>
                                          <a class="dropdown-item toggle-vis" runat="server"  id="task2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Project", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task", "2")%></a>
                                               <a class="dropdown-item toggle-vis" runat="server"  id="task5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timer", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timer History", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Description", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Dependency", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task10" data-column="10"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Weightage", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Sub Task", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task15" data-column="15"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Extended Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task16" data-column="16"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Start Date & Time", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task17" data-column="17"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("End Date & Time", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task18" data-column="18"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task19" data-column="19"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task20" data-column="20"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issues", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task21" data-column="21"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("File", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task22" data-column="22"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task Duration (Hour)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task23" data-column="23"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Stage", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task24" data-column="24"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Work Hours (W)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task25" data-column="25"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timelog Total (T)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="task26" data-column="26"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Difference (W-T)", "2")%></a>

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
                        <div class="card-body">
                            <div class="table-responsive">
                                <div id="divgridview" style="width:100%;">
                                <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                                 </div>
                             <div style="display:none;">
                              <asp:Button ID="btndelete" runat="server"    Text="delete"  />
                             </div>
                            </div>
                        </div>
         <%--           </div>
                </div>
            </div>           
        </div>
    </div>--%>

<div class="offcanvas offcanvas-end" id="addtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
             <label id="lbltitle" runat="server"><%=fns.getltext("Add Task", "2")%></label>   

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
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
                                 <asp:Button ID="btnload" runat="server" Text="" OnClick="btnload_Click" />
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                   <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnmilstoneid" runat="server" Value="0" />

                                   <div class="col-xxl-12 col-lg-12" id="divproject" >
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectId" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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

                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true"  OnSelectedIndexChanged="Milestoneid_SelectedIndexChanged" onchange="GetWeitagebindDtls()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnweightage" runat="server" Value="0" />
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
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklist" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>



                                    <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                       <label class="form-label flex-grow-1"><%=fns.getltext("Task Name", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="taskname" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="taskname" runat="server" placeholder="Task Name" class="form-control">
                                </div>


                              <div class="col-xxl-12 col-lg-12" id="divmiletimeline" style="display:none;">
                              <asp:Label ID="lblmiletimeline" runat="server"></asp:Label>
                              </div>

                             <%--<div class="card-body" id="divtasktimeline" style="display:none;">
                            <div class="table-responsive">
                            <asp:Label ID="lbltasktimeline" runat="server"></asp:Label>
                            </div>
                           </div>--%>

                                
                            <div class="col-xxl-12 col-lg-12" id="divtasktimeline" style="display:none;">
                           <asp:Label ID="lbltasktimeline" runat="server"></asp:Label>
                            </div>



                                <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="issubtask" runat="server" />  Has Sub Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                    <div class="col-xxl-10 col-lg-12" >
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkTaskWeightage" runat="server" />  Do You Want Same Weightage on Sub Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                



                                
                             
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Description", "2")%></label>
                                    </div>
                                    <textarea id="taskdescripton" runat="server" class="form-control" placeholder="Task Description"></textarea>
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
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Startdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input type="text" id="Startdate" runat="server" class="form-control" onchange="checkcomparisionmilestonedate()">--%>
                                              <input type="text" id="Startdate" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id),validateHours()">
                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                              <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;
                                            
                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="StartTime" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="StartTime" runat="server" class="form-control" onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>




                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Targetdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>
                                   <%-- onchange="checkcomparisionmilestonedate()"--%>
                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Targetdate" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id),validateHours()">
                                             <%--<input type="date" id="Targetdate" runat="server" class="form-control" onchange="checkcomparisionmilestonedate()">--%>
                                               <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;
                                            
                                          <%--  <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Timeline" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Timeline" runat="server" class="form-control" onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                    <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour" onkeyup="checkdurationcalculatenew(this.id),validateHours()" class="form-control">
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
                                             <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Status" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
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
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp; </label>
                                    </div>
                                    
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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
                                
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
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
                                
                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="ChooseDate" runat="server" class="form-control" onclick="this.showPicker()">
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
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Time" runat="server" class="form-control" onclick="this.showPicker()">
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
                                   
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                      


                                </div> 
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
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
                               <asp:Button ID="btnuploadimg" runat="server" Text="" OnClick="btnuploadimg_Click" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:FileUpload ID="Uploaddrawing" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreviewnew();"  />
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

   <%-- <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Save</a>
        <a href="javascript:void(0);" class="btn btn-secondary">Save & Add More</a>
    </div>--%>

    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-center"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img2" />
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
                         <asp:HiddenField ID="hdntransaction_id" runat="server" Value="" />
                          <asp:HiddenField ID="hdnstoptimer_id" runat="server" Value="" />

                          <asp:HiddenField ID="hdnprojecnots_tid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnmilestonenotes_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasklistnotes_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasknotes_id" runat="server" Value="0" />
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


<div id="ExtendedTimline_task" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended_task" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button4" runat="server" Text="" OnClick="Button4_Click" />
   </div>
                      <asp:HiddenField ID="hdnextendid_task" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml_task" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button4" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendatestrt_task" runat="server" class="form-control"   onclick="this.showPicker()">
                                               <asp:HiddenField ID="hdnextndstrt_task" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button4" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendateend_task" runat="server" class="form-control"  onclick="this.showPicker()">
                                              <asp:HiddenField ID="hdnextndend_task" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveextended_task" class="btn btn-primary" onclick="SvaeExtended_task('saveExtended_task');">Save</button>
            </div>
        </div>
    </div>
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
 <script src="/pms/assets/js/jscolor.js"></script>

<script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>

<%--<script>
    const table = new DataTable('.datatable', {  
        scrollX:  true,
        scrollCollapse: true,
        paging:         true,
        searching: true,
 
    });
</script>--%>
<%--<script>    
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
</script>--%>
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


                     

<asp:HiddenField ID="hdntype" runat="server" />
<asp:HiddenField ID="hdntypeproject" runat="server" />
<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
        document.getElementById("divproject").style.display = "none";

        if (document.getElementById('<%=hdntypeproject.ClientID%>').value == "Project")
        {
            document.getElementById("divmilestone").style.display = "block";
        }
        else
        {
              document.getElementById("divmilestone").style.display = "none";
        }

        document.getElementById("divptasklist").style.display = "none";




         var RecordId=document.getElementById("<%=Milestoneid.ClientID%>").value;
         
         $.ajax({
                    url: 'Add_Task.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise,
                    error: function (response) {
                        //alert(response.responseText);
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
                      if (waitagechkdtls.find("checkmileWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkmileWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }


      //Owner Bind Milestone Wise;
        debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'Add_Task.aspx/bindMultiOwner_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
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
    





     //Team Bind Milestone Wise;
        debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'Add_Task.aspx/bindMultiTeam_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
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


        //  Milestone Timeline //
              debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var mile_id = document.getElementById("<%=Milestoneid.ClientID%>").value;
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'Add_Task.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

     
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }



           //  Task Timeline //
             debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = document.getElementById("<%=Milestoneid.ClientID%>").value;
             //alert(projec_id);
        
                debugger;
                  $.ajax({
                      url: 'Add_Task.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
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

           

      }
    

</script>



<script>

      function GetOwnerempdetails() {
        debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'Add_Task.aspx/bindMultiOwner_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
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
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'Add_Task.aspx/bindMultiTeam_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
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

                  var RecordId=document.getElementById("<%=Milestoneid.ClientID%>").value;

                  $.ajax({
                    url: 'Add_Task.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschk,
                    error: function (response) {
                        //alert(response.responseText);
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
                      if (waitagechkdtls.find("checkmileWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkmileWeightage").text() == 0)
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

             document.getElementById("divmiletimeline").style.display = "block";
            Bindmilestonetimeline_data();
             document.getElementById("divtasktimeline").style.display = "block";
            Bindtasktimeline_data();


         }


       
    }
    

</script>


<script>
   
    function waitagepercentchk(input)
    {
        
        if (input.value > 100)
        {
            
           // input.value = 100;
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

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
        if (document.getElementById("<%=Startdate.ClientID%>").value != "" && document.getElementById("<%=Targetdate.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=Startdate.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=Targetdate.ClientID%>").value);
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

               var milesto_nid = document.getElementById("<%=Milestoneid.ClientID%>").value;



                $.ajax({
                    url: 'Add_Task.aspx/Bindcheckstrtdatemiledetails',
                    data: "{ 'milesto_nid': '" + milesto_nid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var taskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (taskstrtdatechkdtls.find("id").text() != "")
                        {
                            const today = new Date(taskstrtdatechkdtls.find("StartDate").text());
                            var fdt = new Date(today);
                            const today2 = new Date(taskstrtdatechkdtls.find("EndDate").text());
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
                function showdivimg() {
                    if (Page_ClientValidate("btnclose")) {
                        //if (confirm('Do you want Leave this page ?')) {   

                           Cleardata();
                            document.getElementById("btnclose").click();
                                
                            return true;
                       // }
                       // else {
                          //  return false;
                        //}
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
                            url: "Add_Task.aspx/statussave",
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
                            url: "Add_Task.aspx/AddPrioritytype",
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
                            url: "Add_Task.aspx/AddStage",
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


                        
                        var taskdetails = {};
                        taskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; 
                        taskdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        taskdetails.Milestoneid = document.getElementById('<%=Milestoneid.ClientID%>').value; 
                        taskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        taskdetails.taskname = document.getElementById('<%=taskname.ClientID%>').value; 
                        taskdetails.taskdescripton = document.getElementById('<%=taskdescripton.ClientID%>').value; 
                        taskdetails.owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        taskdetails.Startdate = document.getElementById('<%=Startdate.ClientID%>').value; 
                        taskdetails.Targetdate = document.getElementById('<%=Targetdate.ClientID%>').value;
                        taskdetails.StartTime = document.getElementById('<%=StartTime.ClientID%>').value; 
                        taskdetails.Timeline = document.getElementById('<%=Timeline.ClientID%>').value; 
                        taskdetails.TaskDuration  = document.getElementById('<%=taskDuration.ClientID%>').value; 
                        <%--taskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        taskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>
                        taskdetails.P_Status = statusid;
                        taskdetails.Priority = Prioritytypeid;
                        taskdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        taskdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;
                        taskdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 
                        taskdetails.ReminderType = document.getElementById('<%=ReminderType.ClientID%>').value; 
                        taskdetails.Days = document.getElementById('<%=Days.ClientID%>').value; 
                        taskdetails.Time = document.getElementById('<%=Time.ClientID%>').value; 
                        taskdetails.Notifyusers = document.getElementById('<%=hdnNotifyusers.ClientID%>').value; 
                        taskdetails.ChooseDate = document.getElementById('<%=ChooseDate.ClientID%>').value; 
                        
                        taskdetails.Occurrence = document.getElementById('<%=Occurrence.ClientID%>').value; 
                        taskdetails.tagto = document.getElementById('<%=tagto.ClientID%>').value; 
                        taskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value;
                        taskdetails.Stage = stageid;
 document.getElementById('<%=btnuploadimg.ClientID%>').click();
                     taskdetails.Uploaddrawing = document.getElementById('<%=hdnUploaddrawing.ClientID%>').value;
            
                       taskdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value;
                   
                    if (document.getElementById('<%=issubtask.ClientID%>').checked == true)
                    {

                        taskdetails.issubtask = 1

                    }

                    if (document.getElementById('<%=issubtask.ClientID%>').checked == false)
                    {

                        taskdetails.issubtask = 0

                   }


                      if (document.getElementById('<%=checkTaskWeightage.ClientID%>').checked == true)
                       {

                        taskdetails.checkTaskWeightage = 1

                      }

                      if (document.getElementById('<%=checkTaskWeightage.ClientID%>').checked == false)
                      {

                        taskdetails.checkTaskWeightage = 0

                      }

                  
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
                    UploadFiles(response.d);
                    alert("Data Saved successfully."); 

                    document.getElementById('<%=btnload.ClientID%>').click();

                   
                    document.getElementById('<%=ProjectId.ClientID%>').value = "0";
                    document.getElementById('<%=Milestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=taskname.ClientID%>').value = "";
                    document.getElementById('<%=taskdescripton.ClientID%>').value = "";
                    document.getElementById('<%=Startdate.ClientID%>').value = "";
                    document.getElementById('<%=Targetdate.ClientID%>').value = "";
                     document.getElementById('<%=StartTime.ClientID%>').value = "";
                     document.getElementById('<%=Timeline.ClientID%>').value = "";
                    document.getElementById('<%=P_Status.ClientID%>').value = "0";
                     document.getElementById('<%=Priority.ClientID%>').value = "0";
                    document.getElementById('<%=Reminder.ClientID%>').value = "";
                     document.getElementById('<%=Occurrence.ClientID%>').value = "";
                     document.getElementById('<%=tagto.ClientID%>').value = "";
                     document.getElementById('<%=ReminderType.ClientID%>').value = "";
                   
try
{
 document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/pms/assets/images/user.jpg";
                   
}
catch
{}

                      document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                     document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                    $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnstartdate.ClientID%>').value ="";
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = "";
                     document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "0";
                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                     document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                    document.getElementById('<%=hdnteam.ClientID%>').value = "";
                    document.getElementById('<%=hdnstage.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
              document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
                    document.getElementById('<%=ChooseDate.ClientID%>').value = "";

                    document.getElementById('<%=issubtask.ClientID%>').checked = 0;
                    document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = 0;

                      $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyusers.ClientID%>")).change();

                   $(document.getElementById("<%=team.ClientID%>")).val(0);
                   $(document.getElementById("<%=team.ClientID%>")).change();

                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        Bindtask_data();
                    }
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        //document.getElementById("btncls").click();
                        Bindtask_data();
                    }
                   

               
                    
                }



          function Cleardata() {
                        
                 debugger
                   document.getElementById('<%=ProjectId.ClientID%>').value = "0";
                    document.getElementById('<%=Milestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=taskname.ClientID%>').value = "";
                    document.getElementById('<%=taskdescripton.ClientID%>').value = "";
                    document.getElementById('<%=Startdate.ClientID%>').value = "";
                    document.getElementById('<%=Targetdate.ClientID%>').value = "";
                     document.getElementById('<%=StartTime.ClientID%>').value = "";
                     document.getElementById('<%=Timeline.ClientID%>').value = "";
                    document.getElementById('<%=P_Status.ClientID%>').value = "0";
                     document.getElementById('<%=Priority.ClientID%>').value = "0";
                    document.getElementById('<%=Reminder.ClientID%>').value = "";
                     document.getElementById('<%=Occurrence.ClientID%>').value = "";
                     document.getElementById('<%=tagto.ClientID%>').value = "";
                     document.getElementById('<%=ReminderType.ClientID%>').value = "";



                      document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                     document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                  document.getElementById('<%=hdntasklist.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnstartdate.ClientID%>').value ="";
              document.getElementById('<%=hdntargetdate.ClientID%>').value = "";

              document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "0";


                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
              document.getElementById('<%=hdnPriority.ClientID%>').value = "";
               document.getElementById('<%=Budget.ClientID%>').value = "";
                document.getElementById('<%=Actual.ClientID%>').value = "";
              document.getElementById('<%=hdnteam.ClientID%>').value = "";

              document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnstage.ClientID%>').value = "";
               document.getElementById('<%=Weightage.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";
              document.getElementById('<%=issubtask.ClientID%>').checked = 0;
               document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = 0;

                $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();

               $(document.getElementById("<%=team.ClientID%>")).val(0);
             $(document.getElementById("<%=team.ClientID%>")).change();

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
                    
                    $("[id*=ProjectId]").val(Bindtaskdetail.find("ProjectId").text());
                     $("[id*=Milestoneid]").val(Bindtaskdetail.find("Milestoneid").text());
                    $("[id*=tasklist]").val(Bindtaskdetail.find("tasklist").text());
                    $("[id*=taskname]").val(Bindtaskdetail.find("taskname").text());
                    $("[id*=taskdescripton]").val(Bindtaskdetail.find("taskdescripton").text());
                    $("[id*=owner]").val(Bindtaskdetail.find("owner").text());
                    $("[id*=Startdate]").val(Bindtaskdetail.find("Startdate").text());
                    $("[id*=Targetdate]").val(Bindtaskdetail.find("Targetdate").text()); 
                    $("[id*=P_Status]").val(Bindtaskdetail.find("P_Status").text()); 
                    $("[id*=Priority]").val(Bindtaskdetail.find("Priority").text()); 

                    $("[id*=Budget]").val(Bindtaskdetail.find("Budget").text()); 
                    $("[id*=Actual]").val(Bindtaskdetail.find("Actual").text()); 

                    $("[id*=Reminder]").val(Bindtaskdetail.find("Reminder").text()); 
                    $("[id*=Occurrence]").val(Bindtaskdetail.find("Occurrence").text()); 
                    $("[id*=tagto]").val(Bindtaskdetail.find("tagto").text()); 
                    $("[id*=team]").val(Bindtaskdetail.find("team").text()); 

                    $("[id*=ReminderType]").val(Bindtaskdetail.find("ReminderType").text()); 
                    $("[id*=Days]").val(Bindtaskdetail.find("Days").text()); 
                    $("[id*=Time]").val(Bindtaskdetail.find("team").text()); 
                    $("[id*=Notifyusers]").val(Bindtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDate]").val(Bindtaskdetail.find("ChooseDate").text()); 
                    
try
{
if (Bindtaskdetail.find("Uploaddrawing").text() != "" && Bindtaskdetail.find("Uploaddrawing").text() != "0")
                    {
                        
                        bindUploadedDrawings(Bindtaskdetail.find("Uploaddrawing").text());
                    }
}
catch
{}
                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Bindtaskdetail.find("Milestoneid").text();
                     document.getElementById('<%=hdntasklist.ClientID%>').value = Bindtaskdetail.find("tasklist").text();
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = Bindtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = Bindtaskdetail.find("Targetdate").text();

                    document.getElementById('<%=hdnstarttime.ClientID%>').value = Bindtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = Bindtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDuration.ClientID%>').value = Bindtaskdetail.find("TaskDuration").text();

                    document.getElementById('<%=P_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                     document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";


                    document.getElementById('<%=Priority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindtaskdetail.find("team").text();

                    document.getElementById('<%=Stage.ClientID%>').value = Bindtaskdetail.find("Stage").text();
                    document.getElementById('<%=hdnstage.ClientID%>').value = Bindtaskdetail.find("Stage").text();
                     document.getElementById('<%=txtnewstage.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').style.display = "none";

                     document.getElementById('<%=Weightage.ClientID%>').value = Bindtaskdetail.find("Weightage").text();

                    document.getElementById('<%=hdnreminder.ClientID%>').value = Bindtaskdetail.find("Reminder").text();
                    document.getElementById('<%=hdnReminderType.ClientID%>').value = Bindtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=hdnTime.ClientID%>').value = Bindtaskdetail.find("Time").text();
                    document.getElementById('<%=hdnNotifyusers.ClientID%>').value = Bindtaskdetail.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDate.ClientID%>').value = Bindtaskdetail.find("ChooseDate").text();
                

                  if (Bindtaskdetail.find("issubtask").text() == 1)
                  {
                      document.getElementById('<%=issubtask.ClientID%>').checked = true;

                  }

                  if (Bindtaskdetail.find("issubtask").text() == 0)
                  {
                      document.getElementById('<%=issubtask.ClientID%>').checked = false;

                 }

                  if (Bindtaskdetail.find("checkTaskWeightage").text() == 1)
                  {
                      document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = true;

                  }

                  if (Bindtaskdetail.find("checkTaskWeightage").text() == 0)
                  {
                      document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = false;

                  }

                  

                    // time and reminder fill and edit strt//

                     if (Bindtaskdetail.find("Reminder").text() == "0")
                     {
                       document.getElementById("divremindertype").style.display = "none";
                       document.getElementById("divDays").style.display = "none";
                       document.getElementById("Divtime").style.display = "none";
                       document.getElementById("divNotifyusers").style.display = "none";
                       document.getElementById("divChooseDate").style.display = "none";

                    }

                    // document.getElementById("divmiletimeline").style.display = "block";
                    //Bindmilestonetimeline_data();


               //  Milestone Timeline //
              debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'Add_Task.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

     
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }



                    //document.getElementById("divtasktimeline").style.display = "block";
                    //Bindtasktimeline_data();


             //  Task Timeline //
             debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
        
                debugger;
                  $.ajax({
                      url: 'Add_Task.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
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

                    

                    GetReminderDtls();
                    GetRemindertypeDtls();

                    

                    // time and reminder fill and edit end//

             if (Bindtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }

            document.getElementById('<%=Button1.ClientID%>').click();

             GetOwnerempdetails();
             if (Bindtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
                  else
                  {

                   var idsemp = Bindtaskdetail.find("owner").text();
                  $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
             
             }



             GetTeamempdetails();
             if (Bindtaskdetail.find("team").text() != "")
             {
                 debugger
                 if (Bindtaskdetail.find("team").text().includes(','))
                 {
                      var idsemp = Bindtaskdetail.find("team").text().split(',');
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = Bindtaskdetail.find("team").text();
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 

            }


                     
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit  Task";
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
debugger
             if (document.getElementById('<%=hdnbtns.ClientID%>').value != "") {
                 var txtids = document.getElementById('<%=hdnbtns.ClientID%>').value.split(',');
                 if (txtids.length > 0) {
                     for (var i = 0; i <= txtids.length; i++) {
                         var btn = document.getElementById('btnauto_' + txtids[i]);

            if (btn) {
                btn.click();
            }
                     }
                 }
             }
                //alert("ji");
       
       
        }

   
        $(document).ready(function () {
            
            Bindtask_data();
           
            //start_1('00', '00', '00', '000', '');
});
         function Bindtask_data() {
            
             debugger;
            // alert($("[id*=hdiprojid]").val());
             //alert($("[id*=hdnmilstoneid]").val());
                  $.ajax({
                    url: 'Add_Task.aspx/bindtaskdata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "' ,'milstoneid': '" + $("[id*=hdnmilstoneid]").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetailsmain,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetailsmain(response)
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
            leftColumns: 5
        }
        



                });
            }
            catch
            { }
            //alert("hi");
            autoplay(); 

            hidecolumngrid();


        }
       
    </script>



<script>
    
    $(document).ready(function ()
    {
            Bindmilestonetimeline_data();
});
    function Bindmilestonetimeline_data()
    {
             debugger
            
             
             var mile_id = document.getElementById("<%=Milestoneid.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'Add_Task.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
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
            
             
             var mile_id = document.getElementById("<%=Milestoneid.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'Add_Task.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
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


 <%--Owner details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeDataowner(val12A) {

            if (Page_ClientValidate("btnsaveowner")) {
               
                debugger
                
                // save all details

            
                $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/Savetask_Owner',
                    //data: '{Bindtaskdetail: ' + JSON.stringify(Bindtaskdetail) + '}',
                     data: "{ 'id': '" + document.getElementById('<%=hdnchangeid.ClientID%>').value + "' ,'ownerdropdownid': '" + document.getElementById('<%=hdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessowner,
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
                function OnSuccessowner(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=ddlchangeowner]").val("");
                   
                     document.getElementById('<%=hdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();
                    document.getElementById("btnclsowner").click();
                    // Bindproject_data();
                    debugger
                    
                    document.getElementById("lblowner_" + document.getElementById("<%=hdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
          
        
                    
                }

        

          function Binddetails_Owner(RecordId,lblownerid) {
              
              debugger;

               document.getElementById("<%=hdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
                 
                $.ajax({
                    url: 'Add_Task.aspx/Bindtaskdetail_Owner',
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
                var Bindtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = Bindtaskdetail.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = Bindtaskdetail.find("owner").text();

               document.getElementById('<%=multichngmilestoneid.ClientID%>').value = Bindtaskdetail.find("Milestoneid").text();
              Multichangeowner();
              if (Bindtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindtaskdetail.find("owner").text();
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

                var taskdetails = {};
                taskdetails.id = document.getElementById('<%=hdnteameid.ClientID%>').value;;
                taskdetails.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/Save_Team',
                    data: '{taskdetails: ' + JSON.stringify(taskdetails) + '}',
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
                   // Bindtask_data();
                   document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                    
                }

        

          function Binddetails_Team(RecordId,lbluserid) {

          
              debugger;
               document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                 
                $.ajax({
                    url: 'Add_Task.aspx/Bindetail_Team',
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
                var taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = taskdetails.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = taskdetails.find("team").text();

              document.getElementById('<%=multichngmilestoneid.ClientID%>').value = taskdetails.find("Milestoneid").text();
              MultichangeTeam();
              if (taskdetails.find("team").text() != "")
              {
                  debugger
                  if (taskdetails.find("team").text().includes(','))
                  {
                      var idsemp = taskdetails.find("team").text().split(',');
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = taskdetails.find("team").text();
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
    <asp:HiddenField ID="hdtaskid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
                   var taskid=document.getElementById("<%=hdtaskid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Add_Task.aspx/Update_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
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
        <%--function OnSuccessStatusupdt(response) {
            alert('Data Saved successfully.');
            Bindtask_data();
            var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }--%>
        
        function setprojectid(taskid,btnststusid)
        {
            debugger
             document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdtaskid.ClientID%>").value = taskid;
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
                    url: 'Add_Task.aspx/UpdatePriority_Status',
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
            //Bindtask_data();
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
             
        function SvaeExtended_task(val225A) {

            if (Page_ClientValidate("btnsaveextended_task")) {
               
                debugger
                
                // save all details


                var task_id = document.getElementById('<%=hdnextendid_task.ClientID%>').value;
                var extendStrtdate_task = document.getElementById('<%=Extendatestrt_task.ClientID%>').value;
                var extendEnddate_task = document.getElementById('<%=Extendateend_task.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/UpdateExtendeddatedtls_task',
                    data: "{ 'task_id': '" + task_id + "' ,'extendStrtdate_task': '" + extendStrtdate_task + "','extendEnddate_task': '" + extendEnddate_task + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessupdtexdate_task,
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
                function OnSuccessupdtexdate_task(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=hdnextndstrt_task.ClientID%>').value = "";
                    document.getElementById('<%=hdnextendid_task.ClientID%>').value = "";
                    document.getElementById("btnclsextended_task").click();
                    //Bindproject_data();
                    debugger
                    
                   
                    document.getElementById("lblExtnddttask_" + document.getElementById("<%=hdnextnhtml_task.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Bindextenddatetask_Details(RecordId,lblExtndid) {
              
              debugger;

              document.getElementById("<%=hdnextnhtml_task.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'Add_Task.aspx/Bindtaskdetail_extendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate_task,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate_task(response) {
            // alert(response.d);
            debugger
            try {
                var taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnextendid_task.ClientID%>').value = taskdetails.find("id").text();
                    
                    document.getElementById('<%=hdnextndstrt_task.ClientID%>').value = taskdetails.find("StartDate").text();
                    document.getElementById('<%=hdnextndend_task.ClientID%>').value = taskdetails.find("TargetDate").text();
                     document.getElementById('<%=Button4.ClientID%>').click();
             
                   
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
                    url: 'Add_Task.aspx/Update_Stagge',
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
                
                  var taskid = document.getElementById('<%=hdnchngdependencyid.ClientID%>').value; 
                  var dependencyid = document.getElementById('<%=hdndependancy.ClientID%>').value;
        
                $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/UpdateTask_Dependency',
                    data: "{ 'taskid': '" + taskid + "' ,'dependencyid': '" + dependencyid + "'}",
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
                    Bindtask_data();
                    document.getElementById("lblDependancy_" + document.getElementById("<%=hdnchngdependcyhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Dependency(RecordId,lbldependencyid) {
              
              debugger;
              document.getElementById("<%=hdnchngdependcyhtml.ClientID%>").value = lbldependencyid.split('_')[1];
             
                 
                $.ajax({
                    url: 'Add_Task.aspx/Bindtask_Dependancy',
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

                    if (taskdetails.find("dependancy").text() != "") {
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
                var ctrlid = document.getElementById('<%=hdntask_id.ClientID%>').value;
                
                    vartaskid = ctrlid;
                   //document.getElementById('hour_' + vartaskid).innerHTML = "00";
                   // document.getElementById('minute_' + vartaskid).innerHTML = "00";
                   // document.getElementById('second_' + vartaskid).innerHTML = "00";
                   // document.getElementById('millisecond_' + vartaskid).innerHTML="000";
                 // alert(document.getElementById('hour_' + vartaskid).innerHTML);
                
                
                $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/SaveTimer',
                    data: "{'ProjectId': '" + document.getElementById('<%=hdnprojec_tid.ClientID%>').value + "','Milestoneid': '" + document.getElementById('<%=hdnmilestone_id.ClientID%>').value + "' ,'tasklistid': '" + document.getElementById('<%=hdntasklist_id.ClientID%>').value + "','taskid': '" + document.getElementById('<%=hdntask_id.ClientID%>').value + "','TimerNotes': '" + document.getElementById('<%=TimerNotes.ClientID%>').value + "','hour': '" + document.getElementById('hour_' + vartaskid).innerHTML + "','minute': '" + document.getElementById('minute_' + vartaskid).innerHTML + "','second': '" + document.getElementById('second_' + vartaskid).innerHTML + "','millisecond': '" + document.getElementById('millisecond_' + vartaskid).innerHTML + "','Transactionid': '" + document.getElementById('<%=hdntransaction_id.ClientID%>').value + "'}",
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
                    var ctrlid = document.getElementById('<%=hdntask_id.ClientID%>').value;
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
                    Bindtask_data();
                    
        
                    
                }

        

          function Binddetails_Timer(ProjectId,Milestoneid,tasklistid,taskid,hours,minutes,seconds,milliseconds,transactionid,varid) {
              
              debugger;

                    document.getElementById('<%=hdnprojec_tid.ClientID%>').value = ProjectId;
                    document.getElementById('<%=hdnmilestone_id.ClientID%>').value = Milestoneid;
                    document.getElementById('<%=hdntasklist_id.ClientID%>').value = tasklistid;
              document.getElementById('<%=hdntask_id.ClientID%>').value = taskid;
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
           
              
                //$.ajax({
                //    url: 'Add_Task.aspx/BindTimerDetails',
                //    data: "{'ProjectId': '" + ProjectId + "','Milestoneid': '" + Milestoneid + "','tasklistid': '" + tasklistid + "','taskid': '" + taskid + "'}",
                //    dataType: "json",
                //    type: "POST",
                //    contentType: "application/json; charset=utf-8",
                //    async: false,
                //    success: OnSuccess1Timer,
                //    error: function (response) {
                //        //alert(response.responseText);
                //        alert(response.responseText);
                //    },
                //    failure: function (response) {
                //        alert(response.responseText);
                //    }
                //});

        }


         function OnSuccess1Timer(response) {
            
           <%-- debugger
            try {
                var BindTimerdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                
                if (BindTimerdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = BindTimerdetail.find("id").text();
                    document.getElementById('<%=hdnprojec_tid.ClientID%>').value = BindTimerdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnmilestone_id.ClientID%>').value = BindTimerdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist_id.ClientID%>').value = BindTimerdetail.find("tasklist").text();
                    document.getElementById('<%=hdntask_id.ClientID%>').value = BindTimerdetail.find("taskid").text();
                  
                   

                }
              


              
                

            }
            catch
                {
                }--%>

            }


        
        


</script>


<%-- add timer notes end--%>



<%-- timer start --%>

<%--<asp:HiddenField ID="hdnstsrttime" runat="server" Value="0" />
<asp:HiddenField ID="hdnenddate" runat="server" Value="0" />--%>
<script>
   
        function hideshow(div1,div2)
        {
            debugger
            //alert(div1.id);
            //alert(div2.id);
            document.getElementById(div1.id).style.display = "none";
            document.getElementById(div2.id).style.display = "block";
       }

        function hideshowPausebutton(btnpause,btnresum)
        {
            debugger
            //alert(btnpause.id);
            document.getElementById(btnpause.id).style.display = "none";
            document.getElementById(btnresum.id).style.display = "inline-block";
        }
        function hideshowResumebutton(btnpause,btnresum)
        {
            debugger
            //alert(btnpause.id);
            document.getElementById(btnpause.id).style.display = "inline-block";
            document.getElementById(btnresum.id).style.display = "none";
       }

       

   
    

</script>
    
<asp:HiddenField ID="hdnprojecttaskid" runat="server" Value="0" />
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
  //pause();
        cron = setInterval(() => { timer(hours.id, minutes.id, seconds.id, milliseconds.id); }, 10);

  //      hour = 4;
  //minute = 35;
  //second = 22;
  //millisecond = 100;
}
    function savestartdetails(projectid, milstoneid, tasklistid, taskid,chkdepandancy)
    {
        debugger
        if (chkdepandancy != "")
        {
            alert("There is already a dependency on this task " + chkdepandancy);
            return false;
        }
        else {
            document.getElementById("<%=hdnprojecttaskid.ClientID%>").value = taskid;
            $.ajax({
                type: "POST",
                url: 'Add_Task.aspx/SaveTimerstartdetails',
                data: "{'ProjectId': '" + projectid + "','Milestoneid': '" + milstoneid + "' ,'tasklistid': '" + tasklistid + "','taskid': '" + taskid + "'}",
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
       var cid= document.getElementById("hdntrans_" + document.getElementById("<%=hdnprojecttaskid.ClientID%>").value)
        cid.value = response.d;
    }

     
        function SvaePauseDetialss(val12A) {

            if (Page_ClientValidate("btnpausesaveNotes")) {

               
                debugger


                var ProjectId = document.getElementById('<%=hdnprojecnots_tid.ClientID%>').value;
                var Milestoneid = document.getElementById('<%=hdnmilestonenotes_id.ClientID%>').value;
                var tasklistid = document.getElementById('<%=hdntasklistnotes_id.ClientID%>').value;
                var taskid= document.getElementById('<%=hdntasknotes_id.ClientID%>').value;
                var varid = document.getElementById('<%=hdnpause_id.ClientID%>').value;
                var transactionid = document.getElementById('<%=hdnnotestransaction_id.ClientID%>').value;
                  
            var cid = document.getElementById("hdntrans_" + taskid)
            //cid.value = response.d;
            if (transactionid == "" || transactionid == "0") {
                transactionid = cid.value;
            }

            //alert(cid.value);
            //return false;
                
                
                $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/SaveTimerpausedetails',
                    data: "{'ProjectId': '" + ProjectId + "','Milestoneid': '" + Milestoneid + "' ,'tasklistid': '" + tasklistid + "','taskid': '" + taskid + "','transactionid': '" + transactionid + "','Notes': '" + document.getElementById('<%=Notes.ClientID%>').value + "'}",
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
                   var cornid=document.getElementById('<%=hdnpause_id.ClientID%>').value; 
               pause(cornid);

                    var ctrlid = document.getElementById('<%=hdntasknotes_id.ClientID%>').value;
                    document.getElementById('btnpause_' + ctrlid).style.display ="none";
                    document.getElementById('btnresum_' + ctrlid).style.display = "inline-block";

                    document.getElementById('<%=Notes.ClientID%>').value = "";
                    document.getElementById("btnclsNotes").click();
                    debugger
                    
        
                    
                }

        

          function BindPausedetails_Notes(ProjectId,Milestoneid,tasklistid,taskid,transactionid,varid) {
              
              debugger;

                    document.getElementById('<%=hdnprojecnots_tid.ClientID%>').value = ProjectId;
                    document.getElementById('<%=hdnmilestonenotes_id.ClientID%>').value = Milestoneid;
                    document.getElementById('<%=hdntasklistnotes_id.ClientID%>').value = tasklistid;
                    document.getElementById('<%=hdntasknotes_id.ClientID%>').value = taskid;

                   document.getElementById('<%=hdnpause_id.ClientID%>').value = varid;
                   document.getElementById('<%=hdnnotestransaction_id.ClientID%>').value = transactionid;
                   document.getElementById('<%=Notes.ClientID%>').value = "";
               

        }


    function OnSuccess1Notes(response)
    {
            
    }


    

 function saveResumeDetails(projectid, milstoneid, tasklistid,taskid,transactionid)
 {
     debugger
         var cid= document.getElementById("hdntrans_" + taskid)
         //cid.value = response.d;
         if (transactionid == "" || transactionid == "0") {
             transactionid = cid.value;
         }
         //alert(cid.value);
         //return false;
         $.ajax({
                    type: "POST",
                    url: 'Add_Task.aspx/SaveTimerresumedetails',
                    data: "{'ProjectId': '" + projectid + "','Milestoneid': '" + milstoneid + "' ,'tasklistid': '" + tasklistid + "','taskid': '" + taskid + "','transactionid': '" + transactionid + "'}",
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
debugger
 if (window[varcornid]) {
        clearInterval(window[varcornid]);
    }
//cron
    //clearInterval(cron);
    //var
    
    
}
function stop(varcornid) {
  //clearInterval(cron);
    //clearInterval(varcornid);
if (window[varcornid]) {
        clearInterval(window[varcornid]);
    }
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


<%--timer end--%>

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

<%-- <script>
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
    var ddteam = $(document.getElementById("<%=team.ClientID%>"));
   

    $(ddteam).select2({
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
    var ddLNotifyusers = $(document.getElementById("<%=Notifyusers.ClientID%>"));
   

    $(ddLNotifyusers).select2({
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

 <asp:HiddenField ID="multichngmilestoneid" runat="server" Value="0" />
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
        var mile_stoneid = document.getElementById("<%=multichngmilestoneid.ClientID%>").value;
        
        $.ajax({
            url: 'Add_Task.aspx/bindMultiChangeOwner_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
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
        var mile_stoneid = document.getElementById("<%=multichngmilestoneid.ClientID%>").value;
        
        $.ajax({
            url: 'Add_Task.aspx/bindMultiChangeTeam_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
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
                        url: 'Add_Task.aspx/Savecolumndata',
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


    <script type="text/javascript">
        function hidecolumngrid()
        {
            
            for (var i = 0; i < 27; i++)
            {  
              
                var aa = "ContentPlaceHolder1_Task_hdn" + i; 
                if (document.getElementById(aa).value == "0")
                { 
                    
                    let column = table.column(i); 
                    column.visible(false);

                }
            }
          
            document.getElementById("divgridview").style.display = "block";
            document.getElementById("divgridview").style.width = "100%";
            document.getElementById("ContentPlaceHolder1_Task_lblprojectdata").style.width = "100%";
           
        }
    </script>

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
<div id="viewissues" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title" id="lbldetails"></h5>
                <button type="button" class="btn-close" id="btnclsD" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row">
                        <div class="col-sm-12 mb-1">
                            <iframe id='issuesdetails' width="100%" height="490px" style="padding:0px 0px;" border="0px"></iframe>      
                        </div>                        
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</div>

<script type="text/javascript">

            ////debugger
            var count = 1;
            function loadsrcdetails(valframe ,valsrc,stredit) {
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

                    document.getElementById('lbldetails').innerHTML = stredit;
                }
                catch {
                }
            }

      
        </script>
<script>
function Binddetails_D(tid,ctrlid)
{
 debugger
                  $.ajax({
                    url: '/pms/Add_Task.aspx/Binddependencydata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'tid': '" +tid+ "'}",
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
                url: "FileUploadHandlertask.ashx?id="+id+"", 
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
debugger
            document.getElementById("<%=fileupload.ClientID%>").value = null;
         debugger
        }
function ShowPreviewnew() {
    debugger;

    var input = document.getElementById("<%= fileupload.ClientID %>");
    var previewContainer = document.getElementById("previewContainer");
    previewContainer.innerHTML = ''; // Clear previous previews

    if (input.files && input.files.length > 0) {
        let isAllValid = true;

        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];

            var isImage = file.type.startsWith('image/');
            var isPDF = file.type === 'application/pdf';

            if (!isImage && !isPDF) {
                alert("Only image or PDF files are allowed.");
                isAllValid = false;
                break;
            }

            if (isImage && file.size > 25000) {
                alert("Image '" + file.name + "' exceeds 25KB.");
                isAllValid = false;
                break;
            }

            if (isPDF && file.size > 1048576) {
                alert("PDF '" + file.name + "' exceeds 1MB.");
                isAllValid = false;
                break;
            }
        }

         if (!isAllValid) {
            input.value = ""; // Clear selected files
            previewContainer.innerHTML = ""; // Clear previews
            return;
        }

       for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];
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
                        var icon = document.createElement("div");
                        icon.textContent = "📄"; // PDF emoji
                        icon.style.fontSize = "40px";
                        icon.style.margin = "5px";
                        icon.title = theFile.name;
                        icon.className = "preview-item";
                        previewContainer.appendChild(icon);
                    }
                };
            })(file);

            reader.readAsDataURL(file);
        }
    }
}
function bindUploadedDrawings(strfiles) {
    var uploadData =strfiles; 
    var basePath = "/pms/TaskImages/";
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


<script>
function validateHours() {
    debugger;

    // Get input values
    const fromDateStr = document.getElementById("ContentPlaceHolder1_Task_Startdate").value;
    const toDateStr = document.getElementById("ContentPlaceHolder1_Task_Targetdate").value;
    const enteredHoursStr = document.getElementById("ContentPlaceHolder1_Task_taskDuration").value;

    let startTimeStr = document.getElementById("ContentPlaceHolder1_Task_StartTime").value;
    let endTimeStr = document.getElementById("ContentPlaceHolder1_Task_Timeline").value;

    // Default time to 00:00 if empty
    if (!startTimeStr) {
        startTimeStr = "00:00";
        //document.getElementById("ContentPlaceHolder1_Task_StartTime").value = startTimeStr;
    }
    if (!endTimeStr) {
        endTimeStr = "00:00";
        //document.getElementById("ContentPlaceHolder1_Task_Timeline").value = endTimeStr;
    }

    if (fromDateStr && toDateStr && enteredHoursStr) {
        const enteredHours = parseFloat(enteredHoursStr);

        if (isNaN(enteredHours) || enteredHours < 0) {
            alert("Please enter a valid positive number for hours.");
            document.getElementById("ContentPlaceHolder1_Task_taskDuration").value = "";
            return false;
        }

        const startDateTimeStr = `${fromDateStr}T${startTimeStr}`;
        const endDateTimeStr = `${toDateStr}T${endTimeStr}`;

        const fromDate = new Date(startDateTimeStr);
        const toDate = new Date(endDateTimeStr);

        if (fromDate > toDate) {
            alert("From Date & Time cannot be after To Date & Time.");
            return false;
        }

        let totalAvailableHours = 0;

       
        if (startTimeStr === "00:00" && endTimeStr === "00:00") {
            const oneDayMs = 1000 * 60 * 60 * 24;
            const dateDiffDays = Math.floor((toDate - fromDate) / oneDayMs)+1;
            totalAvailableHours = dateDiffDays * 24;

        }
 else {
           
            const diffMs = toDate - fromDate;
            totalAvailableHours = diffMs / (1000 * 60 * 60); // Convert ms to hours
        }

        // Validate
        if (enteredHours > totalAvailableHours) {
            alert(`Entered hours (${enteredHours}) exceed the maximum available (${totalAvailableHours.toFixed(2)}) for the selected date/time range.`);
            document.getElementById("ContentPlaceHolder1_Task_taskDuration").value = 0;
            return false;
        }

        return true;
    }

    return true;
}


function validateHoursold() {
    //alert("ok");
    debugger;

    // Get input values
    const fromDateStr = document.getElementById("ContentPlaceHolder1_Task_Startdate").value;
    const toDateStr = document.getElementById("ContentPlaceHolder1_Task_Targetdate").value;
    const enteredHoursStr = document.getElementById("ContentPlaceHolder1_Task_taskDuration").value;
     const startTimeStr = document.getElementById("ContentPlaceHolder1_Task_StartTime").value;
    const endTimeStr = document.getElementById("ContentPlaceHolder1_Task_Timeline").value;
    if (!startTimeStr) {
        startTimeStr = "00:00";        
    }
if (!endTimeStr) {
        endTimeStr = "00:00";        
    }
    if (fromDateStr && toDateStr && enteredHoursStr) {
        const enteredHours = parseFloat(enteredHoursStr);

        // If enteredHours is not a number, alert and stop
        if (isNaN(enteredHours)) {
            alert("Please enter a valid number for hours.");
            return false;
        }
       //const fromDate = new Date(fromDateStr);
        //const toDate = new Date(toDateStr);

    const startDateTimeStr = `${fromDateStr}T${startTimeStr}`;
    const endDateTimeStr = `${toDateStr}T${endTimeStr}`;


        const fromDate = new Date(startDateTimeStr);
        const toDate = new Date(endDateTimeStr);

        // Check date logic
        if (fromDate > toDate) {
            alert("From Date cannot be after To Date.");
            return false;
        }

        // Calculate total days (inclusive)
        //const oneDayMs = 1000 * 60 * 60 * 24;
        //const dayDiff = Math.floor((toDate - fromDate) / oneDayMs) + 1;
        //const maxHours = dayDiff * 24;
    const diffMs = toDate - fromDate;
    const totalAvailableHours = diffMs / (1000 * 60 * 60); // Convert ms to hours

        //if (enteredHours > maxHours) {
        if (enteredHours > totalAvailableHours) {
            document.getElementById("ContentPlaceHolder1_Task_taskDuration").value = 0;
            //alert(`Entered hours (${enteredHours}) exceed the maximum allowed hours (${maxHours}) for the selected date range.`);
            alert(`Entered hours (${enteredHours}) exceed the maximum allowed hours (${totalAvailableHours}) for the selected date range.`);
            return false;
        }

       return true; 
    } else {

        // If any input is missing, just skip validation (or you can alert if needed)
        // alert("Please fill in all fields before validation.");
    }
}

</script>
<script>
   
    function checkdurationcalculate(input)
    {
debugger
var invalue=document.getElementById(input);
        var Milestoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;

       

                  $.ajax({
                    url: 'Add_Task.aspx/BindcheckTaskdurationdetails',
                    data: "{ 'Milestoneid': '" + Milestoneid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function(response)
{
debugger
try {
                var Taskduraionchkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
var enterval=parseFloat(invalue.value==""?"0":invalue.value);
                if (Taskduraionchkdtls.find("Durationhourcal").text() != "")
                {

                    var Task_Duration = parseFloat(Taskduraionchkdtls.find("TaskDuration").text());
                    var Duration_hourcal = parseFloat(Taskduraionchkdtls.find("Durationhourcal").text());
                    if (Duration_hourcal > Task_Duration)
                    {
                        //alert('yes');
                        if (Task_Duration != 0)
                        {
                            if (enterval > Task_Duration) {
                                alert('Task hour can not greater milestone hour');
                                invalue.value = 0;
return false;
                            }
                            var chktsk = parseFloat(Task_Duration) + enterval;
                            //alert(chktsk);
                            if (chktsk > Duration_hourcal) {

                                alert('Milestone hour can not greater task hour');
                                invalue.value = 0;
return false;

                            }
                        }
                    }

                    if (Duration_hourcal < enterval)
                    {
                         alert('Task hour can not greater milestone hour');
                         invalue.value = 0;
return false;
                    }

                    if (Duration_hourcal == Task_Duration )
                    {
                        if (enterval > 0)
                        {
                            
                            alert('Task hour can not greater milestone hour');
                            invalue.value = 0;
return false;
                        }
                    }

                       
                  
                    
                    
                }
                   
            }
            catch

            {

            }

           

         
},
                    error: function (response) {
                     alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });

        function OnSuccessChktaskdurationlschk(response) {
            
            debugger
           // alert(response.d);
            try {
                var Taskduraionchkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
var enterval=parseFloat(input.value==""?"0":input.value);
                if (Taskduraionchkdtls.find("Durationhourcal").text() != "")
                {

                    var Task_Duration = parseFloat(Taskduraionchkdtls.find("TaskDuration").text());
                    var Duration_hourcal = parseFloat(Taskduraionchkdtls.find("Durationhourcal").text());
                    if (Duration_hourcal > Task_Duration)
                    {
                        //alert('yes');
                        if (Task_Duration != 0)
                        {
                            if (enterval > Task_Duration) {
                                alert('Task hour can not greater milestone hour');
                                input.value = 0;
                            }
                            var chktsk = parseFloat(Task_Duration) + enterval;
                            //alert(chktsk);
                            if (chktsk > Duration_hourcal) {

                                alert('Milestone hour can not greater task hour');
                                input.value = 0;

                            }
                        }
                    }

                    if (Duration_hourcal < enterval)
                    {
                         alert('Task hour can not greater milestone hour');
                         input.value = 0;

                    }

                    if (Duration_hourcal == Task_Duration )
                    {
                        if (enterval > 0)
                        {
                            
                            alert('Milestone hour can not greater task hour');
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
   
    function checkdurationcalculatenew(input)
    {
debugger
var invalue=document.getElementById(input);
        var Milestoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;

       

                  $.ajax({
                    url: 'Add_Task.aspx/BindcheckTaskdurationdetails',
                    data: "{ 'Milestoneid': '" + Milestoneid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function(response)
{
debugger
try {
                var Taskduraionchkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                var enterval=parseFloat(invalue.value==""?"0":invalue.value);
                if (Taskduraionchkdtls.find("Durationhourcal").text() != "")
                {

                    var Task_Duration = parseFloat(Taskduraionchkdtls.find("TaskDuration").text());
                    var Duration_hourcal = parseFloat(Taskduraionchkdtls.find("Durationhourcal").text());
                     if (Duration_hourcal < enterval)
                    {
                         alert('Task hour can not greater milestone hour');
                         invalue.value = 0;
                     return false;
                    }
                        
                    //var chktsk = parseFloat(Duration_hourcal) + enterval;
                   // if (chktsk > Duration_hourcal) {

                       // alert('Milestone hour can not greater task hour');
                        //invalue.value = 0;
                   // return false;

                   // }
                            
             }
              
            }
            catch

            {

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

       
    

 
</script>
