<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Milestone.ascx.cs" Inherits="ucontrol_Milestone" %>
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
        .files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
    </style>   
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico green">
                                            <i class="ri-flag-2-line"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p><%=fns.getltext("Milestone", "2")%></p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside" style="display:none;">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Milestone</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end" style="display:none;">
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetMilstonstatus(0)"><span class="align-middle">All Milestone</span></a> 
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetMilstonstatus(1)"><span class="align-middle">Status 1</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetMilstonstatus(2)"><span class="align-middle">Status 2</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetMilstonstatus(3)"><span class="align-middle">Status 3</span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                 
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                       <%if (ischeckCreate == 1)
                                           {%>
                                          
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addmilestone"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                        <%}%>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">


                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>
                                           
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Project", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>

                                             <a class="dropdown-item toggle-vis" runat="server"  id="mile4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timer", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timer History", "2")%></a>

                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Weightage", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile10" data-column="10"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Extended timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Start Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("End Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile15" data-column="15"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile16" data-column="16"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile17" data-column="17"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issue", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="mile18" data-column="18"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("File", "2")%></a>

                                        </div>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                                        <%--<div class="dropdown-menu">
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

                                 <div id="divgridview" style="width:100%;">
                            <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                                </div>

                             <div style="display:none;">
                              <asp:Button ID="btndelete" runat="server"    Text="delete"  />
                             </div>

                                
                            </div>
                        </div>
                      
        

<div class="offcanvas offcanvas-end" id="addmilestone" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
          <label id="lbltitle" runat="server"><%=fns.getltext("Add Milestone", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fs12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fse12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
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
                                <div class="col-xxl-12 col-lg-12" id="divproject">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectId" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                    <%--onchange="GetWeitagebindDtls()"--%>
                                      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectId" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" onchange="GetWeitagebindDtls()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectId" runat="server" Value="0" />
                                              <asp:HiddenField ID="hdnweightage" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Milestone" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Milestone" placeholder="Milestone" runat="server"  class="form-control">
                                </div>


                             <%-- <div class="card-body" id="divprojecttimeline" style="display:none;">
                            <div class="table-responsive">
                            <asp:Label ID="projecttimeline" runat="server"></asp:Label>
                             </div>
                           </div>--%>

                                <div class="col-xxl-12 col-lg-12" id="divprojecttimeline" style="display:none;">
                                 <asp:Label ID="projecttimeline" runat="server"></asp:Label>
                                 </div>


                            <%-- <div class="card-body" id="divmiletimeline" style="display:none;">
                            <div class="table-responsive">
                            <asp:Label ID="lblmiletimeline" runat="server"></asp:Label>
                            </div>
                           </div>--%>

                                 <div class="col-xxl-12 col-lg-12" id="divmiletimeline" style="display:none;">
                                  <asp:Label ID="lblmiletimeline" runat="server"></asp:Label>
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


                              


                               
                                <div class="col-xxl-12 col-lg-12" id="divweitage">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchk(this);" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                       <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                     
                                             <%--<input  id="StartDate" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate(this.id)">--%>
                                             <input type="text" id="StartDate" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                         <asp:HiddenField ID="hdnStartDate" runat="server" Value="0" />
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

                                      <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input  id="EndDate" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate(this.id)">--%>
                                             <input type="text" id="EndDate" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                    
                                            
                                             <asp:HiddenField ID="hdnEndDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>

                                      <asp:DropDownList ID="Owner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
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


                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="P_Status" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Status" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
                                                </cc1:DropDownListChosen>
                                              <div id="dvnewprojectstatus" style="display:none">
                                          <input type="text" id="txtnewprojectstatus" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Status" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
                                         </cc1:DropDownListChosen>
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

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



                                <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Access", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Access" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>
                                  
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
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Duration Type", "2")%>  &nbsp;&nbsp;
                                            
                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                    <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" class="form-control">
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
  <%--  <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
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

<div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>

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
function Binddetails_D(mid,ctrlid)
{
 debugger
                  $.ajax({
                    url: '/pms/Addmilestone.aspx/Binddependencydata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'mid': '" +mid+ "'}",
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
                          <%--  <select id="select_emp1" class="form-control" multiple="multiple">
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
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
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
        </div>
    </div>
</div>


<div id="ExtendedTimline" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button2" runat="server" Text="" OnClick="Button2_Click" />
   </div>
                      <asp:HiddenField ID="hdnextendid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input id="Extendatestrt" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnextndstrt" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input id="Extendateend" runat="server" class="form-control">
                                              <asp:HiddenField ID="hdnextndend" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveextended" class="btn btn-primary" onclick="SvaeExtended('saveExtended');">Save</button>
            </div>
        </div>
    </div>
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
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
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



<link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
<%-- <script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
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

    <%--<script>
    const table = new DataTable('.datatable', {  
        scrollX:  false,
        scrollCollapse: true,
        paging:         true,
        searching: true
        
    });
</script>--%>


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
<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
        document.getElementById("divproject").style.display = "none";


          var RecordId=document.getElementById("<%=ProjectId.ClientID%>").value;
         
         $.ajax({
                    url: 'Addmilestone.aspx/Bindcheckweitagedetails',
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
                      if (waitagechkdtls.find("checkprojectWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkprojectWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }

            }
            catch

            {

            }


         //Owner Bind Project Wise;
         debugger
        var RecordId = document.getElementById("<%=hdnProjectId.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
       }
        $.ajax({
            url: 'Addmilestone.aspx/bindMultiOwner_new',
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

                                                         var ddLOwner = $(document.getElementById("<%=Owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
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
        var RecordId = document.getElementById("<%=hdnProjectId.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
       }
        $.ajax({
            url: 'Addmilestone.aspx/bindMultiTeam_new',
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



        // project tomeline //
           debugger
            document.getElementById("divprojecttimeline").style.display = "block";
            var projec_id = document.getElementById("<%=ProjectId.ClientID%>").value;
             debugger;
                  $.ajax({
                      url: 'Addmilestone.aspx/bindProjecttimelinedata',
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

        
        function OnSuccessproject_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=projecttimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }





       //  Milestone Timeline //
              debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var projec_id = document.getElementById("<%=ProjectId.ClientID%>").value;
              debugger;
                  $.ajax({
                      url: 'Addmilestone.aspx/bindMiletimelinedata',
                    data: "{ 'projec_id': '" + projec_id + "'}",
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
        
      }
    

</script>





<script>
   function Getempdetails() {
        debugger
        var RecordId = document.getElementById("<%=hdnProjectId.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
       }
        $.ajax({
            url: 'Addmilestone.aspx/bindMultiOwner_new',
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

                                                         var ddLOwner = $(document.getElementById("<%=Owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
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
        var RecordId = document.getElementById("<%=hdnProjectId.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
       }
        $.ajax({
            url: 'Addmilestone.aspx/bindMultiTeam_new',
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
        debugger
                  var RecordId=document.getElementById("<%=ProjectId.ClientID%>").value;

                  $.ajax({
                    url: 'Addmilestone.aspx/Bindcheckweitagedetails',
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
                      if (waitagechkdtls.find("checkprojectWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkprojectWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
                
            }
            catch

            {

            }

            Getempdetails();
            GetTeamempdetails();

            document.getElementById("divprojecttimeline").style.display = "block";
            Bindprojecttimeline_data();
             document.getElementById("divmiletimeline").style.display = "block";
            Bindmilestonetimeline_data();
          
         }


       
    }
    

</script>


<%--<script>
  function handleChange(input) {
    if (input.value < 0) input.value = 0;
    if (input.value > 100) input.value = 100;
  }
</script>--%>


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
function checkdatecomparisiontaskdate1(enterdate) {
        debugger
        if (enterdate != "") {
            var strdt;
            if (document.getElementById(enterdate).value.includes("/")) {
                strdt = document.getElementById(enterdate).value.split("/");
            }
            if (document.getElementById(enterdate).value.includes("-")) {
                strdt = document.getElementById(enterdate).value.split("-");
            }
            var fdt = new Date(document.getElementById(enterdate).value);
            var date = new Date();

            var fcompdate = new Date();
            //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
            if (document.getElementById(enterdate).value.includes("-")) {
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
            }
            else {
                fcompdate = new Date(strdt[2], parseInt(strdt[1]) - 1, strdt[0])
            }
            var tcompdate = new Date();
            tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())
            if (fcompdate < tcompdate) {
                alert("Enter date should not be less then current date!!");
                document.getElementById(enterdate).value = "";
                document.getElementById(enterdate).focus();
                return false;
            }
            else {
                     if (document.getElementById("<%=EndDate.ClientID%>").value!= "") 
                    {
                      checkdatecomparisiontaskdate2('ContentPlaceHolder1_Milestone_EndDate');
                    }
                    else
                    {
                     return true
                      }
               }
        }
            }
function checkdatecomparisiontaskdate2(enterdate)
            {
                debugger
                 var fcompenddate = new Date();
                if (document.getElementById("<%=StartDate.ClientID%>").value == "") {
                    alert("Please select start date!!");
                    document.getElementById(enterdate).value = "";
                    document.getElementById("<%=StartDate.ClientID%>").focus();
                    return false;
                }
                else
                {
             var strenddt;
            if (document.getElementById("<%=StartDate.ClientID%>").value.includes("/")) {
                strenddt = document.getElementById("<%=StartDate.ClientID%>").value.split("/");
            }
           else if (document.getElementById("<%=StartDate.ClientID%>").value.includes("-")) {
                strenddt = document.getElementById("<%=StartDate.ClientID%>").value.split("-");
            }
           
            //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
            if (document.getElementById("<%=StartDate.ClientID%>").value.includes("-")) {
                fcompenddate = new Date(strenddt[0], parseInt(strenddt[1]) - 1, strenddt[2])
            }
            else {
                fcompenddate = new Date(strenddt[2], parseInt(strenddt[1]) - 1, strenddt[0])
            }
          }
        if (enterdate != "") {
            var strdt;
            if (document.getElementById(enterdate).value.includes("/")) {
                strdt = document.getElementById(enterdate).value.split("/");
            }
            if (document.getElementById(enterdate).value.includes("-")) {
                strdt = document.getElementById(enterdate).value.split("-");
            }
            var fdt = new Date(document.getElementById(enterdate).value);
            var date = new Date();

            var fcompdate = new Date();
            //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
            if (document.getElementById(enterdate).value.includes("-")) {
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
            }
            else {
                fcompdate = new Date(strdt[2], parseInt(strdt[1]) - 1, strdt[0])
            }
            var tcompdate = new Date();
            tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())
            if (fcompdate < tcompdate) {
                alert("Enter date should not be less then current date!!");
                document.getElementById(enterdate).value = "";
                document.getElementById(enterdate).focus();
                return false;
            }           
            else
            {
                if (fcompdate < fcompenddate)
                {
                    alert("End date should not be less then start date date!!");
                    document.getElementById(enterdate).value = "";
                    document.getElementById(enterdate).focus();
                    return false;
                }
            else
            {
                    return true
                }
            }
        }
}

    function checkdate(enterdate)
    {
        if (document.getElementById("<%=StartDate.ClientID%>").value != "" && document.getElementById("<%=EndDate.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=StartDate.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=EndDate.ClientID%>").value);
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
var strdt;
            if (checkdate(enterdate)) {
                //var strdt = document.getElementById(enterdate).value.split("/");
                 if (document.getElementById(enterdate).value.includes("/")) {
                strdt = document.getElementById(enterdate).value.split("/");
            }
            if (document.getElementById(enterdate).value.includes("-")) {
                strdt = document.getElementById(enterdate).value.split("-");
            }


var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

                var projec_id = document.getElementById("<%=ProjectId.ClientID%>").value;



                $.ajax({
                    url: 'Addmilestone.aspx/Bindcheckstrtdateprojectdetails',
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
    function checkdatecomparisiontaskdateold(enterdate) {
        debugger
        if (enterdate != "") {

            if (checkdate(enterdate)) {
                var strdt = document.getElementById(enterdate).value.split("/");
                var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

                var projec_id = document.getElementById("<%=ProjectId.ClientID%>").value;



                $.ajax({
                    url: 'Addmilestone.aspx/Bindcheckstrtdateprojectdetails',
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
                            const today = new Date(convertDate(milestonestrtdatechkdtls.find("StartDate").text()));
                            var fdt = new Date(today);
                            const today2 = new Date(convertDate(milestonestrtdatechkdtls.find("TargetDate").text()));
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

            //if (fcompdate < tcompdate) {
            //    alert("Enter Valid Date");
            //    document.getElementById(enterdate).value = "";
            //    document.getElementById(enterdate).focus();
            //    return false;
            //}
            //else {
            //    return true
            //}
        }
    }

    
    
function convertDate(input) {
debugger
        // Input expected: '07-22-205' => mm-dd-yyy (but year is 3 digits)
       // var parts = input.split('-');

        //if (parts.length !== 3) {
           // return 'Invalid date';
       // }

        //var month = parts[0].padStart(2, '0');
        //var day = parts[1].padStart(2, '0');
        //var year = parts[2];

         //if (year.length === 3) {
           // year = '2' + year; 
      //  }

        //return `${day}/${month}/${year}`;
 var date = new Date(input);

        // Extract day, month, year
        var day = date.getDate().toString().padStart(2, '0');
        var month = (date.getMonth() + 1).toString().padStart(2, '0'); // getMonth() is 0-based
        var year = date.getFullYear();

        return `${day}/${month}/${year}`;
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



<asp:HiddenField ID="hdnsavetype" runat="server" />

    <%--save data start --%> 

    <script type="text/javascript">
                function showdivimg() {
                    if (Page_ClientValidate("btnclose")) {
                        //if (confirm('Do you want Leave this page ?')) {   

                            Cleardata();
document.getElementById('btnsaveandmore').style.display='';
                            document.getElementById("btnclose").click();
                                
                            return true;
                        //}
                        //else {
                           // return false;
                       // }
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




                      // Project status save
                        
                        var milstonestatusid = "0";
                        if (document.getElementById('<%=hdnP_Status.ClientID%>').value != "" && document.getElementById('<%=hdnP_Status.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addmilestone.aspx/AddAddMilestonetatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatus.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milstonestatusid = response.d;
                                    

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
                            milstonestatusid = document.getElementById('<%=P_Status.ClientID%>').value;
                        }

                        // Priority type save

                         var milePrioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addmilestone.aspx/AddMilestonePrioritytype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewpriority.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milePrioritytypeid = response.d;
                                    

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
                            milePrioritytypeid = document.getElementById('<%=Priority.ClientID%>').value;
                        }


                        // save all details

                        
                        var milestonedetails = {};
                        milestonedetails.id = document.getElementById('<%=hdnid.ClientID%>').value; 
                        milestonedetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        milestonedetails.Milestone = document.getElementById('<%=Milestone.ClientID%>').value; 
                        milestonedetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value; 
                        milestonedetails.StartDate = document.getElementById('<%=StartDate.ClientID%>').value; 
                        milestonedetails.EndDate = document.getElementById('<%=EndDate.ClientID%>').value; 
                       milestonedetails.Owner = document.getElementById('<%=hdnowner.ClientID%>').value;
                       milestonedetails.Access = document.getElementById('<%=Access.ClientID%>').value; 
                        milestonedetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                        milestonedetails.P_Status = milstonestatusid;
                        milestonedetails.Priority = milePrioritytypeid;
                        milestonedetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        milestonedetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;


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
                   milestonedetails.TaskDuration = document.getElementById('<%=taskDuration.ClientID%>').value; 

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
 UploadFiles(response.d);
                    alert("Data Saved successfully."); 

                    document.getElementById('<%=btnload.ClientID%>').click();
try
{
 document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/pms/assets/images/user.jpg";
                   
}
catch
{}

                   document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                    document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
                    document.getElementById('<%=StartDate.ClientID%>').value = "";
                    document.getElementById('<%=EndDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDate.ClientID%>').value ="";
                    
                      document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=Owner.ClientID%>")).val(0);
                   $(document.getElementById("<%=Owner.ClientID%>")).change();

                     $(document.getElementById("<%=team.ClientID%>")).val(0);
                    $(document.getElementById("<%=team.ClientID%>")).change();
                    document.getElementById('<%=hdnAccess.ClientID%>').value = "0";
                     document.getElementById('<%=hdnteam.ClientID%>').value = "";
                    document.getElementById('<%=P_Status.ClientID%>').value = "0";
                    document.getElementById('<%=Priority.ClientID%>').value = "0";
                    document.getElementById('<%=Budget.ClientID%>').value = "0";
                    document.getElementById('<%=Actual.ClientID%>').value = "0";
                    document.getElementById('<%=istask.ClientID%>').checked = 0;
                    document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;
                    <%--alert(document.getElementById('<%=hdnsavetype.ClientID%>').value);--%>

                    document.getElementById('<%=taskDuration.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";

                     document.getElementById('<%=hdnid.ClientID%>').value = "0";
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        Bindmilestone_data();
                    }
                    
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                       // document.getElementById("btncls").click();
                        Bindmilestone_data();
                    }
                   

               
                    
                }



          function Cleardata() {
                        
                   //$("[id*=ProjectId]").val("0");
                   //$("[id*=Milestone]").val("");
                   // $("[id*=Weightage]").val("");
                   // $("[id*=StartDate]").val("");
                   // $("[id*=EndDate]").val("");
                   // $("[id*=Owner]").val("0");
                   // $("[id*=Access]").val("0");
                   // $("[id*=team]").val("");
                   // $("[id*=P_Status]").val("0");
                   // $("[id*=Priority]").val("0");
                   // $("[id*=Budget]").val("");
                   // $("[id*=Actual]").val("");

              document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                  document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
                    document.getElementById('<%=StartDate.ClientID%>').value = "";
                    document.getElementById('<%=EndDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDate.ClientID%>').value ="";
                    document.getElementById('<%=hdnowner.ClientID%>').value ="";
                   document.getElementById('<%=hdnAccess.ClientID%>').value = "";
                   document.getElementById('<%=hdnteam.ClientID%>').value = "";
                   document.getElementById('<%=hdnP_Status.ClientID%>').value = "0";
                   document.getElementById('<%=hdnPriority.ClientID%>').value = "0";
                   document.getElementById('<%=istask.ClientID%>').checked = 0;
                 document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;

                   document.getElementById('<%=taskDuration.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";


                  $(document.getElementById("<%=Owner.ClientID%>")).val(0);
                 $(document.getElementById("<%=Owner.ClientID%>")).change();

                  $(document.getElementById("<%=team.ClientID%>")).val(0);
                 $(document.getElementById("<%=team.ClientID%>")).change();

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
try
{
if (milestonedetails.find("Uploaddrawing").text() != "" && milestonedetails.find("Uploaddrawing").text() != "0")
                    {
                        bindUploadedDrawings(milestonedetails.find("Uploaddrawing").text());
                       // document.getElementById("empimg").src = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                        //document.getElementById("imgpath").href = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    }
}
catch
{}
                    document.getElementById('<%=hdnid.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=ProjectId.ClientID%>').value = (milestonedetails.find("ProjectId").text());

                     document.getElementById('<%=Milestone.ClientID%>').value=(milestonedetails.find("Milestone").text());
                    document.getElementById('<%=Weightage.ClientID%>').value=(milestonedetails.find("Weightage").text());
                    document.getElementById('<%=StartDate.ClientID%>').value=(milestonedetails.find("StartDate").text());
                    document.getElementById('<%=EndDate.ClientID%>').value=(milestonedetails.find("EndDate").text());
                    document.getElementById('<%=hdnowner.ClientID%>').value=(milestonedetails.find("Owner").text());
                    document.getElementById('<%=Access.ClientID%>').value = (milestonedetails.find("Access").text()); 
                    
                    document.getElementById('<%=Budget.ClientID%>').value = (milestonedetails.find("Budget").text()); 
                    document.getElementById('<%=Actual.ClientID%>').value = (milestonedetails.find("Actual").text()); 

                    document.getElementById('<%=P_Status.ClientID%>').value = milestonedetails.find("P_Status").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = milestonedetails.find("P_Status").text();
                        document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";


                    document.getElementById('<%=Priority.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";

                     document.getElementById('<%=hdnteam.ClientID%>').value = milestonedetails.find("team").text();

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = milestonedetails.find("ProjectId").text();
                     document.getElementById('<%=hdnStartDate.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=hdnEndDate.ClientID%>').value = milestonedetails.find("EndDate").text();
                    document.getElementById('<%=hdnAccess.ClientID%>').value = milestonedetails.find("Access").text();


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
                    document.getElementById('<%=taskDuration.ClientID%>').value = milestonedetails.find("TaskDuration").text();

                    
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
                     document.getElementById("divmiletimeline").style.display = "block";
                    Bindmilestonetimeline_data();

                  //  WeitageChecked();
                  //GetWeitagebindDtls();
                    document.getElementById('<%=Button1.ClientID%>').click();
                    Getempdetails();

              if (milestonedetails.find("Owner").text() != "")
              {
                  debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=Owner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=Owner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
                $(document.getElementById("<%=Owner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=Owner.ClientID%>")).change();
                  }
             
            }

              GetTeamempdetails();
             if (milestonedetails.find("team").text() != "")
             {
                 debugger
                 if (milestonedetails.find("team").text().includes(','))
                 {
                      var idsemp = milestonedetails.find("team").text().split(',');
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = milestonedetails.find("team").text();
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }



            }
                  
                     
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Milestone";
                   document.getElementById('btnsaveandmore').style.display='none';
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




        $( document ).ready(function() {
            Bindmilestone_data();
});
         function Bindmilestone_data() {
             debugger
            
             //alert($("[id*=hdiprojid]").val());

                debugger;
                  $.ajax({
                      url: 'Addmilestone.aspx/bindMilestonedata',
                      data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
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

               try {
            table = new DataTable('.datatable', {
         scrollX: true,
        scrollY:  300,
        scrollCollapse: true,
        paging: true,
                searching: true,


                fixedColumns: {
            leftColumns: 4
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

<asp:HiddenField ID="hdnpdates" runat="server" />

<script>
    
    $(document).ready(function ()
    {
            Bindprojecttimeline_data();
});
    function Bindprojecttimeline_data()
    {
             debugger
            
             var projec_id = document.getElementById("<%=ProjectId.ClientID%>").value;
             //alert(projec_id);
              debugger;
                  $.ajax({
                      url: 'Addmilestone.aspx/bindProjecttimelinedata',
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
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
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
            
             
             var projec_id = document.getElementById("<%=ProjectId.ClientID%>").value;
             //alert(projec_id);


                debugger;
                  $.ajax({
                      url: 'Addmilestone.aspx/bindMiletimelinedata',
                    data: "{ 'projec_id': '" + projec_id + "'}",
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
           
    //        table = new DataTable('.datatable', {
    //     scrollX: true,
    //    scrollY:  300,
    //    scrollCollapse: true,
    //    paging: true,
    //    searching: true,
        
    //});
           
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



<%--Owner details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeDataowner(val12A) {

            if (Page_ClientValidate("btnsaveowner")) {
               

                //if (confirm('Are You Sure You want to Submit This Entry!'))
                //{
                    

                //}
                //else
                //{
                //    return false;
                //}
                debugger


                // save all details

                var milestonedetails = {};
                milestonedetails.id = document.getElementById('<%=hdnchangeid.ClientID%>').value;;
                milestonedetails.Owner = document.getElementById('<%=hdncahngeowner.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/SaveMilestone_Owner',
                    data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
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
                   // Bindmilestone_data();
                     document.getElementById("lblowner_" + document.getElementById("<%=hdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Owner(RecordId,lblownerid) {
              
              debugger;
              document.getElementById("<%=hdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
                 
                $.ajax({
                    url: 'Addmilestone.aspx/BindpMilestonedetail_Owner',
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
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "")
                {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = milestonedetails.find("Owner").text();
                    document.getElementById('<%=multichngprojectid.ClientID%>').value = milestonedetails.find("ProjectId").text();
            

              Multichangeowner();
              if (milestonedetails.find("Owner").text() != "")
              {
                  debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
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
               

                //if (confirm('Are You Sure You want to Submit This Entry!'))
                //{
                    

                //}
                //else
                //{
                //    return false;
                //}
                debugger


                // save all details

                var milestonedetails = {};
                milestonedetails.id = document.getElementById('<%=hdnteameid.ClientID%>').value;;
                milestonedetails.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/SaveProject_Team',
                    data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
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
                  //  Bindmilestone_data();
                   document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                    
                }

        

          function Binddetails_Team(RecordId,lbluserid) {
              
              debugger;

               document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                 
                $.ajax({
                    url: 'Addmilestone.aspx/Bindmilestonedetail_Team',
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
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = milestonedetails.find("team").text();
                    

              document.getElementById('<%=multichngprojectid.ClientID%>').value = milestonedetails.find("ProjectId").text();
              MultichangeTeam();
              if (milestonedetails.find("team").text() != "")
              {
                  debugger
                  if (milestonedetails.find("team").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("team").text().split(',');
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("team").text();
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


   <%--status update start--%>
    <asp:HiddenField ID="hdnmilstntid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
            var milstoneid=document.getElementById("<%=hdnmilstntid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addmilestone.aspx/UpdateMilestone_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'milstoneid': '" + milstoneid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
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
                var arrcol = response.d.split("~~");
                alert('Data Saved successfully.');
             var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value;
                document.getElementById(btnid).innerHTML = arrcol[0];
                document.getElementById(btnid).style.backgroundColor = arrcol[1];
            }
            else {
                alert(response.d);
            }
            
        }
        <%--function OnSuccessStatusupdt(response) {
            alert('Data Saved successfully.');
            Bindmilestone_data();
             var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }--%>
        
        function setmilsestoneid(milstoneid,btnststusid)
        {
            debugger
            document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdnmilstntid.ClientID%>").value = milstoneid;
        }
            </script>

 <%--   status update end --%>


   <%-- property update start--%>

      <asp:HiddenField ID="hdnpropertID" runat="server" Value="0" />
      <asp:HiddenField ID="hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var milstoneid=document.getElementById("<%=hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addmilestone.aspx/UpdatePriority_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'milstoneid': '" + milstoneid + "' ,'Propertyid': '" + Propertyid + "' }",
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
           // Bindmilestone_data();
            var btnid = document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setpriorityid(milston_id,btnpriortyid)
        {
            debugger
             document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=hdnpropertID.ClientID%>").value = milston_id;
        }
            </script>

    <%--proprety update end--%>


 <%--Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeExtended(val125A) {

            if (Page_ClientValidate("btnsaveextended")) {
               
                debugger
                
                // save all details


                var Milestoneid = document.getElementById('<%=hdnextendid.ClientID%>').value;
                var extendStrtdate = document.getElementById('<%=Extendatestrt.ClientID%>').value;
                var extendEnddate = document.getElementById('<%=Extendateend.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/UpdateExtendeddatedtls',
                    data: "{ 'Milestoneid': '" + Milestoneid + "' ,'extendStrtdate': '" + extendStrtdate + "','extendEnddate': '" + extendEnddate + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessUpdateexdate,
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
                function OnSuccessUpdateexdate(response) {
                    debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=hdnextndstrt.ClientID%>').value = "";
                    document.getElementById('<%=hdnextendid.ClientID%>').value = "";
                    document.getElementById("btnclsextended").click();
                    //Bindproject_data();
                    debugger
                    
                   
                    document.getElementById("lblExtnddt_" + document.getElementById("<%=hdnextnhtml.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Bindextenddate_Details(RecordId,lblExtndid) {
              
              debugger;

              document.getElementById("<%=hdnextnhtml.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'Addmilestone.aspx/Bindprojectdetail_extendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate(response) {
            // alert(response.d);
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnextendid.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=hdnextndstrt.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=hdnextndend.ClientID%>').value = milestonedetails.find("EndDate").text();
                     document.getElementById('<%=Button2.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }
         

            </script>
    
      <%--Extended date details bind and update data end  --%>




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
                
                  var milestoneid = document.getElementById('<%=hdnchngdependencyid.ClientID%>').value; 
                  var dependencyid = document.getElementById('<%=hdndependancy.ClientID%>').value;
        
                $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/UpdateMilestone_Dependency',
                   // data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
                      data: "{ 'milestoneid': '" + milestoneid + "' ,'dependencyid': '" + dependencyid + "'}",

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
                    document.getElementById("lblDependancy_" + document.getElementById("<%=hdnchngdependcyhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Dependency(RecordId,lbldependencyid) {
              
              debugger;
              document.getElementById("<%=hdnchngdependcyhtml.ClientID%>").value = lbldependencyid.split('_')[1];
              document.getElementById("<%=hdnchngdependencyid.ClientID%>").value = RecordId;
                 
                $.ajax({
                    url: 'Addmilestone.aspx/Bindmilestone_Dependancy',
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
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("taskid").text() != "") {
                    //document.getElementById('<%=hdnchngdependencyid.ClientID%>').value = milestonedetails.find("id").text();
                     //document.getElementById('<%=txtdependencymilestone.ClientID%>').value = milestonedetails.find("Milestoneid").text();
                    document.getElementById('<%=hdndependancy.ClientID%>').value = milestonedetails.find("taskid").text();

               if (milestonedetails.find("taskid").text() != "")
             {
               var idsemp = milestonedetails.find("taskid").text().split(',');
               $(document.getElementById("<%=ddldependancy.ClientID%>")).val(idsemp);
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



<%--<script type="text/javascript">
function isnumeric(evt)
{
var charcode=(evt.which)?evt.which:event.keycode;
if(charcode>31 && (charcode<48 || charcode>57))
{ alert("Please Enter Only Numeric Value");
return false;
}
return true;
}
</script>--%>

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
    

 <asp:HiddenField ID="multichngprojectid" runat="server" Value="0" />
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
        var projectid = document.getElementById("<%=multichngprojectid.ClientID%>").value;
        
        $.ajax({
            url: 'Addmilestone.aspx/bindMultiChangeOwner_new',
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
        var projectid = document.getElementById("<%=multichngprojectid.ClientID%>").value;
        
        $.ajax({
            url: 'Addmilestone.aspx/bindMultiChangeTeam_new',
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



<%-- add timer notes start--%>

<script type="text/javascript">
    
        function SvaeDataTimer(val15A) {

            if (Page_ClientValidate("btnsaveTimer")) {
               
                debugger
                
                // save all details
                var ctrlid = document.getElementById('<%=hdnmilestone_id.ClientID%>').value;
                
                    varmilstoneid = ctrlid;
                
                $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/SaveTimer',
                    data: "{'ProjectId': '" + document.getElementById('<%=hdnprojec_tid.ClientID%>').value + "','Milestoneid': '" + document.getElementById('<%=hdnmilestone_id.ClientID%>').value + "' ,'TimerNotes': '" + document.getElementById('<%=TimerNotes.ClientID%>').value + "','hour': '" + document.getElementById('hour_' + varmilstoneid).innerHTML + "','minute': '" + document.getElementById('minute_' + varmilstoneid).innerHTML + "','second': '" + document.getElementById('second_' + varmilstoneid).innerHTML + "','millisecond': '" + document.getElementById('millisecond_' + varmilstoneid).innerHTML + "','Transactionid': '" + document.getElementById('<%=hdntransaction_id.ClientID%>').value + "'}",
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
                    var ctrlid = document.getElementById('<%=hdnmilestone_id.ClientID%>').value;
                   // varmilstoneid = ctrlid.id.split('_')[1];
                    stop(document.getElementById('<%=hdnstoptimer_id.ClientID%>').value);
                    document.getElementById('divs_' + ctrlid).style.display = '';
                    document.getElementById('dinv_' + ctrlid).style.display = 'none';

                    document.getElementById('hour_' + ctrlid).innerHTML = "00";
                    document.getElementById('minute_' + ctrlid).innerHTML = "00";
                    document.getElementById('second_' + ctrlid).innerHTML = "00";
                    document.getElementById('millisecond_' + ctrlid).innerHTML="000";

                     document.getElementById('<%=TimerNotes.ClientID%>').value = "";
                     document.getElementById("btnclsTimer").click();
                     Bindmilestone_data();
                    
        
                    
                }

        

          function Binddetails_Timer(ProjectId,Milestoneid,hours,minutes,seconds,milliseconds,transactionid,varid) {
              
              debugger;

                    document.getElementById('<%=hdnprojec_tid.ClientID%>').value = ProjectId;
                    document.getElementById('<%=hdnmilestone_id.ClientID%>').value = Milestoneid;
                   
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
                <%--  alert(document.getElementById("<%=hdnminutecount.ClientID%>").innerHTML);--%>
           
              
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
    
<asp:HiddenField ID="hdnprojectmilstoneid" runat="server" Value="0" />
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
    function savestartdetails(projectid, milstoneid)
    {
        debugger
     
            document.getElementById("<%=hdnprojectmilstoneid.ClientID%>").value = milstoneid;
            $.ajax({
                type: "POST",
                url: 'Addmilestone.aspx/SaveTimerstartdetails',
                data: "{'ProjectId': '" + projectid + "','Milestoneid': '" + milstoneid + "'}",
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
    function OnSuccessTimerdetails(response)
    {
        debugger
       var cid= document.getElementById("hdntrans_" + document.getElementById("<%=hdnprojectmilstoneid.ClientID%>").value)
        cid.value = response.d;
    }

     
        function SvaePauseDetialss(val12A) {

            if (Page_ClientValidate("btnpausesaveNotes")) {

               
                debugger


                var ProjectId = document.getElementById('<%=hdnprojecnots_tid.ClientID%>').value;
                var Milestoneid = document.getElementById('<%=hdntasknotes_id.ClientID%>').value;
                var varid = document.getElementById('<%=hdnpause_id.ClientID%>').value;
                var transactionid = document.getElementById('<%=hdnnotestransaction_id.ClientID%>').value;
                  
            var cid = document.getElementById("hdntrans_" + Milestoneid)
            //cid.value = response.d;
            if (transactionid == "" || transactionid == "0") {
                transactionid = cid.value;
            }

            //alert(cid.value);
            //return false;
                
                
                $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/SaveTimerpausedetails',
                    data: "{'ProjectId': '" + ProjectId + "','Milestoneid': '" + Milestoneid + "' ,'transactionid': '" + transactionid + "','Notes': '" + document.getElementById('<%=Notes.ClientID%>').value + "'}",
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

        

          function BindPausedetails_Notes(ProjectId,Milestoneid,transactionid,varid) {
              
              debugger;

                    document.getElementById('<%=hdnprojecnots_tid.ClientID%>').value = ProjectId;
                    document.getElementById('<%=hdntasknotes_id.ClientID%>').value = Milestoneid;
                   
                   document.getElementById('<%=hdnpause_id.ClientID%>').value = varid;
                   document.getElementById('<%=hdnnotestransaction_id.ClientID%>').value = transactionid;
                   document.getElementById('<%=Notes.ClientID%>').value = "";
               

        }


    function OnSuccess1Notes(response)
    {
            
    }


    

 function saveResumeDetails(projectid, milstoneid,transactionid)
 {
     debugger
         var cid= document.getElementById("hdntrans_" + milstoneid)
         //cid.value = response.d;
         if (transactionid == "" || transactionid == "0") {
             transactionid = cid.value;
         }
         //alert(cid.value);
         //return false;
         $.ajax({
                    type: "POST",
                    url: 'Addmilestone.aspx/SaveTimerresumedetails',
                    data: "{'ProjectId': '" + projectid + "','Milestoneid': '" + milstoneid + "' ,'transactionid': '" + transactionid + "'}",
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
                        url: 'Addmilestone.aspx/Savecolumndata',
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
 

    <script type="text/javascript">
        function hidecolumngrid()
        {
            
            for (var i = 0; i < 19; i++)
            {  
              
                var aa = "ContentPlaceHolder1_Milestone_hdn" + i; 
                if (document.getElementById(aa).value == "0")
                { 
                    
                    let column = table.column(i); 
                    column.visible(false);

                }
            }
          
            document.getElementById("divgridview").style.display = "block";
            document.getElementById("divgridview").style.width = "100%";
            document.getElementById("ContentPlaceHolder1_Milestone_lblprojectdata").style.width = "100%";
           
        }
    </script>
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.3/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/ui/1.13.3/jquery-ui.js"></script>
  <script>
  
   function bindDatePickers() {
       
       <%-- $("#<%=StartDate.ClientID %>").datepicker({
            todayBtn: "linked",
            language: "it",
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=EndDate.ClientID %>").datepicker({
            todayBtn: "linked",
            language: "it",
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/mm/yy'
        });--%>
    }

    // Bind on first page load
    $(function () {
        bindDatePickers();
    });

    // Bind again after every partial postback (UpdatePanel refresh)
    Sys.Application.add_load(function () {
        bindDatePickers();
    });

    
function binddate()
            {
debugger


try {
                   
                     if (document.getElementById("<%=hdnStartDate.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdnStartDate.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=StartDate.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }

try {
                   
                     if (document.getElementById("<%=hdnEndDate.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdnEndDate.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=EndDate.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }
try {
                   
                     if (document.getElementById("<%=hdnextndstrt.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdnextndstrt.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=Extendatestrt.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }
try {
                   
                     if (document.getElementById("<%=hdnextndend.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdnextndend.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=Extendateend.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }


}
  </script>
 <%-- show column hide show end--%>
<script>
    function calculateHours() {
      debugger
if(document.getElementById("<%=ddldurationtype.ClientID%>").value=="1")
{

        var start = document.getElementById("<%=StartDate.ClientID%>").value;
        var end = document.getElementById("<%=EndDate.ClientID%>").value;

        if (!start || !end) {
            alert("Please select both start and end date/time.");
            return;
        }

       var startDate = new Date(start);
        var endDate = new Date(end);

         if (endDate < startDate) {
            alert("End date must be after start date.");
            return;
        }

         var diffMs = endDate - startDate;
        var diffHours = diffMs / (1000 * 60 * 60);
     alert("Total hours: " + diffHours.toFixed(2));
       
}
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
                url: "FileUploadHandlermile.ashx?id="+id+"", 
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
    var basePath = "/pms/MileImages/";
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


