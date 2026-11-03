<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tags.ascx.cs" Inherits="ucontrol_Tags" %>
 <%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
    

    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
                <div class="row">
                    <div class="col-sm-3" style="background:#f7f7f7;padding-right:0;">
                        <div class="lead_form_body">
                            <div class="row">

                                 <div style="display: none">
                                <asp:Button ID="Button2" runat="server" Text="" OnClick="Button2_Click" />
                                 </div>

                                <div class="col-xxl-12 col-lg-12">
                                   <%-- <select class="form-control">
                                        <option>-- Select Project --</option>
                                        <option>All Project</option>
                                        <option selected="selected">Absolute Project</option>
                                    </select>--%>

                                     <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                        <cc1:DropDownListChosen ID="ddltagsproject" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ddltagsproject_SelectedIndexChanged" onchange="GetTagsDatabind()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntagsproject" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="mail-list mt-3 fs-md">
                                        <%--<a href="#" class="active">All <span class="badge bg-secondary-subtle text-secondary ms-auto">1</span></a>
                                        <a href="#">Status <span class="badge bg-secondary-subtle text-secondary ms-auto">0</span></a>
                                        <a href="#">Phases <span class="badge bg-secondary-subtle text-secondary ms-auto">0</span></a>
                                        <a href="#">Tasks <span class="badge bg-secondary-subtle text-secondary ms-auto">0</span></a>
                                        <a href="#">Forums <span class="badge bg-secondary-subtle text-secondary ms-auto">0</span></a>
                                        <a href="#">Issues <span class="badge bg-secondary-subtle text-secondary ms-auto">0</span></a>--%>

                                         <asp:Label ID="lbltagsdata" runat="server"></asp:Label>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-9" style="padding-left:0;">
                        <div class="card border-0">
                            <div class="card-header">
                                <div class="row">
                                    <div class="col-sm-auto">
                                        <div class="inner-head">
                                            <div class="opp-ico3">
                                                <i class="ri-price-tag-line"></i>
                                            </div>
                                           <%-- <div class="head-txt">
                                                <span class="badge badge-label" style="background:<%=tagscolorname%>;"><i class="ti ti-point-filled"></i> <%=tagsname%> <a href="javascript:void(0);"><i class="ph-caret-circle-down-fill"></i></a></span>
                                            </div>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm mt-1">
                                        
                                    </div>                                
                                </div> 
                                <hr class="mb-0 mt-2" />
                            </div>
                            <div class="card-body">
                            <div class="table-responsive">
                            <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
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
    </div>

            
                    
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico green">
                                            <i class="ri-price-tag-line"></i>
                                        </div>
                                       
                                    </div>
                                </div>
                                 
                                <div class="col-sm mt-1" style="display:none">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addTags"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
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
                                            <a class="dropdown-item toggle-vis" data-column="14"><i class="ri-check-line align-middle fs-lg"></i> Task</a>
                                            <a class="dropdown-item toggle-vis" data-column="15"><i class="ri-check-line align-middle fs-lg"></i> Issues</a>
                                            <a class="dropdown-item toggle-vis" data-column="16"><i class="ri-check-line align-middle fs-lg"></i> File</a>
                                            <a class="dropdown-item toggle-vis" data-column="17"><i class="ri-check-line align-middle fs-lg"></i> Add Columns</a>

                                        </div>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                                        <button type="button" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                                        <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                                        <%--<div class="dropdown-menu">
                                            <a class="dropdown-item" href="#">List</a>
                                            <a class="dropdown-item" href="#">Gantt</a>
                                        </div>--%>
                                    </div>
                                    </div>
                                </div>                                
                            </div> 
                        </div>
                        
                      
        

