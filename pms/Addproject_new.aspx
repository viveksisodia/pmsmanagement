<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="Addproject_new.aspx.cs" Inherits="Addproject" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<script type="text/javascript" language="javascript">
window.onload = function() {
  document.getElementById('myclass').className = 'dashboard-topbar-wrapper';
};
      </script> --%>
<style>
.dataTables_scrollBody {min-height:300px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content" style="padding-bottom:20px;">
     
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico orange">
                                            <i class="ri-pencil-ruler-2-line"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p><%=fns.getltext("Project Management", "2")%></p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside" style="display:none;">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Projects</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end" style="display:none;">
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(0)"><span class="align-middle">All Projects</span></a>                        
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(1)"><span class="align-middle">Under Negotiationaa</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(2)"><span class="align-middle">Working</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(3)"><span class="align-middle">Stuck</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(4)"><span class="align-middle">Done</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(5)"><span class="align-middle">under progress</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(6)"><span class="align-middle">ug process</span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addproject"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>

                                      <%-- excel import data start--%>

                                      <%--  <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="#conversation" title="Import"><i class="ri-download-2-line"></i></span></button>--%>

                                         <a href="javascript:void(0);" style="display:none;" type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="modal" data-bs-target="#import_mdl"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></a>


                                        <div class="modal fade" id="import_mdl" data-bs-backdrop="static"
                                            data-bs-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog modal-dialog-centered" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-body text-center p-3">
                                                        <div class="mt-4">
                                                            <h4 class="mb-1">Import Project</h4>
                                                            <p class="text-muted mb-3">Download Our <a href="Downloadexcel.aspx"><b>Sample File</b></a> To Get Started.</p>
                                                            
                                                            <div class="input-group mb-3">
                                                        <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server"  />
                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" SetFocusOnError="true" 
                               ValidationGroup="gi"  runat="server" ControlToValidate="FileUpload1"
                                ErrorMessage="Accepts Excel Files Only !" ValidationExpression="^.*\.xls[xm]?$"
                                Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnupload" class="btn btn-success" runat="server" Text="Upload" OnClick="btnupload_Click"/>
                                                            </div>
                                                            <div class="hstack gap-2 justify-content-center">
                                                                <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="modal"><i class="ri-close-line me-1 align-middle"></i> Close</a>
                                                            </div>
                                                             <div id="dvbutton" style="display:none">
                                              
                                                </div>
                                            <div style="display:none">
                                             
                                                </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                         <%-- excel import data end--%>



                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                       
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No.", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Project ID", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Project Name", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj10" data-column="10"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Extended Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Start Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("End Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj15" data-column="15"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issues", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj16" data-column="16"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Tags", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj17" data-column="17"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Budget", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj18" data-column="18"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Actual", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj19" data-column="19"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Project Address", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj20" data-column="20"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Client Name", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj21" data-column="21"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Award Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj22" data-column="22"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Approval of Drawing", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj23" data-column="23"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Drawing Approval Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="proj24" data-column="24"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Upload Drawing", "2")%></a>
                                        </div>

                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                                        <button type="button" style="display:none;" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                                        <div class="dropdown-menu" style="display:none;">
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
                    </div>
                </div>
            </div>           
        </div>
    </div>

<div class="offcanvas offcanvas-end" id="addproject" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
          <label id="lbltitle" runat="server"><%=fns.getltext("Add Project", "2")%> </label>

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
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project ID", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="ProjectID" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   <input type="text" id="ProjectID" class="form-control"  placeholder="Project ID" runat="server" >
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project Name", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectName" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="ProjectName" class="form-control"  placeholder="Project Name" runat="server">
                                </div>


                                 <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkprojectWeightage" runat="server"  />  Do You Want Same Weightage on Milestone   
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                 <div class="col-xxl-12 col-lg-12" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" class="form-control">
                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="CmpOwner" CssClass="form-control" runat="server" width="450px"
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


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="ProjectStatus" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />

                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ProjectStatus" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectStatus" runat="server" Value="0" />
                                             </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewprojectstatus" style="display:none">
                                          <input type="text" id="txtnewprojectstatus" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                                  </div>
                                                
                                           



                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project Address", "2")%></label>
                                    </div>
                                    <div class="row">
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" id="ProjectAddress1" runat="server" class="form-control" placeholder="Address Line 1">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" id="ProjectAddress2" runat="server" class="form-control" placeholder="Address Line 2">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" id="City" runat="server" class="form-control" placeholder="City">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                          
                                           <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                            <cc1:DropDownListChosen ID="Countryid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Country ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="Countryid_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Countryid" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                                               <asp:HiddenField ID="hdnCountryid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                           

                                        </div>

                                         <div class="col-xxl-7 col-lg-7">
                                           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                            <cc1:DropDownListChosen ID="StateName" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="State ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnstatename" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                        </div>

                                        <div class="col-xxl-5 col-lg-5">
                                            <input type="text" id="PostalCode" runat="server" class="form-control" placeholder="Zip/Postal Code">
                                        </div>
                                       
                                        
                                    </div>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Client Name", "2")%></label>
                                    </div>
                                    <input type="text" id="ClientName" runat="server" placeholder="Client Name" class="form-control">
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
                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                             

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

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Date of Award", "2")%></label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                        <input type="date" id="AwardDate" runat="server" class="form-control">

                                           <asp:HiddenField ID="hdnawarddate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="StartDate" runat="server" class="form-control" onchange="checkdate(this.id)">
                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" 
                                                ControlToValidate="TargetDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="TargetDate" runat="server" class="form-control" onchange="checkdate(this.id)">
                                              <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                   <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="isdrawing" runat="server" onchange="DrawingRequired();" />Drawing Required 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-xxl-6 col-lg-6" id="divaprovaldraing" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Approval of Drawing", "2")%></label>
                                    </div>
                              
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ApprovalDrawing" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="ApprovalDrawing ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">Select</asp:ListItem>
                                          <asp:ListItem  value="1">Yes</asp:ListItem>
                                          <asp:ListItem  value="2">No</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnaprovaldrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                              

                                </div>
                                <div class="col-xxl-6 col-lg-6" id="divaprovalDate" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Drawing Approval Date", "2")%></label>
                                    </div>
                                      <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="DrawingApprovalDate" runat="server" class="form-control">
                                             <asp:HiddenField ID="hdndrawingaprvldate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>  
                                
                                <div style="display: none">
                             <asp:Button ID="btnuploadimg" runat="server" Text="" OnClick="btnuploadimg_Click" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:FileUpload ID="Uploaddrawing" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreview();"/>
                                       <a href="#" id="imgpath" target="_blank"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>

                                               

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    

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


    <div class="offcanvas offcanvas-end canvas_lg_screen canvas-fullscreen" id="canvas_popup_multi" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopupmulti" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoominmulti" onclick="fse3Functionmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomoutmulti" onclick="fse3Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup_multi' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>

