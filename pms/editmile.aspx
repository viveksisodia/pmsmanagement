<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="editmile.aspx.cs" Inherits="editmile" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 68px);">
            <div class="acitivity-timeline p-3">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
    <div class="row"><div style="display: none">
                                <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />
                                     <asp:Button ID="btnload" runat="server" Text="" OnClick="btnload_Click" />
                                 </div>
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
                                <div class="col-xxl-12 col-lg-12" id="divprojectmile" style="display:none">
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
                                    <span class="input-group-text" style="display:none; cursor:pointer;" onclick="openCalendar()"></span>
                                       <asp:UpdatePanel ID="UpdatePanel36" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input type="text" id="StartDate" runat="server" class="form-control">checkcomparisionmilestonedate()--%>
                                      <input type="text" id="StartDatemile" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
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

                                      <asp:UpdatePanel ID="UpdatePanel37" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="EndDatemile" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                         <%--<input type="text" id="EndDate" runat="server" class="form-control">--%>
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
           <div style="display:none">
                               <asp:Button ID="btnuploadimg1" runat="server" Text="" OnClick="btnuploadimg1_Click" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing1" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg1" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:fileupload ID="Uploaddrawing1" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing1" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:fileupload ID="fileupload1" CssClass="form-control" runat="server" OnChange="ShowPreviewnew1();"  />
                                       <a href="#" id="imgpath1" target="_blank" style="display:none"> <img src="/pms/assets/images/user.jpg" id="empimg1" style="height:50px;width:50px"/></a>
                                    <div id="previewContainer1" style="margin-top: 10px;"></div>
                                               

                                </div>

                            </div>

                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-center"> 
         <div id="pnlimgmile" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img3" />
                    </div>
                    <div class="btn-group">
                      
                        <%--<div class="btn-group" id="divsubmile" style="display: block;">--%>
                        <div class="btn" id="divsubmile" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsavemile" onclick="SvaeDatamile('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" style="display:none" class="btn btn-secondary" id="btnsaveandmoremile" onclick="SvaeDatamile('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimgmilenew()"  aria-label="Close" <%=strdisplay_close%>>Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclosemile" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>
    <script src="/pms/assets/js/select2.min.js"></script> 
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
     <script>
    debugger
    var Ownermile1 = $(document.getElementById("<%=Ownermile.ClientID%>"));
    var teammile1 = $(document.getElementById("<%=teammile.ClientID%>"));
   
     $(Ownermile1).select2({
        data: [
    <%=Multiowner%>
        ],
        multiple: true,
        placeholder: "Milestone...",
        width: "100%"
    });
         $(teammile1).select2({
        data: [
    <%=Multiteam%>
        ],
        multiple: true,
        placeholder: "Team...",
        width: "100%"
    });
   
     $(document.getElementById("<%=Ownermile.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=Ownermile.ClientID%>")).val();
        document.getElementById("<%=hdnownermile.ClientID %>").value = ids;
        document.getElementById("<%=Ownermile.ClientID%>").click();

    });

