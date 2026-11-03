<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Issues.ascx.cs" Inherits="ucontrol_Issues" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<style>
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
    <%--<div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
       <div class="card border-0">--%>
    <div class="card-header">
        <div class="row">
            <div class="col-sm-auto">
                <div class="inner-head">
                    <div class="opp-ico red">
                        <i class="ri-alert-line"></i>
                    </div>
                    <div class="head-txt">
                        <p>Issues</p>                                                    
                        <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside" style="display:none;">
                            <h2><label id="ContentPlaceHolder1_lblowner">All Issues</label> <i class="bi bi-caret-down-fill"></i></h2>
                        </a>
                        <div class="dropdown-menu dropdown-menu-end">
                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">All Issues</span></a>                                             
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
                    <%if (ischeckCreate == 1)
                    {%> 
                    <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addissues"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                     <%}%>
                    
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                    <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                    <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                    <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">

                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issues", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Tags", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Assign To", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Create Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Target Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue10" data-column="10"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issue Type", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Replicability", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("File", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="issue15" data-column="15"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>

                                          

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
    <div class="card-body pb-0">
                                 <div class="table-responsive" style="height: calc(100vh - 110px);overflow:auto;">
                                  <div id="divgridview" >
                                 <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                                 </div>
                             <div style="display:none;">
                              <asp:Button ID="btndelete" runat="server"    Text="delete"  />
                             </div>
        </div>
    </div>
          <%--  </div>
            </div>
      </div>
    </div>--%>
    
<div class="offcanvas offcanvas-end" id="addissues" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
           <label id="lbltitle" runat="server">Add Issues</label> 

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fs12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fse12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
             <div style="display:none"><button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 90px);">
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
                                 <asp:HiddenField ID="hdnprojecttype" runat="server" />

                                 <asp:HiddenField ID="hdnmil_stoneid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntasklis_tid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntask_id" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnsubtask_id" runat="server" Value="0" />


                                 <div class="col-xxl-12 col-lg-12" id="divproject">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
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
                                
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="tasklistid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="tasklistid_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklistid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                               <div class="col-xxl-12 col-lg-12" id="divtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>
                                  

                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="taskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Task ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="taskid_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntaskid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divsubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task", "2")%></label>
                                    </div>
                                  

                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="subtaskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Sub Task ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnsubtaskid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Issues", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Issues" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Issues" runat="server" placeholder="Issues" class="form-control">
                                </div>
                                <%--<div class="col-xxl-2 col-lg-2">
                                    <div class="d-block align-items-center">
                                        <label class="form-label">&nbsp;</label>
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Copy From Existing">
                                            <label class="form-label">
                                                <input class="form-check-input" type="checkbox" />  Copy                                               
                                            </label>
                                        </div>
                                    </div>
                                </div>--%>
                              


                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Description", "2")%></label>
                                    </div>
                                    <textarea id="Description" runat="server" placeholder="Description" class="form-control"></textarea>
                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="Owner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                      

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


                                <div class="col-xxl-12 col-lg-12"  id="divreminder">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%>  &nbsp;&nbsp;</label>
                                    </div>
                                   
                                      <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                              <cc1:DropDownListChosen ID="Reminder" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true" onchange="GetReminderDtls()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Based on Due Date</asp:ListItem>
                                          <asp:ListItem  value="2">Specific Date</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnReminder" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divremindertype">
                                <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder Type", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator10" 
                                                ControlToValidate="ReminderType" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                    
                                      <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                              <cc1:DropDownListChosen ID="ReminderType" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true" onchange="GetRemindertypeDtls()">
                                         <asp:ListItem  value="1">Daily</asp:ListItem>
                                          <asp:ListItem  value="2">On due date</asp:ListItem>
                                          <asp:ListItem  value="3">Day(s) before due date</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnReminderType" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                     </div>

                                      <div class="col-xxl-12 col-lg-12" id="divDays">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Enter Day(s)</label>
                                        </div>
                                        <input type="text" id="Days" runat="server" placeholder="Days" class="form-control">
                                    </div>

                                <div class="col-xxl-12 col-lg-12" id="divChooseDate">
                                <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Choose Date</label>
                                    </div>
                                
                                      <asp:UpdatePanel ID="UpdatePanel16" runat="server">
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
                                        <label class="form-label flex-grow-1">Time</label>
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
                                        <label class="form-label flex-grow-1">Assign To &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" 
                                                ControlToValidate="AssignTo" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                            
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                             <cc1:DropDownListChosen ID="AssignTo" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Assign To ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnAssignTo" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



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
                                    <input type="text" id="StartDate" runat="server" class="form-control">
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
                                    <input type="text" id="EndDate" runat="server" class="form-control">
                                             <asp:HiddenField ID="hdnEndDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Status &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="IssueStatus" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                     <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                             <cc1:DropDownListChosen ID="IssueStatus" CssClass="form-control" runat="server" Width="220px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Status..." AllowSingleDeselect="true" onchange="AddNewIssuestatus()">                                                        
                                                    </cc1:DropDownListChosen>
                                             </ContentTemplate>
                                        </asp:UpdatePanel>

                                         <div id="dvnewissuestatus" style="display:none">
                                          <input type="text" id="txtnewissustatus" class="form-control" placeholder="Status"  runat="server" />
                                           <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                             </div>
                                            <asp:HiddenField ID="hdnIssueStatus" runat="server" Value="0" />
                                           


                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="priority ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
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
                                        <label class="form-label flex-grow-1">Issue Type &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="IssueType" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   <%-- <select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Security</option>
                                        <option>Crash/Hang</option>
                                        <option>Data Loss</option>
                                        <option>Performance</option>
                                        <option>+ Add</option>
                                    </select>--%>


                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>


                                               <cc1:DropDownListChosen ID="IssueType" CssClass="form-control" runat="server" Width="220px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Issue Type..." AllowSingleDeselect="true" onchange="AddNewIssueType()">                                                        
                                                    </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                         <div id="dvnewissueTypes" style="display:none">
                                          <input type="text" id="txtnewissutype" class="form-control" placeholder="Issue Type"  runat="server" />
                                             <input  data-jscolor="{}" id="txtIssuescolor" class="jscolor form-control" runat="server">
                                              </div>
                                               <asp:HiddenField ID="hdnIssueType" runat="server" Value="0" />
                                         

                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Replicability &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Replicability" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                              <cc1:DropDownListChosen ID="Replicability" CssClass="form-control" runat="server" Width="220px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Replicability..." AllowSingleDeselect="true" onchange="AddNewReplicability()">                                                        
                                                    </cc1:DropDownListChosen>
                                              </ContentTemplate>
                                        </asp:UpdatePanel>
                                         <div id="dvnewReplicability" style="display:none">
                                          <input type="text" id="txtReplicability" class="form-control" placeholder="Replicability"  runat="server" />
                                              <input  data-jscolor="{}" id="txtReplicabilitycolor" class="jscolor form-control" runat="server">
                                              </div>
                                               <asp:HiddenField ID="hdnReplicability" runat="server" Value="0" />
                                          


                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">File(s) Upload</label>
                                    </div>
                                    

                                        <div style="display:none;">
                                        <input type="file" id="UploadImage" runat="server" class="form-control" style="border-color:#777;border-radius:0px;" aria-label="Upload">
                                        <button class="btn btn-success" style="border-radius:0px;"  type="button">Upload</button>  
                                            </div>
                                        
                                        <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreviewnew();"/>
                                        <a href="#" id="imgpath" target="_blank" style="display:none"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>
                                    <div id="previewContainer1" style="margin-top: 10px;"></div>

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

<link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
  <%--  <script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
 <script src="/pms/assets/js/jscolor.js"></script>

<script src="/pms/assets/js/jquery.dataTables.min.js"></script>
<script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>

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
<asp:HiddenField ID="hdntype" runat="server" />
<asp:HiddenField ID="hdntypeproject" runat="server" />
<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
       
        if (document.getElementById('<%=hdntypeproject.ClientID%>').value == "Project")
        {
            document.getElementById("divproject").style.display = "block";
            document.getElementById("divmilestone").style.display = "block";
            document.getElementById("divtask").style.display = "block";
            document.getElementById("divsubtask").style.display = "block";
        }
        else
        {
            document.getElementById("divproject").style.display = "none";
            document.getElementById("divmilestone").style.display = "none";
            document.getElementById("divptasklist").style.display = "none";
            document.getElementById("divtask").style.display = "none";
            document.getElementById("divsubtask").style.display = "none";
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
                       // if (confirm('Do you want Leave this page ?')) {   

                           Cleardata();
                            document.getElementById("btnclose").click();
                                
                            return true;
                        //}
                        //else {
                          //  return false;
                        //}
                    }
        }



        function AddNewIssuestatus()
        {
                debugger
                document.getElementById('<%=hdnIssueStatus.ClientID%>').value = "0";
                 document.getElementById("dvnewissuestatus").style.display = "none";
               if (document.getElementById('<%=IssueStatus.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnIssueStatus.ClientID%>').value = "1";
                    document.getElementById("dvnewissuestatus").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewissustatus.ClientID%>').value = "";
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


        function AddNewIssueType()
        {
                debugger
                document.getElementById('<%=hdnIssueType.ClientID%>').value = "0";
                 document.getElementById("dvnewissueTypes").style.display = "none";
               if (document.getElementById('<%=IssueType.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnIssueType.ClientID%>').value = "1";
                    document.getElementById("dvnewissueTypes").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewissutype.ClientID%>').value = "";
                   document.getElementById('<%=txtIssuescolor.ClientID%>').value = "";
               }
        }


         function AddNewReplicability()
        {
                debugger
                document.getElementById('<%=hdnReplicability.ClientID%>').value = "0";
                 document.getElementById("dvnewReplicability").style.display = "none";
               if (document.getElementById('<%=Replicability.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnReplicability.ClientID%>').value = "1";
                    document.getElementById("dvnewReplicability").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtReplicability.ClientID%>').value = "";
                   document.getElementById('<%=txtReplicabilitycolor.ClientID%>').value = "";
               }
         }

      


          function SvaeData(val1A) {
              debugger
             // alert($("[id*=hdiprojid]").val());
              //alert($("[id*=ProjectId]").val());
              //alert($("[id*=Milestoneid]").val());
              //alert($("[id*=tasklistid]").val());
              //alert($("[id*=taskid]").val());
              //alert($("[id*=subtaskid]").val());
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

                        // isssues status save
                        
                        var issustatusid = "0";
                        if (document.getElementById('<%=hdnIssueStatus.ClientID%>').value != "" && document.getElementById('<%=hdnIssueStatus.ClientID%>').value != "0" && document.getElementById('<%=txtnewissustatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addissues.aspx/AddIssustatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewissustatus.ClientID%>').value) + ' ,statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        issustatusid = response.d;
                                    

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
                            issustatusid = document.getElementById('<%=IssueStatus.ClientID%>').value;
                        }


                      // Priority type save

                         var Prioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addissues.aspx/AddPrioritytype",
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



                        ////Issu type save

                         var issusTypeid = "0";
                        if (document.getElementById('<%=hdnIssueType.ClientID%>').value != "" && document.getElementById('<%=hdnIssueType.ClientID%>').value != "0" && document.getElementById('<%=txtnewissutype.ClientID%>').value != "" && document.getElementById('<%=txtIssuescolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addissues.aspx/AddIssustype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewissutype.ClientID%>').value) + ' ,Issuescolor: ' + JSON.stringify(document.getElementById('<%=txtIssuescolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        issusTypeid = response.d;
                                    

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
                            issusTypeid = document.getElementById('<%=IssueType.ClientID%>').value;
                        }


                        // replicabilty save //

                         var Replicabilityid = "0";
                        if (document.getElementById('<%=hdnReplicability.ClientID%>').value != "" && document.getElementById('<%=hdnReplicability.ClientID%>').value != "0" && document.getElementById('<%=txtReplicability.ClientID%>').value != "" && document.getElementById('<%=txtReplicabilitycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addissues.aspx/AddReplicability",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtReplicability.ClientID%>').value) + ' ,Replicabilitycolor: ' + JSON.stringify(document.getElementById('<%=txtReplicabilitycolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        Replicabilityid = response.d;
                                    

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
                            Replicabilityid = document.getElementById('<%=Replicability.ClientID%>').value;
                        }


                        // save all details


                        debugger
                        var issuesdetails = {};
                        issuesdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; 
                        issuesdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        issuesdetails.Milestoneid = document.getElementById('<%=Milestoneid.ClientID%>').value; 
                        issuesdetails.tasklistid = document.getElementById('<%=tasklistid.ClientID%>').value; 
                        issuesdetails.taskid = document.getElementById('<%=taskid.ClientID%>').value; 
                        issuesdetails.subtaskid = document.getElementById('<%=subtaskid.ClientID%>').value; 
                        issuesdetails.Issues = document.getElementById('<%=Issues.ClientID%>').value; 
                        issuesdetails.StartDate = document.getElementById('<%=StartDate.ClientID%>').value; 
                        issuesdetails.EndDate = document.getElementById('<%=EndDate.ClientID%>').value; 
                        issuesdetails.Owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        issuesdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                        issuesdetails.Issueon = $("[id*=hdnprojecttype]").val(); 
                        issuesdetails.project_id = $("[id*=hdiprojid]").val();
                        issuesdetails.Description = document.getElementById('<%=Description.ClientID%>').value; 
                        issuesdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 
                        issuesdetails.ReminderType = document.getElementById('<%=ReminderType.ClientID%>').value; 
                        issuesdetails.Days = document.getElementById('<%=Days.ClientID%>').value; 
                        issuesdetails.Time = document.getElementById('<%=Time.ClientID%>').value; 
                        issuesdetails.Notifyusers = document.getElementById('<%=hdnNotifyusers.ClientID%>').value; 
                        issuesdetails.ChooseDate = document.getElementById('<%=ChooseDate.ClientID%>').value; 
                        issuesdetails.AssignTo = document.getElementById('<%=AssignTo.ClientID%>').value; 
                        issuesdetails.IssueStatus = issustatusid; 
                        issuesdetails.Priority = Prioritytypeid;
                        issuesdetails.IssueType = issusTypeid; 
                        issuesdetails.Replicability = Replicabilityid; 
                        issuesdetails.UploadImage = document.getElementById('<%=UploadImage.ClientID%>').value; 

                        $.ajax({
                            type: "POST",
                            url: 'Addissues.aspx/SaveIsues',
                            data: '{issuesdetails: ' + JSON.stringify(issuesdetails) + '}',
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
                    //$("[id*=ProjectId]").val("0");
                    //$("[id*=Milestoneid]").val("0");
                    //$("[id*=tasklistid]").val("0");
                    //$("[id*=taskid]").val("0");
                    //$("[id*=subtaskid]").val("0");
                    //$("[id*=Issues]").val("");
                    //$("[id*=StartDate]").val("");
                    //$("[id*=EndDate]").val("");
                    //$("[id*=Description]").val("");
                    //$("[id*=Reminder]").val("0");
                    //$("[id*=ReminderType]").val("0");
                    //$("[id*=Days]").val("");
                    //$("[id*=Time]").val("");
                    //$("[id*=ChooseDate]").val("");
                    //$("[id*=AssignTo]").val("0");
                    //$("[id*=IssueStatus]").val("0");
                    //$("[id*=txtnewissustatus]").val("");
                    //$("[id*=IssueType]").val("0");
                    //$("[id*=txtnewissutype]").val("");
                    //$("[id*=Replicability]").val("0");
                    //$("[id*=txtReplicability]").val("");
                    //$("[id*=UploadImage]").val("");

                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                      document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                      document.getElementById('<%=hdntasklistid.ClientID%>').value = "0";
                      document.getElementById('<%=hdntaskid.ClientID%>').value = "0";
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value = "0";
                    document.getElementById('<%=Issues.ClientID%>').value = "0";
                     document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDate.ClientID%>').value = "";

                      document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=Owner.ClientID%>")).val(0);
                   $(document.getElementById("<%=Owner.ClientID%>")).change();

                    document.getElementById('<%=hdnteam.ClientID%>').value = "";
                     $(document.getElementById("<%=team.ClientID%>")).val(0);
                   $(document.getElementById("<%=team.ClientID%>")).change();

                    document.getElementById('<%=Description.ClientID%>').value = "";
                  document.getElementById('<%=hdnReminder.ClientID%>').value = "0";
                    document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
                     document.getElementById('<%=Days.ClientID%>').value = "";
                    document.getElementById('<%=hdnTime.ClientID%>').value = "";
                     document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=hdnChooseDate.ClientID%>').value = "";
              document.getElementById('<%=hdnAssignTo.ClientID%>').value = "";
              document.getElementById('<%=hdnIssueStatus.ClientID%>').value = "";
              document.getElementById('<%=hdnIssueType.ClientID%>').value = "";
              document.getElementById('<%=txtReplicability.ClientID%>').value = "";
                    document.getElementById('<%=hdnReplicability.ClientID%>').value = "";

                     document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/assets/images/user.jpg";


                       $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyusers.ClientID%>")).change();


                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        Bindissues_data();
                    }
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        //document.getElementById("btncls").click();
                        Bindissues_data();
                    }
                   

               
                    
                }



          function Cleardata() {
                        
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid]").val("0");
                    $("[id*=tasklistid]").val("0");
                    $("[id*=taskid]").val("0");
                    $("[id*=subtaskid]").val("0");
                    $("[id*=Issues]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=EndDate]").val("");
                    $("[id*=Description]").val("");
                    $("[id*=Reminder]").val("0");
                    $("[id*=ReminderType]").val("0");
                    $("[id*=Days]").val("");
                    $("[id*=Time]").val("");
                    $("[id*=ChooseDate]").val("");
                    $("[id*=AssignTo]").val("0");
                    $("[id*=IssueStatus]").val("0");
                    $("[id*=txtnewissustatus]").val("");
                    $("[id*=IssueType]").val("0");
                    $("[id*=txtnewissutype]").val("");
                    $("[id*=Replicability]").val("0");
                    $("[id*=txtReplicability]").val("");
                    $("[id*=UploadImage]").val("");

                      document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                      document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                      document.getElementById('<%=hdntasklistid.ClientID%>').value = "0";
                      document.getElementById('<%=hdntaskid.ClientID%>').value = "0";
                     document.getElementById('<%=hdnsubtaskid.ClientID%>').value = "0";
                     document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                   document.getElementById('<%=hdnEndDate.ClientID%>').value = "";

                 $(document.getElementById("<%=Owner.ClientID%>")).val(0);
                $(document.getElementById("<%=Owner.ClientID%>")).change();

                $(document.getElementById("<%=team.ClientID%>")).val(0);
                $(document.getElementById("<%=team.ClientID%>")).change();

                  document.getElementById('<%=hdnReminder.ClientID%>').value = "0";
              document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
               document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnChooseDate.ClientID%>').value = "";
              document.getElementById('<%=hdnAssignTo.ClientID%>').value = "";
              document.getElementById('<%=hdnIssueStatus.ClientID%>').value = "";
              document.getElementById('<%=hdnIssueType.ClientID%>').value = "";
              document.getElementById('<%=hdnReplicability.ClientID%>').value = "";

               document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/assets/images/user.jpg";


                 $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                $(document.getElementById("<%=Notifyusers.ClientID%>")).change();

                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Issues";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'Addissues.aspx/Bindissuesdetail',
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
                var issuesdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (issuesdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = issuesdetails.find("id").text();
try
{
if (issuesdetails.find("UploadImage").text() != "" && issuesdetails.find("UploadImage").text() != "0")
                    {
                        
                        bindUploadedDrawings(issuesdetails.find("UploadImage").text());
                    }
}
catch
{}
                    document.getElementById('<%=ProjectId.ClientID%>').value = (issuesdetails.find("ProjectId").text());
                    document.getElementById('<%=Milestoneid.ClientID%>').value = (issuesdetails.find("Milestoneid").text());
                    document.getElementById('<%=tasklistid.ClientID%>').value = (issuesdetails.find("tasklistid").text());
                    document.getElementById('<%=taskid.ClientID%>').value = (issuesdetails.find("taskid").text());
                    document.getElementById('<%=subtaskid.ClientID%>').value=(issuesdetails.find("subtaskid").text());
                    document.getElementById('<%=Issues.ClientID%>').value=(issuesdetails.find("Issues").text());
                   <%-- document.getElementById('<%=StartDate.ClientID%>').value=(issuesdetails.find("StartDate").text());
                    document.getElementById('<%=EndDate.ClientID%>').value=(issuesdetails.find("EndDate").text());--%>

                    document.getElementById('<%=hdnowner.ClientID%>').value = issuesdetails.find("Owner").text();
                     document.getElementById('<%=hdnteam.ClientID%>').value = issuesdetails.find("team").text();

                    document.getElementById('<%=Description.ClientID%>').value = (issuesdetails.find("Description").text()); 
                    document.getElementById('<%=Reminder.ClientID%>').value = (issuesdetails.find("Reminder").text()); 
                    document.getElementById('<%=ReminderType.ClientID%>').value = (issuesdetails.find("ReminderType").text()); 
                     document.getElementById('<%=Days.ClientID%>').value = (issuesdetails.find("Days").text()); 
                   <%-- document.getElementById('<%=Time.ClientID%>').value = (issuesdetails.find("Time").text()); --%>
                 <%--   document.getElementById('<%=ChooseDate.ClientID%>').value = (issuesdetails.find("ChooseDate").text()); --%>
                    document.getElementById('<%=AssignTo.ClientID%>').value = (issuesdetails.find("AssignTo").text()); 
                    document.getElementById('<%=IssueStatus.ClientID%>').value = (issuesdetails.find("IssueStatus").text()); 
                    document.getElementById('<%=IssueType.ClientID%>').value = (issuesdetails.find("IssueType").text()); 
                    document.getElementById('<%=Replicability.ClientID%>').value = (issuesdetails.find("Replicability").text()); 
                    <%--document.getElementById('<%=UploadImage.ClientID%>').value=(issuesdetails.find("UploadImage").text()); --%>

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = issuesdetails.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = issuesdetails.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklistid.ClientID%>').value = issuesdetails.find("tasklistid").text();
                    document.getElementById('<%=hdntaskid.ClientID%>').value = issuesdetails.find("taskid").text();
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value = issuesdetails.find("subtaskid").text();
                    document.getElementById('<%=hdnStartDate.ClientID%>').value = issuesdetails.find("StartDate").text();
                    document.getElementById('<%=hdnEndDate.ClientID%>').value = issuesdetails.find("EndDate").text();
                    document.getElementById('<%=hdnReminder.ClientID%>').value = issuesdetails.find("Reminder").text();
                    document.getElementById('<%=hdnReminderType.ClientID%>').value = issuesdetails.find("ReminderType").text();
                    document.getElementById('<%=hdnTime.ClientID%>').value = issuesdetails.find("Time").text();
                    document.getElementById('<%=hdnNotifyusers.ClientID%>').value = issuesdetails.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDate.ClientID%>').value = issuesdetails.find("ChooseDate").text();
                    document.getElementById('<%=hdnAssignTo.ClientID%>').value = issuesdetails.find("AssignTo").text();
                    document.getElementById('<%=hdnIssueStatus.ClientID%>').value = issuesdetails.find("IssueStatus").text();
                     document.getElementById('<%=hdnPriority.ClientID%>').value = issuesdetails.find("Priority").text();
                    document.getElementById('<%=hdnIssueType.ClientID%>').value = issuesdetails.find("IssueType").text();
                    document.getElementById('<%=hdnReplicability.ClientID%>').value = issuesdetails.find("Replicability").text();

                    // if (issuesdetails.find("UploadImage").text() != "")
                    //{
                      //  document.getElementById("empimg").src = "/assets/images/" + issuesdetails.find("UploadImage").text();
                       // document.getElementById("imgpath").href = "/assets/images/" + issuesdetails.find("UploadImage").text();
                   // }
                   // else
                   // {
                      //  document.getElementById("imgpath").href = "#";
                      //  document.getElementById("empimg").src = "/assets/images/user.jpg";
                  //  }


                      // time and reminder fill and edit strt//

                     if (issuesdetails.find("Reminder").text() == "0")
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


                    
              if (issuesdetails.find("Owner").text() != "")
             {
             var idsemp = issuesdetails.find("Owner").text().split(',');
            $(document.getElementById("<%=Owner.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Owner.ClientID%>")).change();
            }


             if (issuesdetails.find("team").text() != "")
             {
             var idsemp = issuesdetails.find("team").text().split(',');
            $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=team.ClientID%>")).change();
            }


              if (issuesdetails.find("Notifyusers").text() != "")
             {
             var idsemp = issuesdetails.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }
                  
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Issues";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       



    <script>
        $( document ).ready(function() {
            Bindissues_data();
});
         function Bindissues_data() {

               //  alert($("[id*=hdiprojid]").val());
               //alert($("[id*=hdnmil_stoneid]").val());
               // alert($("[id*=hdntask_id]").val());
               // alert($("[id*=hdnsubtask_id]").val());
               // alert($("[id*=hdnprojecttype]").val());

                debugger;
                  $.ajax({
                    url: 'Addissues.aspx/bindIssuesdata',                  
                     // data: '{}',

                       //  data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "','issuetype': '" + $("[id*=hdnprojecttype]").val() + "' }",
                       data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "','milstoneid': '" + $("[id*=hdnmil_stoneid]").val() + "','taskid': '" + $("[id*=hdntask_id]").val() + "','subtaskid': '" + $("[id*=hdnsubtask_id]").val() + "' ,'issuetype': '" + $("[id*=hdnprojecttype]").val() + "' }",
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
                scrollX: false,
        scrollCollapse: true,
        paging:         true,
                searching: true,

                fixedColumns: {
            leftColumns: 2
        }
        
            });


            hidecolumngrid();
           
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
                    url: 'Addissues.aspx/Deleteissuesdata',
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
            Bindissues_data();
        }

    </script>

  

    <%--save data end  --%>



 <%--Owner details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeDataowner(val12A) {

            if (Page_ClientValidate("btnsaveowner")) {
               
                debugger


                // save all details

                var issuesdetails = {};
                issuesdetails.id = document.getElementById('<%=hdnchangeid.ClientID%>').value;
                issuesdetails.owner = document.getElementById('<%=hdncahngeowner.ClientID%>').value;

                    $.ajax({
                    type: "POST",
                    url: 'Addissues.aspx/Save_Owner',
                    data: '{issuesdetails: ' + JSON.stringify(issuesdetails) + '}',
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
                   // Bindissues_data();
                    document.getElementById("lblowner_" + document.getElementById("<%=hdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                    
                }

        

          function Binddetails_Owner(RecordId,lblownerid) {
              
              debugger;
               document.getElementById("<%=hdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
            
                $.ajax({
                    url: 'Addissues.aspx/Binddetail_Owner',
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
                var issuesdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (issuesdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = issuesdetails.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = issuesdetails.find("Owner").text();

               if (issuesdetails.find("Owner").text() != "")
             {
               var idsemp = issuesdetails.find("Owner").text().split(',');
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

                var issuesdetails = {};
                issuesdetails.id = document.getElementById('<%=hdnteameid.ClientID%>').value;
                issuesdetails.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;
                $.ajax({
                    type: "POST",
                    url: 'Addissues.aspx/Save_Team',
                    data: '{issuesdetails: ' + JSON.stringify(issuesdetails) + '}',
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
                  //  Bindissues_data();
                     document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Team(RecordId,lbluserid) {

          
                debugger;
               document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];

                $.ajax({
                    url: 'Addissues.aspx/Bindetail_Team',
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
                var issuesdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (issuesdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = issuesdetails.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = issuesdetails.find("team").text();

               if (issuesdetails.find("team").text() != "")
             {
               var idsemp = issuesdetails.find("team").text().split(',');
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
    <asp:HiddenField ID="hdissuesid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid)
        {
            debugger
                   var issuesid=document.getElementById("<%=hdissuesid.ClientID%>").value;
                    //alert(issuesid);
                    $.ajax({
                    url: 'Addissues.aspx/Update_Status',
                    data: "{ 'issuesid': '" + issuesid + "' ,'statusid': '" + statusid + "' }",
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
           // Bindissues_data();
            var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setissustatusid(issuesid,btnststusid)
        {
            debugger
             document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdissuesid.ClientID%>").value = issuesid;
        }
            </script>


      <asp:HiddenField ID="hdnpropertID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var issuesid=document.getElementById("<%=hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addissues.aspx/UpdatePriority_Status',
                    data: "{ 'issuesid': '" + issuesid + "' ,'Propertyid': '" + Propertyid + "' }",
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
           // Bindissues_data();
            var btnid = document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setpriorityid(issuesid,btnpriortyid)
        {
            debugger
            document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=hdnpropertID.ClientID%>").value = issuesid;
        }
            </script>

    <%--status and  proprety update end--%>

<%--issues and replicable  update start--%>


      <asp:HiddenField ID="hdn_issuid" runat="server" Value="0" />
    <asp:HiddenField ID="hdn_btnissutypeid" runat="server" Value="0" />
 
    <script type="text/javascript">
             
        function UpdateDataIssuetypedeatils(IssueTypeid)
        {
            debugger
                   var issuesid=document.getElementById("<%=hdn_issuid.ClientID%>").value;
                    //alert(issuesid);
                    $.ajax({
                    url: 'Addissues.aspx/Update_Issutype',
                    data: "{ 'issuesid': '" + issuesid + "' ,'IssueTypeid': '" + IssueTypeid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessIssutypedt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessIssutypedt(response) {
            alert('Data Saved successfully.');
           // Bindissues_data();
            var btnissutyeid = document.getElementById("<%=hdn_btnissutypeid.ClientID%>").value; 
            var arrcolissue = response.d.split("~~");
            document.getElementById(btnissutyeid).innerHTML = arrcolissue[0];
            document.getElementById(btnissutyeid).style.backgroundColor = arrcolissue[1];
        }
        
        function setissutypestatusid(issuesid,btnIssutypeidid)
        {
            debugger
             document.getElementById("<%=hdn_btnissutypeid.ClientID%>").value = btnIssutypeidid;
            document.getElementById("<%=hdn_issuid.ClientID%>").value = issuesid;
        }
            </script>


    

 <asp:HiddenField ID="hdnissu_repID" runat="server" Value="0" />
    <asp:HiddenField ID="hdn_btn_replicableid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataReplicabledeatils(Replicableidid)
        {
                   debugger
                   var issuesid=document.getElementById("<%=hdnissu_repID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addissues.aspx/UpdateReplicable',
                    data: "{ 'issuesid': '" + issuesid + "' ,'Replicableidid': '" + Replicableidid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessReplicableupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessReplicableupdt(response) {
            alert('Data Saved successfully.');
           // Bindissues_data();
            var btnrepid = document.getElementById("<%=hdn_btn_replicableid.ClientID%>").value; 
            var arrcolreplicable = response.d.split("~~");
            document.getElementById(btnrepid).innerHTML = arrcolreplicable[0];
            document.getElementById(btnrepid).style.backgroundColor = arrcolreplicable[1];
        }
        
        function setreplicableid(issuesid,btnreplicableid)
        {
            debugger
            document.getElementById("<%=hdn_btn_replicableid.ClientID%>").value = btnreplicableid;
            document.getElementById("<%=hdnissu_repID.ClientID%>").value = issuesid;
        }
            </script>


  <%--issues and replicable update end--%>




    <%--Multi dropdown select start --%>


 <script>
    debugger
    var ddLOwner = $(document.getElementById("<%=Owner.ClientID%>"));
     $(ddLOwner).select2({
        data: [
    <%=MultiOwner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        width: "100%"
    });

   
     $(document.getElementById("<%=Owner.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=Owner.ClientID%>")).val();
        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
        document.getElementById("<%=Owner.ClientID%>").click();

    });
   
  
    </script>
    

    <script>
    debugger
    var ddLTeam = $(document.getElementById("<%=team.ClientID%>"));
    $(ddLTeam).select2({
        data: [
    <%=Multi_team%>
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
    var ddLtifyusers = $(document.getElementById("<%=Notifyusers.ClientID%>"));
   

    $(ddLtifyusers).select2({
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

    <%--Multi dropdown select start end--%> 


 <%-- image upload start--%>


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
                url: "FileUploadHandlerIssues.ashx?id="+id+"", 
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
                    return;
                }
            }  
        }  
</script>
<script>
        

      
function ShowPreviewnew() {
    debugger;

    var input = document.getElementById("<%= fileupload.ClientID %>");
    var previewContainer = document.getElementById("previewContainer1");
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
    var basePath = "/pms/IssueImages/";
    var previewContainer = document.getElementById("previewContainer1");
    
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
    <%-- image upload end--%>


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
                        url: 'Addissues.aspx/Savecolumndata',
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
            
            for (var i = 0; i < 16; i++)
            {  
              
                var aa = "ContentPlaceHolder1_Issues_hdn" + i; 
                if (document.getElementById(aa).value == "0")
                { 
                    
                    let column = table.column(i); 
                    column.visible(false);

                }
            }
          
            document.getElementById("divgridview").style.display = "block";
            document.getElementById("divgridview").style.width = "100%";
            document.getElementById("ContentPlaceHolder1_Issues_lblprojectdata").style.width = "100%";
           
        }
    </script>

 <%-- show column hide show end--%>
