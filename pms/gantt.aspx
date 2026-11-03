<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="gantt.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="/pms/assets/js/dhtmlxgantt.js"></script>
	<link rel="stylesheet" href="/pms/assets/css/dhtmlxgantt_meadow.css">

	<script src="/pms/assets/js/testdatanew.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        
         <h5 class="offcanvas-title"><%=Projectname%></h5>

       <%-- <div class="offcanvas-header border-bottom" style="padding:22px 24px;">
        <h5 class="offcanvas-title"></h5>--%>

        <div class="hstack gap-2 cls_btn">
            <button type="button" id="zoominidmulti" onclick="zoominmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutidmulti" onclick="zoomoutmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="closepopupmainmulti()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>

    <div class="offcanvas-body p-0 overflow-hidden">
        <div class="row">
            <div class="col-lg-12">
                <div class="card">
                    <div class="card-header pt-2 pb-2">
                        <div class="row">
                            <div class="col-sm-auto">
                                <div class="inner-head">
                                    <div class="opp-ico orange">
                                        <i class="ri-pencil-ruler-2-line"></i>
                                    </div>
                                    <div class="head-txt">
                                        <p>Project Management</p>                                                    
                                        <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside">
                                            <h2><label id="ContentPlaceHolder1_lblowner">All Projects</label> <i class="bi bi-caret-down-fill"></i></h2>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-end">
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">All Projects</span></a>                                             
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Under Negotiation</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Won</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Scheduled</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Working</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Stuck</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Done</span></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm mt-1">
                                <div class="d-flex justify-content-sm-end">
                                <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                    <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addproject"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                                    <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                                    <div class="dropdown-menu">
                                        <%--<a class="dropdown-item" href="/pms/makemilestone.aspx?id=<%=strquery%>">List</a>
                                        <a class="dropdown-item" href="/pms/gantt.aspx?id=<%=strquery%>">Gantt</a>--%>
                                        <a class="dropdown-item" href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multinew" onclick="loadsrcnew('inner_popup_multinew','/pms/makemilestone.aspx<%=strquery%>')">List</a>
                                        <a class="dropdown-item" href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multinew" onclick="loadsrcnew('inner_popup_multinew','/pms/gantt.aspx<%=strquery%>')">Gantt</a>
                                    </div>
                                </div>
                                </div>
                            </div>                                
                        </div> 
                    </div><!-- end card header -->
                </div>
            </div>


            <div class="col-lg-12">                    
                                        
                        <div id="gantt_here" style='width:100%; height:430px'></div>                        
                   
            </div>
        </div>           
    </div>




