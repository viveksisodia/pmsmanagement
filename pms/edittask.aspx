<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="edittask.aspx.cs" Inherits="edittask" %>
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
    <div class="row">
                                <div style="display: none">
                                <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />
                                     <asp:Button ID="btnload" runat="server" Text="" OnClick="btnload_Click" />
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                   <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnmilstoneid" runat="server" Value="0" />

                                   <div class="col-xxl-12 col-lg-12" id="divproject" >
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectId" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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


                                <div class="col-xxl-12 col-lg-12" id="divmilestone">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Milestoneid" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true"  OnSelectedIndexChanged="Milestoneid_SelectedIndexChanged" onchange="GetWeitagebindDtls()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnweightage" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divptasklist" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task List", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="tasklist" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklist" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>



                                    <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                       <label class="form-label flex-grow-1"><%=fns.getltext("Task Name", "2")%></label>
                                    </div>
                                    <input type="text" id="taskname" runat="server" placeholder="Task Name" class="form-control">
                                </div>



                               <div class="col-xxl-12 col-lg-12" id="divmiletimeline" style="display:none;">
                              <asp:Label ID="lblmiletimeline" runat="server"></asp:Label>
                              </div>

                             <div class="col-xxl-12 col-lg-12" id="divtasktimeline" style="display:none;">
                           <asp:Label ID="lbltasktimeline" runat="server"></asp:Label>
                            </div>



                                 <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="issubtask" runat="server" />  Has Sub Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                    <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkTaskWeightage" runat="server"  />  Do You Want Save Weightage on Sub Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                               



                                
                             
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Description", "2")%></label>
                                    </div>
                                    <textarea id="taskdescripton" runat="server" class="form-control" placeholder="Task Description"></textarea>
                                </div>


                                  <div class="col-xxl-12 col-lg-12" id="divweitage">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchk(this);" class="form-control">
                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="owner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                      


                                </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>&nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Startdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server"  class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Startdate" runat="server" class="form-control"  onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">

                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                              <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;</label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server"  class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="StartTime" runat="server" class="form-control"  onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>




                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Targetdate" ValidationGroup="btnsave"
                                                                runat="server"  ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Targetdate" runat="server" class="form-control"  onclick="this.showPicker()"  onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;</label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Timeline" runat="server" class="form-control"  onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                    <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour" onkeyup="checkdurationcalculate(this);" class="form-control">
                                </div>

                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="P_Status" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
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
                                
                                       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
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


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budget" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actual" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divreminder">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp;</label>
                                    </div>
                                    
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Reminder" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetReminderDtls()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Based on Due Date</asp:ListItem>
                                          <asp:ListItem  value="2">Specific Date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnreminder" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divremindertype">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder Type", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ReminderType" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetRemindertypeDtls()">
                                         <asp:ListItem  value="1">Daily</asp:ListItem>
                                          <asp:ListItem  value="2">On due date</asp:ListItem>
                                          <asp:ListItem  value="3">Day(s) before due date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnReminderType" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="col-xxl-12 col-lg-12"  id="divDays">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Enter Day(s)", "2")%></label>
                                    </div>
                                    <input type="text" id="Days" runat="server" placeholder="Days" class="form-control">
                                     </div>


                                <div class="col-xxl-12 col-lg-12" id="divChooseDate">
                                 <div class="d-flex align-items-center">
                                 <label class="form-label flex-grow-1"><%=fns.getltext("Choose Date", "2")%></label>
                                 </div>
                                
                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="ChooseDate" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnChooseDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                    <div class="col-xxl-12 col-lg-12" id="Divtime">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Time", "2")%></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Time" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                   <div class="col-xxl-12 col-lg-12"  id="divNotifyusers">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Notify users", "2")%></label>
                                    </div>
                                
                                      <asp:DropDownList ID="Notifyusers" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Notify User ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnNotifyusers" runat="server" Value="0" />
                                     
                                    </div>
                                

                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Occurrence", "2")%></label>
                                    </div>
                                    <input type="text" id="Occurrence" runat="server" placeholder="Occurrence"  class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Tag To", "2")%></label>
                                    </div>
                                    <input type="text" id="tagto" runat="server" placeholder="Tag To" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                   <%--  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>--%>
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                       <%--    </ContentTemplate>
                                        </asp:UpdatePanel>--%>


                                </div> 
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Stage" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Stage ..." AllowSingleDeselect="true" onchange="AddNewStage()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewStage" style="display:none">
                                          <input type="text" id="txtnewstage" class="form-control" placeholder="Stage"  runat="server" />
                                                  <input  data-jscolor="{}" id="txtstageprintcolor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnstage" runat="server" Value="0" />
                                        

                                </div>
         <div style="display:none">
                               <asp:Button ID="btnuploadimg" runat="server" Text="" OnClick="btnuploadimg_Click" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:FileUpload ID="Uploaddrawing" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreviewnew();"  />
                                       <a href="#" id="imgpath" target="_blank" style="display:none"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>
                                    <div id="previewContainer" style="margin-top: 10px;"></div>
                                               

                                </div>


                            </div>

                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-center"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img3" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
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
    var Ownermile1 = $(document.getElementById("<%=owner.ClientID%>"));
    var teammile1 = $(document.getElementById("<%=team.ClientID%>"));
   
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
   
     $(document.getElementById("<%=owner.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
        document.getElementById("<%=owner.ClientID%>").click();

    });