<div class="offcanvas offcanvas-end" id="addcolumns" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title"">Add Columns</h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fs1Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fse1Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form">
                            <div class="lead_form_head">
                            <h4>+ Add Custom Columns</h4>
                            </div>
                            <div class="lead_form_body">
                                <div class="row">
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Column Type</label>
                                        </div>
                                        <select class="form-control">
                                            <option>-- Select --</option>
                                            <option selected="selected">Text Box</option>
                                            <option>Textarea</option>
                                            <option>Dropdown</option>
                                            <option>Multi Select</option>
                                            <option>Date</option>
                                            <option>Time</option>
                                            <option>Checkbox</option>
                                            <option>Radiobox</option>
                                        </select>
                                    </div>
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Column Name</label>
                                            <div class="gap-1 flex-shrink-0 align-self-center">
                                                <div class="form-check">
                                                    <label class="form-label">
                                                        <input class="form-check-input" type="checkbox" />
                                                        Is Mandatory
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <input type="text" class="form-control">
                                    </div>
                                    <div class="col-xxl-12 col-lg-12 mb-2" style="">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Add Dropdown Values</label>
                                        </div>
                                        <div class="bg-light p-3 border">
                                            <div class="input-group mb-3">
                                                <input type="text" class="form-control" aria-label="Recipient's username with two button addons">
                                                <button class="btn btn-success" type="button">Add</button>
                                            </div>
                                            <div class="list-group nested-list nested-sortable-handle custom-sortable">
                                                <div class="list-group-item">
                                                    <div class="d-flex align-items-center">
                                                        <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Select 1</div>
                                                        <div class="flex-shrink-0 dropdown">
                                                            <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                <i class="bi bi-three-dots-vertical"></i>
                                                            </a>
                                                            <ul class="dropdown-menu dropdown-menu-end">
                                                                <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="list-group-item">
                                                    <div class="d-flex align-items-center">
                                                        <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Select 2</div>
                                                        <div class="flex-shrink-0 dropdown">
                                                            <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                <i class="bi bi-three-dots-vertical"></i>
                                                            </a>
                                                            <ul class="dropdown-menu dropdown-menu-end">
                                                                <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="list-group-item">
                                                    <div class="d-flex align-items-center">
                                                        <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Select 3</div>
                                                        <div class="flex-shrink-0 dropdown">
                                                            <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                <i class="bi bi-three-dots-vertical"></i>
                                                            </a>
                                                            <ul class="dropdown-menu dropdown-menu-end">
                                                                <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Default Value</label>
                                        </div>
                                        <input type="text" class="form-control" style="display:none;">
                                        <textarea class="form-control" style="display:none;"></textarea>
                                        <select class="form-control" style="display:none;">
                                            <option>-- Select --</option>
                                            <option>Select 1</option>
                                            <option>Select 2</option>
                                            <option>Select 3</option>
                                        </select>
                                        <div class="mb-2" style="">
                                        <select id="select_custom" class="form-control" multiple="multiple">
                                            <option>Select 1</option>
                                            <option>Select 2</option>
                                            <option>Select 3</option>
                                        </select>
                                        </div>
                                        <input type="date" class="form-control" style="display:none;">
                                        <input type="time" class="form-control" style="display:none;">
                                        <div class="form-check" style="display:none;">
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <label class="form-label d-block">
                                                        <input class="form-check-input" type="checkbox" />
                                                        Checkbox 1
                                                    </label>
                                                </div>
                                                <div class="col-sm-6">
                                                    <label class="form-label d-block">
                                                        <input class="form-check-input" type="checkbox" />
                                                        Checkbox 2
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-check form-radio-primary" style="display:none;">
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <label class="form-check-label d-block">
                                                        <input value="" name="radiobox" type="radio" class="form-check-input">
                                                        Radiobox 1
                                                    </label>
                                                </div>
                                                <div class="col-sm-6">
                                                    <label class="form-check-label d-block">
                                                        <input value="" name="radiobox" type="radio" class="form-check-input">
                                                        Radiobox 2
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    

                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Description</label>
                                        </div>
                                        <textarea class="form-control"></textarea>
                                    </div>
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Access</label>
                                        </div>
                                        <select id="select_access" class="form-control" multiple="multiple">
                                            <option>Staff</option>
                                            <option>Client</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Add</a>
    </div>--%>
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
                                    <input type="date" id="Extendatestrt" runat="server" class="form-control">
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
                                    <input type="date" id="Extendateend" runat="server" class="form-control">
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


