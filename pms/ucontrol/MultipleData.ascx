<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultipleData.ascx.cs" Inherits="ucontrol_MultipleData" %>
 <%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>


<style>
    .custom-popup {
  position: absolute;
  background: #fff;
  border: 1px solid #ccc;
  z-index: 9999;
  padding: 5px 10px;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0,0,0,0.2);
  font-size: 14px;
  transition: opacity 0.2s ease;
}

.custom-popup ul {
  margin: 0;
  padding: 0;
  list-style: none;
}

.custom-popup ul li {
  cursor: pointer;
  padding: 5px 10px;
}

.custom-popup ul li:hover {
  background-color: #f0f0f0;
}

</style>
<script>

</script>
<style>
td.details-control {
    background: url('/assets/images/open.png') no-repeat center right;
    cursor: pointer;
}
td.shown {
    background: url('/assets/images/close.png') no-repeat center right;
}
.datatable tr th:first-child, .datatable tr td:first-child {display:none;}
    </style>



                   <%--     <div class="card-header" style="display:none;">
                            <div class="row">
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addmile"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add Milestone"><i class="ri-add-line"></i></span></a>
                                       
                                    </div>
                                    </div>
                                </div>                                
                            </div> 
                        </div>--%>

                                      <div class="card-header">
                                      <div class="row">
                                     <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                   
                                      
                                        <a href="javascript:void(0);" id="add1" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addmile"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add Milestone"><i class="ri-add-line"></i></span></a>

                                           
                                       

                                        
                                           

                                        <button  type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">

                                          <a style="display:none;" class="dropdown-item toggle-vis" runat="server"  id="multi0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>
                                         <a class="dropdown-item toggle-vis" runat="server"  id="multi3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Weightage", "2")%></a>
                                           <a class="dropdown-item toggle-vis" runat="server"  id="multi5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Extended Timeline", "2")%></a>
                                           <a class="dropdown-item toggle-vis" runat="server"  id="multi9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Start Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi10" data-column="10"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("End Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issues", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task Duration (Hour)", "2")%></a>
                                             <a class="dropdown-item toggle-vis" runat="server"  id="multi15" data-column="15"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Stage", "2")%></a>

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








<%--Task add--%>
<div class="offcanvas offcanvas-end" id="addtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
             <label id="lbltitle" runat="server"><%=fns.getltext("Add Task", "2")%></label>   

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Milestoneid" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
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
                                       <label class="form-label flex-grow-1"><%=fns.getltext("Task Name", "2")%></label>
                                    </div>
                                    <input type="text" id="taskname" runat="server" placeholder="Task Name" class="form-control">
                                </div>



                               <div class="col-xxl-12 col-lg-12" id="divmiletimeline" style="display:none;">
                              <asp:Label ID="lblmiletimeline" runat="server"></asp:Label>
                              </div>

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


                                    <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkTaskWeightage" runat="server"  />  Do You Want Save Weightage on Sub Task 
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

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Startdate" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate(this.id)">

                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                              <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;</label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="StartTime" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>




                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Targetdate" ValidationGroup="btnsave"
                                                                runat="server"  ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Targetdate" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;</label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Timeline" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                    <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour" onkeyup="checkdurationcalculate(this);" class="form-control">
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp;</label>
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
                                
                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="ChooseDate" runat="server" class="form-control">
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
                                    <input type="text" id="Time" runat="server" class="form-control">
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
                                   
                                   <%--  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>--%>
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                       <%--    </ContentTemplate>
                                        </asp:UpdatePanel>--%>


                                </div> 
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel13" runat="server">
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

<%--subtask add--%>
<div class="offcanvas offcanvas-end" id="addsubtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
             <label id="lbltitlesubtask" runat="server"><%=fns.getltext("Add Sub Task", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunctionsubtask()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunctionsubtask()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimgsubtask()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btnclssubtask" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
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
                                <asp:Button ID="Button1subtask" runat="server" Text="" OnClick="Button1subtask_Click" />
                                     <asp:Button ID="btnloadsubtask" runat="server" Text="" OnClick="btnloadsubtask_Click" />
                                 </div>

                                   <asp:HiddenField ID="hdnidsubtask" runat="server" Value="0" />
                                   <asp:HiddenField ID="hdiprojidsubtask" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnmilstoneidsubtask" runat="server" Value="0" />
                                  

                                   <div class="col-xxl-12 col-lg-12" id="divprojectsubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="ProjectIdsubtask" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavesubtask" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectIdsubtask" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ProjectIdsubtask_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectIdsubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divmilestonesubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneidsubtask" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="Milestoneidsubtask_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneidsubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>

                                 

                               <div class="col-xxl-12 col-lg-12" id="divtasksubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="task" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task ..." AllowSingleDeselect="true" AutoPostBack="true" onchange="GetWeitagebindDtlssubtask()" OnSelectedIndexChanged="task_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntask" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnweightagesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task Name", "2")%></label>
                                    </div>
                                    <input type="text" id="Subtaskname" runat="server" placeholder="Sub Task Name" class="form-control">
                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divtasktimelinesub" style="display:none;">
                               <asp:Label ID="lbltasktimelinesub" runat="server"></asp:Label>
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

                               
                                  <div class="col-xxl-12 col-lg-12" id="divweitagesubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightagesubtask" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchksubtask(this);" class="form-control">
                                </div>

                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="ownersubtask" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnownersubtask" runat="server" Value="0" />
                                      
                                </div>

                                 <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>&nbsp;&nbsp;
                                            
                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Startdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                   <input type="text" id="Startdatesubtask" runat="server" class="form-control" onchange="checkdatecomparisiontaskdatesubtask(this.id)">
                                               <asp:HiddenField ID="hdnStartdatesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="StartTime" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="StartTimesubtask" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnstarttimesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;
                                            
                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Targetdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input type="text" id="Targetdate" runat="server" class="form-control" onchange="checkcomparisiontaskstonedate()">--%>
                                          <input type="text" id="Targetdatesubtask" runat="server" class="form-control" onchange="checkdatecomparisiontaskdatesubtask(this.id)">
                                               <asp:HiddenField ID="hdnTargetdatesubtask" runat="server" Value="0" />
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
                                   
                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Timelinesubtask" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTimelinesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                    <input type="text" id="taskDurationsubtask" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour" onkeyup="checkdurationcalculatesubtask(this);" class="form-control">
                                </div>

                               

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="P_Statussubtask" InitialValue="0" ValidationGroup="btnsavesubtask" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadsubtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Statussubtask" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatussubtask()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewprojectstatussubtask" style="display:none">
                                          <input type="text" id="txtnewprojectstatussubtask" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColorsubtask" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Statussubtask" runat="server" Value="0" />
                                      

                                </div>




                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" 
                                                ControlToValidate="Prioritysubtask" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavesubtask" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadsubtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Prioritysubtask" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Priority ..." AllowSingleDeselect="true" onchange="AddNewprioritysubtask()">
                                         </cc1:DropDownListChosen>
                                              </ContentTemplate>
                                        </asp:UpdatePanel>
                                                 <div id="dvnewprioritysubtask" style="display:none">
                                          <input type="text" id="txtnewprioritysubtask" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolorsubtask" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPrioritysubtask" runat="server" Value="0" />
                                          

                                </div>


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budgetsubtask" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actualsubtask" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divremindersubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp;</label>
                                    </div>
                                    
                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Remindersubtask" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetReminderDtlssubtask()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Based on Due Date</asp:ListItem>
                                          <asp:ListItem  value="2">Specific Date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnremindersubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divremindertypesubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder Type", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ReminderTypesubtask" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetReminderDtlssubtask()">
                                         <asp:ListItem  value="1">Daily</asp:ListItem>
                                          <asp:ListItem  value="2">On due date</asp:ListItem>
                                          <asp:ListItem  value="3">Day(s) before due date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnReminderTypesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="col-xxl-12 col-lg-12"  id="divDayssubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Enter Day(s)", "2")%></label>
                                    </div>
                                    <input type="text" id="Dayssubtask" runat="server" placeholder="Days" class="form-control">
                                     </div>


                                <div class="col-xxl-12 col-lg-12" id="divChooseDatesubtask">
                                 <div class="d-flex align-items-center">
                                 <label class="form-label flex-grow-1"><%=fns.getltext("Choose Date", "2")%></label>
                                 </div>
                                
                                     <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="ChooseDatesubtask" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnChooseDatesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                    <div class="col-xxl-12 col-lg-12" id="Divtimesubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Time", "2")%></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel34" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Timesubtask" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTimesubtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                   <div class="col-xxl-12 col-lg-12"  id="divNotifyuserssubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Notify users", "2")%></label>
                                    </div>
                                
                                      <asp:DropDownList ID="Notifyuserssubtask" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Notify User ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnNotifyuserssubtask" runat="server" Value="0" />
                                     
                                    </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Occurrence", "2")%></label>
                                    </div>
                                    <input type="text" id="Occurrencesubtask" runat="server" placeholder="Occurrence"  class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Tag To", "2")%></label>
                                    </div>
                                    <input type="text" id="tagtosubtask" runat="server" placeholder="Tag To" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="teamsubtask" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteamsubtask" runat="server" Value="0" />
                                        


                                </div>  
                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1subtask" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadsubtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Stagesubtask" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Stage ..." AllowSingleDeselect="true" onchange="AddNewStagesubtask()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewStagesubtask" style="display:none">
                                          <input type="text" id="txtnewstagesubtask" class="form-control" placeholder="Stage"  runat="server" />
                                                  <input  data-jscolor="{}" id="txtstageprintcolorsubtask" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnstagesubtask" runat="server" Value="0" />
                                        

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


      <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
         <div id="pnlimgsubtask" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img1" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsubsubtask" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsavesubtask" onclick="SvaeDatasubtask('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmoresubtask" onclick="SvaeDatasubtask('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimgsubtask()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclosesubtask" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>


</div>

<%--milestone add--%>
<div class="offcanvas offcanvas-end" id="addmile" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
             <label id="lbltitlemile" runat="server"><%=fns.getltext("Add Milestone", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimgmile()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btnclsmile" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
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
                                <asp:Button ID="Button1mile" runat="server" Text="" OnClick="Button1mile_Click" />
                                     <asp:Button ID="btnloadmile" runat="server" Text="" OnClick="btnloadmile_Click" />
                                 </div>

                                    <asp:HiddenField ID="hdnidmile" runat="server" Value="0" />
                                <asp:HiddenField ID="hdiprojidmile" runat="server" Value="0" />
                                <div class="col-xxl-12 col-lg-12" id="divprojectmile">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="ProjectIdmile" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>
                                 
                                    <%--onchange="GetWeitagebindDtls()"--%>
                                      <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                              
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectIdmile" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" onchange="GetWeitagebindDtlsmile()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectIdmile" runat="server" Value="0" />
                                              <asp:HiddenField ID="hdnweightagemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Milestone" ValidationGroup="btnsavemile"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Milestone" placeholder="Milestone" runat="server"  class="form-control">
                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divprojecttimeline" style="display:none;">
                                 <asp:Label ID="projecttimeline" runat="server"></asp:Label>
                                 </div>



                                 <div class="col-xxl-12 col-lg-12" id="divmiletimeline_mile" style="display:none;">
                                  <asp:Label ID="lblmiletimeline_mile" runat="server"></asp:Label>
                                  </div>



                                  <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="istask" runat="server" />Has Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkmileWeightage" runat="server"  />  Do You Want Same Weightage on Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                              


                               
                                <div class="col-xxl-12 col-lg-12" id="divweitagemile">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightagemile" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchkmile(this);" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                       <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input type="text" id="StartDate" runat="server" class="form-control">checkcomparisionmilestonedate()--%>
                                      <input type="text" id="StartDatemile" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate_mile(this.id)">
                                         <asp:HiddenField ID="hdnStartDatemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Date", "2")%>

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="EndDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="EndDatemile" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate_mile(this.id)">
                                         <%--<input type="text" id="EndDate" runat="server" class="form-control">--%>
                                             <asp:HiddenField ID="hdnEndDatemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>

                                      <asp:DropDownList ID="Ownermile" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnownermile" runat="server" Value="0" /> 
  

                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="teammile" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnteammile" runat="server" Value="0" />
                                      


                                </div>


                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator10" 
                                                ControlToValidate="P_Statusmile" InitialValue="0" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadmile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Statusmile" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatusmile()">
                                                </cc1:DropDownListChosen>
                                              <div id="dvnewprojectstatusmile" style="display:none">
                                          <input type="text" id="txtnewprojectstatusmile" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColormile" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Statusmile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator11" 
                                                ControlToValidate="Prioritymile" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadmile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Prioritymile" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewprioritymile()">
                                         </cc1:DropDownListChosen>
                                                 <div id="dvnewprioritymile" style="display:none">
                                          <input type="text" id="txtnewprioritymile" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolormile" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPrioritymile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budgetmile" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actualmile" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>



                                <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Access", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Access" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Accessmile" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Staff</asp:ListItem>
                                          <asp:ListItem  value="2">Client</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnAccessmile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>    
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Duration Type", "2")%>  &nbsp;&nbsp;
                                            
                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ddldurationtype" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Task Duration ..." AllowSingleDeselect="true" onchange="GetDurationchange()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                         <asp:ListItem  value="1">Hours</asp:ListItem>
                                          <asp:ListItem  value="2">Days</asp:ListItem>
                                          <asp:ListItem  value="3">Month</asp:ListItem>
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdndurationtype" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                 <div class="col-xxl-12 col-lg-12" id="divchngtype" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration", "2")%> <span id="chngtype"></span></label>
                                    </div>
                                    <input type="text" id="taskDurationmile" runat="server" onkeypress="return isnumeric(event)"  class="form-control">
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


      <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
         <div id="pnlimgmile" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img3" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsubmile" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsavemile" onclick="SvaeDatamile('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmoremile" onclick="SvaeDatamile('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimgmile()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclosemile" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
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


<div id="milechangeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" id="milebtnclsowner" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="milehdnchangeid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          
                               <asp:DropDownList ID="mileddlchangeowner" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="milehdncahngeowner" runat="server" Value="0" />
                                             <asp:HiddenField ID="milehdnchnownrghtml" runat="server" Value="0" />




                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="milebtnsaveowner" class="btn btn-primary" onclick="mileSvaeDataowner('milesaveowner');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="sbtskchangeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" id="sbtskbtnclsowner" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="sbtskhdnchangeid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                           

                               <asp:DropDownList ID="sbtskddlchangeowner" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="sbtskhdncahngeowner" runat="server" Value="0" />
                                             <asp:HiddenField ID="sbtskhdnchnownrghtml" runat="server" Value="0" />




                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="sbtskbtnsaveowner" class="btn btn-primary" onclick="sbtskSvaeDataowner('sbtsksaveowner');">Save</button>
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

<div id="mileaddteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" id="milebtnclsTeam" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                         <asp:HiddenField ID="milehdnteameid" runat="server" Value="0" />
                        <div class="col-sm-12 mb-1">
                        
                             <asp:DropDownList ID="mileddladdteam" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="milehdnaddteam" runat="server" Value="0" />
                                            <asp:HiddenField ID="milehdnaddteamhtml" runat="server" Value="0" />

                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="milebtnsaveteam" class="btn btn-primary" onclick="mileSvaeDatateam('milesaveteam');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="sbtskaddteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" id="sbtskbtnclsTeam" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                         <asp:HiddenField ID="sbtskhdnteameid" runat="server" Value="0" />
                        <div class="col-sm-12 mb-1">
                         

                             <asp:DropDownList ID="sbtskddladdteam" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="sbtskhdnaddteam" runat="server" Value="0" />
                                             <asp:HiddenField ID="sbtskhdnaddteamhtml" runat="server" Value="0" />
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="sbtskbtnsaveteam" class="btn btn-primary" onclick="sbtskSvaeDatateam('sbtsksaveteam');">Save</button>
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
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button4" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendatestrt_task" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnextndstrt_task" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button4" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendateend_task" runat="server" class="form-control">
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

<div id="MileExtendedTimline" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="milebtnclsextended" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button2" runat="server" Text="" OnClick="Button2_Click" />
   </div>
                      <asp:HiddenField ID="milehdnextendid" runat="server" Value="0" />
                          <asp:HiddenField ID="milehdnextnhtml" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="mileExtendatestrt" runat="server" class="form-control">
                                               <asp:HiddenField ID="milehdnextndstrt" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="mileExtendateend" runat="server" class="form-control">
                                              <asp:HiddenField ID="milehdnextndend" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="milebtnsaveextended" class="btn btn-primary" onclick="mileSvaeExtended('milesaveExtended');">Save</button>
            </div>
        </div>
    </div>
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
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendatestrt_subtask" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnextndstrt_subtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendateend_subtask" runat="server" class="form-control">
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

<script>
    function fsFunctionsubtask(){
        var element = document.getElementById("addsubtask");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunctionsubtask(){
        var element = document.getElementById("addsubtask");
        element.classList.remove("canvas-fullscreen");
    }
</script>


<script>
    function fsFunctionmile(){
        var element = document.getElementById("addmile");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunctionmile(){
        var element = document.getElementById("addmile");
        element.classList.remove("canvas-fullscreen");
    }
</script>

    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
    <%--<script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
  <script src="/pms/assets/js/jscolor.js"></script>
<script src="/pms/assets/js/jquery.dataTables.min.js"></script>

<%-- <script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>--%>

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

<%--<script type="text/javascript" src="https://cdn.jsdelivr.net/momentjs/latest/moment.min.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
<link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />--%>
<%--<script>
$(function() {
  $('#daterange').daterangepicker({
    opens: 'center'
  });
});
</script>--%>


                           

<asp:HiddenField ID="hdntype" runat="server" />

<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
        document.getElementById("divproject").style.display = "none";
        document.getElementById("divprojectmile").style.display = "none";
        document.getElementById("divprojectsubtask").style.display = "none";

          var RecordId=document.getElementById("<%=ProjectIdmile.ClientID%>").value;
         
         $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_mile',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise_mil,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });




              function OnSuccessweitagedtlschkpagewise_mil(response) {
            
            debugger
            try {
                var waitagechkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtls.find("checkprojectWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkprojectWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "block";

                     }
                    
                }

            }
            catch

            {

            }


         //Owner Bind Project Wise;
         debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
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

                                                         var ddLOwnermile = $(document.getElementById("<%=Ownermile.ClientID%>"));
                                                         $(ddLOwnermile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=Ownermile.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=Ownermile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnownermile.ClientID %>").value = ids;
                                                        document.getElementById("<%=Ownermile.ClientID%>").click();

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





         //Team Bind Project Wise;
         debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
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

                                                         var ddLteammile = $(document.getElementById("<%=teammile.ClientID%>"));
                                                         $(ddLteammile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=teammile.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=teammile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteammile.ClientID %>").value = ids;
                                                        document.getElementById("<%=teammile.ClientID%>").click();

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



        // project tomeline //
           debugger
            document.getElementById("divprojecttimeline").style.display = "block";
            var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
             debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindProjecttimelinedata',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessproject_timeline_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        
        function OnSuccessproject_timeline_mile(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=projecttimeline.ClientID%>");
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }





       //  Milestone Timeline //
              debugger
              document.getElementById("divmiletimeline_mile").style.display = "block";
              var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
              debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata_mile',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccessgetdetails_miletimelinedata_mile(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                   // lbl.innerHTML = '';                    
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
            url: 'MultiProjectTask.aspx/bindMultiOwner_new',
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
            url: 'MultiProjectTask.aspx/bindMultiTeam_new',
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
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails',
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
            
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>


<script>
   
    function checkdurationcalculate(input)
    {

        var Milestoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;

       

                  $.ajax({
                    url: 'MultiProjectTask.aspx/BindcheckTaskdurationdetails',
                    data: "{ 'Milestoneid': '" + Milestoneid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessChktaskdurationlschk,
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
                if (Taskduraionchkdtls.find("Durationhourcal").text() != "")
                {

                    var Task_Duration = Taskduraionchkdtls.find("TaskDuration").text();
                    var Duration_hourcal = Taskduraionchkdtls.find("Durationhourcal").text();

                  
                    //alert(Task_Duration);
                    //alert(Duration_hourcal);
                    //alert(input.value);

                    if (Duration_hourcal > Task_Duration)
                    {
                        //alert('yes');
                        if (Task_Duration != 0)
                        {
                            if (parseInt(input.value) > Task_Duration) {

                                alert('Milestone hour can not greater task hour');
                                input.value = 0;

                            }

                            var chktsk = parseInt(Task_Duration) + parseInt(input.value);
                            //alert(chktsk);
                            if (chktsk > Duration_hourcal) {

                                alert('Milestone hour can not greater task hour');
                                input.value = 0;

                            }
                        }
                    }

                    if (Duration_hourcal < parseInt(input.value))
                    {
                         alert('Milestone hour can not greater task hour');
                         input.value = 0;

                    }

                    if (Duration_hourcal == Task_Duration )
                    {
                        if (parseInt(input.value) > 0)
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
                    url: 'MultiProjectTask.aspx/Bindcheckstrtdatemiledetails',
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
                            url: "MultiProjectTask.aspx/statussave",
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
                            url: "MultiProjectTask.aspx/AddPrioritytype",
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
                            url: "MultiProjectTask.aspx/AddStage",
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
                            url: 'MultiProjectTask.aspx/SaveTask',
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
                     document.getElementById('<%=btnload.ClientID%>').click();
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid]").val("0");
                    $("[id*=tasklist]").val("0");
                    $("[id*=taskname]").val("");
                    $("[id*=taskdescripton]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                    $("[id*=Targetdate]").val("");
                    $("[id*=StartTime]").val("");
                    $("[id*=Timeline]").val("");
                     $("[id*=taskDuration]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    //$("[id*=Weightage]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("");

                    $("[id*=ReminderType]").val("");
                    $("[id*=Days]").val("");
                    $("[id*=Time]").val("");
                    $("[id*=Notifyusers]").val("");
                    $("[id*=ChooseDate]").val("");

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
                        
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid]").val("0");
                   $("[id*=tasklist]").val("0");
                    $("[id*=taskname]").val("");
                    $("[id*=taskdescripton]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                  $("[id*=Targetdate]").val("");

                 $("[id*=StartTime]").val("");
                 $("[id*=Timeline]").val("");
                 $("[id*=taskDuration]").val("");


                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                   // $("[id*=Weightage]").val("");
                    $("[id*=tagto]").val("");
                   $("[id*=team]").val("");

                   $("[id*=ReminderType]").val("");
                    $("[id*=Days]").val("");
                    $("[id*=Time]").val("");
                    $("[id*=Notifyusers]").val("");
                    $("[id*=ChooseDate]").val("");

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

               //alert(RecordId);
                debugger;
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindtaskdetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1dataupdate,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


        function OnSuccess1dataupdate(response) {
          //  alert(response.d);
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
                    

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Bindtaskdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist.ClientID%>').value = Bindtaskdetail.find("tasklist").text();
                    document.getElementById("divproject").style.display = "none";
                    document.getElementById("divmilestone").style.display = "none";
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


                     //  Milestone Timeline //
              debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata',
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
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }



             //  Task Timeline //
             debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
        
                debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata',
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
                   // lbl.innerHTML = '';                    
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

       


<asp:HiddenField ID="hdnbtns" runat="server" />
    <script>
        
        $(document).ready(function () {
            
            Bindtask_data();
           
           
});
         function Bindtask_data() {
            
             debugger;
            // alert($("[id*=hdiprojid]").val());
            
                  $.ajax({
                    url: 'MultiProjectTask.aspx/bindtaskdata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
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
                   // lbl.innerHTML = '';                    
            lbl.innerHTML = todolist;
      
                table = new DataTable('.datatable', {
                    scrollX: true,
                    scrollY: 300,
                    scrollCollapse: true,
                    paging: false,
                    searching: true,

        //      fixedColumns: {
        //    leftColumns: 2
        //}

                });
      
         <%=setmilstone%>


              <%-- debugger
                 var RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
                 if (RecordId == "" || RecordId == "0")
                 {
                 RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
                 }
               
                   $.ajax({
                    url: 'MultiProjectTask.aspx/bindtaskdataP',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssetmilstonehideshow_data_bind,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

     

     function OnSuccesssetmilstonehideshow_data_bind(response)
     {
           // alert(response.d);

            $(function() {$('#showtrM_8').on('click', function() { $('.taskshow_8').toggle();  });}); $('#showtrM_8').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_27').on('click', function() {$('.subtaskshow_27').toggle();  });}); $('#showtasktr_27').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_28').on('click', function() {$('.subtaskshow_28').toggle();  });}); $('#showtasktr_28').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtrM_7037').on('click', function() { $('.taskshow_7037').toggle();  });}); $('#showtrM_7037').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_2052').on('click', function() {$('.subtaskshow_2052').toggle();  });}); $('#showtasktr_2052').click(function() { $(this).toggleClass('shown');});

       
         


    }--%>



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
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata',
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
                   // lbl.innerHTML = '';                    
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
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata',
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
                   // lbl.innerHTML = '';                    
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
                    url: 'MultiProjectTask.aspx/Deletetaskdata',
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
                    url: 'MultiProjectTask.aspx/Savetask_Owner',
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
                    url: 'MultiProjectTask.aspx/Bindtaskdetail_Owner',
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
                    url: 'MultiProjectTask.aspx/Save_Team',
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
                    url: 'MultiProjectTask.aspx/Bindetail_Team',
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
                    //alert(iscomplete);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/Update_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'statusid': '" + statusid + "' ,'iscomplete': '" + iscomplete + "' }",
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
            Bindtask_data();
            var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
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
                    url: 'MultiProjectTask.aspx/UpdatePriority_Status',
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
                    url: 'MultiProjectTask.aspx/UpdateExtendeddatedtls_task',
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
                    url: 'MultiProjectTask.aspx/Bindtaskdetail_extendatedate',
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
                    url: 'MultiProjectTask.aspx/Update_Stagge',
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



    
<asp:HiddenField ID="hdnprojecttaskid" runat="server" Value="0" />


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
   
  
    </script>--%>

  <%--<script>
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
    <%--debugger
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
            url: 'MultiProjectTask.aspx/bindMultiChangeOwner_new',
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
    <%--debugger
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
            url: 'MultiProjectTask.aspx/bindMultiChangeTeam_new',
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





    <%--Multi dropdown select start end--%>   




 <%--Milestone Data show--%>   

<%--Milestone Owner details bind and update data start --%>
<asp:HiddenField ID="multichngprojectidmile" runat="server" Value="0" />
<script>
  

   <%--  debugger
    var mileddlchangeowner = $(document.getElementById("<%=mileddlchangeowner.ClientID%>"));
   

    $(mileddlchangeowner).select2({
        data: [
    <%=mileMultichangeowner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        dropdownParent: $("#milechangeowner"),
        width: "100%"
    });

   
     $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val();
        document.getElementById("<%=milehdncahngeowner.ClientID %>").value = ids;
        document.getElementById("<%=mileddlchangeowner.ClientID%>").click();

    });--%>


    function MultichangeownerMile() {
        debugger
        var projectid = document.getElementById("<%=multichngprojectidmile.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeOwnerMile_new',
            data: "{'projectid': '" + projectid + "'}",
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

                                                         var mileddlchangeowner = $(document.getElementById("<%=mileddlchangeowner.ClientID%>"));
                                                         $(mileddlchangeowner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             dropdownParent: $("#milechangeowner"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val();
                                                        document.getElementById("<%=milehdncahngeowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=mileddlchangeowner.ClientID%>").click();

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
<script type="text/javascript">
             
        function mileSvaeDataowner(val12A) {

            if (Page_ClientValidate("milebtnsaveowner")) {
               
                debugger


                // save all details
                
                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/SaveMilestone_Owner',
                    data: "{ 'mileid': '" + document.getElementById('<%=milehdnchangeid.ClientID%>').value + "' ,'mileownerdropdownid': '" + document.getElementById('<%=milehdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessowner_mile,
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
                function OnSuccessowner_mile(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=mileddlchangeowner]").val("");
                   
                     document.getElementById('<%=milehdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change();
                    document.getElementById("milebtnclsowner").click();
                   // Bindmilestone_data();
                     document.getElementById("milelblowner_" + document.getElementById("<%=milehdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function mile_Binddetails_Owner(RecordId,lblownerid) {
              
              debugger;
              document.getElementById("<%=milehdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/BindpMilestonedetail_Owner',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1owner_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1owner_mile(response) {
            
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=milehdnchangeid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=milehdncahngeowner.ClientID%>').value = milestonedetails.find("Owner").text();

             document.getElementById('<%=multichngprojectidmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
             MultichangeownerMile();
              if (milestonedetails.find("Owner").text() != "")
              {
                  debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
                $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change();
                  }
             
            }

             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    
<%--Milestone Owner details bind and update data end  --%>


<%--Milestone Team details bind and update data start --%>
<script>
   <%-- debugger
    var mileddLAddteam = $(document.getElementById("<%=mileddladdteam.ClientID%>"));
   

    $(mileddLAddteam).select2({
        data: [
    <%=mileMultiaddteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        dropdownParent: $("#mileaddteam"),
        width: "100%"
    });

   
     $(document.getElementById("<%=mileddladdteam.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=mileddladdteam.ClientID%>")).val();
        document.getElementById("<%=milehdnaddteam.ClientID %>").value = ids;
        document.getElementById("<%=mileddladdteam.ClientID%>").click();

    });--%>



     function MultichangeTeamMile() {
        debugger
        var projectid = document.getElementById("<%=multichngprojectidmile.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeTeamMile_new',
            data: "{'projectid': '" + projectid + "'}",
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

                                                         var mileddLAddteam = $(document.getElementById("<%=mileddladdteam.ClientID%>"));
                                                         $(mileddLAddteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             dropdownParent: $("#mileaddteam"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=mileddladdteam.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=mileddladdteam.ClientID%>")).val();
                                                        document.getElementById("<%=milehdnaddteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=mileddladdteam.ClientID%>").click();

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
<script type="text/javascript">
             
        function mileSvaeDatateam(val13A) {

            if (Page_ClientValidate("milebtnsaveteam")) {
               
                debugger


                // save all details
                
                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/SaveMileProject_Team',
                    data: "{ 'mileid': '" + document.getElementById('<%=milehdnteameid.ClientID%>').value + "' ,'mileTeamdropdownid': '" + document.getElementById('<%=milehdnaddteam.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTeam_mile,
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
                function OnSuccessTeam_mile(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=mileddladdteam]").val("");
                   
                     document.getElementById('<%=milehdnaddteam.ClientID%>').value = "";
                     $(document.getElementById("<%=mileddladdteam.ClientID%>")).val(0);
                    $(document.getElementById("<%=mileddladdteam.ClientID%>")).change();
                    document.getElementById("milebtnclsTeam").click();
                  //  Bindmilestone_data();
                   document.getElementById("milelbluser_" + document.getElementById("<%=milehdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                    
                }

        

          function mileBinddetails_Team(RecordId,lbluserid) {
              
              debugger;
              //alert(RecordId);
              //alert(lbluserid);
               document.getElementById("<%=milehdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindmilestonedetail_Team',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1team_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1team_mile(response) {
            
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=milehdnteameid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=milehdnaddteam.ClientID%>').value = milestonedetails.find("team").text();
                    
              document.getElementById('<%=multichngprojectidmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
              MultichangeTeamMile();
              if (milestonedetails.find("team").text() != "")
              {
                  debugger
                  if (milestonedetails.find("team").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("team").text().split(',');
                      $(document.getElementById("<%=mileddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=mileddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("team").text();
                $(document.getElementById("<%=mileddladdteam.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=mileddladdteam.ClientID%>")).change();
                  }
             
            }

             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>

<%--Milestone Team details bind and update data end --%>



<%--Milestone  status update start--%>
    <asp:HiddenField ID="hdnmilstntid" runat="server" Value="0" />
 <asp:HiddenField ID="milehdnbtnstatusid" runat="server" Value="0" />

    <script type="text/javascript">
             
        function MileUpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
            var milstoneid=document.getElementById("<%=hdnmilstntid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/UpdateMilestone_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'milstoneid': '" + milstoneid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdt_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStatusupdt_mile(response) {
            alert('Data Saved successfully.');
            Bindtask_data();
             var btnid = document.getElementById("<%=milehdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setmilsestoneid(milstoneid,btnststusid)
        {
            debugger
            document.getElementById("<%=milehdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdnmilstntid.ClientID%>").value = milstoneid;
        }
            </script>

 <%-- Milestone  status update end --%>


   <%--Milestone property update start--%>

      <asp:HiddenField ID="milehdnpropertID" runat="server" Value="0" />
      <asp:HiddenField ID="milehdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function MileUpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var milstoneid=document.getElementById("<%=milehdnpropertID.ClientID%>").value;
                   $.ajax({
                    url: 'MultiProjectTask.aspx/MileUpdatePriority_Status',
                    data: "{ 'milstoneid': '" + milstoneid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdt_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdt_mile(response) {
            alert('Data Saved successfully.');
           // Bindmilestone_data();
            var btnid = document.getElementById("<%=milehdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function milesetpriorityid(milston_id,btnpriortyid)
        {
            debugger
             document.getElementById("<%=milehdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=milehdnpropertID.ClientID%>").value = milston_id;
        }
            </script>

    <%--Milestone proprety update end--%>


 <%--Milestone Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function mileSvaeExtended(val125A) {

            if (Page_ClientValidate("milebtnsaveextended")) {
               
                debugger
                
                // save all details


                var Milestoneid = document.getElementById('<%=milehdnextendid.ClientID%>').value;
                var mileextendStrtdate = document.getElementById('<%=mileExtendatestrt.ClientID%>').value;
                var mileextendEnddate = document.getElementById('<%=mileExtendateend.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/MileUpdateExtendeddatedtls',
                    data: "{ 'Milestoneid': '" + Milestoneid + "' ,'mileextendStrtdate': '" + mileextendStrtdate + "','mileextendEnddate': '" + mileextendEnddate + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessUpdateexdate_mile,
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
                function OnSuccessUpdateexdate_mile(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=milehdnextndstrt.ClientID%>').value = "";
                    document.getElementById('<%=milehdnextendid.ClientID%>').value = "";
                    document.getElementById("milebtnclsextended").click();
                    debugger
                    document.getElementById("milelblExtnddt_" + document.getElementById("<%=milehdnextnhtml.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function MileBindextenddate_Details(RecordId,lblExtndid) {
              
              debugger;

              document.getElementById("<%=milehdnextnhtml.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindprojectdetail_Mileextendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate_mile(response) {
            // alert(response.d);
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=milehdnextendid.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=milehdnextndstrt.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=milehdnextndend.ClientID%>').value = milestonedetails.find("EndDate").text();
                     document.getElementById('<%=Button2.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }
         

            </script>
    
      <%--Milestone Extended date details bind and update data end  --%>


  <%--Project status update start--%>
    <asp:HiddenField ID="proj_hdnprojectid" runat="server" Value="0" />
    <asp:HiddenField ID="proj_hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function ProjUpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
            var projectid=document.getElementById("<%=proj_hdnprojectid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/UpdateProject_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'projectid': '" + projectid + "' ,'statusid': '" + statusid + "' ,'iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdt_project,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStatusupdt_project(response) {
            debugger
            alert('Data Saved successfully.');
            var btnid = document.getElementById("<%=proj_hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
            //btnid.innerHTML = response.d;
            Bindtask_data();
        }
        
        function projectsetprojectid(projectid,btnststusid)
        {
            debugger
            //var id = btnststusid;
            document.getElementById("<%=proj_hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=proj_hdnprojectid.ClientID%>").value = projectid;
        }
            </script>

 <%--   status update end --%>


   <%--Project property update start--%>

      <asp:HiddenField ID="proj_hdnpropertID" runat="server" Value="0" />
      <asp:HiddenField ID="proj_hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function ProjUpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var projectid=document.getElementById("<%=proj_hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/projectUpdatePriority_Status',
                    data: "{ 'projectid': '" + projectid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdt_project,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdt_project(response) {
            alert('Data Saved successfully.');
            var btnid = document.getElementById("<%=proj_hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
           // Bindproject_data();
        }
        
        function projectsetpriorityid(projectid,btnpriortyid)
        {
            debugger
            document.getElementById("<%=proj_hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=proj_hdnpropertID.ClientID%>").value = projectid;
        }
            </script>

    <%--Project proprety update end--%>



 <%--Multi sub task details start--%>

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
                    url: 'MultiProjectTask.aspx/UpdateExtendeddatedtls_subtask',
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
                    url: 'MultiProjectTask.aspx/Bindsubtaskdetail_extendatedate',
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



 <%--status and propery  update start--%>
    <asp:HiddenField ID="hdsubtaskid" runat="server" Value="0" />
 <asp:HiddenField ID="sbtskhdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function sbtskUpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
                   var subtaskid=document.getElementById("<%=hdsubtaskid.ClientID%>").value;
                     //alert(statusid);
                    //alert(iscomplete);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskUpdate_Status',
                    data: "{ 'subtaskid': '" + subtaskid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdtsbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStatusupdtsbtsk(response) {
            alert('Data Saved successfully.');
            Bindtask_data();
            var btnid = document.getElementById("<%=sbtskhdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setsubtaskstustid(subtaskid,btnststusid)
        {
            debugger
            document.getElementById("<%=sbtskhdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdsubtaskid.ClientID%>").value = subtaskid;
        }
            </script>


      <asp:HiddenField ID="sbtskhdnpropertID" runat="server" Value="0" />
<asp:HiddenField ID="sbtskhdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function sbtskUpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var taskid=document.getElementById("<%=sbtskhdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskUpdatePriority_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdtsbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdtsbtsk(response) {
            alert('Data Saved successfully.');
           // BindSubtask_data();
            var btnid = document.getElementById("<%=sbtskhdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function sbtsksetpriorityid(tasklistid,btnpriortyid)
        {
            debugger
            document.getElementById("<%=sbtskhdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=sbtskhdnpropertID.ClientID%>").value = tasklistid;
        }
            </script>

    <%--status and  proprety update end--%>
    

<%--All stage update start--%>

 <asp:HiddenField ID="sbtskhdntaskstageid" runat="server" Value="0" />
 <asp:HiddenField ID="sbtskhdnbtn_stageid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function sbtskUpdateDataStagedeatils(Stageid)
        {
            debugger
                   var sbtsktaskid=document.getElementById("<%=sbtskhdntaskstageid.ClientID%>").value;
                    //alert(taskid);
                    //alert(Stageid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskUpdate_Stagge',
                    data: "{ 'taskid': '" + sbtsktaskid + "' ,'Stageid': '" + Stageid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStageupdtsbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStageupdtsbtsk(response) {
            alert('Data Saved successfully.');
           // Bindtask_data();
            var btnstgid = document.getElementById("<%=sbtskhdnbtn_stageid.ClientID%>").value; 
            var arrstgcol = response.d.split("~~");
            document.getElementById(btnstgid).innerHTML = arrstgcol[0];
            document.getElementById(btnstgid).style.backgroundColor = arrstgcol[1];
        }
        
        function sbtsksetStageid(sbtsktaskid,btnStageid)
        {
            debugger
             document.getElementById("<%=sbtskhdnbtn_stageid.ClientID%>").value = btnStageid;
            document.getElementById("<%=sbtskhdntaskstageid.ClientID%>").value = sbtsktaskid;
        }
            </script>

<%--All stage update end--%>



 <%--Owner details bind and update data start --%>

<asp:HiddenField ID="multichngtaskid" runat="server" Value="0" />
<script>
   <%-- debugger
    var sbtskddLchangeowner = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>"));
   

    $(sbtskddlchangeowner).select2({
        data: [
    <%=sbtskMultichangeowner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        dropdownParent: $("#sbtskchangeowner"),
        width: "100%"
    });

   
     $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val();
        document.getElementById("<%=sbtskhdncahngeowner.ClientID %>").value = ids;
        document.getElementById("<%=sbtskddlchangeowner.ClientID%>").click();

    });--%>


      function Multichangeownersubtask() {
        debugger
        var tsk_id = document.getElementById("<%=multichngtaskid.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeOwnersubtask_new',
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

                                                         var sbtskddlchangeowner = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>"));
                                                         $(sbtskddlchangeowner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             dropdownParent: $("#sbtskchangeowner"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val();
                                                        document.getElementById("<%=sbtskhdncahngeowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=sbtskddlchangeowner.ClientID%>").click();

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
<script type="text/javascript">
             
        function sbtskSvaeDataowner(val12A) {

            if (Page_ClientValidate("sbtskbtnsaveowner")) {
               
                debugger


                // save all details


                    $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/sbtskSave_Owner',
                   // data: '{Bindsubtaskdetail: ' + JSON.stringify(Bindsubtaskdetail) + '}',
                    data: "{ 'id': '" + document.getElementById('<%=sbtskhdnchangeid.ClientID%>').value + "' ,'ownerdropdownid': '" + document.getElementById('<%=sbtskhdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessownerchangesbtsk,
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
                function OnSuccessownerchangesbtsk(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=sbtskddlchangeowner]").val("");
                   
                     document.getElementById('<%=sbtskhdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();
                    document.getElementById("sbtskbtnclsowner").click();
                    document.getElementById("sbtsklblowner_" + document.getElementById("<%=sbtskhdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function sbtskBinddetails_Owner(RecordId,sbtsklblownerid) {
              
               debugger;
               document.getElementById("<%=sbtskhdnchnownrghtml.ClientID%>").value = sbtsklblownerid.split('_')[1];
             
                $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskBinddetail_Owner',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1ownersbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1ownersbtsk(response) {
            
            debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "")
                {
                    document.getElementById('<%=sbtskhdnchangeid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    document.getElementById('<%=sbtskhdncahngeowner.ClientID%>').value = Bindsubtaskdetail.find("owner").text();

             <%--  if (Bindsubtaskdetail.find("owner").text() != "")
             {
               var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
               $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();
            }--%>


              document.getElementById('<%=multichngtaskid.ClientID%>').value = Bindsubtaskdetail.find("task").text();
              Multichangeownersubtask();
              if (Bindsubtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text();
                $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();
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

<script>
   <%-- debugger
    var sbtskddLAddteam = $(document.getElementById("<%=sbtskddladdteam.ClientID%>"));
   

    $(sbtskddLAddteam).select2({
        data: [
    <%=sbtskMultiaddteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        dropdownParent: $("#sbtskaddteam"),
        width: "100%"
    });

   
     $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val();
        document.getElementById("<%=sbtskhdnaddteam.ClientID %>").value = ids;
        document.getElementById("<%=sbtskddladdteam.ClientID%>").click();

    });--%>



     function sbtskMultiaddteam() {
        debugger
        var tsk_id = document.getElementById("<%=multichngtaskid.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangesubtaskTeam_new',
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

                                                         var sbtskddladdteam = $(document.getElementById("<%=sbtskddladdteam.ClientID%>"));
                                                         $(sbtskddladdteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             dropdownParent: $("#sbtskaddteam"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val();
                                                        document.getElementById("<%=sbtskhdnaddteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=sbtskddladdteam.ClientID%>").click();

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


    <script type="text/javascript">
             
        function sbtskSvaeDatateam(val13A) {

            if (Page_ClientValidate("sbtskbtnsaveteam")) {
               
                debugger


                // save all details
                

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/sbtskSave_Team',
                    data: "{ 'id': '" + document.getElementById('<%=sbtskhdnteameid.ClientID%>').value + "' ,'teamdropdownid': '" + document.getElementById('<%=sbtskhdnaddteam.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTeamsbtsk,
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
                function OnSuccessTeamsbtsk(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    $("[id*=sbtskddladdteam]").val("");
                   
                     document.getElementById('<%=sbtskhdnaddteam.ClientID%>').value = "";
                     $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(0);
                    $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();
                    document.getElementById("sbtskbtnclsTeam").click();
                    document.getElementById("sbtsklbluser_" + document.getElementById("<%=sbtskhdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function sbtskBinddetails_Team(RecordId,sbtsklbluserid) {

          
                debugger;
               document.getElementById("<%=sbtskhdnaddteamhtml.ClientID%>").value = sbtsklbluserid.split('_')[1];

                $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskBindetail_Team',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1teamsbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1teamsbtsk(response) {
            
            debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=sbtskhdnteameid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    document.getElementById('<%=sbtskhdnaddteam.ClientID%>').value = Bindsubtaskdetail.find("team").text();

              <%-- if (Bindsubtaskdetail.find("team").text() != "")
             {
               var idsemp = Bindsubtaskdetail.find("team").text().split(',');
               $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();
            }--%>


              document.getElementById('<%=multichngtaskid.ClientID%>').value = Bindsubtaskdetail.find("task").text();
              sbtskMultiaddteam();
              if (Bindsubtaskdetail.find("team").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("team").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("team").text().split(',');
                      $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindsubtaskdetail.find("team").text();
                $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();
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

 <%--Multi sub task details End--%>



  <%--sub task save edit delete  start--%>
<script>
    debugger
    var ddLNotifyuserssubtask = $(document.getElementById("<%=Notifyuserssubtask.ClientID%>"));
   

    $(ddLNotifyuserssubtask).select2({
        data: [
    <%=Multiuserssubtask%>
        ],
        multiple: true,
        placeholder: "Search Notify users",
        width: "100%"
    });

   
     $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).val();
        document.getElementById("<%=hdnNotifyuserssubtask.ClientID %>").value = ids;
        document.getElementById("<%=Notifyuserssubtask.ClientID%>").click();

    });
   
  
    </script>





<script>


     function GetOwnerempdetails_subtask() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_newsubtask',
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

                                                         var ddLOwnersubtask = $(document.getElementById("<%=ownersubtask.ClientID%>"));
                                                         $(ddLOwnersubtask).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=ownersubtask.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=ownersubtask.ClientID%>")).val();
                                                        document.getElementById("<%=hdnownersubtask.ClientID %>").value = ids;
                                                        document.getElementById("<%=ownersubtask.ClientID%>").click();

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





     function GetTeamempdetails_subtask() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_newsubtask',
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

                                                         var ddLteamsubtask = $(document.getElementById("<%=teamsubtask.ClientID%>"));
                                                         $(ddLteamsubtask).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=teamsubtask.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=teamsubtask.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteamsubtask.ClientID %>").value = ids;
                                                        document.getElementById("<%=teamsubtask.ClientID%>").click();

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

   
    function GetWeitagebindDtlssubtask() {

                  var RecordId=document.getElementById("<%=task.ClientID%>").value;

                  $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_subtask',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschk_subtask,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschk_subtask(response) {
            
            debugger
            try {
                var waitagechkdtlssubtask = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtlssubtask.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagesubtask").style.display = "none";

                     }

                     if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagesubtask").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }

            GetOwnerempdetails_subtask();
            GetTeamempdetails_subtask();

             document.getElementById("divtasktimelinesub").style.display = "block";
            Bindtasktimeline_datasubtask();
            document.getElementById("divsubtasktimeline").style.display = "block";
            BindSubtasktimeline_data();


         }


       
    }
    

</script>


<script>
   
    function waitagepercentchksubtask(input)
    {
        
        if (input.value > 100)
        {
            
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>




<script>
   
    function checkdurationcalculatesubtask(input)
    {

        var ts_kid = document.getElementById("<%=task.ClientID%>").value;

       

                  $.ajax({
                    url: 'MultiProjectTask.aspx/BindcheckSubTaskdurationdetails',
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
  function GetReminderDtlssubtask()
  {
    if (document.getElementById('<%=Remindersubtask.ClientID%>').value == "0")
    {
            document.getElementById("divremindertypesubtask").style.display = "none";
            document.getElementById("divDayssubtask").style.display = "none";
            document.getElementById("Divtimesubtask").style.display = "none";
            document.getElementById("divNotifyuserssubtask").style.display = "none";
            document.getElementById("divChooseDatesubtask").style.display = "none";

    }


    if (document.getElementById('<%=Remindersubtask.ClientID%>').value == "1")
    {
            document.getElementById("divremindertypesubtask").style.display = "block";
            document.getElementById("Divtimesubtask").style.display = "block";
            document.getElementById("divNotifyuserssubtask").style.display = "block";
            document.getElementById("divChooseDatesubtask").style.display = "none";

      }

    if (document.getElementById('<%=Remindersubtask.ClientID%>').value == "2")
    {
            document.getElementById("divremindertypesubtask").style.display = "none";
            document.getElementById("divChooseDatesubtask").style.display = "block";
            document.getElementById("Divtimesubtask").style.display = "block";
            document.getElementById("divNotifyuserssubtask").style.display = "block";
            document.getElementById("divDayssubtask").style.display = "none";

      }


  } 

</script>

<script>
  function GetRemindertypeDtlssubtask()
  {
    if (document.getElementById('<%=ReminderTypesubtask.ClientID%>').value == "2")
    {
            document.getElementById("Divtimesubtask").style.display = "block";
            document.getElementById("divNotifyuserssubtask").style.display = "block";
            

    }
    if (document.getElementById('<%=ReminderTypesubtask.ClientID%>').value == "3")
    {
            document.getElementById("divDayssubtask").style.display = "none";
            document.getElementById("divChooseDatesubtask").style.display = "block";
            document.getElementById("Divtimesubtask").style.display = "block";
            document.getElementById("divNotifyuserssubtask").style.display = "block";
            

      }

    if (document.getElementById('<%=Remindersubtask.ClientID%>').value == "1" && document.getElementById('<%=ReminderTypesubtask.ClientID%>').value == "3")
    {
            document.getElementById("divChooseDatesubtask").style.display = "none";
            document.getElementById("divDayssubtask").style.display = "block";
            document.getElementById("Divtimesubtask").style.display = "block";
            document.getElementById("divNotifyuserssubtask").style.display = "block";
            

    }
  } 

</script>

<script>
    function checkdatesubtask(enterdate)
    {
        if (document.getElementById("<%=Startdatesubtask.ClientID%>").value != "" && document.getElementById("<%=Targetdatesubtask.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=Startdatesubtask.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=Targetdatesubtask.ClientID%>").value);
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
    function checkdatecomparisiontaskdatesubtask(enterdate) {
        debugger
        if (enterdate != "") {

            if (checkdatesubtask(enterdate)) {
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
                    url: 'MultiProjectTask.aspx/Bindcheckstrtdatetaskdetails',
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




<asp:HiddenField ID="hdnsavetypesubtask" runat="server" />
 <%--save data start --%> 

    <script type="text/javascript">
                function showdivimgsubtask() {
                    if (Page_ClientValidate("btnclosesubtask")) {
                        if (confirm('Do you want Leave this page ?')) {   

                           Cleardatasubtask();
                            document.getElementById("btnclosesubtask").click();
                                
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
        }


         function AddNewProjectstatussubtask()
        {
                debugger
                document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatussubtask").style.display = "none";
               if (document.getElementById('<%=P_Statussubtask.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatussubtask").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatussubtask.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColorsubtask.ClientID%>').value = "";
               }
        }


        function AddNewprioritysubtask()
        {
                debugger
                document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value = "0";
                 document.getElementById("dvnewprioritysubtask").style.display = "none";
               if (document.getElementById('<%=Prioritysubtask.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value = "1";
                    document.getElementById("dvnewprioritysubtask").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprioritysubtask.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolorsubtask.ClientID%>').value = "";
               }
        }


       function AddNewStagesubtask()
        {
                debugger
                document.getElementById('<%=hdnstagesubtask.ClientID%>').value = "0";
                 document.getElementById("dvnewStagesubtask").style.display = "none";
               if (document.getElementById('<%=Stagesubtask.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnstagesubtask.ClientID%>').value = "1";
                    document.getElementById("dvnewStagesubtask").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewstagesubtask.ClientID%>').value = "";
                    document.getElementById('<%=txtstageprintcolorsubtask.ClientID%>').value = "";
               }
        }


      


          function SvaeDatasubtask(val1Asubtask) {
                    debugger
              if (Page_ClientValidate("btnsavesubtask"))
              {
                        
                        document.getElementById('<%=hdnsavetypesubtask.ClientID%>').value = val1Asubtask;  

                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimgsubtask").style.display = "block";
                            document.getElementById("divsubsubtask").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }


                         //status save
                        
                        var statusidsubtask = "0";
                        if (document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value != "" && document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatussubtask.ClientID%>').value != "" && document.getElementById('<%=stustprintColorsubtask.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/statussavesubtask",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatussubtask.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColorsubtask.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        statusidsubtask = response.d;
                                    

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
                            statusidsubtask = document.getElementById('<%=P_Statussubtask.ClientID%>').value;
                        }

                        // Priority type save

                         var Prioritytypeidsubtask = "0";
                        if (document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value != "" && document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value != "0" && document.getElementById('<%=txtnewprioritysubtask.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolorsubtask.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddPrioritytypesubtask",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprioritysubtask.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolorsubtask.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        Prioritytypeidsubtask = response.d;
                                    

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
                            Prioritytypeidsubtask = document.getElementById('<%=Prioritysubtask.ClientID%>').value;
                        }



                      // Stage save

                         var stageidsubtask = "0";
                        if (document.getElementById('<%=hdnstagesubtask.ClientID%>').value != "" && document.getElementById('<%=hdnstagesubtask.ClientID%>').value != "0" && document.getElementById('<%=txtnewstagesubtask.ClientID%>').value != "" && document.getElementById('<%=txtstageprintcolorsubtask.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddStagesubtask",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewstagesubtask.ClientID%>').value) + ',Stagecolor: ' + JSON.stringify(document.getElementById('<%=txtstageprintcolorsubtask.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        stageidsubtask = response.d;
                                    

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
                            stageidsubtask = document.getElementById('<%=Stagesubtask.ClientID%>').value;
                        }



                        // save all details
                        
                        var subtaskdetails = {};
                        subtaskdetails.id = document.getElementById('<%=hdnidsubtask.ClientID%>').value;  
                        subtaskdetails.ProjectId = document.getElementById('<%=ProjectIdsubtask.ClientID%>').value; 
                        subtaskdetails.Milestoneid = document.getElementById('<%=Milestoneidsubtask.ClientID%>').value; 
                        subtaskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        subtaskdetails.task = document.getElementById('<%=task.ClientID%>').value; 
                        subtaskdetails.Subtaskname = document.getElementById('<%=Subtaskname.ClientID%>').value; 
                        subtaskdetails.SubTaskDescription = document.getElementById('<%=SubTaskDescription.ClientID%>').value; 
                        subtaskdetails.owner = document.getElementById('<%=hdnownersubtask.ClientID%>').value; 
                        subtaskdetails.Startdate = document.getElementById('<%=Startdatesubtask.ClientID%>').value; 
                       subtaskdetails.Targetdate = document.getElementById('<%=Targetdatesubtask.ClientID%>').value; 
                  

                        subtaskdetails.StartTime = document.getElementById('<%=StartTimesubtask.ClientID%>').value; 
                        subtaskdetails.Timeline = document.getElementById('<%=Timelinesubtask.ClientID%>').value; 
                        subtaskdetails.TaskDuration  = document.getElementById('<%=taskDurationsubtask.ClientID%>').value; 

                        <%--subtaskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        subtaskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>

                        subtaskdetails.P_Status = statusidsubtask;
                        subtaskdetails.Priority = Prioritytypeidsubtask;
                        subtaskdetails.Budget = document.getElementById('<%=Budgetsubtask.ClientID%>').value;
                        subtaskdetails.Actual = document.getElementById('<%=Actualsubtask.ClientID%>').value;

                        subtaskdetails.Reminder = document.getElementById('<%=Remindersubtask.ClientID%>').value; 

                        subtaskdetails.ReminderType = document.getElementById('<%=ReminderTypesubtask.ClientID%>').value; 
                        subtaskdetails.Days = document.getElementById('<%=Dayssubtask.ClientID%>').value; 
                        subtaskdetails.Time = document.getElementById('<%=Timesubtask.ClientID%>').value; 
                        subtaskdetails.Notifyusers = document.getElementById('<%=hdnNotifyuserssubtask.ClientID%>').value; 
                        subtaskdetails.ChooseDate = document.getElementById('<%=ChooseDatesubtask.ClientID%>').value; 

                        subtaskdetails.Occurrence = document.getElementById('<%=Occurrencesubtask.ClientID%>').value; 
                        subtaskdetails.tagto = document.getElementById('<%=tagtosubtask.ClientID%>').value; 
                       subtaskdetails.team = document.getElementById('<%=hdnteamsubtask.ClientID%>').value; 
                       subtaskdetails.Stage = stageidsubtask;
                       subtaskdetails.Weightage = document.getElementById('<%=Weightagesubtask.ClientID%>').value; 
                        
                        $.ajax({
                            type: "POST",
                            url: 'MultiProjectTask.aspx/SaveSubTask',
                            data: '{subtaskdetails: ' + JSON.stringify(subtaskdetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: OnSuccesssubtask,
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
                function OnSuccesssubtask(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=btnloadsubtask.ClientID%>').click();

                    

                     document.getElementById('<%=hdnProjectIdsubtask.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneidsubtask.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
                    document.getElementById('<%=hdntask.ClientID%>').value = "0";
                    document.getElementById('<%=Subtaskname.ClientID%>').value = "";
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = "";

                    document.getElementById('<%=hdnownersubtask.ClientID%>').value = "";
                     $(document.getElementById("<%=ownersubtask.ClientID%>")).val(0);
                    $(document.getElementById("<%=ownersubtask.ClientID%>")).change();

                    document.getElementById('<%=Startdatesubtask.ClientID%>').value ="";
                    document.getElementById('<%=Targetdatesubtask.ClientID%>').value = "";
                       document.getElementById('<%=hdnstarttimesubtask.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimelinesubtask.ClientID%>').value = "";
                     document.getElementById('<%=taskDurationsubtask.ClientID%>').value = "";
                 document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value = "";
                    document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value = "";
                     document.getElementById('<%=Budgetsubtask.ClientID%>').value = "";
                    document.getElementById('<%=Actualsubtask.ClientID%>').value = "";
                  document.getElementById('<%=Remindersubtask.ClientID%>').value = "";
                    document.getElementById('<%=Occurrencesubtask.ClientID%>').value = "";
                    document.getElementById('<%=Weightagesubtask.ClientID%>').value = "";
                  document.getElementById('<%=tagtosubtask.ClientID%>').value = "";
                document.getElementById('<%=hdnteamsubtask.ClientID%>').value = "0";

                     document.getElementById('<%=hdnReminderTypesubtask.ClientID%>').value = "";
              document.getElementById('<%=hdnTimesubtask.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyuserssubtask.ClientID%>').value = "";
              document.getElementById('<%=ChooseDatesubtask.ClientID%>').value = "";

                      $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).change();
                
            
             $(document.getElementById("<%=teamsubtask.ClientID%>")).val(0);
             $(document.getElementById("<%=teamsubtask.ClientID%>")).change();

                  
                    if (document.getElementById('<%=hdnsavetypesubtask.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimgsubtask").style.display = "none";
                        document.getElementById("divsubsubtask").style.display = "block";
                        document.getElementById("btnclssubtask").click();
                        Bindtask_data();
                    }
                    if (document.getElementById('<%=hdnsavetypesubtask.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimgsubtask").style.display = "none";
                        document.getElementById("divsubsubtask").style.display = "block";
                       // document.getElementById("btncls").click();
                       Bindtask_data();

                    }
                   

               
                    
                }



          function Cleardatasubtask() {
                        
                    document.getElementById('<%=hdnProjectIdsubtask.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneidsubtask.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
              document.getElementById('<%=hdntask.ClientID%>').value = "0";

                      document.getElementById('<%=hdnownersubtask.ClientID%>').value = "";
                     $(document.getElementById("<%=ownersubtask.ClientID%>")).val(0);
                  $(document.getElementById("<%=ownersubtask.ClientID%>")).change();

                    document.getElementById('<%=hdnStartdatesubtask.ClientID%>').value ="";
               document.getElementById('<%=hdnTargetdatesubtask.ClientID%>').value = "";
             

                 document.getElementById('<%=hdnstarttimesubtask.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimelinesubtask.ClientID%>').value = "";
                     document.getElementById('<%=taskDurationsubtask.ClientID%>').value = "";

                 document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value = "";
              document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value = "";
                document.getElementById('<%=Budgetsubtask.ClientID%>').value = "";
                document.getElementById('<%=Actualsubtask.ClientID%>').value = "";
              document.getElementById('<%=hdnteamsubtask.ClientID%>').value = "0";


               document.getElementById('<%=hdnReminderTypesubtask.ClientID%>').value = "";
              document.getElementById('<%=hdnTimesubtask.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyuserssubtask.ClientID%>').value = "";
              document.getElementById('<%=ChooseDatesubtask.ClientID%>').value = "";
              document.getElementById('<%=Weightagesubtask.ClientID%>').value = "";

                $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).val(0);
             $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).change();
                
            
            $(document.getElementById("<%=teamsubtask.ClientID%>")).val(0);
             $(document.getElementById("<%=teamsubtask.ClientID%>")).change();
           

                   document.getElementById('<%=Button1subtask.ClientID%>').click();
                   document.getElementById('<%=lbltitlesubtask.ClientID%>').innerHTML= "Add Sub Task";
         
                        
                    }

        

          function Binddetailssubtask(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindsubtaskdetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1subtask,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1subtask(response) {
            
            debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "")
                {
                    document.getElementById('<%=hdnidsubtask.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                     document.getElementById('<%=hdnProjectIdsubtask.ClientID%>').value = Bindsubtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneidsubtask.ClientID%>').value = Bindsubtaskdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist.ClientID%>').value = Bindsubtaskdetail.find("tasklist").text();
                    document.getElementById('<%=hdntask.ClientID%>').value = Bindsubtaskdetail.find("task").text();
                    document.getElementById("divprojectsubtask").style.display = "none";
                    document.getElementById("divmilestonesubtask").style.display = "none";
                    document.getElementById("divtasksubtask").style.display = "none";
                    document.getElementById('<%=Subtaskname.ClientID%>').value = Bindsubtaskdetail.find("Subtaskname").text();
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = Bindsubtaskdetail.find("SubTaskDescription").text();
                    document.getElementById('<%=Budgetsubtask.ClientID%>').value = Bindsubtaskdetail.find("Budget").text();
                    document.getElementById('<%=Actualsubtask.ClientID%>').value = Bindsubtaskdetail.find("Actual").text();
                    document.getElementById('<%=hdnremindersubtask.ClientID%>').value = Bindsubtaskdetail.find("Reminder").text();
                    document.getElementById('<%=Occurrencesubtask.ClientID%>').value = Bindsubtaskdetail.find("Occurrence").text();
                    document.getElementById('<%=tagtosubtask.ClientID%>').value = Bindsubtaskdetail.find("tagto").text();
                    document.getElementById('<%=teamsubtask.ClientID%>').value = Bindsubtaskdetail.find("team").text();
                    document.getElementById('<%=ReminderTypesubtask.ClientID%>').value = Bindsubtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=Dayssubtask.ClientID%>').value = Bindsubtaskdetail.find("Days").text();
                   
                    $("[id*=Notifyuserssubtask]").val(Bindsubtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDatesubtask]").val(Bindsubtaskdetail.find("ChooseDate").text()); 
                    document.getElementById('<%=hdnownersubtask.ClientID%>').value = Bindsubtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnStartdatesubtask.ClientID%>').value = Bindsubtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdnTargetdatesubtask.ClientID%>').value = Bindsubtaskdetail.find("Targetdate").text();
                    
                    document.getElementById('<%=hdnstarttimesubtask.ClientID%>').value = Bindsubtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimelinesubtask.ClientID%>').value = Bindsubtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDurationsubtask.ClientID%>').value = Bindsubtaskdetail.find("TaskDuration").text();

                    document.getElementById('<%=P_Statussubtask.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnP_Statussubtask.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                     document.getElementById('<%=txtnewprojectstatussubtask.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColorsubtask.ClientID%>').style.display = "none";


                    document.getElementById('<%=Prioritysubtask.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnPrioritysubtask.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                     document.getElementById('<%=txtnewprioritysubtask.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolorsubtask.ClientID%>').style.display = "none";

                    document.getElementById('<%=hdnteamsubtask.ClientID%>').value = Bindsubtaskdetail.find("team").text();


                    document.getElementById('<%=Stagesubtask.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
                    document.getElementById('<%=hdnstagesubtask.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
                    document.getElementById('<%=txtnewstagesubtask.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtstageprintcolorsubtask.ClientID%>').style.display = "none";

                    document.getElementById('<%=Weightagesubtask.ClientID%>').value = Bindsubtaskdetail.find("Weightage").text();

                     document.getElementById('<%=hdnReminderTypesubtask.ClientID%>').value = Bindsubtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=hdnTimesubtask.ClientID%>').value = Bindsubtaskdetail.find("Time").text();
                    document.getElementById('<%=hdnNotifyuserssubtask.ClientID%>').value = Bindsubtaskdetail.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDatesubtask.ClientID%>').value = Bindsubtaskdetail.find("ChooseDate").text();


                     // time and reminder fill and edit strt//

                     if (Bindsubtaskdetail.find("Reminder").text() == "0")
                     {
                       document.getElementById("divremindertypesubtask").style.display = "none";
                       document.getElementById("divDayssubtask").style.display = "none";
                       document.getElementById("Divtimesubtask").style.display = "none";
                       document.getElementById("divNotifyuserssubtask").style.display = "none";
                       document.getElementById("divChooseDatesubtask").style.display = "none";

                    }


           
            //  Task Timeline //
            debugger
            document.getElementById("divtasktimelinesub").style.display = "block";
             var tski_d = Bindsubtaskdetail.find("task").text();
             //alert(tski_d);
               debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata_subtask',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timelinesubtask,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timelinesubtask(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimelinesub.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }


              
             //Sub Task Timeline //
             debugger
             document.getElementById("divsubtasktimeline").style.display = "block";
             var tski_d = Bindsubtaskdetail.find("task").text();
             //alert(tski_d);
              debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindSubtasktimelinedatasubtask',
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
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }





                    GetReminderDtlssubtask();
                    GetRemindertypeDtlssubtask();
                    // time and reminder fill and edit end//


             if (Bindsubtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindsubtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyuserssubtask.ClientID%>")).change();
            }
           document.getElementById('<%=Button1subtask.ClientID%>').click();

           

             GetOwnerempdetails_subtask();
             if (Bindsubtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=ownersubtask.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ownersubtask.ClientID%>")).change();

                  }
                  else
                  {

                   var idsemp = Bindsubtaskdetail.find("owner").text();
                  $(document.getElementById("<%=ownersubtask.ClientID%>")).val(idsemp);
                  $(document.getElementById("<%=ownersubtask.ClientID%>")).change();

                  }
             
             }



              GetTeamempdetails_subtask();
             if (Bindsubtaskdetail.find("team").text() != "")
             {
                 debugger
                 if (Bindsubtaskdetail.find("team").text().includes(','))
                 {
                      var idsemp = Bindsubtaskdetail.find("team").text().split(',');
                     $(document.getElementById("<%=teamsubtask.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teamsubtask.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = Bindsubtaskdetail.find("team").text();
                     $(document.getElementById("<%=teamsubtask.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teamsubtask.ClientID%>")).change();
                 }
                 

            }

                    

                    
                    document.getElementById('btnsavesubtask').value = "Update";
                   document.getElementById('<%=lbltitlesubtask.ClientID%>').innerHTML= "Edit Sub Task";
                }
            }
            catch
                {
                }

            }


        
        


            </script>





<script>
    
    $(document).ready(function ()
    {
            Bindtasktimeline_datasubtask();
});
    function Bindtasktimeline_datasubtask()
    {
             debugger
            
             
             var tski_d = document.getElementById("<%=task.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata_subtask',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timelinesubtask,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccesstask_timelinesubtask(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimelinesub.ClientID%>");
                  //  lbl.innerHTML = '';                    
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
                      url: 'MultiProjectTask.aspx/bindSubtasktimelinedatasubtask',
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
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }

    </script>


<script>
        function deletedatasubtask(RecordId) {
            
                 if (confirm('Do you really want to delete the Entery?'))
                 {
                      
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                    url: 'MultiProjectTask.aspx/Deletesubtaskdata',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessdelete_subtaskdata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessdelete_subtaskdata(response) {
            alert('Data Deleted !');
            Bindtask_data();
        }

    </script>


<%--sub task save edit delete  end--%>



<script> 
    
    <%=setmilstone%>
   
</script>


<%--<script>
    
$(function() {
  $('#showtr2').on('click', function() {
    $('.taskshow2').toggle();
  });
});
$('#showtr2').click(function() {
    $(this).toggleClass('shown');
    });

    //function GetbindtaskdataP_hideshow() {
        debugger
        var RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
        if (RecordId == "" || RecordId == "0") {
            RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
        }

        $.ajax({
            url: 'MultiProjectTask.aspx/bindtaskdataP',
            data: "{'RecordId': '" + RecordId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: OnSuccesssetmilstonehideshow_data,
            error: function (response) {
                //alert(response.responseText);
                alert(response.responseText);
            },
            failure: function (response) {
                alert(response.responseText);
            }
        });

  //  }

     function OnSuccesssetmilstonehideshow_data(response)
     {
         debugger
        // // alert(response.d);
        // var todolist = response.d;
        // alert(todolist);
        //// lbl.innerHTML = todolist;
        // todolist;



          //$(function() {$('#showtrM_8').on('click', function() { $('.taskshow_8').toggle();  });}); $('#showtrM_8').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_27').on('click', function() {$('.subtaskshow_27').toggle();  });}); $('#showtasktr_27').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_28').on('click', function() {$('.subtaskshow_28').toggle();  });}); $('#showtasktr_28').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtrM_7037').on('click', function() { $('.taskshow_7037').toggle();  });}); $('#showtrM_7037').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_2052').on('click', function() {$('.subtaskshow_2052').toggle();  });}); $('#showtasktr_2052').click(function() { $(this).toggleClass('shown');});
                  
     
        


    }


    


  



</script>--%>



<%--save milestone and edit delete start --%>

<script>
   function Getempdetailsmile() {
        debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
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

                                                         var ddLOwnermile = $(document.getElementById("<%=Ownermile.ClientID%>"));
                                                         $(ddLOwnermile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=Ownermile.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=Ownermile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnownermile.ClientID %>").value = ids;
                                                        document.getElementById("<%=Ownermile.ClientID%>").click();

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





     function GetTeamempdetailsmile() {
        debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
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

                                                         var ddLteammile = $(document.getElementById("<%=teammile.ClientID%>"));
                                                         $(ddLteammile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=teammile.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=teammile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteammile.ClientID %>").value = ids;
                                                        document.getElementById("<%=teammile.ClientID%>").click();

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


    function GetWeitagebindDtlsmile() {
        debugger
                  var RecordId=document.getElementById("<%=ProjectIdmile.ClientID%>").value;

                  $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_mile',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkmilestone,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschkmilestone(response) {
            
            debugger
            try {
                 
                var waitagechkdtlsmile = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtlsmile.find("id").text() != "")
                {
                     // alert(waitagechkdtlsmile.find("checkprojectWeightage").text());
                      if (waitagechkdtlsmile.find("checkprojectWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "none";

                     }

                     if (waitagechkdtlsmile.find("checkprojectWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "block";

                     }
                    
                }
                
            }
            catch

            {

            }

            Getempdetailsmile();
            GetTeamempdetailsmile();

            document.getElementById("divprojecttimeline").style.display = "block";
            Bindprojecttimeline_data();

             document.getElementById("divmiletimeline_mile").style.display = "block";
            Bindmilestonetimeline_data_mile();
          
         }


       
    }
    

</script>



<script>
   
    function waitagepercentchkmile(input)
    {
        
        if (input.value > 100)
        {
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>


<script>
    function checkdatemile(enterdate)
    {
        if (document.getElementById("<%=StartDatemile.ClientID%>").value != "" && document.getElementById("<%=EndDatemile.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=StartDatemile.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=EndDatemile.ClientID%>").value);
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
    function checkdatecomparisiontaskdate_mile(enterdate) {
        debugger
        if (enterdate != "") {

            if (checkdatemile(enterdate)) {
                var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

                var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;



                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckstrtdateprojectdetails',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                        var milestonestrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (milestonestrtdatechkdtls.find("id").text() != "") {
                            const today = new Date(milestonestrtdatechkdtls.find("StartDate").text());
                            var fdt = new Date(today);
                            const today2 = new Date(milestonestrtdatechkdtls.find("TargetDate").text());
                            var todt = new Date(today2);

                            var t1 = new Date();
                            t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                            var t2 = new Date();
                            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
                            if (fcompdate >= t1 && fcompdate <= t2) {
                                return true
                            }
                            else {
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



<script>
  function GetDurationchange()
  {


    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "0")
    {
        document.getElementById("divchngtype").style.display = "none";
         
    }

    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "1")
    {
        document.getElementById("divchngtype").style.display = "block";
        document.getElementById("chngtype").innerHTML = "(Hours)";
         
    }

    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "2")
    {
        document.getElementById("divchngtype").style.display = "block";
        document.getElementById("chngtype").innerHTML = "(Days)";
          
     }


    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "3")
    {
        document.getElementById("divchngtype").style.display = "block";
       document.getElementById("chngtype").innerHTML = "(Month)";
        
    }





  } 

</script>



<asp:HiddenField ID="hdnsavetypemile" runat="server" />

    <%--save data start --%> 

    <script type="text/javascript">
                function showdivimgmile() {
                    if (Page_ClientValidate("btnclosemile")) {
                        if (confirm('Do you want Leave this page ?')) {   

                           Cleardatamile();
                            document.getElementById("btnclosemile").click();
                                
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
         }


        function AddNewProjectstatusmile()
        {
                debugger
                document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatusmile").style.display = "none";
               if (document.getElementById('<%=P_Statusmile.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatusmile").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColormile.ClientID%>').value = "";
               }
        }


        function AddNewprioritymile()
        {
                debugger
                document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "0";
                 document.getElementById("dvnewprioritymile").style.display = "none";
               if (document.getElementById('<%=Prioritymile.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "1";
                    document.getElementById("dvnewprioritymile").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprioritymile.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolormile.ClientID%>').value = "";
               }
        }

         

      


          function SvaeDatamile(val1Amile) {
                    debugger
              if (Page_ClientValidate("btnsavemile"))
              {
                       
                       document.getElementById('<%=hdnsavetypemile.ClientID%>').value = val1Amile;  
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimgmile").style.display = "block";
                            document.getElementById("divsubmile").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }




                      // Project status save
                        
                        var milstonestatusidmile = "0";
                        if (document.getElementById('<%=hdnP_Statusmile.ClientID%>').value != "" && document.getElementById('<%=hdnP_Statusmile.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value != "" && document.getElementById('<%=stustprintColormile.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddAddMilestonetatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColormile.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milstonestatusidmile = response.d;
                                    

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
                            milstonestatusidmile = document.getElementById('<%=P_Statusmile.ClientID%>').value;
                        }

                        // Priority type save

                         var milePrioritytypeidmile = "0";
                        if (document.getElementById('<%=hdnPrioritymile.ClientID%>').value != "" && document.getElementById('<%=hdnPrioritymile.ClientID%>').value != "0" && document.getElementById('<%=txtnewprioritymile.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolormile.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddMilestonePrioritytype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprioritymile.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolormile.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milePrioritytypeidmile = response.d;
                                    

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
                            milePrioritytypeidmile = document.getElementById('<%=Prioritymile.ClientID%>').value;
                        }


                        // save all details

                        
                        var milestonedetails = {};
                        milestonedetails.id = document.getElementById('<%=hdnidmile.ClientID%>').value; 
                        milestonedetails.ProjectId = document.getElementById('<%=ProjectIdmile.ClientID%>').value; 
                        milestonedetails.Milestone = document.getElementById('<%=Milestone.ClientID%>').value; 
                        milestonedetails.Weightage = document.getElementById('<%=Weightagemile.ClientID%>').value; 
                        milestonedetails.StartDate = document.getElementById('<%=StartDatemile.ClientID%>').value; 
                        milestonedetails.EndDate = document.getElementById('<%=EndDatemile.ClientID%>').value; 
                       milestonedetails.Owner = document.getElementById('<%=hdnownermile.ClientID%>').value;
                       milestonedetails.Access = document.getElementById('<%=Accessmile.ClientID%>').value; 
                        milestonedetails.team = document.getElementById('<%=hdnteammile.ClientID%>').value; 
                        milestonedetails.P_Status = milstonestatusidmile;
                        milestonedetails.Priority = milePrioritytypeidmile;
                        milestonedetails.Budget = document.getElementById('<%=Budgetmile.ClientID%>').value;
                        milestonedetails.Actual = document.getElementById('<%=Actualmile.ClientID%>').value;


                       if (document.getElementById('<%=istask.ClientID%>').checked == true)
                       {

                        milestonedetails.istask = 1

                       }

                       if (document.getElementById('<%=istask.ClientID%>').checked == false)
                       {

                        milestonedetails.istask = 0

                       }

                  
                       if (document.getElementById('<%=checkmileWeightage.ClientID%>').checked == true)
                       {

                        milestonedetails.checkmileWeightage = 1

                       }

                       if (document.getElementById('<%=checkmileWeightage.ClientID%>').checked == false)
                       {

                        milestonedetails.checkmileWeightage = 0

                       }


                   milestonedetails.Durationtype = document.getElementById('<%=ddldurationtype.ClientID%>').value; 
                   milestonedetails.TaskDuration = document.getElementById('<%=taskDurationmile.ClientID%>').value; 

                        $.ajax({
                            type: "POST",
                            url: 'MultiProjectTask.aspx/SaveMilestone',
                            data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: OnSuccess_mile,
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
                function OnSuccess_mile(response) {
                    debugger
                    alert("Data Saved successfully."); 

                    document.getElementById('<%=btnloadmile.ClientID%>').click();

                  

                   document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = "0";
                    document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightagemile.ClientID%>').value = "";
                    document.getElementById('<%=StartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=EndDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value ="";
                    
                      document.getElementById('<%=hdnownermile.ClientID%>').value = "";
                     $(document.getElementById("<%=Ownermile.ClientID%>")).val(0);
                   $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                     $(document.getElementById("<%=teammile.ClientID%>")).val(0);
                    $(document.getElementById("<%=teammile.ClientID%>")).change();
                    document.getElementById('<%=hdnAccessmile.ClientID%>').value = "0";
                     document.getElementById('<%=hdnteammile.ClientID%>').value = "";
                    document.getElementById('<%=P_Statusmile.ClientID%>').value = "0";
                    document.getElementById('<%=Prioritymile.ClientID%>').value = "0";
                    document.getElementById('<%=Budgetmile.ClientID%>').value = "0";
                    document.getElementById('<%=Actualmile.ClientID%>').value = "0";
                    document.getElementById('<%=istask.ClientID%>').checked = 0;
                    document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;
                    <%--alert(document.getElementById('<%=hdnsavetype.ClientID%>').value);--%>

                    document.getElementById('<%=taskDurationmile.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";

                     document.getElementById('<%=hdnidmile.ClientID%>').value = "0";
                    if (document.getElementById('<%=hdnsavetypemile.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                        document.getElementById("btnclsmile").click();
                        Bindtask_data();
                    }
                    
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                       // document.getElementById("btncls").click();
                        Bindtask_data();
                    }
                   
                   
               
                    
                }



          function Cleardatamile() {
                        
                  
              document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = "0";
                  document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightagemile.ClientID%>').value = "";
                    document.getElementById('<%=StartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=EndDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value ="";
                    document.getElementById('<%=hdnownermile.ClientID%>').value ="";
                   document.getElementById('<%=hdnAccessmile.ClientID%>').value = "";
                   document.getElementById('<%=hdnteammile.ClientID%>').value = "";
                   document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "0";
                   document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "0";
                   document.getElementById('<%=istask.ClientID%>').checked = 0;
                 document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;

                   document.getElementById('<%=taskDurationmile.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";


                  $(document.getElementById("<%=Ownermile.ClientID%>")).val(0);
                 $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                  $(document.getElementById("<%=teammile.ClientID%>")).val(0);
                 $(document.getElementById("<%=teammile.ClientID%>")).change();

                   document.getElementById('<%=Button1mile.ClientID%>').click();
                   document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML= "Add Milestone";
         
                        
                    }

        

          function Binddetails_mile(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindmilestonedetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1_milestonedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1_milestonedata(response) {
            
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnidmile.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=ProjectIdmile.ClientID%>').value = (milestonedetails.find("ProjectId").text());

                     document.getElementById('<%=Milestone.ClientID%>').value=(milestonedetails.find("Milestone").text());
                    document.getElementById('<%=Weightagemile.ClientID%>').value=(milestonedetails.find("Weightage").text());
                    document.getElementById('<%=StartDatemile.ClientID%>').value=(milestonedetails.find("StartDate").text());
                    document.getElementById('<%=EndDatemile.ClientID%>').value=(milestonedetails.find("EndDate").text());
                    document.getElementById('<%=hdnownermile.ClientID%>').value=(milestonedetails.find("Owner").text());
                    document.getElementById('<%=Accessmile.ClientID%>').value = (milestonedetails.find("Access").text()); 
                    
                    document.getElementById('<%=Budgetmile.ClientID%>').value = (milestonedetails.find("Budget").text()); 
                    document.getElementById('<%=Actualmile.ClientID%>').value = (milestonedetails.find("Actual").text()); 

                    document.getElementById('<%=P_Statusmile.ClientID%>').value = milestonedetails.find("P_Status").text();
                    document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = milestonedetails.find("P_Status").text();
                        document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColormile.ClientID%>').style.display = "none";


                    document.getElementById('<%=Prioritymile.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=hdnPrioritymile.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=txtnewprioritymile.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolormile.ClientID%>').style.display = "none";

                     document.getElementById('<%=hdnteammile.ClientID%>').value = milestonedetails.find("team").text();

                    document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
                     document.getElementById('<%=hdnStartDatemile.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value = milestonedetails.find("EndDate").text();
                    document.getElementById('<%=hdnAccessmile.ClientID%>').value = milestonedetails.find("Access").text();


                  if (milestonedetails.find("istask").text() == 1)
                  {
                      document.getElementById('<%=istask.ClientID%>').checked = true;

                  }

                  if (milestonedetails.find("istask").text() == 0)
                  {
                      document.getElementById('<%=istask.ClientID%>').checked = false;

                  }


                   if (milestonedetails.find("checkmileWeightage").text() == 1)
                   {
                      document.getElementById('<%=checkmileWeightage.ClientID%>').checked = true;

                   }

                   if (milestonedetails.find("checkmileWeightage").text() == 0)
                   {
                      document.getElementById('<%=checkmileWeightage.ClientID%>').checked = false;

                    }

                    document.getElementById('<%=hdndurationtype.ClientID%>').value = milestonedetails.find("Durationtype").text();
                    document.getElementById('<%=taskDurationmile.ClientID%>').value = milestonedetails.find("TaskDuration").text();

                    
                    if (milestonedetails.find("Durationtype").text() == "1")
                    {
                      document.getElementById("divchngtype").style.display = "block";
                     document.getElementById("chngtype").innerHTML = "(Hours)";
         
                    }
                    if (milestonedetails.find("Durationtype").text() == "2")
                    {
                     document.getElementById("divchngtype").style.display = "block";
                     document.getElementById("chngtype").innerHTML = "(Days)";
                    }

                    if (milestonedetails.find("Durationtype").text() == "3")
                    {
                     document.getElementById("divchngtype").style.display = "block";
                     document.getElementById("chngtype").innerHTML = "(Month)";
                    }

                     document.getElementById("divprojecttimeline").style.display = "block";
                    Bindprojecttimeline_data();
                     document.getElementById("divmiletimeline_mile").style.display = "block";
                    Bindmilestonetimeline_data_mile();

                  //  WeitageChecked();
                  //GetWeitagebindDtls();
                    document.getElementById('<%=Button1mile.ClientID%>').click();
                    Getempdetailsmile();

              if (milestonedetails.find("Owner").text() != "")
              {
                  debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=Ownermile.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
                $(document.getElementById("<%=Ownermile.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=Ownermile.ClientID%>")).change();
                  }
             
            }

              GetTeamempdetailsmile();
             if (milestonedetails.find("team").text() != "")
             {
                 debugger
                 if (milestonedetails.find("team").text().includes(','))
                 {
                      var idsemp = milestonedetails.find("team").text().split(',');
                     $(document.getElementById("<%=teammile.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teammile.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = milestonedetails.find("team").text();
                     $(document.getElementById("<%=teammile.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teammile.ClientID%>")).change();
                 }



            }
                  
                     
                    document.getElementById('btnsavemile').value = "Update";
                   document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML= "Edit Milestone";
                }
            }
            catch
                {
                }

            }


        
        


            </script>



<script>
    
    $(document).ready(function ()
    {
            Bindprojecttimeline_data();
});
    function Bindprojecttimeline_data()
    {
             debugger
            
             var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
             //alert(projec_id);
              debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindProjecttimelinedata',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessproject_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessproject_timeline(response)
        {
            debugger
            var todolist = response.d;

                    let lbl = document.getElementById("<%=projecttimeline.ClientID%>");
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }

    </script>
<script>
    
    $(document).ready(function ()
    {
            Bindmilestonetimeline_data_mile();
});
    function Bindmilestonetimeline_data_mile()
    {
             debugger
            
             
             var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata_mile',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetails_miletimelinedata_mile(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline_mile.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
   
           
        }

    </script>







    <script>
        function deletedatamile(RecordId) {
            
                 if (confirm('Do you really want to delete the Entery?'))
                 {
                      
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                    url: 'MultiProjectTask.aspx/DeleteMilestonedata',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessdeletemile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessdeletemile(response) {
            alert('Data Deleted !');
            Bindtask_data();
        }

    </script>




<%--save milestone save and edit delete end--%>

   

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
                   // alert(column.visible());
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
                        url: 'MultiProjectTask.aspx/Savecolumndata',
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
 

    <script type="text/javascript">
        function hidecolumngrid()
        {
           // alert("yes");
            for (var i = 0; i < 16; i++)
            {  
               // alert("no");
                var aa = "ContentPlaceHolder1_MultipleData_hdn" + i; 
                if (document.getElementById(aa).value == "0")
                { 
                   // alert("0");
                    let column = table.column(i); 
                    column.visible(false);

                }
            }
          
            document.getElementById("divgridview").style.display = "block";
            document.getElementById("divgridview").style.width = "100%";
            document.getElementById("ContentPlaceHolder1_MultipleData_lblprojectdata").style.width = "100%";
           
        }
    </script>

 <%-- show column hide show end--%>



 <%-- task auto fill start --%>
 <script type="text/javascript">
  function AddMultitask(Projectid,Milestoneid) {
      //alert(Milestoneid);
      //alert(Projectid);
      
      debugger;

      document.getElementById("<%=hdnProjectId.ClientID%>").value = Projectid;
      document.getElementById("<%=hdnMilestoneid.ClientID%>").value = Milestoneid;
      document.getElementById('<%=Button1.ClientID%>').click();
      
        document.getElementById("divproject").style.display = "none";
        document.getElementById("divmilestone").style.display = "none";
        var RecordId=document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
         
         $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails',
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
                var waitagechkdtls_task = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls_task.find("id").text() != "")
                {
                 
                      if (waitagechkdtls_task.find("checkmileWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls_task.find("checkmileWeightage").text() == 0)
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
           mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new',
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
           mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new',
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
              var mile_id = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata',
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
                    lbl.innerHTML = todolist;
           
        }



           //  Task Timeline //
             debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
             //alert(projec_id);
        
                debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline_autofill,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline_autofill(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                    lbl.innerHTML = todolist;
           
           
        }
    

        }

    </script>
 <%-- task auto fill end --%>

<%--subtask task auto fill start --%>
<script type="text/javascript">
  function AddMultisubtask(Projectid,Milestoneid,task_id) {
      //alert(Projectid);
      //alert(Milestoneid);
      //alert(task_id);
      debugger;
      document.getElementById("<%=hdnProjectIdsubtask.ClientID%>").value = Projectid;
      document.getElementById("<%=hdnMilestoneidsubtask.ClientID%>").value = Milestoneid;
      document.getElementById("<%=hdntask.ClientID%>").value = task_id;
      document.getElementById('<%=Button1subtask.ClientID%>').click();

      
    
            document.getElementById("divprojectsubtask").style.display = "none";
            document.getElementById("divmilestonesubtask").style.display = "none";
           document.getElementById("divtasksubtask").style.display = "none";
      
         var RecordId=document.getElementById("<%=hdntask.ClientID%>").value;
         
         $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_subtask',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise_autosub,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });




              function OnSuccessweitagedtlschkpagewise_autosub(response) {
            
            debugger
            try {
                var waitagechkdtlssubtask = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtlssubtask.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagesubtask").style.display = "none";

                     }

                     if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagesubtask").style.display = "block";

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
           tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_newsubtask',
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

                                                         var ddLOwnersubtask = $(document.getElementById("<%=ownersubtask.ClientID%>"));
                                                         $(ddLOwnersubtask).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=ownersubtask.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=ownersubtask.ClientID%>")).val();
                                                        document.getElementById("<%=hdnownersubtask.ClientID %>").value = ids;
                                                        document.getElementById("<%=ownersubtask.ClientID%>").click();

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
           tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_newsubtask',
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

                                                         var ddLteamsubtask = $(document.getElementById("<%=teamsubtask.ClientID%>"));
                                                         $(ddLteamsubtask).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=teamsubtask.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=teamsubtask.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteamsubtask.ClientID %>").value = ids;
                                                        document.getElementById("<%=teamsubtask.ClientID%>").click();

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
            document.getElementById("divtasktimelinesub").style.display = "block";
             var tski_d = document.getElementById("<%=hdntask.ClientID%>").value;
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata_subtask',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline_atofl,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline_atofl(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimelinesub.ClientID%>");
                    lbl.innerHTML = todolist;
           
           
        }

            //Sub Task Timeline //
             debugger
             document.getElementById("divsubtasktimeline").style.display = "block";
             var tski_d = document.getElementById("<%=hdntask.ClientID%>").value;
             //alert(projec_id);
              debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindSubtasktimelinedatasubtask',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssubtask_timeline_auto,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesssubtask_timeline_auto(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblsubtasktimeline.ClientID%>");
                    lbl.innerHTML = todolist;
           
           
        }

   

        }

    </script>
<%--subtask auto fill end --%>




    


  


 





