<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tasklist.ascx.cs" Inherits="ucontrol_Tasklist" %>
 <%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
   <%--<div class="page-content">
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
                                            <p><%=fns.getltext("Task List", "2")%></p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Task List</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                             <div class="dropdown-menu dropdown-menu-end">
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTasklist(0)"><span class="align-middle">All Task List</span></a>                         
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTasklist(1)"><span class="align-middle">Status 1</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTasklist(2)"><span class="align-middle">Status 2</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetTasklist(3)"><span class="align-middle">Status 3</span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addtasklist"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">
                                            <a class="dropdown-item toggle-vis" data-column="0"><i class="ri-check-line align-middle fs-lg"></i> Select</a>
                                            <a class="dropdown-item toggle-vis" data-column="1"><i class="ri-check-line align-middle fs-lg"></i> S.No.</a>
                                            <a class="dropdown-item toggle-vis" data-column="2"><i class="ri-check-line align-middle fs-lg"></i> Task List</a>
                                            <a class="dropdown-item toggle-vis" data-column="3"><i class="ri-check-line align-middle fs-lg"></i> Team</a>
                                            <a class="dropdown-item toggle-vis" data-column="4"><i class="ri-check-line align-middle fs-lg"></i> Status</a>
                                            <a class="dropdown-item toggle-vis" data-column="5"><i class="ri-check-line align-middle fs-lg"></i> Priority</a>
                                            <a class="dropdown-item toggle-vis" data-column="6"><i class="ri-check-line align-middle fs-lg"></i> Budget</a>
                                            <a class="dropdown-item toggle-vis" data-column="7"><i class="ri-check-line align-middle fs-lg"></i> Actual</a>
                                            <a class="dropdown-item toggle-vis" data-column="8"><i class="ri-check-line align-middle fs-lg"></i> Start Date</a>
                                            <a class="dropdown-item toggle-vis" data-column="9"><i class="ri-check-line align-middle fs-lg"></i> Target Date</a>
                                            <a class="dropdown-item toggle-vis" data-column="10"><i class="ri-check-line align-middle fs-lg"></i> Timeline</a>
                                            <a class="dropdown-item toggle-vis" data-column="11"><i class="ri-check-line align-middle fs-lg"></i> Completion Status</a>
                                            <a class="dropdown-item toggle-vis" data-column="12"><i class="ri-check-line align-middle fs-lg"></i> Issues</a>
                                            <a class="dropdown-item toggle-vis" data-column="13"><i class="ri-check-line align-middle fs-lg"></i> File</a>
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
          <%--          </div>
                </div>
            </div>           
        </div>
    </div>--%>

<div class="offcanvas offcanvas-end" id="addtasklist" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
         <label id="lbltitle" runat="server"> Add Task List</label>

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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>   &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectId" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%>   &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Milestone" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestone" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnmilestone" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task List", "2")%></label>
                                    </div>
                                    <input type="text" id="TaskList" placeholder="Task List" runat="server" class="form-control">
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="P_Status" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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
                                
                                       <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
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


                                  <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Targate Date", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="TargateDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="TargateDate" runat="server" class="form-control">
                                             <asp:HiddenField ID="hdnTargateDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                               
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Tag To", "2")%></label>
                                    </div>
                                    <input type="text" id="TagTo" runat="server" placeholder="Tag To" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Access", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Access" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
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
                        <%--    <select id="select_emp1" class="form-control" multiple="multiple">
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
        var element = document.getElementById("addtasklist");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunction(){
        var element = document.getElementById("addtasklist");
        element.classList.remove("canvas-fullscreen");
    }