<div id="addtags" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Tags</h5>
                <button type="button" id="btnclstags" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>



            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                         <asp:HiddenField ID="hdntagprjid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntagshtml" runat="server" Value="0" />

                        <div class="col-sm-8 mb-1">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">Tag Name&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="tags" ValidationGroup="btnsaveTags"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                            </div>
                            <input type="text" id="tags" runat="server" class="form-control" />
                        </div>   
                        <div class="col-sm-4 mb-1">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">Color</label>
                            </div>
                            <input data-jscolor="{}" id="tagscolor" runat="server" class="jscolor form-control">
                        </div> 
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveTags" class="btn btn-primary" onclick="SvaeadTags('saveAddTags');">Add</button>

            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>



<div id="boq" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Bill of Quantity (BOQ)</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="row">
                    <div class="col-sm mb-2">
                        <div class="d-flex justify-content-sm-end">
                        <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                            <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="IMPORT FROM ANOTHER SOFTWARE"><i class="ri-download-2-line"></i></span></button>
                            <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="SEND BY MAIL"><i class="ri-mail-send-line"></i></span></button>                                           
                            <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="DOWNLOAD"><i class="ri-download-line"></i></span></button>
                        </div>
                        </div>
                    </div>                                
                </div>
                <div class="table-responsive">
                    <table class="table align-middle table-striped table-hover table-nowrap table-bordered table-sm">
                        <tr>
                            <th>S.No.</th>
                            <th>Item & Description</th>
                            <th>Quantity</th>
                            <th>Make & Model</th>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
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
        var element = document.getElementById("addproject");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunction(){
        var element = document.getElementById("addproject");
        element.classList.remove("canvas-fullscreen");
    }
</script>
<script>
    function fs1Function(){
        var element = document.getElementById("addcolumns");
        element.classList.add("canvas-fullscreen");  
    }
    function fse1Function(){
        var element = document.getElementById("addcolumns");
        element.classList.remove("canvas-fullscreen");
    }
</script>
    
    <script src="/pms/assets/libs/sortablejs/Sortable.min.js"></script>
    <script src="/pms/assets/js/pages/nestable.init.js"></script>
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
    <%--<script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/jscolor.js"></script>

    
<script>
    function fs2Function(){
        var element = document.getElementById("canvas_popup");
        element.classList.add("canvas-fullscreen");  
    }
    function fse2Function(){
        var element = document.getElementById("canvas_popup");
        element.classList.remove("canvas-fullscreen");
    }
</script>  

    <script>
    function fs3Functionmulti(){
        var element = document.getElementById("canvas_popup_multi");
        element.classList.add("canvas-fullscreen");  
    }
    function fse3Functionmulti(){
        var element = document.getElementById("canvas_popup_multi");
        element.classList.remove("canvas-fullscreen");
    }
</script>

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
    $('#select_access').select2({
        dropdownParent: $("#addcolumns"),
        placeholder: 'Select access',
	    width: '100%',
        closeOnSelect: true,
        allowClear: true,
    });   
</script>
<script>
    $('#select_custom').select2({
        dropdownParent: $("#addcolumns"),
        placeholder: '-- Select --',
	    width: '100%',
        closeOnSelect: true,
        allowClear: true,
    });   
</script>
 
 <script type="text/javascript" src="/pms/assets/js/moment.min.js"></script>
<script type="text/javascript" src="/pms/assets/js/daterangepicker.min.js"></script>
<link rel="stylesheet" type="text/css" href="/pms/assets/js/daterangepicker.css" />

  <script>
$(function() {
  $('.timesheet-wrap').daterangepicker({
    opens: 'center',
    locale: {
      format: 'DD/MM/YYYY'
    }
  });
});
</script>

<script>
$('body').on('click', function (e) {
        $('[data-bs-toggle=popover]').each(function () {
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                (($(this).popover('hide').data('bs.popover')||{}).inState||{}).click = false;
            }
        });
    });
