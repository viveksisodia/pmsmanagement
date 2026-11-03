<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="AddPDependency.aspx.cs" Inherits="AddPDependency" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
   
    
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
            <a href="javascript:void(0);" id="btnaddnew" class="btn btn-outline-primary btn-icon" onclick="addnewdependency()"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
            <button type="button" class="btn btn-danger btn-icon" onclick="closepopupmain()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
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


                                
                        

                                <div class="col-xxl-6 col-lg-6" id="divmilestone" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>

                                    
                                       <cc1:DropDownListChosen ID="ddlMilestone" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true" disabled >
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
                                          



                                </div>
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
                        <div class="col-xxl-6 col-lg-6">
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
                                                     onchange="bindtask(this.id)" >
                                                </asp:DropDownList>
                                             <asp:HiddenField ID="hdndmid" runat="server" Value="0" />
                                           <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>

                                </div> 

                               <div class="col-xxl-6 col-lg-6" id="divtask" style="display:none">
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


                                <div class="col-xxl-6 col-lg-6" id="divsubtask" style="display:none">
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
                                                    DataPlaceHolder="Sub Task ..." AllowSingleDeselect="true">
                                                </asp:DropDownList>
                                     <asp:HiddenField ID="hdnsubtaskid" runat="server" Value="0" />

                                </div>

                                
                         </div>   
                         </div>
                

                        <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-center"> 
         <div id="pnlimgDM" style="display: none;">
                        <img src="/pms/Image/loading8.gif" id="ContentPlaceHolder1_Issues_img2">
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsubDM" style="display: block;">
                            <a href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData();" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-danger " id="btncncl" onclick="discarddata();" aria-label="Save">Cancel</a>
                            
                        </div>
                    </div>
      

    </div>
                                                               
                    </div>        
                
                 <div class="card-body">
                     <div class="head_sec mb-2">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="mb-0"><%=fns.getltext("Dependency Details", "2")%></h5>
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

                   

    function addnewdependency()
    {
  document.getElementById("btnaddnew").style.display='none';
  document.getElementById("dvnew").style.display='';
//bindddldatadefault();
    }
function discarddata()
    {
debugger
    document.getElementById('<%=hdnid.ClientID%>').value="";
    document.getElementById('<%=hdntaskid.ClientID%>').value="";
    document.getElementById('<%=hdnsubtaskid.ClientID%>').value="";
    document.getElementById('<%=hdndmid.ClientID%>').value=document.getElementById('<%=hdnmid.ClientID%>').value;
 <%--   document.getElementById('<%=btncancel.ClientID%>').click();--%>
    document.getElementById("dvnew").style.display='none';
    document.getElementById("btnaddnew").style.display='';
    //bindtask('ContentPlaceHolder1_ddldmid'); 
      bindddldata();  
   
    }
    function bindtask(ctrlid)
    {
debugger
     
     <%--document.getElementById('<%=hdndmid.ClientID%>').value=document.getElementById(ctrlid).value;
     document.getElementById('<%=btntask.ClientID%>').click();--%>

 $.ajax({
            url: '<%=ResolveUrl("AddPDependency.aspx/bindtasks") %>',
            //data: "{'groupid':'"+document.getElementById(ctrlid).value+"'}",
            data: "{'mid':'"+document.getElementById("<%=hdndmid.ClientID %>").value+"'}",
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
     <%--document.getElementById('<%=hdntaskid.ClientID%>').value=document.getElementById(ctrlid).value;
     document.getElementById('<%=btnsubtask.ClientID%>').click();--%>

 $.ajax({
            url: '<%=ResolveUrl("AddPDependency.aspx/bindsubtasks") %>',
            //data: "{'groupid':'"+document.getElementById(ctrlid).value+"'}",
            data: "{'tid':'"+document.getElementById("<%=hdntaskid.ClientID %>").value+"'}",
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
                    url: 'AddPDependency.aspx/bindtaskdata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'RecordId': '" + document.getElementById('<%=hdnpid.ClientID%>').value + "'}",
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
                        mdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; ;  
                        mdetails.pid = document.getElementById('<%=hdnpid.ClientID%>').value; ;  
                        
                        mdetails.dmid = document.getElementById('<%=hdndmid.ClientID%>').value;
                        mdetails.dtaskid = document.getElementById('<%=hdntaskid.ClientID%>').value;
                        mdetails.dsubtaskid = document.getElementById('<%=hdnsubtaskid.ClientID%>').value;
                        document.getElementById("divsubDM").style.display = "none";
                        document.getElementById("pnlimgDM").style.display = "";
                        
                        $.ajax({
                            type: "POST",
                            url: "/pms/AddPDependency.aspx/SaveeDependency",
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
                    url: '/pms/AddPDependency.aspx/Binddetail',
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
                    addnewdependency();
                    document.getElementById('<%=hdnid.ClientID%>').value = Itemdetails.find("id").text();


                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Itemdetails.find("milestoneid").text();
                    document.getElementById('<%=hdndmid.ClientID%>').value = Itemdetails.find("dmilestoneid").text();
                    //bindtask('ContentPlaceHolder1_ddldmid');
                    document.getElementById('<%=hdntaskid.ClientID%>').value = Itemdetails.find("taskid").text();
                   // bindsubtask('ContentPlaceHolder1_taskid');
                    document.getElementById('<%=hdnsubtaskid.ClientID%>').value = Itemdetails.find("subtaskid").text();
                   }
bindddldata();
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
                    var idsemp = document.getElementById("<%=hdndmid.ClientID%>").value.split(',');
                    $(document.getElementById("<%=ddldmid.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=ddldmid.ClientID%>")).change();
                }
                catch
                {}
                try {
                    debugger
                    var idsemp2 = document.getElementById("<%=hdntaskid.ClientID%>").value.split(',');
                    $(document.getElementById("<%=taskid.ClientID%>")).val(idsemp2);
                     $(document.getElementById("<%=taskid.ClientID%>")).change();
                }
                catch
                {}
            try {
                    debugger
                    var idsemp3 = document.getElementById("<%=hdnsubtaskid.ClientID%>").value.split(',');
                    $(document.getElementById("<%=subtaskid.ClientID%>")).val(idsemp3);
                     $(document.getElementById("<%=subtaskid.ClientID%>")).change();
                }
                catch
                {}
}
function bindddldatadefault()
            {try {
                    debugger
                    var idsemp = document.getElementById("<%=hdnmid.ClientID%>").value.split(',');
                    $(document.getElementById("<%=ddldmid.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=ddldmid.ClientID%>")).change();
                }
                catch
                {}
               
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
                   
                    url: "/pms/AddPDependency.aspx/DeleteDependency",
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
