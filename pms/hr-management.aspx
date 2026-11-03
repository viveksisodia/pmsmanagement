<%@ Page Title="" Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="hr-management.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<script type="text/javascript" language="javascript">
window.onload = function() {
  document.getElementById('myclass').className = 'dashboard-topbar-wrapper';
};
      </script> --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content">
        <div class="container-fluid">
            <div class="row"> 
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-header pt-2 pb-2">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="pt-2">Manage Employees</h5>
                                </div> 
                                <div class="col-sm">
                                    <div class="d-flex justify-content-sm-end hstack gap-2 ">
                                        <a href="javascript:void(0);"  class="btn btn-primary" data-bs-toggle="offcanvas" data-bs-target="#masterlist">+ New Employee</a>
                                    </div>
                                </div>                                
                            </div> 
                        </div><!-- end card header -->
                    </div>
                </div>

                <div class="col-lg-12">                    
                    <div class="card">
                        <div class="card-body">                            
                            <div class="table-responsive">
                              
                                <asp:Label ID="lblempdata" runat="server"></asp:Label>

                             <div style="display:none;">

                                 <asp:Button ID="btndelete" runat="server" Text="delete"  />
                             </div>
                            </div>                          
                        </div>
                    </div>
                </div>
            </div>           
        </div>
    </div>


    <div class="offcanvas offcanvas-end" id="masterlist" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
            <label id="lbltitle" runat="server">Add New Employee</label>
        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
             <a  href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="showdivimg()"  aria-label="Close"><i class="ph-x-bold"></i></a>
          
           <div style="display:none"> <button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button></div>
       
            </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form">
                            <div class="lead_form_head">
                            <h4>Employee Information</h4>
                            </div>
                            <div class="lead_form_body">
                                <div class="row">
                                                                  <div style="display: none">
    <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />
   