</script>
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
   <%-- <script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
 <script src="/pms/assets/js/jscolor.js"></script>
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



                      <asp:HiddenField ID="hdnsearchstaus" runat="server" />
                            <script type="text/javascript">
                                function GetTasklist(statusid)
                                {
                                    debugger
                                    document.getElementById('<%=hdnsearchstaus.ClientID%>').value = statusid;
                                if (statusid == "0")
                                {
                                   BindTasklist_data_drosearch();
                                }
                                else if (statusid == "1") 
                                {
                                   
                                    BindTasklist_data_drosearch();

                                }
                               else if (statusid == "2")
                                {
                                   
                                    BindTasklist_data_drosearch();

                                }
                                else if (statusid == "3")
                                {
                                   
                                    BindTasklist_data_drosearch();

                               }
                                else
                                {
                                   
                                    BindTasklist_data_drosearch();

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
                            url: "Addtasklist.aspx/statussave",
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
                            url: "Addtasklist.aspx/AddPrioritytype",
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


                        // save all details

                   
                        
                        var Taskdetails = {};
                        Taskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value;;  
                        Taskdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        Taskdetails.Milestone = document.getElementById('<%=Milestone.ClientID%>').value; 
                        Taskdetails.TaskList = document.getElementById('<%=TaskList.ClientID%>').value; 
                        Taskdetails.TagTo = document.getElementById('<%=TagTo.ClientID%>').value; 
                        Taskdetails.Access = document.getElementById('<%=Access.ClientID%>').value; 
                        Taskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                       <%-- Taskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        Taskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>
                        Taskdetails.P_Status = statusid;
                        Taskdetails.Priority = Prioritytypeid;
                        Taskdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        Taskdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;
                        Taskdetails.StartDate = document.getElementById('<%=StartDate.ClientID%>').value; 
                        Taskdetails.TargateDate = document.getElementById('<%=TargateDate.ClientID%>').value; 
                       
                        $.ajax({
                            type: "POST",
                            url: 'Addtasklist.aspx/SaveTasklist',
                            data: '{Taskdetails: ' + JSON.stringify(Taskdetails) + '}',
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
                     $("[id*=ProjectId]").val("0");
                    $("[id*=Milestone]").val("0");
                    $("[id*=TaskList]").val("");
                    $("[id*=TagTo]").val("");
                    $("[id*=Access]").val("0");


                    

                    document.getElementById('<%=hdnteam.ClientID%>').value = "";
                    document.getElementById('<%=P_Status.ClientID%>').value = "0";
                    document.getElementById('<%=Priority.ClientID%>').value = "0";
                    document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                     document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnTargateDate.ClientID%>').value = "";

                     $(document.getElementById("<%=team.ClientID%>")).val(0);
                    $(document.getElementById("<%=team.ClientID%>")).change();

                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save") {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        BindTasklist_data();
                    }
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                       // document.getElementById("btncls").click();
                        BindTasklist_data();
                    }
                   

               
                    
                }



          function Cleardata() {
                        

                   $("[id*=ProjectId]").val("0");
                   $("[id*=Milestone]").val("0");
                    $("[id*=TaskList]").val("");
                    $("[id*=TagTo]").val("");
                    $("[id*=Access]").val("0");

                  
                  

                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                    document.getElementById('<%=hdnmilestone.ClientID%>').value = "0";
                   document.getElementById('<%=hdnAccess.ClientID%>').value = "0";

                   document.getElementById('<%=hdnteam.ClientID%>').value = "";
                    document.getElementById('<%=P_Status.ClientID%>').value = "0";
                    document.getElementById('<%=Priority.ClientID%>').value = "0";
                    document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDate.ClientID%>').value = "";
                    document.getElementById('<%=hdnTargateDate.ClientID%>').value ="";

                     $(document.getElementById("<%=team.ClientID%>")).val(0);
                    $(document.getElementById("<%=team.ClientID%>")).change();

                    document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Task List";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'Addtasklist.aspx/BinTasklistdetail',
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
                var Taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = Taskdetails.find("id").text();
                    $("[id*=ProjectId]").val(Taskdetails.find("ProjectId").text());
                    $("[id*=Milestone]").val(Taskdetails.find("Milestone").text());
                    $("[id*=TaskList]").val(Taskdetails.find("TaskList").text());
                    $("[id*=TagTo]").val(Taskdetails.find("TagTo").text());
                    $("[id*=Access]").val(Taskdetails.find("Access").text());

                    document.getElementById('<%=hdnteam.ClientID%>').value = Taskdetails.find("team").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Taskdetails.find("P_Status").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Taskdetails.find("Priority").text();
                     document.getElementById('<%=Budget.ClientID%>').value = (Taskdetails.find("Budget").text()); 
                    document.getElementById('<%=Actual.ClientID%>').value = (Taskdetails.find("Actual").text()); 
                    document.getElementById('<%=StartDate.ClientID%>').value=(Taskdetails.find("StartDate").text());
                    document.getElementById('<%=TargateDate.ClientID%>').value=(Taskdetails.find("TargateDate").text());
           
                     

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Taskdetails.find("ProjectId").text();
                     document.getElementById('<%=hdnmilestone.ClientID%>').value = Taskdetails.find("Milestone").text();
                    document.getElementById('<%=hdnAccess.ClientID%>').value = Taskdetails.find("Access").text();

                     document.getElementById('<%=hdnteam.ClientID%>').value = Taskdetails.find("team").text();
                     document.getElementById('<%=hdnP_Status.ClientID%>').value = Taskdetails.find("P_Status").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Taskdetails.find("Priority").text();
                    document.getElementById('<%=hdnStartDate.ClientID%>').value = Taskdetails.find("StartDate").text();
                    document.getElementById('<%=hdnTargateDate.ClientID%>').value = Taskdetails.find("TargateDate").text();

                        if (Taskdetails.find("team").text() != "")
             {
             var idsemp = Taskdetails.find("team").text().split(',');
            $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=team.ClientID%>")).change();
            }

                  
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Task List";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       



    <script>
        $( document ).ready(function() {
            BindTasklist_data();
});
         function BindTasklist_data() {
            
                debugger;
                  $.ajax({
                    url: 'Addtasklist.aspx/bindTasklistdata',                  
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
        scrollY:  300,
        scrollCollapse: true,
        paging: true,
        searching: true,
        
    });
           
        }

    </script>


<script>
        $( document ).ready(function() {
            BindTasklist_data_drosearch();
});
         function BindTasklist_data_drosearch() {
            
                debugger;
                  $.ajax({
                    url: 'Addtasklist.aspx/bindTasklistdata_statussearch',                  
                      //data: '{}',
                       data: "{ 'RecordId': '" + $("[id*=hdnsearchstaus]").val() + "'}",
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
        scrollY:  300,
        scrollCollapse: true,
        paging: true,
        searching: true,
        
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
                    url: 'Addtasklist.aspx/DeleteTasklistdata',
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
            BindTasklist_data();
        }

    </script>

  

    <%--save data end  --%> 



<%--Team details bind and update data start --%>

    <script type="text/javascript">
             
        function SvaeDatateam(val13A) {

            if (Page_ClientValidate("btnsaveteam")) {
               
                debugger


                // save all details

                var Taskdetails = {};
                Taskdetails.id = document.getElementById('<%=hdnteameid.ClientID%>').value;;
                Taskdetails.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'Addtasklist.aspx/Save_Team',
                    data: '{Taskdetails: ' + JSON.stringify(Taskdetails) + '}',
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
                   // BindTasklist_data();
                    document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function Binddetails_Team(RecordId,lbluserid) {
              
              debugger;

               document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                 
                $.ajax({
                    url: 'Addtasklist.aspx/Bindetail_Team',
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
                var Taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = Taskdetails.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = Taskdetails.find("team").text();

               if (Taskdetails.find("team").text() != "")
             {
               var idsemp = Taskdetails.find("team").text().split(',');
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
    <asp:HiddenField ID="hdtasklistidid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid)
        {
            debugger
                   var tasklistid=document.getElementById("<%=hdtasklistidid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addtasklist.aspx/Update_Status',
                    data: "{ 'tasklistid': '" + tasklistid + "' ,'statusid': '" + statusid + "' }",
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
           // BindTasklist_data();
             var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setprojectid(tasklistid,btnststusid)
        {
            debugger
            document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdtasklistidid.ClientID%>").value = tasklistid;
        }
            </script>


      <asp:HiddenField ID="hdnpropertID" runat="server" Value="0" />
<asp:HiddenField ID="hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDatapropertydeatils(Propertyid)
        {
                   debugger
                   var tasklistid=document.getElementById("<%=hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addtasklist.aspx/UpdatePriority_Status',
                    data: "{ 'tasklistid': '" + tasklistid + "' ,'Propertyid': '" + Propertyid + "' }",
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
          //  BindTasklist_data();
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
