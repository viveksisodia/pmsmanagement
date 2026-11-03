<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="AddSTDependency.aspx.cs" Inherits="AddSTDependency" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
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
    </style>
    
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="loading" style="
    position: fixed;
    width: 100%;
    height: 100%;
    background: rgba(255,255,255,0.8);
    z-index: 9999;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
">
    <img src="/pms/image/loading8.gif" alt="Loading..." />
    
</div>
    <script>
$(document).ready(function() {
    $('#loading').fadeOut('slow');
});
$(window).on('load', function() {
    // Hide the loading overlay when page + all resources have loaded
    $('#loading').fadeOut('slow');
});
    </script>
<div class="offcanvas-header border-bottom" style="padding:14px 24px;">
    
       <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico green">
                                            <i class="ri-flag-2-line"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p><%=milestonenamename%></p>                                                    
                                           <span>
               <b><%=Projectname%></b>
           </span>
                                        </div>
                                    </div>
                                </div>
           
       
        
        <div class="hstack gap-2 cls_btn">
            <%--<button type="button" id="zoominid" onclick="zoomin()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutid" onclick="zoomout()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            --%>
            <%if (ischeckCreate == 1)
                {%>
            <a href="javascript:void(0);" id="btnaddnew" class="btn btn-outline-primary btn-icon" onclick="addnewdependency()" style="<%=strbtnnew%>"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>            
               <%}%>
            <button type="button" class="btn btn-danger btn-icon" onclick="closepopupmain()"   aria-label="Close" <%=strdisplay_close%>><i class="ph-x-bold"></i></button>
        </div>
    </div>

    <asp:HiddenField ID="hdndependid" runat="server" Value="0" />
    <asp:HiddenField ID="hdndependtype" runat="server" Value="" />
     <asp:HiddenField ID="hdnmid" runat="server" Value="0" />
     <asp:HiddenField ID="hdntid" runat="server" Value="0" />
     <asp:HiddenField ID="hdnstid" runat="server" Value="0" />
     <asp:HiddenField ID="hdnpid" runat="server" Value="0" />
    <asp:HiddenField ID="hdnprojecttype" runat="server" />
    
    <div class="offcanvas-body p-0 overflow-hidden">
       <%-- <div data-simplebar style="height: calc(100vh - 57px);">  --%>
            <div style="height: calc(100vh - 57px);">  
            <div class="acitivity-timeline p-4">
                <div class="card border-0" id="dvnew" style="display:none">
                     <div class="lead_form_body p-0 pb-2">
                    <div class="row">
                                <div style="display: none">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                             <asp:Button ID="btnexist" runat="server" Text="" OnClick="ddlsubtask1_SelectedIndexChanged"/>
                                        </ContentTemplate>
                                   </asp:UpdatePanel>
                               
                                <asp:Button ID="btnedit" runat="server" Text="" OnClick="btnedit_Click"/>
                                <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click"/>
                                <asp:Button ID="btncancel" runat="server" Text="" OnClick="btncancel_Click"/>
                                <asp:Button ID="btntask" runat="server" Text="" OnClick="btntask_Click"/>
                                <asp:Button ID="btnsubtask" runat="server" Text="" OnClick="btnsubtask_Click"/>
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                 <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                 <asp:HiddenField ID="HiddenField2" runat="server" />

                                 <asp:HiddenField ID="hdnmil_stoneid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntasklis_tid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntask_id" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnsubtask_id" runat="server" Value="0" />
                                

                        <div class="col-xxl-12 col-lg-12" <%=strmilestone%>>
                        <div class="head_sec mb-2">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="mb-0"><%=fns.getltext("Dependency For", "2")%></h5>
                                </div>
                            </div>
                        </div>
                        </div>
                                <div class="col-xxl-4 col-lg-4" id="divmilestone1" <%=strmilestone%>>
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ddlMilestone" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="type here ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ddlmilestone_SelectedIndexChanged" >
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>
                        <div class="col-xxl-4 col-lg-4" id="divtask1" <%=strtask%>>
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>

                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnedit" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="ddlMilestone" EventName="SelectedIndexChanged" />
                                              
                                          </Triggers>
                                         <ContentTemplate>
                                      <cc1:DropDownListChosen ID="ddltask1" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="type here ..." AllowSingleDeselect="true" AutoPostBack="true"  OnSelectedIndexChanged="ddltask1_SelectedIndexChanged" >
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntask1" runat="server" Value="0" />
                                            
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    
                                       
                                          



                                </div>
                        <div class="col-xxl-4 col-lg-4" id="divsubtask1" <%=strsubtask%>>
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("SubTask", "2")%></label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnedit" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="ddltask1" EventName="SelectedIndexChanged" />
                                              
                                          </Triggers>
                                         <ContentTemplate>
                                     <cc1:DropDownListChosen ID="ddlsubtask1" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="type here ..." AllowSingleDeselect="true" AutoPostBack="false"  onchange="bindexist();">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnsubtask1" runat="server" Value="0" />
                                          
                                            
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       



                                </div>

                        <script>
                            function hidediv_task(ctrtask){
                                if (ctrtask == "1") {
                                    document.getElementById("divtask1").style.display = '';
                                }
                                else {
                                    document.getElementById("divtask1").style.display = 'none';
                                }
                                
                            }
                            function hidediv_task(ctrtask) {
                                if (ctrlsubtask == "1") {
                                    document.getElementById("divsubtask1").style.display = '';
                                }
                                else {
                                    document.getElementById("divsubtask1").style.display = 'none';
                                }
                                
                            }
                        </script>

                           <div class="col-xxl-12 col-lg-12">
                        <div class="head_sec mb-2">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="mb-0"><%=fns.getltext("Dependency On", "2")%></h5>
                                </div>
                            </div>
                        </div>
                        </div>
                           <div class="col-xxl-6 col-lg-6" id="divproject" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%></label>
                                    </div>
                                 
                                      
                                       <cc1:DropDownListChosen ID="ProjectId" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" >
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectId" runat="server" Value="0" />
                                          

                                </div>   
                        <div class="col-xxl-3 col-lg-3">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>
                                 
                                       <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />
                                         </Triggers>
                                         <ContentTemplate>--%>
                                       <%--<cc1:DropDownListChosen ID="ddldmid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" onchange="bindtask(this.id)" >
                                                </cc1:DropDownListChosen>--%>
                                             <asp:DropDownList ID="ddldmid" CssClass="form-control" runat="server" width="450px" AutoPostBack="false"
                                                     onchange="return bindtask(this.id)" >
                                                </asp:DropDownList>
                                             <asp:HiddenField ID="hdndmid" runat="server" Value="0" />
                                           <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>

                                </div> 

                               <div class="col-xxl-3 col-lg-3" id="divtask" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>
                                  

                                    <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btntask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>--%>
                                   <%--    <cc1:DropDownListChosen ID="taskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Task ..." AllowSingleDeselect="true" onchange="bindsubtask(this.id)">
                                                </cc1:DropDownListChosen>--%>

                                       <asp:DropDownList ID="taskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Task ..." AllowSingleDeselect="true" onchange="bindsubtask(this.id)">
                                                </asp:DropDownList>


                                             <asp:HiddenField ID="hdntaskid" runat="server" Value="0" />
                                         <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>


                                </div>


                                <div class="col-xxl-3 col-lg-3" id="divsubtask" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task", "2")%></label>
                                    </div>
                                  

                                    <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btncancel" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnsubtask" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="subtaskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Sub Task ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                            
                                           </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                      <asp:DropDownList ID="subtaskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Sub Task ..." AllowSingleDeselect="true" >
                                                </asp:DropDownList>
                                     <asp:HiddenField ID="hdnsubtaskid" runat="server" Value="0" />

                                </div>
                        <div class="col-xxl-3 col-lg-3">
                           <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">&nbsp;</label>
                                    </div>
         <div id="pnlimgDM" style="display: none;">
                        <img src="/Image/loading8.gif" id="ContentPlaceHolder1_Issues_img2">
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsubDM" style="display: block;">
                            <a href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData();" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-danger " id="btncncl" onclick="discarddata();" aria-label="Save">Cancel</a>
                            
                        </div>
                    </div>
      

    
                        </div>
                                
                               </div>
                </div>

                        
                                                                
                            
                </div>
                 <div class="card-body" id="dvdetails">
                     <div class="col-xxl-12 col-lg-12">
                        <div class="head_sec mb-2">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="mb-0"><%=fns.getltext("Dependency Details", "2")%></h5>
                                </div>
                            </div>
                        </div>
                        </div>
                       <div class="card border-0" id="dvfilter" <%=strdvfilter%>>
                     <div class="lead_form_body p-0 pb-2">
                             <div class="row">
                            <div class="col-xxl-4 col-lg-4">
 <cc1:DropDownListChosen ID="ddlfilter" CssClass="form-select" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Dependency ..." AllowSingleDeselect="true" AutoPostBack="false"  onchange="bindcustomdata();">
                                                        <asp:ListItem Value="All">All</asp:ListItem>
                                                        <asp:ListItem Value="milestone">Milestone</asp:ListItem>
                                                        <asp:ListItem Value="task">Task</asp:ListItem>
                                                        <asp:ListItem Value="subtask">Sub Task</asp:ListItem>
                                                                           
     </cc1:DropDownListChosen>
                          </div>
                           <div class="col-xxl-4 col-lg-4">
 <cc1:DropDownListChosen ID="ddldstatus" CssClass="form-select" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Select Status ..." AllowSingleDeselect="true" AutoPostBack="false"  onchange="bindcustomdata();">
                          <asp:ListItem Value="1">Done</asp:ListItem>
                                                        <asp:ListItem Value="2">Pending</asp:ListItem>
                                                                           
     </cc1:DropDownListChosen>
                          </div>
                          </div>
                       </div>

                        
                                                                
                            
                </div>
                            <div class="table-responsive">
                                
                             <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                                     

                        </div>
                        </div>
            </div>
      </div>
    </div>
    