</div>
                                      <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnbtnstatus" runat="server" Value="save" />
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Department</label>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <Triggers>

                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />


                                            </Triggers>
                                            <ContentTemplate>
                                        <cc1:DropDownListChosen ID="ddldepartment" CssClass="form-control" runat="server" Width="250px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Department..." AllowSingleDeselect="true"  onchange="AddNewdepartment()">                                                        
                                                    </cc1:DropDownListChosen>
                                         <div id="dvnewdepartment" style="display:none">
                                                        <asp:HiddenField ID="hdndepartment" runat="server" Value="0" />
                                                        <input type="text" id="txtnewdepatment" class="form-control" placeholder="Department Name"  runat="server" />
                                                    
                                            
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                         </div>
                                    
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Designation</label>
                                        </div>

                                         <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <Triggers>

                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />


                                            </Triggers>
                                            <ContentTemplate>

                                         <cc1:DropDownListChosen ID="ddljobtitle" CssClass="form-control" runat="server" Width="250px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Designation..." AllowSingleDeselect="true" onchange="AddNewdesignation()">                                                        
                                                    </cc1:DropDownListChosen>
                                         <div id="dvnewDesignation" style="display:none">
                                                        <asp:HiddenField ID="hdndesignation" runat="server" Value="0" />
                                                        <input type="text" id="txtnewdesignation" class="form-control" placeholder="Designation Name"  runat="server" />
                                                 
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
             
                                         </div>
                                    
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Name  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="txtname" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                        </div>
                                          <input type="text" id="txtname" class="form-control" placeholder="Name" runat="server" >
                                    </div>
                                   
                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Report To</label>
                                        </div>   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <Triggers>

                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />


                                            </Triggers>
                                            <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ddlreportto" CssClass="form-control" runat="server" Width="250px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Report To..." AllowSingleDeselect="true">                                                        
                                                    </cc1:DropDownListChosen>
                                                 <asp:HiddenField ID="hdnreportto" runat="server" Value="0" />
                                                 </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                       <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Email id    &nbsp;&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  SetFocusOnError="true"  ControlToValidate="EmailId" ValidationGroup="btnsave"
                                                                ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                                Display="Dynamic" ErrorMessage="* Invalid email address" /></label>
                                        </div>
                                         <input type="text" id="EmailId"  class="form-control" placeholder="Email id" runat="server" />
                                       
                                    </div>
                                     <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Mobile Number </label>
                                        </div>
                                         <input type="text" id="txtmobile"  class="form-control" placeholder="Mobile Number"  runat="server" />
                                    </div>
                                     <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Roles & Responsibility</label>
                                        </div>
                                           <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                        <cc1:DropDownListChosen ID="ddlrolandresponsiblity" CssClass="form-control" runat="server" Width="250px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Roles & Responsibility..." AllowSingleDeselect="true">                                                        
                                                    </cc1:DropDownListChosen>

                                         <asp:HiddenField ID="hdnrolandresponsiblity" runat="server" />
                                                   </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                      <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Login-ID  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="txtloginid" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*" /></label>
                                        </div>
                                         <input type="text" id="txtloginid"  class="form-control" placeholder="Login-ID"  runat="server" />
                                    </div>
                                      <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Password  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="txtpassword" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*" /></label>
                                        </div>
                                         <input type="text" id="txtpassword"  class="form-control" placeholder="Password"  runat="server" />
                                    </div>


                             

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
                        <img src="/images/loader.gif" runat="server" id="img1" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                            
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>
</div>
<script>
    function fsFunction(){
        var element = document.getElementById("masterlist");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunction(){
        var element = document.getElementById("masterlist");
        element.classList.remove("canvas-fullscreen");
    }
</script>
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

         function AddNewdepartment() {
                debugger
                document.getElementById('<%=hdndepartment.ClientID%>').value = "0";
                 document.getElementById("dvnewdepartment").style.display = "none";
                if (document.getElementById('<%=ddldepartment.ClientID%>').value == "-1") {
                    document.getElementById('<%=hdndepartment.ClientID%>').value = "1";
                    document.getElementById("dvnewdepartment").style.display = "block";
                }
                else {
                    document.getElementById('<%=txtnewdepatment.ClientID%>').value = "";
                }
            }
         
         function AddNewdesignation() {
                debugger
                document.getElementById('<%=hdndesignation.ClientID%>').value = "0";
                 document.getElementById("dvnewDesignation").style.display = "none";
                if (document.getElementById('<%=ddljobtitle.ClientID%>').value == "-1") {
                    document.getElementById('<%=hdndesignation.ClientID%>').value = "1";
                    document.getElementById("dvnewDesignation").style.display = "block";
                }
                else {
                    document.getElementById('<%=txtnewdesignation.ClientID%>').value = "";
                }
         }




          function SvaeData(val1A) {
                    debugger
                    if (Page_ClientValidate("btnsave")) {
                        
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimg").style.display = "block";
                            document.getElementById("divsub").style.display = "none";

                        }
                        else {
                            return false;
                        }
                        document.getElementById('<%=hdnbtnstatus.ClientID%>').value = val1A;
                        var designationid = "0";
                        if (document.getElementById('<%=hdndesignation.ClientID%>').value != "" && document.getElementById('<%=hdndesignation.ClientID%>').value != "0" && document.getElementById('<%=txtnewdesignation.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "hr-management.aspx/Adddesignation",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewdesignation.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        designationid = response.d;
                                    

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
                        else {
                            designationid = document.getElementById('<%=ddljobtitle.ClientID%>').value;
                        }
                        ////department

                        var departmentid = "0";
                        if (document.getElementById('<%=hdndepartment.ClientID%>').value != "" && document.getElementById('<%=hdndepartment.ClientID%>').value != "0" && document.getElementById('<%=txtnewdepatment.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "hr-management.aspx/Adddepartment",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewdepatment.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        departmentid = response.d;
                                    

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
                          
                        });
                            

                        }
                        else {
                            departmentid = document.getElementById('<%=ddldepartment.ClientID%>').value;
                        }

                  
                        var empdata = {};
                        empdata.id = document.getElementById('<%=hdnid.ClientID%>').value; ;  
                        empdata.Department =departmentid; 
                        empdata.Location ="1"; 
                        empdata.JobTitle = designationid; 
                        empdata.DisplayName = document.getElementById('<%=txtname.ClientID%>').value; 
                        empdata.ReportingManager = document.getElementById('<%=ddlreportto.ClientID%>').value; 
                        empdata.Email = document.getElementById('<%=EmailId.ClientID%>').value; 
                        empdata.MobileNumber = document.getElementById('<%=txtmobile.ClientID%>').value;
                        empdata.roleid = document.getElementById('<%=ddlrolandresponsiblity.ClientID%>').value;
                        empdata.emp_name = document.getElementById('<%=txtloginid.ClientID%>').value;
                        empdata.Password =document.getElementById('<%=txtpassword.ClientID%>').value;
                        $.ajax({
                            type: "POST",
                            url: "hr-management.aspx/Saveemployee",
                            data: '{empdetails: ' + JSON.stringify(empdata) + '}',
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
                    $("[id*=txtnewdepatment]").val("");
                    $("[id*=txtnewdesignation]").val("");
                    $("[id*=txtname]").val("");
                    $("[id*=EmailId]").val("");
                    $("[id*=txtmobile]").val("");
                    $("[id*=txtloginid]").val("");
                   $("[id*=txtloginid]").val("");
                    $("[id*=txtpassword]").val("");
                   
                    $("[id*=ddldepartment]").val("0");
                    $("[id*=ddljobtitle]").val("0");
                    $("[id*=hdndepartment]").val("0");
                    $("[id*=hdndesignation]").val("0");
                    $("[id*=ddlreportto]").val("0");
                    $("[id*=hdnrolandresponsiblity]").val("0");
                    document.getElementById('<%=hdnid.ClientID%>').value = "0";
                    document.getElementById("pnlimg").style.display = "none";
                    document.getElementById("divsub").style.display = "block";
                    document.getElementById("btncls").click();

                       window.location.href = '/hr-management.aspx';
                    bindemployee_data();

                    <%-- document.getElementById('btnsave').value = "Save";
                    if (document.getElementById('<%=hdnbtnstatus.ClientID%>').value == "save") {
                            window.location.href = '/hr-management.aspx';
                            //window.open('/crm/lead.aspx');
                            return false;
                        }
                        else if (document.getElementById('<%=hdnbtnstatus.ClientID%>').value == "view") {
                            // window.location.reload();
                            window.location.href = '/hr-management.aspx';
                        }
                        else if (document.getElementById('<%=hdnbtnstatus.ClientID%>').value == "new") {
                        //fs8Function();
                        window.location.href = '/hr-management.aspx';

                    }--%>
                }



          function Cleardata() {
                        
              
             $("[id*=txtnewdepatment]").val("");
                    $("[id*=txtnewdesignation]").val("");
                    $("[id*=txtname]").val("");
                    $("[id*=EmailId]").val("");
                    $("[id*=txtmobile]").val("");
                    $("[id*=txtloginid]").val("");
                   $("[id*=txtloginid]").val("");
                    $("[id*=txtpassword]").val("");                   
                    $("[id*=ddldepartment]").val("0");
                    $("[id*=ddljobtitle]").val("0");
                    $("[id*=hdndepartment]").val("0");
                    $("[id*=hdndesignation]").val("0");
                    $("[id*=hdnreportto]").val("0");
              $("[id*=hdnrolandresponsiblity]").val("0");
                  document.getElementById('<%=hdnid.ClientID%>').value = "0";
              document.getElementById('<%=Button1.ClientID%>').click();
                 document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add New Employee";
                
                    //$("[id*=hdndepartment]").val("0");
                    //$("[id*=hdndesignation]").val("0");
                   
                    //  document.getElementById('btnsave').value = "Save";
                       
                        
                    }

         function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'hr-management.aspx/Bindempdetailsdetail',
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
                    document.getElementById('<%=hdnid.ClientID%>').value = Itemdetails.find("id").text();

                     
                    $("[id*=txtnewdepatment]").val("");
                    $("[id*=txtnewdesignation]").val("");
                    $("[id*=txtname]").val(Itemdetails.find("DisplayName").text());
                    $("[id*=EmailId]").val(Itemdetails.find("Email").text());
                    $("[id*=txtmobile]").val(Itemdetails.find("MobileNumber").text());
                    $("[id*=txtloginid]").val(Itemdetails.find("Department").text());
                    $("[id*=txtloginid]").val(Itemdetails.find("emp_name").text());
                    $("[id*=txtpassword]").val(Itemdetails.find("Password").text()); 
                       
                    document.getElementById('<%=hdndepartment.ClientID%>').value = Itemdetails.find("Department").text();
                    document.getElementById('<%=hdndesignation.ClientID%>').value = Itemdetails.find("JobTitle").text();
                    document.getElementById('<%=hdnreportto.ClientID%>').value = Itemdetails.find("ReportingManager").text();
                     document.getElementById('<%=hdnrolandresponsiblity.ClientID%>').value = Itemdetails.find("roleid").text();
                    
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                    document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit Employee";
                }
            }
            catch
                {
                }

            }


            </script>

    <script>
        $( document ).ready(function() {
            bindemployee_data();
});
         function bindemployee_data() {
            
                debugger;
                  $.ajax({
                    url: 'hr-management.aspx/bindemployee',                  
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
                    let lbl = document.getElementById("<%=lblempdata.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
            table = new DataTable('.datatable', {  
        scrollX:  false,
        scrollCollapse: true,
        paging:         true,
        searching: true
        
    });
           
        }

    </script>


   <%-- <script>
        function deletedata(val) {
            document.getElementById('<%=hdnid.ClientID%>').value = val;
             document.getElementById('<%=btndelete.ClientID%>').click();
        }
    </script>--%>

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
                    url: 'hr-management.aspx/Deleteempdata',
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
            bindemployee_data();
        }

    </script>


 <%--<script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
<script>
    const table = new DataTable('.datatable', {  
        scrollX:  false,
        scrollCollapse: true,
        paging:         true,
        searching: true
        
    });
</script>
</asp:Content>