<div class="offcanvas offcanvas-end" id="addproject" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">Add Project</h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
                            <div class="row">
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Project ID</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Project Name</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Owner</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Status</label>
                                    </div>
                                    <select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Under Negotiation</option>
                                        <option>Won</option>
                                        <option>Scheduled</option>
                                        <option>Working</option>
                                        <option>Stuck</option>
                                        <option>Done</option>
                                    </select>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Project Address</label>
                                    </div>
                                    <div class="row">
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" class="form-control" placeholder="Address Line 1">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" class="form-control" placeholder="Address Line 2">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" class="form-control" placeholder="City">
                                        </div>
                                        <div class="col-xxl-5 col-lg-5">
                                            <input type="text" class="form-control" placeholder="Zip/Postal Code">
                                        </div>
                                        <div class="col-xxl-7 col-lg-7">
                                            <input type="text" class="form-control" placeholder="State/Province">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <select class="form-control">
                                                <option>-- Select Country --</option>
                                                <option>India</option>
                                                <option>USA</option>
                                                <option>UAE</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Client Name</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Date of Award</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Start Date</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Target Date</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Approval of Drawing</label>
                                    </div>
                                    <select class="form-control">
                                        <option>Yes</option>
                                        <option>No</option>
                                    </select>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Drawing Approval Date</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Upload drawing</label>
                                    </div>
                                    <input type="file" class="form-control">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Save</a>
        <a href="javascript:void(0);" class="btn btn-secondary">Save & Add More</a>
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
<div class="offcanvas offcanvas-end" id="addcolumns" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title"">Add Columns</h5>
        <div class="hstack gap-2">
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div style="height: calc(100vh - 55px);overflow:auto;">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="add_show_new show_column mb-3">
                            <a class="dropdown-item toggle-vis text-mute" data-column="19"><i class="ri-check-line align-middle fs-lg"></i> Project Address</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="20"><i class="ri-check-line align-middle fs-lg"></i> Client Name</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="21"><i class="ri-check-line align-middle fs-lg"></i> Award Date</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="22"><i class="ri-check-line align-middle fs-lg"></i> Approval of Drawing</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="23"><i class="ri-check-line align-middle fs-lg"></i> Drawing Approval Date</a>
                        </div>
                        <div class="accordion">
                            <div class="accordion-item text-black" style="border-radius:0px;">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" style="border-radius:0px;padding:12px;" type="button" data-bs-toggle="collapse"data-bs-target="#AddCustomColumns">+ Add Custom Columns</button>
                                </h2>
                                <div id="AddCustomColumns" class="accordion-collapse collapse">
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
                                            <div class="col-xxl-12 col-lg-12 mb-2" style="display:none;">
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
                                            <div class="col-xxl-12 col-lg-12 mb-2" style="display:none;">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Add Checkbox Values</label>
                                                </div>
                                                <div class="bg-light p-3 border">
                                                    <div class="input-group mb-3">
                                                        <input type="text" class="form-control" aria-label="Recipient's username with two button addons">
                                                        <button class="btn btn-success" type="button">Add</button>
                                                    </div>
                                                    <div class="list-group nested-list nested-sortable-handle custom-sortable">
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Checkbox 1</div>
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
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Checkbox 2</div>
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
                                            <div class="col-xxl-12 col-lg-12 mb-2" style="display:none;">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Add Radiobox Values</label>
                                                </div>
                                                <div class="bg-light p-3 border">
                                                    <div class="input-group mb-3">
                                                        <input type="text" class="form-control" aria-label="Recipient's username with two button addons">
                                                        <button class="btn btn-success" type="button">Add</button>
                                                    </div>
                                                    <div class="list-group nested-list nested-sortable-handle custom-sortable">
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Radiobox 1</div>
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
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Radiobox 2</div>
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
                                                <input type="text" class="form-control" style="">
                                                <textarea class="form-control" style="display:none;"></textarea>
                                                <select class="form-control" style="display:none;">
                                                    <option>-- Select --</option>
                                                    <option>Select 1</option>
                                                    <option>Select 2</option>
                                                    <option>Select 3</option>
                                                </select>
                                                <div class="mb-2" style="display:none">
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
                                            <div class="col-xxl-12 col-lg-12 mt-3">
                                                <a href="javascript:void(0);" class="btn btn-success">Add</a>
                                            </div>
                                        </div>
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
<div id="conversation" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Conversation</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pt-0" style="min-height:465px;">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs-custom nav-tab-rpv"
                    role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" data-bs-toggle="tab" href="#Comments1" role="tab">Comments</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#Documents1" role="tab">Documents
                            
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#ActivityLog1" role="tab">Activity Log</a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content text-muted">
                    <div class="tab-pane active" id="Comments1" role="tabpanel">                        
                        <div class="chat-conversation p-3" data-simplebar style="height: 350px;">
                            <ul class="list-unstyled chat-conversation-list chat-sm">
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-2.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">                                                
                                            <h6>Frank Hook</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good morning All</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list right">
                                    <div class="conversation-list">
                                        <div class="user-chat-content">
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good morning, How are you? What about our next meeting?</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-1.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">                                                
                                            <h6>Christi</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good morning</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-2.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">
                                            <h6>Frank Hook</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Yeah everything is fine. Our next meeting tomorrow at 10.00 AM</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->

                                <li class="chat-list right">
                                    <div class="conversation-list">
                                        <div class="user-chat-content">
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Wow that's great</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-1.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">                                                
                                            <h6>Christi</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                            </ul>
                        </div>                        
                        <div class="border-top border-top-dashed">
                            <div class="row g-2 mx-3 mt-2 mb-0">
                                <div class="col">
                                    <div class="position-relative">
                                        <input type="text" class="form-control border-light bg-light" placeholder="Enter Message...">
                                    </div>
                                </div><!-- end col -->
                                <div class="col-auto">
                                    <button type="submit" class="btn btn-secondary"><i class="ti ti-send"></i></button>
                                </div><!-- end col -->
                            </div><!-- end row -->
                        </div>
                    </div>
                    <div class="tab-pane" id="Documents1" role="tabpanel">
                        <%--<div class="lead_form_body p-2 pb-0 mt-2">
                            <input type="text" class="form-control" placeholder="File Name">
                        </div>   --%> 
                        <div class="input-group p-2 mb-2 mt-2">
                            <input type="file" class="form-control" style="border-color:#777;border-radius:0px;" aria-label="Upload">
                            <button class="btn btn-success" style="border-radius:0px;"  type="button">Upload</button>                                                    
                        </div>
                            
                        <div class="p-2 pt-0 pb-0" data-simplebar style="max-height: 300px;">
                            <%--<p>No file selected.</p>--%>
                            <table class="table table-sm text-left">
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-pdf-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-word-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-excel-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-pdf-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-word-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-image-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-pdf-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-word-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-image-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="tab-pane" id="ActivityLog1" role="tabpanel">
                        <div class="table-responsive mt-3" data-simplebar style="height: 350px;">
                            <table class="table align-middle table-striped table-hover table-nowrap table-sm">
                                <thead class="table-light">
                                    <tr>
                                        <th align="center" width="30">S. No.</th>
                                        <th align="left">Owner</th>
                                        <th align="left">Project Name</th>
                                        <th align="left">Date & Time</th>
                                        <th align="left">Log Type</th>
                                        <th align="center">Old Value</th>
                                        <th width="15"></th>
                                        <th align="center">New Value</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td align="center">1</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Status</td>
                                        <td align="center"><span class="badge bg-danger text-uppercase new_badge">Stuck</span></td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center"><span class="badge bg-secondary text-uppercase new_badge">Working</span></td>
                                    </tr>
                                    <tr>
                                        <td align="center">2</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Priority</td>
                                        <td align="center"><span class="badge text-secondary bg-secondary-subtle text-uppercase new_badge">Low</span></td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center"><span class="badge text-danger bg-danger-subtle text-uppercase new_badge">High</span></td>
                                    </tr>
                                    <tr>
                                        <td align="center">3</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Budget</td>
                                        <td align="center">-</td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center">$ 5000</td>
                                    </tr>
                                    <tr>
                                        <td align="center">4</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Target Date</td>
                                        <td align="center">25/02/2024</td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center">29/02/2024</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>                                        
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="changeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <div class="col-sm-12 mb-1">
                            <select class="form-control">
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
                            </select>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Save</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
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
	gantt.templates.rightside_text = function (start, end, task) {
		return "ID: #" + task.id;
	};

	gantt.templates.leftside_text = function (start, end, task) {
		return task.duration + " days";
	};

	gantt.init("gantt_here");
	gantt.parse(demo_tasks);
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

    <script>
        function zoominmulti() {
            window.parent.document.getElementById("btnclosemainpopupzoominmulti").click();
            document.getElementById("zoomoutidmulti").style.display = "block";
            document.getElementById("zoominidmulti").style.display = "none";            
        }
        function zoomoutmulti() {
            window.parent.document.getElementById("btnclosemainpopupzoomoutmulti").click();
            document.getElementById("zoominidmulti").style.display = "block";
             document.getElementById("zoomoutidmulti").style.display="none";            
        }
        function closepopupmainmulti() {
            window.parent.document.getElementById("btnclosemainpopupmulti").click();            
        }
    </script>
</asp:Content>