<div class="offcanvas offcanvas-end" id="addTags" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
          <label id="lbltitle" runat="server"><%=fns.getltext("Add Tags", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fs12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fse12Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
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
                                <div class="col-xxl-12 col-lg-12" id="divproject">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
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
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectId" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>

                               
                                   <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Description", "2")%></label>
                                    </div>
                                    <textarea id="description" runat="server" class="form-control" placeholder="Description"></textarea>
                                </div>
                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Tags", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="tags" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                     <input type="text" id="tags" runat="server" placeholder="Tags" class="form-control">

                                </div>
                                


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Color", "2")%> </label>
                                    </div>
                                     
                                 <input  data-jscolor="{}" id="tagscolor" class="jscolor form-control" runat="server">
                                     
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



<link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
   <%-- <script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
  <script src="/pms/assets/js/jscolor.js"></script>
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
        var element = document.getElementById("addTags");
        element.classList.add("canvas-fullscreen");  
    }
    function fse12Function(){
        var element = document.getElementById("addTags");
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

    <script>
    const table = new DataTable('.datatable', {  
        scrollX:  false,
        scrollCollapse: true,
        paging:         true,
        searching: true
        
    });
</script>


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




                          

<asp:HiddenField ID="hdntype" runat="server" />
<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
            document.getElementById("divproject").style.display = "none";
            

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


                  

                        // save all details

                        
                        var Tagsdetails = {};
                        Tagsdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; 
                        Tagsdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        Tagsdetails.Description = document.getElementById('<%=description.ClientID%>').value; 
                        Tagsdetails.Tags = document.getElementById('<%=tags.ClientID%>').value; 
                        Tagsdetails.TagsColor = document.getElementById('<%=tagscolor.ClientID%>').value; 
                       
                        $.ajax({
                            type: "POST",
                            url: 'AddTags.aspx/SaveTags',
                            data: '{Tagsdetails: ' + JSON.stringify(Tagsdetails) + '}',
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
                    
                    document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                    document.getElementById('<%=description.ClientID%>').value = "";
                    document.getElementById('<%=tags.ClientID%>').value = "";
                    document.getElementById('<%=tagscolor.ClientID%>').value = "";
                   
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        BindTags_data();
                    }
                    
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                       // document.getElementById("btncls").click();
                        BindTags_data();
                    }
                   

               
                    
                }



          function Cleardata() {
                       
                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                    document.getElementById('<%=description.ClientID%>').value = "";
                    document.getElementById('<%=tags.ClientID%>').value = "";
                    document.getElementById('<%=tagscolor.ClientID%>').value = "";

                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Tags";
         
                        
                    }

        

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'AddTags.aspx/BindTagsdetail',
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
                var Tagsdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Tagsdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = Tagsdetails.find("id").text();
                    
                    document.getElementById('<%=ProjectId.ClientID%>').value=(Tagsdetails.find("ProjectId").text());
                     document.getElementById('<%=description.ClientID%>').value=(Tagsdetails.find("Description").text());
                    document.getElementById('<%=tags.ClientID%>').value=(Tagsdetails.find("Tags").text());
                    document.getElementById('<%=tagscolor.ClientID%>').value=(Tagsdetails.find("TagsColor").text());
                   
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Tags";
                }
            }
            catch
                {
                }

            }


        
        


            </script>

       



    <script>
        $( document ).ready(function() {
            BindTags_data();
});
         function BindTags_data() {
             debugger
            
             //alert($("[id*=hdiprojid]").val());

                debugger;
                  $.ajax({
                      url: 'AddTags.aspx/bindTagsdata',
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
   
    function GetTagsDatabind()
    {
     
        <%--alert(document.getElementById('<%=ddltagsproject.ClientID%>').value);--%>
       <%-- document.getElementById('<%=Button2.ClientID%>').click();--%>
        BindDroptags_data();
    }
    

</script>

<script>
        $( document ).ready(function() {
            BindDroptags_data();
});
         function BindDroptags_data() {
             debugger

           <%-- alert(document.getElementById('<%=ddltagsproject.ClientID%>').value);--%>
              var RecordId = 0;
              RecordId = document.getElementById('<%=ddltagsproject.ClientID%>').value;

                debugger;
                  $.ajax({
                      url: 'AddTags.aspx/bindTags_Dropdata',
                      data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessTagsdrop,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessTagsdrop(response)
        {
            debugger
            var todolist = response.d;
            // alert(response.d);
                    let lbl = document.getElementById("<%=lbltagsdata.ClientID%>");
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
                    url: 'AddTags.aspx/DeleteTagsdata',
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
            BindTags_data();
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
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = milestonedetails.find("Owner").text();

               if (milestonedetails.find("Owner").text() != "")
             {
               var idsemp = milestonedetails.find("Owner").text().split(',');
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


    
  <%--Multi dropdown select start --%> 
   
 <%--Multi dropdown select start end--%> 