</script>


     <asp:HiddenField ID="hdnsearchstaus" runat="server" />
                            <script type="text/javascript">
                                function GetStatussrch(statusid)
                                {
                                    debugger
                                    document.getElementById('<%=hdnsearchstaus.ClientID%>').value = statusid;
                                if (statusid == "0")
                                {
                                   Bindproject_data_searchstus();
                                }
                                else if (statusid == "1") 
                                {
                                   
                                    Bindproject_data_searchstus();

                                }
                               else if (statusid == "2")
                                {
                                   
                                    Bindproject_data_searchstus();

                                }
                                else if (statusid == "3")
                                {
                                   
                                    Bindproject_data_searchstus();

                                }
                                else if (statusid == "4")
                                {
                                   
                                    Bindproject_data_searchstus();

                                }

                                else if (statusid == "5")
                                {
                                   
                                    Bindproject_data_searchstus();

                                }
                                else if (statusid == "6")
                                {
                                   
                                    Bindproject_data_searchstus();

                                }
                                else
                                {
                                   
                                    Bindproject_data_searchstus();

                               }
                             }
                            </script>



   <script type="text/javascript" >
       function DrawingRequired()
       {

       if (document.getElementById('<%=isdrawing.ClientID%>').checked == true)
       {

           document.getElementById("divaprovaldraing").style.display = "block";
           document.getElementById("divaprovalDate").style.display = "block";
           document.getElementById("divuploaddrawing").style.display = "block";
   
        }
       if (document.getElementById('<%=isdrawing.ClientID%>').checked == false)
       {
      
           document.getElementById("divaprovaldraing").style.display = "none";
           document.getElementById("divaprovalDate").style.display = "none";
           document.getElementById("divuploaddrawing").style.display = "none";

    

       }
    }
