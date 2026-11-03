<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="Status_Master.aspx.cs" Inherits="Status_Master" %>

<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="/pms/new/assets/css/icons.min.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="page-content">

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
                                            <p><%=fns.getltext("Status Master", "2")%></p>
                                           
                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                        <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                            <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addproject"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>

                                          <%--  <a href="javascript:void(0);" type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="modal" data-bs-target="#import_mdl"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></a>--%>


                                      <%--      <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>--%>
                                         <%--   <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>--%>
                                          <%--  <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>--%>
                                            <%--<div class="dropdown-menu show_column" data-simplebar style="max-height: 300px;">
                                                <a class="dropdown-item toggle-vis" data-column="0"><i class="ri-check-line align-middle fs-lg"></i>Select</a>
                                                <a class="dropdown-item toggle-vis" data-column="1"><i class="ri-check-line align-middle fs-lg"></i>S.No.</a>
                                                <a class="dropdown-item toggle-vis" data-column="2"><i class="ri-check-line align-middle fs-lg"></i>Project ID</a>
                                                <a class="dropdown-item toggle-vis" data-column="3"><i class="ri-check-line align-middle fs-lg"></i>Project Name</a>
                                                <a class="dropdown-item toggle-vis" data-column="4"><i class="ri-check-line align-middle fs-lg"></i>Owner</a>
                                                <a class="dropdown-item toggle-vis" data-column="5"><i class="ri-check-line align-middle fs-lg"></i>Team</a>
                                                <a class="dropdown-item toggle-vis" data-column="6"><i class="ri-check-line align-middle fs-lg"></i>Status</a>
                                                <a class="dropdown-item toggle-vis" data-column="7"><i class="ri-check-line align-middle fs-lg"></i>Priority</a>
                                                <a class="dropdown-item toggle-vis" data-column="8"><i class="ri-check-line align-middle fs-lg"></i>Budget</a>
                                                <a class="dropdown-item toggle-vis" data-column="9"><i class="ri-check-line align-middle fs-lg"></i>Actual</a>
                                                <a class="dropdown-item toggle-vis" data-column="10"><i class="ri-check-line align-middle fs-lg"></i>Start Date</a>
                                                <a class="dropdown-item toggle-vis" data-column="11"><i class="ri-check-line align-middle fs-lg"></i>Target Date</a>
                                                <a class="dropdown-item toggle-vis" data-column="12"><i class="ri-check-line align-middle fs-lg"></i>Timeline</a>
                                                <a class="dropdown-item toggle-vis" data-column="13"><i class="ri-check-line align-middle fs-lg"></i>Completion Status</a>
                                                <a class="dropdown-item toggle-vis" data-column="14"><i class="ri-check-line align-middle fs-lg"></i>Milestone</a>
                                                <a class="dropdown-item toggle-vis" data-column="15"><i class="ri-check-line align-middle fs-lg"></i>Task</a>
                                                <a class="dropdown-item toggle-vis" data-column="16"><i class="ri-check-line align-middle fs-lg"></i>Issues</a>
                                                <a class="dropdown-item toggle-vis" data-column="17"><i class="ri-check-line align-middle fs-lg"></i>File</a>

                                                <a class="dropdown-item toggle-vis text-mute" data-column="18"><i class="ri-check-line align-middle fs-lg"></i>Project Address</a>
                                                <a class="dropdown-item toggle-vis text-mute" data-column="19"><i class="ri-check-line align-middle fs-lg"></i>Client Name</a>
                                                <a class="dropdown-item toggle-vis text-mute" data-column="20"><i class="ri-check-line align-middle fs-lg"></i>Award Date</a>
                                                <a class="dropdown-item toggle-vis text-mute" data-column="21"><i class="ri-check-line align-middle fs-lg"></i>Approval of Drawing</a>
                                                <a class="dropdown-item toggle-vis text-mute" data-column="22"><i class="ri-check-line align-middle fs-lg"></i>Drawing Approval Date</a>
                                            </div>--%>
                                          <%--  <button type="button" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>--%>
                                           <%-- <button type="button" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>--%>
                                          <%--  <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>--%>
                                          <%--  <div class="dropdown-menu">
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

                                <div style="display: none;">

                                    <asp:Button ID="btndelete" runat="server" Text="delete" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="offcanvas offcanvas-end" id="addproject" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
        <div class="offcanvas-header border-bottom" style="padding: 14px 24px;">
            <h5 class="offcanvas-title" id="offcanvasRightLabel">
                <label id="lbltitle" runat="server"><%=fns.getltext("Add Status  Master", "2")%></label>

            </h5>
            <div class="hstack gap-2">
                <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
                <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
                <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
                <div style="display: none">
                    <button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
                </div>
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
                                        <asp:Button ID="Button3" runat="server" Text="" OnClick="Button1_Click" />

                                    </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />


                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">
                                                <%=fns.getltext("Status Name", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1"
                                                    ControlToValidate="Name" ValidationGroup="btnsave"
                                                    runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                            
                                        </div>
                                        <input type="text" id="Name" class="form-control" placeholder="Status Name..." runat="server">
                                    </div>

                                   
                                     <%--<div class="col-xxl-12 col-lg-12">

                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">
                                                <%=fns.getltext("Print Icon", "2")%></label><%--<label id="lblicon">&nbsp;&nbsp;<i class="bi bi-dash-circle"></i></label>
                                        </div>
                                       
                                       <cc1:DropDownListChosen ID="ddlicons" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Select icon ..." AllowSingleDeselect="true" onchange="updateIcon();">
                                                </cc1:DropDownListChosen>
                                    </div>--%>

                                    <div class="col-xxl-12 col-lg-12">

                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">
                                                <%=fns.getltext("Print Icon & Color", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3"
                                                    ControlToValidate="PrintingColor" ValidationGroup="btnsave"
                                                    runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                        </div>
                                        
                                        <div class="d-flex gap-2 align-items-center">
                                        <div>
                                         <div class="dropdown" >
    <button class="btn btn-light dropdown-toggle" style="border-radius:0;"
            type="button"
            data-bs-toggle="dropdown"
            id="selectediconBtn">
        <i class="bi bi-dash-circle"></i> None
    </button>

    <ul class="dropdown-menu">
        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bi bi-dash-circle','None')">
                <i class="bi bi-dash-circle"></i> None
            </a>
        </li>

        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bi bi-star','Star')">
                <i class="bi bi-star"></i> Star
            </a>
        </li>

        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bi bi-heart','Heart')">
                <i class="bi bi-heart"></i> Heart
            </a>
        </li>

        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bi bi-flag','Flag')">
                <i class="bi bi-flag"></i> Flag
            </a>
        </li>

        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bi bi-hourglass','Hourglass')">
                <i class="bi bi-hourglass"></i> Hourglass
            </a>
        </li>

        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bi bi-clock','Clock')">
                <i class="bi bi-clock"></i> Clock
            </a>
        </li>
        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('ri-share-forward-line','Forward')">
                <i class="ri-share-forward-line"></i> Forward
            </a>
        </li>
        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bx bx-history','History')">
                <i class="bx bx-history"></i> History
            </a>
        </li>
        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bx bx-check-circle','Check Circle')">
                <i class="bx bx-check-circle"></i> Check Circle
            </a>
        </li>
        <li>
            <a class="dropdown-item" href="javascript:void(0)"
               onclick="selectIcon('bx bx-archive','Archive')">
                <i class="bx bx-archive"></i> Archive
            </a>
        </li>
    </ul>
</div>
                                        </div>
                                        <div class="w-100">
                                            <input data-jscolor="{}" id="PrintingColor" class="jscolor form-control m-0" runat="server">
                                        </div>
                                            </div>
                                   <input type="hidden" id="hdnicon" runat="server" />

                                        </div>

                                    <div class="col-xxl-10 col-lg-12">
    <div class="d-flex align-items-center gap-3">

        <div class="form-check">
            <label class="form-label">
                <input class="form-check-input" type="radio" name="statusGroup" id="isdefault" runat="server" />
                <%=fns.getltext("Is Default", "2")%>
            </label>
        </div>

        <div class="form-check">
            <label class="form-label">
                <input class="form-check-input" type="radio" name="statusGroup" id="iscomplete" runat="server" />
                <%=fns.getltext("Is complete", "2")%>
            </label>
        </div>
        <div class="form-check">
            <label class="form-label">
                <input class="form-check-input" type="radio" name="statusGroup" id="isrework" runat="server" />
                <%=fns.getltext("Is rework", "2")%>
            </label>
        </div>
    </div>
</div>
                                   <%-- <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="isdefault" runat="server" group="a1" /><%=fns.getltext("Is Default", "2")%> 
                                              
                                        </label>
                                            
                                        </div>
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                           <label class="form-label">
                                            <input class="form-check-input" type="checkbox" id="iscomplete" runat="server" group="a1"/><%=fns.getltext("Is complete", "2")%>   
                                        </label>
                                            
                                        </div>
                                    </div>
                                </div>--%>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right">
            <div id="pnlimg" style="display: none;">
                <img src="/Image/loading8.gif" runat="server" id="img1" />
            </div>
            <div class="btn-group">

                <div class="btn-group" id="divsub" style="display: block;">
                    <a href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                    <%-- <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>--%>
                </div>
            </div>

            <a href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()" aria-label="Close">Close</a>
            <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display: none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        </div>
    </div>

    <div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
        <div style="display: none;">
            <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
            <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
        </div>
        <iframe id='inner_popup' width="100%" height="100%" style="padding: 0px 0px;" border="0px"></iframe>
    </div>


    <div id="addcolor" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title"><%=fns.getltext("Set Color", "2")%><span style='width:10px;height:10px;display:inline-block;background:red;margin-left:5px;'  id="stageoldcolor" ></span></h5>
                <button type="button" id="btnclscolor" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>



            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                     <div class="row">
                        
                            <asp:HiddenField ID="hdncolorprjid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdncolorhtml" runat="server" Value="0" />

                        <div class="col-xxl-12 col-lg-12">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">
                                    <%=fns.getltext("New Color", "2")%><asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ControlToValidate="LabelColor" ValidationGroup="btnsavecolor"
                                        runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                            </div>
                            <div class="nano-colorpicker">
                              <%--  <asp:TextBox data-jscolor="{}" ID="LabelColor" runat="server" class="jscolor form-control"></asp:TextBox>--%>
                                  <asp:TextBox class="jscolor{}" ID="LabelColor" runat="server"></asp:TextBox>
                                 
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsavecolor" class="btn btn-primary" onclick="Svaeadcolor('saveAddcolor');">Update</button>

            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>


    <%-- --------------------------------------------------------------------------------------------%>
    <script src="/pms/assets/libs/sortablejs/Sortable.min.js"></script>
    <script src="/pms/assets/js/pages/nestable.init.js"></script>
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css">
    <script src="/pms/assets/js/jquery-3.7.0.js"></script>
    <script src="/pms/assets/js/select2.min.js"></script>
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/jscolor.js"></script>

    <script>
        function fsFunction() {
            var element = document.getElementById("addproject");
            element.classList.add("canvas-fullscreen");
        }
        function fseFunction() {
            var element = document.getElementById("addproject");
            element.classList.remove("canvas-fullscreen");
        }
    </script>

    <script>
        const table = new DataTable('.datatable', {
            scrollX: false,
            scrollCollapse: true,
            paging: true,
            searching: true

        });
    </script>
    <asp:HiddenField ID="hdnsavetype" runat="server" />
    <%--save data end  --%>

    <script type="text/javascript">
        function showdivimg() {
            if (Page_ClientValidate("btnclose")) {
                //if (confirm('Do you want to Leave this page ?')) {

                    Cleardata();
                    document.getElementById("btnclose").click();

                    return true;
                //}
                //else {
                   // return false;
                //}
            }
        }

        function SvaeData(val1A) {
            //debugger
            if (Page_ClientValidate("btnsave")) {
                document.getElementById('<%=hdnsavetype.ClientID%>').value = val1A;

                if (confirm('Are You Sure You want to Submit This Entry!')) {
                    document.getElementById("pnlimg").style.display = "block";
                    document.getElementById("divsub").style.display = "none";

                }
                else {
                    return false;
                }





                // save all details

                //debugger
                var printingdetails = {};

                printingdetails.id = document.getElementById('<%=hdnid.ClientID%>').value;
                printingdetails.StatusName = document.getElementById('<%=Name.ClientID%>').value;
                printingdetails.PrintColor = document.getElementById('<%=PrintingColor.ClientID%>').value;
                printingdetails.PrintIcon = document.getElementById('<%=hdnicon.ClientID%>').value;
                printingdetails.iscomplete = 0
                 if (document.getElementById('<%=iscomplete.ClientID%>').checked == true)
                 {
                        printingdetails.iscomplete = 1
                 }
                 printingdetails.isdefault = 0
                 if (document.getElementById('<%=isdefault.ClientID%>').checked == true)
                 {
                        printingdetails.isdefault = 1
                }
                 printingdetails.isrework = 0
                 if (document.getElementById('<%=isrework.ClientID%>').checked == true)
                 {
                        printingdetails.isrework = 1
                 }
                //alert(printingdetails);
                $.ajax({
                    type: "POST",
                    url: '/pms/settings/Status_Master.aspx/SavePrintingMaster',
                    data: '{printingdetails: ' + JSON.stringify(printingdetails) + '}',
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccess,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }

                });
                return false;



            };
        }
        function OnSuccess(response) {
            //debugger
            //commntprt
            alert("Data Saved successfully.");
            $("[id*=Name]").val("");
            //$("[id*=Type]").val("");
            $("[id*=PrintingColor]").val("");

            document.getElementById('<%=Name.ClientID%>').value = "";
            document.getElementById('<%=PrintingColor.ClientID%>').value = "";
              document.getElementById('<%=iscomplete.ClientID%>').checked = 0;
              document.getElementById('<%=isdefault.ClientID%>').checked = 0;
            if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save") {

                document.getElementById("pnlimg").style.display = "none";
                document.getElementById("divsub").style.display = "block";
                document.getElementById("btncls").click();
                BindPrinting_data();
            }

            if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore") {
                document.getElementById("pnlimg").style.display = "none";
                document.getElementById("divsub").style.display = "block";
                BindPrinting_data();
            }



        }



        function Cleardata() {
            //debugger;

            $("[id*=Name]").val("");
            //$("[id*=Type]").val("");
            $("[id*=PrintingColor]").val("");


            document.getElementById('<%=Name.ClientID%>').value = "";
            document.getElementById('<%=PrintingColor.ClientID%>').value = "";
             document.getElementById('<%=iscomplete.ClientID%>').checked = 0;
             document.getElementById('<%=isdefault.ClientID%>').checked = 0;
            <%--document.getElementById('<%=Button1.ClientID%>').click();--%>
            document.getElementById('<%=lbltitle.ClientID%>').innerHTML = "Add Status Mater";


        }

        function Binddetails(RecordId) {


            //debugger;

            $.ajax({
                url: '/pms/settings/Status_Master.aspx/BindPrintingdetail',
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

            //debugger
            try {
                var printingdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (printingdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = printingdetails.find("id").text();
                    document.getElementById('<%=Name.ClientID%>').value = printingdetails.find("ProjectStatus").text(); 
 
                     document.getElementById('<%=PrintingColor.ClientID%>').value = printingdetails.find("PrintColor").text().replace("#","");
try
{
                      setSelectedIcon(printingdetails.find("PrintIcon").text());
                 }
catch{}
 document.getElementById('<%=iscomplete.ClientID%>').checked = false;
if (printingdetails.find("iscomplete").text() == 1)
                  {
                      document.getElementById('<%=iscomplete.ClientID%>').checked = true;

                  }
document.getElementById('<%=isdefault.ClientID%>').checked = false;
                  if (printingdetails.find("isdefault").text() == 1)
                  {
                      document.getElementById('<%=isdefault.ClientID%>').checked = true;

                    }
                    document.getElementById('<%=isrework.ClientID%>').checked = false;
                  if (printingdetails.find("isrework").text() == 1)
                  {
                      document.getElementById('<%=isrework.ClientID%>').checked = true;

                  }

                    document.getElementById('btnsave').value = "Update";
                    document.getElementById('<%=lbltitle.ClientID%>').innerHTML = "Edit Status Master";
                }
            }
            catch
            {
            }

        }


    </script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        /* errorprt*/
        var jq = jQuery.noConflict();


        jq(document).ready(function () {
            BindPrinting_data();
        });
        function BindPrinting_data() {

            //debugger;
            $.ajax({
                url: '/pms/settings/Status_Master.aspx/bindPrintingMasterData',
                data: '{}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: OnSuccessgetdetails,
                error: function (response) {

                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });

        }
        function OnSuccessgetdetails(response) {
            //debugger
            var todolist = response.d;
            let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
            lbl.innerHTML = '';
            lbl.innerHTML = todolist;

            /*errorprt*/

            table = new DataTable('.datatable', {
                scrollX: true,
                scrollCollapse: true,
                paging: true,
                searching: true

            });

        }

    </script>


    
     <%--Add Color details bind and update data start --%>
    
     <script type="text/javascript">
             
        function Svaeadcolor(val126A) {

            if (Page_ClientValidate("btnsavecolor")) {
               
                //debugger
                
                // save all details


                var update_id = document.getElementById('<%=hdncolorprjid.ClientID%>').value;
                var Color = document.getElementById('<%=LabelColor.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: '/pms/settings/Status_Master.aspx/UpdateColor',
                    data: "{ 'update_id': '" + update_id + "' ,'Color': '" + Color + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessupdatecolor,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
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
                function OnSuccessupdatecolor(response) {
                    //debugger
                    //alert("Data Saved successfully."); 
                    document.getElementById('<%=LabelColor.ClientID%>').value = "";
                    document.getElementById("btnclscolor").click();
                    //debugger
                    
                   
                   document.getElementById("lblAddcolor_" + document.getElementById("<%=hdncolorhtml.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Binddetails_Addcolor(RecordId,color,lblAddcolor) {
              
              //debugger;
             document.getElementById('<%=hdncolorprjid.ClientID%>').value = RecordId;
              document.getElementById("<%=hdncolorhtml.ClientID%>").value = lblAddcolor.split('_')[1];
               document.getElementById("stageoldcolor").style.backgroundColor = color;
               var v = document.getElementById('<%=LabelColor.ClientID%>');
              if (v.value == "") {
               v.removeAttribute('style');
               }
               else if (chkcolor == 0) {
               v.value = "";
               v.removeAttribute('style');
               chkcolor = 1;
               }
              
             
        }


        


        
        


            </script>
    
      <%--Add Color details bind and update data end  --%>


    <script>
        function deletedata(RecordId) {
            //debugger

            if (confirm('Do you really want to delete the Entry?')) {

            }
            else {
                return false;
            }

            $.ajax({
                url: '/pms/settings/Status_Master.aspx/DeletePrintingMaster',
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
            BindPrinting_data();
        }

    </script>
    <script>
function selectIcon(iconClass, text) {
//debugger
    document.getElementById('selectediconBtn').innerHTML =
        '<i class="' + iconClass + '"></i> ' + text;

   
    document.getElementById('<%=hdnicon.ClientID %>').value = iconClass;
}
function setSelectedIcon(iconClass) {

    var text = "None";

    switch(iconClass) {
        case "bi bi-star": text = "Star"; break;
        case "bi bi-heart": text = "Heart"; break;
        case "bi bi-flag": text = "Flag"; break;
        case "bi bi-hourglass": text = "Hourglass"; break;
        case "bi bi-clock": text = "Clock"; break;
        case "ri-share-forward-line": text = "Forward"; break;
        case "bx bx-history": text = "History"; break;
        case "bx bx-check-circle": text = "Check Circle"; break;
        case "bx bx-archive": text = "Archive"; break;
    }

    document.getElementById('<%=hdnicon.ClientID%>').value = iconClass;
    document.getElementById('selectediconBtn').innerHTML =
        '<i class="' + iconClass + '"></i> ' + text;
}
<%--function updateIcon() {
    var ddl = document.getElementById('<%=ddlicons.ClientID %>');
    var icon = ddl.value;

    document.getElementById('lblicon').innerHTML =
        '&nbsp;&nbsp;<i class="' + icon + '"></i>';
}--%>
</script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <link href="/pms/assets/css/newchoosen.css" rel="stylesheet" />

<script src="/pms/assets/js/newchoosen.js"></script>
 
</asp:Content>



