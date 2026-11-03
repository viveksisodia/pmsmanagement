<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Editmilestone.aspx.cs" Inherits="Editmilestone" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link rel="shortcut icon" href="/pms/assets/images/favicon.png">
    <link rel="stylesheet" href="/pms/assets/libs/dragula/dragula.min.css">
    <link href="/pms/assets/js/jquery.dataTables.min.css" rel="stylesheet" type="text/css">  
    <link href="/pms/assets/libs/sweetalert2/sweetalert2.min.css" rel="stylesheet" type="text/css">
    <script src="/pms/assets/js/layout.js"></script>
    <link href="/pms/assets/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="/pms/assets/css/icons.min.css" rel="stylesheet" type="text/css">
    <link href="/pms/assets/css/app.min.css" rel="stylesheet" type="text/css">
    <link href="/pms/assets/css/custom.min.css" rel="stylesheet" type="text/css">
   
    <style>
        body {background:#fff;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
                function showdivimgmile() {
                    if (Page_ClientValidate("btnclosemile")) {
                        if (confirm('Do you want Leave this page ?')) {   

                            Cleardatamile();
                            
                            document.getElementById("btnclosemile").click();
                           
                        }
                        else {
                            return false;
                        }
                        //reloaddata();
                        return true;
                    }
         }


        function AddNewProjectstatusmile()
        {
                debugger
                document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatusmile").style.display = "none";
               if (document.getElementById('<%=P_Statusmile.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatusmile").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColormile.ClientID%>').value = "";
               }
        }


        function AddNewprioritymile()
        {
                debugger
                document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "0";
                 document.getElementById("dvnewprioritymile").style.display = "none";
               if (document.getElementById('<%=Prioritymile.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "1";
                    document.getElementById("dvnewprioritymile").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprioritymile.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolormile.ClientID%>').value = "";
               }
        }

         

      


          function SvaeDatamile(val1Amile) {
                    debugger
              if (Page_ClientValidate("btnsavemile"))
              {
                       
                       document.getElementById('<%=hdnsavetypemile.ClientID%>').value = val1Amile;  
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimgmile").style.display = "block";
                            document.getElementById("divsubmile").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }




                      // Project status save
                        
                        var milstonestatusidmile = "0";
                        if (document.getElementById('<%=hdnP_Statusmile.ClientID%>').value != "" && document.getElementById('<%=hdnP_Statusmile.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value != "" && document.getElementById('<%=stustprintColormile.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddAddMilestonetatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColormile.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milstonestatusidmile = response.d;
                                    

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
                            milstonestatusidmile = document.getElementById('<%=P_Statusmile.ClientID%>').value;
                        }

                        // Priority type save

                         var milePrioritytypeidmile = "0";
                        if (document.getElementById('<%=hdnPrioritymile.ClientID%>').value != "" && document.getElementById('<%=hdnPrioritymile.ClientID%>').value != "0" && document.getElementById('<%=txtnewprioritymile.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolormile.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddMilestonePrioritytype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprioritymile.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolormile.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milePrioritytypeidmile = response.d;
                                    

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
                            milePrioritytypeidmile = document.getElementById('<%=Prioritymile.ClientID%>').value;
                        }


                        // save all details

                        
                        var milestonedetails = {};
                        milestonedetails.id = document.getElementById('<%=hdnidmile.ClientID%>').value; 
                        milestonedetails.ProjectId = document.getElementById('<%=ProjectIdmile.ClientID%>').value; 
                        milestonedetails.Milestone = document.getElementById('<%=Milestone.ClientID%>').value; 
                        milestonedetails.Weightage = document.getElementById('<%=Weightagemile.ClientID%>').value; 
                        milestonedetails.StartDate = document.getElementById('<%=StartDatemile.ClientID%>').value; 
                        milestonedetails.EndDate = document.getElementById('<%=EndDatemile.ClientID%>').value; 
                       milestonedetails.Owner = document.getElementById('<%=hdnownermile.ClientID%>').value;
                       milestonedetails.Access = document.getElementById('<%=Accessmile.ClientID%>').value; 
                        milestonedetails.team = document.getElementById('<%=hdnteammile.ClientID%>').value; 
                        milestonedetails.P_Status = milstonestatusidmile;
                        milestonedetails.Priority = milePrioritytypeidmile;
                        milestonedetails.Budget = document.getElementById('<%=Budgetmile.ClientID%>').value;
                        milestonedetails.Actual = document.getElementById('<%=Actualmile.ClientID%>').value;


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
                   milestonedetails.TaskDuration = document.getElementById('<%=taskDurationmile.ClientID%>').value; 

                        $.ajax({
                            type: "POST",
                            url: 'MultiProjectTask.aspx/SaveMilestone',
                            data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: OnSuccess_mile,
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
                function OnSuccess_mile(response) {
                    debugger
                    alert("Data Saved successfully."); 

                    document.getElementById('<%=btnloadmile.ClientID%>').click();

                  

                   document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = "0";
                    document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightagemile.ClientID%>').value = "";
                    document.getElementById('<%=StartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=EndDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value ="";
                    
                      document.getElementById('<%=hdnownermile.ClientID%>').value = "";
                     $(document.getElementById("<%=Ownermile.ClientID%>")).val(0);
                   $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                     $(document.getElementById("<%=teammile.ClientID%>")).val(0);
                    $(document.getElementById("<%=teammile.ClientID%>")).change();
                    document.getElementById('<%=hdnAccessmile.ClientID%>').value = "0";
                     document.getElementById('<%=hdnteammile.ClientID%>').value = "";
                    document.getElementById('<%=P_Statusmile.ClientID%>').value = "0";
                    document.getElementById('<%=Prioritymile.ClientID%>').value = "0";
                    document.getElementById('<%=Budgetmile.ClientID%>').value = "0";
                    document.getElementById('<%=Actualmile.ClientID%>').value = "0";
                    document.getElementById('<%=istask.ClientID%>').checked = 0;
                    document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;
                    <%--alert(document.getElementById('<%=hdnsavetype.ClientID%>').value);--%>

                    document.getElementById('<%=taskDurationmile.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";

                     document.getElementById('<%=hdnidmile.ClientID%>').value = "0";
                    if (document.getElementById('<%=hdnsavetypemile.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                        document.getElementById("btnclsmile").click();
                        Bindtask_data();
                    }
                    
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                       // document.getElementById("btncls").click();
                        //Bindtask_data();
                    }
                   
                   
               
                    
                }



          function Cleardatamile() {
                        
                  
                 document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = "0";
                  document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightagemile.ClientID%>').value = "";
                    document.getElementById('<%=StartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=EndDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value ="";
                    document.getElementById('<%=hdnownermile.ClientID%>').value ="";
                   document.getElementById('<%=hdnAccessmile.ClientID%>').value = "";
                   document.getElementById('<%=hdnteammile.ClientID%>').value = "";
                   document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "0";
                   document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "0";
                   document.getElementById('<%=istask.ClientID%>').checked = 0;
                 document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;

                   document.getElementById('<%=taskDurationmile.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";


                  $(document.getElementById("<%=Ownermile.ClientID%>")).val(0);
                 $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                  $(document.getElementById("<%=teammile.ClientID%>")).val(0);
                 $(document.getElementById("<%=teammile.ClientID%>")).change();

                   document.getElementById('<%=Button1mile.ClientID%>').click();
                   document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML= "Add Milestone";
         
                        
                    }

        

          function Binddetails_mile(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindmilestonedetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1_milestonedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
              
        }


         function OnSuccess1_milestonedata(response) {
            
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnidmile.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=ProjectIdmile.ClientID%>').value = (milestonedetails.find("ProjectId").text());

                     document.getElementById('<%=Milestone.ClientID%>').value=(milestonedetails.find("Milestone").text());
                    document.getElementById('<%=Weightagemile.ClientID%>').value=(milestonedetails.find("Weightage").text());
                    document.getElementById('<%=StartDatemile.ClientID%>').value=(milestonedetails.find("StartDate").text());
                    document.getElementById('<%=EndDatemile.ClientID%>').value=(milestonedetails.find("EndDate").text());
                    document.getElementById('<%=hdnownermile.ClientID%>').value=(milestonedetails.find("Owner").text());
                    document.getElementById('<%=Accessmile.ClientID%>').value = (milestonedetails.find("Access").text()); 
                    
                    document.getElementById('<%=Budgetmile.ClientID%>').value = (milestonedetails.find("Budget").text()); 
                    document.getElementById('<%=Actualmile.ClientID%>').value = (milestonedetails.find("Actual").text()); 

                    document.getElementById('<%=P_Statusmile.ClientID%>').value = milestonedetails.find("P_Status").text();
                    document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = milestonedetails.find("P_Status").text();
                        document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColormile.ClientID%>').style.display = "none";


                    document.getElementById('<%=Prioritymile.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=hdnPrioritymile.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=txtnewprioritymile.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolormile.ClientID%>').style.display = "none";

                     document.getElementById('<%=hdnteammile.ClientID%>').value = milestonedetails.find("team").text();

                    document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
                     document.getElementById('<%=hdnStartDatemile.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value = milestonedetails.find("EndDate").text();
                    document.getElementById('<%=hdnAccessmile.ClientID%>').value = milestonedetails.find("Access").text();


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
                    document.getElementById('<%=taskDurationmile.ClientID%>').value = milestonedetails.find("TaskDuration").text();

                    
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
                     document.getElementById("divmiletimeline_mile").style.display = "block";
                    Bindmilestonetimeline_data_mile();

                  //  WeitageChecked();
                  //GetWeitagebindDtls();
                    document.getElementById('<%=Button1mile.ClientID%>').click();
                    Getempdetailsmile();

              if (milestonedetails.find("Owner").text() != "")
              {
                  debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=Ownermile.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
                $(document.getElementById("<%=Ownermile.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=Ownermile.ClientID%>")).change();
                  }
             
            }

              GetTeamempdetailsmile();
             if (milestonedetails.find("team").text() != "")
             {
                 debugger
                 if (milestonedetails.find("team").text().includes(','))
                 {
                      var idsemp = milestonedetails.find("team").text().split(',');
                     $(document.getElementById("<%=teammile.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teammile.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = milestonedetails.find("team").text();
                     $(document.getElementById("<%=teammile.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teammile.ClientID%>")).change();
                 }



            }
                  
                     
                    document.getElementById('btnsavemile').value = "Update";
                   document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML= "Edit Milestone";
                }
            }
            catch
                {
                }

            }


        
        


            </script>
       <div class="offcanvas offcanvas-end" id="addmile" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
             <label id="lbltitlemile" runat="server"><%=fns.getltext("Add Milestone", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimgmile()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btnclsmile" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
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
                                     <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                                          <ContentTemplate>
                                     <asp:Button ID="Button1mile" runat="server" Text="" OnClick="Button1mile_Click" />
                                             <asp:Button ID="btnloadmile" runat="server" Text="" OnClick="btnloadmile_Click" />
                                              </ContentTemplate>
                                         </asp:UpdatePanel>
                                     
                                 </div>

                                    <asp:HiddenField ID="hdnidmile" runat="server" Value="0" />
                                <asp:HiddenField ID="hdiprojidmile" runat="server" Value="0" />
                                <div class="col-xxl-12 col-lg-12" id="divprojectmile">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="ProjectIdmile" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>
                                 
                                    <%--onchange="GetWeitagebindDtls()"--%>
                                      <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                              
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectIdmile" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" onchange="GetWeitagebindDtlsmile()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectIdmile" runat="server" Value="0" />
                                              <asp:HiddenField ID="hdnweightagemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Milestone" ValidationGroup="btnsavemile"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Milestone" placeholder="Milestone" runat="server"  class="form-control">
                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divprojecttimeline" style="display:none;">
                                 <asp:Label ID="projecttimeline" runat="server"></asp:Label>
                                 </div>



                                 <div class="col-xxl-12 col-lg-12" id="divmiletimeline_mile" style="display:none;">
                                  <asp:Label ID="lblmiletimeline_mile" runat="server"></asp:Label>
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


                              


                               
                                <div class="col-xxl-12 col-lg-12" id="divweitagemile">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightagemile" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchkmile(this);" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                       <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input type="date" id="StartDate" runat="server" class="form-control">checkcomparisionmilestonedate()--%>
                                      <input type="date" id="StartDatemile" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate_mile(this.id)">
                                         <asp:HiddenField ID="hdnStartDatemile" runat="server" Value="0" />
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

                                      <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="EndDatemile" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate_mile(this.id)">
                                         <%--<input type="date" id="EndDate" runat="server" class="form-control">--%>
                                             <asp:HiddenField ID="hdnEndDatemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>

                                      <asp:DropDownList ID="Ownermile" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnownermile" runat="server" Value="0" /> 
  

                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="teammile" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnteammile" runat="server" Value="0" />
                                      


                                </div>


                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator10" 
                                                ControlToValidate="P_Statusmile" InitialValue="0" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadmile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Statusmile" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatusmile()">
                                                </cc1:DropDownListChosen>
                                              <div id="dvnewprojectstatusmile" style="display:none">
                                          <input type="text" id="txtnewprojectstatusmile" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColormile" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Statusmile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator11" 
                                                ControlToValidate="Prioritymile" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadmile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Prioritymile" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewprioritymile()">
                                         </cc1:DropDownListChosen>
                                                 <div id="dvnewprioritymile" style="display:none">
                                          <input type="text" id="txtnewprioritymile" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolormile" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPrioritymile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budgetmile" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actualmile" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>



                                <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Access", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Access" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Accessmile" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Staff</asp:ListItem>
                                          <asp:ListItem  value="2">Client</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnAccessmile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>    
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Duration Type", "2")%>  &nbsp;&nbsp;
                                            
                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
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
                                    <input type="text" id="taskDurationmile" runat="server" onkeypress="return isnumeric(event)"  class="form-control">
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


      <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
         <div id="pnlimgmile" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img3" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsubmile" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsavemile" onclick="SvaeDatamile('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmoremile" onclick="SvaeDatamile('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimgmile()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclosemile" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>


</div>
    </form>
</body>
</html>