$(document.getElementById("<%=team.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=team.ClientID%>")).val();
        document.getElementById("<%=hdnteam.ClientID %>").value = ids;
        document.getElementById("<%=team.ClientID%>").click();

    });
  
   
 var ddLNotifyusers = $(document.getElementById("<%=Notifyusers.ClientID%>"));
   

    $(ddLNotifyusers).select2({
        data: [
    <%=Multiusers%>
        ],
        multiple: true,
        placeholder: "Search Notify users",
        width: "100%"
    });

   
     $(document.getElementById("<%=Notifyusers.ClientID%>")).change(function () {
        debugger
                                                    var ids = $(document.getElementById("<%=Notifyusers.ClientID%>")).val();
        document.getElementById("<%=hdnNotifyusers.ClientID %>").value = ids;
        document.getElementById("<%=Notifyusers.ClientID%>").click();

    });
    </script>

    <script>
       

 function SvaeData(val1A) {
                    debugger
              if (Page_ClientValidate("btnsave"))
              {
                        
                        
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
                            url: "MultiProjectTask.aspx/statussave",
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
                            url: "MultiProjectTask.aspx/AddPrioritytype",
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



                      // Stage save

                         var stageid = "0";
                        if (document.getElementById('<%=hdnstage.ClientID%>').value != "" && document.getElementById('<%=hdnstage.ClientID%>').value != "0" && document.getElementById('<%=txtnewstage.ClientID%>').value != "" && document.getElementById('<%=txtstageprintcolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddStage",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewstage.ClientID%>').value) + ',Stagecolor: ' + JSON.stringify(document.getElementById('<%=txtstageprintcolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        stageid = response.d;
                                    

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
                            stageid = document.getElementById('<%=Stage.ClientID%>').value;
                        }



                        // save all details


                        
                        var taskdetails = {};
                        taskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; 
                        taskdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        taskdetails.Milestoneid = document.getElementById('<%=Milestoneid.ClientID%>').value; 
                        taskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        taskdetails.taskname = document.getElementById('<%=taskname.ClientID%>').value; 
                        taskdetails.taskdescripton = document.getElementById('<%=taskdescripton.ClientID%>').value; 
                        taskdetails.owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        taskdetails.Startdate = document.getElementById('<%=Startdate.ClientID%>').value; 
                        taskdetails.Targetdate = document.getElementById('<%=Targetdate.ClientID%>').value;
                        taskdetails.StartTime = document.getElementById('<%=StartTime.ClientID%>').value; 
                        taskdetails.Timeline = document.getElementById('<%=Timeline.ClientID%>').value; 
                        taskdetails.TaskDuration  = document.getElementById('<%=taskDuration.ClientID%>').value; 
                        <%--taskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        taskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>
                        taskdetails.P_Status = statusid;
                        taskdetails.Priority = Prioritytypeid;
                        taskdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        taskdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;
                        taskdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 
                        taskdetails.ReminderType = document.getElementById('<%=ReminderType.ClientID%>').value; 
                        taskdetails.Days = document.getElementById('<%=Days.ClientID%>').value; 
                        taskdetails.Time = document.getElementById('<%=Time.ClientID%>').value; 
                        taskdetails.Notifyusers = document.getElementById('<%=hdnNotifyusers.ClientID%>').value; 
                        taskdetails.ChooseDate = document.getElementById('<%=ChooseDate.ClientID%>').value; 
                        
                        taskdetails.Occurrence = document.getElementById('<%=Occurrence.ClientID%>').value; 
                        taskdetails.tagto = document.getElementById('<%=tagto.ClientID%>').value; 
                        taskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value;
                        taskdetails.Stage = stageid;
                       taskdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value;
                   
                    if (document.getElementById('<%=issubtask.ClientID%>').checked == true)
                    {

                        taskdetails.issubtask = 1

                    }

                    if (document.getElementById('<%=issubtask.ClientID%>').checked == false)
                    {

                        taskdetails.issubtask = 0

                    }


                      if (document.getElementById('<%=checkTaskWeightage.ClientID%>').checked == true)
                       {

                        taskdetails.checkTaskWeightage = 1

                      }

                      if (document.getElementById('<%=checkTaskWeightage.ClientID%>').checked == false)
                      {

                        taskdetails.checkTaskWeightage = 0

                      }

                  
                        $.ajax({
                            type: "POST",
                            url: '/pms/makemilestone.aspx/SaveTask',
                            data: '{taskdetails: ' + JSON.stringify(taskdetails) + '}',
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
                    alert("Data Saved successfully."); 
                    // window.parent.document.getElementById("btnreload").click();
//window.parent.document.getElementById("btnclsmile").click();
window.parent.document.getElementById("hdneditid").value="1";
window.parent.document.getElementById("btncloseedit").click();
                        
                    
                }
        function showdivimgmilenew() {
            window.parent.document.getElementById("btnclosemile").click();

        }


 
    </script>
    <script>
 
function Binddetails(RecordId) {
    //alert(RecordId);
    debugger;

    $.ajax({
        url: '/pms/MultiProjectTask.aspx/Bindtaskdetail',
        data: "{ 'RecordId': '" + RecordId + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: OnSuccess1dataupdate,
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });

    // document.getElementById("btnreload").click();
}

function OnSuccess1dataupdate(response) {
          //  alert(response.d);
            debugger
            try {
                var Bindtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindtaskdetail.find("id").text() != "") {
debugger
document.getElementById('btnsave').innerHTML = "Update";
                    document.getElementById('<%=hdnid.ClientID%>').value = Bindtaskdetail.find("id").text();
                    
                    $("[id*=ProjectId]").val(Bindtaskdetail.find("ProjectId").text());
                     $("[id*=Milestoneid]").val(Bindtaskdetail.find("Milestoneid").text());
                    $("[id*=tasklist]").val(Bindtaskdetail.find("tasklist").text());
                    $("[id*=taskname]").val(Bindtaskdetail.find("taskname").text());
                    $("[id*=taskdescripton]").val(Bindtaskdetail.find("taskdescripton").text());
                    $("[id*=owner]").val(Bindtaskdetail.find("owner").text());
                    $("[id*=Startdate]").val(Bindtaskdetail.find("Startdate").text());
                    $("[id*=Targetdate]").val(Bindtaskdetail.find("Targetdate").text()); 
                    $("[id*=P_Status]").val(Bindtaskdetail.find("P_Status").text()); 
                    $("[id*=Priority]").val(Bindtaskdetail.find("Priority").text()); 

                    $("[id*=Budget]").val(Bindtaskdetail.find("Budget").text()); 
                    $("[id*=Actual]").val(Bindtaskdetail.find("Actual").text()); 

                    $("[id*=Reminder]").val(Bindtaskdetail.find("Reminder").text()); 
                    $("[id*=Occurrence]").val(Bindtaskdetail.find("Occurrence").text()); 
                    $("[id*=tagto]").val(Bindtaskdetail.find("tagto").text()); 
                    $("[id*=team]").val(Bindtaskdetail.find("team").text()); 

                    $("[id*=ReminderType]").val(Bindtaskdetail.find("ReminderType").text()); 
                    $("[id*=Days]").val(Bindtaskdetail.find("Days").text()); 
                    $("[id*=Time]").val(Bindtaskdetail.find("team").text()); 
                    $("[id*=Notifyusers]").val(Bindtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDate]").val(Bindtaskdetail.find("ChooseDate").text()); 
                    
try
{
if (Bindtaskdetail.find("Uploaddrawing").text() != "" && Bindtaskdetail.find("Uploaddrawing").text() != "0")
                    {
                        
                        //bindUploadedDrawings(Bindtaskdetail.find("Uploaddrawing").text());
var strfiles=Bindtaskdetail.find("Uploaddrawing").text();
var uploadData =strfiles; 
    var basePath = "/pms/TaskImages/";
    var previewContainer = document.getElementById("previewContainer");
    
    previewContainer.innerHTML = ""; // Clear previous content

    if (uploadData && uploadData !== "0") {
        var files = uploadData.split("^");

        files.forEach(function(file) {
            var filePath = basePath + file.trim();
            var fileName = file.split("/").pop();
            var ext = fileName.split('.').pop().toLowerCase();

            if (["jpg", "jpeg", "png", "gif", "bmp"].includes(ext)) {
                // It's an image
                var imgLink = document.createElement("a");
                imgLink.href = filePath;
                imgLink.target = "_blank";
                
                var img = document.createElement("img");
                img.src = filePath;
                img.style.maxWidth = "100px";
                img.style.margin = "5px";
                img.title = fileName;
                img.className = "preview-item";
                imgLink.appendChild(img);
                previewContainer.appendChild(imgLink);
            } else if (ext === "pdf") {
                // It's a PDF
                var pdfLink = document.createElement("a");
                pdfLink.href = filePath;
                pdfLink.target = "_blank";
                pdfLink.title = fileName;
                pdfLink.style.display = "inline-block";
                pdfLink.style.margin = "5px";
                pdfLink.style.textDecoration = "none";
                pdfLink.style.fontSize = "40px";

                // Option 1: Emoji icon
                pdfLink.textContent = "📄";
pdfLink.className = "preview-item";
                previewContainer.appendChild(pdfLink);
            }
        });
    }
                    }
}
catch
{}
                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Bindtaskdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist.ClientID%>').value = Bindtaskdetail.find("tasklist").text();
                    document.getElementById("divproject").style.display = "none";
                    document.getElementById("divmilestone").style.display = "none";
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = Bindtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = Bindtaskdetail.find("Targetdate").text();

                    document.getElementById('<%=hdnstarttime.ClientID%>').value = Bindtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = Bindtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDuration.ClientID%>').value = Bindtaskdetail.find("TaskDuration").text();

                     document.getElementById('<%=P_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                     document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";


                    document.getElementById('<%=Priority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindtaskdetail.find("team").text();

                     document.getElementById('<%=Stage.ClientID%>').value = Bindtaskdetail.find("Stage").text();
                    document.getElementById('<%=hdnstage.ClientID%>').value = Bindtaskdetail.find("Stage").text();
                      document.getElementById('<%=txtnewstage.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').style.display = "none";

                     document.getElementById('<%=Weightage.ClientID%>').value = Bindtaskdetail.find("Weightage").text();

                    document.getElementById('<%=hdnreminder.ClientID%>').value = Bindtaskdetail.find("Reminder").text();
                    document.getElementById('<%=hdnReminderType.ClientID%>').value = Bindtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=hdnTime.ClientID%>').value = Bindtaskdetail.find("Time").text();
                    document.getElementById('<%=hdnNotifyusers.ClientID%>').value = Bindtaskdetail.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDate.ClientID%>').value = Bindtaskdetail.find("ChooseDate").text();
                

                  if (Bindtaskdetail.find("issubtask").text() == 1)
                  {
                      document.getElementById('<%=issubtask.ClientID%>').checked = true;

                  }

                  if (Bindtaskdetail.find("issubtask").text() == 0)
                  {
                      document.getElementById('<%=issubtask.ClientID%>').checked = false;

                 }

                  if (Bindtaskdetail.find("checkTaskWeightage").text() == 1)
                  {
                      document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = true;

                  }

                  if (Bindtaskdetail.find("checkTaskWeightage").text() == 0)
                  {
                      document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = false;

                  }
                  debugger
           document.getElementById('<%=Button1.ClientID%>').click();

                    // time and reminder fill and edit strt//

                     if (Bindtaskdetail.find("Reminder").text() == "0")
                     {
                       document.getElementById("divremindertype").style.display = "none";
                       document.getElementById("divDays").style.display = "none";
                       document.getElementById("Divtime").style.display = "none";
                       document.getElementById("divNotifyusers").style.display = "none";
                       document.getElementById("divChooseDate").style.display = "none";

                    }
                    else if (document.getElementById('<%=Reminder.ClientID%>').value == "1")
                        {
                                document.getElementById("divremindertype").style.display = "block";
                                document.getElementById("Divtime").style.display = "block";
                                document.getElementById("divNotifyusers").style.display = "block";
                                document.getElementById("divChooseDate").style.display = "none";

                          }

                       else if (document.getElementById('<%=Reminder.ClientID%>').value == "2")
                        {
                                document.getElementById("divremindertype").style.display = "none";
                                document.getElementById("divChooseDate").style.display = "block";
                                document.getElementById("Divtime").style.display = "block";
                                document.getElementById("divNotifyusers").style.display = "block";
                                document.getElementById("divDays").style.display = "none";

                          }
                   if (document.getElementById('<%=ReminderType.ClientID%>').value == "2")
    {
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divDays").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "1" && document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divChooseDate").style.display = "none";
            document.getElementById("divDays").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }

                     //  Milestone Timeline //
              debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: '/pms/MultiProjectTask.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

     
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }



             //  Task Timeline //
             debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
        
                debugger;
                  $.ajax({
                      url: '/pms/MultiProjectTask.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }

 if (Bindtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
                  else
                  {

                   var idsemp = Bindtaskdetail.find("owner").text();
                  $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
             
             }



             
             if (Bindtaskdetail.find("team").text() != "")
             {
                 debugger
                 if (Bindtaskdetail.find("team").text().includes(','))
                 {
                      var idsemp = Bindtaskdetail.find("team").text().split(',');
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = Bindtaskdetail.find("team").text();
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 

            }

                    
                    // time and reminder fill and edit end//


             if (Bindtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }


             
            


                     document.getElementById('<%=Button1.ClientID%>').click();
                    
                   
                }
            }
            catch
                {
                }
            
            }

        
Binddetails('<%=HttpContext.Current.Request.QueryString["id"].ToString()%>');
    </script>
    <script>
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }

</script>
    <script>
  function GetReminderDtls()
  {
debugger
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }


    if (document.getElementById('<%=Reminder.ClientID%>').value == "1")
    {
            document.getElementById("divremindertype").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            document.getElementById("divChooseDate").style.display = "none";

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "2")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            document.getElementById("divDays").style.display = "none";

      }


  } 

</script>

<script>
  function GetRemindertypeDtls()
  {
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "2")
    {
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divDays").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "1" && document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divChooseDate").style.display = "none";
            document.getElementById("divDays").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
  } 

</script>
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
                url: "FileUploadHandlertask.ashx?id="+id+"", 
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
function ShowPreviewnew() {
    debugger;

    var input = document.getElementById("<%= fileupload.ClientID %>");
    var previewContainer = document.getElementById("previewContainer");
    previewContainer.innerHTML = ''; // Clear previous previews

    if (input.files && input.files.length > 0) {
        let isAllValid = true;

        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];

            var isImage = file.type.startsWith('image/');
            var isPDF = file.type === 'application/pdf';

            if (!isImage && !isPDF) {
                alert("Only image or PDF files are allowed.");
                isAllValid = false;
                break;
            }

            if (isImage && file.size > 25000) {
                alert("Image '" + file.name + "' exceeds 25KB.");
                isAllValid = false;
                break;
            }

            if (isPDF && file.size > 1048576) {
                alert("PDF '" + file.name + "' exceeds 1MB.");
                isAllValid = false;
                break;
            }
        }

         if (!isAllValid) {
            input.value = ""; // Clear selected files
            previewContainer.innerHTML = ""; // Clear previews
            return;
        }

       for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];
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
                        previewContainer.appendChild(img);
                    } else if (theFile.type === 'application/pdf') {
                        var icon = document.createElement("div");
                        icon.textContent = "📄"; // PDF emoji
                        icon.style.fontSize = "40px";
                        icon.style.margin = "5px";
                        icon.title = theFile.name;
                        icon.className = "preview-item";
                        previewContainer.appendChild(icon);
                    }
                };
            })(file);

            reader.readAsDataURL(file);
        }
    }
}