</script>


    <script>
    function checkdate(enterdate)
    {
        if (document.getElementById("<%=StartDate.ClientID%>").value != "" && document.getElementById("<%=StartDate.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=StartDate.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=TargetDate.ClientID%>").value);
                              var todt = new Date(today2);

                               var t1 = new Date();
                               t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                               var t2 = new Date();
            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
            if (t1 > t2)
            {
                 alert("Enter Valid Date");
                document.getElementById(enterdate).value = "";
                document.getElementById(enterdate).focus();
                return false;
            }
            else
            {
             return true;
            }
        }
        else
        {
            return true;
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
                document.getElementById('<%=hdnProjectStatus.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatus").style.display = "none";
               if (document.getElementById('<%=ProjectStatus.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnProjectStatus.ClientID%>').value = "1";
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
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) 
                        {
                            document.getElementById("pnlimg").style.display = "block";
                            document.getElementById("divsub").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }


                       // Project status save
                        
                        var projectstatusid = "0";
                        if (document.getElementById('<%=hdnProjectStatus.ClientID%>').value != "" && document.getElementById('<%=hdnProjectStatus.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addproject.aspx/AddAddprojecttatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatus.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        projectstatusid = response.d;
                                    

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
                            projectstatusid = document.getElementById('<%=ProjectStatus.ClientID%>').value;
                        }

                        // Priority type save

                         var Prioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addproject.aspx/AddPrioritytype",
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

                        
                        var projectdetails = {};
                        projectdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; ;  
                        projectdetails.ProjectID = document.getElementById('<%=ProjectID.ClientID%>').value; 
                        projectdetails.ProjectName = document.getElementById('<%=ProjectName.ClientID%>').value; 
                        projectdetails.Owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        projectdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                        <%--projectdetails.ProjectStatus = document.getElementById('<%=ProjectStatus.ClientID%>').value;--%> 
                        projectdetails.ProjectStatus = projectstatusid;
                        projectdetails.ProjectAddress1 = document.getElementById('<%=ProjectAddress1.ClientID%>').value; 
                        projectdetails.ProjectAddress2 = document.getElementById('<%=ProjectAddress2.ClientID%>').value; 
                        projectdetails.City = document.getElementById('<%=City.ClientID%>').value; 
                        projectdetails.PostalCode = document.getElementById('<%=PostalCode.ClientID%>').value; 
                        projectdetails.StateName = document.getElementById('<%=StateName.ClientID%>').value; 
                        projectdetails.Countryid = document.getElementById('<%=Countryid.ClientID%>').value; 
                        projectdetails.ClientName = document.getElementById('<%=ClientName.ClientID%>').value;
                        <%--projectdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value;--%>
                        projectdetails.Priority = Prioritytypeid;
                        projectdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        projectdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;
                        projectdetails.AwardDate = document.getElementById('<%=AwardDate.ClientID%>').value; 
                        projectdetails.StartDate = document.getElementById('<%=StartDate.ClientID%>').value; 
                        projectdetails.TargetDate = document.getElementById('<%=TargetDate.ClientID%>').value; 
                        projectdetails.ApprovalDrawing = document.getElementById('<%=ApprovalDrawing.ClientID%>').value; 
                        projectdetails.DrawingApprovalDate = document.getElementById('<%=DrawingApprovalDate.ClientID%>').value; 
                     <%--  projectdetails.Uploaddrawing = document.getElementById('<%=Uploaddrawing.ClientID%>').value; --%>

                        document.getElementById('<%=btnuploadimg.ClientID%>').click();
                     projectdetails.Uploaddrawing = document.getElementById('<%=hdnUploaddrawing.ClientID%>').value;
                  projectdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value; 

                    if (document.getElementById('<%=isdrawing.ClientID%>').checked == true)
                    {

                        projectdetails.isdrawing = 1

                    }

                    if (document.getElementById('<%=isdrawing.ClientID%>').checked == false)
                    {

                        projectdetails.isdrawing = 0

                  }

                       if (document.getElementById('<%=checkprojectWeightage.ClientID%>').checked == true)
                       {

                        projectdetails.checkprojectWeightage = 1

                       }

                       if (document.getElementById('<%=checkprojectWeightage.ClientID%>').checked == false)
                       {

                        projectdetails.checkprojectWeightage = 0

                       }


                       
                        $.ajax({
                            type: "POST",
                            url: 'Addproject.aspx/SaveProject',
                            data: '{projectdetails: ' + JSON.stringify(projectdetails) + '}',
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
                   // alert(response.d);
                    alert("Data Saved successfully.");
                    document.getElementById('<%=btnload.ClientID%>').click();


                    $("[id*=ProjectID]").val("");
                    $("[id*=ProjectName]").val("");
                    $("[id*=CmpOwner]").val("");
                    $("[id*=team]").val("");
                    $("[id*=ProjectStatus]").val("0");
                    $("[id*=ProjectAddress1]").val("");
                    $("[id*=ProjectAddress2]").val("");
                    $("[id*=City]").val("");
                    $("[id*=PostalCode]").val("");
                    $("[id*=StateName]").val("0");
                    $("[id*=Countryid]").val("0");
                    $("[id*=ClientName]").val("");
                    $("[id*=Priority]").val("0");
                    $("[id*=Budget]").val("");
                    $("[id*=Actual]").val("");
                    $("[id*=AwardDate]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=TargetDate]").val("");
                    $("[id*=ApprovalDrawing]").val("");
                    $("[id*=DrawingApprovalDate]").val("");
                    $("[id*=Uploaddrawing]").val("");

                     document.getElementById("<%=ProjectStatus.ClientID%>").value = "0";
                     document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=CmpOwner.ClientID%>")).val(0);
                   $(document.getElementById("<%=CmpOwner.ClientID%>")).change();

                    document.getElementById('<%=hdnteam.ClientID%>').value = "";
                     document.getElementById('<%=Weightage.ClientID%>').value = "";
                     $(document.getElementById("<%=team.ClientID%>")).val(0);
                    $(document.getElementById("<%=team.ClientID%>")).change();
                    document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = 0;
                    document.getElementById('<%=isdrawing.ClientID%>').checked = 0;
                       
                        document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/assets/images/user.jpg";
                   
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save") 
                    {
                        
                      
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        Bindproject_data();
                    }

                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore") 
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        Bindproject_data();
                    }
                   
                   // showdivimg();


                    //document.getElementById('btnsave').value = "Save";

                  <%--    if (document.getElementById('<%=hdnbtnstatus.ClientID%>').value == "save") {
                            window.location.href = '/Addproject.aspx';
                            return false;
                        }
                        else if (document.getElementById('<%=hdnbtnstatus.ClientID%>').value == "view") {
                            window.location.href = '/Addproject.aspx';
                        }
                        else if (document.getElementById('<%=hdnbtnstatus.ClientID%>').value == "new") {
                        window.location.href = '/Addproject.aspx';--%>

                   // }
                    
                }



          function Cleardata() {
                        
              
                   $("[id*=ProjectID]").val("");
                    $("[id*=ProjectName]").val("");
                    $("[id*=CmpOwner]").val("");
                    $("[id*=team]").val("");
                    $("[id*=ProjectStatus]").val("0");
                    $("[id*=ProjectAddress1]").val("");
                    $("[id*=ProjectAddress2]").val("");
                    $("[id*=City]").val("");
                    $("[id*=PostalCode]").val("");
                    $("[id*=StateName]").val("0");
                    $("[id*=Countryid]").val("0");
                   $("[id*=ClientName]").val("");
                    $("[id*=Priority]").val("0");
                    $("[id*=Budget]").val("");
                    $("[id*=Actual]").val("");
                    $("[id*=AwardDate]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=TargetDate]").val("");
                    $("[id*=ApprovalDrawing]").val("");
                    $("[id*=DrawingApprovalDate]").val("");
                   $("[id*=Uploaddrawing]").val("");

            <%--  if (document.getElementById('<%=ProjectStatus.ClientID%>').value == "1")
              {
                  
                  document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                  document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";
                  document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                  document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";
              }
              else
              {
                  document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "block";
                  document.getElementById('<%=stustprintColor.ClientID%>').style.display = "block";
                  document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "block";
                  document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "block";

              }--%>


                    document.getElementById('<%=CmpOwner.ClientID%>').value = "";
                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                  document.getElementById('<%=hdnCountryid.ClientID%>').value = "";
              document.getElementById('<%=hdnstatename.ClientID%>').value = "";
              document.getElementById('<%=ProjectStatus.ClientID%>').value = "0";
              document.getElementById('<%=hdnProjectStatus.ClientID%>').value = "0";
               document.getElementById('<%=Priority.ClientID%>').value = "0";
              document.getElementById('<%=hdnPriority.ClientID%>').value = "0";

              document.getElementById('<%=AwardDate.ClientID%>').value = "";
              document.getElementById('<%=StartDate.ClientID%>').value = "";
              document.getElementById('<%=TargetDate.ClientID%>').value = "";
              document.getElementById('<%=TargetDate.ClientID%>').value = "";

                    document.getElementById('<%=hdnawarddate.ClientID%>').value = "";
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = "";
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = "";
                    document.getElementById('<%=hdndrawingaprvldate.ClientID%>').value = "";
                 document.getElementById('<%=hdnteam.ClientID%>').value = "";
              document.getElementById('<%=Weightage.ClientID%>').value = "";

               document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = 0;
                 document.getElementById('<%=isdrawing.ClientID%>').checked = 0;

                   $(document.getElementById("<%=CmpOwner.ClientID%>")).val(0);
                  $(document.getElementById("<%=CmpOwner.ClientID%>")).change();

                  $(document.getElementById("<%=team.ClientID%>")).val(0);
              $(document.getElementById("<%=team.ClientID%>")).change();

                        document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/assets/images/user.jpg";

                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Project";
         
                        
                    }

        //$(document).ready(function IsPostBack() {
        //    Binddetails();
        //});


       

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'Addproject.aspx/Bindprojectdetail',
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
                var projectdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (projectdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = projectdetails.find("id").text();
                    
                    $("[id*=ProjectID]").val(projectdetails.find("ProjectID").text());
                    $("[id*=ProjectName]").val(projectdetails.find("ProjectName").text());
                    $("[id*=CmpOwner]").val(projectdetails.find("CmpOwner").text());
                    $("[id*=team]").val(projectdetails.find("team").text()); 
                    //$("[id*=ProjectStatus]").val(projectdetails.find("ProjectStatus").text());
                    $("[id*=ProjectAddress1]").val(projectdetails.find("ProjectAddress1").text());
                    $("[id*=ProjectAddress2]").val(projectdetails.find("ProjectAddress2").text()); 
                    $("[id*=City]").val(projectdetails.find("City").text()); 
                    $("[id*=PostalCode]").val(projectdetails.find("PostalCode").text()); 
                    $("[id*=StateName]").val(projectdetails.find("StateName").text()); 
                    $("[id*=Countryid]").val(projectdetails.find("Countryid").text()); 
                    $("[id*=ClientName]").val(projectdetails.find("ClientName").text()); 
                    //$("[id*=Priority]").val(projectdetails.find("Priority").text()); 
                    $("[id*=Budget]").val(projectdetails.find("Budget").text()); 
                    $("[id*=Actual]").val(projectdetails.find("Actual").text()); 
                    $("[id*=AwardDate]").val(projectdetails.find("AwardDate").text()); 
                    $("[id*=StartDate]").val(projectdetails.find("StartDate").text()); 
                    $("[id*=TargetDate]").val(projectdetails.find("TargetDate").text()); 
                    $("[id*=ApprovalDrawing]").val(projectdetails.find("ApprovalDrawing").text()); 
                    $("[id*=DrawingApprovalDate]").val(projectdetails.find("DrawingApprovalDate").text()); 


                    //if (projectdetails.find("Uploaddrawing").text() != "")
                    //{
                        
                    //    document.getElementById("empimg").src = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    //    document.getElementById("imgpath").href = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    //}
                    //else
                    //{
                    //    document.getElementById("imgpath").href = "#";
                    //    document.getElementById("empimg").src = "/assets/images/user.jpg";
                    //}


                    if (projectdetails.find("Uploaddrawing").text() != "" && projectdetails.find("Uploaddrawing").text() != "0")
                    {
                        
                        document.getElementById("empimg").src = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                        document.getElementById("imgpath").href = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    }
                    


                    document.getElementById('<%=hdnowner.ClientID%>').value = projectdetails.find("Owner").text();
                    document.getElementById('<%=hdnCountryid.ClientID%>').value = projectdetails.find("Countryid").text();
                    document.getElementById('<%=hdnstatename.ClientID%>').value = projectdetails.find("StateName").text();

                     document.getElementById('<%=ProjectStatus.ClientID%>').value = projectdetails.find("ProjectStatus").text();
                    document.getElementById('<%=hdnProjectStatus.ClientID%>').value = projectdetails.find("ProjectStatus").text();
                    
                    document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";
                
                     document.getElementById('<%=Priority.ClientID%>').value = projectdetails.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = projectdetails.find("Priority").text();

                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";
                  
                    document.getElementById('<%=hdnawarddate.ClientID%>').value = projectdetails.find("AwardDate").text();
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = projectdetails.find("StartDate").text();
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = projectdetails.find("TargetDate").text();
                    document.getElementById('<%=hdndrawingaprvldate.ClientID%>').value = projectdetails.find("DrawingApprovalDate").text();
                    document.getElementById('<%=hdnteam.ClientID%>').value = projectdetails.find("team").text();
                    document.getElementById('<%=Weightage.ClientID%>').value = projectdetails.find("Weightage").text();


                  if (projectdetails.find("isdrawing").text() == 1)
                  {
                      document.getElementById('<%=isdrawing.ClientID%>').checked = true;
                      document.getElementById("divaprovaldraing").style.display = "block";
                      document.getElementById("divaprovalDate").style.display = "block";
                      document.getElementById("divuploaddrawing").style.display = "block";

                  }

                  if (projectdetails.find("isdrawing").text() == 0)
                  {
                      document.getElementById('<%=isdrawing.ClientID%>').checked = false;
                      document.getElementById("divaprovaldraing").style.display = "none";
                      document.getElementById("divaprovalDate").style.display = "none";
                      document.getElementById("divuploaddrawing").style.display = "none";

                    }


                   if (projectdetails.find("checkprojectWeightage").text() == 1)
                   {
                      document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = true;

                   }

                   if (projectdetails.find("checkprojectWeightage").text() == 0)
                   {
                      document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = false;

                    }


                   
                   

               if (projectdetails.find("Owner").text() != "")
             {
             var idsemp = projectdetails.find("Owner").text().split(',');
            $(document.getElementById("<%=CmpOwner.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=CmpOwner.ClientID%>")).change();
            }

              if (projectdetails.find("team").text() != "")
             {
             var idsemp = projectdetails.find("team").text().split(',');
            $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=team.ClientID%>")).change();
            }


                   
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                    document.getElementById('<%=lbltitle.ClientID%>').innerHTML = "Edit Project";
                     //document.getElementById("btnsaveandmore").style.display = "none";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       
  

  
    <script>
        $( document ).ready(function() {
            Bindproject_data();
});
         function Bindproject_data() {
            
                debugger;
                  $.ajax({
                    url: 'Addproject.aspx/bindProjectdata',                  
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
            Bindproject_data_searchstus();
});
         function Bindproject_data_searchstus() {
            
                debugger;
                  $.ajax({
                    url: 'Addproject.aspx/bindProjectdata_searchstatus',                  
                      //data: '{}',
                      data: "{ 'RecordId': '" + $("[id*=hdnsearchstaus]").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails1,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetails1(response)
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
                    url: 'Addproject.aspx/Deleteprojectdata',
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
            Bindproject_data();
        }

    </script>

  

    <%--save data end  --%>


      <%--Owner details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeDataowner(val12A) {

            if (Page_ClientValidate("btnsaveowner")) {
               
                debugger
                
                // save all details

                var projectdetails = {};
                projectdetails.id = document.getElementById('<%=hdnchangeid.ClientID%>').value;;
                projectdetails.Owner = document.getElementById('<%=hdncahngeowner.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'Addproject.aspx/SaveProject_Owner',
                    data: '{projectdetails: ' + JSON.stringify(projectdetails) + '}',
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
                    url: 'Addproject.aspx/Bindprojectdetail_Owner',
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
                var projectdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (projectdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = projectdetails.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = projectdetails.find("Owner").text();

               if (projectdetails.find("Owner").text() != "")
             {
               var idsemp = projectdetails.find("Owner").text().split(',');
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

                var projectdetails = {};
                projectdetails.id = document.getElementById('<%=hdnteameid.ClientID%>').value;;
                projectdetails.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'Addproject.aspx/SaveProject_Team',
                    data: '{projectdetails: ' + JSON.stringify(projectdetails) + '}',
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
                   // Bindproject_data();
                    debugger
                    
                    document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

        function Binddetails_Team(RecordId,lbluserid) {
              debugger;

           document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                
                 
                $.ajax({
                    url: 'Addproject.aspx/Bindprojectdetail_Team',
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
                var projectdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (projectdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = projectdetails.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = projectdetails.find("team").text();
                   

               if (projectdetails.find("team").text() != "")
             {
               var idsemp = projectdetails.find("team").text().split(',');
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

    <%--status update start--%>
    <asp:HiddenField ID="hdnprojectid" runat="server" Value="0" />
    <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
            var projectid=document.getElementById("<%=hdnprojectid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addproject.aspx/UpdateProject_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'projectid': '" + projectid + "' ,'statusid': '" + statusid + "' ,'iscomplete': '" + iscomplete + "' }",
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
            debugger
            alert('Data Saved successfully.');
            var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
            //btnid.innerHTML = response.d;
            Bindproject_data();
        }
        
        function setprojectid(projectid,btnststusid)
        {
            debugger
            //var id = btnststusid;
            document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdnprojectid.ClientID%>").value = projectid;
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
                   var projectid=document.getElementById("<%=hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'Addproject.aspx/UpdatePriority_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'projectid': '" + projectid + "' ,'Propertyid': '" + Propertyid + "' }",
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
            var btnid = document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
           // Bindproject_data();
        }
        
        function setpriorityid(projectid,btnpriortyid)
        {
            debugger
            document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=hdnpropertID.ClientID%>").value = projectid;
        }
            </script>

    <%--proprety update end--%>




    <%--Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeExtended(val225A) {

            if (Page_ClientValidate("btnsaveextended")) {
               
                debugger
                
                // save all details


                var prjctid = document.getElementById('<%=hdnextendid.ClientID%>').value;
                var extendStrtdate = document.getElementById('<%=Extendatestrt.ClientID%>').value;
                var extendEnddate = document.getElementById('<%=Extendateend.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'Addproject.aspx/UpdateExtendeddatedtls',
                    data: "{ 'prjctid': '" + prjctid + "' ,'extendStrtdate': '" + extendStrtdate + "','extendEnddate': '" + extendEnddate + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessupdtexdate,
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
                function OnSuccessupdtexdate(response) {
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
                    url: 'Addproject.aspx/Bindprojectdetail_extendatedate',
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
                var projectdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (projectdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnextendid.ClientID%>').value = projectdetails.find("id").text();
                    
                    document.getElementById('<%=hdnextndstrt.ClientID%>').value = projectdetails.find("StartDate").text();
                    document.getElementById('<%=hdnextndend.ClientID%>').value = projectdetails.find("TargetDate").text();
                     document.getElementById('<%=Button2.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    
      <%--Extended date details bind and update data end  --%>
      


     <%--Add Tags details bind and update data start --%>
    
     <script type="text/javascript">
             
        function SvaeadTags(val126A) {

            if (Page_ClientValidate("btnsaveTags")) {
               
                debugger
                
                // save all details


                var prjctid = document.getElementById('<%=hdntagprjid.ClientID%>').value;
                var Tags = document.getElementById('<%=tags.ClientID%>').value;
                var TagsColor = document.getElementById('<%=tagscolor.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'Addproject.aspx/SaveTags',
                    data: "{ 'prjctid': '" + prjctid + "' ,'Tags': '" + Tags + "','TagsColor': '" + TagsColor + "' }",
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
                    document.getElementById('<%=tags.ClientID%>').value = "";
                    document.getElementById('<%=tagscolor.ClientID%>').value = "";
                    document.getElementById("btnclstags").click();
                    debugger
                    
                   
                   document.getElementById("lblAddtags_" + document.getElementById("<%=hdntagshtml.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Binddetails_Addtags(RecordId,lblAddtags) {
              
              debugger;
              document.getElementById('<%=hdntagprjid.ClientID%>').value = RecordId;
              document.getElementById("<%=hdntagshtml.ClientID%>").value = lblAddtags.split('_')[1];
             
        }


        


        
        


            </script>
    
      <%--Add Tags details bind and update data end  --%>




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
    var ddLOwner = $(document.getElementById("<%=CmpOwner.ClientID%>"));
     $(ddLOwner).select2({
        data: [
    <%=MultiOwner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        width: "100%"
    });

   
     $(document.getElementById("<%=CmpOwner.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=CmpOwner.ClientID%>")).val();
        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
        document.getElementById("<%=CmpOwner.ClientID%>").click();

    });
   
  
    </script>
    

    <script>
    debugger
    var ddLTeam = $(document.getElementById("<%=team.ClientID%>"));
    $(ddLTeam).select2({
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
                url: "FileUploadHandler.ashx?id="+id+"", 
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

    <%-- image upload end--%>



      <%-- show column hide show start--%>


    <script>
         document.getElementById("divgridview").style.display = "none";
     
    </script>


    <asp:HiddenField ID="showidcolumnid" runat="server" Value="0" />
     <script type="text/javascript"> 
         document.querySelectorAll('a.toggle-vis').forEach((el) => {
                debugger
             el.addEventListener('click', function (e) {
                    debugger
                    e.preventDefault();

                    let columnIdx = e.target.getAttribute('data-column');
                    let column = table.column(columnIdx);

                    // Toggle the visibility
                    column.visible(!column.visible());

                    var showhide_column = {};
                    alert(column.visible());
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
                        url: 'Addproject.aspx/Savecolumndata',
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
            //hidecolumn();

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

    <script type="text/javascript">
        function hidecolumngrid() {

            for (var i = 0; i < 25; i++)
            { 
                var aa = "ContentPlaceHolder1_hdn" + i; 
                if (document.getElementById(aa).value == "0")
                { 
                    let column = table.column(i); 
                    column.visible(false);

                }
            }
             //table.width("100%")
            document.getElementById("divgridview").style.display = "block";
            document.getElementById("divgridview").style.width = "100%";
            document.getElementById("ContentPlaceHolder1_lblprojectdata").style.width = "100%";
           
        }
    </script>

    <script>
           var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        prm.add_endRequest(function (sender, e) {
            debugger
            if (sender._postBackSettings.panelsToUpdate != null) {



         const table = new DataTable('.datatable', {  
        scroll:  true,
        scrollCollapse: true,
        paging:         true,
        searching: true,
        pageLength: 100,
       
                });



            }
        });
        };
    // hidecolumngrid();
    </script>

    <script>
    const table = new DataTable('.datatable', {  
        
        scrollX: true,
        scrollY:  300,
        scrollCollapse: true,
        paging: true,
        searching: true,
        
        });

        //Bindproject_data();

</script>

      <%-- show column hide show end--%>

</asp:Content>