$(document.getElementById("<%=teammile.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=teammile.ClientID%>")).val();
        document.getElementById("<%=hdnteammile.ClientID %>").value = ids;
        document.getElementById("<%=teammile.ClientID%>").click();

    });
  
   
 
    </script>
    <script>
        Binddetails_mile('<%=HttpContext.Current.Request.QueryString["id"].ToString()%>');
 function Binddetails_mile(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: '/pms/MultiProjectTask.aspx/Bindmilestonedetail',
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
try
{
if (milestonedetails.find("Uploaddrawing").text() != "" && milestonedetails.find("Uploaddrawing").text() != "0")
                    {
                         //bindUploadedDrawings1(milestonedetails.find("Uploaddrawing").text());
var strfiles=milestonedetails.find("Uploaddrawing").text();
var uploadData = strfiles; 
    var basePath = "/pms/MileImages/";
    var previewContainer1 = document.getElementById("previewContainer1");
    
    previewContainer1.innerHTML = ""; // Clear previous content

    if (uploadData && uploadData !== "0") {
        var files = uploadData.split("^");

        files.forEach(function(file) {
            file = file.trim();
            if (!file) return; // Skip empty entries

            var filePath = basePath + file;
            var fileName = file.split("/").pop();
            var ext = fileName.split('.').pop().toLowerCase();

            if (["jpg", "jpeg", "png", "gif", "bmp"].includes(ext)) {
                var imgLink = document.createElement("a");
                imgLink.href = filePath;
                imgLink.target = "_blank";
                
                var img = document.createElement("img");
                img.src = filePath;
                img.alt = fileName;
                img.title = fileName;
                img.className = "preview-item";
                img.style.maxWidth = "100px";
                img.style.margin = "5px";

                imgLink.appendChild(img);
                previewContainer1.appendChild(imgLink);
            } else if (ext === "pdf") {
                var pdfLink = document.createElement("a");
                pdfLink.href = filePath;
                pdfLink.target = "_blank";
                pdfLink.title = fileName;
                pdfLink.className = "preview-item";
                pdfLink.style.display = "inline-block";
                pdfLink.style.margin = "5px";
                pdfLink.style.textDecoration = "none";
                pdfLink.style.fontSize = "40px";
                pdfLink.textContent = "📄"; // or "📄 PDF"

                previewContainer1.appendChild(pdfLink);
            }
        });
    }
                    }
}
catch
{}
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
                    // Bindprojecttimeline_data();
                     document.getElementById("divmiletimeline_mile").style.display = "block";
                   // Bindmilestonetimeline_data_mile();

                    
                    //Getempdetailsmile();

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

              //GetTeamempdetailsmile();
                    debugger
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
                  
                      document.getElementById('<%=Button1mile.ClientID%>').click();
                    document.getElementById('btnsavemile').innerHTML = "Update";
                  
                }
            }
            catch
                {
                }

        }
        function showdivimgmilenew() {
            window.parent.document.getElementById("btnclosemile").click();

        }