function bindUploadedDrawings(strfiles) {
    var uploadData =strfiles; 
    var basePath = "/pms/TaskImages/";
    var previewContainer = document.getElementById("previewContainer");
    
    previewContainer.innerHTML = ""; // Clear previous content

    if (uploadData && uploadData !== "0") {
        var files = uploadData.split("^");

        files.forEach(function(file) {
            var filePath = basePath + file.trim();
            var fileName = file.split("/").pop();
            var ext = fileName.split('.').pop().toLowerCase();

            if (["jpg", "jpeg", "png", "gif", "bmp"].includes(ext)) {
                // It's an image
                var imgLink = document.createElement("a");
                imgLink.href = filePath;
                imgLink.target = "_blank";
                
                var img = document.createElement("img");
                img.src = filePath;
                img.style.maxWidth = "100px";
                img.style.margin = "5px";
                img.title = fileName;
                img.className = "preview-item";
                imgLink.appendChild(img);
                previewContainer.appendChild(imgLink);
            } else if (ext === "pdf") {
                // It's a PDF
                var pdfLink = document.createElement("a");
                pdfLink.href = filePath;
                pdfLink.target = "_blank";
                pdfLink.title = fileName;
                pdfLink.style.display = "inline-block";
                pdfLink.style.margin = "5px";
                pdfLink.style.textDecoration = "none";
                pdfLink.style.fontSize = "40px";

                // Option 1: Emoji icon
                pdfLink.textContent = "📄";
pdfLink.className = "preview-item";
                previewContainer.appendChild(pdfLink);
            }
        });
    }
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
                   input.value = '';
                    return;
                }
            }  
        }  
</script>
    <script>
    function checkdate(enterdate)
    {
        if (document.getElementById("<%=Startdate.ClientID%>").value != "" && document.getElementById("<%=Targetdate.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=Startdate.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=Targetdate.ClientID%>").value);
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
    function checkdatecomparisiontaskdate(enterdate) {
        debugger
        if (enterdate != "") {

            if (checkdate(enterdate)) {
                var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

               var milesto_nid = document.getElementById("<%=Milestoneid.ClientID%>").value;



                $.ajax({
                    url: 'Add_Task.aspx/Bindcheckstrtdatemiledetails',
                    data: "{ 'milesto_nid': '" + milesto_nid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var taskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (taskstrtdatechkdtls.find("id").text() != "")
                        {
                            const today = new Date(taskstrtdatechkdtls.find("StartDate").text());
                            var fdt = new Date(today);
                            const today2 = new Date(taskstrtdatechkdtls.find("EndDate").text());
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