<asp:HiddenField ID="hdnbtns" runat="server" />
    <script>
        
        $(document).ready(function () {
            Bindtask_data();
           
   });

                   
       
        function bindcustomdata() {
     
             //alert("ok");
            
                  $.ajax({
                    url: 'AddSTDependency.aspx/bindtaskdatacustom',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'RecordType': '" + document.getElementById('<%=ddlfilter.ClientID%>').value + "','status': '" + document.getElementById('<%=ddldstatus.ClientID%>').value + "','strid': '" + document.getElementById('<%=hdndependid.ClientID%>').value + "'}",
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
        //fixedColumns: {
        //    leftColumns: 4
        //}
        function OnSuccessgetdetailsmain(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
                    lbl.innerHTML = '';                    
            lbl.innerHTML = todolist;
      
             
     




        }

    function addnewdependency()
    {
  document.getElementById("dvdetails").style.display='none';
  document.getElementById("btnaddnew").style.display='none';
  document.getElementById("dvnew").style.display='';
  bindddldatadefault();
    }
function discarddata()
    {
debugger
    document.getElementById('<%=hdnid.ClientID%>').value="";
    document.getElementById('<%=hdntaskid.ClientID%>').value="";
    document.getElementById('<%=hdnsubtaskid.ClientID%>').value="";
    document.getElementById('<%=hdndmid.ClientID%>').value="";
 <%--   document.getElementById('<%=btncancel.ClientID%>').click(); document.getElementById('<%=hdnmid.ClientID%>').value--%>
    document.getElementById("dvnew").style.display='none';
    document.getElementById("btnaddnew").style.display = '';
     document.getElementById("dvdetails").style.display='';
    //bindtask('ContentPlaceHolder1_ddldmid'); 
      bindddldata();  
   
        }
   function bindmilestone(strtype)
    {
debugger
     var pid = '<%=HttpContext.Current.Request.QueryString["mprojectid"].ToString()%>';
       var mid = document.getElementById("<%=ddlMilestone.ClientID %>").value;
       
       if (strtype == "all")
       {
           mid = "0";
       }
       
  
 $.ajax({
            url: '<%=ResolveUrl("AddSTDependency.aspx/bindmilestonedata") %>',
            //data: "{'groupid':'"+document.getElementById(ctrlid).value+"'}",
            data: "{'mprojectid':'"+pid+"','mid':'"+mid+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                 //alert("ok");
debugger


                    $('#<%=ddldmid.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%=ddldmid.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
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
       document.getElementById('<%=hdndmid.ClientID%>').value = "";
       document.getElementById('<%=hdntaskid.ClientID%>').value = "";
       document.getElementById('<%=hdnsubtaskid.ClientID%>').value = "";
       try {
                    debugger
           var idsemp = [];
           if (document.getElementById('<%=hdndmid.ClientID%>').value != "") {
               idsemp = document.getElementById('<%=hdndmid.ClientID%>').value.split(",");
           }
                    $(document.getElementById("<%=ddldmid.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=ddldmid.ClientID%>")).change();
                }
                catch
                {}
                try {
                    debugger
                    var idsemp2 = [];
                     if (document.getElementById('<%=hdntaskid.ClientID%>').value != "") {
               idsemp2 = document.getElementById('<%=hdntaskid.ClientID%>').value.split(",");
           }
                    $(document.getElementById("<%=taskid.ClientID%>")).val(idsemp2);
                     $(document.getElementById("<%=taskid.ClientID%>")).change();
                }
                catch
                {}
            try {
                    debugger
                var idsemp3 = [];
                if (document.getElementById('<%=hdnsubtaskid.ClientID%>').value != "") {
               idsemp2 = document.getElementById('<%=hdnsubtaskid.ClientID%>').value.split(",");
           }
                    $(document.getElementById("<%=subtaskid.ClientID%>")).val(idsemp3);
                     $(document.getElementById("<%=subtaskid.ClientID%>")).change();
                }
                catch
                {}
    }
    function bindtask(ctrlid)
    {
debugger
     
        var pid = '<%=HttpContext.Current.Request.QueryString["mprojectid"].ToString()%>';
        var mid =document.getElementById("<%=hdndmid.ClientID %>").value;
        var tid = document.getElementById("<%=ddltask1.ClientID %>").value;
        if (document.getElementById("<%=ddlsubtask1.ClientID %>").value != "" && document.getElementById("<%=ddlsubtask1.ClientID %>").value != "0") {
            tid = "0";
        }
       // if (mid == "" || mid == "0") {
           // return false;
        //}
 $.ajax({
            url: '<%=ResolveUrl("AddSTDependency.aspx/bindtasks") %>',
            //data: "{'groupid':'"+document.getElementById(ctrlid).value+"'}",
            data: "{'mprojectid':'"+pid+"','mid':'"+mid+"','tid':'"+tid+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                 //alert("ok");
debugger
var tasks = response.d;
if(tasks.length>0)
{
document.getElementById("divtask").style.display='';
}
else
{
document.getElementById("divtask").style.display='none';
}

                    $('#<%=taskid.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%= taskid.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
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
  function bindsubtask(ctrlid)
    {
debugger
         var pid = '<%=HttpContext.Current.Request.QueryString["mprojectid"].ToString()%>';

 $.ajax({
            url: '<%=ResolveUrl("AddSTDependency.aspx/bindsubtasks") %>',
            //data: "{'groupid':'"+document.getElementById(ctrlid).value+"'}",
            data: "{'mprojectid':'"+pid+"','tid':'"+document.getElementById("<%=hdntaskid.ClientID %>").value+"','stid':'"+document.getElementById("<%=ddlsubtask1.ClientID %>").value+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                 //alert("ok");
var subtasks = response.d;
if(subtasks.length>0)
{
document.getElementById("divsubtask").style.display='';
}
else
{
document.getElementById("divsubtask").style.display='none';
}
                    $('#<%=subtaskid.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%=subtaskid.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
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

         function Bindtask_data() {
     
             //alert("ok");
            debugger
                  $.ajax({
                    url: 'AddSTDependency.aspx/bindtaskdata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'RecordId': '" + document.getElementById('<%=hdndependid.ClientID%>').value + "','RecordType': '" + document.getElementById('<%=hdndependtype.ClientID%>').value + "'}",
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
        //fixedColumns: {
        //    leftColumns: 4
        //}
        function OnSuccessgetdetailsmain(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
                    lbl.innerHTML = '';                    
            lbl.innerHTML = todolist;
      
                //table = new DataTable('.datatable', {
                   // scrollX: true,
                   // scrollY: 300,
                    //scrollCollapse: true,
                    //paging: false,
                    //searching: true,
                     
      

                //});
     




        }
      

    </script>
    <script>
 function SvaeData()
                    {
debugger
                        var mdetails = {};
                        mdetails.id = document.getElementById('<%=hdnid.ClientID%>').value;   
                        mdetails.mprojectid = '<%=HttpContext.Current.Request.QueryString["mprojectid"].ToString()%>';   
                        mdetails.did = document.getElementById('<%=hdndependid.ClientID%>').value;   
                        mdetails.dtype = document.getElementById('<%=hdndependtype.ClientID%>').value;   
                        mdetails.mid =document.getElementById('<%=ddlMilestone.ClientID%>').value;
                        mdetails.tid =document.getElementById('<%=ddltask1.ClientID%>').value;
                        mdetails.stid =document.getElementById('<%=ddlsubtask1.ClientID%>').value;
                        mdetails.dmid = document.getElementById('<%=hdndmid.ClientID%>').value;
                        mdetails.dtaskid = document.getElementById('<%=hdntaskid.ClientID%>').value;
                        mdetails.dsubtaskid = document.getElementById('<%=hdnsubtaskid.ClientID%>').value;
                        document.getElementById("divsubDM").style.display = "none";
                        document.getElementById("pnlimgDM").style.display = "";
                        
                        $.ajax({
                            type: "POST",
                            url: "/pms/AddSTDependency.aspx/SaveeDependency",
                            data: '{mdetails: ' + JSON.stringify(mdetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success:OnSuccess,
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


                    }
                

                function OnSuccess(response) {
                    debugger
                    if (response.d != "0") {
                         document.getElementById("pnlimgDM").style.display = "none";
                            document.getElementById("divsubDM").style.display = "block";
                     
                    alert("Data Saved successfully."); 
 Bindtask_data();
                   cleardata();
                  
                   }
}
                   
                    
                
function cleardata()
{
                  
                    document.getElementById('<%=hdnid.ClientID%>').value = "";
                    document.getElementById('<%=hdndmid.ClientID%>').value="";
                    document.getElementById('<%=hdntaskid.ClientID%>').value="";
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value="";
                    document.getElementById("pnlimgDM").style.display = "none";
                    document.getElementById("divsubDM").style.display = "block";
                    discarddata();
                    
        }

function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: '/pms/AddSTDependency.aspx/Binddetail',
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
                var Itemdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Itemdetails.find("id").text() != "") {
                     document.getElementById("dvdetails").style.display='none';
                    document.getElementById("btnaddnew").style.display='none';
                    document.getElementById("dvnew").style.display='';
                    document.getElementById('<%=hdnid.ClientID%>').value = Itemdetails.find("id").text();


                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Itemdetails.find("milestoneid").text();
                    document.getElementById('<%=hdntask1.ClientID%>').value = Itemdetails.find("taskid").text();
                    document.getElementById('<%=hdnsubtask1.ClientID%>').value = Itemdetails.find("subtaskid").text();
                    document.getElementById('<%=hdndmid.ClientID%>').value = Itemdetails.find("dmilestoneid").text();
                    //bindtask('ContentPlaceHolder1_ddldmid');
                    document.getElementById('<%=hdntaskid.ClientID%>').value = Itemdetails.find("dtaskid").text();
                   // bindsubtask('ContentPlaceHolder1_taskid');
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value = Itemdetails.find("dsubtaskid").text();
                   }
                  bindddldata();
            }
            catch
                {
                }
             <%--document.getElementById('<%=Button1.ClientID%>').click();--%>
            bindddldata();
            document.getElementById('<%=btnedit.ClientID%>').click();
            }
       function bindexist()
        {
document.getElementById('<%=btnexist.ClientID%>').click();
        }
 function BinddetailsExist(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: '/pms/AddSTDependency.aspx/Binddetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExist,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessExist(response) {
            
            debugger
            try {
                var Itemdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Itemdetails.find("id").text() != "") {


                    document.getElementById('<%=hdndmid.ClientID%>').value = Itemdetails.find("dmilestoneid").text();
                    document.getElementById('<%=hdntaskid.ClientID%>').value = Itemdetails.find("dtaskid").text();
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value = Itemdetails.find("dsubtaskid").text();
                }
                else {
                    document.getElementById('<%=hdndmid.ClientID%>').value = "";
                    document.getElementById('<%=hdntaskid.ClientID%>').value = "";
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value = "";
           
                }
                  
            }
            catch
                {
                }
             <%--document.getElementById('<%=Button1.ClientID%>').click();--%>
            bindddldata();
           
            }
  function bindddldata()
            {try {
                    debugger
                    var idsemp = document.getElementById("<%=hdndmid.ClientID%>").value.split(",");
                    $(document.getElementById("<%=ddldmid.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=ddldmid.ClientID%>")).change();
                }
                catch
                {}
                try {
                    debugger
                    var idsemp2 = [];
                    <%--document.getElementById('<%=hdntaskid.ClientID%>').value.split(",");--%>
                    if (document.getElementById('<%=hdntaskid.ClientID%>').value != "") {
                        idsemp2=document.getElementById('<%=hdntaskid.ClientID%>').value.split(",");
                    }
                    $(document.getElementById("<%=taskid.ClientID%>")).val(idsemp2);
                     $(document.getElementById("<%=taskid.ClientID%>")).change();
                }
                catch
                {}
            try {
                    debugger
                var idsemp3 = []; 
                if (document.getElementById('<%=hdnsubtaskid.ClientID%>').value != "") {
                        idsemp3=document.getElementById('<%=hdnsubtaskid.ClientID%>').value.split(",");
                    }
                $(document.getElementById("<%=subtaskid.ClientID%>")).val(idsemp3);
                     $(document.getElementById("<%=subtaskid.ClientID%>")).change();
                }
                catch
                {}
}
function bindddldatadefault()
{
    <%--try {
                    debugger
                    var idsemp = document.getElementById("<%=hdnmid.ClientID%>").value.split(',');
                    $(document.getElementById("<%=ddldmid.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=ddldmid.ClientID%>")).change();
                }
                catch
                {}--%>
               
}
        function deletedatamile(RecordId) {

        debugger    
                 if (confirm('Do you really want to delete the Entery?'))
                 {
                     //toggleHiddenFieldValue(strpid,RecordId, '','delete') ;
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                   
                    url: "/pms/AddSTDependency.aspx/DeleteDependency",
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response)
                    {
debugger
                    alert('Data Deleted !');
                     // const row = btn.closest('tr');
                      //row.remove();
                    Bindtask_data();

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

        function OnSuccessdeletemile(response) {
            alert('Data Deleted !');
            Bindtask_data();
        }

    </script>
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
    <script src="/pms/assets/js/select2.min.js"></script> 
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
     <link href="/pms/assets/js/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <script>
    debugger
    var ddldmid = $(document.getElementById("<%=ddldmid.ClientID%>"));
    var ddltask = $(document.getElementById("<%=taskid.ClientID%>"));
    var ddldsubtask = $(document.getElementById("<%=subtaskid.ClientID%>"));
     $(ddldmid).select2({
        data: [
    <%=MultiMilestone%>
        ],
        multiple: true,
        placeholder: "Milestone...",
        width: "100%"
    });
   
     $(document.getElementById("<%=ddldmid.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=ddldmid.ClientID%>")).val();
        document.getElementById("<%=hdndmid.ClientID %>").value = ids;
        document.getElementById("<%=ddldmid.ClientID%>").click();

    });


  $(ddltask).select2({
        data: [
    <%=MultiTask%>
        ],
        multiple: true,
        placeholder: "Task...",
        width: "100%"
    });
   
     $(document.getElementById("<%=taskid.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=taskid.ClientID%>")).val();
        document.getElementById("<%=hdntaskid.ClientID %>").value = ids;
        document.getElementById("<%=taskid.ClientID%>").click();

    });

   $(ddldsubtask).select2({
        data: [
    <%=MultiSubtask%>
        ],
        multiple: true,
        placeholder: "Task...",
        width: "100%"
    });
   
     $(document.getElementById("<%=subtaskid.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=subtaskid.ClientID%>")).val();
        document.getElementById("<%=hdnsubtaskid.ClientID %>").value = ids;
        <%--document.getElementById("<%=ddldmid.ClientID%>").click();--%>

    });
   
  bindddldatadefault();
    </script>
</asp:Content>