function SvaeDatamile(val1Amile) {
                    debugger
              if (Page_ClientValidate("btnsavemile"))
              {
                       
                      
                        
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
                            url: '/pms/MultiProjectTask.aspx/SaveMilestone',
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
                   
UploadFiles(response.d);
 alert("Data Saved successfully."); 

                    
                        document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                        //window.parent.document.getElementById("btnreload").click();
                        window.parent.document.getElementById("hdneditid").value="1";;
                        window.parent.document.getElementById("btncloseedit").click();
                        
                    
                    
                }
    </script>
    <script>
        

      function UploadFiles(id)
      {
        debugger
        if (document.getElementById("<%=fileupload1.ClientID%>").files.length >0)
        {       
            
            let fileupload1 = document.getElementById("<%=fileupload1.ClientID%>");
            let files = fileupload1.files;
            
  
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
            document.getElementById("<%=fileupload1.ClientID%>").value = null;
         debugger
        }
function ShowPreviewnew1() {
    debugger
    var input = document.getElementById("<%= fileupload1.ClientID %>");
    var previewContainer1 = document.getElementById("previewContainer1");
    previewContainer1.innerHTML = ''; // Clear previous previews

    if (input.files && input.files.length > 0) {
        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];

            // Check file type
            var isImage = file.type.startsWith('image/');
            var isPDF = file.type === 'application/pdf';

            if (!isImage && !isPDF) {
                alert("Only image or PDF files are allowed.");
                continue;
            }

            // Size checks
            if (isImage && file.size > 25000) {
                alert("Image '" + file.name + "' exceeds 25KB.");
                continue;
            }

            if (isPDF && file.size > 1048576) {
                alert("PDF '" + file.name + "' exceeds 1MB.");
                continue;
            }

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
                        previewContainer1.appendChild(img);
                    } else if (theFile.type === 'application/pdf') {
                        // === OPTION 1: Show Emoji ===
                        var icon = document.createElement("div");
                        icon.textContent = "📄"; // PDF emoji
                        icon.style.fontSize = "40px";
                        icon.style.margin = "5px";
                        icon.title = theFile.name;
icon.className = "preview-item";
                        previewContainer1.appendChild(icon);

                        // === OPTION 2: Show Image Icon ===
                        /*
                        var img = document.createElement("img");
                        img.src = "/images/pdf-icon.png"; // Adjust path as needed
                        img.style.width = "40px";
                        img.style.margin = "5px";
                        img.title = theFile.name;
                        previewContainer1.appendChild(img);
                        */
                    }
                };
            })(file);

            reader.readAsDataURL(file);
        }
    }
}

        function ShowPreview()
        {
           debugger
            var input = document.getElementById("<%=fileupload1.ClientID%>");
            if (input.files && input.files[0]) {
                if (input.files.item(0).size < 30000) {
                    var ImageDir = new FileReader();
                    ImageDir.onload = function (e) {
                        //Jquery
                        //$('#impPrev').attr('src', e.target.result);  
                        var image = document.getElementById("empimg1");
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
function bindUploadedDrawings1(strfiles) {
debugger
    var uploadData = strfiles; 
    var basePath = "/pms/MileImages/";
    var previewContainer1 = document.getElementById("previewContainer1");
    
    previewContainer1.innerHTML = ""; // Clear previous content

    if (uploadData && uploadData !== "0") {
        var files = uploadData.split("^");

        files.forEach(function(file) {
            file = file.trim();
            if (!file) return; // Skip empty entries

            var filePath = basePath + file;
            var fileName = file.split("/").pop();
            var ext = fileName.split('.').pop().toLowerCase();

            if (["jpg", "jpeg", "png", "gif", "bmp"].includes(ext)) {
                var imgLink = document.createElement("a");
                imgLink.href = filePath;
                imgLink.target = "_blank";
                
                var img = document.createElement("img");
                img.src = filePath;
                img.alt = fileName;
                img.title = fileName;
                img.className = "preview-item";
                img.style.maxWidth = "100px";
                img.style.margin = "5px";

                imgLink.appendChild(img);
                previewContainer1.appendChild(imgLink);
            } else if (ext === "pdf") {
                var pdfLink = document.createElement("a");
                pdfLink.href = filePath;
                pdfLink.target = "_blank";
                pdfLink.title = fileName;
                pdfLink.className = "preview-item";
                pdfLink.style.display = "inline-block";
                pdfLink.style.margin = "5px";
                pdfLink.style.textDecoration = "none";
                pdfLink.style.fontSize = "40px";
                pdfLink.textContent = "📄"; // or "📄 PDF"

                previewContainer1.appendChild(pdfLink);
            }
        });
    }
}

    </script>

    <script>
function checkdatecomparisiontaskdate_mile1(enterdate) {
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
                    if (document.getElementById("<%=EndDatemile.ClientID%>").value!= "") 
                    {
                      checkdatecomparisiontaskdate_mile2('ContentPlaceHolder1_EndDatemile');
                    }
                    else
                    {
                     return true
                      }
               }
        }
            }
function checkdatecomparisiontaskdate_mile2(enterdate)
            {
                debugger
                 var fcompenddate = new Date();
                if (document.getElementById("<%=StartDatemile.ClientID%>").value == "") {
                    alert("Please select start date!!");
                    document.getElementById(enterdate).value = "";
                    document.getElementById("<%=StartDatemile.ClientID%>").focus();
                    return false;
                }
                else
                {
             var strenddt;
            if (document.getElementById("<%=StartDatemile.ClientID%>").value.includes("/")) {
                strenddt = document.getElementById("<%=StartDatemile.ClientID%>").value.split("/");
            }
           else if (document.getElementById("<%=StartDatemile.ClientID%>").value.includes("-")) {
                strenddt = document.getElementById("<%=StartDatemile.ClientID%>").value.split("-");
            }
           
            //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
            if (document.getElementById("<%=StartDatemile.ClientID%>").value.includes("-")) {
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
        if (document.getElementById("<%=StartDatemile.ClientID%>").value != "" && document.getElementById("<%=EndDatemile.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=StartDatemile.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=EndDatemile.ClientID%>").value);
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

                var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;



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
</script>
</asp:Content>

