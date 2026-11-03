<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="Addnewsubtask - Copy.aspx.cs" Inherits="Addnewsubtask" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 49px);">
            <div class="acitivity-timeline p-3">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
    <div class="row">
                                 <div style="display: none">
                                <asp:Button ID="Button2" runat="server" Text="" OnClick="Button1_Click" />
                                     <asp:Button ID="Button3" runat="server" Text="" OnClick="btnload_Click" />
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                   <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                 <asp:HiddenField ID="hdnmilstoneid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntasklistid" runat="server" Value="0" />
                                <asp:HiddenField ID="hdntaskid" runat="server" Value="0" />

                                   <div class="col-xxl-12 col-lg-12" id="divproject">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel9" runat="server">
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
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="Milestoneid_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
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
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="tasklist_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklist" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                               <div class="col-xxl-12 col-lg-12" id="divtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="task" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task ..." AllowSingleDeselect="true" AutoPostBack="true" onchange="GetWeitagebindDtls()" OnSelectedIndexChanged="task_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntask" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnweightage" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task Name", "2")%></label>
                                    </div>
                                    <input type="text" id="Subtaskname" runat="server" placeholder="Sub Task Name" class="form-control">
                                </div>


                          

                                <div class="col-xxl-12 col-lg-12" id="divtasktimeline" style="display:none;">
                               <asp:Label ID="lbltasktimeline" runat="server"></asp:Label>
                              </div>

                              <div class="col-xxl-12 col-lg-12" id="divsubtasktimeline" style="display:none;">
                            <asp:Label ID="lblsubtasktimeline" runat="server"></asp:Label>
                           </div>


                               
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task Description", "2")%></label>
                                    </div>
                                    <textarea id="SubTaskDescription" runat="server" placeholder="Sub Task Description" class="form-control"></textarea>
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
                                            
                                     
                                        </label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                     <input type="date" id="Startdate" runat="server" class="form-control"  onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdnStartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;
                                            
                                         
                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="StartTime" runat="server" class="form-control"  onclick="this.showPicker()">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;
                                            
                                          

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                          <input type="date" id="Targetdate" runat="server" class="form-control"  onclick="this.showPicker()" onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdnTargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                 <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;
                                            
                                        

                                        </label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="Timeline" runat="server" class="form-control"  onclick="this.showPicker()">
                                             <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                     <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour"  class="form-control">
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
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnP_Status" runat="server" Value="0" />
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewprojectstatus" style="display:none">
                                          <input type="text" id="txtnewprojectstatus" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="HiddenField3" runat="server" Value="0" />
                                      

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
                                                    DataPlaceHolder="Priority ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
                                         </cc1:DropDownListChosen>
                                              <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                              </ContentTemplate>
                                        </asp:UpdatePanel>
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="HiddenField4" runat="server" Value="0" />
                                          

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
                                    
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
                                
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
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
                                
                                     <asp:UpdatePanel ID="UpdatePanel12" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="ChooseDate" runat="server" class="form-control"  onclick="this.showPicker()" >
                                               <asp:HiddenField ID="hdnChooseDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                    <div class="col-xxl-12 col-lg-12" id="Divtime">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Time", "2")%></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" class="input-group">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="time" id="Time" runat="server" class="form-control"  onclick="this.showPicker()" >
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
                                   

                                    <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>--%>
                                         <%--<ContentTemplate>--%>
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                          <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>


                                </div>  
                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
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
                             <asp:Button ID="btnuploadimg" runat="server" Text="" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:FileUpload ID="Uploaddrawing" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreviewnew();" multiple />
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
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                           </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimgmilenew()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclosemile" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>
   <script src="/pms/assets/js/select2.min.js"></script> 
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css">
    <script>
  function GetReminderDtlssubtask()
  {
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
       
GetReminderDtlssubtask();
                    //GetRemindertypeDtlssubtask();
</script>
     
    <script>
    debugger
    var ddLNotifyuserssubtask = $(document.getElementById("<%=Notifyusers.ClientID%>"));
   

    $(ddLNotifyuserssubtask).select2({
        data: [
    <%=Multiuserssubtask%>
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
    debugger
    var Ownermile1 = $(document.getElementById("<%=owner.ClientID%>"));
    var teammile1 = $(document.getElementById("<%=team.ClientID%>"));
   
     $(Ownermile1).select2({
        data: [
    <%=Multiowner%>
        ],
        multiple: true,
        placeholder: "Task Owner...",
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
  
   
    </script>
    <script type="text/javascript">
  function AddMultisubtask(Projectid,Milestoneid,task_id) {
      //alert(Projectid);
      //alert(Milestoneid);
      //alert(task_id);
      debugger;
      document.getElementById("<%=hdnProjectId.ClientID%>").value = Projectid;
      document.getElementById("<%=hdnMilestoneid.ClientID%>").value = Milestoneid;
      document.getElementById("<%=hdntask.ClientID%>").value = task_id;
      document.getElementById('<%=Button1.ClientID%>').click();

      
    
            document.getElementById("divprojectsubtask").style.display = "none";
            document.getElementById("divmilestonesubtask").style.display = "none";
           document.getElementById("divtasksubtask").style.display = "none";
      
         var RecordId=document.getElementById("<%=hdntask.ClientID%>").value;
         
         $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_subtask',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise_autosub,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });




              function OnSuccessweitagedtlschkpagewise_autosub(response) {
            
            debugger
            try {
                var waitagechkdtlssubtask = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtlssubtask.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagesubtask").style.display = "none";

                     }

                     if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagesubtask").style.display = "block";

                     }
                    
                }
            }
            catch

            {

             }



      //Owner Bind Task Wise;

        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_newsubtask',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwnersubtask = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwnersubtask).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

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



       //Team Bind Task Wise;
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_newsubtask',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteamsubtask = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteamsubtask).select2({
                                                             data: data,
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



             //  Task Timeline //
            debugger
            document.getElementById("divtasktimelinesub").style.display = "block";
             var tski_d = document.getElementById("<%=hdntask.ClientID%>").value;
             //alert(projec_id);
               debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata_subtask',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline_atofl,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline_atofl(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                    lbl.innerHTML = todolist;
           
           
        }

            //Sub Task Timeline //
             debugger
             document.getElementById("divsubtasktimeline").style.display = "block";
             var tski_d = document.getElementById("<%=hdntask.ClientID%>").value;
             //alert(projec_id);
              debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindSubtasktimelinedatasubtask',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssubtask_timeline_auto,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesssubtask_timeline_auto(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblsubtasktimeline.ClientID%>");
                    lbl.innerHTML = todolist;
           
           
        }

   

        }

    </script>
    <script>


     function GetOwnerempdetails_subtask() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_newsubtask',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwnersubtask = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwnersubtask).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

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





     function GetTeamempdetails_subtask() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_newsubtask',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteamsubtask = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteamsubtask).select2({
                                                             data: data,
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

   
        function GetWeitagebindDtlssubtask() {


                  var RecordId=document.getElementById("<%=task.ClientID%>").value;

                  $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_subtask',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschk_subtask,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschk_subtask(response) {
            
            debugger
            try {
                var waitagechkdtlssubtask = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtlssubtask.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtlssubtask.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }

            GetOwnerempdetails_subtask();
            GetTeamempdetails_subtask();

             document.getElementById("divtasktimeline").style.display = "block";
            Bindtasktimeline_datasubtask();
            document.getElementById("divsubtasktimeline").style.display = "block";
            BindSubtasktimeline_data();


         }


       
    }
    

</script>
    <script>
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
                url: "FileUploadHandlersubtask.ashx?id="+id+"", 
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
    </script>
    <div style="display: none">
                                <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />
                                     <asp:Button ID="btnload" runat="server" Text="" OnClick="btnload_Click" />
                                 </div>
    <script>
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


                         //status save
                        
                        var statusid = "0";
                        if (document.getElementById('<%=hdnP_Status.ClientID%>').value != "" && document.getElementById('<%=hdnP_Status.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "AddSubtask.aspx/statussave",
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
                            <%--statusid = document.getElementById('<%=P_Status.ClientID%>').value;--%>
                        }

                        // Priority type save

                         var Prioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "AddSubtask.aspx/AddPrioritytype",
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
                            url: "AddSubtask.aspx/AddStage",
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
                        
                        var subtaskdetails = {};
                        subtaskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value;  
                        subtaskdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        subtaskdetails.Milestoneid = document.getElementById('<%=Milestoneid.ClientID%>').value; 
                        subtaskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        subtaskdetails.task = document.getElementById('<%=task.ClientID%>').value; 
                        subtaskdetails.Subtaskname = document.getElementById('<%=Subtaskname.ClientID%>').value; 
                        subtaskdetails.SubTaskDescription = document.getElementById('<%=SubTaskDescription.ClientID%>').value; 
                        subtaskdetails.owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        subtaskdetails.Startdate = document.getElementById('<%=Startdate.ClientID%>').value; 
                       subtaskdetails.Targetdate = document.getElementById('<%=Targetdate.ClientID%>').value; 
                  

                        subtaskdetails.StartTime = document.getElementById('<%=StartTime.ClientID%>').value; 
                        subtaskdetails.Timeline = document.getElementById('<%=Timeline.ClientID%>').value; 
                        subtaskdetails.TaskDuration  = document.getElementById('<%=taskDuration.ClientID%>').value; 

                        <%--subtaskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        subtaskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>

                        subtaskdetails.P_Status = statusid;
                        subtaskdetails.Priority = Prioritytypeid;
                        subtaskdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        subtaskdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;

                        subtaskdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 

                        subtaskdetails.ReminderType = document.getElementById('<%=ReminderType.ClientID%>').value; 
                        subtaskdetails.Days = document.getElementById('<%=Days.ClientID%>').value; 
                        subtaskdetails.Time = document.getElementById('<%=Time.ClientID%>').value; 
                        subtaskdetails.Notifyusers = document.getElementById('<%=hdnNotifyusers.ClientID%>').value; 
                        subtaskdetails.ChooseDate = document.getElementById('<%=ChooseDate.ClientID%>').value; 

                        subtaskdetails.Occurrence = document.getElementById('<%=Occurrence.ClientID%>').value; 
                        subtaskdetails.tagto = document.getElementById('<%=tagto.ClientID%>').value; 
                       subtaskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                       subtaskdetails.Stage = stageid;
                       subtaskdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value; 
                       subtaskdetails.Uploaddrawing = document.getElementById('<%=hdnUploaddrawing.ClientID%>').value;
            
                        $.ajax({
                            type: "POST",
                            url: 'AddSubtask.aspx/SaveSubTask',
                            data: '{subtaskdetails: ' + JSON.stringify(subtaskdetails) + '}',
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
                    document.getElementById('<%=btnload.ClientID%>').click();



try
{
 document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/pms/assets/images/user.jpg";
                   
}
catch
{}

                     document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
                    document.getElementById('<%=hdntask.ClientID%>').value = "0";
                    document.getElementById('<%=Subtaskname.ClientID%>').value = "";
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                    $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=Startdate.ClientID%>').value ="";
                    document.getElementById('<%=Targetdate.ClientID%>').value = "";
                       document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "";
                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                     document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                  document.getElementById('<%=Reminder.ClientID%>').value = "";
                    document.getElementById('<%=Occurrence.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
                  document.getElementById('<%=tagto.ClientID%>').value = "";
                document.getElementById('<%=hdnteam.ClientID%>').value = "0";

                     document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";

                      $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
                
            
             $(document.getElementById("<%=team.ClientID%>")).val(0);
             $(document.getElementById("<%=team.ClientID%>")).change();

                  
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        BindSubtask_data();
                    }
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                       

                    }
                   

               
                    
                }



          function Cleardata() {
                        
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid]").val("0");
                    $("[id*=tasklist]").val("0");
                    $("[id*=task]").val("0");
                    $("[id*=Subtaskname]").val("");
                    $("[id*=SubTaskDescription]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                    $("[id*=Targetdate]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("0");

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                   document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "0";
              document.getElementById('<%=hdntask.ClientID%>').value = "0";

                      document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnStartdate.ClientID%>').value ="";
               document.getElementById('<%=hdnTargetdate.ClientID%>').value = "";
             

                 document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "";

                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
              document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                document.getElementById('<%=Budget.ClientID%>').value = "";
                document.getElementById('<%=Actual.ClientID%>').value = "";
              document.getElementById('<%=hdnteam.ClientID%>').value = "0";


               document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";
              document.getElementById('<%=Weightage.ClientID%>').value = "";

                $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
                
            
            $(document.getElementById("<%=team.ClientID%>")).val(0);
             $(document.getElementById("<%=team.ClientID%>")).change();
           

                   document.getElementById('<%=Button1.ClientID%>').click();
                   <%--document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Sub Task";--%>
         
                        
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
                url: "FileUploadHandlersubtask.ashx?id="+id+"", 
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
    debugger
    var input = document.getElementById("<%= fileupload.ClientID %>");
    var previewContainer = document.getElementById("previewContainer");
    previewContainer.innerHTML = ''; // Clear previous previews

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
                        previewContainer.appendChild(img);
                    } else if (theFile.type === 'application/pdf') {
                        // === OPTION 1: Show Emoji ===
                        var icon = document.createElement("div");
                        icon.textContent = "📄"; // PDF emoji
                        icon.style.fontSize = "40px";
                        icon.style.margin = "5px";
                        icon.title = theFile.name;
icon.className = "preview-item";
                        previewContainer.appendChild(icon);

                        // === OPTION 2: Show Image Icon ===
                        /*
                        var img = document.createElement("img");
                        img.src = "/images/pdf-icon.png"; // Adjust path as needed
                        img.style.width = "40px";
                        img.style.margin = "5px";
                        img.title = theFile.name;
                        previewContainer.appendChild(img);
                        */
                    }
                };
            })(file);

            reader.readAsDataURL(file);
        }
    }
}
function bindUploadedDrawings(strfiles) {
    var uploadData =strfiles; 
    var basePath = "/pms/SubtaskImages/";
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
Binddetails('<%=HttpContext.Current.Request.QueryString["id"].ToString()%>'); 
 function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'AddSubtask.aspx/Bindsubtaskdetail',
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
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                   
try
{
if (Bindsubtaskdetail.find("Uploaddrawing").text() != "" && Bindsubtaskdetail.find("Uploaddrawing").text() != "0")
                    {
                        
                        bindUploadedDrawings(Bindsubtaskdetail.find("Uploaddrawing").text());
                    }
}
catch
{}
                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindsubtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Bindsubtaskdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist.ClientID%>').value = Bindsubtaskdetail.find("tasklist").text();
                    document.getElementById('<%=hdntask.ClientID%>').value = Bindsubtaskdetail.find("task").text();
                    document.getElementById('<%=Subtaskname.ClientID%>').value = Bindsubtaskdetail.find("Subtaskname").text();
                    document.getElementById('<%=SubTaskDescription.ClientID%>').value = Bindsubtaskdetail.find("SubTaskDescription").text();
                    document.getElementById('<%=Budget.ClientID%>').value = Bindsubtaskdetail.find("Budget").text();
                    document.getElementById('<%=Actual.ClientID%>').value = Bindsubtaskdetail.find("Actual").text();
                    document.getElementById('<%=hdnreminder.ClientID%>').value = Bindsubtaskdetail.find("Reminder").text();
                    document.getElementById('<%=Occurrence.ClientID%>').value = Bindsubtaskdetail.find("Occurrence").text();
                    document.getElementById('<%=tagto.ClientID%>').value = Bindsubtaskdetail.find("tagto").text();
                    document.getElementById('<%=team.ClientID%>').value = Bindsubtaskdetail.find("team").text();
                    document.getElementById('<%=ReminderType.ClientID%>').value = Bindsubtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=Days.ClientID%>').value = Bindsubtaskdetail.find("Days").text();
                   
                    $("[id*=Notifyusers]").val(Bindsubtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDate]").val(Bindsubtaskdetail.find("ChooseDate").text()); 
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindsubtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnStartdate.ClientID%>').value = Bindsubtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdnTargetdate.ClientID%>').value = Bindsubtaskdetail.find("Targetdate").text();
                    
                    document.getElementById('<%=hdnstarttime.ClientID%>').value = Bindsubtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = Bindsubtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDuration.ClientID%>').value = Bindsubtaskdetail.find("TaskDuration").text();

                    document.getElementById('<%=P_Status.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindsubtaskdetail.find("P_Status").text();
                     document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";


                    document.getElementById('<%=Priority.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindsubtaskdetail.find("Priority").text();
                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindsubtaskdetail.find("team").text();


                    document.getElementById('<%=Stage.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
                    document.getElementById('<%=hdnstage.ClientID%>').value = Bindsubtaskdetail.find("Stage").text();
                    document.getElementById('<%=txtnewstage.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=Weightage.ClientID%>').value = Bindsubtaskdetail.find("Weightage").text();

                     document.getElementById('<%=hdnReminderType.ClientID%>').value = Bindsubtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=hdnTime.ClientID%>').value = Bindsubtaskdetail.find("Time").text();
                    document.getElementById('<%=hdnNotifyusers.ClientID%>').value = Bindsubtaskdetail.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDate.ClientID%>').value = Bindsubtaskdetail.find("ChooseDate").text();


                     // time and reminder fill and edit strt//

                     if (Bindsubtaskdetail.find("Reminder").text() == "0")
                     {
                       document.getElementById("divremindertype").style.display = "none";
                       document.getElementById("divDays").style.display = "none";
                       document.getElementById("Divtime").style.display = "none";
                       document.getElementById("divNotifyusers").style.display = "none";
                       document.getElementById("divChooseDate").style.display = "none";

                    }


             // Bindtasktimeline_data();
            //  Task Timeline //
            debugger
            document.getElementById("divtasktimeline").style.display = "block";
             var tski_d = Bindsubtaskdetail.find("task").text();
             //alert(tski_d);
               debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
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
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }


              // BindSubtasktimeline_data();
             //Sub Task Timeline //
             debugger
             document.getElementById("divsubtasktimeline").style.display = "block";
             var tski_d = Bindsubtaskdetail.find("task").text();
             //alert(tski_d);
              debugger;
                  $.ajax({
                      url: 'AddSubtask.aspx/bindSubtasktimelinedata',
                    data: "{ 'tski_d': '" + tski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesssubtask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesssubtask_timeline(response)
        {
            debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblsubtasktimeline.ClientID%>");
                    lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }





                    GetReminderDtls();
                    GetRemindertypeDtls();
                    // time and reminder fill and edit end//


             if (Bindsubtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindsubtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }
           document.getElementById('<%=Button1.ClientID%>').click();

           

             GetOwnerempdetails();
             if (Bindsubtaskdetail.find("owner").text() != "")
              {
                  debugger
                  if (Bindsubtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
                  else
                  {

                   var idsemp = Bindsubtaskdetail.find("owner").text();
                  $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
             
             }



              GetTeamempdetails();
             if (Bindsubtaskdetail.find("team").text() != "")
             {
                 debugger
                 if (Bindsubtaskdetail.find("team").text().includes(','))
                 {
                      var idsemp = Bindsubtaskdetail.find("team").text().split(',');
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = Bindsubtaskdetail.find("team").text();
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 

            }




                    
                    document.getElementById('btnsave').value = "Update";
                   
                }
            }
            catch
                {
                }

            }
</script>
    <script>


     function GetOwnerempdetails() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiOwner_new',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwner = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

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





     function GetTeamempdetails() {
        debugger
        var tas_kid = document.getElementById("<%=hdntask.ClientID%>").value;
       if (tas_kid == "" || tas_kid == "0") {
           tas_kid = document.getElementById("<%=task.ClientID%>").value;
       }
        $.ajax({
            url: 'AddSubtask.aspx/bindMultiTeam_new',
            data: "{'tas_kid': '" + tas_kid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteam = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteam).select2({
                                                             data: data,
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

   
    function GetWeitagebindDtls() {

                  var RecordId=document.getElementById("<%=task.ClientID%>").value;

                  $.ajax({
                    url: 'AddSubtask.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschk,
                    error: function (response) {
                    alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschk(response) {
            
            debugger
            try {
                var waitagechkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtls.find("checkTaskWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkTaskWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }

            GetOwnerempdetails();
            GetTeamempdetails();

             document.getElementById("divtasktimeline").style.display = "block";
            Bindtasktimeline_data();
            document.getElementById("divsubtasktimeline").style.display = "block";
            BindSubtasktimeline_data();


         }


       
    }
    

</script>
    <script>
  function GetReminderDtls()
  {
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
    <asp:HiddenField ID="hdnsavetype" runat="server" />
    
     <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />

    
    
</asp:Content>

